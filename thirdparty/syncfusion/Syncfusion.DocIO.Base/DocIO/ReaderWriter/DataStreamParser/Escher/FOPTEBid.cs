// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.FOPTEBid
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class FOPTEBid : FOPTEBase
{
  private uint m_value;

  internal uint Value
  {
    get => this.m_value;
    set => this.m_value = value;
  }

  internal FOPTEBid(int id, bool isBid, uint value)
    : base(id, isBid)
  {
    this.m_value = value;
  }

  internal override void Write(Stream stream)
  {
    int num = this.Id | (this.IsBid ? 16384 /*0x4000*/ : 0);
    BaseWordRecord.WriteInt16(stream, (short) num);
    BaseWordRecord.WriteUInt32(stream, this.m_value);
  }

  internal override FOPTEBase Clone() => (FOPTEBase) new FOPTEBid(this.Id, this.IsBid, this.Value);
}
