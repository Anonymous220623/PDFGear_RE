// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Redaction.PdfRedactionResult
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Redaction;

public class PdfRedactionResult
{
  private RectangleF redactionBounds;
  private bool isRedactionSuccess;
  private int pageNumber;

  internal PdfRedactionResult(int pageNumber, bool success, RectangleF bounds)
  {
    this.pageNumber = pageNumber;
    this.redactionBounds = bounds;
    this.isRedactionSuccess = success;
  }

  public RectangleF RedactionBounds => this.redactionBounds;

  public bool IsRedactionSuccess => this.isRedactionSuccess;

  public int PageNumber => this.pageNumber;
}
