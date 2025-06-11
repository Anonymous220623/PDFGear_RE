// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.DXFN
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal class DXFN
{
  private const int DEF_FONT_FIRST_RESERVED_SIZE = 64 /*0x40*/;
  private const int DEF_FONT_SECOND_RESERVED_SIZE = 3;
  private const int DEF_FONT_THIRD_RESERVED_SIZE = 16 /*0x10*/;
  private const uint DEF_FONT_POSTURE_MASK = 2;
  private const uint DEF_FONT_CANCELLATION_MASK = 128 /*0x80*/;
  private const uint DEF_FONT_STYLE_MODIFIED_MASK = 2;
  private const uint DEF_FONT_CANCELLATION_MODIFIED_MASK = 128 /*0x80*/;
  private const ushort DEF_BORDER_LEFT_MASK = 15;
  private const ushort DEF_BORDER_RIGHT_MASK = 240 /*0xF0*/;
  private const ushort DEF_BORDER_TOP_MASK = 3840 /*0x0F00*/;
  private const ushort DEF_BORDER_BOTTOM_MASK = 61440 /*0xF000*/;
  private const uint DEF_BORDER_LEFT_COLOR_MASK = 127 /*0x7F*/;
  private const uint DEF_BORDER_RIGHT_COLOR_MASK = 16256;
  private const uint DEF_BORDER_TOP_COLOR_MASK = 8323072 /*0x7F0000*/;
  private const uint DEF_BORDER_BOTTOM_COLOR_MASK = 1065353216 /*0x3F800000*/;
  private const int DEF_BORDER_LEFT_COLOR_START = 0;
  private const int DEF_BORDER_RIGHT_COLOR_START = 7;
  private const int DEF_BORDER_TOP_COLOR_START = 16 /*0x10*/;
  private const int DEF_BORDER_BOTTOM_COLOR_START = 23;
  private const ushort DEF_PATTERN_MASK = 64512;
  private const ushort DEF_PATTERN_COLOR_MASK = 127 /*0x7F*/;
  private const ushort DEF_PATTERN_BACKCOLOR_MASK = 16256;
  private const int DEF_PATTERN_START = 10;
  private const int DEF_PATTERN_BACKCOLOR_START = 7;
  private const int DEF_FONT_BLOCK_SIZE = 118;
  private const int DEF_BORDER_BLOCK_SIZE = 8;
  private const int DEF_PATTERN_BLOCK_SIZE = 4;
  private const int DEF_NUMBER_FORMAT_BLOCK_SIZE = 2;
  public const uint DefaultColorIndex = 4294967295 /*0xFFFFFFFF*/;
  private byte m_uiOptions;
  private byte m_usReserved;
  private bool m_bLeftBorder = true;
  private bool m_bRightBorder = true;
  private bool m_bTopBorder = true;
  private bool m_bBottomBorder = true;
  private bool m_bPatternStyle = true;
  private bool m_bPatternColor = true;
  private bool m_bPatternBackColor = true;
  private bool m_bNumberFormatModified = true;
  private bool m_bNumberFormatPresent;
  private bool m_bFontFormat;
  private bool m_bBorderFormat;
  private bool m_bPatternFormat;
  private bool m_numberFormatIsUserDefined;
  private uint m_uiFontHeight = uint.MaxValue;
  private uint m_uiFontOptions;
  private ushort m_usFontWeight = 400;
  private ushort m_usEscapmentType;
  private byte m_Underline;
  private uint m_uiFontColorIndex = uint.MaxValue;
  private uint m_uiModifiedFlags = 15;
  private uint m_uiEscapmentModified = 1;
  private uint m_uiUnderlineModified = 1;
  private ushort m_usBorderLineStyles;
  private uint m_uiBorderColors;
  private ushort m_usPatternStyle;
  private ushort m_usPatternColors;
  private ushort m_unUsed;
  private ushort m_numFormatIndex;
  private ushort m_userdefNumFormatSize;
  private ushort m_charCount;
  private bool m_isHighByte;
  private string m_strValue;

  public int ParseDXFN(DataProvider provider, int iOffset, OfficeVersion version)
  {
    this.m_uiOptions = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_bLeftBorder = provider.ReadBit(iOffset, 2);
    this.m_bRightBorder = provider.ReadBit(iOffset, 3);
    this.m_bTopBorder = provider.ReadBit(iOffset, 4);
    this.m_bBottomBorder = provider.ReadBit(iOffset, 5);
    ++iOffset;
    this.m_bPatternStyle = provider.ReadBit(iOffset, 0);
    this.m_bPatternColor = provider.ReadBit(iOffset, 1);
    this.m_bPatternBackColor = provider.ReadBit(iOffset, 2);
    this.m_bNumberFormatModified = provider.ReadBit(iOffset, 3);
    ++iOffset;
    this.m_bNumberFormatPresent = provider.ReadBit(iOffset, 1);
    this.m_bFontFormat = provider.ReadBit(iOffset, 2);
    this.m_bBorderFormat = provider.ReadBit(iOffset, 4);
    this.m_bPatternFormat = provider.ReadBit(iOffset, 5);
    ++iOffset;
    this.m_numberFormatIsUserDefined = provider.ReadBit(iOffset, 0);
    ++iOffset;
    this.m_usReserved = provider.ReadByte(iOffset);
    ++iOffset;
    iOffset = this.m_numberFormatIsUserDefined ? this.ParseUserdefinedNumberFormatBlock(provider, ref iOffset) : this.ParseNumberFormatBlock(provider, ref iOffset);
    iOffset = this.ParseFontBlock(provider, ref iOffset);
    iOffset = this.ParseBorderBlock(provider, ref iOffset);
    iOffset = this.ParsePatternBlock(provider, ref iOffset);
    return iOffset;
  }

  public int SerializeDXFN(DataProvider provider, int iOffset, OfficeVersion version)
  {
    provider.WriteByte(iOffset, this.m_uiOptions);
    ++iOffset;
    provider.WriteBit(iOffset, this.m_bLeftBorder, 2);
    provider.WriteBit(iOffset, this.m_bRightBorder, 3);
    provider.WriteBit(iOffset, this.m_bTopBorder, 4);
    provider.WriteBit(iOffset, this.m_bBottomBorder, 5);
    ++iOffset;
    provider.WriteBit(iOffset, this.m_bPatternStyle, 0);
    provider.WriteBit(iOffset, this.m_bPatternColor, 1);
    provider.WriteBit(iOffset, this.m_bPatternBackColor, 2);
    provider.WriteBit(iOffset, this.m_bNumberFormatModified, 3);
    ++iOffset;
    provider.WriteBit(iOffset, this.m_bNumberFormatPresent, 1);
    provider.WriteBit(iOffset, this.m_bFontFormat, 2);
    provider.WriteBit(iOffset, this.m_bBorderFormat, 4);
    provider.WriteBit(iOffset, this.m_bPatternFormat, 5);
    ++iOffset;
    provider.WriteBit(iOffset, this.m_numberFormatIsUserDefined, 0);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_usReserved);
    ++iOffset;
    iOffset = this.m_numberFormatIsUserDefined ? this.SerializeUserdefinedNumberFormatBlock(provider, ref iOffset) : this.SerializeNumberFormatBlock(provider, ref iOffset);
    iOffset = this.SerializeFontBlock(provider, ref iOffset);
    iOffset = this.SerializeBorderBlock(provider, ref iOffset);
    iOffset = this.SerializePatternBlock(provider, ref iOffset);
    return iOffset;
  }

  public int ParseFontBlock(DataProvider provider, ref int iOffset)
  {
    if (!this.m_bFontFormat)
      return iOffset;
    iOffset += 64 /*0x40*/;
    this.m_uiFontHeight = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_uiFontOptions = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_usFontWeight = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usEscapmentType = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_Underline = provider.ReadByte(iOffset);
    ++iOffset;
    iOffset += 3;
    this.m_uiFontColorIndex = provider.ReadUInt32(iOffset);
    iOffset += 4;
    iOffset += 4;
    this.m_uiModifiedFlags = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_uiEscapmentModified = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_uiUnderlineModified = provider.ReadUInt32(iOffset);
    iOffset += 4;
    iOffset += 16 /*0x10*/;
    iOffset += 2;
    return iOffset;
  }

  public int ParseBorderBlock(DataProvider provider, ref int iOffset)
  {
    if (!this.m_bBorderFormat)
      return iOffset;
    this.m_usBorderLineStyles = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_uiBorderColors = provider.ReadUInt32(iOffset);
    iOffset += 4;
    iOffset += 2;
    return iOffset;
  }

  public int ParsePatternBlock(DataProvider provider, ref int iOffset)
  {
    if (!this.m_bPatternFormat)
      return iOffset;
    this.m_usPatternStyle = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usPatternColors = provider.ReadUInt16(iOffset);
    iOffset += 2;
    return iOffset;
  }

  public int ParseNumberFormatBlock(DataProvider provider, ref int iOffset)
  {
    if (!this.m_bNumberFormatPresent)
      return iOffset;
    this.m_unUsed = (ushort) provider.ReadByte(iOffset);
    ++iOffset;
    this.m_numFormatIndex = (ushort) provider.ReadByte(iOffset);
    ++iOffset;
    return iOffset;
  }

  public int ParseUserdefinedNumberFormatBlock(DataProvider provider, ref int iOffset)
  {
    if (!this.m_bNumberFormatPresent)
      return iOffset;
    this.m_userdefNumFormatSize = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_charCount = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_isHighByte = provider.ReadBit(iOffset, 0);
    ++iOffset;
    int stringLength = this.m_isHighByte ? 2 * (int) this.m_charCount : (int) this.m_charCount;
    this.m_strValue = provider.ReadString(iOffset, stringLength, Encoding.UTF8, true);
    iOffset += stringLength;
    return iOffset;
  }

  public int SerializeFontBlock(DataProvider provider, ref int iOffset)
  {
    if (provider == null)
      throw new ArgumentNullException(nameof (provider));
    if (!this.m_bFontFormat)
      return iOffset;
    int num1 = 0;
    while (num1 < 64 /*0x40*/)
    {
      provider.WriteByte(iOffset, (byte) 0);
      ++num1;
      ++iOffset;
    }
    provider.WriteUInt32(iOffset, this.m_uiFontHeight);
    iOffset += 4;
    provider.WriteUInt32(iOffset, this.m_uiFontOptions);
    iOffset += 4;
    provider.WriteUInt16(iOffset, this.m_usFontWeight);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usEscapmentType);
    iOffset += 2;
    provider.WriteByte(iOffset, this.m_Underline);
    ++iOffset;
    int num2 = 0;
    while (num2 < 3)
    {
      provider.WriteByte(iOffset, (byte) 0);
      ++num2;
      ++iOffset;
    }
    provider.WriteUInt32(iOffset, this.m_uiFontColorIndex);
    iOffset += 4;
    provider.WriteUInt32(iOffset, 0U);
    iOffset += 4;
    provider.WriteUInt32(iOffset, this.m_uiModifiedFlags);
    iOffset += 4;
    provider.WriteUInt32(iOffset, this.m_uiEscapmentModified);
    iOffset += 4;
    provider.WriteUInt32(iOffset, this.m_uiUnderlineModified);
    iOffset += 4;
    int num3 = 0;
    while (num3 < 16 /*0x10*/)
    {
      provider.WriteByte(iOffset, (byte) 0);
      ++num3;
      ++iOffset;
    }
    provider.WriteUInt16(iOffset, (ushort) 1);
    iOffset += 2;
    return iOffset;
  }

  public int SerializeBorderBlock(DataProvider provider, ref int iOffset)
  {
    if (!this.m_bBorderFormat)
      return iOffset;
    if (provider == null)
      throw new ArgumentNullException(nameof (provider));
    provider.WriteUInt16(iOffset, this.m_usBorderLineStyles);
    iOffset += 2;
    provider.WriteUInt32(iOffset, this.m_uiBorderColors);
    iOffset += 4;
    provider.WriteUInt16(iOffset, (ushort) 0);
    iOffset += 2;
    return iOffset;
  }

  public int SerializePatternBlock(DataProvider provider, ref int iOffset)
  {
    if (!this.m_bPatternFormat)
      return iOffset;
    if (provider == null)
      throw new ArgumentNullException(nameof (provider));
    provider.WriteUInt16(iOffset, this.m_usPatternStyle);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usPatternColors);
    iOffset += 2;
    return iOffset;
  }

  public int SerializeNumberFormatBlock(DataProvider provider, ref int iOffset)
  {
    if (!this.m_bNumberFormatPresent)
      return iOffset;
    if (provider == null)
      throw new ArgumentNullException(nameof (provider));
    provider.WriteUInt16(iOffset, this.m_unUsed);
    ++iOffset;
    provider.WriteUInt16(iOffset, this.m_numFormatIndex);
    ++iOffset;
    return iOffset;
  }

  public int SerializeUserdefinedNumberFormatBlock(DataProvider provider, ref int iOffset)
  {
    if (!this.m_bNumberFormatPresent)
      return iOffset;
    if (provider == null)
      throw new ArgumentNullException(nameof (provider));
    provider.WriteUInt16(iOffset, this.m_userdefNumFormatSize);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_charCount);
    iOffset += 2;
    if (!this.m_isHighByte)
      provider.WriteByte(iOffset, (byte) 0);
    else
      provider.WriteByte(iOffset, (byte) 1);
    ++iOffset;
    byte[] bytes = Encoding.UTF8.GetBytes(this.m_strValue);
    int length = bytes.Length;
    provider.WriteBytes(iOffset, bytes, 0, length);
    iOffset += length;
    return iOffset;
  }

  public int GetStoreSize(OfficeVersion version)
  {
    int storeSize = 6;
    if (this.m_bFontFormat)
      storeSize += 118;
    if (this.m_bBorderFormat)
      storeSize += 8;
    if (this.m_bPatternFormat)
      storeSize += 4;
    if (this.m_bNumberFormatPresent)
      storeSize += 2;
    return storeSize;
  }

  public new int GetHashCode()
  {
    return this.m_uiOptions.GetHashCode() ^ this.m_usReserved.GetHashCode() ^ this.m_bLeftBorder.GetHashCode() ^ this.m_bRightBorder.GetHashCode() ^ this.m_bTopBorder.GetHashCode() ^ this.m_bBottomBorder.GetHashCode() ^ this.m_bPatternStyle.GetHashCode() ^ this.m_bPatternColor.GetHashCode() ^ this.m_bPatternBackColor.GetHashCode() ^ this.m_bNumberFormatModified.GetHashCode() ^ this.m_bNumberFormatPresent.GetHashCode() ^ this.m_bFontFormat.GetHashCode() ^ this.m_bBorderFormat.GetHashCode() ^ this.m_bPatternFormat.GetHashCode() ^ this.m_uiFontHeight.GetHashCode() ^ this.m_uiFontOptions.GetHashCode() ^ this.m_usFontWeight.GetHashCode() ^ this.m_usEscapmentType.GetHashCode() ^ this.m_Underline.GetHashCode() ^ this.m_uiFontColorIndex.GetHashCode() ^ this.m_uiModifiedFlags.GetHashCode() ^ this.m_uiEscapmentModified.GetHashCode() ^ this.m_uiUnderlineModified.GetHashCode() ^ this.m_usBorderLineStyles.GetHashCode() ^ this.m_uiBorderColors.GetHashCode() ^ this.m_usPatternStyle.GetHashCode() ^ this.m_usPatternColors.GetHashCode();
  }
}
