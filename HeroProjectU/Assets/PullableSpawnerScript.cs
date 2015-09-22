using UnityEngine;
using System.Collections;

public class PullableSpawnerScript : MonoBehaviour {

	public GameObject spawnThis;
	public PullScript pullScriptRef;
	private bool spawnItem;

	[HideInInspector]
	public float cooldownTimeMax;
	public float cooldownTime;

	private GameObject ballReference;

	// Use this for initialization
	void Start ()
	{
		transform.renderer.enabled = false;
		spawnItem = true;
		cooldownTimeMax = cooldownTime;
		cooldownTime = 0;
	}

	// Update is called once per frame
	void Update ()
	{
		if(cooldownTime <= 0)
		{
			if(spawnItem)
			{
				ballReference = (GameObject)Instantiate (spawnThis);
				ballReference.transform.position = transform.position;
				ballReference.rigidbody.useGravity = false;
				cooldownTime = cooldownTimeMax;
				spawnItem = false;
			}
		}
		else
		if(cooldownTime > 0)
		{
			cooldownTime = cooldownTime - Time.deltaTime;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (!pullScriptRef.ballHeld)
		{
			spawnItem = true;
			cooldownTime = cooldownTimeMax;
		}
	}
}
