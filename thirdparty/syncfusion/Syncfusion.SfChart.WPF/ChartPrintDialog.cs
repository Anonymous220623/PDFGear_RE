// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartPrintDialog
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartPrintDialog : Window, IComponentConnector
{
  private FrameworkElement m_elementToPrint;
  private PrintDialog m_nativePrintDialog = new PrintDialog();
  private VisualBrush m_visualBrush;
  private double ChartHeight;
  private double ChartWidth;
  public static readonly DependencyProperty PrintStretchProperty = DependencyProperty.Register(nameof (PrintStretch), typeof (Stretch), typeof (ChartPrintDialog), (PropertyMetadata) new FrameworkPropertyMetadata((object) Stretch.Uniform, new PropertyChangedCallback(ChartPrintDialog.OnPrintStretchChanged)));
  public static readonly DependencyProperty PrintModeProperty = DependencyProperty.Register(nameof (PrintMode), typeof (ChartPrintMode), typeof (ChartPrintDialog), (PropertyMetadata) new FrameworkPropertyMetadata((object) ChartPrintMode.Portrait, new PropertyChangedCallback(ChartPrintDialog.OnPrintModeChanged)));
  internal static readonly DependencyProperty BorderStyleProperty = DependencyProperty.Register(nameof (BorderStyle), typeof (Style), typeof (ChartPrintDialog), (PropertyMetadata) null);
  internal static readonly DependencyProperty DashedBorderStyleProperty = DependencyProperty.Register(nameof (DashedBorderStyle), typeof (Style), typeof (ChartPrintDialog), (PropertyMetadata) null);
  internal Rectangle PreviewRect;
  internal RadioButton colorMode;
  private bool _contentLoaded;

  public ChartPrintMode PrintMode
  {
    get => (ChartPrintMode) this.GetValue(ChartPrintDialog.PrintModeProperty);
    set => this.SetValue(ChartPrintDialog.PrintModeProperty, (object) value);
  }

  public Stretch PrintStretch
  {
    get => (Stretch) this.GetValue(ChartPrintDialog.PrintStretchProperty);
    set => this.SetValue(ChartPrintDialog.PrintStretchProperty, (object) value);
  }

  internal Style BorderStyle
  {
    get => (Style) this.GetValue(ChartPrintDialog.BorderStyleProperty);
    set => this.SetValue(ChartPrintDialog.BorderStyleProperty, (object) value);
  }

  internal Style DashedBorderStyle
  {
    get => (Style) this.GetValue(ChartPrintDialog.DashedBorderStyleProperty);
    set => this.SetValue(ChartPrintDialog.DashedBorderStyleProperty, (object) value);
  }

  public event PropertyChangedCallback PrintStretchChanged;

  public event PropertyChangedCallback PrintModeChanged;

  public ChartPrintDialog() => this.InitializeComponent();

  public bool? ShowPrintDialog(FrameworkElement element)
  {
    return this.ShowPrintDialog(element, Rect.Empty, 335.0, 252.0);
  }

  public virtual Rectangle GetPrintVisual(FrameworkElement element)
  {
    this.m_elementToPrint = element;
    VisualBrush visualBrush = new VisualBrush(this.CloneVisualState(element));
    this.ChartHeight = element.ActualHeight;
    this.ChartWidth = element.ActualWidth;
    visualBrush.Stretch = Stretch.Uniform;
    visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
    visualBrush.Viewbox = new Rect(0.0, 0.0, this.m_elementToPrint.ActualWidth, this.m_elementToPrint.ActualHeight);
    this.m_visualBrush = visualBrush;
    Rectangle printVisual = new Rectangle();
    printVisual.Height = this.ChartHeight;
    printVisual.Width = this.ChartWidth;
    printVisual.Fill = (Brush) this.m_visualBrush;
    return printVisual;
  }

  public virtual bool? ShowPrintDialog(
    FrameworkElement element,
    Rect printArea,
    double elem_height,
    double elem_width)
  {
    this.m_elementToPrint = element;
    this.ChartHeight = element.ActualHeight;
    this.ChartWidth = element.ActualWidth;
    VisualBrush visualBrush = new VisualBrush(this.CloneVisualState(element));
    visualBrush.Stretch = Stretch.Uniform;
    visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
    visualBrush.Viewbox = printArea.IsEmpty ? new Rect(0.0, 0.0, this.m_elementToPrint.ActualWidth, this.m_elementToPrint.ActualHeight) : printArea;
    this.m_visualBrush = visualBrush;
    this.PreviewRect.Fill = (Brush) visualBrush;
    return this.ShowDialog();
  }

  private void StartPrint()
  {
    PrintCapabilities printCapabilities = this.m_nativePrintDialog.PrintQueue.GetPrintCapabilities(this.m_nativePrintDialog.PrintTicket);
    Size size1 = new Size(this.m_nativePrintDialog.PrintableAreaWidth, this.m_nativePrintDialog.PrintableAreaHeight);
    Size size2 = new Size(printCapabilities.PageImageableArea.ExtentWidth, printCapabilities.PageImageableArea.ExtentHeight);
    Rectangle rectangle = new Rectangle();
    rectangle.Fill = (Brush) this.m_visualBrush;
    this.SetViewport(this.m_visualBrush, size2);
    rectangle.Arrange(new Rect(new Point(0.0, 0.0), size2));
    RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int) size2.Width, (int) size2.Height, 96.0, 96.0, PixelFormats.Default);
    renderTargetBitmap.Render((Visual) rectangle);
    FormatConvertedBitmap image1 = new FormatConvertedBitmap();
    image1.BeginInit();
    image1.Source = (BitmapSource) renderTargetBitmap;
    bool? isChecked1 = this.colorMode.IsChecked;
    int num = !isChecked1.GetValueOrDefault() ? 0 : (isChecked1.HasValue ? 1 : 0);
    image1.DestinationFormat = num == 0 ? PixelFormats.Gray32Float : PixelFormats.Default;
    image1.EndInit();
    Image image2 = new Image();
    image2.Height = (double) (int) size2.Height;
    image2.Width = (double) (int) size2.Width;
    image2.Source = (ImageSource) image1;
    bool? isChecked2 = this.colorMode.IsChecked;
    if ((isChecked2.GetValueOrDefault() ? 0 : (isChecked2.HasValue ? 1 : 0)) != 0)
      rectangle.Fill = (Brush) new ImageBrush((ImageSource) image1);
    PrintQueue.CreateXpsDocumentWriter(this.m_nativePrintDialog.PrintQueue).Write((Visual) rectangle, this.m_nativePrintDialog.PrintTicket);
    this.SetViewport(this.m_visualBrush, new Size(this.PreviewRect.ActualWidth, this.PreviewRect.ActualHeight));
  }

  private void OnPrintClick(object sender, RoutedEventArgs args)
  {
    this.StartPrint();
    this.DialogResult = new bool?(true);
  }

  private void OnCancelClick(object sender, RoutedEventArgs args)
  {
    this.DialogResult = new bool?(false);
  }

  private void OnColorClick(object sender, RoutedEventArgs args) => this.RefreshRect();

  private void OnBlackAndWhiteClick(object sender, RoutedEventArgs args) => this.RefreshRect();

  private void OnPrintStretchChanged(object sender, RoutedEventArgs args) => this.RefreshRect();

  private void OnPrintModeChanged(object sender, RoutedEventArgs args) => this.RefreshRect();

  private void RefreshRect()
  {
    PrintCapabilities printCapabilities = this.m_nativePrintDialog.PrintQueue.GetPrintCapabilities(this.m_nativePrintDialog.PrintTicket);
    Size size1 = new Size(this.m_nativePrintDialog.PrintableAreaWidth, this.m_nativePrintDialog.PrintableAreaHeight);
    Size size2 = new Size(printCapabilities.PageImageableArea.ExtentWidth, printCapabilities.PageImageableArea.ExtentHeight);
    Rectangle rectangle = new Rectangle();
    rectangle.Fill = (Brush) this.m_visualBrush;
    this.SetViewport(this.m_visualBrush, size2);
    rectangle.Arrange(new Rect(new Point(0.0, 0.0), size2));
    RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int) size2.Width, (int) size2.Height, 96.0, 96.0, PixelFormats.Default);
    renderTargetBitmap.Render((Visual) rectangle);
    FormatConvertedBitmap image1 = new FormatConvertedBitmap();
    image1.BeginInit();
    image1.Source = (BitmapSource) renderTargetBitmap;
    bool? isChecked = this.colorMode.IsChecked;
    int num = !isChecked.GetValueOrDefault() ? 0 : (isChecked.HasValue ? 1 : 0);
    image1.DestinationFormat = num == 0 ? PixelFormats.Gray32Float : PixelFormats.Default;
    image1.EndInit();
    Image image2 = new Image();
    image2.Height = (double) (int) size2.Height;
    image2.Width = (double) (int) size2.Width;
    image2.Source = (ImageSource) image1;
    this.PreviewRect.Fill = (Brush) new ImageBrush((ImageSource) image1);
  }

  private void OnAdvancedClick(object sender, RoutedEventArgs args)
  {
    this.m_nativePrintDialog.ShowDialog();
  }

  private static Size GetPrintSize(Stretch stretch, Size viewport, Size original)
  {
    Size printSize = Size.Empty;
    switch (stretch)
    {
      case Stretch.None:
        printSize = original;
        break;
      case Stretch.Fill:
        printSize = viewport;
        break;
      case Stretch.Uniform:
        double num1 = viewport.Width / original.Width;
        double num2 = viewport.Height / original.Height;
        printSize = num1 >= num2 ? new Size(num2 * original.Width, viewport.Height) : new Size(viewport.Width, num1 * original.Height);
        break;
      case Stretch.UniformToFill:
        double num3 = viewport.Width / original.Width;
        double num4 = viewport.Height / original.Height;
        printSize = num3 <= num4 ? new Size(num4 * original.Width, viewport.Height) : new Size(viewport.Width, num3 * original.Height);
        break;
    }
    return printSize;
  }

  private static void OnPrintStretchChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ChartPrintDialog) d).OnPrintStretchChanged(e);
  }

  private static void OnPrintModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ChartPrintDialog) d).OnPrintModeChanged(e);
  }

  private void SetViewport(VisualBrush brush, Size size)
  {
    if (brush == null)
      throw new ArgumentNullException(nameof (brush));
    if (this.PrintStretch == Stretch.Uniform)
    {
      double num1 = size.Height / brush.Viewbox.Height;
      double num2 = size.Width / brush.Viewbox.Width;
      if (num1 < num2)
      {
        double width = num1 * brush.Viewbox.Width / size.Width;
        double x = (1.0 - width) / 2.0;
        brush.Viewport = new Rect(new Point(x, 0.0), new Size(width, 1.0));
      }
      else
      {
        if (num1 <= num2)
          return;
        double height = num2 * brush.Viewbox.Height / size.Height;
        double y = (1.0 - height) / 2.0;
        brush.Viewport = new Rect(new Point(0.0, y), new Size(1.0, height));
      }
    }
    else if (this.PrintStretch == Stretch.None)
    {
      if (size.Width > brush.Viewbox.Width || size.Height > brush.Viewbox.Height)
      {
        double width = brush.Viewbox.Width / size.Width;
        double height = brush.Viewbox.Height / size.Height;
        double x = (1.0 - width) / 2.0;
        double y = (1.0 - height) / 2.0;
        brush.Viewport = new Rect(new Point(x, y), new Size(width, height));
      }
      else
        brush.Viewport = new Rect(0.0, 0.0, 1.0, 1.0);
    }
    else
      brush.Viewport = new Rect(0.0, 0.0, 1.0, 1.0);
  }

  protected virtual void OnPrintStretchChanged(DependencyPropertyChangedEventArgs e)
  {
    this.m_visualBrush.Stretch = this.PrintStretch;
    this.SetViewport(this.m_visualBrush, new Size(this.PreviewRect.ActualWidth, this.PreviewRect.ActualHeight));
    if (this.PrintStretchChanged == null)
      return;
    this.PrintStretchChanged((DependencyObject) this, e);
  }

  protected virtual void OnPrintModeChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.PrintMode == ChartPrintMode.Landscape)
    {
      this.m_visualBrush.RelativeTransform = (Transform) new TransformGroup()
      {
        Children = {
          (Transform) new RotateTransform()
          {
            Angle = 90.0,
            CenterX = 0.5,
            CenterY = 0.5
          }
        }
      };
      this.SetViewport(this.m_visualBrush, new Size(this.PreviewRect.ActualWidth, this.PreviewRect.ActualHeight));
    }
    else
    {
      this.m_visualBrush.RelativeTransform = (Transform) new TransformGroup()
      {
        Children = {
          (Transform) new RotateTransform()
          {
            Angle = 0.0,
            CenterX = 0.5,
            CenterY = 0.5
          }
        }
      };
      this.SetViewport(this.m_visualBrush, new Size(this.PreviewRect.ActualWidth, this.PreviewRect.ActualHeight));
    }
    if (this.PrintModeChanged == null)
      return;
    this.PrintModeChanged((DependencyObject) this, e);
  }

  protected override void OnRender(DrawingContext drawingContext)
  {
    base.OnRender(drawingContext);
    this.SetViewport(this.m_visualBrush, new Size(this.PreviewRect.ActualWidth, this.PreviewRect.ActualHeight));
  }

  public Rect GetUIElementBounds(UIElement element)
  {
    return new Rect((Point) VisualTreeHelper.GetOffset((Visual) element), element.DesiredSize);
  }

  public Visual CloneVisualState(FrameworkElement targetElement)
  {
    DrawingVisual drawingVisual = new DrawingVisual();
    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
    {
      VisualBrush visualBrush1 = new VisualBrush((Visual) targetElement);
      visualBrush1.Stretch = Stretch.None;
      visualBrush1.AlignmentX = AlignmentX.Left;
      visualBrush1.AlignmentY = AlignmentY.Top;
      VisualBrush visualBrush2 = visualBrush1;
      drawingContext.DrawRectangle((Brush) visualBrush2, (Pen) null, new Rect(0.0, 0.0, targetElement.ActualWidth, targetElement.ActualHeight));
    }
    return (Visual) drawingVisual;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/Syncfusion.SfChart.WPF;component/utils/chartprintdialog.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.PreviewRect = (Rectangle) target;
        break;
      case 2:
        this.colorMode = (RadioButton) target;
        this.colorMode.Click += new RoutedEventHandler(this.OnColorClick);
        break;
      case 3:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.OnBlackAndWhiteClick);
        break;
      case 4:
        ((Selector) target).SelectionChanged += new SelectionChangedEventHandler(this.OnPrintStretchChanged);
        break;
      case 5:
        ((Selector) target).SelectionChanged += new SelectionChangedEventHandler(this.OnPrintModeChanged);
        break;
      case 6:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.OnAdvancedClick);
        break;
      case 7:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.OnPrintClick);
        break;
      case 8:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.OnCancelClick);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
