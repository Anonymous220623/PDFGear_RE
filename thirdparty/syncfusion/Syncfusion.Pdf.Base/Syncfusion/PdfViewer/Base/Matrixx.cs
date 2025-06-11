// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Matrixx
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal struct Matrixx
{
  internal Matrixx Identity => new Matrixx(1.0, 0.0);

  internal double M11 { get; set; }

  internal double M12 { get; set; }

  internal Matrixx(double m11, double m12)
    : this()
  {
    this.M11 = m11;
    this.M12 = m12;
  }
}
