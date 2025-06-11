// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.RelationCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

public class RelationCollection : IEnumerable, ICloneable
{
  private const string RelationIdStart = "rId";
  private static readonly int RelationIdStartLen = "rId".Length;
  private Dictionary<string, Relation> m_dicRelations = new Dictionary<string, Relation>();
  private string m_strItemPath;
  private int m_hyperlinkCount;

  public Relation this[string id]
  {
    get
    {
      Relation relation;
      this.m_dicRelations.TryGetValue(id, out relation);
      return relation;
    }
    set => this.m_dicRelations[id] = value;
  }

  public int Count => this.m_dicRelations.Count;

  public string ItemPath
  {
    get => this.m_strItemPath;
    set => this.m_strItemPath = value;
  }

  internal Dictionary<string, Relation> DicRelations => this.m_dicRelations;

  public void Remove(string id) => this.m_dicRelations.Remove(id);

  public void RemoveByContentType(string contentType)
  {
    if (contentType == null || contentType.Length <= 0)
      return;
    foreach (KeyValuePair<string, Relation> dicRelation in this.m_dicRelations)
    {
      if (dicRelation.Value.Type == contentType)
      {
        this.m_dicRelations.Remove(dicRelation.Key);
        break;
      }
    }
  }

  internal void RemoveRelationByTarget(string targetName)
  {
    for (string relationByTarget = this.FindRelationByTarget(targetName); relationByTarget != null; relationByTarget = this.FindRelationByTarget(targetName))
      this.Remove(relationByTarget);
  }

  public Relation FindRelationByContentType(string contentType, out string relationId)
  {
    Relation relationByContentType = (Relation) null;
    relationId = (string) null;
    if (contentType != null && contentType.Length > 0)
    {
      foreach (KeyValuePair<string, Relation> dicRelation in this.m_dicRelations)
      {
        Relation relation = dicRelation.Value;
        if (relation != null && relation.Type == contentType)
        {
          relationByContentType = relation;
          relationId = dicRelation.Key;
          break;
        }
      }
    }
    return relationByContentType;
  }

  internal int CalculatePivotTableRelationInWorkbook(string contentType)
  {
    int relationInWorkbook = 0;
    if (contentType != null && contentType.Length > 0)
    {
      foreach (KeyValuePair<string, Relation> dicRelation in this.m_dicRelations)
      {
        Relation relation = dicRelation.Value;
        if (relation != null && relation.Type == contentType)
          ++relationInWorkbook;
      }
    }
    return relationInWorkbook;
  }

  public string FindRelationByTarget(string itemName)
  {
    string relationByTarget = (string) null;
    if (itemName != null && itemName.Length > 0)
    {
      foreach (KeyValuePair<string, Relation> dicRelation in this.m_dicRelations)
      {
        Relation relation = dicRelation.Value;
        if (relation != null && relation.Target == itemName)
        {
          relationByTarget = dicRelation.Key;
          break;
        }
      }
    }
    return relationByTarget;
  }

  public string GenerateRelationId()
  {
    string key = (string) null;
    for (int index = 1; index < int.MaxValue; ++index)
    {
      key = "rId" + (object) index;
      if (!this.m_dicRelations.ContainsKey(key))
        break;
    }
    return key;
  }

  internal string GenerateHyperlinkRelationId()
  {
    string key = (string) null;
    do
    {
      if (this.m_hyperlinkCount < int.MaxValue)
      {
        key = "rId" + (object) (this.m_hyperlinkCount + 1);
        ++this.m_hyperlinkCount;
      }
    }
    while (this.m_dicRelations.ContainsKey(key));
    return key;
  }

  public string Add(Relation relation)
  {
    string empty = string.Empty;
    if (relation == null)
      return empty;
    string relationId = this.GenerateRelationId();
    this[relationId] = relation;
    return relationId;
  }

  internal string Add(Relation relation, int index)
  {
    string str = string.Empty;
    bool flag = true;
    int num = 1;
    if (relation == null)
      return str;
    foreach (string key in this.m_dicRelations.Keys)
    {
      if (num == index)
      {
        str = key;
        break;
      }
      ++num;
    }
    if (str != string.Empty)
    {
      for (int index1 = 1; index1 <= this.m_dicRelations.Count; ++index1)
      {
        if (this.m_dicRelations[str] != null && this.m_dicRelations[str].Target.Equals(relation.Target.Replace("/xl", "..")))
          flag = false;
      }
    }
    if (flag)
    {
      str = this.GenerateRelationId();
      this[str] = relation;
    }
    return str;
  }

  internal string Add(Relation relation, string strId)
  {
    if (relation == null)
      return strId;
    if (string.IsNullOrEmpty(strId))
      strId = this.GenerateRelationId();
    this[strId] = relation;
    return strId;
  }

  public void Clear() => this.m_dicRelations.Clear();

  public RelationCollection Clone()
  {
    RelationCollection relationCollection = (RelationCollection) this.MemberwiseClone();
    relationCollection.m_dicRelations = CloneUtils.CloneHash<string, Relation>(this.m_dicRelations);
    return relationCollection;
  }

  object ICloneable.Clone() => (object) this.Clone();

  public void Dispose()
  {
    this.Clear();
    this.m_dicRelations = (Dictionary<string, Relation>) null;
  }

  public IEnumerator GetEnumerator() => (IEnumerator) this.m_dicRelations.GetEnumerator();
}
