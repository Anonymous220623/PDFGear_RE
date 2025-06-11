// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.FormatTokenBase
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public abstract class FormatTokenBase : ICloneable
{
  protected const RegexOptions DEF_OPTIONS = RegexOptions.Compiled;
  protected string m_strFormat;

  public abstract int TryParse(string strFormat, int iIndex);

  protected int TryParseRegex(Regex regex, string strFormat, int iIndex)
  {
    return this.TryParseRegex(regex, strFormat, iIndex, out Match _);
  }

  protected int TryParseRegex(Regex regex, string strFormat, int iIndex, out Match m)
  {
    if (regex == null)
      throw new ArgumentNullException(nameof (regex));
    int num = strFormat != null ? strFormat.Length : throw new ArgumentNullException(nameof (strFormat));
    if (num == 0)
      throw new ArgumentException("strFormat - string cannot be empty");
    if (iIndex < 0 || iIndex > num)
      throw new ArgumentOutOfRangeException(nameof (iIndex), "Value cannot be less than 0 or greater than Format Length");
    m = regex.Match(strFormat, iIndex);
    if (m.Success && m.Index == iIndex)
    {
      this.Format = m.Value;
      iIndex += this.m_strFormat.Length;
    }
    return iIndex;
  }

  public virtual string ApplyFormat(ref double value)
  {
    return this.ApplyFormat(ref value, false, (CultureInfo) null, (FormatSection) null);
  }

  public abstract string ApplyFormat(string value, bool bShowHiddenSymbols);

  public virtual string ApplyFormat(string value) => this.ApplyFormat(value, false);

  public object Clone() => this.MemberwiseClone();

  public abstract string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section);

  public string Format
  {
    get => this.m_strFormat;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (value));
        case "":
          throw new ArgumentException("value - string cannot be empty.");
        default:
          if (!(this.m_strFormat != value))
            break;
          this.m_strFormat = value;
          this.OnFormatChange();
          break;
      }
    }
  }

  public abstract TokenType TokenType { get; }

  public int FindString(string[] arrStrings, string strFormat, int iIndex, bool bIgnoreCase)
  {
    int num = strFormat != null ? strFormat.Length : throw new ArgumentNullException(nameof (strFormat));
    if (num == 0)
      throw new ArgumentException("strFormat - string cannot be empty.");
    if (iIndex < 0 || iIndex > num - 1)
      throw new ArgumentOutOfRangeException(nameof (iIndex), "Value cannot be less than 0 and greater than than format length - 1.");
    int index = 0;
    for (int length = arrStrings.Length; index < length; ++index)
    {
      string arrString = arrStrings[index];
      StringComparison comparisonType = bIgnoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
      if (string.Compare(strFormat, iIndex, arrString, 0, arrString.Length, comparisonType) == 0)
        return index;
    }
    return -1;
  }

  protected virtual void OnFormatChange()
  {
  }
}
