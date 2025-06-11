// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TemplateMarkers.TemplateMarkersImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Microsoft.CSharp.RuntimeBinder;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.PivotTables;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.TemplateMarkers;

public class TemplateMarkersImpl : CommonObject, ITemplateMarkersProcessor
{
  private const string DEF_MARKER_PREFIX = "%";
  private const string DEF_PARTS_SEPARATOR = ".";
  private const string DEF_NULL_VALUE = "";
  private const char DEF_STANDARD_SEPARATOR = ';';
  private const int DEF_IMAGE_SIZE_ARG_INDEX = 4;
  private const int DEF_IMAGE_POSITION_ARG_INDEX = 5;
  private int m_insertCount;
  private int DEF_FIT_TO_CELL_ARG_INDEX = 6;
  private bool isNestobjectFirstrow;
  private static readonly List<MarkerArgument> s_arrArguments = new List<MarkerArgument>();
  private static readonly Type[] DEF_PARENT_TYPES = new Type[2]
  {
    typeof (WorksheetImpl),
    typeof (WorkbookImpl)
  };
  private Dictionary<long, string> dictMarkers = new Dictionary<long, string>();
  private Dictionary<string, object> m_dicVariables = new Dictionary<string, object>();
  private string m_strMarkerPrefix = "%";
  private char m_chSeparator = ';';
  private WorkbookImpl m_book;
  private Dictionary<string, VariableTypeAction> m_variableTypeActions = new Dictionary<string, VariableTypeAction>();
  private Dictionary<string, CondFormatCollectionWrapper> m_conditionalFormats;
  private Dictionary<string, CallSite<Func<CallSite, object, object>>> m_callSiteCollection;
  private Dictionary<int, InsertCopiedCellsInfo> InsertCopiedCellsList;

  static TemplateMarkersImpl()
  {
    TemplateMarkersImpl.s_arrArguments.Add((MarkerArgument) new CopyRangeArgument());
    TemplateMarkersImpl.s_arrArguments.Add((MarkerArgument) new DirectionArgument());
    TemplateMarkersImpl.s_arrArguments.Add((MarkerArgument) new JumpArgument());
    TemplateMarkersImpl.s_arrArguments.Add((MarkerArgument) new NewSpaceArgument());
    TemplateMarkersImpl.s_arrArguments.Add((MarkerArgument) new ImageSizeArgument());
    TemplateMarkersImpl.s_arrArguments.Add((MarkerArgument) new ImagePositionArgument());
    TemplateMarkersImpl.s_arrArguments.Add((MarkerArgument) new FitToCellArgument());
    TemplateMarkersImpl.s_arrArguments.Add((MarkerArgument) new ImportOptionsArgument());
    TemplateMarkersImpl.s_arrArguments.Add((MarkerArgument) new ImportGroupOptionsArgument());
  }

