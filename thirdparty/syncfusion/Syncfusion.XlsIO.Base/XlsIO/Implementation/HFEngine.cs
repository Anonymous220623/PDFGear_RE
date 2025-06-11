// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.HFEngine
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class HFEngine : 
  RichTextString,
  IHFEngine,
  IRichTextString,
  IParentApplication,
  IOptimizedUpdate
{
  private const string DEF_AMP = "&";
  private const char DEF_UNDERLINE = 'U';
  private const char DEF_DOUBLE_UNDERLINE = 'E';
  private const char DEF_STRIKEOUT = 'S';
  private const char DEF_SUBSCRIPT = 'Y';
  private const char DEF_SUPERSCRIPT = 'X';
  private const char DEF_FONT_NAME_EDGE = '"';
  private const char DEF_FONT_STYLE_SEPARATOR = ',';
  private const string DEF_BOLD_VALUE = "bold";
  private const string DEF_ITALIC_VALUE = "italic";
  private const string DEF_REGULAR_STYLE = "regular";
  private const char DEF_SPACE = ' ';
  private List<FontImpl> m_arrFonts = new List<FontImpl>();

  public HFEngine(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_text = new TextWithFormat();
  }

  public void Parse(string strText)
  {
    if (strText == null || strText.Length == 0)
    {
      this.Clear();
    }
    else
    {
      int iPos = 0;
      int length = strText.Length;
      this.m_text = new TextWithFormat();
      this.m_arrFonts.Clear();
      this.m_arrFonts.Add(this.DefaultFont);
      FontImpl defaultFont = this.DefaultFont;
      StringBuilder builder = new StringBuilder(length);
      while (iPos < length)
      {
        int num = strText.IndexOf("&", iPos);
        if (num >= 0)
        {
          string str = strText.Substring(iPos, num - iPos);
          builder.Append(str);
          iPos = num + 1;
          this.ProcessCharacter(strText, builder, ref iPos, ref defaultFont);
        }
        else
        {
          string str = strText.Substring(iPos);
          builder.Append(str);
          this.AddTextBlock(builder, defaultFont);
          iPos = length;
        }
      }
    }
  }

  public string GetHeaderFooterString()
  {
    StringBuilder builder = new StringBuilder();
    SortedList<int, int> formattingRuns = this.m_text.FormattingRuns;
    IList<int> keys = formattingRuns.Keys;
    IList<int> values = formattingRuns.Values;
    FontImpl prevFont = (FontImpl) null;
    int iPrevPos = 0;
    int index1 = 0;
    for (int formattingRunsCount = this.m_text.FormattingRunsCount; index1 < formattingRunsCount; ++index1)
    {
      int index2 = values[index1];
      int iCurPos = keys[index1];
      FontImpl arrFont = this.m_arrFonts[index2];
      this.WritePrevTextBlock(builder, iPrevPos, iCurPos);
      this.WriteFontDifference(builder, prevFont, arrFont);
      iPrevPos = iCurPos;
      prevFont = arrFont;
    }
    this.WritePrevTextBlock(builder, iPrevPos, this.m_text.Text.Length);
    return builder.ToString();
  }

  private void ProcessCharacter(
    string strText,
    StringBuilder builder,
    ref int iPos,
    ref FontImpl font)
  {
    int num1 = strText != null ? strText.Length : throw new ArgumentNullException(nameof (strText));
    char c = strText[iPos];
    switch (c)
    {
      case '"':
        int startIndex = iPos + 1;
        int num2 = strText.IndexOf('"', startIndex);
        if (num2 != -1)
        {
          string strFontName = strText.Substring(startIndex, num2 - startIndex);
          this.AddTextBlock(builder, font);
          font = font.TypedClone();
          this.SetFont(font, strFontName);
          iPos = num2 + 1;
          return;
        }
        break;
      case 'E':
        this.AddTextBlock(builder, font);
        font = font.TypedClone();
        ExcelUnderline underline1 = font.Underline;
        font.Underline = underline1 == ExcelUnderline.DoubleAccounting ? ExcelUnderline.None : ExcelUnderline.DoubleAccounting;
        ++iPos;
        return;
      case 'S':
        this.AddTextBlock(builder, font);
        font = font.TypedClone();
        font.Strikethrough = !font.Strikethrough;
        ++iPos;
        return;
      case 'U':
        this.AddTextBlock(builder, font);
        font = font.TypedClone();
        ExcelUnderline underline2 = font.Underline;
        font.Underline = underline2 == ExcelUnderline.SingleAccounting ? ExcelUnderline.None : ExcelUnderline.SingleAccounting;
        ++iPos;
        return;
      case 'X':
        this.AddTextBlock(builder, font);
        font = font.TypedClone();
        font.Superscript = !font.Superscript;
        ++iPos;
        return;
      case 'Y':
        this.AddTextBlock(builder, font);
        font = font.TypedClone();
        font.Subscript = !font.Subscript;
        ++iPos;
        return;
    }
    if (char.IsDigit(c))
    {
      int num3 = 0;
      for (; char.IsDigit(c); c = strText[iPos])
      {
        num3 = 10 * num3 + (int) c - 48 /*0x30*/;
        ++iPos;
        if (iPos >= num1)
          break;
      }
      this.AddTextBlock(builder, font);
      font = font.TypedClone();
      font.Size = (double) num3;
    }
    else
    {
      builder.Append("&");
      builder.Append(c);
      ++iPos;
    }
  }

  private void AddTextBlock(StringBuilder builder, FontImpl font)
  {
    int length = this.m_text.Text.Length;
    this.m_text.Text += builder.ToString();
    if (builder.Length <= 0)
      return;
    int iFontIndex = this.AddFont((IFont) font);
    this.m_text.SetTextFontIndex(length, length + builder.Length - 1, iFontIndex);
    builder.Length = 0;
  }

  protected override int AddFont(IFont fontToAdd)
  {
    fontToAdd = fontToAdd != null ? (IFont) ((IInternalFont) fontToAdd).Font : throw new ArgumentNullException(nameof (fontToAdd));
    int index = 0;
    for (int count = this.m_arrFonts.Count; index < count; ++index)
    {
      if (this.m_arrFonts[index] == fontToAdd)
        return index;
    }
    this.m_arrFonts.Add((FontImpl) fontToAdd);
    return this.m_arrFonts.Count - 1;
  }

  private void SetFont(FontImpl font, string strFontName)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    switch (strFontName)
    {
      case null:
        break;
      case "":
        break;
      default:
        int length = strFontName.IndexOf(',');
        string strFontStyle = (string) null;
        if (length >= 0)
        {
          strFontStyle = strFontName.Substring(length + 1);
          strFontName = strFontName.Substring(0, length);
        }
        ENUMLOGFONTEX font1 = this.FindFont(strFontName, strFontStyle);
        font.FontName = strFontName;
        if (font1 != null)
        {
          HFEngine.CopyFontSettings(Font.FromLogFont((object) font1.LogFont), font);
          break;
        }
        string lower = strFontStyle.ToLower();
        font.Bold = lower.IndexOf("bold") >= 0;
        font.Italic = lower.IndexOf("italic") >= 0;
        break;
    }
  }

  public ENUMLOGFONTEX FindFont(string strFontName, string strFontStyle)
  {
    LOGFONT lpLogfont = new LOGFONT();
    int length = strFontName.Length;
    if (strFontName[length - 1] != char.MinValue)
      strFontName += (string) (object) char.MinValue;
    Encoding.ASCII.GetBytes(strFontName).CopyTo((Array) lpLogfont.lfFaceName, 0);
    lpLogfont.lfCharSet = (byte) 1;
    Graphics innerGraphics = this.m_book.InnerGraphics;
    IntPtr hdc = innerGraphics.GetHdc();
    HFEngine.FindFontData findFontData = new HFEngine.FindFontData();
    findFontData.strFontStyle = strFontStyle;
    object objData = (object) findFontData;
    API.EnumFontFamiliesEx(hdc, lpLogfont, new EnumFontFamExProc(HFEngine.FontEnumProc), ref objData, 0);
    innerGraphics.ReleaseHdc(hdc);
    return findFontData.fontData;
  }

  private static int FontEnumProc(
    ENUMLOGFONTEX lpelf,
    IntPtr lpntm,
    int FontType,
    ref object objData)
  {
    if (!(objData is HFEngine.FindFontData findFontData))
      return 0;
    if (string.Compare(lpelf.Style, findFontData.strFontStyle, StringComparison.CurrentCultureIgnoreCase) != 0)
      return 1;
    findFontData.fontData = lpelf;
    return 0;
  }

  private static void CopyFontSettings(Font sourceFont, FontImpl destFont)
  {
    if (sourceFont == null)
      throw new ArgumentNullException(nameof (sourceFont));
    if (destFont == null)
      throw new ArgumentNullException(nameof (destFont));
    destFont.Bold = sourceFont.Bold;
    destFont.Italic = sourceFont.Italic;
  }

  private void WritePrevTextBlock(StringBuilder builder, int iPrevPos, int iCurPos)
  {
    if (builder == null)
      throw new ArgumentNullException(nameof (builder));
    if (iPrevPos >= iCurPos)
      return;
    builder.Append(this.m_text.Text, iPrevPos, iCurPos - iPrevPos);
  }

  private void WriteFontDifference(StringBuilder builder, FontImpl prevFont, FontImpl curFont)
  {
    if (builder == null)
      throw new ArgumentNullException(nameof (builder));
    this.WriteFontName(builder, prevFont, curFont);
    this.WriteFontSize(builder, prevFont, curFont);
    this.WriteFontUnderline(builder, prevFont, curFont);
    this.WriteFontSupSub(builder, prevFont, curFont);
    this.WriteFontStrikeout(builder, prevFont, curFont);
  }

  private void WriteFontName(StringBuilder builder, FontImpl prevFont, FontImpl curFont)
  {
    if (builder == null)
      throw new ArgumentNullException(nameof (builder));
    bool flag1 = prevFont == null;
    bool flag2 = curFont == null;
    if (flag1 && flag2)
      return;
    if (flag2)
      throw new ArgumentNullException(nameof (curFont));
    if (!flag1 && prevFont.FontName == curFont.FontName && prevFont.Bold == curFont.Bold && prevFont.Italic == curFont.Italic)
      return;
    builder.Append("&");
    builder.Append('"');
    builder.Append(curFont.FontName);
    if (curFont.Bold || curFont.Italic)
    {
      builder.Append(',');
      if (curFont.Bold)
        builder.Append("bold");
      if (curFont.Italic)
      {
        if (curFont.Bold)
          builder.Append(' ');
        builder.Append("italic");
      }
    }
    else if (!flag1 && (prevFont.Bold || prevFont.Italic))
    {
      builder.Append(',');
      builder.Append("regular");
    }
    builder.Append('"');
  }

  private void WriteFontUnderline(StringBuilder builder, FontImpl prevFont, FontImpl curFont)
  {
    if (builder == null)
      throw new ArgumentNullException(nameof (builder));
    bool flag1 = prevFont == null;
    bool flag2 = curFont == null;
    if (flag1 && flag2)
      return;
    if (flag2)
      throw new ArgumentNullException(nameof (curFont));
    if (!flag1 && prevFont.Underline == curFont.Underline)
      return;
    switch (curFont.Underline)
    {
      case ExcelUnderline.None:
        this.WriteFontUnderline(builder, (FontImpl) null, prevFont);
        break;
      case ExcelUnderline.Single:
      case ExcelUnderline.SingleAccounting:
        builder.Append("&");
        builder.Append('U');
        break;
      case ExcelUnderline.Double:
      case ExcelUnderline.DoubleAccounting:
        builder.Append("&");
        builder.Append('E');
        break;
      default:
        throw new ArgumentOutOfRangeException("underline");
    }
  }

  private void WriteFontSupSub(StringBuilder builder, FontImpl prevFont, FontImpl curFont)
  {
    if (builder == null)
      throw new ArgumentNullException(nameof (builder));
    bool flag1 = prevFont == null;
    bool flag2 = curFont == null;
    if (flag1 && flag2)
      return;
    if (flag2)
      throw new ArgumentNullException(nameof (curFont));
    if (!flag1 && prevFont.Superscript == curFont.Superscript && prevFont.Subscript == curFont.Subscript)
      return;
    if (curFont.Superscript)
    {
      builder.Append("&");
      builder.Append('X');
    }
    else if (curFont.Subscript)
    {
      builder.Append("&");
      builder.Append('Y');
    }
    else
      this.WriteFontSupSub(builder, (FontImpl) null, prevFont);
  }

  private void WriteFontStrikeout(StringBuilder builder, FontImpl prevFont, FontImpl curFont)
  {
    if (builder == null)
      throw new ArgumentNullException(nameof (builder));
    bool flag1 = prevFont == null;
    bool flag2 = curFont == null;
    if (flag1 && flag2)
      return;
    if (flag2)
      throw new ArgumentNullException(nameof (curFont));
    if (!flag1 && prevFont.Strikethrough == curFont.Strikethrough || flag1 && !curFont.Strikethrough)
      return;
    builder.Append("&");
    builder.Append('S');
  }

  private void WriteFontSize(StringBuilder builder, FontImpl prevFont, FontImpl curFont)
  {
    if (builder == null)
      throw new ArgumentNullException(nameof (builder));
    bool flag1 = prevFont == null;
    bool flag2 = curFont == null;
    if (flag1 && flag2)
      return;
    if (flag2)
      throw new ArgumentNullException(nameof (curFont));
    if (!flag1 && prevFont.Size == curFont.Size)
      return;
    builder.Append("&");
    builder.Append(curFont.Size);
  }

  protected internal override FontImpl GetFontByIndex(int iFontIndex)
  {
    return this.m_arrFonts[iFontIndex];
  }

  public override void Clear()
  {
    base.Clear();
    this.m_arrFonts.Clear();
  }

  public override void EndUpdate()
  {
    if (this.m_text.FormattingRunsCount != 0)
      return;
    int length = this.m_text.Text.Length;
    if (length <= 0)
      return;
    this.SetFont(0, length - 1, (IFont) this.DefaultFont);
  }

  private class FindFontData
  {
    public string strFontStyle;
    public ENUMLOGFONTEX fontData;
  }
}
