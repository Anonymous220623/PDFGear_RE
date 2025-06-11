// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.Copts
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
internal class Copts
{
  private uint m_flagsA = 4126933000;
  private byte m_flagsg;
  private DOPDescriptor m_dopBase;

  internal Copts80 Copts80 => this.m_dopBase.Dop95.Copts80;

  internal bool SpLayoutLikeWW8
  {
    get => ((int) this.m_flagsA & 1) != 0;
    set => this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294967294U) | (value ? 1L : 0L));
  }

  internal bool FtnLayoutLikeWW8
  {
    get => (this.m_flagsA & 2U) >> 1 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294967293U) | (long) ((value ? 1 : 0) << 1));
    }
  }

  internal bool DontUseHTMLParagraphAutoSpacing
  {
    get => (this.m_flagsA & 4U) >> 2 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294967291U) | (long) ((value ? 1 : 0) << 2));
    }
  }

  internal bool DontAdjustLineHeightInTable
  {
    get => (this.m_flagsA & 8U) >> 3 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294967287U) | (long) ((value ? 1 : 0) << 3));
    }
  }

  internal bool ForgetLastTabAlign
  {
    get => (this.m_flagsA & 16U /*0x10*/) >> 4 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294967279U) | (long) ((value ? 1 : 0) << 4));
    }
  }

  internal bool UseAutospaceForFullWidthAlpha
  {
    get => (this.m_flagsA & 32U /*0x20*/) >> 5 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294967263U) | (long) ((value ? 1 : 0) << 5));
    }
  }

  internal bool AlignTablesRowByRow
  {
    get => (this.m_flagsA & 64U /*0x40*/) >> 6 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294967231U) | (long) ((value ? 1 : 0) << 6));
    }
  }

  internal bool LayoutRawTableWidth
  {
    get => (this.m_flagsA & 128U /*0x80*/) >> 7 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294967167U) | (long) ((value ? 1 : 0) << 7));
    }
  }

  internal bool LayoutTableRowsApart
  {
    get => (this.m_flagsA & 256U /*0x0100*/) >> 8 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294967039U) | (long) ((value ? 1 : 0) << 8));
    }
  }

  internal bool UseWord97LineBreakingRules
  {
    get => (this.m_flagsA & 512U /*0x0200*/) >> 9 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294966783U) | (long) ((value ? 1 : 0) << 9));
    }
  }

  internal bool DontBreakWrappedTables
  {
    get => (this.m_flagsA & 1024U /*0x0400*/) >> 10 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294966271U) | (long) ((value ? 1 : 0) << 10));
    }
  }

  internal bool DontSnapToGridInCell
  {
    get => (this.m_flagsA & 2048U /*0x0800*/) >> 11 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294965247U) | (long) ((value ? 1 : 0) << 11));
    }
  }

  internal bool DontAllowFieldEndSelect
  {
    get => (this.m_flagsA & 4096U /*0x1000*/) >> 12 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294963199U) | (long) ((value ? 1 : 0) << 12));
    }
  }

  internal bool ApplyBreakingRules
  {
    get => (this.m_flagsA & 8192U /*0x2000*/) >> 13 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294959103U) | (long) ((value ? 1 : 0) << 13));
    }
  }

  internal bool DontWrapTextWithPunct
  {
    get => (this.m_flagsA & 16384U /*0x4000*/) >> 14 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294950911U) | (long) ((value ? 1 : 0) << 14));
    }
  }

  internal bool DontUseAsianBreakRules
  {
    get => (this.m_flagsA & 32768U /*0x8000*/) >> 15 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294934527U) | (long) ((value ? 1 : 0) << 15));
    }
  }

  internal bool UseWord2002TableStyleRules
  {
    get => (this.m_flagsA & 65536U /*0x010000*/) >> 16 /*0x10*/ != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294901759U) | (long) ((value ? 1 : 0) << 16 /*0x10*/));
    }
  }

  internal bool GrowAutoFit
  {
    get => (this.m_flagsA & 131072U /*0x020000*/) >> 17 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294836223U) | (long) ((value ? 1 : 0) << 17));
    }
  }

  internal bool UseNormalStyleForList
  {
    get => (this.m_flagsA & 262144U /*0x040000*/) >> 18 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294705151U) | (long) ((value ? 1 : 0) << 18));
    }
  }

  internal bool DontUseIndentAsNumberingTabStop
  {
    get => (this.m_flagsA & 524288U /*0x080000*/) >> 19 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4294443007U) | (long) ((value ? 1 : 0) << 19));
    }
  }

  internal bool FELineBreak11
  {
    get => (this.m_flagsA & 1048576U /*0x100000*/) >> 20 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4293918719U) | (long) ((value ? 1 : 0) << 20));
    }
  }

  internal bool AllowSpaceOfSameStyleInTable
  {
    get => (this.m_flagsA & 2097152U /*0x200000*/) >> 21 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4292870143U) | (long) ((value ? 1 : 0) << 21));
    }
  }

  internal bool WW11IndentRules
  {
    get => (this.m_flagsA & 4194304U /*0x400000*/) >> 22 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4290772991U) | (long) ((value ? 1 : 0) << 22));
    }
  }

  internal bool DontAutofitConstrainedTables
  {
    get => (this.m_flagsA & 8388608U /*0x800000*/) >> 23 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4286578687U) | (long) ((value ? 1 : 0) << 23));
    }
  }

  internal bool AutofitLikeWW11
  {
    get => (this.m_flagsA & 16777216U /*0x01000000*/) >> 24 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4278190079U) | (long) ((value ? 1 : 0) << 24));
    }
  }

  internal bool UnderlineTabInNumList
  {
    get => (this.m_flagsA & 33554432U /*0x02000000*/) >> 25 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4261412863U) | (long) ((value ? 1 : 0) << 25));
    }
  }

  internal bool HangulWidthLikeWW11
  {
    get => (this.m_flagsA & 67108864U /*0x04000000*/) >> 26 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4227858431U) | (long) ((value ? 1 : 0) << 26));
    }
  }

  internal bool SplitPgBreakAndParaMark
  {
    get => (this.m_flagsA & 134217728U /*0x08000000*/) >> 27 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4160749567U) | (long) ((value ? 1 : 0) << 27));
    }
  }

  internal bool DontVertAlignCellWithSp
  {
    get => (this.m_flagsA & 268435456U /*0x10000000*/) >> 28 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 4026531839U /*0xEFFFFFFF*/) | (long) ((value ? 1 : 0) << 28));
    }
  }

  internal bool DontBreakConstrainedForcedTables
  {
    get => (this.m_flagsA & 536870912U /*0x20000000*/) >> 29 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 3758096383U /*0xDFFFFFFF*/) | (long) ((value ? 1 : 0) << 29));
    }
  }

  internal bool DontVertAlignInTxbx
  {
    get => (this.m_flagsA & 1073741824U /*0x40000000*/) >> 30 != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & 3221225471U /*0xBFFFFFFF*/) | (long) ((value ? 1 : 0) << 30));
    }
  }

  internal bool Word11KerningPairs
  {
    get => (this.m_flagsA & 2147483648U /*0x80000000*/) >> 31 /*0x1F*/ != 0U;
    set
    {
      this.m_flagsA = (uint) ((long) (this.m_flagsA & (uint) int.MaxValue) | (long) ((value ? 1 : 0) << 31 /*0x1F*/));
    }
  }

  internal bool CachedColBalance
  {
    get => ((int) this.m_flagsg & 1) != 0;
    set => this.m_flagsg = (byte) ((int) this.m_flagsg & 254 | (value ? 1 : 0));
  }

  internal Copts(DOPDescriptor dopBase) => this.m_dopBase = dopBase;

  internal void Parse(Stream stream)
  {
    this.Copts80.Parse(stream);
    this.m_flagsA = BaseWordRecord.ReadUInt32(stream);
    this.m_flagsg = (byte) stream.ReadByte();
    stream.ReadByte();
    int num1 = (int) BaseWordRecord.ReadUInt16(stream);
    int num2 = (int) BaseWordRecord.ReadUInt32(stream);
    int num3 = (int) BaseWordRecord.ReadUInt32(stream);
    int num4 = (int) BaseWordRecord.ReadUInt32(stream);
    int num5 = (int) BaseWordRecord.ReadUInt32(stream);
    int num6 = (int) BaseWordRecord.ReadUInt32(stream);
  }

  internal void Write(Stream stream)
  {
    this.Copts80.Write(stream);
    this.UseNormalStyleForList = true;
    this.DontUseIndentAsNumberingTabStop = true;
    this.FELineBreak11 = true;
    this.AllowSpaceOfSameStyleInTable = true;
    this.WW11IndentRules = true;
    this.DontAutofitConstrainedTables = true;
    this.AutofitLikeWW11 = true;
    this.HangulWidthLikeWW11 = true;
    this.SplitPgBreakAndParaMark = true;
    this.DontVertAlignCellWithSp = true;
    this.DontBreakConstrainedForcedTables = true;
    this.DontVertAlignInTxbx = true;
    this.Word11KerningPairs = true;
    BaseWordRecord.WriteUInt32(stream, this.m_flagsA);
    this.CachedColBalance = true;
    stream.WriteByte(this.m_flagsg);
    stream.WriteByte((byte) 0);
    BaseWordRecord.WriteUInt16(stream, (ushort) 0);
    BaseWordRecord.WriteUInt32(stream, 0U);
    BaseWordRecord.WriteUInt32(stream, 0U);
    BaseWordRecord.WriteUInt32(stream, 0U);
    BaseWordRecord.WriteUInt32(stream, 0U);
    BaseWordRecord.WriteUInt32(stream, 0U);
  }
}
