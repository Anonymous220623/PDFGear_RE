// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ColumnCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class ColumnCollection : XDLSSerializableCollection
{
  public Column this[int index] => (Column) this.InnerList[index];

  internal WSection OwnerSection => this.OwnerBase as WSection;

  internal ColumnCollection(WSection section)
    : base(section.Document, (OwnerHolder) section)
  {
  }

  public int Add(Column column) => this.Add(column, false);

  public void Populate(int count, float spacing)
  {
    float num = this.OwnerSection.PageSetup.ClientWidth / (float) count - spacing;
    this.InnerList.Clear();
    for (int index = 0; index < count; ++index)
      this.Add(new Column((IWordDocument) this.Document)
      {
        Width = num,
        Space = spacing
      }, true);
  }

  internal int Add(Column column, bool isOpening)
  {
    if (!isOpening && this.OwnerBase is WSection)
      (this.OwnerBase as WSection).PageSetup.EqualColumnWidth = false;
    column.SetOwner(this.OwnerBase);
    return this.InnerList.Add((object) column);
  }

  internal void CloneTo(ColumnCollection coll)
  {
    int index = 0;
    for (int count = this.InnerList.Count; index < count; ++index)
    {
      Column inner = this.InnerList[index] as Column;
      coll.Add(inner.Clone(), true);
    }
  }

  protected override OwnerHolder CreateItem(IXDLSContentReader reader)
  {
    return (OwnerHolder) new Column((IWordDocument) this.Document);
  }

  protected override string GetTagItemName() => "column";
}
