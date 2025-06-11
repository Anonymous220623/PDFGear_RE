// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.DecodeIntResult
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class DecodeIntResult
{
  private int m_intResult;
  private bool m_booleanResult;

  internal DecodeIntResult(int intResult, bool booleanResult)
  {
    this.m_intResult = intResult;
    this.m_booleanResult = booleanResult;
  }

  internal int IntResult => this.m_intResult;

  internal bool BooleanResult => this.m_booleanResult;
}
