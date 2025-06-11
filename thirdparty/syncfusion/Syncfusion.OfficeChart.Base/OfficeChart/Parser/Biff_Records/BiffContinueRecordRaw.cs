// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.BiffContinueRecordRaw
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
internal abstract class BiffContinueRecordRaw : BiffRecordRawWithArray
{
  protected ContinueRecordExtractor m_extractor;
  private ContinueRecordBuilder m_builder;
  protected internal List<int> m_arrContinuePos = new List<int>();
  private int m_iIntLen = -1;

  protected ContinueRecordBuilder Builder
  {
    get
    {
      return this.m_builder != null ? this.m_builder : throw new ArgumentNullException(nameof (Builder), "Class does not call parent method InfillInternalData.");
    }
  }

  protected BiffContinueRecordRaw()
  {
  }

  protected BiffContinueRecordRaw(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  protected BiffContinueRecordRaw(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure() => this.ExtractContinueRecords();

  public override void InfillInternalData(OfficeVersion version)
  {
    this.AutoGrowData = true;
    this.m_builder = this.CreateBuilder();
  }

  protected virtual ContinueRecordBuilder CreateBuilder()
  {
    ContinueRecordBuilder builder = new ContinueRecordBuilder(this);
    builder.OnFirstContinue += new EventHandler(this.builder_OnFirstContinue);
    return builder;
  }

  protected virtual bool ExtractContinueRecords()
  {
    if (this.m_extractor == null)
      throw new ArgumentNullException("m_extractor");
    this.m_extractor.StoreStreamPosition();
    int length1 = this.m_data.Length;
    this.m_arrContinuePos.Clear();
    this.m_arrContinuePos.Add(length1);
    ((IEnumerator) this.m_extractor).Reset();
    int iFullLength;
    List<byte[]> numArrayList = this.CollectRecordsData(out iFullLength, ref length1);
    int count = numArrayList.Count;
    if (count > 0)
    {
      byte[] dst = new byte[iFullLength + this.m_iLength];
      Buffer.BlockCopy((Array) this.m_data, 0, (Array) dst, 0, this.m_iLength);
      int iLength = this.m_iLength;
      for (int index = 0; index < count; ++index)
      {
        byte[] src = numArrayList[index];
        int length2 = src.Length;
        Buffer.BlockCopy((Array) src, 0, (Array) dst, iLength, length2);
        iLength += length2;
      }
      this.m_data = dst;
    }
    return count > 0;
  }

  protected List<byte[]> CollectRecordsData(out int iFullLength, ref int iLastPos)
  {
    ((IEnumerator) this.m_extractor).Reset();
    List<byte[]> arrRecords = new List<byte[]>();
    iFullLength = 0;
    while (((IEnumerator) this.m_extractor).MoveNext())
    {
      int num = this.AddRecordData(arrRecords, this.m_extractor.Current);
      iLastPos += num;
      iFullLength += num;
      this.m_arrContinuePos.Add(iLastPos);
    }
    return arrRecords;
  }

  protected virtual int AddRecordData(List<byte[]> arrRecords, BiffRecordRaw record)
  {
    if (arrRecords == null)
      throw new ArgumentNullException(nameof (arrRecords));
    byte[] numArray = record != null ? record.Data : throw new ArgumentNullException(nameof (record));
    arrRecords.Add(numArray);
    return numArray.Length;
  }

  protected virtual void builder_OnFirstContinue(object sender, EventArgs e)
  {
    ContinueRecordBuilder continueRecordBuilder = (ContinueRecordBuilder) sender;
    continueRecordBuilder.OnFirstContinue -= new EventHandler(this.builder_OnFirstContinue);
    this.m_iIntLen = continueRecordBuilder.Position;
  }

  protected void AddContinueRecordType(TBIFFRecord recordType)
  {
    this.m_extractor.AddRecordType(recordType);
  }

  public override object Clone()
  {
    BiffContinueRecordRaw continueRecordRaw = (BiffContinueRecordRaw) base.Clone();
    continueRecordRaw.m_arrContinuePos = CloneUtils.CloneCloneable<int>(this.m_arrContinuePos);
    return (object) continueRecordRaw;
  }
}
