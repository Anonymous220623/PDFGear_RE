// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaLine
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

internal class PdfLoadedXfaLine : PdfLoadedXfaStyledField
{
  private PdfXfaEdge edge;

  internal void ReadField(XmlNode node)
  {
    this.currentNode = node;
    this.ReadCommonProperties(node);
    if (node["value"] == null || node["value"]["line"] == null || node["value"]["line"]["edge"] == null)
      return;
    this.edge = new PdfXfaEdge();
    this.edge.Read((XmlNode) node["value"]["line"]["edge"], this.edge);
    if ((double) this.Height != 0.0)
      return;
    this.Height = this.edge.Thickness;
  }

  internal void DrawLine(PdfGraphics graphics, RectangleF bounds)
  {
    PdfPen pen = (PdfPen) null;
    if (this.edge != null)
      pen = new PdfPen(this.edge.Color, this.edge.Thickness);
    graphics.DrawLine(pen, bounds.Location, new PointF(bounds.Width + bounds.X, bounds.Height + bounds.Y));
  }
}
