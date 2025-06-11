// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.RangeNavigatorPanel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class RangeNavigatorPanel : Panel
{
  public static readonly DependencyProperty RowProperty = DependencyProperty.RegisterAttached("Row", typeof (int), typeof (RangeNavigatorPanel), new PropertyMetadata((object) 0));
  private RangeNavigatorRowDefinitions rowDefinitions;

  public RangeNavigatorRowDefinitions RowDefinitions
  {
    get
    {
      if (this.rowDefinitions == null)
        this.rowDefinitions = new RangeNavigatorRowDefinitions();
      return this.rowDefinitions;
    }
    set => this.rowDefinitions = value;
  }

  public static int GetRow(UIElement obj) => (int) obj.GetValue(RangeNavigatorPanel.RowProperty);

  public static void SetRow(UIElement obj, int value)
  {
    obj.SetValue(RangeNavigatorPanel.RowProperty, (object) value);
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    foreach (UIElement child in this.Children)
      this.RowDefinitions[RangeNavigatorPanel.GetRow(child)].Element.Add(child);
    return base.MeasureOverride(availableSize);
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    double top = 0.0;
    double num1 = 0.0;
    double num2 = this.RowDefinitions.Sum<RangeNavigatorRowDefinition>((Func<RangeNavigatorRowDefinition, double>) (rowDef => rowDef.Unit == ChartUnitType.Star ? rowDef.Height : 0.0));
    double num3 = this.RowDefinitions.Sum<RangeNavigatorRowDefinition>((Func<RangeNavigatorRowDefinition, double>) (rowDef => rowDef.Unit == ChartUnitType.Pixels ? rowDef.Height : 0.0));
    double num4 = Math.Max(0.0, finalSize.Height - num3) / num2;
    for (int index = 0; index < this.RowDefinitions.Count; ++index)
    {
      RangeNavigatorRowDefinition rowDefinition = this.RowDefinitions[index];
      double val1 = finalSize.Height - num1;
      double d = rowDefinition.Unit != ChartUnitType.Star ? Math.Min(val1, rowDefinition.Height) : Math.Min(val1, rowDefinition.Height * num4);
      rowDefinition.Height = double.IsNaN(d) ? 1.0 : d;
      this.RowDefinitions[index].Arrange(finalSize, top);
      num1 += double.IsNaN(d) ? 1.0 : d;
      rowDefinition.RowTop = top;
      top += double.IsNaN(d) ? 1.0 : d;
    }
    return base.ArrangeOverride(finalSize);
  }
}
