// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ScreenshotWindow
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Extension;
using HandyControl.Tools.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_Canvas", Type = typeof (InkCanvas))]
[TemplatePart(Name = "PART_MaskAreaLeft", Type = typeof (FrameworkElement))]
[TemplatePart(Name = "PART_MaskAreaTop", Type = typeof (FrameworkElement))]
[TemplatePart(Name = "PART_MaskAreaRight", Type = typeof (FrameworkElement))]
[TemplatePart(Name = "PART_MaskAreaBottom", Type = typeof (FrameworkElement))]
[TemplatePart(Name = "PART_TargetArea", Type = typeof (InkCanvas))]
[TemplatePart(Name = "PART_Magnifier", Type = typeof (FrameworkElement))]
public class ScreenshotWindow : System.Windows.Window
{
  private readonly Screenshot _screenshot;
  private VisualBrush _visualPreview;
  private Size _viewboxSize;
  private BitmapSource _imageSource;
  private static readonly Guid BmpGuid = new Guid("{b96b3cab-0728-11d3-9d7b-0000f81ef32e}");
  private const int IntervalLength = 1;
  private const int IntervalBigLength = 10;
  private const int SnapLength = 4;
  private IntPtr _desktopWindowHandle;
  private IntPtr _mouseOverWindowHandle;
  private readonly IntPtr _screenshotWindowHandle;
  private InteropValues.RECT _desktopWindowRect;
  private InteropValues.RECT _targetWindowRect;
  private readonly int[] _flagArr = new int[4];
  private bool _isOut;
  private bool _canDrag;
  private bool _receiveMoveMsg = true;
  private Point _mousePointOld;
  private Point _pointFixed;
  private InteropValues.POINT _pointFloating;
  private bool _saveScreenshot;
  private FrameworkElement _magnifier;
  private const string ElementCanvas = "PART_Canvas";
  private const string ElementMaskAreaLeft = "PART_MaskAreaLeft";
  private const string ElementMaskAreaTop = "PART_MaskAreaTop";
  private const string ElementMaskAreaRight = "PART_MaskAreaRight";
  private const string ElementMaskAreaBottom = "PART_MaskAreaBottom";
  private const string ElementTargetArea = "PART_TargetArea";
  private const string ElementMagnifier = "PART_Magnifier";
  public static readonly DependencyProperty IsDrawingProperty = DependencyProperty.Register(nameof (IsDrawing), typeof (bool), typeof (ScreenshotWindow), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty IsSelectingProperty = DependencyProperty.Register(nameof (IsSelecting), typeof (bool), typeof (ScreenshotWindow), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(nameof (Size), typeof (Size), typeof (ScreenshotWindow), new PropertyMetadata((object) new Size()));
  public static readonly DependencyProperty SizeStrProperty = DependencyProperty.Register(nameof (SizeStr), typeof (string), typeof (ScreenshotWindow), new PropertyMetadata((object) null));
  public static readonly DependencyProperty PixelColorProperty = DependencyProperty.Register(nameof (PixelColor), typeof (Color), typeof (ScreenshotWindow), new PropertyMetadata((object) new Color()));
  public static readonly DependencyProperty PixelColorStrProperty = DependencyProperty.Register(nameof (PixelColorStr), typeof (string), typeof (ScreenshotWindow), new PropertyMetadata((object) null));
  public static readonly DependencyPropertyKey PreviewBrushPropertyKey = DependencyProperty.RegisterReadOnly(nameof (PreviewBrush), typeof (Brush), typeof (ScreenshotWindow), new PropertyMetadata((object) null));
  public static readonly DependencyProperty PreviewBrushProperty = ScreenshotWindow.PreviewBrushPropertyKey.DependencyProperty;

  internal InkCanvas Canvas { get; set; }

  internal FrameworkElement MaskAreaLeft { get; set; }

  internal FrameworkElement MaskAreaTop { get; set; }

  internal FrameworkElement MaskAreaRight { get; set; }

  internal FrameworkElement MaskAreaBottom { get; set; }

  internal FrameworkElement TargetArea { get; set; }

  public bool IsDrawing
  {
    get => (bool) this.GetValue(ScreenshotWindow.IsDrawingProperty);
    internal set => this.SetValue(ScreenshotWindow.IsDrawingProperty, ValueBoxes.BooleanBox(value));
  }

  public bool IsSelecting
  {
    get => (bool) this.GetValue(ScreenshotWindow.IsSelectingProperty);
    internal set
    {
      this.SetValue(ScreenshotWindow.IsSelectingProperty, ValueBoxes.BooleanBox(value));
    }
  }

  public Size Size
  {
    get => (Size) this.GetValue(ScreenshotWindow.SizeProperty);
    internal set => this.SetValue(ScreenshotWindow.SizeProperty, (object) value);
  }

  public string SizeStr
  {
    get => (string) this.GetValue(ScreenshotWindow.SizeStrProperty);
    internal set => this.SetValue(ScreenshotWindow.SizeStrProperty, (object) value);
  }

  public Color PixelColor
  {
    get => (Color) this.GetValue(ScreenshotWindow.PixelColorProperty);
    internal set => this.SetValue(ScreenshotWindow.PixelColorProperty, (object) value);
  }

  public string PixelColorStr
  {
    get => (string) this.GetValue(ScreenshotWindow.PixelColorStrProperty);
    internal set => this.SetValue(ScreenshotWindow.PixelColorStrProperty, (object) value);
  }

  public Brush PreviewBrush
  {
    get => (Brush) this.GetValue(ScreenshotWindow.PreviewBrushProperty);
    set => this.SetValue(ScreenshotWindow.PreviewBrushProperty, (object) value);
  }

  public ScreenshotWindow(Screenshot screenshot)
  {
    this.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("Window4ScreenshotStyle");
    this._screenshot = screenshot;
    this.DataContext = (object) this;
    this._screenshotWindowHandle = WindowHelper.GetHandle(this);
    InteropMethods.EnableWindow(this._screenshotWindowHandle, false);
    this.Loaded += new RoutedEventHandler(this.ScreenshotWindow_Loaded);
    this.Closed += new EventHandler(this.ScreenshotWindow_Closed);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.Canvas = this.GetTemplateChild("PART_Canvas") as InkCanvas;
    this.MaskAreaLeft = this.GetTemplateChild("PART_MaskAreaLeft") as FrameworkElement;
    this.MaskAreaTop = this.GetTemplateChild("PART_MaskAreaTop") as FrameworkElement;
    this.MaskAreaRight = this.GetTemplateChild("PART_MaskAreaRight") as FrameworkElement;
    this.MaskAreaBottom = this.GetTemplateChild("PART_MaskAreaBottom") as FrameworkElement;
    this.TargetArea = this.GetTemplateChild("PART_TargetArea") as FrameworkElement;
    this._magnifier = this.GetTemplateChild("PART_Magnifier") as FrameworkElement;
    if (this._magnifier != null)
      this._viewboxSize = new Size(29.0, 21.0);
    VisualBrush visualBrush = new VisualBrush((Visual) this.Canvas);
    visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
    this._visualPreview = visualBrush;
    this.SetValue(ScreenshotWindow.PreviewBrushPropertyKey, (object) this._visualPreview);
    this._magnifier.Show();
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    if (e.Key == Key.Escape)
      this.Close();
    if (!this.IsDrawing)
      return;
    switch (e.Key)
    {
      case Key.Return:
        this._saveScreenshot = true;
        this.Close();
        break;
      case Key.Left:
        this.MoveTargetArea(ScreenshotWindow.MoveRect(this._targetWindowRect, -1, rightFlag: -1));
        break;
      case Key.Up:
        this.MoveTargetArea(ScreenshotWindow.MoveRect(this._targetWindowRect, topFlag: -1, bottomFlag: -1));
        break;
      case Key.Right:
        this.MoveTargetArea(ScreenshotWindow.MoveRect(this._targetWindowRect, 1, rightFlag: 1));
        break;
      case Key.Down:
        this.MoveTargetArea(ScreenshotWindow.MoveRect(this._targetWindowRect, topFlag: 1, bottomFlag: 1));
        break;
    }
  }

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    this._mousePointOld = e.GetPosition((IInputElement) this);
  }

  protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    this._magnifier.Collapse();
  }

  protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
  {
    this._saveScreenshot = true;
    this.Close();
  }

  protected override void OnPreviewMouseMove(MouseEventArgs e)
  {
    if (e.LeftButton != MouseButtonState.Pressed)
    {
      this.UpdateStatus(e.GetPosition((IInputElement) this.TargetArea));
    }
    else
    {
      Point position = Mouse.GetPosition((IInputElement) this);
      int num1 = (int) (position.X - this._mousePointOld.X);
      int num2 = (int) (position.Y - this._mousePointOld.Y);
      if (this.IsDrawing)
      {
        if (this._isOut)
          return;
        InteropValues.RECT targetWindowRect = this._targetWindowRect;
        if (this._canDrag)
        {
          targetWindowRect.Left += num1;
          targetWindowRect.Top += num2;
          targetWindowRect.Right += num1;
          targetWindowRect.Bottom += num2;
        }
        else
        {
          InteropValues.POINT point = new InteropValues.POINT((int) position.X, (int) position.Y);
          if (this._flagArr[0] > 0)
          {
            this._pointFloating.X += num1 * this._flagArr[0];
            point.X = this._pointFloating.X;
          }
          else if (this._flagArr[2] > 0)
          {
            this._pointFloating.X += num1 * this._flagArr[2];
            point.X = this._pointFloating.X - 1;
          }
          if (this._flagArr[1] > 0)
          {
            this._pointFloating.Y += num2 * this._flagArr[1];
            point.Y = this._pointFloating.Y;
          }
          else if (this._flagArr[3] > 0)
          {
            this._pointFloating.Y += num2 * this._flagArr[3];
            point.Y = this._pointFloating.Y - 1;
          }
          targetWindowRect.Left = (int) Math.Min(this._pointFixed.X, (double) this._pointFloating.X);
          targetWindowRect.Top = (int) Math.Min(this._pointFixed.Y, (double) this._pointFloating.Y);
          targetWindowRect.Right = (int) Math.Max(this._pointFixed.X, (double) this._pointFloating.X);
          targetWindowRect.Bottom = (int) Math.Max(this._pointFixed.Y, (double) this._pointFloating.Y);
          this._magnifier.Show();
          this.MoveMagnifier(point);
        }
        this.MoveTargetArea(targetWindowRect);
        this._mousePointOld = position;
      }
      else if (this.IsSelecting)
      {
        int left = (int) Math.Min(this._mousePointOld.X, position.X);
        int right = (int) Math.Max(this._mousePointOld.X, position.X);
        int top = (int) Math.Min(this._mousePointOld.Y, position.Y);
        int bottom = (int) Math.Max(this._mousePointOld.Y, position.Y);
        this.MoveTargetArea(new InteropValues.RECT(left, top, right, bottom));
      }
      else
      {
        if (this.IsSelecting || num1 <= 0 || num2 <= 0)
          return;
        this.IsSelecting = true;
      }
    }
  }

