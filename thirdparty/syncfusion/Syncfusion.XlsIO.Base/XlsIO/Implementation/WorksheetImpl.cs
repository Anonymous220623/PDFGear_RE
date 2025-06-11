// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.WorksheetImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Microsoft.CSharp.RuntimeBinder;
using Syncfusion.Calculate;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Exceptions;
using Syncfusion.XlsIO.Implementation.PivotTables;
using Syncfusion.XlsIO.Implementation.Security;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.Tables;
using Syncfusion.XlsIO.Implementation.Xlsb;
using Syncfusion.XlsIO.Implementation.XmlReaders;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Windows.Forms;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class WorksheetImpl : 
  WorksheetBaseImpl,
  ISerializableNamedObject,
  INamedObject,
  IParseable,
  ICloneParent,
  IInternalWorksheet,
  IWorksheet,
  ITabSheet,
  IParentApplication,
  ISheetData,
  ICalcData
{
  private const string CalcualtedFormulaRangeFormat = "R{0}C{2}:R{1}C{2}";
  private const RegexOptions DEF_REGEX = RegexOptions.Compiled;
  internal const char DEF_STANDARD_CHAR = '0';
  private const float DEF_AXE_IN_RADIANS = 0.0174532924f;
  private const int DEF_MAX_COLUMN_WIDTH = 255 /*0xFF*/;
  private const double DEF_ZERO_CHAR_WIDTH = 8.0;
  private const int DEF_ARRAY_SIZE = 100;
  private const int DEF_AUTO_FILTER_WIDTH = 16 /*0x10*/;
  private const int DEF_INDENT_WIDTH = 12;
  private const double DEF_OLE_DOUBLE = 2958465.9999999884;
  private const double DEF_MAX_DOUBLE = 2958466.0;
  internal const char CarriageReturn = '\r';
  internal const char NewLine = '\n';
  private const char Comma = ',';
  private const string MSExcel = "Microsoft Excel";
  private const int DEFAULT_DATE_NUMBER_FORMAT_INDEX = 14;
  private CalcEngine m_calcEngine;
  internal bool m_hasSheetCalculation;
  private bool m_hasAlternateContent;
  private RichTextReader m_richTextReader;
  internal int unknown_formula_name = 9;
  private Dictionary<long, MergedCellInfo> mergedCellPositions;
  private static readonly TBIFFRecord[] s_arrAutofilterRecord = new TBIFFRecord[3]
  {
    TBIFFRecord.AutoFilter,
    TBIFFRecord.AutoFilterInfo,
    TBIFFRecord.FilterMode
  };
  private static string m_linkPattern = "((www\\.|(http|https|ftp|news|file)+\\:\\/\\/)[&#95;.a-z0-9-]+\\.[a-z0-9\\/&#95;:@=.+?,##%&~-]*[^.|\\'|\\# |!|\\(|?|,| |>|<|;|\\)])";
  private static string m_mailPattern = "([\\w-]+(.[\\w-]+)@([a-z0-9-]+(.[a-z0-9-]+)?.[a-z]{2,6}|(\\d{1,3}.){3}\\d{1,3})(:\\d{4})?)";
  private static List<Regex> m_hyperlinkPatterns = new List<Regex>();
  private bool m_isExportDataTable;
  private List<int> m_insertedRows;
  private SortedDictionary<int, int> m_movedRows;
  private Dictionary<long, string> m_formulaValues;
  private Dictionary<int, int> m_indexAndLevels;
  private bool m_bParseDataOnDemand;
  private RangeImpl m_rngUsed;
  private CellRecordCollection m_dicRecordsCells;
  private ColumnInfoRecord[] m_arrColumnInfo;
  private bool m_bDisplayPageBreaks;
  private PageSetupImpl m_pageSetup;
  private double m_dStandardColWidth = 8.43;
  private MergeCellsImpl m_mergedCells;
  private List<SelectionRecord> m_arrSelections;
  private PaneRecord m_pane;
  private WorksheetNamesCollection m_names;
  private ExcelSheetType m_sheetType;
  private bool m_bStringsPreserved;
  private List<BiffRecordRaw> m_arrAutoFilter;
  private SortedList<int, NoteRecord> m_arrNotes;
  private SortedList<long, NoteRecord> m_arrNotesByCellIndex;
  private NameImpl.NameIndexChangedEventHandler m_nameIndexChanged;
  private DataValidationTable m_dataValidation;
  private AutoFiltersCollection m_autofilters;
  private PivotTableCollection m_pivotTables;
  private HyperLinksCollection m_hyperlinks;
  private List<BiffRecordRaw> m_arrSortRecords;
  private int m_iPivotStartIndex = -1;
  private int m_iHyperlinksStartIndex = -1;
  private int m_iCondFmtPos = -1;
  private int m_iDValPos = -1;
  private int m_iCustomPropertyStartIndex = -1;
  private List<BiffRecordRaw> m_arrDConRecords;
  private WorksheetConditionalFormats m_arrConditionalFormats;
  private WorksheetCustomProperties m_arrCustomProperties;
  private IMigrantRange m_migrantRange;
  private IndexRecord m_index;
  private bool m_bUsedRangeIncludesFormatting = true;
  private bool m_busedRangeIncludesCF;
  private RangeTrueFalse m_stringPreservedRanges = new RangeTrueFalse();
  private ItemSizeHelper m_rowHeightHelper;
  private ListObjectCollection m_listObjects;
  private List<BiffRecordRaw> m_tableRecords;
  private bool m_isRowHeightSet;
  private bool m_isZeroHeight;
  private int m_baseColumnWidth = -1;
  private bool m_isThickBottom;
  private bool m_isThickTop;
  private byte m_outlineLevelColumn;
  private double m_defaultColWidth = 8.43;
  private byte m_outlineLevelRow;
  private ColumnInfoRecord m_rawColRecord;
  private bool m_bOptimizeImport;
  private SheetView m_view;
  internal List<Stream> preservedStreams;
  internal Dictionary<int, CondFMTRecord> m_dictCondFMT = new Dictionary<int, CondFMTRecord>();
  internal Dictionary<int, CFExRecord> m_dictCFExRecords = new Dictionary<int, CFExRecord>();
  private AutoFitManager m_autoFitManager;
  internal List<IOutlineWrapper> m_outlineWrappers;
  private Dictionary<int, List<GroupPoint>> m_columnOutlineLevels;
  private Dictionary<int, List<GroupPoint>> m_rowOutlineLevels;
  internal bool hasWrapMerge;
  private Syncfusion.XlsIO.Implementation.OleObjects m_oleObjects;
  private Syncfusion.XlsIO.Implementation.SparklineGroups m_sparklineGroups;
  private Dictionary<string, string> m_inlineStrings;
  private List<BiffRecordRaw> m_preserveExternalConnection;
  private List<Stream> m_preservePivotTables;
  internal Stream m_worksheetSlicer;
  private bool m_bIsExportDataTable;
  private ColumnCollection columnCollection;
  private bool m_isInsertingColumn;
  private bool m_isInsertingRow;
  private int insertRowCount;
  private int insertRowIndex;
  private int m_deleteRowIndex;
  private int m_deleteRowCount;
  private bool m_isDeletingRow;
  private bool m_isDeletingColumn;
  private ImportDTHelper m_importDTHelper;
  private bool m_bIsImporting;
  private ExtendedFormatImpl format;
  private int dateTimeStyleIndex;
  private RangeImpl m_CopyToRange;
  internal bool m_bisCFCopied;
  private DataTable m_dataTable;
  private IRange m_sourceRange;
  private int m_destXFIndex;
  private string m_destCell;
  private Dictionary<long, string> m_destRange = new Dictionary<long, string>();
  private bool m_bcheckCF;
  internal uint m_sharedFormulaGroupIndex;
  private bool m_bIsSubtotal;
  private IDataSort m_dataSorter;
  private bool m_bIsUnsupportedFormula;
  private bool m_isInsertingSubTotal;
  private bool m_isRemovingSubTotal;
  private bool m_isArrayFormulaSeparated;
  private int m_iDefaultXFIndex = -1;
  private List<Rectangle> m_importMergeRanges;
  private IRanges m_importMergeRangeCollection;
  private bool m_hasBaseColWidth;
  internal Dictionary<long, CellFormula> m_cellFormulas;
  internal Dictionary<string, string> m_autoFilterDisplayTexts = new Dictionary<string, string>();
  internal bool m_parseCondtionalFormats = true;
  internal bool m_parseCF = true;

  public event MissingFunctionEventHandler MissingFunction;

  public event RangeImpl.CellValueChangedEventHandler CellValueChanged;

  public event WorksheetImpl.ExportDataTableEventHandler ExportDataTableEvent;

  public CalcEngine CalcEngine
  {
    get => this.m_calcEngine;
    set => this.m_calcEngine = value;
  }

  internal bool HasSheetCalculation => this.m_hasSheetCalculation;

  internal bool HasAlternateContent
  {
    get => this.m_hasAlternateContent;
    set => this.m_hasAlternateContent = value;
  }

  public void EnableSheetCalculations()
  {
    this.m_book.EnabledCalcEngine = true;
    if (this.CalcEngine == null)
    {
      CalcEngine.ParseArgumentSeparator = this.AppImplementation.ArgumentsSeparator;
      CalcEngine.ParseDecimalSeparator = Convert.ToChar(this.AppImplementation.DecimalSeparator);
      this.CalcEngine = new CalcEngine((ICalcData) this);
      this.CalcEngine.UseDatesInCalculations = true;
      this.CalcEngine.UseNoAmpersandQuotes = true;
      this.CalcEngine.ArrayParser.GetArrayRecordPosition = new ArrayParser.ArrayDelegate(this.GetArrayRecordPosition);
      lock (this.CalcEngine)
      {
        int sheetFamilyId = CalcEngine.CreateSheetFamilyID();
        string str = "!";
        foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) this.ParentWorkbook.Worksheets)
        {
          if (worksheet.CalcEngine == null)
          {
            worksheet.CalcEngine = new CalcEngine((ICalcData) worksheet);
            worksheet.CalcEngine.UseDatesInCalculations = true;
            worksheet.CalcEngine.UseNoAmpersandQuotes = true;
            worksheet.CalcEngine.ExcelLikeComputations = true;
          }
          if (CalcEngine.modelToSheetID != null && CalcEngine.modelToSheetID.ContainsKey((object) (worksheet as WorksheetImpl)))
            CalcEngine.modelToSheetID.Remove((object) worksheet);
          worksheet.CalcEngine.RegisterGridAsSheet(worksheet.Name, (ICalcData) worksheet, sheetFamilyId);
          worksheet.CalcEngine.UnknownFunction += new UnknownFunctionEventHandler(this.CalcEngine_UnknownFunction);
          worksheet.CalcEngine.UpdateNamedRange += new UpdateNamedRangeEventHandler(this.UpdateNamedRange);
          worksheet.CalcEngine.UpdateExternalFormula += new UpdateExternalFormulaEventHandler(this.UpdateExternalFormula);
          worksheet.CalcEngine.QueryExternalWorksheet += new GetExternalWorksheetEventHandler(this.GetExternWorksheet);
          worksheet.CalcEngine.ArrayParser.GetArrayRecordPosition = new ArrayParser.ArrayDelegate(this.GetArrayRecordPosition);
          str = $"{str}{worksheet.Name}!";
          if (this.ParentWorkbook.Date1904)
            worksheet.CalcEngine.UseDate1904 = true;
        }
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach (IName name in (IEnumerable) this.ParentWorkbook.Names)
        {
          if (name.Scope.Length > 0 && str.IndexOf($"!{name.Scope}!") > -1 && name.Value != null)
          {
            if (dictionary.ContainsKey($"{name.Scope}!{name.Name}".ToUpper()))
              dictionary.Remove($"{name.Scope}!{name.Name}".ToUpper());
            dictionary.Add($"{name.Scope}!{name.Name}".ToUpper(), name.Value.Replace("'", ""));
          }
          else if (name.Name != null && name.Value != null)
          {
            if (dictionary.ContainsKey(name.Name.ToUpper()))
              dictionary.Remove(name.Name.ToUpper());
            dictionary.Add(name.Name.ToUpper(), name.Value.Replace("'", ""));
          }
        }
        Hashtable hashtable = new Hashtable();
        if (dictionary != null)
        {
          foreach (string key in dictionary.Keys)
            hashtable.Add((object) key.ToUpper(CultureInfo.InvariantCulture), (object) dictionary[key]);
        }
        foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) this.ParentWorkbook.Worksheets)
          worksheet.CalcEngine.NamedRanges = hashtable;
      }
    }
    this.m_hasSheetCalculation = true;
  }

  private void CalcEngine_UnknownFunction(object sender, UnknownFunctionEventArgs args)
  {
    this.m_bIsUnsupportedFormula = true;
    if (this.MissingFunction == null || this.CalcEngine == null)
      return;
    this.MissingFunction((object) this, new MissingFunctionEventArgs()
    {
      MissingFunctionName = args.MissingFunctionName,
      CellLocation = args.CellLocation
    });
  }

  private void UpdateNamedRange(object sender, UpdateNamedRangeEventArgs args)
  {
    IRange intersect1 = (IRange) null;
    string name = args.Name;
    if (this.TryGetIntersectRange(name, out intersect1) && intersect1 != null)
    {
      args.Address = intersect1.AddressGlobal;
      args.IsFormulaUpdated = true;
    }
    else
    {
      string intersect2;
      if (this.TryGetExternalIntersectRange(name, out intersect2) && intersect2 != null)
      {
        args.Address = intersect2;
        args.IsFormulaUpdated = true;
      }
      else
      {
        int length = name.LastIndexOf('!');
        if (length > 0)
        {
          string str1 = name.Substring(0, length);
          string str2 = name.Substring(length + 1);
          if (str1[0] == '\'' && str1[str1.Length - 1] == '\'')
            str1 = str1.Substring(1, str1.Length - 2);
          WorkbookImpl workbook = this.Workbook as WorkbookImpl;
          string str3 = workbook.IsCreated ? workbook.GetWorkbookName(workbook) : workbook.FullFileName;
          if (str3 == str1 || workbook.FullFileName != null && str3 == workbook.GetFilePath(workbook.FullFileName) + str1)
            name = str2;
        }
        if (this.Names.Contains(name) && this.Names[name].RefersToRange != null)
        {
          args.Address = this.Names[name].RefersToRange.AddressGlobal;
          args.IsFormulaUpdated = true;
        }
        else if (this.Workbook.Names.Contains(name) && this.Workbook.Names[name].RefersToRange != null)
        {
          args.Address = this.Workbook.Names[name].RefersToRange.AddressGlobal;
          args.IsFormulaUpdated = true;
        }
        else
          args.Address = name;
      }
    }
  }

  private void UpdateExternalFormula(object sender, UpdateExternalFormulaEventArgs args)
  {
    WorkbookImpl workbook1 = this.Workbook as WorkbookImpl;
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    string str = string.Empty;
    for (int index = 0; index < workbook1.ExternWorkbooks.Count; ++index)
    {
      ExternWorkbookImpl externWorkbook = workbook1.ExternWorkbooks[index];
      if (!externWorkbook.IsInternalReference)
      {
        string key = externWorkbook.URL.Replace("\\", "/");
        dictionary.Add(key, $"[{(object) externWorkbook.Index}]");
      }
    }
    string input = args.formula.Replace("[", "").Replace("]", "").Replace("\\", "/");
    foreach (KeyValuePair<string, string> keyValuePair in dictionary)
    {
      if (input.IndexOf(keyValuePair.Key, StringComparison.OrdinalIgnoreCase) > -1)
      {
        input = Regex.Replace(input, Regex.Escape(keyValuePair.Key), keyValuePair.Value, RegexOptions.IgnoreCase);
        args.formula = input;
        args.parsedFormula = args.formula;
        args.IsFormulaUpdated = true;
      }
    }
    string formula = args.formula;
    int num1 = formula.IndexOf('!');
    int num2 = -1;
    string empty = string.Empty;
    int result = -1;
    string identifier = formula.Substring(num1 + 1);
    bool flag = this.IsCellRange(workbook1, identifier);
    ExternWorkbookImpl externBook = (ExternWorkbookImpl) null;
    if ((formula.Contains("[") || formula.Contains("'[")) && formula.Contains("]"))
    {
      int num3 = formula.IndexOf("[");
      if (num3 > 0 && formula[num3 - 1] == '\'')
        str = formula.Substring(0, num3 - 1);
      int index;
      for (index = num3 + 1; formula[index] != ']'; ++index)
        empty += (string) (object) formula[index];
      if (int.TryParse(empty, out result) && result < workbook1.ExternWorkbooks.Count)
        externBook = workbook1.ExternWorkbooks[result];
      num2 = index;
    }
    if (externBook != null)
    {
      string sheetName = string.Empty;
      if (num2 != -1 && num1 > num2)
      {
        sheetName = formula.Substring(num2 + 1, num1 - num2 - 1);
        if (sheetName.EndsWith("'"))
          sheetName = sheetName.Substring(0, sheetName.Length - 1);
      }
      int externSheetIndex = this.GetExternSheetIndex(sheetName, externBook);
      if (args.IsFormulaUpdated && !externBook.IsParsed)
      {
        string filePath = workbook1.GetFilePath(workbook1.FullFileName);
        if (File.Exists(externBook.URL) || File.Exists(filePath + externBook.URL))
        {
          string strFileName = !File.Exists(externBook.URL) ? filePath + externBook.URL : externBook.URL;
          ExcelEngine excelEngine = new ExcelEngine();
          (excelEngine.Excel as ApplicationImpl).IsExternBookParsing = true;
          IWorkbook workbook2 = excelEngine.Excel.Workbooks.OpenReadOnly(strFileName);
          (excelEngine.Excel as ApplicationImpl).IsExternBookParsing = false;
          for (int Index = 0; Index < workbook2.Worksheets.Count; ++Index)
          {
            WorksheetImpl worksheet1 = workbook2.Worksheets[Index] as WorksheetImpl;
            int orAddSheet = externBook.FindOrAddSheet(worksheet1.Name);
            ExternWorksheetImpl worksheet2 = externBook.Worksheets[orAddSheet];
            worksheet2.SetCellRecords(worksheet1.CellRecords.Clone((object) worksheet2));
            worksheet2.FirstRow = worksheet1.FirstRow;
            worksheet2.FirstColumn = worksheet1.FirstColumn;
            worksheet2.LastRow = worksheet1.LastRow;
            worksheet2.LastColumn = worksheet1.LastColumn;
          }
          foreach (IName name in (CollectionBase<IName>) (workbook2 as WorkbookImpl).InnerNamesColection)
          {
            if (name != null && name.Name != null)
            {
              int index = !externBook.ExternNames.Contains(name.Name) ? externBook.ExternNames.Add(name.Name) : externBook.ExternNames.GetNameIndex(name.Name);
              externBook.ExternNames[index].RefersTo = name.RefersTo;
              externBook.ExternNames[index].sheetId = name.Worksheet != null ? name.Worksheet.Index : -1;
            }
          }
          workbook2.Close();
          excelEngine.Dispose();
          externBook.IsParsed = true;
        }
      }
      if (!this.TryGetIdentifier(externBook, ref identifier, ref externSheetIndex) && !flag || externSheetIndex == -1)
        return;
      args.parsedFormula = $"{str}[{externBook.Index.ToString()}]!{externSheetIndex.ToString()}!{identifier}";
      args.IsFormulaUpdated = true;
    }
    else
      args.parsedFormula = args.formula;
  }

  private bool TryGetIdentifier(
    ExternWorkbookImpl externBook,
    ref string identifier,
    ref int sheetIndex)
  {
    bool flag = false;
    if (identifier != string.Empty)
    {
      foreach (ExternNameImpl externName in (CollectionBase<ExternNameImpl>) externBook.ExternNames)
      {
        if (string.Equals(externName.Name.Replace("[", "").Replace("]", "").Replace(" ", ""), identifier, StringComparison.OrdinalIgnoreCase))
        {
          flag = true;
          string str = externName.RefersTo;
          if (str[0] == '=')
            str = str.Substring(1);
          int length = str.LastIndexOf('!');
          identifier = str.Substring(length + 1);
          if (identifier.Contains("$"))
            identifier = identifier.Replace("$", "");
          string sheetName = str.Substring(0, length);
          if (sheetName[0] == '\'' && sheetName[sheetName.Length - 1] == '\'')
            sheetName = sheetName.Substring(1, sheetName.Length - 2);
          sheetIndex = this.GetExternSheetIndex(sheetName, externBook);
          break;
        }
      }
    }
    return sheetIndex != -1 && flag;
  }

  private bool TryGetExternRangeAddress(WorkbookImpl workbook, ref string formula)
  {
    string str1 = string.Empty;
    string str2 = formula;
    int num1 = str2.IndexOf('!');
    int num2 = -1;
    string empty = string.Empty;
    int result = -1;
    string identifier = str2.Substring(num1 + 1);
    bool flag = this.IsCellRange(workbook, identifier);
    ExternWorkbookImpl externBook = (ExternWorkbookImpl) null;
    if (str2.Contains("'[") && str2.Contains("]"))
    {
      int num3 = str2.IndexOf("[");
      if (num3 > 0 && str2[num3 - 1] == '\'')
        str1 = str2.Substring(0, num3 - 1);
      int index;
      for (index = num3 + 1; str2[index] != ']'; ++index)
        empty += (string) (object) str2[index];
      if (int.TryParse(empty, out result) && result < workbook.ExternWorkbooks.Count)
        externBook = workbook.ExternWorkbooks[result];
      num2 = index;
    }
    bool externRangeAddress = false;
    if (externBook != null)
    {
      string sheetName = string.Empty;
      if (num2 != -1 && num1 > num2)
      {
        sheetName = str2.Substring(num2 + 1, num1 - num2 - 1);
        if (sheetName.EndsWith("'") && str2.LastIndexOf('\'') + 1 == num1)
          sheetName = sheetName.Substring(0, sheetName.Length - 1);
      }
      int externSheetIndex = this.GetExternSheetIndex(sheetName, externBook);
      if ((this.TryGetIdentifier(externBook, ref identifier, ref externSheetIndex) || flag) && externSheetIndex != -1)
      {
        string str3 = $"{str1}[{externBook.Index.ToString()}]!{externSheetIndex.ToString()}!{identifier}";
        formula = str3;
        externRangeAddress = true;
      }
    }
    return externRangeAddress;
  }

  private bool IsCellRange(WorkbookImpl workbook, string idendifier)
  {
    string strColumn2 = string.Empty;
    string str1;
    string str2;
    string strSheetName;
    string strRow2;
    return FormulaUtil.IsCell(idendifier, false, out str1, out str2) || FormulaUtil.IsCell3D(idendifier, false, out strSheetName, out str1, out str2) || workbook.FormulaUtil.IsCellRange(idendifier, false, out str1, out str2, out strRow2, out strColumn2) || workbook.FormulaUtil.IsCellRange3D(idendifier, false, out strSheetName, out str1, out str2, out strRow2, out strColumn2);
  }

  private int GetExternSheetIndex(string sheetName, ExternWorkbookImpl externBook)
  {
    int externSheetIndex = -1;
    if (sheetName != string.Empty)
    {
      foreach (KeyValuePair<int, ExternWorksheetImpl> worksheet in externBook.Worksheets)
      {
        if (string.Equals(worksheet.Value.Name, sheetName, StringComparison.OrdinalIgnoreCase))
        {
          externSheetIndex = worksheet.Value.Index;
          break;
        }
      }
    }
    return externSheetIndex;
  }

  private void GetExternWorksheet(object sender, QueryExternalWorksheetEventArgs args)
  {
    WorkbookImpl workbook = this.Workbook as WorkbookImpl;
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    int result1 = 0;
    int result2 = 0;
    string formula = args.formula;
    int index1 = 0;
    if (formula[index1] == '[')
    {
      for (++index1; index1 < formula.Length && formula[index1] != ']'; ++index1)
        empty2 += (string) (object) formula[index1];
      if (index1 < formula.Length)
        ++index1;
    }
    if (formula[index1] == '!')
    {
      int index2;
      for (index2 = index1 + 1; index2 < formula.Length && formula[index2] != '!'; ++index2)
        empty1 += (string) (object) formula[index2];
      if (index2 < formula.Length)
      {
        int num = index2 + 1;
      }
    }
    if (!(empty2 != string.Empty) || !int.TryParse(empty2, out result1) || result1 < 0 || result1 >= workbook.ExternWorkbooks.Count)
      return;
    ExternWorkbookImpl externWorkbook = workbook.ExternWorkbooks[result1];
    if (!(empty1 != string.Empty) || !int.TryParse(empty1, out result2) || result2 < 0 || result2 >= externWorkbook.Worksheets.Count)
      return;
    args.worksheet = (ICalcData) externWorkbook.Worksheets[result2];
    args.worksheetName = externWorkbook.Worksheets[result2].Name;
    args.IsWorksheetUpdated = true;
  }

  public void DisableSheetCalculations()
  {
    this.m_book.EnabledCalcEngine = false;
    if (this.CalcEngine == null || this.ParentWorkbook == null || this.ParentWorkbook.Worksheets == null)
      return;
    foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) this.ParentWorkbook.Worksheets)
    {
      if (worksheet.CalcEngine != null)
      {
        worksheet.CalcEngine.UnknownFunction -= new UnknownFunctionEventHandler(this.CalcEngine_UnknownFunction);
        worksheet.CalcEngine.UpdateNamedRange -= new UpdateNamedRangeEventHandler(this.UpdateNamedRange);
        worksheet.CalcEngine.UpdateExternalFormula -= new UpdateExternalFormulaEventHandler(this.UpdateExternalFormula);
        worksheet.CalcEngine.QueryExternalWorksheet -= new GetExternalWorksheetEventHandler(this.GetExternWorksheet);
        worksheet.CalcEngine.ArrayParser.GetArrayRecordPosition -= new ArrayParser.ArrayDelegate(this.GetArrayRecordPosition);
        worksheet.CalcEngine.Dispose();
      }
      worksheet.CalcEngine = (CalcEngine) null;
    }
    if (CalcEngine.modelToSheetID != null && CalcEngine.modelToSheetID.Count > 0)
    {
      List<object> objectList = new List<object>();
      foreach (object key in (IEnumerable) CalcEngine.modelToSheetID.Keys)
        objectList.Add(key);
      for (int index = 0; index < objectList.Count; ++index)
      {
        object key1 = objectList[index];
        if (key1 is ExternWorksheetImpl)
        {
          int key2 = (int) CalcEngine.modelToSheetID[key1];
          if (CalcEngine.sheetFamiliesList != null && CalcEngine.sheetFamiliesList.Count > 0)
          {
            CalcEngine.sheetFamiliesList[(object) key2] = (object) null;
            CalcEngine.sheetFamiliesList.Remove((object) key2);
          }
          CalcEngine.modelToSheetID.Remove(key1);
        }
      }
    }
    this.m_hasSheetCalculation = false;
  }

  public object GetValueRowCol(int row, int col)
  {
    bool flag = !this.m_book.IsCreated && this.m_book.Version != ExcelVersion.Excel97to2003 && this.m_book.BuiltInDocumentProperties.ApplicationName == "Microsoft Excel";
    WorksheetImpl.TRangeValueType type = this.GetCellType(row, col, false);
    if (flag && !this.m_book.IsCellModified && (this.m_book.IsConverting || this.m_book.Saving || this.m_book.Loading) && this.CalcEngine != null && this.CalcEngine.UseFormulaValues)
      return type == WorksheetImpl.TRangeValueType.Formula && this.GetCellType(row, col, true) == (WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula) && this.GetFormulaNumberValue(row, col) == 0.0 ? (object) this.GetFormulaFromWorksheet(row, col, true) : this.GetTextFromCellType(row, col, out type);
    if (type == WorksheetImpl.TRangeValueType.Formula)
      return (object) this.GetFormulaFromWorksheet(row, col, true);
    double number = this.GetNumber(row, col);
    ExcelFormatType excelFormatType = ExcelFormatType.General;
    if (!double.IsNaN(number))
    {
      int num = this.m_book.GetExtFormat(this.GetXFIndex(row, col)).NumberFormatIndex;
      if (this.m_book.InnerFormats.Count > 14 && !this.m_book.InnerFormats.Contains(num))
        num = 14;
      excelFormatType = this.m_book.InnerFormats[num].FormatType;
    }
    if (excelFormatType == ExcelFormatType.DateTime)
    {
      if (number < 0.0)
      {
        string text = this.GetText(row, col);
        return text == null ? (object) number : (object) Convert.ToDateTime(text).ToString();
      }
      return number == double.NaN || number > DateTime.MaxValue.ToOADate() || number < DateTime.MinValue.ToOADate() ? (object) DateTime.MinValue.ToString() : (object) UtilityMethods.ConvertNumberToDateTime(number, this.m_book.Date1904).ToString();
    }
    object valueRowCol = (object) this.GetValue(this.GetRecord(row, col), false);
    if (this.GetCellType(row, col, false) == WorksheetImpl.TRangeValueType.String && this.GetFormatType(row, col, false) == ExcelFormatType.Text)
      valueRowCol = (object) $"\"{valueRowCol}\"";
    return valueRowCol;
  }

  internal int GetArrayRecordPosition(int row, int col, ref int height, ref int width)
  {
    return this.GetArrayRecordPosition(row, col, ref height, ref width, (ICalcData) this);
  }

  internal int GetArrayRecordPosition(
    int row,
    int col,
    ref int height,
    ref int width,
    ICalcData calcData)
  {
    if (!(calcData is WorksheetImpl worksheetImpl))
      return -1;
    ArrayRecord arrayRecord = worksheetImpl.CellRecords.GetArrayRecord(row, col);
    int num1 = height;
    int num2 = width;
    if (row - (arrayRecord.FirstRow + 1) >= num1 || col - (arrayRecord.FirstColumn + 1) >= num2)
      return -1;
    int arrayRecordPosition = 0;
    if (height > arrayRecord.LastRow - arrayRecord.FirstRow + 1)
      height = arrayRecord.LastRow - arrayRecord.FirstRow + 1;
    if (width > arrayRecord.LastColumn - arrayRecord.FirstColumn + 1)
      width = arrayRecord.LastColumn - arrayRecord.FirstColumn + 1;
    for (int index1 = arrayRecord.FirstRow + 1; index1 <= arrayRecord.LastRow + 1; ++index1)
    {
      for (int index2 = arrayRecord.FirstColumn + 1; index2 <= arrayRecord.LastColumn + 1; ++index2)
      {
        if (index1 - (arrayRecord.FirstRow + 1) < num1 && index2 - (arrayRecord.FirstColumn + 1) < num2)
        {
          if (index1 == row && index2 == col)
            return arrayRecordPosition;
          ++arrayRecordPosition;
        }
      }
    }
    return arrayRecordPosition;
  }

  internal string GetFormulaFromWorksheet(int row, int col, bool isReturnArrayFormula)
  {
    Ptg[] arrayFormulaPtg = this.m_dicRecordsCells.Table.GetFormulaValue(row, col);
    string formulaFromWorksheet = !this.HasArrayFormula(arrayFormulaPtg) ? this.GetFormula(row - 1, col - 1, arrayFormulaPtg, false, this.m_book.FormulaUtil, false) : this.GetArrayFormula(row, col, isReturnArrayFormula, out arrayFormulaPtg);
    if (formulaFromWorksheet != null && formulaFromWorksheet.Contains("_xlfn."))
      formulaFromWorksheet = formulaFromWorksheet.Replace("_xlfn.", string.Empty);
    if (formulaFromWorksheet != null && formulaFromWorksheet.Contains("[#This Row],"))
      formulaFromWorksheet = formulaFromWorksheet.Replace("[#This Row],", "[#This Row]" + (object) this.AppImplementation.ArgumentsSeparator);
    return formulaFromWorksheet;
  }

  internal string GetArrayFormula(
    int row,
    int col,
    bool isReturnArrayFormula,
    out Ptg[] arrayFormulaPtg)
  {
    arrayFormulaPtg = (Ptg[]) null;
    if (!(this.GetRecord(row, col) is FormulaRecord record) || !this.IsArrayFormula(record))
      return (string) null;
    FormulaUtil formulaUtil = this.m_book.FormulaUtil;
    ArrayRecord arrayRecord = this.CellRecords.GetArrayRecord(record.Row + 1, record.Column + 1);
    string ptgArray;
    if (arrayRecord != null)
    {
      ptgArray = formulaUtil.ParsePtgArray(arrayRecord.Formula, arrayRecord.FirstRow, arrayRecord.FirstColumn, false, false);
      arrayFormulaPtg = arrayRecord.Formula;
    }
    else
    {
      record.RecalculateAlways = true;
      record.CalculateOnOpen = true;
      ptgArray = formulaUtil.ParsePtgArray(record.ParsedExpression, row - 1, col - 1, false, false);
      arrayFormulaPtg = record.ParsedExpression;
    }
    return isReturnArrayFormula ? $"{{={ptgArray}}}" : "=" + ptgArray;
  }

  public void SetValueRowCol(object value, int row, int col)
  {
    if (value != null && !this.m_bIsUnsupportedFormula && !value.Equals((object) "#NAME?"))
      this.SetValue(row, col, value.ToString());
    this.m_bIsUnsupportedFormula = false;
  }

  public void WireParentObject()
  {
  }

  public event Syncfusion.Calculate.ValueChangedEventHandler ValueChanged;

  public void OnValueChanged(int row, int col, string value)
  {
    if (this.ValueChanged == null)
      return;
    this.ValueChanged((object) this, new Syncfusion.Calculate.ValueChangedEventArgs(row, col, value));
  }

  static WorksheetImpl()
  {
    WorksheetImpl.m_hyperlinkPatterns.Add(new Regex(WorksheetImpl.m_linkPattern, RegexOptions.IgnoreCase));
    WorksheetImpl.m_hyperlinkPatterns.Add(new Regex(WorksheetImpl.m_mailPattern, RegexOptions.IgnoreCase));
  }

  public WorksheetImpl(IApplication application, object parent)
    : base(application, parent)
  {
  }

  [CLSCompliant(false)]
  public WorksheetImpl(
    IApplication application,
    object parent,
    BiffReader reader,
    ExcelParseOptions options,
    bool bSkipParsing,
    Dictionary<int, int> hashNewXFormatIndexes,
    IDecryptor decryptor)
    : base(application, parent, reader, options, bSkipParsing, hashNewXFormatIndexes, decryptor)
  {
  }

  protected override void InitializeCollections()
  {
    base.InitializeCollections();
    this.m_nameIndexChanged = new NameImpl.NameIndexChangedEventHandler(this.OnNameIndexChanged);
    this.m_dicRecordsCells = new CellRecordCollection(this.Application, (object) this);
    this.m_pageSetup = new PageSetupImpl(this.Application, (object) this);
    if (this.Application.DefaultVersion != ExcelVersion.Excel97to2003)
      this.m_pageSetup.DefaultRowHeight = (int) this.Application.StandardHeight * 20;
    this.m_names = new WorksheetNamesCollection(this.Application, (object) this);
    this.m_autofilters = new AutoFiltersCollection(this.Application, (object) this);
    this.m_arrConditionalFormats = new WorksheetConditionalFormats(this.Application, (object) this);
    this.m_errorIndicators = new ErrorIndicatorsCollection(this.Application, (object) this);
    this.m_arrColumnInfo = new ColumnInfoRecord[this.m_book.MaxColumnCount + 2];
    this.m_bOptimizeImport = this.Application.OptimizeImport;
    this.Index = this.m_book.Worksheets.Count;
    this.m_arrSelections = new List<SelectionRecord>();
    this.StandardWidth = this.Application.StandardWidth;
    this.StandardHeight = this.Application.StandardHeight;
    this.StandardHeightFlag = this.Application.StandardHeightFlag;
    this.CustomHeight = false;
    this.AttachEvents();
  }

  protected void ClearAll() => this.ClearAll(ExcelWorksheetCopyFlags.CopyAll);

  protected override void ClearAll(ExcelWorksheetCopyFlags flags)
  {
    this.m_dicRecordsCells.Clear();
    this.m_arrSelections.Clear();
    if ((flags & ExcelWorksheetCopyFlags.CopyNames) != ExcelWorksheetCopyFlags.None)
      this.m_names.Clear();
    base.ClearAll(flags);
    if (this.m_autofilters != null)
    {
      foreach (AutoFilterImpl autofilter in (CollectionBase<object>) this.m_autofilters)
        autofilter?.Dispose();
      this.m_autofilters.Clear();
    }
    if (this.m_hyperlinks != null)
      this.m_hyperlinks.Clear();
    if (this.m_arrCustomProperties == null)
      return;
    this.m_arrCustomProperties.Clear();
  }

  internal void CopyStyles(IWorksheet worksheet, Dictionary<int, int> hashExtFormatIndexes)
  {
    Dictionary<int, int> usedCellStyleIndex = (worksheet.Workbook as WorkbookImpl).m_usedCellStyleIndex;
    if (usedCellStyleIndex.Count <= 0 || (worksheet.Workbook as WorkbookImpl).m_usedCellStyleIndex == this.m_book.m_usedCellStyleIndex)
      return;
    foreach (int key1 in usedCellStyleIndex.Keys)
    {
      if (hashExtFormatIndexes.ContainsKey(key1))
      {
        int key2 = 0;
        hashExtFormatIndexes.TryGetValue(key1, out key2);
        int num = 0;
        usedCellStyleIndex.TryGetValue(key1, out num);
        if (!this.m_book.m_usedCellStyleIndex.ContainsKey(key2))
          this.m_book.m_usedCellStyleIndex.Add(key2, num);
      }
    }
    this.m_book.m_bisStylesCopied = true;
  }

  protected void CopyNames(
    WorksheetImpl basedOn,
    Dictionary<string, string> hashNewSheetNames,
    Dictionary<int, int> hashNewNameIndexes,
    Dictionary<int, int> hashExternSheetIndexes)
  {
    if (basedOn == null)
      throw new ArgumentNullException(nameof (basedOn));
    for (int index = this.m_names.Count - 1; index >= 0; --index)
      this.m_names[index].Delete();
    if (hashNewSheetNames == null)
    {
      hashNewSheetNames = new Dictionary<string, string>();
      hashNewSheetNames.Add(this.Name, basedOn.Name);
    }
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    this.m_names.FillFrom(basedOn.m_names, (IDictionary) hashNewSheetNames, hashNewNameIndexes, ExcelNamesMergeOptions.MakeLocal, hashExternSheetIndexes);
    WorkbookNamesCollection names1 = basedOn.Workbook.Names as WorkbookNamesCollection;
    WorkbookNamesCollection names2 = this.Workbook.Names as WorkbookNamesCollection;
    int index1 = 0;
    for (int count = names1.Count; index1 < count; ++index1)
    {
      NameImpl inner = (NameImpl) names1.InnerList[index1];
      if (!inner.IsLocal && (!basedOn.Names.Contains(inner.Name) || this.Workbook != basedOn.Workbook))
      {
        IRange refersToRange = inner.RefersToRange;
        int index2 = inner.Index;
        if (refersToRange == null)
        {
          if (!names2.Contains(inner.Name))
          {
            NameRecord name1 = (NameRecord) inner.Record.Clone();
            WorksheetNamesCollection.UpdateReferenceIndexes(name1, inner.Workbook, (IDictionary) hashNewSheetNames, hashExternSheetIndexes, this.m_book);
            IName name2 = names2.Add(name1);
            if (!inner.m_isTableNamedRange && basedOn.Workbook != this.Workbook && this.m_book.GetFilePath(name2.Value) == "'" + this.m_book.GetFilePath(this.m_book.FullFileName) && this.m_book.GetFileName(name2.Value).Contains(this.m_book.GetFileName(this.m_book.FullFileName)))
              this.ChangeReferenceIndex(basedOn.Workbook as WorkbookImpl, this.Workbook as WorkbookImpl, inner.Record, (name2 as NameImpl).Record);
            hashNewNameIndexes[index2] = (name2 as NameImpl).Index;
          }
        }
        else if (refersToRange.Worksheet == basedOn)
        {
          try
          {
            IName name = names2.AddCopy((IName) inner, (IWorksheet) this, hashExternSheetIndexes, (IDictionary) hashNewSheetNames);
            hashNewNameIndexes[index2] = (name as NameImpl).Index;
          }
          catch (Exception ex)
          {
            if (!names2.Contains(inner.Name))
            {
              NameRecord name3 = (NameRecord) inner.Record.Clone();
              WorksheetNamesCollection.UpdateReferenceIndexes(name3, inner.Workbook, (IDictionary) hashNewSheetNames, hashExternSheetIndexes, this.m_book);
              IName name4 = names2.Add(name3);
              hashNewNameIndexes[index2] = (name4 as NameImpl).Index;
            }
          }
        }
      }
    }
    int index3 = 0;
    for (int count = names1.Count; index3 < count; ++index3)
    {
      NameImpl inner = (NameImpl) names1.InnerList[index3];
      if (!inner.IsLocal && inner.Record.FormulaTokens != null)
      {
        foreach (Ptg formulaToken in inner.Record.FormulaTokens)
        {
          if (formulaToken.TokenCode == FormulaToken.tName1 || formulaToken.TokenCode == FormulaToken.tName2 || formulaToken.TokenCode == FormulaToken.tName3)
          {
            NameImpl nameImpl1 = (NameImpl) names1[(formulaToken as NamePtg).ExternNameIndexInt - 1];
            NameImpl nameImpl2 = (NameImpl) names2[nameImpl1.Name];
            if (nameImpl2 != null)
              dictionary[(formulaToken as NamePtg).ExternNameIndexInt] = nameImpl2.Index;
          }
        }
      }
    }
    Dictionary<int, object> usedNames = basedOn.GetUsedNames();
    foreach (int key in usedNames.Keys)
    {
      if (hashNewNameIndexes.ContainsKey(key))
      {
        NameImpl nameByIndex = (NameImpl) names2.GetNameByIndex(hashNewNameIndexes[key]);
        if (nameByIndex.Record.FormulaTokens != null)
        {
          foreach (Ptg formulaToken in nameByIndex.Record.FormulaTokens)
          {
            if (formulaToken.TokenCode == FormulaToken.tName1 || formulaToken.TokenCode == FormulaToken.tName2 || formulaToken.TokenCode == FormulaToken.tName3)
            {
              if (dictionary.ContainsKey((formulaToken as NamePtg).ExternNameIndexInt))
                (formulaToken as NamePtg).ExternNameIndexInt = (int) (ushort) (dictionary[(formulaToken as NamePtg).ExternNameIndexInt] + 1);
              else if (hashNewNameIndexes.ContainsKey((formulaToken as NamePtg).ExternNameIndexInt))
              {
                ushort hashNewNameIndex = (ushort) hashNewNameIndexes[(formulaToken as NamePtg).ExternNameIndexInt];
                (formulaToken as NamePtg).ExternNameIndexInt = (int) hashNewNameIndex;
              }
            }
          }
        }
      }
    }
    foreach (int key in usedNames.Keys)
    {
      if (!hashNewNameIndexes.ContainsKey(key))
      {
        NameImpl nameToCopy = (NameImpl) names1[key];
        IName name = names2.AddCopy((IName) nameToCopy, (IWorksheet) this, hashExternSheetIndexes, (IDictionary) hashNewSheetNames);
        if (!nameToCopy.m_isTableNamedRange && basedOn.Workbook != this.Workbook)
          this.ChangeReferenceIndex(basedOn.Workbook as WorkbookImpl, this.Workbook as WorkbookImpl, nameToCopy.Record, (name as NameImpl).Record);
        hashNewNameIndexes[key] = (name as NameImpl).Index;
      }
    }
  }

  private Dictionary<int, object> GetUsedNames()
  {
    ArrayListEx rows = this.m_dicRecordsCells.Table.Rows;
    Dictionary<int, object> result = new Dictionary<int, object>();
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
      rows[iFirstRow - 1]?.GetUsedNames(result);
    return result;
  }

  private void ChangeReferenceIndex(
    WorkbookImpl sourceBook,
    WorkbookImpl destBook,
    NameRecord oldName,
    NameRecord newName)
  {
    if (sourceBook.ExternSheet.RefList == null)
      return;
    Ptg[] formulaTokens1 = oldName.FormulaTokens;
    Ptg[] formulaTokens2 = newName.FormulaTokens;
    if (formulaTokens1 == null || formulaTokens2 == null)
      return;
    int index = 0;
    for (int length = formulaTokens2.Length; index < length; ++index)
    {
      if (formulaTokens2[index] is IReference reference)
      {
        int refIndex = (int) (formulaTokens1[index] as IReference).RefIndex;
        int iNewRefIndex = (int) reference.RefIndex;
        if (refIndex < sourceBook.ExternSheet.RefList.Count)
        {
          if (!sourceBook.IsExternalReference(refIndex))
          {
            iNewRefIndex = RowStorage.ChangeExternSheet(sourceBook, destBook, (int) sourceBook.ExternSheet.RefList[refIndex].FirstSheet, iNewRefIndex);
          }
          else
          {
            ExternWorkbookImpl externWorkbook = sourceBook.ExternWorkbooks[sourceBook.GetBookIndex(refIndex)];
            string str = destBook.IsCreated ? destBook.GetWorkbookName(destBook) : destBook.FullFileName;
            if (str == externWorkbook.URL || sourceBook.FullFileName != null && str == sourceBook.GetFilePath(sourceBook.FullFileName) + externWorkbook.URL)
            {
              int firstSheet = (int) sourceBook.ExternSheet.RefList[refIndex].FirstSheet;
              string sheetName = externWorkbook.GetSheetName(firstSheet);
              if (sheetName != null && sheetName != string.Empty)
                iNewRefIndex = destBook.AddSheetReference(sheetName);
            }
          }
          reference.RefIndex = (ushort) iNewRefIndex;
        }
      }
    }
  }

  protected void CopyRowHeight(WorksheetImpl sourceSheet, Dictionary<int, int> hashExtFormatIndexes)
  {
    if (sourceSheet == null)
      throw new ArgumentNullException(nameof (sourceSheet));
  }

  protected void CopyConditionalFormats(WorksheetImpl sourceSheet)
  {
    if (sourceSheet == null)
      throw new ArgumentNullException(nameof (sourceSheet));
    sourceSheet.ParseSheetCF();
    this.ParseSheetCF();
    this.m_arrConditionalFormats.CopyFrom(sourceSheet.m_arrConditionalFormats);
    if (this.m_arrConditionalFormats.Count <= 0)
      return;
    this.m_parseCondtionalFormats = false;
  }

  protected void CopyAutoFilters(WorksheetImpl sourceSheet)
  {
    List<BiffRecordRaw> biffRecordRawList = sourceSheet != null ? sourceSheet.m_arrAutoFilter : throw new ArgumentNullException(nameof (sourceSheet));
    if (biffRecordRawList != null)
    {
      List<BiffRecordRaw> autoFilterRecords = this.AutoFilterRecords;
      int index = 0;
      for (int count = biffRecordRawList.Count; index < count; ++index)
        autoFilterRecords.Add((BiffRecordRaw) CloneUtils.CloneCloneable((ICloneable) biffRecordRawList[index]));
    }
    if (sourceSheet.m_arrAutoFilter == null && sourceSheet.m_autofilters.Count <= 0)
      return;
    this.m_autofilters = sourceSheet.m_autofilters.Clone(this);
  }

  protected void CopyDataValidations(WorksheetImpl sourceSheet)
  {
    DataValidationTable dataValidationTable = sourceSheet != null ? sourceSheet.m_dataValidation : throw new ArgumentNullException(nameof (sourceSheet));
    if (dataValidationTable == null)
      return;
    this.m_dataValidation = (DataValidationTable) dataValidationTable.Clone((object) this);
  }

  protected void CopyColumnWidth(
    WorksheetImpl sourceSheet,
    Dictionary<int, int> hashExtFormatIndexes)
  {
    ColumnInfoRecord[] sourceArray = sourceSheet != null ? CloneUtils.CloneArray(sourceSheet.m_arrColumnInfo) : throw new ArgumentNullException(nameof (sourceSheet));
    int length1 = Math.Min(sourceArray.Length, this.m_arrColumnInfo.Length);
    Array.Copy((Array) sourceArray, (Array) this.m_arrColumnInfo, length1);
    this.UpdateIndexes((ICollection) this.m_arrColumnInfo, sourceSheet, hashExtFormatIndexes);
    if (hashExtFormatIndexes == null)
      return;
    int defaultXfIndex = sourceSheet.ParentWorkbook.DefaultXFIndex;
    int iXFIndex;
    if (!hashExtFormatIndexes.TryGetValue(defaultXfIndex, out iXFIndex))
      return;
    List<int> arrIsDefaultColumnWidth = (List<int>) null;
    if (iXFIndex != defaultXfIndex)
      arrIsDefaultColumnWidth = this.CreateColumnsOnUpdate(this.m_arrColumnInfo, iXFIndex);
    double d = double.NaN;
    int num = -1;
    int startIndex = 0;
    int index = 1;
    for (int length2 = this.m_arrColumnInfo.Length; index < length2; ++index)
    {
      ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[index];
      if (columnInfoRecord != null)
      {
        if (this.IsDefaultColumnWidth(arrIsDefaultColumnWidth, ref startIndex, index))
        {
          if (num < 0)
          {
            int columnWidth = (int) columnInfoRecord.ColumnWidth;
            int columnWidthInPixels = sourceSheet.GetColumnWidthInPixels(index);
            this.SetColumnWidthInPixels(index, columnWidthInPixels);
            num = (int) columnInfoRecord.ColumnWidth;
          }
          else
            columnInfoRecord.ColumnWidth = (ushort) num;
        }
        else if (double.IsNaN(d))
        {
          int columnWidth = (int) columnInfoRecord.ColumnWidth;
          int columnWidthInPixels = sourceSheet.GetColumnWidthInPixels(index);
          this.SetColumnWidthInPixels(index, columnWidthInPixels);
          d = (double) columnInfoRecord.ColumnWidth / (double) columnWidth;
        }
        else
          columnInfoRecord.ColumnWidth = (ushort) ((double) columnInfoRecord.ColumnWidth * d);
      }
    }
  }

  private bool IsDefaultColumnWidth(
    List<int> arrIsDefaultColumnWidth,
    ref int startIndex,
    int columnIndex)
  {
    if (arrIsDefaultColumnWidth == null)
      return false;
    int count = arrIsDefaultColumnWidth.Count;
    if (count == 0 || startIndex >= count)
      return false;
    int num;
    for (num = arrIsDefaultColumnWidth[startIndex]; num < columnIndex; num = arrIsDefaultColumnWidth[startIndex])
    {
      ++startIndex;
      if (startIndex >= count)
        return false;
    }
    return num == columnIndex;
  }

  private void UpdateIndexes(
    ICollection collection,
    WorksheetImpl sourceSheet,
    Dictionary<int, int> hashExtFormatIndexes)
  {
    this.UpdateIndexes(collection, sourceSheet, hashExtFormatIndexes, true);
  }

  private void UpdateIndexes(
    ICollection collection,
    WorksheetImpl sourceSheet,
    Dictionary<int, int> hashExtFormatIndexes,
    bool bUpdateDefault)
  {
    if (collection == null)
      throw new ArgumentNullException(nameof (collection));
    if (sourceSheet.Workbook == this.Workbook)
      return;
    WorkbookImpl parentWorkbook = sourceSheet.ParentWorkbook;
    int defaultXfIndex = this.m_book.DefaultXFIndex;
    foreach (IOutline outline in (IEnumerable) collection)
    {
      if (outline != null)
      {
        int extendedFormatIndex = (int) outline.ExtendedFormatIndex;
        if (hashExtFormatIndexes.ContainsKey(extendedFormatIndex))
        {
          int hashExtFormatIndex = hashExtFormatIndexes[extendedFormatIndex];
          if (bUpdateDefault || hashExtFormatIndex == defaultXfIndex)
            outline.ExtendedFormatIndex = (ushort) hashExtFormatIndex;
        }
      }
    }
  }

  private void UpdateOutlineIndexes(ICollection collection, int[] extFormatIndexes)
  {
    if (collection == null)
      throw new ArgumentNullException(nameof (collection));
    foreach (IOutline outline in (IEnumerable) collection)
    {
      if (outline != null)
      {
        int extendedFormatIndex = (int) outline.ExtendedFormatIndex;
        int extFormatIndex = extFormatIndexes[extendedFormatIndex];
        outline.ExtendedFormatIndex = (ushort) extFormatIndex;
      }
    }
  }

  private List<int> CreateColumnsOnUpdate(ColumnInfoRecord[] columns, int iXFIndex)
  {
    if (columns == null)
      throw new ArgumentNullException(nameof (columns));
    List<int> columnsOnUpdate = new List<int>();
    for (int index = 1; index <= this.m_book.MaxColumnCount; ++index)
    {
      if (columns[index] == null)
      {
        ColumnInfoRecord record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
        record.FirstColumn = record.LastColumn = (ushort) (index - 1);
        record.ExtendedFormatIndex = (ushort) iXFIndex;
        columns[index] = record;
        columnsOnUpdate.Add(index);
      }
    }
    return columnsOnUpdate;
  }

  protected void CopyMerges(WorksheetImpl sourceSheet)
  {
    MergeCellsImpl toClone = sourceSheet != null ? sourceSheet.MergeCells : throw new ArgumentNullException(nameof (sourceSheet));
    if (toClone == null)
      return;
    this.m_mergedCells = (MergeCellsImpl) CloneUtils.CloneCloneable((ICloneParent) toClone, (object) this);
  }

  protected void AttachEvents()
  {
    if (!this.m_book.Styles.Contains("Normal") && this.m_book.Loading)
      return;
    (this.m_book.Styles["Normal"].Font as FontWrapper).AfterChangeEvent += new EventHandler(this.NormalFont_OnAfterChange);
  }

  protected void DetachEvents()
  {
    if (this.m_book == null || this.m_book.Styles == null || !this.m_book.Styles.Contains("Normal"))
      return;
    (this.m_book.Styles["Normal"].Font as FontWrapper).AfterChangeEvent -= new EventHandler(this.NormalFont_OnAfterChange);
  }

  protected override void OnDispose()
  {
    if (this.m_bIsDisposed)
      return;
    this.DetachEvents();
    this.m_arrColumnInfo = (ColumnInfoRecord[]) null;
    this.m_arrCustomProperties = (WorksheetCustomProperties) null;
    this.m_arrDConRecords = (List<BiffRecordRaw>) null;
    this.m_arrNotes = (SortedList<int, NoteRecord>) null;
    this.m_arrNotesByCellIndex = (SortedList<long, NoteRecord>) null;
    this.m_arrRecords = (List<BiffRecordRaw>) null;
    this.m_arrSortRecords = (List<BiffRecordRaw>) null;
    this.m_autofilters = (AutoFiltersCollection) null;
    if (this.columnCollection != null)
    {
      this.columnCollection.Clear();
      this.columnCollection = (ColumnCollection) null;
    }
    if (this.m_arrAutoFilter != null)
    {
      this.m_arrAutoFilter.Clear();
      this.m_arrAutoFilter = (List<BiffRecordRaw>) null;
    }
    if (this.m_arrSelections != null)
    {
      this.m_arrSelections.Clear();
      this.m_arrSelections = (List<SelectionRecord>) null;
    }
    if (this.m_arrConditionalFormats != null)
    {
      this.m_arrConditionalFormats.Dispose();
      this.m_arrConditionalFormats = (WorksheetConditionalFormats) null;
    }
    if (this.m_autoFitManager != null)
      this.m_autoFitManager.Dispose();
    this.m_bof = (BOFRecord) null;
    if (this.m_dataHolder != null && this.m_book == null)
    {
      this.m_dataHolder.Dispose();
      this.m_dataHolder = (WorksheetDataHolder) null;
    }
    this.m_dataValidation = (DataValidationTable) null;
    if (this.m_dicRecordsCells != null)
    {
      this.m_dicRecordsCells.Dispose();
      this.m_dicRecordsCells = (CellRecordCollection) null;
    }
    this.m_dataValidation = (DataValidationTable) null;
    this.m_dictCFExRecords = (Dictionary<int, CFExRecord>) null;
    this.m_dictCondFMT = (Dictionary<int, CondFMTRecord>) null;
    this.m_errorIndicators = (ErrorIndicatorsCollection) null;
    this.m_hyperlinks = (HyperLinksCollection) null;
    if (this.m_inlineStrings != null)
    {
      this.m_inlineStrings.Clear();
      this.m_inlineStrings = (Dictionary<string, string>) null;
    }
    if (this.m_listObjects != null)
    {
      this.m_listObjects.Dispose();
      this.m_listObjects = (ListObjectCollection) null;
    }
    if (this.m_mergedCells != null)
    {
      this.m_mergedCells.Dispose();
      this.m_mergedCells = (MergeCellsImpl) null;
    }
    this.m_migrantRange = (IMigrantRange) null;
    this.m_nameIndexChanged = (NameImpl.NameIndexChangedEventHandler) null;
    if (this.m_oleObjects != null)
      this.m_oleObjects.Clear();
    this.m_pane = (PaneRecord) null;
    if (this.m_pivotTables != null)
      this.m_pivotTables.Clear();
    this.m_preserveExternalConnection = (List<BiffRecordRaw>) null;
    if (this.m_preservePivotTables != null)
      this.m_preservePivotTables.Clear();
    this.m_rawColRecord = (ColumnInfoRecord) null;
    this.m_rowHeightHelper = (ItemSizeHelper) null;
    if (this.m_sparklineGroups != null)
      this.m_sparklineGroups.Clear();
    if (this.m_tableRecords != null)
      this.m_tableRecords.Clear();
    this.m_worksheetSlicer = (Stream) null;
    if (this.ValueChanged != null)
      this.ValueChanged = (Syncfusion.Calculate.ValueChangedEventHandler) null;
    if (this.m_book != null)
      this.m_book.EnabledCalcEngine = false;
    if (this.m_dicRecordsCells != null)
    {
      this.m_dicRecordsCells.Dispose();
      this.m_dicRecordsCells = (CellRecordCollection) null;
    }
    if (this.m_pageSetup != null)
      this.m_pageSetup.Dispose();
    if (this.m_dataSorter != null)
      this.m_dataSorter = (IDataSort) null;
    this.RowHeightChanged = (ValueChangedEventHandler) null;
    this.ColumnWidthChanged = (ValueChangedEventHandler) null;
    if (this.m_formulaValues != null)
    {
      this.m_formulaValues.Clear();
      this.m_formulaValues = (Dictionary<long, string>) null;
    }
    GC.SuppressFinalize((object) this);
  }

  protected void CopyPageSetup(WorksheetImpl sourceSheet)
  {
    this.m_pageSetup = sourceSheet != null ? sourceSheet.m_pageSetup.Clone((object) this) : throw new ArgumentNullException(nameof (sourceSheet));
  }

  protected int ImportExtendedFormat(
    int iXFIndex,
    WorkbookImpl basedOn,
    Dictionary<int, int> hashExtFormatIndexes)
  {
    return this.m_book.InnerExtFormats.Import(basedOn.InnerExtFormats[iXFIndex], hashExtFormatIndexes);
  }

  protected internal override void UpdateStyleIndexes(int[] styleIndexes)
  {
    this.UpdateOutlineIndexes((ICollection) this.m_arrColumnInfo, styleIndexes);
    this.m_dicRecordsCells.UpdateExtendedFormatIndex(styleIndexes);
  }

  internal bool IsInsertingSubTotal
  {
    get => this.m_isInsertingSubTotal;
    set => this.m_isInsertingSubTotal = value;
  }

  internal bool IsRemovingSubTotal
  {
    get => this.m_isRemovingSubTotal;
    set => this.m_isRemovingSubTotal = value;
  }

  internal bool IsArrayFormulaSeparated
  {
    get => this.m_isArrayFormulaSeparated;
    set => this.m_isArrayFormulaSeparated = value;
  }

  internal CellRecordCollection RecordsCells => this.m_dicRecordsCells;

  internal List<int> InsertedRows
  {
    get => this.m_insertedRows;
    set => this.m_insertedRows = value;
  }

  internal SortedDictionary<int, int> MovedRows
  {
    get => this.m_movedRows;
    set => this.m_movedRows = value;
  }

  internal Dictionary<long, string> FormulaValues
  {
    get
    {
      if (this.m_formulaValues == null)
        this.m_formulaValues = new Dictionary<long, string>();
      return this.m_formulaValues;
    }
  }

  internal Dictionary<int, int> IndexAndLevels
  {
    get
    {
      if (this.m_indexAndLevels == null)
        this.m_indexAndLevels = new Dictionary<int, int>();
      return this.m_indexAndLevels;
    }
    set => this.m_indexAndLevels = value;
  }

  internal Stream WorksheetSlicerStream
  {
    get => this.m_worksheetSlicer;
    set => this.m_worksheetSlicer = value;
  }

  internal bool HasHyperlinks => this.m_hyperlinks != null;

  internal double DefaultColumnWidth
  {
    set => this.m_defaultColWidth = value;
    get => this.m_defaultColWidth;
  }

  public MergeCellsImpl MergeCells
  {
    get
    {
      this.ParseData();
      if (this.m_mergedCells == null)
        this.m_mergedCells = new MergeCellsImpl(this.Application, (object) this);
      return this.m_mergedCells;
    }
  }

  internal RichTextReader RichTextReader
  {
    get
    {
      this.m_richTextReader = new RichTextReader((IWorksheet) this);
      return this.m_richTextReader;
    }
  }

  [CLSCompliant(false)]
  public ColumnInfoRecord[] ColumnInformation
  {
    get
    {
      this.ParseData();
      return this.m_arrColumnInfo;
    }
  }

  public int VerticalSplit
  {
    get
    {
      this.ParseData();
      return this.m_pane == null ? 0 : this.m_pane.VerticalSplit;
    }
    set
    {
      this.ParseData();
      if (this.m_pane == null)
        this.CreateEmptyPane();
      this.m_pane.VerticalSplit = (int) (ushort) value;
    }
  }

  public int HorizontalSplit
  {
    get
    {
      this.ParseData();
      return this.m_pane == null ? 0 : this.m_pane.HorizontalSplit;
    }
    set
    {
      this.ParseData();
      if (this.m_pane == null)
        this.CreateEmptyPane();
      this.m_pane.HorizontalSplit = (int) (ushort) value;
    }
  }

  public int FirstVisibleRow
  {
    get
    {
      this.ParseData();
      return this.m_pane == null ? 0 : this.m_pane.FirstRow;
    }
    set
    {
      this.ParseData();
      if (this.m_pane == null)
        this.CreateEmptyPane();
      this.m_pane.FirstRow = (int) (ushort) value;
    }
  }

  internal int MaxColumnWidth => (int) byte.MaxValue;

  public int FirstVisibleColumn
  {
    get
    {
      this.ParseData();
      return this.m_pane == null ? 0 : this.m_pane.FirstColumn;
    }
    set
    {
      this.ParseData();
      if (this.m_pane == null)
        this.CreateEmptyPane();
      this.m_pane.FirstColumn = (int) (ushort) value;
    }
  }

  public IRange PrintArea => this.UsedRange;

  public int SelectionCount
  {
    get
    {
      this.ParseData();
      int selectionCount = 1;
      if (this.m_pane != null)
      {
        if (this.m_pane.VerticalSplit != 0)
          selectionCount *= 2;
        if (this.m_pane.HorizontalSplit != 0)
          selectionCount *= 2;
      }
      return selectionCount;
    }
  }

  public DataValidationTable DVTable
  {
    get
    {
      this.ParseData();
      if (this.m_dataValidation == null)
        this.m_dataValidation = new DataValidationTable(this.Application, (object) this);
      return this.m_dataValidation;
    }
  }

  public IAutoFilters AutoFilters
  {
    get
    {
      this.ParseData();
      return (IAutoFilters) this.m_autofilters;
    }
  }

  public HyperLinksCollection InnerHyperLinks
  {
    get
    {
      this.ParseData();
      if (this.m_hyperlinks == null)
        this.m_hyperlinks = new HyperLinksCollection(this.Application, (object) this);
      return this.m_hyperlinks;
    }
  }

  public HyperLinksCollection InnerHyperLinksOrNull
  {
    get
    {
      this.ParseData();
      return this.m_hyperlinks;
    }
  }

  public SheetView View
  {
    get => this.m_view;
    set
    {
      this.m_view = value;
      if (this.m_view == SheetView.PageBreakPreview)
        this.WindowTwo.IsSavedInPageBreakPreview = true;
      else
        this.WindowTwo.IsSavedInPageBreakPreview = false;
    }
  }

  public int DefaultRowHeight
  {
    get
    {
      this.ParseData();
      return this.m_pageSetup.DefaultRowHeight;
    }
    set
    {
      this.ParseData();
      if (this.m_pageSetup.DefaultRowHeight == value)
        return;
      int defaultRowHeight = this.m_pageSetup.DefaultRowHeight;
      if (this.m_iFirstRow >= 0 && this.m_iLastRow >= 0)
      {
        for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
        {
          RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iFirstRow, false);
          if (row != null && !row.IsBadFontHeight && (int) row.Height == defaultRowHeight)
          {
            row.Height = (ushort) value;
            row.IsBadFontHeight = true;
          }
        }
      }
      this.m_pageSetup.DefaultRowHeight = value;
    }
  }

  public WorksheetNamesCollection InnerNames => this.m_names;

  public DataValidationTable InnerDVTable => this.m_dataValidation;

  public CellRecordCollection CellRecords
  {
    [DebuggerStepThrough] get
    {
      this.ParseData();
      return this.m_dicRecordsCells;
    }
  }

  public override PageSetupBaseImpl PageSetupBase
  {
    get
    {
      this.ParseData();
      return (PageSetupBaseImpl) this.m_pageSetup;
    }
  }

  public WorksheetConditionalFormats ConditionalFormats
  {
    get
    {
      this.ParseData();
      if (this.m_parseCondtionalFormats)
        this.ParseSheetCF();
      return this.m_arrConditionalFormats;
    }
  }

  [CLSCompliant(false)]
  public PaneRecord Pane
  {
    get
    {
      this.ParseData();
      if (this.m_pane == null)
        this.m_pane = (PaneRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Pane);
      return this.m_pane;
    }
  }

  [CLSCompliant(false)]
  public List<SelectionRecord> Selections
  {
    get
    {
      this.ParseData();
      return this.m_arrSelections;
    }
  }

  public WorksheetCustomProperties InnerCustomProperties
  {
    get
    {
      this.ParseData();
      return this.m_arrCustomProperties;
    }
  }

  public bool UseRangesCache
  {
    get
    {
      this.ParseData();
      return this.m_dicRecordsCells.UseCache;
    }
    set
    {
      this.ParseData();
      this.m_dicRecordsCells.UseCache = value;
    }
  }

  private List<BiffRecordRaw> AutoFilterRecords
  {
    get
    {
      this.ParseData();
      if (this.m_arrAutoFilter == null)
        this.m_arrAutoFilter = new List<BiffRecordRaw>();
      return this.m_arrAutoFilter;
    }
  }

  private List<BiffRecordRaw> DConRecords
  {
    get
    {
      this.ParseData();
      if (this.m_arrDConRecords == null)
        this.m_arrDConRecords = new List<BiffRecordRaw>();
      return this.m_arrDConRecords;
    }
  }

  private List<BiffRecordRaw> SortRecords
  {
    get
    {
      this.ParseData();
      if (this.m_arrSortRecords == null)
        this.m_arrSortRecords = new List<BiffRecordRaw>();
      return this.m_arrSortRecords;
    }
  }

  public ErrorIndicatorsCollection ErrorIndicators
  {
    get
    {
      this.ParseData();
      return this.m_errorIndicators;
    }
  }

  public string QuotedName
  {
    get
    {
      this.ParseData();
      return $"'{this.Name.Replace("'", "''")}'";
    }
  }

  public ExcelVersion Version
  {
    get => this.m_book.Version;
    set
    {
      if (this.m_iLastRow != -1)
        this.m_iLastRow = Math.Min(this.m_iLastRow, this.m_book.MaxRowCount);
      if (this.m_iFirstRow != -1)
        this.m_iFirstRow = Math.Min(this.m_iFirstRow, this.m_book.MaxRowCount);
      if (this.m_iFirstColumn != int.MaxValue)
        this.m_iFirstColumn = Math.Min(this.m_iFirstColumn, this.m_book.MaxColumnCount);
      if (this.m_iLastColumn != int.MaxValue)
        this.m_iLastColumn = Math.Min(this.m_iLastColumn, this.m_book.MaxColumnCount);
      ColumnInfoRecord[] arrColumnInfo = this.m_arrColumnInfo;
      this.m_arrColumnInfo = new ColumnInfoRecord[this.m_book.MaxColumnCount + 2];
      Array.Copy((Array) arrColumnInfo, 0, (Array) this.m_arrColumnInfo, 0, Math.Min(arrColumnInfo.Length, this.m_arrColumnInfo.Length));
      if (this.m_book.IsConverted && arrColumnInfo[arrColumnInfo.Length - 1] != null && this.m_rawColRecord != null)
      {
        for (int length = arrColumnInfo.Length; length < this.m_arrColumnInfo.Length; ++length)
        {
          ColumnInfoRecord columnInfoRecord = this.m_rawColRecord.Clone() as ColumnInfoRecord;
          columnInfoRecord.FirstColumn = (ushort) length;
          columnInfoRecord.LastColumn = (ushort) length;
          this.m_arrColumnInfo[length] = columnInfoRecord;
        }
      }
      this.m_dicRecordsCells.Version = value;
      HPageBreaksCollection hpageBreaks = (HPageBreaksCollection) this.HPageBreaks;
      if (hpageBreaks != null && value == ExcelVersion.Excel97to2003)
        hpageBreaks.ChangeToExcel97to03Version();
      VPageBreaksCollection vpageBreaks = (VPageBreaksCollection) this.VPageBreaks;
      if (vpageBreaks != null && value == ExcelVersion.Excel97to2003)
        vpageBreaks.ChangeToExcel97to03Version();
      if (value == ExcelVersion.Excel97to2003 && this.m_mergedCells != null)
        this.m_mergedCells.SetNewDimensions(this.m_book.MaxRowCount, this.m_book.MaxColumnCount);
      if (this.AutoFilters.Count != 0)
        ((AutoFiltersCollection) this.AutoFilters).ChangeVersions(this.m_book.MaxRowCount, this.m_book.MaxColumnCount, value);
      FileDataHolder dataHolder = ((WorkbookImpl) this.Workbook).DataHolder;
      if (value == ExcelVersion.Excel97to2003 && dataHolder != null)
      {
        this.ParseCFFromExcel2007(dataHolder);
        foreach (Syncfusion.XlsIO.Implementation.Collections.ConditionalFormats conditionalFormat in (CollectionBase<Syncfusion.XlsIO.Implementation.Collections.ConditionalFormats>) this.ConditionalFormats)
          conditionalFormat.ConvertToExcel97to03Version();
      }
      this.InnerNames?.ConvertFullRowColumnNames(value);
      if (this.m_pane != null && (this.m_pane.FirstRow > this.m_book.MaxRowCount - 1 || this.m_pane.FirstColumn > this.m_book.MaxColumnCount - 1))
        this.m_pane = (PaneRecord) null;
      this.InnerShapes?.SetVersion(value);
      if (this.Version != ExcelVersion.Excel97to2003)
        return;
      this.ClearPivotTables();
    }
  }

  private void ClearPivotTables()
  {
    if (this.m_pivotTables == null)
      return;
    this.m_pivotTables.Clear();
  }

  public RecordExtractor RecordExtractor => this.CellRecords.RecordExtractor;

  internal ItemSizeHelper RowHeightHelper
  {
    get
    {
      if (this.m_rowHeightHelper == null)
        this.m_rowHeightHelper = new ItemSizeHelper(new ItemSizeHelper.SizeGetter(this.GetRowHeightInPixels));
      return this.m_rowHeightHelper;
    }
  }

  internal bool IsVisible
  {
    get => this.m_isRowHeightSet;
    set => this.m_isRowHeightSet = value;
  }

  internal bool IsZeroHeight
  {
    get => this.m_isZeroHeight;
    set => this.m_isZeroHeight = value;
  }

  internal int BaseColumnWidth
  {
    get => this.m_baseColumnWidth;
    set => this.m_baseColumnWidth = value;
  }

  internal bool IsThickBottom
  {
    get => this.m_isThickBottom;
    set => this.m_isThickBottom = value;
  }

  internal bool IsThickTop
  {
    get => this.m_isThickTop;
    set => this.m_isThickTop = value;
  }

  internal byte OutlineLevelColumn
  {
    get => this.m_outlineLevelColumn;
    set => this.m_outlineLevelColumn = value;
  }

  internal byte OutlineLevelRow
  {
    get => this.m_outlineLevelRow;
    set => this.m_outlineLevelRow = value;
  }

  internal bool CustomHeight
  {
    get => this.m_isCustomHeight;
    set => this.m_isCustomHeight = value;
  }

  public int RowsOutlineLevel
  {
    get
    {
      this.OutlineLevelRow = (byte) this.RowOutlineLevels.Count;
      return (int) this.OutlineLevelRow;
    }
  }

  public int ColumnsOutlineLevel
  {
    get
    {
      this.OutlineLevelColumn = (byte) this.ColumnOutlineLevels.Count;
      return (int) this.OutlineLevelColumn;
    }
  }

  public List<IOutlineWrapper> OutlineWrappers
  {
    get
    {
      this.UpdateOutlineRowStorage();
      this.UpdateColumnOutline();
      this.OutlineWrappers = new List<IOutlineWrapper>();
      this.SortGroups(this.ColumnOutlineLevels);
      this.SortGroups(this.RowOutlineLevels);
      this.CreateOutlineWrappers(this.ColumnOutlineLevels, ExcelGroupBy.ByColumns);
      this.CreateOutlineWrappers(this.RowOutlineLevels, ExcelGroupBy.ByRows);
      return this.m_outlineWrappers;
    }
    set => this.m_outlineWrappers = value;
  }

  internal bool IsInsertingRow => this.m_isInsertingRow;

  internal bool IsDeletingRow => this.m_isDeletingRow;

  internal Dictionary<long, CellFormula> CellFormulas
  {
    get => this.m_cellFormulas;
    set => this.m_cellFormulas = value;
  }

  internal void SortGroups(Dictionary<int, List<GroupPoint>> OutlineLevels)
  {
    this.AddGroupsinLevel(OutlineLevels);
    for (int key = 1; key <= OutlineLevels.Count; ++key)
    {
      List<GroupPoint> OutlineLevels1 = (List<GroupPoint>) null;
      OutlineLevels.TryGetValue(key, out OutlineLevels1);
      if (OutlineLevels1 != null)
      {
        if (OutlineLevels1.Count > 1)
          this.ArrangeGroups(OutlineLevels1);
        else if (OutlineLevels1.Count == 0)
          OutlineLevels.Remove(key);
      }
    }
  }

  internal void AddGroupsinLevel(Dictionary<int, List<GroupPoint>> OutlineLevels)
  {
    for (int count = OutlineLevels.Count; count > 1; --count)
    {
      List<GroupPoint> groupPointList1 = (List<GroupPoint>) null;
      OutlineLevels.TryGetValue(count - 1, out groupPointList1);
      List<GroupPoint> groupPointList2 = (List<GroupPoint>) null;
      OutlineLevels.TryGetValue(count, out groupPointList2);
      if (groupPointList2 != null)
      {
        for (int index = 0; index < groupPointList2.Count; ++index)
        {
          if (groupPointList2[index].IsParse)
          {
            GroupPoint groupPoint = new GroupPoint(groupPointList2[index].X, groupPointList2[index].Y);
            groupPoint.IsParse = true;
            groupPointList1.Add(groupPoint);
            groupPointList2.RemoveAt(index);
            groupPoint.IsParse = false;
            groupPointList2.Insert(index, groupPoint);
          }
        }
      }
    }
  }

  private void ArrangeGroups(List<GroupPoint> OutlineLevels)
  {
    for (int index1 = OutlineLevels.Count - 1; index1 > 0; --index1)
    {
      GroupPoint outlineLevel1 = OutlineLevels[index1];
      for (int index2 = index1 - 1; index2 >= 0; --index2)
      {
        GroupPoint outlineLevel2 = OutlineLevels[index2];
        if (outlineLevel2.Y == outlineLevel1.X - 1)
        {
          GroupPoint groupPoint = new GroupPoint(outlineLevel2.X, outlineLevel1.Y);
          OutlineLevels.RemoveAt(index1);
          OutlineLevels.RemoveAt(index2);
          OutlineLevels.Insert(index2, groupPoint);
          break;
        }
        if (outlineLevel1.Y == outlineLevel2.Y - 1)
        {
          GroupPoint groupPoint = new GroupPoint(outlineLevel1.X, outlineLevel2.Y);
          OutlineLevels.RemoveAt(index1);
          OutlineLevels.RemoveAt(index2);
          OutlineLevels.Insert(index2, groupPoint);
          break;
        }
        if (outlineLevel1.X == outlineLevel2.Y - 1)
        {
          GroupPoint groupPoint = new GroupPoint(outlineLevel1.X, outlineLevel2.Y);
          OutlineLevels.RemoveAt(index1);
          OutlineLevels.RemoveAt(index2);
          OutlineLevels.Insert(index2, groupPoint);
          break;
        }
        if (outlineLevel1.Y == outlineLevel2.X - 1)
        {
          GroupPoint groupPoint = new GroupPoint(outlineLevel1.X, outlineLevel2.Y);
          OutlineLevels.RemoveAt(index1);
          OutlineLevels.RemoveAt(index2);
          OutlineLevels.Insert(index2, groupPoint);
          break;
        }
      }
    }
  }

  internal void UpdateOutlineRowStorage()
  {
    CellRecordCollection cellRecords = this.CellRecords;
    Dictionary<int, List<GroupPoint>> outlines = new Dictionary<int, List<GroupPoint>>();
    ArrayListEx arrRows = cellRecords.Table.m_arrRows;
    int firstRow = cellRecords.FirstRow;
    for (int count = arrRows.GetCount(); firstRow <= count; ++firstRow)
    {
      if (cellRecords.ContainsRow(firstRow - 1))
      {
        int outlineLevel = (int) arrRows[firstRow - 1].OutlineLevel;
        if (outlineLevel != 0)
          this.UpdateOutline(firstRow, outlineLevel, outlines);
      }
    }
    this.RowOutlineLevels = new Dictionary<int, List<GroupPoint>>();
    for (int key = 1; key <= outlines.Count; ++key)
      this.RowOutlineLevels.Add(key, outlines[key]);
    this.OutlineLevelRow = (byte) outlines.Count;
  }

  internal void UpdateColumnOutline()
  {
    Dictionary<int, List<GroupPoint>> outlines = new Dictionary<int, List<GroupPoint>>();
    ColumnInfoRecord[] arrColumnInfo = this.m_arrColumnInfo;
    int index = 1;
    for (int maxColumnCount = this.m_book.MaxColumnCount; index <= maxColumnCount; ++index)
    {
      ColumnInfoRecord columnInfoRecord = arrColumnInfo[index];
      if (columnInfoRecord != null)
      {
        int outlineLevel = (int) columnInfoRecord.OutlineLevel;
        if (outlineLevel != 0)
          this.UpdateOutline(index, outlineLevel, outlines);
      }
    }
    this.ColumnOutlineLevels = new Dictionary<int, List<GroupPoint>>();
    for (int key = 1; key <= outlines.Count; ++key)
      this.ColumnOutlineLevels.Add(key, outlines[key]);
    this.OutlineLevelColumn = (byte) outlines.Count;
  }

  internal void UpdateOutline(int index, int level, Dictionary<int, List<GroupPoint>> outlines)
  {
    if (!outlines.ContainsKey(level))
    {
      outlines.Add(level, new List<GroupPoint>()
      {
        new GroupPoint(index, index)
      });
      if (level <= 1)
        return;
      this.UpdateInAllLevels(level, index, outlines);
    }
    else
      this.UpdateInAllLevels(level, index, outlines);
  }

  internal void UpdateInAllLevels(int level, int index, Dictionary<int, List<GroupPoint>> outlines)
  {
    for (int key = 1; key <= level; ++key)
    {
      if (outlines.ContainsKey(key))
      {
        List<GroupPoint> outline = outlines[key];
        int index1;
        GroupPoint groupPoint1;
        for (index1 = 0; index1 < outline.Count; ++index1)
        {
          GroupPoint groupPoint2 = outline[index1];
          if (groupPoint2.Y == index - 1)
          {
            groupPoint1 = new GroupPoint(groupPoint2.X, index);
            outline.RemoveAt(index1);
            outline.Add(groupPoint1);
            break;
          }
          if (groupPoint2.Y == index)
            break;
        }
        if (index1 == outline.Count)
        {
          groupPoint1 = new GroupPoint(index, index);
          outline.Add(groupPoint1);
        }
      }
      else
        outlines.Add(key, new List<GroupPoint>()
        {
          new GroupPoint(index, index)
        });
    }
  }

  public bool HasMergedCells => this.m_mergedCells != null && this.m_mergedCells.MergeCount > 0;

  public ListObjectCollection InnerListObjects => this.m_listObjects;

  protected override ExcelSheetProtection DefaultProtectionOptions
  {
    get
    {
      return ExcelSheetProtection.LockedCells | ExcelSheetProtection.UnLockedCells | ExcelSheetProtection.Content;
    }
  }

  protected override ExcelSheetProtection UnprotectedOptions => ExcelSheetProtection.Content;

  internal Dictionary<string, string> InlineStrings
  {
    get
    {
      if (this.m_inlineStrings == null)
        this.m_inlineStrings = new Dictionary<string, string>();
      return this.m_inlineStrings;
    }
  }

  internal List<BiffRecordRaw> PreserveExternalConnection
  {
    get
    {
      if (this.m_preserveExternalConnection == null)
        this.m_preserveExternalConnection = new List<BiffRecordRaw>();
      return this.m_preserveExternalConnection;
    }
  }

  internal List<Stream> PreservePivotTables
  {
    get
    {
      if (this.m_preservePivotTables == null)
        this.m_preservePivotTables = new List<Stream>();
      return this.m_preservePivotTables;
    }
  }

  internal override bool ParseDataOnDemand
  {
    get => this.m_bParseDataOnDemand;
    set => this.m_bParseDataOnDemand = value;
  }

  internal Dictionary<int, List<GroupPoint>> ColumnOutlineLevels
  {
    get
    {
      if (this.m_columnOutlineLevels == null)
        this.m_columnOutlineLevels = new Dictionary<int, List<GroupPoint>>();
      return this.m_columnOutlineLevels;
    }
    set => this.m_columnOutlineLevels = value;
  }

  internal Dictionary<int, List<GroupPoint>> RowOutlineLevels
  {
    get
    {
      if (this.m_rowOutlineLevels == null)
        this.m_rowOutlineLevels = new Dictionary<int, List<GroupPoint>>();
      return this.m_rowOutlineLevels;
    }
    set => this.m_rowOutlineLevels = value;
  }

  internal List<Rectangle> ImportMergeRanges
  {
    get => this.m_importMergeRanges;
    set => this.m_importMergeRanges = value;
  }

  internal IRanges ImportMergeRangeCollection
  {
    get => this.m_importMergeRangeCollection;
    set => this.m_importMergeRangeCollection = value;
  }

  public IRange this[int row, int column] => this.Range[row, column];

  public IRange this[int row, int column, int lastRow, int lastColumn]
  {
    get => this.Range[row, column, lastRow, lastColumn];
  }

  public IRange this[string name] => this[name, false];

  public IRange this[string name, bool IsR1C1Notation] => this.Range[name, IsR1C1Notation];

  public int ActivePane
  {
    get
    {
      this.Parse();
      return this.m_pane == null ? int.MinValue : (int) this.m_pane.ActivePane;
    }
    set
    {
      this.ParseData();
      if (value < 0 || value > 3)
        throw new ArgumentOutOfRangeException("Value must be 0 to 3");
      if (this.m_pane == null)
        this.CreateEmptyPane();
      this.m_pane.ActivePane = (ushort) value;
    }
  }

  public IRange[] Cells => this.UsedRange.Cells;

  internal ColumnCollection Columnss
  {
    get
    {
      if (this.columnCollection != null)
        return this.columnCollection;
      int num = this.GetAppImpl().GetFontCalc2() * 8 + this.GetAppImpl().GetFontCalc3();
      this.columnCollection = new ColumnCollection(this, 8.0 + (double) ((num / 8 + 1) * 8 - num) * 1.0 / (double) this.GetAppImpl().GetFontCalc2());
      return this.columnCollection;
    }
  }

  public IRange[] Columns => this.UsedRange.Columns;

  public bool DisplayPageBreaks
  {
    get
    {
      this.Parse();
      return this.m_bDisplayPageBreaks;
    }
    set
    {
      this.ParseData();
      if (this.m_bDisplayPageBreaks == value)
        return;
      this.SetChanged();
      this.m_bDisplayPageBreaks = value;
    }
  }

  internal AutoFitManager AutoFitManagerImpl
  {
    get
    {
      if (this.m_autoFitManager == null)
        this.m_autoFitManager = new AutoFitManager(this);
      return this.m_autoFitManager;
    }
    set => this.m_autoFitManager = value;
  }

  public IOleObjects OleObjects
  {
    get
    {
      if (this.m_oleObjects == null)
        this.m_oleObjects = new Syncfusion.XlsIO.Implementation.OleObjects(this);
      return (IOleObjects) this.m_oleObjects;
    }
  }

  public bool HasOleObject => this.m_oleObjects != null && this.m_oleObjects.Count > 0;

  public ISparklineGroups SparklineGroups
  {
    get
    {
      if (this.m_book.Loading && this.Version == ExcelVersion.Excel2007)
        this.m_book.Version = ExcelVersion.Excel2010;
      if (this.m_sparklineGroups == null)
        this.m_sparklineGroups = new Syncfusion.XlsIO.Implementation.SparklineGroups(this.m_book);
      return (ISparklineGroups) this.m_sparklineGroups;
    }
  }

  public IHPageBreaks HPageBreaks
  {
    get
    {
      this.ParseData();
      return (IHPageBreaks) this.m_pageSetup.HPageBreaks;
    }
  }

  public IHyperLinks HyperLinks => (IHyperLinks) this.InnerHyperLinks;

  public bool IsDisplayZeros
  {
    get
    {
      this.ParseData();
      return this.WindowTwo.IsDisplayZeros;
    }
    set
    {
      this.ParseData();
      this.WindowTwo.IsDisplayZeros = value;
    }
  }

  public bool IsGridLinesVisible
  {
    get
    {
      this.Parse();
      return this.WindowTwo.IsDisplayGridlines;
    }
    set
    {
      this.ParseData();
      this.WindowTwo.IsDisplayGridlines = value;
    }
  }

  public bool IsRowColumnHeadersVisible
  {
    get
    {
      this.ParseData();
      return this.WindowTwo.IsDisplayRowColHeadings;
    }
    set
    {
      this.ParseData();
      this.WindowTwo.IsDisplayRowColHeadings = value;
    }
  }

  public bool IsStringsPreserved
  {
    get
    {
      this.ParseData();
      return this.m_bStringsPreserved;
    }
    set
    {
      this.ParseData();
      this.m_stringPreservedRanges.Clear();
      this.m_bStringsPreserved = value;
    }
  }

  public IRange[] MergedCells
  {
    get
    {
      this.ParseData();
      int mergeCount = this.m_mergedCells != null ? this.m_mergedCells.MergeCount : 0;
      IRange[] mergedCells = mergeCount > 0 ? new IRange[mergeCount] : (IRange[]) null;
      if (mergedCells != null)
      {
        HashSet<Rectangle> mergedRegions = this.m_mergedCells.MergedRegions;
        int index = 0;
        foreach (Rectangle rectangle in mergedRegions)
        {
          RangeImpl range = this.AppImplementation.CreateRange((object) this, rectangle.X + 1, rectangle.Y + 1, rectangle.Right + 1, rectangle.Bottom + 1);
          mergedCells[index] = (IRange) range;
          ++index;
        }
      }
      return mergedCells;
    }
  }

  public INames Names => (INames) this.m_names;

  public IPageSetup PageSetup
  {
    get
    {
      this.ParseData();
      return (IPageSetup) this.m_pageSetup;
    }
  }

  public IRange PaneFirstVisible
  {
    get
    {
      this.ParseData();
      return (IRange) this.AppImplementation.CreateRange((object) this, this.FirstVisibleColumn + 1, this.FirstVisibleRow + 1);
    }
    set
    {
      this.ParseData();
      this.FirstVisibleRow = value.Row - 1;
      this.FirstVisibleColumn = value.Column - 1;
    }
  }

  public IRange Range
  {
    [DebuggerStepThrough] get => this.GetUsedRange(false);
  }

  public IRange[] Rows => this.UsedRange.Rows;

  public bool IsFreezePanes => this.WindowTwo.IsFreezePanes;

  public IRange SplitCell
  {
    get
    {
      this.ParseData();
      return (IRange) this.AppImplementation.CreateRange((object) this, this.VerticalSplit + 1, this.HorizontalSplit + 1);
    }
    set
    {
      this.ParseData();
      int num1 = value.Column - this.TopLeftCell.Column;
      int num2 = value.Row - this.TopLeftCell.Row;
      this.VerticalSplit = num1 > 0 ? num1 : value.Column - 1;
      this.HorizontalSplit = num2 > 0 ? num2 : value.Row - 1;
      this.WindowTwo.IsFreezePanes = true;
      this.WindowTwo.IsFreezePanesNoSplit = true;
    }
  }

  public double StandardHeight
  {
    get => (double) this.DefaultRowHeight / 20.0;
    set
    {
      if (value < 0.0)
        throw new ArgumentOutOfRangeException("Standard Row Height");
      this.DefaultRowHeight = (int) (value * 20.0);
      if ((this.Workbook as WorkbookImpl).Loading)
        return;
      this.m_isCustomHeight = true;
    }
  }

  public bool StandardHeightFlag
  {
    get
    {
      this.ParseData();
      return this.m_pageSetup.DefaultRowHeightFlag;
    }
    set
    {
      this.ParseData();
      this.m_pageSetup.DefaultRowHeightFlag = value;
    }
  }

  public double StandardWidth
  {
    get
    {
      this.ParseData();
      return this.m_dStandardColWidth;
    }
    set
    {
      if (value < 0.0 || value > (double) byte.MaxValue)
        throw new ArgumentOutOfRangeException("Standard Column Width.");
      this.ParseData();
      if (this.m_hasBaseColWidth)
        this.m_hasBaseColWidth = false;
      this.m_dStandardColWidth = value;
      this.m_defaultColWidth = 8.43;
    }
  }

  public ExcelSheetType Type
  {
    get => this.m_sheetType;
    set
    {
      this.m_sheetType = value;
      if (this.IsSupported || this.m_sheetType != ExcelSheetType.Worksheet)
        return;
      this.IsSupported = true;
    }
  }

  public IRange UsedRange => this.GetUsedRange(true);

  private IRange GetUsedRange(bool isUsedRange)
  {
    this.ParseData();
    if (this.m_busedRangeIncludesCF && this.m_parseCF && !(this.Workbook as WorkbookImpl).Loading && !(this.Workbook as WorkbookImpl).Saving)
      this.ParseSheetCF();
    if (this.m_iFirstColumn == this.m_iLastColumn && this.m_iFirstColumn == int.MaxValue || this.m_iFirstRow == this.m_iLastRow && this.m_iFirstRow < 0)
    {
      if (this.m_rngUsed != null)
        this.m_rngUsed.Dispose();
      this.m_rngUsed = this.AppImplementation.CreateRange((object) this);
    }
    else
    {
      int iFirstRow = this.m_iFirstRow;
      int iFirstColumn = this.m_iFirstColumn;
      int iLastRow = this.m_iLastRow;
      int iLastColumn = this.m_iLastColumn;
      this.GetRangeCoordinates(ref iFirstRow, ref iFirstColumn, ref iLastRow, ref iLastColumn, isUsedRange);
      this.CreateUsedRange(iFirstRow, iFirstColumn, iLastRow, iLastColumn);
    }
    if (this.m_rngUsed.Row == 0 && this.m_rngUsed.Column == 0 && this.ImportDTHelper != null && !this.m_book.Saving && !this.ImportDTHelper.IsLoading)
    {
      this.ImportDTHelper.IsLoading = true;
      this.ImportDataTable(this.ImportDTHelper.DataTable, this.ImportDTHelper.IsFieldNameShown, this.ImportDTHelper.FirstRow, this.ImportDTHelper.FirstColumn, -1, -1, (DataColumn[]) null, true);
      this.m_rngUsed = this.UsedRange as RangeImpl;
      this.ImportDTHelper.IsLoading = false;
    }
    if (isUsedRange)
    {
      if (this.m_rngUsed.FirstColumn > 0 && this.m_rngUsed.FirstRow > 0 && this.m_rngUsed.LastColumn <= this.m_book.MaxColumnCount && this.m_rngUsed.LastRow <= this.m_book.MaxRowCount)
      {
        RangeImpl usedRange = (RangeImpl) this.m_rngUsed.Clone((object) this.m_rngUsed.Worksheet, (Dictionary<string, string>) null, this.m_rngUsed.Workbook);
        usedRange.IsAbsolute = false;
        return (IRange) usedRange;
      }
    }
    else
      this.m_rngUsed.IsAbsolute = true;
    return (IRange) this.m_rngUsed;
  }

  public IRange[] UsedCells
  {
    get
    {
      this.ParseData();
      List<IRange> rangeList = new List<IRange>();
      int num = 0;
      foreach (DictionaryEntry dicRecordsCell in this.m_dicRecordsCells)
      {
        if (dicRecordsCell.Value != null)
        {
          ICellPositionFormat cellPositionFormat = dicRecordsCell.Value as ICellPositionFormat;
          rangeList.Add(this.InnerGetCell(cellPositionFormat.Column + 1, cellPositionFormat.Row + 1));
          ++num;
        }
      }
      return rangeList.ToArray();
    }
  }

  public IVPageBreaks VPageBreaks
  {
    get
    {
      this.ParseData();
      return (IVPageBreaks) this.m_pageSetup.VPageBreaks;
    }
  }

  public bool IsEmpty
  {
    get
    {
      this.ParseData();
      return this.m_iFirstRow == -1 || this.m_iFirstColumn == int.MaxValue;
    }
  }

  public IWorksheetCustomProperties CustomProperties
  {
    get
    {
      this.ParseData();
      if (this.m_arrCustomProperties == null)
        this.m_arrCustomProperties = new WorksheetCustomProperties();
      return (IWorksheetCustomProperties) this.m_arrCustomProperties;
    }
  }

  public IMigrantRange MigrantRange
  {
    get
    {
      this.ParseData();
      if (this.m_migrantRange == null)
        this.CreateMigrantRange();
      return this.m_migrantRange;
    }
  }

  public bool UsedRangeIncludesFormatting
  {
    get => this.m_bUsedRangeIncludesFormatting;
    set => this.m_bUsedRangeIncludesFormatting = value;
  }

  internal bool UsedRangeIncludesCF
  {
    get => this.m_busedRangeIncludesCF;
    set => this.m_busedRangeIncludesCF = value;
  }

  public IPivotTables PivotTables
  {
    get
    {
      if (this.m_pivotTables == null)
        this.m_pivotTables = new PivotTableCollection(this.Application, (object) this);
      return (IPivotTables) this.m_pivotTables;
    }
  }

  public PivotTableCollection InnerPivotTables => this.m_pivotTables;

  public IListObjects ListObjects
  {
    get
    {
      if (this.m_listObjects == null)
        this.m_listObjects = new ListObjectCollection(this);
      return (IListObjects) this.m_listObjects;
    }
  }

  public override bool ProtectContents
  {
    get => (this.InnerProtection & ExcelSheetProtection.Content) == ExcelSheetProtection.None;
    internal set
    {
      if (!value)
        this.InnerProtection |= ExcelSheetProtection.Content;
      else
        this.InnerProtection &= ~ExcelSheetProtection.Content;
    }
  }

  internal ImportDTHelper ImportDTHelper
  {
    get => this.m_importDTHelper;
    set => this.m_importDTHelper = value;
  }

  internal bool IsImporting
  {
    get => this.m_bIsImporting;
    set => this.m_bIsImporting = value;
  }

  internal bool IsSubtotal
  {
    get => this.m_bIsSubtotal;
    set => this.m_bIsSubtotal = value;
  }

  public IDataSort DataSorter
  {
    get
    {
      if (this.m_dataSorter == null)
      {
        this.m_dataSorter = (IDataSort) new Syncfusion.XlsIO.Implementation.Sorting.DataSorter((object) this);
        this.Workbook.CreateDataSorter();
      }
      return this.m_dataSorter;
    }
    internal set => this.m_dataSorter = value;
  }

  internal RangeImpl CopyToRange
  {
    get => this.m_CopyToRange;
    set => this.m_CopyToRange = value;
  }

  internal int CondFmtPos => this.m_iCondFmtPos;

  internal int DefaultXFIndex
  {
    get => this.m_iDefaultXFIndex;
    set
    {
      this.m_iDefaultXFIndex = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof (DefaultXFIndex));
    }
  }

  internal bool HasBaseColWidth
  {
    get => this.m_hasBaseColWidth;
    set => this.m_hasBaseColWidth = value;
  }

  public event ValueChangedEventHandler ColumnWidthChanged;

  public event ValueChangedEventHandler RowHeightChanged;

  internal void SetStandardWidth(double width) => this.m_dStandardColWidth = width;

  private void CreateOutlineWrappers(
    Dictionary<int, List<GroupPoint>> outlineLevels,
    ExcelGroupBy groupBy)
  {
    if (outlineLevels == null)
      return;
    foreach (int key in outlineLevels.Keys)
    {
      List<GroupPoint> outlineLevel = outlineLevels[key];
      for (int index = 0; index < outlineLevel.Count; ++index)
      {
        OutlineWrapper outlineWrapper = new OutlineWrapper();
        outlineWrapper.OutlineLevel = (ushort) key;
        outlineWrapper.FirstIndex = outlineLevel[index].X;
        outlineWrapper.LastIndex = outlineLevel[index].Y;
        outlineWrapper.GroupBy = groupBy;
        IOutline outline1 = (IOutline) null;
        IOutline outline2;
        if (groupBy == ExcelGroupBy.ByRows)
        {
          outlineWrapper.OutlineRange = this[outlineLevel[index].X, 1, outlineLevel[index].Y, 1];
          outline2 = (outlineWrapper.OutlineRange as RangeImpl).GetRowOutline(outlineWrapper.LastIndex);
          if (this.PageSetup.IsSummaryRowBelow)
            outline1 = (outlineWrapper.OutlineRange as RangeImpl).GetRowOutline(outlineWrapper.LastIndex + 1);
          else if (outlineWrapper.FirstIndex > 1)
            outline1 = (outlineWrapper.OutlineRange as RangeImpl).GetRowOutline(outlineWrapper.FirstIndex - 1);
        }
        else
        {
          outlineWrapper.OutlineRange = this[1, outlineLevel[index].X, 1, outlineLevel[index].Y];
          outline2 = (outlineWrapper.OutlineRange as RangeImpl).GetColumnOutline(outlineWrapper.LastIndex);
          if (this.PageSetup.IsSummaryColumnRight)
            outline1 = (outlineWrapper.OutlineRange as RangeImpl).GetColumnOutline(outlineWrapper.LastIndex + 1);
          else if (outlineWrapper.FirstIndex > 1)
            outline1 = (outlineWrapper.OutlineRange as RangeImpl).GetColumnOutline(outlineWrapper.FirstIndex - 1);
        }
        outlineWrapper.Outline = outline1;
        outlineWrapper.IsHidden = outline2.IsHidden;
        if (outline1 != null)
          outlineWrapper.IsCollapsed = outline1.IsCollapsed;
        this.m_outlineWrappers.Add((IOutlineWrapper) outlineWrapper);
      }
    }
  }

  public IInternalWorksheet GetClonedObject(
    Dictionary<string, string> hashNewNames,
    WorkbookImpl book)
  {
    string str1 = this.Name;
    string str2;
    if (hashNewNames != null && hashNewNames.TryGetValue(str1, out str2))
      str1 = str2;
    return book.Worksheets[str1] as IInternalWorksheet;
  }

  public void ParseCFFromExcel2007(FileDataHolder dataHolder)
  {
    if (dataHolder == null)
      return;
    List<DxfImpl> dxfsCollection = dataHolder.ParseDxfsCollection();
    WorksheetDataHolder worksheetDataHolder = (WorksheetDataHolder) null;
    if (dxfsCollection != null)
      worksheetDataHolder = this.DataHolder;
    worksheetDataHolder?.ParseConditionalFormatting(dxfsCollection, this);
  }

  public void ParseSheetCF()
  {
    if (this.Version == ExcelVersion.Excel97to2003)
      return;
    bool isParsing = this.IsParsing;
    this.IsParsing = true;
    this.m_book.AppImplementation.IsFormulaParsed = false;
    char argumentsSeparator = Convert.ToChar(this.m_book.FormulaUtil.OperandsSeparator);
    char arrayRowsSeparator = Convert.ToChar(this.m_book.FormulaUtil.ArrayRowSeparator);
    this.m_book.SetSeparators(Convert.ToChar(this.AppImplementation.CheckAndApplySeperators().TextInfo.ListSeparator), this.AppImplementation.RowSeparator);
    this.ParseCFFromExcel2007(this.m_book.DataHolder);
    this.m_book.AppImplementation.IsFormulaParsed = true;
    this.m_book.SetSeparators(argumentsSeparator, arrayRowsSeparator);
    this.IsParsing = isParsing;
  }

  public override void UpdateExtendedFormatIndex(Dictionary<int, int> dictFormats)
  {
    this.ParseData();
    base.UpdateExtendedFormatIndex(dictFormats);
    this.m_dicRecordsCells.UpdateExtendedFormatIndex(dictFormats);
    this.UpdateOutlineAfterXFRemove((ICollection) this.m_arrColumnInfo, (IDictionary) dictFormats);
  }

  public void UpdateExtendedFormatIndex(int maxCount)
  {
    this.Parse();
    if (maxCount <= 0)
      throw new ArgumentOutOfRangeException(nameof (maxCount));
    this.m_dicRecordsCells.UpdateExtendedFormatIndex(maxCount);
    int defaultXfIndex = this.m_book.DefaultXFIndex;
    int index = 0;
    for (int length = this.m_arrColumnInfo.Length; index < length; ++index)
    {
      ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[index];
      if (columnInfoRecord != null && (int) columnInfoRecord.ExtendedFormatIndex >= maxCount)
        columnInfoRecord.ExtendedFormatIndex = (ushort) defaultXfIndex;
    }
  }

  public RangeRichTextString CreateLabelSSTRTFString(long cellIndex)
  {
    this.ParseData();
    IRange range = (IRange) this.m_dicRecordsCells.GetRange(cellIndex);
    return range == null ? new RangeRichTextString(this.Application, (object) this, cellIndex) : (RangeRichTextString) range.RichText;
  }

  public IRange[] Find(IRange range, byte findValue, bool bIsError, bool bIsFindFirst)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    this.ParseData();
    return this.ConvertCellListIntoRange(this.m_dicRecordsCells.Find(range, findValue, bIsError, bIsFindFirst));
  }

  public IRange[] Find(IRange range, double findValue, ExcelFindType flags, bool bIsFindFirst)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    this.ParseData();
    return (flags & ExcelFindType.Comments) == ExcelFindType.Comments ? this.FindInComments(findValue.ToString(), ExcelFindOptions.None) : this.ConvertCellListIntoRange(this.m_dicRecordsCells.Find(range, findValue, flags, bIsFindFirst));
  }

  internal IRange[] FindEmpty(
    IRange range,
    string findValue,
    bool bFindFirst,
    ExcelFindType findType)
  {
    List<IRange> rangeList = new List<IRange>();
    string empty = string.Empty;
    bool flag = (findType & ExcelFindType.Formula) == ExcelFindType.Formula;
    int lastRow = this.UsedRange.LastRow;
    int lastColumn = this.UsedRange.LastColumn;
    for (int row = 1; row <= lastRow; ++row)
    {
      for (int column = 1; column <= lastColumn; ++column)
      {
        IRange range1 = this.Range[row, column];
        string str = !flag ? range1.DisplayText : range1.Value;
        if (str != null && str == string.Empty)
        {
          rangeList.Add(range1);
          if (bFindFirst)
            break;
        }
      }
    }
    return rangeList.Count <= 0 ? (IRange[]) null : rangeList.ToArray();
  }

  public IRange[] Find(IRange range, string findValue, ExcelFindType flags, bool bIsFindFirst)
  {
    return this.Find(range, findValue, flags, ExcelFindOptions.None, bIsFindFirst);
  }

  public IRange[] Find(
    IRange range,
    string findValue,
    ExcelFindType flags,
    ExcelFindOptions findOptions,
    bool bIsFindFirst)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    if (findValue == null || findValue.Length == 0)
      return (IRange[]) null;
    this.ParseData();
    return (flags & ExcelFindType.Comments) == ExcelFindType.Comments ? this.FindInComments(findValue, findOptions) : this.ConvertCellListIntoRange(this.m_dicRecordsCells.Find(range, findValue, flags, findOptions, bIsFindFirst));
  }

  private IRange[] FindInComments(string findValue, ExcelFindOptions findOptions)
  {
    List<IRange> rangeList = new List<IRange>();
    Regex wildCardRegex = this.GetWildCardRegex(findValue, string.Empty);
    foreach (IComment comment in (IEnumerable) this.Comments)
    {
      if (findOptions != ExcelFindOptions.None ? RowStorage.CheckStringValue(comment.Text, findValue, findOptions, (WorkbookImpl) this.Workbook, wildCardRegex) : (wildCardRegex != null ? wildCardRegex.IsMatch(comment.Text) : comment.Text.Contains(findValue)))
        rangeList.Add(this[comment.Row, comment.Column]);
    }
    return rangeList.Count <= 0 ? (IRange[]) null : rangeList.ToArray();
  }

  public void CopyToClipboard()
  {
    this.ParseData();
    this.m_book.CopyToClipboard(this);
  }

  public void MoveRange(
    IRange destination,
    IRange source,
    ExcelCopyRangeOptions options,
    bool bUpdateRowRecords)
  {
    this.MoveRange(destination, source, options, bUpdateRowRecords, (IOperation) null);
  }

  private void MoveRange(
    IRange destination,
    IRange source,
    ExcelCopyRangeOptions options,
    bool bUpdateRowRecords,
    IOperation beforeMove,
    IRange cFDestination,
    IRange cFSource)
  {
    if (this.CanMove(ref cFDestination, cFSource))
    {
      WorksheetImpl worksheet1 = (WorksheetImpl) cFDestination.Worksheet;
      WorksheetImpl worksheet2 = (WorksheetImpl) cFSource.Worksheet;
      Rectangle rectangle1 = Rectangle.FromLTRB(cFSource.Column - 1, cFSource.Row - 1, cFSource.LastColumn - 1, cFSource.LastRow - 1);
      Rectangle rectangle2 = Rectangle.FromLTRB(cFDestination.Column - 1, cFDestination.Row - 1, cFDestination.LastColumn - 1, cFDestination.LastRow - 1);
      if ((options & ExcelCopyRangeOptions.CopyConditionalFormats) == ExcelCopyRangeOptions.CopyConditionalFormats)
        worksheet2.CopyMoveConditionalFormatting(rectangle1.Y + 1, rectangle1.X + 1, rectangle1.Height + 1, rectangle1.Width + 1, rectangle2.Y + 1, rectangle2.X + 1, worksheet1, true, false);
      this.m_bcheckCF = true;
    }
    this.MoveRange(destination, source, options, true, beforeMove);
    this.m_bcheckCF = false;
  }

  private void MoveRange(
    IRange destination,
    IRange source,
    ExcelCopyRangeOptions options,
    bool bUpdateRowRecords,
    IOperation beforeMove)
  {
    this.ParseData();
    if (destination == source)
      return;
    ExtendedFormatsCollection innerExtFormats = (source.Worksheet.Workbook as WorkbookImpl).InnerExtFormats;
    List<string> stringList = new List<string>();
    if (this.IsPasswordProtected)
    {
      for (int row = source.Row; row <= source.LastRow; ++row)
      {
        for (int column = source.Column; column <= source.LastColumn; ++column)
        {
          int xfIndex = this.GetXFIndex(row, column);
          if (!innerExtFormats[xfIndex].Locked)
          {
            string cellName = RangeImpl.GetCellName(column, row);
            stringList.Add(cellName);
          }
        }
      }
    }
    if ((options & ExcelCopyRangeOptions.UpdateMerges) != ExcelCopyRangeOptions.None && source.IsMerged)
      source = this.GetRangeFromMergedRegion(source);
    WorksheetImpl worksheetImpl = this.CanMove(ref destination, source) ? (WorksheetImpl) destination.Worksheet : throw new InvalidRangeException();
    WorksheetImpl worksheet = (WorksheetImpl) source.Worksheet;
    beforeMove?.Do();
    int iSourceIndex = this.m_book.AddSheetReference((IWorksheet) worksheet);
    int iDestIndex = this.m_book.AddSheetReference((IWorksheet) worksheetImpl);
    this.m_book.AddSheetReference((IWorksheet) worksheet);
    Rectangle rectangle1 = Rectangle.FromLTRB(source.Column - 1, source.Row - 1, source.LastColumn - 1, source.LastRow - 1);
    Rectangle rectangle2 = Rectangle.FromLTRB(destination.Column - 1, destination.Row - 1, destination.LastColumn - 1, destination.LastRow - 1);
    int row1 = destination.Row;
    int row2 = source.Row;
    int column1 = destination.Column;
    int column2 = source.Column;
    bool flag = (options & ExcelCopyRangeOptions.UpdateFormulas) != ExcelCopyRangeOptions.None;
    RangeImpl rangeImpl = (RangeImpl) destination;
    int iMaxRow = 0;
    int iMaxColumn = 0;
    bool bInsert = false;
    if (beforeMove == null && bUpdateRowRecords)
      bInsert = true;
    if ((options & ExcelCopyRangeOptions.UpdateMerges) != ExcelCopyRangeOptions.None)
      WorksheetImpl.CopyRangeMerges(destination, source, true);
    if (!this.m_isInsertingSubTotal || this.m_isRemovingSubTotal)
    {
      RecordTable source1 = this.CacheAndRemoveFromParent(source, destination, ref iMaxRow, ref iMaxColumn, worksheet.m_dicRecordsCells, bInsert);
      worksheet.PartialClearRange(rectangle1);
      worksheetImpl.CellRecords.ClearRange(rectangle2);
      this.CopyCacheInto(source1, worksheetImpl.m_dicRecordsCells.Table, bUpdateRowRecords);
      source1?.Dispose();
    }
    else
    {
      RecordTable table = worksheet.m_dicRecordsCells.Table;
      table.InsertIntoDefaultRows(source.Row - 1, 1);
      if (this.m_movedRows == null)
        this.m_movedRows = new SortedDictionary<int, int>();
      int num1 = 0;
      if (this.m_movedRows.ContainsKey(source.Row))
      {
        num1 = this.m_movedRows[source.Row];
        this.m_movedRows.Remove(source.Row);
        this.m_movedRows.Add(table.LastRow + 1, 0);
      }
      for (int key = source.Row + 1; key <= table.LastRow + 1; ++key)
      {
        int num2 = 0;
        if (this.m_movedRows.TryGetValue(key, out num2))
        {
          int movedRow = this.m_movedRows[key];
          this.m_movedRows[key] = num1 + 1;
          num1 = movedRow;
        }
        else
          this.m_movedRows.Add(key, num1 + 1);
        RowStorage row3 = WorksheetHelper.GetOrCreateRow(source.Worksheet as IInternalWorksheet, key - 1, false);
        if (row3 != null)
          row3.Row = key - 1;
      }
      if (table.LastRow + 1 <= this.m_book.MaxRowCount)
        WorksheetHelper.AccessRow((IInternalWorksheet) this, table.LastRow + 1);
    }
    if (this.HyperLinks.Count > 0)
      this.CopyMoveHyperlinks(rectangle1.Y + 1, rectangle1.X + 1, rectangle1.Height + 1, rectangle1.Width + 1, rectangle2.Y + 1, rectangle2.X + 1, worksheetImpl, true);
    WorksheetHelper.AccessRow((IInternalWorksheet) worksheetImpl, rangeImpl.FirstRow);
    WorksheetHelper.AccessColumn((IInternalWorksheet) worksheetImpl, rangeImpl.FirstColumn);
    if (iMaxColumn > rangeImpl.FirstColumn)
      WorksheetHelper.AccessColumn((IInternalWorksheet) worksheetImpl, iMaxColumn);
    if (iMaxRow > rangeImpl.FirstRow && iMaxRow <= this.m_book.MaxRowCount)
      WorksheetHelper.AccessRow((IInternalWorksheet) worksheetImpl, iMaxRow);
    WorksheetHelper.AccessColumn((IInternalWorksheet) worksheetImpl, rangeImpl.LastColumn);
    if ((options & ExcelCopyRangeOptions.CopyErrorIndicators) != ExcelCopyRangeOptions.None)
      worksheet.CopyMoveErrorIndicators(rectangle1.Y + 1, rectangle1.X + 1, rectangle1.Height + 1, rectangle1.Width + 1, rectangle2.Y + 1, rectangle2.X + 1, worksheetImpl, true);
    if ((options & ExcelCopyRangeOptions.CopyConditionalFormats) == ExcelCopyRangeOptions.CopyConditionalFormats && !this.m_bcheckCF)
      worksheet.CopyMoveConditionalFormatting(rectangle1.Y + 1, rectangle1.X + 1, rectangle1.Height + 1, rectangle1.Width + 1, rectangle2.Y + 1, rectangle2.X + 1, worksheetImpl, true, false);
    if (flag)
    {
      if (this.m_isInsertingRow)
      {
        rectangle1 = Rectangle.FromLTRB(0, rectangle1.Y, this.Workbook.MaxColumnCount - 1, this.Workbook.MaxRowCount - (rectangle2.Y - rectangle1.Y) - 1);
        rectangle2 = Rectangle.FromLTRB(0, rectangle2.Y, this.Workbook.MaxColumnCount - 1, this.Workbook.MaxRowCount - 1);
      }
      else if (this.m_isInsertingColumn)
      {
        rectangle1 = Rectangle.FromLTRB(rectangle1.X, 0, this.Workbook.MaxColumnCount - (rectangle2.X - rectangle1.X) - 1, this.Workbook.MaxRowCount - 1);
        rectangle2 = Rectangle.FromLTRB(rectangle2.X, 0, this.Workbook.MaxColumnCount - 1, this.Workbook.MaxRowCount - 1);
      }
      if (!this.m_isDeletingRow && !this.m_isDeletingColumn && (!this.m_isInsertingSubTotal || this.m_isRemovingSubTotal))
        this.m_book.UpdateFormula(iSourceIndex, rectangle1, iDestIndex, rectangle2);
    }
    if ((source.Worksheet as WorksheetImpl).m_dataValidation != null && (source.Worksheet as WorksheetImpl).m_dataValidation.Count > 0 && (options & ExcelCopyRangeOptions.CopyDataValidations) == ExcelCopyRangeOptions.CopyDataValidations)
      worksheet.CopyMoveDataValidations(rectangle1.Y + 1, rectangle1.X + 1, rectangle1.Height + 1, rectangle1.Width + 1, rectangle2.Y + 1, rectangle2.X + 1, worksheetImpl, true);
    if ((options & ExcelCopyRangeOptions.CopyShapes) != ExcelCopyRangeOptions.None)
    {
      ++rectangle1.X;
      ++rectangle1.Y;
      ++rectangle2.X;
      ++rectangle2.Y;
      ((ShapesCollection) this.Shapes).CopyMoveShapeOnRangeCopy(worksheetImpl, rectangle1, rectangle2, false);
    }
    if (this.m_book.InnerNamesColection.Count > 0)
      this.m_book.InnerNamesColection.MoveNamedRanges(destination as RangeImpl, source as RangeImpl, options);
    this.CopyMoveSparkLines(source, destination, worksheetImpl, true);
    if ((this.ListObjects.Count > 0 || this.PivotTables.Count > 0) && (source.Worksheet != destination.Worksheet || source.Worksheet.Workbook != destination.Worksheet.Workbook || !(source.AddressLocal == destination.AddressLocal)))
      this.MoveTables(destination, source, options);
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, source.Worksheet);
    foreach (string name in stringList)
    {
      int iRow;
      int iColumn;
      RangeImpl.CellNameToRowColumn(name, out iRow, out iColumn);
      migrantRangeImpl.ResetRowColumn(iRow, iColumn);
      migrantRangeImpl.CellStyle.Locked = false;
    }
    this.SetChanged();
  }

  private void MoveTables(IRange destination, IRange source, ExcelCopyRangeOptions options)
  {
    int count = this.PivotTables.Count;
    WorksheetImpl worksheet = destination.Worksheet as WorksheetImpl;
    if (options != ExcelCopyRangeOptions.All)
      return;
    for (int index = 0; index < this.ListObjects.Count; ++index)
    {
      IListObject listObject = this.ListObjects[index];
      IRange location = listObject.Location;
      IRange destTableRange = this.GetDestTableRange(source, destination, location);
      if (destTableRange != null)
      {
        bool flag;
        if (this == worksheet)
        {
          flag = this.HasTableRange(worksheet, index, destTableRange);
          if (!flag)
            listObject.Location = destTableRange;
        }
        else
        {
          flag = this.HasTableRange(worksheet, 0, destTableRange);
          if (!flag)
          {
            string name = listObject.Name;
            TableBuiltInStyles builtInTableStyle = listObject.BuiltInTableStyle;
            this.ListObjects.Remove(listObject);
            this.Workbook.Names.Remove(name);
            worksheet.ListObjects.Create(name, destTableRange).BuiltInTableStyle = builtInTableStyle;
            --index;
          }
        }
        if (flag)
        {
          string name = listObject.Name;
          this.ListObjects.Remove(listObject);
          this.Workbook.Names.Remove(name);
          --index;
        }
      }
    }
    for (int index = 0; index < count; ++index)
    {
      IPivotTable pivotTable = this.PivotTables[index];
      IRange location = pivotTable.Location;
      IRange destTableRange = this.GetDestTableRange(source, destination, location);
      if (destTableRange != null)
      {
        if (this.HasPivotTableRange(worksheet, index, destTableRange))
          break;
        pivotTable.Location = destTableRange;
        break;
      }
    }
  }

  private bool HasPivotTableRange(WorksheetImpl sheet, int index, IRange destPivotTableRange)
  {
    bool flag = sheet.CheckOverLap<IPivotTables>(destPivotTableRange, index, sheet.PivotTables);
    if (sheet.ListObjects.Count > 0 && !flag)
      flag = sheet.CheckOverLap<IListObjects>(destPivotTableRange, 0, sheet.ListObjects);
    return flag;
  }

  private bool HasTableRange(WorksheetImpl sheet, int index, IRange destTableRange)
  {
    bool flag = sheet.CheckOverLap<IListObjects>(destTableRange, index, sheet.ListObjects);
    if (sheet.PivotTables.Count > 0 && !flag)
      flag = sheet.CheckOverLap<IPivotTables>(destTableRange, 0, sheet.PivotTables);
    return flag;
  }

  private bool CheckOverLap<T>(IRange range, int index, T obj)
  {
    bool flag = true;
    WorksheetImpl worksheet = range.Worksheet as WorksheetImpl;
    int num1 = 0;
    ListObjectCollection objectCollection = (ListObjectCollection) null;
    PivotTableCollection pivotTableCollection = (PivotTableCollection) null;
    if ((object) obj is ListObjectCollection)
    {
      num1 = worksheet.ListObjects.Count;
      objectCollection = (object) obj as ListObjectCollection;
    }
    else if ((object) obj is PivotTableCollection)
    {
      num1 = worksheet.PivotTables.Count;
      pivotTableCollection = (object) obj as PivotTableCollection;
    }
    if (!worksheet.IsParsing)
    {
      int row = range.Row;
      int column = range.Column;
      int lastRow = range.LastRow;
      int lastColumn = range.LastColumn;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      int num5 = 0;
      for (int index1 = 0; index1 < num1; ++index1)
      {
        if (index != index1 || index == 0)
        {
          if (objectCollection != null)
          {
            IListObject listObject = objectCollection[index1];
            num2 = listObject.Location.Row;
            num3 = listObject.Location.Column;
            num4 = listObject.Location.LastRow;
            num5 = listObject.Location.LastColumn;
          }
          else if (pivotTableCollection != null)
          {
            IPivotTable pivotTable = pivotTableCollection[index1];
            num2 = pivotTable.Location.Row;
            num3 = pivotTable.Location.Column;
            num4 = pivotTable.Location.LastRow;
            num5 = pivotTable.Location.LastColumn;
          }
          if (row <= num2 && lastRow >= num4 && column <= num3 && lastColumn >= num5 && objectCollection != null)
          {
            string name = objectCollection[index1].Name;
            worksheet.ListObjects.Remove(objectCollection[index1]);
            this.Workbook.Names.Remove(name);
            --num1;
            --index1;
          }
          else if (row > num2 - 1 && row < num4 + 1 && column > num3 - 1 && column < num5 + 1 || row > num2 - 1 && row < num4 + 1 && lastColumn > num3 - 1 && lastColumn < num5 + 1 || lastRow > num2 - 1 && lastRow < num4 + 1 && column > num3 - 1 && column < num5 + 1 || lastRow > num2 - 1 && lastRow < num4 + 1 && lastColumn > num3 - 1 && lastColumn < num5 + 1 || (row < num2 - 1 && row < num4 + 1 || lastRow > num2 - 1 && lastRow < num4 + 1) && num3 > column && num5 < lastColumn || (column < num3 - 1 && column < num5 + 1 || lastColumn > num3 - 1 && lastColumn < num5 + 1) && num2 > row && num4 < lastRow)
            return flag;
        }
      }
    }
    return false;
  }

  private IRange GetDestTableRange(IRange srcRange, IRange destRange, IRange tableRange)
  {
    IRange destTableRange = (IRange) null;
    if (srcRange.Row <= tableRange.Row && srcRange.Column <= tableRange.Column && srcRange.LastRow >= tableRange.LastRow && srcRange.LastColumn >= tableRange.LastColumn)
    {
      int row = tableRange.Row - srcRange.Row + destRange.Row;
      int column = tableRange.Column - srcRange.Column + destRange.Column;
      int lastRow = destRange.LastRow - (srcRange.LastRow - tableRange.LastRow);
      int lastColumn = destRange.LastColumn - (srcRange.LastColumn - tableRange.LastColumn);
      destTableRange = destRange.Worksheet[row, column, lastRow, lastColumn];
    }
    return destTableRange;
  }

  public IRange CopyRange(IRange destination, IRange source)
  {
    return this.CopyRange(destination, source, ExcelCopyRangeOptions.UpdateMerges);
  }

  internal IRange CopyRange(IRange destination, IRange source, bool pasteLink)
  {
    bool flag1 = ((WorkbookImpl) source.Worksheet.Workbook).FullFileName != null;
    string str1 = "";
    string str2;
    if (flag1)
    {
      string fullFileName = ((WorkbookImpl) source.Worksheet.Workbook).FullFileName;
      str2 = Path.GetFileName(fullFileName);
      str1 = fullFileName.Substring(0, fullFileName.LastIndexOf('\\'));
    }
    else
      str2 = "Book1";
    bool flag2 = source.Worksheet == destination.Worksheet;
    bool flag3 = source.Worksheet.Workbook == destination.Worksheet.Workbook;
    int num1 = destination.LastRow - destination.Row + 1;
    int num2 = destination.LastColumn - destination.Column + 1;
    if (destination.Count == 1)
    {
      num1 = source.LastRow - source.Row + 1;
      num2 = source.LastColumn - source.Column + 1;
    }
    MigrantRangeImpl source1 = new MigrantRangeImpl(source.Application, source.Worksheet);
    MigrantRangeImpl destination1 = new MigrantRangeImpl(destination.Application, destination.Worksheet);
    if ((source as RangeImpl).IsSingleCell)
    {
      for (int index1 = 0; index1 < num1; ++index1)
      {
        for (int index2 = 0; index2 < num2; ++index2)
        {
          source1.ResetRowColumn(index1 + source.Row, index2 + source.Column);
          destination1.ResetRowColumn(index1 + destination.Row, index2 + destination.Column);
          if (!flag3)
          {
            if (flag1)
              destination1.Formula = $"'{str1}\\[{str2}]{source1.Worksheet.Name}'!{source1.AddressGlobalWithoutSheetName}";
            else
              destination1.Formula = $"[{str2}]{source1.Worksheet.Name}!{source1.AddressGlobalWithoutSheetName}";
          }
          else if (flag2)
            destination1.Formula = ((RangeImpl) source).AddressGlobalWithoutSheetName;
          else
            destination1.Formula = ((RangeImpl) source).AddressGlobal;
          this.CopyRangeFormulaValue(index1 + destination.Row, index2 + destination.Column, index1 + source.Row, index2 + source.Column, (IMigrantRange) destination1, (IMigrantRange) source1);
        }
      }
    }
    else
    {
      for (int index3 = 0; index3 < num1; ++index3)
      {
        for (int index4 = 0; index4 < num2; ++index4)
        {
          destination1.ResetRowColumn(index3 + destination.Row, index4 + destination.Column);
          source1.ResetRowColumn(index3 + source.Row, index4 + source.Column);
          if (!flag3)
          {
            if (flag1)
              destination1.Formula = $"'{str1}\\[{str2}]{source.Worksheet.Name}'!{source1.AddressLocal}";
            else
              destination1.Formula = $"[{str2}]{source1.Worksheet.Name}!{source1.AddressLocal}";
          }
          else if (flag2)
            destination1.Formula = source1.AddressLocal;
          else
            destination1.Formula = source1.Address;
          this.CopyRangeFormulaValue(index3 + destination.Row, index4 + destination.Column, index3 + source.Row, index4 + source.Column, (IMigrantRange) destination1, (IMigrantRange) source1);
        }
      }
    }
    return destination;
  }

  internal void CopyRangeFormulaValue(
    int destRow,
    int destColumn,
    int srcRow,
    int srcColumn,
    IMigrantRange destination,
    IMigrantRange source)
  {
    MigrantRangeImpl migrantRangeImpl1 = new MigrantRangeImpl(source.Application, source.Worksheet);
    MigrantRangeImpl migrantRangeImpl2 = new MigrantRangeImpl(destination.Application, destination.Worksheet);
    migrantRangeImpl1.ResetRowColumn(srcRow, srcColumn);
    migrantRangeImpl2.ResetRowColumn(destRow, destColumn);
    if (destination.Worksheet.Workbook != source.Worksheet.Workbook)
    {
      switch (this.GetCellType(srcRow, srcColumn, false))
      {
        case WorksheetImpl.TRangeValueType.Blank:
          migrantRangeImpl2.Worksheet.SetFormulaStringValue(destRow, destColumn, string.Empty);
          break;
        case WorksheetImpl.TRangeValueType.Error:
          migrantRangeImpl2.Worksheet.SetFormulaErrorValue(destRow, destColumn, this.GetError(srcRow, srcColumn));
          break;
        case WorksheetImpl.TRangeValueType.Boolean:
          migrantRangeImpl2.Worksheet.SetFormulaBoolValue(destRow, destColumn, this.GetBoolean(srcRow, srcColumn));
          break;
        case WorksheetImpl.TRangeValueType.Number:
          migrantRangeImpl2.Worksheet.SetFormulaNumberValue(destRow, destColumn, this.GetNumber(srcRow, srcColumn));
          break;
        case WorksheetImpl.TRangeValueType.Formula:
          switch (this.GetCellType(srcRow, srcColumn, true))
          {
            case WorksheetImpl.TRangeValueType.Formula:
              migrantRangeImpl2.Worksheet.SetFormulaStringValue(destRow, destColumn, this.GetFormulaStringValue(srcRow, srcColumn));
              break;
            case WorksheetImpl.TRangeValueType.Error | WorksheetImpl.TRangeValueType.Formula:
              migrantRangeImpl2.Worksheet.SetFormulaErrorValue(destRow, destColumn, this.GetFormulaErrorValue(srcRow, srcColumn));
              break;
            case WorksheetImpl.TRangeValueType.Boolean | WorksheetImpl.TRangeValueType.Formula:
              migrantRangeImpl2.Worksheet.SetFormulaBoolValue(destRow, destColumn, this.GetFormulaBoolValue(srcRow, srcColumn));
              break;
            case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
              migrantRangeImpl2.Worksheet.SetFormulaNumberValue(destRow, destColumn, this.GetFormulaNumberValue(srcRow, srcColumn));
              break;
            case WorksheetImpl.TRangeValueType.Formula | WorksheetImpl.TRangeValueType.String:
              migrantRangeImpl2.Worksheet.SetFormulaStringValue(destRow, destColumn, this.GetFormulaStringValue(srcRow, srcColumn));
              break;
          }
          break;
        case WorksheetImpl.TRangeValueType.String:
          migrantRangeImpl2.Worksheet.SetFormulaStringValue(destRow, destColumn, this.GetText(srcRow, srcColumn));
          break;
      }
      if (((WorksheetImpl) migrantRangeImpl2.Worksheet).GetRecord(destRow, destColumn) is FormulaRecord record)
        record.CalculateOnOpen = false;
      ((WorksheetImpl) migrantRangeImpl2.Worksheet).InnerSetCell(destColumn, destRow, (BiffRecordRaw) record);
    }
    FormatImpl numberFormatSettings1 = this.m_book.InnerExtFormats[(int) migrantRangeImpl1.ExtendedFormatIndex].NumberFormatSettings as FormatImpl;
    FormatImpl numberFormatSettings2 = ((WorksheetBaseImpl) migrantRangeImpl2.Worksheet).m_book.InnerExtFormats[(int) migrantRangeImpl2.ExtendedFormatIndex].NumberFormatSettings as FormatImpl;
    if (numberFormatSettings1.FormatType == numberFormatSettings2.FormatType || numberFormatSettings1.FormatType == ExcelFormatType.General)
      return;
    if (numberFormatSettings2.FormatType == ExcelFormatType.General)
    {
      migrantRangeImpl2.NumberFormat = migrantRangeImpl1.NumberFormat;
    }
    else
    {
      if (numberFormatSettings2.FormatType == ExcelFormatType.Unknown || numberFormatSettings2.FormatType != ExcelFormatType.Number && numberFormatSettings2.FormatType != ExcelFormatType.Percentage && numberFormatSettings2.FormatType != ExcelFormatType.DecimalPercentage && numberFormatSettings2.FormatType != ExcelFormatType.Text)
        return;
      migrantRangeImpl2.NumberFormat = migrantRangeImpl1.NumberFormat;
    }
  }

  internal void CopyRangeValues(
    WorksheetImpl source,
    WorksheetImpl destination,
    int srcRow,
    int srcColumn,
    int destRow,
    int destColumn)
  {
    switch (source.GetCellType(srcRow, srcColumn, false))
    {
      case WorksheetImpl.TRangeValueType.Blank:
        destination.SetString(destRow, destColumn, string.Empty);
        break;
      case WorksheetImpl.TRangeValueType.Error:
        destination.SetError(destRow, destColumn, source.GetError(srcRow, srcColumn));
        break;
      case WorksheetImpl.TRangeValueType.Boolean:
        destination.SetBoolean(destRow, destColumn, source.GetBoolean(srcRow, srcColumn));
        break;
      case WorksheetImpl.TRangeValueType.Number:
        destination.SetNumber(destRow, destColumn, source.GetNumber(srcRow, srcColumn));
        break;
      case WorksheetImpl.TRangeValueType.Formula:
        switch (source.GetCellType(srcRow, srcColumn, true))
        {
          case WorksheetImpl.TRangeValueType.Formula:
            destination.SetString(destRow, destColumn, source.GetFormulaStringValue(srcRow, srcColumn));
            return;
          case WorksheetImpl.TRangeValueType.Error | WorksheetImpl.TRangeValueType.Formula:
            destination.SetError(destRow, destColumn, source.GetFormulaErrorValue(srcRow, srcColumn));
            return;
          case WorksheetImpl.TRangeValueType.Boolean | WorksheetImpl.TRangeValueType.Formula:
            destination.SetBoolean(destRow, destColumn, source.GetFormulaBoolValue(srcRow, srcColumn));
            return;
          case WorksheetImpl.TRangeValueType.Error | WorksheetImpl.TRangeValueType.Boolean | WorksheetImpl.TRangeValueType.Formula:
            return;
          case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
            destination.SetNumber(destRow, destColumn, source.GetFormulaNumberValue(srcRow, srcColumn));
            return;
          case WorksheetImpl.TRangeValueType.Formula | WorksheetImpl.TRangeValueType.String:
            destination.SetString(destRow, destColumn, source.GetFormulaStringValue(srcRow, srcColumn));
            return;
          default:
            return;
        }
      case WorksheetImpl.TRangeValueType.String:
        destination.SetString(destRow, destColumn, source.GetText(srcRow, srcColumn));
        break;
    }
  }

  public IRange CopyRange(IRange destination, IRange source, ExcelCopyRangeOptions options)
  {
    return this.CopyRange(destination, source, options, false);
  }

  internal IRange CopyRange(
    IRange destination,
    IRange source,
    ExcelCopyRangeOptions options,
    bool skipBlank)
  {
    (destination.Worksheet.Workbook as WorkbookImpl).m_bisCopy = true;
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    List<IRange> rangeList = new List<IRange>(1);
    if (destination is IRanges)
      rangeList.AddRange((IEnumerable<IRange>) (destination as RangesCollection).InnerList);
    else
      rangeList.Add(destination);
    for (int index = 0; index < rangeList.Count; ++index)
    {
      destination = rangeList[index];
      if (destination == null)
        throw new ArgumentNullException(nameof (destination));
      this.ParseData();
      if (source.Worksheet != this)
        return ((WorksheetImpl) source.Worksheet).CopyRange(destination, source);
      RangeImpl rangeImpl1 = (RangeImpl) source;
      RangeImpl rangeImpl2 = (RangeImpl) destination;
      if (rangeImpl1.IsEntireRow)
      {
        int row1 = rangeImpl1.Row;
        int row2 = rangeImpl2.Row;
        while (row1 <= rangeImpl1.LastRow)
        {
          RowStorage row3 = WorksheetHelper.GetOrCreateRow(rangeImpl1.Worksheet as IInternalWorksheet, row1 - 1, false);
          if (row3 != null)
          {
            RowStorage row4 = WorksheetHelper.GetOrCreateRow(rangeImpl2.Worksheet as IInternalWorksheet, row2 - 1, true);
            row4.CopyRowRecordFrom(row3);
            (rangeImpl2.Worksheet as WorksheetImpl).RaiseRowHeightChangedEvent(row2, (double) row4.Height / 20.0);
          }
          ++row1;
          ++row2;
        }
      }
      if (rangeImpl1.IsEntireColumn)
      {
        if (source.ColumnWidth < 0.0)
          rangeImpl1.SetDifferedColumnWidth(rangeImpl1, rangeImpl2);
        else
          destination.ColumnWidth = source.ColumnWidth;
        int column = destination.Column;
        for (int firstColumn = rangeImpl1.FirstColumn; firstColumn <= rangeImpl1.LastColumn; ++firstColumn)
        {
          this.m_arrColumnInfo[column].ExtendedFormatIndex = this.m_arrColumnInfo[firstColumn].ExtendedFormatIndex;
          ++column;
        }
      }
      if (rangeImpl1.IsEntireColumn)
        this.SetEntireRowAndColumnRange(rangeImpl1, rangeImpl2, true);
      else if (rangeImpl1.IsEntireRow)
        this.SetEntireRowAndColumnRange(rangeImpl1, rangeImpl2, false);
      int row5 = destination.Row;
      int column1 = destination.Column;
      IRange fromMergedRegion = this.GetRangeFromMergedRegion(source);
      if (fromMergedRegion.IsMerged && fromMergedRegion.AddressGlobal == this.GetRangeFromMergedRegion(destination).AddressGlobal)
      {
        destination.UnMerge();
      }
      else
      {
        int num1 = 1;
        int num2 = 1;
        int row6 = source.Row;
        int column2 = source.Column;
        int num3 = destination.LastRow - row5 + 1;
        int num4 = destination.LastColumn - column1 + 1;
        int num5 = source.LastRow - source.Row + 1;
        int num6 = source.LastColumn - source.Column + 1;
        RangeImpl rangeImpl3;
        if (destination.LastRow - destination.Row <= source.LastRow - source.Row && destination.LastColumn - destination.Column <= source.LastColumn - source.Column)
        {
          bool isSingleCell = (destination as RangeImpl).IsSingleCell;
          destination = (IRange) (rangeImpl3 = (RangeImpl) rangeImpl2.Worksheet[destination.Row, destination.Column, destination.Row + source.LastRow - source.Row, destination.Column + source.LastColumn - source.Column]);
          if (isSingleCell)
          {
            num3 = destination.LastRow - row5 + 1;
            num4 = destination.LastColumn - column1 + 1;
          }
        }
        else
        {
          if (num3 % num5 == 0 && num4 % num6 == 0)
          {
            num1 = num3 / num5;
            num2 = num4 / num6;
          }
          else if (num3 % num5 == 0 && num4 <= num6)
            num1 = num3 / num5;
          else if (num4 % num6 == 0 && num3 <= num5)
            num2 = num4 / num6;
          destination = (IRange) (rangeImpl3 = (RangeImpl) rangeImpl2.Worksheet[destination.Row, destination.Column, destination.Row + num5 * num1 - 1, destination.Column + num6 * num2 - 1]);
        }
        if ((source as RangeImpl).IsEntireColumn || (source as RangeImpl).IsEntireRow || !(destination.AddressLocal != this.GetRangeFromMergedRegion(destination).AddressLocal) || source.Row > destination.LastRow || source.LastRow < destination.Row)
        {
          if (rangeImpl1.IsSingleCell && !rangeImpl3.IsSingleCell)
            (destination.Worksheet as WorksheetImpl).m_CopyToRange = rangeImpl3;
          (rangeImpl3.Worksheet as WorksheetImpl).m_bisCFCopied = true;
          bool copying = true;
          if (options == ExcelCopyRangeOptions.CopyStyles)
          {
            int num7 = num3 / num5;
            int num8 = num4 / num6;
            int num9 = num3 % num5;
            int num10 = num4 % num6;
            int num11 = column1;
            int num12 = num4;
            int num13 = 0;
            while (num13 < num7 + 1)
            {
              if (num13 == num7)
                num3 = num9;
              if (num3 != 0)
              {
                int column3 = num11;
                int num14 = num12;
                int num15 = 0;
                while (num15 < num8 + 1)
                {
                  if (num15 == num8)
                    num14 = num10;
                  if (num14 != 0)
                  {
                    RangeImpl source1 = (RangeImpl) destination.Worksheet[row6, column2, row6 + (num3 < num5 ? num3 : num5) - 1, column2 + (num14 < num6 ? num14 : num6) - 1];
                    RangeImpl destination1 = (RangeImpl) destination.Worksheet[row5, column3, row5 + (num3 < num5 ? num3 : num5) - 1, column3 + (num14 < num6 ? num14 : num6) - 1];
                    if (!destination1.AreFormulaArraysNotSeparated)
                      throw new InvalidRangeException();
                    this.CopyRangeWithoutCheck(source1, destination1, options, copying, skipBlank);
                    ++num15;
                    column3 += num6;
                  }
                  else
                    break;
                }
                ++num13;
                row5 += num5;
              }
              else
                break;
            }
          }
          else
          {
            int num16 = 0;
            while (num16 < num1)
            {
              int num17 = 0;
              int column4 = column1;
              while (num17 < num2)
              {
                RangeImpl destination2 = (RangeImpl) destination.Worksheet[row5, column4, row5 + num5 - 1, column4 + num6 - 1];
                if (!destination2.AreFormulaArraysNotSeparated)
                  throw new InvalidRangeException();
                this.CopyRangeWithoutCheck(rangeImpl1, destination2, options, copying, skipBlank);
                ++num17;
                column4 += num6;
              }
              ++num16;
              row5 += num5;
            }
          }
        }
      }
    }
    this.SetChanged();
    return destination;
  }

  internal void SetEntireRowAndColumnRange(
    RangeImpl source,
    RangeImpl destination,
    bool isEntireColumn)
  {
    if (destination == null)
      throw new ArgumentException(nameof (destination));
    WorksheetImpl worksheetImpl = source != null ? source.Worksheet as WorksheetImpl : throw new ArgumentException("Source");
    WorksheetImpl worksheet = destination.Worksheet as WorksheetImpl;
    if (isEntireColumn)
    {
      if (worksheetImpl.UsedRange.LastRow >= worksheet.UsedRange.LastRow)
      {
        source.LastRow = worksheetImpl.UsedRange.LastRow;
        destination.LastRow = worksheetImpl.UsedRange.LastRow;
      }
      else
      {
        source.LastRow = worksheet.UsedRange.LastRow;
        destination.LastRow = worksheet.UsedRange.LastRow;
      }
    }
    else
    {
      if (isEntireColumn)
        return;
      if (worksheetImpl.UsedRange.LastColumn >= worksheet.UsedRange.LastColumn)
      {
        source.LastColumn = worksheetImpl.UsedRange.LastColumn;
        destination.LastColumn = worksheetImpl.UsedRange.LastColumn;
      }
      else
      {
        source.LastColumn = worksheet.UsedRange.LastColumn;
        destination.LastColumn = worksheet.UsedRange.LastColumn;
      }
    }
  }

  private IListObject CheckTableRange(IRange destRange)
  {
    foreach (IListObject listObject in (IEnumerable<IListObject>) destRange.Worksheet.ListObjects)
    {
      IRange range = destRange.IntersectWith(destRange.Worksheet[listObject.Location.Row, listObject.Location.Column, listObject.Location.Row, listObject.Location.LastColumn]);
      if (listObject.ShowHeaderRow && range != null)
        return listObject;
    }
    return (IListObject) null;
  }

  internal IListObject IsTableRange(IRange destRange)
  {
    foreach (IListObject listObject in (IEnumerable<IListObject>) destRange.Worksheet.ListObjects)
    {
      if (this.IntersectRanges(destRange, listObject.Location) != null)
        return listObject;
    }
    return (IListObject) null;
  }

  internal IListObject IsAdjacentTable(IRange range)
  {
    foreach (IListObject listObject in (IEnumerable<IListObject>) range.Worksheet.ListObjects)
    {
      int row1 = range.Row;
      int column1 = range.Column;
      int lastRow1 = range.LastRow;
      int lastColumn1 = range.LastColumn;
      int row2 = listObject.Location.Row;
      int column2 = listObject.Location.Column;
      int lastRow2 = listObject.Location.LastRow;
      int lastColumn2 = listObject.Location.LastColumn;
      if (row1 >= row2 && lastRow1 <= lastRow2 && column1 == lastColumn2 + 1 && (listObject.Worksheet[row2, lastColumn2 + 1, lastRow2, lastColumn2 + 1] as RangeImpl).CellType == RangeImpl.TCellType.Blank)
        return listObject;
    }
    return (IListObject) null;
  }

  internal IListObject IsAdjacentTableRow(IRange range)
  {
    foreach (IListObject listObject in (IEnumerable<IListObject>) range.Worksheet.ListObjects)
    {
      int row1 = range.Row;
      int column1 = range.Column;
      int lastRow1 = range.LastRow;
      int lastColumn1 = range.LastColumn;
      int row2 = listObject.Location.Row;
      int column2 = listObject.Location.Column;
      int lastRow2 = listObject.Location.LastRow;
      int lastColumn2 = listObject.Location.LastColumn;
      if (row1 <= lastRow2 + 1 && lastColumn1 <= lastColumn2 && column1 >= column2 && (listObject.Worksheet[lastRow2 + 1, column2, lastRow2 + 1, lastColumn2] as RangeImpl).CellType == RangeImpl.TCellType.Blank)
        return listObject;
    }
    return (IListObject) null;
  }

  internal IListObject IsEntireTableRange(IRange range)
  {
    if (range.Worksheet.ListObjects.Count <= 0)
      return (IListObject) null;
    IListObject listObject = range.Worksheet.ListObjects[0];
    for (int index = 0; index < range.Worksheet.ListObjects.Count; ++index)
    {
      listObject = range.Worksheet.ListObjects[index];
      int row1 = range.Row;
      int column1 = range.Column;
      int lastRow1 = range.LastRow;
      int lastColumn1 = range.LastColumn;
      int row2 = listObject.Location.Row;
      int column2 = listObject.Location.Column;
      int lastRow2 = listObject.Location.LastRow;
      int lastColumn2 = listObject.Location.LastColumn;
      if (row1 == row2 && lastRow1 == lastRow2 && lastColumn1 == lastColumn2 && column1 == column2 && (listObject.Worksheet[row2, column2, lastRow2, lastColumn2] as RangeImpl).CellType != RangeImpl.TCellType.Blank)
        return (IListObject) null;
    }
    return listObject;
  }

  public void CopyRangeWithoutCheck(
    RangeImpl source,
    RangeImpl destination,
    ExcelCopyRangeOptions options)
  {
    this.CopyRangeWithoutCheck(source, destination, options, false, false);
  }

  internal bool IsValidNameExist(IRange range, string name)
  {
    bool flag = false;
    WorksheetNamesCollection names1 = range.Worksheet.Names as WorksheetNamesCollection;
    WorkbookNamesCollection names2 = range.Worksheet.Workbook.Names as WorkbookNamesCollection;
    if (names1.Contains(name))
    {
      if (names1[name] != null && names1[name].RefersToRange != null)
        flag = true;
    }
    else if (names2.Contains(name) && names2[name] != null && names2[name].RefersToRange != null)
      flag = true;
    return flag;
  }

  private void CopyRangeWithoutCheck(
    RangeImpl source,
    RangeImpl destination,
    ExcelCopyRangeOptions options,
    bool copying,
    bool skipBlank)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    if (!destination.AreFormulaArraysNotSeparated)
      throw new InvalidRangeException("Can't copy to destination range.");
    this.ParseData();
    int iRowCount = source.LastRow - source.Row + 1;
    int iColumnCount = source.LastColumn - source.Column + 1;
    Rectangle rectIntersection;
    RecordTable intersection = this.m_dicRecordsCells.CacheIntersection((IRange) destination, (IRange) source, out rectIntersection);
    Dictionary<ArrayRecord, object> formulaArrays = destination.FormulaArrays;
    WorksheetImpl worksheet = (WorksheetImpl) destination.Worksheet;
    if (source.Worksheet != destination.Worksheet)
    {
      List<string> namesFromFormula = this.GetNamesFromFormula(source.Row, source.Column, source.LastRow, source.LastColumn, source.Worksheet);
      if (namesFromFormula.Count > 0)
      {
        foreach (string key in namesFromFormula)
        {
          IName name1 = (IName) null;
          if ((source.Worksheet as WorksheetImpl).InnerNames.m_hashNameToIName.TryGetValue(key, out name1))
          {
            if (!(name1 as NameImpl).IsDeleted && name1.Worksheet != null && !this.IsValidNameExist((IRange) destination, name1.Name))
            {
              IName name2 = destination.Worksheet.Names.Add(name1.Name);
              (name2 as NameImpl).m_isTableNamedRange = (name1 as NameImpl).m_isTableNamedRange;
              (name2 as NameImpl).m_isFormulaNamedRange = (name1 as NameImpl).m_isFormulaNamedRange;
              if (name1.RefersToRange != null)
              {
                name2.RefersToRange = destination.Worksheet[name1.RefersToRange.AddressLocal];
              }
              else
              {
                NameRecord name3 = (NameRecord) ((NameImpl) name1).Record.Clone();
                CellRecordCollection.ChangeReferenceIndex(source.Workbook, destination.Workbook, destination.Worksheet as WorksheetImpl, name3.FormulaTokens);
                (name2 as NameImpl).Parse(name3);
              }
              if ((name1.RefersToRange != null ? (name1.RefersToRange.Worksheet != source.Worksheet ? 1 : 0) : (!(this.m_book.GetFilePath(name1.Value) == "'" + this.m_book.GetFilePath(destination.Workbook.FullFileName)) ? 0 : (this.m_book.GetFileName(name1.RefersTo).Contains(this.m_book.GetFileName(this.m_book.FullFileName)) ? 1 : 0))) != 0)
                this.ChangeReferenceIndex(source.Workbook, destination.Workbook, (name1 as NameImpl).Record, (name2 as NameImpl).Record);
            }
          }
          else if (source.Workbook != destination.Workbook && source.Workbook.InnerNamesColection.m_hashNameToIName.TryGetValue(key, out name1) && !(name1 as NameImpl).IsDeleted && !this.IsValidNameExist((IRange) destination, name1.Name))
          {
            IName name4 = destination.Workbook.Names.Add(name1.Name);
            (name4 as NameImpl).m_isTableNamedRange = (name1 as NameImpl).m_isTableNamedRange;
            (name4 as NameImpl).m_isFormulaNamedRange = (name1 as NameImpl).m_isFormulaNamedRange;
            if (name1.RefersToRange != null)
            {
              name4.RefersToRange = destination.Worksheet[name1.RefersToRange.AddressLocal];
            }
            else
            {
              NameRecord name5 = (NameRecord) ((NameImpl) name1).Record.Clone();
              CellRecordCollection.ChangeReferenceIndex(source.Workbook, destination.Workbook, destination.Worksheet as WorksheetImpl, name5.FormulaTokens);
              (name4 as NameImpl).Parse(name5);
            }
            if ((name1.RefersToRange != null ? (name1.RefersToRange.Worksheet != source.Worksheet ? 1 : 0) : (!(this.m_book.GetFilePath(name1.Value) == "'" + this.m_book.GetFilePath(destination.Workbook.FullFileName)) ? 0 : (this.m_book.GetFileName(name1.RefersTo).Contains(this.m_book.GetFileName(this.m_book.FullFileName)) ? 1 : 0))) != 0)
              this.ChangeReferenceIndex(source.Workbook, destination.Workbook, (name1 as NameImpl).Record, (name4 as NameImpl).Record);
          }
        }
      }
    }
    if ((options & ExcelCopyRangeOptions.UpdateMerges) != ExcelCopyRangeOptions.None)
      this.CopyRangeMerges((IRange) destination, (IRange) source);
    if (formulaArrays != null && formulaArrays.Count > 0)
      worksheet.RemoveArrayFormulas((ICollection<ArrayRecord>) formulaArrays.Keys, true);
    int column1 = destination.Column;
    int row1 = destination.Row;
    int row2 = source.Row;
    int column2 = source.Column;
    int lastRow1 = destination.LastRow;
    int lastColumn1 = destination.LastColumn;
    IListObject table = this.IsTableRange((IRange) destination);
    if (table != null && (source.Workbook != destination.Workbook || source.Worksheet != destination.Worksheet || !(source.AddressGlobal == destination.AddressGlobal)))
    {
      IRange location1 = table.Location;
      int row3 = location1.Row;
      int column3 = location1.Column;
      int lastRow2 = location1.LastRow;
      int lastColumn2 = location1.LastColumn;
      if (row1 >= row3 && column1 >= column3)
      {
        if (row1 == row3 && column1 == column3 && lastColumn1 > lastColumn2)
        {
          int updateCount = lastColumn1 - lastColumn2;
          this.UpdateTableColumn(table, updateCount);
        }
        if (lastRow1 > lastRow2 && (row1 > row3 || column1 > column3 ? (!table.ShowTotals ? 1 : 0) : 1) != 0 && this.IsTableRange((IRange) source) == null)
        {
          IRange location2 = table.Location;
          table.Location = location2.Worksheet[row3, column3, lastRow2 + (lastRow1 - lastRow2), lastColumn2];
        }
      }
      if (this.IsAdjacentTableRow((IRange) destination) != null)
      {
        table = this.IsAdjacentTableRow((IRange) destination);
        IRange location3 = table.Location;
        int updateCount = destination.LastRow - location3.LastRow;
        this.UpdateTableRow(table, updateCount);
      }
    }
    else if (this.IsAdjacentTable((IRange) destination) != null && (source.Workbook != destination.Workbook || source.Worksheet != destination.Worksheet || !(source.AddressGlobal == destination.AddressGlobal)))
    {
      table = this.IsAdjacentTable((IRange) destination);
      if (table != null)
      {
        IRange location = table.Location;
        int updateCount = destination.LastColumn - location.LastColumn;
        this.UpdateTableColumn(table, updateCount);
      }
    }
    else if (this.IsAdjacentTableRow((IRange) destination) != null && (source.Workbook != destination.Workbook || source.Worksheet != destination.Worksheet || !(source.AddressGlobal == destination.AddressGlobal)) && (this.IsTableRange((IRange) source) == null || this.IsEntireTableRange((IRange) source) != null))
    {
      table = this.IsAdjacentTableRow((IRange) destination);
      IRange location = table.Location;
      int updateCount = destination.LastRow - location.LastRow;
      this.UpdateTableRow(table, updateCount);
    }
    if ((options & ExcelCopyRangeOptions.CopyShapes) != ExcelCopyRangeOptions.None)
    {
      Rectangle rec = new Rectangle(column2, row2, iColumnCount - 1, iRowCount - 1);
      Rectangle rectangle = rec with
      {
        X = column1,
        Y = row1
      };
      ShapesCollection shapes = (ShapesCollection) this.Shapes;
      if (copying)
        this.RemoveComments((IWorksheet) worksheet, rectangle);
      shapes.CopyMoveShapeOnRangeCopy(worksheet, rec, rectangle, true);
    }
    this.m_destXFIndex = (int) destination.ExtendedFormatIndex;
    if (options == ExcelCopyRangeOptions.CopyStyles)
    {
      this.MigrantRange.ResetRowColumn(row1, column1);
      if (this.MigrantRange.Value != null)
      {
        this.m_destCell = (string) null;
        if (column1 != destination.LastColumn || row1 != destination.LastRow)
        {
          for (int index1 = column1; index1 <= destination.LastColumn; ++index1)
          {
            for (int index2 = row1; index2 <= destination.LastRow; ++index2)
            {
              this.MigrantRange.ResetRowColumn(index2, index1);
              long cellIndex = RangeImpl.GetCellIndex(index1, index2);
              if (!this.m_destRange.ContainsKey(cellIndex) && !skipBlank)
                this.m_destRange.Add(cellIndex, this.MigrantRange.Value);
            }
          }
        }
      }
    }
    if ((options & ExcelCopyRangeOptions.CopyErrorIndicators) == ExcelCopyRangeOptions.CopyErrorIndicators)
      this.CopyMoveErrorIndicators(row2, column2, iRowCount, iColumnCount, row1, column1, worksheet, false);
    if ((options & ExcelCopyRangeOptions.CopyConditionalFormats) == ExcelCopyRangeOptions.CopyConditionalFormats)
      this.CopyMoveConditionalFormatting(row2, column2, iRowCount, iColumnCount, row1, column1, worksheet, false, copying);
    if ((source.Worksheet as WorksheetImpl).m_dataValidation != null && (source.Worksheet as WorksheetImpl).m_dataValidation.Count > 0 && (options & ExcelCopyRangeOptions.CopyDataValidations) != ExcelCopyRangeOptions.None)
      this.CopyMoveDataValidations(row2, column2, iRowCount, iColumnCount, row1, column1, worksheet, false);
    this.CopyMoveSparkLines((IRange) source, (IRange) destination, worksheet, false);
    this.CopyMoveHyperlinks(row2, column2, iRowCount, iColumnCount, row1, column1, worksheet, false);
    int num = 0;
    if (table != null)
      num = table.Index;
    bool isEntireRowOrColumn = source.IsEntireColumn || source.IsEntireRow;
    this.CopyRange(row2, column2, iRowCount, iColumnCount, row1, column1, worksheet, intersection, rectIntersection, options, copying, skipBlank, isEntireRowOrColumn);
    IListObject listObject = this.IsTableRange((IRange) destination);
    if (listObject != null && listObject.ShowTotals && num == listObject.Index && (source.Workbook != destination.Workbook || source.Worksheet != destination.Worksheet || !(source.AddressGlobal == destination.AddressGlobal)))
    {
      IRange location = listObject.Location;
      if (lastRow1 > location.LastRow)
      {
        for (int index = 0; index < listObject.Columns.Count; ++index)
        {
          IRange range = this.IntersectRanges((IRange) destination, (listObject.Columns[index] as ListObjectColumn).TotalCell);
          if (range != null)
          {
            (listObject.Columns[index] as ListObjectColumn).TotalsRowLabel = range.DisplayText;
            destination.Worksheet.SetText(range.Row, range.Column, range.DisplayText);
            (listObject.Columns[index] as ListObjectColumn).SetTotalsCalculation(ExcelTotalsCalculation.None);
          }
        }
      }
    }
    intersection?.Dispose();
  }

  private void UpdateTableColumn(IListObject table, int updateCount)
  {
    for (; updateCount > 0; --updateCount)
    {
      int count = table.Columns.Count;
      IRange location = table.Location;
      int row = location.Row;
      int column = location.Column;
      int lastRow = location.LastRow;
      int lastColumn = location.LastColumn;
      table.Location = location.Worksheet[row, column, lastRow, lastColumn + 1];
      table.Columns.Insert(count, (IListObjectColumn) new ListObjectColumn("Column" + (object) (count + 1), count + 1, table as ListObject, count + 1));
      (table.Columns[table.Columns.Count - 1] as ListObjectColumn).SetName("Column" + (object) (count + 1));
      location.Worksheet[lastRow, table.Location.LastColumn].Text = "Column" + (object) (count + 1);
    }
  }

  private void UpdateTableRow(IListObject table, int updateCount)
  {
    if (updateCount <= 0)
      return;
    IRange location = table.Location;
    int row = location.Row;
    int column = location.Column;
    int lastRow = location.LastRow;
    int lastColumn = location.LastColumn;
    table.Location = location.Worksheet[row, column, lastRow + updateCount, lastColumn];
  }

  internal List<string> GetNamesFromFormula(
    int firstRow,
    int firstCol,
    int lastRow,
    int lastCol,
    IWorksheet sheet)
  {
    List<string> namesFromFormula = new List<string>();
    WorkbookImpl workbook = sheet.Workbook as WorkbookImpl;
    WorksheetImpl worksheetImpl = sheet as WorksheetImpl;
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(sheet.Application, sheet);
    for (int index1 = firstRow; index1 <= lastRow; ++index1)
    {
      for (int index2 = firstCol; index2 <= lastCol; ++index2)
      {
        if (worksheetImpl.GetCellType(index1, index2, false) == WorksheetImpl.TRangeValueType.Formula)
        {
          Ptg[] arrayFormulaPtg = worksheetImpl.m_dicRecordsCells.Table.GetFormulaValue(index1, index2);
          if (this.HasArrayFormula(arrayFormulaPtg))
            this.GetArrayFormula(index1, index2, false, out arrayFormulaPtg);
          if (arrayFormulaPtg != null)
          {
            foreach (Ptg ptg in arrayFormulaPtg)
            {
              if (ptg is NamePtg)
              {
                string name = workbook.InnerNamesColection.GetNameByIndex((ptg as NamePtg).ExternNameIndexInt - 1).Name;
                if (name != null && !namesFromFormula.Contains(name))
                  namesFromFormula.Add(name);
              }
            }
          }
        }
        if (worksheetImpl.Range[index1, index2].HasDataValidation)
        {
          Ptg[] arrayFormulaPtg1 = (worksheetImpl[index1, index2].DataValidation as DataValidationWrapper).FirstFormulaTokens;
          if (this.HasArrayFormula(arrayFormulaPtg1))
            this.GetArrayFormula(index1, index2, false, out arrayFormulaPtg1);
          if (arrayFormulaPtg1 != null)
          {
            foreach (Ptg ptg in arrayFormulaPtg1)
            {
              if (ptg is NamePtg)
              {
                string name = workbook.InnerNamesColection.GetNameByIndex((ptg as NamePtg).ExternNameIndexInt - 1).Name;
                if (name != null && !namesFromFormula.Contains(name))
                  namesFromFormula.Add(name);
              }
            }
          }
          Ptg[] arrayFormulaPtg2 = (worksheetImpl[index1, index2].DataValidation as DataValidationWrapper).SecondFormulaTokens;
          if (this.HasArrayFormula(arrayFormulaPtg2))
            this.GetArrayFormula(index1, index2, false, out arrayFormulaPtg2);
          if (arrayFormulaPtg2 != null)
          {
            foreach (Ptg ptg in arrayFormulaPtg2)
            {
              if (ptg is NamePtg)
              {
                string name = workbook.InnerNamesColection.GetNameByIndex((ptg as NamePtg).ExternNameIndexInt - 1).Name;
                if (name != null && !namesFromFormula.Contains(name))
                  namesFromFormula.Add(name);
              }
            }
          }
        }
      }
    }
    return namesFromFormula;
  }

  private void CopyMoveSparkLines(
    IRange source,
    IRange destination,
    WorksheetImpl destSheet,
    bool isMove)
  {
    List<ISparklineGroup> sparklineGroupList = new List<ISparklineGroup>();
    if (this.SparklineGroups.Count > 0 && source.Worksheet.SparklineGroups.Count > 0)
    {
      ISparklineGroups sparklineGroups = source.Worksheet.SparklineGroups;
      for (int index1 = 0; index1 < sparklineGroups.Count; ++index1)
      {
        ISparklineGroup sparklineGroup1 = sparklineGroups[index1];
        for (int index2 = 0; index2 < sparklineGroup1.Count; ++index2)
        {
          ISparklines sparkLines = sparklineGroup1[index2];
          List<ISparkline> sparklineList = this.GetSparkLines(source, sparkLines);
          if (this.m_isInsertingRow && sparklineList.Count == 0 || this.m_isDeletingRow)
            sparklineList = source.Row > sparkLines[0].ReferenceRange.Row ? this.GetSparkLines(source[sparkLines[0].ReferenceRange.Row, source.Column, source.LastRow, source.LastColumn], sparkLines) : this.GetSparkLines(source[source.Row - 1, source.Column, source.LastRow, source.LastColumn], sparkLines);
          if (sparklineList != null && sparklineList.Count > 0)
          {
            if (!this.m_isInsertingRow && !this.m_isDeletingRow)
            {
              ISparklineGroup sparklineGroup2 = destSheet.SparklineGroups.Add();
              if (sparklineList.Count > 0)
              {
                sparklineGroup2.Add();
                ISparklineGroup parentGroup = (ISparklineGroup) ((sparklineList[0] as Sparkline).Parent as Sparklines).ParentGroup;
                sparklineGroup2.AxisColor = parentGroup.AxisColor;
                sparklineGroup2.DisplayAxis = parentGroup.DisplayAxis;
                sparklineGroup2.DisplayEmptyCellsAs = parentGroup.DisplayEmptyCellsAs;
                sparklineGroup2.DisplayHiddenRC = parentGroup.DisplayHiddenRC;
                sparklineGroup2.FirstPointColor = parentGroup.FirstPointColor;
                sparklineGroup2.HighPointColor = parentGroup.HighPointColor;
                sparklineGroup2.HorizontalDateAxis = parentGroup.HorizontalDateAxis;
                if (parentGroup.HorizontalDateAxisRange != null)
                  sparklineGroup2.HorizontalDateAxisRange = destSheet[parentGroup.HorizontalDateAxisRange.AddressLocal];
                sparklineGroup2.LastPointColor = parentGroup.LastPointColor;
                sparklineGroup2.LineWeight = parentGroup.LineWeight;
                sparklineGroup2.LowPointColor = parentGroup.LowPointColor;
                sparklineGroup2.MarkersColor = parentGroup.MarkersColor;
                sparklineGroup2.NegativePointColor = parentGroup.NegativePointColor;
                sparklineGroup2.PlotRightToLeft = parentGroup.PlotRightToLeft;
                sparklineGroup2.ShowFirstPoint = parentGroup.ShowFirstPoint;
                sparklineGroup2.ShowHighPoint = parentGroup.ShowHighPoint;
                sparklineGroup2.ShowLastPoint = parentGroup.ShowLastPoint;
                sparklineGroup2.ShowLowPoint = parentGroup.ShowLowPoint;
                sparklineGroup2.ShowMarkers = parentGroup.ShowMarkers;
                sparklineGroup2.ShowNegativePoint = parentGroup.ShowNegativePoint;
                sparklineGroup2.SparklineColor = parentGroup.SparklineColor;
                sparklineGroup2.SparklineType = parentGroup.SparklineType;
                sparklineGroup2.VerticalAxisMaximum.VerticalAxisOptions = parentGroup.VerticalAxisMaximum.VerticalAxisOptions;
                if (sparklineGroup2.VerticalAxisMaximum.VerticalAxisOptions == SparklineVerticalAxisOptions.Custom)
                  sparklineGroup2.VerticalAxisMaximum.CustomValue = parentGroup.VerticalAxisMaximum.CustomValue;
                sparklineGroup2.VerticalAxisMinimum.VerticalAxisOptions = parentGroup.VerticalAxisMinimum.VerticalAxisOptions;
                if (sparklineGroup2.VerticalAxisMinimum.VerticalAxisOptions == SparklineVerticalAxisOptions.Custom)
                  sparklineGroup2.VerticalAxisMinimum.CustomValue = parentGroup.VerticalAxisMinimum.CustomValue;
              }
              while (sparklineList.Count > 0)
              {
                IRange referenceRange = destSheet[destination.Row + (sparklineList[0].ReferenceRange.Row - source.Row), destination.Column + (sparklineList[0].ReferenceRange.Column - source.Column)];
                sparklineGroup2[sparklineGroup2.Count - 1].Add(destSheet[sparklineList[0].DataRange.AddressLocal], referenceRange);
                if (isMove)
                {
                  int index3 = sparkLines.IndexOf(sparklineList[0]);
                  if (index3 >= 0)
                    sparkLines.RemoveAt(index3);
                }
                sparklineList.RemoveAt(0);
              }
            }
            else if (this.m_isDeletingRow)
            {
              ISparklineGroup sparklineGroup3 = destSheet.SparklineGroups[index1];
              ISparklines sparklines = sparklineGroup3[sparklineGroup3.Count - 1];
              while (sparklineList.Count > 0)
              {
                if (sparklineList[0].DataRange != null && sparklineList[0].DataRange.Row == sparklineList[0].DataRange.LastRow && sparklineList[0].ReferenceRange.Row == sparklineList[0].ReferenceRange.LastRow)
                {
                  if (sparklineList[0].DataRange.Row == sparklineList[0].ReferenceRange.Row)
                  {
                    if (sparklineList[0].DataRange.Row == this.m_deleteRowIndex || sparklineList[0].DataRange.Row == this.m_deleteRowIndex + (this.m_deleteRowCount - 1))
                    {
                      int index4 = sparkLines.IndexOf(sparklineList[0]);
                      if (index4 >= 0)
                        sparkLines.RemoveAt(index4);
                      sparklineList.RemoveAt(0);
                    }
                    else if (sparklineList[0].DataRange.Row > this.m_deleteRowIndex + this.m_deleteRowCount - 1)
                    {
                      IRange range = destSheet[destination.Row + (sparklineList[0].ReferenceRange.Row - source.Row), destination.Column + (sparklineList[0].ReferenceRange.Column - source.Column)];
                      int index5 = sparkLines.IndexOf(sparklineList[0]);
                      if (index5 >= 0)
                      {
                        ISparkline sparkline = sparkLines[index5];
                        sparkline.DataRange = destSheet[sparklineList[0].DataRange.Row - this.m_deleteRowCount, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.Row - this.m_deleteRowCount, sparklineList[0].DataRange.LastColumn];
                        sparkline.ReferenceRange = range;
                      }
                      sparklineList.RemoveAt(0);
                    }
                    else
                      sparklineList.RemoveAt(0);
                  }
                  else if (sparklineList[0].ReferenceRange.Row != sparklineList[0].DataRange.Row)
                  {
                    if (sparklineList[0].ReferenceRange.Row == this.m_deleteRowIndex || sparklineList[0].ReferenceRange.Row == this.m_deleteRowIndex + (this.m_deleteRowCount - 1))
                    {
                      int index6 = sparkLines.IndexOf(sparklineList[0]);
                      if (index6 >= 0)
                        sparkLines.RemoveAt(index6);
                      sparklineList.RemoveAt(0);
                    }
                    else if (sparklineList[0].DataRange.Row == this.m_deleteRowIndex || sparklineList[0].DataRange.Row == this.m_deleteRowIndex + (this.m_deleteRowCount - 1))
                    {
                      int index7 = sparkLines.IndexOf(sparklineList[0]);
                      if (index7 >= 0)
                      {
                        ISparkline sparkline = sparkLines[index7];
                        sparkline.DataRange = (IRange) null;
                        if (sparklineList[0].ReferenceRange.Row > this.m_deleteRowIndex + (this.m_deleteRowCount - 1))
                        {
                          IRange range = destSheet[destination.Row + (sparklineList[0].ReferenceRange.Row - source.Row), destination.Column + (sparklineList[0].ReferenceRange.Column - source.Column)];
                          sparkline.ReferenceRange = range;
                        }
                      }
                      sparklineList.RemoveAt(0);
                    }
                    else if (sparklineList[0].DataRange.Row > this.m_deleteRowIndex + (this.m_deleteRowCount - 1) && sparklineList[0].ReferenceRange.Row > this.m_deleteRowIndex + (this.m_deleteRowCount - 1))
                    {
                      IRange range = destSheet[destination.Row + (sparklineList[0].ReferenceRange.Row - source.Row), destination.Column + (sparklineList[0].ReferenceRange.Column - source.Column)];
                      int index8 = sparkLines.IndexOf(sparklineList[0]);
                      if (index8 >= 0)
                      {
                        ISparkline sparkline = sparkLines[index8];
                        sparkline.DataRange = destSheet[sparklineList[0].DataRange.Row - this.m_deleteRowCount, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.Row - this.m_deleteRowCount, sparklineList[0].DataRange.LastColumn];
                        sparkline.ReferenceRange = range;
                      }
                      sparklineList.RemoveAt(0);
                    }
                    else if (sparklineList[0].DataRange.Row > this.m_deleteRowIndex + (this.m_deleteRowCount - 1) && sparklineList[0].ReferenceRange.Row < this.m_deleteRowIndex + (this.m_deleteRowCount - 1))
                    {
                      int index9 = sparkLines.IndexOf(sparklineList[0]);
                      if (index9 >= 0)
                        sparkLines[index9].DataRange = destSheet[sparklineList[0].DataRange.Row - this.m_deleteRowCount, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.Row - this.m_deleteRowCount, sparklineList[0].DataRange.LastColumn];
                      sparklineList.RemoveAt(0);
                    }
                    else if (sparklineList[0].DataRange.Row < this.m_deleteRowIndex + (this.m_deleteRowCount - 1) && sparklineList[0].ReferenceRange.Row > this.m_deleteRowIndex + (this.m_deleteRowCount - 1))
                    {
                      IRange range = destSheet[destination.Row + (sparklineList[0].ReferenceRange.Row - source.Row), destination.Column + (sparklineList[0].ReferenceRange.Column - source.Column)];
                      int index10 = sparkLines.IndexOf(sparklineList[0]);
                      if (index10 >= 0)
                        sparkLines[index10].ReferenceRange = range;
                      sparklineList.RemoveAt(0);
                    }
                    else
                      sparklineList.RemoveAt(0);
                  }
                }
                else if (sparklineList[0].DataRange == null && sparklineList[0].ReferenceRange != null)
                {
                  if (sparklineList[0].ReferenceRange.Row == this.m_deleteRowIndex || sparklineList[0].ReferenceRange.Row == this.m_deleteRowIndex + (this.m_deleteRowCount - 1))
                  {
                    int index11 = sparkLines.IndexOf(sparklineList[0]);
                    if (index11 >= 0)
                      sparkLines.RemoveAt(index11);
                    sparklineList.RemoveAt(0);
                  }
                  else if (sparklineList[0].ReferenceRange.Row > this.m_deleteRowIndex + (this.m_deleteRowCount - 1))
                  {
                    IRange range = destSheet[destination.Row + (sparklineList[0].ReferenceRange.Row - source.Row), destination.Column + (sparklineList[0].ReferenceRange.Column - source.Column)];
                    int index12 = sparkLines.IndexOf(sparklineList[0]);
                    if (index12 >= 0)
                      sparkLines[index12].ReferenceRange = range;
                    sparklineList.RemoveAt(0);
                  }
                  else if (sparklineList[0].ReferenceRange.Row < this.m_deleteRowIndex + (this.m_deleteRowCount - 1))
                    sparklineList.RemoveAt(0);
                  else
                    sparklineList.RemoveAt(0);
                }
                else
                  sparklineList.RemoveAt(0);
              }
            }
            else
            {
              while (sparklineList.Count > 0)
              {
                ISparklineGroup sparklineGroup4 = destSheet.SparklineGroups[index1];
                ISparklines sparklines = sparklineGroup4[sparklineGroup4.Count - 1];
                int num = 0;
                if (sparklineList[0].DataRange.Row == sparklineList[0].DataRange.LastRow)
                {
                  if (this.IsTableRange(source) != null)
                  {
                    if (sparklineList[0].ReferenceRange.Row <= this.insertRowIndex)
                    {
                      if (sparklineList[0].DataRange.Row >= this.insertRowIndex)
                      {
                        for (; num <= this.insertRowCount; ++num)
                          sparklines.Add(destSheet[sparklineList[0].DataRange.Row + num, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.LastRow + num, sparklineList[0].DataRange.LastColumn], destSheet[sparklineList[0].ReferenceRange.Row + num, sparklineList[0].ReferenceRange.Column]);
                      }
                      else
                      {
                        for (; num <= this.insertRowCount; ++num)
                          sparklines.Add(destSheet[sparklineList[0].DataRange.Row - num, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.LastRow - num, sparklineList[0].DataRange.LastColumn], destSheet[sparklineList[0].ReferenceRange.Row + this.insertRowCount - num, sparklineList[0].ReferenceRange.Column]);
                      }
                    }
                    else if (sparklineList[0].DataRange.Row >= this.insertRowIndex)
                      sparklines.Add(destSheet[sparklineList[0].DataRange.Row + this.insertRowCount, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.LastRow + this.insertRowCount, sparklineList[0].DataRange.LastColumn], destSheet[sparklineList[0].ReferenceRange.Row + this.insertRowCount, sparklineList[0].ReferenceRange.Column]);
                    else
                      sparklines.Add(destSheet[sparklineList[0].DataRange.Row, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.LastRow, sparklineList[0].DataRange.LastColumn], destSheet[sparklineList[0].ReferenceRange.Row + this.insertRowCount, sparklineList[0].ReferenceRange.Column]);
                  }
                  else if (sparklineList[0].ReferenceRange.Row < this.insertRowIndex)
                  {
                    if (sparklineList[0].DataRange.Row < this.insertRowIndex)
                    {
                      for (; num <= this.insertRowCount; ++num)
                        sparklines.Add(destSheet[sparklineList[0].DataRange.Row + num, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.LastRow + num, sparklineList[0].DataRange.LastColumn], destSheet[sparklineList[0].ReferenceRange.Row + num, sparklineList[0].ReferenceRange.Column]);
                    }
                    else if (sparklineList[0].DataRange.Row > this.insertRowIndex)
                    {
                      for (; num <= this.insertRowCount; ++num)
                        sparklines.Add(destSheet[sparklineList[0].DataRange.Row + this.insertRowCount + num, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.LastRow + this.insertRowCount + num, sparklineList[0].DataRange.LastColumn], destSheet[sparklineList[0].ReferenceRange.Row + num, sparklineList[0].ReferenceRange.Column]);
                    }
                    else
                      sparklines.Add(destSheet[sparklineList[0].DataRange.Row + this.insertRowCount, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.LastRow + this.insertRowCount, sparklineList[0].DataRange.LastColumn], destSheet[sparklineList[0].ReferenceRange.Row, sparklineList[0].ReferenceRange.Column]);
                  }
                  else if (sparklineList[0].DataRange.Row >= this.insertRowIndex)
                    sparklines.Add(destSheet[sparklineList[0].DataRange.Row + this.insertRowCount, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.LastRow + this.insertRowCount, sparklineList[0].DataRange.LastColumn], destSheet[sparklineList[0].ReferenceRange.Row + this.insertRowCount, sparklineList[0].ReferenceRange.Column]);
                  else
                    sparklines.Add(destSheet[sparklineList[0].DataRange.Row, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.LastRow, sparklineList[0].DataRange.LastColumn], destSheet[sparklineList[0].ReferenceRange.Row + this.insertRowCount, sparklineList[0].ReferenceRange.Column]);
                }
                else if (this.IsTableRange(source) != null)
                {
                  if (sparklineList[0].ReferenceRange.Row <= this.insertRowIndex)
                  {
                    if (sparklineList[0].DataRange.Row > this.insertRowIndex)
                    {
                      for (; num <= this.insertRowCount; ++num)
                        sparklines.Add(destSheet[sparklineList[0].DataRange.Row + this.insertRowCount, sparklineList[0].DataRange.Column - num, sparklineList[0].DataRange.LastRow + this.insertRowCount, sparklineList[0].DataRange.LastColumn - num], destSheet[sparklineList[0].ReferenceRange.Row + this.insertRowCount - num, sparklineList[0].ReferenceRange.Column]);
                    }
                    else
                    {
                      for (; num <= this.insertRowCount; ++num)
                        sparklines.Add(destSheet[sparklineList[0].DataRange.Row, sparklineList[0].DataRange.Column - num, sparklineList[0].DataRange.LastRow, sparklineList[0].DataRange.LastColumn - num], destSheet[sparklineList[0].ReferenceRange.Row + this.insertRowCount - num, sparklineList[0].ReferenceRange.Column]);
                    }
                  }
                  else if (sparklineList[0].DataRange.LastRow >= this.insertRowIndex)
                    sparklines.Add(destSheet[sparklineList[0].DataRange.Row + this.insertRowCount, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.LastRow + this.insertRowCount, sparklineList[0].DataRange.LastColumn], destSheet[sparklineList[0].ReferenceRange.Row + this.insertRowCount, sparklineList[0].ReferenceRange.Column]);
                  else
                    sparklines.Add(destSheet[sparklineList[0].DataRange.Row, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.LastRow, sparklineList[0].DataRange.LastColumn], destSheet[sparklineList[0].ReferenceRange.Row + this.insertRowCount, sparklineList[0].ReferenceRange.Column]);
                }
                else if (sparklineList[0].ReferenceRange.Row < this.insertRowIndex)
                {
                  if (sparklineList[0].DataRange.Row < this.insertRowIndex)
                  {
                    for (; num <= this.insertRowCount; ++num)
                      sparklines.Add(destSheet[sparklineList[0].DataRange.Row + num, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.LastRow + num, sparklineList[0].DataRange.LastColumn], destSheet[sparklineList[0].ReferenceRange.Row + num, sparklineList[0].ReferenceRange.Column]);
                  }
                  else
                  {
                    for (; num <= this.insertRowCount; ++num)
                      sparklines.Add(destSheet[sparklineList[0].DataRange.Row + this.insertRowCount + num, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.LastRow + this.insertRowCount + num, sparklineList[0].DataRange.LastColumn], destSheet[sparklineList[0].ReferenceRange.Row + num, sparklineList[0].ReferenceRange.Column]);
                  }
                }
                else if (sparklineList[0].DataRange.LastRow >= this.insertRowIndex)
                  sparklines.Add(destSheet[sparklineList[0].DataRange.Row + this.insertRowCount, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.LastRow + this.insertRowCount, sparklineList[0].DataRange.LastColumn], destSheet[sparklineList[0].ReferenceRange.Row + this.insertRowCount, sparklineList[0].ReferenceRange.Column]);
                else
                  sparklines.Add(destSheet[sparklineList[0].DataRange.Row, sparklineList[0].DataRange.Column, sparklineList[0].DataRange.LastRow, sparklineList[0].DataRange.LastColumn], destSheet[sparklineList[0].ReferenceRange.Row + this.insertRowCount, sparklineList[0].ReferenceRange.Column]);
                if (isMove)
                {
                  int index13 = sparkLines.IndexOf(sparklineList[0]);
                  if (index13 >= 0)
                    sparkLines.RemoveAt(index13);
                }
                sparklineList.RemoveAt(0);
              }
            }
          }
          if (isMove && sparklineGroup1[index2].Count <= 0)
            sparklineGroupList.Add(sparklineGroup1);
        }
      }
    }
    if (!isMove)
      return;
    for (int index = 0; index < sparklineGroupList.Count; ++index)
      source.Worksheet.SparklineGroups.Remove(sparklineGroupList[index]);
  }

  internal List<ISparkline> GetSparkLines(IRange source, ISparklines sparkLines)
  {
    List<ISparkline> sparkLines1 = new List<ISparkline>();
    List<IRange> rangeList = new List<IRange>((IEnumerable<IRange>) source.Cells);
    while (rangeList.Count > 0)
    {
      bool flag = false;
      foreach (ISparkline sparkLine in (IEnumerable<ISparkline>) sparkLines)
      {
        if (sparkLine.ReferenceRange.AddressLocal == rangeList[0].AddressLocal)
        {
          sparkLines1.Add(sparkLine);
          rangeList.RemoveAt(0);
          flag = true;
          break;
        }
      }
      if (!flag)
        rangeList.RemoveAt(0);
    }
    rangeList.Clear();
    return sparkLines1;
  }

  private void RemoveComments(IWorksheet destSheets, Rectangle destRect)
  {
    int y = destRect.Y;
    int x = destRect.X;
    int num1 = destRect.Y + destRect.Height;
    int num2 = destRect.X + destRect.Width;
    for (int iRow = y; iRow <= num1; ++iRow)
    {
      for (int iColumn = x; iColumn <= num2; ++iColumn)
      {
        ICommentShape innerComment = (destSheets as WorksheetImpl).InnerComments[iRow, iColumn];
        if (innerComment != null)
          (destSheets as WorksheetImpl).InnerComments.InnerRemove(innerComment);
      }
    }
  }

  internal IRange GetRangeFromMergedRegion(IRange range)
  {
    Rectangle rect2 = new Rectangle(range.Column - 1, range.Row - 1, range.LastColumn - range.Column, range.LastRow - range.Row);
    foreach (Rectangle mergedRegion in (range.Worksheet as WorksheetImpl).MergeCells.MergedRegions)
    {
      if (UtilityMethods.Intersects(mergedRegion, rect2))
      {
        int x = Math.Min(mergedRegion.X, rect2.X);
        int y = Math.Min(mergedRegion.Y, rect2.Y);
        int num1 = Math.Max(mergedRegion.Width + mergedRegion.X, rect2.Width + rect2.X);
        int num2 = Math.Max(mergedRegion.Height + mergedRegion.Y, rect2.Height + rect2.Y);
        rect2 = new Rectangle(x, y, num1 - x, num2 - y);
      }
    }
    return range.Worksheet[rect2.Y + 1, rect2.X + 1, rect2.Y + rect2.Height + 1, rect2.X + rect2.Width + 1];
  }

  private void CopyMoveDataValidations(
    int iSourceRow,
    int iSourceColumn,
    int iRowCount,
    int iColumnCount,
    int iDestRow,
    int iDestColumn,
    WorksheetImpl destSheet,
    bool bIsMove)
  {
    if (destSheet == null)
      throw new ArgumentNullException(nameof (destSheet));
    destSheet.ParseData();
    this.ParseData();
    if (iSourceColumn == int.MaxValue || iDestColumn == int.MaxValue || iSourceRow == -1 || iDestRow == -1)
      return;
    DataValidationTable destDataValidation = destSheet.m_dataValidation;
    bool flag1 = this.m_dataValidation == null || this.m_dataValidation.Count == 0;
    bool flag2 = destDataValidation == null || destDataValidation.Count == 0;
    if (flag1 && flag2)
      return;
    if (flag1)
    {
      Rectangle rectangle = new Rectangle(iDestColumn - 1, iDestRow - 1, iColumnCount, iRowCount);
      destDataValidation.Remove(new Rectangle[1]
      {
        rectangle
      });
    }
    else if (flag2)
      destDataValidation = destSheet.DVTable;
    if (!bIsMove)
    {
      Rectangle rectangle1 = new Rectangle(iSourceColumn - 1, iSourceRow - 1, iColumnCount - 1, iRowCount - 1);
      Rectangle rectangle2 = new Rectangle(iDestColumn - 1, iDestRow - 1, iColumnCount - 1, iRowCount - 1);
      if (rectangle1 != rectangle2)
        destSheet.DVTable.Remove(new Rectangle[1]
        {
          rectangle2
        });
    }
    Rectangle[] rectangles;
    if (iSourceRow == iDestRow && iSourceColumn == iDestColumn)
      rectangles = new Rectangle[1]
      {
        Rectangle.FromLTRB(iDestColumn, iDestRow, iDestColumn + (iColumnCount - 1), iDestRow + (iRowCount - 1))
      };
    else
      rectangles = new Rectangle[1]
      {
        Rectangle.FromLTRB(iDestColumn - 1, iDestRow - 1, iDestColumn + (iColumnCount - 1), iDestRow + (iRowCount - 1))
      };
    if (bIsMove && this != destSheet)
      destSheet.DVTable.Remove(rectangles);
    if (this.m_dataValidation != null)
    {
      this.m_dataValidation.CopyMoveTo(destDataValidation, iSourceRow, iSourceColumn, iDestRow, iDestColumn, iRowCount, iColumnCount, bIsMove);
      if (this.m_dataValidation.Count == 0)
        this.m_dataValidation = (DataValidationTable) null;
    }
    if (destDataValidation.Count == 0)
      destSheet.m_dataValidation = (DataValidationTable) null;
    if (destSheet.m_dataValidation == null)
      return;
    foreach (CollectionBase<DataValidationImpl> inner in destSheet.m_dataValidation.InnerList)
    {
      foreach (DataValidationImpl dataValidationImpl in inner)
      {
        CellRecordCollection.ChangeReferenceIndex(this.Workbook as WorkbookImpl, destSheet.Workbook as WorkbookImpl, destSheet, dataValidationImpl.FirstFormulaTokens);
        CellRecordCollection.ChangeReferenceIndex(this.Workbook as WorkbookImpl, destSheet.Workbook as WorkbookImpl, destSheet, dataValidationImpl.SecondFormulaTokens);
      }
    }
  }

  private void CopyMoveConditionalFormatting(
    int iSourceRow,
    int iSourceColumn,
    int iRowCount,
    int iColumnCount,
    int iDestRow,
    int iDestColumn,
    WorksheetImpl destSheet,
    bool bIsMove,
    bool copying)
  {
    if (destSheet == null)
      throw new ArgumentNullException(nameof (destSheet));
    destSheet.ParseSheetCF();
    this.ParseSheetCF();
    if (iSourceColumn == int.MaxValue || iDestColumn == int.MaxValue || iSourceRow == -1 || iDestRow == -1)
      return;
    WorksheetConditionalFormats conditionalFormats = destSheet.ConditionalFormats;
    if (!bIsMove)
    {
      Rectangle rectangle1 = new Rectangle(iSourceColumn - 1, iSourceRow - 1, iColumnCount - 1, iRowCount - 1);
      Rectangle rectangle2 = new Rectangle(iDestColumn - 1, iDestRow - 1, iColumnCount - 1, iRowCount - 1);
      if (rectangle1 != rectangle2)
        conditionalFormats.Remove(new Rectangle[1]
        {
          rectangle2
        }, false);
    }
    int i1 = 0;
    for (int count1 = this.m_arrConditionalFormats.Count; i1 < count1; ++i1)
    {
      Syncfusion.XlsIO.Implementation.Collections.ConditionalFormats conditionalFormat = this.m_arrConditionalFormats[i1];
      int num1 = iDestColumn - iSourceColumn;
      int num2 = iDestRow - iSourceRow;
      int index = destSheet.Index;
      Syncfusion.XlsIO.Implementation.Collections.ConditionalFormats part = conditionalFormat.GetPart(iSourceRow, iSourceColumn, iRowCount, iColumnCount, bIsMove, num2, num1, (object) conditionalFormats);
      if (!bIsMove)
      {
        Rectangle sourceRect = new Rectangle(iSourceColumn - 1, iSourceRow - 1, iColumnCount, iRowCount);
        Rectangle destRect = new Rectangle(iSourceColumn - 1, iSourceRow - 1, 0, 0);
        destRect.Offset(num1, num2);
        bool flag = false;
        if (part != null)
        {
          for (int i2 = 0; i2 < part.Count; ++i2)
          {
            if (part[i2].FormatType == ExcelCFType.Formula || !string.IsNullOrEmpty(part[i2].FirstFormula) || !string.IsNullOrEmpty(part[i2].SecondFormula))
            {
              flag = true;
              break;
            }
          }
        }
        if (flag)
        {
          part.IsCopying = copying;
          part.UpdateFormula(index, index, sourceRect, index, destRect);
        }
      }
      if (conditionalFormat.IsEmpty)
      {
        this.m_arrConditionalFormats.RemoveItem(conditionalFormat);
        --i1;
        --count1;
      }
      if (part != null)
      {
        IRange copyToRange = (IRange) destSheet.CopyToRange;
        int count2 = destSheet.ConditionalFormats.Count;
        this.UpdatePriority(destSheet, part, copying);
        if (copyToRange != null && count2 > 0)
        {
          this.UpdateMulticellCF(part, copyToRange, count2, destSheet, conditionalFormats, copying);
        }
        else
        {
          if (copying)
            part.CondFMTRecord.Index = (ushort) (conditionalFormats.Count + 1);
          conditionalFormats.Add(part);
          part.AddCells((IList<Rectangle>) part.CellRectangles);
        }
      }
      destSheet.m_parseCondtionalFormats = false;
    }
  }

  private void UpdatePriority(
    WorksheetImpl destSheet,
    Syncfusion.XlsIO.Implementation.Collections.ConditionalFormats destFormats,
    bool copying)
  {
    WorksheetConditionalFormats conditionalFormats1 = destSheet.ConditionalFormats;
    int count1 = destFormats.Count;
    if (copying)
    {
      int count2 = destFormats.InnerList.Count;
      Dictionary<int, ConditionalFormatImpl> dictionary = new Dictionary<int, ConditionalFormatImpl>();
      for (int index = 0; index < count2; ++index)
      {
        ConditionalFormatImpl inner = (ConditionalFormatImpl) destFormats.InnerList[index];
        if (!dictionary.ContainsKey(inner.Priority))
          dictionary.Add(inner.Priority, inner);
      }
      List<int> intList = new List<int>((IEnumerable<int>) dictionary.Keys);
      intList.Sort();
      int num = 1;
      foreach (int key in intList)
      {
        dictionary[key].Priority = num;
        ++num;
      }
      if (destSheet.m_bisCFCopied)
      {
        for (int i = 0; i < conditionalFormats1.Count; ++i)
        {
          Syncfusion.XlsIO.Implementation.Collections.ConditionalFormats conditionalFormats2 = conditionalFormats1[i];
          for (int index = 0; index < conditionalFormats2.InnerList.Count; ++index)
          {
            ConditionalFormatImpl inner = (ConditionalFormatImpl) conditionalFormats2.InnerList[index];
            if (inner.Priority > 0)
              inner.Priority += count1;
            else
              inner.Priority += count1 + 1;
          }
        }
      }
    }
    if (!destSheet.m_bisCFCopied)
      return;
    destSheet.m_bisCFCopied = false;
  }

  internal void UpdateMulticellCF(
    Syncfusion.XlsIO.Implementation.Collections.ConditionalFormats destFormats,
    IRange copyToRange,
    int destSheetCFCount,
    WorksheetImpl destSheet,
    WorksheetConditionalFormats destSheetCondFormats,
    bool copying)
  {
    bool flag = false;
    Rectangle cellRectangle1 = destFormats.CellRectangles[0];
    if (cellRectangle1.Y + 1 < copyToRange.Row || cellRectangle1.Y + 1 > copyToRange.LastRow || cellRectangle1.X + 1 < copyToRange.Column || cellRectangle1.X + 1 > copyToRange.LastColumn)
      return;
    for (int i = 0; i < destSheetCFCount; ++i)
    {
      Syncfusion.XlsIO.Implementation.Collections.ConditionalFormats conditionalFormat = destSheet.ConditionalFormats[i];
      Rectangle cellRectangle2 = conditionalFormat.CellRectangles[0];
      if (copyToRange.Row >= cellRectangle2.Y + 1 && copyToRange.Row <= cellRectangle2.Y + cellRectangle2.Height + 1 && copyToRange.Column >= cellRectangle2.X + 1 && copyToRange.Column <= cellRectangle2.X + cellRectangle2.Width + 1)
      {
        flag = true;
        if (conditionalFormat.CondFMT12Record != null)
          conditionalFormat.AddCellsCondFMT12(destFormats);
        else
          conditionalFormat.AddCells(destFormats);
      }
    }
    if (flag)
      return;
    if (copying)
      destFormats.CondFMTRecord.Index = (ushort) (destSheetCondFormats.Count + 1);
    destSheetCondFormats.Add(destFormats);
  }

  private void CopyMoveHyperlinks(
    int iSourceRow,
    int iSourceColumn,
    int iRowCount,
    int iColumnCount,
    int iDestRow,
    int iDestColumn,
    WorksheetImpl destSheet,
    bool bIsMove)
  {
    if (destSheet == null)
      throw new ArgumentNullException(nameof (destSheet));
    if (iSourceColumn == int.MaxValue || iDestColumn == int.MaxValue || iSourceRow == -1 || iDestRow == -1)
      return;
    IRange usedRange1 = this.UsedRange;
    IRange usedRange2 = destSheet.UsedRange;
    MigrantRangeImpl destRange = new MigrantRangeImpl(destSheet.Application, (IWorksheet) destSheet);
    if (iDestRow > iSourceRow)
    {
      for (int row = iRowCount - 1; row >= 0; --row)
      {
        if (iDestColumn > iSourceColumn)
        {
          for (int column = iColumnCount - 1; column >= 0; --column)
            this.CopyMoveHyperlinks(iSourceRow, iSourceColumn, iDestRow, iDestColumn, destSheet, usedRange1, usedRange2, destRange, bIsMove, row, column);
        }
        else
        {
          for (int column = 0; column < iColumnCount; ++column)
            this.CopyMoveHyperlinks(iSourceRow, iSourceColumn, iDestRow, iDestColumn, destSheet, usedRange1, usedRange2, destRange, bIsMove, row, column);
        }
      }
    }
    else
    {
      for (int row = 0; row < iRowCount; ++row)
      {
        if (iDestColumn > iSourceColumn)
        {
          for (int column = iColumnCount - 1; column >= 0; --column)
            this.CopyMoveHyperlinks(iSourceRow, iSourceColumn, iDestRow, iDestColumn, destSheet, usedRange1, usedRange2, destRange, bIsMove, row, column);
        }
        else
        {
          for (int column = 0; column < iColumnCount; ++column)
            this.CopyMoveHyperlinks(iSourceRow, iSourceColumn, iDestRow, iDestColumn, destSheet, usedRange1, usedRange2, destRange, bIsMove, row, column);
        }
      }
    }
  }

  private void CopyMoveHyperlinks(
    int iSourceRow,
    int iSourceColumn,
    int iDestRow,
    int iDestColumn,
    WorksheetImpl destSheet,
    IRange srcUsedRange,
    IRange destUsedRange,
    MigrantRangeImpl destRange,
    bool bIsMove,
    int row,
    int column)
  {
    if ((iSourceRow + row > srcUsedRange.LastRow || iSourceColumn + column > srcUsedRange.LastColumn) && (iDestRow + row > destUsedRange.LastRow || iDestColumn + column > destUsedRange.LastColumn) || iSourceRow == iDestRow && iSourceColumn == iDestColumn && this == destSheet)
      return;
    if (this.m_hyperlinks != null && this.m_hyperlinks.GetHyperlinkByCellIndex(RangeImpl.GetCellIndex(iSourceColumn + column, iSourceRow + row)) != null)
    {
      destRange.ResetRowColumn(iDestRow + row, iDestColumn + column);
      IHyperLink hyperLink = destSheet.HyperLinks.Add((IRange) destRange);
      IHyperLink hyperlinkByCellIndex = this.m_hyperlinks.GetHyperlinkByCellIndex(RangeImpl.GetCellIndex(iSourceColumn + column, iSourceRow + row));
      hyperLink.Type = hyperlinkByCellIndex.Type;
      hyperLink.TextToDisplay = hyperlinkByCellIndex.TextToDisplay;
      hyperLink.Address = hyperlinkByCellIndex.Address;
      hyperLink.ScreenTip = hyperlinkByCellIndex.ScreenTip;
      if (!bIsMove)
        return;
      this.RemoveHyperlink((IWorksheet) this, iSourceRow + row, iSourceColumn + column);
    }
    else
    {
      if (destSheet.m_hyperlinks == null || destSheet.m_hyperlinks.GetHyperlinkByCellIndex(RangeImpl.GetCellIndex(iDestColumn + column, iDestRow + row)) == null)
        return;
      this.RemoveHyperlink((IWorksheet) destSheet, iDestRow + row, iDestColumn + column);
    }
  }

  private void RemoveHyperlink(IWorksheet worksheet, int row, int col)
  {
    bool flag = false;
    for (int index = 0; index < worksheet.HyperLinks.Count && !flag; ++index)
    {
      if (worksheet.HyperLinks[index].Range.Row == row && worksheet.HyperLinks[index].Range.Column == col)
      {
        (worksheet.HyperLinks as HyperLinksCollection).RemoveHyperlinksWithoutClearingFormat(index);
        flag = true;
      }
    }
  }

  private void CopyMoveErrorIndicators(
    int iSourceRow,
    int iSourceColumn,
    int iRowCount,
    int iColumnCount,
    int iDestRow,
    int iDestColumn,
    WorksheetImpl destSheet,
    bool bIsMove)
  {
    ErrorIndicatorsCollection indicatorsCollection = destSheet != null ? destSheet.m_errorIndicators : throw new ArgumentNullException(nameof (destSheet));
    foreach (KeyValuePair<long, ErrorIndicatorImpl> errorIndicator1 in this.m_errorIndicators.GetErrorIndicators(new Rectangle(iSourceColumn - 1, iSourceRow - 1, iColumnCount - 1, iRowCount - 1)))
    {
      Rectangle[] arrRanges = new Rectangle[1]
      {
        new Rectangle(RangeImpl.GetColumnFromCellIndex(errorIndicator1.Key) - 1, RangeImpl.GetRowFromCellIndex(errorIndicator1.Key) - 1, 0, 0)
      };
      ErrorIndicatorImpl errorIndicatorImpl1 = indicatorsCollection.Find(arrRanges);
      if (bIsMove && errorIndicatorImpl1 == null)
        errorIndicator1.Value.Remove(arrRanges);
      ErrorIndicatorImpl errorIndicator2 = new ErrorIndicatorImpl(errorIndicator1.Value.IgnoreOptions);
      List<Rectangle> arrCells = new List<Rectangle>();
      Rectangle rectangle = new Rectangle(RangeImpl.GetColumnFromCellIndex(errorIndicator1.Key) - 1 + (iDestColumn - iSourceColumn), RangeImpl.GetRowFromCellIndex(errorIndicator1.Key) + (iDestRow - iSourceRow) - 1, 0, 0);
      arrCells.Add(rectangle);
      ErrorIndicatorImpl errorIndicatorImpl2 = indicatorsCollection.Find(new Rectangle[1]
      {
        rectangle
      });
      if (errorIndicatorImpl2 != null && errorIndicatorImpl2.IgnoreOptions != errorIndicator1.Value.IgnoreOptions)
      {
        errorIndicatorImpl2.AddCells((IList<Rectangle>) arrCells);
      }
      else
      {
        errorIndicator2.AddCells((IList<Rectangle>) arrCells);
        indicatorsCollection.Add(errorIndicator2);
      }
    }
  }

  private void CopyCell(
    ICellPositionFormat cell,
    string strFormulaValue,
    IDictionary dicXFIndexes,
    long lNewIndex,
    WorkbookImpl book,
    Dictionary<int, int> dicFontIndexes,
    ExcelCopyRangeOptions options,
    IRange sourceRange,
    bool skipBlank)
  {
    this.m_sourceRange = sourceRange;
    this.CopyCell(cell, strFormulaValue, dicXFIndexes, lNewIndex, book, dicFontIndexes, options, sourceRange.Worksheet as WorksheetImpl, skipBlank);
  }

  [CLSCompliant(false)]
  public void CopyCell(
    ICellPositionFormat cell,
    string strFormulaValue,
    IDictionary dicXFIndexes,
    long lNewIndex,
    WorkbookImpl book,
    Dictionary<int, int> dicFontIndexes,
    ExcelCopyRangeOptions options)
  {
    this.CopyCell(cell, strFormulaValue, dicXFIndexes, lNewIndex, book, dicFontIndexes, options, this, false);
  }

  [CLSCompliant(false)]
  internal void CopyCell(
    ICellPositionFormat cell,
    string strFormulaValue,
    IDictionary dicXFIndexes,
    long lNewIndex,
    WorkbookImpl book,
    Dictionary<int, int> dicFontIndexes,
    ExcelCopyRangeOptions options,
    WorksheetImpl sourceSheet,
    bool skipBlank)
  {
    this.ParseData();
    int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(lNewIndex);
    int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(lNewIndex);
    this.m_destXFIndex = (int) (this[rowFromCellIndex, columnFromCellIndex] as RangeImpl).ExtendedFormatIndex;
    if (!skipBlank || sourceSheet.GetCellType(cell.Row + 1, cell.Column + 1, false) != WorksheetImpl.TRangeValueType.Blank || sourceSheet[cell.Row + 1, cell.Column + 1].IsMerged)
      this.m_dicRecordsCells.CopyCell(cell, strFormulaValue, dicXFIndexes, lNewIndex, book, dicFontIndexes, options, this.m_destXFIndex);
    if (options == ExcelCopyRangeOptions.None && cell.TypeCode == TBIFFRecord.Formula)
    {
      rowFromCellIndex = RangeImpl.GetRowFromCellIndex(lNewIndex);
      columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(lNewIndex);
      bool flag = false;
      if (this.m_calcEngine == null)
      {
        this.EnableSheetCalculations();
        flag = true;
      }
      if (this.m_sourceRange != null)
      {
        this.CopyRangeValues(this.m_sourceRange.Worksheet as WorksheetImpl, this, cell.Row + 1, cell.Column + 1, rowFromCellIndex, columnFromCellIndex);
        this.m_sourceRange = (IRange) null;
      }
      else
        this.CopyRangeValues(this.Range.Worksheet as WorksheetImpl, this, cell.Row + 1, cell.Column + 1, rowFromCellIndex, columnFromCellIndex);
      if (flag)
        this.DisableSheetCalculations();
    }
    if ((options == ExcelCopyRangeOptions.CopyStyles || options == ExcelCopyRangeOptions.CopyValueAndSourceFormatting) && cell.TypeCode == TBIFFRecord.Formula || cell.TypeCode == TBIFFRecord.Number || cell.TypeCode == TBIFFRecord.LabelSST || cell.TypeCode == TBIFFRecord.BoolErr || cell.TypeCode == TBIFFRecord.RK || cell.TypeCode == TBIFFRecord.Blank)
    {
      rowFromCellIndex = RangeImpl.GetRowFromCellIndex(lNewIndex);
      columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(lNewIndex);
      bool flag = true;
      if (options == ExcelCopyRangeOptions.CopyStyles)
      {
        if (this.m_calcEngine == null)
        {
          this.EnableSheetCalculations();
          flag = true;
        }
        if (this.m_destCell != null)
        {
          this.Range[rowFromCellIndex, columnFromCellIndex].Value = this.m_destCell;
        }
        else
        {
          long cellIndex = RangeImpl.GetCellIndex(columnFromCellIndex, rowFromCellIndex);
          if (this.m_destRange.ContainsKey(cellIndex))
            this.Range[rowFromCellIndex, columnFromCellIndex].Value = this.m_destRange[cellIndex];
        }
      }
      if (options == ExcelCopyRangeOptions.CopyValueAndSourceFormatting)
      {
        if (this.m_calcEngine == null)
        {
          this.EnableSheetCalculations();
          flag = true;
        }
        if (this.m_sourceRange != null)
        {
          this.CopyRangeValues(this.m_sourceRange.Worksheet as WorksheetImpl, this, cell.Row + 1, cell.Column + 1, rowFromCellIndex, columnFromCellIndex);
          this.Range[rowFromCellIndex, columnFromCellIndex].NumberFormat = this.m_sourceRange.Worksheet.Range[cell.Row + 1, cell.Column + 1].NumberFormat;
          this.m_sourceRange = (IRange) null;
        }
      }
      if (flag)
        this.DisableSheetCalculations();
    }
    int row = cell.Row;
    int column = cell.Column;
    this.m_dicRecordsCells.GetRange(lNewIndex)?.UpdateRecord();
    IRange destRange = this.Range[rowFromCellIndex, columnFromCellIndex];
    IListObject listObject = this.CheckTableRange(destRange);
    if (listObject == null || options == ExcelCopyRangeOptions.CopyStyles || !(this.m_destCell != destRange.DisplayText))
      return;
    if (destRange.Value != string.Empty)
    {
      string numberFormat = destRange.NumberFormat;
      string str1 = destRange.DisplayText;
      string str2 = str1;
      destRange.Value = this.m_destCell;
      int num = 1;
      for (int index = 0; index < listObject.Columns.Count; ++index)
      {
        if (listObject.Columns[index].Name == str1)
        {
          ++num;
          if (num > 1)
            str1 = str2 + num.ToString();
        }
      }
      destRange.Text = str1;
      destRange.NumberFormat = numberFormat;
    }
    else
    {
      string str = "Column1";
      int num = int.Parse(str[str.Length - 1].ToString(), NumberStyles.Number);
      for (int index = 0; index < listObject.Columns.Count; ++index)
      {
        if (this.Range[listObject.Location.Row, listObject.Location.Column + index].DisplayText == str)
          ++num;
        str = "Column" + num.ToString();
      }
      destRange.Value = str;
    }
  }

  internal bool CheckOverLab(IRange range)
  {
    bool flag = false;
    WorksheetImpl worksheet = range.Worksheet as WorksheetImpl;
    if (!worksheet.IsParsing)
    {
      int row1 = range.Row;
      int column1 = range.Column;
      int lastRow1 = range.LastRow;
      int lastColumn1 = range.LastColumn;
      for (int index = 0; index < worksheet.ListObjects.Count; ++index)
      {
        IListObject listObject = worksheet.ListObjects[index];
        int row2 = listObject.Location.Row;
        int column2 = listObject.Location.Column;
        int lastRow2 = listObject.Location.LastRow;
        int lastColumn2 = listObject.Location.LastColumn;
        if (row1 > row2 - 1 && row1 < lastRow2 + 1 && column1 > column2 - 1 && column1 < lastColumn2 + 1)
          flag = true;
      }
    }
    return flag;
  }

  public void CopyRange(
    int iSourceRow,
    int iSourceColumn,
    int iRowCount,
    int iColumnCount,
    int iDestRow,
    int iDestColumn,
    WorksheetImpl destSheet,
    RecordTable intersection,
    Rectangle rectIntersection,
    ExcelCopyRangeOptions options)
  {
    this.CopyRange(iSourceRow, iSourceColumn, iRowCount, iColumnCount, iDestRow, iDestColumn, destSheet, intersection, rectIntersection, options, false, false, false);
  }

  private void CopyRange(
    int iSourceRow,
    int iSourceColumn,
    int iRowCount,
    int iColumnCount,
    int iDestRow,
    int iDestColumn,
    WorksheetImpl destSheet,
    RecordTable intersection,
    Rectangle rectIntersection,
    ExcelCopyRangeOptions options,
    bool copying,
    bool skipBlank,
    bool isEntireRowOrColumn)
  {
    this.ParseData();
    Dictionary<int, int> dicFontIndexes = (Dictionary<int, int>) null;
    Dictionary<int, int> dicXFIndexes = (Dictionary<int, int>) null;
    int lastRow1 = iSourceRow + iRowCount - 1;
    int lastColumn1 = iSourceColumn + iColumnCount - 1;
    IRange range1 = this.Range[iSourceRow, iSourceColumn, lastRow1, lastColumn1];
    bool flag1 = (options & ExcelCopyRangeOptions.CopyStyles) != ExcelCopyRangeOptions.None || (options & ExcelCopyRangeOptions.CopyValueAndSourceFormatting) != ExcelCopyRangeOptions.None;
    if (flag1)
    {
      dicXFIndexes = this.GetUpdatedXFIndexes(iSourceRow, iSourceColumn, iRowCount, iColumnCount, destSheet, out dicFontIndexes);
      if ((options & ExcelCopyRangeOptions.CopyConditionalFormats) != ExcelCopyRangeOptions.CopyConditionalFormats)
        this.CopyMoveConditionalFormatting(iSourceRow, iSourceColumn, iRowCount, iColumnCount, iDestRow, iDestColumn, destSheet, false, copying);
    }
    CellRecordCollection cellRecords = destSheet.CellRecords;
    int iDeltaColumn = iDestColumn - iSourceColumn;
    int iDeltaRow = iDestRow - iSourceRow;
    Dictionary<long, long> dictionary1 = new Dictionary<long, long>();
    RecordTable table = destSheet.CellRecords.Table;
    int allocationBlockSize = this.Application.RowStorageAllocationBlockSize;
    bool flag2 = false;
    int num1 = 0;
    for (int index1 = 0; index1 < iRowCount; ++index1)
    {
      int num2 = iDestRow + index1 - num1;
      int num3 = iSourceRow + index1;
      for (int index2 = 0; index2 < iColumnCount; ++index2)
      {
        int num4 = iDestColumn + index2;
        int num5 = iSourceColumn + index2;
        IRange range2 = this.Range[num3, num5, num3, num5];
        bool flag3 = false;
        if (isEntireRowOrColumn && range2.IsMerged && range2.MergeArea.Column >= range1.Column && range2.MergeArea.LastColumn <= range1.LastColumn && range2.MergeArea.Row >= range1.Row && range2.MergeArea.LastRow <= range1.LastRow)
          flag3 = true;
        if (!isEntireRowOrColumn || !range2.IsMerged || flag3)
        {
          long cellIndex1 = RangeImpl.GetCellIndex(num4, num2);
          RecordTable recordTable = this.GetRecordTable(num3, num5, rectIntersection, intersection, this.m_dicRecordsCells.Table);
          RowStorage row = recordTable.Rows[num3 - 1];
          ICellPositionFormat record = row?.GetRecord(num5 - 1, allocationBlockSize);
          if (index1 < 1 || index1 > this.m_book.MaxRowCount || index2 < 1 || index2 > this.m_book.MaxColumnCount)
          {
            IRange cell = destSheet.InnerGetCell(num4, num2);
            destSheet.m_destCell = cell.Value;
            if (!skipBlank)
              cell.Clear();
          }
          if (record != null)
          {
            if (!row.IsHidden || !row.IsFilteredRow)
            {
              string formulaStringValue = row.GetFormulaStringValue(num5 - 1);
              if (!destSheet.Equals((object) this) && options == ExcelCopyRangeOptions.None || options == ExcelCopyRangeOptions.CopyValueAndSourceFormatting)
                destSheet.CopyCell(record, formulaStringValue, (IDictionary) dicXFIndexes, cellIndex1, this.m_book, dicFontIndexes, options, this[iSourceRow, iSourceColumn], skipBlank);
              else
                destSheet.CopyCell(record, formulaStringValue, (IDictionary) dicXFIndexes, cellIndex1, this.m_book, dicFontIndexes, options, this, skipBlank);
              if (row.IsWrapText && destSheet.Index == this.Index && recordTable.Rows[num2 - 1] != null)
                recordTable.Rows[num2 - 1].IsWrapText = true;
              else if (row.IsWrapText && destSheet.Index != this.Index)
                table.Rows[num2 - 1].IsWrapText = true;
              if (record.TypeCode == TBIFFRecord.Formula)
              {
                ArrayRecord arrayRecord = recordTable.GetArrayRecord(record);
                if (arrayRecord != null)
                {
                  long cellIndex2 = RangeImpl.GetCellIndex(arrayRecord.FirstColumn, arrayRecord.FirstRow);
                  int num6;
                  int num7;
                  if (dictionary1.ContainsKey(cellIndex2))
                  {
                    long index3 = dictionary1[cellIndex2];
                    num6 = RangeImpl.GetRowFromCellIndex(index3);
                    num7 = RangeImpl.GetColumnFromCellIndex(index3);
                  }
                  else
                  {
                    num6 = num2 - 1;
                    num7 = num4 - 1;
                    long cellIndex3 = RangeImpl.GetCellIndex(num7, num6);
                    dictionary1[cellIndex2] = cellIndex3;
                    arrayRecord.FirstColumn = Math.Max(arrayRecord.FirstColumn, iSourceColumn - 1) + iDeltaColumn;
                    arrayRecord.FirstRow = Math.Max(arrayRecord.FirstRow, iSourceRow - 1) + iDeltaRow;
                    arrayRecord.LastColumn = Math.Min(arrayRecord.LastColumn, iSourceColumn + iColumnCount - 2);
                    arrayRecord.LastColumn += iDeltaColumn;
                    arrayRecord.LastRow = Math.Min(arrayRecord.LastRow, iSourceRow + iRowCount - 2);
                    arrayRecord.LastRow += iDeltaRow;
                    if ((options & ExcelCopyRangeOptions.UpdateFormulas) != ExcelCopyRangeOptions.None)
                      this.UpdateArrayFormula(arrayRecord, (IWorksheet) destSheet, iDeltaRow, iDeltaColumn);
                    table.Rows[num2 - 1].SetArrayRecord(num4 - 1, arrayRecord, this.Application.RowStorageAllocationBlockSize);
                  }
                  table.Rows[num2 - 1]?.SetArrayFormulaIndex(num4 - 1, num6, num7, this.Application.RowStorageAllocationBlockSize);
                }
              }
            }
            else
            {
              ++num1;
              break;
            }
          }
          else if (this.CheckTableRange(destSheet[iDestRow, iDestColumn]) != null)
          {
            IRange destRange = destSheet[iDestRow, iDestColumn];
            IListObject listObject = this.CheckTableRange(destRange);
            string displayText = destRange.DisplayText;
            destRange.Clear();
            int num8 = 1;
            string numberFormat = destRange.NumberFormat;
            int num9 = iDestColumn - listObject.Location.Column;
            Match match = Match.Empty;
            if (num9 != 0)
              match = Regex.Match(listObject.Columns[num9 - 1].Name, "[\\d]+$");
            if (displayText == string.Empty || num9 == 0 || !match.Success)
            {
              string str = "Column1";
              num8 = int.Parse(str[str.Length - 1].ToString(), NumberStyles.Number);
              for (int index4 = 0; index4 < listObject.Columns.Count; ++index4)
              {
                if (destSheet[listObject.Location.Row, listObject.Location.Column + index4].DisplayText == str)
                  ++num8;
                str = "Column" + num8.ToString();
              }
              destRange.Value = str;
              destRange.NumberFormat = numberFormat;
            }
            else
            {
              num8 = int.Parse(match.Value);
              string str1 = listObject.Columns[num9 - 1].Name;
              string str2 = str1;
              for (int index5 = 0; index5 < listObject.Columns.Count; ++index5)
              {
                if (destSheet[listObject.Location.Row, listObject.Location.Column + index5].DisplayText == str1)
                {
                  ++num8;
                  if (num8 > 1)
                    str1 = str2.Substring(0, match.Index) + num8.ToString();
                }
              }
              destRange.Value = str1;
              destRange.NumberFormat = numberFormat;
            }
          }
          else if (!skipBlank)
            cellRecords.Remove(cellIndex1);
        }
      }
    }
    if (!flag1 || options != ExcelCopyRangeOptions.All)
      return;
    int count1 = this.ListObjects.Count;
    int count2 = destSheet.ListObjects.Count;
    IRange range1_1 = destSheet[iDestRow, iDestColumn, iDestRow + iRowCount - 1, iDestColumn + iColumnCount - 1];
    IRange range3 = this[iSourceRow, iSourceColumn, iSourceRow + iRowCount - 1, iSourceColumn + iColumnCount - 1];
    Dictionary<IRange, bool> dictionary2 = new Dictionary<IRange, bool>();
    if (count2 != 0 && count1 != 0)
    {
      for (int index6 = 0; index6 < destSheet.ListObjects.Count; ++index6)
      {
        IListObject listObject = destSheet.ListObjects[index6];
        int num10 = index6;
        IRange location = listObject.Location;
        dictionary2.Add(location, true);
        if (this.IntersectRanges(range1_1, location) != null)
        {
          if ((this.CheckOverLab(destSheet[iDestRow, iDestColumn]) || range1_1.Row <= location.Row || range1_1.Column >= location.Column) && range1_1.Row <= location.Row && range1_1.Column <= location.Column && range1_1.LastRow >= location.LastRow && range1_1.LastColumn >= location.LastColumn)
          {
            string name = listObject.Name;
            destSheet.ListObjects.Remove(listObject);
            destSheet.Workbook.Names.Remove(name);
            --index6;
            flag2 = false;
          }
          else
            dictionary2[location] = false;
          if (num10 == index6)
          {
            IRange range4 = range1_1.IntersectWith(range1_1.Worksheet[listObject.Location.Row, listObject.Location.Column, listObject.Location.Row, listObject.Location.LastColumn]);
            IRange range5 = range1_1.IntersectWith(range1_1.Worksheet[listObject.Location.LastRow, listObject.Location.Column, listObject.Location.LastRow, listObject.Location.LastColumn]);
            if (listObject.ShowHeaderRow && range4 != null || listObject.ShowTotals && range5 != null)
            {
              for (int index7 = 0; index7 < listObject.Columns.Count; ++index7)
              {
                if (listObject.ShowHeaderRow && range4 != null && range4.IntersectWith((listObject.Columns[index7] as ListObjectColumn).HeaderCell) != null)
                  (listObject.Columns[index7] as ListObjectColumn).SetName((listObject.Columns[index7] as ListObjectColumn).HeaderCell.Value);
                if (listObject.ShowTotals && range5 != null && range1_1.Worksheet[range3.AddressLocal].IntersectWith(range1_1.Worksheet[listObject.Location.LastRow, listObject.Location.Column, listObject.Location.LastRow, listObject.Location.LastColumn]) != null && range5.IntersectWith((listObject.Columns[index7] as ListObjectColumn).TotalCell) != null)
                  (listObject.Columns[index7] as ListObjectColumn).SetTotalsCalculation(ExcelTotalsCalculation.Custom);
              }
            }
          }
        }
      }
    }
    if (count1 == 0)
      return;
    Dictionary<string, string> hashTableNames = new Dictionary<string, string>();
    Dictionary<int, string> hashTableNameRanges = new Dictionary<int, string>();
    WorkbookImpl workbook1 = this.Workbook as WorkbookImpl;
    WorkbookImpl workbook2 = destSheet.Workbook as WorkbookImpl;
    for (int index = 0; index < count1; ++index)
    {
      IListObject listObject1 = this.ListObjects[index];
      IRange location = listObject1.Location;
      if (iSourceRow <= location.Row && iSourceColumn <= location.Column && iSourceRow + (iRowCount - 1) >= location.LastRow && iSourceColumn + (iColumnCount - 1) >= location.LastColumn)
      {
        int row = location.Row - iSourceRow + iDestRow;
        int column = location.Column - iSourceColumn + iDestColumn;
        int lastRow2 = iDestRow + (iRowCount - 1) - (iSourceRow + (iRowCount - 1) - location.LastRow);
        int lastColumn2 = iDestColumn + (iColumnCount - 1) - (iSourceColumn + (iColumnCount - 1) - location.LastColumn);
        IRange range6 = destSheet[row, column, lastRow2, lastColumn2];
        bool flag4 = true;
        foreach (KeyValuePair<IRange, bool> keyValuePair in dictionary2)
        {
          if (keyValuePair.Key.IntersectWith(range6) != null)
          {
            flag4 = keyValuePair.Value;
            break;
          }
        }
        if (flag4)
        {
          string str;
          string stringPart;
          int numberPart;
          for (str = "XlsIOTable_" + destSheet.ListObjects.Count.ToString(); (destSheet.ListObjects as ListObjectCollection)[str] != null; str = stringPart + (object) numberPart)
          {
            (listObject1 as ListObject).SplitName(str, out stringPart, out numberPart);
            ++numberPart;
          }
          IListObject listObject2 = destSheet.ListObjects.Create(str, range6);
          hashTableNames.Add(listObject1.Name, listObject2.Name);
        }
      }
    }
    foreach (KeyValuePair<string, string> keyValuePair1 in hashTableNames)
    {
      IListObject listObject3 = (this.ListObjects as ListObjectCollection)[keyValuePair1.Key];
      IListObject listObject4 = (destSheet.ListObjects as ListObjectCollection)[keyValuePair1.Value];
      IRange location = listObject4.Location;
      int row = location.Row;
      int column = location.Column;
      int lastRow3 = location.LastRow;
      int lastColumn3 = location.LastColumn;
      for (int index = 0; index < listObject3.Columns.Count; ++index)
      {
        if (listObject3.Columns[index].CalculatedFormula != null)
        {
          string str = listObject3.Columns[index].CalculatedFormula;
          foreach (KeyValuePair<string, string> keyValuePair2 in hashTableNames)
          {
            if (str.Contains(keyValuePair2.Key))
            {
              str = str.Replace(keyValuePair2.Key, keyValuePair2.Value);
              (listObject4.Columns[index] as ListObjectColumn).SetCalculatedFormula(str);
              break;
            }
          }
          if (workbook1.FormulaUtil.HasCellReference(str))
          {
            bool flag5 = (options & ExcelCopyRangeOptions.UpdateFormulas) != ExcelCopyRangeOptions.None;
            int num11 = flag5 ? listObject3.Location.Columns[index].Row - listObject4.Location.Columns[index].Row : 0;
            int num12 = flag5 ? listObject3.Location.Columns[index].Column - listObject4.Location.Columns[index].Column : 0;
            (listObject4.Columns[index] as ListObjectColumn).SetCalculatedFormula(str);
            Ptg[] ptgArray = this.UpdateFormula((listObject4.Columns[index] as ListObjectColumn).CalculatedFormulaPtgs, ~num11 + 1, ~num12 + 1);
            (listObject4.Columns[index] as ListObjectColumn).CalculatedFormulaPtgs = ptgArray;
          }
          else
            (listObject4.Columns[index] as ListObjectColumn).SetCalculatedFormula(str);
          if (workbook1 != workbook2)
          {
            Ptg[] calculatedFormulaPtgs = (listObject4.Columns[index] as ListObjectColumn).CalculatedFormulaPtgs;
            if (workbook2.FullFileName != null && str.Contains(workbook2.FullFileName))
              this.ChangeReferenceIndex(workbook1, workbook2, listObject4.Worksheet, calculatedFormulaPtgs);
            Ptg[] ptgArray = this.ChangeTablePtgArray(workbook1, workbook2, calculatedFormulaPtgs, hashTableNames, hashTableNameRanges);
            (listObject4.Columns[index] as ListObjectColumn).CalculatedFormulaPtgs = ptgArray;
          }
        }
        if (listObject3.Columns[index].TotalsRowLabel != null)
          (listObject4.Columns[index] as ListObjectColumn).SetTotalsLabel(listObject3.Columns[index].TotalsRowLabel);
        if (listObject3.Columns[index].TotalsCalculation != ExcelTotalsCalculation.None)
          (listObject4.Columns[index] as ListObjectColumn).SetTotalsCalculation(listObject3.Columns[index].TotalsCalculation);
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(destSheet.Application, (IWorksheet) destSheet);
        for (int iRow = row; iRow <= lastRow3; ++iRow)
        {
          migrantRangeImpl.ResetRowColumn(iRow, column + index);
          if (migrantRangeImpl.Record is FormulaRecord record)
          {
            if (workbook1 != workbook2)
            {
              Ptg[] parsedExpression = record.ParsedExpression;
              Ptg[] ptgArray = this.ChangeTablePtgArray(workbook1, workbook2, parsedExpression, hashTableNames, hashTableNameRanges);
              record.ParsedExpression = ptgArray;
              migrantRangeImpl.Record = (BiffRecordRaw) record;
            }
            foreach (KeyValuePair<string, string> keyValuePair3 in hashTableNames)
            {
              if (migrantRangeImpl.Formula.Contains(keyValuePair3.Key))
              {
                migrantRangeImpl.Formula = migrantRangeImpl.Formula.Replace(keyValuePair3.Key, keyValuePair3.Value);
                break;
              }
            }
          }
        }
      }
      (listObject4 as ListObject).TotalsRowCount = listObject3.TotalsRowCount;
      (listObject4 as ListObject).TotalsRowShown = (listObject3 as ListObject).TotalsRowShown;
      listObject4.BuiltInTableStyle = listObject3.BuiltInTableStyle;
    }
    foreach (KeyValuePair<int, string> keyValuePair in hashTableNameRanges)
    {
      NameImpl nameByIndex = workbook2.InnerNamesColection.GetNameByIndex(keyValuePair.Key) as NameImpl;
      string name = nameByIndex.Name.Substring(0, nameByIndex.Name.IndexOf('['));
      if (!(this.ListObjects[0] as ListObject).ListObjectNameExist(workbook2, name))
      {
        if (workbook2.InnerNamesColection[keyValuePair.Value] != null)
          workbook2.InnerNamesColection[keyValuePair.Value].Delete();
        workbook2.InnerNamesColection.GetNameByIndex(keyValuePair.Key).Name = keyValuePair.Value;
      }
    }
  }

  public void RemoveArrayFormulas(ICollection<ArrayRecord> colRemove, bool bClearRange)
  {
    if (colRemove == null)
      throw new ArgumentNullException(nameof (colRemove));
    this.ParseData();
    foreach (ArrayRecord record in (IEnumerable<ArrayRecord>) colRemove)
      this.RemoveArrayFormula(record, bClearRange);
  }

  public Ptg[] UpdateFormula(Ptg[] arrFormula, int iRowOffset, int iColOffset)
  {
    if (arrFormula == null)
      throw new ArgumentNullException(nameof (arrFormula));
    this.ParseData();
    bool flag = iRowOffset != 0 || iColOffset != 0;
    Ptg[] ptgArray = new Ptg[arrFormula.Length];
    int index = 0;
    for (int length = arrFormula.Length; index < length; ++index)
      ptgArray[index] = !flag ? (Ptg) arrFormula[index].Clone() : arrFormula[index].Offset(iRowOffset, iColOffset, this.m_book);
    return ptgArray;
  }

  public override void UpdateFormula(
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect)
  {
    this.ParseData();
    this.m_dicRecordsCells.UpdateFormula(iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect);
    base.UpdateFormula(iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect);
    if (this.m_arrConditionalFormats == null)
      return;
    this.m_arrConditionalFormats.UpdateFormula(iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect);
  }

  public void AutofitRow(int rowIndex)
  {
    int column = this.UsedRange.Column;
    int lastColumn = this.UsedRange.LastColumn;
    this.AutofitRow(rowIndex, column, lastColumn, true);
  }

  public void AutofitColumn(int colIndex)
  {
    if (this.IsEmpty)
      return;
    (this[this.UsedRange.Row, this.UsedRange.Column, this.UsedRange.LastRow, this.UsedRange.LastColumn] as RangeImpl).AutoFitToColumn(colIndex, colIndex);
  }

  public void AutofitColumn(int colIndex, int firstRow, int lastRow)
  {
    (this[firstRow, colIndex, lastRow, colIndex] as RangeImpl).AutoFitToColumn(colIndex, colIndex);
  }

  public void CopyFrom(
    WorksheetImpl worksheet,
    Dictionary<string, string> hashStyleNames,
    Dictionary<string, string> hashWorksheetNames,
    Dictionary<int, int> dicFontIndexes,
    ExcelWorksheetCopyFlags flags)
  {
    Dictionary<int, int> hashExtFormatIndexes = new Dictionary<int, int>();
    Dictionary<int, int> hashNameIndexes = new Dictionary<int, int>();
    Dictionary<int, int> hashExternSheets = new Dictionary<int, int>();
    this.CopyFrom(worksheet, hashStyleNames, hashWorksheetNames, dicFontIndexes, flags, hashExtFormatIndexes, hashNameIndexes, hashExternSheets);
  }

  public void CopyFrom(
    WorksheetImpl worksheet,
    Dictionary<string, string> hashStyleNames,
    Dictionary<string, string> hashWorksheetNames,
    Dictionary<int, int> dicFontIndexes,
    ExcelWorksheetCopyFlags flags,
    Dictionary<int, int> hashExtFormatIndexes,
    Dictionary<int, int> hashNameIndexes)
  {
    this.CopyFrom(worksheet, hashStyleNames, hashWorksheetNames, dicFontIndexes, flags, hashExtFormatIndexes, hashNameIndexes, new Dictionary<int, int>(0));
  }

  public void CopyFrom(
    WorksheetImpl worksheet,
    Dictionary<string, string> hashStyleNames,
    Dictionary<string, string> hashWorksheetNames,
    Dictionary<int, int> dicFontIndexes,
    ExcelWorksheetCopyFlags flags,
    Dictionary<int, int> hashExtFormatIndexes,
    Dictionary<int, int> hashNameIndexes,
    Dictionary<int, int> hashExternSheets)
  {
    this.Parse();
    if (worksheet.ParseDataOnDemand || worksheet.ParseOnDemand)
    {
      if (worksheet.m_dataHolder == null && worksheet.ParseOnDemand && !worksheet.IsParsed && worksheet.Parent is WorksheetsCollection || worksheet.m_dataHolder != null && worksheet.ParseDataOnDemand)
        worksheet.ParseData((Dictionary<int, int>) null);
      this.CopyStyles((IWorksheet) worksheet, hashExtFormatIndexes);
      if (this.Parent is WorksheetsCollection)
      {
        foreach (WorksheetImpl worksheetImpl in (CollectionBase<IWorksheet>) (this.Parent as WorksheetsCollection))
        {
          if (worksheetImpl.m_dataHolder == null && worksheetImpl.ParseOnDemand && !worksheetImpl.IsParsed && worksheetImpl.Parent is WorksheetsCollection || worksheetImpl.m_dataHolder != null && worksheetImpl.ParseDataOnDemand)
            worksheetImpl.ParseData((Dictionary<int, int>) null);
        }
      }
    }
    if ((flags & ExcelWorksheetCopyFlags.ClearBefore) != ExcelWorksheetCopyFlags.None)
    {
      this.ClearAll(flags);
      flags &= ~ExcelWorksheetCopyFlags.ClearBefore;
    }
    if ((flags & ExcelWorksheetCopyFlags.CopyColumnHeight) != ExcelWorksheetCopyFlags.None)
    {
      this.CopyColumnWidth(worksheet, hashExtFormatIndexes);
      flags &= ~ExcelWorksheetCopyFlags.CopyColumnHeight;
    }
    if ((flags & ExcelWorksheetCopyFlags.CopyRowHeight) != ExcelWorksheetCopyFlags.None)
    {
      this.CopyRowHeight(worksheet, hashExtFormatIndexes);
      flags &= ~ExcelWorksheetCopyFlags.CopyRowHeight;
    }
    this.CustomHeight = worksheet.CustomHeight;
    if ((flags & ExcelWorksheetCopyFlags.CopyNames | ExcelWorksheetCopyFlags.CopyConditionlFormats) != ExcelWorksheetCopyFlags.None)
    {
      bool throwOnUnknownNames = worksheet.Workbook.ThrowOnUnknownNames;
      worksheet.Workbook.ThrowOnUnknownNames = false;
      worksheet.ParseSheetCF();
      worksheet.Workbook.ThrowOnUnknownNames = throwOnUnknownNames;
    }
    if ((flags & ExcelWorksheetCopyFlags.CopyNames) != ExcelWorksheetCopyFlags.None)
    {
      this.CopyNames(worksheet, hashWorksheetNames, hashNameIndexes, hashExternSheets);
      flags &= ~ExcelWorksheetCopyFlags.CopyNames;
    }
    if (worksheet.ParentWorkbook.HasMacros && this.ParentWorkbook.VbaProject != null && worksheet.VbaModule != null)
      this.VbaModule.Code = worksheet.VbaModule.Code;
    this.CopyFrom((WorksheetBaseImpl) worksheet, hashStyleNames, hashWorksheetNames, dicFontIndexes, flags, hashExtFormatIndexes);
    this.Visibility = worksheet.Visibility;
    if ((flags & ExcelWorksheetCopyFlags.CopyCells) != ExcelWorksheetCopyFlags.None)
    {
      this.m_iFirstRow = worksheet.m_iFirstRow;
      this.m_iLastRow = worksheet.m_iLastRow;
      this.m_iFirstColumn = worksheet.m_iFirstColumn;
      this.Zoom = worksheet.Zoom;
      this.m_iLastColumn = worksheet.m_iLastColumn;
      this.m_dicRecordsCells.CopyCells(worksheet.m_dicRecordsCells, hashStyleNames, hashWorksheetNames, hashExtFormatIndexes, hashNameIndexes, dicFontIndexes, hashExternSheets);
      this.CopyErrorIndicators(worksheet.m_errorIndicators);
      this.CopyHyperlinks(worksheet.m_hyperlinks);
    }
    if (this.InnerNames.Contains(PageSetupImpl.DEF_AREA_XlS) || this.InnerNames.Contains(PageSetupImpl.DEF_AREA_XlSX))
    {
      IName name = this.InnerNames[PageSetupImpl.DEF_AREA_XlS] ?? this.InnerNames[PageSetupImpl.DEF_AREA_XlSX];
      if (name != null && (name as NameImpl).Record != null)
      {
        foreach (Ptg formulaToken in (name as NameImpl).Record.FormulaTokens)
        {
          if (formulaToken.TokenCode == FormulaToken.tNameX1 || formulaToken.TokenCode == FormulaToken.tNameX2 || formulaToken.TokenCode == FormulaToken.tNameX3)
          {
            int num = (this.Workbook as WorkbookImpl).AddSheetReference((this.Workbook as WorkbookImpl).ExternWorkbooks.InsertSelfSupbook(), this.Index, this.Index);
            (formulaToken as NameXPtg).RefIndex = (ushort) num;
          }
        }
      }
    }
    if ((flags & ExcelWorksheetCopyFlags.CopyMerges) != ExcelWorksheetCopyFlags.None)
      this.CopyMerges(worksheet);
    if ((flags & ExcelWorksheetCopyFlags.CopyConditionlFormats) != ExcelWorksheetCopyFlags.None)
      this.CopyConditionalFormats(worksheet);
    if ((flags & ExcelWorksheetCopyFlags.CopyAutoFilters) != ExcelWorksheetCopyFlags.None)
      this.CopyAutoFilters(worksheet);
    if ((flags & ExcelWorksheetCopyFlags.CopyDataValidations) != ExcelWorksheetCopyFlags.None)
      this.CopyDataValidations(worksheet);
    if ((flags & ExcelWorksheetCopyFlags.CopyPageSetup) != ExcelWorksheetCopyFlags.None)
      this.CopyPageSetup(worksheet);
    if ((flags & ExcelWorksheetCopyFlags.CopyTables) != ExcelWorksheetCopyFlags.None)
      this.CopyTables(worksheet, hashWorksheetNames);
    if ((flags & ExcelWorksheetCopyFlags.CopyPivotTables) == ExcelWorksheetCopyFlags.None)
      return;
    this.CopyPivotTables(worksheet, hashWorksheetNames);
  }

  private void CopyPivotTables(
    WorksheetImpl worksheet,
    Dictionary<string, string> hashWorksheetNames)
  {
    if (worksheet == null)
      throw new ArgumentNullException(nameof (worksheet));
    if (worksheet.m_pivotTables == null || worksheet.m_pivotTables.Count == 0)
      return;
    this.m_pivotTables = worksheet.m_pivotTables.Clone(this, hashWorksheetNames);
  }

  private void CopyTables(WorksheetImpl worksheet, Dictionary<string, string> hashWorksheetNames)
  {
    if (worksheet == null)
      throw new ArgumentNullException(nameof (worksheet));
    if (worksheet.m_listObjects == null || worksheet.m_listObjects.Count == 0)
      return;
    this.m_listObjects = worksheet.m_listObjects.Clone(this, hashWorksheetNames);
    this.m_listObjects = this.UpdateCalculatedColumnAndTotalFormulas(this.m_listObjects, worksheet.m_listObjects);
  }

  private ListObjectCollection UpdateCalculatedColumnAndTotalFormulas(
    ListObjectCollection copiedListObjects,
    ListObjectCollection sourceListObjects)
  {
    if (copiedListObjects != null)
    {
      Dictionary<string, string> hashTableNames = new Dictionary<string, string>();
      Dictionary<int, string> hashTableNameRanges = new Dictionary<int, string>();
      for (int index = 0; index < copiedListObjects.Count; ++index)
        hashTableNames.Add(sourceListObjects[index].Name, copiedListObjects[index].Name);
      int count1 = copiedListObjects.Count;
      for (int index1 = 0; index1 < count1; ++index1)
      {
        IListObject copiedListObject = copiedListObjects[index1];
        WorkbookImpl workbook1 = sourceListObjects[index1].Worksheet.Workbook as WorkbookImpl;
        WorkbookImpl workbook2 = copiedListObject.Worksheet.Workbook as WorkbookImpl;
        IRange location = copiedListObject.Location;
        int row = location.Row;
        int column = location.Column;
        int lastRow = location.LastRow;
        int lastColumn = location.LastColumn;
        int count2 = copiedListObject.Columns.Count;
        for (int index2 = 0; index2 < count2; ++index2)
        {
          if (sourceListObjects[index1].Columns[index2].CalculatedFormula != null)
          {
            string calculatedFormula = sourceListObjects[index1].Columns[index2].CalculatedFormula;
            foreach (KeyValuePair<string, string> keyValuePair in hashTableNames)
            {
              if (calculatedFormula.Contains(keyValuePair.Key))
              {
                calculatedFormula = calculatedFormula.Replace(keyValuePair.Key, keyValuePair.Value);
                (copiedListObject.Columns[index2] as ListObjectColumn).SetCalculatedFormula(calculatedFormula);
                break;
              }
            }
            if (workbook1 != workbook2)
            {
              Ptg[] calculatedFormulaPtgs = (copiedListObject.Columns[index2] as ListObjectColumn).CalculatedFormulaPtgs;
              if (workbook2.FullFileName != null && calculatedFormula.Contains(workbook2.FullFileName))
                this.ChangeReferenceIndex(workbook1, workbook2, copiedListObject.Worksheet, calculatedFormulaPtgs);
              Ptg[] ptgArray = this.ChangeTablePtgArray(workbook1, workbook2, calculatedFormulaPtgs, hashTableNames, hashTableNameRanges);
              (copiedListObject.Columns[index2] as ListObjectColumn).CalculatedFormulaPtgs = ptgArray;
            }
          }
          if (sourceListObjects[index1].Columns[index2].TotalsRowLabel != null)
            (copiedListObject.Columns[index2] as ListObjectColumn).SetTotalsLabel(sourceListObjects[index1].Columns[index2].TotalsRowLabel);
          if (sourceListObjects[index1].Columns[index2].TotalsCalculation != ExcelTotalsCalculation.None)
            (copiedListObject.Columns[index2] as ListObjectColumn).SetTotalsCalculation(sourceListObjects[index1].Columns[index2].TotalsCalculation);
          MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(copiedListObject.Worksheet.Application, copiedListObject.Worksheet);
          for (int iRow = row; iRow <= lastRow; ++iRow)
          {
            migrantRangeImpl.ResetRowColumn(iRow, column + index2);
            if (migrantRangeImpl.Record is FormulaRecord record)
            {
              if (workbook1 != workbook2)
              {
                Ptg[] parsedExpression = record.ParsedExpression;
                Ptg[] ptgArray = this.ChangeTablePtgArray(workbook1, workbook2, parsedExpression, hashTableNames, hashTableNameRanges);
                record.ParsedExpression = ptgArray;
                migrantRangeImpl.Record = (BiffRecordRaw) record;
              }
              foreach (KeyValuePair<string, string> keyValuePair in hashTableNames)
              {
                if (migrantRangeImpl.Formula.Contains(keyValuePair.Key))
                {
                  migrantRangeImpl.Formula = migrantRangeImpl.Formula.Replace(keyValuePair.Key, keyValuePair.Value);
                  break;
                }
              }
            }
          }
        }
      }
      foreach (KeyValuePair<int, string> keyValuePair in hashTableNameRanges)
      {
        WorkbookImpl workbook = this.Workbook as WorkbookImpl;
        NameImpl nameByIndex = workbook.InnerNamesColection.GetNameByIndex(keyValuePair.Key) as NameImpl;
        if ((this.ListObjects as ListObjectCollection)[nameByIndex.Name.Substring(0, nameByIndex.Name.IndexOf('['))] == null)
        {
          if (workbook.InnerNamesColection[keyValuePair.Value] != null)
            workbook.InnerNamesColection[keyValuePair.Value].Delete();
          workbook.InnerNamesColection.GetNameByIndex(keyValuePair.Key).Name = keyValuePair.Value;
        }
      }
      if (hashTableNames != null && hashTableNames.Count > 0)
        this.UpdateTableFormulaInCells(hashTableNames);
    }
    return copiedListObjects;
  }

  internal void UpdateTableFormulaInCells(Dictionary<string, string> hashTableNames)
  {
    IMigrantRange migrantRange = this.MigrantRange;
    IRange usedRange = this.UsedRange;
    for (int column = usedRange.Column; column <= usedRange.LastColumn; ++column)
    {
      for (int row = usedRange.Row; row <= usedRange.LastRow; ++row)
      {
        migrantRange.ResetRowColumn(row, column);
        RangeImpl destRange = migrantRange as RangeImpl;
        if (this.IsTableRange((IRange) destRange) == null && destRange.Record is FormulaRecord)
        {
          foreach (KeyValuePair<string, string> hashTableName in hashTableNames)
          {
            if (migrantRange.Formula.Contains(hashTableName.Key))
            {
              migrantRange.Formula = migrantRange.Formula.Replace(hashTableName.Key, hashTableName.Value);
              break;
            }
          }
        }
      }
    }
  }

  private Ptg[] ChangeTablePtgArray(
    WorkbookImpl sourceBook,
    WorkbookImpl destBook,
    Ptg[] arrPtg,
    Dictionary<string, string> hashTableNames,
    Dictionary<int, string> hashTableNameRanges)
  {
    for (int index1 = 0; index1 < arrPtg.Length; ++index1)
    {
      Ptg ptg1 = arrPtg[index1];
      switch (ptg1)
      {
        case NamePtg _:
          if (destBook.InnerNamesColection.GetNameByIndex((ptg1 as NamePtg).ExternNameIndexInt - 1) is NameImpl nameByIndex1 && (nameByIndex1.m_isTableNamedRange || nameByIndex1.Name.Contains("[") && nameByIndex1.Name.Contains("]")))
          {
            string str = nameByIndex1.Name.Substring(0, nameByIndex1.Name.IndexOf('['));
            if (!hashTableNames.ContainsKey(str) && !hashTableNames.ContainsValue(str))
            {
              arrPtg[index1] = this.ChangeTablePtg(sourceBook, destBook, nameByIndex1, ptg1);
              break;
            }
            if (!hashTableNames.ContainsKey(str) && !hashTableNames.ContainsValue(str))
            {
              using (Dictionary<string, string>.Enumerator enumerator = hashTableNames.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  KeyValuePair<string, string> current = enumerator.Current;
                  if (nameByIndex1.Name.Contains(current.Key) && !hashTableNameRanges.ContainsKey(nameByIndex1.Index))
                  {
                    hashTableNameRanges.Add(nameByIndex1.Index, nameByIndex1.Name.Replace(current.Key, current.Value));
                    break;
                  }
                }
                break;
              }
            }
            if (hashTableNames.ContainsKey(str))
            {
              if (!hashTableNameRanges.ContainsKey(nameByIndex1.Index))
                hashTableNameRanges.Add(nameByIndex1.Index, nameByIndex1.Name.Replace(str, hashTableNames[str]));
              if (destBook.InnerNamesColection[nameByIndex1.Name.Replace(str, hashTableNames[str])] != null)
              {
                (ptg1 as NamePtg).ExternNameIndexInt = (destBook.InnerNamesColection[nameByIndex1.Name.Replace(str, hashTableNames[str])] as NameImpl).Index + 1;
                break;
              }
              break;
            }
            break;
          }
          break;
        case NameXPtg _:
          if (!destBook.IsExternalReference((int) (ptg1 as NameXPtg).RefIndex) && destBook.InnerNamesColection.GetNameByIndex((int) (ptg1 as NameXPtg).NameIndex - 1) is NameImpl nameByIndex2 && (nameByIndex2.m_isTableNamedRange || nameByIndex2.Name.Contains("[") && nameByIndex2.Name.Contains("]")))
          {
            int index2 = nameByIndex2.Index;
            Ptg ptg2 = FormulaUtil.CreatePtg(NamePtg.IndexToCode(FormulaUtil.GetIndex(typeof (NamePtg), 0, (Dictionary<System.Type, ReferenceIndexAttribute>) null, 0, ExcelParseFormulaOptions.RootLevel | ExcelParseFormulaOptions.InArray)), (object) index2);
            (ptg2 as NamePtg).ExternNameIndexInt = index2 + 1;
            arrPtg[index1] = ptg2;
            break;
          }
          break;
      }
    }
    return arrPtg;
  }

  private Ptg ChangeTablePtg(
    WorkbookImpl sourceBook,
    WorkbookImpl destBook,
    NameImpl name,
    Ptg ptg)
  {
    FormulaToken code = NameXPtg.IndexToCode(FormulaUtil.GetIndex(typeof (NameXPtg), 0, (Dictionary<System.Type, ReferenceIndexAttribute>) null, 0, ExcelParseFormulaOptions.RootLevel | ExcelParseFormulaOptions.InArray));
    int bookIndex = sourceBook.ExternWorkbooks.InsertSelfSupbook();
    ExternWorkbookImpl externBook = CellRecordCollection.GetExternBook(sourceBook, destBook, bookIndex);
    int index1 = externBook.Index;
    int num = destBook.ExternSheet.AddReference(index1, 65534, 65534);
    int index2 = name.Index;
    ptg = FormulaUtil.CreatePtg(code, (object) num, (object) index2);
    int index3;
    if (!externBook.ExternNames.Contains(name.Name))
    {
      index3 = externBook.ExternNames.Add(name.Name);
      externBook.ExternNames[index3].RefersTo = name.RefersTo;
      externBook.ExternNames[index3].sheetId = -1;
    }
    else
      index3 = externBook.ExternNames.GetNameIndex(name.Name);
    (ptg as NameXPtg).NameIndex = (ushort) (index3 + 1);
    return ptg;
  }

  private void ChangeReferenceIndex(
    WorkbookImpl sourceBook,
    WorkbookImpl destBook,
    IWorksheet destSheet,
    Ptg[] formulaPtg)
  {
    for (int index = 0; index < formulaPtg.Length; ++index)
    {
      Ptg ptg = formulaPtg[index];
      if (ptg is ISheetReference)
      {
        ISheetReference sheetReference = (ISheetReference) ptg;
        int num = (int) sheetReference.RefIndex;
        int firstSheet = (int) destBook.ExternSheet.RefList[num].FirstSheet;
        ExternWorkbookImpl externWorkbook = destBook.ExternWorkbooks[destBook.GetBookIndex(num)];
        if (externWorkbook.URL != null)
        {
          string str = destBook.IsCreated ? destBook.GetWorkbookName(destBook) : destBook.FullFileName;
          if (str == externWorkbook.URL || sourceBook.FullFileName != null && str == sourceBook.GetFilePath(sourceBook.FullFileName) + externWorkbook.URL)
          {
            string sheetName = externWorkbook.GetSheetName(firstSheet);
            if (sheetName != null && sheetName != string.Empty)
              num = destBook.AddSheetReference(sheetName);
            else if (firstSheet == 65534)
            {
              int supIndex = destBook.ExternWorkbooks.InsertSelfSupbook();
              num = destBook.ExternSheet.AddReference(supIndex, firstSheet, firstSheet);
            }
            if (ptg is NameXPtg nameXptg)
            {
              int nameIndex = CellRecordCollection.GetNameIndex(externWorkbook.ExternNames[(int) nameXptg.NameIndex - 1].Name, destBook, destBook.Worksheets[sheetName] as WorksheetImpl);
              if (nameIndex != -1)
                nameXptg.NameIndex = (ushort) (nameIndex + 1);
            }
          }
          sheetReference.RefIndex = (ushort) num;
        }
      }
    }
  }

  private void CopyErrorIndicators(ErrorIndicatorsCollection sourceErrors)
  {
    if (sourceErrors == null)
      throw new ArgumentNullException(nameof (sourceErrors));
    this.ParseData();
    if (sourceErrors.Count <= 0)
      return;
    this.m_errorIndicators = (ErrorIndicatorsCollection) sourceErrors.Clone((object) this);
  }

  private void CopyHyperlinks(HyperLinksCollection source)
  {
    this.ParseData();
    this.m_hyperlinks = (HyperLinksCollection) CloneUtils.CloneCloneable((ICloneParent) source, (object) this);
  }

  public bool CanMove(ref IRange destination, IRange source)
  {
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    this.ParseData();
    RangeImpl rangeImpl1 = (RangeImpl) destination;
    RangeImpl rangeImpl2 = (RangeImpl) source;
    int lastRow = rangeImpl1.FirstRow + rangeImpl2.LastRow - rangeImpl2.FirstRow;
    int lastColumn = rangeImpl1.FirstColumn + rangeImpl2.LastColumn - rangeImpl2.FirstColumn;
    RangeImpl rangeImpl3;
    destination = (IRange) (rangeImpl3 = (RangeImpl) rangeImpl1.InnerWorksheet.Range[rangeImpl1.Row, rangeImpl1.Column, lastRow, lastColumn]);
    if (rangeImpl3 == rangeImpl2)
      return true;
    Dictionary<ArrayRecord, object> hashToSkip = new Dictionary<ArrayRecord, object>();
    bool formulasNotSeparated = rangeImpl2.GetAreArrayFormulasNotSeparated(hashToSkip);
    if (formulasNotSeparated)
    {
      if (rangeImpl3.Worksheet != rangeImpl2.Worksheet)
        hashToSkip.Clear();
      formulasNotSeparated = rangeImpl3.GetAreArrayFormulasNotSeparated(hashToSkip);
    }
    return formulasNotSeparated;
  }

  public bool CanInsertRow(int iRowIndex, int iRowCount, ExcelInsertOptions options)
  {
    this.ParseData();
    if (iRowIndex < 1 || iRowIndex > this.m_book.MaxRowCount || iRowCount <= 0)
      return false;
    if (this.m_iLastRow <= iRowIndex)
      return true;
    if (iRowIndex >= this.m_iFirstRow && this.m_iLastColumn <= this.m_book.MaxColumnCount && !((RangeImpl) this.Range[iRowIndex, this.m_iFirstColumn, this.m_iLastRow, this.m_iLastColumn]).AreFormulaArraysNotSeparated)
      return false;
    int num1 = this.m_iLastRow + iRowCount - this.m_book.MaxRowCount;
    if (num1 > 0)
    {
      int num2 = Math.Max(this.m_iLastRow - num1, this.m_iFirstRow);
      for (int iLastRow = this.m_iLastRow; iLastRow >= num2; --iLastRow)
      {
        if (!this.IsRowEmpty(iLastRow))
          return false;
        --this.m_iLastRow;
      }
      if (this.m_iFirstRow > this.m_iLastRow)
      {
        this.m_iLastRow = this.m_iFirstRow = -1;
        this.m_iLastColumn = this.m_iFirstColumn = int.MaxValue;
      }
    }
    return true;
  }

  public bool CanInsertColumn(int iColumnIndex, int iColumnCount, ExcelInsertOptions options)
  {
    this.ParseData();
    if (iColumnIndex < 1 || iColumnIndex > this.m_book.MaxColumnCount)
      return false;
    if (this.m_iLastColumn < iColumnIndex || this.m_iFirstColumn == int.MaxValue)
      return true;
    if (iColumnIndex >= this.m_iFirstColumn && !((RangeImpl) this.Range[this.m_iFirstRow, iColumnIndex, this.m_iLastRow, this.m_iLastColumn]).AreFormulaArraysNotSeparated)
      return false;
    int num1 = this.m_iLastColumn + iColumnCount - this.m_book.MaxColumnCount;
    if (num1 > 0)
    {
      int num2 = Math.Max(this.m_iLastColumn - num1, this.m_iFirstColumn);
      for (int iLastColumn = this.m_iLastColumn; iLastColumn >= num2; --iLastColumn)
      {
        if (!this.IsColumnEmpty(iLastColumn))
          return false;
        --this.m_iLastColumn;
      }
      if (this.m_iFirstColumn > this.m_iLastColumn)
      {
        this.m_iLastRow = this.m_iFirstRow = -1;
        this.m_iLastColumn = this.m_iFirstColumn = int.MaxValue;
      }
    }
    return true;
  }

  public IRange GetRangeByString(string strRangeValue, bool hasFormula)
  {
    if (strRangeValue == null || strRangeValue.Length == 0)
      return (IRange) null;
    FormulaUtil formulaUtil = this.m_book.Loading ? new FormulaUtil(this.Application, (object) this.m_book, NumberFormatInfo.InvariantInfo, this.Application.ArgumentsSeparator, this.Application.RowSeparator) : this.m_book.FormulaUtil;
    Ptg[] arrPtgs = formulaUtil.ParseString(strRangeValue);
    Stack<object> objectStack = (Stack<object>) null;
    if (!arrPtgs[arrPtgs.Length - 1].IsOperation)
      objectStack = this.m_book.GetStackOfRange(arrPtgs, this);
    if (hasFormula && (objectStack == null ? 1 : (objectStack.Count != 1 ? 1 : 0)) != 0)
    {
      string str1;
      string str2;
      string strSheetName;
      string strRow2;
      string strColumn2;
      if (FormulaUtil.IsCell(strRangeValue, false, out str1, out str2) || FormulaUtil.IsCell3D(strRangeValue, false, out strSheetName, out str1, out str2) || formulaUtil.IsCellRange(strRangeValue, false, out str1, out str2, out strRow2, out strColumn2) || formulaUtil.IsCellRange3D(strRangeValue, false, out strSheetName, out str1, out str2, out strRow2, out strColumn2))
      {
        if (objectStack == null)
          objectStack = this.m_book.GetStackOfRange(arrPtgs, this);
      }
      else
      {
        bool flag = this.CalcEngine != null;
        try
        {
          if (!flag)
            this.EnableSheetCalculations();
          if (this.m_book.Loading || this.m_book.Saving)
          {
            if (this.m_book.Loading)
            {
              if (!this.CalcEngine.UseFormulaValues)
                this.CalcEngine.UseFormulaValues = true;
            }
            else if (this.m_book.Saving)
              this.m_book.CalcEngineMemberValuesOnSheet(false);
            if (!flag)
              this.m_book.CalcEngineEnabledOnReadWrite = true;
          }
          strRangeValue = this.GetOverloadedOffsetFunction(formulaUtil, arrPtgs, strRangeValue);
          this.CalcEngine.IsAreaCalculation = true;
          string str3 = this.AddSheetName(this.CalcEngine.ParseAndComputeFormula(strRangeValue));
          objectStack = !this.IsErrorStringsContains(str3, this.CalcEngine.ErrorStrings, this.CalcEngine.FormulaErrorStrings) ? this.m_book.GetStackOfRange(formulaUtil.ParseString(str3), this) : (Stack<object>) null;
          this.CalcEngine.IsAreaCalculation = false;
          if (!flag)
          {
            if (!this.m_book.CalcEngineEnabledOnReadWrite)
              this.DisableSheetCalculations();
          }
        }
        catch (Exception ex)
        {
          if (!flag && !this.m_book.CalcEngineEnabledOnReadWrite)
            this.DisableSheetCalculations();
          return (IRange) null;
        }
      }
    }
    IRanges rangeByString = (IRanges) null;
    if (objectStack != null && objectStack.Count > 0)
    {
      List<IRange> rangeList = (List<IRange>) objectStack.Pop();
      int count = rangeList.Count;
      if (count == 1)
        return rangeList[0];
      if (rangeList[0] != null)
        rangeByString = rangeList[0].Worksheet.CreateRangesCollection();
      for (int index = 0; index < count; ++index)
      {
        if (rangeList[index] == null)
          return (IRange) null;
        rangeByString.Add(rangeList[index]);
      }
    }
    return (IRange) rangeByString;
  }

  private bool IsErrorStringsContains(
    string computedValue,
    ArrayList errorStrings,
    string[] formulaErrorStrings)
  {
    if (errorStrings.Contains((object) computedValue))
      return true;
    for (int index = 0; index < formulaErrorStrings.Length; ++index)
    {
      if (computedValue.Equals(formulaErrorStrings[index]))
        return true;
    }
    return false;
  }

  private string GetOverloadedOffsetFunction(
    FormulaUtil formulaUtil,
    Ptg[] arrPtgs,
    string formula)
  {
    string overloadedOffsetFunction = formula;
    if (arrPtgs != null && arrPtgs.Length > 0)
    {
      List<Ptg> ptgList = new List<Ptg>((IEnumerable<Ptg>) arrPtgs);
      int count = ptgList.Count;
      for (int index = 0; index < count; ++index)
      {
        if (ptgList[index].IsOperation && ptgList[index] is FunctionPtg functionPtg && functionPtg.FunctionIndex == ExcelFunction.OFFSET && functionPtg.NumberOfArguments == (byte) 4)
        {
          functionPtg.NumberOfArguments = (byte) 5;
          ptgList.Insert(index, (Ptg) new IntegerPtg((ushort) 1));
          ++count;
          ++index;
        }
      }
      if (arrPtgs.Length != ptgList.Count)
        overloadedOffsetFunction = formulaUtil.ParsePtgArray(ptgList.ToArray());
      ptgList.Clear();
    }
    return overloadedOffsetFunction;
  }

  public void UpdateNamedRangeIndexes(int[] arrNewIndex)
  {
    if (arrNewIndex == null)
      throw new ArgumentNullException(nameof (arrNewIndex));
    this.ParseData();
    this.m_dicRecordsCells.UpdateNameIndexes(this.m_book, arrNewIndex);
    if (this.m_dataValidation != null)
      this.m_dataValidation.UpdateNamedRangeIndexes(arrNewIndex);
    this.InnerShapes?.UpdateNamedRangeIndexes(arrNewIndex);
  }

  public void UpdateNamedRangeIndexes(IDictionary<int, int> dicNewIndex)
  {
    if (dicNewIndex == null)
      throw new ArgumentNullException(nameof (dicNewIndex));
    this.ParseData();
    if (this.m_dicRecordsCells != null)
      this.m_dicRecordsCells.UpdateNameIndexes(this.m_book, dicNewIndex);
    if (this.m_dataValidation != null)
      this.m_dataValidation.UpdateNamedRangeIndexes(dicNewIndex);
    this.InnerShapes?.UpdateNamedRangeIndexes(dicNewIndex);
  }

  public int GetStringIndex(long cellIndex)
  {
    this.ParseData();
    return !(this.m_dicRecordsCells.GetCellRecord(cellIndex) is LabelSSTRecord cellRecord) ? -1 : cellRecord.SSTIndex;
  }

  public TextWithFormat GetTextWithFormat(long cellIndex)
  {
    this.ParseData();
    ICellPositionFormat cellRecord1 = this.m_dicRecordsCells.GetCellRecord(cellIndex);
    if (cellRecord1 is LabelRecord)
    {
      LabelRecord labelRecord = cellRecord1 as LabelRecord;
      this.SetString(labelRecord.Row + 1, labelRecord.Column + 1, labelRecord.Label);
    }
    else if (cellRecord1 is FormulaRecord formulaRecord && formulaRecord.HasString)
      return new TextWithFormat()
      {
        Text = this.GetFormulaStringValue(formulaRecord.Row + 1, formulaRecord.Column + 1)
      };
    return !(this.m_dicRecordsCells.GetCellRecord(cellIndex) is LabelSSTRecord cellRecord2) ? (TextWithFormat) null : this.m_book.InnerSST[cellRecord2.SSTIndex];
  }

  public object GetTextObject(long cellIndex)
  {
    this.ParseData();
    return !(this.m_dicRecordsCells.GetCellRecord(cellIndex) is LabelSSTRecord cellRecord) ? (object) null : (object) this.m_book.InnerSST[cellRecord.SSTIndex];
  }

  public ExtendedFormatImpl GetExtendedFormat(long cellIndex)
  {
    this.ParseData();
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    if (!this.m_dicRecordsCells.Contains(cellIndex))
      return (ExtendedFormatImpl) null;
    int extendedFormatIndex = (int) this.m_dicRecordsCells.GetCellRecord(cellIndex).ExtendedFormatIndex;
    return innerExtFormats[extendedFormatIndex];
  }

  public void SetLabelSSTIndex(long cellIndex, int iSSTIndex)
  {
    this.ParseData();
    int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(cellIndex);
    int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(cellIndex);
    ICellPositionFormat cell = this.m_dicRecordsCells.GetCellRecord(rowFromCellIndex, columnFromCellIndex);
    if (iSSTIndex == -1)
    {
      if (cell != null && cell.TypeCode == TBIFFRecord.Blank)
        return;
      this.m_dicRecordsCells.SetCellRecord(rowFromCellIndex, columnFromCellIndex, (ICellPositionFormat) this.GetRecord(TBIFFRecord.Blank, rowFromCellIndex, columnFromCellIndex));
    }
    else
    {
      if (iSSTIndex < 0 || iSSTIndex >= this.m_book.InnerSST.Count)
        throw new ArgumentOutOfRangeException(nameof (iSSTIndex));
      if (cell == null || cell.TypeCode != TBIFFRecord.LabelSST)
        cell = (ICellPositionFormat) this.GetRecord(TBIFFRecord.LabelSST, rowFromCellIndex, columnFromCellIndex);
      ((LabelSSTRecord) cell).SSTIndex = iSSTIndex;
      if (rowFromCellIndex == 0 && columnFromCellIndex == 0)
        return;
      this.m_dicRecordsCells.SetCellRecord(rowFromCellIndex, columnFromCellIndex, cell);
    }
  }

  public void UpdateStringIndexes(List<int> arrNewIndexes)
  {
    if (arrNewIndexes == null)
      throw new ArgumentNullException(nameof (arrNewIndexes));
    this.ParseData();
    this.m_dicRecordsCells.UpdateStringIndexes(arrNewIndexes);
  }

  public void RemoveMergedCells(IRange range)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    this.ParseData();
    int row = range.Row;
    int column = range.Column;
    int lastRow = range.LastRow;
    int lastColumn = range.LastColumn;
    for (int firstRow = row; firstRow <= lastRow; ++firstRow)
    {
      for (int firstColumn = column; firstColumn <= lastColumn; ++firstColumn)
      {
        if (firstRow != row || firstColumn != column)
          this.m_dicRecordsCells.Remove(RangeImpl.GetCellIndex(firstColumn, firstRow));
      }
    }
  }

  public void SetActiveCell(IRange range) => this.SetActiveCell(range, true);

  public void SetActiveCell(IRange range, bool updateApplication)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    this.ParseData();
    if (updateApplication)
      this.AppImplementation.SetActiveCell(range);
    this.CreateAllSelections();
    this.ActivatePane(range);
    SelectionRecord activeSelection = this.GetActiveSelection();
    int firstCol = range.Column - 1;
    int firstRow = range.Row - 1;
    activeSelection.ColumnActiveCell = (ushort) firstCol;
    activeSelection.RowActiveCell = (ushort) firstRow;
    if (!this.IsParsing)
    {
      if (firstRow > this.Pane.FirstRow)
        this.Pane.FirstRow = (int) (ushort) firstRow;
      if (firstCol > this.Pane.FirstColumn)
        this.Pane.FirstColumn = (int) (ushort) firstCol;
    }
    SelectionRecord.TAddr addr = new SelectionRecord.TAddr(firstRow, range.LastRow - 1, firstCol, range.LastColumn - 1);
    activeSelection.SetSelection(0, addr);
  }

  private void ActivatePane(IRange range)
  {
    if (!this.WindowTwo.IsFreezePanes)
      return;
    IRange splitCell = this.SplitCell;
    this.m_pane.ActivePane = this.TopLeftCell.Row >= splitCell.Row ? (this.TopLeftCell.Column < splitCell.Column ? (ushort) 1 : (ushort) 3) : (this.TopLeftCell.Column < splitCell.Column ? (ushort) 0 : (ushort) 2);
  }

  internal SelectionRecord GetActiveSelection()
  {
    SelectionRecord activeSelection = (SelectionRecord) null;
    int activePane = this.m_pane != null ? (int) this.m_pane.ActivePane : 0;
    int index = 0;
    for (int count = this.m_arrSelections.Count; index < count; ++index)
    {
      SelectionRecord arrSelection = this.m_arrSelections[index];
      if ((int) arrSelection.Pane == activePane)
      {
        activeSelection = arrSelection;
        break;
      }
    }
    if (activeSelection == null && this.m_arrSelections.Count == 1)
      activeSelection = this.m_arrSelections[0];
    return activeSelection;
  }

  public IRange GetActiveCell()
  {
    this.ParseData();
    SelectionRecord activeSelection = this.GetActiveSelection();
    int num1 = 0;
    int num2 = 0;
    if (activeSelection != null)
    {
      if (activeSelection.Addr != null && activeSelection.Addr.Length > 0)
      {
        num1 = activeSelection.Addr[0].EFirstRow;
        num2 = activeSelection.Addr[0].EFirstCol;
      }
      else
      {
        num1 = (int) activeSelection.RowActiveCell;
        num2 = (int) activeSelection.ColumnActiveCell;
      }
    }
    return this[num1 + 1, num2 + 1];
  }

  [CLSCompliant(false)]
  public bool IsArrayFormula(FormulaRecord formula)
  {
    return formula != null && formula.ParsedExpression != null && formula.ParsedExpression.Length != 0 && formula.ParsedExpression[0].TokenCode == FormulaToken.tExp && this.CellRecords.GetArrayRecord(formula.Row + 1, formula.Column + 1) != null;
  }

  public bool IsArrayFormula(long cellIndex)
  {
    this.ParseData();
    return this.m_dicRecordsCells.GetCellRecord(cellIndex) is FormulaRecord cellRecord && this.IsArrayFormula(cellRecord);
  }

  public double InnerGetRowHeight(int iRow, bool bRaiseEvents)
  {
    if (iRow < 1 || iRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException("Value cannot be less 1 and greater than max row index.");
    RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iRow - 1, false);
    bool flag1 = false;
    bool flag2 = false;
    if (row != null)
    {
      int firstColumn = this.FirstColumn;
      int lastColumn = this.LastColumn;
      if (row.IsHidden)
        return 0.0;
      if (this.m_bIsExportDataTable || row.AutoHeight || row.IsBadFontHeight || firstColumn > 0 && lastColumn > 0 && this.CustomHeight && this.StandardHeight == (double) row.Height / 20.0 && this.Range[iRow, firstColumn, iRow, lastColumn].CellStyle.Rotation <= 0 || row.IsWrapText && !this.Range[iRow, firstColumn, iRow, lastColumn].IsMerged)
        return (double) row.Height / 20.0;
      if (!row.HasRowHeight && !this.m_book.IsCreated && this.m_book.Version != ExcelVersion.Excel97to2003 && !this.m_book.IsConverting)
        return this.StandardHeight;
      if (firstColumn <= this.m_book.MaxColumnCount && lastColumn <= this.m_book.MaxColumnCount)
      {
        RowStorageEnumerator enumerator = row.GetEnumerator(this.RecordExtractor) as RowStorageEnumerator;
        double standardFontSize = this.m_book.StandardFontSize;
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl((IApplication) this.AppImplementation, (IWorksheet) this);
        while (enumerator.MoveNext())
        {
          int iColumn = enumerator.ColumnIndex + 1;
          migrantRangeImpl.ResetRowColumn(iRow, iColumn);
          IFont font = migrantRangeImpl.CellStyle.Font;
          double size = font.Size;
          string fontName = font.FontName;
          IRange mergeArea = migrantRangeImpl.MergeArea;
          ICellPositionFormat cellRecord = this.CellRecords[iRow, iColumn];
          if (mergeArea == null || mergeArea.LastRow - mergeArea.Row == 0)
          {
            if (!flag2 && migrantRangeImpl.CellStyle.Rotation > 0 && !migrantRangeImpl.IsMerged && (!row.HasRowHeight || row.DyDescent == 0.0))
            {
              flag1 = true;
              if (!this.Application.SkipAutoFitRow)
              {
                this.AutofitRow(iRow, firstColumn, lastColumn, bRaiseEvents);
                break;
              }
              break;
            }
            if (migrantRangeImpl.HasRichText)
              flag1 = true;
            if (cellRecord != null)
            {
              int index = (int) cellRecord.ExtendedFormatIndex > this.m_book.InnerExtFormats.Count ? this.m_book.DefaultXFIndex : (int) cellRecord.ExtendedFormatIndex;
              if (size > standardFontSize || fontName != this.m_book.StandardFont && !row.IsSpaceBelowRow || cellRecord != null && Convert.ToInt32(this.m_book.InnerExtFormats[index].Record.Rotation) > 0)
              {
                flag1 = true;
                if ((double) row.Height / 20.0 == this.StandardHeight && !this.Application.SkipAutoFitRow)
                {
                  this.AutofitRow(iRow, this.FirstColumn, this.LastColumn, bRaiseEvents);
                  break;
                }
                if (fontName != this.m_book.StandardFont && !row.IsSpaceBelowRow && size <= standardFontSize && (cellRecord == null || Convert.ToInt32(this.m_book.InnerExtFormats[index].Record.Rotation) <= 0) && (double) row.Height / 20.0 - this.StandardHeight > 5.0 && !this.Application.SkipAutoFitRow)
                {
                  this.AutofitRow(iRow, this.FirstColumn, this.LastColumn, bRaiseEvents);
                  break;
                }
                break;
              }
            }
          }
        }
      }
    }
    if (flag1)
    {
      if (!this.Application.SkipAutoFitRow)
        row.AutoHeight = true;
      return (double) row.Height / 20.0;
    }
    double standardHeight = this.StandardHeight;
    if (row != null)
    {
      if (row.IsSpaceAboveRow)
        standardHeight += ApplicationImpl.ConvertFromPixel(1.0, MeasureUnits.Point);
      if (row.IsSpaceBelowRow)
        standardHeight += ApplicationImpl.ConvertFromPixel(1.0, MeasureUnits.Point);
    }
    return standardHeight;
  }

  public override object Clone(object parent, bool cloneShapes)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    this.ParseData();
    WorksheetImpl worksheetImpl = (WorksheetImpl) base.Clone(parent, cloneShapes);
    worksheetImpl.m_rngUsed = (RangeImpl) null;
    worksheetImpl.m_migrantRange = (IMigrantRange) null;
    worksheetImpl.m_pane = (PaneRecord) CloneUtils.CloneCloneable((ICloneable) this.m_pane);
    worksheetImpl.m_arrSortRecords = CloneUtils.CloneCloneable(this.m_arrSortRecords);
    worksheetImpl.m_arrDConRecords = CloneUtils.CloneCloneable(this.m_arrDConRecords);
    worksheetImpl.m_arrAutoFilter = CloneUtils.CloneCloneable(this.m_arrAutoFilter);
    worksheetImpl.m_arrSelections = CloneUtils.CloneCloneable<SelectionRecord>(this.m_arrSelections);
    if (this.m_arrCustomProperties != null)
      worksheetImpl.m_arrCustomProperties = (WorksheetCustomProperties) this.m_arrCustomProperties.CloneAll();
    if (this.m_arrNotes != null)
    {
      worksheetImpl.m_arrNotes = CloneUtils.CloneCloneable<int, NoteRecord>(this.m_arrNotes);
      worksheetImpl.m_arrNotesByCellIndex = CloneUtils.CloneCloneable<long, NoteRecord>(this.m_arrNotesByCellIndex);
    }
    worksheetImpl.m_arrColumnInfo = CloneUtils.CloneArray(this.m_arrColumnInfo);
    worksheetImpl.m_dataValidation = (DataValidationTable) CloneUtils.CloneCloneable((ICloneParent) this.m_dataValidation, (object) worksheetImpl);
    worksheetImpl.m_names = new WorksheetNamesCollection(this.Application, (object) this);
    worksheetImpl.m_pageSetup = this.m_pageSetup.Clone((object) worksheetImpl);
    worksheetImpl.m_mergedCells = (MergeCellsImpl) CloneUtils.CloneCloneable((ICloneParent) this.m_mergedCells, (object) worksheetImpl);
    worksheetImpl.m_autofilters = this.m_autofilters.Clone(worksheetImpl);
    worksheetImpl.m_pivotTables = (PivotTableCollection) CloneUtils.CloneCloneable((ICloneParent) this.m_pivotTables, (object) worksheetImpl);
    worksheetImpl.m_hyperlinks = (HyperLinksCollection) CloneUtils.CloneCloneable((ICloneParent) this.m_hyperlinks, (object) worksheetImpl);
    worksheetImpl.m_arrConditionalFormats = (WorksheetConditionalFormats) CloneUtils.CloneCloneable((ICloneParent) this.m_arrConditionalFormats, (object) worksheetImpl);
    worksheetImpl.m_dicRecordsCells = this.m_dicRecordsCells.Clone((object) worksheetImpl);
    worksheetImpl.m_book.InnerWorksheets.InnerAdd((IWorksheet) worksheetImpl);
    return (object) worksheetImpl;
  }

  public void ReAddAllStrings()
  {
    this.ParseData();
    this.m_dicRecordsCells.ReAddAllStrings();
  }

  public bool? GetStringPreservedValue(ICombinedRange range)
  {
    return this.m_stringPreservedRanges.GetRangeValue(range);
  }

  public void SetStringPreservedValue(ICombinedRange range, bool? value)
  {
    this.m_stringPreservedRanges.SetRange(range, value);
  }

  public override void MarkUsedReferences(bool[] usedItems)
  {
    this.m_dicRecordsCells.MarkUsedReferences(usedItems);
    if (this.m_dataValidation != null)
      this.m_dataValidation.MarkUsedReferences(usedItems);
    if (this.m_arrConditionalFormats != null)
      this.m_arrConditionalFormats.MarkUsedReferences(usedItems);
    IChartShapes charts = this.Charts;
    int index = 0;
    for (int count = charts.Count; index < count; ++index)
    {
      if (charts[index] is ChartImpl)
        (charts[index] as ChartImpl).MarkUsedReferences(usedItems);
    }
  }

  public override void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    this.m_dicRecordsCells.UpdateReferenceIndexes(arrUpdatedIndexes);
    if (this.m_dataValidation != null)
      this.m_dataValidation.UpdateReferenceIndexes(arrUpdatedIndexes);
    if (this.m_arrConditionalFormats != null)
      this.m_arrConditionalFormats.UpdateReferenceIndexes(arrUpdatedIndexes);
    IChartShapes charts = this.Charts;
    int index = 0;
    for (int count = charts.Count; index < count; ++index)
    {
      if (charts[index] is ChartImpl)
        (charts[index] as ChartImpl).UpdateReferenceIndexes(arrUpdatedIndexes);
    }
  }

  protected void CreateEmptyPane()
  {
    this.m_pane = (PaneRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Pane);
  }

  protected void CopyCell(IRange destCell, IRange sourceCell)
  {
    this.CopyCell(destCell, sourceCell, ExcelCopyRangeOptions.None);
  }

  protected void CopyCell(IRange destCell, IRange sourceCell, ExcelCopyRangeOptions options)
  {
    if (destCell == null)
      throw new ArgumentNullException(nameof (destCell));
    if (sourceCell == null)
      throw new ArgumentNullException(nameof (sourceCell));
    RangeImpl dest = (RangeImpl) destCell;
    RangeImpl source = (RangeImpl) sourceCell;
    if (!dest.IsSingleCell || !source.IsSingleCell)
      throw new ArgumentException("Each range argument should contain a single cell");
    dest.ExtendedFormatIndex = source.ExtendedFormatIndex;
    if (source.Record != null && source.Record is FormulaRecord)
    {
      if (sourceCell.HasFormulaArray)
        return;
      FormulaRecord record1 = (FormulaRecord) source.Record;
      FormulaRecord record2 = (FormulaRecord) record1.Clone();
      record2.Row = (int) (ushort) (destCell.Row - 1);
      record2.Column = (int) (ushort) (destCell.Column - 1);
      bool flag = (options & ExcelCopyRangeOptions.UpdateFormulas) != ExcelCopyRangeOptions.None;
      int iRowOffset = flag ? destCell.Row - sourceCell.Row : 0;
      int iColOffset = flag ? destCell.Column - sourceCell.Column : 0;
      record2.ParsedExpression = this.UpdateFormula(record1.ParsedExpression, iRowOffset, iColOffset);
      dest.SetFormula(record2);
    }
    else
      destCell.Value = sourceCell.Value;
    this.CopyComment(source, dest);
  }

  private void UpdateHyperlinks(RangeImpl source, RangeImpl dest)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    int num = dest != null ? this.GetRowCount(source, dest) : throw new ArgumentNullException(nameof (dest));
    int columnCount = this.GetColumnCount(source, dest);
    if (source.Hyperlinks == null)
      return;
    IHyperLinks hyperlinks = source.Hyperlinks;
    HyperLinksCollection hyperLinks = (HyperLinksCollection) this.HyperLinks;
    int index1 = 0;
    for (int count = hyperlinks.Count; index1 < count; ++index1)
    {
      IRange range = hyperlinks[index1].Range;
      long cellIndex = RangeImpl.GetCellIndex(range.Column, range.Row);
      hyperLinks.m_dicCellToList.Remove(cellIndex);
    }
    int index2 = 0;
    for (int count = hyperlinks.Count; index2 < count; ++index2)
    {
      IRange range = hyperlinks[index2].Range;
      ((HyperLinkImpl) hyperlinks[index2]).Range = this[range.Row + num, range.Column + columnCount];
      HyperLinkImpl link = (HyperLinkImpl) hyperlinks[index2];
      hyperLinks.AddToHash(link);
    }
  }

  private int GetColumnCount(RangeImpl source, RangeImpl dest)
  {
    int columnCount = 0;
    if (source.Row != dest.Row)
      columnCount = dest.Column - source.Column;
    return columnCount;
  }

  private int GetRowCount(RangeImpl source, RangeImpl dest)
  {
    int rowCount = 0;
    if (source.Row != dest.Row)
      rowCount = dest.Row - source.Row;
    return rowCount;
  }

  private void CopyComment(RangeImpl source, RangeImpl dest)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    if (dest == null)
      throw new ArgumentNullException(nameof (dest));
    if (!source.IsSingleCell || !dest.IsSingleCell)
      throw new ArgumentException("Ranges should be single cells.");
    if (source.Comment == null)
      return;
    dest.AddComment(source.Comment);
  }

  private void RemoveLastRow(bool bUpdateFormula)
  {
    this.RemoveLastRow(bUpdateFormula, 1, this.LastRow - 1);
  }

  private void RemoveLastRow(bool bUpdateFormula, int count, int index)
  {
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count));
    if (count == 0)
      return;
    this.ParseData();
    int num1 = this.m_dicRecordsCells.Table.LastRow + 1;
    int num2 = 0;
    int iRowIndex = num1;
    while (num2 < count)
    {
      if (iRowIndex < 0)
      {
        count = num2;
        break;
      }
      this.m_dicRecordsCells.RemoveRow(iRowIndex);
      ++num2;
      --iRowIndex;
    }
    this.m_iLastRow = this.m_dicRecordsCells.Table.LastRow + 1;
    if (this.m_iLastRow == 0)
    {
      this.m_iFirstRow = -1;
      this.m_iLastRow = -1;
    }
    if (!bUpdateFormula)
      return;
    Rectangle rectSource = Rectangle.FromLTRB(0, index + count - 1, this.m_book.MaxColumnCount - 1, this.m_book.MaxRowCount - 1);
    Rectangle rectDest = Rectangle.FromLTRB(0, index - 1, this.m_book.MaxColumnCount - 1, this.m_book.MaxRowCount - 1);
    int num3 = this.m_book.AddSheetReference((IWorksheet) this);
    this.m_book.UpdateFormula(num3, rectSource, num3, rectDest);
  }

  private void RemoveLastColumn(bool bUpdateFormula)
  {
    this.ParseData();
    int num1 = this.m_iLastColumn--;
    this.m_dicRecordsCells.RemoveLastColumn(num1);
    if (this.m_iFirstColumn > this.m_iLastColumn)
      this.m_iLastColumn = this.m_iFirstColumn = int.MaxValue;
    if (!bUpdateFormula)
      return;
    Rectangle rectSource = Rectangle.FromLTRB(num1, 0, this.m_book.MaxColumnCount - 1, this.m_book.MaxRowCount - 1);
    Rectangle rectDest = Rectangle.FromLTRB(num1 - 1, 0, this.m_book.MaxColumnCount - 1, this.m_book.MaxRowCount - 1);
    int num2 = this.m_book.AddSheetReference((IWorksheet) this);
    this.m_book.UpdateFormula(num2, rectSource, num2, rectDest);
  }

  private void RemoveLastColumn(bool bUpdateFormula, int count, int index)
  {
    this.ParseData();
    --this.m_iLastColumn;
    WorksheetImpl worksheetImpl;
    for (int index1 = 0; index1 < count && this.m_iLastColumn >= 0; --worksheetImpl.m_iLastColumn)
    {
      this.m_dicRecordsCells.RemoveLastColumn(this.m_iLastColumn + 1);
      ++index1;
      worksheetImpl = this;
    }
    this.m_iLastColumn = this.m_dicRecordsCells.LastColumn + 1;
    if (this.m_iFirstColumn > this.m_iLastColumn)
      this.m_iLastColumn = this.m_iFirstColumn;
    if (!bUpdateFormula)
      return;
    Rectangle rectSource = Rectangle.FromLTRB(index + count - 1, 0, this.m_book.MaxColumnCount - 1, this.m_book.MaxRowCount - 1);
    Rectangle rectDest = Rectangle.FromLTRB(index - 1, 0, this.m_book.MaxColumnCount - 1, this.m_book.MaxRowCount - 1);
    int num = this.m_book.AddSheetReference((IWorksheet) this);
    this.m_book.UpdateFormula(num, rectSource, num, rectDest);
  }

  private void PartialClearRange(Rectangle rect)
  {
    this.ParseData();
    if (!this.m_dicRecordsCells.UseCache)
      return;
    int num1 = rect.Top + 1;
    int num2 = rect.Left + 1;
    int num3 = rect.Bottom + 1;
    int num4 = rect.Right + 1;
    for (int iRow = num1; iRow <= num3; ++iRow)
    {
      for (int iColumn = num2; iColumn <= num4; ++iColumn)
        this.m_dicRecordsCells.GetRange(iRow, iColumn)?.PartialClear();
    }
  }

  private RecordTable CacheAndRemoveFromParent(
    IRange source,
    IRange destination,
    ref int iMaxRow,
    ref int iMaxColumn,
    CellRecordCollection tableSource,
    bool bInsert)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    if (tableSource == null)
      throw new ArgumentNullException(nameof (tableSource));
    WorksheetImpl parent = (WorksheetImpl) destination.Parent;
    WorksheetImpl worksheet = (WorksheetImpl) source.Worksheet;
    int iDeltaRow = destination.Row - source.Row;
    int iDeltaColumn = destination.Column - source.Column;
    int lastColumn = source.LastColumn;
    int column = source.Column;
    return tableSource.CacheAndRemove((RangeImpl) source, iDeltaRow, iDeltaColumn, ref iMaxRow, ref iMaxColumn, bInsert);
  }

  private void CopyCacheInto(RecordTable source, RecordTable destination, bool bUpdateRowRecords)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    if (source.FirstRow < 0)
      return;
    int firstRow = source.FirstRow;
    for (int lastRow = source.LastRow; firstRow <= lastRow; ++firstRow)
    {
      RowStorage row1 = source.Rows[firstRow];
      RowStorage row2 = destination.Rows[firstRow];
      if (row1 == null)
      {
        if (row2 != null && row2.UsedSize == 0 && bUpdateRowRecords)
          destination.RemoveRow(firstRow);
      }
      else
      {
        RowStorage row3 = destination.Rows[firstRow];
        WorksheetHelper.AccessRow((IInternalWorksheet) this, firstRow + 1);
        if (row3 == null)
        {
          row3 = new RowStorage(firstRow, this.AppImplementation.StandardHeightInRowUnits, destination.Workbook.DefaultXFIndex);
          row3.IsFormatted = false;
          ExcelVersion version = destination.Workbook.Version;
          switch (version)
          {
            case ExcelVersion.Excel97to2003:
              row3.SetCellPositionSize(4, this.AppImplementation.RowStorageAllocationBlockSize, version);
              break;
            case ExcelVersion.Excel2007:
            case ExcelVersion.Excel2010:
            case ExcelVersion.Excel2013:
            case ExcelVersion.Excel2016:
            case ExcelVersion.Xlsx:
              row3.SetCellPositionSize(8, this.AppImplementation.RowStorageAllocationBlockSize, version);
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
          destination.SetRow(firstRow, row3);
        }
        if (bUpdateRowRecords)
          row3.CopyRowRecordFrom(row1);
        if (row1.UsedSize > 0)
        {
          WorksheetHelper.AccessColumn((IInternalWorksheet) this, row1.FirstColumn + 1);
          WorksheetHelper.AccessColumn((IInternalWorksheet) this, row1.LastColumn + 1);
          row3.InsertRowData(row1, this.Application.RowStorageAllocationBlockSize, destination.Workbook.HeapHandle);
        }
      }
    }
  }

  private static void ClearRange(IDictionary dictionary, Rectangle rect)
  {
    if (dictionary == null)
      throw new ArgumentNullException(nameof (dictionary));
    int num1 = rect.Top + 1;
    int num2 = rect.Left + 1;
    int num3 = rect.Bottom + 1;
    int num4 = rect.Right + 1;
    for (int firstRow = num1; firstRow <= num3; ++firstRow)
    {
      for (int firstColumn = num2; firstColumn <= num4; ++firstColumn)
      {
        long cellIndex = RangeImpl.GetCellIndex(firstColumn, firstRow);
        dictionary.Remove((object) cellIndex);
      }
    }
  }

  private void UpdateArrayFormula(
    ArrayRecord array,
    IWorksheet destSheet,
    int iDeltaRow,
    int iDeltaColumn)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    WorkbookImpl destBook = destSheet != null ? (WorkbookImpl) destSheet.Workbook : throw new ArgumentNullException(nameof (destSheet));
    array.Formula = destBook.FormulaUtil.UpdateFormula(array.Formula, iDeltaRow, iDeltaColumn, this.m_book, destBook, destSheet as WorksheetImpl);
  }

  private RecordTable GetRecordTable(
    int iRow,
    int iColumn,
    Rectangle rectIntersection,
    RecordTable intersection,
    RecordTable rectSource)
  {
    return !UtilityMethods.Contains(rectIntersection, iColumn, iRow) ? rectSource : intersection;
  }

  private Dictionary<int, int> GetUpdatedXFIndexes(
    int iRow,
    int iColumn,
    int iRowCount,
    int iColCount,
    WorksheetImpl destSheet,
    out Dictionary<int, int> dicFontIndexes)
  {
    if (destSheet == null)
      throw new ArgumentNullException(nameof (destSheet));
    dicFontIndexes = (Dictionary<int, int>) null;
    if (this.m_book == destSheet.Workbook)
      return (Dictionary<int, int>) null;
    this.ParseData();
    dicFontIndexes = new Dictionary<int, int>();
    Dictionary<int, object> hashToAdd = new Dictionary<int, object>();
    IList<ExtendedFormatImpl> arrXFormats = (IList<ExtendedFormatImpl>) new List<ExtendedFormatImpl>();
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    int firstRow = iRow;
    for (int index1 = iRow + iRowCount; firstRow < index1; ++firstRow)
    {
      int firstColumn = iColumn;
      for (int index2 = iColumn + iColCount; firstColumn < index2; ++firstColumn)
      {
        ICellPositionFormat cellRecord = this.m_dicRecordsCells.GetCellRecord(RangeImpl.GetCellIndex(firstColumn, firstRow));
        if (cellRecord != null)
        {
          int extendedFormatIndex = (int) cellRecord.ExtendedFormatIndex;
          innerExtFormats.AddIndex(hashToAdd, arrXFormats, extendedFormatIndex);
        }
      }
    }
    WorkbookImpl parentWorkbook = destSheet.ParentWorkbook;
    Dictionary<int, int> hashExtFormatIndexes = parentWorkbook.InnerExtFormats.Merge(arrXFormats, out dicFontIndexes);
    parentWorkbook.InnerStyles.MergeStyles((IWorkbook) this.m_book, ExcelStyleMergeOptions.CreateDiffName, hashExtFormatIndexes);
    return hashExtFormatIndexes;
  }

  private void ClearCell(long cellIndex)
  {
    this.ParseData();
    this.m_dicRecordsCells.Remove(cellIndex);
    this.m_dicRecordsCells.GetRange(cellIndex)?.Clear();
  }

  private void SetArrayFormulaRanges(ArrayRecord array)
  {
    this.ParseData();
    Ptg ptg = FormulaUtil.CreatePtg(FormulaToken.tExp, (object) array.FirstRow, (object) array.FirstColumn);
    int lastRow = array.LastRow;
    int lastColumn = array.LastColumn;
    int firstRow = array.FirstRow;
    int firstColumn = array.FirstColumn;
    for (int index1 = firstRow; index1 <= lastRow; ++index1)
    {
      for (int index2 = firstColumn; index2 <= lastColumn; ++index2)
      {
        long cellIndex = RangeImpl.GetCellIndex(index2 + 1, index1 + 1);
        FormulaRecord record = (FormulaRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Formula);
        record.Row = index1;
        record.Column = index2;
        record.ParsedExpression = new Ptg[1]
        {
          (Ptg) ptg.Clone()
        };
        this.m_dicRecordsCells.SetCellRecord(cellIndex, (ICellPositionFormat) record);
        ((RangeImpl) this.Range[index1 + 1, index2 + 1]).UpdateRecord();
      }
    }
    this.UpdateFirstLast(firstRow + 1, firstColumn + 1);
    this.UpdateFirstLast(lastRow + 1, lastColumn + 1);
  }

  [CLSCompliant(false)]
  protected void RemoveArrayFormula(ArrayRecord record, bool bClearRange)
  {
    this.ParseData();
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    int num1 = record.FirstRow + 1;
    int num2 = record.FirstColumn + 1;
    int num3 = record.LastRow + 1;
    int num4 = record.LastColumn + 1;
    for (int iRow = num1; iRow <= num3; ++iRow)
    {
      for (int iColumn = num2; iColumn <= num4; ++iColumn)
        this.m_dicRecordsCells.SetCellRecord(iRow, iColumn, (ICellPositionFormat) null);
    }
  }

  private ArrayRecord CreateArrayFormula(
    ArrayRecord arraySource,
    IRange destination,
    IRange source,
    int iRow,
    int iColumn,
    bool bUpdateFormula)
  {
    if (arraySource == null)
      throw new ArgumentNullException(nameof (arraySource));
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    this.ParseData();
    Rectangle rectangle = Rectangle.Intersect(Rectangle.FromLTRB(arraySource.FirstRow, arraySource.FirstColumn, arraySource.LastRow, arraySource.LastColumn), Rectangle.FromLTRB(source.Row - 1, source.Column - 1, source.LastRow - 1, source.LastColumn - 1));
    if (rectangle.IsEmpty)
      throw new ArgumentNullException("Intersection is empty");
    rectangle.Offset(destination.Row - source.Row, destination.Column - source.Column);
    if (rectangle.Left < 0)
      throw new ArgumentOutOfRangeException();
    if (rectangle.Top < 0)
      throw new ArgumentOutOfRangeException();
    ArrayRecord record = (ArrayRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Array);
    record.FirstRow = iRow + destination.Row - source.Row - 1;
    record.FirstColumn = iColumn + destination.Column - source.Column - 1;
    record.LastRow = record.FirstRow - rectangle.Left + rectangle.Right;
    record.LastColumn = record.FirstColumn - rectangle.Top + rectangle.Bottom;
    record.IsRecalculateAlways = true;
    record.IsRecalculateOnOpen = true;
    int iRowOffset = bUpdateFormula ? record.FirstRow - arraySource.FirstRow : 0;
    int iColOffset = bUpdateFormula ? record.FirstColumn - arraySource.FirstColumn : 0;
    record.Formula = this.UpdateFormula(arraySource.Formula, iRowOffset, iColOffset);
    return record;
  }

  protected void CheckRangesSizes(IRange destination, IRange source)
  {
    if (destination.LastRow - destination.Row != source.LastRow - source.Row || destination.LastColumn - destination.Column != source.LastColumn - source.Column)
      throw new ArgumentException("Ranges do not fit each other");
  }

  private void CopyRangeMerges(IRange destination, IRange source)
  {
    WorksheetImpl.CopyRangeMerges(destination, source, false);
  }

  private static void CopyRangeMerges(IRange destination, IRange source, bool bDeleteSource)
  {
    RangeImpl source1 = (RangeImpl) source;
    RangeImpl destination1 = (RangeImpl) destination;
    MergeCellsImpl mergeCells1 = source1.InnerWorksheet.MergeCells;
    MergeCellsImpl mergeCells2 = destination1.InnerWorksheet.MergeCells;
    if (mergeCells1 == null)
      return;
    if (mergeCells1 == mergeCells2)
    {
      mergeCells1.CopyMoveMerges((IRange) destination1, (IRange) source1, bDeleteSource);
    }
    else
    {
      int iRowDelta = destination.Row - source.Row;
      int num = destination.Column - source.Column;
      List<MergeCellsRecord.MergedRegion> mergesToCopyMove = mergeCells1.FindMergesToCopyMove((IRange) source1, bDeleteSource);
      if (bDeleteSource)
      {
        Rectangle range = Rectangle.FromLTRB(destination.Column - 1, destination.Row - 1, destination.LastColumn - 1, destination.LastRow - 1);
        mergeCells2.DeleteMerge(range);
      }
      if (!bDeleteSource)
        mergeCells1.CopyMerges(destination, source, mergesToCopyMove, iRowDelta, num);
      mergeCells2.AddCache(mergesToCopyMove, iRowDelta, num);
    }
  }

  [CLSCompliant(false)]
  protected internal NoteRecord GetNoteByObjectIndex(int index)
  {
    if (index < 0)
      throw new ArgumentOutOfRangeException("index < 0");
    this.ParseData();
    NoteRecord noteByObjectIndex = (NoteRecord) null;
    if (this.m_arrNotes != null)
      this.m_arrNotes.TryGetValue(index, out noteByObjectIndex);
    return noteByObjectIndex;
  }

  [CLSCompliant(false)]
  protected internal void AddNote(NoteRecord note)
  {
    this.ParseData();
    int objId = (int) note.ObjId;
    bool flag = this.m_arrNotes == null;
    if (!flag && this.m_arrNotes.ContainsKey(objId))
    {
      NoteRecord arrNote = this.m_arrNotes[objId];
      this.m_arrNotesByCellIndex.Remove(RangeImpl.GetCellIndex((int) arrNote.Column, (int) arrNote.Row));
    }
    else if (flag)
    {
      this.m_arrNotes = new SortedList<int, NoteRecord>();
      this.m_arrNotesByCellIndex = new SortedList<long, NoteRecord>();
    }
    this.m_arrNotes[objId] = note;
    this.m_arrNotesByCellIndex[RangeImpl.GetCellIndex((int) note.Column, (int) note.Row)] = note;
  }

  public void AutofitRow(int rowIndex, int firstColumn, int lastColumn, bool bRaiseEvents)
  {
    bool isConverting = this.m_book.IsConverting;
    this.m_book.IsConverting = false;
    this.ParseData();
    RichTextString richText = new RichTextString(this.Application, (object) this, false, true);
    if (firstColumn == 0 || lastColumn == 0 || firstColumn > lastColumn)
      return;
    SizeF sizeF1 = new SizeF(0.0f, 0.0f);
    bool flag = false;
    bool bIsMergedAndWrapped = false;
    MergeCellsRecord.MergedRegion mergedRegion = (MergeCellsRecord.MergedRegion) null;
    for (int index = firstColumn; index <= lastColumn; ++index)
    {
      long cellIndex = RangeImpl.GetCellIndex(index, rowIndex);
      if (this.m_dicRecordsCells.Contains(cellIndex))
      {
        if (this.m_mergedCells != null)
        {
          MergeCellsRecord.MergedRegion mergedCell = this.m_mergedCells[Rectangle.FromLTRB(index - 1, rowIndex - 1, index - 1, rowIndex - 1)];
          if (mergedCell != null && mergedCell.RowTo - mergedCell.RowFrom != 0)
          {
            mergedRegion = (MergeCellsRecord.MergedRegion) null;
            continue;
          }
        }
        SizeF sizeF2 = this.MeasureCell(cellIndex, true, richText, false, out bIsMergedAndWrapped);
        if ((double) sizeF1.Height < (double) sizeF2.Height && (!this[rowIndex, index].HasNumber || this.Workbook.StandardFontSize != this[rowIndex, index].CellStyle.Font.Size))
          sizeF1.Height = sizeF2.Height;
        if (this.Range[rowIndex, index].CellStyle.Rotation > 0 && (double) sizeF1.Height < (double) sizeF2.Width && !this[rowIndex, index].IsMerged && !this.GetExtendedFormat(cellIndex).Record.isWrappedFirst)
        {
          sizeF1.Height = sizeF2.Width;
          flag = true;
        }
      }
    }
    if ((double) sizeF1.Height == 0.0)
      sizeF1.Height = (this.m_book.Styles["Normal"].Font as FontWrapper).Wrapped.MeasureString('0'.ToString()).Height;
    double num = flag ? ApplicationImpl.ConvertFromPixel((double) sizeF1.Height + this.StandardWidth, MeasureUnits.Point) : ApplicationImpl.ConvertFromPixel((double) sizeF1.Height, MeasureUnits.Point);
    if (num > 409.5)
      num = 409.5;
    if (num > this.StandardHeight)
      (this.Range[rowIndex, firstColumn] as RangeImpl).SetRowHeight(num, bIsMergedAndWrapped, bRaiseEvents);
    else
      (this.Range[rowIndex, firstColumn] as RangeImpl).SetRowHeight(this.StandardHeight, bIsMergedAndWrapped, bRaiseEvents);
    RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, rowIndex - 1, false);
    if (row != null)
      row.AutoHeight = true;
    this.m_book.IsConverting = isConverting;
  }

  internal void InnerSetRowHeight(
    int iRowIndex,
    double value,
    bool bIsBadFontHeight,
    MeasureUnits units,
    bool bRaiseEvents)
  {
    value = this.Application.ConvertUnits(value, units, MeasureUnits.Point);
    RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iRowIndex - 1, true);
    if (value == 0.0)
    {
      row.IsHidden = true;
    }
    else
    {
      ushort num = (ushort) Math.Round(value * 20.0);
      if ((int) row.Height != (int) num)
      {
        row.IsHidden = false;
        row.Height = num;
        row.IsBadFontHeight = bIsBadFontHeight;
        WorksheetHelper.AccessRow((IInternalWorksheet) this, iRowIndex);
        this.SetChanged();
      }
      row.AutoHeight = !bIsBadFontHeight;
      if (!bRaiseEvents)
        return;
      this.RaiseRowHeightChangedEvent(iRowIndex, value);
    }
  }

  private bool IsRowEmpty(int iRowIndex) => this.IsRowEmpty(iRowIndex, true);

  private bool IsRowEmpty(int iRowIndex, bool bCheckStyle)
  {
    this.ParseData();
    if (iRowIndex < this.m_iFirstRow || iRowIndex > this.m_iLastRow)
      return true;
    int defaultXfIndex = this.m_book.DefaultXFIndex;
    for (int iFirstColumn = this.m_iFirstColumn; iFirstColumn <= this.m_iLastColumn; ++iFirstColumn)
    {
      long cellIndex = RangeImpl.GetCellIndex(iFirstColumn, iRowIndex);
      if (this.m_dicRecordsCells.Contains(cellIndex))
      {
        bool flag = true;
        ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
        ICellPositionFormat cellRecord = this.m_dicRecordsCells.GetCellRecord(cellIndex);
        if (bCheckStyle && cellRecord.TypeCode == TBIFFRecord.Blank)
        {
          int extendedFormatIndex = (int) cellRecord.ExtendedFormatIndex;
          ExtendedFormatRecord record1 = innerExtFormats[defaultXfIndex].Record;
          ExtendedFormatRecord record2 = innerExtFormats[extendedFormatIndex].Record;
          if (extendedFormatIndex == defaultXfIndex || extendedFormatIndex == 0 || (int) record1.FillIndex == (int) record2.FillIndex && (int) record1.BorderIndex == (int) record2.BorderIndex && (int) record1.AdtlFillPattern == (int) record2.AdtlFillPattern && (int) record1.BottomBorderPaletteIndex == (int) record2.BottomBorderPaletteIndex && (int) record1.LeftBorderPaletteIndex == (int) record2.LeftBorderPaletteIndex && (int) record1.RightBorderPaletteIndex == (int) record2.RightBorderPaletteIndex && (int) record1.TopBorderPaletteIndex == (int) record2.TopBorderPaletteIndex)
            flag = false;
        }
        if (flag)
          return false;
      }
    }
    return true;
  }

  private bool IsColumnEmpty(int iColumnIndex) => this.IsColumnEmpty(iColumnIndex, true);

  private bool IsColumnEmpty(int iColumnIndex, bool bIgnoreStyles)
  {
    this.ParseData();
    if (iColumnIndex < 1 || iColumnIndex > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (iColumnIndex), "Value cannot be less 1 and greater than max column index.");
    if (iColumnIndex < this.m_iFirstColumn || iColumnIndex > this.m_iLastColumn)
      return true;
    int defaultXfIndex = this.m_book.DefaultXFIndex;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
    {
      long cellIndex = RangeImpl.GetCellIndex(iColumnIndex, iFirstRow);
      if (this.m_dicRecordsCells.Contains(cellIndex))
      {
        bool flag = true;
        ICellPositionFormat cellRecord = this.m_dicRecordsCells.GetCellRecord(cellIndex);
        if (bIgnoreStyles && cellRecord.TypeCode == TBIFFRecord.Blank)
        {
          int extendedFormatIndex = (int) cellRecord.ExtendedFormatIndex;
          if (extendedFormatIndex == defaultXfIndex || extendedFormatIndex == 0)
            flag = false;
        }
        if (flag)
          return false;
      }
    }
    return true;
  }

  private int ParseRange(IMigrantRange range, string strRowString, string separator, int i)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    if (strRowString == null)
      throw new ArgumentNullException(nameof (strRowString));
    if (separator == null)
      throw new ArgumentNullException(nameof (separator));
    string csvQualifier = this.Application.CsvQualifier;
    int length1 = strRowString.Length;
    int length2 = separator.Length;
    bool flag = true;
    int startIndex = i;
    int num1 = i;
    while (flag && num1 < length1)
    {
      if ((int) strRowString[num1] == (int) csvQualifier[0] && num1 + 1 < length1)
      {
        int num2 = strRowString.IndexOf(csvQualifier[0], num1 + 1);
        if (num2 != -1)
          num1 = num2 + 1;
        else
          ++num1;
      }
      else if (string.CompareOrdinal(strRowString, num1, separator, 0, length2) == 0)
        flag = false;
      else
        ++num1;
    }
    int length3 = num1 - startIndex;
    string str = strRowString.Substring(startIndex, length3);
    if (length3 > 1 && (int) str[0] == (int) csvQualifier[0] && (int) str[length3 - 1] == (int) csvQualifier[0])
      str = str.Substring(1, length3 - 2);
    string s = str.Replace(csvQualifier + csvQualifier, csvQualifier);
    if (s.IndexOf(this.Application.CsvRecordDelimiter) >= 0 || s.IndexOf("\n") >= 0 || s.IndexOf("\r") >= 0)
      range.WrapText = true;
    if (!string.IsNullOrEmpty(s) && (s.Length > 1 && s[0] == '=' && char.IsLetter(s, 1) || this.Application.PreserveCSVDataTypes))
      range.Value = s;
    else
      range.Text = s;
    return Math.Min(length1, num1 + length2 - 1);
  }

  internal SizeF MeasureCell(IRange cell, bool bAutoFitRows, bool ignoreRotation)
  {
    return this.MeasureCell(RangeImpl.GetCellIndex(cell.Column, cell.Row), bAutoFitRows, ignoreRotation);
  }

  internal SizeF MeasureCell(long cellIndex, bool bAutoFitRows, bool ignoreRotation)
  {
    RichTextString richText = new RichTextString(this.Application, (object) this, false, true);
    bool bIsMergedAndWrapped = false;
    return this.MeasureCell(cellIndex, bAutoFitRows, richText, ignoreRotation, out bIsMergedAndWrapped);
  }

  private SizeF MeasureCell(
    long cellIndex,
    bool bAutoFitRows,
    RichTextString richText,
    bool ignoreRotation,
    out bool bIsMergedAndWrapped)
  {
    this.ParseData();
    this.m_dicRecordsCells.FillRTFString(cellIndex, bAutoFitRows, richText);
    int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(cellIndex);
    int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(cellIndex);
    bool flag = false;
    string text = richText.Text;
    richText.CellIndex = cellIndex;
    if (text == null || text.Length == 0)
    {
      bIsMergedAndWrapped = false;
      return new SizeF(0.0f, 0.0f);
    }
    if (this.m_mergedCells != null)
    {
      MergeCellsRecord.MergedRegion mergedCell = this.m_mergedCells[Rectangle.FromLTRB(columnFromCellIndex - 1, rowFromCellIndex - 1, columnFromCellIndex - 1, rowFromCellIndex - 1)];
      if (mergedCell != null && (bAutoFitRows && (mergedCell.RowFrom <= rowFromCellIndex - 1 || mergedCell.RowTo >= rowFromCellIndex - 1) || !bAutoFitRows && (mergedCell.ColumnFrom <= columnFromCellIndex - 1 || mergedCell.ColumnTo >= columnFromCellIndex - 1)))
        flag = true;
    }
    FontImpl wrapped = (this.m_book.Styles["Normal"].Font as FontWrapper).Wrapped;
    ExtendedFormatImpl extendedFormat = this.GetExtendedFormat(cellIndex);
    int rotation = extendedFormat.Rotation;
    SizeF sizeF = richText.StringSize;
    int horizontalAlignment = (int) extendedFormat.HorizontalAlignment;
    if (bAutoFitRows)
    {
      IRange range = this[rowFromCellIndex, columnFromCellIndex];
      double indentLevel = (double) this[rowFromCellIndex, columnFromCellIndex].IndentLevel;
      double columnWidth1 = (double) this.GetColumnWidthInPixels(columnFromCellIndex);
      double columnWidth2 = 0.0;
      if (indentLevel > 0.0 || extendedFormat.IsVerticalText)
      {
        columnWidth2 = (double) (int) ((double) this.AutoFitManagerImpl.MeasureString("0", new Font(range.CellStyle.Font.FontName, (float) range.CellStyle.Font.Size), new RectangleF(0.0f, 0.0f, 1800f, 100f), false).Width + 0.05);
        if (extendedFormat.IsVerticalText)
          columnWidth2 += this.StandardWidth;
        double num = indentLevel * columnWidth2;
        if (num < columnWidth1)
          columnWidth1 -= num;
        else
          columnWidth1 = columnWidth2;
      }
      if ((!flag || this.hasWrapMerge) && extendedFormat.WrapText && (!range.IsBlank || this.Workbook.StandardFontSize == range.CellStyle.Font.Size))
      {
        int wrappedCell = this.AutoFitManagerImpl.CalculateWrappedCell(extendedFormat, text, (int) columnWidth1, this.AppImplementation);
        if (this[rowFromCellIndex, columnFromCellIndex].HasNumber)
          sizeF.Width = (float) wrappedCell;
        else
          sizeF.Height = (float) wrappedCell;
      }
      if (extendedFormat.Record.isWrappedFirst && rotation > 0)
        ignoreRotation = true;
      if (!ignoreRotation && !flag && rotation > 0 && this.m_book.IsCreated)
      {
        if (rotation == 90 || rotation == 180)
        {
          if (range != null)
          {
            Font font = new Font(range.CellStyle.Font.FontName, (float) range.CellStyle.Font.Size);
            RectangleF rectF = new RectangleF(0.0f, 0.0f, 1800f, 100f);
            sizeF.Width = (float) ((int) ((double) this.AutoFitManagerImpl.MeasureString(text, font, rectF, false).Width + 0.05) + 14) - (float) this.StandardWidth;
            sizeF.Width = this.UpdateTextWidthOrHeightByRotation(sizeF, rotation, true);
          }
        }
        else
          sizeF.Width = !extendedFormat.IsVerticalText ? this.UpdateTextWidthOrHeightByRotation(sizeF, rotation, false) : (float) Convert.ToInt64(ApplicationImpl.ConvertToPixels((double) this.AutoFitManagerImpl.CalculateWrappedCell(extendedFormat, text, (int) columnWidth2, this.AppImplementation), MeasureUnits.Point) - columnWidth2);
      }
    }
    else
    {
      sizeF = this.UpdateAutofitByIndent(sizeF, extendedFormat);
      if (!ignoreRotation)
        sizeF.Width = this.UpdateTextWidthOrHeightByRotation(sizeF, rotation, false);
      IRange filterRange = this.m_autofilters.FilterRange;
      if (filterRange != null && filterRange.Row == rowFromCellIndex && columnFromCellIndex >= filterRange.Column && columnFromCellIndex <= filterRange.LastColumn)
        sizeF = this.UpdateAutoFitByAutoFilter(sizeF, extendedFormat, this.m_dicRecordsCells, cellIndex);
    }
    bIsMergedAndWrapped = flag && extendedFormat.WrapText;
    if (this.GetCellType(rowFromCellIndex, columnFromCellIndex, true) == WorksheetImpl.TRangeValueType.Blank)
      richText.Text = string.Empty;
    return sizeF;
  }

  private Size WrapLine(IWorksheet sheet, IRichTextString rtf, int columnIndex)
  {
    RichTextString richTextString = rtf as RichTextString;
    string[] strArray = richTextString.Text.Split('\n');
    int startIndex = 0;
    Size empty = Size.Empty;
    int columnWidthInPixels = sheet.GetColumnWidthInPixels(columnIndex);
    foreach (string line in strArray)
    {
      RichTextString stringPart = richTextString.Clone(richTextString.Parent) as RichTextString;
      string str = line.TrimEnd('\r');
      stringPart.Substring(startIndex, str.Length);
      Size size = this.WrapSingleLine(line, columnWidthInPixels, stringPart);
      empty.Height += size.Height;
      empty.Width = Math.Max(size.Width, empty.Width);
      startIndex += line.Length + 1;
    }
    return empty;
  }

  private Size WrapSingleLine(string line, int availableWidth, RichTextString stringPart)
  {
    Size empty = Size.Empty;
    SizeF sizeF = stringPart.StringSize;
    if ((int) sizeF.Width > availableWidth)
      sizeF = (SizeF) this.FitByWords(stringPart, availableWidth);
    empty.Height += (int) sizeF.Height;
    empty.Width = Math.Max((int) sizeF.Width, empty.Width);
    return empty;
  }

  private Size FitByWords(RichTextString stringPart, int availableWidth)
  {
    RichTextString originalString = (RichTextString) stringPart.Clone(stringPart.Parent);
    int currentIndex = 0;
    int startIndex = 0;
    int length = stringPart.Text.Length;
    Size empty = Size.Empty;
    while (startIndex < length)
    {
      stringPart = this.AddNextWord(originalString, currentIndex, ref currentIndex);
      RichTextString richTextString = (RichTextString) null;
      int num = -1;
      SizeF stringSize;
      for (stringSize = stringPart.StringSize; (double) stringSize.Width < (double) availableWidth && currentIndex < originalString.Text.Length; stringSize = stringPart.StringSize)
      {
        richTextString = stringPart;
        num = currentIndex;
        stringPart = this.AddNextWord(originalString, startIndex, ref currentIndex);
      }
      if ((double) stringSize.Width > (double) availableWidth)
      {
        stringPart = richTextString;
        currentIndex = num;
      }
      SizeF sizeF;
      if (stringPart != null)
      {
        sizeF = stringPart.StringSize;
      }
      else
      {
        currentIndex = startIndex;
        sizeF = this.SplitByChars(originalString, startIndex, ref currentIndex, availableWidth);
      }
      empty.Width = Math.Max((int) sizeF.Width, empty.Width);
      empty.Height += (int) sizeF.Height;
      startIndex = currentIndex;
      if (currentIndex == 0)
        ++startIndex;
    }
    return empty;
  }

  private SizeF SplitByChars(
    RichTextString originalString,
    int startIndex,
    ref int currentIndex,
    int availableWidth)
  {
    int length = originalString.Text.Length;
    Size size1 = Size.Empty;
    Size empty = Size.Empty;
    while (currentIndex < length && size1.Width < availableWidth)
    {
      RichTextString richTextString = (RichTextString) originalString.Clone(originalString.Parent);
      richTextString.Substring(startIndex, currentIndex - startIndex + 1);
      Size size2 = size1;
      size1 = richTextString.StringSize.ToSize();
      if (size1.Width > availableWidth)
      {
        size1 = size2;
        break;
      }
      ++currentIndex;
    }
    return (SizeF) size1;
  }

  private RichTextString AddNextWord(
    RichTextString originalString,
    int startIndex,
    ref int currentIndex)
  {
    RichTextString richTextString = (RichTextString) originalString.Clone(originalString.Parent);
    int num = richTextString.Text.IndexOfAny(new char[2]
    {
      '-',
      ' '
    }, currentIndex);
    if (num < 0)
      num = richTextString.Text.Length - 1;
    richTextString.Substring(startIndex, num - startIndex + 1);
    currentIndex = num + 1;
    return richTextString;
  }

  private SizeF UpdateAutofitByIndent(SizeF curSize, ExtendedFormatImpl format)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (format.HorizontalAlignment != ExcelHAlign.HAlignLeft && format.HorizontalAlignment != ExcelHAlign.HAlignRight && format.Rotation != 0 && format.IndentLevel == 0)
      return curSize;
    curSize.Width += (float) (format.IndentLevel * 12);
    return curSize;
  }

  private float UpdateTextWidthOrHeightByRotation(SizeF size, int rotation, bool bUpdateHeight)
  {
    switch (rotation)
    {
      case 0:
        return !bUpdateHeight ? size.Width : size.Height;
      case 90:
      case 180:
        return !bUpdateHeight ? size.Height : size.Width;
      default:
        if (rotation > 90)
          rotation -= 90;
        if (bUpdateHeight)
          rotation = 90 - rotation;
        float num = (float) Math.Sin(Math.PI / 180.0 * (double) rotation) * size.Height;
        return (float) Math.Cos(Math.PI / 180.0 * (double) rotation) * size.Width + num;
    }
  }

  private FontImpl GetFontByExtendedFormatIndex(ICellPositionFormat cellFormat, out int rotation)
  {
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    if (innerExtFormats.Count <= (int) cellFormat.ExtendedFormatIndex)
      throw new ArgumentException(nameof (cellFormat));
    ExtendedFormatImpl extendedFormatImpl = innerExtFormats[(int) cellFormat.ExtendedFormatIndex];
    rotation = extendedFormatImpl.Rotation;
    return (FontImpl) this.m_book.InnerFonts[extendedFormatImpl.FontIndex];
  }

  protected override void CopyOptions(WorksheetBaseImpl sourceSheet)
  {
    base.CopyOptions(sourceSheet);
    WorksheetImpl worksheetImpl = (WorksheetImpl) sourceSheet;
    this.IsRowColumnHeadersVisible = worksheetImpl.IsRowColumnHeadersVisible;
    this.IsStringsPreserved = worksheetImpl.IsStringsPreserved;
    this.IsGridLinesVisible = worksheetImpl.IsGridLinesVisible;
    this.m_pane = (PaneRecord) CloneUtils.CloneCloneable((ICloneable) worksheetImpl.m_pane);
  }

  protected override void OnRealIndexChanged(int iOldIndex)
  {
    if (this.m_names == null)
      return;
    this.m_names.SetSheetIndex(this.RealIndex);
  }

  private void OnInsertRowColumnComplete(int iRowIndex, int iRowCount, bool bRow)
  {
  }

  private SizeF UpdateAutoFitByAutoFilter(
    SizeF size,
    ExtendedFormatImpl format,
    CellRecordCollection col,
    long cellIndex)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (col == null)
      throw new ArgumentNullException(nameof (col));
    ExcelHAlign horizontalAlignment = format.HorizontalAlignment;
    int rotation = format.Rotation;
    switch (horizontalAlignment)
    {
      case ExcelHAlign.HAlignLeft:
      case ExcelHAlign.HAlignCenter:
        size.Width += horizontalAlignment == ExcelHAlign.HAlignLeft ? 16f : 32f;
        return size;
      case ExcelHAlign.HAlignRight:
      case ExcelHAlign.HAlignFill:
      case ExcelHAlign.HAlignCenterAcrossSelection:
        return size;
      case ExcelHAlign.HAlignJustify:
      case ExcelHAlign.HAlignDistributed:
        if (rotation > 0 && rotation < 90)
          size.Width += 16f;
        return size;
      default:
        return this.UpdateAutoFilterForGeneralAllignment(size, rotation, col, cellIndex);
    }
  }

  private SizeF UpdateAutoFilterForGeneralAllignment(
    SizeF size,
    int iRot,
    CellRecordCollection col,
    long cellIndex)
  {
    if (col == null)
      throw new ArgumentNullException(nameof (col));
    if (this.m_dicRecordsCells.ContainFormulaBoolOrError(cellIndex) || this.m_dicRecordsCells.ContainBoolOrError(cellIndex))
    {
      size.Width += 32f;
      return size;
    }
    if (iRot > 0 && iRot < 90 || iRot >= 180)
    {
      size.Width += 16f;
      return size;
    }
    if (!this.m_dicRecordsCells.ContainFormulaNumber(cellIndex) && !this.m_dicRecordsCells.ContainNumber(cellIndex) && iRot == 0)
      size.Width += 16f;
    return size;
  }

  private void CreateMigrantRange()
  {
    this.m_migrantRange = (IMigrantRange) new MigrantRangeImpl(this.Application, (IWorksheet) this);
  }

  private IStyle GetDefaultOutlineStyle(IDictionary dicOutlines, int iIndex)
  {
    IOutline outline = dicOutlines != null ? (IOutline) dicOutlines[(object) iIndex] : throw new ArgumentNullException(nameof (dicOutlines));
    return (IStyle) new ExtendedFormatWrapper(this.m_book, outline != null ? (int) outline.ExtendedFormatIndex : this.m_book.DefaultXFIndex);
  }

  private int ssSetDefaultRowColumnStyle(
    int iIndex,
    int iEndIndex,
    IStyle defaultStyle,
    IDictionary dicOutlines,
    WorksheetImpl.OutlineDelegate createOutline,
    bool bIsRow)
  {
    this.ParseData();
    int correctIndex = this.ConvertStyleToCorrectIndex(defaultStyle);
    for (int index = iIndex; index <= iEndIndex; ++index)
      (dicOutlines.Contains((object) index) ? (IOutline) dicOutlines[(object) index] : createOutline(index)).ExtendedFormatIndex = (ushort) correctIndex;
    return correctIndex;
  }

  private int SetDefaultRowColumnStyle(
    int iIndex,
    int iEndIndex,
    IStyle defaultStyle,
    IList outlines,
    WorksheetImpl.OutlineDelegate createOutline,
    bool bIsRow)
  {
    this.ParseData();
    int correctIndex = this.ConvertStyleToCorrectIndex(defaultStyle);
    for (int index = iIndex; index <= iEndIndex; ++index)
    {
      (outlines[index] != null ? (IOutline) outlines[index] : createOutline(index)).ExtendedFormatIndex = (ushort) correctIndex;
      this.SetCellStyle(index, (ushort) correctIndex);
    }
    return correctIndex;
  }

  private int ConvertStyleToCorrectIndex(IStyle style)
  {
    int index = style != null ? ((IXFIndex) style).XFormatIndex : throw new ArgumentNullException("defaultStyle");
    if (index == int.MinValue)
      throw new ArgumentException("defaultStyle");
    return this.m_book.InnerExtFormats[index].CreateChildFormat().Index;
  }

  private IOutline CreateColumnOutline(int iColumnIndex)
  {
    this.ParseData();
    if (iColumnIndex < 1 || iColumnIndex > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (iColumnIndex), "Column index is out of range.");
    ColumnInfoRecord record = BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo) as ColumnInfoRecord;
    record.FirstColumn = (ushort) (iColumnIndex - 1);
    record.LastColumn = (ushort) (iColumnIndex - 1);
    record.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
    WorksheetHelper.AccessColumn((IInternalWorksheet) this, iColumnIndex);
    this.m_arrColumnInfo[iColumnIndex] = record;
    return (IOutline) record;
  }

  private void CopyConditionalFormatsAfterInsert(
    int iIndex,
    int iCount,
    ExcelInsertOptions options,
    bool bIsRow)
  {
    if (options == ExcelInsertOptions.FormatDefault)
      return;
    if (bIsRow)
    {
      int iSourceRow = options == ExcelInsertOptions.FormatAsBefore ? iIndex - 1 : iIndex + iCount;
      int iColumnCount = this.m_iLastColumn - this.m_iFirstColumn + 1;
      int iDestRow = iIndex;
      for (int index = iIndex + iCount; iDestRow < index; ++iDestRow)
        this.CopyMoveConditionalFormatting(iSourceRow, this.m_iFirstColumn, 1, iColumnCount, iDestRow, this.m_iFirstColumn, this, false, false);
    }
    else
    {
      int iSourceColumn = options == ExcelInsertOptions.FormatAsBefore ? iIndex - 1 : iIndex + iCount;
      int iDestColumn = iIndex;
      for (int index = iIndex + iCount; iDestColumn < index; ++iDestColumn)
        this.CopyMoveConditionalFormatting(this.m_iFirstRow, iSourceColumn, this.m_iLastRow, 1, this.m_iFirstRow, iDestColumn, this, false, false);
    }
  }

  private void CopyStylesAfterInsert(
    int iIndex,
    int iCount,
    ExcelInsertOptions options,
    bool bRow)
  {
    int indexForStyleCopy = this.GetIndexForStyleCopy(iIndex, iCount, options);
    int num1;
    int num2;
    if (!bRow)
    {
      num1 = this.m_iFirstRow;
      num2 = this.m_iLastRow;
    }
    else if (this.m_iFirstColumn == int.MaxValue)
    {
      num1 = -1;
      num2 = -1;
    }
    else
    {
      num1 = this.m_iFirstColumn;
      num2 = this.m_iLastColumn;
    }
    RowStorage sourceRow = (RowStorage) null;
    ColumnInfoRecord sourceColumn = (ColumnInfoRecord) null;
    if (indexForStyleCopy != -1)
    {
      if (bRow)
        sourceRow = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, indexForStyleCopy - 1, false);
      else
        sourceColumn = this.m_arrColumnInfo[indexForStyleCopy];
      if (num1 > 0)
      {
        for (int index1 = num1; index1 <= num2; ++index1)
        {
          ICellPositionFormat cellRecord = this.m_dicRecordsCells.GetCellRecord(bRow ? RangeImpl.GetCellIndex(index1, indexForStyleCopy) : RangeImpl.GetCellIndex(indexForStyleCopy, index1));
          if (cellRecord != null && (int) cellRecord.ExtendedFormatIndex != this.m_book.DefaultXFIndex && (sourceRow == null || (int) cellRecord.ExtendedFormatIndex != (int) sourceRow.ExtendedFormatIndex))
          {
            int num3 = iIndex;
            for (int index2 = iIndex + iCount; num3 < index2; ++num3)
            {
              long key = bRow ? RangeImpl.GetCellIndex(index1, num3) : RangeImpl.GetCellIndex(num3, index1);
              BlankRecord record = (BlankRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Blank);
              record.Row = (bRow ? num3 : index1) - 1;
              record.Column = (bRow ? index1 : num3) - 1;
              record.ExtendedFormatIndex = cellRecord.ExtendedFormatIndex;
              (this.Workbook as WorkbookImpl).UpdateUsedStyleIndex((int) cellRecord.ExtendedFormatIndex, 1);
              this.m_dicRecordsCells.SetCellRecord(key, (ICellPositionFormat) record);
            }
          }
        }
      }
    }
    if (!bRow)
      return;
    int iCurIndex = iIndex;
    for (int index = iIndex + iCount; iCurIndex < index; ++iCurIndex)
      this.CopyRowColumnSettings(sourceRow, sourceColumn, bRow, indexForStyleCopy, iCurIndex, options);
    if (!this.m_isInsertingRow || this.m_book.Version == ExcelVersion.Excel97to2003)
      return;
    this.RowHeightHelper.m_arrSizeSum = new List<double>();
    this.RowHeightHelper.m_arrSizeSum.Add(0.0);
    double total = (double) this.RowHeightHelper.GetTotal(iIndex + iCount);
  }

  private void CopyRowColumnSettings(
    RowStorage sourceRow,
    ColumnInfoRecord sourceColumn,
    bool bRow,
    int iSourceIndex,
    int iCurIndex,
    ExcelInsertOptions options)
  {
    if (options == ExcelInsertOptions.FormatDefault)
    {
      if (bRow)
      {
        RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iCurIndex - 1, false);
        if (row == null)
          return;
        row.SetDefaultRowOptions();
        row.Height = (ushort) this.AppImplementation.StandardHeightInRowUnits;
        row.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
      }
      else
      {
        ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[iCurIndex];
        if (columnInfoRecord == null)
          return;
        columnInfoRecord.FirstColumn = columnInfoRecord.LastColumn = (ushort) (iCurIndex - 1);
        columnInfoRecord.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
        columnInfoRecord.SetDefaultOptions();
      }
    }
    else
    {
      if (iSourceIndex == -1)
        return;
      if (bRow)
      {
        RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iCurIndex - 1, sourceRow != null);
        if (sourceRow == null && row != null)
        {
          row.Height = (ushort) this.AppImplementation.StandardHeightInRowUnits;
          row.SetDefaultRowOptions();
        }
        else
        {
          if (sourceRow == null)
            return;
          row.CopyRowRecordFrom(sourceRow);
          this.RaiseRowHeightChangedEvent(iCurIndex, (double) row.Height / 20.0);
        }
      }
      else
      {
        ColumnInfoRecord columnInfoRecord = (ColumnInfoRecord) CloneUtils.CloneCloneable((ICloneable) sourceColumn);
        this.m_arrColumnInfo[iCurIndex] = columnInfoRecord;
        if (columnInfoRecord == null)
          return;
        columnInfoRecord.FirstColumn = columnInfoRecord.LastColumn = (ushort) (iCurIndex - 1);
      }
    }
  }

  private int GetIndexForStyleCopy(int iIndex, int iCount, ExcelInsertOptions options)
  {
    switch (options)
    {
      case ExcelInsertOptions.FormatAsBefore:
        --iIndex;
        break;
      case ExcelInsertOptions.FormatAsAfter:
        iIndex += iCount;
        break;
      default:
        iIndex = -1;
        break;
    }
    return iIndex;
  }

  internal ExcelFormatType GetFormatType(int iRow, int iColumn, bool bUseDefaultStyle)
  {
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    int index;
    if (bUseDefaultStyle)
    {
      ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[iColumn];
      if (columnInfoRecord == null)
        return ExcelFormatType.General;
      index = (int) columnInfoRecord.ExtendedFormatIndex;
    }
    else
    {
      ICellPositionFormat cellRecord = this.m_dicRecordsCells.GetCellRecord(iRow, iColumn);
      index = cellRecord != null ? (int) cellRecord.ExtendedFormatIndex : this.m_book.DefaultXFIndex;
    }
    return this.m_book.InnerFormats[innerExtFormats[index].NumberFormatIndex].GetFormatType(1.0);
  }

  internal ExcelExportType GetExportType(
    ExcelFormatType formatType,
    int row,
    int column,
    int maxRows,
    ExcelExportDataTableOptions options,
    out System.Type formulaDataType)
  {
    this.ParseData();
    bool flag = (options & ExcelExportDataTableOptions.ComputedFormulaValues) != ExcelExportDataTableOptions.None;
    formulaDataType = (System.Type) null;
    ICellPositionFormat cellRecord = this.m_dicRecordsCells.GetCellRecord(row, column);
    ExcelExportType exportType = ExcelExportType.Text;
    int num1 = row + maxRows;
    bool bUseDefaultStyle = (options & ExcelExportDataTableOptions.DefaultStyleColumnTypes) != ExcelExportDataTableOptions.None;
    for (; row <= num1 && (cellRecord == null || cellRecord != null && cellRecord.TypeCode == TBIFFRecord.Blank); cellRecord = this.m_dicRecordsCells.GetCellRecord(row, column))
    {
      ++row;
      formatType = this.GetFormatType(row, column, bUseDefaultStyle);
    }
    if (cellRecord != null)
    {
      switch (cellRecord.TypeCode)
      {
        case TBIFFRecord.Formula:
          if ((options & ExcelExportDataTableOptions.ComputedFormulaValues) != ExcelExportDataTableOptions.None)
          {
            int num2;
            switch (formatType)
            {
              case ExcelFormatType.General:
                if (flag)
                {
                  IRange range = this.Range[row, column];
                  if (flag)
                  {
                    string calculatedValue = this.Range[row, column].CalculatedValue;
                  }
                  this.m_dicRecordsCells.GetCellRecord(row, column);
                  if (range.HasFormulaStringValue)
                  {
                    exportType = ExcelExportType.Text;
                    goto label_31;
                  }
                  if (range.HasFormulaBoolValue)
                  {
                    exportType = ExcelExportType.Bool;
                    goto label_31;
                  }
                  if (range.HasFormulaDateTime)
                  {
                    exportType = ExcelExportType.DateTime;
                    goto label_31;
                  }
                  if (range.HasFormulaNumberValue)
                  {
                    exportType = ExcelExportType.Number;
                    goto label_31;
                  }
                  goto label_31;
                }
                exportType = ExcelExportType.Text;
                goto label_31;
              case ExcelFormatType.Text:
                goto label_31;
              case ExcelFormatType.DateTime:
                num2 = 3;
                break;
              default:
                num2 = 1;
                break;
            }
            exportType = (ExcelExportType) num2;
            break;
          }
          exportType = ExcelExportType.Formula;
          break;
        case TBIFFRecord.LabelSST:
          exportType = ExcelExportType.Text;
          break;
        case TBIFFRecord.Number:
        case TBIFFRecord.RK:
          int num3;
          switch (formatType)
          {
            case ExcelFormatType.Text:
              goto label_31;
            case ExcelFormatType.DateTime:
              num3 = 3;
              break;
            default:
              num3 = 1;
              break;
          }
          exportType = (ExcelExportType) num3;
          break;
        case TBIFFRecord.BoolErr:
          if (formatType != ExcelFormatType.Text)
          {
            exportType = ((BoolErrRecord) cellRecord).IsErrorCode ? ExcelExportType.Error : ExcelExportType.Bool;
            break;
          }
          break;
        default:
          exportType = ExcelExportType.Text;
          break;
      }
    }
label_31:
    return exportType;
  }

  internal System.Type GetType(ExcelExportType exportType, bool preserveOLEDate)
  {
    switch (exportType)
    {
      case ExcelExportType.Bool:
        return typeof (bool);
      case ExcelExportType.Number:
        return typeof (double);
      case ExcelExportType.Text:
      case ExcelExportType.Error:
      case ExcelExportType.Formula:
        return typeof (string);
      case ExcelExportType.DateTime:
        return !preserveOLEDate ? typeof (DateTime) : typeof (double);
      default:
        throw new ArgumentOutOfRangeException(nameof (exportType));
    }
  }

  internal object GetValue(
    int iRow,
    int iColumn,
    ExcelExportType formatType,
    bool bExportFormulaValues,
    bool preserveOLEDate)
  {
    this.ParseData();
    ICellPositionFormat cellRecord = this.m_dicRecordsCells.GetCellRecord(iRow, iColumn);
    if (cellRecord == null || cellRecord.TypeCode == TBIFFRecord.Blank)
      return (object) DBNull.Value;
    FormulaRecord formulaRecord = (FormulaRecord) null;
    if (cellRecord.TypeCode == TBIFFRecord.Formula)
    {
      formulaRecord = cellRecord as FormulaRecord;
      if (bExportFormulaValues && formulaRecord.IsBlank)
        return (object) DBNull.Value;
      if (bExportFormulaValues)
      {
        string calculatedValue = this.Range[iRow, iColumn].CalculatedValue;
        cellRecord = this.m_dicRecordsCells.GetCellRecord(iRow, iColumn);
        formulaRecord = cellRecord as FormulaRecord;
      }
    }
    object obj;
    if (formatType == ExcelExportType.Text)
    {
      if (!bExportFormulaValues || cellRecord.TypeCode != TBIFFRecord.Formula)
      {
        this.m_isExportDataTable = true;
        obj = (object) this.GetValue(cellRecord, preserveOLEDate);
        this.m_isExportDataTable = false;
      }
      else
      {
        double num = formulaRecord.Value;
        string formulaStringValue = this.GetFormulaStringValue(iRow, iColumn);
        FormatImpl numberFormatSettings = this.m_book.InnerExtFormats[(int) cellRecord.ExtendedFormatIndex].NumberFormatSettings as FormatImpl;
        if (formulaStringValue != null)
          obj = (object) formulaStringValue;
        else if (bExportFormulaValues && formulaRecord.IsError)
          obj = (object) this.GetErrorValueToString(formulaRecord.ErrorValue, iRow);
        else if (bExportFormulaValues && formulaRecord.IsBool)
          obj = (object) this.Range[iRow, iColumn].FormulaBoolValue;
        else if (numberFormatSettings.GetFormatType(num) == ExcelFormatType.DateTime)
        {
          if (!double.IsNaN(num))
          {
            DateTime dateTime = UtilityMethods.ConvertNumberToDateTime(num, this.m_book.Date1904);
            obj = preserveOLEDate ? (object) dateTime.ToOADate() : (object) dateTime;
          }
          else
            obj = (object) this.GetFormulaBoolValue(iRow, iColumn);
        }
        else
          obj = (object) num;
      }
    }
    else
    {
      switch (formatType)
      {
        case ExcelExportType.Bool:
          obj = (object) (bool) (!(cellRecord is BoolErrRecord boolErrRecord) || boolErrRecord.IsErrorCode ? 0 : (boolErrRecord.BoolOrError != (byte) 0 ? 1 : 0));
          break;
        case ExcelExportType.Number:
          obj = (object) (!(cellRecord is IDoubleValue doubleValue1) || doubleValue1.DoubleValue.Equals((object) double.NaN.ToString()) ? 0.0 : doubleValue1.DoubleValue);
          break;
        case ExcelExportType.DateTime:
          double dNumber = cellRecord is IDoubleValue doubleValue2 ? doubleValue2.DoubleValue : double.NaN;
          obj = !preserveOLEDate ? (object) UtilityMethods.ConvertNumberToDateTime(dNumber, this.m_book.Date1904) : (object) dNumber;
          break;
        case ExcelExportType.Error:
          obj = (object) this.GetError(iRow, iColumn);
          break;
        case ExcelExportType.Formula:
          switch (this.GetCellType(iRow, iColumn, true))
          {
            case WorksheetImpl.TRangeValueType.Formula:
            case WorksheetImpl.TRangeValueType.Formula | WorksheetImpl.TRangeValueType.String:
              obj = (object) this.GetFormulaStringValue(iRow, iColumn);
              break;
            case WorksheetImpl.TRangeValueType.Error | WorksheetImpl.TRangeValueType.Formula:
              obj = (object) this.GetFormulaErrorValue(iRow, iColumn);
              break;
            case WorksheetImpl.TRangeValueType.Boolean | WorksheetImpl.TRangeValueType.Formula:
              obj = (object) this.GetFormulaBoolValue(iRow, iColumn).ToString().ToUpper();
              break;
            case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
              obj = (object) this.GetFormulaNumberValue(iRow, iColumn);
              break;
            default:
              return (object) string.Empty;
          }
          break;
        default:
          obj = (object) this.GetText(iRow, iColumn);
          break;
      }
    }
    return obj;
  }

  internal string GetValue(ICellPositionFormat cell, bool preserveOLEDate)
  {
    if (cell == null)
      return string.Empty;
    bool flag1 = false;
    bool flag2 = false;
    object obj1;
    switch (cell.TypeCode)
    {
      case TBIFFRecord.Formula:
        FormulaRecord formula = (FormulaRecord) cell;
        Ptg[] parsedExpression = formula.ParsedExpression;
        if (parsedExpression == null)
        {
          formula = (FormulaRecord) this.GetRecord(formula.Row + 1, formula.Column + 1);
          parsedExpression = formula.ParsedExpression;
        }
        obj1 = (object) (!this.HasArrayFormula(parsedExpression) ? this.GetFormula(cell.Row, cell.Column, parsedExpression, false, this.m_book.FormulaUtil, false) : this.GetFormulaArray(formula)).Replace("_xlfn.", "");
        break;
      case TBIFFRecord.LabelSST:
        object sstContentByIndex = this.m_book.InnerSST.GetSSTContentByIndex(((LabelSSTRecord) cell).SSTIndex);
        obj1 = sstContentByIndex is TextWithFormat textWithFormat ? (textWithFormat.IsEncoded ? (object) string.Empty : (object) textWithFormat.Text) : sstContentByIndex;
        break;
      case TBIFFRecord.Blank:
        obj1 = (object) string.Empty;
        break;
      case TBIFFRecord.Number:
      case TBIFFRecord.RK:
        bool isUnusedXfRemoved = this.m_book.IsUnusedXFRemoved;
        ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
        if (isUnusedXfRemoved != this.m_book.m_bisUnusedXFRemoved)
          cell = this.m_dicRecordsCells.GetCellRecord(cell.Row + 1, cell.Column + 1);
        double doubleValue = ((IDoubleValue) cell).DoubleValue;
        int extendedFormatIndex = (int) cell.ExtendedFormatIndex;
        FormatImpl innerFormat = this.m_book.InnerFormats[innerExtFormats[extendedFormatIndex].NumberFormatIndex];
        System.Globalization.Calendar calendar = CultureInfo.CurrentCulture.DateTimeFormat.Calendar;
        if (innerFormat != null && innerFormat.FormatType == ExcelFormatType.DateTime)
        {
          object obj2;
          if (doubleValue < 0.0)
          {
            obj2 = preserveOLEDate ? (object) DateTime.FromOADate(doubleValue).ToOADate() : (innerFormat.IsTimeFormat(doubleValue) ? (object) DateTime.FromOADate(doubleValue).ToLongTimeString() : (innerFormat.IsDateFormat(doubleValue) ? (object) DateTime.FromOADate(doubleValue).ToShortDateString() : (object) doubleValue));
          }
          else
          {
            if (doubleValue < 60.0 && this.WindowTwo.IsDisplayZeros)
              ++doubleValue;
            if (doubleValue % 1.0 == 0.0)
              flag1 = true;
            else if (doubleValue - 1.0 < 1.0)
              flag2 = true;
            obj2 = doubleValue > calendar.MaxSupportedDateTime.ToOADate() || doubleValue < calendar.MinSupportedDateTime.ToOADate() ? (object) doubleValue : (preserveOLEDate ? (object) DateTime.FromOADate(doubleValue).ToOADate() : (!flag2 || !innerFormat.IsTimeFormat(doubleValue) ? (this.m_isExportDataTable || !flag1 || !innerFormat.IsDateFormat(doubleValue) ? (object) DateTime.FromOADate(doubleValue) : (object) DateTime.FromOADate(doubleValue).ToShortDateString()) : (object) DateTime.FromOADate(doubleValue).ToLongTimeString()));
          }
          if (this.Workbook.Date1904 && doubleValue > 0.0)
          {
            double d = 1462.0 + DateTime.Parse(obj2.ToString()).ToOADate();
            obj2 = preserveOLEDate ? (object) DateTime.FromOADate(d).ToOADate() : (innerFormat.IsTimeFormat(d) ? (object) DateTime.FromOADate(d).ToLongTimeString() : (this.m_isExportDataTable || !innerFormat.IsDateFormat(d) ? (object) DateTime.FromOADate(d) : (object) DateTime.FromOADate(d).ToShortDateString()));
          }
          return WorksheetImpl.ConvertSecondsMinutesToHours(obj2.ToString(), doubleValue);
        }
        obj1 = 7.9228162514264338E+28 <= doubleValue || -7.9228162514264338E+28 >= doubleValue ? (object) doubleValue : (object) (Decimal) doubleValue;
        break;
      case TBIFFRecord.Label:
        obj1 = (object) ((LabelRecord) cell).Label;
        break;
      case TBIFFRecord.BoolErr:
        BoolErrRecord boolErrRecord = (BoolErrRecord) cell;
        int boolOrError = (int) boolErrRecord.BoolOrError;
        obj1 = boolErrRecord.IsErrorCode ? (object) FormulaUtil.ErrorCodeToName[boolOrError] : (object) (boolErrRecord.BoolOrError != (byte) 0).ToString().ToUpper();
        break;
      case TBIFFRecord.String:
        obj1 = (object) ((StringRecord) cell).Value;
        break;
      default:
        throw new ArgumentException("Cannot recognize cell type.");
    }
    return obj1.ToString();
  }

  private void UpdateOutlineAfterXFRemove(ICollection dictOutline, IDictionary dictFormats)
  {
    foreach (IOutline outline in (IEnumerable) dictOutline)
    {
      if (outline != null)
      {
        int extendedFormatIndex = (int) outline.ExtendedFormatIndex;
        if (dictFormats.Contains((object) extendedFormatIndex))
        {
          int dictFormat = (int) dictFormats[(object) extendedFormatIndex];
          outline.ExtendedFormatIndex = (ushort) dictFormat;
        }
      }
    }
  }

  internal static string ConvertSecondsMinutesToHours(string value, double dNumber)
  {
    bool flag = false;
    System.Globalization.Calendar calendar = CultureInfo.CurrentCulture.DateTimeFormat.Calendar;
    if (dNumber % 1.0 == 0.0)
      flag = true;
    if (!flag && dNumber > calendar.MinSupportedDateTime.ToOADate() && dNumber < calendar.MaxSupportedDateTime.ToOADate() && DateTime.FromOADate(dNumber).Millisecond > 500)
    {
      string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
      foreach (Match match in new Regex($"([0-9]*:[0-9]*:[0-9]*\\{decimalSeparator}[0-9]*|[0-9]*:[0-9]*:[0-9]*|[0-9]*:[0-9]*\\{decimalSeparator}[0-9]*|[0-9]*:[0-9]*)").Matches(value))
      {
        string timeSeparator = CultureInfo.CurrentCulture.DateTimeFormat.TimeSeparator;
        string str1 = "00";
        string[] strArray = match.Value.Split(timeSeparator.ToCharArray());
        int minute = DateTime.FromOADate(dNumber).Minute;
        string str2 = strArray[0];
        switch (strArray.Length)
        {
          case 2:
            if (minute + 1 == 60)
            {
              string newValue = (int.Parse(strArray[0]) + 1).ToString(str1) + timeSeparator + strArray[strArray.Length - 1].Replace(strArray[strArray.Length - 1].ToString(), str1);
              value = value.Replace(match.Value, newValue);
              continue;
            }
            continue;
          case 3:
            int second = DateTime.FromOADate(dNumber).Second;
            int num1 = second + (strArray[strArray.Length - 1].Contains(decimalSeparator) ? 0 : 1);
            string newValue1;
            if (num1 == 60)
            {
              int num2 = minute + 1;
              if (num2 == 60)
                newValue1 = (int.Parse(strArray[0]) + 1).ToString(str1) + timeSeparator + str1 + timeSeparator + strArray[strArray.Length - 1].Replace(second.ToString(), str1);
              else
                newValue1 = strArray[0] + timeSeparator + num2.ToString(str1) + timeSeparator + strArray[strArray.Length - 1].Replace(second.ToString(), str1);
            }
            else
              newValue1 = strArray[0] + timeSeparator + strArray[1] + timeSeparator + strArray[strArray.Length - 1].Replace(second.ToString(), num1.ToString());
            value = value.Replace(match.Value, newValue1);
            continue;
          default:
            continue;
        }
      }
    }
    return value;
  }

  private IRange[] ConvertCellListIntoRange(List<long> arrIndexes)
  {
    if (arrIndexes == null || arrIndexes.Count == 0)
      return (IRange[]) null;
    int count = arrIndexes.Count;
    IRange[] rangeArray = new IRange[count];
    for (int index = 0; index < count; ++index)
    {
      long arrIndex = arrIndexes[index];
      int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(arrIndex);
      int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(arrIndex);
      rangeArray[index] = this[rowFromCellIndex, columnFromCellIndex];
    }
    return rangeArray;
  }

  private IRange FindValueForNumber(
    BiffRecordRaw record,
    double findValue,
    bool bIsNumber,
    bool bIsFormulaValue)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    double num = double.MinValue;
    ICellPositionFormat cellPositionFormat = (ICellPositionFormat) record;
    if (bIsNumber)
    {
      if (record is NumberRecord)
        num = ((NumberRecord) record).Value;
      if (record is RKRecord)
        num = ((RKRecord) record).RKNumber;
    }
    if (bIsFormulaValue && record is FormulaRecord)
      num = ((FormulaRecord) record).Value;
    return num != findValue ? (IRange) null : this.Range[cellPositionFormat.Row + 1, cellPositionFormat.Column + 1];
  }

  private IRange FindValueForByteOrError(BoolErrRecord boolError, byte findValue, bool bIsError)
  {
    if (boolError == null)
      throw new ArgumentNullException(nameof (boolError));
    return bIsError == boolError.IsErrorCode && (int) boolError.BoolOrError == (int) findValue ? this.Range[boolError.Row + 1, boolError.Column + 1] : (IRange) null;
  }

  protected internal IRange InnerGetCell(int column, int row)
  {
    return this.InnerGetCell(column, row, this.GetXFIndex(row, column));
  }

  protected internal IRange InnerGetCell(int column, int row, int iXFIndex)
  {
    this.ParseData();
    IRange cell = (IRange) this.m_dicRecordsCells.GetRange(row, column);
    if (cell == null)
    {
      if (!(this.m_dicRecordsCells.GetCellRecord(row, column) is BiffRecordRaw cellRecord))
      {
        RangeImpl range = this.AppImplementation.CreateRange((object) this, column, row, column, row);
        if ((int) range.ExtendedFormatIndex != iXFIndex)
          range.ExtendedFormatIndex = (ushort) iXFIndex;
        this.m_dicRecordsCells.SetRange(row, column, range);
        cell = (IRange) range;
      }
      else
        cell = this.ConvertRecordToRange(cellRecord);
    }
    return cell;
  }

  protected internal IStyle InnerGetCellStyle(
    int column,
    int row,
    int iXFIndex,
    RangeImpl rangeImpl)
  {
    return RangeImpl.CreateTempStyleWrapperWithoutRange(rangeImpl, iXFIndex);
  }

  private IRange ConvertRecordToRange(BiffRecordRaw record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    this.ParseData();
    RangeImpl range = this.AppImplementation.CreateRange((object) this, record, false);
    this.m_dicRecordsCells.SetRange(range.CellIndex, range);
    return (IRange) range;
  }

  protected void UpdateFirstLast(int iRowIndex, int iColumnIndex)
  {
    this.ParseData();
    this.m_iFirstColumn = this.m_iFirstColumn > iColumnIndex || this.m_iFirstColumn == int.MaxValue ? (int) (ushort) iColumnIndex : this.m_iFirstColumn;
    this.m_iLastColumn = this.m_iLastColumn < iColumnIndex || this.m_iLastColumn == int.MaxValue ? (int) (ushort) iColumnIndex : this.m_iLastColumn;
    this.m_iFirstRow = this.m_iFirstRow > iRowIndex || this.m_iFirstRow < 0 ? iRowIndex : this.m_iFirstRow;
    this.m_iLastRow = this.m_iLastRow < iRowIndex || this.m_iLastRow < 0 ? iRowIndex : this.m_iLastRow;
  }

  protected internal void InnerSetCell(int column, int row, RangeImpl range)
  {
    if (!range.IsSingleCell)
      throw new ArgumentException("Range must represent single cell");
    this.ParseData();
    this.m_dicRecordsCells.SetRange(row, column, range);
  }

  [CLSCompliant(false)]
  protected internal void InnerSetCell(long cellIndex, BiffRecordRaw record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    this.ParseData();
    ICellPositionFormat cell = (ICellPositionFormat) record;
    WorksheetHelper.AccessColumn((IInternalWorksheet) this, cell.Column + 1);
    WorksheetHelper.AccessRow((IInternalWorksheet) this, cell.Row + 1);
    this.m_dicRecordsCells.SetCellRecord(cellIndex, cell);
  }

  [CLSCompliant(false)]
  protected internal void InnerSetCell(int iColumn, int iRow, BiffRecordRaw record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    this.ParseData();
    this.m_dicRecordsCells.SetCellRecord(iRow, iColumn, record as ICellPositionFormat);
  }

  protected internal void InnerGetDimensions(
    out int left,
    out int top,
    out int right,
    out int bottom)
  {
    this.ParseData();
    left = this.m_iFirstColumn;
    right = this.m_iLastColumn;
    top = this.m_iFirstRow;
    bottom = this.m_iLastRow;
  }

  protected internal void InnerGetColumnDimensions(int column, out int top, out int bottom)
  {
    this.ParseData();
    int num1 = -1;
    int num2 = -1;
    int firstRow = this.FirstRow;
    for (int lastRow = this.LastRow; firstRow <= lastRow; ++firstRow)
    {
      if (this.m_dicRecordsCells.Contains(RangeImpl.GetCellIndex(column, firstRow)))
      {
        if (num1 < firstRow)
          num1 = firstRow;
        if (num2 == -1)
          num2 = firstRow;
      }
    }
    top = num2;
    bottom = num1;
  }

  internal void UpdateLabelSSTIndexes(Dictionary<int, int> dictUpdatedIndexes, IncreaseIndex method)
  {
    this.ParseData();
    this.m_dicRecordsCells.UpdateLabelSSTIndexes(dictUpdatedIndexes, method);
  }

  private void InsertIntoDefaultColumns(
    int iColumnIndex,
    int iColumnCount,
    ExcelInsertOptions insertOptions)
  {
    this.ParseData();
    for (int maxColumnCount = this.m_book.MaxColumnCount; maxColumnCount > iColumnIndex + iColumnCount - 1; --maxColumnCount)
    {
      int index = maxColumnCount - iColumnCount;
      ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[index];
      if (columnInfoRecord != null)
        columnInfoRecord.FirstColumn = columnInfoRecord.LastColumn = (ushort) (maxColumnCount - 1);
      this.m_arrColumnInfo[maxColumnCount] = columnInfoRecord;
      this.m_arrColumnInfo[index] = (ColumnInfoRecord) null;
    }
    ColumnInfoRecord columnInfoRecord1 = (ColumnInfoRecord) null;
    switch (insertOptions)
    {
      case ExcelInsertOptions.FormatAsBefore:
        columnInfoRecord1 = this.m_arrColumnInfo[iColumnIndex - 1];
        break;
      case ExcelInsertOptions.FormatAsAfter:
        columnInfoRecord1 = this.m_arrColumnInfo[iColumnIndex + iColumnCount];
        break;
    }
    if (columnInfoRecord1 == null)
      return;
    int index1 = iColumnIndex;
    for (int index2 = iColumnIndex + iColumnCount; index1 < index2; ++index1)
    {
      columnInfoRecord1 = (ColumnInfoRecord) columnInfoRecord1.Clone();
      columnInfoRecord1.FirstColumn = columnInfoRecord1.LastColumn = (ushort) (index1 - 1);
      this.m_arrColumnInfo[index1] = columnInfoRecord1;
    }
  }

  private void RemoveFromDefaultColumns(
    int iColumnIndex,
    int iColumnCount,
    ExcelInsertOptions insertOptions)
  {
    this.ParseData();
    for (int index = iColumnIndex; index <= this.m_book.MaxColumnCount - iColumnCount; ++index)
    {
      ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[index + iColumnCount];
      if (columnInfoRecord != null)
        columnInfoRecord.FirstColumn = columnInfoRecord.LastColumn = (ushort) (index - 1);
      this.m_arrColumnInfo[index] = columnInfoRecord;
    }
    int maxColumnCount = this.m_book.MaxColumnCount;
    ColumnInfoRecord columnInfoRecord1 = (ColumnInfoRecord) CloneUtils.CloneCloneable((ICloneable) this.m_arrColumnInfo[maxColumnCount - 1]);
    this.m_arrColumnInfo[maxColumnCount] = columnInfoRecord1;
    if (columnInfoRecord1 == null)
      return;
    columnInfoRecord1.FirstColumn = columnInfoRecord1.LastColumn = (ushort) (maxColumnCount - 1);
  }

  private void GetRangeCoordinates(
    ref int firstRow,
    ref int firstColumn,
    ref int lastRow,
    ref int lastColumn,
    bool isUsedRange)
  {
    if (!isUsedRange || this.m_bUsedRangeIncludesFormatting || firstRow == 1 && firstColumn == 1 && lastRow == 1 && lastColumn == 1 && !this.m_book.IsCreated && this.m_book.Version == ExcelVersion.Excel97to2003 && this.IsRowBlankOnly(firstRow) && this.IsColumnBlankOnly(firstColumn))
      return;
    while (firstRow <= lastRow && this.IsRowBlankOnly(firstRow))
      ++firstRow;
    while (lastRow >= firstRow && this.IsRowBlankOnly(lastRow))
      --lastRow;
    while (firstColumn <= lastColumn && this.IsColumnBlankOnly(firstColumn))
      ++firstColumn;
    while (lastColumn >= firstColumn && this.IsColumnBlankOnly(lastColumn))
      --lastColumn;
    if (firstRow <= lastRow || firstColumn <= lastColumn)
      return;
    firstRow = lastRow = firstColumn = lastColumn = 0;
  }

  private bool IsRowBlankOnly(int rowIndex)
  {
    bool flag = true;
    for (int iFirstColumn = this.m_iFirstColumn; iFirstColumn <= this.m_iLastColumn; ++iFirstColumn)
    {
      if (this.GetCellType(rowIndex, iFirstColumn, false) != WorksheetImpl.TRangeValueType.Blank)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  private bool IsColumnBlankOnly(int columnIndex)
  {
    bool flag = true;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
    {
      if (this.GetCellType(iFirstRow, columnIndex, false) != WorksheetImpl.TRangeValueType.Blank)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  private void CreateUsedRange(int firstRow, int firstColumn, int lastRow, int lastColumn)
  {
    if (this.m_rngUsed != null && this.m_rngUsed.FirstColumn == firstColumn && this.m_rngUsed.FirstRow == firstRow && this.m_rngUsed.LastColumn == lastColumn && this.m_rngUsed.LastRow == lastRow || firstRow == 0 && firstColumn == 0 && lastColumn == 0 && lastRow == 0)
    {
      this.m_rngUsed.ResetCells();
    }
    else
    {
      if (this.m_rngUsed != null)
        this.m_rngUsed.Dispose();
      this.m_rngUsed = this.AppImplementation.CreateRange((object) this, firstColumn, firstRow, lastColumn, lastRow);
    }
  }

  protected override ExcelSheetProtection PrepareProtectionOptions(ExcelSheetProtection options)
  {
    return options &= ~ExcelSheetProtection.Content;
  }

  internal void ShowFilteredRows(
    int rowIndex,
    int columnIndex,
    bool isVisible,
    bool isAnd,
    bool isFirstCondition)
  {
    if (rowIndex < 1 || rowIndex > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (rowIndex));
    RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, rowIndex - 1, true);
    List<int> columnFilterHideRow = row.ColumnFilterHideRow;
    if (isVisible)
    {
      if (row.m_isFilteredRow)
      {
        if (!columnFilterHideRow.Contains(columnIndex) || isAnd)
          return;
        columnFilterHideRow.Remove(columnIndex);
        if (columnFilterHideRow.Count > 0)
          return;
      }
    }
    else if (!isFirstCondition && !columnFilterHideRow.Contains(columnIndex) && !isAnd)
    {
      columnFilterHideRow.Remove(columnIndex);
      isVisible = true;
    }
    row.m_isFilteredRow = !isVisible;
    if (!this.IsParsing)
      row.IsHidden = !isVisible;
    if (row.m_isFilteredRow && !columnFilterHideRow.Contains(columnIndex))
      row.ColumnFilterHideRow.Add(columnIndex);
    this.UpdateShapes();
  }

  private bool CheckItRefersToSingleCell(int iRow, int iColumn)
  {
    bool singleCell = false;
    if (this.CellRecords[iRow, iColumn] is FormulaRecord cellRecord && cellRecord.ParsedExpression.Length == 1 && (cellRecord.ParsedExpression[0].TokenCode == FormulaToken.tRef1 || cellRecord.ParsedExpression[0].TokenCode == FormulaToken.tRef2 || cellRecord.ParsedExpression[0].TokenCode == FormulaToken.tRef3))
      singleCell = true;
    return singleCell;
  }

  [CLSCompliant(false)]
  protected internal void Parse(BiffReader reader, IDecryptor decryptor)
  {
    this.Parse(reader, ExcelParseOptions.Default, false, (Dictionary<int, int>) null, decryptor);
  }

  protected override void PrepareVariables(ExcelParseOptions options, bool bSkipParsing)
  {
    base.PrepareVariables(options, bSkipParsing);
    if (this.m_arrAutoFilter != null)
      this.m_arrAutoFilter.Clear();
    if (this.m_arrDConRecords != null)
      this.m_arrDConRecords.Clear();
    this.m_iDValPos = -1;
    this.m_iCondFmtPos = -1;
    this.m_iPivotStartIndex = -1;
    this.m_iHyperlinksStartIndex = -1;
  }

  [CLSCompliant(false)]
  protected override void ParseRecord(
    BiffRecordRaw raw,
    bool bIgnoreStyles,
    Dictionary<int, int> hashNewXFormatIndexes)
  {
    if (this.m_book.HasDuplicatedNames && raw.TypeCode == TBIFFRecord.Formula)
      this.UpdateDuplicatedNameIndexes((FormulaRecord) raw);
    if (this.IsSkipParsing)
      return;
    if (UtilityMethods.IndexOf(WorksheetImpl.s_arrAutofilterRecord, raw.TypeCode) >= 0)
      this.AutoFilterRecords.Add(raw);
    if (raw is ICellPositionFormat cellPositionFormat && bIgnoreStyles)
      cellPositionFormat.ExtendedFormatIndex = (ushort) this.GetNewXFormatIndex((int) cellPositionFormat.ExtendedFormatIndex, hashNewXFormatIndexes);
    switch (raw.TypeCode)
    {
      case TBIFFRecord.Note:
        this.AddNote(raw as NoteRecord);
        break;
      case TBIFFRecord.Selection:
        this.m_arrSelections.Add((SelectionRecord) raw);
        break;
      case TBIFFRecord.Pane:
        this.m_pane = (PaneRecord) raw;
        break;
      case TBIFFRecord.DCON:
        this.DConRecords.Add(raw);
        break;
      case TBIFFRecord.DefaultColWidth:
        this.ParseDefaultColWidth(raw);
        break;
      case TBIFFRecord.ColumnInfo:
        this.ParseColumnInfo((ColumnInfoRecord) raw, bIgnoreStyles);
        break;
      case TBIFFRecord.Sort:
        this.SortRecords.Add(raw);
        break;
      case TBIFFRecord.PivotViewDefinition:
        if (!this.KeepRecord)
        {
          this.KeepRecord = true;
          this.m_arrRecords.Add(raw);
        }
        if (this.m_iPivotStartIndex >= 0)
          break;
        this.m_iPivotStartIndex = this.m_arrRecords.Count - 1;
        break;
      case TBIFFRecord.PivotString:
      case TBIFFRecord.ExternalSourceInfo:
      case TBIFFRecord.Qsi:
      case TBIFFRecord.QsiSXTag:
      case TBIFFRecord.DBQueryExt:
      case TBIFFRecord.ExtString:
      case TBIFFRecord.TextQuery:
      case TBIFFRecord.Qsir:
      case TBIFFRecord.Qsif:
      case TBIFFRecord.OleDbConn:
      case TBIFFRecord.PivotViewAdditionalInfo:
        this.PreserveExternalConnection.Add(raw);
        break;
      case TBIFFRecord.MergeCells:
        this.MergeCells.AddMerge((MergeCellsRecord) raw);
        break;
      case TBIFFRecord.CondFMT:
        if (!this.KeepRecord)
        {
          this.KeepRecord = true;
          this.m_arrRecords.Add(raw);
        }
        if (this.m_iCondFmtPos >= 0)
          break;
        this.m_iCondFmtPos = this.m_arrRecords.Count - 1;
        break;
      case TBIFFRecord.DVal:
        if (!this.KeepRecord)
        {
          this.KeepRecord = true;
          this.m_arrRecords.Add(raw);
        }
        ((DValRecord) raw).IsDataCached = false;
        if (this.m_iDValPos >= 0)
          break;
        this.m_iDValPos = this.m_arrRecords.Count - 1;
        break;
      case TBIFFRecord.HLink:
        if (!this.KeepRecord)
        {
          this.KeepRecord = true;
          this.m_arrRecords.Add(raw);
        }
        if (this.m_iHyperlinksStartIndex >= 0)
          break;
        this.m_iHyperlinksStartIndex = this.m_arrRecords.Count - 1;
        break;
      case TBIFFRecord.Row:
        this.ParseRowRecord((RowRecord) raw, bIgnoreStyles);
        break;
      case TBIFFRecord.Index:
        this.m_index = (IndexRecord) raw;
        break;
      case TBIFFRecord.CustomProperty:
        if (!this.KeepRecord)
        {
          this.KeepRecord = true;
          this.m_arrRecords.Add(raw);
        }
        if (this.m_iCustomPropertyStartIndex >= 0)
          break;
        this.m_iCustomPropertyStartIndex = this.m_arrRecords.Count - 1;
        break;
      case TBIFFRecord.ContinueFrt:
      case (TBIFFRecord) 2161:
      case (TBIFFRecord) 2162:
      case (TBIFFRecord) 2167:
      case TBIFFRecord.Feature12:
        if (this.m_tableRecords == null)
          this.m_tableRecords = new List<BiffRecordRaw>();
        this.m_tableRecords.Add(raw);
        break;
      case TBIFFRecord.RangeProtection:
        this.ParseErrorIndicators((RangeProtectionRecord) raw);
        break;
      case TBIFFRecord.CondFMT12:
        if (!this.KeepRecord)
        {
          this.KeepRecord = true;
          this.m_arrRecords.Add(raw);
        }
        if (this.m_iCondFmtPos >= 0)
          break;
        this.m_iCondFmtPos = this.m_arrRecords.Count - 1;
        break;
    }
  }

  private void ParseErrorIndicators(RangeProtectionRecord record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    if (record.ErrorIndicator == null)
      return;
    if (this.m_errorIndicators == null)
      this.m_errorIndicators = new ErrorIndicatorsCollection(this.Application, (object) this);
    this.m_errorIndicators.Add(record.ErrorIndicator);
  }

  private void UpdateDuplicatedNameIndexes(FormulaRecord formula)
  {
    Ptg[] ptgArray = formula != null ? formula.ParsedExpression : throw new ArgumentNullException(nameof (formula));
    int index = 0;
    for (int length = ptgArray.Length; index < length; ++index)
    {
      Ptg ptg = ptgArray[index];
      if (FormulaUtil.IndexOf(FormulaUtil.NameXCodes, ptg.TokenCode) != -1)
      {
        NameXPtg nameXptg = (NameXPtg) ptg;
        int refIndex = (int) nameXptg.RefIndex;
        int nameIndex = (int) nameXptg.NameIndex;
        if (!this.m_book.IsLocalReference(refIndex))
        {
          ExternWorkbookImpl externWorkbook = this.m_book.ExternWorkbooks[refIndex];
          nameXptg.NameIndex = (ushort) (externWorkbook.GetNewIndex(nameIndex - 1) + 1);
        }
      }
    }
  }

  private int GetNewXFormatIndex(int iXFIndex, Dictionary<int, int> hashNewXFormatIndexes)
  {
    if (hashNewXFormatIndexes == null)
      throw new ArgumentNullException(nameof (hashNewXFormatIndexes));
    int formatIndex = (int) this.m_book.InnerExtFormatRecords[iXFIndex].FormatIndex;
    return hashNewXFormatIndexes[formatIndex];
  }

  public void Parse(TextReader streamToRead, string separator, int row, int column, bool isValid)
  {
    if (streamToRead == null)
      throw new ArgumentNullException(nameof (streamToRead));
    switch (separator)
    {
      case null:
        throw new ArgumentNullException(nameof (separator));
      case "":
        throw new ArgumentException(nameof (separator));
      default:
        int iRow = row;
        StringBuilder builder = new StringBuilder();
        int iColumn = column;
        this.CustomHeight = false;
        while (streamToRead.Peek() >= 0 && iRow <= this.m_book.MaxRowCount && iColumn <= this.m_book.MaxColumnCount)
        {
          string strRowString = this.ReadCellValue(streamToRead, separator, builder, isValid);
          bool flag = strRowString.EndsWith('\n'.ToString()) || strRowString.EndsWith(this.Application.CsvRecordDelimiter) || strRowString.EndsWith('\r'.ToString());
          if (flag)
            strRowString = strRowString.Remove(strRowString.Length - 1);
          if (strRowString.Length > 0)
          {
            IMigrantRange range = (IMigrantRange) new MigrantRangeImpl(this.Application, (IWorksheet) this);
            range.ResetRowColumn(iRow, iColumn);
            this.ParseRange(range, strRowString, separator, 0);
          }
          if (flag)
          {
            ++iRow;
            iColumn = column;
            if (iRow > this.m_book.MaxRowCount && this.m_book.Version == ExcelVersion.Excel97to2003)
              this.m_book.Version = ExcelVersion.Excel2007;
          }
          else
          {
            ++iColumn;
            if (iColumn > this.m_book.MaxColumnCount && this.m_book.Version == ExcelVersion.Excel97to2003)
              this.m_book.Version = ExcelVersion.Excel2007;
          }
        }
        break;
    }
  }

  private string ReadCellValue(
    TextReader reader,
    string separator,
    StringBuilder builder,
    bool isValid)
  {
    builder.Length = 0;
    bool flag1 = true;
    bool flag2 = false;
    string csvQualifier = this.Application.CsvQualifier;
    do
    {
      char ch;
      do
      {
        int num1;
        do
        {
          num1 = reader.Read();
          if (num1 < 0)
            goto label_18;
        }
        while (num1 == 0);
        ch = (char) num1;
        if ((int) ch == (int) csvQualifier[0])
        {
          if (builder.Length == 0)
          {
            flag1 = false;
            flag2 = true;
          }
          else
          {
            flag1 = true;
            if (flag2)
            {
              if (reader.Peek() == (int) csvQualifier[0])
              {
                int num2 = 0;
                while (reader.Peek() == (int) csvQualifier[0])
                {
                  builder.Append((char) reader.Read());
                  ++num2;
                }
                if (num2 % 2 == 0)
                  flag2 = false;
              }
              else
                flag2 = false;
            }
          }
        }
      }
      while (ch == '\r' && reader.Peek() == 10);
      if ((ch == '\n' || ch == '\r') && !flag2)
      {
        builder.Append(ch);
        break;
      }
      builder.Append(ch);
    }
    while (!flag1 || !WorksheetImpl.EndsWith(builder, separator) || flag2);
label_18:
    return builder.ToString();
  }

  private static bool EndsWith(StringBuilder builder, string separator)
  {
    if (string.IsNullOrEmpty(separator))
      throw new ArgumentException(nameof (separator));
    int length1 = builder.Length;
    int length2 = separator.Length;
    bool flag = false;
    if (length1 >= length2)
    {
      flag = true;
      int index1 = length1 - 1;
      for (int index2 = length2 - 1; index2 >= 0; --index2)
      {
        if ((int) builder[index1] != (int) separator[index2])
        {
          flag = false;
          break;
        }
        --index1;
      }
    }
    return flag;
  }

  private static void ReadToChar(
    TextReader reader,
    char endChar,
    StringBuilder builder,
    string separator,
    bool isValid)
  {
    if (isValid)
      WorksheetImpl.ReadToChar(reader, endChar, builder);
    else
      WorksheetImpl.RemoveJunkChar(reader, endChar, builder, separator);
  }

  private static void RemoveJunkChar(
    TextReader reader,
    char endChar,
    StringBuilder builder,
    string separator)
  {
    char ch1 = ' ';
    bool flag1 = true;
    bool flag2 = true;
    int num;
    do
    {
      char ch2 = ch1;
      num = reader.Read();
      ch1 = (char) num;
      if ((int) ch1 == (int) endChar)
      {
        flag2 = !flag2 || ch2 == '�';
        char ch3 = (char) reader.Peek();
        if (((int) ch3 == (int) Convert.ToChar(separator) || ch3 == '\r' || ch3 == '\n') && !flag2)
        {
          flag1 = false;
          builder.Append(ch1);
        }
        else if (ch2 != '�')
          builder.Append(ch1);
      }
      else if (num > 0)
        builder.Append(ch1);
    }
    while (flag1 && num > 0);
  }

  private static void ReadToChar(TextReader reader, char endChar, StringBuilder builder)
  {
    int num;
    char ch;
    do
    {
      num = reader.Read();
      ch = (char) num;
      builder.Append(ch);
    }
    while ((int) ch != (int) endChar && num > 0 && ch != '\r');
  }

  private static int CharCount(string value, char ch)
  {
    int num = 0;
    for (int index = value.Length - 1; index >= 0; --index)
    {
      if ((int) value[index] == (int) ch)
        ++num;
    }
    return num;
  }

  internal override void ParseBinaryData(
    Dictionary<int, int> dictUpdatedSSTIndexes,
    XlsbDataHolder holder)
  {
    this.m_dataHolder.ParseBinaryWorksheetData(this, dictUpdatedSSTIndexes, this.ParseDataOnDemand, holder);
  }

  protected internal override void ParseData(Dictionary<int, int> dictUpdatedSSTIndexes)
  {
    if ((this.IsParsed || this.IsParsing) && !this.ParseDataOnDemand)
      return;
    this.IsParsing = true;
    bool loading = this.m_book.Loading;
    this.m_book.Loading = true;
    if (this.m_dataHolder == null)
    {
      if (!this.IsSkipParsing)
      {
        if (this.ParseOnDemand)
        {
          Stream stream = (Stream) new MemoryStream();
          BinaryWriter binaryWriter = new BinaryWriter(stream);
          foreach (BiffRecordRaw arrRecord in this.m_arrRecords)
          {
            int recordCode = arrRecord.RecordCode;
            int count = 0;
            byte[] data = arrRecord.Data;
            if (data != null)
              count = data.Length;
            binaryWriter.Write((short) recordCode);
            binaryWriter.Write((short) count);
            if (data != null)
              binaryWriter.Write(data, 0, count);
          }
          binaryWriter.Flush();
          if (stream.Length > 0L)
          {
            this.m_arrRecords.Clear();
            this.ParseOnDemand = false;
            this.m_book.Loading = true;
            stream.Position = 0L;
            this.Parse(new BiffReader(stream), ExcelParseOptions.Default, false, (Dictionary<int, int>) null, (IDecryptor) null);
            this.Parse();
            this.m_book.ParseWorksheetsOnDemand();
            this.m_book.Loading = false;
          }
        }
        int calculationOptions = this.ExtractCalculationOptions();
        this.ReplaceSharedFormula();
        this.ExtractPageSetup(calculationOptions);
        this.ExtractPivotTables(this.m_iPivotStartIndex);
        this.ExtractHyperLinks(this.m_iHyperlinksStartIndex);
        if (this.m_iCondFmtPos >= 0)
          this.ExtractConditionalFormats(this.m_iCondFmtPos);
        if (this.m_iDValPos >= 0)
          this.ExtractDataValidation(this.m_iDValPos);
        if (this.m_iCustomPropertyStartIndex >= 0)
          this.ExtractCustomProperties(this.m_iCustomPropertyStartIndex);
      }
    }
    else
    {
      this.AttachEvents();
      if (this.AppImplementation.IsFormulaParsed)
        this.AppImplementation.IsFormulaParsed = false;
      this.m_dataHolder.ParseWorksheetData(this, dictUpdatedSSTIndexes, this.ParseDataOnDemand);
      this.AppImplementation.IsFormulaParsed = true;
    }
    if (!this.IsParsed)
      this.IsSaved = true;
    this.IsParsed = true;
    this.IsParsing = false;
    this.m_book.Loading = loading;
  }

  private void ReplaceSharedFormula() => this.m_dicRecordsCells.ReplaceSharedFormula();

  internal void ParseColumnInfo(ColumnInfoRecord columnInfo, bool bIgnoreStyles)
  {
    if (columnInfo == null)
      throw new ArgumentNullException(nameof (columnInfo));
    if (bIgnoreStyles)
      columnInfo.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
    columnInfo.ColumnWidth = (ushort) this.EvaluateRealColumnWidth((int) columnInfo.ColumnWidth);
    int extendedFormatIndex = (int) columnInfo.ExtendedFormatIndex;
    if ((int) columnInfo.FirstColumn != (int) columnInfo.LastColumn)
    {
      if ((int) columnInfo.LastColumn == this.m_book.MaxColumnCount)
        this.m_rawColRecord = columnInfo.Clone() as ColumnInfoRecord;
      for (int firstColumn = (int) columnInfo.FirstColumn; firstColumn <= (int) columnInfo.LastColumn; ++firstColumn)
      {
        int index = firstColumn + 1;
        ColumnInfoRecord columnInfoRecord = (ColumnInfoRecord) columnInfo.Clone();
        columnInfoRecord.FirstColumn = (ushort) firstColumn;
        columnInfoRecord.LastColumn = (ushort) firstColumn;
        this.m_arrColumnInfo[index] = columnInfoRecord;
      }
    }
    else
      this.m_arrColumnInfo[(int) columnInfo.FirstColumn + 1] = columnInfo;
  }

  internal void ParseRowRecord(RowRecord row, bool bIgnoreStyles)
  {
    if (row == null)
      throw new ArgumentNullException(nameof (row));
    if (bIgnoreStyles)
    {
      row.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
      row.IsFormatted = false;
    }
    if (!row.IsBadFontHeight && (double) row.Height > 8190.0)
      row.Height = (ushort) this.DefaultRowHeight;
    RowStorage row1 = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, (int) row.RowNumber, true);
    row1.UpdateRowInfo(row, this.AppImplementation.UseFastRecordParsing);
    int num = (int) row.RowNumber + 1;
    if (num < this.FirstRow)
      this.FirstRow = num;
    if (num > this.LastRow)
      this.LastRow = num;
    if (this.FirstColumn == int.MaxValue)
      this.FirstColumn = 0;
    if (this.LastColumn == int.MaxValue)
      this.LastColumn = 1;
    int extendedFormatIndex = (int) row1.ExtendedFormatIndex;
    if (extendedFormatIndex >= this.m_book.InnerExtFormats.Count)
    {
      row1.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
    }
    else
    {
      ExtendedFormatImpl innerExtFormat = this.m_book.InnerExtFormats[extendedFormatIndex];
      if (innerExtFormat.HasParent)
        return;
      ExtendedFormatImpl format = (ExtendedFormatImpl) innerExtFormat.Clone();
      format.ParentIndex = extendedFormatIndex;
      format.Record.XFType = ExtendedFormatRecord.TXFType.XF_STYLE;
      ExtendedFormatImpl extendedFormatImpl = this.m_book.InnerExtFormats.Add(format);
      row1.ExtendedFormatIndex = (ushort) extendedFormatImpl.Index;
    }
  }

  internal void ParseDefaultColWidth(BiffRecordRaw raw)
  {
    DefaultColWidthRecord defaultColWidthRecord = (DefaultColWidthRecord) raw;
    if (defaultColWidthRecord.Width == (ushort) 8)
      return;
    this.m_dStandardColWidth = this.m_book.WidthToFileWidth((double) defaultColWidthRecord.Width);
  }

  protected void ExtractHyperLinks(int iLinkIndex)
  {
    if (iLinkIndex < 0)
      return;
    this.InnerHyperLinks.Clear();
    this.InnerHyperLinks.Parse((IList) this.m_arrRecords, iLinkIndex);
  }

  protected int ExtractCalculationOptions()
  {
    int num = 0;
    for (int count = this.m_arrRecords.Count; num < count; ++num)
    {
      BiffRecordRaw arrRecord = this.m_arrRecords[num];
      if (Array.IndexOf<TBIFFRecord>(CalculationOptionsImpl.DEF_CORRECT_CODES, arrRecord.TypeCode) != -1)
        return this.m_book.InnerCalculation.Parse((IList) this.m_arrRecords, num);
    }
    return 0;
  }

  protected void ExtractPageSetup(int iStartIndex)
  {
    int num = iStartIndex >= 0 ? iStartIndex : throw new ArgumentOutOfRangeException(nameof (iStartIndex));
    for (int count = this.m_arrRecords.Count; num < count; ++num)
    {
      switch (this.m_arrRecords[num].TypeCode)
      {
        case TBIFFRecord.VerticalPageBreaks:
        case TBIFFRecord.HorizontalPageBreaks:
        case TBIFFRecord.PrintHeaders:
        case TBIFFRecord.DefaultRowHeight:
          this.m_pageSetup = new PageSetupImpl(this.Application, (object) this, this.m_arrRecords, num);
          num = this.m_arrRecords.Count;
          break;
      }
    }
  }

  protected void ExtractConditionalFormats(int iCondFmtPos)
  {
    if (iCondFmtPos < 0)
      throw new ArgumentOutOfRangeException(nameof (iCondFmtPos));
    bool flag = true;
    int num = 0;
    CondFMTRecord format1 = (CondFMTRecord) null;
    List<CFRecord> lstConditions = new List<CFRecord>();
    CondFmt12Record format2 = (CondFmt12Record) null;
    List<CF12Record> conditions = new List<CF12Record>();
    List<CFExRecord> CFExRecords = new List<CFExRecord>();
    while (flag)
    {
      BiffRecordRaw arrRecord = this.m_arrRecords[iCondFmtPos];
      switch (arrRecord.TypeCode)
      {
        case TBIFFRecord.CondFMT:
          if (format1 != null)
          {
            this.CreateFormatsCollection(format1, (IList) lstConditions, (IList) CFExRecords, false);
            lstConditions.Clear();
          }
          if (format2 != null)
          {
            this.CreateCF12RecordCollection(format2, (IList) conditions);
            conditions.Clear();
            format2 = (CondFmt12Record) null;
          }
          format1 = (CondFMTRecord) arrRecord;
          ++num;
          if (format1.Index == (ushort) 0)
            format1.Index = (ushort) num;
          if (!this.m_dictCondFMT.ContainsKey((int) format1.Index))
          {
            this.m_dictCondFMT.Add((int) format1.Index, format1);
            break;
          }
          ++num;
          format1.Index = (ushort) num;
          this.m_dictCondFMT.Add((int) format1.Index, format1);
          break;
        case TBIFFRecord.CF:
          lstConditions.Add((CFRecord) arrRecord);
          break;
        case TBIFFRecord.CondFMT12:
          if (format2 != null)
          {
            this.CreateCF12RecordCollection(format2, (IList) conditions);
            conditions.Clear();
          }
          if (format1 != null)
          {
            this.CreateFormatsCollection(format1, (IList) lstConditions, (IList) CFExRecords, false);
            lstConditions.Clear();
            format1 = (CondFMTRecord) null;
          }
          format2 = (CondFmt12Record) arrRecord;
          break;
        case TBIFFRecord.CF12:
          if (this.m_dictCFExRecords.Count > 0)
          {
            CFExRecord dictCfExRecord = this.m_dictCFExRecords[this.m_dictCFExRecords.Count - 1];
            if (dictCfExRecord.IsCF12Extends == (byte) 1)
            {
              dictCfExRecord.CF12RecordIfExtends = (CF12Record) arrRecord;
              break;
            }
            conditions.Add((CF12Record) arrRecord);
            break;
          }
          conditions.Add((CF12Record) arrRecord);
          break;
        case TBIFFRecord.CFEx:
          if (format1 != null)
          {
            this.CreateFormatsCollection(format1, (IList) lstConditions, (IList) CFExRecords, false);
            lstConditions.Clear();
            format1 = (CondFMTRecord) null;
          }
          if (format2 != null)
          {
            this.CreateCF12RecordCollection(format2, (IList) conditions);
            conditions.Clear();
            format2 = (CondFmt12Record) null;
          }
          CFExRecord extRecord = (CFExRecord) arrRecord;
          for (int i = 0; i < this.m_arrConditionalFormats.Count; ++i)
          {
            if (this.m_arrConditionalFormats[i].CondFMTRecord != null && (int) this.m_arrConditionalFormats[i].CondFMTRecord.Index == (int) extRecord.CondFmtIndex)
            {
              this.m_dictCFExRecords.Add(this.m_dictCFExRecords.Count, extRecord);
              this.m_arrConditionalFormats.UpdateCFExProperties(this.m_arrConditionalFormats[i], extRecord);
            }
          }
          break;
        default:
          flag = false;
          break;
      }
      ++iCondFmtPos;
    }
    if (format1 != null)
    {
      this.CreateFormatsCollection(format1, (IList) lstConditions, (IList) CFExRecords, false);
      lstConditions.Clear();
    }
    if (format2 != null)
    {
      this.CreateCF12RecordCollection(format2, (IList) conditions);
      conditions.Clear();
    }
    this.m_dictCondFMT.Clear();
    this.m_dictCFExRecords.Clear();
  }

  protected void ExtractDataValidation(int iDValPos)
  {
    if (iDValPos < 0)
      throw new ArgumentOutOfRangeException(nameof (iDValPos));
    this.m_dataValidation = new DataValidationTable(this.Application, (object) this, this.m_arrRecords, ref iDValPos);
    if (this.m_dataValidation == null)
      return;
    for (int index = 0; index < this.m_dataValidation.Count; ++index)
    {
      int iOffset = 0;
      DataValidationCollection validationCollection = this.m_dataValidation[index];
      validationCollection?.UpdateRecords(validationCollection.DataValidations, ref iOffset, validationCollection.DataValidations.Count);
    }
  }

  protected void ExtractCustomProperties(int iCustomPropertyPos)
  {
    this.m_arrCustomProperties = iCustomPropertyPos >= 0 ? new WorksheetCustomProperties((IList) this.m_arrRecords, iCustomPropertyPos) : throw new ArgumentOutOfRangeException(nameof (iCustomPropertyPos));
  }

  private void CreateFormatsCollection(
    CondFMTRecord format,
    IList lstConditions,
    IList CFExRecords,
    bool isFutureRecord)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (lstConditions == null && CFExRecords == null)
      throw new ArgumentNullException("Conditions");
    Syncfusion.XlsIO.Implementation.Collections.ConditionalFormats conditionalFormats = this.AppImplementation.CreateConditionalFormats((object) this, format, lstConditions, CFExRecords);
    conditionalFormats.IsFutureRecord = isFutureRecord;
    this.m_arrConditionalFormats.Add(conditionalFormats);
  }

  private void CreateCF12RecordCollection(CondFmt12Record format, IList conditions)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (conditions == null)
      throw new ArgumentNullException(nameof (conditions));
    this.m_arrConditionalFormats.Add(this.AppImplementation.CreateConditionalFormats((object) this, format, conditions));
  }

  public double InnerGetColumnWidth(int iColumn)
  {
    if (iColumn < 1)
      throw new ArgumentOutOfRangeException("iColumn can't be less then 1");
    this.ParseData();
    ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[iColumn];
    return columnInfoRecord != null ? (columnInfoRecord.IsHidden ? 0.0 : (double) columnInfoRecord.ColumnWidth / 256.0) : this.StandardWidth;
  }

  public int ColumnWidthToPixels(double widthInChars)
  {
    return (int) this.m_book.FileWidthToPixels(this.m_book.WidthToFileWidth(widthInChars));
  }

  public double PixelsToColumnWidth(int pixels) => this.m_book.PixelsToWidth((double) pixels);

  public void AdvancedFilter(
    ExcelFilterAction action,
    IRange filterRange,
    IRange criteriaRange,
    IRange copyToRange,
    bool isUnique)
  {
    int num1 = 0;
    WorksheetImpl worksheet1 = filterRange.Worksheet as WorksheetImpl;
    WorksheetImpl worksheet2 = criteriaRange.Worksheet as WorksheetImpl;
    WorksheetImpl.TRangeValueType type1 = WorksheetImpl.TRangeValueType.String;
    if (filterRange.LastRow - filterRange.Row < 1 || criteriaRange.LastRow - criteriaRange.Row < 1)
      return;
    if (copyToRange != null && action == ExcelFilterAction.FilterCopy)
    {
      if (this != copyToRange.Worksheet)
        return;
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      for (int column = filterRange.Column; column <= filterRange.LastColumn; ++column)
      {
        string str = worksheet1.GetTextFromCellType(filterRange.Row, column, out type1).ToString();
        if (str != string.Empty)
          stringList1.Add(str.ToLower());
      }
      for (int column = copyToRange.Column; column <= copyToRange.LastColumn; ++column)
      {
        string str = this.GetTextFromCellType(copyToRange.Row, column, out type1).ToString();
        if (!stringList1.Contains(str.ToLower()))
        {
          if (str == string.Empty)
            ++num1;
          stringList2.Add(str);
        }
      }
      if (num1 == copyToRange.Columns.Length)
        stringList2.Clear();
      else if (stringList2.Count != 0)
        return;
    }
    else if (action == ExcelFilterAction.FilterCopy)
      throw new ArgumentNullException(nameof (copyToRange));
    if (filterRange != null)
    {
      IName name = worksheet1.m_names.Add("_xlnm._FilterDatabase");
      name.Visible = false;
      name.RefersToRange = filterRange;
    }
    if (criteriaRange != null)
      worksheet2.m_names.Add("_xlnm.Criteria").RefersToRange = criteriaRange;
    if (copyToRange != null)
      this.m_names.Add("_xlnm.Extract").RefersToRange = copyToRange;
    List<Rectangle> rectangleList1 = new List<Rectangle>(filterRange.LastColumn - filterRange.Column + 1);
    List<Rectangle> rectangleList2 = new List<Rectangle>(criteriaRange.LastColumn - criteriaRange.Column + 1);
    List<int> intList1 = new List<int>(criteriaRange.LastColumn - criteriaRange.Column + 1);
    for (int column1 = criteriaRange.Column; column1 <= criteriaRange.LastColumn; ++column1)
    {
      bool flag = false;
      string str1 = worksheet2.GetTextFromCellType(criteriaRange.Row, column1, out type1).ToString();
      rectangleList2.Add(new Rectangle(column1, criteriaRange.Row + 1, 0, criteriaRange.LastRow - criteriaRange.Row - 1));
      for (int column2 = filterRange.Column; column2 <= filterRange.LastColumn; ++column2)
      {
        string str2 = worksheet1.GetTextFromCellType(filterRange.Row, column2, out type1).ToString();
        if (str2 != string.Empty && str2.Equals(str1, StringComparison.OrdinalIgnoreCase))
        {
          rectangleList1.Add(new Rectangle(column2, filterRange.Row + 1, 0, filterRange.LastRow - filterRange.Row - 1));
          flag = true;
          break;
        }
      }
      if (!flag && !intList1.Contains(column1))
        intList1.Add(column1);
    }
    long[,] numArray1 = new long[rectangleList2[0].Height + 1, rectangleList2.Count];
    List<int> intList2 = new List<int>();
    for (int index1 = 0; index1 < rectangleList2.Count; ++index1)
    {
      Rectangle rectangle = rectangleList2[index1];
      bool flag = false;
      int index2 = 0;
      for (int y = rectangle.Y; y <= rectangle.Bottom; ++y)
      {
        if (!intList1.Contains(rectangle.X))
          numArray1[index2, index1] = RangeImpl.GetCellIndex(rectangle.X, y);
        else if (worksheet2.GetCellType(y, rectangle.X, true) == (WorksheetImpl.TRangeValueType.Boolean | WorksheetImpl.TRangeValueType.Formula))
        {
          Ptg[] outputPtg = worksheet2.m_dicRecordsCells.Table.GetFormulaValue(y, rectangle.X);
          flag = this.TryGetValidFormula(outputPtg, filterRange, false, out outputPtg, 0);
          if (!flag)
            break;
        }
        ++index2;
      }
      if (flag)
      {
        int index3 = 0;
        for (int y = rectangle.Y; y <= rectangle.Bottom; ++y)
        {
          numArray1[index3, index1] = RangeImpl.GetCellIndex(rectangle.X, y);
          ++index3;
        }
      }
      else if (intList1.Contains(rectangle.X))
        intList1.Remove(rectangle.X);
    }
    if (intList1.Count == 0 && (rectangleList1.Count == 0 || rectangleList2.Count == 0))
    {
      if (copyToRange == null)
        return;
      for (int row = filterRange.Row; row <= filterRange.LastRow; ++row)
        intList2.Add(row);
    }
    else
    {
      long[,] numArray2 = (long[,]) null;
      if (rectangleList1.Count > 0)
      {
        numArray2 = new long[rectangleList1[0].Height + 1, rectangleList1.Count];
        for (int index4 = 0; index4 < rectangleList1.Count; ++index4)
        {
          Rectangle rectangle = rectangleList1[index4];
          int index5 = 0;
          for (int y = rectangle.Y; y <= rectangle.Bottom; ++y)
          {
            numArray2[index5, index4] = RangeImpl.GetCellIndex(rectangle.X, y);
            ++index5;
          }
        }
      }
      bool flag1 = false;
      if (intList1.Count > 0 && !(this.Workbook as WorkbookImpl).EnabledCalcEngine)
      {
        worksheet1.EnableSheetCalculations();
        worksheet1.CalcEngine.RethrowParseExceptions = false;
        flag1 = true;
      }
      List<int> collection1 = new List<int>();
      List<int> collection2 = new List<int>();
      for (int index6 = 0; index6 < numArray1.GetLength(0); ++index6)
      {
        int index7 = 0;
        int index8 = 0;
        for (; index7 < numArray1.GetLength(1); ++index7)
        {
          int rowFromCellIndex1 = RangeImpl.GetRowFromCellIndex(numArray1[index6, index7]);
          int columnFromCellIndex1 = RangeImpl.GetColumnFromCellIndex(numArray1[index6, index7]);
          if (intList1.Contains(columnFromCellIndex1))
          {
            Ptg[] formulaValue = worksheet2.m_dicRecordsCells.Table.GetFormulaValue(rowFromCellIndex1, columnFromCellIndex1);
            if (formulaValue == null)
            {
              for (int index9 = filterRange.Row + 1; index9 <= filterRange.LastRow; ++index9)
              {
                if (index7 == 0)
                  collection1.Add(index9);
                else if (collection2.Contains(index9))
                  collection1.Add(index9);
              }
            }
            else
            {
              for (int currentRow = filterRange.Row + 1; currentRow <= filterRange.LastRow; ++currentRow)
              {
                Ptg[] outputPtg;
                if (currentRow == filterRange.Row + 1)
                {
                  outputPtg = formulaValue;
                }
                else
                {
                  outputPtg = this.CloneArrayPtg(formulaValue);
                  this.TryGetValidFormula(outputPtg, filterRange, true, out outputPtg, currentRow);
                }
                string ptgArray = this.m_book.FormulaUtil.ParsePtgArray(outputPtg);
                if (worksheet1.CalcEngine.ParseAndComputeFormula(ptgArray) == "TRUE")
                {
                  if (index7 == 0)
                    collection1.Add(currentRow);
                  else if (collection2.Contains(currentRow))
                    collection1.Add(currentRow);
                }
              }
            }
          }
          else if (columnFromCellIndex1 == 0)
          {
            for (int index10 = filterRange.Row + 1; index10 <= filterRange.LastRow; ++index10)
            {
              if (index7 == 0)
                collection1.Add(index10);
              else if (collection2.Contains(index10))
                collection1.Add(index10);
            }
          }
          else if (numArray2 != null)
          {
            WorksheetImpl.TRangeValueType type2 = WorksheetImpl.TRangeValueType.String;
            string outputValue = "";
            double criteriaCellValD = double.NaN;
            string criteriaValue = "";
            string condition = "";
            bool flag2 = false;
            Regex regex = (Regex) null;
            object textFromCellType1 = worksheet2.GetTextFromCellType(rowFromCellIndex1, columnFromCellIndex1, out type2);
            if (type2 == WorksheetImpl.TRangeValueType.String)
            {
              outputValue = textFromCellType1.ToString();
              condition = this.GetComparisionOperator(outputValue, out outputValue);
              criteriaValue = outputValue;
              regex = this.GetWildCardRegex(outputValue, condition);
            }
            else
              criteriaCellValD = (double) textFromCellType1;
            if (type2 == WorksheetImpl.TRangeValueType.String && regex == null && !condition.Equals(string.Empty))
            {
              worksheet2[rowFromCellIndex1, columnFromCellIndex1].Value2 = (object) outputValue;
              object textFromCellType2 = worksheet2.GetTextFromCellType(rowFromCellIndex1, columnFromCellIndex1, out type2);
              if (type2 == WorksheetImpl.TRangeValueType.Number)
                criteriaCellValD = (double) textFromCellType2;
              else
                criteriaValue = textFromCellType2.ToString();
              flag2 = true;
            }
            for (int index11 = 0; index11 < numArray2.GetLength(0); ++index11)
            {
              int rowFromCellIndex2 = RangeImpl.GetRowFromCellIndex(numArray2[index11, index8]);
              int columnFromCellIndex2 = RangeImpl.GetColumnFromCellIndex(numArray2[index11, index8]);
              if (!intList2.Contains(rowFromCellIndex2) || intList2.Count == 0)
              {
                WorksheetImpl.TRangeValueType type3 = WorksheetImpl.TRangeValueType.String;
                object textFromCellType3 = worksheet1.GetTextFromCellType(rowFromCellIndex2, columnFromCellIndex2, out type3);
                if (regex == null ? (type2 != WorksheetImpl.TRangeValueType.Number ? (flag2 ? this.IsConditionSatisfied(criteriaValue, condition, textFromCellType3.ToString(), type2, type3) : textFromCellType3.ToString().StartsWith(outputValue, StringComparison.OrdinalIgnoreCase)) : type2 == type3 && this.IsConditionSatisfied(criteriaCellValD, condition, (double) textFromCellType3)) : regex.IsMatch(textFromCellType3.ToString()))
                {
                  if (index7 == 0)
                    collection1.Add(rowFromCellIndex2);
                  else if (collection2.Contains(rowFromCellIndex2))
                    collection1.Add(rowFromCellIndex2);
                }
                else if (outputValue == string.Empty && criteriaCellValD == double.NaN)
                {
                  if (index7 == 0)
                  {
                    for (int index12 = 0; index12 < numArray2.GetLength(0); ++index12)
                      collection1.Add(RangeImpl.GetRowFromCellIndex(numArray2[index12, index8]));
                  }
                  else if (collection2.Contains(rowFromCellIndex2))
                    collection1.Add(rowFromCellIndex2);
                }
              }
            }
            if (flag2)
              worksheet2[rowFromCellIndex1, columnFromCellIndex1].Text = condition + outputValue;
            ++index8;
          }
          collection2.Clear();
          collection2.AddRange((IEnumerable<int>) collection1);
          collection1.Clear();
        }
        intList2.AddRange((IEnumerable<int>) collection2);
        collection2.Clear();
      }
      if (flag1)
        worksheet1.DisableSheetCalculations();
    }
    if (isUnique)
    {
      List<string> stringList = new List<string>();
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(filterRange.Worksheet.Application, filterRange.Worksheet);
      for (int row = filterRange.Row; row <= filterRange.LastRow; ++row)
      {
        if (intList2.Contains(row))
        {
          string empty = string.Empty;
          for (int column = filterRange.Column; column <= filterRange.LastColumn; ++column)
          {
            migrantRangeImpl.ResetRowColumn(row, column);
            empty += migrantRangeImpl.DisplayText;
          }
          if (!stringList.Contains(empty))
            stringList.Add(empty);
          else
            intList2.Remove(row);
        }
      }
    }
    if (copyToRange != null)
    {
      if (num1 == copyToRange.Columns.Length)
      {
        int num2 = 0;
        for (int row = filterRange.Row; row <= filterRange.LastRow; ++row)
        {
          if (intList2.Contains(row))
          {
            worksheet1[row, filterRange.Column, row, filterRange.LastColumn].CopyTo(copyToRange.Worksheet[copyToRange.Row + num2, copyToRange.Column], ExcelCopyRangeOptions.CopyValueAndSourceFormatting);
            ++num2;
          }
          else if (num2 == 0)
          {
            worksheet1[row, filterRange.Column, row, filterRange.LastColumn].CopyTo(copyToRange.Worksheet[copyToRange.Row + num2, copyToRange.Column], ExcelCopyRangeOptions.CopyValueAndSourceFormatting);
            ++num2;
          }
        }
      }
      else
      {
        for (int column3 = filterRange.Column; column3 <= filterRange.LastColumn; ++column3)
        {
          string str3 = worksheet1.GetTextFromCellType(filterRange.Row, column3, out type1).ToString();
          for (int column4 = copyToRange.Column; column4 <= copyToRange.LastColumn; ++column4)
          {
            string str4 = worksheet1.GetTextFromCellType(copyToRange.Row, column4, out type1).ToString();
            int num3 = 0;
            if (str3.Equals(str4, StringComparison.OrdinalIgnoreCase))
            {
              if (isUnique)
              {
                List<string> stringList = new List<string>();
                IRange range = worksheet1[filterRange.Row, column3, filterRange.LastRow, column3];
                string empty = string.Empty;
                MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(range.Worksheet.Application, range.Worksheet);
                for (int row = range.Row; row <= range.LastRow; ++row)
                {
                  if (intList2.Contains(row))
                  {
                    migrantRangeImpl.ResetRowColumn(row, column3);
                    string displayText = migrantRangeImpl.DisplayText;
                    if (!stringList.Contains(displayText))
                      stringList.Add(displayText);
                    else
                      intList2.Remove(row);
                  }
                }
              }
              MigrantRangeImpl migrantRangeImpl1 = new MigrantRangeImpl(worksheet1.Application, (IWorksheet) worksheet1);
              for (int row = filterRange.Row; row <= filterRange.LastRow; ++row)
              {
                if (intList2.Contains(row))
                {
                  migrantRangeImpl1.ResetRowColumn(row, column3);
                  IRange destination = copyToRange.Worksheet[copyToRange.Row + num3, column4];
                  migrantRangeImpl1.CopyTo(destination, ExcelCopyRangeOptions.CopyValueAndSourceFormatting);
                  ++num3;
                }
                else if (num3 == 0)
                {
                  migrantRangeImpl1.ResetRowColumn(row, column3);
                  IRange destination = copyToRange.Worksheet[copyToRange.Row + num3, column4];
                  migrantRangeImpl1.CopyTo(destination, ExcelCopyRangeOptions.CopyValueAndSourceFormatting);
                  ++num3;
                }
              }
            }
          }
        }
      }
    }
    else
    {
      for (int rowIndex = filterRange.Row + 1; rowIndex <= filterRange.LastRow; ++rowIndex)
      {
        if (!intList2.Contains(rowIndex))
          worksheet1.ShowRow(rowIndex, false);
        else if (!this.IsRowVisible(rowIndex))
          worksheet1.ShowRow(rowIndex, true);
      }
    }
  }

  private Ptg[] CloneArrayPtg(Ptg[] arrPtg)
  {
    Ptg[] ptgArray = new Ptg[arrPtg.Length];
    for (int index = 0; index < arrPtg.Length; ++index)
      ptgArray[index] = (Ptg) arrPtg[index].Clone();
    return ptgArray;
  }

  private bool TryGetValidFormula(
    Ptg[] arrPtg,
    IRange filterRange,
    bool isReplace,
    out Ptg[] outputPtg,
    int currentRow)
  {
    bool validFormula = false;
    outputPtg = arrPtg;
    for (int index = 0; index < arrPtg.Length; ++index)
    {
      if (!arrPtg[index].IsOperation)
      {
        RefPtg refPtg = arrPtg[index] as RefPtg;
        AreaPtg areaPtg = arrPtg[index] as AreaPtg;
        if ((refPtg != null && refPtg.IsRowIndexRelative || areaPtg != null && areaPtg.IsFirstRowRelative && areaPtg.IsLastRowRelative) && arrPtg[index] is IRangeGetter rangeGetter)
        {
          IRange range = rangeGetter.GetRange((IWorkbook) this.m_book, filterRange.Worksheet);
          if (range.Worksheet == filterRange.Worksheet && range.Column >= filterRange.Column && range.LastColumn <= filterRange.LastColumn)
          {
            if (isReplace)
            {
              if (refPtg != null)
              {
                refPtg.RowIndex = currentRow + range.Row - (filterRange.Row + 1) - 1;
              }
              else
              {
                areaPtg.FirstRow = currentRow + range.Row - (filterRange.Row + 1) - 1;
                areaPtg.LastRow = currentRow + range.LastRow - (filterRange.Row + 1) - 1;
              }
            }
            else
            {
              validFormula = true;
              break;
            }
          }
        }
      }
    }
    return validFormula;
  }

  private bool IsConditionSatisfied(
    double criteriaCellValD,
    string condition,
    double filterCellValue)
  {
    bool flag = false;
    switch (condition)
    {
      case ">":
        if (filterCellValue > criteriaCellValD)
        {
          flag = true;
          break;
        }
        break;
      case "<":
        if (filterCellValue < criteriaCellValD)
        {
          flag = true;
          break;
        }
        break;
      case ">=":
        if (filterCellValue >= criteriaCellValD)
        {
          flag = true;
          break;
        }
        break;
      case "<=":
        if (filterCellValue <= criteriaCellValD)
        {
          flag = true;
          break;
        }
        break;
      case "<>":
        if (filterCellValue != criteriaCellValD)
        {
          flag = true;
          break;
        }
        break;
      case "=":
      case "":
        if (filterCellValue == criteriaCellValD)
        {
          flag = true;
          break;
        }
        break;
    }
    return flag;
  }

  internal Regex GetWildCardRegex(string criteriaCelllVal, string condition)
  {
    Regex wildCardRegex = (Regex) null;
    if (!condition.Equals("=") && !condition.Equals(string.Empty))
      return (Regex) null;
    if (Regex.IsMatch(criteriaCelllVal, "[?~*]"))
    {
      string pattern = criteriaCelllVal.Replace(".", "\\.").Replace('?', '.').Replace("*", ".*").Replace("~.*", "\\*").Replace("~.", "\\?").Replace("~~", "~").Replace("[", "\\[").Replace("]", "\\]").Replace("^", "\\^").Replace("$", "\\$").Replace("|", "\\|").Replace("+", "\\+").Replace("{", "\\{").Replace("}", "\\}").Replace("(", "\\(").Replace(")", "\\)");
      if (!criteriaCelllVal.Equals(pattern))
      {
        if (condition == "=")
          pattern = $"^{pattern}$";
        wildCardRegex = new Regex(pattern, RegexOptions.IgnoreCase);
      }
    }
    return wildCardRegex;
  }

  private bool IsConditionSatisfied(
    string criteriaValue,
    string condition,
    string filterValue,
    WorksheetImpl.TRangeValueType criteriaValueType,
    WorksheetImpl.TRangeValueType filterValueType)
  {
    bool flag = false;
    int num = string.Compare(criteriaValue, filterValue, StringComparison.OrdinalIgnoreCase);
    switch (condition)
    {
      case ">":
        if (num < 0 && criteriaValueType == filterValueType)
        {
          flag = true;
          break;
        }
        break;
      case "<":
        if (num > 0 && criteriaValueType == filterValueType)
        {
          flag = true;
          break;
        }
        break;
      case ">=":
        if (num <= 0 && criteriaValueType == filterValueType)
        {
          flag = true;
          break;
        }
        break;
      case "<=":
        if (num >= 0 && criteriaValueType == filterValueType)
        {
          flag = true;
          break;
        }
        break;
      case "<>":
        if (num != 0)
        {
          flag = true;
          break;
        }
        break;
      case "=":
        if (num == 0)
        {
          flag = true;
          break;
        }
        break;
    }
    return flag;
  }

  private string GetComparisionOperator(string inputValue, out string outputValue)
  {
    string comparisionOperator = string.Empty;
    outputValue = inputValue;
    if (inputValue.StartsWith(">="))
    {
      outputValue = inputValue.Substring(2, inputValue.Length - 2);
      comparisionOperator = ">=";
    }
    else if (inputValue.StartsWith("<="))
    {
      outputValue = inputValue.Substring(2, inputValue.Length - 2);
      comparisionOperator = "<=";
    }
    else if (inputValue.StartsWith("<>"))
    {
      outputValue = inputValue.Substring(2, inputValue.Length - 2);
      comparisionOperator = "<>";
    }
    else if (inputValue.StartsWith(">"))
    {
      outputValue = inputValue.Substring(1, inputValue.Length - 1);
      comparisionOperator = ">";
    }
    else if (inputValue.StartsWith("<"))
    {
      outputValue = inputValue.Substring(1, inputValue.Length - 1);
      comparisionOperator = "<";
    }
    else if (inputValue.StartsWith("="))
    {
      outputValue = inputValue.Substring(1, inputValue.Length - 1);
      comparisionOperator = "=";
    }
    return comparisionOperator;
  }

  internal object GetTextFromCellType(int row, int column, out WorksheetImpl.TRangeValueType type)
  {
    WorksheetImpl.TRangeValueType cellType = this.GetCellType(row, column, false);
    string empty = string.Empty;
    type = WorksheetImpl.TRangeValueType.String;
    switch (cellType)
    {
      case WorksheetImpl.TRangeValueType.Blank:
        return (object) string.Empty;
      case WorksheetImpl.TRangeValueType.Error:
        return (object) this.GetError(row, column);
      case WorksheetImpl.TRangeValueType.Boolean:
        return (object) this.GetBoolean(row, column).ToString();
      case WorksheetImpl.TRangeValueType.Number:
        type = WorksheetImpl.TRangeValueType.Number;
        return (object) this.GetNumber(row, column);
      case WorksheetImpl.TRangeValueType.Formula:
        switch (this.GetCellType(row, column, true))
        {
          case WorksheetImpl.TRangeValueType.Formula:
            return (object) this.GetFormulaStringValue(row, column);
          case WorksheetImpl.TRangeValueType.Error | WorksheetImpl.TRangeValueType.Formula:
            return (object) this.GetFormulaErrorValue(row, column);
          case WorksheetImpl.TRangeValueType.Boolean | WorksheetImpl.TRangeValueType.Formula:
            return (object) this.GetFormulaBoolValue(row, column).ToString();
          case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
            type = WorksheetImpl.TRangeValueType.Number;
            return (object) this.GetFormulaNumberValue(row, column);
          case WorksheetImpl.TRangeValueType.Formula | WorksheetImpl.TRangeValueType.String:
            return (object) this.GetFormulaStringValue(row, column);
          default:
            return (object) string.Empty;
        }
      case WorksheetImpl.TRangeValueType.String:
        return (object) this.GetText(row, column);
      default:
        return (object) empty;
    }
  }

  internal bool TryGetIntersectRange(string name, out IRange intersect)
  {
    intersect = (IRange) null;
    bool flag1 = false;
    bool flag2 = false;
    string str1 = name.TrimStart('(').TrimEnd(')');
    string strSheetName = string.Empty;
    string[] nameRanges = str1.Split(' ');
    if (nameRanges.Length > 1)
    {
      for (int index = 0; index < nameRanges.Length; ++index)
        nameRanges[index] = this.AddSheetName(nameRanges[index]);
      string strRow1;
      string strColumn1;
      string strRow2;
      string strColumn2;
      foreach (string str2 in nameRanges)
      {
        if (!string.IsNullOrEmpty(str2) && str2.Trim().Length != 0)
        {
          if (this.Names.Contains(str2))
            flag1 = true;
          else if (((this.Workbook as WorkbookImpl).FormulaUtil.IsCellRange3D(str2, false, out strSheetName, out strRow1, out strColumn1, out strRow2, out strColumn2) || (this.Workbook as WorkbookImpl).FormulaUtil.IsCellRange(str2, false, out strRow1, out strColumn1, out strRow2, out strColumn2)) && this.GetRangeByString(str2, false) != null)
            flag1 = true;
          else if (this.IsEntireRange(str2))
          {
            flag1 = true;
          }
          else
          {
            flag1 = false;
            break;
          }
        }
      }
      if (!flag1)
      {
        foreach (string str3 in nameRanges)
        {
          if (!string.IsNullOrEmpty(str3) && str3.Trim().Length != 0)
          {
            if (this.Workbook.Names.Contains(str3))
            {
              flag1 = true;
              flag2 = true;
            }
            else if (((this.Workbook as WorkbookImpl).FormulaUtil.IsCellRange3D(str3, false, out strSheetName, out strRow1, out strColumn1, out strRow2, out strColumn2) || (this.Workbook as WorkbookImpl).FormulaUtil.IsCellRange(str3, false, out strRow1, out strColumn1, out strRow2, out strColumn2)) && this.GetRangeByString(str3, false) != null)
              flag1 = true;
            else if (this.IsEntireRange(str3))
            {
              flag1 = true;
            }
            else
            {
              flag1 = false;
              flag2 = false;
              break;
            }
          }
        }
      }
      if (flag1)
        intersect = !flag2 ? this.GetIntersectionRange(this.Names, nameRanges) : this.GetIntersectionRange(this.Workbook.Names, nameRanges);
    }
    else
      intersect = (IRange) null;
    return flag1 && intersect != null;
  }

  private bool IsEntireRange(string nameRange)
  {
    string input = nameRange;
    if (nameRange.IndexOf("!") >= 0)
      input = nameRange.Substring(nameRange.IndexOf("!") + 1);
    Match match1 = FormulaUtil.FullRowRangeRegex.Match(input);
    if (match1.Success && match1.Index == 0 && match1.Length == input.Length)
      return true;
    Match match2 = FormulaUtil.FullColumnRangeRegex.Match(input);
    return match2.Success && match2.Index == 0 && match2.Length == input.Length;
  }

  internal bool TryGetExternalIntersectRange(string name, out string intersect)
  {
    intersect = (string) null;
    bool flag = false;
    string str1 = name.TrimStart('(').TrimEnd(')');
    string str2 = string.Empty;
    string[] nameRanges = str1.Split(' ');
    if (nameRanges.Length > 1)
    {
      for (int index = 0; index < nameRanges.Length; ++index)
      {
        string formula = nameRanges[index];
        if (!string.IsNullOrEmpty(formula) && formula.Trim().Length != 0)
        {
          if (this.TryGetExternRangeAddress(this.Workbook as WorkbookImpl, ref formula))
          {
            if (str2 == string.Empty)
              str2 = formula.Substring(0, formula.LastIndexOf('!') + 1);
            if (str2 == formula.Substring(0, formula.LastIndexOf('!') + 1))
            {
              nameRanges[index] = $"{this.Name}!{formula.Substring(formula.LastIndexOf('!') + 1)}";
              flag = true;
            }
            else
            {
              flag = false;
              break;
            }
          }
          else
          {
            flag = false;
            break;
          }
        }
      }
      if (flag)
      {
        IRange intersectionRange = this.GetIntersectionRange(this.Workbook.Names, nameRanges);
        intersect = str2 + intersectionRange.Address.Substring(intersectionRange.Address.LastIndexOf('!') + 1);
      }
    }
    else
      intersect = (string) null;
    return flag && intersect != null;
  }

  private string AddSheetName(string address)
  {
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    int result = 0;
    if (address.Length > 2 && address[0] == '!')
    {
      for (int index = 1; index < address.Length && address[index] != '!'; ++index)
        empty1 += (string) (object) address[index];
    }
    if (int.TryParse(empty1, out result))
    {
      string name = this.Workbook.Worksheets[result].Name;
      address = address.Replace($"!{empty1}!", name + "!");
    }
    return address;
  }

  private IRange GetIntersectionRange(INames names, string[] nameRanges)
  {
    IRange range1 = (IRange) null;
    IRange range2 = (IRange) null;
    IRange intersectionRange = (IRange) null;
    if (names.Contains(nameRanges[0]))
      range1 = names[nameRanges[0]].RefersToRange;
    if (range1 == null)
      range1 = this.GetRangeByString(nameRanges[0], false);
    for (int index = 1; index < nameRanges.Length; ++index)
    {
      if (!string.IsNullOrEmpty(nameRanges[index]) && nameRanges[index].Trim().Length != 0)
      {
        if (names.Contains(nameRanges[index]))
          range2 = names[nameRanges[index]].RefersToRange;
        if (range2 == null)
          range2 = this.GetRangeByString(nameRanges[index], false);
        if (range1 is NameImpl)
          range1 = (range1 as NameImpl).RefersToRange;
        if (range2 is NameImpl)
          range2 = (range2 as NameImpl).RefersToRange;
        if (range1 != null && range2 != null)
          intersectionRange = range1.IntersectWith(range2);
      }
      range1 = intersectionRange;
      range2 = (IRange) null;
    }
    return intersectionRange;
  }

  internal int EvaluateRealColumnWidth(int fileWidth)
  {
    return (int) (this.m_book.PixelsToWidth(this.m_book.FileWidthToPixels((double) fileWidth / 256.0)) * 256.0);
  }

  internal int EvaluateFileColumnWidth(int realWidth)
  {
    return (int) (this.m_book.WidthToFileWidth((double) realWidth / 256.0) * 256.0);
  }

  private void OnNameIndexChanged(object sender, NameIndexChangedEventArgs args)
  {
    throw new NotImplementedException();
  }

  internal void AttachNameIndexChangedEvent() => this.AttachNameIndexChangedEvent(0);

  internal void AttachNameIndexChangedEvent(int iStartIndex) => throw new NotImplementedException();

  public void ParseAutoFilters()
  {
    if (this.m_arrAutoFilter == null || this.m_arrAutoFilter.Count <= 0)
      return;
    this.m_autofilters.Parse(this.m_arrAutoFilter);
  }

  protected void ExtractPivotTables(int iStartIndex)
  {
    if (iStartIndex < 0)
      return;
    if (this.m_pivotTables == null)
      this.m_pivotTables = new PivotTableCollection(this.Application, (object) this);
    this.m_pivotTables.Parse((IList) this.m_arrRecords, iStartIndex);
  }

  [CLSCompliant(false)]
  protected internal ICellPositionFormat GetRecord(long cellIndex)
  {
    return this.m_dicRecordsCells.GetCellRecord(cellIndex);
  }

  [CLSCompliant(false)]
  protected internal ICellPositionFormat GetRecord(int iRow, int iColumn)
  {
    return this.m_dicRecordsCells != null ? this.m_dicRecordsCells.GetCellRecord(iRow, iColumn) : (ICellPositionFormat) null;
  }

  [CLSCompliant(false)]
  protected override int ParseNextRecord(
    BiffReader reader,
    int iBOFCounter,
    ExcelParseOptions options,
    bool bSkipStyles,
    Dictionary<int, int> hashNewXFormatIndexes,
    IDecryptor decryptor)
  {
    TBIFFRecord tbiffRecord = reader.PeekRecordType();
    if (iBOFCounter != 1 || this.ParseOnDemand)
      return base.ParseNextRecord(reader, iBOFCounter, options, bSkipStyles, hashNewXFormatIndexes, decryptor);
    switch (tbiffRecord)
    {
      case TBIFFRecord.Formula:
      case TBIFFRecord.MulRK:
      case TBIFFRecord.MulBlank:
      case TBIFFRecord.RString:
      case TBIFFRecord.LabelSST:
      case TBIFFRecord.Blank:
      case TBIFFRecord.Number:
      case TBIFFRecord.Label:
      case TBIFFRecord.BoolErr:
      case TBIFFRecord.String:
      case TBIFFRecord.Row:
      case TBIFFRecord.Array:
      case TBIFFRecord.RK:
        if (!this.Application.UseFastRecordParsing || decryptor != null || !this.m_dicRecordsCells.ExtractRangesFast(this.m_index, reader, bSkipStyles, hashNewXFormatIndexes))
          this.m_dicRecordsCells.ExtractRanges(reader, bSkipStyles, hashNewXFormatIndexes, decryptor);
        return iBOFCounter;
      default:
        return base.ParseNextRecord(reader, iBOFCounter, options, bSkipStyles, hashNewXFormatIndexes, decryptor);
    }
  }

  [CLSCompliant(false)]
  protected override void ParseDimensions(DimensionsRecord dimensions)
  {
    base.ParseDimensions(dimensions);
    this.m_dicRecordsCells.Table.EnsureSize(this.m_iLastRow);
  }

  public void SetPaneCell(IRange range)
  {
    if (range.Row != range.LastRow || range.Column != range.LastColumn)
      throw new ArgumentOutOfRangeException(nameof (range));
    this.SplitCell = range;
    this.PaneFirstVisible = range;
    this.CreateAllSelections();
  }

  private void CreateAllSelections()
  {
    int selectionCount = this.SelectionCount;
    Dictionary<int, object> usedIndexes = new Dictionary<int, object>();
    for (int index = this.m_arrSelections.Count - 1; index >= 0; --index)
      usedIndexes[(int) this.m_arrSelections[index].Pane] = (object) null;
    int currentIndex = 0;
    for (int count = this.m_arrSelections.Count; count < selectionCount; ++count)
    {
      SelectionRecord record = (SelectionRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Selection);
      currentIndex = (int) (record.Pane = (byte) this.GetFreeIndex(currentIndex, usedIndexes));
      this.m_arrSelections.Add(record);
    }
    int count1 = this.m_arrSelections.Count - selectionCount;
    if (count1 > 0)
      this.m_arrSelections.RemoveRange(selectionCount, count1);
    this.ReIndexSelections(usedIndexes);
  }

  private void ReIndexSelections(Dictionary<int, object> usedIndexes)
  {
    int num1 = 0;
    int num2 = 0;
    if (this.m_pane != null)
    {
      num1 = this.m_pane.VerticalSplit;
      num2 = this.m_pane.HorizontalSplit;
    }
    List<int> panes = new List<int>();
    Dictionary<int, object> mustPresent = new Dictionary<int, object>();
    if (num1 != 0 && num2 != 0)
    {
      this.TryAdd(mustPresent, panes, usedIndexes, 0);
      this.TryAdd(mustPresent, panes, usedIndexes, 1);
      this.TryAdd(mustPresent, panes, usedIndexes, 2);
      this.TryAdd(mustPresent, panes, usedIndexes, 3);
    }
    else if (num1 != 0)
    {
      this.TryAdd(mustPresent, panes, usedIndexes, 3);
      this.TryAdd(mustPresent, panes, usedIndexes, 1);
    }
    else if (num2 != 0)
    {
      this.TryAdd(mustPresent, panes, usedIndexes, 3);
      this.TryAdd(mustPresent, panes, usedIndexes, 2);
    }
    else
      this.TryAdd(mustPresent, panes, usedIndexes, 3);
    int index1 = 0;
    int index2 = 0;
    int count1 = this.m_arrSelections.Count;
    for (int count2 = panes.Count; index1 < count1 && index2 < count2; ++index1)
    {
      SelectionRecord arrSelection = this.m_arrSelections[index1];
      int pane = (int) arrSelection.Pane;
      if (!mustPresent.ContainsKey(pane))
      {
        arrSelection.Pane = (byte) panes[index2];
        ++index2;
      }
    }
    if (this.m_pane == null || mustPresent.ContainsKey((int) this.m_pane.ActivePane))
      return;
    this.m_pane.ActivePane = (ushort) 3;
  }

  private void TryAdd(
    Dictionary<int, object> mustPresent,
    List<int> panes,
    Dictionary<int, object> usedIndexes,
    int paneIndex)
  {
    mustPresent.Add(paneIndex, (object) null);
    if (usedIndexes.ContainsKey(paneIndex))
      return;
    panes.Add(paneIndex);
  }

  private int GetFreeIndex(int currentIndex, Dictionary<int, object> usedIndexes)
  {
    while (usedIndexes.ContainsKey(currentIndex))
      ++currentIndex;
    usedIndexes[currentIndex] = (object) null;
    return currentIndex;
  }

  public void Clear()
  {
    this.ParseData();
    base.ClearAll(ExcelWorksheetCopyFlags.CopyAll);
    this.ClearData();
    if (this.m_dicRecordsCells != null)
      this.m_dicRecordsCells.Clear();
    if (this.m_parseCondtionalFormats)
      this.ParseSheetCF();
    if (this.m_arrConditionalFormats != null)
      this.m_arrConditionalFormats.Clear();
    this.m_rngUsed = (RangeImpl) null;
    this.m_arrColumnInfo = new ColumnInfoRecord[this.m_book.MaxColumnCount + 2];
    if (this.m_hyperlinks != null)
      this.m_hyperlinks.Clear();
    this.m_iFirstColumn = int.MaxValue;
    this.m_iLastColumn = int.MaxValue;
    this.m_iFirstRow = -1;
    this.m_iLastRow = -1;
    if (this.m_mergedCells != null)
      this.m_mergedCells.Clear();
    if (this.Comments == null)
      return;
    this.Comments.Clear();
  }

  internal void ClearAllData()
  {
    WorksheetBaseImpl worksheetBaseImpl = (WorksheetBaseImpl) this;
    ShapesCollection shapes = worksheetBaseImpl.m_shapes;
    if (this.m_calcEngine != null)
      this.DisableSheetCalculations();
    base.ClearAll(ExcelWorksheetCopyFlags.CopyAll);
    this.ClearData();
    if (this.PivotTables.Count > 0)
    {
      for (int index = 0; index < this.PivotTables.Count; ++index)
      {
        PivotCacheImpl cache = (this.PivotTables[index] as PivotTableImpl).Cache;
        if (cache != null && cache.Consolidation != null)
        {
          cache.Consolidation.Dispose();
          cache.Consolidation = (Stream) null;
        }
      }
    }
    if (this.m_dicRecordsCells != null)
      this.m_dicRecordsCells.Clear();
    if (this.m_arrConditionalFormats != null)
      this.m_arrConditionalFormats.Clear();
    if (this.m_rngUsed != null)
    {
      this.m_rngUsed.Dispose();
      this.m_rngUsed = (RangeImpl) null;
    }
    if (this.m_arrColumnInfo != null)
      this.m_arrColumnInfo = (ColumnInfoRecord[]) null;
    if (this.m_hyperlinks != null)
    {
      this.m_hyperlinks.Clear();
      this.m_hyperlinks.ClearAll();
    }
    if (this.m_dataHolder != null)
      this.m_dataHolder.Dispose();
    if (this.m_dicRecordsCells != null)
    {
      this.m_dicRecordsCells.Dispose();
      this.m_dicRecordsCells = (CellRecordCollection) null;
    }
    this.m_iFirstColumn = int.MaxValue;
    this.m_iLastColumn = int.MaxValue;
    this.m_iFirstRow = -1;
    this.m_iLastRow = -1;
    if (this.m_mergedCells != null)
      this.m_mergedCells.Clear();
    if (this.m_sparklineGroups != null)
      this.m_sparklineGroups.Clear();
    if (this.m_autoFitManager != null)
    {
      this.m_autoFitManager.Dispose();
      this.m_autoFitManager = (AutoFitManager) null;
    }
    if (this.m_dataValidation != null)
      this.m_dataValidation.Clear();
    if (this.m_listObjects != null)
      this.m_listObjects.Dispose();
    if (this.m_book != null)
      this.m_book = (WorkbookImpl) null;
    if (this.m_pageSetup != null)
    {
      this.m_pageSetup.Dispose();
      this.m_pageSetup = (PageSetupImpl) null;
    }
    if (this.ValueChanged != null)
      this.ValueChanged = (Syncfusion.Calculate.ValueChangedEventHandler) null;
    if (worksheetBaseImpl.m_bof != null)
      worksheetBaseImpl.m_bof = (BOFRecord) null;
    if (worksheetBaseImpl.m_charts != null)
      worksheetBaseImpl.m_charts = (WorksheetChartsCollection) null;
    if (worksheetBaseImpl.m_checkBoxes != null)
      worksheetBaseImpl.m_checkBoxes = (CheckBoxCollection) null;
    if (worksheetBaseImpl.m_comboBoxes != null)
      worksheetBaseImpl.m_comboBoxes = (ComboBoxCollection) null;
    if (worksheetBaseImpl.m_optionButtons != null)
      worksheetBaseImpl.m_optionButtons = (OptionButtonCollection) null;
    if (shapes != null)
    {
      if (shapes.m_comments != null)
        shapes.m_comments = (CommentsCollection) null;
    }
    if (worksheetBaseImpl.m_pictures != null)
      worksheetBaseImpl.m_pictures = (PicturesCollection) null;
    if (worksheetBaseImpl.m_shapes != null)
      worksheetBaseImpl.m_shapes = (ShapesCollection) null;
    if (worksheetBaseImpl.m_headerFooterShapes != null)
      worksheetBaseImpl.m_headerFooterShapes = (HeaderFooterShapeCollection) null;
    if (worksheetBaseImpl.m_dataHolder != null)
      worksheetBaseImpl.m_dataHolder = (WorksheetDataHolder) null;
    if (this.m_textBoxes != null)
      this.m_textBoxes = (TextBoxCollection) null;
    if (this.m_pivotTables == null)
      return;
    this.m_pivotTables = (PivotTableCollection) null;
  }

  public void ClearData()
  {
    if (this.m_dicRecordsCells != null)
      this.m_dicRecordsCells.ClearData();
    if (this.m_listObjects == null)
      return;
    foreach (ListObject listObject in (List<IListObject>) this.m_listObjects)
      this.m_book.Names.Remove(listObject.Name);
    this.m_listObjects.Clear();
  }

  public bool Contains(int iRow, int iColumn)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Contains(RangeImpl.GetCellIndex(iColumn, iRow));
  }

  public IRanges CreateRangesCollection()
  {
    return (IRanges) this.AppImplementation.CreateRangesCollection((object) this);
  }

  public void CreateNamedRanges(string namedRange, string referRange, bool vertical)
  {
    IWorksheet worksheet = (IWorksheet) this;
    IRanges rangesCollection = worksheet.CreateRangesCollection();
    int row = worksheet[referRange].Row;
    int lastRow = worksheet[referRange].LastRow;
    int column = worksheet[referRange].Column;
    int lastColumn = worksheet[referRange].LastColumn;
    if (!vertical)
    {
      for (int index = row; index < lastRow + 1; ++index)
        rangesCollection.Add(worksheet[index, column, index, lastColumn]);
    }
    else
    {
      for (int index = column; index < lastColumn + 1; ++index)
        rangesCollection.Add(worksheet[row, index, lastRow, index]);
    }
    int index1 = 0;
    INames names = worksheet.Names;
    try
    {
      foreach (IRange range in (IEnumerable<IRange>) worksheet[namedRange])
      {
        names.Add(range.Text).RefersToRange = rangesCollection[index1];
        ++index1;
      }
    }
    catch (Exception ex)
    {
      throw new InvalidRangeException("NamedRange and data count mismatch");
    }
  }

  internal SortedList<long, ExtendedFormatImpl> ApplyCF(IRange cfRange)
  {
    CFApplier cfApplier = new CFApplier();
    SortedList<long, ExtendedFormatImpl> sortedList = new SortedList<long, ExtendedFormatImpl>();
    if (this.mergedCellPositions == null)
      this.BuildMergedRegions();
    IRange usedRange = this.UsedRange;
    int lastColumn = usedRange.LastColumn;
    int lastRow = usedRange.LastRow;
    WorksheetConditionalFormats conditionalFormats = this.ConditionalFormats;
    foreach (Syncfusion.XlsIO.Implementation.Collections.ConditionalFormats format in (CollectionBase<Syncfusion.XlsIO.Implementation.Collections.ConditionalFormats>) conditionalFormats)
    {
      Rectangle rectangle1 = new Rectangle();
      int num1 = this.m_book.MaxColumnCount;
      int num2 = this.m_book.MaxRowCount;
      List<Rectangle> ranges = new List<Rectangle>();
      cfApplier.FirstRowDifference = 0;
      cfApplier.FirstRange = new Rectangle();
      foreach (string cells in format.CellsList)
      {
        Rectangle rectangleFromRangeString = this.GetRectangleFromRangeString(cells);
        ranges.Add(rectangleFromRangeString);
      }
      if (ranges.Count > 1)
      {
        for (int index = 0; index < ranges.Count; ++index)
        {
          if (ranges[index].Left < num1)
          {
            num1 = ranges[index].Left;
            rectangle1 = ranges[index];
          }
        }
        int num3 = num2;
        for (int index = 0; index < ranges.Count; ++index)
        {
          if (rectangle1.Left == ranges[index].Left && ranges[index].Top < num3)
          {
            num3 = ranges[index].Top;
            rectangle1 = ranges[index];
          }
        }
        for (int index = 0; index < ranges.Count; ++index)
        {
          if (ranges[index].Top < num2)
            num2 = ranges[index].Top;
        }
        if (rectangle1.Top > num2)
        {
          Rectangle rectangle2 = new Rectangle();
          for (int index = 0; index < ranges.Count; ++index)
          {
            if (rectangle1.Left == ranges[index].Left && ranges[index].Top > rectangle2.Top)
              rectangle2 = ranges[index];
          }
          cfApplier.FirstRowDifference = rectangle2.Top - num2;
          cfApplier.FirstRange = rectangle2;
        }
        else
          cfApplier.FirstRange = rectangle1;
      }
      else if (ranges.Count == 1)
        cfApplier.FirstRange = ranges[0];
      cfApplier.SetRanges(ranges);
      foreach (Rectangle range in ranges)
      {
        ExtendedFormatImpl xf1 = (ExtendedFormatImpl) null;
        cfApplier.ColumnDifference = 0;
        cfApplier.RowDifference = 0;
        int top = range.Top;
        int left = range.Left;
        int num4 = range.Bottom;
        int num5 = range.Right;
        if (num5 > lastColumn)
          num5 = lastColumn;
        if (num4 > lastRow)
          num4 = lastRow;
        long num6 = 0;
        MigrantRangeImpl cell = new MigrantRangeImpl(this.Application, (IWorksheet) this);
        cfApplier.SetRange(range);
        for (int index1 = top; index1 <= num4; ++index1)
        {
          for (int index2 = left; index2 <= num5; ++index2)
          {
            if (cfRange == null || index1 >= cfRange.Row && index1 <= cfRange.LastRow && index2 >= cfRange.Column && index2 <= cfRange.LastColumn)
            {
              cell.ResetRowColumn(index1, index2);
              long cellIndex = RangeImpl.GetCellIndex(index2, index1);
              if (!this.mergedCellPositions.ContainsKey(cellIndex))
              {
                if (!sortedList.TryGetValue(cellIndex, out xf1))
                {
                  ExtendedFormatImpl wrapped = (cell.CellStyle as CellStyle).Wrapped;
                  xf1 = cfApplier.MergeCF(format, wrapped, (IRange) cell);
                  sortedList.Add(cellIndex, xf1);
                }
                else
                {
                  xf1 = cfApplier.MergeCF(format, xf1, (IRange) cell);
                  sortedList[cellIndex] = xf1;
                }
                if (cfApplier.IsColorApplied)
                  cfApplier.IsColorApplied = false;
              }
            }
          }
        }
        if (top > num4)
          num4 = range.Bottom;
        if (left > num5)
          num5 = range.Right;
        ExtendedFormatImpl xf2 = (ExtendedFormatImpl) null;
        cell.ResetRowColumn(top, left);
        num6 = RangeImpl.GetCellIndex(left, top);
        List<MergeCellsRecord.MergedRegion> lstRegions = new List<MergeCellsRecord.MergedRegion>();
        MergeCellsImpl mergeCells = this.MergeCells;
        mergeCells.CacheMerges(new Rectangle(left, top, num5 - left, num4 - top), lstRegions);
        int index = 0;
        for (int count = lstRegions.Count; index < count; ++index)
        {
          cell.ResetRowColumn(lstRegions[index].RowFrom + 1, lstRegions[index].ColumnFrom + 1);
          long cellIndex = RangeImpl.GetCellIndex(lstRegions[index].ColumnFrom + 1, lstRegions[index].RowFrom + 1);
          if (!sortedList.TryGetValue(cellIndex, out xf2))
          {
            xf2 = mergeCells.GetFormat(lstRegions[index]);
            xf2 = cfApplier.MergeCF(format, xf2, (IRange) cell);
            sortedList.Add(cellIndex, xf2);
          }
          else
          {
            xf2 = cfApplier.MergeCF(format, xf2, (IRange) cell);
            sortedList[cellIndex] = xf2;
          }
          if (cfApplier.IsColorApplied)
            cfApplier.IsColorApplied = false;
        }
        lstRegions?.Clear();
        if (cell != null)
          ;
      }
      if (cfApplier.Top10Cells != null)
      {
        cfApplier.Top10Cells.Clear();
        cfApplier.Top10Cells = (Dictionary<TopBottomImpl, List<long>>) null;
      }
      if (cfApplier.CellValues != null)
      {
        cfApplier.CellValues.Clear();
        cfApplier.CellValues = (Dictionary<long, double>) null;
      }
      if (cfApplier.AboveAverageValues != null)
      {
        cfApplier.AboveAverageValues.Clear();
        cfApplier.AboveAverageValues = (Dictionary<IConditionalFormats, List<double>>) null;
      }
      if (cfApplier.MaxValue != 0.0)
        cfApplier.MaxValue = 0.0;
      if (cfApplier.MinValue != 0.0)
        cfApplier.MinValue = 0.0;
      if (cfApplier.IsNegative)
        cfApplier.IsNegative = false;
    }
    if (cfApplier.CalculationEnabled)
    {
      this.m_book.CalcEngineMemberValuesOnSheet(true);
      this.DisableSheetCalculations();
      cfApplier.CalculationEnabled = false;
    }
    if (conditionalFormats != null)
      ;
    return sortedList;
  }

  internal SortedList<long, ExtendedFormatImpl> ApplyCF() => this.ApplyCF((IRange) null);

  internal Rectangle GetRectangleFromRangeString(string name)
  {
    RangeImpl.GetWorksheetName(ref name);
    IName name1 = this.Names[name] ?? this.m_book.Names[name];
    if (name1 != null)
    {
      IRange refersToRange = name1.RefersToRange;
      switch (refersToRange)
      {
        case null:
        case ExternalRange _:
          return Rectangle.Empty;
        default:
          return new Rectangle(refersToRange.Column, refersToRange.Row, refersToRange.LastColumn - refersToRange.Column, refersToRange.LastRow - refersToRange.Row);
      }
    }
    else
    {
      IRange intersect;
      if (this.TryGetIntersectRange(name, out intersect))
        return new Rectangle(intersect.Column, intersect.Row, intersect.LastColumn - intersect.Column, intersect.LastRow - intersect.Row);
      name = name.ToUpper();
      int iFirstRow;
      int iFirstColumn;
      int iLastRow;
      int iLastColumn;
      switch (RangeImpl.ParseRangeString(name, this.Workbook, out iFirstRow, out iFirstColumn, out iLastRow, out iLastColumn))
      {
        case 1:
          return new Rectangle(iFirstColumn, iFirstRow, 0, 0);
        case 2:
          return new Rectangle(iFirstColumn, iFirstRow, iLastColumn - iFirstColumn, iLastRow - iFirstRow);
        default:
          return Rectangle.Empty;
      }
    }
  }

  public ITemplateMarkersProcessor CreateTemplateMarkersProcessor()
  {
    return (ITemplateMarkersProcessor) this.AppImplementation.CreateTemplateMarkers((object) this);
  }

  public void ShowColumn(int columnIndex, bool isVisible)
  {
    this.ParseData();
    if (columnIndex < 0 || columnIndex > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (columnIndex), "Value cannot be less than 0 and greater than 255");
    ColumnInfoRecord record = this.m_arrColumnInfo[columnIndex];
    if (record == null)
    {
      record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
      record.FirstColumn = (ushort) (columnIndex - 1);
      record.LastColumn = (ushort) (columnIndex - 1);
      record.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
      record.IsUserSet = true;
      record.IsBestFit = false;
      this.m_arrColumnInfo[columnIndex] = record;
    }
    else if (isVisible && record.ColumnWidth == (ushort) 0)
      this.SetColumnWidth(columnIndex, this.StandardWidth);
    record.IsHidden = !isVisible;
    this.UpdateShapes();
  }

  public void HideColumn(int columnIndex) => this.ShowColumn(columnIndex, false);

  public void HideRow(int rowIndex) => this.ShowRow(rowIndex, false);

  public void ShowRow(int rowIndex, bool isVisible)
  {
    if (rowIndex < 1 || rowIndex > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (rowIndex));
    WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, rowIndex - 1, true).IsHidden = !isVisible;
    this.UpdateShapes();
  }

  internal void ShowFilteredRows(int rowIndex, int columnIndex, bool isVisible)
  {
    if (rowIndex < 1 || rowIndex > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (rowIndex));
    RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, rowIndex - 1, true);
    List<int> columnFilterHideRow = row.ColumnFilterHideRow;
    if (isVisible && row.m_isFilteredRow)
    {
      if (columnFilterHideRow.Count > 0 && !columnFilterHideRow.Contains(columnIndex))
        return;
      columnFilterHideRow.Remove(columnIndex);
      if (columnFilterHideRow.Count > 0)
        return;
    }
    row.m_isFilteredRow = !isVisible;
    if (!this.IsParsing)
      row.IsHidden = !isVisible;
    if (row.m_isFilteredRow && !columnFilterHideRow.Contains(columnIndex))
      row.ColumnFilterHideRow.Add(columnIndex);
    this.UpdateShapes();
  }

  private void UpdateShapes()
  {
    if (this.Shapes.Count == 0)
      return;
    for (int index = 0; index < this.Shapes.Count; ++index)
    {
      if (!this.Shapes[index].IsSizeWithCell && !(this.Shapes[index] is CommentShapeImpl))
        ((ShapeImpl) this.Shapes[index]).UpdateAnchorPoints();
    }
  }

  public void ShowRange(IRange range, bool isVisible)
  {
    bool flag1 = false;
    bool flag2 = false;
    if (range.Row < 1 || range.Row > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException("Row");
    int num1 = range.Row;
    int num2 = range.LastRow;
    if (range.LastRow - range.Row > this.m_book.MaxRowCount - (range.LastRow - range.Row) && range.LastRow == this.m_book.MaxRowCount && !isVisible)
    {
      flag1 = true;
      num1 = 1;
      num2 = range.Row - 1;
      if (num2 < this.UsedRange.LastRow)
      {
        num1 = range.Row;
        num2 = this.UsedRange.LastRow;
        flag2 = true;
      }
      this.IsZeroHeight = true;
      isVisible = true;
    }
    int num3 = num1;
    for (int index = num2; num3 <= index; ++num3)
    {
      WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, num3 - 1, true).IsHidden = flag2 ? isVisible : !isVisible;
      this.ParseData();
    }
    if (range.Column < 0 || range.Column > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException("Column", "Value cannot be less than 0 and greater than 255");
    int column = range.Column;
    for (int lastColumn = range.LastColumn; column <= lastColumn; ++column)
    {
      ColumnInfoRecord record = this.m_arrColumnInfo[column];
      if (record == null)
      {
        record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
        record.FirstColumn = (ushort) (column - 1);
        record.LastColumn = (ushort) (lastColumn - 1);
        record.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
        record.IsUserSet = true;
        record.IsBestFit = false;
        this.m_arrColumnInfo[column] = record;
      }
      else if (isVisible && record.ColumnWidth == (ushort) 0)
        this.SetColumnWidth(column, this.StandardWidth);
      record.IsHidden = flag1 ? isVisible : !isVisible;
    }
    this.UpdateShapes();
  }

  public void ShowRange(RangesCollection ranges, bool isVisible)
  {
    if (ranges.Count == 0)
      return;
    foreach (IRange range in ranges)
      this.ShowRange(range, isVisible);
  }

  public void ShowRange(IRange[] ranges, bool isVisible)
  {
    if (ranges.Length == 0)
      return;
    RangesCollection ranges1 = new RangesCollection(this.Application, (object) this);
    foreach (IRange range in ranges)
      ranges1.Add(range);
    this.ShowRange(ranges1, isVisible);
  }

  public bool IsColumnVisible(int columnIndex)
  {
    if (columnIndex < 1 || columnIndex > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (columnIndex), "Value cannot be less than 0 and greater than 255");
    this.ParseData();
    ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[columnIndex];
    return columnInfoRecord == null || !columnInfoRecord.IsHidden;
  }

  public bool IsRowVisible(int rowIndex)
  {
    if (rowIndex < 1 || rowIndex > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (rowIndex));
    RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, rowIndex - 1, false);
    return row == null || !row.IsHidden;
  }

  public void InsertRow(int iRowIndex)
  {
    this.InsertRow(iRowIndex, 1, ExcelInsertOptions.FormatDefault);
  }

  public void InsertRow(int iRowIndex, int iRowCount)
  {
    this.InsertRow(iRowIndex, iRowCount, ExcelInsertOptions.FormatDefault);
  }

  public void InsertRow(int iRowIndex, int iRowCount, ExcelInsertOptions insertOptions)
  {
    this.m_isInsertingRow = true;
    this.ParseData();
    this.insertRowCount = iRowCount;
    this.insertRowIndex = iRowIndex;
    if (iRowIndex < 1 || iRowIndex > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (iRowIndex));
    if (!this.CanInsertRow(iRowIndex, iRowCount, insertOptions) || !this.InnerShapes.CanInsertRowColumn(iRowIndex, iRowCount, true, this.m_book.MaxRowCount))
      throw new ArgumentException("Can't insert row");
    if (this.m_mergedCells != null)
      this.m_mergedCells.InsertRow(iRowIndex, iRowCount);
    this.m_book.InnerNamesColection.InsertRow(iRowIndex, iRowCount, this.Name);
    bool flag1 = iRowIndex <= this.m_iLastRow;
    bool flag2 = iRowIndex + iRowCount >= this.m_book.MaxRowCount;
    int iFirstRow = this.m_iFirstRow;
    int iLastRow = this.m_iLastRow;
    int iFirstColumn = this.m_iFirstColumn;
    int iLastColumn1 = this.m_iLastColumn;
    int iLastColumn2 = this.m_iLastColumn;
    bool flag3 = false;
    if (!flag2)
    {
      if (!flag1)
        iLastRow = iRowIndex;
      if (this.ConditionalFormats.Count > 0)
        this.GetCfUsedLimit(ref iFirstRow, ref iFirstColumn, ref iLastRow, ref iLastColumn1);
      if (iRowIndex <= this.m_iLastRow + 1 && iFirstColumn > 0 && iFirstColumn <= this.m_book.MaxColumnCount)
      {
        if (iRowIndex != this.m_iLastRow + 1)
        {
          flag3 = true;
          int row = iRowIndex;
          int num = iLastRow + iRowCount > this.Workbook.MaxRowCount ? iLastRow + iRowCount - this.Workbook.MaxRowCount : 0;
          IRange source = this.Range[row, iFirstColumn, iLastRow - num, iLastColumn1];
          ExcelCopyRangeOptions options = ExcelCopyRangeOptions.UpdateFormulas | ExcelCopyRangeOptions.CopyErrorIndicators | ExcelCopyRangeOptions.CopyConditionalFormats;
          this.MoveRange(this.Range[row + iRowCount, iFirstColumn, iLastRow + iRowCount - num, iLastColumn1], source, options, true);
        }
        else
          WorksheetHelper.AccessRow((IInternalWorksheet) this, iRowIndex + iRowCount - 1);
      }
      else
      {
        flag3 = true;
        int num = this.m_book.AddSheetReference((IWorksheet) this);
        Rectangle rectSource = Rectangle.FromLTRB(0, iRowIndex - 1, this.Workbook.MaxColumnCount - 1, this.Workbook.MaxRowCount - iRowCount - 1);
        Rectangle rectDest = Rectangle.FromLTRB(0, iRowIndex + iRowCount - 1, this.Workbook.MaxColumnCount - 1, this.Workbook.MaxRowCount - 1);
        this.m_book.UpdateFormula(num, rectSource, num, rectDest);
        iLastRow += iRowCount;
        this.m_dicRecordsCells.Table.InsertIntoDefaultRows(iRowIndex - 1, iRowCount);
      }
    }
    if (flag1)
    {
      this.CopyStylesAfterInsert(iRowIndex, iRowCount, insertOptions, true);
      this.CopyConditionalFormatsAfterInsert(iRowIndex, iRowCount, insertOptions, true);
      this.m_iLastColumn = iLastColumn2;
      this.CopyDataValidationAfterInsert(iRowIndex, iRowCount, insertOptions, true);
    }
    else if (insertOptions != ExcelInsertOptions.FormatDefault)
    {
      this.CopyStylesAfterInsert(iRowIndex, iRowCount, insertOptions, true);
      this.CopyConditionalFormatsAfterInsert(iRowIndex, iRowCount, insertOptions, true);
      this.CopyDataValidationAfterInsert(iRowIndex, iRowCount, insertOptions, true);
    }
    if (this.ListObjects != null)
    {
      for (int index = 0; index < this.ListObjects.Count; ++index)
      {
        IRange location = this.ListObjects[index].Location;
        if (location.Row >= iRowIndex)
          this.ListObjects[index].Location = this[location.Row + iRowCount, location.Column, location.LastRow + iRowCount, location.LastColumn];
        else if (location.Row < iRowIndex && location.LastRow >= iRowIndex)
        {
          this.ListObjects[index].Location = this[location.Row, location.Column, location.LastRow + iRowCount, location.LastColumn];
          this.UpdateCalculatedFormula(iRowIndex, this.ListObjects[index]);
        }
        if (this.Charts != null && this.Charts.Count > 0 && location.Row < this.insertRowIndex && location.LastRow >= this.insertRowIndex)
        {
          foreach (IChart chart in (IEnumerable) this.Charts)
          {
            if (chart.DataRange.Row < this.insertRowIndex && chart.DataRange.LastRow >= this.insertRowIndex)
              chart.DataRange = this[chart.DataRange.Row, chart.DataRange.Column, chart.DataRange.LastRow + this.insertRowCount, chart.DataRange.LastColumn];
          }
        }
      }
    }
    if (this.m_pane != null && iRowIndex <= this.m_pane.HorizontalSplit)
      this.m_pane.HorizontalSplit = (this.m_pane.FirstRow += iRowCount);
    if (!flag3)
    {
      int num = iLastRow + iRowCount > this.Workbook.MaxRowCount ? iLastRow + iRowCount - this.Workbook.MaxRowCount : 0;
      IRange source = this.Range[iRowIndex, iFirstColumn, iLastRow - num, iLastColumn1];
      IRange destination = this.Range[iRowIndex + iRowCount, iFirstColumn, iLastRow + iRowCount - num, iLastColumn1];
      WorksheetImpl worksheet = (WorksheetImpl) destination.Worksheet;
      this.CopyMoveSparkLines(source, destination, worksheet, true);
    }
    this.InnerShapes.InsertRemoveRowColumn(iRowIndex, iRowCount, true, false);
    this.m_book.UpdatePivotCachesAfterInsertRemove(this, iRowIndex, iRowCount, true, false);
    (this.HPageBreaks as HPageBreaksCollection).InsertRows(iRowIndex - 1, iRowCount);
    if (insertOptions == ExcelInsertOptions.FormatDefault)
      this.UpdateRowOutlineLevel(iRowIndex, iRowCount);
    this.SetChanged();
    this.m_isInsertingRow = false;
  }

  private void UpdateCalculatedFormula(int iRowIndex, IListObject listObject)
  {
    for (int index1 = 0; index1 < listObject.Columns.Count; ++index1)
    {
      int iColumn = listObject.Location.Column + index1;
      if (listObject.Columns[index1].CalculatedFormula != null)
      {
        if ((this.Workbook as WorkbookImpl).FormulaUtil.HasCellReference(listObject.Columns[index1].CalculatedFormula))
        {
          (this.Workbook as WorkbookImpl).Application.EnableIncrementalFormula = true;
          int index2 = listObject.Columns[index1].Index;
          IRange range = this.Range[string.Format("R{0}C{2}:R{1}C{2}", (object) (listObject.Location.Columns[index2 - 1].Row + 1), (object) (listObject.ShowTotals ? listObject.Location.Columns[index2 - 1].LastRow - 1 : listObject.Location.Columns[index2 - 1].LastRow), (object) listObject.Location.Columns[index2 - 1].Column), true];
          for (int index3 = 0; index3 < range.Cells.Length; ++index3)
          {
            if (range.Cells[index3].Formula == null)
              range.Cells[index3].Formula = this.GetCalculatedFormula(listObject.Columns[index1].CalculatedFormula, listObject.Name);
          }
          this.SetChanged();
        }
        else
        {
          this.SetFormula(iRowIndex, iColumn, listObject.Columns[index1].CalculatedFormula, true);
          this.SetChanged();
        }
      }
    }
  }

  private string GetCalculatedFormula(string calculatedFormula, string name)
  {
    if (!calculatedFormula.Contains("[["))
    {
      MatchCollection matchCollection = Regex.Matches(calculatedFormula, "\\[.*?\\]", RegexOptions.Compiled);
      List<string> stringList = new List<string>();
      foreach (Match match in matchCollection)
      {
        if (!stringList.Contains(match.Value))
          stringList.Add(match.Value);
      }
      foreach (string str in stringList)
      {
        if (calculatedFormula.Contains($"{name}{str}"))
          calculatedFormula = calculatedFormula.Replace($"{name}{str}", str);
        calculatedFormula = calculatedFormula.Replace(str, $"{name}{str}");
      }
    }
    return calculatedFormula;
  }

  private void UpdateRowOutlineLevel(int iRowIndex, int iRowCount)
  {
    if (iRowIndex <= 1)
      return;
    IRange range1 = this[iRowIndex - 1, 1];
    IRange range2 = this[iRowIndex + iRowCount, 1];
    if (range1 == null || range2 == null || !range1.IsGroupedByRow || !range2.IsGroupedByRow)
      return;
    int num = Math.Min(range1.RowGroupLevel, range2.RowGroupLevel);
    for (int index = 0; index < num; ++index)
      this[iRowIndex, 1, iRowIndex + iRowCount - 1, 1].Group(ExcelGroupBy.ByRows);
  }

  public void InsertColumn(int iColumnIndex)
  {
    this.InsertColumn(iColumnIndex, 1, ExcelInsertOptions.FormatDefault);
  }

  public void InsertColumn(int iColumnIndex, int iColumnCount)
  {
    this.InsertColumn(iColumnIndex, iColumnCount, ExcelInsertOptions.FormatDefault);
  }

  public void InsertColumn(int iColumnIndex, int iColumnCount, ExcelInsertOptions insertOptions)
  {
    this.m_isInsertingColumn = true;
    this.ParseData();
    if (iColumnIndex < 1 || iColumnIndex > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException("Value cannot be less 1 and greater than max column index.");
    if (!this.CanInsertColumn(iColumnIndex, iColumnCount, insertOptions) || !this.InnerShapes.CanInsertRowColumn(iColumnIndex, iColumnCount, false, this.m_book.MaxColumnCount))
      throw new ArgumentException("Can't insert column");
    if (iColumnCount < 1 || iColumnCount > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (iColumnCount), "Value cannot be less 1 and greater than max column index");
    int column1 = this.m_iFirstColumn;
    int iFirstRow1 = this.m_iFirstRow;
    int iLastRow1 = this.m_iLastRow;
    int iFirstColumn = this.m_iFirstColumn;
    int iLastColumn1 = this.m_iLastColumn;
    if (this.ConditionalFormats.Count > 0)
      this.GetCfUsedLimit(ref iFirstRow1, ref iFirstColumn, ref iLastRow1, ref iLastColumn1);
    if (this.m_mergedCells != null)
      this.m_mergedCells.InsertColumn(iColumnIndex, iColumnCount);
    this.m_book.InnerNamesColection.InsertColumn(iColumnIndex, iColumnCount, this.Name);
    if (iColumnIndex <= iLastColumn1 + 1 && iFirstRow1 > 0 && iFirstRow1 <= this.m_book.MaxRowCount)
    {
      if (iColumnIndex != iLastColumn1 + 1)
      {
        if (iColumnIndex >= column1)
          column1 = iColumnIndex;
        IRange source = this.Range[iFirstRow1, column1, iLastRow1, iLastColumn1];
        ExcelCopyRangeOptions options = ExcelCopyRangeOptions.UpdateFormulas | ExcelCopyRangeOptions.CopyErrorIndicators | ExcelCopyRangeOptions.CopyConditionalFormats;
        this.MoveRange(this.Range[iFirstRow1, column1 + iColumnCount], source, options, false);
      }
      else
        WorksheetHelper.AccessColumn((IInternalWorksheet) this, iColumnIndex + iColumnCount - 1);
      this.InsertIntoDefaultColumns(iColumnIndex, iColumnCount, insertOptions);
    }
    else
    {
      int num = this.m_book.AddSheetReference((IWorksheet) this);
      Rectangle rectSource = Rectangle.FromLTRB(iColumnIndex - 1, 0, this.Workbook.MaxColumnCount - iColumnCount - 1, this.Workbook.MaxRowCount - 1);
      Rectangle rectDest = Rectangle.FromLTRB(iColumnIndex + iColumnCount - 1, 0, this.Workbook.MaxColumnCount - 1, this.Workbook.MaxRowCount - 1);
      this.m_book.UpdateFormula(num, rectSource, num, rectDest);
    }
    this.CopyStylesAfterInsert(iColumnIndex, iColumnCount, insertOptions, false);
    this.CopyConditionalFormatsAfterInsert(iColumnIndex, iColumnCount, insertOptions, false);
    this.m_book.UpdatePivotCachesAfterInsertRemove(this, iColumnIndex, iColumnCount, false, false);
    this.CopyDataValidationAfterInsert(iColumnIndex, iColumnCount, insertOptions, false);
    this.InnerShapes.InsertRemoveRowColumn(iColumnIndex, iColumnCount, false, false);
    (this.VPageBreaks as VPageBreaksCollection).InsertColumns(iColumnIndex - 1, iColumnCount);
    int iFirstRow2 = this.m_iFirstRow;
    int iLastRow2 = this.m_iLastRow;
    int iLastColumn2 = this.m_iLastColumn;
    if (iFirstRow2 > 0 && (column1 != iLastColumn2 || column1 == 1 && iLastColumn2 == 1))
    {
      if (!((RangeImpl) this.Range[iFirstRow2, iColumnIndex, iLastRow2, iColumnIndex + iColumnCount - 1]).AreFormulaArraysNotSeparated)
        throw new InvalidRangeException();
      int num1 = 0;
      int count1 = this.ListObjects.Count;
      for (int index = 0; index < count1; ++index)
      {
        IRange location = this.ListObjects[index].Location;
        int column2 = location.Column;
        int lastColumn = location.LastColumn;
        int num2 = iColumnIndex + iColumnCount - 1 - column2;
        int num3 = lastColumn - iColumnIndex;
        if (num2 >= 0 && num3 >= 0)
          ++num1;
        if (num1 > 1)
          throw new Exception("The operation is not allowed. The operation is attempting to shift cells in a table on your worksheet");
      }
      for (int index1 = 0; index1 < count1; ++index1)
      {
        IRange location = this.ListObjects[index1].Location;
        int column3 = location.Column;
        int lastColumn = location.LastColumn;
        int row = location.Row;
        int lastRow = location.LastRow;
        int num4 = iColumnIndex + iColumnIndex - 1 - column3;
        int num5 = lastColumn - iColumnIndex;
        if (num4 >= 0 && num5 >= 0)
        {
          int num6 = iColumnCount;
          int count2 = this.ListObjects[index1].Columns.Count;
          if (iColumnIndex > column3)
          {
            int num7 = iColumnIndex - column3;
            int index2 = num7;
            for (int index3 = 0; index3 < num6; ++index3)
            {
              if (this.ListObjects[index1].ShowHeaderRow)
              {
                this.ListObjects[index1].Columns.Insert(index2, (IListObjectColumn) new ListObjectColumn(this.ListObjects[index1].Columns[num7 - 1].Name + (object) (index3 + 2), index2, this.ListObjects[index1] as ListObject, count2 + index3 + 1));
                this.Range[row, index2 + column3].Text = this.ListObjects[index1].Columns[index2].Name;
                if (this.ListObjects[index1].TableType == ExcelTableType.queryTable)
                {
                  ListObjectColumn column4 = this.ListObjects[index1].Columns[index2] as ListObjectColumn;
                  column4.QueryTableFieldId = column4.Id;
                  this.ListObjects[index1].QueryTable.QueryTableRefresh.NextId = this.ListObjects[index1].Columns.Count + 1;
                }
              }
              else
                this.ListObjects[index1].Columns.Insert(index2, (IListObjectColumn) new ListObjectColumn("Column" + (object) (index3 + 1), index2, this.ListObjects[index1] as ListObject, count2 + index2));
              ++lastColumn;
              ++index2;
            }
            this.ListObjects[index1].Location = this.Range[row, column3, lastRow, lastColumn];
          }
          else
            this.ListObjects[index1].Location = this.Range[row, column3 + num6, lastRow, lastColumn + num6];
        }
      }
    }
    if (insertOptions == ExcelInsertOptions.FormatDefault)
      this.UpdateColumnOutlineLevel(iColumnIndex, iColumnCount);
    this.m_isInsertingColumn = false;
    this.SetChanged();
  }

  private void UpdateColumnOutlineLevel(int index, int count)
  {
    if (index <= 1)
      return;
    IRange range1 = this[1, index - 1];
    IRange range2 = this[1, index + count];
    if (range1 == null || range2 == null || !range1.IsGroupedByColumn || !range2.IsGroupedByColumn)
      return;
    int num = Math.Min(range1.ColumnGroupLevel, range2.ColumnGroupLevel);
    for (int index1 = 0; index1 < num; ++index1)
      this[1, index, 1, index + count - 1].Group(ExcelGroupBy.ByColumns);
  }

  private void CopyDataValidationAfterInsert(
    int iIndex,
    int iCount,
    ExcelInsertOptions insertOptions,
    bool bIsRow)
  {
    if (bIsRow)
    {
      int iRowCount = this.m_book.MaxRowCount - iIndex + 1;
      int iColumnCount = this.m_book.MaxColumnCount - this.m_iFirstColumn + 1;
      this.CopyMoveDataValidations(iIndex, this.m_iFirstColumn, iRowCount, iColumnCount, iIndex + iCount, this.m_iFirstColumn, this, true);
    }
    else
    {
      int iRowCount = this.m_book.MaxRowCount - this.m_iFirstRow + 1;
      int iColumnCount = this.m_book.MaxColumnCount - iIndex + 1;
      this.CopyMoveDataValidations(this.m_iFirstRow, iIndex, iRowCount, iColumnCount, this.m_iFirstRow, iIndex + iCount, this, true);
    }
    this.UpdateDataValidationOnInsertOption(iIndex, iCount, insertOptions, bIsRow);
  }

  private void UpdateDataValidationOnInsertOption(
    int iIndex,
    int iCount,
    ExcelInsertOptions insertOptions,
    bool bIsRow)
  {
    int num1;
    int num2;
    int iDestRow;
    int iDestColumn;
    int iRowCount;
    int iColumnCount;
    if (bIsRow)
    {
      num1 = 1;
      num2 = 0;
      iDestRow = iIndex;
      iDestColumn = 1;
      iRowCount = 1;
      iColumnCount = this.m_book.MaxColumnCount;
    }
    else
    {
      num1 = 0;
      num2 = 1;
      iDestRow = 1;
      iDestColumn = iIndex;
      iRowCount = this.m_book.MaxRowCount;
      iColumnCount = 1;
    }
    int iSourceRow;
    int iSourceColumn;
    if (insertOptions == ExcelInsertOptions.FormatAsBefore || insertOptions == ExcelInsertOptions.FormatDefault)
    {
      if (bIsRow)
      {
        iSourceRow = iIndex - 1;
        iSourceColumn = 1;
      }
      else
      {
        iSourceRow = 1;
        iSourceColumn = iIndex - 1;
      }
    }
    else if (bIsRow)
    {
      iSourceRow = iIndex + iCount;
      iSourceColumn = 1;
    }
    else
    {
      iSourceRow = 1;
      iSourceColumn = iIndex + iCount;
    }
    int num3 = 0;
    while (num3 < iCount)
    {
      this.CopyMoveDataValidations(iSourceRow, iSourceColumn, iRowCount, iColumnCount, iDestRow, iDestColumn, this, false);
      ++num3;
      iDestRow += num1;
      iDestColumn += num2;
    }
  }

  public void DeleteRow(int index) => this.DeleteRow(index, 1);

  public void DeleteRow(int index, int count)
  {
    this.m_isDeletingRow = true;
    this.ParseData();
    this.m_deleteRowIndex = index;
    this.m_deleteRowCount = count;
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count));
    if (index < 1 || index > this.m_book.MaxRowCount - count + 1)
      throw new ArgumentOutOfRangeException("row index");
    if (this.m_parseCondtionalFormats)
      this.ParseSheetCF();
    int num1 = 0;
    int count1 = this.ListObjects.Count;
    for (int index1 = 0; index1 < count1; ++index1)
    {
      IRange location = this.ListObjects[index1].Location;
      int row = location.Row;
      int lastRow = location.LastRow;
      int num2 = index + count - 1 - row;
      int num3 = lastRow - index;
      if (num2 >= 0 && num3 >= 0)
        ++num1;
      if (num1 > 1)
        return;
    }
    for (int index2 = 0; index2 < count1; ++index2)
    {
      IRange location = this.ListObjects[index2].Location;
      int row1 = location.Row;
      int lastRow1 = location.LastRow;
      int column = location.Column;
      int lastColumn = location.LastColumn;
      if (this.ListObjects[index2].ShowHeaderRow && location.Rows.Length <= 2 && location.LastRow == index)
      {
        this[lastRow1, column, lastRow1, lastColumn].Clear(ExcelClearOptions.ClearContent);
        return;
      }
      int num4 = index + count - 1 - row1;
      int num5 = lastRow1 - index;
      IListObject listObject = this.ListObjects[index2];
      if (listObject.ShowHeaderRow && listObject.Location.Row + 1 == index && listObject.Location.LastRow <= index + count - 1)
      {
        int num6;
        if (index + count - 1 > lastRow1)
        {
          int num7 = lastRow1 - index + 1;
          num6 = lastRow1 - num7;
        }
        else
          num6 = lastRow1 - count;
        int lastRow2 = num6 + 1;
        this.ListObjects[index2].Location = this.Range[row1, column, lastRow2, lastColumn];
      }
      else if (!listObject.ShowHeaderRow && listObject.Location.Row == index && listObject.Location.LastRow <= index + count - 1)
        this.ListObjects.RemoveAt(index2);
      else if (num4 >= 0 && num5 >= 0)
      {
        if (index <= row1)
          return;
        int lastRow3;
        if (index + count - 1 > lastRow1)
        {
          int num8 = lastRow1 - index + 1;
          lastRow3 = lastRow1 - num8;
        }
        else
          lastRow3 = lastRow1 - count;
        this.ListObjects[index2].Location = this.Range[row1, column, lastRow3, lastColumn];
      }
      else if (num4 < 0)
      {
        int row2 = row1 - count;
        int lastRow4 = lastRow1 - count;
        this.ListObjects[index2].Location = this.Range[row2, column, lastRow4, lastColumn];
      }
    }
    RecordTable table = this.m_dicRecordsCells.Table;
    int num9 = table.FirstRow + 1;
    int iFirstRow = num9;
    int lastRow5 = table.LastRow + 1;
    int iLastRow = lastRow5;
    int iFirstColumn = this.m_iFirstColumn;
    int column1 = iFirstColumn > 0 ? iFirstColumn : 1;
    int iFirstCol = column1;
    int iLastColumn = this.m_iLastColumn;
    int lastColumn1 = iLastColumn > 0 ? iLastColumn : 1;
    int iLastCol = lastColumn1;
    if (this.ConditionalFormats.Count > 0)
      this.GetCfUsedLimit(ref iFirstRow, ref iFirstCol, ref iLastRow, ref iLastCol);
    if (num9 > 0 || column1 == 1 && lastColumn1 == 1)
    {
      int row = index + count;
      this.m_arrConditionalFormats.Remove(new Rectangle[1]
      {
        Rectangle.FromLTRB(iFirstCol - 1, index - 1, iLastCol - 1, index + count - 2)
      });
      if (row <= lastRow5 && column1 != int.MaxValue)
      {
        IRange source = this.Range[row, column1, lastRow5, lastColumn1];
        IRange destination = this.Range[index, column1];
        IRange cFSource = this.Range[row, iFirstCol, iLastRow, iLastCol];
        IRange cFDestination = this.Range[index, iFirstCol];
        ExcelCopyRangeOptions options = ExcelCopyRangeOptions.UpdateFormulas | ExcelCopyRangeOptions.CopyConditionalFormats;
        IOperation beforeMove = (IOperation) new RowsClearer(this, index, count);
        this.MoveRange(destination, source, options, true, beforeMove, cFDestination, cFSource);
      }
      if (this.m_mergedCells != null)
        this.m_mergedCells.RemoveRow(index, count);
    }
    if (this.m_pane != null && index <= this.m_pane.HorizontalSplit)
      this.m_pane.FirstRow = index + count < this.m_pane.HorizontalSplit ? (this.m_pane.HorizontalSplit -= count) : (this.m_pane.HorizontalSplit = index == 1 ? index : index - 1);
    this.m_book.InnerNamesColection.RemoveRow(index, this.Name, count);
    this.m_book.UpdatePivotCachesAfterInsertRemove(this, index, count, true, true);
    this.InnerShapes.InsertRemoveRowColumn(index, count, true, true);
    (this.HPageBreaks as HPageBreaksCollection).DeleteRows(index - 1, count);
    int count2 = Math.Min(lastRow5 - index + 1, count);
    if (count2 > 0)
      this.RemoveLastRow(true, count2, index);
    if (this.HasSheetCalculation)
    {
      CalcEngine.CreateSheetFamilyID();
      string str = "!";
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      foreach (IName name in (IEnumerable) this.ParentWorkbook.Names)
      {
        if (name.Scope.Length > 0 && str.IndexOf($"!{name.Scope}!") > -1 && name.Value != null)
          dictionary.Add($"{name.Scope}!{name.Name}".ToUpper(), name.Value.Replace("'", ""));
        else if (name.Name != null && name.Value != null && !dictionary.ContainsKey(name.Name.ToUpper()))
          dictionary.Add(name.Name.ToUpper(), name.Value.Replace("'", ""));
      }
      Hashtable hashtable = new Hashtable();
      if (dictionary != null)
      {
        foreach (string key in dictionary.Keys)
          hashtable.Add((object) key.ToUpper(CultureInfo.InvariantCulture), (object) dictionary[key]);
      }
      foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) this.ParentWorkbook.Worksheets)
        worksheet.CalcEngine.NamedRanges = hashtable;
    }
    this.SetChanged();
    this.m_isDeletingRow = false;
  }

  private void CopyRowRecord(int iDestRowIndex, int iSourceRowIndex)
  {
    RowStorage row1 = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iDestRowIndex, true);
    RowStorage row2 = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iSourceRowIndex, true);
    row1.CopyRowRecordFrom(row2);
    this.RaiseRowHeightChangedEvent(iDestRowIndex, (double) row1.Height / 20.0);
  }

  public void DeleteColumn(int index) => this.DeleteColumn(index, 1);

  public void DeleteColumn(int index, int count)
  {
    this.m_isDeletingColumn = true;
    this.ParseData();
    if (index < 1 || index > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException("column index");
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count));
    if (index + count > this.m_book.MaxColumnCount)
      count = this.m_book.MaxColumnCount - index;
    if (count == 0)
      return;
    int iFirstRow = this.m_iFirstRow;
    int iLastRow = this.m_iLastRow;
    int iFirstCol = this.m_iFirstColumn;
    int iLastColumn = this.m_iLastColumn;
    if (this.ConditionalFormats.Count > 0)
      this.GetCfUsedLimit(ref iFirstRow, ref iFirstCol, ref iLastRow, ref iLastColumn);
    if (iFirstRow > 0 || iFirstCol == 1 && iLastColumn == 1)
    {
      if (this.m_parseCondtionalFormats)
        this.ParseSheetCF();
      if (!((RangeImpl) this.Range[iFirstRow, index, iLastRow, index + count - 1]).AreFormulaArraysNotSeparated)
        throw new InvalidRangeException();
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int count1 = this.ListObjects.Count;
      for (int index1 = 0; index1 < count1; ++index1)
      {
        IRange location = this.ListObjects[index1].Location;
        int column = location.Column;
        int lastColumn = location.LastColumn;
        int num4 = index + count - 1 - column;
        int num5 = lastColumn - index;
        if (this.ListObjects[index1].TableType == ExcelTableType.queryTable)
        {
          int nextId = this.ListObjects[index1].QueryTable.QueryTableRefresh.NextId;
          this.ListObjects[index1].QueryTable.QueryTableRefresh.NextId = nextId == 0 ? this.ListObjects[index1].Columns.Count + 1 : nextId;
        }
        if (num4 >= 0 && num5 >= 0)
          ++num3;
        if (num3 > 1)
          return;
      }
      for (int index2 = 0; index2 < count1; ++index2)
      {
        IRange location = this.ListObjects[index2].Location;
        int column1 = location.Column;
        int lastColumn1 = location.LastColumn;
        int row = location.Row;
        int lastRow = location.LastRow;
        int num6 = index + count - 1 - column1;
        int num7 = lastColumn1 - index;
        if (num6 >= 0 && num7 >= 0)
        {
          int index3 = 0;
          int num8 = 0;
          int num9 = count;
          if (index >= column1)
          {
            index3 = index - column1;
            if (index + count - 1 > lastColumn1)
              num9 = count1 - index3;
          }
          else
          {
            num8 = column1 - index;
            if (lastColumn1 >= index + count - 1)
              num9 -= num8;
            else
              num9 = count1;
          }
          for (int index4 = 0; index4 < num9; ++index4)
          {
            this.ListObjects[index2].Columns.RemoveAt(index3);
            if (this.m_book.InnerNamesColection[$"{this.ListObjects[index2].Name}[{this.Range[row, index].DisplayText}]"] is NameImpl nameImpl)
            {
              nameImpl.Delete();
              nameImpl.IsDeleted = false;
              nameImpl.m_isTableNamedRange = false;
              nameImpl.m_isTableNamedRangeDeleted = true;
              (this.ListObjects[index2] as ListObject).TableModified = true;
            }
            --lastColumn1;
          }
          if (this.ListObjects[index2].Columns.Count == 0)
          {
            string name = this.ListObjects[index2].Name;
            this.ListObjects.RemoveAt(index2);
            this.Workbook.Names.Remove(name);
            --index2;
            --count1;
          }
          else if (index < column1)
          {
            num1 = column1 - num8;
            num2 = lastColumn1 - num8;
          }
          else
            this.ListObjects[index2].Location = this.Range[row, column1, lastRow, lastColumn1];
        }
        else if (num6 < 0)
        {
          int column2 = column1 - count;
          int lastColumn2 = lastColumn1 - count;
          this.ListObjects[index2].Location = this.Range[row, column2, lastRow, lastColumn2];
        }
      }
      Rectangle[] rectangleArray = new Rectangle[1]
      {
        Rectangle.FromLTRB(index - 1, iFirstRow - 1, index + count - 2, iLastRow - 1)
      };
      this.m_arrConditionalFormats.Remove(rectangleArray);
      if (this.m_dataValidation != null)
        this.m_dataValidation.Remove(rectangleArray);
      if (index < iLastColumn)
      {
        iFirstCol = index + count;
        if (iFirstCol <= iLastColumn)
        {
          IRange source = this.Range[iFirstRow, iFirstCol, iLastRow, iLastColumn];
          IRange destination = this.Range[iFirstRow, index];
          ExcelCopyRangeOptions options = ExcelCopyRangeOptions.UpdateFormulas | ExcelCopyRangeOptions.CopyConditionalFormats | ExcelCopyRangeOptions.CopyDataValidations;
          this.MoveRange(destination, source, options, false);
        }
      }
      if (this.m_mergedCells != null)
        this.m_mergedCells.RemoveColumn(index, count);
    }
    this.m_book.InnerNamesColection.RemoveColumn(index, this.Name, count);
    this.m_book.UpdatePivotCachesAfterInsertRemove(this, index, count, false, true);
    this.RemoveFromDefaultColumns(index, count, ExcelInsertOptions.FormatDefault);
    this.InnerShapes.InsertRemoveRowColumn(index, count, false, true);
    (this.VPageBreaks as VPageBreaksCollection).DeleteColumns(index - 1, count);
    if (this.UsedRangeIncludesFormatting && this.UsedRange.LastColumn >= index || !this.UsedRangeIncludesFormatting && this.m_iLastColumn >= index)
    {
      count = Math.Min(count, iLastColumn - index + 1);
      this.RemoveLastColumn(true, count, index);
    }
    this.SetChanged();
    this.m_isDeletingColumn = false;
  }

  private void GetCfUsedLimit(
    ref int iFirstRow,
    ref int iFirstCol,
    ref int iLastRow,
    ref int iLastCol)
  {
    for (int i = 0; i < this.ConditionalFormats.Count; ++i)
    {
      foreach (string cells in this.ConditionalFormats[i].CellsList)
      {
        IRange range = this[cells];
        if (iLastRow < range.LastRow)
          iLastRow = range.LastRow;
        if (iLastCol < range.LastColumn)
          iLastCol = range.LastColumn;
        if (iFirstRow > range.Row)
          iFirstRow = range.Row;
        if (iFirstCol > range.Column)
          iFirstCol = range.Column;
      }
    }
  }

  public double GetColumnWidth(int iColumnIndex)
  {
    if (iColumnIndex < 1 || iColumnIndex > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException("Value cannot be less 1 and greater than max column index.");
    return this.InnerGetColumnWidth(iColumnIndex);
  }

  public int GetColumnWidthInPixels(int iColumnIndex)
  {
    if (iColumnIndex > this.m_book.MaxColumnCount)
      iColumnIndex = this.m_book.MaxColumnCount;
    if (iColumnIndex < 1 || iColumnIndex > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException("Value cannot be less 1 and greater than max column index.");
    return this.ColumnWidthToPixels(this.InnerGetColumnWidth(iColumnIndex));
  }

  public int GetHiddenColumnWidthInPixels(int iColumnIndex)
  {
    if (iColumnIndex < 1 || iColumnIndex > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException("Value cannot be less 1 and greater than max column index.");
    ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[iColumnIndex];
    return this.ColumnWidthToPixels(columnInfoRecord == null ? this.StandardWidth : (!columnInfoRecord.IsHidden ? this.InnerGetColumnWidth(iColumnIndex) : (double) ((int) columnInfoRecord.ColumnWidth / 256 /*0x0100*/)));
  }

  public int GetHiddenRowHeightInPixels(int iRowIndex)
  {
    if (iRowIndex < 1 || iRowIndex > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException("Value cannot be less 1 and greater than max row index.");
    RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iRowIndex - 1, false);
    return (int) ApplicationImpl.ConvertToPixels(row == null ? this.StandardHeight : (!row.IsHidden ? this.GetRowHeight(iRowIndex) : (double) ((int) row.Height / 20)), MeasureUnits.Point);
  }

  public double GetRowHeight(int iRow) => this.InnerGetRowHeight(iRow, true);

  internal double GetInnerRowHeight(int iRow) => this.InnerGetRowHeight(iRow + 1, true);

  public int GetRowHeightInPixels(int iRowIndex)
  {
    return (int) ApplicationImpl.ConvertToPixels(this.GetRowHeight(iRowIndex), MeasureUnits.Point);
  }

  internal int GetInnerRowHeightInPixels(int iRowIndex)
  {
    return (int) ApplicationImpl.ConvertToPixels(this.GetInnerRowHeight(iRowIndex), MeasureUnits.Point);
  }

  internal int GetColumnWidthInPixels(int firstColumn, int lastColumn)
  {
    int columnWidthInPixels = 0;
    for (int iColumn = firstColumn; iColumn <= lastColumn; ++iColumn)
      columnWidthInPixels += this.ColumnWidthToPixels(this.InnerGetColumnWidth(iColumn));
    return columnWidthInPixels;
  }

  internal int GetRowHeightInPixels(int firstRow, int lastRow)
  {
    int rowHeightInPixels = 0;
    for (int iRow = firstRow; iRow <= lastRow; ++iRow)
      rowHeightInPixels += (int) ApplicationImpl.ConvertToPixels(this.GetRowHeight(iRow), MeasureUnits.Point);
    return rowHeightInPixels;
  }

  private int ImportArray<T>(T[] arrObject, int firstRow, int firstColumn, bool isVertical)
  {
    if (arrObject == null)
      throw new ArgumentNullException(nameof (arrObject));
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentNullException(nameof (firstColumn));
    this.ParseData();
    int index1 = 0;
    int num = !isVertical ? Math.Min(firstColumn + arrObject.Length - 1, this.m_book.MaxColumnCount) - firstColumn + 1 : Math.Min(firstRow + arrObject.Length - 1, this.m_book.MaxRowCount) - firstRow + 1;
    int defaultXfIndex = this.m_book.DefaultXFIndex;
    if (num > 0)
    {
      IRange cell = this.InnerGetCell(firstColumn, firstRow);
      cell.Value2 = (object) arrObject[index1] != null ? (object) arrObject[index1] : (object) null;
      int extendedFormatIndex = (int) ((RangeImpl) cell).ExtendedFormatIndex;
    }
    int index2;
    for (index2 = 1; index2 < num; ++index2)
    {
      IRange range = isVertical ? this.InnerGetCell(firstColumn, firstRow + index2) : this.InnerGetCell(firstColumn + index2, firstRow);
      if ((object) arrObject[index2] is string)
      {
        if (!(range.Worksheet as WorksheetImpl).CheckAndAddHyperlink((object) arrObject[index2] as string, range as RangeImpl))
          range.Value2 = (object) arrObject[index2];
      }
      else
        range.Value2 = (object) arrObject[index2];
    }
    return index2;
  }

  private bool CheckIsFormula(object value) => value.ToString().StartsWith("=");

  public int ImportArray(object[] arrObject, int firstRow, int firstColumn, bool isVertical)
  {
    return this.ImportArray<object>(arrObject, firstRow, firstColumn, isVertical);
  }

  public int ImportArray(string[] arrString, int firstRow, int firstColumn, bool isVertical)
  {
    return this.ImportArray<string>(arrString, firstRow, firstColumn, isVertical);
  }

  public int ImportArray(int[] arrInt, int firstRow, int firstColumn, bool isVertical)
  {
    return this.ImportArray<int>(arrInt, firstRow, firstColumn, isVertical);
  }

  public int ImportArray(double[] arrDouble, int firstRow, int firstColumn, bool isVertical)
  {
    return this.ImportArray<double>(arrDouble, firstRow, firstColumn, isVertical);
  }

  public int ImportArray(DateTime[] arrDateTime, int firstRow, int firstColumn, bool isVertical)
  {
    if (arrDateTime == null)
      throw new ArgumentNullException("arrObject");
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentNullException(nameof (firstColumn));
    this.ParseData();
    int index1 = 0;
    int num = !isVertical ? Math.Min(firstColumn + arrDateTime.Length - 1, this.m_book.MaxColumnCount) - firstColumn + 1 : Math.Min(firstRow + arrDateTime.Length - 1, this.m_book.MaxRowCount) - firstRow + 1;
    int iXFIndex = this.m_book.DefaultXFIndex;
    if (num > 0)
    {
      IRange range = isVertical ? this.InnerGetCell(firstColumn, firstRow) : this.InnerGetCell(firstColumn, firstRow);
      range.DateTime = arrDateTime[index1];
      iXFIndex = (int) ((RangeImpl) range).ExtendedFormatIndex;
    }
    int index2;
    for (index2 = 1; index2 < num; ++index2)
      (isVertical ? this.InnerGetCell(firstColumn, firstRow + index2, iXFIndex) : this.InnerGetCell(firstColumn + index2, firstRow, iXFIndex)).DateTime = arrDateTime[index2];
    return index2;
  }

  public int ImportArray(object[,] arrObject, int firstRow, int firstColumn)
  {
    if (arrObject == null)
      throw new ArgumentNullException(nameof (arrObject));
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentNullException(nameof (firstColumn));
    this.ParseData();
    int num = Math.Min(firstRow + arrObject.GetLength(0) - 1, this.m_book.MaxRowCount) - firstRow + 1;
    int length = Math.Min(firstColumn + arrObject.GetLength(1) - 1, this.m_book.MaxColumnCount) - firstColumn + 1;
    int[] numArray = new int[length];
    if (length <= 0 || num <= 0)
      return 0;
    for (int index = 0; index < length; ++index)
    {
      IRange cell = this.InnerGetCell(index + firstColumn, firstRow);
      if (arrObject[0, index] == null)
        cell.Value2 = (object) null;
      else if (arrObject[0, index] is string)
      {
        if (!(cell.Worksheet as WorksheetImpl).CheckAndAddHyperlink(arrObject[0, index] as string, cell as RangeImpl))
          cell.Value2 = arrObject[0, index];
      }
      else
        cell.Value2 = arrObject[0, index];
      RangeImpl rangeImpl = (RangeImpl) cell;
      numArray[index] = (int) rangeImpl.ExtendedFormatIndex;
    }
    int index1;
    for (index1 = 1; index1 < num; ++index1)
    {
      for (int index2 = 0; index2 < length; ++index2)
        this.InnerGetCell(firstColumn + index2, index1 + firstRow, numArray[index2]).Value2 = arrObject[index1, index2] != null ? arrObject[index1, index2] : (object) null;
    }
    return index1;
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn)
  {
    return this.ImportDataTable(dataTable, isFieldNameShown, firstRow, firstColumn, -1, -1);
  }

  public int ImportDataTable(
    DataTable dataTable,
    int firstRow,
    int firstColumn,
    bool importOnSave)
  {
    return this.ImportDataTable(dataTable, false, firstRow, firstColumn, -1, -1, (DataColumn[]) null, false, importOnSave);
  }

  public int ImportDataTable(
    DataTable dataTable,
    int firstRow,
    int firstColumn,
    bool importOnSave,
    bool includeheader)
  {
    return this.ImportDataTable(dataTable, includeheader, firstRow, firstColumn, -1, -1, (DataColumn[]) null, false, importOnSave);
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    bool preserveTypes)
  {
    return this.ImportDataTable(dataTable, isFieldNameShown, firstRow, firstColumn, -1, -1, preserveTypes);
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns)
  {
    return this.ImportDataTable(dataTable, isFieldNameShown, firstRow, firstColumn, maxRows, maxColumns, (DataColumn[]) null, false);
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    bool preserveTypes)
  {
    return this.ImportDataTable(dataTable, isFieldNameShown, firstRow, firstColumn, maxRows, maxColumns, (DataColumn[]) null, preserveTypes);
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    DataColumn[] arrDataColumns,
    bool bPreserveTypes)
  {
    return this.ImportDataTable(dataTable, isFieldNameShown, firstRow, firstColumn, maxRows, maxColumns, arrDataColumns, bPreserveTypes, false);
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    DataColumn[] arrDataColumns,
    bool bPreserveTypes,
    bool bImportOnSave)
  {
    if (dataTable == null)
      throw new ArgumentNullException(nameof (dataTable));
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (firstColumn));
    this.ParseData();
    if (bImportOnSave)
    {
      SSTDictionary innerSst = this.ParentWorkbook.InnerSST;
      this.dateTimeStyleIndex = this.ReplaceDataTable(dataTable, innerSst, isFieldNameShown);
      this.m_importDTHelper = new ImportDTHelper(dataTable, firstRow, firstColumn, this.dateTimeStyleIndex, isFieldNameShown);
      maxRows = Math.Min(dataTable.Rows.Count, this.m_book.MaxRowCount - firstRow);
      return maxRows;
    }
    this.m_dicRecordsCells.UpdateRows(dataTable.Rows.Count);
    this.m_book.MaxImportColumns = dataTable.Columns.Count * 18;
    if (arrDataColumns == null || arrDataColumns.Length == 0)
    {
      arrDataColumns = new DataColumn[dataTable.Columns.Count];
      dataTable.Columns.CopyTo(arrDataColumns, 0);
    }
    int count = dataTable.Rows.Count;
    if (maxRows < 0 || maxRows > count)
      maxRows = count;
    int length = arrDataColumns.Length;
    if (maxColumns < 0 || maxColumns > length)
      maxColumns = length;
    maxColumns = Math.Min(maxColumns, this.m_book.MaxColumnCount - firstColumn + 1);
    maxRows = Math.Min(maxRows, this.m_book.MaxRowCount - firstRow);
    if (isFieldNameShown)
    {
      for (int index = 0; index < maxColumns; ++index)
        this.SetText(firstRow, firstColumn + index, arrDataColumns[index].Caption);
      ++firstRow;
    }
    if (!bPreserveTypes)
      this.ImportDataTableWithoutCheck(dataTable, firstRow, firstColumn, maxRows, maxColumns, arrDataColumns, this.m_bOptimizeImport);
    else
      this.ImportDataTableWithoutCheckPreserve(dataTable, firstRow, firstColumn, maxRows, maxColumns, arrDataColumns);
    this.m_book.MaxImportColumns = 0;
    this.m_book.IsCellModified = true;
    return maxRows;
  }

  internal int ReplaceDataTable(DataTable dataTable, SSTDictionary sst, bool isFieldNameShown)
  {
    for (int index1 = 0; index1 < dataTable.Columns.Count; ++index1)
    {
      DataColumn column = dataTable.Columns[index1];
      if (isFieldNameShown)
      {
        string columnName = column.ColumnName;
        if (!columnName.Equals((object) DBNull.Value) && !columnName.Equals(string.Empty))
          sst.AddIncrease((object) columnName);
      }
      switch (column.DataType.Name)
      {
        case "String":
          for (int index2 = 0; index2 < dataTable.Rows.Count; ++index2)
          {
            object obj = dataTable.Rows[index2][column];
            if (!obj.Equals((object) DBNull.Value) && !obj.Equals((object) string.Empty))
              sst.AddIncrease((object) obj.ToString());
          }
          break;
        case "DateTime":
          if (this.format == null)
          {
            this.format = (this.Workbook as WorkbookImpl).CreateExtFormat(true) as ExtendedFormatImpl;
            this.format.NumberFormatIndex = 14;
            this.dateTimeStyleIndex = this.format.ParentIndex + 1;
            break;
          }
          break;
      }
    }
    return this.dateTimeStyleIndex;
  }

  public int ImportDataTable(DataTable dataTable, IName namedRange, bool isFieldNameShown)
  {
    return this.ImportDataTable(dataTable, namedRange, isFieldNameShown, 0, 0);
  }

  public int ImportDataTable(
    DataTable dataTable,
    IName namedRange,
    bool isFieldNameShown,
    int rowOffset,
    int columnOffset)
  {
    return this.ImportDataTable(dataTable, namedRange, isFieldNameShown, rowOffset, columnOffset, -1, -1);
  }

  public int ImportDataTable(
    DataTable dataTable,
    IName namedRange,
    bool isFieldNameShown,
    int rowOffset,
    int columnOffset,
    int iMaxRow,
    int iMaxCol)
  {
    return this.ImportDataTable(dataTable, namedRange, isFieldNameShown, rowOffset, columnOffset, iMaxRow, iMaxCol, false);
  }

  public int ImportDataTable(
    DataTable dataTable,
    IName namedRange,
    bool isFieldNameShown,
    int rowOffset,
    int columnOffset,
    int iMaxRow,
    int iMaxCol,
    bool bPreserveTypes)
  {
    if (dataTable == null)
      throw new ArgumentNullException(nameof (dataTable));
    if (namedRange == null)
      throw new ArgumentNullException(nameof (namedRange));
    if (rowOffset < 0)
      throw new ArgumentOutOfRangeException(nameof (rowOffset));
    if (columnOffset < 0)
      throw new ArgumentOutOfRangeException(nameof (columnOffset));
    this.ParseData();
    IRange refersToRange = namedRange.RefersToRange;
    if (!(refersToRange is RangeImpl))
      throw new NotSupportedException("Doesnot support range collection as named range.");
    int count1 = dataTable.Rows.Count;
    if (iMaxRow < 0 || iMaxRow > count1)
      iMaxRow = count1;
    int count2 = dataTable.Columns.Count;
    if (iMaxCol < 0 || iMaxCol > count2)
      iMaxCol = count2;
    int num1 = refersToRange.LastColumn - refersToRange.Column + 1 - columnOffset - iMaxCol;
    int num2 = refersToRange.LastRow - refersToRange.Row + 1 - rowOffset - iMaxRow;
    if (isFieldNameShown)
      --num2;
    if (num2 < 0 || num1 < 0)
      throw new NotSupportedException("Bounds of data table is greatfull than bounds of named range.");
    WorksheetImpl worksheet = (WorksheetImpl) refersToRange.Worksheet;
    DataColumn[] dataColumnArray = new DataColumn[count2];
    dataTable.Columns.CopyTo(dataColumnArray, 0);
    if (isFieldNameShown)
    {
      for (int index = 0; index < iMaxCol; ++index)
        refersToRange.Worksheet[refersToRange.Row + rowOffset, refersToRange.Column + columnOffset + index].Value2 = (object) dataColumnArray[index].Caption;
      ++rowOffset;
    }
    if (bPreserveTypes)
      worksheet.ImportDataTableWithoutCheckPreserve(dataTable, refersToRange.Row + rowOffset, refersToRange.Column + columnOffset, iMaxRow, iMaxCol, dataColumnArray);
    else
      worksheet.ImportDataTableWithoutCheck(dataTable, refersToRange.Row + rowOffset, refersToRange.Column + columnOffset, iMaxRow, iMaxCol, dataColumnArray, this.m_bOptimizeImport);
    return iMaxRow;
  }

  public int ImportDataColumn(
    DataColumn dataColumn,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn)
  {
    if (dataColumn == null)
      throw new ArgumentNullException(nameof (dataColumn));
    return this.ImportDataColumns(new DataColumn[1]
    {
      dataColumn
    }, isFieldNameShown, firstRow, firstColumn);
  }

  public int ImportDataColumn(
    DataColumn dataColumn,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    bool preserveTypes)
  {
    if (dataColumn == null)
      throw new ArgumentNullException(nameof (dataColumn));
    return this.ImportDataColumns(new DataColumn[1]
    {
      dataColumn
    }, isFieldNameShown, firstRow, firstColumn, preserveTypes);
  }

  public int ImportDataReader(
    IDataReader dataReader,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn)
  {
    DataTable dataTable = new DataTable();
    dataTable.BeginLoadData();
    dataTable.Load(dataReader, LoadOption.Upsert);
    dataTable.EndLoadData();
    return this.ImportDataTable(dataTable, isFieldNameShown, firstRow, firstColumn);
  }

  public int ImportDataReader(
    IDataReader dataReader,
    int firstRow,
    int firstColumn,
    bool importOnSave)
  {
    DataTable dataTable = new DataTable();
    dataTable.BeginLoadData();
    dataTable.Load(dataReader, LoadOption.Upsert);
    dataTable.EndLoadData();
    return this.ImportDataTable(dataTable, firstRow, firstColumn, importOnSave);
  }

  public int ImportDataReader(
    IDataReader dataReader,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    bool preserveTypes)
  {
    DataTable dataTable = new DataTable();
    dataTable.BeginLoadData();
    dataTable.Load(dataReader, LoadOption.Upsert);
    dataTable.EndLoadData();
    return this.ImportDataTable(dataTable, isFieldNameShown, firstRow, firstColumn, preserveTypes);
  }

  public int ImportDataReader(IDataReader dataReader, IName namedRange, bool isFieldNameShown)
  {
    DataTable dataTable = new DataTable();
    dataTable.BeginLoadData();
    dataTable.Load(dataReader, LoadOption.Upsert);
    dataTable.EndLoadData();
    return this.ImportDataTable(dataTable, namedRange, isFieldNameShown);
  }

  public int ImportDataColumns(
    DataColumn[] arrDataColumns,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn)
  {
    if (arrDataColumns == null)
      throw new ArgumentNullException(nameof (arrDataColumns));
    if (arrDataColumns.Length == 0)
      throw new ArgumentException("arrDataColumns can't be empty");
    return this.ImportDataTable(arrDataColumns[0].Table, isFieldNameShown, firstRow, firstColumn, -1, -1, arrDataColumns, false);
  }

  internal int ImportDataColumns(
    DataColumn[] arrDataColumns,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    bool preserveTypes)
  {
    if (arrDataColumns == null)
      throw new ArgumentNullException(nameof (arrDataColumns));
    if (arrDataColumns.Length == 0)
      throw new ArgumentException("arrDataColumns can't be empty");
    return this.ImportDataTable(arrDataColumns[0].Table, isFieldNameShown, firstRow, firstColumn, -1, -1, arrDataColumns, preserveTypes);
  }

  public void ImportDataGrid(
    System.Windows.Forms.DataGrid dataGrid,
    int firstRow,
    int firstColumn,
    bool isImportHeader,
    bool isImportStyle)
  {
    if (dataGrid == null)
      throw new ArgumentNullException(nameof (dataGrid));
    if (dataGrid.DataSource == null || dataGrid.DataSource == (object) string.Empty)
      throw new ArgumentNullException("dataSource");
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (firstColumn));
    int columnCount;
    int num1 = this.ImportDataSource(dataGrid.DataSource, firstRow, firstColumn, isImportHeader, dataGrid.DataMember, out columnCount);
    if (!isImportStyle)
      return;
    int num2 = isImportHeader ? firstRow + num1 + 1 : firstRow + num1;
    for (int iRow = firstRow; iRow < num2; ++iRow)
    {
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this);
      if (iRow == firstRow && isImportHeader)
      {
        for (int iColumn = firstColumn; iColumn < firstColumn + columnCount; ++iColumn)
        {
          migrantRangeImpl.ResetRowColumn(iRow, iColumn);
          IStyle cellStyle = migrantRangeImpl.CellStyle;
          cellStyle.BeginUpdate();
          cellStyle.Color = dataGrid.HeaderBackColor;
          IFont font = cellStyle.Font;
          font.BeginUpdate();
          font.RGBColor = dataGrid.HeaderForeColor;
          this.SetGridFontStyle(dataGrid.HeaderFont, font);
          font.EndUpdate();
          cellStyle.EndUpdate();
          if (dataGrid.GridLineStyle == DataGridLineStyle.None)
            migrantRangeImpl.BorderAround(ExcelLineStyle.None);
          else
            migrantRangeImpl.BorderAround(ExcelLineStyle.Medium, dataGrid.GridLineColor);
        }
      }
      else
      {
        int num3 = isImportHeader ? iRow + 1 : iRow;
        for (int iColumn = firstColumn; iColumn < firstColumn + columnCount; ++iColumn)
        {
          migrantRangeImpl.ResetRowColumn(iRow, iColumn);
          IStyle cellStyle = migrantRangeImpl.CellStyle;
          cellStyle.BeginUpdate();
          if (firstRow % 2 != 0)
          {
            if (num3 % 2 != 1)
              cellStyle.Color = dataGrid.AlternatingBackColor;
            else
              cellStyle.Color = dataGrid.BackColor;
          }
          else if (num3 % 2 != 0)
            cellStyle.Color = dataGrid.AlternatingBackColor;
          else
            cellStyle.Color = dataGrid.BackColor;
          IFont font = cellStyle.Font;
          font.BeginUpdate();
          font.RGBColor = dataGrid.ForeColor;
          this.SetGridFontStyle(dataGrid.Font, font);
          font.EndUpdate();
          cellStyle.EndUpdate();
          if (dataGrid.GridLineStyle == DataGridLineStyle.None)
            migrantRangeImpl.BorderAround(ExcelLineStyle.None);
          else
            migrantRangeImpl.BorderAround(ExcelLineStyle.Medium, dataGrid.GridLineColor);
        }
      }
    }
  }

  private void SetGridFontStyle(Font gridFont, IFont cellFont)
  {
    cellFont.Size = (double) gridFont.Size;
    cellFont.Bold = gridFont.Bold;
    cellFont.FontName = gridFont.FontFamily.Name;
    cellFont.Italic = gridFont.Italic;
    cellFont.Strikethrough = gridFont.Strikeout;
    if (!gridFont.Underline)
      return;
    cellFont.Underline = ExcelUnderline.Single;
  }

  private int ImportDataSource(
    object dataSource,
    int firstRow,
    int firstColumn,
    bool isImportHeader,
    string dataMember,
    out int columnCount)
  {
    columnCount = 0;
    int num;
    switch (dataSource)
    {
      case DataTable _:
        DataTable dataTable1 = (DataTable) dataSource;
        num = this.ImportDataTable(dataTable1, isImportHeader, firstRow, firstColumn);
        columnCount = dataTable1.Columns.Count;
        break;
      case DataView _:
        DataView dataView = (DataView) dataSource;
        num = this.ImportDataView(dataView, isImportHeader, firstRow, firstColumn);
        columnCount = dataView.Table.Columns.Count;
        break;
      case DataSet _:
        DataSet dataSet = (DataSet) dataSource;
        DataTable dataTable2 = (DataTable) null;
        if (dataMember != string.Empty)
          dataTable2 = dataSet.Tables[dataMember];
        else if (dataSet.Tables.Count > 0)
          dataTable2 = dataSet.Tables[0];
        num = this.ImportDataTable(dataTable2, isImportHeader, firstRow, firstColumn);
        columnCount = dataTable2.Columns.Count;
        break;
      case int[] _:
        num = this.ImportArray((int[]) dataSource, firstRow, firstColumn, true);
        columnCount = 1;
        break;
      case double[] _:
        num = this.ImportArray((double[]) dataSource, firstRow, firstColumn, true);
        columnCount = 1;
        break;
      case string[] _:
        num = this.ImportArray((string[]) dataSource, firstRow, firstColumn, true);
        columnCount = 1;
        break;
      case DateTime[] _:
        num = this.ImportArray((DateTime[]) dataSource, firstRow, firstColumn, true);
        columnCount = 1;
        break;
      case object[] _:
        num = this.ImportArray((object[]) dataSource, firstRow, firstColumn, true);
        columnCount = 1;
        break;
      case object[,] _:
        object[,] arrObject1 = (object[,]) dataSource;
        num = this.ImportArray(arrObject1, firstRow, firstColumn);
        columnCount = arrObject1.GetLength(1);
        break;
      case IEnumerable _:
        IEnumerable arrObject2 = (IEnumerable) dataSource;
        num = this.ImportData(arrObject2, firstRow, firstColumn, isImportHeader);
        List<PropertyInfo> propertyInfo1 = (List<PropertyInfo>) null;
        object obj1 = (object) null;
        IEnumerator enumerator1 = arrObject2.GetEnumerator();
        if (enumerator1.MoveNext())
          obj1 = enumerator1.Current;
        this.GetObjectMembersInfo(obj1, out propertyInfo1);
        columnCount = propertyInfo1.Count;
        break;
      case IListSource _:
        IListSource listSource = (IListSource) dataSource;
        num = this.ImportData((IEnumerable) listSource.GetList(), firstRow, firstColumn, isImportHeader);
        List<PropertyInfo> propertyInfo2 = (List<PropertyInfo>) null;
        object obj2 = (object) null;
        IEnumerator enumerator2 = listSource.GetList().GetEnumerator();
        if (enumerator2.MoveNext())
          obj2 = enumerator2.Current;
        this.GetObjectMembersInfo(obj2, out propertyInfo2);
        columnCount = propertyInfo2.Count;
        break;
      default:
        throw new Exception("Data source is not supported.");
    }
    return num;
  }

  private void SetWebGridFontSize(FontInfo gridFont, IFont cellFont)
  {
    if (gridFont.Size.IsEmpty)
      return;
    switch (gridFont.Size.Type)
    {
      case FontSize.Smaller:
        cellFont.Size = 18.0;
        break;
      case FontSize.Larger:
        cellFont.Size = 36.0;
        break;
      case FontSize.XXSmall:
        cellFont.Size = 7.5;
        break;
      case FontSize.XSmall:
        cellFont.Size = 10.0;
        break;
      case FontSize.Medium:
        cellFont.Size = 13.5;
        break;
      case FontSize.Large:
        cellFont.Size = 18.0;
        break;
      case FontSize.XLarge:
        cellFont.Size = 24.0;
        break;
      case FontSize.XXLarge:
        cellFont.Size = 36.0;
        break;
      default:
        cellFont.Size = gridFont.Size.Unit.Value;
        break;
    }
  }

  public void ImportDataGrid(
    System.Web.UI.WebControls.DataGrid dataGrid,
    int firstRow,
    int firstColumn,
    bool isImportHeader,
    bool isImportStyle)
  {
    if (dataGrid == null)
      throw new ArgumentNullException("datagrid");
    if (dataGrid.DataSource == null || dataGrid.DataSource == (object) string.Empty)
      throw new ArgumentNullException("dataSource");
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (firstColumn));
    int num1 = this.ImportDataSource(dataGrid.DataSource, firstRow, firstColumn, isImportHeader, dataGrid.DataMember, out int _);
    if (!isImportStyle)
      return;
    int num2 = isImportHeader ? firstRow + num1 + 1 : firstRow + num1;
    for (int iRow = firstRow; iRow < num2; ++iRow)
    {
      MigrantRangeImpl migrantRange = new MigrantRangeImpl(this.Application, (IWorksheet) this);
      if (iRow == firstRow && isImportHeader)
      {
        for (int iColumn = firstColumn; iColumn < firstColumn + dataGrid.Items[0].Cells.Count; ++iColumn)
        {
          migrantRange.ResetRowColumn(iRow, iColumn);
          this.SetWebGridStyle((IRange) migrantRange, dataGrid.HeaderStyle, dataGrid, true);
        }
      }
      else
      {
        int num3 = isImportHeader ? iRow + 1 : iRow;
        for (int iColumn = firstColumn; iColumn < firstColumn + dataGrid.Items[0].Cells.Count; ++iColumn)
        {
          migrantRange.ResetRowColumn(iRow, iColumn);
          TableItemStyle gridCellStyle = firstRow % 2 == 0 ? (num3 % 2 == 0 ? dataGrid.ItemStyle : dataGrid.AlternatingItemStyle) : (num3 % 2 == 1 ? dataGrid.ItemStyle : dataGrid.AlternatingItemStyle);
          this.SetWebGridStyle((IRange) migrantRange, gridCellStyle, dataGrid, false);
        }
      }
    }
  }

  private void SetWebGridStyle(
    IRange migrantRange,
    TableItemStyle gridCellStyle,
    System.Web.UI.WebControls.DataGrid dataGrid,
    bool isHeader)
  {
    IStyle cellStyle = migrantRange.CellStyle;
    cellStyle.BeginUpdate();
    if (!gridCellStyle.BackColor.IsEmpty)
      cellStyle.Color = gridCellStyle.BackColor;
    else if (!dataGrid.BackColor.IsEmpty)
      cellStyle.Color = dataGrid.BackColor;
    else
      cellStyle.Color = Color.White;
    IFont font1 = cellStyle.Font;
    font1.BeginUpdate();
    if (!gridCellStyle.ForeColor.IsEmpty)
      font1.RGBColor = gridCellStyle.ForeColor;
    else if (!dataGrid.ForeColor.IsEmpty)
      font1.RGBColor = dataGrid.ForeColor;
    FontInfo font2 = gridCellStyle.Font;
    if (font2.Bold)
      font1.Bold = font2.Bold;
    else if (dataGrid.Font.Bold)
      font1.Bold = dataGrid.Font.Bold;
    if (font2.Italic)
      font1.Italic = font2.Italic;
    else if (dataGrid.Font.Italic)
      font1.Italic = dataGrid.Font.Italic;
    if (font2.Strikeout)
      font1.Strikethrough = font2.Strikeout;
    else if (dataGrid.Font.Strikeout)
      font1.Strikethrough = dataGrid.Font.Strikeout;
    if (font2.Underline)
      font1.Underline = ExcelUnderline.Single;
    else if (dataGrid.Font.Underline)
      font1.Underline = ExcelUnderline.Single;
    if (font2.Name != string.Empty)
      font1.FontName = font2.Name;
    else if (dataGrid.Font.Name != string.Empty)
      font1.FontName = dataGrid.Font.Name;
    if (!font2.Size.IsEmpty)
      this.SetWebGridFontSize(font2, font1);
    else if (!dataGrid.Font.Size.IsEmpty)
      this.SetWebGridFontSize(dataGrid.Font, font1);
    font1.EndUpdate();
    cellStyle.EndUpdate();
    this.SetWebGridBorderStyle(gridCellStyle.BorderStyle, gridCellStyle.BorderColor, migrantRange);
  }

  private void SetWebGridBorderStyle(System.Web.UI.WebControls.BorderStyle borderStyle, Color borderColor, IRange range)
  {
    switch (borderStyle)
    {
      case System.Web.UI.WebControls.BorderStyle.None:
        range.BorderAround(ExcelLineStyle.None);
        break;
      case System.Web.UI.WebControls.BorderStyle.Dotted:
        range.BorderAround(ExcelLineStyle.Dotted, borderColor);
        break;
      case System.Web.UI.WebControls.BorderStyle.Double:
        range.BorderAround(ExcelLineStyle.Double, borderColor);
        break;
      default:
        range.BorderAround(ExcelLineStyle.Thin, borderColor);
        break;
    }
  }

  public void ImportGridView(
    GridView gridView,
    int firstRow,
    int firstColumn,
    bool isImportHeader,
    bool isImportStyle)
  {
    if (gridView == null)
      throw new ArgumentNullException(nameof (gridView));
    if (gridView.DataSource == null || gridView.DataSource == (object) string.Empty)
      throw new ArgumentNullException("dataSource");
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (firstColumn));
    int num1 = this.ImportDataSource(gridView.DataSource, firstRow, firstColumn, isImportHeader, gridView.DataMember, out int _);
    if (gridView.SortExpression != string.Empty)
    {
      IDataSort dataSorter = this.Workbook.CreateDataSorter();
      dataSorter.SortRange = this.Range[firstRow, firstColumn, firstRow + num1, firstColumn + gridView.Rows[0].Cells.Count];
      int num2 = 0;
      for (int column = firstColumn; column <= firstColumn + gridView.Rows[0].Cells.Count; ++column)
      {
        if (gridView.SortExpression == this.Range[firstRow, column].Text)
        {
          num2 = column;
          break;
        }
      }
      if (gridView.SortDirection == SortDirection.Ascending)
        dataSorter.SortFields.Add(num2 - 1, SortOn.Values, OrderBy.Ascending);
      else if (gridView.SortDirection == SortDirection.Ascending)
        dataSorter.SortFields.Add(num2 - 1, SortOn.Values, OrderBy.Descending);
      dataSorter.Sort();
    }
    if (!isImportStyle)
      return;
    int num3 = isImportHeader ? firstRow + num1 + 1 : firstRow + num1;
    for (int iRow = firstRow; iRow < num3; ++iRow)
    {
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this);
      if (iRow == firstRow && isImportHeader)
      {
        for (int iColumn = firstColumn; iColumn < firstColumn + gridView.Rows[0].Cells.Count; ++iColumn)
        {
          migrantRangeImpl.ResetRowColumn(iRow, iColumn);
          IStyle cellStyle = migrantRangeImpl.CellStyle;
          cellStyle.BeginUpdate();
          if (!gridView.HeaderRow.BackColor.IsEmpty)
            cellStyle.Color = gridView.HeaderRow.BackColor;
          else if (!gridView.HeaderStyle.BackColor.IsEmpty)
            cellStyle.Color = gridView.HeaderStyle.BackColor;
          else if (!gridView.BackColor.IsEmpty)
            cellStyle.Color = gridView.BackColor;
          else
            cellStyle.Color = Color.White;
          IFont font = cellStyle.Font;
          font.BeginUpdate();
          if (!gridView.HeaderRow.ForeColor.IsEmpty)
            font.RGBColor = gridView.HeaderRow.ForeColor;
          else if (!gridView.HeaderStyle.ForeColor.IsEmpty)
            font.RGBColor = gridView.HeaderStyle.ForeColor;
          else if (!gridView.ForeColor.IsEmpty)
            font.RGBColor = gridView.ForeColor;
          font.Bold = !gridView.HeaderRow.Font.Bold ? (!gridView.HeaderStyle.Font.Bold ? !gridView.Font.Bold || gridView.Font.Bold : gridView.HeaderStyle.Font.Bold) : gridView.HeaderRow.Font.Bold;
          if (gridView.HeaderRow.Font.Italic)
            font.Italic = gridView.HeaderRow.Font.Italic;
          else if (gridView.HeaderStyle.Font.Italic)
            font.Italic = gridView.HeaderStyle.Font.Italic;
          else if (gridView.Font.Italic)
            font.Italic = gridView.Font.Italic;
          if (gridView.HeaderRow.Font.Strikeout)
            font.Strikethrough = gridView.HeaderRow.Font.Strikeout;
          else if (gridView.HeaderStyle.Font.Strikeout)
            font.Strikethrough = gridView.HeaderStyle.Font.Strikeout;
          else if (gridView.Font.Strikeout)
            font.Strikethrough = gridView.Font.Strikeout;
          if (gridView.HeaderRow.Font.Underline)
            font.Underline = ExcelUnderline.Single;
          else if (gridView.HeaderStyle.Font.Underline)
            font.Underline = ExcelUnderline.Single;
          else if (gridView.Font.Underline)
            font.Underline = ExcelUnderline.Single;
          if (gridView.HeaderRow.Font.Name != string.Empty)
            font.FontName = gridView.HeaderRow.Font.Name;
          else if (gridView.HeaderStyle.Font.Name != string.Empty)
            font.FontName = gridView.HeaderStyle.Font.Name;
          else if (gridView.Font.Name != string.Empty)
            font.FontName = gridView.Font.Name;
          if (!gridView.HeaderRow.Font.Size.IsEmpty)
            this.SetWebGridFontSize(gridView.HeaderRow.Font, font);
          else if (!gridView.HeaderStyle.Font.Size.IsEmpty)
            this.SetWebGridFontSize(gridView.HeaderStyle.Font, font);
          else if (!gridView.Font.Size.IsEmpty)
            this.SetWebGridFontSize(gridView.Font, font);
          font.EndUpdate();
          cellStyle.EndUpdate();
          if (gridView.HeaderRow.BorderStyle != System.Web.UI.WebControls.BorderStyle.NotSet)
            this.SetWebGridBorderStyle(gridView.HeaderRow.BorderStyle, gridView.HeaderRow.BorderColor, (IRange) migrantRangeImpl);
          else if (gridView.HeaderStyle.BorderStyle != System.Web.UI.WebControls.BorderStyle.NotSet)
            this.SetWebGridBorderStyle(gridView.HeaderStyle.BorderStyle, gridView.HeaderStyle.BorderColor, (IRange) migrantRangeImpl);
          else
            migrantRangeImpl.BorderAround(ExcelLineStyle.Thin, Color.Black);
        }
      }
      else
      {
        int num4 = isImportHeader ? iRow - 1 : iRow;
        for (int iColumn = firstColumn; iColumn < firstColumn + gridView.Rows[0].Cells.Count; ++iColumn)
        {
          migrantRangeImpl.ResetRowColumn(iRow, iColumn);
          IStyle cellStyle = migrantRangeImpl.CellStyle;
          cellStyle.BeginUpdate();
          if (!gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].BackColor.IsEmpty)
            cellStyle.Color = gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].BackColor;
          else if (!gridView.Rows[num4 - firstRow].BackColor.IsEmpty)
            cellStyle.Color = gridView.Rows[num4 - firstRow].BackColor;
          else if (!gridView.AlternatingRowStyle.BackColor.IsEmpty && (num4 - firstRow) % 2 == 1)
            cellStyle.Color = gridView.AlternatingRowStyle.BackColor;
          else if (!gridView.RowStyle.BackColor.IsEmpty)
            cellStyle.Color = gridView.RowStyle.BackColor;
          else if (!gridView.BackColor.IsEmpty)
            cellStyle.Color = gridView.BackColor;
          else
            cellStyle.Color = Color.White;
          IFont font = cellStyle.Font;
          font.BeginUpdate();
          if (!gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].ForeColor.IsEmpty)
            font.RGBColor = gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].ForeColor;
          else if (!gridView.Rows[num4 - firstRow].ForeColor.IsEmpty)
            font.RGBColor = gridView.Rows[num4 - firstRow].ForeColor;
          else if (!gridView.AlternatingRowStyle.ForeColor.IsEmpty && (num4 - firstRow) % 2 == 1)
          {
            font.RGBColor = gridView.AlternatingRowStyle.ForeColor;
          }
          else
          {
            Color foreColor = gridView.RowStyle.ForeColor;
            font.RGBColor = foreColor.IsEmpty ? gridView.ForeColor : gridView.RowStyle.ForeColor;
          }
          if (gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].Font.Bold)
            font.Bold = gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].Font.Bold;
          else if (gridView.Rows[num4 - firstRow].Font.Bold)
            font.Bold = gridView.Rows[num4 - firstRow].Font.Bold;
          else if (gridView.AlternatingRowStyle.Font.Bold && (num4 - firstRow) % 2 == 1)
            font.Bold = gridView.AlternatingRowStyle.Font.Bold;
          else if (gridView.RowStyle.Font.Bold)
            font.Bold = gridView.RowStyle.Font.Bold;
          else if (gridView.Font.Bold)
            font.Bold = gridView.Font.Bold;
          if (gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].Font.Italic)
            font.Italic = gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].Font.Italic;
          else if (gridView.Rows[num4 - firstRow].Font.Italic)
            font.Italic = gridView.Rows[num4 - firstRow].Font.Italic;
          else if (gridView.AlternatingRowStyle.Font.Italic && (num4 - firstRow) % 2 == 1)
            font.Italic = gridView.AlternatingRowStyle.Font.Italic;
          else if (gridView.RowStyle.Font.Italic)
            font.Italic = gridView.RowStyle.Font.Italic;
          else if (gridView.Font.Italic)
            font.Italic = gridView.Font.Italic;
          if (gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].Font.Strikeout)
            font.Strikethrough = gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].Font.Strikeout;
          else if (gridView.Rows[num4 - firstRow].Font.Strikeout)
            font.Strikethrough = gridView.Rows[num4 - firstRow].Font.Strikeout;
          else if (gridView.AlternatingRowStyle.Font.Strikeout && (num4 - firstRow) % 2 == 1)
            font.Strikethrough = gridView.AlternatingRowStyle.Font.Strikeout;
          else if (gridView.RowStyle.Font.Strikeout)
            font.Strikethrough = gridView.RowStyle.Font.Strikeout;
          else if (gridView.Font.Strikeout)
            font.Strikethrough = gridView.Font.Strikeout;
          if (gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].Font.Underline)
            font.Underline = ExcelUnderline.Single;
          else if (gridView.Rows[num4 - firstRow].Font.Underline)
            font.Underline = ExcelUnderline.Single;
          else if (gridView.AlternatingRowStyle.Font.Underline && (num4 - firstRow) % 2 == 1)
            font.Underline = ExcelUnderline.Single;
          else if (gridView.RowStyle.Font.Bold)
            font.Underline = ExcelUnderline.Single;
          else if (gridView.Font.Underline)
            font.Underline = ExcelUnderline.Single;
          if (gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].Font.Name != string.Empty)
            font.FontName = gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].Font.Name;
          else if (gridView.Rows[num4 - firstRow].Font.Name != string.Empty)
            font.FontName = gridView.Rows[num4 - firstRow].Font.Name;
          else if (gridView.AlternatingRowStyle.Font.Name != string.Empty && (num4 - firstRow) % 2 == 1)
            font.FontName = gridView.AlternatingRowStyle.Font.Name;
          else if (gridView.RowStyle.Font.Name != string.Empty)
            font.FontName = gridView.RowStyle.Font.Name;
          else if (gridView.Font.Name != string.Empty)
            font.FontName = gridView.Font.Name;
          if (!gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].Font.Size.IsEmpty)
            this.SetWebGridFontSize(gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].Font, font);
          else if (!gridView.Rows[num4 - firstRow].Font.Size.IsEmpty)
            this.SetWebGridFontSize(gridView.Rows[num4 - firstRow].Font, font);
          else if (!gridView.AlternatingRowStyle.Font.Size.IsEmpty && (num4 - firstRow) % 2 == 1)
            this.SetWebGridFontSize(gridView.AlternatingRowStyle.Font, font);
          else if (!gridView.RowStyle.Font.Size.IsEmpty)
            this.SetWebGridFontSize(gridView.RowStyle.Font, font);
          else if (!gridView.Font.Size.IsEmpty)
            this.SetWebGridFontSize(gridView.Font, font);
          font.EndUpdate();
          cellStyle.EndUpdate();
          System.Web.UI.WebControls.BorderStyle borderStyle;
          Color borderColor;
          if (gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].BorderStyle != System.Web.UI.WebControls.BorderStyle.NotSet)
          {
            borderStyle = gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].BorderStyle;
            borderColor = gridView.Rows[num4 - firstRow].Cells[iColumn - firstColumn].BorderColor;
          }
          else if (gridView.Rows[num4 - firstRow].BorderStyle != System.Web.UI.WebControls.BorderStyle.NotSet)
          {
            borderStyle = gridView.Rows[num4 - firstRow].BorderStyle;
            borderColor = gridView.Rows[num4 - firstRow].BorderColor;
          }
          else if (gridView.AlternatingRowStyle.BorderStyle != System.Web.UI.WebControls.BorderStyle.NotSet && (num4 - firstRow) % 2 == 1)
          {
            borderStyle = gridView.AlternatingRowStyle.BorderStyle;
            borderColor = gridView.AlternatingRowStyle.BorderColor;
          }
          else if (gridView.RowStyle.BorderStyle != System.Web.UI.WebControls.BorderStyle.NotSet)
          {
            borderStyle = gridView.RowStyle.BorderStyle;
            borderColor = gridView.RowStyle.BorderColor;
          }
          else
          {
            borderStyle = gridView.BorderStyle;
            borderColor = gridView.BorderColor;
          }
          this.SetWebGridBorderStyle(borderStyle, borderColor, (IRange) migrantRangeImpl);
        }
      }
    }
  }

  public void ImportDataGridView(
    DataGridView dataGridView,
    int firstRow,
    int firstColumn,
    bool isImportHeader,
    bool isImportStyle)
  {
    if (dataGridView == null)
      throw new ArgumentNullException(nameof (dataGridView));
    if (dataGridView.DataSource == (object) string.Empty || dataGridView.Rows.Count < 1 && dataGridView.Rows[0].Cells.Count < 1)
      throw new ArgumentNullException("dataSource");
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (firstColumn));
    int num1 = 0;
    if (dataGridView.DataSource != null)
      num1 = this.ImportDataSource(dataGridView.DataSource, firstRow, firstColumn, isImportHeader, dataGridView.DataMember, out int _);
    else if (dataGridView.Rows.Count > 0 && dataGridView.Columns.Count > 0)
    {
      if (isImportHeader)
      {
        for (int index = 0; index < dataGridView.Columns.Count; ++index)
          this.SetText(firstRow, firstColumn + index, dataGridView.Columns[index].Name);
        ++firstRow;
      }
      for (int index1 = 0; index1 < dataGridView.Rows.Count - 1; ++index1)
      {
        for (int index2 = 0; index2 < dataGridView.Columns.Count; ++index2)
        {
          switch (dataGridView.Rows[index1].Cells[index2].ValueType.Name)
          {
            case "Boolean":
              this.SetBoolean(firstRow + index1, firstColumn + index2, (bool) dataGridView.Rows[index1].Cells[index2].Value);
              break;
            case "String":
              if (!this.CheckAndAddHyperlink((string) dataGridView.Rows[index1].Cells[index2].Value, this[firstRow + index1, firstColumn + index2] as RangeImpl))
              {
                this.SetString(firstRow + index1, firstColumn + index2, (string) dataGridView.Rows[index1].Cells[index2].Value);
                break;
              }
              break;
            case "Double":
              this.SetNumber(firstRow + index1, firstColumn + index2, (double) dataGridView.Rows[index1].Cells[index2].Value);
              break;
            case "DateTime":
              object obj = dataGridView.Rows[index1].Cells[index2].Value;
              if (obj is DateTime dateTime && dateTime >= RangeImpl.DEF_MIN_DATETIME)
              {
                this.IsImporting = true;
                MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this);
                migrantRangeImpl.ResetRowColumn(firstRow + index1, firstColumn + index2);
                migrantRangeImpl.Value2 = obj;
                this.IsImporting = false;
                break;
              }
              this.SetString(firstRow + index1, firstColumn + index2, obj.ToString());
              break;
            case "Int32":
              this.SetNumber(firstRow + index1, firstColumn + index2, (double) Convert.ToInt32(dataGridView.Rows[index1].Cells[index2].Value));
              break;
            default:
              this.SetValueRowCol(dataGridView.Rows[index1].Cells[index2].Value, firstRow + index1, firstColumn + index2);
              break;
          }
        }
      }
      num1 = dataGridView.Rows.Count - 1;
      if (isImportHeader)
        --firstRow;
    }
    if (dataGridView.SortedColumn != null)
    {
      IDataSort dataSorter = this.Workbook.CreateDataSorter();
      dataSorter.SortRange = this.Range[firstRow, firstColumn, firstRow + num1, firstColumn + dataGridView.Rows[0].Cells.Count];
      if (dataGridView.SortOrder == SortOrder.Ascending)
      {
        int num2 = firstColumn + dataGridView.SortedColumn.Index;
        dataSorter.SortFields.Add(num2 - 1, SortOn.Values, OrderBy.Ascending);
      }
      else if (dataGridView.SortOrder == SortOrder.Descending)
      {
        int num3 = firstColumn + dataGridView.SortedColumn.Index;
        dataSorter.SortFields.Add(num3 - 1, SortOn.Values, OrderBy.Descending);
      }
      dataSorter.Sort();
    }
    if (!isImportStyle)
      return;
    int num4 = isImportHeader ? firstRow + num1 + 1 : firstRow + num1;
    for (int iRow = firstRow; iRow < num4; ++iRow)
    {
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this);
      if (iRow == firstRow && isImportHeader)
      {
        for (int iColumn = firstColumn; iColumn < firstColumn + dataGridView.Rows[0].Cells.Count; ++iColumn)
        {
          migrantRangeImpl.ResetRowColumn(iRow, iColumn);
          IStyle cellStyle = migrantRangeImpl.CellStyle;
          cellStyle.BeginUpdate();
          DataGridViewCellStyle defaultCellStyle = dataGridView.ColumnHeadersDefaultCellStyle;
          cellStyle.Color = defaultCellStyle.BackColor;
          IFont font = cellStyle.Font;
          font.RGBColor = defaultCellStyle.ForeColor;
          if (defaultCellStyle.Font != null)
            this.SetGridFontStyle(defaultCellStyle.Font, font);
          font.EndUpdate();
          cellStyle.EndUpdate();
          if (dataGridView.ColumnHeadersBorderStyle == DataGridViewHeaderBorderStyle.None)
            migrantRangeImpl.BorderAround(ExcelLineStyle.None);
          else if (dataGridView.ColumnHeadersBorderStyle == DataGridViewHeaderBorderStyle.Custom)
            migrantRangeImpl.BorderAround(ExcelLineStyle.Thin);
          else
            migrantRangeImpl.BorderAround(ExcelLineStyle.Medium);
        }
      }
      else
      {
        int num5 = isImportHeader ? iRow - 1 : iRow;
        for (int iColumn = firstColumn; iColumn < firstColumn + dataGridView.Rows[0].Cells.Count; ++iColumn)
        {
          migrantRangeImpl.ResetRowColumn(iRow, iColumn);
          IStyle cellStyle = migrantRangeImpl.CellStyle;
          cellStyle.BeginUpdate();
          if (!dataGridView.Rows[num5 - firstRow].Cells[iColumn - firstColumn].Style.BackColor.IsEmpty)
            cellStyle.Color = dataGridView.Rows[num5 - firstRow].Cells[iColumn - firstColumn].Style.BackColor;
          else if (!dataGridView.Rows[num5 - firstRow].DefaultCellStyle.BackColor.IsEmpty)
            cellStyle.Color = dataGridView.Rows[num5 - firstRow].DefaultCellStyle.BackColor;
          else if (!dataGridView.AlternatingRowsDefaultCellStyle.BackColor.IsEmpty && (num5 - firstRow) % 2 == 1)
            cellStyle.Color = dataGridView.AlternatingRowsDefaultCellStyle.BackColor;
          else if (!dataGridView.RowsDefaultCellStyle.BackColor.IsEmpty)
            cellStyle.Color = dataGridView.RowsDefaultCellStyle.BackColor;
          else if (!dataGridView.Columns[iColumn - firstColumn].DefaultCellStyle.BackColor.IsEmpty)
            cellStyle.Color = dataGridView.Columns[iColumn - firstColumn].DefaultCellStyle.BackColor;
          else
            cellStyle.Color = dataGridView.DefaultCellStyle.BackColor;
          IFont font = cellStyle.Font;
          font.BeginUpdate();
          if (!dataGridView.Rows[num5 - firstRow].Cells[iColumn - firstColumn].Style.ForeColor.IsEmpty)
            font.RGBColor = dataGridView.Rows[num5 - firstRow].Cells[iColumn - firstColumn].Style.ForeColor;
          else if (!dataGridView.Rows[num5 - firstRow].DefaultCellStyle.ForeColor.IsEmpty)
            font.RGBColor = dataGridView.Rows[num5 - firstRow].DefaultCellStyle.ForeColor;
          else if (!dataGridView.AlternatingRowsDefaultCellStyle.ForeColor.IsEmpty && (num5 - firstRow) % 2 == 1)
            font.RGBColor = dataGridView.AlternatingRowsDefaultCellStyle.ForeColor;
          else if (!dataGridView.RowsDefaultCellStyle.ForeColor.IsEmpty)
          {
            font.RGBColor = dataGridView.RowsDefaultCellStyle.ForeColor;
          }
          else
          {
            Color foreColor = dataGridView.Columns[iColumn - firstColumn].DefaultCellStyle.ForeColor;
            font.RGBColor = foreColor.IsEmpty ? dataGridView.DefaultCellStyle.ForeColor : dataGridView.Columns[iColumn - firstColumn].DefaultCellStyle.ForeColor;
          }
          this.SetGridFontStyle((dataGridView.Rows[num5 - firstRow].Cells[iColumn - firstColumn].Style.Font == null ? (dataGridView.Rows[num5 - firstRow].DefaultCellStyle.Font == null ? (dataGridView.AlternatingRowsDefaultCellStyle.Font == null || (num5 - firstRow) % 2 != 1 ? (dataGridView.RowsDefaultCellStyle.Font == null ? (dataGridView.Columns[iColumn - firstColumn].DefaultCellStyle.Font == null ? dataGridView.DefaultCellStyle : dataGridView.Columns[iColumn - firstColumn].DefaultCellStyle) : dataGridView.RowsDefaultCellStyle) : dataGridView.AlternatingRowsDefaultCellStyle) : dataGridView.Rows[num5 - firstRow].DefaultCellStyle) : dataGridView.Rows[num5 - firstRow].Cells[iColumn - firstColumn].Style).Font, font);
          font.EndUpdate();
          cellStyle.EndUpdate();
          if (dataGridView.CellBorderStyle == DataGridViewCellBorderStyle.None)
            migrantRangeImpl.BorderAround(ExcelLineStyle.None);
          else if (dataGridView.CellBorderStyle == DataGridViewCellBorderStyle.Custom)
            migrantRangeImpl.BorderAround(ExcelLineStyle.Thin);
          else
            migrantRangeImpl.BorderAround(ExcelLineStyle.Medium);
        }
      }
    }
  }

  public int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn)
  {
    return this.ImportDataView(dataView, isFieldNameShown, firstRow, firstColumn, false);
  }

  public int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    bool bPreserveTypes)
  {
    if (dataView == null)
      throw new ArgumentNullException(nameof (dataView));
    return dataView.Table == null ? 0 : this.ImportDataView(dataView, isFieldNameShown, firstRow, firstColumn, dataView.Count, dataView.Table.Columns.Count, bPreserveTypes);
  }

  public int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns)
  {
    return this.ImportDataView(dataView, isFieldNameShown, firstRow, firstColumn, maxRows, maxColumns, false);
  }

  public int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    bool bPreserveTypes)
  {
    if (dataView == null)
      throw new ArgumentNullException(nameof (dataView));
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (firstColumn));
    this.ParseData();
    DataColumnCollection columns = dataView.Table.Columns;
    int count1 = dataView.Count;
    if (maxRows < 0 || maxRows > count1)
      maxRows = count1;
    int count2 = columns.Count;
    if (maxColumns < 0 || maxColumns > count2)
      maxColumns = count2;
    maxColumns = Math.Min(maxColumns, this.m_book.MaxColumnCount - firstColumn + 1);
    if (isFieldNameShown)
    {
      for (int index = 0; index < maxColumns; ++index)
        this.Range[firstRow, firstColumn + index].Value2 = (object) columns[index].Caption;
      ++firstRow;
    }
    maxRows = Math.Min(maxRows, this.m_book.MaxRowCount - firstRow + 1);
    if (!bPreserveTypes)
      this.ImportDataViewWithoutCheck(dataView, firstRow, firstColumn, maxRows, maxColumns);
    else
      this.ImportDataViewWithoutCheckPreserve(dataView, firstRow, firstColumn, maxRows, maxColumns);
    return maxRows;
  }

  public DataTable ExportDataTable(
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    ExcelExportDataTableOptions options)
  {
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (firstColumn));
    this.ParseData();
    int lastRow = this.UsedRange.LastRow;
    if (firstRow > lastRow)
      maxRows = firstRow + 1;
    if (maxRows > lastRow)
      maxRows = lastRow;
    bool flag1 = (options & ExcelExportDataTableOptions.ColumnNames) != ExcelExportDataTableOptions.None;
    bool bExportFormulaValues = (options & ExcelExportDataTableOptions.ComputedFormulaValues) != ExcelExportDataTableOptions.None;
    bool flag2 = (options & ExcelExportDataTableOptions.DetectColumnTypes) != ExcelExportDataTableOptions.None;
    bool flag3 = (options & ExcelExportDataTableOptions.DefaultStyleColumnTypes) != ExcelExportDataTableOptions.None;
    bool preserveOLEDate = (options & ExcelExportDataTableOptions.PreserveOleDate) != ExcelExportDataTableOptions.None;
    bool flag4 = (options & ExcelExportDataTableOptions.ExportHiddenColumns) != ExcelExportDataTableOptions.None;
    bool flag5 = (options & ExcelExportDataTableOptions.ExportHiddenRows) != ExcelExportDataTableOptions.None;
    bool flag6 = (options & ExcelExportDataTableOptions.DetectMixedValueType) != ExcelExportDataTableOptions.DetectColumnTypes;
    DataTable dataTable = new DataTable(this.Name);
    maxColumns = Math.Min(maxColumns, this.m_book.MaxColumnCount - firstColumn + 1);
    maxRows = Math.Min(maxRows, this.m_book.MaxRowCount - firstRow + (flag1 ? 2 : 1));
    ExcelExportType[] excelExportTypeArray = flag2 ? new ExcelExportType[maxColumns] : (ExcelExportType[]) null;
    int num1 = flag1 ? firstRow + 1 : firstRow;
    List<double> doubleList = new List<double>(this.Columns.Length);
    IMigrantRange migrantRange = (IMigrantRange) new MigrantRangeImpl(this.Application, (IWorksheet) this);
    for (int index = 0; index < maxColumns; ++index)
    {
      DataColumn column = new DataColumn();
      System.Type type = typeof (string);
      migrantRange.ResetRowColumn(firstRow, firstColumn + index);
      double columnWidth = migrantRange.ColumnWidth;
      doubleList.Add(columnWidth);
      if (flag4 || columnWidth > 0.0)
      {
        if (flag2)
        {
          ExcelExportType exportType = this.GetExportType(this.GetFormatType(num1, firstColumn + index, flag3), num1, firstColumn + index, maxRows, options, out System.Type _);
          type = this.GetType(exportType, preserveOLEDate);
          if (flag6 && exportType != ExcelExportType.Text || type == typeof (DateTime))
          {
            exportType = this.GetNextExportType(num1, firstColumn + index, flag3, exportType, maxRows, options);
            if (exportType == ExcelExportType.Text)
              type = typeof (string);
          }
          excelExportTypeArray[index] = exportType;
        }
        column.DataType = type;
        string str = migrantRange.DisplayText;
        if ((options & ExcelExportDataTableOptions.TrimColumnNames) != ExcelExportDataTableOptions.None)
          str = str.Trim();
        if (!TimeSpan.TryParse(str, out TimeSpan _) && DateTime.TryParse(str, out DateTime _))
        {
          DateTime dateTime = migrantRange.DateTime;
          str = migrantRange.DateTime.ToString();
        }
        if (dataTable.Columns.Contains(str))
          column.ColumnName = str + (object) index;
        else if (flag1)
          column.ColumnName = str;
        dataTable.Columns.Add(column);
      }
    }
    firstRow = num1;
    if (flag1)
      --maxRows;
    bool flag7 = false;
    this.m_bIsExportDataTable = true;
    if (this.HasSheetCalculation)
      this.m_book.CalcEngineMemberValuesOnSheet(false);
    else if (this.IsSaved || this.m_book.IsCellModified || this.m_book.BuiltInDocumentProperties.ApplicationName != "Microsoft Excel")
    {
      this.EnableSheetCalculations();
      this.CalcEngine.UseNoAmpersandQuotes = true;
      this.m_book.CalcEngineMemberValuesOnSheet(false);
      flag7 = true;
    }
    bool flag8 = false;
    bool flag9 = false;
    ExportDataTableEventArgs dataTableEventArgs = (ExportDataTableEventArgs) null;
    for (int index1 = 0; index1 < maxRows; ++index1)
    {
      DataRow row = dataTable.NewRow();
      int num2 = 0;
      if (flag5 || this.Range[firstRow + index1, firstColumn].RowHeight > 0.0)
      {
        for (int index2 = 0; index2 < maxColumns; ++index2)
        {
          if (flag4 || doubleList[index2] > 0.0)
          {
            ExcelExportType formatType = excelExportTypeArray != null ? excelExportTypeArray[index2] : ExcelExportType.Text;
            object excelCellValue = this.GetValue(firstRow + index1, firstColumn + index2, formatType, bExportFormulaValues, preserveOLEDate);
            dataTableEventArgs = this.OnExportDataTableInfo(firstRow + index1, firstColumn + index2, num2, this[firstRow + index1, firstColumn + index2], dataTable.Columns[num2].DataType, excelCellValue);
            flag8 = dataTableEventArgs != null && dataTableEventArgs.ExportDataTableAction == ExportDataTableActions.SkipRow;
            flag9 = dataTableEventArgs != null && dataTableEventArgs.ExportDataTableAction == ExportDataTableActions.StopExporting;
            if (!flag9 && !flag8)
            {
              row[num2] = dataTableEventArgs == null || dataTableEventArgs.DataTableValue == null ? excelCellValue : dataTableEventArgs.DataTableValue;
              ++num2;
            }
            else
              break;
          }
        }
        if (dataTableEventArgs == null || !flag9)
        {
          if (dataTableEventArgs == null || !flag8)
          {
            dataTable.Rows.Add(row);
            flag9 = false;
            flag8 = false;
          }
        }
        else
          break;
      }
    }
    if (this.HasSheetCalculation)
      this.m_book.CalcEngineMemberValuesOnSheet(true);
    else if (flag7)
    {
      this.m_book.CalcEngineMemberValuesOnSheet(true);
      this.DisableSheetCalculations();
    }
    this.m_bIsExportDataTable = false;
    return dataTable;
  }

  internal ExportDataTableEventArgs OnExportDataTableInfo(
    int excelRowIndex,
    int excelColumnIndex,
    int dataTableColumnIndex,
    IRange cellRange,
    System.Type dataTableColumnType,
    object excelCellValue)
  {
    if (this.ExportDataTableEvent == null)
      return (ExportDataTableEventArgs) null;
    ExportDataTableEventArgs exportDataTableEventSettings = new ExportDataTableEventArgs();
    exportDataTableEventSettings.ExcelRowIndex = excelRowIndex;
    exportDataTableEventSettings.ExcelColumnIndex = excelColumnIndex;
    exportDataTableEventSettings.DataTableColumnIndex = dataTableColumnIndex;
    exportDataTableEventSettings.CellRange = cellRange;
    string str = excelCellValue.ToString();
    if (!string.IsNullOrEmpty(str))
      exportDataTableEventSettings.ExcelValue = (object) str;
    exportDataTableEventSettings.ColumnType = dataTableColumnType;
    this.ExportDataTableEvent(exportDataTableEventSettings);
    return exportDataTableEventSettings;
  }

  public DataTable ExportDataTable(
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    ExcelExportDataTableOptions options,
    PivotTableImpl pivotTable)
  {
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (firstColumn));
    this.ParseData();
    bool flag1 = (options & ExcelExportDataTableOptions.ColumnNames) != ExcelExportDataTableOptions.None;
    bool bExportFormulaValues = (options & ExcelExportDataTableOptions.ComputedFormulaValues) != ExcelExportDataTableOptions.None;
    bool flag2 = (options & ExcelExportDataTableOptions.DetectColumnTypes) != ExcelExportDataTableOptions.None;
    bool flag3 = (options & ExcelExportDataTableOptions.DefaultStyleColumnTypes) != ExcelExportDataTableOptions.None;
    bool preserveOLEDate = (options & ExcelExportDataTableOptions.PreserveOleDate) != ExcelExportDataTableOptions.None;
    bool flag4 = (options & ExcelExportDataTableOptions.DetectMixedValueType) != ExcelExportDataTableOptions.DetectColumnTypes;
    DataTable dataTable = new DataTable(this.Name);
    List<string> stringList = new List<string>();
    maxColumns = Math.Min(maxColumns, this.m_book.MaxColumnCount - firstColumn + 1);
    maxRows = Math.Min(maxRows + (flag1 ? 1 : 0), this.m_book.MaxRowCount - firstRow + (flag1 ? 2 : 1));
    ExcelExportType[] excelExportTypeArray = flag2 ? new ExcelExportType[maxColumns] : (ExcelExportType[]) null;
    int num1 = flag1 ? firstRow + 1 : firstRow;
    for (int index = 0; index < maxColumns; ++index)
    {
      DataColumn column = new DataColumn();
      System.Type type = typeof (string);
      if (flag2)
      {
        ExcelExportType exportType = this.GetExportType(this.GetFormatType(num1, firstColumn + index, flag3), num1, firstColumn + index, maxRows, options, out System.Type _);
        type = this.GetType(exportType, preserveOLEDate);
        if (flag4 && exportType != ExcelExportType.Text || type == typeof (DateTime))
        {
          exportType = this.GetNextExportType(num1, firstColumn + index, flag3, exportType, maxRows, options);
          if (exportType == ExcelExportType.Text)
            type = typeof (string);
        }
        excelExportTypeArray[index] = exportType;
      }
      column.DataType = type;
      if (flag1)
      {
        column.ColumnName = this.Range[firstRow, firstColumn + index].Value;
        int num2 = 1;
        string columnName = column.ColumnName;
        for (; stringList.Contains(column.ColumnName); column.ColumnName = columnName + num2.ToString())
          ++num2;
        stringList.Add(column.ColumnName);
      }
      dataTable.Columns.Add(column);
    }
    for (int i = 0; i < pivotTable.DataFields.Count; ++i)
    {
      if (pivotTable.DataFields[i].Subtotal == PivotSubtotalTypes.Count)
      {
        int index = pivotTable.DataFields[i].Field.CacheField.Index;
        if (dataTable.Columns[index].DataType == typeof (double))
          dataTable.Columns[index].DataType = typeof (int);
      }
    }
    firstRow = num1;
    if (flag1)
      --maxRows;
    bool flag5 = false;
    bool flag6 = false;
    ExportDataTableEventArgs dataTableEventArgs = (ExportDataTableEventArgs) null;
    for (int index1 = 0; index1 < maxRows; ++index1)
    {
      DataRow row = dataTable.NewRow();
      for (int index2 = 0; index2 < maxColumns; ++index2)
      {
        ExcelExportType formatType = excelExportTypeArray != null ? excelExportTypeArray[index2] : ExcelExportType.Text;
        object excelCellValue = this.GetValue(firstRow + index1, firstColumn + index2, formatType, bExportFormulaValues, preserveOLEDate);
        dataTableEventArgs = this.OnExportDataTableInfo(firstRow + index1, firstColumn + index2, index2, this[firstRow + index1, firstColumn + index2], dataTable.Columns[index2].DataType, excelCellValue);
        flag5 = dataTableEventArgs != null && dataTableEventArgs.ExportDataTableAction == ExportDataTableActions.SkipRow;
        flag6 = dataTableEventArgs != null && dataTableEventArgs.ExportDataTableAction == ExportDataTableActions.StopExporting;
        if (!flag6 && !flag5)
          row[index2] = dataTableEventArgs == null || dataTableEventArgs.DataTableValue == null ? excelCellValue : dataTableEventArgs.DataTableValue;
        else
          break;
      }
      if (dataTableEventArgs == null || !flag6)
      {
        if (dataTableEventArgs == null || !flag5)
        {
          dataTable.Rows.Add(row);
          flag6 = false;
          flag5 = false;
        }
      }
      else
        break;
    }
    return dataTable;
  }

  internal ExcelExportType GetNextExportType(
    int iFirstDataRow,
    int firstColumn,
    bool bUseDefaultStyles,
    ExcelExportType exportType,
    int maxRows,
    ExcelExportDataTableOptions options)
  {
    for (int index = iFirstDataRow + 1; index <= maxRows; ++index)
    {
      if (this.GetCellType(index, firstColumn, false) != WorksheetImpl.TRangeValueType.Blank)
      {
        ExcelFormatType formatType = this.GetFormatType(index, firstColumn, bUseDefaultStyles);
        ExcelExportType exportType1 = this.GetExportType(formatType, index, firstColumn, maxRows, options, out System.Type _);
        if (((options & ExcelExportDataTableOptions.DetectMixedValueType) != ExcelExportDataTableOptions.DetectColumnTypes ? (exportType1 != exportType ? 1 : 0) : (exportType1 == ExcelExportType.Text ? 1 : 0)) != 0 || formatType == ExcelFormatType.General)
        {
          exportType = ExcelExportType.Text;
          break;
        }
      }
    }
    return exportType;
  }

  public DataTable ExportDataTable(IRange range, ExcelExportDataTableOptions options)
  {
    int firstRow = range != null ? range.Row : throw new ArgumentNullException(nameof (range));
    int column = range.Column;
    return firstRow == 0 || column == 0 ? (DataTable) null : this.ExportDataTable(firstRow, column, range.LastRow - firstRow + 1, range.LastColumn - column + 1, options);
  }

  public DataTable PEExportDataTable(
    IRange range,
    ExcelExportDataTableOptions options,
    PivotTableImpl pivotTable)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    int num = (options & ExcelExportDataTableOptions.ColumnNames) != ExcelExportDataTableOptions.None ? 0 : 1;
    int row = range.Row;
    int column = range.Column;
    return row == 0 || column == 0 ? (DataTable) null : this.ExportDataTable(row, column, range.LastRow - row + num, range.LastColumn - column + 1, options, pivotTable);
  }

  public List<T> ExportData<T>(int firstRow, int firstColumn, int lastRow, int lastColumn) where T : new()
  {
    return this.ExportData<T>(firstRow, firstColumn, lastRow, lastColumn, (Dictionary<string, string>) null);
  }

  public List<T> ExportData<T>(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    Dictionary<string, string> mappingProperties)
    where T : new()
  {
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (firstColumn));
    if (lastRow <= firstRow || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (lastRow));
    if (lastColumn < firstColumn || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (lastColumn));
    List<T> objList = new List<T>(lastRow - firstRow);
    MigrantRangeImpl exportDataMigrant = new MigrantRangeImpl(this.Application, (IWorksheet) this);
    bool flag = false;
    if (!this.m_book.EnabledCalcEngine)
    {
      this.EnableSheetCalculations();
      flag = true;
    }
    Dictionary<int, string> exportColumns = new Dictionary<int, string>();
    System.Type classType = typeof (T);
    this.ExtractPropertyNames(classType, firstRow, firstColumn, lastColumn, exportColumns, exportDataMigrant, mappingProperties, "");
    for (int currentRow = firstRow + 1; currentRow <= lastRow; ++currentRow)
    {
      int columnCount = 0;
      object propertyValue = this.GetPropertyValue(classType, currentRow, 1, exportColumns, exportDataMigrant, string.Empty, objList.Count, out columnCount);
      objList.Add((T) propertyValue);
    }
    if (flag)
      this.DisableSheetCalculations();
    return objList;
  }

  private void ExtractPropertyNames(
    System.Type classType,
    int firstRow,
    int firstColumn,
    int lastColumn,
    Dictionary<int, string> exportColumns,
    MigrantRangeImpl exportDataMigrant,
    Dictionary<string, string> mappingProperties,
    string parentName)
  {
    List<PropertyInfo> propertyInfo = (List<PropertyInfo>) null;
    List<TypeCode> objectMembersInfo = this.GetObjectMembersInfo(classType, out propertyInfo);
    PropertyInfo[] array = propertyInfo.ToArray();
    string empty = string.Empty;
    for (int index1 = firstColumn; index1 <= lastColumn; ++index1)
    {
      exportDataMigrant.ResetRowColumn(firstRow, index1);
      string key = exportDataMigrant.Text;
      string str1 = string.Empty;
      if (mappingProperties != null && !string.IsNullOrEmpty(key) && mappingProperties.ContainsKey(key))
        key = mappingProperties[key];
      if (!string.IsNullOrEmpty(key))
      {
        for (int index2 = 0; index2 < array.Length; ++index2)
        {
          string name = array[index2].Name;
          string str2 = parentName + name;
          if (key.Equals(str2, StringComparison.OrdinalIgnoreCase) && !exportColumns.ContainsKey(index1))
          {
            exportColumns.Add(index1, str2);
            break;
          }
          object[] customAttributes = array[index2].GetCustomAttributes(typeof (DisplayNameAttribute), true);
          if (customAttributes != null)
          {
            foreach (Attribute attribute in customAttributes)
            {
              if (attribute is DisplayNameAttribute displayNameAttribute && !string.IsNullOrEmpty(displayNameAttribute.DisplayName))
              {
                str1 = displayNameAttribute.DisplayName;
                if (key.Equals(str1, StringComparison.OrdinalIgnoreCase) && !exportColumns.ContainsKey(index1))
                {
                  exportColumns.Add(index1, str2);
                  break;
                }
              }
            }
          }
          if (!exportColumns.ContainsKey(index1))
          {
            if ((key.Contains(str2) || !string.IsNullOrEmpty(str1) && key.Contains(str1)) && objectMembersInfo[index2] == TypeCode.Object)
            {
              System.Type propertyType = array[index2].PropertyType;
              if (propertyType.Namespace != null && !propertyType.Namespace.Contains("System"))
                this.ExtractPropertyNames(propertyType, firstRow, index1, lastColumn, exportColumns, exportDataMigrant, mappingProperties, str2 + ".");
            }
          }
          else
            break;
        }
      }
    }
  }

  private object GetPropertyValue(
    System.Type classType,
    int currentRow,
    int currentColumn,
    Dictionary<int, string> exportColumns,
    MigrantRangeImpl exportDataMigrant,
    string parentName,
    int mismatchRecordIndex,
    out int columnCount)
  {
    object instance1 = Activator.CreateInstance(classType);
    int currentColumn1 = 0;
    int num = 0;
    int columnCount1 = 0;
    foreach (KeyValuePair<int, string> exportColumn in exportColumns)
    {
      ++currentColumn1;
      if (currentColumn1 >= currentColumn)
      {
        string name1 = exportColumn.Value;
        if (!string.IsNullOrEmpty(parentName) && name1.Contains(parentName))
          name1 = name1.Replace(parentName, "");
        PropertyInfo property = classType.GetProperty(name1);
        object obj1 = (object) null;
        if (property == (PropertyInfo) null)
        {
          if (name1.IndexOf('.') > 0)
          {
            int length = name1.IndexOf('.');
            string name2 = name1.Substring(0, length);
            property = classType.GetProperty(name2);
            if (!(property == (PropertyInfo) null))
            {
              obj1 = this.GetPropertyValue(property.PropertyType, currentRow, currentColumn1, exportColumns, exportDataMigrant, $"{parentName}{property.Name}.", mismatchRecordIndex, out columnCount1);
              --num;
              currentColumn = currentColumn1 + columnCount1;
            }
            else
              continue;
          }
          else
            continue;
        }
        ++num;
        TypeCode typeCode = !property.PropertyType.IsGenericType || !(property.PropertyType.GetGenericTypeDefinition() == typeof (Nullable<>)) ? System.Type.GetTypeCode(property.PropertyType) : System.Type.GetTypeCode(Nullable.GetUnderlyingType(property.PropertyType));
        exportDataMigrant.ResetRowColumn(currentRow, exportColumn.Key);
        if (!exportDataMigrant.IsBlank && (property.PropertyType.IsValueType || property.PropertyType.IsClass))
        {
          if (property.PropertyType.IsEnum)
          {
            property.SetValue(instance1, Enum.Parse(property.PropertyType, exportDataMigrant.DisplayText, true), (object[]) null);
          }
          else
          {
            WorksheetImpl.TRangeValueType cellType = this.GetCellType(exportDataMigrant.Row, exportDataMigrant.Column, true);
            if (exportDataMigrant.HasFormula)
              RangeImpl.UpdateCellValue((object) this, exportDataMigrant.Column, exportDataMigrant.Row, true);
            switch (typeCode)
            {
              case TypeCode.Object:
                if (property.PropertyType.GetInterface("IHyperLink", false) != (System.Type) null && exportDataMigrant.Hyperlinks.Count > 0)
                {
                  IHyperLink instance2 = Activator.CreateInstance(property.PropertyType) as IHyperLink;
                  IHyperLink hyperlink = exportDataMigrant.Hyperlinks[0];
                  if (hyperlink.Type != ExcelHyperLinkType.None)
                    instance2.Type = hyperlink.Type;
                  if (hyperlink.Address != null)
                    instance2.Address = hyperlink.Address;
                  if (hyperlink.ScreenTip != null)
                    instance2.ScreenTip = hyperlink.ScreenTip;
                  if (hyperlink.SubAddress != null)
                    instance2.SubAddress = hyperlink.SubAddress;
                  if (hyperlink.TextToDisplay != null)
                    instance2.TextToDisplay = hyperlink.TextToDisplay;
                  property.SetValue(instance1, (object) instance2, (object[]) null);
                  continue;
                }
                if (property.PropertyType.Name == "TimeSpan")
                {
                  if (exportDataMigrant.HasFormulaDateTime)
                  {
                    property.SetValue(instance1, (object) exportDataMigrant.FormulaDateTime.TimeOfDay, (object[]) null);
                    continue;
                  }
                  property.SetValue(instance1, (object) exportDataMigrant.TimeSpan, (object[]) null);
                  continue;
                }
                if (property.PropertyType.Name == "Object")
                {
                  if (exportDataMigrant.HasFormulaStringValue)
                  {
                    property.SetValue(instance1, (object) exportDataMigrant.FormulaStringValue, (object[]) null);
                    continue;
                  }
                  if (exportDataMigrant.HasFormulaNumberValue)
                  {
                    property.SetValue(instance1, (object) exportDataMigrant.FormulaNumberValue, (object[]) null);
                    continue;
                  }
                  if (exportDataMigrant.HasFormulaBoolValue)
                  {
                    property.SetValue(instance1, (object) exportDataMigrant.FormulaBoolValue, (object[]) null);
                    continue;
                  }
                  if (exportDataMigrant.HasFormulaDateTime)
                  {
                    property.SetValue(instance1, (object) exportDataMigrant.FormulaDateTime, (object[]) null);
                    continue;
                  }
                  property.SetValue(instance1, exportDataMigrant.Value2, (object[]) null);
                  continue;
                }
                if (obj1 != null)
                {
                  property.SetValue(instance1, obj1, (object[]) null);
                  continue;
                }
                continue;
              case TypeCode.Boolean:
                switch (cellType)
                {
                  case WorksheetImpl.TRangeValueType.Boolean:
                    property.SetValue(instance1, (object) exportDataMigrant.Boolean, (object[]) null);
                    continue;
                  case WorksheetImpl.TRangeValueType.Boolean | WorksheetImpl.TRangeValueType.Formula:
                    property.SetValue(instance1, (object) exportDataMigrant.FormulaBoolValue, (object[]) null);
                    continue;
                  default:
                    object obj2 = this.NotifyAndSetUserValue(property.PropertyType, (IRange) exportDataMigrant, property.Name, cellType.ToString(), mismatchRecordIndex);
                    if (obj2 != null && obj2 is bool flag)
                    {
                      property.SetValue(instance1, (object) flag, (object[]) null);
                      continue;
                    }
                    continue;
                }
              case TypeCode.Char:
                if (exportDataMigrant.DisplayText.Length >= 1)
                {
                  property.SetValue(instance1, (object) Convert.ToChar(exportDataMigrant.DisplayText.Substring(0, 1)), (object[]) null);
                  continue;
                }
                object obj3 = this.NotifyAndSetUserValue(property.PropertyType, (IRange) exportDataMigrant, property.Name, cellType.ToString(), mismatchRecordIndex);
                if (obj3 != null && obj3 is char ch)
                {
                  property.SetValue(instance1, (object) ch, (object[]) null);
                  continue;
                }
                continue;
              case TypeCode.SByte:
                switch (cellType)
                {
                  case WorksheetImpl.TRangeValueType.Number:
                    property.SetValue(instance1, (object) (sbyte) exportDataMigrant.Number, (object[]) null);
                    continue;
                  case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
                    property.SetValue(instance1, (object) (sbyte) exportDataMigrant.FormulaNumberValue, (object[]) null);
                    continue;
                  default:
                    object obj4 = this.NotifyAndSetUserValue(property.PropertyType, (IRange) exportDataMigrant, property.Name, cellType.ToString(), mismatchRecordIndex);
                    double result1;
                    if (obj4 != null && double.TryParse(obj4.ToString(), out result1))
                    {
                      property.SetValue(instance1, (object) (sbyte) result1, (object[]) null);
                      continue;
                    }
                    continue;
                }
              case TypeCode.Byte:
                switch (cellType)
                {
                  case WorksheetImpl.TRangeValueType.Number:
                    property.SetValue(instance1, (object) (byte) exportDataMigrant.Number, (object[]) null);
                    continue;
                  case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
                    property.SetValue(instance1, (object) (byte) exportDataMigrant.FormulaNumberValue, (object[]) null);
                    continue;
                  default:
                    object obj5 = this.NotifyAndSetUserValue(property.PropertyType, (IRange) exportDataMigrant, property.Name, cellType.ToString(), mismatchRecordIndex);
                    double result2;
                    if (obj5 != null && double.TryParse(obj5.ToString(), out result2))
                    {
                      property.SetValue(instance1, (object) (byte) result2, (object[]) null);
                      continue;
                    }
                    continue;
                }
              case TypeCode.Int16:
                switch (cellType)
                {
                  case WorksheetImpl.TRangeValueType.Number:
                    property.SetValue(instance1, (object) (short) exportDataMigrant.Number, (object[]) null);
                    continue;
                  case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
                    property.SetValue(instance1, (object) (short) exportDataMigrant.FormulaNumberValue, (object[]) null);
                    continue;
                  default:
                    object obj6 = this.NotifyAndSetUserValue(property.PropertyType, (IRange) exportDataMigrant, property.Name, cellType.ToString(), mismatchRecordIndex);
                    double result3;
                    if (obj6 != null && double.TryParse(obj6.ToString(), out result3))
                    {
                      property.SetValue(instance1, (object) (short) result3, (object[]) null);
                      continue;
                    }
                    continue;
                }
              case TypeCode.UInt16:
                switch (cellType)
                {
                  case WorksheetImpl.TRangeValueType.Number:
                    property.SetValue(instance1, (object) (ushort) exportDataMigrant.Number, (object[]) null);
                    continue;
                  case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
                    property.SetValue(instance1, (object) (ushort) exportDataMigrant.FormulaNumberValue, (object[]) null);
                    continue;
                  default:
                    object obj7 = this.NotifyAndSetUserValue(property.PropertyType, (IRange) exportDataMigrant, property.Name, cellType.ToString(), mismatchRecordIndex);
                    double result4;
                    if (obj7 != null && double.TryParse(obj7.ToString(), out result4))
                    {
                      property.SetValue(instance1, (object) (ushort) result4, (object[]) null);
                      continue;
                    }
                    continue;
                }
              case TypeCode.Int32:
                switch (cellType)
                {
                  case WorksheetImpl.TRangeValueType.Number:
                    property.SetValue(instance1, (object) (int) exportDataMigrant.Number, (object[]) null);
                    continue;
                  case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
                    property.SetValue(instance1, (object) (int) exportDataMigrant.FormulaNumberValue, (object[]) null);
                    continue;
                  default:
                    object obj8 = this.NotifyAndSetUserValue(property.PropertyType, (IRange) exportDataMigrant, property.Name, cellType.ToString(), mismatchRecordIndex);
                    double result5;
                    if (obj8 != null && double.TryParse(obj8.ToString(), out result5))
                    {
                      property.SetValue(instance1, (object) (int) result5, (object[]) null);
                      continue;
                    }
                    continue;
                }
              case TypeCode.UInt32:
                switch (cellType)
                {
                  case WorksheetImpl.TRangeValueType.Number:
                    property.SetValue(instance1, (object) (uint) exportDataMigrant.Number, (object[]) null);
                    continue;
                  case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
                    property.SetValue(instance1, (object) (uint) exportDataMigrant.FormulaNumberValue, (object[]) null);
                    continue;
                  default:
                    object obj9 = this.NotifyAndSetUserValue(property.PropertyType, (IRange) exportDataMigrant, property.Name, cellType.ToString(), mismatchRecordIndex);
                    double result6;
                    if (obj9 != null && double.TryParse(obj9.ToString(), out result6))
                    {
                      property.SetValue(instance1, (object) (uint) result6, (object[]) null);
                      continue;
                    }
                    continue;
                }
              case TypeCode.Int64:
                switch (cellType)
                {
                  case WorksheetImpl.TRangeValueType.Number:
                    property.SetValue(instance1, (object) (long) exportDataMigrant.Number, (object[]) null);
                    continue;
                  case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
                    property.SetValue(instance1, (object) (long) exportDataMigrant.FormulaNumberValue, (object[]) null);
                    continue;
                  default:
                    object obj10 = this.NotifyAndSetUserValue(property.PropertyType, (IRange) exportDataMigrant, property.Name, cellType.ToString(), mismatchRecordIndex);
                    double result7;
                    if (obj10 != null && double.TryParse(obj10.ToString(), out result7))
                    {
                      property.SetValue(instance1, (object) (long) result7, (object[]) null);
                      continue;
                    }
                    continue;
                }
              case TypeCode.UInt64:
                switch (cellType)
                {
                  case WorksheetImpl.TRangeValueType.Number:
                    property.SetValue(instance1, (object) (ulong) exportDataMigrant.Number, (object[]) null);
                    continue;
                  case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
                    property.SetValue(instance1, (object) (ulong) exportDataMigrant.FormulaNumberValue, (object[]) null);
                    continue;
                  default:
                    object obj11 = this.NotifyAndSetUserValue(property.PropertyType, (IRange) exportDataMigrant, property.Name, cellType.ToString(), mismatchRecordIndex);
                    double result8;
                    if (obj11 != null && double.TryParse(obj11.ToString(), out result8))
                    {
                      property.SetValue(instance1, (object) (ulong) result8, (object[]) null);
                      continue;
                    }
                    continue;
                }
              case TypeCode.Single:
                switch (cellType)
                {
                  case WorksheetImpl.TRangeValueType.Number:
                    property.SetValue(instance1, (object) (float) exportDataMigrant.Number, (object[]) null);
                    continue;
                  case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
                    property.SetValue(instance1, (object) (float) exportDataMigrant.FormulaNumberValue, (object[]) null);
                    continue;
                  default:
                    object obj12 = this.NotifyAndSetUserValue(property.PropertyType, (IRange) exportDataMigrant, property.Name, cellType.ToString(), mismatchRecordIndex);
                    double result9;
                    if (obj12 != null && double.TryParse(obj12.ToString(), out result9))
                    {
                      property.SetValue(instance1, (object) (float) result9, (object[]) null);
                      continue;
                    }
                    continue;
                }
              case TypeCode.Double:
                switch (cellType)
                {
                  case WorksheetImpl.TRangeValueType.Number:
                    property.SetValue(instance1, (object) exportDataMigrant.Number, (object[]) null);
                    continue;
                  case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
                    property.SetValue(instance1, (object) exportDataMigrant.FormulaNumberValue, (object[]) null);
                    continue;
                  default:
                    object obj13 = this.NotifyAndSetUserValue(property.PropertyType, (IRange) exportDataMigrant, property.Name, cellType.ToString(), mismatchRecordIndex);
                    double result10;
                    if (obj13 != null && double.TryParse(obj13.ToString(), out result10))
                    {
                      property.SetValue(instance1, (object) result10, (object[]) null);
                      continue;
                    }
                    continue;
                }
              case TypeCode.Decimal:
                switch (cellType)
                {
                  case WorksheetImpl.TRangeValueType.Number:
                    property.SetValue(instance1, (object) (Decimal) exportDataMigrant.Number, (object[]) null);
                    continue;
                  case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
                    property.SetValue(instance1, (object) (Decimal) exportDataMigrant.FormulaNumberValue, (object[]) null);
                    continue;
                  default:
                    object obj14 = this.NotifyAndSetUserValue(property.PropertyType, (IRange) exportDataMigrant, property.Name, cellType.ToString(), mismatchRecordIndex);
                    double result11;
                    if (obj14 != null && double.TryParse(obj14.ToString(), out result11))
                    {
                      property.SetValue(instance1, (object) (Decimal) result11, (object[]) null);
                      continue;
                    }
                    continue;
                }
              case TypeCode.DateTime:
                if (exportDataMigrant.HasFormulaDateTime)
                {
                  property.SetValue(instance1, (object) exportDataMigrant.FormulaDateTime, (object[]) null);
                  continue;
                }
                if (exportDataMigrant.HasDateTime)
                {
                  property.SetValue(instance1, (object) exportDataMigrant.DateTime, (object[]) null);
                  continue;
                }
                object obj15 = this.NotifyAndSetUserValue(property.PropertyType, (IRange) exportDataMigrant, property.Name, cellType.ToString(), mismatchRecordIndex);
                if (obj15 != null && obj15 is DateTime dateTime)
                {
                  property.SetValue(instance1, (object) dateTime, (object[]) null);
                  continue;
                }
                continue;
              case TypeCode.String:
                property.SetValue(instance1, (object) exportDataMigrant.DisplayText, (object[]) null);
                continue;
              default:
                continue;
            }
          }
        }
      }
    }
    columnCount = num + columnCount1;
    return instance1;
  }

  private object NotifyAndSetUserValue(
    System.Type typeCode,
    IRange errorRange,
    string errorProperty,
    string cellType,
    int mismatchRecordIndex)
  {
    string str = System.Type.GetTypeCode(typeCode).ToString();
    string error = $"{cellType} cannot be set to {str} type";
    ExportEventArgs args = new ExportEventArgs(errorRange, error, errorProperty, errorRange.Value2, typeCode, cellType, mismatchRecordIndex);
    this.AppImplementation.RaiseTypeMismatchOnExportEvent((object) this, args);
    return args.NewValue;
  }

  public int ImportData(IEnumerable arrObject, int firstRow, int firstColumn, bool includeHeader)
  {
    return this.ImportData(arrObject, firstRow, firstColumn, includeHeader, false, ExcelNestedDataLayoutOptions.Default, ~ExcelNestedDataGroupOptions.Expand, 0, true);
  }

  public int ImportData(IEnumerable arrObject, ExcelImportDataOptions importDataOptions)
  {
    int firstRow = importDataOptions.FirstRow;
    int firstColumn = importDataOptions.FirstColumn;
    bool includeHeader = importDataOptions.IncludeHeader;
    bool includeHeaderParent = importDataOptions.IncludeHeaderParent;
    ExcelNestedDataLayoutOptions dataLayoutOptions = importDataOptions.NestedDataLayoutOptions;
    ExcelNestedDataGroupOptions dataGroupOptions = importDataOptions.NestedDataGroupOptions;
    int collapseLevel = importDataOptions.CollapseLevel;
    bool preserveTypes = importDataOptions.PreserveTypes;
    return this.ImportData(arrObject, firstRow, firstColumn, includeHeader, includeHeaderParent, dataLayoutOptions, dataGroupOptions, collapseLevel, preserveTypes);
  }

  internal int ImportData(
    IEnumerable arrObject,
    int firstRow,
    int firstColumn,
    bool includeHeader,
    bool includeHeaderParent,
    ExcelNestedDataLayoutOptions nestedLayoutOptions,
    ExcelNestedDataGroupOptions nestedGroupOptions,
    int collapseLevel,
    bool preserveTypes)
  {
    if (arrObject == null)
      throw new ArgumentNullException(nameof (arrObject));
    if (firstRow < 1 || firstRow > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (firstRow));
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (firstColumn));
    IEnumerator enumerator = arrObject.GetEnumerator();
    if (enumerator == null)
      return 0;
    bool flag = false;
    List<PropertyInfo> propertyInfo = (List<PropertyInfo>) null;
    int i = 0;
    if (!enumerator.MoveNext())
      return 0;
    object current = enumerator.Current;
    if (current == null)
      return 0;
    System.Type type = current.GetType();
    IDictionary<string, object> dictProperties = (IDictionary<string, object>) null;
    bool isDynamic = this.CheckIfDynamic(current, out dictProperties);
    if (!isDynamic && type.Namespace == null || type.Namespace != null && !type.Namespace.Contains("System"))
      flag = true;
    if (!isDynamic && !flag)
      return 0;
    IList<string> dynamicPropertyNames = (IList<string>) null;
    List<int> indexesOfNullableTypes = (List<int>) null;
    Dictionary<string, CallSite<Func<CallSite, object, object>>> callSiteCollection = (Dictionary<string, CallSite<Func<CallSite, object, object>>>) null;
    List<TypeCode> propertyTypeCodeCollection = isDynamic ? this.GetDynamicObjectMembersInfo(enumerator, current, dictProperties, out dynamicPropertyNames, out callSiteCollection, out indexesOfNullableTypes) : this.GetObjectMembersInfo(current, out propertyInfo);
    if (includeHeader)
    {
      this.AddHeaders(firstRow, firstColumn, isDynamic, dynamicPropertyNames, callSiteCollection, propertyTypeCodeCollection, propertyInfo, current, includeHeaderParent, "");
      ++firstRow;
    }
    this.m_importMergeRanges = new List<Rectangle>();
    this.m_importMergeRangeCollection = this.CreateRangesCollection();
    int rowCount = 0;
    this.FillData(firstRow, firstColumn, enumerator, current, propertyInfo, propertyTypeCodeCollection, isDynamic, callSiteCollection, dynamicPropertyNames, indexesOfNullableTypes, ref i, nestedLayoutOptions, nestedGroupOptions, 0, collapseLevel, preserveTypes, out rowCount);
    callSiteCollection?.Clear();
    if (this.m_importMergeRanges != null && this.m_importMergeRanges.Count > 0)
    {
      foreach (Rectangle importMergeRange in this.m_importMergeRanges)
        this.MergeCells.MergedRegions.Add(importMergeRange);
      this.m_importMergeRanges.Clear();
    }
    if (this.m_importMergeRangeCollection != null && this.m_importMergeRangeCollection.Count > 0)
    {
      string defaultName = CollectionBaseEx<IStyle>.GenerateDefaultName((ICollection<IStyle>) (this.m_book.Styles as StylesCollection), "VAlignTopStyle");
      this.m_book.Styles.Add(defaultName).VerticalAlignment = ExcelVAlign.VAlignTop;
      this.m_importMergeRangeCollection.CellStyleName = defaultName;
    }
    this.m_importMergeRanges = (List<Rectangle>) null;
    this.m_importMergeRangeCollection = (IRanges) null;
    return i;
  }

  private int AddHeaders(
    int firstRow,
    int firstColumn,
    bool isDynamic,
    IList<string> dynamicPropertyNames,
    Dictionary<string, CallSite<Func<CallSite, object, object>>> callSiteCollection,
    List<TypeCode> propertyTypeCodeCollection,
    List<PropertyInfo> propertyInfoCollection,
    object obj,
    bool includeHeaderParent,
    string parentName)
  {
    int num1 = 0;
    int num2 = isDynamic ? dynamicPropertyNames.Count : propertyInfoCollection.Count;
    for (int index = 0; index < num2; ++index)
    {
      string empty = string.Empty;
      string str;
      object propertyValue;
      if (!isDynamic)
      {
        str = propertyInfoCollection[index].Name;
        object[] customAttributes = propertyInfoCollection[index].GetCustomAttributes(typeof (DisplayNameAttribute), true);
        if (customAttributes != null)
        {
          foreach (Attribute attribute in customAttributes)
          {
            if (attribute is DisplayNameAttribute displayNameAttribute && !string.IsNullOrEmpty(displayNameAttribute.DisplayName))
            {
              str = displayNameAttribute.DisplayName;
              break;
            }
          }
        }
        propertyValue = this.GetPropertyValue(obj, isDynamic, propertyInfoCollection[index], (Dictionary<string, CallSite<Func<CallSite, object, object>>>) null, (string) null);
      }
      else
      {
        str = dynamicPropertyNames[index];
        propertyValue = this.GetPropertyValue(obj, isDynamic, (PropertyInfo) null, callSiteCollection, dynamicPropertyNames[index]);
      }
      if (includeHeaderParent)
        str = parentName + str;
      if (propertyValue != null && propertyTypeCodeCollection[index] == TypeCode.Object && !(propertyValue is IHyperLink))
      {
        List<PropertyInfo> infoCollection = (List<PropertyInfo>) null;
        List<TypeCode> typeCodeCollection = (List<TypeCode>) null;
        IDictionary<string, object> dictProperties = (IDictionary<string, object>) null;
        IList<string> childDynamicPropertyNames = (IList<string>) null;
        List<int> childIndexesOfNullableTypes = (List<int>) null;
        IEnumerator enumValue = (IEnumerator) null;
        object enumObj = (object) null;
        bool bIsDynamic = false;
        Dictionary<string, CallSite<Func<CallSite, object, object>>> childCallSiteCollection = (Dictionary<string, CallSite<Func<CallSite, object, object>>>) null;
        if (propertyValue is IEnumerable && this.GetChildObjectMembersInfo(propertyValue, out enumValue, out enumObj, out bIsDynamic, out typeCodeCollection, out infoCollection, out dictProperties, out childDynamicPropertyNames, out childCallSiteCollection, out childIndexesOfNullableTypes))
          num1 = this.AddHeaders(firstRow, firstColumn + index + num1, bIsDynamic, childDynamicPropertyNames, childCallSiteCollection, typeCodeCollection, infoCollection, enumObj, includeHeaderParent, str + ".");
        else if (this.ExtractObjectMemberInfo(propertyValue, out bIsDynamic, out typeCodeCollection, out infoCollection, out dictProperties, out childDynamicPropertyNames, out childCallSiteCollection, out childIndexesOfNullableTypes))
          num1 = this.AddHeaders(firstRow, firstColumn + index + num1, bIsDynamic, childDynamicPropertyNames, childCallSiteCollection, typeCodeCollection, infoCollection, propertyValue, includeHeaderParent, str + ".");
        else
          this.SetText(firstRow, firstColumn + index + num1, str);
      }
      else
        this.SetText(firstRow, firstColumn + index + num1, str);
    }
    return num2 - 1;
  }

  private int FillData(
    int firstRow,
    int firstColumn,
    IEnumerator valueEnum,
    object obj,
    List<PropertyInfo> propertyInfoCollection,
    List<TypeCode> propertyTypeCodeCollection,
    bool isDynamic,
    Dictionary<string, CallSite<Func<CallSite, object, object>>> callSiteCollection,
    IList<string> dynamicPropertyNames,
    List<int> indexesOfNullableTypes,
    ref int i,
    ExcelNestedDataLayoutOptions nestedLayoutOptions,
    ExcelNestedDataGroupOptions nestedGroupOptions,
    int currentLevel,
    int collapseLevel,
    bool bPreserveTypes,
    out int rowCount)
  {
    int num1 = 0;
    int num2 = 0;
    rowCount = 0;
    bool isSameTypeCode = false;
    ++currentLevel;
    WorksheetImpl.SetPropertyType(propertyInfoCollection, propertyTypeCodeCollection, isDynamic);
    IMigrantRange migrantRange = this.MigrantRange;
    do
    {
      if (valueEnum != null)
        obj = valueEnum.Current;
      if (obj != null)
      {
        int num3 = 0;
        rowCount = 0;
        num1 = isDynamic ? dynamicPropertyNames.Count : propertyInfoCollection.Count;
        for (int index = 0; index < num1; ++index)
        {
          migrantRange.ResetRowColumn(firstRow + i, firstColumn + index);
          object propertyValue = !isDynamic ? this.GetPropertyValue(obj, isDynamic, propertyInfoCollection[index], callSiteCollection, (string) null) : this.GetPropertyValue(obj, isDynamic, (PropertyInfo) null, callSiteCollection, dynamicPropertyNames[index]);
          if (propertyValue != null && propertyValue != DBNull.Value)
          {
            if (isDynamic && bPreserveTypes)
              isSameTypeCode = System.Type.GetTypeCode(propertyValue.GetType()).Equals((object) propertyTypeCodeCollection[index]);
            if (index > propertyTypeCodeCollection.Count - 1 && indexesOfNullableTypes != null && indexesOfNullableTypes.Count > 0)
            {
              propertyTypeCodeCollection.Add(System.Type.GetTypeCode(propertyValue.GetType()));
              indexesOfNullableTypes.Remove(index);
            }
            IHyperLink hyperLink1 = propertyValue as IHyperLink;
            if (propertyValue != null && hyperLink1 != null)
            {
              IHyperLink hyperLink2 = hyperLink1;
              IHyperLink hyperLink3 = this.HyperLinks.Add((IRange) migrantRange);
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
            else if (propertyValue != null && propertyTypeCodeCollection[index] == TypeCode.Object)
            {
              List<PropertyInfo> infoCollection = (List<PropertyInfo>) null;
              List<TypeCode> typeCodeCollection = (List<TypeCode>) null;
              IDictionary<string, object> dictProperties = (IDictionary<string, object>) null;
              IList<string> childDynamicPropertyNames = (IList<string>) null;
              List<int> childIndexesOfNullableTypes = (List<int>) null;
              IEnumerator enumValue = (IEnumerator) null;
              object enumObj = (object) null;
              bool bIsDynamic = false;
              Dictionary<string, CallSite<Func<CallSite, object, object>>> childCallSiteCollection = (Dictionary<string, CallSite<Func<CallSite, object, object>>>) null;
              if (propertyValue is IEnumerable && this.GetChildObjectMembersInfo(propertyValue, out enumValue, out enumObj, out bIsDynamic, out typeCodeCollection, out infoCollection, out dictProperties, out childDynamicPropertyNames, out childCallSiteCollection, out childIndexesOfNullableTypes))
              {
                int num4 = currentLevel;
                num3 = this.FillData(firstRow, migrantRange.Column + num3, enumValue, enumObj, infoCollection, typeCodeCollection, bIsDynamic, childCallSiteCollection, childDynamicPropertyNames, childIndexesOfNullableTypes, ref i, nestedLayoutOptions, nestedGroupOptions, currentLevel, collapseLevel, bPreserveTypes, out rowCount);
                i -= rowCount + 1;
                currentLevel = num4;
              }
              else if (this.ExtractObjectMemberInfo(propertyValue, out bIsDynamic, out typeCodeCollection, out infoCollection, out dictProperties, out childDynamicPropertyNames, out childCallSiteCollection, out childIndexesOfNullableTypes))
              {
                int num5 = currentLevel;
                num3 = this.FillData(firstRow, migrantRange.Column + num3, enumValue, propertyValue, infoCollection, typeCodeCollection, bIsDynamic, childCallSiteCollection, childDynamicPropertyNames, childIndexesOfNullableTypes, ref i, nestedLayoutOptions, nestedGroupOptions, currentLevel, collapseLevel, bPreserveTypes, out rowCount);
                i -= rowCount + 1;
                currentLevel = num5;
              }
              else if (!bPreserveTypes)
                migrantRange.Value2 = propertyValue;
              else
                migrantRange.SetValue(propertyValue.ToString());
            }
            else if (!bPreserveTypes)
              WorksheetImpl.ImportDataWithoutCheck(propertyTypeCodeCollection[index], propertyValue, migrantRange);
            else
              WorksheetImpl.ImportDataWithoutCheckPreserve(propertyTypeCodeCollection[index], propertyValue, migrantRange, isDynamic, isSameTypeCode);
          }
        }
        ++i;
        ++num2;
        i += rowCount;
        if (nestedLayoutOptions != ExcelNestedDataLayoutOptions.Default || nestedGroupOptions != ~ExcelNestedDataGroupOptions.Expand)
        {
          bool flag = false;
          int collectionCount = 0;
          this.GetCollectionCount(obj, out collectionCount);
          if (collectionCount > 0)
          {
            if (nestedGroupOptions != ~ExcelNestedDataGroupOptions.Expand)
            {
              int row = firstRow + i - (collectionCount + 1) + 1;
              int lastRow = row + collectionCount - 1;
              (this[row, firstColumn, lastRow, firstColumn] as RangeImpl).Group(ExcelGroupBy.ByRows, nestedGroupOptions == ExcelNestedDataGroupOptions.Collapse && currentLevel >= collapseLevel, true);
            }
            for (int index = 0; index < num1; ++index)
            {
              int num6 = firstRow + i - (collectionCount + 1);
              int lastRow = num6 + collectionCount;
              int num7 = firstColumn + index;
              if (flag)
                num7 += num3;
              object obj1 = !isDynamic ? this.GetPropertyValue(obj, isDynamic, propertyInfoCollection[index], callSiteCollection, (string) null) : this.GetPropertyValue(obj, isDynamic, (PropertyInfo) null, callSiteCollection, dynamicPropertyNames[index]);
              if (propertyTypeCodeCollection[index] != TypeCode.String && obj1 is IEnumerable)
              {
                flag = true;
              }
              else
              {
                switch (nestedLayoutOptions)
                {
                  case ExcelNestedDataLayoutOptions.Merge:
                    int x = num7 - 1;
                    int y = num6 - 1;
                    int num8 = lastRow - 1;
                    this.m_importMergeRanges.Add(new Rectangle(x, y, x - x, num8 - y));
                    this.m_importMergeRangeCollection.Add(this[num6, num7]);
                    continue;
                  case ExcelNestedDataLayoutOptions.Repeat:
                    this.SetRepeatRangeValues(num6, num7, lastRow, num7);
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
        }
      }
    }
    while (valueEnum != null && valueEnum.MoveNext());
    rowCount = num2 - 1;
    return num1 - 1;
  }

  internal void SetRepeatRangeValues(int firstRow, int firstColumn, int lastRow, int lastColumn)
  {
    object textFromCellType = this.GetTextFromCellType(firstRow, firstColumn, out WorksheetImpl.TRangeValueType _);
    WorksheetImpl.TRangeValueType cellType = this.GetCellType(firstRow, firstColumn, true);
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this);
    for (int iRow = firstRow; iRow <= lastRow; ++iRow)
    {
      for (int iColumn = firstColumn; iColumn <= lastColumn; ++iColumn)
      {
        migrantRangeImpl.ResetRowColumn(iRow, iColumn);
        switch (cellType)
        {
          case WorksheetImpl.TRangeValueType.Blank:
            this.SetString(iRow, iColumn, string.Empty);
            break;
          case WorksheetImpl.TRangeValueType.Error:
          case WorksheetImpl.TRangeValueType.Error | WorksheetImpl.TRangeValueType.Formula:
            this.SetError(iRow, iColumn, (string) textFromCellType);
            break;
          case WorksheetImpl.TRangeValueType.Boolean:
          case WorksheetImpl.TRangeValueType.Boolean | WorksheetImpl.TRangeValueType.Formula:
            this.SetBoolean(iRow, iColumn, (bool) textFromCellType);
            break;
          case WorksheetImpl.TRangeValueType.Number:
          case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
            if (migrantRangeImpl.HasDateTime)
            {
              migrantRangeImpl.SetValue(migrantRangeImpl.DateTime);
              break;
            }
            if (migrantRangeImpl.HasFormulaDateTime)
            {
              migrantRangeImpl.SetValue(migrantRangeImpl.FormulaDateTime);
              break;
            }
            this.SetNumber(iRow, iColumn, (double) textFromCellType);
            break;
          case WorksheetImpl.TRangeValueType.Formula:
          case WorksheetImpl.TRangeValueType.String:
          case WorksheetImpl.TRangeValueType.Formula | WorksheetImpl.TRangeValueType.String:
            this.SetString(iRow, iColumn, (string) textFromCellType);
            break;
        }
      }
    }
  }

  private object GetPropertyValue(
    object obj,
    bool isDynamic,
    PropertyInfo propertyInfo,
    Dictionary<string, CallSite<Func<CallSite, object, object>>> callSiteCollection,
    string dynamicPropertyName)
  {
    System.Type type = obj.GetType();
    object propertyValue = (object) null;
    if (isDynamic)
    {
      if (callSiteCollection != null)
      {
        string key = $"{type.Name}_{dynamicPropertyName}";
        if (callSiteCollection.ContainsKey(key))
          propertyValue = callSiteCollection[key].Target((CallSite) callSiteCollection[key], obj);
      }
      else if ((obj as IDictionary<string, object>).ContainsKey(dynamicPropertyName))
        propertyValue = (obj as IDictionary<string, object>)[dynamicPropertyName];
    }
    else
      propertyValue = this.GetValueFromProperty(obj, propertyInfo);
    return propertyValue;
  }

  private bool CheckIfDynamic(object obj, out IDictionary<string, object> dictProperties)
  {
    bool flag = false;
    System.Type type = obj.GetType();
    dictProperties = (IDictionary<string, object>) null;
    if (type.FullName.Contains("System.Dynamic.ExpandoObject"))
    {
      dictProperties = obj as IDictionary<string, object>;
      if (dictProperties == null)
        throw new ArgumentOutOfRangeException("not found");
      flag = true;
    }
    else if (obj is DynamicObject && this.AppImplementation.HasDynamicOverrideMethods(type))
      flag = true;
    return flag;
  }

  private static void SetPropertyType(
    List<PropertyInfo> propertyInfoCollection,
    List<TypeCode> propertyTypeCodeCollection,
    bool isDynamic)
  {
    if (isDynamic)
      return;
    for (int index = 0; index < propertyInfoCollection.Count; ++index)
    {
      if (propertyTypeCodeCollection[index] == TypeCode.Object)
      {
        System.Type propertyType = propertyInfoCollection[index].PropertyType;
        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof (Nullable<>))
        {
          switch (propertyType.GetGenericArguments()[0].Name)
          {
            case "Int32":
              propertyTypeCodeCollection[index] = TypeCode.Int32;
              continue;
            case "Int64":
              propertyTypeCodeCollection[index] = TypeCode.Int64;
              continue;
            case "Int16":
              propertyTypeCodeCollection[index] = TypeCode.Int16;
              continue;
            case "Decimal":
              propertyTypeCodeCollection[index] = TypeCode.Decimal;
              continue;
            case "Double":
              propertyTypeCodeCollection[index] = TypeCode.Double;
              continue;
            case "DateTime":
              propertyTypeCodeCollection[index] = TypeCode.DateTime;
              continue;
            case "Boolean":
              propertyTypeCodeCollection[index] = TypeCode.Boolean;
              continue;
            default:
              continue;
          }
        }
      }
    }
  }

  private static void ImportDataWithoutCheckPreserve(
    TypeCode propertyTypeCode,
    object propertyValue,
    IMigrantRange migrantRange,
    bool isDynamic,
    bool isSameTypeCode)
  {
    switch (propertyTypeCode)
    {
      case TypeCode.Boolean:
        if (isDynamic && !isSameTypeCode)
        {
          migrantRange.Value2 = propertyValue;
          break;
        }
        migrantRange.SetValue((bool) propertyValue);
        break;
      case TypeCode.Int16:
        if (isDynamic && !isSameTypeCode)
        {
          migrantRange.Value2 = propertyValue;
          break;
        }
        migrantRange.SetValue((int) Convert.ToInt16(propertyValue));
        break;
      case TypeCode.Int32:
        if (isDynamic && !isSameTypeCode)
        {
          migrantRange.Value2 = propertyValue;
          break;
        }
        migrantRange.SetValue((int) propertyValue);
        break;
      case TypeCode.Int64:
      case TypeCode.Decimal:
        if (isDynamic && !isSameTypeCode)
        {
          migrantRange.Value2 = propertyValue;
          break;
        }
        migrantRange.SetValue(Convert.ToDouble(propertyValue));
        break;
      case TypeCode.Double:
        if (isDynamic && !isSameTypeCode)
        {
          migrantRange.Value2 = propertyValue;
          break;
        }
        migrantRange.SetValue((double) propertyValue);
        break;
      case TypeCode.DateTime:
        if (isDynamic && !isSameTypeCode)
        {
          migrantRange.Value2 = propertyValue;
          break;
        }
        migrantRange.SetValue((DateTime) propertyValue);
        break;
      case TypeCode.String:
        if (isDynamic && !isSameTypeCode)
        {
          migrantRange.Value2 = propertyValue;
          break;
        }
        migrantRange.SetValue(propertyValue.ToString());
        break;
      default:
        migrantRange.SetValue(propertyValue.ToString());
        break;
    }
  }

  private static void ImportDataWithoutCheck(
    TypeCode propertyTypeCode,
    object propertyValue,
    IMigrantRange migrantRange)
  {
    switch (propertyTypeCode)
    {
      case TypeCode.Boolean:
        migrantRange.Value2 = propertyValue;
        break;
      case TypeCode.Int16:
        migrantRange.SetValue((int) Convert.ToInt16(propertyValue));
        break;
      case TypeCode.Int32:
        migrantRange.SetValue((int) propertyValue);
        break;
      case TypeCode.Int64:
      case TypeCode.Decimal:
        migrantRange.SetValue(Convert.ToDouble(propertyValue));
        break;
      case TypeCode.Double:
        migrantRange.SetValue((double) propertyValue);
        break;
      case TypeCode.DateTime:
        migrantRange.Value2 = (object) (DateTime) propertyValue;
        break;
      case TypeCode.String:
        migrantRange.Value2 = propertyValue;
        break;
      default:
        migrantRange.Value2 = propertyValue;
        break;
    }
  }

  private bool GetChildObjectMembersInfo(
    object propertyValue,
    out IEnumerator enumValue,
    out object enumObj,
    out bool bIsDynamic,
    out List<TypeCode> typeCodeCollection,
    out List<PropertyInfo> infoCollection,
    out IDictionary<string, object> dictProperties,
    out IList<string> childDynamicPropertyNames,
    out Dictionary<string, CallSite<Func<CallSite, object, object>>> childCallSiteCollection,
    out List<int> childIndexesOfNullableTypes)
  {
    childCallSiteCollection = (Dictionary<string, CallSite<Func<CallSite, object, object>>>) null;
    infoCollection = (List<PropertyInfo>) null;
    typeCodeCollection = (List<TypeCode>) null;
    dictProperties = (IDictionary<string, object>) null;
    childDynamicPropertyNames = (IList<string>) null;
    childIndexesOfNullableTypes = (List<int>) null;
    enumObj = (object) null;
    enumValue = (IEnumerator) null;
    bIsDynamic = false;
    if (propertyValue is IEnumerable)
    {
      enumValue = (propertyValue as IEnumerable).GetEnumerator();
      if (enumValue.MoveNext())
      {
        enumObj = enumValue.Current;
        enumObj.GetType();
        bIsDynamic = this.CheckIfDynamic(enumObj, out dictProperties);
        typeCodeCollection = bIsDynamic ? this.GetDynamicObjectMembersInfo(enumValue, enumObj, dictProperties, out childDynamicPropertyNames, out childCallSiteCollection, out childIndexesOfNullableTypes) : this.GetObjectMembersInfo(enumObj, out infoCollection);
        return typeCodeCollection.Count > 0;
      }
    }
    return false;
  }

  private bool ExtractObjectMemberInfo(
    object obj,
    out bool bIsDynamic,
    out List<TypeCode> typeCodeCollection,
    out List<PropertyInfo> infoCollection,
    out IDictionary<string, object> dictProperties,
    out IList<string> childDynamicPropertyNames,
    out Dictionary<string, CallSite<Func<CallSite, object, object>>> childCallSiteCollection,
    out List<int> childIndexesOfNullableTypes)
  {
    childCallSiteCollection = (Dictionary<string, CallSite<Func<CallSite, object, object>>>) null;
    infoCollection = (List<PropertyInfo>) null;
    typeCodeCollection = (List<TypeCode>) null;
    dictProperties = (IDictionary<string, object>) null;
    childDynamicPropertyNames = (IList<string>) null;
    childIndexesOfNullableTypes = (List<int>) null;
    System.Type type1 = obj.GetType();
    bIsDynamic = this.CheckIfDynamic(obj, out dictProperties);
    bool flag = false;
    if (!bIsDynamic && type1.Namespace == null || type1.Namespace != null && !type1.Namespace.Contains("System"))
      flag = true;
    if (!bIsDynamic && !flag)
      return false;
    if (!bIsDynamic)
      typeCodeCollection = this.GetObjectMembersInfo(obj, out infoCollection);
    else if (dictProperties != null)
    {
      typeCodeCollection = this.GetPropertyInfosFromDictionary(dictProperties, out childIndexesOfNullableTypes);
      childDynamicPropertyNames = (IList<string>) new List<string>((IEnumerable<string>) dictProperties.Keys);
    }
    else
    {
      object obj1 = obj;
      ref IList<string> local = ref childDynamicPropertyNames;
      // ISSUE: reference to a compiler-generated field
      if (WorksheetImpl.\u003CExtractObjectMemberInfo\u003Eo__SiteContainer1.\u003C\u003Ep__Site2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WorksheetImpl.\u003CExtractObjectMemberInfo\u003Eo__SiteContainer1.\u003C\u003Ep__Site2 = CallSite<Func<CallSite, System.Type, object, List<string>>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (WorksheetImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, object, List<string>> target = WorksheetImpl.\u003CExtractObjectMemberInfo\u003Eo__SiteContainer1.\u003C\u003Ep__Site2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, object, List<string>>> pSite2 = WorksheetImpl.\u003CExtractObjectMemberInfo\u003Eo__SiteContainer1.\u003C\u003Ep__Site2;
      System.Type type2 = typeof (List<string>);
      // ISSUE: reference to a compiler-generated field
      if (WorksheetImpl.\u003CExtractObjectMemberInfo\u003Eo__SiteContainer1.\u003C\u003Ep__Site3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WorksheetImpl.\u003CExtractObjectMemberInfo\u003Eo__SiteContainer1.\u003C\u003Ep__Site3 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "GetDynamicMemberNames", (IEnumerable<System.Type>) null, typeof (WorksheetImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = WorksheetImpl.\u003CExtractObjectMemberInfo\u003Eo__SiteContainer1.\u003C\u003Ep__Site3.Target((CallSite) WorksheetImpl.\u003CExtractObjectMemberInfo\u003Eo__SiteContainer1.\u003C\u003Ep__Site3, obj1);
      List<string> stringList = target((CallSite) pSite2, type2, obj2);
      local = (IList<string>) stringList;
      typeCodeCollection = this.GetTypeCodesFromCustomDynamic(obj, childDynamicPropertyNames, out childCallSiteCollection, out childIndexesOfNullableTypes);
    }
    return typeCodeCollection != null && typeCodeCollection.Count > 0;
  }

  private List<TypeCode> GetDynamicObjectMembersInfo(
    IEnumerator valueEnum,
    object obj,
    IDictionary<string, object> dictProperties,
    out IList<string> dynamicPropertyNames,
    out Dictionary<string, CallSite<Func<CallSite, object, object>>> callSiteCollection,
    out List<int> indexesOfNullableTypes)
  {
    callSiteCollection = (Dictionary<string, CallSite<Func<CallSite, object, object>>>) null;
    indexesOfNullableTypes = (List<int>) null;
    dynamicPropertyNames = (IList<string>) null;
    List<TypeCode> objectMembersInfo = (List<TypeCode>) null;
    if (dictProperties != null)
    {
      if (dictProperties.Count == 0)
      {
        while (valueEnum.MoveNext())
        {
          obj = valueEnum.Current;
          dictProperties = obj as IDictionary<string, object>;
          if (dictProperties.Count != 0)
            goto label_5;
        }
        return (List<TypeCode>) null;
      }
label_5:
      objectMembersInfo = this.GetPropertyInfosFromDictionary(dictProperties, out indexesOfNullableTypes);
      dynamicPropertyNames = (IList<string>) new List<string>((IEnumerable<string>) dictProperties.Keys);
    }
    else
    {
      callSiteCollection = (Dictionary<string, CallSite<Func<CallSite, object, object>>>) null;
      object obj1 = obj;
      ref IList<string> local1 = ref dynamicPropertyNames;
      // ISSUE: reference to a compiler-generated field
      if (WorksheetImpl.\u003CGetDynamicObjectMembersInfo\u003Eo__SiteContainer4.\u003C\u003Ep__Site5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WorksheetImpl.\u003CGetDynamicObjectMembersInfo\u003Eo__SiteContainer4.\u003C\u003Ep__Site5 = CallSite<Func<CallSite, System.Type, object, List<string>>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (WorksheetImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, object, List<string>> target1 = WorksheetImpl.\u003CGetDynamicObjectMembersInfo\u003Eo__SiteContainer4.\u003C\u003Ep__Site5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, object, List<string>>> pSite5 = WorksheetImpl.\u003CGetDynamicObjectMembersInfo\u003Eo__SiteContainer4.\u003C\u003Ep__Site5;
      System.Type type1 = typeof (List<string>);
      // ISSUE: reference to a compiler-generated field
      if (WorksheetImpl.\u003CGetDynamicObjectMembersInfo\u003Eo__SiteContainer4.\u003C\u003Ep__Site6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WorksheetImpl.\u003CGetDynamicObjectMembersInfo\u003Eo__SiteContainer4.\u003C\u003Ep__Site6 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "GetDynamicMemberNames", (IEnumerable<System.Type>) null, typeof (WorksheetImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = WorksheetImpl.\u003CGetDynamicObjectMembersInfo\u003Eo__SiteContainer4.\u003C\u003Ep__Site6.Target((CallSite) WorksheetImpl.\u003CGetDynamicObjectMembersInfo\u003Eo__SiteContainer4.\u003C\u003Ep__Site6, obj1);
      List<string> stringList1 = target1((CallSite) pSite5, type1, obj2);
      local1 = (IList<string>) stringList1;
      if (dynamicPropertyNames.Count == 0)
      {
        while (valueEnum.MoveNext())
        {
          obj = valueEnum.Current;
          object obj3 = obj;
          ref IList<string> local2 = ref dynamicPropertyNames;
          // ISSUE: reference to a compiler-generated field
          if (WorksheetImpl.\u003CGetDynamicObjectMembersInfo\u003Eo__SiteContainer4.\u003C\u003Ep__Site7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            WorksheetImpl.\u003CGetDynamicObjectMembersInfo\u003Eo__SiteContainer4.\u003C\u003Ep__Site7 = CallSite<Func<CallSite, System.Type, object, List<string>>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (WorksheetImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, System.Type, object, List<string>> target2 = WorksheetImpl.\u003CGetDynamicObjectMembersInfo\u003Eo__SiteContainer4.\u003C\u003Ep__Site7.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, System.Type, object, List<string>>> pSite7 = WorksheetImpl.\u003CGetDynamicObjectMembersInfo\u003Eo__SiteContainer4.\u003C\u003Ep__Site7;
          System.Type type2 = typeof (List<string>);
          // ISSUE: reference to a compiler-generated field
          if (WorksheetImpl.\u003CGetDynamicObjectMembersInfo\u003Eo__SiteContainer4.\u003C\u003Ep__Site8 == null)
          {
            // ISSUE: reference to a compiler-generated field
            WorksheetImpl.\u003CGetDynamicObjectMembersInfo\u003Eo__SiteContainer4.\u003C\u003Ep__Site8 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "GetDynamicMemberNames", (IEnumerable<System.Type>) null, typeof (WorksheetImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj4 = WorksheetImpl.\u003CGetDynamicObjectMembersInfo\u003Eo__SiteContainer4.\u003C\u003Ep__Site8.Target((CallSite) WorksheetImpl.\u003CGetDynamicObjectMembersInfo\u003Eo__SiteContainer4.\u003C\u003Ep__Site8, obj3);
          List<string> stringList2 = target2((CallSite) pSite7, type2, obj4);
          local2 = (IList<string>) stringList2;
          if (dynamicPropertyNames.Count != 0)
            goto label_18;
        }
        return (List<TypeCode>) null;
      }
label_18:
      if (dynamicPropertyNames.Count > 1)
        objectMembersInfo = this.GetTypeCodesFromCustomDynamic(obj, dynamicPropertyNames, out callSiteCollection, out indexesOfNullableTypes);
    }
    return objectMembersInfo;
  }

  internal bool GetCollectionCount(object value, out int collectionCount)
  {
    collectionCount = 0;
    bool flag = false;
    System.Type type1 = value.GetType();
    List<PropertyInfo> propertyInfoList = new List<PropertyInfo>((IEnumerable<PropertyInfo>) type1.GetProperties());
    if (propertyInfoList.Count < 1)
    {
      if (type1.FullName.Contains("System.Dynamic.ExpandoObject"))
      {
        foreach (object obj1 in (IEnumerable<object>) (value as IDictionary<string, object>).Values)
        {
          if (obj1 != null && obj1 is IEnumerable && !(obj1 is string))
          {
            flag = true;
            foreach (object obj2 in (IEnumerable) (obj1 as IList))
            {
              int collectionCount1 = 0;
              if (this.GetCollectionCount(obj2, out collectionCount1))
                collectionCount += collectionCount1;
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
        if (WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sitea == null)
        {
          // ISSUE: reference to a compiler-generated field
          WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sitea = CallSite<Func<CallSite, System.Type, object, List<string>>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (WorksheetImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, System.Type, object, List<string>> target1 = WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sitea.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, System.Type, object, List<string>>> pSitea = WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sitea;
        System.Type type2 = typeof (List<string>);
        // ISSUE: reference to a compiler-generated field
        if (WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Siteb == null)
        {
          // ISSUE: reference to a compiler-generated field
          WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Siteb = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "GetDynamicMemberNames", (IEnumerable<System.Type>) null, typeof (WorksheetImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Siteb.Target((CallSite) WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Siteb, obj3);
        foreach (string str1 in (IEnumerable<string>) target1((CallSite) pSitea, type2, obj4))
        {
          // ISSUE: reference to a compiler-generated field
          if (WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sitec == null)
          {
            // ISSUE: reference to a compiler-generated field
            WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sitec = CallSite<Func<CallSite, object, CallSiteBinder>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof (CallSiteBinder), typeof (WorksheetImpl)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, CallSiteBinder> target2 = WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sitec.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, CallSiteBinder>> pSitec = WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sitec;
          // ISSUE: reference to a compiler-generated field
          if (WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sited == null)
          {
            // ISSUE: reference to a compiler-generated field
            WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sited = CallSite<Func<CallSite, System.Type, CSharpBinderFlags, string, object, CSharpArgumentInfo[], object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "GetMember", (IEnumerable<System.Type>) null, typeof (WorksheetImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[5]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, System.Type, CSharpBinderFlags, string, object, CSharpArgumentInfo[], object> target3 = WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sited.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, System.Type, CSharpBinderFlags, string, object, CSharpArgumentInfo[], object>> pSited = WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sited;
          System.Type type3 = typeof (Microsoft.CSharp.RuntimeBinder.Binder);
          string str2 = str1;
          // ISSUE: reference to a compiler-generated field
          if (WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sitee == null)
          {
            // ISSUE: reference to a compiler-generated field
            WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sitee = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "GetType", (IEnumerable<System.Type>) null, typeof (WorksheetImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj5 = WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sitee.Target((CallSite) WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sitee, obj3);
          CSharpArgumentInfo[] csharpArgumentInfoArray = new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          };
          object obj6 = target3((CallSite) pSited, type3, CSharpBinderFlags.None, str2, obj5, csharpArgumentInfoArray);
          CallSite<Func<CallSite, object, object>> callSite = CallSite<Func<CallSite, object, object>>.Create(target2((CallSite) pSitec, obj6));
          // ISSUE: reference to a compiler-generated field
          if (WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sitef == null)
          {
            // ISSUE: reference to a compiler-generated field
            WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sitef = CallSite<Func<CallSite, Func<CallSite, object, object>, CallSite<Func<CallSite, object, object>>, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Invoke(CSharpBinderFlags.None, typeof (WorksheetImpl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj7 = WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sitef.Target((CallSite) WorksheetImpl.\u003CGetCollectionCount\u003Eo__SiteContainer9.\u003C\u003Ep__Sitef, callSite.Target, callSite, obj3);
          if (obj7 is IEnumerable && !(obj7 is string))
          {
            flag = true;
            foreach (object obj8 in (IEnumerable) (obj7 as IList))
            {
              int collectionCount2 = 0;
              if (this.GetCollectionCount(obj8, out collectionCount2))
                collectionCount += collectionCount2;
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
        if (obj9 is IEnumerable && !(obj9 is string))
        {
          flag = true;
          foreach (object obj10 in (IEnumerable) (obj9 as IList))
          {
            int collectionCount3 = 0;
            if (this.GetCollectionCount(obj10, out collectionCount3))
              collectionCount += collectionCount3;
          }
          collectionCount = collectionCount + (obj9 as IList).Count - 1;
          break;
        }
      }
    }
    return flag && collectionCount > 0;
  }

  private List<TypeCode> GetPropertyInfosFromDictionary(
    IDictionary<string, object> objects,
    out List<int> nullableList)
  {
    List<TypeCode> infosFromDictionary = new List<TypeCode>(objects.Keys.Count);
    nullableList = (List<int>) null;
    int num = 0;
    foreach (object obj in (IEnumerable<object>) objects.Values)
    {
      if (obj == null)
      {
        if (nullableList == null)
          nullableList = new List<int>(objects.Values.Count);
        nullableList.Add(num);
      }
      else
        infosFromDictionary.Add(System.Type.GetTypeCode(obj.GetType()));
      ++num;
    }
    return infosFromDictionary;
  }

  private List<TypeCode> GetObjectMembersInfo(object obj, out List<PropertyInfo> propertyInfo)
  {
    return this.GetObjectMembersInfo(obj.GetType(), out propertyInfo);
  }

  private List<TypeCode> GetObjectMembersInfo(System.Type type, out List<PropertyInfo> propertyInfo)
  {
    List<TypeCode> objectMembersInfo = new List<TypeCode>();
    propertyInfo = new List<PropertyInfo>();
    foreach (PropertyInfo property in type.GetProperties())
    {
      object[] customAttributes = property.GetCustomAttributes(typeof (BindableAttribute), true);
      bool flag = true;
      if (customAttributes != null)
      {
        foreach (Attribute attribute in customAttributes)
        {
          if (attribute is BindableAttribute && !(attribute as BindableAttribute).Bindable)
            flag = false;
        }
      }
      if (flag)
      {
        propertyInfo.Add(property);
        objectMembersInfo.Add(System.Type.GetTypeCode(property.PropertyType));
      }
    }
    return objectMembersInfo;
  }

  private object GetValueFromProperty(object value, PropertyInfo strProperty)
  {
    value = !(strProperty == (PropertyInfo) null) ? strProperty.GetValue(value, (object[]) null) : throw new ArgumentOutOfRangeException("Can't find property");
    return value;
  }

  public void RemovePanes()
  {
    this.ParseData();
    this.WindowTwo.IsFreezePanes = false;
    this.WindowTwo.IsFreezePanesNoSplit = false;
    this.m_pane = (PaneRecord) null;
  }

  public IRange IntersectRanges(IRange range1, IRange range2)
  {
    if (range1 == null)
      throw new ArgumentNullException(nameof (range1));
    if (range1 == null)
      throw new ArgumentNullException(nameof (range2));
    if (range1.Parent != range2.Parent)
      return (IRange) null;
    Rectangle rectangle = Rectangle.Intersect(Rectangle.FromLTRB(range1.Column, range1.Row, range1.LastColumn, range1.LastRow), Rectangle.FromLTRB(range2.Column, range2.Row, range2.LastColumn, range2.LastRow));
    return rectangle == Rectangle.Empty ? (IRange) null : range1.Worksheet[rectangle.Top, rectangle.Left, rectangle.Bottom, rectangle.Right];
  }

  public IRange MergeRanges(IRange range1, IRange range2)
  {
    if (range1 == null)
      throw new ArgumentNullException(nameof (range1));
    if (range2 == null)
      throw new ArgumentNullException(nameof (range2));
    if (range1.Parent != range2.Parent)
      return (IRange) null;
    int num1 = range1.LastColumn - range1.Column + 1;
    int num2 = range2.LastColumn - range2.Column + 1;
    int num3 = range1.LastRow - range1.Row + 1;
    int num4 = range2.LastRow - range2.Row + 1;
    if (num1 != num2 && num3 != num4)
      return (IRange) null;
    if (num1 == num2 && range1.Column == range2.Column)
    {
      if (range2.Row < range1.Row)
      {
        IRange range = range1;
        range1 = range2;
        range2 = range;
      }
      if (range2.Row >= range1.Row && range2.Row <= range1.LastRow + 1)
        return range1.Worksheet[range1.Row, range1.Column, Math.Max(range1.LastRow, range2.LastRow), range1.LastColumn];
    }
    if (num3 == num4 && range1.Row == range2.Row)
    {
      if (range2.Column < range1.Column)
      {
        IRange range = range1;
        range1 = range2;
        range2 = range;
      }
      if (range2.Column >= range1.Column && range2.Column <= range1.LastColumn + 1)
        return range1.Worksheet[range1.Row, range1.Column, range1.LastRow, Math.Max(range1.LastColumn, range2.LastColumn)];
    }
    return (IRange) null;
  }

  private IRange[] Find(string value)
  {
    this.ParseData();
    return this.ConvertCellListIntoRange(this.m_dicRecordsCells.Find(this.m_book.InnerSST.GetStringIndexes(value)));
  }

  internal IRange[] FindRangesWithValues(
    string oldValue,
    ExcelFindOptions findOptions,
    IRange ranges)
  {
    if (this.IsPasswordProtected)
      throw new ApplicationException("You cannot use this command on a protected sheet");
    return oldValue.Equals(string.Empty) ? this.FindEmpty(ranges, oldValue, false, ExcelFindType.Formula) : this.Find(ranges, oldValue, ExcelFindType.Formula, findOptions, false);
  }

  public void Replace(string oldValue, string newValue)
  {
    this.Replace(oldValue, newValue, ExcelFindOptions.None);
  }

  public void Replace(string oldValue, string newValue, ExcelFindOptions findOptions)
  {
    IRange[] rangesWithValues = this.FindRangesWithValues(oldValue, findOptions, this.UsedRange);
    this.ReplaceWithValues(oldValue, newValue, rangesWithValues, findOptions);
  }

  internal void ReplaceWithValues(
    string oldValue,
    string newValue,
    IRange[] arrRange,
    ExcelFindOptions findOptions)
  {
    if (this.IsPasswordProtected)
      throw new ApplicationException("You cannot use this command on a protected sheet");
    if (arrRange == null)
      return;
    for (int index = 0; index < arrRange.Length; ++index)
    {
      bool throwOnUnknownNames = this.m_book.ThrowOnUnknownNames;
      this.m_book.ThrowOnUnknownNames = false;
      string input = arrRange[index].Value;
      if (oldValue.Contains("[") && !oldValue.Contains("]"))
      {
        oldValue = oldValue.Replace("[", string.Empty);
        input = input.Replace("[", string.Empty);
      }
      Regex regex = (findOptions & ExcelFindOptions.MatchEntireCellContent) != ExcelFindOptions.None ? (!oldValue.Contains("[") || !oldValue.Contains("]") ? new Regex(input.Replace("^", "\\^").Replace("$", "\\$").Replace("|", "\\|").Replace("?", "\\?").Replace("*", "\\*").Replace("+", "\\+").Replace("{", "\\{").Replace("}", "\\}").Replace("(", "\\(").Replace(")", "\\)"), RegexOptions.None) : new Regex($"\\[{input}+\\]", RegexOptions.None)) : (!oldValue.Contains("[") || !oldValue.Contains("]") ? new Regex(oldValue.Replace("^", "\\^").Replace("$", "\\$").Replace("|", "\\|").Replace("?", "\\?").Replace("*", "\\*").Replace("+", "\\+").Replace("{", "\\{").Replace("}", "\\}").Replace("(", "\\(").Replace(")", "\\)"), RegexOptions.IgnoreCase) : new Regex($"\\[{oldValue}+\\]", RegexOptions.IgnoreCase));
      arrRange[index].Value = regex.Replace(input, newValue);
      this.m_book.ThrowOnUnknownNames = throwOnUnknownNames;
    }
  }

  public void Replace(string oldValue, DateTime newValue)
  {
    if (this.IsPasswordProtected)
      throw new ApplicationException("You cannot use this command on a protected sheet");
    IRange[] rangeArray = this.Find(oldValue);
    int length = rangeArray != null ? rangeArray.Length : 0;
    for (int index = 0; index < length; ++index)
      rangeArray[index].DateTime = newValue;
  }

  public void Replace(string oldValue, double newValue)
  {
    if (this.IsPasswordProtected)
      throw new ApplicationException("You cannot use this command on a protected sheet");
    IRange[] rangeArray = this.Find(oldValue);
    int length = rangeArray != null ? rangeArray.Length : 0;
    for (int index = 0; index < length; ++index)
      rangeArray[index].Number = newValue;
  }

  public void Replace(string oldValue, string[] newValues, bool isVertical)
  {
    if (this.IsPasswordProtected)
      throw new ApplicationException("You cannot use this command on a protected sheet");
    IRange[] rangeArray = this.Find(oldValue);
    int length = rangeArray != null ? rangeArray.Length : 0;
    for (int index = 0; index < length; ++index)
      ((RangeImpl) rangeArray[index]).Replace(oldValue, newValues, isVertical);
  }

  public void Replace(string oldValue, int[] newValues, bool isVertical)
  {
    if (this.IsPasswordProtected)
      throw new ApplicationException("You cannot use this command on a protected sheet");
    IRange[] rangeArray = this.Find(oldValue);
    int length = rangeArray != null ? rangeArray.Length : 0;
    for (int index = 0; index < length; ++index)
      ((RangeImpl) rangeArray[index]).Replace(oldValue, newValues, isVertical);
  }

  public void Replace(string oldValue, double[] newValues, bool isVertical)
  {
    if (this.IsPasswordProtected)
      throw new ApplicationException("You cannot use this command on a protected sheet");
    IRange[] rangeArray = this.Find(oldValue);
    int length = rangeArray != null ? rangeArray.Length : 0;
    for (int index = 0; index < length; ++index)
      ((RangeImpl) rangeArray[index]).Replace(oldValue, newValues, isVertical);
  }

  public void Replace(string oldValue, DataTable newValues, bool isFieldNamesShown)
  {
    if (this.IsPasswordProtected)
      throw new ApplicationException("You cannot use this command on a protected sheet");
    IRange[] rangeArray = this.Find(oldValue);
    int length = rangeArray != null ? rangeArray.Length : 0;
    for (int index = 0; index < length; ++index)
      ((RangeImpl) rangeArray[index]).Replace(oldValue, newValues, isFieldNamesShown);
  }

  public void Replace(string oldValue, DataColumn column, bool isFieldNamesShown)
  {
    if (this.IsPasswordProtected)
      throw new ApplicationException("You cannot use this command on a protected sheet");
    IRange[] rangeArray = this.Find(oldValue);
    int length = rangeArray != null ? rangeArray.Length : 0;
    for (int index = 0; index < length; ++index)
      ((RangeImpl) rangeArray[index]).Replace(oldValue, column, isFieldNamesShown);
  }

  public void Remove()
  {
    this.ParseData();
    if (this.m_dataValidation != null)
      this.m_dataValidation.Clear();
    if (this.m_arrConditionalFormats != null)
      this.m_arrConditionalFormats.Clear();
    if (this.m_pivotTables != null)
    {
      this.m_pivotTables.Clear();
      this.m_book.RemoveUnusedCaches();
    }
    this.m_book.InnerWorksheets.InnerRemove(this.Index);
    this.m_names.Clear();
    if (this.m_listObjects != null && this.m_listObjects.Count > 0)
    {
      for (int index = 0; index < this.m_listObjects.Count; ++index)
      {
        if (this.m_listObjects[index].TableType == ExcelTableType.queryTable)
          this.m_book.Connections.Remove((IConnection) this.m_listObjects[index].QueryTable.ExternalConnection);
        this.m_book.Names.Remove(this.m_listObjects[index].Name);
      }
    }
    if (this.m_listObjects != null)
      this.m_listObjects.Dispose();
    if (this.m_book != null && (this.m_book.HasVbaProject || this.m_book.HasMacros) && this.VbaModule != null)
      this.m_book.VbaProject.Modules.Remove(this.VbaModule.Name);
    this.Dispose();
  }

  public void Move(int iNewIndex)
  {
    int realIndex = this.RealIndex;
    int worksheetNotBefore = this.FindWorksheetNotBefore(iNewIndex);
    this.m_book.Objects.Move(realIndex, iNewIndex);
    this.m_book.InnerWorksheets.Move(this.Index, worksheetNotBefore);
  }

  private int FindWorksheetNotBefore(int iNewIndex)
  {
    int index = iNewIndex;
    for (int objectCount = this.m_book.ObjectCount; index < objectCount; ++index)
    {
      if (this.m_book.Objects[index] is IWorksheet worksheet)
        return worksheet.Index;
    }
    return this.m_book.Worksheets.Count - 1;
  }

  public void SetColumnWidth(int iColumn, double value)
  {
    this.SetColumnWidth(iColumn, value, false);
  }

  public void SetColumnWidthInPixels(int iColumn, int value)
  {
    this.SetColumnWidthInPixels(iColumn, value, false);
  }

  internal void SetColumnWidthInPixels(int iColumn, int value, bool isBestFit)
  {
    this.ParseData();
    double columnWidth = this.PixelsToColumnWidth(value);
    this.SetColumnWidth(iColumn, columnWidth, isBestFit);
  }

  internal void SetColumnWidth(int iColumn, double value, bool isBestFit)
  {
    if (iColumn < 1 || iColumn > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException("Column", "Column index cannot be larger then 256 or less then one");
    if (this.InnerGetColumnWidth(iColumn) == value)
      return;
    ColumnInfoRecord record = this.m_arrColumnInfo[iColumn];
    if (record == null)
    {
      record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
      record.FirstColumn = record.LastColumn = (ushort) (iColumn - 1);
      record.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
      record.ColumnWidth = (ushort) (this.Application.StandardWidth * 256.0);
      record.IsUserSet = true;
      this.m_arrColumnInfo[iColumn] = record;
    }
    if (value == 0.0)
    {
      record.IsHidden = true;
    }
    else
    {
      if (value > (double) byte.MaxValue)
        value = (double) byte.MaxValue;
      record.IsHidden = false;
      record.ColumnWidth = (ushort) (value * 256.0);
      this.RaiseColumnWidthChangedEvent(iColumn, value);
    }
    record.IsBestFit = isBestFit;
    this.SetChanged();
  }

  public void SetColumnWidthInPixels(int iStartColumnIndex, int iCount, int value)
  {
    this.ParseData();
    double columnWidth = this.PixelsToColumnWidth(value);
    for (int index = 0; index < iCount; ++index)
      this.SetColumnWidth(iStartColumnIndex++, columnWidth);
  }

  public void SetRowHeight(int iRow, double value)
  {
    this.InnerSetRowHeight(iRow, value, true, MeasureUnits.Point, true);
  }

  public void SetRowHeightInPixels(int iRowIndex, double value)
  {
    if (iRowIndex < 1 || iRowIndex > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (iRowIndex), "Value cannot be less 1 and greater than max row index.");
    if (value < 0.0)
      throw new ArgumentOutOfRangeException(nameof (value));
    this.InnerSetRowHeight(iRowIndex, value, true, MeasureUnits.Pixel, true);
  }

  public void SetRowHeightInPixels(int iStartRowIndex, int iCount, double value)
  {
    if (iStartRowIndex < 1 || iStartRowIndex > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException("Row Index", "value cannot be less than 1 and greater than max row index");
    if (iStartRowIndex + iCount > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException("End Row Index", "Value cannot be greater than max row index");
    if (value < 0.0)
      throw new ArgumentOutOfRangeException(nameof (value));
    for (int index = 0; index < iCount; ++index)
      this.InnerSetRowHeight(iStartRowIndex++, value, true, MeasureUnits.Pixel, true);
  }

  public IRange FindFirst(string findValue, ExcelFindType flags)
  {
    return this.FindFirst(findValue, flags, ExcelFindOptions.None);
  }

  public IRange FindStringStartsWith(string findValue, ExcelFindType flags)
  {
    return this.FindStringStartsWith(findValue, flags, false);
  }

  public IRange FindStringStartsWith(string findValue, ExcelFindType flags, bool ignoreCase)
  {
    this.m_book.IsStartsOrEndsWith = new bool?(true);
    ExcelFindOptions findOptions = ignoreCase ? ExcelFindOptions.None : ExcelFindOptions.MatchCase;
    return this.FindFirst(findValue, flags, findOptions);
  }

  public IRange FindStringEndsWith(string findValue, ExcelFindType flags)
  {
    return this.FindStringEndsWith(findValue, flags, false);
  }

  public IRange FindStringEndsWith(string findValue, ExcelFindType flags, bool ignoreCase)
  {
    this.m_book.IsStartsOrEndsWith = new bool?(false);
    ExcelFindOptions findOptions = ignoreCase ? ExcelFindOptions.None : ExcelFindOptions.MatchCase;
    return this.FindFirst(findValue, flags, findOptions);
  }

  public IRange FindFirst(string findValue, ExcelFindType flags, ExcelFindOptions findOptions)
  {
    IRange[] rangeArray = (IRange[]) null;
    bool flag = (flags & ExcelFindType.Comments) == ExcelFindType.Comments;
    if (findValue.Equals(string.Empty))
    {
      if (flag)
        rangeArray = this.FindInComments(findValue, findOptions);
      else if (this.FirstRow == 1)
        rangeArray = this.FindEmpty(this.UsedRange, findValue, true, flags);
      else
        rangeArray[0] = this["A1"];
    }
    else
      rangeArray = this.Find(this.UsedRange, findValue, flags, findOptions, true);
    return rangeArray?[0];
  }

  public IRange FindFirst(double findValue, ExcelFindType flags)
  {
    return this.Find(this.UsedRange, findValue, flags, true)?[0];
  }

  public IRange FindFirst(bool findValue)
  {
    return this.Find(this.UsedRange, findValue ? (byte) 1 : (byte) 0, false, true)?[0];
  }

  public IRange FindFirst(DateTime findValue)
  {
    return this.Find(this.UsedRange, UtilityMethods.ConvertDateTimeToNumber(findValue), ExcelFindType.Number, true)?[0];
  }

  public IRange FindFirst(TimeSpan findValue)
  {
    return this.Find(this.UsedRange, (double) findValue.Days + (double) (findValue.Hours * 360000 + findValue.Minutes * 6000 + findValue.Seconds * 100 + findValue.Milliseconds) / 8640000.0, ExcelFindType.Number, true)?[0];
  }

  public IRange[] FindAll(string findValue, ExcelFindType flags)
  {
    return this.FindAll(findValue, flags, ExcelFindOptions.None);
  }

  public IRange[] FindAll(string findValue, ExcelFindType flags, ExcelFindOptions findOptions)
  {
    return findValue == null ? (IRange[]) null : this.Find(this.UsedRange, findValue, flags, findOptions, false);
  }

  public IRange[] FindAll(double findValue, ExcelFindType flags)
  {
    bool flag1 = (flags & ExcelFindType.FormulaValue) == ExcelFindType.FormulaValue;
    bool flag2 = (flags & ExcelFindType.Number) == ExcelFindType.Number;
    bool flag3 = (flags & ExcelFindType.Comments) == ExcelFindType.Comments;
    if (!flag1 && !flag2 && !flag3)
      throw new ArgumentException("Parameter flags is not valid.", nameof (flags));
    return this.Find(this.UsedRange, findValue, flags, false);
  }

  public IRange[] FindAll(bool findValue)
  {
    return this.Find(this.UsedRange, findValue ? (byte) 1 : (byte) 0, false, false);
  }

  public IRange[] FindAll(DateTime findValue)
  {
    return this.FindAll(UtilityMethods.ConvertDateTimeToNumber(findValue), ExcelFindType.Number | ExcelFindType.FormulaValue);
  }

  public IRange[] FindAll(TimeSpan findValue)
  {
    return this.FindAll(findValue.TotalDays, ExcelFindType.Number | ExcelFindType.FormulaValue);
  }

  public void SaveAs(string fileName, string separator)
  {
    this.SaveAs(fileName, separator, Encoding.UTF8);
  }

  public void SaveAs(string fileName, string separator, Encoding encoding)
  {
    if (fileName == null)
      throw new ArgumentNullException("Filename");
    if (separator == null || separator == string.Empty)
      throw new ArgumentNullException(nameof (separator));
    string path = fileName.Length != 0 ? Path.GetFullPath(fileName) : throw new ArgumentException("FileName cannot be empty.");
    string directoryName = Path.GetDirectoryName(path);
    if (File.Exists(path) && (File.GetAttributes(path) & FileAttributes.ReadOnly) != (FileAttributes) 0)
      throw new IOException("Cannot save. File is readonly.");
    if (directoryName != null && directoryName.Length > 0 && !Directory.Exists(directoryName))
      Directory.CreateDirectory(directoryName);
    using (FileStream fileStream = new FileStream(path, FileMode.Create))
    {
      this.SaveAs((Stream) fileStream, separator, encoding);
      fileStream.Close();
    }
  }

  public void SaveAs(Stream stream, string separator)
  {
    this.SaveAs(stream, separator, Encoding.UTF8);
  }

  public void SaveAsInternal(Stream stream, string separator, Encoding encoding)
  {
    this.ParseData();
    StreamWriter streamWriter = new StreamWriter(stream, encoding);
    string csvQualifier = this.Application.CsvQualifier;
    if (!this.IsEmpty)
    {
      for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
      {
        if (!this.IsRowEmpty(iFirstRow, false))
        {
          for (int iFirstColumn = this.m_iFirstColumn; iFirstColumn <= this.m_iLastColumn; ++iFirstColumn)
          {
            long cellIndex = RangeImpl.GetCellIndex(iFirstColumn, iFirstRow);
            WorksheetImpl.TRangeValueType cellType = this.m_dicRecordsCells.GetCellType(iFirstRow, iFirstColumn);
            string empty = string.Empty;
            if (cellType != WorksheetImpl.TRangeValueType.Blank)
            {
              string str = this.m_dicRecordsCells.GetValue(cellIndex, iFirstRow, iFirstColumn, this.Range, separator);
              if ((!str.StartsWith(csvQualifier) || !str.EndsWith(csvQualifier)) && (str.Contains('\n'.ToString()) || str.Contains(separator)))
                str = csvQualifier + str + csvQualifier;
              if (this.Application.UseStringDelimiter && (!str.StartsWith(csvQualifier) || !str.EndsWith(csvQualifier)) && cellType == WorksheetImpl.TRangeValueType.String)
                str = '"'.ToString() + str + (object) '"';
              streamWriter.Write(str);
            }
            if (iFirstColumn != this.m_iLastColumn)
              streamWriter.Write(separator);
          }
        }
        streamWriter.Write(this.Application.CsvRecordDelimiter);
      }
    }
    if (this.AppImplementation.EvalExpired)
      streamWriter.WriteLine("Created with a trial version of Syncfusion Essential XlsIO");
    streamWriter.Flush();
    stream.Flush();
  }

  public void SaveAs(Stream stream, string separator, Encoding encoding)
  {
    if (stream == null)
      throw new ArgumentException(nameof (stream));
    if (separator == null || separator.Length == 0)
      throw new ArgumentException(nameof (separator));
    this.SaveAsInternal(stream, separator, encoding);
  }

  public void SaveAsHtml(string filename) => this.SaveAsHtml(filename, HtmlSaveOptions.Default);

  public void SaveAsHtml(Stream stream) => this.SaveAsHtml(stream, HtmlSaveOptions.Default);

  public void SaveAsHtml(string fileName, HtmlSaveOptions saveOption)
  {
    switch (fileName)
    {
      case null:
        throw new ArgumentNullException("Filename");
      case "":
        throw new ArgumentException("FileName cannot be empty.");
      default:
        string fullPath = Path.GetFullPath(fileName);
        if (File.Exists(fullPath))
          File.Delete(fullPath);
        string withoutExtension = Path.GetFileNameWithoutExtension(fullPath);
        string str = Path.Combine(Path.GetDirectoryName(fullPath), $"{withoutExtension}_files");
        if ((this.HasPictures || this.HasCharts) && (saveOption.ImagePath == null || saveOption.ImagePath.Equals(string.Empty)))
        {
          if (Directory.Exists(str))
          {
            Directory.Delete(str, true);
            Directory.CreateDirectory(str);
          }
          else
            Directory.CreateDirectory(str);
          saveOption.ImagePath = str;
        }
        using (FileStream fileStream = new FileStream(fileName, FileMode.CreateNew))
        {
          new ExcelToHtmlConverter().ConvertToHtml((Stream) fileStream, this, str, saveOption);
          fileStream.Close();
          break;
        }
    }
  }

  public void SaveAsHtml(Stream stream, HtmlSaveOptions saveOption)
  {
    ExcelToHtmlConverter excelToHtmlConverter = new ExcelToHtmlConverter();
    string outputDirectoryPath = (string) null;
    if (!Directory.Exists(saveOption.ImagePath) && saveOption.ImagePath != null)
      throw new ArgumentException("Image Path doesn't exist");
    if (saveOption.ImagePath != null)
      outputDirectoryPath = Path.GetFullPath(saveOption.ImagePath);
    excelToHtmlConverter.ConvertToHtml(stream, this, outputDirectoryPath, saveOption);
    stream.Flush();
  }

  public void SetDefaultColumnStyle(int iColumnIndex, IStyle defaultStyle)
  {
    this.SetDefaultRowColumnStyle(iColumnIndex, iColumnIndex, defaultStyle, (IList) this.m_arrColumnInfo, new WorksheetImpl.OutlineDelegate(this.CreateColumnOutline), false);
    WorksheetHelper.AccessRow((IInternalWorksheet) this, iColumnIndex);
  }

  public void SetDefaultColumnStyle(
    int iStartColumnIndex,
    int iEndColumnIndex,
    IStyle defaultStyle)
  {
    this.ParseData();
    ushort correctIndex = (ushort) this.ConvertStyleToCorrectIndex(defaultStyle);
    for (int index = iStartColumnIndex; index <= iEndColumnIndex; ++index)
    {
      ((IOutline) this.m_arrColumnInfo[index] ?? this.CreateColumnOutline(index)).ExtendedFormatIndex = correctIndex;
      this.SetCellStyle(index, correctIndex);
    }
    WorksheetHelper.AccessColumn((IInternalWorksheet) this, iStartColumnIndex);
    WorksheetHelper.AccessColumn((IInternalWorksheet) this, iEndColumnIndex);
  }

  public void SetDefaultRowStyle(int iRowIndex, IStyle defaultStyle)
  {
    ushort correctIndex = (ushort) this.ConvertStyleToCorrectIndex(defaultStyle);
    WorksheetHelper.AccessRow((IInternalWorksheet) this, iRowIndex);
    --iRowIndex;
    RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iRowIndex, true);
    if (this.Rows.Length > 0)
    {
      foreach (IRange cell in this.Rows[iRowIndex].Cells)
        row.SetCellStyle(iRowIndex, cell.Column - 1, (int) correctIndex, this.Application.RowStorageAllocationBlockSize);
    }
    row.ExtendedFormatIndex = correctIndex;
  }

  public void SetDefaultRowStyle(int iStartRowIndex, int iEndRowIndex, IStyle defaultStyle)
  {
    ushort correctIndex = (ushort) this.ConvertStyleToCorrectIndex(defaultStyle);
    WorksheetHelper.AccessRow((IInternalWorksheet) this, iStartRowIndex);
    WorksheetHelper.AccessRow((IInternalWorksheet) this, iEndRowIndex);
    --iStartRowIndex;
    --iEndRowIndex;
    for (int index = iStartRowIndex; index <= iEndRowIndex; ++index)
    {
      RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, index, true);
      if (this.Rows.Length > 0)
      {
        foreach (IRange cell in this.Rows[index].Cells)
          row.SetCellStyle(index, cell.Column - 1, (int) correctIndex, this.Application.RowStorageAllocationBlockSize);
      }
      row.ExtendedFormatIndex = correctIndex;
    }
  }

  private void SetCellStyle(int iColIndex, ushort XFindex)
  {
    for (int rowIndex = this.CellRecords.FirstRow - 1; rowIndex <= this.CellRecords.LastRow; ++rowIndex)
    {
      RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, rowIndex, false);
      if (row != null && row.ExtendedFormatIndex != (ushort) 0)
      {
        ICellPositionFormat cellRecord = this.m_dicRecordsCells.GetCellRecord(rowIndex + 1, iColIndex);
        if (cellRecord != null)
        {
          cellRecord.ExtendedFormatIndex = XFindex;
          this.m_dicRecordsCells.AddRecord(cellRecord, false);
        }
        else
        {
          ICellPositionFormat cell = this.m_dicRecordsCells.CreateCell(rowIndex + 1, iColIndex, TBIFFRecord.Blank);
          cell.ExtendedFormatIndex = XFindex;
          this.m_dicRecordsCells.AddRecord(cell, false);
        }
      }
    }
  }

  public IStyle GetDefaultColumnStyle(int iColumnIndex)
  {
    this.ParseData();
    if (iColumnIndex < 1 || iColumnIndex > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (iColumnIndex), "Value cannot be less than 1 and greater than m_book.MaxColumnCount.");
    IOutline outline = (IOutline) this.m_arrColumnInfo[iColumnIndex];
    return (IStyle) new ExtendedFormatWrapper(this.m_book, outline != null ? (int) outline.ExtendedFormatIndex : this.m_book.DefaultXFIndex);
  }

  public IStyle GetDefaultRowStyle(int iRowIndex)
  {
    if (iRowIndex < 1 || iRowIndex > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (iRowIndex), "Value cannot be less than 1 and greater than m_book.MaxColumnCount.");
    RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iRowIndex - 1, false);
    return (IStyle) new ExtendedFormatWrapper(this.m_book, row == null || !this.m_book.IsFormatted((int) row.ExtendedFormatIndex) ? this.m_book.DefaultXFIndex : (int) row.ExtendedFormatIndex);
  }

  public void FreeRange(IRange range)
  {
    int row = range.Row;
    for (int lastRow = range.LastRow; row <= lastRow; ++row)
    {
      int column = range.Column;
      for (int lastColumn = range.LastColumn; column <= lastColumn; ++column)
        this.FreeRange(row, column);
    }
  }

  public void FreeRange(int iRow, int iColumn)
  {
    this.ParseData();
    this.CellRecords.FreeRange(iRow, iColumn);
  }

  public System.Drawing.Image ConvertToImage(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn)
  {
    return this.ConvertToImage(firstRow, firstColumn, lastRow, lastColumn, ImageType.Bitmap, (Stream) null);
  }

  public System.Drawing.Image ConvertToImage(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    ImageType imageType,
    Stream outputStream)
  {
    return this.ConvertToImage(firstRow, firstColumn, lastRow, lastColumn, imageType, outputStream, EmfType.EmfOnly);
  }

  public System.Drawing.Image ConvertToImage(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    EmfType emfType,
    Stream outputStream)
  {
    return this.ConvertToImage(firstRow, firstColumn, lastRow, lastColumn, ImageType.Metafile, outputStream, emfType);
  }

  public System.Drawing.Image ConvertToImage(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    ImageType imageType,
    Stream outputStream,
    EmfType emfType)
  {
    return new WorksheetImageConverter().ConvertToImage(this, firstRow, firstColumn, lastRow, lastColumn, imageType, outputStream, emfType);
  }

  public IRange TopLeftCell
  {
    get => this[this.TopVisibleRow, this.LeftVisibleColumn];
    set
    {
      if (this.IsFreezePanes)
      {
        if (value.Row <= this.PaneFirstVisible.Row || value.Column <= this.PaneFirstVisible.Column)
          return;
        this.FirstVisibleRow = value.Row - 1;
        this.FirstVisibleColumn = value.Column - 1;
      }
      else
      {
        this.TopVisibleRow = value.Row;
        this.LeftVisibleColumn = value.Column;
      }
    }
  }

  private void ImportDataTableWithoutCheck(
    DataTable dataTable,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    DataColumn[] arrColumns,
    bool isOptimized)
  {
    if (isOptimized)
    {
      for (int index1 = 0; index1 < maxRows; ++index1)
      {
        DataRow row = dataTable.Rows[index1];
        for (int index2 = 0; index2 < maxColumns; ++index2)
        {
          DataColumn arrColumn = arrColumns[index2];
          this.SetString(firstRow + index1, firstColumn + index2, row[arrColumn].ToString());
        }
      }
    }
    else
    {
      for (int index3 = 0; index3 < maxRows; ++index3)
      {
        DataRow row = dataTable.Rows[index3];
        for (int index4 = 0; index4 < maxColumns; ++index4)
        {
          DataColumn arrColumn = arrColumns[index4];
          object obj = row[arrColumn];
          if (obj != DBNull.Value)
          {
            switch (arrColumn.DataType.Name)
            {
              case "String":
                MigrantRangeImpl migrantRangeImpl1 = new MigrantRangeImpl(this.Application, (IWorksheet) this);
                migrantRangeImpl1.ResetRowColumn(firstRow + index3, firstColumn + index4);
                if (!this.CheckAndAddHyperlink((string) row[arrColumn], this[firstRow + index3, firstColumn + index4] as RangeImpl))
                {
                  migrantRangeImpl1.Value2 = (object) (string) row[arrColumn];
                  continue;
                }
                continue;
              case "DateTime":
                if (obj is DateTime dateTime && dateTime >= RangeImpl.DEF_MIN_DATETIME)
                {
                  this.IsImporting = true;
                  MigrantRangeImpl migrantRangeImpl2 = new MigrantRangeImpl(this.Application, (IWorksheet) this);
                  migrantRangeImpl2.ResetRowColumn(firstRow + index3, firstColumn + index4);
                  migrantRangeImpl2.Value2 = row[arrColumn];
                  this.IsImporting = false;
                  continue;
                }
                this.SetString(firstRow + index3, firstColumn + index4, obj.ToString());
                continue;
              case "Double":
                this.SetNumber(firstRow + index3, firstColumn + index4, (double) obj);
                continue;
              case "Int32":
                this.SetNumber(firstRow + index3, firstColumn + index4, (double) (int) obj);
                continue;
              case "Boolean":
                MigrantRangeImpl migrantRangeImpl3 = new MigrantRangeImpl(this.Application, (IWorksheet) this);
                migrantRangeImpl3.ResetRowColumn(firstRow + index3, firstColumn + index4);
                migrantRangeImpl3.Value2 = row[arrColumn];
                continue;
              default:
                this.SetValueRowCol(obj, firstRow + index3, firstColumn + index4);
                continue;
            }
          }
        }
      }
    }
  }

  private void ImportDataTableWithoutCheckPreserve(
    DataTable dataTable,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    DataColumn[] arrColumns)
  {
    for (int index1 = 0; index1 < maxRows; ++index1)
    {
      DataRow row = dataTable.Rows[index1];
      for (int index2 = 0; index2 < maxColumns; ++index2)
      {
        DataColumn arrColumn = arrColumns[index2];
        object stringValue = row[arrColumn];
        if (stringValue != DBNull.Value)
        {
          switch (arrColumn.DataType.Name)
          {
            case "String":
              this.CheckAndAddHyperlink((string) stringValue, this[firstRow + index1, firstColumn + index2] as RangeImpl);
              this.SetString(firstRow + index1, firstColumn + index2, (string) stringValue);
              continue;
            case "DateTime":
              if (stringValue is DateTime dateTime && dateTime >= RangeImpl.DEF_MIN_DATETIME)
              {
                this.IsImporting = true;
                MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this);
                migrantRangeImpl.ResetRowColumn(firstRow + index1, firstColumn + index2);
                migrantRangeImpl.Value2 = row[arrColumn];
                this.IsImporting = false;
                continue;
              }
              this.SetString(firstRow + index1, firstColumn + index2, stringValue.ToString());
              continue;
            case "Double":
              this.SetNumber(firstRow + index1, firstColumn + index2, (double) stringValue);
              continue;
            case "Int32":
              this.SetNumber(firstRow + index1, firstColumn + index2, (double) (int) stringValue);
              continue;
            case "Boolean":
              MigrantRangeImpl migrantRangeImpl1 = new MigrantRangeImpl(this.Application, (IWorksheet) this);
              migrantRangeImpl1.ResetRowColumn(firstRow + index1, firstColumn + index2);
              migrantRangeImpl1.Value2 = row[arrColumn];
              continue;
            default:
              this.SetValueRowCol(stringValue, firstRow + index1, firstColumn + index2);
              continue;
          }
        }
      }
    }
  }

  private void ImportDataViewWithoutCheck(
    DataView dataView,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns)
  {
    for (int recordIndex = 0; recordIndex < maxRows; ++recordIndex)
    {
      DataRowView dataRowView = dataView[recordIndex];
      for (int ndx = 0; ndx < maxColumns; ++ndx)
        this.InnerGetCell(firstColumn + ndx, firstRow + recordIndex).Value2 = dataRowView[ndx];
    }
  }

  private void ImportDataViewWithoutCheckPreserve(
    DataView dataView,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns)
  {
    Dictionary<int, WorksheetImpl.RangeProperty> hashColumnTypes = new Dictionary<int, WorksheetImpl.RangeProperty>(maxColumns);
    for (int recordIndex = 0; recordIndex < maxRows; ++recordIndex)
    {
      DataRowView dataRowView = dataView[recordIndex];
      for (int index = 0; index < maxColumns; ++index)
      {
        object obj = dataRowView[index];
        switch (obj)
        {
          case null:
          case DBNull _:
            continue;
          default:
            WorksheetImpl.RangeProperty valueType = this.GetValueType(obj, index, hashColumnTypes);
            IRange cell = this.InnerGetCell(firstColumn + index, firstRow + recordIndex);
            switch (valueType)
            {
              case WorksheetImpl.RangeProperty.Text:
                cell.Text = (string) obj;
                continue;
              case WorksheetImpl.RangeProperty.DateTime:
                cell.DateTime = (DateTime) obj;
                continue;
              case WorksheetImpl.RangeProperty.TimeSpan:
                cell.TimeSpan = (TimeSpan) obj;
                continue;
              default:
                cell.Value2 = obj;
                continue;
            }
        }
      }
    }
  }

  private WorksheetImpl.RangeProperty GetValueType(
    object value,
    int iColumnIndex,
    Dictionary<int, WorksheetImpl.RangeProperty> hashColumnTypes)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (hashColumnTypes == null)
      throw new ArgumentNullException(nameof (hashColumnTypes));
    if (hashColumnTypes.ContainsKey(iColumnIndex))
      return hashColumnTypes[iColumnIndex];
    WorksheetImpl.RangeProperty valueType = WorksheetImpl.RangeProperty.Value2;
    switch (value)
    {
      case string _:
        valueType = WorksheetImpl.RangeProperty.Text;
        break;
      case DateTime _:
        valueType = WorksheetImpl.RangeProperty.DateTime;
        break;
      case TimeSpan _:
        valueType = WorksheetImpl.RangeProperty.TimeSpan;
        break;
    }
    hashColumnTypes.Add(iColumnIndex, valueType);
    return valueType;
  }

  private List<TypeCode> GetTypeCodesFromCustomDynamic(
    object dynamicObject,
    IList<string> propertyNames,
    out Dictionary<string, CallSite<Func<CallSite, object, object>>> callSiteCollection,
    out List<int> nullableList)
  {
    callSiteCollection = new Dictionary<string, CallSite<Func<CallSite, object, object>>>(propertyNames.Count);
    List<TypeCode> fromCustomDynamic = new List<TypeCode>(propertyNames.Count);
    nullableList = (List<int>) null;
    string name = dynamicObject.GetType().Name;
    for (int index = 0; index < propertyNames.Count; ++index)
    {
      CallSite<Func<CallSite, object, object>> callSite = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, propertyNames[index], dynamicObject.GetType(), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
      callSiteCollection.Add($"{name}_{propertyNames[index]}", callSite);
      object obj = callSite.Target((CallSite) callSite, dynamicObject);
      if (obj == null)
      {
        if (nullableList == null)
          nullableList = new List<int>(propertyNames.Count);
        nullableList.Add(index);
      }
      else
        fromCustomDynamic.Add(System.Type.GetTypeCode(obj.GetType()));
    }
    return fromCustomDynamic;
  }

  internal bool CheckAndAddHyperlink(string stringValue, RangeImpl range)
  {
    bool flag = false;
    if ((stringValue.StartsWith("www.") || stringValue.StartsWith("http") || stringValue.StartsWith("https") || stringValue.StartsWith("ftp") || stringValue.StartsWith("news") || stringValue.StartsWith("file") || stringValue.IndexOf("@") > 0 && stringValue.IndexOf(".") < stringValue.Length - 2) && this.CheckHyperlink(stringValue))
    {
      IHyperLink hyperLink = this.HyperLinks.Add((IRange) range);
      hyperLink.Type = ExcelHyperLinkType.Url;
      hyperLink.Address = stringValue;
      hyperLink.TextToDisplay = stringValue;
      flag = true;
    }
    return flag;
  }

  internal bool CheckHyperlink(string stringValue)
  {
    for (int index = 0; index < WorksheetImpl.m_hyperlinkPatterns.Count; ++index)
    {
      Match match = WorksheetImpl.m_hyperlinkPatterns[index].Match(stringValue);
      if (match.Success && match.Value == stringValue)
        return true;
    }
    return false;
  }

  [CLSCompliant(false)]
  public override void Serialize(OffsetArrayList records)
  {
    if (this.ParseOnDemand || !this.IsParsed)
      records.AddList((IList) this.m_arrRecords);
    else
      this.Serialize(records, false, (IRange) null);
  }

  protected override bool ContainsProtection
  {
    get => base.ContainsProtection || this.m_errorIndicators.Count > 0;
  }

  private void SerializeErrorIndicators(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_errorIndicators == null || this.m_errorIndicators.Count == 0)
      return;
    SheetProtectionRecord sheetProtection = this.SheetProtection;
    if (sheetProtection != null && sheetProtection.ContainProtection && (sheetProtection.ProtectedOptions | 1024 /*0x0400*/) != 0)
    {
      int i = 0;
      for (int count = this.m_errorIndicators.Count; i < count; ++i)
      {
        ErrorIndicatorImpl errorIndicator = this.m_errorIndicators[i];
        if ((errorIndicator.IgnoreOptions & ExcelIgnoreError.UnlockedFormulaCells) != ExcelIgnoreError.None)
        {
          this.m_errorIndicators.Remove(errorIndicator);
          --i;
          --count;
        }
      }
    }
    if (this.m_errorIndicators.Count == 0)
      return;
    SheetProtectionRecord record = (SheetProtectionRecord) BiffRecordFactory.GetRecord(TBIFFRecord.SheetProtection);
    record.Type = (short) 3;
    records.Add(record.Clone());
    int i1 = 0;
    for (int count = this.m_errorIndicators.Count; i1 < count; ++i1)
    {
      RangeProtectionRecord protectionRecord = this.m_rangeProtectionRecord == null ? (RangeProtectionRecord) BiffRecordFactory.GetRecord(TBIFFRecord.RangeProtection) : this.m_rangeProtectionRecord;
      ErrorIndicatorImpl errorIndicator = this.m_errorIndicators[i1];
      errorIndicator.OptimizeStorage();
      protectionRecord.IgnoreOptions = errorIndicator.IgnoreOptions;
      protectionRecord.ErrorIndicator = errorIndicator;
      if (protectionRecord.GetStoreSize(ExcelVersion.Excel97to2003) > 8224)
      {
        if (!this.m_book.IsLoaded)
          throw new ArgumentOutOfRangeException("Too many regions with error indicators. Please reduce them before saving.");
        errorIndicator.SetLength(1024 /*0x0400*/);
      }
      records.Add((IBiffStorage) protectionRecord);
      if (protectionRecord.m_continueRecords != null && protectionRecord.m_continueRecords.Count > 0)
      {
        foreach (UnknownRecord continueRecord in protectionRecord.m_continueRecords)
          records.Add((IBiffStorage) continueRecord);
      }
    }
  }

  private void SerializeNotParsedWorksheet(OffsetArrayList records)
  {
    throw new NotImplementedException();
  }

  [CLSCompliant(false)]
  public void SerializeForClipboard(OffsetArrayList records)
  {
    this.SerializeForClipboard(records, true, (IRange) null);
  }

  internal void SerializeForClipboard(OffsetArrayList records, bool bClipboard, IRange range)
  {
    this.Serialize(records, bClipboard, range);
  }

  [CLSCompliant(false)]
  protected void SerializeColumnInfo(OffsetArrayList records)
  {
    int num = records != null ? this.SerializeGroupColumnInfo(records) : throw new ArgumentNullException(nameof (records));
    if (num >= (int) byte.MaxValue)
      return;
    ColumnInfoRecord record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
    record.FirstColumn = (ushort) (num + 1);
    record.LastColumn = (ushort) byte.MaxValue;
    record.ColumnWidth = (ushort) this.EvaluateFileColumnWidth((int) (this.StandardWidth * 256.0));
    record.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
    records.Add((IBiffStorage) record);
  }

  [CLSCompliant(false)]
  protected int SerializeGroupColumnInfo(OffsetArrayList records)
  {
    int index1 = 1;
    int index2 = 1;
    ColumnInfoRecord columnInfoRecord1 = (ColumnInfoRecord) null;
    for (; index1 <= 256 /*0x0100*/; index1 = index2 + 1)
    {
      for (; index1 <= 256 /*0x0100*/; ++index1)
      {
        columnInfoRecord1 = this.m_arrColumnInfo[index1];
        if (columnInfoRecord1 != null)
          break;
      }
      if (columnInfoRecord1 != null)
      {
        index2 = index1;
        ColumnInfoRecord columnInfoRecord2;
        do
        {
          ++index2;
          columnInfoRecord2 = this.m_arrColumnInfo[index2];
          if (columnInfoRecord1.CompareTo((object) columnInfoRecord2) != 0)
            columnInfoRecord2 = (ColumnInfoRecord) null;
        }
        while (index2 <= 256 /*0x0100*/ && columnInfoRecord2 != null);
        if (columnInfoRecord2 == null)
        {
          --index2;
          columnInfoRecord2 = this.m_arrColumnInfo[index2];
        }
        ColumnInfoRecord columnInfoRecord3;
        if (index1 == index2)
        {
          columnInfoRecord3 = (ColumnInfoRecord) columnInfoRecord1.Clone();
        }
        else
        {
          columnInfoRecord3 = (ColumnInfoRecord) columnInfoRecord1.Clone();
          columnInfoRecord3.LastColumn = columnInfoRecord2.LastColumn;
        }
        columnInfoRecord3.ColumnWidth = (ushort) this.EvaluateFileColumnWidth((int) columnInfoRecord3.ColumnWidth);
        records.Add((IBiffStorage) columnInfoRecord3);
      }
      else
        break;
    }
    return index2 - 1;
  }

  [CLSCompliant(false)]
  protected void SerializeConditionalFormatting(OffsetArrayList records)
  {
    this.m_arrConditionalFormats.Serialize(records);
  }

  [CLSCompliant(false)]
  protected void SerializeDataValidation(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_dataValidation == null)
      return;
    int index = 0;
    for (int count = this.m_dataValidation.Count; index < count; ++index)
      this.m_dataValidation[index].Serialize(records);
  }

  private bool CompareDVWithoutRanges(DVRecord curDV, DVRecord dvToAdd)
  {
    if (curDV == null)
      return dvToAdd == null;
    return curDV.Condition == dvToAdd.Condition && curDV.DataType == dvToAdd.DataType && curDV.ErrorBoxText == dvToAdd.ErrorBoxText && curDV.ErrorBoxTitle == dvToAdd.ErrorBoxTitle && curDV.ErrorStyle == dvToAdd.ErrorStyle && curDV.IsEmptyCell == dvToAdd.IsEmptyCell && curDV.IsShowErrorBox == dvToAdd.IsShowErrorBox && curDV.IsShowPromptBox == dvToAdd.IsShowPromptBox && curDV.IsStrListExplicit == dvToAdd.IsStrListExplicit && curDV.IsSuppressArrow == dvToAdd.IsSuppressArrow && curDV.PromtBoxText == dvToAdd.PromtBoxText && curDV.PromtBoxTitle == dvToAdd.PromtBoxTitle && Ptg.CompareArrays(curDV.FirstFormulaTokens, dvToAdd.FirstFormulaTokens) && Ptg.CompareArrays(curDV.SecondFormulaTokens, dvToAdd.SecondFormulaTokens);
  }

  private void MergeDVRanges(DVRecord curDv, DVRecord dvToAdd)
  {
    if (curDv == null)
      throw new ArgumentNullException(nameof (curDv));
    if (dvToAdd == null)
      throw new ArgumentNullException(nameof (dvToAdd));
    curDv.AddRange(dvToAdd.AddrList);
  }

  [CLSCompliant(false)]
  protected override void SerializeMsoDrawings(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    base.SerializeMsoDrawings(records);
    if ((this.Application.SkipOnSave & SkipExtRecords.Drawings) == SkipExtRecords.Drawings || this.m_arrNotesByCellIndex == null)
      return;
    foreach (NoteRecord noteRecord in (IEnumerable<NoteRecord>) this.m_arrNotesByCellIndex.Values)
      records.Add((IBiffStorage) noteRecord);
  }

  private void Serialize(OffsetArrayList records, bool bClipboard, IRange range)
  {
    if (this.m_arrNotes != null)
    {
      this.m_arrNotes.Clear();
      this.m_arrNotesByCellIndex.Clear();
    }
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (!this.IsSupported)
    {
      records.AddList((IList) this.m_arrRecords);
    }
    else
    {
      if (!this.IsParsed)
        this.Parse();
      this.m_bof.Type = BOFRecord.TType.TYPE_WORKSHEET;
      records.Add((IBiffStorage) this.m_bof);
      IndexRecord indexRecord = (IndexRecord) null;
      int num1 = this.m_iLastRow - this.m_iFirstRow + 1;
      int length = 0;
      if (num1 > 0)
      {
        int num2 = num1 % 32 /*0x20*/;
        length = num1 / 32 /*0x20*/;
        if (num2 != 0)
          ++length;
      }
      if (!bClipboard)
      {
        indexRecord = (IndexRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Index);
        indexRecord.DbCells = new int[length];
        indexRecord.FirstRow = this.m_iLastRow != this.m_iFirstRow || this.m_iFirstRow != -1 ? this.m_iFirstRow - 1 : 0;
        indexRecord.LastRow = this.m_iLastRow != this.m_iFirstRow || this.m_iFirstRow != -1 ? this.m_iLastRow : 0;
        records.Add((IBiffStorage) indexRecord);
      }
      this.m_book.InnerCalculation.Serialize(records);
      records.Add((IBiffStorage) this.m_pageSetup);
      if (this.m_arrCustomProperties != null)
        this.m_arrCustomProperties.Serialize(records);
      this.SerializeProtection(records, false);
      DefaultColWidthRecord record1 = (DefaultColWidthRecord) BiffRecordFactory.GetRecord(TBIFFRecord.DefaultColWidth);
      record1.Width = (ushort) this.m_dStandardColWidth;
      records.Add((IBiffStorage) record1);
      this.SerializeColumnInfo(records);
      this.m_autofilters.Serialize(records);
      if (this.m_arrSortRecords != null)
        records.AddList((IList) this.m_arrSortRecords);
      DimensionsRecord record2 = (DimensionsRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Dimensions);
      if (bClipboard && range != null)
      {
        record2.LastRow = range.LastRow != range.Row || range.Row != -1 ? range.LastRow : 0;
        record2.LastColumn = range.Column != range.Column || range.Column != int.MaxValue ? (ushort) range.LastColumn : (ushort) 0;
        record2.FirstRow = range.LastRow != range.Row || range.Row != -1 ? range.Row - 1 : 0;
        record2.FirstColumn = range.LastColumn != range.Column || range.Column != int.MaxValue ? (ushort) (range.Column - 1) : (ushort) 0;
      }
      else
      {
        record2.LastRow = this.m_iLastRow != this.m_iFirstRow || this.m_iFirstRow != -1 ? this.m_iLastRow : 0;
        record2.LastColumn = this.m_iLastColumn != this.m_iFirstColumn || this.m_iFirstColumn != int.MaxValue ? (ushort) this.m_iLastColumn : (ushort) 0;
        record2.FirstRow = this.m_iLastRow != this.m_iFirstRow || this.m_iFirstRow != -1 ? this.m_iFirstRow - 1 : 0;
        record2.FirstColumn = this.m_iLastColumn != this.m_iFirstColumn || this.m_iFirstColumn != int.MaxValue ? (ushort) (this.m_iFirstColumn - 1) : (ushort) 0;
      }
      records.Add((IBiffStorage) record2);
      List<DBCellRecord> arrDBCells = new List<DBCellRecord>();
      this.m_dicRecordsCells.Serialize(records, arrDBCells);
      if (!bClipboard)
        indexRecord.DbCellRecords = arrDBCells;
      this.SerializeMsoDrawings(records);
      if (this.m_arrDConRecords != null)
        records.AddList((IList) this.m_arrDConRecords);
      if (this.m_pivotTables != null)
        this.m_pivotTables.Serialize(records);
      this.SerializeHeaderFooterPictures(records);
      this.SerializeWindowTwo(records);
      this.SerializePageLayoutView(records);
      this.SerializeWindowZoom(records);
      if (this.m_pane != null)
      {
        if (this.VerticalSplit == 0 && this.HorizontalSplit == 0)
          this.m_pane.ActivePane = (ushort) 3;
        else if (this.VerticalSplit == 0)
          this.m_pane.ActivePane = (ushort) 2;
        else if (this.HorizontalSplit == 0)
          this.m_pane.ActivePane = (ushort) 1;
        records.Add((IBiffStorage) this.m_pane);
      }
      this.CreateAllSelections();
      records.AddList((IList) this.m_arrSelections);
      if (this.m_mergedCells != null)
        this.m_mergedCells.Serialize(records);
      records.AddList((IList) this.PreserveExternalConnection);
      records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.UnkMarker));
      if (this.m_hyperlinks != null)
        this.m_hyperlinks.Serialize(records);
      this.SerializeConditionalFormatting(records);
      this.SerializeDataValidation(records);
      this.SerializeMacrosSupport(records);
      this.SerializeSheetLayout(records);
      this.SerializeSheetProtection(records);
      this.SerializeErrorIndicators(records);
      if (this.m_tableRecords != null)
        records.AddRange((ICollection) this.m_tableRecords);
      records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.EOF));
      if (this.m_arrNotes == null)
        return;
      this.m_arrNotes.Clear();
      this.m_arrNotesByCellIndex.Clear();
    }
  }

  protected void RaiseColumnWidthChangedEvent(int iColumn, double dNewValue)
  {
    if (this.ColumnWidthChanged == null)
      return;
    this.ColumnWidthChanged((object) this, new ValueChangedEventArgs((object) iColumn, (object) dNewValue, "ColumnWidth"));
  }

  protected void RaiseRowHeightChangedEvent(int iRow, double dNewValue)
  {
    this.RowHeightHelper.UpdateRowIndexValue(iRow, (float) (int) ApplicationImpl.ConvertToPixels(dNewValue, MeasureUnits.Point));
    if (this.RowHeightChanged == null)
      return;
    this.RowHeightChanged((object) this, new ValueChangedEventArgs((object) iRow, (object) dNewValue, "RowHeight"));
  }

  private void NormalFont_OnAfterChange(object sender, EventArgs e)
  {
    if (this.m_iFirstRow <= 0)
      return;
    for (int iFirstRow = this.m_iFirstRow; iFirstRow <= this.m_iLastRow; ++iFirstRow)
    {
      RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iFirstRow, false);
      if (row != null && !row.IsBadFontHeight)
        this.AutofitRow(iFirstRow);
    }
  }

  public void SetFormulaValue(int iRow, int iColumn, string value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (value != string.Empty && value[0] == '#' && FormulaUtil.ErrorNameToCode.ContainsKey(value))
    {
      this.SetFormulaErrorValue(iRow, iColumn, value);
    }
    else
    {
      string formula = (string) null;
      WorksheetImpl.TRangeValueType cellType = this.GetCellType(iRow, iColumn, false);
      if (cellType == WorksheetImpl.TRangeValueType.Formula)
        formula = RangeImpl.GetFormula(this.GetFormulaFromWorksheet(iRow, iColumn, false));
      RangeImpl rangeImpl = (RangeImpl) null;
      if (RangeImpl.GetAddressLocal(iRow, iColumn, iRow, iColumn) != formula)
        rangeImpl = this.GetFormulaRange(formula);
      bool flag = false;
      if (rangeImpl != null && (rangeImpl.CellStyle.IsFirstSymbolApostrophe || rangeImpl.NumberFormat == "@"))
      {
        this.SetFormulaStringValue(iRow, iColumn, value);
        flag = true;
      }
      if (flag)
        return;
      string numberFormat = this.Range[iRow, iColumn].NumberFormat;
      double result1;
      if (double.TryParse(value, out result1) && formula != null && !formula.Equals("TEXT") && (!numberFormat.Contains("@") || numberFormat.Length != 1) && this.checkIsNumber(value, CultureInfo.CurrentCulture) && !value.Trim().EndsWith("."))
      {
        this.SetFormulaNumberValue(iRow, iColumn, result1);
      }
      else
      {
        DateTime dateValue;
        if (cellType == WorksheetImpl.TRangeValueType.Formula && this.CheckItRefersToSingleCell(iRow, iColumn) && this.TryParseDateTime(value, out dateValue))
        {
          this.SetFormulaNumberValue(iRow, iColumn, dateValue.ToOADate());
        }
        else
        {
          bool result2;
          if (bool.TryParse(value, out result2))
            this.SetFormulaBoolValue(iRow, iColumn, result2);
          else
            this.SetFormulaStringValue(iRow, iColumn, value);
        }
      }
    }
  }

  internal bool TryParseDateTime(string value, out DateTime dateValue)
  {
    if (this.m_book.DetectDateTimeInValue && value.Contains(RangeImpl.DateSeperator))
      return DateTime.TryParse(value, out dateValue);
    dateValue = DateTime.MinValue;
    return false;
  }

  internal bool TryParseExactDateTime(string value, out DateTime result)
  {
    if (!Regex.IsMatch(value, $"^(\\d{{1,4}})(:(\\d{{1,2}}))?:(\\d{{1,2}})({NumberFormatInfo.CurrentInfo.NumberDecimalSeparator}[\\d]+){{0,1}}[\\s]*$") && !Regex.IsMatch(value, $"^([0-9]|1[0-2])(:(\\d{{1,2}}))?:(\\d{{1,2}})({NumberFormatInfo.CurrentInfo.NumberDecimalSeparator}[\\d]+){{0,1}}([\\s]+(AM|PM|am|pm|aM|pM|Am|Pm)){{0,1}}[\\s]*$"))
      return DateTime.TryParseExact(value, "", (IFormatProvider) null, DateTimeStyles.None, out result);
    int int16 = (int) Convert.ToInt16(value.Substring(0, value.IndexOf(":")));
    int num1 = 0;
    if (!value.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator) || value.Substring(0, value.IndexOf(":")).Contains(":"))
    {
      if (int16 >= 24)
      {
        num1 = int16 / 24;
        int num2 = int16 % 24;
        value = value.Replace(value.Substring(0, value.IndexOf(":") + 1), num2.ToString() + ":");
      }
    }
    if (DateTime.TryParse(value, (IFormatProvider) null, DateTimeStyles.None, out result))
    {
      result = result.AddDays(Convert.ToDouble(num1));
      return true;
    }
    if (!DateTime.TryParseExact(value, $"mm:ss{NumberFormatInfo.CurrentInfo.NumberDecimalSeparator}0", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
      return false;
    result = result.AddDays(Convert.ToDouble(num1));
    return true;
  }

  internal RangeImpl GetFormulaRange(string formula)
  {
    string strSheetName = string.Empty;
    rangeImpl = (RangeImpl) null;
    string str1;
    string str2;
    string strRow2;
    string strColumn2;
    return (FormulaUtil.IsCell(formula, false, out str1, out str2) || FormulaUtil.IsCell3D(formula, false, out strSheetName, out str1, out str2) || (this.Workbook as WorkbookImpl).FormulaUtil.IsCellRange3D(formula, false, out strSheetName, out str1, out str2, out strRow2, out strColumn2) || (this.Workbook as WorkbookImpl).FormulaUtil.IsCellRange(formula, false, out str1, out str2, out strRow2, out strColumn2)) && this.GetRangeByString(formula, false) != null && this.GetRangeByString(formula, false) is RangeImpl rangeImpl && rangeImpl.HasFormula ? this.GetFormulaRange(RangeImpl.GetFormula(rangeImpl.Formula)) : rangeImpl;
  }

  public void SetValue(int iRow, int iColumn, string value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    WorksheetImpl.TRangeValueType cellType = this.GetCellType(iRow, iColumn, false);
    if (cellType != WorksheetImpl.TRangeValueType.Formula && value.Length == 0)
      this.SetBlankRecord(iRow, iColumn);
    else if (!string.IsNullOrEmpty(value) && value[0] == '=')
      this.SetFormula(iRow, iColumn, value.Substring(1));
    else if (!string.IsNullOrEmpty(value) && value[0] == '#' && FormulaUtil.ErrorNameToCode.ContainsKey(value) && this.GetFormulaErrorValue(iRow, iColumn) != null)
      this.SetFormulaErrorValue(iRow, iColumn, value);
    else if (cellType == WorksheetImpl.TRangeValueType.Formula)
    {
      this.SetFormulaValue(iRow, iColumn, value);
    }
    else
    {
      RangeImpl rangeImpl = this.Range[iRow, iColumn] as RangeImpl;
      bool dateTime = this.TryParseDateTime(value, out DateTime _);
      double result;
      bool flag = double.TryParse(value, out result);
      CultureInfo cultureInfo = this.AppImplementation.CheckAndApplySeperators();
      if (flag)
        flag = this.checkIsNumber(value, cultureInfo);
      if (flag && !dateTime)
        this.SetNumber(iRow, iColumn, result);
      else if (dateTime)
        rangeImpl.Value = value;
      else
        this.SetString(iRow, iColumn, value);
    }
  }

  internal bool checkIsNumber(string value, CultureInfo cultureInfo)
  {
    bool flag = true;
    if (value.Contains(cultureInfo.NumberFormat.NumberDecimalSeparator))
    {
      if (new Regex($"[{cultureInfo.NumberFormat.NumberDecimalSeparator}]").Matches(value).Count > 1)
        return false;
      if (value.Contains(cultureInfo.NumberFormat.NumberGroupSeparator))
      {
        int length = value.IndexOf(cultureInfo.NumberFormat.NumberDecimalSeparator);
        string str = value.Substring(0, length);
        if (value.Substring(length + 1, value.Length - 1 - length).Contains(cultureInfo.NumberFormat.NumberGroupSeparator))
          return false;
        flag = this.checkGroupSeparatorPosition(str, cultureInfo);
      }
    }
    else
      flag = this.checkGroupSeparatorPosition(value, cultureInfo);
    return flag;
  }

  private bool checkGroupSeparatorPosition(string value, CultureInfo cultureInfo)
  {
    value = value.TrimEnd(' ');
    string input = "";
    for (int index = value.Length - 1; index >= 0; --index)
      input += (string) (object) value[index];
    MatchCollection matchCollection = new Regex($"[{cultureInfo.NumberFormat.NumberGroupSeparator}]").Matches(input);
    Regex regex;
    for (int i = 0; i < matchCollection.Count; ++i)
    {
      if ((matchCollection[i].Index - i) % 3 != 0)
      {
        regex = (Regex) null;
        return false;
      }
    }
    regex = (Regex) null;
    return true;
  }

  public void SetNumber(int iRow, int iColumn, double value)
  {
    int iXFIndex = this.RemoveString(iRow, iColumn);
    RKRecord rkRecord = this.TryCreateRkRecord(iRow, iColumn, value, iXFIndex);
    if (rkRecord != null)
      this.InnerSetCell(iColumn, iRow, (BiffRecordRaw) rkRecord);
    else
      this.SetNumberRecord(iRow, iColumn, value, iXFIndex);
  }

  public void SetBoolean(int iRow, int iColumn, bool value)
  {
    int iXFIndex = this.RemoveString(iRow, iColumn);
    BoolErrRecord record = (BoolErrRecord) this.GetRecord(TBIFFRecord.BoolErr, iRow, iColumn, iXFIndex);
    record.IsErrorCode = false;
    record.BoolOrError = value ? (byte) 1 : (byte) 0;
    this.InnerSetCell(iColumn, iRow, (BiffRecordRaw) record);
  }

  public void SetText(int iRow, int iColumn, string value)
  {
    if (value == null)
      throw new ArgumentOutOfRangeException("Text value cannot be null or empty");
    this.SetString(iRow, iColumn, value);
  }

  public void SetFormula(int iRow, int iColumn, string value)
  {
    this.SetFormula(iRow, iColumn, value, false);
  }

  public void SetFormula(int iRow, int iColumn, string value, bool bIsR1C1)
  {
    if (value == null || value.Length == 0 || value[0] == '=')
      throw new ArgumentOutOfRangeException("Text value cannot be null or empty. First symbol of formula cannot be '='");
    this.SetFormulaValue(iRow, iColumn, value, bIsR1C1);
  }

  public void SetError(int iRow, int iColumn, string value)
  {
    if (value == null || value.Length == 0 || value[0] != '#')
      throw new ArgumentOutOfRangeException("Text value cannot be null or empty. First symbol must be '#'");
    this.SetError(iRow, iColumn, value, false);
  }

  public void SetBlank(int iRow, int iColumn) => this.SetBlankRecord(iRow, iColumn);

  private void SetBlankRecord(int iRow, int iColumn)
  {
    int iXFIndex = this.RemoveString(iRow, iColumn);
    BiffRecordRaw record = this.GetRecord(TBIFFRecord.Blank, iRow, iColumn, iXFIndex);
    this.InnerSetCell(iColumn, iRow, record);
  }

  private void SetNumberRecord(int iRow, int iColumn, double value, int iXFIndex)
  {
    NumberRecord record = (NumberRecord) this.GetRecord(TBIFFRecord.Number, iRow, iColumn, iXFIndex);
    record.Value = value;
    this.InnerSetCell(iColumn, iRow, (BiffRecordRaw) record);
  }

  private void SetRKRecord(int iRow, int iColumn, double value)
  {
    RKRecord record = (RKRecord) this.GetRecord(TBIFFRecord.RK, iRow, iColumn);
    record.RKNumber = value;
    this.InnerSetCell(iColumn, iRow, (BiffRecordRaw) record);
  }

  private void SetFormulaValue(int iRow, int iColumn, string value, bool bIsR1C1)
  {
    int iXFIndex = this.RemoveString(iRow, iColumn);
    FormulaRecord record = (FormulaRecord) this.GetRecord(TBIFFRecord.Formula, iRow, iColumn, iXFIndex);
    record.ParsedExpression = this.m_book.FormulaUtil.ParseString(value, (IWorksheet) this, (Dictionary<string, string>) null, iRow - 1, iColumn - 1, bIsR1C1);
    this.InnerSetCell(iColumn, iRow, (BiffRecordRaw) record);
  }

  public void SetFormulaNumberValue(int iRow, int iColumn, double value)
  {
    if ((this.GetCellType(iRow, iColumn, false) & WorksheetImpl.TRangeValueType.Formula) != WorksheetImpl.TRangeValueType.Formula)
      throw new ArgumentException("Cannot sets formula value in cell that doesn't contain formula");
    this.SetFormulaValue(iRow, iColumn, value);
  }

  public void SetFormulaErrorValue(int iRow, int iColumn, string value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (!FormulaUtil.ErrorNameToCode.ContainsKey(value))
      throw new ArgumentOutOfRangeException("Value does not valid error string.");
    if ((this.GetCellType(iRow, iColumn, false) & WorksheetImpl.TRangeValueType.Formula) != WorksheetImpl.TRangeValueType.Formula)
      throw new ArgumentException("Cannot sets formula value in cell that doesn't contain formula");
    double boolErrorValue = FormulaRecord.GetBoolErrorValue((byte) FormulaUtil.ErrorNameToCode[value], true);
    this.SetFormulaValue(iRow, iColumn, boolErrorValue);
  }

  public void SetFormulaBoolValue(int iRow, int iColumn, bool value)
  {
    if ((this.GetCellType(iRow, iColumn, false) & WorksheetImpl.TRangeValueType.Formula) != WorksheetImpl.TRangeValueType.Formula)
      throw new ArgumentException("Cannot sets formula value in cell that doesn't contain formula");
    double boolErrorValue = FormulaRecord.GetBoolErrorValue(value ? (byte) 1 : (byte) 0, false);
    this.SetFormulaValue(iRow, iColumn, boolErrorValue);
  }

  public void SetFormulaStringValue(int iRow, int iColumn, string value)
  {
    if ((this.GetCellType(iRow, iColumn, false) & WorksheetImpl.TRangeValueType.Formula) != WorksheetImpl.TRangeValueType.Formula)
      throw new ArgumentException("Cannot sets formula value in cell that doesn't contain formula");
    StringRecord record = (StringRecord) this.RecordExtractor.GetRecord(519);
    record.Value = value;
    double defStringValue = FormulaRecord.DEF_STRING_VALUE;
    this.SetFormulaValue(iRow, iColumn, defStringValue, record);
  }

  public void SetError(int iRow, int iColumn, string value, bool isSetText)
  {
    int num;
    if (!FormulaUtil.ErrorNameToCode.TryGetValue(value, out num))
    {
      if (!isSetText)
        throw new ArgumentOutOfRangeException("Cannot parse error code.");
      this.SetString(iRow, iColumn, value);
    }
    else
    {
      int iXFIndex = this.RemoveString(iRow, iColumn);
      BoolErrRecord record = (BoolErrRecord) this.GetRecord(TBIFFRecord.BoolErr, iRow, iColumn, iXFIndex);
      record.IsErrorCode = true;
      record.BoolOrError = (byte) num;
      this.InnerSetCell(iColumn, iRow, (BiffRecordRaw) record);
    }
  }

  private void SetString(int iRow, int iColumn, string value)
  {
    int iXFIndex = this.RemoveString(iRow, iColumn);
    int num = this.m_book.InnerSST.AddIncrease((object) value);
    LabelSSTRecord record = (LabelSSTRecord) this.GetRecord(TBIFFRecord.LabelSST, iRow, iColumn, iXFIndex);
    record.SSTIndex = num;
    this.InnerSetCell(iColumn, iRow, (BiffRecordRaw) record);
  }

  private int RemoveString(int iRow, int iColumn)
  {
    this.ParseData();
    ICellPositionFormat cellRecord = this.m_dicRecordsCells.GetCellRecord(iRow, iColumn);
    int num = this.m_book.DefaultXFIndex;
    if (cellRecord != null)
    {
      num = (int) cellRecord.ExtendedFormatIndex;
    }
    else
    {
      RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iRow - 1, false);
      if (row != null)
        num = (int) row.ExtendedFormatIndex;
      if (num == 0 || num == this.m_book.DefaultXFIndex)
      {
        ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[iColumn];
        if (columnInfoRecord != null)
          num = (int) columnInfoRecord.ExtendedFormatIndex;
      }
    }
    if (cellRecord is LabelSSTRecord labelSstRecord)
      this.m_book.InnerSST.RemoveDecrease(labelSstRecord.SSTIndex);
    return num;
  }

  internal int GetXFIndex(int iRow, int iColumn)
  {
    this.ParseData();
    int extendedFormatIndex = this.m_dicRecordsCells.GetExtendedFormatIndex(iRow, iColumn);
    if (extendedFormatIndex < 0)
    {
      RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iRow - 1, false);
      if ((row != null ? (int) row.ExtendedFormatIndex : 0) != 0 && row.IsFormatted)
      {
        extendedFormatIndex = (int) row.ExtendedFormatIndex;
      }
      else
      {
        ColumnInfoRecord columnInfoRecord = this.m_arrColumnInfo[iColumn];
        if (columnInfoRecord != null)
          extendedFormatIndex = (int) columnInfoRecord.ExtendedFormatIndex;
      }
    }
    return extendedFormatIndex >= 0 ? extendedFormatIndex : this.m_book.DefaultXFIndex;
  }

  internal int GetXFIndex(int iRow)
  {
    this.ParseData();
    int num = this.m_dicRecordsCells.GetExtendedFormatIndexByRow(iRow);
    if (num < 0)
    {
      RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this, iRow - 1, false);
      int extendedFormatIndex = row == null || !this.m_book.IsFormatted((int) row.ExtendedFormatIndex) ? 0 : (int) row.ExtendedFormatIndex;
      if (extendedFormatIndex != 0 && extendedFormatIndex != this.m_book.DefaultXFIndex)
        num = (int) row.ExtendedFormatIndex;
    }
    return num >= 0 ? num : this.m_book.DefaultXFIndex;
  }

  internal int GetColumnXFIndex(int firstColumn)
  {
    this.ParseData();
    int num = this.m_dicRecordsCells.GetExtendedFormatIndexByColumn(firstColumn);
    if (num < 0)
    {
      ColumnInfoRecord record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
      record.FirstColumn = (ushort) (firstColumn - 1);
      int extendedFormatIndex = record == null || !this.m_book.IsFormatted((int) record.ExtendedFormatIndex) ? 0 : (int) record.ExtendedFormatIndex;
      if (extendedFormatIndex != 0 && extendedFormatIndex != this.m_book.DefaultXFIndex)
        num = (int) record.ExtendedFormatIndex;
    }
    WorksheetHelper.AccessColumn((IInternalWorksheet) this, firstColumn);
    return num >= 0 ? num : this.m_book.DefaultXFIndex;
  }

  [CLSCompliant(false)]
  protected internal RKRecord TryCreateRkRecord(int iRow, int iColumn, double value)
  {
    this.ParseData();
    int rkNumber = RKRecord.ConvertToRKNumber(value);
    if (rkNumber == int.MaxValue)
      return (RKRecord) null;
    RKRecord record = (RKRecord) this.GetRecord(TBIFFRecord.RK, iRow, iColumn);
    record.SetConvertedNumber(rkNumber);
    return record;
  }

  [CLSCompliant(false)]
  protected internal RKRecord TryCreateRkRecord(int iRow, int iColumn, double value, int iXFIndex)
  {
    this.ParseData();
    int rkNumber = RKRecord.ConvertToRKNumber(value);
    if (rkNumber == int.MaxValue)
      return (RKRecord) null;
    RKRecord record = (RKRecord) this.GetRecord(TBIFFRecord.RK, iRow, iColumn, iXFIndex);
    record.SetConvertedNumber(rkNumber);
    return record;
  }

  [CLSCompliant(false)]
  public BiffRecordRaw GetRecord(TBIFFRecord recordCode, int iRow, int iColumn)
  {
    return this.GetRecord(recordCode, iRow, iColumn, this.GetXFIndex(iRow, iColumn));
  }

  private BiffRecordRaw GetRecord(TBIFFRecord recordCode, int iRow, int iColumn, int iXFIndex)
  {
    ICellPositionFormat record = this.RecordExtractor.GetRecord((int) recordCode) as ICellPositionFormat;
    record.Row = iRow - 1;
    record.Column = iColumn - 1;
    record.ExtendedFormatIndex = (ushort) iXFIndex;
    return record as BiffRecordRaw;
  }

  private void SetFormulaValue(int iRow, int iColumn, double value)
  {
    this.SetFormulaValue(iRow, iColumn, value, (StringRecord) null);
  }

  private void SetFormulaValue(int iRow, int iColumn, double value, StringRecord strRecord)
  {
    this.ParseData();
    this.m_dicRecordsCells.Table.SetFormulaValue(iRow, iColumn, value, strRecord);
  }

  public string GetFormula(int row, int column, bool bR1C1)
  {
    return this.GetFormula(row, column, bR1C1, false);
  }

  public string GetFormula(int row, int column, bool bR1C1, bool isForSerialization)
  {
    return this.GetFormula(row, column, bR1C1, this.m_book.FormulaUtil, isForSerialization);
  }

  public string GetFormula(
    int row,
    int column,
    bool bR1C1,
    FormulaUtil formulaUtil,
    bool isForSerialization)
  {
    this.ParseData();
    Ptg[] formulaValue = this.m_dicRecordsCells.Table.GetFormulaValue(row, column);
    --row;
    --column;
    return this.GetFormula(row, column, formulaValue, bR1C1, formulaUtil, isForSerialization);
  }

  internal string GetFormula(
    int row,
    int column,
    Ptg[] arrTokens,
    bool bR1C1,
    FormulaUtil formulaUtil,
    bool isForSerialization)
  {
    return arrTokens == null ? (string) null : "=" + formulaUtil.ParsePtgArray(arrTokens, row, column, bR1C1, (NumberFormatInfo) null, false, isForSerialization, (IWorksheet) this);
  }

  private string GetFormulaArray(FormulaRecord formula)
  {
    ArrayRecord arrayRecord = this.CellRecords.GetArrayRecord(formula.Row + 1, formula.Column + 1);
    return arrayRecord == null ? (string) null : "=" + this.m_book.FormulaUtil.ParsePtgArray(arrayRecord.Formula, arrayRecord.FirstRow, arrayRecord.FirstColumn, false, (NumberFormatInfo) null, false, false, (IWorksheet) this);
  }

  public string GetStringValue(long cellIndex)
  {
    this.ParseData();
    return this.GetText(RangeImpl.GetRowFromCellIndex(cellIndex), RangeImpl.GetColumnFromCellIndex(cellIndex));
  }

  public string GetText(int row, int column)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Table.GetStringValue(row, column, this.m_book.InnerSST);
  }

  public string GetFormulaStringValue(int row, int column)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Table.GetFormulaStringValue(row, column, this.m_book.InnerSST);
  }

  public double GetNumber(int row, int column)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Table.GetNumberValue(row, column);
  }

  public double GetFormulaNumberValue(int row, int column)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Table.GetFormulaNumberValue(row, column);
  }

  public string GetError(int row, int column)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Table.GetErrorValue(row, column);
  }

  internal string GetErrorValueToString(byte value, int row)
  {
    return this.m_dicRecordsCells.Table.GetErrorValue(value, row);
  }

  public string GetFormulaErrorValue(int row, int column)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Table.GetFormulaErrorValue(row, column);
  }

  public bool GetBoolean(int row, int column)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Table.GetBoolValue(row, column) > 0;
  }

  public bool GetFormulaBoolValue(int row, int column)
  {
    this.ParseData();
    return this.m_dicRecordsCells.Table.GetFormulaBoolValue(row, column) > 0;
  }

  public bool HasArrayFormulaRecord(int row, int column)
  {
    this.ParseData();
    return this.HasArrayFormula(this.m_dicRecordsCells.Table.GetFormulaValue(row, column));
  }

  public bool HasArrayFormula(Ptg[] arrTokens)
  {
    if (arrTokens == null || arrTokens.Length != 1)
      return false;
    Ptg arrToken = arrTokens[0];
    if (arrToken.TokenCode != FormulaToken.tExp)
      return false;
    ControlPtg controlPtg = arrToken as ControlPtg;
    return this.m_dicRecordsCells.Table.HasFormulaArrayRecord(controlPtg.RowIndex, controlPtg.ColumnIndex);
  }

  public WorksheetImpl.TRangeValueType GetCellType(int row, int column, bool bNeedFormulaSubType)
  {
    this.ParseData();
    return this.m_dicRecordsCells != null && this.m_dicRecordsCells.Table != null ? this.m_dicRecordsCells.Table.GetCellType(row, column, bNeedFormulaSubType) : WorksheetImpl.TRangeValueType.Error;
  }

  public bool IsExternalFormula(int row, int column)
  {
    this.ParseData();
    Ptg[] formulaValue = this.m_dicRecordsCells.Table.GetFormulaValue(row, column);
    if (formulaValue != null)
    {
      int index = 0;
      for (int length = formulaValue.Length; index < length; ++index)
      {
        if (formulaValue[index] is ISheetReference sheetReference && this.m_book.IsExternalReference((int) sheetReference.RefIndex))
          return true;
      }
    }
    return false;
  }

  internal void OnCellValueChanged(object oldValue, object newValue, IRange range)
  {
    if (this.CellValueChanged == null)
      return;
    this.CellValueChanged((object) this, new CellValueChangedEventArgs()
    {
      OldValue = oldValue,
      NewValue = newValue,
      Range = range
    });
  }

  public int GetFirstRow() => this.Rows[0].Row;

  public int GetLastRow() => this.Rows[this.Rows.Length - 1].Row;

  public int GetRowCount() => this.Rows.Length;

  public int GetFirstColumn() => this.Columns[0].Column;

  public int GetLastColumn() => this.Columns[this.Columns.Length - 1].Column;

  public int GetColumnCount() => this.Columns.Length;

  internal ApplicationImpl GetAppImpl() => this.AppImplementation;

  internal int GetViewColumnWidthPixel(int column)
  {
    WorksheetImpl.CheckColumnIndex(column);
    double columnWidth = this.GetColumnWidth(column + 1);
    double num = this.View == SheetView.PageLayout ? 1.05 : 1.0;
    return columnWidth > 1.0 ? (int) ((double) ((int) (columnWidth * (double) this.GetAppImpl().GetFontCalc2() + 0.5) + (int) ((double) (this.GetAppImpl().GetFontCalc2() * this.GetAppImpl().GetFontCalc1()) / 256.0 + 0.5)) * num + 0.5) : (int) ((double) (int) (columnWidth * (double) (this.GetAppImpl().GetFontCalc2() + (int) ((double) (this.GetAppImpl().GetFontCalc2() * this.GetAppImpl().GetFontCalc1()) / 256.0 + 0.5)) + 0.5) * num + 0.5);
  }

  internal double CharacterWidth(double width)
  {
    ApplicationImpl appImpl = this.GetAppImpl();
    int num1 = (int) (width * (double) appImpl.GetFontCalc2() + 0.5);
    int fontCalc2 = appImpl.GetFontCalc2();
    int fontCalc1 = appImpl.GetFontCalc1();
    int fontCalc3 = appImpl.GetFontCalc3();
    if (num1 < fontCalc2 + fontCalc3)
      return 1.0 * (double) num1 / (double) (fontCalc2 + fontCalc3);
    double num2 = (double) (int) ((double) (num1 - (int) ((double) (fontCalc2 * fontCalc1) / 256.0 + 0.5)) * 100.0 / (double) fontCalc2 + 0.5) / 100.0;
    if (num2 > (double) byte.MaxValue)
      num2 = (double) byte.MaxValue;
    return num2;
  }

  internal static int CharacterWidth(double width, ApplicationImpl application)
  {
    return width > 1.0 ? (int) (width * (double) application.GetFontCalc2() + 0.5) + (int) ((double) (application.GetFontCalc2() * application.GetFontCalc1()) / 256.0 + 0.5) : (int) (width * (double) (application.GetFontCalc2() + (int) ((double) (application.GetFontCalc2() * application.GetFontCalc1()) / 256.0 + 0.5)) + 0.5);
  }

  internal static void CheckColumnIndex(int columnIndex)
  {
    if (columnIndex < 0 || columnIndex > 16383 /*0x3FFF*/)
      throw new ArgumentException("Invalid column index.");
  }

  internal static void CheckRowIndex(int rowIndex)
  {
    if (rowIndex < 0 || rowIndex > 1048575 /*0x0FFFFF*/)
      throw new ArgumentException("Invalid row index.");
  }

  internal Dictionary<long, MergedCellInfo> BuildMergedRegions()
  {
    HashSet<Rectangle> mergedRegions = this.MergeCells.MergedRegions;
    this.mergedCellPositions = new Dictionary<long, MergedCellInfo>(mergedRegions.Count);
    foreach (Rectangle rectangle in mergedRegions)
    {
      int firstRow = rectangle.Y + 1;
      int num1 = rectangle.Height + firstRow;
      int firstColumn = rectangle.X + 1;
      int num2 = rectangle.Width + firstColumn;
      long cellIndex = RangeImpl.GetCellIndex(firstColumn, firstRow);
      for (int index1 = firstRow; index1 <= num1; ++index1)
      {
        for (int index2 = firstColumn; index2 <= num2; ++index2)
        {
          long key = ((long) index1 << 32 /*0x20*/) + (long) index2;
          if (!this.mergedCellPositions.ContainsKey(key))
          {
            MergedCellInfo mergedCellInfo1 = new MergedCellInfo();
            if (index1 != firstRow || index2 != firstColumn)
            {
              MergedCellInfo mergedCellInfo2 = this.mergedCellPositions[cellIndex].Clone() with
              {
                IsFirst = false,
                FirstCellIndex = cellIndex
              };
              this.mergedCellPositions.Add(key, mergedCellInfo2);
            }
            else
            {
              mergedCellInfo1.IsFirst = true;
              mergedCellInfo1.RowSpan = num1 - firstRow + 1;
              mergedCellInfo1.ColSpan = num2 - firstColumn + 1;
              if (mergedCellInfo1.RowSpan > 1)
                mergedCellInfo1.TableSpan = TableSpan.Row;
              if (mergedCellInfo1.ColSpan > 1)
                mergedCellInfo1.TableSpan |= TableSpan.Column;
              this.mergedCellPositions.Add(key, mergedCellInfo1);
            }
          }
        }
      }
    }
    return this.mergedCellPositions;
  }

  internal bool IsValueChanged => this.ValueChanged != null;

  public void Calculate()
  {
    bool enabledCalcEngine = this.m_book.EnabledCalcEngine;
    if (!enabledCalcEngine)
      this.EnableSheetCalculations();
    IMigrantRange migrantRange = (IMigrantRange) new MigrantRangeImpl(this.Application, (IWorksheet) this);
    for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
    {
      for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
      {
        migrantRange.ResetRowColumn(firstRow, firstColumn);
        if ((migrantRange as RangeImpl).CellType == RangeImpl.TCellType.Formula)
        {
          string calculatedValue = migrantRange.CalculatedValue;
        }
      }
    }
    if (enabledCalcEngine)
      return;
    this.DisableSheetCalculations();
  }

  public void ImportHtmlTable(string fileName, int row, int column)
  {
    FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
    this.ImportHtmlTable((Stream) fileStream, row, column);
    fileStream.Dispose();
  }

  public void ImportHtmlTable(Stream fileStream, int row, int column)
  {
    HtmlToExcelConverter toExcelConverter = new HtmlToExcelConverter();
    string htmlText = (string) null;
    using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
      htmlText = streamReader.ReadToEnd();
    toExcelConverter.ParseHTMLTable(htmlText, this, row, column);
  }

  public delegate void ExportDataTableEventHandler(
    ExportDataTableEventArgs exportDataTableEventSettings);

  private enum RangeProperty
  {
    Value2,
    Text,
    DateTime,
    TimeSpan,
    Double,
    Int,
  }

  [Flags]
  public enum TRangeValueType
  {
    Blank = 0,
    Error = 1,
    Boolean = 2,
    Number = 4,
    Formula = 8,
    String = 16, // 0x00000010
  }

  private delegate IOutline OutlineDelegate(int iIndex);
}
