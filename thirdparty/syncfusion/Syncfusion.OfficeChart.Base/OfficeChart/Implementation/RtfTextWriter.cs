// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.RtfTextWriter
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class RtfTextWriter : TextWriter
{
  private const string DEF_FONT = "{{\\f{0}\\fnil\\fcharset{1} {2};}}";
  private const string DEF_FONT_ATTRIBUTE = "\\f{0}\\fs{1}";
  private const string DEF_COLOR_FORMAT = "\\red{0}\\green{1}\\blue{2};";
  private static readonly string[] UnderlineTags = new string[19]
  {
    "\\ul",
    "\\ul0",
    "\\uld",
    "\\uldash",
    "\\uldashd",
    "\\uldashdd",
    "\\uldb",
    "\\ulhwave",
    "\\ulldash",
    "\\ulnone",
    "\\ulth",
    "\\ulthd",
    "\\ulthdash",
    "\\ulthdashd",
    "\\ulthdashdd",
    "\\ulthldash",
    "\\ululdbwave",
    "\\ulw",
    "\\ulwave"
  };
  private static readonly string[] StrikeThroughTags = new string[4]
  {
    "\\strike1",
    "\\strike0",
    "\\striked1",
    "\\striked0"
  };
  internal static readonly string[] DEF_TAGS = new string[18]
  {
    "{\\fonttbl",
    "}",
    "{\\colortbl",
    "}",
    "\\b",
    "\\b0",
    "\\i",
    "\\i0",
    "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033",
    "}",
    "{",
    "}",
    "\\par",
    "\\cf{0}",
    "\\cb{0}",
    "\\sub",
    "\\super",
    "\\nosupersub"
  };
  private List<Color> m_arrColors = new List<Color>();
  private Dictionary<Font, int> m_hashFonts = new Dictionary<Font, int>();
  private Dictionary<Color, int> m_hashColorTable = new Dictionary<Color, int>();
  private bool m_bEnableFormatting;
  private TextWriter m_innerWriter;
  private bool m_bTabsPending;
  private bool m_bEscape;
  private static readonly char[] newLine = "\\line\r\n".ToCharArray();

  public RtfTextWriter()
    : this((TextWriter) new StringWriter(), true)
  {
  }

  public RtfTextWriter(bool enableFormatting)
    : this((TextWriter) new StringWriter(), enableFormatting)
  {
  }

  public RtfTextWriter(TextWriter underlyingWriter)
    : this(underlyingWriter, true)
  {
  }

  public RtfTextWriter(TextWriter underlyingWriter, bool enableFormatting)
  {
    this.m_innerWriter = underlyingWriter;
    this.m_bEnableFormatting = enableFormatting;
  }

  protected virtual void OutputTabs()
  {
    if (!this.m_bTabsPending)
      return;
    this.m_bTabsPending = false;
  }

  protected string GetImageRTF(string rtf)
  {
    int startIndex = rtf.IndexOf("{\\pict");
    int num = rtf.IndexOf("}", startIndex);
    return rtf.Substring(startIndex, num - startIndex + 1);
  }

  private void WriteFontInTable(Font font)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (!this.m_bEnableFormatting)
      return;
    int num = this.m_hashFonts.ContainsKey(font) ? this.m_hashFonts[font] : throw new ApplicationException("Collection does not contain font");
    this.Escape = false;
    this.Escape = true;
  }

  private void WriteFontAttribute(int iFontId, int iFontSize)
  {
    if (!this.m_bEnableFormatting)
      return;
    this.Escape = false;
    this.m_innerWriter.Write($"\\f{iFontId}\\fs{iFontSize * 2}");
    this.Escape = true;
  }

  private void WriteColorInTable(Color value)
  {
    if (!this.m_bEnableFormatting)
      return;
    this.Escape = false;
    this.Write($"\\red{value.R}\\green{value.G}\\blue{value.B};");
    this.Escape = true;
  }

  private void WriteChar(char value)
  {
    if (this.m_bEscape)
    {
      switch (value)
      {
        case '\\':
          this.m_innerWriter.Write('\\');
          this.m_innerWriter.Write('\\');
          break;
        case '{':
          this.m_innerWriter.Write('\\');
          this.m_innerWriter.Write('{');
          break;
        case '}':
          this.m_innerWriter.Write('\\');
          this.m_innerWriter.Write('}');
          break;
        default:
          this.m_innerWriter.Write($"\\u{(object) (int) value}*");
          break;
      }
    }
    else
      this.m_innerWriter.Write(value);
  }

  private void WriteString(string value)
  {
    switch (value)
    {
      case null:
        break;
      case "":
        break;
      default:
        if (this.m_bEscape)
        {
          int index = 0;
          for (int length = value.Length; index < length; ++index)
            this.Write(value[index]);
          break;
        }
        this.m_innerWriter.Write(value);
        break;
    }
  }

  private void WriteImageString(string value)
  {
    switch (value)
    {
      case null:
        break;
      case "":
        break;
      default:
        this.m_innerWriter.Write(value);
        break;
    }
  }

  private void WriteString(string value, string image, string align)
  {
    switch (value)
    {
      case null:
        break;
      case "":
        break;
      default:
        if (this.m_bEscape)
        {
          int index = 0;
          for (int length = value.Length; index < length; ++index)
          {
            if (value[index] == '&' && value[index + 1] == 'G' && image != null)
            {
              this.WriteImageString(this.GetImageRTF(image));
              index += 2;
              if (index == length)
                break;
            }
            this.Write(value[index]);
          }
          break;
        }
        this.m_innerWriter.Write(value);
        break;
    }
  }

  private void WriteNewLine()
  {
    if (this.m_bEscape)
      this.m_innerWriter.Write(RtfTextWriter.newLine);
    else
      this.m_innerWriter.WriteLine();
  }

  private void WriteNewLine(string value)
  {
    if (this.m_bEscape)
    {
      this.Write(value);
      this.m_innerWriter.Write(RtfTextWriter.newLine);
    }
    else
      this.m_innerWriter.WriteLine(value);
  }

  public override string ToString() => this.m_innerWriter.ToString();

  public override void Write(bool value)
  {
    this.OutputTabs();
    this.m_innerWriter.Write(value);
  }

  public override void Write(char value)
  {
    this.OutputTabs();
    this.WriteChar(value);
  }

  public override void Write(char[] buffer)
  {
    this.OutputTabs();
    this.m_innerWriter.Write(buffer);
  }

  public override void Write(double value)
  {
    this.OutputTabs();
    this.m_innerWriter.Write(value);
  }

  public override void Write(int value)
  {
    this.OutputTabs();
    this.m_innerWriter.Write(value);
  }

  public override void Write(long value)
  {
    this.OutputTabs();
    this.m_innerWriter.Write(value);
  }

  public override void Write(object value)
  {
    this.OutputTabs();
    this.m_innerWriter.Write(value);
  }

  public override void Write(float value)
  {
    this.OutputTabs();
    this.m_innerWriter.Write(value);
  }

  public override void Write(string s)
  {
    this.OutputTabs();
    this.WriteString(s);
  }

  internal void Write(string value, string image, string align)
  {
    this.OutputTabs();
    this.WriteString(value, image, align);
  }

  [CLSCompliant(false)]
  public override void Write(uint value)
  {
    this.OutputTabs();
    this.m_innerWriter.Write(value);
  }

  public override void Write(string format, object arg0)
  {
    this.OutputTabs();
    this.m_innerWriter.Write(format, arg0);
  }

  public override void Write(string format, params object[] arg)
  {
    this.OutputTabs();
    this.m_innerWriter.Write(format, arg);
  }

  public override void Write(string format, object arg0, object arg1)
  {
    this.OutputTabs();
    this.m_innerWriter.Write(format, arg0, arg1);
  }

  public override void Write(char[] buffer, int index, int count)
  {
    this.OutputTabs();
    this.m_innerWriter.Write(buffer, index, count);
  }

  public override void WriteLine()
  {
    this.OutputTabs();
    this.WriteNewLine();
    this.m_bTabsPending = true;
  }

  public override void WriteLine(bool value)
  {
    this.OutputTabs();
    this.m_innerWriter.WriteLine(value);
    this.m_bTabsPending = true;
  }

  public override void WriteLine(char value)
  {
    this.OutputTabs();
    this.m_innerWriter.WriteLine(value);
    this.m_bTabsPending = true;
  }

  public override void WriteLine(char[] buffer)
  {
    this.OutputTabs();
    this.m_innerWriter.WriteLine(buffer);
    this.m_bTabsPending = true;
  }

  public override void WriteLine(double value)
  {
    this.OutputTabs();
    this.m_innerWriter.WriteLine(value);
    this.m_bTabsPending = true;
  }

  public override void WriteLine(int value)
  {
    this.OutputTabs();
    this.m_innerWriter.WriteLine(value);
    this.m_bTabsPending = true;
  }

  public override void WriteLine(long value)
  {
    this.OutputTabs();
    this.m_innerWriter.WriteLine(value);
    this.m_bTabsPending = true;
  }

  public override void WriteLine(object value)
  {
    this.OutputTabs();
    this.m_innerWriter.WriteLine(value);
    this.m_bTabsPending = true;
  }

  public override void WriteLine(float value)
  {
    this.OutputTabs();
    this.m_innerWriter.WriteLine(value);
    this.m_bTabsPending = true;
  }

  public override void WriteLine(string s)
  {
    this.OutputTabs();
    this.WriteNewLine(s);
    this.m_bTabsPending = true;
  }

  [CLSCompliant(false)]
  public override void WriteLine(uint value)
  {
    this.OutputTabs();
    this.m_innerWriter.WriteLine(value);
    this.m_bTabsPending = true;
  }

  public override void WriteLine(string format, params object[] arg)
  {
    this.OutputTabs();
    this.m_innerWriter.WriteLine(format, arg);
    this.m_bTabsPending = true;
  }

  public override void WriteLine(string format, object arg0)
  {
    this.OutputTabs();
    this.m_innerWriter.WriteLine(format, arg0);
    this.m_bTabsPending = true;
  }

  public override void WriteLine(string format, object arg0, object arg1)
  {
    this.OutputTabs();
    this.m_innerWriter.WriteLine(format, arg0, arg1);
    this.m_bTabsPending = true;
  }

  public override void WriteLine(char[] buffer, int index, int count)
  {
    this.OutputTabs();
    this.m_innerWriter.WriteLine(buffer, index, count);
    this.m_bTabsPending = true;
  }

  public int AddFont(Font font)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (this.m_hashFonts.ContainsKey(font))
      return this.m_hashFonts[font];
    int num = this.m_hashFonts.Count + 1;
    this.m_hashFonts.Add(font, num);
    return num;
  }

  public int AddColor(Color color)
  {
    if (!this.m_hashColorTable.ContainsKey(color))
    {
      this.m_hashColorTable.Add(color, this.m_hashColorTable.Count + 1);
      this.m_arrColors.Add(color);
    }
    return this.m_hashColorTable[color];
  }

  public void WriteFontTable()
  {
    if (this.m_hashFonts.Count == 0)
      return;
    this.WriteTag(RtfTags.FontTableBegin);
    foreach (Font key in this.m_hashFonts.Keys)
      this.WriteFontInTable(key);
    this.WriteTag(RtfTags.FontTableEnd);
  }

  public void WriteColorTable()
  {
    if (this.m_hashColorTable.Count == 0)
      return;
    this.WriteTag(RtfTags.ColorTableStart);
    int index = 0;
    for (int count = this.m_arrColors.Count; index < count; ++index)
      this.WriteColorInTable(this.m_arrColors[index]);
    this.WriteTag(RtfTags.ColorTableEnd);
  }

  public void WriteText(Font font, string strText)
  {
    this.WriteText(font, ColorExtension.Empty, ColorExtension.Empty, strText);
  }

  public void WriteText(Font font, Color foreColor, string strText)
  {
    this.WriteText(font, foreColor, ColorExtension.Empty, strText);
  }

  public void WriteText(Font font, Color foreColor, Color backColor, string strText)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    switch (strText)
    {
      case null:
        break;
      case "":
        break;
      default:
        this.WriteTag(RtfTags.GroupStart);
        this.WriteFont(font);
        if (foreColor != ColorExtension.Empty)
          this.WriteForeColorAttribute(foreColor);
        if (backColor != ColorExtension.Empty)
          this.WriteBackColorAttribute(backColor);
        int startIndex = 0;
        int length = strText.Length;
        bool flag = true;
        int num;
        for (; startIndex < length; startIndex = num + this.NewLine.Length)
        {
          num = strText.IndexOf(this.NewLine, startIndex);
          if (num == -1)
          {
            num = strText.Length;
            flag = false;
          }
          this.Write(strText.Substring(startIndex, num - startIndex));
          if (flag)
            this.WriteTag(RtfTags.EndLine);
        }
        this.WriteTag(RtfTags.GroupEnd);
        break;
    }
  }

  public void WriteText(IOfficeFont font, string strText)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    switch (strText)
    {
      case null:
        break;
      case "":
        break;
      default:
        this.WriteTag(RtfTags.GroupStart);
        this.WriteFont(font);
        int startIndex = 0;
        int length = strText.Length;
        bool flag = true;
        int num;
        for (; startIndex < length; startIndex = num + this.NewLine.Length)
        {
          num = strText.IndexOf(this.NewLine, startIndex);
          if (num == -1)
          {
            num = strText.Length;
            flag = false;
          }
          this.Write(strText.Substring(startIndex, num - startIndex));
          if (flag)
            this.WriteTag(RtfTags.EndLine);
        }
        this.WriteTag(RtfTags.GroupEnd);
        break;
    }
  }

  internal void WriteImageText(IOfficeFont font, string strText, string image, string align)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    switch (strText)
    {
      case null:
        break;
      case "":
        break;
      default:
        this.WriteTag(RtfTags.GroupStart);
        this.WriteFont(font);
        int startIndex = 0;
        int length = strText.Length;
        bool flag = true;
        int num;
        for (; startIndex < length; startIndex = num + this.NewLine.Length)
        {
          num = strText.IndexOf(this.NewLine, startIndex);
          if (num == -1)
          {
            num = strText.Length;
            flag = false;
          }
          this.Write(strText.Substring(startIndex, num - startIndex), image, align);
          if (flag)
            this.WriteTag(RtfTags.EndLine);
        }
        this.WriteTag(RtfTags.GroupEnd);
        break;
    }
  }

  public void WriteFontAttribute(Font font)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (!this.m_hashFonts.ContainsKey(font))
      throw new ArgumentException("Unknown font");
    this.WriteFontAttribute(this.m_hashFonts[font], (int) font.Size);
  }

  public void WriteFont(Font font)
  {
    this.WriteFontAttribute(font);
    this.WriteFontItalicBoldStriked(font);
    if (!font.Underline)
      return;
    this.WriteUnderlineAttribute();
  }

  public void WriteFont(IOfficeFont font)
  {
    FontImpl font1;
    switch (font)
    {
      case null:
        throw new ArgumentNullException(nameof (font));
      case FontImpl _:
        font1 = (FontImpl) font;
        break;
      case FontWrapper _:
        font1 = ((FontWrapper) font).Wrapped;
        break;
      default:
        throw new InvalidCastException("Wrong type of font");
    }
    Font nativeFont = font.GenerateNativeFont();
    this.WriteFontAttribute(nativeFont);
    this.WriteFontItalicBoldStriked(nativeFont);
    this.WriteUnderline(font1);
    this.WriteSubSuperScript(font1);
    this.WriteForeColorAttribute(font.RGBColor);
  }

  public void WriteSubSuperScript(FontImpl font)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (font.Subscript)
    {
      this.WriteTag(RtfTags.SubScript);
    }
    else
    {
      if (!font.Superscript)
        return;
      this.WriteTag(RtfTags.SuperScript);
    }
  }

  public void WriteFontItalicBoldStriked(Font font)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (font.Italic)
      this.WriteTag(RtfTags.ItalicOn);
    if (font.Bold)
      this.WriteTag(RtfTags.BoldOn);
    if (!font.Strikeout)
      return;
    this.WriteStrikeThrough(StrikeThroughStyle.SingleOn);
  }

  public void WriteUnderline(FontImpl font)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    switch (font.Underline)
    {
      case OfficeUnderline.Single:
      case OfficeUnderline.SingleAccounting:
        this.WriteUnderlineAttribute(UnderlineStyle.Continuous);
        break;
      case OfficeUnderline.Double:
      case OfficeUnderline.DoubleAccounting:
        this.WriteUnderlineAttribute(UnderlineStyle.Double);
        break;
    }
  }

  public void WriteUnderlineAttribute()
  {
    if (!this.m_bEnableFormatting)
      return;
    this.WriteUnderlineAttribute(UnderlineStyle.Continuous);
  }

  public void WriteUnderlineAttribute(UnderlineStyle style)
  {
    int index = (int) style;
    if (index < 0 || index >= RtfTextWriter.UnderlineTags.Length)
      throw new ArgumentOutOfRangeException(nameof (style));
    this.Escape = false;
    this.Write(RtfTextWriter.UnderlineTags[index]);
    this.Escape = true;
  }

  public void WriteStrikeThrough(StrikeThroughStyle style)
  {
    int index = (int) style;
    if (index < 0 || index >= RtfTextWriter.StrikeThroughTags.Length)
      throw new ArgumentOutOfRangeException(nameof (style));
    this.Escape = false;
    this.Write(RtfTextWriter.StrikeThroughTags[index]);
    this.Escape = true;
  }

  public void WriteBackColorAttribute(Color color)
  {
    if (!this.m_hashColorTable.ContainsKey(color))
      throw new ArgumentOutOfRangeException(nameof (color), "Unknown color");
    this.WriteTag(RtfTags.BackColor, (object) this.m_hashColorTable[color]);
  }

  public void WriteForeColorAttribute(Color color)
  {
    if (!this.m_hashColorTable.ContainsKey(color))
      throw new ArgumentOutOfRangeException(nameof (color), "Unknown color");
    this.WriteTag(RtfTags.ForeColor, (object) this.m_hashColorTable[color]);
  }

  public void WriteLineNoTabs(string s) => this.m_innerWriter.WriteLine(s);

  public void WriteTag(RtfTags tag)
  {
    if (!this.m_bEnableFormatting)
      return;
    int index = (int) tag;
    if (index < 0 || index >= RtfTextWriter.DEF_TAGS.Length)
      throw new ArgumentOutOfRangeException(nameof (tag));
    this.Escape = false;
    this.m_innerWriter.Write(RtfTextWriter.DEF_TAGS[index]);
    this.Escape = true;
  }

  public void WriteTag(RtfTags tag, params object[] arrParams)
  {
    if (!this.m_bEnableFormatting)
      return;
    int index = (int) tag;
    if (index < 0 || index >= RtfTextWriter.DEF_TAGS.Length)
      throw new ArgumentOutOfRangeException(nameof (tag));
    this.Escape = false;
    this.m_innerWriter.Write(string.Format(RtfTextWriter.DEF_TAGS[index], arrParams));
    this.Escape = true;
  }

  public bool Escape
  {
    get => this.m_bEscape;
    set => this.m_bEscape = value;
  }

  public override Encoding Encoding => this.m_innerWriter.Encoding;

  internal void WriteAlignment(string alignment)
  {
    string str = "ql";
    switch (alignment)
    {
      case "Center":
        str = "qc";
        break;
      case "Left":
        str = "ql";
        break;
      case "Right":
        str = "qr";
        break;
    }
    this.Escape = false;
    this.m_innerWriter.Write($"\\pard\\{str}");
    this.Escape = true;
  }
}
