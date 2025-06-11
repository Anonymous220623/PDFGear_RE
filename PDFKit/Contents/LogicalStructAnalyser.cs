// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.LogicalStructAnalyser
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net;
using PDFKit.Contents.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace PDFKit.Contents;

public class LogicalStructAnalyser
{
  private Dictionary<Paragraph, PdfParagraphImpl> paragraphMap = new Dictionary<Paragraph, PdfParagraphImpl>();
  private PdfPage page;
  private ParagraphRecognizer paraRecognizer;

  public LogicalStructAnalyser(PdfPage page)
  {
    this.page = page;
    this.Initialize();
  }

  private void Initialize()
  {
    this.paraRecognizer = new ParagraphRecognizer(this.page, this.page.PageObjects, false);
  }

  public PdfPage Page => this.page;

  public int ParagraphsCount => this.paraRecognizer.Paragraphs.Count;

  public IPdfParagraph GetParagraph(int index)
  {
    if (index < 0 || index >= this.paraRecognizer.Paragraphs.Count)
      return (IPdfParagraph) null;
    PdfParagraphImpl paragraph1;
    if (this.paragraphMap.TryGetValue(this.paraRecognizer.Paragraphs[index], out paragraph1))
      return (IPdfParagraph) paragraph1;
    PdfParagraphImpl paragraph2 = new PdfParagraphImpl(this.page.Document, this.page, this.paraRecognizer.Paragraphs[index]);
    this.paragraphMap[this.paraRecognizer.Paragraphs[index]] = paragraph2;
    return (IPdfParagraph) paragraph2;
  }

  public IPdfParagraph NewParagraphAt(FS_POINTF point, float width, float height)
  {
    Paragraph paragraph = new Paragraph(this.Page, this.paraRecognizer.GetCurParaID());
    paragraph.NewEmpty(point.X, point.Y, width, height);
    this.paraRecognizer.InsertParagraph(this.paraRecognizer.Paragraphs.Count, paragraph);
    PdfParagraphImpl pdfParagraphImpl = new PdfParagraphImpl(this.page.Document, this.page, paragraph);
    pdfParagraphImpl.BuildCarets();
    this.paragraphMap[paragraph] = pdfParagraphImpl;
    return (IPdfParagraph) pdfParagraphImpl;
  }

  public bool DeleteParagraph(
    IPdfParagraph paragraph,
    bool buildUndoItem,
    out IPdfUndoItem undoItem)
  {
    undoItem = (IPdfUndoItem) null;
    Paragraph paragraph1 = paragraph.Paragraph;
    if (buildUndoItem)
    {
      DeleteParagraphUndoItem paragraphUndoItem = new DeleteParagraphUndoItem()
      {
        ClonePara = paragraph1.Clone(),
        PageDict = this.page.Dictionary,
        PageIndex = this.page.PageIndex,
        ParagraphId = paragraph1.Id
      };
      undoItem = (IPdfUndoItem) paragraphUndoItem;
      int paragraphsCount = this.ParagraphsCount;
      for (int index = 0; index < paragraphsCount; ++index)
      {
        if (this.GetParagraph(index).Paragraph.Id == paragraph1.Id)
        {
          paragraphUndoItem.ParagraphInsertIndex = index;
          break;
        }
      }
      TextLine textLine = paragraph1.Lines.FirstOrDefault<TextLine>((Func<TextLine, bool>) (c => c.TextObjects.Count > 0));
      if (textLine != null)
      {
        int num = this.page.PageObjects.IndexOf((PdfPageObject) textLine.TextObjects[0]);
        paragraphUndoItem.PageObjectInsertIndex = num;
      }
    }
    int index1 = this.paraRecognizer.Paragraphs.IndexOf<Paragraph>(paragraph1);
    if (index1 != -1)
    {
      this.paraRecognizer.RemoveParagraph(index1);
      this.paragraphMap.Remove(paragraph1);
      paragraph.Dispose();
    }
    return true;
  }

  internal void InsertParagraph(int insertAt, Paragraph paragraph)
  {
    this.paraRecognizer.InsertParagraph(insertAt, paragraph);
    PdfParagraphImpl pdfParagraphImpl = new PdfParagraphImpl(this.page.Document, this.page, paragraph);
    this.paragraphMap[paragraph] = pdfParagraphImpl;
  }
}
