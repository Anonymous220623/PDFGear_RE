// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.MsofbtGeneral
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class MsofbtGeneral : BaseEscherRecord
{
  private byte[] m_data;

  internal byte[] Data
  {
    get => this.m_data;
    set => this.m_data = value;
  }

  internal MsofbtGeneral(WordDocument doc)
    : base(doc)
  {
  }

  protected override void ReadRecordData(Stream stream)
  {
    this.m_data = new byte[this.Header.Length];
    stream.Read(this.m_data, 0, this.Header.Length);
  }

  protected override void WriteRecordData(Stream stream)
  {
    stream.Write(this.m_data, 0, this.m_data.Length);
  }

  internal override BaseEscherRecord Clone()
  {
    MsofbtGeneral msofbtGeneral = new MsofbtGeneral(this.m_doc);
    msofbtGeneral.m_data = new byte[this.m_data.Length];
    this.m_data.CopyTo((Array) msofbtGeneral.m_data, 0);
    msofbtGeneral.Header = this.Header.Clone();
    msofbtGeneral.m_doc = this.m_doc;
    return (BaseEscherRecord) msofbtGeneral;
  }

  internal override void Close()
  {
    base.Close();
    this.m_data = (byte[]) null;
  }
}
