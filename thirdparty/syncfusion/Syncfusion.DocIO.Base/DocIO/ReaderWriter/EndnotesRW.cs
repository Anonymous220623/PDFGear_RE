// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.EndnotesRW
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class EndnotesRW : FootnotesRW
{
  internal EndnotesRW()
  {
  }

  internal EndnotesRW(Stream stream, Fib fib)
    : base(stream, fib)
  {
  }

  protected override void WriteTxtPositions()
  {
    if (this.m_txtPositions.Count <= 0)
      return;
    this.m_fib.FibRgFcLcb97FcPlcfendTxt = (uint) this.m_writer.BaseStream.Position;
    this.WriteTxtPositionsBase();
    this.m_fib.FibRgFcLcb97LcbPlcfendTxt = (uint) ((ulong) this.m_writer.BaseStream.Position - (ulong) this.m_fib.FibRgFcLcb97FcPlcfendTxt);
  }

  protected override void WriteDescriptors()
  {
    if (this.m_descrFootEndntes.Count <= 0)
      return;
    this.m_fib.FibRgFcLcb97FcPlcfendRef = (uint) this.m_writer.BaseStream.Position;
    this.WriteRefPositions(this.m_endReference);
    int index = 0;
    for (int count = this.m_descrFootEndntes.Count; index < count; ++index)
      this.m_writer.Write(this.m_descrFootEndntes[index]);
    this.m_fib.FibRgFcLcb97LcbPlcfendRef = (uint) ((ulong) this.m_writer.BaseStream.Position - (ulong) this.m_fib.FibRgFcLcb97FcPlcfendRef);
  }

  protected override void ReadTxtPositions()
  {
    int lcb97LcbPlcfendTxt = (int) this.m_fib.FibRgFcLcb97LcbPlcfendTxt;
    if (lcb97LcbPlcfendTxt <= 0)
      return;
    this.m_reader.BaseStream.Position = (long) this.m_fib.FibRgFcLcb97FcPlcfendTxt;
    this.ReadTxtPositions(lcb97LcbPlcfendTxt / 4);
  }

  protected override void ReadDescriptors()
  {
    if (this.m_fib.FibRgFcLcb97LcbPlcfendRef <= 0U)
      return;
    this.m_reader.BaseStream.Position = (long) this.m_fib.FibRgFcLcb97FcPlcfendRef;
    int lcb97LcbPlcfendRef = (int) this.m_fib.FibRgFcLcb97LcbPlcfendRef;
    this.m_reader.ReadBytes((int) this.m_fib.FibRgFcLcb97LcbPlcfendRef);
    this.m_reader.BaseStream.Position = (long) this.m_fib.FibRgFcLcb97FcPlcfendRef;
    this.ReadDescriptors((int) this.m_fib.FibRgFcLcb97LcbPlcfendRef, 2);
    base.ReadDescriptors();
  }

  protected override void ReadDescriptor(BinaryReader reader, int pos, int posNext)
  {
    if (reader.BaseStream.Position >= reader.BaseStream.Length)
      return;
    this.m_descrFootEndntes.Add(reader.ReadInt16());
    base.ReadDescriptor(reader, pos, posNext);
  }
}
