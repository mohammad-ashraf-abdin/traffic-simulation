using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    public Camera cam;
    public GameObject gg;
    public NavMeshAgent agent;
    public  bool stopInTraffic = false;

    public float range = 2;

    public bool followWayPoint = false;

    public GameObject[] wayPoint;

    public int waypointNumber = 0;

    void Start()
    {
        if (wayPoint.Length != 0)
            followWayPoint = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(followWayPoint && agent.isStopped)
        {
            StartCoroutine(FollowingWaypoint());
        }

        if(stopInTraffic)
        {
            StartCoroutine(StoppingNearCar());
        }
        //print(agent.pathStatus);
        //print("1");
        //print(agent.remainingDistance);
        //print("2");
        //print(agent.stoppingDistance);
        //print(agent.remainingDistance - agent.stoppingDistance);
        print(agent.destination);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            //print("Youreach");
            agent.isStopped = true;
            if (waypointNumber < wayPoint.Length-1)
                waypointNumber++;
            else
                waypointNumber = 0;
        }
        //print(agent.isStopped);
        if (stopInTraffic)
            agent.isStopped = true;
        if(!stopInTraffic && (agent.remainingDistance >= agent.stoppingDistance))
            agent.isStopped = false;
        //if (agent.isStopped)
        //    print("HI");

    }
    IEnumerator FollowingWaypoint()
    {
        yield return new WaitForEndOfFrame();
        agent.SetDestination(wayPoint[waypointNumber].transform.position);
    }
   IEnumerator StoppingNearCar()
    { 
                yield return new WaitForEndOfFrame();

        GameObject[] cars = GameObject.FindGameObjectsWithTag("car");
        GameObject tl = GameObject.FindGameObjectWithTag("TrafficLight");
        foreach (GameObject gameObject in cars)
        {
            if (Vector3.Distance(transform.position, gameObject.transform.position) <= range)
            {
                if (tl.GetComponent<TrafficLight>().red)
                    gameObject.GetComponent<PlayerMove>().stopInTraffic = true;
                if (tl.GetComponent<TrafficLight>().orange)
                    gameObject.GetComponent<PlayerMove>().stopInTraffic = false;
            }
            

        }
    }
    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
