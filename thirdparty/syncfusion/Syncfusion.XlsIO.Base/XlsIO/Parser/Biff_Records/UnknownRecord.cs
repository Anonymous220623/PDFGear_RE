// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.UnknownRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.Unknown)]
[CLSCompliant(false)]
public class UnknownRecord : BiffRecordRawWithArray, ICloneable
{
  private static UnknownRecord _empty = new UnknownRecord();
  private byte[] m_tempData;

  public static BiffRecordRaw Empty => (BiffRecordRaw) UnknownRecord._empty;

  public UnknownRecord()
  {
  }

  public UnknownRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public UnknownRecord(BinaryReader reader, out int itemSize)
  {
    this.m_iCode = (int) reader.ReadInt16();
    this.m_iLength = (int) reader.ReadInt16();
    this.m_data = new byte[this.m_iLength];
    reader.BaseStream.Read(this.m_data, 0, this.m_iLength);
    itemSize = this.m_iLength;
  }

  public UnknownRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override bool NeedDataArray => true;

  public new int RecordCode
  {
    get => base.RecordCode;
    set => this.m_iCode = value;
  }

  public int DataLen
  {
    get => this.m_iLength;
    set => this.m_iLength = value;
  }

  public override void ParseStructure()
  {
    this.m_tempData = new byte[this.m_data.Length];
    this.m_data.CopyTo((Array) this.m_tempData, 0);
  }

  public override void InfillInternalData(ExcelVersion version)
  {
  }

  public override int GetStoreSize(ExcelVersion version) => this.m_iLength;

  public new object Clone()
  {
    UnknownRecord unknownRecord = (UnknownRecord) base.Clone();
    if (this.m_tempData != null)
      unknownRecord.m_tempData = CloneUtils.CloneByteArray(this.m_tempData);
    return (object) unknownRecord;
  }
}
