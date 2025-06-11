// Decompiled with JetBrains decompiler
// Type: PDFKit.ToolBars.PdfToolBarRotate
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Enums;
using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace PDFKit.ToolBars;

public class PdfToolBarRotate : PdfToolBar
{
  protected override void InitializeButtons()
  {
    this.Items.Add((object) this.CreateButton("btnRotateLeft", PDFKit.Properties.Resources.btnRotateLeftText, PDFKit.Properties.Resources.btnRotateLeftToolTipText, this.CreateUriToResource("rotateLeft.png"), new RoutedEventHandler(this.btn_RotateLeftClick)));
    this.Items.Add((object) this.CreateButton("btnRotateRight", PDFKit.Properties.Resources.btnRotateRightText, PDFKit.Properties.Resources.btnRotateRightToolTipText, this.CreateUriToResource("rotateRight.png"), new RoutedEventHandler(this.btn_RotateRightClick)));
  }

  protected override void UpdateButtons()
  {
    if (this.Items[0] is Button button1)
    {
      int num = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
      button1.IsEnabled = num != 0;
    }
    if (!(this.Items[1] is Button button2))
      return;
    int num1 = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
    button2.IsEnabled = num1 != 0;
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

  private void btn_RotateLeftClick(object sender, EventArgs e)
  {
    this.OnRotateLeftClick(this.Items[0] as Button);
  }

  private void btn_RotateRightClick(object sender, EventArgs e)
  {
    this.OnRotateRightClick(this.Items[1] as Button);
  }

  protected virtual void OnRotateLeftClick(Button item)
  {
    PageRotate rotation = this.PdfViewer.Document.Pages.CurrentPage.Rotation;
    this.PdfViewer.RotatePage(this.PdfViewer.CurrentIndex, rotation <= PageRotate.Normal ? PageRotate.Rotate270 : rotation - 1);
  }

  protected virtual void OnRotateRightClick(Button item)
  {
    PageRotate rotation = this.PdfViewer.Document.Pages.CurrentPage.Rotation;
    this.PdfViewer.RotatePage(this.PdfViewer.CurrentIndex, rotation >= PageRotate.Rotate270 ? PageRotate.Normal : rotation + 1);
  }

  private void UnsubscribePdfViewEvents(PdfViewer oldValue)
  {
    oldValue.AfterDocumentChanged -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.DocumentLoaded -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.DocumentClosed -= new EventHandler(this.PdfViewer_SomethingChanged);
  }

  private void SubscribePdfViewEvents(PdfViewer newValue)
  {
    newValue.AfterDocumentChanged += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.DocumentLoaded += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.DocumentClosed += new EventHandler(this.PdfViewer_SomethingChanged);
  }
}
