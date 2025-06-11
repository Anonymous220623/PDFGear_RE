// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfExportAnnotationCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfExportAnnotationCollection : PdfCollection
{
  public PdfLoadedAnnotation this[int index]
  {
    get
    {
      int count = this.List.Count;
      return count >= 0 && index < count ? this.List[index] as PdfLoadedAnnotation : throw new IndexOutOfRangeException(nameof (index));
    }
  }

  public void Add(PdfLoadedAnnotation annotation)
  {
    if (annotation == null)
      throw new ArgumentNullException(nameof (annotation));
    this.List.Add((object) annotation);
  }
}
