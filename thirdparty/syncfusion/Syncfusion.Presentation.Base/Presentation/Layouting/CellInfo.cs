// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Layouting.CellInfo
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.TableImplementation;
using System.Drawing;

#nullable disable
namespace Syncfusion.Presentation.Layouting;

internal class CellInfo
{
  private Cell _cell;
  private RectangleF _bounds;
  private RectangleF _textLayoutingBounds;
  private float _totalTextHeight;
  private float _maxTextWidth;

  internal CellInfo(Cell cell) => this._cell = cell;

  internal Cell Cell => this._cell;

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

  internal float TotalTextHeight
  {
    get => this._totalTextHeight;
    set => this._totalTextHeight = value;
  }

  internal float MaxTextWidth
  {
    get => this._maxTextWidth;
    set => this._maxTextWidth = value;
  }

  internal void UpdateBoundsHeight(float value) => this._bounds.Height = value;

  internal void UpdateTextLayoutingBoundsHeight(float value)
  {
    this._textLayoutingBounds.Height = value;
  }

  internal void UpdateBoundsTop(float value) => this._bounds.Y = value;

  internal void UpdateTextLayoutingBoundsTop(float value) => this._textLayoutingBounds.Y = value;

  internal CellInfo Clone(Cell newParent)
  {
    CellInfo cellInfo = (CellInfo) this.MemberwiseClone();
    cellInfo._bounds = this._bounds;
    cellInfo._cell = newParent;
    cellInfo._maxTextWidth = this._maxTextWidth;
    cellInfo._textLayoutingBounds = this._textLayoutingBounds;
    cellInfo._totalTextHeight = this._totalTextHeight;
    return cellInfo;
  }
}
