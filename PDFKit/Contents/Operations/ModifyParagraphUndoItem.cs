// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Operations.ModifyParagraphUndoItem
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace PDFKit.Contents.Operations;

internal class ModifyParagraphUndoItem : IPdfUndoItemInternal, IPdfUndoItem, IDisposable
{
  internal ModifyParagraphUndoItem()
  {
    this.StartLineIndex = -1;
    this.EndLineIndex = -1;
    this.OffsetStartLineIndex = -1;
    this.DeleteStartLineIndex = -1;
    this.DeleteEndLineIndex = -1;
    this.LinesOffsetX = 0.0f;
    this.LinesOffsetY = 0.0f;
    this.InsertReturnAt = ~InsertReturnAtEnum.LineHeader;
    this.Caret = -1;
    this.EndCaret = -1;
    this.PdfFont = (PdfFont) null;
    this.FontSize = 0.0f;
    this.TextColor = new FS_COLOR();
    this.StrokeColor = new FS_COLOR();
    this.FillStroke = StrokeFillFlags.None;
    this.BoldItalic = BoldItalicFlags.None;
    this.UnderStrikeout = 0;
    this.Script = ScriptEnum.Normal;
    this.CharSpace = 0.0f;
    this.WordSpace = 0.0f;
    this.LineSpace = 0.0f;
    this.Align = AlignType.AlignNone;
    this.ExtractFromFollowing = false;
    this.InsertAtIndex = -1;
    this.ParagraphOffsetX = 0.0f;
    this.ParagraphOffsetY = 0.0f;
  }

  public int ParagraphId { get; internal set; }

  public int PageIndex { get; internal set; }

  public PdfTypeDictionary PageDict { get; internal set; }

  public int Caret { get; internal set; }

  public int EndCaret { get; internal set; }

  public int StartLineIndex { get; internal set; }

  public int EndLineIndex { get; internal set; }

  public InsertReturnAtEnum InsertReturnAt { get; internal set; }

  public float LinesOffsetX { get; internal set; }

  public float LinesOffsetY { get; internal set; }

  public UndoTypes UndoType { get; internal set; }

  public int OffsetStartLineIndex { get; internal set; }

  public int DeleteStartLineIndex { get; internal set; }

  public int DeleteEndLineIndex { get; internal set; }

  public int InsertAtIndex { get; internal set; }

  internal string Text { get; set; }

  internal PdfFont PdfFont { get; set; }

  internal float FontSize { get; set; }

  internal FS_COLOR TextColor { get; set; }

  internal FS_COLOR StrokeColor { get; set; }

  internal StrokeFillFlags FillStroke { get; set; }

  internal BoldItalicFlags BoldItalic { get; set; }

  internal CultureInfo CultureInfo { get; set; }

  internal int UnderStrikeout { get; set; }

  internal ScriptEnum Script { get; set; }

  internal float CharSpace { get; set; }

  internal float WordSpace { get; set; }

  internal float LineSpace { get; set; }

  internal AlignType Align { get; set; }

  internal bool ExtractFromFollowing { get; set; }

  internal float ParagraphOffsetX { get; set; }

  internal float ParagraphOffsetY { get; set; }

  internal List<TextLine> CloneLines { get; set; }

