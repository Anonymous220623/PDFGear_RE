// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ContinueRecordExtractor
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
internal class ContinueRecordExtractor : IEnumerator
{
  private static readonly int[] DEF_RECORDS = new int[1]
  {
    60
  };
  private BinaryReader m_tmpReader;
  private long m_lStartPos;
  private BiffRecordRaw m_continue;
  private bool m_bReset;
  private List<int> m_arrAllowedRecords = new List<int>((IEnumerable<int>) ContinueRecordExtractor.DEF_RECORDS);
  private byte[] arrBuffer = new byte[4096 /*0x1000*/];
  private DataProvider m_provider;

  private ContinueRecordExtractor()
  {
  }

  protected bool IsStreamEOF
  {
    get
    {
      if (this.m_tmpReader == null)
        throw new ArgumentNullException("m_tmpReader");
      try
      {
        return this.m_tmpReader.BaseStream.Position == this.m_tmpReader.BaseStream.Length || this.PeekRecord() == null;
      }
      catch (Exception ex)
      {
        return true;
      }
    }
  }

  protected BiffRecordRaw PeekRecord()
  {
    if (this.m_tmpReader == null)
      throw new ArgumentNullException("m_tmpReader");
    long position = this.m_tmpReader.BaseStream.Position;
    BiffRecordRaw untypedRecord = BiffRecordFactory.GetUntypedRecord(this.m_tmpReader);
    this.m_tmpReader.BaseStream.Position = position;
    return untypedRecord;
  }

  public BiffRecordRaw Current
  {
    get
    {
      if (this.m_tmpReader == null)
        throw new ArgumentNullException("m_tmpReader");
      return this.m_continue != null && this.m_bReset ? this.m_continue : throw new ArgumentException("First call Reset method and then MoveNext. Wrong enumerator initialization.");
    }
  }

  public long StoreStreamPosition()
  {
    this.m_lStartPos = this.m_tmpReader.BaseStream.Position;
    this.m_continue = (BiffRecordRaw) null;
    this.m_bReset = false;
    return this.m_lStartPos;
  }

  public void AddRecordType(TBIFFRecord recordType)
  {
    int num = (int) recordType;
    if (this.m_arrAllowedRecords.IndexOf(num) != -1)
      return;
    this.m_arrAllowedRecords.Add(num);
  }

  void IEnumerator.Reset()
  {
    if (this.m_tmpReader == null)
      throw new ArgumentNullException("m_tmpReader");
    this.m_tmpReader.BaseStream.Position = this.m_lStartPos;
    this.m_bReset = true;
  }

  object IEnumerator.Current
  {
    get
    {
      if (this.m_tmpReader == null)
        throw new ArgumentNullException("m_tmpReader");
      return this.m_continue != null && this.m_bReset ? (object) this.m_continue : throw new ArgumentException("First call Reset method and then MoveNext. Wrong enumerator initialization.");
    }
  }

  bool IEnumerator.MoveNext()
  {
    if (this.m_tmpReader == null)
      throw new ArgumentNullException("m_tmpReader");
    if (!this.IsStreamEOF)
    {
      int type = (int) this.m_tmpReader.ReadInt16();
      int count = (int) this.m_tmpReader.ReadInt16();
      if (this.m_arrAllowedRecords.IndexOf(type) != -1)
      {
        this.m_continue = BiffRecordFactory.GetRecord(type);
        this.m_continue.Length = count;
        this.m_continue.Data = this.m_tmpReader.ReadBytes(count);
        this.m_bReset = true;
        return true;
      }
      this.m_tmpReader.BaseStream.Position -= 4L;
    }
    this.m_continue = (BiffRecordRaw) null;
    this.m_bReset = false;
    return false;
  }
}