  private void ScreenshotWindow_Closed(object sender, EventArgs e)
  {
    if (this._saveScreenshot)
      this.SaveScreenshot();
    this.StopHooks();
    this.IsDrawing = false;
    this.Loaded -= new RoutedEventHandler(this.ScreenshotWindow_Loaded);
    this.Closed -= new EventHandler(this.ScreenshotWindow_Closed);
  }

  private void ScreenshotWindow_Loaded(object sender, RoutedEventArgs e)
  {
    this._imageSource = this.GetDesktopSnapshot();
    Image image = new Image()
    {
      Source = (ImageSource) this._imageSource
    };
    RenderOptions.SetBitmapScalingMode((DependencyObject) image, BitmapScalingMode.NearestNeighbor);
    this.Canvas.Children.Add((UIElement) image);
    this.StartHooks();
    InteropValues.POINT pt;
    InteropMethods.GetCursorPos(out pt);
    this.MoveElement(pt);
    this.MoveMagnifier(pt);
  }

  private void UpdateStatus(Point point)
  {
    double num1 = Math.Abs(point.X);
    double num2 = Math.Abs(point.Y);
    double num3 = Math.Abs(point.X - (double) this._targetWindowRect.Width);
    double num4 = Math.Abs(point.Y - (double) this._targetWindowRect.Height);
    this._canDrag = false;
    this._isOut = false;
    this._flagArr[0] = 0;
    this._flagArr[1] = 0;
    this._flagArr[2] = 0;
    this._flagArr[3] = 0;
    Cursor cursor;
    if (num1 <= 4.0)
    {
      if (num2 > 4.0)
      {
        if (num4 > 4.0)
        {
          cursor = Cursors.SizeWE;
          this._pointFixed = new Point((double) this._targetWindowRect.Right, (double) this._targetWindowRect.Top);
          this._pointFloating = new InteropValues.POINT(this._targetWindowRect.Left, this._targetWindowRect.Bottom);
          this._flagArr[0] = 1;
        }
        else
        {
          cursor = Cursors.SizeNESW;
          this._pointFixed = new Point((double) this._targetWindowRect.Right, (double) this._targetWindowRect.Top);
          this._pointFloating = new InteropValues.POINT(this._targetWindowRect.Left, this._targetWindowRect.Bottom);
          this._flagArr[0] = 1;
          this._flagArr[3] = 1;
        }
      }
      else
      {
        cursor = Cursors.SizeNWSE;
        this._pointFixed = new Point((double) this._targetWindowRect.Right, (double) this._targetWindowRect.Bottom);
        this._pointFloating = new InteropValues.POINT(this._targetWindowRect.Left, this._targetWindowRect.Top);
        this._flagArr[0] = 1;
        this._flagArr[1] = 1;
      }
    }
    else if (num3 > 4.0)
    {
      if (num2 > 4.0)
      {
        if (num4 > 4.0)
        {
          if (this.TargetArea.IsMouseOver)
          {
            cursor = Cursors.SizeAll;
            this._canDrag = true;
          }
          else
          {
            cursor = Cursors.Arrow;
            this._isOut = true;
          }
        }
        else
        {
          cursor = Cursors.SizeNS;
          this._pointFixed = new Point((double) this._targetWindowRect.Left, (double) this._targetWindowRect.Top);
          this._pointFloating = new InteropValues.POINT(this._targetWindowRect.Right, this._targetWindowRect.Bottom);
          this._flagArr[3] = 1;
        }
      }
      else
      {
        cursor = Cursors.SizeNS;
        this._pointFixed = new Point((double) this._targetWindowRect.Right, (double) this._targetWindowRect.Bottom);
        this._pointFloating = new InteropValues.POINT(this._targetWindowRect.Left, this._targetWindowRect.Top);
        this._flagArr[1] = 1;
      }
    }
    else if (num3 <= 4.0)
    {
      if (num2 > 4.0)
      {
        if (num4 > 4.0)
        {
          cursor = Cursors.SizeWE;
          this._pointFixed = new Point((double) this._targetWindowRect.Left, (double) this._targetWindowRect.Bottom);
          this._pointFloating = new InteropValues.POINT(this._targetWindowRect.Right, this._targetWindowRect.Top);
          this._flagArr[2] = 1;
        }
        else
        {
          cursor = Cursors.SizeNWSE;
          this._pointFixed = new Point((double) this._targetWindowRect.Left, (double) this._targetWindowRect.Top);
          this._pointFloating = new InteropValues.POINT(this._targetWindowRect.Right, this._targetWindowRect.Bottom);
          this._flagArr[2] = 1;
          this._flagArr[3] = 1;
        }
      }
      else
      {
        cursor = Cursors.SizeNESW;
        this._pointFixed = new Point((double) this._targetWindowRect.Left, (double) this._targetWindowRect.Bottom);
        this._pointFloating = new InteropValues.POINT(this._targetWindowRect.Right, this._targetWindowRect.Top);
        this._flagArr[1] = 1;
        this._flagArr[2] = 1;
      }
    }
    else
    {
      cursor = Cursors.Arrow;
      this._isOut = true;
    }
    this.TargetArea.Cursor = cursor;
  }

