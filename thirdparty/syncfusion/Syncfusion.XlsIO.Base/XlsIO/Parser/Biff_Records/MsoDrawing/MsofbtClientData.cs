// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.MsofbtClientData
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;

[CLSCompliant(false)]
[Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.MsoDrawing(MsoRecords.msofbtClientData)]
public class MsofbtClientData : MsoBase
{
  private List<BiffRecordRaw> m_arrAdditionalData = new List<BiffRecordRaw>();

  public OBJRecord ObjectRecord
  {
    get
    {
      return this.m_arrAdditionalData.Count > 0 && this.m_arrAdditionalData[0] is OBJRecord ? (OBJRecord) this.m_arrAdditionalData[0] : (OBJRecord) null;
    }
    set => throw new NotImplementedException();
  }

  public BiffRecordRaw[] AdditionalData
  {
    get
    {
      return this.m_arrAdditionalData == null ? (BiffRecordRaw[]) null : this.m_arrAdditionalData.ToArray();
    }
  }

  public MsofbtClientData(MsoBase parent)
    : base(parent)
  {
  }

  public MsofbtClientData(MsoBase parent, byte[] data, int iOffset)
    : base(parent, data, iOffset)
  {
  }

  public MsofbtClientData(
    MsoBase parent,
    byte[] data,
    int iOffset,
    GetNextMsoDrawingData dataGetter)
    : base(parent, data, iOffset, dataGetter)
  {
    BiffRecordRaw[] collection = dataGetter();
    if (collection == null)
      throw new ArgumentException("Additional data can't be null");
    this.m_arrAdditionalData.Clear();
    this.m_arrAdditionalData.AddRange((IEnumerable<BiffRecordRaw>) collection);
    for (int index = this.m_arrAdditionalData.Count - 1; index >= 0 && this.m_arrAdditionalData[index] is NoteRecord; --index)
      this.m_arrAdditionalData.RemoveAt(index);
  }

  public override void InfillInternalData(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords)
  {
    this.m_iLength = 0;
    if (arrBreaks == null || arrRecords == null)
      return;
    arrBreaks.Add(this.m_iLength + iOffset);
    arrRecords.Add(this.m_arrAdditionalData);
  }

  public override void ParseStructure(Stream stream)
  {
  }

  protected override object InternalClone()
  {
    MsofbtClientData msofbtClientData = (MsofbtClientData) base.InternalClone();
    msofbtClientData.m_arrAdditionalData = CloneUtils.CloneCloneable(this.m_arrAdditionalData);
    return (object) msofbtClientData;
  }

  public override void UpdateNextMsoDrawingData()
  {
    BiffRecordRaw[] collection = this.DataGetter();
    if (collection == null)
      throw new ArgumentException("Additional data can't be null");
    this.m_arrAdditionalData.Clear();
    this.m_arrAdditionalData.AddRange((IEnumerable<BiffRecordRaw>) collection);
    for (int index = this.m_arrAdditionalData.Count - 1; index >= 0 && this.m_arrAdditionalData[index] is NoteRecord; --index)
      this.m_arrAdditionalData.RemoveAt(index);
  }

  public void AddRecord(BiffRecordRaw record) => this.m_arrAdditionalData.Add(record);

  public void AddRecordRange(ICollection<BiffRecordRaw> records)
  {
    this.m_arrAdditionalData.AddRange((IEnumerable<BiffRecordRaw>) records);
  }

  public void AddRecordRange(IList records)
  {
    int index = 0;
    for (int count = records.Count; index < count; ++index)
      this.m_arrAdditionalData.Add(records[index] as BiffRecordRaw);
  }
}
