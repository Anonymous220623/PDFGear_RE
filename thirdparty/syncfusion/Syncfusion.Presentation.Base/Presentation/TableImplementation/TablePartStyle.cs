// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.TableImplementation.TablePartStyle
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.TableImplementation;

internal class TablePartStyle
{
  private TableTextStyle _tableTextStyle;
  private TableCellStyle _tableCellStyle;
  private TableStyle _parent;

  internal TablePartStyle(TableStyle tableStyle) => this._parent = tableStyle;

  internal TableStyle Parent => this._parent;

  internal TableTextStyle TableTextStyle
  {
    get => this._tableTextStyle;
    set => this._tableTextStyle = value;
  }

  internal TableCellStyle TableCellStyle
  {
    get => this._tableCellStyle;
    set => this._tableCellStyle = value;
  }

  internal void Close() => this.ClearAll();

  private void ClearAll()
  {
    if (this._tableCellStyle != null)
    {
      this._tableCellStyle.Close();
      this._tableCellStyle = (TableCellStyle) null;
    }
    if (this._tableTextStyle == null)
      return;
    this._tableTextStyle.Close();
    this._tableTextStyle = (TableTextStyle) null;
  }

  public TablePartStyle Clone()
  {
    TablePartStyle tablePartStyle = (TablePartStyle) this.MemberwiseClone();
    if (this._tableCellStyle != null)
    {
      tablePartStyle._tableCellStyle = this._tableCellStyle.Clone();
      tablePartStyle._tableCellStyle.SetParent(tablePartStyle);
    }
    if (this._tableTextStyle != null)
    {
      tablePartStyle._tableTextStyle = this._tableTextStyle.Clone();
      tablePartStyle._tableTextStyle.SetParent(tablePartStyle);
    }
    return tablePartStyle;
  }

  internal void SetParent(TableStyle tableStyle) => this._parent = tableStyle;
}
