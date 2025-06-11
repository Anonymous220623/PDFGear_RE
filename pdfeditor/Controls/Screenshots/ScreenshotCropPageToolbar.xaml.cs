// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.ScreenshotCropPageToolbar
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using CommomLib.Controls;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf;
using Patagames.Pdf.Net;
using pdfeditor.Controls.PageEditor;
using pdfeditor.ViewModels;
using PDFKit;
using PDFKit.Utils.PageHeaderFooters;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public partial class ScreenshotCropPageToolbar : UserControl, IComponentConnector
{
  public int[] ApplypageIndex;
  public static readonly DependencyProperty ScreenshotDialogProperty = DependencyProperty.Register(nameof (ScreenshotDialog), typeof (ScreenshotDialog), typeof (ScreenshotCropPageToolbar), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty PageMarginProperty = DependencyProperty.Register(nameof (PageMargin), typeof (MarginModel), typeof (ScreenshotCropPageToolbar), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty SelectionBorderVisibleProperty = DependencyProperty.Register(nameof (SelectionBorderVisible), typeof (bool), typeof (ScreenshotCropPageToolbar), new PropertyMetadata((object) false));
  public static readonly DependencyProperty PageRangeBorderVisibleProperty = DependencyProperty.Register(nameof (PageRangeBorderVisible), typeof (bool), typeof (ScreenshotCropPageToolbar), new PropertyMetadata((object) false));
  internal Canvas LayoutRoot;
  internal Border MenuBorder;
  internal Button SelectionButton;
  internal Button AcceptButton;
  internal Button CancelButton;
  internal Border SelectionBorder;
  internal Button btn_SelectionBorder_Close;
  internal NumberBox topNumBox;
  internal NumberBox bottomNumbox;
  internal NumberBox leftNumbox;
  internal NumberBox rightNumbox;
  internal Border PageRangeBorder;
  internal Button btn_PageRangeBorder_Close;
  internal RadioButton AllPagesRadioButton;
  internal RadioButton CurrentPageRadioButton;
  internal RadioButton SelectedPagesRadioButton;
  internal PageRangeTextBox RangeBox;
  internal ComboBox applyToComboBox;
  internal Button btnPageRangeOk;
  private bool _contentLoaded;

  public MainViewModel VM => Ioc.Default.GetRequiredService<MainViewModel>();

  public ScreenshotCropPageToolbar() => this.InitializeComponent();

  public ScreenshotDialog ScreenshotDialog
  {
    get => (ScreenshotDialog) this.GetValue(ScreenshotCropPageToolbar.ScreenshotDialogProperty);
    set => this.SetValue(ScreenshotCropPageToolbar.ScreenshotDialogProperty, (object) value);
  }

  public MarginModel PageMargin
  {
    get => (MarginModel) this.GetValue(ScreenshotCropPageToolbar.PageMarginProperty);
    set => this.SetValue(ScreenshotCropPageToolbar.PageMarginProperty, (object) value);
  }

  private static void PageMarginPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
  }

  public bool SelectionBorderVisible
  {
    get => (bool) this.GetValue(ScreenshotCropPageToolbar.SelectionBorderVisibleProperty);
    set => this.SetValue(ScreenshotCropPageToolbar.SelectionBorderVisibleProperty, (object) value);
  }

  public bool PageRangeBorderVisible
  {
    get => (bool) this.GetValue(ScreenshotCropPageToolbar.PageRangeBorderVisibleProperty);
    set => this.SetValue(ScreenshotCropPageToolbar.PageRangeBorderVisibleProperty, (object) value);
  }

  private void RangeButton_Click(object sender, RoutedEventArgs e)
  {
    this.PageRangeBorderVisible = !this.PageRangeBorderVisible;
    this.RangeBox.Text = string.Empty;
    this.SelectionBorderVisible = false;
    this.ApplypageIndex = (int[]) null;
  }

  private void SelectionButton_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("CropPage", "AdjustBtn", "Count", 1L);
    this.PageRangeBorderVisible = false;
    if (this.SelectionBorder.Visibility != Visibility.Visible)
    {
      this.SelectionBorderVisible = true;
      this.SelectionBorder.Visibility = Visibility.Visible;
    }
    else
    {
      this.SelectionBorderVisible = false;
      this.SelectionBorder.Visibility = Visibility.Collapsed;
    }
    this.topNumBox.Maximum = PageHeaderFooterUtils.PdfPointToCm(this.PageMargin.PageHeight - this.PageMargin.Bottom);
    this.bottomNumbox.Maximum = PageHeaderFooterUtils.PdfPointToCm(this.PageMargin.PageHeight - this.PageMargin.Top);
    this.leftNumbox.Maximum = PageHeaderFooterUtils.PdfPointToCm(this.PageMargin.PageWidth - this.PageMargin.Right);
    this.rightNumbox.Maximum = PageHeaderFooterUtils.PdfPointToCm(this.PageMargin.PageWidth - this.PageMargin.Left);
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("CropPage", "CancelBtn", "Count", 1L);
    this.ScreenshotDialog?.Close();
    this.ApplypageIndex = (int[]) null;
  }

  private void AcceptButton_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("CropPage", "DoneBtn", "Count", 1L);
    this.ScreenshotDialog.CompleteCropPageAsync(this.ApplypageIndex);
    this.ApplypageIndex = (int[]) null;
    PDFKit.PdfControl.GetPdfControl(this.VM.Document)?.Viewer.PageCropBoxInfo?.Clear();
  }

  private void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
  {
  }

  private void btnPageRangeOk_Click(object sender, RoutedEventArgs e)
  {
    if (string.IsNullOrEmpty(this.RangeBox.Text.Trim()))
    {
      bool? isChecked1 = this.CurrentPageRadioButton.IsChecked;
      bool flag1 = false;
      if (isChecked1.GetValueOrDefault() == flag1 & isChecked1.HasValue)
      {
        bool? isChecked2 = this.AllPagesRadioButton.IsChecked;
        bool flag2 = false;
        if (isChecked2.GetValueOrDefault() == flag2 & isChecked2.HasValue)
        {
          int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.WinSignaturePageRangeIncorrect, UtilManager.GetProductName());
          return;
        }
      }
    }
    this.ApplypageIndex = this.GetImportPageRange();
    if (this.ApplypageIndex == null)
      return;
    this.PageRangeBorderVisible = false;
    FS_POINTF startPt = this.ScreenshotDialog.startPt;
    FS_POINTF curPt = this.ScreenshotDialog.curPt;
    this.SetApplyPageIndexCropBoxsInfo(this.ApplypageIndex, new FS_RECTF(Math.Min(startPt.X, curPt.X), Math.Max(startPt.Y, curPt.Y), Math.Max(startPt.X, curPt.X), Math.Min(startPt.Y, curPt.Y)));
  }

  private void SetApplyPageIndexCropBoxsInfo(int[] applyIndexs, FS_RECTF rect)
  {
    SortedDictionary<int, FS_RECTF> sortedDictionary = new SortedDictionary<int, FS_RECTF>();
    PdfViewer viewer = PDFKit.PdfControl.GetPdfControl(this.VM.Document)?.Viewer;
    for (int index = 0; index < applyIndexs.Length; ++index)
      sortedDictionary.Add(applyIndexs[index], rect);
    viewer.PageCropBoxInfo = sortedDictionary;
  }

  private int[] GetImportPageRange()
  {
    PdfDocument doc = this.VM.Document;
    int[] source;
    if (this.AllPagesRadioButton.IsChecked.GetValueOrDefault())
    {
      if (doc.Pages.Count == 0)
        return (int[]) null;
      source = Enumerable.Range(0, doc.Pages.Count).ToArray<int>();
    }
    else if (this.CurrentPageRadioButton.IsChecked.GetValueOrDefault())
      source = new int[1]{ this.ScreenshotDialog.pageIdx };
    else
      source = this.RangeBox.PageIndexes.ToArray<int>();
    if (((IEnumerable<int>) source).Any<int>((Func<int, bool>) (c => c < 0 || c >= doc.Pages.Count)))
    {
      int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.WinSignaturePageRangeIncorrect, UtilManager.GetProductName());
      return (int[]) null;
    }
    if (this.applyToComboBox.SelectedIndex == 1)
      source = ((IEnumerable<int>) source).Where<int>((Func<int, bool>) (c => c % 2 == 0)).ToArray<int>();
    else if (this.applyToComboBox.SelectedIndex == 2)
      source = ((IEnumerable<int>) source).Where<int>((Func<int, bool>) (c => c % 2 == 1)).ToArray<int>();
    if (source.Length != 0)
      return source;
    int num1 = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.WinSignaturePageRangeIncorrect, UtilManager.GetProductName());
    return (int[]) null;
  }

  private void btn_SelectionBorder_Close_Click(object sender, RoutedEventArgs e)
  {
    this.SelectionBorderVisible = false;
    this.SelectionBorder.Visibility = Visibility.Collapsed;
  }

  private void btn_PageRangeBorder_Close_Click(object sender, RoutedEventArgs e)
  {
    this.PageRangeBorderVisible = false;
    this.ApplypageIndex = (int[]) null;
  }

  private void NumberBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
  {
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/screenshots/screenshotcroppagetoolbar.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.LayoutRoot = (Canvas) target;
        this.LayoutRoot.SizeChanged += new SizeChangedEventHandler(this.LayoutRoot_SizeChanged);
        break;
      case 2:
        this.MenuBorder = (Border) target;
        break;
      case 3:
        this.SelectionButton = (Button) target;
        this.SelectionButton.Click += new RoutedEventHandler(this.SelectionButton_Click);
        break;
      case 4:
        this.AcceptButton = (Button) target;
        this.AcceptButton.Click += new RoutedEventHandler(this.AcceptButton_Click);
        break;
      case 5:
        this.CancelButton = (Button) target;
        this.CancelButton.Click += new RoutedEventHandler(this.CancelButton_Click);
        break;
      case 6:
        this.SelectionBorder = (Border) target;
        break;
      case 7:
        this.btn_SelectionBorder_Close = (Button) target;
        this.btn_SelectionBorder_Close.Click += new RoutedEventHandler(this.btn_SelectionBorder_Close_Click);
        break;
      case 8:
        this.topNumBox = (NumberBox) target;
        break;
      case 9:
        this.bottomNumbox = (NumberBox) target;
        break;
      case 10:
        this.leftNumbox = (NumberBox) target;
        break;
      case 11:
        this.rightNumbox = (NumberBox) target;
        break;
      case 12:
        this.PageRangeBorder = (Border) target;
        break;
      case 13:
        this.btn_PageRangeBorder_Close = (Button) target;
        this.btn_PageRangeBorder_Close.Click += new RoutedEventHandler(this.btn_PageRangeBorder_Close_Click);
        break;
      case 14:
        this.AllPagesRadioButton = (RadioButton) target;
        break;
      case 15:
        this.CurrentPageRadioButton = (RadioButton) target;
        break;
      case 16 /*0x10*/:
        this.SelectedPagesRadioButton = (RadioButton) target;
        break;
      case 17:
        this.RangeBox = (PageRangeTextBox) target;
        break;
      case 18:
        this.applyToComboBox = (ComboBox) target;
        break;
      case 19:
        this.btnPageRangeOk = (Button) target;
        this.btnPageRangeOk.Click += new RoutedEventHandler(this.btnPageRangeOk_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
