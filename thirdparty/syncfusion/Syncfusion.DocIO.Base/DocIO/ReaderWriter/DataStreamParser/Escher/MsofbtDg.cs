// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.MsofbtDg
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class MsofbtDg : BaseEscherRecord
{
  private int m_shapeCount;
  private int m_spidLast;

  internal int DrawingId
  {
    get => this.Header.Instance;
    set => this.Header.Instance = value;
  }

  internal int ShapeCount
  {
    get => this.m_shapeCount;
    set => this.m_shapeCount = value;
  }

  internal int SpidLast
  {
    get => this.m_spidLast;
    set => this.m_spidLast = value;
  }

  internal MsofbtDg(WordDocument doc)
    : base(MSOFBT.msofbtDg, 0, doc)
  {
    this.m_shapeCount = 1;
  }

  protected override void ReadRecordData(Stream stream)
  {
    this.m_shapeCount = BaseWordRecord.ReadInt32(stream);
    this.m_spidLast = BaseWordRecord.ReadInt32(stream);
  }

  protected override void WriteRecordData(Stream stream)
  {
    BaseWordRecord.WriteInt32(stream, this.m_shapeCount);
    BaseWordRecord.WriteInt32(stream, this.m_spidLast);
  }

  internal override BaseEscherRecord Clone()
  {
    MsofbtDg msofbtDg = (MsofbtDg) this.MemberwiseClone();
    msofbtDg.m_doc = this.m_doc;
    return (BaseEscherRecord) msofbtDg;
  }
}
