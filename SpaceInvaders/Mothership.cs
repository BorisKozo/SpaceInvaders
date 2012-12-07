using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace SpaceInvaders
{
  public class Mothership:AnimatedSprite
  {

    
    public Mothership(Vector2 position) :
      base("Images//saucer", position, 80)
    {
    }

    public override void LoadContent(ContentManager content)
    {
      base.LoadContent(content);
     
    }

    public override void Update(GameTime gameTime)
    {
      MoveHorizontal(1f);
      base.Update(gameTime);
      if (BoundingRect().Left > Game2D.CurrentGame.ScreenWidth)
        Alive = false;
    }

    internal void RegisterCollision(SpriteCollisionCoordinator CollisionCoordinator)
    {
      CollisionCoordinator.AddItem(this, false);
    }

    public override void Collide(Sprite other, Vector2 screenXY)
    {
      Alive = false;
      OnDead();
    }

    public override List<string> MyCollisionTags
    {
      get
      {
        List<string> result = new List<string>();
        result.Add("MOTHERSHIP");
        return result;
      }
    }

    public event InvaderDeadEventHandler Dead;
    internal virtual void OnDead()
    {
      int score = Game2D.GameRandom.Next(1, 10) * 10;
      if (Dead != null)
        Dead(this, new InvaderDeathEventArgs(score));
    }


  }
}
