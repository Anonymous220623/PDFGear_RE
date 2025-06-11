// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartTooltipBehavior
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.ObjectModel;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartTooltipBehavior : ChartBehavior
{
  public static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.Register(nameof (HorizontalAlignment), typeof (HorizontalAlignment), typeof (ChartTooltipBehavior), new PropertyMetadata((object) HorizontalAlignment.Center));
  public static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.Register(nameof (VerticalAlignment), typeof (VerticalAlignment), typeof (ChartTooltipBehavior), new PropertyMetadata((object) VerticalAlignment.Top));
  public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(nameof (HorizontalOffset), typeof (double), typeof (ChartTooltipBehavior), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(nameof (VerticalOffset), typeof (double), typeof (ChartTooltipBehavior), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty InitialShowDelayProperty = DependencyProperty.Register(nameof (InitialShowDelay), typeof (int), typeof (ChartTooltipBehavior), new PropertyMetadata((object) 0));
  public static readonly DependencyProperty ShowDurationProperty = DependencyProperty.Register(nameof (ShowDuration), typeof (int), typeof (ChartTooltipBehavior), new PropertyMetadata((object) 1000));
  public static readonly DependencyProperty EnableAnimationProperty = DependencyProperty.Register(nameof (EnableAnimation), typeof (bool), typeof (ChartTooltipBehavior), new PropertyMetadata((object) true));
  public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof (Position), typeof (TooltipPosition), typeof (ChartTooltipBehavior), new PropertyMetadata((object) TooltipPosition.Auto));
  public static readonly DependencyProperty StyleProperty = DependencyProperty.Register(nameof (Style), typeof (Style), typeof (ChartTooltipBehavior), (PropertyMetadata) null);
  public static readonly DependencyProperty LabelStyleProperty = DependencyProperty.Register(nameof (LabelStyle), typeof (Style), typeof (ChartTooltipBehavior), new PropertyMetadata((PropertyChangedCallback) null));
  internal static readonly DependencyProperty MarginProperty = DependencyProperty.Register(nameof (Margin), typeof (Thickness), typeof (ChartTooltipBehavior), new PropertyMetadata((object) new Thickness().GetThickness(0.0, 0.0, 0.0, 0.0)));

  public HorizontalAlignment HorizontalAlignment
  {
    get => (HorizontalAlignment) this.GetValue(ChartTooltipBehavior.HorizontalAlignmentProperty);
    set => this.SetValue(ChartTooltipBehavior.HorizontalAlignmentProperty, (object) value);
  }

  public VerticalAlignment VerticalAlignment
  {
    get => (VerticalAlignment) this.GetValue(ChartTooltipBehavior.VerticalAlignmentProperty);
    set => this.SetValue(ChartTooltipBehavior.VerticalAlignmentProperty, (object) value);
  }

  public double HorizontalOffset
  {
    get => (double) this.GetValue(ChartTooltipBehavior.HorizontalOffsetProperty);
    set => this.SetValue(ChartTooltipBehavior.HorizontalOffsetProperty, (object) value);
  }

  public double VerticalOffset
  {
    get => (double) this.GetValue(ChartTooltipBehavior.VerticalOffsetProperty);
    set => this.SetValue(ChartTooltipBehavior.VerticalOffsetProperty, (object) value);
  }

  public int InitialShowDelay
  {
    get => (int) this.GetValue(ChartTooltipBehavior.InitialShowDelayProperty);
    set => this.SetValue(ChartTooltipBehavior.InitialShowDelayProperty, (object) value);
  }

  public int ShowDuration
  {
    get => (int) this.GetValue(ChartTooltipBehavior.ShowDurationProperty);
    set => this.SetValue(ChartTooltipBehavior.ShowDurationProperty, (object) value);
  }

  public bool EnableAnimation
  {
    get => (bool) this.GetValue(ChartTooltipBehavior.EnableAnimationProperty);
    set => this.SetValue(ChartTooltipBehavior.EnableAnimationProperty, (object) value);
  }

  public TooltipPosition Position
  {
    get => (TooltipPosition) this.GetValue(ChartTooltipBehavior.PositionProperty);
    set => this.SetValue(ChartTooltipBehavior.PositionProperty, (object) value);
  }

  public Style Style
  {
    get => (Style) this.GetValue(ChartTooltipBehavior.StyleProperty);
    set => this.SetValue(ChartTooltipBehavior.StyleProperty, (object) value);
  }

  public Style LabelStyle
  {
    get => (Style) this.GetValue(ChartTooltipBehavior.LabelStyleProperty);
    set => this.SetValue(ChartTooltipBehavior.LabelStyleProperty, (object) value);
  }

  internal Thickness Margin
  {
    get => (Thickness) this.GetValue(ChartTooltipBehavior.MarginProperty);
    set => this.SetValue(ChartTooltipBehavior.MarginProperty, (object) value);
  }

  protected override DependencyObject CloneBehavior(DependencyObject obj)
  {
    return base.CloneBehavior((DependencyObject) new ChartTooltipBehavior()
    {
      HorizontalAlignment = this.HorizontalAlignment,
      VerticalAlignment = this.VerticalAlignment,
      HorizontalOffset = this.HorizontalOffset,
      VerticalOffset = this.VerticalOffset,
      ShowDuration = this.ShowDuration,
      InitialShowDelay = this.InitialShowDelay,
      Margin = this.Margin,
      Position = this.Position,
      Style = this.Style,
      LabelStyle = this.LabelStyle
    });
  }

  protected internal override void OnSizeChanged(SizeChangedEventArgs e)
  {
    if (this.ChartArea == null || this.ChartArea.Series == null || this.ChartArea.Series.Count <= 0 || !this.ChartArea.ShowTooltip)
      return;
    foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeries>) this.ChartArea.Series)
      chartSeriesBase.RemoveTooltip();
  }
}
