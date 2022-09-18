using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigos : MonoBehaviour
{
    public GameObject havoc;
    public GameObject enemyDrone;
    public List<GameObject> springs;
    public GameObject prop;
    public Rigidbody rb;

    private float thrust = 300f;
    private float currentThrust = 0f;
    private bool creepFromAfar = true;

    private float health = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        addthrust();
        followDrone();
        attackDrone();
        updateHealth();
    }
    // responsible for hover movement, air suspension
    void addthrust(){
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
        
        //Anti Stuck-In-Stilts
        
        //Back-Left
        Debug.DrawRay(springs[0].transform.position, transform.TransformDirection(Vector3.back), Color.blue); 
        Debug.DrawRay(springs[0].transform.position, transform.TransformDirection(Vector3.left), Color.blue); 

        if(Physics.Raycast(springs[0].transform.position, transform.TransformDirection(Vector3.left), 0.2f)){
            rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.back) * 200f);
        }

        if(Physics.Raycast(springs[0].transform.position, transform.TransformDirection(Vector3.back), 0.2f)){
            rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.right) * 200f);
        }

        //Back-Right
        Debug.DrawRay(springs[1].transform.position, transform.TransformDirection(Vector3.back), Color.blue); 
        Debug.DrawRay(springs[1].transform.position, transform.TransformDirection(Vector3.right), Color.blue); 

        if(Physics.Raycast(springs[1].transform.position, transform.TransformDirection(Vector3.back), 0.2f)){
            rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.right) * 200f);
        }

        if(Physics.Raycast(springs[1].transform.position, transform.TransformDirection(Vector3.right), 0.2f)){
            rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.forward) * 200f);
        }

        //Top-Right
        Debug.DrawRay(springs[2].transform.position, transform.TransformDirection(Vector3.forward), Color.blue); 
        Debug.DrawRay(springs[2].transform.position, transform.TransformDirection(Vector3.right), Color.blue);

        if(Physics.Raycast(springs[2].transform.position, transform.TransformDirection(Vector3.right), 0.2f)){
            rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.forward) * 200f);
        }

        if(Physics.Raycast(springs[2].transform.position, transform.TransformDirection(Vector3.forward), 0.2f)){
            rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.left) * 200f);
        } 

        //Top-Left
        Debug.DrawRay(springs[3].transform.position, transform.TransformDirection(Vector3.forward), Color.blue); 
        Debug.DrawRay(springs[3].transform.position, transform.TransformDirection(Vector3.left), Color.blue); 

        if(Physics.Raycast(springs[2].transform.position, transform.TransformDirection(Vector3.forward), 0.2f)){
            rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.left) * 200f);
        } 

        if(Physics.Raycast(springs[0].transform.position, transform.TransformDirection(Vector3.left), 0.2f)){
            rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.back) * 200f);
        }
        
        
    }

    // responsible for lateral movements, follow drone
    void followDrone(){
        int angle = AngleTo(enemyDrone,havoc);
        rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.up) * angle * 100f);
        int distance = distanceTo(enemyDrone, havoc);
        if(distance < 100){
            rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * distance * 6f, prop.transform.position);
        }else{
            rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * distance * 3f, prop.transform.position);
        }
    }

    // responsible for shooting at drone
    void attackDrone(){

    }

    // responsible for keeping track of hitpoints, and destroying if health <= 0
    void updateHealth(){
        //if distance To object tag bullet --> -1
        //if distance to object tag missile --> -5
    }

    int AngleTo(GameObject havocb, GameObject target){
        Vector3 direction = (target.transform.position - havocb.transform.position);
        int angle = (int)((Vector3.Dot(havocb.transform.right,direction))*5f);
        return angle;
    }

    int distanceTo(GameObject havocb, GameObject target){
        float dx = target.transform.position.x - havocb.transform.position.x;
        float dz = target.transform.position.z - havocb.transform.position.z;
        int distance = (int)(Mathf.Sqrt(Mathf.Pow(dx,2) + Mathf.Pow(dz,2)));
        return distance;
    }
}
