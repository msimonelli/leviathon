using System.Collections.Generic;
using UnityEngine;
using ORKFramework;


/// <summary>
/// This represents a PC (player character) in the game.
///
/// Player characters represent real people in a users party. There can
/// can be numerous player characters in a party.
/// </summary>
public class Party : MonoBehaviour
{
  private Position m_position;

  /// <summary>
  /// Sets or returns the position of the party.
  /// </summary>
  public Position Position
  {
     get
     {
        return m_position;
     }

     set
     {
        if (m_position.X != value.X || m_position.Y != value.Y)
        {
           m_position = value;
			
		   // Move our actual player to our new position. This will also
		   // trigger events that may occur if we run onto a tile that has
		   // an event attached to it.
		   if (ORK.Game.GetPlayer())
		      ORK.Game.GetPlayer().transform.position = new Vector3(m_position.Y, ORK.Game.GetPlayer().transform.position.y, m_position.X);
        }
        else
           m_position = value;

        Debug.Log("xPosX: " + m_position.X.ToString() + ", xPosY: " + m_position.Y.ToString());
     }
  }
	

  /// <summary>
  /// Constructs an empty Party instance.
  /// </summary>
  public Party()
  {
	 m_position = new Position();
  }


  /// <summary>
  /// Constructs a copy of a Party instance.
  /// </summary>
  public Party(Party other)
  {
     m_position = other.m_position;;
  }
	

  public void MoveForward()
  {
     if (Game.Instance.TaskQueue.TasksQueued<Task.MoveParty>() == 0 && Game.Instance.TaskQueue.TasksQueued<Task.TurnParty>() == 0)
        Game.Instance.TaskQueue.AddTask(new Task.MoveParty(Move.Forward));
  }


  public void MoveBackward()
  {
     if (Game.Instance.TaskQueue.TasksQueued<Task.MoveParty>() == 0 && Game.Instance.TaskQueue.TasksQueued<Task.TurnParty>() == 0)
        Game.Instance.TaskQueue.AddTask(new Task.MoveParty(Move.Back));
  }


  public void MoveLeft()
  {
     if (Game.Instance.TaskQueue.TasksQueued<Task.MoveParty>() == 0 && Game.Instance.TaskQueue.TasksQueued<Task.TurnParty>() == 0)
        Game.Instance.TaskQueue.AddTask(new Task.MoveParty(Move.Left));
  }


  public void MoveRight()
  {
     if (Game.Instance.TaskQueue.TasksQueued<Task.MoveParty>() == 0 && Game.Instance.TaskQueue.TasksQueued<Task.TurnParty>() == 0)
        Game.Instance.TaskQueue.AddTask(new Task.MoveParty(Move.Right));
  }


  public void TeleportTo(int x, int y)
  {
     // TODO: We need to have some kind of "TeleportParty" task that shows some
     // graphical indicators that we are teleporting, then set the position AFTER
     // this, similar to moving forward. But for now we just appear there.

     Position new_pos = new Position(x, y);
     
     if (Game.Instance.Map.IsObstructed(new_pos.X, new_pos.Y))
        return;

     this.Position = new_pos;
  }


  public void Turn(Turn turn_direction)
  {
     if (Game.Instance.TaskQueue.TasksQueued<Task.MoveParty>() == 0 && Game.Instance.TaskQueue.TasksQueued<Task.TurnParty>() == 0)
        Game.Instance.TaskQueue.AddTask(new Task.TurnParty(turn_direction));
  }
}
