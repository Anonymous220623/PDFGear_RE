// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.CFInterpolationCurve
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public class CFInterpolationCurve
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

  public int ParseCFGradientInterp(DataProvider provider, int iOffset, ExcelVersion version)
  {
    iOffset = this.m_cfvo.ParseCFVO(provider, iOffset, version);
    this.m_numDomain = provider.ReadDouble(iOffset);
    iOffset += 8;
    return iOffset;
  }

  public int SerializeCFGradientInterp(
    DataProvider provider,
    int iOffset,
    ExcelVersion version,
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

  public int GetStoreSize(ExcelVersion version) => 8 + this.m_cfvo.GetStoreSize(version);

  internal void ClearAll()
  {
    this.m_cfvo.ClearAll();
    this.m_cfvo = (CFVO) null;
  }
}
