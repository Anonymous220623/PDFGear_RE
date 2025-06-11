// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.Annotation
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class Annotation : FrameworkElement, ICloneable
{
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (Annotation), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(Annotation.OnTextChanged)));
  public static readonly DependencyProperty EnableEditingProperty = DependencyProperty.Register(nameof (EnableEditing), typeof (bool), typeof (Annotation), new PropertyMetadata((object) false, new PropertyChangedCallback(Annotation.OnEnableEditingChanged)));
  public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(nameof (ContentTemplate), typeof (DataTemplate), typeof (Annotation), new PropertyMetadata((object) null, new PropertyChangedCallback(Annotation.OnContentTemplateChanged)));
  public static readonly DependencyProperty EnableClippingProperty = DependencyProperty.Register(nameof (EnableClipping), typeof (bool), typeof (ShapeAnnotation), new PropertyMetadata((object) false, new PropertyChangedCallback(Annotation.OnEnableClippingPropertyChanged)));
  public static readonly DependencyProperty ShowToolTipProperty = DependencyProperty.Register(nameof (ShowToolTip), typeof (bool), typeof (Annotation), new PropertyMetadata((object) false));
  public static readonly DependencyProperty ToolTipContentProperty = DependencyProperty.Register(nameof (ToolTipContent), typeof (object), typeof (Annotation), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ToolTipShowDurationProperty = DependencyProperty.Register(nameof (ToolTipShowDuration), typeof (double), typeof (Annotation), new PropertyMetadata((object) double.NaN));
  public static readonly DependencyProperty ToolTipTemplateProperty = DependencyProperty.Register(nameof (ToolTipTemplate), typeof (DataTemplate), typeof (Annotation), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ToolTipPlacementProperty = DependencyProperty.Register(nameof (ToolTipPlacement), typeof (ToolTipLabelPlacement), typeof (Annotation), new PropertyMetadata((object) ToolTipLabelPlacement.Right));
  public static readonly DependencyProperty CoordinateUnitProperty = DependencyProperty.Register(nameof (CoordinateUnit), typeof (CoordinateUnit), typeof (Annotation), new PropertyMetadata((object) CoordinateUnit.Axis, new PropertyChangedCallback(Annotation.OnCoordinatePropertyChanged)));
  public static readonly DependencyProperty XAxisNameProperty = DependencyProperty.Register(nameof (XAxisName), typeof (string), typeof (Annotation), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(Annotation.OnAxisNameChanged)));
  public static readonly DependencyProperty YAxisNameProperty = DependencyProperty.Register(nameof (YAxisName), typeof (string), typeof (Annotation), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(Annotation.OnAxisNameChanged)));
  public static readonly DependencyProperty X1Property = DependencyProperty.Register(nameof (X1), typeof (object), typeof (Annotation), new PropertyMetadata((object) null, new PropertyChangedCallback(Annotation.OnUpdatePropertyChanged)));
  public static readonly DependencyProperty Y1Property = DependencyProperty.Register(nameof (Y1), typeof (object), typeof (Annotation), new PropertyMetadata((object) null, new PropertyChangedCallback(Annotation.OnUpdatePropertyChanged)));
  public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(nameof (FontSize), typeof (double), typeof (Annotation), new PropertyMetadata((object) 12.0));
  public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register(nameof (FontFamily), typeof (FontFamily), typeof (Annotation), new PropertyMetadata(TextBlock.FontFamilyProperty.GetMetadata(typeof (TextBlock)).DefaultValue));
  public static readonly DependencyProperty FontStretchProperty = DependencyProperty.Register(nameof (FontStretch), typeof (FontStretch), typeof (Annotation), new PropertyMetadata(TextBlock.FontStretchProperty.GetMetadata(typeof (TextBlock)).DefaultValue));
  public static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register(nameof (FontStyle), typeof (FontStyle), typeof (Annotation), new PropertyMetadata(TextBlock.FontStyleProperty.GetMetadata(typeof (TextBlock)).DefaultValue));
  public static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register(nameof (FontWeight), typeof (FontWeight), typeof (Annotation), new PropertyMetadata(TextBlock.FontWeightProperty.GetMetadata(typeof (TextBlock)).DefaultValue));
  public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(nameof (Foreground), typeof (Brush), typeof (Annotation), new PropertyMetadata((PropertyChangedCallback) null));
  internal static readonly DependencyProperty InternalHorizontalAlignmentProperty = DependencyProperty.Register(nameof (InternalHorizontalAlignment), typeof (HorizontalAlignment), typeof (Annotation), new PropertyMetadata((object) HorizontalAlignment.Right, new PropertyChangedCallback(Annotation.OnUpdatePropertyChanged)));
  internal static readonly DependencyProperty InternalVerticalAlignmentProperty = DependencyProperty.Register(nameof (InternalVerticalAlignment), typeof (VerticalAlignment), typeof (Annotation), new PropertyMetadata((object) VerticalAlignment.Bottom, new PropertyChangedCallback(Annotation.OnUpdatePropertyChanged)));
  internal static readonly DependencyProperty InternalVisibilityProperty = DependencyProperty.Register(nameof (InternalVisibility), typeof (Visibility), typeof (Annotation), new PropertyMetadata((object) Visibility.Visible, new PropertyChangedCallback(Annotation.OnVisibiltyChanged)));
  internal SfChart chart;
  private Matrix transformation;
  private Rect transformedDesiredElement;
  private DataTemplate contentTemplate;

  private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is Annotation annotation) || annotation.TextElement == null)
      return;
    annotation.TextElement.Content = (object) (string) e.NewValue;
  }

  private static void OnContentTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is Annotation annotation) || annotation.TextElement == null)
      return;
    annotation.TextElement.ContentTemplate = (DataTemplate) e.NewValue;
  }

  public Annotation()
  {
    this.AnnotationElement = new Grid();
    this.TextElementCanvas = new Canvas();
    this.TextElement = new ContentControl();
    this.TextElement.MouseLeftButtonDown += new MouseButtonEventHandler(this.TextElement_MouseLeftButtonDown);
    this.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 68, (byte) 68, (byte) 68));
  }

  public new event MouseButtonEventHandler MouseRightButtonDown
  {
    add => this.AnnotationElement.MouseRightButtonDown += value;
    remove => this.AnnotationElement.MouseRightButtonDown -= value;
  }

  public new event MouseButtonEventHandler MouseRightButtonUp
  {
    add => this.AnnotationElement.MouseRightButtonUp += value;
    remove => this.AnnotationElement.MouseRightButtonUp -= value;
  }

  public new event MouseButtonEventHandler MouseUp
  {
    add => this.AnnotationElement.MouseUp += value;
    remove => this.AnnotationElement.MouseUp -= value;
  }

  public new event MouseButtonEventHandler MouseDown
  {
    add => this.AnnotationElement.MouseDown += value;
    remove => this.AnnotationElement.MouseDown -= value;
  }

  public new event MouseEventHandler MouseMove
  {
    add => this.AnnotationElement.MouseMove += value;
    remove => this.AnnotationElement.MouseMove -= value;
  }

  public new event MouseButtonEventHandler MouseLeftButtonDown
  {
    add => this.AnnotationElement.MouseLeftButtonDown += value;
    remove => this.AnnotationElement.MouseLeftButtonDown -= value;
  }

  public new event MouseButtonEventHandler MouseLeftButtonUp
  {
    add => this.AnnotationElement.MouseLeftButtonUp += value;
    remove => this.AnnotationElement.MouseLeftButtonUp -= value;
  }

  public new event MouseEventHandler MouseLeave
  {
    add => this.AnnotationElement.MouseLeave += value;
    remove => this.AnnotationElement.MouseLeave -= value;
  }

  public event EventHandler Selected;

  public event EventHandler UnSelected;

  public string Text
  {
    get => (string) this.GetValue(Annotation.TextProperty);
    set => this.SetValue(Annotation.TextProperty, (object) value);
  }

  public bool EnableEditing
  {
    get => (bool) this.GetValue(Annotation.EnableEditingProperty);
    set => this.SetValue(Annotation.EnableEditingProperty, (object) value);
  }

  public DataTemplate ContentTemplate
  {
    get => (DataTemplate) this.GetValue(Annotation.ContentTemplateProperty);
    set => this.SetValue(Annotation.ContentTemplateProperty, (object) value);
  }

  public bool EnableClipping
  {
    get => (bool) this.GetValue(Annotation.EnableClippingProperty);
    set => this.SetValue(Annotation.EnableClippingProperty, (object) value);
  }

  public bool ShowToolTip
  {
    get => (bool) this.GetValue(Annotation.ShowToolTipProperty);
    set => this.SetValue(Annotation.ShowToolTipProperty, (object) value);
  }

  public object ToolTipContent
  {
    get => this.GetValue(Annotation.ToolTipContentProperty);
    set => this.SetValue(Annotation.ToolTipContentProperty, value);
  }

  public double ToolTipShowDuration
  {
    get => (double) this.GetValue(Annotation.ToolTipShowDurationProperty);
    set => this.SetValue(Annotation.ToolTipShowDurationProperty, (object) value);
  }

  public DataTemplate ToolTipTemplate
  {
    get => (DataTemplate) this.GetValue(Annotation.ToolTipTemplateProperty);
    set => this.SetValue(Annotation.ToolTipTemplateProperty, (object) value);
  }

  public ToolTipLabelPlacement ToolTipPlacement
  {
    get => (ToolTipLabelPlacement) this.GetValue(Annotation.ToolTipPlacementProperty);
    set => this.SetValue(Annotation.ToolTipPlacementProperty, (object) value);
  }

  public CoordinateUnit CoordinateUnit
  {
    get => (CoordinateUnit) this.GetValue(Annotation.CoordinateUnitProperty);
    set => this.SetValue(Annotation.CoordinateUnitProperty, (object) value);
  }

  public string XAxisName
  {
    get => (string) this.GetValue(Annotation.XAxisNameProperty);
    set => this.SetValue(Annotation.XAxisNameProperty, (object) value);
  }

  public string YAxisName
  {
    get => (string) this.GetValue(Annotation.YAxisNameProperty);
    set => this.SetValue(Annotation.YAxisNameProperty, (object) value);
  }

  public object X1
  {
    get => this.GetValue(Annotation.X1Property);
    set => this.SetValue(Annotation.X1Property, value);
  }

  public object Y1
  {
    get => this.GetValue(Annotation.Y1Property);
    set => this.SetValue(Annotation.Y1Property, value);
  }

  public double FontSize
  {
    get => (double) this.GetValue(Annotation.FontSizeProperty);
    set => this.SetValue(Annotation.FontSizeProperty, (object) value);
  }

  public FontFamily FontFamily
  {
    get => (FontFamily) this.GetValue(Annotation.FontFamilyProperty);
    set => this.SetValue(Annotation.FontFamilyProperty, (object) value);
  }

  public FontStretch FontStretch
  {
    get => (FontStretch) this.GetValue(Annotation.FontStretchProperty);
    set => this.SetValue(Annotation.FontStretchProperty, (object) value);
  }

  public FontStyle FontStyle
  {
    get => (FontStyle) this.GetValue(Annotation.FontStyleProperty);
    set => this.SetValue(Annotation.FontStyleProperty, (object) value);
  }

  public FontWeight FontWeight
  {
    get => (FontWeight) this.GetValue(Annotation.FontWeightProperty);
    set => this.SetValue(Annotation.FontWeightProperty, (object) value);
  }

  public Brush Foreground
  {
    get => (Brush) this.GetValue(Annotation.ForegroundProperty);
    set => this.SetValue(Annotation.ForegroundProperty, (object) value);
  }

  internal virtual SfChart Chart
  {
    get => this.chart;
    set
    {
      this.chart = value;
      this.SetAxisFromName();
    }
  }

  internal HorizontalAlignment InternalHorizontalAlignment
  {
    get => (HorizontalAlignment) this.GetValue(Annotation.InternalHorizontalAlignmentProperty);
    set => this.SetValue(Annotation.InternalHorizontalAlignmentProperty, (object) value);
  }

  internal VerticalAlignment InternalVerticalAlignment
  {
    get => (VerticalAlignment) this.GetValue(Annotation.InternalVerticalAlignmentProperty);
    set => this.SetValue(Annotation.InternalVerticalAlignmentProperty, (object) value);
  }

  internal Visibility InternalVisibility
  {
    get => (Visibility) this.GetValue(Annotation.InternalVisibilityProperty);
    set => this.SetValue(Annotation.InternalVisibilityProperty, (object) value);
  }

  internal Rect RotatedRect { get; set; }

  internal bool IsSelected { get; set; }

  internal bool IsResizing { get; set; }

  internal bool IsVisbilityChanged { get; set; }

  internal ChartAxis XAxis { get; set; }

  internal ChartAxis YAxis { get; set; }

  protected double x1 { get; set; }

  protected double y1 { get; set; }

  protected bool IsUiCleared { get; set; }

  protected Grid AnnotationElement { get; set; }

  protected Canvas TextElementCanvas { get; set; }

  protected ContentControl TextElement { get; set; }

  protected Rect RotatedTextRect { get; set; }

  public DependencyObject Clone() => this.CloneAnnotation((Annotation) null);

  public virtual UIElement GetRenderedAnnotation() => (UIElement) this.AnnotationElement;

  public virtual void UpdateAnnotation() => this.SetData();

  internal static void OnTextAlignmentChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    (d as Annotation).UpdateAnnotation();
  }

  internal static double ConvertData(object data, ChartAxis axis)
  {
    switch (axis)
    {
      case NumericalAxis _:
        return Convert.ToDouble(data);
      case DateTimeAxis _:
        switch (data)
        {
          case DateTime dateTime:
            return dateTime.ToOADate();
          case string _:
            return Convert.ToDateTime(data.ToString()).ToOADate();
          default:
            return Convert.ToDouble(data);
        }
      case TimeSpanAxis _:
        switch (data)
        {
          case TimeSpan timeSpan:
            return timeSpan.TotalMilliseconds;
          case string _:
            return TimeSpan.Parse(data.ToString()).TotalMilliseconds;
          default:
            return Convert.ToDouble(data);
        }
      case LogarithmicAxis logarithmicAxis:
        data = Convert.ToDouble(data) > 0.0 ? data : (object) 1;
        return Math.Log(Convert.ToDouble(data), logarithmicAxis.LogarithmicBase);
      default:
        return Convert.ToDouble(data);
    }
  }

  internal static object ConvertToObject(double data, ChartAxis axis)
  {
    switch (axis)
    {
      case DateTimeAxis _:
        return (object) data.FromOADate();
      case TimeSpanAxis _:
        return (object) TimeSpan.FromMilliseconds(data);
      case LogarithmicAxis logarithmicAxis:
        return (object) Math.Pow(logarithmicAxis.LogarithmicBase, data);
      default:
        return (object) Convert.ToDouble(data);
    }
  }

  internal void Dispose()
  {
    if (this is AnnotationResizer annotationResizer && annotationResizer.ResizerControl != null)
    {
      annotationResizer.ResizerControl.Dispose();
      annotationResizer.ResizerControl = (Resizer) null;
    }
    this.Chart = (SfChart) null;
    this.XAxis = (ChartAxis) null;
    this.YAxis = (ChartAxis) null;
  }

  internal string Serialize()
  {
    StringBuilder result = new StringBuilder();
    ChartBase.GetSerializedString(out result, (object) this);
    return result.ToString();
  }

  internal virtual void OnVisibilityChanged()
  {
    if (this.chart == null || this.chart.AnnotationManager == null || this.chart.Annotations == null || !this.chart.Annotations.Contains(this))
      return;
    bool flag = this is ShapeAnnotation shapeAnnotation && this.chart.AnnotationManager.SelectedAnnotation == this && shapeAnnotation.CanResize;
    this.IsVisbilityChanged = true;
    if (this.Visibility.Equals((object) Visibility.Collapsed) || this.Visibility.Equals((object) Visibility.Hidden))
    {
      this.chart.AnnotationManager.AddOrRemoveAnnotations(this, true);
      if (!flag || this.chart.AnnotationManager.AnnotationResizer == null)
        return;
      this.chart.AnnotationManager.AnnotationResizer.IsVisbilityChanged = true;
      this.chart.AnnotationManager.AddOrRemoveAnnotationResizer((Annotation) this.chart.AnnotationManager.AnnotationResizer, true);
    }
    else
    {
      this.chart.AnnotationManager.AddOrRemoveAnnotations(this, false);
      if (!flag || this.chart.AnnotationManager.AnnotationResizer == null)
        return;
      this.chart.AnnotationManager.AnnotationResizer.IsVisbilityChanged = true;
      this.chart.AnnotationManager.AddOrRemoveAnnotationResizer((Annotation) this.chart.AnnotationManager.AnnotationResizer, false);
    }
  }

  internal void UpdatePropertyChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.Chart != null && this.Chart.AnnotationManager != null && this.Chart.AnnotationManager.SelectedAnnotation != null && this is AnnotationResizer && args.OldValue != null)
    {
      this.Chart.AnnotationManager.SelectedAnnotation.Y1 = this.Y1;
      this.Chart.AnnotationManager.SelectedAnnotation.X1 = this.X1;
    }
    this.UpdateAnnotation();
    if (this.Chart == null || this.CoordinateUnit != CoordinateUnit.Axis || !this.CanUpdateRange(this.X1, this.Y1))
      return;
    this.Chart.ScheduleUpdate();
  }

  internal virtual UIElement CreateAnnotation() => (UIElement) this.AnnotationElement;

  internal void SetAxisFromName()
  {
    if (this.Chart == null)
      return;
    this.XAxis = this.Chart.Axes[this.XAxisName] ?? this.Chart.InternalPrimaryAxis;
    this.YAxis = this.Chart.Axes[this.YAxisName] ?? this.Chart.InternalSecondaryAxis;
  }

  protected internal virtual void OnSelected(EventArgs args)
  {
    if (this.Selected == null)
      return;
    this.Selected((object) this, args);
  }

  protected internal virtual void OnUnSelected(EventArgs args)
  {
    if (this.UnSelected == null)
      return;
    this.UnSelected((object) this, args);
  }

  protected bool CanUpdateRange(object x, object y)
  {
    bool flag1 = false;
    bool flag2 = false;
    if (x != null)
    {
      double num = Annotation.ConvertData(x, this.XAxis);
      if (num < this.XAxis.VisibleRange.Start || num > this.XAxis.VisibleRange.End)
        flag1 = true;
    }
    if (y != null)
    {
      double num = Annotation.ConvertData(y, this.YAxis);
      if (num < this.YAxis.VisibleRange.Start || num > this.YAxis.VisibleRange.End)
        flag2 = true;
    }
    return flag1 || flag2;
  }

  protected Point GetElementPosition(Size desiredSize, Point originalPosition)
  {
    Point elementPosition = originalPosition;
    HorizontalAlignment horizontalAlignment = this.InternalHorizontalAlignment;
    VerticalAlignment verticalAlignment = this.InternalVerticalAlignment;
    switch (horizontalAlignment)
    {
      case HorizontalAlignment.Left:
        elementPosition.X -= desiredSize.Width;
        break;
      case HorizontalAlignment.Center:
        elementPosition.X -= desiredSize.Width / 2.0;
        break;
    }
    switch (verticalAlignment)
    {
      case VerticalAlignment.Top:
        elementPosition.Y -= desiredSize.Height;
        break;
      case VerticalAlignment.Center:
        elementPosition.Y -= desiredSize.Height / 2.0;
        break;
    }
    return elementPosition;
  }

  protected Point GetElementPosition(FrameworkElement element, Point originalPosition)
  {
    Point elementPosition = originalPosition;
    HorizontalAlignment horizontalAlignment = this.InternalHorizontalAlignment;
    VerticalAlignment verticalAlignment = this.InternalVerticalAlignment;
    Size size = new Size(element.ActualWidth, element.ActualHeight);
    switch (horizontalAlignment)
    {
      case HorizontalAlignment.Left:
        elementPosition.X -= size.Width;
        break;
      case HorizontalAlignment.Center:
        elementPosition.X -= size.Width / 2.0;
        break;
    }
    switch (verticalAlignment)
    {
      case VerticalAlignment.Top:
        elementPosition.Y -= size.Height;
        break;
      case VerticalAlignment.Center:
        elementPosition.Y -= size.Height / 2.0;
        break;
    }
    return elementPosition;
  }

  protected bool IntersectsWith(Rect r1, Rect r2)
  {
    return r2.Left <= r1.Right && r2.Right >= r1.Left && r2.Top <= r1.Bottom && r2.Bottom >= r1.Top;
  }

  protected double GetClippingValues(double value, ChartAxis axis)
  {
    DoubleRange visibleRange = axis.VisibleRange;
    if (value < visibleRange.Start)
      return visibleRange.Start;
    return value > visibleRange.End ? visibleRange.End : value;
  }

  protected void SetData()
  {
    this.SetAxisFromName();
    if (this.XAxis == null || this.YAxis == null)
      return;
    this.x1 = Annotation.ConvertData(this.X1, this.XAxis);
    this.y1 = Annotation.ConvertData(this.Y1, this.YAxis);
  }

  protected virtual void SetBindings()
  {
    Binding binding1 = new Binding()
    {
      Path = new PropertyPath("HorizontalAlignment", new object[0]),
      Source = (object) this
    };
    this.SetBinding(Annotation.InternalHorizontalAlignmentProperty, (BindingBase) binding1);
    Binding binding2 = new Binding()
    {
      Path = new PropertyPath("VerticalAlignment", new object[0]),
      Source = (object) this
    };
    this.SetBinding(Annotation.InternalVerticalAlignmentProperty, (BindingBase) binding2);
    Binding binding3 = new Binding()
    {
      Path = new PropertyPath("Visibility", new object[0]),
      Source = (object) this
    };
    this.SetBinding(Annotation.InternalVisibilityProperty, (BindingBase) binding3);
    if (this.TextElement == null)
      return;
    Binding binding4 = new Binding()
    {
      Path = new PropertyPath("Text", new object[0]),
      Source = (object) this
    };
    this.TextElement.SetBinding(ContentControl.ContentProperty, (BindingBase) binding4);
    Binding binding5 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("FontSize", new object[0])
    };
    this.TextElement.SetBinding(Control.FontSizeProperty, (BindingBase) binding5);
    Binding binding6 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("FontStyle", new object[0])
    };
    this.TextElement.SetBinding(Control.FontStyleProperty, (BindingBase) binding6);
    Binding binding7 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("FontStretch", new object[0])
    };
    this.TextElement.SetBinding(Control.FontStretchProperty, (BindingBase) binding7);
    Binding binding8 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("FontFamily", new object[0])
    };
    this.TextElement.SetBinding(Control.FontFamilyProperty, (BindingBase) binding8);
    Binding binding9 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("FontWeight", new object[0])
    };
    this.TextElement.SetBinding(Control.FontWeightProperty, (BindingBase) binding9);
    Binding binding10 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Foreground", new object[0])
    };
    this.TextElement.SetBinding(Control.ForegroundProperty, (BindingBase) binding10);
    Binding binding11 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("ContentTemplate", new object[0])
    };
    this.TextElement.SetBinding(ContentControl.ContentTemplateProperty, (BindingBase) binding11);
    Binding binding12 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Cursor", new object[0])
    };
    this.TextElement.SetBinding(FrameworkElement.CursorProperty, (BindingBase) binding12);
  }

  protected Rect RotateElement(double angle, FrameworkElement item)
  {
    double num1 = 2.0 * Math.PI * angle / 360.0;
    double num2 = Math.Cos(num1);
    double m12 = Math.Sin(num1);
    MatrixTransform matrixTransform = new MatrixTransform();
    this.transformation = new Matrix(num2, m12, -m12, num2, 0.0, 0.0);
    double num3 = item.ActualHeight / 2.0;
    this.transformedDesiredElement = Annotation.ElementTransform(new Rect(0.0, 0.0, item.ActualWidth, item.ActualHeight), this.transformation);
    matrixTransform.Matrix = new Matrix(num2, m12, -m12, num2, 0.0, this.transformedDesiredElement.Height / 2.0 - num3);
    return this.transformedDesiredElement;
  }

  protected Point GetRotatePoint(double angle, FrameworkElement item, Point originalPoint)
  {
    double num1 = 2.0 * Math.PI * angle / 360.0;
    double num2 = Math.Cos(num1);
    double m12 = Math.Sin(num1);
    TransformGroup transformGroup = new TransformGroup();
    MatrixTransform matrixTransform = new MatrixTransform();
    this.transformation = new Matrix(num2, m12, -m12, num2, 0.0, 0.0);
    double num3 = item.ActualHeight / 2.0;
    this.transformedDesiredElement = Annotation.ElementTransform(new Rect(0.0, 0.0, item.ActualWidth, item.ActualHeight), this.transformation);
    matrixTransform.Matrix = new Matrix(num2, m12, -m12, num2, 0.0, this.transformedDesiredElement.Height / 2.0 - num3);
    item.RenderTransformOrigin = new Point(0.5, 0.5);
    transformGroup.Children.Add((Transform) matrixTransform);
    item.RenderTransform = (Transform) transformGroup;
    return matrixTransform.Transform(originalPoint);
  }

  protected Rect RotateElement(double angle, FrameworkElement item, Size itemSize)
  {
    double num1 = 2.0 * Math.PI * angle / 360.0;
    double num2 = Math.Cos(num1);
    double m12 = Math.Sin(num1);
    TransformGroup transformGroup = new TransformGroup();
    MatrixTransform matrixTransform = new MatrixTransform();
    this.transformation = new Matrix(num2, m12, -m12, num2, 0.0, 0.0);
    double num3 = itemSize.Height / 2.0;
    this.transformedDesiredElement = Annotation.ElementTransform(new Rect(0.0, 0.0, itemSize.Width, itemSize.Height), this.transformation);
    matrixTransform.Matrix = new Matrix(num2, m12, -m12, num2, 0.0, this.transformedDesiredElement.Height / 2.0 - num3);
    transformGroup.Children.Add((Transform) matrixTransform);
    return this.transformedDesiredElement;
  }

  protected Point EnsurePoint(Point point1, Point point2)
  {
    double x = point1.X;
    double y = point1.Y;
    return new Point(Math.Min(x, point2.X), Math.Min(y, point2.Y));
  }

  protected virtual DependencyObject CloneAnnotation(Annotation annotation)
  {
    annotation.ContentTemplate = this.ContentTemplate;
    annotation.CoordinateUnit = this.CoordinateUnit;
    annotation.FontFamily = this.FontFamily;
    annotation.FontSize = this.FontSize;
    annotation.FontStyle = this.FontStyle;
    annotation.FontWeight = this.FontWeight;
    annotation.Foreground = this.Foreground;
    annotation.InternalHorizontalAlignment = this.InternalHorizontalAlignment;
    annotation.Text = this.Text;
    annotation.EnableEditing = this.EnableEditing;
    annotation.InternalVerticalAlignment = this.InternalVerticalAlignment;
    annotation.X1 = this.X1;
    annotation.Y1 = this.Y1;
    annotation.XAxisName = this.XAxisName;
    annotation.YAxisName = this.YAxisName;
    annotation.ShowToolTip = this.ShowToolTip;
    annotation.ToolTipContent = this.ToolTipContent;
    annotation.ToolTipPlacement = this.ToolTipPlacement;
    annotation.ToolTipShowDuration = this.ToolTipShowDuration;
    annotation.ToolTipTemplate = this.ToolTipTemplate;
    return (DependencyObject) annotation;
  }

  private static void OnAxisNameChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(sender is Annotation annotation))
      return;
    annotation.UpdateAnnotation();
  }

  private static void OnUpdatePropertyChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(sender is Annotation annotation))
      return;
    annotation.UpdatePropertyChanged(args);
  }

  private static void OnVisibiltyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (e.OldValue.Equals((object) Visibility.Collapsed) && e.NewValue.Equals((object) Visibility.Hidden) || e.OldValue.Equals((object) Visibility.Hidden) && e.NewValue.Equals((object) Visibility.Collapsed) || e.OldValue.Equals(e.NewValue))
      return;
    (d as Annotation).OnVisibilityChanged();
  }

  private static void OnEnableEditingChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is Annotation annotation))
      return;
    annotation.OnEditing(annotation);
  }

  private static void OnEnableClippingPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is Annotation annotation))
      return;
    annotation.UpdateAnnotation();
  }

  private static void OnCoordinatePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    Annotation annotation = d as Annotation;
    if (e.OldValue.Equals(e.NewValue) || annotation == null || annotation.chart == null || annotation.chart.AnnotationManager == null)
      return;
    annotation.chart.AnnotationManager.AddOrRemoveAnnotations(annotation, true);
    annotation.UpdatePropertyChanged(e);
  }

  private static Rect ElementTransform(Rect rect, Matrix matrix)
  {
    Point point1 = matrix.Transform(new Point(rect.Left, rect.Top));
    Point point2 = matrix.Transform(new Point(rect.Right, rect.Top));
    Point point3 = matrix.Transform(new Point(rect.Left, rect.Bottom));
    Point point4 = matrix.Transform(new Point(rect.Right, rect.Bottom));
    double x = Math.Min(Math.Min(point1.X, point2.X), Math.Min(point3.X, point4.X));
    double y = Math.Min(Math.Min(point1.Y, point2.Y), Math.Min(point3.Y, point4.Y));
    double num1 = Math.Max(Math.Max(point1.X, point2.X), Math.Max(point3.X, point4.X));
    double num2 = Math.Max(Math.Max(point1.Y, point2.Y), Math.Max(point3.Y, point4.Y));
    return new Rect(x, y, num1 - x, num2 - y);
  }

  private void OnEditing(Annotation annotation)
  {
    if (this.Chart == null || this.Chart.AnnotationManager.TextAnnotation != annotation)
      return;
    this.Chart.AnnotationManager.OnTextEditing();
  }

  private void TextElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (this.Chart.AnnotationManager.IsEditing)
      this.Chart.AnnotationManager.OnTextEditing();
    if (!this.EnableEditing || this.ContentTemplate != null)
      return;
    e.Handled = true;
    this.OnTextEditingMode(this.TextElement);
  }

  private void SetTextElementBinding(TextBox textElement)
  {
    if (textElement == null)
      return;
    Binding binding1 = new Binding()
    {
      Path = new PropertyPath("Text", new object[0]),
      Source = (object) this
    };
    textElement.SetBinding(TextBox.TextProperty, (BindingBase) binding1);
    Binding binding2 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("FontSize", new object[0])
    };
    textElement.SetBinding(Control.FontSizeProperty, (BindingBase) binding2);
    Binding binding3 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("FontStyle", new object[0])
    };
    textElement.SetBinding(Control.FontStyleProperty, (BindingBase) binding3);
    Binding binding4 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("FontStretch", new object[0])
    };
    textElement.SetBinding(Control.FontStretchProperty, (BindingBase) binding4);
    Binding binding5 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("FontFamily", new object[0])
    };
    textElement.SetBinding(Control.FontFamilyProperty, (BindingBase) binding5);
    Binding binding6 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("FontWeight", new object[0])
    };
    textElement.SetBinding(Control.FontWeightProperty, (BindingBase) binding6);
    Binding binding7 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Foreground", new object[0])
    };
    textElement.SetBinding(Control.ForegroundProperty, (BindingBase) binding7);
    Binding binding8 = new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Cursor", new object[0])
    };
    textElement.SetBinding(FrameworkElement.CursorProperty, (BindingBase) binding8);
  }

  private void OnTextEditingMode(ContentControl textElement)
  {
    if (this.chart.AnnotationManager.SelectedAnnotation != null)
      this.chart.AnnotationManager.SelectedAnnotation = (Annotation) null;
    if (textElement != null)
      this.Chart.AnnotationManager.EditAnnotation = textElement;
    this.contentTemplate = ChartDictionaries.GenericCommonDictionary[(object) "textBoxAnnotation"] as DataTemplate;
    TextBox textElement1 = this.contentTemplate.LoadContent() as TextBox;
    textElement.SetValue(ContentControl.ContentProperty, (object) textElement1);
    this.SetTextElementBinding(textElement1);
    this.Chart.AnnotationManager.TextBox = textElement1;
    this.Chart.AnnotationManager.IsEditing = true;
    this.Chart.AnnotationManager.TextAnnotation = this;
  }
}
