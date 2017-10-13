using System;
using UnityEngine;


public class FloatCalculation : ICalculationType<float>
{
  private float BeginValue;
  private float EndValue;
  public float TotalPeriod { get; set; }


  public FloatCalculation(float begin_value, float end_value, float total_period)
  {
     BeginValue = begin_value;
     EndValue = end_value;
     TotalPeriod = total_period;
  }


  public float Calculate(float delta_period)
  {
     float percent_value = Math.Min(Math.Max(delta_period / TotalPeriod, 0.0f), 1.0f);
     return (float)(BeginValue * (1 - percent_value) + EndValue * percent_value);
  }
}
