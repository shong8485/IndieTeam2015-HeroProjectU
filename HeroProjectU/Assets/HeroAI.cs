using UnityEngine;
using System.Collections;

public class HeroAI : MonoBehaviour 
{

	private AIStates myState;

	enum AIStates
	{
		Roaming,
		Attacking,
		Dodging
	}

	// Use this for initialization
	void Start ()
	{
		myState = AIStates.Roaming;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