  private void StopHooks()
  {
    MouseHook.Stop();
    MouseHook.StatusChanged -= new EventHandler<MouseHookEventArgs>(this.MouseHook_StatusChanged);
  }

  private void StartHooks()
  {
    MouseHook.Start();
    MouseHook.StatusChanged += new EventHandler<MouseHookEventArgs>(this.MouseHook_StatusChanged);
  }

  private void MouseHook_StatusChanged(object sender, MouseHookEventArgs e)
  {
    switch (e.MessageType)
    {
      case MouseHookMessageType.MouseMove:
        this.MoveElement(e.Point);
        this.MoveMagnifier(e.Point);
        break;
      case MouseHookMessageType.LeftButtonDown:
        this._receiveMoveMsg = false;
        this._mousePointOld = new Point((double) e.Point.X, (double) e.Point.Y);
        InteropMethods.EnableWindow(this._screenshotWindowHandle, true);
        break;
      case MouseHookMessageType.LeftButtonUp:
        this.StopHooks();
        this.IsSelecting = false;
        this.IsDrawing = true;
        this._magnifier.Collapse();
        break;
      case MouseHookMessageType.RightButtonDown:
        if (this.IsDrawing)
          break;
        this.Close();
        break;
    }
  }

  private void SaveScreenshot()
  {
    this._screenshot.OnSnapped((ImageSource) new CroppedBitmap(this._imageSource, new Int32Rect(this._targetWindowRect.Left, this._targetWindowRect.Top, this._targetWindowRect.Width, this._targetWindowRect.Height)));
    this.Close();
  }

