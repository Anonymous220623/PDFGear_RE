// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.StringSplitter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Office;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Layouting;

internal class StringSplitter
{
  private DrawingContext m_dc;
  private Hyphenator hyphen;

  public StringSplitter() => this.m_dc = new DrawingContext();

  public StringSplitResult Split(
    string text,
    Font font,
    Font defaultFont,
    StringFormat format,
    SizeF size,
    WCharacterFormat charFormat,
    ref bool isLastWordFit,
    bool isTabStopInterSectingfloattingItem,
    ref bool isTrailSpacesWrapped,
    bool isAutoHyphenated,
    IStringWidget strWidget)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    return this.DoSplit(ref isLastWordFit, text, font, defaultFont, charFormat, format, size, isTabStopInterSectingfloattingItem, ref isTrailSpacesWrapped, isAutoHyphenated, strWidget);
  }

  internal void Close() => this.m_dc.Close();

  private StringSplitResult DoSplit(
    ref bool isLastWordFit,
    string text,
    Font font,
    Font defaultFont,
    WCharacterFormat charFormat,
    StringFormat format,
    SizeF size,
    bool isTabStopInterSectingfloattingItem,
    ref bool isTrailSpacesWrapped,
    bool isAutoHyphenated,
    IStringWidget strWidget)
  {
    StringSplitResult result = new StringSplitResult();
    StringSplitResult stringSplitResult = new StringSplitResult();
    List<TextLineInfo> lines = new List<TextLineInfo>();
    StringParser reader = new StringParser(text);
    string line = reader.PeekLine();
    float lineIndent = this.GetLineIndent(true, format, size);
    while (line != null)
    {
      StringSplitResult lineResult = this.SplitLine(line, lineIndent, ref isLastWordFit, font, defaultFont, charFormat, format, size, isTabStopInterSectingfloattingItem, ref isTrailSpacesWrapped, isAutoHyphenated, strWidget as IEntity);
      if (reader.Length == line.Length)
        return lineResult;
      if (!lineResult.Empty)
      {
        int numInserted = 0;
        if (!this.CopyToResult(result, lineResult, lines, out numInserted, font, defaultFont, charFormat, format, size))
        {
          reader.Read(numInserted);
          break;
        }
        if (reader.Length == reader.Position)
          break;
      }
      if (lineResult.Remainder == null || lineResult.Remainder.Length <= 0)
      {
        reader.ReadLine();
        line = reader.PeekLine();
        lineIndent = this.GetLineIndent(false, format, size);
      }
      else
        break;
    }
    this.SaveResult(result, lines, reader, font, text);
    return result;
  }

  private bool CopyToResult(
    StringSplitResult result,
    StringSplitResult lineResult,
    List<TextLineInfo> lines,
    out int numInserted,
    Font font,
    Font defaultFont,
    WCharacterFormat charFormat,
    StringFormat format,
    SizeF textSize)
  {
    if (result == null)
      throw new ArgumentNullException(nameof (result));
    if (lineResult == null)
      throw new ArgumentNullException(nameof (lineResult));
    if (lines == null)
      throw new ArgumentNullException(nameof (lines));
    bool result1 = true;
    bool flag = format != null && format.FormatFlags != StringFormatFlags.LineLimit;
    float num1 = result.ActualSize.Height;
    float height = textSize.Height;
    numInserted = 0;
    if (lineResult.Lines != null)
    {
      int index = 0;
      for (int length = lineResult.Lines.Length; index < length; ++index)
      {
        float num2 = num1 + lineResult.LineHeight;
        if ((double) num2 <= (double) height || (double) height <= 0.0 || flag)
        {
          TextLineInfo info = lineResult.Lines[index];
          numInserted += info.Line.Length;
          info = this.TrimLine(info, lines.Count == 0, font, defaultFont, charFormat, format, textSize);
          lines.Add(info);
          SizeF actualSize = result.ActualSize;
          actualSize.Width = Math.Max(actualSize.Width, info.Width);
          result.ActualSize = actualSize;
          if ((double) num2 >= (double) height && (double) height > 0.0 && flag)
          {
            if (format == null || format.FormatFlags != StringFormatFlags.NoClip)
            {
              float num3 = num2 - height;
              float num4 = lineResult.LineHeight - num3;
              num1 += num4;
            }
            else
              num1 = num2;
            result1 = false;
            break;
          }
          num1 = num2;
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
      result.ActualSize = actualSize;
    }
    return result1;
  }

  private void SaveResult(
    StringSplitResult result,
    List<TextLineInfo> lines,
    StringParser reader,
    Font m_font,
    string m_text)
  {
    if (result == null)
      throw new ArgumentNullException(nameof (result));
    result.Lines = lines != null ? lines.ToArray() : throw new ArgumentNullException(nameof (lines));
    result.LineHeight = this.GetLineHeight(m_font);
    if (!reader.EOF)
    {
      int startIndex = 0;
      if (lines.Count > 0)
        startIndex = lines[0].Line.Length;
      result.Remainder = m_text.Substring(startIndex, m_text.Length - startIndex).TrimStart(StringParser.Spaces);
    }
    lines.Clear();
  }

  private float GetLineHeight(Font m_font) => (float) m_font.Height;

  private StringSplitResult SplitLine(
    string line,
    float lineIndent,
    ref bool isLastWordFit,
    Font font,
    Font defaultFont,
    WCharacterFormat charFormat,
    StringFormat format,
    SizeF size,
    bool isTabStopInterSectingfloattingItem,
    ref bool isTrailSpacesWrapped,
    bool isAutoHyphenated,
    IEntity strWidget)
  {
    List<string> stringList = (List<string>) null;
    IEntity previousSibling = (strWidget as Entity).PreviousSibling;
    bool flag1 = false;
    if (previousSibling is Shape)
      flag1 = (previousSibling as Shape).IsHorizontalRule;
    else if (previousSibling is WPicture && (previousSibling as WPicture).IsShape)
      flag1 = (previousSibling as WPicture).PictureShape.IsHorizontalRule;
    line = line != null ? line.Replace(ControlChar.Tab, "    ") : throw new ArgumentNullException(nameof (line));
    StringSplitResult lineResult = new StringSplitResult();
    lineResult.LineHeight = this.GetLineHeight(font);
    List<TextLineInfo> lines = new List<TextLineInfo>();
    float width = size.Width;
    float lineWidth1 = this.GetLineWidth(line, font, defaultFont, charFormat) + lineIndent;
    TextLineType textLineType = TextLineType.FirstParagraphLine;
    bool flag2 = true;
    bool flag3 = false;
    if (((double) width <= 0.0 || (double) lineWidth1 <= (double) width) && (double) width > 0.0)
    {
      this.AddToLineResult(lineResult, lines, line, lineWidth1, TextLineType.NewLineBreak | textLineType, font);
    }
    else
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      float lineWidth2 = lineIndent;
      StringParser stringParser = new StringParser(line);
      string str = stringParser.PeekWord();
      if (str.Length != stringParser.Length && str == " ")
      {
        ++stringParser.Position;
        str = " " + stringParser.PeekWord();
      }
      while (str != null)
      {
        stringBuilder2.Append(str);
        bool flag4 = false;
        float num1 = this.GetLineWidth(stringBuilder2.ToString(), font, defaultFont, charFormat);
        if ((double) num1 > (double) width)
        {
          if (stringBuilder2.ToString().TrimEnd(ControlChar.SpaceChar) == line.TrimEnd(ControlChar.SpaceChar))
          {
            if ((double) this.GetLineWidth(stringBuilder2.ToString().TrimEnd(ControlChar.SpaceChar), font, defaultFont, charFormat) <= (double) width)
            {
              num1 = width;
              str = string.Empty;
              isTrailSpacesWrapped = true;
            }
          }
        }
        if ((double) num1 > (double) width)
        {
          if (this.GetWrapType(format) != StringTrimming.None)
          {
            if (stringBuilder2.Length == str.Length)
            {
              int length = line.Split((char[]) null).Length;
              if (this.GetWrapType(format) == StringTrimming.Word || length == 1)
              {
                if (!charFormat.OwnerBase.Document.DOP.HyphCapitals && str.ToUpper().Equals(str, StringComparison.Ordinal))
                  isAutoHyphenated = false;
                if (isAutoHyphenated && str.Trim() != "")
                {
                  string languageCode = ((LocaleIDs) charFormat.LocaleIdASCII).ToString().Replace('_', '-');
                  this.GetDictionary(charFormat, languageCode);
                  if (this.hyphen != null)
                  {
                    if (str.ToUpper().Equals(str, StringComparison.Ordinal))
                    {
                      flag4 = true;
                      str = str.ToLower();
                    }
                    List<string> hyphenatedWords = new List<string>((IEnumerable<string>) this.hyphen.HyphenateText(str).Split('='));
                    if (str.ToLower().Equals(str, StringComparison.Ordinal) && flag4)
                      str = str.ToUpper();
                    string fittedWord = "";
                    string remainingWord = "";
                    this.SplitAsPerAutoHyphenation(width, hyphenatedWords, ref fittedWord, ref remainingWord, font, defaultFont, charFormat);
                    hyphenatedWords.Clear();
                    stringList = (List<string>) null;
                    if (fittedWord != "")
                    {
                      stringBuilder1.Append(fittedWord + (object) '-');
                      lineResult.Remainder = remainingWord;
                      flag3 = true;
                      break;
                    }
                  }
                  else
                  {
                    lineResult.Remainder = line.Substring(stringParser.Position);
                    if (str.StartsWith(" "))
                    {
                      stringBuilder1.Append(line.Substring(0, stringParser.Position));
                      break;
                    }
                    break;
                  }
                }
                lineResult.Remainder = line.Substring(stringParser.Position);
                if (str.StartsWithExt(" "))
                {
                  stringBuilder1.Append(line.Substring(0, stringParser.Position));
                  break;
                }
                break;
              }
              flag2 = false;
              stringBuilder2.Length = 0;
              str = stringParser.Peek().ToString();
            }
            else
            {
              if (this.GetWrapType(format) != StringTrimming.Character || !flag2)
              {
                if (!charFormat.OwnerBase.Document.DOP.HyphCapitals && str.ToUpper().Equals(str, StringComparison.Ordinal))
                  isAutoHyphenated = false;
                if (isAutoHyphenated && str.Trim() != "")
                {
                  string languageCode = ((LocaleIDs) charFormat.LocaleIdASCII).ToString().Replace('_', '-');
                  this.GetDictionary(charFormat, languageCode);
                  if (this.hyphen != null)
                  {
                    if (str.ToUpper().Equals(str, StringComparison.Ordinal))
                    {
                      flag4 = true;
                      str = str.ToLower();
                    }
                    List<string> hyphenatedWords = new List<string>((IEnumerable<string>) this.hyphen.HyphenateText(str).Split('='));
                    if (str.ToLower().Equals(str, StringComparison.Ordinal) && flag4)
                      str = str.ToUpper();
                    float lineWidth3 = this.GetLineWidth(stringBuilder2.ToString().Substring(0, stringBuilder2.ToString().IndexOf(str)), font, defaultFont, charFormat);
                    float maxWidth = width - lineWidth3;
                    string fittedWord = "";
                    string remainingWord = "";
                    this.SplitAsPerAutoHyphenation(maxWidth, hyphenatedWords, ref fittedWord, ref remainingWord, font, defaultFont, charFormat);
                    if (fittedWord != "")
                    {
                      stringBuilder1.Append(fittedWord + (object) '-');
                      lineResult.Remainder = remainingWord;
                      flag3 = true;
                      break;
                    }
                    break;
                  }
                  break;
                }
                break;
              }
              flag2 = false;
              stringBuilder2.Length = 0;
              stringBuilder2.Append(stringBuilder1.ToString());
              str = stringParser.Peek().ToString();
            }
          }
          else
            break;
        }
        else
        {
          stringBuilder1.Append(str);
          lineWidth2 = num1;
          if (flag2)
          {
            stringParser.ReadWord();
            str = stringParser.PeekWord();
          }
          else
          {
            int num2 = (int) stringParser.Read();
            str = stringParser.Peek().ToString();
          }
        }
      }
      if (stringBuilder1.Length <= 0)
      {
        if ((!(stringBuilder2.ToString().TrimEnd(ControlChar.SpaceChar) == string.Empty) || flag1) && !isTabStopInterSectingfloattingItem)
          goto label_52;
      }
      string line1 = stringBuilder1.ToString();
      this.AddToLineResult(lineResult, lines, line1, lineWidth2, TextLineType.NewLineBreak | TextLineType.LastParagraphLine, font);
      lineResult.Remainder = flag3 ? lineResult.Remainder : stringParser.ReadToEnd();
label_52:
      stringParser.Close();
    }
    lineResult.Lines = lines.ToArray();
    lines.Clear();
    return lineResult;
  }

  private void GetDictionary(WCharacterFormat charFormat, string languageCode)
  {
    if (Hyphenator.LoadedHyphenators.ContainsKey(languageCode))
      this.hyphen = Hyphenator.LoadedHyphenators[languageCode];
    else if (Hyphenator.Dictionaries.ContainsKey(languageCode))
    {
      Stream dictionary = Hyphenator.Dictionaries[languageCode];
      this.hyphen = dictionary != null ? new Hyphenator(dictionary) : (Hyphenator) null;
      Hyphenator.LoadedHyphenators.Add(languageCode, this.hyphen);
    }
    else
    {
      languageCode = charFormat.Document.Hyphenator.GetAlternateForMissedLanguageCode(languageCode);
      if (Hyphenator.LoadedHyphenators.ContainsKey(languageCode))
      {
        this.hyphen = Hyphenator.LoadedHyphenators[languageCode];
      }
      else
      {
        if (!Hyphenator.Dictionaries.ContainsKey(languageCode))
          return;
        Stream dictionary = Hyphenator.Dictionaries[languageCode];
        this.hyphen = dictionary != null ? new Hyphenator(dictionary) : (Hyphenator) null;
        Hyphenator.LoadedHyphenators.Add(languageCode, this.hyphen);
      }
    }
  }

  private void SplitAsPerAutoHyphenation(
    float maxWidth,
    List<string> hyphenatedWords,
    ref string fittedWord,
    ref string remainingWord,
    Font font,
    Font defaultFont,
    WCharacterFormat charFormat)
  {
    int index1 = 0;
    for (float lineWidth = this.GetLineWidth(hyphenatedWords[index1] + (object) '-', font, defaultFont, charFormat); (double) lineWidth <= (double) maxWidth && index1 + 1 < hyphenatedWords.Count; lineWidth = this.GetLineWidth(fittedWord + hyphenatedWords[index1] + (object) '-', font, defaultFont, charFormat))
    {
      fittedWord += hyphenatedWords[index1];
      ++index1;
    }
    for (int index2 = index1; index2 < hyphenatedWords.Count; ++index2)
      remainingWord += hyphenatedWords[index2];
  }

  private void AddToLineResult(
    StringSplitResult lineResult,
    List<TextLineInfo> lines,
    string line,
    float lineWidth,
    TextLineType breakType,
    Font font)
  {
    if (lineResult == null)
      throw new ArgumentNullException(nameof (lineResult));
    if (lines == null)
      throw new ArgumentNullException(nameof (lines));
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    lines.Add(new TextLineInfo()
    {
      Line = line,
      Width = lineWidth,
      LineType = breakType,
      Length = line.Length
    });
    SizeF actualSize = lineResult.ActualSize;
    actualSize.Height += this.GetLineHeight(font);
    actualSize.Width = Math.Max(actualSize.Width, lineWidth);
    lineResult.ActualSize = actualSize;
  }

  private TextLineInfo TrimLine(
    TextLineInfo info,
    bool firstLine,
    Font font,
    Font defaultFont,
    WCharacterFormat charFormat,
    StringFormat format,
    SizeF size)
  {
    string str = info.Line;
    float num = info.Width;
    bool flag1 = (info.LineType & TextLineType.FirstParagraphLine) == TextLineType.None;
    bool flag2 = format == null || format.FormatFlags != StringFormatFlags.DirectionRightToLeft;
    char[] spaces = StringParser.Spaces;
    if (flag1)
      str = flag2 ? str.TrimStart(spaces) : str.TrimEnd(spaces);
    if (format == null || format.FormatFlags != StringFormatFlags.MeasureTrailingSpaces)
      str = (info.LineType & TextLineType.FirstParagraphLine) <= TextLineType.None || !StringParser.IsWhitespace(str) ? (flag2 ? str.TrimEnd(spaces) : str.TrimStart(spaces)) : new string(' ', 1);
    if (str.Length != info.Line.Length)
    {
      num = this.GetLineWidth(str, font, defaultFont, charFormat);
      if ((info.LineType & TextLineType.FirstParagraphLine) > TextLineType.None)
        num += this.GetLineIndent(firstLine, format, size);
    }
    info.Line = str;
    info.Width = num;
    return info;
  }

  private float GetLineWidth(
    string line,
    Font font,
    Font defaultFont,
    WCharacterFormat charFormat)
  {
    return defaultFont.Name != font.Name && this.m_dc.IsUnicodeText(line) ? this.m_dc.MeasureString(line, font, defaultFont, (StringFormat) null, charFormat).Width : this.m_dc.MeasureString(line, font, (StringFormat) null, charFormat, false).Width;
  }

  private float GetLineIndent(bool firstLine, StringFormat format, SizeF size)
  {
    float val2 = 0.0f;
    if (format != null)
      val2 = (double) size.Width > 0.0 ? Math.Min(size.Width, val2) : val2;
    return val2;
  }

  private StringTrimming GetWrapType(StringFormat format)
  {
    return format != null ? format.Trimming : StringTrimming.Word;
  }
}
