using UnityEngine;
using System.Collections;

//Attach this script to the Main Camera

public class PullScript : MonoBehaviour {

	public float launchForce = 1000.0f; //Launch force
	public float raycastDist = 100.0f; //How far the raycast will search for solid objects (greater than ballPlane, less than gamePlane)
	public string canBePulledTag = "canBePulled";//tag the pull script will look for, for pullable objects
	public float gravityValue = 4.9f; //self explanatory
	public float minimumPullDistance = 0.5f;//player must pull at least this far to fire the ball

	[HideInInspector]
	public bool ballHeld;
	GameObject sphere;
	RaycastHit hitInfo;
	Ray ray;
	
	RaycastHit ballInfo;
	Vector3 originalBallPosition;
	Vector3 tempVect;

	//Prediction Line variables
	LineRenderer lineRendererReference;
	Vector3[] predictionPoints;

	public GameObject ballHolder;
	private bool firing;

	// Use this for initialization
	void Start ()
	{
		ballHolder.renderer.enabled = false;
		ballHeld = false;
		lineRendererReference = (LineRenderer)Camera.main.GetComponent<LineRenderer>();
		predictionPoints = new Vector3[10];
		firing = false;
		Physics.gravity = new Vector3 (0, -gravityValue*2, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast (ray, out hitInfo,raycastDist))
			{
				//Check if the thing we pressed can be puled
				if(hitInfo.transform.tag == canBePulledTag && ballHeld == false)
				{
					//Set the object to be invisible to raycasts and get the original ball position so we can launch it in that direction
					ballInfo = hitInfo;
					originalBallPosition = hitInfo.transform.position;
					hitInfo.transform.gameObject.layer = 2;
					ballHeld = true;
				}
			}
		}
		//If we have a ball held, transpose it to the invisible plane and our mouse position
		if (ballHeld)
		{
			ballHolder.renderer.enabled=true;
			lineRendererReference.enabled=true;
			if(Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo,raycastDist))
			{
				ballHolder.transform.position = hitInfo.point;
				ballHolder.transform.rotation = Quaternion.LookRotation(originalBallPosition-ballInfo.transform.position,Vector3.up);
				ballInfo.transform.position = hitInfo.point;
			}
			tempVect = originalBallPosition-ballInfo.transform.position;
			int j=0;
			for(float i = 0; i < 5; i = i+0.25f)
			{
				lineRendererReference.SetPosition(j,ballInfo.transform.position+tempVect*launchForce/ballInfo.rigidbody.mass*Time.fixedDeltaTime*i+Vector3.down*i*i*gravityValue);
				j++;
			}
		}
		//Make the ballHolder travel with the ball until it is "fired"
		if (firing)
		{
			ballHolder.transform.position = ballInfo.transform.position;
			if(ballHolder.transform.position.magnitude<originalBallPosition.magnitude)
			{
				ballHolder.renderer.enabled=false;
				firing = false;
			}
		}
		//If we let go, launch the ball
		if(Input.GetMouseButtonUp (0) && ballHeld)
		{
			//Make sure the player has pulled the ball far back enough
			if((originalBallPosition-ballInfo.transform.position).magnitude>minimumPullDistance)
			{
				firing = true;
				tempVect = originalBallPosition-ballInfo.transform.position;
				ballInfo.rigidbody.useGravity=true;
				ballInfo.rigidbody.AddForce(launchForce* tempVect);

			}
			else
			{
				firing = false;
				ballInfo.transform.position = originalBallPosition;
				ballHolder.renderer.enabled = false;
			}
			ballHeld = false;
			ballInfo.transform.gameObject.layer = 0;
			lineRendererReference.enabled=false;
		}
	}
}
