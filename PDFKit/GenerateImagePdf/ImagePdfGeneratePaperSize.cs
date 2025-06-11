// Decompiled with JetBrains decompiler
// Type: PDFKit.GenerateImagePdf.ImagePdfGeneratePaperSize
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace PDFKit.GenerateImagePdf;

public class ImagePdfGeneratePaperSize
{
  private static IReadOnlyDictionary<string, SizeF> PdfPageSizes = (IReadOnlyDictionary<string, SizeF>) new Dictionary<string, SizeF>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
  {
    [nameof (Letter)] = new SizeF(612f, 792f),
    [nameof (Note)] = new SizeF(540f, 720f),
    [nameof (Legal)] = new SizeF(612f, 1008f),
    [nameof (A0)] = new SizeF(2380f, 3368f),
    [nameof (A1)] = new SizeF(1684f, 2380f),
    [nameof (A2)] = new SizeF(1190f, 1684f),
    [nameof (A3)] = new SizeF(842f, 1190f),
    [nameof (A4)] = new SizeF(595f, 842f),
    [nameof (A5)] = new SizeF(421f, 595f),
    [nameof (A6)] = new SizeF(297f, 421f),
    [nameof (A7)] = new SizeF(210f, 297f),
    [nameof (A8)] = new SizeF(148f, 210f),
    [nameof (A9)] = new SizeF(105f, 148f),
    [nameof (A10)] = new SizeF(74f, 105f),
    [nameof (B0)] = new SizeF(2836f, 4008f),
    [nameof (B1)] = new SizeF(2004f, 2836f),
    [nameof (B2)] = new SizeF(1418f, 2004f),
    [nameof (B3)] = new SizeF(1002f, 1418f),
    [nameof (B4)] = new SizeF(709f, 1002f),
    [nameof (B5)] = new SizeF(501f, 709f),
    [nameof (ArchE)] = new SizeF(2592f, 3456f),
    [nameof (ArchD)] = new SizeF(1728f, 2592f),
    [nameof (ArchC)] = new SizeF(1296f, 1728f),
    [nameof (ArchB)] = new SizeF(864f, 1296f),
    [nameof (ArchA)] = new SizeF(648f, 864f),
    [nameof (Flsa)] = new SizeF(612f, 936f),
    [nameof (HalfLetter)] = new SizeF(396f, 612f),
    [nameof (Letter11x17)] = new SizeF(792f, 1224f),
    [nameof (Ledger)] = new SizeF(1224f, 792f)
  };

  public static SizeF? Auto => new SizeF?();

  public static SizeF Letter => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (Letter)];

  public static SizeF Note => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (Note)];

  public static SizeF Legal => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (Legal)];

  public static SizeF A0 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (A0)];

  public static SizeF A1 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (A1)];

  public static SizeF A2 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (A2)];

  public static SizeF A3 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (A3)];

  public static SizeF A4 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (A4)];

  public static SizeF A5 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (A5)];

  public static SizeF A6 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (A6)];

  public static SizeF A7 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (A7)];

  public static SizeF A8 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (A8)];

  public static SizeF A9 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (A9)];

  public static SizeF A10 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (A10)];

  public static SizeF B0 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (B0)];

  public static SizeF B1 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (B1)];

  public static SizeF B2 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (B2)];

  public static SizeF B3 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (B3)];

  public static SizeF B4 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (B4)];

  public static SizeF B5 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (B5)];

  public static SizeF ArchE => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (ArchE)];

  public static SizeF ArchD => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (ArchD)];

  public static SizeF ArchC => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (ArchC)];

  public static SizeF ArchB => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (ArchB)];

  public static SizeF ArchA => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (ArchA)];

  public static SizeF Flsa => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (Flsa)];

  public static SizeF HalfLetter => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (HalfLetter)];

  public static SizeF Letter11x17 => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (Letter11x17)];

  public static SizeF Ledger => ImagePdfGeneratePaperSize.PdfPageSizes[nameof (Ledger)];

  public static bool TryGetPaperSize(string name, out SizeF size)
  {
    size = new SizeF();
    return !string.IsNullOrEmpty(name) && ImagePdfGeneratePaperSize.PdfPageSizes.TryGetValue(name, out size);
  }
}
