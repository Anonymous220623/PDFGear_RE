// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.CharacterToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal class CharacterToken : FormatTokenBase
{
  private const char DEF_START = '\\';
  private const char DEF_FORMAT_CHAR = '@';

  public override int TryParse(string strFormat, int iIndex)
  {
    switch (strFormat)
    {
      case null:
        throw new ArgumentNullException(nameof (strFormat));
      case "":
        throw new ArgumentException("strFormat - string cannot be empty.");
      default:
        if (strFormat[iIndex] == '\\')
        {
          this.m_strFormat = strFormat[iIndex + 1].ToString();
          if (this.m_strFormat != '@'.ToString())
            iIndex += 2;
          else
            this.m_strFormat = '@'.ToString();
        }
        else if (strFormat[iIndex] == '[' && strFormat[iIndex + 2] == '$')
        {
          this.m_strFormat = strFormat[iIndex + 1].ToString();
          iIndex = strFormat.IndexOf(']', iIndex + 3) + 1;
        }
        return iIndex;
    }
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

  public override TokenType TokenType => TokenType.Character;
}
