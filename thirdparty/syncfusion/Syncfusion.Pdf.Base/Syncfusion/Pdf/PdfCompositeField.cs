// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfCompositeField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfCompositeField : PdfMultipleValueField
{
  private PdfAutomaticField[] m_automaticFields;
  private string m_text = string.Empty;

  public PdfCompositeField()
  {
  }

  public PdfCompositeField(PdfFont font)
    : base(font)
  {
  }

  public PdfCompositeField(PdfFont font, PdfBrush brush)
    : base(font, brush)
  {
  }

  public PdfCompositeField(PdfFont font, string text)
    : base(font)
  {
    this.Text = text;
  }

  public PdfCompositeField(PdfFont font, PdfBrush brush, string text)
    : base(font, brush)
  {
    this.Text = text;
  }

  public PdfCompositeField(string text, params PdfAutomaticField[] list)
  {
    this.m_automaticFields = list;
    this.Text = text;
  }

  public PdfCompositeField(PdfFont font, string text, params PdfAutomaticField[] list)
    : base(font)
  {
    this.Text = text;
    this.m_automaticFields = list;
  }

  public PdfCompositeField(
    PdfFont font,
    PdfBrush brush,
    string text,
    params PdfAutomaticField[] list)
    : base(font, brush)
  {
    this.Text = text;
    this.m_automaticFields = list;
  }

  public string Text
  {
    get => this.m_text;
    set => this.m_text = value != null ? value : throw new ArgumentNullException(nameof (Text));
  }

  public PdfAutomaticField[] AutomaticFields
  {
    get => this.m_automaticFields;
    set => this.m_automaticFields = value;
  }

  protected internal override string GetValue(PdfGraphics graphics)
  {
    if (this.m_automaticFields == null || this.m_automaticFields.Length <= 0)
      return this.m_text;
    string[] strArray = new string[this.m_automaticFields.Length];
    int num = 0;
    foreach (PdfAutomaticField automaticField in this.m_automaticFields)
      strArray[num++] = automaticField.GetValue(graphics);
    return string.Format(this.m_text, (object[]) strArray);
  }
}
