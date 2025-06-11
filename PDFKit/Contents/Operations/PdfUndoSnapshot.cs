// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Operations.PdfUndoSnapshot
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace PDFKit.Contents.Operations;

internal class PdfUndoSnapshot
{
  private readonly PdfParagraphImpl paragraph;
  private readonly int startCaret;
  private readonly int endCaret;
  private readonly int editStartLineIndex;
  private readonly int editEndLineIndex;
  private readonly bool cloneInRangeOnly;
  private int paraId;
  private PdfTypeDictionary pageDict;
  private int pageIndex;
  private int insertObjAt = -1;
  private int lineCloneStart = -1;
  private int lineCloneEnd = -1;
  private List<PdfUndoSnapshot.LineSnapshotNode> nodes;
  private List<TextLine> cloneLines;

  public PdfUndoSnapshot(
    PdfParagraphImpl para,
    int startCaret,
    int endCaret,
    int editStartLineIndex,
    int editEndLineIndex,
    bool cloneInRangeOnly = false)
  {
    this.paragraph = para;
    this.startCaret = startCaret;
    this.endCaret = endCaret;
    this.editStartLineIndex = editStartLineIndex;
    this.editEndLineIndex = editEndLineIndex;
    this.cloneInRangeOnly = cloneInRangeOnly;
    this.nodes = new List<PdfUndoSnapshot.LineSnapshotNode>();
    this.cloneLines = new List<TextLine>();
    this.Initialize();
  }

  private void Initialize()
  {
    this.pageDict = this.paragraph.Page.Dictionary;
    this.pageIndex = this.paragraph.Page.PageIndex;
    this.paraId = this.paragraph.Paragraph.Id;
    bool flag = false;
    for (int index1 = 0; index1 < this.paragraph.Paragraph.Lines.Count; ++index1)
    {
      TextLine line = this.paragraph.Paragraph.Lines[index1];
      PdfUndoSnapshot.LineSnapshotNode lineSnapshotNode = new PdfUndoSnapshot.LineSnapshotNode()
      {
        LineBox = line.GetBoundingBox(false)
      };
      if (line.TextObjects.Count > 0)
      {
        for (int index2 = 0; index2 < line.TextObjects.Count; ++index2)
        {
          PdfTextObject textObject = line.TextObjects[index2];
          if (index2 == 0 || lineSnapshotNode.LineHeaderObject == null)
            lineSnapshotNode.LineHeaderObject = textObject;
          lineSnapshotNode.LineCharCount += textObject.CharsCount;
        }
        if (this.insertObjAt == -1)
          this.insertObjAt = this.paragraph.Page.PageObjects.IndexOf((PdfPageObject) line.TextObjects[0]);
      }
      else
      {
        lineSnapshotNode.LineCharCount = 0;
        lineSnapshotNode.EmptyLineId = index1;
        line.Id = index1;
      }
      this.nodes.Add(lineSnapshotNode);
      if (!flag && index1 >= this.editStartLineIndex)
      {
        TextLine textLine = line.Clone();
        if (this.lineCloneStart == -1)
          this.lineCloneStart = index1;
        this.lineCloneEnd = index1;
        this.cloneLines.Add(textLine);
        if (index1 >= this.editEndLineIndex)
        {
          if (this.cloneInRangeOnly)
            flag = true;
          if (line.ReturnFlag)
            flag = true;
        }
      }
    }
  }

