using UnityEngine;
using ORKFramework;


namespace Task
{
	public class TurnParty : BaseTask
	{
	  private Interpolator<Vector3, Vector3Calculation> m_position_interpolator;
	  private Interpolator<Quaternion, QuaternionCalculation> m_rotation_interpolator;
	  private Turn m_turn_direction;
	  private float m_turn_speed;


	  /// <summary>
	  /// Constructs a job to turn the party.
	  /// </summary>
	  /// <param name="game_object"></param>
	  public TurnParty(Turn turn_direction)
	  : base(ORK.Game.GetPlayer())
	  {
	     m_position_interpolator = null;
	     m_rotation_interpolator = null;
	     m_turn_direction = turn_direction;
	     m_turn_speed = 1.0f;

	     // When we turn we actually need to reposition our camera a bit, as it
	     // is offset from the center of the tile to the back of the tile. So we
	     // recenter camera, turn, then offset back.
	     Quaternion start_quat = m_game_object.transform.rotation;
	     Vector3 start_pos = m_game_object.transform.position;
	     //m_game_object.transform.Translate(Vector3.forward * 0.5f);
	     m_game_object.transform.RotateAround(m_game_object.transform.position, Vector3.up, (m_turn_direction == Turn.Left ? -90 : (m_turn_direction == Turn.Right ? 90 : 180)));
	     //m_game_object.transform.Translate(Vector3.back * 0.5f);
	     Vector3 end_pos = m_game_object.transform.position;
	     Quaternion end_quat = m_game_object.transform.rotation;
	     m_game_object.transform.rotation = start_quat;
	     m_game_object.transform.position = start_pos;

	     // Setup the interpolator with begin/end rotations and position and the time required
	     // to span that (0.5 seconds by default).
	     m_rotation_interpolator = new Interpolator<Quaternion, QuaternionCalculation>(new QuaternionCalculation(start_quat, end_quat, 0.4f / m_turn_speed));
	     m_position_interpolator = new Interpolator<Vector3, Vector3Calculation>(new Vector3Calculation(start_pos, end_pos, 0.4f / m_turn_speed));
	  }


	  /// <summary>
	  /// Performs the job. Note that this job may not complete in a
	  /// single call to Perform, so check the IsFinished to verify
	  /// when a job is complete.
	  /// </summary>
	  /// <param name="time_delta">The number of milliseconds that has passed since the last call.</param>
	  public override void Perform(float time_delta)
	  {
	     // Calculate and set the new rotation based on time elapsed.
	     Quaternion rot = m_rotation_interpolator.Calculate(Time.deltaTime);
	     m_game_object.transform.rotation = rot;

	     // Calculate and set the new position based on time elapsed.
	     Vector3 pos = m_position_interpolator.Calculate(Time.deltaTime);
	     m_game_object.transform.position = pos;

	     if (m_rotation_interpolator.IsFinished())
	     {
	        // We have completed our turn, so notify the game engine
	        // that our direction has changed.
			Position new_pos = Game.Instance.Party.Position;

	        if (m_turn_direction == Turn.Left && new_pos.Direction == Direction.North ||
	           m_turn_direction == Turn.Around && new_pos.Direction == Direction.East ||
	           m_turn_direction == Turn.Right && new_pos.Direction == Direction.South)
	           new_pos.Direction = Direction.West;
	        else if (m_turn_direction == Turn.Right && new_pos.Direction == Direction.North ||
	           m_turn_direction == Turn.Around && new_pos.Direction == Direction.West ||
	           m_turn_direction == Turn.Left && new_pos.Direction == Direction.South)
	           new_pos.Direction = Direction.East;
	        else if (m_turn_direction == Turn.Right && new_pos.Direction == Direction.West ||
	           m_turn_direction == Turn.Around && new_pos.Direction == Direction.South ||
	           m_turn_direction == Turn.Left && new_pos.Direction == Direction.East)
	           new_pos.Direction = Direction.North;
	        else if (m_turn_direction == Turn.Right && new_pos.Direction == Direction.East ||
	           m_turn_direction == Turn.Around && new_pos.Direction == Direction.North ||
	           m_turn_direction == Turn.Left && new_pos.Direction == Direction.West)
	           new_pos.Direction = Direction.South;

			Game.Instance.Party.Position = new_pos;

	        m_rotation_interpolator = null;
	        m_position_interpolator = null;
	        m_turn_direction = Turn.None;
	        Finished = true;
	     }
	  }
	}
}
