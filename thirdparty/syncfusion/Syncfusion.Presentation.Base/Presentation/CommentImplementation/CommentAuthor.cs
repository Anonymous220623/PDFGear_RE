// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.CommentImplementation.CommentAuthor
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.CommentImplementation;

internal class CommentAuthor
{
  private uint _authorId;
  private string _name;
  private string _initials;
  private uint _lastIndex;
  private uint _colorIndex;
  private List<Comment> _commentlist;
  private Syncfusion.Presentation.Presentation _presentation;

  internal CommentAuthor(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
    this._commentlist = new List<Comment>();
  }

  internal List<Comment> CommentList
  {
    get => this._commentlist ?? (this._commentlist = new List<Comment>());
  }

  internal uint AuthorId
  {
    get => this._authorId;
    set => this._authorId = value;
  }

  internal uint LastIndex
  {
    get => this._lastIndex;
    set => this._lastIndex = value;
  }

  internal uint ColorIndex
  {
    get => this._colorIndex;
    set => this._colorIndex = value;
  }

  internal string Name
  {
    get => this._name;
    set => this._name = value;
  }

  internal string Initials
  {
    get => this._initials;
    set => this._initials = value;
  }

  internal void Clear()
  {
    if (this._commentlist == null)
      return;
    foreach (Comment comment in this._commentlist)
      comment.Close();
    this._commentlist.Clear();
    this._commentlist = (List<Comment>) null;
  }

  internal CommentAuthor CloneCommentAuthor() => (CommentAuthor) this.MemberwiseClone();
}
