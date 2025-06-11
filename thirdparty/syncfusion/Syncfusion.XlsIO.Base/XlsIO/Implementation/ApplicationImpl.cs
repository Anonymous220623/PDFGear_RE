// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ApplicationImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using Syncfusion.CompoundFile.XlsIO.Native;
using Syncfusion.Compression.Zip;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Clipboard;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Security;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.TemplateMarkers;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ApplicationImpl : IApplication, IParentApplication
{
  private const double DEF_ZERO_CHAR_WIDTH = 8.0;
  private const int DEF_BUILD_NUMBER = 0;
  private const double DEF_STANDARD_FONT_SIZE = 10.0;
  private const int DEF_FIXED_DECIMAL_PLACES = 4;
  private const int DEF_SHEETS_IN_NEW_WORKBOOK = 3;
  private const string DEF_DEFAULT_FONT = "Arial";
  private const string DEF_VALUE = "Microsoft Excel";
  private const string DEF_SWITCH_NAME = "Syncfusion.XlsIO.DebugInfo";
  private const string DEF_SWITCH_DESCRIPTION = "Indicates wether to show library debug messages.";
  public const char DEF_ARGUMENT_SEPARATOR = ',';
  public const char DEF_ROW_SEPARATOR = ';';
  private const byte DEF_BIFF_HEADER_SIZE = 8;
  private const string DEF_XML_HEADER = "<?xml";
  private const string DEF_HTML_HEADER = "<html";
  private const int DEF_BUFFER_SIZE = 512 /*0x0200*/;
  internal const int DEF_COLUMN_MAX_WIDTH = 255 /*0xFF*/;
  internal const string DEF_TSV_SEPARATOR = "\t";
  internal const string DEF_CSV_SEPARATOR = ",";
  internal const int DefaultRowHeightXlsx = 300;
  private string DEF_PATH_SEPARATOR = string.Concat((object) Path.PathSeparator);
  private static readonly byte[] DEF_XLS_FILE_HEADER = new byte[8]
  {
    (byte) 208 /*0xD0*/,
    (byte) 207,
    (byte) 17,
    (byte) 224 /*0xE0*/,
    (byte) 161,
    (byte) 177,
    (byte) 26,
    (byte) 225
  };
  internal List<List<string>> CustomLists;
  private static double[] s_arrProportions;
  internal static readonly SizeF MinCellSize = new SizeF(8.43f, 12.75f);
  private readonly Graphics m_graphics;
  private static readonly BooleanSwitch m_switch = new BooleanSwitch("Syncfusion.XlsIO.DebugInfo", "Indicates wether to show library debug messages.");
  private static readonly bool m_bDebugMessage = ApplicationImpl.m_switch.Enabled;
  internal static Type[] AssemblyTypes;
  private object m_parent;
  private static bool m_bIsDebugInfoEnabled = false;
  private StringEnumerations m_stringEnum = new StringEnumerations();
  private static ExcelDataProviderType m_dataType = ExcelDataProviderType.Native;
  private static Dictionary<string, Dictionary<double, float>> m_fontsHeight;
  private bool m_isChangeSeparator;
  private bool m_useStringDelimiter;
  private char[] _numberFormatChar = new char[1]{ '�' };
  private string[] m_defaultStyleNames = new string[54]
  {
    "Normal",
    "RowLevel_",
    "ColLevel_",
    "Comma",
    "Currency",
    "Percent",
    "Comma [0]",
    "Currency [0]",
    "Hyperlink",
    "Followed Hyperlink",
    "Note",
    "Warning Text",
    "Emphasis 1",
    "Emphasis 2",
    "",
    "Title",
    "Heading 1",
    "Heading 2",
    "Heading 3",
    "Heading 4",
    "Input",
    "Output",
    "Calculation",
    "Check Cell",
    "Linked Cell",
    "Total",
    "Good",
    "Bad",
    "Neutral",
    "Accent1",
    "20% - Accent1",
    "40% - Accent1",
    "60% - Accent1",
    "Accent2",
    "20% - Accent2",
    "40% - Accent2",
    "60% - Accent2",
    "Accent3",
    "20% - Accent3",
    "40% - Accent3",
    "60% - Accent3",
    "Accent4",
    "20% - Accent4",
    "40% - Accent4",
    "60% - Accent4",
    "Accent5",
    "20% - Accent5",
    "40% - Accent5",
    "60% - Accent5",
    "Accent6",
    "20% - Accent6",
    "40% - Accent6",
    "60% - Accent6",
    "Explanatory Text"
  };
  private Dictionary<int, PageSetupBaseImpl.PaperSizeEntry> m_dicPaperSizeTable;
  private IRange m_ActiveCell;
  private WorksheetBaseImpl m_ActiveSheet;
  private ITabSheet m_ActiveTabSheet;
  private IWorkbook m_ActiveBook;
  private WorkbooksCollection m_workbooks;
  private bool m_bFixedDecimal;
  private bool m_bIgnoreSheetNameException;
  private bool m_bUseSystemSep;
  private double m_dbStandardFontSize;
  private int m_iFixedDecimalPlaces;
  private int m_iSheetsInNewWorkbook;
  private string m_strDecimalSeparator;
  private string m_strStandardFont;
  private string m_strThousandsSeparator;
  private string m_strUserName;
  private bool m_bChangeStyle;
  private SkipExtRecords m_enSkipExtRecords;
  private int m_iStandardRowHeight = (int) byte.MaxValue;
  private bool m_bStandartRowHeightFlag;
  private double m_dStandardColWidth = 8.43;
  private bool m_bOptimizeFonts;
  private bool m_bOptimizeImport;
  private char m_chRowSeparator = ';';
  private char m_chArgumentSeparator = ',';
  private string m_strCSVSeparator = ",";
  private string m_strCSVQualifier = "\"";
  private string m_strCSVRecordDelimiter = "\r\n";
  private bool m_bUseFastRecordParsing;
  private int m_iRowStorageBlock = 128 /*0x80*/;
  private bool m_bDeleteDestinationFile = true;
  private CultureInfo m_standredCulture;
  private CultureInfo m_currentCulture;
  private readonly Graphics m_autoFilterManagerGraphics;
  private ExcelVersion m_defaultVersion;
  private bool m_bNetStorage;
  private bool m_bEvalExpired;
  private bool m_bIsDefaultFontChanged;
  private Syncfusion.Compression.CompressionLevel? m_compressionLevel;
  private bool m_preserveTypes;
  private bool m_isFormulaparsed = true;
  private StyleImpl.StyleSettings[] m_builtInStyleInfo;
  private bool m_bIncrementalFormulaEnable;
  private bool m_bUpdateSheetFormulaReference = true;
  private IChartToImageConverter m_chartToImageConverter;
  private bool m_isChartCacheEnabled;
  private static bool m_isPartialTrustEnabled;
  private ExcelRangeIndexerMode m_rangeIndexerMode;
  private bool m_isExternBookParsing;
  private static object m_lock = new object();
  private bool m_skipAutoFitRow;
  private bool m_excludeAdditionalCharacters;
  internal bool m_isExplicitlySet;
  private int dpiX = 96 /*0x60*/;
  private int dpiY = 96 /*0x60*/;

  public string[] DefaultStyleNames => this.m_defaultStyleNames;

  internal StyleImpl.StyleSettings[] BuiltInStyleInfo => this.m_builtInStyleInfo;

  internal Dictionary<int, PageSetupBaseImpl.PaperSizeEntry> DicPaperSizeTable
  {
    get => this.m_dicPaperSizeTable;
  }

  public static BooleanSwitch DebugInfo => ApplicationImpl.m_switch;

  public static bool IsDebugInfoEnabled
  {
    get => ApplicationImpl.m_bIsDebugInfoEnabled;
    set => ApplicationImpl.m_bIsDebugInfoEnabled = value;
  }

  [Obsolete]
  public static bool UseUnsafeCodeStatic
  {
    get => ApplicationImpl.m_dataType == ExcelDataProviderType.Unsafe;
    set
    {
      if (value)
        ApplicationImpl.m_dataType = ExcelDataProviderType.Unsafe;
      else
        ApplicationImpl.m_dataType = ExcelDataProviderType.Native;
    }
  }

  public static ExcelDataProviderType DataProviderTypeStatic
  {
    get => ApplicationImpl.m_dataType;
    set => ApplicationImpl.m_dataType = value;
  }

  public bool IsSaved
  {
    get
    {
      IWorkbooks workbooks = this.Workbooks;
      if (workbooks != null)
      {
        int Index = 0;
        for (int count = workbooks.Count; Index < count; ++Index)
        {
          if (!workbooks[Index].Saved)
            return false;
        }
      }
      return true;
    }
  }

  public bool IsFormulaParsed
  {
    get => this.m_isFormulaparsed;
    set => this.m_isFormulaparsed = value;
  }

  public int StandardHeightInRowUnits => this.m_iStandardRowHeight;

  internal bool EvalExpired
  {
    get => this.m_bEvalExpired;
    set => this.m_bEvalExpired = value;
  }

  public bool UseStringDelimiter
  {
    get => this.m_useStringDelimiter;
    set => this.m_useStringDelimiter = value;
  }

  public IRange ActiveCell => this.m_ActiveCell;

  public IWorksheet ActiveSheet => this.m_ActiveSheet as IWorksheet;

  internal ITabSheet ActiveTabSheet => this.m_ActiveTabSheet;

  public IWorkbook ActiveWorkbook => this.m_ActiveBook;

  public IApplication Application => (IApplication) this;

  public IWorkbooks Workbooks
  {
    [DebuggerStepThrough] get => (IWorkbooks) this.m_workbooks;
  }

  public IWorksheets Worksheets
  {
    get => this.ActiveWorkbook != null ? this.ActiveWorkbook.Worksheets : (IWorksheets) null;
  }

  public object Parent
  {
    [DebuggerStepThrough] get => (object) null;
  }

  public IRange Range => (IRange) this.CreateRange((object) this);

  public bool FixedDecimal
  {
    get => this.m_bFixedDecimal;
    set => this.m_bFixedDecimal = value;
  }

  public bool IgnoreSheetNameException
  {
    get => this.m_bIgnoreSheetNameException;
    set => this.m_bIgnoreSheetNameException = value;
  }

  public bool UseSystemSeparators
  {
    get => this.m_bUseSystemSep;
    set => this.m_bUseSystemSep = value;
  }

  public double StandardFontSize
  {
    get => this.m_dbStandardFontSize;
    set
    {
      this.m_dbStandardFontSize = value;
      this.m_bIsDefaultFontChanged = true;
    }
  }

  public int Build
  {
    [DebuggerStepThrough] get => 0;
  }

  public int FixedDecimalPlaces
  {
    get => this.m_iFixedDecimalPlaces;
    set => this.m_iFixedDecimalPlaces = value;
  }

  public int SheetsInNewWorkbook
  {
    get => this.m_iSheetsInNewWorkbook;
    set
    {
      this.m_iSheetsInNewWorkbook = value >= 1 ? value : throw new ArgumentException("Sheets in workbook cannot be less then 1");
    }
  }

  public string DecimalSeparator
  {
    get => this.m_strDecimalSeparator;
    set
    {
      this.m_strDecimalSeparator = value;
      CultureInfo currentCulture = CultureInfo.CurrentCulture;
      if (this.m_currentCulture.Name == currentCulture.Name)
      {
        this.SetNumberDecimalSeparator(this.m_currentCulture.NumberFormat, this.m_strDecimalSeparator);
        this.m_isChangeSeparator = true;
      }
      else
        this.SetNumberDecimalSeparator(currentCulture.NumberFormat, this.m_strDecimalSeparator);
    }
  }

  public string DefaultFilePath
  {
    get => Environment.CurrentDirectory;
    set => Environment.CurrentDirectory = value;
  }

  public string PathSeparator => this.DEF_PATH_SEPARATOR;

  public string StandardFont
  {
    get => this.m_strStandardFont;
    set
    {
      this.m_strStandardFont = value;
      this.m_bIsDefaultFontChanged = true;
    }
  }

  public string ThousandsSeparator
  {
    get => this.m_strThousandsSeparator;
    set
    {
      this.m_strThousandsSeparator = value;
      CultureInfo currentCulture = CultureInfo.CurrentCulture;
      if (this.m_currentCulture.Name == currentCulture.Name)
      {
        this.SetThousandsSeparator(this.m_currentCulture.NumberFormat, this.m_strThousandsSeparator);
        this.m_isChangeSeparator = true;
      }
      else
        this.SetThousandsSeparator(currentCulture.NumberFormat, this.m_strThousandsSeparator);
    }
  }

  public string UserName
  {
    get => this.m_strUserName;
    set => this.m_strUserName = value;
  }

  public string Value
  {
    [DebuggerStepThrough] get => "Microsoft Excel";
  }

  public bool ChangeStyleOnCellEdit
  {
    get => this.m_bChangeStyle;
    set
    {
      if (value == this.m_bChangeStyle)
        return;
      if (this.m_workbooks.Count > 0)
        throw new ArgumentException("ChangeStyleOnCellEdit property can be changed only when Application does not contains any workbook");
      this.m_bChangeStyle = value;
    }
  }

  public SkipExtRecords SkipOnSave
  {
    get => this.m_enSkipExtRecords;
    set => this.m_enSkipExtRecords = value;
  }

  public double StandardHeight
  {
    get => (double) this.m_iStandardRowHeight / 20.0;
    set
    {
      if (value < 0.0)
        throw new ArgumentOutOfRangeException(nameof (StandardHeight));
      this.m_iStandardRowHeight = (int) (value * 20.0);
      this.m_bStandartRowHeightFlag = true;
    }
  }

  public bool StandardHeightFlag
  {
    get => this.m_bStandartRowHeightFlag;
    set => this.m_bStandartRowHeightFlag = value;
  }

  public double StandardWidth
  {
    get => this.m_dStandardColWidth;
    set
    {
      if (value < 0.0 || value > (double) byte.MaxValue)
        throw new ArgumentOutOfRangeException("Standard Column Width.");
      if (this.m_dStandardColWidth == value)
        return;
      this.m_dStandardColWidth = value;
    }
  }

  public bool OptimizeFonts
  {
    get => this.m_bOptimizeFonts;
    set => this.m_bOptimizeFonts = value;
  }

  public bool OptimizeImport
  {
    get => this.m_bOptimizeImport;
    set => this.m_bOptimizeImport = value;
  }

  public char RowSeparator
  {
    get => this.m_chRowSeparator;
    set => this.m_chRowSeparator = value;
  }

  public char ArgumentsSeparator
  {
    get => this.m_chArgumentSeparator;
    set
    {
      this.m_chArgumentSeparator = value;
      CultureInfo currentCulture = CultureInfo.CurrentCulture;
      if (this.m_currentCulture.Name == currentCulture.Name)
      {
        this.m_currentCulture.TextInfo.ListSeparator = Convert.ToString(this.m_chArgumentSeparator);
        this.m_isChangeSeparator = true;
      }
      else
        currentCulture.TextInfo.ListSeparator = Convert.ToString(this.m_chArgumentSeparator);
    }
  }

  public string CSVSeparator
  {
    get => this.m_strCSVSeparator;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (value));
        case "":
          throw new ArgumentException("value - string cannot be empty");
        default:
          this.m_strCSVSeparator = value;
          break;
      }
    }
  }

  public string CsvQualifier
  {
    get => this.m_strCSVQualifier;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (value));
        case "":
          throw new ArgumentException("value - string cannot be empty");
        default:
          if (value == this.m_strCSVSeparator || value == this.m_strCSVRecordDelimiter)
            throw new Exception("Qualifier cannot be same as Separator or Record Delimiter");
          this.m_strCSVQualifier = value;
          break;
      }
    }
  }

  public string CsvRecordDelimiter
  {
    get => this.m_strCSVRecordDelimiter;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (value));
        case "":
          throw new ArgumentException("value - string cannot be empty");
        default:
          this.m_strCSVRecordDelimiter = value;
          break;
      }
    }
  }

  [Obsolete]
  public bool UseNativeOptimization
  {
    get => ApplicationImpl.UseUnsafeCodeStatic;
    set => ApplicationImpl.UseUnsafeCodeStatic = value;
  }

  public bool UseFastRecordParsing
  {
    get => this.m_bUseFastRecordParsing;
    set => this.m_bUseFastRecordParsing = value;
  }

  public int RowStorageAllocationBlockSize
  {
    get => this.m_iRowStorageBlock;
    set
    {
      this.m_iRowStorageBlock = value > 0 ? value : throw new ArgumentOutOfRangeException("RowStorageAllocationBlock", "Property must be larger than zero.");
    }
  }

  public bool DeleteDestinationFile
  {
    get => this.m_bDeleteDestinationFile;
    set => this.m_bDeleteDestinationFile = value;
  }

  public ExcelVersion DefaultVersion
  {
    get => this.m_defaultVersion;
    set
    {
      this.m_defaultVersion = value;
      if (!this.m_bIsDefaultFontChanged)
        this.CheckDefaultFont();
      if (value == ExcelVersion.Excel97to2003)
        return;
      this.m_iStandardRowHeight = 300;
      this.m_bStandartRowHeightFlag = true;
    }
  }

  public bool UseNativeStorage
  {
    get => !this.m_bNetStorage;
    set => this.m_bNetStorage = !value;
  }

  public ExcelDataProviderType DataProviderType
  {
    get => ApplicationImpl.m_dataType;
    set => ApplicationImpl.m_dataType = value;
  }

  public Syncfusion.Compression.CompressionLevel? CompressionLevel
  {
    get => this.m_compressionLevel;
    set => this.m_compressionLevel = value;
  }

  public bool PreserveCSVDataTypes
  {
    get => this.m_preserveTypes;
    set => this.m_preserveTypes = value;
  }

  public bool EnableIncrementalFormula
  {
    get => this.m_bIncrementalFormulaEnable;
    set => this.m_bIncrementalFormulaEnable = value;
  }

  internal StringEnumerations StringEnum => this.m_stringEnum;

  public IChartToImageConverter ChartToImageConverter
  {
    get => this.m_chartToImageConverter;
    set => this.m_chartToImageConverter = value;
  }

  public bool IsChartCacheEnabled
  {
    get => this.m_isChartCacheEnabled;
    set => this.m_isChartCacheEnabled = value;
  }

  internal static bool EnablePartialTrustCodeStatic
  {
    get => ApplicationImpl.m_isPartialTrustEnabled;
    set => ApplicationImpl.m_isPartialTrustEnabled = value;
  }

  public bool EnablePartialTrustCode
  {
    get => ApplicationImpl.m_isPartialTrustEnabled;
    set => ApplicationImpl.m_isPartialTrustEnabled = value;
  }

  public ExcelRangeIndexerMode RangeIndexerMode
  {
    get => this.m_rangeIndexerMode;
    set => this.m_rangeIndexerMode = value;
  }

  public bool UpdateSheetFormulaReference
  {
    get => this.m_bUpdateSheetFormulaReference;
    set => this.m_bUpdateSheetFormulaReference = value;
  }

  internal bool IsExternBookParsing
  {
    get => this.m_isExternBookParsing;
    set => this.m_isExternBookParsing = value;
  }

  public bool SkipAutoFitRow
  {
    get => this.m_skipAutoFitRow;
    set => this.m_skipAutoFitRow = value;
  }

  internal static Dictionary<string, Dictionary<double, float>> FontsHeight
  {
    get
    {
      if (ApplicationImpl.m_fontsHeight == null)
        ApplicationImpl.InitializeFontHeight();
      return ApplicationImpl.m_fontsHeight;
    }
  }

  public bool ExcludeAdditionalCharacters
  {
    get => this.m_excludeAdditionalCharacters;
    set
    {
      this.m_isExplicitlySet = true;
      this.m_excludeAdditionalCharacters = value;
    }
  }

  static ApplicationImpl()
  {
    Bitmap bitmap = new Bitmap(1, 1);
    Graphics graphics = Graphics.FromImage((Image) bitmap);
    PointF[] pts = new PointF[1]{ new PointF(1f, 1f) };
    GraphicsContainer container = graphics.BeginContainer(new Rectangle(0, 0, 1, 1), new Rectangle(0, 0, 1, 1), GraphicsUnit.Pixel);
    graphics.PageUnit = GraphicsUnit.Inch;
    graphics.TransformPoints(CoordinateSpace.Device, CoordinateSpace.Page, pts);
    graphics.EndContainer(container);
    float x = pts[0].X;
    ApplicationImpl.s_arrProportions = new double[8]
    {
      (double) x / 75.0,
      (double) x / 300.0,
      (double) x,
      (double) x / 25.4,
      (double) x / 2.54,
      1.0,
      (double) x / 72.0,
      (double) x / 72.0 / 12700.0
    };
    graphics.Dispose();
    bitmap.Dispose();
    ApplicationImpl.MinCellSize.Height = (float) ApplicationImpl.ConvertToPixels((double) ApplicationImpl.MinCellSize.Height, MeasureUnits.Point);
    try
    {
      ApplicationImpl.AssemblyTypes = Assembly.GetExecutingAssembly().GetTypes();
    }
    catch (ReflectionTypeLoadException ex)
    {
      if (ex.Types == null)
        return;
      List<Type> typeList = new List<Type>(ex.Types.Length);
      foreach (Type type in ex.Types)
      {
        if (type != (Type) null)
          typeList.Add(type);
      }
      ApplicationImpl.AssemblyTypes = typeList.ToArray();
    }
  }

  public ApplicationImpl(ExcelEngine excelEngine)
  {
    this.m_parent = (object) excelEngine;
    if (ExcelEngine.IsSecurityGranted)
      this.m_strUserName = Environment.UserName;
    Bitmap bitmap = new Bitmap(1, 1);
    this.m_graphics = Graphics.FromImage((Image) bitmap);
    this.dpiX = (int) this.m_graphics.DpiX;
    this.dpiY = (int) this.m_graphics.DpiY;
    this.m_builtInStyleInfo = new StyleImpl.StyleSettings[this.m_defaultStyleNames.Length];
    this.m_dbStandardFontSize = 10.0;
    this.m_iFixedDecimalPlaces = 4;
    this.m_iSheetsInNewWorkbook = 3;
    this.m_standredCulture = CultureInfo.InvariantCulture;
    this.m_currentCulture = new CultureInfo(CultureInfo.CurrentCulture.Name);
    CultureInfo currentCulture = CultureInfo.CurrentCulture;
    this.m_currentCulture.NumberFormat.NumberDecimalSeparator = currentCulture.NumberFormat.NumberDecimalSeparator;
    this.m_currentCulture.NumberFormat.PercentDecimalSeparator = currentCulture.NumberFormat.PercentDecimalSeparator;
    this.m_currentCulture.NumberFormat.CurrencyDecimalSeparator = currentCulture.NumberFormat.CurrencyDecimalSeparator;
    this.m_currentCulture.NumberFormat.NumberGroupSeparator = currentCulture.NumberFormat.NumberGroupSeparator;
    this.m_currentCulture.NumberFormat.PercentGroupSeparator = currentCulture.NumberFormat.PercentGroupSeparator;
    this.m_currentCulture.NumberFormat.CurrencyGroupSeparator = currentCulture.NumberFormat.CurrencyGroupSeparator;
    this.m_currentCulture.TextInfo.ListSeparator = Convert.ToString(currentCulture.TextInfo.ListSeparator);
    this.m_strDecimalSeparator = this.m_currentCulture.NumberFormat.NumberDecimalSeparator;
    this.m_strStandardFont = "Tahoma";
    this.m_strThousandsSeparator = this.m_currentCulture.NumberFormat.NumberGroupSeparator;
    this.m_chArgumentSeparator = Convert.ToChar(this.m_currentCulture.TextInfo.ListSeparator);
    this.m_graphics.PageUnit = GraphicsUnit.Pixel;
    bitmap.Dispose();
    this.InitializeCollection();
    this.InitializeStyleCollections();
    this.InitializePageSetup();
    this.InitializeCustomListCollections();
  }

  private void InitializeCustomListCollections()
  {
    this.CustomLists = new List<List<string>>();
    this.CustomLists.Add(new List<string>((IEnumerable<string>) new string[7]
    {
      "Sun",
      "Mon",
      "Tue",
      "Wed",
      "Thu",
      "Fri",
      "Sat"
    }));
    this.CustomLists.Add(new List<string>((IEnumerable<string>) new string[7]
    {
      "Sunday",
      "Monday",
      "Tuesday",
      "Wednesday",
      "Thursday",
      "Friday",
      "Saturday"
    }));
    this.CustomLists.Add(new List<string>((IEnumerable<string>) new string[12]
    {
      "Jan",
      "Feb",
      "Mar",
      "Apr",
      "May",
      "Jun",
      "Jul",
      "Aug",
      "Sep",
      "Oct",
      "Nov",
      "Dec"
    }));
    this.CustomLists.Add(new List<string>((IEnumerable<string>) new string[12]
    {
      "January",
      "February",
      "March",
      "April",
      "May",
      "June",
      "July",
      "August",
      "September",
      "October",
      "November",
      "December"
    }));
  }

  private void InitializePageSetup()
  {
    this.m_dicPaperSizeTable = new Dictionary<int, PageSetupBaseImpl.PaperSizeEntry>();
    this.m_dicPaperSizeTable.Add(1, new PageSetupBaseImpl.PaperSizeEntry(8.5, 11.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(2, new PageSetupBaseImpl.PaperSizeEntry(8.5, 11.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(3, new PageSetupBaseImpl.PaperSizeEntry(11.0, 17.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(4, new PageSetupBaseImpl.PaperSizeEntry(17.0, 11.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(5, new PageSetupBaseImpl.PaperSizeEntry(8.5, 14.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(6, new PageSetupBaseImpl.PaperSizeEntry(5.5, 8.5, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(7, new PageSetupBaseImpl.PaperSizeEntry(7.25, 10.5, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(8, new PageSetupBaseImpl.PaperSizeEntry(297.0, 420.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(9, new PageSetupBaseImpl.PaperSizeEntry(210.0, 297.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(10, new PageSetupBaseImpl.PaperSizeEntry(210.0, 297.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(11, new PageSetupBaseImpl.PaperSizeEntry(148.0, 210.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(12, new PageSetupBaseImpl.PaperSizeEntry(257.0, 368.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(13, new PageSetupBaseImpl.PaperSizeEntry(182.0, 257.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(14, new PageSetupBaseImpl.PaperSizeEntry(8.5, 13.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(15, new PageSetupBaseImpl.PaperSizeEntry(215.0, 275.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(16 /*0x10*/, new PageSetupBaseImpl.PaperSizeEntry(10.0, 14.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(17, new PageSetupBaseImpl.PaperSizeEntry(11.0, 17.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(18, new PageSetupBaseImpl.PaperSizeEntry(8.5, 11.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(19, new PageSetupBaseImpl.PaperSizeEntry(3.875, 8.875, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(20, new PageSetupBaseImpl.PaperSizeEntry(4.125, 9.5, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(21, new PageSetupBaseImpl.PaperSizeEntry(4.5, 10.375, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(22, new PageSetupBaseImpl.PaperSizeEntry(4.75, 11.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(23, new PageSetupBaseImpl.PaperSizeEntry(5.0, 11.5, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(24, new PageSetupBaseImpl.PaperSizeEntry(17.0, 22.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(25, new PageSetupBaseImpl.PaperSizeEntry(22.0, 34.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(26, new PageSetupBaseImpl.PaperSizeEntry(34.0, 44.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(27, new PageSetupBaseImpl.PaperSizeEntry(110.0, 220.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(28, new PageSetupBaseImpl.PaperSizeEntry(162.0, 229.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(29, new PageSetupBaseImpl.PaperSizeEntry(324.0, 458.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(30, new PageSetupBaseImpl.PaperSizeEntry(229.0, 324.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(31 /*0x1F*/, new PageSetupBaseImpl.PaperSizeEntry(114.0, 162.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(32 /*0x20*/, new PageSetupBaseImpl.PaperSizeEntry(114.0, 229.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(33, new PageSetupBaseImpl.PaperSizeEntry(250.0, 353.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(34, new PageSetupBaseImpl.PaperSizeEntry(176.0, 250.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(35, new PageSetupBaseImpl.PaperSizeEntry(125.0, 176.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(36, new PageSetupBaseImpl.PaperSizeEntry(110.0, 230.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(37, new PageSetupBaseImpl.PaperSizeEntry(3.875, 7.5, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(38, new PageSetupBaseImpl.PaperSizeEntry(3.625, 6.5, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(39, new PageSetupBaseImpl.PaperSizeEntry(14.875, 11.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(40, new PageSetupBaseImpl.PaperSizeEntry(8.5, 12.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(41, new PageSetupBaseImpl.PaperSizeEntry(8.5, 13.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(42, new PageSetupBaseImpl.PaperSizeEntry(250.0, 353.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(43, new PageSetupBaseImpl.PaperSizeEntry(100.0, 148.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(44, new PageSetupBaseImpl.PaperSizeEntry(9.0, 11.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(45, new PageSetupBaseImpl.PaperSizeEntry(10.0, 11.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(46, new PageSetupBaseImpl.PaperSizeEntry(15.0, 11.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(47, new PageSetupBaseImpl.PaperSizeEntry(220.0, 220.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(50, new PageSetupBaseImpl.PaperSizeEntry(9.5, 12.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(51, new PageSetupBaseImpl.PaperSizeEntry(9.5, 15.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(52, new PageSetupBaseImpl.PaperSizeEntry(187.0 / 16.0, 18.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(53, new PageSetupBaseImpl.PaperSizeEntry(235.0, 322.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(54, new PageSetupBaseImpl.PaperSizeEntry(8.5, 11.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(55, new PageSetupBaseImpl.PaperSizeEntry(210.0, 297.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(56, new PageSetupBaseImpl.PaperSizeEntry(9.5, 12.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(57, new PageSetupBaseImpl.PaperSizeEntry(227.0, 356.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(58, new PageSetupBaseImpl.PaperSizeEntry(305.0, 487.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(59, new PageSetupBaseImpl.PaperSizeEntry(8.5, 203.0 / 16.0, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(60, new PageSetupBaseImpl.PaperSizeEntry(210.0, 330.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(61, new PageSetupBaseImpl.PaperSizeEntry(148.0, 210.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(62, new PageSetupBaseImpl.PaperSizeEntry(182.0, 257.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(63 /*0x3F*/, new PageSetupBaseImpl.PaperSizeEntry(322.0, 445.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(64 /*0x40*/, new PageSetupBaseImpl.PaperSizeEntry(174.0, 235.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(65, new PageSetupBaseImpl.PaperSizeEntry(201.0, 276.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(66, new PageSetupBaseImpl.PaperSizeEntry(420.0, 594.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(67, new PageSetupBaseImpl.PaperSizeEntry(297.0, 420.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(68, new PageSetupBaseImpl.PaperSizeEntry(322.0, 445.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(69, new PageSetupBaseImpl.PaperSizeEntry(200.0, 148.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(70, new PageSetupBaseImpl.PaperSizeEntry(105.0, 148.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(75, new PageSetupBaseImpl.PaperSizeEntry(11.0, 8.5, MeasureUnits.Inch));
    this.m_dicPaperSizeTable.Add(76, new PageSetupBaseImpl.PaperSizeEntry(420.0, 297.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(77, new PageSetupBaseImpl.PaperSizeEntry(297.0, 210.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(78, new PageSetupBaseImpl.PaperSizeEntry(210.0, 148.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(79, new PageSetupBaseImpl.PaperSizeEntry(364.0, 257.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(80 /*0x50*/, new PageSetupBaseImpl.PaperSizeEntry(257.0, 182.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(81, new PageSetupBaseImpl.PaperSizeEntry(148.0, 100.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(82, new PageSetupBaseImpl.PaperSizeEntry(148.0, 200.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(83, new PageSetupBaseImpl.PaperSizeEntry(148.0, 105.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(88, new PageSetupBaseImpl.PaperSizeEntry(128.0, 182.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(89, new PageSetupBaseImpl.PaperSizeEntry(182.0, 128.0, MeasureUnits.Millimeter));
    this.m_dicPaperSizeTable.Add(90, new PageSetupBaseImpl.PaperSizeEntry(12.0, 11.0, MeasureUnits.Inch));
  }

  private void InitializeStyleCollections()
  {
    int index1 = 0;
    this.BuiltInStyleInfo[index1] = new StyleImpl.StyleSettings((FillImpl) null, (StyleImpl.FontSettings) null);
    int index2 = index1 + 1;
    this.BuiltInStyleInfo[index2] = new StyleImpl.StyleSettings((FillImpl) null, (StyleImpl.FontSettings) null);
    int index3 = index2 + 1;
    this.BuiltInStyleInfo[index3] = new StyleImpl.StyleSettings((FillImpl) null, (StyleImpl.FontSettings) null);
    int index4 = index3 + 1;
    this.BuiltInStyleInfo[index4] = new StyleImpl.StyleSettings((FillImpl) null, (StyleImpl.FontSettings) null);
    int index5 = index4 + 1;
    this.BuiltInStyleInfo[index5] = new StyleImpl.StyleSettings((FillImpl) null, (StyleImpl.FontSettings) null);
    int index6 = index5 + 1;
    this.BuiltInStyleInfo[index6] = new StyleImpl.StyleSettings((FillImpl) null, (StyleImpl.FontSettings) null);
    int index7 = index6 + 1;
    this.BuiltInStyleInfo[index7] = new StyleImpl.StyleSettings((FillImpl) null, (StyleImpl.FontSettings) null);
    int index8 = index7 + 1;
    this.BuiltInStyleInfo[index8] = new StyleImpl.StyleSettings((FillImpl) null, (StyleImpl.FontSettings) null);
    int index9 = index8 + 1;
    this.BuiltInStyleInfo[index9] = new StyleImpl.StyleSettings((FillImpl) null, (StyleImpl.FontSettings) null);
    int index10 = index9 + 1;
    this.BuiltInStyleInfo[index10] = new StyleImpl.StyleSettings((FillImpl) null, (StyleImpl.FontSettings) null);
    int index11 = index10 + 1;
    FillImpl fill1 = new FillImpl(ExcelPattern.Solid, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 204), ColorExtension.Empty);
    StyleImpl.FontSettings font1 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 1));
    StyleImpl.BorderSettings borders1 = new StyleImpl.BorderSettings((ColorObject) Color.FromArgb((int) byte.MaxValue, 178, 178, 178), ExcelLineStyle.Thin);
    this.BuiltInStyleInfo[index11] = new StyleImpl.StyleSettings(fill1, font1, borders1);
    int index12 = index11 + 1;
    StyleImpl.FontSettings font2 = new StyleImpl.FontSettings((ColorObject) Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0, 0));
    this.BuiltInStyleInfo[index12] = new StyleImpl.StyleSettings((FillImpl) null, font2);
    int index13 = index12 + 1;
    this.BuiltInStyleInfo[index13] = new StyleImpl.StyleSettings((FillImpl) null, (StyleImpl.FontSettings) null);
    int index14 = index13 + 1;
    this.BuiltInStyleInfo[index14] = new StyleImpl.StyleSettings((FillImpl) null, (StyleImpl.FontSettings) null);
    int index15 = index14 + 1;
    this.BuiltInStyleInfo[index15] = new StyleImpl.StyleSettings((FillImpl) null, (StyleImpl.FontSettings) null);
    int index16 = index15 + 1;
    StyleImpl.FontSettings font3 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 3), 18, FontStyle.Bold, "Cambria");
    this.BuiltInStyleInfo[index16] = new StyleImpl.StyleSettings((FillImpl) null, font3);
    int index17 = index16 + 1;
    StyleImpl.FontSettings font4 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 3), 15, FontStyle.Bold);
    StyleImpl.BorderSettings borders2 = new StyleImpl.BorderSettings(new ColorObject(ColorType.Theme, 4), ExcelLineStyle.None, ExcelLineStyle.None, ExcelLineStyle.None, ExcelLineStyle.Thick);
    this.BuiltInStyleInfo[index17] = new StyleImpl.StyleSettings((FillImpl) null, font4, borders2);
    int index18 = index17 + 1;
    StyleImpl.FontSettings font5 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 3), 13, FontStyle.Bold);
    StyleImpl.BorderSettings borders3 = new StyleImpl.BorderSettings(new ColorObject(ColorType.Theme, 4, 0.499984740745262), ExcelLineStyle.None, ExcelLineStyle.None, ExcelLineStyle.None, ExcelLineStyle.Thick);
    this.BuiltInStyleInfo[index18] = new StyleImpl.StyleSettings((FillImpl) null, font5, borders3);
    int index19 = index18 + 1;
    this.BuiltInStyleInfo[index19] = new StyleImpl.StyleSettings((FillImpl) null, new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 3), FontStyle.Bold), new StyleImpl.BorderSettings(new ColorObject(ColorType.Theme, 4, 0.39997558519241921), ExcelLineStyle.None, ExcelLineStyle.None, ExcelLineStyle.None, ExcelLineStyle.Medium));
    int index20 = index19 + 1;
    StyleImpl.FontSettings font6 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 3), FontStyle.Bold);
    this.BuiltInStyleInfo[index20] = new StyleImpl.StyleSettings((FillImpl) null, font6);
    int index21 = index20 + 1;
    FillImpl fill2 = new FillImpl(ExcelPattern.Solid, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 204, 153), ColorExtension.Empty);
    StyleImpl.FontSettings font7 = new StyleImpl.FontSettings((ColorObject) Color.FromArgb((int) byte.MaxValue, 63 /*0x3F*/, 63 /*0x3F*/, 118));
    StyleImpl.BorderSettings borders4 = new StyleImpl.BorderSettings((ColorObject) Color.FromArgb((int) byte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue), ExcelLineStyle.Thin);
    this.BuiltInStyleInfo[index21] = new StyleImpl.StyleSettings(fill2, font7, borders4);
    int index22 = index21 + 1;
    FillImpl fill3 = new FillImpl(ExcelPattern.Solid, Color.FromArgb((int) byte.MaxValue, 242, 242, 242), ColorExtension.Empty);
    StyleImpl.FontSettings font8 = new StyleImpl.FontSettings((ColorObject) Color.FromArgb((int) byte.MaxValue, 63 /*0x3F*/, 63 /*0x3F*/, 63 /*0x3F*/), FontStyle.Bold);
    StyleImpl.BorderSettings borders5 = new StyleImpl.BorderSettings((ColorObject) Color.FromArgb((int) byte.MaxValue, 63 /*0x3F*/, 63 /*0x3F*/, 63 /*0x3F*/), ExcelLineStyle.Thin);
    this.BuiltInStyleInfo[index22] = new StyleImpl.StyleSettings(fill3, font8, borders5);
    int index23 = index22 + 1;
    FillImpl fill4 = new FillImpl(ExcelPattern.Solid, Color.FromArgb((int) byte.MaxValue, 242, 242, 242), ColorExtension.Empty);
    StyleImpl.FontSettings font9 = new StyleImpl.FontSettings((ColorObject) Color.FromArgb((int) byte.MaxValue, 250, 125, 0), FontStyle.Bold);
    StyleImpl.BorderSettings borders6 = new StyleImpl.BorderSettings((ColorObject) Color.FromArgb((int) byte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue), ExcelLineStyle.Thin);
    this.BuiltInStyleInfo[index23] = new StyleImpl.StyleSettings(fill4, font9, borders6);
    int index24 = index23 + 1;
    FillImpl fill5 = new FillImpl(ExcelPattern.Solid, Color.FromArgb((int) byte.MaxValue, 165, 165, 165), ColorExtension.Empty);
    StyleImpl.FontSettings font10 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 0), FontStyle.Bold);
    StyleImpl.BorderSettings borders7 = new StyleImpl.BorderSettings((ColorObject) Color.FromArgb((int) byte.MaxValue, 63 /*0x3F*/, 63 /*0x3F*/, 63 /*0x3F*/), ExcelLineStyle.Double);
    this.BuiltInStyleInfo[index24] = new StyleImpl.StyleSettings(fill5, font10, borders7);
    int index25 = index24 + 1;
    StyleImpl.FontSettings font11 = new StyleImpl.FontSettings((ColorObject) Color.FromArgb((int) byte.MaxValue, 250, 125, 0));
    StyleImpl.BorderSettings borders8 = new StyleImpl.BorderSettings((ColorObject) Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 128 /*0x80*/, 1), ExcelLineStyle.None, ExcelLineStyle.None, ExcelLineStyle.None, ExcelLineStyle.Double);
    this.BuiltInStyleInfo[index25] = new StyleImpl.StyleSettings((FillImpl) null, font11, borders8);
    int index26 = index25 + 1;
    StyleImpl.FontSettings font12 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 1), FontStyle.Bold);
    StyleImpl.BorderSettings borders9 = new StyleImpl.BorderSettings(new ColorObject(ColorType.Theme, 4), ExcelLineStyle.None, ExcelLineStyle.None, ExcelLineStyle.Thin, ExcelLineStyle.Double);
    this.BuiltInStyleInfo[index26] = new StyleImpl.StyleSettings((FillImpl) null, font12, borders9);
    int index27 = index26 + 1;
    FillImpl fill6 = new FillImpl(ExcelPattern.Solid, Color.FromArgb((int) byte.MaxValue, 198, 239, 206), ColorExtension.Empty);
    StyleImpl.FontSettings font13 = new StyleImpl.FontSettings((ColorObject) Color.FromArgb((int) byte.MaxValue, 0, 97, 0));
    this.BuiltInStyleInfo[index27] = new StyleImpl.StyleSettings(fill6, font13);
    int index28 = index27 + 1;
    FillImpl fill7 = new FillImpl(ExcelPattern.Solid, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 199, 206), ColorExtension.Empty);
    StyleImpl.FontSettings font14 = new StyleImpl.FontSettings((ColorObject) Color.FromArgb((int) byte.MaxValue, 156, 0, 6));
    this.BuiltInStyleInfo[index28] = new StyleImpl.StyleSettings(fill7, font14);
    int index29 = index28 + 1;
    FillImpl fill8 = new FillImpl(ExcelPattern.Solid, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 235, 156), ColorExtension.Empty);
    StyleImpl.FontSettings font15 = new StyleImpl.FontSettings((ColorObject) Color.FromArgb((int) byte.MaxValue, 156, 101, 0));
    this.BuiltInStyleInfo[index29] = new StyleImpl.StyleSettings(fill8, font15);
    int index30 = index29 + 1;
    FillImpl fill9 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 4), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font16 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index30] = new StyleImpl.StyleSettings(fill9, font16);
    int index31 = index30 + 1;
    FillImpl fill10 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 4, 0.79998168889431442), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font17 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index31] = new StyleImpl.StyleSettings(fill10, font17);
    int index32 = index31 + 1;
    FillImpl fill11 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 4, 0.59999389629810485), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font18 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index32] = new StyleImpl.StyleSettings(fill11, font18);
    int index33 = index32 + 1;
    FillImpl fill12 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 4, 0.39997558519241921), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font19 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index33] = new StyleImpl.StyleSettings(fill12, font19);
    int index34 = index33 + 1;
    FillImpl fill13 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 5), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font20 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index34] = new StyleImpl.StyleSettings(fill13, font20);
    int index35 = index34 + 1;
    FillImpl fill14 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 5, 0.79998168889431442), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font21 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index35] = new StyleImpl.StyleSettings(fill14, font21);
    int index36 = index35 + 1;
    FillImpl fill15 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 5, 0.59999389629810485), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font22 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index36] = new StyleImpl.StyleSettings(fill15, font22);
    int index37 = index36 + 1;
    FillImpl fill16 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 5, 0.39997558519241921), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font23 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index37] = new StyleImpl.StyleSettings(fill16, font23);
    int index38 = index37 + 1;
    FillImpl fill17 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 6), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font24 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index38] = new StyleImpl.StyleSettings(fill17, font24);
    int index39 = index38 + 1;
    FillImpl fill18 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 6, 0.79998168889431442), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font25 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index39] = new StyleImpl.StyleSettings(fill18, font25);
    int index40 = index39 + 1;
    FillImpl fill19 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 6, 0.59999389629810485), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font26 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index40] = new StyleImpl.StyleSettings(fill19, font26);
    int index41 = index40 + 1;
    FillImpl fill20 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 6, 0.39997558519241921), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font27 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index41] = new StyleImpl.StyleSettings(fill20, font27);
    int index42 = index41 + 1;
    FillImpl fill21 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 7), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font28 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index42] = new StyleImpl.StyleSettings(fill21, font28);
    int index43 = index42 + 1;
    FillImpl fill22 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 7, 0.79998168889431442), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font29 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index43] = new StyleImpl.StyleSettings(fill22, font29);
    int index44 = index43 + 1;
    FillImpl fill23 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 7, 0.59999389629810485), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font30 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index44] = new StyleImpl.StyleSettings(fill23, font30);
    int index45 = index44 + 1;
    FillImpl fill24 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 7, 0.39997558519241921), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font31 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index45] = new StyleImpl.StyleSettings(fill24, font31);
    int index46 = index45 + 1;
    FillImpl fill25 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 8), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font32 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index46] = new StyleImpl.StyleSettings(fill25, font32);
    int index47 = index46 + 1;
    FillImpl fill26 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 8, 0.79998168889431442), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font33 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index47] = new StyleImpl.StyleSettings(fill26, font33);
    int index48 = index47 + 1;
    FillImpl fill27 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 8, 0.59999389629810485), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font34 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index48] = new StyleImpl.StyleSettings(fill27, font34);
    int index49 = index48 + 1;
    FillImpl fill28 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 8, 0.39997558519241921), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font35 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index49] = new StyleImpl.StyleSettings(fill28, font35);
    int index50 = index49 + 1;
    FillImpl fill29 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 9), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font36 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index50] = new StyleImpl.StyleSettings(fill29, font36);
    int index51 = index50 + 1;
    FillImpl fill30 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 9, 0.79998168889431442), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font37 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index51] = new StyleImpl.StyleSettings(fill30, font37);
    int index52 = index51 + 1;
    FillImpl fill31 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 9, 0.59999389629810485), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font38 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index52] = new StyleImpl.StyleSettings(fill31, font38);
    int index53 = index52 + 1;
    FillImpl fill32 = new FillImpl(ExcelPattern.Solid, new ColorObject(ColorType.Theme, 9, 0.39997558519241921), (ColorObject) ColorExtension.Empty);
    StyleImpl.FontSettings font39 = new StyleImpl.FontSettings(new ColorObject(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index53] = new StyleImpl.StyleSettings(fill32, font39);
    int index54 = index53 + 1;
    StyleImpl.FontSettings font40 = new StyleImpl.FontSettings((ColorObject) Color.FromArgb((int) byte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue), FontStyle.Italic);
    this.BuiltInStyleInfo[index54] = new StyleImpl.StyleSettings((FillImpl) null, font40);
    int num = index54 + 1;
  }

  protected void InitializeCollection()
  {
    this.m_workbooks = new WorkbooksCollection(this.Application, (object) this);
  }

  private static void InitializeFontHeight()
  {
    lock (ApplicationImpl.m_lock)
    {
      ApplicationImpl.m_fontsHeight = new Dictionary<string, Dictionary<double, float>>(2);
      ApplicationImpl.m_fontsHeight.Add("Calibri", new Dictionary<double, float>(409)
      {
        {
          1.0,
          5.25f
        },
        {
          2.0,
          5.25f
        },
        {
          3.0,
          6f
        },
        {
          4.0,
          6.75f
        },
        {
          5.0,
          8.25f
        },
        {
          6.0,
          8.25f
        },
        {
          7.0,
          9f
        },
        {
          8.0,
          11.25f
        },
        {
          9.0,
          12f
        },
        {
          10.0,
          12.75f
        },
        {
          11.0,
          15f
        },
        {
          12.0,
          15.75f
        },
        {
          13.0,
          17.25f
        },
        {
          14.0,
          18.75f
        },
        {
          15.0,
          19.5f
        },
        {
          16.0,
          21f
        },
        {
          17.0,
          22.5f
        },
        {
          18.0,
          23.25f
        },
        {
          19.0,
          24.75f
        },
        {
          20.0,
          26.25f
        },
        {
          21.0,
          27.75f
        },
        {
          22.0,
          28.5f
        },
        {
          23.0,
          30f
        },
        {
          24.0,
          31.5f
        },
        {
          25.0,
          32.25f
        },
        {
          26.0,
          33.75f
        },
        {
          27.0,
          35.25f
        },
        {
          28.0,
          36f
        },
        {
          29.0,
          37.5f
        },
        {
          30.0,
          39f
        },
        {
          31.0,
          39.75f
        },
        {
          32.0,
          42f
        },
        {
          33.0,
          42.75f
        },
        {
          34.0,
          43.5f
        },
        {
          35.0,
          45.75f
        },
        {
          36.0,
          46.5f
        },
        {
          37.0,
          47.25f
        },
        {
          38.0,
          49.5f
        },
        {
          39.0,
          50.25f
        },
        {
          40.0,
          51f
        },
        {
          41.0,
          53.25f
        },
        {
          42.0,
          54f
        },
        {
          43.0,
          54.75f
        },
        {
          44.0,
          57f
        },
        {
          45.0,
          57.75f
        },
        {
          46.0,
          58.5f
        },
        {
          47.0,
          60.75f
        },
        {
          48.0,
          61.5f
        },
        {
          49.0,
          62.25f
        },
        {
          50.0,
          64.5f
        },
        {
          51.0,
          65.25f
        },
        {
          52.0,
          66.75f
        },
        {
          53.0,
          68.25f
        },
        {
          54.0,
          69f
        },
        {
          55.0,
          70.5f
        },
        {
          56.0,
          72f
        },
        {
          57.0,
          72.75f
        },
        {
          58.0,
          74.25f
        },
        {
          59.0,
          75.75f
        },
        {
          60.0,
          76.5f
        },
        {
          61.0,
          78f
        },
        {
          62.0,
          79.5f
        },
        {
          63.0,
          81f
        },
        {
          64.0,
          81.75f
        },
        {
          65.0,
          83.25f
        },
        {
          66.0,
          84.75f
        },
        {
          67.0,
          85.5f
        },
        {
          68.0,
          87f
        },
        {
          69.0,
          88.5f
        },
        {
          70.0,
          89.25f
        },
        {
          71.0,
          91.5f
        },
        {
          72.0,
          92.25f
        },
        {
          73.0,
          93f
        },
        {
          74.0,
          95.25f
        },
        {
          75.0,
          96f
        },
        {
          76.0,
          96.75f
        },
        {
          77.0,
          99f
        },
        {
          78.0,
          99.75f
        },
        {
          79.0,
          100.5f
        },
        {
          80.0,
          102.75f
        },
        {
          81.0,
          103.5f
        },
        {
          82.0,
          104.25f
        },
        {
          83.0,
          106.5f
        },
        {
          84.0,
          107.25f
        },
        {
          85.0,
          108f
        },
        {
          86.0,
          110.25f
        },
        {
          87.0,
          111f
        },
        {
          88.0,
          111.75f
        },
        {
          89.0,
          114f
        },
        {
          90.0,
          114.75f
        },
        {
          91.0,
          115.5f
        },
        {
          92.0,
          117.75f
        },
        {
          93.0,
          118.5f
        },
        {
          94.0,
          120f
        },
        {
          95.0,
          121.5f
        },
        {
          96.0,
          122.25f
        },
        {
          97.0,
          123.75f
        },
        {
          98.0,
          125.25f
        },
        {
          99.0,
          126f
        },
        {
          100.0,
          127.5f
        },
        {
          101.0,
          129f
        },
        {
          102.0,
          130.5f
        },
        {
          103.0,
          131.25f
        },
        {
          104.0,
          132.75f
        },
        {
          105.0,
          134.25f
        },
        {
          106.0,
          135f
        },
        {
          107.0,
          136.5f
        },
        {
          108.0,
          138f
        },
        {
          109.0,
          138.75f
        },
        {
          110.0,
          140.25f
        },
        {
          111.0,
          141.75f
        },
        {
          112.0,
          142.5f
        },
        {
          113.0,
          144.75f
        },
        {
          114.0,
          145.5f
        },
        {
          115.0,
          146.25f
        },
        {
          116.0,
          148.5f
        },
        {
          117.0,
          149.25f
        },
        {
          118.0,
          150f
        },
        {
          119.0,
          152.25f
        },
        {
          120.0,
          153f
        },
        {
          121.0,
          153.75f
        },
        {
          122.0,
          156f
        },
        {
          123.0,
          156.75f
        },
        {
          124.0,
          157.5f
        },
        {
          125.0,
          159.75f
        },
        {
          126.0,
          160.5f
        },
        {
          (double) sbyte.MaxValue,
          161.25f
        },
        {
          128.0,
          163.5f
        },
        {
          129.0,
          164.25f
        },
        {
          130.0,
          165f
        },
        {
          131.0,
          167.25f
        },
        {
          132.0,
          168f
        },
        {
          133.0,
          169.5f
        },
        {
          134.0,
          171f
        },
        {
          135.0,
          171.75f
        },
        {
          136.0,
          173.25f
        },
        {
          137.0,
          174.75f
        },
        {
          138.0,
          175.5f
        },
        {
          139.0,
          177f
        },
        {
          140.0,
          178.5f
        },
        {
          141.0,
          179.25f
        },
        {
          142.0,
          180.75f
        },
        {
          143.0,
          182.25f
        },
        {
          144.0,
          183.75f
        },
        {
          145.0,
          184.5f
        },
        {
          146.0,
          186f
        },
        {
          147.0,
          187.5f
        },
        {
          148.0,
          188.25f
        },
        {
          149.0,
          189.75f
        },
        {
          150.0,
          191.25f
        },
        {
          151.0,
          192f
        },
        {
          152.0,
          194.25f
        },
        {
          153.0,
          195f
        },
        {
          154.0,
          195.75f
        },
        {
          155.0,
          198f
        },
        {
          156.0,
          198.75f
        },
        {
          157.0,
          199.5f
        },
        {
          158.0,
          201.75f
        },
        {
          159.0,
          202.5f
        },
        {
          160.0,
          203.25f
        },
        {
          161.0,
          205.5f
        },
        {
          162.0,
          206.25f
        },
        {
          163.0,
          207f
        },
        {
          164.0,
          209.25f
        },
        {
          165.0,
          210f
        },
        {
          166.0,
          210.75f
        },
        {
          167.0,
          213f
        },
        {
          168.0,
          213.75f
        },
        {
          169.0,
          214.5f
        },
        {
          170.0,
          216.75f
        },
        {
          171.0,
          217.5f
        },
        {
          172.0,
          218.25f
        },
        {
          173.0,
          220.5f
        },
        {
          174.0,
          221.25f
        },
        {
          175.0,
          222.75f
        },
        {
          176.0,
          224.25f
        },
        {
          177.0,
          225f
        },
        {
          178.0,
          226.5f
        },
        {
          179.0,
          228f
        },
        {
          180.0,
          228.75f
        },
        {
          181.0,
          230.25f
        },
        {
          182.0,
          231.75f
        },
        {
          183.0,
          233.25f
        },
        {
          184.0,
          234f
        },
        {
          185.0,
          235.5f
        },
        {
          186.0,
          237f
        },
        {
          187.0,
          237.75f
        },
        {
          188.0,
          239.25f
        },
        {
          189.0,
          240.75f
        },
        {
          190.0,
          241.5f
        },
        {
          191.0,
          243f
        },
        {
          192.0,
          244.5f
        },
        {
          193.0,
          245.25f
        },
        {
          194.0,
          247.5f
        },
        {
          195.0,
          248.25f
        },
        {
          196.0,
          249f
        },
        {
          197.0,
          251.25f
        },
        {
          198.0,
          252f
        },
        {
          199.0,
          252.75f
        },
        {
          200.0,
          (float) byte.MaxValue
        },
        {
          201.0,
          255.75f
        },
        {
          202.0,
          256.5f
        },
        {
          203.0,
          258.75f
        },
        {
          204.0,
          259.5f
        },
        {
          205.0,
          260.25f
        },
        {
          206.0,
          262.5f
        },
        {
          207.0,
          263.25f
        },
        {
          208.0,
          264f
        },
        {
          209.0,
          266.25f
        },
        {
          210.0,
          267f
        },
        {
          211.0,
          267.75f
        },
        {
          212.0,
          270f
        },
        {
          213.0,
          270.75f
        },
        {
          214.0,
          272.25f
        },
        {
          215.0,
          273.75f
        },
        {
          216.0,
          274.5f
        },
        {
          217.0,
          276f
        },
        {
          218.0,
          277.5f
        },
        {
          219.0,
          278.25f
        },
        {
          220.0,
          279.75f
        },
        {
          221.0,
          281.25f
        },
        {
          222.0,
          282f
        },
        {
          223.0,
          283.5f
        },
        {
          224.0,
          285f
        },
        {
          225.0,
          286.5f
        },
        {
          226.0,
          287.25f
        },
        {
          227.0,
          288.75f
        },
        {
          228.0,
          290.25f
        },
        {
          229.0,
          291f
        },
        {
          230.0,
          292.5f
        },
        {
          231.0,
          294f
        },
        {
          232.0,
          294.75f
        },
        {
          233.0,
          297f
        },
        {
          234.0,
          297.75f
        },
        {
          235.0,
          298.5f
        },
        {
          236.0,
          300.75f
        },
        {
          237.0,
          301.5f
        },
        {
          238.0,
          302.25f
        },
        {
          239.0,
          304.5f
        },
        {
          240.0,
          305.25f
        },
        {
          241.0,
          306f
        },
        {
          242.0,
          308.25f
        },
        {
          243.0,
          309f
        },
        {
          244.0,
          309.75f
        },
        {
          245.0,
          312f
        },
        {
          246.0,
          312.75f
        },
        {
          247.0,
          313.5f
        },
        {
          248.0,
          315.75f
        },
        {
          249.0,
          316.5f
        },
        {
          250.0,
          317.25f
        },
        {
          251.0,
          319.5f
        },
        {
          252.0,
          320.25f
        },
        {
          253.0,
          321.75f
        },
        {
          254.0,
          323.25f
        },
        {
          (double) byte.MaxValue,
          324f
        },
        {
          256.0,
          325.5f
        },
        {
          257.0,
          327f
        },
        {
          258.0,
          327.75f
        },
        {
          259.0,
          329.25f
        },
        {
          260.0,
          330.75f
        },
        {
          261.0,
          331.5f
        },
        {
          262.0,
          333f
        },
        {
          263.0,
          334.5f
        },
        {
          264.0,
          336f
        },
        {
          265.0,
          336.75f
        },
        {
          266.0,
          338.25f
        },
        {
          267.0,
          339.75f
        },
        {
          268.0,
          340.5f
        },
        {
          269.0,
          342f
        },
        {
          270.0,
          343.5f
        },
        {
          271.0,
          344.25f
        },
        {
          272.0,
          345.75f
        },
        {
          273.0,
          347.25f
        },
        {
          274.0,
          348f
        },
        {
          275.0,
          350.25f
        },
        {
          276.0,
          351f
        },
        {
          277.0,
          351.75f
        },
        {
          278.0,
          354f
        },
        {
          279.0,
          354.75f
        },
        {
          280.0,
          355.5f
        },
        {
          281.0,
          357.75f
        },
        {
          282.0,
          358.5f
        },
        {
          283.0,
          359.25f
        },
        {
          284.0,
          361.5f
        },
        {
          285.0,
          362.25f
        },
        {
          286.0,
          363f
        },
        {
          287.0,
          365.25f
        },
        {
          288.0,
          366f
        },
        {
          289.0,
          366.75f
        },
        {
          290.0,
          369f
        },
        {
          291.0,
          369.75f
        },
        {
          292.0,
          370.5f
        },
        {
          293.0,
          372.75f
        },
        {
          294.0,
          373.5f
        },
        {
          295.0,
          375f
        },
        {
          296.0,
          376.5f
        },
        {
          297.0,
          377.25f
        },
        {
          298.0,
          378.75f
        },
        {
          299.0,
          380.25f
        },
        {
          300.0,
          381f
        },
        {
          301.0,
          382.5f
        },
        {
          302.0,
          384f
        },
        {
          303.0,
          384.75f
        },
        {
          304.0,
          386.25f
        },
        {
          305.0,
          387.75f
        },
        {
          306.0,
          389.25f
        },
        {
          307.0,
          390f
        },
        {
          308.0,
          391.5f
        },
        {
          309.0,
          393f
        },
        {
          310.0,
          393.75f
        },
        {
          311.0,
          395.25f
        },
        {
          312.0,
          396.75f
        },
        {
          313.0,
          397.5f
        },
        {
          314.0,
          399.75f
        },
        {
          315.0,
          400.5f
        },
        {
          316.0,
          401.25f
        },
        {
          317.0,
          403.5f
        },
        {
          318.0,
          404.25f
        },
        {
          319.0,
          405f
        },
        {
          320.0,
          407.25f
        },
        {
          321.0,
          408f
        },
        {
          322.0,
          408.75f
        },
        {
          323.0,
          409.5f
        },
        {
          324.0,
          409.5f
        },
        {
          325.0,
          409.5f
        },
        {
          326.0,
          409.5f
        },
        {
          327.0,
          409.5f
        },
        {
          328.0,
          409.5f
        },
        {
          329.0,
          409.5f
        },
        {
          330.0,
          409.5f
        },
        {
          331.0,
          409.5f
        },
        {
          332.0,
          409.5f
        },
        {
          333.0,
          409.5f
        },
        {
          334.0,
          409.5f
        },
        {
          335.0,
          409.5f
        },
        {
          336.0,
          409.5f
        },
        {
          337.0,
          409.5f
        },
        {
          338.0,
          409.5f
        },
        {
          339.0,
          409.5f
        },
        {
          340.0,
          409.5f
        },
        {
          341.0,
          409.5f
        },
        {
          342.0,
          409.5f
        },
        {
          343.0,
          409.5f
        },
        {
          344.0,
          409.5f
        },
        {
          345.0,
          409.5f
        },
        {
          346.0,
          409.5f
        },
        {
          347.0,
          409.5f
        },
        {
          348.0,
          409.5f
        },
        {
          349.0,
          409.5f
        },
        {
          350.0,
          409.5f
        },
        {
          351.0,
          409.5f
        },
        {
          352.0,
          409.5f
        },
        {
          353.0,
          409.5f
        },
        {
          354.0,
          409.5f
        },
        {
          355.0,
          409.5f
        },
        {
          356.0,
          409.5f
        },
        {
          357.0,
          409.5f
        },
        {
          358.0,
          409.5f
        },
        {
          359.0,
          409.5f
        },
        {
          360.0,
          409.5f
        },
        {
          361.0,
          409.5f
        },
        {
          362.0,
          409.5f
        },
        {
          363.0,
          409.5f
        },
        {
          364.0,
          409.5f
        },
        {
          365.0,
          409.5f
        },
        {
          366.0,
          409.5f
        },
        {
          367.0,
          409.5f
        },
        {
          368.0,
          409.5f
        },
        {
          369.0,
          409.5f
        },
        {
          370.0,
          409.5f
        },
        {
          371.0,
          409.5f
        },
        {
          372.0,
          409.5f
        },
        {
          373.0,
          409.5f
        },
        {
          374.0,
          409.5f
        },
        {
          375.0,
          409.5f
        },
        {
          376.0,
          409.5f
        },
        {
          377.0,
          409.5f
        },
        {
          378.0,
          409.5f
        },
        {
          379.0,
          409.5f
        },
        {
          380.0,
          409.5f
        },
        {
          381.0,
          409.5f
        },
        {
          382.0,
          409.5f
        },
        {
          383.0,
          409.5f
        },
        {
          384.0,
          409.5f
        },
        {
          385.0,
          409.5f
        },
        {
          386.0,
          409.5f
        },
        {
          387.0,
          409.5f
        },
        {
          388.0,
          409.5f
        },
        {
          389.0,
          409.5f
        },
        {
          390.0,
          409.5f
        },
        {
          391.0,
          409.5f
        },
        {
          392.0,
          409.5f
        },
        {
          393.0,
          409.5f
        },
        {
          394.0,
          409.5f
        },
        {
          395.0,
          409.5f
        },
        {
          396.0,
          409.5f
        },
        {
          397.0,
          409.5f
        },
        {
          398.0,
          409.5f
        },
        {
          399.0,
          409.5f
        },
        {
          400.0,
          409.5f
        },
        {
          401.0,
          409.5f
        },
        {
          402.0,
          409.5f
        },
        {
          403.0,
          409.5f
        },
        {
          404.0,
          409.5f
        },
        {
          405.0,
          409.5f
        },
        {
          406.0,
          409.5f
        },
        {
          407.0,
          409.5f
        },
        {
          408.0,
          409.5f
        },
        {
          409.0,
          409.5f
        }
      });
      ApplicationImpl.m_fontsHeight.Add("Tahoma", new Dictionary<double, float>(409)
      {
        {
          1.0,
          5.25f
        },
        {
          2.0,
          5.25f
        },
        {
          3.0,
          6f
        },
        {
          4.0,
          6.75f
        },
        {
          5.0,
          8.25f
        },
        {
          6.0,
          8.25f
        },
        {
          7.0,
          9f
        },
        {
          8.0,
          10.5f
        },
        {
          9.0,
          11.25f
        },
        {
          10.0,
          12.75f
        },
        {
          11.0,
          14.25f
        },
        {
          12.0,
          15f
        },
        {
          13.0,
          16.5f
        },
        {
          14.0,
          18f
        },
        {
          15.0,
          18.75f
        },
        {
          16.0,
          19.5f
        },
        {
          17.0,
          21.75f
        },
        {
          18.0,
          22.5f
        },
        {
          19.0,
          23.25f
        },
        {
          20.0,
          25.5f
        },
        {
          21.0,
          26.25f
        },
        {
          22.0,
          27f
        },
        {
          23.0,
          28.5f
        },
        {
          24.0,
          30f
        },
        {
          25.0,
          30.75f
        },
        {
          26.0,
          32.25f
        },
        {
          27.0,
          33f
        },
        {
          28.0,
          34.5f
        },
        {
          29.0,
          36f
        },
        {
          30.0,
          36.75f
        },
        {
          31.0,
          37.5f
        },
        {
          32.0,
          39.75f
        },
        {
          33.0,
          40.5f
        },
        {
          34.0,
          41.25f
        },
        {
          35.0,
          43.5f
        },
        {
          36.0,
          44.25f
        },
        {
          37.0,
          45f
        },
        {
          38.0,
          47.25f
        },
        {
          39.0,
          48f
        },
        {
          40.0,
          48.75f
        },
        {
          41.0,
          50.25f
        },
        {
          42.0,
          51.75f
        },
        {
          43.0,
          52.5f
        },
        {
          44.0,
          54f
        },
        {
          45.0,
          54.75f
        },
        {
          46.0,
          56.25f
        },
        {
          47.0,
          57.75f
        },
        {
          48.0,
          58.5f
        },
        {
          49.0,
          59.25f
        },
        {
          50.0,
          61.5f
        },
        {
          51.0,
          62.25f
        },
        {
          52.0,
          63f
        },
        {
          53.0,
          65.25f
        },
        {
          54.0,
          66f
        },
        {
          55.0,
          66.75f
        },
        {
          56.0,
          68.25f
        },
        {
          57.0,
          69.75f
        },
        {
          58.0,
          70.5f
        },
        {
          59.0,
          72f
        },
        {
          60.0,
          73.5f
        },
        {
          61.0,
          74.25f
        },
        {
          62.0,
          75.75f
        },
        {
          63.0,
          76.5f
        },
        {
          64.0,
          78f
        },
        {
          65.0,
          79.5f
        },
        {
          66.0,
          80.25f
        },
        {
          67.0,
          81f
        },
        {
          68.0,
          83.25f
        },
        {
          69.0,
          84f
        },
        {
          70.0,
          84.75f
        },
        {
          71.0,
          87f
        },
        {
          72.0,
          87.75f
        },
        {
          73.0,
          88.5f
        },
        {
          74.0,
          90f
        },
        {
          75.0,
          91.5f
        },
        {
          76.0,
          92.25f
        },
        {
          77.0,
          93.75f
        },
        {
          78.0,
          94.5f
        },
        {
          79.0,
          96f
        },
        {
          80.0,
          97.5f
        },
        {
          81.0,
          98.25f
        },
        {
          82.0,
          99.75f
        },
        {
          83.0,
          101.25f
        },
        {
          84.0,
          102f
        },
        {
          85.0,
          102.75f
        },
        {
          86.0,
          105f
        },
        {
          87.0,
          105.75f
        },
        {
          88.0,
          106.5f
        },
        {
          89.0,
          108.75f
        },
        {
          90.0,
          109.5f
        },
        {
          91.0,
          110.25f
        },
        {
          92.0,
          111.75f
        },
        {
          93.0,
          113.25f
        },
        {
          94.0,
          114f
        },
        {
          95.0,
          115.5f
        },
        {
          96.0,
          116.25f
        },
        {
          97.0,
          117.75f
        },
        {
          98.0,
          119.25f
        },
        {
          99.0,
          120f
        },
        {
          100.0,
          120.75f
        },
        {
          101.0,
          123f
        },
        {
          102.0,
          123.75f
        },
        {
          103.0,
          124.5f
        },
        {
          104.0,
          126.75f
        },
        {
          105.0,
          127.5f
        },
        {
          106.0,
          128.25f
        },
        {
          107.0,
          130.5f
        },
        {
          108.0,
          131.25f
        },
        {
          109.0,
          132f
        },
        {
          110.0,
          133.5f
        },
        {
          111.0,
          135f
        },
        {
          112.0,
          135.75f
        },
        {
          113.0,
          137.25f
        },
        {
          114.0,
          138f
        },
        {
          115.0,
          139.5f
        },
        {
          116.0,
          141f
        },
        {
          117.0,
          141.75f
        },
        {
          118.0,
          142.5f
        },
        {
          119.0,
          144.75f
        },
        {
          120.0,
          145.5f
        },
        {
          121.0,
          146.25f
        },
        {
          122.0,
          148.5f
        },
        {
          123.0,
          149.25f
        },
        {
          124.0,
          150f
        },
        {
          125.0,
          151.5f
        },
        {
          126.0,
          153f
        },
        {
          (double) sbyte.MaxValue,
          153.75f
        },
        {
          128.0,
          155.25f
        },
        {
          129.0,
          156.75f
        },
        {
          130.0,
          157.5f
        },
        {
          131.0,
          159f
        },
        {
          132.0,
          159.75f
        },
        {
          133.0,
          161.25f
        },
        {
          134.0,
          162.75f
        },
        {
          135.0,
          163.5f
        },
        {
          136.0,
          164.25f
        },
        {
          137.0,
          166.5f
        },
        {
          138.0,
          167.25f
        },
        {
          139.0,
          168f
        },
        {
          140.0,
          170.25f
        },
        {
          141.0,
          171f
        },
        {
          142.0,
          171.75f
        },
        {
          143.0,
          173.25f
        },
        {
          144.0,
          174.75f
        },
        {
          145.0,
          175.5f
        },
        {
          146.0,
          177f
        },
        {
          147.0,
          177.75f
        },
        {
          148.0,
          179.25f
        },
        {
          149.0,
          180.75f
        },
        {
          150.0,
          181.5f
        },
        {
          151.0,
          183f
        },
        {
          152.0,
          184.5f
        },
        {
          153.0,
          185.25f
        },
        {
          154.0,
          186f
        },
        {
          155.0,
          188.25f
        },
        {
          156.0,
          189f
        },
        {
          157.0,
          189.75f
        },
        {
          158.0,
          192f
        },
        {
          159.0,
          192.75f
        },
        {
          160.0,
          193.5f
        },
        {
          161.0,
          195f
        },
        {
          162.0,
          196.5f
        },
        {
          163.0,
          197.25f
        },
        {
          164.0,
          198.75f
        },
        {
          165.0,
          199.5f
        },
        {
          166.0,
          201f
        },
        {
          167.0,
          202.5f
        },
        {
          168.0,
          203.25f
        },
        {
          169.0,
          204f
        },
        {
          170.0,
          206.25f
        },
        {
          171.0,
          207f
        },
        {
          172.0,
          207.75f
        },
        {
          173.0,
          210f
        },
        {
          174.0,
          210.75f
        },
        {
          175.0,
          211.5f
        },
        {
          176.0,
          213.75f
        },
        {
          177.0,
          214.5f
        },
        {
          178.0,
          215.25f
        },
        {
          179.0,
          216.75f
        },
        {
          180.0,
          218.25f
        },
        {
          181.0,
          219f
        },
        {
          182.0,
          220.5f
        },
        {
          183.0,
          221.25f
        },
        {
          184.0,
          222.75f
        },
        {
          185.0,
          224.25f
        },
        {
          186.0,
          225f
        },
        {
          187.0,
          225.75f
        },
        {
          188.0,
          228f
        },
        {
          189.0,
          228.75f
        },
        {
          190.0,
          229.5f
        },
        {
          191.0,
          231.75f
        },
        {
          192.0,
          232.5f
        },
        {
          193.0,
          233.25f
        },
        {
          194.0,
          234.75f
        },
        {
          195.0,
          236.25f
        },
        {
          196.0,
          237f
        },
        {
          197.0,
          238.5f
        },
        {
          198.0,
          240f
        },
        {
          199.0,
          240.75f
        },
        {
          200.0,
          242.25f
        },
        {
          201.0,
          243f
        },
        {
          202.0,
          244.5f
        },
        {
          203.0,
          246f
        },
        {
          204.0,
          246.75f
        },
        {
          205.0,
          247.5f
        },
        {
          206.0,
          249.75f
        },
        {
          207.0,
          250.5f
        },
        {
          208.0,
          251.25f
        },
        {
          209.0,
          253.5f
        },
        {
          210.0,
          254.25f
        },
        {
          211.0,
          (float) byte.MaxValue
        },
        {
          212.0,
          256.5f
        },
        {
          213.0,
          258f
        },
        {
          214.0,
          258.75f
        },
        {
          215.0,
          260.25f
        },
        {
          216.0,
          261f
        },
        {
          217.0,
          262.5f
        },
        {
          218.0,
          264f
        },
        {
          219.0,
          264.75f
        },
        {
          220.0,
          266.25f
        },
        {
          221.0,
          267.75f
        },
        {
          222.0,
          268.5f
        },
        {
          223.0,
          269.25f
        },
        {
          224.0,
          271.5f
        },
        {
          225.0,
          272.25f
        },
        {
          226.0,
          273f
        },
        {
          227.0,
          275.25f
        },
        {
          228.0,
          276f
        },
        {
          229.0,
          276.75f
        },
        {
          230.0,
          278.25f
        },
        {
          231.0,
          279.75f
        },
        {
          232.0,
          280.5f
        },
        {
          233.0,
          282f
        },
        {
          234.0,
          282.75f
        },
        {
          235.0,
          284.25f
        },
        {
          236.0,
          285.75f
        },
        {
          237.0,
          286.5f
        },
        {
          238.0,
          287.25f
        },
        {
          239.0,
          289.5f
        },
        {
          240.0,
          290.25f
        },
        {
          241.0,
          291f
        },
        {
          242.0,
          293.25f
        },
        {
          243.0,
          294f
        },
        {
          244.0,
          294.75f
        },
        {
          245.0,
          297f
        },
        {
          246.0,
          297.75f
        },
        {
          247.0,
          298.5f
        },
        {
          248.0,
          300f
        },
        {
          249.0,
          301.5f
        },
        {
          250.0,
          302.25f
        },
        {
          251.0,
          303.75f
        },
        {
          252.0,
          304.5f
        },
        {
          253.0,
          306f
        },
        {
          254.0,
          307.5f
        },
        {
          (double) byte.MaxValue,
          308.25f
        },
        {
          256.0,
          309f
        },
        {
          257.0,
          311.25f
        },
        {
          258.0,
          312f
        },
        {
          259.0,
          312.75f
        },
        {
          260.0,
          315f
        },
        {
          261.0,
          315.75f
        },
        {
          262.0,
          316.5f
        },
        {
          263.0,
          318f
        },
        {
          264.0,
          319.5f
        },
        {
          265.0,
          320.25f
        },
        {
          266.0,
          321.75f
        },
        {
          267.0,
          323.25f
        },
        {
          268.0,
          324f
        },
        {
          269.0,
          325.5f
        },
        {
          270.0,
          326.25f
        },
        {
          271.0,
          327.75f
        },
        {
          272.0,
          329.25f
        },
        {
          273.0,
          330f
        },
        {
          274.0,
          330.75f
        },
        {
          275.0,
          333f
        },
        {
          276.0,
          333.75f
        },
        {
          277.0,
          334.5f
        },
        {
          278.0,
          336.75f
        },
        {
          279.0,
          337.5f
        },
        {
          280.0,
          338.25f
        },
        {
          281.0,
          339.75f
        },
        {
          282.0,
          341.25f
        },
        {
          283.0,
          342f
        },
        {
          284.0,
          343.5f
        },
        {
          285.0,
          344.25f
        },
        {
          286.0,
          345.75f
        },
        {
          287.0,
          347.25f
        },
        {
          288.0,
          348f
        },
        {
          289.0,
          349.5f
        },
        {
          290.0,
          351f
        },
        {
          291.0,
          351.75f
        },
        {
          292.0,
          352.5f
        },
        {
          293.0,
          354.75f
        },
        {
          294.0,
          355.5f
        },
        {
          295.0,
          356.25f
        },
        {
          296.0,
          358.5f
        },
        {
          297.0,
          359.25f
        },
        {
          298.0,
          360f
        },
        {
          299.0,
          361.5f
        },
        {
          300.0,
          363f
        },
        {
          301.0,
          363.75f
        },
        {
          302.0,
          365.25f
        },
        {
          303.0,
          366f
        },
        {
          304.0,
          367.5f
        },
        {
          305.0,
          369f
        },
        {
          306.0,
          369.75f
        },
        {
          307.0,
          370.5f
        },
        {
          308.0,
          372.75f
        },
        {
          309.0,
          373.5f
        },
        {
          310.0,
          374.25f
        },
        {
          311.0,
          376.5f
        },
        {
          312.0,
          377.25f
        },
        {
          313.0,
          378f
        },
        {
          314.0,
          380.25f
        },
        {
          315.0,
          381f
        },
        {
          316.0,
          381.75f
        },
        {
          317.0,
          383.25f
        },
        {
          318.0,
          384.75f
        },
        {
          319.0,
          385.5f
        },
        {
          320.0,
          387f
        },
        {
          321.0,
          387.75f
        },
        {
          322.0,
          389.25f
        },
        {
          323.0,
          390.75f
        },
        {
          324.0,
          391.5f
        },
        {
          325.0,
          392.25f
        },
        {
          326.0,
          394.5f
        },
        {
          327.0,
          395.25f
        },
        {
          328.0,
          396f
        },
        {
          329.0,
          398.25f
        },
        {
          330.0,
          399f
        },
        {
          331.0,
          399.75f
        },
        {
          332.0,
          401.25f
        },
        {
          333.0,
          402.75f
        },
        {
          334.0,
          403.5f
        },
        {
          335.0,
          405f
        },
        {
          336.0,
          406.5f
        },
        {
          337.0,
          407.25f
        },
        {
          338.0,
          408.75f
        },
        {
          339.0,
          409.5f
        },
        {
          340.0,
          409.5f
        },
        {
          341.0,
          409.5f
        },
        {
          342.0,
          409.5f
        },
        {
          343.0,
          409.5f
        },
        {
          344.0,
          409.5f
        },
        {
          345.0,
          409.5f
        },
        {
          346.0,
          409.5f
        },
        {
          347.0,
          409.5f
        },
        {
          348.0,
          409.5f
        },
        {
          349.0,
          409.5f
        },
        {
          350.0,
          409.5f
        },
        {
          351.0,
          409.5f
        },
        {
          352.0,
          409.5f
        },
        {
          353.0,
          409.5f
        },
        {
          354.0,
          409.5f
        },
        {
          355.0,
          409.5f
        },
        {
          356.0,
          409.5f
        },
        {
          357.0,
          409.5f
        },
        {
          358.0,
          409.5f
        },
        {
          359.0,
          409.5f
        },
        {
          360.0,
          409.5f
        },
        {
          361.0,
          409.5f
        },
        {
          362.0,
          409.5f
        },
        {
          363.0,
          409.5f
        },
        {
          364.0,
          409.5f
        },
        {
          365.0,
          409.5f
        },
        {
          366.0,
          409.5f
        },
        {
          367.0,
          409.5f
        },
        {
          368.0,
          409.5f
        },
        {
          369.0,
          409.5f
        },
        {
          370.0,
          409.5f
        },
        {
          371.0,
          409.5f
        },
        {
          372.0,
          409.5f
        },
        {
          373.0,
          409.5f
        },
        {
          374.0,
          409.5f
        },
        {
          375.0,
          409.5f
        },
        {
          376.0,
          409.5f
        },
        {
          377.0,
          409.5f
        },
        {
          378.0,
          409.5f
        },
        {
          379.0,
          409.5f
        },
        {
          380.0,
          409.5f
        },
        {
          381.0,
          409.5f
        },
        {
          382.0,
          409.5f
        },
        {
          383.0,
          409.5f
        },
        {
          384.0,
          409.5f
        },
        {
          385.0,
          409.5f
        },
        {
          386.0,
          409.5f
        },
        {
          387.0,
          409.5f
        },
        {
          388.0,
          409.5f
        },
        {
          389.0,
          409.5f
        },
        {
          390.0,
          409.5f
        },
        {
          391.0,
          409.5f
        },
        {
          392.0,
          409.5f
        },
        {
          393.0,
          409.5f
        },
        {
          394.0,
          409.5f
        },
        {
          395.0,
          409.5f
        },
        {
          396.0,
          409.5f
        },
        {
          397.0,
          409.5f
        },
        {
          398.0,
          409.5f
        },
        {
          399.0,
          409.5f
        },
        {
          400.0,
          409.5f
        },
        {
          401.0,
          409.5f
        },
        {
          402.0,
          409.5f
        },
        {
          403.0,
          409.5f
        },
        {
          404.0,
          409.5f
        },
        {
          405.0,
          409.5f
        },
        {
          406.0,
          409.5f
        },
        {
          407.0,
          409.5f
        },
        {
          408.0,
          409.5f
        },
        {
          409.0,
          409.5f
        }
      });
      ApplicationImpl.m_fontsHeight.Add("Arial", new Dictionary<double, float>(409)
      {
        {
          1.0,
          5.25f
        },
        {
          2.0,
          5.25f
        },
        {
          3.0,
          6f
        },
        {
          4.0,
          6.75f
        },
        {
          5.0,
          8.25f
        },
        {
          6.0,
          8.25f
        },
        {
          7.0,
          9f
        },
        {
          8.0,
          11.25f
        },
        {
          9.0,
          12f
        },
        {
          10.0,
          12.75f
        },
        {
          11.0,
          14.25f
        },
        {
          12.0,
          15f
        },
        {
          13.0,
          16.5f
        },
        {
          14.0,
          18f
        },
        {
          15.0,
          18.75f
        },
        {
          16.0,
          20.25f
        },
        {
          17.0,
          21.75f
        },
        {
          18.0,
          23.25f
        },
        {
          19.0,
          23.25f
        },
        {
          20.0,
          25.5f
        },
        {
          21.0,
          26.25f
        },
        {
          22.0,
          27f
        },
        {
          23.0,
          29.25f
        },
        {
          24.0,
          30f
        },
        {
          25.0,
          30.75f
        },
        {
          26.0,
          33f
        },
        {
          27.0,
          33.75f
        },
        {
          28.0,
          34.5f
        },
        {
          29.0,
          36.75f
        },
        {
          30.0,
          37.5f
        },
        {
          31.0,
          38.25f
        },
        {
          32.0,
          40.5f
        },
        {
          33.0,
          41.25f
        },
        {
          34.0,
          42f
        },
        {
          35.0,
          43.5f
        },
        {
          36.0,
          44.25f
        },
        {
          37.0,
          45.75f
        },
        {
          38.0,
          47.25f
        },
        {
          39.0,
          48.75f
        },
        {
          40.0,
          49.5f
        },
        {
          41.0,
          51f
        },
        {
          42.0,
          52.5f
        },
        {
          43.0,
          53.25f
        },
        {
          44.0,
          54.75f
        },
        {
          45.0,
          55.5f
        },
        {
          46.0,
          56.25f
        },
        {
          47.0,
          58.5f
        },
        {
          48.0,
          59.25f
        },
        {
          49.0,
          60f
        },
        {
          50.0,
          62.25f
        },
        {
          51.0,
          63f
        },
        {
          52.0,
          63.75f
        },
        {
          53.0,
          66f
        },
        {
          54.0,
          66.75f
        },
        {
          55.0,
          67.5f
        },
        {
          56.0,
          69f
        },
        {
          57.0,
          69.75f
        },
        {
          58.0,
          72.75f
        },
        {
          59.0,
          74.25f
        },
        {
          60.0,
          75f
        },
        {
          61.0,
          76.5f
        },
        {
          62.0,
          78f
        },
        {
          63.0,
          79.5f
        },
        {
          64.0,
          80.25f
        },
        {
          65.0,
          81.75f
        },
        {
          66.0,
          83.25f
        },
        {
          67.0,
          84f
        },
        {
          68.0,
          85.5f
        },
        {
          69.0,
          86.25f
        },
        {
          70.0,
          87f
        },
        {
          71.0,
          89.25f
        },
        {
          72.0,
          90f
        },
        {
          73.0,
          90.75f
        },
        {
          74.0,
          93f
        },
        {
          75.0,
          93.75f
        },
        {
          76.0,
          94.5f
        },
        {
          77.0,
          96.75f
        },
        {
          78.0,
          97.5f
        },
        {
          79.0,
          99f
        },
        {
          80.0,
          99.75f
        },
        {
          81.0,
          100.5f
        },
        {
          82.0,
          102f
        },
        {
          83.0,
          103.5f
        },
        {
          84.0,
          105f
        },
        {
          85.0,
          105.75f
        },
        {
          86.0,
          107.25f
        },
        {
          87.0,
          108.75f
        },
        {
          88.0,
          109.5f
        },
        {
          89.0,
          111.75f
        },
        {
          90.0,
          112.5f
        },
        {
          91.0,
          113.25f
        },
        {
          92.0,
          114.75f
        },
        {
          93.0,
          115.5f
        },
        {
          94.0,
          116.25f
        },
        {
          95.0,
          118.5f
        },
        {
          96.0,
          119.25f
        },
        {
          97.0,
          120f
        },
        {
          98.0,
          122.25f
        },
        {
          99.0,
          123f
        },
        {
          100.0,
          124.5f
        },
        {
          101.0,
          126f
        },
        {
          102.0,
          126.75f
        },
        {
          103.0,
          128.25f
        },
        {
          104.0,
          130.5f
        },
        {
          105.0,
          132f
        },
        {
          106.0,
          132.75f
        },
        {
          107.0,
          134.25f
        },
        {
          108.0,
          135.75f
        },
        {
          109.0,
          136.5f
        },
        {
          110.0,
          138.75f
        },
        {
          111.0,
          139.5f
        },
        {
          112.0,
          140.25f
        },
        {
          113.0,
          141f
        },
        {
          114.0,
          142.5f
        },
        {
          115.0,
          143.25f
        },
        {
          116.0,
          145.5f
        },
        {
          117.0,
          146.25f
        },
        {
          118.0,
          147f
        },
        {
          119.0,
          148.5f
        },
        {
          120.0,
          149.25f
        },
        {
          121.0,
          150.75f
        },
        {
          122.0,
          152.25f
        },
        {
          123.0,
          153f
        },
        {
          124.0,
          154.5f
        },
        {
          125.0,
          156.75f
        },
        {
          126.0,
          157.5f
        },
        {
          (double) sbyte.MaxValue,
          158.25f
        },
        {
          128.0,
          159.75f
        },
        {
          129.0,
          160.5f
        },
        {
          130.0,
          161.25f
        },
        {
          131.0,
          162.75f
        },
        {
          132.0,
          163.5f
        },
        {
          133.0,
          165f
        },
        {
          134.0,
          166.5f
        },
        {
          135.0,
          168f
        },
        {
          136.0,
          169.5f
        },
        {
          137.0,
          171f
        },
        {
          138.0,
          171.75f
        },
        {
          139.0,
          172.5f
        },
        {
          140.0,
          174f
        },
        {
          141.0,
          175.5f
        },
        {
          142.0,
          176.25f
        },
        {
          143.0,
          177.75f
        },
        {
          144.0,
          178.5f
        },
        {
          145.0,
          179.25f
        },
        {
          146.0,
          181.5f
        },
        {
          147.0,
          182.25f
        },
        {
          148.0,
          183.75f
        },
        {
          149.0,
          186.75f
        },
        {
          150.0,
          188.25f
        },
        {
          151.0,
          189f
        },
        {
          152.0,
          190.5f
        },
        {
          153.0,
          191.25f
        },
        {
          154.0,
          192f
        },
        {
          155.0,
          193.5f
        },
        {
          156.0,
          194.25f
        },
        {
          157.0,
          195f
        },
        {
          158.0,
          198f
        },
        {
          159.0,
          198.75f
        },
        {
          160.0,
          199.5f
        },
        {
          161.0,
          201.75f
        },
        {
          162.0,
          202.5f
        },
        {
          163.0,
          203.25f
        },
        {
          164.0,
          204.75f
        },
        {
          165.0,
          205.5f
        },
        {
          166.0,
          206.25f
        },
        {
          167.0,
          208.5f
        },
        {
          168.0,
          210f
        },
        {
          169.0,
          210.75f
        },
        {
          170.0,
          212.25f
        },
        {
          171.0,
          213f
        },
        {
          172.0,
          213.75f
        },
        {
          173.0,
          216f
        },
        {
          174.0,
          216.75f
        },
        {
          175.0,
          217.5f
        },
        {
          176.0,
          219.75f
        },
        {
          177.0,
          220.5f
        },
        {
          178.0,
          221.25f
        },
        {
          179.0,
          223.5f
        },
        {
          180.0,
          224.25f
        },
        {
          181.0,
          225f
        },
        {
          182.0,
          226.5f
        },
        {
          183.0,
          227.25f
        },
        {
          184.0,
          228.75f
        },
        {
          185.0,
          230.25f
        },
        {
          186.0,
          231.75f
        },
        {
          187.0,
          232.5f
        },
        {
          188.0,
          234f
        },
        {
          189.0,
          234.75f
        },
        {
          190.0,
          236.25f
        },
        {
          191.0,
          237f
        },
        {
          192.0,
          237.75f
        },
        {
          193.0,
          238.5f
        },
        {
          194.0,
          240.75f
        },
        {
          195.0,
          243f
        },
        {
          196.0,
          243.75f
        },
        {
          197.0,
          246f
        },
        {
          198.0,
          246.75f
        },
        {
          199.0,
          247.5f
        },
        {
          200.0,
          249.75f
        },
        {
          201.0,
          250.5f
        },
        {
          202.0,
          251.25f
        },
        {
          203.0,
          252.75f
        },
        {
          204.0,
          254.25f
        },
        {
          205.0,
          (float) byte.MaxValue
        },
        {
          206.0,
          256.5f
        },
        {
          207.0,
          257.25f
        },
        {
          208.0,
          258.75f
        },
        {
          209.0,
          260.25f
        },
        {
          210.0,
          261f
        },
        {
          211.0,
          262.5f
        },
        {
          212.0,
          264f
        },
        {
          213.0,
          264.75f
        },
        {
          214.0,
          265.5f
        },
        {
          215.0,
          267.75f
        },
        {
          216.0,
          268.5f
        },
        {
          217.0,
          269.25f
        },
        {
          218.0,
          271.5f
        },
        {
          219.0,
          272.25f
        },
        {
          220.0,
          273f
        },
        {
          221.0,
          275.25f
        },
        {
          222.0,
          276f
        },
        {
          223.0,
          276.75f
        },
        {
          224.0,
          278.25f
        },
        {
          225.0,
          279.75f
        },
        {
          226.0,
          280.5f
        },
        {
          227.0,
          282f
        },
        {
          228.0,
          282.75f
        },
        {
          229.0,
          284.25f
        },
        {
          230.0,
          285.75f
        },
        {
          231.0,
          286.5f
        },
        {
          232.0,
          287.25f
        },
        {
          233.0,
          289.5f
        },
        {
          234.0,
          290.25f
        },
        {
          235.0,
          291f
        },
        {
          236.0,
          293.25f
        },
        {
          237.0,
          294f
        },
        {
          238.0,
          294.75f
        },
        {
          239.0,
          297f
        },
        {
          240.0,
          297.75f
        },
        {
          241.0,
          300f
        },
        {
          242.0,
          301.5f
        },
        {
          243.0,
          303f
        },
        {
          244.0,
          303.75f
        },
        {
          245.0,
          305.25f
        },
        {
          246.0,
          306.75f
        },
        {
          247.0,
          307.5f
        },
        {
          248.0,
          309f
        },
        {
          249.0,
          309.75f
        },
        {
          250.0,
          311.25f
        },
        {
          251.0,
          312.75f
        },
        {
          252.0,
          313.5f
        },
        {
          253.0,
          314.25f
        },
        {
          254.0,
          316.5f
        },
        {
          (double) byte.MaxValue,
          317.25f
        },
        {
          256.0,
          318f
        },
        {
          257.0,
          320.25f
        },
        {
          258.0,
          321f
        },
        {
          259.0,
          321.75f
        },
        {
          260.0,
          324f
        },
        {
          261.0,
          324.75f
        },
        {
          262.0,
          325.5f
        },
        {
          263.0,
          327f
        },
        {
          264.0,
          328.5f
        },
        {
          265.0,
          329.25f
        },
        {
          266.0,
          330.75f
        },
        {
          267.0,
          331.5f
        },
        {
          268.0,
          333f
        },
        {
          269.0,
          334.5f
        },
        {
          270.0,
          335.25f
        },
        {
          271.0,
          336.75f
        },
        {
          272.0,
          338.25f
        },
        {
          273.0,
          339f
        },
        {
          274.0,
          339.75f
        },
        {
          275.0,
          342f
        },
        {
          276.0,
          342.75f
        },
        {
          277.0,
          343.5f
        },
        {
          278.0,
          345.75f
        },
        {
          279.0,
          346.5f
        },
        {
          280.0,
          347.25f
        },
        {
          281.0,
          348.75f
        },
        {
          282.0,
          350.25f
        },
        {
          283.0,
          351f
        },
        {
          284.0,
          352.5f
        },
        {
          285.0,
          354f
        },
        {
          286.0,
          354.75f
        },
        {
          287.0,
          357.75f
        },
        {
          288.0,
          358.5f
        },
        {
          289.0,
          360f
        },
        {
          290.0,
          361.5f
        },
        {
          291.0,
          362.25f
        },
        {
          292.0,
          363f
        },
        {
          293.0,
          365.25f
        },
        {
          294.0,
          366f
        },
        {
          295.0,
          366.75f
        },
        {
          296.0,
          369f
        },
        {
          297.0,
          369.75f
        },
        {
          298.0,
          370.5f
        },
        {
          299.0,
          372.75f
        },
        {
          300.0,
          373.5f
        },
        {
          301.0,
          374.25f
        },
        {
          302.0,
          375.75f
        },
        {
          303.0,
          377.25f
        },
        {
          304.0,
          378f
        },
        {
          305.0,
          379.5f
        },
        {
          306.0,
          380.25f
        },
        {
          307.0,
          381.75f
        },
        {
          308.0,
          383.25f
        },
        {
          309.0,
          384f
        },
        {
          310.0,
          385.5f
        },
        {
          311.0,
          387f
        },
        {
          312.0,
          387.75f
        },
        {
          313.0,
          388.5f
        },
        {
          314.0,
          390.75f
        },
        {
          315.0,
          391.5f
        },
        {
          316.0,
          392.25f
        },
        {
          317.0,
          394.5f
        },
        {
          318.0,
          395.25f
        },
        {
          319.0,
          396f
        },
        {
          320.0,
          397.5f
        },
        {
          321.0,
          399f
        },
        {
          322.0,
          399.75f
        },
        {
          323.0,
          401.25f
        },
        {
          324.0,
          402.75f
        },
        {
          325.0,
          403.5f
        },
        {
          326.0,
          405f
        },
        {
          327.0,
          405.75f
        },
        {
          328.0,
          407.25f
        },
        {
          329.0,
          408.75f
        },
        {
          330.0,
          409.5f
        },
        {
          331.0,
          409.5f
        },
        {
          332.0,
          409.5f
        },
        {
          333.0,
          409.5f
        },
        {
          334.0,
          409.5f
        },
        {
          335.0,
          409.5f
        },
        {
          336.0,
          409.5f
        },
        {
          337.0,
          409.5f
        },
        {
          338.0,
          409.5f
        },
        {
          339.0,
          409.5f
        },
        {
          340.0,
          409.5f
        },
        {
          341.0,
          409.5f
        },
        {
          342.0,
          409.5f
        },
        {
          343.0,
          409.5f
        },
        {
          344.0,
          409.5f
        },
        {
          345.0,
          409.5f
        },
        {
          346.0,
          409.5f
        },
        {
          347.0,
          409.5f
        },
        {
          348.0,
          409.5f
        },
        {
          349.0,
          409.5f
        },
        {
          350.0,
          409.5f
        },
        {
          351.0,
          409.5f
        },
        {
          352.0,
          409.5f
        },
        {
          353.0,
          409.5f
        },
        {
          354.0,
          409.5f
        },
        {
          355.0,
          409.5f
        },
        {
          356.0,
          409.5f
        },
        {
          357.0,
          409.5f
        },
        {
          358.0,
          409.5f
        },
        {
          359.0,
          409.5f
        },
        {
          360.0,
          409.5f
        },
        {
          361.0,
          409.5f
        },
        {
          362.0,
          409.5f
        },
        {
          363.0,
          409.5f
        },
        {
          364.0,
          409.5f
        },
        {
          365.0,
          409.5f
        },
        {
          366.0,
          409.5f
        },
        {
          367.0,
          409.5f
        },
        {
          368.0,
          409.5f
        },
        {
          369.0,
          409.5f
        },
        {
          370.0,
          409.5f
        },
        {
          371.0,
          409.5f
        },
        {
          372.0,
          409.5f
        },
        {
          373.0,
          409.5f
        },
        {
          374.0,
          409.5f
        },
        {
          375.0,
          409.5f
        },
        {
          376.0,
          409.5f
        },
        {
          377.0,
          409.5f
        },
        {
          378.0,
          409.5f
        },
        {
          379.0,
          409.5f
        },
        {
          380.0,
          409.5f
        },
        {
          381.0,
          409.5f
        },
        {
          382.0,
          409.5f
        },
        {
          383.0,
          409.5f
        },
        {
          384.0,
          409.5f
        },
        {
          385.0,
          409.5f
        },
        {
          386.0,
          409.5f
        },
        {
          387.0,
          409.5f
        },
        {
          388.0,
          409.5f
        },
        {
          389.0,
          409.5f
        },
        {
          390.0,
          409.5f
        },
        {
          391.0,
          409.5f
        },
        {
          392.0,
          409.5f
        },
        {
          393.0,
          409.5f
        },
        {
          394.0,
          409.5f
        },
        {
          395.0,
          409.5f
        },
        {
          396.0,
          409.5f
        },
        {
          397.0,
          409.5f
        },
        {
          398.0,
          409.5f
        },
        {
          399.0,
          409.5f
        },
        {
          400.0,
          409.5f
        },
        {
          401.0,
          409.5f
        },
        {
          402.0,
          409.5f
        },
        {
          403.0,
          409.5f
        },
        {
          404.0,
          409.5f
        },
        {
          405.0,
          409.5f
        },
        {
          406.0,
          409.5f
        },
        {
          407.0,
          409.5f
        },
        {
          408.0,
          409.5f
        },
        {
          409.0,
          409.5f
        }
      });
    }
  }

  public double CentimetersToPoints(double Centimeters)
  {
    return this.ConvertUnits(Centimeters, MeasureUnits.Centimeter, MeasureUnits.Point);
  }

  public bool IsSupported(string FilePath)
  {
    if (File.Exists(FilePath))
    {
      using (FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
      {
        if (fileStream != null)
          return this.IsSupported((Stream) fileStream);
      }
    }
    return false;
  }

  public bool IsSupported(Stream stream)
  {
    return this.DetectFileFromStream(stream) != ExcelOpenType.Automatic;
  }

  public ExcelOpenType DetectFileFromStream(Stream stream)
  {
    ExcelOpenType excelOpenType = ExcelOpenType.Automatic;
    long lPosition = stream != null ? stream.Position : throw new ArgumentNullException(nameof (stream));
    if (ZipArchive.ReadInt32(stream) == 67324752)
    {
      stream.Position = lPosition;
      excelOpenType = ExcelOpenType.SpreadsheetML2007;
    }
    else
    {
      stream.Position = lPosition;
      byte[] buffer = new byte[512 /*0x0200*/];
      int count = stream.Read(buffer, 0, 512 /*0x0200*/);
      bool flag = false;
      if (count != 0)
      {
        if (count >= 8)
        {
          for (int index = 0; index < 8; ++index)
          {
            if ((int) ApplicationImpl.DEF_XLS_FILE_HEADER[index] == (int) buffer[index])
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
        stream.Position = lPosition;
        if (flag)
        {
          using (ICompoundFile compoundFile = this.CreateCompoundFile(stream))
          {
            excelOpenType = Excel2007Decryptor.CheckEncrypted(compoundFile.RootStorage) ? ExcelOpenType.SpreadsheetML2007 : ExcelOpenType.BIFF;
            if (excelOpenType == ExcelOpenType.BIFF)
            {
              if (this.FindStreamCaseInsensitive(compoundFile.RootStorage, "Workbook") == null)
              {
                if (this.FindStreamCaseInsensitive(compoundFile.RootStorage, "Book") == "Book")
                  excelOpenType = ExcelOpenType.Automatic;
              }
            }
          }
        }
        else
        {
          using (MemoryStream memoryStream = new MemoryStream(buffer, 0, count))
            excelOpenType = this.DetectIsTSVOrCSVOrXML(stream, memoryStream, lPosition);
        }
      }
    }
    stream.Position = lPosition;
    return excelOpenType;
  }

  private string FindStreamCaseInsensitive(ICompoundStorage storage, string streamName)
  {
    switch (streamName)
    {
      case null:
        throw new ArgumentNullException("strStreamName");
      case "":
        throw new ArgumentException("strStreamName - string cannot be empty.");
      default:
        string[] streams = storage.Streams;
        string streamCaseInsensitive = (string) null;
        int index = 0;
        for (int length = streams.Length; index < length; ++index)
        {
          string strA = streams[index];
          if (string.Compare(strA, streamName, StringComparison.CurrentCultureIgnoreCase) == 0)
          {
            streamCaseInsensitive = strA;
            break;
          }
        }
        return streamCaseInsensitive;
    }
  }

  private ExcelOpenType DetectIsTSVOrCSVOrXML(
    Stream stream,
    MemoryStream memoryStream,
    long lPosition)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (memoryStream == null)
      throw new ArgumentNullException(nameof (memoryStream));
    string csvSeparator = this.Application.CSVSeparator;
    bool bIsCompare = this.IsContainSurrogate(csvSeparator, "", false);
    bool flag1 = false;
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    bool flag2 = false;
    bool flag3 = false;
    StreamReader streamReader = new StreamReader((Stream) memoryStream, true);
    string str = streamReader.ReadLine();
    Encoding currentEncoding = streamReader.CurrentEncoding;
    ExcelOpenType excelOpenType = ExcelOpenType.Automatic;
    for (; str != null; str = streamReader.ReadLine())
    {
      string lower = str.ToLower();
      int num5 = lower.IndexOf("<?xml");
      if (num5 != -1)
      {
        stream.Position = lPosition + (long) num5;
        excelOpenType = ExcelOpenType.SpreadsheetML;
        break;
      }
      if (lower.IndexOf("<html") == -1)
      {
        if (!flag1)
          flag1 = this.IsContainSurrogate(lower, csvSeparator, bIsCompare);
        if (!flag1)
        {
          char ch = Convert.ToChar("\t");
          int length1 = lower.Split(ch).Length;
          object[] objArray = new object[length1];
          for (int index = 0; index < length1; ++index)
            objArray[index] = lower.Split(ch).GetValue(index);
          int length2 = objArray.Length;
          if (length2 != 0 && length2 == num4)
            flag2 = true;
          num4 = length2;
          num2 += num4;
        }
        if (!flag1 && csvSeparator.Length == 1)
        {
          char ch = Convert.ToChar(csvSeparator);
          int length3 = lower.Split(ch).Length;
          object[] objArray = new object[length3];
          for (int index = 0; index < length3; ++index)
            objArray[index] = lower.Split(ch).GetValue(index);
          int length4 = objArray.Length;
          if (length4 != 0 && num3 == length4)
            flag3 = true;
          num3 = length4;
          num1 += num3;
        }
        lPosition += (long) currentEncoding.GetByteCount(lower);
        lPosition += (long) (currentEncoding.GetByteCount("\n") * 2);
      }
      else
        break;
    }
    if (flag2 && flag3 && excelOpenType == ExcelOpenType.Automatic && !flag1 && num2 > num1)
      excelOpenType = ExcelOpenType.TSV;
    if (excelOpenType == ExcelOpenType.Automatic && !flag3 && !flag1 && num2 > num1)
      excelOpenType = ExcelOpenType.TSV;
    if (excelOpenType == ExcelOpenType.Automatic && !flag1)
      excelOpenType = ExcelOpenType.CSV;
    return !flag1 ? excelOpenType : ExcelOpenType.Automatic;
  }

  private bool IsContainSurrogate(string strValue, string strSeparator, bool bIsCompare)
  {
    if (strValue == null)
      throw new ArgumentNullException(nameof (strValue));
    if (strSeparator == null)
      throw new ArgumentNullException(nameof (strSeparator));
    int index = 0;
    for (int length = strValue.Length; index < length; ++index)
    {
      char c = strValue[index];
      if (!char.IsLetterOrDigit(c) && !char.IsPunctuation(c) && !char.IsSeparator(c) && !char.IsSymbol(c) && !char.IsWhiteSpace(c) && (!bIsCompare || strSeparator.IndexOf(c) == -1))
        return true;
    }
    return false;
  }

  public double InchesToPoints(double Inches)
  {
    return this.ConvertUnits(Inches, MeasureUnits.Inch, MeasureUnits.Point);
  }

  public void Save(string Filename)
  {
    if (this.ActiveWorkbook == null)
      return;
    this.ActiveWorkbook.SaveAs(Filename);
  }

  public virtual WorkbookImpl CreateWorkbook(object parent, ExcelVersion version)
  {
    return new WorkbookImpl((IApplication) this, parent, version);
  }

  public virtual WorkbookImpl CreateWorkbook(
    object parent,
    Stream stream,
    string separator,
    int row,
    int column,
    ExcelVersion version,
    string fileName,
    Encoding encoding)
  {
    return new WorkbookImpl((IApplication) this, parent, stream, separator, row, column, version, fileName, encoding);
  }

  public virtual WorkbookImpl CreateWorkbook(
    object parent,
    Stream stream,
    ExcelVersion version,
    ExcelParseOptions options)
  {
    return new WorkbookImpl((IApplication) this, parent, stream, options, version);
  }

  public virtual WorkbookImpl CreateWorkbook(
    object parent,
    Stream stream,
    ExcelParseOptions options,
    ExcelVersion version)
  {
    return new WorkbookImpl((IApplication) this, parent, stream, options, version);
  }

  public virtual WorkbookImpl CreateWorkbook(
    object parent,
    string strTemplateFile,
    ExcelParseOptions options,
    bool bReadOnly,
    string password,
    ExcelVersion version)
  {
    return new WorkbookImpl((IApplication) this, parent, strTemplateFile, options, bReadOnly, password, version);
  }

  public virtual WorkbookImpl CreateWorkbook(
    object parent,
    Stream stream,
    ExcelParseOptions options,
    bool bReadOnly,
    string password,
    ExcelVersion version)
  {
    return new WorkbookImpl((IApplication) this, parent, stream, options, bReadOnly, password, version);
  }

  public virtual WorkbookImpl CreateWorkbook(
    object parent,
    int sheetsQuantity,
    ExcelVersion version)
  {
    return new WorkbookImpl((IApplication) this, parent, sheetsQuantity, version);
  }

  public virtual WorkbookImpl CreateWorkbook(
    object parent,
    string strTemplateFile,
    ExcelVersion version)
  {
    return new WorkbookImpl((IApplication) this, parent, strTemplateFile, version);
  }

  public virtual WorkbookImpl CreateWorkbook(
    object parent,
    string strTemplateFile,
    ExcelParseOptions options,
    ExcelVersion version)
  {
    return new WorkbookImpl((IApplication) this, parent, strTemplateFile, options, version);
  }

  public virtual WorksheetImpl CreateWorksheet(object parent)
  {
    return new WorksheetImpl((IApplication) this, parent);
  }

  [CLSCompliant(false)]
  public virtual WorksheetImpl CreateWorksheet(
    object parent,
    BiffReader reader,
    ExcelParseOptions options,
    bool bSkipParsing,
    Dictionary<int, int> hashNewXFormatIndexes,
    IDecryptor decryptor)
  {
    return new WorksheetImpl((IApplication) this, parent, reader, options, bSkipParsing, hashNewXFormatIndexes, decryptor);
  }

  public virtual RangeImpl CreateRange(object parent) => new RangeImpl((IApplication) this, parent);

  public virtual RangeImpl CreateRange(object parent, int col, int row)
  {
    return new RangeImpl((IApplication) this, parent, col, row);
  }

  [CLSCompliant(false)]
  public virtual RangeImpl CreateRange(object parent, BiffRecordRaw[] data, ref int i)
  {
    return new RangeImpl((IApplication) this, parent, data, i);
  }

  [CLSCompliant(false)]
  public virtual RangeImpl CreateRange(
    object parent,
    BiffRecordRaw[] data,
    ref int i,
    bool ignoreStyles)
  {
    return new RangeImpl((IApplication) this, parent, data, ref i, ignoreStyles);
  }

  public virtual RangeImpl CreateRange(
    object parent,
    List<BiffRecordRaw> data,
    ref int i,
    bool ignoreStyles)
  {
    return new RangeImpl((IApplication) this, parent, data, ref i, ignoreStyles);
  }

  public virtual RangeImpl CreateRange(
    object parent,
    int firstCol,
    int firstRow,
    int lastCol,
    int lastRow)
  {
    return new RangeImpl((IApplication) this, parent, firstCol, firstRow, lastCol, lastRow);
  }

  [CLSCompliant(false)]
  public virtual RangeImpl CreateRange(object parent, BiffRecordRaw record, bool bIgnoreStyles)
  {
    return new RangeImpl((IApplication) this, parent, record, bIgnoreStyles);
  }

  public virtual StyleImpl CreateStyle(WorkbookImpl parent, string name)
  {
    return new StyleImpl(parent, name);
  }

  public virtual StyleImpl CreateStyle(WorkbookImpl parent, string name, StyleImpl basedOn)
  {
    return new StyleImpl(parent, name, basedOn);
  }

  [CLSCompliant(false)]
  public virtual StyleImpl CreateStyle(WorkbookImpl parent, StyleRecord style)
  {
    return new StyleImpl(parent, style);
  }

  public virtual StyleImpl CreateStyle(WorkbookImpl parent, string name, bool bIsBuildIn)
  {
    return new StyleImpl(parent, name, (StyleImpl) null, bIsBuildIn);
  }

  public virtual FontImpl CreateFont(object parent) => new FontImpl((IApplication) this, parent);

  public virtual FontImpl CreateFont(object parent, Font nativeFont)
  {
    return new FontImpl((IApplication) this, parent, nativeFont);
  }

  public virtual FontImpl CreateFont(IFont basedOn) => new FontImpl(basedOn);

  [CLSCompliant(false)]
  public virtual FontImpl CreateFont(object parent, FontRecord font)
  {
    return new FontImpl((IApplication) this, parent, font);
  }

  [CLSCompliant(false)]
  public virtual FontImpl CreateFont(object parent, FontImpl font)
  {
    return new FontImpl((IApplication) this, parent, font);
  }

  public void CheckDefaultFont()
  {
    if (this.m_defaultVersion == ExcelVersion.Excel97to2003)
    {
      this.StandardFont = "Tahoma";
      this.StandardFontSize = 10.0;
    }
    else
    {
      this.StandardFont = "Calibri";
      this.StandardFontSize = 11.0;
    }
  }

  public virtual ClipboardProvider CreateClipboardProvider()
  {
    return (ClipboardProvider) new Biff8ClipboardProvider((ClipboardProvider) new DelimiterClipboardProvider());
  }

  public virtual ClipboardProvider CreateClipboardProvider(IWorksheet sheet)
  {
    return (ClipboardProvider) new Biff8ClipboardProvider(sheet, (ClipboardProvider) new DelimiterClipboardProvider(sheet));
  }

  public virtual ChartImpl CreateChart(object parent) => new ChartImpl((IApplication) this, parent);

  public virtual ChartSerieImpl CreateSerie(object parent)
  {
    return new ChartSerieImpl((IApplication) this, parent);
  }

  public RangesCollection CreateRangesCollection(object parent)
  {
    return new RangesCollection((IApplication) this, parent);
  }

  public virtual HyperLinkImpl CreateHyperLink(object parent)
  {
    return new HyperLinkImpl((IApplication) this, parent);
  }

  public virtual HyperLinkImpl CreateHyperLink(object parent, IRange range)
  {
    return new HyperLinkImpl((IApplication) this, parent, range);
  }

  internal virtual HyperLinkImpl CreateHyperLink(object parent, IShape shape)
  {
    return new HyperLinkImpl((IApplication) this, parent, shape);
  }

  public virtual CommentsRange CreateCommentsRange(IRange parentRange)
  {
    return new CommentsRange((IApplication) this, parentRange);
  }

  public virtual CommentShapeImpl CreateCommentShapeImpl(object parent)
  {
    return this.CreateCommentShapeImpl(parent, true);
  }

  public virtual CommentShapeImpl CreateCommentShapeImpl(object parent, bool bIsParseOptions)
  {
    return new CommentShapeImpl((IApplication) this, parent, bIsParseOptions);
  }

  [CLSCompliant(false)]
  public virtual CommentShapeImpl CreateCommentShapeImpl(object parent, MsofbtSpContainer container)
  {
    return new CommentShapeImpl((IApplication) this, parent, container);
  }

  [CLSCompliant(false)]
  public virtual CommentShapeImpl CreateCommentShapeImpl(
    object parent,
    MsofbtSpContainer container,
    ExcelParseOptions options)
  {
    return new CommentShapeImpl((IApplication) this, parent, container, options);
  }

  public virtual DataValidationArray CreateDataValidationArrayImpl(IRange parent)
  {
    return new DataValidationArray(parent);
  }

  public virtual DataValidationWrapper CreateDataValidationWrapper(
    RangeImpl range,
    DataValidationImpl wrap)
  {
    return new DataValidationWrapper(range, wrap);
  }

  public virtual DataValidationImpl CreateDataValidationImpl(DataValidationCollection parent)
  {
    return new DataValidationImpl(parent);
  }

  [CLSCompliant(false)]
  public virtual DataValidationImpl CreateDataValidationImpl(
    DataValidationCollection parent,
    DVRecord dv)
  {
    return new DataValidationImpl(parent, dv);
  }

  public virtual CondFormatCollectionWrapper CreateCondFormatCollectionWrapper(ICombinedRange range)
  {
    return new CondFormatCollectionWrapper(range);
  }

  [CLSCompliant(false)]
  public virtual ConditionalFormats CreateConditionalFormats(
    object parent,
    CondFMTRecord format,
    IList formats,
    IList CFExRecords)
  {
    return new ConditionalFormats((IApplication) this, parent, format, formats, CFExRecords);
  }

  [CLSCompliant(false)]
  public virtual ConditionalFormats CreateConditionalFormats(
    object parent,
    CondFmt12Record format,
    IList formats)
  {
    return new ConditionalFormats((IApplication) this, parent, format, formats);
  }

  public TemplateMarkersImpl CreateTemplateMarkers(object parent)
  {
    return new TemplateMarkersImpl((IApplication) this, parent);
  }

  public static DataProvider CreateDataProvider(IntPtr heapHandle)
  {
    DataProvider dataProvider = (DataProvider) null;
    switch (ApplicationImpl.m_dataType)
    {
      case ExcelDataProviderType.Native:
        dataProvider = (DataProvider) new IntPtrDataProvider(heapHandle);
        break;
      case ExcelDataProviderType.Unsafe:
        dataProvider = (DataProvider) new UnsafeDataProvider(heapHandle);
        break;
      case ExcelDataProviderType.ByteArray:
        dataProvider = (DataProvider) new ByteArrayDataProvider();
        break;
    }
    return dataProvider;
  }

  internal static DataProvider CreateDataProvider()
  {
    DataProvider dataProvider = (DataProvider) null;
    switch (ApplicationImpl.m_dataType)
    {
      case ExcelDataProviderType.Native:
        dataProvider = (DataProvider) new IntPtrDataProvider(IntPtr.Zero);
        break;
      case ExcelDataProviderType.Unsafe:
        dataProvider = (DataProvider) new UnsafeDataProvider(IntPtr.Zero);
        break;
      case ExcelDataProviderType.ByteArray:
        dataProvider = (DataProvider) new ByteArrayDataProvider();
        break;
    }
    return dataProvider;
  }

  internal ICompoundFile CreateCompoundFile(Stream stream)
  {
    return this.m_bNetStorage ? (ICompoundFile) new Syncfusion.CompoundFile.XlsIO.Net.CompoundFile(stream) : (ICompoundFile) new Syncfusion.CompoundFile.XlsIO.Native.CompoundFile(stream);
  }

  internal ICompoundFile CreateCompoundFile(string fileName, STGM storageOptions)
  {
    bool create = (storageOptions & STGM.STGM_CREATE) != STGM.STGM_READ;
    ICompoundFile compoundFile;
    if (this.m_bNetStorage)
      compoundFile = (ICompoundFile) new Syncfusion.CompoundFile.XlsIO.Net.CompoundFile(fileName, create)
      {
        DirectMode = true
      };
    else
      compoundFile = (ICompoundFile) new Syncfusion.CompoundFile.XlsIO.Native.CompoundFile(fileName, storageOptions);
    return compoundFile;
  }

  internal ICompoundFile CreateCompoundFile()
  {
    return this.m_bNetStorage ? (ICompoundFile) new Syncfusion.CompoundFile.XlsIO.Net.CompoundFile() : (ICompoundFile) new Syncfusion.CompoundFile.XlsIO.Native.CompoundFile();
  }

  public static Image CreateImage(Stream stream) => Image.FromStream(stream, true, false);

  internal TextBoxShapeImpl CreateTextBoxShapeImpl(
    ShapesCollection shapesCollection,
    WorksheetImpl sheet)
  {
    return new TextBoxShapeImpl((IApplication) this, (object) shapesCollection, sheet);
  }

  public CheckBoxShapeImpl CreateCheckBoxShapeImpl(object shapesCollection)
  {
    return new CheckBoxShapeImpl((IApplication) this, shapesCollection);
  }

  public OptionButtonShapeImpl CreateOptionButtonShapeImpl(object shapesCollection)
  {
    return new OptionButtonShapeImpl((IApplication) this, shapesCollection);
  }

  public ComboBoxShapeImpl CreateComboBoxShapeImpl(object shapesCollection)
  {
    return new ComboBoxShapeImpl((IApplication) this, shapesCollection);
  }

  public virtual Stream CreateCompressor(Stream outputStream)
  {
    return this.m_compressionLevel.HasValue ? (Stream) new NetCompressor(this.m_compressionLevel.HasValue ? this.m_compressionLevel.Value : Syncfusion.Compression.CompressionLevel.Normal, outputStream) : (Stream) new DeflateStream(outputStream, CompressionMode.Compress, true);
  }

  internal CultureInfo CheckAndApplySeperators()
  {
    if (!this.IsFormulaParsed)
      return this.m_standredCulture;
    return !(this.m_currentCulture.Name == CultureInfo.CurrentCulture.Name) ? CultureInfo.CurrentCulture : this.GetCultureInfo(this.m_currentCulture, CultureInfo.CurrentCulture);
  }

  internal CultureInfo GetCultureInfo(CultureInfo oldCulture, CultureInfo newCulture)
  {
    return this.m_isChangeSeparator || oldCulture.NumberFormat.NumberDecimalSeparator == newCulture.NumberFormat.NumberDecimalSeparator && oldCulture.NumberFormat.PercentDecimalSeparator == newCulture.NumberFormat.PercentDecimalSeparator && oldCulture.NumberFormat.CurrencyDecimalSeparator == newCulture.NumberFormat.CurrencyDecimalSeparator && oldCulture.NumberFormat.NumberGroupSeparator == newCulture.NumberFormat.NumberGroupSeparator && oldCulture.NumberFormat.PercentGroupSeparator == newCulture.NumberFormat.PercentGroupSeparator && oldCulture.NumberFormat.CurrencyGroupSeparator == newCulture.NumberFormat.CurrencyGroupSeparator && oldCulture.TextInfo.ListSeparator == newCulture.TextInfo.ListSeparator ? oldCulture : newCulture;
  }

  internal void SetNumberDecimalSeparator(
    NumberFormatInfo numberFormat,
    string numberDecimalSeparator)
  {
    numberFormat.NumberDecimalSeparator = numberDecimalSeparator;
    numberFormat.PercentDecimalSeparator = numberDecimalSeparator;
    numberFormat.CurrencyDecimalSeparator = numberDecimalSeparator;
  }

  internal void SetThousandsSeparator(NumberFormatInfo numberFormat, string thousandsSeparator)
  {
    numberFormat.NumberGroupSeparator = thousandsSeparator;
    numberFormat.CurrencyGroupSeparator = thousandsSeparator;
    numberFormat.PercentGroupSeparator = thousandsSeparator;
  }

  public void SetActiveWorkbook(IWorkbook book) => this.m_ActiveBook = book;

  public void SetActiveWorksheet(WorksheetBaseImpl sheet)
  {
    this.m_ActiveSheet = sheet;
    this.m_ActiveTabSheet = (ITabSheet) sheet;
  }

  public void SetActiveCell(IRange cell) => this.m_ActiveCell = cell;

  internal static void UpdateDpiScales(float dpiValue)
  {
    ApplicationImpl.s_arrProportions = new double[8]
    {
      (double) dpiValue / 75.0,
      (double) dpiValue / 300.0,
      (double) dpiValue,
      (double) dpiValue / 25.4,
      (double) dpiValue / 2.54,
      1.0,
      (double) dpiValue / 72.0,
      (double) dpiValue / 72.0 / 12700.0
    };
  }

  internal static double ConvertToPixels(double value, MeasureUnits from)
  {
    return value * ApplicationImpl.s_arrProportions[(int) from];
  }

  internal static double ConvertFromPixel(double value, MeasureUnits to)
  {
    return value / ApplicationImpl.s_arrProportions[(int) to];
  }

  public static double ConvertUnitsStatic(double value, MeasureUnits from, MeasureUnits to)
  {
    return value * ApplicationImpl.s_arrProportions[(int) from] / ApplicationImpl.s_arrProportions[(int) to];
  }

  public double ConvertUnits(double value, MeasureUnits from, MeasureUnits to)
  {
    return from != to ? value * ApplicationImpl.s_arrProportions[(int) from] / ApplicationImpl.s_arrProportions[(int) to] : value;
  }

  public void RaiseProgressEvent(long curPos, long fullSize)
  {
    if (this.ProgressEvent == null)
      return;
    this.ProgressEvent((object) this, new ProgressEventArgs(curPos, fullSize));
  }

  internal void RaiseTypeMismatchOnExportEvent(object sender, ExportEventArgs args)
  {
    if (this.TypeMismatchOnExport == null)
      return;
    this.TypeMismatchOnExport(sender, args);
  }

  public SizeF MeasureString(string strToMeasure, FontImpl font, SizeF rectSize)
  {
    StringFormat stringFormat = new StringFormat(StringFormatFlags.NoWrap);
    lock (this.m_graphics)
      return this.m_graphics.MeasureString(strToMeasure, font.GenerateNativeFont(), rectSize, stringFormat);
  }

  internal bool RaiseOnPasswordRequired(object sender, PasswordRequiredEventArgs e)
  {
    bool flag = false;
    if (this.OnPasswordRequired != null)
    {
      this.OnPasswordRequired(sender, e);
      flag = true;
    }
    return flag;
  }

  internal bool RaiseOnWrongPassword(object sender, PasswordRequiredEventArgs e)
  {
    bool flag = false;
    if (this.OnWrongPassword != null)
    {
      this.OnWrongPassword(sender, e);
      flag = true;
    }
    return flag;
  }

  public event ProgressEventHandler ProgressEvent;

  public event PasswordRequiredEventHandler OnPasswordRequired;

  public event PasswordRequiredEventHandler OnWrongPassword;

  public event SubstituteFontEventHandler SubstituteFont;

  public event ExportEventHandler TypeMismatchOnExport;

  internal void Dispose()
  {
    if (this.ActiveSheet is WorksheetImpl activeSheet && activeSheet.Parent != null)
    {
      foreach (IWorksheet inner in ((CollectionBase<IWorksheet>) activeSheet.Parent).InnerList)
        (inner as WorksheetImpl).ClearAllData();
      if (activeSheet != null && activeSheet.ParentWorkbook != null)
        activeSheet.ParentWorkbook.DisposeAll();
    }
    if (this.m_graphics != null)
      this.m_graphics.Dispose();
    this.m_defaultStyleNames = (string[]) null;
    this.m_workbooks = (WorkbooksCollection) null;
    this.RemoveStylesCollection();
    this.RemovePageSetupCollection();
  }

  internal void RemoveStylesCollection()
  {
    int length = this.m_builtInStyleInfo.Length;
    for (int index = 0; index < length; ++index)
    {
      this.m_builtInStyleInfo[index].Clear();
      this.m_builtInStyleInfo[index] = (StyleImpl.StyleSettings) null;
    }
    this.m_builtInStyleInfo = (StyleImpl.StyleSettings[]) null;
  }

  internal object GetParent() => this.m_parent;

  internal void RemovePageSetupCollection()
  {
    this.m_dicPaperSizeTable.Clear();
    this.m_dicPaperSizeTable = (Dictionary<int, PageSetupBaseImpl.PaperSizeEntry>) null;
  }

  internal int GetFontCalc1() => 182;

  internal int GetFontCalc2() => 7;

  internal int GetFontCalc3() => 5;

  internal int GetdpiX() => this.dpiX;

  internal int GetdpiY() => this.dpiY;

  internal bool HasDynamicOverrideMethods(Type type)
  {
    string[] strArray = new string[3]
    {
      "TrySetMember",
      "TryGetMember",
      "GetDynamicMemberNames"
    };
    int num = 0;
    for (int index = 0; index < strArray.Length; ++index)
    {
      if (!(type.GetMethod(strArray[index]).DeclaringType == type))
        throw new NotImplementedException("Method is not overrided");
      ++num;
    }
    return num == 3;
  }

  internal bool IsValidImage(byte[] bytes)
  {
    byte[] header1 = new byte[2]{ (byte) 66, (byte) 77 };
    byte[] header2 = new byte[3]
    {
      (byte) 71,
      (byte) 73,
      (byte) 70
    };
    byte[] header3 = new byte[4]
    {
      (byte) 137,
      (byte) 80 /*0x50*/,
      (byte) 78,
      (byte) 71
    };
    byte[] header4 = new byte[3]
    {
      (byte) 73,
      (byte) 73,
      (byte) 42
    };
    byte[] header5 = new byte[3]
    {
      (byte) 77,
      (byte) 77,
      (byte) 42
    };
    byte[] header6 = new byte[4]
    {
      byte.MaxValue,
      (byte) 216,
      byte.MaxValue,
      (byte) 224 /*0xE0*/
    };
    byte[] header7 = new byte[4]
    {
      byte.MaxValue,
      (byte) 216,
      byte.MaxValue,
      (byte) 225
    };
    return this.CheckImageHeader(header1, bytes) || this.CheckImageHeader(header2, bytes) || this.CheckImageHeader(header3, bytes) || this.CheckImageHeader(header4, bytes) || this.CheckImageHeader(header5, bytes) || this.CheckImageHeader(header6, bytes) || this.CheckImageHeader(header7, bytes);
  }

  internal bool CheckImageHeader(byte[] header, byte[] imageBytes)
  {
    if (imageBytes.Length <= header.Length)
      return false;
    for (int index = 0; index < header.Length; ++index)
    {
      if ((int) header[index] != (int) imageBytes[index])
        return false;
    }
    return true;
  }

  internal string TryGetFontStream(
    string fontName,
    float fontSize,
    FontStyle fontStyle,
    out Stream stream)
  {
    stream = (Stream) null;
    if (this.SubstituteFont != null)
    {
      string empty = string.Empty;
      Font font1 = this.CreateFont(fontName, fontSize, fontStyle);
      if (font1.Name != fontName)
      {
        SubstituteFontEventArgs args = new SubstituteFontEventArgs(fontName, "Microsoft Sans Serif");
        while (this.SubstituteFont != null)
        {
          string alternateFontName = args.AlternateFontName;
          this.SubstituteFont((object) this, args);
          if (args.AlternateFontStream != null && args.AlternateFontStream.Length > 0L)
          {
            stream = args.AlternateFontStream;
            stream.Position = 0L;
            return fontName;
          }
          if (!string.IsNullOrEmpty(args.AlternateFontName))
          {
            Font font2 = this.CreateFont(args.AlternateFontName, fontSize, fontStyle);
            string name = font2.Name;
            if (name == args.AlternateFontName || alternateFontName != "Microsoft Sans Serif" || alternateFontName == args.AlternateFontName)
            {
              fontName = name;
              font2.Dispose();
              break;
            }
          }
        }
      }
      else
        font1.Dispose();
    }
    return fontName;
  }

  private Font CreateFont(string fontName, float fontSize, FontStyle fontStyle)
  {
    try
    {
      return new Font(fontName, fontSize, fontStyle);
    }
    catch
    {
      FontFamily fontFamily = new FontFamily(fontName);
      if (fontFamily.IsStyleAvailable(FontStyle.Bold))
        fontStyle |= FontStyle.Bold;
      if (fontFamily.IsStyleAvailable(FontStyle.Italic))
        fontStyle |= FontStyle.Italic;
      if (fontFamily.IsStyleAvailable(FontStyle.Underline))
        fontStyle |= FontStyle.Underline;
      if (fontFamily.IsStyleAvailable(FontStyle.Strikeout))
        fontStyle |= FontStyle.Strikeout;
      return new Font(fontName, fontSize, fontStyle);
    }
  }
}
