// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.MsofbtClientTextbox
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class MsofbtClientTextbox : BaseEscherRecord
{
  private int m_txId;

  internal int Txid
  {
    get => this.m_txId;
    set => this.m_txId = value;
  }

  public MsofbtClientTextbox(WordDocument doc)
    : base(doc)
  {
    this.Header.Type = MSOFBT.msofbtClientTextbox;
  }

  protected override void ReadRecordData(Stream stream)
  {
    this.m_txId = BaseWordRecord.ReadInt32(stream);
  }

  protected override void WriteRecordData(Stream stream)
  {
    BaseWordRecord.WriteInt32(stream, this.m_txId);
  }

  internal override BaseEscherRecord Clone()
  {
    MsofbtClientTextbox msofbtClientTextbox = (MsofbtClientTextbox) this.MemberwiseClone();
    msofbtClientTextbox.m_doc = this.m_doc;
    return (BaseEscherRecord) msofbtClientTextbox;
  }
}
