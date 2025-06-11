// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsoFactory
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;

[CLSCompliant(false)]
internal class MsoFactory
{
  private static Dictionary<int, MsoBase> m_hashCodeToMSORecord = new Dictionary<int, MsoBase>();

  private MsoFactory()
  {
  }

  static MsoFactory() => MsoFactory.RegisterAllTypes();

  public static MsoBase CreateMsoRecord(
    MsoBase parent,
    MsoRecords recordType,
    byte[] data,
    ref int iOffset)
  {
    int key = (int) recordType;
    MsoBase msoRecord = (MsoBase) (MsoFactory.m_hashCodeToMSORecord.ContainsKey(key) ? (BiffRecordRaw) MsoFactory.m_hashCodeToMSORecord[key] : (BiffRecordRaw) MsoFactory.m_hashCodeToMSORecord[(int) ushort.MaxValue]).Clone();
    msoRecord.FillRecord(data, iOffset);
    iOffset += msoRecord.Length + 8;
    return msoRecord;
  }

  public static MsoBase CreateMsoRecord(
    MsoBase parent,
    MsoRecords recordType,
    byte[] data,
    ref int iOffset,
    GetNextMsoDrawingData dataGetter)
  {
    MsoBase msoRecord = (MsoBase) (MsoFactory.m_hashCodeToMSORecord.ContainsKey((int) recordType) ? (BiffRecordRaw) MsoFactory.m_hashCodeToMSORecord[(int) recordType] : (BiffRecordRaw) MsoFactory.m_hashCodeToMSORecord[(int) ushort.MaxValue]).Clone();
    msoRecord.DataGetter = dataGetter;
    msoRecord.FillRecord(data, iOffset);
    msoRecord.UpdateNextMsoDrawingData();
    iOffset += msoRecord.Length + 8;
    return msoRecord;
  }

  public static MsoBase CreateMsoRecord(MsoBase parent, byte[] data, ref int iOffset)
  {
    MsoRecords uint16 = (MsoRecords) BitConverter.ToUInt16(data, iOffset + 2);
    return MsoFactory.CreateMsoRecord(parent, uint16, data, ref iOffset);
  }

  public static MsoBase CreateMsoRecord(
    MsoBase parent,
    byte[] data,
    ref int iOffset,
    GetNextMsoDrawingData dataGetter)
  {
    MsoRecords uint16 = (MsoRecords) BitConverter.ToUInt16(data, iOffset + 2);
    return MsoFactory.CreateMsoRecord(parent, uint16, data, ref iOffset, dataGetter);
  }

  public static MsoBase CreateMsoRecord(MsoBase parent, Stream stream)
  {
    byte[] buffer = new byte[4];
    stream.Read(buffer, 0, 4);
    stream.Position -= 4L;
    MsoRecords uint16 = (MsoRecords) BitConverter.ToUInt16(buffer, 2);
    return MsoFactory.CreateMsoRecord(parent, uint16, stream);
  }

  public static MsoBase CreateMsoRecord(MsoBase parent, MsoRecords recordType, Stream stream)
  {
    int key = (int) recordType;
    MsoBase msoRecord = (MsoBase) (MsoFactory.m_hashCodeToMSORecord.ContainsKey(key) ? (BiffRecordRaw) MsoFactory.m_hashCodeToMSORecord[key] : (BiffRecordRaw) MsoFactory.m_hashCodeToMSORecord[(int) ushort.MaxValue]).Clone();
    msoRecord.FillRecord(stream);
    return msoRecord;
  }

  public static MsoBase CreateMsoRecord(
    MsoBase parent,
    Stream stream,
    GetNextMsoDrawingData dataGetter)
  {
    byte[] buffer = new byte[4];
    stream.Read(buffer, 0, 4);
    stream.Position -= 4L;
    MsoRecords uint16 = (MsoRecords) BitConverter.ToUInt16(buffer, 2);
    return MsoFactory.CreateMsoRecord(parent, uint16, stream, dataGetter);
  }

  public static MsoBase CreateMsoRecord(
    MsoBase parent,
    MsoRecords recordType,
    Stream stream,
    GetNextMsoDrawingData dataGetter)
  {
    MsoBase msoRecord = (MsoBase) (MsoFactory.m_hashCodeToMSORecord.ContainsKey((int) recordType) ? (BiffRecordRaw) MsoFactory.m_hashCodeToMSORecord[(int) recordType] : (BiffRecordRaw) MsoFactory.m_hashCodeToMSORecord[(int) ushort.MaxValue]).Clone();
    msoRecord.DataGetter = dataGetter;
    msoRecord.FillRecord(stream);
    msoRecord.UpdateNextMsoDrawingData();
    return msoRecord;
  }

  public static MsoBase GetRecord(MsoRecords type)
  {
    MsoBase record = MsoFactory.m_hashCodeToMSORecord[(int) type];
    if (record != null)
      record = (MsoBase) record.Clone();
    return record;
  }

  private static void RegisterType(Type type, MsoDrawingAttribute[] attributes)
  {
    ConstructorInfo constructor = type.GetConstructor(new Type[1]
    {
      typeof (MsoBase)
    });
    object obj = !(constructor == (ConstructorInfo) null) ? constructor.Invoke(new object[1]) : throw new ApplicationException("Cannot find constructor");
    MsoFactory.m_hashCodeToMSORecord.Add((int) attributes[0].RecordType, (MsoBase) obj);
  }

  private static void RegisterAllTypes()
  {
    MsoBase msoBase1 = (MsoBase) new MsofbtClientTextBox((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61453, msoBase1);
    MsoBase msoBase2 = (MsoBase) new MsofbtSp((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61450, msoBase2);
    MsoBase msoBase3 = (MsoBase) new MsofbtSpgrContainer((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61443, msoBase3);
    MsoBase msoBase4 = (MsoBase) new MsofbtAnchor((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61454, msoBase4);
    MsoBase msoBase5 = (MsoBase) new MsofbtClientAnchor((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61456, msoBase5);
    MsoBase msoBase6 = (MsoBase) new MsofbtDgContainer((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61442, msoBase6);
    MsoBase msoBase7 = (MsoBase) new MsofbtRegroupItems((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61720, msoBase7);
    MsoBase msoBase8 = (MsoBase) new MsofbtDg((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61448, msoBase8);
    MsoBase msoBase9 = (MsoBase) new MsofbtDggContainer((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61440 /*0xF000*/, msoBase9);
    MsoBase msoBase10 = (MsoBase) new MsofbtOPT((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61451, msoBase10);
    MsoBase msoBase11 = (MsoBase) new MsofbtSpContainer((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61444, msoBase11);
    MsoBase msoBase12 = (MsoBase) new MsofbtSplitMenuColors((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61726, msoBase12);
    MsoBase msoBase13 = (MsoBase) new MsofbtDgg((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61446, msoBase13);
    MsoBase msoBase14 = (MsoBase) new MsofbtBSE((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61447, msoBase14);
    MsoBase msoBase15 = (MsoBase) new MsofbtSpgr((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61449, msoBase15);
    MsoBase msoBase16 = (MsoBase) new MsofbtBstoreContainer((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61441, msoBase16);
    MsoBase msoBase17 = (MsoBase) new MsofbtClientData((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61457, msoBase17);
    MsoBase msoBase18 = (MsoBase) new MsoUnknown((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add((int) ushort.MaxValue, msoBase18);
    MsoBase msoBase19 = (MsoBase) new MsofbtChildAnchor((MsoBase) null);
    MsoFactory.m_hashCodeToMSORecord.Add(61455, msoBase19);
  }
}
