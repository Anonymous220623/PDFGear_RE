// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WRowCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WRowCollection(WTable owner) : EntityCollection(owner.Document, (Entity) owner)
{
  private static readonly Type[] DEF_ELEMENT_TYPES = new Type[1]
  {
    typeof (WTableRow)
  };

  public WTableRow this[int index] => this.InnerList[index] as WTableRow;

  protected override Type[] TypesOfElement => WRowCollection.DEF_ELEMENT_TYPES;

  public int Add(WTableRow row) => this.Add((IEntity) row);

  public void Insert(int index, WTableRow row) => this.Insert(index, (IEntity) row);

  public int IndexOf(WTableRow row) => this.IndexOf((IEntity) row);

  public void Remove(WTableRow row) => this.Remove((IEntity) row);

  protected override string GetTagItemName() => "row";

  protected override OwnerHolder CreateItem(IXDLSContentReader reader)
  {
    return (OwnerHolder) new WTableRow((IWordDocument) this.Document);
  }
}
