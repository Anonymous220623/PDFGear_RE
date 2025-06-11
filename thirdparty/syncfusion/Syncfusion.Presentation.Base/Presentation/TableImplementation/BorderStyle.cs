// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.TableImplementation.BorderStyle
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;

#nullable disable
namespace Syncfusion.Presentation.TableImplementation;

internal class BorderStyle
{
  private LineFormat _left;
  private LineFormat _right;
  private LineFormat _top;
  private LineFormat _bottom;
  private LineFormat _insideH;
  private LineFormat _insideV;
  private TableCellStyle _parent;

  internal BorderStyle(TableCellStyle tableCellStyle) => this._parent = tableCellStyle;

  internal TableCellStyle Parent => this._parent;

  internal LineFormat Left
  {
    get => this._left;
    set => this._left = value;
  }

  internal LineFormat Right
  {
    get => this._right;
    set => this._right = value;
  }

  internal LineFormat Top
  {
    get => this._top;
    set => this._top = value;
  }

  internal LineFormat Bottom
  {
    get => this._bottom;
    set => this._bottom = value;
  }

  internal LineFormat InsideHorzBorder
  {
    get => this._insideH;
    set => this._insideH = value;
  }

  internal LineFormat InsideVertBorder
  {
    get => this._insideV;
    set => this._insideV = value;
  }

  public void Close() => this.ClearAll();

  private void ClearAll()
  {
    if (this._bottom != null)
    {
      this._bottom.Close();
      this._bottom = (LineFormat) null;
    }
    if (this._insideH != null)
    {
      this._insideH.Close();
      this._insideH = (LineFormat) null;
    }
    if (this._insideV != null)
    {
      this._insideV.Close();
      this._insideV = (LineFormat) null;
    }
    if (this._top != null)
    {
      this._top.Close();
      this._top = (LineFormat) null;
    }
    if (this._left != null)
    {
      this._left.Close();
      this._left = (LineFormat) null;
    }
    if (this._right == null)
      return;
    this._right.Close();
    this._right = (LineFormat) null;
  }

  public BorderStyle Clone()
  {
    BorderStyle borderStyle = (BorderStyle) this.MemberwiseClone();
    if (this._bottom != null)
      borderStyle._bottom = this._bottom.Clone();
    if (this._insideH != null)
      borderStyle._insideH = this._insideH.Clone();
    if (this._insideV != null)
      borderStyle._insideV = this._insideV.Clone();
    if (this._left != null)
      borderStyle._left = this._left.Clone();
    if (this._right != null)
      borderStyle._right = this._right.Clone();
    if (this._top != null)
      borderStyle._top = this._top.Clone();
    return borderStyle;
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    if (this._bottom != null)
      this._bottom.SetParent(presentation);
    if (this._insideH != null)
      this._insideH.SetParent(presentation);
    if (this._insideV != null)
      this._insideV.SetParent(presentation);
    if (this._left != null)
      this._left.SetParent(presentation);
    if (this._right != null)
      this._right.SetParent(presentation);
    if (this._top == null)
      return;
    this._top.SetParent(presentation);
  }
}
