// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.CFInterpolationCurve
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal class CFInterpolationCurve
{
  private const ushort DEF_MINIMUM_SIZE = 8;
  private double m_numDomain;
  private CFVO m_cfvo;

  public double NumDomain
  {
    get => this.m_numDomain;
    set => this.m_numDomain = value;
  }

  public CFVO CFVO
  {
    get => this.m_cfvo;
    set => this.m_cfvo = value;
  }

  public CFInterpolationCurve() => this.m_cfvo = new CFVO();

  public int ParseCFGradientInterp(DataProvider provider, int iOffset, OfficeVersion version)
  {
    iOffset = this.m_cfvo.ParseCFVO(provider, iOffset, version);
    this.m_numDomain = provider.ReadDouble(iOffset);
    iOffset += 8;
    return iOffset;
  }

  public int SerializeCFGradientInterp(
    DataProvider provider,
    int iOffset,
    OfficeVersion version,
    double numValue,
    bool isParsed)
  {
    iOffset = this.m_cfvo.SerializeCFVO(provider, iOffset, version);
    if (!isParsed)
      provider.WriteDouble(iOffset, numValue);
    else
      provider.WriteDouble(iOffset, this.m_numDomain);
    iOffset += 8;
    return iOffset;
  }

  public int GetStoreSize(OfficeVersion version) => 8 + this.m_cfvo.GetStoreSize(version);

  internal void ClearAll()
  {
    this.m_cfvo.ClearAll();
    this.m_cfvo = (CFVO) null;
  }
}
