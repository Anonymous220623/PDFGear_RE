﻿// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.RTLCharacters
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class RTLCharacters
{
  private const sbyte L = 0;
  private const sbyte LRE = 1;
  private const sbyte LRO = 2;
  private const sbyte R = 3;
  private const sbyte AL = 4;
  private const sbyte RLE = 5;
  private const sbyte RLO = 6;
  private const sbyte PDF = 7;
  private const sbyte EN = 8;
  private const sbyte ES = 9;
  private const sbyte ET = 10;
  private const sbyte AN = 11;
  private const sbyte CS = 12;
  private const sbyte NSM = 13;
  private const sbyte BN = 14;
  private const sbyte B = 15;
  private const sbyte S = 16 /*0x10*/;
  private const sbyte WS = 17;
  private const sbyte ON = 18;
  private sbyte[] m_types;
  private sbyte m_textOrder = -1;
  private int m_length;
  private sbyte[] m_result;
  private sbyte[] m_levels;
  private sbyte[] m_rtlCharacterTypes = new sbyte[65536 /*0x010000*/];
  private readonly char[] CharTypes = new char[1725]
  {
    char.MinValue,
    '\b',
    '\u000E',
    '\t',
    '\t',
    '\u0010',
    '\n',
    '\n',
    '\u000F',
    '\v',
    '\v',
    '\u0010',
    '\f',
    '\f',
    '\u0011',
    '\r',
    '\r',
    '\u000F',
    '\u000E',
    '\u001B',
    '\u000E',
    '\u001C',
    '\u001E',
    '\u000F',
    '\u001F',
    '\u001F',
    '\u0010',
    ' ',
    ' ',
    '\u0011',
    '!',
    '"',
    '\u0012',
    '#',
    '%',
    '\n',
    '&',
    '*',
    '\u0012',
    '+',
    '+',
    '\t',
    ',',
    ',',
    '\f',
    '-',
    '-',
    '\t',
    '.',
    '.',
    '\f',
    '/',
    '/',
    '\f',
    '0',
    '9',
    '\b',
    ':',
    ':',
    '\f',
    ';',
    '@',
    '\u0012',
    'A',
    'Z',
    char.MinValue,
    '[',
    '`',
    '\u0012',
    'a',
    'z',
    char.MinValue,
    '{',
    '~',
    '\u0012',
    '\u007F',
    '\u0084',
    '\u000E',
    '\u0085',
    '\u0085',
    '\u000F',
    '\u0086',
    '\u009F',
    '\u000E',
    ' ',
    ' ',
    '\f',
    '¡',
    '¡',
    '\u0012',
    '¢',
    '¥',
    '\n',
    '¦',
    '©',
    '\u0012',
    'ª',
    'ª',
    char.MinValue,
    '«',
    '¯',
    '\u0012',
    '°',
    '±',
    '\n',
    '\u00B2',
    '\u00B3',
    '\b',
    '´',
    '´',
    '\u0012',
    'µ',
    'µ',
    char.MinValue,
    '¶',
    '¸',
    '\u0012',
    '\u00B9',
    '\u00B9',
    '\b',
    'º',
    'º',
    char.MinValue,
    '»',
    '¿',
    '\u0012',
    'À',
    'Ö',
    char.MinValue,
    '×',
    '×',
    '\u0012',
    'Ø',
    'ö',
    char.MinValue,
    '÷',
    '÷',
    '\u0012',
    'ø',
    'ʸ',
    char.MinValue,
    'ʹ',
    'ʺ',
    '\u0012',
    'ʻ',
    'ˁ',
    char.MinValue,
    '˂',
    'ˏ',
    '\u0012',
    'ː',
    'ˑ',
    char.MinValue,
    '˒',
    '˟',
    '\u0012',
    'ˠ',
    'ˤ',
    char.MinValue,
    '˥',
    '˭',
    '\u0012',
    'ˮ',
    'ˮ',
    char.MinValue,
    '˯',
    '˿',
    '\u0012',
    '̀',
    '͗',
    '\r',
    '͘',
    '͜',
    char.MinValue,
    '͝',
    'ͯ',
    '\r',
    'Ͱ',
    'ͳ',
    char.MinValue,
    'ʹ',
    '͵',
    '\u0012',
    'Ͷ',
    'ͽ',
    char.MinValue,
    ';',
    ';',
    '\u0012',
    'Ϳ',
    '\u0383',
    char.MinValue,
    '΄',
    '΅',
    '\u0012',
    'Ά',
    'Ά',
    char.MinValue,
    '·',
    '·',
    '\u0012',
    'Έ',
    'ϵ',
    char.MinValue,
    '϶',
    '϶',
    '\u0012',
    'Ϸ',
    '҂',
    char.MinValue,
    '҃',
    '҆',
    '\r',
    '҇',
    '҇',
    char.MinValue,
    '҈',
    '҉',
    '\r',
    'Ҋ',
    '։',
    char.MinValue,
    '֊',
    '֊',
    '\u0012',
    '\u058B',
    '\u0590',
    char.MinValue,
    '֑',
    '֡',
    '\r',
    '֢',
    '֢',
    char.MinValue,
    '֣',
    'ֹ',
    '\r',
    'ֺ',
    'ֺ',
    char.MinValue,
    'ֻ',
    'ֽ',
    '\r',
    '־',
    '־',
    '\u0003',
    'ֿ',
    'ֿ',
    '\r',
    '׀',
    '׀',
    '\u0003',
    'ׁ',
    'ׂ',
    '\r',
    '׃',
    '׃',
    '\u0003',
    'ׄ',
    'ׄ',
    '\r',
    'ׅ',
    '\u05CF',
    char.MinValue,
    'א',
    'ת',
    '\u0003',
    '\u05EB',
    '\u05EF',
    char.MinValue,
    'װ',
    '״',
    '\u0003',
    '\u05F5',
    '\u05FF',
    char.MinValue,
    '\u0600',
    '\u0603',
    '\u0004',
    '\u0604',
    '؋',
    char.MinValue,
    '،',
    '،',
    '\f',
    '؍',
    '؍',
    '\u0004',
    '؎',
    '؏',
    '\u0012',
    'ؐ',
    'ؕ',
    '\r',
    'ؖ',
    'ؚ',
    char.MinValue,
    '؛',
    '؛',
    '\u0004',
    '\u061C',
    '؞',
    char.MinValue,
    '؟',
    '؟',
    '\u0004',
    'ؠ',
    'ؠ',
    char.MinValue,
    'ء',
    'غ',
    '\u0004',
    'ػ',
    'ؿ',
    char.MinValue,
    'ـ',
    'ي',
    '\u0004',
    'ً',
    '٘',
    '\r',
    'ٙ',
    'ٟ',
    char.MinValue,
    '٠',
    '٩',
    '\v',
    '٪',
    '٪',
    '\n',
    '٫',
    '٬',
    '\v',
    '٭',
    'ٯ',
    '\u0004',
    'ٰ',
    'ٰ',
    '\r',
    'ٱ',
    'ە',
    '\u0004',
    'ۖ',
    'ۜ',
    '\r',
    '\u06DD',
    '\u06DD',
    '\u0004',
    '۞',
    'ۤ',
    '\r',
    'ۥ',
    'ۦ',
    '\u0004',
    'ۧ',
    'ۨ',
    '\r',
    '۩',
    '۩',
    '\u0012',
    '۪',
    'ۭ',
    '\r',
    'ۮ',
    'ۯ',
    '\u0004',
    '۰',
    '۹',
    '\b',
    'ۺ',
    '܍',
    '\u0004',
    '\u070E',
    '\u070E',
    char.MinValue,
    '\u070F',
    '\u070F',
    '\u000E',
    'ܐ',
    'ܐ',
    '\u0004',
    'ܑ',
    'ܑ',
    '\r',
    'ܒ',
    'ܯ',
    '\u0004',
    'ܰ',
    '݊',
    '\r',
    '\u074B',
    '\u074C',
    char.MinValue,
    'ݍ',
    'ݏ',
    '\u0004',
    'ݐ',
    'ݿ',
    char.MinValue,
    'ހ',
    'ޥ',
    '\u0004',
    'ަ',
    'ް',
    '\r',
    'ޱ',
    'ޱ',
    '\u0004',
    '\u07B2',
    'ऀ',
    char.MinValue,
    'ँ',
    'ं',
    '\r',
    'ः',
    'ऻ',
    char.MinValue,
    '़',
    '़',
    '\r',
    'ऽ',
    'ी',
    char.MinValue,
    'ु',
    'ै',
    '\r',
    'ॉ',
    'ौ',
    char.MinValue,
    '्',
    '्',
    '\r',
    'ॎ',
    'ॐ',
    char.MinValue,
    '॑',
    '॔',
    '\r',
    'ॕ',
    'ॡ',
    char.MinValue,
    'ॢ',
    'ॣ',
    '\r',
    '।',
    'ঀ',
    char.MinValue,
    'ঁ',
    'ঁ',
    '\r',
    'ং',
    '\u09BB',
    char.MinValue,
    '়',
    '়',
    '\r',
    'ঽ',
    'ী',
    char.MinValue,
    'ু',
    'ৄ',
    '\r',
    '\u09C5',
    'ৌ',
    char.MinValue,
    '্',
    '্',
    '\r',
    'ৎ',
    'ৡ',
    char.MinValue,
    'ৢ',
    'ৣ',
    '\r',
    '\u09E4',
    'ৱ',
    char.MinValue,
    '৲',
    '৳',
    '\n',
    '\u09F4',
    '\u0A00',
    char.MinValue,
    'ਁ',
    'ਂ',
    '\r',
    'ਃ',
    '\u0A3B',
    char.MinValue,
    '਼',
    '਼',
    '\r',
    '\u0A3D',
    'ੀ',
    char.MinValue,
    'ੁ',
    'ੂ',
    '\r',
    '\u0A43',
    '\u0A46',
    char.MinValue,
    'ੇ',
    'ੈ',
    '\r',
    '\u0A49',
    '\u0A4A',
    char.MinValue,
    'ੋ',
    '੍',
    '\r',
    '\u0A4E',
    '੯',
    char.MinValue,
    'ੰ',
    'ੱ',
    '\r',
    'ੲ',
    '\u0A80',
    char.MinValue,
    'ઁ',
    'ં',
    '\r',
    'ઃ',
    '\u0ABB',
    char.MinValue,
    '઼',
    '઼',
    '\r',
    'ઽ',
    'ી',
    char.MinValue,
    'ુ',
    'ૅ',
    '\r',
    '\u0AC6',
    '\u0AC6',
    char.MinValue,
    'ે',
    'ૈ',
    '\r',
    'ૉ',
    'ૌ',
    char.MinValue,
    '્',
    '્',
    '\r',
    '\u0ACE',
    'ૡ',
    char.MinValue,
    'ૢ',
    'ૣ',
    '\r',
    '\u0AE4',
    '૰',
    char.MinValue,
    '૱',
    '૱',
    '\n',
    '\u0AF2',
    '\u0B00',
    char.MinValue,
    'ଁ',
    'ଁ',
    '\r',
    'ଂ',
    '\u0B3B',
    char.MinValue,
    '଼',
    '଼',
    '\r',
    'ଽ',
    'ା',
    char.MinValue,
    'ି',
    'ି',
    '\r',
    'ୀ',
    'ୀ',
    char.MinValue,
    'ୁ',
    'ୃ',
    '\r',
    'ୄ',
    'ୌ',
    char.MinValue,
    '୍',
    '୍',
    '\r',
    '\u0B4E',
    '\u0B55',
    char.MinValue,
    'ୖ',
    'ୖ',
    '\r',
    'ୗ',
    '\u0B81',
    char.MinValue,
    'ஂ',
    'ஂ',
    '\r',
    'ஃ',
    'ி',
    char.MinValue,
    'ீ',
    'ீ',
    '\r',
    'ு',
    'ௌ',
    char.MinValue,
    '்',
    '்',
    '\r',
    '\u0BCE',
    '\u0BF2',
    char.MinValue,
    '௳',
    '௸',
    '\u0012',
    '௹',
    '௹',
    '\n',
    '௺',
    '௺',
    '\u0012',
    '\u0BFB',
    'ఽ',
    char.MinValue,
    'ా',
    'ీ',
    '\r',
    'ు',
    '\u0C45',
    char.MinValue,
    'ె',
    'ై',
    '\r',
    '\u0C49',
    '\u0C49',
    char.MinValue,
    'ొ',
    '్',
    '\r',
    '\u0C4E',
    '\u0C54',
    char.MinValue,
    'ౕ',
    'ౖ',
    '\r',
    '\u0C57',
    '\u0CBB',
    char.MinValue,
    '಼',
    '಼',
    '\r',
    'ಽ',
    'ೋ',
    char.MinValue,
    'ೌ',
    '್',
    '\r',
    '\u0CCE',
    'ീ',
    char.MinValue,
    'ു',
    'ൃ',
    '\r',
    'ൄ',
    'ൌ',
    char.MinValue,
    '്',
    '്',
    '\r',
    'ൎ',
    '\u0DC9',
    char.MinValue,
    '්',
    '්',
    '\r',
    '\u0DCB',
    'ෑ',
    char.MinValue,
    'ි',
    'ු',
    '\r',
    '\u0DD5',
    '\u0DD5',
    char.MinValue,
    'ූ',
    'ූ',
    '\r',
    '\u0DD7',
    'ะ',
    char.MinValue,
    'ั',
    'ั',
    '\r',
    'า',
    'ำ',
    char.MinValue,
    'ิ',
    'ฺ',
    '\r',
    '\u0E3B',
    '\u0E3E',
    char.MinValue,
    '฿',
    '฿',
    '\n',
    'เ',
    'ๆ',
    char.MinValue,
    '็',
    '๎',
    '\r',
    '๏',
    'ະ',
    char.MinValue,
    'ັ',
    'ັ',
    '\r',
    'າ',
    'ຳ',
    char.MinValue,
    'ິ',
    'ູ',
    '\r',
    '\u0EBA',
    '\u0EBA',
    char.MinValue,
    'ົ',
    'ຼ',
    '\r',
    'ຽ',
    '\u0EC7',
    char.MinValue,
    '່',
    'ໍ',
    '\r',
    '\u0ECE',
    '༗',
    char.MinValue,
    '༘',
    '༙',
    '\r',
    '༚',
    '༴',
    char.MinValue,
    '༵',
    '༵',
    '\r',
    '༶',
    '༶',
    char.MinValue,
    '༷',
    '༷',
    '\r',
    '༸',
    '༸',
    char.MinValue,
    '༹',
    '༹',
    '\r',
    '༺',
    '༽',
    '\u0012',
    '༾',
    '\u0F70',
    char.MinValue,
    'ཱ',
    'ཾ',
    '\r',
    'ཿ',
    'ཿ',
    char.MinValue,
    'ྀ',
    '྄',
    '\r',
    '྅',
    '྅',
    char.MinValue,
    '྆',
    '྇',
    '\r',
    'ྈ',
    'ྏ',
    char.MinValue,
    'ྐ',
    'ྗ',
    '\r',
    '\u0F98',
    '\u0F98',
    char.MinValue,
    'ྙ',
    'ྼ',
    '\r',
    '\u0FBD',
    '࿅',
    char.MinValue,
    '࿆',
    '࿆',
    '\r',
    '࿇',
    'ာ',
    char.MinValue,
    'ိ',
    'ူ',
    '\r',
    'ေ',
    'ေ',
    char.MinValue,
    'ဲ',
    'ဲ',
    '\r',
    'ဳ',
    'ဵ',
    char.MinValue,
    'ံ',
    '့',
    '\r',
    'း',
    'း',
    char.MinValue,
    '္',
    '္',
    '\r',
    '်',
    'ၗ',
    char.MinValue,
    'ၘ',
    'ၙ',
    '\r',
    'ၚ',
    'ᙿ',
    char.MinValue,
    ' ',
    ' ',
    '\u0011',
    'ᚁ',
    'ᚚ',
    char.MinValue,
    '᚛',
    '᚜',
    '\u0012',
    '\u169D',
    'ᜑ',
    char.MinValue,
    'ᜒ',
    '᜔',
    '\r',
    '\u1715',
    'ᜱ',
    char.MinValue,
    'ᜲ',
    '᜴',
    '\r',
    '᜵',
    'ᝑ',
    char.MinValue,
    'ᝒ',
    'ᝓ',
    '\r',
    '\u1754',
    '\u1771',
    char.MinValue,
    'ᝲ',
    'ᝳ',
    '\r',
    '\u1774',
    'ា',
    char.MinValue,
    'ិ',
    'ួ',
    '\r',
    'ើ',
    'ៅ',
    char.MinValue,
    'ំ',
    'ំ',
    '\r',
    'ះ',
    'ៈ',
    char.MinValue,
    '៉',
    '៓',
    '\r',
    '។',
    '៚',
    char.MinValue,
    '៛',
    '៛',
    '\n',
    'ៜ',
    'ៜ',
    char.MinValue,
    '៝',
    '៝',
    '\r',
    '\u17DE',
    '\u17EF',
    char.MinValue,
    '\u17F0',
    '\u17F9',
    '\u0012',
    '\u17FA',
    '\u17FF',
    char.MinValue,
    '᠀',
    '᠊',
    '\u0012',
    '᠋',
    '᠍',
    '\r',
    '\u180E',
    '\u180E',
    '\u0011',
    '\u180F',
    'ᢨ',
    char.MinValue,
    'ᢩ',
    'ᢩ',
    '\r',
    'ᢪ',
    '\u191F',
    char.MinValue,
    'ᤠ',
    'ᤢ',
    '\r',
    'ᤣ',
    'ᤦ',
    char.MinValue,
    'ᤧ',
    'ᤫ',
    '\r',
    '\u192C',
    'ᤱ',
    char.MinValue,
    'ᤲ',
    'ᤲ',
    '\r',
    'ᤳ',
    'ᤸ',
    char.MinValue,
    '᤹',
    '᤻',
    '\r',
    '\u193C',
    '\u193F',
    char.MinValue,
    '᥀',
    '᥀',
    '\u0012',
    '\u1941',
    '\u1943',
    char.MinValue,
    '᥄',
    '᥅',
    '\u0012',
    '᥆',
    '᧟',
    char.MinValue,
    '᧠',
    '᧿',
    '\u0012',
    'ᨀ',
    'ᾼ',
    char.MinValue,
    '᾽',
    '᾽',
    '\u0012',
    'ι',
    'ι',
    char.MinValue,
    '᾿',
    '῁',
    '\u0012',
    'ῂ',
    'ῌ',
    char.MinValue,
    '῍',
    '῏',
    '\u0012',
    'ῐ',
    '\u1FDC',
    char.MinValue,
    '῝',
    '῟',
    '\u0012',
    'ῠ',
    'Ῥ',
    char.MinValue,
    '῭',
    '`',
    '\u0012',
    '\u1FF0',
    'ῼ',
    char.MinValue,
    '´',
    '῾',
    '\u0012',
    '\u1FFF',
    '\u1FFF',
    char.MinValue,
    ' ',
    ' ',
    '\u0011',
    '\u200B',
    '\u200D',
    '\u000E',
    '\u200E',
    '\u200E',
    char.MinValue,
    '\u200F',
    '\u200F',
    '\u0003',
    '‐',
    '‧',
    '\u0012',
    '\u2028',
    '\u2028',
    '\u0011',
    '\u2029',
    '\u2029',
    '\u000F',
    '\u202A',
    '\u202A',
    '\u0001',
    '\u202B',
    '\u202B',
    '\u0005',
    '\u202C',
    '\u202C',
    '\a',
    '\u202D',
    '\u202D',
    '\u0002',
    '\u202E',
    '\u202E',
    '\u0006',
    ' ',
    ' ',
    '\u0011',
    '‰',
    '‴',
    '\n',
    '‵',
    '⁔',
    '\u0012',
    '⁕',
    '⁖',
    char.MinValue,
    '⁗',
    '⁗',
    '\u0012',
    '⁘',
    '⁞',
    char.MinValue,
    ' ',
    ' ',
    '\u0011',
    '\u2060',
    '\u2063',
    '\u000E',
    '\u2064',
    '\u2069',
    char.MinValue,
    '\u206A',
    '\u206F',
    '\u000E',
    '\u2070',
    '\u2070',
    '\b',
    'ⁱ',
    '\u2073',
    char.MinValue,
    '\u2074',
    '\u2079',
    '\b',
    '⁺',
    '⁻',
    '\n',
    '⁼',
    '⁾',
    '\u0012',
    'ⁿ',
    'ⁿ',
    char.MinValue,
    '\u2080',
    '\u2089',
    '\b',
    '₊',
    '₋',
    '\n',
    '₌',
    '₎',
    '\u0012',
    '\u208F',
    '\u209F',
    char.MinValue,
    '₠',
    '₱',
    '\n',
    '₲',
    '\u20CF',
    char.MinValue,
    '⃐',
    '⃪',
    '\r',
    '⃫',
    '\u20FF',
    char.MinValue,
    '℀',
    '℁',
    '\u0012',
    'ℂ',
    'ℂ',
    char.MinValue,
    '℃',
    '℆',
    '\u0012',
    'ℇ',
    'ℇ',
    char.MinValue,
    '℈',
    '℉',
    '\u0012',
    'ℊ',
    'ℓ',
    char.MinValue,
    '℔',
    '℔',
    '\u0012',
    'ℕ',
    'ℕ',
    char.MinValue,
    '№',
    '℘',
    '\u0012',
    'ℙ',
    'ℝ',
    char.MinValue,
    '℞',
    '℣',
    '\u0012',
    'ℤ',
    'ℤ',
    char.MinValue,
    '℥',
    '℥',
    '\u0012',
    'Ω',
    'Ω',
    char.MinValue,
    '℧',
    '℧',
    '\u0012',
    'ℨ',
    'ℨ',
    char.MinValue,
    '℩',
    '℩',
    '\u0012',
    'K',
    'ℭ',
    char.MinValue,
    '℮',
    '℮',
    '\n',
    'ℯ',
    'ℱ',
    char.MinValue,
    'Ⅎ',
    'Ⅎ',
    '\u0012',
    'ℳ',
    'ℹ',
    char.MinValue,
    '℺',
    '℻',
    '\u0012',
    'ℼ',
    'ℿ',
    char.MinValue,
    '⅀',
    '⅄',
    '\u0012',
    'ⅅ',
    'ⅉ',
    char.MinValue,
    '⅊',
    '⅋',
    '\u0012',
    '⅌',
    '\u2152',
    char.MinValue,
    '\u2153',
    '\u215F',
    '\u0012',
    'Ⅰ',
    '\u218F',
    char.MinValue,
    '←',
    '∑',
    '\u0012',
    '−',
    '∓',
    '\n',
    '∔',
    '⌵',
    '\u0012',
    '⌶',
    '⍺',
    char.MinValue,
    '⍻',
    '⎔',
    '\u0012',
    '⎕',
    '⎕',
    char.MinValue,
    '⎖',
    '⏐',
    '\u0012',
    '⏑',
    '\u23FF',
    char.MinValue,
    '␀',
    '␦',
    '\u0012',
    '\u2427',
    '\u243F',
    char.MinValue,
    '⑀',
    '⑊',
    '\u0012',
    '\u244B',
    '\u245F',
    char.MinValue,
    '\u2460',
    '\u249B',
    '\b',
    '⒜',
    'ⓩ',
    char.MinValue,
    '\u24EA',
    '\u24EA',
    '\b',
    '\u24EB',
    '☗',
    '\u0012',
    '☘',
    '☘',
    char.MinValue,
    '☙',
    '♽',
    '\u0012',
    '♾',
    '♿',
    char.MinValue,
    '⚀',
    '⚑',
    '\u0012',
    '⚒',
    '⚟',
    char.MinValue,
    '⚠',
    '⚡',
    '\u0012',
    '⚢',
    '✀',
    char.MinValue,
    '✁',
    '✄',
    '\u0012',
    '✅',
    '✅',
    char.MinValue,
    '✆',
    '✉',
    '\u0012',
    '✊',
    '✋',
    char.MinValue,
    '✌',
    '✧',
    '\u0012',
    '✨',
    '✨',
    char.MinValue,
    '✩',
    '❋',
    '\u0012',
    '❌',
    '❌',
    char.MinValue,
    '❍',
    '❍',
    '\u0012',
    '❎',
    '❎',
    char.MinValue,
    '❏',
    '❒',
    '\u0012',
    '❓',
    '❕',
    char.MinValue,
    '❖',
    '❖',
    '\u0012',
    '❗',
    '❗',
    char.MinValue,
    '❘',
    '❞',
    '\u0012',
    '❟',
    '❠',
    char.MinValue,
    '❡',
    '➔',
    '\u0012',
    '➕',
    '➗',
    char.MinValue,
    '➘',
    '➯',
    '\u0012',
    '➰',
    '➰',
    char.MinValue,
    '➱',
    '➾',
    '\u0012',
    '➿',
    '⟏',
    char.MinValue,
    '⟐',
    '⟫',
    '\u0012',
    '⟬',
    '⟯',
    char.MinValue,
    '⟰',
    '⬍',
    '\u0012',
    '⬎',
    '\u2E7F',
    char.MinValue,
    '⺀',
    '⺙',
    '\u0012',
    '\u2E9A',
    '\u2E9A',
    char.MinValue,
    '⺛',
    '⻳',
    '\u0012',
    '\u2EF4',
    '\u2EFF',
    char.MinValue,
    '⼀',
    '⿕',
    '\u0012',
    '\u2FD6',
    '\u2FEF',
    char.MinValue,
    '⿰',
    '⿻',
    '\u0012',
    '\u2FFC',
    '\u2FFF',
    char.MinValue,
    '　',
    '　',
    '\u0011',
    '、',
    '〄',
    '\u0012',
    '々',
    '〇',
    char.MinValue,
    '〈',
    '〠',
    '\u0012',
    '〡',
    '〩',
    char.MinValue,
    '〪',
    '〯',
    '\r',
    '〰',
    '〰',
    '\u0012',
    '〱',
    '〵',
    char.MinValue,
    '〶',
    '〷',
    '\u0012',
    '〸',
    '〼',
    char.MinValue,
    '〽',
    '〿',
    '\u0012',
    '\u3040',
    '\u3098',
    char.MinValue,
    '゙',
    '゚',
    '\r',
    '゛',
    '゜',
    '\u0012',
    'ゝ',
    'ゟ',
    char.MinValue,
    '゠',
    '゠',
    '\u0012',
    'ァ',
    'ヺ',
    char.MinValue,
    '・',
    '・',
    '\u0012',
    'ー',
    '㈜',
    char.MinValue,
    '㈝',
    '㈞',
    '\u0012',
    '\u321F',
    '\u324F',
    char.MinValue,
    '㉐',
    '\u325F',
    '\u0012',
    '㉠',
    '㉻',
    char.MinValue,
    '㉼',
    '㉽',
    '\u0012',
    '㉾',
    '㊰',
    char.MinValue,
    '\u32B1',
    '\u32BF',
    '\u0012',
    '㋀',
    '㋋',
    char.MinValue,
    '㋌',
    '㋏',
    '\u0012',
    '㋐',
    '㍶',
    char.MinValue,
    '㍷',
    '㍺',
    '\u0012',
    '㍻',
    '㏝',
    char.MinValue,
    '㏞',
    '㏟',
    '\u0012',
    '㏠',
    '㏾',
    char.MinValue,
    '㏿',
    '㏿',
    '\u0012',
    '㐀',
    '\u4DBF',
    char.MinValue,
    '䷀',
    '䷿',
    '\u0012',
    '一',
    '\uA48F',
    char.MinValue,
    '꒐',
    '꓆',
    '\u0012',
    '\uA4C7',
    '\uFB1C',
    char.MinValue,
    'יִ',
    'יִ',
    '\u0003',
    'ﬞ',
    'ﬞ',
    '\r',
    'ײַ',
    'ﬨ',
    '\u0003',
    '﬩',
    '﬩',
    '\n',
    'שׁ',
    'זּ',
    '\u0003',
    '\uFB37',
    '\uFB37',
    char.MinValue,
    'טּ',
    'לּ',
    '\u0003',
    '\uFB3D',
    '\uFB3D',
    char.MinValue,
    'מּ',
    'מּ',
    '\u0003',
    '\uFB3F',
    '\uFB3F',
    char.MinValue,
    'נּ',
    'סּ',
    '\u0003',
    '\uFB42',
    '\uFB42',
    char.MinValue,
    'ףּ',
    'פּ',
    '\u0003',
    '\uFB45',
    '\uFB45',
    char.MinValue,
    'צּ',
    'ﭏ',
    '\u0003',
    'ﭐ',
    'ﮱ',
    '\u0004',
    '﮲',
    '\uFBD2',
    char.MinValue,
    'ﯓ',
    'ﴽ',
    '\u0004',
    '﴾',
    '﴿',
    '\u0012',
    '\uFD40',
    '\uFD4F',
    char.MinValue,
    'ﵐ',
    'ﶏ',
    '\u0004',
    '\uFD90',
    '\uFD91',
    char.MinValue,
    'ﶒ',
    'ﷇ',
    '\u0004',
    '\uFDC8',
    '\uFDEF',
    char.MinValue,
    'ﷰ',
    '﷼',
    '\u0004',
    '﷽',
    '﷽',
    '\u0012',
    '\uFDFE',
    '\uFDFF',
    char.MinValue,
    '︀',
    '️',
    '\r',
    '︐',
    '\uFE1F',
    char.MinValue,
    '︠',
    '︣',
    '\r',
    '︤',
    '︯',
    char.MinValue,
    '︰',
    '﹏',
    '\u0012',
    '﹐',
    '﹐',
    '\f',
    '﹑',
    '﹑',
    '\u0012',
    '﹒',
    '﹒',
    '\f',
    '\uFE53',
    '\uFE53',
    char.MinValue,
    '﹔',
    '﹔',
    '\u0012',
    '﹕',
    '﹕',
    '\f',
    '﹖',
    '﹞',
    '\u0012',
    '﹟',
    '﹟',
    '\n',
    '﹠',
    '﹡',
    '\u0012',
    '﹢',
    '﹣',
    '\n',
    '﹤',
    '﹦',
    '\u0012',
    '\uFE67',
    '\uFE67',
    char.MinValue,
    '﹨',
    '﹨',
    '\u0012',
    '﹩',
    '﹪',
    '\n',
    '﹫',
    '﹫',
    '\u0012',
    '\uFE6C',
    '\uFE6F',
    char.MinValue,
    'ﹰ',
    'ﹴ',
    '\u0004',
    '\uFE75',
    '\uFE75',
    char.MinValue,
    'ﹶ',
    'ﻼ',
    '\u0004',
    '\uFEFD',
    '\uFEFE',
    char.MinValue,
    '\uFEFF',
    '\uFEFF',
    '\u000E',
    '\uFF00',
    '\uFF00',
    char.MinValue,
    '！',
    '＂',
    '\u0012',
    '＃',
    '％',
    '\n',
    '＆',
    '＊',
    '\u0012',
    '＋',
    '＋',
    '\n',
    '，',
    '，',
    '\f',
    '－',
    '－',
    '\n',
    '．',
    '．',
    '\f',
    '／',
    '／',
    '\t',
    '０',
    '９',
    '\b',
    '：',
    '：',
    '\f',
    '；',
    '＠',
    '\u0012',
    'Ａ',
    'Ｚ',
    char.MinValue,
    '［',
    '｀',
    '\u0012',
    'ａ',
    'ｚ',
    char.MinValue,
    '｛',
    '･',
    '\u0012',
    'ｦ',
    '\uFFDF',
    char.MinValue,
    '￠',
    '￡',
    '\n',
    '￢',
    '￤',
    '\u0012',
    '￥',
    '￦',
    '\n',
    '\uFFE7',
    '\uFFE7',
    char.MinValue,
    '￨',
    '￮',
    '\u0012',
    '\uFFEF',
    '\uFFF8',
    char.MinValue,
    '\uFFF9',
    '\uFFFB',
    '\u000E',
    '￼',
    '�',
    '\u0012',
    '\uFFFE',
    char.MaxValue,
    char.MinValue
  };

  internal RTLCharacters()
  {
    int num1;
    for (int index = 0; index < this.CharTypes.Length; index = num1 + 1)
    {
      int charType1 = (int) this.CharTypes[index];
      int num2;
      int charType2 = (int) this.CharTypes[num2 = index + 1];
      sbyte charType3 = (sbyte) this.CharTypes[num1 = num2 + 1];
      while (charType1 <= charType2)
        this.m_rtlCharacterTypes[charType1++] = charType3;
    }
  }

  internal byte[] GetVisualOrder(OtfGlyphInfo[] inputText, bool isRTL)
  {
    this.m_types = this.GetCharacterCode(inputText);
    this.m_textOrder = isRTL ? (sbyte) 1 : (sbyte) 0;
    this.DoVisualOrder();
    byte[] visualOrder = new byte[this.m_result.Length];
    for (int index = 0; index < this.m_levels.Length; ++index)
      visualOrder[index] = (byte) this.m_levels[index];
    return visualOrder;
  }

  internal byte[] GetVisualOrder(string inputText, bool isRTL)
  {
    this.m_types = this.GetCharacterCode(inputText);
    this.m_textOrder = isRTL ? (sbyte) 1 : (sbyte) 0;
    this.DoVisualOrder();
    byte[] visualOrder = new byte[this.m_result.Length];
    for (int index = 0; index < this.m_levels.Length; ++index)
      visualOrder[index] = (byte) this.m_levels[index];
    return visualOrder;
  }

  private sbyte[] GetCharacterCode(string text)
  {
    sbyte[] characterCode = new sbyte[text.Length];
    for (int index = 0; index < text.Length; ++index)
      characterCode[index] = this.m_rtlCharacterTypes[(int) text[index]];
    return characterCode;
  }

  private sbyte[] GetCharacterCode(OtfGlyphInfo[] text)
  {
    sbyte[] characterCode = new sbyte[text.Length];
    for (int index1 = 0; index1 < text.Length; ++index1)
    {
      char index2 = text[index1].CharCode > 0 ? (char) text[index1].CharCode : (text[index1].Characters != null ? text[index1].Characters[0] : char.MinValue);
      characterCode[index1] = this.m_rtlCharacterTypes[(int) index2];
    }
    return characterCode;
  }

  private void SetDefaultLevels()
  {
    for (int index = 0; index < this.m_length; ++index)
      this.m_levels[index] = this.m_textOrder;
  }

  private void SetLevels()
  {
    this.SetDefaultLevels();
    for (int index = 0; index < this.m_length; ++index)
    {
      sbyte level = this.m_levels[index];
      if (((int) level & 128 /*0x80*/) != 0)
      {
        level &= sbyte.MaxValue;
        this.m_result[index] = ((int) level & 1) == 0 ? (sbyte) 0 : (sbyte) 3;
      }
      this.m_levels[index] = level;
    }
  }

  private void UpdateLevels(int index, sbyte level, int length)
  {
    if (((int) level & 1) == 0)
    {
      for (int index1 = index; index1 < length; ++index1)
      {
        if (this.m_result[index1] == (sbyte) 3)
          ++this.m_levels[index1];
        else if (this.m_result[index1] != (sbyte) 0)
          this.m_levels[index1] += (sbyte) 2;
      }
    }
    else
    {
      for (int index2 = index; index2 < length; ++index2)
      {
        if (this.m_result[index2] != (sbyte) 3)
          ++this.m_levels[index2];
      }
    }
  }

  private void DoVisualOrder()
  {
    this.m_length = this.m_types.Length;
    this.m_result = (sbyte[]) this.m_types.Clone();
    this.m_levels = new sbyte[this.m_length];
    this.SetLevels();
    this.m_length = this.GetEmbeddedCharactersLength();
    sbyte val1 = this.m_textOrder;
    int length;
    for (int index = 0; index < this.m_length; index = length)
    {
      sbyte level = this.m_levels[index];
      sbyte startType = ((int) Math.Max(val1, level) & 1) == 0 ? (sbyte) 0 : (sbyte) 3;
      length = index + 1;
      while (length < this.m_length && (int) this.m_levels[length] == (int) level)
        ++length;
      sbyte endType = ((int) Math.Max(length < this.m_length ? this.m_levels[length] : this.m_textOrder, level) & 1) == 0 ? (sbyte) 0 : (sbyte) 3;
      this.CheckNSM(index, length, level, startType, endType);
      this.UpdateLevels(index, level, length);
      val1 = level;
    }
    this.CheckEmbeddedCharacters(this.m_length);
  }

  private int GetEmbeddedCharactersLength()
  {
    int charactersLength = 0;
    for (int index = 0; index < this.m_length; ++index)
    {
      if (this.m_types[index] != (sbyte) 1 && this.m_types[index] != (sbyte) 5 && this.m_types[index] != (sbyte) 2 && this.m_types[index] != (sbyte) 6 && this.m_types[index] != (sbyte) 7 && this.m_types[index] != (sbyte) 14)
      {
        this.m_result[charactersLength] = this.m_result[index];
        this.m_levels[charactersLength] = this.m_levels[index];
        ++charactersLength;
      }
    }
    return charactersLength;
  }

  private void CheckEmbeddedCharacters(int length)
  {
    for (int index = this.m_types.Length - 1; index >= 0; --index)
    {
      if (this.m_types[index] == (sbyte) 1 || this.m_types[index] == (sbyte) 5 || this.m_types[index] == (sbyte) 2 || this.m_types[index] == (sbyte) 6 || this.m_types[index] == (sbyte) 7 || this.m_types[index] == (sbyte) 14)
      {
        this.m_result[index] = this.m_types[index];
        this.m_levels[index] = (sbyte) -1;
      }
      else
      {
        --length;
        this.m_result[index] = this.m_result[length];
        this.m_levels[index] = this.m_levels[length];
      }
    }
    for (int index = 0; index < this.m_types.Length; ++index)
    {
      if (this.m_levels[index] == (sbyte) -1)
        this.m_levels[index] = index != 0 ? this.m_levels[index - 1] : this.m_textOrder;
    }
  }

  private void CheckNSM(int index, int length, sbyte level, sbyte startType, sbyte endType)
  {
    sbyte num = startType;
    for (int index1 = index; index1 < length; ++index1)
    {
      if (this.m_result[index1] == (sbyte) 13)
        this.m_result[index1] = num;
      else
        num = this.m_result[index1];
    }
    this.CheckEuropeanDigits(index, length, level, startType, endType);
  }

  private void CheckEuropeanDigits(
    int index,
    int length,
    sbyte level,
    sbyte startType,
    sbyte endType)
  {
    for (int index1 = index; index1 < length; ++index1)
    {
      if (this.m_result[index1] == (sbyte) 8)
      {
        for (int index2 = index1 - 1; index2 >= index; --index2)
        {
          if (this.m_result[index2] == (sbyte) 0 || this.m_result[index2] == (sbyte) 3 || this.m_result[index2] == (sbyte) 4)
          {
            if (this.m_result[index2] == (sbyte) 4)
            {
              this.m_result[index1] = (sbyte) 11;
              break;
            }
            break;
          }
        }
      }
    }
    this.CheckArabicCharacters(index, length, level, startType, endType);
  }

  private void CheckArabicCharacters(
    int index,
    int length,
    sbyte level,
    sbyte startType,
    sbyte endType)
  {
    for (int index1 = index; index1 < length; ++index1)
    {
      if (this.m_result[index1] == (sbyte) 4)
        this.m_result[index1] = (sbyte) 3;
    }
    this.CheckEuropeanNumberSeparator(index, length, level, startType, endType);
  }

  private void CheckEuropeanNumberSeparator(
    int index,
    int length,
    sbyte level,
    sbyte startType,
    sbyte endType)
  {
    for (int index1 = index + 1; index1 < length - 1; ++index1)
    {
      if (this.m_result[index1] == (sbyte) 9 || this.m_result[index1] == (sbyte) 12)
      {
        sbyte num1 = this.m_result[index1 - 1];
        sbyte num2 = this.m_result[index1 + 1];
        if (num1 == (sbyte) 8 && num2 == (sbyte) 8)
          this.m_result[index1] = (sbyte) 8;
        else if (this.m_result[index1] == (sbyte) 12 && num1 == (sbyte) 11 && num2 == (sbyte) 11)
          this.m_result[index1] = (sbyte) 11;
      }
    }
    this.CheckEuropeanNumberTerminator(index, length, level, startType, endType);
  }

  private void CheckEuropeanNumberTerminator(
    int index,
    int length,
    sbyte level,
    sbyte startType,
    sbyte endType)
  {
    for (int index1 = index; index1 < length; ++index1)
    {
      if (this.m_result[index1] == (sbyte) 10)
      {
        int index2 = index1;
        int length1 = this.GetLength(index2, length, new sbyte[1]
        {
          (sbyte) 10
        });
        sbyte num = index2 == index ? startType : this.m_result[index2 - 1];
        if (num != (sbyte) 8)
          num = length1 == length ? endType : this.m_result[length1];
        if (num == (sbyte) 8)
        {
          for (int index3 = index2; index3 < length1; ++index3)
            this.m_result[index3] = (sbyte) 8;
        }
        index1 = length1;
      }
    }
    this.CheckOtherNeutrals(index, length, level, startType, endType);
  }

  private void CheckOtherNeutrals(
    int index,
    int length,
    sbyte level,
    sbyte startType,
    sbyte endType)
  {
    for (int index1 = index; index1 < length; ++index1)
    {
      if (this.m_result[index1] == (sbyte) 9 || this.m_result[index1] == (sbyte) 10 || this.m_result[index1] == (sbyte) 12)
        this.m_result[index1] = (sbyte) 18;
    }
    this.CheckOtherCharacters(index, length, level, startType, endType);
  }

  private void CheckOtherCharacters(
    int index,
    int length,
    sbyte level,
    sbyte startType,
    sbyte endType)
  {
    for (int index1 = index; index1 < length; ++index1)
    {
      if (this.m_result[index1] == (sbyte) 8)
      {
        sbyte num = startType;
        for (int index2 = index1 - 1; index2 >= index; --index2)
        {
          if (this.m_result[index2] == (sbyte) 0 || this.m_result[index2] == (sbyte) 3)
          {
            num = this.m_result[index2];
            break;
          }
        }
        if (num == (sbyte) 0)
          this.m_result[index1] = (sbyte) 0;
      }
    }
    this.CheckCommanCharacters(index, length, level, startType, endType);
  }

  private int GetLength(int index, int length, sbyte[] validSet)
  {
    --index;
    if (++index >= length)
      return length;
    sbyte num = this.m_result[index];
    for (int index1 = 0; index1 < validSet.Length; ++index1)
    {
      if ((int) num == (int) validSet[index1])
        index = this.GetLength(++index, length, validSet);
    }
    return index;
  }

  private void CheckCommanCharacters(
    int index,
    int length,
    sbyte level,
    sbyte startType,
    sbyte endType)
  {
    for (int index1 = index; index1 < length; ++index1)
    {
      if (this.m_result[index1] == (sbyte) 17 || this.m_result[index1] == (sbyte) 18 || this.m_result[index1] == (sbyte) 15 || this.m_result[index1] == (sbyte) 16 /*0x10*/)
      {
        int index2 = index1;
        int length1 = this.GetLength(index2, length, new sbyte[4]
        {
          (sbyte) 15,
          (sbyte) 16 /*0x10*/,
          (sbyte) 17,
          (sbyte) 18
        });
        sbyte num1;
        if (index2 == index)
        {
          num1 = startType;
        }
        else
        {
          num1 = this.m_result[index2 - 1];
          switch (num1)
          {
            case 8:
              num1 = (sbyte) 3;
              break;
            case 11:
              num1 = (sbyte) 3;
              break;
          }
        }
        sbyte num2;
        if (length1 == length)
        {
          num2 = endType;
        }
        else
        {
          num2 = this.m_result[length1];
          switch (num2)
          {
            case 8:
              num2 = (sbyte) 3;
              break;
            case 11:
              num2 = (sbyte) 3;
              break;
          }
        }
        sbyte num3 = (int) num1 != (int) num2 ? (((int) level & 1) == 0 ? (sbyte) 0 : (sbyte) 3) : num1;
        for (int index3 = index2; index3 < length1; ++index3)
          this.m_result[index3] = num3;
        index1 = length1;
      }
    }
  }
}
