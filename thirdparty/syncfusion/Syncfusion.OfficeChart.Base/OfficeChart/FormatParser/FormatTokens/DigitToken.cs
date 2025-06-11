// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.DigitToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal abstract class DigitToken : SingleCharToken
{
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
    int digit = (int) (value % 10.0);
    value /= 10.0;
    if (digit == 0 && value == 0.0)
      return (int) byte.MaxValue;
    int num = Math.Sign(value);
    value = value > 0.0 ? Math.Floor(value) : Math.Ceiling(value);
    value += (double) num * 0.1;
    return digit;
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
