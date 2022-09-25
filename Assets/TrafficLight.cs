using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public bool red = false;
    public bool orange = true;
    public bool green = false;

    public GameObject redLight, orangeLight, greenLight;
    public float redTime = 20.0f, orangeTime = 2.0f, greenTime = 6.0f, plustime = 0.0f;
    private bool isCoroutineExecuting = false;

    public float range = 3;
    public float rangeToSlow = 7;
    private void Start()
    {
        if (red && !green)
            StartCoroutine(ChangeStaues(plustime, 2));
        if (green && !orange)
            StartCoroutine(ChangeStaues(plustime, 1));
    }
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(ChangeCarMode());
        if (green&& !orange)
            StartCoroutine(ChangeStaues(greenTime, 1));
        else if (red&& !green)
            StartCoroutine(ChangeStaues(redTime, 2));
        else if (orange&& !red &&!green)
            StartCoroutine(ChangeStaues(orangeTime, 3));


    }
    [SerializeField]
    IEnumerator ChangeStaues(float time,int x)
    {
        if (isCoroutineExecuting)
            yield break;
        isCoroutineExecuting = true;
        yield return new WaitForSeconds(time);
        if (x == 1)
        {
            greenLight.SetActive(false);
            redLight.SetActive(true);
            orangeLight.SetActive(false);
            orange = false;
            green = false;
            red = true;
        }
        else if(x==2)
        {
            greenLight.SetActive(false);
            redLight.SetActive(false);
            orangeLight.SetActive(true);
            red = false;
            orange = true;
            green = false;
        }
        else if(x==3)
        {
            orangeLight.SetActive(false);
            greenLight.SetActive(true);
            redLight.SetActive(false);
            orange = false;
            green = true;
            red = false;
        }
        isCoroutineExecuting = false;


    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeToSlow);
    }

    IEnumerator ChangeCarMode()
    {
        yield return new WaitForEndOfFrame();

        GameObject[] cars = GameObject.FindGameObjectsWithTag("car");
        foreach (GameObject gameObject in cars)
        {
            if(Vector3.Distance(transform.position,gameObject.transform.position)<=range)
            {
                if(red)
                gameObject.GetComponent<PlayerMove>().stopInTraffic = true;
                if(green)
                    gameObject.GetComponent<PlayerMove>().stopInTraffic = false;
            }
            else if (Vector3.Distance(transform.position, gameObject.transform.position) <= rangeToSlow)
            {
                if (red)
                    gameObject.GetComponent<PlayerMove>().agent.speed = 1.1f;
                if (green)
                    gameObject.GetComponent<PlayerMove>().agent.speed = 4.1f;
                if(orange)
                    gameObject.GetComponent<PlayerMove>().agent.speed = 3.1f;


            }

        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    print("HI11");
    //}
}
