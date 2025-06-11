// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfRecord
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Diagnostics;

#nullable disable
namespace Syncfusion.Pdf;

[DebuggerDisplay("({OperatorName}, operands={Operands.Length})")]
internal class PdfRecord
{
  private string m_operatorName;
  private string[] m_operands;
  private byte[] m_inlineImageBytes;

  internal string OperatorName
  {
    get => this.m_operatorName;
    set => this.m_operatorName = value;
  }

  internal string[] Operands
  {
    get => this.m_operands;
    set => this.m_operands = value;
  }

  internal byte[] InlineImageBytes
  {
    get => this.m_inlineImageBytes;
    set => this.m_inlineImageBytes = value;
  }

  public PdfRecord(string name, string[] operands)
  {
    this.m_operatorName = name;
    this.m_operands = operands;
  }

  internal PdfRecord(string name, byte[] imageData)
  {
    this.m_operatorName = name;
    this.m_inlineImageBytes = imageData;
  }
}
