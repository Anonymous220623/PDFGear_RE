// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.CultureToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class CultureToken : FormatTokenBase
{
  private const string DEF_LOCALE_GROUP = "LocaleID";
  private const string DEF_CHAR_GROUP = "Character";
  private const int SystemSettingsLocaleId = 63488;
  private const int SystemSettingsTimeLocaleId = 62464;
  internal static readonly Regex CultureRegex = new Regex("\\[\\$(?<Character>[\\D]*)\\s*\\-\\s*(?<LocaleID>[0-9A-Za-z]+)\\s*\\]", RegexOptions.Compiled);
  private int m_iLocaleId;
  private string m_strCharacter;

  public override int TryParse(string strFormat, int iIndex)
  {
    Match m;
    int regex = this.TryParseRegex(CultureToken.CultureRegex, strFormat, iIndex, out m);
    if (regex != iIndex)
    {
      if (!int.TryParse(m.Groups["LocaleID"].Value, NumberStyles.HexNumber, (IFormatProvider) null, out this.m_iLocaleId))
        this.m_iLocaleId = new CultureInfo(new Regex("[\\w]{2}[-]{1}[\\w]{2}").Match(m.Value).Value).LCID;
      this.m_strCharacter = m.Groups["Character"].Value;
    }
    return regex;
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    return this.m_strCharacter == null ? string.Empty : this.m_strCharacter;
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols)
  {
    return this.m_strCharacter == null ? string.Empty : this.m_strCharacter;
  }

  public override TokenType TokenType => TokenType.Culture;

  public CultureInfo Culture
  {
    get
    {
      try
      {
        return new CultureInfo(this.m_iLocaleId);
      }
      catch (Exception ex)
      {
        return CultureInfo.CurrentCulture;
      }
    }
  }

  public bool UseSystemSettings => this.m_iLocaleId == 63488;

  internal bool UseSystemTimeSettings => this.m_iLocaleId == 62464;
}
