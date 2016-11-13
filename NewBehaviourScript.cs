using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour
{


    public Transform[] backgrounds;                 //Array(list) of all the back and foregrounds to be paralaxed
    private float[] parallaxScales;                  //Propertion of the camera movement by which to move the backgrounds
    public float smoothing = 1f;                    //How smooth the paralax is going to be. Make sure to set this above '0'

    private Transform cam;                          //Reference to the main camera's transform
    private Vector3 previousCamPos;                 //The position of the camera in the previous frame

    //is called before Start. Great for references.
    void Awake()
    {
        //setup the camera reference
        cam = Camera.main.transform;
    }
    // Use this for initialization
    void Start()
    {
        //The previous frame had the current camera's position
        previousCamPos = cam.position;

        //assigning corresponding parallax scales
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //for each background
        for (int i = 0; i < backgrounds.Length; i++)
        {
            //the parallax is opposite of the camera movement because the previous frame multiplied by the scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            //set a target x position which is the current position plus the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            //create a target position which is the backgrounds current position with its target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            //fade between current position and target position using what is called 'lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
        //set the previousCamPos to the camera's position at the end of the frame
        previousCamPos = cam.position;
    }
}