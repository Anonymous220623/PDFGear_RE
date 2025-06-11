// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.CFExTextTemplateParameter
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal class CFExTextTemplateParameter
{
  private ushort m_textRuleType;

  public CFTextRuleType TextRuleType
  {
    get => (CFTextRuleType) this.m_textRuleType;
    set => this.m_textRuleType = (ushort) (byte) value;
  }

  public void ParseTextTemplateParameter(DataProvider provider, int iOffset, OfficeVersion version)
  {
    this.m_textRuleType = provider.ReadUInt16(iOffset);
    iOffset += 2;
    provider.ReadInt64(iOffset);
    iOffset += 14;
  }

  public void SerializeTextTemplateParameter(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_textRuleType);
    iOffset += 2;
    provider.WriteInt64(iOffset, 0L);
    iOffset += 14;
  }
}
