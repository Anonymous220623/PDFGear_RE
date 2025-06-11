// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WDropDownCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WDropDownCollection(WordDocument doc) : XDLSSerializableCollection(doc, (OwnerHolder) null)
{
  public WDropDownItem this[int index] => (WDropDownItem) this.InnerList[index];

  public WDropDownItem Add(string text)
  {
    if (this.InnerList.Count > 24)
      throw new ArgumentOutOfRangeException("InnerList", "You can have no more than 25 items in your drop-down list box");
    WDropDownItem wdropDownItem = new WDropDownItem((IWordDocument) this.Document);
    wdropDownItem.Text = text;
    this.InnerList.Add((object) wdropDownItem);
    return wdropDownItem;
  }

  public void Remove(int index)
  {
    if (index >= this.InnerList.Count)
      throw new ArgumentException("DropDownItem with such index doesn't exist.");
    this.InnerList.Remove((object) (WDropDownItem) this.InnerList[index]);
  }

  public void Clear() => this.InnerList.Clear();

  internal int Add(WDropDownItem item) => this.InnerList.Add((object) item);

  internal void CloneTo(WDropDownCollection destColl)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      destColl.Add(this[index].Clone());
  }

  protected override OwnerHolder CreateItem(IXDLSContentReader reader)
  {
    return (OwnerHolder) new WDropDownItem((IWordDocument) this.Document);
  }

  protected override string GetTagItemName() => "dropdown-items";

  internal override void Close()
  {
    if (this.InnerList != null && this.InnerList.Count > 0)
    {
      while (this.InnerList.Count > 0)
      {
        if (this.InnerList[0] is WDropDownItem inner)
        {
          if (inner.OwnerBase == null)
            this.InnerList.Remove((object) inner);
          else
            this.Remove(0);
          inner.Close();
        }
      }
    }
    base.Close();
  }
}
