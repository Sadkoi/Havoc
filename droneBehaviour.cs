using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droneBehaviour : MonoBehaviour
{
    public List<GameObject> springs;
    public Rigidbody rb;
    public GameObject prop;
    public GameObject CM;
    public Joystick joystick;
    public static bool isDroneActive = false;

    private bool startDrone;
    private bool isStartup;
    private float thrust = 300f;
    private float currentThrust = 0f;
    // Start is called before the first frame update
    void Start()
    {
        rb.centerOfMass = CM.transform.localPosition;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(CameraFollow.isDoorOpen && prop.transform.position.z < 826f && joystick.Vertical > 0f){
            transform.position += new Vector3(0f,0f,joystick.Vertical*0.01f);
        }else{
            if(prop.transform.position.z > 826f || isDroneActive){
                isDroneActive = true;
            }
        }
        if(isDroneActive){
            if(currentThrust < thrust){
                addThrust();
            }else{
                control_normal();
            }
        }
    }

    void control_normal(){
        rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * joystick.Vertical * 4000f, prop.transform.position);
        rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.up) * joystick.Horizontal * 300f);
        foreach(GameObject spring in springs){
            RaycastHit hit;
            Debug.DrawRay(spring.transform.position, transform.TransformDirection(Vector3.down), Color.red); 
            if(Physics.Raycast(spring.transform.position, transform.TransformDirection(Vector3.down), out hit , 3f)){
                rb.AddForceAtPosition((Time.deltaTime * transform.TransformDirection(Vector3.up) * Mathf.Pow(3f - hit.distance, 2) * thrust), spring.transform.position);
            }

            // anti - rollover feature
            Debug.DrawRay(spring.transform.position, transform.TransformDirection(Vector3.up), Color.green);
            if(Physics.Raycast(spring.transform.position, transform.TransformDirection(Vector3.up), 0.2f)){
                rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.forward) * 600f);
            }
        }
        rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.forward) * 20f);
    }

    void addThrust(){
        if(currentThrust <= thrust){
            currentThrust += 200 * Time.deltaTime;
            foreach(GameObject spring in springs){
                RaycastHit hit;
                if(Physics.Raycast(spring.transform.position,transform.TransformDirection(Vector3.down),out hit, 3f)){
                    rb.AddForceAtPosition((Time.deltaTime * transform.TransformDirection(Vector3.up) * Mathf.Pow(3f - hit.distance, 2) * currentThrust), spring.transform.position);
                }
            }
        }
    }
//826

    
}
