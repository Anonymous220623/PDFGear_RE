// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ImageViewer
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Properties.Langs;
using HandyControl.Tools;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_PanelMain", Type = typeof (Panel))]
[TemplatePart(Name = "PART_CanvasSmallImg", Type = typeof (Canvas))]
[TemplatePart(Name = "PART_BorderMove", Type = typeof (Border))]
[TemplatePart(Name = "PART_BorderBottom", Type = typeof (Border))]
[TemplatePart(Name = "PART_ImageMain", Type = typeof (Image))]
public class ImageViewer : Control
{
  private const string ElementPanelMain = "PART_PanelMain";
  private const string ElementCanvasSmallImg = "PART_CanvasSmallImg";
  private const string ElementBorderMove = "PART_BorderMove";
  private const string ElementBorderBottom = "PART_BorderBottom";
  private const string ElementImageMain = "PART_ImageMain";
  private const double ScaleInternal = 0.2;
  private static readonly SaveFileDialog SaveFileDialog;
  private Panel _panelMain;
  private Canvas _canvasSmallImg;
  private Border _borderMove;
  private Border _borderBottom;
  private Image _imageMain;
  private bool _borderSmallIsLoaded;
  private bool _canMoveX;
  private bool _canMoveY;
  private Thickness _imgActualMargin;
  private double _imgActualRotate;
  private double _imgActualScale = 1.0;
  private Point _imgCurrentPoint;
  private bool _imgIsMouseDown;
  private Thickness _imgMouseDownMargin;
  private Point _imgMouseDownPoint;
  private Point _imgSmallCurrentPoint;
  private bool _imgSmallIsMouseDown;
  private Thickness _imgSmallMouseDownMargin;
  private Point _imgSmallMouseDownPoint;
  private double _imgWidHeiScale;
  private bool _isOblique;
  private double _scaleInternalHeight;
  private double _scaleInternalWidth;
  private bool _showBorderBottom;
  private DispatcherTimer _dispatcher;
  private bool _isLoaded;
  private MouseBinding _mouseMoveBinding;
  private ImageBrowser _imageBrowser;
  public static readonly DependencyProperty ShowImgMapProperty;
  public static readonly DependencyProperty ImageSourceProperty;
  public static readonly DependencyProperty UriProperty;
  public static readonly DependencyProperty ShowToolBarProperty;
  public static readonly DependencyProperty IsFullScreenProperty;
  public static readonly DependencyProperty MoveGestureProperty;
  internal static readonly DependencyProperty ImgPathProperty;
  internal static readonly DependencyProperty ImgSizeProperty;
  internal static readonly DependencyProperty ShowFullScreenButtonProperty;
  internal static readonly DependencyProperty ShowCloseButtonProperty;
  internal static readonly DependencyProperty ImageContentProperty;
  internal static readonly DependencyProperty ImageMarginProperty;
  internal static readonly DependencyProperty ImageWidthProperty;
  internal static readonly DependencyProperty ImageHeightProperty;
  internal static readonly DependencyProperty ImageScaleProperty;
  internal static readonly DependencyProperty ScaleStrProperty;
  internal static readonly DependencyProperty ImageRotateProperty;
  internal static readonly DependencyProperty ShowSmallImgInternalProperty;

