// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SfSurfaceChart
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using Syncfusion.Licensing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SfSurfaceChart : SurfaceBase, IDisposable
{
  private Point currentMousePosition;
  private Dictionary<Point3D, int> pointDictionary;
  private Point3D[,] dataPoints;
  private Point3D[,] points;
  private double yMin;
  private double yMax;
  private bool drawWireframe;
  private Brush materail;
  internal bool IsContour;
  private bool isOppositeAxis;
  private double previousScale;
  internal IEnumerable XValues;
  internal List<double> YValues;
  internal IEnumerable ZValues;
  internal List<object> ActualData;
  internal Size ViewBoxSize;
  internal bool RangeChanged;
  internal static readonly DependencyProperty WallContentProperty = DependencyProperty.Register(nameof (WallContent), typeof (Model3DGroup), typeof (SfSurfaceChart), new PropertyMetadata((object) null));
  internal static readonly DependencyProperty MeshContentProperty = DependencyProperty.Register(nameof (MeshContent), typeof (Model3DGroup), typeof (SfSurfaceChart), new PropertyMetadata((object) null));
  internal static readonly DependencyProperty AxisContentProperty = DependencyProperty.Register(nameof (AxisContent), typeof (Model3DGroup), typeof (SfSurfaceChart), new PropertyMetadata((object) null));
  internal static readonly DependencyProperty GridLineContentProperty = DependencyProperty.Register(nameof (GridLineContent), typeof (Model3DGroup), typeof (SfSurfaceChart), new PropertyMetadata((object) null));
  internal static readonly DependencyProperty LightContentProperty = DependencyProperty.Register(nameof (LightContent), typeof (Model3DGroup), typeof (SfSurfaceChart), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof (ItemsSource), typeof (IEnumerable), typeof (SfSurfaceChart), new PropertyMetadata((object) null, new PropertyChangedCallback(SfSurfaceChart.OnItemsSourceChanged)));
  public static readonly DependencyProperty XAxisProperty = DependencyProperty.Register(nameof (XAxis), typeof (SurfaceAxis), typeof (SfSurfaceChart), new PropertyMetadata((object) null, new PropertyChangedCallback(SfSurfaceChart.OnXAxisChanged)));
  public static readonly DependencyProperty YAxisProperty = DependencyProperty.Register(nameof (YAxis), typeof (SurfaceAxis), typeof (SfSurfaceChart), new PropertyMetadata((object) null, new PropertyChangedCallback(SfSurfaceChart.OnYAxisChanged)));
  public static readonly DependencyProperty ZAxisProperty = DependencyProperty.Register(nameof (ZAxis), typeof (SurfaceAxis), typeof (SfSurfaceChart), new PropertyMetadata((object) null, new PropertyChangedCallback(SfSurfaceChart.OnZAxisChanged)));
  public static readonly DependencyProperty ColorBarProperty = DependencyProperty.Register(nameof (ColorBar), typeof (ChartColorBar), typeof (SfSurfaceChart), new PropertyMetadata((object) null, new PropertyChangedCallback(SfSurfaceChart.OnColorBarPropertyChanged)));
  public static readonly DependencyProperty ApplyGradientBrushProperty = DependencyProperty.Register(nameof (ApplyGradientBrush), typeof (bool), typeof (SfSurfaceChart), new PropertyMetadata((object) false, new PropertyChangedCallback(SfSurfaceChart.OnGradientPalatteChanged)));
  public static readonly DependencyProperty XBindingPathProperty = DependencyProperty.Register(nameof (XBindingPath), typeof (string), typeof (SfSurfaceChart), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty YBindingPathProperty = DependencyProperty.Register(nameof (YBindingPath), typeof (string), typeof (SfSurfaceChart), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ZBindingPathProperty = DependencyProperty.Register(nameof (ZBindingPath), typeof (string), typeof (SfSurfaceChart), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty RowSizeProperty = DependencyProperty.Register(nameof (RowSize), typeof (int), typeof (SfSurfaceChart), new PropertyMetadata((object) 0));
  public static readonly DependencyProperty ColumnSizeProperty = DependencyProperty.Register(nameof (ColumnSize), typeof (int), typeof (SfSurfaceChart), new PropertyMetadata((object) 0));
  public static readonly DependencyProperty LegendLabelFormatProperty = DependencyProperty.Register(nameof (LegendLabelFormat), typeof (string), typeof (SfSurfaceChart), new PropertyMetadata((object) "#0.#", new PropertyChangedCallback(SfSurfaceChart.OnLegendLabelFormatPropertyChanged)));
  public static readonly DependencyProperty WireframeStrokeProperty = DependencyProperty.Register(nameof (WireframeStroke), typeof (Brush), typeof (SfSurfaceChart), new PropertyMetadata((object) new SolidColorBrush(Colors.Black), new PropertyChangedCallback(SfSurfaceChart.OnWireframePropertyChanged)));
  public static readonly DependencyProperty WireframeStrokeThicknessProperty = DependencyProperty.Register(nameof (WireframeStrokeThickness), typeof (double), typeof (SfSurfaceChart), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(SfSurfaceChart.OnWireframePropertyChanged)));
  public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof (Type), typeof (SurfaceType), typeof (SfSurfaceChart), new PropertyMetadata((object) SurfaceType.Surface, new PropertyChangedCallback(SfSurfaceChart.OnTypePropertyChanged)));
  public static readonly DependencyProperty CameraProjectionProperty = DependencyProperty.Register(nameof (CameraProjection), typeof (CameraProjection), typeof (SfSurfaceChart), new PropertyMetadata((object) CameraProjection.Orthographic, new PropertyChangedCallback(SfSurfaceChart.OnCameraProjectionChanged)));
  public static readonly DependencyProperty ZoomLevelProperty = DependencyProperty.Register(nameof (ZoomLevel), typeof (double), typeof (SfSurfaceChart), new PropertyMetadata((object) 0.4, new PropertyChangedCallback(SfSurfaceChart.OnZoomLevelChanged)));
  public static readonly DependencyProperty ShowContourLineProperty = DependencyProperty.Register(nameof (ShowContourLine), typeof (bool), typeof (SfSurfaceChart), new PropertyMetadata((object) false, new PropertyChangedCallback(SfSurfaceChart.OnContourLineChanged)));
  public static readonly DependencyProperty EnableZoomingProperty = DependencyProperty.Register(nameof (EnableZooming), typeof (bool), typeof (SfSurfaceChart), new PropertyMetadata((object) false));
  public static readonly DependencyProperty BrushCountProperty = DependencyProperty.Register(nameof (BrushCount), typeof (int), typeof (SfSurfaceChart), new PropertyMetadata((object) 4, new PropertyChangedCallback(SfSurfaceChart.OnBrushCountPropertyChanged)));

  public SfSurfaceChart()
  {
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
    this.DefaultStyleKey = (object) typeof (SfSurfaceChart);
    this.Viewport = new Viewport3D();
    this.Data = new DataPointCollection();
    this.Axes = new AxisCollection();
    this.ColorModel = new ChartColorModel(this.Palette);
    this.CreateCamera();
    this.UpdateViewport();
  }

  internal Model3DGroup WallContent
  {
    get => (Model3DGroup) this.GetValue(SfSurfaceChart.WallContentProperty);
    set => this.SetValue(SfSurfaceChart.WallContentProperty, (object) value);
  }

  internal Model3DGroup MeshContent
  {
    get => (Model3DGroup) this.GetValue(SfSurfaceChart.MeshContentProperty);
    set => this.SetValue(SfSurfaceChart.MeshContentProperty, (object) value);
  }

  internal Model3DGroup AxisContent
  {
    get => (Model3DGroup) this.GetValue(SfSurfaceChart.AxisContentProperty);
    set => this.SetValue(SfSurfaceChart.AxisContentProperty, (object) value);
  }

  internal Model3DGroup GridLineContent
  {
    get => (Model3DGroup) this.GetValue(SfSurfaceChart.GridLineContentProperty);
    set => this.SetValue(SfSurfaceChart.GridLineContentProperty, (object) value);
  }

  internal Model3DGroup LightContent
  {
    get => (Model3DGroup) this.GetValue(SfSurfaceChart.LightContentProperty);
    set => this.SetValue(SfSurfaceChart.LightContentProperty, (object) value);
  }

  internal ChartValueType XAxisValueType { get; set; }

  internal ChartValueType YAxisValueType { get; set; }

  internal ChartValueType ZAxisValueType { get; set; }

  public DataPointCollection Data { get; set; }

  public IEnumerable ItemsSource
  {
    get => (IEnumerable) this.GetValue(SfSurfaceChart.ItemsSourceProperty);
    set => this.SetValue(SfSurfaceChart.ItemsSourceProperty, (object) value);
  }

  private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfSurfaceChart sfSurfaceChart))
      return;
    sfSurfaceChart.OnItemsSourceChanged(e);
    sfSurfaceChart.ScheduleUpdate();
  }

  private void OnItemsSourceChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.XValues != null)
    {
      if (this.XValues is IList<double>)
        (this.XValues as IList<double>).Clear();
      else if (this.XValues is IList<string>)
        (this.XValues as IList<string>).Clear();
    }
    if (this.ZValues != null)
    {
      if (this.ZValues is IList<double>)
        (this.ZValues as IList<double>).Clear();
      else if (this.ZValues is IList<string>)
        (this.ZValues as IList<string>).Clear();
    }
    if (args.NewValue == null)
    {
      this.XRange = DoubleRange.Empty;
      this.YRange = DoubleRange.Empty;
      this.ZRange = DoubleRange.Empty;
    }
    if (this.YValues != null)
      this.YValues.Clear();
    this.IsPointsGenerated = false;
    this.IsUpdateLegend = true;
    if (args.OldValue is INotifyCollectionChanged)
      (args.OldValue as INotifyCollectionChanged).CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnDataCollectionChanged);
    if (args.NewValue is INotifyCollectionChanged)
      (args.NewValue as INotifyCollectionChanged).CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnDataCollectionChanged);
    if (this.ItemsSource == null || string.IsNullOrEmpty(this.XBindingPath) || string.IsNullOrEmpty(this.YBindingPath) || string.IsNullOrEmpty(this.ZBindingPath))
      return;
    this.OnCollectionChanged(args.NewValue as IEnumerable);
  }

  private void OnDataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.Action != NotifyCollectionChangedAction.Reset)
      return;
    this.Refresh();
  }

  private void Refresh()
  {
    this.Data.XValues.Clear();
    this.Data.YValues.Clear();
    this.Data.ZValues.Clear();
    (this.XValues as IList<double>).Clear();
    this.YValues.Clear();
    (this.ZValues as IList<double>).Clear();
    this.ActualData.Clear();
    this.XRange = DoubleRange.Empty;
    this.YRange = DoubleRange.Empty;
    this.ZRange = DoubleRange.Empty;
    this.IsPointsGenerated = false;
    this.IsUpdateLegend = true;
    this.ScheduleUpdate();
  }

  private void OnCollectionChanged(IEnumerable enumerable)
  {
    IEnumerator enumerator = this.ItemsSource.GetEnumerator();
    if (!enumerator.MoveNext())
      return;
    PropertyInfo declaredProperty1 = enumerator.Current.GetType().GetTypeInfo().GetDeclaredProperty(this.XBindingPath);
    IPropertyAccessor propertyAccessor1 = (IPropertyAccessor) null;
    if (declaredProperty1 != (PropertyInfo) null)
      propertyAccessor1 = FastReflectionCaches.PropertyAccessorCache.Get(declaredProperty1);
    if (propertyAccessor1 == null)
      return;
    Func<object, object> getMethod1 = propertyAccessor1.GetMethod;
    PropertyInfo declaredProperty2 = enumerator.Current.GetType().GetTypeInfo().GetDeclaredProperty(this.YBindingPath);
    IPropertyAccessor propertyAccessor2 = (IPropertyAccessor) null;
    if (declaredProperty2 != (PropertyInfo) null)
      propertyAccessor2 = FastReflectionCaches.PropertyAccessorCache.Get(declaredProperty2);
    if (propertyAccessor2 == null)
      return;
    Func<object, object> getMethod2 = propertyAccessor2.GetMethod;
    PropertyInfo declaredProperty3 = enumerator.Current.GetType().GetTypeInfo().GetDeclaredProperty(this.ZBindingPath);
    IPropertyAccessor propertyAccessor3 = (IPropertyAccessor) null;
    if (declaredProperty3 != (PropertyInfo) null)
      propertyAccessor3 = FastReflectionCaches.PropertyAccessorCache.Get(declaredProperty3);
    if (propertyAccessor3 == null)
      return;
    Func<object, object> getMethod3 = propertyAccessor3.GetMethod;
    this.YValues = new List<double>();
    this.ZValues = (IEnumerable) new List<double>();
    this.ActualData = new List<object>();
    this.XAxisValueType = SfSurfaceChart.GetDataType(propertyAccessor1, this.ItemsSource);
    this.ZAxisValueType = SfSurfaceChart.GetDataType(propertyAccessor3, this.ItemsSource);
    if (this.XAxisValueType == ChartValueType.String || this.ZAxisValueType == ChartValueType.String)
    {
      if (this.XAxisValueType == ChartValueType.String && this.ZAxisValueType == ChartValueType.String)
      {
        this.XValues = (IEnumerable) new List<string>();
        this.ZValues = (IEnumerable) new List<string>();
        IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
        IList<string> zvalues = (IList<string>) (this.ZValues as List<string>);
        do
        {
          object obj1 = getMethod1(enumerator.Current);
          object obj2 = getMethod2(enumerator.Current);
          object obj3 = getMethod3(enumerator.Current);
          xvalues.Add((string) obj1);
          this.YValues.Add(Convert.ToDouble(obj2));
          zvalues.Add((string) obj3);
          this.ActualData.Add(enumerator.Current);
        }
        while (enumerator.MoveNext());
      }
      else if (this.XAxisValueType == ChartValueType.String && this.ZAxisValueType != ChartValueType.String)
      {
        this.XValues = (IEnumerable) new List<string>();
        this.ZValues = (IEnumerable) new List<double>();
        IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
        IList<double> zvalues = (IList<double>) (this.ZValues as List<double>);
        do
        {
          object obj4 = getMethod1(enumerator.Current);
          object obj5 = getMethod2(enumerator.Current);
          object obj6 = getMethod3(enumerator.Current);
          xvalues.Add((string) obj4);
          this.YValues.Add(Convert.ToDouble(obj5));
          zvalues.Add(Convert.ToDouble(obj6));
          this.ActualData.Add(enumerator.Current);
        }
        while (enumerator.MoveNext());
      }
      else
      {
        if (this.XAxisValueType == ChartValueType.String || this.ZAxisValueType != ChartValueType.String)
          return;
        this.XValues = (IEnumerable) new List<double>();
        this.ZValues = (IEnumerable) new List<string>();
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        IList<string> zvalues = (IList<string>) (this.ZValues as List<string>);
        do
        {
          object obj7 = getMethod1(enumerator.Current);
          object obj8 = getMethod2(enumerator.Current);
          object obj9 = getMethod3(enumerator.Current);
          xvalues.Add(Convert.ToDouble(obj7));
          this.YValues.Add(Convert.ToDouble(obj8));
          zvalues.Add((string) obj9);
          this.ActualData.Add(enumerator.Current);
        }
        while (enumerator.MoveNext());
      }
    }
    else
    {
      this.XValues = (IEnumerable) new List<double>();
      IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
      this.ZValues = (IEnumerable) new List<double>();
      IList<double> zvalues = (IList<double>) (this.ZValues as List<double>);
      do
      {
        object obj10 = getMethod1(enumerator.Current);
        object obj11 = getMethod2(enumerator.Current);
        object obj12 = getMethod3(enumerator.Current);
        xvalues.Add(Convert.ToDouble(obj10));
        this.YValues.Add(Convert.ToDouble(obj11));
        zvalues.Add(Convert.ToDouble(obj12));
        this.ActualData.Add(enumerator.Current);
      }
      while (enumerator.MoveNext());
    }
  }

  public SurfaceAxis XAxis
  {
    get => (SurfaceAxis) this.GetValue(SfSurfaceChart.XAxisProperty);
    set => this.SetValue(SfSurfaceChart.XAxisProperty, (object) value);
  }

  private static void OnXAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfSurfaceChart sfSurfaceChart))
      return;
    if (e.NewValue is SurfaceAxis newValue)
    {
      sfSurfaceChart.InterernalXAxis = newValue;
      newValue.Orientation = Orientation.Horizontal;
    }
    sfSurfaceChart.OnAxisChanged(e);
  }

  public SurfaceAxis YAxis
  {
    get => (SurfaceAxis) this.GetValue(SfSurfaceChart.YAxisProperty);
    set => this.SetValue(SfSurfaceChart.YAxisProperty, (object) value);
  }

  private static void OnYAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfSurfaceChart sfSurfaceChart))
      return;
    SurfaceAxis newValue = e.NewValue as SurfaceAxis;
    if (e.NewValue != null)
    {
      sfSurfaceChart.InterernalYAxis = newValue;
      newValue.Orientation = Orientation.Vertical;
    }
    sfSurfaceChart.OnAxisChanged(e);
  }

  public SurfaceAxis ZAxis
  {
    get => (SurfaceAxis) this.GetValue(SfSurfaceChart.ZAxisProperty);
    set => this.SetValue(SfSurfaceChart.ZAxisProperty, (object) value);
  }

  private static void OnZAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    SfSurfaceChart sfSurfaceChart = d as SfSurfaceChart;
    SurfaceAxis newValue = e.NewValue as SurfaceAxis;
    if (sfSurfaceChart == null)
      return;
    if (newValue != null)
    {
      sfSurfaceChart.InterernalZAxis = newValue;
      newValue.Orientation = Orientation.Horizontal;
    }
    sfSurfaceChart.OnAxisChanged(e);
  }

  private void OnAxisChanged(DependencyPropertyChangedEventArgs e)
  {
    SurfaceAxis newValue = e.NewValue as SurfaceAxis;
    if (e.OldValue is SurfaceAxis oldValue && this.Axes.Contains(oldValue))
      this.Axes.Remove(oldValue);
    if (newValue == null || this.Axes.Contains(newValue))
      return;
    this.Axes.Add(newValue);
    newValue.Area = this;
  }

  public ChartColorBar ColorBar
  {
    get => (ChartColorBar) this.GetValue(SfSurfaceChart.ColorBarProperty);
    set => this.SetValue(SfSurfaceChart.ColorBarProperty, (object) value);
  }

  private static void OnColorBarPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfSurfaceChart sfSurfaceChart))
      return;
    sfSurfaceChart.IsUpdateLegend = true;
    sfSurfaceChart.ScheduleUpdate();
  }

  public bool ApplyGradientBrush
  {
    get => (bool) this.GetValue(SfSurfaceChart.ApplyGradientBrushProperty);
    set => this.SetValue(SfSurfaceChart.ApplyGradientBrushProperty, (object) value);
  }

  private static void OnGradientPalatteChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfSurfaceChart sfSurfaceChart))
      return;
    sfSurfaceChart.IsPointsGenerated = false;
    sfSurfaceChart.CanDrawMaterial = false;
    sfSurfaceChart.ScheduleUpdate();
  }

  public string XBindingPath
  {
    get => (string) this.GetValue(SfSurfaceChart.XBindingPathProperty);
    set => this.SetValue(SfSurfaceChart.XBindingPathProperty, (object) value);
  }

  public string YBindingPath
  {
    get => (string) this.GetValue(SfSurfaceChart.YBindingPathProperty);
    set => this.SetValue(SfSurfaceChart.YBindingPathProperty, (object) value);
  }

  public string ZBindingPath
  {
    get => (string) this.GetValue(SfSurfaceChart.ZBindingPathProperty);
    set => this.SetValue(SfSurfaceChart.ZBindingPathProperty, (object) value);
  }

  public int RowSize
  {
    get => (int) this.GetValue(SfSurfaceChart.RowSizeProperty);
    set => this.SetValue(SfSurfaceChart.RowSizeProperty, (object) value);
  }

  public int ColumnSize
  {
    get => (int) this.GetValue(SfSurfaceChart.ColumnSizeProperty);
    set => this.SetValue(SfSurfaceChart.ColumnSizeProperty, (object) value);
  }

  public string LegendLabelFormat
  {
    get => (string) this.GetValue(SfSurfaceChart.LegendLabelFormatProperty);
    set => this.SetValue(SfSurfaceChart.LegendLabelFormatProperty, (object) value);
  }

  private static void OnLegendLabelFormatPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfSurfaceChart sfSurfaceChart))
      return;
    sfSurfaceChart.IsUpdateLegend = true;
    sfSurfaceChart.ScheduleUpdate();
  }

  public Brush WireframeStroke
  {
    get => (Brush) this.GetValue(SfSurfaceChart.WireframeStrokeProperty);
    set => this.SetValue(SfSurfaceChart.WireframeStrokeProperty, (object) value);
  }

  public double WireframeStrokeThickness
  {
    get => (double) this.GetValue(SfSurfaceChart.WireframeStrokeThicknessProperty);
    set => this.SetValue(SfSurfaceChart.WireframeStrokeThicknessProperty, (object) value);
  }

  private static void OnWireframePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfSurfaceChart sfSurfaceChart))
      return;
    sfSurfaceChart.IsPointsGenerated = false;
    sfSurfaceChart.ScheduleUpdate();
  }

  public SurfaceType Type
  {
    get => (SurfaceType) this.GetValue(SfSurfaceChart.TypeProperty);
    set => this.SetValue(SfSurfaceChart.TypeProperty, (object) value);
  }

  private static void OnTypePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as SfSurfaceChart).OnTypePropertyChanged(e);
  }

  private void OnTypePropertyChanged(DependencyPropertyChangedEventArgs e)
  {
    this.drawWireframe = (SurfaceType) e.NewValue == SurfaceType.WireframeSurface || (SurfaceType) e.NewValue == SurfaceType.WireframeContour;
    this.IsContour = (SurfaceType) e.NewValue == SurfaceType.Contour || (SurfaceType) e.NewValue == SurfaceType.WireframeContour;
    this.PositionCamera(this.Viewport.Camera as ProjectionCamera);
    this.IsPointsGenerated = false;
    this.ScheduleUpdate();
    this.CanDrawWall = false;
  }

  public CameraProjection CameraProjection
  {
    get => (CameraProjection) this.GetValue(SfSurfaceChart.CameraProjectionProperty);
    set => this.SetValue(SfSurfaceChart.CameraProjectionProperty, (object) value);
  }

  private static void OnCameraProjectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as SfSurfaceChart).CreateCamera();
  }

  public double ZoomLevel
  {
    get => (double) this.GetValue(SfSurfaceChart.ZoomLevelProperty);
    set => this.SetValue(SfSurfaceChart.ZoomLevelProperty, (object) value);
  }

  private static void OnZoomLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfSurfaceChart sfSurfaceChart))
      return;
    sfSurfaceChart.CreateCamera();
    sfSurfaceChart.ScheduleUpdate();
  }

  public bool ShowContourLine
  {
    get => (bool) this.GetValue(SfSurfaceChart.ShowContourLineProperty);
    set => this.SetValue(SfSurfaceChart.ShowContourLineProperty, (object) value);
  }

  private static void OnContourLineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfSurfaceChart sfSurfaceChart))
      return;
    sfSurfaceChart.IsPointsGenerated = false;
    sfSurfaceChart.ScheduleUpdate();
  }

  public bool EnableZooming
  {
    get => (bool) this.GetValue(SfSurfaceChart.EnableZoomingProperty);
    set => this.SetValue(SfSurfaceChart.EnableZoomingProperty, (object) value);
  }

  public int BrushCount
  {
    get => (int) this.GetValue(SfSurfaceChart.BrushCountProperty);
    set => this.SetValue(SfSurfaceChart.BrushCountProperty, (object) value);
  }

  private static void OnBrushCountPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfSurfaceChart sfSurfaceChart))
      return;
    sfSurfaceChart.IsPointsGenerated = false;
    sfSurfaceChart.CanDrawMaterial = false;
    sfSurfaceChart.IsUpdateLegend = true;
    sfSurfaceChart.ScheduleUpdate();
  }

  public override void OnApplyTemplate()
  {
    this.Container = this.GetTemplateChild("Part_Container") as ContentControl;
    this.DockPanel = this.GetTemplateChild("Part_DockPanel") as ChartDockPanel;
    this.RootPanel = this.GetTemplateChild("Part_LayoutRoot") as ChartRootPanel;
    if (this.Container != null)
      this.Container.Content = (object) this.Viewport;
    if (this.RootPanel != null)
      this.RootPanel.Surface = this;
    base.OnApplyTemplate();
  }

  internal override void UpdateArea()
  {
    if (!this.RootPanelDesiredSize.HasValue)
      return;
    this.CalculateViewSize();
    if (!this.IsPointsGenerated)
    {
      this.UpdateData();
      this.UpdateRange();
    }
    this.InitializeDefaultAxes();
    if (this.ColorBar != null && this.IsUpdateLegend)
    {
      this.UpdateColorBar();
      this.IsUpdateLegend = false;
    }
    this.UpdateColorBarArrangeRect();
    this.UpdateAxis(this.RootPanelDesiredSize.Value);
    if (!this.CanDrawWall)
    {
      this.DrawWall();
      this.CanDrawWall = true;
    }
    this.SetLight();
    this.DrawAxisElements();
    this.UpdateGridLines();
    if (!this.IsPointsGenerated || this.RangeChanged)
    {
      this.CreateSurface();
      this.RangeChanged = false;
    }
    this.IsPointsGenerated = true;
    this.IsUpdateDispatched = false;
  }

  private void CalculateViewSize()
  {
    System.Windows.Media.Media3D.Matrix3D viewMatrix = this.CreateViewMatrix();
    this.ViewBoxSize = new Size(viewMatrix.Transform(new Point3D(1.3, 1.0, 1.0)).X - viewMatrix.Transform(new Point3D(-1.3, 1.0, 1.0)).X, viewMatrix.Transform(new Point3D(1.0, -1.05, 1.0)).Y - viewMatrix.Transform(new Point3D(1.0, 1.05, 1.0)).Y);
  }

  private System.Windows.Media.Media3D.Matrix3D CreateViewMatrix()
  {
    System.Windows.Media.Media3D.Matrix3D identity = System.Windows.Media.Media3D.Matrix3D.Identity;
    if (this.Viewport.Camera is PerspectiveCamera)
    {
      identity.Append(MeshGenerator.GetDirectionMatrix());
      identity.Append(MeshGenerator.GetPerspectiveCameraMatrix(this.Viewport.Camera as PerspectiveCamera, this.RootPanelDesiredSize.Value.Width / this.RootPanelDesiredSize.Value.Height));
    }
    else
      identity.Append(MeshGenerator.GetOrthographicCameraMatrix(this.Viewport.Camera as OrthographicCamera, this.RootPanelDesiredSize.Value.Width / this.RootPanelDesiredSize.Value.Height));
    identity.Append(MeshGenerator.GetViewportTransform(this.RootPanelDesiredSize.Value));
    return identity;
  }

  private void InitializeDefaultAxes()
  {
    if (this.XAxis == null)
      this.XAxis = new SurfaceAxis();
    if (this.YAxis == null)
      this.YAxis = new SurfaceAxis();
    if (this.ZAxis != null)
      return;
    this.ZAxis = new SurfaceAxis();
  }

  private void UpdateData()
  {
    if (this.Data.IsDataAvailable)
      this.CalculatePoints();
    this.dataPoints = this.GetPoints();
    this.points = this.dataPoints.Clone() as Point3D[,];
  }

  public virtual Point3D[,] GetPoints()
  {
    Point3D[,] points = new Point3D[this.RowSize, this.ColumnSize];
    int index1 = 0;
    List<double> xvalues = this.GetXValues();
    List<double> zvalues = this.GetZValues();
    if (xvalues != null && zvalues != null && xvalues.Count != 0)
    {
      for (int index2 = 0; index2 < this.RowSize; ++index2)
      {
        for (int index3 = 0; index3 < this.ColumnSize; ++index3)
        {
          if (index1 == xvalues.Count)
          {
            points.Address(index2, index3) = new Point3D()
            {
              X = (double) index2,
              Y = 0.0,
              Z = (double) index3
            };
          }
          else
          {
            points.Address(index2, index3) = new Point3D()
            {
              X = xvalues[index1],
              Y = this.YValues[index1],
              Z = zvalues[index1]
            };
            ++index1;
          }
        }
      }
      this.XRange = new DoubleRange(xvalues.Min(), xvalues.Max());
      this.YRange = new DoubleRange(this.YValues.Min(), this.YValues.Max());
      this.ZRange = new DoubleRange(zvalues.Min(), zvalues.Max());
    }
    return points;
  }

  private void CalculatePoints()
  {
    this.XValues = (IEnumerable) new List<double>();
    this.YValues = new List<double>();
    this.ZValues = (IEnumerable) new List<double>();
    this.XValues = (IEnumerable) this.Data.XValues;
    this.YValues = this.Data.YValues;
    this.ZValues = (IEnumerable) this.Data.ZValues;
  }

  protected internal List<double> GetXValues()
  {
    if (this.XValues == null)
      return (List<double>) null;
    double xIndexValues = 0.0;
    if (!(this.XValues is List<double> xvalues))
      xvalues = (this.XValues as List<string>).Select<string, double>((Func<string, double>) (val => Math.Floor(xIndexValues++ / (double) this.ColumnSize))).ToList<double>();
    return xvalues;
  }

  protected internal List<double> GetZValues()
  {
    if (this.ZValues == null)
      return (List<double>) null;
    double xIndexValues = 0.0;
    if (!(this.ZValues is List<double> zvalues))
      zvalues = (this.ZValues as List<string>).Select<string, double>((Func<string, double>) (val => xIndexValues++ % (double) this.RowSize)).ToList<double>();
    return zvalues;
  }

  private void UpdateRange()
  {
    this.XRange = this.XRange.IsEmpty ? new DoubleRange(MeshGenerator.GetMin("X", this.dataPoints), MeshGenerator.GetMax("X", this.dataPoints)) : this.XRange;
    this.YRange = this.YRange.IsEmpty ? new DoubleRange(MeshGenerator.GetMin("Y", this.dataPoints), MeshGenerator.GetMax("Y", this.dataPoints)) : this.YRange;
    this.ZRange = this.ZRange.IsEmpty ? new DoubleRange(MeshGenerator.GetMin("Z", this.dataPoints), MeshGenerator.GetMax("Z", this.dataPoints)) : this.ZRange;
  }

  private void UpdateColorBar()
  {
    if (!this.DockPanel.Children.Contains((UIElement) this.ColorBar))
      this.DockPanel.Children.Add((UIElement) this.ColorBar);
    this.ColorBar.Area = this;
    this.ColorBar.UpdateColorBarItemsSource();
  }

  private void UpdateAxis(Size size)
  {
    foreach (SurfaceAxis ax in (Collection<SurfaceAxis>) this.Axes)
      size = this.RenderAxis(size, ax);
  }

  internal Size RenderAxis(Size size, SurfaceAxis axis)
  {
    if (axis.Orientation == Orientation.Horizontal)
    {
      if (axis == this.InterernalXAxis)
      {
        axis.ComputeDesiredSize(new Size(this.ViewBoxSize.Width, size.Height));
        axis.ArrangeAxisPanel(new Size(this.ViewBoxSize.Width, size.Height));
      }
      else
      {
        axis.ComputeDesiredSize(new Size(this.ViewBoxSize.Height, size.Height));
        axis.ArrangeAxisPanel(new Size(this.ViewBoxSize.Height, size.Height));
      }
    }
    else
    {
      axis.ComputeDesiredSize(new Size(size.Width, this.ViewBoxSize.Height));
      axis.ArrangeAxisPanel(new Size(size.Width, this.ViewBoxSize.Height));
    }
    return size;
  }

  private void DrawWall()
  {
    if (this.IsContour)
    {
      this.WallContent = (Model3DGroup) null;
    }
    else
    {
      Model3DGroup model3Dgroup = new Model3DGroup();
      if (this.ShowLeftWall)
      {
        DiffuseMaterial diffuseMaterial = new DiffuseMaterial(this.LeftWallBrush);
        GeometryModel3D geometryModel3D = new GeometryModel3D();
        geometryModel3D.Geometry = (Geometry3D) MeshGenerator.BuildWall(this.WallThickness.Left, 2.1, 2.1);
        geometryModel3D.Material = (Material) diffuseMaterial;
        geometryModel3D.BackMaterial = (Material) diffuseMaterial;
        if (this.Rotate >= 0.0 && this.Rotate < 90.0)
          geometryModel3D.Transform = (Transform3D) new TranslateTransform3D(-1.3 - this.WallThickness.Left / 2.0, 0.0, 0.0);
        else if (this.Rotate >= 90.0 && this.Rotate < 180.0)
          geometryModel3D.Transform = (Transform3D) new TranslateTransform3D(-1.3 - this.WallThickness.Left / 2.0, 0.0, 0.0);
        if (this.Rotate >= 180.0 && this.Rotate < 270.0)
          geometryModel3D.Transform = (Transform3D) new TranslateTransform3D(1.3 + this.WallThickness.Left / 2.0, 0.0, 0.0);
        else if (this.Rotate >= 270.0 && this.Rotate <= 360.0)
          geometryModel3D.Transform = (Transform3D) new TranslateTransform3D(1.3 + this.WallThickness.Left / 2.0, 0.0, 0.0);
        model3Dgroup.Children.Add((Model3D) geometryModel3D);
      }
      if (this.ShowBackWall)
      {
        DiffuseMaterial diffuseMaterial = new DiffuseMaterial(this.BackWallBrush);
        GeometryModel3D geometryModel3D = new GeometryModel3D();
        geometryModel3D.Geometry = (Geometry3D) MeshGenerator.BuildWall(2.6, 2.1, this.WallThickness.Back);
        geometryModel3D.Material = (Material) diffuseMaterial;
        geometryModel3D.BackMaterial = (Material) diffuseMaterial;
        if (this.Rotate >= 0.0 && this.Rotate < 90.0)
          geometryModel3D.Transform = (Transform3D) new TranslateTransform3D(0.0, 0.0, -1.05 - this.WallThickness.Back / 2.0);
        if (this.Rotate >= 90.0 && this.Rotate < 180.0)
          geometryModel3D.Transform = (Transform3D) new TranslateTransform3D(0.0, 0.0, 1.05 + this.WallThickness.Back / 2.0);
        else if (this.Rotate >= 180.0 && this.Rotate < 270.0)
          geometryModel3D.Transform = (Transform3D) new TranslateTransform3D(0.0, 0.0, 1.05 + this.WallThickness.Back / 2.0);
        else if (this.Rotate >= 270.0 && this.Rotate <= 360.0)
          geometryModel3D.Transform = (Transform3D) new TranslateTransform3D(0.0, 0.0, -1.05 - this.WallThickness.Back / 2.0);
        model3Dgroup.Children.Add((Model3D) geometryModel3D);
      }
      if (this.ShowBottomWall)
      {
        DiffuseMaterial diffuseMaterial = new DiffuseMaterial(this.BottomWallBrush);
        GeometryModel3D geometryModel3D = new GeometryModel3D();
        geometryModel3D.Geometry = (Geometry3D) MeshGenerator.BuildWall(2.6, this.WallThickness.Bottom, 2.1);
        geometryModel3D.Material = (Material) diffuseMaterial;
        geometryModel3D.BackMaterial = (Material) diffuseMaterial;
        geometryModel3D.Transform = (Transform3D) new TranslateTransform3D(0.0, -1.05 - this.WallThickness.Bottom / 2.0, 0.0);
        model3Dgroup.Children.Add((Model3D) geometryModel3D);
      }
      this.WallContent = model3Dgroup;
    }
  }

  private void SetLight() => this.LightContent = this.DefineLights();

  protected internal virtual Model3DGroup DefineLights()
  {
    Model3DGroup model3Dgroup = new Model3DGroup();
    if (this.IsContour)
    {
      Model3DCollection children1 = model3Dgroup.Children;
      AmbientLight ambientLight1 = new AmbientLight();
      ambientLight1.Color = Colors.Gray;
      AmbientLight ambientLight2 = ambientLight1;
      children1.Add((Model3D) ambientLight2);
      Model3DCollection children2 = model3Dgroup.Children;
      DirectionalLight directionalLight1 = new DirectionalLight();
      directionalLight1.Color = Colors.Gray;
      directionalLight1.Direction = new System.Windows.Media.Media3D.Vector3D(0.0, -1.0, -1.0);
      DirectionalLight directionalLight2 = directionalLight1;
      children2.Add((Model3D) directionalLight2);
      Model3DCollection children3 = model3Dgroup.Children;
      DirectionalLight directionalLight3 = new DirectionalLight();
      directionalLight3.Color = Colors.Gray;
      directionalLight3.Direction = new System.Windows.Media.Media3D.Vector3D(-1.0, 1.0, 0.0);
      DirectionalLight directionalLight4 = directionalLight3;
      children3.Add((Model3D) directionalLight4);
      return model3Dgroup;
    }
    Model3DCollection children4 = model3Dgroup.Children;
    AmbientLight ambientLight3 = new AmbientLight();
    ambientLight3.Color = Colors.Gray;
    AmbientLight ambientLight4 = ambientLight3;
    children4.Add((Model3D) ambientLight4);
    Model3DCollection children5 = model3Dgroup.Children;
    DirectionalLight directionalLight5 = new DirectionalLight();
    directionalLight5.Color = Colors.Gray;
    directionalLight5.Direction = new System.Windows.Media.Media3D.Vector3D(-0.4, -0.2, -0.8);
    DirectionalLight directionalLight6 = directionalLight5;
    children5.Add((Model3D) directionalLight6);
    Model3DCollection children6 = model3Dgroup.Children;
    DirectionalLight directionalLight7 = new DirectionalLight();
    directionalLight7.Color = Colors.Gray;
    directionalLight7.Direction = new System.Windows.Media.Media3D.Vector3D(1.0, -2.0, 2.0);
    DirectionalLight directionalLight8 = directionalLight7;
    children6.Add((Model3D) directionalLight8);
    return model3Dgroup;
  }

  private void DrawAxisElements()
  {
    Model3DGroup model3Dgroup = new Model3DGroup();
    foreach (SurfaceAxis ax in (Collection<SurfaceAxis>) this.Axes)
    {
      if (this.Type == SurfaceType.Contour || this.Type == SurfaceType.WireframeContour)
        model3Dgroup.Children.Add(this.DrawContourAxis(ax));
      else
        model3Dgroup.Children.Add(this.DrawAxis(ax));
    }
    this.AxisContent = model3Dgroup;
  }

  private Model3D DrawContourAxis(SurfaceAxis axis)
  {
    if (axis == this.InterernalXAxis)
    {
      axis.ComputeDesiredSize(new Size(this.ViewBoxSize.Width, this.RootPanelDesiredSize.Value.Height));
      axis.ArrangeAxisPanel(new Size(this.ViewBoxSize.Width, this.RootPanelDesiredSize.Value.Height));
    }
    else
    {
      axis.ComputeDesiredSize(new Size(this.ViewBoxSize.Height, this.RootPanelDesiredSize.Value.Height));
      axis.ArrangeAxisPanel(new Size(this.ViewBoxSize.Height, this.RootPanelDesiredSize.Value.Height));
    }
    GeometryModel3D geometryModel3D = new GeometryModel3D();
    double width = 2.5;
    double num = 2.0;
    if (axis == this.InterernalXAxis)
    {
      double height = axis.AxisDesiredSize.Height / this.ViewBoxSize.Height * 2.1;
      double leftOffset = axis.LeftOffset / this.ViewBoxSize.Width * width;
      double rightOffset = axis.RightOffset / this.ViewBoxSize.Width * width;
      geometryModel3D.Geometry = (Geometry3D) MeshGenerator.PlaneXYZ(width, height, leftOffset, rightOffset);
      Transform3DGroup transform3Dgroup = new Transform3DGroup();
      RotateTransform3D rotateTransform3D = new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
      {
        Angle = -90.0,
        Axis = new System.Windows.Media.Media3D.Vector3D(1.0, 0.0, 0.0)
      });
      TranslateTransform3D translateTransform3D = new TranslateTransform3D(0.0, num / 2.0 + 0.05 + height / 2.0 + this.WallThickness.Bottom, num / 2.0 + 0.05 + height / 2.0);
      transform3Dgroup.Children.Add((Transform3D) rotateTransform3D);
      transform3Dgroup.Children.Add((Transform3D) translateTransform3D);
      geometryModel3D.Transform = (Transform3D) transform3Dgroup;
    }
    else if (axis == this.InterernalZAxis)
    {
      double height = axis.AxisDesiredSize.Height / this.ViewBoxSize.Height * 2.1;
      double leftOffset = axis.LeftOffset / this.ViewBoxSize.Width * width;
      double rightOffset = axis.RightOffset / this.ViewBoxSize.Width * width;
      geometryModel3D.Geometry = (Geometry3D) MeshGenerator.PlaneXYZ(2.0, height, leftOffset, rightOffset);
      Transform3DGroup transform3Dgroup = new Transform3DGroup();
      RotateTransform3D rotateTransform3D1 = new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
      {
        Angle = 90.0,
        Axis = new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0)
      });
      RotateTransform3D rotateTransform3D2 = new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
      {
        Angle = -90.0,
        Axis = new System.Windows.Media.Media3D.Vector3D(1.0, 0.0, 0.0)
      });
      TranslateTransform3D translateTransform3D = new TranslateTransform3D(width / 2.0 + 0.05 + height / 2.0, num / 2.0 + 0.05 + height / 2.0 + this.WallThickness.Bottom, 0.0);
      transform3Dgroup.Children.Add((Transform3D) rotateTransform3D2);
      transform3Dgroup.Children.Add((Transform3D) rotateTransform3D1);
      transform3Dgroup.Children.Add((Transform3D) translateTransform3D);
      geometryModel3D.Transform = (Transform3D) transform3Dgroup;
    }
    MaterialGroup materialGroup = new MaterialGroup();
    VisualBrush target = new VisualBrush((Visual) axis);
    RenderOptions.SetCachingHint((DependencyObject) target, CachingHint.Cache);
    materialGroup.Children.Add((Material) new DiffuseMaterial(axis.Background));
    materialGroup.Children.Add((Material) new DiffuseMaterial((Brush) target));
    materialGroup.Children.Add((Material) new EmissiveMaterial((Brush) target));
    geometryModel3D.Material = (Material) materialGroup;
    geometryModel3D.BackMaterial = (Material) materialGroup;
    return (Model3D) geometryModel3D;
  }

  private Model3D DrawAxis(SurfaceAxis axis)
  {
    GeometryModel3D geometry = new GeometryModel3D();
    double xshift = 2.5;
    double yshift = 2.0;
    double zshift = 2.0;
    if (axis == this.InterernalXAxis)
      this.DrawXAxis(axis, geometry, xshift, yshift, zshift);
    if (axis == this.InterernalYAxis)
      this.DrawYAxis(axis, geometry, xshift, yshift);
    if (axis == this.InterernalZAxis)
      this.DrawZAxis(axis, geometry, xshift, zshift);
    MaterialGroup materialGroup = new MaterialGroup();
    VisualBrush target = new VisualBrush((Visual) axis);
    RenderOptions.SetCachingHint((DependencyObject) target, CachingHint.Cache);
    materialGroup.Children.Add((Material) new DiffuseMaterial(axis.Background));
    materialGroup.Children.Add((Material) new DiffuseMaterial((Brush) target));
    materialGroup.Children.Add((Material) new EmissiveMaterial((Brush) target));
    geometry.Material = (Material) materialGroup;
    geometry.BackMaterial = (Material) materialGroup;
    return (Model3D) geometry;
  }

  private void DrawZAxis(SurfaceAxis axis, GeometryModel3D geometry, double xshift, double zshift)
  {
    double height = axis.AxisDesiredSize.Height / this.ViewBoxSize.Height * 2.1;
    double leftOffset = axis.LeftOffset / this.ViewBoxSize.Height * zshift;
    double rightOffset = axis.RightOffset / this.ViewBoxSize.Height * zshift;
    geometry.Geometry = (Geometry3D) MeshGenerator.PlaneXYZ(zshift, height, leftOffset, rightOffset);
    if (this.Rotate >= 0.0 && this.Rotate < 90.0)
    {
      axis.IsInversed = false;
      Transform3DGroup transform3Dgroup = new Transform3DGroup();
      RotateTransform3D rotateTransform3D = new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
      {
        Angle = 90.0,
        Axis = new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0)
      });
      TranslateTransform3D translateTransform3D = new TranslateTransform3D(xshift / 2.0 + 0.05, -(zshift / 2.0 + 0.05) - height / 2.0 - this.WallThickness.Bottom, 0.0);
      transform3Dgroup.Children.Add((Transform3D) rotateTransform3D);
      transform3Dgroup.Children.Add((Transform3D) translateTransform3D);
      geometry.Transform = (Transform3D) transform3Dgroup;
    }
    else if (this.Rotate >= 90.0 && this.Rotate < 180.0)
    {
      axis.IsInversed = false;
      Transform3DGroup transform3Dgroup = new Transform3DGroup();
      RotateTransform3D rotateTransform3D = new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
      {
        Angle = 90.0,
        Axis = new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0)
      });
      TranslateTransform3D translateTransform3D = new TranslateTransform3D(xshift / 2.0 + 0.05, -(zshift / 2.0 + 0.05) - height / 2.0 - this.WallThickness.Bottom, 0.0);
      transform3Dgroup.Children.Add((Transform3D) rotateTransform3D);
      transform3Dgroup.Children.Add((Transform3D) translateTransform3D);
      geometry.Transform = (Transform3D) transform3Dgroup;
    }
    else if (this.Rotate >= 180.0 && this.Rotate < 270.0)
    {
      axis.IsInversed = true;
      Transform3DGroup transform3Dgroup = new Transform3DGroup();
      RotateTransform3D rotateTransform3D = new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
      {
        Angle = -90.0,
        Axis = new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0)
      });
      TranslateTransform3D translateTransform3D = new TranslateTransform3D(-(xshift / 2.0) - 0.05, -(zshift / 2.0 + 0.05) - height / 2.0 - this.WallThickness.Bottom, 0.0);
      transform3Dgroup.Children.Add((Transform3D) rotateTransform3D);
      transform3Dgroup.Children.Add((Transform3D) translateTransform3D);
      geometry.Transform = (Transform3D) transform3Dgroup;
    }
    else
    {
      if (this.Rotate < 270.0 || this.Rotate > 360.0)
        return;
      axis.IsInversed = true;
      Transform3DGroup transform3Dgroup = new Transform3DGroup();
      RotateTransform3D rotateTransform3D = new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
      {
        Angle = -90.0,
        Axis = new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0)
      });
      TranslateTransform3D translateTransform3D = new TranslateTransform3D(-(xshift / 2.0) - 0.05, -(zshift / 2.0 + 0.05) - height / 2.0 - this.WallThickness.Bottom, 0.0);
      transform3Dgroup.Children.Add((Transform3D) rotateTransform3D);
      transform3Dgroup.Children.Add((Transform3D) translateTransform3D);
      geometry.Transform = (Transform3D) transform3Dgroup;
    }
  }

  private void DrawYAxis(SurfaceAxis axis, GeometryModel3D geometry, double xshift, double yshift)
  {
    double width = axis.AxisDesiredSize.Width / this.ViewBoxSize.Width * 2.6;
    double leftOffset = axis.LeftOffset / this.ViewBoxSize.Height * yshift;
    double rightOffset = axis.RightOffset / this.ViewBoxSize.Height * yshift;
    geometry.Geometry = (Geometry3D) MeshGenerator.PlaneY(width, yshift, leftOffset, rightOffset);
    if (this.Rotate >= 0.0 && this.Rotate < 45.0)
    {
      if (this.isOppositeAxis)
        this.PositioningAxis(axis, this.isOppositeAxis);
      Transform3DGroup transform3Dgroup = new Transform3DGroup();
      RotateTransform3D rotateTransform3D = new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
      {
        Angle = this.Rotate,
        Axis = new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0)
      });
      double num1 = width / 2.0 * Math.Sin(this.Rotate * (Math.PI / 180.0));
      double num2 = width / 2.0 - Math.Abs(width / 2.0 * Math.Cos(this.Rotate * (Math.PI / 180.0)));
      TranslateTransform3D translateTransform3D = new TranslateTransform3D(-(xshift / 2.0 + 0.05) - width / 2.0 - this.WallThickness.Left + num2, 0.0, yshift / 2.0 + 0.05 + num1);
      transform3Dgroup.Children.Add((Transform3D) rotateTransform3D);
      transform3Dgroup.Children.Add((Transform3D) translateTransform3D);
      geometry.Transform = (Transform3D) transform3Dgroup;
    }
    else if (this.Rotate >= 45.0 && this.Rotate < 90.0)
    {
      if (!this.isOppositeAxis)
        this.PositioningAxis(axis, this.isOppositeAxis);
      double num3 = 90.0 - this.Rotate;
      Transform3DGroup transform3Dgroup = new Transform3DGroup();
      RotateTransform3D rotateTransform3D = new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
      {
        Angle = (90.0 - num3),
        Axis = new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0)
      });
      double num4 = width / 2.0 * Math.Sin(num3 * (Math.PI / 180.0));
      double num5 = width / 2.0 - Math.Abs(width / 2.0 * Math.Cos(num3 * (Math.PI / 180.0)));
      TranslateTransform3D translateTransform3D = new TranslateTransform3D(xshift / 2.0 + 0.05 + num4, 0.0, -(yshift / 2.0 + 0.05) + width / 2.0 - width - this.WallThickness.Back + num5);
      transform3Dgroup.Children.Add((Transform3D) rotateTransform3D);
      transform3Dgroup.Children.Add((Transform3D) translateTransform3D);
      geometry.Transform = (Transform3D) transform3Dgroup;
    }
    else if (this.Rotate >= 90.0 && this.Rotate < 135.0)
    {
      if (this.isOppositeAxis)
        this.PositioningAxis(axis, this.isOppositeAxis);
      Transform3DGroup transform3Dgroup = new Transform3DGroup();
      double num6 = this.Rotate - 90.0;
      RotateTransform3D rotateTransform3D = new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
      {
        Angle = (90.0 + num6)
      });
      double num7 = width / 2.0 * Math.Sin(num6 * (Math.PI / 180.0));
      double num8 = width / 2.0 - Math.Abs(width / 2.0 * Math.Cos(num6 * (Math.PI / 180.0)));
      TranslateTransform3D translateTransform3D = new TranslateTransform3D(xshift / 2.0 + 0.05 - width / 2.0 + width / 2.0 + num7, 0.0, width / 2.0 + yshift / 2.0 + 0.05 + this.WallThickness.Back - num8);
      transform3Dgroup.Children.Add((Transform3D) rotateTransform3D);
      transform3Dgroup.Children.Add((Transform3D) translateTransform3D);
      geometry.Transform = (Transform3D) transform3Dgroup;
    }
    else if (this.Rotate >= 135.0 && this.Rotate < 180.0)
    {
      if (!this.isOppositeAxis)
        this.PositioningAxis(axis, this.isOppositeAxis);
      Transform3DGroup transform3Dgroup = new Transform3DGroup();
      double num9 = 180.0 - this.Rotate;
      RotateTransform3D rotateTransform3D = new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
      {
        Angle = (180.0 - num9)
      });
      double num10 = width / 2.0 * Math.Sin(num9 * (Math.PI / 180.0));
      double num11 = width / 2.0 - Math.Abs(width / 2.0 * Math.Cos(num9 * (Math.PI / 180.0)));
      TranslateTransform3D translateTransform3D = new TranslateTransform3D(-(xshift / 2.0 + 0.05) - width / 2.0 - this.WallThickness.Left + num11, 0.0, -(yshift / 2.0 + 0.05) + width / 2.0 - width / 2.0 - num10);
      transform3Dgroup.Children.Add((Transform3D) rotateTransform3D);
      transform3Dgroup.Children.Add((Transform3D) translateTransform3D);
      geometry.Transform = (Transform3D) transform3Dgroup;
    }
    else if (this.Rotate >= 180.0 && this.Rotate < 225.0)
    {
      if (this.isOppositeAxis)
        this.PositioningAxis(axis, this.isOppositeAxis);
      Transform3DGroup transform3Dgroup = new Transform3DGroup();
      double num12 = this.Rotate - 180.0;
      RotateTransform3D rotateTransform3D = new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
      {
        Angle = (180.0 + num12)
      });
      double num13 = width / 2.0 * Math.Sin(num12 * (Math.PI / 180.0));
      double num14 = width / 2.0 - Math.Abs(width / 2.0 * Math.Cos(num12 * (Math.PI / 180.0)));
      TranslateTransform3D translateTransform3D = new TranslateTransform3D(xshift / 2.0 + 0.05 + width / 2.0 + this.WallThickness.Left - num14, 0.0, -(yshift / 2.0 + 0.05) + width / 2.0 - width / 2.0 - num13);
      transform3Dgroup.Children.Add((Transform3D) rotateTransform3D);
      transform3Dgroup.Children.Add((Transform3D) translateTransform3D);
      geometry.Transform = (Transform3D) transform3Dgroup;
    }
    else if (this.Rotate >= 225.0 && this.Rotate < 270.0)
    {
      if (!this.isOppositeAxis)
        this.PositioningAxis(axis, this.isOppositeAxis);
      Transform3DGroup transform3Dgroup = new Transform3DGroup();
      double num15 = 270.0 - this.Rotate;
      RotateTransform3D rotateTransform3D = new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
      {
        Angle = (-90.0 - num15)
      });
      double num16 = width / 2.0 * Math.Sin(num15 * (Math.PI / 180.0));
      double num17 = width / 2.0 - Math.Abs(width / 2.0 * Math.Cos(num15 * (Math.PI / 180.0)));
      TranslateTransform3D translateTransform3D = new TranslateTransform3D(-(xshift / 2.0 + 0.05) - num16, 0.0, yshift / 2.0 + 0.05 + width / 2.0 + this.WallThickness.Back - num17);
      transform3Dgroup.Children.Add((Transform3D) rotateTransform3D);
      transform3Dgroup.Children.Add((Transform3D) translateTransform3D);
      geometry.Transform = (Transform3D) transform3Dgroup;
    }
    else if (this.Rotate >= 270.0 && this.Rotate < 315.0)
    {
      if (this.isOppositeAxis)
        this.PositioningAxis(axis, this.isOppositeAxis);
      Transform3DGroup transform3Dgroup = new Transform3DGroup();
      double num18 = this.Rotate - 270.0;
      RotateTransform3D rotateTransform3D = new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
      {
        Angle = (num18 - 90.0)
      });
      double num19 = width / 2.0 * Math.Sin(num18 * (Math.PI / 180.0));
      double num20 = width / 2.0 - Math.Abs(width / 2.0 * Math.Cos(num18 * (Math.PI / 180.0)));
      TranslateTransform3D translateTransform3D = new TranslateTransform3D(-(xshift / 2.0 + 0.05) - width / 2.0 + width / 2.0 - num19, 0.0, -(yshift / 2.0 + 0.05) + width / 2.0 - width - this.WallThickness.Back + num20);
      transform3Dgroup.Children.Add((Transform3D) rotateTransform3D);
      transform3Dgroup.Children.Add((Transform3D) translateTransform3D);
      geometry.Transform = (Transform3D) transform3Dgroup;
    }
    else
    {
      if (this.Rotate < 315.0 || this.Rotate > 360.0)
        return;
      if (!this.isOppositeAxis)
        this.PositioningAxis(axis, this.isOppositeAxis);
      Transform3DGroup transform3Dgroup = new Transform3DGroup();
      double num21 = 360.0 - this.Rotate;
      RotateTransform3D rotateTransform3D = new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
      {
        Angle = -num21
      });
      double num22 = width / 2.0 * Math.Sin(num21 * (Math.PI / 180.0));
      double num23 = width / 2.0 - Math.Abs(width / 2.0 * Math.Cos(num21 * (Math.PI / 180.0)));
      TranslateTransform3D translateTransform3D = new TranslateTransform3D(xshift / 2.0 + 0.05 + width / 2.0 + this.WallThickness.Left - num23, 0.0, yshift / 2.0 + 0.05 + num22);
      transform3Dgroup.Children.Add((Transform3D) rotateTransform3D);
      transform3Dgroup.Children.Add((Transform3D) translateTransform3D);
      geometry.Transform = (Transform3D) transform3Dgroup;
    }
  }

  private void DrawXAxis(
    SurfaceAxis axis,
    GeometryModel3D geometry,
    double xshift,
    double yshift,
    double zshift)
  {
    double height = axis.AxisDesiredSize.Height / this.ViewBoxSize.Height * 2.1;
    double leftOffset = axis.LeftOffset / this.ViewBoxSize.Width * xshift;
    double rightOffset = axis.RightOffset / this.ViewBoxSize.Width * xshift;
    geometry.Geometry = (Geometry3D) MeshGenerator.PlaneXYZ(xshift, height, leftOffset, rightOffset);
    if (this.Rotate >= 0.0 && this.Rotate < 90.0)
    {
      axis.IsInversed = false;
      geometry.Transform = (Transform3D) new TranslateTransform3D(0.0, -(yshift / 2.0 + 0.05) - height / 2.0 - this.WallThickness.Bottom, zshift / 2.0 + 0.05);
    }
    else if (this.Rotate >= 90.0 && this.Rotate < 180.0)
    {
      axis.IsInversed = true;
      Transform3DGroup transform3Dgroup = new Transform3DGroup();
      RotateTransform3D rotateTransform3D = new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
      {
        Axis = new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0),
        Angle = 180.0
      });
      TranslateTransform3D translateTransform3D = new TranslateTransform3D(0.0, -(yshift / 2.0 + 0.05) - height / 2.0 - this.WallThickness.Bottom, -(zshift / 2.0) - 0.05);
      transform3Dgroup.Children.Add((Transform3D) rotateTransform3D);
      transform3Dgroup.Children.Add((Transform3D) translateTransform3D);
      geometry.Transform = (Transform3D) transform3Dgroup;
    }
    else if (this.Rotate >= 180.0 && this.Rotate < 270.0)
    {
      axis.IsInversed = true;
      Transform3DGroup transform3Dgroup = new Transform3DGroup();
      RotateTransform3D rotateTransform3D = new RotateTransform3D((Rotation3D) new AxisAngleRotation3D()
      {
        Angle = 180.0
      });
      TranslateTransform3D translateTransform3D = new TranslateTransform3D(0.0, -(yshift / 2.0 + 0.05) - height / 2.0 - this.WallThickness.Bottom, -(zshift / 2.0) - 0.05);
      transform3Dgroup.Children.Add((Transform3D) rotateTransform3D);
      transform3Dgroup.Children.Add((Transform3D) translateTransform3D);
      geometry.Transform = (Transform3D) transform3Dgroup;
    }
    else
    {
      if (this.Rotate < 270.0 || this.Rotate > 360.0)
        return;
      axis.IsInversed = false;
      geometry.Transform = (Transform3D) new TranslateTransform3D(0.0, -(yshift / 2.0 + 0.05) - height / 2.0 - this.WallThickness.Bottom, zshift / 2.0 + 0.05);
    }
  }

  private void PositioningAxis(SurfaceAxis axis, bool isOpp)
  {
    axis.OpposedPosition = !isOpp;
    axis.ComputeDesiredSize(new Size(this.RootPanelDesiredSize.Value.Width, this.ViewBoxSize.Height));
    axis.ArrangeAxisPanel(new Size(this.RootPanelDesiredSize.Value.Width, this.ViewBoxSize.Height));
    this.isOppositeAxis = !isOpp;
  }

  private void UpdateGridLines()
  {
    if (this.IsContour)
    {
      this.GridLineContent = (Model3DGroup) null;
    }
    else
    {
      Model3DGroup model3Dgroup = new Model3DGroup();
      foreach (SurfaceAxis ax in (Collection<SurfaceAxis>) this.Axes)
      {
        if (ax.ShowGridLines)
          model3Dgroup.Children.Add(this.DrawGridlines(ax));
      }
      this.GridLineContent = model3Dgroup;
    }
  }

  private Model3D DrawGridlines(SurfaceAxis axis)
  {
    GeometryModel3D geometryModel3D = new GeometryModel3D();
    MeshGeometry3D gridframe = new MeshGeometry3D();
    double offset = 1.05;
    for (int index = 0; index < axis.VisibleLabels.Count; ++index)
    {
      double num = axis.ValueTo3DCoefficient(axis.VisibleLabels[index].Position, false);
      this.DrawAxisGridlines(axis, num, offset, gridframe);
    }
    if (axis.smallTicksRequired)
    {
      for (int index = 0; index < axis.SmallTickPoints.Count; ++index)
      {
        double num = axis.ValueTo3DCoefficient(axis.SmallTickPoints[index], false);
        this.DrawAxisGridlines(axis, num, offset, gridframe);
      }
    }
    geometryModel3D.Geometry = (Geometry3D) gridframe;
    geometryModel3D.Material = (Material) new DiffuseMaterial(axis.GridLineStroke);
    geometryModel3D.BackMaterial = (Material) new DiffuseMaterial(axis.GridLineStroke);
    return (Model3D) geometryModel3D;
  }

  private void DrawAxisGridlines(
    SurfaceAxis axis,
    double value,
    double offset,
    MeshGeometry3D gridframe)
  {
    double num = 1.3;
    if (axis == this.InterernalXAxis)
    {
      double thickness = axis.GridLineThickness / this.RootPanelDesiredSize.Value.Height;
      System.Windows.Media.Media3D.Vector3D up1 = new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0);
      System.Windows.Media.Media3D.Vector3D up2 = new System.Windows.Media.Media3D.Vector3D(1.0, 0.0, 0.0);
      Point3D point3D1 = new Point3D()
      {
        X = value,
        Y = -offset,
        Z = offset
      };
      Point3D point3D2 = new Point3D()
      {
        X = value,
        Y = -offset,
        Z = -offset
      };
      Point3D point2 = new Point3D()
      {
        X = value,
        Y = offset,
        Z = -offset
      };
      Point3D point1 = new Point3D()
      {
        X = value,
        Y = offset,
        Z = offset
      };
      if (this.Rotate >= 90.0 && this.Rotate < 180.0)
      {
        MeshGenerator.AddSegment(gridframe, point1, point3D1, up2, thickness);
        MeshGenerator.AddSegment(gridframe, point3D1, point3D2, up1, thickness);
      }
      else if (this.Rotate >= 180.0 && this.Rotate < 270.0)
      {
        MeshGenerator.AddSegment(gridframe, point1, point3D1, up2, thickness);
        MeshGenerator.AddSegment(gridframe, point3D1, point3D2, up1, thickness);
      }
      else if (this.Rotate >= 270.0 && this.Rotate <= 360.0)
      {
        MeshGenerator.AddSegment(gridframe, point3D1, point3D2, up1, thickness);
        MeshGenerator.AddSegment(gridframe, point3D2, point2, up2, thickness);
      }
      else
      {
        if (this.Rotate < 0.0 || this.Rotate >= 90.0)
          return;
        MeshGenerator.AddSegment(gridframe, point3D1, point3D2, up1, thickness);
        MeshGenerator.AddSegment(gridframe, point3D2, point2, up2, thickness);
      }
    }
    else if (axis == this.InterernalYAxis)
    {
      double thickness = axis.GridLineThickness / this.RootPanelDesiredSize.Value.Height;
      System.Windows.Media.Media3D.Vector3D up3 = new System.Windows.Media.Media3D.Vector3D(1.0, 0.0, 0.0);
      System.Windows.Media.Media3D.Vector3D up4 = new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0);
      Point3D point3D3 = new Point3D()
      {
        X = -num,
        Y = value,
        Z = offset
      };
      Point3D point3D4 = new Point3D()
      {
        X = -num,
        Y = value,
        Z = -offset
      };
      Point3D point2 = new Point3D()
      {
        X = num,
        Y = value,
        Z = -offset
      };
      Point3D point3D5 = new Point3D()
      {
        X = num,
        Y = value,
        Z = offset
      };
      Point3D point3D6 = new Point3D()
      {
        X = num,
        Y = value,
        Z = -offset
      };
      if (this.Rotate >= 90.0 && this.Rotate < 180.0)
      {
        MeshGenerator.AddSegment(gridframe, point3D5, point3D3, up4, thickness);
        MeshGenerator.AddSegment(gridframe, point3D3, point3D4, up3, thickness);
      }
      else if (this.Rotate >= 180.0 && this.Rotate < 270.0)
      {
        MeshGenerator.AddSegment(gridframe, point3D6, point3D5, up3, thickness);
        MeshGenerator.AddSegment(gridframe, point3D5, point3D3, up4, thickness);
      }
      else if (this.Rotate >= 270.0 && this.Rotate <= 360.0)
      {
        MeshGenerator.AddSegment(gridframe, point3D4, point3D6, up4, thickness);
        MeshGenerator.AddSegment(gridframe, point3D6, point3D5, up3, thickness);
      }
      else
      {
        if (this.Rotate < 0.0 || this.Rotate >= 90.0)
          return;
        MeshGenerator.AddSegment(gridframe, point3D3, point3D4, up3, thickness);
        MeshGenerator.AddSegment(gridframe, point3D4, point2, up4, thickness);
      }
    }
    else
    {
      if (axis != this.InterernalZAxis)
        return;
      double thickness = axis.GridLineThickness / this.RootPanelDesiredSize.Value.Height;
      System.Windows.Media.Media3D.Vector3D up5 = new System.Windows.Media.Media3D.Vector3D(0.0, 0.0, 1.0);
      System.Windows.Media.Media3D.Vector3D up6 = new System.Windows.Media.Media3D.Vector3D(1.0, 0.0, 0.0);
      Point3D point3D7 = new Point3D()
      {
        X = num,
        Y = -offset,
        Z = value
      };
      Point3D point3D8 = new Point3D()
      {
        X = -num,
        Y = -offset,
        Z = value
      };
      Point3D point2 = new Point3D()
      {
        X = -num,
        Y = offset,
        Z = value
      };
      Point3D point1 = new Point3D()
      {
        X = num,
        Y = offset,
        Z = value
      };
      if (this.Rotate >= 90.0 && this.Rotate < 180.0)
      {
        MeshGenerator.AddSegment(gridframe, point3D7, point3D8, up5, thickness);
        MeshGenerator.AddSegment(gridframe, point3D8, point2, up6, thickness);
      }
      if (this.Rotate >= 180.0 && this.Rotate < 270.0)
      {
        MeshGenerator.AddSegment(gridframe, point1, point3D7, up6, thickness);
        MeshGenerator.AddSegment(gridframe, point3D7, point3D8, up5, thickness);
      }
      else if (this.Rotate >= 270.0 && this.Rotate < 360.0)
      {
        MeshGenerator.AddSegment(gridframe, point1, point3D7, up6, thickness);
        MeshGenerator.AddSegment(gridframe, point3D7, point3D8, up5, thickness);
      }
      else
      {
        if (this.Rotate < 0.0 || this.Rotate >= 90.0)
          return;
        MeshGenerator.AddSegment(gridframe, point3D7, point3D8, up5, thickness);
        MeshGenerator.AddSegment(gridframe, point3D8, point2, up6, thickness);
      }
    }
  }

  private void CreateSurface()
  {
    Model3DGroup model3Dgroup = new Model3DGroup();
    for (int index1 = 0; index1 < this.points.GetLength(0); ++index1)
    {
      for (int index2 = 0; index2 < this.points.GetLength(1); ++index2)
        this.points[index1, index2] = MeshGenerator.GetNormalize(this.dataPoints[index1, index2], this.InterernalXAxis.ActualRange.Start, this.InterernalXAxis.ActualRange.End, this.InterernalYAxis.ActualRange.Start, this.InterernalYAxis.ActualRange.End, this.InterernalZAxis.ActualRange.Start, this.InterernalZAxis.ActualRange.End);
    }
    this.yMin = MeshGenerator.GetMin("Y", this.points);
    this.yMax = MeshGenerator.GetMax("Y", this.points);
    if (!this.CanDrawMaterial)
    {
      List<Brush> brushes1 = this.ColorModel.GetBrushes(this.Palette);
      if (brushes1 != null && brushes1.Count > 0)
      {
        List<Brush> brushes2 = this.Palette == ChartColorPalette.Custom ? brushes1 : (this.BrushCount > 10 ? MeshGenerator.GetBrushRange(this.BrushCount - 10, brushes1) : brushes1.GetRange(0, this.BrushCount));
        if (!this.ApplyGradientBrush)
          this.materail = (Brush) MeshGenerator.DrawMaterial(brushes2, this.ApplyGradientBrush);
        else if (this.Palette == ChartColorPalette.Elite || this.Palette == ChartColorPalette.LightCandy || this.Palette == ChartColorPalette.SandyBeach)
        {
          this.materail = (Brush) MeshGenerator.DrawMaterial(brushes2, true);
        }
        else
        {
          double num = 1.0 / (double) (brushes2.Count - 1);
          LinearGradientBrush linearGradientBrush = new LinearGradientBrush()
          {
            StartPoint = new Point(0.0, 0.5),
            EndPoint = new Point(1.0, 0.5)
          };
          GradientStopCollection gradientStopCollection = new GradientStopCollection();
          double offset = 0.0;
          for (int index = 0; index < brushes2.Count; ++index)
          {
            gradientStopCollection.Add(new GradientStop((brushes2[index] as SolidColorBrush).Color, offset));
            offset += num;
          }
          linearGradientBrush.GradientStops = gradientStopCollection;
          this.materail = (Brush) linearGradientBrush;
        }
        this.CanDrawMaterial = true;
      }
      else
        this.materail = (Brush) new ImageBrush();
    }
    Point3D[] point3DArray = new Point3D[4];
    MeshGeometry3D meshGeometry3D = new MeshGeometry3D();
    this.pointDictionary = new Dictionary<Point3D, int>();
    for (int index3 = 0; index3 < this.points.GetLength(0) - 1; ++index3)
    {
      for (int index4 = 0; index4 < this.points.GetLength(1) - 1; ++index4)
      {
        point3DArray[0] = this.points[index3, index4];
        point3DArray[1] = this.points[index3, index4 + 1];
        point3DArray[2] = this.points[index3 + 1, index4 + 1];
        point3DArray[3] = this.points[index3 + 1, index4];
        this.AddTriangle(meshGeometry3D, point3DArray[0], point3DArray[1], point3DArray[2]);
        this.AddTriangle(meshGeometry3D, point3DArray[0], point3DArray[2], point3DArray[3]);
      }
    }
    DiffuseMaterial diffuseMaterial1 = new DiffuseMaterial(this.materail);
    Brush brush = (Brush) null;
    if (this.ShowContourLine)
      brush = (Brush) MeshGenerator.DrawContourLine(this.BrushCount);
    DiffuseMaterial diffuseMaterial2 = new DiffuseMaterial(brush);
    MaterialGroup materialGroup = new MaterialGroup();
    materialGroup.Children.Add((Material) diffuseMaterial1);
    materialGroup.Children.Add((Material) diffuseMaterial2);
    model3Dgroup.Children.Add((Model3D) new GeometryModel3D((Geometry3D) meshGeometry3D, (Material) materialGroup)
    {
      BackMaterial = (Material) materialGroup
    });
    if (this.drawWireframe)
    {
      double thickness = this.WireframeStrokeThickness / this.RootPanelDesiredSize.Value.Height;
      MeshGeometry3D geometry = MeshGenerator.MakeWireframe(meshGeometry3D, thickness);
      DiffuseMaterial diffuseMaterial3 = new DiffuseMaterial(this.WireframeStroke);
      model3Dgroup.Children.Add((Model3D) new GeometryModel3D((Geometry3D) geometry, (Material) diffuseMaterial3)
      {
        BackMaterial = (Material) diffuseMaterial3
      });
    }
    this.MeshContent = model3Dgroup;
  }

  private void AddTriangle(
    MeshGeometry3D mesh,
    Point3D point3D1,
    Point3D point3D2,
    Point3D point3D3)
  {
    this.AddPoints(mesh, point3D1);
    this.AddPoints(mesh, point3D2);
    this.AddPoints(mesh, point3D3);
  }

  private void AddPoints(MeshGeometry3D mesh, Point3D point)
  {
    if (this.pointDictionary.ContainsKey(point))
    {
      mesh.TriangleIndices.Add(this.pointDictionary[point]);
    }
    else
    {
      mesh.Positions.Add(point);
      int num = mesh.Positions.Count - 1;
      this.pointDictionary.Add(point, num);
      mesh.TriangleIndices.Add(num);
      double scaleValue = MeshGenerator.GetScaleValue(point.Y, this.yMin, this.yMax);
      mesh.TextureCoordinates.Add(new Point(scaleValue >= 0.995 ? 0.995 : scaleValue, 0.0));
    }
  }

  private void UpdateViewport()
  {
    ModelVisual3D visaul3D1 = new ModelVisual3D();
    this.SetViewportBinding(visaul3D1, new PropertyPath("WallContent", new object[0]));
    ModelVisual3D visaul3D2 = new ModelVisual3D();
    this.SetViewportBinding(visaul3D2, new PropertyPath("AxisContent", new object[0]));
    ModelVisual3D visaul3D3 = new ModelVisual3D();
    this.SetViewportBinding(visaul3D3, new PropertyPath("GridLineContent", new object[0]));
    ModelVisual3D visaul3D4 = new ModelVisual3D();
    this.SetViewportBinding(visaul3D4, new PropertyPath("MeshContent", new object[0]));
    ModelVisual3D visaul3D5 = new ModelVisual3D();
    this.SetViewportBinding(visaul3D5, new PropertyPath("LightContent", new object[0]));
    this.Viewport.Children.Clear();
    this.Viewport.Children.Add((Visual3D) visaul3D1);
    this.Viewport.Children.Add((Visual3D) visaul3D3);
    this.Viewport.Children.Add((Visual3D) visaul3D2);
    this.Viewport.Children.Add((Visual3D) visaul3D4);
    this.Viewport.Children.Add((Visual3D) visaul3D5);
  }

  private void SetViewportBinding(ModelVisual3D visaul3D, PropertyPath path)
  {
    BindingOperations.SetBinding((DependencyObject) visaul3D, ModelVisual3D.ContentProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = path
    });
  }

  internal void UpdateColorBarArrangeRect()
  {
    if (this.ColorBar == null)
      return;
    Size desiredSize = this.ColorBar.DesiredSize;
    Rect rect = new Rect(0.0, 0.0, this.RootPanelDesiredSize.Value.Width, this.RootPanelDesiredSize.Value.Height);
    switch (this.ColorBar.InternalDockPosition)
    {
      case ChartDock.Left:
        this.ColorBar.ArrangeRect = new Rect(rect.Left, rect.Top, desiredSize.Width, rect.Height);
        break;
      case ChartDock.Top:
        this.ColorBar.ArrangeRect = new Rect(rect.Left, rect.Top, rect.Width, desiredSize.Height);
        break;
      case ChartDock.Right:
        this.ColorBar.ArrangeRect = new Rect(rect.Width, rect.Top, desiredSize.Width, rect.Height);
        break;
      case ChartDock.Bottom:
        this.ColorBar.ArrangeRect = new Rect(rect.Left, rect.Bottom, rect.Width, desiredSize.Height);
        break;
    }
  }

  private void CreateCamera()
  {
    ProjectionCamera camera;
    if (this.CameraProjection == CameraProjection.Perspective)
      camera = (ProjectionCamera) new PerspectiveCamera()
      {
        FieldOfView = (this.ZoomLevel * 180.0)
      };
    else
      camera = (ProjectionCamera) new OrthographicCamera()
      {
        Width = (this.ZoomLevel * 15.0)
      };
    this.PositionCamera(camera);
    this.Viewport.Camera = (Camera) camera;
  }

  protected internal override void PositionCamera(ProjectionCamera camera)
  {
    double num1 = Math.PI / 180.0;
    double num2 = this.IsContour ? 90.0 : this.Tilt;
    double num3 = 0.0;
    double num4 = this.Rotate * num1;
    double num5 = num2 * num1;
    double num6 = num3 * num1;
    double num7 = Math.Cos(num4);
    double num8 = Math.Sin(num4);
    double z = Math.Cos(num4 + Math.PI / 2.0);
    double x = Math.Sin(num4 + Math.PI / 2.0);
    double num9 = Math.Cos(num5);
    double y = Math.Sin(num5);
    double num10 = Math.Cos(num6);
    double num11 = Math.Sin(num6);
    System.Windows.Media.Media3D.Vector3D vector2 = new System.Windows.Media.Media3D.Vector3D(x, 0.0, z);
    System.Windows.Media.Media3D.Vector3D vector1 = new System.Windows.Media.Media3D.Vector3D(num8 * num9, y, num7 * num9);
    System.Windows.Media.Media3D.Vector3D vector3D = System.Windows.Media.Media3D.Vector3D.CrossProduct(vector1, vector2);
    if (camera == null)
      return;
    camera.Position = (Point3D) (vector1 * 5.0);
    camera.LookDirection = -vector1;
    camera.UpDirection = num10 * vector3D - vector2 * num11;
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (e.OriginalSource is Viewport3D && this.EnableRotation)
    {
      this.currentMousePosition = e.GetPosition((IInputElement) this);
      Mouse.OverrideCursor = Keyboard.IsKeyDown(Key.LeftCtrl) ? Cursors.Hand : Cursors.SizeAll;
      this.CaptureMouse();
    }
    base.OnMouseLeftButtonDown(e);
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    if (e.MouseDevice.Captured == this && this.EnableRotation && e.LeftButton == MouseButtonState.Pressed)
    {
      double num1 = this.currentMousePosition.X - e.GetPosition((IInputElement) this).X;
      double num2 = this.currentMousePosition.Y - e.GetPosition((IInputElement) this).Y;
      this.Rotate += num1;
      if (!this.IsContour)
        this.Tilt -= num2;
      this.Rotate = this.Rotate >= 360.0 ? this.Rotate - 360.0 : (this.Rotate < 0.0 ? 360.0 + this.Rotate : this.Rotate);
      this.currentMousePosition = e.GetPosition((IInputElement) this);
      this.PositionCamera(this.Viewport.Camera as ProjectionCamera);
      this.DrawAxisElements();
      this.DrawWall();
      this.UpdateGridLines();
    }
    base.OnMouseMove(e);
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    this.ReleaseMouseCapture();
    this.currentMousePosition.X = double.NaN;
    this.currentMousePosition.Y = double.NaN;
    Mouse.OverrideCursor = Cursors.Arrow;
    base.OnMouseLeftButtonUp(e);
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    if (this.EnableZooming && e.OriginalSource is Viewport3D)
    {
      double zoomLevel = this.ZoomLevel;
      double num = ChartMath.MinMax(zoomLevel + 0.035 * (e.Delta > 0 ? -1.0 : 1.0), 0.0, 1.0);
      this.ZoomLevel = num == 0.0 ? zoomLevel : (num == 1.0 ? zoomLevel : num);
    }
    base.OnMouseWheel(e);
  }

  protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
  {
    this.previousScale = 1.0;
    base.OnManipulationStarted(e);
  }

  protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
  {
    if (this.EnableZooming)
    {
      double num1 = Math.Max(e.CumulativeManipulation.Scale.X, e.CumulativeManipulation.Scale.Y);
      double zoomLevel = this.ZoomLevel;
      bool flag = e.CumulativeManipulation.Scale.X != 0.0 || e.CumulativeManipulation.Scale.Y != 0.0;
      double d = zoomLevel * ((num1 - this.previousScale) / this.previousScale);
      if (flag && num1 != this.previousScale && !double.IsNaN(d) && !double.IsInfinity(d))
      {
        double num2 = zoomLevel - d;
        this.ZoomLevel = num2 <= 0.0 ? zoomLevel : (num2 >= 1.0 ? zoomLevel : num2);
      }
      this.previousScale = num1;
    }
    base.OnManipulationDelta(e);
  }

  protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
  {
    if (this.EnableZooming && e.OriginalSource is Viewport3D)
      this.ClearValue(SfSurfaceChart.ZoomLevelProperty);
    base.OnMouseDoubleClick(e);
  }

  private static ChartValueType GetDataType(
    IPropertyAccessor propertyAccessor,
    IEnumerable itemsSource)
  {
    if (itemsSource == null)
      return ChartValueType.Double;
    IEnumerator enumerator = itemsSource.GetEnumerator();
    object xval = (object) null;
    if (enumerator.MoveNext())
    {
      do
      {
        xval = propertyAccessor.GetValue(enumerator.Current);
      }
      while (enumerator.MoveNext() && xval == null);
    }
    return SfSurfaceChart.GetDataType(xval);
  }

  private static ChartValueType GetDataType(object xval)
  {
    switch (xval)
    {
      case string _:
      case string[] _:
        return ChartValueType.String;
      case DateTime _:
      case DateTime[] _:
        return ChartValueType.DateTime;
      case TimeSpan _:
      case TimeSpan[] _:
        return ChartValueType.TimeSpan;
      default:
        return ChartValueType.Double;
    }
  }

  public void Dispose()
  {
    if (this.DockPanel != null)
      this.DockPanel.Children.Clear();
    if (this.ColorBar == null)
      return;
    this.ColorBar.Area = (SfSurfaceChart) null;
    this.ColorBar.ItemsSource = (IEnumerable) null;
  }
}
