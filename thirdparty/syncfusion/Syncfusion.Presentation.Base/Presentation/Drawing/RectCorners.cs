// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.RectCorners
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class RectCorners
{
  private PointF _topLeft;
  private PointF _top;
  private PointF _topRight;
  private PointF _left;
  private PointF _right;
  private PointF _bottomLeft;
  private PointF _bottom;
  private PointF _bottomRight;
  private PointF _center;
  private RectangleF _outerBounds;

  internal RectCorners(RectangleF bounds)
  {
    this._outerBounds = bounds;
    this._topLeft = new PointF(bounds.X, bounds.Y);
    this._top = new PointF(bounds.X + bounds.Width / 2f, bounds.Y);
    this._topRight = new PointF(bounds.X + bounds.Width, bounds.Y);
    this._left = new PointF(bounds.X, bounds.Y + bounds.Height / 2f);
    this._right = new PointF(bounds.X + bounds.Width, bounds.Y + bounds.Height / 2f);
    this._bottomLeft = new PointF(bounds.X, bounds.Y + bounds.Height);
    this._bottom = new PointF(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height);
    this._bottomRight = new PointF(bounds.X + bounds.Width, bounds.Y + bounds.Height);
    this._center = new PointF(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f);
  }

  internal PointF TopLeft => this._topLeft;

  internal PointF Top => this._top;

  internal PointF TopRight => this._topRight;

  internal PointF Left => this._left;

  internal PointF Right => this._right;

  internal PointF BottomLeft => this._bottomLeft;

  internal PointF Bottom => this._bottom;

  internal PointF BottomRight => this._bottomRight;

  internal PointF Center => this._center;

  internal RectangleF OuterBounds => this._outerBounds;
}
