// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TextFinder
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class TextFinder
{
  private List<WParagraph> m_linePCol;
  [ThreadStatic]
  public static TextFinder m_instance;

  public static TextFinder Instance
  {
    get
    {
      if (TextFinder.m_instance == null)
        TextFinder.m_instance = new TextFinder();
      return TextFinder.m_instance;
    }
  }

  internal List<WParagraph> SingleLinePCol
  {
    get
    {
      if (this.m_linePCol == null)
        this.m_linePCol = new List<WParagraph>();
      return this.m_linePCol;
    }
  }

  public TextSelectionList Find(WParagraph para, Regex pattern, bool onlyFirstMatch)
  {
    string text = para.Text;
    MatchCollection matchCollection = pattern.Matches(text);
    TextSelectionList selections = new TextSelectionList();
    if (matchCollection.Count > 0)
    {
      foreach (Match match in matchCollection)
      {
        int index = match.Index;
        int endCharPos = match.Index + match.Length;
        TextSelection textSelection = new TextSelection(para, index, endCharPos);
        if (textSelection.Count != 0)
        {
          textSelection.SelectionChain = selections;
          selections.Add(textSelection);
        }
        if (onlyFirstMatch)
          break;
      }
    }
    TextFinder.FindInItems(para.Items, pattern, onlyFirstMatch, selections);
    return selections;
  }

  private static void FindInItems(
    ParagraphItemCollection items,
    Regex pattern,
    bool onlyFirstMatch,
    TextSelectionList selections)
  {
    if (selections.Count > 0 && onlyFirstMatch)
      return;
    foreach (ParagraphItem paragraphItem in (CollectionImpl) items)
    {
      WTextBody textBody = TextFinder.GetTextBody(paragraphItem);
      if (textBody == null)
      {
        if (paragraphItem is InlineContentControl)
        {
          TextFinder.FindInItems((paragraphItem as InlineContentControl).ParagraphItems, pattern, onlyFirstMatch, selections);
          if (selections.Count > 0 && onlyFirstMatch)
            break;
        }
        else if (paragraphItem is GroupShape && TextFinder.Find(((GroupShape) paragraphItem).ChildShapes, pattern, onlyFirstMatch, selections))
          break;
      }
      else if (TextFinder.Find(textBody, pattern, onlyFirstMatch, selections))
        break;
    }
  }

  private static bool Find(
    ChildShapeCollection childShapes,
    Regex pattern,
    bool onlyFirstMatch,
    TextSelectionList selections)
  {
    foreach (ChildShape childShape in (CollectionImpl) childShapes)
    {
      if (childShape is ChildGroupShape)
      {
        if (TextFinder.Find(((ChildGroupShape) childShape).ChildShapes, pattern, onlyFirstMatch, selections))
          return true;
      }
      else if (childShape.HasTextBody && TextFinder.Find(childShape.TextBody, pattern, onlyFirstMatch, selections))
        return true;
    }
    return false;
  }

  private static bool Find(
    WTextBody textBody,
    Regex pattern,
    bool onlyFirstMatch,
    TextSelectionList selections)
  {
    if (onlyFirstMatch)
    {
      TextSelection textSelection = textBody.Find(pattern);
      if (textSelection != null)
      {
        selections.Add(textSelection);
        return true;
      }
    }
    else
    {
      TextSelectionList all = textBody.FindAll(pattern);
      if (all != null && all.Count > 0)
        selections.AddRange((IEnumerable<TextSelection>) all);
    }
    return false;
  }

  private static WTextBody GetTextBody(ParagraphItem item)
  {
    WTextBody textBody = (WTextBody) null;
    switch (item.EntityType)
    {
      case EntityType.Comment:
        textBody = ((WComment) item).TextBody;
        break;
      case EntityType.Footnote:
        textBody = ((WFootnote) item).TextBody;
        break;
      case EntityType.TextBox:
        textBody = ((WTextBox) item).TextBoxBody;
        break;
      case EntityType.AutoShape:
        textBody = ((Shape) item).TextBody;
        break;
    }
    return textBody;
  }

  public TextSelection[] FindSingleLine(WTextBody textBody, Regex pattern)
  {
    return textBody.Items.Count == 0 ? (TextSelection[]) null : this.FindSingleLine(textBody, pattern, 0, textBody.Items.Count - 1);
  }

  public TextSelection[] FindSingleLine(
    WTextBody textBody,
    Regex pattern,
    int startIndex,
    int endIndex)
  {
    TextSelection[] singleLine = (TextSelection[]) null;
    for (int index = startIndex; index <= endIndex; ++index)
    {
      if (textBody.Items[index] is WParagraph)
      {
        WParagraph para = textBody.Items[index] as WParagraph;
        TextSelection[] inItems = this.FindInItems(para, pattern, 0, para.Items.Count - 1);
        if (inItems != null)
          return inItems;
        singleLine = this.FindSingleLine(pattern);
      }
      else if (textBody.Items[index] is WTable)
        singleLine = this.FindSingleLine(textBody.Items[index] as WTable, pattern);
      if (singleLine != null)
        return singleLine;
    }
    return this.FindSingleLine(pattern);
  }

  internal TextSelection[] FindInItems(
    WParagraph para,
    Regex pattern,
    int startIndex,
    int endIndex)
  {
    if (!this.SingleLinePCol.Contains(para))
      this.SingleLinePCol.Add(para);
    TextSelection[] inItems = (TextSelection[]) null;
    for (int index = startIndex; index <= endIndex; ++index)
    {
      ParagraphItem paragraphItem = para[index];
      WTextBody textBody = TextFinder.GetTextBody(paragraphItem);
      if (textBody == null)
      {
        if (paragraphItem is GroupShape)
          inItems = this.FindInItems(((GroupShape) paragraphItem).ChildShapes, pattern);
      }
      else
        inItems = this.FindSingleLine(textBody, pattern);
      if (inItems != null)
        return inItems;
    }
    return inItems;
  }

  private TextSelection[] FindInItems(ChildShapeCollection childShapes, Regex pattern)
  {
    TextSelection[] inItems = (TextSelection[]) null;
    foreach (ChildShape childShape in (CollectionImpl) childShapes)
    {
      if (childShape is ChildGroupShape)
        inItems = this.FindInItems(((ChildGroupShape) childShape).ChildShapes, pattern);
      else if (childShape.HasTextBody)
        inItems = this.FindSingleLine(childShape.TextBody, pattern);
      if (inItems != null)
        return inItems;
    }
    return inItems;
  }

  internal TextSelection[] FindSingleLine(Regex pattern)
  {
    if (this.m_linePCol == null || this.m_linePCol.Count == 0)
      return (TextSelection[]) null;
    string empty = string.Empty;
    Match match = (Match) null;
    StringBuilder stringBuilder = new StringBuilder();
    int index1 = 0;
    for (int count = this.m_linePCol.Count; index1 < count; ++index1)
    {
      WParagraph wparagraph = this.m_linePCol[index1];
      stringBuilder.Append(wparagraph.Text);
      if (index1 == count - 1)
      {
        empty = stringBuilder.ToString();
        match = pattern.Match(empty);
      }
    }
    if (match != null && match.Success)
    {
      int index2 = match.Index;
      int num = index2 + match.Length;
      TextSelectionList textSelectionList;
      if (index2 == 0 && num == empty.Length)
      {
        textSelectionList = new TextSelectionList();
        foreach (WParagraph para in this.m_linePCol)
        {
          if (match.Length == para.Text.Length)
          {
            TextSelection textSelection = new TextSelection(para, 0, para.Text.Length);
            textSelectionList.Add(textSelection);
          }
        }
      }
      else
        textSelectionList = this.FindSingleLine(this.m_linePCol, match);
      if (textSelectionList != null && textSelectionList.Count > 0)
      {
        this.m_linePCol.Clear();
        return textSelectionList.ToArray();
      }
    }
    return (TextSelection[]) null;
  }

  internal TextSelection[] FindSingleLine(WTable table, Regex pattern)
  {
    TextSelection[] singleLine = (TextSelection[]) null;
    foreach (WTableRow row in (CollectionImpl) table.Rows)
    {
      foreach (WTextBody cell in (CollectionImpl) row.Cells)
      {
        singleLine = this.FindSingleLine(cell, pattern);
        if (singleLine != null)
          return singleLine;
      }
    }
    return singleLine;
  }

  private TextSelectionList FindSingleLine(List<WParagraph> paragraphs, Match match)
  {
    int index = match.Index;
    int num = index + match.Length;
    string empty = string.Empty;
    TextSelectionList singleLine = (TextSelectionList) null;
    foreach (WParagraph paragraph in paragraphs)
    {
      int startCharPos = -1;
      int endCharPos = -1;
      int length1 = empty.Length;
      empty += paragraph.Text;
      int length2 = empty.Length;
      if (length1 <= index && index <= length2)
      {
        singleLine = new TextSelectionList();
        startCharPos = index - length1;
      }
      if (length1 <= num && num <= length2)
        endCharPos = num - length1;
      if (startCharPos != -1 || endCharPos != -1)
      {
        if (startCharPos != -1 && endCharPos != -1)
        {
          singleLine.Add(new TextSelection(paragraph, startCharPos, endCharPos));
          break;
        }
        if (startCharPos != -1 && startCharPos < paragraph.Text.Length)
          singleLine.Add(new TextSelection(paragraph, startCharPos, paragraph.Text.Length));
        else if (endCharPos != -1 && endCharPos <= paragraph.Text.Length)
        {
          singleLine.Add(new TextSelection(paragraph, 0, endCharPos));
          break;
        }
      }
      else if (length1 > index && length2 < num && paragraph.Text != string.Empty)
        singleLine.Add(new TextSelection(paragraph, 0, paragraph.Text.Length));
    }
    return singleLine;
  }

  internal static void Close()
  {
    if (TextFinder.m_instance == null)
      return;
    TextFinder.m_instance = (TextFinder) null;
  }
}
