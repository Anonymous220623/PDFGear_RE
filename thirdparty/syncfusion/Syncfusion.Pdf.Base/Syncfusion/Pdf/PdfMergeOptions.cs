// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfMergeOptions
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

public class PdfMergeOptions
{
  private bool m_optimizeResources;
  private bool m_extendMargin;

  public bool OptimizeResources
  {
    get => this.m_optimizeResources;
    set => this.m_optimizeResources = value;
  }

  public bool ExtendMargin
  {
    get => this.m_extendMargin;
    set => this.m_extendMargin = value;
  }
}
