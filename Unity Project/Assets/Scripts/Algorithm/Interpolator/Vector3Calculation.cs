using System;
using UnityEngine;


public class Vector3Calculation : ICalculationType<Vector3>
{
  private Vector3 BeginValue;
  private Vector3 EndValue;
  public float TotalPeriod { get; set; }


  public Vector3Calculation(Vector3 begin_value, Vector3 end_value, float total_period)
  {
     BeginValue = begin_value;
     EndValue = end_value;
     TotalPeriod = total_period;
  }


  public Vector3 Calculate(float delta_period)
  {
     float percent_value = Math.Min(Math.Max(delta_period / TotalPeriod, 0.0f), 1.0f);
     return (Vector3)(new Vector3(BeginValue.x * (1 - percent_value) + EndValue.x * percent_value, BeginValue.y * (1 - percent_value) + EndValue.y * percent_value, BeginValue.z * (1 - percent_value) + EndValue.z * percent_value));
  }
}
