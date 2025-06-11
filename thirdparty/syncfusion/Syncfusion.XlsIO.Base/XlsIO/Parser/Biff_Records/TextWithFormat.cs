// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.TextWithFormat
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public class TextWithFormat : IComparable, ICloneable
{
  private const byte DEF_COMPRESSED_MASK = 1;
  private const byte DEF_RICHTEXT_MASK = 8;
  internal const int DEF_FR_SIZE = 4;
  private const byte DEF_PLAIN_OPTIONS = 1;
  private const byte DEF_RTF_OPTIONS = 9;
  private SortedList<int, int> m_arrFormattingRuns;
  private string m_strValue = string.Empty;
  private int m_iDefaultIndex;
  private TextWithFormat.StringType m_options = TextWithFormat.StringType.Unicode;
  private bool m_bNeedDefragment = true;
  public int RefCount;
  private string m_rtfText;
  private bool m_isPreserved;
  private bool m_isEncoded;

  public TextWithFormat()
  {
  }

  public TextWithFormat(int fontIndex)
    : this()
  {
    this.m_iDefaultIndex = fontIndex;
  }

  public static implicit operator string(TextWithFormat format) => format.Text;

  public static explicit operator TextWithFormat(string value)
  {
    return new TextWithFormat() { Text = value };
  }

  internal string RtfText
  {
    get => this.m_rtfText;
    set => this.m_rtfText = value;
  }

  public string Text
  {
    get => this.m_strValue;
    set => this.m_strValue = value;
  }

  public SortedList<int, int> FormattingRuns
  {
    get
    {
      if (this.m_arrFormattingRuns == null)
        this.m_arrFormattingRuns = new SortedList<int, int>();
      return this.m_arrFormattingRuns;
    }
  }

  internal SortedList<int, int> InnerFormattingRuns => this.m_arrFormattingRuns;

  public int DefaultFontIndex
  {
    get => this.m_iDefaultIndex;
    set => this.m_iDefaultIndex = value;
  }

  public int FormattingRunsCount
  {
    get => this.m_arrFormattingRuns == null ? 0 : this.m_arrFormattingRuns.Count;
  }

  public bool IsPreserved
  {
    get => this.m_isPreserved;
    set => this.m_isPreserved = value;
  }

  internal bool IsEncoded
  {
    get => this.m_isEncoded;
    set => this.m_isEncoded = value;
  }

  public void SetTextFontIndex(int iStartPos, int iEndPos, int iFontIndex)
  {
    this.m_bNeedDefragment = true;
    this.CreateFormattingRuns();
    if (iStartPos < 0 || iStartPos > this.m_strValue.Length)
      throw new ArgumentOutOfRangeException(nameof (iStartPos));
    if (iEndPos < 0 || iEndPos > this.m_strValue.Length)
      throw new ArgumentOutOfRangeException(nameof (iEndPos));
    int num1 = iStartPos <= iEndPos ? this.GetPreviousPosition(iStartPos) : throw new ArgumentException("iStartPos cannot be larger than iEndPos.");
    int previousPosition = this.GetPreviousPosition(iEndPos);
    if (num1 >= 0)
    {
      int arrFormattingRun = this.m_arrFormattingRuns[num1];
    }
    int num2 = previousPosition >= 0 ? this.m_arrFormattingRuns[previousPosition] : this.m_iDefaultIndex;
    this.RemoveAllInsideRange(num1, previousPosition);
    this.m_arrFormattingRuns[iStartPos] = iFontIndex;
    if (iEndPos >= this.m_strValue.Length - 1 || this.m_arrFormattingRuns.ContainsKey(iEndPos + 1))
      return;
    this.m_arrFormattingRuns[iEndPos + 1] = num2;
  }

  public int GetTextFontIndex(int iPos)
  {
    if (this.m_arrFormattingRuns == null)
      return this.m_iDefaultIndex;
    int textFontIndex = this.m_iDefaultIndex;
    int previousPosition = this.GetPreviousPosition(iPos);
    if (previousPosition >= 0)
      textFontIndex = this.m_arrFormattingRuns[previousPosition];
    return textFontIndex;
  }

  public int GetTextFontIndex(int iPos, bool iscopy)
  {
    if (this.m_arrFormattingRuns == null)
      return this.m_iDefaultIndex;
    int textFontIndex = this.m_iDefaultIndex;
    int positionByIndex = this.GetPositionByIndex(iPos);
    if (positionByIndex >= 0)
      textFontIndex = this.m_arrFormattingRuns[positionByIndex];
    return textFontIndex;
  }

  public int GetFontByIndex(int iIndex)
  {
    if (this.m_arrFormattingRuns == null)
      throw new ArgumentOutOfRangeException(nameof (iIndex));
    TextWithFormat.CheckOffset(this.m_arrFormattingRuns.Count, iIndex);
    return this.m_arrFormattingRuns.Values[iIndex];
  }

  public int GetPositionByIndex(int iIndex)
  {
    if (this.m_arrFormattingRuns.Count <= iIndex || iIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (iIndex));
    return this.m_arrFormattingRuns.Keys[iIndex];
  }

  public void SetFontByIndex(int index, int iFontIndex)
  {
    this.CreateFormattingRuns();
    this.m_arrFormattingRuns[this.m_arrFormattingRuns.Keys[index]] = iFontIndex;
  }

  public void ClearFormatting()
  {
    if (this.m_arrFormattingRuns == null)
      return;
    this.m_arrFormattingRuns.Clear();
  }

  public int CompareTo(object obj)
  {
    int num = 0;
    if (obj is TextWithFormat textWithFormat)
    {
      num = string.CompareOrdinal(textWithFormat.m_strValue, this.m_strValue);
      if (num == 0)
      {
        if (this.FormattingRunsCount == 0 && textWithFormat.FormattingRunsCount == 0)
          return 0;
        this.Defragment();
        textWithFormat.Defragment();
        return TextWithFormat.CompareFormattingRuns(this.m_arrFormattingRuns, textWithFormat.m_arrFormattingRuns);
      }
    }
    return num;
  }

  public static int CompareFormattingRuns(SortedList<int, int> fRuns1, SortedList<int, int> fRuns2)
  {
    if (fRuns1 == null && fRuns2 == null)
      return 0;
    if (fRuns1 == null)
      return -1;
    if (fRuns2 == null)
      return 1;
    int num1 = Math.Min(fRuns1.Count, fRuns2.Count);
    IList<int> keys1 = fRuns1.Keys;
    IList<int> keys2 = fRuns2.Keys;
    IList<int> values1 = fRuns1.Values;
    IList<int> values2 = fRuns2.Values;
    for (int index = 0; index < num1; ++index)
    {
      int num2 = keys1[index] - keys2[index];
      if (num2 != 0)
        return num2;
      int num3 = values1[index] - values2[index];
      if (num3 != 0)
        return num3;
    }
    return fRuns1.Count - fRuns2.Count;
  }

  private void CreateFormattingRuns()
  {
    if (this.m_arrFormattingRuns != null)
      return;
    this.m_arrFormattingRuns = new SortedList<int, int>();
  }

  private int GetPreviousPosition(int iPos)
  {
    int val1 = 0;
    int num1 = this.FormattingRunsCount - 1;
    if (num1 < 0)
      return -1;
    IList<int> keys = this.m_arrFormattingRuns.Keys;
    int previousPosition1;
    while (true)
    {
      int num2 = (val1 + num1) / 2;
      previousPosition1 = keys[num2];
      if (val1 < num1 - 1)
      {
        if (previousPosition1 != iPos)
        {
          if (previousPosition1 < iPos)
            val1 = Math.Min(num1, num2);
          else
            num1 = Math.Max(val1, num2);
        }
        else
          goto label_10;
      }
      else
        break;
    }
    int previousPosition2 = keys[num1];
    if (previousPosition2 <= iPos)
      return previousPosition2;
    return previousPosition1 <= iPos ? previousPosition1 : -1;
label_10:
    return previousPosition1;
  }

  internal void RemoveAllInsideRange(int iStartPos, int iEndPos)
  {
    int count = this.m_arrFormattingRuns.Count;
    if (count == 0)
      return;
    IList<int> keys = this.m_arrFormattingRuns.Keys;
    int[] array = new int[count];
    keys.CopyTo(array, 0);
    int index1 = iStartPos == -1 ? 0 : Array.BinarySearch<int>(array, iStartPos);
    int num = iEndPos != -1 ? Array.BinarySearch<int>(array, iEndPos) : iEndPos;
    if (keys[index1] == iStartPos)
      ++index1;
    for (int index2 = index1; index2 <= num; ++index2)
      this.m_arrFormattingRuns.RemoveAt(index1);
  }

  public void Defragment()
  {
    if (this.m_arrFormattingRuns == null)
      this.m_bNeedDefragment = false;
    if (!this.m_bNeedDefragment)
      return;
    int count = this.m_arrFormattingRuns.Count;
    IList<int> values = this.m_arrFormattingRuns.Values;
    int index = 0;
    while (index < count - 1)
    {
      if (values[index] == values[index + 1])
      {
        this.m_arrFormattingRuns.RemoveAt(index + 1);
        --count;
      }
      else
        ++index;
    }
    this.m_bNeedDefragment = false;
  }

  public static void CheckOffset(int len, int iOffset)
  {
    if (iOffset < 0 || iOffset > len)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
  }

  public void CopyFormattingTo(TextWithFormat twin)
  {
    if (twin == null)
      throw new ArgumentNullException(nameof (twin));
    if (this.m_arrFormattingRuns == null || this.m_arrFormattingRuns.Count == 0)
    {
      twin.m_arrFormattingRuns = (SortedList<int, int>) null;
    }
    else
    {
      twin.m_arrFormattingRuns = new SortedList<int, int>();
      IList<int> keys = this.m_arrFormattingRuns.Keys;
      IList<int> values = this.m_arrFormattingRuns.Values;
      int index = 0;
      for (int count = this.m_arrFormattingRuns.Count; index < count; ++index)
        twin.m_arrFormattingRuns.Add(keys[index], values[index]);
    }
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder(this.m_strValue + Environment.NewLine);
    stringBuilder.Append("str[ 0 ] ... - " + (object) this.m_iDefaultIndex);
    if (this.m_arrFormattingRuns != null)
    {
      IList<int> keys = this.m_arrFormattingRuns.Keys;
      int index = 0;
      for (int count = this.m_arrFormattingRuns.Count; index < count; ++index)
      {
        int key = keys[index];
        stringBuilder.AppendFormat("\nstr[ {0} ] ... - {1}", (object) key, (object) this.m_arrFormattingRuns[key]);
      }
    }
    return stringBuilder.ToString();
  }

  public override bool Equals(object obj) => obj is TextWithFormat && this.CompareTo(obj) == 0;

  public override int GetHashCode() => this.m_strValue.GetHashCode();

  public virtual int Parse(byte[] data, int iOffset)
  {
    int len = data != null ? data.Length : throw new ArgumentNullException(nameof (data));
    TextWithFormat.CheckOffset(len, iOffset);
    if (iOffset < 0 || iOffset > data.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    int startIndex = iOffset;
    TextWithFormat.CheckOffset(len, startIndex + 2);
    ushort uint16 = BitConverter.ToUInt16(data, startIndex);
    int index = startIndex + 2;
    TextWithFormat.CheckOffset(len, index + 1);
    this.m_options = (TextWithFormat.StringType) data[index];
    int iOffset1 = index + 1;
    bool bIsUnicode = (this.m_options & TextWithFormat.StringType.Unicode) != TextWithFormat.StringType.NonUnicode;
    bool flag1 = (this.m_options & TextWithFormat.StringType.FarEast) != TextWithFormat.StringType.NonUnicode;
    bool flag2 = (this.m_options & TextWithFormat.StringType.RichText) != TextWithFormat.StringType.NonUnicode;
    int iFRCount = 0;
    int iFarEastDataLen = 0;
    if (flag2)
    {
      TextWithFormat.CheckOffset(len, iOffset1 + 2);
      iFRCount = (int) BitConverter.ToUInt16(data, iOffset1);
      iOffset1 += 2;
    }
    if (flag1)
    {
      TextWithFormat.CheckOffset(len, iOffset1 + 4);
      iFarEastDataLen = BitConverter.ToInt32(data, iOffset1);
      iOffset1 += 4;
    }
    this.Text = this.GetText(data, uint16, bIsUnicode, ref iOffset1);
    if (iFRCount > 0)
      this.ParseFormattingRuns(data, iOffset1, iFRCount);
    if (iFarEastDataLen > 0)
      this.ParseFarEastData(data, iOffset1, iFarEastDataLen);
    return iOffset1 - iOffset;
  }

  public int GetTextSize() => Encoding.Unicode.GetByteCount(this.m_strValue) + 3;

  public int GetFormattingSize()
  {
    this.Defragment();
    int formattingRunsCount = this.FormattingRunsCount;
    if (formattingRunsCount > 0)
      this.m_options |= TextWithFormat.StringType.RichText;
    return formattingRunsCount * 4;
  }

  public byte[] SerializeFormatting()
  {
    byte[] numArray = this.SerializeFormattingRuns();
    if (numArray != null && numArray.Length > 0)
      this.m_options |= TextWithFormat.StringType.RichText;
    return numArray;
  }

  public int SerializeFormatting(byte[] arrBuffer, int iOffset, bool bDefragment)
  {
    int num = this.SerializeFormattingRuns(arrBuffer, iOffset, bDefragment);
    if (num > 0)
      this.m_options |= TextWithFormat.StringType.RichText;
    return num;
  }

  public TextWithFormat.StringType GetOptions() => this.m_options;

  private string GetText(byte[] data, ushort usChartCount, bool bIsUnicode, ref int iOffset)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    int count;
    string text;
    if (bIsUnicode)
    {
      count = (int) usChartCount * 2;
      TextWithFormat.CheckOffset(data.Length, iOffset + count);
      text = Encoding.Unicode.GetString(data, iOffset, count);
    }
    else
    {
      count = (int) usChartCount;
      TextWithFormat.CheckOffset(data.Length, iOffset + count);
      text = BiffRecordRaw.LatinEncoding.GetString(data, iOffset, count);
    }
    iOffset += count;
    return text;
  }

  private void ParseFormattingRuns(byte[] data, int iOffset, int iFRCount)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (iOffset >= data.Length && iFRCount > 0)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    this.CreateFormattingRuns();
    int length = data.Length;
    int num = 0;
    while (num < iFRCount)
    {
      TextWithFormat.CheckOffset(length, iOffset + 4);
      this.m_arrFormattingRuns[(int) BitConverter.ToUInt16(data, iOffset)] = (int) BitConverter.ToUInt16(data, iOffset + 2);
      ++num;
      iOffset += 4;
    }
  }

  internal void ParseFormattingRuns(byte[] data)
  {
    if (data == null || data.Length == 0)
      return;
    this.ParseFormattingRuns(data, 0, data.Length / 4);
  }

  private void ParseFarEastData(byte[] data, int iOffset, int iFarEastDataLen)
  {
  }

  private byte[] SerializeFormattingRuns()
  {
    this.Defragment();
    int formattingRunsCount = this.FormattingRunsCount;
    if (formattingRunsCount == 0)
      return (byte[]) null;
    byte[] arrDestination = new byte[formattingRunsCount * 4];
    this.SerializeFormattingRuns(arrDestination, 0, true);
    return arrDestination;
  }

  private int SerializeFormattingRuns(byte[] arrDestination, int iOffset, bool bDefragment)
  {
    if (bDefragment)
      this.Defragment();
    int formattingRunsCount = this.FormattingRunsCount;
    if (formattingRunsCount == 0)
      return 0;
    if (arrDestination == null)
      throw new ArgumentNullException(nameof (arrDestination));
    int num = formattingRunsCount * 4;
    if (iOffset < 0 || iOffset + num > arrDestination.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    IList<int> keys = this.m_arrFormattingRuns.Keys;
    IList<int> values = this.m_arrFormattingRuns.Values;
    int index = 0;
    while (index < formattingRunsCount)
    {
      byte[] bytes1 = BitConverter.GetBytes((ushort) keys[index]);
      arrDestination[iOffset] = bytes1[0];
      arrDestination[iOffset + 1] = bytes1[1];
      byte[] bytes2 = BitConverter.GetBytes((ushort) values[index]);
      arrDestination[iOffset + 2] = bytes2[0];
      arrDestination[iOffset + 3] = bytes2[1];
      ++index;
      iOffset += 4;
    }
    return num;
  }

  public object Clone() => (object) this.TypedClone();

  public TextWithFormat TypedClone()
  {
    TextWithFormat twin = this.MemberwiseClone() as TextWithFormat;
    if (this.m_arrFormattingRuns != null)
      this.CopyFormattingTo(twin);
    return twin;
  }

  public TextWithFormat Clone(Dictionary<int, int> dicFontIndexes)
  {
    TextWithFormat textWithFormat = this.TypedClone();
    if (dicFontIndexes != null)
      textWithFormat.UpdateFontIndexes(dicFontIndexes);
    return textWithFormat;
  }

  private void UpdateFontIndexes(Dictionary<int, int> dicFontIndexes)
  {
    if (dicFontIndexes == null)
      throw new ArgumentNullException("arrFontIndexes");
    if (this.FormattingRunsCount <= 0)
      return;
    IList<int> values = this.m_arrFormattingRuns.Values;
    IList<int> keys = this.m_arrFormattingRuns.Keys;
    int index = 0;
    for (int formattingRunsCount = this.FormattingRunsCount; index < formattingRunsCount; ++index)
      this.m_arrFormattingRuns[keys[index]] = FontImpl.UpdateFontIndexes(values[index], dicFontIndexes, ExcelParseOptions.Default);
  }

  internal void ReplaceFont(int oldFontIndex, int newFontIndex)
  {
    SortedList<int, int> sortedList = new SortedList<int, int>();
    foreach (KeyValuePair<int, int> arrFormattingRun in this.m_arrFormattingRuns)
    {
      int num = arrFormattingRun.Value;
      if (num == oldFontIndex)
        num = newFontIndex;
      sortedList.Add(arrFormattingRun.Key, num);
    }
    this.m_arrFormattingRuns = sortedList;
  }

  internal void RemoveAtStart(int length)
  {
    int previousPosition = this.GetPreviousPosition(length);
    if (previousPosition >= 0)
    {
      int arrFormattingRun1 = this.m_arrFormattingRuns[previousPosition];
      for (int index = this.m_arrFormattingRuns.IndexOfKey(previousPosition); index >= 0; --index)
        this.m_arrFormattingRuns.RemoveAt(index);
      this.m_arrFormattingRuns[length] = arrFormattingRun1;
      SortedList<int, int> sortedList = new SortedList<int, int>();
      foreach (KeyValuePair<int, int> arrFormattingRun2 in this.m_arrFormattingRuns)
        sortedList[arrFormattingRun2.Key - length] = arrFormattingRun2.Value;
      this.m_arrFormattingRuns = sortedList;
    }
    this.Text = this.Text.Substring(length);
  }

  internal void RemoveAtEnd(int length)
  {
    int length1 = this.Text.Length - length;
    int previousPosition = this.GetPreviousPosition(length1 - 1);
    if (previousPosition >= 0)
    {
      int arrFormattingRun = this.m_arrFormattingRuns[previousPosition];
      int num = this.m_arrFormattingRuns.IndexOfKey(previousPosition) + 1;
      for (int index = this.m_arrFormattingRuns.Count - 1; index >= num; --index)
        this.m_arrFormattingRuns.RemoveAt(index);
    }
    else
      this.ClearFormatting();
    this.Text = this.Text.Substring(0, length1);
  }

  [Flags]
  public enum StringType : byte
  {
    NonUnicode = 0,
    Unicode = 1,
    FarEast = 4,
    RichText = 8,
  }
}
