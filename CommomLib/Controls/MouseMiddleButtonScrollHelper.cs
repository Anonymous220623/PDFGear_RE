// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.MouseMiddleButtonScrollHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

#nullable disable
namespace CommomLib.Controls;

internal class MouseMiddleButtonScrollHelper : IDisposable
{
  private readonly object locker = new object();
  private readonly ScrollViewer scrollViewer;
  private bool disposeValue;
  private Window window;
  private System.Windows.Point startPoint;
  private bool hasScrolled;
  private DispatcherTimer timer;
  private double scrollStartThreshold = 20.0;
  private bool showCursorAtStartPoint;

  public MouseMiddleButtonScrollHelper(ScrollViewer scrollViewer)
  {
    this.scrollViewer = scrollViewer;
    scrollViewer.MouseDown += new MouseButtonEventHandler(this.ScrollViewer_MouseDown);
  }

  public double ScrollStartThreshold
  {
    get => this.scrollStartThreshold;
    set
    {
      if (value == this.scrollStartThreshold)
        return;
      this.scrollStartThreshold = value >= 0.0 ? value : throw new ArgumentException(nameof (ScrollStartThreshold));
      this.UpdateScrollStates();
    }
  }

  public bool ShowCursorAtStartPoint
  {
    get => this.showCursorAtStartPoint;
    set
    {
      if (this.showCursorAtStartPoint == value)
        return;
      this.showCursorAtStartPoint = value;
      this.UpdateScrollStates();
    }
  }

  public bool InScrollMode => this.window != null;

