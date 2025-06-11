// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ShapeStyleReference
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class ShapeStyleReference
{
  private int m_styleRefIndex;
  private Color m_styleRefColor;
  private float m_styleRefOpacity;

  internal int StyleRefIndex
  {
    get => this.m_styleRefIndex;
    set => this.m_styleRefIndex = value;
  }

  internal Color StyleRefColor
  {
    get => this.m_styleRefColor;
    set => this.m_styleRefColor = value;
  }

  internal float StyleRefOpacity
  {
    get => this.m_styleRefOpacity;
    set => this.m_styleRefOpacity = value;
  }
}
