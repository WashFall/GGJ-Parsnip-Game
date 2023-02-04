using Pathfinding;
using UnityEngine;

public class FarmerBehaviour : MonoBehaviour
{
    public Transform[] patrolSpots;
    public Transform playerPosition;

    Transform target;
    FOV fov;
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
        fov = GetComponent<FOV>();

        setDestination.target = SelectNewRandomSpot();

        Attack.explosion += () => { explosionHeard = true; };
    }

    private void Update()
    {
        if (!playerCaught && !fov.TargetInView(target)) { Patrol(); }
    }

    Transform SelectNewRandomSpot()
    {
        playerCaught = false;

        pathFinder.maxSpeed = patrolSpeed;
        int newRandomDestination = Random.Range(0, patrolSpots.Length);

        while (randomDestinationSpot == newRandomDestination) { newRandomDestination = Random.Range(0, patrolSpots.Length); }

        randomDestinationSpot = newRandomDestination;

        return patrolSpots[randomDestinationSpot];
    }

    void Patrol()
    {
        if(fov.TargetInView(target))
        {
            pathFinder.maxSpeed = seekSpeed;
            setDestination.target = target;

            if(Vector3.Distance(transform.position, target.position) < fov.innerRadius) { CatchTarget(); }
        }

        if (!explosionHeard && !fov.TargetInView(target))
        {
            if (Vector3.Distance(transform.position, setDestination.target.position) < closeDistanceToTarget)
            {
                setDestination.target = SelectNewRandomSpot();
                Debug.Log("Hej");
            }
        }

        if (explosionHeard && !fov.TargetInView(target))
        {
            target = playerPosition.GetComponent<Attack>().currentExplosionSite;

            pathFinder.maxSpeed = seekSpeed;
            setDestination.target = target;

            if (Vector3.Distance(transform.position, target.position) > closeDistanceToTarget)
            {
                //TODO: Add question mark above head and fix CoRoutine
                explosionHeard = false;
                Invoke(nameof(Patrol), 2);
            }
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
