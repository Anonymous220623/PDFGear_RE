// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfMetafileLayoutFormat
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfMetafileLayoutFormat : PdfLayoutFormat
{
  private bool m_splitLines;
  private bool m_useImageResolution;
  private bool m_splitImages;
  private bool m_htmlPageBreak;
  internal bool m_enableDirectLayout;

  public bool SplitTextLines
  {
    get => this.m_splitLines;
    set => this.m_splitLines = value;
  }

  public bool UseImageResolution
  {
    get => this.m_useImageResolution;
    set => this.m_useImageResolution = value;
  }

  public bool SplitImages
  {
    get => this.m_splitImages;
    set => this.m_splitImages = value;
  }

  public bool IsHTMLPageBreak
  {
    get => this.m_htmlPageBreak;
    set => this.m_htmlPageBreak = value;
  }
}
