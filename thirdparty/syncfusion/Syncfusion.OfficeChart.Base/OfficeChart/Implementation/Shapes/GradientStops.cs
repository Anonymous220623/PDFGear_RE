// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Shapes.GradientStops
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Shapes;

internal class GradientStops : List<GradientStopImpl>
{
  internal const int MaxPosition = 100000;
  private int m_iAngle;
  private GradientType m_gradientType;
  private Rectangle m_fillToRect;
  private Rectangle m_tileRect;

  public int Angle
  {
    get => this.m_iAngle;
    set => this.m_iAngle = value;
  }

  public GradientType GradientType
  {
    get => this.m_gradientType;
    set => this.m_gradientType = value;
  }

  public Rectangle FillToRect
  {
    get => this.m_fillToRect;
    set => this.m_fillToRect = value;
  }

  public Rectangle TileRect
  {
    get => this.m_tileRect;
    set => this.m_tileRect = value;
  }

  public bool IsDoubled
  {
    get
    {
      int count = this.Count;
      bool isDoubled = true;
      if (count <= 2)
      {
        isDoubled = false;
      }
      else
      {
        int index1 = 0;
        for (int index2 = count - 1; index1 <= index2; --index2)
        {
          GradientStopImpl gradientStopImpl1 = this[index1];
          GradientStopImpl gradientStopImpl2 = this[index2];
          if (gradientStopImpl1.ColorObject != gradientStopImpl2.ColorObject || gradientStopImpl1.Position != 100000 - gradientStopImpl2.Position)
          {
            isDoubled = false;
            break;
          }
          ++index1;
        }
      }
      return isDoubled;
    }
  }

  public GradientStops()
  {
  }

  public GradientStops(byte[] data) => this.Parse(data);

  public void Serialize(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    byte[] bytes1 = BitConverter.GetBytes(this.Count);
    stream.Write(bytes1, 0, bytes1.Length);
    byte[] bytes2 = BitConverter.GetBytes(this.m_iAngle);
    stream.Write(bytes2, 0, bytes2.Length);
    stream.WriteByte((byte) this.m_gradientType);
    byte[] bytes3 = BitConverter.GetBytes(this.m_fillToRect.Left);
    stream.Write(bytes3, 0, bytes3.Length);
    byte[] bytes4 = BitConverter.GetBytes(this.m_fillToRect.Top);
    stream.Write(bytes4, 0, bytes4.Length);
    byte[] bytes5 = BitConverter.GetBytes(this.m_fillToRect.Right);
    stream.Write(bytes5, 0, bytes5.Length);
    byte[] bytes6 = BitConverter.GetBytes(this.m_fillToRect.Bottom);
    stream.Write(bytes6, 0, bytes6.Length);
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this[index].Serialize(stream);
  }

  private void Parse(byte[] data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    int startIndex1 = 0;
    int int32_1 = BitConverter.ToInt32(data, startIndex1);
    int startIndex2 = startIndex1 + 4;
    this.m_iAngle = BitConverter.ToInt32(data, startIndex2);
    int index1 = startIndex2 + 4;
    this.m_gradientType = (GradientType) data[index1];
    int startIndex3 = index1 + 1;
    int int32_2 = BitConverter.ToInt32(data, startIndex3);
    int startIndex4 = startIndex3 + 4;
    int int32_3 = BitConverter.ToInt32(data, startIndex4);
    int startIndex5 = startIndex4 + 4;
    int int32_4 = BitConverter.ToInt32(data, startIndex5);
    int startIndex6 = startIndex5 + 4;
    int int32_5 = BitConverter.ToInt32(data, startIndex6);
    int offset = startIndex6 + 4;
    this.m_fillToRect = Rectangle.FromLTRB(int32_2, int32_3, int32_4, int32_5);
    for (int index2 = 0; index2 < int32_1; ++index2)
    {
      GradientStopImpl gradientStopImpl = new GradientStopImpl(data, offset);
      offset += 12;
      this.Add(gradientStopImpl);
    }
  }

  public void DoubleGradientStops()
  {
    int count = this.Count;
    if (count == 0)
      return;
    GradientStopImpl gradientStopImpl1 = this[count - 1];
    int position = gradientStopImpl1.Position;
    int num1 = position >> 1;
    gradientStopImpl1.Position = num1;
    if (position != 100000)
    {
      GradientStopImpl gradientStopImpl2 = gradientStopImpl1.Clone();
      gradientStopImpl2.Position = 100000 - num1;
      this.Add(gradientStopImpl2);
    }
    for (int index = count - 2; index >= 0; --index)
    {
      GradientStopImpl gradientStopImpl3 = this[index];
      int num2 = gradientStopImpl3.Position >> 1;
      gradientStopImpl3.Position = num2;
      GradientStopImpl gradientStopImpl4 = gradientStopImpl3.Clone();
      gradientStopImpl4.Position = 100000 - num2;
      this.Add(gradientStopImpl4);
    }
  }

  public void InvertGradientStops()
  {
    int count = this.Count;
    if (count == 0)
      return;
    for (int index = 0; index < count; ++index)
    {
      GradientStopImpl gradientStopImpl = this[index];
      int position = gradientStopImpl.Position;
      gradientStopImpl.Position = 100000 - position;
    }
  }

  public GradientStops ShrinkGradientStops()
  {
    GradientStops gradientStops = new GradientStops();
    gradientStops.m_iAngle = this.m_iAngle;
    gradientStops.m_gradientType = this.m_gradientType;
    gradientStops.m_fillToRect = this.m_fillToRect;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      GradientStopImpl gradientStopImpl1 = this[index];
      if (gradientStopImpl1.Position <= 50000)
      {
        GradientStopImpl gradientStopImpl2 = gradientStopImpl1.Clone();
        gradientStopImpl2.Position <<= 1;
        gradientStops.Add(gradientStopImpl2);
      }
      else
        break;
    }
    return gradientStops;
  }

  public GradientStops Clone()
  {
    GradientStops gradientStops = new GradientStops();
    gradientStops.m_iAngle = this.m_iAngle;
    gradientStops.m_gradientType = this.m_gradientType;
    gradientStops.m_fillToRect = this.m_fillToRect;
    gradientStops.m_tileRect = this.m_tileRect;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      gradientStops.Add(this[index].Clone());
    return gradientStops;
  }

  internal bool EqualColors(GradientStops gradientStops)
  {
    if (gradientStops == null)
      return false;
    bool flag = false;
    int count = this.Count;
    if (gradientStops.Count == count)
    {
      flag = true;
      for (int index = 0; index < count; ++index)
      {
        if (!this[index].EqualsWithoutTransparency(gradientStops[index]))
        {
          flag = false;
          break;
        }
      }
    }
    return flag;
  }

  internal void Dispose()
  {
    foreach (GradientStopImpl gradientStopImpl in (List<GradientStopImpl>) this)
      gradientStopImpl.Dispose();
  }
}
