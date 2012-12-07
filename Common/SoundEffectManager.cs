using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace XNA.Common
{
  public class SoundEffectManager
  {
    private Dictionary<string, SoundEffect> m_Sounds = new Dictionary<string, SoundEffect>();
    private ContentManager m_Content;

    #region Singeltone Management
    private static SoundEffectManager m_Instance = new SoundEffectManager();
    private SoundEffectManager()
    {

    }

    static SoundEffectManager()
    {

    }

    public static SoundEffectManager Instance
    {
      get { return m_Instance; }
    }

    #endregion

    public void LoadContent(ContentManager content)
    {
      m_Content = content;
    }

    public SoundEffectInstance GetSoundEffect(string assetName)
    {
      
      if (!m_Sounds.ContainsKey(assetName))
        m_Sounds.Add(assetName, m_Content.Load<SoundEffect>(assetName));
      return  m_Sounds[assetName].CreateInstance();
    }
  }
}
