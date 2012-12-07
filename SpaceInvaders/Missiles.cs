using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA.Common;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
  public class ShipMissile : Sprite
  {

    private float m_Speed = -7f;

    public ShipMissile(Vector2 initialPosition):base("Images//missile",initialPosition)
    {
    }

    public override void Update(GameTime gameTime)
    {
      if (!Alive) return;
      Rectangle boundingRect = BoundingRect();
      if ((boundingRect.Bottom + m_Speed < 0))
        Alive = false;
      MoveVertical(m_Speed);
    }

    public override List<string> CollideWithTags
    {
      get
      {
        List<String> result = new List<string>();
        result.Add("INVADER");
        result.Add("SHIELD");
        result.Add("MOTHERSHIP");
        return result;
      }
    }

    public override void Collide(Sprite other, Vector2 screenXY)
    {
      if (other is InvaderBase)
      {
        Alive = false;
        (other as InvaderBase).OnDead();
        return;
      }

      if (other is Mothership)
      {
        Alive = false;

      }

    }
  }

  public class InvaderMissile1 : Sprite
  {

    private float m_Speed = 4f;
    public InvaderMissile1(Vector2 position) :
      base("Images//InvaderMissile1", position)
    {
    }

    public override void Update(GameTime gameTime)
    {
      if (!Alive) return;
      Rectangle boundingRect = BoundingRect();
      if ((boundingRect.Top + m_Speed > Game2D.CurrentGame.ScreenHeight))
        Alive = false;
      MoveVertical(m_Speed);
    }

    public override List<string> CollideWithTags
    {
      get
      {
        List<String> result = new List<string>();
        result.Add("SHIP");
        result.Add("SHIELD");
        return result;
      }
    }
  }

  public class InvaderMissile2 : AnimatedSprite
  {

    private float m_Speed = 4f;
    public InvaderMissile2(Vector2 position) :
      base("Images//InvaderMissile2", position, 5)
    {
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (!Alive) return;
      Rectangle boundingRect = BoundingRect();
      if ((boundingRect.Top + m_Speed > Game2D.CurrentGame.ScreenHeight))
        Alive = false;
      MoveVertical(m_Speed);
    }

    public override List<string> CollideWithTags
    {
      get
      {
        List<String> result = new List<string>();
        result.Add("SHIP");
        result.Add("SHIELD");
        return result;
      }
    }
  }

}