  private BitmapSource GetDesktopSnapshot()
  {
    this._desktopWindowHandle = InteropMethods.GetDesktopWindow();
    IntPtr windowDc = InteropMethods.GetWindowDC(this._desktopWindowHandle);
    IntPtr compatibleDc = InteropMethods.CreateCompatibleDC(windowDc);
    InteropMethods.GetWindowRect(this._desktopWindowHandle, out this._desktopWindowRect);
    int num1 = this._desktopWindowRect.Right - this._desktopWindowRect.Left;
    int num2 = this._desktopWindowRect.Bottom - this._desktopWindowRect.Top;
    IntPtr compatibleBitmap = InteropMethods.CreateCompatibleBitmap(windowDc, num1, num2);
    IntPtr hgdiobj = InteropMethods.SelectObject(compatibleDc, compatibleBitmap);
    InteropMethods.BitBlt(compatibleDc, 0, 0, num1, num2, windowDc, 0, 0, 13369376);
    InteropMethods.SelectObject(compatibleDc, hgdiobj);
    InteropMethods.DeleteDC(compatibleDc);
    InteropMethods.ReleaseDC(this._desktopWindowHandle, windowDc);
    IntPtr bitmap;
    int bitmapFromHbitmap = InteropMethods.Gdip.GdipCreateBitmapFromHBITMAP(new HandleRef((object) null, compatibleBitmap), new HandleRef((object) null, IntPtr.Zero), out bitmap);
    if (bitmapFromHbitmap != 0)
      throw InteropMethods.Gdip.StatusException(bitmapFromHbitmap);
    using (MemoryStream dataStream = new MemoryStream())
    {
      int numEncoders;
      int size;
      int imageEncodersSize = InteropMethods.Gdip.GdipGetImageEncodersSize(out numEncoders, out size);
      if (imageEncodersSize != 0)
        throw InteropMethods.Gdip.StatusException(imageEncodersSize);
      IntPtr num3 = Marshal.AllocHGlobal(size);
      try
      {
        int status = InteropMethods.Gdip.GdipGetImageEncoders(numEncoders, size, num3);
        if (status != 0)
          throw InteropMethods.Gdip.StatusException(status);
        ImageCodecInfo imageCodecInfo = ((IEnumerable<ImageCodecInfo>) ImageCodecInfo.ConvertFromMemory(num3, numEncoders)).FirstOrDefault<ImageCodecInfo>((Func<ImageCodecInfo, bool>) (item => item.FormatID.Equals(ScreenshotWindow.BmpGuid)));
        if (imageCodecInfo == null)
          throw new Exception("ImageCodecInfo is null");
        IntPtr zero = IntPtr.Zero;
        try
        {
          Guid clsid = imageCodecInfo.Clsid;
          status = InteropMethods.Gdip.GdipSaveImageToStream(new HandleRef((object) this, bitmap), (InteropValues.IStream) new InteropValues.ComStreamFromDataStream((Stream) dataStream), ref clsid, new HandleRef((object) null, zero));
        }
        finally
        {
          if (zero != IntPtr.Zero)
            Marshal.FreeHGlobal(zero);
        }
        if (status != 0)
          throw InteropMethods.Gdip.StatusException(status);
      }
      finally
      {
        Marshal.FreeHGlobal(num3);
      }
      BitmapImage desktopSnapshot = new BitmapImage();
      desktopSnapshot.BeginInit();
      desktopSnapshot.CacheOption = BitmapCacheOption.OnLoad;
      desktopSnapshot.StreamSource = (Stream) dataStream;
      desktopSnapshot.EndInit();
      desktopSnapshot.Freeze();
      return (BitmapSource) desktopSnapshot;
    }
  }

