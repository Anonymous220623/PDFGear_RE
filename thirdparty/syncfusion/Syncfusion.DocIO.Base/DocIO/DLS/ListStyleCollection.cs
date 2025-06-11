// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ListStyleCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class ListStyleCollection : XDLSSerializableCollection
{
  public ListStyle this[int index] => (ListStyle) this.InnerList[index];

  internal ListStyleCollection(WordDocument doc)
    : base(doc, (OwnerHolder) null)
  {
  }

  public int Add(ListStyle style)
  {
    if (style == null)
      throw new ArgumentNullException(nameof (style));
    style.CloneRelationsTo(this.Document, (OwnerHolder) null);
    return this.InnerList.Add((object) style);
  }

  public ListStyle FindByName(string name)
  {
    return StyleCollection.FindStyleByName(this.InnerList, name) as ListStyle;
  }

  protected override OwnerHolder CreateItem(IXDLSContentReader reader)
  {
    return (OwnerHolder) new ListStyle(this.Document);
  }

  protected override string GetTagItemName() => "style";

  internal bool HasEquivalentStyle(ListStyle listStyle)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index].Compare(listStyle))
        return true;
    }
    return false;
  }

  internal ListStyle GetEquivalentStyle(ListStyle listStyle)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      ListStyle equivalentStyle = this[index];
      if (equivalentStyle.Compare(listStyle))
        return equivalentStyle;
    }
    return (ListStyle) null;
  }

  internal bool HasSameListId(ListStyle currentListStyle)
  {
    foreach (ListStyle listStyle in (CollectionImpl) this)
    {
      if (listStyle.ListID == currentListStyle.ListID)
        return true;
    }
    return false;
  }
}
