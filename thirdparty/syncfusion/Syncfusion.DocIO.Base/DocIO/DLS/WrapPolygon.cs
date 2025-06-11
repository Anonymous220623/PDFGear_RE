// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WrapPolygon
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class WrapPolygon
{
  private byte m_bFlags;
  private List<PointF> m_vertices;

  internal bool Edited
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal List<PointF> Vertices
  {
    get => this.m_vertices;
    set => this.m_vertices = value;
  }

  internal WrapPolygon() => this.m_vertices = new List<PointF>();

  internal WrapPolygon Clone()
  {
    WrapPolygon wrapPolygon = new WrapPolygon();
    if (this.Vertices != null)
    {
      wrapPolygon.Vertices = new List<PointF>();
      foreach (PointF vertex in this.Vertices)
      {
        PointF pointF = new PointF(vertex.X, vertex.Y);
        wrapPolygon.Vertices.Add(pointF);
      }
    }
    return wrapPolygon;
  }

  internal void Close()
  {
    if (this.m_vertices == null)
      return;
    this.m_vertices.Clear();
    this.m_vertices = (List<PointF>) null;
  }
}
