// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.TableImplementation.TableCellStyle
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;

#nullable disable
namespace Syncfusion.Presentation.TableImplementation;

internal class TableCellStyle
{
  private BorderStyle _cellBorderStyle;
  private Fill _fill;
  private TablePartStyle _parent;
  private string _fillRefIndex;
  private ColorObject _fillRefColor;

  internal TableCellStyle(TablePartStyle tablePartStyle) => this._parent = tablePartStyle;

  internal TablePartStyle Parent => this._parent;

  internal BorderStyle CellBorderStyle
  {
    get => this._cellBorderStyle;
    set => this._cellBorderStyle = value;
  }

  internal Fill Fill
  {
    get => this._fill;
    set => this._fill = value;
  }

  internal string FillRefIndex
  {
    get => this._fillRefIndex;
    set => this._fillRefIndex = value;
  }

  internal ColorObject FillRefColor
  {
    get => this._fillRefColor;
    set => this._fillRefColor = value;
  }

  internal void Close() => this.ClearAll();

  private void ClearAll()
  {
    if (this._fill != null)
    {
      this._fill.Close();
      this._fill = (Fill) null;
    }
    if (this._cellBorderStyle != null)
    {
      this._cellBorderStyle.Close();
      this._cellBorderStyle = (BorderStyle) null;
    }
    if (this._fillRefColor == null)
      return;
    this._fillRefColor.Close();
    this._fillRefColor = (ColorObject) null;
  }

  public TableCellStyle Clone()
  {
    TableCellStyle tableCellStyle = (TableCellStyle) this.MemberwiseClone();
    if (this._cellBorderStyle != null)
      tableCellStyle._cellBorderStyle = this._cellBorderStyle.Clone();
    if (this._fill != null)
      tableCellStyle._fill = this._fill.Clone();
    if (this._fillRefColor != null)
      tableCellStyle._fillRefColor = this._fillRefColor.CloneColorObject();
    return tableCellStyle;
  }

  internal void SetParent(TablePartStyle tablePartStyle)
  {
    this._parent = tablePartStyle;
    if (this._fill != null)
      this._fill.SetParent((object) this._parent.Parent.Presentation);
    if (this._cellBorderStyle == null)
      return;
    this._cellBorderStyle.SetParent(this._parent.Parent.Presentation);
  }
}
