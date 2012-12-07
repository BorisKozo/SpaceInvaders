using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace XNA.Common
{
  //The type of looping:
  //None, play animation once
  //Manual, let the programmer specify when to move
  //LoopAround, play animation repeatedly
  //LoopAroundReversed, play animation repeatedly in reversed order
  //LoopBackAndForth, play animation back and forth

  public enum AnimatedSpriteLoopType
  { None, Manual, LoopAround, LoopAroundReversed, LoopBackAndForth };


  public class AnimatedSprite : Sprite
  {

    private AnimatedSpriteLoopType m_LoopType;
    private TimeSpan m_AnimationSpeed;
    private int m_FrameWidth;

    private int m_CurrentFrame;
    private int m_FrameCount;
    private TimeSpan m_UpdateTimeSum;
    private bool m_AnimationStopped;

    private int m_BackAndForthDirection;



    /// <summary>
    /// The rectangle that will be used to cut the image
    /// </summary>
    private Rectangle ImageRect
    {
      get
      {
        return new Rectangle(m_CurrentFrame * m_FrameWidth, 0, ImageWidth(), ImageHeight());
      }
    }

    protected override int ImageWidth()
    {
      return m_FrameWidth;
    }

    protected override int ImageHeight()
    {
      return base.ImageHeight();
    }

    protected override Color[,] GetImageData()
    {
      Color[,] data = m_Image.ImageData;
      Rectangle imageRect = ImageRect;
      Color[,] result = new Color[imageRect.Width, imageRect.Height];
      int k = 0;
      int h = 0;
      //Can be made faster with caching
      for (int i = imageRect.Left; i < imageRect.Right; i++, k++)
      {
        h = 0;
        for (int j = imageRect.Top; j < imageRect.Bottom; j++, h++)
          result[k, h] = data[i, j];
      }
      return result;
    }

    public AnimatedSprite(string assetName, Vector2 position, Vector2 rotationCenter, float angle, float scale, int frameWidth) :
      base(assetName, position, rotationCenter, angle, scale)
    {
      LoopType = AnimatedSpriteLoopType.None;
      AnimationSpeed = TimeSpan.FromMilliseconds(100);
      m_FrameWidth = frameWidth;
      ResetLoop();
    }

    public AnimatedSprite(string assetName, Vector2 position, Vector2 rotationCenter, float angle, int frameWidth)
      : this(assetName, position, rotationCenter, angle, 1, frameWidth)
    { }


    public AnimatedSprite(string assetName, Vector2 position, int frameWidth)
      : this(assetName, position, new Vector2(), 0, 1, frameWidth)
    { }


    protected AnimatedSprite(AnimatedSprite other)
      : base(other)
    {
      m_AnimationSpeed = other.m_AnimationSpeed;
      m_AnimationStopped = other.m_AnimationStopped;
      m_BackAndForthDirection = other.m_BackAndForthDirection;
      m_CurrentFrame = other.m_CurrentFrame;
      m_FrameCount = other.m_FrameCount;
      m_FrameWidth = other.m_FrameWidth;
      m_LoopType = other.m_LoopType;
      m_UpdateTimeSum = other.m_UpdateTimeSum;
      ResetLoop();
    }

    public override void LoadContent(ContentManager content)
    {
      base.LoadContent(content);
      m_FrameCount = m_Image.Image.Width / FrameWidth;
    }

    public override void Update(GameTime gameTime)
    {
      if (m_AnimationStopped) return;
      if (m_LoopType == AnimatedSpriteLoopType.Manual) return;
      m_UpdateTimeSum = m_UpdateTimeSum.Add(gameTime.ElapsedGameTime);
      if (m_UpdateTimeSum >= m_AnimationSpeed)
      {
        m_UpdateTimeSum = m_UpdateTimeSum - m_AnimationSpeed;
        NextFrame(m_LoopType);
      }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      if (Alive)
        spriteBatch.Draw(m_Image.Image, Position, ImageRect, Color.White, Angle, Origin, Scale, SpriteEffects.None, 0);
    }

    public void ResetLoop()
    {
      m_CurrentFrame = 0;
      m_UpdateTimeSum = new TimeSpan();
      m_AnimationStopped = false;
      m_BackAndForthDirection = 1;
    }

    public void NextFrame(AnimatedSpriteLoopType loopType)
    {
      switch (loopType)
      {
        case AnimatedSpriteLoopType.None:
          {
            m_CurrentFrame++;
            if (m_CurrentFrame == m_FrameCount)
            {
              m_CurrentFrame--;
              m_AnimationStopped = true;
            }
            break;
          }
        case AnimatedSpriteLoopType.LoopAround:
          {
            m_CurrentFrame++;
            if (m_CurrentFrame == m_FrameCount)
              m_CurrentFrame = 0;
            break;
          }
        case AnimatedSpriteLoopType.LoopAroundReversed:
          {
            m_CurrentFrame--;
            if (m_CurrentFrame == -1)
              m_CurrentFrame = m_FrameCount - 1;
            break;
          }
        case AnimatedSpriteLoopType.LoopBackAndForth:
          {
            m_CurrentFrame += m_BackAndForthDirection;
            if (m_CurrentFrame == m_FrameCount)
            {
              m_BackAndForthDirection = -1;
              m_CurrentFrame -= 2;
            }
            else
              if (m_CurrentFrame == -1)
              {
                m_BackAndForthDirection = 1;
                m_CurrentFrame = 1;
              }
            break;
          }
      }

    }

    public void PrevFrame(AnimatedSpriteLoopType loopType)
    {
      switch (loopType)
      {
        case AnimatedSpriteLoopType.None:
          {
            m_CurrentFrame--;
            if (m_CurrentFrame < 0)
            {
              m_CurrentFrame = 0;
              m_AnimationStopped = true;
            }
            break;
          }
        case AnimatedSpriteLoopType.LoopAround:
          {
            m_CurrentFrame--;
            if (m_CurrentFrame < 0)
              m_CurrentFrame = m_FrameCount-1;
            break;
          }
        case AnimatedSpriteLoopType.LoopAroundReversed:
          {
            m_CurrentFrame++;
            if (m_CurrentFrame == m_FrameCount)
              m_CurrentFrame = 0;
            break;
          }
        case AnimatedSpriteLoopType.LoopBackAndForth:
          {
            m_CurrentFrame -= m_BackAndForthDirection;
            if (m_CurrentFrame == m_FrameCount)
            {
              m_BackAndForthDirection = -1;
              m_CurrentFrame -= 2;
            }
            else
              if (m_CurrentFrame == -1)
              {
                m_BackAndForthDirection = 1;
                m_CurrentFrame = 1;
              }
            break;
          }
      }
    }

    public AnimatedSpriteLoopType LoopType
    {
      get { return m_LoopType; }
      set { m_LoopType = value; }
    }

    public TimeSpan AnimationSpeed
    {
      get { return m_AnimationSpeed; }
      set { m_AnimationSpeed = value; }
    }

    public int FrameWidth
    {
      get { return m_FrameWidth; }
      set
      {
        m_FrameWidth = value;
      }
    }

    public bool AnimationStopped
    {
      get { return m_AnimationStopped; }
    }

    public int CurrentFrame
    {
      get { return m_CurrentFrame; }
    }

  }
}
