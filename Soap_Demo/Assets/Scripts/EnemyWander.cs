using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWander : MonoBehaviour
{
    private enum EnemyState
    { 
        Patrol,
        Chase
    }

    private EnemyState currentState;

    private GameObject player;

    [SerializeField]
    private float chaseRadius = 500;
    [SerializeField]
    private float patrolAreaHeight = 50;
    [SerializeField]
    private float patrolAreaWidth = 50;

    private Vector3 originPosition;

    public Vector3 destinationPos;
    private NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.Patrol;
        agent = gameObject.GetComponent<NavMeshAgent>();
        destinationPos = new Vector3(Random.Range(transform.position.x - patrolAreaHeight, transform.position.x + patrolAreaHeight), 0, Random.Range(transform.position.z - patrolAreaWidth, transform.position.z + patrolAreaWidth));
        originPosition = transform.position;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case EnemyState.Patrol: PatrolUpdate(); break;
            case EnemyState.Chase: ChaseUpdate(); break;
        }
    }


    void PatrolUpdate()
    {
        if(Vector3.Distance(transform.position, destinationPos) <= 10f)
        {
            destinationPos = new Vector3(Random.Range(originPosition.x - patrolAreaHeight, originPosition.x + patrolAreaHeight), 0, Random.Range(originPosition.z - patrolAreaWidth, originPosition.z + patrolAreaWidth));
        }
        if(Vector3.Distance(transform.position, player.transform.position) <= chaseRadius)
        {
            currentState = EnemyState.Chase;
        }

        agent.SetDestination(destinationPos);
    }

    void ChaseUpdate()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > chaseRadius)
        {
            currentState = EnemyState.Patrol;
        }

        agent.SetDestination(player.transform.position);
    }
}
