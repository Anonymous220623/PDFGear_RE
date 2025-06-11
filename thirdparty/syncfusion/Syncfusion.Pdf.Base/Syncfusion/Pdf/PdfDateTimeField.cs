// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfDateTimeField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfDateTimeField : PdfStaticField
{
  private DateTime m_date = DateTime.Now;
  private string m_formatString = "dd'/'MM'/'yyyy hh':'mm':'ss";

  public PdfDateTimeField()
  {
  }

  public PdfDateTimeField(PdfFont font)
    : base(font)
  {
  }

  public PdfDateTimeField(PdfFont font, PdfBrush brush)
    : base(font, brush)
  {
  }

  public PdfDateTimeField(PdfFont font, RectangleF bounds)
    : base(font, bounds)
  {
  }

  public string DateFormatString
  {
    get => this.m_formatString;
    set => this.m_formatString = value;
  }

  protected internal override string GetValue(PdfGraphics graphics)
  {
    return this.m_date.ToString(this.m_formatString);
  }
}
