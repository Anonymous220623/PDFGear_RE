// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.MsoBase
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;

[CLSCompliant(false)]
public abstract class MsoBase : BiffRecordRawWithArray
{
  private const ushort DEF_VERSION_MASK = 15;
  private const ushort DEF_INST_MASK = 65520;
  private const ushort DEF_INST_START_BIT = 4;
  private const int DEF_MAXIMUM_RECORD_SIZE = 2147483647 /*0x7FFFFFFF*/;
  protected ushort m_usVersionAndInst;
  private ushort m_usRecordType;
  private GetNextMsoDrawingData m_dataGetter;
  private MsoBase m_parent;
  private static Dictionary<Type, int> s_dicTypeToCode = new Dictionary<Type, int>();

  static MsoBase()
  {
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtClientTextBox), 61453);
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtSp), 61450);
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtSpgrContainer), 61443);
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtAnchor), 61454);
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtClientAnchor), 61456);
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtDgContainer), 61442);
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtRegroupItems), 61720);
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtDg), 61448);
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtDggContainer), 61440 /*0xF000*/);
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtOPT), 61451);
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtSpContainer), 61444);
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtSplitMenuColors), 61726);
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtDgg), 61446);
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtBSE), 61447);
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtSpgr), 61449);
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtBstoreContainer), 61441);
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtClientData), 61457);
    MsoBase.s_dicTypeToCode.Add(typeof (MsoUnknown), (int) ushort.MaxValue);
    MsoBase.s_dicTypeToCode.Add(typeof (MsofbtChildAnchor), 61455);
    MsoBase.s_dicTypeToCode.Add(typeof (MsoMetafilePicture), 0);
    MsoBase.s_dicTypeToCode.Add(typeof (MsoBitmapPicture), 0);
  }

  public MsoBase()
  {
    Type type = this.GetType();
    MsoBase.s_dicTypeToCode.TryGetValue(type, out this.m_iCode);
    this.m_usRecordType = (ushort) this.m_iCode;
  }

  public MsoBase(MsoBase parent)
    : this()
  {
    this.m_parent = parent;
  }

  public MsoBase(MsoBase parent, byte[] data, int offset)
    : this(parent, data, offset, (GetNextMsoDrawingData) null)
  {
  }

  public MsoBase(MsoBase parent, byte[] data, int offset, GetNextMsoDrawingData dataGetter)
    : this(parent)
  {
    this.m_dataGetter = dataGetter;
    this.FillRecord(data, offset);
  }

  public MsoBase(MsoBase parent, Stream stream, GetNextMsoDrawingData dataGetter)
    : this(parent)
  {
    this.m_dataGetter = dataGetter;
    this.FillRecord(stream);
  }

  public int Version
  {
    get => (int) BiffRecordRaw.GetUInt16BitsByMask(this.m_usVersionAndInst, (ushort) 15);
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usVersionAndInst, (ushort) 15, (ushort) value);
    }
  }

  public int Instance
  {
    get => (int) BiffRecordRaw.GetUInt16BitsByMask(this.m_usVersionAndInst, (ushort) 65520) >> 4;
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usVersionAndInst, (ushort) 65520, (ushort) (value << 4));
    }
  }

  public MsoRecords MsoRecordType
  {
    get => (MsoRecords) this.m_usRecordType;
    set => this.m_usRecordType = (ushort) value;
  }

  public GetNextMsoDrawingData DataGetter
  {
    get => this.m_dataGetter;
    set => this.m_dataGetter = value;
  }

  public MsoBase Parent => this.m_parent;

  public virtual int FillRecord(byte[] data, int iOffset)
  {
    if (data == null)
      throw new ArgumentNullException("reader");
    int num = iOffset;
    try
    {
      if (data.Length - iOffset - 8 < 0)
        throw new ApplicationException("Unexpected end of record - reached end of the array.");
      this.m_usVersionAndInst = BitConverter.ToUInt16(data, iOffset);
      iOffset += 2;
      this.m_usRecordType = BitConverter.ToUInt16(data, iOffset);
      iOffset += 2;
      if (this.m_usRecordType == (ushort) 0)
        throw new ApplicationException("Mso Record identification code is wrong (zero).");
      this.m_iLength = BitConverter.ToInt32(data, iOffset);
      iOffset += 4;
      if (this.m_iLength < this.MinimumRecordSize)
        throw new SmallBiffRecordDataException($"Code :{this.m_iCode.ToString()}\n Real size: {(object) this.m_iLength}. Expected size: {this.MaximumRecordSize.ToString()}");
      if (this.m_iLength > this.MaximumRecordSize)
        throw new LargeBiffRecordDataException($"Code :{((MsoRecords) this.m_iCode).ToString()}{(object) this.m_iCode}\n Real size: {(object) this.m_iLength}. Expected size: {this.MaximumRecordSize.ToString()}");
      if (data.Length - iOffset - this.m_iLength < 0)
        throw new ApplicationException("Unexpected end of records stream. Record data cannot be read - reached end of stream.");
      this.m_data = new byte[this.m_iLength];
      Array.Copy((Array) data, iOffset, (Array) this.m_data, 0, this.m_iLength);
      this.ParseStructure();
      return this.m_iLength + 8;
    }
    catch (ApplicationException ex)
    {
      Exception innerException = ex.InnerException;
      iOffset = num;
      throw;
    }
  }

  public virtual void FillArray(Stream stream)
  {
    this.FillArray(stream, 0, (List<int>) null, (List<List<BiffRecordRaw>>) null);
  }

  public virtual void FillArray(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords)
  {
    byte[] buffer = new byte[8];
    long position1 = stream.Position;
    stream.Position += 8L;
    this.InfillInternalData(stream, iOffset, arrBreaks, arrRecords);
    long position2 = stream.Position;
    stream.Position = position1;
    int index1 = 0;
    BitConverter.GetBytes(this.m_usVersionAndInst).CopyTo((Array) buffer, index1);
    int index2 = index1 + 2;
    BitConverter.GetBytes(this.m_usRecordType).CopyTo((Array) buffer, index2);
    int index3 = index2 + 2;
    BitConverter.GetBytes(this.m_iLength).CopyTo((Array) buffer, index3);
    int count = index3 + 4;
    stream.Write(buffer, 0, count);
    stream.Position = position2;
  }

  public override int MaximumRecordSize => int.MaxValue;

  public override void InfillInternalData(ExcelVersion version)
  {
    this.InfillInternalData((Stream) new MemoryStream(), 0, (List<int>) null, (List<List<BiffRecordRaw>>) null);
  }

  public abstract void InfillInternalData(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords);

  public MsoBase Clone(MsoBase parent)
  {
    MsoBase msoBase = (MsoBase) this.InternalClone();
    msoBase.m_parent = parent;
    return msoBase;
  }

  protected virtual object InternalClone()
  {
    MsoBase msoBase = (MsoBase) base.Clone();
    if (this.m_data != null)
      msoBase.m_data = CloneUtils.CloneByteArray(this.m_data);
    return (object) msoBase;
  }

  public override object Clone() => this.InternalClone();

  public virtual void UpdateNextMsoDrawingData()
  {
  }

  public abstract void ParseStructure(Stream stream);

  public override void ParseStructure()
  {
    throw new NotSupportedException("The method or operation is not supported for MsoRecords.");
  }

  public static double ConvertFromInt32(int value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    double int16 = (double) BitConverter.ToInt16(bytes, 0);
    ushort uint16 = BitConverter.ToUInt16(bytes, 2);
    double num1 = 0.5;
    ushort num2 = 32768 /*0x8000*/;
    for (int index = 0; index < 16 /*0x10*/; ++index)
    {
      if (((int) num2 & (int) uint16) != 0)
        int16 += num1;
      num1 /= 2.0;
      num2 >>= 1;
    }
    return int16;
  }

  public static void WriteInt32(Stream stream, int value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    stream.Write(bytes, 0, bytes.Length);
  }

  public static void WriteUInt32(Stream stream, uint value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    stream.Write(bytes, 0, bytes.Length);
  }

  public static void WriteInt16(Stream stream, short value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    stream.Write(bytes, 0, bytes.Length);
  }

  public static void WriteUInt16(Stream stream, ushort value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    stream.Write(bytes, 0, bytes.Length);
  }

  public static int ReadInt32(Stream stream)
  {
    byte[] buffer = new byte[4];
    stream.Read(buffer, 0, 4);
    return BitConverter.ToInt32(buffer, 0);
  }

  public static uint ReadUInt32(Stream stream)
  {
    byte[] buffer = new byte[4];
    stream.Read(buffer, 0, 4);
    return BitConverter.ToUInt32(buffer, 0);
  }

  public static short ReadInt16(Stream stream)
  {
    byte[] buffer = new byte[2];
    stream.Read(buffer, 0, 2);
    return BitConverter.ToInt16(buffer, 0);
  }

  public static ushort ReadUInt16(Stream stream)
  {
    byte[] buffer = new byte[2];
    stream.Read(buffer, 0, 2);
    return BitConverter.ToUInt16(buffer, 0);
  }

  internal void FillRecord(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    try
    {
      if (stream.Length - stream.Position - 8L < 0L)
        throw new ApplicationException("Unexpected end of record - reached end of the array.");
      this.m_usVersionAndInst = MsoBase.ReadUInt16(stream);
      this.m_usRecordType = MsoBase.ReadUInt16(stream);
      if (this.m_usRecordType == (ushort) 0)
        throw new ApplicationException("Mso Record identification code is wrong (zero).");
      this.m_iLength = MsoBase.ReadInt32(stream);
      if (this.m_iLength < this.MinimumRecordSize)
        throw new SmallBiffRecordDataException($"Code :{this.m_iCode.ToString()}\n Real size: {(object) this.m_iLength}. Expected size: {this.MaximumRecordSize.ToString()}");
      if (this.m_iLength > this.MaximumRecordSize)
        throw new LargeBiffRecordDataException($"Code :{((MsoRecords) this.m_iCode).ToString()}{(object) this.m_iCode}\n Real size: {(object) this.m_iLength}. Expected size: {this.MaximumRecordSize.ToString()}");
      if (stream.Length - stream.Position - (long) this.m_iLength < 0L)
        throw new ApplicationException("Unexpected end of records stream. Record data cannot be read - reached end of stream.");
      this.ParseStructure(stream);
    }
    catch (ApplicationException ex)
    {
      Exception innerException = ex.InnerException;
      throw;
    }
  }
}
