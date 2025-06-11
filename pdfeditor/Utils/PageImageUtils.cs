// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.PageImageUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Net;
using System.Windows;

#nullable disable
namespace pdfeditor.Utils;

public class PageImageUtils
{
  public static bool ImageTestHitTest(PdfPage page, Point point, out int Index)
  {
    Index = -1;
    foreach (PdfPageObject pageObject in page.PageObjects)
    {
      ++Index;
      if (pageObject is PdfImageObject pdfImageObject && point.X >= (double) pdfImageObject.BoundingBox.left - 5.0 && point.X <= (double) pdfImageObject.BoundingBox.right + 5.0 && point.Y >= (double) pdfImageObject.BoundingBox.bottom - 5.0 && point.Y <= (double) pdfImageObject.BoundingBox.top + 5.0)
        return true;
    }
    return false;
  }
}
