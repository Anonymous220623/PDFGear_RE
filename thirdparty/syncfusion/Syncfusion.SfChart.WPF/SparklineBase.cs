// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SparklineBase
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using Syncfusion.Licensing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class SparklineBase : Control
{
  public static readonly DependencyProperty EnableAnimationProperty = DependencyProperty.Register(nameof (EnableAnimation), typeof (bool), typeof (SparklineBase), new PropertyMetadata((object) false, new PropertyChangedCallback(SparklineBase.OnEnableAnimationChanged)));
  public static readonly DependencyProperty InteriorProperty = DependencyProperty.Register(nameof (Interior), typeof (Brush), typeof (SparklineBase), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 27, (byte) 161, (byte) 226))));
  public static readonly DependencyProperty RangeBandBrushProperty = DependencyProperty.Register(nameof (RangeBandBrush), typeof (Brush), typeof (SparklineBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty BandRangeEndProperty = DependencyProperty.Register(nameof (BandRangeEnd), typeof (double), typeof (SparklineBase), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(SparklineBase.OnHighlightValueChanged)));
  public static readonly DependencyProperty BandRangeStartProperty = DependencyProperty.Register(nameof (BandRangeStart), typeof (double), typeof (SparklineBase), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(SparklineBase.OnHighlightValueChanged)));
  public static readonly DependencyProperty MinimumYValueProperty = DependencyProperty.Register(nameof (MinimumYValue), typeof (double), typeof (SparklineBase), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(SparklineBase.OnYRangeChanged)));
  public static readonly DependencyProperty MaximumYValueProperty = DependencyProperty.Register(nameof (MaximumYValue), typeof (double), typeof (SparklineBase), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(SparklineBase.OnYRangeChanged)));
  public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof (StrokeThickness), typeof (double), typeof (SparklineBase), new PropertyMetadata((object) 2.0));
  public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof (Stroke), typeof (Brush), typeof (SparklineBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof (ItemsSource), typeof (IEnumerable), typeof (SparklineBase), new PropertyMetadata((object) null, new PropertyChangedCallback(SparklineBase.OnItemsSourceChanged)));
  public static readonly DependencyProperty YBindingPathProperty = DependencyProperty.Register(nameof (YBindingPath), typeof (string), typeof (SparklineBase), new PropertyMetadata((object) "", new PropertyChangedCallback(SparklineBase.OnYBingingPathChanged)));
  public static readonly DependencyProperty EmptyPointValueProperty = DependencyProperty.Register(nameof (EmptyPointValue), typeof (EmptyPointValues), typeof (SparklineBase), new PropertyMetadata((object) EmptyPointValues.None, new PropertyChangedCallback(SparklineBase.OnEmptyPointValueChanged)));
  internal List<double> yValues;
  internal List<double> xValues;
  internal double minYValue;
  internal double maxYValue;
  internal double deltaY;
  internal double minXValue;
  internal double maxXValue;
  internal double deltaX;
  internal double availableWidth;
  internal double availableHeight;
  private Rectangle rangeBand;
  private SparklinePointsInfo info;
  private bool isTemplateApplied;
  private bool isMeasureed;
  private bool needToAnimate = true;
  private SparklineMouseMoveEventArgs args = new SparklineMouseMoveEventArgs();
  private SparklinePointsInfo pointsInfo = new SparklinePointsInfo();

  public SparklineBase()
  {
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
    this.DefaultStyleKey = (object) typeof (SparklineBase);
  }

  public event SparklineMouseMoveHandler OnSparklineMouseMove;

  public bool EnableAnimation
  {
    get => (bool) this.GetValue(SparklineBase.EnableAnimationProperty);
    set => this.SetValue(SparklineBase.EnableAnimationProperty, (object) value);
  }

  public Brush Interior
  {
    get => (Brush) this.GetValue(SparklineBase.InteriorProperty);
    set => this.SetValue(SparklineBase.InteriorProperty, (object) value);
  }

  public Brush RangeBandBrush
  {
    get => (Brush) this.GetValue(SparklineBase.RangeBandBrushProperty);
    set => this.SetValue(SparklineBase.RangeBandBrushProperty, (object) value);
  }

  public double BandRangeEnd
  {
    get => (double) this.GetValue(SparklineBase.BandRangeEndProperty);
    set => this.SetValue(SparklineBase.BandRangeEndProperty, (object) value);
  }

  public double BandRangeStart
  {
    get => (double) this.GetValue(SparklineBase.BandRangeStartProperty);
    set => this.SetValue(SparklineBase.BandRangeStartProperty, (object) value);
  }

  public double MinimumYValue
  {
    get => (double) this.GetValue(SparklineBase.MinimumYValueProperty);
    set => this.SetValue(SparklineBase.MinimumYValueProperty, (object) value);
  }

  public double MaximumYValue
  {
    get => (double) this.GetValue(SparklineBase.MaximumYValueProperty);
    set => this.SetValue(SparklineBase.MaximumYValueProperty, (object) value);
  }

  public double StrokeThickness
  {
    get => (double) this.GetValue(SparklineBase.StrokeThicknessProperty);
    set => this.SetValue(SparklineBase.StrokeThicknessProperty, (object) value);
  }

  public Brush Stroke
  {
    get => (Brush) this.GetValue(SparklineBase.StrokeProperty);
    set => this.SetValue(SparklineBase.StrokeProperty, (object) value);
  }

  public IEnumerable ItemsSource
  {
    get => (IEnumerable) this.GetValue(SparklineBase.ItemsSourceProperty);
    set => this.SetValue(SparklineBase.ItemsSourceProperty, (object) value);
  }

  public string YBindingPath
  {
    get => (string) this.GetValue(SparklineBase.YBindingPathProperty);
    set => this.SetValue(SparklineBase.YBindingPathProperty, (object) value);
  }

  public EmptyPointValues EmptyPointValue
  {
    get => (EmptyPointValues) this.GetValue(SparklineBase.EmptyPointValueProperty);
    set => this.SetValue(SparklineBase.EmptyPointValueProperty, (object) value);
  }

  internal Grid RootPanel { get; set; }

  internal Canvas UtilityPresenter { get; set; }

  internal bool IsIndexed { get; set; }

  protected Canvas SegmentPresenter { get; set; }

  protected List<double> EmptyPointIndexes { get; set; }

  protected int DataCount { get; set; }

  public virtual Point TransformToVisible(double x, double y)
  {
    return new Point(Math.Round((this.availableWidth - this.Padding.Left * 2.0) * ((x - this.minXValue) / this.deltaX)), Math.Round((this.availableHeight - this.Padding.Top * 2.0) * (1.0 - (y - this.minYValue) / this.deltaY)));
  }

  public virtual void Reset()
  {
    this.SegmentPresenter.Children.Clear();
    this.yValues.Clear();
    this.xValues.Clear();
    this.DataCount = 0;
  }

  public void Serialize(string fileName)
  {
    StringBuilder result = new StringBuilder();
    ChartBase.GetSerializedString(out result, (object) this);
    File.WriteAllText(fileName, result.ToString());
  }

  public void Serialize(Stream stream)
  {
    StringBuilder result = new StringBuilder();
    ChartBase.GetSerializedString(out result, (object) this);
    using (StreamWriter streamWriter = new StreamWriter(stream))
      streamWriter.WriteLine(result.ToString());
  }

  public void Serialize()
  {
    StringBuilder result = new StringBuilder();
    ChartBase.GetSerializedString(out result, (object) this);
    File.WriteAllText(Directory.GetParent("../").FullName + "\\sparkline.xml", result.ToString());
  }

  public object Deserialize(Stream stream) => XamlReader.Load(new StreamReader(stream).BaseStream);

  public object Deserialize(string fileName)
  {
    return XamlReader.Load(new StreamReader(fileName).BaseStream);
  }

  public object Deserialize()
  {
    return XamlReader.Load(new StreamReader(Directory.GetParent("../").FullName + "\\sparkline.xml").BaseStream);
  }

  public void UpdateArea()
  {
    if (this.yValues == null || this.yValues.Count <= 0 || !this.isTemplateApplied || !this.isMeasureed)
      return;
    this.RenderSegments();
    if (!double.IsNaN(this.BandRangeStart) && !double.IsNaN(this.BandRangeEnd))
      this.UpdateRangeBand();
    if (this.EnableAnimation && this.needToAnimate)
      this.AnimateSegments(this.SegmentPresenter.Children);
    this.needToAnimate = false;
    this.Clip = (Geometry) new RectangleGeometry()
    {
      Rect = new Rect(0.0, 0.0, this.availableWidth, this.availableHeight)
    };
  }

  internal void ValidateEmptyPoints(List<double> yValues)
  {
    switch (this.EmptyPointValue)
    {
      case EmptyPointValues.Zero:
        using (List<double>.Enumerator enumerator = this.EmptyPointIndexes.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            int current = (int) enumerator.Current;
            if (current != -1)
              yValues[current] = 0.0;
          }
          break;
        }
      case EmptyPointValues.Average:
        using (List<double>.Enumerator enumerator = this.EmptyPointIndexes.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            int current = (int) enumerator.Current;
            switch (current)
            {
              case -1:
                continue;
              case 0:
                yValues[current] = double.IsNaN(yValues[current + 1]) ? 0.0 : yValues[current + 1] / 2.0;
                continue;
              default:
                yValues[current] = current != this.DataCount - 1 ? (double.IsNaN(yValues[current - 1]) ? 0.0 : yValues[current - 1]) + (double.IsNaN(yValues[current + 1]) ? 0.0 : yValues[current + 1] / 2.0) : (double.IsNaN(yValues[current - 1]) ? 0.0 : yValues[current - 1] / 2.0);
                continue;
            }
          }
          break;
        }
      case EmptyPointValues.None:
        using (List<double>.Enumerator enumerator = this.EmptyPointIndexes.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            int current = (int) enumerator.Current;
            if (current != -1)
              yValues[current] = double.NaN;
          }
          break;
        }
    }
  }

  internal void FindEmptyPoints()
  {
    this.EmptyPointIndexes.Clear();
    this.EmptyPointIndexes.Add(-1.0);
    for (int index = 0; index < this.yValues.Count; ++index)
    {
      if (double.IsNaN(this.yValues[index]))
        this.EmptyPointIndexes.Add((double) index);
    }
  }

  internal virtual void ClearUnUsedSegments(int dataCount)
  {
    int count = this.SegmentPresenter.Children.Count;
    if (count <= dataCount)
      return;
    for (int index = dataCount; index < count; ++index)
      this.SegmentPresenter.Children.RemoveAt(dataCount);
  }

  internal virtual void SetBinding(Shape element)
  {
    element.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeThickness", new object[0])
    });
  }

  internal SparklinePointsInfo FindPoints(double xVal, double yVal)
  {
    double num1 = xVal / this.availableWidth;
    double num2 = yVal / this.availableHeight;
    double num3 = double.NaN;
    double y = double.NaN;
    if (!this.IsIndexed)
    {
      Point point1 = new Point(Math.Round(this.minXValue + (this.maxXValue - this.minXValue) * num1), Math.Round(this.minYValue + (this.maxYValue - this.minYValue) * num2));
      Point point2 = new Point(this.minXValue, this.minYValue);
      for (int index = 0; index < this.DataCount; ++index)
      {
        double xValue = this.xValues[index];
        double yValue = this.yValues[index];
        if (Math.Abs(point1.X - xValue) <= Math.Abs(point1.X - point2.X))
        {
          point2 = new Point(xValue, yValue);
          num3 = this.xValues[index];
          y = this.yValues[index];
        }
      }
    }
    else
    {
      num3 = Math.Round(this.minXValue + this.maxXValue * num1);
      if (num3 <= this.maxXValue && num3 >= this.minXValue && this.xValues.Count > 0)
        y = this.yValues[(int) this.xValues[(int) num3]];
    }
    this.pointsInfo.Coordinate = this.TransformToVisible(num3, y);
    this.pointsInfo.Value = new Point(num3, y);
    return this.pointsInfo;
  }

  internal void StyleBinding(FrameworkElement element, Style inputStyle, string propertyPath)
  {
    element.SetBinding(FrameworkElement.StyleProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath(propertyPath, new object[0])
    });
  }

  protected virtual void AnimateSegments(UIElementCollection elements)
  {
  }

  protected virtual void GeneratePoints(string xPath)
  {
    if (this.ItemsSource == null)
      return;
    this.yValues = new List<double>();
    this.xValues = new List<double>();
    this.EmptyPointIndexes = new List<double>();
    double num1 = 0.0;
    IEnumerator enumerator = this.ItemsSource.GetEnumerator();
    if (enumerator.MoveNext())
    {
      if (string.IsNullOrEmpty(this.YBindingPath))
      {
        do
        {
          if (enumerator.Current is double)
          {
            this.yValues.Add(Convert.ToDouble(enumerator.Current));
            this.xValues.Add(num1);
          }
          ++num1;
        }
        while (enumerator.MoveNext());
      }
      else
      {
        PropertyInfo propertyInfo = ChartDataUtils.GetPropertyInfo(enumerator.Current, this.YBindingPath);
        if (propertyInfo != (PropertyInfo) null)
        {
          Func<object, object> getMethod1 = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo).GetMethod;
          if (string.IsNullOrEmpty(xPath))
          {
            this.IsIndexed = true;
            do
            {
              double num2 = Convert.ToDouble(getMethod1(enumerator.Current));
              this.xValues.Add(num1);
              this.yValues.Add(num2);
              ++num1;
            }
            while (enumerator.MoveNext());
          }
          else
          {
            this.IsIndexed = false;
            Func<object, object> getMethod2 = FastReflectionCaches.PropertyAccessorCache.Get(ChartDataUtils.GetPropertyInfo(enumerator.Current, xPath)).GetMethod;
            if (getMethod2(enumerator.Current) is DateTime)
            {
              do
              {
                double oaDate = ((DateTime) getMethod2(enumerator.Current)).ToOADate();
                this.yValues.Add(Convert.ToDouble(getMethod1(enumerator.Current)));
                this.xValues.Add(oaDate);
              }
              while (enumerator.MoveNext());
            }
            else
            {
              do
              {
                double num3 = Convert.ToDouble(getMethod2(enumerator.Current));
                this.yValues.Add(Convert.ToDouble(getMethod1(enumerator.Current)));
                this.xValues.Add(num3);
              }
              while (enumerator.MoveNext());
            }
          }
        }
      }
    }
    this.DataCount = this.xValues.Count;
    this.FindEmptyPoints();
    this.ValidateEmptyPoints(this.yValues);
    this.UpdateMinMaxValues();
  }

  protected virtual void UpdateMinMaxValues()
  {
    if (this.xValues == null || this.xValues.Count <= 0)
      return;
    this.minYValue = this.yValues.Where<double>((Func<double, bool>) (p => !double.IsNaN(p))).Min();
    this.minYValue = double.IsNaN(this.MinimumYValue) ? this.minYValue : this.MinimumYValue;
    this.minXValue = this.xValues.Min();
    this.maxXValue = this.xValues.Max();
    this.maxYValue = this.yValues.Max();
    this.maxYValue = double.IsNaN(this.MaximumYValue) ? this.maxYValue : this.MaximumYValue;
    this.deltaY = this.maxYValue - this.minYValue;
    this.deltaX = this.maxXValue - this.minXValue;
    this.deltaY = this.deltaY == 0.0 ? 1.0 : this.deltaY;
    this.deltaX = this.deltaX == 0.0 ? 1.0 : this.deltaX;
  }

  protected virtual void SetIndividualPoints(int index, object obj, bool replace, string xPath)
  {
    if (obj == null)
      return;
    if (string.IsNullOrEmpty(this.YBindingPath))
    {
      if (replace && this.yValues.Count > index)
      {
        this.yValues[index] = Convert.ToDouble(obj);
      }
      else
      {
        this.yValues.Add(Convert.ToDouble(obj));
        this.xValues.Add((double) this.DataCount);
      }
    }
    else
    {
      PropertyInfo propertyInfo1 = ChartDataUtils.GetPropertyInfo(obj, this.YBindingPath);
      if (propertyInfo1 != (PropertyInfo) null)
      {
        IPropertyAccessor propertyAccessor1 = (IPropertyAccessor) null;
        if (propertyInfo1 != (PropertyInfo) null)
          propertyAccessor1 = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo1);
        Func<object, object> getMethod1 = propertyAccessor1.GetMethod;
        if (string.IsNullOrEmpty(xPath))
        {
          double num = Convert.ToDouble(getMethod1(obj));
          if (replace && this.yValues.Count > index)
          {
            this.yValues[index] = num;
          }
          else
          {
            this.yValues.Insert(index, num);
            this.xValues.Add((double) this.DataCount);
          }
        }
        else
        {
          PropertyInfo propertyInfo2 = ChartDataUtils.GetPropertyInfo(obj, xPath);
          IPropertyAccessor propertyAccessor2 = (IPropertyAccessor) null;
          if (propertyInfo2 != (PropertyInfo) null)
            propertyAccessor2 = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo2);
          Func<object, object> getMethod2 = propertyAccessor2.GetMethod;
          double num1 = Convert.ToDouble(getMethod1(obj));
          object obj1 = getMethod2(obj);
          double num2 = !(obj1 is DateTime dateTime) ? Convert.ToDouble(obj1) : dateTime.ToOADate();
          if (replace && this.yValues.Count > index)
          {
            this.yValues[index] = num1;
          }
          else
          {
            this.xValues.Insert(index, num2);
            this.yValues.Insert(index, num1);
          }
        }
      }
    }
    ++this.DataCount;
  }

  protected abstract void RenderSegments();

  public override void OnApplyTemplate()
  {
    this.RootPanel = this.GetTemplateChild("PART_RootPanel") as Grid;
    this.SegmentPresenter = new Canvas();
    this.RootPanel.Children.Add((UIElement) this.SegmentPresenter);
    this.isTemplateApplied = true;
    this.GeneratePoints(string.Empty);
    this.UpdateArea();
    if (!this.EnableAnimation || this.SegmentPresenter == null)
      return;
    this.AnimateSegments(this.SegmentPresenter.Children);
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    if (this.availableHeight != availableSize.Height || this.availableWidth != availableSize.Width)
    {
      this.isMeasureed = true;
      this.availableWidth = double.IsInfinity(availableSize.Width) ? 100.0 : availableSize.Width;
      this.availableHeight = double.IsInfinity(availableSize.Height) ? 50.0 : availableSize.Height;
      this.UpdateArea();
    }
    return base.MeasureOverride(new Size(this.availableWidth, this.availableHeight));
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    if (this.OnSparklineMouseMove == null)
      return;
    Point position = e.GetPosition((IInputElement) this);
    this.info = this.FindPoints(position.X, position.Y);
    this.args.Value = !(e.OriginalSource is Rectangle) ? this.info.Value : (!((e.OriginalSource as Shape).Tag is object[] tag) || !(tag[1] is double) || !(tag[2] is double) ? this.info.Value : new Point(Convert.ToDouble(tag[1]), Convert.ToDouble(tag[2])));
    this.args.Coordinate = this.info.Coordinate;
    this.args.OriginalSource = e.OriginalSource;
    this.args.RootPanel = (Panel) this.RootPanel;
    this.OnSparklineMouseMove((object) this, this.args);
  }

  private static void OnYRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    SparklineBase sparklineBase = d as SparklineBase;
    sparklineBase.UpdateMinMaxValues();
    sparklineBase.UpdateArea();
  }

  private static void OnHighlightValueChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as SparklineBase).UpdateRangeBand();
  }

  private static void OnEnableAnimationChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as SparklineBase).OnEnableAnimationChanged();
  }

  private static void OnEmptyPointValueChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as SparklineBase).UpdateArea();
  }

  private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as SparklineBase).OnItemsSourceChanged(e);
  }

  private static void OnYBingingPathChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
  }

  private static void OnXBindingPathChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
  }

  private void OnEnableAnimationChanged()
  {
    if (!this.EnableAnimation || this.SegmentPresenter == null)
      return;
    this.AnimateSegments(this.SegmentPresenter.Children);
  }

  private void OnItemsSourceChanged(DependencyPropertyChangedEventArgs e)
  {
    if (e.OldValue is INotifyCollectionChanged)
      (e.OldValue as INotifyCollectionChanged).CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnDataCollectionChanged);
    if (e.NewValue is INotifyCollectionChanged)
      (e.NewValue as INotifyCollectionChanged).CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnDataCollectionChanged);
    if (this.isTemplateApplied)
      this.GeneratePoints(string.Empty);
    this.UpdateArea();
  }

  private void OnDataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.Action == NotifyCollectionChangedAction.Add)
      this.SetIndividualPoints(e.NewStartingIndex, e.NewItems[0], false, string.Empty);
    if (e.Action == NotifyCollectionChangedAction.Remove)
    {
      this.yValues.RemoveAt(e.OldStartingIndex);
      this.xValues.RemoveAt(this.DataCount - 1);
      --this.DataCount;
      if (this.DataCount == 0)
        this.Reset();
    }
    if (e.Action == NotifyCollectionChangedAction.Replace)
      this.SetIndividualPoints(e.NewStartingIndex, e.NewItems[0], true, string.Empty);
    if (e.Action == NotifyCollectionChangedAction.Reset)
      this.Reset();
    this.FindEmptyPoints();
    this.ValidateEmptyPoints(this.yValues);
    this.UpdateMinMaxValues();
    this.UpdateArea();
  }

  private void UpdateRangeBand()
  {
    if (double.IsNaN(this.BandRangeStart) || double.IsNaN(this.BandRangeEnd) || this.RootPanel == null)
      return;
    if (this.rangeBand == null)
    {
      if (this.UtilityPresenter == null)
      {
        this.UtilityPresenter = new Canvas();
        this.RootPanel.Children.Add((UIElement) this.UtilityPresenter);
      }
      this.rangeBand = new Rectangle();
      this.rangeBand.Opacity = 0.3;
      this.UtilityPresenter.Children.Add((UIElement) this.rangeBand);
      this.BindRangeBandBrush((Shape) this.rangeBand);
    }
    Rect rect = new Rect(this.TransformToVisible(this.minXValue, this.BandRangeStart), this.TransformToVisible(this.maxXValue, this.BandRangeEnd));
    this.rangeBand.Width = rect.Width;
    this.rangeBand.SetValue(Canvas.LeftProperty, (object) rect.X);
    this.rangeBand.Height = rect.Height;
    this.rangeBand.SetValue(Canvas.TopProperty, (object) rect.Y);
  }

  private void BindRangeBandBrush(Shape rangeBand)
  {
    rangeBand.SetBinding(Shape.FillProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("RangeBandBrush", new object[0])
    });
  }
}
