// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.SignificantDigitToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class SignificantDigitToken : DigitToken
{
  private const char DEF_FORMAT_CHAR = '0';

  public override int TryParse(string strFormat, int iIndex)
  {
    int num = strFormat != null ? strFormat.Length : throw new ArgumentNullException(nameof (strFormat));
    if (num == 0)
      throw new ArgumentException("strFormat - string cannot be empty");
    char c = iIndex >= 0 && iIndex <= num - 1 ? strFormat[iIndex] : throw new ArgumentOutOfRangeException(nameof (iIndex), "Value cannot be less than 0 and greater than format length - 1.");
    if (char.IsDigit(c))
    {
      ++iIndex;
      this.m_strFormat = c.ToString();
    }
    else if (strFormat[iIndex] == '\\' && (int) strFormat[iIndex + 1] == (int) this.FormatChar)
    {
      this.m_strFormat = strFormat[iIndex + 1].ToString();
      iIndex += 2;
    }
    return iIndex;
  }

  public override TokenType TokenType => TokenType.SignificantDigit;

  public override char FormatChar
  {
    get => this.m_strFormat == null ? '0' : Convert.ToChar(this.m_strFormat);
  }
}
