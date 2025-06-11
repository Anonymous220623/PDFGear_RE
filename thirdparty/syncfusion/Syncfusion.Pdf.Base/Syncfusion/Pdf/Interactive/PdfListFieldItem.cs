// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfListFieldItem
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfListFieldItem : IPdfWrapper
{
  private int c_textIndex = 1;
  private int c_valueIndex;
  private string m_text = string.Empty;
  private string m_value = string.Empty;
  private PdfArray m_array = new PdfArray();

  public PdfListFieldItem() => this.Initialize(this.m_text, this.m_value);

  public PdfListFieldItem(string text, string value) => this.Initialize(text, value);

  public string Text
  {
    get => this.m_text;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Text));
      if (!(this.m_text != value))
        return;
      this.m_text = value;
      ((PdfString) this.m_array[this.c_textIndex]).Value = this.m_text;
    }
  }

  public string Value
  {
    get => this.m_value;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Value));
      if (!(this.m_value != value))
        return;
      this.m_value = value;
      ((PdfString) this.m_array[this.c_valueIndex]).Value = this.m_value;
    }
  }

  private void Initialize(string text, string value)
  {
    if (this.c_valueIndex < this.c_textIndex)
    {
      this.m_array.Add((IPdfPrimitive) new PdfString(value));
      this.m_array.Add((IPdfPrimitive) new PdfString(text));
    }
    else
    {
      this.m_array.Add((IPdfPrimitive) new PdfString(text));
      this.m_array.Add((IPdfPrimitive) new PdfString(value));
    }
    this.m_text = text;
    this.m_value = value;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_array;
}
