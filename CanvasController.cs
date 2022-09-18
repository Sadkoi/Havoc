using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour //God script
{
    public GameObject FireR;
    public Button btnFR;
    public Button btnFL;
    public GameObject FireL;
    public GameObject Camera;
    public GameObject thrust;
    public GameObject swivel;
    public GameObject thrusthandle;
    public GameObject heading;
    public GameObject turret;
    public float time = 0;
    public Text dialogueBox;
    public Text levelName;
// MEANT FOR ENEMY SPAWN
    public GameObject originalDrone;
    public int enemyCount;
    private float xpos;
    private float zpos;
    private float ypos;
// ABOVE MEANT FOR ENEMY SPAWN
    public GameObject plane;
    public GameObject highpoint;
    public GameObject bullet;

    private int DialogueIndex = -1;
    private float timeIndex = 3;

    private Joystick joystick;
    private bool isClear = false;
    private bool isTouchedJoystick = false;
    private float interval = 0.25f;
    private float startDelay = 0.5f;
    private bool currentState = true;
    private bool defaultState = true;
    private bool isBlinking = false;
    private bool untouched = true;
    private bool hasNotReachedTop = true;
    private bool checkedBoxState = true;

    private bool warDialogue = true;
    private float startHighPointD = Mathf.Infinity;
    public int warDialogueIndex;

    private float WARFAREDisplay = Mathf.Infinity;
    private bool warfareTimeSet = true;

    private float startTime; // used in displayfor method - 1. Exploration
    private bool checkedTime = true;
    private float startTimetutorial = Mathf.Infinity; 
    private int DialogueIndex2 = -1;

    private string[] dialogue = {
        "MAY 21, 2058",
        "",
        "We are Havoc",
        "Mercenaries, fighters, soldiers", 
        "Whatever the hell you wanna call us", 
        "We wage war without mercy", 
        "We strike without sympathy", 
        "And we answer to no one",            //pause
        "",
        "Our line of work requires sacrifice", 
        "Some of us die heroes", 
        "Others live to become them",
        "And me?", 
        "Well, I made my peace with death a long time ago", //pause
        "",
        "Our mission today is different",
        "Today, we’re not trading blood for money",
        "We are here to put Artificial Intelligence in its rightful place",
        "They eradicated our people", 
        "Made our planet uninhabitable", 
        "And now",
        "We’ve come to return the favor", 
        ""
        };

    private string[] tutorialdialogue = {
        "Welcome to the HAVOC P-99X",
        "The newest edition to the HAVOC fleet",
        "Use the left joystick to move around",
        "Use the right joystick to look around",
        "",
        "",
        "Your orders are to destroy the AI base",
        "But to do that, you have to find it first",
        "Head to the highest point within a five mile radius",
        "Coordinates have been uploaded to your vehicle",
        "Follow the red triangle on the top of your screen",
        "And try not to get yourself killed",
        "Havoc - Over and out",
        ""
    };

    private string[] highpointDialogue = {
        "",
        "Scanning",
        "",
        "The AI base is three klicks northeast of your position",
        "But the base is impenetrable to outside attacks",
        "You will need one of their drones to get in",
        "They've been using the AVION M-86",
        "You will have to steal one",
        "That means finding a posse, and starting up a fight",
        "But hey, that's what you signed up for right?",
        "",
        "Now might be a good time to get familiar with your weapon systems", // show the target buttons
        "Tap once to shoot", // wait for tap
        "Press and hold to launch a guided missile", 
        "Use the joystick to steer",
        "And destroy your target",
        "Dont dissapoint us",
        "Havoc - over and out",
        "",
    };

    private string[] fightThePosseDialogue = {
        "",
        "You've been spotted",
        "They've scrambled drones",
        "You're surrounded",
        "There are ten AVION M-86's closing in on you right now",
        "Ten of them, and one of you",
        "You like those odds champ?",
        "Kill them",
        "And if you fail, don't bother coming back",
        "Then again, you won't have much of a choice",
        "Havoc - over and out",
        "",
    };

    // Start is called before the first frame update
    void Start()
    {
        joystick = thrust.GetComponent<Joystick>();
        originalDrone = GameObject.Find("enemigos");
        dialogueBox.text = "MAY 21, 2058";
        btnFR.onClick.AddListener(fireBullet);
        btnFL.onClick.AddListener(fireBullet);
    }

    // Update is called once per frame
    void Update()
    {
        updateTime();
        showDialogue();
        DisplayDialogue();
        
        updateTutorialDialgoue2();
        DisplayTutorialDialogue();
        showHeading();
        updatehilltopDialogue();
        displayhilltopDialogue();
        phase2update();
        phase2Display();
        if(warDialogueIndex == 11){
            FireR.SetActive(true);
            FireL.SetActive(true);
        }

        if(!CameraFollow.isGrounded){
            heading.SetActive(false);
            FireR.SetActive(false);
            FireL.SetActive(false);
            Camera.SetActive(false);
            thrust.SetActive(false);
            levelName.text = "";
            swivel.SetActive(true);
        }else{
            swivel.SetActive(false);
            if(CameraFollow.isDoorOpen){
                thrust.SetActive(true);
            }
        }
        if(droneBehaviour.isDroneActive){
            swivel.SetActive(true);
        }
        if(droneBehaviour.isDroneActive && checkedTime){
            startTime = time;
            checkedTime = false;
            levelName.text = "1. Exploration";
            startTimetutorial = startTime + 8;
        }
        if((time > startTime + 3) && checkedBoxState){
            levelName.text = "";
        }
        //print(warDialogueIndex);
        checkForEnemySpawnTime();
        checkForWeaponsFire();
        startWarfareDialogue();
    }

    void updateTime(){
        time += Time.deltaTime;
    }

    void showDialogue(){
        if(time > timeIndex){
            timeIndex += 3;
            DialogueIndex++;
        }
    }

    void DisplayDialogue(){
        if(DialogueIndex >= 0 && DialogueIndex < dialogue.Length){
            dialogueBox.text = dialogue[DialogueIndex];
        }
    }

    void updateTutorialDialgoue2(){
        if(startTimetutorial < time){
            startTimetutorial += 3;
            DialogueIndex2++;
        }
    }

    void DisplayTutorialDialogue(){
        
        if(DialogueIndex2 >= 0 && DialogueIndex2 < tutorialdialogue.Length){
            dialogueBox.text = tutorialdialogue[DialogueIndex2];
        }
    }

    int AngleTo(GameObject havoc, GameObject target){
        Vector3 direction = (target.transform.position - havoc.transform.position);
        int angle = (int)((Vector3.Dot(havoc.transform.right,direction))*5f);
        return angle;
    }

    void showHeading(){                                          //for highest point
        if(DialogueIndex2 >= 9 && plane.transform.position.y < 185f && hasNotReachedTop){
            heading.SetActive(true);
            int angle = AngleTo(plane, highpoint);
            heading.GetComponent<RectTransform>().anchoredPosition = new Vector3(Mathf.Clamp(angle,-790,790), 426f,0f);
            checkedBoxState = false;
        }else{
            if(plane.transform.position.y > 185f && DialogueIndex2 >= 9){
                heading.SetActive(false);
                hasNotReachedTop = false;
                if(warDialogue){
                    startHighPointD = time;
                    warDialogue = false;
                }
            }
        }
        
    }

    void updatehilltopDialogue(){
        if(time > startHighPointD){
            startHighPointD += 3;
            warDialogueIndex++;
        }
    }

    void displayhilltopDialogue(){
        if(warDialogueIndex > 0 && warDialogueIndex < highpointDialogue.Length){
            dialogueBox.text = highpointDialogue[warDialogueIndex];
        }
    }

    void phase2update(){
        if(warDialogueIndex >= highpointDialogue.Length && warfareTimeSet){
            WARFAREDisplay = time;
            warfareTimeSet = false;
        }    
    }

    void phase2Display(){
        if(time >= WARFAREDisplay && time < WARFAREDisplay + 4){
            levelName.text = "   2. Warfare";

        }
        if(time >= WARFAREDisplay + 4){
            levelName.text = "";
        }
    }

    IEnumerator spawner(){
        while(enemyCount < 10){
            xpos = Random.Range(plane.transform.position.x - 50, plane.transform.position.x + 50);
            zpos = Random.Range(plane.transform.position.z - 50, plane.transform.position.z + 50);
            ypos = Random.Range(plane.transform.position.y + 50, plane.transform.position.y + 150);
            Instantiate(originalDrone, new Vector3(xpos, ypos, zpos), Quaternion.identity);
            enemyCount++;
            yield return new WaitForSeconds(1f);
        }
    }

    //checks to see if its time to spawn enemies (refrencing warDialogueIndex) 
    //and then spawns them by calling spawner
    private bool enumHasNotStarted = true;
    void checkForEnemySpawnTime(){
        if(enumHasNotStarted && (warDialogueIndex == 30)){
            StartCoroutine(spawner());
            enumHasNotStarted = false;
        }
    }

    void startWarfareDialogue(){
        if(warDialogueIndex >= 28 && warDialogueIndex <= 39){
            dialogueBox.text = fightThePosseDialogue[warDialogueIndex - 28];
        }
    }

    void checkForWeaponsFire(){
        
    }

    void fireBullet(){
        Instantiate(bullet, turret.transform.position, plane.transform.rotation);
    }

    //Havoc Garage y pos = 320.95
}
