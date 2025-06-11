// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.CoverFlowItem
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

#nullable disable
namespace HandyControl.Controls;

public class CoverFlowItem : ModelVisual3D
{
  internal static readonly double Interval = 0.2;
  internal static readonly double AnimationSpeed = 400.0;
  private readonly AxisAngleRotation3D _rotation3D;
  private readonly TranslateTransform3D _transform3D;
  private readonly Model3DGroup _model3DGroup;
  private readonly UIElement _uiElement;

  internal int Index { get; set; }

  public CoverFlowItem(int itemIndex, int currentIndex, UIElement element)
  {
    this.Index = itemIndex;
    this._uiElement = element;
    this._uiElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    this._rotation3D = new AxisAngleRotation3D(new Vector3D(0.0, 1.0, 0.0), this.GetAngleByPos(currentIndex));
    this._transform3D = new TranslateTransform3D(this.GetXByPos(currentIndex), 0.0, this.GetZByPos(currentIndex));
    Transform3DGroup transform3Dgroup = new Transform3DGroup();
    transform3Dgroup.Children.Add((Transform3D) new RotateTransform3D((Rotation3D) this._rotation3D));
    transform3Dgroup.Children.Add((Transform3D) this._transform3D);
    this._model3DGroup = new Model3DGroup();
    this._model3DGroup.Children.Add((Model3D) new GeometryModel3D(this.CreateItemGeometry(), (Material) new DiffuseMaterial((Brush) new VisualBrush((Visual) element))));
    this._model3DGroup.Transform = (Transform3D) transform3Dgroup;
    this.Content = (Model3D) this._model3DGroup;
  }

  private static MeshGeometry3D CreateMeshGeometry(Point3D p0, Point3D p1, Point3D p2, Point3D p3)
  {
    Point3DCollection point3Dcollection = new Point3DCollection()
    {
      p0,
      p1,
      p2,
      p3
    };
    Int32Collection int32Collection = new Int32Collection()
    {
      0,
      1,
      2,
      2,
      3,
      0
    };
    PointCollection pointCollection = new PointCollection()
    {
      new Point(0.0, 1.0),
      new Point(1.0, 1.0),
      new Point(1.0, 0.0),
      new Point(0.0, 0.0),
      new Point(1.0, 0.0),
      new Point(1.0, 1.0)
    };
    Vector3DCollection vector3Dcollection = new Vector3DCollection()
    {
      ArithmeticHelper.CalNormal(p0, p1, p2),
      ArithmeticHelper.CalNormal(p2, p3, p0)
    };
    MeshGeometry3D meshGeometry = new MeshGeometry3D();
    meshGeometry.Positions = point3Dcollection;
    meshGeometry.TriangleIndices = int32Collection;
    meshGeometry.TextureCoordinates = pointCollection;
    meshGeometry.Normals = vector3Dcollection;
    meshGeometry.Freeze();
    return meshGeometry;
  }

  private Geometry3D CreateItemGeometry()
  {
    Size desiredSize1 = this._uiElement.DesiredSize;
    double width1 = desiredSize1.Width;
    desiredSize1 = this._uiElement.DesiredSize;
    double height1 = desiredSize1.Height;
    double num1;
    if (width1 <= height1)
    {
      Size desiredSize2 = this._uiElement.DesiredSize;
      double width2 = desiredSize2.Width;
      desiredSize2 = this._uiElement.DesiredSize;
      double height2 = desiredSize2.Height;
      num1 = 1.0 - width2 / height2;
    }
    else
      num1 = 0.0;
    double num2 = num1;
    Size desiredSize3 = this._uiElement.DesiredSize;
    double width3 = desiredSize3.Width;
    desiredSize3 = this._uiElement.DesiredSize;
    double height3 = desiredSize3.Height;
    double num3;
    if (width3 >= height3)
    {
      Size desiredSize4 = this._uiElement.DesiredSize;
      double height4 = desiredSize4.Height;
      desiredSize4 = this._uiElement.DesiredSize;
      double width4 = desiredSize4.Width;
      num3 = 1.0 - height4 / width4;
    }
    else
      num3 = 0.0;
    double num4 = num3;
    Point3D p0 = new Point3D(num2 - 1.0, num4 - 1.0, 0.0);
    Point3D point3D1 = new Point3D(1.0 - num2, num4 - 1.0, 0.0);
    Point3D point3D2 = new Point3D(1.0 - num2, 1.0 - num4, 0.0);
    Point3D point3D3 = new Point3D(num2 - 1.0, 1.0 - num4, 0.0);
    Point3D p1 = point3D1;
    Point3D p2 = point3D2;
    Point3D p3 = point3D3;
    return (Geometry3D) CoverFlowItem.CreateMeshGeometry(p0, p1, p2, p3);
  }

  private double GetAngleByPos(int index) => (double) (Math.Sign(this.Index - index) * -90);

  private double GetXByPos(int index)
  {
    return (double) this.Index * CoverFlowItem.Interval + (double) Math.Sign(this.Index - index) * 1.5;
  }

  private double GetZByPos(int index) => (double) (this.Index == index ? 1 : 0);

  internal void Move(int currentIndex)
  {
    this._rotation3D.BeginAnimation(AxisAngleRotation3D.AngleProperty, (AnimationTimeline) AnimationHelper.CreateAnimation(this.GetAngleByPos(currentIndex), CoverFlowItem.AnimationSpeed));
    this._transform3D.BeginAnimation(TranslateTransform3D.OffsetXProperty, (AnimationTimeline) AnimationHelper.CreateAnimation(this.GetXByPos(currentIndex), CoverFlowItem.AnimationSpeed));
    this._transform3D.BeginAnimation(TranslateTransform3D.OffsetZProperty, (AnimationTimeline) AnimationHelper.CreateAnimation(this.GetZByPos(currentIndex), CoverFlowItem.AnimationSpeed));
  }

  internal bool HitTest(MeshGeometry3D mesh)
  {
    foreach (Model3D child in this._model3DGroup.Children)
    {
      if (child is GeometryModel3D geometryModel3D && object.Equals((object) geometryModel3D.Geometry, (object) mesh))
        return true;
    }
    return false;
  }
}
