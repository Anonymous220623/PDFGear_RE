// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TextBodySelection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class TextBodySelection
{
  private WTextBody m_textBody;
  private int m_itemStartIndex = -1;
  private int m_itemEndIndex = -1;
  private int m_pItemStartIndex = -1;
  private int m_pItemEndIndex = -1;

  public WTextBody TextBody => this.m_textBody;

  public int ItemStartIndex
  {
    get => this.m_itemStartIndex;
    set => this.m_itemStartIndex = value;
  }

  public int ItemEndIndex
  {
    get => this.m_itemEndIndex;
    set => this.m_itemEndIndex = value;
  }

  public int ParagraphItemStartIndex
  {
    get => this.m_pItemStartIndex;
    set => this.m_pItemStartIndex = value;
  }

  public int ParagraphItemEndIndex
  {
    get => this.m_pItemEndIndex;
    set => this.m_pItemEndIndex = value;
  }

  public TextBodySelection(ParagraphItem itemStart, ParagraphItem itemEnd)
  {
    WParagraph ownerParagraph1 = itemStart.OwnerParagraph;
    WParagraph ownerParagraph2 = itemEnd.OwnerParagraph;
    if (ownerParagraph1.Owner != ownerParagraph2.Owner)
      throw new ArgumentException("itemStart and itemEnd must be contained in one text body");
    this.m_textBody = ownerParagraph1.OwnerTextBody;
    this.m_itemStartIndex = ownerParagraph1.GetIndexInOwnerCollection();
    this.m_itemEndIndex = ownerParagraph2.GetIndexInOwnerCollection();
    this.m_pItemStartIndex = itemStart.GetIndexInOwnerCollection();
    this.m_pItemEndIndex = itemEnd.GetIndexInOwnerCollection();
    if (itemEnd is BookmarkEnd bookmarkEnd && bookmarkEnd.Name != "_GoBack" && !bookmarkEnd.HasRenderableItemBefore())
    {
      WParagraph previousSibling = ownerParagraph2.PreviousSibling as WParagraph;
      if (ownerParagraph1 != ownerParagraph2 && previousSibling != null)
      {
        this.m_itemEndIndex = previousSibling.GetIndexInOwnerCollection();
        this.m_pItemEndIndex = previousSibling.ChildEntities.Count;
      }
    }
    this.ValidateIndexes();
  }

  public TextBodySelection(
    ITextBody textBody,
    int itemStartIndex,
    int itemEndIndex,
    int pItemStartIndex,
    int pItemEndIndex)
  {
    this.m_textBody = textBody != null ? (WTextBody) textBody : throw new ArgumentNullException(nameof (textBody));
    this.m_itemStartIndex = itemStartIndex;
    this.m_itemEndIndex = itemEndIndex;
    this.m_pItemStartIndex = pItemStartIndex;
    this.m_pItemEndIndex = pItemEndIndex;
    this.ValidateIndexes();
  }

  private void ValidateIndexes()
  {
    if (this.m_itemStartIndex < 0 || this.m_itemStartIndex >= this.m_textBody.Items.Count)
      throw new ArgumentOutOfRangeException("m_itemStartIndex", "m_itemStartIndex is less than 0 or greater than " + (object) this.m_textBody.Items.Count);
    if (this.m_itemEndIndex < this.m_itemStartIndex || this.m_itemEndIndex >= this.m_textBody.Items.Count)
      throw new ArgumentOutOfRangeException("m_itemEndIndex", $"m_itemEndIndex is less than {(object) this.m_itemStartIndex} or greater than {(object) this.m_textBody.Items.Count}");
    WParagraph wparagraph1 = this.m_textBody.Items[this.m_itemStartIndex] as WParagraph;
    WParagraph wparagraph2 = this.m_textBody.Items[this.m_itemEndIndex] as WParagraph;
    if (wparagraph1 != null && (this.m_pItemStartIndex < 0 || this.m_pItemStartIndex > wparagraph1.Items.Count))
      throw new ArgumentOutOfRangeException("m_pItemStartIndex", "m_pItemStartIndex is less than 0 or greater than " + (object) wparagraph1.Items.Count);
    if (wparagraph2 != null && (this.m_pItemEndIndex < 0 || this.m_pItemEndIndex > wparagraph2.Items.Count))
      throw new ArgumentOutOfRangeException("m_pItemEndIndex", "m_pItemEndIndex is less than 0 or greater than " + (object) wparagraph2.Items.Count);
  }
}
