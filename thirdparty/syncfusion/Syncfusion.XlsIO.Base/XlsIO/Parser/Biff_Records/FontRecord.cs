// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.FontRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.Font)]
[CLSCompliant(false)]
public class FontRecord : BiffRecordRaw
{
  private const int DEF_INCORRECT_HASH = -1;
  private const int DEF_STRING_TYPE_OFFSET = 15;
  public const int DefaultFontColor = 32767 /*0x7FFF*/;
  [BiffRecordPos(0, 2)]
  private ushort m_usFontHeight = 200;
  [BiffRecordPos(2, 2)]
  private FontRecord.FontAttributes m_attributes;
  [BiffRecordPos(4, 2)]
  private ushort m_usPaletteColorIndex = (ushort) short.MaxValue;
  [BiffRecordPos(6, 2)]
  private ushort m_usBoldWeight = 400;
  [BiffRecordPos(8, 2)]
  private ushort m_SuperSubscript;
  [BiffRecordPos(10, 1)]
  private byte m_Underline;
  [BiffRecordPos(11, 1)]
  private byte m_Family;
  [BiffRecordPos(12, 1)]
  private byte m_Charset;
  [BiffRecordPos(13, 1)]
  private byte m_Reserved;
  [BiffRecordPos(14, TFieldType.String)]
  private string m_strFontName = "Arial";
  private int m_iHashCode = -1;
  private int m_baseLine;

  public ushort Attributes => (ushort) this.m_attributes;

  public ushort FontHeight
  {
    get => this.m_usFontHeight;
    set
    {
      if ((int) this.m_usFontHeight == (int) value)
        return;
      this.m_usFontHeight = value;
      this.m_iHashCode = -1;
    }
  }

  public ushort PaletteColorIndex
  {
    get => this.m_usPaletteColorIndex;
    set
    {
      if ((int) this.m_usPaletteColorIndex == (int) value)
        return;
      this.m_usPaletteColorIndex = value;
      this.m_iHashCode = -1;
    }
  }

  public ushort BoldWeight
  {
    get => this.m_usBoldWeight;
    set
    {
      if ((int) this.m_usBoldWeight == (int) value)
        return;
      this.m_usBoldWeight = value;
      this.m_iHashCode = -1;
    }
  }

  public ExcelFontVertialAlignment SuperSubscript
  {
    get => (ExcelFontVertialAlignment) this.m_SuperSubscript;
    set
    {
      if ((int) this.m_SuperSubscript == (int) (ushort) value)
        return;
      this.m_SuperSubscript = (ushort) value;
      this.m_iHashCode = -1;
    }
  }

  public int Baseline
  {
    get => this.m_baseLine;
    set => this.m_baseLine = value;
  }

  public ExcelUnderline Underline
  {
    get => (ExcelUnderline) this.m_Underline;
    set
    {
      if ((int) this.m_Underline == (int) (byte) value)
        return;
      this.m_Underline = (byte) value;
      this.m_iHashCode = -1;
    }
  }

  public byte Family
  {
    get => this.m_Family;
    set
    {
      if ((int) this.m_Family == (int) value)
        return;
      this.m_Family = value;
      this.m_iHashCode = -1;
    }
  }

  public byte Charset
  {
    get => this.m_Charset;
    set
    {
      if ((int) this.m_Charset == (int) value)
        return;
      this.m_Charset = value;
      this.m_iHashCode = -1;
    }
  }

  public string FontName
  {
    get => this.m_strFontName;
    set
    {
      if (!(this.m_strFontName != value))
        return;
      this.m_strFontName = value;
      this.m_iHashCode = -1;
    }
  }

  public bool IsItalic
  {
    get => (this.m_attributes & FontRecord.FontAttributes.Italic) != (FontRecord.FontAttributes) 0;
    set
    {
      if (value)
        this.m_attributes |= FontRecord.FontAttributes.Italic;
      else
        this.m_attributes &= ~FontRecord.FontAttributes.Italic;
      this.m_iHashCode = -1;
    }
  }

  public bool IsStrikeout
  {
    get
    {
      return (this.m_attributes & FontRecord.FontAttributes.Strikeout) != (FontRecord.FontAttributes) 0;
    }
    set
    {
      if (value)
        this.m_attributes |= FontRecord.FontAttributes.Strikeout;
      else
        this.m_attributes &= ~FontRecord.FontAttributes.Strikeout;
      this.m_iHashCode = -1;
    }
  }

  public bool IsMacOutline
  {
    get
    {
      return (this.m_attributes & FontRecord.FontAttributes.MacOutline) != (FontRecord.FontAttributes) 0;
    }
    set
    {
      if (value)
        this.m_attributes |= FontRecord.FontAttributes.MacOutline;
      else
        this.m_attributes &= ~FontRecord.FontAttributes.MacOutline;
      this.m_iHashCode = -1;
    }
  }

  public bool IsMacShadow
  {
    get
    {
      return (this.m_attributes & FontRecord.FontAttributes.MacShadow) != (FontRecord.FontAttributes) 0;
    }
    set
    {
      if (value)
        this.m_attributes |= FontRecord.FontAttributes.MacShadow;
      else
        this.m_attributes &= ~FontRecord.FontAttributes.MacShadow;
      this.m_iHashCode = -1;
    }
  }

  public byte Reserved => this.m_Reserved;

  public override int MinimumRecordSize => 16 /*0x10*/;

  public FontRecord()
  {
  }

