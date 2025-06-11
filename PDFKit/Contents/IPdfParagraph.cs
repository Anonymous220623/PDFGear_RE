// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.IPdfParagraph
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net;
using PDFKit.Contents.Operations;
using System;
using System.Globalization;

#nullable disable
namespace PDFKit.Contents;

public interface IPdfParagraph : IDisposable
{
  System.Collections.Generic.IReadOnlyList<TextLine> Lines { get; }

  System.Collections.Generic.IReadOnlyList<ParagraphCaret> Carets { get; }

  Paragraph Paragraph { get; }

  bool IsRotate { get; }

  bool IsVertWriteMode { get; }

  bool BackspaceAt(int caret, bool buildUndoItem, out IPdfUndoItem undoItem);

  bool DeleteText(int start, int end, bool buildUndoItem, out IPdfUndoItem undoItem);

  AlignType GetAlign(int caret);

  FS_RECTF GetBBox();

  int GetCaretAt(FS_POINTF point);

  bool GetCaretPos(int caret, out FS_POINTF top, out FS_POINTF bottom);

  float? GetCharSpace(int caret);

  int GetCurrentCaret();

  bool GetCurrentCaretsRange(out int start, out int end);

  PdfFont GetFont(int caret);

  float GetFontSize(int caret);

  int GetLineHeaderCaret(int caret);

  int GetLineTailCaret(int caret);

  int GetNextCaret(int caret, ParagraphCaretDirect direct);

  System.Collections.Generic.IReadOnlyList<FS_RECTF> GetSelectedBoxs(int startCaret, int endCaret);

  string GetText(int startCaret, int endCaret);

  FS_COLOR? GetTextColor(int caret);

  float? GetWordSpace(int caret);

  bool InsertReturn(int caret, bool buildUndoItem, out IPdfUndoItem undoItem);

  bool InsertText(
    int caret,
    PdfFont font,
    float fontSize,
    FS_COLOR color,
    string text,
    BoldItalicFlags boldItalic,
    int underLineStrikeout,
    ScriptEnum script,
    bool buildUndoItem,
    out IPdfUndoItem undoItem);

  bool InsertText(
    int caret,
    PdfFont font,
    float fontSize,
    FS_COLOR color,
    string text,
    BoldItalicFlags boldItalic,
    int underLineStrikeout,
    ScriptEnum script,
    CultureInfo cultureInfo,
    bool buildUndoItem,
    out IPdfUndoItem undoItem);

  bool InsertText(
    int caret,
    string text,
    CultureInfo cultureInfo,
    bool buildUndoItem,
    out IPdfUndoItem undoItem);

  bool InsertText(int caret, string text, bool buildUndoItem, out IPdfUndoItem undoItem);

  bool IsBold(int startCaret, int endCaret);

  bool IsHeaderCaret(int caret);

  bool IsItalic(int startCaret, int endCaret);

  bool IsTailCaret(int caret);

  void Offset(float dx, float dy, bool buildUndoItem, out IPdfUndoItem undoItem);

  void SetAlign(
    int startCaret,
    int endCaret,
    AlignType align,
    bool buildUndoItem,
    out IPdfUndoItem undoItem);

  bool SetBold(int startCaret, int endCaret, bool buildUndoItem, out IPdfUndoItem undoItem);

  bool SetBold(
    int startCaret,
    int endCaret,
    CultureInfo cultureInfo,
    bool buildUndoItem,
    out IPdfUndoItem undoItem);

  void SetCurrentCarets(int start, int end);

  bool SetFont(
    int startCaret,
    int endCaret,
    PdfFont font,
    bool buildUndoItem,
    out IPdfUndoItem undoItem);

  bool SetFont(
    int startCaret,
    int endCaret,
    string fontFamilyName,
    CultureInfo cultureInfo,
    bool buildUndoItem,
    out IPdfUndoItem undoItem);

  bool SetFontSize(
    int startCaret,
    int endCaret,
    float fontSize,
    bool buildUndoItem,
    out IPdfUndoItem undoItem);

  bool SetItalic(int startCaret, int endCaret, bool buildUndoItem, out IPdfUndoItem undoItem);

  bool SetItalic(
    int startCaret,
    int endCaret,
    CultureInfo cultureInfo,
    bool buildUndoItem,
    out IPdfUndoItem undoItem);

  bool SetStroke(
    int startCaret,
    int endCaret,
    FS_COLOR? color,
    bool buildUndoItem,
    out IPdfUndoItem undoItem);

  bool SetTextColor(
    int startCaret,
    int endCaret,
    FS_COLOR color,
    bool buildUndoItem,
    out IPdfUndoItem undoItem);
}
