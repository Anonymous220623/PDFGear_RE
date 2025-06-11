// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.DigitToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public abstract class DigitToken : SingleCharToken
{
  private const int DEF_MAX_NUMBER_DIGIT = 15;
  private bool m_bLastDigit;
  private bool m_bCenterDigit;
  private double m_originalValue;

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    int iDigit = this.GetDigit(ref value);
    this.GetIndexOfZero(this.OriginalValue, iDigit);
    if (iDigit == (int) byte.MaxValue)
      iDigit = 0;
    return this.GetDigitString(value, iDigit, bShowHiddenSymbols);
  }

  private void GetIndexOfZero(double value, int iDigit)
  {
    if (iDigit != 0)
      return;
    int startIndex = value.ToString().IndexOf(iDigit.ToString());
    int lastIndex = value.ToString().LastIndexOf(iDigit.ToString());
    if (startIndex == value.ToString().Length - 1 || startIndex == 0 || startIndex == -1 || !this.CheckIsZeroes(startIndex, lastIndex, value.ToString()))
      return;
    this.m_bCenterDigit = true;
  }

  private bool CheckIsZeroes(int startIndex, int lastIndex, string p)
  {
    bool flag = true;
    if (startIndex == lastIndex)
      return flag;
    for (int index = startIndex; index <= lastIndex; ++index)
    {
      if (p[index] == '0')
      {
        flag = false;
      }
      else
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => value;

  public bool IsLastDigit
  {
    get => this.m_bLastDigit;
    set => this.m_bLastDigit = value;
  }

  public bool IsCenterDigit
  {
    get => this.m_bCenterDigit;
    internal set => this.m_bCenterDigit = value;
  }

  internal double OriginalValue
  {
    get => this.m_originalValue;
    set => this.m_originalValue = value;
  }

  protected internal int GetDigit(ref double value)
  {
    value = FormatSection.Round(value);
    int exponentialString = this.GetExponentialString(value);
    value /= 10.0;
    if (exponentialString == 0 && value == 0.0)
      return (int) byte.MaxValue;
    int num = Math.Sign(value);
    value = value > 0.0 ? Math.Floor(value) : Math.Ceiling(value);
    value += (double) num * 0.1;
    return exponentialString;
  }

  internal int GetExponentialString(double value)
  {
    string str = value.ToString();
    if (str.Contains("E"))
    {
      int num = str.IndexOf("E");
      if (Convert.ToInt16(str.Substring(num + 2, str.Length - (num + 2))) >= (short) 15)
        return 0;
    }
    return (int) (value % 10.0);
  }

  protected internal virtual string GetDigitString(
    double value,
    int iDigit,
    bool bShowHiddenSymbols)
  {
    iDigit = Math.Abs(iDigit);
    return iDigit <= 9 ? iDigit.ToString() : throw new ArgumentOutOfRangeException(nameof (iDigit), "Value cannot be less than -9 and greater than than 9.");
  }
}
