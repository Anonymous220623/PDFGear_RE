// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SyntaxAnalyzerResult
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class SyntaxAnalyzerResult : AnalyzerResult
{
  private List<PdfException> m_errors;
  private bool m_isCorrupted;

  internal SyntaxAnalyzerResult()
  {
  }

  public List<PdfException> Errors
  {
    get => this.m_errors;
    internal set => this.m_errors = value;
  }

  public bool IsCorrupted
  {
    get => this.m_isCorrupted;
    internal set => this.m_isCorrupted = value;
  }
}
