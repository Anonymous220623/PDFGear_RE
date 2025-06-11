// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.PdfParagraphImpl
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using PDFKit.Contents.Operations;
using PDFKit.Contents.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Contents;

[Synchronization(4, true)]
internal class PdfParagraphImpl : IPdfParagraph, IDisposable
{
  private bool disposedValue;
  private Paragraph paragraph;
  private List<ParagraphCaret> caretList;
  private int curCaret = -1;
  private int endCaret = -1;
  private PdfDocument document;
  private PdfParagraphImpl.TempCaretType tempCaretType = PdfParagraphImpl.TempCaretType.None;
  private PdfPage page;
  private ParagraphCaret tempCaretInfo;
  private ParagraphCaret tempEndCaretInfo;

  public PdfParagraphImpl(PdfDocument document, PdfPage page, Paragraph paragraph)
  {
    this.document = document;
    this.page = page;
    this.paragraph = paragraph;
    this.tempCaretInfo = new ParagraphCaret();
    this.tempEndCaretInfo = new ParagraphCaret();
  }

  public System.Collections.Generic.IReadOnlyList<TextLine> Lines => this.paragraph.Lines;

  public System.Collections.Generic.IReadOnlyList<ParagraphCaret> Carets
  {
    get => (System.Collections.Generic.IReadOnlyList<ParagraphCaret>) this.caretList;
  }

  public FS_RECTF GetBBox() => this.paragraph.OuterBox;

  public bool IsRotate => this.paragraph.IsRotate;

  public bool IsVertWriteMode => this.paragraph.IsVertWriteMode;

  public int CurrentCaretIndex => this.caretList == null ? -1 : this.curCaret;

  public PdfPage Page => this.page;

  public Paragraph Paragraph => this.paragraph;

  public string GetText(int startCaret, int endCaret)
  {
    StringBuilder _sb = new StringBuilder();
    bool flag = false;
    if (startCaret == endCaret)
    {
      if (startCaret != -1)
        return string.Empty;
      flag = true;
    }
    if (this.caretList == null)
      this.BuildCarets();
    if (startCaret == 0 && endCaret == this.caretList.Count - 1)
      flag = true;
    if (flag)
    {
      foreach (TextLine line in (IEnumerable<TextLine>) this.paragraph.Lines)
      {
        _sb.Append(line.LineText);
        if (line.ReturnFlag)
          _sb.Append('\n');
      }
    }
    else
    {
      ParagraphCaret startCaret1;
      ParagraphCaret endCaret1;
      if (this.VerifyCarets(startCaret, endCaret, out startCaret1, out endCaret1))
        TryAppendSegment(this.paragraph, startCaret1, endCaret1, _sb);
    }
    return _sb.ToString();

    static void TryAppendSegment(
      Paragraph _para,
      ParagraphCaret _start,
      ParagraphCaret _end,
      StringBuilder _sb)
    {
      int num1 = _para.Lines.IndexOf<TextLine>(_start.Line);
      int num2 = _para.Lines.IndexOf<TextLine>(_end.Line);
      for (int index = num1; index <= num2; ++index)
      {
        if (index == num1 && index == num2)
          TryAppendLine(_start, _end, _sb);
        else if (index == num1)
          TryAppendLine(_start, (ParagraphCaret) null, _sb);
        else if (index == num2)
        {
          TryAppendLine((ParagraphCaret) null, _end, _sb);
        }
        else
        {
          string lineText = _para.Lines[index].LineText;
          if (lineText.Length > 0)
          {
            _sb.Append(lineText);
            if (_para.Lines[index].ReturnFlag)
              _sb.Append('\n');
          }
        }
      }
    }

    static void TryAppendLine(ParagraphCaret _start, ParagraphCaret _end, StringBuilder _sb)
    {
      if (_start == null && _end == null)
        throw new ArgumentException(nameof (_start));
      if (_start != null && _end != null && _start.Line != _end.Line)
        throw new ArgumentException(nameof (_end));
      if (_start == null)
        _start = new ParagraphCaret()
        {
          Line = _end.Line,
          CharIndex = 0,
          TextObject = _end.Line.TextObjects[0]
        };
      if (_end == null)
      {
        PdfTextObject textObject = _start.Line.TextObjects[_start.Line.TextObjects.Count - 1];
        _end = new ParagraphCaret()
        {
          Line = _start.Line,
          CharIndex = textObject.CharsCount,
          TextObject = textObject
        };
      }
      PdfTextObject textObject1 = _start.TextObject;
      int num1 = _start.Line.TextObjects.IndexOf<PdfTextObject>(textObject1);
      PdfTextObject textObject2 = _end.TextObject;
      int _endCharIndex = _end.CharIndex;
      int num2 = _start.Line.TextObjects.IndexOf<PdfTextObject>(textObject2);
      if (_endCharIndex == 0)
      {
        textObject2 = _start.Line.TextObjects[num2 - 1];
        _endCharIndex = textObject2.CharsCount;
      }
      for (int index = num1; index <= num2; ++index)
      {
        if (index == num1 && index == num2)
          TryAppendObject(textObject1, _start.CharIndex, _end.CharIndex, _sb);
        else if (index == num1)
          TryAppendObject(textObject1, _start.CharIndex, textObject1.CharsCount, _sb);
        else if (index == num2)
        {
          TryAppendObject(textObject2, 0, _endCharIndex, _sb);
        }
        else
        {
          string text = _start.Line.TextObjects[index].GetText();
          if (text.Length > 0)
            _sb.Append(text);
        }
      }
      if (!_start.Line.ReturnFlag)
        return;
      _sb.Append('\n');
    }

    static void TryAppendObject(
      PdfTextObject _textObj,
      int _startCharIndex,
      int _endCharIndex,
      StringBuilder _sb)
    {
      for (int _charIndex = _startCharIndex; _charIndex < _endCharIndex; ++_charIndex)
        TryAppendChar(_textObj, _charIndex, _sb);
    }

    static void TryAppendChar(PdfTextObject _textObj, int _charIndex, StringBuilder _sb)
    {
      int charCode;
      _textObj.GetCharInfo(_charIndex, out charCode, out float _, out float _);
      PdfFont font = _textObj.Font;
      if (font != null && font.IsUnicodeCompatible)
      {
        char unicode = _textObj.Font.ToUnicode(charCode);
        if (unicode <= char.MinValue)
          return;
        _sb.Append(unicode);
      }
      else if (charCode > 0 && charCode <= (int) ushort.MaxValue)
        _sb.Append((char) charCode);
    }
  }

  public int GetCurrentCaret() => this.caretList == null ? -1 : this.curCaret;

  public bool GetCurrentCaretsRange(out int start, out int end)
  {
    start = -1;
    end = -1;
    if (this.caretList == null || this.curCaret == -1 && this.endCaret == -1)
      return false;
    start = this.curCaret;
    end = this.endCaret;
    return true;
  }

  public int GetCaretAt(FS_POINTF point)
  {
    FS_RECTF outerBox = this.paragraph.OuterBox;
    if (!this.paragraph.IsRotate && !outerBox.Contains(point))
      return -1;
    int num1 = -1;
    float num2 = (float) ushort.MaxValue;
    float num3 = (float) ushort.MaxValue;
    TextLine line = (TextLine) null;
    for (int index = 0; index < this.paragraph.Lines.Count; ++index)
    {
      TextLine line1 = this.paragraph.Lines[index];
      if (!this.paragraph.IsRotate)
      {
        FS_RECTF boundingBox = line1.GetBoundingBox(false);
        boundingBox.Inflate(new FS_RECTF(2f, 0.0f, 2f, 0.0f));
        if (boundingBox.Contains(point))
        {
          line = line1;
          num1 = index;
          break;
        }
        if (!this.paragraph.IsVertWriteMode)
        {
          float num4 = Math.Abs(boundingBox.bottom + boundingBox.Height / 2f - point.Y);
          if ((double) num4 <= (double) num3)
          {
            line = line1;
            num3 = num4;
            num1 = index;
          }
        }
      }
    }
    if (line == null)
      return -1;
    if (this.caretList == null)
      this.BuildCarets();
    if (line.TextObjects.Count == 0)
    {
      if (num1 == 0)
        return 0;
      for (int index = 0; index < this.caretList.Count; ++index)
      {
        if (this.caretList[index].Line == line)
          return index;
      }
      return 0;
    }
    int caretAt = -1;
    int num5 = this.caretList.IndexOf<ParagraphCaret>((Func<ParagraphCaret, bool>) (c => c.TextObject == line.TextObjects[0]));
    if (num5 != -1)
    {
      for (int index = num5; index < this.caretList.Count; ++index)
      {
        ParagraphCaret caret = this.caretList[index];
        if (caret.Line == line)
        {
          if (!this.paragraph.IsRotate && !this.paragraph.IsVertWriteMode)
          {
            float num6 = Math.Abs(caret.TopPoint.X - point.X);
            if ((double) num6 <= (double) num2)
            {
              num2 = num6;
              caretAt = index;
            }
          }
        }
        else
          break;
      }
    }
    return caretAt;
  }

  public bool GetCaretPos(int caret, out FS_POINTF top, out FS_POINTF bottom)
  {
    top = new FS_POINTF();
    bottom = new FS_POINTF();
    if (this.caretList == null || caret < 0 || caret >= this.caretList.Count)
      return false;
    ParagraphCaret caret1 = this.caretList[caret];
    top = caret1.TopPoint;
    bottom = caret1.BottomPoint;
    return true;
  }

  public int GetNextCaret(int caret, ParagraphCaretDirect direct)
  {
    if (this.caretList == null)
      return -1;
    if (caret == 0)
    {
      if (direct == ParagraphCaretDirect.Left || direct == ParagraphCaretDirect.Up)
        return 0;
    }
    else if (caret == this.caretList.Count - 1 && (direct == ParagraphCaretDirect.Right || direct == ParagraphCaretDirect.Down))
      return caret;
    switch (direct)
    {
      case ParagraphCaretDirect.Left:
        --caret;
        return caret;
      case ParagraphCaretDirect.Up:
        if (!this.paragraph.IsVertWriteMode)
        {
          ParagraphCaret caret1 = this.caretList[caret];
          int num1 = this.paragraph.Lines.IndexOf<TextLine>(caret1.Line);
          if (num1 == 0)
            return caret;
          TextLine prevLine = this.paragraph.Lines[num1 - 1];
          int num2 = this.caretList.IndexOf<ParagraphCaret>((Func<ParagraphCaret, bool>) (c => c.Line == prevLine));
          float num3 = (float) ushort.MaxValue;
          int num4 = -1;
          for (int index = num2; index < this.caretList.Count; ++index)
          {
            ParagraphCaret caret2 = this.caretList[index];
            if (caret2.Line == prevLine)
            {
              float num5 = Math.Abs(caret2.BottomPoint.X - caret1.BottomPoint.X);
              if ((double) num5 < (double) num3)
              {
                num3 = num5;
                num4 = index;
              }
            }
            else
              break;
          }
          return num4 >= 0 ? num4 : caret;
        }
        break;
      case ParagraphCaretDirect.Right:
        ++caret;
        return caret;
      case ParagraphCaretDirect.Down:
        if (!this.paragraph.IsVertWriteMode)
        {
          ParagraphCaret caret3 = this.caretList[caret];
          int num6 = this.paragraph.Lines.IndexOf<TextLine>(caret3.Line);
          if (num6 == this.paragraph.Lines.Count - 1)
            return caret;
          TextLine nextLine = this.paragraph.Lines[num6 + 1];
          int num7 = this.caretList.IndexOf<ParagraphCaret>((Func<ParagraphCaret, bool>) (c => c.Line == nextLine));
          float num8 = (float) ushort.MaxValue;
          int num9 = -1;
          for (int index = num7; index < this.caretList.Count; ++index)
          {
            ParagraphCaret caret4 = this.caretList[index];
            if (caret4.Line == nextLine)
            {
              float num10 = Math.Abs(caret4.BottomPoint.X - caret3.BottomPoint.X);
              if ((double) num10 < (double) num8)
              {
                num8 = num10;
                num9 = index;
              }
            }
            else
              break;
          }
          return num9 >= 0 ? num9 : caret;
        }
        break;
    }
    return caret;
  }

  public bool IsHeaderCaret(int caret) => caret == 0;

  public bool IsTailCaret(int caret) => this.caretList != null && caret == this.caretList.Count - 1;

  public bool IsLineHeaderCaret(int caret)
  {
    return this.caretList != null && caret >= 0 && caret < this.caretList.Count && this.IsLineHeaderCaret(this.caretList[caret]);
  }

  private bool IsLineHeaderCaret(ParagraphCaret caret)
  {
    return this.caretList != null && caret != null && this.caretList.FirstOrDefault<ParagraphCaret>((Func<ParagraphCaret, bool>) (c => c.Line == caret.Line)) == caret;
  }

  public bool IsLineTailCaret(int caret)
  {
    return this.caretList != null && caret >= 0 && caret < this.caretList.Count && this.IsLineTailCaret(this.caretList[caret]);
  }

  private bool IsLineTailCaret(ParagraphCaret caret)
  {
    return this.caretList != null && caret != null && this.caretList.LastOrDefault<ParagraphCaret>((Func<ParagraphCaret, bool>) (c => c.Line == caret.Line)) == caret;
  }

  public int GetLineHeaderCaret(int caret)
  {
    if (this.caretList == null || caret < 0 || caret >= this.caretList.Count)
      return -1;
    ParagraphCaret _c = this.caretList[caret];
    return this.caretList.IndexOf<ParagraphCaret>((Func<ParagraphCaret, bool>) (c => c.Line == _c.Line));
  }

  public int GetLineTailCaret(int caret)
  {
    if (this.caretList == null || caret < 0 || caret >= this.caretList.Count)
      return -1;
    ParagraphCaret caret1 = this.caretList[caret];
    for (int index = this.caretList.Count - 1; index >= 0; --index)
    {
      if (this.caretList[index].Line == caret1.Line)
        return index;
    }
    return -1;
  }

  public System.Collections.Generic.IReadOnlyList<FS_RECTF> GetSelectedBoxs(
    int startCaret,
    int endCaret)
  {
    if (startCaret == endCaret)
      return (System.Collections.Generic.IReadOnlyList<FS_RECTF>) Array.Empty<FS_RECTF>();
    if (startCaret < 0 && endCaret < 0)
      return (System.Collections.Generic.IReadOnlyList<FS_RECTF>) Array.Empty<FS_RECTF>();
    if (this.caretList == null)
      this.BuildCarets();
    int val1_1 = Math.Min(startCaret, endCaret);
    int val1_2 = Math.Max(startCaret, endCaret);
    int num1 = Math.Min(val1_1, this.caretList.Count - 1);
    int num2 = Math.Min(val1_2, this.caretList.Count - 1);
    List<FS_RECTF> selectedBoxs = new List<FS_RECTF>();
    ParagraphCaret _c1 = (ParagraphCaret) null;
    for (int index = num1; index <= num2; ++index)
    {
      ParagraphCaret caret1 = this.caretList[index];
      if (_c1 != null && caret1.Line != _c1.Line)
      {
        ParagraphCaret caret2 = this.caretList[index - 1];
        selectedBoxs.Add(GetBoxCore(_c1, caret2));
        _c1 = (ParagraphCaret) null;
      }
      if (_c1 == null)
        _c1 = this.caretList[index];
      if (index == num2)
        selectedBoxs.Add(GetBoxCore(_c1, caret1));
    }
    return (System.Collections.Generic.IReadOnlyList<FS_RECTF>) selectedBoxs;

    static FS_RECTF GetBoxCore(ParagraphCaret _c1, ParagraphCaret _c2)
    {
      FS_POINTF fsPointf = _c1.TopPoint;
      double x1 = (double) fsPointf.X;
      fsPointf = _c1.TopPoint;
      double y1 = (double) fsPointf.Y;
      fsPointf = _c2.BottomPoint;
      double x2 = (double) fsPointf.X;
      fsPointf = _c2.BottomPoint;
      double y2 = (double) fsPointf.Y;
      return new FS_RECTF((float) x1, (float) y1, (float) x2, (float) y2);
    }
  }

  public bool DeleteText(int start, int end, bool buildUndoItem, out IPdfUndoItem undoItem)
  {
    if (start + 1 == end)
      return this.DeleteText(start, start, buildUndoItem, out undoItem);
    if (this.DeleteTextCore(start, end, buildUndoItem, out undoItem))
      return true;
    IPdfUndoItem undoItem1;
    if (start != end || !this.DeleteAtLineHeader(start, buildUndoItem, out undoItem1))
      return false;
    this.curCaret = start;
    this.endCaret = -1;
    if (undoItem1 != null)
      undoItem = (IPdfUndoItem) new CompositeUndoItem((System.Collections.Generic.IReadOnlyList<IPdfUndoItem>) new List<IPdfUndoItem>()
      {
        undoItem1
      })
      {
        PageDict = this.page.Dictionary,
        PageIndex = this.page.PageIndex,
        ParagraphId = this.paragraph.Id,
        PrevCaret = start,
        PrevEndCaret = start
      };
    return true;
  }

