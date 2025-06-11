// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Operations.DeleteParagraphUndoItem
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace PDFKit.Contents.Operations;

internal class DeleteParagraphUndoItem : IPdfUndoItemInternal, IPdfUndoItem, IDisposable
{
  public UndoTypes UndoType => UndoTypes.DeleteParagraph;

  public int ParagraphId { get; internal set; }

  public int PageIndex { get; internal set; }

  public PdfTypeDictionary PageDict { get; internal set; }

  public int PageObjectInsertIndex { get; set; }

  public int ParagraphInsertIndex { get; set; }

  public Paragraph ClonePara { get; set; }

  public void Undo(LogicalStructAnalyser analyser)
  {
    PdfPage page = analyser.Page;
    Paragraph paragraph = this.ClonePara.Clone();
    analyser.InsertParagraph(this.ParagraphInsertIndex, paragraph);
    int index1 = this.PageObjectInsertIndex;
    if (index1 < 0 || index1 > page.PageObjects.Count)
      index1 = page.PageObjects.Count;
    for (int index2 = 0; index2 < paragraph.Lines.Count; ++index2)
    {
      TextLine line = paragraph.Lines[index2];
      for (int index3 = 0; index3 < line.TextObjects.Count; ++index3)
      {
        page.PageObjects.Insert(index1, (PdfPageObject) line.TextObjects[index3]);
        ++index1;
      }
    }
  }

  public void Redo(LogicalStructAnalyser analyser)
  {
    int paragraphsCount = analyser.ParagraphsCount;
    for (int index = 0; index < paragraphsCount; ++index)
    {
      IPdfParagraph paragraph = analyser.GetParagraph(index);
      if (paragraph.Paragraph.Id == this.ParagraphId)
      {
        analyser.DeleteParagraph(paragraph, false, out IPdfUndoItem _);
        break;
      }
    }
  }

  public void Dispose()
  {
  }
}
