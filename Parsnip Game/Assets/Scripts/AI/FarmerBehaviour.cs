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
    float patrolSpeed;
    float seekSpeed;

    int randomDestinationSpot;

    bool playerCaught;

    void Start()
    {
        setDestination = GetComponent<AIDestinationSetter>();

        Patrol();
    }

    Transform SelectNewRandomSpot()
    {
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
        playerCaught = false;
        setDestination.target = SelectNewRandomSpot();

        if (false/*sound of explosion is heard*/) 
        {
            target.position = playerPosition.position;

            pathFinder.maxSpeed = seekSpeed;
            setDestination.target = target;
            
            if(Vector2.Distance(transform.position, target.position) > closeDistanceToTarget)
            {
                //TODO: Add question mark above head
                Invoke(nameof(Patrol), 2);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, patrolSpots[randomDestinationSpot].position) < closeDistanceToTarget)
            {
                Patrol();
            }
        }

    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
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
