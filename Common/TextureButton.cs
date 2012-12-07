using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace XNA.Common
{

  public class TextureButtonClickedEventArgs : EventArgs
  {
    private MouseState m_State;

    public MouseState State
    {
      get { return m_State; }
      set { m_State = value; }
    }

    public TextureButtonClickedEventArgs(MouseState state)
    {
      m_State = state;
    }

  }

  public delegate void TextureButtonClickedHandler(object sender, TextureButtonClickedEventArgs args);

  public class TextureButton : IDisplayedItem
  {
    private TextureImage m_NormalTexture;
    private TextureImage m_HoverTexture;
    private TextureImage m_PressedTexture;

    private TextureImage m_NormalTextureMask;
    private TextureImage m_CurrentImage;

    private Vector2 m_Position;


    private bool m_ClickBlocked = false;

    public event TextureButtonClickedHandler Clicked;
    /// <summary>
    /// When creating the button you provide the asset name i.e. myButton
    /// and the button loads the assets myButton_normal, myButton_hover, myButton_pressed
    /// and myButton_normalMask
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="position">The top left corner of the button</param>
    public TextureButton(string assetName, Vector2 position)
    {

      m_NormalTexture = TextureImageManager.Instance.GetTextureImage(assetName+"_normal");
      m_HoverTexture = TextureImageManager.Instance.GetTextureImage(assetName+"_hover");
      m_PressedTexture = TextureImageManager.Instance.GetTextureImage(assetName+"_pressed");
      m_NormalTextureMask = TextureImageManager.Instance.GetTextureImage(assetName+"_normalMask");

      m_Position = position;
    }

    private bool IsMaskHit(MouseState state,TextureImage maskImage)
    {
      int top = Convert.ToInt32(Math.Floor(m_Position.Y));
      int left = Convert.ToInt32(Math.Floor(m_Position.X));
      if (state.X < left || state.Y < top || state.X >= left + maskImage.Image.Width || state.Y >= top + maskImage.Image.Height)
        return false;
      Color tempColor = maskImage.ColorAtXY(state.X - left, state.Y - top);
      return (tempColor.A != 0);
    }

    public void Update(GameTime gameTime)
    {
      m_CurrentImage = m_NormalTexture;
      MouseState state = Mouse.GetState();
      if (IsMaskHit(state, m_NormalTextureMask))
      {
        if (state.LeftButton == ButtonState.Pressed)
        {
          m_CurrentImage = m_PressedTexture;
          if (!m_ClickBlocked)
            OnClicked(state);
          m_ClickBlocked = true;
        }
        else
        {
          m_CurrentImage = m_HoverTexture;
          m_ClickBlocked = false;
        }
      }
      //else
      //  m_ClickBlocked = false;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      spriteBatch.Draw(m_CurrentImage.Image, m_Position, Color.White);
      spriteBatch.End();
    }

    public void OnClicked(MouseState state)
    {
      if (Clicked != null)
        Clicked(this, new TextureButtonClickedEventArgs(state));
    }

    public Vector2 Position
    {
      get { return m_Position; }
      set { m_Position = value; }
    }

    public int NormalWidth
    {
      get
      {
        return m_NormalTexture.Image.Width;
      }
    }

    public int NormalHeight
    {
      get
      {
        return m_NormalTexture.Image.Height;
      }
    }
  }
}
