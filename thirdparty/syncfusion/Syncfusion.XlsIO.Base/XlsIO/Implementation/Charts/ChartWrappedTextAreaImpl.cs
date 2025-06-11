// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartWrappedTextAreaImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartWrappedTextAreaImpl : ChartTextAreaImpl, ISerializable
{
  private static readonly byte[][] DEF_UNKNOWN_START = new byte[4][]
  {
    new byte[20]
    {
      (byte) 80 /*0x50*/,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 10,
      (byte) 10,
      (byte) 3,
      (byte) 0,
      (byte) 80 /*0x50*/,
      (byte) 8,
      (byte) 90,
      (byte) 8,
      (byte) 97,
      (byte) 8,
      (byte) 97,
      (byte) 8,
      (byte) 106,
      (byte) 8,
      (byte) 107,
      (byte) 8
    },
    new byte[12]
    {
      (byte) 82,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 13,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[12]
    {
      (byte) 106,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[12]
    {
      (byte) 84,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 18,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    }
  };
  private static readonly byte[][] DEF_UNKNOWN_END = new byte[2][]
  {
    new byte[12]
    {
      (byte) 85,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 18,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[12]
    {
      (byte) 83,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 13,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    }
  };

  public ChartWrappedTextAreaImpl(IApplication application, object parent)
    : base(application, parent)
  {
  }

  public ChartWrappedTextAreaImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : base(application, parent, data, ref iPos)
  {
  }

  [CLSCompliant(false)]
  public ChartWrappedTextAreaImpl(
    IApplication application,
    object parent,
    ExcelObjectTextLink textLink)
    : base(application, parent, textLink)
  {
  }

  protected override ChartFrameFormatImpl CreateFrameFormat()
  {
    return (ChartFrameFormatImpl) new ChartWrappedFrameFormatImpl(this.Application, (object) this);
  }

  [CLSCompliant(false)]
  protected override void SerializeRecord(IList<IBiffStorage> records, BiffRecordRaw record)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    if (record.TypeCode == TBIFFRecord.ChartDataLabels)
    {
      base.SerializeRecord(records, record);
    }
    else
    {
      ChartWrapperRecord record1 = (ChartWrapperRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartWrapper);
      record1.Record = (BiffRecordRaw) record.Clone();
      records.Add((IBiffStorage) record1);
    }
  }

  protected override bool ShouldSerialize => true;

  private void SerializeUnknown(OffsetArrayList records, byte[][] arrUnknown)
  {
    if (arrUnknown == null)
      throw new ArgumentNullException(nameof (arrUnknown));
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    int index = 0;
    for (int length1 = arrUnknown.Length; index < length1; ++index)
    {
      byte[] numArray = arrUnknown[index];
      int length2 = numArray != null ? numArray.Length : throw new ArgumentNullException("arrData");
      UnknownRecord record = (UnknownRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Unknown);
      record.RecordCode = (int) BitConverter.ToUInt16(numArray, 0);
      record.m_data = new byte[length2];
      record.DataLen = length2;
      numArray.CopyTo((Array) record.m_data, 0);
      records.Add((IBiffStorage) record);
    }
  }
}
