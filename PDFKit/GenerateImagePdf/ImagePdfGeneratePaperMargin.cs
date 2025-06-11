// Decompiled with JetBrains decompiler
// Type: PDFKit.GenerateImagePdf.ImagePdfGeneratePaperMargin
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

#nullable disable
namespace PDFKit.GenerateImagePdf;

public struct ImagePdfGeneratePaperMargin
{
  public LengthUnit Left;
  public LengthUnit Top;
  public LengthUnit Right;
  public LengthUnit Bottom;

  public ImagePdfGeneratePaperMargin(
    LengthUnit left,
    LengthUnit top,
    LengthUnit right,
    LengthUnit bottom)
  {
    this.Left = left;
    this.Top = top;
    this.Right = right;
    this.Bottom = bottom;
  }

  public ImagePdfGeneratePaperMargin(LengthUnit topAndBottom, LengthUnit leftAndRight)
  {
    this.Left = this.Right = leftAndRight;
    this.Top = this.Bottom = topAndBottom;
  }

  public ImagePdfGeneratePaperMargin(LengthUnit uniformLength)
  {
    this.Left = this.Top = this.Right = this.Bottom = uniformLength;
  }
}
