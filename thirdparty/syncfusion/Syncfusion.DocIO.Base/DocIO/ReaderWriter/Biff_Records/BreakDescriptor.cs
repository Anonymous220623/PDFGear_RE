// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.BreakDescriptor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class BreakDescriptor : BaseWordRecord
{
  internal const int DEF_BKD_SIZE = 6;
  internal short m_ipgd;
  internal short m_dcpDepend;
  internal byte m_iCol;
  internal byte m_options;

  internal short Ipgd
  {
    get => this.m_ipgd;
    set => this.m_ipgd = value;
  }

  internal short DcpDepend
  {
    get => this.m_dcpDepend;
    set => this.m_dcpDepend = value;
  }

  internal byte Options
  {
    get => this.m_options;
    set => this.m_options = value;
  }

  internal bool TableBreak
  {
    get => ((int) this.m_options & 1) != 0;
    set
    {
      if (value)
        this.m_options |= (byte) 1;
      else
        this.m_options &= (byte) 254;
    }
  }

  internal bool ColumnBreak
  {
    get => ((int) this.m_options & 2) >> 1 != 0;
    set
    {
      if (value)
        this.m_options |= (byte) 2;
      else
        this.m_options &= (byte) 253;
    }
  }

  internal bool Marked
  {
    get => ((int) this.m_options & 4) >> 2 != 0;
    set
    {
      if (value)
        this.m_options |= (byte) 4;
      else
        this.m_options &= (byte) 251;
    }
  }

  internal bool Unk
  {
    get => ((int) this.m_options & 8) >> 3 != 0;
    set
    {
      if (value)
        this.m_options |= (byte) 8;
      else
        this.m_options &= (byte) 247;
    }
  }

  internal bool TextOverflow
  {
    get => ((int) this.m_options & 16 /*0x10*/) >> 4 != 0;
    set
    {
      if (value)
        this.m_options |= (byte) 16 /*0x10*/;
      else
        this.m_options &= (byte) 239;
    }
  }

  internal BreakDescriptor()
  {
  }

  internal BreakDescriptor(Stream stream) => this.Read(stream);

  internal void Read(Stream stream)
  {
    this.m_ipgd = BaseWordRecord.ReadInt16(stream);
    this.m_dcpDepend = BaseWordRecord.ReadInt16(stream);
    this.m_iCol = (byte) stream.ReadByte();
    this.m_options = (byte) stream.ReadByte();
  }

  internal void Write(Stream stream)
  {
    BaseWordRecord.WriteInt16(stream, this.m_ipgd);
    BaseWordRecord.WriteInt16(stream, this.m_dcpDepend);
    stream.WriteByte(this.m_iCol);
    stream.WriteByte(this.m_options);
  }
}
