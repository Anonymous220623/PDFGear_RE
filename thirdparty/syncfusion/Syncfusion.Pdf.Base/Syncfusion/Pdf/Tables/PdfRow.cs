// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tables.PdfRow
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Tables;

public class PdfRow
{
  private object[] m_values;

  public object[] Values
  {
    get => this.m_values;
    set => this.m_values = value;
  }

  internal PdfRow()
  {
  }

  internal PdfRow(object[] values)
    : this()
  {
    this.m_values = values;
  }
}
