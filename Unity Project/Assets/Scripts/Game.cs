using System.Collections.Generic;
using System;
using UnityEngine;
using ORKFramework;


public class Game : MonoBehaviour
{
	public static Game Instance = null;
	public Party Party { get; set; }
	public TaskQueue TaskQueue { get; set; }
	public Map Map { get; set; }


	// This sets up the main game object
	void Awake()
	{
		// Singleton
		Instance = this;
		
		// Make sure this object stays alive even after switching scenes!
		DontDestroyOnLoad(this);

		
		// Create our JobQueue to create tasks that take time to execute/complete
		TaskQueue = CreateGameScriptInstance<TaskQueue>("TaskQueue");
		DontDestroyOnLoad(TaskQueue);

		// Displays the town or dungeon
		Map = CreateGameScriptInstance<Map>("Map");
		DontDestroyOnLoad(Map);

		// Create party instance
		Party = CreateGameScriptInstance<Party>("Party");
		DontDestroyOnLoad(Party);


		//cube.AddComponent("PlayerCharacter");
		//Component comp = cube.GetComponent("PlayerCharacter");
		/*
		
		Camera.main.gameObject.AddComponent("Party");
		Party = (Party)Camera.main.gameObject.GetComponent("Party");
		
		Party.Position = new Position((int)Math.Ceiling(Camera.main.transform.position.z), (int)Math.Ceiling(Camera.main.transform.position.x));
		Party.Position.Direction = Direction.East;
		
		Debug.Log("PosX: " + Party.Position.X.ToString() + ", PosY: " + Party.Position.Y.ToString());
		Debug.Log("Move speed = " + GamePreferences.PartyMoveSpeed);*/
	}
	
	
	public T CreateGameScriptInstance<T>(string object_name)
	{
		GameObject obj = new GameObject(object_name);
		UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(obj, "Assets/Scripts/Game.cs (56,3)", object_name);
		
		return (T)Convert.ChangeType(obj.GetComponent(object_name), typeof(T));
	}
	
	/*
	/// <summary>
	/// This checks for a keypress to indicate the player wants to turn.
	/// </summary>
	void CheckTurnKeys()
	{
		// Check if user wants to turn
		if ((Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") < 0.0f))
		{
			Turn turn = Turn.None;
			
			if (Input.GetAxis("Horizontal") < 0.0f)
				turn = Turn.Left;
			else if (Input.GetAxis("Horizontal") > 0.0f)
				turn = Turn.Right;
			else if (Input.GetAxis("Vertical") < 0.0f)
				turn = Turn.Around;
			
			Party.Turn(turn);
		}
	}
	

	/// <summary>
	/// This checks for a keypress to indicate the player wants to move.
	/// </summary>
	void CheckMoveKeys()
	{
		if (Input.GetAxis("Vertical") > 0.0f)
		{
			Party.MoveForward();
		}
	}
	
	
	/// <summary>
	/// This handles moving the camera if the player choose to move or turn
	/// around.
	/// </summary>
	void Update()
	{
		CheckMoveKeys();
		CheckTurnKeys();
	}
	*/
}
