// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.TableImplementation.TableStyle
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;

#nullable disable
namespace Syncfusion.Presentation.TableImplementation;

internal class TableStyle
{
  private string _styleId;
  private string _styleName;
  private Fill _tableBackgroundFill;
  private TablePartStyle _wholeTable;
  private TablePartStyle _band1Horz;
  private TablePartStyle _band2Horz;
  private TablePartStyle _band1Vert;
  private TablePartStyle _band2Vert;
  private TablePartStyle _lastCol;
  private TablePartStyle _firstCol;
  private TablePartStyle _lastRow;
  private TablePartStyle _firstRow;
  private Syncfusion.Presentation.Presentation _parent;
  private string _bgRefIndex;
  private ColorObject _bgRefColor;
  private bool _isCustom;

  internal TableStyle(Syncfusion.Presentation.Presentation presentation)
  {
    this._parent = presentation;
  }

  internal Syncfusion.Presentation.Presentation Presentation => this._parent;

  internal bool IsCustom
  {
    get => this._isCustom;
    set => this._isCustom = value;
  }

  internal TablePartStyle LastColumn
  {
    get => this._lastCol;
    set => this._lastCol = value;
  }

  internal Fill TableBackgroundFill
  {
    get => this._tableBackgroundFill;
    set => this._tableBackgroundFill = value;
  }

  internal TablePartStyle FirstColumn
  {
    get => this._firstCol;
    set => this._firstCol = value;
  }

  internal TablePartStyle LastRow
  {
    get => this._lastRow;
    set => this._lastRow = value;
  }

  internal string Id
  {
    get => this._styleId;
    set => this._styleId = value;
  }

  internal string Name
  {
    get => this._styleName;
    set => this._styleName = value;
  }

  internal TablePartStyle WholeTableStyle
  {
    get => this._wholeTable;
    set => this._wholeTable = value;
  }

  internal TablePartStyle HorizontalBand1Style
  {
    get => this._band1Horz;
    set => this._band1Horz = value;
  }

  internal TablePartStyle HorizontalBand2Style
  {
    get => this._band2Horz;
    set => this._band2Horz = value;
  }

  internal TablePartStyle VerticalBand1Style
  {
    get => this._band1Vert;
    set => this._band1Vert = value;
  }

  internal TablePartStyle VerticalBand2Style
  {
    get => this._band2Vert;
    set => this._band2Vert = value;
  }

  internal TablePartStyle FirstRow
  {
    get => this._firstRow;
    set => this._firstRow = value;
  }

  internal string BgRefIndex
  {
    get => this._bgRefIndex;
    set => this._bgRefIndex = value;
  }

  internal ColorObject BgRefColor
  {
    get => this._bgRefColor;
    set => this._bgRefColor = value;
  }

  internal void Close() => this.ClearAll();

  private void ClearAll()
  {
    if (this._tableBackgroundFill != null)
    {
      this._tableBackgroundFill.Close();
      this._tableBackgroundFill = (Fill) null;
    }
    if (this._band1Horz != null)
    {
      this._band1Horz.Close();
      this._band1Horz = (TablePartStyle) null;
    }
    if (this._band1Vert != null)
    {
      this._band1Vert.Close();
      this._band1Vert = (TablePartStyle) null;
    }
    if (this._band2Horz != null)
    {
      this._band2Horz.Close();
      this._band2Horz = (TablePartStyle) null;
    }
    if (this._band2Vert != null)
    {
      this._band2Vert.Close();
      this._band2Vert = (TablePartStyle) null;
    }
    if (this._bgRefColor != null)
    {
      this._bgRefColor.Close();
      this._bgRefColor = (ColorObject) null;
    }
    if (this._firstCol != null)
    {
      this._firstCol.Close();
      this._firstCol = (TablePartStyle) null;
    }
    if (this._firstRow != null)
    {
      this._firstRow.Close();
      this._firstRow = (TablePartStyle) null;
    }
    if (this._lastCol != null)
    {
      this._lastCol.Close();
      this._lastCol = (TablePartStyle) null;
    }
    if (this._lastRow != null)
    {
      this._lastRow.Close();
      this._lastRow = (TablePartStyle) null;
    }
    if (this._wholeTable != null)
    {
      this._wholeTable.Close();
      this._wholeTable = (TablePartStyle) null;
    }
    this._parent = (Syncfusion.Presentation.Presentation) null;
  }

  public TableStyle Clone()
  {
    TableStyle tableStyle = (TableStyle) this.MemberwiseClone();
    if (this._band1Horz != null)
    {
      tableStyle._band1Horz = this._band1Horz.Clone();
      tableStyle._band1Horz.SetParent(tableStyle);
    }
    if (this._band1Vert != null)
    {
      tableStyle._band1Vert = this._band1Vert.Clone();
      tableStyle._band1Vert.SetParent(tableStyle);
    }
    if (this._band2Horz != null)
    {
      tableStyle._band2Horz = this._band2Horz.Clone();
      tableStyle._band2Horz.SetParent(tableStyle);
    }
    if (this._band2Vert != null)
    {
      tableStyle._band2Vert = this._band2Vert.Clone();
      tableStyle._band2Vert.SetParent(tableStyle);
    }
    if (this._bgRefColor != null)
      tableStyle._bgRefColor = this._bgRefColor.CloneColorObject();
    if (this._firstCol != null)
    {
      tableStyle._firstCol = this._firstCol.Clone();
      tableStyle._firstCol.SetParent(tableStyle);
    }
    if (this._firstRow != null)
    {
      tableStyle._firstRow = this._firstRow.Clone();
      tableStyle._firstRow.SetParent(tableStyle);
    }
    if (this._lastCol != null)
    {
      tableStyle._lastCol = this._lastCol.Clone();
      tableStyle._lastCol.SetParent(tableStyle);
    }
    if (this._lastRow != null)
    {
      tableStyle._lastRow = this._lastRow.Clone();
      tableStyle._lastRow.SetParent(tableStyle);
    }
    if (this._tableBackgroundFill != null)
      tableStyle._tableBackgroundFill = this._tableBackgroundFill.Clone();
    if (this._wholeTable != null)
    {
      tableStyle._wholeTable = this._wholeTable.Clone();
      tableStyle._wholeTable.SetParent(tableStyle);
    }
    return tableStyle;
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    this._parent = presentation;
    if (this._tableBackgroundFill == null)
      return;
    this._tableBackgroundFill.SetParent((object) presentation);
  }
}
