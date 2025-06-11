// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Shapes.CommentShapeImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Drawing;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Shapes;

public class CommentShapeImpl : 
  TextBoxShapeBase,
  ICommentShape,
  IComment,
  ITextBox,
  IShape,
  IParentApplication
{
  internal const int ShapeInstance = 202;
  private const int DEF_SHAPE_VERSION = 2;
  private const int DEF_OPTIONS_VERSION = 3;
  private const int DEF_OPTIONS_INSTANCE = 10;
  public const int DEF_OFFSET = 240 /*0xF0*/;
  private const int DEF_COMMENT_SHADOWED = 196611 /*0x030003*/;
  private const int DEF_COMMENT_SHOW_ALWAYS = 131072 /*0x020000*/;
  private const int DEF_COMMENT_NOT_SHOW_ALWAYS = 131074 /*0x020002*/;
  private int m_iRow;
  private int m_iColumn;
  private bool m_bVisible;
  private string m_strAuthor;
  private TextBodyPropertiesHolder textBodyPropertiesHolder;

  public CommentShapeImpl(IApplication application, object parent)
    : this(application, parent, true)
  {
    this.InitializeVariables();
    this.ShapeType = ExcelShapeType.Comment;
    this.m_bUpdateLineFill = true;
    this.Fill.ForeColor = ShapeFillImpl.DEF_COMENT_PARSE_COLOR;
    this.Line.ForeColor = ColorExtension.Black;
    this.Fill.BackColor = ColorExtension.Empty;
    this.Line.BackColor = ColorExtension.Empty;
    this.Fill.Transparency = 1.0;
    this.m_strAuthor = this.m_shapes.Worksheet.Workbook.Author;
    this.IsMoveWithCell = false;
    this.IsSizeWithCell = false;
  }

  public CommentShapeImpl(IApplication application, object parent, bool bIsParseOptions)
    : base(application, parent)
  {
    this.InitializeVariables();
    this.ShapeType = ExcelShapeType.Comment;
    RichTextString richText = this.RichText as RichTextString;
    if (!this.ParentWorkbook.Loading && richText != null)
    {
      IFont font = this.Workbook.CreateFont();
      font.FontName = "Tahoma";
      font.Size = 9.0;
      this.Workbook.AddFont(font);
      richText.DefaultFont = (font as FontWrapper).Wrapped;
      richText.TextObject.DefaultFontIndex = (font as FontWrapper).Wrapped.Index;
    }
    if (bIsParseOptions)
    {
      this.m_bUpdateLineFill = true;
      this.Fill.ForeColor = ShapeFillImpl.DEF_COMENT_PARSE_COLOR;
      this.Line.ForeColor = ColorExtension.Black;
      this.Fill.BackColor = ColorExtension.Empty;
      this.Line.BackColor = ColorExtension.Empty;
    }
    this.m_strAuthor = this.m_shapes.Worksheet.Workbook.Author;
    this.IsMoveWithCell = false;
    this.IsSizeWithCell = false;
    this.FillColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 225);
  }

  public CommentShapeImpl(IApplication application, object parent, string commentText)
    : this(application, parent)
  {
    this.RichText.Text = commentText;
  }

  [CLSCompliant(false)]
  public CommentShapeImpl(IApplication application, object parent, MsofbtSpContainer container)
    : this(application, parent, container, ExcelParseOptions.Default)
  {
  }

  [CLSCompliant(false)]
  public CommentShapeImpl(
    IApplication application,
    object parent,
    MsofbtSpContainer container,
    ExcelParseOptions options)
    : base(application, parent, container, options)
  {
    this.ShapeType = ExcelShapeType.Comment;
    this.m_bSupportOptions = true;
    this.m_bUpdateLineFill = true;
    List<MsoBase> itemsList = container.ItemsList;
    int index = 0;
    for (int count = itemsList.Count; index < count; ++index)
    {
      if (itemsList[index] is MsofbtClientData)
      {
        this.ParseNoteRecord((int) ((itemsList[index] as MsofbtClientData).ObjectRecord.RecordsList[0] as ftCmo).ID);
        break;
      }
    }
  }

  protected override void InitializeVariables()
  {
    base.InitializeVariables();
    this.VmlShape = true;
    this.IsShapeVisible = false;
    if (!this.Worksheet.IsParsed)
      return;
    this.FillClientAnchor();
  }

  private void FillClientAnchor()
  {
    this.ClientAnchor.Options = (ushort) 3;
    this.ClientAnchor.LeftColumn = this.Column - 1;
    this.ClientAnchor.RightColumn = this.Column + 1;
    this.ClientAnchor.TopRow = this.Row - 1;
    this.ClientAnchor.BottomRow = this.Row + 3;
    this.ClientAnchor.LeftOffset = 240 /*0xF0*/;
    this.ClientAnchor.RightOffset = 240 /*0xF0*/;
    this.ClientAnchor.TopOffset = 240 /*0xF0*/;
    this.ClientAnchor.BottomOffset = 240 /*0xF0*/;
    this.UpdateWidth();
    this.UpdateHeight();
    this.EvaluateTopLeftPosition();
  }

  public override void Dispose()
  {
    base.Dispose();
    if (this.m_graphicFrame != null)
      this.m_graphicFrame.Dispose();
    if (this.m_options != null)
      this.m_options.Dispose();
    if (this.m_shape != null)
      this.m_shape.Dispose();
    this.m_shapes = (ShapeCollectionBase) null;
    GC.SuppressFinalize((object) this);
  }

  public int Row
  {
    get => this.m_iRow + 1;
    set => this.m_iRow = value - 1;
  }

  public int Column
  {
    get => this.m_iColumn + 1;
    set => this.m_iColumn = value - 1;
  }

  public bool IsVisible
  {
    get => this.m_bVisible;
    set
    {
      this.m_bVisible = value;
      this.IsShapeVisible = value;
    }
  }

  public string Author
  {
    get => this.m_strAuthor != null ? this.m_strAuthor : this.Workbook.Author;
    set => this.m_strAuthor = value;
  }

  public override int Instance => this.m_shape == null ? 202 : this.m_shape.Instance;

  public new IHyperLink Hyperlink => throw new NotSupportedException("HyperLink");

  internal TextBodyPropertiesHolder TextBodyPropertiesHolder
  {
    get
    {
      if (this.textBodyPropertiesHolder == null)
        this.textBodyPropertiesHolder = new TextBodyPropertiesHolder();
      return this.textBodyPropertiesHolder;
    }
  }

  public override void RegisterInSubCollection()
  {
    this.m_shapes.WorksheetBase.InnerComments.AddComment((ICommentShape) this);
  }

  public override IShape Clone(
    object parent,
    Dictionary<string, string> hashNewNames,
    Dictionary<int, int> dicFontIndexes,
    bool addToCollections)
  {
    CommentShapeImpl comment = (CommentShapeImpl) base.Clone(parent, hashNewNames, dicFontIndexes, addToCollections);
    comment.IsShapeVisible = this.IsShapeVisible;
    comment.CopyFrom((ShapeImpl) this, hashNewNames, dicFontIndexes);
    comment.CopyCommentOptions(this, dicFontIndexes);
    if (addToCollections)
      comment.Worksheet.InnerComments.AddComment((ICommentShape) comment);
    return (IShape) comment;
  }

  [CLSCompliant(false)]
  protected override void SerializeShape(MsofbtSpgrContainer spgrContainer)
  {
    if (spgrContainer == null)
      throw new ArgumentNullException(nameof (spgrContainer));
    MsofbtSpContainer record1 = (MsofbtSpContainer) MsoFactory.GetRecord(MsoRecords.msofbtSpContainer);
    MsofbtClientData record2 = (MsofbtClientData) MsoFactory.GetRecord(MsoRecords.msofbtClientData);
    ftCmo record3;
    if (this.Obj == null)
    {
      OBJRecord record4 = (OBJRecord) BiffRecordFactory.GetRecord(TBIFFRecord.OBJ);
      record3 = new ftCmo();
      record3.ObjectType = TObjType.otComment;
      record3.Printable = true;
      record3.Locked = true;
      record3.AutoLine = true;
      ftEnd record5 = new ftEnd();
      record4.AddSubRecord((ObjSubRecord) record3);
      record4.AddSubRecord((ObjSubRecord) record5);
      record2.AddRecord((BiffRecordRaw) record4);
    }
    else
    {
      record3 = this.Obj.RecordsList[0] as ftCmo;
      record2.AddRecord((BiffRecordRaw) this.Obj);
    }
    record3.ID = this.OldObjId > 0U ? (ushort) this.OldObjId : (ushort) this.ParentWorkbook.CurrentObjectId;
    record1.AddItem((MsoBase) this.m_shape);
    MsofbtOPT itemToAdd = this.SerializeOptions((MsoBase) record1);
    if (itemToAdd.Properties.Length > 0)
      record1.AddItem((MsoBase) itemToAdd);
    record1.AddItem((MsoBase) this.ClientAnchor);
    record1.AddItem((MsoBase) record2);
    record1.AddItem((MsoBase) this.GetClientTextBoxRecord((MsoBase) record1));
    spgrContainer.AddItem((MsoBase) record1);
    this.SerializeNoteRecord(record3.ID);
  }

  private void SerializeNoteRecord(ushort objId)
  {
    NoteRecord record = (NoteRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Note);
    record.Row = (ushort) this.m_iRow;
    record.Column = (ushort) this.m_iColumn;
    record.AuthorName = this.Author;
    record.IsVisible = this.IsShapeVisible;
    record.ObjId = objId;
    this.m_shapes.Worksheet.AddNote(record);
  }

  [CLSCompliant(false)]
  protected void SerializeTextId(MsofbtOPT options)
  {
    if (options == null)
      throw new ArgumentNullException(nameof (options));
    options.AddOptionsOrReplace(new MsofbtOPT.FOPTE()
    {
      Id = MsoOptions.TextId,
      UInt32Value = 19990000U,
      IsValid = false,
      IsComplex = false
    });
  }

  [CLSCompliant(false)]
  protected void SerializeOption344(MsofbtOPT options)
  {
    if (options == null)
      throw new ArgumentNullException(nameof (options));
    options.AddOptionsOrReplace(new MsofbtOPT.FOPTE()
    {
      Id = (MsoOptions) 344,
      UInt32Value = 0U,
      IsValid = false,
      IsComplex = false
    });
  }

  [CLSCompliant(false)]
  protected override MsofbtOPT CreateDefaultOptions()
  {
    MsofbtOPT defaultOptions = base.CreateDefaultOptions();
    defaultOptions.Version = 3;
    defaultOptions.Instance = 2;
    return defaultOptions;
  }

  [CLSCompliant(false)]
  protected override void SerializeCommentShadow(MsofbtOPT option)
  {
    if (option == null)
      throw new ArgumentNullException(nameof (option));
    ShapeImpl.SerializeForte((IFopteOptionWrapper) option, MsoOptions.ShadowObscured, 196611 /*0x030003*/);
    ShapeImpl.SerializeForte((IFopteOptionWrapper) option, MsoOptions.ForeShadowColor, 0);
    int num = this.IsShapeVisible ? 131072 /*0x020000*/ : 131074 /*0x020002*/;
    ShapeImpl.SerializeForte((IFopteOptionWrapper) option, MsoOptions.CommentShowAlways, num);
  }

  public override bool CanCopyShapesOnRangeCopy(
    Rectangle sourceRec,
    Rectangle destRec,
    out Rectangle newPosition)
  {
    newPosition = new Rectangle(0, 0, 0, 0);
    int row = this.Row;
    int column = this.Column;
    if (row < sourceRec.Top || row > sourceRec.Bottom || column < sourceRec.Left || column > sourceRec.Right)
      return false;
    newPosition.Y = row - sourceRec.Top + destRec.Top;
    newPosition.X = column - sourceRec.Left + destRec.Left;
    newPosition.Width = this.Width;
    newPosition.Height = this.Height;
    return true;
  }

  public override ShapeImpl CopyMoveShapeOnRangeCopyMove(
    WorksheetImpl sheet,
    Rectangle destRec,
    bool bIsCopy)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    WorksheetImpl worksheet = this.ParentShapes.Worksheet;
    IRange range = sheet[destRec.Y, destRec.X];
    Dictionary<int, int> dicFontIndexes = (Dictionary<int, int>) null;
    if (this.Worksheet.Workbook != sheet.Workbook)
      dicFontIndexes = (sheet.Workbook as WorkbookImpl).InnerFonts.AddRange((this.Worksheet.Workbook as WorkbookImpl).InnerFonts);
    CommentShapeImpl commentShapeImpl = (CommentShapeImpl) range.AddComment();
    commentShapeImpl.CopyCommentOptions(this, dicFontIndexes);
    commentShapeImpl.IsShapeVisible = this.IsShapeVisible;
    if (!bIsCopy)
      worksheet.InnerComments.Remove((ICommentShape) this);
    commentShapeImpl.FillClientAnchor();
    commentShapeImpl.Height = this.Height;
    commentShapeImpl.Width = this.Width;
    commentShapeImpl.UpdateRightColumn();
    commentShapeImpl.UpdateBottomRow();
    return (ShapeImpl) commentShapeImpl;
  }

  protected override void UpdateNotSizeNotMoveShape(bool bRow, int index, int iCount)
  {
    if (index == 0)
      return;
    int num1 = bRow ? this.BottomRow : this.RightColumn;
    bool flag = index <= num1;
    --index;
    if (iCount < 0 && (bRow && index <= this.m_iRow && index - iCount > this.m_iRow || !bRow && index <= this.m_iColumn && index - iCount > this.m_iColumn))
    {
      this.Remove();
    }
    else
    {
      if ((!bRow || index > this.m_iRow) && (bRow || index > this.m_iColumn))
        return;
      int iRow = this.m_iRow;
      int iColumn = this.m_iColumn;
      int num2 = bRow ? iRow + iCount : iRow;
      int num3 = bRow ? iColumn : iColumn + iCount;
      this.m_iRow = num2;
      this.m_iColumn = num3;
      ((ShapesCollection) this.m_shapes).InnerComments.ReRegisterOnAccess = true;
      if (!flag)
        return;
      if (bRow)
      {
        int num4 = this.ClientAnchor.TopRow + iCount;
        if (num4 > 0)
          this.ClientAnchor.TopRow = num4;
        this.UpdateBottomRow();
      }
      else
      {
        int num5 = this.ClientAnchor.LeftColumn + iCount;
        if (num5 > 0)
          this.ClientAnchor.LeftColumn = num5;
        if (iCount == 1)
          this.UpdateRightColumn(iCount);
        else
          this.UpdateRightColumn();
      }
    }
  }

  protected override void OnDelete()
  {
    base.OnDelete();
    this.m_shapes.WorksheetBase.InnerComments.InnerRemove((ICommentShape) this);
  }

  protected override void CreateDefaultFillLineFormats()
  {
    this.m_bSupportOptions = true;
    base.CreateDefaultFillLineFormats();
    this.Fill.ForeColor = ShapeFillImpl.DEF_COMENT_PARSE_COLOR;
    this.Line.ForeColor = ColorExtension.Black;
    this.Fill.BackColor = ColorExtension.Empty;
    this.Line.BackColor = ColorExtension.Empty;
  }

  public void CopyCommentOptions(
    CommentShapeImpl sourceComment,
    Dictionary<int, int> dicFontIndexes)
  {
    if (sourceComment == null)
      throw new ArgumentNullException(nameof (sourceComment));
    ((RichTextString) this.RichText).CopyFrom((RichTextString) sourceComment.RichText, dicFontIndexes);
    this.Name = sourceComment.Name;
    if (!this.m_bUpdateLineFill)
      return;
    this.CopyFillOptions((ShapeImpl) sourceComment, (IDictionary) dicFontIndexes);
  }

  protected override void OnPrepareForSerialization()
  {
    this.m_shape = (MsofbtSp) MsoFactory.GetRecord(MsoRecords.msofbtSp);
    this.m_shape.Version = 2;
    this.m_shape.Instance = 202;
    this.m_shape.IsHaveAnchor = true;
    this.m_shape.IsHaveSpt = true;
  }

  private void ParseNoteRecord(int iObjectId)
  {
    NoteRecord noteByObjectIndex = (this.Worksheet as WorksheetImpl).GetNoteByObjectIndex(iObjectId);
    if (noteByObjectIndex == null)
      return;
    this.Author = noteByObjectIndex.AuthorName;
    this.m_iRow = (int) noteByObjectIndex.Row;
    this.m_iColumn = (int) noteByObjectIndex.Column;
    this.IsVisible = noteByObjectIndex.IsVisible;
  }
}
