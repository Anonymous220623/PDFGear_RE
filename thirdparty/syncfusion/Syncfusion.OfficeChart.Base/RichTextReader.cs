// Decompiled with JetBrains decompiler
// Type: RichTextReader
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart;
using Syncfusion.OfficeChart.Implementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;

#nullable disable
internal class RichTextReader
{
  private const string FontCharSet = "charset";
  internal const string Seperator = ";";
  internal const string ControlStart = "\\";
  internal const string ParaStart = "pard";
  internal const string FontIndex = "f";
  internal const string FontSize = "fs";
  internal const string Bold = "b";
  internal const string Italic = "i";
  internal const string Tab = "tab";
  internal const string ForegroundColor = "cf";
  internal const string Para = "par\r\n";
  internal const string Para2 = "par\n";
  internal const string Para3 = "par";
  internal const string UnderLine = "ul";
  internal const string ItalicsUnderline = "iul";
  internal const string StopUnderLine = "ulnone";
  internal const string Strike = "strike";
  internal const string Subscript = "sub";
  internal const string NoSuperSub = "nosupersub";
  internal const string Superscript = "super";
  internal const string DestinationMark = "*";
  internal const string Red = "red";
  internal const string Green = "green";
  internal const string Blue = "blue";
  internal const string ParagraphCenter = "qc";
  internal const string ParagraphJustify = "qj";
  internal const string ParagraphLeft = "ql";
  internal const string ParagraphRight = "qr";
  internal const string Language = "lang";
  internal const string openBraces = "{";
  internal const string closeBraces = "}";
  internal const string singleQuote = "'";
  private const string unicode = "u";
  private string m_defaultCodePage;
  private IApplication m_application;
  private Dictionary<int, Color> m_colorDict = new Dictionary<int, Color>();
  private string[] m_complete;
  private Dictionary<int, IOfficeFont> m_fontDict = new Dictionary<int, IOfficeFont>();
  private int m_index;
  private WorkbookImpl m_book;
  private int m_iFontIndex;
  private string m_rtfText;
  private WorksheetImpl m_sheet;
  private IRange m_range;
  private RichTextString m_rtf;
  private int currentStrIndex;

  public RichTextReader(IWorksheet parentSheet)
  {
    this.m_sheet = parentSheet as WorksheetImpl;
    this.m_book = parentSheet.Workbook as WorkbookImpl;
    this.m_application = (parentSheet as WorksheetImpl).Application;
  }

  private Color FindColor(string findColor)
  {
    string[] strArray = findColor.Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    return strArray.Length == 3 ? Color.FromArgb((int) byte.MaxValue, (int) byte.Parse(strArray[0].TrimStart("red".ToCharArray())), (int) byte.Parse(strArray[1].TrimStart("green".ToCharArray())), (int) byte.Parse(strArray[2].TrimStart("blue".ToCharArray()))) : Color.Empty;
  }

  private void Parse()
  {
    this.m_complete = this.m_rtfText.Split(new char[2]
    {
      '{',
      '}'
    }, StringSplitOptions.RemoveEmptyEntries);
    this.m_index = 0;
    while (this.m_index < this.m_complete.Length)
    {
      this.m_complete[this.m_index].Split(new char[1]{ ' ' }, 2);
      ++this.m_index;
      this.ParseFontTable();
      this.ParseColorTable();
      this.ParseContent();
    }
    if (this.m_range != null && this.m_range.WrapText)
      this.m_range.AutofitColumns();
    this.m_rtf.TextObject.RtfText = this.m_rtfText;
  }

  private void ParseColorTable()
  {
    while (!this.m_complete[this.m_index].Contains("\\par") && !("{" + this.m_complete[this.m_index]).StartsWith(RtfTextWriter.DEF_TAGS[2]))
      ++this.m_index;
    this.m_colorDict.Add(0, Color.Black);
    int count = this.m_colorDict.Count;
    string str = "{" + this.m_complete[this.m_index];
    if (!str.StartsWith(RtfTextWriter.DEF_TAGS[2]))
      return;
    string[] strArray = str.Split(";".ToCharArray());
    for (int index = 1; index < strArray.Length; ++index)
    {
      Color color = this.FindColor(strArray[index]);
      if (color != Color.Empty)
      {
        this.m_colorDict.Add(count, color);
        ++count;
      }
    }
    ++this.m_index;
  }

