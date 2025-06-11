// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.RangeNavigatorRowDefinition
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class RangeNavigatorRowDefinition : DependencyObject
{
  public static readonly DependencyProperty HeightProperty = DependencyProperty.Register(nameof (Height), typeof (double), typeof (RangeNavigatorRowDefinition), new PropertyMetadata((object) 1.0));
  public static readonly DependencyProperty UnitProperty = DependencyProperty.Register(nameof (Unit), typeof (ChartUnitType), typeof (RangeNavigatorRowDefinition), new PropertyMetadata((object) ChartUnitType.Star));
  public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(nameof (BorderThickness), typeof (double), typeof (RangeNavigatorRowDefinition), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty BorderStrokeProperty = DependencyProperty.Register(nameof (BorderStroke), typeof (Brush), typeof (RangeNavigatorRowDefinition), new PropertyMetadata((object) new SolidColorBrush(Colors.Red)));
  private List<UIElement> element;
  private Rect[] elementBounds;

  public RangeNavigatorRowDefinition() => this.element = new List<UIElement>();

  public double RowTop { get; internal set; }

  public double Height
  {
    get => (double) this.GetValue(RangeNavigatorRowDefinition.HeightProperty);
    set => this.SetValue(RangeNavigatorRowDefinition.HeightProperty, (object) value);
  }

  public ChartUnitType Unit
  {
    get => (ChartUnitType) this.GetValue(RangeNavigatorRowDefinition.UnitProperty);
    set => this.SetValue(RangeNavigatorRowDefinition.UnitProperty, (object) value);
  }

  public double BorderThickness
  {
    get => (double) this.GetValue(RangeNavigatorRowDefinition.BorderThicknessProperty);
    set => this.SetValue(RangeNavigatorRowDefinition.BorderThicknessProperty, (object) value);
  }

  public Brush BorderStroke
  {
    get => (Brush) this.GetValue(RangeNavigatorRowDefinition.BorderStrokeProperty);
    set => this.SetValue(RangeNavigatorRowDefinition.BorderStrokeProperty, (object) value);
  }

  internal List<UIElement> Element
  {
    get => this.element;
    set => this.element = value;
  }

  internal void Measure(Size size, int rowIndex, double rowHeight)
  {
    this.elementBounds = new Rect[this.Element.Count];
    for (int index = 0; index < this.elementBounds.Length; ++index)
    {
      if (this.Unit == ChartUnitType.Pixels)
        this.elementBounds[index].Height = this.Height;
    }
  }

  internal void Arrange(Size availableSize, double top)
  {
    foreach (UIElement uiElement in this.Element)
    {
      double height = this.Height < 0.0 ? 1.0 : this.Height;
      uiElement.Measure(new Size(availableSize.Width, height));
      uiElement.Arrange(new Rect(0.0, top, availableSize.Width, height));
    }
  }
}
