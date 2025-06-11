// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Watermark
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_Root", Type = typeof (Border))]
[System.Windows.Markup.ContentProperty("Content")]
public class Watermark : Control
{
  private const string ElementRoot = "PART_Root";
  private Border _borderRoot;
  private DrawingBrush _brush;
  public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(nameof (Angle), typeof (double), typeof (Watermark), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof (Content), typeof (object), typeof (Watermark), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MarkProperty = DependencyProperty.Register(nameof (Mark), typeof (object), typeof (Watermark), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty MarkWidthProperty = DependencyProperty.Register(nameof (MarkWidth), typeof (double), typeof (Watermark), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty MarkHeightProperty = DependencyProperty.Register(nameof (MarkHeight), typeof (double), typeof (Watermark), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty MarkBrushProperty = DependencyProperty.Register(nameof (MarkBrush), typeof (Brush), typeof (Watermark), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty AutoSizeEnabledProperty = DependencyProperty.Register(nameof (AutoSizeEnabled), typeof (bool), typeof (Watermark), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty MarkMarginProperty = DependencyProperty.Register(nameof (MarkMargin), typeof (Thickness), typeof (Watermark), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(), FrameworkPropertyMetadataOptions.AffectsRender));

  public double Angle
  {
    get => (double) this.GetValue(Watermark.AngleProperty);
    set => this.SetValue(Watermark.AngleProperty, (object) value);
  }

  public object Content
  {
    get => this.GetValue(Watermark.ContentProperty);
    set => this.SetValue(Watermark.ContentProperty, value);
  }

  public object Mark
  {
    get => this.GetValue(Watermark.MarkProperty);
    set => this.SetValue(Watermark.MarkProperty, value);
  }

  public double MarkWidth
  {
    get => (double) this.GetValue(Watermark.MarkWidthProperty);
    set => this.SetValue(Watermark.MarkWidthProperty, (object) value);
  }

  public double MarkHeight
  {
    get => (double) this.GetValue(Watermark.MarkHeightProperty);
    set => this.SetValue(Watermark.MarkHeightProperty, (object) value);
  }

  public Brush MarkBrush
  {
    get => (Brush) this.GetValue(Watermark.MarkBrushProperty);
    set => this.SetValue(Watermark.MarkBrushProperty, (object) value);
  }

  public bool AutoSizeEnabled
  {
    get => (bool) this.GetValue(Watermark.AutoSizeEnabledProperty);
    set => this.SetValue(Watermark.AutoSizeEnabledProperty, ValueBoxes.BooleanBox(value));
  }

  public Thickness MarkMargin
  {
    get => (Thickness) this.GetValue(Watermark.MarkMarginProperty);
    set => this.SetValue(Watermark.MarkMarginProperty, (object) value);
  }

  public override void OnApplyTemplate()
  {
    this._borderRoot = this.GetTemplateChild("PART_Root") as Border;
  }

  private void EnsureBrush()
  {
    ContentPresenter contentPresenter1 = new ContentPresenter();
    if (this.Mark is Geometry mark2)
    {
      ContentPresenter contentPresenter2 = contentPresenter1;
      Path path = new Path();
      path.Width = this.MarkWidth;
      path.Height = this.MarkHeight;
      path.Fill = this.MarkBrush;
      path.Stretch = Stretch.Uniform;
      path.Data = mark2;
      contentPresenter2.Content = (object) path;
    }
    else if (this.Mark is string mark1)
      contentPresenter1.Content = (object) new TextBlock()
      {
        Text = mark1,
        FontSize = this.FontSize,
        Foreground = this.MarkBrush
      };
    else
      contentPresenter1.Content = this.Mark;
    Size size;
    if (this.AutoSizeEnabled)
    {
      contentPresenter1.Measure(new Size(double.MaxValue, double.MaxValue));
      size = contentPresenter1.DesiredSize;
    }
    else
      size = new Size(this.MarkWidth, this.MarkHeight);
    DrawingBrush drawingBrush = new DrawingBrush();
    drawingBrush.ViewportUnits = BrushMappingMode.Absolute;
    drawingBrush.Stretch = Stretch.Uniform;
    drawingBrush.TileMode = TileMode.Tile;
    drawingBrush.Transform = (Transform) new RotateTransform(this.Angle);
    GeometryDrawing geometryDrawing = new GeometryDrawing();
    Border border = new Border();
    border.Background = (Brush) Brushes.Transparent;
    border.Padding = this.MarkMargin;
    border.Child = (UIElement) contentPresenter1;
    geometryDrawing.Brush = (Brush) new VisualBrush((Visual) border);
    geometryDrawing.Geometry = (Geometry) new RectangleGeometry(new Rect(size));
    drawingBrush.Drawing = (Drawing) geometryDrawing;
    drawingBrush.Viewport = new Rect(size);
    this._brush = drawingBrush;
    RenderOptions.SetCacheInvalidationThresholdMinimum((DependencyObject) this._brush, 0.5);
    RenderOptions.SetCacheInvalidationThresholdMaximum((DependencyObject) this._brush, 2.0);
    RenderOptions.SetCachingHint((DependencyObject) this._brush, CachingHint.Cache);
    if (this._borderRoot == null)
      return;
    this._borderRoot.Background = (Brush) this._brush;
  }

  protected override void OnRender(DrawingContext drawingContext) => this.EnsureBrush();
}
