// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.FOPTEComplex
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class FOPTEComplex : FOPTEBase
{
  private int m_dataLength;
  private byte[] m_data;

  internal byte[] Value
  {
    get => this.m_data;
    set => this.m_data = value;
  }

  internal FOPTEComplex(int id, bool isBid, int valueLength)
    : base(id, isBid)
  {
    this.m_dataLength = valueLength;
  }

  internal void ReadData(Stream stream)
  {
    this.m_data = new byte[this.m_dataLength];
    stream.Read(this.m_data, 0, this.m_dataLength);
  }

  internal override void Write(Stream stream)
  {
    int num = this.Id | (this.IsBid ? 16384 /*0x4000*/ : 0) | 32768 /*0x8000*/;
    BaseWordRecord.WriteInt16(stream, (short) num);
    BaseWordRecord.WriteInt32(stream, this.m_data.Length);
  }

  internal void WriteData(Stream stream) => stream.Write(this.m_data, 0, this.m_data.Length);

  internal override FOPTEBase Clone()
  {
    FOPTEComplex fopteComplex = new FOPTEComplex(this.Id, this.IsBid, this.m_dataLength);
    fopteComplex.m_data = new byte[this.m_dataLength];
    this.m_data.CopyTo((Array) fopteComplex.m_data, 0);
    return (FOPTEBase) fopteComplex;
  }
}