  private bool DeleteAtLineHeader(int caret, bool buildUndoItem, out IPdfUndoItem undoItem)
  {
    undoItem = (IPdfUndoItem) null;
    int num1 = caret + 1;
    ParagraphCaret startCaret1;
    ParagraphCaret endCaret;
    if (!this.VerifyCarets(num1, num1, out startCaret1, out endCaret))
      return false;
    bool flag1 = num1 == 0;
    if (!flag1)
      flag1 = endCaret.TextObject != null && endCaret.Line.IsLineHeader(endCaret.TextObject, endCaret.CharIndex);
    if (!flag1)
      flag1 = endCaret.Line.TextObjects.Count == 0;
    if (!flag1)
      return false;
    int num2 = num1 - 1;
    if (num2 < 0)
      return false;
    ParagraphCaret startCaret2;
    this.VerifyCarets(num2, num2, out startCaret2, out ParagraphCaret _);
    int num3 = this.paragraph.Lines.IndexOf<TextLine>(startCaret1.Line);
    int editStartLineIndex = this.paragraph.Lines.IndexOf<TextLine>(startCaret2.Line);
    PdfUndoSnapshot pdfUndoSnapshot = (PdfUndoSnapshot) null;
    bool flag2;
    if (startCaret1.Line.TextObjects.Count == 0)
    {
      if (buildUndoItem)
        pdfUndoSnapshot = new PdfUndoSnapshot(this, num1, num1, num3, num3);
      if (num3 + 1 < this.paragraph.Lines.Count)
      {
        TextLine line = this.paragraph.Lines[num3 + 1];
        float lineSpace = this.GetLineSpace(startCaret1.Line, true);
        if (!this.IsRotate && !this.IsVertWriteMode)
          this.paragraph.OffsetLinesVert(lineSpace, line);
      }
      this.paragraph.RemoveLine(startCaret1.Line);
      flag2 = true;
    }
    else
    {
      if (buildUndoItem)
        pdfUndoSnapshot = new PdfUndoSnapshot(this, num1, num1, editStartLineIndex, num3);
      bool returnFlag = startCaret2.Line.ReturnFlag;
      startCaret2.Line.ReturnFlag = false;
      flag2 = this.ExtractFollowingLinesToMakeAlign(startCaret2.Line, false, false);
      if (!flag2)
      {
        startCaret2.Line.ReturnFlag = returnFlag;
        this.curCaret = num2;
        this.endCaret = num2;
      }
    }
    if (flag2 & buildUndoItem)
    {
      ModifyParagraphUndoItem undoItem1 = new ModifyParagraphUndoItem();
      undoItem = (IPdfUndoItem) undoItem1;
      undoItem1.UndoType = UndoTypes.DeleteText;
      pdfUndoSnapshot.FillUndoItem(undoItem1, false);
      undoItem1.EndCaret = -1;
    }
    this.BuildCarets();
    this.paragraph.InvalidateBoundingBox(CalcBoundingBoxOrientation.Vertical);
    return true;
  }

  private bool DeleteTextCore(int start, int end, bool buildUndoItem, out IPdfUndoItem undoItem)
  {
    undoItem = (IPdfUndoItem) null;
    if (end == -1)
      end = start;
    bool flag1 = this.IsTailCaret(end);
    if (!this.IsLineTailCaret(end) && !flag1 && end > 0 && start != end)
    {
      ParagraphCaret startCaret;
      ParagraphCaret endCaret;
      if (!this.VerifyCarets(start, end, out startCaret, out endCaret))
        return false;
      if (startCaret.TextObject == endCaret.TextObject)
        --end;
    }
    ParagraphCaret startCaret1;
    ParagraphCaret endCaret1;
    if (!this.VerifyCarets(start, end, out startCaret1, out endCaret1) || startCaret1.Line == null || startCaret1.Line.TextObjects.Count == 0 || endCaret1.Line == null || endCaret1.Line.TextObjects.Count == 0)
      return false;
    PdfUndoSnapshot pdfUndoSnapshot = (PdfUndoSnapshot) null;
    if (buildUndoItem)
      pdfUndoSnapshot = new PdfUndoSnapshot(this, this.curCaret, this.endCaret, this.paragraph.Lines.IndexOf<TextLine>(startCaret1.Line), this.paragraph.Lines.IndexOf<TextLine>(endCaret1.Line));
    if (startCaret1.TextObject == null && startCaret1 == endCaret1 && startCaret1.Line.TextObjects.Count > 0)
    {
      int index1 = start - 1;
      if (index1 >= 0)
      {
        ParagraphCaret caretByIndex1 = this.GetCaretByIndex(index1);
        if (caretByIndex1 != null)
        {
          float dx = caretByIndex1.BottomPoint.X - startCaret1.BottomPoint.X;
          int index2 = start + 1;
          for (ParagraphCaret caretByIndex2 = this.GetCaretByIndex(index2); caretByIndex2 != null && caretByIndex2.Line == startCaret1.Line; caretByIndex2 = this.GetCaretByIndex(index2))
          {
            if (caretByIndex2.TextObject != null)
              caretByIndex2.TextObject.Offset(dx, 0.0f);
            ++index2;
          }
          startCaret1.Line.InvalidateBoundingBox();
          this.paragraph.InvalidateBoundingBox(CalcBoundingBoxOrientation.Vertical);
          this.curCaret = index1;
          this.BuildCarets();
          if (buildUndoItem)
          {
            ModifyParagraphUndoItem undoItem1 = new ModifyParagraphUndoItem();
            undoItem1.UndoType = UndoTypes.DeleteText;
            undoItem1.Caret = index1;
            pdfUndoSnapshot.FillUndoItem(undoItem1, false);
            undoItem = (IPdfUndoItem) undoItem1;
          }
        }
      }
      this.curCaret = start;
      this.endCaret = -1;
      return true;
    }
    bool flag2 = startCaret1.Line.IsLineTail(startCaret1.TextObject, startCaret1.CharIndex);
    if (startCaret1 == endCaret1 & flag2 && startCaret1.Line.ReturnFlag)
      return false;
    int num1 = this.paragraph.Lines.IndexOf<TextLine>(startCaret1.Line);
    int num2 = num1 - 1;
    int index = num1 == -1 || num1 >= this.paragraph.Lines.Count - 1 ? -1 : num1 + 1;
    bool flag3 = this.IsLineHeaderCaret(startCaret1);
    bool flag4 = this.IsLineTailCaret(endCaret1);
    bool flag5 = flag3 & flag4 || flag3 && startCaret1.Line != endCaret1.Line;
    bool flag6 = flag4 & flag3 || flag4 && startCaret1.Line != endCaret1.Line;
    if (startCaret1 == endCaret1 && flag3 | flag4 && startCaret1.TextObject != null && startCaret1.Line.CharCount == 1)
      flag5 = flag6 = startCaret1.TextObject.CharsCount == 1;
    ModifyParagraphUndoItem undoItem2 = (ModifyParagraphUndoItem) null;
    if (buildUndoItem)
    {
      undoItem2 = new ModifyParagraphUndoItem();
      undoItem2.UndoType = UndoTypes.DeleteText;
      undoItem = (IPdfUndoItem) undoItem2;
    }
    bool flag7 = false;
    if (startCaret1.TextObject == endCaret1.TextObject)
    {
      if (flag4 && Math.Abs(end - start) <= 1)
      {
        if (index != -1)
        {
          TextLine line = this.paragraph.Lines[index];
          IPdfUndoItem undoItem3;
          if (this.DeleteAtLineHeader(this.GetLineHeaderCaret(start + 1), buildUndoItem, out undoItem3))
          {
            if (buildUndoItem)
              undoItem = (IPdfUndoItem) new CompositeUndoItem((System.Collections.Generic.IReadOnlyList<IPdfUndoItem>) new IPdfUndoItem[1]
              {
                undoItem3
              })
              {
                PageDict = this.page.Dictionary,
                PageIndex = this.page.PageIndex,
                ParagraphId = this.paragraph.Id,
                PrevCaret = start,
                PrevEndCaret = start
              };
            return true;
          }
        }
        else
          flag7 = false;
      }
      else
        flag7 = this.DeleteTextInOneObject(startCaret1, endCaret1);
    }
    else
      flag7 = this.DeleteTextInMultiObject(startCaret1, endCaret1);
    if (flag7 & buildUndoItem)
      pdfUndoSnapshot.FillUndoItem(undoItem2, false);
    this.curCaret = start;
    this.endCaret = -1;
    return flag7;
  }

  public bool BackspaceAt(int caret, bool buildUndoItem, out IPdfUndoItem undoItem)
  {
    undoItem = (IPdfUndoItem) null;
    IPdfUndoItem undoItem1;
    if (!this.DeleteTextCore(caret - 1, caret - 1, buildUndoItem, out undoItem1) && !this.DeleteAtLineHeader(caret - 1, buildUndoItem, out undoItem1))
      return false;
    this.curCaret = Math.Min(caret - 1, this.caretList.Count - 1);
    this.endCaret = -1;
    if (buildUndoItem && undoItem1 != null)
      undoItem = (IPdfUndoItem) new CompositeUndoItem((System.Collections.Generic.IReadOnlyList<IPdfUndoItem>) new List<IPdfUndoItem>()
      {
        undoItem1
      })
      {
        PageDict = this.page.Dictionary,
        PageIndex = this.page.PageIndex,
        ParagraphId = this.paragraph.Id,
        PrevCaret = caret,
        PrevEndCaret = caret,
        NextCaret = this.curCaret,
        NextEndCaret = this.curCaret
      };
    return true;
  }

  public bool InsertText(int caret, string text, bool buildUndoItem, out IPdfUndoItem undoItem)
  {
    return this.InsertText(caret, text, (CultureInfo) null, buildUndoItem, out undoItem);
  }

  public bool InsertText(
    int caret,
    string text,
    CultureInfo cultureInfo,
    bool buildUndoItem,
    out IPdfUndoItem undoItem)
  {
    undoItem = (IPdfUndoItem) null;
    if (string.IsNullOrEmpty(text))
      return false;
    ParagraphCaret caretByIndex1 = this.GetCaretByIndex(caret);
    if (caretByIndex1 == null)
      return false;
    BoldItalicFlags boldItalic = BoldItalicFlags.None;
    FS_COLOR color = FS_COLOR.Black;
    List<(PdfFont, string, float)> valueTupleList = new List<(PdfFont, string, float)>();
    PdfTextObject textObject = caretByIndex1.TextObject;
    if (textObject == null)
    {
      for (int index = caret - 1; index >= 0; --index)
      {
        ParagraphCaret caretByIndex2 = this.GetCaretByIndex(index);
        if (caretByIndex2.TextObject != null)
        {
          textObject = caretByIndex2.TextObject;
          break;
        }
      }
    }
    if (textObject != null)
    {
      PdfFont font = textObject.Font;
      boldItalic = textObject.IsBold() ? BoldItalicFlags.Bold : BoldItalicFlags.None;
      if (textObject.IsItalic())
        boldItalic |= BoldItalicFlags.Italic;
      float fontSize = textObject.GetFontSize();
      color = textObject.FillColor;
      bool flag = true;
      for (int index = 0; index < text.Length; ++index)
      {
        if (!font.ContainsChar(text[index]))
        {
          flag = false;
          break;
        }
      }
      if (flag)
      {
        valueTupleList.Add((font, text, fontSize));
      }
      else
      {
        foreach ((PdfFont font, string text, float fontScale) substFont in (IEnumerable<(PdfFont font, string text, float fontScale)>) PdfFontHelper.FindSubstFonts(this.document, text, font, cultureInfo))
          valueTupleList.Add((substFont.font, substFont.text, fontSize * substFont.fontScale));
      }
    }
    else
    {
      foreach ((PdfFont font, string text, float fontScale) substFont in (IEnumerable<(PdfFont font, string text, float fontScale)>) PdfFontHelper.FindSubstFonts(this.document, text, (PdfFont) null, cultureInfo))
        valueTupleList.Add((substFont.font, substFont.text, 9f * substFont.fontScale));
    }
    List<IPdfUndoItem> undoItems = new List<IPdfUndoItem>();
    bool flag1 = false;
    PdfTextObject pdfTextObject = (PdfTextObject) null;
    int num = -1;
    for (int index = valueTupleList.Count - 1; index >= 0; --index)
    {
      (PdfFont font, string text1, float fontSize) = valueTupleList[index];
      IPdfUndoItem undoItem1;
      bool flag2 = this.InsertText(caret, font, fontSize, color, text1, boldItalic, 0, ScriptEnum.Normal, cultureInfo, buildUndoItem, out undoItem1);
      flag1 |= flag2;
      if (flag2 && pdfTextObject == null)
      {
        ParagraphCaret caretByIndex3 = this.GetCaretByIndex(this.curCaret);
        if (caretByIndex3 != null)
        {
          pdfTextObject = caretByIndex3.TextObject;
          num = caretByIndex3.CharIndex;
        }
      }
      if (flag2 & buildUndoItem)
        undoItems.Add(undoItem1);
    }
    if (flag1 && undoItems.Count > 0)
    {
      this.tempCaretInfo.TextObject = pdfTextObject;
      this.tempCaretInfo.CharIndex = num;
      this.UpdateCurCaret();
      CompositeUndoItem compositeUndoItem = new CompositeUndoItem((System.Collections.Generic.IReadOnlyList<IPdfUndoItem>) undoItems)
      {
        PageDict = this.page.Dictionary,
        ParagraphId = this.paragraph.Id,
        PageIndex = this.page.PageIndex,
        NextCaret = this.curCaret,
        NextEndCaret = this.endCaret
      };
      undoItem = (IPdfUndoItem) compositeUndoItem;
    }
    return flag1;
  }

  public bool InsertText(
    int caret,
    PdfFont font,
    float fontSize,
    FS_COLOR color,
    string text,
    BoldItalicFlags boldItalic,
    int underLineStrikeout,
    ScriptEnum script,
    bool buildUndoItem,
    out IPdfUndoItem undoItem)
  {
    return this.InsertText(caret, font, fontSize, color, text, boldItalic, underLineStrikeout, script, (CultureInfo) null, buildUndoItem, out undoItem);
  }

  public bool InsertText(
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
    out IPdfUndoItem undoItem)
  {
    undoItem = (IPdfUndoItem) null;
    ParagraphCaret paragraphCaret1 = this.GetCaretByIndex(caret);
    if (paragraphCaret1 == null)
      return false;
    bool flag1 = false;
    ParagraphCaret caretByIndex1 = this.GetCaretByIndex(caret + 1);
    if (caretByIndex1 == null || caretByIndex1.Line != paragraphCaret1.Line)
      flag1 = true;
    int num1 = this.paragraph.Lines.IndexOf<TextLine>(paragraphCaret1.Line);
    PdfUndoSnapshot pdfUndoSnapshot = (PdfUndoSnapshot) null;
    ModifyParagraphUndoItem undoItem1 = (ModifyParagraphUndoItem) null;
    if (buildUndoItem)
    {
      pdfUndoSnapshot = new PdfUndoSnapshot(this, caret, caret, num1, num1);
      undoItem1 = new ModifyParagraphUndoItem();
      undoItem = (IPdfUndoItem) undoItem1;
      undoItem1.UndoType = UndoTypes.InsertText;
      undoItem1.Caret = caret;
      undoItem1.PdfFont = font;
      undoItem1.FontSize = fontSize;
      undoItem1.Text = text;
      undoItem1.CultureInfo = cultureInfo;
      undoItem1.TextColor = color;
    }
    this.curCaret = caret;
    this.endCaret = -1;
    FS_RECTF boundingBox1 = this.paragraph.GetBoundingBox(false);
    FS_RECTF boundingBox2 = paragraphCaret1.Line.GetBoundingBox(false);
    bool flag2 = false;
    ParagraphCaret paragraphCaret2 = (ParagraphCaret) null;
    int num2 = 0;
    PdfTextObject textObj;
    if (paragraphCaret1.TextObject != null)
    {
      if (paragraphCaret1.CharIndex == 0 && caret - 1 >= 0)
      {
        ParagraphCaret caretByIndex2 = this.GetCaretByIndex(caret - 1);
        if (caretByIndex2.Line == paragraphCaret1.Line && caretByIndex2.TextObject != null)
        {
          paragraphCaret2 = paragraphCaret1;
          paragraphCaret1 = caretByIndex2;
          ++paragraphCaret1.CharIndex;
        }
      }
      num2 = paragraphCaret1.TextObject.CharsCount;
      textObj = this.paragraph.InsertText(paragraphCaret1.Line, paragraphCaret1.TextObject, paragraphCaret1.CharIndex, text, font, fontSize, color, color, boldItalic, underLineStrikeout, script, cultureInfo);
    }
    else if (paragraphCaret1.Line.TextObjects.Count == 0)
    {
      TextLine textLine1 = (TextLine) null;
      TextLine textLine2 = (TextLine) null;
      int num3 = -1;
      for (int index = 0; index < this.paragraph.Lines.Count; ++index)
      {
        TextLine line = this.paragraph.Lines[index];
        if (line == paragraphCaret1.Line)
        {
          num3 = index;
          if (textLine1 != null && textLine1.TextObjects.Count > 0)
            break;
        }
        if (line.TextObjects.Count > 0)
        {
          if (num3 != -1 && index > num3)
          {
            textLine2 = line;
            break;
          }
          if (index != num3)
            textLine1 = line;
        }
      }
      PdfPageObjectsCollection container;
      int insertAt;
      if (textLine1 != null)
      {
        container = textLine1.TextObjects[textLine1.TextObjects.Count - 1].Container;
        insertAt = container.Count;
      }
      else if (textLine2 != null)
      {
        container = textLine2.TextObjects[0].Container;
        insertAt = 0;
      }
      else
      {
        container = this.page.PageObjects;
        insertAt = container.Count;
      }
      flag2 = true;
      textObj = this.paragraph.InsertTextToEmptyLine(paragraphCaret1.Line, container, insertAt, text, font, fontSize, color, color, boldItalic, underLineStrikeout, script);
    }
    else
      textObj = this.paragraph.InsertText(paragraphCaret1.Line, paragraphCaret1.BottomPoint.X, paragraphCaret1.BottomPoint.Y, text, font, fontSize, color, color, boldItalic, underLineStrikeout, script);
    if (textObj == null)
      return false;
    if (paragraphCaret2 != null)
    {
      this.tempCaretInfo.TextObject = paragraphCaret2.TextObject;
      this.tempCaretInfo.CharIndex = paragraphCaret2.CharIndex;
    }
    else
    {
      this.tempCaretInfo.TextObject = textObj;
      if (textObj == paragraphCaret1.TextObject)
      {
        int charsCount = textObj.CharsCount;
        this.tempCaretInfo.CharIndex = paragraphCaret1.CharIndex + (charsCount - num2);
      }
      else
        this.tempCaretInfo.CharIndex = textObj.CharsCount;
    }
    if (this.Lines.Count == 1)
    {
      this.Lines[0].GetBoundingBox(false);
      FS_RECTF box = textObj.GetBox();
      float offsetY = boundingBox2.Height - box.Height;
      float num4 = boundingBox2.top - box.top;
      if ((double) offsetY <= -0.5 && (double) num4 <= -0.5)
        this.paragraph.OffsetLinesVert(offsetY, paragraphCaret1.Line);
      this.paragraph.InvalidateBoundingBox();
      this.BuildCarets();
      this.UpdateCurCaret();
      if (buildUndoItem)
      {
        pdfUndoSnapshot.FillUndoItem(undoItem1, false);
        undoItem1.StartLineIndex = 0;
        return true;
      }
    }
    FS_RECTF boundingBox3 = paragraphCaret1.Line.GetBoundingBox(false);
    if (!this.Paragraph.IsRotate)
    {
      FS_RECTF box = textObj.GetBox();
      float offsetY = boundingBox2.Height - box.Height;
      float num5 = boundingBox2.top - box.top;
      if ((double) offsetY <= -0.5 && (double) num5 <= -0.5)
      {
        this.paragraph.OffsetLinesVert(offsetY, paragraphCaret1.Line);
        boundingBox1 = this.paragraph.GetBoundingBox(false);
      }
      if (!this.paragraph.IsVertWriteMode)
      {
        AlignType align = this.paragraph.Align;
        if ((double) boundingBox3.right <= (double) boundingBox1.right)
        {
          if (align == AlignType.AlignCenter || align == AlignType.AlignRight)
            this.InflateLineToAlign(paragraphCaret1.Line);
        }
        else
        {
          float num6 = boundingBox3.right - boundingBox1.right;
          if ((double) boundingBox3.Width <= (double) boundingBox1.Width)
          {
            this.InflateLineToAlign(paragraphCaret1.Line);
          }
          else
          {
            if ((double) (boundingBox3.left - boundingBox1.left) <= 0.5)
              ;
            if ((double) num6 > 0.10000000149011612)
            {
              List<SplitToken> tokens = this.CalcSplitTokens(paragraphCaret1.Line);
              int index1 = -1;
              if (tokens.Count > 0)
              {
                for (int index2 = tokens.Count - 1; index2 >= 0; --index2)
                {
                  SplitToken splitToken = tokens[index2];
                  if (splitToken.TextObject == this.tempCaretInfo.TextObject && splitToken.SplitCharAt < this.tempCaretInfo.CharIndex)
                  {
                    this.tempCaretInfo.CharIndex -= splitToken.SplitCharAt + 1;
                    if (this.tempCaretInfo.CharIndex == 0)
                      this.tempCaretInfo.CharIndex = 1;
                    index1 = index2;
                    break;
                  }
                }
                this.LayoutPara(paragraphCaret1.Line, tokens, !flag1, (ModifyParagraphUndoItem) null);
                if (index1 >= 0)
                  this.tempCaretInfo.TextObject = tokens[index1].SplitNewTextObject;
              }
            }
          }
        }
        this.BuildCarets();
        this.UpdateCurCaret();
      }
    }
    if (buildUndoItem)
      pdfUndoSnapshot.FillUndoItem(undoItem1, false);
    return true;
  }

