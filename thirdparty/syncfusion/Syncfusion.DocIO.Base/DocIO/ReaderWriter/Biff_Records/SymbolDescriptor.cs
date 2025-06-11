// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.SymbolDescriptor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class SymbolDescriptor
{
  internal const int DEF_STRUCT_SIZE = 4;
  internal const int DEF_EXT_VALUE = 240 /*0xF0*/;
  private short m_fontCode;
  private byte m_charSpecifier;
  private byte m_charSpecifierExt = 240 /*0xF0*/;

  internal short FontCode
  {
    get => this.m_fontCode;
    set => this.m_fontCode = value;
  }

  internal byte CharCode
  {
    get => this.m_charSpecifier;
    set => this.m_charSpecifier = value;
  }

  internal byte CharCodeExt
  {
    get => this.m_charSpecifierExt;
    set => this.m_charSpecifierExt = value;
  }

  internal SymbolDescriptor()
  {
  }

  internal void Parse(byte[] operand)
  {
    this.m_fontCode = BitConverter.ToInt16(operand, 0);
    this.m_charSpecifier = operand[2];
    this.m_charSpecifierExt = operand[3];
  }

  internal byte[] Save()
  {
    byte[] numArray = new byte[4];
    BitConverter.GetBytes(this.m_fontCode).CopyTo((Array) numArray, 0);
    numArray[2] = this.m_charSpecifier;
    numArray[3] = this.m_charSpecifierExt;
    return numArray;
  }
}
