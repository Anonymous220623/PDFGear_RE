// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.MSODrawingGroupRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.MSODrawingGroup)]
[CLSCompliant(false)]
public class MSODrawingGroupRecord : BiffContinueRecordRaw, ICloneable, IDisposable
{
  private const int DEF_DATA_OFFSET = 0;
  protected byte[] m_tempData;
  protected List<MsoBase> m_arrStructures = new List<MsoBase>();

  public MSODrawingGroupRecord()
  {
  }

  protected override void OnDispose()
  {
    this.m_tempData = (byte[]) null;
    for (int index = 0; index < this.m_arrStructures.Count; ++index)
      this.m_arrStructures[index].Dispose();
  }

  public MSODrawingGroupRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public MSODrawingGroupRecord(int iReserve)
    : base(iReserve)
  {
  }

  public MsoBase[] Structures => this.m_arrStructures.ToArray();

  public List<MsoBase> StructuresList => this.m_arrStructures;

  public override bool NeedDataArray => true;

  protected virtual int StructuresOffset => 0;

  public override void ParseStructure()
  {
    if (this.m_extractor == null)
      return;
    this.AddContinueRecordType(this.TypeCode);
    base.ParseStructure();
    this.m_tempData = new byte[this.m_data.Length];
    this.m_data.CopyTo((Array) this.m_tempData, 0);
    this.ParseData();
  }

  protected virtual void ParseData()
  {
    MemoryStream memoryStream = new MemoryStream(this.m_data);
    memoryStream.Position = (long) this.StructuresOffset;
    int length = this.m_data.Length;
    while (memoryStream.Position < (long) length)
      this.m_arrStructures.Add(MsoFactory.CreateMsoRecord((MsoBase) null, (Stream) memoryStream));
  }

  public override void InfillInternalData(ExcelVersion version)
  {
    if (this.m_arrStructures.Count <= 0)
      return;
    this.m_arrContinuePos.Clear();
    int iStartIndex;
    Stream dataList = this.CreateDataList(out iStartIndex);
    this.FillDataList(dataList, iStartIndex);
    int length1 = (int) dataList.Length;
    this.m_tempData = ((MemoryStream) dataList).GetBuffer();
    this.m_iLength = length1 > this.MaximumRecordSize ? this.MaximumRecordSize : length1;
    this.AutoGrowData = true;
    this.SetBytes(0, this.m_tempData, 0, this.m_iLength);
    base.InfillInternalData(version);
    if (length1 <= this.MaximumRecordSize)
      return;
    int iLength = this.m_iLength;
    int length2 = length1 - iLength;
    this.Builder.AppendBytes(this.m_tempData, iLength, length2);
    this.m_iLength = this.Builder.Total;
  }

  protected virtual Stream CreateDataList(out int iStartIndex)
  {
    iStartIndex = 0;
    return (Stream) new MemoryStream();
  }

  protected void FillDataList(Stream stream, int iStartIndex)
  {
    int count = this.m_arrStructures.Count;
    for (int index = 0; index < count; ++index)
      this.m_arrStructures[index].FillArray(stream);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    if (this.NeedInfill)
    {
      this.InfillInternalData(version);
      this.NeedInfill = false;
    }
    return this.m_iLength;
  }

  public void AddStructure(MsoBase item) => this.m_arrStructures.Add(item);

  protected override ContinueRecordBuilder CreateBuilder()
  {
    ContinueRecordBuilder builder = base.CreateBuilder();
    builder.FirstContinueType = TBIFFRecord.MSODrawingGroup;
    return builder;
  }

  public new object Clone()
  {
    MSODrawingGroupRecord drawingGroupRecord = (MSODrawingGroupRecord) base.Clone();
    this.m_arrStructures = new List<MsoBase>((IEnumerable<MsoBase>) drawingGroupRecord.m_arrStructures);
    return (object) drawingGroupRecord;
  }
}
