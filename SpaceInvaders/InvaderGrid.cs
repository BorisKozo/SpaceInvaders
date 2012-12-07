using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNA.Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
  public class InvaderGrid : IDisplayedItem
  {
    private Vector2 m_Position;
    private Vector2 m_InitialPosition;
    private Vector2 m_ScreenSize;
    private List<InvaderBase> m_InvaderRows;
    private Explosion m_ExplosionSample;
    private float m_Left;
    private float m_Right;
    private float m_Bottom;
    private int m_Count;
    private float m_Direction = 1;
    private int m_DownDirection = 0;
    private GameScene m_Scene;

    public InvaderGrid(GameScene scene, Vector2 position,Vector2 screenSize,int fireSpeed)
    {
      m_Scene = scene;
      m_Position = position;
      m_InitialPosition = position;
      m_ScreenSize = screenSize;
      m_Count = 64;
      m_InvaderRows = new List<InvaderBase>();
      m_ExplosionSample = new Explosion(new Vector2());
      float height = m_Position.Y;
      for (int rowNumber = 0; rowNumber < 5; rowNumber++)
      {
        if (rowNumber == 0) //Invader 1 row
        {
          for (int i = 0; i < 16; i++)
          {
            Invader1 invader = new Invader1(new Vector2(m_Position.X + i * (Invader1.InvaderWidth + 2), height),fireSpeed);
            invader.Dead +=new InvaderDeadEventHandler(invader_Dead);
            m_InvaderRows.Add(invader);
          }
          height += Invader1.InvaderHeight + 8;
        }
        
        if (rowNumber == 1 || rowNumber == 2)
        {
          for (int i = 0; i < 12; i++)
          {
            Invader2 invader = new Invader2(new Vector2(m_Position.X + i * (Invader2.InvaderWidth + 2), height),fireSpeed);
            invader.Dead += new InvaderDeadEventHandler(invader_Dead);
            m_InvaderRows.Add(invader);
          }
          height += Invader2.InvaderHeight + 8;
        }

        if (rowNumber == 3 || rowNumber == 4)
        {
          for (int i = 0; i < 12; i++)
          {
            Invader3 invader = new Invader3(new Vector2(m_Position.X + i * (Invader3.InvaderWidth + 2), height),fireSpeed);
            invader.Dead +=new InvaderDeadEventHandler(invader_Dead);
            m_InvaderRows.Add(invader);
          }
          height += Invader3.InvaderHeight + 8;
        }

      }
    }

    void invader_Dead(object sender, InvaderDeathEventArgs args)
    {
      int prevScore = m_Scene.Score;
      m_Scene.Score += args.Score;
      Explosion explosion = new Explosion(m_ExplosionSample);
      explosion.Alive = true;
      Vector2 position = (sender as InvaderBase).Position;
      position.X -= 10;
      position.Y -= 25;
      explosion.Position = position;
      m_Scene.AddBottomSprite(explosion);
      if (m_Scene.Score / 5000 != prevScore / 5000)
        m_Scene.Lives = m_Scene.Lives+1;
    }

    internal void Reset(int fireSpeed)
    {
      m_Count = 64;
      m_Position = m_InitialPosition;
      float height = m_Position.Y;
      int counter = 0;
      for (int rowNumber = 0; rowNumber < 5; rowNumber++)
      {
        if (rowNumber == 0) //Invader 1 row
        {
          for (int i = 0; i < 16; i++)
          {
            Invader1 invader = (Invader1)m_InvaderRows[counter];
            invader.Position = new Vector2(m_Position.X + i * (Invader1.InvaderWidth + 2), height);
            invader.FireSpeed = fireSpeed;
            invader.Alive = true;
            invader.ResetMissile();
            invader.ResetLoop();
            counter++;
          }
          height += Invader1.InvaderHeight + 8;
        }

        if (rowNumber == 1 || rowNumber == 2)
        {
          for (int i = 0; i < 12; i++)
          {
            Invader2 invader = (Invader2)m_InvaderRows[counter];
            invader.Position = new Vector2(m_Position.X + i * (Invader2.InvaderWidth + 2), height);
            invader.FireSpeed =  fireSpeed;
            invader.Alive = true;
            invader.ResetMissile();
            invader.ResetLoop();
            counter++;
          }
          height += Invader2.InvaderHeight + 8;
        }

        if (rowNumber == 3 || rowNumber == 4)
        {
          for (int i = 0; i < 12; i++)
          {
            Invader3 invader = (Invader3)m_InvaderRows[counter];
            invader.Position = new Vector2(m_Position.X + i * (Invader3.InvaderWidth + 2), height);
            invader.FireSpeed = fireSpeed;
            invader.Alive = true;
            invader.ResetMissile();
            invader.ResetLoop();
            counter++;
          }
          height += Invader3.InvaderHeight + 8;
        }

      }

    }


    public void LoadContent(ContentManager content)
    {
      m_ExplosionSample.LoadContent(content);
      foreach (InvaderBase invader in m_InvaderRows)
        invader.LoadContent(content);
    }

    #region IDisplayedItem Members

    public void Update(GameTime gameTime)
    {
      m_Left = float.MaxValue;
      m_Right = float.MinValue;
      m_Bottom = 0;
      int invaderAnimationSpeed = MathCommon.FloatToIntFloor(1000f * (float)m_Count / 100f);
      float invaderMovementSpeed = MathHelper.Clamp(20f / (float)m_Count + 0.1f, 0.3f,7f);
      m_Count = 0;
      foreach (InvaderBase invader in m_InvaderRows)
      {
        //if (Game2D.GameRandom.Next(1000) == 0)
        //  invader.Alive = false;
        if (invader.Alive)
        {

          invader.AnimationSpeed = TimeSpan.FromMilliseconds(invaderAnimationSpeed);
          Rectangle tempRect = invader.BoundingRect();
          invader.SetLeft(tempRect.Left + m_Direction * invaderMovementSpeed);
          invader.SetTop(tempRect.Top + m_DownDirection);
          m_Count++;
          
          if (tempRect.Left < m_Left)
          {
            m_Left = tempRect.Left;
          }
          if (tempRect.Right > m_Right)
          {
            m_Right = tempRect.Right;
          }

          if (tempRect.Bottom > m_Bottom)
            m_Bottom = tempRect.Bottom;

          invader.Update(gameTime);
        }
      }
      if (m_Count == 0)
      {
        OnGameWon();
        return;
      }
      if (m_Bottom >= m_ScreenSize.Y - 75)
      {
        OnGameLost();
        return;
      }
      if (m_Bottom >= m_ScreenSize.Y - 150)
      {
        OnReachedShiledLevel();
      }
      m_DownDirection = 0;
      if (m_Right >= m_ScreenSize.X - 10)
      {
        m_Direction = -1;
        m_DownDirection = 4;
      }
      if (m_Left <= 10)
      {
        m_Direction = 1;
        m_DownDirection = 4;
      }

    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {

      foreach (InvaderBase invader in m_InvaderRows)
      {
        if (invader.Alive)
          invader.Draw(gameTime, spriteBatch);
      }

    }

    #endregion

    public event EventHandler GameWon;
    public void OnGameWon()
    {
      if (GameWon != null)
        GameWon(this, new EventArgs());
    }

    public event EventHandler GameLost;
    public void OnGameLost()
    {
      if (GameLost != null)
        GameLost(this, new EventArgs());
    }

    public event EventHandler ReachedShieldLevel;
    public void OnReachedShiledLevel()
    {
      if (ReachedShieldLevel != null)
        ReachedShieldLevel(this, new EventArgs());
    }

    internal void RegisterCollision(SpriteCollisionCoordinator collisionCoordinator)
    {
      foreach (InvaderBase invader in m_InvaderRows)
      {
        collisionCoordinator.AddItem(invader,false);
        invader.CollisionCoordinator = collisionCoordinator;
      }
    }

  }
}
