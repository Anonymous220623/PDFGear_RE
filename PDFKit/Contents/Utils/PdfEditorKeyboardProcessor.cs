// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Utils.PdfEditorKeyboardProcessor
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using PDFKit.Contents.Controls;
using PDFKit.Contents.Operations;
using System;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace PDFKit.Contents.Utils;

internal class PdfEditorKeyboardProcessor
{
  private readonly PdfEditor editor;

  internal PdfEditorKeyboardProcessor(PdfEditor editor) => this.editor = editor;

  public bool OnKeyDown(Key key)
  {
    bool flag1 = (Keyboard.Modifiers & ModifierKeys.Control) != 0;
    bool flag2 = (Keyboard.Modifiers & ModifierKeys.Alt) != 0;
    bool flag3 = (Keyboard.Modifiers & ModifierKeys.Shift) != 0;
    if (this.editor.MouseMode == EditorMouseModes.Default)
    {
      switch (key)
      {
        case Key.Back:
          return this.OnBackKey();
        case Key.Return:
          return this.OnEnterKey();
        case Key.Escape:
          return this.OnEscapeKey();
        case Key.End:
        case Key.Home:
          return this.OnHomeEndKey(key);
        case Key.Left:
        case Key.Up:
        case Key.Right:
        case Key.Down:
          return this.OnDirectionKey(key);
        case Key.Delete:
          return this.OnDeleteKey();
        case Key.A:
          if (flag1)
          {
            this.SelectAll();
            return true;
          }
          break;
        case Key.C:
          if (flag1)
          {
            this.CopyText();
            return true;
          }
          break;
        case Key.V:
          if (flag1)
          {
            this.PasteText();
            return true;
          }
          break;
        case Key.X:
          if (flag1)
          {
            this.CutText();
            return true;
          }
          break;
        case Key.Y:
          if (flag1)
            return false;
          break;
        case Key.Z:
          if (flag1)
            return false;
          break;
      }
      char ch;
      if (KeyboardHelper.TryGetCharFromKey(key, out ch))
        return this.InsertText($"{ch}");
    }
    else
    {
      switch (key)
      {
        case Key.Y:
          if (flag1)
          {
            this.Redo();
            return true;
          }
          break;
        case Key.Z:
          if (flag1)
          {
            if (flag3)
              this.Redo();
            else
              this.Undo();
            return true;
          }
          break;
      }
    }
    return false;
  }

  private bool OnEscapeKey()
  {
    this.editor.caretInfo.Caret = -1;
    this.editor.caretInfo.EndCaret = -1;
    this.editor.UpdateCurrentParagraphCarets();
    this.editor.caretInfo.RaiseCaretChanged();
    this.editor.MouseMode = EditorMouseModes.SelectParagraph;
    return true;
  }

  private bool OnDeleteKey()
  {
    IPdfParagraph caretParagraph = this.editor.GetCaretParagraph();
    if (caretParagraph != null)
    {
      if (this.editor.caretInfo.EndCaret == -1 || this.editor.caretInfo.Caret == this.editor.caretInfo.EndCaret)
      {
        IPdfUndoItem undoItem;
        if (caretParagraph.DeleteText(this.editor.caretInfo.Caret, this.editor.caretInfo.Caret, true, out undoItem))
        {
          this.editor.operationManager.AddUndoItem(undoItem);
          this.editor.UpdateCaretInfo();
          this.Redraw(true);
          return true;
        }
      }
      else
      {
        int start = Math.Min(this.editor.caretInfo.Caret, this.editor.caretInfo.EndCaret);
        int end = Math.Max(this.editor.caretInfo.Caret, this.editor.caretInfo.EndCaret);
        IPdfUndoItem undoItem;
        if (caretParagraph.DeleteText(start, end, true, out undoItem))
        {
          this.editor.operationManager.AddUndoItem(undoItem);
          this.editor.UpdateCaretInfo();
          this.Redraw(true);
          return true;
        }
      }
    }
    return false;
  }

  private bool OnBackKey()
  {
    IPdfParagraph caretParagraph = this.editor.GetCaretParagraph();
    if (caretParagraph != null)
    {
      if (this.editor.caretInfo.EndCaret == -1 || this.editor.caretInfo.EndCaret == this.editor.caretInfo.Caret)
      {
        IPdfUndoItem undoItem;
        if (caretParagraph.BackspaceAt(this.editor.caretInfo.Caret, true, out undoItem))
        {
          this.editor.operationManager.AddUndoItem(undoItem);
          this.editor.UpdateCaretInfo();
          this.Redraw(true);
          return true;
        }
      }
      else
      {
        int start = Math.Min(this.editor.caretInfo.Caret, this.editor.caretInfo.EndCaret);
        int end = Math.Max(this.editor.caretInfo.Caret, this.editor.caretInfo.EndCaret);
        IPdfUndoItem undoItem;
        if (caretParagraph.DeleteText(start, end, true, out undoItem))
        {
          this.editor.operationManager.AddUndoItem(undoItem);
          this.editor.UpdateCaretInfo();
          this.Redraw(true);
          return true;
        }
      }
    }
    return false;
  }

