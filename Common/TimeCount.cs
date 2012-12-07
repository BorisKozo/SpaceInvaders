using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA.Common
{
  public class TimeCount
  {
    public TimeSpan TotalTimeSpan{get;set;}
    public TimeSpan CurrentTimeSpan{get;set;}
    public bool Active { get; set; }
    public TimeCount(TimeSpan totalTime,bool active)
    {
      TotalTimeSpan = totalTime;
      Active = active;
      CurrentTimeSpan = new TimeSpan();
    }
    public TimeCount() : this(new TimeSpan(), false) { }
  }
}
