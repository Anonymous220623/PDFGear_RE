// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.InBracketToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal abstract class InBracketToken : FormatTokenBase
{
  private const char DEF_START = '[';
  private const char DEF_END = ']';

  public override int TryParse(string strFormat, int iIndex)
  {
    int num = strFormat != null ? strFormat.Length : throw new ArgumentNullException(nameof (strFormat));
    if (num == 0)
      throw new ArgumentException("strFormat - string cannot be empty.");
    int iStartIndex = iIndex >= 0 && iIndex <= num - 1 ? iIndex : throw new ArgumentOutOfRangeException(nameof (iIndex), "Value cannot be less than 0 and greater than than format length - 1.");
    if (strFormat[iIndex] != '[')
      return iIndex;
    ++iIndex;
    int iEndIndex = strFormat.IndexOf(']', iIndex);
    return iEndIndex < iIndex ? iStartIndex : this.TryParse(strFormat, iStartIndex, iIndex, iEndIndex);
  }

  public abstract int TryParse(string strFormat, int iStartIndex, int iIndex, int iEndIndex);
}
