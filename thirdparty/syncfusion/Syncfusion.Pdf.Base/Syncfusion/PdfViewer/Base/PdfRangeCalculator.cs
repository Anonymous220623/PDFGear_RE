// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.PdfRangeCalculator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class PdfRangeCalculator
{
  public int RangeStart { get; private set; }

  public int RangeEnd { get; private set; }

  public PdfRangeCalculator() => this.RangeStart = this.RangeEnd = 0;

  public PdfRangeCalculator(int start, int end)
  {
    this.RangeStart = start;
    this.RangeEnd = end;
  }

  public bool IsInRange(int value) => this.RangeStart <= value && value <= this.RangeEnd;
}
