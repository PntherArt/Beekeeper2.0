using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bee : MonoBehaviour
{
    public float beeMaxAcceleration = 1f;
    public float beeMaxVelocity = 1f;
    public float maxProduct = 0.05f;
    public float product = 0f;
    public float beeAvoidanceRadius = 0.5f;
    public float beeAvoidanceMaxAcceleration = 0.1f;
    public Beehive parentBhive;
    public DayCycle dayCycle;
    [Range(0, 1)] public float lessStrafeBias = 1f;
    [Range(0, 1)] public float ratioOfProductCollected = 0.1f;

    enum BeeState { Search, Return, Idle };
    Rigidbody rb;
    [SerializeField] private BeeState bState = BeeState.Search;

    // bees can only carry one product at one go.
    [SerializeField] public ProductObj heldProduct = null;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // start bee algorithm
        StartCoroutine("startSearch");
    }

    private void OnDrawGizmos()
    {
        // visualizing bee states on scene
        if (bState == BeeState.Search) Gizmos.color = Color.green;
        else if (bState == BeeState.Return) Gizmos.color = Color.yellow;
        else Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, beeAvoidanceRadius * 0.1f);
        Gizmos.DrawLine(transform.position, transform.position + rb.velocity * 0.1f);
        Vector3 facing = (transform.localToWorldMatrix * Vector3.forward);
        Gizmos.DrawLine(transform.position, transform.position + facing * 0.1f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * (1 - Vector3.Dot(transform.localToWorldMatrix * Vector3.forward, rb.velocity.normalized)) * 0.1f);
    }

    IEnumerator goTo(GameObject obj, GameObject lookObj, Vector3 offset, float targetDist = 0.5f)
    {
        Vector3 target = obj.transform.position + offset;
        Vector3 lookTarget = lookObj.transform.position;


        // move towards target
        Vector3 distVector = target - transform.position;
        float dist = distVector.magnitude;
        Vector3 lookVector = lookTarget - transform.position;
        int numLoops = 0; // keeping track of num of loops
        // Debug.Log(Mathf.Abs(dist - targetDist) + " " + obj);
        while (dist - targetDist > 0.1f && obj && lookObj && obj.activeSelf && lookObj.activeSelf)
        {
            // Debug.Log("moving towards" + target);

            // --- avoid bees ---
            // get bees within radius
            Collider[] nearBees = Physics.OverlapSphere(transform.position, beeAvoidanceRadius, 1 << 6, QueryTriggerInteraction.Collide);

            // move away from all bees within radius
            Vector3 away = Vector3.zero;
            for (int i = 0; i < nearBees.Length; i++)
            {
                Vector3 otherBeePos = nearBees[i].transform.position;
                away += (transform.position - otherBeePos).normalized;
            }

            // figure out how much bee is "strafing" (moving sideways, circling etc)
            float strafe = 1 - Vector3.Dot(transform.localToWorldMatrix * Vector3.forward, rb.velocity.normalized);
            if (strafe < 0)
            {
                strafe = -strafe + 1;
            }

            // move away from other bees
            rb.velocity = Vector3.MoveTowards(
                rb.velocity,
                (rb.velocity + away).normalized * beeMaxVelocity * Mathf.Clamp01(dist),
                (beeAvoidanceMaxAcceleration) * Time.deltaTime
            );

            // --- look and move towards target ---
            if (strafe > 0)
            {
                rb.velocity = Vector3.MoveTowards(
                    rb.velocity,
                    (rb.velocity + distVector.normalized).normalized * beeMaxVelocity * Mathf.Clamp01(dist), // slow down if close to target
                    (beeMaxAcceleration + strafe * 100 * lessStrafeBias) * Time.deltaTime // biased against strafing (not sure if this works?)
                );
            }

            // update vectors
            lookVector = lookTarget - transform.position;
            distVector = target - transform.position;

            // update look direction
            transform.rotation = Quaternion.LookRotation(
                Vector3.MoveTowards(
                    transform.localToWorldMatrix * Vector3.forward,
                    lookVector.normalized,
                    (beeMaxAcceleration + strafe * 2) * Time.deltaTime
                ),
                Vector3.up
            );

            // update distance
            dist = distVector.magnitude;

            yield return new WaitForSeconds(0.01f);
            numLoops++;

            // update target positions
            if (obj)
            {
                target = obj.transform.position + offset;
                lookTarget = lookObj.transform.position;
            }

        }


        if (!(obj && lookObj && obj.activeSelf && lookObj.activeSelf)) yield return null;

    }

    IEnumerator pollinate(Flower flower)
    {
        GameObject flowerObj = flower.gameObject;
        if (heldProduct == null)
        {
            heldProduct = flower.flowerInfo.product;
        }

        // move towards flower until close enough
        Vector3 offset = Random.insideUnitSphere * flower.flowerInfo.radius + new Vector3(0, flower.flowerInfo.height, 0);

        if (flowerObj)
        {
            yield return StartCoroutine(goTo(flowerObj, flowerObj, offset, 0f));
        }
        else
        {
            yield return null;
        }

        //pollinate
        if (flowerObj)
        {
            float nectarCollected = Mathf.Min(flower.flowerInfo.productPerDay * ratioOfProductCollected, flower.Nectar, maxProduct - product); // get as much pollen as possible
            product += nectarCollected;
            flower.Nectar -= nectarCollected; // remove collected pollen from flower
            // Debug.Log("collected " + nectarCollected + " of " + flower.flowerInfo.name);
        }
        else
        {
            yield return null;
        }
    }

    public IEnumerator startSearch()
    {
        bState = BeeState.Search;
        GameObject lastFlower = null;

        // searching loop
        do
        {
            if (FlowerManager.numFlowers != 0)
            {
                // find a flower
                GameObject flowerObject = FlowerManager.getRandomFlowerObject(flowerObject =>
                {
                    return (
                        (
                            heldProduct == null // either no product held yet,
                            || heldProduct == flowerObject.GetComponent<Flower>().flowerInfo.product) //  or holding the product of this flower
                        )
                        && Vector3.Distance(flowerObject.transform.position, transform.position) < parentBhive.interactionRadius // flower is within interaction radius
                        && flowerObject != lastFlower // not the latest flower visited
                        && flowerObject.GetComponent<Flower>().Nectar != 0; // the nectar of the flower is not 0
                });

                // if no flowers are left, return
                if (flowerObject == null) break;

                // go to flower and pollinate
                Flower flower = flowerObject.GetComponent<Flower>();
                if (flower.gameObject) // failsafe for if a flower is missing its object
                    yield return StartCoroutine(pollinate(flower));
                else
                    Debug.LogError("A flower is missing its information!");

                if (lastFlower != null)
                    lastFlower = flower.gameObject;

                yield return new WaitForSeconds(0.3f); // wait for a bit
            }
        } while (bState == BeeState.Search && product < maxProduct && FlowerManager.numFlowers != 0 && !dayCycle.isNight());

        StartCoroutine("startReturn");
    }

    IEnumerator startReturn()
    {
        bState = BeeState.Return;

        yield return StartCoroutine(goTo(parentBhive.gameObject, parentBhive.gameObject, new Vector3(0f, 1f, 0f), 0.5f));
        bState = BeeState.Idle;

        Debug.Log("arrived home! alerting beehive...");

        parentBhive.arrivedHome(this);
    }
}