  public bool InsertReturn(int caret, bool buildUndoItem, out IPdfUndoItem undoItem)
  {
    undoItem = (IPdfUndoItem) null;
    ParagraphCaret startCaret;
    if (!this.VerifyCarets(caret, caret, out startCaret, out ParagraphCaret _))
      return false;
    FS_RECTF boundingBox1 = this.paragraph.GetBoundingBox(false);
    int num1 = this.paragraph.Lines.IndexOf<TextLine>(startCaret.Line);
    if (num1 == -1)
      return false;
    TextLine textLine = new TextLine(this.page);
    AlignType align = this.paragraph.Align;
    TextLine line1 = num1 > 0 ? this.paragraph.Lines[num1 - 1] : (TextLine) null;
    TextLine line2 = num1 < this.paragraph.Lines.Count - 1 ? this.paragraph.Lines[num1 + 1] : (TextLine) null;
    FS_RECTF boundingBox2 = startCaret.Line.GetBoundingBox(false);
    if (align != AlignType.AlignCenter && align != AlignType.AlignRight)
      boundingBox2.left = boundingBox1.left;
    bool flag1 = startCaret.Line.IsLineHeader(startCaret.TextObject, startCaret.CharIndex);
    bool flag2 = startCaret.Line.IsLineTail(startCaret.TextObject, startCaret.CharIndex);
    PdfUndoSnapshot pdfUndoSnapshot = (PdfUndoSnapshot) null;
    ModifyParagraphUndoItem undoItem1 = (ModifyParagraphUndoItem) null;
    if (buildUndoItem)
    {
      pdfUndoSnapshot = new PdfUndoSnapshot(this, caret, caret, num1, num1 + 1);
      undoItem1 = new ModifyParagraphUndoItem();
      undoItem = (IPdfUndoItem) undoItem1;
      undoItem1.Caret = caret;
      undoItem1.UndoType = UndoTypes.InsertReturn;
      if (!flag1 && !flag2)
      {
        undoItem1.InsertReturnAt = InsertReturnAtEnum.SplitLine;
      }
      else
      {
        undoItem1.StartLineIndex = num1;
        undoItem1.EndLineIndex = num1 + 1;
        undoItem1.InsertReturnAt = flag1 ? InsertReturnAtEnum.LineHeader : InsertReturnAtEnum.LineTail;
      }
    }
    if (flag2)
    {
      if (line2 != null)
      {
        FS_RECTF boundingBox3 = line2.GetBoundingBox(false);
        if (align != AlignType.AlignCenter && align != AlignType.AlignRight)
          boundingBox3.left = boundingBox1.left;
        textLine.SetBox(boundingBox3);
        textLine.IsRotate = this.paragraph.IsRotate;
        textLine.IsVertWriteMode = this.paragraph.IsVertWriteMode;
        float lineSpace = this.GetLineSpace(startCaret.Line, false);
        float num2 = lineSpace;
        float num3 = lineSpace;
        this.paragraph.InsertLine(num1 + 1, textLine);
        for (int index = num1 + 2; index < this.paragraph.Lines.Count; ++index)
        {
          TextLine line3 = this.paragraph.Lines[index];
          if (this.paragraph.IsVertWriteMode)
            line3.Offset(-num2, 0.0f);
          else
            line3.Offset(0.0f, -num3);
        }
      }
      else
      {
        textLine.SetBox(boundingBox2);
        textLine.IsRotate = this.paragraph.IsRotate;
        textLine.IsVertWriteMode = this.paragraph.IsVertWriteMode;
        float lineSpace = this.GetLineSpace(startCaret.Line, true);
        float num4 = lineSpace;
        float num5 = lineSpace;
        this.paragraph.InsertLine(num1 + 1, textLine);
        if (this.paragraph.IsVertWriteMode)
          textLine.Offset(-num4, 0.0f);
        else
          textLine.Offset(0.0f, -num5);
      }
    }
    else if (flag1)
    {
      textLine.SetBox(boundingBox2);
      textLine.IsRotate = this.paragraph.IsRotate;
      textLine.IsVertWriteMode = this.paragraph.IsVertWriteMode;
      float lineSpace = this.GetLineSpace(startCaret.Line, true);
      float num6 = lineSpace;
      float num7 = lineSpace;
      this.paragraph.InsertLine(num1, textLine);
      for (int index = num1 + 1; index < this.paragraph.Lines.Count; ++index)
      {
        TextLine line4 = this.paragraph.Lines[index];
        if (this.paragraph.IsVertWriteMode)
          line4.Offset(-num6, 0.0f);
        else
          line4.Offset(0.0f, -num7);
      }
    }
    else
    {
      PdfTextObject pdfTextObject = startCaret.TextObject != null ? (startCaret.CharIndex != 0 ? (startCaret.CharIndex != startCaret.TextObject.CharsCount ? startCaret.Line.SplitAt(startCaret.TextObject, startCaret.CharIndex, false) : _GetNextObject(caret)) : startCaret.TextObject) : _GetNextObject(caret);
      if (pdfTextObject == null)
        return false;
      textLine.SetBox(boundingBox2);
      textLine.IsRotate = this.paragraph.IsRotate;
      textLine.IsVertWriteMode = this.paragraph.IsVertWriteMode;
      this.paragraph.InsertLine(num1 + 1, textLine);
      startCaret.Line.ReturnFlag = true;
      for (int index = startCaret.Line.TextObjects.Count - 1; index >= 0; --index)
      {
        PdfTextObject textObject = startCaret.Line.TextObjects[index];
        textLine.InsertText(0, textObject);
        startCaret.Line.RemoveObj(index);
        if (textObject == pdfTextObject)
          break;
      }
      float val1_1 = 10f;
      float val1_2 = 10f + boundingBox2.top - boundingBox2.bottom;
      if (line1 != null)
      {
        FS_RECTF boundingBox4 = line1.GetBoundingBox(false);
        float val1_3 = boundingBox4.left - boundingBox2.left;
        float val1_4 = boundingBox4.bottom - boundingBox2.bottom;
        if (line2 != null)
        {
          FS_RECTF boundingBox5 = line2.GetBoundingBox(false);
          float val2_1 = boundingBox2.left - boundingBox5.left;
          float val2_2 = boundingBox2.bottom - boundingBox5.bottom;
          val1_1 = Math.Min(val1_3, val2_1);
          val1_2 = Math.Min(val1_4, val2_2);
        }
        else
        {
          val1_1 = val1_3;
          val1_2 = val1_4;
        }
      }
      else if (line2 != null)
      {
        FS_RECTF boundingBox6 = line2.GetBoundingBox(false);
        float val2_3 = boundingBox2.left - boundingBox6.left;
        float val2_4 = boundingBox2.bottom - boundingBox6.bottom;
        val1_1 = Math.Min(val1_1, val2_3);
        val1_2 = Math.Min(val1_2, val2_4);
      }
      textLine.InvalidateBoundingBox();
      FS_RECTF boundingBox7 = textLine.GetBoundingBox(true);
      if (this.paragraph.IsVertWriteMode)
      {
        float num8 = boundingBox1.top - boundingBox7.top;
        textLine.Offset(0.0f, -num8);
      }
      else
      {
        float dx = boundingBox1.left - boundingBox7.left;
        if (align == AlignType.AlignCenter)
          dx = (float) ((double) (boundingBox1.left + (float) (((double) boundingBox1.right - (double) boundingBox1.left) / 2.0)) - (double) boundingBox7.left - (double) boundingBox7.Width / 2.0);
        else if (align != AlignType.AlignRight)
          ;
        textLine.Offset(dx, 0.0f);
      }
      for (int index = num1 + 1; index < this.paragraph.Lines.Count; ++index)
      {
        TextLine line5 = this.paragraph.Lines[index];
        if (this.paragraph.IsVertWriteMode)
          line5.Offset(-val1_1, 0.0f);
        else
          line5.Offset(0.0f, -val1_2);
      }
    }
    this.paragraph.InvalidateBoundingBox();
    if (align == AlignType.AlignAdjust && !textLine.ReturnFlag && !flag1 && !flag2)
      this.ExtractFollowingLinesToMakeAlign(textLine, true, false);
    this.paragraph.InvalidateBoundingBox();
    if (buildUndoItem)
      pdfUndoSnapshot.FillUndoItem(undoItem1, false);
    this.BuildCarets();
    ++this.curCaret;
    this.endCaret = -1;
    return true;

    PdfTextObject _GetNextObject(int _caret)
    {
      PdfTextObject pdfTextObject = (PdfTextObject) null;
      int index = _caret + 1;
      ParagraphCaret caretByIndex;
      for (caretByIndex = this.GetCaretByIndex(index); caretByIndex.Line == startCaret.Line; caretByIndex = this.GetCaretByIndex(index))
      {
        if (caretByIndex.TextObject != null)
        {
          pdfTextObject = caretByIndex.TextObject;
          break;
        }
        ++index;
      }
      return caretByIndex.TextObject;
    }
  }

  public bool SetStroke(
    int startCaret,
    int endCaret,
    FS_COLOR? color,
    bool buildUndoItem,
    out IPdfUndoItem undoItem)
  {
    return this.SetColorToText(startCaret, endCaret, color, new FS_COLOR?(), buildUndoItem, out undoItem);
  }

  public bool SetTextColor(
    int startCaret,
    int endCaret,
    FS_COLOR color,
    bool buildUndoItem,
    out IPdfUndoItem undoItem)
  {
    return this.SetColorToText(startCaret, endCaret, new FS_COLOR?(color), new FS_COLOR?(color), buildUndoItem, out undoItem);
  }

  public bool SetFont(
    int startCaret,
    int endCaret,
    string fontFamilyName,
    CultureInfo cultureInfo,
    bool buildUndoItem,
    out IPdfUndoItem undoItem)
  {
    undoItem = (IPdfUndoItem) null;
    ParagraphCaret startCaret1;
    ParagraphCaret endCaret1;
    if (string.IsNullOrEmpty(fontFamilyName) || !this.VerifyCarets(startCaret, endCaret, out startCaret1, out endCaret1) || string.IsNullOrEmpty(this.GetText(startCaret, endCaret)))
      return false;
    PdfFont font = (PdfFont) null;
    if (PdfFontHelper.IsStandardFontFace(fontFamilyName))
    {
      try
      {
        font = PdfFont.CreateStock(this.document, fontFamilyName);
      }
      catch
      {
      }
    }
    else
    {
      WindowsFontFamily winFont = WindowsFonts.GetFontFamily(fontFamilyName);
      if (winFont != null)
      {
        FontCharSet[] source = WindowsFonts.MapCultureInfoToCharSet(cultureInfo).Where<FontCharSet>((Func<FontCharSet, bool>) (c => winFont.LOGFONT.ContainsKey(c))).ToArray<FontCharSet>();
        if (source.Length == 0)
          source = (FontCharSet[]) null;
        Dictionary<FontCharSet, int> dictionary = new Dictionary<FontCharSet, int>();
        int num1 = this.paragraph.Lines.IndexOf<TextLine>(startCaret1.Line);
        int num2 = 0;
        for (int index1 = num1; index1 < this.paragraph.Lines.Count; ++index1)
        {
          TextLine line = this.paragraph.Lines[index1];
          int num3 = 0;
          int num4 = line.TextObjects.Count - 1;
          if (line == startCaret1.Line)
            num3 = line.TextObjects.IndexOf<PdfTextObject>(startCaret1.TextObject);
          if (line == endCaret1.Line)
            num4 = line.TextObjects.IndexOf<PdfTextObject>(endCaret1.TextObject);
          for (int index2 = num3; index2 <= num4; ++index2)
          {
            ++num2;
            if (num2 > 1000)
            {
              index1 = this.paragraph.Lines.Count;
              break;
            }
            PdfTextObject textObject = line.TextObjects[index2];
            if (textObject.CharsCount > 0 && textObject.Font != null)
            {
              int Charset;
              Pdfium.FPDFFont_GetSubstFont(textObject.Font.Handle, out IntPtr _, out string _, out Charset, out FontSubstFlags _, out int _, out int _, out bool _, out int _, out bool _);
              int num5;
              if (dictionary.TryGetValue((FontCharSet) Charset, out num5))
                ++num5;
              else
                num5 = 1;
              dictionary[(FontCharSet) Charset] = num5;
            }
          }
          if (line == endCaret1.Line)
            break;
        }
        FontCharSet? nullable = new FontCharSet?();
        FontCharSet key = FontCharSet.DEFAULT_CHARSET;
        int num6 = 0;
        foreach (KeyValuePair<FontCharSet, int> keyValuePair in dictionary)
        {
          if (winFont.LOGFONT.ContainsKey(keyValuePair.Key) && keyValuePair.Value > num6)
          {
            key = keyValuePair.Key;
            num6 = keyValuePair.Value;
          }
          if (!nullable.HasValue && source != null && ((IEnumerable<FontCharSet>) source).Contains<FontCharSet>(keyValuePair.Key))
            nullable = new FontCharSet?(keyValuePair.Key);
        }
        if (num6 == 0 && nullable.HasValue)
          key = nullable.Value;
        try
        {
          font = PdfFont.CreateWindowsFont(this.document, winFont.LOGFONT[key], false, false);
        }
        catch
        {
        }
      }
    }
    return font != null && this.SetFont(startCaret, endCaret, font, buildUndoItem, out undoItem);
  }

  public bool SetFont(
    int startCaret,
    int endCaret,
    PdfFont font,
    bool buildUndoItem,
    out IPdfUndoItem undoItem)
  {
    return this.SetFontInfo(startCaret, endCaret, font, new float?(), buildUndoItem, out undoItem);
  }

  public bool SetFontSize(
    int startCaret,
    int endCaret,
    float fontSize,
    bool buildUndoItem,
    out IPdfUndoItem undoItem)
  {
    return this.SetFontInfo(startCaret, endCaret, (PdfFont) null, new float?(fontSize), buildUndoItem, out undoItem);
  }

  public bool SetItalic(
    int startCaret,
    int endCaret,
    bool buildUndoItem,
    out IPdfUndoItem undoItem)
  {
    return this.SetItalic(startCaret, endCaret, (CultureInfo) null, buildUndoItem, out undoItem);
  }

  public bool SetItalic(
    int startCaret,
    int endCaret,
    CultureInfo cultureInfo,
    bool buildUndoItem,
    out IPdfUndoItem undoItem)
  {
    bool flag = this.IsAllBoldItalic(startCaret, endCaret, BoldItalicFlags.Italic);
    return this.SetBoldItalic(startCaret, endCaret, !flag, false, cultureInfo, buildUndoItem, out undoItem);
  }

  public bool SetBold(int startCaret, int endCaret, bool buildUndoItem, out IPdfUndoItem undoItem)
  {
    return this.SetBold(startCaret, endCaret, (CultureInfo) null, buildUndoItem, out undoItem);
  }

  public bool SetBold(
    int startCaret,
    int endCaret,
    CultureInfo cultureInfo,
    bool buildUndoItem,
    out IPdfUndoItem undoItem)
  {
    bool flag = this.IsAllBoldItalic(startCaret, endCaret, BoldItalicFlags.Bold);
    return this.SetBoldItalic(startCaret, endCaret, false, !flag, cultureInfo, buildUndoItem, out undoItem);
  }