  public ImageViewer()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Save, new ExecutedRoutedEventHandler(this.ButtonSave_OnClick)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Open, new ExecutedRoutedEventHandler(this.ButtonWindowsOpen_OnClick)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Restore, new ExecutedRoutedEventHandler(this.ButtonActual_OnClick)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Reduce, new ExecutedRoutedEventHandler(this.ButtonReduce_OnClick)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Enlarge, new ExecutedRoutedEventHandler(this.ButtonEnlarge_OnClick)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.RotateLeft, new ExecutedRoutedEventHandler(this.ButtonRotateLeft_OnClick)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.RotateRight, new ExecutedRoutedEventHandler(this.ButtonRotateRight_OnClick)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.MouseMove, new ExecutedRoutedEventHandler(this.ImageMain_OnMouseDown)));
    this.OnMoveGestureChanged(this.MoveGesture);
    this.Loaded += (RoutedEventHandler) ((s, e) =>
    {
      this._isLoaded = true;
      this.Init();
    });
  }

  public ImageViewer(Uri uri)
    : this()
  {
    this.Uri = uri;
  }

  public ImageViewer(string path)
    : this(new Uri(path))
  {
  }

  public bool IsFullScreen
  {
    get => (bool) this.GetValue(ImageViewer.IsFullScreenProperty);
    set => this.SetValue(ImageViewer.IsFullScreenProperty, ValueBoxes.BooleanBox(value));
  }

  [ValueSerializer(typeof (MouseGestureValueSerializer))]
  [TypeConverter(typeof (MouseGestureConverter))]
  public MouseGesture MoveGesture
  {
    get => (MouseGesture) this.GetValue(ImageViewer.MoveGestureProperty);
    set => this.SetValue(ImageViewer.MoveGestureProperty, (object) value);
  }

  public bool ShowImgMap
  {
    get => (bool) this.GetValue(ImageViewer.ShowImgMapProperty);
    set => this.SetValue(ImageViewer.ShowImgMapProperty, ValueBoxes.BooleanBox(value));
  }

  public BitmapFrame ImageSource
  {
    get => (BitmapFrame) this.GetValue(ImageViewer.ImageSourceProperty);
    set => this.SetValue(ImageViewer.ImageSourceProperty, (object) value);
  }

  public Uri Uri
  {
    get => (Uri) this.GetValue(ImageViewer.UriProperty);
    set => this.SetValue(ImageViewer.UriProperty, (object) value);
  }

  public bool ShowToolBar
  {
    get => (bool) this.GetValue(ImageViewer.ShowToolBarProperty);
    set => this.SetValue(ImageViewer.ShowToolBarProperty, ValueBoxes.BooleanBox(value));
  }

  internal object ImageContent
  {
    get => this.GetValue(ImageViewer.ImageContentProperty);
    set => this.SetValue(ImageViewer.ImageContentProperty, value);
  }

  internal string ImgPath
  {
    get => (string) this.GetValue(ImageViewer.ImgPathProperty);
    set => this.SetValue(ImageViewer.ImgPathProperty, (object) value);
  }

  internal long ImgSize
  {
    get => (long) this.GetValue(ImageViewer.ImgSizeProperty);
    set => this.SetValue(ImageViewer.ImgSizeProperty, (object) value);
  }

  internal bool ShowFullScreenButton
  {
    get => (bool) this.GetValue(ImageViewer.ShowFullScreenButtonProperty);
    set => this.SetValue(ImageViewer.ShowFullScreenButtonProperty, ValueBoxes.BooleanBox(value));
  }

  internal Thickness ImageMargin
  {
    get => (Thickness) this.GetValue(ImageViewer.ImageMarginProperty);
    set => this.SetValue(ImageViewer.ImageMarginProperty, (object) value);
  }

  internal double ImageWidth
  {
    get => (double) this.GetValue(ImageViewer.ImageWidthProperty);
    set => this.SetValue(ImageViewer.ImageWidthProperty, (object) value);
  }

  internal double ImageHeight
  {
    get => (double) this.GetValue(ImageViewer.ImageHeightProperty);
    set => this.SetValue(ImageViewer.ImageHeightProperty, (object) value);
  }

  internal double ImageScale
  {
    get => (double) this.GetValue(ImageViewer.ImageScaleProperty);
    set => this.SetValue(ImageViewer.ImageScaleProperty, (object) value);
  }

  internal string ScaleStr
  {
    get => (string) this.GetValue(ImageViewer.ScaleStrProperty);
    set => this.SetValue(ImageViewer.ScaleStrProperty, (object) value);
  }

  internal double ImageRotate
  {
    get => (double) this.GetValue(ImageViewer.ImageRotateProperty);
    set => this.SetValue(ImageViewer.ImageRotateProperty, (object) value);
  }

  internal bool ShowSmallImgInternal
  {
    get => (bool) this.GetValue(ImageViewer.ShowSmallImgInternalProperty);
    set => this.SetValue(ImageViewer.ShowSmallImgInternalProperty, ValueBoxes.BooleanBox(value));
  }

  private double ImageOriWidth { get; set; }

  private double ImageOriHeight { get; set; }

  internal bool ShowCloseButton
  {
    get => (bool) this.GetValue(ImageViewer.ShowCloseButtonProperty);
    set => this.SetValue(ImageViewer.ShowCloseButtonProperty, ValueBoxes.BooleanBox(value));
  }

  internal bool ShowBorderBottom
  {
    get => this._showBorderBottom;
    set
    {
      if (this._showBorderBottom == value)
        return;
      this._borderBottom?.BeginAnimation(UIElement.OpacityProperty, value ? (AnimationTimeline) AnimationHelper.CreateAnimation(1.0, 100.0) : (AnimationTimeline) AnimationHelper.CreateAnimation(0.0, 400.0));
      this._showBorderBottom = value;
    }
  }

  public override void OnApplyTemplate()
  {
    if (this._canvasSmallImg != null)
    {
      this._canvasSmallImg.MouseLeftButtonDown -= new MouseButtonEventHandler(this.CanvasSmallImg_OnMouseLeftButtonDown);
      this._canvasSmallImg.MouseLeftButtonUp -= new MouseButtonEventHandler(this.CanvasSmallImg_OnMouseLeftButtonUp);
      this._canvasSmallImg.MouseMove -= new MouseEventHandler(this.CanvasSmallImg_OnMouseMove);
    }
    base.OnApplyTemplate();
    this._panelMain = this.GetTemplateChild("PART_PanelMain") as Panel;
    this._canvasSmallImg = this.GetTemplateChild("PART_CanvasSmallImg") as Canvas;
    this._borderMove = this.GetTemplateChild("PART_BorderMove") as Border;
    this._imageMain = this.GetTemplateChild("PART_ImageMain") as Image;
    this._borderBottom = this.GetTemplateChild("PART_BorderBottom") as Border;
    if (this._imageMain != null)
    {
      RotateTransform target = new RotateTransform();
      BindingOperations.SetBinding((DependencyObject) target, RotateTransform.AngleProperty, (BindingBase) new Binding(ImageViewer.ImageRotateProperty.Name)
      {
        Source = (object) this
      });
      this._imageMain.LayoutTransform = (Transform) target;
    }
    if (this._canvasSmallImg != null)
    {
      this._canvasSmallImg.MouseLeftButtonDown += new MouseButtonEventHandler(this.CanvasSmallImg_OnMouseLeftButtonDown);
      this._canvasSmallImg.MouseLeftButtonUp += new MouseButtonEventHandler(this.CanvasSmallImg_OnMouseLeftButtonUp);
      this._canvasSmallImg.MouseMove += new MouseEventHandler(this.CanvasSmallImg_OnMouseMove);
    }
    this._borderSmallIsLoaded = false;
  }

  private static void OnImageScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ImageViewer imageViewer) || !(e.NewValue is double newValue))
      return;
    imageViewer.ImageWidth = imageViewer.ImageOriWidth * newValue;
    imageViewer.ImageHeight = imageViewer.ImageOriHeight * newValue;
    imageViewer.ScaleStr = $"{newValue * 100.0:#0}%";
  }

  private void Init()
  {
    if (this.ImageSource == null || !this._isLoaded)
      return;
    if (this.ImageSource.IsDownloading)
    {
      this._dispatcher = new DispatcherTimer(DispatcherPriority.ApplicationIdle)
      {
        Interval = TimeSpan.FromSeconds(1.0)
      };
      this._dispatcher.Tick += new EventHandler(this.Dispatcher_Tick);
      this._dispatcher.Start();
    }
    else
    {
      double num1;
      double num2;
      if (!this._isOblique)
      {
        num1 = (double) this.ImageSource.PixelWidth;
        num2 = (double) this.ImageSource.PixelHeight;
      }
      else
      {
        num1 = (double) this.ImageSource.PixelHeight;
        num2 = (double) this.ImageSource.PixelWidth;
      }
      this.ImageWidth = num1;
      this.ImageHeight = num2;
      this.ImageOriWidth = num1;
      this.ImageOriHeight = num2;
      this._scaleInternalWidth = this.ImageOriWidth * 0.2;
      this._scaleInternalHeight = this.ImageOriHeight * 0.2;
      if (Math.Abs(num2 - 0.0) < 0.001 || Math.Abs(num1 - 0.0) < 0.001)
      {
        int num3 = (int) MessageBox.Show(Lang.ErrorImgSize);
      }
      else
      {
        this._imgWidHeiScale = num1 / num2;
        double num4 = this.ActualWidth / this.ActualHeight;
        this.ImageScale = 1.0;
        if (this._imgWidHeiScale > num4)
        {
          if (num1 > this.ActualWidth)
            this.ImageScale = this.ActualWidth / num1;
        }
        else if (num2 > this.ActualHeight)
          this.ImageScale = this.ActualHeight / num2;
        this.ImageMargin = new Thickness((this.ActualWidth - this.ImageWidth) / 2.0, (this.ActualHeight - this.ImageHeight) / 2.0, 0.0, 0.0);
        this._imgActualScale = this.ImageScale;
        this._imgActualMargin = this.ImageMargin;
        this.InitBorderSmall();
      }
    }
  }

  private void Dispatcher_Tick(object sender, EventArgs e)
  {
    if (this._dispatcher == null)
      return;
    if (this.ImageSource == null || !this._isLoaded)
    {
      this._dispatcher.Stop();
      this._dispatcher.Tick -= new EventHandler(this.Dispatcher_Tick);
      this._dispatcher = (DispatcherTimer) null;
    }
    else
    {
      if (this.ImageSource.IsDownloading)
        return;
      this._dispatcher.Stop();
      this._dispatcher.Tick -= new EventHandler(this.Dispatcher_Tick);
      this._dispatcher = (DispatcherTimer) null;
      this.Init();
    }
  }

  private void ButtonActual_OnClick(object sender, RoutedEventArgs e)
  {
    DoubleAnimation animation1 = AnimationHelper.CreateAnimation(1.0);
    animation1.FillBehavior = FillBehavior.Stop;
    this._imgActualScale = 1.0;
    animation1.Completed += (EventHandler) ((s, e1) =>
    {
      this.ImageScale = 1.0;
      this._canMoveX = this.ImageWidth > this.ActualWidth;
      this._canMoveY = this.ImageHeight > this.ActualHeight;
      this.BorderSmallShowSwitch();
    });
    Thickness thickness = new Thickness((this.ActualWidth - this.ImageOriWidth) / 2.0, (this.ActualHeight - this.ImageOriHeight) / 2.0, 0.0, 0.0);
    ThicknessAnimation animation2 = AnimationHelper.CreateAnimation(thickness);
    animation2.FillBehavior = FillBehavior.Stop;
    this._imgActualMargin = thickness;
    animation2.Completed += (EventHandler) ((s, e1) => this.ImageMargin = thickness);
    this.BeginAnimation(ImageViewer.ImageScaleProperty, (AnimationTimeline) animation1);
    this.BeginAnimation(ImageViewer.ImageMarginProperty, (AnimationTimeline) animation2);
  }

  private void ButtonReduce_OnClick(object sender, RoutedEventArgs e) => this.ScaleImg(false);

  private void ButtonEnlarge_OnClick(object sender, RoutedEventArgs e) => this.ScaleImg(true);

  private void ButtonRotateLeft_OnClick(object sender, RoutedEventArgs e)
  {
    this.RotateImg(this._imgActualRotate - 90.0);
  }

  private void ButtonRotateRight_OnClick(object sender, RoutedEventArgs e)
  {
    this.RotateImg(this._imgActualRotate + 90.0);
  }

  private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
  {
    if (this.ImageSource == null)
      return;
    ImageViewer.SaveFileDialog.FileName = $"{DateTime.Now:yyyy-M-d-h-m-s.fff}";
    bool? nullable = ImageViewer.SaveFileDialog.ShowDialog();
    bool flag = true;
    if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
      return;
    using (FileStream fileStream = new FileStream(ImageViewer.SaveFileDialog.FileName, FileMode.Create, FileAccess.Write))
    {
      PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
      pngBitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapSource) this.ImageSource));
      pngBitmapEncoder.Save((Stream) fileStream);
    }
  }

  private void ButtonWindowsOpen_OnClick(object sender, RoutedEventArgs e)
  {
    Uri uri = this.Uri;
    if ((object) uri == null)
      return;
    this._imageBrowser?.Close();
    this._imageBrowser = new ImageBrowser(uri);
    this._imageBrowser.Show();
  }

  protected override void OnMouseMove(MouseEventArgs e) => this.MoveImg();

  protected override void OnMouseLeave(MouseEventArgs e) => this.ShowBorderBottom = false;

  protected override void OnMouseWheel(MouseWheelEventArgs e) => this.ScaleImg(e.Delta > 0);

  protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
  {
    base.OnRenderSizeChanged(sizeInfo);
    this.OnRenderSizeChanged();
  }

  private void OnRenderSizeChanged()
  {
    if (this.ImageWidth < 0.001 || this.ImageHeight < 0.001)
      return;
    this._canMoveX = true;
    this._canMoveY = true;
    double left = this.ImageMargin.Left;
    double top = this.ImageMargin.Top;
    if (this.ImageWidth <= this.ActualWidth)
    {
      this._canMoveX = false;
      left = (this.ActualWidth - this.ImageWidth) / 2.0;
    }
    if (this.ImageHeight <= this.ActualHeight)
    {
      this._canMoveY = false;
      top = (this.ActualHeight - this.ImageHeight) / 2.0;
    }
    this.ImageMargin = new Thickness(left, top, 0.0, 0.0);
    this._imgActualMargin = this.ImageMargin;
    this.BorderSmallShowSwitch();
    this._imgSmallMouseDownMargin = this._borderMove.Margin;
    this.MoveSmallImg(this._imgSmallMouseDownMargin.Left, this._imgSmallMouseDownMargin.Top);
  }

  private void ImageMain_OnMouseDown(object sender, ExecutedRoutedEventArgs e)
  {
    this._imgMouseDownPoint = Mouse.GetPosition((IInputElement) this._panelMain);
    this._imgMouseDownMargin = this.ImageMargin;
    this._imgIsMouseDown = true;
  }

  protected override void OnPreviewMouseUp(MouseButtonEventArgs e) => this._imgIsMouseDown = false;

  private void BorderSmallShowSwitch()
  {
    if (this._canMoveX || this._canMoveY)
    {
      if (!this._borderSmallIsLoaded)
      {
        this._canvasSmallImg.Background = (Brush) new VisualBrush((Visual) this._imageMain);
        this.InitBorderSmall();
        this._borderSmallIsLoaded = true;
      }
      this.ShowSmallImgInternal = true;
      this.UpdateBorderSmall();
    }
    else
      this.ShowSmallImgInternal = false;
  }

  private void InitBorderSmall()
  {
    if (this._canvasSmallImg == null)
      return;
    if (this._imgWidHeiScale > this._canvasSmallImg.MaxWidth / this._canvasSmallImg.MaxHeight)
    {
      this._canvasSmallImg.Width = this._canvasSmallImg.MaxWidth;
      this._canvasSmallImg.Height = this._canvasSmallImg.Width / this._imgWidHeiScale;
    }
    else
    {
      this._canvasSmallImg.Width = this._canvasSmallImg.MaxHeight * this._imgWidHeiScale;
      this._canvasSmallImg.Height = this._canvasSmallImg.MaxHeight;
    }
  }

  private void UpdateBorderSmall()
  {
    if (!this.ShowSmallImgInternal)
      return;
    double num1 = Math.Min(this.ImageWidth, this.ActualWidth);
    double num2 = Math.Min(this.ImageHeight, this.ActualHeight);
    this._borderMove.Width = num1 / this.ImageWidth * this._canvasSmallImg.Width;
    this._borderMove.Height = num2 / this.ImageHeight * this._canvasSmallImg.Height;
    Thickness imageMargin = this.ImageMargin;
    double val2_1 = -imageMargin.Left / this.ImageWidth * this._canvasSmallImg.Width;
    imageMargin = this.ImageMargin;
    double val2_2 = -imageMargin.Top / this.ImageHeight * this._canvasSmallImg.Height;
    double val1_1 = this._canvasSmallImg.Width - this._borderMove.Width;
    double val1_2 = this._canvasSmallImg.Height - this._borderMove.Height;
    double val2_3 = Math.Max(0.0, val2_1);
    double left = Math.Min(val1_1, val2_3);
    double val2_4 = Math.Max(0.0, val2_2);
    double top = Math.Min(val1_2, val2_4);
    this._borderMove.Margin = new Thickness(left, top, 0.0, 0.0);
  }

  private void ScaleImg(bool isEnlarge)
  {
    if (Mouse.LeftButton == MouseButtonState.Pressed)
      return;
    double imageWidth = this.ImageWidth;
    double imageHeight = this.ImageHeight;
    double num1 = isEnlarge ? this._imgActualScale + 0.2 : this._imgActualScale - 0.2;
    if (Math.Abs(num1) < 0.2)
      num1 = 0.2;
    else if (Math.Abs(num1) > 50.0)
      num1 = 50.0;
    this.ImageScale = num1;
    Point position = Mouse.GetPosition((IInputElement) this._panelMain);
    Point point = new Point(position.X - this._imgActualMargin.Left, position.Y - this._imgActualMargin.Top);
    double num2 = 0.5 * this._scaleInternalWidth;
    double num3 = 0.5 * this._scaleInternalHeight;
    if (this.ImageWidth > this.ActualWidth)
    {
      this._canMoveX = true;
      if (this.ImageHeight > this.ActualHeight)
      {
        this._canMoveY = true;
        num2 = point.X / imageWidth * this._scaleInternalWidth;
        num3 = point.Y / imageHeight * this._scaleInternalHeight;
      }
      else
        this._canMoveY = false;
    }
    else
    {
      this._canMoveY = this.ImageHeight > this.ActualHeight;
      this._canMoveX = false;
    }
    Thickness thickness1;
    if (isEnlarge)
    {
      thickness1 = new Thickness(this._imgActualMargin.Left - num2, this._imgActualMargin.Top - num3, 0.0, 0.0);
    }
    else
    {
      double left1 = this._imgActualMargin.Left + num2;
      double top1 = this._imgActualMargin.Top + num3;
      double num4 = this.ImageWidth - this.ActualWidth;
      double num5 = this.ImageHeight - this.ActualHeight;
      double num6 = this._borderMove.Width - this._canvasSmallImg.ActualWidth;
      Thickness thickness2 = this._borderMove.Margin;
      double left2 = thickness2.Left;
      double num7 = Math.Abs(num6 + left2);
      double num8 = this._borderMove.Height - this._canvasSmallImg.ActualHeight;
      thickness2 = this._borderMove.Margin;
      double top2 = thickness2.Top;
      double num9 = Math.Abs(num8 + top2);
      thickness2 = this.ImageMargin;
      if (Math.Abs(thickness2.Left) < 0.001 || num7 < 0.001)
      {
        double left3 = this._imgActualMargin.Left;
        thickness2 = this._borderMove.Margin;
        double num10 = thickness2.Left / (this._canvasSmallImg.ActualWidth - this._borderMove.Width) * this._scaleInternalWidth;
        left1 = left3 + num10;
      }
      thickness2 = this.ImageMargin;
      if (Math.Abs(thickness2.Top) < 0.001 || num9 < 0.001)
      {
        double top3 = this._imgActualMargin.Top;
        thickness2 = this._borderMove.Margin;
        double num11 = thickness2.Top / (this._canvasSmallImg.ActualHeight - this._borderMove.Height) * this._scaleInternalHeight;
        top1 = top3 + num11;
      }
      if (num4 < 0.001)
        left1 = (this.ActualWidth - this.ImageWidth) / 2.0;
      if (num5 < 0.001)
        top1 = (this.ActualHeight - this.ImageHeight) / 2.0;
      thickness1 = new Thickness(left1, top1, 0.0, 0.0);
    }
    this.ImageMargin = thickness1;
    this._imgActualScale = num1;
    this._imgActualMargin = thickness1;
    this.BorderSmallShowSwitch();
    this._imgSmallMouseDownMargin = this._borderMove.Margin;
    this.MoveSmallImg(this._imgSmallMouseDownMargin.Left, this._imgSmallMouseDownMargin.Top);
  }

  private void RotateImg(double rotate)
  {
    this._imgActualRotate = rotate;
    this._isOblique = ((int) this._imgActualRotate - 90) % 180 == 0;
    this.ShowSmallImgInternal = false;
    this.Init();
    this.InitBorderSmall();
    DoubleAnimation animation = AnimationHelper.CreateAnimation(rotate);
    animation.Completed += (EventHandler) ((s, e1) => this.ImageRotate = rotate);
    animation.FillBehavior = FillBehavior.Stop;
    this.BeginAnimation(ImageViewer.ImageRotateProperty, (AnimationTimeline) animation);
  }

  private MouseButtonState GetMouseButtonState()
  {
    MouseButtonState mouseButtonState;
    switch (this.MoveGesture.MouseAction)
    {
      case MouseAction.LeftClick:
        mouseButtonState = Mouse.LeftButton;
        break;
      case MouseAction.RightClick:
        mouseButtonState = Mouse.RightButton;
        break;
      case MouseAction.MiddleClick:
        mouseButtonState = Mouse.MiddleButton;
        break;
      default:
        mouseButtonState = Mouse.LeftButton;
        break;
    }
    return mouseButtonState;
  }

  private void MoveImg()
  {
    this._imgCurrentPoint = Mouse.GetPosition((IInputElement) this._panelMain);
    this.ShowCloseButton = this._imgCurrentPoint.Y < 200.0;
    this.ShowBorderBottom = this._imgCurrentPoint.Y > this.ActualHeight - 200.0;
    if (this.GetMouseButtonState() == MouseButtonState.Released || !this._imgIsMouseDown)
      return;
    double num1 = this._imgCurrentPoint.X - this._imgMouseDownPoint.X;
    double num2 = this._imgCurrentPoint.Y - this._imgMouseDownPoint.Y;
    double left = this._imgMouseDownMargin.Left;
    if (this.ImageWidth > this.ActualWidth)
    {
      left = this._imgMouseDownMargin.Left + num1;
      if (left >= 0.0)
        left = 0.0;
      else if (-left + this.ActualWidth >= this.ImageWidth)
        left = this.ActualWidth - this.ImageWidth;
      this._canMoveX = true;
    }
    double top = this._imgMouseDownMargin.Top;
    if (this.ImageHeight > this.ActualHeight)
    {
      top = this._imgMouseDownMargin.Top + num2;
      if (top >= 0.0)
        top = 0.0;
      else if (-top + this.ActualHeight >= this.ImageHeight)
        top = this.ActualHeight - this.ImageHeight;
      this._canMoveY = true;
    }
    this.ImageMargin = new Thickness(left, top, 0.0, 0.0);
    this._imgActualMargin = this.ImageMargin;
    this.UpdateBorderSmall();
  }

  private void MoveSmallImg()
  {
    if (!this._imgSmallIsMouseDown || this.GetMouseButtonState() == MouseButtonState.Released)
      return;
    this._imgSmallCurrentPoint = Mouse.GetPosition((IInputElement) this._canvasSmallImg);
    this.MoveSmallImg(this._imgSmallMouseDownMargin.Left + (this._imgSmallCurrentPoint.X - this._imgSmallMouseDownPoint.X), this._imgSmallMouseDownMargin.Top + (this._imgSmallCurrentPoint.Y - this._imgSmallMouseDownPoint.Y));
  }

  private void MoveSmallImg(double marginX, double marginY)
  {
    if (marginX < 0.0)
      marginX = 0.0;
    else if (marginX + this._borderMove.Width >= this._canvasSmallImg.Width)
      marginX = this._canvasSmallImg.Width - this._borderMove.Width;
    if (marginY < 0.0)
      marginY = 0.0;
    else if (marginY + this._borderMove.Height >= this._canvasSmallImg.Height)
      marginY = this._canvasSmallImg.Height - this._borderMove.Height;
    this._borderMove.Margin = new Thickness(marginX, marginY, 0.0, 0.0);
    double left = (this.ActualWidth - this.ImageWidth) / 2.0;
    double top = (this.ActualHeight - this.ImageHeight) / 2.0;
    if (this._canMoveX)
      left = -marginX / this._canvasSmallImg.Width * this.ImageWidth;
    if (this._canMoveY)
      top = -marginY / this._canvasSmallImg.Height * this.ImageHeight;
    this.ImageMargin = new Thickness(left, top, 0.0, 0.0);
    this._imgActualMargin = this.ImageMargin;
  }

  private void CanvasSmallImg_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this._imgSmallMouseDownPoint = Mouse.GetPosition((IInputElement) this._canvasSmallImg);
    this._imgSmallMouseDownMargin = this._borderMove.Margin;
    this._imgSmallIsMouseDown = true;
    e.Handled = true;
  }

  private void CanvasSmallImg_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    this._imgSmallIsMouseDown = false;
  }

  private void CanvasSmallImg_OnMouseMove(object sender, MouseEventArgs e) => this.MoveSmallImg();

  private static void OnMoveGestureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ImageViewer) d).OnMoveGestureChanged((MouseGesture) e.NewValue);
  }

  private void OnMoveGestureChanged(MouseGesture newValue)
  {
    this.InputBindings.Remove((InputBinding) this._mouseMoveBinding);
    this._mouseMoveBinding = new MouseBinding((ICommand) ControlCommands.MouseMove, newValue);
    this.InputBindings.Add((InputBinding) this._mouseMoveBinding);
  }

  private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ImageViewer) d).OnImageSourceChanged();
  }

  private void OnImageSourceChanged() => this.Init();

  private static void OnUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ImageViewer) d).OnUriChanged((Uri) e.NewValue);
  }

  private void OnUriChanged(Uri newValue)
  {
    if ((object) newValue != null)
    {
      this.ImageSource = GetBitmapFrame(newValue);
      this.ImgPath = newValue.AbsolutePath;
      if (!File.Exists(this.ImgPath))
        return;
      this.ImgSize = new FileInfo(this.ImgPath).Length;
    }
    else
    {
      this.ImageSource = (BitmapFrame) null;
      this.ImgPath = string.Empty;
    }

    static BitmapFrame GetBitmapFrame(Uri source)
    {
      try
      {
        return BitmapFrame.Create(source);
      }
      catch
      {
        return (BitmapFrame) null;
      }
    }
  }

  static ImageViewer()
  {
    SaveFileDialog saveFileDialog = new SaveFileDialog();
    saveFileDialog.Filter = Lang.PngImg + "|*.png";
    ImageViewer.SaveFileDialog = saveFileDialog;
    ImageViewer.ShowImgMapProperty = DependencyProperty.Register(nameof (ShowImgMap), typeof (bool), typeof (ImageViewer), new PropertyMetadata(ValueBoxes.FalseBox));
    ImageViewer.ImageSourceProperty = DependencyProperty.Register(nameof (ImageSource), typeof (BitmapFrame), typeof (ImageViewer), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageViewer.OnImageSourceChanged)));
    ImageViewer.UriProperty = DependencyProperty.Register(nameof (Uri), typeof (Uri), typeof (ImageViewer), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageViewer.OnUriChanged)));
    ImageViewer.ShowToolBarProperty = DependencyProperty.Register(nameof (ShowToolBar), typeof (bool), typeof (ImageViewer), new PropertyMetadata(ValueBoxes.TrueBox));
    ImageViewer.IsFullScreenProperty = DependencyProperty.Register(nameof (IsFullScreen), typeof (bool), typeof (ImageViewer), new PropertyMetadata(ValueBoxes.FalseBox));
    ImageViewer.MoveGestureProperty = DependencyProperty.Register(nameof (MoveGesture), typeof (MouseGesture), typeof (ImageViewer), (PropertyMetadata) new UIPropertyMetadata((object) new MouseGesture(MouseAction.LeftClick), new PropertyChangedCallback(ImageViewer.OnMoveGestureChanged)));
    ImageViewer.ImgPathProperty = DependencyProperty.Register(nameof (ImgPath), typeof (string), typeof (ImageViewer), new PropertyMetadata((object) null));
    ImageViewer.ImgSizeProperty = DependencyProperty.Register(nameof (ImgSize), typeof (long), typeof (ImageViewer), new PropertyMetadata((object) -1L));
    ImageViewer.ShowFullScreenButtonProperty = DependencyProperty.Register(nameof (ShowFullScreenButton), typeof (bool), typeof (ImageViewer), new PropertyMetadata(ValueBoxes.FalseBox));
    ImageViewer.ShowCloseButtonProperty = DependencyProperty.Register(nameof (ShowCloseButton), typeof (bool), typeof (ImageViewer), new PropertyMetadata(ValueBoxes.FalseBox));
    ImageViewer.ImageContentProperty = DependencyProperty.Register(nameof (ImageContent), typeof (object), typeof (ImageViewer), new PropertyMetadata((object) null));
    ImageViewer.ImageMarginProperty = DependencyProperty.Register(nameof (ImageMargin), typeof (Thickness), typeof (ImageViewer), new PropertyMetadata((object) new Thickness()));
    ImageViewer.ImageWidthProperty = DependencyProperty.Register(nameof (ImageWidth), typeof (double), typeof (ImageViewer), new PropertyMetadata(ValueBoxes.Double0Box));
    ImageViewer.ImageHeightProperty = DependencyProperty.Register(nameof (ImageHeight), typeof (double), typeof (ImageViewer), new PropertyMetadata(ValueBoxes.Double0Box));
    ImageViewer.ImageScaleProperty = DependencyProperty.Register(nameof (ImageScale), typeof (double), typeof (ImageViewer), new PropertyMetadata(ValueBoxes.Double1Box, new PropertyChangedCallback(ImageViewer.OnImageScaleChanged)));
    ImageViewer.ScaleStrProperty = DependencyProperty.Register(nameof (ScaleStr), typeof (string), typeof (ImageViewer), new PropertyMetadata((object) "100%"));
    ImageViewer.ImageRotateProperty = DependencyProperty.Register(nameof (ImageRotate), typeof (double), typeof (ImageViewer), new PropertyMetadata(ValueBoxes.Double0Box));
    ImageViewer.ShowSmallImgInternalProperty = DependencyProperty.Register(nameof (ShowSmallImgInternal), typeof (bool), typeof (ImageViewer), new PropertyMetadata(ValueBoxes.FalseBox));
  }
}
