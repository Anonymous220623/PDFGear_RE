// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedRadioButtonItemCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedRadioButtonItemCollection : PdfLoadedStateItemCollection
{
  public PdfLoadedRadioButtonItem this[int index] => base[index] as PdfLoadedRadioButtonItem;

  internal int IndexOf(PdfLoadedRadioButtonItem item) => this.IndexOf((PdfLoadedStateItem) item);

  internal void Add(PdfLoadedRadioButtonItem item) => this.Add((PdfLoadedStateItem) item);

  internal int Remove(PdfLoadedRadioButtonItem item)
  {
    int index = item != null ? this.List.IndexOf((object) item) : throw new NullReferenceException(nameof (item));
    if (this.List.Count != 0)
      this.List.RemoveAt(index);
    this.List.Remove((object) item);
    this.Remove((PdfLoadedStateItem) item);
    return index;
  }

  internal PdfLoadedRadioButtonItemCollection Clone()
  {
    PdfLoadedRadioButtonItemCollection buttonItemCollection = new PdfLoadedRadioButtonItemCollection();
    for (int index = 0; index < this.List.Count; ++index)
    {
      if (this.List[index] is PdfLoadedRadioButtonItem)
        buttonItemCollection.Add((this.List[index] as PdfLoadedRadioButtonItem).Clone());
    }
    return buttonItemCollection;
  }
}
