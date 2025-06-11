// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WCommentMark
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WCommentMark : ParagraphItem, ILeafWidget, IWidget
{
  private string m_commentId = "";
  private CommentMarkType m_markType;
  private WComment m_ownerComment;
  private byte m_bFlags;

  public WComment Comment
  {
    get => this.m_ownerComment;
    internal set => this.m_ownerComment = value;
  }

  internal string CommentId
  {
    get => this.m_commentId;
    set => this.m_commentId = value;
  }

  public override EntityType EntityType => EntityType.CommentMark;

  public CommentMarkType Type
  {
    get => this.m_markType;
    set => this.m_markType = value;
  }

  internal bool IsAfterCellMark
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal WCommentMark(WordDocument doc, string commentId)
    : base(doc)
  {
    this.m_commentId = commentId;
  }

  internal WCommentMark(WordDocument doc, string commentId, CommentMarkType type)
    : this(doc, commentId)
  {
    this.m_markType = type;
  }

  protected override object CloneImpl()
  {
    WCommentMark wcommentMark = (WCommentMark) base.CloneImpl();
    if (this.m_commentId == "")
      wcommentMark.CommentId = this.m_markType != CommentMarkType.CommentStart ? Convert.ToString(TagIdRandomizer.GetMarkerId(Convert.ToInt32(this.m_commentId), false)) : Convert.ToString(TagIdRandomizer.GetMarkerId(Convert.ToInt32(this.m_commentId), true));
    return (object) wcommentMark;
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo();
    if (this.Document.RevisionOptions.CommentDisplayMode != CommentDisplayMode.ShowInBalloons)
      return;
    this.m_layoutInfo.IsSkip = false;
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  SizeF ILeafWidget.Measure(DrawingContext dc) => new SizeF();
}
