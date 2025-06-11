// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRichTextStrings.TextPointerLineInfo
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System.Windows.Controls;
using System.Windows.Documents;

#nullable disable
namespace PDFKit.Utils.PdfRichTextStrings;

internal struct TextPointerLineInfo(
  int lineIndex,
  TextPointer lineStart,
  TextPointer lineEnd,
  bool isEndLine)
{
  public int LineIndex { get; } = lineIndex;

  public bool IsEndLine { get; } = isEndLine;

  public TextPointer LineStart { get; } = lineStart;

  public TextPointer LineEnd { get; } = lineEnd;

  public int Length => this.LineStart.GetOffsetToPosition(this.LineEnd);

  public TextPointerLineInfo? GetNextLineInfo(TextPointer documentEndPointer)
  {
    if (this.IsEndLine)
      return new TextPointerLineInfo?();
    TextPointer lineEnd = this.LineEnd;
    TextPointer lineStartPosition = lineEnd.GetLineStartPosition(1);
    return new TextPointerLineInfo?(new TextPointerLineInfo(this.LineIndex + 1, lineEnd, lineStartPosition ?? documentEndPointer, lineStartPosition == null));
  }

  public static TextPointerLineInfo? CreateFirstLine(RichTextBox rtb)
  {
    TextPointer lineStart = rtb.Document.ContentStart;
    if (!lineStart.IsAtLineStartPosition)
      lineStart = lineStart.GetLineStartPosition(0);
    if (lineStart == null)
      return new TextPointerLineInfo?();
    TextPointer lineStartPosition = lineStart.GetLineStartPosition(1);
    bool isEndLine = lineStartPosition == null;
    return new TextPointerLineInfo?(new TextPointerLineInfo(0, lineStart, lineStartPosition ?? rtb.Document.ContentEnd, isEndLine));
  }
}
