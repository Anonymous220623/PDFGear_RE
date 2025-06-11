// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.CarouselPanelHelperMethods
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal static class CarouselPanelHelperMethods
{
  public static Path GetPath()
  {
    Path path = new Path();
    object obj = new PathFigureCollectionConverter().ConvertFromString("M639,-115.5 C702,-106.5 666.49972,-35 491.49972,-35 300.4994,-35 293.49973,-116 343.50004,-116");
    PathGeometry pathGeometry = new PathGeometry();
    path.Stretch = Stretch.Fill;
    BrushConverter brushConverter = new BrushConverter();
    path.Stroke = (Brush) brushConverter.ConvertFromString("#FF0998f8");
    path.StrokeThickness = 2.0;
    pathGeometry.Figures = (PathFigureCollection) obj;
    path.Data = (Geometry) pathGeometry;
    return path;
  }

  public static PathFractionCollection GetDefaultFractionsCollection(double fraction)
  {
    PathFractionCollection fractionsCollection = new PathFractionCollection();
    fractionsCollection.Add(new FractionValue()
    {
      Fraction = 0.0,
      Value = fraction
    });
    fractionsCollection.Add(new FractionValue()
    {
      Fraction = 0.5,
      Value = 1.0
    });
    fractionsCollection.Add(new FractionValue()
    {
      Fraction = 1.0,
      Value = fraction
    });
    return fractionsCollection;
  }

  internal static int GetItemCountlater(PathFractionRangeHandler range, int itemCount)
  {
    return range.LastVisibleItemIndex >= itemCount ? 0 : itemCount - range.LastVisibleItemIndex - 1;
  }

  internal static int GetItemCountBefore(PathFractionRangeHandler range)
  {
    return range.FirstVisibleItemIndex < 0 ? 0 : range.FirstVisibleItemIndex;
  }

  public static bool IsInRange(int value, int min, int max) => value >= min && value <= max;

  public static int CoerceRangeValues(int value, int min, int max)
  {
    int num = value <= max ? value : max;
    if (num < min)
      num = min;
    return num;
  }
}
