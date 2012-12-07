using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace SpaceInvaders
{

  public class InvaderDeathEventArgs : EventArgs
  {
    public int Score
    {
      get;
      set;
    }

    public InvaderDeathEventArgs(int score)
    {
      Score = score;
    }
  }

  public delegate void InvaderDeadEventHandler(object sender, InvaderDeathEventArgs args);

  public abstract class InvaderBase : AnimatedSprite
  {
    protected SoundEffectInstance m_DeathSound;
    protected SoundEffectInstance m_InvaderFire;
    protected SoundEffectInstance[] m_MovementSounds;
    protected int prevFrame;
    protected int currentMovementSound;
    public SpriteCollisionCoordinator CollisionCoordinator { get; set; }
    public int FireSpeed { get; set; }
    protected int Score { get; set; }

    public event InvaderDeadEventHandler Dead;

    protected InvaderBase(string assetName, Vector2 position, int frameWidth,int fireSpeed)
      : base(assetName, position, frameWidth)
    {
      FireSpeed = fireSpeed;
      prevFrame = 1;
      currentMovementSound = 0;
    }

    public override void LoadContent(ContentManager content)
    {
      base.LoadContent(content);
      m_DeathSound = SoundEffectManager.Instance.GetSoundEffect("Sound//Explosion");
      m_InvaderFire = SoundEffectManager.Instance.GetSoundEffect("Sound//InvaderFire");
      m_MovementSounds = new SoundEffectInstance[4];
      m_MovementSounds[0] = SoundEffectManager.Instance.GetSoundEffect("Sound//InvaderMove1");
      m_MovementSounds[1] = SoundEffectManager.Instance.GetSoundEffect("Sound//InvaderMove2");
      m_MovementSounds[2] = SoundEffectManager.Instance.GetSoundEffect("Sound//InvaderMove3");
      m_MovementSounds[3] = SoundEffectManager.Instance.GetSoundEffect("Sound//InvaderMove4");
      for (int i = 0; i < 4; i++)
      {
        m_MovementSounds[i].Volume = 0.6f;
      }
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (prevFrame != CurrentFrame)
      {
        prevFrame = CurrentFrame;
        m_MovementSounds[currentMovementSound].Play();
        currentMovementSound = (currentMovementSound + 1) % 4;
      }
    }

    public override List<string> MyCollisionTags
    {
      get
      {
        List<String> result = new List<string>();
        result.Add("INVADER");
        return result;
      }
    }

    public override void Collide(Sprite other, Vector2 screenXY)
    {
      if (other is ShipMissile)
      {
        m_DeathSound.Play();
        Alive = false;
      }
    }

    internal virtual void OnDead()
    {
      if (Dead != null)
        Dead(this, new InvaderDeathEventArgs(Score));
    }
 
  }

  public class Invader1 : InvaderBase
  {
    private InvaderMissile2 m_Missile;
    private Random rnd = new Random();

    public Invader1(Vector2 position,int fireSpeed)
      : base("Images//Invader1", position, Invader1.InvaderWidth,fireSpeed)
    {
      LoopType = AnimatedSpriteLoopType.LoopAround;
      AnimationSpeed = TimeSpan.FromSeconds(1);
      m_Missile = new InvaderMissile2(position);
      m_Missile.Alive = false;
      m_Missile.AnimationSpeed = TimeSpan.FromMilliseconds(100);
      m_Missile.LoopType = AnimatedSpriteLoopType.LoopBackAndForth;
      Score = 40;
    }


    public static int InvaderWidth
    {
      get { return 40; }
    }

    public static int InvaderHeight
    {
      get { return 32; }
    }

    public override void LoadContent(ContentManager content)
    {
      base.LoadContent(content);
      m_Missile.LoadContent(content);
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (m_Missile.Alive)
      {
        m_Missile.Update(gameTime);
      }
      else
      {
        if (MainGame.GameRandom.Next(FireSpeed) == 0)
        {
          m_Missile.Alive = true;
          m_Missile.Position = Position;
          m_Missile.ResetLoop();
          CollisionCoordinator.AddItem(m_Missile,true);
          m_InvaderFire.Play();
        }
      }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      base.Draw(gameTime, spriteBatch);
      if (m_Missile.Alive)
        m_Missile.Draw(gameTime, spriteBatch);
    }


    internal void ResetMissile()
    {
      m_Missile.Alive = false;
    }
  }

  public class Invader2 : InvaderBase
  {
    private InvaderMissile1 m_Missile;
    public Invader2(Vector2 position,int fireSpeed)
      : base("Images//Invader2", position, InvaderWidth,fireSpeed)
    {
      LoopType = AnimatedSpriteLoopType.LoopAround;
      AnimationSpeed = TimeSpan.FromSeconds(1);
      m_Missile = new InvaderMissile1(position);
      m_Missile.Alive = false;
      Score = 20;
    }

    public override void LoadContent(ContentManager content)
    {
      base.LoadContent(content);
      m_Missile.LoadContent(content);
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (m_Missile.Alive)
      {
        m_Missile.Update(gameTime);
      }
      else
      {
        if (MainGame.GameRandom.Next(FireSpeed) == 0)
        {
          m_Missile.Alive = true;
          m_Missile.Position = Position;
          CollisionCoordinator.AddItem(m_Missile,true);
          m_InvaderFire.Play();
        }
      }

    }


    public static int InvaderWidth
    {
      get { return 54; }
    }

    public static int InvaderHeight
    {
      get { return 32; }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      base.Draw(gameTime, spriteBatch);
      if (m_Missile.Alive)
        m_Missile.Draw(gameTime, spriteBatch);
    }

    internal void ResetMissile()
    {
      m_Missile.Alive = false;
    }

  }

  public class Invader3 : InvaderBase
  {
    private InvaderMissile1 m_Missile;
    public Invader3(Vector2 position,int fireSpeed)
      : base("Images//Invader3", position, InvaderWidth,fireSpeed)
    {
      LoopType = AnimatedSpriteLoopType.LoopAround;
      AnimationSpeed = TimeSpan.FromSeconds(1);
      m_Missile = new InvaderMissile1(position);
      m_Missile.Alive = false;
      Score = 10;
    }

    public override void LoadContent(ContentManager content)
    {
      base.LoadContent(content);
      m_Missile.LoadContent(content);
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (m_Missile.Alive)
      {
        m_Missile.Update(gameTime);
      }
      else
      {
        if (MainGame.GameRandom.Next(FireSpeed) == 0)
        {
          m_Missile.Alive = true;
          m_Missile.Position = Position;
          CollisionCoordinator.AddItem(m_Missile,true);
          m_InvaderFire.Play();
        }
      }

    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      base.Draw(gameTime, spriteBatch);
      if (m_Missile.Alive)
        m_Missile.Draw(gameTime, spriteBatch);
    }

    public static int InvaderWidth
    {
      get { return 54; }
    }

    public static int InvaderHeight
    {
      get { return 32; }
    }

    internal void ResetMissile()
    {
      m_Missile.Alive = false;
    }

  }
}
