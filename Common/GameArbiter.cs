using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA.Common
{
  public class GameArbiter
  {
    private Dictionary<string, Scene> m_Scenes;
    private Dictionary<FullyCompareablePair<string, string>, string> m_SceneMap;
    private Scene m_CurrentScene;

    public GameArbiter()
    {
      m_Scenes = new Dictionary<string, Scene>();
      m_SceneMap = new Dictionary<FullyCompareablePair<string, string>, string>();
    }

    internal void Initialize(GraphicsDeviceManager m_Graphics, Game2D game2D)
    {
      foreach (Scene scene in m_Scenes.Values)
        scene.Initialize(m_Graphics, game2D);
    }

    public void AddScene(Scene scene)
    {
      string sceneName = scene.Name;
      if (m_Scenes.ContainsKey(sceneName))
        m_Scenes.Remove(sceneName);
      m_Scenes.Add(sceneName, scene);
    }

    public void AddTransition(Scene activeScene, Scene targetScene, string transitionWord)
    {
      if (!m_Scenes.ContainsKey(activeScene.Name) || !m_Scenes.ContainsKey(targetScene.Name))
        throw new ArgumentException("At least one of the given scenes were not added to the arbiter");
      FullyCompareablePair<string, string> key = new FullyCompareablePair<string, string>(activeScene.Name, transitionWord);
      if (m_SceneMap.ContainsKey(key))
        m_SceneMap.Remove(key);
      m_SceneMap.Add(key, targetScene.Name);
    }

    public void AddTransition(string activeSceneName, string targetSceneName, string transitionWord)
    {
      if (!m_Scenes.ContainsKey(activeSceneName) || !m_Scenes.ContainsKey(targetSceneName))
        throw new ArgumentException("At least one of the given scenes were not added to the arbiter");
      FullyCompareablePair<string, string> key = new FullyCompareablePair<string, string>(activeSceneName, transitionWord);
      if (m_SceneMap.ContainsKey(key))
        m_SceneMap.Remove(key);
      m_SceneMap.Add(key, targetSceneName);
    }

    public void Transition(string transitionWord, object transitionData)
    {
      FullyCompareablePair<string, string> mapping = new FullyCompareablePair<string, string>(CurrentScene.Name, transitionWord);
      if (m_SceneMap.ContainsKey(mapping))
      {
        Scene tempScene = GetScene(m_SceneMap[mapping]);
        tempScene.TransitionReset(transitionWord, transitionData);
        CurrentScene = tempScene;
      }
    }

    public void Transition(string transitionWord)
    {
      Transition(transitionWord, null);
    }

    public List<Scene> GetAllScenes()
    {
      List<Scene> result = new List<Scene>(m_Scenes.Values);
      return result;

    }

    public Scene GetScene(string sceneName)
    {
      if (m_Scenes.ContainsKey(sceneName))
        return m_Scenes[sceneName];
      return null;
    }

    public Scene CurrentScene
    {
      get { return m_CurrentScene; }
      internal set 
      {
        if (!m_Scenes.ContainsKey(value.Name))
          throw new ArgumentException("The given scene was not recognized by the arbiter");
        m_CurrentScene = value; 
      }
    }

    
  }
}
