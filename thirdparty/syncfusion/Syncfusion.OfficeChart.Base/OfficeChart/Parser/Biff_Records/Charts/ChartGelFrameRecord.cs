// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartGelFrameRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartGelFrame)]
[CLSCompliant(false)]
internal class ChartGelFrameRecord : BiffContinueRecordRaw
{
  public const int DEF_START_MSO_INDEX = 384;
  public const int DEF_LAST_MSO_INDEX = 412;
  public const int DEF_OFFSET = 8;
  private readonly byte[] DEF_FIRST_BYTES = new byte[4]
  {
    (byte) 227,
    (byte) 1,
    (byte) 11,
    (byte) 240 /*0xF0*/
  };
  private readonly byte[] DEF_LAST_BYTES = new byte[74]
  {
    (byte) 179,
    (byte) 0,
    (byte) 34,
    (byte) 241,
    (byte) 66,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 158,
    (byte) 1,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue,
    (byte) 159,
    (byte) 1,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue,
    (byte) 160 /*0xA0*/,
    (byte) 1,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 32 /*0x20*/,
    (byte) 161,
    (byte) 193,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 162,
    (byte) 1,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue,
    (byte) 163,
    (byte) 1,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue,
    (byte) 164,
    (byte) 1,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 32 /*0x20*/,
    (byte) 165,
    (byte) 193,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 166,
    (byte) 1,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue,
    (byte) 167,
    (byte) 1,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue,
    (byte) 191,
    (byte) 1,
    (byte) 0,
    (byte) 0,
    (byte) 96 /*0x60*/,
    (byte) 0
  };
  private List<MsofbtOPT.FOPTE> m_list = new List<MsofbtOPT.FOPTE>();

  public override bool NeedDataArray => true;

  public List<MsofbtOPT.FOPTE> OptionList
  {
    get => this.m_list;
    set
    {
      this.m_list = value != null ? value : throw new ArgumentNullException(nameof (OptionList));
    }
  }

  public ChartGelFrameRecord()
  {
  }

  public ChartGelFrameRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartGelFrameRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure()
  {
    if (this.m_extractor == null)
      return;
    this.AddContinueRecordType(this.TypeCode);
    base.ParseStructure();
    this.ParseData();
    this.m_arrContinuePos.Clear();
  }

  private void ParseData()
  {
    int iOffset = 8;
    uint num = BitConverter.ToUInt32(this.m_data, 4) + (uint) iOffset;
    while ((long) iOffset < (long) num)
    {
      MsofbtOPT.FOPTE fopte = new MsofbtOPT.FOPTE(this.m_data, ref iOffset);
      this.m_list.Add(fopte);
      if (fopte.IsComplex)
        num -= fopte.UInt32Value;
    }
    int index = 0;
    for (int count = this.m_list.Count; index < count; ++index)
      this.m_list[index].ReadComplexData(this.m_data, ref iOffset);
  }

  public override void InfillInternalData(OfficeVersion version)
  {
  }

  private void FillDataList()
  {
    this.m_iLength = 0;
    int index1 = 8;
    int index2 = 0;
    for (int count = this.m_list.Count; index2 < count; ++index2)
    {
      MsofbtOPT.FOPTE fopte = this.m_list[index2];
      this.m_iLength += fopte.MainData.Length;
      index1 += fopte.MainData.Length;
      if (fopte.AdditionalData != null && fopte.AdditionalData.Length > 0)
        this.m_iLength += fopte.AdditionalData.Length;
    }
    this.m_iLength += this.DEF_LAST_BYTES.Length + 8;
    this.m_data = new byte[this.m_iLength];
    this.DEF_FIRST_BYTES.CopyTo((Array) this.m_data, 0);
    this.DEF_LAST_BYTES.CopyTo((Array) this.m_data, this.m_iLength - this.DEF_LAST_BYTES.Length);
    Array.Copy((Array) BitConverter.GetBytes(this.m_iLength - 8 - this.DEF_LAST_BYTES.Length), 0, (Array) this.m_data, 4, 4);
    int index3 = 8;
    int index4 = 0;
    for (int count = this.m_list.Count; index4 < count; ++index4)
    {
      MsofbtOPT.FOPTE fopte = this.m_list[index4];
      fopte.MainData.CopyTo((Array) this.m_data, index3);
      index3 += fopte.MainData.Length;
      if (fopte.AdditionalData != null && fopte.AdditionalData.Length > 0)
      {
        fopte.AdditionalData.CopyTo((Array) this.m_data, index1);
        index1 += fopte.AdditionalData.Length;
      }
    }
  }