  private bool SetBoldItalic(
    int startCaret,
    int endCaret,
    bool italic,
    bool bold,
    CultureInfo cultureInfo,
    bool buildUndoItem,
    out IPdfUndoItem undoItem)
  {
    undoItem = (IPdfUndoItem) null;
    ParagraphCaret startCaret1;
    ParagraphCaret endCaret1;
    if (!this.VerifyCarets(startCaret, endCaret, out startCaret1, out endCaret1))
      return false;
    int startLineIndex = this.paragraph.Lines.IndexOf<TextLine>(startCaret1.Line);
    int endLineIndex = this.paragraph.Lines.IndexOf<TextLine>(startCaret1.Line);
    PdfUndoSnapshot pdfUndoSnapshot = (PdfUndoSnapshot) null;
    ModifyParagraphUndoItem undoItem1 = (ModifyParagraphUndoItem) null;
    if (buildUndoItem)
    {
      pdfUndoSnapshot = new PdfUndoSnapshot(this, startCaret, endCaret, startLineIndex, endLineIndex);
      undoItem1 = new ModifyParagraphUndoItem();
      undoItem = (IPdfUndoItem) undoItem1;
      undoItem1.UndoType = italic ? UndoTypes.SetItalic : UndoTypes.SetBold;
      undoItem1.StartLineIndex = startLineIndex;
      undoItem1.Caret = startCaret;
      undoItem1.EndCaret = endCaret;
      undoItem1.CultureInfo = cultureInfo;
      if (bold)
        undoItem1.BoldItalic |= BoldItalicFlags.Bold;
      if (italic)
        undoItem1.BoldItalic |= BoldItalicFlags.Italic;
    }
    int startTextObjectIndex;
    int endTextObjectIndex;
    if (!this.GetSelectedObjectRange(ref startCaret1, ref endCaret1, out startLineIndex, out startTextObjectIndex, out endLineIndex, out endTextObjectIndex))
      return false;
    TextLine line1 = this.paragraph.Lines[startLineIndex];
    TextLine line2 = this.paragraph.Lines[endLineIndex];
    for (int index1 = startLineIndex; index1 < this.paragraph.Lines.Count; ++index1)
    {
      bool flag = index1 == endLineIndex;
      TextLine line3 = this.paragraph.Lines[index1];
      if (line3.TextObjects.Count > 0)
      {
        line3.GetBoundingBox(false);
        int index2 = 0;
        int num = line3.TextObjects.Count - 1;
        if (line3 == line1)
          index2 = startTextObjectIndex;
        if (line3 == line2)
          num = endTextObjectIndex;
        PdfTextObject textObject1 = line3.TextObjects[index2];
        if (line3 == line1)
        {
          this.tempCaretInfo.Line = line3;
          this.tempCaretInfo.TextObject = textObject1;
          this.tempCaretInfo.CharIndex = 0;
        }
        int startCharIdx = 0;
        PdfTextObject textObject2 = line3.TextObjects[num];
        int endCharIdx = textObject2.CharsCount - 1;
        line3.SetBoldItalic(textObject1, startCharIdx, textObject2, endCharIdx, bold, italic, cultureInfo);
        if (flag)
        {
          this.StoreCaretInfo(line3, endLineIndex, textObject2, num, endCharIdx, false);
          break;
        }
      }
      else if (flag)
        break;
    }
    if (buildUndoItem)
      pdfUndoSnapshot.FillUndoItem(undoItem1, false);
    this.BuildCarets();
    this.UpdateCurCaret();
    return true;
  }

  private bool IsAllBoldItalic(int startCaret, int endCaret, BoldItalicFlags boldItalic)
  {
    int startCaretPos = startCaret;
    if (startCaret == endCaret)
      --startCaret;
    if (!this.VerifyCarets(startCaret, endCaret, out ParagraphCaret _, out ParagraphCaret _) || !this.VerifyCarets(startCaretPos, endCaret, out ParagraphCaret _, out ParagraphCaret _))
      return false;
    bool flag = true;
    int num = startCaret;
    while (num < endCaret)
    {
      ParagraphCaret caretByIndex = this.GetCaretByIndex(num);
      ++num;
      if (caretByIndex.TextObject != null)
      {
        if (!IsBoldItalic(num, boldItalic))
          return false;
        if (caretByIndex.TextObject.CharsCount != 0)
          flag = false;
      }
    }
    return !flag;

    bool IsBoldItalic(int _caret, BoldItalicFlags flag)
    {
      _caret = this.FitCaretIndex(_caret);
      ParagraphCaret caretByIndex = this.GetCaretByIndex(_caret);
      if (caretByIndex?.TextObject != null)
      {
        switch (flag)
        {
          case BoldItalicFlags.Bold:
            return caretByIndex.TextObject.IsBold();
          case BoldItalicFlags.Italic:
            return caretByIndex.TextObject.IsItalic();
          case BoldItalicFlags.Bold | BoldItalicFlags.Italic:
            return caretByIndex.TextObject.IsBold() && caretByIndex.TextObject.IsItalic();
        }
      }
      return false;
    }
  }

