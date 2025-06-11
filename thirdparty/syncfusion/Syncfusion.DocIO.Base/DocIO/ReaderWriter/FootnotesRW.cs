// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.FootnotesRW
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class FootnotesRW : SubDocumentRW
{
  internal int InitialDescriptorNumber
  {
    get => this.m_iInitialDesctiptorNumber;
    set => this.m_iInitialDesctiptorNumber = value;
  }

  internal FootnotesRW()
  {
  }

  internal FootnotesRW(Stream stream, Fib fib)
    : base(stream, fib)
  {
  }

  internal void AddReferense(int pos, bool autoNumbered)
  {
    this.m_refPositions.Add(pos);
    if (autoNumbered)
      ++this.m_autoCount;
    this.m_descrFootEndntes.Add(autoNumbered ? (short) this.m_autoCount : (short) 0);
  }

  internal int GetDescriptor(int index) => (int) this.m_descrFootEndntes[index];

  protected override void WriteTxtPositions()
  {
    if (this.m_txtPositions.Count <= 0)
      return;
    this.m_fib.FibRgFcLcb97FcPlcffndTxt = (uint) this.m_writer.BaseStream.Position;
    this.WriteTxtPositionsBase();
    this.m_fib.FibRgFcLcb97LcbPlcffndTxt = (uint) ((ulong) this.m_writer.BaseStream.Position - (ulong) this.m_fib.FibRgFcLcb97FcPlcffndTxt);
  }

  protected override void WriteDescriptors()
  {
    if (this.m_descrFootEndntes.Count <= 0)
      return;
    this.m_fib.FibRgFcLcb97FcPlcffndRef = (uint) this.m_writer.BaseStream.Position;
    this.WriteRefPositions(this.m_endReference);
    foreach (short descrFootEndnte in this.m_descrFootEndntes)
      this.m_writer.Write(descrFootEndnte);
    this.m_fib.FibRgFcLcb97LcbPlcffndRef = (uint) ((ulong) this.m_writer.BaseStream.Position - (ulong) this.m_fib.FibRgFcLcb97FcPlcffndRef);
  }

  protected override void ReadTxtPositions()
  {
    int lcb97LcbPlcffndTxt = (int) this.m_fib.FibRgFcLcb97LcbPlcffndTxt;
    if (lcb97LcbPlcffndTxt <= 0)
      return;
    this.m_reader.BaseStream.Position = (long) this.m_fib.FibRgFcLcb97FcPlcffndTxt;
    this.ReadTxtPositions(lcb97LcbPlcffndTxt / 4);
  }

  protected override void ReadDescriptors()
  {
    if (this.m_fib.FibRgFcLcb97LcbPlcffndRef <= 0U)
      return;
    this.m_reader.BaseStream.Position = (long) this.m_fib.FibRgFcLcb97FcPlcffndRef;
    int lcb97LcbPlcffndRef = (int) this.m_fib.FibRgFcLcb97LcbPlcffndRef;
    this.m_reader.ReadBytes((int) this.m_fib.FibRgFcLcb97LcbPlcffndRef);
    this.m_reader.BaseStream.Position = (long) this.m_fib.FibRgFcLcb97FcPlcffndRef;
    this.ReadDescriptors((int) this.m_fib.FibRgFcLcb97LcbPlcffndRef, 2);
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
