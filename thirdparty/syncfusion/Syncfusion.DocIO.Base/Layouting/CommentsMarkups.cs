// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.CommentsMarkups
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;

#nullable disable
namespace Syncfusion.Layouting;

internal class CommentsMarkups : TrackChangesMarkups
{
  private WComment m_comment;
  private float m_extraSpacing;

  internal CommentsMarkups(WordDocument wordDocument, WComment comment)
    : base(wordDocument)
  {
    this.m_comment = comment;
  }

  internal WComment Comment => this.m_comment;

  internal string CommentID
  {
    get => this.Comment.CommentRangeEnd == null ? "0" : this.Comment.CommentRangeEnd.CommentId;
  }

  internal float ExtraSpacing
  {
    get => this.m_extraSpacing;
    set => this.m_extraSpacing = value;
  }

  private string GetBalloonValueForComments()
  {
    string str = this.Comment.Format.UserInitials + (this.Document.Comments.InnerList.IndexOf((object) this.Comment) + 1).ToString();
    if (this.Comment.Ancestor != null)
    {
      int num = this.Document.Comments.InnerList.IndexOf((object) this.Comment.Ancestor) + 1;
      str = $"{str}R{num.ToString()}";
    }
    return $"Commented [{str}]";
  }

  internal void AppendInCommentsBalloon()
  {
    WTextBody wtextBody = this.Comment.TextBody.Clone() as WTextBody;
    wtextBody.SetOwner((OwnerHolder) new WSection((IWordDocument) this.Document));
    this.ChangedValue = wtextBody;
    float fontSize = 10f;
    string str = "Segoe UI";
    if (this.Document.Styles.FindByName("Balloon Text", StyleType.ParagraphStyle) is WParagraphStyle byName)
    {
      fontSize = byName.CharacterFormat.FontSize;
      str = byName.CharacterFormat.FontName;
    }
    this.ApplyCommentsProperties(fontSize);
    if (this.ChangedValue.ChildEntities.Count > 0)
    {
      if (!(this.ChangedValue.ChildEntities[0] is WParagraph wparagraph))
      {
        wparagraph = new WParagraph((IWordDocument) this.Document);
        this.ChangedValue.ChildEntities.Insert(0, (IEntity) wparagraph);
      }
      wparagraph.ChildEntities.Insert(0, (IEntity) new WTextRange((IWordDocument) this.Document)
      {
        Text = (this.GetBalloonValueForComments() + ": "),
        CharacterFormat = {
          FontSize = (fontSize + 1f),
          FontName = str,
          Bold = true
        }
      });
    }
    else
    {
      this.ChangedValue.AddParagraph();
      IWTextRange wtextRange = this.ChangedValue.LastParagraph.AppendText(this.GetBalloonValueForComments() + ": ");
      wtextRange.CharacterFormat.Bold = true;
      wtextRange.CharacterFormat.FontSize = fontSize + 1f;
      wtextRange.CharacterFormat.FontName = str;
    }
  }

  private void ApplyCommentsProperties(float fontSize)
  {
    foreach (TextBodyItem childEntity1 in (CollectionImpl) this.ChangedValue.ChildEntities)
    {
      if (childEntity1 is WParagraph)
      {
        WParagraph wparagraph = childEntity1 as WParagraph;
        wparagraph.ParagraphFormat.AfterSpacing = 0.0f;
        wparagraph.ParagraphFormat.BeforeSpacing = 0.0f;
        wparagraph.ParagraphFormat.LineSpacing = 12f;
        wparagraph.ParagraphFormat.LogicalJustification = HorizontalAlignment.Left;
        foreach (ParagraphItem childEntity2 in (CollectionImpl) wparagraph.ChildEntities)
        {
          if (childEntity2 is WTextRange)
            (childEntity2 as WTextRange).CharacterFormat.FontSize = fontSize;
        }
      }
    }
  }
}
