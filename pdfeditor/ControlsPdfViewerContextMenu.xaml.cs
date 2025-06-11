// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PdfViewerContextMenu
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls;

public partial class PdfViewerContextMenu : ContextMenu
{
  private List<PdfViewerContextMenuItem> currentItems;
  private Border Border;
  private DispatcherTimer timer;
  private System.Windows.Point? centerPoint;
  private DpiScale dpiScale;
  public static readonly DependencyProperty AutoCloseOnMouseLeaveProperty = DependencyProperty.Register(nameof (AutoCloseOnMouseLeave), typeof (bool), typeof (PdfViewerContextMenu), new PropertyMetadata((object) true, new PropertyChangedCallback(PdfViewerContextMenu.OnAutoCloseOnMouseLeavePropertyChanged)));

  static PdfViewerContextMenu()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (PdfViewerContextMenu), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (PdfViewerContextMenu)));
  }

  public PdfViewerContextMenu()
  {
    this.currentItems = new List<PdfViewerContextMenuItem>();
    this.timer = new DispatcherTimer(DispatcherPriority.Normal)
    {
      Interval = TimeSpan.FromSeconds(0.05)
    };
    this.timer.Tick += new EventHandler(this.Timer_Tick);
    this.Placement = PlacementMode.Custom;
    this.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.OnSelectTextContextMenuPlacement);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.Border = this.GetTemplateChild("Border") as Border;
    if (this.Border == null)
      return;
    this.Border.Opacity = 1.0;
  }

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new PdfViewerContextMenuItem();
  }

  protected override bool IsItemItsOwnContainerOverride(object item)
  {
    return item is PdfViewerContextMenuItem;
  }

  protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
  {
    if (element is PdfViewerContextMenuItem viewerContextMenuItem)
    {
      lock (this.currentItems)
        this.currentItems.Add(viewerContextMenuItem);
    }
    base.PrepareContainerForItemOverride(element, item);
  }

  protected override void ClearContainerForItemOverride(DependencyObject element, object item)
  {
    if (element is PdfViewerContextMenuItem viewerContextMenuItem)
    {
      lock (this.currentItems)
        this.currentItems.Remove(viewerContextMenuItem);
    }
    base.ClearContainerForItemOverride(element, item);
  }

  protected override void OnOpened(RoutedEventArgs e)
  {
    base.OnOpened(e);
    if (this.Border != null)
    {
      this.Border.Opacity = 1.0;
      this.centerPoint = new System.Windows.Point?(this.Border.PointToScreen(new System.Windows.Point(this.Border.ActualWidth / 2.0, this.Border.ActualHeight / 2.0)));
      this.dpiScale = VisualTreeHelper.GetDpi((Visual) this.Border);
    }
    else
      this.centerPoint = new System.Windows.Point?();
    if (!this.AutoCloseOnMouseLeave)
      return;
    this.timer.Start();
  }

  protected override void OnClosed(RoutedEventArgs e)
  {
    if (this.Border != null)
      this.Border.Opacity = 1.0;
    this.centerPoint = new System.Windows.Point?();
    this.timer.Stop();
    base.OnClosed(e);
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    MainView mainView = Application.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
    if (mainView != null)
    {
      mainView.RaiseEvent((RoutedEventArgs) e);
      if (e.Handled)
      {
        if (!this.IsOpen)
          return;
        this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() =>
        {
          try
          {
            if (!this.IsOpen)
              return;
            this.IsOpen = false;
          }
          catch
          {
          }
        }));
        return;
      }
    }
    base.OnPreviewKeyDown(e);
  }

  private void Timer_Tick(object sender, EventArgs e)
  {
    if (this.Border == null || !this.IsOpen)
    {
      this.timer.Stop();
    }
    else
    {
      lock (this.currentItems)
      {
        if (this.currentItems.Any<PdfViewerContextMenuItem>((Func<PdfViewerContextMenuItem, bool>) (c => c.IsSubmenuOpen)))
        {
          this.Border.Opacity = 1.0;
        }
        else
        {
          long distanceFromCursor = this.GetPixelDistanceFromCursor();
          if (distanceFromCursor == -1L)
          {
            this.Border.Opacity = 1.0;
          }
          else
          {
            double num1 = (double) distanceFromCursor / this.dpiScale.PixelsPerDip;
            double num2 = Math.Max(this.Border.ActualWidth, this.Border.ActualHeight);
            if (num2 <= 150.0)
              num2 = 150.0;
            if (num1 < num2)
              this.Border.Opacity = 1.0;
            else if (num1 < num2 * 2.0)
            {
              double num3 = num1 - num2;
              this.Border.Opacity = (num2 - num3) / num2;
            }
            else
              this.Border.Opacity = 0.0;
          }
        }
      }
      if (this.Border.Opacity >= 0.05)
        return;
      this.timer.Stop();
      this.IsOpen = false;
    }
  }

  public bool AutoCloseOnMouseLeave
  {
    get => (bool) this.GetValue(PdfViewerContextMenu.AutoCloseOnMouseLeaveProperty);
    set => this.SetValue(PdfViewerContextMenu.AutoCloseOnMouseLeaveProperty, (object) value);
  }

  private static void OnAutoCloseOnMouseLeavePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if ((bool) e.NewValue == (bool) e.OldValue || !(d is PdfViewerContextMenu viewerContextMenu))
      return;
    if (viewerContextMenu.IsOpen)
    {
      if ((bool) e.NewValue)
        viewerContextMenu.timer.Start();
      else
        viewerContextMenu.timer.Stop();
    }
    if ((bool) e.NewValue)
    {
      if (viewerContextMenu.IsOpen)
        viewerContextMenu.timer.Start();
      viewerContextMenu.Placement = PlacementMode.Custom;
      viewerContextMenu.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(viewerContextMenu.OnSelectTextContextMenuPlacement);
    }
    else
    {
      viewerContextMenu.timer.Stop();
      viewerContextMenu.Placement = PlacementMode.MousePoint;
      viewerContextMenu.CustomPopupPlacementCallback = (CustomPopupPlacementCallback) null;
    }
  }

  private CustomPopupPlacement[] OnSelectTextContextMenuPlacement(
    System.Windows.Size popupSize,
    System.Windows.Size targetSize,
    System.Windows.Point offset)
  {
    FrameworkElement placementTarget = this.PlacementTarget as FrameworkElement;
    Rect placementRectangle = this.PlacementRectangle;
    double pixelsPerDip = VisualTreeHelper.GetDpi((Visual) this).PixelsPerDip;
    if (placementTarget == null)
    {
      Action action = (Action) null;
      action = (Action) (() =>
      {
        if (this.IsOpen)
          this.IsOpen = false;
        else
          this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) action);
      });
      this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) action);
      return new CustomPopupPlacement[1]
      {
        new CustomPopupPlacement(new System.Windows.Point(0.0, 0.0), PopupPrimaryAxis.None)
      };
    }
    System.Windows.Size size1 = new System.Windows.Size(placementTarget.ActualWidth, placementTarget.ActualHeight);
    System.Windows.Size size2 = new System.Windows.Size(popupSize.Width / pixelsPerDip, popupSize.Height / pixelsPerDip);
    System.Windows.Point position = Mouse.GetPosition((IInputElement) placementTarget);
    Rect rect1 = new Rect(position, size2);
    if (!placementRectangle.IsEmpty)
    {
      bool flag1 = Math.Abs(placementRectangle.Left - position.X) < Math.Abs(placementRectangle.Right - position.X);
      bool flag2 = Math.Abs(placementRectangle.Top - position.Y) < Math.Abs(placementRectangle.Bottom - position.Y);
      Rect rect2 = placementRectangle;
      rect2.Intersect(rect1);
      if (!rect2.IsEmpty)
      {
        double num1 = Math.Min(placementRectangle.Left, rect2.Left) - rect1.Right;
        double num2 = Math.Max(placementRectangle.Right, rect2.Right) - rect1.Left;
        double num3 = Math.Min(placementRectangle.Top, rect2.Top) - rect1.Bottom;
        double num4 = Math.Max(placementRectangle.Bottom, rect2.Bottom) - rect1.Top;
        double offsetX = flag1 ? num1 : num2;
        double offsetY = flag2 ? num3 : num4;
        if (Math.Abs(offsetX) < Math.Abs(offsetY))
          rect1.Offset(offsetX, 0.0);
        else
          rect1.Offset(0.0, offsetY);
      }
    }
    double left = rect1.Left;
    double top = rect1.Top;
    if (!placementRectangle.IsEmpty)
    {
      left -= placementRectangle.Left;
      top -= placementRectangle.Top;
    }
    return new CustomPopupPlacement[1]
    {
      new CustomPopupPlacement(new System.Windows.Point(left * pixelsPerDip, top * pixelsPerDip), PopupPrimaryAxis.None)
    };
  }

  private long GetPixelDistanceFromCursor()
  {
    PdfViewerContextMenu.POINT lpPoint = new PdfViewerContextMenu.POINT();
    if (!PdfViewerContextMenu.GetCursorPosNative(out lpPoint))
      return -1;
    MainView mainView = Application.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
    PdfViewerContextMenu.RECT lpRect;
    if (mainView != null && PdfViewerContextMenu.GetWindowRect(new WindowInteropHelper((Window) mainView).EnsureHandle(), out lpRect))
    {
      Rect rect = (Rect) lpRect;
      if (!rect.IsEmpty)
      {
        double num = rect.Width - 17.0 * VisualTreeHelper.GetDpi((Visual) mainView).PixelsPerDip;
        if (num >= 0.0)
        {
          rect.Width = num;
          if (!rect.Contains((double) lpPoint.X, (double) lpPoint.Y))
            return (long) int.MaxValue;
        }
      }
    }
    if (!this.centerPoint.HasValue)
      return -1;
    System.Windows.Point point = this.centerPoint.Value;
    return (long) Math.Max(Math.Abs((double) lpPoint.X - point.X), Math.Abs((double) lpPoint.Y - point.Y));
  }

  [DllImport("user32.dll", EntryPoint = "GetCursorPos", SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool GetCursorPosNative(out PdfViewerContextMenu.POINT lpPoint);

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool GetWindowRect(IntPtr hWnd, out PdfViewerContextMenu.RECT lpRect);

  private struct POINT(int x, int y)
  {
    public int X = x;
    public int Y = y;

    public static implicit operator System.Drawing.Point(PdfViewerContextMenu.POINT p)
    {
      return new System.Drawing.Point(p.X, p.Y);
    }

    public static implicit operator PdfViewerContextMenu.POINT(System.Drawing.Point p)
    {
      return new PdfViewerContextMenu.POINT(p.X, p.Y);
    }
  }

  public struct RECT
  {
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;

    public int Width => this.Right - this.Left;

    public int Height => this.Bottom - this.Top;

    public static implicit operator PdfViewerContextMenu.RECT(Int32Rect rect)
    {
      return new PdfViewerContextMenu.RECT()
      {
        Left = rect.X,
        Top = rect.Y,
        Right = rect.X + rect.Width,
        Bottom = rect.Y + rect.Height
      };
    }

    public static implicit operator Int32Rect(PdfViewerContextMenu.RECT rect)
    {
      return rect.Left > rect.Right || rect.Top > rect.Bottom ? Int32Rect.Empty : new Int32Rect(rect.Left, rect.Top, rect.Width, rect.Height);
    }

    public static implicit operator Rect(PdfViewerContextMenu.RECT rect)
    {
      return rect.Left > rect.Right || rect.Top > rect.Bottom ? Rect.Empty : new Rect((double) rect.Left, (double) rect.Top, (double) rect.Width, (double) rect.Height);
    }
  }
}
