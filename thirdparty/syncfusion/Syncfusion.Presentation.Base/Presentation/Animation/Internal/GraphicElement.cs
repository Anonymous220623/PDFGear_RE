// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.GraphicElement
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class GraphicElement
{
  private Chart graphicChart;
  private Diagram graphicDiagram;

  internal Chart GraphicChart
  {
    get => this.graphicChart;
    set => this.graphicChart = value;
  }

  internal Diagram GraphicDiagram
  {
    get => this.graphicDiagram;
    set => this.graphicDiagram = value;
  }

  internal GraphicElement Clone()
  {
    GraphicElement graphicElement = (GraphicElement) this.MemberwiseClone();
    if (this.graphicChart != null)
      graphicElement.graphicChart = this.graphicChart.Clone();
    if (this.graphicDiagram != null)
      graphicElement.graphicDiagram = this.graphicDiagram.Clone();
    return graphicElement;
  }
}
