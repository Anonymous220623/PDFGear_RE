// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ExtSSTRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Exceptions;
using Syncfusion.XlsIO.Implementation.Security;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.ExtSST)]
[CLSCompliant(false)]
public class ExtSSTRecord : BiffRecordRawWithArray
{
  private const int DEF_FIXED_SIZE = 2;
  private const int DEF_SUB_ITEM_SIZE = 8;
  [BiffRecordPos(0, 2)]
  private ushort m_usStringPerBucket = 8;
  private ExtSSTInfoSubRecord[] m_arrSSTInfo;
  private bool m_bIsEnd;
  private SSTRecord m_sst;

  public ushort StringPerBucket
  {
    get => this.m_usStringPerBucket;
    set => this.m_usStringPerBucket = value;
  }

  public ExtSSTInfoSubRecord[] SSTInfo
  {
    get => this.m_arrSSTInfo;
    set => this.m_arrSSTInfo = value;
  }

  public override int MinimumRecordSize => 0;

  public bool IsEnd => this.m_bIsEnd;

  public SSTRecord SST
  {
    get => this.m_sst;
    set => this.m_sst = value;
  }

  public ExtSSTRecord()
  {
  }

  public ExtSSTRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ExtSSTRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override int FillStream(
    BinaryWriter writer,
    DataProvider provider,
    IEncryptor encryptor,
    int streamPosition)
  {
    this.UpdateStringOffsets();
    return base.FillStream(writer, provider, encryptor, streamPosition);
  }

  public override void ParseStructure()
  {
    this.m_usStringPerBucket = this.GetUInt16(0);
    int length = (this.m_iLength - 2) / 8;
    int num = this.m_iLength - 2;
    if (num % 8 != 0)
    {
      if (num % 4 != 0)
      {
        this.m_bIsEnd = this.GetInt32(this.m_iLength - 4) == 10;
        if (this.m_bIsEnd)
          throw new WrongBiffRecordDataException("ExtSSTRecord's data size minus 2 must be divided by 8.");
      }
      else
        this.m_bIsEnd = this.GetInt32(this.m_iLength - 4) == 10;
    }
    this.m_arrSSTInfo = new ExtSSTInfoSubRecord[length];
    int iOffset = 2;
    using (ByteArrayDataProvider arrData = new ByteArrayDataProvider(this.m_data))
    {
      int index = 0;
      while (index < length)
      {
        ExtSSTInfoSubRecord record = (ExtSSTInfoSubRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExtSSTInfoSub);
        record.StreamPos = this.StreamPos + (long) iOffset;
        record.ParseStructure((DataProvider) arrData, iOffset, 8, ExcelVersion.Excel97to2003);
        this.m_arrSSTInfo[index] = record;
        ++index;
        iOffset += 8;
      }
    }
  }

  public override void InfillInternalData(ExcelVersion version)
  {
    this.m_data = new byte[this.GetStoreSize(ExcelVersion.Excel97to2003)];
    this.SetUInt16(0, this.m_usStringPerBucket);
    this.m_iLength = 2;
    if (this.m_arrSSTInfo == null)
      return;
    int index = 0;
    int length = this.m_arrSSTInfo.Length;
    while (index < length)
    {
      this.m_arrSSTInfo[index].StreamPos = (long) this.m_iLength;
      this.SetBytes(this.m_iLength, this.m_arrSSTInfo[index].Data, 0, 8);
      ++index;
      this.m_iLength += 8;
    }
  }

  public void UpdateStringOffsets()
  {
    int streamPos = (int) this.m_sst.StreamPos;
    int numberOfUniqueStrings = (int) this.m_sst.NumberOfUniqueStrings;
    if (numberOfUniqueStrings <= 0)
      return;
    int[] stringsOffsets = this.m_sst.StringsOffsets;
    int[] stringsStreamPos = this.m_sst.StringsStreamPos;
    int index1 = 0;
    int index2 = 0;
    while (index1 < numberOfUniqueStrings)
    {
      int num = stringsOffsets[index1];
      ExtSSTInfoSubRecord sstInfoSubRecord = this.m_arrSSTInfo[index2];
      sstInfoSubRecord.StreamPosition = streamPos + stringsStreamPos[index1];
      sstInfoSubRecord.BucketSSTOffset = (ushort) num;
      index1 += (int) this.StringPerBucket;
      ++index2;
    }
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    return 2 + (this.m_arrSSTInfo != null ? this.m_arrSSTInfo.Length : 0) * 8;
  }
}
