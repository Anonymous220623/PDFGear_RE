// Decompiled with JetBrains decompiler
// Type: PDFKit.ToolBars.PdfToolBarSizes
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Windows;
using System.Windows.Controls.Primitives;

#nullable disable
namespace PDFKit.ToolBars;

public class PdfToolBarSizes : PdfToolBar
{
  protected override void InitializeButtons()
  {
    this.Items.Add((object) this.CreateToggleButton("btnActualSize", PDFKit.Properties.Resources.btnActualSizeText, PDFKit.Properties.Resources.btnActualSizeToolTipText, "viewActualSize.PNG", new RoutedEventHandler(this.btn_ActualSizeClick)));
    this.Items.Add((object) this.CreateToggleButton("btnFitPage", PDFKit.Properties.Resources.btnFitPageText, PDFKit.Properties.Resources.btnFitPageToolTipText, "viewFitPage.PNG", new RoutedEventHandler(this.btn_FitPageClick)));
    this.Items.Add((object) this.CreateToggleButton("btnFitWidth", PDFKit.Properties.Resources.btnFitWidthText, PDFKit.Properties.Resources.btnFitWidthToolTipText, "viewFitWidth.PNG", new RoutedEventHandler(this.btn_FitWidthClick)));
    this.Items.Add((object) this.CreateToggleButton("btnFitHeight", PDFKit.Properties.Resources.btnFitHeightText, PDFKit.Properties.Resources.btnFitHeightToolTipText, "viewFitHeight.PNG", new RoutedEventHandler(this.btn_FitHeightClick)));
  }

  protected override void UpdateButtons()
  {
    if (this.Items[0] is ToggleButton toggleButton1)
    {
      int num = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
      toggleButton1.IsEnabled = num != 0;
    }
    if (this.Items[1] is ToggleButton toggleButton2)
    {
      int num = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
      toggleButton2.IsEnabled = num != 0;
    }
    if (this.Items[2] is ToggleButton toggleButton3)
    {
      int num = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
      toggleButton3.IsEnabled = num != 0;
    }
    if (this.Items[3] is ToggleButton toggleButton4)
    {
      int num = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
      toggleButton4.IsEnabled = num != 0;
    }
    if (this.PdfViewer == null || this.PdfViewer.Document == null)
      return;
    if (this.Items[0] is ToggleButton toggleButton5)
    {
      bool? nullable = new bool?(this.PdfViewer.SizeMode == SizeModes.Zoom && (double) this.PdfViewer.Zoom >= 0.99996 && (double) this.PdfViewer.Zoom <= 1.00004);
      toggleButton5.IsChecked = nullable;
    }
    if (this.Items[1] is ToggleButton toggleButton6)
      toggleButton6.IsChecked = new bool?(this.PdfViewer.SizeMode == SizeModes.FitToSize);
    if (this.Items[2] is ToggleButton toggleButton7)
      toggleButton7.IsChecked = new bool?(this.PdfViewer.SizeMode == SizeModes.FitToWidth);
    if (!(this.Items[3] is ToggleButton toggleButton8))
      return;
    toggleButton8.IsChecked = new bool?(this.PdfViewer.SizeMode == SizeModes.FitToHeight);
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

  private void btn_ActualSizeClick(object sender, EventArgs e)
  {
    this.OnActualSizeClick(this.Items[0] as ToggleButton);
  }

  private void btn_FitPageClick(object sender, EventArgs e)
  {
    this.OnFitPageClick(this.Items[1] as ToggleButton);
  }

  private void btn_FitWidthClick(object sender, EventArgs e)
  {
    this.OnFitWidthClick(this.Items[2] as ToggleButton);
  }

  private void btn_FitHeightClick(object sender, EventArgs e)
  {
    this.OnFitHeightClick(this.Items[3] as ToggleButton);
  }

  protected virtual void OnActualSizeClick(ToggleButton item)
  {
    this.UnsubscribePdfViewEvents(this.PdfViewer);
    this.PdfViewer.SizeMode = SizeModes.Zoom;
    this.PdfViewer.Zoom = 1f;
    this.SubscribePdfViewEvents(this.PdfViewer);
    this.UpdateButtons();
  }

  protected virtual void OnFitPageClick(ToggleButton item)
  {
    this.PdfViewer.SizeMode = SizeModes.FitToSize;
  }

  protected virtual void OnFitWidthClick(ToggleButton item)
  {
    this.PdfViewer.SizeMode = SizeModes.FitToWidth;
  }

  protected virtual void OnFitHeightClick(ToggleButton item)
  {
    this.PdfViewer.SizeMode = SizeModes.FitToHeight;
  }

  private void UnsubscribePdfViewEvents(PdfViewer oldValue)
  {
    oldValue.AfterDocumentChanged -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.DocumentLoaded -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.DocumentClosed -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.SizeModeChanged -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.ZoomChanged -= new EventHandler(this.PdfViewer_SomethingChanged);
  }

  private void SubscribePdfViewEvents(PdfViewer newValue)
  {
    newValue.AfterDocumentChanged += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.DocumentLoaded += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.DocumentClosed += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.SizeModeChanged += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.ZoomChanged += new EventHandler(this.PdfViewer_SomethingChanged);
  }
}
