using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace XNA.Common
{
  public class TextureImage
  {
    private Texture2D m_Image = null;
    private Color[,] m_ImageData = null;
    private readonly string m_AssetName;

    internal TextureImage(string assetName, ContentManager content, bool addToManager)
    {
      if (addToManager)
        TextureImageManager.Instance.GetTextureImage(assetName);
      m_Image = content.Load<Texture2D>(assetName);
      m_AssetName = assetName;

    }

    public TextureImage(string assetName, ContentManager content)
      : this(assetName, content, true)
    {
    }

    public Color ColorAtXY(int x, int y)
    {
      int position = x + y * m_Image.Width;
      if (!XYInRange(x,y)) 
        throw new ArgumentOutOfRangeException("x,y", "In ColorAtXY");
      return ImageData[x, y];
    }

    public bool XYInRange(int x, int y)
    {
      if (x < 0 || x >= ImageData.GetLength(0) || y < 0 || y >= ImageData.GetLength(1))
        return false;
      return true;
    }

    public Texture2D Image
    {
      get { return m_Image; }
    }

    public Color[,] ImageData
    {
      get
      {
        if (m_ImageData == null)
        {
          m_ImageData = TextureTo2DArray(m_Image);
          
        }
        return m_ImageData;
      }
    }

    public string AssetName
    {
      get { return m_AssetName; }
    }

    public static Color[,] TextureTo2DArray(Texture2D texture)
    {
      Color[] colors1D = new Color[texture.Width * texture.Height];
      texture.GetData(colors1D);

      Color[,] colors2D = new Color[texture.Width, texture.Height];
      for (int x = 0; x < texture.Width; x++)
        for (int y = 0; y < texture.Height; y++)
          colors2D[x, y] = colors1D[x + y * texture.Width];

      return colors2D;
    }

  }
}
