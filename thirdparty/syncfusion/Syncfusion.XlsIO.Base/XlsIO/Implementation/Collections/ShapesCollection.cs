// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.ShapesCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class ShapesCollection : ShapeCollectionBase, IShapes, IEnumerable, IParentApplication
{
  public const string DefaultChartNameStart = "Chart ";
  public const string DefaultTextBoxNameStart = "TextBox ";
  public const string DefaultCheckBoxNameStart = "CheckBox ";
  public const string DefaultOptionButtonNameStart = "Option Button ";
  public const string DefaultComboBoxNameStart = "Drop Down ";
  public const string DefaultPictureNameStart = "Picture ";
  internal const string DefaultOleObjectNameStart = "Object ";
  internal const string DefaultGroupShapeNameStart = "Group ";
  internal CommentsCollection m_comments;

  public ShapesCollection(IApplication application, object parent)
    : base(application, parent)
  {
  }

  [CLSCompliant(false)]
  public ShapesCollection(
    IApplication application,
    object parent,
    MsofbtSpgrContainer container,
    ExcelParseOptions options)
    : base(application, parent, container, options)
  {
  }

  protected override void InitializeCollection()
  {
    base.InitializeCollection();
    if (!(this.m_sheet is WorksheetImpl))
      return;
    this.m_comments = new CommentsCollection(this.Application, (object) this);
  }

  public IComments Comments => (IComments) this.m_comments;

  public CommentsCollection InnerComments => this.m_comments;

  public override TBIFFRecord RecordCode => TBIFFRecord.MSODrawing;

  public override WorkbookShapeDataImpl ShapeData => this.Workbook.ShapesData;

  public IPictureShape AddPicture(Image image, string pictureName, ExcelImageFormat imageFormat)
  {
    IPictureShape pictureShape = (IPictureShape) this.AddPicture(this.ShapeData.AddPicture(image, imageFormat, pictureName), pictureName);
    pictureShape.Height = (int) Math.Round((double) image.Height * ApplicationImpl.ConvertToPixels(1.0, MeasureUnits.Inch) / (double) image.VerticalResolution);
    pictureShape.Width = (int) Math.Round((double) image.Width * ApplicationImpl.ConvertToPixels(1.0, MeasureUnits.Inch) / (double) image.HorizontalResolution);
    if (image is Bitmap && Array.IndexOf<int>(image.PropertyIdList, 274) > -1)
    {
      switch (image.GetPropertyItem(274).Value[0])
      {
        case 3:
          pictureShape.ShapeRotation = 180;
          break;
        case 6:
          pictureShape.ShapeRotation = 90;
          break;
        case 8:
          pictureShape.ShapeRotation = 270;
          break;
      }
    }
    pictureShape.Name = pictureName;
    return pictureShape;
  }

  internal BitmapShapeImpl AddPictureAsLink(string strPictureName)
  {
    BitmapShapeImpl picture = new BitmapShapeImpl(this.Application, (object) this);
    picture.FileName = strPictureName;
    picture.ShapeType = ExcelShapeType.Picture;
    this.Add((IShape) picture);
    picture.IsSizeWithCell = false;
    picture.IsMoveWithCell = true;
    (this.m_sheet.Pictures as PicturesCollection).AddPicture((IPictureShape) picture);
    return picture;
  }

  public IPictureShape AddPicture(string fileName)
  {
    return this.AddPicture(Image.FromFile(fileName), Path.GetFileNameWithoutExtension(fileName), ExcelImageFormat.Original);
  }

  public ICommentShape AddComment(string commentText) => this.AddComment(commentText, true);

  public ICommentShape AddComment(string commentText, bool bIsParseOptions)
  {
    CommentShapeImpl commentShapeImpl = this.AppImplementation.CreateCommentShapeImpl((object) this, bIsParseOptions);
    commentShapeImpl.RichText.Text = commentText;
    return this.AddShape((ShapeImpl) commentShapeImpl) as ICommentShape;
  }

  public ICommentShape AddComment() => this.AddComment(string.Empty);

  public IChartShape AddChart()
  {
    ChartShapeImpl newShape = new ChartShapeImpl(this.Application, (object) this);
    newShape.Name = CollectionBaseEx<IShape>.GenerateDefaultName((ICollection<IShape>) this.List, "Chart ");
    this.AddShape((ShapeImpl) newShape);
    return (IChartShape) newShape;
  }

  public ITextBoxShapeEx AddTextBox()
  {
    TextBoxShapeImpl textBoxShapeImpl = this.AppImplementation.CreateTextBoxShapeImpl(this, this.m_sheet as WorksheetImpl);
    this.AddShape((ShapeImpl) textBoxShapeImpl);
    this.m_sheet.TypedTextBoxes.AddTextBox((ITextBoxShape) textBoxShapeImpl);
    textBoxShapeImpl.Name = CollectionBaseEx<IShape>.GenerateDefaultName((ICollection<IShape>) this, "TextBox ");
    return (ITextBoxShapeEx) textBoxShapeImpl;
  }

  public ICheckBoxShape AddCheckBox()
  {
    CheckBoxShapeImpl checkBoxShapeImpl = this.AppImplementation.CreateCheckBoxShapeImpl((object) this);
    this.AddShape((ShapeImpl) checkBoxShapeImpl);
    this.m_sheet.TypedCheckBoxes.AddCheckBox((ICheckBoxShape) checkBoxShapeImpl);
    checkBoxShapeImpl.Name = CollectionBaseEx<IShape>.GenerateDefaultName((ICollection<IShape>) this, "CheckBox ");
    return (ICheckBoxShape) checkBoxShapeImpl;
  }

  public IOptionButtonShape AddOptionButton()
  {
    OptionButtonShapeImpl optionButtonShapeImpl = this.AppImplementation.CreateOptionButtonShapeImpl((object) this);
    this.AddShape((ShapeImpl) optionButtonShapeImpl);
    this.m_sheet.TypedOptionButtons.AddOptionButton((IOptionButtonShape) optionButtonShapeImpl);
    optionButtonShapeImpl.Name = CollectionBaseEx<IShape>.GenerateDefaultName((ICollection<IShape>) this, "Option Button ");
    return (IOptionButtonShape) optionButtonShapeImpl;
  }

  public IComboBoxShape AddComboBox()
  {
    ComboBoxShapeImpl comboBoxShapeImpl = this.AppImplementation.CreateComboBoxShapeImpl((object) this);
    this.AddShape((ShapeImpl) comboBoxShapeImpl);
    this.m_sheet.TypedComboBoxes.AddComboBox((IComboBoxShape) comboBoxShapeImpl);
    comboBoxShapeImpl.Name = CollectionBaseEx<IShape>.GenerateDefaultName((ICollection<IShape>) this, "Drop Down ");
    return (IComboBoxShape) comboBoxShapeImpl;
  }

  public void RegenerateComboBoxNames()
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      if (this[index] is ComboBoxShapeImpl comboBoxShapeImpl)
      {
        string name = comboBoxShapeImpl.Name;
        if (name == null || name.Length == 0)
          comboBoxShapeImpl.Name = CollectionBaseEx<IShape>.GenerateDefaultName((ICollection<IShape>) this, "Drop Down ");
      }
      (this[index] as CommentShapeImpl)?.CheckLeftOffset();
    }
  }

  [CLSCompliant(false)]
  protected override ShapeImpl CreateShape(
    TObjType objType,
    MsofbtSpContainer shapeContainer,
    ExcelParseOptions options,
    System.Collections.Generic.List<ObjSubRecord> subRecords,
    int cmoIndex)
  {
    ShapeImpl shape = (ShapeImpl) null;
    switch (objType)
    {
      case TObjType.otChart:
        shape = (ShapeImpl) new ChartShapeImpl(this.Application, (object) this, shapeContainer, options);
        if (string.IsNullOrEmpty(shape.Name))
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
          (this.m_sheet.Pictures as PicturesCollection).AddPicture(shape as IPictureShape);
          break;
        }
        break;
      case TObjType.otCheckBox:
        CheckBoxShapeImpl checkbox = new CheckBoxShapeImpl(this.Application, (object) this, shapeContainer, options);
        this.m_sheet.TypedCheckBoxes.AddCheckBox((ICheckBoxShape) checkbox);
        shape = (ShapeImpl) checkbox;
        break;
      case TObjType.otOptionBtn:
        OptionButtonShapeImpl optionButton = new OptionButtonShapeImpl(this.Application, (object) this, shapeContainer, options, cmoIndex);
        this.m_sheet.TypedOptionButtons.AddOptionButton((IOptionButtonShape) optionButton);
        shape = (ShapeImpl) optionButton;
        break;
      case TObjType.otComboBox:
        ComboBoxShapeImpl combobox = new ComboBoxShapeImpl(this.Application, (object) this, shapeContainer, options, subRecords);
        if (combobox.ComboType == ExcelComboType.Regular)
          this.m_sheet.TypedComboBoxes.AddComboBox((IComboBoxShape) combobox);
        shape = (ShapeImpl) combobox;
        break;
      case TObjType.otComment:
        shape = (ShapeImpl) this.AppImplementation.CreateCommentShapeImpl((object) this, shapeContainer, options);
        this.m_comments.AddComment((ICommentShape) shape);
        break;
    }
    return shape;
  }

  private ShapeImpl ChoosePictureShape(
    MsofbtSpContainer shapeContainer,
    ExcelParseOptions options,
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

  public FormControlShapeImpl AddFormControlShape()
  {
    FormControlShapeImpl newShape = new FormControlShapeImpl(this.Application, (object) this);
    this.AddShape((ShapeImpl) newShape);
    return newShape;
  }

  public void InnerRemoveComment(ICommentShape comment)
  {
    if (comment == null)
      throw new ArgumentNullException(nameof (comment));
    this.InnerList.Remove((IShape) comment);
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
      int num = iIndex + iCount;
      if (bRemove)
      {
        if ((bRow ? (inner.TopRow < iIndex ? 0 : (inner.BottomRow < num ? 1 : 0)) : (inner.LeftColumn < iIndex ? 0 : (inner.RightColumn < num ? 1 : 0))) != 0)
          inner.Remove();
        else
          inner.RemoveRowColumn(iIndex, iCount, bRow);
      }
      else
        inner.InsertRowColumn(iIndex, iCount, bRow);
    }
    ((AutoFiltersCollection) this.Worksheet?.AutoFilters)?.UpdateFilterRange();
  }

  public BitmapShapeImpl AddPicture(int iBlipId, string strPictureName)
  {
    BitmapShapeImpl picture = new BitmapShapeImpl(this.Application, (object) this);
    picture.FileName = strPictureName;
    picture.ShapeType = ExcelShapeType.Picture;
    picture.BlipId = (uint) iBlipId;
    this.Add((IShape) picture);
    picture.IsSizeWithCell = false;
    picture.IsMoveWithCell = true;
    (this.m_sheet.Pictures as PicturesCollection).AddPicture((IPictureShape) picture);
    return picture;
  }

  public void AddPicture(BitmapShapeImpl shape)
  {
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    this.Add((IShape) shape);
    (this.m_sheet.Pictures as PicturesCollection).AddPicture((IPictureShape) shape);
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
    ShapesCollection parent1 = parent != null ? (ShapesCollection) base.Clone(parent) : throw new ArgumentNullException(nameof (parent));
    if (this.m_comments != null)
      parent1.m_comments = (CommentsCollection) this.m_comments.Clone((object) parent1);
    System.Collections.Generic.List<IShape> innerList = parent1.InnerList;
    return (object) parent1;
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
    if (rec.Y < recDest.Y)
      list = this.SortingBList(list);
    if (rec.Y > recDest.Y)
      list = this.SortingTList(list);
    if (rec.X < recDest.X)
      list = this.SortingRList(list);
    if (rec.X > recDest.X)
      list = this.SortingLList(list);
    for (int index = list.Count - 1; index >= 0; --index)
    {
      ShapeImpl shapeImpl = (ShapeImpl) list[index];
      Rectangle newPosition;
      if (shapeImpl.CanCopyShapesOnRangeCopy(rec, recDest, out newPosition))
        shapeImpl.CopyMoveShapeOnRangeCopyMove(destSheet, newPosition, bIsCopy);
    }
  }

  public void SetVersion(ExcelVersion version)
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

  private IList<IShape> SortingTList(IList<IShape> list)
  {
    for (int index1 = 0; index1 <= list.Count - 1; ++index1)
    {
      for (int index2 = index1 + 1; index2 <= list.Count - 1; ++index2)
      {
        int num1;
        int num2;
        if (list[index1] is CommentShapeImpl)
        {
          CommentShapeImpl commentShapeImpl = list[index1] as CommentShapeImpl;
          num1 = commentShapeImpl.Row;
          num2 = commentShapeImpl.Column;
        }
        else
        {
          ShapeImpl shapeImpl = list[index1] as ShapeImpl;
          num1 = shapeImpl.TopRow;
          num2 = shapeImpl.LeftColumn;
        }
        int num3;
        int num4;
        if (list[index2] is CommentShapeImpl)
        {
          CommentShapeImpl commentShapeImpl = list[index2] as CommentShapeImpl;
          num3 = commentShapeImpl.Row;
          num4 = commentShapeImpl.Column;
        }
        else
        {
          ShapeImpl shapeImpl = list[index2] as ShapeImpl;
          num3 = shapeImpl.TopRow;
          num4 = shapeImpl.LeftColumn;
        }
        if (num2 == num4 && num1 < num3)
        {
          IShape shape = list[index1];
          list[index1] = list[index2];
          list[index2] = shape;
        }
      }
    }
    return list;
  }

  private IList<IShape> SortingBList(IList<IShape> list)
  {
    for (int index1 = 0; index1 <= list.Count - 1; ++index1)
    {
      for (int index2 = index1 + 1; index2 <= list.Count - 1; ++index2)
      {
        int num1;
        int num2;
        if (list[index1] is CommentShapeImpl)
        {
          CommentShapeImpl commentShapeImpl = list[index1] as CommentShapeImpl;
          num1 = commentShapeImpl.Row;
          num2 = commentShapeImpl.Column;
        }
        else
        {
          ShapeImpl shapeImpl = list[index1] as ShapeImpl;
          num1 = shapeImpl.TopRow;
          num2 = shapeImpl.LeftColumn;
        }
        int num3;
        int num4;
        if (list[index2] is CommentShapeImpl)
        {
          CommentShapeImpl commentShapeImpl = list[index2] as CommentShapeImpl;
          num3 = commentShapeImpl.Row;
          num4 = commentShapeImpl.Column;
        }
        else
        {
          ShapeImpl shapeImpl = list[index2] as ShapeImpl;
          num3 = shapeImpl.TopRow;
          num4 = shapeImpl.LeftColumn;
        }
        if (num2 == num4 && num1 > num3)
        {
          IShape shape = list[index1];
          list[index1] = list[index2];
          list[index2] = shape;
        }
      }
    }
    return list;
  }

  private IList<IShape> SortingLList(IList<IShape> list)
  {
    for (int index1 = 0; index1 <= list.Count - 1; ++index1)
    {
      for (int index2 = index1 + 1; index2 <= list.Count - 1; ++index2)
      {
        int num1;
        int num2;
        if (list[index1] is CommentShapeImpl)
        {
          CommentShapeImpl commentShapeImpl = list[index1] as CommentShapeImpl;
          num1 = commentShapeImpl.Row;
          num2 = commentShapeImpl.Column;
        }
        else
        {
          ShapeImpl shapeImpl = list[index1] as ShapeImpl;
          num1 = shapeImpl.TopRow;
          num2 = shapeImpl.LeftColumn;
        }
        int num3;
        int num4;
        if (list[index2] is CommentShapeImpl)
        {
          CommentShapeImpl commentShapeImpl = list[index2] as CommentShapeImpl;
          num3 = commentShapeImpl.Row;
          num4 = commentShapeImpl.Column;
        }
        else
        {
          ShapeImpl shapeImpl = list[index2] as ShapeImpl;
          num3 = shapeImpl.TopRow;
          num4 = shapeImpl.LeftColumn;
        }
        if (num1 == num3 && num2 < num4)
        {
          IShape shape = list[index1];
          list[index1] = list[index2];
          list[index2] = shape;
        }
      }
    }
    return list;
  }

  private IList<IShape> SortingRList(IList<IShape> list)
  {
    for (int index1 = 0; index1 <= list.Count - 1; ++index1)
    {
      for (int index2 = index1 + 1; index2 <= list.Count - 1; ++index2)
      {
        int num1;
        int num2;
        if (list[index1] is CommentShapeImpl)
        {
          CommentShapeImpl commentShapeImpl = list[index1] as CommentShapeImpl;
          num1 = commentShapeImpl.Row;
          num2 = commentShapeImpl.Column;
        }
        else
        {
          ShapeImpl shapeImpl = list[index1] as ShapeImpl;
          num1 = shapeImpl.TopRow;
          num2 = shapeImpl.LeftColumn;
        }
        int num3;
        int num4;
        if (list[index2] is CommentShapeImpl)
        {
          CommentShapeImpl commentShapeImpl = list[index2] as CommentShapeImpl;
          num3 = commentShapeImpl.Row;
          num4 = commentShapeImpl.Column;
        }
        else
        {
          ShapeImpl shapeImpl = list[index2] as ShapeImpl;
          num3 = shapeImpl.TopRow;
          num4 = shapeImpl.LeftColumn;
        }
        if (num1 == num3 && num2 > num4)
        {
          IShape shape = list[index1];
          list[index1] = list[index2];
          list[index2] = shape;
        }
      }
    }
    return list;
  }

  public IShape GetShapeById(int id)
  {
    ShapeImpl shapeById = (ShapeImpl) null;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      ShapeImpl groupShape = (ShapeImpl) this[index];
      if (groupShape.ShapeType == ExcelShapeType.Group)
      {
        shapeById = this.GetShapeFromGroupShape(groupShape as GroupShapeImpl, id);
        if (shapeById != null)
          break;
      }
      if (groupShape.ShapeId == id)
      {
        shapeById = groupShape;
        break;
      }
    }
    return (IShape) shapeById;
  }

  internal IShape GetOLEShapeById(int id)
  {
    ShapeImpl oleShapeById = (ShapeImpl) null;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      if (this[index] is BitmapShapeImpl)
      {
        ShapeImpl shapeImpl = (ShapeImpl) this[index];
        if (shapeImpl.ShapeId == id)
        {
          oleShapeById = shapeImpl;
          break;
        }
      }
    }
    return (IShape) oleShapeById;
  }

  private ShapeImpl GetShapeFromGroupShape(GroupShapeImpl groupShape, int id)
  {
    ShapeImpl shapeFromGroupShape = (ShapeImpl) null;
    if (groupShape.Items != null)
    {
      int length = groupShape.Items.Length;
      for (int index = 0; index < length; ++index)
      {
        ShapeImpl groupShape1 = groupShape.Items[index] as ShapeImpl;
        if (groupShape1.ShapeType == ExcelShapeType.Group)
        {
          shapeFromGroupShape = this.GetShapeFromGroupShape(groupShape1 as GroupShapeImpl, id);
          if (shapeFromGroupShape != null)
            break;
        }
        if (groupShape1.ShapeId == id)
        {
          shapeFromGroupShape = groupShape1;
          break;
        }
      }
    }
    return shapeFromGroupShape;
  }

  public IShape AddAutoShapes(
    AutoShapeType autoShapeType,
    int topRow,
    int leftColumn,
    int height,
    int width)
  {
    return this.AddAutoShapes(autoShapeType, topRow, 0, leftColumn, 0, height, width);
  }

  public IGroupShape Group(IShape[] groupItems)
  {
    if (groupItems == null || groupItems.Length < 2)
      throw new ArgumentOutOfRangeException("Group items must contains at least two items.");
    GroupShapeImpl groupShape = new GroupShapeImpl(this.Application, (object) this);
    groupShape.ShapeType = ExcelShapeType.Group;
    groupShape.Name = CollectionBaseEx<IShape>.GenerateDefaultName((ICollection<IShape>) this, "Group ");
    GroupShapeImpl newShape = this.Group(groupShape, groupItems, true);
    this.AddShape((ShapeImpl) newShape);
    return (IGroupShape) newShape;
  }

  internal GroupShapeImpl Group(GroupShapeImpl groupShape, IShape[] groupItems, bool isRemove)
  {
    bool flag = false;
    double leftDouble = (groupItems[0] as ShapeImpl).LeftDouble;
    double topDouble = (groupItems[0] as ShapeImpl).TopDouble;
    double num1 = (groupItems[0] as ShapeImpl).LeftDouble + (groupItems[0] as ShapeImpl).WidthDouble;
    double num2 = (groupItems[0] as ShapeImpl).TopDouble + (groupItems[0] as ShapeImpl).HeightDouble;
    for (int index = 0; index < groupItems.Length; ++index)
    {
      ShapeImpl groupItem = groupItems[index] as ShapeImpl;
      flag = groupItem.EnableAlternateContent;
      groupItem.ChangeParent((object) groupShape);
      if (groupItem.LeftDouble < leftDouble)
        leftDouble = groupItem.LeftDouble;
      if (groupItem.TopDouble < topDouble)
        topDouble = groupItem.TopDouble;
      if (groupItem.LeftDouble + groupItem.WidthDouble > num1)
        num1 = groupItem.LeftDouble + groupItem.WidthDouble;
      if (groupItem.TopDouble + groupItem.HeightDouble > num2)
        num2 = groupItem.TopDouble + groupItem.HeightDouble;
      if (isRemove)
        this.Remove((IShape) groupItem);
    }
    groupShape.LeftDouble = leftDouble;
    groupShape.TopDouble = topDouble;
    groupShape.WidthDouble = num1 - leftDouble;
    groupShape.HeightDouble = num2 - topDouble;
    groupShape.ShapeFrame.SetChildAnchor(groupShape.ShapeFrame.OffsetX, groupShape.ShapeFrame.OffsetY, groupShape.ShapeFrame.OffsetCX, groupShape.ShapeFrame.OffsetCY);
    if (flag)
      groupShape.EnableAlternateContent = flag;
    groupShape.Items = groupItems;
    return groupShape;
  }

  public void Ungroup(IGroupShape groupShape) => this.Ungroup(groupShape, false);

  public void Ungroup(IGroupShape groupShape, bool isAll)
  {
    if (!this.Contains((IShape) groupShape))
      return;
    (groupShape as GroupShapeImpl).LayoutGroupShape(isAll);
    this.UngroupShapes(groupShape, isAll, true);
  }

  private void UngroupShapes(IGroupShape groupShape, bool isAll, bool IsRemove)
  {
    bool flipVertical = (groupShape as GroupShapeImpl).FlipVertical;
    bool flipHorizontal = (groupShape as GroupShapeImpl).FlipHorizontal;
    for (int index = 0; index < groupShape.Items.Length; ++index)
    {
      ShapeImpl newShape = groupShape.Items[index] as ShapeImpl;
      if (flipVertical || flipHorizontal)
      {
        switch (newShape)
        {
          case AutoShapeImpl _:
            if (flipVertical)
              (newShape as AutoShapeImpl).ShapeExt.FlipVertical = !(newShape as AutoShapeImpl).ShapeExt.FlipVertical;
            if (flipHorizontal)
            {
              (newShape as AutoShapeImpl).ShapeExt.FlipHorizontal = !(newShape as AutoShapeImpl).ShapeExt.FlipHorizontal;
              break;
            }
            break;
          case TextBoxShapeImpl _:
            if (flipVertical)
              (newShape as TextBoxShapeImpl).FlipVertical = !(newShape as TextBoxShapeImpl).FlipVertical;
            if (flipHorizontal)
            {
              (newShape as TextBoxShapeImpl).FlipHorizontal = !(newShape as TextBoxShapeImpl).FlipHorizontal;
              break;
            }
            break;
          case BitmapShapeImpl _:
            if (flipVertical)
              (newShape as BitmapShapeImpl).FlipVertical = !(newShape as BitmapShapeImpl).FlipVertical;
            if (flipHorizontal)
            {
              (newShape as BitmapShapeImpl).FlipHorizontal = !(newShape as BitmapShapeImpl).FlipHorizontal;
              break;
            }
            break;
          case GroupShapeImpl _:
            if (flipVertical)
              (newShape as GroupShapeImpl).FlipVertical = !(newShape as GroupShapeImpl).FlipVertical;
            if (flipHorizontal)
            {
              (newShape as GroupShapeImpl).FlipHorizontal = !(newShape as GroupShapeImpl).FlipHorizontal;
              break;
            }
            break;
        }
      }
      if (isAll && newShape is GroupShapeImpl)
      {
        this.UngroupShapes(newShape as IGroupShape, isAll, false);
      }
      else
      {
        newShape.ChangeParent((object) this);
        if (newShape.GroupFrame != null)
        {
          newShape.SetPostion(newShape.GroupFrame.OffsetX, newShape.GroupFrame.OffsetY, newShape.GroupFrame.OffsetCX, newShape.GroupFrame.OffsetCY);
          newShape.ShapeRotation = newShape.GroupFrame.Rotation / 60000;
          newShape.ShapeFrame.SetAnchor(newShape.ShapeRotation * 60000, newShape.GroupFrame.OffsetX, newShape.GroupFrame.OffsetY, newShape.GroupFrame.OffsetCX, newShape.GroupFrame.OffsetCY);
        }
        this.AddShape(newShape);
      }
    }
    if (!IsRemove)
      return;
    this.Remove((IShape) groupShape);
  }

  internal GroupShapeImpl AddGroupShape(GroupShapeImpl groupShape, ShapeImpl[] shapes)
  {
    bool flag = false;
    for (int index = 0; index < shapes.Length; ++index)
    {
      ShapeImpl shape = shapes[index];
      flag = shape.EnableAlternateContent;
      shape.ChangeParent((object) groupShape);
      this.Remove((IShape) shape);
    }
    if (flag)
      groupShape.EnableAlternateContent = flag;
    groupShape.Items = (IShape[]) shapes;
    this.AddShape((ShapeImpl) groupShape);
    return groupShape;
  }

  internal new void Remove(IShape shape) => base.Remove(shape);

  internal IShape AddAutoShapes(
    AutoShapeType autoShapeType,
    int topRow,
    int topRowOffset,
    int leftColumn,
    int leftColumnOffset,
    int height,
    int width)
  {
    AutoShapeImpl newShape = new AutoShapeImpl(this.Application, (object) this);
    WorksheetBaseImpl sheet = this.m_sheet;
    string autoShapeName = ShapesCollection.GetAutoShapeName(autoShapeType);
    newShape.CreateShape(autoShapeType, sheet);
    newShape.ShapeExt.IsCreated = true;
    if (!(this.Parent is WorksheetImpl))
      newShape.ShapeExt.AnchorType = AnchorType.RelSize;
    newShape.TopRow = topRow;
    newShape.TopRowOffset = topRowOffset;
    newShape.LeftColumn = leftColumn;
    newShape.LeftColumnOffset = leftColumnOffset;
    newShape.HeightDouble = (double) (height * (this.Application as ApplicationImpl).GetdpiY()) / 96.0;
    newShape.WidthDouble = (double) (width * (this.Application as ApplicationImpl).GetdpiX()) / 96.0;
    int autoShapeCount = 0;
    for (int index = 0; index < sheet.Shapes.Count; ++index)
    {
      if (sheet.Shapes[index].ShapeType == ExcelShapeType.AutoShape)
        ++autoShapeCount;
    }
    newShape.Name = ShapesCollection.GenerateDefaultAutoShapeName(autoShapeName, autoShapeCount);
    newShape.EvaluateTopLeftPosition();
    newShape.ShapeExt.ParentSheet = this.Parent as WorksheetBaseImpl;
    this.AddShape((ShapeImpl) newShape);
    return (IShape) newShape;
  }

  private static string GenerateDefaultAutoShapeName(string strStart, int autoShapeCount)
  {
    int num = autoShapeCount + 1;
    return $"{strStart} {num.ToString()}";
  }

  private static string GetAutoShapeName(AutoShapeType autoShapeType)
  {
    string autoShapeName = string.Empty;
    switch (autoShapeType)
    {
      case AutoShapeType.Unknown:
        autoShapeName = "Unknown";
        break;
      case AutoShapeType.Rectangle:
        autoShapeName = "Rectangle";
        break;
      case AutoShapeType.Parallelogram:
        autoShapeName = "Parallelogram";
        break;
      case AutoShapeType.Trapezoid:
        autoShapeName = "Trapezoid";
        break;
      case AutoShapeType.Diamond:
        autoShapeName = "Diamond";
        break;
      case AutoShapeType.RoundedRectangle:
        autoShapeName = "Rectangle: Rounded Corners";
        break;
      case AutoShapeType.Octagon:
        autoShapeName = "Octagon";
        break;
      case AutoShapeType.IsoscelesTriangle:
        autoShapeName = "Isosceles Triangle";
        break;
      case AutoShapeType.RightTriangle:
        autoShapeName = "Right Triangle";
        break;
      case AutoShapeType.Oval:
        autoShapeName = "Oval";
        break;
      case AutoShapeType.Hexagon:
        autoShapeName = "Hexagon";
        break;
      case AutoShapeType.Cross:
        autoShapeName = "Cross";
        break;
      case AutoShapeType.RegularPentagon:
        autoShapeName = "Pentagon";
        break;
      case AutoShapeType.Can:
        autoShapeName = "Cylinder";
        break;
      case AutoShapeType.Cube:
        autoShapeName = "Cube";
        break;
      case AutoShapeType.Bevel:
        autoShapeName = "Rectangle: Beveled";
        break;
      case AutoShapeType.FoldedCorner:
        autoShapeName = "Rectangle: Folded Corner";
        break;
      case AutoShapeType.SmileyFace:
        autoShapeName = "Smiley Face";
        break;
      case AutoShapeType.Donut:
        autoShapeName = "Circle: Hollow";
        break;
      case AutoShapeType.NoSymbol:
        autoShapeName = "\"Not Allowed\" Symbol";
        break;
      case AutoShapeType.BlockArc:
        autoShapeName = "Block Arc";
        break;
      case AutoShapeType.Heart:
        autoShapeName = "Heart";
        break;
      case AutoShapeType.LightningBolt:
        autoShapeName = "Lightning Bolt";
        break;
      case AutoShapeType.Sun:
        autoShapeName = "Sun";
        break;
      case AutoShapeType.Moon:
        autoShapeName = "Moon";
        break;
      case AutoShapeType.Arc:
        autoShapeName = "Arc";
        break;
      case AutoShapeType.DoubleBracket:
        autoShapeName = "Double Bracket";
        break;
      case AutoShapeType.DoubleBrace:
        autoShapeName = "Double Brace";
        break;
      case AutoShapeType.Plaque:
        autoShapeName = "Plaque";
        break;
      case AutoShapeType.LeftBracket:
        autoShapeName = "Left Bracket";
        break;
      case AutoShapeType.RightBracket:
        autoShapeName = "Right Bracket";
        break;
      case AutoShapeType.LeftBrace:
        autoShapeName = "Left Brace";
        break;
      case AutoShapeType.RightBrace:
        autoShapeName = "Right Brace";
        break;
      case AutoShapeType.RightArrow:
        autoShapeName = "Arrow: Right";
        break;
      case AutoShapeType.LeftArrow:
        autoShapeName = "Arrow: Left";
        break;
      case AutoShapeType.UpArrow:
        autoShapeName = "Arrow: Up";
        break;
      case AutoShapeType.DownArrow:
        autoShapeName = "Arrow: Down";
        break;
      case AutoShapeType.LeftRightArrow:
        autoShapeName = "Arrow: Left-Right";
        break;
      case AutoShapeType.UpDownArrow:
        autoShapeName = "Arrow: Up-Down";
        break;
      case AutoShapeType.QuadArrow:
        autoShapeName = "Arrow: Quad";
        break;
      case AutoShapeType.LeftRightUpArrow:
        autoShapeName = "Arrow: Left-Right-Up";
        break;
      case AutoShapeType.BentArrow:
        autoShapeName = "Arrow: Bent";
        break;
      case AutoShapeType.UTurnArrow:
        autoShapeName = "Arrow: U-Turn";
        break;
      case AutoShapeType.LeftUpArrow:
        autoShapeName = "Arrow: Left-Up";
        break;
      case AutoShapeType.BentUpArrow:
        autoShapeName = "Arrow: Bent-UP";
        break;
      case AutoShapeType.CurvedRightArrow:
        autoShapeName = "Arrow: Curved Right";
        break;
      case AutoShapeType.CurvedLeftArrow:
        autoShapeName = "Arrow: Curved Left";
        break;
      case AutoShapeType.CurvedUpArrow:
        autoShapeName = "Arrow: Curved Up";
        break;
      case AutoShapeType.CurvedDownArrow:
        autoShapeName = "Arrow: Curved Down";
        break;
      case AutoShapeType.StripedRightArrow:
        autoShapeName = "Arrow: Striped Right";
        break;
      case AutoShapeType.NotchedRightArrow:
        autoShapeName = "Arrow: Notched Right";
        break;
      case AutoShapeType.Pentagon:
        autoShapeName = "Arrow: Pentagon";
        break;
      case AutoShapeType.Chevron:
        autoShapeName = "Chevron";
        break;
      case AutoShapeType.RightArrowCallout:
        autoShapeName = "Callout: Right Arrow";
        break;
      case AutoShapeType.LeftArrowCallout:
        autoShapeName = "Callout: Left Arrow";
        break;
      case AutoShapeType.UpArrowCallout:
        autoShapeName = "Callout: Up Arrow";
        break;
      case AutoShapeType.DownArrowCallout:
        autoShapeName = "Callout: Down Arrow";
        break;
      case AutoShapeType.LeftRightArrowCallout:
        autoShapeName = "Callout: Left-Right Arrow";
        break;
      case AutoShapeType.UpDownArrowCallout:
        autoShapeName = "Callout: Up-Down Arrow";
        break;
      case AutoShapeType.QuadArrowCallout:
        autoShapeName = "Callout: Quad Arrow";
        break;
      case AutoShapeType.CircularArrow:
        autoShapeName = "Arrow: Circular";
        break;
      case AutoShapeType.FlowChartProcess:
        autoShapeName = "Flowchart: Process";
        break;
      case AutoShapeType.FlowChartAlternateProcess:
        autoShapeName = "Flowchart: Alternate Process";
        break;
      case AutoShapeType.FlowChartDecision:
        autoShapeName = "Flowchart: Decision";
        break;
      case AutoShapeType.FlowChartData:
        autoShapeName = "Flowchart: Data";
        break;
      case AutoShapeType.FlowChartPredefinedProcess:
        autoShapeName = "Flowchart: Predefined Process";
        break;
      case AutoShapeType.FlowChartInternalStorage:
        autoShapeName = "Flowchart: Internal Storage";
        break;
      case AutoShapeType.FlowChartDocument:
        autoShapeName = "Flowchart: Document";
        break;
      case AutoShapeType.FlowChartMultiDocument:
        autoShapeName = "Flowchart: Multidocument";
        break;
      case AutoShapeType.FlowChartTerminator:
        autoShapeName = "Flowchart: Terminator";
        break;
      case AutoShapeType.FlowChartPreparation:
        autoShapeName = "Flowchart: Preparation";
        break;
      case AutoShapeType.FlowChartManualInput:
        autoShapeName = "Flowchart: Manual Input";
        break;
      case AutoShapeType.FlowChartManualOperation:
        autoShapeName = "Flowchart: Manual Operation";
        break;
      case AutoShapeType.FlowChartConnector:
        autoShapeName = "Flowchart: Connector";
        break;
      case AutoShapeType.FlowChartOffPageConnector:
        autoShapeName = "Flowchart: Off-page Connector";
        break;
      case AutoShapeType.FlowChartCard:
        autoShapeName = "Flowchart: Card";
        break;
      case AutoShapeType.FlowChartPunchedTape:
        autoShapeName = "Flowchart: Punched Tape";
        break;
      case AutoShapeType.FlowChartSummingJunction:
        autoShapeName = "Flowchart: Summing Junction";
        break;
      case AutoShapeType.FlowChartOr:
        autoShapeName = "Flowchart: Or";
        break;
      case AutoShapeType.FlowChartCollate:
        autoShapeName = "Flowchart: Collate";
        break;
      case AutoShapeType.FlowChartSort:
        autoShapeName = "Flowchart: Sort";
        break;
      case AutoShapeType.FlowChartExtract:
        autoShapeName = "Flowchart: Extract";
        break;
      case AutoShapeType.FlowChartMerge:
        autoShapeName = "Flowchart: Merge";
        break;
      case AutoShapeType.FlowChartStoredData:
        autoShapeName = "Flowchart: Stored Data";
        break;
      case AutoShapeType.FlowChartDelay:
        autoShapeName = "Flowchart: Delay";
        break;
      case AutoShapeType.FlowChartSequentialAccessStorage:
        autoShapeName = "Flowchart: Sequential Access Storage";
        break;
      case AutoShapeType.FlowChartMagneticDisk:
        autoShapeName = "Flowchart: Magnetic Disk";
        break;
      case AutoShapeType.FlowChartDirectAccessStorage:
        autoShapeName = "Flowchart: Direct Access Storage";
        break;
      case AutoShapeType.FlowChartDisplay:
        autoShapeName = "Flowchart: Display";
        break;
      case AutoShapeType.Explosion1:
        autoShapeName = "Explosion: 8 Points";
        break;
      case AutoShapeType.Explosion2:
        autoShapeName = "Explosion: 14 Points";
        break;
      case AutoShapeType.Star4Point:
        autoShapeName = "Star: 4 Points";
        break;
      case AutoShapeType.Star5Point:
        autoShapeName = "Star: 5 Points";
        break;
      case AutoShapeType.Star8Point:
        autoShapeName = "Star: 8 Points";
        break;
      case AutoShapeType.Star16Point:
        autoShapeName = "Star: 16 Points";
        break;
      case AutoShapeType.Star24Point:
        autoShapeName = "Star: 24 Points";
        break;
      case AutoShapeType.Star32Point:
        autoShapeName = "Star: 32 Points";
        break;
      case AutoShapeType.UpRibbon:
        autoShapeName = "Ribbon: Tilted Up";
        break;
      case AutoShapeType.DownRibbon:
        autoShapeName = "Ribbon: Tilted Down";
        break;
      case AutoShapeType.CurvedUpRibbon:
        autoShapeName = "Ribbon: Curved and Tilted Up";
        break;
      case AutoShapeType.CurvedDownRibbon:
        autoShapeName = "Ribbon: Curved and Tilted Down";
        break;
      case AutoShapeType.VerticalScroll:
        autoShapeName = "Scroll: Vertical";
        break;
      case AutoShapeType.HorizontalScroll:
        autoShapeName = "Scroll: Horizontal";
        break;
      case AutoShapeType.Wave:
        autoShapeName = "Wave";
        break;
      case AutoShapeType.DoubleWave:
        autoShapeName = "Double Wave";
        break;
      case AutoShapeType.RectangularCallout:
        autoShapeName = "Speech Bubble: Rectangle";
        break;
      case AutoShapeType.RoundedRectangularCallout:
        autoShapeName = "Speech Bubble: Rectangle with Corners Rounded";
        break;
      case AutoShapeType.OvalCallout:
        autoShapeName = "Speech Bubble: Oval";
        break;
      case AutoShapeType.CloudCallout:
        autoShapeName = "Thought Bubble: Cloud";
        break;
      case AutoShapeType.LineCallout1:
        autoShapeName = "Callout: Line";
        break;
      case AutoShapeType.LineCallout2:
        autoShapeName = "Callout: Bent Line";
        break;
      case AutoShapeType.LineCallout3:
        autoShapeName = "Callout: Double Bent Line";
        break;
      case AutoShapeType.LineCallout1NoBorder:
        autoShapeName = "Callout: Line with No Border";
        break;
      case AutoShapeType.LineCallout1AccentBar:
        autoShapeName = "Callout: Line with Accent Bar";
        break;
      case AutoShapeType.LineCallout2AccentBar:
        autoShapeName = "Callout: Bent Line with Accent Bar";
        break;
      case AutoShapeType.LineCallout3AccentBar:
        autoShapeName = "Callout: Double Bent Line with Accent Bar";
        break;
      case AutoShapeType.LineCallout2NoBorder:
        autoShapeName = "Callout: Bent Line with No Border";
        break;
      case AutoShapeType.LineCallout3NoBorder:
        autoShapeName = "Callout: Double Bent Line with No Border";
        break;
      case AutoShapeType.LineCallout1BorderAndAccentBar:
        autoShapeName = "Callout: Line with Border and Accent Bar";
        break;
      case AutoShapeType.LineCallout2BorderAndAccentBar:
        autoShapeName = "Callout: Bent Line with Border and Accent Bar";
        break;
      case AutoShapeType.LineCallout3BorderAndAccentBar:
        autoShapeName = "Callout: Double Bent Line with Border and Accent Bar";
        break;
      case AutoShapeType.DiagonalStripe:
        autoShapeName = "Diagonal Stripe";
        break;
      case AutoShapeType.Pie:
        autoShapeName = "Partial Circle";
        break;
      case AutoShapeType.Decagon:
        autoShapeName = "Decagon";
        break;
      case AutoShapeType.Heptagon:
        autoShapeName = "Heptagon";
        break;
      case AutoShapeType.Dodecagon:
        autoShapeName = "Dodecagon";
        break;
      case AutoShapeType.Star6Point:
        autoShapeName = "Star: 6 Points";
        break;
      case AutoShapeType.Star7Point:
        autoShapeName = "Star: 7 Points";
        break;
      case AutoShapeType.Star10Point:
        autoShapeName = "Star: 10 Points";
        break;
      case AutoShapeType.Star12Point:
        autoShapeName = "Star: 12 Points";
        break;
      case AutoShapeType.RoundSingleCornerRectangle:
        autoShapeName = "Rectangle: Single Corner Rounded";
        break;
      case AutoShapeType.RoundSameSideCornerRectangle:
        autoShapeName = "Rectangle: Top Corners Rounded";
        break;
      case AutoShapeType.RoundDiagonalCornerRectangle:
        autoShapeName = "Rectangle: Diagonal Corners Rounded";
        break;
      case AutoShapeType.SnipAndRoundSingleCornerRectangle:
        autoShapeName = "Rectangle: Top Corners One Rounded and One Snipped";
        break;
      case AutoShapeType.SnipSingleCornerRectangle:
        autoShapeName = "Rectangle: Single Corner Snipped";
        break;
      case AutoShapeType.SnipSameSideCornerRectangle:
        autoShapeName = "Rectangle: Top Corners Snipped";
        break;
      case AutoShapeType.SnipDiagonalCornerRectangle:
        autoShapeName = "Rectangle: Diagonal Corners Snipped";
        break;
      case AutoShapeType.Frame:
        autoShapeName = "Frame";
        break;
      case AutoShapeType.HalfFrame:
        autoShapeName = "Half Frame";
        break;
      case AutoShapeType.Teardrop:
        autoShapeName = "Teardrop";
        break;
      case AutoShapeType.Chord:
        autoShapeName = "Chord";
        break;
      case AutoShapeType.L_Shape:
        autoShapeName = "L-Shape";
        break;
      case AutoShapeType.MathPlus:
        autoShapeName = "Plus Sign";
        break;
      case AutoShapeType.MathMinus:
        autoShapeName = "Minus Sign";
        break;
      case AutoShapeType.MathMultiply:
        autoShapeName = "Multiplication Sign";
        break;
      case AutoShapeType.MathDivision:
        autoShapeName = "Division Sign";
        break;
      case AutoShapeType.MathEqual:
        autoShapeName = "Equals";
        break;
      case AutoShapeType.MathNotEqual:
        autoShapeName = "Not Equal";
        break;
      case AutoShapeType.Cloud:
        autoShapeName = "Cloud";
        break;
      case AutoShapeType.Line:
        autoShapeName = "Line";
        break;
      case AutoShapeType.StraightConnector:
        autoShapeName = "Straight Connector";
        break;
      case AutoShapeType.ElbowConnector:
        autoShapeName = "Connector: Elbow";
        break;
      case AutoShapeType.CurvedConnector:
        autoShapeName = "Connector: Curved";
        break;
      case AutoShapeType.BentConnector2:
        autoShapeName = "Bent Connector 2";
        break;
      case AutoShapeType.BentConnector4:
        autoShapeName = "Bent Connector 4";
        break;
      case AutoShapeType.BentConnector5:
        autoShapeName = "Bent Connector 5";
        break;
      case AutoShapeType.CurvedConnector2:
        autoShapeName = "Connector: Curved 2";
        break;
      case AutoShapeType.CurvedConnector4:
        autoShapeName = "Curved Connector 4";
        break;
      case AutoShapeType.CurvedConnector5:
        autoShapeName = "Curved Connector 5";
        break;
    }
    return autoShapeName;
  }
}