  public TemplateMarkersImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("Cannot find parent workbook.");
  }

  public void ApplyMarkers() => this.ApplyMarkers(UnknownVariableAction.Exception);

  public void ApplyMarkers(UnknownVariableAction action)
  {
    object parent = this.FindParent(TemplateMarkersImpl.DEF_PARENT_TYPES);
    IWorksheet sheet = parent != null ? parent as IWorksheet : throw new ArgumentOutOfRangeException("Can't find parent workbook or worksheet");
    this.m_book.InnerFormats.FillFormatIndexes();
    if (sheet != null)
      this.ApplyMarkers(sheet, action);
    else
      this.ApplyMarkers((IWorkbook) parent, action);
  }

  public void AddVariable(string strName, object variable)
  {
    VariableTypeAction makerType = VariableTypeAction.None;
    if (this.IsDataColumn(variable) || this.IsDataTable(variable) || this.IsDataView(variable) || this.IsDataSet(variable))
      makerType = VariableTypeAction.DetectDataType;
    this.AddVariable(strName, variable, makerType);
  }

  public void AddVariable(string strName, object variable, VariableTypeAction makerType)
  {
    switch (strName)
    {
      case null:
        throw new ArgumentNullException(nameof (strName));
      case "":
        throw new ArgumentException("strName - string cannot be empty.");
      default:
        if (variable == null)
          throw new ArgumentNullException(nameof (variable));
        this.m_variableTypeActions.Add(strName, makerType);
        this.m_dicVariables.Add(strName, variable);
        break;
    }
  }

  public void RemoveVariable(string strName)
  {
    this.m_variableTypeActions.Remove(strName);
    this.m_dicVariables.Remove(strName);
  }

  public void ApplyMarkers(IWorksheet sheet)
  {
    this.ApplyMarkers(sheet, UnknownVariableAction.Exception);
  }

  public void ApplyMarkers(IWorksheet sheet, UnknownVariableAction action)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    List<int> arrLabels = ((WorkbookImpl) sheet.Workbook).InnerSST.StartWith(this.m_strMarkerPrefix);
    this.ApplyMarkers(sheet, arrLabels, action);
    if (this.m_callSiteCollection == null)
      return;
    this.m_callSiteCollection.Clear();
    this.m_callSiteCollection = (Dictionary<string, CallSite<Func<CallSite, object, object>>>) null;
  }

  public void ApplyMarkers(IWorkbook book)
  {
    this.ApplyMarkers(book, UnknownVariableAction.Exception);
  }

  public void ApplyMarkers(IWorkbook book, UnknownVariableAction action)
  {
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    List<int> arrLabels = ((WorkbookImpl) book).InnerSST.StartWith(this.m_strMarkerPrefix);
    IWorksheets worksheets = book.Worksheets;
    int Index = 0;
    for (int count = worksheets.Count; Index < count; ++Index)
      this.ApplyMarkers(worksheets[Index], arrLabels, action);
    if (this.m_callSiteCollection == null)
      return;
    this.m_callSiteCollection.Clear();
    this.m_callSiteCollection = (Dictionary<string, CallSite<Func<CallSite, object, object>>>) null;
  }

  public bool ContainsVariable(string strName) => this.m_dicVariables.ContainsKey(strName);

  public IConditionalFormats CreateConditionalFormats(IRange range)
  {
    if (this.m_conditionalFormats == null)
      this.m_conditionalFormats = new Dictionary<string, CondFormatCollectionWrapper>();
    CondFormatCollectionWrapper conditionalFormats;
    if (this.m_conditionalFormats.ContainsKey(range.AddressGlobal))
    {
      conditionalFormats = this.m_conditionalFormats[range.AddressGlobal];
    }
    else
    {
      conditionalFormats = range.ConditionalFormats as CondFormatCollectionWrapper;
      this.m_conditionalFormats.Add(range.AddressGlobal, conditionalFormats);
    }
    return (IConditionalFormats) conditionalFormats;
  }

  private void ApplyMarkers(IWorksheet sheet, List<int> arrLabels, UnknownVariableAction action)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    IList<long> arrCells = arrLabels != null ? this.PrepareCellIndexes(sheet, arrLabels) : throw new ArgumentNullException(nameof (arrLabels));
    for (int index = 0; index < arrCells.Count; ++index)
    {
      string stringValue = (sheet as WorksheetImpl).GetStringValue(arrCells[index]);
      if (stringValue != null)
        this.dictMarkers.Add(arrCells[index], stringValue);
    }
    WorksheetImpl worksheetImpl = sheet as WorksheetImpl;
    worksheetImpl.ImportMergeRanges = new List<Rectangle>();
    worksheetImpl.ImportMergeRangeCollection = sheet.CreateRangesCollection();
    this.InsertCopiedCellsList = (Dictionary<int, InsertCopiedCellsInfo>) null;
    if (sheet.PivotTables.Count > 0)
    {
      List<long> longList = new List<long>();
      foreach (long index1 in (IEnumerable<long>) arrCells)
      {
        int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(index1);
        int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(index1);
        for (int index2 = 0; index2 < sheet.PivotTables.Count; ++index2)
        {
          PivotTableImpl pivotTable = sheet.PivotTables[index2] as PivotTableImpl;
          if (rowFromCellIndex < pivotTable.Location.Row || rowFromCellIndex > pivotTable.Location.LastRow || columnFromCellIndex < pivotTable.Location.Column || columnFromCellIndex > pivotTable.Location.LastColumn)
            longList.Add(index1);
        }
      }
      arrCells = (IList<long>) longList;
    }
    int count = arrCells.Count;
    List<RangeBuilder> rangeBuilderList = new List<RangeBuilder>(count);
    IMigrantRange migrantRange = (IMigrantRange) new MigrantRangeImpl(this.Application, sheet);
    if (this.CheckClassMarker(sheet, action, arrCells, migrantRange))
    {
      arrLabels = ((WorkbookImpl) sheet.Workbook).InnerSST.StartWith(this.m_strMarkerPrefix);
      arrCells = this.PrepareCellIndexes(sheet, arrLabels);
      count = arrCells.Count;
      rangeBuilderList = new List<RangeBuilder>(count);
      migrantRange = (IMigrantRange) new MigrantRangeImpl(this.Application, sheet);
    }
    bool isMergeEnabled = false;
    for (int i = 0; i < count; ++i)
      rangeBuilderList.Add(this.ApplyMarker(sheet, arrCells, i, migrantRange, action, ref isMergeEnabled));
    if (worksheetImpl.ImportMergeRanges != null && worksheetImpl.ImportMergeRanges.Count > 0)
    {
      foreach (Rectangle importMergeRange in worksheetImpl.ImportMergeRanges)
        (sheet as WorksheetImpl).MergeCells.MergedRegions.Add(importMergeRange);
    }
    if (worksheetImpl.ImportMergeRangeCollection != null && worksheetImpl.ImportMergeRangeCollection.Count > 0)
      worksheetImpl.ImportMergeRangeCollection.CellStyle.VerticalAlignment = ExcelVAlign.VAlignTop;
    this.ApplyConditionalFormats(rangeBuilderList, sheet);
    if (count > 0)
    {
      this.UpdateChartRanges(sheet.Workbook, sheet, arrCells, (IList<RangeBuilder>) rangeBuilderList);
      this.UpdateTableRange(sheet, (IList<RangeBuilder>) rangeBuilderList);
    }
    if (this.InsertCopiedCellsList != null)
    {
      this.InsertCopiedCellsList.Clear();
      this.InsertCopiedCellsList = (Dictionary<int, InsertCopiedCellsInfo>) null;
    }
    if (this.dictMarkers.Count > 0)
      this.dictMarkers.Clear();
    if (worksheetImpl.ImportMergeRanges.Count > 0)
    {
      worksheetImpl.ImportMergeRanges.Clear();
      worksheetImpl.ImportMergeRanges = (List<Rectangle>) null;
    }
    if (worksheetImpl.ImportMergeRangeCollection.Count > 0)
      worksheetImpl.ImportMergeRangeCollection = (IRanges) null;
    MarkerArgument.MaxRowIndex = -1;
  }

  public bool CheckClassMarker(
    IWorksheet sheet,
    UnknownVariableAction action,
    IList<long> arrCells,
    IMigrantRange migrantRange)
  {
    Dictionary<string, object>.Enumerator enumerator1 = this.m_dicVariables.GetEnumerator();
    bool flag = false;
label_16:
    while (enumerator1.MoveNext())
    {
      object obj1 = enumerator1.Current.Value;
      if (obj1 != null)
      {
        object current;
        if (this.IsArray(obj1))
        {
          IList list = (IList) obj1;
          if (list.Count != 0)
            current = list[0];
          else
            continue;
        }
        else
        {
          if (!this.IsCollection(obj1))
            return flag;
          IEnumerator enumerator2 = ((IEnumerable) obj1).GetEnumerator();
          if (enumerator2.MoveNext())
            current = enumerator2.Current;
          else
            continue;
        }
        if (this.IsDynamic(current))
        {
          foreach (object obj2 in enumerator1.Current.Value as IEnumerable)
          {
            flag = this.TrySetHeaders(obj2, enumerator1.Current.Key, true, sheet, action, arrCells, migrantRange);
            if (flag)
              goto label_16;
          }
          return false;
        }
        if (current.GetType().Namespace == null || current.GetType().Namespace != null && !current.GetType().Namespace.Contains("System"))
          flag = this.CheckAndApplyHeaders(current, sheet, action, arrCells, migrantRange);
      }
    }
    return flag;
  }

  public bool CheckAndApplyHeaders(
    object obj,
    IWorksheet sheet,
    UnknownVariableAction action,
    IList<long> arrCells,
    IMigrantRange migrantRange)
  {
    string name = obj.GetType().Name;
    return this.TrySetHeaders(obj, name, false, sheet, action, arrCells, migrantRange);
  }

  private bool TrySetHeaders(
    object obj,
    string className,
    bool isDynamic,
    IWorksheet sheet,
    UnknownVariableAction action,
    IList<long> arrCells,
    IMigrantRange migrantRange)
  {
    bool flag = false;
    foreach (long arrCell in (IEnumerable<long>) arrCells)
    {
      string stringValue = (sheet as WorksheetImpl).GetStringValue(arrCell);
      if (stringValue.Contains(className) && stringValue.Split(".".ToCharArray()).Length == 1)
      {
        string strArguments = (string) null;
        string strMarkerText = stringValue;
        this.GetVariableName(ref stringValue, out strArguments);
        MarkerOptionsImpl options;
        IList arguments = this.ParseArguments(strArguments, out options);
        migrantRange = (IMigrantRange) new MigrantRangeImpl(this.Application, sheet);
        int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(arrCell);
        int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(arrCell);
        migrantRange.ResetRowColumn(rowFromCellIndex, columnFromCellIndex);
        flag = this.SetObjectHeader(obj, strMarkerText, sheet, migrantRange, arrCells, arguments, options, action, isDynamic);
      }
    }
    return flag;
  }

  private bool SetObjectHeader(
    object obj,
    string strMarkerText,
    IWorksheet sheet,
    IMigrantRange migrantRange,
    IList<long> arrMarkerCells,
    IList lstArguments,
    MarkerOptionsImpl options,
    UnknownVariableAction action,
    bool isDynamic)
  {
    IList columnNames = (IList) null;
    IList list = (IList) null;
    if (!isDynamic)
    {
      list = this.GetObjectMembersInfo(obj, out columnNames);
    }
    else
    {
      Type type1 = obj.GetType();
      if (type1.FullName.Contains("System.Dynamic.ExpandoObject"))
        columnNames = (IList) new List<string>((IEnumerable<string>) (obj as IDictionary<string, object>).Keys);
      else if (obj is DynamicObject && this.AppImplementation.HasDynamicOverrideMethods(type1))
      {
        object obj1 = obj;
        // ISSUE: reference to a compiler-generated field
        if (TemplateMarkersImpl.\u003CSetObjectHeader\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TemplateMarkersImpl.\u003CSetObjectHeader\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 = CallSite<Func<CallSite, Type, object, List<string>>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (TemplateMarkersImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, List<string>> target = TemplateMarkersImpl.\u003CSetObjectHeader\u003Eo__SiteContainer0.\u003C\u003Ep__Site1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, List<string>>> pSite1 = TemplateMarkersImpl.\u003CSetObjectHeader\u003Eo__SiteContainer0.\u003C\u003Ep__Site1;
        Type type2 = typeof (List<string>);
        // ISSUE: reference to a compiler-generated field
        if (TemplateMarkersImpl.\u003CSetObjectHeader\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TemplateMarkersImpl.\u003CSetObjectHeader\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "GetDynamicMemberNames", (IEnumerable<Type>) null, typeof (TemplateMarkersImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = TemplateMarkersImpl.\u003CSetObjectHeader\u003Eo__SiteContainer0.\u003C\u003Ep__Site2.Target((CallSite) TemplateMarkersImpl.\u003CSetObjectHeader\u003Eo__SiteContainer0.\u003C\u003Ep__Site2, obj1);
        columnNames = (IList) target((CallSite) pSite1, type2, obj2);
      }
      if ((columnNames == null ? 1 : (columnNames.Count == 0 ? 1 : 0)) != 0)
        return false;
    }
    int insertCount = isDynamic ? columnNames.Count : list.Count;
    int row = migrantRange.Row;
    int column = migrantRange.Column;
    int iRow = row;
    bool flag = this.ArrangeRowsOrColumns(sheet, migrantRange, options, insertCount);
    int startIndex = strMarkerText.IndexOf(';');
    if (startIndex == -1)
      startIndex = strMarkerText.Length;
    for (int index = 0; index < insertCount; ++index)
    {
      string str1 = columnNames[index].ToString();
      string str2 = isDynamic ? str1 : list[index].ToString();
      if (index != 0)
      {
        string[] strArray = strMarkerText.Split(';');
        string direction = new DirectionArgument().TryGetDirection(strMarkerText);
        strMarkerText = direction == null ? strArray[0] : strArray[0] + (object) ';' + direction;
      }
      string str3 = strMarkerText.Insert(startIndex, "." + str2);
      if (flag)
      {
        sheet.SetValue(iRow, column, str1);
        sheet.SetValue(iRow, column + 1, str3);
        ++iRow;
      }
      else
      {
        sheet.SetValue(iRow, column, str1);
        sheet.SetValue(iRow + 1, column, str3);
        ++column;
      }
    }
    return true;
  }

  private bool ArrangeRowsOrColumns(
    IWorksheet sheet,
    IMigrantRange migrantRange,
    MarkerOptionsImpl options,
    int insertCount)
  {
    bool flag;
    if (options.Direction == MarkerDirection.Vertical)
    {
      flag = false;
      sheet.InsertColumn(migrantRange.Column, insertCount - 1, ExcelInsertOptions.FormatAsAfter);
      sheet.InsertRow(migrantRange.Row + 1);
    }
    else
    {
      flag = true;
      sheet.InsertRow(migrantRange.Row, insertCount - 1, ExcelInsertOptions.FormatAsAfter);
      sheet.InsertColumn(migrantRange.Column + 1);
    }
    return flag;
  }

  private IList GetObjectMembersInfo(object obj, out IList columnNames)
  {
    Type type = obj.GetType();
    IList objectMembersInfo = (IList) new List<string>();
    columnNames = (IList) new List<string>();
    foreach (PropertyInfo property in type.GetProperties())
    {
      string str = property.Name;
      object[] customAttributes = property.GetCustomAttributes(true);
      if (customAttributes != null && customAttributes.Length > 0)
      {
        TemplateMarkerAttributes markerAttributes = customAttributes[0] as TemplateMarkerAttributes;
        if (!markerAttributes.Exclude)
        {
          if (markerAttributes.HeaderName != null)
            str = markerAttributes.HeaderName;
        }
        else
          continue;
      }
      columnNames.Add((object) str);
      objectMembersInfo.Add((object) property.Name);
    }
    return objectMembersInfo;
  }

  private void ApplyConditionalFormats(List<RangeBuilder> ranges, IWorksheet sheet)
  {
    if (this.m_conditionalFormats == null || ranges.Count == 0)
      return;
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    bool flag1 = false;
    int num = 0;
    bool flag2 = false;
    foreach (KeyValuePair<string, CondFormatCollectionWrapper> conditionalFormat in this.m_conditionalFormats)
    {
      RangesOperations rangesOperations = new RangesOperations();
      IRange range1 = conditionalFormat.Value.Range;
      Rectangle rectangeOfRange = RangeImpl.GetRectangeOfRange(range1, true);
      rangesOperations.AddRectangles((IList<Rectangle>) new Rectangle[1]
      {
        rectangeOfRange
      });
      if (range1.Worksheet.Name.Equals(sheet.Name))
      {
        foreach (RangeBuilder range2 in ranges)
        {
          if (range2.Count == 0)
          {
            flag2 = true;
          }
          else
          {
            Rectangle range3 = range2[0];
            int width = range3.Width;
            int height = range3.Height;
            flag1 = width == 0;
            range3.Width = 0;
            range3.Height = 0;
            if (rangesOperations.Contains(range3))
            {
              num = !flag1 ? width : height;
              flag2 = true;
              break;
            }
          }
        }
        if (!flag2)
          throw new ArgumentException("The specified conditional format range is an invalid Template marker range.");
        Rectangle cellRectangle = conditionalFormat.Value.ConditionalFormats.CellRectangles[0];
        if (flag1)
          cellRectangle.Height += num;
        else
          cellRectangle.Width += num;
        conditionalFormat.Value.ConditionalFormats.AddRange(cellRectangle);
        flag2 = false;
      }
    }
  }

  private void UpdateChartRanges(
    IWorkbook book,
    IWorksheet sheet,
    IList<long> arrCells,
    IList<RangeBuilder> arrResultRanges)
  {
    if (arrCells == null)
      throw new ArgumentNullException(nameof (arrCells));
    if (arrResultRanges == null)
      throw new ArgumentNullException(nameof (arrResultRanges));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    int index = 0;
    for (int count = arrCells.Count; index < count; ++index)
    {
      long arrCell = arrCells[index];
      RangeBuilder arrResultRange = arrResultRanges[index];
      this.UpdateChartRanges(book, sheet, arrCell, arrResultRange);
    }
  }

  private void UpdateChartRanges(
    IWorkbook book,
    IWorksheet sheet,
    long cellIndex,
    RangeBuilder builder)
  {
    IWorksheets worksheets = book.Worksheets;
    int Index = 0;
    for (int count = worksheets.Count; Index < count; ++Index)
      this.UpdateSheetCharts(worksheets[Index], sheet, cellIndex, builder);
    ICharts charts = book.Charts;
    int index = 0;
    for (int count = charts.Count; index < count; ++index)
      this.UpdateChartRanges(charts[index], sheet, cellIndex, builder);
  }

  private void UpdateSheetCharts(
    IWorksheet sheet,
    IWorksheet sheetCell,
    long cellIndex,
    RangeBuilder builder)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (builder == null)
      throw new ArgumentNullException(nameof (builder));
    if (sheetCell == null)
      throw new ArgumentNullException(nameof (sheetCell));
    IChartShapes charts = sheet.Charts;
    int index = 0;
    for (int count = charts.Count; index < count; ++index)
      this.UpdateChartRanges((IChart) charts[index], sheetCell, cellIndex, builder);
  }

  private void UpdateTableRange(IWorksheet sheet, IList<RangeBuilder> arrResultRanges)
  {
    if (arrResultRanges == null)
      throw new ArgumentNullException(nameof (arrResultRanges));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    for (int index = 0; index < arrResultRanges.Count; ++index)
    {
      IRange range = arrResultRanges[index].ToRange(sheet);
      if (range != null)
      {
        foreach (IListObject listObject in (IEnumerable<IListObject>) sheet.ListObjects)
        {
          if (range.Row <= listObject.Location.LastRow && range.LastRow > listObject.Location.LastRow)
            listObject.Location = listObject.Location.Worksheet[listObject.Location.Row, listObject.Location.Column, range.LastRow, listObject.Location.LastColumn];
          if (range.Column <= listObject.Location.LastColumn && range.LastColumn > listObject.Location.LastColumn)
            listObject.Location = listObject.Location.Worksheet[listObject.Location.Row, listObject.Location.Column, listObject.Location.LastRow, range.LastColumn];
        }
      }
    }
  }

  private void UpdateChartRanges(
    IChart chart,
    IWorksheet sheet,
    long cellIndex,
    RangeBuilder builder)
  {
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (builder == null)
      throw new ArgumentNullException(nameof (builder));
    IChartSeries series = chart.Series;
    int index = 0;
    for (int count = series.Count; index < count; ++index)
    {
      IChartSerie chartSerie = series[index];
      if (this.CompareRange(chartSerie.Values, sheet, cellIndex))
        chartSerie.Values = builder.ToRange(sheet);
      if (this.CompareRange(chartSerie.CategoryLabels, sheet, cellIndex))
        chartSerie.CategoryLabels = builder.ToRange(sheet);
      if (this.CompareRange(chartSerie.Bubbles, sheet, cellIndex))
        chartSerie.Bubbles = builder.ToRange(sheet);
    }
  }

  private bool CompareRange(IRange range, IWorksheet sheet, long cellIndex)
  {
    if (range == null)
      return false;
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(cellIndex);
    int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(cellIndex);
    return range.Row == rowFromCellIndex && range.LastRow == rowFromCellIndex && range.Column == columnFromCellIndex && range.LastColumn == columnFromCellIndex;
  }

  private IList<long> PrepareCellIndexes(IWorksheet sheet, List<int> arrLabels)
  {
    CellRecordCollection recordCollection = sheet != null ? ((WorksheetImpl) sheet).CellRecords : throw new ArgumentNullException(nameof (sheet));
    List<long> longList = new List<long>();
    int count = arrLabels.Count;
    foreach (DictionaryEntry dictionaryEntry in recordCollection)
    {
      BiffRecordRaw biffRecordRaw = (BiffRecordRaw) dictionaryEntry.Value;
      long key = (long) dictionaryEntry.Key;
      switch (biffRecordRaw.TypeCode)
      {
        case TBIFFRecord.RString:
        case TBIFFRecord.Label:
          if (((IStringValue) biffRecordRaw).StringValue.StartsWith(this.m_strMarkerPrefix))
          {
            longList.Add(key);
            continue;
          }
          continue;
        case TBIFFRecord.LabelSST:
          if (count > 0)
          {
            int sstIndex = ((LabelSSTRecord) biffRecordRaw).SSTIndex;
            if (arrLabels.BinarySearch(sstIndex) >= 0)
            {
              longList.Add(key);
              continue;
            }
            continue;
          }
          continue;
        default:
          continue;
      }
    }
    foreach (long index in longList)
    {
      int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(index);
      if (rowFromCellIndex > MarkerArgument.MaxRowIndex)
        MarkerArgument.MaxRowIndex = rowFromCellIndex;
    }
    return (IList<long>) longList;
  }

  private RangeBuilder ApplyMarker(
    IWorksheet sheet,
    IList<long> arrCells,
    int i,
    IMigrantRange migrantRange,
    UnknownVariableAction action,
    ref bool isMergeEnabled)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (arrCells == null)
      throw new ArgumentNullException(nameof (arrCells));
    if (migrantRange == null)
      throw new ArgumentNullException(nameof (migrantRange));
    int count = arrCells.Count;
    if (i < 0 || i > count - 1)
      throw new ArgumentOutOfRangeException(nameof (i), "Value cannot be less than 0 and greater than iCount - 1.");
    WorksheetImpl worksheetImpl = (WorksheetImpl) sheet;
    long arrCell = arrCells[i];
    string stringValue = worksheetImpl.GetStringValue(arrCell);
    string str1 = stringValue;
    int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(arrCell);
    int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(arrCell);
    string strArguments;
    string str2 = stringValue != null && (stringValue == null || stringValue.Trim().Length != 0) ? this.GetVariableName(ref stringValue, out strArguments).Trim() : throw new ArgumentNullException($"Cannot find the variable in the cell [{(object) rowFromCellIndex},{(object) columnFromCellIndex}]");
    RangeBuilder builder = new RangeBuilder();
    migrantRange.ResetRowColumn(rowFromCellIndex, columnFromCellIndex);
    if (!str2.Contains(".") && !this.m_dicVariables.ContainsKey(str2) && action == UnknownVariableAction.Skip && strArguments == null)
      return builder;
    MarkerOptionsImpl options;
    IList arguments = this.ParseArguments(strArguments, out options);
    options.MarkerIndex = i;
    options.OriginalMarker = str1;
    if (isMergeEnabled)
      options.IsMergeApplied = true;
    this.SetVariable(str2, stringValue, sheet, migrantRange, arrCells, arguments, options, builder, action);
    isMergeEnabled = options.IsMergeApplied;
    return builder;
  }

  private string GetVariableName(ref string strText, out string strArguments)
  {
    strArguments = (string) null;
    if (strText == null)
      throw new ArgumentNullException(nameof (strText));
    if (strText.Length == 0)
      throw new ArgumentException("strText - string cannot be empty.");
    int length1 = strText.IndexOf(this.m_chSeparator);
    if (length1 > 0)
    {
      strArguments = strText.Substring(length1 + 1);
      strText = strText.Substring(0, length1);
    }
    if (strText.StartsWith(this.m_strMarkerPrefix))
      strText = strText.Remove(0, this.m_strMarkerPrefix.Length);
    int length2 = strText.IndexOf(".");
    string variableName = length2 >= 0 ? strText.Substring(0, length2) : strText;
    strText = length2 >= 0 ? strText.Remove(0, length2 + 1) : string.Empty;
    return variableName;
  }

  private string GetParentName(string strText)
  {
    int length1 = strText.IndexOf(this.m_chSeparator);
    if (length1 > 0)
      strText = strText.Substring(0, length1);
    if (strText.StartsWith(this.m_strMarkerPrefix))
      strText = strText.Remove(0, this.m_strMarkerPrefix.Length);
    int length2 = strText.LastIndexOf(".");
    if (length2 > 0)
      strText = strText.Substring(0, length2);
    return strText;
  }

  private void SetVariable(
    string strVariable,
    string strText,
    IWorksheet sheet,
    IMigrantRange migrantRange,
    IList<long> arrMarkerCells,
    IList lstArguments,
    MarkerOptionsImpl options,
    RangeBuilder builder,
    UnknownVariableAction action)
  {
    switch (strVariable)
    {
      case null:
        throw new ArgumentNullException(nameof (strVariable));
      case "":
        throw new ArgumentException("strVariable - string cannot be empty.");
      default:
        object empty;
        this.m_dicVariables.TryGetValue(strVariable, out empty);
        if (empty == null)
        {
          switch (action)
          {
            case UnknownVariableAction.Exception:
              throw new ArgumentOutOfRangeException(strVariable, "Can't find variable");
            case UnknownVariableAction.Skip:
              return;
            case UnknownVariableAction.ReplaceBlank:
              empty = (object) string.Empty;
              break;
          }
        }
        VariableTypeAction variableTypeAction = VariableTypeAction.None;
        this.m_variableTypeActions.TryGetValue(strVariable, out variableTypeAction);
        migrantRange.Text = string.Empty;
        this.SetUnknownVariable(empty, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
        break;
    }
  }

  private void SetUnknownVariable(
    object value,
    string strText,
    IWorksheet sheet,
    IMigrantRange migrantRange,
    IList<long> arrMarkerCells,
    IList lstArguments,
    MarkerOptionsImpl options,
    RangeBuilder builder,
    UnknownVariableAction action,
    VariableTypeAction variableTypeAction)
  {
    if (this.IsDataView(value))
      this.SetDataView((DataView) value, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
    else if (this.IsArray(value))
    {
      if ((strText == null || strText.Length == 0) && value is byte[] bytes)
      {
        if (!this.AppImplementation.IsValidImage(bytes))
          return;
        this.SetSimpleValue(value, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, (string) null, (Type) null, (IHyperLink) null);
      }
      else
        this.SetArrayValue((IList) value, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
    }
    else if (this.IsCollection(value))
      this.SetCollectionValue((ICollection) value, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
    else if (this.IsDataSet(value))
      this.SetDataSetValue((DataSet) value, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
    else if (this.IsDataTable(value))
      this.SetDataTable((DataTable) value, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
    else if (this.IsDataColumn(value))
      this.SetDataColumn((DataColumn) value, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
    else
      this.SetSimpleValue(value, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, (string) null, (Type) null, (IHyperLink) null);
  }

  private void SetUnknownVariable(
    object value,
    string strText,
    IWorksheet sheet,
    IMigrantRange migrantRange,
    IList<long> arrMarkerCells,
    IList lstArguments,
    MarkerOptionsImpl options,
    RangeBuilder builder,
    UnknownVariableAction action,
    VariableTypeAction variableTypeAction,
    string numberFormat,
    Type type,
    IHyperLink imageHyperlink)
  {
    if (this.IsDataView(value))
      this.SetDataView((DataView) value, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
    else if (this.IsArray(value))
    {
      if ((strText == null || strText.Length == 0) && value is byte[] bytes)
      {
        if (!this.AppImplementation.IsValidImage(bytes))
          return;
        this.SetSimpleValue(value, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, numberFormat, type, imageHyperlink);
      }
      else
        this.SetArrayValue((IList) value, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
    }
    else if (this.IsCollection(value))
      this.SetCollectionValue((ICollection) value, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
    else if (this.IsDataSet(value))
      this.SetDataSetValue((DataSet) value, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
    else if (this.IsDataTable(value))
      this.SetDataTable((DataTable) value, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
    else if (this.IsDataColumn(value))
      this.SetDataColumn((DataColumn) value, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
    else
      this.SetSimpleValue(value, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, numberFormat, type, imageHyperlink);
  }

  private void SetSimpleValue(
    object value,
    string strText,
    IWorksheet sheet,
    IMigrantRange migrantRange,
    IList<long> arrMarkerCells,
    IList lstArguments,
    MarkerOptionsImpl options,
    RangeBuilder builder,
    UnknownVariableAction action,
    string numberFormat,
    Type valueType,
    IHyperLink imageHyperlink)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (builder == null)
      throw new ArgumentNullException(nameof (builder));
    VariableTypeAction variableTypeAction = VariableTypeAction.None;
    if (strText == null || strText.Length == 0)
    {
      this.SetValueOnRange(value, sheet, migrantRange, lstArguments, options, builder, variableTypeAction, numberFormat, valueType, imageHyperlink);
    }
    else
    {
      string newNumberFormat = (string) null;
      Type newType = (Type) null;
      if (value is IHyperLink)
        imageHyperlink = value as IHyperLink;
      value = this.GetNextValue(value, sheet, ref strText, out newNumberFormat, out newType, action);
      this.SetUnknownVariable(value, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction, newNumberFormat, newType, imageHyperlink);
    }
  }

  private void SetValueOnRange(
    object value,
    IWorksheet sheet,
    IMigrantRange migrantRange,
    IList lstArguments,
    MarkerOptionsImpl options,
    RangeBuilder builder,
    VariableTypeAction variableTypeAction,
    string numberFormat,
    Type valueType,
    IHyperLink imageHyperlink)
  {
    if (valueType != (Type) null)
    {
      variableTypeAction = VariableTypeAction.DetectDataType;
      value = value is string ? this.GetValue(value as string, valueType) : value;
      migrantRange.Value2 = value == null || this.IsDynamic(value) ? (object) "" : value;
      if (numberFormat != null)
      {
        variableTypeAction = VariableTypeAction.DetectNumberFormat;
        migrantRange.NumberFormat = numberFormat;
      }
    }
    else
    {
      IHyperLink hyperLink1 = value as IHyperLink;
      string numberFormat1 = migrantRange.NumberFormat;
      if (value == null)
        migrantRange.Value2 = (object) "";
      else if (numberFormat1.Contains("@") && value is string)
        migrantRange.Text = value.ToString();
      else if (value is Bitmap)
      {
        bool isMergeApplied = options.IsMergeApplied || this.IsMergeOnArgumentList(lstArguments);
        if (!this.IsFitToCell((Image) value, sheet, migrantRange, lstArguments, isMergeApplied))
          this.SetImageValueWithArguments((Image) value, sheet, migrantRange, lstArguments, isMergeApplied, imageHyperlink);
      }
      else if (value is byte[] numArray)
      {
        if (this.AppImplementation.IsValidImage(numArray))
        {
          using (MemoryStream memoryStream = new MemoryStream(numArray))
          {
            Image image = Image.FromStream((Stream) memoryStream);
            if (image != null)
            {
              bool isMergeApplied = options.IsMergeApplied || this.IsMergeOnArgumentList(lstArguments);
              if (!this.IsFitToCell(image, sheet, migrantRange, lstArguments, isMergeApplied))
                this.SetImageValueWithArguments(image, sheet, migrantRange, lstArguments, isMergeApplied, imageHyperlink);
            }
          }
        }
      }
      else if (hyperLink1 != null)
      {
        IHyperLink hyperLink2 = hyperLink1;
        IHyperLink hyperLink3 = sheet.HyperLinks.Add((IRange) migrantRange);
        int type = (int) hyperLink2.Type;
        hyperLink3.Type = hyperLink2.Type;
        if (hyperLink2.Address != null)
          hyperLink3.Address = hyperLink2.Address;
        if (hyperLink2.ScreenTip != null)
          hyperLink3.ScreenTip = hyperLink2.ScreenTip;
        if (hyperLink2.SubAddress != null)
          hyperLink3.SubAddress = hyperLink2.SubAddress;
        if (hyperLink2.TextToDisplay != null)
          hyperLink3.TextToDisplay = hyperLink2.TextToDisplay;
      }
      else if (!string.IsNullOrEmpty(value as string))
      {
        string stringValue = value as string;
        if (!(migrantRange.Worksheet as WorksheetImpl).CheckAndAddHyperlink(stringValue, migrantRange as RangeImpl))
          migrantRange.Value2 = value == null || this.IsDynamic(value) ? (object) "" : value;
      }
      else
        migrantRange.Value2 = value == null || this.IsDynamic(value) ? (object) "" : value;
    }
    builder.Add(migrantRange.Row, migrantRange.Column);
  }

  private bool IsMergeOnArgumentList(IList lstArguments)
  {
    bool flag = false;
    for (int index = 0; index < lstArguments.Count; ++index)
    {
      if (lstArguments[index] is NewSpaceArgument lstArgument && lstArgument.IsMergeEnabled)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public void GetColumnType(
    string[] values,
    out List<string> numberFormats,
    out List<Type> valueTypes,
    VariableTypeAction variableTypeAction)
  {
    numberFormats = new List<string>();
    valueTypes = new List<Type>();
    if (variableTypeAction == VariableTypeAction.None)
      return;
    string numberFormat = (string) null;
    Type valueType = (Type) null;
    bool detectNumberFormat = variableTypeAction == VariableTypeAction.DetectNumberFormat;
    foreach (string str in values)
    {
      this.GetValue(str, ref numberFormat, ref valueType, detectNumberFormat);
      if (detectNumberFormat)
        numberFormats.Add(numberFormat);
      valueTypes.Add(valueType);
    }
  }

  public object GetValue(string value, Type valueType)
  {
    string s = value;
    int startIndex = value.IndexOf("%");
    if (startIndex != -1)
      s = value.Remove(startIndex);
    if (value.IndexOf("/") != -1)
    {
      string[] strArray = value.Split("/"[0]);
      if (strArray.Length == 2)
      {
        double result1 = 0.0;
        double result2 = 0.0;
        bool flag1 = double.TryParse(strArray[0], out result1);
        bool flag2 = double.TryParse(strArray[1], out result2);
        s = !flag1 || !flag2 ? value : (result1 / result2).ToString();
      }
    }
    if (valueType == typeof (int))
    {
      int result = 0;
      if (int.TryParse(s, NumberStyles.Any, (IFormatProvider) null, out result))
        return (object) result;
    }
    else if (valueType == typeof (double))
    {
      double result = 0.0;
      if (double.TryParse(s, NumberStyles.Any, (IFormatProvider) null, out result))
        return (object) result;
    }
    else if (valueType == typeof (DateTime))
    {
      DateTime result = DateTime.Now;
      if (DateTime.TryParse(value, (IFormatProvider) null, DateTimeStyles.AdjustToUniversal, out result) && result.Ticks > new DateTime(1900, 1, 1, 0, 0, 0, 0).Ticks)
        return (object) result;
    }
    else if (valueType == typeof (bool))
    {
      bool result = false;
      if (bool.TryParse(value, out result))
        return (object) result;
    }
    return (object) value;
  }

  public object GetValue(
    string value,
    ref string numberFormat,
    ref Type valueType,
    bool detectNumberFormat)
  {
    string s = value;
    DateTime result1 = DateTime.Now;
    double result2 = 0.0;
    bool result3 = false;
    int result4 = 0;
    if (value == null)
      return (object) "";
    int startIndex = value.IndexOf("%");
    if (startIndex != -1)
      s = value.Remove(startIndex);
    if (value.IndexOf("/") != -1)
    {
      string[] strArray = value.Split("/"[0]);
      if (strArray.Length == 2)
      {
        double result5 = 0.0;
        double result6 = 0.0;
        bool flag1 = double.TryParse(strArray[0], out result5);
        bool flag2 = double.TryParse(strArray[1], out result6);
        s = !flag1 || !flag2 ? value : (result5 / result6).ToString();
      }
    }
    if (double.TryParse(s, NumberStyles.Any, (IFormatProvider) null, out result2))
    {
      if (detectNumberFormat)
        numberFormat = this.m_book.InnerFormats.GetNumberFormat(value);
      valueType = typeof (double);
      return (object) result2;
    }
    if (int.TryParse(s, NumberStyles.Currency, (IFormatProvider) null, out result4))
    {
      if (detectNumberFormat)
        numberFormat = this.m_book.InnerFormats.GetNumberFormat(value);
      valueType = typeof (int);
      return (object) result4;
    }
    if (DateTime.TryParse(value, (IFormatProvider) null, DateTimeStyles.AdjustToUniversal, out result1))
    {
      if (detectNumberFormat)
        numberFormat = this.m_book.InnerFormats.GetDateFormat(value);
      valueType = typeof (DateTime);
      return (object) result1;
    }
    if (!bool.TryParse(value, out result3))
      return (object) value;
    valueType = typeof (bool);
    return (object) result3;
  }

  private object GetNextValue(
    object value,
    IWorksheet sheet,
    ref string strText,
    out string newNumberFormat,
    out Type newType,
    UnknownVariableAction action)
  {
    if (strText == null)
      throw new ArgumentNullException(nameof (strText));
    string str = strText.Length != 0 ? this.GetNextPropertyName(ref strText) : throw new ArgumentException("strText - string cannot be empty.");
    if (value == null || value == (object) string.Empty)
    {
      switch (action)
      {
        case UnknownVariableAction.Exception:
          throw new ArgumentNullException(nameof (value));
        case UnknownVariableAction.Skip:
        case UnknownVariableAction.ReplaceBlank:
          newType = (Type) null;
          newNumberFormat = (string) null;
          return (object) string.Empty;
      }
    }
    object[] arrAtt = (object[]) null;
    bool isCustomDynamic = false;
    bool isExpando;
    object fromUnknownObject = this.GetPropertyFromUnknownObject(value, str, out isExpando, out isCustomDynamic);
    if (isExpando)
    {
      IDictionary<string, object> dictionary = fromUnknownObject as IDictionary<string, object>;
      if (dictionary.Count != 0)
        value = dictionary[str];
    }
    else if (isCustomDynamic)
    {
      CallSite<Func<CallSite, object, object>> callSite = fromUnknownObject as CallSite<Func<CallSite, object, object>>;
      value = callSite.Target((CallSite) callSite, value);
    }
    else
    {
      PropertyInfo propertyInfo = fromUnknownObject as PropertyInfo;
      value = propertyInfo.GetValue(value, (object[]) null);
      if (propertyInfo != (PropertyInfo) null)
        arrAtt = propertyInfo.GetCustomAttributes(true);
    }
    if (arrAtt != null && arrAtt.Length > 0)
    {
      this.GetNewTypeAndNumberFormat(value, arrAtt, out newType, out newNumberFormat);
    }
    else
    {
      newType = (Type) null;
      newNumberFormat = (string) null;
    }
    return value;
  }

  private void GetNewTypeAndNumberFormat(
    object value,
    object[] arrAtt,
    out Type newType,
    out string newNumberFormat)
  {
    newType = (Type) null;
    if (value != null)
      newType = value.GetType();
    if (arrAtt[0] is TemplateMarkerAttributes markerAttributes)
    {
      if ((newNumberFormat = markerAttributes.NumberFormat) != null)
        return;
      newType = (Type) null;
    }
    else
    {
      newType = (Type) null;
      newNumberFormat = (string) null;
    }
  }

  private object GetPropertyFromUnknownObject(
    object value,
    string strProperty,
    out bool isExpando,
    out bool isCustomDynamic)
  {
    object fromUnknownObject = (object) null;
    isExpando = false;
    isCustomDynamic = false;
    Type type = value.GetType();
    PropertyInfo property = type.GetProperty(strProperty);
    if (property == (PropertyInfo) null)
    {
      if (type.FullName.Contains("System.Dynamic.ExpandoObject"))
      {
        if (!(value is IDictionary<string, object> dictionary))
          throw new ArgumentOutOfRangeException(strProperty, $"Property  \"{strProperty}\" not found");
        if (dictionary.Count != 0 && !dictionary.ContainsKey(strProperty))
          throw new ArgumentOutOfRangeException(strProperty, $"Property  \"{strProperty}\" not found");
        isExpando = true;
        fromUnknownObject = (object) dictionary;
      }
      else
      {
        bool? nullable = new bool?(false);
        if (value is DynamicObject && this.AppImplementation.HasDynamicOverrideMethods(type))
        {
          nullable = this.HasProperty(value, strProperty);
          if (nullable.HasValue && nullable.Value)
          {
            isCustomDynamic = true;
            if (this.m_callSiteCollection == null)
              this.m_callSiteCollection = new Dictionary<string, CallSite<Func<CallSite, object, object>>>();
            if (this.m_callSiteCollection.ContainsKey($"{type.Name}_{strProperty}"))
            {
              fromUnknownObject = (object) this.m_callSiteCollection[$"{type.Name}_{strProperty}"];
            }
            else
            {
              CallSite<Func<CallSite, object, object>> callSite = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, strProperty, type, (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
              this.m_callSiteCollection.Add($"{type.Name}_{strProperty}", callSite);
              fromUnknownObject = (object) callSite;
            }
          }
        }
        if ((nullable.HasValue ? (!nullable.Value ? 1 : 0) : 0) != 0)
          throw new ArgumentOutOfRangeException(strProperty, $"Property  \"{strProperty}\" not found");
      }
    }
    else
      fromUnknownObject = (object) property;
    return fromUnknownObject;
  }

  private bool? HasProperty(object value, string propertyName)
  {
    // ISSUE: reference to a compiler-generated field
    if (TemplateMarkersImpl.\u003CHasProperty\u003Eo__SiteContainer3.\u003C\u003Ep__Site4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      TemplateMarkersImpl.\u003CHasProperty\u003Eo__SiteContainer3.\u003C\u003Ep__Site4 = CallSite<Func<CallSite, Type, object, List<string>>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (TemplateMarkersImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    Func<CallSite, Type, object, List<string>> target = TemplateMarkersImpl.\u003CHasProperty\u003Eo__SiteContainer3.\u003C\u003Ep__Site4.Target;
    // ISSUE: reference to a compiler-generated field
    CallSite<Func<CallSite, Type, object, List<string>>> pSite4 = TemplateMarkersImpl.\u003CHasProperty\u003Eo__SiteContainer3.\u003C\u003Ep__Site4;
    Type type = typeof (List<string>);
    // ISSUE: reference to a compiler-generated field
    if (TemplateMarkersImpl.\u003CHasProperty\u003Eo__SiteContainer3.\u003C\u003Ep__Site5 == null)
    {
      // ISSUE: reference to a compiler-generated field
      TemplateMarkersImpl.\u003CHasProperty\u003Eo__SiteContainer3.\u003C\u003Ep__Site5 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "GetDynamicMemberNames", (IEnumerable<Type>) null, typeof (TemplateMarkersImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj = TemplateMarkersImpl.\u003CHasProperty\u003Eo__SiteContainer3.\u003C\u003Ep__Site5.Target((CallSite) TemplateMarkersImpl.\u003CHasProperty\u003Eo__SiteContainer3.\u003C\u003Ep__Site5, value);
    IList<string> stringList = (IList<string>) target((CallSite) pSite4, type, obj);
    return stringList.Count == 0 ? new bool?() : new bool?(stringList.Contains(propertyName));
  }

  private bool IsDynamic(object dynamicValue)
  {
    return dynamicValue.GetType().FullName.Contains("System.Dynamic.ExpandoObject") || dynamicValue is DynamicObject;
  }

  private void SetArrayValue(
    IList value,
    string strText,
    IWorksheet sheet,
    IMigrantRange migrantRange,
    IList<long> arrMarkerCells,
    IList lstArguments,
    MarkerOptionsImpl options,
    RangeBuilder builder,
    UnknownVariableAction action,
    VariableTypeAction variableTypeAction)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    int length = value != null ? value.Count : throw new ArgumentNullException(nameof (value));
    this.m_insertCount = value.Count;
    string newNumberFormat = (string) null;
    Type newType = (Type) null;
    if (length == 0)
      return;
    List<string> numberFormats = new List<string>();
    List<Type> valueTypes = new List<Type>();
    if (value[0] is string)
    {
      string[] values = new string[length];
      for (int index = 0; index < value.Count; ++index)
      {
        if (value[index] != null)
          values[index] = value[index].ToString();
        else if (action == UnknownVariableAction.Exception)
          throw new ArgumentNullException();
      }
      this.GetColumnType(values, out numberFormats, out valueTypes, variableTypeAction);
      if (valueTypes.Count > 0)
      {
        newType = valueTypes[0];
        if (numberFormats.Count > 0)
          newNumberFormat = numberFormats[0];
      }
    }
    int row1 = migrantRange.Row;
    object obj1 = value[0];
    object parentObject1 = obj1;
    int dotIndex = strText == null || strText != "" ? strText.IndexOf(".") : 0;
    PropertyInfo propertyInfo = (PropertyInfo) null;
    bool isExpando = false;
    bool isCustomDynamic = false;
    bool flag1 = false;
    bool isNestedObject = false;
    int collectionCount = 0;
    IList<string> arrMarkers = (IList<string>) new List<string>();
    int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(arrMarkerCells[options.MarkerIndex]);
    int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(arrMarkerCells[options.MarkerIndex]);
    foreach (KeyValuePair<long, string> dictMarker in this.dictMarkers)
    {
      if ((dictMarker.Key == arrMarkerCells[options.MarkerIndex] || options.Direction != MarkerDirection.Vertical ? (columnFromCellIndex != RangeImpl.GetColumnFromCellIndex(dictMarker.Key) ? 0 : (dictMarker.Value != null ? 1 : 0)) : (rowFromCellIndex == RangeImpl.GetRowFromCellIndex(dictMarker.Key) ? 1 : 0)) != 0)
        arrMarkers.Add(dictMarker.Value);
    }
    bool isClassObject = false;
    this.GetNestedObject(strText, sheet, options, action, parentObject1, dotIndex, arrMarkers, ref isNestedObject, ref collectionCount, ref isClassObject);
    CallSite<Func<CallSite, object, object>> callSite = (CallSite<Func<CallSite, object, object>>) null;
    if (dotIndex == -1)
    {
      object fromUnknownObject = this.GetPropertyFromUnknownObject(obj1, strText, out isExpando, out isCustomDynamic);
      if (isExpando)
      {
        IDictionary<string, object> dictionary = fromUnknownObject as IDictionary<string, object>;
        if (dictionary.Count != 0)
          obj1 = dictionary[strText];
        if (obj1 != null)
          flag1 = obj1.GetType() == typeof (string);
      }
      else if (isCustomDynamic)
      {
        callSite = fromUnknownObject as CallSite<Func<CallSite, object, object>>;
        obj1 = callSite.Target((CallSite) callSite, obj1);
        if (obj1 != null)
          flag1 = obj1.GetType() == typeof (string);
      }
      else
      {
        propertyInfo = fromUnknownObject as PropertyInfo;
        obj1 = propertyInfo.GetValue(obj1, (object[]) null);
        object[] customAttributes = propertyInfo.GetCustomAttributes(true);
        if (customAttributes != null && customAttributes.Length > 0)
          this.GetNewTypeAndNumberFormat(obj1, customAttributes, out newType, out newNumberFormat);
        flag1 = propertyInfo.PropertyType == typeof (string);
      }
      this.SetValueOnRange(obj1, sheet, migrantRange, lstArguments, options, builder, VariableTypeAction.None, newNumberFormat, newType, (IHyperLink) null);
    }
    else
      this.SetSimpleValue(obj1, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, newNumberFormat, newType, (IHyperLink) null);
    newNumberFormat = (string) null;
    Type valueType = (Type) null;
    Point startPoint = new Point(migrantRange.Row, migrantRange.Column);
    bool flag2 = false;
    if (options.IsMergeApplied && options.Direction == MarkerDirection.Horizontal)
      flag2 = true;
    bool isFirstRowInsert = true;
    for (int index = 1; index < length && this.PrepareNextCell(sheet, migrantRange, arrMarkerCells, lstArguments, options, dotIndex == -1 && isNestedObject, collectionCount, isClassObject, isFirstRowInsert); ++index)
    {
      if (flag2)
      {
        Point mergeCount = NewSpaceArgument.GetMergeCount(startPoint.X, startPoint.Y, sheet);
        if (mergeCount.X > 1 || mergeCount.Y > 1)
          sheet[migrantRange.Row, migrantRange.Column, migrantRange.Row + mergeCount.X - 1, migrantRange.Column + mergeCount.Y - 1].Merge();
      }
      if (valueTypes.Count > 0)
      {
        valueType = valueTypes[index];
        if (numberFormats.Count > 0)
          newNumberFormat = numberFormats[index];
      }
      object obj2 = value[index];
      object parentObject2 = obj2;
      isFirstRowInsert = false;
      this.GetNestedObject(strText, sheet, options, action, parentObject2, dotIndex, arrMarkers, ref isNestedObject, ref collectionCount, ref isClassObject);
      if (dotIndex == -1 && variableTypeAction == VariableTypeAction.None && flag1 && (propertyInfo != (PropertyInfo) null || isExpando || isCustomDynamic))
      {
        if (propertyInfo != (PropertyInfo) null)
          obj2 = propertyInfo.GetValue(obj2, (object[]) null);
        else if (isExpando)
        {
          IDictionary<string, object> dictionary = obj2 as IDictionary<string, object>;
          if (dictionary.Count != 0)
            obj2 = dictionary[strText];
        }
        else if (isCustomDynamic && callSite != null)
          obj2 = callSite.Target((CallSite) callSite, obj2);
        if (!this.TryAndSetStringValue(row1, migrantRange, obj2, options, false, builder))
          this.SetValueOnRange(obj2, sheet, migrantRange, lstArguments, options, builder, VariableTypeAction.None, newNumberFormat, valueType, (IHyperLink) null);
      }
      else
        this.SetSimpleValue(obj2, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, newNumberFormat, valueType, (IHyperLink) null);
    }
    if (dotIndex == -1 && isNestedObject || isClassObject)
    {
      int row2 = migrantRange.Row;
      int column = migrantRange.Column;
      this.m_insertCount = collectionCount;
      bool isLastRowInsert = true;
      this.ApplyArguments(sheet, ref row2, ref column, arrMarkerCells, lstArguments, options, isNestedObject, isClassObject, isFirstRowInsert, isLastRowInsert);
      this.m_insertCount = 0;
      if (options.Direction == MarkerDirection.Horizontal)
        column += collectionCount - 1;
      else
        row2 += collectionCount - 1;
      if (row2 != 0 && column != 0)
        migrantRange.ResetRowColumn(row2, column);
    }
    this.TryAndSetCopyRangeArguments(sheet, lstArguments, options, startPoint);
  }

  private void GetNestedObject(
    string strText,
    IWorksheet sheet,
    MarkerOptionsImpl options,
    UnknownVariableAction action,
    object parentObject,
    int dotIndex,
    IList<string> arrMarkers,
    ref bool isNestedObject,
    ref int collectionCount,
    ref bool isClassObject)
  {
    string parentName = this.GetParentName(options.OriginalMarker);
    string[] strArray = parentName.Split("."[0]);
    object obj1 = parentObject;
    foreach (string arrMarker in (IEnumerable<string>) arrMarkers)
    {
      if (arrMarker.Contains(".") && parentName != this.GetParentName(arrMarker) && arrMarker.Contains(parentName))
      {
        int collectionCount1 = 0;
        List<string> stringList = new List<string>((IEnumerable<string>) this.GetParentName(arrMarker).Split("."[0]));
        if (dotIndex == -1)
        {
          foreach (string str in strArray)
          {
            if (stringList.Contains(str))
              stringList.Remove(str);
          }
        }
        else
        {
          obj1 = parentObject;
          string strText1 = strText;
          string newNumberFormat = (string) null;
          Type newType = (Type) null;
          strArray = strText.Split("."[0]);
          int count = stringList.IndexOf(strArray[0]);
          if (count != -1)
            stringList.RemoveRange(0, count);
          int num;
          do
          {
            IList nextValue = this.GetNextValue(obj1, sheet, ref strText1, out newNumberFormat, out newType, action) as IList;
            num = strText1 == null || strText1 != "" ? strText1.IndexOf(".") : 0;
            if (nextValue != null)
            {
              obj1 = nextValue[nextValue.Count - 1];
              stringList.RemoveAt(0);
            }
            else
              break;
          }
          while (obj1 != null && num >= 0);
        }
        string[] array = stringList.ToArray();
        bool flag = this.CheckIfNestedObject(obj1, array, out collectionCount1);
        if (!flag)
          collectionCount = collectionCount;
        else if (isNestedObject || flag && collectionCount1 > 0 && collectionCount1 > collectionCount)
        {
          collectionCount = collectionCount1 > 0 ? collectionCount1 : 0;
          isNestedObject = true;
        }
      }
      else if (arrMarker.Contains(".") && arrMarker.Contains(parentName) && strArray.Length > 1)
      {
        List<PropertyInfo> propertyInfoList = new List<PropertyInfo>((IEnumerable<PropertyInfo>) obj1.GetType().GetProperties());
        bool flag = false;
        int num = 0;
        foreach (PropertyInfo propertyInfo in propertyInfoList)
        {
          object obj2 = propertyInfo.GetValue(obj1, (object[]) null);
          if (obj2 is IList)
          {
            if ((obj2 as IList).Count == 0)
              num = 0;
            else if (num < (obj2 as IList).Count - 1)
              num = (obj2 as IList).Count - 1;
          }
          else if (strArray[1] == propertyInfo.Name && !(obj2 is IList))
            flag = true;
        }
        if (flag && (collectionCount != 0 || num != 0))
        {
          collectionCount = num;
          isClassObject = flag;
        }
      }
    }
  }

  private void SetCollectionValue(
    ICollection value,
    string strText,
    IWorksheet sheet,
    IMigrantRange migrantRange,
    IList<long> arrMarkerCells,
    IList lstArguments,
    MarkerOptionsImpl options,
    RangeBuilder builder,
    UnknownVariableAction action,
    VariableTypeAction variableTypeAction)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (builder == null)
      throw new ArgumentNullException(nameof (builder));
    bool flag1 = true;
    this.m_insertCount = value.Count;
    Point startPoint = new Point(migrantRange.Row, migrantRange.Column);
    bool flag2 = false;
    if (options.IsMergeApplied && options.Direction == MarkerDirection.Horizontal)
      flag2 = true;
    int row = migrantRange.Row;
    PropertyInfo propertyInfo = (PropertyInfo) null;
    bool isExpando = false;
    bool isCustomDynamic = false;
    bool flag3 = false;
    CallSite<Func<CallSite, object, object>> callSite = (CallSite<Func<CallSite, object, object>>) null;
    foreach (object obj1 in (IEnumerable) value)
    {
      if (flag1 && !strText.Contains("."))
      {
        object fromUnknownObject = this.GetPropertyFromUnknownObject(obj1, strText, out isExpando, out isCustomDynamic);
        object obj2 = (object) null;
        if (isExpando)
        {
          IDictionary<string, object> dictionary = fromUnknownObject as IDictionary<string, object>;
          if (dictionary.Count != 0)
            obj2 = dictionary[strText];
          flag3 = obj2.GetType() == typeof (string);
        }
        else if (isCustomDynamic)
        {
          callSite = fromUnknownObject as CallSite<Func<CallSite, object, object>>;
          obj2 = callSite.Target((CallSite) callSite, obj1);
          flag3 = obj2.GetType() == typeof (string);
        }
        else
        {
          propertyInfo = fromUnknownObject as PropertyInfo;
          obj2 = propertyInfo.GetValue(obj1, (object[]) null);
          flag3 = propertyInfo.PropertyType == typeof (string);
        }
        this.SetValueOnRange(obj2, sheet, migrantRange, lstArguments, options, builder, VariableTypeAction.None, (string) null, (Type) null, (IHyperLink) null);
        flag1 = false;
      }
      else
      {
        if (!flag1)
        {
          if (!this.PrepareNextCell(sheet, migrantRange, arrMarkerCells, lstArguments, options))
            break;
        }
        if (!flag1 && flag2)
        {
          Point mergeCount = NewSpaceArgument.GetMergeCount(startPoint.X, startPoint.Y, sheet);
          if (mergeCount.X > 1 || mergeCount.Y > 1)
            sheet[migrantRange.Row, migrantRange.Column, migrantRange.Row + mergeCount.X - 1, migrantRange.Column + mergeCount.Y - 1].Merge();
        }
        if (flag3 && variableTypeAction == VariableTypeAction.None && !strText.Contains(".") && (propertyInfo != (PropertyInfo) null || isExpando || isCustomDynamic))
        {
          object obj3 = (object) null;
          if (propertyInfo != (PropertyInfo) null)
            obj3 = propertyInfo.GetValue(obj1, (object[]) null);
          else if (isExpando)
          {
            IDictionary<string, object> dictionary = obj1 as IDictionary<string, object>;
            if (dictionary.Count != 0)
              obj3 = dictionary[strText];
          }
          else if (isCustomDynamic && callSite != null)
            obj3 = callSite.Target((CallSite) callSite, obj1);
          if (!this.TryAndSetStringValue(row, migrantRange, obj3, options, false, builder))
            this.SetValueOnRange(obj3, sheet, migrantRange, lstArguments, options, builder, VariableTypeAction.None, (string) null, (Type) null, (IHyperLink) null);
        }
        else
          this.SetSimpleValue(obj1, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, (string) null, (Type) null, (IHyperLink) null);
        flag1 = false;
      }
    }
    this.TryAndSetCopyRangeArguments(sheet, lstArguments, options, startPoint);
  }

  private bool TryAndSetStringValue(
    int startIndex,
    IMigrantRange migrantRange,
    object item,
    MarkerOptionsImpl options,
    bool isStringSourceFormat,
    RangeBuilder builder)
  {
    bool flag = false;
    if (this.InsertCopiedCellsList != null && this.InsertCopiedCellsList.ContainsKey(startIndex))
    {
      int num = startIndex + this.InsertCopiedCellsList[startIndex].InsertedCellsCount;
      if (options.Direction == MarkerDirection.Vertical && migrantRange.Row <= num)
      {
        if ((this.InsertCopiedCellsList[startIndex].IsStyleCopied ? (isStringSourceFormat ? 1 : 0) : 0) != 0)
          migrantRange.Text = item != null ? item.ToString() : "";
        else
          migrantRange.Value = item != null ? item.ToString() : "";
        builder.Add(migrantRange.Row, migrantRange.Column);
        flag = true;
      }
    }
    return flag;
  }

  internal bool CheckIfNestedObject(object value, string[] arrProperty, out int collectionCount)
  {
    collectionCount = 0;
    bool flag = false;
    Type type1 = value.GetType();
    List<PropertyInfo> propertyInfoList = new List<PropertyInfo>((IEnumerable<PropertyInfo>) type1.GetProperties());
    if (propertyInfoList.Count < 1)
    {
      if (type1.FullName.Contains("System.Dynamic.ExpandoObject"))
      {
        foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) (value as IDictionary<string, object>))
        {
          object obj1 = keyValuePair.Value;
          if (obj1 is IEnumerable && !(obj1 is string) && arrProperty[0] == keyValuePair.Key)
          {
            flag = true;
            List<string> stringList = new List<string>((IEnumerable<string>) arrProperty);
            stringList.RemoveAt(0);
            arrProperty = stringList.ToArray();
            if (arrProperty.Length > 0)
            {
              foreach (object obj2 in (IEnumerable) (obj1 as IList))
              {
                int collectionCount1 = 0;
                if (this.CheckIfNestedObject(obj2, arrProperty, out collectionCount1))
                  collectionCount += collectionCount1;
              }
            }
            collectionCount = collectionCount + (obj1 as IList).Count - 1;
            break;
          }
        }
      }
      else if (value is DynamicObject && this.AppImplementation.HasDynamicOverrideMethods(type1))
      {
        object obj3 = value;
        string name = value.GetType().Name;
        // ISSUE: reference to a compiler-generated field
        if (TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Site7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Site7 = CallSite<Func<CallSite, Type, object, List<string>>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (TemplateMarkersImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, List<string>> target1 = TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Site7.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, List<string>>> pSite7 = TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Site7;
        Type type2 = typeof (List<string>);
        // ISSUE: reference to a compiler-generated field
        if (TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Site8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Site8 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "GetDynamicMemberNames", (IEnumerable<Type>) null, typeof (TemplateMarkersImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Site8.Target((CallSite) TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Site8, obj3);
        foreach (string str1 in (IEnumerable<string>) target1((CallSite) pSite7, type2, obj4))
        {
          // ISSUE: reference to a compiler-generated field
          if (TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Site9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Site9 = CallSite<Func<CallSite, object, CallSiteBinder>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof (CallSiteBinder), typeof (TemplateMarkersImpl)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, CallSiteBinder> target2 = TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Site9.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, CallSiteBinder>> pSite9 = TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Site9;
          // ISSUE: reference to a compiler-generated field
          if (TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Sitea == null)
          {
            // ISSUE: reference to a compiler-generated field
            TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Sitea = CallSite<Func<CallSite, Type, CSharpBinderFlags, string, object, CSharpArgumentInfo[], object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "GetMember", (IEnumerable<Type>) null, typeof (TemplateMarkersImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[5]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, CSharpBinderFlags, string, object, CSharpArgumentInfo[], object> target3 = TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Sitea.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, CSharpBinderFlags, string, object, CSharpArgumentInfo[], object>> pSitea = TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Sitea;
          Type type3 = typeof (Microsoft.CSharp.RuntimeBinder.Binder);
          string str2 = str1;
          // ISSUE: reference to a compiler-generated field
          if (TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Siteb == null)
          {
            // ISSUE: reference to a compiler-generated field
            TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Siteb = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "GetType", (IEnumerable<Type>) null, typeof (TemplateMarkersImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj5 = TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Siteb.Target((CallSite) TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Siteb, obj3);
          CSharpArgumentInfo[] csharpArgumentInfoArray = new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          };
          object obj6 = target3((CallSite) pSitea, type3, CSharpBinderFlags.None, str2, obj5, csharpArgumentInfoArray);
          CallSite<Func<CallSite, object, object>> callSite = CallSite<Func<CallSite, object, object>>.Create(target2((CallSite) pSite9, obj6));
          // ISSUE: reference to a compiler-generated field
          if (TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Sitec == null)
          {
            // ISSUE: reference to a compiler-generated field
            TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Sitec = CallSite<Func<CallSite, Func<CallSite, object, object>, CallSite<Func<CallSite, object, object>>, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Invoke(CSharpBinderFlags.None, typeof (TemplateMarkersImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj7 = TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Sitec.Target((CallSite) TemplateMarkersImpl.\u003CCheckIfNestedObject\u003Eo__SiteContainer6.\u003C\u003Ep__Sitec, callSite.Target, callSite, obj3);
          if (obj7 is IEnumerable && !(obj7 is string) && arrProperty[0] == str1)
          {
            flag = true;
            List<string> stringList = new List<string>((IEnumerable<string>) arrProperty);
            stringList.RemoveAt(0);
            arrProperty = stringList.ToArray();
            if (arrProperty.Length > 0)
            {
              foreach (object obj8 in (IEnumerable) (obj7 as IList))
              {
                int collectionCount2 = 0;
                if (this.CheckIfNestedObject(obj8, arrProperty, out collectionCount2))
                  collectionCount += collectionCount2;
              }
            }
            collectionCount = collectionCount + (obj7 as IList).Count - 1;
            break;
          }
        }
      }
    }
    else
    {
      foreach (PropertyInfo propertyInfo in propertyInfoList)
      {
        object obj9 = propertyInfo.GetValue(value, (object[]) null);
        if (obj9 is IEnumerable && !(obj9 is string) && arrProperty[0] == propertyInfo.Name)
        {
          flag = true;
          List<string> stringList = new List<string>((IEnumerable<string>) arrProperty);
          stringList.RemoveAt(0);
          arrProperty = stringList.ToArray();
          if (arrProperty.Length > 0)
          {
            foreach (object obj10 in (IEnumerable) (obj9 as IList))
            {
              int collectionCount3 = 0;
              if (this.CheckIfNestedObject(obj10, arrProperty, out collectionCount3))
                collectionCount += collectionCount3;
            }
          }
          if ((obj9 as IList).Count != 0)
          {
            collectionCount = collectionCount + (obj9 as IList).Count - 1;
            break;
          }
          break;
        }
      }
    }
    return flag;
  }

  private void SetDataSetValue(
    DataSet value,
    string strText,
    IWorksheet sheet,
    IMigrantRange migrantRange,
    IList<long> arrMarkerCells,
    IList lstArguments,
    MarkerOptionsImpl options,
    RangeBuilder builder,
    UnknownVariableAction action,
    VariableTypeAction variableTypeAction)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (strText == null || strText.Length == 0)
    {
      if (value.Tables.Count != 1)
        throw new ArgumentException("Can't import DataSet");
      this.SetDataTable(value.Tables[0], strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
    }
    else
    {
      string str = this.PeekNextPropertyName(strText);
      int length1 = str.Length;
      int length2 = strText.Length;
      PropertyInfo prop;
      if (this.IsProperty((object) value, str, out prop))
      {
        object obj = prop.GetValue((object) value, (object[]) null);
        strText = length2 > length1 ? strText.Substring(length1 + 1) : string.Empty;
        this.SetUnknownVariable(obj, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
      }
      else
      {
        DataTable table;
        if ((table = value.Tables[str]) != null)
        {
          strText = length2 > length1 ? strText.Substring(length1 + 1) : string.Empty;
          this.SetDataTable(table, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
        }
        else
          this.UnknownProperty(str, action, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, variableTypeAction);
      }
    }
  }

  private void SetDataView(
    DataView value,
    string strText,
    IWorksheet sheet,
    IMigrantRange migrantRange,
    IList<long> arrMarkerCells,
    IList lstArguments,
    MarkerOptionsImpl options,
    RangeBuilder builder,
    UnknownVariableAction action,
    VariableTypeAction variableTypeAction)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    string str = strText != null && strText.Length > 0 ? this.PeekNextPropertyName(strText) : throw new NotImplementedException();
    int length1 = str.Length;
    int length2 = strText.Length;
    PropertyInfo prop;
    if (this.IsProperty((object) value, str, out prop))
    {
      object obj = prop.GetValue((object) value, (object[]) null);
      strText = length2 > length1 ? strText.Substring(length1 + 1) : string.Empty;
      this.SetUnknownVariable(obj, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
    }
    else
    {
      DataColumn column;
      if ((column = value.ToTable().Columns[str]) != null)
      {
        strText = length2 > length1 ? strText.Substring(length1 + 1) : string.Empty;
        this.SetDataColumn(column, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
      }
      else
        this.UnknownProperty(str, action, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, variableTypeAction);
    }
  }

  private void SetDataTable(
    DataTable value,
    string strText,
    IWorksheet sheet,
    IMigrantRange migrantRange,
    IList<long> arrMarkerCells,
    IList lstArguments,
    MarkerOptionsImpl options,
    RangeBuilder builder,
    UnknownVariableAction action,
    VariableTypeAction variableTypeAction)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    this.m_insertCount = value.Rows.Count;
    string str = strText != null && strText.Length > 0 ? this.PeekNextPropertyName(strText) : throw new NotImplementedException();
    int length1 = str.Length;
    int length2 = strText.Length;
    PropertyInfo prop;
    if (this.IsProperty((object) value, str, out prop))
    {
      object obj = prop.GetValue((object) value, (object[]) null);
      strText = length2 > length1 ? strText.Substring(length1 + 1) : string.Empty;
      this.SetUnknownVariable(obj, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
    }
    else
    {
      DataColumn column;
      if ((column = value.Columns[str]) != null)
      {
        strText = length2 > length1 ? strText.Substring(length1 + 1) : string.Empty;
        this.SetDataColumn(column, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, variableTypeAction);
      }
      else
        this.UnknownProperty(str, action, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, variableTypeAction);
    }
  }

  private void UnknownProperty(
    string strProperty,
    UnknownVariableAction action,
    string strText,
    IWorksheet sheet,
    IMigrantRange migrantRange,
    IList<long> arrMarkerCells,
    IList lstArguments,
    MarkerOptionsImpl options,
    RangeBuilder builder,
    VariableTypeAction variableTypeAction)
  {
    switch (action)
    {
      case UnknownVariableAction.Exception:
        throw new ApplicationException($"Variable {strProperty} not found");
      case UnknownVariableAction.Skip:
        migrantRange.Text = options.OriginalMarker;
        break;
      case UnknownVariableAction.ReplaceBlank:
        this.SetSimpleValue((object) string.Empty, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, (string) null, (Type) null, (IHyperLink) null);
        break;
      default:
        throw new ApplicationException();
    }
  }

  private void SetDataColumn(
    DataColumn value,
    string strText,
    IWorksheet sheet,
    IMigrantRange migrantRange,
    IList<long> arrMarkerCells,
    IList lstArguments,
    MarkerOptionsImpl options,
    RangeBuilder builder,
    UnknownVariableAction action,
    VariableTypeAction variableTypeAction)
  {
    DataTable table = value.Table;
    DataRowCollection rows = table.Rows;
    if (rows.Count == 0)
      return;
    string numberFormat = (string) null;
    Type valueType = (Type) null;
    bool flag1 = value.DataType == typeof (string);
    if (variableTypeAction == VariableTypeAction.DetectNumberFormat || variableTypeAction == VariableTypeAction.DetectDataType)
    {
      bool detectNumberFormat = variableTypeAction == VariableTypeAction.DetectNumberFormat;
      object obj = rows[0][value];
      if (rows[0][value].GetType() == typeof (DBNull))
      {
        bool flag2 = false;
        for (int index = 0; index < table.Rows.Count; ++index)
        {
          if (rows[index][value].ToString().Length > 0 && !flag2)
          {
            flag2 = true;
            obj = rows[index][value];
          }
        }
      }
      if (obj is string)
        this.GetValue(obj as string, ref numberFormat, ref valueType, detectNumberFormat);
    }
    this.m_insertCount = rows.Count;
    Point startPoint = new Point(migrantRange.Row, migrantRange.Column);
    bool flag3 = false;
    if (options.IsMergeApplied && options.Direction == MarkerDirection.Horizontal)
      flag3 = true;
    bool isStringSourceFormat = migrantRange.NumberFormat == "General" || migrantRange.NumberFormat.Contains("@");
    int row = migrantRange.Row;
    int index1 = 0;
    for (int count = rows.Count; index1 < count; ++index1)
    {
      object obj = rows[index1][value];
      if (index1 <= 0 || this.PrepareNextCell(sheet, migrantRange, arrMarkerCells, lstArguments, options))
      {
        if (index1 > 0 && flag3)
        {
          Point mergeCount = NewSpaceArgument.GetMergeCount(startPoint.X, startPoint.Y, sheet);
          if (mergeCount.X > 1 || mergeCount.Y > 1)
            sheet[migrantRange.Row, migrantRange.Column, migrantRange.Row + mergeCount.X - 1, migrantRange.Column + mergeCount.Y - 1].Merge();
        }
        if (flag1 && variableTypeAction == VariableTypeAction.None)
        {
          if (!this.TryAndSetStringValue(row, migrantRange, obj, options, isStringSourceFormat, builder))
            this.SetSimpleValue(obj, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, numberFormat, valueType, (IHyperLink) null);
        }
        else
          this.SetSimpleValue(obj, strText, sheet, migrantRange, arrMarkerCells, lstArguments, options, builder, action, numberFormat, valueType, (IHyperLink) null);
      }
      else
        break;
    }
    this.TryAndSetCopyRangeArguments(sheet, lstArguments, options, startPoint);
  }

  private void TryAndSetCopyRangeArguments(
    IWorksheet sheet,
    IList lstArguments,
    MarkerOptionsImpl options,
    Point startPoint)
  {
    int count = lstArguments.Count;
    if (count == 0)
      return;
    for (int index = 0; index < count; ++index)
    {
      if (lstArguments[index] is CopyRangeArgument lstArgument)
        lstArgument.TryAndApplyCopy(sheet, options, startPoint);
    }
  }

  private bool PrepareNextCell(
    IWorksheet sheet,
    IMigrantRange migrantRange,
    IList<long> arrMarkerCells,
    IList lstArguments,
    MarkerOptionsImpl options)
  {
    return this.PrepareNextCell(sheet, migrantRange, arrMarkerCells, lstArguments, options, false, 0, false, false);
  }

  private bool PrepareNextCell(
    IWorksheet sheet,
    IMigrantRange migrantRange,
    IList<long> arrMarkerCells,
    IList lstArguments,
    MarkerOptionsImpl options,
    bool isNestedObject,
    int collectionCount,
    bool isClassObject,
    bool isFirstRowInsert)
  {
    int row = migrantRange.Row;
    int column = migrantRange.Column;
    if (isNestedObject || isClassObject)
      this.m_insertCount = collectionCount;
    this.ApplyArguments(sheet, ref row, ref column, arrMarkerCells, lstArguments, options, isNestedObject, isClassObject, isFirstRowInsert, false);
    if (isNestedObject || isClassObject)
    {
      if (options.Direction == MarkerDirection.Horizontal)
        column += collectionCount;
      else
        row += collectionCount;
    }
    bool flag = row != 0 && column != 0;
    if (flag)
      migrantRange.ResetRowColumn(row, column);
    return flag;
  }

  private bool IsArray(object value) => value != null && value is IList;

  private bool IsCollection(object value) => value != null && value is ICollection;

  private bool IsDataSet(object value) => value != null && value is DataSet;

  private bool IsDataTable(object value) => value != null && value is DataTable;

  private bool IsDataView(object value) => value is DataView;

  private bool IsDataColumn(object value) => value != null && value is DataColumn;

  private bool IsProperty(object value, string strPropName, out PropertyInfo prop)
  {
    prop = (PropertyInfo) null;
    if (value == null)
      return false;
    if (strPropName == null || strPropName.Length == 0)
      throw new ArgumentException("strPropName - string cannot be empty.");
    Type type = value.GetType();
    prop = type.GetProperty(strPropName);
    return prop != (PropertyInfo) null;
  }

  private string GetNextPropertyName(ref string strText)
  {
    if (strText == null)
      throw new ArgumentNullException(nameof (strText));
    if (strText.Length == 0)
      throw new ArgumentException("strText - string cannot be empty.");
    int length = strText.IndexOf(".");
    string nextPropertyName;
    if (length < 0)
    {
      nextPropertyName = strText;
      strText = string.Empty;
    }
    else
    {
      nextPropertyName = strText.Substring(0, length);
      strText = strText.Substring(length + 1);
    }
    return nextPropertyName;
  }

  private string PeekNextPropertyName(string strText)
  {
    switch (strText)
    {
      case null:
        throw new ArgumentNullException(nameof (strText));
      case "":
        throw new ArgumentException("strText - string cannot be empty.");
      default:
        int length = strText.IndexOf(".");
        return length < 0 ? strText : strText.Substring(0, length);
    }
  }

  private IList ParseArguments(string strArguments, out MarkerOptionsImpl options)
  {
    options = new MarkerOptionsImpl((IWorkbook) this.m_book);
    SortedList<int, List<MarkerArgument>> lstArguments = (SortedList<int, List<MarkerArgument>>) null;
    if (strArguments != null && strArguments.Length != 0)
    {
      string[] strArray = strArguments.Split(this.m_chSeparator);
      int length = strArray.Length;
      lstArguments = new SortedList<int, List<MarkerArgument>>(length);
      for (int index = 0; index < length; ++index)
      {
        MarkerArgument markerArgument = this.ParseArgument(strArray[index]);
        if (markerArgument == null)
          throw new ArgumentOutOfRangeException("strArgument", "Unknown argument");
        if (markerArgument.IsPreparing)
          markerArgument.PrepareOptions(options);
        if (markerArgument.IsApplyable)
        {
          int priority = markerArgument.Priority;
          List<MarkerArgument> markerArgumentList;
          if (!lstArguments.TryGetValue(markerArgument.Priority, out markerArgumentList))
          {
            markerArgumentList = new List<MarkerArgument>();
            lstArguments.Add(priority, markerArgumentList);
          }
          markerArgumentList.Add(markerArgument);
        }
      }
    }
    return this.ConvertToList(lstArguments, options);
  }

  private IList ConvertToList(
    SortedList<int, List<MarkerArgument>> lstArguments,
    MarkerOptionsImpl options)
  {
    List<MarkerArgument> list = new List<MarkerArgument>();
    if (lstArguments == null)
      lstArguments = new SortedList<int, List<MarkerArgument>>(1);
    if (lstArguments.ContainsKey(TemplateMarkersImpl.s_arrArguments[this.DEF_FIT_TO_CELL_ARG_INDEX].Priority))
    {
      lstArguments.Remove(TemplateMarkersImpl.s_arrArguments[5].Priority);
      lstArguments.Remove(TemplateMarkersImpl.s_arrArguments[4].Priority);
    }
    this.InsertJumpAttribute(lstArguments, options);
    IList<List<MarkerArgument>> values = lstArguments.Values;
    int index = 0;
    for (int count = lstArguments.Count; index < count; ++index)
    {
      List<MarkerArgument> collection = values[index];
      list.AddRange((IEnumerable<MarkerArgument>) collection);
    }
    return (IList) list;
  }

  private void InsertJumpAttribute(
    SortedList<int, List<MarkerArgument>> lstMarkers,
    MarkerOptionsImpl options)
  {
    if (lstMarkers == null)
      throw new ArgumentNullException(nameof (lstMarkers));
    if (options == null)
      throw new ArgumentNullException(nameof (options));
    int iRow;
    int iColumn;
    if (options.Direction == MarkerDirection.Horizontal)
    {
      iRow = 0;
      iColumn = 1;
    }
    else
    {
      iRow = 1;
      iColumn = 0;
    }
    JumpArgument jumpArgument = new JumpArgument(iRow, iColumn, true, true);
    if (lstMarkers.ContainsKey(jumpArgument.Priority))
      return;
    lstMarkers.Add(jumpArgument.Priority, new List<MarkerArgument>(1)
    {
      (MarkerArgument) jumpArgument
    });
  }

  private MarkerArgument ParseArgument(string strArgument)
  {
    switch (strArgument)
    {
      case null:
        throw new ArgumentNullException(nameof (strArgument));
      case "":
        throw new ArgumentException("strArgument - string cannot be empty.");
      default:
        int index = 0;
        for (int count = TemplateMarkersImpl.s_arrArguments.Count; index < count; ++index)
        {
          MarkerArgument markerArgument = TemplateMarkersImpl.s_arrArguments[index].TryParse(strArgument);
          if (markerArgument != null)
            return markerArgument;
        }
        return (MarkerArgument) null;
    }
  }

  private void ApplyArguments(
    IWorksheet sheet,
    ref int iRow,
    ref int iColumn,
    IList<long> arrMarkerCells,
    IList lstArguments,
    MarkerOptionsImpl options,
    bool isNestedObject,
    bool isClassObject,
    bool isFirstRowInsert,
    bool isLastRowInsert)
  {
    int num1 = lstArguments != null ? lstArguments.Count : throw new ArgumentNullException(nameof (lstArguments));
    if (num1 == 0)
      return;
    Point pOldPosition = new Point(iRow, iColumn);
    Point mergedPoint = Point.Empty;
    for (int index = 0; index < num1; ++index)
    {
      MarkerArgument lstArgument = (MarkerArgument) lstArguments[index];
      switch (lstArgument)
      {
        case ImageSizeArgument _:
        case ImagePositionArgument _:
        case FitToCellArgument _:
          continue;
        case NewSpaceArgument _:
          NewSpaceArgument newSpaceArgument = (NewSpaceArgument) lstArgument;
          if (this.InsertCopiedCellsList == null)
            this.InsertCopiedCellsList = new Dictionary<int, InsertCopiedCellsInfo>();
          if (options.Direction == MarkerDirection.Vertical && !newSpaceArgument.IsInsertRow)
          {
            int cellsCount = !options.IsMergeApplied || mergedPoint.X <= 1 ? this.m_insertCount - 1 : (this.m_insertCount - 1) * mergedPoint.X;
            if (this.InsertCopiedCellsList.ContainsKey(iRow))
              this.InsertCopiedCellsList[iRow] = new InsertCopiedCellsInfo(newSpaceArgument.CopyStyles, cellsCount);
            else
              this.InsertCopiedCellsList.Add(iRow, new InsertCopiedCellsInfo(newSpaceArgument.CopyStyles, cellsCount));
          }
          if (isNestedObject && isFirstRowInsert && !isLastRowInsert && (!this.isNestobjectFirstrow || this.m_insertCount > 0))
          {
            mergedPoint = newSpaceArgument.ApplyArgumentWithMerge(sheet, pOldPosition, ref iRow, ref iColumn, arrMarkerCells, options, this.m_insertCount + 2);
            continue;
          }
          if (isNestedObject && isLastRowInsert && this.m_insertCount > 0)
          {
            mergedPoint = newSpaceArgument.ApplyArgumentWithMerge(sheet, pOldPosition, ref iRow, ref iColumn, arrMarkerCells, options, this.m_insertCount + 1);
            continue;
          }
          if (isNestedObject && !isLastRowInsert && !isFirstRowInsert && this.isNestobjectFirstrow && this.m_insertCount > 0)
          {
            mergedPoint = newSpaceArgument.ApplyArgumentWithMerge(sheet, pOldPosition, ref iRow, ref iColumn, arrMarkerCells, options, this.m_insertCount + 1);
            continue;
          }
          if (isNestedObject && !isLastRowInsert && !isFirstRowInsert && !this.isNestobjectFirstrow)
          {
            mergedPoint = newSpaceArgument.ApplyArgumentWithMerge(sheet, pOldPosition, ref iRow, ref iColumn, arrMarkerCells, options, this.m_insertCount + 2);
            continue;
          }
          if (!isNestedObject)
          {
            mergedPoint = newSpaceArgument.ApplyArgumentWithMerge(sheet, pOldPosition, ref iRow, ref iColumn, arrMarkerCells, options, this.m_insertCount);
            this.isNestobjectFirstrow = true;
            continue;
          }
          continue;
        case ImportOptionsArgument _:
        case ImportGroupOptionsArgument _:
          if (isNestedObject || isClassObject)
          {
            WorksheetImpl worksheetImpl = sheet as WorksheetImpl;
            switch (lstArgument)
            {
              case ImportOptionsArgument _:
                ImportOptionsArgument importOptionsArgument = lstArgument as ImportOptionsArgument;
                if (importOptionsArgument.IsMerge || importOptionsArgument.IsRepeat)
                {
                  if (options.Direction == MarkerDirection.Horizontal)
                  {
                    int num2 = iColumn - 1;
                    if (importOptionsArgument.IsMerge)
                    {
                      int x = num2 - 1;
                      int num3 = num2 + this.m_insertCount - 1;
                      int y = iRow - 1;
                      worksheetImpl.ImportMergeRanges.Add(new Rectangle(x, y, num3 - x, y - y));
                      worksheetImpl.ImportMergeRangeCollection.Add(sheet[iRow, num2]);
                      continue;
                    }
                    if (importOptionsArgument.IsRepeat)
                    {
                      worksheetImpl.SetRepeatRangeValues(iRow, num2, iRow, num2 + this.m_insertCount);
                      continue;
                    }
                    continue;
                  }
                  if (this.m_insertCount > 0)
                  {
                    int num4 = iRow - 1;
                    if (importOptionsArgument.IsMerge)
                    {
                      int x = iColumn - 1;
                      int y = num4 - 1;
                      int num5 = num4 + this.m_insertCount - 1;
                      worksheetImpl.ImportMergeRanges.Add(new Rectangle(x, y, x - x, num5 - y));
                      worksheetImpl.ImportMergeRangeCollection.Add(sheet[num4, iColumn]);
                      continue;
                    }
                    if (importOptionsArgument.IsRepeat)
                    {
                      worksheetImpl.SetRepeatRangeValues(num4, iColumn, num4 + this.m_insertCount, iColumn);
                      continue;
                    }
                    continue;
                  }
                  continue;
                }
                continue;
              case ImportGroupOptionsArgument _:
                if (this.m_insertCount > 0)
                {
                  ImportGroupOptionsArgument groupOptionsArgument = lstArgument as ImportGroupOptionsArgument;
                  if (options.Direction == MarkerDirection.Horizontal)
                  {
                    (sheet[iRow, iColumn, iRow, iColumn + this.m_insertCount - 1] as RangeImpl).Group(ExcelGroupBy.ByColumns, groupOptionsArgument.IsCollapse && !groupOptionsArgument.IsExpand, true);
                    continue;
                  }
                  (sheet[iRow, iColumn, iRow + this.m_insertCount - 1, iColumn] as RangeImpl).Group(ExcelGroupBy.ByRows, groupOptionsArgument.IsCollapse && !groupOptionsArgument.IsExpand, true);
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
          else
            continue;
        default:
          if (options.IsMergeApplied && lstArgument is JumpArgument)
          {
            JumpArgument jumpArgument = lstArgument as JumpArgument;
            if (mergedPoint.IsEmpty)
              mergedPoint = NewSpaceArgument.GetMergeCount(iRow, iColumn, sheet);
            if (mergedPoint.X > 1 || mergedPoint.Y > 1)
              jumpArgument.UpdateRowColumn(mergedPoint);
          }
          lstArgument.ApplyArgument(sheet, pOldPosition, ref iRow, ref iColumn, arrMarkerCells, options, this.m_insertCount);
          continue;
      }
    }
    this.m_insertCount = 0;
  }

  private bool IsFitToCell(
    Image image,
    IWorksheet sheet,
    IMigrantRange migrantRange,
    IList lstArguments,
    bool isMergeApplied)
  {
    bool cell = false;
    for (int index = 0; index < lstArguments.Count; ++index)
    {
      if (lstArguments[index] is FitToCellArgument)
      {
        cell = ((FitToCellArgument) lstArguments[index]).IsFitToCell;
        break;
      }
    }
    if (cell)
    {
      int columnWidthInPixels;
      int rowHeightInPixels;
      if (isMergeApplied && migrantRange.IsMerged)
      {
        int column = migrantRange.MergeArea.Column;
        int lastColumn = migrantRange.MergeArea.LastColumn;
        int row = migrantRange.MergeArea.Row;
        int lastRow = migrantRange.MergeArea.LastRow;
        columnWidthInPixels = (sheet as WorksheetImpl).GetColumnWidthInPixels(column, lastColumn);
        rowHeightInPixels = (sheet as WorksheetImpl).GetRowHeightInPixels(row, lastRow);
      }
      else
      {
        columnWidthInPixels = sheet.GetColumnWidthInPixels(migrantRange.Column);
        rowHeightInPixels = sheet.GetRowHeightInPixels(migrantRange.Row);
      }
      IPictureShape pictureShape = sheet.Pictures.AddPicture(migrantRange.Row, migrantRange.Column, image);
      pictureShape.Width = columnWidthInPixels;
      pictureShape.Height = rowHeightInPixels;
    }
    return cell;
  }

  private void SetImageValueWithArguments(
    Image image,
    IWorksheet sheet,
    IMigrantRange migrantRange,
    IList lstArguments,
    bool isMergeApplied,
    IHyperLink imageHyperlink)
  {
    Size sizeOfMigrantRange = new Size();
    bool flag = false;
    if (isMergeApplied && migrantRange.IsMerged)
    {
      int column = migrantRange.MergeArea.Column;
      int lastColumn = migrantRange.MergeArea.LastColumn;
      int row = migrantRange.MergeArea.Row;
      int lastRow = migrantRange.MergeArea.LastRow;
      sizeOfMigrantRange.Width = (sheet as WorksheetImpl).GetColumnWidthInPixels(column, lastColumn);
      sizeOfMigrantRange.Height = (sheet as WorksheetImpl).GetRowHeightInPixels(row, lastRow);
      flag = true;
    }
    else
    {
      sizeOfMigrantRange.Width = sheet.GetColumnWidthInPixels(migrantRange.Column);
      sizeOfMigrantRange.Height = sheet.GetRowHeightInPixels(migrantRange.Row);
    }
    Size sizeOfImage = this.CheckAndGetSizeOfImage(lstArguments, sheet, migrantRange, image);
    WorksheetImpl worksheetImpl = (WorksheetImpl) sheet;
    if (sizeOfImage.Width > sizeOfMigrantRange.Width)
    {
      if (flag)
      {
        int column = migrantRange.MergeArea.Column;
        int lastColumn = migrantRange.MergeArea.LastColumn;
        int num = (int) Math.Ceiling((double) sizeOfImage.Width / (double) (lastColumn - column + 1));
        for (int iColumn = column; iColumn <= lastColumn; ++iColumn)
          worksheetImpl.SetColumnWidthInPixels(iColumn, num);
        sizeOfMigrantRange.Width = num * (lastColumn - column + 1);
      }
      else
      {
        worksheetImpl.SetColumnWidthInPixels(migrantRange.Column, sizeOfImage.Width);
        sizeOfMigrantRange.Width = sizeOfImage.Width;
      }
    }
    if (sizeOfImage.Height > sizeOfMigrantRange.Height)
    {
      if (flag)
      {
        int row = migrantRange.MergeArea.Row;
        int lastRow = migrantRange.MergeArea.LastRow;
        int num = (int) Math.Ceiling((double) sizeOfImage.Height / (double) (lastRow - row + 1));
        for (int iRowIndex = row; iRowIndex <= lastRow; ++iRowIndex)
          worksheetImpl.SetRowHeightInPixels(iRowIndex, (double) num);
        sizeOfMigrantRange.Height = num * (lastRow - row + 1);
      }
      else
      {
        worksheetImpl.SetRowHeightInPixels(migrantRange.Row, (double) sizeOfImage.Height);
        sizeOfMigrantRange.Height = sizeOfImage.Height;
      }
    }
    IPictureShape pictureShape = sheet.Pictures.AddPicture(migrantRange.Row, migrantRange.Column, image);
    pictureShape.Width = sizeOfImage.Width;
    pictureShape.Height = sizeOfImage.Height;
    if (imageHyperlink != null)
      sheet.HyperLinks.Add((IShape) pictureShape, imageHyperlink.Type, imageHyperlink.Address, imageHyperlink.ScreenTip);
    this.CheckAndApplyPositionOfImage(pictureShape, sizeOfMigrantRange, lstArguments);
  }

  private void CheckAndApplyPositionOfImage(
    IPictureShape pictureShape,
    Size sizeOfMigrantRange,
    IList lstArguments)
  {
    ImagePositionArgument positionArgument = (ImagePositionArgument) TemplateMarkersImpl.s_arrArguments[5];
    for (int index = 0; index < lstArguments.Count; ++index)
    {
      if (lstArguments[index] is ImagePositionArgument)
        positionArgument = (ImagePositionArgument) lstArguments[index];
    }
    switch (positionArgument.HPosition)
    {
      case ImageHorizontalPosition.Center:
        pictureShape.Left += (sizeOfMigrantRange.Width - pictureShape.Width) / 2;
        break;
      case ImageHorizontalPosition.Right:
        pictureShape.Left += sizeOfMigrantRange.Width - pictureShape.Width;
        break;
    }
    switch (positionArgument.VPosition)
    {
      case ImageVerticalPosition.Middle:
        pictureShape.Top += (sizeOfMigrantRange.Height - pictureShape.Height) / 2;
        break;
      case ImageVerticalPosition.Bottom:
        pictureShape.Top += sizeOfMigrantRange.Height - pictureShape.Height;
        break;
    }
  }

  private Size CheckAndGetSizeOfImage(
    IList lstArguments,
    IWorksheet sheet,
    IMigrantRange migrantRange,
    Image image)
  {
    ImageSizeArgument arrArgument = (ImageSizeArgument) TemplateMarkersImpl.s_arrArguments[4];
    Size sizeOfImage = new Size(arrArgument.Width, arrArgument.Height);
    for (int index = 0; index < lstArguments.Count; ++index)
    {
      if (lstArguments[index] is ImageSizeArgument)
      {
        ImageSizeArgument lstArgument = (ImageSizeArgument) lstArguments[index];
        if (lstArgument.IsAutoWidth && lstArgument.IsAutoHeight)
        {
          sizeOfImage.Width = image.Width;
          sizeOfImage.Height = image.Height;
        }
        else if (lstArgument.IsAutoWidth)
        {
          double num = (double) lstArgument.Height / (double) image.Height;
          sizeOfImage.Width = (int) Math.Abs((double) image.Width * num);
          sizeOfImage.Height = lstArgument.Height;
        }
        else if (lstArgument.IsAutoHeight)
        {
          double num = (double) lstArgument.Width / (double) image.Width;
          sizeOfImage.Height = (int) Math.Abs((double) image.Height * num);
          sizeOfImage.Width = lstArgument.Width;
        }
        else
        {
          sizeOfImage.Width = lstArgument.Width;
          sizeOfImage.Height = lstArgument.Height;
        }
      }
    }
    return sizeOfImage;
  }

  public string MarkerPrefix
  {
    get => this.m_strMarkerPrefix;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (value));
        case "":
          throw new ArgumentException("value - string cannot be empty.");
        default:
          this.m_strMarkerPrefix = value;
          break;
      }
    }
  }

  public char ArgumentSeparator
  {
    get => this.m_chSeparator;
    set => this.m_chSeparator = value;
  }
}