  public FontRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public FontRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usFontHeight = provider.ReadUInt16(iOffset);
    this.m_attributes = (FontRecord.FontAttributes) provider.ReadUInt16(iOffset + 2);
    this.m_usPaletteColorIndex = provider.ReadUInt16(iOffset + 4);
    this.m_usBoldWeight = provider.ReadUInt16(iOffset + 6);
    this.m_SuperSubscript = provider.ReadUInt16(iOffset + 8);
    this.m_Underline = provider.ReadByte(iOffset + 10);
    this.m_Family = provider.ReadByte(iOffset + 11);
    this.m_Charset = provider.ReadByte(iOffset + 12);
    this.m_Reserved = provider.ReadByte(iOffset + 13);
    this.m_strFontName = provider.ReadString8Bit(iOffset + 14, out iLength);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usFontHeight);
    iOffset += 2;
    provider.WriteUInt16(iOffset, (ushort) this.m_attributes);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usPaletteColorIndex);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usBoldWeight);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_SuperSubscript);
    iOffset += 2;
    provider.WriteByte(iOffset, this.m_Underline);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_Family);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_Charset);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_Reserved);
    ++iOffset;
    int length = this.m_strFontName.Length;
    provider.WriteByte(iOffset, (byte) length);
    ++iOffset;
    if (length <= 0)
      return;
    provider.WriteStringNoLenUpdateOffset(ref iOffset, this.m_strFontName);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    return 16 /*0x10*/ + this.m_strFontName.Length * 2;
  }

  public override bool Equals(object obj)
  {
    FontRecord fontRecord = obj as FontRecord;
    return fontRecord.m_strFontName == this.m_strFontName && (int) fontRecord.m_usFontHeight == (int) this.m_usFontHeight && (int) fontRecord.m_usPaletteColorIndex == (int) this.m_usPaletteColorIndex && (int) fontRecord.m_usBoldWeight == (int) this.m_usBoldWeight && (int) fontRecord.m_Underline == (int) this.m_Underline && (int) fontRecord.m_SuperSubscript == (int) this.m_SuperSubscript && (int) fontRecord.m_Family == (int) this.m_Family && (int) fontRecord.m_Charset == (int) this.m_Charset && (fontRecord.m_attributes & FontRecord.FontAttributes.AllKnown) == (this.m_attributes & FontRecord.FontAttributes.AllKnown);
  }

  public override int GetHashCode()
  {
    if (this.m_iHashCode == -1)
      this.m_iHashCode = this.m_usFontHeight.GetHashCode() + (this.m_attributes & FontRecord.FontAttributes.AllKnown).GetHashCode() + this.m_usPaletteColorIndex.GetHashCode() + this.m_usBoldWeight.GetHashCode() + this.m_SuperSubscript.GetHashCode() + this.m_Underline.GetHashCode() + this.m_Family.GetHashCode() + this.m_Charset.GetHashCode() + this.m_strFontName.GetHashCode();
    return this.m_iHashCode;
  }

  public override void CopyTo(BiffRecordRaw raw)
  {
    if (!(raw is FontRecord fontRecord))
      throw new ArgumentNullException("twin");
    fontRecord.m_usFontHeight = this.m_usFontHeight;
    fontRecord.m_attributes = this.m_attributes;
    fontRecord.m_usPaletteColorIndex = this.m_usPaletteColorIndex;
    fontRecord.m_usBoldWeight = this.m_usBoldWeight;
    fontRecord.m_SuperSubscript = this.m_SuperSubscript;
    fontRecord.m_Underline = this.m_Underline;
    fontRecord.m_Family = this.m_Family;
    fontRecord.m_Charset = this.m_Charset;
    fontRecord.m_strFontName = this.m_strFontName;
    fontRecord.m_iHashCode = this.m_iHashCode;
  }

  public int CompareTo(FontRecord record)
  {
    if (record == null)
      return 1;
    int num1 = this.GetHashCode() - record.GetHashCode();
    if (num1 != 0)
      return num1;
    int num2 = (int) this.m_usFontHeight - (int) record.m_usFontHeight;
    if (num2 != 0)
      return num2;
    int num3 = this.m_strFontName.CompareTo(record.m_strFontName);
    if (num3 != 0)
      return num3;
    int num4 = (this.m_attributes & FontRecord.FontAttributes.AllKnown).CompareTo((object) (record.m_attributes & FontRecord.FontAttributes.AllKnown));
    if (num4 != 0)
      return num4;
    int num5 = (int) this.m_usPaletteColorIndex - (int) record.m_usPaletteColorIndex;
    if (num5 != 0)
      return num5;
    int num6 = (int) this.m_usBoldWeight - (int) record.m_usBoldWeight;
    if (num6 != 0)
      return num6;
    int num7 = (int) this.m_SuperSubscript - (int) record.m_SuperSubscript;
    if (num7 != 0)
      return num7;
    int num8 = (int) this.m_Underline - (int) record.m_Underline;
    if (num8 != 0)
      return num8;
    int num9 = (int) this.m_Family - (int) record.m_Family;
    if (num9 != 0)
      return num9;
    int num10 = (int) this.m_Charset - (int) record.m_Charset;
    return num10 != 0 ? num10 : 0;
  }

  [Flags]
  internal enum FontAttributes : ushort
  {
    Italic = 2,
    Strikeout = 8,
    MacOutline = 16, // 0x0010
    MacShadow = 32, // 0x0020
    AllKnown = MacShadow | MacOutline | Strikeout | Italic, // 0x003A
  }
}