  public List<BiffRecordRaw> UpdatesToAddInStream()
  {
    this.FillDataList();
    List<BiffRecordRaw> addInStream = new List<BiffRecordRaw>();
    int length1 = this.m_data.Length;
    BiffRecordRawWithArray recordRawWithArray = (BiffRecordRawWithArray) this;
    int sourceIndex = 0;
    int num = 0;
    for (; length1 > 8224; length1 -= 8224)
    {
      recordRawWithArray = (BiffRecordRawWithArray) BiffRecordFactory.GetRecord(num < 2 ? TBIFFRecord.ChartGelFrame : TBIFFRecord.Continue);
      byte[] numArray = new byte[8224];
      Array.Copy((Array) this.m_data, sourceIndex, (Array) numArray, 0, 8224);
      recordRawWithArray.SetInternalData(numArray);
      recordRawWithArray.Length = 8224;
      addInStream.Add((BiffRecordRaw) recordRawWithArray);
      sourceIndex += 8224;
      ++num;
    }
    if (sourceIndex == 0)
    {
      addInStream.Add((BiffRecordRaw) recordRawWithArray);
    }
    else
    {
      BiffRecordRawWithArray record = (BiffRecordRawWithArray) BiffRecordFactory.GetRecord(num < 2 ? TBIFFRecord.ChartGelFrame : TBIFFRecord.Continue);
      int length2 = this.m_data.Length - sourceIndex;
      byte[] numArray = new byte[length2];
      Array.Copy((Array) this.m_data, sourceIndex, (Array) numArray, 0, length2);
      record.SetInternalData(numArray);
      record.Length = length2;
      addInStream.Add((BiffRecordRaw) record);
    }
    return addInStream;
  }

  public override object Clone()
  {
    ChartGelFrameRecord chartGelFrameRecord = (ChartGelFrameRecord) base.Clone();
    chartGelFrameRecord.m_list = new List<MsofbtOPT.FOPTE>();
    int index = 0;
    for (int count = this.m_list.Count; index < count; ++index)
    {
      MsofbtOPT.FOPTE fopte = this.m_list[index];
      chartGelFrameRecord.m_list.Add((MsofbtOPT.FOPTE) fopte.Clone());
    }
    return (object) chartGelFrameRecord;
  }

  public void UpdateToSerialize()
  {
    this.m_iLength = -1;
    int index1;
    for (int index2 = 384; index2 <= 412; ++index2)
    {
      MsoOptions id = (MsoOptions) index2;
      if (!this.Contains(id, out index1))
      {
        MsofbtOPT.FOPTE fopte = new MsofbtOPT.FOPTE();
        fopte.Id = id;
        if (fopte.Id == MsoOptions.Transparency || fopte.Id == MsoOptions.GradientTransparency)
          fopte.UInt32Value = (uint) ushort.MaxValue;
        if (fopte.Id == MsoOptions.GradientColorType)
          fopte.UInt32Value = 1U;
        this.m_list.Insert(index1, fopte);
      }
    }
    if (this.Contains(MsoOptions.NoFillHitTest, out index1))
      return;
    this.m_list.Insert(index1, new MsofbtOPT.FOPTE()
    {
      Id = MsoOptions.NoFillHitTest
    });
  }

  private bool Contains(MsoOptions id, out int index)
  {
    index = 0;
    int count = this.m_list.Count;
    while (index < count && this.m_list[index].Id != id)
      ++index;
    return index < this.m_list.Count;
  }
}
