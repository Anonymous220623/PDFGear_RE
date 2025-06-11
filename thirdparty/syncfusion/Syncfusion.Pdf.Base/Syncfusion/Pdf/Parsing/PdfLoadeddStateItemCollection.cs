// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadeddStateItemCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadeddStateItemCollection : PdfCollection
{
  public PdfLoadedStateItem this[int index]
  {
    get
    {
      return index >= 0 && index < this.Count ? (PdfLoadedStateItem) (this.List[index] as PdfLoadedCheckBoxItem) : throw new IndexOutOfRangeException(nameof (index));
    }
  }

  internal int IndexOf(PdfLoadedStateItem item) => this.List.IndexOf((object) item);

  internal void Add(PdfLoadedStateItem item)
  {
    if (item == null)
      throw new NullReferenceException(nameof (item));
    this.List.Add((object) item);
  }

  internal int Remove(PdfLoadedStateItem item)
  {
    int index = item != null ? this.List.IndexOf((object) item) : throw new NullReferenceException(nameof (item));
    this.List.RemoveAt(index);
    this.List.Remove((object) item);
    return index;
  }
}
