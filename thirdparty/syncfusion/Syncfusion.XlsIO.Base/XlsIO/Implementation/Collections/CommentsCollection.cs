// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.CommentsCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class CommentsCollection : CollectionBaseEx<CommentShapeImpl>, IComments, IEnumerable
{
  private const int DEFAULT_WIDTH = 200;
  private const int DEFAULT_HEIGHT = 100;
  private const int DEF_COLUMNS_COUNT = 1;
  private const int DEF_ROWS_COUNT = 3;
  private WorksheetImpl m_sheet;
  private Dictionary<long, ICommentShape> m_hashComments = new Dictionary<long, ICommentShape>();
  private bool m_bReRegister;

  public ICommentShape this[int index] => (ICommentShape) this.InnerList[index];

  public ICommentShape this[int iRow, int iColumn]
  {
    get
    {
      ICommentShape commentShape;
      this.HashComments.TryGetValue(RangeImpl.GetCellIndex(iColumn, iRow), out commentShape);
      return commentShape;
    }
  }

  public ICommentShape this[string name]
  {
    get
    {
      ICommentShape commentShape1 = (ICommentShape) null;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        ICommentShape commentShape2 = this[index];
        if (commentShape2.Name == name)
        {
          commentShape1 = commentShape2;
          break;
        }
      }
      return commentShape1;
    }
  }

  public bool ReRegisterOnAccess
  {
    get => this.m_bReRegister;
    set => this.m_bReRegister = value;
  }

  private Dictionary<long, ICommentShape> HashComments
  {
    get
    {
      if (this.m_bReRegister)
        this.ReRegisterComments();
      return this.m_hashComments;
    }
  }

  public CommentsCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  public ICommentShape AddComment(IRange parentRange)
  {
    if (parentRange == null)
      throw new ArgumentNullException(nameof (parentRange));
    return ((RangeImpl) parentRange).IsSingleCell ? this.AddComment(parentRange.Row, parentRange.Column) : (ICommentShape) null;
  }

  public ICommentShape AddComment(int iRow, int iColumn) => this.AddComment(iRow, iColumn, true);

  public ICommentShape AddComment(int iRow, int iColumn, bool bIsParseOptions)
  {
    WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this.m_sheet, iRow - 1, true);
    CommentShapeImpl commentShapeImpl = this.m_sheet.Shapes.AddComment(string.Empty, bIsParseOptions) as CommentShapeImpl;
    commentShapeImpl.Column = iColumn;
    commentShapeImpl.Row = iRow;
    MsofbtClientAnchor clientAnchor = commentShapeImpl.ClientAnchor;
    clientAnchor.LeftColumn = iColumn - 1;
    clientAnchor.TopRow = iRow - 1;
    clientAnchor.RightColumn = iColumn + 1;
    clientAnchor.BottomRow = iRow + 3;
    clientAnchor.LeftOffset = 240 /*0xF0*/;
    clientAnchor.RightOffset = 240 /*0xF0*/;
    clientAnchor.TopOffset = 240 /*0xF0*/;
    clientAnchor.BottomOffset = 240 /*0xF0*/;
    int num1 = this.m_sheet.Workbook.MaxColumnCount - 1;
    if (clientAnchor.RightColumn > num1)
    {
      int num2 = clientAnchor.RightColumn - num1;
      clientAnchor.RightColumn = num1;
      clientAnchor.LeftColumn -= num2 + 1;
    }
    int maxRowCount = this.m_sheet.Workbook.MaxRowCount;
    if (clientAnchor.BottomRow >= maxRowCount)
    {
      int num3 = clientAnchor.BottomRow - maxRowCount + 1;
      clientAnchor.BottomRow = maxRowCount - 1;
      clientAnchor.TopRow -= num3 + 1;
    }
    commentShapeImpl.EvaluateTopLeftPosition();
    commentShapeImpl.UpdateWidth();
    commentShapeImpl.UpdateHeight();
    this.Add(commentShapeImpl);
    return (ICommentShape) commentShapeImpl;
  }

  internal void AddComment(ICommentShape comment)
  {
    long cellIndex = RangeImpl.GetCellIndex(comment.Column, comment.Row);
    if (!this.m_hashComments.ContainsKey(cellIndex))
      this.Add(comment as CommentShapeImpl);
    else
      this.m_hashComments[cellIndex] = comment;
  }

  private void SetParents()
  {
    this.m_sheet = this.FindParent(typeof (WorksheetImpl)) as WorksheetImpl;
    if (this.m_sheet == null)
      throw new ArgumentNullException("Can't find parent worksheet");
  }

  public void Remove(ICommentShape comment)
  {
    this.Remove(comment as CommentShapeImpl);
    ((ShapesCollection) this.m_sheet.Shapes).InnerRemoveComment(comment);
  }

  internal void InnerRemove(ICommentShape comment)
  {
    this.m_hashComments.Remove(RangeImpl.GetCellIndex(comment.Column, comment.Row));
    this.Remove(comment);
  }

  internal void ReRegisterComments()
  {
    this.m_hashComments.Clear();
    System.Collections.Generic.List<CommentShapeImpl> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      CommentShapeImpl commentShapeImpl = innerList[index];
      this.m_hashComments.Add(RangeImpl.GetCellIndex(commentShapeImpl.Column, commentShapeImpl.Row), (ICommentShape) commentShapeImpl);
    }
  }

  protected override void OnClear()
  {
    base.OnClear();
    for (int index = this.Count - 1; index >= 0; --index)
      ((ShapesCollection) this.m_sheet.Shapes).InnerRemoveComment(this[index]);
  }

  protected override void OnInsertComplete(int index, CommentShapeImpl value)
  {
    base.OnInsertComplete(index, value);
    int row = value.Row;
    this.HashComments[RangeImpl.GetCellIndex(value.Column, row)] = (ICommentShape) value;
  }

  protected override void OnRemoveComplete(int index, CommentShapeImpl value)
  {
    base.OnRemoveComplete(index, value);
    int row = value.Row;
    this.m_hashComments.Remove(RangeImpl.GetCellIndex(value.Column, row));
  }

  protected override void OnClearComplete()
  {
    base.OnClearComplete();
    this.m_hashComments.Clear();
  }

  protected override void OnSetComplete(
    int index,
    CommentShapeImpl oldValue,
    CommentShapeImpl newValue)
  {
    base.OnSetComplete(index, oldValue, newValue);
    int row = newValue.Row;
    this.HashComments[RangeImpl.GetCellIndex(newValue.Column, row)] = (ICommentShape) newValue;
  }
}
