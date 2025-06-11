// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Utils.TextProperties
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net;
using PDFKit.Contents.Controls;
using PDFKit.Contents.Operations;
using System;
using System.Globalization;
using System.Windows.Threading;

#nullable disable
namespace PDFKit.Contents.Utils;

public class TextProperties
{
  private readonly PdfEditor editor;
  private DispatcherTimer raiseEventTimer;
  private bool propertyChanging;

  internal TextProperties(PdfEditor pdfEditor)
  {
    this.editor = pdfEditor;
    this.editor.caretInfo.CaretChanged += new EventHandler(this.CaretInfo_CaretChanged);
    this.CultureInfo = CultureInfo.CurrentUICulture;
    this.raiseEventTimer = new DispatcherTimer(DispatcherPriority.Normal)
    {
      Interval = TimeSpan.FromMilliseconds(50.0)
    };
    this.raiseEventTimer.Tick += (EventHandler) ((s, a) => this.RaisePropertyChangedInternal());
  }

  public CultureInfo CultureInfo { get; set; }

  public float FontSize
  {
    get
    {
      IPdfParagraph caretParagraph = this.editor.GetCaretParagraph();
      return caretParagraph == null ? 0.0f : caretParagraph.GetFontSize(this.GetActualCaret());
    }
    set
    {
      int start;
      int end;
      if (this.propertyChanging || this.editor.MouseMode != EditorMouseModes.Default || (double) value <= 0.0 || !this.TryGetCaretRange(out start, out end))
        return;
      IPdfParagraph caretParagraph = this.editor.GetCaretParagraph();
      if (caretParagraph != null)
      {
        IPdfUndoItem undoItem;
        if (caretParagraph.SetFontSize(start, end, value, true, out undoItem))
        {
          this.editor.operationManager.AddUndoItem(undoItem);
          this.UpdateViewerState(true);
        }
        else
          this.UpdateViewerState(false);
      }
    }
  }

  public FS_COLOR? TextColor
  {
    get => this.editor.GetCaretParagraph()?.GetTextColor(this.GetActualCaret());
    set
    {
      int start;
      int end;
      if (this.propertyChanging || this.editor.MouseMode != EditorMouseModes.Default || !value.HasValue || !this.TryGetCaretRange(out start, out end))
        return;
      IPdfParagraph caretParagraph = this.editor.GetCaretParagraph();
      if (caretParagraph != null)
      {
        IPdfUndoItem undoItem;
        if (caretParagraph.SetTextColor(start, end, value.Value, true, out undoItem))
        {
          this.editor.operationManager.AddUndoItem(undoItem);
          this.UpdateViewerState(true);
        }
        else
          this.UpdateViewerState(false);
      }
    }
  }

  public bool IsBold
  {
    get
    {
      int start;
      int end;
      if (this.editor.MouseMode == EditorMouseModes.Default && this.TryGetCaretRange(out start, out end))
      {
        IPdfParagraph caretParagraph = this.editor.GetCaretParagraph();
        if (caretParagraph != null)
          return caretParagraph.IsBold(start, end);
      }
      return false;
    }
  }

  public bool ToggleBold()
  {
    int start;
    int end;
    if (!this.propertyChanging && this.editor.MouseMode == EditorMouseModes.Default && this.TryGetCaretRange(out start, out end))
    {
      IPdfParagraph caretParagraph = this.editor.GetCaretParagraph();
      if (caretParagraph != null)
      {
        IPdfUndoItem undoItem;
        if (caretParagraph.SetBold(start, end, true, out undoItem))
        {
          this.editor.operationManager.AddUndoItem(undoItem);
          this.UpdateViewerState(true);
        }
        else
          this.UpdateViewerState(false);
      }
    }
    return this.IsBold;
  }

  public bool IsItalic
  {
    get
    {
      int start;
      int end;
      if (this.editor.MouseMode == EditorMouseModes.Default && this.TryGetCaretRange(out start, out end))
      {
        IPdfParagraph caretParagraph = this.editor.GetCaretParagraph();
        if (caretParagraph != null)
          return caretParagraph.IsItalic(start, end);
      }
      return false;
    }
  }

