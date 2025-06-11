// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Layouting.ShapeInfo
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Presentation.Layouting;

internal class ShapeInfo
{
  private IShape _shape;
  private RectangleF _bounds;
  private RectangleF _textLayoutingBounds;
  private List<ColumnInfo> _columnsInfo;

  internal ShapeInfo(IShape shape) => this._shape = shape;

  internal Shape Shape => (Shape) this._shape;

  internal RectangleF Bounds
  {
    get => this._bounds;
    set => this._bounds = value;
  }

  internal RectangleF TextLayoutingBounds
  {
    get => this._textLayoutingBounds;
    set => this._textLayoutingBounds = value;
  }

  internal List<ColumnInfo> ColumnsInfo
  {
    get => this._columnsInfo;
    set => this._columnsInfo = value;
  }

  internal ShapeInfo Clone()
  {
    ShapeInfo shapeInfo = (ShapeInfo) this.MemberwiseClone();
    shapeInfo._textLayoutingBounds = this._textLayoutingBounds;
    return shapeInfo;
  }

  internal void SetParent(Shape shape) => this._shape = (IShape) shape;
}
