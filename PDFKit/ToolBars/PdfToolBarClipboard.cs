// Decompiled with JetBrains decompiler
// Type: PDFKit.ToolBars.PdfToolBarClipboard
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace PDFKit.ToolBars;

public class PdfToolBarClipboard : PdfToolBar
{
  protected override void InitializeButtons()
  {
    this.Items.Add((object) this.CreateButton("btnSelectAll", PDFKit.Properties.Resources.btnSelectAllText, PDFKit.Properties.Resources.btnSelectAllToolTipText, this.CreateUriToResource("selectAll.png"), new RoutedEventHandler(this.btn_SelectAllClick)));
    this.Items.Add((object) this.CreateButton("btnCopy", PDFKit.Properties.Resources.btnCopyText, PDFKit.Properties.Resources.btnCopyToolTipText, this.CreateUriToResource("textCopy.png"), new RoutedEventHandler(this.btn_CopyClick)));
  }

  protected override void UpdateButtons()
  {
    if (this.Items[0] is Button button1)
    {
      int num = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
      button1.IsEnabled = num != 0;
    }
    if (this.Items[1] is Button button2)
    {
      int num = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
      button2.IsEnabled = num != 0;
    }
    if (this.PdfViewer == null || this.PdfViewer.Document == null || !(this.Items[1] is Button button3))
      return;
    button3.IsEnabled = this.PdfViewer.SelectedText.Length > 0;
  }

  protected override void OnPdfViewerChanging(PdfViewer oldValue, PdfViewer newValue)
  {
    base.OnPdfViewerChanging(oldValue, newValue);
    if (oldValue != null)
      this.UnsubscribePdfViewEvents(oldValue);
    if (newValue == null)
      return;
    this.SubscribePdfViewEvents(newValue);
  }

  private void PdfViewer_SomethingChanged(object sender, EventArgs e) => this.UpdateButtons();

  private void btn_SelectAllClick(object sender, EventArgs e)
  {
    this.OnSelectAllClick(this.Items[0] as Button);
  }

  private void btn_CopyClick(object sender, EventArgs e)
  {
    this.OnCopyClick(this.Items[1] as Button);
  }

  protected virtual void OnSelectAllClick(Button item)
  {
    this.PdfViewer.SelectText(0, 0, this.PdfViewer.Document.Pages.Count - 1, this.PdfViewer.Document.Pages[this.PdfViewer.Document.Pages.Count - 1].Text.CountChars);
  }

  protected virtual void OnCopyClick(Button item) => Clipboard.SetText(this.PdfViewer.SelectedText);

  private void UnsubscribePdfViewEvents(PdfViewer oldValue)
  {
    oldValue.AfterDocumentChanged -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.DocumentLoaded -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.DocumentClosed -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.SelectionChanged -= new EventHandler(this.PdfViewer_SomethingChanged);
  }

  private void SubscribePdfViewEvents(PdfViewer newValue)
  {
    newValue.AfterDocumentChanged += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.DocumentLoaded += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.DocumentClosed += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.SelectionChanged += new EventHandler(this.PdfViewer_SomethingChanged);
  }
}
