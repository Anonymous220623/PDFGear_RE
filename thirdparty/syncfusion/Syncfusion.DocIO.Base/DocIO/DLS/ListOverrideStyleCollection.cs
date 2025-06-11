// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ListOverrideStyleCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class ListOverrideStyleCollection : StyleCollection
{
  internal ListOverrideStyleCollection(WordDocument doc)
    : base(doc)
  {
  }

  public ListOverrideStyle this[int index] => (ListOverrideStyle) this.InnerList[index];

  internal int Add(ListOverrideStyle listOverrideStyle)
  {
    listOverrideStyle.CloneRelationsTo(this.Document, (OwnerHolder) null);
    return this.InnerList.Add((object) listOverrideStyle);
  }

  public ListOverrideStyle FindByName(string name) => base.FindByName(name) as ListOverrideStyle;

  protected override OwnerHolder CreateItem(IXDLSContentReader reader)
  {
    return (OwnerHolder) new ListOverrideStyle(this.Document);
  }

  protected override string GetTagItemName() => "OverrideListStyle";

  internal bool HasEquivalentStyle(ListOverrideStyle listOverrideStyle)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index].Compare(listOverrideStyle))
        return true;
    }
    return false;
  }

  internal ListOverrideStyle GetEquivalentStyle(ListOverrideStyle listOverrideStyle)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      ListOverrideStyle equivalentStyle = this[index];
      if (equivalentStyle.Compare(listOverrideStyle))
        return equivalentStyle;
    }
    return (ListOverrideStyle) null;
  }
}