  public void Undo(LogicalStructAnalyser analyser)
  {
    Common.WriteLog($"ModifyParagraph Undo Page {this.PageIndex} UndoType {this.UndoType}");
    PdfParagraphImpl paragraphFromAnalyser = ModifyParagraphUndoItem.GetParagraphFromAnalyser(analyser, this.PageDict, this.ParagraphId);
    if (paragraphFromAnalyser == null)
      return;
    if (this.UndoType == UndoTypes.OffsetParagraph)
    {
      paragraphFromAnalyser.Offset(-this.ParagraphOffsetX, -this.ParagraphOffsetY, false, out IPdfUndoItem _);
    }
    else
    {
      int count1 = paragraphFromAnalyser.Paragraph.Lines.Count;
      TextLine textLine = (TextLine) null;
      if (this.OffsetStartLineIndex >= 0)
      {
        int offsetStartLineIndex = this.OffsetStartLineIndex;
        if (offsetStartLineIndex < count1 && ((double) this.LinesOffsetX != 0.0 || (double) this.LinesOffsetY != 0.0))
        {
          for (int index = offsetStartLineIndex; index < count1; ++index)
          {
            TextLine line = paragraphFromAnalyser.Paragraph.Lines[index];
            if (line != null)
            {
              line.Offset(this.LinesOffsetX, this.LinesOffsetY);
              if (textLine == null && line.TextObjects.Count > 0)
                textLine = line;
            }
          }
        }
      }
      PdfPageObjectsCollection objectsCollection = (PdfPageObjectsCollection) null;
      int num1 = -1;
      int num2;
      if (this.DeleteStartLineIndex >= 0 && this.DeleteStartLineIndex < count1 && this.DeleteEndLineIndex >= this.DeleteStartLineIndex && this.DeleteEndLineIndex < count1)
      {
        int deleteStartLineIndex = this.DeleteStartLineIndex;
        int deleteEndLineIndex = this.DeleteEndLineIndex;
        int num3 = deleteStartLineIndex - 1;
        num2 = deleteStartLineIndex - 1;
        if (num2 < 0 && deleteStartLineIndex >= 0 && deleteStartLineIndex < paragraphFromAnalyser.Lines.Count)
        {
          TextLine line = paragraphFromAnalyser.Lines[deleteStartLineIndex];
          if (line.TextObjects.Count > 0)
          {
            PdfTextObject textObject = line.TextObjects[0];
            objectsCollection = textObject.Container;
            num1 = objectsCollection.IndexOf((PdfPageObject) textObject) - 1;
          }
        }
        while (deleteEndLineIndex >= 0)
        {
          TextLine line = paragraphFromAnalyser.Paragraph.Lines[deleteEndLineIndex];
          paragraphFromAnalyser.Paragraph.RemoveLine(deleteEndLineIndex);
          --deleteEndLineIndex;
          if (deleteEndLineIndex == num3)
            break;
        }
      }
      else
        num2 = this.StartLineIndex - 1;
      if (objectsCollection == null)
      {
        int index = num2;
        while (index >= 0)
        {
          TextLine line = paragraphFromAnalyser.Paragraph.Lines[index];
          --index;
          int count2 = line.TextObjects.Count;
          if (count2 > 0)
          {
            PdfTextObject textObject = line.TextObjects[count2 - 1];
            objectsCollection = textObject.Container;
            num1 = objectsCollection.IndexOf((PdfPageObject) textObject);
          }
        }
        if (objectsCollection == null && textLine != null)
        {
          int count3 = textLine.TextObjects.Count;
          PdfTextObject textObject = textLine.TextObjects[count3 - 1];
          objectsCollection = textObject.Container;
          num1 = objectsCollection.IndexOf((PdfPageObject) textObject);
        }
        if (objectsCollection == null)
        {
          objectsCollection = paragraphFromAnalyser.Page.PageObjects;
          num1 = objectsCollection.Count - 1;
        }
      }
      if (objectsCollection == null)
        return;
      List<TextLine> cloneLines = this.CloneLines;
      // ISSUE: explicit non-virtual call
      int count4 = cloneLines != null ? __nonvirtual (cloneLines.Count) : 0;
      if (num1 > objectsCollection.Count - 1)
        num1 = objectsCollection.Count - 1;
      for (int index1 = count4 - 1; index1 >= 0; --index1)
      {
        TextLine line = this.CloneLines[index1].Clone();
        for (int index2 = line.TextObjects.Count - 1; index2 >= 0; --index2)
        {
          PdfTextObject textObject = line.TextObjects[index2];
          objectsCollection.Insert(num1 + 1, (PdfPageObject) textObject);
        }
        paragraphFromAnalyser.Paragraph.InsertLine(num2 + 1, line);
      }
      paragraphFromAnalyser.Paragraph.InvalidateBoundingBox();
      paragraphFromAnalyser.BuildCarets();
      paragraphFromAnalyser.SetCurrentCarets(this.Caret, this.EndCaret);
    }
  }

