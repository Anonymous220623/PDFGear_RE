// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.MailMerge
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class MailMerge
{
  private WordDocument m_doc;
  private MailMerge.GroupSelector m_groupSelector;
  private WSectionCollection m_contentSections;
  private string[] m_names;
  private string[] m_values;
  private byte m_bFlags = 1;
  private MailMergeSettings m_settings;
  private DbConnection m_conn;
  private DataSet m_curDataSet;
  private ArrayList m_commands;
  private DataSet m_dataSet;
  private Dictionary<string, IRowsEnumerator> m_nestedEnums;
  private Regex m_varCmdRegex;
  private Stack<MailMerge.GroupSelector> m_groupSelectors;
  private Dictionary<string, string> m_mappedFields;
  private MailMergeDataSet m_dataSetDocIO;
  private List<DictionaryEntry> m_commandsDocIO;
  private MailMergeDataSet m_curDataSetDocIO;
  private int m_mergedRecordCount;
  private Dictionary<string, bool> m_clearFieldsState;
  private Stack<WIfField> m_IfFieldCollections;
  private WMergeField _previousMergeField;
  private bool _isInValidNextField;

  private bool IsSqlConnection
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  private bool IsBeginGroupFound
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  private bool IsEndGroupFound
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  private bool IsNested
  {
    get => ((int) this.m_bFlags & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 191 | (value ? 1 : 0) << 6);
  }

  public bool ClearFields
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  protected WordDocument Document => this.m_doc;

  public bool RemoveEmptyParagraphs
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  public bool RemoveEmptyGroup
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
  }

  public bool InsertAsNewRow
  {
    get => ((int) this.m_bFlags & 128 /*0x80*/) >> 7 != 0;
    set
    {
      this.m_bFlags = (byte) ((int) this.m_bFlags & 65407 | (value ? 1 : 0) << 7);
      if (this.m_groupSelector == null)
        return;
      this.m_groupSelector.InsertAsNewRow = this.InsertAsNewRow;
    }
  }

  private Dictionary<string, IRowsEnumerator> NestedEnums
  {
    get
    {
      if (this.m_nestedEnums == null)
        this.m_nestedEnums = new Dictionary<string, IRowsEnumerator>();
      return this.m_nestedEnums;
    }
  }

  private DataSet CurrentDataSet
  {
    get
    {
      if (this.m_curDataSet == null)
        this.m_curDataSet = new DataSet();
      return this.m_curDataSet;
    }
  }

  private Regex VariableCommandRegex
  {
    get
    {
      if (this.m_varCmdRegex == null)
        this.m_varCmdRegex = new Regex("%([^\"%]+)%");
      return this.m_varCmdRegex;
    }
  }

  private Stack<MailMerge.GroupSelector> GroupSelectors
  {
    get
    {
      if (this.m_groupSelectors == null)
        this.m_groupSelectors = new Stack<MailMerge.GroupSelector>();
      return this.m_groupSelectors;
    }
  }

  public Dictionary<string, string> MappedFields
  {
    get
    {
      if (this.m_mappedFields == null)
        this.m_mappedFields = new Dictionary<string, string>();
      return this.m_mappedFields;
    }
  }

  private MailMergeDataSet CurrentDataSetDocIO
  {
    get
    {
      if (this.m_curDataSetDocIO == null)
        this.m_curDataSetDocIO = new MailMergeDataSet();
      return this.m_curDataSetDocIO;
    }
  }

  private Dictionary<string, bool> ClearFieldsState
  {
    get
    {
      if (this.m_clearFieldsState == null)
        this.m_clearFieldsState = new Dictionary<string, bool>();
      return this.m_clearFieldsState;
    }
  }

  public MailMergeSettings Settings
  {
    get
    {
      if (this.m_settings == null)
        this.m_settings = new MailMergeSettings();
      return this.m_settings;
    }
  }

  public event MergeFieldEventHandler MergeField;

  public event MergeImageFieldEventHandler MergeImageField;

  public event BeforeClearFieldEventHandler BeforeClearField;

  public event BeforeClearGroupFieldEventHandler BeforeClearGroupField;

  internal MailMerge(WordDocument document)
  {
    this.m_doc = document;
    this.m_contentSections = new WSectionCollection();
    this.m_groupSelector = new MailMerge.GroupSelector(new MailMerge.GroupSelector.GroupFound(this.OnGroupFound), this.InsertAsNewRow);
  }

  public void Execute(string[] fieldNames, string[] fieldValues)
  {
    this.Document.IsMailMerge = true;
    if (fieldNames == null)
      throw new ArgumentNullException(nameof (fieldNames));
    if (fieldValues == null)
      throw new ArgumentNullException(nameof (fieldValues));
    this.m_names = fieldNames;
    this.m_values = fieldValues;
    if (this.m_names.Length > 0)
    {
      for (int index = 0; index < this.Document.Sections.Count; ++index)
        this.ExecuteForSection((IWSection) this.Document.Sections[index], (IRowsEnumerator) null);
    }
    this.Document.IsMailMerge = false;
  }

  public void Execute(IEnumerable dataSource)
  {
    if (dataSource == null)
      throw new ArgumentNullException("datasource");
    this.ExecuteGroup(new MailMergeDataTable(string.Empty, dataSource));
  }

  public void ExecuteGroup(MailMergeDataTable dataSource)
  {
    if (dataSource == null)
      throw new ArgumentNullException("datasource");
    if (dataSource.GroupName == string.Empty)
      this.Execute((IRowsEnumerator) new DataTableEnumerator(dataSource));
    else
      this.ExecuteGroup((IRowsEnumerator) new DataTableEnumerator(dataSource));
  }

  public void Execute(DataRow row)
  {
    if (row == null)
      throw new ArgumentNullException(nameof (row));
    this.Execute((IRowsEnumerator) new DataTableEnumerator(row));
  }

  public void Execute(DataTable table)
  {
    if (table == null)
      throw new ArgumentNullException(nameof (table));
    this.Execute((IRowsEnumerator) new DataTableEnumerator(table));
  }

  public void Execute(DataView dataView)
  {
    if (dataView == null)
      throw new ArgumentNullException(nameof (dataView));
    this.Execute((IRowsEnumerator) new DataViewEnumerator(dataView));
  }

  public void Execute(OleDbDataReader dataReader)
  {
    if (dataReader == null)
      throw new ArgumentNullException(nameof (dataReader));
    this.Execute((IRowsEnumerator) new DataReaderEnumerator((IDataReader) dataReader));
  }

  public void Execute(IDataReader dataReader)
  {
    if (dataReader == null)
      throw new ArgumentNullException(nameof (dataReader));
    this.Execute((IRowsEnumerator) new DataReaderEnumerator(dataReader));
  }

  public void ExecuteGroup(DataTable table)
  {
    if (table == null)
      throw new ArgumentNullException(nameof (table));
    this.ExecuteGroup((IRowsEnumerator) new DataTableEnumerator(table));
  }

  public void ExecuteGroup(DataView dataView)
  {
    if (dataView == null)
      throw new ArgumentNullException(nameof (dataView));
    this.ExecuteGroup((IRowsEnumerator) new DataViewEnumerator(dataView));
  }

  public void ExecuteGroup(IDataReader dataReader)
  {
    if (dataReader == null)
      throw new ArgumentNullException(nameof (dataReader));
    this.ExecuteGroup((IRowsEnumerator) new DataReaderEnumerator(dataReader));
  }

  public void ExecuteNestedGroup(DbConnection conn, ArrayList commands)
  {
    if (conn == null)
      throw new ArgumentException(nameof (conn));
    if (commands == null)
      throw new ArgumentException(nameof (commands));
    this.RemoveSpellChecking();
    this.m_conn = conn;
    this.m_commands = commands;
    DictionaryEntry command = (DictionaryEntry) commands[0];
    this.Document.IsMailMerge = true;
    this.IsNested = true;
    this.ExecuteNestedGroup((string) command.Key);
    if (this.m_nestedEnums != null)
    {
      this.m_nestedEnums.Clear();
      this.m_nestedEnums = (Dictionary<string, IRowsEnumerator>) null;
    }
    if (this.m_curDataSet != null)
    {
      this.m_curDataSet.Clear();
      this.m_curDataSet = (DataSet) null;
    }
    this.Document.IsMailMerge = false;
    this.IsNested = false;
  }

  public void ExecuteNestedGroup(DbConnection conn, ArrayList commands, bool isSqlConnection)
  {
    this.IsSqlConnection = isSqlConnection;
    this.ExecuteNestedGroup(conn, commands);
  }

  public void ExecuteNestedGroup(DataSet dataSet, ArrayList commands)
  {
    if (dataSet == null)
      throw new ArgumentException(nameof (dataSet));
    if (commands == null)
      throw new ArgumentException(nameof (commands));
    this.RemoveSpellChecking();
    this.m_dataSet = dataSet.Copy();
    this.m_commands = commands;
    DictionaryEntry command = (DictionaryEntry) commands[0];
    this.Document.IsMailMerge = true;
    this.IsNested = true;
    this.ExecuteNestedGroup((string) command.Key);
    if (this.m_nestedEnums != null)
    {
      this.m_nestedEnums.Clear();
      this.m_nestedEnums = (Dictionary<string, IRowsEnumerator>) null;
    }
    if (this.m_dataSet != null)
    {
      this.m_dataSet.Clear();
      this.m_dataSet = (DataSet) null;
    }
    this.Document.IsMailMerge = false;
    this.IsNested = false;
  }

  public void ExecuteNestedGroup(MailMergeDataTable dataTable)
  {
    if (dataTable == null)
      throw new ArgumentException("dataset");
    this.RemoveSpellChecking();
    this.m_dataSetDocIO = new MailMergeDataSet();
    this.m_dataSetDocIO.Add((object) dataTable);
    this.Document.IsMailMerge = true;
    this.IsNested = true;
    this.ExecuteNestedGroup(dataTable.GroupName);
    if (this.m_nestedEnums != null)
    {
      this.m_nestedEnums.Clear();
      this.m_nestedEnums = (Dictionary<string, IRowsEnumerator>) null;
    }
    if (this.m_dataSetDocIO != null)
    {
      this.m_dataSetDocIO.Clear();
      this.m_dataSetDocIO = (MailMergeDataSet) null;
    }
    this.Document.IsMailMerge = false;
    this.IsNested = false;
  }

  public void ExecuteNestedGroup(MailMergeDataSet dataSource, List<DictionaryEntry> commands)
  {
    if (dataSource == null || dataSource.DataSet.Count == 0)
      throw new ArgumentException("dataSet is empty");
    if (commands == null || commands.Count == 0)
      throw new ArgumentException("commands list is empty");
    this.RemoveSpellChecking();
    this.m_dataSetDocIO = dataSource;
    this.m_commandsDocIO = commands;
    DictionaryEntry command = commands[0];
    this.Document.IsMailMerge = true;
    this.IsNested = true;
    this.ExecuteNestedGroup((string) command.Key);
    if (this.m_nestedEnums != null)
    {
      this.m_nestedEnums.Clear();
      this.m_nestedEnums = (Dictionary<string, IRowsEnumerator>) null;
    }
    if (this.m_dataSetDocIO != null)
    {
      this.m_dataSetDocIO.Clear();
      this.m_dataSetDocIO = (MailMergeDataSet) null;
    }
    if (this.m_commandsDocIO != null)
    {
      this.m_commandsDocIO.Clear();
      this.m_commandsDocIO = (List<DictionaryEntry>) null;
    }
    this.Document.IsMailMerge = false;
    this.IsNested = false;
  }

  public string[] GetMergeFieldNames()
  {
    List<string> fieldsArray = new List<string>();
    this.GetMergeFieldNamesImpl(fieldsArray, (string) null);
    return fieldsArray.ToArray();
  }

  public string[] GetMergeFieldNames(string groupName)
  {
    List<string> fieldsArray = new List<string>();
    this.GetMergeFieldNamesImpl(fieldsArray, groupName);
    return fieldsArray.ToArray();
  }

  public string[] GetMergeGroupNames()
  {
    List<string> stringList1 = (List<string>) null;
    List<string> stringList2 = (List<string>) null;
    List<string> stringList3 = new List<string>();
    Stack<EntityEntry> entityEntryStack = new Stack<EntityEntry>();
    entityEntryStack.Push(new EntityEntry((Entity) this.Document));
    do
    {
      EntityEntry entityEntry = entityEntryStack.Peek();
      if (entityEntry.Current != null && entityEntry.Current.IsComposite)
      {
        ICompositeEntity current = entityEntry.Current as ICompositeEntity;
        if (current.ChildEntities.Count > 0)
        {
          entityEntryStack.Push(new EntityEntry(current.ChildEntities[0]));
          goto label_16;
        }
      }
      if (entityEntry.Current != null && entityEntry.Current.EntityType == EntityType.MergeField)
      {
        WMergeField current = entityEntry.Current as WMergeField;
        if (MailMerge.IsBeginGroup(current))
        {
          if (stringList1 == null)
            stringList1 = new List<string>();
          stringList1.Add(current.FieldName);
        }
        else if (MailMerge.IsEndGroup(current))
        {
          if (stringList2 == null)
            stringList2 = new List<string>();
          stringList2.Add(current.FieldName);
        }
      }
      for (; !entityEntry.Fetch(); entityEntry = entityEntryStack.Peek())
      {
        entityEntryStack.Pop();
        if (entityEntryStack.Count == 0)
          break;
      }
label_16:;
    }
    while (entityEntryStack.Count > 0);
    if (stringList1 != null && stringList2 != null)
    {
      foreach (string str1 in stringList1)
      {
        foreach (string str2 in stringList2)
        {
          if (str1 == str2)
          {
            stringList3.Add(str1);
            stringList2.Remove(str2);
            break;
          }
        }
      }
    }
    return stringList3.ToArray();
  }

  private void GetMergeFieldNamesImpl(List<string> fieldsArray, string groupName)
  {
    int index1 = 0;
    for (int count1 = this.Document.Sections.Count; index1 < count1; ++index1)
    {
      WSection section = this.Document.Sections[index1];
      int index2 = 0;
      for (int count2 = section.Body.Items.Count; index2 < count2; ++index2)
      {
        TextBodyItem paragraph = section.Body.Items[index2];
        this.GetFieldNamesForParagraph(fieldsArray, paragraph, groupName);
      }
      for (int index3 = 0; index3 < 6; ++index3)
      {
        int index4 = 0;
        for (int count3 = section.HeadersFooters[index3].Items.Count; index4 < count3; ++index4)
        {
          TextBodyItem paragraph = section.HeadersFooters[index3].Items[index4];
          this.GetFieldNamesForParagraph(fieldsArray, paragraph, groupName);
        }
      }
    }
  }

  internal void Close()
  {
    this.m_doc = (WordDocument) null;
    if (this.m_contentSections != null)
    {
      this.m_contentSections.Close();
      this.m_contentSections = (WSectionCollection) null;
    }
    if (this.m_names != null)
      this.m_names = (string[]) null;
    if (this.m_values != null)
      this.m_values = (string[]) null;
    if (this.m_conn != null)
      this.m_conn = (DbConnection) null;
    if (this.m_curDataSet != null)
      this.m_curDataSet = (DataSet) null;
    if (this.m_commands != null)
      this.m_commands = (ArrayList) null;
    if (this.m_dataSet != null)
      this.m_dataSet = (DataSet) null;
    if (this.m_nestedEnums != null)
    {
      this.m_nestedEnums.Clear();
      this.m_nestedEnums = (Dictionary<string, IRowsEnumerator>) null;
    }
    if (this.m_varCmdRegex != null)
      this.m_varCmdRegex = (Regex) null;
    if (this.m_groupSelectors != null)
    {
      this.m_groupSelectors.Clear();
      this.m_groupSelectors = (Stack<MailMerge.GroupSelector>) null;
    }
    if (this.m_mappedFields != null)
    {
      this.m_mappedFields.Clear();
      this.m_mappedFields = (Dictionary<string, string>) null;
    }
    if (this.m_dataSetDocIO != null)
      this.m_dataSetDocIO = (MailMergeDataSet) null;
    if (this.m_commandsDocIO != null)
      this.m_commandsDocIO = (List<DictionaryEntry>) null;
    if (this.m_curDataSetDocIO != null)
      this.m_curDataSetDocIO = (MailMergeDataSet) null;
    if (this.m_clearFieldsState != null)
    {
      this.m_clearFieldsState.Clear();
      this.m_clearFieldsState = (Dictionary<string, bool>) null;
    }
    if (this.m_settings == null)
      return;
    this.m_settings.Close();
    this.m_settings = (MailMergeSettings) null;
  }

  private void OnGroupFound(IRowsEnumerator rowsEnum)
  {
    bool flag = false;
    MailMerge.GroupSelector groupSelector = this.m_groupSelector;
    if (this.CheckRecordsCount(rowsEnum) && this.RemoveEmptyGroup)
    {
      flag = true;
      this.EmptyGroup(groupSelector);
    }
    else if (this.IsNested)
    {
      this.m_groupSelector.BeginGroupField.FieldName = string.Empty;
      this.m_groupSelector.EndGroupField.FieldName = string.Empty;
    }
    if (!flag && !this.CheckSelection(rowsEnum) || this.CheckRecordsCount(rowsEnum) && this.RemoveEmptyGroup)
      return;
    if (groupSelector.GroupSelection != null)
    {
      this.OnBodyGroupFound(rowsEnum);
    }
    else
    {
      if (groupSelector.RowSelection == null)
        return;
      this.OnRowGroupFound(rowsEnum);
    }
  }

  private bool CheckRecordsCount(IRowsEnumerator rowsEnum)
  {
    if (!(rowsEnum is DataTableEnumerator) || (rowsEnum as DataTableEnumerator).m_MMtable == null)
      return rowsEnum.RowsCount == 0;
    if (string.IsNullOrEmpty((rowsEnum as DataTableEnumerator).Command) && (rowsEnum as DataTableEnumerator).RowsCount > 0)
      return false;
    return string.IsNullOrEmpty((rowsEnum as DataTableEnumerator).Command) || (rowsEnum as DataTableEnumerator).MatchingRecordsCount <= 0;
  }

  private void EmptyGroup(MailMerge.GroupSelector gs)
  {
    if (gs.BeginGroupField.OwnerParagraph.IsInCell || gs.EndGroupField.OwnerParagraph.IsInCell)
    {
      if (gs.BeginGroupField.Prefix == "TableStart")
        this.EmptyGroupInTable(gs);
      else
        this.EmptyGroupInTableCell(gs);
    }
    else
      this.EmptyGroupInTextbody(gs);
    if (gs.GroupSelection != null)
      gs.GroupSelection.ItemEndIndex = gs.GroupSelection.ItemStartIndex;
    if (gs.RowSelection == null)
      return;
    gs.RowSelection.EndRowIndex = gs.RowSelection.StartRowIndex;
  }

  private void EmptyGroupInTextbody(MailMerge.GroupSelector gs)
  {
    if (!(gs.BeginGroupField.Owner is WParagraph) || gs.BeginGroupField.OwnerParagraph.OwnerTextBody == null || gs.EndGroupField.FieldEnd == null || !(gs.EndGroupField.FieldEnd.Owner is WParagraph) || gs.EndGroupField.FieldEnd.OwnerParagraph.OwnerTextBody == null)
      return;
    int inOwnerCollection1 = gs.BeginGroupField.GetIndexInOwnerCollection();
    WParagraph ownerParagraph1 = gs.BeginGroupField.OwnerParagraph;
    BookmarkStart bookmarkStart = new BookmarkStart((IWordDocument) this.m_doc, "_fieldBookmark");
    BookmarkEnd bookmarkEnd = new BookmarkEnd((IWordDocument) this.m_doc, "_fieldBookmark");
    ownerParagraph1.Items.Insert(inOwnerCollection1, (IEntity) bookmarkStart);
    gs.BeginGroupField.EnsureBookmarkStart(bookmarkStart);
    WParagraph ownerParagraph2 = gs.EndGroupField.FieldEnd.OwnerParagraph;
    int inOwnerCollection2 = gs.EndGroupField.FieldEnd.GetIndexInOwnerCollection();
    ownerParagraph2.Items.Insert(inOwnerCollection2 + 1, (IEntity) bookmarkEnd);
    gs.EndGroupField.EnsureBookmarkStart(bookmarkEnd);
    BookmarksNavigator bookmarksNavigator = new BookmarksNavigator((IWordDocument) this.m_doc);
    bookmarksNavigator.MoveToBookmark("_fieldBookmark");
    bookmarksNavigator.RemoveEmptyParagraph = false;
    this.Document.IsSkipFieldDetach = true;
    bookmarksNavigator.DeleteBookmarkContent(false);
    this.Document.IsSkipFieldDetach = false;
    if (ownerParagraph1.Items.Contains((IEntity) bookmarkStart))
      ownerParagraph1.Items.Remove((IEntity) bookmarkStart);
    if (ownerParagraph2.Items.Contains((IEntity) bookmarkEnd))
      ownerParagraph2.Items.Remove((IEntity) bookmarkEnd);
    if (ownerParagraph1.ChildEntities.Count == 0 && ownerParagraph1.Equals((object) ownerParagraph2))
    {
      ownerParagraph1.OwnerTextBody.ChildEntities.Remove((IEntity) ownerParagraph1);
    }
    else
    {
      if (ownerParagraph1.ChildEntities.Count == 0)
        ownerParagraph1.OwnerTextBody.ChildEntities.Remove((IEntity) ownerParagraph1);
      if (ownerParagraph2.ChildEntities.Count != 0)
        return;
      ownerParagraph2.OwnerTextBody.ChildEntities.Remove((IEntity) ownerParagraph2);
    }
  }

  private void EmptyGroupInTable(MailMerge.GroupSelector gs)
  {
    if (!(gs.BeginGroupField.Owner is WParagraph) || gs.BeginGroupField.OwnerParagraph.OwnerTextBody == null || gs.EndGroupField.FieldEnd == null || !(gs.EndGroupField.FieldEnd.Owner is WParagraph) || gs.EndGroupField.FieldEnd.OwnerParagraph.OwnerTextBody == null)
      return;
    this.RemoveGoBackBookmark(gs);
    int inOwnerCollection1 = gs.BeginGroupField.GetIndexInOwnerCollection();
    int inOwnerCollection2 = gs.EndGroupField.FieldEnd.GetIndexInOwnerCollection();
    WTableCell ownerEntity1 = (gs.BeginGroupField.Owner as WParagraph).GetOwnerEntity() as WTableCell;
    WTableCell ownerEntity2 = (gs.EndGroupField.FieldEnd.Owner as WParagraph).GetOwnerEntity() as WTableCell;
    int inOwnerCollection3 = gs.BeginGroupField.OwnerParagraph.GetIndexInOwnerCollection();
    int cellIndex1 = ownerEntity1.GetCellIndex();
    int cellIndex2 = ownerEntity2.GetCellIndex();
    int rowIndex1 = (ownerEntity1.Owner as WTableRow).GetRowIndex();
    int rowIndex2 = (ownerEntity2.Owner as WTableRow).GetRowIndex();
    int inOwnerCollection4 = gs.EndGroupField.FieldEnd.OwnerParagraph.GetIndexInOwnerCollection();
    bool flag = rowIndex1 == rowIndex2;
    WTable owner = gs.BeginGroupField.Owner.Owner.Owner.Owner as WTable;
    this.Document.IsSkipFieldDetach = true;
    this.RemoveItemsAfterTableStart(gs, owner, inOwnerCollection1, inOwnerCollection3, cellIndex1, ref cellIndex2, rowIndex1, ref rowIndex2);
    if (!flag)
      this.RemoveItemsAtTableEnd(gs, owner, inOwnerCollection2, inOwnerCollection4, cellIndex1, cellIndex2, rowIndex1, rowIndex2);
    else if (inOwnerCollection1 == 0 && inOwnerCollection3 == 0)
    {
      if (inOwnerCollection4 == owner.Rows[rowIndex1].Cells[cellIndex1].Items.Count - 1 && inOwnerCollection2 == (owner.Rows[rowIndex1].Cells[cellIndex1].Items[inOwnerCollection4] as WParagraph).Items.Count - 1)
      {
        owner.Rows.RemoveAt(rowIndex1);
      }
      else
      {
        this.RemoveItems((Entity) owner.Rows[rowIndex1].Cells[cellIndex1].Items[inOwnerCollection4], 0, inOwnerCollection2);
        this.RemoveItems((Entity) owner.Rows[rowIndex1].Cells[cellIndex1], 0, inOwnerCollection4);
      }
    }
    this.Document.IsSkipFieldDetach = false;
  }

  private void RemoveGoBackBookmark(MailMerge.GroupSelector gs)
  {
    for (int index = 0; index < gs.BeginGroupField.OwnerParagraph.Items.Count; ++index)
    {
      ParagraphItem paragraphItem = gs.BeginGroupField.OwnerParagraph.Items[index];
      if (paragraphItem is BookmarkStart && (paragraphItem as BookmarkStart).Name.ToLower() == "_goback" || paragraphItem is BookmarkEnd && (paragraphItem as BookmarkEnd).Name.ToLower() == "_goback")
      {
        paragraphItem.RemoveSelf();
        --index;
      }
    }
    for (int index = 0; index < gs.EndGroupField.OwnerParagraph.Items.Count; ++index)
    {
      ParagraphItem paragraphItem = gs.EndGroupField.OwnerParagraph.Items[index];
      if (paragraphItem is BookmarkStart && (paragraphItem as BookmarkStart).Name.ToLower() == "_goback" || paragraphItem is BookmarkEnd && (paragraphItem as BookmarkEnd).Name.ToLower() == "_goback")
      {
        paragraphItem.RemoveSelf();
        --index;
      }
    }
  }

  private void RemoveItemsAfterTableStart(
    MailMerge.GroupSelector gs,
    WTable table,
    int startIndex,
    int startParaIndex,
    int startCellIndex,
    ref int endCellIndex,
    int startRowIndex,
    ref int endRowIndex)
  {
    if (startIndex == 0)
    {
      if (startParaIndex == 0)
      {
        if (startCellIndex == 0)
        {
          if (startRowIndex != endRowIndex)
          {
            this.RemoveItems((Entity) table, startRowIndex, endRowIndex);
            endRowIndex = startRowIndex;
          }
          else
          {
            if (startCellIndex == endCellIndex)
              return;
            this.RemoveItems((Entity) table.Rows[startRowIndex], startCellIndex, endCellIndex);
            endCellIndex = startCellIndex;
          }
        }
        else if (startRowIndex != endRowIndex)
        {
          if (startCellIndex == table.Rows[startRowIndex].Cells.Count - 1)
            table.Rows[startRowIndex].Cells.RemoveAt(startCellIndex);
          else
            this.RemoveItems((Entity) table.Rows[startRowIndex], startCellIndex, table.Rows[startRowIndex].Cells.Count - 1);
          if (startRowIndex + 1 >= endRowIndex)
            return;
          this.RemoveItems((Entity) table, startRowIndex + 1, endRowIndex);
          endRowIndex = startRowIndex + 1;
        }
        else if (startCellIndex == endCellIndex)
        {
          table.Rows[startRowIndex].Cells.RemoveAt(startCellIndex);
        }
        else
        {
          this.RemoveItems((Entity) table.Rows[startRowIndex], startCellIndex, endCellIndex);
          endCellIndex = startCellIndex;
        }
      }
      else
        this.RemoveItems((Entity) table.Rows[startRowIndex].Cells[startCellIndex], startParaIndex, table.Rows[startRowIndex].Cells[startCellIndex].Items.Count);
    }
    else
    {
      this.RemoveItems((Entity) (table.Rows[startRowIndex].Cells[startCellIndex].Items[startParaIndex] as WParagraph), startIndex, (table.Rows[startRowIndex].Cells[startCellIndex].Items[startParaIndex] as WParagraph).Items.Count);
      this.RemoveItems((Entity) table.Rows[startRowIndex].Cells[startCellIndex], startParaIndex + 1, table.Rows[startRowIndex].Cells[startCellIndex].Items.Count);
      if (startRowIndex == endRowIndex)
      {
        this.RemoveItems((Entity) table.Rows[startRowIndex], startCellIndex + 1, endCellIndex);
        endCellIndex = startCellIndex + 1;
      }
      else
        this.RemoveItems((Entity) table.Rows[startRowIndex], startCellIndex + 1, table.Rows[startRowIndex].Cells.Count);
      this.RemoveItems((Entity) table, startRowIndex + 1, endRowIndex);
      endRowIndex = startRowIndex + 1;
    }
  }

  private void RemoveItemsAtTableEnd(
    MailMerge.GroupSelector gs,
    WTable table,
    int endIndex,
    int endParaIndex,
    int startCellIndex,
    int endCellIndex,
    int startRowIndex,
    int endRowIndex)
  {
    if (startRowIndex != endRowIndex && endIndex == (table.Rows[startRowIndex + 1].Cells[endCellIndex].Items[endParaIndex] as WParagraph).Items.Count - 1)
    {
      if (endParaIndex == table.Rows[startRowIndex + 1].Cells[endCellIndex].Items.Count - 1)
      {
        if (endCellIndex == table.Rows[startRowIndex + 1].Cells.Count - 1)
          table.Rows.RemoveAt(startRowIndex + 1);
        else
          this.RemoveItems((Entity) table.Rows[startRowIndex + 1], 0, endCellIndex + 1);
      }
      else
      {
        this.RemoveItems((Entity) table.Rows[startRowIndex + 1].Cells[endCellIndex], 0, endParaIndex + 1);
        this.RemoveItems((Entity) table.Rows[startRowIndex + 1], 0, endCellIndex);
      }
    }
    else
    {
      this.RemoveItems((Entity) (table.Rows[endRowIndex].Cells[endCellIndex].Items[endParaIndex] as WParagraph), 0, endIndex + 1);
      this.RemoveItems((Entity) table.Rows[endRowIndex].Cells[endCellIndex], 0, endParaIndex);
      this.RemoveItems((Entity) table.Rows[endRowIndex], 0, endCellIndex);
    }
  }

  private void EmptyGroupInTableCell(MailMerge.GroupSelector gs)
  {
    if (!(gs.BeginGroupField.Owner is WParagraph) || gs.BeginGroupField.OwnerParagraph.OwnerTextBody == null || gs.EndGroupField.FieldEnd == null || !(gs.EndGroupField.FieldEnd.Owner is WParagraph) || gs.EndGroupField.FieldEnd.OwnerParagraph.OwnerTextBody == null)
      return;
    int inOwnerCollection1 = gs.BeginGroupField.GetIndexInOwnerCollection();
    int inOwnerCollection2 = gs.BeginGroupField.OwnerParagraph.GetIndexInOwnerCollection();
    int inOwnerCollection3 = gs.EndGroupField.FieldEnd.GetIndexInOwnerCollection();
    int inOwnerCollection4 = gs.EndGroupField.FieldEnd.OwnerParagraph.GetIndexInOwnerCollection();
    WTableCell ownerEntity = gs.BeginGroupField.OwnerParagraph.GetOwnerEntity() as WTableCell;
    this.Document.IsSkipFieldDetach = true;
    if ((ownerEntity.Items[inOwnerCollection2] as WParagraph).Items.Count > 1 && inOwnerCollection1 > 0)
    {
      this.RemoveItems((Entity) (ownerEntity.Items[inOwnerCollection2] as WParagraph), inOwnerCollection1, (ownerEntity.Items[inOwnerCollection2] as WParagraph).Items.Count);
      this.RemoveItems((Entity) ownerEntity, inOwnerCollection2 + 1, inOwnerCollection4);
      if (inOwnerCollection3 == (ownerEntity.Items[inOwnerCollection2 + 1] as WParagraph).Items.Count - 1)
        ownerEntity.Items.RemoveAt(inOwnerCollection2 + 1);
      else if ((ownerEntity.Items[inOwnerCollection2 + 1] as WParagraph).Items.Count > 0)
        this.RemoveItems((Entity) (ownerEntity.Items[inOwnerCollection2 + 1] as WParagraph), 0, inOwnerCollection3);
    }
    else
    {
      this.RemoveItems((Entity) ownerEntity, inOwnerCollection2, inOwnerCollection4);
      int inOwnerCollection5 = gs.EndGroupField.FieldEnd.OwnerParagraph.GetIndexInOwnerCollection();
      if (inOwnerCollection2 != inOwnerCollection5 && ownerEntity.Items.Count > inOwnerCollection2 + 1)
      {
        if (inOwnerCollection3 == (ownerEntity.Items[inOwnerCollection2 + 1] as WParagraph).Items.Count - 1)
          ownerEntity.Items.RemoveAt(inOwnerCollection2 + 1);
        else if ((ownerEntity.Items[inOwnerCollection2 + 1] as WParagraph).Items.Count > 0)
          this.RemoveItems((Entity) (ownerEntity.Items[inOwnerCollection2 + 1] as WParagraph), 0, (ownerEntity.Items[inOwnerCollection2 + 1] as WParagraph).Items.Count + 1);
      }
      else if (inOwnerCollection3 == (ownerEntity.Items[inOwnerCollection2] as WParagraph).Items.Count - 1)
        ownerEntity.Items.RemoveAt(inOwnerCollection2);
      else if ((ownerEntity.Items[inOwnerCollection2] as WParagraph).Items.Count > 0)
        this.RemoveItems((Entity) (ownerEntity.Items[inOwnerCollection2] as WParagraph), 0, inOwnerCollection3 + 1);
    }
    this.Document.IsSkipFieldDetach = false;
  }

  private void RemoveItems(Entity ent, int startIndex, int endIndex)
  {
    switch (ent.EntityType)
    {
      case EntityType.TextBody:
        for (int index = startIndex; index < endIndex; ++index)
          (ent as WTextBody).Items.RemoveAt(startIndex);
        break;
      case EntityType.Paragraph:
        for (int index = startIndex; index < endIndex; ++index)
          (ent as WParagraph).Items.RemoveAt(startIndex);
        break;
      case EntityType.Table:
        for (int index = startIndex; index < endIndex; ++index)
          (ent as WTable).Rows.RemoveAt(startIndex);
        break;
      case EntityType.TableRow:
        for (int index = startIndex; index < endIndex; ++index)
          (ent as WTableRow).Cells.RemoveAt(startIndex);
        break;
      case EntityType.TableCell:
        for (int index = startIndex; index < endIndex; ++index)
          (ent as WTableCell).Items.RemoveAt(startIndex);
        break;
    }
  }

  private string[] GetTableCommand(string command) => command?.Split(' ');

  private void OnBodyGroupFound(IRowsEnumerator rowsEnum)
  {
    MailMerge.GroupSelector groupSelector = this.m_groupSelector;
    TextBodyPart txtBodyPart = new TextBodyPart();
    TextBodySelection groupSelection = groupSelector.GroupSelection;
    txtBodyPart.Copy(groupSelection);
    rowsEnum.Reset();
    int num = 0;
    this.RemoveBookMarks(txtBodyPart);
    while (this.CheckNextRow(rowsEnum))
    {
      if (this.m_conn != null || this.m_dataSet != null || this.m_dataSetDocIO != null)
        this.UpdateEnum(groupSelector.GroupName, rowsEnum);
      int count1 = groupSelection.TextBody.Items.Count;
      WParagraph wparagraph1 = groupSelection.TextBody.Items[groupSelection.ItemEndIndex] as WParagraph;
      int count2 = groupSelection.TextBody.Items.Count - 1 < groupSelection.ItemEndIndex || wparagraph1 == null ? 0 : wparagraph1.Items.Count;
      this.ExecuteGroupForSelection(groupSelection.TextBody, groupSelection.ItemStartIndex, groupSelection.ItemEndIndex, groupSelection.ParagraphItemStartIndex, groupSelection.ParagraphItemEndIndex, rowsEnum);
      ++this.m_mergedRecordCount;
      ++num;
      groupSelection.ItemEndIndex += groupSelection.TextBody.Items.Count - count1;
      if (rowsEnum.IsLast || (rowsEnum is DataTableEnumerator ? (num == (rowsEnum as DataTableEnumerator).MatchingRecordsCount ? 1 : 0) : 0) != 0)
      {
        if (!this.IsNested)
          break;
        this.NestedEnums.Remove(groupSelector.GroupName);
        break;
      }
      if ((groupSelection.TextBody.Items.Count > groupSelection.ItemEndIndex ? groupSelection.TextBody.Items[groupSelection.ItemEndIndex] : (TextBodyItem) null) is WParagraph wparagraph2)
        groupSelection.ParagraphItemEndIndex = wparagraph2.Items.Count <= 0 ? wparagraph2.Items.Count : groupSelection.ParagraphItemEndIndex + 1 - (count2 - wparagraph2.Items.Count);
      if (groupSelection.ItemStartIndex == groupSelection.ItemEndIndex)
      {
        txtBodyPart.PasteAt((ITextBody) groupSelection.TextBody, groupSelection.ItemEndIndex, groupSelection.ParagraphItemEndIndex);
        groupSelection.ParagraphItemStartIndex = groupSelection.ParagraphItemEndIndex;
        for (int index = wparagraph2.Items.Count - 1; index >= 0; --index)
        {
          if (wparagraph2.Items[index] is WFieldMark)
          {
            groupSelection.ParagraphItemEndIndex = index;
            break;
          }
        }
      }
      else
      {
        txtBodyPart.PasteAt((ITextBody) groupSelection.TextBody, groupSelection.ItemEndIndex, groupSelection.ParagraphItemEndIndex);
        groupSelection.ItemStartIndex = groupSelection.ItemEndIndex;
        groupSelection.ItemEndIndex += txtBodyPart.BodyItems.Count - 1;
        groupSelection.ParagraphItemStartIndex = groupSelection.ParagraphItemEndIndex;
        WParagraph bodyItem = txtBodyPart.BodyItems[txtBodyPart.BodyItems.Count - 1] is WParagraph ? txtBodyPart.BodyItems[txtBodyPart.BodyItems.Count - 1] as WParagraph : (WParagraph) null;
        if (bodyItem != null)
          groupSelection.ParagraphItemEndIndex = bodyItem.Items.Count - 1;
      }
      this.Document.Settings.DuplicateListStyleNames = string.Empty;
    }
  }

  private void RemoveBookMarks(TextBodyPart txtBodyPart)
  {
    for (int index = 0; index < txtBodyPart.BodyItems.Count; ++index)
      this.DeleteBoookmarks((IEntity) txtBodyPart.BodyItems[index]);
  }

  private void DeleteBoookmarks(IEntity entity)
  {
    if (entity.IsComposite)
    {
      for (int index = (entity as ICompositeEntity).ChildEntities.Count - 1; index >= 0; --index)
        this.DeleteBoookmarks((IEntity) (entity as ICompositeEntity).ChildEntities[index]);
    }
    else
    {
      if (entity.EntityType != EntityType.BookmarkStart && entity.EntityType != EntityType.BookmarkEnd)
        return;
      (entity.Owner as WParagraph).Items.Remove(entity);
    }
  }

  private void OnRowGroupFound(IRowsEnumerator rowsEnum)
  {
    MailMerge.GroupSelector groupSelector = this.m_groupSelector;
    WTable table = groupSelector.RowSelection.Table;
    int startRowIndex1 = groupSelector.RowSelection.StartRowIndex;
    int endRowIndex = groupSelector.RowSelection.EndRowIndex;
    int count1 = table.Rows.Count;
    int startRowIndex2 = startRowIndex1;
    if (this.IsNested)
      this.VerifyNestedGroups(startRowIndex1, endRowIndex, table);
    int count2 = endRowIndex - startRowIndex1 + 1;
    WTableRow[] wtableRowArray = new WTableRow[count2];
    int index1 = 0;
    for (int index2 = startRowIndex1; index2 <= endRowIndex; ++index2)
    {
      wtableRowArray[index1] = table.Rows[index2].Clone();
      ++index1;
    }
    rowsEnum.Reset();
    int num1 = 0;
    while (this.CheckNextRow(rowsEnum))
    {
      if (this.m_conn != null || this.m_dataSet != null || this.m_dataSetDocIO != null)
        this.UpdateEnum(groupSelector.GroupName, rowsEnum);
      int num2 = this.ExecuteGroupForRowSelection(table, startRowIndex2, count2, rowsEnum);
      ++num1;
      ++this.m_mergedRecordCount;
      if (rowsEnum.IsLast || (rowsEnum is DataTableEnumerator ? (num1 == (rowsEnum as DataTableEnumerator).MatchingRecordsCount ? 1 : 0) : 0) != 0)
      {
        if (this.IsNested)
          this.NestedEnums.Remove(groupSelector.GroupName);
        startRowIndex2 += num2 - 1;
        break;
      }
      startRowIndex2 += num2;
      for (int index3 = 0; index3 < count2; ++index3)
        table.Rows.Insert(startRowIndex2 + index3, wtableRowArray[index3].Clone());
    }
    groupSelector.RowSelection.StartRowIndex = startRowIndex2;
  }

  private void ExecuteGroup(IRowsEnumerator rowsEnum)
  {
    this.Document.IsMailMerge = true;
    this.RemoveSpellChecking();
    int index = 0;
    for (int count = this.Document.Sections.Count; index < count; ++index)
      this.ExecuteGroup(this.Document.Sections[index], rowsEnum);
    this.Document.IsMailMerge = false;
  }

  private void ExecuteGroup(WSection section, IRowsEnumerator rowsEnum)
  {
    this.m_groupSelector.ProcessGroups(section.Body, rowsEnum);
    for (int index = 0; index < 6; ++index)
    {
      WTextBody headersFooter = (WTextBody) section.HeadersFooters[index];
      if (headersFooter.Items.Count > 0)
        this.m_groupSelector.ProcessGroups(headersFooter, rowsEnum);
    }
  }

  private void ExecuteGroupForSelection(
    WTextBody textBody,
    int itemStart,
    int itemEnd,
    int pItemStart,
    int pItemEnd,
    IRowsEnumerator rowsEnum)
  {
    if (itemEnd < 0)
      itemEnd = textBody.Items.Count - 1;
    int index1 = itemStart;
    for (int index2 = itemEnd; index1 <= index2 && index1 < textBody.Items.Count; ++index1)
    {
      TextBodyItem textBodyItem = textBody.Items[index1];
      switch (textBodyItem.EntityType)
      {
        case EntityType.Paragraph:
          WParagraph para = textBodyItem as WParagraph;
          int num1 = index1 == itemStart ? pItemStart : 0;
          int endIndex = index1 != index2 || pItemEnd <= -1 ? para.Items.Count - 1 : pItemEnd;
          int count1 = para.Items.Count;
          for (int index3 = num1; index3 <= endIndex && !para.DeepDetached; ++index3)
          {
            if (para.Items.Count > index3 && para.Items[index3].EntityType == EntityType.TextBox)
              this.ExecuteGroupForSelection((para.Items[index3] as WTextBox).TextBoxBody, 0, -1, 0, -1, rowsEnum);
            else if (para.Items.Count > index3 && para.Items[index3].EntityType == EntityType.AutoShape)
            {
              this.ExecuteGroupForSelection((para.Items[index3] as Shape).TextBody, 0, -1, 0, -1, rowsEnum);
            }
            else
            {
              WField field = para.Items.Count > index3 ? para.Items[index3] as WField : (WField) null;
              if (field != null)
              {
                if (field is WMergeField)
                {
                  (field as WMergeField).UpdateFieldMarks();
                  if (field.Owner is WParagraph && field.FieldEnd != null && field.FieldEnd.Owner is WParagraph)
                  {
                    WMergeField wmergeField = field as WMergeField;
                    if (!MailMerge.IsBeginGroup(wmergeField) && !MailMerge.IsEndGroup(wmergeField))
                    {
                      if (!wmergeField.Prefix.StartsWithExt("Image") ? this.UpdateMergeFieldValue(wmergeField, rowsEnum) : this.UpdateImageMergeFieldValue(wmergeField, rowsEnum))
                      {
                        --index3;
                        endIndex = this.UpdateEndIndex(ref count1, para, endIndex);
                        if (this.RemoveEmptyParagraphs && para.Items.Count == 0)
                        {
                          if (para.Owner is WTableCell owner && owner.ChildEntities.Count == 1)
                            para.ChildEntities.Clear();
                          else
                            para.RemoveEmpty = true;
                        }
                      }
                    }
                    else if (this.IsNested)
                    {
                      if (MailMerge.IsBeginGroup(wmergeField) && !this.NestedEnums.ContainsKey(wmergeField.FieldName) && wmergeField.FieldName != string.Empty)
                      {
                        string fieldName = wmergeField.FieldName;
                        IRowsEnumerator rowsEnum1 = this.GetEnum(fieldName);
                        if (rowsEnum1 == null)
                        {
                          if (this.RemoveField(field, false) != -1)
                            --index3;
                          endIndex = this.UpdateEndIndex(ref count1, para, endIndex);
                          if (this.RemoveEmptyParagraphs && para.Items.Count == 0)
                          {
                            if (para.Owner is WTableCell owner && owner.ChildEntities.Count == 1)
                              para.ChildEntities.Clear();
                            else
                              para.RemoveEmpty = true;
                          }
                          if (this.RemoveEmptyGroup && para.Items.Count == 0)
                            para.RemoveEmpty = true;
                        }
                        else
                        {
                          int count2 = textBody.Items.Count;
                          this.GroupSelectors.Push(this.m_groupSelector);
                          this.m_groupSelector = new MailMerge.GroupSelector(new MailMerge.GroupSelector.GroupFound(this.OnGroupFound), this.InsertAsNewRow);
                          this.m_groupSelector.ProcessGroupsInNested(textBody, rowsEnum1, itemStart, itemEnd);
                          int selectedBodyItemsCount = this.m_groupSelector.SelectedBodyItemsCount;
                          if ((wmergeField.Prefix == "TableStart" || wmergeField.Prefix == "BeginGroup") && selectedBodyItemsCount == -1)
                          {
                            Entity tableEntity = this.GetTableEntity((Entity) wmergeField);
                            if (!(tableEntity is WTable))
                              throw new ApplicationException($"Group \"{fieldName}\" is missing in the source document.");
                            this.m_groupSelector.ProcessGroups(tableEntity as WTable, wmergeField.OwnerParagraph.Owner.Owner.GetIndexInOwnerCollection(), (tableEntity as WTable).Rows.Count - 1, rowsEnum1);
                          }
                          else
                          {
                            if (selectedBodyItemsCount == -1)
                              throw new ApplicationException($"Group \"{fieldName}\" is missing in the source document.");
                            if (selectedBodyItemsCount > 0)
                            {
                              int num2 = textBody.Items.Count - count2;
                              index1 += num2 + selectedBodyItemsCount - 1;
                              index2 += num2;
                              itemEnd = index2;
                              if (-num2 != selectedBodyItemsCount)
                                --index1;
                            }
                            else if (wmergeField.Owner is WParagraph && this.HideField((WField) wmergeField, true))
                            {
                              int num3 = index3 - 1;
                              this.UpdateEndIndex(ref count1, para, endIndex);
                              if (this.RemoveEmptyParagraphs && para.Items.Count == 0)
                              {
                                if (para.Owner is WTableCell owner && owner.ChildEntities.Count == 1)
                                  para.ChildEntities.Clear();
                                else
                                  para.RemoveEmpty = true;
                              }
                            }
                          }
                          if (this.m_curDataSet != null)
                            this.CurrentDataSet.Tables.Remove(fieldName);
                          else if (this.m_curDataSetDocIO != null)
                            this.CurrentDataSetDocIO.RemoveDataTable(fieldName);
                          this.m_groupSelector = this.GroupSelectors.Pop();
                          break;
                        }
                      }
                      else if (MailMerge.IsBeginGroup(wmergeField) || MailMerge.IsEndGroup(wmergeField))
                      {
                        if (!MailMerge.IsEndGroup(wmergeField) || this.BeforeClearField == null && this.ClearFields || this.IsNeedToRemoveGroupEnd(wmergeField))
                        {
                          if (this.RemoveField(field, false) != -1)
                            --index3;
                          endIndex = this.UpdateEndIndex(ref count1, para, endIndex);
                          if (this.RemoveEmptyParagraphs && para.Items.Count == 0)
                          {
                            if (para.Owner is WTableCell owner && owner.ChildEntities.Count == 1)
                              para.ChildEntities.Clear();
                            else
                              para.RemoveEmpty = true;
                          }
                        }
                        else
                          break;
                      }
                    }
                    else if (MailMerge.IsBeginGroup(wmergeField) || MailMerge.IsEndGroup(wmergeField))
                    {
                      if (!MailMerge.IsEndGroup(wmergeField) || this.BeforeClearField == null && this.ClearFields || this.IsNeedToRemoveGroupEnd(wmergeField))
                      {
                        if (this.RemoveField(field, false) != -1)
                          --index3;
                        endIndex = this.UpdateEndIndex(ref count1, para, endIndex);
                        if (this.RemoveEmptyParagraphs && para.Items.Count == 0)
                        {
                          if (para.Owner is WTableCell owner && owner.ChildEntities.Count == 1)
                            para.ChildEntities.Clear();
                          else
                            para.RemoveEmpty = true;
                        }
                      }
                      else
                        break;
                    }
                  }
                }
                else if (field is WIfField)
                  this.UpdateIfFieldValue(field as WIfField, rowsEnum);
                else if (field.FieldType == FieldType.FieldNext)
                {
                  if (rowsEnum != null && !rowsEnum.IsEnd)
                    this.CheckNextRow(rowsEnum);
                  if (this.RemoveField(field, false) != -1)
                    --index3;
                  endIndex = this.UpdateEndIndex(ref count1, para, endIndex);
                  if (this.RemoveEmptyParagraphs && para.Items.Count == 0)
                  {
                    if (para.Owner is WTableCell owner && owner.ChildEntities.Count == 1)
                      para.ChildEntities.Clear();
                    else
                      para.RemoveEmpty = true;
                  }
                }
                else if (field.FieldType == FieldType.FieldNextIf)
                {
                  if (field.UpdateNextIfField() && rowsEnum != null && !rowsEnum.IsEnd)
                    this.CheckNextRow(rowsEnum);
                  if (this.RemoveField(field, false) != -1)
                    --index3;
                  endIndex = para.Items.Count - 1;
                  if (this.RemoveEmptyParagraphs && para.Items.Count == 0)
                  {
                    if (para.Owner is WTableCell owner && owner.ChildEntities.Count == 1)
                      para.ChildEntities.Clear();
                    else
                      para.RemoveEmpty = true;
                  }
                }
                else if (field.FieldType == FieldType.FieldMergeRec || field.FieldType == FieldType.FieldMergeSeq)
                {
                  int num4 = 1;
                  if (rowsEnum != null)
                    num4 += rowsEnum.CurrentRowIndex;
                  if (!field.OwnerParagraph.IsInCell || this.ClearFieldsState.Count <= 0 || this.ClearFieldsState.ContainsKey(rowsEnum.TableName))
                    this.ConvertToText(field, num4.ToString());
                }
              }
            }
          }
          break;
        case EntityType.BlockContentControl:
          WTextBody textBody1 = ((BlockContentControl) textBodyItem).TextBody;
          this.ExecuteGroupForSelection(textBody1, textBody1.Items.FirstItem.Index, textBody1.Items.LastItem.Index, 0, -1, rowsEnum);
          break;
        case EntityType.Table:
          WTable table = textBodyItem as WTable;
          for (int startRowIndex = 0; startRowIndex < table.Rows.Count; ++startRowIndex)
            this.ExecuteGroupForRowSelection(table, startRowIndex, 1, rowsEnum);
          break;
      }
    }
  }

  private bool IsNeedToRemoveGroupEnd(WMergeField mergeField)
  {
    int count = this.Document.Fields.Count;
    for (int index = 0; index < count; ++index)
    {
      if (this.Document.Fields[index] is WMergeField field)
      {
        List<string> stringList = new List<string>((IEnumerable<string>) field.FieldCode.Split(':', ' '));
        int num = stringList.IndexOf(field.Prefix);
        string empty = string.Empty;
        if (!string.IsNullOrEmpty(field.Prefix))
          empty = stringList[num + 1];
        if (MailMerge.IsBeginGroup(field) && mergeField.FieldCode.Contains(empty) && this.m_groupSelector.GroupSelection != null && this.m_groupSelector.GroupSelection.ItemStartIndex <= field.OwnerParagraph.Index && this.m_groupSelector.GroupSelection.ItemEndIndex >= field.OwnerParagraph.Index && empty != string.Empty && field.OwnerParagraph.ChildEntities.Count != 0 && mergeField.GetOwnerTextBody((Entity) mergeField) == field.GetOwnerTextBody((Entity) field))
          return false;
      }
    }
    return true;
  }

  private int UpdateEndIndex(ref int count, WParagraph para, int endIndex)
  {
    int num = count - para.Items.Count;
    count = para.Items.Count;
    return endIndex - num;
  }

  private Entity GetTableEntity(Entity entity)
  {
    Entity tableEntity = entity;
    while (tableEntity.Owner != null)
    {
      tableEntity = tableEntity.Owner;
      if (tableEntity is WTable)
        return tableEntity;
    }
    return tableEntity;
  }

  private int ExecuteGroupForRowSelection(
    WTable table,
    int startRowIndex,
    int count,
    IRowsEnumerator rowsEnum)
  {
    int count1 = table.Rows.Count;
    int num1 = startRowIndex + count - 1;
    int count2 = table.Rows.Count;
    for (int index1 = startRowIndex; index1 <= (table.Rows.Count > num1 ? num1 : table.Rows.Count - 1); ++index1)
    {
      WTableRow row = table.Rows[index1];
      int count3 = row.Cells.Count;
      for (int index2 = 0; index2 < row.Cells.Count; ++index2)
      {
        WTableCell cell = row.Cells[index2];
        int count4 = table.Rows.Count;
        this.ExecuteGroupForSelection((WTextBody) cell, 0, -1, 0, -1, rowsEnum);
        if (count4 < table.Rows.Count)
          num1 += table.Rows.Count - count4;
        if (count3 > row.Cells.Count)
        {
          --index2;
          count3 = row.Cells.Count;
        }
      }
      if (count2 > table.Rows.Count)
      {
        --index1;
        count2 = table.Rows.Count;
      }
    }
    int endRow = table.Rows.Count > num1 ? num1 : table.Rows.Count - 1;
    if (this.IsNested)
    {
      string nestedGroup = this.FindNestedGroup(startRowIndex, endRow, table);
      int count5 = table.Rows.Count;
      if (nestedGroup != null && this.ClearFieldsState.ContainsKey(nestedGroup))
      {
        this.ClearFields = this.ClearFieldsState[nestedGroup];
        this.ClearFieldsState.Remove(nestedGroup);
      }
      while (nestedGroup != null)
      {
        int num2 = table.Rows.Count - count5;
        endRow += num2;
        startRowIndex += num2;
        count5 = table.Rows.Count;
        IRowsEnumerator rowsEnum1 = this.GetEnum(nestedGroup);
        if (rowsEnum1 != null)
        {
          this.GroupSelectors.Push(this.m_groupSelector);
          this.m_groupSelector = new MailMerge.GroupSelector(new MailMerge.GroupSelector.GroupFound(this.OnGroupFound), this.InsertAsNewRow);
          this.m_groupSelector.ProcessGroups(table, startRowIndex, endRow, rowsEnum1);
          if (this.m_curDataSet != null)
            this.CurrentDataSet.Tables.Remove(nestedGroup);
          else if (this.m_curDataSetDocIO != null)
            this.CurrentDataSetDocIO.RemoveDataTable(nestedGroup);
          this.m_groupSelector = this.GroupSelectors.Pop();
        }
        string str = nestedGroup;
        nestedGroup = this.FindNestedGroup(startRowIndex, endRow, table);
        if (str == nestedGroup)
          break;
      }
    }
    int num3 = table.Rows.Count - count1;
    return count + num3;
  }

  private void ExecuteNestedGroup(string tableName)
  {
    IRowsEnumerator rowsEnum = this.GetEnum(tableName);
    if (rowsEnum == null)
      return;
    int index = 0;
    for (int count = this.Document.Sections.Count; index < count; ++index)
      this.ExecuteGroup(this.Document.Sections[index], rowsEnum);
    if (this.m_curDataSet != null)
    {
      this.CurrentDataSet.Tables.Remove(tableName);
    }
    else
    {
      if (this.m_curDataSetDocIO == null)
        return;
      this.CurrentDataSetDocIO.RemoveDataTable(tableName);
    }
  }

  private IRowsEnumerator GetEnum(string tableName)
  {
    IRowsEnumerator rowsEnumerator = (IRowsEnumerator) null;
    object dataTable = this.GetDataTable(tableName);
    switch (dataTable)
    {
      case DataTable _:
        DataTable table = dataTable as DataTable;
        this.CurrentDataSet.Tables.Add(table);
        rowsEnumerator = (IRowsEnumerator) new DataTableEnumerator(table);
        rowsEnumerator.Reset();
        break;
      case MailMergeDataTable _:
        MailMergeDataTable mailMergeDataTable = dataTable as MailMergeDataTable;
        this.CurrentDataSetDocIO.Add((object) mailMergeDataTable);
        rowsEnumerator = (IRowsEnumerator) new DataTableEnumerator(mailMergeDataTable);
        rowsEnumerator.Reset();
        break;
    }
    return rowsEnumerator;
  }

  private void UpdateEnum(string tableName, IRowsEnumerator rowsEnum)
  {
    if (!this.NestedEnums.ContainsKey(tableName))
      this.NestedEnums.Add(tableName, rowsEnum);
    else
      this.NestedEnums[tableName] = rowsEnum;
  }

  private object GetDataTable(string tableName)
  {
    if (this.m_conn != null)
      return (object) this.GetDataTableConn(tableName);
    return this.m_dataSetDocIO != null ? (object) this.GetDataTable(tableName, this.m_dataSetDocIO) : (object) this.GetDataTableDSet(tableName);
  }

  private MailMergeDataTable GetDataTable(string tableName, MailMergeDataSet dataSet)
  {
    MailMergeDataTable dataTable = (MailMergeDataTable) null;
    if (dataSet != null)
      dataTable = dataSet.GetDataTable(tableName);
    if (dataTable == null)
    {
      if (this.m_commandsDocIO != null)
        return (MailMergeDataTable) null;
      object cellValue = this.NestedEnums[this.m_groupSelector.GroupName].GetCellValue(tableName);
      if (cellValue is IEnumerable && !(cellValue is IDictionary<string, object>))
      {
        IEnumerable enumerable = cellValue as IEnumerable;
        return new MailMergeDataTable(tableName, enumerable);
      }
      List<object> objectList = new List<object>();
      if (cellValue != null)
        objectList.Add(cellValue);
      return new MailMergeDataTable(tableName, (IEnumerable) objectList);
    }
    string command = this.GetCommand(tableName);
    if (command == string.Empty || this.m_commandsDocIO == null)
    {
      dataTable.Command = string.Empty;
      return dataTable;
    }
    if (command == null)
    {
      List<object> objectList = new List<object>();
      return new MailMergeDataTable(tableName, (IEnumerable) objectList);
    }
    dataTable.Command = command;
    dataTable.Select(command);
    return dataTable;
  }

  private string GetCommand(string tableName)
  {
    DictionaryEntry dictionaryEntry = new DictionaryEntry((object) string.Empty, (object) string.Empty);
    bool flag = false;
    if (this.m_commands != null)
    {
      int index = 0;
      for (int count = this.m_commands.Count; index < count; ++index)
      {
        dictionaryEntry = (DictionaryEntry) this.m_commands[index];
        if (tableName == (string) dictionaryEntry.Key)
        {
          flag = true;
          break;
        }
      }
    }
    else if (this.m_commandsDocIO != null)
    {
      int index = 0;
      for (int count = this.m_commandsDocIO.Count; index < count; ++index)
      {
        dictionaryEntry = this.m_commandsDocIO[index];
        if (tableName == (string) dictionaryEntry.Key)
        {
          flag = true;
          break;
        }
      }
    }
    if (!flag)
      return (string) null;
    string command = (string) dictionaryEntry.Value;
    return command.IndexOf("%") == -1 ? command : this.UpdateVarCmd(command);
  }

  private string UpdateVarCmd(string command)
  {
    MatchCollection matchCollection = this.VariableCommandRegex.Matches(command);
    if (matchCollection.Count == 0)
      return (string) null;
    char[] chArray = new char[1]{ '.' };
    int i = 0;
    for (int count = matchCollection.Count; i < count; ++i)
    {
      string str = matchCollection[i].Value.Replace("%", string.Empty);
      string[] strArray = str.Split(chArray);
      if (strArray.Length != 2)
        throw new ArgumentException("String value between '%' symbols (variable command) is not valid.");
      IRowsEnumerator rowsEnumerator = (IRowsEnumerator) null;
      if (this.NestedEnums.ContainsKey(strArray[0]))
        rowsEnumerator = this.NestedEnums[strArray[0]];
      if (rowsEnumerator == null)
        return string.Empty;
      string newValue = rowsEnumerator.GetCellValue(strArray[1]).ToString();
      if (this.m_dataSet != null && this.m_dataSet.Tables.Contains(strArray[0]))
      {
        DataTable table = this.m_dataSet.Tables[strArray[0]];
        if (table.Columns.Contains(strArray[1]) && table.Columns[strArray[1]].DataType.Name == "String")
          newValue = $"'{newValue.Replace("'", "''")}'";
      }
      command = command.Replace($"%{str}%", newValue);
    }
    return command;
  }

  private void VerifyNestedGroups(int startRow, int endRow, WTable table)
  {
    Dictionary<string, WMergeField> dictionary1 = new Dictionary<string, WMergeField>();
    Dictionary<string, WMergeField> dictionary2 = new Dictionary<string, WMergeField>();
    for (int index = startRow; index <= endRow; ++index)
    {
      foreach (WTextBody cell in (CollectionImpl) table.Rows[index].Cells)
      {
        foreach (WParagraph paragraph in (IEnumerable) cell.Paragraphs)
        {
          foreach (ParagraphItem paragraphItem in (CollectionImpl) paragraph.Items)
          {
            if (paragraphItem is WMergeField)
            {
              WMergeField field = paragraphItem as WMergeField;
              if (MailMerge.IsBeginGroup(field) && !string.IsNullOrEmpty(field.FieldName) && !dictionary1.ContainsKey(field.FieldName))
                dictionary1.Add(field.FieldName, field);
              else if (MailMerge.IsEndGroup(field) && !string.IsNullOrEmpty(field.FieldName) && !dictionary2.ContainsKey(field.FieldName))
                dictionary2.Add(field.FieldName, field);
            }
          }
        }
      }
    }
    if (dictionary1.Count == 0)
    {
      if (dictionary2.Count > 0)
      {
        using (Dictionary<string, WMergeField>.KeyCollection.Enumerator enumerator = dictionary2.Keys.GetEnumerator())
        {
          if (enumerator.MoveNext())
            throw new ApplicationException($"GroupEnd field \"{enumerator.Current}\" doesn't have GroupStart field equivalent.");
        }
      }
    }
    else if (dictionary2.Count == 0 && dictionary1.Count > 0)
    {
      using (Dictionary<string, WMergeField>.KeyCollection.Enumerator enumerator = dictionary1.Keys.GetEnumerator())
      {
        if (enumerator.MoveNext())
          throw new ApplicationException($"GroupStart field \"{enumerator.Current}\" doesn't have GroupEnd field equivalent.");
      }
    }
    foreach (string key in dictionary1.Keys)
    {
      if (!dictionary2.ContainsKey(key))
        throw new ApplicationException($"GroupStart field \"{key}\" doesn't have GroupEnd field equivalent.");
      dictionary2.Remove(key);
    }
    if (dictionary2.Count > 0)
    {
      using (Dictionary<string, WMergeField>.KeyCollection.Enumerator enumerator = dictionary2.Keys.GetEnumerator())
      {
        if (enumerator.MoveNext())
          throw new ApplicationException($"GroupEnd field \"{enumerator.Current}\" doesn't have GroupStart field equivalent.");
      }
    }
    dictionary1.Clear();
    dictionary2.Clear();
  }

  private string FindNestedGroup(int startRow, int endRow, WTable table)
  {
    for (int index = startRow; index <= endRow; ++index)
    {
      foreach (WTextBody cell in (CollectionImpl) table.Rows[index].Cells)
      {
        foreach (WParagraph paragraph in (IEnumerable) cell.Paragraphs)
        {
          foreach (ParagraphItem paragraphItem in (CollectionImpl) paragraph.Items)
          {
            if (paragraphItem is WMergeField)
            {
              WMergeField field = paragraphItem as WMergeField;
              if (MailMerge.IsBeginGroup(field) && field.FieldName != string.Empty)
              {
                string fieldName = (paragraphItem as WMergeField).FieldName;
                return fieldName == string.Empty ? (string) null : fieldName;
              }
            }
          }
        }
      }
    }
    return (string) null;
  }

  private DataTable GetDataTableConn(string tableName)
  {
    string cmdText = this.GetCommand(tableName);
    if (cmdText == null)
      cmdText = "Select * from " + tableName;
    else if (cmdText == string.Empty)
      return (DataTable) null;
    DataTable dataTable = new DataTable(tableName);
    DbDataAdapter dbDataAdapter = !this.IsSqlConnection ? (DbDataAdapter) new OleDbDataAdapter((DbCommand) new OleDbCommand(cmdText, this.m_conn as OleDbConnection) as OleDbCommand) : (DbDataAdapter) new SqlDataAdapter((DbCommand) new SqlCommand(cmdText, this.m_conn as SqlConnection) as SqlCommand);
    try
    {
      dbDataAdapter?.Fill(dataTable);
    }
    catch
    {
      return dataTable;
    }
    return dataTable;
  }

  private DataTable GetDataTableDSet(string tableName)
  {
    DataTable table = this.m_dataSet.Tables[tableName];
    if (table == null)
      return new DataTable(tableName);
    string command = this.GetCommand(tableName);
    if (command == null)
      return (DataTable) null;
    DataRow[] dataRowArray = (DataRow[]) null;
    try
    {
      dataRowArray = table.Select(command);
    }
    catch
    {
    }
    DataTable dataTableDset = new DataTable(tableName);
    foreach (DataColumn column in (InternalDataCollectionBase) table.Columns)
      dataTableDset.Columns.Add(column.ColumnName).DataType = column.DataType;
    if (dataRowArray == null)
      return dataTableDset;
    foreach (DataRow dataRow in dataRowArray)
    {
      DataRow row = dataTableDset.NewRow();
      row.ItemArray = dataRow.ItemArray;
      row.RowError = dataRow.RowError;
      dataTableDset.Rows.Add(row);
    }
    return dataTableDset;
  }

  protected MergeFieldEventArgs SendMergeField(
    IWMergeField field,
    object value,
    IRowsEnumerator rowsEnum)
  {
    string groupName = this.GetGroupName(rowsEnum != null ? rowsEnum.TableName : string.Empty);
    MergeFieldEventArgs args = new MergeFieldEventArgs((IWordDocument) this.Document, rowsEnum.TableName, rowsEnum.CurrentRowIndex, field, value, groupName);
    if (this.MergeField != null)
      this.MergeField((object) this, args);
    return args;
  }

  internal MergeFieldEventArgs SendMergeField(
    IWMergeField field,
    object value,
    IRowsEnumerator rowsEnum,
    int valIndex)
  {
    MergeFieldEventArgs args = (MergeFieldEventArgs) null;
    string groupName = this.GetGroupName(rowsEnum != null ? rowsEnum.TableName : string.Empty);
    if (rowsEnum != null)
      args = new MergeFieldEventArgs((IWordDocument) this.Document, rowsEnum.TableName, rowsEnum.CurrentRowIndex, field, value, groupName);
    else if (valIndex != -1)
      args = new MergeFieldEventArgs((IWordDocument) this.Document, "", valIndex, field, (object) this.m_values[valIndex], groupName);
    if (this.MergeField != null && args != null)
      this.MergeField((object) this, args);
    return args;
  }

  protected MergeImageFieldEventArgs SendMergeImageField(
    IWMergeField field,
    object bmp,
    IRowsEnumerator rowsEnum)
  {
    MergeImageFieldEventArgs args = rowsEnum == null ? new MergeImageFieldEventArgs((IWordDocument) this.Document, (string) null, int.MaxValue, field, bmp) : new MergeImageFieldEventArgs((IWordDocument) this.Document, rowsEnum.TableName, rowsEnum.CurrentRowIndex, field, bmp);
    if (this.MergeImageField != null)
      this.MergeImageField((object) this, args);
    return args;
  }

  internal BeforeClearFieldEventArgs SendBeforeClearField(
    IRowsEnumerator rowsEnum,
    IWMergeField mergeField,
    object value)
  {
    bool fieldHasMappedInDataSource1 = false;
    string groupName = this.GetGroupName(rowsEnum != null ? rowsEnum.TableName : string.Empty);
    BeforeClearFieldEventArgs args;
    if (rowsEnum == null)
    {
      args = new BeforeClearFieldEventArgs(this.Document, groupName, -1, mergeField, fieldHasMappedInDataSource1, value);
    }
    else
    {
      List<string> stringList = new List<string>((IEnumerable<string>) rowsEnum.ColumnNames);
      for (int index = 0; index < stringList.Count; ++index)
        stringList[index] = stringList[index].ToLower();
      bool fieldHasMappedInDataSource2 = stringList.Contains(mergeField.FieldName.ToLower());
      args = new BeforeClearFieldEventArgs(this.Document, groupName, rowsEnum.CurrentRowIndex, mergeField, fieldHasMappedInDataSource2, value);
    }
    if (args != null && this.BeforeClearField != null)
      this.BeforeClearField((object) this, args);
    return args;
  }

  internal BeforeClearGroupFieldEventArgs SendBeforeClearGroup(
    IRowsEnumerator rowsEnum,
    MailMerge.GroupSelector groupSelector)
  {
    bool fieldHasMappedInDataSource = false;
    for (int index = 0; index < this.Document.Fields.Count; ++index)
    {
      if (this.Document.Fields.InnerList[index] is WMergeField inner && inner.FieldName == string.Empty && inner.FieldCode.Contains(rowsEnum.TableName))
        inner.FieldName = rowsEnum.TableName;
    }
    string groupName = this.GetGroupName(rowsEnum != null ? rowsEnum.TableName : string.Empty);
    string[] array = groupSelector.FieldNames.ToArray();
    BeforeClearGroupFieldEventArgs args = new BeforeClearGroupFieldEventArgs(this.Document, groupName, this as IWMergeField, fieldHasMappedInDataSource, array);
    if (this.BeforeClearGroupField != null && args != null)
      this.BeforeClearGroupField((object) this, args);
    return args;
  }

  private string GetGroupName(string currGroupName)
  {
    string groupName = string.Empty;
    if (this.NestedEnums != null)
    {
      if (this.NestedEnums.Count > 0)
      {
        List<string> stringList = new List<string>((IEnumerable<string>) this.NestedEnums.Keys);
        for (int index = 0; index < stringList.Count; ++index)
          groupName = $"{groupName}{stringList[index]}:";
        groupName = groupName.TrimEnd(':');
      }
      else if (!string.IsNullOrEmpty(currGroupName))
        groupName = currGroupName;
    }
    if (!string.IsNullOrEmpty(currGroupName) && !groupName.Contains(currGroupName))
      groupName = $"{groupName}:{currGroupName}";
    return groupName;
  }

  private void Execute(IRowsEnumerator rowsEnum)
  {
    this.Document.IsMailMerge = true;
    this.RemoveSpellChecking();
    int num1 = rowsEnum != null ? rowsEnum.RowsCount : throw new ArgumentNullException(nameof (rowsEnum));
    int num2 = 0;
    if (num1 > 1)
      this.CopyContent(this.Document);
    IWSectionCollection sections = (IWSectionCollection) this.Document.Sections;
    rowsEnum.Reset();
    if (rowsEnum.RowsCount == 0 && this.ClearFields)
    {
      int index = 0;
      for (int count = sections.Count; index < count; ++index)
        this.ExecuteForSection((IWSection) sections[index], (IRowsEnumerator) null);
    }
    else
    {
      while (this.CheckNextRow(rowsEnum))
      {
        int index = num2;
        for (int count = sections.Count; index < count; ++index)
          this.ExecuteForSection((IWSection) sections[index], rowsEnum);
        num2 = sections.Count;
        if (!rowsEnum.IsLast)
          this.AppendCopiedContent(this.Document);
      }
    }
    this.Document.IsMailMerge = false;
  }

  private bool CheckNextRow(IRowsEnumerator rowsEnum)
  {
    if (!this.IsNested || !(rowsEnum is DataTableEnumerator) || (rowsEnum as DataTableEnumerator).m_MMtable == null)
      return rowsEnum.NextRow();
    string[] tableCommand = this.GetTableCommand((rowsEnum as DataTableEnumerator).Command);
    return tableCommand == null ? rowsEnum.NextRow() : (rowsEnum as DataTableEnumerator).NextRow(tableCommand);
  }

  private void ExecuteForSection(IWSection sec, IRowsEnumerator rowsEnum)
  {
    this.ExecuteForTextBody(sec.Body.Items, rowsEnum);
    for (int index = 0; index < 6; ++index)
    {
      BodyItemCollection childEntities = (BodyItemCollection) sec.HeadersFooters[index].ChildEntities;
      if (childEntities.Count > 0)
        this.ExecuteForTextBody(childEntities, rowsEnum);
    }
  }

  private void ExecuteForTextBody(BodyItemCollection bodyItems, IRowsEnumerator rowsEnum)
  {
    for (int index = 0; index < bodyItems.Count; ++index)
    {
      ITextBodyItem bodyItem = (ITextBodyItem) bodyItems[index];
      if (bodyItem != null)
        this.ExecuteForTextBodyItem(bodyItem, rowsEnum);
    }
  }

  private void ExecuteForTextBodyItem(ITextBodyItem item, IRowsEnumerator rowsEnum)
  {
    switch (item)
    {
      case IWParagraph _:
        this.ExecuteForParagraph(item as WParagraph, rowsEnum);
        break;
      case IWTable _:
        if (!(item is IWTable table))
          break;
        this.ExecuteForTable(table, rowsEnum);
        break;
      case BlockContentControl _:
        BodyItemCollection childEntities = (item as BlockContentControl).TextBody.ChildEntities as BodyItemCollection;
        if (childEntities.Count <= 0)
          break;
        ITextBodyItem nextSibling;
        for (ITextBodyItem textBodyItem = (ITextBodyItem) childEntities[0]; textBodyItem != null; textBodyItem = nextSibling)
        {
          nextSibling = textBodyItem.NextSibling as ITextBodyItem;
          this.ExecuteForTextBodyItem(textBodyItem, rowsEnum);
        }
        break;
    }
  }

  private void ExecuteForParagraph(WParagraph paragraph, IRowsEnumerator rowsEnum)
  {
    bool paraItemCollectionChanged = false;
    int count1 = paragraph.Items.Count;
    for (int index = 0; index < paragraph.Items.Count && !paragraph.DeepDetached; ++index)
    {
      this.ExecuteForParagraphItems(paragraph[index], paragraph, rowsEnum, ref paraItemCollectionChanged);
      if (paraItemCollectionChanged)
      {
        int count2 = paragraph.Items.Count;
        --index;
        paraItemCollectionChanged = false;
        if (this.RemoveEmptyParagraphs && !paragraph.RemoveEmpty && (paragraph.Items.Count == 0 || paragraph.IsOnlyHasSpaces()))
        {
          if (paragraph.Owner is WTableCell owner && (owner.ChildEntities.Count == 1 || paragraph == owner.LastParagraph && this.IsRemainingParaHasRemoveEmpty(owner)))
            paragraph.ChildEntities.Clear();
          else
            paragraph.RemoveEmpty = true;
        }
      }
    }
  }

  private bool IsRemainingParaHasRemoveEmpty(WTableCell parentCell)
  {
    bool flag = false;
    foreach (WParagraph childEntity in (CollectionImpl) parentCell.ChildEntities)
    {
      if (childEntity != parentCell.LastParagraph)
      {
        if (childEntity.RemoveEmpty)
        {
          flag = true;
        }
        else
        {
          flag = false;
          break;
        }
      }
    }
    return flag;
  }

  private void ExecuteForParagraphItems(
    ParagraphItem pItem,
    WParagraph paragraph,
    IRowsEnumerator rowsEnum,
    ref bool paraItemCollectionChanged)
  {
    switch (pItem)
    {
      case WMergeField wmergeField:
        wmergeField.UpdateFieldMarks();
        if (this._isInValidNextField && this._previousMergeField != null && this._previousMergeField.FieldName == wmergeField.FieldName)
        {
          if (!this.ClearFields)
            break;
          paraItemCollectionChanged = this.RemoveField((WField) wmergeField, true) != -1;
          break;
        }
        paraItemCollectionChanged = !wmergeField.Prefix.StartsWithExt("Image") ? this.UpdateMergeFieldValue(wmergeField, rowsEnum) : this.UpdateImageMergeFieldValue(wmergeField, rowsEnum);
        this._previousMergeField = wmergeField;
        break;
      case WField _:
        WField field = pItem as WField;
        if (field.FieldType == FieldType.FieldNext)
        {
          if (rowsEnum != null && !rowsEnum.IsEnd)
            this.CheckNextRow(rowsEnum);
          else
            this._isInValidNextField = true;
          paraItemCollectionChanged = this.RemoveField(field, false) != -1;
          break;
        }
        if (field.FieldType == FieldType.FieldNextIf)
        {
          if (field.UpdateNextIfField() && rowsEnum != null && !rowsEnum.IsEnd)
            this.CheckNextRow(rowsEnum);
          paraItemCollectionChanged = this.RemoveField(field, false) != -1;
          break;
        }
        if (field.FieldType == FieldType.FieldIf)
        {
          if (this.m_IfFieldCollections == null)
            this.m_IfFieldCollections = new Stack<WIfField>();
          this.m_IfFieldCollections.Push(field as WIfField);
          this.UpdateIfFieldValue(field as WIfField, rowsEnum);
          break;
        }
        if (field.FieldType != FieldType.FieldMergeRec && field.FieldType != FieldType.FieldMergeSeq)
          break;
        int num = 1;
        if (rowsEnum != null)
          num += rowsEnum.CurrentRowIndex;
        this.ConvertToText(field, num.ToString());
        break;
      case WTextBox _:
        this.ExecuteForTextBody((BodyItemCollection) (pItem as WTextBox).TextBoxBody.ChildEntities, rowsEnum);
        break;
      case Shape _:
        this.ExecuteForTextBody((BodyItemCollection) (pItem as Shape).TextBody.ChildEntities, rowsEnum);
        break;
      case InlineContentControl _:
        IEnumerator enumerator1 = (pItem as InlineContentControl).ParagraphItems.GetEnumerator();
        try
        {
          while (enumerator1.MoveNext())
            this.ExecuteForParagraphItems((ParagraphItem) enumerator1.Current, paragraph, rowsEnum, ref paraItemCollectionChanged);
          break;
        }
        finally
        {
          if (enumerator1 is IDisposable disposable)
            disposable.Dispose();
        }
      case WComment _:
        IEnumerator enumerator2 = (pItem as WComment).ChildEntities.GetEnumerator();
        try
        {
          while (enumerator2.MoveNext())
          {
            WParagraph current = (WParagraph) enumerator2.Current;
            for (int index = 0; index < current.Items.Count; ++index)
              this.ExecuteForParagraphItems(current.Items[index], current, rowsEnum, ref paraItemCollectionChanged);
          }
          break;
        }
        finally
        {
          if (enumerator2 is IDisposable disposable)
            disposable.Dispose();
        }
      case WFootnote _:
        IEnumerator enumerator3 = (pItem as WFootnote).TextBody.ChildEntities.GetEnumerator();
        try
        {
          while (enumerator3.MoveNext())
          {
            WParagraph current = (WParagraph) enumerator3.Current;
            for (int index = 0; index < current.Items.Count; ++index)
              this.ExecuteForParagraphItems(current.Items[index], current, rowsEnum, ref paraItemCollectionChanged);
          }
          break;
        }
        finally
        {
          if (enumerator3 is IDisposable disposable)
            disposable.Dispose();
        }
      case WFieldMark _:
        if (((WFieldMark) pItem).Type != FieldMarkType.FieldEnd || !(((WFieldMark) pItem).ParentField is WIfField) || this.m_IfFieldCollections == null)
          break;
        if (this.m_IfFieldCollections.Count == 1)
        {
          this.m_IfFieldCollections.Clear();
          this.m_IfFieldCollections = (Stack<WIfField>) null;
          break;
        }
        this.m_IfFieldCollections.Pop();
        break;
    }
  }

  private void ExecuteForTable(IWTable table, IRowsEnumerator rowsEnum)
  {
    if (table.Rows.Count <= 0)
      return;
    WTableRow nextSibling1;
    for (WTableRow wtableRow = table.Rows[0]; wtableRow != null; wtableRow = nextSibling1)
    {
      nextSibling1 = wtableRow.NextSibling as WTableRow;
      WTableCell nextSibling2;
      if (wtableRow.Cells.Count > 0)
      {
        for (WTableCell wtableCell = wtableRow.Cells[0]; wtableCell != null; wtableCell = nextSibling2)
        {
          nextSibling2 = wtableCell.NextSibling as WTableCell;
          this.ExecuteForTextBody((BodyItemCollection) wtableCell.ChildEntities, rowsEnum);
        }
      }
    }
  }

  private void ConvertToText(WField field, string text)
  {
    WTextRange wtextRange = new WTextRange((IWordDocument) this.Document);
    WParagraph ownerParagraph = field.OwnerParagraph;
    int inOwnerCollection = field.GetIndexInOwnerCollection();
    ownerParagraph.Items.Remove((IEntity) field);
    ownerParagraph.Items.Insert(inOwnerCollection, (IEntity) wtextRange);
    wtextRange.CharacterFormat.ImportContainer((FormatBase) field.CharacterFormat);
    wtextRange.CharacterFormat.CopyProperties((FormatBase) field.CharacterFormat);
    if (wtextRange.CharacterFormat.HasValue(106))
      wtextRange.CharacterFormat.PropertiesHash.Remove(106);
    wtextRange.Text = text;
  }

  private bool UpdateMergeFieldValue(WMergeField mergeField, IRowsEnumerator rowsEnum)
  {
    bool flag = false;
    bool clearFields = this.ClearFields;
    if (rowsEnum == null)
    {
      flag = this.UpdateMergeFieldValue(mergeField);
    }
    else
    {
      object obj = this.TriggerSendBeforeClearFieldEvent(this.GetFieldValue((IWMergeField) mergeField, rowsEnum), rowsEnum, mergeField);
      if (obj != null)
        flag = this.UpdateMergeFieldResult(mergeField, obj, rowsEnum, -1);
      else if (this.ClearFields)
        flag = this.RemoveField((WField) mergeField, true) != -1;
    }
    this.ClearFields = clearFields;
    return flag;
  }

  private object TriggerSendBeforeClearFieldEvent(
    object value,
    IRowsEnumerator rowsEnum,
    WMergeField mergeField)
  {
    if ((value == null || value.ToString() == string.Empty) && this.BeforeClearField != null)
    {
      BeforeClearFieldEventArgs clearFieldEventArgs = this.SendBeforeClearField(rowsEnum, (IWMergeField) mergeField, value);
      if (clearFieldEventArgs != null)
      {
        this.ClearFields = clearFieldEventArgs.ClearField;
        if (!clearFieldEventArgs.ClearField && clearFieldEventArgs.FieldValue != null && !string.IsNullOrEmpty(clearFieldEventArgs.FieldValue.ToString()))
          value = clearFieldEventArgs.FieldValue;
      }
    }
    return value;
  }

  private bool UpdateMergeFieldResult(
    WMergeField mergeField,
    object value,
    IRowsEnumerator rowsEnum,
    int valIndex)
  {
    bool flag = false;
    bool clearFields = this.ClearFields;
    MergeFieldEventArgs args = this.SendMergeField((IWMergeField) mergeField, value, rowsEnum, valIndex);
    if (args != null)
    {
      if (args.TextRange.Text == args.Text)
        mergeField.FieldResult = args.Text;
      else
        mergeField.FieldResult = args.TextRange.Text;
    }
    if (this.m_IfFieldCollections != null && this.m_IfFieldCollections.Count > 0)
      this.EnusreDoubelQuotesForResult(this.m_IfFieldCollections.Peek(), mergeField);
    value = this.TriggerSendBeforeClearFieldEvent(value, rowsEnum, mergeField);
    if (mergeField.Owner is WParagraph && mergeField.FieldEnd != null && mergeField.FieldEnd.Owner is WParagraph)
    {
      if (value != null && value.ToString() != string.Empty)
      {
        this.InsertMergeFieldResultAsTextRange(mergeField, args);
        flag = this.RemoveField((WField) mergeField, true) != -1;
      }
      else if (this.ClearFields)
        flag = this.RemoveField((WField) mergeField, true) != -1;
    }
    this.ClearFields = clearFields;
    return flag;
  }

  private void EnusreDoubelQuotesForResult(WIfField ifField, WMergeField mergeField)
  {
    string textBefore = string.Empty;
    string textAfter = string.Empty;
    if (!string.IsNullOrEmpty(mergeField.TextBefore))
      textBefore = mergeField.TextBefore;
    if (!string.IsNullOrEmpty(mergeField.TextAfter))
      textAfter = mergeField.TextAfter;
    string resultText = textBefore + mergeField.FieldResult + textAfter;
    if (string.IsNullOrEmpty(resultText) || resultText.Trim().Contains(" "))
    {
      int doubleQuotesCount = 0;
      int inOwnerCollection1 = ifField.GetIndexInOwnerCollection();
      int inOwnerCollection2 = ifField.FieldSeparator != null ? ifField.FieldSeparator.GetIndexInOwnerCollection() : 0;
      int inOwnerCollection3 = mergeField.GetIndexInOwnerCollection();
      WParagraph ownerParagraph1 = ifField.OwnerParagraph;
      WParagraph ownerParagraph2 = mergeField.OwnerParagraph;
      if (inOwnerCollection2 == 0 || inOwnerCollection3 <= inOwnerCollection2)
      {
        bool flag = true;
        if (ownerParagraph1 == ownerParagraph2)
          this.CountDoubleQuotes(ownerParagraph1, inOwnerCollection1 + 1, inOwnerCollection3, ref doubleQuotesCount);
        else if (ownerParagraph1.OwnerTextBody == ownerParagraph2.OwnerTextBody)
        {
          int inOwnerCollection4 = ownerParagraph1.GetIndexInOwnerCollection();
          int inOwnerCollection5 = ownerParagraph2.GetIndexInOwnerCollection();
          WTextBody ownerTextBody = ownerParagraph1.OwnerTextBody;
          for (int index = inOwnerCollection4; index <= inOwnerCollection5 && index < ownerTextBody.ChildEntities.Count; ++index)
          {
            if (ownerTextBody.ChildEntities[index] is WParagraph childEntity)
              this.CountDoubleQuotes(childEntity, index == inOwnerCollection4 ? inOwnerCollection1 + 1 : 0, index == inOwnerCollection5 ? inOwnerCollection3 : childEntity.ChildEntities.Count, ref doubleQuotesCount);
          }
        }
        else
          flag = false;
        if (ifField.FieldValue.Contains("\""))
          doubleQuotesCount += ifField.FieldValue.Length - ifField.FieldValue.Replace("\"", "").Length;
        if (flag && doubleQuotesCount % 2 == 0)
        {
          mergeField.TextBefore = textBefore = "\"" + textBefore;
          mergeField.TextAfter = (textAfter += "\"");
          resultText = textBefore + mergeField.FieldResult + textAfter;
        }
      }
    }
    ifField.EnusreSpaceInResultText(mergeField, resultText, textBefore, textAfter);
  }

  private void CountDoubleQuotes(
    WParagraph paragraph,
    int startIindex,
    int endIndex,
    ref int doubleQuotesCount)
  {
    for (int index = startIindex; index < endIndex && index < paragraph.ChildEntities.Count; ++index)
    {
      if (paragraph.ChildEntities[index] is WTextRange childEntity && childEntity.Text.Contains("\""))
        doubleQuotesCount += childEntity.Text.Length - childEntity.Text.Replace("\"", "").Length;
    }
  }

  private bool IsDeepDetached(WField field)
  {
    return field.Owner is WParagraph && (field.Owner as WParagraph).DeepDetached || field.FieldEnd.Owner is WParagraph && (field.FieldEnd.Owner as WParagraph).DeepDetached;
  }

  private int RemoveField(WField field, bool isMergeStartAndEndPara)
  {
    if (!(field.Owner is WParagraph) || field.FieldEnd == null || !(field.FieldEnd.Owner is WParagraph) || field.OwnerParagraph.OwnerTextBody == null || field.FieldEnd.OwnerParagraph.OwnerTextBody == null || this.IsDeepDetached(field))
      return -1;
    int inOwnerCollection1 = field.GetIndexInOwnerCollection();
    WParagraph ownerParagraph1 = field.OwnerParagraph;
    BookmarkStart bookmarkStart = new BookmarkStart((IWordDocument) this.m_doc, "_fieldBookmark");
    BookmarkEnd bookmarkEnd = new BookmarkEnd((IWordDocument) this.m_doc, "_fieldBookmark");
    ownerParagraph1.Items.Insert(inOwnerCollection1, (IEntity) bookmarkStart);
    field.EnsureBookmarkStart(bookmarkStart);
    WParagraph ownerParagraph2 = field.FieldEnd.OwnerParagraph;
    int inOwnerCollection2 = field.FieldEnd.GetIndexInOwnerCollection();
    ownerParagraph2.Items.Insert(inOwnerCollection2 + 1, (IEntity) bookmarkEnd);
    field.EnsureBookmarkStart(bookmarkEnd);
    BookmarksNavigator bookmarksNavigator = new BookmarksNavigator((IWordDocument) this.m_doc);
    bookmarksNavigator.MoveToBookmark("_fieldBookmark");
    bookmarksNavigator.RemoveEmptyParagraph = false;
    this.Document.IsSkipFieldDetach = true;
    bookmarksNavigator.DeleteBookmarkContent(false);
    this.Document.IsSkipFieldDetach = false;
    if (ownerParagraph1.Items.Contains((IEntity) bookmarkStart))
      ownerParagraph1.Items.Remove((IEntity) bookmarkStart);
    if (ownerParagraph2.Items.Contains((IEntity) bookmarkEnd))
      ownerParagraph2.Items.Remove((IEntity) bookmarkEnd);
    if (isMergeStartAndEndPara && ownerParagraph1 != ownerParagraph2)
    {
      int index = 0;
      while (ownerParagraph1.ChildEntities.Count > 0)
      {
        ownerParagraph2.ChildEntities.Insert(index, (IEntity) ownerParagraph1.ChildEntities[0]);
        ++index;
      }
      ownerParagraph1.RemoveEmpty = true;
    }
    return inOwnerCollection1;
  }

  private void InsertMergeFieldResultAsTextRange(WMergeField mergeField, MergeFieldEventArgs args)
  {
    List<WCharacterFormat> characterFormatting = mergeField.GetResultCharacterFormatting();
    int inOwnerCollection = mergeField.GetIndexInOwnerCollection();
    string empty = string.Empty;
    if (!string.IsNullOrEmpty(mergeField.TextBefore))
      empty += mergeField.TextBefore;
    string str1 = empty + mergeField.FieldResult;
    if (!string.IsNullOrEmpty(mergeField.TextAfter))
      str1 += mergeField.TextAfter;
    if (characterFormatting.Count <= 1)
    {
      WTextRange textRange = args.TextRange;
      textRange.Text = str1;
      if (characterFormatting.Count == 1 && textRange.CharacterFormat.HasValue(106))
        textRange.CharacterFormat.PropertiesHash.Remove(106);
      mergeField.OwnerParagraph.Items.Insert(inOwnerCollection, (IEntity) textRange);
    }
    else
    {
      List<string> stringList = new List<string>((IEnumerable<string>) str1.Split(' ', ',', '.', '/', '-', ':', '\t', '�', '�'));
      WTextRange wtextRange = (WTextRange) null;
      WCharacterFormat wcharacterFormat1 = (WCharacterFormat) null;
      foreach (string str2 in stringList)
      {
        WCharacterFormat wcharacterFormat2 = characterFormatting.Count > 0 ? characterFormatting[0] : (WCharacterFormat) null;
        if (wcharacterFormat1 != null && wcharacterFormat2 != null && wcharacterFormat1.Compare(wcharacterFormat2))
        {
          str1 = str1.Remove(0, str2.Length);
          string str3 = str2;
          if (str1.Length > 0)
          {
            str3 += (string) (object) str1[0];
            str1 = str1.Remove(0, 1);
          }
          wtextRange.Text += str3;
        }
        else
        {
          wtextRange = new WTextRange((IWordDocument) this.m_doc);
          str1 = str1.Remove(0, str2.Length);
          string str4 = str2;
          if (str1.Length > 0)
          {
            str4 += (string) (object) str1[0];
            str1 = str1.Remove(0, 1);
          }
          wtextRange.Text = str4;
          if (wcharacterFormat2 != null)
          {
            if (wcharacterFormat2.HasValue(106))
              wcharacterFormat2.PropertiesHash.Remove(106);
            wtextRange.ApplyCharacterFormat(wcharacterFormat2);
          }
          else if (mergeField.FieldSeparator != null && mergeField.FieldSeparator.NextSibling != null)
          {
            if ((mergeField.FieldSeparator.NextSibling as ParagraphItem).ParaItemCharFormat.HasValue(106))
              wcharacterFormat2.PropertiesHash.Remove(106);
            wtextRange.ApplyCharacterFormat((mergeField.FieldSeparator.NextSibling as ParagraphItem).ParaItemCharFormat);
          }
          mergeField.OwnerParagraph.Items.Insert(inOwnerCollection, (IEntity) wtextRange);
          ++inOwnerCollection;
        }
        wcharacterFormat1 = wcharacterFormat2;
        if (characterFormatting.Count > 0)
          characterFormatting.RemoveAt(0);
      }
    }
  }

  private object GetFieldValue(IWMergeField field, IRowsEnumerator rowsEnum)
  {
    string fieldName1 = field.FieldName;
    string key = "";
    int num = 0;
    string[] array = (string[]) null;
    if (this.IsNested && this.m_nestedEnums != null && this.m_nestedEnums.Count > 0)
    {
      if (fieldName1.Contains(":"))
        key = fieldName1.Substring(0, fieldName1.IndexOf(":"));
      if (!string.IsNullOrEmpty(key) && this.m_nestedEnums.ContainsKey(key))
      {
        rowsEnum = this.m_nestedEnums[key];
        fieldName1 = fieldName1.Remove(0, key.Length + 1);
      }
      else
      {
        array = new string[this.m_nestedEnums.Count];
        this.m_nestedEnums.Keys.CopyTo(array, 0);
        if (this.m_nestedEnums.ContainsKey(rowsEnum.TableName))
        {
          foreach (KeyValuePair<string, IRowsEnumerator> nestedEnum in this.m_nestedEnums)
          {
            if (nestedEnum.Value != rowsEnum)
              ++num;
            else
              break;
          }
        }
        else
          num = this.m_nestedEnums.Count;
      }
    }
    string mappedColName = this.GetMappedColName(fieldName1);
    string fieldName2 = mappedColName == null ? fieldName1.ToUpper() : mappedColName.ToUpper();
    object fieldValue = (object) null;
    while (fieldValue == null && num >= 0)
    {
      fieldValue = this.GetFieldValue(fieldName2, rowsEnum);
      if (fieldValue == null && num > 0)
      {
        rowsEnum = this.m_nestedEnums[array[num - 1]];
        fieldValue = this.GetFieldValue(fieldName2, rowsEnum);
        --num;
      }
      if (num == 0)
        break;
    }
    return fieldValue;
  }

  private object GetFieldValue(string fieldName, IRowsEnumerator rowsEnum)
  {
    int index = 0;
    for (int length = rowsEnum.ColumnNames.Length; index < length; ++index)
    {
      string columnName = rowsEnum.ColumnNames[index];
      string upper = columnName.ToUpper();
      if (fieldName == upper || fieldName == $"\"{upper}\"")
        return rowsEnum.GetCellValue(columnName);
    }
    return (object) null;
  }

  private void UpdateIfFieldValue(WIfField field, IRowsEnumerator rowsEnum)
  {
    if (field.MergeFields.Count == 0 || rowsEnum == null)
      return;
    int index1 = 0;
    for (int length = rowsEnum.ColumnNames.Length; index1 < length; ++index1)
    {
      string columnName = rowsEnum.ColumnNames[index1];
      string upper = columnName.ToUpper();
      string empty = string.Empty;
      int index2 = 0;
      for (int count = field.MergeFields.Count; index2 < count; ++index2)
      {
        PseudoMergeField mergeField = field.MergeFields[index2];
        if (mergeField.Name != null && mergeField.Name.ToUpper() == upper)
        {
          object cellValue = rowsEnum.GetCellValue(columnName);
          mergeField.Value = cellValue.ToString();
        }
      }
    }
  }

  private bool UpdateImageMergeFieldValue(WMergeField mergeField, IRowsEnumerator rowsEnum)
  {
    bool flag = false;
    bool clearFields = this.ClearFields;
    if (rowsEnum == null && this.MergeImageField == null)
      flag = this.UpdateMergeFieldValue(mergeField);
    else if (rowsEnum == null && this.MergeImageField != null)
    {
      string fieldName = mergeField.FieldName;
      object obj1 = (object) null;
      for (int index = 0; index < this.m_names.Length; ++index)
      {
        if (fieldName != null && this.m_names[index].ToUpper() == fieldName.ToUpper())
        {
          obj1 = (object) this.m_values[index];
          break;
        }
      }
      object obj2 = this.TriggerSendBeforeClearFieldEvent(obj1, rowsEnum, mergeField);
      if (obj2 != null)
      {
        Bitmap bitmap = this.GetBitmap(obj2);
        if (bitmap != null)
          obj2 = (object) bitmap;
        MergeImageFieldEventArgs args = this.SendMergeImageField((IWMergeField) mergeField, obj2, rowsEnum);
        flag = this.UpdateMergedPicture(mergeField, args);
      }
      else if (this.ClearFields)
        flag = this.RemoveField((WField) mergeField, true) != -1;
    }
    else
    {
      object obj = this.TriggerSendBeforeClearFieldEvent(this.GetFieldValue((IWMergeField) mergeField, rowsEnum), rowsEnum, mergeField);
      if (obj != null)
      {
        Bitmap bitmap = this.GetBitmap(obj);
        if (bitmap != null)
          obj = (object) bitmap;
        MergeImageFieldEventArgs args = this.SendMergeImageField((IWMergeField) mergeField, obj, rowsEnum);
        flag = this.UpdateMergedPicture(mergeField, args);
      }
      else if (this.ClearFields)
        flag = this.RemoveField((WField) mergeField, true) != -1;
    }
    this.ClearFields = clearFields;
    return flag;
  }

  private bool UpdateMergedPicture(WMergeField mergeField, MergeImageFieldEventArgs args)
  {
    bool flag = false;
    if (args.UseText)
    {
      if (mergeField.Owner is WParagraph && mergeField.FieldEnd != null && mergeField.FieldEnd.Owner is WParagraph)
      {
        if (!string.IsNullOrEmpty(args.Text))
        {
          this.InsertMergeFieldResultAsTextRange(mergeField, (MergeFieldEventArgs) args);
          this.RemoveField((WField) mergeField, true);
          flag = true;
        }
        else if (this.ClearFields)
        {
          this.RemoveField((WField) mergeField, true);
          flag = true;
        }
      }
    }
    else if (!args.Skip && args.Image != null)
    {
      WParagraph ownerParagraph = mergeField.OwnerParagraph;
      int inOwnerCollection = mergeField.GetIndexInOwnerCollection();
      if (!string.IsNullOrEmpty(mergeField.TextBefore))
      {
        WTextRange wtextRange = new WTextRange((IWordDocument) this.m_doc);
        wtextRange.Text = mergeField.TextBefore;
        wtextRange.ApplyCharacterFormat(mergeField.CharacterFormat);
        ownerParagraph.Items.Insert(inOwnerCollection, (IEntity) wtextRange);
        ++inOwnerCollection;
      }
      IWPicture picture = (IWPicture) args.Picture;
      ownerParagraph.Items.Insert(inOwnerCollection, (IEntity) picture);
      int index = inOwnerCollection + 1;
      (picture as WPicture).LoadImage(args.Image);
      if (!string.IsNullOrEmpty(mergeField.TextAfter))
      {
        WTextRange wtextRange = new WTextRange((IWordDocument) this.m_doc);
        wtextRange.Text = mergeField.TextAfter;
        wtextRange.ApplyCharacterFormat(mergeField.CharacterFormat);
        ownerParagraph.Items.Insert(index, (IEntity) wtextRange);
      }
      flag = this.RemoveField((WField) mergeField, true) != -1;
    }
    else if (this.ClearFields)
      flag = this.RemoveField((WField) mergeField, true) != -1;
    return flag;
  }

  private bool UpdateMergeFieldValue(WMergeField mergeField)
  {
    bool flag = false;
    bool clearFields = this.ClearFields;
    if (this.ClearFields && this.m_values == null)
    {
      flag = this.RemoveField((WField) mergeField, true) != -1;
    }
    else
    {
      int rowIndex = -1;
      string mappedColName = this.GetMappedColName(mergeField.FieldName);
      if (mappedColName != null)
      {
        for (int index = 0; index < this.m_names.Length; ++index)
        {
          if (this.m_names[index].ToUpper() == mappedColName.ToUpper())
          {
            rowIndex = index;
            break;
          }
        }
      }
      if (rowIndex == -1)
      {
        for (int index = 0; index < this.m_names.Length; ++index)
        {
          if (this.m_names[index].ToUpper() == mergeField.FieldName.ToUpper())
          {
            rowIndex = index;
            break;
          }
        }
      }
      if (rowIndex == -1 && this.BeforeClearField != null)
      {
        BeforeClearFieldEventArgs clearFieldEventArgs = this.SendBeforeClearField((IRowsEnumerator) null, (IWMergeField) mergeField, (object) null);
        if (clearFieldEventArgs != null)
        {
          this.ClearFields = clearFieldEventArgs.ClearField;
          if (!clearFieldEventArgs.ClearField && !string.IsNullOrEmpty(clearFieldEventArgs.FieldValue.ToString()))
          {
            rowIndex = this.m_values.Length;
            this.m_values = new List<string>((IEnumerable<string>) this.m_values)
            {
              clearFieldEventArgs.FieldValue.ToString()
            }.ToArray();
          }
        }
      }
      if (rowIndex != -1)
      {
        MergeFieldEventArgs args = new MergeFieldEventArgs((IWordDocument) this.Document, "", rowIndex, (IWMergeField) mergeField, (object) this.m_values[rowIndex]);
        if (this.MergeField != null)
          this.MergeField((object) this, args);
        if (args.FieldValue != null)
        {
          if (args.TextRange.Text == args.Text)
            mergeField.FieldResult = args.Text;
          else
            mergeField.FieldResult = args.TextRange.Text;
          if (this.m_IfFieldCollections != null && this.m_IfFieldCollections.Count > 0)
            this.EnusreDoubelQuotesForResult(this.m_IfFieldCollections.Peek(), mergeField);
          if (mergeField.FieldResult == string.Empty)
          {
            if (this.ClearFields)
              flag = this.RemoveField((WField) mergeField, true) != -1;
          }
          else if (mergeField.Owner is WParagraph && mergeField.FieldEnd != null && mergeField.FieldEnd.Owner is WParagraph)
          {
            this.InsertMergeFieldResultAsTextRange(mergeField, args);
            flag = this.RemoveField((WField) mergeField, true) != -1;
          }
        }
        else if (this.ClearFields)
          flag = this.RemoveField((WField) mergeField, true) != -1;
      }
      else if (this.ClearFields)
        flag = this.RemoveField((WField) mergeField, true) != -1;
    }
    this.ClearFields = clearFields;
    return flag;
  }

  private void CopyContent(WordDocument document)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    this.m_contentSections.Clear();
    document.Sections.CloneTo((EntityCollection) this.m_contentSections);
  }

  private void AppendCopiedContent(WordDocument document)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    int index = 0;
    for (int count = this.m_contentSections.Count; index < count; ++index)
    {
      IWSection contentSection = (IWSection) this.m_contentSections[index];
      document.Sections.Add((IWSection) contentSection.Clone());
      this.Document.Settings.DuplicateListStyleNames = string.Empty;
    }
  }

  private Bitmap GetBitmap(object data)
  {
    if (!(data.GetType() == typeof (byte[])))
      return (Bitmap) null;
    MemoryStream memoryStream = new MemoryStream((byte[]) data);
    try
    {
      return new Bitmap((Stream) memoryStream);
    }
    catch
    {
      return (Bitmap) null;
    }
  }

  private void GetFieldNamesForParagraph(
    List<string> fieldsArray,
    TextBodyItem paragraph,
    string groupName)
  {
    switch (paragraph)
    {
      case BlockContentControl _:
        this.GetFiledNamesForSDTBlockItems(fieldsArray, paragraph as BlockContentControl, groupName);
        break;
      case IWTable _:
        WTable wtable = paragraph as WTable;
        int index1 = 0;
        for (int count1 = wtable.Rows.Count; index1 < count1; ++index1)
        {
          WTableRow row = wtable.Rows[index1];
          int index2 = 0;
          for (int count2 = row.Cells.Count; index2 < count2; ++index2)
          {
            WTableCell cell = row.Cells[index2];
            int index3 = 0;
            for (int count3 = cell.ChildEntities.Count; index3 < count3; ++index3)
            {
              TextBodyItem paragraph1 = cell.Items[index3];
              this.GetFieldNamesForParagraph(fieldsArray, paragraph1, groupName);
            }
          }
        }
        break;
      default:
        int index4 = 0;
        for (int count = (paragraph as WParagraph).Items.Count; index4 < count; ++index4)
        {
          ParagraphItem paragraphItem = (paragraph as WParagraph)[index4];
          this.GetFieldNamesForParagraphItems(fieldsArray, paragraphItem, groupName);
        }
        break;
    }
  }

  private void GetFieldNamesForParagraphItems(
    List<string> fieldsArray,
    ParagraphItem item,
    string groupName)
  {
    switch (item)
    {
      case WMergeField field when field.FieldType == FieldType.FieldMergeField:
        if (field.FieldName == groupName)
        {
          if (!this.IsBeginGroupFound && MailMerge.IsBeginGroup(field))
          {
            this.IsBeginGroupFound = true;
            this.IsEndGroupFound = false;
          }
          if (this.IsEndGroupFound || !MailMerge.IsEndGroup(field))
            break;
          this.IsEndGroupFound = true;
          this.IsBeginGroupFound = false;
          break;
        }
        if (groupName != null && (!this.IsBeginGroupFound || this.IsEndGroupFound))
          break;
        fieldsArray.Add(field.FieldName);
        break;
      case WTextBox _:
        WTextBox wtextBox = item as WTextBox;
        int count1 = wtextBox.TextBoxBody.Items.Count;
        for (int index = 0; index < count1; ++index)
          this.GetFieldNamesForParagraph(fieldsArray, wtextBox.TextBoxBody.Items[index], groupName);
        break;
      case Shape _:
        Shape shape = item as Shape;
        int count2 = shape.TextBody.Items.Count;
        for (int index = 0; index < count2; ++index)
          this.GetFieldNamesForParagraph(fieldsArray, shape.TextBody.Items[index], groupName);
        break;
      case InlineContentControl _:
        IEnumerator enumerator = (item as InlineContentControl).ParagraphItems.GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            ParagraphItem current = (ParagraphItem) enumerator.Current;
            this.GetFieldNamesForParagraphItems(fieldsArray, current, groupName);
          }
          break;
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
    }
  }

  private void GetFiledNamesForSDTBlockItems(
    List<string> fieldsArray,
    BlockContentControl structureDocumentTagBlocklockContent,
    string groupName)
  {
    for (int index = 0; index < structureDocumentTagBlocklockContent.TextBody.ChildEntities.Count; ++index)
      this.GetFieldNamesForParagraph(fieldsArray, structureDocumentTagBlocklockContent.TextBody.ChildEntities[index] as TextBodyItem, groupName);
  }

  private static bool IsBeginGroup(WMergeField field)
  {
    string prefix = field.Prefix;
    return prefix == "TableStart" || prefix == "BeginGroup";
  }

  private static bool IsEndGroup(WMergeField field)
  {
    string prefix = field.Prefix;
    return prefix == "TableEnd" || prefix == "EndGroup";
  }

  private bool CheckSelection(IRowsEnumerator rowsEnum)
  {
    if (rowsEnum.RowsCount > 0)
      return true;
    MailMerge.GroupSelector groupSelector = this.m_groupSelector;
    bool clearFields = this.ClearFields;
    if (this.BeforeClearGroupField != null && rowsEnum.RowsCount == 0)
    {
      BeforeClearGroupFieldEventArgs groupFieldEventArgs = this.SendBeforeClearGroup(rowsEnum, groupSelector);
      if (groupFieldEventArgs != null)
        this.ClearFields = groupFieldEventArgs.ClearGroup;
    }
    if (!this.ClearFields && !this.RemoveEmptyParagraphs)
      return true;
    if (groupSelector.GroupSelection != null && this.ClearFields)
      this.RemoveEmptyMergeFieldsInBodyItems(groupSelector.GroupSelection);
    else if (groupSelector.RowSelection != null && this.ClearFields)
    {
      int startRowIndex = groupSelector.RowSelection.StartRowIndex;
      int endRowIndex = groupSelector.RowSelection.EndRowIndex;
      for (int index = startRowIndex; index <= endRowIndex; ++index)
      {
        if (groupSelector.RowSelection.Table.Rows.Count > startRowIndex)
          this.RemoveEmptyMergeFieldsInTableRow(groupSelector.RowSelection.Table.Rows[index]);
      }
    }
    this.ClearFields = clearFields;
    return false;
  }

  private void HideFields(IWSectionCollection sections)
  {
    int index = 0;
    for (int count = sections.Count; index < count; ++index)
      this.ExecuteForSection((IWSection) sections[index], (IRowsEnumerator) null);
  }

  private void RemoveEmptyMergeFieldsInTableRow(WTableRow row)
  {
    int index = 0;
    for (int count = row.Cells.Count; index < count; ++index)
      this.RemoveEmptyMergeFieldsInBodyItems(row.Cells[index].Items);
  }

  private void RemoveEmptyMergeFieldsInBodyItems(TextBodySelection selection)
  {
    if (selection == null)
      return;
    int num = selection.ItemEndIndex + 1;
    for (int itemStartIndex = selection.ItemStartIndex; itemStartIndex < num; ++itemStartIndex)
    {
      TextBodyItem textBodyItem = selection.TextBody.Items[itemStartIndex];
      switch (textBodyItem)
      {
        case WParagraph _:
          WParagraph para = textBodyItem as WParagraph;
          if (this.RemoveEmptyMergeFieldsInParagraph(para) && this.RemoveEmptyParagraphs && num > 1)
          {
            selection.TextBody.Items.Remove((IEntity) para);
            --itemStartIndex;
            --num;
            break;
          }
          break;
        case WTable _:
          WTable wtable = textBodyItem as WTable;
          int index = 0;
          for (int count = wtable.Rows.Count; index < count; ++index)
            this.RemoveEmptyMergeFieldsInTableRow(wtable.Rows[index]);
          break;
      }
    }
  }

  private void RemoveEmptyMergeFieldsInBodyItems(BodyItemCollection items)
  {
    for (int index1 = items.Count - 1; index1 >= 0; --index1)
    {
      TextBodyItem textBodyItem = items[index1];
      switch (textBodyItem)
      {
        case WParagraph _:
          WParagraph para = textBodyItem as WParagraph;
          if (this.RemoveEmptyMergeFieldsInParagraph(para) && this.RemoveEmptyParagraphs && items.Count > 1)
          {
            items.Remove((IEntity) para);
            break;
          }
          break;
        case WTable _:
          WTable wtable = textBodyItem as WTable;
          int index2 = 0;
          for (int count = wtable.Rows.Count; index2 < count; ++index2)
            this.RemoveEmptyMergeFieldsInTableRow(wtable.Rows[index2]);
          break;
      }
    }
  }

  private bool RemoveEmptyMergeFieldsInParagraph(WParagraph para)
  {
    bool flag1 = false;
    int index = 0;
    for (int count = para.Items.Count; index < count; ++index)
    {
      if (para.Items[index] is WField)
      {
        bool flag2 = false;
        WField field = para.Items[index] as WField;
        if (field.FieldType == FieldType.FieldMergeField)
          flag2 = this.RemoveField(field, false) != -1;
        else if (field.FieldType == FieldType.FieldNext)
          flag2 = this.RemoveField(field, false) != -1;
        if (para.Items.Count == 0 && flag2)
        {
          flag1 = true;
          break;
        }
        if (flag2)
        {
          --index;
          count = para.Items.Count;
        }
      }
      else if (para.Items[index] is WTextBox)
        this.RemoveEmptyMergeFieldsInBodyItems((para.Items[index] as WTextBox).TextBoxBody.Items);
      else if (para.Items[index] is Shape)
        this.RemoveEmptyMergeFieldsInBodyItems((para.Items[index] as Shape).TextBody.Items);
    }
    return flag1;
  }

  private bool HideField(WField field, bool hide)
  {
    bool flag = false;
    if (field is WMergeField && (MailMerge.IsBeginGroup(field as WMergeField) || MailMerge.IsEndGroup(field as WMergeField) || hide))
    {
      this.RemoveField(field, false);
      flag = true;
    }
    return flag;
  }

  private string GetMappedColName(string fieldName)
  {
    return this.m_mappedFields != null && this.m_mappedFields.ContainsKey(fieldName) ? this.m_mappedFields[fieldName] : (string) null;
  }

  private void RemoveSpellChecking()
  {
    if (this.Document.GrammarSpellingData == null)
      return;
    this.Document.GrammarSpellingData.PlcfgramData = (byte[]) null;
    this.Document.GrammarSpellingData.PlcfsplData = (byte[]) null;
  }

  internal class GroupSelector
  {
    private TextBodySelection m_groupSelection;
    private MailMerge.TableRowSelection m_rowSelection;
    private WTextBody m_groupTextBody;
    private WTextBody m_body;
    private WMergeField m_beginGroupField;
    private WMergeField m_endGroupField;
    private int m_bodyItemEndIndex;
    private int m_bodyItemStartIndex = -1;
    private int m_paragraphItemEndIndex = -1;
    private int m_paragraphItemStartIndex = -1;
    private int m_rowIndex = -1;
    private int m_startRowIndex = -1;
    private string m_groupName;
    private MailMerge.GroupSelector.GroupFound SendGroupFound;
    private IRowsEnumerator m_rowsEnum;
    private int m_selBodyItemsCnt = -1;
    private byte m_bFlags;
    private List<string> m_fieldNames;

    internal bool InsertAsNewRow
    {
      get => ((int) this.m_bFlags & 1) != 0;
      set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
    }

    internal TextBodySelection GroupSelection => this.m_groupSelection;

    internal MailMerge.TableRowSelection RowSelection => this.m_rowSelection;

    internal WMergeField BeginGroupField => this.m_beginGroupField;

    internal WMergeField EndGroupField
    {
      get => this.m_endGroupField;
      set => this.m_endGroupField = value;
    }

    internal int BodyItemIndex
    {
      get => this.m_bodyItemEndIndex;
      set => this.m_bodyItemEndIndex = value;
    }

    internal bool IsGroupFound => this.m_endGroupField != null;

    internal string GroupName => this.m_groupName;

    internal int SelectedBodyItemsCount => this.m_selBodyItemsCnt;

    internal List<string> FieldNames
    {
      get
      {
        if (this.m_fieldNames == null)
          this.m_fieldNames = new List<string>();
        return this.m_fieldNames;
      }
    }

    internal GroupSelector(MailMerge.GroupSelector.GroupFound onGroupFound, bool insertAsNewRow)
    {
      this.InsertAsNewRow = insertAsNewRow;
      this.SendGroupFound += onGroupFound;
    }

    private void InitProcess(IRowsEnumerator rowsEnum)
    {
      this.m_groupSelection = (TextBodySelection) null;
      this.m_rowSelection = (MailMerge.TableRowSelection) null;
      this.m_beginGroupField = (WMergeField) null;
      this.m_endGroupField = (WMergeField) null;
      this.m_bodyItemEndIndex = 0;
      this.m_bodyItemStartIndex = -1;
      this.m_paragraphItemEndIndex = -1;
      this.m_paragraphItemStartIndex = -1;
      this.m_rowIndex = -1;
      this.m_selBodyItemsCnt = -1;
      this.m_rowsEnum = rowsEnum;
      this.m_groupName = this.m_rowsEnum.TableName;
      if (this.m_fieldNames == null)
        return;
      this.m_fieldNames.Clear();
      this.m_fieldNames = (List<string>) null;
    }

    internal void ProcessGroupsInNested(
      WTextBody body,
      IRowsEnumerator rowsEnum,
      int itemStart,
      int itemEnd)
    {
      this.InitProcess(rowsEnum);
      this.m_groupTextBody = this.m_body = body;
      this.FindInBodyItemsInNested(this.m_body.Items, itemStart, itemEnd);
    }

    internal void ProcessGroups(WTextBody body, IRowsEnumerator rowsEnum)
    {
      this.InitProcess(rowsEnum);
      this.m_groupTextBody = this.m_body = body;
      this.FindInBodyItems(this.m_body.Items);
    }

    internal void ProcessGroups(WTable table, int startRow, int endRow, IRowsEnumerator rowsEnum)
    {
      this.InitProcess(rowsEnum);
      this.FindInTable(table, startRow, endRow);
    }

    private void FindInBodyItemsInNested(BodyItemCollection bodyItems, int itemStart, int itemEnd)
    {
      int count = bodyItems.Count;
      int index1 = itemStart;
      for (int index2 = itemEnd; index1 <= index2; ++index1)
      {
        bool flag = false;
        TextBodyItem bodyItem = bodyItems[index1];
        this.m_bodyItemEndIndex = index1;
        switch (bodyItem.EntityType)
        {
          case EntityType.Paragraph:
            WParagraph wparagraph = (WParagraph) bodyItem;
            for (int index3 = 0; index3 < wparagraph.Items.Count; ++index3)
            {
              ParagraphItem paragraphItem = wparagraph.Items[index3];
              this.m_paragraphItemEndIndex = index3;
              switch (paragraphItem)
              {
                case WTextBox _:
                  this.m_bodyItemEndIndex = 0;
                  this.FindInBodyItems((paragraphItem as WTextBox).TextBoxBody.Items);
                  this.m_bodyItemEndIndex = index1;
                  break;
                case Shape _:
                  this.m_bodyItemEndIndex = 0;
                  this.FindInBodyItems((paragraphItem as Shape).TextBody.Items);
                  this.m_bodyItemEndIndex = index1;
                  break;
                default:
                  this.CheckItem(paragraphItem);
                  break;
              }
              if (this.IsGroupFound)
              {
                if (this.m_groupSelection != null)
                {
                  index1 = this.m_groupSelection.ItemEndIndex;
                  if (this.m_groupSelection.ItemStartIndex == this.m_groupSelection.ItemEndIndex)
                    index3 = this.m_groupSelection.ParagraphItemEndIndex;
                  index2 += bodyItems.Count - count;
                  flag = true;
                  this.ClearSelection();
                }
                else
                {
                  if (this.m_rowSelection.StartRowIndex == this.m_rowSelection.EndRowIndex)
                  {
                    index2 += bodyItems.Count - count;
                    break;
                  }
                  break;
                }
              }
            }
            goto case EntityType.AlternateChunk;
          case EntityType.AlternateChunk:
            if (flag)
              return;
            continue;
          case EntityType.BlockContentControl:
            WTextBody textBody = ((BlockContentControl) bodyItem).TextBody;
            this.FindInBodyItemsInNested(textBody.Items, textBody.Items.FirstItem.Index, textBody.Items.LastItem.Index);
            goto case EntityType.AlternateChunk;
          case EntityType.Table:
            WTable table = (WTable) bodyItem;
            this.FindInTable(table, 0, table.Rows.Count - 1);
            goto case EntityType.AlternateChunk;
          default:
            throw new Exception();
        }
      }
    }

    private void FindInBodyItems(BodyItemCollection bodyItems)
    {
      int index1 = 0;
      for (int count = bodyItems.Count; index1 < count; ++index1)
      {
        TextBodyItem bodyItem = bodyItems[index1];
        this.m_bodyItemEndIndex = index1;
        switch (bodyItem.EntityType)
        {
          case EntityType.Paragraph:
            WParagraph wparagraph = (WParagraph) bodyItem;
            for (int index2 = 0; index2 < wparagraph.Items.Count; ++index2)
            {
              ParagraphItem paragraphItem = wparagraph.Items[index2];
              this.m_paragraphItemEndIndex = index2;
              switch (paragraphItem)
              {
                case WTextBox _:
                  this.m_bodyItemEndIndex = 0;
                  this.FindInBodyItems((paragraphItem as WTextBox).TextBoxBody.Items);
                  this.m_bodyItemEndIndex = index1;
                  break;
                case Shape _:
                  this.m_bodyItemEndIndex = 0;
                  this.FindInBodyItems((paragraphItem as Shape).TextBody.Items);
                  this.m_bodyItemEndIndex = index1;
                  break;
                default:
                  this.CheckItem(paragraphItem);
                  break;
              }
              if (this.IsGroupFound)
              {
                if (this.m_groupSelection != null)
                {
                  index1 = this.m_groupSelection.ItemEndIndex;
                  if (this.m_groupSelection.ItemStartIndex == this.m_groupSelection.ItemEndIndex)
                    index2 = this.m_groupSelection.ParagraphItemEndIndex;
                  count = bodyItems.Count;
                  this.ClearSelection();
                }
                else
                {
                  if (this.m_rowSelection.StartRowIndex == this.m_rowSelection.EndRowIndex)
                  {
                    count = bodyItems.Count;
                    break;
                  }
                  break;
                }
              }
            }
            continue;
          case EntityType.AlternateChunk:
            continue;
          case EntityType.BlockContentControl:
            this.FindInBodyItems(((BlockContentControl) bodyItem).TextBody.Items);
            continue;
          case EntityType.Table:
            WTable table = (WTable) bodyItem;
            this.FindInTable(table, 0, table.Rows.Count - 1);
            continue;
          default:
            throw new Exception();
        }
      }
    }

    private void FindInTable(WTable table, int startRow, int endRow)
    {
      int count1 = table.Rows.Count;
      for (int index1 = startRow; index1 <= endRow; ++index1)
      {
        WTableRow row = table.Rows[index1];
        this.m_rowIndex = index1;
        int index2 = 0;
        for (int count2 = row.Cells.Count; index2 < count2; ++index2)
        {
          this.FindInBodyItems(row.Cells[index2].Items);
          if (this.IsGroupFound)
          {
            endRow += table.Rows.Count - count1;
            count1 = table.Rows.Count;
            index1 = this.m_rowSelection.StartRowIndex;
            this.ClearSelection();
            break;
          }
        }
      }
    }

    private void ClearSelection()
    {
      this.m_groupSelection = (TextBodySelection) null;
      this.m_rowSelection = (MailMerge.TableRowSelection) null;
      this.m_beginGroupField = (WMergeField) null;
      this.m_endGroupField = (WMergeField) null;
      if (this.m_fieldNames == null)
        return;
      this.m_fieldNames.Clear();
      this.m_fieldNames = (List<string>) null;
    }

    private void CheckItem(ParagraphItem item)
    {
      if (item.EntityType != EntityType.MergeField)
        return;
      WMergeField field = item as WMergeField;
      if (this.m_beginGroupField != null)
      {
        if (field.Prefix != string.Empty)
        {
          if (field.FieldName != this.m_groupName)
            this.FieldNames.Add($"{field.Prefix}:{field.FieldName}");
        }
        else
          this.FieldNames.Add(field.FieldName);
      }
      if (!(field.FieldName == this.m_groupName))
        return;
      if (this.m_beginGroupField == null)
      {
        if (!MailMerge.IsBeginGroup(field))
          return;
        this.StartSelection(field);
      }
      else
      {
        if (!MailMerge.IsEndGroup(field))
          return;
        this.EndSelection(field);
        if (this.SendGroupFound == null)
          return;
        this.SendGroupFound(this.m_rowsEnum);
      }
    }

    private void StartSelection(WMergeField field)
    {
      this.m_beginGroupField = field;
      this.m_groupTextBody = field.OwnerParagraph.OwnerTextBody;
      this.m_bodyItemStartIndex = this.m_bodyItemEndIndex;
      this.m_paragraphItemStartIndex = this.m_paragraphItemEndIndex;
      this.m_startRowIndex = this.m_rowIndex;
    }

    private bool IsNeedToInsertAsNewRow(WTextBody textBody)
    {
      return this.InsertAsNewRow && textBody.EntityType == EntityType.TableCell && textBody == this.m_groupTextBody && this.IsRowContainsSingleCell(textBody as WTableCell, (textBody as WTableCell).OwnerRow);
    }

    private bool IsRowContainsSingleCell(WTableCell cell, WTableRow ownerRow)
    {
      if (ownerRow.Cells.Count == 1)
        return true;
      if (cell.Index != 0 || cell.CellFormat.HorizontalMerge != CellMerge.Start)
        return false;
      for (int index = 1; index < ownerRow.Cells.Count; ++index)
      {
        if (ownerRow.Cells[index].CellFormat.HorizontalMerge != CellMerge.Continue)
          return false;
      }
      return true;
    }

    private void EndSelection(WMergeField field)
    {
      this.m_endGroupField = field;
      WTextBody ownerTextBody = field.OwnerParagraph.OwnerTextBody;
      if (field.FieldEnd != null)
      {
        this.m_bodyItemEndIndex = field.FieldEnd.OwnerParagraph.GetIndexInOwnerCollection();
        this.m_paragraphItemEndIndex = field.FieldEnd.GetIndexInOwnerCollection();
      }
      this.m_selBodyItemsCnt = this.m_bodyItemEndIndex - this.m_bodyItemStartIndex + 1;
      if (this.IsNeedToInsertAsNewRow(ownerTextBody))
      {
        this.m_rowIndex = (ownerTextBody as WTableCell).OwnerRow.Index;
        this.m_rowSelection = new MailMerge.TableRowSelection(ownerTextBody.Owner.Owner as WTable, this.m_startRowIndex, this.m_rowIndex);
      }
      else if (ownerTextBody == this.m_groupTextBody)
      {
        this.m_groupSelection = new TextBodySelection((ITextBody) ownerTextBody, this.m_bodyItemStartIndex, this.m_bodyItemEndIndex, this.m_paragraphItemStartIndex, this.m_paragraphItemEndIndex);
      }
      else
      {
        if (ownerTextBody.EntityType != EntityType.TableCell || this.m_groupTextBody.EntityType != EntityType.TableCell || (this.m_groupTextBody.Owner as WTableRow).OwnerTable != (ownerTextBody.Owner as WTableRow).OwnerTable)
          throw new MailMergeException();
        this.m_rowIndex = (ownerTextBody as WTableCell).OwnerRow.GetRowIndex();
        this.UpdateEndSelection(ownerTextBody as WTableCell);
        this.m_rowSelection = new MailMerge.TableRowSelection(ownerTextBody.Owner.Owner as WTable, this.m_startRowIndex, this.m_rowIndex);
      }
    }

    private void UpdateEndSelection(WTableCell cell)
    {
      WTableRow wtableRow = cell.OwnerRow;
      bool flag1 = false;
      foreach (WTableCell cell1 in (CollectionImpl) wtableRow.Cells)
      {
        if (cell.CellFormat.VerticalMerge != CellMerge.None)
        {
          flag1 = true;
          break;
        }
      }
      if (!flag1)
        return;
      while (wtableRow.NextSibling != null)
      {
        wtableRow = wtableRow.NextSibling as WTableRow;
        bool flag2 = false;
        foreach (WTableCell cell2 in (CollectionImpl) wtableRow.Cells)
        {
          if (cell.CellFormat.VerticalMerge != CellMerge.None)
          {
            flag2 = true;
            break;
          }
        }
        if (!flag2)
          break;
        ++this.m_rowIndex;
      }
    }

    internal delegate void GroupFound(IRowsEnumerator rowsEnum);
  }

  internal class TableRowSelection
  {
    internal WTable Table;
    internal int StartRowIndex;
    internal int EndRowIndex;

    internal TableRowSelection(WTable table, int startRowIndex, int endRowIndex)
    {
      this.Table = table;
      this.StartRowIndex = startRowIndex;
      this.EndRowIndex = endRowIndex;
      this.ValidateIndexes();
    }

    private void ValidateIndexes()
    {
      if (this.StartRowIndex < 0 || this.StartRowIndex >= this.Table.Rows.Count)
        throw new ArgumentOutOfRangeException("StartRowIndex");
      if (this.EndRowIndex < 0 || this.EndRowIndex >= this.Table.Rows.Count)
        throw new ArgumentOutOfRangeException("EndRowIndex");
    }
  }
}
