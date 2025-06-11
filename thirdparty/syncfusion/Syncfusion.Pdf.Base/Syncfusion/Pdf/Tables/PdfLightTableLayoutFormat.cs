// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tables.PdfLightTableLayoutFormat
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;

#nullable disable
namespace Syncfusion.Pdf.Tables;

public class PdfLightTableLayoutFormat : PdfLayoutFormat
{
  private int m_startColumn;
  private int m_endColumn;

  public int StartColumnIndex
  {
    get => this.m_startColumn;
    set => this.m_startColumn = value;
  }

  public int EndColumnIndex
  {
    get => this.m_endColumn;
    set
    {
      this.m_endColumn = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof (EndColumnIndex));
    }
  }

  public PdfLightTableLayoutFormat()
  {
  }

  public PdfLightTableLayoutFormat(PdfLayoutFormat baseFormat)
    : base(baseFormat)
  {
  }
}
