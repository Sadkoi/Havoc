using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public GameObject bullet;
    public GameObject plane;
    public GameObject newBullet;
    // Start is called before the first frame update
    void createBullet(){
        newBullet = Instantiate(bullet, plane.transform.position, plane.transform.rotation);

    }
}
