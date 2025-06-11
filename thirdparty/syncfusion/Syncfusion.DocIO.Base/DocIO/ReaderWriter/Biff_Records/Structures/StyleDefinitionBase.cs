// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.StyleDefinitionBase
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
[StructLayout(LayoutKind.Sequential)]
internal class StyleDefinitionBase : IDataStructure
{
  internal const int DEF_RECORD_SIZE = 12;
  private const int DEF_START_ID = 0;
  private const int DEF_MASK_ID = 4095 /*0x0FFF*/;
  private const int DEF_BIT_SCRATCH = 12;
  private const int DEF_BIT_INVALID_HEIGHT = 13;
  private const int DEF_BIT_HAS_UPE = 14;
  private const int DEF_BIT_MASS_COPY = 15;
  private const int DEF_MASK_TYPE_CODE = 15;
  private const int DEF_START_TYPE_CODE = 0;
  private const int DEF_MASK_BASE_STYLE = 65520;
  private const int DEF_START_BASE_STYLE = 4;
  private const int DEF_MASK_UPX_NUMBER = 15;
  private const int DEF_START_UPX_NUMBER = 0;
  private const int DEF_MASK_NEXT_STYLE = 65520;
  private const int DEF_START_NEXT_STYLE = 4;
  private const int DEF_BIT_AUTO_REDEFINE = 0;
  private const int DEF_BIT_HIDDEN = 1;
  private const int DEF_BIT_SEMIHIDDEN = 8;
  private const int DEF_BIT_UNHIDEUSED = 11;
  private const int DEF_BIT_QFORMAT = 12;
  private ushort m_usOptions1;
  private ushort m_usOptions2;
  private ushort m_usOptions3;
  private ushort m_usUpeOffset;
  private ushort m_usOptions4;
  private ushort m_usOptions5;

  internal ushort StyleId
  {
    get => (ushort) BaseWordRecord.GetBitsByMask((int) this.m_usOptions1, 4095 /*0x0FFF*/, 0);
    set
    {
      this.m_usOptions1 = (ushort) BaseWordRecord.SetBitsByMask((int) this.m_usOptions1, 4095 /*0x0FFF*/, 0, (int) value);
    }
  }

  internal bool IsScratch
  {
    get => BaseWordRecord.GetBit((int) this.m_usOptions1, 12);
    set => this.m_usOptions1 = (ushort) BaseWordRecord.SetBit((int) this.m_usOptions1, 12, value);
  }

  internal bool IsInvalidHeight
  {
    get => BaseWordRecord.GetBit((int) this.m_usOptions1, 13);
    set => this.m_usOptions1 = (ushort) BaseWordRecord.SetBit((int) this.m_usOptions1, 13, value);
  }

  internal bool HasUpe
  {
    get => BaseWordRecord.GetBit((int) this.m_usOptions1, 14);
    set => this.m_usOptions1 = (ushort) BaseWordRecord.SetBit((int) this.m_usOptions1, 14, value);
  }

  internal bool IsMassCopy
  {
    get => BaseWordRecord.GetBit((int) this.m_usOptions1, 15);
    set => this.m_usOptions1 = (ushort) BaseWordRecord.SetBit((int) this.m_usOptions1, 15, value);
  }

  internal ushort TypeCode
  {
    get => (ushort) BaseWordRecord.GetBitsByMask((int) this.m_usOptions2, 15, 0);
    set
    {
      this.m_usOptions2 = (ushort) BaseWordRecord.SetBitsByMask((int) this.m_usOptions2, 15, 0, (int) value);
    }
  }

  internal ushort BaseStyle
  {
    get => (ushort) BaseWordRecord.GetBitsByMask((int) this.m_usOptions2, 65520, 4);
    set
    {
      this.m_usOptions2 = (ushort) BaseWordRecord.SetBitsByMask((int) this.m_usOptions2, 65520, 4, (int) value);
    }
  }

  internal ushort UPEOffset
  {
    get => this.m_usUpeOffset;
    set => this.m_usUpeOffset = value;
  }

