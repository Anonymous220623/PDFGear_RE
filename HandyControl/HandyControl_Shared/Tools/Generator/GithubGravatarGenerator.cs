// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.GithubGravatarGenerator
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Controls;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace HandyControl.Tools;

public class GithubGravatarGenerator : IGravatarGenerator
{
  private const int RenderDataMaxLength = 15;

  public object GetGravatar(string id)
  {
    string hashCode = this.GetHashCode(id);
    int[] renderData = this.GetRenderData(hashCode);
    Brush renderBrush = this.GetRenderBrush(hashCode);
    GeometryGroup geometryGroup = new GeometryGroup();
    int index1 = 0;
    for (int i = 0; i < 2; ++i)
    {
      int j = 0;
      while (j < 5)
      {
        AddRec(i, j, renderData[index1] == 0);
        ++j;
        ++index1;
      }
    }
    int j1 = 0;
    while (j1 < 5)
    {
      AddRec(2, j1, renderData[index1] == 0);
      ++j1;
      ++index1;
    }
    int index2 = index1 - 10;
    for (int i = 3; i < 5; ++i)
    {
      int j2 = 0;
      while (j2 < 5)
      {
        AddRec(i, j2, renderData[index2] == 0);
        ++j2;
        ++index2;
      }
      index2 -= 10;
    }
    Path target = new Path();
    target.Data = (Geometry) geometryGroup;
    target.Fill = renderBrush;
    target.VerticalAlignment = VerticalAlignment.Top;
    target.Stretch = Stretch.Uniform;
    RenderOptions.SetEdgeMode((DependencyObject) target, EdgeMode.Aliased);
    return (object) target;

    void AddRec(int i, int j, bool hidden = false)
    {
      RectangleGeometry rectangleGeometry = new RectangleGeometry(new Rect(new Point((double) i, (double) j), hidden ? new Size() : new Size(1.0, 1.0)));
      geometryGroup.Children.Add((Geometry) rectangleGeometry);
    }
  }

  private string GetHashCode(string id)
  {
    byte[] hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(id));
    StringBuilder stringBuilder = new StringBuilder();
    foreach (byte num in hash)
      stringBuilder.Append(num.ToString("X2"));
    return stringBuilder.ToString();
  }

  private int[] GetRenderData(string hashcode)
  {
    int[] renderData = new int[15];
    for (int index = 0; index < 15; ++index)
    {
      int num = (int) hashcode[index];
      renderData[index] = num % 2;
    }
    return renderData;
  }

  private Brush GetRenderBrush(string hashcode)
  {
    return (Brush) new SolidColorBrush(this.Hsl2Rgb((double) int.Parse(hashcode.Substring(hashcode.Length - 7), NumberStyles.HexNumber) / 268435455.0));
  }

  private Color Hsl2Rgb(double h, double s = 0.7, double b = 0.5)
  {
    h *= 6.0;
    double[] numArray1 = new double[6];
    double num1 = b;
    double num2 = s;
    double num3 = b < 0.5 ? b : 1.0 - b;
    double num4;
    s = num4 = num2 * num3;
    double num5;
    b = num5 = num1 + num4;
    numArray1[0] = num5;
    numArray1[1] = b - h % 1.0 * s * 2.0;
    numArray1[2] = (b -= (s *= 2.0));
    numArray1[3] = b;
    numArray1[4] = b + h % 1.0 * s;
    numArray1[5] = b + s;
    double[] numArray2 = numArray1;
    int num6 = (int) Math.Floor(h);
    return Color.FromRgb((byte) (numArray2[num6 % 6] * (double) byte.MaxValue), (byte) (numArray2[(num6 | 16 /*0x10*/) % 6] * (double) byte.MaxValue), (byte) (numArray2[(num6 | 8) % 6] * (double) byte.MaxValue));
  }
}
