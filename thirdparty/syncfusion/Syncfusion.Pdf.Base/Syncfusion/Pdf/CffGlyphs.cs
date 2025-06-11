// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.CffGlyphs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal class CffGlyphs
{
  private Dictionary<string, byte[]> m_glyphs = new Dictionary<string, byte[]>();
  private double[] m_fontMatrix;
  private Dictionary<int, string> m_differenceEncoding = new Dictionary<int, string>();
  private Dictionary<string, object> m_renderedPath = new Dictionary<string, object>();
  private int m_globalBias;
  private string[] m_diffTable;

  internal Dictionary<string, byte[]> Glyphs
  {
    get => this.m_glyphs;
    set => this.m_glyphs = value;
  }

  public int GlobalBias
  {
    get => this.m_globalBias;
    set => this.m_globalBias = value;
  }

  internal double[] FontMatrix
  {
    get => this.m_fontMatrix;
    set => this.m_fontMatrix = value;
  }

  internal Dictionary<int, string> DifferenceEncoding
  {
    get => this.m_differenceEncoding;
    set => this.m_differenceEncoding = value;
  }

  internal Dictionary<string, object> RenderedPath
  {
    get => this.m_renderedPath;
    set => this.m_renderedPath = value;
  }

  internal string[] DiffTable
  {
    get => this.m_diffTable;
    set => this.m_diffTable = value;
  }
}
