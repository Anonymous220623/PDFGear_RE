// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WTableCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WTableCollection(BodyItemCollection bodyItems) : 
  EntitySubsetCollection((EntityCollection) bodyItems, EntityType.Table),
  IWTableCollection,
  IEntityCollectionBase,
  ICollectionBase,
  IEnumerable
{
  public IWTable this[int index]
  {
    get
    {
      this.ClearIndexes();
      return this.GetByIndex(index) as IWTable;
    }
  }

  internal ITextBody OwnerTextBody => this.Owner as ITextBody;

  public int Add(IWTable table) => this.InternalAdd((Entity) table);

  public bool Contains(IWTable table) => this.InternalContains((Entity) table);

  public int IndexOf(IWTable table) => this.InternalIndexOf((Entity) table);

  public int Insert(int index, IWTable table) => this.InternalInsert(index, (Entity) table);

  public void Remove(IWTable table) => this.InternalRemove((Entity) table);

  public void RemoveAt(int index) => this.InternalRemoveAt(index);
}
