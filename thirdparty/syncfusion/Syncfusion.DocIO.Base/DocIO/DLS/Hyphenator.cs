// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Hyphenator
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class Hyphenator
{
  private const char marker = '.';
  private Dictionary<string, List<int>> m_patterns;
  private static Dictionary<string, Stream> m_dictionaries;
  private static Dictionary<string, Hyphenator> m_loadedHyphenators;
  private int leftMin = 2;
  private int rightMin = 3;

  internal Hyphenator()
  {
  }

  internal Hyphenator(Stream file) => this.LoadPattern(file);

  public static void UnloadDictionaries()
  {
    Hyphenator.LoadedHyphenators.Clear();
    Hyphenator.Dictionaries.Clear();
  }

  public event AddDictionaryEventHandler AddDictionary;

  internal Dictionary<string, List<int>> Patterns
  {
    get
    {
      if (this.m_patterns == null)
        this.m_patterns = new Dictionary<string, List<int>>();
      return this.m_patterns;
    }
    set => this.m_patterns = value;
  }

  public static Dictionary<string, Stream> Dictionaries
  {
    get
    {
      if (Hyphenator.m_dictionaries == null)
        Hyphenator.m_dictionaries = new Dictionary<string, Stream>();
      return Hyphenator.m_dictionaries;
    }
    set => Hyphenator.m_dictionaries = value;
  }

  internal static Dictionary<string, Hyphenator> LoadedHyphenators
  {
    get
    {
      if (Hyphenator.m_loadedHyphenators == null)
        Hyphenator.m_loadedHyphenators = new Dictionary<string, Hyphenator>();
      return Hyphenator.m_loadedHyphenators;
    }
    set => Hyphenator.m_loadedHyphenators = value;
  }

  private void LoadPattern(Stream file)
  {
    Encoding encoding = Encoding.Default;
    StreamReader streamReader = new StreamReader(file, encoding);
    streamReader.ReadLine();
    string str;
    while ((str = streamReader.ReadLine()) != null)
    {
      if (!(str == "") && !str[0].Equals((object) "%") && !str[0].Equals('#'))
      {
        if (str.Contains("COMPOUNDLEFTHYPHENMIN"))
          this.leftMin = int.Parse(str[22].ToString((IFormatProvider) CultureInfo.InvariantCulture));
        else if (str.Contains("COMPOUNDRIGHTHYPHENMIN"))
          this.rightMin = int.Parse(str[23].ToString((IFormatProvider) CultureInfo.InvariantCulture));
        else if (str.Contains("LEFTHYPHENMIN"))
          this.leftMin = int.Parse(str[14].ToString((IFormatProvider) CultureInfo.InvariantCulture));
        else if (str.Contains("RIGHTHYPHENMIN"))
        {
          this.leftMin = int.Parse(str[15].ToString((IFormatProvider) CultureInfo.InvariantCulture));
        }
        else
        {
          List<int> levels = new List<int>(str.Length);
          StringBuilder stringBuilder = new StringBuilder();
          bool flag = true;
          foreach (char c in str)
          {
            if (char.IsDigit(c))
            {
              levels.Add(int.Parse(c.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
              flag = false;
            }
            else
            {
              if (flag)
                levels.Add(0);
              stringBuilder.Append(c);
              flag = true;
            }
          }
          if (flag)
            levels.Add(0);
          if (this.FindMaxValue(levels) != 0 && !this.Patterns.ContainsKey(stringBuilder.ToString()))
            this.Patterns.Add(stringBuilder.ToString(), levels);
        }
      }
    }
  }

  private int FindMaxValue(List<int> levels)
  {
    int maxValue = levels[0];
    foreach (int level in levels)
    {
      if (level > maxValue)
        maxValue = level;
    }
    return maxValue;
  }

  internal string HyphenateText(string text)
  {
    int[] hyphenateMaskFromLevels = Hyphenator.CreateHyphenateMaskFromLevels(this.GetPositions(text));
    return this.HyphenateByMask(text, hyphenateMaskFromLevels);
  }

  private int[] GetPositions(string text)
  {
    string str = new StringBuilder().Append('.').Append(text).Append('.').ToString();
    int[] positions = new int[str.Length + 1];
    for (int index1 = 0; index1 < str.Length - 1; ++index1)
    {
      for (int index2 = index1; index2 <= str.Length; ++index2)
      {
        string key = "";
        for (int index3 = index1; index3 < index2; ++index3)
          key += (string) (object) str[index3];
        if (this.Patterns.ContainsKey(key) && !key.Equals("-") && !key.Equals(",") && !key.Equals("'"))
        {
          List<int> pattern = this.Patterns[key];
          int index4 = 0;
          int count = pattern.Count;
          while (pattern[index4] == 0)
            ++index4;
          while (pattern[count - 1] == 0)
            --count;
          List<int> intList = new List<int>();
          for (int index5 = index4; index5 < count; ++index5)
            intList.Add(pattern[index5]);
          int index6 = 0;
          for (int index7 = index1 + index4; index7 < index1 + index4 + intList.Count; ++index7)
          {
            if (positions[index7] < intList[index6])
              positions[index7] = intList[index6];
            ++index6;
          }
        }
      }
    }
    return positions;
  }

  private static int[] CreateHyphenateMaskFromLevels(int[] levels)
  {
    int length = levels.Length - 2;
    int[] hyphenateMaskFromLevels = new int[length];
    for (int index = 0; index < length; ++index)
      hyphenateMaskFromLevels[index] = index == 0 || levels[index + 1] % 2 == 0 ? 0 : 1;
    return hyphenateMaskFromLevels;
  }

  private string HyphenateByMask(string originalWord, int[] hyphenationMask)
  {
    StringBuilder stringBuilder = new StringBuilder();
    int num = originalWord.Length - this.rightMin;
    for (int index = 0; index < originalWord.Length; ++index)
    {
      if (index >= this.leftMin && index <= num && hyphenationMask[index] > 0)
        stringBuilder.Append("=");
      stringBuilder.Append(originalWord[index]);
    }
    return stringBuilder.ToString();
  }

  internal string GetAlternateForMissedLanguageCode(string languageCode)
  {
    if (this.AddDictionary != null)
    {
      AddDictionaryEventArgs args = new AddDictionaryEventArgs(languageCode, "en-US");
      this.AddDictionary((object) this, args);
      Stream dictionaryStream = args.DictionaryStream;
      if (args.DictionaryStream != null && !Hyphenator.Dictionaries.ContainsKey(languageCode))
        Hyphenator.Dictionaries.Add(languageCode, dictionaryStream);
    }
    return languageCode;
  }
}
