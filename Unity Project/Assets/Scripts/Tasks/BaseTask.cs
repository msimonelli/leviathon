using UnityEngine;


/// <summary>
/// Base class for all jobs. Tasks are tasks that Unity game objects
/// need to perform.  Creating separate jobs helps keep game logic
/// out of the classes and make the engine easier to debug and extend.
/// </summary>
public abstract class BaseTask
{
  protected GameObject m_game_object;
  
  /// <summary>
  /// Returns true if this job is finished its task.
  /// </summary>
  public bool Finished { get; set; }


  /// <summary>
  /// Constructs a job instance.
  /// </summary>
  /// <param name="game_object">The Unity game object this job refers to.</param>
  public BaseTask(GameObject game_object)
  {
     m_game_object = game_object;
     Finished = false;
  }
  

  /// <summary>
  /// Performs the job. Note that this job may not complete in a
  /// single call to Perform, so check the IsFinished to verify
  /// when a job is complete.
  /// </summary>
  /// <param name="time_delta">The number of milliseconds that has passed since the last call.</param>
  public abstract void Perform(float time_delta);
}
