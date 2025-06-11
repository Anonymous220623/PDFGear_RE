// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.Graphics3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class Graphics3D
{
  private readonly BspTreeBuilder treeBuilder = new BspTreeBuilder();
  private ChartTransform.ChartTransform3D transform;
  private BspNode tree;

  public ChartTransform.ChartTransform3D Transform
  {
    get => this.transform;
    set
    {
      if (this.transform == value)
        return;
      this.transform = value;
    }
  }

  public int GetVisualCount() => this.treeBuilder.Count();

  public int AddVisual(Polygon3D polygon)
  {
    if (polygon == null || polygon.Test())
      return -1;
    polygon.Graphics3D = this;
    return this.treeBuilder.Add(polygon);
  }

  public void Remove(Polygon3D polygon) => this.treeBuilder.Remove(polygon);

  public void ClearVisual() => this.treeBuilder.Clear();

  public List<Polygon3D> GetVisual() => this.treeBuilder.Polygons;

  public void PrepareView() => this.tree = this.treeBuilder.Build();

  public void PrepareView(
    double perspectiveAngle,
    double depth,
    double rotation,
    double tilt,
    Size size)
  {
    if (this.transform == null)
      this.transform = new ChartTransform.ChartTransform3D(size);
    else
      this.transform.mViewport = size;
    this.transform.Rotation = rotation;
    this.transform.Tilt = tilt;
    this.transform.Depth = depth;
    this.transform.PerspectiveAngle = perspectiveAngle;
    this.transform.Transform();
    this.tree = this.treeBuilder.Build();
  }

  public void View(Panel panel)
  {
    if (panel == null)
      return;
    panel.Children.Clear();
    this.DrawBspNode3D(this.tree, new Vector3D(0.0, 0.0, (double) short.MaxValue), panel);
  }

  public void View(
    Panel panel,
    double rotation,
    double tilt,
    Size size,
    double perspectiveAngle,
    double depth)
  {
    if (panel == null)
      return;
    panel.Children.Clear();
    if (this.transform == null)
      this.transform = new ChartTransform.ChartTransform3D(size);
    else
      this.transform.mViewport = size;
    this.transform.Rotation = rotation;
    this.transform.Tilt = tilt;
    this.transform.Depth = depth;
    this.transform.PerspectiveAngle = perspectiveAngle;
    this.transform.Transform();
    this.DrawBspNode3D(this.tree, new Vector3D(0.0, 0.0, (double) short.MaxValue), panel);
  }

  private void DrawBspNode3D(BspNode tree, Vector3D eye, Panel panel)
  {
    if (tree == null || this.transform == null)
      return;
    while (true)
    {
      for (; (tree.Plane.GetNormal(this.transform.Result) & eye) <= tree.Plane.D; tree = tree.Front)
      {
        if (tree.Back != null)
          this.DrawBspNode3D(tree.Back, eye, panel);
        tree.Plane.Draw(panel);
        if (tree.Front == null)
          return;
      }
      if (tree.Front != null)
        this.DrawBspNode3D(tree.Front, eye, panel);
      tree.Plane.Draw(panel);
      if (tree.Back != null)
        tree = tree.Back;
      else
        break;
    }
  }
}
