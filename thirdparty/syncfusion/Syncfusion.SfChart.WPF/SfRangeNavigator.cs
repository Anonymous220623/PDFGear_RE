// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SfRangeNavigator
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

[System.Windows.Markup.ContentProperty("Content")]
public class SfRangeNavigator : Control
{
  public static readonly DependencyProperty ZoomFactorProperty = DependencyProperty.Register(nameof (ZoomFactor), typeof (double), typeof (SfRangeNavigator), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(SfRangeNavigator.OnZoomFactorChanged)));
  public static readonly DependencyProperty ZoomPositionProperty = DependencyProperty.Register(nameof (ZoomPosition), typeof (double), typeof (SfRangeNavigator), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(SfRangeNavigator.OnZoomPositionChanged)));
  public static readonly DependencyProperty ViewRangeStartProperty = DependencyProperty.Register(nameof (ViewRangeStart), typeof (object), typeof (SfRangeNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfRangeNavigator.OnViewRangeStartChanged)));
  public static readonly DependencyProperty ViewRangeEndProperty = DependencyProperty.Register(nameof (ViewRangeEnd), typeof (object), typeof (SfRangeNavigator), new PropertyMetadata((object) null, new PropertyChangedCallback(SfRangeNavigator.OnViewRangeEndChanged)));
  public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof (Content), typeof (object), typeof (SfRangeNavigator), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty OverlayBrushProperty = DependencyProperty.Register(nameof (OverlayBrush), typeof (Brush), typeof (SfRangeNavigator), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ScrollbarVisibilityProperty = DependencyProperty.Register(nameof (ScrollbarVisibility), typeof (Visibility), typeof (SfRangeNavigator), new PropertyMetadata((object) Visibility.Visible));
  internal double zoomPosition;
  internal double zoomFactor = 1.0;
  private bool isViewRangeSet;

  public SfRangeNavigator()
  {
    LicenseHelper.ValidateLicense();
    this.DataStart = 0.0;
    this.DataEnd = 0.0;
    this.DefaultStyleKey = (object) typeof (SfRangeNavigator);
  }

  public event EventHandler ValueChanged;

  public double ZoomFactor
  {
    get => (double) this.GetValue(SfRangeNavigator.ZoomFactorProperty);
    set => this.SetValue(SfRangeNavigator.ZoomFactorProperty, (object) value);
  }

  public double ZoomPosition
  {
    get => (double) this.GetValue(SfRangeNavigator.ZoomPositionProperty);
    set => this.SetValue(SfRangeNavigator.ZoomPositionProperty, (object) value);
  }

  public object ViewRangeStart
  {
    get => this.GetValue(SfRangeNavigator.ViewRangeStartProperty);
    set => this.SetValue(SfRangeNavigator.ViewRangeStartProperty, value);
  }

  public object ViewRangeEnd
  {
    get => this.GetValue(SfRangeNavigator.ViewRangeEndProperty);
    set => this.SetValue(SfRangeNavigator.ViewRangeEndProperty, value);
  }

  public object Content
  {
    get => this.GetValue(SfRangeNavigator.ContentProperty);
    set => this.SetValue(SfRangeNavigator.ContentProperty, value);
  }

  public Brush OverlayBrush
  {
    get => (Brush) this.GetValue(SfRangeNavigator.OverlayBrushProperty);
    set => this.SetValue(SfRangeNavigator.OverlayBrushProperty, (object) value);
  }

  public Visibility ScrollbarVisibility
  {
    get => (Visibility) this.GetValue(SfRangeNavigator.ScrollbarVisibilityProperty);
    set => this.SetValue(SfRangeNavigator.ScrollbarVisibilityProperty, (object) value);
  }

  internal double XRange { get; set; }

  internal ResizableScrollBar Navigator { get; set; }

  internal ResizableScrollBar Scrollbar { get; set; }

  internal double DataStart { get; set; }

  internal double DataEnd { get; set; }

  internal ObservableCollection<object> Selected { get; set; }

  internal IEnumerable XValues { get; set; }

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
    File.WriteAllText(Directory.GetParent("../").FullName + "\\navigator.xml", result.ToString());
  }

  public object Deserialize(Stream stream) => XamlReader.Load(new StreamReader(stream).BaseStream);

  public object Deserialize(string fileName)
  {
    return XamlReader.Load(new StreamReader(fileName).BaseStream);
  }

  public object Deserialize()
  {
    return XamlReader.Load(new StreamReader(Directory.GetParent("../").FullName + "\\navigator.xml").BaseStream);
  }

  internal virtual void OnZoomFactorChanged(double newValue, double oldValue)
  {
    this.zoomFactor = newValue;
    if (this.Navigator == null)
      return;
    this.Navigator.IsValueChangedTrigger = false;
    this.Navigator.RangeEnd = this.ZoomPosition + newValue;
  }

  internal virtual void OnZoomPositionChanged(double newValue)
  {
    if (this.Navigator != null && this != null)
    {
      this.Navigator.IsValueChangedTrigger = false;
      this.Navigator.RangeEnd = this.ZoomFactor + newValue;
      this.Navigator.IsValueChangedTrigger = false;
      this.Navigator.RangeStart = newValue;
    }
    this.zoomPosition = newValue;
  }

  internal virtual void OnViewRangeEndChanged()
  {
    if (this.Navigator == null || this.Navigator.TrackSize == 0.0 || this.ViewRangeEnd is DateTime)
      return;
    this.Navigator.IsValueChangedTrigger = false;
    this.Navigator.RangeEnd = Convert.ToDouble(this.ViewRangeEnd);
  }

  internal virtual void OnViewRangeStartChanged()
  {
    if (this.Navigator == null || this.Navigator.TrackSize == 0.0 || this.ViewRangeStart is DateTime)
      return;
    this.Navigator.IsValueChangedTrigger = false;
    this.Navigator.RangeStart = Convert.ToDouble(this.ViewRangeStart);
  }

  internal virtual void CalculateSelectedData()
  {
    if (this.isViewRangeSet || this.ViewRangeStart == null && this.ViewRangeEnd == null)
    {
      this.ZoomFactor = this.Navigator.RangeEnd - this.Navigator.RangeStart;
      this.ZoomPosition = this.Navigator.RangeStart;
      this.ViewRangeEnd = (object) this.Navigator.RangeEnd;
      this.ViewRangeStart = (object) this.Navigator.RangeStart;
    }
    else if (Convert.ToDouble(this.ViewRangeStart) >= this.Navigator.Minimum && Convert.ToDouble(this.ViewRangeEnd) <= this.Navigator.Maximum)
    {
      this.Navigator.RangeStart = Convert.ToDouble(this.ViewRangeStart);
      this.Navigator.RangeEnd = Convert.ToDouble(this.ViewRangeEnd);
      this.ZoomFactor = this.Navigator.RangeEnd - this.Navigator.RangeStart;
      this.ZoomPosition = this.Navigator.RangeStart;
    }
    else
    {
      this.isViewRangeSet = true;
      this.CalculateSelectedData();
    }
    this.isViewRangeSet = true;
  }

  public override void OnApplyTemplate()
  {
    ResizableScrollBar resizableScrollBar1 = (ResizableScrollBar) null;
    if (this.Navigator != null)
    {
      this.Navigator.SizeChanged -= new SizeChangedEventHandler(this.OnTimeLineSizeChanged);
      resizableScrollBar1 = this.Navigator;
    }
    this.Navigator = this.GetTemplateChild("Part_RangePicker") as ResizableScrollBar;
    if (resizableScrollBar1 != null)
    {
      this.Navigator.RangeStart = resizableScrollBar1.RangeStart;
      this.Navigator.RangeEnd = resizableScrollBar1.RangeEnd;
    }
    else
    {
      this.Navigator.RangeStart = this.zoomPosition;
      this.Navigator.RangeEnd = this.zoomPosition + this.zoomFactor;
    }
    ResizableScrollBar resizableScrollBar2 = (ResizableScrollBar) null;
    if (this.Scrollbar != null)
    {
      this.Scrollbar.ValueChanged -= new EventHandler(this.OnScrollbarValueChanged);
      resizableScrollBar2 = this.Scrollbar;
    }
    this.Scrollbar = this.GetTemplateChild("Part_Scroll") as ResizableScrollBar;
    if (resizableScrollBar2 != null)
    {
      this.Scrollbar.RangeStart = resizableScrollBar2.RangeStart;
      this.Scrollbar.RangeEnd = resizableScrollBar2.RangeEnd;
      this.ClipNavigator();
    }
    if (this.Scrollbar != null)
      this.Scrollbar.ValueChanged += new EventHandler(this.OnScrollbarValueChanged);
    if (this.Navigator != null)
    {
      this.Navigator.ScrollButtonVisibility = Visibility.Collapsed;
      this.Navigator.SizeChanged += new SizeChangedEventHandler(this.OnTimeLineSizeChanged);
    }
    this.Loaded -= new RoutedEventHandler(this.OnSfRangeNavigatorLoaded);
    this.Loaded += new RoutedEventHandler(this.OnSfRangeNavigatorLoaded);
    this.SizeChanged -= new SizeChangedEventHandler(this.SfRangeNavigator_SizeChanged);
    this.SizeChanged += new SizeChangedEventHandler(this.SfRangeNavigator_SizeChanged);
    base.OnApplyTemplate();
  }

  protected virtual void OnValueChanged()
  {
    if (this.ValueChanged == null)
      return;
    this.ValueChanged((object) this, EventArgs.Empty);
  }

  protected virtual void OnScrollbarValueChanged(object sender, EventArgs e)
  {
    this.ClipNavigator();
  }

  protected virtual void OnTimeLineValueChanged(object sender, EventArgs e)
  {
    this.CalculateSelectedData();
    this.OnValueChanged();
  }

  protected virtual void OnTimeLineSizeChanged(object sender, SizeChangedEventArgs e)
  {
  }

  private static void OnZoomFactorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as SfRangeNavigator).OnZoomFactorChanged(Convert.ToDouble(e.NewValue), Convert.ToDouble(e.OldValue));
  }

  private static void OnZoomPositionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as SfRangeNavigator).OnZoomPositionChanged(Convert.ToDouble(e.NewValue));
  }

  private static void OnViewRangeStartChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfRangeNavigator sfRangeNavigator) || e.NewValue == null)
      return;
    sfRangeNavigator.OnViewRangeStartChanged();
  }

  private static void OnViewRangeEndChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SfRangeNavigator sfRangeNavigator) || e.NewValue == null)
      return;
    sfRangeNavigator.OnViewRangeEndChanged();
  }

  private void SfRangeNavigator_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.ClipNavigator();
  }

  private void OnSfRangeNavigatorLoaded(object sender, RoutedEventArgs e)
  {
    if (this.Navigator == null)
      return;
    this.Navigator.ValueChanged -= new EventHandler(this.OnTimeLineValueChanged);
    this.Navigator.ValueChanged += new EventHandler(this.OnTimeLineValueChanged);
    this.CalculateSelectedData();
  }

  private void ClipNavigator()
  {
    if (this.Navigator == null || this.Navigator.Content == null || this.Scrollbar == null)
      return;
    this.XRange = -(this.ActualWidth * this.Scrollbar.Scale * this.Scrollbar.RangeStart);
    this.Navigator.Width = this.ActualWidth * this.Scrollbar.Scale;
    this.Navigator.Margin = new Thickness(this.XRange, 0.0, 0.0, 0.0);
    this.Clip = (Geometry) new RectangleGeometry()
    {
      Rect = new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight)
    };
  }
}
