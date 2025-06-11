// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.MeshGenerator
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public static class MeshGenerator
{
  internal static MeshGeometry3D PlaneXYZ(
    double width,
    double height,
    double leftOffset,
    double rightOffset)
  {
    MeshGeometry3D meshGeometry3D = new MeshGeometry3D();
    double num = 0.5 * width;
    double y = 0.5 * height;
    meshGeometry3D.Positions.Add(new Point3D(-num - leftOffset, -y, 0.0));
    meshGeometry3D.Positions.Add(new Point3D(num + rightOffset, -y, 0.0));
    meshGeometry3D.Positions.Add(new Point3D(num + rightOffset, y, 0.0));
    meshGeometry3D.Positions.Add(new Point3D(-num - leftOffset, y, 0.0));
    meshGeometry3D.TextureCoordinates.Add(new Point(0.0, 1.0));
    meshGeometry3D.TextureCoordinates.Add(new Point(1.0, 1.0));
    meshGeometry3D.TextureCoordinates.Add(new Point(1.0, 0.0));
    meshGeometry3D.TextureCoordinates.Add(new Point(0.0, 0.0));
    meshGeometry3D.TriangleIndices.Add(0);
    meshGeometry3D.TriangleIndices.Add(1);
    meshGeometry3D.TriangleIndices.Add(2);
    meshGeometry3D.TriangleIndices.Add(0);
    meshGeometry3D.TriangleIndices.Add(2);
    meshGeometry3D.TriangleIndices.Add(3);
    meshGeometry3D.Normals.Add(new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0));
    meshGeometry3D.Normals.Add(new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0));
    meshGeometry3D.Normals.Add(new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0));
    meshGeometry3D.Normals.Add(new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0));
    return meshGeometry3D;
  }

  internal static MeshGeometry3D PlaneZ(double width, double height)
  {
    MeshGeometry3D meshGeometry3D = new MeshGeometry3D();
    double z = 0.5 * width;
    double y = 0.5 * height;
    meshGeometry3D.Positions.Add(new Point3D(0.0, -y, -z));
    meshGeometry3D.Positions.Add(new Point3D(0.0, -y, z));
    meshGeometry3D.Positions.Add(new Point3D(0.0, y, z));
    meshGeometry3D.Positions.Add(new Point3D(0.0, y, -z));
    meshGeometry3D.TextureCoordinates.Add(new Point(0.0, 1.0));
    meshGeometry3D.TextureCoordinates.Add(new Point(1.0, 1.0));
    meshGeometry3D.TextureCoordinates.Add(new Point(1.0, 0.0));
    meshGeometry3D.TextureCoordinates.Add(new Point(0.0, 0.0));
    meshGeometry3D.TriangleIndices.Add(0);
    meshGeometry3D.TriangleIndices.Add(1);
    meshGeometry3D.TriangleIndices.Add(2);
    meshGeometry3D.TriangleIndices.Add(0);
    meshGeometry3D.TriangleIndices.Add(2);
    meshGeometry3D.TriangleIndices.Add(3);
    meshGeometry3D.Normals.Add(new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0));
    meshGeometry3D.Normals.Add(new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0));
    meshGeometry3D.Normals.Add(new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0));
    meshGeometry3D.Normals.Add(new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0));
    return meshGeometry3D;
  }

  internal static MeshGeometry3D PlaneY(
    double width,
    double height,
    double leftOffset,
    double rightOffset)
  {
    MeshGeometry3D meshGeometry3D = new MeshGeometry3D();
    double x = 0.5 * width;
    double num = 0.5 * height;
    meshGeometry3D.Positions.Add(new Point3D(-x, -num - leftOffset, 0.0));
    meshGeometry3D.Positions.Add(new Point3D(x, -num - leftOffset, 0.0));
    meshGeometry3D.Positions.Add(new Point3D(x, num + rightOffset, 0.0));
    meshGeometry3D.Positions.Add(new Point3D(-x, num + rightOffset, 0.0));
    meshGeometry3D.TextureCoordinates.Add(new Point(0.0, 1.0));
    meshGeometry3D.TextureCoordinates.Add(new Point(1.0, 1.0));
    meshGeometry3D.TextureCoordinates.Add(new Point(1.0, 0.0));
    meshGeometry3D.TextureCoordinates.Add(new Point(0.0, 0.0));
    meshGeometry3D.TriangleIndices.Add(0);
    meshGeometry3D.TriangleIndices.Add(1);
    meshGeometry3D.TriangleIndices.Add(2);
    meshGeometry3D.TriangleIndices.Add(0);
    meshGeometry3D.TriangleIndices.Add(2);
    meshGeometry3D.TriangleIndices.Add(3);
    meshGeometry3D.Normals.Add(new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0));
    meshGeometry3D.Normals.Add(new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0));
    meshGeometry3D.Normals.Add(new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0));
    meshGeometry3D.Normals.Add(new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0));
    return meshGeometry3D;
  }

  internal static MeshGeometry3D BuildWall(double sideA, double sideB, double sideC)
  {
    MeshGeometry3D geomerty = new MeshGeometry3D();
    double x = sideA / 2.0;
    double y = sideB / 2.0;
    double z = sideC / 2.0;
    Point3D point3D1 = new Point3D(-x, -y, -z);
    Point3D point3D2 = new Point3D(x, -y, -z);
    Point3D point3D3 = new Point3D(x, y, -z);
    Point3D point3D4 = new Point3D(-x, y, -z);
    Point3D point3D5 = new Point3D(-x, -y, z);
    Point3D point3D6 = new Point3D(x, -y, z);
    Point3D point3D7 = new Point3D(x, y, z);
    Point3D point3D8 = new Point3D(-x, y, z);
    MeshGenerator.AddTexturedQuad(geomerty, point3D3, point3D2, point3D1, point3D4);
    MeshGenerator.AddTexturedQuad(geomerty, point3D8, point3D5, point3D6, point3D7);
    MeshGenerator.AddTexturedQuad(geomerty, point3D4, point3D1, point3D5, point3D8);
    MeshGenerator.AddTexturedQuad(geomerty, point3D7, point3D6, point3D2, point3D3);
    MeshGenerator.AddTexturedQuad(geomerty, point3D4, point3D8, point3D7, point3D3);
    MeshGenerator.AddTexturedQuad(geomerty, point3D5, point3D1, point3D2, point3D6);
    return geomerty;
  }

  private static void AddTexturedQuad(
    MeshGeometry3D geomerty,
    Point3D p1,
    Point3D p2,
    Point3D p3,
    Point3D p4)
  {
    int count = geomerty.Positions.Count;
    geomerty.Positions.Add(p1);
    geomerty.Positions.Add(p2);
    geomerty.Positions.Add(p3);
    geomerty.Positions.Add(p4);
    geomerty.TriangleIndices.Add(count);
    geomerty.TriangleIndices.Add(count + 1);
    geomerty.TriangleIndices.Add(count + 2);
    geomerty.TriangleIndices.Add(count);
    geomerty.TriangleIndices.Add(count + 2);
    geomerty.TriangleIndices.Add(count + 3);
  }

  internal static Point3D GetNormalize(
    Point3D pt,
    double xmin,
    double xmax,
    double ymin,
    double ymax,
    double zmin,
    double zmax)
  {
    pt.X = 2.5 * (pt.X - xmin) / (xmax - xmin) - 1.25;
    pt.Y = 2.0 * (pt.Y - ymin) / (ymax - ymin) - 1.0;
    pt.Z = (2.0 * (pt.Z - zmin) / (zmax - zmin) - 1.0) * -1.0;
    return pt;
  }

  internal static ImageBrush DrawMaterial(List<Brush> brushes, bool isGradient)
  {
    double pixelWidth = isGradient ? 10.0 : 200.0;
    int num = 50;
    DrawingVisual drawingVisual = new DrawingVisual();
    DrawingContext drawingContext = drawingVisual.RenderOpen();
    Rect rectangle = new Rect(0.0, 0.0, 1.0, (double) num);
    for (int index1 = 0; (double) index1 < pixelWidth; ++index1)
    {
      int index2 = (int) ((double) index1 / pixelWidth * (double) brushes.Count);
      Brush brush = brushes[index2];
      rectangle.X = (double) index1;
      drawingContext.DrawRectangle(brush, (Pen) null, rectangle);
    }
    drawingContext.Close();
    RenderTargetBitmap image = new RenderTargetBitmap((int) pixelWidth, num, 0.0, 0.0, PixelFormats.Default);
    image.Render((Visual) drawingVisual);
    ImageBrush imageBrush = new ImageBrush((ImageSource) image);
    imageBrush.ViewportUnits = BrushMappingMode.Absolute;
    return imageBrush;
  }

  internal static ImageBrush DrawContourLine(int Count)
  {
    double pixelWidth = 300.0;
    int num1 = 512 /*0x0200*/;
    DrawingVisual drawingVisual = new DrawingVisual();
    DrawingContext drawingContext = drawingVisual.RenderOpen();
    Pen pen = new Pen((Brush) new SolidColorBrush(Colors.Black), 1.0);
    int num2 = 0;
    for (int x = 0; (double) x < pixelWidth; ++x)
    {
      if ((int) ((double) x / pixelWidth * (double) Count) == num2)
      {
        drawingContext.DrawLine(pen, new Point((double) x, 0.0), new Point((double) x, (double) num1));
        ++num2;
      }
    }
    drawingContext.Close();
    RenderTargetBitmap image = new RenderTargetBitmap((int) pixelWidth, num1, 0.0, 0.0, PixelFormats.Default);
    image.Render((Visual) drawingVisual);
    ImageBrush imageBrush = new ImageBrush((ImageSource) image);
    imageBrush.ViewportUnits = BrushMappingMode.Absolute;
    return imageBrush;
  }

  internal static MeshGeometry3D MakeWireframe(MeshGeometry3D mesh, double thickness)
  {
    Dictionary<int, int> alreadyDrawn = new Dictionary<int, int>();
    MeshGeometry3D wireframe = new MeshGeometry3D();
    for (int index = 0; index < mesh.TriangleIndices.Count; index += 6)
    {
      int triangleIndex1 = mesh.TriangleIndices[index];
      int triangleIndex2 = mesh.TriangleIndices[index + 1];
      int triangleIndex3 = mesh.TriangleIndices[index + 2];
      int triangleIndex4 = mesh.TriangleIndices[index + 5];
      MeshGenerator.AddTriangleSegment(mesh, wireframe, alreadyDrawn, triangleIndex1, triangleIndex2, thickness);
      MeshGenerator.AddTriangleSegment(mesh, wireframe, alreadyDrawn, triangleIndex2, triangleIndex3, thickness);
      MeshGenerator.AddTriangleSegment(mesh, wireframe, alreadyDrawn, triangleIndex3, triangleIndex4, thickness);
      MeshGenerator.AddTriangleSegment(mesh, wireframe, alreadyDrawn, triangleIndex4, triangleIndex1, thickness);
    }
    return wireframe;
  }

  private static void AddTriangleSegment(
    MeshGeometry3D mesh,
    MeshGeometry3D wireframe,
    Dictionary<int, int> alreadyDrawn,
    int index1,
    int index2,
    double thickness)
  {
    if (index1 > index2)
    {
      int num = index1;
      index1 = index2;
      index2 = num;
    }
    int key = index1 * mesh.Positions.Count + index2;
    if (alreadyDrawn.ContainsKey(key))
      return;
    alreadyDrawn.Add(key, key);
    Point3D position1 = mesh.Positions[index1];
    Point3D position2 = mesh.Positions[index2];
    System.Windows.Media.Media3D.Vector3D up = new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0);
    (position2 - position1).Normalize();
    MeshGenerator.AddSegment(wireframe, position1, position2, up, thickness);
  }

  internal static void AddSegment(
    MeshGeometry3D mesh,
    Point3D point1,
    Point3D point2,
    System.Windows.Media.Media3D.Vector3D up,
    double thickness)
  {
    System.Windows.Media.Media3D.Vector3D vector1 = point2 - point1;
    System.Windows.Media.Media3D.Vector3D vector2 = MeshGenerator.CalculateVector(up, thickness);
    System.Windows.Media.Media3D.Vector3D vector3 = MeshGenerator.CalculateVector(System.Windows.Media.Media3D.Vector3D.CrossProduct(vector1, vector2), thickness);
    Point3D point1_1 = point1 + vector2 + vector3;
    Point3D point3D1 = point1 - vector2 + vector3;
    Point3D point3D2 = point1 + vector2 - vector3;
    Point3D point3D3 = point1 - vector2 - vector3;
    Point3D point3D4 = point2 + vector2 + vector3;
    Point3D point3D5 = point2 - vector2 + vector3;
    Point3D point3D6 = point2 + vector2 - vector3;
    Point3D point3D7 = point2 - vector2 - vector3;
    MeshGenerator.AddMesh(mesh, point1_1, point3D1, point3D5);
    MeshGenerator.AddMesh(mesh, point1_1, point3D5, point3D4);
    MeshGenerator.AddMesh(mesh, point1_1, point3D4, point3D6);
    MeshGenerator.AddMesh(mesh, point1_1, point3D6, point3D2);
    MeshGenerator.AddMesh(mesh, point3D2, point3D6, point3D7);
    MeshGenerator.AddMesh(mesh, point3D2, point3D7, point3D3);
    MeshGenerator.AddMesh(mesh, point3D3, point3D7, point3D5);
    MeshGenerator.AddMesh(mesh, point3D3, point3D5, point3D1);
    MeshGenerator.AddMesh(mesh, point1_1, point3D2, point3D3);
    MeshGenerator.AddMesh(mesh, point1_1, point3D3, point3D1);
    MeshGenerator.AddMesh(mesh, point3D4, point3D5, point3D7);
    MeshGenerator.AddMesh(mesh, point3D4, point3D7, point3D6);
  }

  private static void AddMesh(MeshGeometry3D mesh, Point3D point1, Point3D point2, Point3D point3)
  {
    int count = mesh.Positions.Count;
    mesh.Positions.Add(point1);
    mesh.Positions.Add(point2);
    mesh.Positions.Add(point3);
    Int32Collection triangleIndices1 = mesh.TriangleIndices;
    int num1 = count;
    int num2 = num1 + 1;
    triangleIndices1.Add(num1);
    Int32Collection triangleIndices2 = mesh.TriangleIndices;
    int num3 = num2;
    int num4 = num3 + 1;
    triangleIndices2.Add(num3);
    mesh.TriangleIndices.Add(num4);
  }

  internal static double GetScaleValue(double value, double min, double max)
  {
    return (value - min) / (max - min);
  }

  internal static System.Windows.Media.Media3D.Vector3D CalculateVector(
    System.Windows.Media.Media3D.Vector3D vector,
    double length)
  {
    double num = length / vector.Length;
    return new System.Windows.Media.Media3D.Vector3D(vector.X * num, vector.Y * num, vector.Z * num);
  }

  internal static double GetMin(string value, Point3D[,] point3Ds)
  {
    if (point3Ds == null || point3Ds.Length == 0)
      return double.NaN;
    double val1 = double.MaxValue;
    for (int index1 = 0; index1 < point3Ds.GetLength(0); ++index1)
    {
      for (int index2 = 0; index2 < point3Ds.GetLength(1); ++index2)
      {
        switch (value)
        {
          case "X":
            val1 = Math.Min(val1, point3Ds[index1, index2].X);
            break;
          case "Y":
            val1 = Math.Min(val1, point3Ds[index1, index2].Y);
            break;
          default:
            val1 = Math.Min(val1, point3Ds[index1, index2].Z);
            break;
        }
      }
    }
    return val1;
  }

  internal static double GetMax(string value, Point3D[,] point3Ds)
  {
    if (point3Ds == null || point3Ds.Length == 0)
      return double.NaN;
    double val1 = double.MinValue;
    for (int index1 = 0; index1 < point3Ds.GetLength(0); ++index1)
    {
      for (int index2 = 0; index2 < point3Ds.GetLength(1); ++index2)
      {
        switch (value)
        {
          case "X":
            val1 = Math.Max(val1, point3Ds[index1, index2].X);
            break;
          case "Y":
            val1 = Math.Max(val1, point3Ds[index1, index2].Y);
            break;
          default:
            val1 = Math.Max(val1, point3Ds[index1, index2].Z);
            break;
        }
      }
    }
    return val1;
  }

  internal static System.Windows.Media.Media3D.Matrix3D GetViewportTransform(Size size)
  {
    double num = size.Width / 2.0;
    double offsetY = size.Height / 2.0;
    return new System.Windows.Media.Media3D.Matrix3D(num, 0.0, 0.0, 0.0, 0.0, -offsetY, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, num, offsetY, 0.0, 1.0);
  }

  internal static System.Windows.Media.Media3D.Matrix3D GetDirectionMatrix()
  {
    return new System.Windows.Media.Media3D.Matrix3D(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, -5.0, 1.0);
  }

  internal static System.Windows.Media.Media3D.Matrix3D GetOrthographicCameraMatrix(
    OrthographicCamera camera,
    double ratio)
  {
    double width = camera.Width;
    double num = width / ratio;
    double nearPlaneDistance = camera.NearPlaneDistance;
    double farPlaneDistance = camera.FarPlaneDistance;
    double m33 = 1.0 / (nearPlaneDistance - farPlaneDistance);
    double offsetZ = nearPlaneDistance * m33;
    return new System.Windows.Media.Media3D.Matrix3D(2.0 / width, 0.0, 0.0, 0.0, 0.0, 2.0 / num, 0.0, 0.0, 0.0, 0.0, m33, 0.0, 0.0, 0.0, offsetZ, 1.0);
  }

  internal static System.Windows.Media.Media3D.Matrix3D GetPerspectiveCameraMatrix(
    PerspectiveCamera camera,
    double ratio)
  {
    double num = camera.FieldOfView * (Math.PI / 180.0);
    double nearPlaneDistance = camera.NearPlaneDistance;
    double farPlaneDistance = camera.FarPlaneDistance;
    double m11 = 1.0 / Math.Tan(num / 2.0);
    double m22 = ratio * m11;
    double m33 = double.IsPositiveInfinity(farPlaneDistance) ? -1.0 : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
    double offsetZ = nearPlaneDistance * m33;
    return new System.Windows.Media.Media3D.Matrix3D(m11, 0.0, 0.0, 0.0, 0.0, m22, 0.0, 0.0, 0.0, 0.0, m33, -1.0, 0.0, 0.0, offsetZ, 0.0);
  }

  internal static List<Brush> GetBrushRange(int brushCount, List<Brush> brush)
  {
    int brushCount1 = brushCount - 10;
    if (brushCount1 > 0)
    {
      brush.AddRange((IEnumerable<Brush>) brush.GetRange(0, 10));
      MeshGenerator.GetBrushRange(brushCount1, brush);
    }
    else
      brush.AddRange((IEnumerable<Brush>) brush.GetRange(0, brushCount));
    return brush;
  }
}
