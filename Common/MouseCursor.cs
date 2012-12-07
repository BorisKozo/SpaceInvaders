using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace XNA.Common
{
  public class MouseCursor : Sprite
  {
    private Vector2 m_HotSpotOffset;

    public MouseCursor(string assetName, Vector2 hotSpotOffset):base(assetName, new Vector2())
    {
      m_HotSpotOffset = hotSpotOffset;
    }

    public MouseCursor(string assetName):this(assetName,new Vector2())
    {
    }

    public override void LoadContent(ContentManager content)
    {
      base.LoadContent(content);
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      MouseState state = Mouse.GetState();
      SetLeft(state.X);
      SetTop(state.Y);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      base.Draw(gameTime, spriteBatch);
    }

    public Vector2 HotSpot
    {
      get { return new Vector2(Position.X + m_HotSpotOffset.X, Position.Y + m_HotSpotOffset.Y); }
    }
  }
}
