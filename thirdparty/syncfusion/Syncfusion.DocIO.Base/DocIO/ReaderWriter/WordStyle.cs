// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordStyle
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class WordStyle
{
  private string m_strName;
  private readonly WordStyleSheet m_styleSheet;
  private int m_baseStyleIndex = 4095 /*0x0FFF*/;
  private int m_nextStyleIndex = 4095 /*0x0FFF*/;
  private int m_linkStyleIndex;
  private int m_id = -1;
  internal static readonly WordStyle Empty = new WordStyle();
  private WordStyleType m_typeCode;
  private byte[] m_tapx;
  private byte m_bFlags;
  private CharacterPropertyException m_chpx;
  private ParagraphPropertyException m_papx;

  internal WordStyle()
  {
  }

  internal WordStyle(WordStyleSheet styleSheet, string name)
  {
    this.m_strName = name;
    this.m_styleSheet = styleSheet;
    this.m_chpx = new CharacterPropertyException();
    this.m_papx = new ParagraphPropertyException();
  }

  internal WordStyle(WordStyleSheet styleSheet, string name, bool isCharacterStyle)
  {
    this.m_strName = name;
    this.m_styleSheet = styleSheet;
    this.IsCharacterStyle = isCharacterStyle;
    this.m_chpx = new CharacterPropertyException();
    if (this.IsCharacterStyle)
      return;
    this.m_papx = new ParagraphPropertyException();
  }

  internal byte[] TableStyleData
  {
    get => this.m_tapx;
    set => this.m_tapx = value;
  }

  internal WordStyleType TypeCode
  {
    get => this.m_typeCode;
    set => this.m_typeCode = value;
  }

  internal int BaseStyleIndex
  {
    get => this.m_baseStyleIndex;
    set => this.m_baseStyleIndex = value;
  }

  internal int ID
  {
    get => this.m_id;
    set => this.m_id = value;
  }

  internal bool IsCharacterStyle
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal string Name
  {
    get => this.m_strName;
    set => this.m_strName = value;
  }

  internal bool HasUpe
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal int NextStyleIndex
  {
    get => this.m_nextStyleIndex;
    set => this.m_nextStyleIndex = value;
  }

  internal int LinkStyleIndex
  {
    get => this.m_linkStyleIndex;
    set => this.m_linkStyleIndex = value;
  }

  internal bool IsPrimary
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal bool IsSemiHidden
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  internal bool UnhideWhenUsed
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  internal WordStyleSheet StyleSheet => this.m_styleSheet;

  internal CharacterPropertyException CHPX
  {
    get => this.m_chpx;
    set => this.m_chpx = value;
  }

  internal ParagraphPropertyException PAPX
  {
    get => this.m_papx;
    set => this.m_papx = value;
  }

  internal void UpdateName(string name) => this.m_strName = name;
}
