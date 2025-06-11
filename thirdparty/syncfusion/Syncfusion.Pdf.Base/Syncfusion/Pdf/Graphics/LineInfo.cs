// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.LineInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics.Fonts;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class LineInfo
{
  internal string m_text;
  internal float m_width;
  internal LineType m_lineType;
  internal OtfGlyphInfoList OpenTypeGlyphList;

  public LineType LineType
  {
    get => this.m_lineType;
    internal set => this.m_lineType = value;
  }

  public string Text
  {
    get => this.m_text;
    internal set => this.m_text = value;
  }

  public float Width
  {
    get => this.m_width;
    internal set => this.m_width = value;
  }
}
