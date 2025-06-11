// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.TextToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class TextToken : SingleCharToken
{
  private const char DEF_FORMAT_CHAR = '@';

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    return value.ToString((IFormatProvider) culture);
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols)
  {
    return this.m_strFormat != null ? value : (string) null;
  }

  public override TokenType TokenType => TokenType.Text;

  public override char FormatChar => '@';
}
