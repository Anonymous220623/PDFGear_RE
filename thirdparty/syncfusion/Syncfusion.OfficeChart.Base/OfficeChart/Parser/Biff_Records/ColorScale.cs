// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ColorScale
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal class ColorScale
{
  private const ushort DEF_MINIMUM_SIZE = 6;
  private ushort m_undefined;
  private byte m_interpCurve;
  private byte m_gradient;
  private bool m_clamp = true;
  private bool m_background = true;
  private byte m_clampAndBackground = 3;
  private List<CFInterpolationCurve> m_arrCFInterp = new List<CFInterpolationCurve>();
  private List<CFGradientItem> m_arrCFGradient = new List<CFGradientItem>();

  public List<CFInterpolationCurve> ListCFInterpolationCurve
  {
    get => this.m_arrCFInterp;
    set => this.m_arrCFInterp = value;
  }

  public List<CFGradientItem> ListCFGradientItem
  {
    get => this.m_arrCFGradient;
    set => this.m_arrCFGradient = value;
  }

  public ushort DefaultRecordSize => 6;

  public ColorScale()
  {
    this.m_arrCFInterp = new List<CFInterpolationCurve>();
    this.m_arrCFGradient = new List<CFGradientItem>();
  }

  private void CopyColorScale()
  {
  }

  public int ParseColorScale(DataProvider provider, int iOffset, OfficeVersion version)
  {
    this.m_undefined = provider.ReadUInt16(iOffset);
    iOffset += 2;
    int num = (int) provider.ReadByte(iOffset);
    ++iOffset;
    this.m_interpCurve = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_gradient = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_clampAndBackground = provider.ReadByte(iOffset);
    ++iOffset;
    for (int index = 0; index < (int) this.m_interpCurve; ++index)
    {
      CFInterpolationCurve interpolationCurve = new CFInterpolationCurve();
      iOffset = interpolationCurve.ParseCFGradientInterp(provider, iOffset, version);
      this.m_arrCFInterp.Add(interpolationCurve);
    }
    for (int index = 0; index < (int) this.m_gradient; ++index)
    {
      CFGradientItem cfGradientItem = new CFGradientItem();
      iOffset = cfGradientItem.ParseCFGradient(provider, iOffset, version);
      this.m_arrCFGradient.Add(cfGradientItem);
    }
    this.CopyColorScale();
    return iOffset;
  }

  public int GetStoreSize(OfficeVersion version)
  {
    int num1 = 0;
    foreach (CFInterpolationCurve interpolationCurve in this.ListCFInterpolationCurve)
      num1 += interpolationCurve.GetStoreSize(version);
    int num2 = 0;
    foreach (CFGradientItem cfGradientItem in this.ListCFGradientItem)
      num2 += cfGradientItem.GetStoreSize(version);
    return 6 + num1 + num2;
  }

  private double CalculateNumValue(int position)
  {
    double numValue = 0.0;
    if (this.ListCFInterpolationCurve.Count == 3)
    {
      if (position == 1)
        numValue = 0.0;
      if (position == 2)
        numValue = 0.5;
      if (position == 3)
        numValue = 1.0;
    }
    if (this.ListCFInterpolationCurve.Count == 2)
    {
      if (position == 1)
        numValue = 0.0;
      if (position == 2)
        numValue = 1.0;
    }
    return numValue;
  }

  private uint ColorToUInt(Color color)
  {
    return (uint) ((int) color.A << 24 | (int) color.R << 16 /*0x10*/ | (int) color.G << 8) | (uint) color.B;
  }

  private Color UIntToColor(uint color)
  {
    return Color.FromArgb((int) (byte) (color >> 24), (int) (byte) (color >> 16 /*0x10*/), (int) (byte) (color >> 8), (int) (byte) color);
  }

  private Color ConvertARGBToRGBA(Color colorValue)
  {
    byte b = colorValue.B;
    byte g = colorValue.G;
    byte r = colorValue.R;
    colorValue = Color.FromArgb((int) colorValue.A, (int) b, (int) g, (int) r);
    return colorValue;
  }

  private Color ConvertRGBAToARGB(Color colorValue)
  {
    colorValue = Color.FromArgb((int) colorValue.A, (int) colorValue.B, (int) colorValue.G, (int) colorValue.R);
    return colorValue;
  }

  internal void ClearAll()
  {
    this.m_arrCFInterp.Clear();
    this.m_arrCFGradient.Clear();
    this.m_arrCFInterp = (List<CFInterpolationCurve>) null;
    this.m_arrCFGradient = (List<CFGradientItem>) null;
  }
}
