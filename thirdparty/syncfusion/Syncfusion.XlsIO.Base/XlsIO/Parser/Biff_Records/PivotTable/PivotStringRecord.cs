// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PivotStringRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[Biff(TBIFFRecord.PivotString)]
[CLSCompliant(false)]
public class PivotStringRecord : BiffRecordRaw, IValueHolder
{
  [BiffRecordPos(0, TFieldType.String16Bit)]
  private string m_strString;
  private bool m_b8Bit;

  public PivotStringRecord()
  {
  }

  public PivotStringRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PivotStringRecord(int iReserve)
    : base(iReserve)
  {
  }

  public string String
  {
    get => this.m_strString;
    set
    {
      this.m_strString = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.m_b8Bit = BiffRecordRawWithArray.IsAsciiString(value);
    }
  }

  public override bool NeedDataArray => true;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    int iFullLength;
    this.m_strString = provider.ReadString16Bit(iOffset, out iFullLength);
    if (this.m_strString.Length * 2 + 3 >= iFullLength)
      return;
    this.m_b8Bit = true;
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    if (this.m_b8Bit)
    {
      int num = iOffset;
      provider.WriteString16BitUpdateOffset(ref iOffset, this.m_strString, false);
      this.m_iLength = iOffset - num;
    }
    else
      this.m_iLength = provider.WriteString16Bit(iOffset, this.m_strString);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    return 3 + (this.m_b8Bit ? this.m_strString.Length : Encoding.Unicode.GetByteCount(this.m_strString));
  }

  object IValueHolder.Value
  {
    get => (object) this.String;
    set => this.String = (string) value;
  }
}
