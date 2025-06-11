// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ColorScale
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public class ColorScale
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
  private Syncfusion.XlsIO.Implementation.ColorScaleImpl m_colorScale;

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

  public IColorScale ColorScaleImpl => (IColorScale) this.m_colorScale;

  public ColorScale()
  {
    this.m_arrCFInterp = new List<CFInterpolationCurve>();
    this.m_arrCFGradient = new List<CFGradientItem>();
    this.m_colorScale = new Syncfusion.XlsIO.Implementation.ColorScaleImpl();
  }

  private void CopyColorScale()
  {
    this.m_colorScale.Criteria.Clear();
    for (int index = 0; index < this.ListCFInterpolationCurve.Count; ++index)
    {
      CFInterpolationCurve interpolationCurve = this.ListCFInterpolationCurve[index];
      ColorConditionValue colorConditionValue = new ColorConditionValue();
      colorConditionValue.Type = interpolationCurve.CFVO.CFVOType;
      colorConditionValue.Value = interpolationCurve.CFVO.Value;
      colorConditionValue.RefPtg = interpolationCurve.CFVO.RefPtg;
      this.m_colorScale.Criteria.Add((IColorConditionValue) colorConditionValue);
    }
    for (int index = 0; index < this.ListCFGradientItem.Count; ++index)
    {
      CFGradientItem cfGradientItem = this.ListCFGradientItem[index];
      this.m_colorScale.Criteria[index].FormatColorRGB = this.ConvertRGBAToARGB(this.UIntToColor(cfGradientItem.ColorValue));
    }
  }

  public int ParseColorScale(DataProvider provider, int iOffset, ExcelVersion version)
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

  public int SerializeColorScale(
    DataProvider provider,
    int iOffset,
    ExcelVersion version,
    IColorScale m_iColorScale)
  {
    bool isParsed = true;
    if (this.ListCFInterpolationCurve.Count == 0)
    {
      isParsed = false;
      this.UpdateColorScaleColor(m_iColorScale);
    }
    provider.WriteUInt16(iOffset, this.m_undefined);
    iOffset += 2;
    provider.WriteByte(iOffset, (byte) 0);
    ++iOffset;
    provider.WriteByte(iOffset, (byte) this.ListCFInterpolationCurve.Count);
    ++iOffset;
    provider.WriteByte(iOffset, (byte) this.ListCFGradientItem.Count);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_clampAndBackground);
    ++iOffset;
    int position1 = 1;
    foreach (CFInterpolationCurve interpolationCurve in this.ListCFInterpolationCurve)
    {
      double numValue = this.CalculateNumValue(position1);
      iOffset = interpolationCurve.SerializeCFGradientInterp(provider, iOffset, version, numValue, isParsed);
      ++position1;
    }
    int position2 = 1;
    foreach (CFGradientItem cfGradientItem in this.ListCFGradientItem)
    {
      double numValue = this.CalculateNumValue(position2);
      iOffset = cfGradientItem.SerializeCFGradient(provider, iOffset, version, numValue, isParsed);
      ++position2;
    }
    return iOffset;
  }

  public int GetStoreSize(ExcelVersion version)
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

  public void UpdateColorScaleColor(IColorScale m_colorScale)
  {
    if (m_colorScale == null)
      return;
    this.ListCFInterpolationCurve.Clear();
    this.ListCFGradientItem.Clear();
    foreach (IColorConditionValue criterion in (IEnumerable<IColorConditionValue>) m_colorScale.Criteria)
    {
      this.ListCFInterpolationCurve.Add(new CFInterpolationCurve()
      {
        CFVO = {
          CFVOType = criterion.Type,
          Value = criterion.Value,
          RefPtg = (criterion as ConditionValue).RefPtg
        }
      });
      CFGradientItem cfGradientItem = new CFGradientItem();
      cfGradientItem.ColorType = ColorType.RGB;
      Color rgba = this.ConvertARGBToRGBA(criterion.FormatColorRGB);
      cfGradientItem.ColorValue = this.ColorToUInt(rgba);
      this.ListCFGradientItem.Add(cfGradientItem);
    }
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
    this.m_colorScale = (Syncfusion.XlsIO.Implementation.ColorScaleImpl) null;
    this.m_arrCFInterp = (List<CFInterpolationCurve>) null;
    this.m_arrCFGradient = (List<CFGradientItem>) null;
  }
}
