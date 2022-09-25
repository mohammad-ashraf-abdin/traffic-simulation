using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public Camera cam;
    public Camera secCam;
    public bool creatingCar = false;
    public bool creatingObs = false;
    public bool carHolded = false;
    public bool carHolded2 = false;
    public GameObject newCar;
    public GameObject newObs;
    public NavMeshAgent agent;

    public GameObject holdedCar;
    public float motor, stear, breakForce;
    public Toggle autoBreak;
    public Slider speed;
    public Slider angularSpeed;
    public Slider accleration;
    public Slider Obs_Avoid;

    public bool secCamera = false;
    public WheelCollider rr, rl, fl, fr;

    public bool manualDrive = false;
  
    // Start is called before the first frame update
    void Start()
    {
        //secCam.enabled = false;
        //cam.enabled = true;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray;
            //if (secCamera &&carHolded)
            //{
            //    FindingCamera();
            //    ray = secCam.ScreenPointToRay(Input.mousePosition);
            //    secCam.enabled = true;
            //    cam.enabled = false;
            //}
            //else
            //{
            ray = cam.ScreenPointToRay(Input.mousePosition);
            //    secCam.enabled = false;
            //    cam.enabled = true;

            //}
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.tag == "car")
                {
                    carHolded = true;
                    carHolded2 = true;
                    holdedCar = hit.collider.gameObject;
                    autoBreak.enabled = holdedCar.GetComponent<NavMeshAgent>().autoBraking;
                    speed.value = holdedCar.GetComponent<NavMeshAgent>().speed;
                    angularSpeed.value = holdedCar.GetComponent<NavMeshAgent>().angularSpeed;
                    accleration.value = holdedCar.GetComponent<NavMeshAgent>().acceleration;
                    for (int i = 0; i < holdedCar.transform.childCount; i++)
                    {
                        if (holdedCar.transform.GetChild(i).gameObject.tag == "fl")
                            fl = holdedCar.transform.GetChild(i).gameObject.GetComponent<WheelCollider>();
                        else if (holdedCar.transform.GetChild(i).gameObject.tag == "fr")
                            fr = holdedCar.transform.GetChild(i).gameObject.GetComponent<WheelCollider>();
                        else if (holdedCar.transform.GetChild(i).gameObject.tag == "rr")
                            rr = holdedCar.transform.GetChild(i).gameObject.GetComponent<WheelCollider>();
                        else if (holdedCar.transform.GetChild(i).gameObject.tag == "rl")
                            rl = holdedCar.transform.GetChild(i).gameObject.GetComponent<WheelCollider>();

                    }
                }

                else if (hit.collider != null && hit.collider.tag == "ground")
                {
                    if (creatingCar)
                    {
                        GameObject d = Instantiate(newCar, new Vector3(hit.point.x, 0.0122752f, hit.point.z), Quaternion.identity);
                        holdedCar = d;

                        creatingCar = false;

                        carHolded = true;
                        carHolded2 = true;
                        autoBreak.enabled = holdedCar.GetComponent<NavMeshAgent>().autoBraking;
                        speed.value = holdedCar.GetComponent<NavMeshAgent>().speed;
                        angularSpeed.value = holdedCar.GetComponent<NavMeshAgent>().angularSpeed;
                        accleration.value = holdedCar.GetComponent<NavMeshAgent>().acceleration;
                    }
                    if (creatingObs)
                    {
                        Instantiate(newObs, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity);
                        creatingObs = false;

                    }

                    else if (carHolded && carHolded2)
                    {
                        print(holdedCar.name);
                        holdedCar.GetComponent<NavMeshAgent>().SetDestination(hit.point);
                        carHolded2 = false;
                        print("Holded");
                    }
                }

            }

        }
        if (Input.GetKey("z"))
            carHolded2 = true;
        if (carHolded && Input.GetKey("m"))
        {
            
                manualDrive = true;

        } 
        if (carHolded && Input.GetKey("n"))
        {
         
                manualDrive = false;

        }
        
        if (manualDrive)
        {

            float horiz = Input.GetAxisRaw("Horizontal");
            float vert = Input.GetAxisRaw("Vertical");
          
            rr.motorTorque = vert*motor;
            rl.motorTorque = vert*motor;

            fr.steerAngle = horiz*stear;
            fl.steerAngle = horiz*stear;
            if (Input.GetKey(KeyCode.Space))
            {
                rl.brakeTorque = breakForce;
                rr.brakeTorque = breakForce;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                rl.brakeTorque = 0;
                rr.brakeTorque = 0;
            }
            if (Input.GetAxis("Vertical") == 0)
            {
                rl.brakeTorque = breakForce;
                rr.brakeTorque = breakForce;
            }
            else
            {
                rl.brakeTorque = 0;
                rr.brakeTorque = 0;
            }
            //Vector3 dir = new Vector3(horiz, 0, vert);
            //holdedCar.transform.Translate(dir.normalized * Time.deltaTime * holdedCar.GetComponent<NavMeshAgent>().speed);
        }

    }
    public void carMakerEnable()
    {
        creatingCar = true;
    }
    public void obsticalMakerEnable()
    {
        creatingObs = true;
    }
    public void ChangeSpeed()
    {
        if (carHolded)
        {
            holdedCar.GetComponent<NavMeshAgent>().speed = speed.value;
        }
    }
    public void ChangeAngularSpeed()
    {
        if (carHolded)
        {
            holdedCar.GetComponent<NavMeshAgent>().angularSpeed = angularSpeed.value;
        }
    }
    public void ChangeAccleration()
    {
        if (carHolded)
        {
            holdedCar.GetComponent<NavMeshAgent>().acceleration = accleration.value;
        }
    }
    public void ChangeBreak()
    {
        if (carHolded)
        {
            holdedCar.GetComponent<NavMeshAgent>().autoBraking = autoBreak.isOn;
        }
    }
    public void LeaveCar()
    {
        carHolded = false;
        carHolded2 = false;
    }
    public void NewDestnationEnable()
    {
        carHolded2 = true;
    }
    //public void ObsAvoid()
    //{
    //    if(carHolded)
    //    {
    //        holdedCar.GetComponent<>
    //    }
    //}

    public void FindingCamera()
    {
        ///* secCam =*/ holdedCar.transform.Find("SecCam").GetComponent<Camera>();
        foreach (Transform child in holdedCar.transform)
        {
            if (child.tag == "SecCam")
                secCam = child.gameObject.GetComponent<Camera>();
        }
    }
}
