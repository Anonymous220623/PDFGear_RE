// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.MsofbtSpgr
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class MsofbtSpgr : BaseEscherRecord
{
  private int m_rectLeft;
  private int m_rectTop;
  private int m_rectRight;
  private int m_rectBottom;

  internal MsofbtSpgr(WordDocument doc)
    : base(MSOFBT.msofbtSpgr, 1, doc)
  {
  }

  protected override void ReadRecordData(Stream stream)
  {
    this.m_rectLeft = BaseWordRecord.ReadInt32(stream);
    this.m_rectTop = BaseWordRecord.ReadInt32(stream);
    this.m_rectRight = BaseWordRecord.ReadInt32(stream);
    this.m_rectBottom = BaseWordRecord.ReadInt32(stream);
  }

  protected override void WriteRecordData(Stream stream)
  {
    BaseWordRecord.WriteInt32(stream, this.m_rectLeft);
    BaseWordRecord.WriteInt32(stream, this.m_rectTop);
    BaseWordRecord.WriteInt32(stream, this.m_rectRight);
    BaseWordRecord.WriteInt32(stream, this.m_rectBottom);
  }

  internal override BaseEscherRecord Clone()
  {
    MsofbtSpgr msofbtSpgr = (MsofbtSpgr) this.MemberwiseClone();
    msofbtSpgr.m_doc = this.m_doc;
    return (BaseEscherRecord) msofbtSpgr;
  }
}
