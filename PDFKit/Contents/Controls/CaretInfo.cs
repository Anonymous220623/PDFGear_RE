// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Controls.CaretInfo
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using PDFKit.Utils;
using System;
using System.Windows;

#nullable disable
namespace PDFKit.Contents.Controls;

internal class CaretInfo
{
  private const float DefaultCaretWidth = 1.33333337f;
  private const float DefaultCaretInflate = 2f;
  private readonly PdfEditor editor;

  internal CaretInfo(PdfEditor editor) => this.editor = editor;

  public int PageIndex { get; set; } = -1;

  public int ParagraphIndex { get; set; } = -1;

  public int Caret { get; set; } = 0;

  public int EndCaret { get; set; } = -1;

  internal void RaiseCaretChanged()
  {
    EventHandler caretChanged = this.CaretChanged;
    if (caretChanged == null)
      return;
    caretChanged((object) this, EventArgs.Empty);
  }

  internal event EventHandler CaretChanged;

  internal bool TryGetCaretRange(out int start, out int end)
  {
    start = -1;
    end = -1;
    if (this.Caret == -1 || this.EndCaret == -1 || this.Caret == this.EndCaret)
      return false;
    int caret = this.Caret;
    int val2 = this.EndCaret;
    if (val2 == -1)
      val2 = this.Caret;
    start = Math.Min(caret, val2);
    end = Math.Max(caret, val2);
    return true;
  }

  internal Rect GetCaretRect(Rect renderRect)
  {
    if (this.Caret == -1 || this.editor == null || renderRect.IsEmpty || renderRect.Width == 0.0 || renderRect.Height == 0.0)
      return Rect.Empty;
    LogicalStructAnalyser pageStructAnalyser = this.editor.GetPageStructAnalyser(this.PageIndex);
    if (pageStructAnalyser == null)
      return Rect.Empty;
    IPdfParagraph paragraph = pageStructAnalyser.GetParagraph(this.ParagraphIndex);
    FS_POINTF top;
    FS_POINTF bottom;
    Rect clientRect;
    if (paragraph == null || !paragraph.GetCaretPos(this.Caret, out top, out bottom) || !this.editor.TryGetClientRect(this.PageIndex, new FS_RECTF(top.X, top.Y, top.X, bottom.Y), out clientRect))
      return Rect.Empty;
    switch (this.editor.Document.Pages[this.PageIndex].Rotation)
    {
      case PageRotate.Normal:
        clientRect.Width = 1.3333333730697632;
        clientRect.Y -= 2.0;
        clientRect.Height += 2.0;
        break;
      case PageRotate.Rotate90:
        clientRect.Height = 1.3333333730697632;
        clientRect.X -= 2.0;
        clientRect.Width += 2.0;
        break;
      case PageRotate.Rotate180:
        clientRect.X -= 1.3333333730697632;
        clientRect.Width = 1.3333333730697632;
        clientRect.Y -= 2.0;
        clientRect.Height += 2.0;
        break;
      case PageRotate.Rotate270:
        clientRect.Y -= 1.3333333730697632;
        clientRect.Height = 1.3333333730697632;
        clientRect.X -= 2.0;
        clientRect.Width += 2.0;
        break;
    }
    return clientRect;
  }
}