  public void Redo(LogicalStructAnalyser analyser)
  {
    Common.WriteLog($"ModifyParagraph Redo Page {this.PageIndex} UndoType {this.UndoType}");
    switch (this.UndoType)
    {
      case UndoTypes.InsertText:
        this.RedoInsertText(analyser);
        break;
      case UndoTypes.DeleteText:
        this.RedoDeleteText(analyser);
        break;
      case UndoTypes.SetFont:
        this.RedoSetFont(analyser);
        break;
      case UndoTypes.SetFontSize:
        this.RedoSetFontSize(analyser);
        break;
      case UndoTypes.SetTextColor:
        this.RedoSetTextColor(analyser);
        break;
      case UndoTypes.SetBold:
        this.RedoSetBold(analyser);
        break;
      case UndoTypes.SetItalic:
        this.RedoSetItalic(analyser);
        break;
      case UndoTypes.SetUnderline:
        this.RedoSetUnderline(analyser);
        break;
      case UndoTypes.SetStrikeout:
        this.RedoSetStrikeout(analyser);
        break;
      case UndoTypes.SetSupperScript:
        this.RedoSetSupperScript(analyser);
        break;
      case UndoTypes.SetSubScript:
        this.RedoSetSubScript(analyser);
        break;
      case UndoTypes.SetCharSpace:
        this.RedoSetCharSpace(analyser);
        break;
      case UndoTypes.SetWordSpace:
        this.RedoSetWordSpace(analyser);
        break;
      case UndoTypes.SetLineSpace:
        this.RedoSetLineSpace(analyser);
        break;
      case UndoTypes.SetAlign:
        this.RedoSetAlign(analyser);
        break;
      case UndoTypes.InsertReturn:
        this.RedoInsertReturn(analyser);
        break;
      case UndoTypes.OffsetParagraph:
        this.RedoOffsetParagraph(analyser);
        break;
    }
  }

  private void RedoInsertText(LogicalStructAnalyser analyser)
  {
    if (this.Caret < 0)
      return;
    PdfParagraphImpl paragraphFromAnalyser = ModifyParagraphUndoItem.GetParagraphFromAnalyser(analyser, this.PageDict, this.ParagraphId);
    if (paragraphFromAnalyser == null)
      return;
    IPdfUndoItem undoItem;
    if (this.Text.Length == 1 && this.Text[0] == '\r')
    {
      paragraphFromAnalyser.InsertReturn(this.Caret, false, out undoItem);
    }
    else
    {
      PdfFont pdfFont = this.PdfFont;
      if (pdfFont != null)
        paragraphFromAnalyser.InsertText(this.Caret, pdfFont, this.FontSize, this.TextColor, this.Text, this.BoldItalic, this.UnderStrikeout, this.Script, this.CultureInfo, false, out undoItem);
    }
  }

  private void RedoDeleteText(LogicalStructAnalyser analyser)
  {
    if (this.Caret < 0)
      return;
    PdfParagraphImpl paragraphFromAnalyser = ModifyParagraphUndoItem.GetParagraphFromAnalyser(analyser, this.PageDict, this.ParagraphId);
    if (paragraphFromAnalyser == null)
      return;
    IPdfUndoItem undoItem;
    if (this.EndCaret == -1)
    {
      paragraphFromAnalyser.BackspaceAt(this.Caret, false, out undoItem);
    }
    else
    {
      if (this.EndCaret < 0)
        return;
      paragraphFromAnalyser.DeleteText(this.Caret, this.EndCaret, false, out undoItem);
    }
  }

  private void RedoSetFont(LogicalStructAnalyser analyser)
  {
    if (this.Caret < 0)
      return;
    PdfParagraphImpl paragraphFromAnalyser = ModifyParagraphUndoItem.GetParagraphFromAnalyser(analyser, this.PageDict, this.ParagraphId);
    if (paragraphFromAnalyser == null)
      return;
    PdfFont pdfFont = this.PdfFont;
    if (pdfFont == null)
      return;
    paragraphFromAnalyser.SetFont(this.Caret, this.EndCaret, pdfFont, false, out IPdfUndoItem _);
  }

