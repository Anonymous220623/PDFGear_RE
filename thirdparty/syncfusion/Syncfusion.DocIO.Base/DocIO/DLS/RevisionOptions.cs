// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.RevisionOptions
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class RevisionOptions
{
  private bool m_showRevisionBars;
  private bool m_showRevisionMarks;
  private RevisionColor m_revisionBarsColor = RevisionColor.Red;
  private float m_revisionMarkWidth = 1f;
  private RevisionColor m_insertedTextColor;
  private RevisedTextEffect m_insertedTextEffect = RevisedTextEffect.Underline;
  private RevisedTextEffect m_deletedTextEffect = RevisedTextEffect.StrikeThrough;
  private RevisedTextEffect m_revisedPropertiesEffect;
  private RevisionColor m_deletedTextColor;
  private bool m_showDeletedText;
  private RevisionColor m_revisedPropertiesColor;
  private RevisionType m_showMarkup = RevisionType.None;
  private RevisionType m_showInBalloons = RevisionType.Deletions | RevisionType.Formatting;
  private RevisionBalloonsOptions m_balloonOptions = RevisionBalloonsOptions.Inline;
  private CommentDisplayMode m_commentDisplayMode;
  private RevisionColor m_commentColor = RevisionColor.Red;

  public CommentDisplayMode CommentDisplayMode
  {
    get => this.m_commentDisplayMode;
    set => this.m_commentDisplayMode = value;
  }

  public RevisionColor CommentColor
  {
    get => this.m_commentColor;
    set => this.m_commentColor = value;
  }

  internal bool ShowRevisionBars
  {
    get => this.m_showRevisionBars;
    set => this.m_showRevisionBars = value;
  }

  internal bool ShowRevisionMarks
  {
    get => this.m_showRevisionMarks;
    set => this.m_showRevisionMarks = value;
  }

  public RevisionColor RevisionBarsColor
  {
    get => this.m_revisionBarsColor;
    set => this.m_revisionBarsColor = value;
  }

  public RevisionColor InsertedTextColor
  {
    get => this.m_insertedTextColor;
    set => this.m_insertedTextColor = value;
  }

  internal float RevisionMarkWidth
  {
    get => this.m_revisionMarkWidth;
    set => this.m_revisionMarkWidth = value;
  }

  internal RevisedTextEffect InsertedTextEffect
  {
    get => this.m_insertedTextEffect;
    set => this.m_insertedTextEffect = value;
  }

  public RevisionColor DeletedTextColor
  {
    get => this.m_deletedTextColor;
    set => this.m_deletedTextColor = value;
  }

  internal RevisedTextEffect DeletedTextEffect
  {
    get => this.m_deletedTextEffect;
    set => this.m_deletedTextEffect = value;
  }

  public RevisionColor RevisedPropertiesColor
  {
    get => this.m_revisedPropertiesColor;
    set => this.m_revisedPropertiesColor = value;
  }

  internal RevisedTextEffect RevisedPropetiesEffect
  {
    get => this.m_revisedPropertiesEffect;
    set => this.m_revisedPropertiesEffect = value;
  }

  internal bool ShowDeletedText
  {
    get => this.m_showDeletedText;
    set => this.m_showDeletedText = value;
  }

  public RevisionType ShowMarkup
  {
    get => this.m_showMarkup;
    set
    {
      this.m_showMarkup = value;
      this.SetTrackChangesOptions();
    }
  }

  public RevisionType ShowInBalloons
  {
    get => this.m_showInBalloons;
    set => this.m_showInBalloons = value;
  }

  internal RevisionBalloonsOptions BalloonOptions
  {
    get => this.m_balloonOptions;
    set => this.m_balloonOptions = value;
  }

  private void SetTrackChangesOptions()
  {
    if (this.m_showMarkup != RevisionType.None)
      this.ShowRevisionBars = true;
    if (this.m_showMarkup == RevisionType.None)
      this.ShowRevisionBars = false;
    if ((this.m_showMarkup & RevisionType.Insertions) == RevisionType.Insertions)
      this.ShowRevisionMarks = true;
    if ((this.m_showMarkup & RevisionType.Deletions) == RevisionType.Deletions)
      this.BalloonOptions |= RevisionBalloonsOptions.Deletions;
    if ((this.m_showMarkup & RevisionType.Formatting) != RevisionType.Formatting)
      return;
    this.BalloonOptions |= RevisionBalloonsOptions.Formatting;
  }
}
