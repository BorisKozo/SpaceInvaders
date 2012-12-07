using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
  public class FloatingNumber:Sprite
  {
    private int m_Number;
    private SpriteFont m_Font;
    private Color m_Color;
    public FloatingNumber(Vector2 position, int number, SpriteFont font,Color color):base("",position)
    {
      m_Number = number;
      m_Font = font;
      m_DeathTimer.TotalTimeSpan = TimeSpan.FromSeconds(3);
      m_DeathTimer.Active = true;
      m_Color = color;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      spriteBatch.DrawString(m_Font, m_Number.ToString(), Position, m_Color);
    }

  }
}
