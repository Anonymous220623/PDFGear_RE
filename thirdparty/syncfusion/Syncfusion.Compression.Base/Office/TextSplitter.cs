// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.TextSplitter
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class TextSplitter
{
  private static readonly char[] WordSplitCharacters = new char[41]
  {
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
    ':',
    ';',
    '<',
    '=',
    '>',
    '?',
    '@',
    '[',
    '\\',
    ']',
    '^',
    '_',
    '`',
    '{',
    '|',
    '}',
    '~',
    '،',
    '؛',
    '؟',
    '‘',
    '’',
    '”',
    '　',
    '\u200F'
  };
  private static readonly char[] NumberNonReversingCharacters = new char[5]
  {
    ',',
    '.',
    '/',
    ':',
    '،'
  };

  internal static bool IsEastAsiaScript(FontScriptType scriptType)
  {
    return scriptType == FontScriptType.Japanese || scriptType == FontScriptType.Korean || scriptType == FontScriptType.Chinese;
  }

  private FontScriptType GetFontScriptType(char inputCharacter)
  {
    if (TextSplitter.IsHindiChar(inputCharacter))
      return FontScriptType.Hindi;
    if (TextSplitter.IsKoreanChar(inputCharacter))
      return FontScriptType.Korean;
    if (TextSplitter.IsJapanese(inputCharacter))
      return FontScriptType.Japanese;
    if (TextSplitter.IsChineseChar(inputCharacter))
      return FontScriptType.Chinese;
    if (TextSplitter.IsArabicChar(inputCharacter))
      return FontScriptType.Arabic;
    return TextSplitter.IsHebrewChar(inputCharacter) ? FontScriptType.Hebrew : FontScriptType.English;
  }

  internal string[] SplitTextByFontScriptType(
    string inputText,
    ref List<FontScriptType> fontScriptTypes)
  {
    List<string> stringList = new List<string>();
    if (string.IsNullOrEmpty(inputText))
      return stringList.ToArray();
    string empty1 = string.Empty;
    FontScriptType fontScriptType1 = FontScriptType.English;
    FontScriptType fontScriptType2 = FontScriptType.English;
    for (int index = 0; index < inputText.Length; ++index)
    {
      if (inputText[index] != ' ' && !char.IsHighSurrogate(inputText[index]) && !char.IsLowSurrogate(inputText[index]))
        fontScriptType2 = this.GetFontScriptType(inputText[index]);
      if (empty1 != string.Empty && fontScriptType2 != fontScriptType1)
      {
        stringList.Add(empty1);
        fontScriptTypes.Add(fontScriptType1);
        empty1 = string.Empty;
      }
      empty1 += (string) (object) inputText[index];
      fontScriptType1 = fontScriptType2;
    }
    if (empty1 != string.Empty)
    {
      stringList.Add(empty1);
      fontScriptTypes.Add(fontScriptType2);
      string empty2 = string.Empty;
    }
    return stringList.ToArray();
  }

  internal string[] SplitTextByConsecutiveLtrAndRtl(
    string text,
    bool isTextBidi,
    bool isRTLLang,
    ref List<CharacterRangeType> characterRangeTypes,
    ref bool? isPrevLTRText,
    ref bool hasRTLCharacter)
  {
    int count = characterRangeTypes.Count;
    List<string> stringList = new List<string>();
    if (string.IsNullOrEmpty(text))
      return stringList.ToArray();
    int num1 = -1;
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string str = string.Empty;
    string empty3 = string.Empty;
    for (int index = 0; index < text.Length; ++index)
    {
      int num2 = 0;
      bool flag = false;
      if ((isPrevLTRText.HasValue ? (!isPrevLTRText.Value ? 1 : 0) : (isTextBidi ? 1 : 0)) != 0 && char.IsNumber(text[index]))
      {
        empty3 += (string) (object) text[index];
        num2 = 4;
      }
      else if (TextSplitter.IsWordSplitChar(text[index]))
      {
        num2 = 2;
        str = !(flag = isTextBidi || (byte) text[index] == (byte) 32 /*0x20*/ && str == string.Empty) ? str + (object) text[index] : str + (object) text[index];
      }
      else if (TextSplitter.IsRTLChar(text[index]) && !char.IsNumber(text[index]))
      {
        isPrevLTRText = new bool?(false);
        hasRTLCharacter = true;
        empty2 += (string) (object) text[index];
        num2 = 1;
      }
      else
      {
        isPrevLTRText = new bool?(true);
        empty1 += (string) (object) text[index];
      }
      if (empty3 != string.Empty && num2 != 4)
      {
        stringList.Add(empty3);
        characterRangeTypes.Add(CharacterRangeType.Number);
        empty3 = string.Empty;
      }
      if (empty2 != string.Empty && num2 != 1)
      {
        stringList.Add(empty2);
        characterRangeTypes.Add(CharacterRangeType.RTL);
        empty2 = string.Empty;
      }
      if (empty1 != string.Empty && num2 != 0)
      {
        stringList.Add(empty1);
        num1 = stringList.Count - 1;
        characterRangeTypes.Add(CharacterRangeType.LTR);
        empty1 = string.Empty;
      }
      if (str != string.Empty && (num2 != 2 || flag))
      {
        stringList.Add(str);
        characterRangeTypes.Add(CharacterRangeType.WordSplit);
        str = string.Empty;
      }
    }
    if (empty3 != string.Empty)
    {
      stringList.Add(empty3);
      characterRangeTypes.Add(CharacterRangeType.Number);
    }
    else if (empty2 != string.Empty)
    {
      stringList.Add(empty2);
      characterRangeTypes.Add(CharacterRangeType.RTL);
    }
    else if (empty1 != string.Empty)
    {
      stringList.Add(empty1);
      num1 = stringList.Count - 1;
      characterRangeTypes.Add(CharacterRangeType.LTR);
    }
    else if (str != string.Empty)
    {
      stringList.Add(str);
      characterRangeTypes.Add(CharacterRangeType.WordSplit);
    }
    if (hasRTLCharacter || isPrevLTRText.HasValue && !isPrevLTRText.Value)
    {
      for (int index = 1; index < stringList.Count; ++index)
      {
        if (characterRangeTypes[index + count] == CharacterRangeType.WordSplit && stringList[index].Length == 1 && index + count + 1 < characterRangeTypes.Count && characterRangeTypes[index + count - 1] != CharacterRangeType.WordSplit && (characterRangeTypes[index + count - 1] != CharacterRangeType.Number || TextSplitter.IsNumberNonReversingCharacter(stringList[index], isTextBidi)) && characterRangeTypes[index + count - 1] == characterRangeTypes[index + count + 1])
        {
          stringList[index - 1] = stringList[index - 1] + stringList[index] + stringList[index + 1];
          stringList.RemoveAt(index);
          stringList.RemoveAt(index);
          characterRangeTypes.RemoveAt(index + count);
          characterRangeTypes.RemoveAt(index + count);
          --index;
        }
      }
    }
    else if (num1 != -1)
    {
      if (isTextBidi)
      {
        for (int index1 = 1; index1 < num1; ++index1)
        {
          if (characterRangeTypes[index1 + count] == CharacterRangeType.WordSplit && index1 < num1 && characterRangeTypes[index1 + count - 1] == CharacterRangeType.LTR)
          {
            string empty4 = string.Empty;
            int num3;
            for (int index2 = index1 + 1; index2 <= num1; index2 = num3 + 1)
            {
              empty4 += stringList[index2];
              stringList.RemoveAt(index2);
              characterRangeTypes.RemoveAt(index2 + count);
              num3 = index2 - 1;
              --num1;
            }
            stringList[index1 - 1] = stringList[index1 - 1] + stringList[index1] + empty4;
            stringList.RemoveAt(index1);
            characterRangeTypes.RemoveAt(index1 + count);
            --index1;
            --num1;
          }
        }
      }
      else
      {
        stringList.Clear();
        stringList.Add(text);
      }
    }
    else if (!isTextBidi)
    {
      stringList.Clear();
      stringList.Add(text);
    }
    if (isTextBidi)
    {
      for (int index = 1; index < stringList.Count; ++index)
      {
        CharacterRangeType characterRangeType = characterRangeTypes[index + count];
        if (characterRangeType == CharacterRangeType.WordSplit && stringList[index].Length == 1 && index + count + 1 < characterRangeTypes.Count && characterRangeTypes[index + count - 1] != CharacterRangeType.WordSplit && (characterRangeTypes[index + count - 1] != CharacterRangeType.Number || TextSplitter.IsNumberNonReversingCharacter(stringList[index], isTextBidi) || !isRTLLang) && characterRangeTypes[index + count - 1] == characterRangeTypes[index + count + 1])
        {
          stringList[index - 1] = stringList[index - 1] + stringList[index] + stringList[index + 1];
          stringList.RemoveAt(index);
          stringList.RemoveAt(index);
          characterRangeTypes.RemoveAt(index + count);
          characterRangeTypes.RemoveAt(index + count);
          --index;
        }
        else if (characterRangeType == CharacterRangeType.WordSplit && characterRangeTypes[index + count - 1] == CharacterRangeType.Number && this.IsNonWordSplitCharacter(stringList[index]) && !isRTLLang)
        {
          stringList[index - 1] = stringList[index - 1] + stringList[index];
          stringList.RemoveAt(index);
          characterRangeTypes.RemoveAt(index + count);
          --index;
        }
        else if (characterRangeType == CharacterRangeType.LTR && (characterRangeTypes[index + count - 1] == CharacterRangeType.Number || characterRangeTypes[index + count - 1] == CharacterRangeType.LTR))
        {
          stringList[index - 1] = stringList[index - 1] + stringList[index];
          characterRangeTypes[index + count - 1] = CharacterRangeType.LTR;
          stringList.RemoveAt(index);
          characterRangeTypes.RemoveAt(index + count);
          --index;
        }
      }
    }
    return stringList.ToArray();
  }

  internal static bool IsRTLChar(char character)
  {
    if (TextSplitter.IsHebrewChar(character) || TextSplitter.IsArabicChar(character) || character >= 'ꦀ' && character <= '꧟' || character >= '܀' && character <= 'ݏ' || character >= 'ހ' && character <= '\u07BF' || character >= 'ࡀ' && character <= '\u085F' || character >= '߀' && character <= '\u07FF')
      return true;
    return character >= 'ࠀ' && character <= '\u083F';
  }

  private bool IsNonWordSplitCharacter(string character)
  {
    bool flag = false;
    foreach (char ch in character)
    {
      switch (ch)
      {
        case '#':
        case '$':
        case '%':
          flag = true;
          continue;
        default:
          flag = false;
          goto label_5;
      }
    }
label_5:
    return flag;
  }

  internal static bool IsNumberNonReversingCharacter(string character, bool isTextBidi)
  {
    foreach (char reversingCharacter in TextSplitter.NumberNonReversingCharacters)
    {
      if ((int) character[0] == (int) reversingCharacter && (reversingCharacter == '/' ? (!isTextBidi ? 1 : 0) : 1) != 0)
        return true;
    }
    return false;
  }

  internal static bool IsWordSplitChar(char character)
  {
    foreach (char wordSplitCharacter in TextSplitter.WordSplitCharacters)
    {
      if ((int) character == (int) wordSplitCharacter)
        return true;
    }
    return false;
  }

  private static bool IsArabicChar(char character)
  {
    if (character >= '\u0600' && character <= 'ۿ' || character >= 'ݐ' && character <= 'ݿ' || character >= 'ࢠ' && character <= 'ࣿ' || character >= 'ﭐ' && character <= '\uFDFF')
      return true;
    return character >= 'ﹰ' && character <= '\uFEFF';
  }

  private static bool IsHebrewChar(char character)
  {
    if (character >= '\u0590' && character <= '\u05FF')
      return true;
    return character >= 'יִ' && character <= 'ﭏ';
  }

  private static bool IsHindiChar(char character)
  {
    if (character >= 'ऀ' && character <= 'ॿ' || character >= '꣠' && character <= '\uA8FF')
      return true;
    return character >= '᳐' && character <= '\u1CFF';
  }

  private static bool IsKoreanChar(char character)
  {
    if (character >= '가' && character <= '힣' || character >= 'ᄀ' && character <= 'ᇿ' || character >= '\u3130' && character <= '\u318F' || character >= 'ꥠ' && character <= '\uA97F' || character >= 'ힰ' && character <= '\uD7FF')
      return true;
    return character >= '가' && character <= '\uD7AF';
  }

  private static bool IsJapanese(char character)
  {
    if (character >= '゠' && character <= 'ヿ')
      return true;
    return character >= '\u3040' && character <= 'ゟ';
  }

  private static bool IsChineseChar(char character)
  {
    if (character >= '一' && character <= '\u9FFF' || character >= '㐀' && character <= '\u4DBF' || character >= '\uD840' && character <= '\uD869' || character >= '\uDC00' && character <= '\uDEDF' || character >= 'ꥠ' && character <= '\uA97F' || character >= '\uFF00' && character <= '\uFFEF')
      return true;
    return character >= '　' && character <= '〿';
  }
}
