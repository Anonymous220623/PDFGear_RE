// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.PolygonItem
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

public class PolygonItem : Control, IDisposable
{
  public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(nameof (Points), typeof (PointCollection), typeof (PolygonItem), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ColorNameProperty = DependencyProperty.Register(nameof (ColorName), typeof (string), typeof (PolygonItem), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty colorProperty = DependencyProperty.Register(nameof (color), typeof (Brush), typeof (PolygonItem), new PropertyMetadata((object) new SolidColorBrush(Colors.Red), new PropertyChangedCallback(PolygonItem.IsColorChanged)));
  public static readonly DependencyProperty RowIndexProperty = DependencyProperty.Register(nameof (RowIndex), typeof (int), typeof (PolygonItem), new PropertyMetadata((object) 0));
  public static readonly DependencyProperty ColumnIndexProperty = DependencyProperty.Register(nameof (ColumnIndex), typeof (int), typeof (PolygonItem), new PropertyMetadata((object) 0));
  private PolygonItem more;
  private Polygon poly;
  internal MoreColorsWindow child;
  internal Path paths;

  public int ColumnIndex
  {
    get => (int) this.GetValue(PolygonItem.ColumnIndexProperty);
    set => this.SetValue(PolygonItem.ColumnIndexProperty, (object) value);
  }

  public string ColorName
  {
    get => (string) this.GetValue(PolygonItem.ColorNameProperty);
    set => this.SetValue(PolygonItem.ColorNameProperty, (object) value);
  }

  public int RowIndex
  {
    get => (int) this.GetValue(PolygonItem.RowIndexProperty);
    set => this.SetValue(PolygonItem.RowIndexProperty, (object) value);
  }

  public Brush color
  {
    get => (Brush) this.GetValue(PolygonItem.colorProperty);
    set => this.SetValue(PolygonItem.colorProperty, (object) value);
  }

  public PointCollection Points
  {
    get => (PointCollection) this.GetValue(PolygonItem.PointsProperty);
    set => this.SetValue(PolygonItem.PointsProperty, (object) value);
  }

  public PolygonItem() => this.DefaultStyleKey = (object) typeof (PolygonItem);

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.poly = this.GetTemplateChild("polygon") as Polygon;
    this.poly.FocusVisualStyle = (Style) null;
    this.Focusable = false;
    this.poly.MouseLeftButtonDown += new MouseButtonEventHandler(this.polyMouseLeftButtonDown);
    this.child = PolygonItem.GetBrushEditParentFromChildren((FrameworkElement) this.poly);
    this.more = new PolygonItem();
  }

  internal static MoreColorsWindow GetBrushEditParentFromChildren(FrameworkElement element)
  {
    parentFromChildren = (MoreColorsWindow) null;
    if (element != null && !(element is MoreColorsWindow parentFromChildren))
    {
      while (element != null)
      {
        element = VisualTreeHelper.GetParent((DependencyObject) element) as FrameworkElement;
        if (element is MoreColorsWindow)
        {
          parentFromChildren = (MoreColorsWindow) element;
          break;
        }
      }
    }
    return parentFromChildren;
  }

  private void polyMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    int num = 0;
    this.child.polygonitem = this;
    this.child.New.Background = this.color;
    this.child.palette.child.polygonitem.color = this.color;
    this.child.palette.child.polygonitem.Points = this.Points;
    if (this.child.palette.child.polygonitem.RowIndex == 7 && this.child.palette.child.polygonitem.ColumnIndex == 7)
    {
      foreach (PolygonItem polygonItem in this.child.palette.child.morecolorcollection.Where<PolygonItem>((Func<PolygonItem, bool>) (more => more.RowIndex == 14 && more.ColumnIndex == 1)))
      {
        this.child.palette.child.path1.Stroke = (Brush) new SolidColorBrush(Colors.Black);
        this.child.palette.child.path1.Fill = (Brush) new SolidColorBrush(Colors.White);
        this.child.palette.child.path1.Data = (Geometry) this.DrawPath(polygonItem.Points);
        num = 1;
      }
    }
    if (this.child.palette.child.polygonitem.RowIndex == 14 && this.child.palette.child.polygonitem.ColumnIndex == 1)
    {
      foreach (PolygonItem polygonItem in this.child.palette.child.morecolorcollection.Where<PolygonItem>((Func<PolygonItem, bool>) (more => more.RowIndex == 7 && more.ColumnIndex == 7)))
      {
        this.child.palette.child.path1.Stroke = (Brush) new SolidColorBrush(Colors.Black);
        this.child.palette.child.path1.Fill = (Brush) new SolidColorBrush(Colors.White);
        this.child.palette.child.path1.Data = (Geometry) this.DrawPath(polygonItem.Points);
        num = 1;
      }
    }
    if (num != 1)
      this.child.palette.child.path1.Data = (Geometry) null;
    this.paths = this.child.palette.child.path;
    if (this.paths != null)
    {
      this.paths.Stroke = (Brush) new SolidColorBrush(Colors.Black);
      this.paths.Fill = (Brush) new SolidColorBrush(Colors.White);
      this.paths.Data = (Geometry) this.DrawPath(this.child.palette.child.polygonitem.Points);
    }
    this.child.Item.Focus();
  }

  internal PathGeometry DrawPath(PointCollection points)
  {
    PathGeometry pathGeometry1 = new PathGeometry();
    pathGeometry1.Figures.Add(new PathFigure()
    {
      StartPoint = points[0],
      Segments = {
        (PathSegment) new LineSegment()
        {
          Point = points[1]
        },
        (PathSegment) new LineSegment()
        {
          Point = points[2]
        },
        (PathSegment) new LineSegment()
        {
          Point = points[3]
        },
        (PathSegment) new LineSegment()
        {
          Point = points[4]
        },
        (PathSegment) new LineSegment()
        {
          Point = points[5]
        },
        (PathSegment) new LineSegment()
        {
          Point = points[0]
        }
      }
    });
    PathGeometry pathGeometry2 = new PathGeometry();
    pathGeometry1.Figures.Add(new PathFigure()
    {
      StartPoint = new Point(points[0].X + 2.0, points[0].Y + 1.0),
      Segments = {
        (PathSegment) new LineSegment()
        {
          Point = new Point(points[1].X, points[1].Y + 3.0)
        },
        (PathSegment) new LineSegment()
        {
          Point = new Point(points[2].X - 3.0, points[2].Y + 1.0)
        },
        (PathSegment) new LineSegment()
        {
          Point = new Point(points[3].X - 3.0, points[3].Y - 1.0)
        },
        (PathSegment) new LineSegment()
        {
          Point = new Point(points[4].X, points[4].Y - 3.0)
        },
        (PathSegment) new LineSegment()
        {
          Point = new Point(points[5].X + 3.0, points[5].Y - 1.0)
        },
        (PathSegment) new LineSegment()
        {
          Point = new Point(points[0].X + 3.0, points[0].Y + 1.0)
        }
      }
    });
    return pathGeometry1;
  }

  private static void IsColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
  {
  }

  internal void Dispose(bool disposing)
  {
    if (!disposing)
      return;
    if (this.poly != null)
    {
      this.poly.MouseLeftButtonDown -= new MouseButtonEventHandler(this.polyMouseLeftButtonDown);
      this.poly = (Polygon) null;
    }
    if (this.child != null)
      this.child = (MoreColorsWindow) null;
    if (this.more != null)
      this.more = (PolygonItem) null;
    if (this.paths == null)
      return;
    this.paths = (Path) null;
  }

  public void Dispose() => this.Dispose(true);
}
