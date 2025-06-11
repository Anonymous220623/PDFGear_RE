// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedCheckBoxItemCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedCheckBoxItemCollection : PdfLoadedStateItemCollection
{
  public PdfLoadedCheckBoxItem this[int index] => base[index] as PdfLoadedCheckBoxItem;

  internal int IndexOf(PdfLoadedCheckBoxItem item) => this.IndexOf((PdfLoadedStateItem) item);

  internal void Add(PdfLoadedCheckBoxItem item) => this.Add((PdfLoadedStateItem) item);

  internal int Remove(PdfLoadedCheckBoxItem item)
  {
    int index = item != null ? this.List.IndexOf((object) item) : throw new NullReferenceException(nameof (item));
    if (this.List.Count != 0)
      this.List.RemoveAt(index);
    this.List.Remove((object) item);
    this.Remove((PdfLoadedStateItem) item);
    return index;
  }

  internal PdfLoadedCheckBoxItemCollection Clone()
  {
    PdfLoadedCheckBoxItemCollection boxItemCollection = new PdfLoadedCheckBoxItemCollection();
    for (int index = 0; index < this.List.Count; ++index)
    {
      if (this.List[index] is PdfLoadedCheckBoxItem)
        boxItemCollection.Add((this.List[index] as PdfLoadedCheckBoxItem).Clone());
    }
    return boxItemCollection;
  }
}
