// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.CFGradientItem
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public class CFGradientItem
{
  private double m_numGradientRange;
  private uint m_colorType = 2;
  private uint m_colorValue;
  private long m_tintShade;

  public double NumGradientRange
  {
    get => this.m_numGradientRange;
    set => this.m_numGradientRange = value;
  }

  public ColorType ColorType
  {
    get => (ColorType) this.m_colorType;
    set => this.m_colorType = 2U;
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

  public int ParseCFGradient(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_numGradientRange = provider.ReadDouble(iOffset);
    iOffset += 8;
    this.m_colorType = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_colorValue = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_tintShade = provider.ReadInt64(iOffset);
    iOffset += 8;
    return iOffset;
  }

  public int SerializeCFGradient(
    DataProvider provider,
    int iOffset,
    ExcelVersion version,
    double numValue,
    bool isParsed)
  {
    if (!isParsed)
      provider.WriteDouble(iOffset, numValue);
    else
      provider.WriteDouble(iOffset, this.m_numGradientRange);
    iOffset += 8;
    provider.WriteUInt32(iOffset, this.m_colorType);
    iOffset += 4;
    provider.WriteUInt32(iOffset, this.m_colorValue);
    iOffset += 4;
    provider.WriteInt64(iOffset, this.m_tintShade);
    iOffset += 8;
    return iOffset;
  }

  public int GetStoreSize(ExcelVersion version) => 24;
}
