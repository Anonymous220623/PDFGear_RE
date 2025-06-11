// Decompiled with JetBrains decompiler
// Type: PDFKit.ToolBars.PdfToolBarPages
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net.EventArguments;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace PDFKit.ToolBars;

public class PdfToolBarPages : PdfToolBar
{
  private TextBox CreateTextBox()
  {
    TextBox textBox = new TextBox();
    textBox.Name = "btnPageNumber";
    textBox.Width = 70.0;
    textBox.HorizontalAlignment = HorizontalAlignment.Center;
    textBox.KeyDown += new KeyEventHandler(this.btnPageNumber_KeyDown);
    return textBox;
  }

  protected override void InitializeButtons()
  {
    this.Items.Add((object) this.CreateButton("btnFirstPage", PDFKit.Properties.Resources.btnFirstPageText, PDFKit.Properties.Resources.btnFirstPageToolTipText, this.CreateUriToResource("toBegin.png"), new RoutedEventHandler(this.btn_FirstPageClick), 16 /*0x10*/, 16 /*0x10*/, PdfToolBar.ImageTextType.ImageOnly));
    this.Items.Add((object) this.CreateButton("btnPreviousPage", PDFKit.Properties.Resources.btnPreviousPageText, PDFKit.Properties.Resources.btnPreviousPageToolTipText, this.CreateUriToResource("toLeft.png"), new RoutedEventHandler(this.btn_PreviousPageClick), 16 /*0x10*/, 16 /*0x10*/, PdfToolBar.ImageTextType.ImageOnly));
    this.Items.Add((object) this.CreateTextBox());
    this.Items.Add((object) this.CreateButton("btnNextPage", PDFKit.Properties.Resources.btnNextPageText, PDFKit.Properties.Resources.btnNextPageToolTipText, this.CreateUriToResource("toRight.png"), new RoutedEventHandler(this.btn_NextPageClick), 16 /*0x10*/, 16 /*0x10*/, PdfToolBar.ImageTextType.ImageOnly));
    this.Items.Add((object) this.CreateButton("btnLastPage", PDFKit.Properties.Resources.btnLastPageText, PDFKit.Properties.Resources.btnLastPageToolTipText, this.CreateUriToResource("toEnd.png"), new RoutedEventHandler(this.btn_LastPageClick), 16 /*0x10*/, 16 /*0x10*/, PdfToolBar.ImageTextType.ImageOnly));
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
    if (this.Items[3] is Button button3)
    {
      int num = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
      button3.IsEnabled = num != 0;
    }
    if (this.Items[4] is Button button4)
    {
      int num = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
      button4.IsEnabled = num != 0;
    }
    if (!(this.Items[2] is TextBox textBox))
      return;
    textBox.IsEnabled = this.PdfViewer != null && this.PdfViewer.Document != null;
    if (this.PdfViewer == null || this.PdfViewer.Document == null)
      textBox.Text = "";
    else
      textBox.Text = $"{this.PdfViewer.Document.Pages.CurrentIndex + 1} / {this.PdfViewer.Document.Pages.Count}";
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

  private void btnPageNumber_KeyDown(object sender, KeyEventArgs e)
  {
    this.OnPageNumberKeyDown(this.Items[2] as TextBox, e);
  }

  private void btn_FirstPageClick(object sender, EventArgs e)
  {
    this.OnToBeginClick(this.Items[0] as Button);
  }

  private void btn_PreviousPageClick(object sender, EventArgs e)
  {
    this.OnToLeftClick(this.Items[1] as Button);
  }

  private void btn_NextPageClick(object sender, EventArgs e)
  {
    this.OnToRightClick(this.Items[3] as Button);
  }

  private void btn_LastPageClick(object sender, EventArgs e)
  {
    this.OnToEndClick(this.Items[4] as Button);
  }

  protected virtual void OnPageNumberKeyDown(TextBox item, KeyEventArgs e)
  {
    if (item == null || e.Key != Key.Return)
      return;
    int result = 0;
    string s = item.Text;
    char[] anyOf = new char[3]{ ' ', '/', '\\' };
    int num = s.LastIndexOfAny(anyOf);
    if (num > 0)
      s = s.Substring(0, num - 1);
    if (!int.TryParse(s, out result))
      return;
    if (result < 1)
      result = 1;
    else if (result > this.PdfViewer.Document.Pages.Count)
      result = this.PdfViewer.Document.Pages.Count;
    this.PdfViewer.ScrollToPage(result - 1);
    this.PdfViewer.CurrentIndex = result - 1;
    item.Text = $"{result} / {this.PdfViewer.Document.Pages.Count}";
  }

  protected virtual void OnToBeginClick(Button item)
  {
    this.PdfViewer.ScrollToPage(0);
    this.PdfViewer.CurrentIndex = 0;
  }

  protected virtual void OnToLeftClick(Button item)
  {
    int currentIndex = this.PdfViewer.CurrentIndex;
    if (currentIndex > 0)
      --currentIndex;
    this.PdfViewer.ScrollToPage(currentIndex);
    this.PdfViewer.CurrentIndex = currentIndex;
  }

  protected virtual void OnToRightClick(Button item)
  {
    int currentIndex = this.PdfViewer.CurrentIndex;
    if (currentIndex < this.PdfViewer.Document.Pages.Count - 1)
      ++currentIndex;
    this.PdfViewer.ScrollToPage(currentIndex);
    this.PdfViewer.CurrentIndex = currentIndex;
  }

  protected virtual void OnToEndClick(Button item)
  {
    int index = this.PdfViewer.Document.Pages.Count - 1;
    this.PdfViewer.ScrollToPage(index);
    this.PdfViewer.CurrentIndex = index;
  }

  private void UnsubscribePdfViewEvents(PdfViewer oldValue)
  {
    if (oldValue.Document != null)
    {
      oldValue.Document.Pages.PageInserted -= new EventHandler<PageCollectionChangedEventArgs>(this.PdfViewer_SomethingChanged);
      oldValue.Document.Pages.PageDeleted -= new EventHandler<PageCollectionChangedEventArgs>(this.PdfViewer_SomethingChanged);
    }
    oldValue.BeforeDocumentChanged -= new EventHandler<DocumentClosingEventArgs>(this.Subscribe_BeforeDocumentChanged);
    oldValue.AfterDocumentChanged -= new EventHandler(this.Subscribe_AfterDocumentChanged);
    oldValue.AfterDocumentChanged -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.DocumentLoaded -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.DocumentClosed -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.CurrentPageChanged -= new EventHandler(this.PdfViewer_SomethingChanged);
  }

  private void SubscribePdfViewEvents(PdfViewer newValue)
  {
    if (newValue.Document != null)
    {
      newValue.Document.Pages.PageInserted -= new EventHandler<PageCollectionChangedEventArgs>(this.PdfViewer_SomethingChanged);
      newValue.Document.Pages.PageDeleted -= new EventHandler<PageCollectionChangedEventArgs>(this.PdfViewer_SomethingChanged);
    }
    newValue.BeforeDocumentChanged += new EventHandler<DocumentClosingEventArgs>(this.Subscribe_BeforeDocumentChanged);
    newValue.AfterDocumentChanged += new EventHandler(this.Subscribe_AfterDocumentChanged);
    newValue.AfterDocumentChanged += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.DocumentLoaded += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.DocumentClosed += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.CurrentPageChanged += new EventHandler(this.PdfViewer_SomethingChanged);
  }

  private void Subscribe_AfterDocumentChanged(object sender, EventArgs e)
  {
    if (this.PdfViewer.Document == null)
      return;
    this.PdfViewer.Document.Pages.PageInserted += new EventHandler<PageCollectionChangedEventArgs>(this.PdfViewer_SomethingChanged);
    this.PdfViewer.Document.Pages.PageDeleted += new EventHandler<PageCollectionChangedEventArgs>(this.PdfViewer_SomethingChanged);
  }

  private void Subscribe_BeforeDocumentChanged(object sender, DocumentClosingEventArgs e)
  {
    if (this.PdfViewer.Document == null)
      return;
    this.PdfViewer.Document.Pages.PageInserted -= new EventHandler<PageCollectionChangedEventArgs>(this.PdfViewer_SomethingChanged);
    this.PdfViewer.Document.Pages.PageDeleted -= new EventHandler<PageCollectionChangedEventArgs>(this.PdfViewer_SomethingChanged);
  }
}
