// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.SectionSeparatorToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class SectionSeparatorToken : FormatTokenBase
{
  private const string DEF_SEPARATOR = ";";

  public SectionSeparatorToken() => this.m_strFormat = ";";

  public override int TryParse(string strFormat, int iIndex)
  {
    int num = strFormat != null ? strFormat.Length : throw new ArgumentNullException(nameof (strFormat));
    if (num == 0)
      throw new ArgumentException("strFormat - string cannot be empty");
    if (iIndex < 0 || iIndex > num - 1)
      throw new ArgumentOutOfRangeException(nameof (iIndex), "Value cannot be less than 0 and greater than than format length - 1.");
    int length = this.m_strFormat.Length;
    if (string.Compare(strFormat, iIndex, this.m_strFormat, 0, length) == 0)
      iIndex += length;
    return iIndex;
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    throw new NotSupportedException();
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols)
  {
    throw new NotSupportedException();
  }

  public override TokenType TokenType => TokenType.Section;
}