  public void FillUndoItem(ModifyParagraphUndoItem undoItem, bool compareX)
  {
    undoItem.ParagraphId = this.paraId;
    undoItem.PageDict = this.pageDict;
    undoItem.Caret = this.startCaret;
    undoItem.EndCaret = this.endCaret;
    undoItem.StartLineIndex = this.editStartLineIndex;
    undoItem.EndLineIndex = this.editEndLineIndex;
    undoItem.PageIndex = this.pageIndex;
    if (undoItem.InsertReturnAt == InsertReturnAtEnum.LineHeader || undoItem.InsertReturnAt == InsertReturnAtEnum.LineTail)
    {
      int endLineIndex = undoItem.EndLineIndex;
      undoItem.EndLineIndex = this.editStartLineIndex;
      undoItem.DeleteStartLineIndex = this.editStartLineIndex;
      undoItem.DeleteEndLineIndex = this.editStartLineIndex;
      if (undoItem.InsertReturnAt == InsertReturnAtEnum.LineTail)
      {
        undoItem.StartLineIndex = this.editStartLineIndex + 1;
        undoItem.EndLineIndex = this.editStartLineIndex + 1;
        undoItem.DeleteStartLineIndex = this.editStartLineIndex + 1;
        undoItem.DeleteEndLineIndex = this.editStartLineIndex + 1;
        ++endLineIndex;
      }
      undoItem.LinesOffsetX = 0.0f;
      undoItem.LinesOffsetY = 0.0f;
      if (endLineIndex >= this.paragraph.Lines.Count)
        return;
      FS_RECTF boundingBox = this.paragraph.Lines[endLineIndex].GetBoundingBox(false);
      undoItem.LinesOffsetX = 0.0f;
      undoItem.LinesOffsetY = this.nodes[undoItem.EndLineIndex].LineBox.bottom - boundingBox.bottom;
      undoItem.OffsetStartLineIndex = undoItem.StartLineIndex;
    }
    else
    {
      if (this.insertObjAt == -1)
        this.insertObjAt = this.paragraph.Page.PageObjects.Count - 1;
      if (undoItem.UndoType == UndoTypes.SetTextColor || undoItem.UndoType == UndoTypes.SetUnderline || undoItem.UndoType == UndoTypes.SetStrikeout || undoItem.UndoType == UndoTypes.SetBold || undoItem.UndoType == UndoTypes.SetItalic)
      {
        undoItem.LinesOffsetX = 0.0f;
        undoItem.LinesOffsetY = 0.0f;
        undoItem.OffsetStartLineIndex = -1;
        undoItem.DeleteStartLineIndex = this.editStartLineIndex;
        undoItem.DeleteEndLineIndex = this.editStartLineIndex + (this.cloneLines.Count > 0 ? this.cloneLines.Count - 1 : 0);
        undoItem.InsertAtIndex = this.insertObjAt;
        undoItem.CloneLines = this.cloneLines.ToList<TextLine>();
      }
      else
      {
        int num1 = 0;
        int count1 = this.nodes.Count;
        int index1 = count1 - 1;
        int num2 = index1;
        int count2 = this.paragraph.Lines.Count;
        int num3 = count2 - 1;
        for (int index2 = count2 - 1; index2 >= this.editStartLineIndex && index1 >= 0; --index2)
        {
          TextLine line = this.paragraph.Lines[index2];
          PdfUndoSnapshot.LineSnapshotNode node = this.nodes[index1];
          if (!PdfUndoSnapshot.IsLineChanged(node, line, compareX))
          {
            ++num1;
            num3 = index2;
            num2 = index1;
            FS_RECTF boundingBox = line.GetBoundingBox(false);
            undoItem.LinesOffsetY = node.LineBox.bottom - boundingBox.bottom;
            undoItem.LinesOffsetX = node.LineBox.left - boundingBox.left;
            --index1;
          }
          else
            break;
        }
        int num4 = count2 - count1;
        if (num1 == 0)
        {
          undoItem.OffsetStartLineIndex = -1;
          undoItem.DeleteStartLineIndex = this.editStartLineIndex;
          undoItem.DeleteEndLineIndex = count2 - 1;
          undoItem.CloneLines = this.cloneLines.ToList<TextLine>();
        }
        else if (num4 > 0)
        {
          undoItem.OffsetStartLineIndex = num3;
          undoItem.DeleteStartLineIndex = this.editStartLineIndex;
          undoItem.DeleteEndLineIndex = num3 - 1;
          undoItem.CloneLines = num2 <= this.lineCloneEnd ? this.cloneLines.Take<TextLine>(num2 - this.lineCloneStart).ToList<TextLine>() : this.cloneLines.ToList<TextLine>();
        }
        else if (num4 < 0)
        {
          undoItem.OffsetStartLineIndex = num3;
          if (num3 > this.editStartLineIndex)
          {
            undoItem.DeleteStartLineIndex = this.editStartLineIndex;
            undoItem.DeleteEndLineIndex = num3 - 1;
          }
          else if (num3 == this.editStartLineIndex)
          {
            undoItem.DeleteStartLineIndex = -1;
            undoItem.DeleteEndLineIndex = -1;
          }
          undoItem.CloneLines = num2 <= this.lineCloneEnd ? this.cloneLines.Take<TextLine>(num2 - this.lineCloneStart).ToList<TextLine>() : this.cloneLines.ToList<TextLine>();
        }
        else
        {
          undoItem.OffsetStartLineIndex = num3;
          if (num3 > this.editStartLineIndex)
          {
            undoItem.DeleteStartLineIndex = this.editStartLineIndex;
            undoItem.DeleteEndLineIndex = num3 - 1;
          }
          else if (num3 == this.editStartLineIndex)
          {
            undoItem.DeleteStartLineIndex = -1;
            undoItem.DeleteEndLineIndex = -1;
          }
          undoItem.CloneLines = num2 <= this.lineCloneEnd ? this.cloneLines.Take<TextLine>(num2 - this.lineCloneStart).ToList<TextLine>() : this.cloneLines.ToList<TextLine>();
        }
        undoItem.InsertAtIndex = this.insertObjAt;
      }
    }
  }

  private static bool IsLineChanged(
    PdfUndoSnapshot.LineSnapshotNode node,
    TextLine line,
    bool compareX)
  {
    if (line.TextObjects.Count == 0)
      return node.LineHeaderObject != null || node.EmptyLineId != line.Id;
    if (node.LineHeaderObject != null)
    {
      PdfTextObject textObject = line.TextObjects[0];
      if (textObject.Handle != node.LineHeaderObject.Handle || node.LineCharCount != line.CharCount)
        return true;
      FS_RECTF boundingBox = line.GetBoundingBox(false);
      if (compareX && (double) boundingBox.left != (double) node.LineBox.left || !line.IsRotate && !line.IsVertWriteMode && (double) boundingBox.right != (double) node.LineBox.right || textObject.Font.Handle != node.LineHeaderObject.Font.Handle || (double) textObject.FontSize != (double) node.LineHeaderObject.FontSize || textObject.RenderMode != node.LineHeaderObject.RenderMode || (double) textObject.Matrix.a != (double) node.LineHeaderObject.Matrix.a)
        return true;
    }
    return false;
  }

  private class LineSnapshotNode
  {
    public PdfTextObject LineHeaderObject { get; set; }

    public int LineCharCount { get; set; }

    public FS_RECTF LineBox { get; set; }

    public int EmptyLineId { get; set; } = -1;
  }
}
