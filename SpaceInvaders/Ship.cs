using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace SpaceInvaders
{
  public class Ship : AnimatedSprite
  {
    Vector2 m_Position;
    ShipMissile m_Missile;
    SpriteCollisionCoordinator m_CollisionCoordinator;
    private SoundEffectInstance m_FireSound;
    private bool m_GotHit;


    public event EventHandler ShipDead;
    public void OnShipDead()
    {
      if (ShipDead != null)
        ShipDead(this, new EventArgs());
    }

    public Ship(Vector2 initialPosition):
      base("Images//ship", initialPosition, 60)
    {
      m_Position = initialPosition;
      AnimationSpeed = TimeSpan.FromMilliseconds(500);
      LoopType = AnimatedSpriteLoopType.LoopBackAndForth;
      m_Missile = new ShipMissile(new Vector2());
      m_Missile.Alive = false;
    }

    public override void LoadContent(ContentManager content)
    {
      base.LoadContent(content);
      m_Missile.LoadContent(content);
      m_FireSound = SoundEffectManager.Instance.GetSoundEffect("Sound\\ShipFire");
    }

    #region IDisplayedItem Members

    public override void Update(GameTime gameTime)
    {
      m_GotHit = false;
      base.Update(gameTime);
      KeyboardState state = Keyboard.GetState();
      if (state.IsKeyDown(Keys.Left))
        m_Position.X -= (2f+(float)MainGame.GameRandom.NextDouble());
      
      if (state.IsKeyDown(Keys.Right))
        m_Position.X += 2f+(float)MainGame.GameRandom.NextDouble();

      m_Position.X = MathHelper.Clamp(m_Position.X, 10, 800-60-10);
      SetLeft(MathCommon.FloatToIntRound(m_Position.X));

      if (state.IsKeyDown(Keys.Space) && !m_Missile.Alive)
      {
        m_Missile.SetLeft(MathCommon.FloatToIntRound(m_Position.X) + 60 / 2 - 1);
        m_Missile.SetTop(m_Position.Y);
        m_Missile.Alive = true;
        m_CollisionCoordinator.AddItem(m_Missile,true);
        
        m_FireSound.Play();
      }

      m_Missile.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      base.Draw(gameTime, spriteBatch);
      m_Missile.Draw(gameTime, spriteBatch);
    }

    #endregion

    public override List<string> MyCollisionTags
    {
      get
      {
        List<String> result = new List<string>();
        result.Add("SHIP");
        return result;
      }
    }

    public override void AfterCollide(Sprite other, Vector2 screenXY)
    {
      if (m_GotHit) return;
      if (other is InvaderMissile1 || other is InvaderMissile2)
      {
        m_GotHit = true;
        OnShipDead();
      }
    }

    internal void RegisterCollision(SpriteCollisionCoordinator collisionCoordinator)
    {
      m_CollisionCoordinator = collisionCoordinator;
      m_CollisionCoordinator.AddItem(this,false);
    }

    internal void Reset()
    {
      m_Missile.Alive = false;
      ResetLoop();
    }
  }
}
