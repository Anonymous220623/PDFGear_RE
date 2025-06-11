// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.ParsedExpressionRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ParsedExpression)]
public class ParsedExpressionRecord : BiffRecordRaw
{
  private const int DEF_EXPRESSION_OFFSET = 4;
  [BiffRecordPos(0, 2)]
  private ushort m_usSize;
  [BiffRecordPos(2, 2)]
  private ushort m_usNameCount;
  private byte[] m_arrParsedExpression;

  public ParsedExpressionRecord()
  {
  }

  public ParsedExpressionRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ParsedExpressionRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort Size => this.m_usSize;

  public ushort NameCount
  {
    get => this.m_usNameCount;
    set => this.m_usNameCount = value;
  }

  public byte[] ParsedExpression
  {
    get => this.m_arrParsedExpression;
    set
    {
      this.m_arrParsedExpression = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.m_usSize = (ushort) this.m_arrParsedExpression.Length;
    }
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usSize = provider.ReadUInt16(iOffset);
    this.m_usNameCount = provider.ReadUInt16(iOffset + 2);
    this.m_arrParsedExpression = new byte[(int) this.m_usSize];
    provider.ReadArray(iOffset + 4, this.m_arrParsedExpression);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usSize);
    provider.WriteUInt16(iOffset + 2, this.m_usNameCount);
    provider.WriteBytes(iOffset + 4, this.m_arrParsedExpression);
    this.m_iLength += this.m_arrParsedExpression.Length + 4;
  }

  public override int GetStoreSize(ExcelVersion version) => this.m_arrParsedExpression.Length + 4;
}
