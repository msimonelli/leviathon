using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using ORKFramework;
using ORKFramework.Behaviours;


// This class handles creating and settings up a new map
public class StartNewMap : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		// Displays the town or dungeon
		Game.Instance.Map.LoadMap(SceneManager.GetActiveScene().name);
		SpawnPoint spawn_point = SpawnPoint.GetSpawnPoint(0);
		//spawn_point.transform.position = new Vector3(5, 0, 3);
		spawn_point.transform.position = new Vector3(1, 0, 1);


		Game.Instance.Party.Position = new Position((int)Math.Ceiling(spawn_point.transform.position.z), (int)Math.Ceiling(spawn_point.transform.position.x));
		Game.Instance.Party.Position.Direction = Direction.East;
	}


	// Update is called once per frame
	void Update()
	{
	}
}
