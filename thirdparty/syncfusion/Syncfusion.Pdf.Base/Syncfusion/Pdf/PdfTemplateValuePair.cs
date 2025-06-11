// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfTemplateValuePair
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class PdfTemplateValuePair
{
  private PdfTemplate m_template;
  private string m_value = string.Empty;

  public PdfTemplateValuePair()
  {
  }

  public PdfTemplateValuePair(PdfTemplate template, string value)
  {
    this.Template = template;
    this.Value = value;
  }

  public PdfTemplate Template
  {
    get => this.m_template;
    set
    {
      this.m_template = value != null ? value : throw new ArgumentNullException(nameof (Template));
    }
  }

  public string Value
  {
    get => this.m_value;
    set
    {
      this.m_value = this.m_value != null ? value : throw new ArgumentNullException(nameof (Value));
    }
  }
}
