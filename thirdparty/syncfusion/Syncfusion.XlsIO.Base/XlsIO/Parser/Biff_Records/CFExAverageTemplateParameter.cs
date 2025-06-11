// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.CFExAverageTemplateParameter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public class CFExAverageTemplateParameter
{
  private ushort m_numberOfStandardDeviation;
  private AboveBelowAverageImpl m_aboveBelowAverage;

  public ushort NumberOfDeviations
  {
    get => this.m_numberOfStandardDeviation;
    set => this.m_numberOfStandardDeviation = value;
  }

  internal IAboveBelowAverage AboveBelowAverage
  {
    get => (IAboveBelowAverage) this.m_aboveBelowAverage;
    set => this.m_aboveBelowAverage = value as AboveBelowAverageImpl;
  }

  internal void CopyAverageTemplaterParameter(ConditionalFormatTemplate template)
  {
    this.m_aboveBelowAverage = new AboveBelowAverageImpl();
    switch (template)
    {
      case ConditionalFormatTemplate.AboveAverage:
        if (this.m_numberOfStandardDeviation > (ushort) 0 && this.m_numberOfStandardDeviation < (ushort) 4)
        {
          this.m_aboveBelowAverage.AverageType = ExcelCFAverageType.AboveStdDev;
          this.m_aboveBelowAverage.StdDevValue = (int) this.m_numberOfStandardDeviation;
          break;
        }
        this.m_aboveBelowAverage.AverageType = ExcelCFAverageType.Above;
        break;
      case ConditionalFormatTemplate.BelowAverage:
        if (this.m_numberOfStandardDeviation > (ushort) 0 && this.m_numberOfStandardDeviation < (ushort) 4)
        {
          this.m_aboveBelowAverage.AverageType = ExcelCFAverageType.BelowStdDev;
          this.m_aboveBelowAverage.StdDevValue = (int) this.m_numberOfStandardDeviation;
          break;
        }
        this.m_aboveBelowAverage.AverageType = ExcelCFAverageType.Below;
        break;
      case ConditionalFormatTemplate.AboveOrEqualToAverage:
        this.m_aboveBelowAverage.AverageType = ExcelCFAverageType.EqualOrAbove;
        break;
      case ConditionalFormatTemplate.BelowOrEqualToAverage:
        this.m_aboveBelowAverage.AverageType = ExcelCFAverageType.EqualOrBelow;
        break;
    }
  }

  internal object Clone()
  {
    CFExAverageTemplateParameter templateParameter = (CFExAverageTemplateParameter) this.MemberwiseClone();
    if (this.m_aboveBelowAverage != (AboveBelowAverageImpl) null)
      templateParameter.m_aboveBelowAverage = this.m_aboveBelowAverage.Clone();
    return (object) templateParameter;
  }

  internal new int GetHashCode() => this.m_numberOfStandardDeviation.GetHashCode();

  internal new bool Equals(object obj)
  {
    return obj is CFExAverageTemplateParameter templateParameter && (int) this.m_numberOfStandardDeviation == (int) templateParameter.m_numberOfStandardDeviation && this.m_aboveBelowAverage == templateParameter.m_aboveBelowAverage;
  }

  public void ParseAverageTemplateParameter(
    DataProvider provider,
    int iOffset,
    ExcelVersion version)
  {
    this.m_numberOfStandardDeviation = provider.ReadUInt16(iOffset);
    iOffset += 2;
    provider.ReadInt64(iOffset);
    iOffset += 14;
  }

  public void SerializeAverageTemplateParameter(
    DataProvider provider,
    int iOffset,
    ExcelVersion version)
  {
    if (this.m_aboveBelowAverage != (AboveBelowAverageImpl) null && this.m_aboveBelowAverage.AverageType.ToString().Contains("StdDev"))
      this.m_numberOfStandardDeviation = (ushort) this.m_aboveBelowAverage.StdDevValue;
    provider.WriteUInt16(iOffset, this.m_numberOfStandardDeviation);
    iOffset += 2;
    provider.WriteInt64(iOffset, 0L);
    iOffset += 14;
  }
}
