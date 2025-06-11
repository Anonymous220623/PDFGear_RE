// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Revision
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class Revision
{
  private string m_author = string.Empty;
  private string m_name = string.Empty;
  private DateTime m_date;
  private Range m_range;
  private RevisionType m_revisionType;
  private RevisionCollection m_childRevisions;
  private object m_ownerBase;
  private WordDocument m_doc;
  private byte m_bFlags;

  public string Author
  {
    get => this.m_author;
    internal set => this.m_author = value;
  }

  public DateTime Date
  {
    get => this.m_date;
    internal set => this.m_date = value;
  }

  internal Range Range
  {
    get
    {
      if (this.m_range == null)
        this.m_range = new Range();
      return this.m_range;
    }
  }

  public RevisionType RevisionType
  {
    get => this.m_revisionType;
    internal set => this.m_revisionType = value;
  }

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal RevisionCollection ChildRevisions
  {
    get
    {
      if (this.m_childRevisions == null)
        this.m_childRevisions = new RevisionCollection(this.m_doc);
      return this.m_childRevisions;
    }
  }

  internal object Owner
  {
    get => this.m_ownerBase;
    set => this.m_ownerBase = value;
  }

  internal bool IsAfterParagraphMark
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal bool IsAfterCellMark
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  internal bool IsAfterRowMark
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  internal bool IsAfterTableMark
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
  }

  public void Accept()
  {
    List<WCharacterFormat> wcharacterFormatList = new List<WCharacterFormat>();
    if (this.m_range == null)
      this.RemoveSelf();
    switch (this.RevisionType)
    {
      case RevisionType.Insertions:
      case RevisionType.MoveTo:
        while (this.Range.Count > 0)
        {
          if (this.Range.Items[0] is Entity)
          {
            RevisionCollection revisions = (this.Range.Items[0] as Entity).Document.Revisions;
            List<Revision> revisionsInternal = (this.Range.Items[0] as Entity).RevisionsInternal;
            if ((this.Range.Items[0] as Entity).RevisionsInternal.Count == 0)
            {
              this.Range.Items.Remove((object) (this.Range.Items[0] as Entity));
              if (this.Range.Count == 0 && revisions.InnerList.Contains((object) this))
                revisions.Remove(this);
            }
            else
              this.UnlinkRangeItem(this.Range.Items[0] as Entity, revisionsInternal[0], true);
            for (int index = 0; index < revisionsInternal.Count; ++index)
            {
              if (revisionsInternal[index].Range.Items.Count == 0 && revisions.InnerList.Contains((object) revisionsInternal[index]))
                revisions.Remove(revisionsInternal[index]);
            }
          }
          else
          {
            if (this.Range.Items[0] is WCharacterFormat && (this.Range.Items[0] as WCharacterFormat).OwnerBase is WParagraph)
              wcharacterFormatList.Add(this.Range.Items[0] as WCharacterFormat);
            this.UnlinkRangeItem(this.Range.Items[0] as FormatBase, this, true);
          }
        }
        foreach (OwnerHolder ownerHolder in wcharacterFormatList)
          this.MakeChangesForBreakCharFormat((TextBodyItem) (ownerHolder.OwnerBase as WParagraph), true);
        if (this.RevisionType == RevisionType.MoveTo)
        {
          this.ClearDependentRevision(true);
          break;
        }
        break;
      case RevisionType.Deletions:
      case RevisionType.MoveFrom:
        while (this.Range.Count > 0)
        {
          if (this.Range.Items[0] is Entity)
          {
            RevisionCollection revisions = (this.Range.Items[0] as Entity).Document.Revisions;
            List<Revision> revisionsInternal = (this.Range.Items[0] as Entity).RevisionsInternal;
            bool flag = false;
            while (revisionsInternal.Count > 0 && this.Range.Items.Count > 0)
            {
              this.UnlinkRangeItem(this.Range.Items[0] as Entity, revisionsInternal[0], true);
              flag = true;
            }
            if (!flag && revisionsInternal.Count == 0 && this.Range.Items.Count > 0)
            {
              this.Range.Items.Remove((object) (this.Range.Items[0] as Entity));
              if (this.Range.Count == 0 && revisions.InnerList.Contains((object) this))
                revisions.Remove(this);
            }
            for (int index = 0; index < revisionsInternal.Count; ++index)
            {
              if (revisionsInternal[index].Range.Items.Count == 0 && revisions.InnerList.Contains((object) revisionsInternal[index]))
                revisions.Remove(revisionsInternal[index]);
            }
          }
          else
          {
            if (this.Range.Items[0] is WCharacterFormat && (this.Range.Items[0] as WCharacterFormat).OwnerBase is WParagraph)
              wcharacterFormatList.Add(this.Range.Items[0] as WCharacterFormat);
            List<Revision> revisions = (this.Range.Items[0] as FormatBase).Revisions;
            for (int index = 0; index < revisions.Count; ++index)
              this.UnlinkRangeItem(this.Range.Items[0] as FormatBase, revisions[index], true);
          }
        }
        foreach (OwnerHolder ownerHolder in wcharacterFormatList)
          this.MakeChangesForBreakCharFormat((TextBodyItem) (ownerHolder.OwnerBase as WParagraph), true);
        if (this.RevisionType == RevisionType.MoveFrom)
        {
          this.ClearDependentRevision(true);
          break;
        }
        break;
      case RevisionType.Formatting:
      case RevisionType.StyleDefinitionChange:
        while (this.Range.Count > 0)
          this.UnlinkRangeItem(this.Range.Items[0] as FormatBase, this, true);
        break;
    }
    wcharacterFormatList.Clear();
  }

  private void ClearDependentRevision(bool isFromAccept)
  {
    if (this.RevisionType != RevisionType.MoveFrom && this.RevisionType != RevisionType.MoveTo)
      return;
    Revision dependentName = this.GetDependentName();
    if (dependentName == null)
      return;
    if (isFromAccept)
      dependentName.Accept();
    else
      dependentName.Reject();
  }

  private Revision GetDependentName()
  {
    if (!(this.Owner is RevisionCollection owner))
      return (Revision) null;
    foreach (Revision dependentName in (CollectionImpl) owner)
    {
      if (dependentName.Name == this.Name)
        return dependentName;
    }
    return (Revision) null;
  }

  private void UnlinkRangeItem(Entity entity, Revision revision, bool isFromAccept)
  {
    entity.RevisionsInternal.Remove(revision);
    if (entity.RevisionsInternal.Count == 0)
      revision.Range.Items.Remove((object) entity);
    this.RemoveItemFromCollectionn(revision, entity, isFromAccept);
    this.MakeChanges(revision, entity as ParagraphItem, isFromAccept);
    if (revision.Range.Count != 0)
      return;
    this.OnClearComplete(revision, (object) entity, isFromAccept);
  }

  private void RemoveItemFromCollectionn(Revision revision, Entity entity, bool isFromAccept)
  {
    if ((revision.RevisionType != RevisionType.Insertions || isFromAccept) && (revision.RevisionType != RevisionType.Deletions || !isFromAccept))
      return;
    for (int index = 0; index < entity.RevisionsInternal.Count; ++index)
    {
      if (entity.RevisionsInternal[index].Range.Count > 0 && entity.RevisionsInternal[index].Range.Items.Contains((object) entity))
        entity.RevisionsInternal[index].Range.Items.Remove((object) entity);
      if (entity.RevisionsInternal[index].Range.Count == 0)
        this.OnClearComplete(entity.RevisionsInternal[index], (object) entity, isFromAccept);
    }
  }

  private void RemoveItemFromCollectionn(
    Revision revision,
    FormatBase formatBase,
    bool isFromAccept)
  {
    if ((revision.RevisionType != RevisionType.Insertions || isFromAccept) && (revision.RevisionType != RevisionType.Deletions || !isFromAccept))
      return;
    for (int index = 0; index < formatBase.Revisions.Count; ++index)
    {
      if (formatBase.Revisions[index].Range.Count > 0 && formatBase.Revisions[index].Range.Items.Contains((object) formatBase))
        formatBase.Revisions[index].Range.Items.Remove((object) formatBase);
      if (formatBase.Revisions[index].Range.Count == 0)
        this.OnClearComplete(formatBase.Revisions[index], (object) formatBase, isFromAccept);
    }
  }

  private void MakeChanges(Revision revision, ParagraphItem item, bool acceptChanges)
  {
    if (this.IsToRemove(revision, acceptChanges))
    {
      item.RemoveSelf();
    }
    else
    {
      if (item.IsChangedCFormat && !acceptChanges)
        item.RemoveChanges();
      if (item is Break)
      {
        if (acceptChanges && item is Break && (item as Break).CharacterFormat.IsDeleteRevision || !acceptChanges && item is Break && (item as Break).CharacterFormat.IsInsertRevision)
          item.RemoveSelf();
        else
          (item as Break).TextRange.AcceptChanges();
      }
      item.AcceptChanges();
    }
  }

  private void MakeChanges(FormatBase format, bool acceptChanges)
  {
    switch (format)
    {
      case RowFormat _ when format.OwnerBase is WTableRow:
        this.MakeChanges(format.OwnerBase as WTableRow, acceptChanges);
        break;
      case RowFormat _ when format.OwnerBase is WTable:
        this.MakeChanges(format.OwnerBase as WTable, acceptChanges);
        break;
      case CellFormat _:
        this.MakeChanges(format.OwnerBase as WTableCell, acceptChanges);
        break;
      case WCharacterFormat _:
        this.MakeChanges(format as WCharacterFormat, acceptChanges);
        break;
      case WParagraphFormat _:
        this.MakeChanges(format as WParagraphFormat, acceptChanges);
        break;
      case WSectionFormat _:
        this.MakeChanges(format as WSectionFormat, acceptChanges);
        break;
    }
  }

  private void MakeChanges(WSectionFormat sectionFormat, bool acceptChanges)
  {
    WSection ownerBase = sectionFormat.OwnerBase as WSection;
    if (ownerBase.m_internalData != null && ownerBase.m_internalData.Length < 300 && ownerBase.m_internalData.Length > 0)
    {
      SinglePropertyModifierArray propertyModifierArray = new SinglePropertyModifierArray(ownerBase.m_internalData, 0);
      if (propertyModifierArray.TryGetSprm(12857) != null)
      {
        while (propertyModifierArray.TryGetSprm(12857) != null)
          propertyModifierArray.Modifiers.Remove(propertyModifierArray.Modifiers[acceptChanges ? 0 : propertyModifierArray.Count - 1]);
        ownerBase.m_internalData = new byte[ownerBase.m_internalData.Length];
        propertyModifierArray.Save(ownerBase.m_internalData, 0);
      }
    }
    Dictionary<int, object> oldFormatBase1 = new Dictionary<int, object>((IDictionary<int, object>) ownerBase.SectionFormat.OldPropertiesHash);
    Dictionary<int, object> oldFormatBase2 = new Dictionary<int, object>((IDictionary<int, object>) ownerBase.PageSetup.OldPropertiesHash);
    Dictionary<int, object> oldFormatBase3 = new Dictionary<int, object>((IDictionary<int, object>) ownerBase.PageSetup.PageNumbers.OldPropertiesHash);
    Dictionary<int, object> oldFormatBase4 = new Dictionary<int, object>((IDictionary<int, object>) ownerBase.PageSetup.Margins.OldPropertiesHash);
    if (sectionFormat.PageSetup != null)
    {
      this.MakeChangesInPropertiesHash((FormatBase) sectionFormat.PageSetup, acceptChanges, oldFormatBase2);
      if (sectionFormat.PageSetup.Margins != null)
        this.MakeChangesInPropertiesHash((FormatBase) sectionFormat.PageSetup.Margins, acceptChanges, oldFormatBase4);
      if (sectionFormat.PageSetup.PageNumbers != null)
        this.MakeChangesInPropertiesHash((FormatBase) sectionFormat.PageSetup.PageNumbers, acceptChanges, oldFormatBase3);
    }
    if (sectionFormat.Columns != null && sectionFormat.Columns.Count > 0)
    {
      if (acceptChanges)
      {
        if (ownerBase.SectionFormat.SectFormattingColumnCollection != null)
          sectionFormat.SectFormattingColumnCollection.InnerList.Clear();
      }
      else if (sectionFormat.SectFormattingColumnCollection != null)
      {
        ownerBase.Columns.InnerList.Clear();
        for (int index = 0; index < ownerBase.SectionFormat.SectFormattingColumnCollection.Count; ++index)
        {
          Column column = new Column((IWordDocument) ownerBase.Document);
          ownerBase.Columns.Add(column);
          foreach (KeyValuePair<int, object> keyValuePair in ownerBase.SectionFormat.SectFormattingColumnCollection[index].OldPropertiesHash)
            ownerBase.Columns[index].PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
        }
      }
    }
    this.MakeChangesInPropertiesHash((FormatBase) sectionFormat, acceptChanges, oldFormatBase1);
    if (acceptChanges)
    {
      sectionFormat.PropertiesHash.Remove(4);
    }
    else
    {
      sectionFormat.PropertiesHash.Remove(5);
      sectionFormat.PropertiesHash.Remove(6);
    }
  }

  private void MakeChangesInPropertiesHash(
    FormatBase formatBase,
    bool acceptChanges,
    Dictionary<int, object> oldFormatBase)
  {
    if (acceptChanges)
    {
      formatBase.AcceptChanges();
      if (formatBase.OldPropertiesHash.Count <= 0)
        return;
      formatBase.OldPropertiesHash.Clear();
    }
    else
    {
      formatBase.RemoveChanges();
      formatBase.PropertiesHash.Clear();
      formatBase.OldPropertiesHash.Clear();
      foreach (KeyValuePair<int, object> keyValuePair in oldFormatBase)
        formatBase.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
    }
  }

  private void MakeChanges(WCharacterFormat format, bool acceptChanges)
  {
    if (acceptChanges)
    {
      format.AcceptChanges();
    }
    else
    {
      format.RemoveChanges();
      if (format.PropertiesHash.ContainsKey(103))
        format.OldPropertiesHash[103] = format.PropertiesHash[103];
      format.PropertiesHash.Clear();
      foreach (KeyValuePair<int, object> keyValuePair in format.OldPropertiesHash)
        format.PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
      format.OldPropertiesHash.Clear();
    }
  }

  private void MakeChangesForBreakCharFormat(TextBodyItem item, bool acceptChanges)
  {
    if (this.RemoveChangedItem(item, acceptChanges))
      return;
    bool next = this.CheckMoveToNext(item, acceptChanges);
    if (!acceptChanges)
      this.RemoveChangedFormat(item);
    if (item.IsInsertRevision || item.IsDeleteRevision || item.IsChangedCFormat)
      item.AcceptCChanges();
    if (item.IsChangedPFormat)
      item.AcceptPChanges();
    if (!next || !this.MoveToNextPara(item))
      return;
    item.RemoveSelf();
  }

  private void MakeChanges(WParagraphFormat format, bool acceptChanges)
  {
    WListFormat wlistFormat = (WListFormat) null;
    if (format.OwnerBase is WParagraph)
      wlistFormat = (format.OwnerBase as WParagraph).ListFormat;
    else if (format.OwnerBase is WParagraphStyle)
      wlistFormat = (format.OwnerBase as WParagraphStyle).ListFormat;
    if (acceptChanges && wlistFormat != null)
    {
      if (wlistFormat.OldPropertiesHash != null && wlistFormat.OldPropertiesHash.Count > 0)
        wlistFormat.OldPropertiesHash.Clear();
      if (format.m_unParsedSprms != null && format.m_unParsedSprms.Count > 0)
      {
        if (format.m_unParsedSprms[50757] != null)
          format.m_unParsedSprms.RemoveValue(50757);
        if (format.m_unParsedSprms[9283] != null)
          format.m_unParsedSprms.RemoveValue(9283);
      }
    }
    if (acceptChanges)
    {
      format.AcceptChanges();
    }
    else
    {
      format.RemoveChanges();
      format.PropertiesHash.Clear();
      foreach (KeyValuePair<int, object> keyValuePair in format.OldPropertiesHash)
        format.PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
      if (format.OldPropertiesHash.ContainsKey(47) && this.m_doc.Styles.FindByName(this.m_doc.StyleNameIds[(string) format.OldPropertiesHash[47]], StyleType.ParagraphStyle) is IWParagraphStyle byName && format.OwnerBase is WParagraph)
        (format.OwnerBase as WParagraph).ApplyStyle(byName, false);
      format.OldPropertiesHash.Clear();
      if (wlistFormat == null)
        return;
      wlistFormat.PropertiesHash.Clear();
      foreach (KeyValuePair<int, object> keyValuePair in wlistFormat.OldPropertiesHash)
        wlistFormat.PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
      wlistFormat.OldPropertiesHash.Clear();
    }
  }

  private void MakeChanges(WTable table, bool acceptChanges)
  {
    if (acceptChanges)
    {
      table.m_trackTblFormat = (XmlTableFormat) null;
      table.m_trackTableGrid = (WTableColumnCollection) null;
      if (table.DocxTableFormat.Format.OldPropertiesHash.Count > 0)
        table.DocxTableFormat.Format.OldPropertiesHash.Clear();
    }
    else if (table.m_trackTblFormat != null)
    {
      table.DocxTableFormat.Format.ClearFormatting();
      table.DocxTableFormat = table.TrackTblFormat.Clone(table);
      table.FirstRow.RowFormat.ClearFormatting();
      table.FirstRow.RowFormat.ImportContainer((FormatBase) table.TrackTblFormat.Format);
      table.m_trackTblFormat = (XmlTableFormat) null;
    }
    if (!acceptChanges && table.m_trackTableGrid != null)
    {
      table.TableGrid.InnerList.Clear();
      foreach (WTableColumn wtableColumn in (CollectionImpl) table.TrackTableGrid)
        table.TableGrid.AddColumns(wtableColumn.EndOffset);
      table.m_trackTableGrid = (WTableColumnCollection) null;
    }
    if (acceptChanges)
    {
      table.DocxTableFormat.Format.AcceptChanges();
    }
    else
    {
      table.DocxTableFormat.Format.RemoveChanges();
      table.DocxTableFormat.Format.PropertiesHash.Clear();
      foreach (KeyValuePair<int, object> keyValuePair in table.DocxTableFormat.Format.OldPropertiesHash)
        table.DocxTableFormat.Format.PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
      table.DocxTableFormat.Format.OldPropertiesHash.Clear();
    }
  }

  private void MakeChanges(WTableRow row, bool acceptChanges)
  {
    if (acceptChanges)
    {
      row.RowFormat.AcceptChanges();
    }
    else
    {
      row.RowFormat.RemoveChanges();
      row.RowFormat.PropertiesHash.Clear();
      foreach (KeyValuePair<int, object> keyValuePair in row.RowFormat.OldPropertiesHash)
        row.RowFormat.PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
      row.RowFormat.OldPropertiesHash.Clear();
    }
    if (acceptChanges)
    {
      row.m_trackRowFormat = (RowFormat) null;
      if (row.RowFormat.OldPropertiesHash.Count > 0)
        row.RowFormat.OldPropertiesHash.Clear();
    }
    else if (row.m_trackRowFormat != null)
    {
      row.RowFormat.ClearFormatting();
      row.m_trackRowFormat = (RowFormat) null;
    }
    if (row.IsDeleteRevision && acceptChanges || row.IsInsertRevision && !acceptChanges)
    {
      if (row.OwnerTable != null && row.OwnerTable.ChildEntities.Count == 1)
        row.OwnerTable.RemoveSelf();
      else
        row.RemoveSelf();
    }
    else if (row.IsInsertRevision && acceptChanges)
    {
      row.IsInsertRevision = false;
    }
    else
    {
      if (!row.IsDeleteRevision || acceptChanges)
        return;
      row.IsDeleteRevision = false;
    }
  }

  private void MakeChanges(WTableCell cell, bool acceptChanges)
  {
    if (acceptChanges)
    {
      cell.m_trackCellFormat = (CellFormat) null;
      if (cell.CellFormat.OldPropertiesHash.Count > 0)
        cell.CellFormat.OldPropertiesHash.Clear();
    }
    else if (cell.m_trackCellFormat != null)
    {
      cell.CellFormat.ClearFormatting();
      cell.CellFormat.ImportContainer((FormatBase) cell.TrackCellFormat);
      cell.m_trackCellFormat = (CellFormat) null;
    }
    if (acceptChanges)
    {
      cell.CellFormat.AcceptChanges();
    }
    else
    {
      cell.CellFormat.RemoveChanges();
      if (cell.CellFormat.PropertiesHash.ContainsKey(14) && cell.CellFormat.OldPropertiesHash.ContainsKey(14) && cell.CellFormat.PropertiesHash.ContainsKey(12) && cell.CellFormat.PropertiesHash[14].Equals(cell.CellFormat.OldPropertiesHash[14]))
        cell.CellFormat.OldPropertiesHash[12] = cell.CellFormat.PropertiesHash[12];
      else if (cell.OwnerRow.OwnerTable != null)
        cell.OwnerRow.OwnerTable.IsTableGridUpdated = false;
      cell.CellFormat.PropertiesHash.Clear();
      foreach (KeyValuePair<int, object> keyValuePair in cell.CellFormat.OldPropertiesHash)
        cell.CellFormat.PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
      cell.CellFormat.OldPropertiesHash.Clear();
    }
  }

  private bool RemoveChangedItem(TextBodyItem item, bool acceptChanges)
  {
    if (!this.IsToRemove(this, acceptChanges) || !(item is WParagraph) || (item as WParagraph).ChildEntities.Count != 0)
      return false;
    item.RemoveSelf();
    return true;
  }

  private bool IsToRemove(Revision revision, bool acceptChanges)
  {
    if (revision.RevisionType == RevisionType.Insertions && !acceptChanges || revision.RevisionType == RevisionType.Deletions && acceptChanges || revision.RevisionType == RevisionType.MoveFrom && acceptChanges)
      return true;
    return revision.RevisionType == RevisionType.MoveTo && !acceptChanges;
  }

  private bool CheckMoveToNext(TextBodyItem item, bool acceptChanges)
  {
    bool next = false;
    if (item is WParagraph && item.NextSibling is WParagraph && (item.IsInsertRevision && !acceptChanges || item.IsDeleteRevision && acceptChanges))
      next = true;
    return next;
  }

  private bool MoveToNextPara(TextBodyItem item)
  {
    if (item is WParagraph wparagraph)
    {
      int num = wparagraph.Items.Count - 1;
      if (!(item.NextSibling is WParagraph nextSibling))
        return false;
      for (int index = num; index >= 0; --index)
        nextSibling.Items.Insert(0, (IEntity) wparagraph.Items[index]);
    }
    return true;
  }

  private void RemoveChangedFormat(TextBodyItem item)
  {
    if (item.IsChangedCFormat)
      item.RemoveCFormatChanges();
    if (!item.IsChangedPFormat)
      return;
    item.RemovePFormatChanges();
  }

  private void UnlinkRangeItem(FormatBase formatBase, Revision revision, bool isFromAccept)
  {
    formatBase.Revisions.Remove(revision);
    revision.Range.Items.Remove((object) formatBase);
    this.RemoveItemFromCollectionn(revision, formatBase, isFromAccept);
    if (!(formatBase is WCharacterFormat) || !(formatBase.OwnerBase is WParagraph) || revision.RevisionType == RevisionType.Formatting)
      this.MakeChanges(formatBase, isFromAccept);
    if (revision.Range.Count != 0)
      return;
    this.OnClearComplete(revision, (object) formatBase, isFromAccept);
  }

  private void OnClearComplete(Revision revision, object item, bool isFromAccept)
  {
    if (revision.ChildRevisions.Count > 0)
    {
      if (revision.RevisionType == RevisionType.MoveFrom || revision.RevisionType == RevisionType.MoveTo)
      {
        for (int index = 0; index < revision.ChildRevisions.Count; ++index)
        {
          while (revision.ChildRevisions[index].Range.Count > 0)
          {
            if (revision.ChildRevisions[index].Range.Items[0] is Entity)
              this.UnlinkRangeItem(revision.ChildRevisions[index].Range.Items[0] as Entity, this, isFromAccept);
            else
              this.UnlinkRangeItem(revision.ChildRevisions[index].Range.Items[0] as FormatBase, this, isFromAccept);
          }
        }
      }
      else
      {
        for (int index = 0; index < revision.ChildRevisions.Count; ++index)
        {
          if (item is FormatBase)
            (item as FormatBase).Document.Revisions.Add(revision.ChildRevisions[index]);
          else
            (item as Entity).Document.Revisions.Add(revision.ChildRevisions[index]);
        }
      }
    }
    RevisionCollection revisionCollection = !(item is FormatBase) ? (item as Entity).Document.Revisions : (item as FormatBase).Document.Revisions;
    if (!revisionCollection.InnerList.Contains((object) revision))
      return;
    revisionCollection.Remove(revision);
  }

  public void Reject()
  {
    List<WCharacterFormat> wcharacterFormatList = new List<WCharacterFormat>();
    if (this.m_range == null)
      this.RemoveSelf();
    switch (this.RevisionType)
    {
      case RevisionType.Insertions:
      case RevisionType.MoveTo:
        while (this.Range.Count > 0)
        {
          if (this.Range.Items[0] is Entity)
          {
            RevisionCollection revisions = (this.Range.Items[0] as Entity).Document.Revisions;
            List<Revision> revisionsInternal = (this.Range.Items[0] as Entity).RevisionsInternal;
            bool flag = false;
            while (revisionsInternal.Count > 0 && this.Range.Items.Count > 0)
            {
              this.UnlinkRangeItem(this.Range.Items[0] as Entity, revisionsInternal[0], false);
              flag = true;
            }
            if (!flag && revisionsInternal.Count == 0 && this.Range.Items.Count > 0)
            {
              this.Range.Items.Remove((object) (this.Range.Items[0] as Entity));
              if (this.Range.Count == 0 && revisions.InnerList.Contains((object) this))
                revisions.Remove(this);
            }
            for (int index = 0; index < revisionsInternal.Count; ++index)
            {
              if (revisionsInternal[index].Range.Items.Count == 0 && revisions.InnerList.Contains((object) revisionsInternal[index]))
                revisions.Remove(revisionsInternal[index]);
            }
          }
          else
          {
            if (this.Range.Items[0] is WCharacterFormat && (this.Range.Items[0] as WCharacterFormat).OwnerBase is WParagraph)
              wcharacterFormatList.Add(this.Range.Items[0] as WCharacterFormat);
            this.UnlinkRangeItem(this.Range.Items[0] as FormatBase, this, false);
          }
        }
        foreach (OwnerHolder ownerHolder in wcharacterFormatList)
          this.MakeChangesForBreakCharFormat((TextBodyItem) (ownerHolder.OwnerBase as WParagraph), false);
        if (this.m_doc.ClonedFields.Count > 0)
        {
          WField wfield = this.m_doc.ClonedFields.Peek();
          if (wfield.OwnerParagraph.Index == wfield.FieldEnd.OwnerParagraph.Index)
            this.m_doc.ClonedFields.Pop();
        }
        if (this.RevisionType == RevisionType.MoveTo)
        {
          this.ClearDependentRevision(false);
          break;
        }
        break;
      case RevisionType.Deletions:
      case RevisionType.MoveFrom:
        while (this.Range.Count > 0)
        {
          if (this.Range.Items[0] is Entity)
          {
            RevisionCollection revisions = (this.Range.Items[0] as Entity).Document.Revisions;
            List<Revision> revisionsInternal = (this.Range.Items[0] as Entity).RevisionsInternal;
            if ((this.Range.Items[0] as Entity).RevisionsInternal.Count == 0)
            {
              this.Range.Items.Remove((object) (this.Range.Items[0] as Entity));
              if (this.Range.Count == 0 && revisions.InnerList.Contains((object) this))
                revisions.Remove(this);
            }
            else
              this.UnlinkRangeItem(this.Range.Items[0] as Entity, revisionsInternal[0], false);
            for (int index = 0; index < revisionsInternal.Count; ++index)
            {
              if (revisionsInternal[index].Range.Items.Count == 0 && revisions.InnerList.Contains((object) revisionsInternal[index]))
                revisions.Remove(revisionsInternal[index]);
            }
          }
          else
          {
            if (this.Range.Items[0] is WCharacterFormat && (this.Range.Items[0] as WCharacterFormat).OwnerBase is WParagraph)
              wcharacterFormatList.Add(this.Range.Items[0] as WCharacterFormat);
            this.UnlinkRangeItem(this.Range.Items[0] as FormatBase, this, false);
          }
        }
        foreach (OwnerHolder ownerHolder in wcharacterFormatList)
          this.MakeChangesForBreakCharFormat((TextBodyItem) (ownerHolder.OwnerBase as WParagraph), false);
        if (this.RevisionType == RevisionType.MoveFrom)
        {
          this.ClearDependentRevision(false);
          break;
        }
        break;
      case RevisionType.Formatting:
      case RevisionType.StyleDefinitionChange:
        while (this.Range.Count > 0)
          this.UnlinkRangeItem(this.Range.Items[0] as FormatBase, this, false);
        break;
    }
    wcharacterFormatList.Clear();
  }

  internal void RemoveSelf() => (this.Owner as RevisionCollection).Remove(this);

  internal Revision Clone()
  {
    Revision revision = (Revision) this.MemberwiseClone();
    if (this.m_childRevisions != null)
    {
      revision.m_childRevisions = new RevisionCollection(this.m_doc);
      this.m_childRevisions.CloneItemsTo(revision.m_childRevisions);
    }
    if (this.m_range != null)
    {
      revision.m_range = new Range();
      this.m_range.CloneItemsTo(revision.m_range);
    }
    return revision;
  }

  internal Revision(WordDocument doc) => this.m_doc = doc;

  internal void Close()
  {
    if (this.m_range != null)
    {
      this.m_range.Close();
      this.m_range = (Range) null;
    }
    if (this.m_childRevisions == null)
      return;
    this.m_childRevisions.Close();
    this.m_childRevisions = (RevisionCollection) null;
  }
}
