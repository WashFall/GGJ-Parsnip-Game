using Pathfinding;
using UnityEngine;

public class FarmerBehaviour : MonoBehaviour
{
    public GameObject player;
    
    public Transform[] patrolSpots;

    Transform target;
    FOV fov;
    AIPath pathFinder;
    AIDestinationSetter setDestination;

    [SerializeField] float patrolSpeed;
    [SerializeField] float seekSpeed;
    [SerializeField] float liftedHeight;

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
    }

    void Patrol()
    {
        if(fov.TargetInView(player.transform) || Vector3.Distance(transform.position, player.transform.position) < fov.outerRadius)
        {
            pathFinder.maxSpeed = seekSpeed;
            setDestination.target = player.transform;

            if(Vector3.Distance(transform.position, player.transform.position) < fov.innerRadius) { CatchTarget(); }
        }
        else if (explosionHeard && !fov.TargetInView(player.transform))
        {
            target = player.GetComponent<Attack>().currentExplosionSite;

            pathFinder.maxSpeed = seekSpeed;
            setDestination.target = target;

            if (Vector3.Distance(transform.position, target.position) > fov.innerRadius)
            {
                //TODO: Add question mark above head and fix CoRoutine
                explosionHeard = false;
                Invoke(nameof(Patrol), 2);
            }
        }
        else if (!explosionHeard && !fov.TargetInView(player.transform))
        {
            if (Vector3.Distance(transform.position, setDestination.target.position) < fov.innerRadius)
            {
                setDestination.target = SelectNewRandomSpot();
            }
        }
    }

    void CatchTarget()
    {
        CharacterMovement playerMovement = player.GetComponent<CharacterMovement>();
        
        playerCaught = true;
        playerMovement.canMove = false;

        player.transform.SetParent(transform, false);
        player.transform.position = new Vector3(transform.position.x, transform.position.y + liftedHeight, transform.position.z);
        //Do some animation
        //Player lose life
        //Carry player off screen => OnComplete Patrol();
        //Spawn new player
    }
}
