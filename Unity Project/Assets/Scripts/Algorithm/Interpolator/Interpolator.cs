using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface ICalculationType<ValueType>
{
  float TotalPeriod { get; set; }
  ValueType Calculate(float delta_period);
}

/// <summary>
/// The Interpolator class computes an itermediate value between two given
/// points based on the period given.  The period given is usually time, but
/// is not always so.  This class is primarily used to smoothly transition
/// from the starting value to the ending value over a set period of time.
/// </summary>
/// <typeparam name="ValueType">The type of value to interpolate</typeparam>
/// <typeparam name="CalculationType">The calculation class that does the interpolation calculation</typeparam>
public class Interpolator<ValueType, CalculationType> where CalculationType : ICalculationType<ValueType>
{
  private CalculationType m_calculation;

  private bool m_paused;
  private int m_current_run;
  private float m_current_period;
  private ValueType m_current_value;

  public float SpeedFactor { get; set; }
  public int MaxRuns { get; set; }


  public Interpolator(CalculationType calculation, float speed_factor = 1.0f, int max_run = 1)
  {
     m_calculation = calculation;
     MaxRuns = max_run;
     SpeedFactor = speed_factor;
     m_current_run = 0;
     m_paused = false;
     m_current_period = 0.0f;
     m_current_value = Calculate(0.0f);
  }


  public void Pause()
  {
     m_paused = true;
  }


  public void Resume()
  {
     m_paused = false;
  }


  public void Reset()
  {
     m_current_run = 0;
     m_current_period = 0;
     m_current_value = Calculate(0.0f);
     m_paused = false;
  }


  public bool IsPaused()
  {
     return m_paused;
  }


  public bool IsFinished()
  {
     return (m_current_run >= MaxRuns && MaxRuns != -1);
  }


  public ValueType Calculate(float delta_period)
  {
     if (!m_paused && m_current_run < MaxRuns)
     {
        // Find our 'virtual elapsed time' based on speed factor.
        m_current_period += (delta_period * SpeedFactor);

        m_current_value = m_calculation.Calculate(m_current_period);

        // Take into account multiple runs
        if (m_current_period >= m_calculation.TotalPeriod)
        {
           if (MaxRuns != -1)
              ++m_current_run;

           if (m_current_run < MaxRuns)
              m_current_period = 0;
        }
     }

     return m_current_value;
  }
}