  internal ushort UpxNumber
  {
    get => (ushort) BaseWordRecord.GetBitsByMask((int) this.m_usOptions3, 15, 0);
    set
    {
      this.m_usOptions3 = (ushort) BaseWordRecord.SetBitsByMask((int) this.m_usOptions3, 15, 0, (int) value);
    }
  }

  internal ushort NextStyleId
  {
    get => (ushort) BaseWordRecord.GetBitsByMask((int) this.m_usOptions3, 65520, 4);
    set
    {
      this.m_usOptions3 = (ushort) BaseWordRecord.SetBitsByMask((int) this.m_usOptions3, 65520, 4, (int) value);
    }
  }

  internal bool IsAutoRedefine
  {
    get => BaseWordRecord.GetBit((int) this.m_usOptions4, 0);
    set => this.m_usOptions4 = (ushort) BaseWordRecord.SetBit((int) this.m_usOptions4, 0, value);
  }

  internal bool IsHidden
  {
    get => BaseWordRecord.GetBit((int) this.m_usOptions4, 1);
    set => this.m_usOptions4 = (ushort) BaseWordRecord.SetBit((int) this.m_usOptions4, 1, value);
  }

  public int Length => 12;

  internal ushort LinkStyleId
  {
    get => (ushort) BaseWordRecord.GetBitsByMask((int) this.m_usOptions5, 4095 /*0x0FFF*/, 0);
    set
    {
      this.m_usOptions5 = (ushort) BaseWordRecord.SetBitsByMask((int) this.m_usOptions5, 4095 /*0x0FFF*/, 0, (int) value);
    }
  }

  internal bool IsSemiHidden
  {
    get => BaseWordRecord.GetBit((int) this.m_usOptions4, 8);
    set => this.m_usOptions4 = (ushort) BaseWordRecord.SetBit((int) this.m_usOptions4, 8, value);
  }

  internal bool IsQFormat
  {
    get => BaseWordRecord.GetBit((int) this.m_usOptions4, 12);
    set => this.m_usOptions4 = (ushort) BaseWordRecord.SetBit((int) this.m_usOptions4, 12, value);
  }

  internal bool UnhideWhenUsed
  {
    get => BaseWordRecord.GetBit((int) this.m_usOptions4, 11);
    set => this.m_usOptions4 = (ushort) BaseWordRecord.SetBit((int) this.m_usOptions4, 11, value);
  }

  internal void Clear()
  {
    this.m_usOptions1 = (ushort) 0;
    this.m_usOptions2 = (ushort) 0;
    this.m_usOptions3 = (ushort) 0;
    this.m_usOptions4 = (ushort) 0;
    this.m_usUpeOffset = (ushort) 0;
    this.m_usOptions5 = (ushort) 0;
  }

  public void Parse(byte[] arrData, int iOffset)
  {
    this.m_usOptions1 = ByteConverter.ReadUInt16(arrData, ref iOffset);
    this.m_usOptions2 = ByteConverter.ReadUInt16(arrData, ref iOffset);
    this.m_usOptions3 = ByteConverter.ReadUInt16(arrData, ref iOffset);
    this.m_usUpeOffset = ByteConverter.ReadUInt16(arrData, ref iOffset);
    this.m_usOptions4 = ByteConverter.ReadUInt16(arrData, ref iOffset);
    this.m_usOptions5 = ByteConverter.ReadUInt16(arrData, ref iOffset);
  }

  public int Save(byte[] arrData, int iOffset)
  {
    ByteConverter.WriteUInt16(arrData, ref iOffset, this.m_usOptions1);
    ByteConverter.WriteUInt16(arrData, ref iOffset, this.m_usOptions2);
    ByteConverter.WriteUInt16(arrData, ref iOffset, this.m_usOptions3);
    ByteConverter.WriteUInt16(arrData, ref iOffset, this.m_usUpeOffset);
    ByteConverter.WriteUInt16(arrData, ref iOffset, this.m_usOptions4);
    ByteConverter.WriteUInt16(arrData, ref iOffset, this.m_usOptions5);
    return 12;
  }
}
