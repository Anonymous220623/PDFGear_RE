// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.ScreenshotDialog
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using pdfeditor.Views;
using PDFKit;
using PDFKit.Services;
using PDFKit.Utils;
using PDFKit.Utils.PageHeaderFooters;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public partial class ScreenshotDialog : System.Windows.Controls.UserControl, INotifyPropertyChanged, IComponentConnector
{
  private Window window;
  private TaskCompletionSource<ScreenshotDialogResult> tcs;
  private PdfViewer viewer;
  private ScrollViewer viewerSv;
  public Rect curRect;
  public int pageIdx = -1;
  public FS_POINTF startPt;
  public FS_POINTF curPt;
  private System.Windows.Point curPoint;
  private DispatcherTimer autoScrollTimer;
  private ScreenshotDialogMode mode;
  private DrawControlMode drawControlMode;
  private System.Windows.Point startDrawPoint;
  private System.Windows.Point curDrawPoint;
  private System.Windows.Media.Brush currentBrush = (System.Windows.Media.Brush) new SolidColorBrush(DrawSettingConstants.Colors[1]);
  private double currentThickness = DrawSettingConstants.Thicknesses[1];
  private double currentFontSize = DrawSettingConstants.DefaultFontSize;
  private Border ControlRectangle;
  private System.Windows.Controls.Control ControlArrow;
  private ControlTemplate controlArrowTemplate;
  private Ellipse ControlCircle;
  private Polyline ControlInk;
  private Border ControlTextBox;
  private string controlTextBoxText;
  private const int _width = 40;
  private Rect controlLocation;
  private Stack<DrawOperation> undoDrawControlStack;
  private bool isDragDrawControl;
  private UIElement selectedDrawControl;
  private TranslateTransform totalTranslate;
  private TranslateTransform tempTranslate;
  private System.Windows.Point dragStartPoint;
  private TransformGroup orignalTransformGroup;
  private float curZoomFactor;
  internal Border RootBorder;
  internal Canvas DraggerParent;
  internal RectangleGeometry BackgroundRectangle;
  internal RectangleGeometry DraggerRectangle;
  internal ResizeView DragResizeView;
  internal ScreenshotExtractTextToolbar ExtractTextToolbar;
  internal ScreenshotImageToolbar ImageToolbar;
  internal ScreenshotOcrToolbar OcrToolbar;
  internal ScreenshotCropPageToolbar CropPageToolbar;
  private bool _contentLoaded;

  public MainViewModel VM => Ioc.Default.GetRequiredService<MainViewModel>();

  public FS_RECTF? BeforeCropBox { get; private set; }

  public DrawControlMode DrawControlMode
  {
    get => this.drawControlMode;
    set
    {
      this.drawControlMode = value;
      this.DragResizeView.DragMode = value != DrawControlMode.None ? ResizeViewOperation.None : ResizeViewOperation.All;
      if (this.SelectedDrawControl != null && (!(this.SelectedDrawControl is Border selectedDrawControl1) || selectedDrawControl1.Child != null || this.drawControlMode != DrawControlMode.DrawRectangle) && (!(this.SelectedDrawControl is Border selectedDrawControl2) || !(selectedDrawControl2.Child is System.Windows.Controls.TextBox) || this.drawControlMode != DrawControlMode.DrawText) && (!(this.SelectedDrawControl is Ellipse) || this.drawControlMode != DrawControlMode.DrawCircle) && (!(this.SelectedDrawControl is Polyline) || this.drawControlMode != DrawControlMode.DrawInk) && (!(this.SelectedDrawControl is System.Windows.Controls.Control) || this.drawControlMode != DrawControlMode.DrawArrow))
        this.RemoveSelected();
      this.RaisePropertyChanged(nameof (DrawControlMode));
    }
  }

  public System.Windows.Media.Brush CurrentBrush
  {
    get => this.currentBrush;
    set => this.currentBrush = value;
  }

  public double CurrentThickness
  {
    get => this.currentThickness;
    set => this.currentThickness = value;
  }

  public double CurrentFontSize
  {
    get => this.currentFontSize;
    set => this.currentFontSize = value;
  }

  public UIElement SelectedDrawControl
  {
    get => this.selectedDrawControl;
    set => this.selectedDrawControl = value;
  }

  public ScreenshotDialog()
  {
    this.InitializeComponent();
    this.Loaded += new RoutedEventHandler(this.ScreenshotDialog_Loaded);
    this.Unloaded += new RoutedEventHandler(this.ScreenshotDialog_Unloaded);
    this.SizeChanged += new SizeChangedEventHandler(this.ScreenshotDialog_SizeChanged);
    this.autoScrollTimer = new DispatcherTimer(DispatcherPriority.Normal);
    this.autoScrollTimer.Interval = TimeSpan.FromMilliseconds(50.0);
    this.autoScrollTimer.Tick += new EventHandler(this.AutoScrollTimer_Tick);
    this.ExtractTextToolbar.ScreenshotDialog = this;
    this.ImageToolbar.ScreenshotDialog = this;
    this.OcrToolbar.ScreenshotDialog = this;
    this.CropPageToolbar.ScreenshotDialog = this;
    this.controlArrowTemplate = (ControlTemplate) this.FindResource((object) "PART_ControlArrow");
    this.undoDrawControlStack = new Stack<DrawOperation>();
  }

  private void ScreenshotDialog_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.BackgroundRectangle.Rect = new Rect(new System.Windows.Point(), e.NewSize);
  }

  private void ScreenshotDialog_Loaded(object sender, RoutedEventArgs e)
  {
    if (!(this.Parent is AnnotationCanvas parent))
      return;
    this.viewer = parent.PdfViewer;
    this.viewerSv = (ScrollViewer) this.viewer.Parent;
  }

  private void ScreenshotDialog_Unloaded(object sender, RoutedEventArgs e)
  {
  }

  private void CancelCore()
  {
    if (this.mode == ScreenshotDialogMode.CropPage)
      this.ResetCropSize(this.pageIdx);
    this.mode = ScreenshotDialogMode.Screenshot;
    this.DragResizeView.Opacity = 0.0;
    this.UpdateToolbarVisibility(false);
    Canvas.SetLeft((UIElement) this.DragResizeView, 0.0);
    Canvas.SetTop((UIElement) this.DragResizeView, 0.0);
    this.DragResizeView.Width = 0.0;
    this.DragResizeView.Height = 0.0;
    this.DraggerRectangle.Rect = new Rect();
    this.RootBorder.Cursor = System.Windows.Input.Cursors.Cross;
    this.pageIdx = -1;
    this.startPt = new FS_POINTF();
    this.curPt = new FS_POINTF();
    this.curPoint = new System.Windows.Point();
    this.viewer.PageCropBoxInfo?.Clear();
    this.startDrawPoint = new System.Windows.Point(-1.0, -1.0);
    this.curDrawPoint = new System.Windows.Point(-1.0, -1.0);
    this.SelectedDrawControl = (UIElement) null;
    this.controlLocation = new Rect();
    this.undoDrawControlStack.Clear();
    this.DragResizeView.ClearDrawUIElementOfCanvas();
    this.DrawControlMode = DrawControlMode.None;
  }

  public async Task<ScreenshotDialogResult> ShowDialogAsync(ScreenshotDialogMode mode)
  {
    ScreenshotDialog screenshotDialog = this;
    if (screenshotDialog.tcs != null)
      screenshotDialog.Close();
    screenshotDialog.mode = mode;
    screenshotDialog.viewer.ZoomChanged -= new EventHandler(screenshotDialog.Viewer_ZoomChanged);
    screenshotDialog.viewer.ZoomChanged += new EventHandler(screenshotDialog.Viewer_ZoomChanged);
    screenshotDialog.viewerSv.ScrollChanged -= new ScrollChangedEventHandler(screenshotDialog.ViewerSv_ScrollChanged);
    screenshotDialog.viewerSv.ScrollChanged += new ScrollChangedEventHandler(screenshotDialog.ViewerSv_ScrollChanged);
    if (screenshotDialog.window != null)
    {
      screenshotDialog.window.PreviewKeyDown -= new System.Windows.Input.KeyEventHandler(screenshotDialog.Window_PreviewKeyDown);
      screenshotDialog.window = (Window) null;
    }
    screenshotDialog.window = Window.GetWindow((DependencyObject) screenshotDialog);
    if (screenshotDialog.window != null)
      screenshotDialog.window.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(screenshotDialog.Window_PreviewKeyDown);
    screenshotDialog.Visibility = Visibility.Visible;
    if (mode == ScreenshotDialogMode.CropPage)
    {
      CommomLib.Commom.GAManager.SendEvent("CropPage", "ShowDialog", "Count", 1L);
      int currentIndex = screenshotDialog.viewer.CurrentIndex;
      screenshotDialog.RenderFirstCropBox(currentIndex);
      screenshotDialog.ResetSelectedBox(currentIndex);
      screenshotDialog.viewer.ScrollToPage(currentIndex);
      screenshotDialog.viewer.UpdateLayout();
      screenshotDialog.viewer.UpdateDocLayout();
      double contentVerticalOffset = screenshotDialog.viewerSv.ContentVerticalOffset;
      screenshotDialog.viewerSv.ScrollToVerticalOffset(contentVerticalOffset - 10.0);
    }
    screenshotDialog.undoDrawControlStack = new Stack<DrawOperation>();
    screenshotDialog.tcs = new TaskCompletionSource<ScreenshotDialogResult>();
    return await screenshotDialog.tcs.Task;
  }

  public void Close(ScreenshotDialogResult result = null)
  {
    this.CancelCore();
    if (this.window != null)
    {
      this.window.PreviewKeyDown -= new System.Windows.Input.KeyEventHandler(this.Window_PreviewKeyDown);
      this.window = (Window) null;
    }
    this.viewerSv.ScrollChanged -= new ScrollChangedEventHandler(this.ViewerSv_ScrollChanged);
    this.autoScrollTimer.Stop();
    if (this.tcs == null || this.tcs.Task.IsCompleted || this.tcs.Task.IsCanceled)
      return;
    TaskCompletionSource<ScreenshotDialogResult> tcs = this.tcs;
    this.tcs = (TaskCompletionSource<ScreenshotDialogResult>) null;
    this.Visibility = Visibility.Collapsed;
    ScreenshotDialogResult result1 = result ?? ScreenshotDialogResult.CreateCancel();
    tcs.SetResult(result1);
  }

  private async void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
  {
    if (e.Key == Key.Escape)
      this.Close();
    else if (e.Key == Key.Return)
    {
      if (this.mode == ScreenshotDialogMode.ExtractText)
        await this.CompleteExtractTextAsync(false);
      else if (this.mode == ScreenshotDialogMode.Screenshot)
      {
        if (this.DrawControlMode == DrawControlMode.DrawText && this.SelectedDrawControl is Border selectedDrawControl && selectedDrawControl.Child is System.Windows.Controls.TextBox)
          return;
        await this.CompleteImageAsync();
      }
      else
      {
        if (this.mode != ScreenshotDialogMode.Ocr)
          return;
        await this.CompleteExtractTextAsync(true);
      }
    }
    else
    {
      if (e.Key != Key.Delete || this.mode != ScreenshotDialogMode.Screenshot || this.SelectedDrawControl == null || this.SelectedDrawControl is Border selectedDrawControl1 && selectedDrawControl1.Child is System.Windows.Controls.TextBox child && string.IsNullOrEmpty(child.Text))
        return;
      FrameworkElement selectedDrawControl2 = (FrameworkElement) this.SelectedDrawControl;
      double left = Canvas.GetLeft((UIElement) selectedDrawControl2);
      double top = Canvas.GetTop((UIElement) selectedDrawControl2);
      this.DragResizeView?.RemoveDrawControl((UIElement) selectedDrawControl2);
      this.undoDrawControlStack.Push((DrawOperation) new DeleteControlOperation((UIElement) selectedDrawControl2, left, top));
      this.SelectedDrawControl = (UIElement) null;
    }
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    base.OnMouseWheel(e);
    Rect clientRect;
    if (this.viewerSv == null || !this.curRect.IsEmpty && !(this.curRect == new Rect()) || this.pageIdx != -1 && (!this.viewer.TryGetClientRect(this.pageIdx, this.viewer.Document.Pages[this.pageIdx].GetEffectiveBox(), out clientRect) || clientRect.Top > 0.0 && e.Delta > 0 || clientRect.Bottom < this.ActualHeight && e.Delta < 0))
      return;
    this.viewerSv.RaiseEvent((RoutedEventArgs) e);
  }

  private void ViewerSv_ScrollChanged(object sender, ScrollChangedEventArgs e)
  {
    if (this.pageIdx == -1)
      return;
    if (this.DragResizeView.Opacity == 0.0)
      this.curPt = this.GetCurrentPagePoint(this.curPoint);
    this.UpdateBounds(false);
  }

  protected override void OnMouseDown(MouseButtonEventArgs e)
  {
    if (e.ChangedButton == MouseButton.Left)
    {
      if (this.mode != ScreenshotDialogMode.Screenshot)
      {
        if (this.DragResizeView.Opacity == 0.0)
        {
          this.autoScrollTimer.Start();
          this.curPoint = e.GetPosition((IInputElement) this);
          this.pageIdx = this.viewer.PointInPage(this.curPoint);
          System.Windows.Point pagePoint;
          if (this.pageIdx != -1 && this.viewer.TryGetPagePoint(this.pageIdx, this.curPoint, out pagePoint))
          {
            this.startPt = pagePoint.ToPdfPoint();
            this.curPt = this.startPt;
            this.CaptureMouse();
            e.Handled = true;
            return;
          }
        }
      }
      else
      {
        if (this.SelectedDrawControl is Border selectedDrawControl && selectedDrawControl.Child is System.Windows.Controls.TextBox child)
        {
          if (child.Text != this.controlTextBoxText && !string.IsNullOrEmpty(this.controlTextBoxText))
            this.undoDrawControlStack.Push((DrawOperation) new ChangeTextBoxTextOperation(this.SelectedDrawControl, this.controlTextBoxText));
          this.controlTextBoxText = (string) null;
          this.RemoveSelected();
          return;
        }
        this.RemoveSelected();
        if (!this.isDragDrawControl)
        {
          if (this.DragResizeView.Opacity == 0.0)
          {
            this.autoScrollTimer.Start();
            this.curPoint = e.GetPosition((IInputElement) this);
            this.pageIdx = this.viewer.PointInPage(this.curPoint);
            System.Windows.Point pagePoint;
            if (this.pageIdx != -1 && this.viewer.TryGetPagePoint(this.pageIdx, this.curPoint, out pagePoint))
            {
              this.startPt = pagePoint.ToPdfPoint();
              this.curPt = this.startPt;
              this.CaptureMouse();
              e.Handled = true;
              return;
            }
          }
          else if (this.DragResizeView.Opacity != 0.0 && this.DrawControlMode != DrawControlMode.None)
          {
            this.startDrawPoint = new System.Windows.Point(-1.0, -1.0);
            System.Windows.Point position = e.GetPosition((IInputElement) this.DragResizeView);
            if (this.CheckPointInDraggerCanvas(position))
            {
              this.startDrawPoint = position;
              this.CaptureMouse();
              if (this.ControlTextBox != null)
                this.Focus();
              if (this.DrawControlMode == DrawControlMode.DrawText)
                this.DrawText();
              else
                this.Focus();
              e.Handled = true;
              return;
            }
          }
        }
      }
    }
    base.OnMouseDown(e);
  }

  protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
  {
    if (e.LeftButton == MouseButtonState.Pressed)
    {
      if (this.mode != ScreenshotDialogMode.Screenshot)
      {
        if (this.DragResizeView.Opacity == 0.0 && this.pageIdx != -1)
        {
          this.curPoint = e.GetPosition((IInputElement) this);
          this.curPt = this.GetCurrentPagePoint(this.curPoint);
          this.UpdateBounds(false);
          e.Handled = true;
          return;
        }
      }
      else if (!this.isDragDrawControl)
      {
        if (this.DragResizeView.Opacity == 0.0 && this.pageIdx != -1)
        {
          this.curPoint = e.GetPosition((IInputElement) this);
          this.curPt = this.GetCurrentPagePoint(this.curPoint);
          this.UpdateBounds(false);
          this.curZoomFactor = this.viewer.Zoom;
          e.Handled = true;
          return;
        }
        if (this.DragResizeView.Opacity != 0.0 && this.DrawControlMode != DrawControlMode.None)
        {
          if (this.startDrawPoint.X == -1.0 && this.startDrawPoint.Y == -1.0)
            return;
          this.curDrawPoint = new System.Windows.Point(-1.0, -1.0);
          this.curDrawPoint = e.GetPosition((IInputElement) this.DragResizeView);
          this.KeepPointInDraggerCanvas(ref this.curDrawPoint);
          if (this.startDrawPoint == this.curDrawPoint)
            return;
          this.DrawControl(this.curDrawPoint, this.DrawControlMode);
        }
      }
    }
    base.OnMouseMove(e);
  }

  protected override void OnMouseUp(MouseButtonEventArgs e)
  {
    if (e.ChangedButton == MouseButton.Left)
    {
      this.ReleaseMouseCapture();
      this.curPoint = e.GetPosition((IInputElement) this);
      this.autoScrollTimer.Stop();
      if (this.mode != ScreenshotDialogMode.Screenshot)
      {
        if (this.DragResizeView.Opacity == 0.0 && this.pageIdx != -1)
        {
          this.curPt = this.GetCurrentPagePoint(this.curPoint);
          this.DragResizeView.Opacity = 1.0;
          this.RootBorder.Cursor = System.Windows.Input.Cursors.Arrow;
          this.UpdateToolbarVisibility(true);
          this.UpdateBounds(false);
        }
      }
      else if (!this.isDragDrawControl)
      {
        if (this.DragResizeView.Opacity == 0.0 && this.pageIdx != -1)
        {
          this.curPt = this.GetCurrentPagePoint(this.curPoint);
          this.DragResizeView.Opacity = 1.0;
          this.RootBorder.Cursor = System.Windows.Input.Cursors.Arrow;
          this.UpdateToolbarVisibility(true);
          this.UpdateBounds(false);
        }
        else if (this.DragResizeView.Opacity != 0.0 && this.DrawControlMode != DrawControlMode.None && this.startDrawPoint.X != -1.0 && this.startDrawPoint.Y != -1.0 && this.startDrawPoint != this.curDrawPoint)
        {
          switch (this.DrawControlMode)
          {
            case DrawControlMode.DrawRectangle:
              if (this.ControlRectangle != null && this.controlLocation != new Rect())
              {
                this.undoDrawControlStack.Push((DrawOperation) new DrawControlOperation(OperationType.DrawRectangle, (UIElement) this.ControlRectangle));
                break;
              }
              break;
            case DrawControlMode.DrawCircle:
              if (this.ControlCircle != null && this.controlLocation != new Rect())
              {
                this.undoDrawControlStack.Push((DrawOperation) new DrawControlOperation(OperationType.DrawCircle, (UIElement) this.ControlCircle));
                break;
              }
              break;
            case DrawControlMode.DrawArrow:
              if (this.ControlArrow != null && this.controlLocation != new Rect())
              {
                this.undoDrawControlStack.Push((DrawOperation) new DrawControlOperation(OperationType.DrawArrow, (UIElement) this.ControlArrow));
                break;
              }
              break;
            case DrawControlMode.DrawInk:
              if (this.ControlInk != null)
              {
                this.undoDrawControlStack.Push((DrawOperation) new DrawControlOperation(OperationType.DrawInk, (UIElement) this.ControlInk));
                break;
              }
              break;
          }
        }
      }
    }
    else if (e.ChangedButton == MouseButton.Right && e.LeftButton != MouseButtonState.Pressed)
    {
      this.ReleaseMouseCapture();
      this.Cancel();
      this.Close();
    }
    this.ClearDraw();
    base.OnMouseUp(e);
  }

  public void ResetSelectedBox(int pageIdx)
  {
    if (pageIdx == -1)
      return;
    this.pageIdx = pageIdx;
    this.DragResizeView.Opacity = 1.0;
    PdfPage page = this.viewer.Document.Pages[pageIdx];
    this.VM.ViewToolbar.DocSizeMode = SizeModes.FitToSize;
    FS_RECTF? nullable = new FS_RECTF?();
    FS_RECTF? beforeCropBox = this.BeforeCropBox;
    if (beforeCropBox.HasValue)
    {
      beforeCropBox = this.BeforeCropBox;
      FS_RECTF fsRectf1 = beforeCropBox.Value;
      if (PageHeaderFooterUtils.PdfPointToCm((double) Math.Abs(fsRectf1.Width)) >= 0.15)
      {
        beforeCropBox = this.BeforeCropBox;
        fsRectf1 = beforeCropBox.Value;
        if (PageHeaderFooterUtils.PdfPointToCm((double) Math.Abs(fsRectf1.Width)) >= 0.15)
        {
          ref FS_RECTF? local = ref nullable;
          beforeCropBox = this.BeforeCropBox;
          FS_RECTF fsRectf2 = beforeCropBox.Value;
          local = new FS_RECTF?(fsRectf2);
        }
      }
    }
    if (!nullable.HasValue)
      nullable = new FS_RECTF?(page.MediaBox);
    this.curPt.X = nullable.Value.right;
    this.curPt.Y = nullable.Value.bottom;
    this.startPt.Y = nullable.Value.top;
    this.startPt.X = nullable.Value.left;
    page.ReloadPage();
    PdfDocumentStateService.TryRedrawViewerCurrentPage(page);
    this.UpdateBounds(false);
    this.UpdateToolbarVisibility(true);
    this.RootBorder.Cursor = System.Windows.Input.Cursors.Arrow;
  }

  private void AutoScrollTimer_Tick(object sender, EventArgs e)
  {
    if (this.pageIdx == -1)
      return;
    double num1 = 0.0;
    double num2 = 0.0;
    Thickness thickness = new Thickness(16.0);
    bool flag1 = false;
    bool flag2 = false;
    if (this.curPoint.X < 50.0)
    {
      flag1 = true;
      num1 = -30.0 * Math.Max(Math.Min((50.0 - this.curPoint.X) / 50.0, 1.0), 0.0);
    }
    else if (this.curPoint.X > this.ActualWidth - 50.0)
      num1 = 30.0 * Math.Max(Math.Min((50.0 - (this.ActualWidth - this.curPoint.X)) / 50.0, 1.0), 0.0);
    if (this.curPoint.Y < 50.0)
    {
      flag2 = true;
      num2 = -30.0 * Math.Max(Math.Min((50.0 - this.curPoint.Y) / 50.0, 1.0), 0.0);
    }
    else if (this.curPoint.Y > this.ActualHeight - 50.0)
      num2 = 30.0 * Math.Max(Math.Min((50.0 - (this.ActualHeight - this.curPoint.Y)) / 50.0, 1.0), 0.0);
    if (num1 == 0.0 && num2 == 0.0)
      return;
    this.viewer.UpdateLayout();
    Rect clientRect;
    if (!this.viewer.TryGetClientRect(this.pageIdx, this.viewer.Document.Pages[this.pageIdx].GetEffectiveBox(), out clientRect))
      return;
    if (flag1)
    {
      if (clientRect.Left - thickness.Left <= 0.0)
      {
        if (num1 < clientRect.Left - thickness.Left)
          num1 = clientRect.Left - thickness.Left;
      }
      else
        num1 = 0.0;
    }
    else if (clientRect.Right + thickness.Right >= this.ActualWidth)
    {
      if (num1 > clientRect.Right + thickness.Right - this.ActualWidth)
        num1 = clientRect.Right + thickness.Right - this.ActualWidth;
    }
    else
      num1 = 0.0;
    if (flag2)
    {
      if (clientRect.Top - thickness.Top <= 0.0)
      {
        if (num2 < clientRect.Top - thickness.Top)
          num2 = clientRect.Top - thickness.Top;
      }
      else
        num2 = 0.0;
    }
    else if (clientRect.Bottom + thickness.Bottom >= this.ActualHeight)
    {
      if (num2 > clientRect.Bottom + thickness.Bottom - this.ActualHeight)
        num2 = clientRect.Bottom + thickness.Bottom - this.ActualHeight;
    }
    else
      num2 = 0.0;
    bool flag3 = false;
    if (Math.Abs(num1) > 1.0)
    {
      this.viewerSv.ScrollToHorizontalOffset(num1 + this.viewerSv.HorizontalOffset);
      flag3 = true;
    }
    if (Math.Abs(num2) > 1.0)
    {
      this.viewerSv.ScrollToVerticalOffset(num2 + this.viewerSv.VerticalOffset);
      flag3 = true;
    }
    if (!flag3)
      return;
    this.viewer.UpdateDocLayout();
  }

  private void UpdateToolbarPosition(Rect rect, bool isInput = false)
  {
    FrameworkElement element = (FrameworkElement) null;
    if (this.mode == ScreenshotDialogMode.Screenshot)
      element = (FrameworkElement) this.ImageToolbar;
    else if (this.mode == ScreenshotDialogMode.ExtractText)
      element = (FrameworkElement) this.ExtractTextToolbar;
    else if (this.mode == ScreenshotDialogMode.Ocr)
      element = (FrameworkElement) this.OcrToolbar;
    else if (this.mode == ScreenshotDialogMode.CropPage)
      element = (FrameworkElement) this.CropPageToolbar;
    if (this.pageIdx == -1 || element == null)
      return;
    double actualWidth = element.ActualWidth;
    double actualHeight = element.ActualHeight;
    double length1 = (rect.Right > this.ActualWidth ? this.ActualWidth : rect.Right) - actualWidth;
    if (length1 < 0.0)
      length1 = 0.0;
    Rect rect1 = new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight);
    double length2 = rect1.Bottom - rect.Bottom <= actualHeight ? (rect.Top - rect1.Top <= actualHeight ? (rect.Top - rect1.Top <= rect1.Bottom - rect.Bottom ? rect.Bottom - actualHeight : rect.Top) : rect.Top - actualHeight) : rect.Bottom;
    if (this.mode != ScreenshotDialogMode.CropPage)
    {
      Canvas.SetLeft((UIElement) element, length1);
      Canvas.SetTop((UIElement) element, length2);
    }
    else
    {
      PdfPage page = this.viewer.Document.Pages[this.pageIdx];
      FS_RECTF effectiveBox = page.GetEffectiveBox();
      float num1 = effectiveBox.Width;
      float num2 = effectiveBox.Height;
      if (page.Dictionary.ContainsKey("CropBox"))
      {
        num1 = page.CropBox.Height;
        num2 = page.CropBox.Width;
      }
      Canvas.SetLeft((UIElement) element, rect.Right);
      if (rect.Top < 0.0)
        Canvas.SetTop((UIElement) element, 0.0);
      else
        Canvas.SetTop((UIElement) element, rect.Top);
      ScreenshotCropPageToolbar screenshotCropPageToolbar = element as ScreenshotCropPageToolbar;
      screenshotCropPageToolbar.SelectionBorder.Visibility = Visibility.Hidden;
      if (rect.Right + screenshotCropPageToolbar.MenuBorder.ActualWidth + screenshotCropPageToolbar.SelectionBorder.ActualWidth > this.viewer.ActualWidth)
      {
        Canvas.SetLeft((UIElement) screenshotCropPageToolbar.SelectionBorder, -screenshotCropPageToolbar.SelectionBorder.ActualWidth);
        Canvas.SetLeft((UIElement) screenshotCropPageToolbar.PageRangeBorder, -screenshotCropPageToolbar.PageRangeBorder.ActualWidth);
      }
      else
      {
        Canvas.SetLeft((UIElement) screenshotCropPageToolbar.SelectionBorder, screenshotCropPageToolbar.MenuBorder.ActualWidth);
        Canvas.SetLeft((UIElement) screenshotCropPageToolbar.PageRangeBorder, screenshotCropPageToolbar.MenuBorder.ActualWidth);
      }
      float num3 = Math.Min(this.startPt.X, this.curPt.X);
      float num4 = Math.Min(this.startPt.Y, this.curPt.Y);
      float num5 = Math.Max(this.startPt.X, this.curPt.X);
      float num6 = Math.Max(this.startPt.Y, this.curPt.Y);
      float num7 = num6 - num4;
      this.CropPageToolbar.PageMargin = new MarginModel()
      {
        Left = (double) num3,
        Bottom = (double) num6 - (double) num7,
        Top = (double) num1 - (double) num6,
        Right = (double) num2 - (double) num5,
        PageWidth = (double) num2,
        PageHeight = (double) num1,
        Screenshot = this
      };
      this.CropPageToolbar.SelectionBorder.Visibility = isInput ? Visibility.Visible : Visibility.Hidden;
    }
  }

  private FS_POINTF GetCurrentPagePoint(System.Windows.Point point)
  {
    if (this.pageIdx == -1)
      return new FS_POINTF();
    try
    {
      System.Windows.Point pagePoint;
      if (this.viewer.TryGetPagePoint(this.pageIdx, point, out pagePoint))
      {
        FS_POINTF pdfPoint = pagePoint.ToPdfPoint();
        PdfPage page = this.viewer.Document.Pages[this.pageIdx];
        FS_RECTF fsRectf = page.GetEffectiveBox();
        if (this.mode == ScreenshotDialogMode.CropPage)
        {
          if (page.Dictionary.ContainsKey("CropBox"))
            fsRectf = page.CropBox;
          else if (page.Dictionary.ContainsKey("MediaBox"))
            fsRectf = page.MediaBox;
        }
        pdfPoint.X = Math.Max(fsRectf.left, Math.Min(pdfPoint.X, fsRectf.right));
        pdfPoint.Y = Math.Max(fsRectf.bottom, Math.Min(pdfPoint.Y, fsRectf.top));
        return pdfPoint;
      }
    }
    catch
    {
      try
      {
        CommomLib.Commom.GAManager.SendEvent("Exception", nameof (GetCurrentPagePoint), "Count", 1L);
      }
      catch
      {
      }
    }
    return new FS_POINTF();
  }

  public void UpdateBounds(bool isDragging, bool isInput = false)
  {
    if (this.pageIdx == -1)
      return;
    PdfPage page = this.viewer.Document.Pages[this.pageIdx];
    System.Windows.Point clientPoint1;
    System.Windows.Point clientPoint2;
    if (!this.viewer.TryGetClientPoint(this.pageIdx, this.startPt.ToPoint(), out clientPoint1) || !this.viewer.TryGetClientPoint(this.pageIdx, this.curPt.ToPoint(), out clientPoint2))
      return;
    Rect rect = new Rect(new System.Windows.Point(Math.Min(clientPoint1.X, clientPoint2.X), Math.Min(clientPoint1.Y, clientPoint2.Y)), new System.Windows.Point(Math.Max(clientPoint1.X, clientPoint2.X), Math.Max(clientPoint1.Y, clientPoint2.Y)));
    this.DraggerRectangle.Rect = rect;
    if (!isDragging && this.DragResizeView.Opacity > 0.0)
    {
      Canvas.SetLeft((UIElement) this.DragResizeView, rect.Left);
      Canvas.SetTop((UIElement) this.DragResizeView, rect.Top);
      this.DragResizeView.Width = rect.Width;
      this.DragResizeView.Height = rect.Height;
    }
    this.UpdateToolbarPosition(rect, isInput);
  }

  private void CloseBtn_Click(object sender, RoutedEventArgs e) => this.Close();

  private void DragResizeView_ResizeDragStarted(
    object sender,
    ResizeViewResizeDragStartedEventArgs e)
  {
    this.curRect = this.DraggerRectangle.Rect;
  }

  private void DragResizeView_ResizeDragging(object sender, ResizeViewResizeDragEventArgs e)
  {
    Rect rect;
    ref Rect local = ref rect;
    double x = this.curRect.Left + e.OffsetX;
    double y = this.curRect.Top + e.OffsetY;
    System.Windows.Size newSize = e.NewSize;
    double width = newSize.Width;
    newSize = e.NewSize;
    double height = newSize.Height;
    local = new Rect(x, y, width, height);
    System.Windows.Point clientPoint1 = new System.Windows.Point(rect.Left, rect.Top);
    System.Windows.Point clientPoint2 = new System.Windows.Point(rect.Right, rect.Bottom);
    System.Windows.Point pagePoint1;
    System.Windows.Point pagePoint2;
    if (!this.viewer.TryGetPagePoint(this.pageIdx, clientPoint1, out pagePoint1) || !this.viewer.TryGetPagePoint(this.pageIdx, clientPoint2, out pagePoint2))
      return;
    this.startPt = pagePoint1.ToPdfPoint();
    this.curPt = pagePoint2.ToPdfPoint();
    this.CropPageToolbar.SelectionBorderVisible = false;
    this.UpdateBounds(true);
  }

  private void DragResizeView_ResizeDragCompleted(object sender, ResizeViewResizeDragEventArgs e)
  {
    Rect rect;
    ref Rect local = ref rect;
    double x = this.curRect.Left + e.OffsetX;
    double y = this.curRect.Top + e.OffsetY;
    System.Windows.Size newSize = e.NewSize;
    double width = newSize.Width;
    newSize = e.NewSize;
    double height = newSize.Height;
    local = new Rect(x, y, width, height);
    System.Windows.Point clientPoint1 = new System.Windows.Point(rect.Left, rect.Top);
    System.Windows.Point clientPoint2 = new System.Windows.Point(rect.Right, rect.Bottom);
    System.Windows.Point pagePoint1;
    System.Windows.Point pagePoint2;
    if (!this.viewer.TryGetPagePoint(this.pageIdx, clientPoint1, out pagePoint1) || !this.viewer.TryGetPagePoint(this.pageIdx, clientPoint2, out pagePoint2))
      return;
    this.startPt = new FS_POINTF(Math.Min(pagePoint1.X, pagePoint2.X), Math.Max(pagePoint1.Y, pagePoint2.Y));
    this.curPt = new FS_POINTF(Math.Max(pagePoint1.X, pagePoint2.X), Math.Min(pagePoint1.Y, pagePoint2.Y));
    PdfPage page = this.viewer.Document.Pages[this.pageIdx];
    FS_RECTF fsRectf = page.GetEffectiveBox();
    if (this.mode == ScreenshotDialogMode.CropPage)
    {
      if (page.Dictionary.ContainsKey("CropBox"))
        fsRectf = page.CropBox;
      else if (page.Dictionary.ContainsKey("MediaBox"))
        fsRectf = page.MediaBox;
    }
    if ((double) this.startPt.X < (double) fsRectf.left)
      this.startPt.X = fsRectf.left;
    else if ((double) this.startPt.X > (double) fsRectf.right)
      this.startPt.X = Math.Max(fsRectf.right - 100f, 0.0f);
    if ((double) this.startPt.Y > (double) fsRectf.top)
      this.startPt.Y = fsRectf.top;
    else if ((double) this.startPt.Y < (double) fsRectf.bottom)
      this.startPt.Y = Math.Min(fsRectf.bottom, 100f);
    if ((double) this.curPt.X > (double) fsRectf.right)
      this.curPt.X = fsRectf.right;
    else if ((double) this.curPt.X < (double) fsRectf.left)
      this.curPt.X = Math.Min(fsRectf.left, this.startPt.X + 100f);
    if ((double) this.curPt.Y < (double) fsRectf.bottom)
      this.curPt.Y = fsRectf.bottom;
    else if ((double) this.curPt.Y > (double) fsRectf.top)
      this.curPt.Y = Math.Max(0.0f, fsRectf.top - 100f);
    this.UpdateBounds(false);
    this.curRect = Rect.Empty;
  }

  private async void DragResizeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
  {
    if (this.mode == ScreenshotDialogMode.ExtractText)
      await this.CompleteExtractTextAsync(false);
    else if (this.mode == ScreenshotDialogMode.Screenshot)
      await this.CompleteImageAsync();
    else if (this.mode == ScreenshotDialogMode.Ocr)
    {
      await this.CompleteExtractTextAsync(true);
    }
    else
    {
      if (this.mode != ScreenshotDialogMode.CropPage)
        return;
      this.CompleteCropPageAsync(this.CropPageToolbar.ApplypageIndex);
    }
  }

  private void UpdateToolbarVisibility(bool value)
  {
    if (value)
    {
      FrameworkElement frameworkElement = (FrameworkElement) null;
      if (this.mode == ScreenshotDialogMode.Screenshot)
        frameworkElement = (FrameworkElement) this.ImageToolbar;
      else if (this.mode == ScreenshotDialogMode.ExtractText)
        frameworkElement = (FrameworkElement) this.ExtractTextToolbar;
      else if (this.mode == ScreenshotDialogMode.Ocr)
        frameworkElement = (FrameworkElement) this.OcrToolbar;
      else if (this.mode == ScreenshotDialogMode.CropPage)
        frameworkElement = (FrameworkElement) this.CropPageToolbar;
      if (frameworkElement == null)
        return;
      frameworkElement.IsEnabled = true;
      frameworkElement.IsHitTestVisible = true;
      frameworkElement.Opacity = 1.0;
    }
    else
    {
      this.ImageToolbar.IsEnabled = false;
      this.ImageToolbar.IsHitTestVisible = false;
      this.ImageToolbar.Opacity = 0.0;
      this.ExtractTextToolbar.IsEnabled = false;
      this.ExtractTextToolbar.IsHitTestVisible = false;
      this.ExtractTextToolbar.Opacity = 0.0;
      this.OcrToolbar.IsEnabled = false;
      this.OcrToolbar.IsHitTestVisible = false;
      this.OcrToolbar.Opacity = 0.0;
      this.CropPageToolbar.IsEnabled = false;
      this.CropPageToolbar.IsHitTestVisible = false;
      this.CropPageToolbar.Opacity = 0.0;
    }
  }

  public void Cancel()
  {
    if (this.DragResizeView.Opacity != 0.0)
    {
      this.CancelCore();
    }
    else
    {
      if (this.mode == ScreenshotDialogMode.CropPage)
        this.ResetCropSize(this.pageIdx);
      this.Close();
    }
  }

  public async Task CompleteExtractTextAsync(bool ocr)
  {
    if (this.pageIdx != -1 && this.startPt != new FS_POINTF() && this.curPt != new FS_POINTF())
    {
      PdfDocument document = this.viewer?.Document;
      if (document != null)
      {
        WriteableBitmap bitmap = await this.GetScaledPageImageAsync();
        PdfPage page = document.Pages[this.pageIdx];
        WriteableBitmap rotatedImage = await this.RotateImageAsync(bitmap, page.Rotation);
        FS_POINTF startPt = this.startPt;
        FS_POINTF curPt = this.curPt;
        FS_RECTF fsRectf = new FS_RECTF(Math.Min(startPt.X, curPt.X), Math.Max(startPt.Y, curPt.Y), Math.Max(startPt.X, curPt.X), Math.Min(startPt.Y, curPt.Y));
        string boundedText = page.Text.GetBoundedText(fsRectf);
        Rect clientRect;
        if (this.viewer.TryGetClientRect(this.pageIdx, fsRectf, out clientRect))
        {
          this.Close(ScreenshotDialogResult.CreateExtractedText(this.pageIdx, boundedText, bitmap, rotatedImage, fsRectf, clientRect, ocr));
          return;
        }
        bitmap = (WriteableBitmap) null;
        page = (PdfPage) null;
      }
      document = (PdfDocument) null;
    }
    this.Close();
  }

  public async Task CompleteImageAsync()
  {
    WriteableBitmap bitmap;
    if (this.pageIdx < 0)
    {
      bitmap = (WriteableBitmap) null;
    }
    else
    {
      bitmap = await this.GetScaledPageImageAsync();
      PdfPage page = this.viewer?.Document.Pages[this.pageIdx];
      WriteableBitmap rotatedImage = this.AttachDrawControl2FinalImage(await this.RotateImageAsync(bitmap, page.Rotation));
      if (rotatedImage != null)
      {
        FS_POINTF startPt = this.startPt;
        FS_POINTF curPt = this.curPt;
        FS_RECTF fsRectf = new FS_RECTF(Math.Min(startPt.X, curPt.X), Math.Max(startPt.Y, curPt.Y), Math.Max(startPt.X, curPt.X), Math.Min(startPt.Y, curPt.Y));
        Rect clientRect;
        if (this.viewer.TryGetClientRect(this.pageIdx, fsRectf, out clientRect))
        {
          try
          {
            System.Windows.Forms.Clipboard.SetImage(this.BitmapSourceToImage((BitmapSource) rotatedImage));
          }
          catch
          {
          }
          this.Close(ScreenshotDialogResult.CreateImage(this.pageIdx, bitmap, rotatedImage, fsRectf, clientRect));
          bitmap = (WriteableBitmap) null;
          return;
        }
      }
      this.Close();
      bitmap = (WriteableBitmap) null;
    }
  }

  public async Task CopyToClipboardAsync()
  {
    WriteableBitmap writeableBitmap = this.AttachDrawControl2FinalImage(await this.RotateImageAsync(await this.GetScaledPageImageAsync(), this.viewer?.Document.Pages[this.pageIdx].Rotation));
    if (writeableBitmap != null)
    {
      try
      {
        System.Windows.Forms.Clipboard.SetImage(this.BitmapSourceToImage((BitmapSource) writeableBitmap));
      }
      catch (Exception ex)
      {
      }
    }
    this.Close();
  }

  public async Task EditImageAsync()
  {
    WriteableBitmap source = await this.RotateImageAsync(await this.GetScaledPageImageAsync(), this.viewer?.Document.Pages[this.pageIdx].Rotation);
    if (source != null)
    {
      string temporaryPath = UtilManager.GetTemporaryPath();
      string path;
      do
      {
        string path2 = Guid.NewGuid().ToString("N").ToUpperInvariant() + ".png";
        path = System.IO.Path.Combine(temporaryPath, path2);
      }
      while (File.Exists(path));
      using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
      {
        PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
        pngBitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapSource) source));
        pngBitmapEncoder.Save((Stream) fileStream);
      }
      try
      {
        Process.Start("mspaint", $"\"{path}\"");
      }
      catch (Exception ex)
      {
        CommomLib.Commom.Log.WriteLog(ex.ToString());
      }
    }
    this.Close();
  }

  public async Task SaveImageAsync()
  {
    WriteableBitmap source = this.AttachDrawControl2FinalImage(await this.RotateImageAsync(await this.GetScaledPageImageAsync(), this.viewer?.Document.Pages[this.pageIdx].Rotation));
    if (source == null)
      return;
    Microsoft.Win32.SaveFileDialog saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
    saveFileDialog1.AddExtension = true;
    saveFileDialog1.Filter = "png|*.png";
    saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    saveFileDialog1.FileName = "PdfImage.png";
    Microsoft.Win32.SaveFileDialog saveFileDialog2 = saveFileDialog1;
    if (!saveFileDialog2.ShowDialog().GetValueOrDefault())
      return;
    string fileName = saveFileDialog2.FileName;
    try
    {
      if (File.Exists(fileName))
      {
        try
        {
          File.Delete(fileName);
        }
        catch
        {
          this.Close();
          return;
        }
      }
      using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
      {
        PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
        pngBitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapSource) source));
        pngBitmapEncoder.Save((Stream) fileStream);
      }
      int num = await ExplorerUtils.SelectItemInExplorerAsync(fileName, new CancellationToken()) ? 1 : 0;
      this.Close();
    }
    catch
    {
    }
  }

  private async Task<WriteableBitmap> RotateImageAsync(WriteableBitmap source, PageRotate rotate)
  {
    ScreenshotDialog screenshotDialog = this;
    if (source == null || rotate == PageRotate.Normal)
      return source;
    Rotation rotation = Rotation.Rotate0;
    switch (rotate)
    {
      case PageRotate.Rotate90:
        rotation = Rotation.Rotate90;
        break;
      case PageRotate.Rotate180:
        rotation = Rotation.Rotate180;
        break;
      case PageRotate.Rotate270:
        rotation = Rotation.Rotate270;
        break;
      default:
        rotation = Rotation.Rotate0;
        break;
    }
    BitmapSource result = (BitmapSource) null;
    await screenshotDialog.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) (() =>
    {
      using (Bitmap bitmap = new Bitmap(source.PixelWidth, source.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
      {
        BitmapData bitmapdata = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, source.PixelWidth, source.PixelHeight), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        source.CopyPixels(new Int32Rect(0, 0, source.PixelWidth, source.PixelHeight), bitmapdata.Scan0, bitmapdata.Stride * bitmapdata.Height, bitmapdata.Stride);
        bitmap.UnlockBits(bitmapdata);
        BitmapSizeOptions sizeOptions = BitmapSizeOptions.FromRotation(rotation);
        Int32Rect sourceRect = new Int32Rect(0, 0, bitmap.Width, bitmap.Height);
        IntPtr hbitmap = bitmap.GetHbitmap();
        try
        {
          result = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, sourceRect, sizeOptions);
        }
        finally
        {
          try
          {
            if (hbitmap != IntPtr.Zero)
              DrawUtils.DeleteObject(hbitmap);
          }
          catch
          {
          }
        }
      }
    }));
    return result == null ? (WriteableBitmap) null : new WriteableBitmap(result);
  }

  private async Task<WriteableBitmap> GetScaledPageImageAsync()
  {
    PdfDocument document = this.viewer?.Document;
    if (document == null)
      return (WriteableBitmap) null;
    FS_POINTF startPt = this.startPt;
    FS_POINTF curPt = this.curPt;
    FS_RECTF pageRect = new FS_RECTF(Math.Min(startPt.X, curPt.X), Math.Max(startPt.Y, curPt.Y), Math.Max(startPt.X, curPt.X), Math.Min(startPt.Y, curPt.Y));
    Rect clientRect;
    if (!this.viewer.TryGetClientRect(this.pageIdx, pageRect, out clientRect))
      return (WriteableBitmap) null;
    PdfPage page = document.Pages[this.pageIdx];
    FS_SIZEF effectiveSize = page.GetEffectiveSize();
    float num1 = (float) ((double) effectiveSize.Width * 96.0 / 72.0 * 2.0);
    float num2 = pageRect.Width * num1 / effectiveSize.Width;
    double width = clientRect.Width;
    double height = clientRect.Height;
    if (page.Rotation == PageRotate.Rotate90 || page.Rotation == PageRotate.Rotate270)
    {
      double num3 = height;
      height = width;
      width = num3;
    }
    if (width < (double) num2)
    {
      double num4 = (double) num2 / width;
      width *= num4;
      height *= num4;
    }
    return await ScreenshotDialog.GetPageImageAsync(width, height, page, new FS_RECTF?(pageRect));
  }

  public void CompleteCropPageAsync(int[] pageIndexs = null)
  {
    FS_POINTF startPt = this.startPt;
    FS_POINTF curPt = this.curPt;
    FS_RECTF fsRectf = new FS_RECTF(Math.Abs(Math.Min(startPt.X, curPt.X)), Math.Abs(Math.Max(startPt.Y, curPt.Y)), Math.Abs(Math.Max(startPt.X, curPt.X)), Math.Abs(Math.Min(startPt.Y, curPt.Y)));
    if (PageHeaderFooterUtils.PdfPointToCm((double) Math.Abs(fsRectf.Width)) < 0.15 || PageHeaderFooterUtils.PdfPointToCm((double) Math.Abs(fsRectf.Height)) < 0.15)
    {
      int num = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.MainCropPageSelectLittleAreaNote, UtilManager.GetProductName());
    }
    else
    {
      Rect clientRect;
      if (this.viewer.TryGetClientRect(this.pageIdx, fsRectf, out clientRect))
      {
        if (pageIndexs == null)
          pageIndexs = new int[1]{ this.pageIdx };
        FS_RECTF? beforeCropBox = this.BeforeCropBox;
        this.Close(ScreenshotDialogResult.GetCropBox(this.pageIdx, pageIndexs, beforeCropBox.Value, fsRectf, clientRect));
      }
      else
        this.Close();
    }
  }

  public static async Task<WriteableBitmap> GetPageImageAsync(
    double width,
    double height,
    PdfPage page,
    FS_RECTF? pageRect = null)
  {
    FS_SIZEF effectiveSize = page.GetEffectiveSize();
    FS_RECTF effectiveBox = PdfLocationUtils.MediaBoxRectToEffectiveBox(page, pageRect ?? page.GetEffectiveBox());
    using (PdfBitmap pdfBitmap = new PdfBitmap((int) width, (int) height, BitmapFormats.FXDIB_Argb))
    {
      if (pdfBitmap == null)
        throw new ArgumentNullException("pdfBitmap");
      if (page == null)
        throw new ArgumentNullException(nameof (page));
      int width1 = pdfBitmap.Width;
      int height1 = pdfBitmap.Height;
      try
      {
        pdfBitmap.FillRectEx(0, 0, width1, height1, -1);
        double num1 = width / (double) effectiveBox.Width;
        double num2 = height / (double) effectiveBox.Height;
        double size_x = (double) effectiveSize.Width * num1;
        double size_y = (double) effectiveSize.Height * num2;
        double start_x = -(double) effectiveBox.left * num1;
        double start_y = -((double) effectiveSize.Height - (double) effectiveBox.top) * num2;
        PDFKit.PdfControl viewer = PDFKit.PdfControl.GetPdfControl(page.Document);
        bool showAnnot = true;
        if (viewer != null)
        {
          Action callback = (Action) (() => showAnnot = viewer.IsAnnotationVisible);
          if (viewer.Dispatcher.CheckAccess())
            callback();
          else
            viewer.Dispatcher.Invoke(callback);
        }
        IntPtr page1 = Pdfium.FPDF_LoadPage(page.Document.Handle, page.PageIndex);
        try
        {
          if (page1 != IntPtr.Zero)
          {
            int rotate = -(int) Pdfium.FPDFPage_GetRotation(page1);
            if (rotate < 0)
              rotate += 4;
            Pdfium.FPDF_RenderPageBitmap(pdfBitmap.Handle, page1, (int) start_x, (int) start_y, (int) size_x, (int) size_y, (PageRotate) rotate, showAnnot ? RenderFlags.FPDF_ANNOT : RenderFlags.FPDF_NONE);
          }
        }
        finally
        {
          try
          {
            if (page1 != IntPtr.Zero)
              Pdfium.FPDF_ClosePage(page1);
          }
          catch
          {
          }
        }
        return await pdfBitmap.ToWriteableBitmapAsync(new CancellationToken());
      }
      catch (Exception ex) when (!(ex is OperationCanceledException))
      {
      }
      return (WriteableBitmap) null;
    }
  }

  public void RenderFirstCropBox(int pageIdx)
  {
    if (pageIdx == -1)
      return;
    PdfPage page = this.viewer.Document.Pages[pageIdx];
    this.BeforeCropBox = new FS_RECTF?(page.GetEffectiveBox());
    page.CropBox = page.MediaBox;
    page.TrimBox = page.MediaBox;
    this.VM.ViewToolbar.DocSizeMode = SizeModes.Zoom;
    this.VM.ViewToolbar.DocZoom = 1f;
    this.viewer.UpdateLayout();
    this.viewer.UpdateDocLayout();
    this.viewer.TryRedrawVisiblePageAsync();
  }

  public void ResetCropSize(int currentpageIdx)
  {
    if (!this.BeforeCropBox.HasValue)
      return;
    PdfPage page = this.viewer.Document.Pages[currentpageIdx];
    page.CropBox = this.BeforeCropBox.Value;
    page.TrimBox = this.BeforeCropBox.Value;
    this.viewer.UpdateLayout();
    this.viewer.UpdateDocLayout();
    this.viewer.TryRedrawVisiblePageAsync();
    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() => this.viewer.CurrentIndex = currentpageIdx));
    this.viewer.CurrentIndex = page.PageIndex;
    this.VM.ViewToolbar.DocSizeMode = SizeModes.Zoom;
    this.VM.ViewToolbar.DocZoom = 1f;
  }

  private void SetCropRect(PdfPage page, FS_RECTF rect)
  {
    if (!page.Dictionary.ContainsKey("CropRect"))
    {
      PdfTypeDictionary pdfTypeDictionary = PdfTypeDictionary.Create();
      pdfTypeDictionary.SetRectAt("CropRect", rect);
      page.Dictionary["CropRect"] = (PdfTypeBase) pdfTypeDictionary;
    }
    else
    {
      PdfTypeDictionary pdfTypeDictionary = page.Dictionary["CropRect"] as PdfTypeDictionary;
      if (!pdfTypeDictionary.ContainsKey("CropRect"))
        return;
      pdfTypeDictionary.SetRectAt("CropRect", rect);
    }
  }

  public event PropertyChangedEventHandler PropertyChanged;

  private void RaisePropertyChanged(string propertyName)
  {
    PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
    if (propertyChanged == null)
      return;
    propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }

  internal void SetDrawControlBrush(UIElement drawControl, System.Windows.Media.Brush brush, bool isAddUndoStack = false)
  {
    if (drawControl == null)
      return;
    System.Windows.Media.Brush originalBrush = (System.Windows.Media.Brush) null;
    if (drawControl is Border border)
    {
      if (border.Child is System.Windows.Controls.TextBox child)
      {
        originalBrush = child.Foreground;
        child.Foreground = brush;
      }
      else
      {
        originalBrush = border.BorderBrush;
        border.BorderBrush = brush;
      }
    }
    else if (drawControl is Ellipse ellipse)
    {
      originalBrush = ellipse.Stroke;
      ellipse.Stroke = brush;
    }
    else if (drawControl is Polyline polyline)
    {
      originalBrush = polyline.Stroke;
      polyline.Stroke = brush;
    }
    else if (drawControl is System.Windows.Controls.Control control)
    {
      originalBrush = control.Background;
      control.Background = brush;
    }
    if (!isAddUndoStack || !(originalBrush.ToString() != brush.ToString()))
      return;
    this.undoDrawControlStack.Push((DrawOperation) new ChangeColorOperation(drawControl, originalBrush));
  }

  internal void SetDrawControlThickness(
    UIElement drawControl,
    double thickness,
    bool isAddUndoStack = false)
  {
    if (drawControl == null)
      return;
    double originalThickness = 0.0;
    if (drawControl is Border border)
    {
      originalThickness = border.BorderThickness.Left;
      border.BorderThickness = new Thickness(thickness);
    }
    else if (drawControl is Ellipse ellipse)
    {
      originalThickness = ellipse.StrokeThickness;
      ellipse.StrokeThickness = thickness;
    }
    else if (drawControl is Polyline polyline)
    {
      originalThickness = polyline.StrokeThickness;
      polyline.StrokeThickness = thickness;
    }
    else if (drawControl is System.Windows.Controls.Control control)
    {
      originalThickness = this.ArrowHeight2Thickness(control.Height);
      control.Height = this.Thickness2ArrowHeight(thickness);
    }
    if (!isAddUndoStack || originalThickness == thickness)
      return;
    this.undoDrawControlStack.Push((DrawOperation) new ChangeThicknessOperation(drawControl, originalThickness));
  }

  internal void SetDrawTextFontSize(UIElement drawControl, double fontSize, bool isAddUndoStack = false)
  {
    if (!(drawControl is Border border) || !(border.Child is System.Windows.Controls.TextBox child))
      return;
    double fontSize1 = child.FontSize;
    child.FontSize = fontSize;
    if (!isAddUndoStack || fontSize1 == fontSize)
      return;
    this.undoDrawControlStack.Push((DrawOperation) new ChangeFontSizeOperation(drawControl, fontSize1));
  }

  private void RemoveSelected()
  {
    UIElement selectedDrawControl = this.SelectedDrawControl;
    if (selectedDrawControl != null)
      selectedDrawControl.UnSelect();
    this.SelectedDrawControl = (UIElement) null;
  }

  private bool CheckPointInDraggerCanvas(System.Windows.Point point)
  {
    return point.X >= 0.0 && point.X <= this.DragResizeView.Width && point.Y >= 0.0 && point.Y <= this.DragResizeView.Height;
  }

  private void DrawText()
  {
    double num1 = this.startDrawPoint.X + 40.0;
    if (this.ControlTextBox != null)
      return;
    Border border = new Border();
    border.BorderBrush = (System.Windows.Media.Brush) System.Windows.Application.Current.FindResource((object) "DottedLineDrawingBrush");
    border.BorderThickness = new Thickness(1.0);
    border.SnapsToDevicePixels = true;
    border.UseLayoutRounding = true;
    this.ControlTextBox = border;
    System.Windows.Controls.TextBox textBox = new System.Windows.Controls.TextBox();
    textBox.Style = (Style) null;
    textBox.Background = (System.Windows.Media.Brush) null;
    textBox.BorderThickness = new Thickness(0.0);
    textBox.Foreground = this.CurrentBrush;
    textBox.FontSize = this.CurrentFontSize;
    textBox.TextWrapping = TextWrapping.Wrap;
    textBox.FontWeight = FontWeights.Normal;
    textBox.MinWidth = 40.0;
    textBox.MaxWidth = this.DragResizeView.Width - this.startDrawPoint.X;
    textBox.MaxHeight = this.DragResizeView.Height - 4.0;
    textBox.Cursor = System.Windows.Input.Cursors.SizeAll;
    textBox.Padding = new Thickness(4.0);
    textBox.AcceptsReturn = true;
    textBox.LostFocus += (RoutedEventHandler) ((s, e1) =>
    {
      System.Windows.Controls.TextBox reference = s as System.Windows.Controls.TextBox;
      DependencyObject parent = VisualTreeHelper.GetParent((DependencyObject) reference);
      if (parent == null || !(parent is Border element2))
        return;
      element2.BorderThickness = new Thickness(0.0);
      if (!string.IsNullOrWhiteSpace(reference.Text))
        return;
      this.DragResizeView.RemoveDrawControl((UIElement) element2);
    });
    this.ControlTextBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.DrawControl_MouseLeftButtonDown);
    this.ControlTextBox.MouseMove += new System.Windows.Input.MouseEventHandler(this.DrawControl_MouseMove);
    this.ControlTextBox.MouseLeftButtonUp += new MouseButtonEventHandler(this.DrawControl_MouseLeftButtonUp);
    this.ControlTextBox.SizeChanged += (SizeChangedEventHandler) ((s, e1) =>
    {
      Border element3 = s as Border;
      double y = this.startDrawPoint.Y;
      if (y + element3.ActualHeight <= this.DragResizeView.Height)
        return;
      double num2 = Math.Abs(this.DragResizeView.Height - (y + element3.ActualHeight));
      this.startDrawPoint.Y = y - num2;
      Canvas.SetTop((UIElement) element3, this.startDrawPoint.Y + 2.0);
    });
    this.ControlTextBox.Child = (UIElement) textBox;
    this.DragResizeView.AddUIElementToCanvas((UIElement) this.ControlTextBox);
    this.undoDrawControlStack.Push((DrawOperation) new DrawControlOperation(OperationType.DrawText, (UIElement) this.ControlTextBox));
    textBox.Focus();
    double x = this.startDrawPoint.X;
    if (num1 > this.DragResizeView.Width)
      x -= num1 - this.DragResizeView.Width;
    Canvas.SetLeft((UIElement) this.ControlTextBox, x - 2.0);
    Canvas.SetTop((UIElement) this.ControlTextBox, this.startDrawPoint.Y - 2.0);
    this.SelectedDrawControl = (UIElement) this.ControlTextBox;
  }

  private void DrawControl(System.Windows.Point current, DrawControlMode drawControlMode)
  {
    switch (drawControlMode)
    {
      case DrawControlMode.DrawRectangle:
        Rect rect1 = new Rect(this.startDrawPoint, current);
        if (this.ControlRectangle == null)
        {
          Border border = new Border();
          border.BorderBrush = this.CurrentBrush;
          border.BorderThickness = new Thickness(this.CurrentThickness);
          border.CornerRadius = new CornerRadius(3.0);
          border.Cursor = System.Windows.Input.Cursors.SizeAll;
          this.ControlRectangle = border;
          this.ControlRectangle.MouseLeftButtonDown += new MouseButtonEventHandler(this.DrawControl_MouseLeftButtonDown);
          this.ControlRectangle.MouseMove += new System.Windows.Input.MouseEventHandler(this.DrawControl_MouseMove);
          this.ControlRectangle.MouseLeftButtonUp += new MouseButtonEventHandler(this.DrawControl_MouseLeftButtonUp);
          this.DragResizeView.AddUIElementToCanvas((UIElement) this.ControlRectangle);
        }
        this.ControlRectangle.Width = rect1.Width;
        this.ControlRectangle.Height = rect1.Height;
        Canvas.SetLeft((UIElement) this.ControlRectangle, rect1.Left);
        Canvas.SetTop((UIElement) this.ControlRectangle, rect1.Top);
        this.controlLocation = new Rect(rect1.Left, rect1.Top, rect1.Width, rect1.Height);
        break;
      case DrawControlMode.DrawCircle:
        Rect rect2 = new Rect(this.startDrawPoint, current);
        if (this.ControlCircle == null)
        {
          Ellipse ellipse = new Ellipse();
          ellipse.Stroke = this.CurrentBrush;
          ellipse.StrokeThickness = this.CurrentThickness;
          ellipse.Cursor = System.Windows.Input.Cursors.SizeAll;
          this.ControlCircle = ellipse;
          this.ControlCircle.MouseLeftButtonDown += new MouseButtonEventHandler(this.DrawControl_MouseLeftButtonDown);
          this.ControlCircle.MouseMove += new System.Windows.Input.MouseEventHandler(this.DrawControl_MouseMove);
          this.ControlCircle.MouseLeftButtonUp += new MouseButtonEventHandler(this.DrawControl_MouseLeftButtonUp);
          this.DragResizeView.AddUIElementToCanvas((UIElement) this.ControlCircle);
        }
        this.ControlCircle.Width = rect2.Width;
        this.ControlCircle.Height = rect2.Height;
        Canvas.SetLeft((UIElement) this.ControlCircle, rect2.Left);
        Canvas.SetTop((UIElement) this.ControlCircle, rect2.Top);
        this.controlLocation = new Rect(rect2.Left, rect2.Top, rect2.Width, rect2.Height);
        break;
      case DrawControlMode.DrawArrow:
        Rect rect3 = new Rect(this.startDrawPoint, current);
        if (this.ControlArrow == null)
        {
          System.Windows.Controls.Control control = new System.Windows.Controls.Control();
          control.Background = this.CurrentBrush;
          control.BorderThickness = new Thickness(this.CurrentThickness);
          control.Template = this.controlArrowTemplate;
          control.Cursor = System.Windows.Input.Cursors.SizeAll;
          this.ControlArrow = control;
          this.ControlArrow.MouseLeftButtonDown += new MouseButtonEventHandler(this.DrawControl_MouseLeftButtonDown);
          this.ControlArrow.MouseMove += new System.Windows.Input.MouseEventHandler(this.DrawControl_MouseMove);
          this.ControlArrow.MouseLeftButtonUp += new MouseButtonEventHandler(this.DrawControl_MouseLeftButtonUp);
          this.ControlArrow.Height = this.Thickness2ArrowHeight(this.CurrentThickness);
          this.DragResizeView.AddUIElementToCanvas((UIElement) this.ControlArrow);
          Canvas.SetLeft((UIElement) this.ControlArrow, rect3.Left);
          Canvas.SetTop((UIElement) this.ControlArrow, rect3.Top - this.ControlArrow.Height / 2.0);
        }
        TransformGroup transformGroup = new TransformGroup();
        RotateTransform rotateTransform = new RotateTransform();
        transformGroup.Children.Add((Transform) rotateTransform);
        this.ControlArrow.RenderTransformOrigin = new System.Windows.Point(0.0, 0.5);
        this.ControlArrow.RenderTransform = (Transform) transformGroup;
        rotateTransform.Angle = ScreenshotDialog.CalculeAngle(this.startDrawPoint, current);
        double x1 = current.X - this.startDrawPoint.X;
        double x2 = current.Y - this.startDrawPoint.Y;
        double num = Math.Sqrt(Math.Pow(x1, 2.0) + Math.Pow(x2, 2.0));
        this.ControlArrow.Width = num < 15.0 ? 15.0 : num;
        this.controlLocation = new Rect(rect3.Left, rect3.Top - this.ControlArrow.Height / 2.0, this.ControlArrow.Width, this.ControlArrow.Height);
        break;
      case DrawControlMode.DrawInk:
        if (this.ControlInk == null)
        {
          Polyline polyline = new Polyline();
          polyline.Stroke = this.CurrentBrush;
          polyline.Cursor = System.Windows.Input.Cursors.SizeAll;
          polyline.StrokeThickness = this.CurrentThickness;
          polyline.StrokeLineJoin = PenLineJoin.Round;
          polyline.StrokeStartLineCap = PenLineCap.Round;
          polyline.StrokeEndLineCap = PenLineCap.Round;
          this.ControlInk = polyline;
          this.ControlInk.MouseLeftButtonDown += new MouseButtonEventHandler(this.DrawControl_MouseLeftButtonDown);
          this.ControlInk.MouseMove += new System.Windows.Input.MouseEventHandler(this.DrawControl_MouseMove);
          this.ControlInk.MouseLeftButtonUp += new MouseButtonEventHandler(this.DrawControl_MouseLeftButtonUp);
          this.DragResizeView.AddUIElementToCanvas((UIElement) this.ControlInk);
        }
        this.ControlInk.Points.Add(current);
        break;
    }
  }

  private void DrawControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (this.SelectedDrawControl is Border selectedDrawControl && selectedDrawControl.Child is System.Windows.Controls.TextBox child1)
    {
      if (child1.Text != this.controlTextBoxText && !string.IsNullOrEmpty(this.controlTextBoxText))
        this.undoDrawControlStack.Push((DrawOperation) new ChangeTextBoxTextOperation(this.SelectedDrawControl, this.controlTextBoxText));
      this.controlTextBoxText = (string) null;
    }
    this.RemoveSelected();
    this.SelectedDrawControl = (UIElement) sender;
    if (this.SelectedDrawControl == null)
      return;
    bool flag = false;
    switch (sender)
    {
      case Border border:
        if (border.Child == null)
        {
          this.DrawControlMode = DrawControlMode.DrawRectangle;
          this.CurrentBrush = border.BorderBrush;
          this.CurrentThickness = border.BorderThickness.Left;
          break;
        }
        if (border.Child is System.Windows.Controls.TextBox child2)
        {
          this.DrawControlMode = DrawControlMode.DrawText;
          this.CurrentBrush = child2.Foreground;
          this.CurrentFontSize = child2.FontSize;
          this.controlTextBoxText = child2.Text;
          Keyboard.Focus((IInputElement) child2);
          flag = true;
          break;
        }
        break;
      case Ellipse ellipse:
        this.DrawControlMode = DrawControlMode.DrawCircle;
        this.CurrentBrush = ellipse.Stroke;
        this.CurrentThickness = ellipse.StrokeThickness;
        break;
      case Polyline polyline:
        this.DrawControlMode = DrawControlMode.DrawInk;
        this.CurrentBrush = polyline.Stroke;
        this.CurrentThickness = polyline.StrokeThickness;
        break;
      case System.Windows.Controls.Control control:
        this.DrawControlMode = DrawControlMode.DrawArrow;
        this.CurrentBrush = control.Background;
        this.CurrentThickness = this.ArrowHeight2Thickness(control.Height);
        break;
    }
    this.RaisePropertyChanged("CurrentBrush");
    if (!flag)
      this.RaisePropertyChanged("CurrentThickness");
    else
      this.RaisePropertyChanged("CurrentFontSize");
    this.SelectedDrawControl.Select();
    this.dragStartPoint = Mouse.GetPosition((IInputElement) this.DragResizeView.GetDraggerCanvas());
    if (this.selectedDrawControl.RenderTransform is TransformGroup renderTransform && renderTransform.Children.Count > 0)
    {
      this.orignalTransformGroup = renderTransform.Clone();
      this.totalTranslate = !(sender is System.Windows.Controls.Control) || renderTransform.Children.Count != 1 ? (TranslateTransform) renderTransform.Children.Last<Transform>() : new TranslateTransform();
    }
    else
    {
      this.orignalTransformGroup = new TransformGroup();
      this.totalTranslate = new TranslateTransform();
    }
    this.tempTranslate = new TranslateTransform();
    this.SelectedDrawControl.CaptureMouse();
    e.Handled = true;
  }

  private void DrawControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
  {
    if (e.LeftButton != MouseButtonState.Pressed || this.SelectedDrawControl == null)
      return;
    this.isDragDrawControl = true;
    System.Windows.Point position = Mouse.GetPosition((IInputElement) this.DragResizeView.GetDraggerCanvas());
    this.KeepPointInDraggerCanvas(ref position);
    double num1 = position.X - this.dragStartPoint.X;
    double num2 = position.Y - this.dragStartPoint.Y;
    this.tempTranslate.X = this.totalTranslate.X + num1;
    this.tempTranslate.Y = this.totalTranslate.Y + num2;
    TransformGroup transformGroup = new TransformGroup();
    if (this.SelectedDrawControl is System.Windows.Controls.Control && this.selectedDrawControl.RenderTransform is TransformGroup renderTransform && renderTransform.Children.Count > 0)
    {
      foreach (Transform child in renderTransform.Children)
      {
        if (child is RotateTransform rotateTransform)
          transformGroup.Children.Add((Transform) rotateTransform);
      }
    }
    transformGroup.Children.Add((Transform) this.tempTranslate);
    this.SelectedDrawControl.RenderTransform = (Transform) transformGroup;
    e.Handled = true;
  }

  private void DrawControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    this.SelectedDrawControl?.ReleaseMouseCapture();
    if (!this.isDragDrawControl)
      return;
    this.isDragDrawControl = false;
    System.Windows.Point position = e.GetPosition((IInputElement) this.DragResizeView.GetDraggerCanvas());
    if (position.Equals(this.dragStartPoint))
      return;
    this.totalTranslate.X += position.X - this.dragStartPoint.X;
    this.totalTranslate.Y += position.Y - this.dragStartPoint.Y;
    this.undoDrawControlStack.Push((DrawOperation) new MoveControlOperation(this.SelectedDrawControl, this.orignalTransformGroup));
  }

  private void Viewer_ZoomChanged(object sender, EventArgs e)
  {
    if (this.pageIdx == -1)
      return;
    Canvas draggerCanvas = this.DragResizeView.GetDraggerCanvas();
    double zoomFactor = (double) this.viewer.Zoom / (double) this.curZoomFactor;
    for (int index = 1; index < draggerCanvas.Children.Count; ++index)
      draggerCanvas.Children[index].Zoom(zoomFactor);
    foreach (DrawOperation undoDrawControl in this.undoDrawControlStack)
    {
      if (undoDrawControl is MoveControlOperation controlOperation && controlOperation.OriginalTransformGroup != null)
      {
        foreach (Transform child in controlOperation.OriginalTransformGroup.Children)
        {
          if (!(child is RotateTransform))
          {
            TranslateTransform translateTransform = (TranslateTransform) child;
            translateTransform.X *= zoomFactor;
            translateTransform.Y *= zoomFactor;
          }
        }
      }
    }
    this.curZoomFactor = this.viewer.Zoom;
  }

  private double ArrowHeight2Thickness(double arrowHeight)
  {
    int index = Array.IndexOf<double>(DrawSettingConstants.ArrowHeight, arrowHeight);
    if (index < 0 || index > ((IEnumerable<double>) DrawSettingConstants.Thicknesses).Count<double>() - 1)
      index = 0;
    return DrawSettingConstants.Thicknesses[index];
  }

  private void KeepPointInDraggerCanvas(ref System.Windows.Point point)
  {
    if (point.X < 0.0)
      point.X = 0.0;
    else if (point.X > this.DragResizeView.Width)
      point.X = this.DragResizeView.Width;
    if (point.Y < 0.0)
    {
      point.Y = 0.0;
    }
    else
    {
      if (point.Y <= this.DragResizeView.Height)
        return;
      point.Y = this.DragResizeView.Height;
    }
  }

  private static double CalculeAngle(System.Windows.Point start, System.Windows.Point end)
  {
    return (Math.Atan2(end.Y - start.Y, end.X - start.X) * (180.0 / Math.PI) + 360.0) % 360.0;
  }

  private double Thickness2ArrowHeight(double thickness)
  {
    int index = Array.IndexOf<double>(DrawSettingConstants.Thicknesses, thickness);
    if (index < 0 || index > ((IEnumerable<double>) DrawSettingConstants.ArrowHeight).Count<double>() - 1)
      index = 0;
    return DrawSettingConstants.ArrowHeight[index];
  }

  private void ClearDraw()
  {
    this.startDrawPoint = new System.Windows.Point(-1.0, -1.0);
    this.ControlInk = (Polyline) null;
    this.ControlRectangle = (Border) null;
    this.ControlCircle = (Ellipse) null;
    this.ControlArrow = (System.Windows.Controls.Control) null;
    this.ControlTextBox = (Border) null;
    this.controlLocation = new Rect();
  }

  public void UndoDrawControl()
  {
    if (this.undoDrawControlStack.Count == 0)
      return;
    DrawOperation drawOperation = this.undoDrawControlStack.Pop();
    switch (drawOperation.Type)
    {
      case OperationType.DrawRectangle:
      case OperationType.DrawCircle:
      case OperationType.DrawArrow:
      case OperationType.DrawInk:
      case OperationType.DrawText:
        this.DragResizeView?.RemoveDrawControl(drawOperation.Element);
        break;
      case OperationType.ChangeColor:
        if (!(drawOperation is ChangeColorOperation changeColorOperation))
          break;
        this.SetDrawControlBrush(changeColorOperation.Element, changeColorOperation.OriginalBrush);
        break;
      case OperationType.ChangeThickness:
        if (!(drawOperation is ChangeThicknessOperation thicknessOperation))
          break;
        this.SetDrawControlThickness(thicknessOperation.Element, thicknessOperation.OriginalThickness);
        break;
      case OperationType.MoveControl:
        if (!(drawOperation is MoveControlOperation controlOperation1))
          break;
        controlOperation1.Element.RenderTransform = (Transform) controlOperation1.OriginalTransformGroup;
        break;
      case OperationType.ChangeTextBoxText:
        if (!(drawOperation is ChangeTextBoxTextOperation boxTextOperation) || !(boxTextOperation.Element is Border element) || !(element.Child is System.Windows.Controls.TextBox child))
          break;
        child.Text = boxTextOperation.OriginalText;
        break;
      case OperationType.ChangeFontSize:
        if (!(drawOperation is ChangeFontSizeOperation fontSizeOperation))
          break;
        this.SetDrawTextFontSize(fontSizeOperation.Element, fontSizeOperation.OriginalFontSize);
        break;
      case OperationType.DeleteControl:
        if (!(drawOperation is DeleteControlOperation controlOperation2))
          break;
        this.DragResizeView.AddUIElementToCanvas(controlOperation2.Element);
        Canvas.SetLeft(controlOperation2.Element, controlOperation2.Left);
        Canvas.SetTop(controlOperation2.Element, controlOperation2.Top);
        break;
    }
  }

  private WriteableBitmap AttachDrawControl2FinalImage(WriteableBitmap source)
  {
    DrawingVisual drawingVisual = new DrawingVisual();
    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
    {
      ResizeViewOperation dragMode = this.DragResizeView.DragMode;
      this.DragResizeView.DragMode = ResizeViewOperation.None;
      drawingContext.DrawImage((ImageSource) source, new Rect(0.0, 0.0, source.Width, source.Height));
      Canvas draggerCanvas = this.DragResizeView.GetDraggerCanvas();
      RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int) draggerCanvas.ActualWidth, (int) draggerCanvas.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
      renderTargetBitmap.Render((Visual) draggerCanvas);
      drawingContext.PushTransform((Transform) new ScaleTransform(source.Width / draggerCanvas.ActualWidth, source.Height / draggerCanvas.ActualHeight));
      drawingContext.DrawImage((ImageSource) renderTargetBitmap, new Rect(0.0, 0.0, renderTargetBitmap.Width, renderTargetBitmap.Height));
      this.DragResizeView.DragMode = dragMode;
    }
    RenderTargetBitmap source1 = new RenderTargetBitmap((int) source.Width, (int) source.Height, 96.0, 96.0, PixelFormats.Pbgra32);
    source1.Render((Visual) drawingVisual);
    return new WriteableBitmap((BitmapSource) source1);
  }

  private System.Drawing.Image BitmapSourceToImage(BitmapSource bitmapSource)
  {
    using (MemoryStream memoryStream = new MemoryStream())
    {
      BmpBitmapEncoder bmpBitmapEncoder = new BmpBitmapEncoder();
      bmpBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
      bmpBitmapEncoder.Save((Stream) memoryStream);
      return (System.Drawing.Image) new Bitmap((Stream) memoryStream);
    }
  }

  public async Task ExtractText()
  {
    try
    {
      WriteableBitmap bitmap = await this.GetScaledPageImageAsync();
      PdfPage page = this.viewer?.Document.Pages[this.pageIdx];
      WriteableBitmap writeableBitmap = this.AttachDrawControl2FinalImage(await this.RotateImageAsync(bitmap, page.Rotation));
      if (writeableBitmap != null)
      {
        FS_POINTF startPt = this.startPt;
        FS_POINTF curPt = this.curPt;
        FS_RECTF fsRectf = new FS_RECTF(Math.Min(startPt.X, curPt.X), Math.Max(startPt.Y, curPt.Y), Math.Max(startPt.X, curPt.X), Math.Min(startPt.Y, curPt.Y));
        Rect clientRect;
        if (this.viewer.TryGetClientRect(this.pageIdx, fsRectf, out clientRect))
        {
          System.Drawing.Image image = this.BitmapSourceToImage((BitmapSource) writeableBitmap);
          try
          {
            System.Windows.Forms.Clipboard.SetImage(image);
          }
          catch
          {
          }
          ScreenshotDialogResult extractImageText = ScreenshotDialogResult.CreateExtractImageText(this.pageIdx, "", bitmap, (System.Drawing.Image) new Bitmap(image), fsRectf, clientRect, true);
          this.Close();
          if (extractImageText != null && extractImageText.Completed)
          {
            ExtractTextResultDialog textResultDialog = new ExtractTextResultDialog(extractImageText);
            textResultDialog.Owner = (Window) System.Windows.Application.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
            textResultDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            textResultDialog.ShowDialog();
          }
        }
      }
      bitmap = (WriteableBitmap) null;
    }
    catch
    {
      int num = (int) ModernMessageBox.Show("Failed to OCR from the image!", "PDFgear");
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    System.Windows.Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/screenshots/screenshotdialog.xaml", UriKind.Relative));
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
        this.RootBorder = (Border) target;
        break;
      case 2:
        this.DraggerParent = (Canvas) target;
        break;
      case 3:
        this.BackgroundRectangle = (RectangleGeometry) target;
        break;
      case 4:
        this.DraggerRectangle = (RectangleGeometry) target;
        break;
      case 5:
        this.DragResizeView = (ResizeView) target;
        break;
      case 6:
        this.ExtractTextToolbar = (ScreenshotExtractTextToolbar) target;
        break;
      case 7:
        this.ImageToolbar = (ScreenshotImageToolbar) target;
        break;
      case 8:
        this.OcrToolbar = (ScreenshotOcrToolbar) target;
        break;
      case 9:
        this.CropPageToolbar = (ScreenshotCropPageToolbar) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
