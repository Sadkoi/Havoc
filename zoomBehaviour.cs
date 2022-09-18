using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class zoomBehaviour : MonoBehaviour
{
    public GameObject swivel;
    public GameObject camera;
    public Button b;
    private int mode = 1;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = b.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void TaskOnClick()
    {
        if(mode == 1){
            camera.transform.position += new Vector3(0f,0f,2f);
            mode = 2;
            return;
        }else{
            if(mode == 2){
                camera.transform.position += new Vector3(0f,0f,-2f);
                mode = 1;
                return;
            }
        }
    }
}
