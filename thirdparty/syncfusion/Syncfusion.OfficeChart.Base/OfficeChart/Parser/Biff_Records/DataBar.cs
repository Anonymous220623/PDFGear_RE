// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.DataBar
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal class DataBar
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

  public DataBar()
  {
    this.m_cfvoMin = new CFVO();
    this.m_cfvoMax = new CFVO();
  }

  private void CopyDataBar()
  {
  }

  private Color ConvertRGBAToARGB(Color colorValue)
  {
    colorValue = Color.FromArgb((int) colorValue.A, (int) colorValue.B, (int) colorValue.G, (int) colorValue.R);
    return colorValue;
  }

  public int ParseDataBar(DataProvider provider, int iOffset, OfficeVersion version)
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

  public int GetStoreSize(OfficeVersion version)
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
    this.m_cfvoMin = (CFVO) null;
    this.m_cfvoMax = (CFVO) null;
  }
}
