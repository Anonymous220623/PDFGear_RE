// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.SingleCharToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public abstract class SingleCharToken : FormatTokenBase
{
  public override int TryParse(string strFormat, int iIndex)
  {
    int num = strFormat != null ? strFormat.Length : throw new ArgumentNullException(nameof (strFormat));
    if (num == 0)
      throw new ArgumentException("strFormat - string cannot be empty");
    char ch = iIndex >= 0 && iIndex <= num - 1 ? strFormat[iIndex] : throw new ArgumentOutOfRangeException(nameof (iIndex), "Value cannot be less than 0 and greater than than format length - 1.");
    if ((int) ch == (int) this.FormatChar)
    {
      ++iIndex;
      this.m_strFormat = ch.ToString();
    }
    else if (strFormat[iIndex] == '\\' && (int) strFormat[iIndex + 1] == (int) this.FormatChar)
    {
      this.m_strFormat = strFormat[iIndex + 1].ToString();
      iIndex += 2;
    }
    return iIndex;
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    return this.m_strFormat;
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => this.m_strFormat;

  public abstract char FormatChar { get; }
}
