using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA.Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNA.Common
{
  public class HorizontalSelector<T>:IDisplayedItem
  {

    private int m_TotalSteps = 150;
    private TextureMover[] m_Images = new TextureMover[5];
    private int m_NextImage;
    private int m_SelectedIndex;

    private TextureButton m_LeftButton;
    private TextureButton m_RightButton;

    private int m_CycleCount;
    Vector2 m_InitialPosition;
    Vector2 m_FinalPosition;

    private Dictionary<TextureImage, T> m_Textures = new Dictionary<TextureImage, T>();

    public HorizontalSelector(Vector2 position, int width)
    {
      m_InitialPosition = position;
      m_FinalPosition = new Vector2(position.X+width, position.Y);
      m_CycleCount = 0;
    }

    public void LoadContent(ContentManager content,string leftButtonAsset, string rightButtonAsset, Dictionary<string,T> assetMap)
    {
      m_LeftButton = new TextureButton(leftButtonAsset, m_InitialPosition);
      m_RightButton = new TextureButton(rightButtonAsset, m_FinalPosition);

      m_RightButton.Position = new Vector2(m_FinalPosition.X - m_RightButton.NormalWidth, m_FinalPosition.Y); 
      m_LeftButton.Clicked += new TextureButtonClickedHandler(LeftButton_Clicked);
      m_RightButton.Clicked += new TextureButtonClickedHandler(RightButton_Clicked);

      foreach (string key in assetMap.Keys)
      {
        m_Textures.Add(TextureImageManager.Instance.GetTextureImage(key),assetMap[key]);
      }

      float minScale = 0.35f;
      float deltaX = (float)m_Textures.Keys.ElementAt(0).Image.Width * minScale;
      m_FinalPosition.X = m_FinalPosition.X - deltaX;


      for (int i = 0; i < 5; i++)
      {
        m_Images[i] = new TextureMover(m_InitialPosition, m_FinalPosition, m_TotalSteps);
        m_Images[i].Image = m_Textures.Keys.ElementAt(i % m_Textures.Keys.Count);
        m_Images[i].InitialScale = 1f;
        m_Images[i].MinimumScale = minScale;
        m_Images[i].CurrentStep = (75 + i * 30) % 150 + 1;
      }

      m_Images[4].Image = m_Textures.Keys.ElementAt(m_Textures.Keys.Count - 1);
      int nextImage = m_Textures.Keys.Count - 2;
      if (nextImage < 0)
        nextImage += m_Textures.Keys.Count;
      m_Images[3].Image = m_Textures.Keys.ElementAt(nextImage);
      if (m_Textures.Keys.Count == 2)
        m_NextImage = 1;
      else
        m_NextImage = 0;
      m_SelectedIndex = 0;
    }



    void RightButton_Clicked(object sender, TextureButtonClickedEventArgs args)
    {

      if (m_CycleCount >= 0)
      {
        m_CycleCount += 30;
        m_SelectedIndex = (m_SelectedIndex + 1) % m_Textures.Keys.Count;
          
      }
    }

    void LeftButton_Clicked(object sender, TextureButtonClickedEventArgs args)
    {
      if (m_CycleCount <= 0)
      {
        m_CycleCount -= 30;

        m_SelectedIndex = m_SelectedIndex == 0 ? m_Textures.Keys.Count - 1 : m_SelectedIndex - 1;
      }
    }

    public void Update(GameTime gameTime)
    {
      //Rotate right
      if (m_CycleCount < 0)
      {
        for (int i = 0; i < 5; i++)
        {
          m_Images[i].StepRight();
          if (m_Images[i].CurrentStep == m_TotalSteps)
          {
            int nextImage = m_NextImage - Math.Min(6, m_Textures.Keys.Count);
            if (nextImage < 0) nextImage += m_Textures.Count;
            m_Images[i].Image = m_Textures.Keys.ElementAt(nextImage % m_Textures.Keys.Count);
            m_NextImage = (m_NextImage - 1);
            if (m_NextImage < 0)
              m_NextImage = m_Textures.Count - 1;
          }
        }
        m_CycleCount++;
      }
      //Rotate Left
      if (m_CycleCount > 0)
      {
        for (int i = 0; i < 5; i++)
        {
          m_Images[i].StepLeft();
          if (m_Images[i].CurrentStep == 1)
          {
            m_Images[i].Image = m_Textures.Keys.ElementAt(m_NextImage % m_Textures.Keys.Count);
            m_NextImage = (m_NextImage + 1) % m_Textures.Count;
          }
        }

        m_CycleCount--;
      }

      for (int i = 0; i < 5; i++)
        m_Images[i].Update(gameTime);

      m_RightButton.Update(gameTime);
      m_LeftButton.Update(gameTime);

    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {

      spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
      for (int i = 0; i < 5; i++)
      {
        m_Images[i].Draw(gameTime, spriteBatch);
      }
      spriteBatch.End();
      m_RightButton.Draw(gameTime, spriteBatch);
      m_LeftButton.Draw(gameTime, spriteBatch);
    }

    public T SelectedItem
    {
      get
      {
        return m_Textures[m_Textures.Keys.ElementAt(m_SelectedIndex)];
      }
    }

  }
}
