// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PostScriptOperators
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class PostScriptOperators
{
  private string m_value;
  private PostScriptOperatorTypes m_operatorType;

  internal string Operand
  {
    set => this.m_value = value;
    get => this.m_value;
  }

  internal PostScriptOperatorTypes Operatortype
  {
    get => this.m_operatorType;
    set => this.m_operatorType = value;
  }

  public PostScriptOperators(PostScriptOperatorTypes key, string value)
  {
    this.m_operatorType = key;
    this.m_value = value;
  }
}
