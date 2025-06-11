// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.CFExAverageTemplateParameter
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal class CFExAverageTemplateParameter
{
  private ushort m_numberOfStandardDeviation;

  public ushort NumberOfDeviations
  {
    get => this.m_numberOfStandardDeviation;
    set => this.m_numberOfStandardDeviation = value;
  }

  public void ParseAverageTemplateParameter(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_numberOfStandardDeviation = provider.ReadUInt16(iOffset);
    iOffset += 2;
    provider.ReadInt64(iOffset);
    iOffset += 14;
  }

  public void SerializeAverageTemplateParameter(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_numberOfStandardDeviation);
    iOffset += 2;
    provider.WriteInt64(iOffset, 0L);
    iOffset += 14;
  }
}
