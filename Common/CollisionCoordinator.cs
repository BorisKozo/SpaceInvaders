using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA.Common
{


  public class SpriteCollisionCoordinator
  {
    private class SpriteRecord
    {
      public Sprite Item{get;set;}
      public bool RemoveWhenDead { get; set; }
      public SpriteRecord(Sprite item, bool removeWhenDead)
      {
        Item = item;
        RemoveWhenDead = removeWhenDead;
      }

    }

    private SortedList<string, LinkedList<SpriteRecord>> m_Items;
    private LinkedList<SpriteRecord> m_AllItems;

    public SpriteCollisionCoordinator()
    {
      m_Items = new SortedList<string, LinkedList<SpriteRecord>>();
      m_AllItems = new LinkedList<SpriteRecord>();
    }

    public void AddItem(Sprite item,bool removeWhenDead)
    {
      List<string> tags = item.CollideWithTags;
      foreach (string tag in tags)
      {
        if (m_Items.ContainsKey(tag))
        {
          m_Items[tag].AddLast(new SpriteRecord(item,removeWhenDead));
        }
        else
        {
          LinkedList<SpriteRecord> innerItems = new LinkedList<SpriteRecord>();
          innerItems.AddLast(new SpriteRecord(item, removeWhenDead));
          m_Items.Add(tag, innerItems);
        }
      }

      m_AllItems.AddLast(new SpriteRecord(item,removeWhenDead));
    }

    public void Run()
    {
      LinkedListNode<SpriteRecord> currentItem = m_AllItems.First;
      LinkedListNode<SpriteRecord> tempNode = null;
      LinkedListNode<SpriteRecord> currentInnerItem = null;
      while (currentItem != null)
      {
        #region Remove dead elements from main list
        //Gets rid of dead elements
        if (currentItem.Value.RemoveWhenDead && !currentItem.Value.Item.Alive)
        {
          tempNode = currentItem;
          currentItem = currentItem.Next;
          m_AllItems.Remove(tempNode);
          continue;
        }
        #endregion
        if (!currentItem.Value.Item.Alive)
        {
          currentItem = currentItem.Next;
          continue;
        }
        //Check all the sprites that need to collide with the current sprite
        List<string> tagsToCheck = currentItem.Value.Item.MyCollisionTags;
        for (int i = 0; i < tagsToCheck.Count; i++)
        {
          string tag = tagsToCheck[i];
          if (!m_Items.ContainsKey(tag)) continue;
          LinkedList<SpriteRecord> targets = m_Items[tag];
          currentInnerItem = targets.First;
          while (currentInnerItem != null)
          {
            #region Remove dead elements from the inner list
            if (currentInnerItem.Value.RemoveWhenDead && !currentInnerItem.Value.Item.Alive)
            {
              tempNode = currentInnerItem;
              currentInnerItem = currentInnerItem.Next;
              targets.Remove(tempNode);
              if (targets.Count == 0)
              {
                m_Items.Remove(tag);
                break;
              }
              continue;
            }
            #endregion
            if (!currentInnerItem.Value.Item.Alive)
            {
              currentInnerItem = currentInnerItem.Next;
              continue;
            }

            #region Handle self collision
            if (currentItem == currentInnerItem)
            {
              currentInnerItem = currentInnerItem.Next;
              continue;
            }
            #endregion

            Vector2 collisionPoint = Sprite.SpritesCollide(currentInnerItem.Value.Item, currentItem.Value.Item);
            if (currentItem.Value.Item.Position.X == 100)
            {
            }

            if (float.IsNaN(collisionPoint.X))
            {
              currentInnerItem = currentInnerItem.Next;
              continue;
            }

            //Collision Detected!
            currentItem.Value.Item.BeforeCollide(currentInnerItem.Value.Item, collisionPoint);
            currentInnerItem.Value.Item.BeforeCollide(currentItem.Value.Item, collisionPoint);
            currentItem.Value.Item.Collide(currentInnerItem.Value.Item, collisionPoint);
            currentInnerItem.Value.Item.Collide(currentItem.Value.Item, collisionPoint);
            currentItem.Value.Item.AfterCollide(currentInnerItem.Value.Item, collisionPoint);
            currentInnerItem.Value.Item.AfterCollide(currentItem.Value.Item, collisionPoint);
            currentInnerItem = currentInnerItem.Next;
          }
        }

        currentItem = currentItem.Next;

      }
    }
  }
}
