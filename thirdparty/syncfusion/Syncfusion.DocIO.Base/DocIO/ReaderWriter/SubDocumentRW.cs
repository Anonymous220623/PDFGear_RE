// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.SubDocumentRW
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal abstract class SubDocumentRW
{
  protected Fib m_fib;
  protected List<int> m_txtPositions;
  protected List<int> m_refPositions;
  protected List<AnnotationDescriptor> m_descriptorsAnnot;
  protected List<short> m_descrFootEndntes;
  protected BinaryReader m_reader;
  protected BinaryWriter m_writer;
  private int m_endRefPosition = -1;
  protected int m_iCount;
  protected int m_iInitialDesctiptorNumber;
  protected int m_autoCount;
  protected int m_endReference;
  internal int m_footEndnoteRefIndex;

  internal int Count => this.m_iCount;

  internal SubDocumentRW(Stream stream, Fib fib)
    : this()
  {
    this.Read(stream, fib);
  }

  internal SubDocumentRW() => this.Init();

  internal bool HasReference(int reference)
  {
    if (!this.m_refPositions.Contains(reference))
      return false;
    this.m_footEndnoteRefIndex = this.m_refPositions.IndexOf(reference);
    return true;
  }

  internal bool HasReference(int startPosition, int endPosition, ref int textLength)
  {
    int num = this.m_footEndnoteRefIndex + 1 < this.m_refPositions.Count ? this.m_refPositions[this.m_footEndnoteRefIndex + 1] : -1;
    if (num == -1 || num <= startPosition || num >= endPosition)
      return false;
    textLength = num - startPosition;
    return true;
  }

  internal bool HasPosition(int position) => this.m_txtPositions.Contains(position);

  internal virtual void Read(Stream stream, Fib fib)
  {
    this.m_fib = fib;
    this.m_reader = new BinaryReader(stream);
    this.ReadTxtPositions();
    this.ReadDescriptors();
  }

  internal virtual void Write(Stream stream, Fib fib)
  {
    this.m_fib = fib;
    this.m_writer = new BinaryWriter(stream);
    this.m_endReference = this.m_fib.CcpText + this.m_fib.CcpFtn + this.m_fib.CcpHdd + this.m_fib.CcpAtn + this.m_fib.CcpEdn + this.m_fib.CcpTxbx + this.m_fib.CcpHdrTxbx;
    this.WriteTxtPositions();
    this.WriteDescriptors();
  }

  internal virtual void AddTxtPosition(int position) => this.m_txtPositions.Add(position);

  internal virtual int GetTxtPosition(int index)
  {
    return this.m_txtPositions.Count != 0 ? this.m_txtPositions[index] : 0;
  }

  internal virtual void Close()
  {
    if (this.m_fib != null)
    {
      this.m_fib.Close();
      this.m_fib = (Fib) null;
    }
    if (this.m_txtPositions != null)
    {
      this.m_txtPositions.Clear();
      this.m_txtPositions = (List<int>) null;
    }
    if (this.m_refPositions != null)
    {
      this.m_refPositions.Clear();
      this.m_refPositions = (List<int>) null;
    }
    if (this.m_descriptorsAnnot != null)
    {
      this.m_descriptorsAnnot.Clear();
      this.m_descriptorsAnnot = (List<AnnotationDescriptor>) null;
    }
    if (this.m_descrFootEndntes != null)
    {
      this.m_descrFootEndntes.Clear();
      this.m_descrFootEndntes = (List<short>) null;
    }
    if (this.m_reader != null)
    {
      this.m_reader.Close();
      this.m_reader = (BinaryReader) null;
    }
    if (this.m_writer == null)
      return;
    this.m_writer.Close();
    this.m_writer = (BinaryWriter) null;
  }

  protected virtual void ReadDescriptors()
  {
    if (this.m_endRefPosition == -1)
      return;
    this.AddRefPosition(this.m_endRefPosition);
  }

  protected abstract void WriteDescriptors();

  protected void ReadDescriptors(int length, int size)
  {
    PosStructReader.Read(this.m_reader, length, size, new PosStructReaderDelegate(this.ReadDescriptor));
  }

  protected void AddRefPosition(int position) => this.m_refPositions.Add(position);

  protected virtual void Init()
  {
    this.m_txtPositions = new List<int>();
    this.m_refPositions = new List<int>();
    this.m_descriptorsAnnot = new List<AnnotationDescriptor>();
    this.m_descrFootEndntes = new List<short>();
  }

  protected abstract void ReadTxtPositions();

  protected void ReadTxtPositions(int count)
  {
    this.m_iCount = count - 1;
    for (int index = 0; index < count; ++index)
      this.AddTxtPosition(this.m_reader.ReadInt32());
  }

  protected void WriteTxtPositionsBase()
  {
    foreach (int txtPosition in this.m_txtPositions)
      this.m_writer.Write(txtPosition);
  }

  protected abstract void WriteTxtPositions();

  protected virtual void WriteRefPositions(int endPos)
  {
    foreach (int refPosition in this.m_refPositions)
      this.m_writer.Write(refPosition);
    if (this.m_refPositions.Count <= 0)
      return;
    this.m_writer.Write(endPos);
  }

  protected virtual void ReadDescriptor(BinaryReader reader, int pos, int posNext)
  {
    this.AddRefPosition(pos);
    this.m_endRefPosition = posNext;
  }
}
