// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Magnifier
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class Magnifier : Control
{
  private const int CdefaultDPI = 96 /*0x60*/;
  private bool m_bIsAdded;
  private AdornerLayer m_adornerLayer;
  private MagnifierAdorner mAdorner;
  private static readonly bool c_isInDesignMode;
  public static readonly DependencyProperty FrameTypeProperty = DependencyProperty.Register(nameof (FrameType), typeof (FrameType), typeof (Magnifier), (PropertyMetadata) new FrameworkPropertyMetadata((object) FrameType.Rectangle, new PropertyChangedCallback(Magnifier.OnFrameTypeChanged)));
  public static readonly DependencyProperty FrameHeightProperty = DependencyProperty.Register(nameof (FrameHeight), typeof (double), typeof (Magnifier), (PropertyMetadata) new FrameworkPropertyMetadata((object) 200.0, new PropertyChangedCallback(Magnifier.OnFrameHeightChanged)));
  public static readonly DependencyProperty FrameWidthProperty = DependencyProperty.Register(nameof (FrameWidth), typeof (double), typeof (Magnifier), (PropertyMetadata) new FrameworkPropertyMetadata((object) 200.0, new PropertyChangedCallback(Magnifier.OnFrameWidthChanged)));
  public static readonly DependencyProperty FrameRadiusProperty = DependencyProperty.Register(nameof (FrameRadius), typeof (double), typeof (Magnifier), (PropertyMetadata) new FrameworkPropertyMetadata((object) 100.0, new PropertyChangedCallback(Magnifier.OnFrameRadiusChanged)));
  public static readonly DependencyProperty FrameCornerRadiusProperty = DependencyProperty.Register(nameof (FrameCornerRadius), typeof (double), typeof (Magnifier), (PropertyMetadata) new FrameworkPropertyMetadata((object) 5.0, new PropertyChangedCallback(Magnifier.OnFrameCornerRadiusChanged)));
  public static readonly DependencyProperty FrameBackgroundProperty = DependencyProperty.Register(nameof (FrameBackground), typeof (Brush), typeof (Magnifier), (PropertyMetadata) new FrameworkPropertyMetadata((object) new SolidColorBrush(), new PropertyChangedCallback(Magnifier.OnFrameBackgroundChanged)));
  public static readonly DependencyProperty ZoomFactorProperty = DependencyProperty.Register(nameof (ZoomFactor), typeof (double), typeof (Magnifier), (PropertyMetadata) new FrameworkPropertyMetadata((object) 1.0, new PropertyChangedCallback(Magnifier.OnZoomFactorChanged), new CoerceValueCallback(Magnifier.CoerceZoomFactor)));
  public static readonly DependencyProperty EnableExportProperty = DependencyProperty.Register(nameof (EnableExport), typeof (bool), typeof (Magnifier), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, new PropertyChangedCallback(Magnifier.OnEnableExportChanged)));
  public static readonly DependencyProperty TargetElementProperty = DependencyProperty.Register(nameof (TargetElement), typeof (UIElement), typeof (Magnifier), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(Magnifier.OnTargetElementChanged), new CoerceValueCallback(Magnifier.CoerceTargetElement)));
  public static readonly DependencyProperty CurrentProperty = DependencyProperty.RegisterAttached("Current", typeof (Magnifier), typeof (Magnifier), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(Magnifier.OnCurrentChanged)));
  public static readonly DependencyProperty ViewboxProperty = DependencyProperty.Register(nameof (Viewbox), typeof (Rect), typeof (Magnifier), (PropertyMetadata) new FrameworkPropertyMetadata((object) Rect.Empty));
  public static readonly DependencyProperty BackgroundWidthProperty = DependencyProperty.Register(nameof (BackgroundWidth), typeof (double), typeof (Magnifier));
  public static readonly DependencyProperty BackgroundHeightProperty = DependencyProperty.Register(nameof (BackgroundHeight), typeof (double), typeof (Magnifier));
  public static readonly DependencyProperty ActualTargetElementProperty = DependencyProperty.Register(nameof (ActualTargetElement), typeof (UIElement), typeof (Magnifier));

  static Magnifier()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (Magnifier), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (Magnifier)));
    Magnifier.c_isInDesignMode = (bool) DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof (DependencyObject)).DefaultValue;
  }

  public Magnifier()
  {
    this.UpdateTemplateDependentProperties();
    this.SizeChanged += new SizeChangedEventHandler(this.Magnifier_SizeChanged);
  }

  public FrameType FrameType
  {
    get => (FrameType) this.GetValue(Magnifier.FrameTypeProperty);
    set => this.SetValue(Magnifier.FrameTypeProperty, (object) value);
  }

  public double FrameHeight
  {
    get => (double) this.GetValue(Magnifier.FrameHeightProperty);
    set => this.SetValue(Magnifier.FrameHeightProperty, (object) value);
  }

  public double FrameWidth
  {
    get => (double) this.GetValue(Magnifier.FrameWidthProperty);
    set => this.SetValue(Magnifier.FrameWidthProperty, (object) value);
  }

  public double FrameRadius
  {
    get => (double) this.GetValue(Magnifier.FrameRadiusProperty);
    set => this.SetValue(Magnifier.FrameRadiusProperty, (object) value);
  }

  public double FrameCornerRadius
  {
    get => (double) this.GetValue(Magnifier.FrameCornerRadiusProperty);
    set => this.SetValue(Magnifier.FrameCornerRadiusProperty, (object) value);
  }

  public Brush FrameBackground
  {
    get => (Brush) this.GetValue(Magnifier.FrameBackgroundProperty);
    set => this.SetValue(Magnifier.FrameBackgroundProperty, (object) value);
  }

  public double ZoomFactor
  {
    get => (double) this.GetValue(Magnifier.ZoomFactorProperty);
    set => this.SetValue(Magnifier.ZoomFactorProperty, (object) value);
  }

  public bool EnableExport
  {
    get => (bool) this.GetValue(Magnifier.EnableExportProperty);
    set => this.SetValue(Magnifier.EnableExportProperty, (object) value);
  }

  public UIElement TargetElement
  {
    get => (UIElement) this.GetValue(Magnifier.TargetElementProperty);
    set => this.SetValue(Magnifier.TargetElementProperty, (object) value);
  }

  internal Rect Viewbox
  {
    get => (Rect) this.GetValue(Magnifier.ViewboxProperty);
    set => this.SetValue(Magnifier.ViewboxProperty, (object) value);
  }

  internal Size CurrentSize
  {
    get
    {
      switch (this.FrameType)
      {
        case FrameType.Rectangle:
        case FrameType.RoundedRectangle:
          return new Size(this.FrameWidth, this.FrameHeight);
        case FrameType.Circle:
          return new Size(2.0 * this.FrameRadius, 2.0 * this.FrameRadius);
        default:
          return Size.Empty;
      }
    }
  }

  internal double BackgroundWidth
  {
    get => (double) this.GetValue(Magnifier.BackgroundWidthProperty);
    set => this.SetValue(Magnifier.BackgroundWidthProperty, (object) value);
  }

  internal double BackgroundHeight
  {
    get => (double) this.GetValue(Magnifier.BackgroundHeightProperty);
    set => this.SetValue(Magnifier.BackgroundHeightProperty, (object) value);
  }

  internal UIElement ActualTargetElement
  {
    get => (UIElement) this.GetValue(Magnifier.ActualTargetElementProperty);
    set => this.SetValue(Magnifier.ActualTargetElementProperty, (object) value);
  }

  public event PropertyChangedCallback FrameTypeChanged;

  public event PropertyChangedCallback FrameHeightChanged;

  public event PropertyChangedCallback FrameWidthChanged;

  public event PropertyChangedCallback FrameRadiusChanged;

  public event PropertyChangedCallback FrameCornerRadiusChanged;

  public event PropertyChangedCallback FrameBackgroundChanged;

  public event PropertyChangedCallback ZoomFactorChanged;

  public event PropertyChangedCallback EnableExportChanged;

  public event PropertyChangedCallback TargetElementChanged;

  protected internal event CoerceValueCallback TargetElementChanging;

  private static void OnFrameTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((Magnifier) d).OnFrameTypeChanged(e);
  }

  protected virtual void OnFrameTypeChanged(DependencyPropertyChangedEventArgs e)
  {
    this.UpdateTemplateDependentProperties();
    if (this.FrameTypeChanged == null)
      return;
    this.FrameTypeChanged((DependencyObject) this, e);
  }

  private static void OnFrameHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((Magnifier) d).OnFrameHeightChanged(e);
  }

  protected virtual void OnFrameHeightChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.FrameType == FrameType.Rectangle || this.FrameType == FrameType.RoundedRectangle)
      this.UpdateTemplateDependentProperties();
    if (this.FrameHeightChanged == null)
      return;
    this.FrameHeightChanged((DependencyObject) this, e);
  }

  private static void OnFrameWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((Magnifier) d).OnFrameWidthChanged(e);
  }

  protected virtual void OnFrameWidthChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.FrameType == FrameType.Rectangle || this.FrameType == FrameType.RoundedRectangle)
      this.UpdateTemplateDependentProperties();
    if (this.FrameWidthChanged == null)
      return;
    this.FrameWidthChanged((DependencyObject) this, e);
  }

  private static void OnFrameRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((Magnifier) d).OnFrameRadiusChanged(e);
  }

  protected virtual void OnFrameRadiusChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.FrameType == FrameType.Circle)
      this.UpdateTemplateDependentProperties();
    if (this.FrameRadiusChanged == null)
      return;
    this.FrameRadiusChanged((DependencyObject) this, e);
  }

  private static void OnFrameCornerRadiusChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Magnifier) d).OnFrameCornerRadiusChanged(e);
  }

  protected virtual void OnFrameCornerRadiusChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.FrameCornerRadiusChanged == null)
      return;
    this.FrameCornerRadiusChanged((DependencyObject) this, e);
  }

  private static void OnFrameBackgroundChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Magnifier) d).OnFrameBackgroundChanged(e);
  }

  protected virtual void OnFrameBackgroundChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.FrameBackgroundChanged == null)
      return;
    this.FrameBackgroundChanged((DependencyObject) this, e);
  }

  private static void OnZoomFactorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((Magnifier) d).OnZoomFactorChanged(e);
  }

  protected virtual void OnZoomFactorChanged(DependencyPropertyChangedEventArgs e)
  {
    this.UpdateTemplateDependentProperties();
    if (this.ZoomFactorChanged == null)
      return;
    this.ZoomFactorChanged((DependencyObject) this, e);
  }

  private static object CoerceZoomFactor(DependencyObject d, object baseValue)
  {
    return ((Magnifier) d).CoerceZoomFactor(baseValue);
  }

  protected virtual object CoerceZoomFactor(object baseValue)
  {
    double num = (double) baseValue;
    if (num > 1.0)
      return (object) 1;
    return num < 0.0 ? (object) 0 : baseValue;
  }

  private static void OnEnableExportChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Magnifier) d).OnEnableExportChanged(e);
  }

  protected virtual void OnEnableExportChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.EnableExportChanged == null)
      return;
    this.EnableExportChanged((DependencyObject) this, e);
  }

  private static void OnTargetElementChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Magnifier) d).OnTargetElementChanged(e);
  }

  protected virtual void OnTargetElementChanged(DependencyPropertyChangedEventArgs e)
  {
    this.ChangeTargetElement(e.OldValue as UIElement, e.NewValue as UIElement);
    if (this.TargetElementChanged == null)
      return;
    this.TargetElementChanged((DependencyObject) this, e);
  }

  private static object CoerceTargetElement(DependencyObject d, object baseValue)
  {
    return ((Magnifier) d).CoerceTargetElement(baseValue);
  }

  protected virtual object CoerceTargetElement(object baseValue)
  {
    if (this.TargetElementChanging != null)
    {
      object obj = this.TargetElementChanging((DependencyObject) this, baseValue);
    }
    return baseValue;
  }

  private static void OnCurrentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is UIElement uiElement))
      throw new ArgumentException("incorrect type");
    if (e.OldValue != null)
      (e.OldValue as Magnifier).TargetElement = (UIElement) null;
    if (e.NewValue == null)
      return;
    (e.NewValue as Magnifier).TargetElement = uiElement;
  }

  public void AssociateWith(UIElement target) => this.TargetElement = target;

  public void ZoomIn(double zoomFactor) => this.ZoomFactor /= zoomFactor;

  public void ZoomOut(double zoomFactor) => this.ZoomFactor *= zoomFactor;

  public static void SetCurrent(DependencyObject d, Magnifier magnifier)
  {
    d.SetValue(Magnifier.CurrentProperty, (object) magnifier);
  }

  public static Magnifier GetCurrent(DependencyObject d)
  {
    return d.GetValue(Magnifier.CurrentProperty) as Magnifier;
  }

  public void Save(Stream stream, BitmapEncoder encoder)
  {
    if (!this.EnableExport)
      return;
    if (!(this.Template.FindName("PART_MagnifierArea", (FrameworkElement) this) is Visual name))
      throw new ArgumentNullException("visual");
    RenderTargetBitmap source = new RenderTargetBitmap((int) this.CurrentSize.Width, (int) this.CurrentSize.Height, 96.0, 96.0, PixelFormats.Default);
    Rectangle rectangle = new Rectangle();
    rectangle.Fill = (Brush) Brushes.Transparent;
    rectangle.Arrange(new Rect(this.RenderSize));
    source.Render((Visual) rectangle);
    source.Render(name);
    encoder.Frames.Add(BitmapFrame.Create((BitmapSource) source));
    encoder.Save(stream);
  }

  public void Save(Stream stream)
  {
    if (!this.EnableExport)
      return;
    this.Save(stream, (BitmapEncoder) new BmpBitmapEncoder());
  }

  public void Save(string fileName)
  {
    if (!this.EnableExport)
      return;
    string lower = new FileInfo(fileName).Extension.ToLower(CultureInfo.InvariantCulture);
    using (Stream stream = (Stream) File.Create(fileName))
    {
      if (lower == ".xps")
        this.SaveToXps(stream);
      else
        this.Save(stream, this.CreateEncoderByExtension(lower));
    }
  }

  public void Save(string fileName, BitmapEncoder encoder)
  {
    if (!this.EnableExport)
      return;
    this.Save((Stream) File.Create(fileName), encoder);
  }

  public void SaveToXps(Stream stream)
  {
    if (!this.EnableExport)
      return;
    if (!(this.Template.FindName("PART_MagnifierArea", (FrameworkElement) this) is Visual name))
      throw new ArgumentNullException("visual");
    Package package = Package.Open(stream, FileMode.Create, FileAccess.ReadWrite);
    XpsDocument xpsDocument = new XpsDocument(package, CompressionOption.Normal);
    XpsDocument.CreateXpsDocumentWriter(xpsDocument).Write(name);
    xpsDocument.Close();
    package.Close();
  }

  public void SaveToXps(string filename)
  {
    if (!this.EnableExport)
      return;
    this.SaveToXps((Stream) File.Create(filename));
  }

  public void CopyToClipboard()
  {
    if (!this.EnableExport)
      return;
    if (!(this.Template.FindName("PART_MagnifierArea", (FrameworkElement) this) is Visual name))
      throw new ArgumentNullException("visual");
    RenderTargetBitmap image = new RenderTargetBitmap((int) this.CurrentSize.Width, (int) this.CurrentSize.Height, 96.0, 96.0, PixelFormats.Default);
    Rectangle rectangle = new Rectangle();
    rectangle.Fill = (Brush) Brushes.White;
    rectangle.Arrange(new Rect(this.RenderSize));
    image.Render((Visual) rectangle);
    image.Render(name);
    Clipboard.SetImage((BitmapSource) image);
  }

  protected override void OnVisualParentChanged(DependencyObject oldParent)
  {
    if (!(this.VisualParent is MagnifierAdorner) && this.VisualParent is Panel)
      (this.VisualParent as Panel).Children.Remove((UIElement) this);
    base.OnVisualParentChanged(oldParent);
  }

  private void ChangeTargetElement(UIElement uieOld, UIElement uieNew)
  {
    if (uieOld != null)
    {
      if (this.mAdorner != null && this.m_adornerLayer != null)
      {
        this.m_adornerLayer.Remove((Adorner) this.mAdorner);
        this.mAdorner.DisconnectMagnifier();
      }
      if (this.ActualTargetElement != null)
      {
        this.ActualTargetElement.MouseEnter -= new MouseEventHandler(this.TargetElement_MouseEnter);
        this.ActualTargetElement.MouseLeave -= new MouseEventHandler(this.TargetElement_MouseLeave);
      }
    }
    if (uieNew == null)
      return;
    this.m_adornerLayer = AdornerLayer.GetAdornerLayer((Visual) uieNew);
    if (this.m_adornerLayer == null)
    {
      uieNew.LayoutUpdated += new EventHandler(this.TargetElement_LayoutUpdated);
    }
    else
    {
      this.UpdateActualTargetElement();
      this.mAdorner = new MagnifierAdorner(this.ActualTargetElement, this);
      this.m_adornerLayer.Add((Adorner) this.mAdorner);
      this.ActualTargetElement.MouseEnter += new MouseEventHandler(this.TargetElement_MouseEnter);
      this.ActualTargetElement.MouseLeave += new MouseEventHandler(this.TargetElement_MouseLeave);
    }
    if (this.ActualTargetElement != null && this.ActualTargetElement.IsMouseOver && !Magnifier.c_isInDesignMode)
      this.Visibility = Visibility.Visible;
    else
      this.Visibility = Visibility.Hidden;
  }

  private void TargetElement_MouseLeave(object sender, MouseEventArgs e)
  {
    this.Visibility = Visibility.Hidden;
  }

  private void TargetElement_MouseEnter(object sender, MouseEventArgs e)
  {
    this.Visibility = Visibility.Visible;
  }

  private void TargetElement_LayoutUpdated(object sender, EventArgs e)
  {
    if (this.m_bIsAdded || Magnifier.c_isInDesignMode)
      return;
    if (this.m_adornerLayer == null)
    {
      this.UpdateActualTargetElement();
      this.m_adornerLayer = AdornerLayer.GetAdornerLayer((Visual) this.ActualTargetElement);
      this.mAdorner = new MagnifierAdorner(this.ActualTargetElement, this);
      this.m_adornerLayer.Add((Adorner) this.mAdorner);
      this.ActualTargetElement.MouseEnter += new MouseEventHandler(this.TargetElement_MouseEnter);
      this.ActualTargetElement.MouseLeave += new MouseEventHandler(this.TargetElement_MouseLeave);
    }
    this.m_bIsAdded = true;
  }

  private void Magnifier_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    if (!(e.NewSize != this.CurrentSize))
      return;
    this.Width = this.CurrentSize.Width;
    this.Height = this.CurrentSize.Height;
  }

  private void UpdateTemplateDependentProperties()
  {
    double num = this.BorderThickness.Bottom + this.BorderThickness.Left + this.BorderThickness.Right + this.BorderThickness.Top == 0.0 ? 1.0 : 0.0;
    double width = this.CurrentSize.Width * this.ZoomFactor;
    double height = this.CurrentSize.Height * this.ZoomFactor;
    this.BackgroundWidth = this.CurrentSize.Width - num;
    this.BackgroundHeight = this.CurrentSize.Height - num;
    this.Viewbox = new Rect(this.Viewbox.Location, new Size(width, height));
    if (this.mAdorner == null)
      return;
    this.mAdorner.UpdateMagnifierViewbox();
  }

  private BitmapEncoder CreateEncoderByExtension(string extension)
  {
    BitmapEncoder encoderByExtension;
    switch (extension)
    {
      case ".bmp":
        encoderByExtension = (BitmapEncoder) new BmpBitmapEncoder();
        break;
      case ".jpg":
      case ".jpeg":
        encoderByExtension = (BitmapEncoder) new JpegBitmapEncoder();
        break;
      case ".png":
        encoderByExtension = (BitmapEncoder) new PngBitmapEncoder();
        break;
      case ".gif":
        encoderByExtension = (BitmapEncoder) new GifBitmapEncoder();
        break;
      case ".tif":
      case ".tiff":
        encoderByExtension = (BitmapEncoder) new TiffBitmapEncoder();
        break;
      case ".wdp":
        encoderByExtension = (BitmapEncoder) new WmpBitmapEncoder();
        break;
      default:
        encoderByExtension = (BitmapEncoder) new BmpBitmapEncoder();
        break;
    }
    return encoderByExtension;
  }

  private void UpdateActualTargetElement()
  {
    UIElement uiElement = (UIElement) null;
    if (this.TargetElement is Window targetElement)
    {
      AdornerDecorator adornerDecorator = this.FindAdornerDecorator((UIElement) targetElement);
      if (adornerDecorator != null)
      {
        int childIndex = 0;
        if (childIndex < VisualTreeHelper.GetChildrenCount((DependencyObject) adornerDecorator))
          uiElement = VisualTreeHelper.GetChild((DependencyObject) adornerDecorator, childIndex) as UIElement;
      }
    }
    else
      uiElement = this.TargetElement;
    this.ActualTargetElement = uiElement;
  }

  private AdornerDecorator FindAdornerDecorator(UIElement rootElement)
  {
    AdornerDecorator adornerDecorator = (AdornerDecorator) null;
    for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount((DependencyObject) rootElement); ++childIndex)
    {
      UIElement child = VisualTreeHelper.GetChild((DependencyObject) rootElement, childIndex) as UIElement;
      if (child is AdornerDecorator)
      {
        adornerDecorator = child as AdornerDecorator;
        break;
      }
      if (child != null)
        adornerDecorator = this.FindAdornerDecorator(child);
    }
    return adornerDecorator;
  }
}
