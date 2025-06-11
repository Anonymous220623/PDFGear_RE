// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Operations.PdfUndoItem
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net;

#nullable disable
namespace PDFKit.Contents.Operations;

internal static class PdfUndoItem
{
  internal static PdfParagraphImpl GetParaFromAnalyser(
    LogicalStructAnalyser analyser,
    PdfPage page,
    int id)
  {
    PdfPage page1 = analyser.Page;
    if (page1 != null && page1.Dictionary.Handle == page.Dictionary.Handle)
    {
      for (int index = 0; index < analyser.ParagraphsCount; ++index)
      {
        if (analyser.GetParagraph(index) is PdfParagraphImpl paragraph && paragraph.Paragraph.Id == id)
          return paragraph;
      }
    }
    return (PdfParagraphImpl) null;
  }
}
