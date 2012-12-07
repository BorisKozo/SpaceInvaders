using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNA.Common
{
  public class TextureMover : IDisplayedItem
  {
    private TextureImage m_Image;
    private int m_CurrentStep;
    private int m_TotalSteps;
    private float m_InitialScale;
    private float m_MinimumScale;


    private Vector2 m_InitialPosition;
    private Vector2 m_FinalPosition;
    private Vector2 m_CurrentPosition;


    private byte m_Tint;
    private float m_Scale;

    public TextureMover(Vector2 initialPosition, Vector2 finalPosition, int totalSteps)
    {
      m_InitialPosition = initialPosition;
      m_FinalPosition = finalPosition;
      m_CurrentPosition = new Vector2(initialPosition.X, initialPosition.Y);
      m_TotalSteps = totalSteps;
      m_CurrentStep = 1;
      m_InitialScale = 1;
    }

    public void StepRight()
    {
      m_CurrentStep++;
      if (m_CurrentStep > m_TotalSteps)
        m_CurrentStep = 1;
    }

    public void StepLeft()
    {
      m_CurrentStep--;
      if (m_CurrentStep == 0)
        m_CurrentStep = m_TotalSteps;
    }
    #region IDisplayedItem Members

    public void Update(GameTime gameTime)
    {
      float motionPecentCyclic = 1f - 2f * (float)Math.Abs(m_CurrentStep - m_TotalSteps / 2) / (float)m_TotalSteps;
      float motionPercent = (float)m_CurrentStep / (float)m_TotalSteps;

      double tintTemp = Math.Round(255f * motionPecentCyclic);
      if (tintTemp < 0) tintTemp = 0;
      if (tintTemp > 255) tintTemp = 255;
      m_Tint = Convert.ToByte(tintTemp);

      m_Scale = (InitialScale - MinimumScale) * motionPecentCyclic + MinimumScale;
      m_CurrentPosition.X = (m_FinalPosition.X - m_InitialPosition.X) * motionPercent + m_InitialPosition.X - (float)m_Image.Image.Width * 0.5f * m_Scale + (float)m_Image.Image.Width * 0.5f * m_MinimumScale;
      m_CurrentPosition.Y = (m_FinalPosition.Y - m_InitialPosition.Y) * motionPercent + m_InitialPosition.Y;

    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      Color drawColor = Color.White;
      drawColor.R = m_Tint;
      drawColor.G = m_Tint;
      drawColor.B = m_Tint;

      spriteBatch.Draw(Image.Image, m_CurrentPosition, null, drawColor, 0, new Vector2(0, 0), m_Scale, SpriteEffects.None,1f-m_Scale);
    }

    #endregion


    public int CurrentStep
    {
      get { return m_CurrentStep; }
      set { m_CurrentStep = value; }
    }

    public TextureImage Image
    {
      get { return m_Image; }
      set { m_Image = value; }
    }

    public float InitialScale
    {
      get { return m_InitialScale; }
      set { m_InitialScale = value; }
    }

    public float MinimumScale
    {
      get { return m_MinimumScale; }
      set { m_MinimumScale = value; }
    }
  }
}
