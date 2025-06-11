// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.Path2D
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class Path2D
{
  private List<double> _pathElementList;
  private double _width;
  private double _height;
  private bool _isStroke = true;
  private PathFillMode _fillMode = PathFillMode.Normal;

  internal List<double> PathElements
  {
    get => this._pathElementList ?? (this._pathElementList = new List<double>());
  }

  internal double Width
  {
    get => this._width;
    set => this._width = value;
  }

  internal double Height
  {
    get => this._height;
    set => this._height = value;
  }

  internal PathFillMode FillMode
  {
    get => this._fillMode;
    set => this._fillMode = value;
  }

  internal bool IsStroke
  {
    get => this._isStroke;
    set => this._isStroke = value;
  }

  internal void Close()
  {
    if (this._pathElementList == null)
      return;
    this._pathElementList.Clear();
    this._pathElementList = (List<double>) null;
  }

  public Path2D Clone()
  {
    Path2D path2D = (Path2D) this.MemberwiseClone();
    path2D._pathElementList = Helper.CloneList(this._pathElementList);
    return path2D;
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
