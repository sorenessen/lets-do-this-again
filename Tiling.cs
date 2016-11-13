using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]
public class Tiling : MonoBehaviour {

    public int offsetX = 2;                 //the offset so we don't get any weird errors

    //these are used for checking to see if we need to instantiate stuff
    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;
    //used if the object is not tileable
    public bool reverseScale = false;
    //this is the width of our element
    private float spriteWidth = 0f;

    private Camera cam;
    private Transform myTransform;

    void Awake () {
        cam = Camera.main;
        myTransform = transform;

    }

    // Use this for initialization
    void Start () {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
	
	}
	
	// Update is called once per frame
	void Update () {

        //does it still need buddies? If not, do nothing...
        if (hasALeftBuddy == false || hasARightBuddy == false) {
            //calculate the cameras extent (half the widtch) of what camera can see in world coordinates
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            //calculate the x position of where the camera can see the edge of the sprite (or element)
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;
            //checking if we can see the edge of the element and then calling MakeNewBuddy if we can
            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false) {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }else if(cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false) {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
	}
    //a function that creates a new buddy on the side required
    void MakeNewBuddy (int rightOrLeft) {
        //calculating the new position for our new buddy and then calling MakeNewBuddy if we can
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        //Instantiating our new buddy and storing him in a new variable
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;
        //if not tileable, let's reverse the x size of our object to get rid of ugly seams
        if (reverseScale == true) {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);

        }

        newBuddy.parent = myTransform.parent;
        if (rightOrLeft > 0){
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;

        }
        else {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;

        }
    }

}
