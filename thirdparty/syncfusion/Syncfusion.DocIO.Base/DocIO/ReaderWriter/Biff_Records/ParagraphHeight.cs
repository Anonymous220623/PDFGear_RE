// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.ParagraphHeight
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class ParagraphHeight : BaseWordRecord
{
  private const int DEF_BIT_SPARE = 0;
  private const int DEF_BIT_VALID = 1;
  private const int DEF_BIT_DIFF_LINES = 2;
  private const int DEF_BYTE_LINES_COUNT = 1;
  private const int DEF_MASK_NEXT_ROW_HINT = 65532;
  private const int DEF_START_NEXT_ROW_HINT = 2;
  private const int DEF_RECORD_SIZE = 13;
  private ParagraphHeightStructure m_structure = new ParagraphHeightStructure();

  internal ParagraphHeightStructure Structure => this.m_structure;

  internal bool IsSpare
  {
    get => BaseWordRecord.GetBit(this.m_structure.Options, 0);
    set => this.m_structure.Options = BaseWordRecord.SetBit(this.m_structure.Options, 0, value);
  }

  internal bool IsValid
  {
    get => !BaseWordRecord.GetBit(this.m_structure.Options, 1);
    set => this.m_structure.Options = BaseWordRecord.SetBit(this.m_structure.Options, 1, !value);
  }

  internal bool IsDifferentLines
  {
    get => BaseWordRecord.GetBit(this.m_structure.Options, 2);
    set => this.m_structure.Options = BaseWordRecord.SetBit(this.m_structure.Options, 2, value);
  }

  internal byte LinesCount
  {
    get => BitConverter.GetBytes(this.m_structure.Options)[1];
    set
    {
      byte[] bytes = BitConverter.GetBytes(this.m_structure.Options);
      bytes[1] = value;
      this.m_structure.Options = BitConverter.ToUInt32(bytes, 0);
    }
  }

  internal int Width
  {
    get => this.m_structure.Width;
    set => this.m_structure.Width = value;
  }

  internal int Height
  {
    get => this.m_structure.Height;
    set => this.m_structure.Height = value;
  }

  internal int NextRowHint
  {
    get => (int) BaseWordRecord.GetBitsByMask(this.m_structure.Options, 65532, 2);
    set
    {
      this.m_structure.Options = BaseWordRecord.SetBitsByMask(this.m_structure.Options, 65532, value << 2);
    }
  }

  protected override IDataStructure UnderlyingStructure => (IDataStructure) this.m_structure;

  internal override int Length => this.m_structure.Length;

  internal ParagraphHeight()
  {
  }

  internal ParagraphHeight(byte[] arrData)
    : base(arrData)
  {
  }

  internal ParagraphHeight(Stream stream)
  {
    byte[] numArray = new byte[13];
    stream.Read(numArray, 0, 13);
    this.Parse(numArray);
  }

  internal void Parse(byte[] arrData, int iOffset) => this.m_structure.Parse(arrData, iOffset);

  internal new int Save(byte[] arrData, int iOffset) => this.m_structure.Save(arrData, iOffset);
}