  private bool SetFontInfo(
    int startCaret,
    int endCaret,
    PdfFont font,
    float? fontSize,
    bool buildUndoItem,
    out IPdfUndoItem undoItem)
  {
    undoItem = (IPdfUndoItem) null;
    ParagraphCaret startCaret1;
    ParagraphCaret endCaret1;
    if (font == null && !fontSize.HasValue || !this.VerifyCarets(startCaret, endCaret, out startCaret1, out endCaret1))
      return false;
    if (font == null && fontSize.HasValue)
    {
      bool flag = false;
      for (int caret = startCaret; caret <= endCaret; ++caret)
      {
        if ((double) Math.Abs(this.GetFontSize(caret) - fontSize.Value) > 0.01)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return false;
    }
    FS_RECTF boundingBox1 = this.paragraph.GetBoundingBox(false);
    PdfUndoSnapshot pdfUndoSnapshot = (PdfUndoSnapshot) null;
    ModifyParagraphUndoItem undoItem1 = (ModifyParagraphUndoItem) null;
    int startLineIndex = this.paragraph.Lines.IndexOf<TextLine>(startCaret1.Line);
    int endLineIndex = this.paragraph.Lines.IndexOf<TextLine>(endCaret1.Line);
    if (buildUndoItem)
    {
      pdfUndoSnapshot = new PdfUndoSnapshot(this, startCaret, endCaret, startLineIndex, endLineIndex);
      undoItem1 = new ModifyParagraphUndoItem();
      undoItem = (IPdfUndoItem) undoItem1;
      undoItem1.UndoType = font == null ? UndoTypes.SetFontSize : UndoTypes.SetFont;
      undoItem1.StartLineIndex = startLineIndex;
      undoItem1.Caret = startCaret;
      undoItem1.EndCaret = endCaret;
      undoItem1.PdfFont = font;
      undoItem1.FontSize = fontSize.GetValueOrDefault();
    }
    int startTextObjectIndex;
    int endTextObjectIndex;
    if (!this.GetSelectedObjectRange(ref startCaret1, ref endCaret1, out startLineIndex, out startTextObjectIndex, out endLineIndex, out endTextObjectIndex))
      return false;
    TextLine line1 = this.paragraph.Lines[startLineIndex];
    TextLine line2 = this.paragraph.Lines[endLineIndex];
    line1.GetBoundingBox(false);
    float? y = line1.TextObjects.FirstOrDefault<PdfTextObject>()?.Location.Y;
    bool flag1 = true;
    float num1 = 0.0f;
    for (int index1 = startLineIndex; index1 < this.paragraph.Lines.Count; ++index1)
    {
      TextLine line3 = this.paragraph.Lines[index1];
      if (line3.TextObjects.Count > 0)
      {
        FS_RECTF boundingBox2 = line3.GetBoundingBox(false);
        int index2 = 0;
        int num2 = line3.TextObjects.Count - 1;
        if (line3 == line1)
          index2 = startTextObjectIndex;
        if (line3 == line2)
          num2 = endTextObjectIndex;
        PdfTextObject textObject1 = line3.TextObjects[index2];
        if (line3 == line1)
        {
          this.tempCaretInfo.Line = line1;
          this.tempCaretInfo.TextObject = textObject1;
          this.tempCaretInfo.CharIndex = 0;
        }
        int startCharIdx = 0;
        PdfTextObject textObject2 = line3.TextObjects[num2];
        int endCharIdx = textObject2.CharsCount - 1;
        float num3 = 0.0f;
        if (flag1)
        {
          if (font != null)
            num3 = line3.SetFontInfo(textObject1, startCharIdx, textObject2, endCharIdx, font, new float?());
          else if (fontSize.HasValue)
            num3 = line3.SetFontInfo(textObject1, startCharIdx, textObject2, endCharIdx, (PdfFont) null, fontSize);
        }
        if (line3 == line2)
        {
          this.StoreCaretInfo(line3, endLineIndex, textObject2, num2, endCharIdx, true);
          flag1 = false;
        }
        float num4 = line3.GetBoundingBox(false).bottom - boundingBox2.bottom;
        float num5 = num1 + num3;
        if ((double) num5 != 0.0)
        {
          if (!this.paragraph.IsVertWriteMode)
            line3.Offset(0.0f, num5);
          else
            line3.Offset(num5, 0.0f);
        }
        num1 = num5 + num4;
      }
    }
    if (this.paragraph.Lines.Count > 1 && !this.paragraph.IsRotate && !this.paragraph.IsVertWriteMode)
    {
      bool flag2 = false;
      float num6 = boundingBox1.right - boundingBox1.left;
      int index = startLineIndex;
      while (index < this.paragraph.Lines.Count)
      {
        TextLine line4 = this.paragraph.Lines[index];
        ++index;
        FS_RECTF boundingBox3 = line4.GetBoundingBox(false);
        if ((double) (boundingBox3.right - boundingBox3.left - num6) > 1.0)
        {
          this.LayoutPara(line4, (List<SplitToken>) null, true, (ModifyParagraphUndoItem) null);
          index = this.paragraph.Lines.IndexOf<TextLine>(line4) + 1;
        }
        else if (!line4.ReturnFlag)
        {
          this.ExtractFollowingLinesToMakeAlign(line4, true, false);
          index = this.paragraph.Lines.IndexOf<TextLine>(line4) + 1;
        }
        if (line4 == line2)
          flag2 = true;
        if (flag2 && line4.ReturnFlag)
          break;
      }
    }
    if (buildUndoItem)
      pdfUndoSnapshot.FillUndoItem(undoItem1, false);
    this.paragraph.InvalidateBoundingBox(CalcBoundingBoxOrientation.Vertical);
    this.BuildCarets();
    this.UpdateCurCaret();
    return true;
  }

  public bool IsBold(int startCaret, int endCaret)
  {
    return this.IsAllBoldItalic(startCaret, endCaret, BoldItalicFlags.Bold);
  }

  public bool IsItalic(int startCaret, int endCaret)
  {
    return this.IsAllBoldItalic(startCaret, endCaret, BoldItalicFlags.Italic);
  }

  public PdfFont GetFont(int caret)
  {
    caret = this.FitCaretIndex(caret);
    ParagraphCaret caretByIndex = this.GetCaretByIndex(caret);
    return caretByIndex?.TextObject != null ? caretByIndex.TextObject.Font : (PdfFont) null;
  }

  public float GetFontSize(int caret)
  {
    caret = this.FitCaretIndex(caret);
    ParagraphCaret caretByIndex = this.GetCaretByIndex(caret);
    return caretByIndex?.TextObject != null ? caretByIndex.TextObject.GetFontSize() : 0.0f;
  }

  public FS_COLOR? GetTextColor(int caret)
  {
    caret = this.FitCaretIndex(caret);
    ParagraphCaret caretByIndex = this.GetCaretByIndex(caret);
    return caretByIndex?.TextObject != null ? new FS_COLOR?(caretByIndex.TextObject.FillColor) : new FS_COLOR?(FS_COLOR.Black);
  }

  public float? GetCharSpace(int caret)
  {
    caret = this.FitCaretIndex(caret);
    ParagraphCaret caretByIndex = this.GetCaretByIndex(caret);
    return caretByIndex?.TextObject != null ? new float?(caretByIndex.TextObject.CharSpacing) : new float?(0.0f);
  }

  public float? GetWordSpace(int caret)
  {
    caret = this.FitCaretIndex(caret);
    ParagraphCaret caretByIndex = this.GetCaretByIndex(caret);
    return caretByIndex?.TextObject != null ? new float?(caretByIndex.TextObject.WordSpacing) : new float?(0.0f);
  }

  public AlignType GetAlign(int caret)
  {
    caret = this.FitCaretIndex(caret);
    ParagraphCaret caretByIndex = this.GetCaretByIndex(caret);
    return caretByIndex?.Line != null ? caretByIndex.Line.Align : AlignType.AlignLeft;
  }

  public void SetAlign(
    int startCaret,
    int endCaret,
    AlignType align,
    bool buildUndoItem,
    out IPdfUndoItem undoItem)
  {
    undoItem = (IPdfUndoItem) null;
    ParagraphCaret startCaret1;
    ParagraphCaret endCaret1;
    if (!this.VerifyCarets(startCaret, endCaret, out startCaret1, out endCaret1))
      return;
    this.tempCaretInfo.Line = startCaret1.Line;
    this.tempCaretInfo.TextObject = startCaret1.TextObject;
    if (this.tempCaretInfo.TextObject == null)
    {
      int index = startCaret + 1;
      ParagraphCaret paragraphCaret = startCaret1;
      while (paragraphCaret.TextObject == null)
      {
        paragraphCaret = this.GetCaretByIndex(index);
        if (paragraphCaret != null)
          ++index;
        else
          break;
      }
      if (paragraphCaret != null)
        this.tempCaretInfo.TextObject = paragraphCaret.TextObject;
    }
    this.tempCaretInfo.CharIndex = startCaret1.CharIndex;
    this.tempEndCaretInfo.Line = endCaret1.Line;
    this.tempEndCaretInfo.TextObject = endCaret1.TextObject;
    if (this.tempEndCaretInfo.TextObject == null)
    {
      int index = endCaret - 1;
      ParagraphCaret paragraphCaret = startCaret1;
      while (paragraphCaret.TextObject == null)
      {
        paragraphCaret = this.GetCaretByIndex(index);
        if (paragraphCaret != null)
          --index;
        else
          break;
      }
      if (paragraphCaret != null)
        this.tempEndCaretInfo.TextObject = paragraphCaret.TextObject;
    }
    this.tempEndCaretInfo.CharIndex = endCaret1.CharIndex;
    int editStartLineIndex = this.paragraph.Lines.IndexOf<TextLine>(startCaret1.Line);
    int editEndLineIndex = this.paragraph.Lines.IndexOf<TextLine>(endCaret1.Line);
    PdfUndoSnapshot pdfUndoSnapshot = (PdfUndoSnapshot) null;
    ModifyParagraphUndoItem undoItem1 = (ModifyParagraphUndoItem) null;
    if (buildUndoItem)
    {
      pdfUndoSnapshot = new PdfUndoSnapshot(this, startCaret, endCaret, editStartLineIndex, editEndLineIndex, true);
      undoItem1 = new ModifyParagraphUndoItem();
      undoItem = (IPdfUndoItem) undoItem1;
      undoItem1.UndoType = UndoTypes.SetAlign;
      undoItem1.Align = align;
    }
    this.SetAlign(startCaret1.Line, endCaret1.Line, align);
    if (!buildUndoItem)
      return;
    pdfUndoSnapshot.FillUndoItem(undoItem1, true);
  }

  private void SetAlign(TextLine startLine, TextLine endLine, AlignType align)
  {
    if (align == AlignType.AlignNone)
      return;
    TextLine textLine = (TextLine) null;
    if (!this.paragraph.IsRotate)
    {
      FS_RECTF boundingBox1 = this.paragraph.GetBoundingBox(false);
      FS_RECTF fsRectf = new FS_RECTF();
      for (int index = this.paragraph.Lines.IndexOf<TextLine>(startLine); index < this.paragraph.Lines.Count; ++index)
      {
        TextLine line = this.paragraph.Lines[index];
        FS_RECTF boundingBox2 = line.GetBoundingBox(false);
        FS_RECTF rect = boundingBox2;
        if (line.Align == align)
        {
          textLine = line;
        }
        else
        {
          line.Align = align;
          switch (align)
          {
            case AlignType.AlignLeft:
            case AlignType.AlignAdjust:
              if (!line.IsFollowingUpon)
              {
                float dx = boundingBox1.left - boundingBox2.left;
                line.Offset(dx, 0.0f);
                break;
              }
              if (textLine != null && !rect.IsEmpty())
              {
                float dx = textLine.GetBoundingBox(false).left - rect.left;
                line.Offset(dx, 0.0f);
              }
              break;
            case AlignType.AlignCenter:
              float dx1 = (float) ((double) boundingBox1.left + (double) boundingBox1.Width / 2.0 - ((double) boundingBox2.left + (double) boundingBox2.Width / 2.0));
              line.Offset(dx1, 0.0f);
              break;
            case AlignType.AlignRight:
              float dx2 = boundingBox1.right - boundingBox2.right;
              line.Offset(dx2, 0.0f);
              break;
          }
          textLine = line;
        }
        if (line == endLine)
          break;
      }
    }
    this.BuildCarets();
    this.UpdateCurCaret();
  }

  public void Offset(float dx, float dy, bool buildUndoItem, out IPdfUndoItem undoItem)
  {
    undoItem = (IPdfUndoItem) null;
    if (buildUndoItem)
    {
      ModifyParagraphUndoItem paragraphUndoItem = new ModifyParagraphUndoItem();
      undoItem = (IPdfUndoItem) paragraphUndoItem;
      paragraphUndoItem.UndoType = UndoTypes.OffsetParagraph;
      paragraphUndoItem.ParagraphOffsetX = dx;
      paragraphUndoItem.ParagraphOffsetY = dy;
      paragraphUndoItem.PageDict = this.page.Dictionary;
      paragraphUndoItem.PageIndex = this.page.PageIndex;
      paragraphUndoItem.ParagraphId = this.paragraph.Id;
    }
    foreach (TextLine line in (IEnumerable<TextLine>) this.paragraph.Lines)
      line.Offset(dx, dy);
    this.paragraph.InvalidateBoundingBox();
    this.BuildCarets();
  }

  public void SetCurrentCarets(int start, int end)
  {
    this.curCaret = start;
    this.endCaret = end;
  }

  private bool UpdateCurCaret()
  {
    if (this.caretList == null)
      return false;
    bool flag1 = this.endCaret > -1 && this.curCaret > -1 && this.endCaret != this.curCaret;
    if (this.tempCaretType == PdfParagraphImpl.TempCaretType.ParaTail)
    {
      this.endCaret = this.caretList.Count - 1;
      flag1 = false;
    }
    if (flag1 && this.tempEndCaretInfo.TextObject == null && this.tempCaretType != 0)
    {
      for (int index = this.caretList.Count - 1; index >= 0; --index)
      {
        ParagraphCaret caret = this.caretList[index];
        if (caret.Line != null && caret.Line.TextObjects.Count > 0)
          this.endCaret = index;
      }
    }
    bool flag2 = false;
    bool flag3 = false;
    int num1 = -1;
    for (int index1 = 0; index1 < this.caretList.Count; ++index1)
    {
      ++num1;
      ParagraphCaret caret = this.caretList[index1];
      IntPtr? handle1;
      int num2;
      if (this.tempCaretInfo.TextObject != null)
      {
        handle1 = caret.TextObject?.Handle;
        IntPtr handle2 = this.tempCaretInfo.TextObject.Handle;
        num2 = handle1.HasValue ? (handle1.GetValueOrDefault() == handle2 ? 1 : 0) : 0;
      }
      else
        num2 = 0;
      if (num2 != 0)
      {
        int charsCount = caret.TextObject.CharsCount;
        if (this.tempCaretInfo.CharIndex >= charsCount)
        {
          this.curCaret = num1 + charsCount;
          flag2 = true;
        }
        else if (caret.CharIndex == this.tempCaretInfo.CharIndex)
        {
          this.curCaret = num1;
          flag2 = true;
        }
        if (!flag1 & flag2)
          return true;
      }
      if (flag1)
      {
        int num3;
        if (this.tempEndCaretInfo.TextObject != null)
        {
          handle1 = caret.TextObject?.Handle;
          IntPtr handle3 = this.tempEndCaretInfo.TextObject.Handle;
          num3 = handle1.HasValue ? (handle1.GetValueOrDefault() == handle3 ? 1 : 0) : 0;
        }
        else
          num3 = 0;
        if (num3 != 0)
        {
          if (this.tempCaretType != 0)
          {
            if (this.tempEndCaretInfo.Line.IsLineHeader(this.tempEndCaretInfo.TextObject, 0))
            {
              for (int index2 = num1 - 1; index2 >= 0; --index2)
              {
                ParagraphCaret caretByIndex = this.GetCaretByIndex(index2);
                if (caretByIndex != null && caretByIndex.Line.TextObjects.Count > 0)
                {
                  this.endCaret = index2;
                  flag3 = true;
                  if (flag2)
                    return true;
                  break;
                }
              }
              if (!flag3)
              {
                this.endCaret = num1;
                --num1;
              }
              flag3 = true;
              if (flag2)
                return true;
            }
            else
            {
              this.endCaret = num1;
              flag3 = true;
              if (flag2)
                return true;
            }
          }
          else
          {
            int charsCount = this.tempEndCaretInfo.TextObject.CharsCount;
            if (this.tempEndCaretInfo.CharIndex >= charsCount)
            {
              this.endCaret = num1 + charsCount;
              flag3 = true;
            }
            else if (caret.CharIndex == this.tempEndCaretInfo.CharIndex)
            {
              this.endCaret = num1;
              flag3 = true;
            }
            if (flag2 & flag3)
              return true;
          }
        }
      }
    }
    return false;
  }

  private void LayoutPara(
    TextLine startLine,
    List<SplitToken> tokens,
    bool addSpace,
    ModifyParagraphUndoItem undoItem)
  {
    if (tokens == null)
      tokens = this.CalcSplitTokens(startLine);
    if (tokens.Count == 0)
      return;
    float charSpaceCJK = 0.0f;
    SplitToken splitToken = tokens.Last<SplitToken>();
    if (splitToken != null)
    {
      int charsCount = splitToken.TextObject.CharsCount;
      if (charsCount > 1)
      {
        float[] charPos1;
        float[] charPos2;
        if (splitToken.SplitCharAt == 0)
        {
          charPos1 = splitToken.TextObject.GetCharPos(0);
          charPos2 = splitToken.TextObject.GetCharPos(1);
        }
        else if (splitToken.SplitCharAt == charsCount)
        {
          charPos1 = splitToken.TextObject.GetCharPos(charsCount - 2);
          charPos2 = splitToken.TextObject.GetCharPos(charsCount - 1);
        }
        else
        {
          charPos1 = splitToken.TextObject.GetCharPos(splitToken.SplitCharAt - 1);
          charPos2 = splitToken.TextObject.GetCharPos(splitToken.SplitCharAt);
        }
        charSpaceCJK = charPos2[0] - charPos1[2];
      }
    }
    List<TextLine> source = new List<TextLine>();
    for (int index1 = tokens.Count - 1; index1 >= 0; --index1)
    {
      SplitToken token = tokens[index1];
      int charsCount = token.TextObject.CharsCount;
      if (token.SplitCharAt > charsCount - 1)
      {
        int index2 = startLine.TextObjects.IndexOf<PdfTextObject>(token.TextObject) + 1;
        if (index2 < startLine.TextObjects.Count)
        {
          token.TextObject = startLine.TextObjects[index2];
          token.ObjectIndex = index2;
          token.SplitCharAt = 0;
        }
        else
          continue;
      }
      PdfTextObject pdfTextObject = token.SplitCharAt != 0 ? startLine.SplitAt(token.TextObject, token.SplitCharAt, true) : token.TextObject;
      if (pdfTextObject == null || pdfTextObject.CharsCount == 0)
        return;
      token.SplitNewTextObject = pdfTextObject;
      TextLine textLine = new TextLine(this.page);
      for (int index3 = startLine.TextObjects.Count - 1; index3 >= 0; --index3)
      {
        PdfTextObject textObject = startLine.TextObjects[index3];
        startLine.RemoveObj(index3);
        textLine.InsertText(0, textObject);
        if (textObject == pdfTextObject)
          break;
      }
      source.Insert(0, textLine);
    }
    startLine.InvalidateBoundingBox();
    if (source.Count == 0)
      return;
    AlignType align = this.paragraph.Align;
    int num1;
    switch (align)
    {
      case AlignType.AlignLeft:
      case AlignType.AlignAdjust:
        num1 = 1;
        break;
      default:
        num1 = align == AlignType.AlignRight ? 1 : 0;
        break;
    }
    bool flag1 = num1 != 0;
    if (startLine.ReturnFlag)
    {
      flag1 = false;
      startLine.ReturnFlag = false;
      int count = source.Count;
      source[count - 1].ReturnFlag = true;
    }
    float lineSpace1 = this.GetLineSpace(startLine, false);
    float dy = 0.0f;
    int num2 = flag1 ? source.Count - 1 : source.Count;
    int index4 = -1;
    if (!this.paragraph.IsRotate)
    {
      for (int index5 = 0; index5 < num2; ++index5)
      {
        TextLine line = source[index5];
        if (!this.paragraph.IsVertWriteMode)
        {
          FS_RECTF boundingBox = line.GetBoundingBox(true);
          float dx = this.paragraph.GetBoundingBox(false).left - boundingBox.left;
          dy -= lineSpace1;
          line.Offset(dx, dy);
          this.InflateLineToAlign(line);
          if (index4 == -1)
            index4 = this.paragraph.Lines.IndexOf<TextLine>(startLine) + 1;
          this.paragraph.InsertLine(index4, line);
          ++index4;
        }
      }
      if ((double) dy != 0.0)
      {
        for (int index6 = index4; index6 < this.paragraph.Lines.Count; ++index6)
          this.paragraph.Lines[index6].Offset(0.0f, dy);
      }
    }
    if (flag1)
    {
      TextLine textLine = (TextLine) null;
      int index7 = index4 != -1 ? index4 : this.paragraph.Lines.IndexOf<TextLine>(startLine) + 1;
      if (index7 < this.paragraph.Lines.Count)
        textLine = this.paragraph.Lines[index7];
      bool flag2 = false;
      if (textLine == null && this.paragraph.Lines.Count > 0)
      {
        TextLine baseLine = this.paragraph.Lines.Last<TextLine>();
        FS_RECTF boundingBox = baseLine.GetBoundingBox(false);
        bool flag3 = source.Last<TextLine>().TextObjects[0].IsCJK();
        float lineSpace2 = this.GetLineSpace(baseLine, !flag3);
        float num3 = lineSpace2;
        float num4 = lineSpace2;
        TextLine line = new TextLine(this.page);
        line.SetBox(boundingBox);
        line.IsRotate = this.paragraph.IsRotate;
        line.IsVertWriteMode = this.paragraph.IsVertWriteMode;
        this.paragraph.InsertLine(this.paragraph.Lines.IndexOf<TextLine>(startLine) + 1, line);
        if (this.paragraph.IsVertWriteMode)
          line.Offset(-num3, 0.0f);
        else
          line.Offset(0.0f, -num4);
        textLine = line;
        flag2 = true;
        addSpace = false;
      }
      bool flag4 = false;
      TextLine line1 = source.Last<TextLine>();
      float ascent = 0.0f;
      float descent = 0.0f;
      textLine.CombinAtHeader(line1, addSpace, charSpaceCJK, out ascent, out descent);
      if (!this.paragraph.IsRotate && !this.paragraph.IsVertWriteMode && ((double) Math.Abs(ascent) > 1.0 || (double) Math.Abs(descent) > 1.0))
      {
        textLine.Offset(0.0f, ascent);
        for (int index8 = this.paragraph.Lines.IndexOf<TextLine>(textLine) + 1; index8 < this.paragraph.Lines.Count; ++index8)
          this.paragraph.Lines[index8].Offset(0.0f, ascent + descent);
      }
      bool flag5 = true;
      if (flag2)
      {
        switch (this.paragraph.Align)
        {
          case AlignType.AlignNone:
          case AlignType.AlignLeft:
          case AlignType.AlignAdjust:
            flag5 = false;
            break;
          default:
            flag5 = true;
            break;
        }
      }
      if (flag5)
        this.ForceAlign(textLine);
      if (!this.paragraph.IsRotate)
      {
        FS_RECTF boundingBox1 = textLine.GetBoundingBox(true);
        FS_RECTF boundingBox2 = this.paragraph.GetBoundingBox(false);
        textLine.TextObjects[0].GetCharPos(0);
        if ((double) boundingBox1.Width > (double) boundingBox2.Width)
        {
          float dx = boundingBox2.left - boundingBox1.left;
          if ((double) dx != 0.0)
            textLine.Offset(dx, 0.0f);
          flag4 = true;
        }
      }
      else
        flag4 = true;
      if (flag4)
        this.LayoutPara(textLine, (List<SplitToken>) null, true, undoItem);
    }
    this.paragraph.InvalidateBoundingBox(CalcBoundingBoxOrientation.Vertical);
  }

  private bool InflateLineToAlign(TextLine line)
  {
    if (line.TextObjects.Count == 0)
      return false;
    PdfTextObject textObject1 = line.TextObjects[0];
    PdfTextObject textObject2 = line.TextObjects[line.TextObjects.Count - 1];
    if (!this.paragraph.IsRotate && !this.paragraph.IsVertWriteMode)
    {
      FS_RECTF boundingBox = this.paragraph.GetBoundingBox(false);
      switch (this.paragraph.Align)
      {
        case AlignType.AlignLeft:
          this.InflateAlignLeftHorizontal(line, boundingBox);
          break;
        case AlignType.AlignCenter:
          this.InflateAlignCenterHorizontal(line, boundingBox);
          break;
        case AlignType.AlignRight:
          this.InflateAlignRightHorizontal(line, boundingBox);
          break;
        case AlignType.AlignAdjust:
          this.InflateAlignAdjustHorizontal(line, boundingBox);
          break;
      }
    }
    return true;
  }

  private void InflateAlignLeftHorizontal(TextLine line, FS_RECTF paraRect)
  {
  }

  private void InflateAlignCenterHorizontal(TextLine line, FS_RECTF paraRect)
  {
    FS_RECTF boundingBox = line.GetBoundingBox(false);
    float num1 = paraRect.left + paraRect.Width / 2f;
    float num2 = boundingBox.left + boundingBox.Width / 2f;
    line.Offset(num1 - num2, 0.0f);
  }

  private void InflateAlignRightHorizontal(TextLine line, FS_RECTF paraRect)
  {
    FS_RECTF boundingBox = line.GetBoundingBox(false);
    if ((double) boundingBox.Width > (double) paraRect.Width)
      return;
    line.Offset(paraRect.right - boundingBox.right, 0.0f);
  }

  private void InflateAlignAdjustHorizontal(TextLine line, FS_RECTF paraRect)
  {
    FS_RECTF boundingBox1 = line.GetBoundingBox(false);
    if ((double) boundingBox1.Width > (double) paraRect.Width || line.GetSymbolAndSpacePos().Count == 0)
      return;
    float num1 = paraRect.right - boundingBox1.right;
    int num2 = 0;
    while ((double) num1 > 4.0)
    {
      ++num2;
      if (num2 <= 99)
      {
        for (int index1 = 0; index1 < line.TextObjects.Count; ++index1)
        {
          PdfTextObject textObject = line.TextObjects[index1];
          FS_RECTF box = textObject.GetBox();
          float num3 = textObject.WordSpacing + 0.5f;
          textObject.WordSpacing = num3;
          float dx = textObject.GetBox().Width - box.Width;
          if ((double) dx != 0.0)
          {
            for (int index2 = index1; index2 < line.TextObjects.Count; ++index2)
              line.TextObjects[index2].Offset(dx, 0.0f);
          }
        }
        line.InvalidateBoundingBox();
        FS_RECTF boundingBox2 = line.GetBoundingBox(true);
        num1 = paraRect.right - boundingBox2.right;
        if ((double) num1 < -4.0)
        {
          int num4 = 0;
          while (num4 < 100)
          {
            ++num4;
            for (int index3 = 0; index3 < line.TextObjects.Count; ++index3)
            {
              PdfTextObject textObject = line.TextObjects[index3];
              FS_RECTF box = textObject.GetBox();
              float num5 = textObject.WordSpacing - 0.1f;
              textObject.WordSpacing = num5;
              float dx = textObject.GetBox().Width - box.Width;
              if ((double) dx != 0.0)
              {
                for (int index4 = index3; index4 < line.TextObjects.Count; ++index4)
                  line.TextObjects[index4].Offset(dx, 0.0f);
              }
            }
            line.InvalidateBoundingBox();
            FS_RECTF boundingBox3 = line.GetBoundingBox(true);
            if ((double) (paraRect.right - boundingBox3.right) > -4.0)
              break;
          }
        }
      }
      else
        break;
    }
    if (line.TextObjects.Count <= 1)
      return;
    FS_RECTF boundingBox4 = line.GetBoundingBox(true);
    float dx1 = (paraRect.right - boundingBox4.right) / (float) (line.TextObjects.Count - 1);
    for (int index = line.TextObjects.Count - 1; index >= 0; --index)
      line.TextObjects[index].Offset(dx1, 0.0f);
    line.InvalidateBoundingBox();
  }

  private List<SplitToken> CalcSplitTokens(TextLine line)
  {
    FS_RECTF boundingBox1 = line.GetBoundingBox(false);
    FS_RECTF boundingBox2 = this.paragraph.GetBoundingBox(false);
    List<SplitToken> splitTokenList = new List<SplitToken>();
    if (!this.paragraph.IsRotate)
    {
      float bounder = boundingBox2.right;
      if (!this.paragraph.IsVertWriteMode)
      {
        int num1 = 0;
        float num2 = boundingBox1.Width - boundingBox2.Width;
        while ((double) num2 >= 0.10000000149011612)
        {
          ++num1;
          PdfTextObject toSplit;
          int objIdx;
          int charIdx;
          if (num1 <= 100 && PdfParagraphImpl.CalcLineSplitInfo(line, bounder, out toSplit, out objIdx, out charIdx) && (toSplit != null || charIdx != -1))
          {
            splitTokenList.Add(new SplitToken()
            {
              ObjectIndex = objIdx,
              SplitCharAt = charIdx,
              TextObject = toSplit
            });
            float[] charPos = toSplit.GetCharPos(charIdx);
            num2 = boundingBox1.Width - (charPos[0] - boundingBox2.left) - boundingBox2.Width;
            bounder = charPos[0] + boundingBox2.Width;
          }
          else
            break;
        }
      }
    }
    return splitTokenList;
  }

  private static bool CalcLineSplitInfo(
    TextLine line,
    float bounder,
    out PdfTextObject toSplit,
    out int objIdx,
    out int charIdx)
  {
    toSplit = (PdfTextObject) null;
    objIdx = -1;
    charIdx = -1;
    PdfTextObject pdfTextObject1 = (PdfTextObject) null;
    if (!line.IsRotate)
    {
      for (int index = 0; index < line.TextObjects.Count; ++index)
      {
        PdfTextObject textObject = line.TextObjects[index];
        FS_RECTF box = textObject.GetBox();
        if (!line.IsVertWriteMode && (double) (box.right - bounder) > 0.0)
        {
          toSplit = textObject;
          objIdx = index;
          if (index + 1 < line.TextObjects.Count)
          {
            pdfTextObject1 = line.TextObjects[index + 1];
            break;
          }
          break;
        }
      }
      if (toSplit == null)
        return false;
      int charsCount1 = toSplit.CharsCount;
      float num1 = 0.0f;
      if (!line.IsVertWriteMode)
      {
        int num2 = -1;
        for (int charIndex = 0; charIndex < charsCount1; ++charIndex)
        {
          float[] charPos = toSplit.GetCharPos(charIndex);
          float num3 = charPos[0];
          float num4 = charPos[2];
          num1 = num3 - 0.5f;
          if ((double) num4 > (double) bounder)
          {
            num2 = charIndex;
            break;
          }
        }
        if (num2 == -1)
          return false;
        if (num2 == charsCount1 - 1 && (toSplit.CharIsPunctuation(num2) || toSplit.CharIsBlankSpace(num2)))
        {
          if (pdfTextObject1 == null)
            return false;
          toSplit = pdfTextObject1;
          charIdx = 0;
          ++objIdx;
        }
        if (toSplit.CharIsCJK(num2))
        {
          if (toSplit.CharIsPunctuation(num2))
          {
            PdfTextObject toSplit1;
            int objIdx1;
            int charIdx1;
            if (PdfParagraphImpl.GetPrevSplitAt(line, toSplit, charIdx, out toSplit1, out objIdx1, out charIdx1))
            {
              toSplit = toSplit1;
              charIdx = charIdx1;
              objIdx = objIdx1;
            }
          }
          else
            charIdx = num2;
          return true;
        }
        if (toSplit.CharIsPunctuation(num2))
        {
          if (num2 + 1 < charsCount1)
          {
            charIdx = num2 + 1;
            if (charIdx == charsCount1 - 1 && (toSplit.CharIsPunctuation(charIdx) || toSplit.CharIsBlankSpace(charIdx)) && pdfTextObject1 != null)
            {
              toSplit = pdfTextObject1;
              charIdx = 0;
              ++objIdx;
              return true;
            }
          }
          else
          {
            for (int index = 0; index < line.TextObjects.Count; ++index)
            {
              if (line.TextObjects[index] == toSplit)
              {
                if (index + 1 < line.TextObjects.Count)
                {
                  toSplit = line.TextObjects[index + 1];
                  charIdx = 0;
                  objIdx = index;
                  break;
                }
                PdfTextObject toSplit2;
                int objIdx2;
                int charIdx2;
                if (PdfParagraphImpl.GetPrevSplitAt(line, toSplit, charIdx, out toSplit2, out objIdx2, out charIdx2))
                {
                  toSplit = toSplit2;
                  charIdx = charIdx2;
                  objIdx = objIdx2;
                }
                break;
              }
            }
            return toSplit != null;
          }
        }
        else
        {
          bool flag1 = toSplit.CharIsBlankSpace(num2);
          bool flag2 = false;
          int charsCount2 = toSplit.CharsCount;
          if (flag1)
          {
            if (num2 == charsCount2 - 1)
            {
              if (objIdx + 1 < line.TextObjects.Count)
              {
                toSplit = line.TextObjects[objIdx + 1];
                charIdx = 0;
                ++objIdx;
                return true;
              }
              flag2 = true;
            }
          }
          else
            flag2 = true;
          charIdx = num2;
          PdfTextObject pdfTextObject2 = toSplit;
          int num5 = charIdx;
          int num6 = objIdx;
          PdfTextObject toSplit3;
          int objIdx3;
          int charIdx3;
          if (flag2 && PdfParagraphImpl.GetPrevSplitAt(line, toSplit, charIdx, out toSplit3, out objIdx3, out charIdx3))
          {
            toSplit = toSplit3;
            charIdx = charIdx3;
            objIdx = objIdx3;
          }
          if (toSplit.CanSplitAt(charIdx))
            return true;
          string lowerInvariant = toSplit.GetText().ToLowerInvariant();
          int num7 = lowerInvariant.IndexOf("http:");
          if (num7 > 0)
          {
            charIdx = num7;
            return true;
          }
          int num8 = lowerInvariant.IndexOf("https:");
          if (num8 > 0)
          {
            charIdx = num8;
            return true;
          }
          int num9 = lowerInvariant.IndexOf("ftp:");
          if (num9 > 0)
          {
            charIdx = num9;
            return true;
          }
          int charsCount3 = toSplit.CharsCount;
          bool flag3 = false;
          for (int index = 0; index < charIdx; ++index)
          {
            int charCode;
            Pdfium.FPDFTextObj_GetCharInfo(toSplit.Handle, index, out charCode, out float _, out float _);
            PdfFont font = toSplit.Font;
            if (font != null)
            {
              char unicode = font.ToUnicode(charCode);
              int num10;
              switch (unicode)
              {
                case ' ':
                case '.':
                  num10 = 1;
                  break;
                default:
                  num10 = CharHelper.IsPunctuation((int) unicode) ? 1 : 0;
                  break;
              }
              if (num10 != 0)
              {
                flag3 = true;
                break;
              }
            }
          }
          if (!flag3 && toSplit != null && charIdx >= 0)
          {
            int length = "certifications".Length;
            if (PdfParagraphImpl.CountCharsWithBounder(line, toSplit, charIdx, bounder) > length)
            {
              toSplit = pdfTextObject2;
              charIdx = num5;
              objIdx = num6;
            }
          }
          return true;
        }
      }
    }
    return false;
  }

  private static int CountCharsWithBounder(
    TextLine line,
    PdfTextObject toSplit,
    int charIdx,
    float bounder)
  {
    int num = 0;
    if (!line.IsRotate)
    {
      for (int charIndex = charIdx; charIndex < toSplit.CharsCount; ++charIndex)
      {
        ++num;
        if ((double) toSplit.GetCharPos(charIndex)[2] > (double) bounder)
          return num;
      }
    }
    return num;
  }

  private static bool GetPrevSplitAt(
    TextLine line,
    PdfTextObject splitObj,
    int splitCharIdx,
    out PdfTextObject toSplit,
    out int objIdx,
    out int charIdx)
  {
    toSplit = (PdfTextObject) null;
    objIdx = -1;
    charIdx = -1;
    bool flag = false;
    for (int index1 = line.TextObjects.Count - 1; index1 >= 0; --index1)
    {
      PdfTextObject textObject = line.TextObjects[index1];
      int charsCount = textObject.CharsCount;
      int num = charsCount - 1;
      if (textObject == splitObj)
      {
        flag = true;
        num = splitCharIdx - 1;
      }
      if (flag)
      {
        for (int index2 = num; index2 > 0; --index2)
        {
          if (textObject.CanSplitAt(index2))
          {
            char unicodeChar = textObject.GetUnicodeChar(index2);
            if ((unicodeChar == '.' || unicodeChar == ',') && PdfParagraphImpl.GetSpitPosBackFromNumberDot(line, splitObj, splitCharIdx, out toSplit, out objIdx, out charIdx))
              return true;
            toSplit = textObject;
            objIdx = index1;
            charIdx = index2;
            if (charIdx == charsCount - 1 && index1 + 1 < line.TextObjects.Count)
            {
              toSplit = line.TextObjects[index1 + 1];
              objIdx = index1 + 1;
              charIdx = 0;
            }
            return true;
          }
        }
      }
    }
    return false;
  }

  private static bool GetSpitPosBackFromNumberDot(
    TextLine line,
    PdfTextObject splitObj,
    int splitCharIdx,
    out PdfTextObject toSplit,
    out int objIdx,
    out int charIdx)
  {
    toSplit = (PdfTextObject) null;
    objIdx = -1;
    charIdx = -1;
    bool flag = false;
    int count = line.TextObjects.Count;
    for (int index1 = count - 1; index1 >= 0; --index1)
    {
      PdfTextObject textObject = line.TextObjects[index1];
      int charsCount1 = textObject.CharsCount;
      int num = charsCount1 - 1;
      if (textObject == splitObj)
      {
        flag = true;
        num = splitCharIdx;
      }
      if (flag)
      {
        for (int index2 = num; index2 >= 0; --index2)
        {
          char unicodeChar = textObject.GetUnicodeChar(index2);
          if (unicodeChar != char.MinValue && unicodeChar != '.' && unicodeChar != ',' && (unicodeChar < '0' || unicodeChar > '9'))
          {
            toSplit = textObject;
            objIdx = index1;
            charIdx = index2;
            int charsCount2 = toSplit.CharsCount;
            if (charIdx == charsCount1 - 1 && index1 + 1 < count)
            {
              toSplit = line.TextObjects[index1 + 1];
              objIdx = index1 + 1;
              charIdx = 0;
            }
            return true;
          }
        }
      }
    }
    return false;
  }

  private float GetLineSpace(TextLine baseLine, bool reduceDesent)
  {
    float lineSpace = 2f;
    if (this.paragraph.Lines.Count == 1)
    {
      FS_RECTF boundingBox = baseLine.GetBoundingBox(false);
      if (!this.paragraph.IsRotate)
        lineSpace += boundingBox.Height;
      return lineSpace;
    }
    TextLine[] textLineArray = new TextLine[2];
    int num = this.paragraph.Lines.IndexOf<TextLine>(baseLine);
    if (num >= 0)
    {
      if (num > 0)
        textLineArray[0] = this.paragraph.Lines[num - 1];
      if (num < this.paragraph.Lines.Count - 1)
        textLineArray[1] = this.paragraph.Lines[num + 1];
    }
    if (!this.paragraph.IsRotate && !this.paragraph.IsVertWriteMode)
    {
      (float _, float baseY1) = this.GetLineBaseLine(baseLine);
      float val1 = (float) ushort.MaxValue;
      if (textLineArray[0] != null)
      {
        (float baseX, float baseY) lineBaseLine = this.GetLineBaseLine(textLineArray[0]);
        float baseX = lineBaseLine.baseX;
        val1 = lineBaseLine.baseY - baseY1;
        if (reduceDesent)
        {
          FS_RECTF boundingBox = baseLine.GetBoundingBox(false);
          val1 += baseY1 - boundingBox.bottom;
        }
      }
      float val2 = (float) ushort.MaxValue;
      if (textLineArray[1] != null)
      {
        (float _, float baseY2) = this.GetLineBaseLine(textLineArray[1]);
        val2 = baseY1 - baseY2;
        if (reduceDesent)
        {
          FS_RECTF boundingBox = baseLine.GetBoundingBox(false);
          val2 += baseY1 - boundingBox.bottom;
        }
      }
      lineSpace = Math.Min(val1, val2);
    }
    return lineSpace;
  }

  private (float baseX, float baseY) GetLineBaseLine(TextLine line)
  {
    int count = line.TextObjects.Count;
    if (count == 0)
    {
      FS_RECTF boundingBox = line.GetBoundingBox(false);
      return (boundingBox.left, boundingBox.bottom);
    }
    float val1_1 = float.MaxValue;
    float val1_2 = float.MaxValue;
    for (int index = 0; index < count; ++index)
    {
      FS_POINTF location = line.TextObjects[index].Location;
      val1_1 = Math.Min(val1_1, location.X);
      val1_2 = Math.Min(val1_2, location.Y);
    }
    return (val1_1, val1_2);
  }

  private bool DeleteTextInOneObject(ParagraphCaret startCaret, ParagraphCaret endCaret)
  {
    if (startCaret.TextObject == null && startCaret == endCaret)
    {
      if (startCaret.Line.TextObjects.Count == 0)
      {
        this.DeleteLines(startCaret.Line, startCaret.Line);
        return true;
      }
      int num1 = this.caretList.IndexOf(startCaret);
      float dx = 0.0f;
      if (num1 != -1)
        ;
      if (num1 + 1 < this.caretList.Count)
      {
        FS_POINTF bottomPoint = startCaret.BottomPoint;
        double x1 = (double) bottomPoint.X;
        bottomPoint = this.caretList[num1 + 1].BottomPoint;
        double x2 = (double) bottomPoint.X;
        dx = (float) (x1 - x2);
      }
      else if (num1 - 1 >= 0)
      {
        FS_POINTF bottomPoint = this.caretList[num1 - 1].BottomPoint;
        double x3 = (double) bottomPoint.X;
        bottomPoint = startCaret.BottomPoint;
        double x4 = (double) bottomPoint.X;
        dx = (float) (x3 - x4);
      }
      int num2 = -1;
      if (num1 + 1 < this.caretList.Count)
      {
        for (int index = num1 + 1; index < this.caretList.Count && this.caretList[index].Line == startCaret.Line; ++index)
        {
          if (this.caretList[index].TextObject != null)
          {
            num2 = index;
            break;
          }
        }
      }
      if (num2 < 0)
        return false;
      if ((double) dx != 0.0)
      {
        for (int index = startCaret.Line.TextObjects.Count - 1; index >= 0; --index)
          startCaret.Line.TextObjects[index].Offset(dx, 0.0f);
      }
      if (num1 - 1 >= 0)
        this.curCaret = this.endCaret = num1 - 1;
      this.BuildCarets();
      return true;
    }
    int num3 = startCaret.Line.TextObjects.IndexOf<PdfTextObject>(startCaret.TextObject);
    int index1 = num3 - 1;
    int index2 = num3 + 1;
    FS_RECTF box = startCaret.TextObject.GetBox();
    bool flag = endCaret.CharIndex == startCaret.TextObject.CharsCount;
    int startIndex = startCaret.CharIndex;
    int endIndex = flag ? startCaret.TextObject.CharsCount - 1 : endCaret.CharIndex;
    if (flag && startCaret == endCaret)
      startIndex = endIndex;
    bool totalAndRemoved;
    startCaret.TextObject.DeleteText(startIndex, endIndex, out totalAndRemoved);
    if (!this.paragraph.IsRotate)
    {
      float num4;
      if (!this.paragraph.IsVertWriteMode)
      {
        if (!totalAndRemoved)
        {
          num4 = startCaret.TextObject.GetBox().Width - box.Width;
        }
        else
        {
          num4 = -box.Width;
          if (index1 >= 0)
          {
            PdfTextObject textObject1 = startCaret.Line.TextObjects[index1];
            if (index2 < startCaret.Line.TextObjects.Count)
            {
              PdfTextObject textObject2 = startCaret.Line.TextObjects[index2];
              num4 = textObject1.GetBox().right - textObject2.GetBox().left;
            }
          }
        }
      }
      else
      {
        num4 = box.Height;
        if (!totalAndRemoved)
          num4 = startCaret.TextObject.GetBox().Height - box.Height;
      }
      for (int index3 = startCaret.Line.TextObjects.Count - 1; index3 >= 0; --index3)
      {
        PdfTextObject textObject = startCaret.Line.TextObjects[index3];
        if (textObject != startCaret.TextObject)
        {
          if (!this.paragraph.IsVertWriteMode)
            textObject.Offset(num4, 0.0f);
          else
            textObject.Offset(0.0f, num4);
        }
        else
          break;
      }
    }
    if (totalAndRemoved)
    {
      PdfTextObject textObject = startCaret.Line.TextObjects[startCaret.Line.TextObjects.Count - 1];
      if (startCaret.TextObject == textObject)
      {
        int index4 = this.curCaret - 1;
        while (index4 > 0)
        {
          ParagraphCaret caretByIndex = this.GetCaretByIndex(index4);
          if (caretByIndex?.TextObject == null || caretByIndex.TextObject.CharsCount == 0)
          {
            --index4;
            this.curCaret = index4;
          }
          else
          {
            if (caretByIndex.TextObject.CharIsBlankSpace(caretByIndex.TextObject.CharsCount - 1))
            {
              this.curCaret = index4;
              break;
            }
            break;
          }
        }
      }
      int index5 = startCaret.Line.TextObjects.IndexOf<PdfTextObject>(startCaret.TextObject);
      startCaret.Line.RemoveObj(index5);
    }
    FS_RECTF boundingBox1 = startCaret.Line.GetBoundingBox(false);
    startCaret.Line.InvalidateBoundingBox();
    if (startCaret.Line.TextObjects.Count == 0)
    {
      if (!this.paragraph.IsRotate)
      {
        FS_RECTF boundingBox2 = this.paragraph.GetBoundingBox(false);
        boundingBox1.left = boundingBox2.left;
        boundingBox1.right = boundingBox2.right;
        startCaret.Line.SetBox(boundingBox1);
      }
    }
    else if (startCaret.Line.Align == AlignType.AlignAdjust && !startCaret.Line.ReturnFlag)
      this.ExtractFollowingLinesToMakeAlign(startCaret.Line, true, false);
    this.BuildCarets();
    return true;
  }

  private bool DeleteTextInMultiObject(ParagraphCaret startCaret, ParagraphCaret endCaret)
  {
    float num1 = 0.0f;
    if (this.paragraph.Lines.Count > 0)
    {
      FS_RECTF boundingBox = this.paragraph.Lines[0].GetBoundingBox(false);
      num1 = !this.paragraph.IsRotate ? boundingBox.Height : boundingBox.top - boundingBox.bottom;
    }
    int startLineIndex;
    int startTextObjectIndex;
    int endLineIndex;
    int endTextObjectIndex;
    if (!this.GetSelectedObjectRange(ref startCaret, ref endCaret, out startLineIndex, out startTextObjectIndex, out endLineIndex, out endTextObjectIndex))
      return false;
    TextLine line1 = this.paragraph.Lines[startLineIndex];
    TextLine line2 = this.paragraph.Lines[endLineIndex];
    if (line1.ReturnFlag && line1 != line2)
      line1.ReturnFlag = false;
    float num2 = 0.0f;
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = startCaret.Line == endCaret.Line;
    for (int index1 = startLineIndex; index1 <= endLineIndex; ++index1)
    {
      bool flag4 = index1 == endLineIndex;
      TextLine line3 = this.paragraph.Lines[index1];
      FS_RECTF boundingBox1 = line3.GetBoundingBox(false);
      int index2 = 0;
      int index3 = line3.TextObjects.Count - 1;
      if (line3 == line1)
        index2 = startTextObjectIndex;
      if (line3 == line2)
        index3 = endTextObjectIndex;
      line3.RemoveText(line3.TextObjects[index2], 0, line3.TextObjects[index3], line3.TextObjects[index3].CharsCount - 1);
      if (line3.TextObjects.Count == 0)
      {
        if (!flag1)
          flag1 = line3 == line1;
        if (!flag2)
          flag2 = line3 == line2;
        if (index1 < this.paragraph.Lines.Count - 1)
        {
          FS_RECTF boundingBox2 = this.paragraph.Lines[index1 + 1].GetBoundingBox(false);
          if (!this.paragraph.IsRotate)
          {
            if (!this.paragraph.IsVertWriteMode)
              num2 += boundingBox1.top - boundingBox2.top;
            else
              num2 += boundingBox1.right - boundingBox2.right;
          }
          if (flag2 && endLineIndex < this.paragraph.Lines.Count)
            ++endLineIndex;
        }
        else if (endLineIndex < this.paragraph.Lines.Count)
        {
          if (flag2)
            endLineIndex = this.paragraph.Lines.Count;
          else
            ++endLineIndex;
        }
        this.paragraph.RemoveLine(index1);
        --index1;
        --endLineIndex;
      }
      if (flag4)
        break;
    }
    if ((double) num2 > 0.0)
    {
      for (int index = endLineIndex; index < this.Lines.Count; ++index)
      {
        TextLine line4 = this.Lines[index];
        if (!this.paragraph.IsRotate)
        {
          if (!this.paragraph.IsVertWriteMode)
            line4.Offset(0.0f, num2);
          else
            line4.Offset(num2, 0.0f);
        }
      }
    }
    if (!line2.ReturnFlag | flag2)
    {
      AlignType align = this.paragraph.Align;
      int num3;
      switch (align)
      {
        case AlignType.AlignLeft:
        case AlignType.AlignRight:
          num3 = 1;
          break;
        default:
          num3 = align == AlignType.AlignAdjust ? 1 : 0;
          break;
      }
      if (num3 != 0)
      {
        if (!flag1)
          this.ExtractFollowingLinesToMakeAlign(line1, false, true);
        else if (!flag2)
          this.ExtractFollowingLinesToMakeAlign(line2, true, true);
      }
      else if (align == AlignType.AlignCenter && !flag2)
        this.ForceAlign(line2);
    }
    if (this.paragraph.Lines.Count == 0)
    {
      TextLine line5 = new TextLine(this.Page);
      FS_RECTF boundingBox3 = this.paragraph.GetBoundingBox(false);
      FS_RECTF boundingBox4 = new FS_RECTF(boundingBox3.left, boundingBox3.top, boundingBox3.right, boundingBox3.top - num1);
      line5.SetBox(boundingBox4);
      this.paragraph.AddLine(line5);
    }
    else
      this.paragraph.InvalidateBoundingBox(CalcBoundingBoxOrientation.Vertical);
    this.BuildCarets();
    return true;
  }

  private bool DeleteLines(TextLine startLine, TextLine endLine)
  {
    FS_RECTF boundingBox1 = startLine.GetBoundingBox(false);
    int index1 = this.paragraph.Lines.IndexOf<TextLine>(startLine);
    int num1 = this.paragraph.Lines.IndexOf<TextLine>(endLine);
    if (index1 == -1 || num1 == -1)
      return false;
    for (int index2 = num1; index2 >= index1; --index2)
      this.paragraph.RemoveLine(index2);
    if (index1 < this.paragraph.Lines.Count)
    {
      FS_RECTF boundingBox2 = this.paragraph.Lines[index1].GetBoundingBox(false);
      float num2 = 0.0f;
      if (!this.paragraph.IsRotate)
        num2 = this.paragraph.IsVertWriteMode ? boundingBox2.right - boundingBox1.right : boundingBox1.top - boundingBox2.top;
      for (int index3 = index1; index3 < this.paragraph.Lines.Count; ++index3)
      {
        if (!this.paragraph.IsRotate)
        {
          if (!this.paragraph.IsVertWriteMode)
            this.paragraph.Lines[index3].Offset(0.0f, num2);
          else
            this.paragraph.Lines[index3].Offset(num2, 0.0f);
        }
      }
    }
    this.BuildCarets();
    this.paragraph.InvalidateBoundingBox();
    return true;
  }

  private bool SetColorToText(
    int startCaret,
    int endCaret,
    FS_COLOR? strokeColor,
    FS_COLOR? fillColor,
    bool buildUndoItem,
    out IPdfUndoItem undoItem)
  {
    undoItem = (IPdfUndoItem) null;
    ParagraphCaret startCaret1;
    ParagraphCaret endCaret1;
    if (!this.VerifyCarets(startCaret, endCaret, out startCaret1, out endCaret1))
      return false;
    if (buildUndoItem)
    {
      ModifyParagraphUndoItem undoItem1 = new ModifyParagraphUndoItem();
      undoItem = (IPdfUndoItem) undoItem1;
      undoItem1.UndoType = UndoTypes.SetTextColor;
      undoItem1.StartLineIndex = this.paragraph.Lines.IndexOf<TextLine>(startCaret1.Line);
      undoItem1.EndLineIndex = this.paragraph.Lines.IndexOf<TextLine>(endCaret1.Line);
      undoItem1.Caret = startCaret;
      undoItem1.EndCaret = endCaret;
      undoItem1.FillStroke = StrokeFillFlags.None;
      if (fillColor.HasValue)
      {
        undoItem1.FillStroke |= StrokeFillFlags.Fill;
        undoItem1.TextColor = fillColor.Value;
      }
      if (strokeColor.HasValue)
      {
        undoItem1.FillStroke |= StrokeFillFlags.Stroke;
        undoItem1.StrokeColor = strokeColor.Value;
      }
      new PdfUndoSnapshot(this, startCaret, endCaret, undoItem1.StartLineIndex, undoItem1.EndLineIndex, true).FillUndoItem(undoItem1, false);
    }
    int startLineIndex;
    int startTextObjectIndex;
    int endLineIndex;
    int endTextObjectIndex;
    if (!this.GetSelectedObjectRange(ref startCaret1, ref endCaret1, out startLineIndex, out startTextObjectIndex, out endLineIndex, out endTextObjectIndex))
      return false;
    TextLine line1 = this.paragraph.Lines[startLineIndex];
    TextLine line2 = this.paragraph.Lines[endLineIndex];
    for (int index1 = startLineIndex; index1 <= endLineIndex; ++index1)
    {
      TextLine line3 = this.paragraph.Lines[index1];
      if (line3.TextObjects.Count > 0)
      {
        line3.GetBoundingBox(false);
        int index2 = 0;
        int num = line3.TextObjects.Count - 1;
        if (line3 == line1)
          index2 = startTextObjectIndex;
        if (line3 == line2)
          num = endTextObjectIndex;
        PdfTextObject textObject1 = line3.TextObjects[index2];
        if (line3 == line1)
        {
          this.tempCaretInfo.Line = line1;
          this.tempCaretInfo.TextObject = textObject1;
          this.tempCaretInfo.CharIndex = 0;
        }
        for (int index3 = index2; index3 <= num; ++index3)
        {
          PdfTextObject textObject2 = line3.TextObjects[index3];
          if (strokeColor.HasValue)
            textObject2.StrokeColor = strokeColor.Value;
          if (fillColor.HasValue)
            textObject2.FillColor = fillColor.Value;
        }
        if (line3 == line2)
        {
          PdfTextObject textObject3 = line3.TextObjects[num];
          int endCharIdx = textObject3.CharsCount - 1;
          this.StoreCaretInfo(line3, endLineIndex, textObject3, num, endCharIdx, true);
        }
      }
    }
    this.BuildCarets();
    this.UpdateCurCaret();
    return true;
  }

  private void StoreCaretInfo(
    TextLine endLine,
    int endLineIdx,
    PdfTextObject endObj,
    int endObjIdx,
    int endCharIdx,
    bool useNextObj)
  {
    this.tempEndCaretInfo.Line = endLine;
    this.tempEndCaretInfo.TextObject = endObj;
    this.tempEndCaretInfo.CharIndex = endCharIdx + 1;
    this.tempCaretType = endLine.IsLineTail(endObj, endCharIdx + 1) ? PdfParagraphImpl.TempCaretType.LineTail : PdfParagraphImpl.TempCaretType.None;
    if (this.tempCaretType != 0)
    {
      if (endLineIdx != -1)
      {
        ++endLineIdx;
        if (endLineIdx >= this.paragraph.Lines.Count)
          endLineIdx = -1;
      }
      if (endLineIdx != -1)
      {
        TextLine line1 = this.paragraph.Lines[endLineIdx];
        this.tempEndCaretInfo.Line = line1;
        if (line1.TextObjects.Count > 0)
        {
          this.tempEndCaretInfo.TextObject = line1.TextObjects[0];
        }
        else
        {
          bool flag = true;
          if (endLineIdx + 1 < this.paragraph.Lines.Count)
          {
            TextLine line2 = this.paragraph.Lines[endLineIdx + 1];
            if (line2.TextObjects.Count > 0)
            {
              this.tempEndCaretInfo.TextObject = line2.TextObjects[0];
              flag = false;
            }
          }
          if (flag)
            this.tempEndCaretInfo.TextObject = (PdfTextObject) null;
        }
        this.tempEndCaretInfo.CharIndex = 0;
      }
      else
        this.tempCaretType = PdfParagraphImpl.TempCaretType.ParaTail;
    }
    else
    {
      if (!useNextObj || endLine.TextObjects.Count <= 0)
        return;
      for (int index = 0; index < endLine.TextObjects.Count; ++index)
      {
        if (endLine.TextObjects[index] == endObj && index + 1 < endLine.TextObjects.Count)
        {
          this.tempEndCaretInfo.TextObject = endLine.TextObjects[index + 1];
          this.tempEndCaretInfo.CharIndex = 0;
          break;
        }
      }
    }
  }

  private bool ExtractFollowingLinesToMakeAlign(
    TextLine startLine,
    bool addSpaceAtTail,
    bool extractAlignLeft)
  {
    if (startLine == this.paragraph.Lines.LastOrDefault<TextLine>())
      return false;
    FS_RECTF boundingBox1 = startLine.GetBoundingBox(true);
    if (!this.paragraph.IsRotate && !this.paragraph.IsVertWriteMode)
    {
      FS_RECTF boundingBox2 = this.paragraph.GetBoundingBox(false);
      if ((double) boundingBox1.right >= (double) boundingBox2.right || (double) boundingBox1.right - (double) boundingBox2.right >= -2.0)
        return false;
      startLine.GetTailCharPos();
      int num1 = this.paragraph.Lines.IndexOf<TextLine>(startLine);
      if (num1 + 1 >= this.paragraph.Lines.Count)
      {
        if (startLine.TextObjects.Count == 0)
          this.DeleteLines(startLine, startLine);
        return true;
      }
      TextLine line = this.paragraph.Lines[num1 + 1];
      float num2 = PdfParagraphImpl.PreCalcLineTailRightX(startLine);
      float num3 = boundingBox2.right - num2;
      FS_RECTF boundingBox3 = line.GetBoundingBox(false);
      if ((double) boundingBox3.Width <= (double) num3)
      {
        if (num1 + 2 < this.paragraph.Lines.Count)
        {
          bool flag = this.endCaret > -1 && this.curCaret > -1 && this.endCaret != this.curCaret;
          if (((this.tempCaretInfo?.Line == null ? 0 : (this.tempEndCaretInfo?.Line == line ? 1 : 0)) & (flag ? 1 : 0)) != 0 && this.tempCaretType != 0)
          {
            this.tempEndCaretInfo.Line = num1 - 1 >= 0 ? this.paragraph.Lines[num1 - 1] : (TextLine) null;
            if (this.tempEndCaretInfo.Line != null && this.tempEndCaretInfo.Line.CharCount > 0)
            {
              this.tempEndCaretInfo.TextObject = this.tempEndCaretInfo.Line.TextObjects[this.tempEndCaretInfo.Line.TextObjects.Count - 1];
              this.tempEndCaretInfo.CharIndex = this.tempEndCaretInfo.TextObject.CharsCount + 1;
            }
          }
        }
        startLine.ExtractFrom(line, 0, line.TextObjects.Count - 1, addSpaceAtTail);
        if (line.ReturnFlag)
          startLine.ReturnFlag = true;
        if (num1 + 2 < this.paragraph.Lines.Count)
        {
          int index = num1 + 2;
          FS_RECTF boundingBox4 = this.paragraph.Lines[index].GetBoundingBox(false);
          float dy = boundingBox3.top - boundingBox4.top;
          for (; index < this.paragraph.Lines.Count; ++index)
            this.paragraph.Lines[index].Offset(0.0f, dy);
          this.paragraph.RemoveLine(num1 + 1);
          if (!startLine.ReturnFlag)
          {
            float width = this.paragraph.GetBoundingBox(true).Width;
            if ((double) startLine.GetBoundingBox(true).Width < (double) width - 4.0)
              this.ExtractFollowingLinesToMakeAlign(startLine, true, extractAlignLeft);
          }
        }
        else
          this.paragraph.RemoveLine(this.paragraph.Lines.Count - 1);
        return true;
      }
      int index1 = -1;
      System.Collections.Generic.IReadOnlyList<SplitToken> symbolAndSpacePos = line.GetSymbolAndSpacePos();
      for (int index2 = 0; index2 < symbolAndSpacePos.Count; ++index2)
      {
        SplitToken splitToken = symbolAndSpacePos[index2];
        int splitCharAt = splitToken.SplitCharAt;
        if (splitToken.TextObject.CharIsBlankSpace(splitCharAt))
          --splitCharAt;
        else if (splitToken.TextObject.CharIsPunctuation(splitCharAt))
        {
          ++splitCharAt;
          splitToken.SplitCharAt = splitCharAt;
        }
        float num4 = splitToken.TextObject.GetCharPos(splitCharAt)[2] - boundingBox1.left;
        if ((double) num4 >= (double) num3)
        {
          index1 = index2 - 1;
          if ((double) num4 - (double) num3 < 0.5)
          {
            index1 = index2;
            break;
          }
          break;
        }
        index1 = index2;
      }
      if (index1 == -1)
        return false;
      SplitToken splitToken1 = symbolAndSpacePos[index1];
      int endIndex = splitToken1.SplitCharAt == 0 ? splitToken1.ObjectIndex - 1 : splitToken1.ObjectIndex;
      if (splitToken1.SplitCharAt > 0 && splitToken1.SplitCharAt < splitToken1.TextObject.CharsCount && line.SplitAt(splitToken1.TextObject, splitToken1.SplitCharAt, true) != null && splitToken1.SplitCharAt == splitToken1.TextObject.CharsCount - 1)
        endIndex = splitToken1.ObjectIndex;
      bool returnFlag = startLine.ReturnFlag;
      if (endIndex >= 0)
      {
        startLine.ExtractFrom(line, 0, endIndex, addSpaceAtTail);
        this.ForceAlign(line);
        if (line.TextObjects.Count == 0)
        {
          if (line.ReturnFlag)
            startLine.ReturnFlag = true;
          if (num1 + 2 >= this.paragraph.Lines.Count)
            return true;
          this.paragraph.RemoveLine(line);
          line = this.paragraph.Lines[num1 + 1];
        }
        if ((double) startLine.GetBoundingBox(true).right - (double) boundingBox2.right > 2.0 && !line.ReturnFlag && !this.ExtractFollowingLinesToMakeAlign(line, true, extractAlignLeft))
          return false;
        this.ForceAlign(line);
      }
      bool flag1 = false;
      if (!returnFlag)
      {
        AlignType align = line.Align;
        if (align == AlignType.AlignAdjust || extractAlignLeft && align == AlignType.AlignLeft)
          flag1 = true;
      }
      if (flag1)
      {
        if (!line.ReturnFlag && !this.ExtractFollowingLinesToMakeAlign(line, true, extractAlignLeft))
          return false;
      }
      else
      {
        this.paragraph.InvalidateBoundingBox(CalcBoundingBoxOrientation.Vertical);
        this.BuildCarets();
        return true;
      }
    }
    return true;
  }

  private void ForceAlign(TextLine line)
  {
    if (line.TextObjects.Count == 0)
      return;
    float dx = 0.0f;
    AlignType align = this.paragraph.Align;
    if (this.paragraph.IsRotate || this.paragraph.IsVertWriteMode)
      return;
    FS_RECTF boundingBox1 = this.paragraph.GetBoundingBox(false);
    FS_RECTF boundingBox2 = line.GetBoundingBox(false);
    switch (align)
    {
      case AlignType.AlignLeft:
      case AlignType.AlignAdjust:
        float x = line.TextObjects[0].Location.X;
        dx = boundingBox1.left - x;
        break;
      case AlignType.AlignCenter:
        dx = boundingBox1.left + boundingBox1.Width / 2f - (boundingBox2.left + boundingBox2.Width / 2f);
        break;
      case AlignType.AlignRight:
        dx = boundingBox1.right - boundingBox2.right;
        break;
    }
    line.Offset(dx, 0.0f);
  }

  public bool GetSelectedObjectRange(
    ref ParagraphCaret startCaret,
    ref ParagraphCaret endCaret,
    out int startLineIndex,
    out int startTextObjectIndex,
    out int endLineIndex,
    out int endTextObjectIndex)
  {
    startLineIndex = -1;
    startTextObjectIndex = -1;
    endLineIndex = -1;
    endTextObjectIndex = -1;
    ParagraphCaret paragraphCaret1 = startCaret;
    ParagraphCaret paragraphCaret2 = endCaret;
    if (!PdfParagraphImpl.SkipSoftCaret((System.Collections.Generic.IReadOnlyList<ParagraphCaret>) this.caretList, ref startCaret, ref endCaret))
      return false;
    for (int index = 0; index < this.paragraph.Lines.Count; ++index)
    {
      if (startLineIndex == -1 && this.paragraph.Lines[index] == startCaret.Line)
      {
        startLineIndex = index;
        if (startCaret.TextObject != null)
          startTextObjectIndex = this.paragraph.Lines[index].TextObjects.IndexOf<PdfTextObject>(startCaret.TextObject);
      }
      if (endLineIndex == -1 && this.paragraph.Lines[index] == endCaret.Line)
      {
        endLineIndex = index;
        if (endCaret.TextObject != null)
          endTextObjectIndex = this.paragraph.Lines[index].TextObjects.IndexOf<PdfTextObject>(endCaret.TextObject);
      }
      if (startLineIndex >= 0 && endLineIndex >= 0)
        break;
    }
    PdfTextObject textObj1 = endCaret.TextObject.SplitAt(endCaret.CharIndex);
    PdfTextObject textObject = endCaret.TextObject;
    if (textObj1 != null)
      endCaret.Line.InsertText(endTextObjectIndex + 1, textObj1);
    else if (endCaret.CharIndex == 0 && endTextObjectIndex > 0)
      --endTextObjectIndex;
    PdfTextObject textObj2 = startCaret.TextObject.SplitAt(startCaret.CharIndex);
    PdfTextObject pdfTextObject = textObj2 == null ? startCaret.TextObject : textObj2;
    if (textObj2 != null)
    {
      if (startCaret.TextObject == textObject)
        endTextObjectIndex = startTextObjectIndex + 1;
      else if (startCaret.Line == endCaret.Line)
        ++endTextObjectIndex;
      ++startTextObjectIndex;
      startCaret.Line.InsertText(startTextObjectIndex, textObj2);
    }
    return true;
  }

  internal void BuildCarets()
  {
    this.caretList = new List<ParagraphCaret>();
    if (this.IsEmpty())
    {
      FS_RECTF boundingBox = this.paragraph.GetBoundingBox(false);
      this.caretList.Add(new ParagraphCaret()
      {
        CharIndex = -1,
        TextObject = (PdfTextObject) null,
        TopPoint = new FS_POINTF(boundingBox.left, boundingBox.top),
        BottomPoint = new FS_POINTF(boundingBox.left, boundingBox.bottom),
        Line = this.paragraph.Lines[0]
      });
    }
    AlignType align = this.paragraph.Align;
    FS_RECTF boundingBox1 = this.paragraph.GetBoundingBox(false);
    foreach (TextLine line in (IEnumerable<TextLine>) this.paragraph.Lines)
    {
      if (line.TextObjects.Count == 0)
      {
        this.caretList.Add(PdfParagraphImpl.CreateEmptyLineCaret(line, align));
      }
      else
      {
        ParagraphCaret prev = (ParagraphCaret) null;
        for (int index1 = 0; index1 < line.TextObjects.Count; ++index1)
        {
          PdfTextObject textObject = line.TextObjects[index1];
          if (index1 == 0 && (align == AlignType.AlignCenter || align == AlignType.AlignRight))
          {
            FS_RECTF box = textObject.GetBox();
            if ((!this.paragraph.IsVertWriteMode ? (double) (box.top - boundingBox1.top) : (double) (box.left - boundingBox1.left)) > (double) textObject.GetSpaceCharWidth() / 2.0)
            {
              FS_RECTF boundingBox2 = line.GetBoundingBox(false);
              ParagraphCaret[] carets;
              if (PdfParagraphImpl.BuildSoftSpaceCarets(new ParagraphCaret()
              {
                TopPoint = new FS_POINTF(boundingBox2.left, boundingBox2.top),
                BottomPoint = new FS_POINTF(boundingBox2.left, boundingBox2.bottom),
                CharIndex = -1,
                TextObject = (PdfTextObject) null
              }, new ParagraphCaret()
              {
                TopPoint = new FS_POINTF(box.left, box.top),
                BottomPoint = new FS_POINTF(box.left, box.bottom),
                CharIndex = -1,
                TextObject = textObject
              }, out carets))
              {
                for (int index2 = 0; index2 < carets.Length; ++index2)
                {
                  ParagraphCaret caret = carets[index2];
                  caret.Line = line;
                  PdfParagraphImpl.CutCaretToFitLineHeight(caret, line);
                  this.caretList.Add(caret);
                }
              }
            }
          }
          List<ParagraphCaret> source = PdfParagraphImpl.BuildObjCarets(textObject, this.paragraph.IsRotate);
          if (source != null)
          {
            ParagraphCaret[] carets;
            if (prev != null && PdfParagraphImpl.BuildSoftSpaceCarets(prev, source[0], out carets))
            {
              for (int index3 = 0; index3 < carets.Length; ++index3)
              {
                ParagraphCaret caret = carets[index3];
                caret.Line = line;
                PdfParagraphImpl.CutCaretToFitLineHeight(caret, line);
                this.caretList.Add(caret);
                if (caret.Line == null)
                  throw new ArgumentException("Line");
              }
              if (carets.Length == 0)
              {
                ParagraphCaret paragraphCaret = prev.Copy();
                this.caretList.Add(paragraphCaret);
                if (paragraphCaret.Line == null)
                  throw new ArgumentException("Line");
              }
            }
            ParagraphCaret paragraphCaret1 = source.LastOrDefault<ParagraphCaret>();
            paragraphCaret1.Line = line;
            prev = paragraphCaret1.Copy();
            int num = index1 == line.TextObjects.Count - 1 ? source.Count : source.Count - 1;
            for (int index4 = 0; index4 < num; ++index4)
            {
              ParagraphCaret caret = source[index4];
              caret.Line = line;
              PdfParagraphImpl.CutCaretToFitLineHeight(caret, line);
              this.caretList.Add(caret);
              if (caret.Line == null)
                throw new ArgumentException("Line");
            }
          }
        }
      }
    }
  }

  private bool VerifyCarets(
    int startCaretPos,
    int endCaretPos,
    out ParagraphCaret startCaret,
    out ParagraphCaret endCaret)
  {
    int index1 = Math.Min(startCaretPos, endCaretPos);
    int index2 = Math.Max(startCaretPos, endCaretPos);
    startCaret = this.GetCaretByIndex(index1);
    endCaret = this.GetCaretByIndex(index2);
    if (startCaret == null || endCaret == null)
    {
      startCaret = (ParagraphCaret) null;
      endCaret = (ParagraphCaret) null;
      return false;
    }
    this.curCaret = index1;
    this.endCaret = index2;
    return true;
  }

  private bool IsEmpty()
  {
    return this.paragraph.Lines.Count == 1 && this.paragraph.Lines[0].TextObjects.Count == 0;
  }

  private int FitCaretIndex(int caret)
  {
    return caret == 0 || this.IsLineHeaderCaret(caret) ? caret : caret - 1;
  }

  private ParagraphCaret GetCaretByIndex(int index)
  {
    return this.caretList == null || index < 0 || index >= this.caretList.Count ? (ParagraphCaret) null : this.caretList[index];
  }

  protected virtual void Dispose(bool disposing)
  {
    if (this.disposedValue)
      return;
    if (!disposing)
      ;
    this.disposedValue = true;
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  private static ParagraphCaret CreateEmptyLineCaret(TextLine line, AlignType paraAlign)
  {
    ParagraphCaret emptyLineCaret = new ParagraphCaret()
    {
      CharIndex = -1,
      TextObject = (PdfTextObject) null,
      Line = line
    };
    FS_RECTF boundingBox = line.GetBoundingBox(false);
    FS_POINTF fsPointf1 = new FS_POINTF();
    FS_POINTF fsPointf2 = new FS_POINTF();
    if (!line.IsRotate)
    {
      if (!line.IsVertWriteMode)
      {
        switch (paraAlign)
        {
          case AlignType.AlignCenter:
            fsPointf1.X = fsPointf2.X = boundingBox.left + (float) (((double) boundingBox.right - (double) boundingBox.left) / 2.0);
            break;
          case AlignType.AlignRight:
            fsPointf1.X = fsPointf2.X = boundingBox.right;
            break;
          default:
            fsPointf1.X = fsPointf2.X = boundingBox.left;
            break;
        }
        fsPointf1.Y = boundingBox.top;
        fsPointf2.Y = boundingBox.bottom;
      }
      else
      {
        fsPointf1.X = boundingBox.left;
        fsPointf1.Y = boundingBox.bottom;
        fsPointf2.X = boundingBox.left;
        fsPointf2.Y = boundingBox.bottom;
      }
    }
    else
    {
      switch (paraAlign)
      {
        case AlignType.AlignCenter:
          fsPointf1.X = fsPointf2.X = boundingBox.left + (float) (((double) boundingBox.right - (double) boundingBox.left) / 2.0);
          break;
        case AlignType.AlignRight:
          fsPointf1.X = fsPointf2.X = boundingBox.right;
          break;
        default:
          fsPointf1.X = fsPointf2.X = boundingBox.left;
          break;
      }
      fsPointf1.Y = boundingBox.top;
      fsPointf2.Y = boundingBox.bottom;
    }
    emptyLineCaret.TopPoint = fsPointf1;
    emptyLineCaret.BottomPoint = fsPointf2;
    return emptyLineCaret;
  }

  private static bool BuildSoftSpaceCarets(
    ParagraphCaret prev,
    ParagraphCaret next,
    out ParagraphCaret[] carets)
  {
    carets = (ParagraphCaret[]) null;
    if (!next.Rotate)
    {
      FS_POINTF fsPointf1 = prev.TopPoint;
      double y1 = (double) fsPointf1.Y;
      fsPointf1 = prev.BottomPoint;
      double y2 = (double) fsPointf1.Y;
      float num1 = (float) (y1 - y2);
      FS_POINTF fsPointf2 = next.TopPoint;
      double y3 = (double) fsPointf2.Y;
      fsPointf2 = next.BottomPoint;
      double y4 = (double) fsPointf2.Y;
      float num2 = (float) (y3 - y4);
      ParagraphCaret paragraphCaret1 = (double) num1 >= (double) num2 ? prev : next;
      bool flag = false;
      if ((double) num1 >= (double) num2 && paragraphCaret1.TextObject == null)
      {
        paragraphCaret1 = next;
        flag = true;
      }
      float spaceCharWidth = paragraphCaret1.TextObject.GetSpaceCharWidth();
      if ((double) spaceCharWidth == 0.0)
        return false;
      List<ParagraphCaret> paragraphCaretList = new List<ParagraphCaret>();
      double x1 = (double) next.BottomPoint.X;
      FS_POINTF fsPointf3 = prev.BottomPoint;
      double x2 = (double) fsPointf3.X;
      float num3 = (float) (x1 - x2);
      if ((double) num3 >= (double) spaceCharWidth)
      {
        int num4 = (int) num3;
        if (num4 == 0 || (double) spaceCharWidth == 0.0)
          return false;
        int num5 = (int) ((double) num4 / (double) spaceCharWidth);
        for (int index = 0; index < num5 - 1; ++index)
        {
          ParagraphCaret paragraphCaret2 = new ParagraphCaret();
          paragraphCaret2.CharIndex = -1;
          paragraphCaret2.TextObject = (PdfTextObject) null;
          fsPointf3 = paragraphCaret1.TopPoint;
          double x3 = (double) fsPointf3.X + (double) spaceCharWidth * (double) index;
          fsPointf3 = paragraphCaret1.TopPoint;
          double y5 = (double) fsPointf3.Y;
          paragraphCaret2.TopPoint = new FS_POINTF((float) x3, (float) y5);
          fsPointf3 = paragraphCaret1.BottomPoint;
          double x4 = (double) fsPointf3.X + (double) spaceCharWidth * (double) index;
          fsPointf3 = paragraphCaret1.BottomPoint;
          double y6 = (double) fsPointf3.Y;
          paragraphCaret2.BottomPoint = new FS_POINTF((float) x4, (float) y6);
          paragraphCaret2.Line = paragraphCaret1.Line;
          ParagraphCaret paragraphCaret3 = paragraphCaret2;
          paragraphCaretList.Add(paragraphCaret3);
        }
        if (flag)
        {
          ParagraphCaret paragraphCaret4 = prev.Copy();
          paragraphCaretList.Insert(0, paragraphCaret4);
        }
        carets = paragraphCaretList.ToArray();
      }
      else if ((double) num3 < (double) spaceCharWidth && (double) num3 >= (double) spaceCharWidth / 2.0)
      {
        carets = Array.Empty<ParagraphCaret>();
        return true;
      }
    }
    return carets != null && carets.Length != 0;
  }

  private static void CutCaretToFitLineHeight(ParagraphCaret caret, TextLine line)
  {
    if (line.IsRotate)
      return;
    FS_RECTF boundingBox = line.GetBoundingBox(false);
    if (!line.IsVertWriteMode)
    {
      ParagraphCaret paragraphCaret1 = caret;
      FS_POINTF fsPointf1 = caret.TopPoint;
      FS_POINTF fsPointf2 = new FS_POINTF(fsPointf1.X, boundingBox.top);
      paragraphCaret1.TopPoint = fsPointf2;
      ParagraphCaret paragraphCaret2 = caret;
      fsPointf1 = caret.BottomPoint;
      FS_POINTF fsPointf3 = new FS_POINTF(fsPointf1.X, boundingBox.bottom);
      paragraphCaret2.BottomPoint = fsPointf3;
    }
    else
      caret.TopPoint = new FS_POINTF(boundingBox.right, caret.TopPoint.Y);
  }

  private static List<ParagraphCaret> BuildObjCarets(PdfTextObject obj, bool isParaRotated)
  {
    if (obj.CharsCount == 0)
      return (List<ParagraphCaret>) null;
    List<ParagraphCaret> paragraphCaretList = new List<ParagraphCaret>();
    Matrix matrix = obj.MatrixFromPage2();
    PdfFont font = obj.Font;
    float fontSize = obj.FontSize;
    int ascent = font.Ascent;
    int descent = font.Descent;
    float[] pPosArray = new float[obj.CharsCount * 2];
    Pdfium.FPDFTextObj_CalcCharPos(obj.Handle, pPosArray);
    for (int index = 0; index <= obj.CharsCount; ++index)
    {
      float num1 = index != obj.CharsCount ? pPosArray[index * 2] : pPosArray[(index - 1) * 2 + 1];
      float x1 = num1;
      float y1 = (float) ascent * fontSize / (float) (ascent - descent);
      float x2 = num1;
      float y2 = (float) descent * fontSize / (float) (ascent - descent);
      Point point1 = matrix.Transform(new Point((double) x1, (double) y1));
      Point point2 = matrix.Transform(new Point((double) x2, (double) y2));
      ParagraphCaret paragraphCaret1 = new ParagraphCaret()
      {
        CharIndex = index,
        TextObject = obj,
        TopPoint = point1.ToPdfPoint(),
        BottomPoint = point2.ToPdfPoint()
      };
      paragraphCaretList.Add(paragraphCaret1);
      FS_POINTF fsPointf1;
      if (!isParaRotated)
      {
        if (index < obj.CharsCount)
        {
          fsPointf1 = paragraphCaret1.TopPoint;
          double x3 = (double) fsPointf1.X;
          fsPointf1 = paragraphCaret1.BottomPoint;
          double x4 = (double) fsPointf1.X;
          float num2 = Math.Min((float) x3, (float) x4);
          ParagraphCaret paragraphCaret2 = paragraphCaret1;
          double x5 = (double) num2;
          fsPointf1 = paragraphCaret1.TopPoint;
          double y3 = (double) fsPointf1.Y;
          FS_POINTF fsPointf2 = new FS_POINTF((float) x5, (float) y3);
          paragraphCaret2.TopPoint = fsPointf2;
          ParagraphCaret paragraphCaret3 = paragraphCaret1;
          double x6 = (double) num2;
          fsPointf1 = paragraphCaret1.BottomPoint;
          double y4 = (double) fsPointf1.Y;
          FS_POINTF fsPointf3 = new FS_POINTF((float) x6, (float) y4);
          paragraphCaret3.BottomPoint = fsPointf3;
        }
        else if (index == obj.CharsCount)
        {
          fsPointf1 = paragraphCaret1.TopPoint;
          double x7 = (double) fsPointf1.X;
          fsPointf1 = paragraphCaret1.BottomPoint;
          double x8 = (double) fsPointf1.X;
          float num3 = Math.Max((float) x7, (float) x8);
          ParagraphCaret paragraphCaret4 = paragraphCaret1;
          double x9 = (double) num3;
          fsPointf1 = paragraphCaret1.TopPoint;
          double y5 = (double) fsPointf1.Y;
          FS_POINTF fsPointf4 = new FS_POINTF((float) x9, (float) y5);
          paragraphCaret4.TopPoint = fsPointf4;
          ParagraphCaret paragraphCaret5 = paragraphCaret1;
          double x10 = (double) num3;
          fsPointf1 = paragraphCaret1.BottomPoint;
          double y6 = (double) fsPointf1.Y;
          FS_POINTF fsPointf5 = new FS_POINTF((float) x10, (float) y6);
          paragraphCaret5.BottomPoint = fsPointf5;
        }
      }
    }
    return paragraphCaretList;
  }

  private static float PreCalcLineTailRightX(TextLine line)
  {
    if (line.TextObjects.Count <= 0)
      return 0.0f;
    PdfTextObject textObject = line.TextObjects[line.TextObjects.Count - 1];
    textObject.GetUnicodeChar(textObject.CharsCount - 1);
    if (textObject.CharIsBlankSpace(textObject.CharsCount - 1))
      return line.GetTailCharPos().X;
    bool flag = textObject.AppendSpace();
    FS_POINTF tailCharPos = line.GetTailCharPos();
    if (flag)
      textObject.DeleteText(textObject.CharsCount, textObject.CharsCount, out bool _);
    return tailCharPos.X;
  }

  private static bool SkipSoftCaret(
    System.Collections.Generic.IReadOnlyList<ParagraphCaret> list,
    ref ParagraphCaret startCaret,
    ref ParagraphCaret endCaret)
  {
    if (startCaret.TextObject == null)
    {
      int num = list.IndexOf<ParagraphCaret>(startCaret);
      if (num == -1)
        return false;
      for (int index = num; index < list.Count; ++index)
      {
        ParagraphCaret paragraphCaret = list[index];
        if (paragraphCaret == endCaret)
          return false;
        if (paragraphCaret.TextObject != null)
        {
          startCaret = paragraphCaret;
          break;
        }
      }
    }
    if (endCaret.TextObject == null)
    {
      int num = list.IndexOf<ParagraphCaret>(endCaret);
      if (num == -1)
        return false;
      for (int index = num; index >= 0; --index)
      {
        ParagraphCaret paragraphCaret = list[index];
        if (paragraphCaret == startCaret)
          return false;
        if (paragraphCaret.TextObject != null)
        {
          endCaret = new ParagraphCaret()
          {
            CharIndex = Math.Max(paragraphCaret.CharIndex + 1, paragraphCaret.TextObject.CharsCount),
            Line = paragraphCaret.Line,
            Rotate = paragraphCaret.Rotate,
            TextObject = paragraphCaret.TextObject,
            TopPoint = paragraphCaret.TopPoint,
            BottomPoint = paragraphCaret.BottomPoint
          };
          break;
        }
      }
    }
    return true;
  }

  private enum TempCaretType : short
  {
    ParaTail = -1, // 0xFFFF
    None = 0,
    LineTail = 1,
  }
}
