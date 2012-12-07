using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA.Common;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
  public class Explosion: AnimatedSprite
  {
    public Explosion(Vector2 position)
      :base("Images//explosion",position,71)
    {
      AnimationSpeed = TimeSpan.FromMilliseconds(50);
      LoopType = AnimatedSpriteLoopType.None;
    }

    public Explosion(Explosion other)
      :base(other)
    {
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (AnimationStopped) Alive = false;
    }

  }
}
