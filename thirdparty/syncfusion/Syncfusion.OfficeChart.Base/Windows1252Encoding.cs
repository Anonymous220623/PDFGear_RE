// Decompiled with JetBrains decompiler
// Type: Windows1252Encoding
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Text;

#nullable disable
internal class Windows1252Encoding : Encoding
{
  private char[] map = new char[256 /*0x0100*/]
  {
    char.MinValue,
    '\u0001',
    '\u0002',
    '\u0003',
    '\u0004',
    '\u0005',
    '\u0006',
    '\a',
    '\b',
    '\t',
    '\n',
    '\v',
    '\f',
    '\r',
    '\u000E',
    '\u000F',
    '\u0010',
    '\u0011',
    '\u0012',
    '\u0013',
    '\u0014',
    '\u0015',
    '\u0016',
    '\u0017',
    '\u0018',
    '\u0019',
    '\u001A',
    '\u001B',
    '\u001C',
    '\u001D',
    '\u001E',
    '\u001F',
    ' ',
    '!',
    '"',
    '#',
    '$',
    '%',
    '&',
    '\'',
    '(',
    ')',
    '*',
    '+',
    ',',
    '-',
    '.',
    '/',
    '0',
    '1',
    '2',
    '3',
    '4',
    '5',
    '6',
    '7',
    '8',
    '9',
    ':',
    ';',
    '<',
    '=',
    '>',
    '?',
    '@',
    'A',
    'B',
    'C',
    'D',
    'E',
    'F',
    'G',
    'H',
    'I',
    'J',
    'K',
    'L',
    'M',
    'N',
    'O',
    'P',
    'Q',
    'R',
    'S',
    'T',
    'U',
    'V',
    'W',
    'X',
    'Y',
    'Z',
    '[',
    '\\',
    ']',
    '^',
    '_',
    '`',
    'a',
    'b',
    'c',
    'd',
    'e',
    'f',
    'g',
    'h',
    'i',
    'j',
    'k',
    'l',
    'm',
    'n',
    'o',
    'p',
    'q',
    'r',
    's',
    't',
    'u',
    'v',
    'w',
    'x',
    'y',
    'z',
    '{',
    '|',
    '}',
    '~',
    '\u007F',
    '€',
    '\u0081',
    '‚',
    'ƒ',
    '„',
    '…',
    '†',
    '‡',
    'ˆ',
    '‰',
    'Š',
    '‹',
    'Œ',
    '\u008D',
    'Ž',
    '\u008F',
    '\u0090',
    '‘',
    '’',
    '“',
    '”',
    '•',
    '–',
    '—',
    '˜',
    '™',
    'š',
    '›',
    'œ',
    '\u009D',
    'ž',
    'Ÿ',
    ' ',
    '¡',
    '¢',
    '£',
    '¤',
    '¥',
    '¦',
    '§',
    '¨',
    '©',
    'ª',
    '«',
    '¬',
    '\u00AD',
    '®',
    '¯',
    '°',
    '±',
    '\u00B2',
    '\u00B3',
    '´',
    'µ',
    '¶',
    '·',
    '¸',
    '\u00B9',
    'º',
    '»',
    '\u00BC',
    '\u00BD',
    '\u00BE',
    '¿',
    'À',
    'Á',
    'Â',
    'Ã',
    'Ä',
    'Å',
    'Æ',
    'Ç',
    'È',
    'É',
    'Ê',
    'Ë',
    'Ì',
    'Í',
    'Î',
    'Ï',
    'Ð',
    'Ñ',
    'Ò',
    'Ó',
    'Ô',
    'Õ',
    'Ö',
    '×',
    'Ø',
    'Ù',
    'Ú',
    'Û',
    'Ü',
    'Ý',
    'Þ',
    'ß',
    'à',
    'á',
    'â',
    'ã',
    'ä',
    'å',
    'æ',
    'ç',
    'è',
    'é',
    'ê',
    'ë',
    'ì',
    'í',
    'î',
    'ï',
    'ð',
    'ñ',
    'ò',
    'ó',
    'ô',
    'õ',
    'ö',
    '÷',
    'ø',
    'ù',
    'ú',
    'û',
    'ü',
    'ý',
    'þ',
    'ÿ'
  };

  public override string WebName => "windows-1252";

  public override int GetMaxByteCount(int charCount) => charCount;

  public override int GetMaxCharCount(int byteCount) => byteCount;

