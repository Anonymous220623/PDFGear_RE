// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.CultureToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal class CultureToken : FormatTokenBase
{
  private const string DEF_LOCALE_GROUP = "LocaleID";
  private const string DEF_CHAR_GROUP = "Character";
  private const int SystemSettingsLocaleId = 63488;
  private static readonly Regex CultureRegex = new Regex("\\[\\$(?<Character>.?)\\-(?<LocaleID>[0-9A-Za-z]+)\\]", RegexOptions.Compiled);
  private int m_iLocaleId;
  private string m_strCharacter;

  public override int TryParse(string strFormat, int iIndex)
  {
    Match m;
    int regex = this.TryParseRegex(CultureToken.CultureRegex, strFormat, iIndex, out m);
    if (regex != iIndex)
    {
      this.m_iLocaleId = int.Parse(m.Groups["LocaleID"].Value, NumberStyles.HexNumber);
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
}
