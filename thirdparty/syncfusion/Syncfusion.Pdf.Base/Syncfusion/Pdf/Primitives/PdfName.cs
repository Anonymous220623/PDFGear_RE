// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Primitives.PdfName
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Primitives;

internal class PdfName : IPdfPrimitive
{
  internal const string StringStartMark = "/";
  public static string Delimiters = "()<>[]{}/%}";
  private static readonly char[] m_replacements = new char[4]
  {
    ' ',
    '\t',
    '\n',
    '\r'
  };
  private string m_value = string.Empty;
  private ObjectStatus m_status;
  private bool m_isSaving;
  private int m_index;
  private int m_position = -1;

  public string Value
  {
    get => this.m_value;
    set
    {
      if (!(value != this.m_value))
        return;
      string str = value;
      if (value != null && value.Length > 0)
        this.m_value = PdfName.NormalizeValue(value.Substring(0, 1) == "/" ? value.Substring(1) : value);
      else
        this.m_value = str;
    }
  }

  public ObjectStatus Status
  {
    get => this.m_status;
    set => this.m_status = value;
  }

  public bool IsSaving
  {
    get => this.m_isSaving;
    set => this.m_isSaving = value;
  }

  public int ObjectCollectionIndex
  {
    get => this.m_index;
    set => this.m_index = value;
  }

  public int Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  public IPdfPrimitive ClonedObject => (IPdfPrimitive) null;

  public PdfName()
  {
  }

  public PdfName(string value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (value.IndexOfAny(PdfName.m_replacements) != -1)
      this.m_value = PdfName.NormalizeValue(value);
    else
      this.m_value = value;
  }

  public PdfName(Enum value)
    : this(value.ToString())
  {
  }

  private static string NormalizeValue(string value)
  {
    string str = value;
    foreach (char replacement in PdfName.m_replacements)
      str = PdfName.NormalizeValue(str, replacement);
    return str;
  }

  private static string NormalizeValue(string value, char symbol)
  {
    return value.Replace(symbol.ToString(), $"#{(int) symbol:X}");
  }

  public static string EscapeString(string str)
  {
    if (str == null)
      throw new ArgumentNullException(nameof (str));
    if (str == string.Empty)
      return str;
    StringBuilder stringBuilder = new StringBuilder();
    int index = 0;
    for (int length = str.Length; index < length; ++index)
    {
      char ch = str[index];
      PdfName.Delimiters.IndexOf(ch);
      switch (ch)
      {
        case '\n':
          stringBuilder.Append("\n");
          break;
        case '\r':
          stringBuilder.Append("\\r");
          break;
        case '(':
        case ')':
        case '\\':
          stringBuilder.Append(ch);
          break;
        default:
          stringBuilder.Append(ch);
          break;
      }
    }
    return stringBuilder.ToString();
  }

  internal static string EncodeName(string value)
  {
    StringBuilder stringBuilder = new StringBuilder();
    foreach (uint num in value.ToCharArray())
    {
      char ch = (char) (num & (uint) byte.MaxValue);
      switch (ch)
      {
        case ' ':
        case '#':
        case '%':
        case '(':
        case ')':
        case '/':
        case '<':
        case '>':
        case '[':
        case ']':
        case '{':
        case '}':
          stringBuilder.Append('#');
          stringBuilder.Append(Convert.ToString((int) ch, 16 /*0x10*/));
          break;
        default:
          if (ch > '~' || ch < ' ')
          {
            stringBuilder.Append('#');
            if (ch < '\u0010')
              stringBuilder.Append('0');
            stringBuilder.Append(Convert.ToString((int) ch, 16 /*0x10*/));
            break;
          }
          stringBuilder.Append(ch);
          break;
      }
    }
    return stringBuilder.ToString();
  }

  internal static string DecodeName(string name)
  {
    StringBuilder stringBuilder = new StringBuilder();
    int length = name.Length;
    for (int index = 0; index < length; ++index)
    {
      char ch = name[index];
      if (ch == '#')
      {
        ch = (char) ((PdfName.ConvertToHex((int) name[index + 1]) << 4) + PdfName.ConvertToHex((int) name[index + 2]));
        index += 2;
      }
      stringBuilder.Append(ch);
    }
    return stringBuilder.ToString();
  }

  private static int ConvertToHex(int hex)
  {
    if (hex >= 48 /*0x30*/ && hex <= 57)
      return hex - 48 /*0x30*/;
    if (hex >= 65 && hex <= 70)
      return hex - 65 + 10;
    return hex >= 97 && hex <= 102 ? hex - 97 + 10 : -1;
  }

  public static explicit operator PdfName(string str)
  {
    return str != null ? new PdfName(str) : throw new ArgumentNullException(nameof (str));
  }

  public override string ToString() => "/" + PdfName.EscapeString(this.Value);

  public override bool Equals(object obj)
  {
    PdfName pdfName = obj as PdfName;
    return !(pdfName == (object) null) && pdfName.Value == this.Value;
  }

  public override int GetHashCode() => this.Value.GetHashCode();

  public static bool operator ==(PdfName name1, object name2)
  {
    object obj = (object) name1;
    bool flag;
    if (obj == name2)
      flag = true;
    else if (obj == null || name2 == null)
    {
      flag = false;
    }
    else
    {
      PdfName pdfName = name2 as PdfName;
      flag = !(pdfName == (PdfName) null) && name1.Value == pdfName.Value;
    }
    return flag;
  }

  public static bool operator !=(PdfName name1, object name2) => !(name1 == name2);

  public static bool operator ==(PdfName name1, PdfName name2)
  {
    return (object) name1 == (object) name2 || (object) name1 != null && (object) name2 != null && name1.Value == name2.Value;
  }

  public static bool operator !=(PdfName name1, PdfName name2) => !(name1 == name2);

  public void Save(IPdfWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.Write(this.ToString());
  }

  public IPdfPrimitive Clone(PdfCrossTable crossTable)
  {
    return (IPdfPrimitive) new PdfName()
    {
      Value = this.m_value
    };
  }
}
