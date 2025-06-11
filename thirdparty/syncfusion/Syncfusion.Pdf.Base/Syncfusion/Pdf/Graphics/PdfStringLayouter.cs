// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfStringLayouter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics.Fonts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfStringLayouter
{
  private string m_text;
  private PdfFont m_font;
  private PdfStringFormat m_format;
  private SizeF m_size;
  private RectangleF m_rect;
  private float m_pageHeight;
  private StringTokenizer m_reader;
  private bool m_isTabReplaced;
  private int m_tabOccuranceCount;

  internal PdfStringLayoutResult Layout(
    string text,
    PdfFont font,
    PdfStringFormat format,
    RectangleF rect,
    float pageHeight)
  {
    this.Initialize(text, font, format, rect, pageHeight);
    PdfStringLayoutResult stringLayoutResult = this.DoLayout();
    this.Clear();
    return stringLayoutResult;
  }

  public PdfStringLayoutResult Layout(
    string text,
    PdfFont font,
    PdfStringFormat format,
    SizeF size)
  {
    this.Initialize(text, font, format, size);
    PdfStringLayoutResult stringLayoutResult = this.DoLayout();
    this.Clear();
    return stringLayoutResult;
  }

  private void Initialize(
    string text,
    PdfFont font,
    PdfStringFormat format,
    RectangleF rect,
    float pageHeight)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    this.m_text = text;
    this.m_font = font;
    this.m_format = format;
    this.m_size = rect.Size;
    this.m_rect = rect;
    this.m_pageHeight = pageHeight;
    this.m_reader = new StringTokenizer(text);
  }

  private void Initialize(string text, PdfFont font, PdfStringFormat format, SizeF size)
  {
    this.Initialize(text, font, format, new RectangleF(PointF.Empty, size), 0.0f);
  }

  private PdfStringLayoutResult DoLayout()
  {
    PdfStringLayoutResult result = new PdfStringLayoutResult();
    PdfStringLayoutResult stringLayoutResult = new PdfStringLayoutResult();
    List<LineInfo> lines = new List<LineInfo>();
    string line = this.m_reader.PeekLine();
    float lineIndent = this.GetLineIndent(true);
    while (line != null)
    {
      PdfStringLayoutResult lineResult;
      if (this.m_format != null && this.m_format.ComplexScript && this.m_font is PdfTrueTypeFont && (this.m_font as PdfTrueTypeFont).InternalFont is UnicodeTrueTypeFont && (this.m_font as PdfTrueTypeFont).TtfReader.isOTFFont())
      {
        PdfTrueTypeFont font = this.m_font as PdfTrueTypeFont;
        Dictionary<ScriptTags, int> dictionary = new Dictionary<ScriptTags, int>();
        LanguageUtil languageUtil = new LanguageUtil();
        for (int index = 0; index < line.Length; ++index)
        {
          ScriptTags language = languageUtil.GetLanguage(line[index]);
          if (language != (ScriptTags) 0)
          {
            if (dictionary.ContainsKey(language))
              ++dictionary[language];
            else
              dictionary.Add(language, 1);
          }
        }
        ScriptTags[] scriptTagsArray = new ScriptTags[dictionary.Count];
        dictionary.Keys.CopyTo(scriptTagsArray, 0);
        bool flag = false;
        foreach (ScriptTags scriptTags in scriptTagsArray)
        {
          if (font.TtfReader.supportedScriptTags.Contains(scriptTags))
          {
            flag = true;
            break;
          }
        }
        lineResult = !flag ? this.LayoutLine(line, lineIndent) : this.LayoutLine(line, lineIndent, scriptTagsArray);
      }
      else
        lineResult = this.LayoutLine(line, lineIndent);
      if (!lineResult.Empty)
      {
        int numInserted = 0;
        if (!this.CopyToResult(result, lineResult, lines, out numInserted))
        {
          this.m_reader.Read(numInserted);
          break;
        }
      }
      if (lineResult.Remainder == null || lineResult.Remainder.Length <= 0)
      {
        this.m_reader.ReadLine();
        line = this.m_reader.PeekLine();
        lineIndent = this.GetLineIndent(false);
      }
      else
        break;
    }
    this.FinalizeResult(result, lines);
    return result;
  }

  private bool CopyToResult(
    PdfStringLayoutResult result,
    PdfStringLayoutResult lineResult,
    List<LineInfo> lines,
    out int numInserted)
  {
    if (result == null)
      throw new ArgumentNullException(nameof (result));
    if (lineResult == null)
      throw new ArgumentNullException(nameof (lineResult));
    if (lines == null)
      throw new ArgumentNullException(nameof (lines));
    bool result1 = true;
    bool flag = this.m_format != null && !this.m_format.LineLimit;
    float num1 = result.ActualSize.Height;
    float num2 = this.m_size.Height;
    if ((double) this.m_pageHeight > 0.0 && (double) num2 + (double) this.m_rect.Y > (double) this.m_pageHeight)
    {
      float val1 = this.m_rect.Y - this.m_pageHeight;
      num2 = Math.Max(val1, -val1);
    }
    numInserted = 0;
    if (lineResult.Lines != null)
    {
      int index = 0;
      for (int length = lineResult.Lines.Length; index < length; ++index)
      {
        float num3 = num1 + lineResult.LineHeight;
        if (index == lineResult.Lines.Length - 1 && this.m_format != null && !this.m_format.m_isList && (double) this.m_format.LineSpacing != 0.0)
          num3 -= this.m_format.LineSpacing;
        if ((double) num3 <= (double) num2 || (double) num2 <= 0.0 || flag)
        {
          LineInfo line = lineResult.Lines[index];
          if (!this.m_isTabReplaced)
          {
            numInserted += line.Text.Length;
          }
          else
          {
            int num4 = this.m_tabOccuranceCount * 3;
            numInserted += line.Text.Length - num4;
            this.m_isTabReplaced = false;
          }
          LineInfo lineInfo = this.TrimLine(line, lines.Count == 0);
          lines.Add(lineInfo);
          SizeF actualSize = result.ActualSize;
          actualSize.Width = Math.Max(actualSize.Width, lineInfo.Width);
          result.m_actualSize = actualSize;
          if ((double) num3 >= (double) num2 && (double) num2 > 0.0 && flag)
          {
            if (this.m_format == null || !this.m_format.NoClip)
            {
              float num5 = num3 - num2;
              float num6 = lineResult.LineHeight - num5;
              num1 += num6;
            }
            else
              num1 = num3;
            result1 = false;
            break;
          }
          num1 = num3;
        }
        else
        {
          result1 = false;
          break;
        }
      }
    }
    if ((double) num1 != (double) result.ActualSize.Height)
    {
      SizeF actualSize = result.ActualSize with
      {
        Height = num1
      };
      result.m_actualSize = actualSize;
    }
    return result1;
  }

  private void FinalizeResult(PdfStringLayoutResult result, List<LineInfo> lines)
  {
    if (result == null)
      throw new ArgumentNullException(nameof (result));
    result.m_lines = lines != null ? lines.ToArray() : throw new ArgumentNullException(nameof (lines));
    result.m_lineHeight = this.GetLineHeight();
    if (lines.Count != 0 && lines[lines.Count - 1].m_lineType == (LineType.NewLineBreak | LineType.FirstParagraphLine) && this.m_reader.Position == 0 && this.m_reader.Length >= 2)
      this.m_reader.Position = 2;
    if (!this.m_reader.EOF)
      result.m_remainder = this.m_reader.ReadToEnd();
    lines.Clear();
  }

  private void Clear()
  {
    this.m_font = (PdfFont) null;
    this.m_format = (PdfStringFormat) null;
    this.m_reader.Close();
    this.m_reader = (StringTokenizer) null;
    this.m_text = (string) null;
  }

  private float GetLineHeight()
  {
    float lineHeight = this.m_font.Height;
    if (this.m_format != null && (double) this.m_format.LineSpacing != 0.0)
      lineHeight = this.m_format.LineSpacing + this.m_font.Height;
    return lineHeight;
  }

  private PdfStringLayoutResult LayoutLine(string line, float lineIndent, ScriptTags[] tags)
  {
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    if (line.Contains("\t"))
    {
      this.m_tabOccuranceCount = 0;
      int startIndex = 0;
      string str = "\t";
      int num;
      while ((num = line.IndexOf(str, startIndex)) != -1)
      {
        startIndex = num + str.Length;
        ++this.m_tabOccuranceCount;
      }
      line = line.Replace("\t", "    ");
      this.m_isTabReplaced = true;
    }
    PdfStringLayoutResult lineResult = new PdfStringLayoutResult();
    lineResult.m_lineHeight = this.GetLineHeight();
    List<LineInfo> lines = new List<LineInfo>();
    float width = this.m_size.Width;
    OtfGlyphInfoList glyphList;
    float lineWidth1 = this.GetLineWidth(line, out glyphList, tags) + lineIndent;
    LineType lineType = LineType.FirstParagraphLine;
    bool readWord = true;
    if ((double) width <= 0.0 || Math.Round((double) lineWidth1, 2) <= Math.Round((double) width, 2))
      this.AddToLineResult(lineResult, lines, line, lineWidth1, LineType.NewLineBreak | lineType, glyphList);
    else if (glyphList != null)
    {
      List<OtfGlyphInfo> glyphs1 = new List<OtfGlyphInfo>();
      OtfGlyphTokenizer otfGlyphTokenizer = new OtfGlyphTokenizer(glyphList);
      string text = (string) null;
      OtfGlyphInfo[] glyphs2 = otfGlyphTokenizer.ReadWord(out text);
      StringTokenizer stringTokenizer = new StringTokenizer(line);
      string str1 = stringTokenizer.PeekWord();
      float num1 = lineIndent;
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      float num2 = 0.0f;
      float outWordSpace = 0.0f;
      float outCharSpace = 0.0f;
      int num3 = 0;
      bool flag1 = false;
      bool flag2 = false;
      while (glyphs2.Length > 0)
      {
        num2 = otfGlyphTokenizer.GetLineWidth(glyphs2, this.m_font as PdfTrueTypeFont, this.m_format, stringBuilder1.ToString() + text, out outWordSpace, out outCharSpace);
        if ((double) num1 + (double) num2 + (double) outWordSpace + (double) outCharSpace > (double) width)
        {
          if (str1.Length > 0 && stringBuilder2.Length == 0)
          {
            if (str1.Length == 1)
            {
              stringBuilder2.Append(str1);
              break;
            }
            int num4 = 0;
            stringTokenizer.Position = num3;
            string str2 = stringTokenizer.Peek().ToString();
            foreach (OtfGlyphInfo glyphs3 in glyphs2)
            {
              float num5 = 0.0f;
              float lineWidth2 = otfGlyphTokenizer.GetLineWidth(glyphs3, this.m_font as PdfTrueTypeFont, this.m_format);
              if (this.m_format != null && (double) this.m_format.CharacterSpacing != 0.0)
              {
                if (glyphs3.Characters != null && glyphs3.Characters.Length > 0)
                {
                  foreach (int character in glyphs3.Characters)
                    num5 += this.m_format.CharacterSpacing;
                }
                else
                  num5 += this.m_format.CharacterSpacing;
              }
              if ((double) num1 + (double) outWordSpace + (double) outCharSpace + (double) num5 + (double) lineWidth2 > (double) width)
              {
                if (str2.Length == 1 && stringTokenizer.Position != 0 && stringBuilder2.Length == 0)
                {
                  otfGlyphTokenizer.m_position -= glyphs2.Length;
                  ++otfGlyphTokenizer.m_position;
                  ++stringTokenizer.Position;
                  stringBuilder1.Append(glyphs3.Characters);
                  stringBuilder2.Append(str2);
                  flag1 = true;
                  break;
                }
                otfGlyphTokenizer.m_position -= glyphs2.Length;
                otfGlyphTokenizer.m_position += num4;
                stringTokenizer.Position = num3;
                if (!flag1)
                {
                  flag2 = true;
                  break;
                }
                break;
              }
              ++num4;
              num1 += lineWidth2;
              outCharSpace += num5;
              glyphs1.Add(glyphs3);
              if (glyphs3.Characters != null)
                stringBuilder1.Append(glyphs3.Characters);
              stringBuilder2.Append(str2);
              if (glyphs3.Characters.Length > 1)
              {
                for (int index = 1; index < glyphs3.Characters.Length; ++index)
                {
                  int num6 = (int) stringTokenizer.Read();
                  string str3 = stringTokenizer.Peek().ToString();
                  stringBuilder2.Append(str3);
                }
              }
              int num7 = (int) stringTokenizer.Read();
              str2 = stringTokenizer.Peek().ToString();
              num3 = stringTokenizer.Position;
              flag1 = true;
            }
            this.AddToLineResult(lineResult, lines, stringBuilder2.ToString(), num1 + outWordSpace + outCharSpace, LineType.LayoutBreak | lineType, new OtfGlyphInfoList(glyphs1));
            lineType = LineType.None;
            glyphs1 = new List<OtfGlyphInfo>();
            num1 = 0.0f;
            outWordSpace = 0.0f;
            outCharSpace = 0.0f;
            stringBuilder1 = new StringBuilder();
            stringBuilder2 = new StringBuilder();
          }
          else
          {
            otfGlyphTokenizer.m_position -= glyphs2.Length;
            stringTokenizer.Position = num3;
            this.AddToLineResult(lineResult, lines, stringBuilder2.ToString(), num1 + outWordSpace + outCharSpace, LineType.LayoutBreak | lineType, new OtfGlyphInfoList(glyphs1));
            lineType = LineType.None;
            glyphs1 = new List<OtfGlyphInfo>();
            num1 = 0.0f;
            outWordSpace = 0.0f;
            outCharSpace = 0.0f;
            stringBuilder1 = new StringBuilder();
            stringBuilder2 = new StringBuilder();
          }
        }
        else
        {
          foreach (OtfGlyphInfo otfGlyphInfo in glyphs2)
            glyphs1.Add(otfGlyphInfo);
          num1 += num2;
          stringBuilder1.Append(text);
          stringBuilder2.Append(str1);
        }
        text = (string) null;
        glyphs2 = otfGlyphTokenizer.ReadWord(out text);
        num3 = stringTokenizer.Position;
        if (flag1)
        {
          str1 = stringTokenizer.ReadWord();
          --stringTokenizer.Position;
          flag1 = false;
        }
        else if (flag2 && stringTokenizer.Position == 0)
        {
          str1 = stringTokenizer.PeekWord();
          flag2 = false;
        }
        else
        {
          stringTokenizer.ReadWord();
          str1 = stringTokenizer.PeekWord();
        }
      }
      if (glyphs1.Count > 0)
      {
        float num8 = num1 + num2;
        this.AddToLineResult(lineResult, lines, stringBuilder2.ToString(), num8 + outWordSpace + outCharSpace, LineType.NewLineBreak | LineType.LastParagraphLine, new OtfGlyphInfoList(glyphs1));
      }
    }
    else
      this.LayoutLine(line, lineWidth1, lineIndent, lineResult, width, lineType, readWord, lines);
    lineResult.m_lines = lines.ToArray();
    lines.Clear();
    return lineResult;
  }

  private void LayoutLine(
    string line,
    float lineIndent,
    PdfStringLayoutResult lineResult,
    float maxWidth,
    LineType lineType,
    List<LineInfo> lines,
    ScriptTags[] tags)
  {
    StringTokenizer stringTokenizer = new StringTokenizer(line);
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder stringBuilder2 = new StringBuilder();
    float num = lineIndent;
    string str = stringTokenizer.PeekWord();
    OtfGlyphInfoList glyphList1 = (OtfGlyphInfoList) null;
    float lineWidth1 = 0.0f;
    while (str != null)
    {
      stringBuilder1.Append(str);
      lineWidth1 = this.GetLineWidth(stringBuilder1.ToString(), out glyphList1, tags) + num;
      if ((double) lineWidth1 > (double) maxWidth)
      {
        stringBuilder1 = stringBuilder2;
        float lineWidth2 = lineWidth1;
        string line1 = stringBuilder2.ToString();
        OtfGlyphInfoList glyphList2 = glyphList1;
        for (int index = 0; index < str.Length; ++index)
        {
          stringBuilder1.Append(str[index]);
          glyphList1 = (OtfGlyphInfoList) null;
          lineWidth1 = this.GetLineWidth(stringBuilder1.ToString(), out glyphList1, tags) + num;
          if ((double) lineWidth1 > (double) maxWidth)
          {
            this.AddToLineResult(lineResult, lines, line1, lineWidth2, LineType.LayoutBreak | lineType, glyphList2);
            stringBuilder1 = new StringBuilder();
            stringBuilder2 = new StringBuilder();
            num = 0.0f;
            lineWidth1 = 0.0f;
            lineType = LineType.None;
            str = str.Substring(index, str.Length - index);
            break;
          }
          lineWidth2 = lineWidth1;
          line1 = stringBuilder1.ToString();
          glyphList2 = glyphList1;
        }
      }
      else
      {
        stringBuilder2.Append(str);
        stringTokenizer.ReadWord();
        str = stringTokenizer.PeekWord();
      }
    }
    if (stringBuilder2.Length <= 0)
      return;
    this.AddToLineResult(lineResult, lines, stringBuilder2.ToString(), lineWidth1, LineType.NewLineBreak | LineType.LastParagraphLine, glyphList1);
  }

  private void LayoutLine(
    string line,
    float lineWidth,
    float lineIndent,
    PdfStringLayoutResult lineResult,
    float maxWidth,
    LineType lineType,
    bool readWord,
    List<LineInfo> lines)
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder stringBuilder2 = new StringBuilder();
    lineWidth = lineIndent;
    float num1 = lineIndent;
    StringTokenizer stringTokenizer = new StringTokenizer(line);
    string str = stringTokenizer.PeekWord();
    bool flag1 = false;
    if (str.Length != stringTokenizer.Length && str == " ")
    {
      stringBuilder2.Append(str);
      stringBuilder1.Append(str);
      ++stringTokenizer.Position;
      str = stringTokenizer.PeekWord();
      bool flag2 = this.m_format != null && (double) this.m_format.ParagraphIndent == 0.0;
      bool flag3 = this.m_format != null && this.m_format.TextDirection == PdfTextDirection.None;
      bool flag4 = this.m_format != null && this.m_format.ComplexScript;
      bool flag5 = this.m_format != null && this.m_format.RightToLeft;
      flag1 = this.GetWrapType() == PdfWordWrapType.Word && flag2 && !flag5 && flag3 && !flag4;
    }
    while (str != null)
    {
      stringBuilder2.Append(str);
      float num2 = this.GetLineWidth(stringBuilder2.ToString()) + num1;
      if (stringBuilder2.ToString() == " ")
      {
        stringBuilder2.Length = 0;
        num2 = 0.0f;
      }
      if ((double) num2 > (double) maxWidth)
      {
        if (this.GetWrapType() == PdfWordWrapType.None)
        {
          StringBuilder stringBuilder3 = stringBuilder1;
          foreach (char ch in str)
          {
            stringBuilder3.Append(ch);
            float num3 = this.GetLineWidth(stringBuilder3.ToString()) + num1;
            if ((double) num3 > (double) maxWidth)
            {
              if (this.m_format == null || !this.m_format.NoClip)
              {
                stringBuilder3.Remove(stringBuilder3.Length - 1, 1);
                break;
              }
              break;
            }
            lineWidth = num3;
          }
          break;
        }
        if (stringBuilder2.Length == str.Length)
        {
          if (this.GetWrapType() == PdfWordWrapType.WordOnly)
          {
            lineResult.m_remainder = line.Substring(stringTokenizer.Position);
            break;
          }
          if (stringBuilder2.Length == 1)
          {
            stringBuilder1.Append(str);
            break;
          }
          readWord = false;
          stringBuilder2.Length = 0;
          str = stringTokenizer.Peek().ToString();
        }
        else if (this.GetWrapType() != PdfWordWrapType.Character || !readWord)
        {
          string line1 = stringBuilder1.ToString();
          if (line1 != " ")
            this.AddToLineResult(lineResult, lines, line1, lineWidth, LineType.LayoutBreak | lineType, (OtfGlyphInfoList) null);
          if (lines.Count == 0 && flag1 && str != " " && str.Length > 1)
          {
            readWord = false;
            stringBuilder2.Length = 0;
            stringBuilder2.Append(stringBuilder1.ToString());
            str = stringTokenizer.Peek().ToString();
          }
          else
          {
            stringBuilder2.Length = 0;
            stringBuilder1.Length = 0;
            lineWidth = 0.0f;
            num1 = 0.0f;
            lineType = LineType.None;
            str = readWord ? str : stringTokenizer.PeekWord();
            readWord = true;
          }
        }
        else
        {
          readWord = false;
          stringBuilder2.Length = 0;
          stringBuilder2.Append(stringBuilder1.ToString());
          str = stringTokenizer.Peek().ToString();
        }
      }
      else
      {
        stringBuilder1.Append(str);
        lineWidth = num2;
        if (readWord)
        {
          stringTokenizer.ReadWord();
          str = stringTokenizer.PeekWord();
        }
        else
        {
          int num4 = (int) stringTokenizer.Read();
          str = stringTokenizer.Peek().ToString();
        }
      }
    }
    if (stringBuilder1.Length > 0)
    {
      string line2 = stringBuilder1.ToString();
      this.AddToLineResult(lineResult, lines, line2, lineWidth, LineType.NewLineBreak | LineType.LastParagraphLine, (OtfGlyphInfoList) null);
    }
    stringTokenizer.Close();
  }

  private PdfStringLayoutResult LayoutLine(string line, float lineIndent)
  {
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    PdfStandardFont font = this.m_font as PdfStandardFont;
    if (line.Contains("\t") && font != null && font.fontEncoding == null || line.Contains("\t") && font == null)
    {
      this.m_tabOccuranceCount = 0;
      int startIndex = 0;
      string str = "\t";
      int num;
      while ((num = line.IndexOf(str, startIndex)) != -1)
      {
        startIndex = num + str.Length;
        ++this.m_tabOccuranceCount;
      }
      line = line.Replace("\t", "    ");
      this.m_isTabReplaced = true;
    }
    PdfStringLayoutResult lineResult = new PdfStringLayoutResult();
    lineResult.m_lineHeight = this.GetLineHeight();
    List<LineInfo> lines = new List<LineInfo>();
    float width = this.m_size.Width;
    float lineWidth = this.GetLineWidth(line) + lineIndent;
    LineType lineType = LineType.FirstParagraphLine;
    bool readWord = true;
    if ((double) width <= 0.0 || Math.Round((double) lineWidth, 2) <= Math.Round((double) width, 2))
      this.AddToLineResult(lineResult, lines, line, lineWidth, LineType.NewLineBreak | lineType, (OtfGlyphInfoList) null);
    else
      this.LayoutLine(line, lineWidth, lineIndent, lineResult, width, lineType, readWord, lines);
    lineResult.m_lines = lines.ToArray();
    lines.Clear();
    return lineResult;
  }

  private void AddToLineResult(
    PdfStringLayoutResult lineResult,
    List<LineInfo> lines,
    string line,
    float lineWidth,
    LineType breakType,
    OtfGlyphInfoList glyphList)
  {
    if (lineResult == null)
      throw new ArgumentNullException(nameof (lineResult));
    if (lines == null)
      throw new ArgumentNullException(nameof (lines));
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    LineInfo lineInfo = new LineInfo();
    if (glyphList != null)
      lineInfo.OpenTypeGlyphList = new OtfGlyphInfoList(glyphList.Glyphs);
    lineInfo.Text = line;
    lineInfo.Width = lineWidth;
    lineInfo.LineType = breakType;
    lines.Add(lineInfo);
    SizeF actualSize = lineResult.ActualSize;
    actualSize.Height += this.GetLineHeight();
    actualSize.Width = Math.Max(actualSize.Width, lineWidth);
    lineResult.m_actualSize = actualSize;
  }

  private LineInfo TrimLine(LineInfo info, bool firstLine)
  {
    string str = info.Text;
    float num = info.Width;
    bool flag1 = (info.LineType & LineType.FirstParagraphLine) == LineType.None;
    bool flag2 = this.m_format == null || !this.m_format.RightToLeft;
    char[] spaces = StringTokenizer.Spaces;
    OtfGlyphTokenizer otfGlyphTokenizer = new OtfGlyphTokenizer();
    if (flag1)
    {
      str = flag2 ? str.TrimStart(spaces) : str.TrimEnd(spaces);
      if (info.OpenTypeGlyphList != null)
        info.OpenTypeGlyphList = flag2 ? otfGlyphTokenizer.TrimStartSpaces(info.OpenTypeGlyphList) : otfGlyphTokenizer.TrimEndSpaces(info.OpenTypeGlyphList);
    }
    if (this.m_format == null || !this.m_format.MeasureTrailingSpaces)
    {
      if ((info.LineType & LineType.FirstParagraphLine) > LineType.None && StringTokenizer.IsWhitespace(str))
      {
        str = new string(' ', 1);
        if (this.m_font.Underline)
          str = string.Empty;
      }
      else
      {
        str = flag2 ? str.TrimEnd(spaces) : str.TrimStart(spaces);
        if (info.OpenTypeGlyphList != null)
          info.OpenTypeGlyphList = flag2 ? otfGlyphTokenizer.TrimEndSpaces(info.OpenTypeGlyphList) : otfGlyphTokenizer.TrimStartSpaces(info.OpenTypeGlyphList);
      }
    }
    if (str.Length != info.Text.Length)
    {
      float outWordSpace;
      float outCharSpace;
      num = info.OpenTypeGlyphList == null ? this.GetLineWidth(str) : otfGlyphTokenizer.GetLineWidth(info.OpenTypeGlyphList.Glyphs.ToArray(), this.m_font as PdfTrueTypeFont, this.m_format, str, out outWordSpace, out outCharSpace) + outWordSpace + outCharSpace;
      if ((info.LineType & LineType.FirstParagraphLine) > LineType.None)
        num += this.GetLineIndent(firstLine);
    }
    info.Text = str;
    info.Width = num;
    return info;
  }

  private float GetLineWidth(string line) => this.m_font.GetLineWidth(line, this.m_format);

  private float GetLineWidth(string line, out OtfGlyphInfoList glyphList, ScriptTags[] tags)
  {
    glyphList = (OtfGlyphInfoList) null;
    float lineWidth;
    if (this.m_font is PdfTrueTypeFont)
    {
      PdfTrueTypeFont font = this.m_font as PdfTrueTypeFont;
      lineWidth = !font.Unicode ? this.m_font.GetLineWidth(line, this.m_format) : font.GetLineWidth(line, this.m_format, out glyphList, tags);
    }
    else
      lineWidth = this.m_font.GetLineWidth(line, this.m_format);
    return lineWidth;
  }

  private float GetLineIndent(bool firstLine)
  {
    float lineIndent = 0.0f;
    if (this.m_format != null)
    {
      float val2 = firstLine ? this.m_format.FirstLineIndent : this.m_format.ParagraphIndent;
      lineIndent = (double) this.m_size.Width > 0.0 ? Math.Min(this.m_size.Width, val2) : val2;
    }
    return lineIndent;
  }

  private PdfWordWrapType GetWrapType()
  {
    return this.m_format != null ? this.m_format.WordWrap : PdfWordWrapType.Word;
  }
}
