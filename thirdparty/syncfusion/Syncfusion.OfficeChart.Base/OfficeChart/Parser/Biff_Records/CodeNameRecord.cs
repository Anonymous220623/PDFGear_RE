// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.CodeNameRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Exceptions;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.CodeName)]
internal class CodeNameRecord : BiffRecordRawWithArray
{
  private string m_strName;

  public string CodeName
  {
    get => this.m_strName;
    set => this.m_strName = value;
  }

  public CodeNameRecord()
  {
  }

  public CodeNameRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public CodeNameRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure()
  {
    if (this.Length <= 0)
      return;
    this.AutoExtractFields();
    int iBytesInString;
    this.m_strName = this.GetString(2, (int) this.GetUInt16(0), out iBytesInString);
    if (3 + iBytesInString != this.Length)
      throw new WrongBiffRecordDataException("Wrong string or data length.");
  }

  public override void InfillInternalData(OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(OfficeVersion.Excel97to2003);
    if (this.m_iLength <= 0)
      return;
    this.m_data = new byte[this.m_iLength];
    this.SetString16BitLen(0, this.m_strName);
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    return (this.m_strName != null ? this.m_strName.Length : 0) == 0 ? 0 : 3 + this.m_strName.Length * 2;
  }
}
