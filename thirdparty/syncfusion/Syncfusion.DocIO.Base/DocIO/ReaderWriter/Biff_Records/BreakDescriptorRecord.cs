// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.BreakDescriptorRecord
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class BreakDescriptorRecord : BaseWordRecord
{
  private const int DEF_BIT_TABLE_BREAK = 0;
  private const int DEF_BIT_COLUMN_BREAK = 1;
  private const int DEF_BIT_MARKED = 2;
  private const int DEF_BIT_LIMIT_VALID = 3;
  internal const int DEF_RECORD_SIZE = 6;
  private BreakDescriptorStructure m_field = new BreakDescriptorStructure();

  internal short PageDescriptorIndex
  {
    get => this.m_field.ipgd;
    set => this.m_field.ipgd = value;
  }

  internal short itxbxs
  {
    get => this.m_field.itxbxs;
    set => this.m_field.itxbxs = value;
  }

  internal short CharPosNumber
  {
    get => this.m_field.dcpDepend;
    set => this.m_field.dcpDepend = value;
  }

  internal ushort Options => (ushort) this.m_field.Options;

  internal byte ColumnIndex
  {
    get => this.m_field.iCol;
    set => this.m_field.iCol = value;
  }

  internal bool IsTableBreak
  {
    get => BaseWordRecord.GetBit((int) this.Options, 0);
    set
    {
      this.m_field.Options = (byte) BaseWordRecord.SetBit((int) this.m_field.Options, 0, value);
    }
  }

  internal bool IsColumnBreak
  {
    get => BaseWordRecord.GetBit((int) this.Options, 1);
    set
    {
      this.m_field.Options = (byte) BaseWordRecord.SetBit((int) this.m_field.Options, 1, value);
    }
  }

  internal bool IsMarked
  {
    get => BaseWordRecord.GetBit((int) this.Options, 2);
    set
    {
      this.m_field.Options = (byte) BaseWordRecord.SetBit((int) this.m_field.Options, 1, value);
    }
  }

  internal bool IsLimitValid
  {
    get => BaseWordRecord.GetBit((int) this.Options, 3);
    set
    {
      this.m_field.Options = (byte) BaseWordRecord.SetBit((int) this.m_field.Options, 3, value);
    }
  }

  internal bool IsTextOverflow
  {
    get => BaseWordRecord.GetBit((int) this.Options, 2);
    set
    {
      this.m_field.Options = (byte) BaseWordRecord.SetBit((int) this.m_field.Options, 1, value);
    }
  }

  protected override IDataStructure UnderlyingStructure => (IDataStructure) this.m_field;

  internal override int Length => 6;

  internal BreakDescriptorRecord()
  {
  }

  internal BreakDescriptorRecord(byte[] data)
    : base(data)
  {
  }

  internal BreakDescriptorRecord(byte[] arrData, int iOffset)
    : base(arrData, iOffset)
  {
  }

  internal BreakDescriptorRecord(byte[] arrData, int iOffset, int iCount)
    : base(arrData, iOffset, iCount)
  {
  }

  internal BreakDescriptorRecord(Stream stream, int iCount)
    : base(stream, iCount)
  {
  }
}
