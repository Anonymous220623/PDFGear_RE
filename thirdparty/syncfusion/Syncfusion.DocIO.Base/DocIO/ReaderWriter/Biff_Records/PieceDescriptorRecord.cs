// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.PieceDescriptorRecord
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class PieceDescriptorRecord : BaseWordRecord
{
  internal const int RECORD_SIZE = 8;
  private PieceDescriptorStructure m_field = new PieceDescriptorStructure();

  internal bool fNoParaLast
  {
    get => BaseWordRecord.GetBit((int) this.m_field.Options, 1);
    set
    {
      this.m_field.Options = (ushort) BaseWordRecord.SetBit((int) this.m_field.Options, 1, value);
    }
  }

  internal bool fPaphNil
  {
    get => BaseWordRecord.GetBit((int) this.m_field.Options, 2);
    set
    {
      this.m_field.Options = (ushort) BaseWordRecord.SetBit((int) this.m_field.Options, 2, value);
    }
  }

  internal bool fCopied
  {
    get => BaseWordRecord.GetBit((int) this.m_field.Options, 4);
    set
    {
      this.m_field.Options = (ushort) BaseWordRecord.SetBit((int) this.m_field.Options, 4, value);
    }
  }

  internal uint FileOffset
  {
    get => this.m_field.FileOffset;
    set => this.m_field.FileOffset = value;
  }

  internal PropertyModifierStructure PropertyModifier
  {
    get => this.m_field.PropertyModifier;
    set => this.m_field.PropertyModifier = value;
  }

  protected override IDataStructure UnderlyingStructure => (IDataStructure) this.m_field;

  internal override int Length => this.m_field.Length;

  internal PieceDescriptorRecord()
  {
  }

  internal PieceDescriptorRecord(byte[] arrData)
    : base(arrData)
  {
  }

  internal PieceDescriptorRecord(byte[] arrData, int iOffset)
    : base(arrData, iOffset)
  {
  }
}