  private void MoveElement(InteropValues.POINT point)
  {
    if (!this._receiveMoveMsg)
      return;
    IntPtr num = InteropMethods.ChildWindowFromPointEx(this._desktopWindowHandle, new InteropValues.POINT()
    {
      X = point.X,
      Y = point.Y
    }, 3);
    if (!(num != this._mouseOverWindowHandle) || !(num != IntPtr.Zero))
      return;
    this._mouseOverWindowHandle = num;
    InteropValues.RECT lpRect;
    InteropMethods.GetWindowRect(this._mouseOverWindowHandle, out lpRect);
    this.MoveTargetArea(lpRect);
  }

  private static InteropValues.RECT MoveRect(
    InteropValues.RECT rect,
    int leftFlag = 0,
    int topFlag = 0,
    int rightFlag = 0,
    int bottomFlag = 0)
  {
    int num = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl) ? 10 : 1;
    rect.Left += leftFlag * num;
    rect.Top += topFlag * num;
    rect.Right += rightFlag * num;
    rect.Bottom += bottomFlag * num;
    return rect;
  }

  private void MoveTargetArea(InteropValues.RECT rect)
  {
    if (rect.Left < 0)
    {
      rect.Right -= rect.Left;
      rect.Left = 0;
    }
    if (rect.Top < 0)
    {
      rect.Bottom -= rect.Top;
      rect.Top = 0;
    }
    if (rect.Right > this._desktopWindowRect.Width)
    {
      rect.Left -= rect.Right - this._desktopWindowRect.Width;
      rect.Right = this._desktopWindowRect.Width;
    }
    if (rect.Bottom > this._desktopWindowRect.Height)
    {
      rect.Top -= rect.Bottom - this._desktopWindowRect.Height;
      rect.Bottom = this._desktopWindowRect.Height;
    }
    rect.Left = Math.Max(0, rect.Left);
    rect.Top = Math.Max(0, rect.Top);
    int width = rect.Width;
    int height = rect.Height;
    int left = rect.Left;
    int top = rect.Top;
    this.TargetArea.Width = (double) width;
    this.TargetArea.Height = (double) height;
    this.TargetArea.Margin = new Thickness((double) left, (double) top, 0.0, 0.0);
    this._targetWindowRect = new InteropValues.RECT(left, top, left + width, top + height);
    this.Size = this._targetWindowRect.Size;
    this.SizeStr = $"{this._targetWindowRect.Width} x {this._targetWindowRect.Height}";
    this.MoveMaskArea();
  }

  private void MoveMaskArea()
  {
    this.MaskAreaLeft.Width = this.TargetArea.Margin.Left;
    this.MaskAreaLeft.Height = (double) this._desktopWindowRect.Height;
    this.MaskAreaTop.Margin = new Thickness(this.TargetArea.Margin.Left, 0.0, 0.0, 0.0);
    this.MaskAreaTop.Width = this.TargetArea.Width;
    this.MaskAreaTop.Height = this.TargetArea.Margin.Top;
    this.MaskAreaRight.Margin = new Thickness(this.TargetArea.Width + this.TargetArea.Margin.Left, 0.0, 0.0, 0.0);
    this.MaskAreaRight.Width = (double) this._desktopWindowRect.Width - this.TargetArea.Margin.Left - this.TargetArea.Width;
    this.MaskAreaRight.Height = (double) this._desktopWindowRect.Height;
    FrameworkElement maskAreaBottom = this.MaskAreaBottom;
    Thickness margin = this.TargetArea.Margin;
    double left = margin.Left;
    double height = this.TargetArea.Height;
    margin = this.TargetArea.Margin;
    double top1 = margin.Top;
    double top2 = height + top1;
    Thickness thickness = new Thickness(left, top2, 0.0, 0.0);
    maskAreaBottom.Margin = thickness;
    this.MaskAreaBottom.Width = this.TargetArea.Width;
    this.MaskAreaBottom.Height = (double) this._desktopWindowRect.Height - this.TargetArea.Height - this.TargetArea.Margin.Top;
  }

  private void MoveMagnifier(InteropValues.POINT point)
  {
    this._magnifier.Margin = new Thickness((double) (point.X + 4), (double) (point.Y + 26), 0.0, 0.0);
    this._visualPreview.Viewbox = new Rect(new Point((double) point.X - this._viewboxSize.Width / 2.0 + 0.5, (double) point.Y - this._viewboxSize.Height / 2.0 + 0.5), this._viewboxSize);
  }
}
