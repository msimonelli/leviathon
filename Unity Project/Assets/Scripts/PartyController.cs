using UnityEngine;
using System.Collections.Generic;
using System;
using ORKFramework;


public class PartyController : MonoBehaviour
{
	public float m_last_check;


	void Start()
	{
		m_last_check = 0.0f;
		gameObject.AddComponent<CameraController>();
	}


	// Update is called once per frame
	void Update ()
	{
		m_last_check += Time.deltaTime;

		//if (m_last_check >= 0.5f)
		{
			CheckMoveKeys();
			CheckTurnKeys();

			m_last_check = 0.0f;
		}
	}
	

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

			Game.Instance.Party.Turn(turn);
		}
	}
	
	
	/// <summary>
	/// This checks for a keypress to indicate the player wants to move.
	/// </summary>
	void CheckMoveKeys()
	{
		if (Input.GetAxis("Vertical") > 0.0f)
		{
			Game.Instance.Party.MoveForward();
		}
	}
}