  private void RedoSetFontSize(LogicalStructAnalyser analyser)
  {
    if (this.Caret < 0)
      return;
    ModifyParagraphUndoItem.GetParagraphFromAnalyser(analyser, this.PageDict, this.ParagraphId)?.SetFontSize(this.Caret, this.EndCaret, this.FontSize, false, out IPdfUndoItem _);
  }

  private void RedoSetTextColor(LogicalStructAnalyser analyser)
  {
    if (this.Caret < 0)
      return;
    ModifyParagraphUndoItem.GetParagraphFromAnalyser(analyser, this.PageDict, this.ParagraphId)?.SetTextColor(this.Caret, this.EndCaret, this.TextColor, false, out IPdfUndoItem _);
  }

  private void RedoSetBold(LogicalStructAnalyser analyser)
  {
    if (this.Caret < 0)
      return;
    ModifyParagraphUndoItem.GetParagraphFromAnalyser(analyser, this.PageDict, this.ParagraphId)?.SetBold(this.Caret, this.EndCaret, this.CultureInfo, false, out IPdfUndoItem _);
  }

  private void RedoSetItalic(LogicalStructAnalyser analyser)
  {
    if (this.Caret < 0)
      return;
    ModifyParagraphUndoItem.GetParagraphFromAnalyser(analyser, this.PageDict, this.ParagraphId)?.SetItalic(this.Caret, this.EndCaret, this.CultureInfo, false, out IPdfUndoItem _);
  }

  private void RedoSetUnderline(LogicalStructAnalyser analyser)
  {
    throw new NotImplementedException();
  }

  private void RedoSetStrikeout(LogicalStructAnalyser analyser)
  {
    throw new NotImplementedException();
  }

  private void RedoSetSupperScript(LogicalStructAnalyser analyser)
  {
    throw new NotImplementedException();
  }

  private void RedoSetSubScript(LogicalStructAnalyser analyser)
  {
    throw new NotImplementedException();
  }

  private void RedoSetCharSpace(LogicalStructAnalyser analyser)
  {
    throw new NotImplementedException();
  }

  private void RedoSetWordSpace(LogicalStructAnalyser analyser)
  {
    throw new NotImplementedException();
  }

  private void RedoSetLineSpace(LogicalStructAnalyser analyser)
  {
    throw new NotImplementedException();
  }

  private void RedoSetAlign(LogicalStructAnalyser analyser)
  {
    if (this.Caret < 0)
      return;
    ModifyParagraphUndoItem.GetParagraphFromAnalyser(analyser, this.PageDict, this.ParagraphId)?.SetAlign(this.Caret, this.EndCaret, this.Align, false, out IPdfUndoItem _);
  }

  private void RedoOffsetParagraph(LogicalStructAnalyser analyser)
  {
    ModifyParagraphUndoItem.GetParagraphFromAnalyser(analyser, this.PageDict, this.ParagraphId)?.Offset(this.ParagraphOffsetX, this.ParagraphOffsetY, false, out IPdfUndoItem _);
  }

  private void RedoInsertReturn(LogicalStructAnalyser analyser)
  {
    if (this.Caret < 0)
      return;
    ModifyParagraphUndoItem.GetParagraphFromAnalyser(analyser, this.PageDict, this.ParagraphId)?.InsertReturn(this.Caret, false, out IPdfUndoItem _);
  }

  public void Dispose()
  {
  }

  internal static PdfParagraphImpl GetParagraphFromAnalyser(
    LogicalStructAnalyser analyser,
    PdfTypeDictionary pageDict,
    int paraId)
  {
    IntPtr handle1 = analyser.Page.Dictionary.Handle;
    IntPtr? handle2 = pageDict?.Handle;
    if (handle2.HasValue && handle1 == handle2.GetValueOrDefault())
    {
      int paragraphsCount = analyser.ParagraphsCount;
      for (int index = 0; index < paragraphsCount; ++index)
      {
        if (analyser.GetParagraph(index) is PdfParagraphImpl paragraph && paragraph.Paragraph.Id == paraId)
          return paragraph;
      }
    }
    return (PdfParagraphImpl) null;
  }
}