  public bool EnterScrollMode()
  {
    lock (this.locker)
    {
      if (this.window != null)
        return true;
      if (this.scrollViewer.IsLoaded && (this.scrollViewer.ScrollableWidth > 0.0 || this.scrollViewer.ScrollableHeight > 0.0) && this.scrollViewer.CaptureMouse())
        this.window = Window.GetWindow((DependencyObject) this.scrollViewer);
      if (this.window != null)
      {
        if (this.timer == null)
        {
          this.timer = new DispatcherTimer(DispatcherPriority.Render)
          {
            Interval = TimeSpan.FromMilliseconds(33.0)
          };
          this.timer.Tick += (EventHandler) ((s, a) => this.UpdateScrollStates());
        }
        this.scrollViewer.Unloaded += new RoutedEventHandler(this.ScrollViewer_Unloaded);
        this.window.PreviewKeyDown += new KeyEventHandler(this.Window_PreviewKeyDown);
        this.window.PreviewMouseWheel += new MouseWheelEventHandler(this.Window_PreviewMouseWheel);
        this.window.PreviewMouseDown += new MouseButtonEventHandler(this.Window_PreviewMouseDown);
        this.window.PreviewMouseUp += new MouseButtonEventHandler(this.Window_PreviewMouseUp);
        this.window.StateChanged += new EventHandler(this.Window_StateChanged);
        this.window.Deactivated += new EventHandler(this.Window_Deactivated);
        this.window.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.Window_IsVisibleChanged);
        this.window.Closed += new EventHandler(this.Window_Closed);
        this.window.SizeChanged += new SizeChangedEventHandler(this.Window_SizeChanged);
        this.startPoint = MouseMiddleButtonScrollHelper.MouseEx.GetPosition((UIElement) this.scrollViewer);
        this.hasScrolled = false;
        this.UpdateScrollStates();
        this.timer.Start();
        return true;
      }
      this.ExitScrollMode();
      return false;
    }
  }

  public void ExitScrollMode()
  {
    lock (this.locker)
    {
      Window window = this.window;
      if (window == null)
        return;
      this.startPoint = new System.Windows.Point();
      this.hasScrolled = false;
      this.RemoveScrollStartCursor();
      this.scrollViewer.ReleaseMouseCapture();
      this.timer?.Stop();
      Mouse.OverrideCursor = (Cursor) null;
      this.scrollViewer.Unloaded -= new RoutedEventHandler(this.ScrollViewer_Unloaded);
      this.window = (Window) null;
      if (window == null)
        return;
      window.PreviewKeyDown -= new KeyEventHandler(this.Window_PreviewKeyDown);
      window.PreviewMouseWheel -= new MouseWheelEventHandler(this.Window_PreviewMouseWheel);
      window.PreviewMouseDown -= new MouseButtonEventHandler(this.Window_PreviewMouseDown);
      window.PreviewMouseUp -= new MouseButtonEventHandler(this.Window_PreviewMouseUp);
      window.StateChanged -= new EventHandler(this.Window_StateChanged);
      window.Deactivated -= new EventHandler(this.Window_Deactivated);
      window.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.Window_IsVisibleChanged);
      window.Closed -= new EventHandler(this.Window_Closed);
      window.SizeChanged -= new SizeChangedEventHandler(this.Window_SizeChanged);
    }
  }

  public void Dispose()
  {
    if (this.disposeValue)
      return;
    this.disposeValue = true;
    this.scrollViewer.MouseDown -= new MouseButtonEventHandler(this.ScrollViewer_MouseDown);
    this.ExitScrollMode();
  }

  private void ScrollViewer_MouseDown(object sender, MouseButtonEventArgs e)
  {
    if (e.ChangedButton != MouseButton.Middle || e.MiddleButton != MouseButtonState.Pressed)
      return;
    this.ExitScrollMode();
    e.Handled = this.EnterScrollMode();
  }

  private void ScrollViewer_Unloaded(object sender, RoutedEventArgs e) => this.ExitScrollMode();

  private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
  {
    if (e.ChangedButton != MouseButton.Middle || e.MiddleButton != MouseButtonState.Released)
      return;
    System.Windows.Point position = e.GetPosition((IInputElement) this.scrollViewer);
    if (Math.Abs(position.X - this.startPoint.X) <= 10.0 && Math.Abs(position.Y - this.startPoint.Y) <= 10.0 && !this.hasScrolled)
      return;
    e.Handled = true;
    this.ExitScrollMode();
  }

  private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    e.Handled = true;
    this.ExitScrollMode();
  }

  private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e) => e.Handled = true;

  private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    e.Handled = true;
    this.ExitScrollMode();
  }

  private void Window_SizeChanged(object sender, SizeChangedEventArgs e) => this.ExitScrollMode();

  private void Window_Closed(object sender, EventArgs e) => this.ExitScrollMode();

  private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    this.ExitScrollMode();
  }

  private void Window_StateChanged(object sender, EventArgs e) => this.ExitScrollMode();

  private void Window_Deactivated(object sender, EventArgs e) => this.ExitScrollMode();

  private void UpdateScrollStates()
  {
    lock (this.locker)
    {
      if (this.window == null)
        return;
      System.Windows.Point currentPoint = MouseMiddleButtonScrollHelper.MouseEx.GetPosition((UIElement) this.scrollViewer);
      this.UpdateScrollStartCursor();
      double scrollOffsetX;
      double scrollOffsetY;
      Mouse.OverrideCursor = this.GetCursor(in this.startPoint, in currentPoint, out scrollOffsetX, out scrollOffsetY);
      if (scrollOffsetX != 0.0)
      {
        this.hasScrolled = true;
        this.scrollViewer.ScrollToHorizontalOffset(Math.Min(this.scrollViewer.HorizontalOffset + scrollOffsetX, this.scrollViewer.ScrollableWidth));
      }
      if (scrollOffsetY == 0.0)
        return;
      this.hasScrolled = true;
      this.scrollViewer.ScrollToVerticalOffset(Math.Min(this.scrollViewer.VerticalOffset + scrollOffsetY, this.scrollViewer.ScrollableHeight));
    }
  }

  private void UpdateScrollStartCursor()
  {
    if (this.window == null)
    {
      this.RemoveScrollStartCursor();
    }
    else
    {
      ScrollViewer scrollViewer = this.scrollViewer;
      if (scrollViewer == null)
        return;
      AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer((Visual) scrollViewer);
      Adorner[] adorners = adornerLayer.GetAdorners((UIElement) scrollViewer);
      MouseMiddleButtonScrollHelper.CursorAdorner cursorAdorner = adorners != null ? adorners.OfType<MouseMiddleButtonScrollHelper.CursorAdorner>().FirstOrDefault<MouseMiddleButtonScrollHelper.CursorAdorner>() : (MouseMiddleButtonScrollHelper.CursorAdorner) null;
      if (this.ShowCursorAtStartPoint)
      {
        if (cursorAdorner == null)
        {
          cursorAdorner = new MouseMiddleButtonScrollHelper.CursorAdorner((UIElement) scrollViewer);
          adornerLayer.Add((Adorner) cursorAdorner);
        }
        cursorAdorner.StartPoint = this.startPoint;
        cursorAdorner.CanHorizontallyScroll = this.scrollViewer.ScrollableWidth > 0.0;
        cursorAdorner.CanVerticallyScroll = this.scrollViewer.ScrollableHeight > 0.0;
      }
      else
        this.RemoveScrollStartCursor();
    }
  }

  private void RemoveScrollStartCursor()
  {
    ScrollViewer scrollViewer = this.scrollViewer;
    if (scrollViewer == null)
      return;
    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer((Visual) scrollViewer);
    MouseMiddleButtonScrollHelper.CursorAdorner cursorAdorner1;
    if (adornerLayer == null)
    {
      cursorAdorner1 = (MouseMiddleButtonScrollHelper.CursorAdorner) null;
    }
    else
    {
      Adorner[] adorners = adornerLayer.GetAdorners((UIElement) scrollViewer);
      cursorAdorner1 = adorners != null ? adorners.OfType<MouseMiddleButtonScrollHelper.CursorAdorner>().FirstOrDefault<MouseMiddleButtonScrollHelper.CursorAdorner>() : (MouseMiddleButtonScrollHelper.CursorAdorner) null;
    }
    MouseMiddleButtonScrollHelper.CursorAdorner cursorAdorner2 = cursorAdorner1;
    if (cursorAdorner2 == null)
      return;
    adornerLayer.Remove((Adorner) cursorAdorner2);
  }

  private Cursor GetCursor(
    in System.Windows.Point startPoint,
    in System.Windows.Point currentPoint,
    out double scrollOffsetX,
    out double scrollOffsetY)
  {
    scrollOffsetX = 0.0;
    scrollOffsetY = 0.0;
    System.Windows.Point point1 = currentPoint;
    double x1 = point1.X;
    point1 = startPoint;
    double x2 = point1.X;
    double num1 = x1 - x2;
    System.Windows.Point point2 = currentPoint;
    double y1 = point2.Y;
    point2 = startPoint;
    double y2 = point2.Y;
    double num2 = y1 - y2;
    bool flag1 = this.scrollViewer.ScrollableWidth > 0.0;
    bool flag2 = this.scrollViewer.ScrollableHeight > 0.0;
    Cursor cursor = !(flag1 & flag2) ? (!flag1 ? MouseMiddleButtonScrollHelper.ScrollCursorHelper.ScrollNS : MouseMiddleButtonScrollHelper.ScrollCursorHelper.ScrollWE) : MouseMiddleButtonScrollHelper.ScrollCursorHelper.ScrollAll;
    if (Math.Abs(num1) >= this.ScrollStartThreshold || Math.Abs(num2) >= this.ScrollStartThreshold)
    {
      if (!flag1)
        num1 = 0.0;
      if (!flag2)
        num2 = 0.0;
      if (Math.Abs(num1) > this.ScrollStartThreshold)
        scrollOffsetX = (num1 > 0.0 ? num1 - this.ScrollStartThreshold : num1 + this.ScrollStartThreshold) * 0.75;
      if (Math.Abs(num2) > this.ScrollStartThreshold)
        scrollOffsetY = (num2 > 0.0 ? num2 - this.ScrollStartThreshold : num2 + this.ScrollStartThreshold) * 0.75;
      if (num1 <= -this.ScrollStartThreshold && num2 <= -this.ScrollStartThreshold)
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.ScrollNW;
      if (num1 >= this.ScrollStartThreshold && num2 <= -this.ScrollStartThreshold)
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.ScrollNE;
      if (num1 >= this.ScrollStartThreshold && num2 >= this.ScrollStartThreshold)
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.ScrollSE;
      if (num1 <= -this.ScrollStartThreshold && num2 >= this.ScrollStartThreshold)
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.ScrollSW;
      if (num1 <= -this.ScrollStartThreshold)
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.ScrollW;
      if (num1 >= this.ScrollStartThreshold)
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.ScrollE;
      if (num2 <= -this.ScrollStartThreshold)
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.ScrollN;
      if (num2 >= this.ScrollStartThreshold)
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.ScrollS;
    }
    return cursor;
  }

  private class CursorAdorner : Adorner
  {
    private System.Windows.Point startPoint;
    private bool canHorizontallyScroll;
    private bool canVerticallyScroll;

    public CursorAdorner(UIElement adornedElement)
      : base(adornedElement)
    {
      this.IsHitTestVisible = false;
    }

    public System.Windows.Point StartPoint
    {
      get => this.startPoint;
      set
      {
        if (!(this.startPoint != value))
          return;
        this.startPoint = value;
        this.InvalidateVisual();
      }
    }

    public bool CanHorizontallyScroll
    {
      get => this.canHorizontallyScroll;
      set
      {
        if (this.canHorizontallyScroll == value)
          return;
        this.canHorizontallyScroll = value;
        this.InvalidateVisual();
      }
    }

    public bool CanVerticallyScroll
    {
      get => this.canVerticallyScroll;
      set
      {
        if (this.canVerticallyScroll == value)
          return;
        this.canVerticallyScroll = value;
        this.InvalidateVisual();
      }
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
      base.OnRender(drawingContext);
      ImageSource cursorImage = this.GetCursorImage();
      if (cursorImage == null)
        return;
      drawingContext.DrawImage(cursorImage, new Rect(this.startPoint.X - 16.0, this.startPoint.Y - 16.0, 32.0, 32.0));
    }

    private ImageSource GetCursorImage()
    {
      if (this.CanHorizontallyScroll && this.CanVerticallyScroll)
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.ScrollAllImage;
      if (this.CanHorizontallyScroll)
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.ScrollWEImage;
      return this.CanVerticallyScroll ? MouseMiddleButtonScrollHelper.ScrollCursorHelper.ScrollNSImage : (ImageSource) null;
    }
  }

  private static class MouseEx
  {
    public static System.Windows.Point GetPosition(UIElement element)
    {
      MouseMiddleButtonScrollHelper.MouseEx._POINT lpPoint;
      return MouseMiddleButtonScrollHelper.MouseEx.GetCursorPos(out lpPoint) ? element.PointFromScreen(new System.Windows.Point((double) lpPoint.X, (double) lpPoint.Y)) : Mouse.GetPosition((IInputElement) element);
    }

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(
      out MouseMiddleButtonScrollHelper.MouseEx._POINT lpPoint);

    private struct _POINT
    {
      public int X;
      public int Y;
    }
  }

  private static class ScrollCursorHelper
  {
    private static readonly Dictionary<string, Cursor> cursors = new Dictionary<string, Cursor>();
    private static readonly Dictionary<string, WriteableBitmap> cursorImages = new Dictionary<string, WriteableBitmap>();
    private static Func<Cursor, SafeHandle> cursorHandleGetter;

    public static Cursor ScrollAll
    {
      get
      {
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.EnsureCursor("Assets.Cursors.ScrollAll.cur", Cursors.ScrollAll);
      }
    }

    public static Cursor ScrollE
    {
      get
      {
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.EnsureCursor("Assets.Cursors.ScrollE.cur", Cursors.ScrollE);
      }
    }

    public static Cursor ScrollN
    {
      get
      {
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.EnsureCursor("Assets.Cursors.ScrollN.cur", Cursors.ScrollN);
      }
    }

    public static Cursor ScrollNE
    {
      get
      {
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.EnsureCursor("Assets.Cursors.ScrollNE.cur", Cursors.ScrollNE);
      }
    }

    public static Cursor ScrollNS
    {
      get
      {
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.EnsureCursor("Assets.Cursors.ScrollNS.cur", Cursors.ScrollNS);
      }
    }

    public static Cursor ScrollNW
    {
      get
      {
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.EnsureCursor("Assets.Cursors.ScrollNW.cur", Cursors.ScrollNW);
      }
    }

    public static Cursor ScrollS
    {
      get
      {
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.EnsureCursor("Assets.Cursors.ScrollS.cur", Cursors.ScrollS);
      }
    }

    public static Cursor ScrollSE
    {
      get
      {
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.EnsureCursor("Assets.Cursors.ScrollSE.cur", Cursors.ScrollSE);
      }
    }

    public static Cursor ScrollSW
    {
      get
      {
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.EnsureCursor("Assets.Cursors.ScrollSW.cur", Cursors.ScrollSW);
      }
    }

    public static Cursor ScrollW
    {
      get
      {
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.EnsureCursor("Assets.Cursors.ScrollW.cur", Cursors.ScrollW);
      }
    }

    public static Cursor ScrollWE
    {
      get
      {
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.EnsureCursor("Assets.Cursors.ScrollWE.cur", Cursors.ScrollWE);
      }
    }

    public static ImageSource ScrollAllImage
    {
      get
      {
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.EnsureCursorImage("Assets.Cursors.ScrollAll.cur", Cursors.ScrollAll);
      }
    }

    public static ImageSource ScrollNSImage
    {
      get
      {
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.EnsureCursorImage("Assets.Cursors.ScrollNS.cur", Cursors.ScrollNS);
      }
    }

    public static ImageSource ScrollWEImage
    {
      get
      {
        return MouseMiddleButtonScrollHelper.ScrollCursorHelper.EnsureCursorImage("Assets.Cursors.ScrollWE.cur", Cursors.ScrollWE);
      }
    }

    private static Cursor EnsureCursor(string fileName, Cursor fallbackCursor)
    {
      lock (MouseMiddleButtonScrollHelper.ScrollCursorHelper.cursors)
      {
        Cursor cursor;
        if (MouseMiddleButtonScrollHelper.ScrollCursorHelper.cursors.TryGetValue(fileName, out cursor))
          return cursor;
        Assembly assembly = typeof (MouseMiddleButtonScrollHelper.ScrollCursorHelper).Assembly;
        using (Stream manifestResourceStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{fileName}"))
          return manifestResourceStream != null ? (MouseMiddleButtonScrollHelper.ScrollCursorHelper.cursors[fileName] = new Cursor(manifestResourceStream, true)) : (MouseMiddleButtonScrollHelper.ScrollCursorHelper.cursors[fileName] = fallbackCursor);
      }
    }

    private static ImageSource EnsureCursorImage(string fileName, Cursor fallbackCursor)
    {
      lock (MouseMiddleButtonScrollHelper.ScrollCursorHelper.cursorImages)
      {
        WriteableBitmap writeableBitmap1;
        if (MouseMiddleButtonScrollHelper.ScrollCursorHelper.cursorImages.TryGetValue(fileName, out writeableBitmap1))
          return (ImageSource) writeableBitmap1;
        Cursor cursor = MouseMiddleButtonScrollHelper.ScrollCursorHelper.EnsureCursor(fileName, fallbackCursor);
        WriteableBitmap writeableBitmap2 = MouseMiddleButtonScrollHelper.ScrollCursorHelper.GetCursorImage(cursor);
        if (writeableBitmap2 == null && cursor != fallbackCursor)
          writeableBitmap2 = MouseMiddleButtonScrollHelper.ScrollCursorHelper.GetCursorImage(fallbackCursor);
        if (writeableBitmap2 == null)
          writeableBitmap2 = new WriteableBitmap(32 /*0x20*/, 32 /*0x20*/, 96.0, 96.0, PixelFormats.Bgra32, (BitmapPalette) null);
        MouseMiddleButtonScrollHelper.ScrollCursorHelper.cursorImages[fileName] = writeableBitmap2;
        return (ImageSource) writeableBitmap2;
      }
    }

    private static unsafe WriteableBitmap GetCursorImage(Cursor cursor)
    {
      if (MouseMiddleButtonScrollHelper.ScrollCursorHelper.cursorHandleGetter == null)
      {
        lock (MouseMiddleButtonScrollHelper.ScrollCursorHelper.cursors)
        {
          if (MouseMiddleButtonScrollHelper.ScrollCursorHelper.cursorHandleGetter == null)
          {
            try
            {
              PropertyInfo property = typeof (Cursor).GetProperty("Handle", BindingFlags.Instance | BindingFlags.NonPublic);
              if (property != (PropertyInfo) null)
              {
                ParameterExpression parameterExpression;
                MouseMiddleButtonScrollHelper.ScrollCursorHelper.cursorHandleGetter = System.Linq.Expressions.Expression.Lambda<Func<Cursor, SafeHandle>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression, property), typeof (SafeHandle)), parameterExpression).Compile();
              }
            }
            catch
            {
            }
            if (MouseMiddleButtonScrollHelper.ScrollCursorHelper.cursorHandleGetter == null)
              MouseMiddleButtonScrollHelper.ScrollCursorHelper.cursorHandleGetter = (Func<Cursor, SafeHandle>) (_ => (SafeHandle) null);
          }
        }
      }
      SafeHandle safeHandle = MouseMiddleButtonScrollHelper.ScrollCursorHelper.cursorHandleGetter(cursor);
      if (safeHandle != null && !safeHandle.IsInvalid)
      {
        bool success = false;
        safeHandle.DangerousAddRef(ref success);
        if (success)
        {
          try
          {
            using (Icon icon = Icon.FromHandle(safeHandle.DangerousGetHandle()))
            {
              using (Bitmap bitmap1 = icon.ToBitmap())
              {
                using (Bitmap bitmap2 = new Bitmap(bitmap1.Width, bitmap1.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                  using (Graphics graphics = Graphics.FromImage((System.Drawing.Image) bitmap2))
                    graphics.DrawImageUnscaled((System.Drawing.Image) bitmap1, 0, 0);
                  BitmapData bitmapdata = bitmap2.LockBits(new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                  if (bitmapdata != null)
                  {
                    try
                    {
                      WriteableBitmap cursorImage = new WriteableBitmap(bitmap2.Width, bitmap2.Height, 96.0, 96.0, PixelFormats.Bgra32, (BitmapPalette) null);
                      if (cursorImage.TryLock(Duration.Forever))
                      {
                        try
                        {
                          int num = bitmapdata.Stride * bitmapdata.Height;
                          Buffer.MemoryCopy((void*) bitmapdata.Scan0, (void*) cursorImage.BackBuffer, (long) num, (long) num);
                          cursorImage.AddDirtyRect(new Int32Rect(0, 0, bitmapdata.Width, bitmapdata.Height));
                        }
                        finally
                        {
                          cursorImage.Unlock();
                        }
                        return cursorImage;
                      }
                    }
                    finally
                    {
                      bitmap2.UnlockBits(bitmapdata);
                    }
                  }
                }
              }
            }
          }
          finally
          {
            safeHandle.DangerousRelease();
          }
        }
      }
      return (WriteableBitmap) null;
    }
  }
}
