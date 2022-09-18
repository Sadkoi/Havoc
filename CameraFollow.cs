using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public GameObject swivel;
    public GameObject camera;
    public GameObject doorHinge;
    public GameObject garageDoor;
    public Rigidbody garagerb;
    public GameObject garage;
    public Joystick joystick;
    public static bool isCameraActive = false;
    public static bool isDoorOpen = false;
    public static bool isGrounded = false;

    private float smooth = 0.3f;
    private float smooth2 = 0.1f;
    private float moveSpeed = 1.2f;
    private float slowSpeed = 0.5f;
    
    private Vector3 originalPos; // original rotation of camera
    private Vector3 cameraPos; // camera position when it parents to swivel
    private Vector3 cameraAngle; // camera angle when it parents to swivel
    private Vector3 offset = new Vector3(0.0f,0.1f,-1.2f);
    
    private float nowAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = camera.transform.eulerAngles;
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        //Do this no matter what     
        swivel.transform.position = target.transform.position;
        updateIsGrounded();

        if(!isGrounded){
            restrictedRotation();
        }else{
            if(!isCameraActive){
                dotheLerp();
                followTargetAngle();
                garagerb.isKinematic = true;
                if(!isDoorOpen){
                    if(garageDoor.transform.eulerAngles.x < 90f){
                        nowAngle = Time.deltaTime * 20f;
                        garageDoor.transform.RotateAround(doorHinge.transform.position,Vector3.right,nowAngle);
                    }else{
                        isDoorOpen = true;
                    }
                }     
            }
        }
        
        if(isDoorOpen){
            isCameraActive = true;
        }

        if(isCameraActive && droneBehaviour.isDroneActive){
            regularRotation();
        }

    }

    void updateIsGrounded(){
        if(transform.position.y < 19.6){
            isGrounded = true;
        }
    }

    void dotheLerp(){
        camera.transform.parent = null;
        cameraPos = swivel.transform.position + new Vector3(0.0f, 0.1f, -1.2f);

        Vector3 nowPos = camera.transform.position;
        nowPos.y = Mathf.Clamp(Mathf.Lerp(nowPos.y,cameraPos.y, smooth2),19f,19.5f);
        nowPos.x = Mathf.Lerp(nowPos.x,cameraPos.x, smooth2);
        nowPos.z = Mathf.Lerp(nowPos.z,cameraPos.z, smooth2);

        Vector3 euler = camera.transform.eulerAngles;
        euler.y = Mathf.LerpAngle(euler.y, 0f, smooth2);
        euler.x = Mathf.LerpAngle(euler.x, 0f, smooth2);
        euler.z = Mathf.LerpAngle(euler.z, 0f, smooth2);

        camera.transform.position = nowPos;
        camera.transform.eulerAngles = euler;
        camera.transform.parent = swivel.transform;
    }

    void regularRotation(){
        if(joystick.Horizontal == 0 && joystick.Vertical == 0){
                Vector3 euler = swivel.transform.eulerAngles;
                euler.y = Mathf.LerpAngle(euler.y, target.transform.rotation.eulerAngles.y, smooth);
                euler.x = Mathf.LerpAngle(euler.x, target.transform.rotation.eulerAngles.x, smooth);
                euler.z = Mathf.LerpAngle(euler.z, target.transform.rotation.eulerAngles.z, smooth);
                swivel.transform.eulerAngles = euler;
            }else{
                swivel.transform.RotateAround(swivel.transform.position, transform.up, -joystick.Horizontal * moveSpeed);
                swivel.transform.RotateAround(swivel.transform.position, transform.right, joystick.Vertical * moveSpeed);
            }
    }

    void restrictedRotation(){
        if(joystick.Horizontal == 0 && joystick.Vertical == 0){
            Vector3 eulerPos = camera.transform.eulerAngles;
            eulerPos.y = Mathf.LerpAngle(eulerPos.y, originalPos.y, smooth);
            eulerPos.x = Mathf.LerpAngle(eulerPos.x, originalPos.x, smooth);
            eulerPos.z = Mathf.LerpAngle(eulerPos.z, originalPos.z, smooth);
            camera.transform.eulerAngles = eulerPos;
        }else{
            camera.transform.Rotate(0f,joystick.Horizontal * slowSpeed,0f,Space.Self);
            Vector3 euler1 = camera.transform.eulerAngles;
            euler1.y = Mathf.Clamp(euler1.y,292f,355f);
            camera.transform.eulerAngles = euler1;
        }
    }

    void followTargetAngle(){
        Vector3 euler = transform.eulerAngles;
        euler.y = Mathf.LerpAngle(euler.y, target.transform.rotation.eulerAngles.y, smooth);
        euler.x = Mathf.LerpAngle(euler.x, target.transform.rotation.eulerAngles.x, smooth);
        euler.z = Mathf.LerpAngle(euler.z, target.transform.rotation.eulerAngles.z, smooth);
        swivel.transform.eulerAngles = euler;
    }

    /*******************
    In the air:
        camera is parented to garage, limited movement (DONE)
    At touchdown:
        camera unparents to garage 
        camera lerps to position
        camera parents itself to swivel (DONE)
    Then:
        left joystick blinks into screen, garage door opens 
        only joystick.Vertical is used
        directly edit transform of drone, unitl it moves out of garage
        once it is out of garage, isDroneActive = true, play as normal

    camera.transform.position - swivel.transform.position = (0.0, 0.1, -1.2); <--- offset

    ******************/
}