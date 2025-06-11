// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.CommentImplementation.Comment
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.CommentImplementation;

internal class Comment : IComment
{
  private double _left;
  private double _top;
  private string _authorName;
  private string _initials;
  private string _text;
  private DateTime _dateTime;
  private uint _index;
  private Dictionary<string, Stream> _preservedElements;
  private Slide _slide;
  private CommentAuthor _commentAuthor;
  private bool _hasChild;
  private bool _isReplyComment;
  private IComment _parent;

  internal Comment(Slide slide)
  {
    this._slide = slide;
    this._commentAuthor = new CommentAuthor(slide.Presentation);
  }

  internal bool IsReply
  {
    get => this._isReplyComment;
    set => this._isReplyComment = value;
  }

  public DateTime DateTime
  {
    get => this._dateTime;
    set => this._dateTime = value;
  }

  public double Left
  {
    get => this._left;
    set => this._left = value;
  }

  public double Top
  {
    get => this._top;
    set => this._top = value;
  }

  public string AuthorName
  {
    get
    {
      if (this._authorName == null)
      {
        foreach (CommentAuthor commentAuthor in this._slide.Presentation.CommentAuthors)
        {
          if ((int) this._commentAuthor.AuthorId == (int) commentAuthor.AuthorId)
          {
            this._authorName = commentAuthor.Name;
            break;
          }
        }
      }
      return this._authorName;
    }
    set
    {
      if (this.AuthorName != null)
      {
        this._commentAuthor = this.AddCommentAuthor(this.Initials, value);
        this._commentAuthor.Name = value;
        this._commentAuthor.Initials = this.Initials;
      }
      this._authorName = value;
    }
  }

  internal CommentAuthor AddCommentAuthor(string intial, string newAuthorName)
  {
    bool flag = false;
    CommentAuthor commentAuthor1 = this.GetCommentAuthor();
    foreach (CommentAuthor commentAuthor2 in this._slide.Presentation.CommentAuthors)
    {
      if ((int) commentAuthor2.AuthorId == (int) commentAuthor1.AuthorId || commentAuthor2.Name == this.AuthorName)
      {
        flag = true;
        commentAuthor1.AuthorId = commentAuthor2.AuthorId;
        commentAuthor2.Initials = intial;
        commentAuthor2.Name = newAuthorName;
        this.Index = (uint) (commentAuthor2.CommentList.Count + 1);
        commentAuthor2.CommentList.Add(this);
      }
    }
    if (!flag)
    {
      commentAuthor1.AuthorId = (uint) (this._slide.Presentation.CommentAuthors.Count + 1);
      this._slide.Presentation.CommentAuthors.Add(commentAuthor1);
    }
    commentAuthor1.ColorIndex = commentAuthor1.AuthorId;
    commentAuthor1.Name = newAuthorName;
    commentAuthor1.Initials = intial;
    commentAuthor1.CommentList.Add(this);
    commentAuthor1.LastIndex = (uint) commentAuthor1.CommentList.Count;
    return commentAuthor1;
  }

  public string Initials
  {
    get
    {
      if (this._initials == null)
      {
        foreach (CommentAuthor commentAuthor in this._slide.Presentation.CommentAuthors)
        {
          if ((int) this._commentAuthor.AuthorId == (int) commentAuthor.AuthorId)
          {
            this._initials = commentAuthor.Initials;
            break;
          }
        }
      }
      return this._initials;
    }
    set
    {
      foreach (Comment comment in (IEnumerable<IComment>) this._slide.Comments)
      {
        if (this.AuthorName == comment.AuthorName)
        {
          this._commentAuthor = this.AddCommentAuthor(value, this.AuthorName);
          this._commentAuthor.Name = this.AuthorName;
          this._commentAuthor.Initials = value;
          break;
        }
      }
      this._initials = value;
    }
  }

  public string Text
  {
    get => this._text;
    set => this._text = value;
  }

  internal uint Index
  {
    get => this._index;
    set => this._index = value;
  }

  public bool HasChild => this._hasChild;

  public List<IComment> GetChild()
  {
    List<IComment> child = new List<IComment>(((Comments) this.GetParentSlide().Comments).Count);
    foreach (IComment comment in (Comments) this.GetParentSlide().Comments)
    {
      if (comment.Parent != null && comment.Parent == this)
        child.Add(comment);
    }
    return child;
  }

  public IComment Parent => this._parent;

  internal Dictionary<string, Stream> PreservedElements
  {
    get => this._preservedElements ?? (this._preservedElements = new Dictionary<string, Stream>());
  }

  internal CommentAuthor GetCommentAuthor() => this._commentAuthor;

  internal void Close()
  {
    if (this._preservedElements != null)
    {
      this._preservedElements.Clear();
      this._preservedElements = (Dictionary<string, Stream>) null;
    }
    if (this._commentAuthor != null)
    {
      this._commentAuthor.Clear();
      this._commentAuthor = (CommentAuthor) null;
    }
    this._slide = (Slide) null;
  }

  internal void SetParent(IComment parent)
  {
    this._parent = parent;
    ((Comment) this._parent)._hasChild = true;
  }

  internal void SetParentSlide(Slide slide) => this._slide = slide;

  internal Slide GetParentSlide() => this._slide;

  public IComment Clone()
  {
    Comment comment = (Comment) this.MemberwiseClone();
    comment._preservedElements = Helper.CloneDictionary(this.PreservedElements);
    comment._commentAuthor = this._commentAuthor.CloneCommentAuthor();
    return (IComment) comment;
  }
}
