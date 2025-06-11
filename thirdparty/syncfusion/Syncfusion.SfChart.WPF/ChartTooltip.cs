// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartTooltip
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartTooltip : ContentControl
{
  public static readonly DependencyProperty ShowDurationProperty = DependencyProperty.RegisterAttached("ShowDuration", typeof (int), typeof (ChartTooltip), new PropertyMetadata((object) 1000));
  public static readonly DependencyProperty InitialShowDelayProperty = DependencyProperty.RegisterAttached("InitialShowDelay", typeof (int), typeof (ChartTooltip), new PropertyMetadata((object) 0));
  public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached("HorizontalOffset", typeof (double), typeof (ChartTooltip), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached("VerticalOffset", typeof (double), typeof (ChartTooltip), new PropertyMetadata((object) 0.0));
  public new static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.RegisterAttached("HorizontalAlignment", typeof (HorizontalAlignment), typeof (ChartTooltip), new PropertyMetadata((object) HorizontalAlignment.Center));
  public new static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.RegisterAttached("VerticalAlignment", typeof (VerticalAlignment), typeof (ChartTooltip), new PropertyMetadata((object) VerticalAlignment.Top));
  public static readonly DependencyProperty EnableAnimationProperty = DependencyProperty.RegisterAttached("EnableAnimation", typeof (bool), typeof (ChartTooltip), new PropertyMetadata((object) true));
  public static readonly DependencyProperty TooltipMarginProperty = DependencyProperty.RegisterAttached("TooltipMargin", typeof (Thickness), typeof (ChartTooltip), new PropertyMetadata((object) new Thickness().GetThickness(0.0, 0.0, 0.0, 0.0)));
  internal static readonly DependencyProperty TopOffsetProperty = DependencyProperty.Register(nameof (TopOffset), typeof (double), typeof (ChartTooltip), new PropertyMetadata((object) 0.0));
  internal static readonly DependencyProperty LeftOffsetProperty = DependencyProperty.Register(nameof (LeftOffset), typeof (double), typeof (ChartTooltip), new PropertyMetadata((object) 0.0));
  internal static readonly DependencyProperty PolygonPathProperty = DependencyProperty.Register(nameof (PolygonPath), typeof (string), typeof (ChartTooltip), new PropertyMetadata((object) " "));
  internal static readonly DependencyProperty BackgroundStyleProperty = DependencyProperty.Register(nameof (BackgroundStyle), typeof (Style), typeof (ChartTooltip), new PropertyMetadata((PropertyChangedCallback) null));
  internal static readonly DependencyProperty LabelStyleProperty = DependencyProperty.Register(nameof (LabelStyle), typeof (Style), typeof (ChartTooltip), new PropertyMetadata((PropertyChangedCallback) null));
  private bool isAnnotationTooltip;

  public ChartTooltip()
  {
    this.DefaultStyleKey = (object) typeof (ChartTooltip);
    this.IsHitTestVisible = false;
    Canvas.SetLeft((UIElement) this, 0.0);
    Canvas.SetTop((UIElement) this, 0.0);
  }

  internal ChartTooltip(bool annotationTooltip)
  {
    this.isAnnotationTooltip = annotationTooltip;
    this.IsHitTestVisible = false;
    Canvas.SetLeft((UIElement) this, 0.0);
    Canvas.SetTop((UIElement) this, 0.0);
  }

  internal string PolygonPath
  {
    get => (string) this.GetValue(ChartTooltip.PolygonPathProperty);
    set => this.SetValue(ChartTooltip.PolygonPathProperty, (object) value);
  }

  internal Style BackgroundStyle
  {
    get => (Style) this.GetValue(ChartTooltip.BackgroundStyleProperty);
    set => this.SetValue(ChartTooltip.BackgroundStyleProperty, (object) value);
  }

  internal Style LabelStyle
  {
    get => (Style) this.GetValue(ChartTooltip.LabelStyleProperty);
    set => this.SetValue(ChartTooltip.LabelStyleProperty, (object) value);
  }

  internal double LeftOffset
  {
    get => (double) this.GetValue(ChartTooltip.LeftOffsetProperty);
    set => this.SetValue(ChartTooltip.LeftOffsetProperty, (object) value);
  }

  internal double TopOffset
  {
    get => (double) this.GetValue(ChartTooltip.TopOffsetProperty);
    set => this.SetValue(ChartTooltip.TopOffsetProperty, (object) value);
  }

  internal HorizontalPosition HorizontalPosition { get; set; }

  internal VerticalPosition VerticalPosition { get; set; }

  internal ChartSeriesBase PreviousSeries { get; set; }

  public static bool GetEnableAnimation(UIElement obj)
  {
    return (bool) obj.GetValue(ChartTooltip.EnableAnimationProperty);
  }

  public static void SetEnableAnimation(UIElement obj, bool value)
  {
    obj.SetValue(ChartTooltip.EnableAnimationProperty, (object) value);
  }

  public static HorizontalAlignment GetHorizontalAlignment(UIElement obj)
  {
    return (HorizontalAlignment) obj.GetValue(ChartTooltip.HorizontalAlignmentProperty);
  }

  public static void SetHorizontalAlignment(UIElement obj, HorizontalAlignment value)
  {
    obj.SetValue(ChartTooltip.HorizontalAlignmentProperty, (object) value);
  }

  public static VerticalAlignment GetVerticalAlignment(UIElement obj)
  {
    return (VerticalAlignment) obj.GetValue(ChartTooltip.VerticalAlignmentProperty);
  }

  public static void SetVerticalAlignment(UIElement obj, VerticalAlignment value)
  {
    obj.SetValue(ChartTooltip.VerticalAlignmentProperty, (object) value);
  }

  public static Thickness GetTooltipMargin(UIElement obj)
  {
    return (Thickness) obj.GetValue(ChartTooltip.TooltipMarginProperty);
  }

  public static void SetTooltipMargin(UIElement obj, Thickness value)
  {
    obj.SetValue(ChartTooltip.TooltipMarginProperty, (object) value);
  }

  public static int GetShowDuration(DependencyObject obj)
  {
    return (int) obj.GetValue(ChartTooltip.ShowDurationProperty);
  }

  public static void SetShowDuration(DependencyObject obj, int value)
  {
    obj.SetValue(ChartTooltip.ShowDurationProperty, (object) value);
  }

  public static int GetInitialShowDelay(DependencyObject obj)
  {
    return (int) obj.GetValue(ChartTooltip.InitialShowDelayProperty);
  }

  public static void SetInitialShowDelay(DependencyObject obj, int value)
  {
    obj.SetValue(ChartTooltip.InitialShowDelayProperty, (object) value);
  }

  public static double GetHorizontalOffset(DependencyObject obj)
  {
    return (double) obj.GetValue(ChartTooltip.HorizontalOffsetProperty);
  }

  public static void SetHorizontalOffset(DependencyObject obj, double value)
  {
    obj.SetValue(ChartTooltip.HorizontalOffsetProperty, (object) value);
  }

  public static double GetVerticalOffset(DependencyObject obj)
  {
    return (double) obj.GetValue(ChartTooltip.VerticalOffsetProperty);
  }

  public static void SetVerticalOffset(DependencyObject obj, double value)
  {
    obj.SetValue(ChartTooltip.VerticalOffsetProperty, (object) value);
  }

  internal static HorizontalAlignment GetActualHorizontalAlignment(
    ChartTooltipBehavior tooltipBehavior,
    HorizontalAlignment horizontalAlignment)
  {
    return tooltipBehavior == null || horizontalAlignment != HorizontalAlignment.Center ? horizontalAlignment : tooltipBehavior.HorizontalAlignment;
  }

  internal static VerticalAlignment GetActualVerticalAlignment(
    ChartTooltipBehavior tooltipBehavior,
    VerticalAlignment verticalAlignment)
  {
    return tooltipBehavior == null || verticalAlignment != VerticalAlignment.Top ? verticalAlignment : tooltipBehavior.VerticalAlignment;
  }

  internal static double GetActualHorizontalOffset(
    ChartTooltipBehavior tooltipBehavior,
    double horizontalOffset)
  {
    return tooltipBehavior == null || horizontalOffset != 0.0 ? horizontalOffset : tooltipBehavior.HorizontalOffset;
  }

  internal static double GetActualVerticalOffset(
    ChartTooltipBehavior tooltipBehavior,
    double verticalOffset)
  {
    return tooltipBehavior == null || verticalOffset != 0.0 ? verticalOffset : tooltipBehavior.VerticalOffset;
  }

  internal static int GetActualShowDuration(ChartTooltipBehavior tooltipBehavior, int showDuration)
  {
    return tooltipBehavior == null || showDuration != 1000 ? showDuration : tooltipBehavior.ShowDuration;
  }

  internal static int GetActualInitialShowDelay(
    ChartTooltipBehavior tooltipBehavior,
    int initialShowDelay)
  {
    return tooltipBehavior == null || initialShowDelay != 0 ? initialShowDelay : tooltipBehavior.InitialShowDelay;
  }

  internal static bool GetActualEnableAnimation(
    ChartTooltipBehavior tooltipBehavior,
    bool enableAnimation)
  {
    return tooltipBehavior == null || !enableAnimation ? enableAnimation : tooltipBehavior.EnableAnimation;
  }

  internal static Thickness GetActualTooltipMargin(
    ChartTooltipBehavior tooltipBehavior,
    Thickness margin)
  {
    return tooltipBehavior == null || !margin.Equals(new Thickness().GetThickness(0.0, 0.0, 0.0, 0.0)) ? margin : tooltipBehavior.Margin;
  }
}
