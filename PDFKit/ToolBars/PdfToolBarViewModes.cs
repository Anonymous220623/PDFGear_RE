// Decompiled with JetBrains decompiler
// Type: PDFKit.ToolBars.PdfToolBarViewModes
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Windows;
using System.Windows.Controls.Primitives;

#nullable disable
namespace PDFKit.ToolBars;

public class PdfToolBarViewModes : PdfToolBar
{
  private int _tilesCount = -1;

  protected override void InitializeButtons()
  {
    this.Items.Add((object) this.CreateToggleButton("btnModeSingle", PDFKit.Properties.Resources.btnModeSingleText, PDFKit.Properties.Resources.btnModeSingleToolTipText, "modeSingle.png", new RoutedEventHandler(this.btn_ModeSingleClick), 16 /*0x10*/, 16 /*0x10*/, PdfToolBar.ImageTextType.ImageOnly));
    this.Items.Add((object) this.CreateToggleButton("btnModeVertical", PDFKit.Properties.Resources.btnModeVerticalText, PDFKit.Properties.Resources.btnModeVerticalToolTipText, "modeVertical.png", new RoutedEventHandler(this.btn_ModeVerticalClick), 16 /*0x10*/, 16 /*0x10*/, PdfToolBar.ImageTextType.ImageOnly));
    this.Items.Add((object) this.CreateToggleButton("btnModeHorizontal", PDFKit.Properties.Resources.btnModeHorizontalText, PDFKit.Properties.Resources.btnModeHorizontalToolTipText, "modeHorizontal.png", new RoutedEventHandler(this.btn_ModeHorizontalClick), 16 /*0x10*/, 16 /*0x10*/, PdfToolBar.ImageTextType.ImageOnly));
    this.Items.Add((object) this.CreateToggleButton("btnModeTiles", PDFKit.Properties.Resources.btnModeTilesText, PDFKit.Properties.Resources.btnModeTilesToolTipText, "modeTiles.png", new RoutedEventHandler(this.btn_ModeTilesClick), 16 /*0x10*/, 16 /*0x10*/, PdfToolBar.ImageTextType.ImageOnly));
    this.Items.Add((object) this.CreateToggleButton("btnModeTwoPage", PDFKit.Properties.Resources.btnModeTwoPageText, PDFKit.Properties.Resources.btnModeTwoPageToolTipText, "modeTwoPage.png", new RoutedEventHandler(this.btn_ModeTwoPageClick), 16 /*0x10*/, 16 /*0x10*/, PdfToolBar.ImageTextType.ImageOnly));
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
    if (this.Items[4] is ToggleButton toggleButton5)
    {
      int num = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
      toggleButton5.IsEnabled = num != 0;
    }
    if (this.PdfViewer == null || this.PdfViewer.Document == null)
      return;
    if (this.Items[0] is ToggleButton toggleButton6)
      toggleButton6.IsChecked = new bool?(this.PdfViewer.ViewMode == ViewModes.SinglePage);
    if (this.Items[1] is ToggleButton toggleButton7)
      toggleButton7.IsChecked = new bool?(this.PdfViewer.ViewMode == ViewModes.Vertical);
    if (this.Items[2] is ToggleButton toggleButton8)
      toggleButton8.IsChecked = new bool?(this.PdfViewer.ViewMode == ViewModes.Horizontal);
    if (this.Items[3] is ToggleButton toggleButton9)
      toggleButton9.IsChecked = new bool?(this.PdfViewer.ViewMode == ViewModes.TilesVertical);
    if (!(this.Items[4] is ToggleButton toggleButton10))
      return;
    toggleButton10.IsChecked = new bool?(this.PdfViewer.ViewMode == ViewModes.TilesLine);
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

  private void btn_ModeSingleClick(object sender, EventArgs e)
  {
    this.OnModeSingleClick(this.Items[0] as ToggleButton);
  }

  private void btn_ModeVerticalClick(object sender, EventArgs e)
  {
    this.OnModeVerticalClick(this.Items[1] as ToggleButton);
  }

  private void btn_ModeHorizontalClick(object sender, EventArgs e)
  {
    this.OnModeHorizontalClick(this.Items[2] as ToggleButton);
  }

  private void btn_ModeTilesClick(object sender, EventArgs e)
  {
    this.OnModeTilesClick(this.Items[3] as ToggleButton);
  }

  private void btn_ModeTwoPageClick(object sender, EventArgs e)
  {
    this.OnModeTwoPageClick(this.Items[4] as ToggleButton);
  }

  protected virtual void OnModeSingleClick(ToggleButton item)
  {
    this.PdfViewer.ViewMode = ViewModes.SinglePage;
  }

  protected virtual void OnModeVerticalClick(ToggleButton item)
  {
    this.PdfViewer.ViewMode = ViewModes.Vertical;
  }

  protected virtual void OnModeHorizontalClick(ToggleButton item)
  {
    this.PdfViewer.ViewMode = ViewModes.Horizontal;
  }

  protected virtual void OnModeTilesClick(ToggleButton item)
  {
    if (this._tilesCount != -1)
      this.PdfViewer.TilesCount = this._tilesCount;
    this.PdfViewer.ViewMode = ViewModes.TilesVertical;
  }

  protected virtual void OnModeTwoPageClick(ToggleButton item)
  {
    this._tilesCount = this.PdfViewer.TilesCount;
    this.PdfViewer.TilesCount = 2;
    this.PdfViewer.ViewMode = ViewModes.TilesLine;
  }

  private void UnsubscribePdfViewEvents(PdfViewer oldValue)
  {
    oldValue.AfterDocumentChanged -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.DocumentLoaded -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.DocumentClosed -= new EventHandler(this.PdfViewer_SomethingChanged);
    oldValue.ViewModeChanged -= new EventHandler(this.PdfViewer_SomethingChanged);
  }

  private void SubscribePdfViewEvents(PdfViewer newValue)
  {
    newValue.AfterDocumentChanged += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.DocumentLoaded += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.DocumentClosed += new EventHandler(this.PdfViewer_SomethingChanged);
    newValue.ViewModeChanged += new EventHandler(this.PdfViewer_SomethingChanged);
  }
}
