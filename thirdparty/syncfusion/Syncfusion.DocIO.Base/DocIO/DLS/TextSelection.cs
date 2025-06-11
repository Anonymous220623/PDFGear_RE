// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TextSelection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class TextSelection : IEnumerable
{
  private WParagraph m_para;
  private WTextRange m_startTr;
  private WTextRange m_endTr;
  private List<WTextRange> m_items = new List<WTextRange>();
  private int m_startCut;
  private int m_endCut;
  private int m_startIndex;
  private int m_endIndex;
  private WTextRange[] m_cachedRanges;
  internal TextSelectionList SelectionChain;

  public string SelectedText
  {
    get
    {
      if (this.m_startTr == null || this.m_endTr == null)
        return string.Empty;
      int startIndex = !(this.m_startTr.Owner is Break) || (this.m_startTr.Owner as Break).BreakType != BreakType.LineBreak ? this.m_startTr.StartPos : (this.m_startTr.Owner as Break).StartPos;
      if (this.m_startCut != 0)
        startIndex += this.m_startCut;
      int length = !(this.m_endTr.Owner is Break) || (this.m_endTr.Owner as Break).BreakType != BreakType.LineBreak ? (this.m_endCut >= 0 ? this.m_endTr.StartPos + this.m_endCut - startIndex : this.m_endTr.StartPos + this.m_endTr.TextLength - startIndex) : (this.m_endCut >= 0 ? (this.m_endTr.Owner as Break).StartPos + this.m_endCut - startIndex : (this.m_endTr.Owner as Break).StartPos + (this.m_endTr.Owner as Break).TextRange.Text.Length - startIndex);
      if (length < 0)
        throw new Exception("Text selection was modified. This could be done while modification of source document.");
      if (this.m_startTr.Owner is Break && (this.m_startTr.Owner as Break).BreakType == BreakType.LineBreak)
        return (this.m_startTr.Owner as Break).OwnerParagraph.Text.Substring(startIndex, length);
      return !(this.m_startTr.Owner is InlineContentControl) ? this.OwnerParagraph.Text.Substring(startIndex, length) : this.m_startTr.GetOwnerParagraphValue().Text.Substring(startIndex, length);
    }
  }

  public string this[int index]
  {
    get
    {
      string str = this.m_items[index].Text;
      if (index == 0 && this.m_startCut > 0)
        str = str.Substring(this.m_startCut);
      if (index == this.m_items.Count - 1 && this.m_endCut != -1)
        str = str.Substring(0, this.m_endCut - this.m_startCut);
      return str;
    }
    set
    {
      WTextRange wtextRange = this.m_items[index];
      string str = value;
      if (index == 0 && this.m_startCut > 0)
        str = wtextRange.Text.Substring(0, this.m_startCut) + str;
      if (index == this.m_items.Count && this.m_endCut != -1)
        str += wtextRange.Text.Substring(this.m_endCut);
      wtextRange.Text = str;
    }
  }

  public int Count => this.m_items.Count;

  internal WParagraph OwnerParagraph
  {
    get
    {
      if (this.m_startTr != null)
        this.m_para = this.m_startTr.OwnerParagraph;
      return this.m_para;
    }
  }

  internal WTextRange StartTextRange => this.m_startTr;

  internal WTextRange EndTextRange => this.m_endTr;

  public TextSelection(WParagraph para, int startCharPos, int endCharPos)
  {
    this.m_para = para;
    if (this.m_para.Items.Count == 0)
      return;
    WTextRange tr;
    this.m_startIndex = FindUtils.GetStartRangeIndex(this.m_para, startCharPos + 1, out tr);
    if (tr == null)
      return;
    if (tr.Owner is Break && (tr.Owner as Break).BreakType == BreakType.LineBreak)
      tr.StartPos = (tr.Owner as Break).StartPos;
    this.m_startCut = startCharPos - tr.StartPos;
    this.m_startTr = tr;
    this.m_endIndex = FindUtils.GetStartRangeIndex(this.m_para, endCharPos, out tr);
    if (this.m_endIndex < this.m_startIndex || tr == null)
    {
      for (int count = para.Items.Count; count > 0; --count)
      {
        ParagraphItem paragraphItem = para[count - 1];
        if (paragraphItem is WTextRange)
        {
          tr = paragraphItem as WTextRange;
          break;
        }
      }
      this.m_endCut = endCharPos - tr.StartPos - 1;
    }
    else
    {
      if (tr.Owner is Break && (tr.Owner as Break).BreakType == BreakType.LineBreak)
        tr.StartPos = (tr.Owner as Break).StartPos;
      this.m_endCut = endCharPos - tr.StartPos;
    }
    this.m_endTr = tr;
    if (this.m_endCut == tr.TextLength)
      this.m_endCut = -1;
    else if (tr.Owner is Break && (tr.Owner as Break).BreakType == BreakType.LineBreak && this.m_endCut == (tr.Owner as Break).TextRange.Text.Length)
      this.m_endCut = -1;
    if (!FindUtils.EnsureSameOwner(this.m_startTr, this.m_endTr))
      return;
    this.GetTextRanges(this.m_startTr);
  }

  public WTextRange[] GetRanges()
  {
    if (this.m_startTr.Owner is InlineContentControl && (this.m_startTr.Owner as InlineContentControl).ParagraphItems.Count == 0)
      return (WTextRange[]) null;
    if (this.OwnerParagraph != null && this.OwnerParagraph.Items.Count == 0)
      return (WTextRange[]) null;
    this.EnsureIndexes();
    if (this.m_startCut > 0 || this.m_endCut != -1)
      this.SplitRanges();
    return this.m_items.ToArray();
  }

  public WTextRange GetAsOneRange()
  {
    WParagraph ownerParagraph = this.OwnerParagraph;
    if (this.m_items.Count > 0 && this.m_items[0].Owner is Break && (this.m_items[0].Owner as Break).BreakType == BreakType.LineBreak)
    {
      ownerParagraph = (this.m_items[0].Owner as Break).OwnerParagraph;
      int inOwnerCollection = (this.m_items[0].Owner as Break).GetIndexInOwnerCollection();
      if (ownerParagraph != null && ownerParagraph.Items.Count > 0)
      {
        Break owner = this.m_items[0].Owner as Break;
        ownerParagraph.Items.Remove((IEntity) owner);
        owner.TextRange = (WTextRange) null;
        ownerParagraph.Items.Insert(inOwnerCollection, (IEntity) this.m_items[0]);
      }
    }
    if (this.m_items[0].Owner is InlineContentControl)
    {
      if ((this.m_items[0].Owner as InlineContentControl).ParagraphItems.Count == 0 || this.m_items[0].GetOwnerParagraphValue().Items.Count == 0)
        return (WTextRange) null;
    }
    else if (ownerParagraph == null || ownerParagraph.Items.Count == 0)
      return (WTextRange) null;
    this.EnsureIndexes();
    if (this.m_startCut > 0 || this.m_endCut != -1)
      this.SplitRanges();
    if (this.Count > 1)
    {
      string selectedText = this.SelectedText;
      while (this.m_items.Count > 1)
      {
        if (this.m_items[1].Owner is Break && (this.m_items[1].Owner as Break).BreakType == BreakType.LineBreak)
          (this.m_items[1].Owner as Break).RemoveSelf();
        else
          this.m_items[1].RemoveSelf();
        this.m_items.RemoveAt(1);
      }
      this.m_startTr.Text = selectedText;
      this.m_endTr = this.m_startTr;
    }
    return this.m_items[0];
  }

  internal int SplitAndErase()
  {
    if (this.m_startTr.Owner is InlineContentControl && (this.m_startTr.Owner as InlineContentControl).ParagraphItems.Count == 0 || this.OwnerParagraph != null && this.OwnerParagraph.Items.Count == 0)
      return 0;
    this.EnsureIndexes();
    if (this.m_startCut > 0 || this.m_endCut != -1)
      this.SplitRanges();
    if (this.Count > 0)
    {
      while (this.m_items.Count > 0)
      {
        this.m_items[0].RemoveSelf();
        this.m_items.RemoveAt(0);
      }
      this.m_startTr = (WTextRange) null;
      this.m_endTr = this.m_startTr;
    }
    return this.m_startIndex;
  }

  private void GetTextRanges(WTextRange startElement)
  {
    ParagraphItemCollection paragraphItemCollection = startElement.Owner is InlineContentControl ? (startElement.Owner as InlineContentControl).ParagraphItems : (startElement.Owner is Break ? (startElement.Owner as Break).OwnerParagraph.Items : startElement.OwnerParagraph.Items);
    for (int startIndex = this.m_startIndex; startIndex <= this.m_endIndex; ++startIndex)
    {
      WTextRange wtextRange = !(paragraphItemCollection[startIndex] is Break) || (paragraphItemCollection[startIndex] as Break).BreakType != BreakType.LineBreak ? paragraphItemCollection[startIndex] as WTextRange : (paragraphItemCollection[startIndex] as Break).TextRange;
      if (wtextRange != null)
        this.m_items.Add(wtextRange);
    }
  }

  internal void CacheRanges()
  {
    if (this.m_cachedRanges != null)
      return;
    WTextRange[] ranges = this.GetRanges();
    if (ranges == null)
      return;
    this.m_cachedRanges = new WTextRange[ranges.Length];
    int index = 0;
    for (int length = ranges.Length; index < length; ++index)
      this.m_cachedRanges[index] = (WTextRange) ranges[index].Clone();
  }

  internal void CopyTo(
    WParagraph para,
    int startIndex,
    bool saveFormatting,
    WCharacterFormat srcFormat)
  {
    this.CacheRanges();
    foreach (Entity cachedRange in this.m_cachedRanges)
    {
      WTextRange wtextRange = (WTextRange) cachedRange.Clone();
      if (saveFormatting && srcFormat != null)
        wtextRange.CharacterFormat.ImportContainer((FormatBase) srcFormat);
      para.Items.Insert(startIndex, (IEntity) wtextRange);
      ++startIndex;
    }
  }

  internal void CopyTo(
    InlineContentControl inlineContentControl,
    int startIndex,
    bool saveFormatting,
    WCharacterFormat srcFormat)
  {
    this.CacheRanges();
    foreach (Entity cachedRange in this.m_cachedRanges)
    {
      WTextRange wtextRange = (WTextRange) cachedRange.Clone();
      if (saveFormatting && srcFormat != null)
        wtextRange.CharacterFormat.ImportContainer((FormatBase) srcFormat);
      inlineContentControl.ParagraphItems.Insert(startIndex, (IEntity) wtextRange);
      ++startIndex;
    }
  }

  public IEnumerator GetEnumerator()
  {
    string[] strArray = new string[this.Count];
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      strArray[index] = this[index];
    return strArray.GetEnumerator();
  }

  private void EnsureIndexes()
  {
    if (this.m_startTr.Owner != this.OwnerParagraph && (!(this.m_startTr.Owner is Break) || (this.m_startTr.Owner as Break).BreakType != BreakType.LineBreak) && !(this.m_startTr.Owner is InlineContentControl) && this.m_endTr.Owner != this.OwnerParagraph && (!(this.m_endTr.Owner is Break) || (this.m_endTr.Owner as Break).BreakType != BreakType.LineBreak) && !(this.m_endTr.Owner is InlineContentControl))
      throw new InvalidOperationException();
    int itemsCount1 = this.GetItemsCount(this.m_startTr);
    if (this.m_startTr.Owner is Break && (this.m_startTr.Owner as Break).BreakType == BreakType.LineBreak)
      this.m_startIndex = (this.m_startTr.Owner as Break).GetIndexInOwnerCollection();
    else if (this.m_startTr.Owner is InlineContentControl && (this.m_startIndex >= itemsCount1 || this.m_startTr != (this.m_startTr.Owner as InlineContentControl).ParagraphItems[this.m_startIndex]))
      this.m_startIndex = this.m_startTr.GetIndexInOwnerCollection();
    else if (!(this.m_startTr.Owner is InlineContentControl) && (this.m_startIndex >= itemsCount1 || this.m_startTr != this.OwnerParagraph.Items[this.m_startIndex]))
      this.m_startIndex = this.m_startTr.GetIndexInOwnerCollection();
    int itemsCount2 = this.GetItemsCount(this.m_endTr);
    if (this.m_endTr.Owner is Break && (this.m_endTr.Owner as Break).BreakType == BreakType.LineBreak)
      this.m_endIndex = (this.m_endTr.Owner as Break).GetIndexInOwnerCollection();
    else if (this.m_endTr.Owner is InlineContentControl && (this.m_endIndex >= itemsCount2 || this.m_endTr != (this.m_endTr.Owner as InlineContentControl).ParagraphItems[this.m_endIndex]))
    {
      this.m_endIndex = this.m_endTr.GetIndexInOwnerCollection();
    }
    else
    {
      if (this.m_endTr.Owner is InlineContentControl || this.m_endIndex < itemsCount2 && this.m_endTr == this.m_endTr.OwnerParagraph.Items[this.m_endIndex])
        return;
      this.m_endIndex = this.m_endTr.GetIndexInOwnerCollection();
    }
  }

  private int GetItemsCount(WTextRange textRange)
  {
    if (textRange.Owner is Break && (textRange.Owner as Break).BreakType == BreakType.LineBreak)
      return (textRange.Owner as Break).OwnerParagraph.Items.Count;
    return this.OwnerParagraph == null && textRange.Owner is InlineContentControl ? (textRange.Owner as InlineContentControl).ParagraphItems.Count : textRange.OwnerParagraph.Items.Count;
  }

  internal void SplitRanges()
  {
    if (this.m_startCut > 0)
    {
      WTextRange wtextRange = new WTextRange((IWordDocument) this.m_startTr.GetOwnerParagraphValue().Document);
      wtextRange.Text = this.m_startTr.Text.Substring(0, this.m_startCut);
      wtextRange.CharacterFormat.ImportContainer((FormatBase) this.m_startTr.CharacterFormat);
      this.m_startTr.Text = this.m_startTr.Text.Substring(this.m_startCut);
      if (this.m_startTr.Owner is InlineContentControl)
        (this.m_startTr.Owner as InlineContentControl).ParagraphItems.Insert(this.m_startIndex, (IEntity) wtextRange);
      else
        this.OwnerParagraph.Items.Insert(this.m_startIndex, (IEntity) wtextRange);
      ++this.m_startIndex;
      ++this.m_endIndex;
      if (this.SelectionChain != null)
        this.UpdateFollowingSelections(true);
      if (this.Count == 1 && this.m_endCut >= 0)
        this.m_endCut -= this.m_startCut;
      this.m_startCut = 0;
    }
    if (this.m_endCut <= 0)
      return;
    WTextRange wtextRange1 = new WTextRange((IWordDocument) this.m_endTr.GetOwnerParagraphValue().Document);
    wtextRange1.Text = this.m_endTr.Text.Substring(this.m_endCut);
    wtextRange1.CharacterFormat.ImportContainer((FormatBase) this.m_endTr.CharacterFormat);
    this.m_endTr.Text = this.m_endTr.Text.Substring(0, this.m_endCut);
    if (this.m_endTr.Owner is InlineContentControl)
      (this.m_endTr.Owner as InlineContentControl).ParagraphItems.Insert(this.m_endIndex + 1, (IEntity) wtextRange1);
    else
      this.OwnerParagraph.Items.Insert(this.m_endIndex + 1, (IEntity) wtextRange1);
    if (this.SelectionChain != null)
      this.UpdateFollowingSelections(false);
    this.m_endCut = -1;
  }

  private void UpdateFollowingSelections(bool forStart)
  {
    foreach (TextSelection textSelection in (List<TextSelection>) this.SelectionChain)
    {
      if (textSelection != this)
      {
        WTextRange wtextRange = forStart ? this.m_startTr : this.m_endTr;
        int num = forStart ? this.m_startCut : this.m_endCut;
        if (textSelection.m_startTr == wtextRange)
        {
          if (!forStart)
          {
            textSelection.m_startTr = (WTextRange) this.m_endTr.NextSibling;
            textSelection.m_items[0] = textSelection.m_startTr;
            textSelection.m_startTr.SafeText = true;
          }
          textSelection.m_startCut -= num;
        }
        if (textSelection.m_endTr == wtextRange)
        {
          if (!forStart)
          {
            textSelection.m_endTr = (WTextRange) this.m_endTr.NextSibling;
            textSelection.m_items[textSelection.m_items.Count - 1] = textSelection.m_endTr;
            textSelection.m_endTr.SafeText = true;
          }
          if (textSelection.m_endCut >= 0)
            textSelection.m_endCut -= num;
        }
      }
    }
  }
}
