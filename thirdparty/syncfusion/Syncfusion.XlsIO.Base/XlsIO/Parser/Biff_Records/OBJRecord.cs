// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.OBJRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.OBJ)]
public class OBJRecord : BiffRecordRaw, ICloneable
{
  private List<ObjSubRecord> m_records = new List<ObjSubRecord>();

  public ObjSubRecord[] Records => this.m_records.ToArray();

  public List<ObjSubRecord> RecordsList => this.m_records;

  public override bool NeedDataArray => true;

  public OBJRecord()
  {
  }

  public OBJRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public OBJRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    int iStartOffset = iOffset;
    int num = iOffset + this.m_iLength;
    TObjType objectType = TObjType.otGroup;
    do
    {
      ObjSubRecord subRecord = this.GetSubRecord(provider, iOffset, iStartOffset, objectType);
      this.m_records.Add(subRecord);
      if (subRecord.Type == TObjSubRecordType.ftCmo)
        objectType = ((ftCmo) subRecord).ObjectType;
      iOffset += (int) subRecord.Length + 4;
    }
    while (iOffset < num);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = 0;
    int index = 0;
    for (int count = this.m_records.Count; index < count; ++index)
    {
      ObjSubRecord record = this.m_records[index];
      record.FillArray(provider, iOffset);
      int storeSize = record.GetStoreSize(version);
      this.m_iLength += storeSize;
      iOffset += storeSize;
    }
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    int storeSize = 0;
    int index = 0;
    for (int count = this.m_records.Count; index < count; ++index)
    {
      ObjSubRecord record = this.m_records[index];
      storeSize += record.GetStoreSize(version);
    }
    return storeSize;
  }

  protected ObjSubRecord GetSubRecord(
    DataProvider provider,
    int offset,
    int iStartOffset,
    TObjType objectType)
  {
    TObjSubRecordType type = (TObjSubRecordType) provider.ReadInt16(offset);
    ushort length = provider.ReadUInt16(offset + 2);
    if (type == TObjSubRecordType.ftLbsData || (int) length + offset + 4 > this.Length)
    {
      int num = 4;
      int iOffset = this.Length - 4;
      if (provider.ReadInt16(iOffset) == (short) 0)
        num += 4;
      length = (ushort) (this.Length - offset - num + iStartOffset);
    }
    if (type == TObjSubRecordType.ftEnd)
      length = (ushort) 0;
    byte[] numArray = new byte[(int) length];
    provider.ReadArray(offset + 4, numArray);
    switch (type)
    {
      case TObjSubRecordType.ftEnd:
        return (ObjSubRecord) new ftEnd(type, length, numArray);
      case TObjSubRecordType.ftMacro:
        return (ObjSubRecord) new ftMacro(length, numArray);
      case TObjSubRecordType.ftCf:
        return (ObjSubRecord) new ftCf(TObjSubRecordType.ftCf, length, numArray);
      case TObjSubRecordType.ftPioGrbit:
        return (ObjSubRecord) new ftPioGrbit(TObjSubRecordType.ftPioGrbit, length, numArray);
      case TObjSubRecordType.ftCbls:
        return (ObjSubRecord) new ftCbls(type, length, numArray);
      case TObjSubRecordType.ftRbo:
        return (ObjSubRecord) new ftRbo(length, numArray);
      case TObjSubRecordType.ftSbs:
        return (ObjSubRecord) new ftSbs(type, length, numArray);
      case TObjSubRecordType.ftNts:
        return (ObjSubRecord) new ftNts(type, length, numArray);
      case TObjSubRecordType.ftSbsFormula:
        return (ObjSubRecord) new ftSbsFormula(type, length, numArray);
      case TObjSubRecordType.ftRboData:
        return (ObjSubRecord) new ftRboData(length, numArray);
      case TObjSubRecordType.ftCblsData:
        return (ObjSubRecord) new ftCblsData(type, length, numArray);
      case TObjSubRecordType.ftLbsData:
        return (ObjSubRecord) new ftLbsData(type, length, numArray, objectType);
      case TObjSubRecordType.ftCblsFmla:
        return (ObjSubRecord) new ftCblsFmla(type, length, numArray);
      case TObjSubRecordType.ftCmo:
        return (ObjSubRecord) new ftCmo(type, length, numArray);
      default:
        return (ObjSubRecord) new ftUnknown(type, length, numArray);
    }
  }

  public void AddSubRecord(ObjSubRecord record) => this.m_records.Add(record);

  public ObjSubRecord FindSubRecord(TObjSubRecordType recordType)
  {
    int subRecordIndex = this.FindSubRecordIndex(recordType);
    return subRecordIndex < 0 ? (ObjSubRecord) null : this.m_records[subRecordIndex];
  }

  public int FindSubRecordIndex(TObjSubRecordType recordType)
  {
    int subRecordIndex = -1;
    int index = 0;
    for (int count = this.m_records.Count; index < count; ++index)
    {
      if (this.m_records[index].Type == recordType)
      {
        subRecordIndex = index;
        break;
      }
    }
    return subRecordIndex;
  }

  public new object Clone()
  {
    OBJRecord objRecord = (OBJRecord) base.Clone();
    objRecord.m_records = CloneUtils.CloneCloneable<ObjSubRecord>(this.m_records);
    return (object) objRecord;
  }
}
