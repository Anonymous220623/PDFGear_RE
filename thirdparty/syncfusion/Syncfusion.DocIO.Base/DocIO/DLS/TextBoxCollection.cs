// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TextBoxCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class TextBoxCollection : CollectionImpl
{
  internal TextBoxCollection(WordDocument doc)
    : base(doc, (OwnerHolder) doc)
  {
  }

  public WTextBox this[int index] => this.InnerList[index] as WTextBox;

  public void RemoveAt(int index)
  {
    WTextBox inner = this.InnerList[index] as WTextBox;
    inner.OwnerParagraph.Items.Remove((IEntity) inner);
  }

  public void Clear()
  {
    while (this.InnerList.Count > 0)
      this.RemoveAt(this.InnerList.Count - 1);
  }

  internal void Add(WTextBox textbox) => this.InnerList.Add((object) textbox);

  internal void Remove(WTextBox textbox) => this.InnerList.Remove((object) textbox);
}
