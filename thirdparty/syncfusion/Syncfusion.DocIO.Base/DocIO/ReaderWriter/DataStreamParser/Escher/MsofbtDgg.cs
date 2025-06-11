// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.MsofbtDgg
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class MsofbtDgg : BaseEscherRecord
{
  private int m_spidMax;
  private int m_shapeCount;
  private int m_drawingCount;
  private List<FIDCL> m_filCls;

  internal int DrawingCount
  {
    get => this.m_drawingCount;
    set => this.m_drawingCount = value;
  }

  internal List<FIDCL> Fidcls => this.m_filCls;

  internal int ShapeCount
  {
    get => this.m_shapeCount;
    set => this.m_shapeCount = value;
  }

  internal int SpidMax
  {
    get => this.m_spidMax;
    set => this.m_spidMax = value;
  }

  internal MsofbtDgg(WordDocument doc)
    : base(MSOFBT.msofbtDgg, 0, doc)
  {
    this.m_filCls = new List<FIDCL>();
    this.m_shapeCount = 1;
  }

  protected override void ReadRecordData(Stream stream)
  {
    this.m_spidMax = BaseWordRecord.ReadInt32(stream);
    int num = BaseWordRecord.ReadInt32(stream) - 1;
    this.m_shapeCount = BaseWordRecord.ReadInt32(stream);
    this.m_drawingCount = BaseWordRecord.ReadInt32(stream);
    for (int index = 0; index < num; ++index)
      this.m_filCls.Add(new FIDCL(stream));
  }

  protected override void WriteRecordData(Stream stream)
  {
    BaseWordRecord.WriteInt32(stream, this.m_spidMax);
    BaseWordRecord.WriteInt32(stream, this.m_filCls.Count + 1);
    BaseWordRecord.WriteInt32(stream, this.m_shapeCount);
    BaseWordRecord.WriteInt32(stream, this.m_drawingCount);
    for (int index = 0; index < this.m_filCls.Count; ++index)
      this.m_filCls[index].Write(stream);
  }

  internal override BaseEscherRecord Clone()
  {
    MsofbtDgg msofbtDgg = (MsofbtDgg) this.MemberwiseClone();
    msofbtDgg.m_filCls = new List<FIDCL>(this.m_filCls.Count);
    int index = 0;
    for (int count = this.m_filCls.Count; index < count; ++index)
      msofbtDgg.m_filCls.Add(this.m_filCls[index]);
    msofbtDgg.m_doc = this.m_doc;
    return (BaseEscherRecord) msofbtDgg;
  }
}
