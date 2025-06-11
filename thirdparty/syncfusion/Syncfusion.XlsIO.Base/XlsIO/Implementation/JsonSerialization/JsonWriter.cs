// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.JsonSerialization.JsonWriter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.JsonSerialization;

internal class JsonWriter : IDisposable
{
  private const string OpenCurlyBraces = "{";
  private const string CloseCurlyBraces = "}";
  private const string OpenSquareBracket = "[";
  private const string CloseSquareBracket = "]";
  private const string NewLine = "\n";
  private const string Comma = ",";
  private const string DoubleQuote = "\"";
  private const string Colon = ":";
  private TextWriter m_textWriter;
  private StreamWriter m_streamWriter;
  private int m_spaces;
  private int m_index;
  private bool m_isValueWritten;
  private bool m_isArrayWritten;
  private bool m_isObjectWritten;
  private int m_arrayOpenCount;
  private int m_objectOpenCount;
  private JsonFormatting m_formatting;
  private bool m_bIsNonSchema;
  private int m_indentation = 2;
  private char m_indentChar = ' ';

  internal JsonFormatting Formatting
  {
    get => this.m_formatting;
    set => this.m_formatting = value;
  }

  internal bool IsNonSchema
  {
    get => this.m_bIsNonSchema;
    set => this.m_bIsNonSchema = value;
  }

  internal int Indentation
  {
    get => this.m_indentation;
    set => this.m_indentation = value;
  }

  internal char IndentChar
  {
    get => this.m_indentChar;
    set => this.m_indentChar = value;
  }

  internal void WriteStartObject()
  {
    if (this.m_isObjectWritten)
    {
      this.m_textWriter.Write(",");
      this.m_textWriter.Write($"\n{this.GetSpaces()}{{");
      this.m_isObjectWritten = false;
    }
    else if (this.m_index != 0)
      this.m_textWriter.Write($"\n{this.GetSpaces()}{{");
    else
      this.m_textWriter.Write(this.GetSpaces() + "{");
    this.m_spaces += this.m_indentation;
    ++this.m_index;
  }

  internal void WriteEndObject()
  {
    this.m_spaces -= this.m_indentation;
    if (this.m_isValueWritten || this.m_isArrayWritten)
    {
      this.m_textWriter.Write($"\n{this.GetSpaces()}}}");
      this.m_isValueWritten = false;
      this.m_isArrayWritten = false;
    }
    else
      this.m_textWriter.Write(this.GetSpaces() + "}");
    this.m_isObjectWritten = true;
  }

  internal void WriteStartArray()
  {
    if (this.m_bIsNonSchema)
    {
      if (this.m_index != 0)
      {
        if (this.m_isArrayWritten)
        {
          this.m_textWriter.Write(",");
          this.m_isArrayWritten = false;
        }
        this.m_textWriter.Write($"\n{this.GetSpaces()}[");
      }
      else
        this.m_textWriter.Write(this.GetSpaces() + "[");
      ++this.m_index;
    }
    else
      this.m_textWriter.Write(this.IndentChar.ToString() + "[");
    this.m_spaces += this.m_indentation;
  }

  internal void WriteEndArray()
  {
    --this.m_arrayOpenCount;
    this.m_spaces -= this.m_indentation;
    this.m_textWriter.Write($"\n{this.GetSpaces()}]");
    this.m_isArrayWritten = true;
    this.m_isObjectWritten = false;
    if (!this.m_bIsNonSchema)
      return;
    this.m_isValueWritten = false;
  }

  internal void WritePropertyName(string Name)
  {
    if (this.m_isValueWritten || this.m_isArrayWritten)
    {
      this.m_textWriter.Write(",");
      this.m_textWriter.Write($"\n{this.GetSpaces()}\"{Name}\":");
      this.m_isValueWritten = false;
      this.m_isArrayWritten = false;
    }
    else
      this.m_textWriter.Write($"\n{this.GetSpaces()}\"{Name}\":");
  }

  internal void WriteValue(string Value)
  {
    if (this.m_bIsNonSchema)
    {
      if (this.m_isValueWritten)
        this.m_textWriter.Write(",");
      this.m_textWriter.Write($"\n{this.GetSpaces()}\"{Value}\"");
    }
    else
      this.m_textWriter.Write($"{(object) this.IndentChar}\"{Value}\"");
    this.m_isValueWritten = true;
  }

  private string GetSpaces()
  {
    string empty = string.Empty;
    for (int index = 1; index <= this.m_spaces; ++index)
      empty += this.m_formatting == JsonFormatting.Indented ? this.IndentChar.ToString() : string.Empty;
    return empty;
  }

  internal JsonWriter(StreamWriter streamWriter) => this.m_textWriter = (TextWriter) streamWriter;

  internal JsonWriter(Stream stream) => this.m_textWriter = (TextWriter) new StreamWriter(stream);

  public void Dispose()
  {
    this.m_textWriter.Flush();
    this.m_textWriter = (TextWriter) null;
  }
}
