// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.RelationCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization;

internal class RelationCollection : IEnumerable, ICloneable
{
  private const string RelationIdStart = "rId";
  private static readonly int RelationIdStartLen = "rId".Length;
  private Dictionary<string, Relation> m_dicRelations = new Dictionary<string, Relation>();
  private string m_strItemPath;

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

  public Relation FindRelationByContentType(string contentType, out string relationId)
  {
    Relation relationByContentType = (Relation) null;
    relationId = (string) null;
    if (contentType != null && contentType.Length > 0)
    {
      foreach (KeyValuePair<string, Relation> dicRelation in this.m_dicRelations)
      {
        Relation relation = dicRelation.Value;
        if (relation.Type == contentType)
        {
          relationByContentType = relation;
          relationId = dicRelation.Key;
          break;
        }
      }
    }
    return relationByContentType;
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

  public string Add(Relation relation)
  {
    string empty = string.Empty;
    if (relation == null)
      return empty;
    string relationId = this.GenerateRelationId();
    this[relationId] = relation;
    return relationId;
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
