using System;
using UnityEngine;


public class QuaternionCalculation : ICalculationType<Quaternion>
{
  private Quaternion BeginValue;
  private Quaternion EndValue;
  public float TotalPeriod { get; set; }


  public QuaternionCalculation(Quaternion begin_value, Quaternion end_value, float total_period)
  {
     BeginValue = begin_value;
     EndValue = end_value;
     TotalPeriod = total_period;
  }


  public Quaternion Calculate(float delta_period)
  {
     float percent_value = Math.Min(Math.Max(delta_period / TotalPeriod, 0.0f), 1.0f);
     return (Quaternion)(new Quaternion(BeginValue.x * (1 - percent_value) + EndValue.x * percent_value, BeginValue.y * (1 - percent_value) + EndValue.y * percent_value, BeginValue.z * (1 - percent_value) + EndValue.z * percent_value, BeginValue.w * (1 - percent_value) + EndValue.w * percent_value));
  }
}
