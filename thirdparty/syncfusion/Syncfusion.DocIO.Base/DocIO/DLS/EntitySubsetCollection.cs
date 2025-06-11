// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.EntitySubsetCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public abstract class EntitySubsetCollection : IEntityCollectionBase, ICollectionBase, IEnumerable
{
  private EntityCollection m_coll;
  private EntityType m_type;
  private int m_lastIndex = -1;
  private int m_lastBaseIndex = -1;
  private int m_count;

  public WordDocument Document => this.m_coll.Document;

  public Entity Owner => this.m_coll.Owner;

  public int Count => this.m_count;

  public Entity this[int index]
  {
    get
    {
      if (this.m_coll.Count < 1)
        throw new ArgumentOutOfRangeException(nameof (index));
      this.ClearIndexes();
      return this.GetByIndex(index);
    }
  }

  internal EntitySubsetCollection(EntityCollection coll, EntityType type)
  {
    this.m_coll = coll;
    this.m_type = type;
    this.UpdateCount();
    coll.ChangeItemsHandlers.Add(new EntityCollection.ChangeItems(this.BaseCollChangeItems));
  }

  public void Clear()
  {
    this.m_coll.InternalClearBy(this.m_type);
    this.m_count = 0;
    this.m_lastIndex = -1;
    this.m_lastBaseIndex = -1;
  }

  internal void Close()
  {
    if (this.m_coll == null)
      return;
    this.m_coll.Close();
    this.m_coll = (EntityCollection) null;
  }

  public IEnumerator GetEnumerator()
  {
    return (IEnumerator) new EntitySubsetCollection.SubSetEnumerator(this);
  }

  internal int InternalAdd(Entity entity)
  {
    this.CheckType(entity);
    this.m_coll.Add((IEntity) entity);
    return this.m_count - 1;
  }

  internal bool InternalContains(Entity entity)
  {
    this.CheckType(entity);
    return this.m_coll.Contains((IEntity) entity);
  }

  internal int InternalIndexOf(Entity entity)
  {
    this.CheckType(entity);
    this.ClearIndexes();
    for (int index = 0; index < this.Count; ++index)
    {
      if (this.GetByIndex(index) == entity)
        return index;
    }
    return -1;
  }

  internal int InternalInsert(int index, Entity entity)
  {
    int baseIndex = this.GetBaseIndex(index);
    this.m_coll.Insert(index, (IEntity) entity);
    return baseIndex + 1;
  }

  internal void InternalRemove(Entity entity)
  {
    this.CheckType(entity);
    this.m_coll.Remove((IEntity) entity);
  }

  internal void InternalRemoveAt(int index) => this.m_coll.RemoveAt(this.GetBaseIndex(index));

  protected Entity GetByIndex(int index)
  {
    if (this.m_lastBaseIndex < 0 || index == this.m_lastIndex)
    {
      this.m_lastBaseIndex = this.GetBaseIndex(index);
      this.m_lastIndex = index;
    }
    else
    {
      bool next = this.m_lastIndex < index;
      for (; index != this.m_lastIndex; this.m_lastIndex += next ? 1 : -1)
        this.m_lastBaseIndex = this.m_coll.GetNextOrPrevIndex(this.m_lastBaseIndex, this.m_type, next);
    }
    return this.m_coll[this.m_lastBaseIndex];
  }

  private int GetBaseIndex(int index)
  {
    int num = 0;
    int index1 = 0;
    for (int count = this.m_coll.Count; index1 < count; ++index1)
    {
      if (this.m_coll[index1].EntityType == this.m_type)
      {
        if (num == index)
          return index1;
        ++num;
      }
    }
    return -1;
  }

  private void UpdateCount()
  {
    int index = -1;
    this.m_count = 0;
    while (true)
    {
      index = this.m_coll.GetNextOrPrevIndex(index, this.m_type, true);
      if (index >= 0)
        ++this.m_count;
      else
        break;
    }
  }

  private void CheckType(Entity entity)
  {
    if (entity == null)
      throw new ArgumentNullException(nameof (entity));
    if (entity.EntityType != this.m_type)
      throw new ArgumentException("Invalid entity type");
  }

  private void BaseCollChangeItems(EntityCollection.ChangeItemsType type, Entity entity)
  {
    switch (type)
    {
      case EntityCollection.ChangeItemsType.Add:
        if (entity.EntityType != this.m_type)
          break;
        ++this.m_count;
        break;
      case EntityCollection.ChangeItemsType.Remove:
        if (entity.EntityType != this.m_type)
          break;
        --this.m_count;
        break;
      case EntityCollection.ChangeItemsType.Clear:
        this.m_count = 0;
        break;
    }
  }

  internal void ClearIndexes()
  {
    this.m_lastIndex = -1;
    this.m_lastBaseIndex = -1;
  }

  public class SubSetEnumerator : IEnumerator
  {
    private int m_currIndex = -1;
    private EntitySubsetCollection m_enColl;

    public SubSetEnumerator(EntitySubsetCollection enColl) => this.m_enColl = enColl;

    public object Current
    {
      get => this.m_currIndex < 0 ? (object) null : (object) this.m_enColl.m_coll[this.m_currIndex];
    }

    public bool MoveNext()
    {
      int nextOrPrevIndex = this.m_enColl.m_coll.GetNextOrPrevIndex(this.m_currIndex, this.m_enColl.m_type, true);
      if (nextOrPrevIndex < 0)
        return false;
      this.m_currIndex = nextOrPrevIndex;
      return true;
    }

    public void Reset() => this.m_currIndex = -1;
  }
}
