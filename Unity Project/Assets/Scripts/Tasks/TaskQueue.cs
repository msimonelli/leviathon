using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The TaskQueue class queues all jobs to be performed in a FIFO container.
/// Tasks are processed one at a time.
/// </summary>
public class TaskQueue : MonoBehaviour
{
  private Queue<BaseTask> m_jobs;
  private BaseTask m_current_job;
  
  /// <summary>
  /// Pauses or unpauses the processing of jobs.
  /// </summary>
  public bool Paused { get; set; }


  /// <summary>
  /// Constructs a new job queue
  /// </summary>
  public TaskQueue()
  {
     m_jobs = new Queue<BaseTask>();
     m_current_job = null;
  }


  /// <summary>
  /// Adds a job to the queue
  /// </summary>
  /// <param name="job">The job to add.</param>
  public void AddTask(BaseTask job)
  {
     Debug.Log("Task added");
     m_jobs.Enqueue(job);
  }


  /// <summary>
  /// Returns the number of jobs currently queued.
  /// </summary>
  /// <returns>The number of jobs currently queued.</returns>
  public int TasksQueued()
  {
     if (m_current_job == null)
        return m_jobs.Count;

     return m_jobs.Count + 1;
  }


  /// <summary>
  /// Returns the number of jobs of the specified type currently queued.
  /// </summary>
  /// <returns>The number of jobs of the specified type currently queued.</returns>
  public int TasksQueued<T>()
  {
     int tasks_num = 0;
     foreach (BaseTask job in m_jobs)
     {
        if (job is T)
           tasks_num++;
     }

     if (m_current_job != null && m_current_job is T)
        tasks_num++;

     return tasks_num;
  }


  /// <summary>
  /// Removes all jobs from the queue. This is normally called when
  /// a particular task takes over, such as a battle.
  /// </summary>
  public void ClearTasks()
  {
     m_jobs.Clear();
  }


  /// <summary>
  /// Continues processing the current job, or processes the next job.
  /// </summary>
  /// <param name="time_delta"></param>
  void Update()
  {
     // Return if we are currently paused
     if (Paused == true)
        return;

     // Get the next job, if needed
     if (m_current_job == null && m_jobs.Count > 0)
        m_current_job = m_jobs.Dequeue();

     // Return if we are out of jobs to perform
     if (m_current_job == null)
        return;

     // Perform this job
     if (!m_current_job.Finished)
     	m_current_job.Perform(Time.deltaTime);

     if (m_current_job.Finished)
        m_current_job = null;
  }
}