  public override int GetByteCount(char[] chars, int index, int count)
  {
    if (chars == null)
      throw new ArgumentNullException(nameof (chars));
    if (index < 0 || index > chars.Length)
      throw new ArgumentOutOfRangeException(nameof (index));
    if (count < 0 || index + count > chars.Length)
      throw new ArgumentOutOfRangeException(nameof (count));
    return count;
  }

  public override int GetCharCount(byte[] bytes, int index, int count)
  {
    if (bytes == null)
      throw new ArgumentNullException(nameof (bytes));
    if (index < 0 || index > bytes.Length)
      throw new ArgumentOutOfRangeException(nameof (index));
    if (count < 0 || index + count > bytes.Length)
      throw new ArgumentOutOfRangeException(nameof (count));
    return count;
  }

  public override int GetBytes(
    char[] chars,
    int charIndex,
    int charCount,
    byte[] bytes,
    int byteIndex)
  {
    if (chars == null)
      throw new ArgumentNullException(nameof (chars));
    if (bytes == null)
      throw new ArgumentNullException(nameof (bytes));
    if (charIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (charIndex));
    if (charCount < 0)
      throw new ArgumentOutOfRangeException(nameof (charCount));
    if (byteIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (byteIndex));
    if (charIndex + charCount > chars.Length)
      throw new ArgumentOutOfRangeException(nameof (charCount));
    if (byteIndex + charCount > bytes.Length)
      throw new ArgumentException(nameof (bytes));
    int num1 = charIndex + charCount;
    int index1 = charIndex;
    int index2 = byteIndex;
    while (index1 < num1)
    {
      char ch = chars[index1];
      byte num2;
      switch (ch)
      {
        case 'Œ':
          num2 = (byte) 140;
          break;
        case 'œ':
          num2 = (byte) 156;
          break;
        case 'Š':
          num2 = (byte) 138;
          break;
        case 'š':
          num2 = (byte) 154;
          break;
        case 'Ÿ':
          num2 = (byte) 159;
          break;
        case 'Ž':
          num2 = (byte) 142;
          break;
        case 'ž':
          num2 = (byte) 158;
          break;
        case 'ƒ':
          num2 = (byte) 131;
          break;
        case 'ˆ':
          num2 = (byte) 136;
          break;
        case '˜':
          num2 = (byte) 152;
          break;
        case '–':
          num2 = (byte) 150;
          break;
        case '—':
          num2 = (byte) 151;
          break;
        case '‘':
          num2 = (byte) 145;
          break;
        case '’':
          num2 = (byte) 146;
          break;
        case '‚':
          num2 = (byte) 130;
          break;
        case '“':
          num2 = (byte) 147;
          break;
        case '”':
          num2 = (byte) 148;
          break;
        case '„':
          num2 = (byte) 132;
          break;
        case '†':
          num2 = (byte) 134;
          break;
        case '‡':
          num2 = (byte) 135;
          break;
        case '•':
          num2 = (byte) 149;
          break;
        case '…':
          num2 = (byte) 133;
          break;
        case '‰':
          num2 = (byte) 137;
          break;
        case '‹':
          num2 = (byte) 139;
          break;
        case '›':
          num2 = (byte) 155;
          break;
        case '€':
          num2 = (byte) 128 /*0x80*/;
          break;
        case '™':
          num2 = (byte) 153;
          break;
        default:
          num2 = ch > 'ÿ' ? (byte) 26 : (byte) ch;
          break;
      }
      bytes[index2] = num2;
      ++index1;
      ++index2;
    }
    return charCount;
  }

  public override int GetChars(
    byte[] bytes,
    int byteIndex,
    int byteCount,
    char[] chars,
    int charIndex)
  {
    if (bytes == null)
      throw new ArgumentNullException(nameof (bytes));
    if (chars == null)
      throw new ArgumentNullException(nameof (chars));
    if (byteIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (byteIndex));
    if (byteCount < 0)
      throw new ArgumentOutOfRangeException(nameof (byteCount));
    if (charIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (charIndex));
    if (byteIndex + byteCount > bytes.Length)
      throw new ArgumentOutOfRangeException(nameof (byteCount));
    if (charIndex + byteCount > chars.Length)
      throw new ArgumentException(nameof (chars));
    int num = byteIndex + byteCount;
    int index1 = byteIndex;
    int index2 = charIndex;
    while (index1 < num)
    {
      char ch = this.map[(int) bytes[index1]];
      chars[index2] = ch;
      ++index1;
      ++index2;
    }
    return byteCount;
  }
}
