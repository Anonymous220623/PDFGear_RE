// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.ShapesCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class ShapesCollection : ShapeCollectionBase, IShapes, IEnumerable, IParentApplication
{
  public const string DefaultChartNameStart = "Chart ";
  public const string DefaultTextBoxNameStart = "TextBox ";
  public const string DefaultCheckBoxNameStart = "CheckBox ";
  public const string DefaultOptionButtonNameStart = "Option Button ";
  public const string DefaultComboBoxNameStart = "Drop Down ";
  public const string DefaultPictureNameStart = "Picture ";

  public ShapesCollection(IApplication application, object parent)
    : base(application, parent)
  {
  }

  [CLSCompliant(false)]
  public ShapesCollection(
    IApplication application,
    object parent,
    MsofbtSpgrContainer container,
    OfficeParseOptions options)
    : base(application, parent, container, options)
  {
  }

  protected override void InitializeCollection() => base.InitializeCollection();

  public override TBIFFRecord RecordCode => TBIFFRecord.MSODrawing;

  public override WorkbookShapeDataImpl ShapeData => this.Workbook.ShapesData;

  public IOfficeChartShape AddChart()
  {
    ChartShapeImpl newShape = new ChartShapeImpl(this.Application, (object) this);
    newShape.Name = CollectionBaseEx<IShape>.GenerateDefaultName((ICollection<IShape>) this.List, "Chart ");
    this.AddShape((ShapeImpl) newShape);
    return (IOfficeChartShape) newShape;
  }

  public ITextBoxShapeEx AddTextBox()
  {
    TextBoxShapeImpl textBoxShapeImpl = this.AppImplementation.CreateTextBoxShapeImpl(this, this.m_sheet as WorksheetImpl);
    this.AddShape((ShapeImpl) textBoxShapeImpl);
    this.m_sheet.TypedTextBoxes.AddTextBox((ITextBoxShape) textBoxShapeImpl);
    textBoxShapeImpl.Name = CollectionBaseEx<IShape>.GenerateDefaultName((ICollection<IShape>) this, "TextBox ");
    return (ITextBoxShapeEx) textBoxShapeImpl;
  }

  public void RegenerateComboBoxNames()
  {
  }

  [CLSCompliant(false)]
  protected override ShapeImpl CreateShape(
    TObjType objType,
    MsofbtSpContainer shapeContainer,
    OfficeParseOptions options,
    System.Collections.Generic.List<ObjSubRecord> subRecords,
    int cmoIndex)
  {
    ShapeImpl shape = (ShapeImpl) null;
    switch (objType)
    {
      case TObjType.otChart:
        shape = (ShapeImpl) new ChartShapeImpl(this.Application, (object) this, shapeContainer, options);
        string name = shape.Name;
        if (name == null || name.Length == 0)
        {
          shape.Name = CollectionBaseEx<IShape>.GenerateDefaultName((ICollection<IShape>) this, "Chart ");
          break;
        }
        break;
      case TObjType.otText:
        TextBoxShapeImpl textbox = new TextBoxShapeImpl(this.Application, (object) this, shapeContainer, options);
        this.m_sheet.TypedTextBoxes.AddTextBox((ITextBoxShape) textbox);
        shape = (ShapeImpl) textbox;
        break;
      case TObjType.otPicture:
        if (shape == null)
        {
          shape = (ShapeImpl) new BitmapShapeImpl(this.Application, (object) this, shapeContainer);
          break;
        }
        break;
    }
    return shape;
  }

  private ShapeImpl ChoosePictureShape(
    MsofbtSpContainer shapeContainer,
    OfficeParseOptions options,
    System.Collections.Generic.List<ObjSubRecord> subRecords,
    int cmoIndex)
  {
    ShapeImpl shapeImpl = (ShapeImpl) null;
    int index = cmoIndex;
    for (int count = subRecords.Count; index < count; ++index)
    {
      ObjSubRecord subRecord = subRecords[index];
      if (subRecord.Type == TObjSubRecordType.ftPictFmla)
      {
        switch (((ftPictFmla) subRecord).Formula)
        {
          case "Forms.TextBox.1":
            TextBoxShapeImpl textbox = new TextBoxShapeImpl(this.Application, (object) this, shapeContainer, options);
            this.m_sheet.TypedTextBoxes.AddTextBox((ITextBoxShape) textbox);
            shapeImpl = (ShapeImpl) textbox;
            goto label_6;
          default:
            goto label_6;
        }
      }
    }
label_6:
    return shapeImpl;
  }

  public bool CanInsertRowColumn(int iIndex, int iCount, bool bRow, int iMaxIndex)
  {
    for (int index = this.Count - 1; index >= 0; --index)
    {
      if (!((ShapeImpl) this.InnerList[index]).CanInsertRowColumn(iIndex, iCount, bRow, iMaxIndex))
        return false;
    }
    return true;
  }

  public void InsertRemoveRowColumn(int iIndex, int iCount, bool bRow, bool bRemove)
  {
    for (int index = this.Count - 1; index >= 0; --index)
    {
      ShapeImpl inner = (ShapeImpl) this.InnerList[index];
      if (bRemove)
        inner.RemoveRowColumn(iIndex, iCount, bRow);
      else
        inner.InsertRowColumn(iIndex, iCount, bRow);
    }
    WorksheetImpl worksheet = this.Worksheet;
  }

  public BitmapShapeImpl AddPicture(int iBlipId, string strPictureName)
  {
    BitmapShapeImpl bitmapShapeImpl = new BitmapShapeImpl(this.Application, (object) this);
    bitmapShapeImpl.FileName = strPictureName;
    bitmapShapeImpl.ShapeType = OfficeShapeType.Picture;
    bitmapShapeImpl.BlipId = (uint) iBlipId;
    this.Add((IShape) bitmapShapeImpl);
    bitmapShapeImpl.IsSizeWithCell = false;
    bitmapShapeImpl.IsMoveWithCell = true;
    return bitmapShapeImpl;
  }

  public void AddPicture(BitmapShapeImpl shape)
  {
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    this.Add((IShape) shape);
  }

  public void UpdateFormula(
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect)
  {
    IList<IShape> innerList = (IList<IShape>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      ((ShapeImpl) innerList[index]).UpdateFormula(iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect);
  }

  public override object Clone(object parent)
  {
    ShapesCollection shapesCollection = parent != null ? (ShapesCollection) base.Clone(parent) : throw new ArgumentNullException(nameof (parent));
    System.Collections.Generic.List<IShape> innerList = shapesCollection.InnerList;
    return (object) shapesCollection;
  }

  public void CopyMoveShapeOnRangeCopy(
    WorksheetImpl destSheet,
    Rectangle rec,
    Rectangle recDest,
    bool bIsCopy)
  {
    if (destSheet == null)
      throw new ArgumentNullException(nameof (destSheet));
    IList<IShape> list = this.List;
    for (int index = list.Count - 1; index >= 0; --index)
    {
      ShapeImpl shapeImpl = (ShapeImpl) list[index];
      Rectangle newPosition;
      if (shapeImpl.CanCopyShapesOnRangeCopy(rec, recDest, out newPosition))
        shapeImpl.CopyMoveShapeOnRangeCopyMove(destSheet, newPosition, bIsCopy);
    }
  }

  public void SetVersion(OfficeVersion version)
  {
    int iRows;
    int iColumns;
    UtilityMethods.GetMaxRowColumnCount(out iRows, out iColumns, version);
    for (int index = this.Count - 1; index >= 0; --index)
    {
      ShapeImpl shapeImpl = (ShapeImpl) this[index];
      if (shapeImpl.LeftColumn > iColumns || shapeImpl.RightColumn > iColumns || shapeImpl.TopRow > iRows || shapeImpl.BottomRow > iRows)
        shapeImpl.Remove();
      else if (shapeImpl.Name == null || shapeImpl.Name.Length == 0)
        shapeImpl.GenerateDefaultName();
    }
  }

  internal void UpdateNamedRangeIndexes(int[] arrNewIndex)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      (this[index] as ShapeImpl).UpdateNamedRangeIndexes(arrNewIndex);
  }

  internal void UpdateNamedRangeIndexes(IDictionary<int, int> dicNewIndex)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      (this[index] as ShapeImpl).UpdateNamedRangeIndexes(dicNewIndex);
  }

  public IShape GetShapeById(int id)
  {
    ShapeImpl shapeById = (ShapeImpl) null;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      ShapeImpl shapeImpl = (ShapeImpl) this[index];
      if (shapeImpl.ShapeId == id)
      {
        shapeById = shapeImpl;
        break;
      }
    }
    return (IShape) shapeById;
  }

  public IShape AddAutoShapes(
    AutoShapeType autoShapeType,
    int topRow,
    int leftColumn,
    int height,
    int width)
  {
    return this.AddAutoShapes(autoShapeType, topRow - 1, 0, leftColumn - 1, 0, height, width);
  }

  internal IShape AddAutoShapes(
    AutoShapeType autoShapeType,
    int topRow,
    int top,
    int leftColumn,
    int left,
    int height,
    int width)
  {
    AutoShapeImpl newShape = new AutoShapeImpl(this.Application, (object) this);
    WorksheetImpl activeSheet = this.Application.ActiveSheet as WorksheetImpl;
    newShape.CreateShape(autoShapeType, activeSheet);
    newShape.ShapeExt.IsCreated = true;
    newShape.ShapeExt.ClientAnchor.SetAnchor(topRow, top, leftColumn, left, height, width);
    this.AddShape((ShapeImpl) newShape);
    return (IShape) newShape;
  }
}
