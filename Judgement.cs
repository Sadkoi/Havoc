using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judgement : MonoBehaviour
{
    private float time;
    public GameObject itself;
    public Rigidbody rb;
    private Vector3 originalPos;
    private float distTraveled;
    // Start is called before the first frame update
    void Start()
    {
        originalPos = itself.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        shootforward();
        checkBoundaries();
    }

    //will destroy itself when it goes outside the world
    void checkBoundaries(){
        if(itself.transform.position.z > 1500 || itself.transform.position.z < -700 || itself.transform.position.x < -2000 || itself.transform.position.x > 400){
            Destroy(itself);
        }
    }

    void shootforward(){
        rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * 5000f, itself.transform.position);
    }
}
