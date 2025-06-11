// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TextReplacer
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class TextReplacer
{
  [ThreadStatic]
  public static TextReplacer m_instance;

  public static TextReplacer Instance
  {
    get
    {
      if (TextReplacer.m_instance == null)
        TextReplacer.m_instance = new TextReplacer();
      return TextReplacer.m_instance;
    }
  }

  public int Replace(WParagraph para, Regex pattern, string replacement)
  {
    MatchCollection matchCollection = (MatchCollection) null;
    string text = para.Text;
    if (!string.IsNullOrEmpty(text))
      matchCollection = pattern.Matches(text);
    int num1 = 0;
    if (matchCollection != null && matchCollection.Count > 0)
    {
      int num2 = 0;
      int length1 = replacement.Length;
      foreach (Match match in matchCollection)
      {
        int num3 = match.Index + num2;
        int length2 = match.Length;
        int offset = length1 - match.Length;
        int endCharPos = num3 + match.Length;
        int startIndex = 0;
        WTextRange tr;
        if (this.EnsureStartAndEndOwner(para, num3, endCharPos, out tr, out startIndex))
        {
          para.ReplaceWithoutCorrection(num3, length2, replacement);
          ++num1;
          int num4 = tr.StartPos + tr.TextLength;
          if (tr.Owner is Break && (tr.Owner as Break).BreakType == BreakType.LineBreak)
          {
            num4 = (tr.Owner as Break).EndPos;
            tr.StartPos = (tr.Owner as Break).StartPos;
            int inOwnerCollection = tr.Owner.GetIndexInOwnerCollection();
            para.Items.UnsafeRemoveAt(inOwnerCollection);
            para.Items.InsertToInnerList(inOwnerCollection, (IEntity) tr);
            tr.SetOwner((OwnerHolder) para);
            tr.TextLength = num4 - tr.StartPos;
          }
          tr.SafeText = false;
          ParagraphItem pItem = (ParagraphItem) tr;
          if (num4 >= num3 + length2)
          {
            tr.TextLength += offset;
          }
          else
          {
            WTextRange nextTr;
            if (tr.Owner is InlineContentControl)
              this.RemoveInternalItems((tr.Owner as InlineContentControl).ParagraphItems, num3 + length2, startIndex + 1, out nextTr);
            else
              this.RemoveInternalItems(para.Items, num3 + length2, startIndex + 1, out nextTr);
            int num5 = num3 + length2;
            if (nextTr != null)
            {
              nextTr.TextLength -= num5 - nextTr.StartPos;
              nextTr.StartPos = num5 + offset;
              ++startIndex;
              pItem = (ParagraphItem) nextTr;
            }
            tr.TextLength = num5 + offset - tr.StartPos;
          }
          this.CorrectNextItems(pItem, startIndex, offset);
          num2 += offset;
          if (para.Document.ReplaceFirst)
            break;
        }
      }
    }
    if (!para.Document.ReplaceFirst || num1 <= 0)
      num1 += TextReplacer.ReplaceInItems(para.Items, pattern, replacement);
    para.IsTextReplaced = false;
    return num1;
  }

  private bool EnsureStartAndEndOwner(
    WParagraph para,
    int startCharPos,
    int endCharPos,
    out WTextRange tr,
    out int startIndex)
  {
    startIndex = FindUtils.GetStartRangeIndex(para, startCharPos + 1, out tr);
    WTextRange startTextRange = tr;
    tr = (WTextRange) null;
    FindUtils.GetStartRangeIndex(para, endCharPos, out tr);
    WTextRange endTextRange = tr;
    tr = (WTextRange) null;
    tr = startTextRange;
    return FindUtils.EnsureSameOwner(startTextRange, endTextRange);
  }

  private static int ReplaceInItems(
    ParagraphItemCollection items,
    Regex pattern,
    string replacement)
  {
    int num = 0;
    foreach (ParagraphItem paragraphItem in (CollectionImpl) items)
    {
      WTextBody wtextBody = (WTextBody) null;
      switch (paragraphItem.EntityType)
      {
        case EntityType.Comment:
          wtextBody = ((WComment) paragraphItem).TextBody;
          break;
        case EntityType.Footnote:
          wtextBody = ((WFootnote) paragraphItem).TextBody;
          break;
        case EntityType.TextBox:
          wtextBody = ((WTextBox) paragraphItem).TextBoxBody;
          break;
        case EntityType.AutoShape:
          wtextBody = ((Shape) paragraphItem).TextBody;
          break;
      }
      if (wtextBody != null)
        num += wtextBody.Replace(pattern, replacement);
      if (paragraphItem is InlineContentControl)
        num += TextReplacer.ReplaceInItems((paragraphItem as InlineContentControl).ParagraphItems, pattern, replacement);
      else if (paragraphItem is GroupShape)
      {
        GroupShape groupShape = (GroupShape) paragraphItem;
        num += TextReplacer.ReplaceInItems(groupShape.ChildShapes, pattern, replacement);
      }
      if (paragraphItem.Document.ReplaceFirst && num > 0)
        return num;
    }
    return num;
  }

  private static int ReplaceInItems(
    ChildShapeCollection childShapes,
    Regex pattern,
    string replacement)
  {
    int num = 0;
    foreach (ChildShape childShape in (CollectionImpl) childShapes)
    {
      if (childShape is ChildGroupShape)
      {
        ChildGroupShape childGroupShape = (ChildGroupShape) childShape;
        num += TextReplacer.ReplaceInItems(childGroupShape.ChildShapes, pattern, replacement);
      }
      else if (childShape.HasTextBody)
        num += childShape.TextBody.Replace(pattern, replacement);
      if (childShape.Document.ReplaceFirst && num > 0)
        return num;
    }
    return num;
  }

  internal void ReplaceSingleLine(TextSelection[] findText, string replacement)
  {
    if (findText == null || findText.Length == 0)
      return;
    for (int index = findText.Length - 1; index > 0; --index)
    {
      TextSelection selection = findText[index];
      selection.SplitAndErase();
      this.RemoveOwnerPara(selection);
    }
    findText[0].GetAsOneRange().Text = replacement;
  }

  internal void ReplaceSingleLine(TextSelection[] findText, TextSelection replacement)
  {
    if (findText == null || findText.Length == 0)
      return;
    for (int index = findText.Length - 1; index >= 0; --index)
    {
      TextSelection selection = findText[index];
      int startIndex = selection.SplitAndErase();
      if (index == 0)
      {
        WParagraph ownerParagraph = selection.OwnerParagraph;
        replacement.CopyTo(ownerParagraph, startIndex, false, (WCharacterFormat) null);
      }
      else
        this.RemoveOwnerPara(selection);
    }
  }

  internal void ReplaceSingleLine(TextSelection[] findText, TextBodyPart replacement)
  {
    if (findText == null || findText.Length == 0)
      return;
    for (int index = findText.Length - 1; index >= 0; --index)
    {
      TextSelection selection = findText[index];
      int pItemIndex = selection.SplitAndErase();
      if (index == 0)
      {
        WParagraph ownerParagraph = selection.OwnerParagraph;
        replacement.PasteAt((ITextBody) ownerParagraph.OwnerTextBody, ownerParagraph.GetIndexInOwnerCollection(), pItemIndex);
      }
      else
        this.RemoveOwnerPara(selection);
    }
  }

  private void RemoveOwnerPara(TextSelection selection)
  {
    WParagraph ownerParagraph = selection.OwnerParagraph;
    if (ownerParagraph.Items.Count != 0)
      return;
    ownerParagraph.RemoveSelf();
  }

  private void RemoveInternalItems(
    ParagraphItemCollection paraItems,
    int end,
    int startIndex,
    out WTextRange nextTr)
  {
    bool flag = false;
    nextTr = (WTextRange) null;
    for (int index = startIndex; index < paraItems.Count; index = index - 1 + 1)
    {
      nextTr = paraItems[index] as WTextRange;
      if (nextTr != null)
      {
        int num = nextTr.StartPos + nextTr.TextLength;
        if (num > end)
          break;
        if (num == end)
          flag = true;
      }
      paraItems.UnsafeRemoveAt(index);
      if (flag)
      {
        nextTr = index < paraItems.Count ? paraItems[index] as WTextRange : (WTextRange) null;
        break;
      }
    }
  }

  private void CorrectNextItems(ParagraphItem pItem, int startIndex, int offset)
  {
    if (pItem.Owner is InlineContentControl)
    {
      pItem.Document.UpdateStartPosOfInlineContentControlItems(pItem.Owner as InlineContentControl, startIndex + 1, offset);
      Entity pItem1 = (Entity) pItem;
      while (!(pItem1 is WParagraph) && pItem1 != null)
      {
        pItem1 = pItem1.Owner;
        if (pItem1.Owner is WParagraph)
        {
          pItem.Document.UpdateStartPosOfParaItems(pItem1 as ParagraphItem, offset);
          break;
        }
        if (pItem1.Owner is InlineContentControl)
          pItem.Document.UpdateStartPosOfInlineContentControlItems(pItem1.Owner as InlineContentControl, pItem1.Index + 1, offset);
      }
    }
    else
      pItem.Document.UpdateStartPosOfParaItems(pItem, offset);
  }
}
