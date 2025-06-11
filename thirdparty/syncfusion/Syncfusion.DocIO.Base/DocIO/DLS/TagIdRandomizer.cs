// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TagIdRandomizer
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class TagIdRandomizer
{
  [ThreadStatic]
  private static Random m_instance;
  [ThreadStatic]
  private static List<int> m_ids;
  [ThreadStatic]
  private static List<int> m_noneChangeIds;
  [ThreadStatic]
  private static Dictionary<int, int> m_changedIds;

  internal static Random Instance
  {
    get
    {
      if (TagIdRandomizer.m_instance == null)
        TagIdRandomizer.m_instance = new Random(1000);
      return TagIdRandomizer.m_instance;
    }
  }

  internal static Dictionary<int, int> ChangedIds
  {
    get
    {
      if (TagIdRandomizer.m_changedIds == null)
        TagIdRandomizer.m_changedIds = new Dictionary<int, int>();
      return TagIdRandomizer.m_changedIds;
    }
  }

  internal static List<int> Identificators
  {
    get
    {
      if (TagIdRandomizer.m_ids == null)
        TagIdRandomizer.m_ids = new List<int>();
      return TagIdRandomizer.m_ids;
    }
  }

  internal static List<int> NoneChangeIds
  {
    get
    {
      if (TagIdRandomizer.m_noneChangeIds == null)
        TagIdRandomizer.m_noneChangeIds = new List<int>();
      return TagIdRandomizer.m_noneChangeIds;
    }
  }

  internal static int GetId(int currentId)
  {
    if (TagIdRandomizer.NoneChangeIds.Contains(currentId))
      return currentId;
    int newId;
    if (!TagIdRandomizer.ChangedIds.ContainsKey(currentId))
    {
      newId = TagIdRandomizer.Instance.Next();
      TagIdRandomizer.ChangedIds.Add(currentId, newId);
      TagIdRandomizer.Identificators.Add(newId);
    }
    else
    {
      newId = TagIdRandomizer.ChangedIds[currentId];
      if (TagIdRandomizer.IsValidId(newId))
      {
        TagIdRandomizer.Identificators.Add(newId);
      }
      else
      {
        newId = TagIdRandomizer.Instance.Next();
        TagIdRandomizer.Identificators.Add(newId);
      }
    }
    return newId;
  }

  private static bool IsValidId(int newId)
  {
    bool flag = true;
    if (TagIdRandomizer.m_ids != null && TagIdRandomizer.m_ids.Count > 0)
    {
      foreach (int id in TagIdRandomizer.m_ids)
      {
        if (id == newId)
        {
          flag = false;
          break;
        }
      }
    }
    return flag;
  }

  internal static int GetMarkerId(int currentId, bool newId)
  {
    if (TagIdRandomizer.NoneChangeIds.Contains(currentId))
      return currentId;
    if (TagIdRandomizer.ChangedIds.ContainsKey(currentId) && !newId)
      return TagIdRandomizer.ChangedIds[currentId];
    int markerId = TagIdRandomizer.Instance.Next();
    if (!TagIdRandomizer.ChangedIds.ContainsKey(currentId))
      TagIdRandomizer.ChangedIds.Add(currentId, markerId);
    else
      TagIdRandomizer.ChangedIds[currentId] = markerId;
    return markerId;
  }
}
