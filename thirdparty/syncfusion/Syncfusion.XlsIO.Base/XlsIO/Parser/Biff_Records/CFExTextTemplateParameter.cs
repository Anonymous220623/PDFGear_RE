// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.CFExTextTemplateParameter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public class CFExTextTemplateParameter
{
  private ushort m_textRuleType;

  public CFTextRuleType TextRuleType
  {
    get => (CFTextRuleType) this.m_textRuleType;
    set => this.m_textRuleType = (ushort) (byte) value;
  }

  public void ParseTextTemplateParameter(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_textRuleType = provider.ReadUInt16(iOffset);
    iOffset += 2;
    provider.ReadInt64(iOffset);
    iOffset += 14;
  }

  public void SerializeTextTemplateParameter(
    DataProvider provider,
    int iOffset,
    ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_textRuleType);
    iOffset += 2;
    provider.WriteInt64(iOffset, 0L);
    iOffset += 14;
  }
}
