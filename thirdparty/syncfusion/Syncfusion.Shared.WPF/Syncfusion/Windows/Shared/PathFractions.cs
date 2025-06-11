// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.PathFractions
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class PathFractions : DependencyObject
{
  public static readonly DependencyProperty PathFractionProperty = DependencyProperty.Register(nameof (PathFraction), typeof (double), typeof (PathFractions), new PropertyMetadata((object) 0.0, (PropertyChangedCallback) null, new CoerceValueCallback(PathFractions.CoercePathFraction)));

  public PathFractions()
  {
  }

  public PathFractions(double pathFraction) => this.PathFraction = pathFraction;

  public double PathFraction
  {
    get => (double) this.GetValue(PathFractions.PathFractionProperty);
    set => this.SetValue(PathFractions.PathFractionProperty, (object) value);
  }

  private static object CoercePathFraction(DependencyObject d, object baseValue)
  {
    double num = (double) baseValue;
    if (num < 0.0)
      num = 0.0;
    if (num > 1.0)
      num = 1.0;
    return (object) num;
  }
}