  private void ParseContent()
  {
    if (this.m_complete[this.m_index].Contains("\\*"))
      ++this.m_index;
    this.ParseControlWord();
  }

  private void ParseControlWord()
  {
    int currentIndex = 0;
    Color color = Color.Empty;
    IOfficeFont font1 = this.m_fontDict[0];
    IOfficeFont baseFont = font1;
    IOfficeFont font2 = baseFont;
    while (this.m_index < this.m_complete.Length)
    {
      bool flag = false;
      string[] rtfArray = this.m_complete[this.m_index].Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
      if (currentIndex < rtfArray.Length)
      {
        string[] rtfString = rtfArray[currentIndex].Split(new char[1]
        {
          ' '
        }, 2);
        int number = this.ParseNumber(rtfString[0]);
        string str1 = number != int.MinValue ? rtfString[0].Split(number.ToString().ToCharArray(), 2)[0] : rtfString[0];
        int length = this.m_rtf.Text != null ? this.m_rtf.Text.Length : 0;
        if ((rtfString[0].Contains(Environment.NewLine) || rtfString[0].Contains("\n")) && (!rtfString[0].EndsWith(Environment.NewLine) || !rtfString[0].EndsWith("\n")))
        {
          string[] strArray = rtfArray[currentIndex].Split(new string[2]
          {
            Environment.NewLine,
            "\n"
          }, 2, StringSplitOptions.RemoveEmptyEntries);
          if (strArray.Length > 1)
          {
            if (baseFont != font1)
              font1 = this.m_book.CreateFont(baseFont);
            this.AppendRTF('\n'.ToString() + strArray[1]);
            flag = true;
          }
          else if (strArray.Length > 0 && !strArray[0].Contains("par"))
            this.AppendRTF(strArray[0]);
        }
        else if (rtfString.Length > 1 && !str1.StartsWith("'") && !rtfString[0].StartsWith("tab") && !rtfString[1].StartsWith(" ") && !rtfString[0].StartsWith("par\r\n") && !rtfString[0].StartsWith("par\n") && !rtfString[0].StartsWith("par") && !rtfString[0].StartsWith("qc") && !rtfString[0].StartsWith("qj") && !rtfString[0].StartsWith("ql") && !rtfString[0].StartsWith("qr") && !rtfString[0].StartsWith("b"))
        {
          this.AppendRTF(rtfString[1]);
          flag = true;
        }
        int iEndPos = this.m_rtf.Text != null ? this.m_rtf.Text.Length - 1 : 0;
        int num = currentIndex;
        switch (str1)
        {
          case "pard":
            font1 = this.m_book.CreateFont(baseFont);
            (font1 as FontImpl).ParaAlign = Excel2007CommentHAlign.l;
            if (rtfString.Length > 1)
            {
              this.m_rtf.SetText(this.m_rtf.Text + rtfString[1]);
              iEndPos = this.m_rtf.Text != null ? this.m_rtf.Text.Length - 1 : 0;
            }
            baseFont = font1;
            break;
          case "qc":
            font1 = this.m_book.CreateFont(baseFont);
            (font1 as FontImpl).ParaAlign = Excel2007CommentHAlign.ctr;
            (font1 as FontImpl).HasParagrapAlign = true;
            if (rtfString.Length > 1)
            {
              this.m_rtf.SetText(this.m_rtf.Text + rtfString[1]);
              iEndPos = this.m_rtf.Text != null ? this.m_rtf.Text.Length - 1 : 0;
            }
            baseFont = font1;
            break;
          case "qj":
          case "ql":
            font1 = this.m_book.CreateFont(baseFont);
            (font1 as FontImpl).ParaAlign = Excel2007CommentHAlign.l;
            (font1 as FontImpl).HasParagrapAlign = true;
            if (rtfString.Length > 1)
            {
              this.m_rtf.SetText(this.m_rtf.Text + rtfString[1]);
              iEndPos = this.m_rtf.Text != null ? this.m_rtf.Text.Length - 1 : 0;
            }
            baseFont = font1;
            break;
          case "qr":
            font1 = this.m_book.CreateFont(baseFont);
            (font1 as FontImpl).ParaAlign = Excel2007CommentHAlign.r;
            (font1 as FontImpl).HasParagrapAlign = true;
            if (rtfString.Length > 1)
            {
              this.m_rtf.SetText(this.m_rtf.Text + rtfString[1]);
              iEndPos = this.m_rtf.Text != null ? this.m_rtf.Text.Length - 1 : 0;
            }
            baseFont = font1;
            break;
          case "f":
            font1 = this.m_book.CreateFont(baseFont);
            font1.FontName = this.m_fontDict[number].FontName;
            font1.RGBColor = color;
            if (rtfString.Length > 1 && !flag)
            {
              this.m_rtf.SetText(this.m_rtf.Text + rtfString[1]);
              iEndPos = this.m_rtf.Text != null ? this.m_rtf.Text.Length - 1 : 0;
            }
            baseFont = font1;
            if (rtfString.Length > 1 && !flag)
            {
              rtfString[1] = rtfString[1].Replace("'7B", "{");
              rtfString[1] = rtfString[1].Replace("'7D", "}");
              this.AppendRTF(rtfString[1]);
              iEndPos = this.m_rtf.Text != null ? this.m_rtf.Text.Length - 1 : 0;
              break;
            }
            break;
          case "fs":
            font1 = this.m_book.CreateFont(baseFont);
            if (rtfString.Length > 1 && !flag)
            {
              this.m_rtf.SetText(this.m_rtf.Text + rtfString[1]);
              iEndPos = this.m_rtf.Text != null ? this.m_rtf.Text.Length - 1 : 0;
            }
            font1.Size = (double) number / 2.0;
            baseFont = font1;
            break;
          case "tab":
            this.AppendRTF("\t");
            if (rtfString.Length > 1)
            {
              this.AppendRTF(rtfString[1].Replace(Environment.NewLine, ""));
              break;
            }
            break;
          case "cf":
            font1 = this.m_book.CreateFont(baseFont);
            font1.RGBColor = this.m_colorDict[number];
            color = font1.RGBColor;
            baseFont = font1;
            break;
          case "par\r\n":
          case "par\n":
            if (!flag)
            {
              font1 = this.m_book.CreateFont(baseFont);
              string[] strArray = rtfString[0].Split(str1.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
              string empty = string.Empty;
              this.m_rtf.SetText((currentIndex == rtfArray.Length - 1 ? this.m_rtf.Text : this.m_rtf.Text + (object) '\n') + (strArray.Length > 0 ? strArray[0] : ""));
              if (rtfString.Length > 1)
                this.AppendRTF(" " + rtfString[1]);
              if (this.m_range != null && !this.m_range.WrapText)
              {
                this.m_range.WrapText = true;
                break;
              }
              break;
            }
            break;
          case "ul":
            font1 = this.m_book.CreateFont(baseFont);
            if (rtfString.Length > 1 && rtfString[1].StartsWith(" "))
            {
              this.AppendRTF(rtfString[1]);
              iEndPos = this.m_rtf.Text != null ? this.m_rtf.Text.Length - 1 : 0;
            }
            font1.Underline = OfficeUnderline.Single;
            baseFont = font1;
            break;
          case "ulnone":
            font1 = this.m_book.CreateFont(baseFont);
            font1.Underline = OfficeUnderline.None;
            baseFont = font1;
            break;
          case "strike":
            font1 = this.m_book.CreateFont(baseFont);
            font1.Strikethrough = !font1.Strikethrough;
            baseFont = font1;
            break;
          case "sub":
            font1 = this.m_book.CreateFont(baseFont);
            font1.Subscript = !font1.Subscript;
            baseFont = font1;
            break;
          case "nosupersub":
            font1 = this.m_book.CreateFont(baseFont);
            font1.Subscript = !font1.Subscript && font1.Subscript;
            font1.Superscript = !font1.Superscript && font1.Superscript;
            baseFont = font1;
            break;
          case "super":
            font1 = this.m_book.CreateFont(baseFont);
            font1.Superscript = !font1.Superscript;
            baseFont = font1;
            break;
          case "*":
            ++currentIndex;
            break;
          case "b":
            font1 = this.m_book.CreateFont(baseFont);
            if (rtfString.Length > 1)
            {
              this.AppendRTF(rtfString[1]);
              iEndPos = this.m_rtf.Text != null ? this.m_rtf.Text.Length - 1 : 0;
            }
            font1.Bold = number != 0;
            baseFont = font1;
            break;
          case "i":
          case "iul":
            font1 = this.m_book.CreateFont(baseFont);
            if (rtfString.Length > 1 && rtfString[1].StartsWith(" "))
            {
              this.AppendRTF(rtfString[1]);
              iEndPos = this.m_rtf.Text != null ? this.m_rtf.Text.Length - 1 : 0;
            }
            font1.Italic = number != 0;
            if (str1 == "iul")
              font1.Underline = OfficeUnderline.Single;
            baseFont = font1;
            break;
          case "lang":
            string str2 = rtfString[0];
            break;
          case "'":
            if (rtfString[0].Length == 3)
            {
              num = this.SetExtendedCharacter(rtfString, font2 as FontWrapper, rtfArray, currentIndex);
              break;
            }
            rtfArray[currentIndex] = rtfArray[currentIndex].Replace("'7B", "{");
            rtfArray[currentIndex] = rtfArray[currentIndex].Replace("'7D", "}");
            num = this.SetExtendedCharacter(rtfString, font2 as FontWrapper, rtfArray, currentIndex);
            iEndPos = this.m_rtf.Text != null ? this.m_rtf.Text.Length - 1 : 0;
            break;
          case "u":
            string str3 = rtfArray[currentIndex].Replace("u", "").Replace("?", "");
            if (str3.Contains(" "))
            {
              this.AppendRTF(((char) Convert.ToInt32(str3)).ToString() + " ");
              break;
            }
            this.AppendRTF(((char) Convert.ToInt32(str3)).ToString());
            break;
          default:
            if (baseFont != font1)
              font1 = this.m_book.CreateFont(baseFont);
            if (str1.StartsWith("'"))
            {
              if (rtfString[0].Length == 3)
              {
                num = this.SetExtendedCharacter(rtfString, font2 as FontWrapper, rtfArray, currentIndex);
                break;
              }
              rtfString[0] = rtfString[0].Replace("'7B", "{");
              rtfString[0] = rtfString[0].Replace("'7D", "}");
              num = this.SetExtendedCharacter(rtfString, font2 as FontWrapper, rtfArray, currentIndex);
              iEndPos = this.m_rtf.Text != null ? this.m_rtf.Text.Length - 1 : 0;
              break;
            }
            break;
        }
        if (rtfString.Length > 1 && length <= iEndPos)
          this.m_rtf.SetFont(length, iEndPos, font1);
        else if (rtfString.Length == 1 && length > iEndPos && iEndPos >= 0)
          this.m_rtf.SetFont(length, length, font1);
        if (num != currentIndex && num != 0)
          currentIndex = num + 1;
        else
          ++currentIndex;
      }
      else
      {
        if (rtfArray.Length == 0)
        {
          string str = this.m_complete[this.m_index];
          this.AppendRTF($"{{{str.Substring(2, str.Length - 2)}}}");
        }
        ++this.m_index;
        currentIndex = 0;
      }
    }
  }

  private void AppendRTF(string rtfString)
  {
    rtfString = rtfString.Replace("'..", "_");
    this.m_rtf.SetText(this.m_rtf.Text + rtfString);
  }

  private int SetExtendedCharacter(
    string[] rtfString,
    FontWrapper font,
    string[] rtfArray,
    int currentIndex)
  {
    string empty = string.Empty;
    string str1 = string.Empty;
    int index = 0;
    this.currentStrIndex = currentIndex;
    while (index < rtfString.Length)
    {
      string str2;
      if (rtfString[index] == string.Empty)
      {
        str2 = " " + rtfString[index++];
      }
      else
      {
        if (rtfString[index].Length > 3)
        {
          str1 = rtfString[index].Substring(3);
          rtfString[index] = rtfString[index].Substring(0, 3);
        }
        str2 = this.GetExtendedCharacter(rtfString[index++], font, rtfArray);
        if (str1 != string.Empty)
          str2 += str1;
        str1 = string.Empty;
      }
      this.m_rtf.SetText(this.m_rtf.Text + str2);
    }
    return this.currentStrIndex;
  }

  private void ParseFontTable()
  {
    ++this.m_index;
    int key = this.m_fontDict.Count > 0 ? this.m_fontDict.Count + 1 : 0;
    for (; this.m_complete[this.m_index].StartsWith("\\f") || this.m_complete[this.m_index].StartsWith("\\*"); ++this.m_index)
    {
      IOfficeFont font = this.m_book.CreateFont();
      string[] strArray1 = this.m_complete[this.m_index].Split(new string[1]
      {
        "\\"
      }, StringSplitOptions.RemoveEmptyEntries);
      string[] strArray2 = strArray1[strArray1.Length - 1].Split(' ');
      if (strArray2[0].Contains("charset"))
      {
        if (strArray2[0].StartsWith("af"))
        {
          strArray2[0] = strArray2[0].Replace("afcharset", " ");
          (font as FontWrapper).CharSet = (int) Convert.ToInt16(strArray2[0]);
        }
        else if (strArray2[0].StartsWith("f"))
        {
          strArray2[0] = strArray2[0].Replace("fcharset", " ");
          (font as FontWrapper).CharSet = (int) Convert.ToInt16(strArray2[0]);
        }
      }
      string str = this.m_complete[this.m_index];
      int startIndex = str.IndexOf(' ') + 1;
      int length = str.LastIndexOf(";"[0]) - startIndex;
      if (length != -1)
        font.FontName = str.Substring(startIndex, length);
      this.m_fontDict.Add(key, font);
      ++key;
    }
    if (key != 0)
      return;
    this.m_fontDict.Add(this.m_iFontIndex, (IOfficeFont) this.DefaultFont);
  }

  private int ParseNumber(string numText)
  {
    string[] strArray = numText.Split("\\".ToCharArray());
    string s = "";
    if (strArray.Length > 0)
    {
      foreach (char ch in strArray[0].ToCharArray())
      {
        int result = 0;
        if (int.TryParse(ch.ToString(), out result))
          s += result.ToString();
      }
    }
    return s != null && s != string.Empty ? int.Parse(s) : int.MinValue;
  }

  public void SetRTF(int row, int column, string text)
  {
    this.m_rtfText = text != null ? text : throw new ArgumentNullException("RTF Text");
    this.m_range = this.m_sheet[row, column];
    this.m_rtf = this.m_range.RichText as RichTextString;
    this.Parse();
  }

  public void SetRTF(object shape, string text)
  {
    this.m_rtfText = text != null ? text : throw new ArgumentNullException("RTF Text");
    this.m_rtf = this.CreateRichTextString() as RichTextString;
    this.Parse();
    switch (shape.GetType().Name)
    {
      case "TextBoxShapeImpl":
        (shape as ITextBoxShape).RichText = (IRichTextString) this.m_rtf;
        break;
    }
  }

  protected IRichTextString CreateRichTextString()
  {
    return (IRichTextString) new RangeRichTextString(this.m_sheet.Application, (object) this.m_sheet, -1, -1);
  }

  private string GetExtendedCharacter(string token, FontWrapper font, string[] rtfArray)
  {
    int num1 = 0;
    string extendedCharacter = "";
    int length = token.Length;
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string s = (string) null;
    Encoding.GetEncoding(this.GetCodePage(font.CharSet));
    if (token.Length >= 3)
      s = token.Substring(1, 2);
    if (!token.StartsWith("'"))
      extendedCharacter = " " + token;
    else if (token == "'.." || token == "'.")
      extendedCharacter = "_";
    else if (s != "3f")
    {
      if (!this.IsSingleByte(font.CharSet))
      {
        if (this.currentStrIndex != rtfArray.Length - 1)
          ++this.currentStrIndex;
        string rtf = rtfArray[this.currentStrIndex];
        string[] strArray = this.SeperateToken(rtf);
        string str1 = strArray[0];
        string str2 = strArray[1];
        s = rtf.Trim().Substring(1) + s;
        num1 = int.Parse(s, NumberStyles.HexNumber);
      }
      int num2 = int.Parse(s, NumberStyles.HexNumber);
      Encoding encoding = (Encoding) new Windows1252Encoding();
      byte[] bytes = BitConverter.GetBytes((short) num2);
      extendedCharacter = encoding.GetString(bytes, 0, bytes.Length).Replace("\0", "");
    }
    if (length > 3)
      extendedCharacter += token.Substring(3, length - 3);
    return extendedCharacter;
  }

  private string[] SeperateToken(string token)
  {
    string[] strArray1 = new string[2];
    for (int index = 0; index < token.Length; ++index)
    {
      char c = token[index];
      if (char.IsDigit(c))
      {
        string[] strArray2;
        (strArray2 = strArray1)[1] = strArray2[1] + (object) c;
      }
      else
      {
        string[] strArray3;
        (strArray3 = strArray1)[0] = strArray3[0] + (object) c;
      }
    }
    return strArray1;
  }

  private bool IsSingleByte(int charset)
  {
    switch (charset)
    {
      case 78:
      case 79:
      case 80 /*0x50*/:
      case 81:
      case 82:
      case 128 /*0x80*/:
      case 129:
      case 130:
      case 134:
      case 136:
        return false;
      default:
        return true;
    }
  }

  private string GetCodePage(int fontCharSet)
  {
    switch (fontCharSet)
    {
      case 0:
      case 1:
        return "Windows-1252";
      case 77:
        return "macintosh";
      case 78:
        return "x-mac-japanese";
      case 79:
        return "x-mac-korean";
      case 80 /*0x50*/:
        return "x-mac-chinesesimp";
      case 81:
      case 82:
        return "x-mac-chinesetrad";
      case 83:
        return "x-mac-hebrew";
      case 84:
        return "x-mac-arabic";
      case 85:
        return "x-mac-greek";
      case 86:
        return "x-mac-turkish";
      case 87:
        return "x-mac-thai";
      case 88:
        return "x-mac-ce";
      case 89:
        return "x-mac-cyrillic";
      case 128 /*0x80*/:
        return "shift_jis";
      case 129:
        return "ks_c_5601-1987";
      case 130:
        return "Johab";
      case 134:
        return "gb2312";
      case 136:
        return "big5";
      case 161:
        return "windows-1253";
      case 162:
        return "windows-1254";
      case 163:
        return "windows-1258";
      case 177:
        return "windows-1255";
      case 178:
      case 179:
      case 180:
      case 181:
        return "windows-1256";
      case 186:
        return "windows-1257";
      case 204:
        return "windows-1251";
      case 222:
        return "windows-874";
      case 238:
        return "windows-1250";
      case 254:
        return "IBM437";
      case (int) byte.MaxValue:
        return "ibm850";
      default:
        return this.DefaultCodePage;
    }
  }

  internal string DefaultCodePage
  {
    get
    {
      if (this.m_defaultCodePage != null)
        return this.m_defaultCodePage;
      return this.IsSupportedCodePage(CultureInfo.CurrentCulture.TextInfo.ANSICodePage) ? this.GetSupportedCodePage(CultureInfo.CurrentCulture.TextInfo.ANSICodePage) : "windows-1252";
    }
    set => this.m_defaultCodePage = value;
  }

  private bool IsSupportedCodePage(int codePage)
  {
    switch (codePage)
    {
      case 37:
      case 437:
      case 500:
      case 708:
      case 720:
      case 737:
      case 775:
      case 850:
      case 852:
      case 855:
      case 857:
      case 858:
      case 860:
      case 861:
      case 862:
      case 863:
      case 864:
      case 865:
      case 866:
      case 869:
      case 870:
      case 874:
      case 875:
      case 932:
      case 936:
      case 949:
      case 950:
      case 1026:
      case 1029:
      case 1047:
      case 1140:
      case 1141:
      case 1142:
      case 1143:
      case 1144:
      case 1145:
      case 1146:
      case 1147:
      case 1148:
      case 1149:
      case 1200:
      case 1201:
      case 1250:
      case 1251:
      case 1252:
      case 1253:
      case 1254:
      case 1255:
      case 1256:
      case 1257:
      case 1258:
      case 1361:
      case 10000:
      case 10001:
      case 10002:
      case 10003:
      case 10004:
      case 10005:
      case 10006:
      case 10007:
      case 10008:
      case 10010:
      case 10017:
      case 10021:
      case 10029:
      case 10079:
      case 10081:
      case 10082:
      case 12000:
      case 12001:
      case 20000:
      case 20001:
      case 20002:
      case 20003:
      case 20004:
      case 20005:
      case 20105:
      case 20106:
      case 20107:
      case 20108:
      case 20127:
      case 20261:
      case 20269:
      case 20273:
      case 20277:
      case 20278:
      case 20280:
      case 20284:
      case 20285:
      case 20290:
      case 20297:
      case 20420:
      case 20423:
      case 20424:
      case 20833:
      case 20838:
      case 20866:
      case 20871:
      case 20880:
      case 20905:
      case 20924:
      case 20932:
      case 20936:
      case 20949:
      case 21025:
      case 21866:
      case 28591:
      case 28592:
      case 28593:
      case 28594:
      case 28595:
      case 28596:
      case 28597:
      case 28598:
      case 28599:
      case 28603:
      case 28605:
      case 29001:
      case 38598:
      case 50220:
      case 50221:
      case 50222:
      case 50225:
      case 50227:
      case 51932:
      case 51936:
      case 51949:
      case 52936:
      case 54936:
      case 57002:
      case 57003:
      case 57004:
      case 57005:
      case 57006:
      case 57007:
      case 57008:
      case 57009:
      case 57010:
      case 57011:
      case 65000:
      case 65001:
        return true;
      default:
        return false;
    }
  }

  private string GetSupportedCodePage(int codePage)
  {
    switch (codePage)
    {
      case 37:
        return "IBM037";
      case 437:
        return "IBM437";
      case 500:
        return "IBM500";
      case 708:
        return "ASMO-708";
      case 720:
        return "DOS-720";
      case 737:
        return "ibm737";
      case 775:
        return "ibm775";
      case 850:
        return "ibm850";
      case 852:
        return "ibm852";
      case 855:
        return "IBM855";
      case 857:
        return "ibm857";
      case 858:
        return "IBM00858";
      case 860:
        return "IBM860";
      case 861:
        return "ibm861";
      case 862:
        return "DOS-862";
      case 863:
        return "IBM863";
      case 864:
        return "IBM864";
      case 865:
        return "IBM865";
      case 866:
        return "cp866";
      case 869:
        return "ibm869";
      case 870:
        return "IBM870";
      case 874:
        return "windows-874";
      case 875:
        return "cp875";
      case 932:
        return "shift_jis";
      case 936:
        return "gb2312";
      case 949:
        return "ks_c_5601-1987";
      case 950:
        return "big5";
      case 1026:
        return "IBM1026";
      case 1047:
        return "IBM01047";
      case 1140:
        return "IBM01140";
      case 1141:
        return "IBM01141";
      case 1142:
        return "IBM01142";
      case 1143:
        return "IBM01143";
      case 1144:
        return "IBM01144";
      case 1145:
        return "IBM01145";
      case 1146:
        return "IBM01146";
      case 1147:
        return "IBM01147";
      case 1148:
        return "IBM01148";
      case 1149:
        return "IBM01149";
      case 1200:
        return "utf-16";
      case 1201:
        return "unicodeFFFE";
      case 1250:
        return "windows-1250";
      case 1251:
        return "windows-1251";
      case 1252:
        return "windows-1252";
      case 1253:
        return "windows-1253";
      case 1254:
        return "windows-1254";
      case 1255:
        return "windows-1255";
      case 1256:
        return "windows-1256";
      case 1257:
        return "windows-1257";
      case 1258:
        return "windows-1258";
      case 1361:
        return "Johab";
      case 10000:
        return "macintosh";
      case 10001:
        return "x-mac-japanese";
      case 10002:
        return "x-mac-chinesetrad";
      case 10003:
        return "x-mac-korean";
      case 10004:
        return "x-mac-arabic";
      case 10005:
        return "x-mac-hebrew";
      case 10006:
        return "x-mac-greek";
      case 10007:
        return "x-mac-cyrillic";
      case 10008:
        return "x-mac-chinesesimp";
      case 10010:
        return "x-mac-romanian";
      case 10017:
        return "x-mac-ukrainian";
      case 10021:
        return "x-mac-thai";
      case 10029:
        return "x-mac-ce";
      case 10079:
        return "x-mac-icelandic";
      case 10081:
        return "x-mac-turkish";
      case 10082:
        return "x-mac-croatian";
      case 12000:
        return "utf-32";
      case 12001:
        return "utf-32BE";
      case 20000:
        return "x-Chinese_CNS";
      case 20001:
        return "x-cp20001";
      case 20002:
        return "x_Chinese-Eten";
      case 20003:
        return "x-cp20003";
      case 20004:
        return "x-cp20004";
      case 20005:
        return "x-cp20005";
      case 20105:
        return "x-IA5";
      case 20106:
        return "x-IA5-German";
      case 20107:
        return "x-IA5-Swedish";
      case 20108:
        return "x-IA5-Norwegian";
      case 20127:
        return "us-ascii";
      case 20261:
        return "x-cp20261";
      case 20269:
        return "x-cp20269";
      case 20273:
        return "IBM273";
      case 20277:
        return "IBM277";
      case 20278:
        return "IBM278";
      case 20280:
        return "IBM280";
      case 20284:
        return "IBM284";
      case 20285:
        return "IBM285";
      case 20290:
        return "IBM290";
      case 20297:
        return "IBM297";
      case 20420:
        return "IBM420";
      case 20423:
        return "IBM423";
      case 20424:
        return "IBM424";
      case 20833:
        return "x-EBCDIC-KoreanExtended";
      case 20838:
        return "IBM-Thai";
      case 20866:
        return "koi8-r";
      case 20871:
        return "IBM871";
      case 20880:
        return "IBM880";
      case 20905:
        return "IBM905";
      case 20924:
        return "IBM00924";
      case 20932:
        return "EUC-JP";
      case 20936:
        return "x-cp20936";
      case 20949:
        return "x-cp20949";
      case 21025:
        return "cp1025";
      case 21866:
        return "koi8-u";
      case 28591:
        return "iso-8859-1";
      case 28592:
        return "iso-8859-2";
      case 28593:
        return "iso-8859-3";
      case 28594:
        return "iso-8859-4";
      case 28595:
        return "iso-8859-5";
      case 28596:
        return "iso-8859-6";
      case 28597:
        return "iso-8859-7";
      case 28598:
        return "iso-8859-8";
      case 28599:
        return "iso-8859-9";
      case 28603:
        return "iso-8859-13";
      case 28605:
        return "iso-8859-15";
      case 29001:
        return "x-Europa";
      case 38598:
        return "iso-8859-8-i";
      case 50220:
        return "iso-2022-jp";
      case 50221:
        return "csISO2022JP";
      case 50222:
        return "iso-2022-jp";
      case 50225:
        return "iso-2022-kr";
      case 50227:
        return "x-cp50227";
      case 51932:
        return "euc-jp";
      case 51936:
        return "EUC-CN";
      case 51949:
        return "euc-kr";
      case 52936:
        return "hz-gb-2312";
      case 54936:
        return "GB18030";
      case 57002:
        return "x-iscii-de";
      case 57003:
        return "x-iscii-be";
      case 57004:
        return "x-iscii-ta";
      case 57005:
        return "x-iscii-te";
      case 57006:
        return "x-iscii-as";
      case 57007:
        return "x-iscii-or";
      case 57008:
        return "x-iscii-ka";
      case 57009:
        return "x-iscii-ma";
      case 57010:
        return "x-iscii-gu";
      case 57011:
        return "x-iscii-pa";
      case 65000:
        return "utf-7";
      case 65001:
        return "utf-8";
      default:
        return "Windows-1252";
    }
  }

  protected virtual FontImpl DefaultFont => (FontImpl) this.m_book.InnerFonts[this.m_iFontIndex];
}