  private bool OnEnterKey()
  {
    IPdfParagraph caretParagraph = this.editor.GetCaretParagraph();
    if (caretParagraph != null)
    {
      IPdfUndoItem undoItem;
      if (caretParagraph.InsertReturn(this.editor.caretInfo.Caret, true, out undoItem))
      {
        this.editor.operationManager.AddUndoItem(undoItem);
        this.editor.UpdateCaretInfo();
        this.Redraw(true);
        return true;
      }
      this.editor.UpdateCaretInfo();
      this.Redraw(false);
    }
    return false;
  }

  private bool OnDirectionKey(Key direction)
  {
    IPdfParagraph caretParagraph = this.editor.GetCaretParagraph();
    if (caretParagraph == null)
      return false;
    ParagraphCaretDirect direct = ParagraphCaretDirect.Right;
    switch (direction)
    {
      case Key.Left:
        direct = ParagraphCaretDirect.Left;
        break;
      case Key.Up:
        direct = ParagraphCaretDirect.Up;
        break;
      case Key.Right:
        direct = ParagraphCaretDirect.Right;
        break;
      case Key.Down:
        direct = ParagraphCaretDirect.Down;
        break;
    }
    int nextCaret = caretParagraph.GetNextCaret(this.editor.caretInfo.Caret, direct);
    bool flag = nextCaret != this.editor.caretInfo.Caret;
    this.editor.caretInfo.Caret = nextCaret;
    this.editor.caretInfo.EndCaret = -1;
    this.editor.caretInfo.RaiseCaretChanged();
    this.editor.UpdateCurrentParagraphCarets();
    this.Redraw(false);
    return flag;
  }

  private bool OnHomeEndKey(Key key)
  {
    IPdfParagraph caretParagraph = this.editor.GetCaretParagraph();
    int caret = this.editor.caretInfo.Caret;
    if (caret != -1 && caretParagraph != null && (key == Key.Home || key == Key.End))
    {
      switch (key)
      {
        case Key.End:
          caret = caretParagraph.GetLineTailCaret(caret);
          break;
        case Key.Home:
          caret = caretParagraph.GetLineHeaderCaret(caret);
          break;
      }
      if (caret != -1)
      {
        this.editor.caretInfo.Caret = caret;
        this.editor.caretInfo.EndCaret = -1;
        this.editor.caretInfo.RaiseCaretChanged();
        this.editor.UpdateCurrentParagraphCarets();
        return true;
      }
    }
    return false;
  }

  private void CopyText()
  {
    int start;
    int end;
    if (!this.editor.caretInfo.TryGetCaretRange(out start, out end))
      return;
    IPdfParagraph caretParagraph = this.editor.GetCaretParagraph();
    if (caretParagraph != null)
    {
      string text = caretParagraph.GetText(start, end);
      if (!string.IsNullOrEmpty(text))
        Clipboard.SetDataObject((object) text);
    }
  }

  private void CutText()
  {
    this.CopyText();
    this.OnDeleteKey();
  }

  private bool PasteText()
  {
    string text = Clipboard.GetText(TextDataFormat.UnicodeText);
    return !string.IsNullOrEmpty(text) && this.InsertText(text);
  }

  internal bool InsertText(string text)
  {
    if (string.IsNullOrEmpty(text))
    {
      this.Redraw(false);
      return false;
    }
    IPdfParagraph caretParagraph = this.editor.GetCaretParagraph();
    if (caretParagraph != null)
    {
      IPdfUndoItem undoItem;
      if (caretParagraph.InsertText(this.editor.caretInfo.Caret, text, true, out undoItem))
      {
        this.editor.operationManager.AddUndoItem(undoItem);
        this.editor.UpdateCaretInfo();
        this.Redraw(true);
        return true;
      }
      this.editor.UpdateCaretInfo();
      this.Redraw(false);
    }
    return false;
  }

  private void SelectAll()
  {
    IPdfParagraph caretParagraph = this.editor.GetCaretParagraph();
    if (caretParagraph == null)
      return;
    int num = caretParagraph.Carets.Count - 1;
    if (num < 0)
      return;
    this.editor.caretInfo.Caret = 0;
    this.editor.caretInfo.EndCaret = num;
    this.editor.caretInfo.RaiseCaretChanged();
    this.editor.UpdateCurrentParagraphCarets();
  }

  private void Redo()
  {
    this.editor.operationManager.Redo();
    this.editor.UpdateCaretInfo();
    this.Redraw(true);
  }

  private void Undo()
  {
    this.editor.operationManager.Undo();
    this.editor.UpdateCaretInfo();
    this.Redraw(true);
  }

  public void SetCaret(int caret)
  {
    this.SetCaretCore(caret, -1);
    this.Redraw(false);
  }

  public void SetCaret(int startCaret, int endCaret)
  {
    this.SetCaret(startCaret, endCaret);
    this.Redraw(false);
  }

  protected void SetCaretCore(int startCaret, int endCaret)
  {
    IPdfParagraph caretParagraph = this.editor.GetCaretParagraph();
    if (caretParagraph == null)
      return;
    caretParagraph.SetCurrentCarets(startCaret, endCaret);
    this.editor.UpdateCaretInfo();
  }

  protected void Redraw(bool force)
  {
    if (force)
      this.editor.ForceRender();
    else
      this.editor.InvalidateVisual();
  }
}