  public bool ToggleItalic()
  {
    int start;
    int end;
    if (!this.propertyChanging && this.editor.MouseMode == EditorMouseModes.Default && this.TryGetCaretRange(out start, out end))
    {
      IPdfParagraph caretParagraph = this.editor.GetCaretParagraph();
      if (caretParagraph != null)
      {
        IPdfUndoItem undoItem;
        if (caretParagraph.SetItalic(start, end, true, out undoItem))
        {
          this.editor.operationManager.AddUndoItem(undoItem);
          this.UpdateViewerState(true);
        }
        else
          this.UpdateViewerState(false);
      }
    }
    return this.IsItalic;
  }

  public FontData GetFont()
  {
    if (this.editor.MouseMode == EditorMouseModes.Default)
    {
      PdfFont font = this.editor.GetCaretParagraph()?.GetFont(this.GetActualCaret());
      if (font != null)
        return this.GetFontData(font);
    }
    return (FontData) null;
  }

  public void SetFont(FontData font)
  {
    int start;
    int end;
    if (this.propertyChanging || this.editor.MouseMode != EditorMouseModes.Default || font == null || !this.TryGetCaretRange(out start, out end))
      return;
    IPdfParagraph caretParagraph = this.editor.GetCaretParagraph();
    if (caretParagraph != null)
    {
      if (font.PdfFont != null)
      {
        IPdfUndoItem undoItem;
        if (caretParagraph.SetFont(start, end, font.PdfFont, true, out undoItem))
        {
          this.editor.operationManager.AddUndoItem(undoItem);
          this.UpdateViewerState(true);
        }
        else
          this.UpdateViewerState(false);
      }
      else if (!string.IsNullOrEmpty(font.FontFamily))
      {
        IPdfUndoItem undoItem;
        if (caretParagraph.SetFont(start, end, font.FontFamily, this.CultureInfo, true, out undoItem))
        {
          this.editor.operationManager.AddUndoItem(undoItem);
          this.UpdateViewerState(true);
        }
        else
          this.UpdateViewerState(false);
      }
    }
  }

  private FontData GetFontData(PdfFont font)
  {
    FontData fontData = new FontData(font);
    string str = font.BaseFontName;
    int num = str.IndexOf('+');
    if (num >= 0)
    {
      if (num == str.Length - 1)
        return (FontData) null;
      str = str.Substring(num + 1);
    }
    bool flag1 = font.IsBold();
    bool flag2 = font.IsItalic();
    fontData.FontFamily = str;
    if (flag1)
      fontData.FontStyle |= BoldItalicFlags.Bold;
    if (flag2)
      fontData.FontStyle |= BoldItalicFlags.Italic;
    return fontData;
  }

  private bool TryGetCaretRange(out int start, out int end)
  {
    return this.editor.caretInfo.TryGetCaretRange(out start, out end);
  }

  private int GetActualCaret()
  {
    int start;
    int end;
    if (!this.TryGetCaretRange(out start, out end))
    {
      start = this.editor.caretInfo.Caret;
      end = start;
    }
    if (start == -1)
      return -1;
    return start == end ? start : start + 1;
  }

  private void CaretInfo_CaretChanged(object sender, EventArgs e) => this.RaisePropertyChanged();

  public event EventHandler PropertyChanged;

  internal void RaisePropertyChanged()
  {
    this.raiseEventTimer.Stop();
    if (this.PropertyChanged == null)
      return;
    this.raiseEventTimer.Start();
  }

  private void RaisePropertyChangedInternal()
  {
    this.raiseEventTimer.Stop();
    if (!this.editor.IsLoaded || this.editor.Document == null)
      return;
    this.propertyChanging = true;
    try
    {
      EventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged != null)
        propertyChanged((object) this, EventArgs.Empty);
    }
    finally
    {
      this.propertyChanging = false;
    }
  }

  private void UpdateViewerState(bool raisePropertyChanged)
  {
    this.editor.UpdateCaretInfo();
    this.editor.ForceRender();
    if (!raisePropertyChanged)
      return;
    this.RaisePropertyChangedInternal();
  }
}
