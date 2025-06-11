// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Shapes.CommentsRange
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Shapes;

public class CommentsRange : 
  CommonObject,
  ICommentShape,
  IComment,
  ITextBox,
  IShape,
  IParentApplication
{
  private IRange m_range;
  private IRichTextString m_richTextString;

  public CommentsRange(IApplication application, IRange parentRange)
    : base(application, (object) parentRange)
  {
    this.m_range = parentRange;
  }

  public string Author
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      string author = (string) null;
      bool flag = true;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
          {
            author = cells[index].Comment.Author;
            flag = false;
          }
          else if (author != cells[index].Comment.Author)
            return (string) null;
        }
      }
      return author;
    }
  }

  public bool IsVisible
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      bool isVisible = false;
      bool flag = true;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
          {
            isVisible = cells[index].Comment.IsShapeVisible;
            flag = false;
          }
          else if (isVisible != cells[index].Comment.IsShapeVisible)
            return false;
        }
      }
      return isVisible;
    }
    set
    {
      int index = 0;
      for (int length = this.m_range.Cells.Length; index < length; ++index)
        this.m_range.Cells[index].AddComment().IsShapeVisible = value;
    }
  }

  public int Row
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      int row = int.MinValue;
      bool flag = true;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
          {
            row = cells[index].Comment.Row;
            flag = false;
          }
          else if (row != cells[index].Comment.Row)
            return int.MinValue;
        }
      }
      return row;
    }
  }

  public int Column
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      int column = int.MinValue;
      bool flag = true;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
          {
            column = cells[index].Comment.Column;
            flag = false;
          }
          else if (column != cells[index].Comment.Column)
            return int.MinValue;
        }
      }
      return column;
    }
  }

  public IRichTextString RichText
  {
    get
    {
      this.m_richTextString = (IRichTextString) new RTFCommentArray(this.Application, (object) this);
      return this.m_richTextString;
    }
    set => this.m_richTextString = value;
  }

  public string Text
  {
    get => this.RichText.Text;
    set => this.RichText.Text = value;
  }

  public bool IsMoveWithCell
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public bool IsSizeWithCell
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public bool AutoSize
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      bool autoSize = false;
      bool flag = true;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
          {
            autoSize = cells[index].Comment.AutoSize;
            flag = false;
          }
          else if (autoSize != cells[index].Comment.AutoSize)
            return false;
        }
      }
      return autoSize;
    }
    set
    {
      int index = 0;
      for (int length = this.m_range.Cells.Length; index < length; ++index)
        this.m_range.Cells[index].AddComment().AutoSize = value;
    }
  }

  public void Remove()
  {
    int index = 0;
    for (int length = this.m_range.Cells.Length; index < length; ++index)
      this.m_range.Cells[index].Comment?.Remove();
  }

  public void Scale(int scaleWidth, int scaleHeight)
  {
    int index = 0;
    for (int length = this.m_range.Cells.Length; index < length; ++index)
      this.m_range.Cells[index].Comment?.Scale(scaleWidth, scaleHeight);
  }

  public bool IsShapeVisible
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      bool isShapeVisible = false;
      bool flag = true;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
          {
            isShapeVisible = cells[index].Comment.IsShapeVisible;
            flag = false;
          }
          else if (isShapeVisible != cells[index].Comment.IsShapeVisible)
            return false;
        }
      }
      return isShapeVisible;
    }
    set
    {
      int index = 0;
      for (int length = this.m_range.Cells.Length; index < length; ++index)
        this.m_range.Cells[index].AddComment().IsShapeVisible = value;
    }
  }

  public int Height
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      int height = int.MinValue;
      bool flag = true;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
          {
            height = cells[index].Comment.Height;
            flag = false;
          }
          else if (height != cells[index].Comment.Height)
            return int.MinValue;
        }
      }
      return height;
    }
    set
    {
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
          cells[index].Comment.Height = value;
      }
    }
  }

  public int Id => 0;

  public int Left
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      int left = int.MinValue;
      bool flag = true;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
          {
            left = cells[index].Comment.Left;
            flag = false;
          }
          else if (left != cells[index].Comment.Left)
            return int.MinValue;
        }
      }
      return left;
    }
    set
    {
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
          cells[index].Comment.Left = value;
      }
    }
  }

  public string Name
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      string name = (string) null;
      bool flag = true;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
          {
            name = cells[index].Comment.Name;
            flag = false;
          }
          else if (name != cells[index].Comment.Name)
            return (string) null;
        }
      }
      return name;
    }
    set
    {
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
          cells[index].Comment.Name = value;
      }
    }
  }

  public int Top
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      int top = int.MinValue;
      bool flag = true;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
          {
            top = cells[index].Comment.Top;
            flag = false;
          }
          else if (top != cells[index].Comment.Top)
            return int.MinValue;
        }
      }
      return top;
    }
    set
    {
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
          cells[index].Comment.Top = value;
      }
    }
  }

  public int Width
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      int width = int.MinValue;
      bool flag = true;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
          {
            width = cells[index].Comment.Width;
            flag = false;
          }
          else if (width != cells[index].Comment.Width)
            return int.MinValue;
        }
      }
      return width;
    }
    set
    {
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
          cells[index].Comment.Width = value;
      }
    }
  }

  public ExcelShapeType ShapeType => ExcelShapeType.Comment;

  public string AlternativeText
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      string alternativeText = (string) null;
      bool flag = true;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
          {
            alternativeText = cells[index].Comment.AlternativeText;
            flag = false;
          }
          else if (alternativeText != cells[index].Comment.AlternativeText)
            return (string) null;
        }
      }
      return alternativeText;
    }
    set
    {
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
          cells[index].Comment.AlternativeText = value;
      }
    }
  }

  public IFill Fill
  {
    get => throw new NotSupportedException("This property doesn't support in this class");
  }

  public IShapeLineFormat Line
  {
    get => throw new NotSupportedException("This property doesn't support in this class");
  }

  public string OnAction
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public IThreeDFormat ThreeD
  {
    get => throw new NotSupportedException("This property doesn't support in this class");
  }

  public IShadow Shadow
  {
    get => throw new NotSupportedException("This property doesn't support in this class");
  }

  public int ShapeRotation
  {
    get => throw new NotSupportedException("This property doesn't support in this class");
    set => throw new NotSupportedException("This property doesn't support in this class");
  }

  public ITextFrame TextFrame
  {
    get => throw new NotImplementedException("This property doesn't support in this class");
  }

  public IHyperLink Hyperlink => throw new NotSupportedException("HyperLink");

  public ExcelCommentHAlign HAlignment
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      ExcelCommentHAlign halignment = ExcelCommentHAlign.Left;
      bool flag = true;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
          {
            halignment = cells[index].Comment.HAlignment;
            flag = false;
          }
          else if (halignment != cells[index].Comment.HAlignment)
            return ExcelCommentHAlign.Left;
        }
      }
      return halignment;
    }
    set
    {
      int index = 0;
      for (int length = this.m_range.Cells.Length; index < length; ++index)
        this.m_range.Cells[index].AddComment().HAlignment = value;
    }
  }

  public ExcelCommentVAlign VAlignment
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      ExcelCommentVAlign valignment = ExcelCommentVAlign.Top;
      bool flag = true;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
          {
            valignment = cells[index].Comment.VAlignment;
            flag = false;
          }
          else if (valignment != cells[index].Comment.VAlignment)
            return ExcelCommentVAlign.Top;
        }
      }
      return valignment;
    }
    set
    {
      int index = 0;
      for (int length = this.m_range.Cells.Length; index < length; ++index)
        this.m_range.Cells[index].AddComment().VAlignment = value;
    }
  }

  public ExcelTextRotation TextRotation
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      ExcelTextRotation textRotation = ExcelTextRotation.LeftToRight;
      bool flag = true;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
          {
            textRotation = cells[index].Comment.TextRotation;
            flag = false;
          }
          else if (textRotation != cells[index].Comment.TextRotation)
            return ExcelTextRotation.LeftToRight;
        }
      }
      return textRotation;
    }
    set
    {
      int index = 0;
      for (int length = this.m_range.Cells.Length; index < length; ++index)
        this.m_range.Cells[index].AddComment().TextRotation = value;
    }
  }

  public bool IsTextLocked
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      bool isTextLocked = false;
      bool flag = true;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
          {
            isTextLocked = cells[index].Comment.IsTextLocked;
            flag = false;
          }
          else if (isTextLocked != cells[index].Comment.IsTextLocked)
            return false;
        }
      }
      return isTextLocked;
    }
    set
    {
      int index = 0;
      for (int length = this.m_range.Cells.Length; index < length; ++index)
        this.m_range.Cells[index].AddComment().IsTextLocked = value;
    }
  }
}
