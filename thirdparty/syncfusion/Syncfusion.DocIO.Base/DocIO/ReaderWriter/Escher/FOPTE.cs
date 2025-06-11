// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Escher.FOPTE
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Escher;

internal class FOPTE : BaseWordRecord
{
  private ushort m_pid;
  private ushort m_bid;
  private ushort m_complex;
  private uint m_op;
  private byte[] m_name;

  public ushort Pid
  {
    get => this.m_pid;
    set => this.m_pid = value;
  }

  public bool IsBid
  {
    get => this.m_bid == (ushort) 1;
    set
    {
      if (value)
        this.m_bid = (ushort) 1;
      else
        this.m_bid = (ushort) 0;
    }
  }

  public bool IsComplex
  {
    get => this.m_complex == (ushort) 1;
    set
    {
      if (value)
        this.m_complex = (ushort) 1;
      else
        this.m_complex = (ushort) 0;
    }
  }

  public uint Op
  {
    get => this.m_op;
    set => this.m_op = value;
  }

  public byte[] NameBytes
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  public int Read(Stream stream)
  {
    ushort num1 = BaseWordRecord.ReadUInt16(stream);
    this.m_pid = (ushort) ((uint) num1 & 16383U /*0x3FFF*/);
    this.m_bid = (ushort) (((int) num1 & 16384 /*0x4000*/) >> 14);
    this.m_complex = (ushort) (((int) num1 & 32768 /*0x8000*/) >> 15);
    this.m_op = BaseWordRecord.ReadUInt32(stream);
    int num2 = 6;
    if (this.IsComplex)
    {
      num2 += (int) this.m_op;
      this.m_name = new byte[(IntPtr) this.m_op];
    }
    return num2;
  }

  public void Write(Stream stream)
  {
    short num = (short) ((int) (short) ((int) (short) this.m_pid + (int) (short) ((int) this.m_bid << 14)) + (int) (short) ((int) this.m_complex << 15));
    BaseWordRecord.WriteUInt16(stream, (ushort) num);
    BaseWordRecord.WriteUInt32(stream, this.m_op);
  }
}
