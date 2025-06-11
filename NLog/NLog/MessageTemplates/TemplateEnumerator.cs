// Decompiled with JetBrains decompiler
// Type: NLog.MessageTemplates.TemplateEnumerator
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace NLog.MessageTemplates;

internal struct TemplateEnumerator : IEnumerator<LiteralHole>, IDisposable, IEnumerator
{
  private static readonly char[] HoleDelimiters = new char[3]
  {
    '}',
    ':',
    ','
  };
  private static readonly char[] TextDelimiters = new char[2]
  {
    '{',
    '}'
  };
  private string _template;
  private int _length;
  private int _position;
  private int _literalLength;
  private LiteralHole _current;
  private const short Zero = 0;

  public TemplateEnumerator(string template)
  {
    this._template = template ?? throw new ArgumentNullException(nameof (template));
    this._length = this._template.Length;
    this._position = 0;
    this._literalLength = 0;
    this._current = new LiteralHole();
  }

  public LiteralHole Current => this._current;

  object IEnumerator.Current => (object) this._current;

  public void Dispose()
  {
    this._template = string.Empty;
    this._length = 0;
    this.Reset();
  }

  public void Reset()
  {
    this._position = 0;
    this._literalLength = 0;
    this._current = new LiteralHole();
  }

  public bool MoveNext()
  {
    try
    {
      while (this._position < this._length)
      {
        switch (this.Peek())
        {
          case '{':
            this.ParseOpenBracketPart();
            return true;
          case '}':
            this.ParseCloseBracketPart();
            return true;
          default:
            this.ParseTextPart();
            continue;
        }
      }
      if (this._literalLength == 0)
        return false;
      this.AddLiteral();
      return true;
    }
    catch (IndexOutOfRangeException ex)
    {
      throw new TemplateParserException("Unexpected end of template.", this._position, this._template);
    }
  }

  private void AddLiteral()
  {
    this._current = new LiteralHole(new Literal(this._literalLength, 0), new Hole());
    this._literalLength = 0;
  }

  private void ParseTextPart()
  {
    this._literalLength = this.SkipUntil(TemplateEnumerator.TextDelimiters, false);
  }

  private void ParseOpenBracketPart()
  {
    this.Skip('{');
    switch (this.Peek())
    {
      case '$':
        this.Skip('$');
        this.ParseHole(CaptureType.Stringify);
        break;
      case '@':
        this.Skip('@');
        this.ParseHole(CaptureType.Serialize);
        break;
      case '{':
        this.Skip('{');
        ++this._literalLength;
        this.AddLiteral();
        break;
      default:
        this.ParseHole(CaptureType.Normal);
        break;
    }
  }

  private void ParseCloseBracketPart()
  {
    this.Skip('}');
    if (this.Read() != '}')
      throw new TemplateParserException("Unexpected '}}' ", this._position - 2, this._template);
    ++this._literalLength;
    this.AddLiteral();
  }

  private void ParseHole(CaptureType type)
  {
    int position = this._position;
    int parameterIndex;
    string name = this.ParseName(out parameterIndex);
    int alignment = 0;
    string format = (string) null;
    if (this.Peek() != '}')
    {
      alignment = this.Peek() == ',' ? this.ParseAlignment() : 0;
      format = this.Peek() == ':' ? this.ParseFormat() : (string) null;
      this.Skip('}');
    }
    else
      ++this._position;
    this._current = new LiteralHole(new Literal(this._literalLength, this._position - position + (type == CaptureType.Normal ? 1 : 2)), new Hole(name, format, type, (short) parameterIndex, (short) alignment));
    this._literalLength = 0;
  }

  private string ParseName(out int parameterIndex)
  {
    parameterIndex = -1;
    switch (this.Peek())
    {
      case '0':
      case '1':
      case '2':
      case '3':
      case '4':
      case '5':
      case '6':
      case '7':
      case '8':
      case '9':
        int position = this._position;
        int num = this.ReadInt();
        char ch = this.Peek();
        if (num >= 0)
        {
          switch (ch)
          {
            case ' ':
              this.SkipSpaces();
              switch (this.Peek())
              {
                case ',':
                case ':':
                case '}':
                  parameterIndex = num;
                  break;
              }
              break;
            case ',':
            case ':':
            case '}':
              parameterIndex = num;
              return TemplateEnumerator.ParameterIndexToString(parameterIndex);
          }
        }
        this._position = position;
        break;
    }
    return this.ReadUntil(TemplateEnumerator.HoleDelimiters);
  }

  private static string ParameterIndexToString(int parameterIndex)
  {
    switch (parameterIndex)
    {
      case 0:
        return "0";
      case 1:
        return "1";
      case 2:
        return "2";
      case 3:
        return "3";
      case 4:
        return "4";
      case 5:
        return "5";
      case 6:
        return "6";
      case 7:
        return "7";
      case 8:
        return "8";
      case 9:
        return "9";
      default:
        return parameterIndex.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }
  }

  private string ParseFormat()
  {
    this.Skip(':');
    string format = this.ReadUntil(TemplateEnumerator.TextDelimiters);
    char ch;
    while (true)
    {
      switch (this.Read())
      {
        case '{':
          ch = this.Peek();
          if (ch == '{')
          {
            this.Skip('{');
            format += "{";
            break;
          }
          goto label_7;
        case '}':
          if (this._position < this._length && this.Peek() == '}')
          {
            this.Skip('}');
            format += "}";
            break;
          }
          goto label_4;
      }
      format += this.ReadUntil(TemplateEnumerator.TextDelimiters);
    }
label_4:
    --this._position;
    return format;
label_7:
    throw new TemplateParserException($"Expected '{{' but found '{ch}' instead in format.", this._position, this._template);
  }

  private int ParseAlignment()
  {
    this.Skip(',');
    this.SkipSpaces();
    int alignment = this.ReadInt();
    this.SkipSpaces();
    char ch = this.Peek();
    switch (ch)
    {
      case ':':
        return alignment;
      case '}':
        return alignment;
      default:
        throw new TemplateParserException($"Expected ':' or '}}' but found '{ch}' instead.", this._position, this._template);
    }
  }

  private char Peek() => this._template[this._position];

  private char Read() => this._template[this._position++];

  private void Skip(char c) => ++this._position;

  private void SkipSpaces()
  {
    while (this._template[this._position] == ' ')
      ++this._position;
  }

  private int SkipUntil(char[] search, bool required = true)
  {
    int position = this._position;
    int num = this._template.IndexOfAny(search, this._position);
    if (num == -1 & required)
      throw new TemplateParserException($"Reached end of template while expecting one of {string.Join(", ", ((IEnumerable<char>) search).Select<char, string>((Func<char, string>) (c => $"'{c.ToString()}'")).ToArray<string>())}.", this._position, this._template);
    this._position = num == -1 ? this._length : num;
    return this._position - position;
  }

  private int ReadInt()
  {
    bool flag = false;
    int num1 = 0;
    for (int index = 0; index < 12; ++index)
    {
      char ch = this.Peek();
      int num2 = (int) ch - 48 /*0x30*/;
      if (num2 < 0 || num2 > 9)
      {
        if (index > 0 && !flag || index > 1)
          return !flag ? num1 : -num1;
        if (index == 0 && ch == '-')
        {
          flag = true;
          ++this._position;
        }
        else
          break;
      }
      else
      {
        ++this._position;
        num1 = num1 * 10 + num2;
      }
    }
    throw new TemplateParserException("An integer is expected", this._position, this._template);
  }

  private string ReadUntil(char[] search, bool required = true)
  {
    return this._template.Substring(this._position, this.SkipUntil(search, required));
  }
}
