// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedComboBoxItemCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedComboBoxItemCollection : PdfCollection
{
  public PdfLoadedComboBoxItem this[int index]
  {
    get
    {
      return index >= 0 && index < this.Count ? this.List[index] as PdfLoadedComboBoxItem : throw new IndexOutOfRangeException(nameof (index));
    }
  }

  internal void Add(PdfLoadedComboBoxItem item)
  {
    if (item == null)
      throw new NullReferenceException(nameof (item));
    this.List.Add((object) item);
  }

  internal int Remove(PdfLoadedComboBoxItem item)
  {
    int index = item != null ? this.List.IndexOf((object) item) : throw new NullReferenceException(nameof (item));
    if (this.List.Count != 0)
      this.List.RemoveAt(index);
    this.List.Remove((object) item);
    return index;
  }

  internal PdfLoadedComboBoxItemCollection Clone()
  {
    PdfLoadedComboBoxItemCollection boxItemCollection = new PdfLoadedComboBoxItemCollection();
    for (int index = 0; index < this.List.Count; ++index)
    {
      if (this.List[index] is PdfLoadedComboBoxItem)
        boxItemCollection.Add((this.List[index] as PdfLoadedComboBoxItem).Clone());
    }
    return boxItemCollection;
  }
}
