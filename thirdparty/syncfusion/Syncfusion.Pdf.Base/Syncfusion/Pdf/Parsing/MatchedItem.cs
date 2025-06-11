// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.MatchedItem
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class MatchedItem
{
  private string m_text;
  private int m_pageNumber;
  private RectangleF m_boundingBox;
  private Color m_textColor;

  public string Text
  {
    get => this.m_text;
    set => this.m_text = value;
  }

  internal Color TextColor
  {
    get => this.m_textColor;
    set => this.m_textColor = value;
  }

  internal int PageNumber
  {
    get => this.m_pageNumber;
    set => this.m_pageNumber = value;
  }

  public RectangleF Bounds
  {
    get => this.m_boundingBox;
    set => this.m_boundingBox = value;
  }
}
