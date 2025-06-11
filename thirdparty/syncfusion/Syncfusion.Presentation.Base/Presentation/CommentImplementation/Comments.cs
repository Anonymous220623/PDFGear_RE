// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.CommentImplementation.Comments
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.CommentImplementation;

internal class Comments : IComments, IEnumerable<IComment>, IEnumerable
{
  private List<IComment> _comments;
  private CommentAuthor _commentAuthor;
  private Slide _slide;

  internal Comments(Slide slide)
  {
    this._slide = slide;
    this._comments = new List<IComment>();
  }

  internal Comments(Comment replyComment)
  {
    this._slide = replyComment.GetParentSlide();
    this._comments = new List<IComment>();
  }

  internal Comments(CommentAuthor commentAuthor)
  {
    this._comments = new List<IComment>();
    this._commentAuthor = commentAuthor;
  }

  public IComment this[int index]
  {
    get
    {
      if (this._comments.Count <= index)
        throw new IndexOutOfRangeException("Index was out of range,value should be greater than slide count");
      return this._comments[index];
    }
  }

  public int Count => this._comments != null ? this._comments.Count : 0;

  public IComment Add(
    double left,
    double top,
    string authorName,
    string authorInitials,
    string text,
    DateTime dateTime)
  {
    Comment comment = this.CreateComment(left, top, authorName, authorInitials, text, dateTime);
    this.UpdateCommentRelation();
    ((Comments) this._slide.Comments).Add(comment);
    return (IComment) comment;
  }

  public IComment Add(
    string authorName,
    string authorInitials,
    string text,
    DateTime dateTime,
    IComment parentComment)
  {
    Comment parent = (Comment) parentComment;
    if (parent.IsReply)
      throw new InvalidOperationException("Reply can be made only to the parent comments.");
    Comment comment = this.CreateComment(parent.Left, parent.Top, authorName, authorInitials, text, dateTime);
    comment.IsReply = true;
    comment.SetParent((IComment) parent);
    ((Comments) this._slide.Comments).Add(comment);
    return (IComment) comment;
  }

  private Comment CreateComment(
    double left,
    double top,
    string authorName,
    string authorInitials,
    string text,
    DateTime dateTime)
  {
    Comment comment = new Comment(this._slide);
    comment.Left = left;
    comment.Top = top;
    comment.AuthorName = authorName;
    comment.Initials = authorInitials;
    comment.Text = text;
    comment.DateTime = dateTime;
    ++comment.Index;
    this.AddCommentAuthor(comment);
    return comment;
  }

  internal void UpdateCommentRelation()
  {
    if (this._slide.HasComment)
      return;
    string relationIdentifier1 = Helper.GenerateRelationIdentifier(this._slide.TopRelation);
    string str = $"comments/comment{(++this._slide.Presentation.CommentCount).ToString()}.xml";
    this._slide.TopRelation.Add(relationIdentifier1, new Relation(relationIdentifier1, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/comments", "../" + str, (string) null));
    this._slide.Presentation.AddOverrideContentType("/ppt/" + str, "application/vnd.openxmlformats-officedocument.presentationml.comments+xml");
    this._slide.CommentRelationId = relationIdentifier1;
    string relationIdentifier2 = Helper.GenerateRelationIdentifier(this._slide.Presentation.TopRelation);
    string target = "commentAuthors.xml";
    this._slide.Presentation.TopRelation.Add(relationIdentifier2, new Relation(relationIdentifier2, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/commentAuthors", target, (string) null));
    this._slide.Presentation.AddOverrideContentType("/ppt/" + target, "application/vnd.openxmlformats-officedocument.presentationml.commentAuthors+xml");
  }

  internal void AddCommentAuthor(Comment comment)
  {
    bool flag = false;
    CommentAuthor commentAuthor1 = comment.GetCommentAuthor();
    foreach (CommentAuthor commentAuthor2 in this._slide.Presentation.CommentAuthors)
    {
      if ((int) commentAuthor2.AuthorId == (int) commentAuthor1.AuthorId || commentAuthor2.Name == comment.AuthorName)
      {
        flag = true;
        commentAuthor2.Initials = comment.Initials;
        commentAuthor1.AuthorId = commentAuthor2.AuthorId;
        commentAuthor2.Name = comment.AuthorName;
        comment.Index = (uint) (commentAuthor2.CommentList.Count + 1);
        commentAuthor2.CommentList.Add(comment);
      }
    }
    if (!flag)
    {
      commentAuthor1.AuthorId = (uint) (this._slide.Presentation.CommentAuthors.Count + 1);
      this._slide.Presentation.CommentAuthors.Add(commentAuthor1);
    }
    commentAuthor1.ColorIndex = commentAuthor1.AuthorId;
    commentAuthor1.Name = comment.AuthorName;
    commentAuthor1.Initials = comment.Initials;
    commentAuthor1.LastIndex = (uint) commentAuthor1.CommentList.Count;
    commentAuthor1.CommentList.Add(comment);
  }

  internal void Add(Comment comment) => this._comments.Add((IComment) comment);

  public IEnumerator<IComment> GetEnumerator()
  {
    return (IEnumerator<IComment>) this._comments.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._comments.GetEnumerator();

  public void RemoveAt(int index) => this.Remove(this._comments[index]);

  public void Remove(IComment comment)
  {
    if (!this._comments.Contains(comment))
      return;
    if (comment.HasChild)
    {
      foreach (IComment comment1 in comment.GetChild())
        this._comments.Remove(comment1);
    }
    this._comments.Remove(comment);
  }

  public void Insert(int index, IComment comment) => this._comments.Insert(index, comment);

  public int IndexOf(IComment comment) => this._comments.IndexOf(comment);

  public void Clear() => this.ClearAll();

  private void ClearAll()
  {
    if (this._comments != null)
    {
      foreach (Comment comment in this._comments)
        comment.Close();
      this._comments.Clear();
      this._comments = (List<IComment>) null;
    }
    this._slide = (Slide) null;
  }

  internal Comments CloneComments()
  {
    Comments comments = (Comments) this.MemberwiseClone();
    comments._comments = this.CloneCommentList();
    return comments;
  }

  private List<IComment> CloneCommentList()
  {
    List<IComment> commentList = new List<IComment>();
    foreach (Comment comment1 in this._comments)
    {
      Comment comment2 = comment1.Clone() as Comment;
      commentList.Add((IComment) comment2);
    }
    return commentList;
  }

  internal void SetParent(Slide slide)
  {
    this._slide = slide;
    foreach (Comment comment in this._comments)
      comment.SetParentSlide(slide);
  }
}
