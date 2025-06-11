// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.DataBar
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public class DataBar
{
  private const ushort DEF_MINIMUM_SIZE = 22;
  private ushort m_undefined;
  private bool m_isRightToLeft;
  private bool m_isShowValue = true;
  private byte m_minDatabarLen;
  private byte m_MaxDatabarLen;
  private uint m_colorType;
  private uint m_colorValue;
  private long m_tintShade;
  private CFVO m_cfvoMin;
  private CFVO m_cfvoMax;
  private Syncfusion.XlsIO.Implementation.DataBarImpl m_dataBar;

  public ColorType ColorType
  {
    get => (ColorType) this.m_colorType;
    set => this.m_colorType = (uint) (byte) value;
  }

  public uint ColorValue
  {
    get => this.m_colorValue;
    set => this.m_colorValue = value;
  }

  public long TintShade
  {
    get => this.m_tintShade;
    set => this.m_tintShade = value;
  }

  public CFVO MinCFVO
  {
    get => this.m_cfvoMin;
    set => this.m_cfvoMin = value;
  }

  public CFVO MaxCFVO
  {
    get => this.m_cfvoMax;
    set => this.m_cfvoMax = value;
  }

  public IDataBar DataBarImpl => (IDataBar) this.m_dataBar;

  public DataBar()
  {
    this.m_cfvoMin = new CFVO();
    this.m_cfvoMax = new CFVO();
    this.m_dataBar = new Syncfusion.XlsIO.Implementation.DataBarImpl();
  }

  private void CopyDataBar()
  {
    this.m_dataBar.MinPoint.Type = this.m_cfvoMin.CFVOType;
    this.m_dataBar.MinPoint.Value = this.m_cfvoMin.Value;
    this.m_dataBar.MaxPoint.Type = this.m_cfvoMax.CFVOType;
    this.m_dataBar.MaxPoint.Value = this.m_cfvoMax.Value;
    this.m_dataBar.BarColor = this.ConvertRGBAToARGB(this.UIntToColor(this.m_colorValue));
    this.m_dataBar.ShowValue = !this.m_isShowValue;
    this.m_dataBar.PercentMin = (int) this.m_minDatabarLen;
    this.m_dataBar.PercentMax = (int) this.m_MaxDatabarLen;
  }

  private Color ConvertRGBAToARGB(Color colorValue)
  {
    colorValue = Color.FromArgb((int) colorValue.A, (int) colorValue.B, (int) colorValue.G, (int) colorValue.R);
    return colorValue;
  }

  public int ParseDataBar(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_undefined = provider.ReadUInt16(iOffset);
    iOffset += 2;
    int num = (int) provider.ReadByte(iOffset);
    ++iOffset;
    this.m_isRightToLeft = provider.ReadBit(iOffset, 0);
    this.m_isShowValue = provider.ReadBit(iOffset, 1);
    ++iOffset;
    this.m_minDatabarLen = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_MaxDatabarLen = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_colorType = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_colorValue = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_tintShade = provider.ReadInt64(iOffset);
    iOffset += 8;
    this.m_cfvoMin = new CFVO();
    iOffset = this.m_cfvoMin.ParseCFVO(provider, iOffset, version);
    this.m_cfvoMax = new CFVO();
    iOffset = this.m_cfvoMax.ParseCFVO(provider, iOffset, version);
    this.CopyDataBar();
    return iOffset;
  }

  public int SerializeDataBar(
    DataProvider provider,
    int iOffset,
    ExcelVersion version,
    IDataBar m_iDatabar)
  {
    if (m_iDatabar != null)
    {
      this.MinCFVO = new CFVO();
      this.MinCFVO.CFVOType = m_iDatabar.MinPoint.Type;
      this.MinCFVO.Value = m_iDatabar.MinPoint.Value;
      this.MaxCFVO = new CFVO();
      this.MaxCFVO.CFVOType = m_iDatabar.MaxPoint.Type;
      this.MaxCFVO.Value = m_iDatabar.MaxPoint.Value;
      this.m_minDatabarLen = (byte) m_iDatabar.PercentMin;
      this.m_MaxDatabarLen = (byte) m_iDatabar.PercentMax;
      this.m_colorType = 2U;
      this.m_colorValue = this.ColorToUInt(this.ConvertARGBToRGBA(m_iDatabar.BarColor));
      this.m_isShowValue = m_iDatabar.ShowValue;
    }
    provider.WriteUInt16(iOffset, this.m_undefined);
    iOffset += 2;
    provider.WriteByte(iOffset, (byte) 0);
    ++iOffset;
    byte num = 0;
    if (!this.m_isShowValue && this.m_isRightToLeft)
      num = (byte) 3;
    if (!this.m_isShowValue && !this.m_isRightToLeft)
      num = (byte) 2;
    if (this.m_isShowValue && this.m_isRightToLeft)
      num = (byte) 1;
    provider.WriteByte(iOffset, num);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_minDatabarLen);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_MaxDatabarLen);
    ++iOffset;
    provider.WriteUInt32(iOffset, this.m_colorType);
    iOffset += 4;
    provider.WriteUInt32(iOffset, this.m_colorValue);
    iOffset += 4;
    provider.WriteInt64(iOffset, this.m_tintShade);
    iOffset += 8;
    iOffset = this.m_cfvoMin.SerializeCFVO(provider, iOffset, version);
    iOffset = this.m_cfvoMax.SerializeCFVO(provider, iOffset, version);
    return iOffset;
  }

  public int GetStoreSize(ExcelVersion version)
  {
    return 22 + this.m_cfvoMin.GetStoreSize(version) + this.m_cfvoMax.GetStoreSize(version);
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

  internal void ClearAll()
  {
    this.m_cfvoMax.ClearAll();
    this.m_cfvoMin.ClearAll();
    this.m_dataBar = (Syncfusion.XlsIO.Implementation.DataBarImpl) null;
    this.m_cfvoMin = (CFVO) null;
    this.m_cfvoMax = (CFVO) null;
  }
}
