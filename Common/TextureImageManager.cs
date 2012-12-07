using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace XNA.Common
{
  public class TextureImageManager
  {
    private Dictionary<string, TextureImage> m_Textures = new Dictionary<string, TextureImage>();
    private ContentManager m_Content;

    #region Singeltone Management
    private static TextureImageManager m_Instance = new TextureImageManager();
    private TextureImageManager()
    {

    }

    static TextureImageManager()
    {

    }

    public static TextureImageManager Instance
    {
      get { return m_Instance; }
    }

    #endregion

    public void LoadContent(ContentManager content)
    {
      m_Content = content;
    }

    public TextureImage GetTextureImage(string assetName)
    {
      if (!m_Textures.ContainsKey(assetName))
        m_Textures.Add(assetName,new TextureImage(assetName,m_Content,false));
      return m_Textures[assetName];
    }
  }
}
