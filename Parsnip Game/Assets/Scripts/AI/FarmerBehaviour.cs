using Pathfinding;
using UnityEngine;

public class FarmerBehaviour : MonoBehaviour
{
    public Transform[] patrolSpots;
    public Transform playerPosition;

    Transform target;
    AIPath pathFinder;
    AIDestinationSetter setDestination;

    [SerializeField] float closeDistanceToTarget;
    [SerializeField] float patrolSpeed;
    [SerializeField] float seekSpeed;

    int randomDestinationSpot;

    bool playerCaught, explosionHeard;

    void Start()
    {
        pathFinder = GetComponent<AIPath>();
        setDestination = GetComponent<AIDestinationSetter>();

        setDestination.target = SelectNewRandomSpot();

        Attack.explosion += () => { explosionHeard = true; };
    }

    private void Update()
    {
        if (!playerCaught) { Patrol(); }
    }

    Transform SelectNewRandomSpot()
    {
        playerCaught = false;

        pathFinder.maxSpeed = patrolSpeed;
        int newRandomDestination = Random.Range(0, patrolSpots.Length);

        while (randomDestinationSpot == newRandomDestination) { newRandomDestination = Random.Range(0, patrolSpots.Length); }

        randomDestinationSpot = newRandomDestination;

        return patrolSpots[randomDestinationSpot];
        //IF sound is heard
        //Move faster towards sound(target)
        //IF target is reached (and no player is found)
        //=> SearchForTarget()
        //ELSE IF target is reached (and colliding with player catch area)
        //CatchTarget()
    }

    void Patrol()
    {
        if (!explosionHeard)
        {
            if (Vector3.Distance(transform.position, setDestination.target.position) < closeDistanceToTarget)
            {
                setDestination.target = SelectNewRandomSpot();
                Debug.Log("Hej");
            }
        }
        //TODO: Make below event
        if (explosionHeard)
        {
            target = playerPosition.GetComponent<Attack>().currentExplosionSite;

            pathFinder.maxSpeed = seekSpeed;
            setDestination.target = target;

            if (Vector3.Distance(transform.position, target.position) > closeDistanceToTarget)
            {
                //TODO: Add question mark above head
                explosionHeard = false;
                Invoke(nameof(Patrol), 2);
            }
        }


    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            CatchTarget();
        }
    }

    void CatchTarget()
    {
        playerCaught = true;

        //Do some animation
        //Player lose life
        //Carry player off screen => OnComplete Patrol();
        //Spawn new player
    }
}
