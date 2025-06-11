// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.CFExDateTemplateParameter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public class CFExDateTemplateParameter
{
  private ushort m_dateComparisonType;

  public ushort DateComparisonOperator
  {
    get => this.m_dateComparisonType;
    set => this.m_dateComparisonType = value;
  }

  public void ParseDateTemplateParameter(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_dateComparisonType = provider.ReadUInt16(iOffset);
    iOffset += 2;
    provider.ReadInt64(iOffset);
    iOffset += 14;
  }

  public void SerializeDateTemplateParameter(
    DataProvider provider,
    int iOffset,
    ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_dateComparisonType);
    iOffset += 2;
    provider.WriteInt64(iOffset, 0L);
    iOffset += 14;
  }
}
