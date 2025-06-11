// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.CarouselPathHelper
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal class CarouselPathHelper
{
  private Path _CarouselPath;
  private PathGeometry _Geometry;
  private Syncfusion.Windows.Shared.PathFractions[] _PathFractions;
  internal Syncfusion.Windows.Shared.PathFractions topElementPathFraction;

  public CarouselPathHelper(Path Path, int ItemsPerPage)
  {
    this.Geometry = Path.Data == null ? PathGeometry.CreateFromGeometry(System.Windows.Media.Geometry.Empty) : PathGeometry.CreateFromGeometry(Path.Data);
    this.CarouselPath = Path;
    this._PathFractions = CarouselPathHelper.PathFraction(ItemsPerPage);
  }

  public Path CarouselPath
  {
    get => this._CarouselPath;
    set => this._CarouselPath = value;
  }

  public PathGeometry Geometry
  {
    get => this._Geometry;
    set => this._Geometry = value;
  }

  public Syncfusion.Windows.Shared.PathFractions[] PathFractions => this._PathFractions;

  public int TopElementPathFractionIndex
  {
    get => this.GetPathFractionIndex(this.topElementPathFraction.PathFraction);
  }

  public double TopElementPathFraction => this.topElementPathFraction.PathFraction;

  public void SetTopElementPathFraction(Syncfusion.Windows.Shared.PathFractions desiredPathFraction)
  {
    desiredPathFraction.PathFraction = this.PathFractions[((IEnumerable<Syncfusion.Windows.Shared.PathFractions>) this.PathFractions).Count<Syncfusion.Windows.Shared.PathFractions>() / 2].PathFraction;
    this.topElementPathFraction = new Syncfusion.Windows.Shared.PathFractions(desiredPathFraction.PathFraction);
    if (this.IsPathFractionDifferentFromStartAndEndFractions(desiredPathFraction))
      return;
    Syncfusion.Windows.Shared.PathFractions pathFractions1 = this.NearestPathFractionAtLeft(desiredPathFraction.PathFraction);
    Syncfusion.Windows.Shared.PathFractions pathFractions2 = this.NearestPathFractionAtRight(desiredPathFraction.PathFraction);
    if (pathFractions1 == null || pathFractions1.PathFraction == 0.0)
      this.topElementPathFraction = pathFractions2;
    else if (pathFractions2 == null || pathFractions2.PathFraction == 1.0)
      this.topElementPathFraction = pathFractions1;
    else if (Math.Abs(pathFractions1.PathFraction - desiredPathFraction.PathFraction) <= Math.Abs(pathFractions2.PathFraction - desiredPathFraction.PathFraction))
      this.topElementPathFraction = pathFractions1;
    else
      this.topElementPathFraction = pathFractions2;
  }

  public Syncfusion.Windows.Shared.PathFractions NearestPathFractionAtLeft(double pathFraction)
  {
    int index = CarouselPathHelper.NearestPathFractionAtLeft(this.PathFractions, pathFraction);
    return index != -1 ? this.PathFractions[index] : (Syncfusion.Windows.Shared.PathFractions) null;
  }

  public static int NearestPathFractionAtLeft(Syncfusion.Windows.Shared.PathFractions[] _pathfractions, double pathFraction)
  {
    int num1 = -1;
    int pathFractionIndex = CarouselPathHelper.FindNearestPathFractionIndex(pathFraction, _pathfractions);
    if (pathFractionIndex < 0)
    {
      int num2 = Math.Abs(pathFractionIndex) - 1;
      if (num2 - 1 >= 0)
        num1 = num2 - 1;
      return num1;
    }
    if (pathFractionIndex - 1 >= 0)
      num1 = pathFractionIndex - 1;
    return num1;
  }

  private static int FindNearestPathFractionIndex(
    double pathFraction,
    Syncfusion.Windows.Shared.PathFractions[] PathFractions)
  {
    Syncfusion.Windows.Shared.PathFractions pathFractions = new Syncfusion.Windows.Shared.PathFractions(pathFraction);
    int pathFractionIndex = 0;
    foreach (Syncfusion.Windows.Shared.PathFractions pathFraction1 in PathFractions)
    {
      if (pathFraction1.PathFraction >= pathFraction)
        return pathFractionIndex;
      ++pathFractionIndex;
    }
    return -1;
  }

  public Syncfusion.Windows.Shared.PathFractions NearestPathFractionAtRight(double pathFraction)
  {
    int index = CarouselPathHelper.NearestPathFractionAtRight(this.PathFractions, pathFraction);
    return index != -1 ? this.PathFractions[index] : (Syncfusion.Windows.Shared.PathFractions) null;
  }

  public static int NearestPathFractionAtRight(Syncfusion.Windows.Shared.PathFractions[] _pathFractions, double pathFraction)
  {
    int num1 = -1;
    int pathFractionIndex = CarouselPathHelper.FindNearestPathFractionIndex(pathFraction, _pathFractions);
    if (pathFractionIndex < 0)
    {
      int num2 = Math.Abs(pathFractionIndex) - 1;
      if (num2 < _pathFractions.Length)
        num1 = num2;
      return num1;
    }
    if (pathFractionIndex + 1 < _pathFractions.Length)
      num1 = pathFractionIndex + 1;
    return num1;
  }

  private bool IsPathFractionDifferentFromStartAndEndFractions(Syncfusion.Windows.Shared.PathFractions _pathFraction)
  {
    return this.IsPathFraction(_pathFraction.PathFraction) && _pathFraction.PathFraction != 0.0 && _pathFraction.PathFraction != 1.0;
  }

  public bool IsPathFraction(double pathFraction) => this.GetPathFractionIndex(pathFraction) >= 0;

  public static Syncfusion.Windows.Shared.PathFractions[] PathFraction(int ItemsPerPage)
  {
    int length = ItemsPerPage % 2 != 0 ? ItemsPerPage + 2 : ItemsPerPage + 3;
    Syncfusion.Windows.Shared.PathFractions[] pathFractionsArray = new Syncfusion.Windows.Shared.PathFractions[length];
    double num = Math.Round(1.0 / (double) (length - 1), 3);
    for (int index = 0; index < length; ++index)
    {
      double pathFraction = Math.Round(num * (double) index, 3);
      pathFractionsArray.SetValue((object) new Syncfusion.Windows.Shared.PathFractions(pathFraction), index);
    }
    return pathFractionsArray;
  }

  internal void UpdateCustomPath(Size availablesize, Thickness padding, Size itemSize)
  {
    if (this.Geometry == null)
      return;
    this.Geometry.Transform = (Transform) null;
    Rect ViewPort = new Rect(padding.Left + itemSize.Width / 2.0, padding.Top + itemSize.Height / 2.0, itemSize.Width < availablesize.Width ? availablesize.Width - (padding.Left + padding.Right + itemSize.Width) : availablesize.Width, itemSize.Height < availablesize.Height ? availablesize.Height - (padding.Top + padding.Bottom + itemSize.Height) : availablesize.Height);
    ScaleTransform scaleTransform = this.ScaleCustomPathWithAvailableSize(ViewPort, this.Geometry.Bounds, this.CarouselPath.Stretch);
    Rect GeometryBounds = CarouselPathHelper.ChangeGeometryBounds(this.Geometry.Bounds, scaleTransform.Value);
    TranslateTransform availableSize = this.ChangeCustomPathToAvailableSize(ViewPort, GeometryBounds);
    TransformGroup transformGroup = new TransformGroup();
    transformGroup.Children.Add((Transform) scaleTransform);
    transformGroup.Children.Add((Transform) availableSize);
    availableSize.Freeze();
    this.Geometry.Transform = (Transform) transformGroup;
  }

  private ScaleTransform ScaleCustomPathWithAvailableSize(
    Rect ViewPort,
    Rect GeometryBounds,
    Stretch Stretch)
  {
    double num1 = double.IsNaN(this.CarouselPath.Width) ? ViewPort.Width : this.CarouselPath.Width;
    double num2 = double.IsNaN(this.CarouselPath.Height) ? ViewPort.Height : this.CarouselPath.Height;
    double val1 = GeometryBounds.Width == 0.0 ? num1 : num1 / GeometryBounds.Width;
    double val2 = GeometryBounds.Height == 0.0 ? num2 : num2 / GeometryBounds.Height;
    switch (Stretch)
    {
      case Stretch.None:
        val1 = val2 = 1.0;
        break;
      case Stretch.Uniform:
        val1 = val2 = Math.Min(val1, val2);
        break;
      case Stretch.UniformToFill:
        val1 = val2 = Math.Max(val1, val2);
        break;
    }
    return new ScaleTransform()
    {
      ScaleX = val1,
      ScaleY = val2
    };
  }

  private static Rect ChangeGeometryBounds(Rect CurrentGeometryBounds, Matrix Transformation)
  {
    Rect rect = Rect.Transform(CurrentGeometryBounds, Transformation);
    rect.Width = rect.Width == 0.0 ? 0.5 : rect.Width;
    rect.Height = rect.Height == 0.0 ? 0.5 : rect.Height;
    return rect;
  }

  private TranslateTransform ChangeCustomPathToAvailableSize(Rect ViewPort, Rect GeometryBounds)
  {
    double left = ViewPort.Left;
    double top = ViewPort.Top;
    double num1 = double.IsNaN(this.CarouselPath.Width) ? GeometryBounds.Width : this.CarouselPath.Width;
    double num2 = double.IsNaN(this.CarouselPath.Height) ? GeometryBounds.Height : this.CarouselPath.Height;
    double RemainingWidth = Math.Max(0.0, ViewPort.Width - num1);
    double RemainHeight = Math.Max(0.0, ViewPort.Height - num2);
    double num3 = left + CarouselPathHelper.CalculateHorizontalTransformation(this.CarouselPath.HorizontalAlignment, RemainingWidth);
    double num4 = top + CarouselPathHelper.CalculateVerticalTransformation(this.CarouselPath.VerticalAlignment, RemainHeight);
    return new TranslateTransform(num3 - GeometryBounds.Left, num4 - GeometryBounds.Top);
  }

  private static double CalculateHorizontalTransformation(
    HorizontalAlignment Alignment,
    double RemainingWidth)
  {
    switch (Alignment)
    {
      case HorizontalAlignment.Center:
      case HorizontalAlignment.Stretch:
        return Math.Max(0.0, RemainingWidth / 2.0);
      case HorizontalAlignment.Right:
        return Math.Max(0.0, RemainingWidth);
      default:
        return 0.0;
    }
  }

  private static double CalculateVerticalTransformation(
    VerticalAlignment Alignment,
    double RemainHeight)
  {
    switch (Alignment)
    {
      case VerticalAlignment.Center:
      case VerticalAlignment.Stretch:
        return Math.Max(0.0, RemainHeight / 2.0);
      case VerticalAlignment.Bottom:
        return Math.Max(0.0, RemainHeight);
      default:
        return 0.0;
    }
  }

  public int GetPathFractionIndex(double pathFraction)
  {
    int pathFractionIndex = 0;
    foreach (Syncfusion.Windows.Shared.PathFractions pathFraction1 in this.PathFractions)
    {
      if (pathFraction1.PathFraction >= pathFraction)
        return pathFractionIndex;
      ++pathFractionIndex;
    }
    return -1;
  }

  public int GetVisiblePathFractionCount() => this.PathFractions.Length;

  public static bool IsVisible(double pathFraction)
  {
    return pathFraction != -1.0 && pathFraction != 0.0 && pathFraction != 1.0;
  }

  public int CompareCustomPathFractions(Syncfusion.Windows.Shared.PathFractions x, Syncfusion.Windows.Shared.PathFractions y)
  {
    if (x == null)
      throw new ArgumentNullException(nameof (x));
    if (y == null)
      throw new ArgumentNullException(nameof (y));
    return x.PathFraction.CompareTo(y.PathFraction);
  }

  public Syncfusion.Windows.Shared.PathFractions GetVisiblePathFraction(int index)
  {
    return index >= 0 && index != this.GetVisiblePathFractionCount() ? this.PathFractions[index + 1] : (Syncfusion.Windows.Shared.PathFractions) null;
  }
}
