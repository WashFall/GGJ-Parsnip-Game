using System.Collections;
using Cinemachine;
using Pathfinding;
using UnityEngine;

public class FarmerBehaviour : MonoBehaviour
{
    public GameObject player;

    public CinemachineVirtualCamera virtualCamera;

    public ParticleSystem cloudParticleEffect;

    public Transform[] patrolSpots;
    public Transform pickedUpLocation;
    public Transform offScreenLocation;

    Transform target;
    FOV fov;
    AIPath pathFinder;
    AIDestinationSetter setDestination;
    CharacterMovement playerMovement;
    Rigidbody playerRb;
    PlaySoundMultiple playSound;

    [SerializeField] float patrolSpeed;
    [SerializeField] float seekSpeed;
    [SerializeField] float liftedHeight;

    int randomDestinationSpot;

    bool playerCaught, explosionHeard; 
    bool justNoticedPlayer = true;

    void Start()
    {
        pathFinder = GetComponent<AIPath>();
        setDestination = GetComponent<AIDestinationSetter>();
        fov = GetComponent<FOV>();
        playerMovement = player.GetComponent<CharacterMovement>();
        playerRb = player.GetComponent<Rigidbody>();
        playSound = GetComponent<PlaySoundMultiple>();

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
        if (fov.TargetInView(player.transform) || Vector3.Distance(transform.position, player.transform.position) < fov.outerRadius)
        {
            pathFinder.maxSpeed = seekSpeed;
            setDestination.target = player.transform;

            if (justNoticedPlayer)
            {
                FarmerSound("Farmer");
                justNoticedPlayer = false;
            }

            if (Vector3.Distance(transform.position, player.transform.position) < fov.innerRadius) { StartCoroutine(CatchTarget()); }
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
                FarmerSound("Farmer 3");

                Invoke(nameof(Patrol), 2);
            }
        }
        else if (!explosionHeard && !fov.TargetInView(player.transform))
        {
            if (Vector3.Distance(transform.position, setDestination.target.position) < fov.innerRadius)
            {
                setDestination.target = SelectNewRandomSpot();
                justNoticedPlayer = true;
            }
        }
    }

    IEnumerator CatchTarget()
    {
        //TODO:Player lose life
        pickedUpLocation.position = transform.position;
        
        virtualCamera.m_LookAt = pickedUpLocation;
        virtualCamera.m_Follow = pickedUpLocation;

        cloudParticleEffect.transform.position = new Vector3(pickedUpLocation.position.x, pickedUpLocation.position.y + 15, pickedUpLocation.position.z);
        cloudParticleEffect.Play();

        playerCaught = true;
        playerMovement.canMove = false;
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        playerRb.useGravity = false;

        player.transform.SetParent(transform);

        //yield return new WaitForSecondsRealtime(1f);
        player.transform.position = new Vector3(transform.position.x, transform.position.y + liftedHeight, transform.position.z);
        FarmerSound("Farmer 2");

        yield return new WaitForSecondsRealtime(2f);
        setDestination.target = offScreenLocation;

        yield return new WaitForSecondsRealtime(6f);
        StartCoroutine(ResetPlayer());
    }

    IEnumerator ResetPlayer()
    {
        cloudParticleEffect.Play();

        yield return new WaitForSecondsRealtime(1f);
        player.transform.SetParent(null);
        player.transform.position = pickedUpLocation.position;
        
        transform.position = new Vector3(pickedUpLocation.position.x - 40, pickedUpLocation.position.y, pickedUpLocation.position.z - 40);
        setDestination.target = SelectNewRandomSpot();

        virtualCamera.m_LookAt = player.transform;
        virtualCamera.m_Follow = player.transform;

        yield return new WaitForSecondsRealtime(1f);
        playerCaught = false;
        playerMovement.canMove = true;
        playerRb.useGravity = true;
    }

    private void FarmerSound(string soundName)
    {
        playSound.PlaySound(soundName);
    }
}
