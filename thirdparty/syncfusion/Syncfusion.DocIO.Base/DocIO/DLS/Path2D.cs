// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Path2D
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class Path2D
{
  private string m_pathCommandType;
  private List<PointF> m_pathPoints;
  private double m_width;
  private double m_height;
  private bool m_isStroke = true;
  private List<string> m_pathElementList;

  internal Path2D(string pathCommandType, List<PointF> pathPoints)
  {
    this.m_pathCommandType = pathCommandType;
    this.m_pathPoints = pathPoints;
  }

  internal Path2D()
  {
  }

  internal string PathCommandType => this.m_pathCommandType;

  internal List<PointF> PathPoints => this.m_pathPoints;

  internal List<string> PathElements
  {
    get => this.m_pathElementList ?? (this.m_pathElementList = new List<string>());
  }

  internal double Width
  {
    get => this.m_width;
    set => this.m_width = value;
  }

  internal double Height
  {
    get => this.m_height;
    set => this.m_height = value;
  }

  internal bool IsStroke
  {
    get => this.m_isStroke;
    set => this.m_isStroke = value;
  }

  internal Path2D Clone()
  {
    Path2D path2D = (Path2D) this.MemberwiseClone();
    List<string> stringList = new List<string>();
    foreach (string pathElement in this.m_pathElementList)
      stringList.Add(pathElement);
    path2D.m_pathElementList = stringList;
    return path2D;
  }

  internal void Close()
  {
    if (this.m_pathPoints != null)
    {
      this.m_pathPoints.Clear();
      this.m_pathPoints = (List<PointF>) null;
    }
    if (this.m_pathElementList == null)
      return;
    this.m_pathElementList.Clear();
    this.m_pathElementList = (List<string>) null;
  }

  internal enum Path2DElements : ushort
  {
    Close = 1,
    MoveTo = 2,
    LineTo = 3,
    ArcTo = 4,
    QuadBezTo = 5,
    CubicBezTo = 6,
  }
}
