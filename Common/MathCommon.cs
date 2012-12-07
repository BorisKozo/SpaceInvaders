using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA.Common
{
  public static class MathCommon
  {
    public static float Max4(float v1, float v2, float v3, float v4)
    {
      float maxTemp1 = MathHelper.Max(v1, v2);
      float maxTemp2 = MathHelper.Max(v3, v4);
      return MathHelper.Max(maxTemp1, maxTemp2);
    }

    public static float Min4(float v1, float v2, float v3, float v4)
    {
      float minTemp1 = MathHelper.Min(v1, v2);
      float minTemp2 = MathHelper.Min(v3, v4);
      return MathHelper.Min(minTemp1, minTemp2);
    }

    public static int FloatToIntFloor(float v)
    {
      return Convert.ToInt32(Math.Floor(v));
    }

    public static int FloatToIntRound(float v)
    {
      return Convert.ToInt32(Math.Round(v));
    }
  }
}
