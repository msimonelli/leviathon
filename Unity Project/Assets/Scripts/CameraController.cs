using UnityEngine;
using System.Collections.Generic;
using System;
using ORKFramework;


public class CameraController : MonoBehaviour
{	
	void Start()
	{
		Camera.main.nearClipPlane = 0.3f;
	}


	// Update is called once per frame
	void Update ()
	{
		GameObject player = ORK.Game.GetPlayer();
		Camera camera = Camera.main;

		if (player == null || camera == null)
			return;

		// Put the camera right where the player is
		camera.transform.position = new Vector3(player.transform.position.x, 0.5f, player.transform.position.z);
		camera.transform.rotation = new Quaternion(player.transform.rotation.x, player.transform.rotation.y, player.transform.rotation.z, player.transform.rotation.w);

		// Move the camera back just so it can see better.
		camera.transform.Translate(Vector3.back * 0.5f);
	}
}
