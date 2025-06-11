// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.Context
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class Context
{
  private short m_qe;
  private char m_mps;
  private char m_lps;

  internal short Qe
  {
    get => this.m_qe;
    set => this.m_qe = value;
  }

  internal char Mps
  {
    get => this.m_mps;
    set => this.m_mps = value;
  }

  internal char Lps
  {
    get => this.m_lps;
    set => this.m_lps = value;
  }

  internal Context(short qe, char mps, char lps)
  {
    this.Qe = qe;
    this.Mps = mps;
    this.Lps = lps;
  }
}
