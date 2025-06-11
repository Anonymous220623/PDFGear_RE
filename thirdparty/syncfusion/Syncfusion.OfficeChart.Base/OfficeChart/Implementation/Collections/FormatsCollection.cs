// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.FormatsCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.FormatParser;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class FormatsCollection(IApplication application, object parent) : CommonObject(application, parent)
{
  public const string DecimalSeparator = ".";
  public const string ThousandSeparator = ",";
  public const string Percentage = "%";
  public const string Fraction = "/";
  public const string Date = "date";
  public const string Time = ":";
  public const string Exponent = "E";
  public const string Minus = "-";
  public const string Currency = "$";
  public const string DEFAULT_EXPONENTAIL = "E+";
  internal const int DEF_FIRST_CUSTOM_INDEX = 163;
  private const int CountryJapan = 81;
  private string[] AdditionalDateFormats = new string[1]
  {
    "[$-409]m/d/yy h:mm AM/PM;@"
  };
  internal string[] DEF_FORMAT_STRING = new string[36]
  {
    "General",
    "0",
    "0.00",
    "#,##0",
    "#,##0.00",
    "\"$\"#,##0_);\\( \"$\"#,##0\\ )",
    "\"$\"#,##0_);[Red]\\( \"$\"#,##0\\ )",
    "\"$\"#,##0.00_);\\( \"$\"#,##0.00\\ )",
    "\"$\"#,##0.00_);[Red]\\( \"$\"#,##0.00\\ )",
    "0%",
    "0.00%",
    "0.00E+00",
    "# ?/?",
    "# ??/??",
    "m/d/yyyy",
    "d\\-mmm\\-yy",
    "d\\-mmm",
    "mmm\\-yy",
    "h:mm\\ AM/PM",
    "h:mm:ss\\ AM/PM",
    "h:mm",
    "h:mm:ss",
    "m/d/yy\\ h:mm",
    "_( #,##0_);\\( #,##0\\ )",
    "_( #,##0_);[Red]\\( #,##0\\ )",
    "_( #,##0.00_);\\( #,##0.00\\ )",
    "_( #,##0.00_);[Red]\\( #,##0.00\\ )",
    "_(* #,##0_);_(* \\( #,##0\\ );_(* \"-\"_);_( @_ )",
    "_(\"$\"* #,##0_);_(\"$\"* \\( #,##0\\ );_(\"$\"* \"-\"_);_( @_ )",
    "_(* #,##0.00_);_(* \\(#,##0.00\\);_(* \"-\"??_);_(@_)",
    "_(\"$\"* #,##0.00_);_(\"$\"* \\( #,##0.00\\ );_(\"$\"* \"-\"??_);_( @_ )",
    "mm:ss",
    "[h]:mm:ss",
    "mm:ss.0",
    "##0.0E+0",
    "@"
  };
  private string[] DEF_CURRENCY_FORMAT_STRING = new string[10]
  {
    "\"$\"#,##0.00",
    "#,##0.00\\ [$֏-42B]",
    "[$₸-43F]#,##0.00",
    "[$£-809]#,##0.00",
    "[$¥-411]#,##0.00",
    "#,##0.00\\ [$₽-419]",
    "#,##0.00\\ [$₭-454]",
    "[$₦-466]\\ #,##0.00",
    "#,##0.00\\ [$€-484]",
    "[$₹-4009]\\ #,##0.00"
  };
  private string[] DEF_CURRENCY_SYMBOL = new string[10]
  {
    "$",
    "֏",
    "₸",
    "£",
    "¥",
    "₱",
    "₭",
    "₦",
    "€",
    "₹"
  };
  private TypedSortedListEx<int, FormatImpl> m_rawFormats = new TypedSortedListEx<int, FormatImpl>();
  private Dictionary<string, FormatImpl> m_hashFormatStrings = new Dictionary<string, FormatImpl>();
  private FormatParserImpl m_parser;
  private Dictionary<string, int[]> m_formatIndexes = new Dictionary<string, int[]>();
  private bool m_hasNumFormats;
  private Dictionary<string, string> m_currencyFormatStrings;

  [CLSCompliant(false)]
  public FormatImpl this[int iIndex] => this.m_rawFormats[iIndex];

  internal bool HasNumberFormats
  {
    get => this.m_hasNumFormats;
    set => this.m_hasNumFormats = value;
  }

  [CLSCompliant(false)]
  public FormatImpl this[string strFormat] => this.m_hashFormatStrings[strFormat];

  public FormatParserImpl Parser
  {
    get
    {
      if (this.m_parser == null)
        this.m_parser = new FormatParserImpl(this.Application, (object) this);
      return this.m_parser;
    }
  }

  internal Dictionary<string, string> CurrencyFormatStrings
  {
    get
    {
      if (this.m_currencyFormatStrings == null)
      {
        this.m_currencyFormatStrings = new Dictionary<string, string>();
        int index = 0;
        for (int length = this.DEF_CURRENCY_SYMBOL.Length; index < length; ++index)
          this.m_currencyFormatStrings[this.DEF_CURRENCY_SYMBOL[index]] = this.DEF_CURRENCY_FORMAT_STRING[index];
      }
      return this.m_currencyFormatStrings;
    }
  }

  public int Parse(IList data, int iPos)
  {
    int num = data != null ? data.Count : throw new ArgumentNullException(nameof (data));
    if (iPos < 0 || iPos >= num)
      throw new ArgumentOutOfRangeException(nameof (iPos));
    throw new NotImplementedException();
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    records.AddRange((ICollection) this.GetUsedFormats(OfficeVersion.Excel97to2003));
  }

  [CLSCompliant(false)]
  public void Add(FormatRecord format)
  {
    FormatImpl format1 = format != null ? new FormatImpl(this.Application, (object) this, format) : throw new ArgumentNullException(nameof (format));
    int index = format.Index;
    this.Register(format1);
  }

  internal void Add(int formatId, string formatString)
  {
    if (formatString == null)
      throw new ArgumentOutOfRangeException(nameof (formatString));
    if (formatId < 0)
      throw new ArgumentOutOfRangeException(nameof (formatId));
    if (formatString.Length == 0)
      return;
    this.Register(new FormatImpl(this.Application, (object) this, formatId, formatString));
  }

  private void Register(FormatImpl format)
  {
    this.m_rawFormats[format.Index] = format != null ? format : throw new ArgumentNullException(nameof (format));
    this.m_hashFormatStrings[format.FormatString] = format;
  }

  public FormatsCollection Clone(object parent)
  {
    FormatsCollection parent1 = (FormatsCollection) this.MemberwiseClone();
    parent1.SetParent(parent);
    parent1.m_parser = (FormatParserImpl) null;
    parent1.m_rawFormats = new TypedSortedListEx<int, FormatImpl>();
    parent1.m_hashFormatStrings = new Dictionary<string, FormatImpl>();
    foreach (KeyValuePair<int, FormatImpl> rawFormat in this.m_rawFormats)
    {
      FormatImpl formatImpl = (FormatImpl) rawFormat.Value.Clone((object) parent1);
      parent1.m_rawFormats.Add(rawFormat.Key, formatImpl);
      parent1.m_hashFormatStrings.Add(formatImpl.FormatString, formatImpl);
    }
    return parent1;
  }

  public int CreateFormat(string formatString)
  {
    switch (formatString)
    {
      case null:
        throw new ArgumentNullException(nameof (formatString));
      case "":
        throw new ArgumentException("formatString - string cannot be empty");
      default:
        if (formatString.Contains("E+".ToLower()))
          formatString = formatString.Replace("E+".ToLower(), "E+");
        if (formatString.Contains("E+".ToLower()))
          formatString = formatString.Replace("E+".ToLower(), "E+");
        formatString = this.GetCustomizedString(formatString);
        if (this.ContainsFormat(formatString))
          return this.m_hashFormatStrings[formatString].Index;
        int num = this.m_rawFormats.GetKey(this.m_rawFormats.Count - 1);
        if (num < 163)
          num = 163;
        int format = num + 1;
        FormatRecord record = (FormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Format);
        record.FormatString = formatString;
        record.Index = format;
        this.Add(record);
        return format;
    }
  }

  private string GetCustomizedString(string formatString)
  {
    string lower = formatString.ToLower();
    if ((!lower.Contains("d") || !lower.Contains("m") || !lower.Contains("y")) && !lower.Contains("h"))
      return formatString;
    string customizedString = lower.Replace("am", "AM").Replace("pm", "PM");
    if (!customizedString.Contains("\\"))
    {
      string[] strArray = new string[4]
      {
        ",",
        " ",
        ".",
        "-"
      };
      if (customizedString.Contains("h") || customizedString.Contains("s"))
        return formatString;
      foreach (string oldValue in strArray)
      {
        if (customizedString.Contains(oldValue))
        {
          string newValue = oldValue.PadLeft(2, '\\');
          customizedString = customizedString.Replace(oldValue, newValue);
        }
      }
    }
    return customizedString;
  }

  public bool ContainsFormat(string formatString)
  {
    return formatString != null && formatString.Length != 0 && this.m_hashFormatStrings.ContainsKey(formatString);
  }

  public int FindOrCreateFormat(string formatString)
  {
    FormatImpl formatImpl;
    return CultureInfo.CurrentCulture.Name != "en-US" && this.m_hashFormatStrings.ContainsKey(formatString) || !this.m_hashFormatStrings.TryGetValue(formatString, out formatImpl) ? this.CreateFormat(formatString) : formatImpl.Index;
  }

  public void InsertDefaultFormats()
  {
    FormatRecord record = (FormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Format);
    int num = 0;
    int index = 0;
    for (int length = this.DEF_FORMAT_STRING.Length; index < length; ++index)
    {
      record.Index = num;
      record.FormatString = this.DEF_FORMAT_STRING[index];
      if (!this.m_rawFormats.Contains(record.Index))
        this.Add((FormatRecord) record.Clone());
      if (num == 22)
        num = 36;
      ++num;
    }
  }

  public List<FormatRecord> GetUsedFormats(OfficeVersion version)
  {
    List<FormatRecord> usedFormats = new List<FormatRecord>();
    if (version == OfficeVersion.Excel97to2003)
    {
      usedFormats.Add(this[5].Record);
      usedFormats.Add(this[6].Record);
      usedFormats.Add(this[7].Record);
      usedFormats.Add(this[8].Record);
      usedFormats.Add(this[42].Record);
      usedFormats.Add(this[41].Record);
      usedFormats.Add(this[44].Record);
      usedFormats.Add(this[43].Record);
    }
    else
    {
      if (this[5].FormatString != this.DEF_FORMAT_STRING[5])
        usedFormats.Add(this[5].Record);
      if (this[6].FormatString != this.DEF_FORMAT_STRING[6])
        usedFormats.Add(this[6].Record);
      if (this[7].FormatString != this.DEF_FORMAT_STRING[7])
        usedFormats.Add(this[7].Record);
      if (this[8].FormatString != this.DEF_FORMAT_STRING[8])
        usedFormats.Add(this[8].Record);
      if (this[42].FormatString != this.DEF_FORMAT_STRING[28])
        usedFormats.Add(this[42].Record);
      if (this[41].FormatString != this.DEF_FORMAT_STRING[27])
        usedFormats.Add(this[41].Record);
      if (this[44].FormatString != this.DEF_FORMAT_STRING[30])
        usedFormats.Add(this[44].Record);
      if (this[43].FormatString != this.DEF_FORMAT_STRING[29])
        usedFormats.Add(this[43].Record);
    }
    int num = this.m_rawFormats.IndexOfKey(49);
    int count = this.m_rawFormats.Count;
    if (num >= 0 && num < count - 1)
    {
      for (int index = num + 1; index < count; ++index)
      {
        FormatImpl byIndex = this.m_rawFormats.GetByIndex(index);
        if (byIndex.Index >= 163 || this.HasNumberFormats)
          usedFormats.Add(byIndex.Record);
      }
    }
    return usedFormats;
  }

  public Dictionary<int, int> Merge(FormatsCollection source)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    TypedSortedListEx<int, FormatImpl> rawFormats = source.m_rawFormats;
    int index1 = 0;
    for (int count = source.Count; index1 < count; ++index1)
    {
      FormatImpl byIndex = rawFormats.GetByIndex(index1);
      int index2 = byIndex.Index;
      int num = this.AddCopy(byIndex);
      dictionary.Add(index2, num);
    }
    return dictionary;
  }

  public Dictionary<int, int> AddRange(IDictionary dicIndexes, FormatsCollection source)
  {
    if (dicIndexes == null)
      throw new ArgumentNullException(nameof (dicIndexes));
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    foreach (int key in (IEnumerable) dicIndexes.Keys)
    {
      int num = this.AddCopy(source[key]);
      dictionary.Add(key, num);
    }
    return dictionary;
  }

  private int AddCopy(FormatImpl format)
  {
    return format != null ? this.AddCopy(format.Record) : throw new ArgumentNullException(nameof (format));
  }

  private int AddCopy(FormatRecord format)
  {
    return format != null ? this.CreateFormat(format.FormatString) : throw new ArgumentNullException(nameof (format));
  }

  private void AddJapaneseFormats()
  {
    this.Add(27, "[$-411]ge.m.d");
    this.Add(28, "[$-411]ggge\\\"年\\\"m\\\"月\\\"d\\\"日\\\"");
    this.Add(29, "[$-411]ggge\\\"年\\\"m\\\"月\\\"d\\\"日\\\"");
    this.Add(30, "m/d/yy");
    this.Add(31 /*0x1F*/, "yyyy\\\"年\\\"m\\\"月\\\"d\\\"日\\\"");
    this.Add(32 /*0x20*/, "h\\\"時\\\"mm\\\"分\\\"");
    this.Add(33, "h\\\"時\\\"mm\\\"分\\\"ss\\\"秒\\\"");
    this.Add(34, "yyyy\\\"年\\\"m\\\"月\\\"");
    this.Add(35, "m\\\"月\\\"d\\\"日\\\"");
    this.Add(36, "[$-411]ge.m.d");
    this.Add(50, "[$-411]ge.m.d");
    this.Add(51, "[$-411]ggge\\\"年\\\"m\\\"月\\\"d\\\"日\\\"");
    this.Add(52, "yyyy\\\"年\\\"m\\\"月\\\"");
    this.Add(53, "m\\\"月\\\"d\\\"日\\\"");
    this.Add(54, "[$-411]ggge\\\"年\\\"m\\\"月\\\"d\\\"日\\\"");
    this.Add(55, "yyyy\\\"年\\\"m\\\"月\\\"");
    this.Add(56, "m\" 月\"d\" 日\"");
    this.Add(57, "[$-411]ge.m.d yyyy\\\"年\\\"");
    this.Add(58, "[$-411]ggge\\\"年\\\"m\\\"月\\\"d\\\"日\\\"");
  }

  internal void AddDefaultFormats(int country)
  {
    if (country != 81)
      return;
    this.AddJapaneseFormats();
  }

  internal void FillFormatIndexes()
  {
    if (this.m_formatIndexes.Count > 0)
      return;
    List<int> intList = new List<int>();
    intList.AddRange((IEnumerable<int>) new int[5]
    {
      14,
      15,
      16 /*0x10*/,
      17,
      22
    });
    foreach (string additionalDateFormat in this.AdditionalDateFormats)
      intList.Add(this.CreateFormat(additionalDateFormat));
    this.m_formatIndexes.Add("%", new int[2]{ 9, 10 });
    this.m_formatIndexes.Add(",", new int[6]
    {
      3,
      4,
      5,
      6,
      7,
      8
    });
    this.m_formatIndexes.Add(".", new int[2]{ 2, 7 });
    this.m_formatIndexes.Add("date", intList.ToArray());
    this.m_formatIndexes.Add(":", new int[4]
    {
      18,
      19,
      20,
      21
    });
    this.m_formatIndexes.Add("/", new int[2]{ 12, 13 });
    this.m_formatIndexes.Add("E", new int[1]{ 11 });
  }

  internal string GetDateFormat(string strValue)
  {
    string[] strArray1 = new string[4]{ "-", "/", ",", " " };
    string str1 = (string) null;
    bool flag = false;
    int num = strValue.IndexOf(":"[0]);
    if (num != -1)
    {
      flag = true;
      if (num <= 2)
        return this.GetTimeFormat(strValue);
    }
    foreach (string str2 in strArray1)
    {
      if (strValue.IndexOf(str2) != -1)
      {
        str1 = str2;
        break;
      }
    }
    int[] formatIndex = this.m_formatIndexes["date"];
    string str3 = strValue;
    if (flag)
    {
      int startIndex1 = strValue.IndexOf("AM");
      if (startIndex1 != -1)
      {
        string str4 = strValue.Remove(startIndex1, 2);
        str3 = str4.Remove(str4.Length - 1);
      }
      int startIndex2 = strValue.IndexOf("PM");
      if (startIndex2 != -1)
      {
        string str5 = strValue.Remove(startIndex2, 2);
        str3 = str5.Remove(str5.Length - 1);
      }
    }
    string[] strArray2 = str3.Split(str1[0]);
    string dateFormat;
    switch (strArray2.Length)
    {
      case 2:
        dateFormat = !char.IsLetter(strArray2[1], 1) ? this.m_rawFormats[formatIndex[3]].FormatString : this.m_rawFormats[formatIndex[2]].FormatString;
        break;
      case 3:
        dateFormat = !flag ? (strArray2[1].Length <= 1 || !char.IsLetter(strArray2[1], 1) ? this.m_rawFormats[formatIndex[0]].FormatString : this.m_rawFormats[formatIndex[1]].FormatString) : (!this.IsStandardTimeFormat(strValue) ? this.m_rawFormats[formatIndex[4]].FormatString : this.m_rawFormats[formatIndex[5]].FormatString);
        break;
      default:
        dateFormat = this.GetTimeFormat(strValue);
        break;
    }
    return dateFormat;
  }

  private string GetTimeFormat(string strValue)
  {
    bool flag1 = this.IsStandardTimeFormat(strValue);
    bool flag2 = this.HasSecond(strValue);
    int[] formatIndex = this.m_formatIndexes[":"];
    string timeFormat = this.m_rawFormats[formatIndex[0]].FormatString;
    foreach (int key in formatIndex)
    {
      string formatString = this.m_rawFormats[key].FormatString;
      if (this.IsStandardTimeFormat(formatString) == flag1 && this.HasSecond(formatString) == flag2)
        timeFormat = formatString;
    }
    return timeFormat;
  }

  private bool HasSecond(string strValue) => strValue.Split(":"[0]).Length > 2;

  private bool IsStandardTimeFormat(string strValue)
  {
    return strValue.IndexOf("AM") != -1 || strValue.IndexOf("PM") != -1;
  }

  internal string GetNumberFormat(string strValue) => this.ParseNumberFormat(strValue) ?? "0.00";

  private string ParseNumberFormat(string strValue)
  {
    bool flag1 = strValue.IndexOf("$") != -1;
    bool flag2 = strValue.IndexOf("."[0]) != -1;
    bool flag3 = strValue.IndexOf("%"[0]) != -1;
    bool flag4 = strValue.IndexOf(","[0]) != -1;
    bool flag5 = strValue.IndexOf("E"[0]) != -1;
    bool flag6 = strValue.IndexOf("/"[0]) != -1;
    string numberFormat = (string) null;
    if (flag4)
    {
      foreach (int key in this.m_formatIndexes[","])
      {
        FormatImpl rawFormat = this.m_rawFormats[key];
        if (rawFormat.DecimalPlaces > 1 && flag2 && (!flag1 || rawFormat.FormatString.IndexOf("$") != -1))
        {
          numberFormat = rawFormat.FormatString;
          break;
        }
      }
    }
    else if (flag6)
    {
      int[] formatIndex = this.m_formatIndexes["/"];
      numberFormat = (strValue.Split("/"[0])[0].Length <= 1 ? this.m_rawFormats[formatIndex[0]] : this.m_rawFormats[formatIndex[1]]).FormatString;
    }
    else if (flag3)
    {
      int[] formatIndex = this.m_formatIndexes["%"];
      numberFormat = !flag2 ? this.m_rawFormats[formatIndex[0]].FormatString : this.m_rawFormats[formatIndex[1]].FormatString;
    }
    else if (flag2)
    {
      int[] formatIndex = this.m_formatIndexes["."];
      numberFormat = !flag1 ? this.m_rawFormats[formatIndex[0]].FormatString : "$" + this.m_rawFormats[this.m_formatIndexes[","][1]].FormatString;
    }
    else
      numberFormat = !flag5 ? (!flag1 ? this.m_rawFormats[1].FormatString : "$" + this.m_rawFormats[this.m_formatIndexes[","][1]].FormatString) : this.m_rawFormats[this.m_formatIndexes["E"][0]].FormatString;
    return numberFormat;
  }

  public bool IsReadOnly => false;

  public IEnumerator<KeyValuePair<int, FormatImpl>> GetEnumerator()
  {
    return this.m_rawFormats.GetEnumerator();
  }

  public void Remove(int key)
  {
    FormatImpl rawFormat = this.m_rawFormats[key];
    if (rawFormat == null)
      return;
    this.m_rawFormats.Remove(key);
    this.m_hashFormatStrings.Remove(rawFormat.FormatString);
  }

  public bool Contains(int key) => this.m_rawFormats.Contains(key);

  public void Clear()
  {
    foreach (KeyValuePair<string, FormatImpl> hashFormatString in this.m_hashFormatStrings)
      hashFormatString.Value.Clear();
    this.m_rawFormats.Clear();
    this.m_hashFormatStrings.Clear();
    this.m_rawFormats = (TypedSortedListEx<int, FormatImpl>) null;
    this.m_hashFormatStrings = (Dictionary<string, FormatImpl>) null;
    if (this.m_parser != null)
    {
      this.m_parser.Clear();
      this.m_parser.Dispose();
    }
    this.Dispose();
  }

  public ICollection Values => throw new NotImplementedException();

  public ICollection Keys => throw new NotImplementedException();

  public bool IsFixedSize => this.m_rawFormats.IsFixedSize;

  public bool IsSynchronized => this.m_rawFormats.IsSynchronized;

  public int Count => this.m_rawFormats.Count;

  public void CopyTo(Array array, int index) => throw new NotImplementedException();

  public object SyncRoot => throw new NotImplementedException();
}
