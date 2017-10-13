using System;
using System.Collections.Generic;
using UnityEngine;
using ORKFramework;


namespace Task
{
	public class MoveParty : BaseTask
	{
	  private Interpolator<Vector3, Vector3Calculation> m_position_interpolator;
	  private Move m_move_direction;
	  private float m_move_speed;
	  private Position m_new_pos;
	  private bool m_is_obstructed;


	  /// <summary>
	  /// Constructs a job to move the party.
	  /// </summary>
	  /// <param name="game_object"></param>
	  public MoveParty(Move move_direction)
	  : base(ORK.Game.GetPlayer())
	  {
	     // Find the position the party wants to move to
	     m_new_pos = new Position(Game.Instance.Party.Position);
	     m_new_pos.OffsetForward();

	     // Don't bother moving if we can't do it
	     m_is_obstructed = Game.Instance.Map.IsObstructed(m_new_pos.X, m_new_pos.Y);
	     if (m_is_obstructed)
		 {
			Finished = true;
	        return;
		 }

	     m_position_interpolator = null;
	     m_move_direction = move_direction;
	     m_move_speed = 1.0f;

	     Vector3 start_pos = m_game_object.transform.position;
	     m_game_object.transform.Translate(Vector3.forward * (m_move_direction == Move.Forward ? 1 : -1));
	     Vector3 end_pos = m_game_object.transform.position;
	     m_game_object.transform.position = start_pos;

			Debug.Log ("Good move: " + m_new_pos.X.ToString() + ", " + m_new_pos.Y.ToString() );

	     // Setup the interpolator with begin/end positions and the time required
	     // to span that (0.f seconds by default).
	     m_position_interpolator = new Interpolator<Vector3, Vector3Calculation>(new Vector3Calculation(start_pos, end_pos, 0.3f / m_move_speed));
	  }


	  /// <summary>
	  /// Performs the job. Note that this job may not complete in a
	  /// single call to Perform, so check the IsFinished to verify
	  /// when a job is complete.
	  /// </summary>
	  /// <param name="time_delta">The number of milliseconds that has passed since the last call.</param>
	  public override void Perform(float time_delta)
	  {
        // Calculate and set the new position based on time elapsed.
        Vector3 pos = m_position_interpolator.Calculate(Time.deltaTime);
        m_game_object.transform.position = pos;

        if (m_position_interpolator.IsFinished())
        {
           Debug.Log("Party moved!");

           // We have completed our move, so notify the Party instance that
           // we have moved.
           Game.Instance.Party.Position = m_new_pos;

           m_position_interpolator = null;
           Finished = true;
        }
	  }
	}
}
