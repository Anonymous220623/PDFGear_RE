// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.ApplicationImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.Compression.Zip;
using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Implementation.Clipboard;
using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
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
namespace Syncfusion.OfficeChart.Implementation;

internal class ApplicationImpl : IApplication, IParentApplication
{
  private const double DEF_ZERO_CHAR_WIDTH = 8.0;
  private const int DEF_BUILD_NUMBER = 0;
  private const double DEF_STANDARD_FONT_SIZE = 10.0;
  private const int DEF_FIXED_DECIMAL_PLACES = 4;
  private const int DEF_SHEETS_IN_NEW_WORKBOOK = 3;
  private const string DEF_DEFAULT_FONT = "Arial";
  private const string DEF_VALUE = "Microsoft Excel";
  private const string DEF_SWITCH_NAME = "Syncfusion.OfficeChart.DebugInfo";
  private const string DEF_SWITCH_DESCRIPTION = "Indicates wether to show library debug messages.";
  public const char DEF_ARGUMENT_SEPARATOR = ',';
  public const char DEF_ROW_SEPARATOR = ';';
  private const byte DEF_BIFF_HEADER_SIZE = 8;
  private const string DEF_XML_HEADER = "<?xml";
  private const string DEF_HTML_HEADER = "<html";
  private const int DEF_BUFFER_SIZE = 512 /*0x0200*/;
  private const int DefaultRowHeightXlsx = 300;
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
  private static readonly double[] s_arrProportions;
  internal static readonly SizeF MinCellSize = new SizeF(8.43f, 12.75f);
  private readonly Graphics m_graphics;
  private static readonly BooleanSwitch m_switch = new BooleanSwitch("Syncfusion.OfficeChart.DebugInfo", "Indicates wether to show library debug messages.");
  private static readonly bool m_bDebugMessage = ApplicationImpl.m_switch.Enabled;
  internal static Type[] AssemblyTypes = Assembly.GetExecutingAssembly().GetTypes();
  private object m_parent;
  private static bool m_bIsDebugInfoEnabled = false;
  private StringEnumerations m_stringEnum = new StringEnumerations();
  private static OfficeDataProviderType m_dataType = OfficeDataProviderType.ByteArray;
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
  private IWorkbook m_ActiveBook;
  private WorkbooksCollection m_workbooks;
  private bool m_bFixedDecimal;
  private bool m_bUseSystemSep;
  private double m_dbStandardFontSize;
  private int m_iFixedDecimalPlaces;
  private int m_iSheetsInNewWorkbook;
  private string m_strDecimalSeparator;
  private string m_strStandardFont;
  private string m_strThousandsSeparator;
  private string m_strUserName;
  private bool m_bChangeStyle;
  private OfficeSkipExtRecords m_enSkipExtRecords;
  private int m_iStandardRowHeight = (int) byte.MaxValue;
  private bool m_bStandartRowHeightFlag;
  private double m_dStandardColWidth = 8.43;
  private bool m_bOptimizeFonts;
  private bool m_bOptimizeImport;
  private char m_chRowSeparator = ';';
  private char m_chArgumentSeparator = ',';
  private string m_strCSVSeparator = ",";
  private bool m_bUseFastRecordParsing;
  private int m_iRowStorageBlock = 128 /*0x80*/;
  private bool m_bDeleteDestinationFile = true;
  private CultureInfo m_standredCulture;
  private CultureInfo m_currentCulture;
  private readonly Graphics m_autoFilterManagerGraphics;
  private OfficeVersion m_defaultVersion = OfficeVersion.Excel2013;
  private bool m_bNetStorage = true;
  private bool m_bEvalExpired;
  private bool m_bIsDefaultFontChanged;
  private Syncfusion.Compression.CompressionLevel? m_compressionLevel;
  private bool m_preserveTypes;
  private bool m_isFormulaparsed = true;
  private StyleImpl.StyleSettings[] m_builtInStyleInfo;
  private IOfficeChartToImageConverter m_chartToImageConverter;
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
    get => ApplicationImpl.m_dataType == OfficeDataProviderType.Unsafe;
    set
    {
      if (value)
        ApplicationImpl.m_dataType = OfficeDataProviderType.Unsafe;
      else
        ApplicationImpl.m_dataType = OfficeDataProviderType.Native;
    }
  }

  public static OfficeDataProviderType DataProviderTypeStatic
  {
    get => ApplicationImpl.m_dataType;
    set => ApplicationImpl.m_dataType = value;
  }

  public bool IsSaved
  {
    get
    {
      IWorkbooks workbooks = this.Workbooks;
      int Index = 0;
      for (int count = workbooks.Count; Index < count; ++Index)
      {
        if (!workbooks[Index].Saved)
          return false;
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

  public IRange ActiveCell => this.m_ActiveCell;

  public IWorksheet ActiveSheet => this.m_ActiveSheet as IWorksheet;

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
      this.m_currentCulture.NumberFormat.NumberDecimalSeparator = this.m_strDecimalSeparator;
      this.m_currentCulture.NumberFormat.PercentDecimalSeparator = this.m_strDecimalSeparator;
      this.m_currentCulture.NumberFormat.CurrencyDecimalSeparator = this.m_strDecimalSeparator;
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
      this.m_currentCulture.NumberFormat.NumberGroupSeparator = this.m_strThousandsSeparator;
      this.m_currentCulture.NumberFormat.PercentGroupSeparator = this.m_strThousandsSeparator;
      this.m_currentCulture.NumberFormat.CurrencyGroupSeparator = this.m_strThousandsSeparator;
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

  public OfficeSkipExtRecords SkipOnSave
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
      this.m_currentCulture.TextInfo.ListSeparator = Convert.ToString(this.m_chArgumentSeparator);
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

  public OfficeVersion DefaultVersion
  {
    get => this.m_defaultVersion;
    set
    {
      this.m_defaultVersion = value;
      if (!this.m_bIsDefaultFontChanged)
        this.CheckDefaultFont();
      if (value == OfficeVersion.Excel97to2003)
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

  public OfficeDataProviderType DataProviderType
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

  internal StringEnumerations StringEnum => this.m_stringEnum;

  public IOfficeChartToImageConverter ChartToImageConverter
  {
    get => this.m_chartToImageConverter;
    set => this.m_chartToImageConverter = value;
  }

  static ApplicationImpl()
  {
    Bitmap bitmap = new Bitmap(1, 1);
    bitmap.SetResolution(96f, 96f);
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
  }

  public ApplicationImpl(ExcelEngine excelEngine)
  {
    this.m_parent = (object) excelEngine;
    if (ExcelEngine.IsSecurityGranted)
      this.m_strUserName = Environment.UserName;
    this.m_graphics = Graphics.FromImage((Image) new Bitmap(1, 1));
    this.dpiX = (int) this.m_graphics.DpiX;
    this.dpiY = (int) this.m_graphics.DpiY;
    this.m_builtInStyleInfo = new StyleImpl.StyleSettings[this.m_defaultStyleNames.Length];
    this.m_dbStandardFontSize = 10.0;
    this.m_iFixedDecimalPlaces = 4;
    this.m_iSheetsInNewWorkbook = 3;
    this.m_standredCulture = CultureInfo.InvariantCulture;
    this.m_currentCulture = new CultureInfo(CultureInfo.CurrentCulture.Name);
    this.m_strDecimalSeparator = this.m_currentCulture.NumberFormat.NumberDecimalSeparator;
    this.m_strStandardFont = "Tahoma";
    this.m_strThousandsSeparator = this.m_currentCulture.NumberFormat.NumberGroupSeparator;
    this.m_chArgumentSeparator = Convert.ToChar(this.m_currentCulture.TextInfo.ListSeparator);
    this.m_graphics.PageUnit = GraphicsUnit.Pixel;
    this.InitializeCollection();
    this.InitializeStyleCollections();
    this.InitializePageSetup();
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
    FillImpl fill1 = new FillImpl(OfficePattern.Solid, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 204), ColorExtension.Empty);
    StyleImpl.FontSettings font1 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 1));
    StyleImpl.BorderSettings borders1 = new StyleImpl.BorderSettings((ChartColor) Color.FromArgb((int) byte.MaxValue, 178, 178, 178), OfficeLineStyle.Thin);
    this.BuiltInStyleInfo[index11] = new StyleImpl.StyleSettings(fill1, font1, borders1);
    int index12 = index11 + 1;
    StyleImpl.FontSettings font2 = new StyleImpl.FontSettings((ChartColor) Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0, 0));
    this.BuiltInStyleInfo[index12] = new StyleImpl.StyleSettings((FillImpl) null, font2);
    int index13 = index12 + 1;
    this.BuiltInStyleInfo[index13] = new StyleImpl.StyleSettings((FillImpl) null, (StyleImpl.FontSettings) null);
    int index14 = index13 + 1;
    this.BuiltInStyleInfo[index14] = new StyleImpl.StyleSettings((FillImpl) null, (StyleImpl.FontSettings) null);
    int index15 = index14 + 1;
    this.BuiltInStyleInfo[index15] = new StyleImpl.StyleSettings((FillImpl) null, (StyleImpl.FontSettings) null);
    int index16 = index15 + 1;
    StyleImpl.FontSettings font3 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 3), 18, FontStyle.Bold, "Cambria");
    this.BuiltInStyleInfo[index16] = new StyleImpl.StyleSettings((FillImpl) null, font3);
    int index17 = index16 + 1;
    StyleImpl.FontSettings font4 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 3), 15, FontStyle.Bold);
    StyleImpl.BorderSettings borders2 = new StyleImpl.BorderSettings(new ChartColor(ColorType.Theme, 4), OfficeLineStyle.None, OfficeLineStyle.None, OfficeLineStyle.None, OfficeLineStyle.Thick);
    this.BuiltInStyleInfo[index17] = new StyleImpl.StyleSettings((FillImpl) null, font4, borders2);
    int index18 = index17 + 1;
    StyleImpl.FontSettings font5 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 3), 13, FontStyle.Bold);
    StyleImpl.BorderSettings borders3 = new StyleImpl.BorderSettings(new ChartColor(ColorType.Theme, 4, 0.499984740745262), OfficeLineStyle.None, OfficeLineStyle.None, OfficeLineStyle.None, OfficeLineStyle.Thick);
    this.BuiltInStyleInfo[index18] = new StyleImpl.StyleSettings((FillImpl) null, font5, borders3);
    int index19 = index18 + 1;
    this.BuiltInStyleInfo[index19] = new StyleImpl.StyleSettings((FillImpl) null, new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 3), FontStyle.Bold), new StyleImpl.BorderSettings(new ChartColor(ColorType.Theme, 4, 0.39997558519241921), OfficeLineStyle.None, OfficeLineStyle.None, OfficeLineStyle.None, OfficeLineStyle.Medium));
    int index20 = index19 + 1;
    StyleImpl.FontSettings font6 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 3), FontStyle.Bold);
    this.BuiltInStyleInfo[index20] = new StyleImpl.StyleSettings((FillImpl) null, font6);
    int index21 = index20 + 1;
    FillImpl fill2 = new FillImpl(OfficePattern.Solid, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 204, 153), ColorExtension.Empty);
    StyleImpl.FontSettings font7 = new StyleImpl.FontSettings((ChartColor) Color.FromArgb((int) byte.MaxValue, 63 /*0x3F*/, 63 /*0x3F*/, 118));
    StyleImpl.BorderSettings borders4 = new StyleImpl.BorderSettings((ChartColor) Color.FromArgb((int) byte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue), OfficeLineStyle.Thin);
    this.BuiltInStyleInfo[index21] = new StyleImpl.StyleSettings(fill2, font7, borders4);
    int index22 = index21 + 1;
    FillImpl fill3 = new FillImpl(OfficePattern.Solid, Color.FromArgb((int) byte.MaxValue, 242, 242, 242), ColorExtension.Empty);
    StyleImpl.FontSettings font8 = new StyleImpl.FontSettings((ChartColor) Color.FromArgb((int) byte.MaxValue, 63 /*0x3F*/, 63 /*0x3F*/, 63 /*0x3F*/), FontStyle.Bold);
    StyleImpl.BorderSettings borders5 = new StyleImpl.BorderSettings((ChartColor) Color.FromArgb((int) byte.MaxValue, 63 /*0x3F*/, 63 /*0x3F*/, 63 /*0x3F*/), OfficeLineStyle.Thin);
    this.BuiltInStyleInfo[index22] = new StyleImpl.StyleSettings(fill3, font8, borders5);
    int index23 = index22 + 1;
    FillImpl fill4 = new FillImpl(OfficePattern.Solid, Color.FromArgb((int) byte.MaxValue, 242, 242, 242), ColorExtension.Empty);
    StyleImpl.FontSettings font9 = new StyleImpl.FontSettings((ChartColor) Color.FromArgb((int) byte.MaxValue, 250, 125, 0), FontStyle.Bold);
    StyleImpl.BorderSettings borders6 = new StyleImpl.BorderSettings((ChartColor) Color.FromArgb((int) byte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue), OfficeLineStyle.Thin);
    this.BuiltInStyleInfo[index23] = new StyleImpl.StyleSettings(fill4, font9, borders6);
    int index24 = index23 + 1;
    FillImpl fill5 = new FillImpl(OfficePattern.Solid, Color.FromArgb((int) byte.MaxValue, 165, 165, 165), ColorExtension.Empty);
    StyleImpl.FontSettings font10 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 0), FontStyle.Bold);
    StyleImpl.BorderSettings borders7 = new StyleImpl.BorderSettings((ChartColor) Color.FromArgb((int) byte.MaxValue, 63 /*0x3F*/, 63 /*0x3F*/, 63 /*0x3F*/), OfficeLineStyle.Double);
    this.BuiltInStyleInfo[index24] = new StyleImpl.StyleSettings(fill5, font10, borders7);
    int index25 = index24 + 1;
    StyleImpl.FontSettings font11 = new StyleImpl.FontSettings((ChartColor) Color.FromArgb((int) byte.MaxValue, 250, 125, 0));
    StyleImpl.BorderSettings borders8 = new StyleImpl.BorderSettings((ChartColor) Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 128 /*0x80*/, 1), OfficeLineStyle.None, OfficeLineStyle.None, OfficeLineStyle.None, OfficeLineStyle.Double);
    this.BuiltInStyleInfo[index25] = new StyleImpl.StyleSettings((FillImpl) null, font11, borders8);
    int index26 = index25 + 1;
    StyleImpl.FontSettings font12 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 1), FontStyle.Bold);
    StyleImpl.BorderSettings borders9 = new StyleImpl.BorderSettings(new ChartColor(ColorType.Theme, 4), OfficeLineStyle.None, OfficeLineStyle.None, OfficeLineStyle.Thin, OfficeLineStyle.Double);
    this.BuiltInStyleInfo[index26] = new StyleImpl.StyleSettings((FillImpl) null, font12, borders9);
    int index27 = index26 + 1;
    FillImpl fill6 = new FillImpl(OfficePattern.Solid, Color.FromArgb((int) byte.MaxValue, 198, 239, 206), ColorExtension.Empty);
    StyleImpl.FontSettings font13 = new StyleImpl.FontSettings((ChartColor) Color.FromArgb((int) byte.MaxValue, 0, 97, 0));
    this.BuiltInStyleInfo[index27] = new StyleImpl.StyleSettings(fill6, font13);
    int index28 = index27 + 1;
    FillImpl fill7 = new FillImpl(OfficePattern.Solid, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 199, 206), ColorExtension.Empty);
    StyleImpl.FontSettings font14 = new StyleImpl.FontSettings((ChartColor) Color.FromArgb((int) byte.MaxValue, 156, 0, 6));
    this.BuiltInStyleInfo[index28] = new StyleImpl.StyleSettings(fill7, font14);
    int index29 = index28 + 1;
    FillImpl fill8 = new FillImpl(OfficePattern.Solid, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 235, 156), ColorExtension.Empty);
    StyleImpl.FontSettings font15 = new StyleImpl.FontSettings((ChartColor) Color.FromArgb((int) byte.MaxValue, 156, 101, 0));
    this.BuiltInStyleInfo[index29] = new StyleImpl.StyleSettings(fill8, font15);
    int index30 = index29 + 1;
    FillImpl fill9 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 4), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font16 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index30] = new StyleImpl.StyleSettings(fill9, font16);
    int index31 = index30 + 1;
    FillImpl fill10 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 4, 0.79998168889431442), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font17 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index31] = new StyleImpl.StyleSettings(fill10, font17);
    int index32 = index31 + 1;
    FillImpl fill11 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 4, 0.59999389629810485), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font18 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index32] = new StyleImpl.StyleSettings(fill11, font18);
    int index33 = index32 + 1;
    FillImpl fill12 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 4, 0.39997558519241921), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font19 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index33] = new StyleImpl.StyleSettings(fill12, font19);
    int index34 = index33 + 1;
    FillImpl fill13 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 5), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font20 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index34] = new StyleImpl.StyleSettings(fill13, font20);
    int index35 = index34 + 1;
    FillImpl fill14 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 5, 0.79998168889431442), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font21 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index35] = new StyleImpl.StyleSettings(fill14, font21);
    int index36 = index35 + 1;
    FillImpl fill15 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 5, 0.59999389629810485), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font22 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index36] = new StyleImpl.StyleSettings(fill15, font22);
    int index37 = index36 + 1;
    FillImpl fill16 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 5, 0.39997558519241921), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font23 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index37] = new StyleImpl.StyleSettings(fill16, font23);
    int index38 = index37 + 1;
    FillImpl fill17 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 6), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font24 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index38] = new StyleImpl.StyleSettings(fill17, font24);
    int index39 = index38 + 1;
    FillImpl fill18 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 6, 0.79998168889431442), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font25 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index39] = new StyleImpl.StyleSettings(fill18, font25);
    int index40 = index39 + 1;
    FillImpl fill19 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 6, 0.59999389629810485), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font26 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index40] = new StyleImpl.StyleSettings(fill19, font26);
    int index41 = index40 + 1;
    FillImpl fill20 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 6, 0.39997558519241921), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font27 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index41] = new StyleImpl.StyleSettings(fill20, font27);
    int index42 = index41 + 1;
    FillImpl fill21 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 7), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font28 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index42] = new StyleImpl.StyleSettings(fill21, font28);
    int index43 = index42 + 1;
    FillImpl fill22 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 7, 0.79998168889431442), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font29 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index43] = new StyleImpl.StyleSettings(fill22, font29);
    int index44 = index43 + 1;
    FillImpl fill23 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 7, 0.59999389629810485), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font30 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index44] = new StyleImpl.StyleSettings(fill23, font30);
    int index45 = index44 + 1;
    FillImpl fill24 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 7, 0.39997558519241921), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font31 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index45] = new StyleImpl.StyleSettings(fill24, font31);
    int index46 = index45 + 1;
    FillImpl fill25 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 8), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font32 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index46] = new StyleImpl.StyleSettings(fill25, font32);
    int index47 = index46 + 1;
    FillImpl fill26 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 8, 0.79998168889431442), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font33 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index47] = new StyleImpl.StyleSettings(fill26, font33);
    int index48 = index47 + 1;
    FillImpl fill27 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 8, 0.59999389629810485), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font34 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index48] = new StyleImpl.StyleSettings(fill27, font34);
    int index49 = index48 + 1;
    FillImpl fill28 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 8, 0.39997558519241921), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font35 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index49] = new StyleImpl.StyleSettings(fill28, font35);
    int index50 = index49 + 1;
    FillImpl fill29 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 9), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font36 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index50] = new StyleImpl.StyleSettings(fill29, font36);
    int index51 = index50 + 1;
    FillImpl fill30 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 9, 0.79998168889431442), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font37 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index51] = new StyleImpl.StyleSettings(fill30, font37);
    int index52 = index51 + 1;
    FillImpl fill31 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 9, 0.59999389629810485), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font38 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 1));
    this.BuiltInStyleInfo[index52] = new StyleImpl.StyleSettings(fill31, font38);
    int index53 = index52 + 1;
    FillImpl fill32 = new FillImpl(OfficePattern.Solid, new ChartColor(ColorType.Theme, 9, 0.39997558519241921), (ChartColor) ColorExtension.Empty);
    StyleImpl.FontSettings font39 = new StyleImpl.FontSettings(new ChartColor(ColorType.Theme, 0));
    this.BuiltInStyleInfo[index53] = new StyleImpl.StyleSettings(fill32, font39);
    int index54 = index53 + 1;
    StyleImpl.FontSettings font40 = new StyleImpl.FontSettings((ChartColor) Color.FromArgb((int) byte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue), FontStyle.Italic);
    this.BuiltInStyleInfo[index54] = new StyleImpl.StyleSettings((FillImpl) null, font40);
    int num = index54 + 1;
  }

  protected void InitializeCollection()
  {
    this.m_workbooks = new WorkbooksCollection(this.Application, (object) this);
  }

  public double CentimetersToPoints(double Centimeters)
  {
    return this.ConvertUnits(Centimeters, MeasureUnits.Centimeter, MeasureUnits.Point);
  }

  public bool IsSupported(string FilePath)
  {
    if (File.Exists(FilePath))
    {
      FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
      if (fileStream != null)
        return this.IsSupported((Stream) fileStream);
    }
    return false;
  }

  public bool IsSupported(Stream stream) => true;

  public OfficeOpenType DetectFileFromStream(Stream stream)
  {
    OfficeOpenType officeOpenType = OfficeOpenType.Automatic;
    long num1 = stream != null ? stream.Position : throw new ArgumentNullException(nameof (stream));
    if (ZipArchive.ReadInt32(stream) == 67324752)
    {
      stream.Position = num1;
      officeOpenType = OfficeOpenType.SpreadsheetML2007;
    }
    else
    {
      stream.Position = num1;
      byte[] buffer = new byte[512 /*0x0200*/];
      int num2 = stream.Read(buffer, 0, 512 /*0x0200*/);
      if (num2 != 0)
      {
        if (num2 >= 8)
        {
          int index = 0;
          while (index < 8 && (int) ApplicationImpl.DEF_XLS_FILE_HEADER[index] == (int) buffer[index])
            ++index;
        }
        stream.Position = num1;
      }
    }
    return officeOpenType;
  }

  private OfficeOpenType DetectIsCSVOrXML(Stream stream, MemoryStream memoryStream, long lPosition)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (memoryStream == null)
      throw new ArgumentNullException(nameof (memoryStream));
    string csvSeparator = this.Application.CSVSeparator;
    bool bIsCompare = this.IsContainSurrogate(csvSeparator, "", false);
    bool flag = false;
    StreamReader streamReader = new StreamReader((Stream) memoryStream, true);
    string str = streamReader.ReadLine();
    Encoding currentEncoding = streamReader.CurrentEncoding;
    OfficeOpenType officeOpenType = OfficeOpenType.Automatic;
    for (; str != null; str = streamReader.ReadLine())
    {
      string lower = str.ToLower();
      int num = lower.IndexOf("<?xml");
      if (num != -1)
      {
        stream.Position = lPosition + (long) num;
        officeOpenType = OfficeOpenType.SpreadsheetML;
        break;
      }
      if (lower.IndexOf("<html") == -1)
      {
        if (!flag)
          flag = this.IsContainSurrogate(lower, csvSeparator, bIsCompare);
        lPosition += (long) currentEncoding.GetByteCount(lower);
        lPosition += (long) (currentEncoding.GetByteCount("\n") * 2);
      }
      else
        break;
    }
    if (officeOpenType == OfficeOpenType.Automatic && !flag)
      officeOpenType = OfficeOpenType.CSV;
    return !flag ? officeOpenType : OfficeOpenType.Automatic;
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

  public virtual WorkbookImpl CreateWorkbook(object parent, OfficeVersion version)
  {
    return new WorkbookImpl((IApplication) this, parent, version);
  }

  public virtual WorkbookImpl CreateWorkbook(
    object parent,
    Stream stream,
    string separator,
    int row,
    int column,
    OfficeVersion version,
    string fileName,
    Encoding encoding)
  {
    return new WorkbookImpl((IApplication) this, parent, stream, separator, row, column, version, fileName, encoding);
  }

  public virtual WorkbookImpl CreateWorkbook(
    object parent,
    Stream stream,
    OfficeVersion version,
    OfficeParseOptions options)
  {
    return new WorkbookImpl((IApplication) this, parent, stream, options, version);
  }

  public virtual WorkbookImpl CreateWorkbook(
    object parent,
    Stream stream,
    OfficeParseOptions options,
    OfficeVersion version)
  {
    return new WorkbookImpl((IApplication) this, parent, stream, options, version);
  }

  public virtual WorkbookImpl CreateWorkbook(
    object parent,
    string strTemplateFile,
    OfficeParseOptions options,
    bool bReadOnly,
    string password,
    OfficeVersion version)
  {
    return new WorkbookImpl((IApplication) this, parent, strTemplateFile, options, bReadOnly, password, version);
  }

  public virtual WorkbookImpl CreateWorkbook(
    object parent,
    Stream stream,
    OfficeParseOptions options,
    bool bReadOnly,
    string password,
    OfficeVersion version)
  {
    return new WorkbookImpl((IApplication) this, parent, stream, options, bReadOnly, password, version);
  }

  public virtual WorkbookImpl CreateWorkbook(
    object parent,
    int sheetsQuantity,
    OfficeVersion version)
  {
    return new WorkbookImpl((IApplication) this, parent, sheetsQuantity, version);
  }

  public virtual WorkbookImpl CreateWorkbook(
    object parent,
    string strTemplateFile,
    OfficeVersion version)
  {
    return new WorkbookImpl((IApplication) this, parent, strTemplateFile, version);
  }

  public virtual WorkbookImpl CreateWorkbook(
    object parent,
    string strTemplateFile,
    OfficeParseOptions options,
    OfficeVersion version)
  {
    return new WorkbookImpl((IApplication) this, parent, strTemplateFile, options, version);
  }

  public virtual WorksheetImpl CreateWorksheet(object parent)
  {
    return new WorksheetImpl((IApplication) this, parent);
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

  public virtual FontImpl CreateFont(IOfficeFont basedOn) => new FontImpl(basedOn);

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
    this.StandardFont = "Calibri";
    this.StandardFontSize = 11.0;
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

  public static DataProvider CreateDataProvider(IntPtr heapHandle)
  {
    return (DataProvider) new ByteArrayDataProvider();
  }

  internal static DataProvider CreateDataProvider() => (DataProvider) new ByteArrayDataProvider();

  public static Image CreateImage(Stream stream) => Image.FromStream(stream, true, true);

  internal TextBoxShapeImpl CreateTextBoxShapeImpl(
    ShapesCollection shapesCollection,
    WorksheetImpl sheet)
  {
    return new TextBoxShapeImpl((IApplication) this, (object) shapesCollection, sheet);
  }

  public virtual Stream CreateCompressor(Stream outputStream)
  {
    return this.m_compressionLevel.HasValue ? (Stream) new NetCompressor(this.m_compressionLevel.HasValue ? this.m_compressionLevel.Value : Syncfusion.Compression.CompressionLevel.Normal, outputStream) : (Stream) new DeflateStream(outputStream, CompressionMode.Compress, true);
  }

  internal CultureInfo CheckAndApplySeperators()
  {
    return !this.IsFormulaParsed ? this.m_standredCulture : this.m_currentCulture;
  }

  public void SetActiveWorkbook(IWorkbook book) => this.m_ActiveBook = book;

  public void SetActiveWorksheet(WorksheetBaseImpl sheet) => this.m_ActiveSheet = sheet;

  public void SetActiveCell(IRange cell) => this.m_ActiveCell = cell;

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

  internal void Dispose()
  {
    if (this.ActiveSheet is WorksheetImpl activeSheet && activeSheet.Parent != null)
    {
      List<IWorksheet> innerList = ((CollectionBase<IWorksheet>) activeSheet.Parent).InnerList;
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
}
