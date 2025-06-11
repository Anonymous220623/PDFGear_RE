// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.WorkbookImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Calculate;
using Syncfusion.CompoundFile.XlsIO;
using Syncfusion.CompoundFile.XlsIO.Native;
using Syncfusion.CompoundFile.XlsIO.Net;
using Syncfusion.Office;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Exceptions;
using Syncfusion.XlsIO.Implementation.JsonSerialization;
using Syncfusion.XlsIO.Implementation.PivotTables;
using Syncfusion.XlsIO.Implementation.Security;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.Sorting;
using Syncfusion.XlsIO.Implementation.Vba;
using Syncfusion.XlsIO.Implementation.Xlsb;
using Syncfusion.XlsIO.Implementation.XmlReaders;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Interfaces.XmlSerialization;
using Syncfusion.XlsIO.ODSConversion;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class WorkbookImpl : CommonObject, IWorkbook, IParentApplication
{
  private const string DEF_SUMMARY_INFO = "\u0005SummaryInformation";
  private const string DEF_DOCUMENT_SUMMARY_INFO = "\u0005DocumentSummaryInformation";
  internal const string DEF_STREAM_NAME1 = "Workbook";
  internal const string DEF_STREAM_NAME2 = "Book";
  private const string DEF_VBA_MACROS = "_VBA_PROJECT_CUR";
  private const string DEF_VBA_SUB_STORAGE = "VBA";
  private const char DEF_CHAR_SELF = '\u0002';
  private const char DEF_CHAR_CODED = '\u0001';
  private const char DEF_CHAR_EMPTY = '\0';
  private const char DEF_CHAR_VOLUME = '\u0001';
  private const char DEF_CHAR_SAMEVOLUME = '\u0002';
  private const char DEF_CHAR_DOWNDIR = '\u0003';
  private const char DEF_CHAR_UPDIR = '\u0004';
  private const char DEF_CHAR_LONGVOLUME = '\u0005';
  private const char DEF_CHAR_STARTUPDIR = '\u0006';
  private const char DEF_CHAR_ALTSTARTUPDIR = '\a';
  private const char DEF_CHAR_LIBDIR = '\b';
  private const char DEF_CHAR_NETWORKPATH = '@';
  private const string DEF_NETWORKPATH_START = "\\\\";
  private const int DEF_NOT_PASSWORD_PROTECTION = 0;
  internal const int DEF_REMOVED_SHEET_INDEX = 65535 /*0xFFFF*/;
  private const string HttpStart = "http:";
  private const string NEW_LINE = "\n";
  public const int DEF_FIRST_USER_COLOR = 8;
  public const string DEF_BAD_SHEET_NAME = "#REF";
  private const int DEF_FIRST_DEFINED_FONT = 10;
  private const string DEF_RESPONSE_OPEN = "inline";
  private const string DEF_RESPONSE_DIALOG = "attachment";
  private const ushort DEF_REMOVED_INDEX = 65535 /*0xFFFF*/;
  private const string DEF_EXCEL97_CONTENT_TYPE = "Application/x-msexcel";
  private const string DEF_EXCEL2000_CONTENT_TYPE = "Application/vnd.ms-excel";
  private const string DEF_EXCEL2007_CONTENT_TYPE = "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
  private const string DEF_CSV_CONTENT_TYPE = "text/csv";
  private const string DEF_ODS_CONTENT_TYPE = "application/vnd.oasis.opendocument.spreadsheet";
  internal const string StandardPassword = "VelvetSweatshop";
  internal const char TextQualifier = '"';
  private const RegexOptions DEF_REGEX = RegexOptions.Compiled;
  private const string DEF_DIRECTORY_GROUP = "DirectoryName";
  private const string DEF_BOOK_GROUP = "BookName";
  private const string DEF_SHEET_GROUP = "SheetName";
  internal const int DEF_BOOK_SHEET_INDEX = 65534;
  private const string DEF_FORMAT_STYLE_NAME_START = "Format_";
  internal const string DEF_EXCEL_2016_THEME_VERSION = "164011";
  internal const string DEF_EXCEL_2013_THEME_VERSION = "153222";
  internal const string DEF_EXCEL_2007_THEME_VERSION = "124226";
  internal const string DEF_EXCEL_Xlsx_THEME_VERSION = "166925";
  internal const string EvaluationWarning = "Created with a trial version of Syncfusion Essential XlsIO";
  private const string EvaluationSheetName = "Evaluation expired";
  private const int FirstChartColor = 77;
  private const int LastChartColor = 79;
  private const string XLS = ".xls";
  private const string XLSX = ".xlsx";
  private const string XLT = ".xlt";
  private const string XLTX = ".xltx";
  private const string XLTM = ".xltm";
  private const string XLSM = ".xlsm";
  private const string ODS = ".ods";
  private const string XLSB = ".xlsb";
  private const char SheetRangeSeparator = ':';
  internal const int Date1904SystemDifference = 1462;
  private const float ScriptFactor = 1.5f;
  internal static readonly Color[] DEF_PALETTE = new Color[64 /*0x40*/]
  {
    ColorExtension.Black,
    ColorExtension.White,
    ColorExtension.Red,
    Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue, 0),
    ColorExtension.Blue,
    ColorExtension.Yellow,
    ColorExtension.Magenta,
    ColorExtension.Cyan,
    Color.FromArgb((int) byte.MaxValue, 0, 0, 0),
    Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue),
    Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0, 0),
    Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue, 0),
    Color.FromArgb((int) byte.MaxValue, 0, 0, (int) byte.MaxValue),
    Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0),
    Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0, (int) byte.MaxValue),
    Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue, (int) byte.MaxValue),
    Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 0, 0),
    Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 0),
    Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/),
    Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 0),
    Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 0, 128 /*0x80*/),
    Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/),
    Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/),
    Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/),
    Color.FromArgb((int) byte.MaxValue, 153, 153, (int) byte.MaxValue),
    Color.FromArgb((int) byte.MaxValue, 153, 51, 102),
    Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 204),
    Color.FromArgb((int) byte.MaxValue, 204, (int) byte.MaxValue, (int) byte.MaxValue),
    Color.FromArgb((int) byte.MaxValue, 102, 0, 102),
    Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/),
    Color.FromArgb((int) byte.MaxValue, 0, 102, 204),
    Color.FromArgb((int) byte.MaxValue, 204, 204, (int) byte.MaxValue),
    Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/),
    Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0, (int) byte.MaxValue),
    Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0),
    Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue, (int) byte.MaxValue),
    Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 0, 128 /*0x80*/),
    Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 0, 0),
    Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/),
    Color.FromArgb((int) byte.MaxValue, 0, 0, (int) byte.MaxValue),
    Color.FromArgb((int) byte.MaxValue, 0, 204, (int) byte.MaxValue),
    Color.FromArgb((int) byte.MaxValue, 204, (int) byte.MaxValue, (int) byte.MaxValue),
    Color.FromArgb((int) byte.MaxValue, 204, (int) byte.MaxValue, 204),
    Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 153),
    Color.FromArgb((int) byte.MaxValue, 153, 204, (int) byte.MaxValue),
    Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 153, 204),
    Color.FromArgb((int) byte.MaxValue, 204, 153, (int) byte.MaxValue),
    Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 204, 153),
    Color.FromArgb((int) byte.MaxValue, 51, 102, (int) byte.MaxValue),
    Color.FromArgb((int) byte.MaxValue, 51, 204, 204),
    Color.FromArgb((int) byte.MaxValue, 153, 204, 0),
    Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 204, 0),
    Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 153, 0),
    Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 102, 0),
    Color.FromArgb((int) byte.MaxValue, 102, 102, 153),
    Color.FromArgb((int) byte.MaxValue, 150, 150, 150),
    Color.FromArgb((int) byte.MaxValue, 0, 51, 102),
    Color.FromArgb((int) byte.MaxValue, 51, 153, 102),
    Color.FromArgb((int) byte.MaxValue, 0, 51, 0),
    Color.FromArgb((int) byte.MaxValue, 51, 51, 0),
    Color.FromArgb((int) byte.MaxValue, 153, 51, 0),
    Color.FromArgb((int) byte.MaxValue, 153, 51, 102),
    Color.FromArgb((int) byte.MaxValue, 51, 51, 153),
    Color.FromArgb((int) byte.MaxValue, 51, 51, 51)
  };
  internal static readonly double[] DefaultTints = new double[16 /*0x10*/]
  {
    -0.0499893185216834,
    -0.249977111117893,
    -0.14999847407452621,
    -0.34998626667073579,
    -0.499984740745262,
    0.34998626667073579,
    0.499984740745262,
    0.249977111117893,
    0.14999847407452621,
    0.0499893185216834,
    0.79998168889431442,
    0.59999389629810485,
    0.39997558519241921,
    -0.0999786370433668,
    -0.749992370372631,
    -0.89999084444715716
  };
  internal static readonly Color[][] ThemeColorPalette = new Color[10][]
  {
    new Color[5]
    {
      Color.FromArgb((int) byte.MaxValue, 242, 242, 242),
      Color.FromArgb((int) byte.MaxValue, 191, 191, 191),
      Color.FromArgb((int) byte.MaxValue, 217, 217, 217),
      Color.FromArgb((int) byte.MaxValue, 166, 166, 166),
      Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/)
    },
    new Color[10]
    {
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 89, 89, 89),
      Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/),
      Color.FromArgb((int) byte.MaxValue, 64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/),
      Color.FromArgb((int) byte.MaxValue, 38, 38, 38),
      Color.FromArgb((int) byte.MaxValue, 13, 13, 13)
    },
    new Color[16 /*0x10*/]
    {
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 196, 189, 151),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 148, 138, 84),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 221, 217, 196),
      Color.FromArgb((int) byte.MaxValue, 73, 69, 41),
      Color.FromArgb((int) byte.MaxValue, 29, 27, 16 /*0x10*/)
    },
    new Color[13]
    {
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 22, 54, 92),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 15, 36, 62),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 197, 217, 241),
      Color.FromArgb((int) byte.MaxValue, 141, 180, 226),
      Color.FromArgb((int) byte.MaxValue, 83, 141, 213)
    },
    new Color[13]
    {
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 54, 96 /*0x60*/, 146),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 36, 64 /*0x40*/, 98),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 220, 230, 241),
      Color.FromArgb((int) byte.MaxValue, 184, 204, 228),
      Color.FromArgb((int) byte.MaxValue, 149, 179, 215)
    },
    new Color[13]
    {
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 150, 54, 52),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 99, 37, 35),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 242, 220, 219),
      Color.FromArgb((int) byte.MaxValue, 230, 184, 183),
      Color.FromArgb((int) byte.MaxValue, 218, 150, 148)
    },
    new Color[13]
    {
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 118, 147, 60),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 79, 98, 40),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 235, 241, 222),
      Color.FromArgb((int) byte.MaxValue, 216, 228, 188),
      Color.FromArgb((int) byte.MaxValue, 196, 215, 155)
    },
    new Color[13]
    {
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 96 /*0x60*/, 73, 122),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 64 /*0x40*/, 49, 81),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 228, 223, 236),
      Color.FromArgb((int) byte.MaxValue, 204, 192 /*0xC0*/, 218),
      Color.FromArgb((int) byte.MaxValue, 177, 160 /*0xA0*/, 199)
    },
    new Color[13]
    {
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 49, 134, 155),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 33, 89, 103),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 218, 238, 243),
      Color.FromArgb((int) byte.MaxValue, 183, 222, 232),
      Color.FromArgb((int) byte.MaxValue, 146, 205, 220)
    },
    new Color[13]
    {
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 226, 107, 10),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 151, 71, 6),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb(0, 0, 0, 0),
      Color.FromArgb((int) byte.MaxValue, 253, 233, 217),
      Color.FromArgb((int) byte.MaxValue, 252, 213, 180),
      Color.FromArgb((int) byte.MaxValue, 250, 191, 143)
    }
  };
  private float[] DEF_FONT_HEIGHT_SINGLE_INCR = new float[12]
  {
    6f,
    8f,
    9f,
    12f,
    14f,
    15f,
    18f,
    21f,
    23f,
    24f,
    26f,
    27f
  };
  private float[] DEF_FONT_HEIGHT_DOUBLE_INCR = new float[3]
  {
    5f,
    17f,
    20f
  };
  private float[] DEF_FONT_WIDTH_SINGLE_INCR = new float[1]
  {
    11f
  };
  private static readonly TBIFFRecord[] DEF_PIVOTRECORDS = new TBIFFRecord[8]
  {
    TBIFFRecord.StreamId,
    TBIFFRecord.PivotViewSource,
    TBIFFRecord.DCONRef,
    TBIFFRecord.DCONBIN,
    TBIFFRecord.DCONNAME,
    TBIFFRecord.DCON,
    TBIFFRecord.PivotViewAdditionalInfo,
    TBIFFRecord.ExternalSourceInfo
  };
  private static readonly Regex ExternSheetRegEx = new Regex("(?<BookName>\\[[\\S^ ']+\\])?(?<SheetName>[\\S ]+)", RegexOptions.Compiled);
  private static readonly Regex ExternSheetRegExComplete = new Regex("(?<DirectoryName>[\\S ]*\\\\)*(?<BookName>\\[[\\S^ ']+\\])?(?<SheetName>[\\S ]+)", RegexOptions.Compiled);
  private static readonly string[] DEF_STREAM_SKIP_COPYING = new string[2]
  {
    "\u0005SummaryInformation",
    "\u0005DocumentSummaryInformation"
  };
  private static readonly char[] DEF_RESERVED_BOOK_CHARS = new char[9]
  {
    '\u0001',
    '\u0002',
    '\u0003',
    '\u0004',
    '\u0005',
    '\u0006',
    '\a',
    '\b',
    '|'
  };
  private static readonly int[] PredefinedStyleOutlines = new int[6]
  {
    0,
    3,
    4,
    5,
    6,
    7
  };
  private static readonly int[] PredefinedXFs = new int[6]
  {
    0,
    16 /*0x10*/,
    18,
    20,
    17,
    19
  };
  internal static readonly Color[] DefaultThemeColors = new Color[12]
  {
    SystemColors.Window,
    SystemColors.WindowText,
    ColorExtension.FromArgb(15658209),
    ColorExtension.FromArgb(2050429),
    ColorExtension.FromArgb(5210557),
    ColorExtension.FromArgb(12603469),
    ColorExtension.FromArgb(10206041),
    ColorExtension.FromArgb(8414370),
    ColorExtension.FromArgb(4959430),
    ColorExtension.FromArgb(16225862),
    ColorExtension.FromArgb((int) byte.MaxValue),
    ColorExtension.FromArgb(8388736 /*0x800080*/)
  };
  internal static readonly Color[] DefaultThemeColors2013 = new Color[12]
  {
    SystemColors.Window,
    SystemColors.WindowText,
    ColorExtension.FromArgb(15197926),
    ColorExtension.FromArgb(4478058),
    ColorExtension.FromArgb(6003669),
    ColorExtension.FromArgb(15564081),
    ColorExtension.FromArgb(10855845 /*0xA5A5A5*/),
    ColorExtension.FromArgb(16760832),
    ColorExtension.FromArgb(4485828),
    ColorExtension.FromArgb(7384391),
    ColorExtension.FromArgb(353217),
    ColorExtension.FromArgb(9785202)
  };
  private static readonly Color[] m_chartColors = new Color[3]
  {
    ColorExtension.ChartForeground,
    ColorExtension.ChartBackground,
    ColorExtension.ChartNeutral
  };
  private int m_pivotTableCount;
  private XlsbDataHolder m_xlsbDataHolder;
  private bool m_xlsbFormat;
  private Syncfusion.XlsIO.Implementation.TableStyles m_tableStyles;
  private Dictionary<string, List<int>> m_workbookFormulas;
  private bool m_enabledCalcEngine;
  private List<BiffRecordRaw> m_records;
  private WorksheetBaseImpl m_ActiveSheet;
  private bool m_showSheetTabs = true;
  private ITabSheet m_ActiveTabSheet;
  private WorksheetsCollection m_worksheets;
  private StylesCollection m_styles;
  private FontsCollection m_fonts;
  private ExtendedFormatsCollection m_extFormats;
  private List<NameRecord> m_arrNames;
  private Dictionary<int, int> m_modifiedFormatRecord = new Dictionary<int, int>();
  private FormatsCollection m_rawFormats;
  private List<BoundSheetRecord> m_arrBound;
  private SSTDictionary m_SSTDictionary;
  private ExternSheetRecord m_externSheet = (ExternSheetRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExternSheet);
  private List<ContinueRecord> m_continue;
  private List<IReparse> m_arrReparse;
  private string m_strFullName;
  private bool m_bDate1904;
  private bool m_bPrecisionAsDisplayed;
  private bool m_bReadOnly;
  private bool m_bSaved;
  private bool m_bSelFSUsed;
  internal bool m_bLoading;
  private bool m_bSaving;
  private bool m_bCellProtect;
  private bool m_bWindowProtect;
  private string m_strCodeName = "ThisWorkbook";
  private bool m_bHidePivotFieldList = true;
  private string m_defaultThemeVersion;
  private bool m_bHasMacros;
  private bool m_bHasSummaryInformation;
  private bool m_bHasDocumentSummaryInformation;
  private bool m_bMacrosDisable;
  private List<Color> m_colors;
  private bool m_hasStandardFont;
  private bool m_bOwnPalette;
  private WindowOneRecord m_windowOne;
  private WorkbookNamesCollection m_names;
  private ChartsCollection m_charts;
  private List<DialogSheet> m_dialogs;
  private List<MacroSheet> m_macros;
  private WorkbookObjectsCollection m_arrObjects;
  private PasswordRecord m_password;
  private PasswordRev4Record m_passwordRev4;
  private ProtectionRev4Record m_protectionRev4;
  private bool m_bThrowInFormula = true;
  private WorkbookShapeDataImpl m_shapesData;
  private int m_iFirstUnusedColor = 8;
  private int m_iCurrentObjectId;
  private int m_iCurrentHeaderId;
  private List<ExtendedFormatRecord> m_arrExtFormatRecords;
  private List<ExtendedXFRecord> m_arrXFExtRecords;
  private List<StyleExtRecord> m_arrStyleExtRecords;
  private ICompoundFile m_workbookFile;
  private bool m_bOptimization;
  private bool m_b3dRangesInDV;
  private ExternBookCollection m_externBooks;
  private AddInFunctionsCollection m_addinFunctions;
  private WorkbookShapeDataImpl m_headerFooterPictures;
  private CalculationOptionsImpl m_calcution;
  private PivotCacheCollection m_pivotCaches;
  private int[] pivotCacheIndexes;
  private int m_dxfPriority;
  private Graphics m_graphics;
  private OleStorageCollection m_OleStorageCollection;
  private string m_fullOutputFileName;
  private FormulaUtil m_formulaUtil;
  private Syncfusion.XlsIO.Implementation.Collections.Grouping.WorksheetGroup m_sheetGroup;
  private bool m_bDuplicatedNames;
  private Syncfusion.XlsIO.Implementation.Collections.BuiltInDocumentProperties m_builtInDocumentProperties;
  private Syncfusion.XlsIO.Implementation.Collections.CustomDocumentProperties m_customDocumentProperties;
  private MetaPropertiesImpl m_contentTypeProperties;
  private CustomXmlPartCollection m_customXmlPartCollection;
  internal bool m_bWriteProtection;
  private FileSharingRecord m_fileSharing;
  private bool m_bDetectDateTimeInValue = true;
  private int m_iFirstCharSize = -1;
  private int m_iSecondCharSize = -1;
  private string m_strEncryptionPassword;
  internal ExcelEncryptionType m_encryptionType;
  private byte[] m_arrDocId;
  private int m_iMaxRowCount = 65536 /*0x010000*/;
  private int m_iMaxColumnCount = 256 /*0x0100*/;
  private int m_iMaxXFCount = 4095 /*0x0FFF*/;
  private int m_iMaxIndent = 250;
  private int m_maxImportColumns;
  private ExcelVersion m_version;
  private int m_iDefaultXFIndex = 15;
  private FileDataHolder m_fileDataHolder;
  private double m_dMaxDigitWidth;
  private IntPtr m_ptrHeapHandle;
  private BiffRecordRaw m_bookExt;
  private BiffRecordRaw m_theme;
  private List<Color> m_themeColors = new List<Color>((IEnumerable<Color>) WorkbookImpl.DefaultThemeColors);
  private Stream m_controlsStream;
  private int m_iMaxTableIndex = 1;
  private int m_iCountry = 1;
  private Stream m_CustomTableStylesStream;
  private bool m_bIsLoaded;
  private bool m_bIsCreated;
  private Dictionary<string, FontImpl> m_majorFonts;
  private Dictionary<string, FontImpl> m_minorFonts;
  private bool isEqualColor;
  internal bool m_hasApostrophe;
  private bool? m_isStartsOrEndsWith;
  private bool m_isOleObjectCopied;
  private bool m_hasOleObjects;
  private MSODrawingGroupRecord m_drawGroup;
  private bool m_checkFirst;
  private int m_versioncheck;
  internal bool m_isThemeColorsParsed;
  private CompatibilityRecord m_compatibility;
  private Stream m_sstStream;
  private bool m_hasInlineString;
  private DataSorter m_dataSorter;
  private bool m_strict;
  private bool m_isConverted;
  private List<Stream> m_preservesPivotCache;
  private List<int> m_arrFontIndexes;
  private ExcelParseOptions m_options;
  internal Dictionary<int, int> m_xfCellCount = new Dictionary<int, int>();
  private int m_xfStyleArrayCount;
  internal bool IsCRCSucceed;
  internal uint crcValue;
  private int beginversion;
  private int m_iLastPivotTableIndex;
  internal Dictionary<string, List<Stream>> m_childElements = new Dictionary<string, List<Stream>>();
  internal Dictionary<string, List<Stream>> m_childElementValues = new Dictionary<string, List<Stream>>();
  internal int XmlInvalidCharCount = 1;
  private bool m_IsDisposed;
  private ExternalConnectionCollection m_connections;
  private ExternalConnectionCollection m_deletedConnections;
  private List<BiffRecordRaw> m_externalConnection;
  private bool m_bParseOnDemand;
  private RecalcIdRecord m_reCalcId = new RecalcIdRecord();
  private uint m_uCalcIdentifier = 152511;
  private bool m_isCellModified;
  internal ExcelVersion originalVersion;
  private List<string> m_preservedExternalLinks;
  private string m_algorithmName;
  private byte[] m_hashValue;
  private byte[] m_saltValue;
  private uint m_spinCount;
  private readonly string[] m_customPatterns = new string[29]
  {
    "m/d",
    "d/m/yyyy",
    "d-m-yyyy",
    "dd/MMM/yyyy",
    "d/MMM/yyyy",
    "dd-MMM-yyyy",
    "d-MMM-yyyy",
    "dd/MMM/yy",
    "d/MMM/yy",
    "dd-MMM-yy",
    "d-MMM-yy",
    "dd-MM-yyyy",
    "dd-MM-yy",
    "d-m-yy",
    "mm-dd-yy",
    "yyyy-dd-MM",
    "d-MMM",
    "dd-MMM",
    "d/MMM",
    "dd/MMM",
    "MMM-dd",
    "MMM/dd",
    "MMM/d",
    "MMM/dd",
    "MMM-yy",
    "MMM/yy",
    "MMM-yyyy",
    "MMM/yyyy",
    "yyyy-M-d"
  };
  private string[] m_DateTimePatterns;
  private bool m_isConverting;
  internal Dictionary<int, int> m_usedCellStyleIndex = new Dictionary<int, int>();
  internal int m_XFRemovedCount;
  internal int m_XFstartIndex;
  internal bool m_bisXFStartIndexFound;
  internal bool m_bisUnusedXFRemoved;
  internal bool m_bisXml;
  internal bool m_bisStylesCopied;
  internal bool m_bisCopy;
  internal bool m_bisVersionSet;
  private XmlMapCollection m_xmlMaps;
  private Dictionary<string, List<string>> m_precedentsCache;
  private Dictionary<int, int> m_arrNewNumberFormatIndexes;
  private bool m_calcEngineEnabledOnReadWrite;
  private IVbaProject m_vbaProject;
  private ICompoundStorage m_macroStorage;
  private Dictionary<int, ShapeLineFormatImpl> m_lineStyles;
  private static Dictionary<ExcelSheetType, string> SheetTypeToName = new Dictionary<ExcelSheetType, string>(5);
  private byte[] m_calcEnginePreviousValues;

  internal event WorkbookImpl.WarningEventHandler WarningCallBack;

  internal VbaModule VbaModule
  {
    get
    {
      VbaModule vbaModule = (VbaModule) null;
      if (!this.Loading && this.HasMacros && this.VbaProject != null)
      {
        IVbaModules modules = this.VbaProject.Modules;
        if (modules != null)
          vbaModule = modules[this.CodeName] as VbaModule;
      }
      return vbaModule;
    }
  }

  internal ICompoundStorage MacroStorage
  {
    get => this.m_macroStorage;
    set => this.m_macroStorage = value;
  }

  public IVbaProject VbaProject
  {
    get
    {
      if (this.m_vbaProject == null)
      {
        if (this.MacroStorage != null && !this.Loading)
          this.ParseVbaProject();
        else
          this.CreateVbaProject();
      }
      return this.m_vbaProject;
    }
    internal set => this.m_vbaProject = value;
  }

  internal bool HasVbaProject => this.m_vbaProject != null;

  public IWorksheet ActiveSheet => this.m_ActiveSheet as IWorksheet;

  internal ITabSheet ActiveTabSheet => this.m_ActiveTabSheet;

  internal List<string> PreservedExternalLinks
  {
    get
    {
      if (this.m_preservedExternalLinks == null)
        this.m_preservedExternalLinks = new List<string>(10);
      return this.m_preservedExternalLinks;
    }
  }

  public int ActiveSheetIndex
  {
    get => this.m_ActiveSheet == null ? -1 : this.m_ActiveSheet.RealIndex;
    set
    {
      if (value < 0 || value >= this.ObjectCount)
        throw new ArgumentOutOfRangeException(nameof (ActiveSheetIndex));
      WorksheetBaseImpl activeSheet1 = this.m_ActiveSheet;
      int activeSheetIndex = this.ActiveSheetIndex;
      this.m_ActiveSheet = this.Objects[value] as WorksheetBaseImpl;
      this.m_ActiveTabSheet = this.Objects[value] as ITabSheet;
      if ((activeSheet1 == null || activeSheet1 != null && activeSheet1.Workbook.Equals((object) this) && activeSheetIndex != value) && this.m_ActiveTabSheet != null)
        this.m_ActiveTabSheet.Activate();
      ISerializableNamedObject activeSheet2 = (ISerializableNamedObject) this.m_ActiveSheet;
      if (activeSheet2 == null)
        return;
      this.WindowOne.SelectedTab = (ushort) activeSheet2.RealIndex;
    }
  }

  internal bool ShowSheetTabs
  {
    get => this.m_showSheetTabs;
    set => this.m_showSheetTabs = value;
  }

  public string Author
  {
    get => this.m_builtInDocumentProperties[ExcelBuiltInProperty.Author].Text;
    set => this.m_builtInDocumentProperties[ExcelBuiltInProperty.Author].Text = value;
  }

  public IBuiltInDocumentProperties BuiltInDocumentProperties
  {
    get => (IBuiltInDocumentProperties) this.m_builtInDocumentProperties;
  }

  internal event ValueChangedEventHandler MacroNameChanged;

  public string CodeName
  {
    get => this.m_strCodeName;
    set
    {
      string strCodeName = this.m_strCodeName;
      if (this.VbaModule != null)
      {
        this.VbaModule.Name = value;
        this.VbaModule.Attributes["VB_NAME"].Value = value;
      }
      this.m_strCodeName = value;
      ValueChangedEventArgs e = new ValueChangedEventArgs((object) strCodeName, (object) value, nameof (CodeName));
      if (this.MacroNameChanged == null)
        return;
      this.MacroNameChanged((object) this, e);
    }
  }

  public bool HidePivotFieldList
  {
    get => this.m_bHidePivotFieldList;
    set => this.m_bHidePivotFieldList = value;
  }

  public string DefaultThemeVersion
  {
    get => this.m_defaultThemeVersion;
    set => this.m_defaultThemeVersion = value;
  }

  public ICustomDocumentProperties CustomDocumentProperties
  {
    get => (ICustomDocumentProperties) this.m_customDocumentProperties;
  }

  public IMetaProperties ContentTypeProperties => (IMetaProperties) this.m_contentTypeProperties;

  public ICustomXmlPartCollection CustomXmlparts
  {
    get => (ICustomXmlPartCollection) this.m_customXmlPartCollection;
  }

  public bool Date1904
  {
    get => this.m_bDate1904;
    set => this.m_bDate1904 = value;
  }

  public bool PrecisionAsDisplayed
  {
    get => this.m_bPrecisionAsDisplayed;
    set => this.m_bPrecisionAsDisplayed = value;
  }

  public bool IsCellProtection => this.m_bCellProtect;

  public bool IsWindowProtection => this.m_bWindowProtect;

  public INames Names
  {
    [DebuggerStepThrough] get => (INames) this.m_names;
  }

  internal List<BiffRecordRaw> PreserveExternalConnectionDetails
  {
    get
    {
      if (this.m_externalConnection == null)
        this.m_externalConnection = new List<BiffRecordRaw>();
      return this.m_externalConnection;
    }
  }

  public bool ReadOnly
  {
    get
    {
      if (this.IsLoaded && this.FullFileName != null)
      {
        string fullPath = Path.GetFullPath(this.FullFileName);
        if (File.Exists(fullPath) && (File.GetAttributes(fullPath) & FileAttributes.ReadOnly) != (FileAttributes) 0)
          return true;
      }
      return this.m_bReadOnly;
    }
    internal set => this.m_bReadOnly = value;
  }

  public bool Saved
  {
    get => this.m_bSaved;
    set
    {
      if (!this.m_bSaved && value != this.m_bSaved)
        this.Save();
      this.m_bSaved = value;
    }
  }

  internal IRange TryParseTableOrNamedRange(string value, WorksheetImpl workSheet)
  {
    IRange tableOrNamedRange = (IRange) null;
    Match match = new Regex("(?<table_name>[^\\[]*)((\\[((?<is_table_all>\\[?#all\\]?)|(?<is_table_headers>\\[?#headers\\]?)|(?<is_table_first_row>@)),?(?<values>.*)\\])|(\\[(?<data_only>.*)\\]))?", RegexOptions.IgnoreCase).Match(value);
    if (match.Success)
    {
      string str1 = match.Groups["table_name"].Value;
      bool success1 = match.Groups["is_table_all"].Success;
      bool success2 = match.Groups["is_table_headers"].Success;
      bool success3 = match.Groups["is_table_first_row"].Success;
      bool success4 = match.Groups["data_only"].Success;
      string str2;
      if (success1 || success3 || success2)
        str2 = match.Groups["values"].Value;
      else if (success4)
      {
        str2 = match.Groups["data_only"].Value;
        if (!str2.Contains(":"))
          str2 = $"[{str2}]";
      }
      else
      {
        WorkbookNamesCollection names = this.m_names;
        return names.Contains(str1) ? names[str1].RefersToRange : tableOrNamedRange;
      }
      if (str2 != "")
      {
        string[] columnsFromTo = str2.Split(':');
        tableOrNamedRange = this.GetTableRangeFromValues(str1, success1, success2, success3, columnsFromTo, workSheet);
      }
      else
        tableOrNamedRange = this.GetTableRangeFromValues(str1, success1, success2, success3, (string[]) null, workSheet);
    }
    return tableOrNamedRange;
  }

  private IRange GetTableRangeFromValues(
    string tableName,
    bool isAll,
    bool isHeaders,
    bool isFirstRow,
    string[] columnsFromTo,
    WorksheetImpl workSheet)
  {
    IRange tableRangeFromValues = (IRange) null;
    WorkbookNamesCollection names = this.m_names;
    if (names.Contains(tableName))
    {
      Regex regex = new Regex("^\\[(?<column_name>.*)\\]$");
      if (columnsFromTo != null)
      {
        string strB1 = regex.Match(columnsFromTo[0]).Groups["column_name"].Value;
        string strB2 = columnsFromTo.Length > 1 ? regex.Match(columnsFromTo[1]).Groups["column_name"].Value : (string) null;
        if (strB1 != null && strB1 != "")
        {
          IListObject table = this.GetTable(workSheet, tableName);
          if (table != null)
          {
            int num1 = 0;
            int num2 = 0;
            for (int index = 0; index < table.Columns.Count; ++index)
            {
              if (string.Compare(table.Columns[index].Name, strB1, true) == 0)
              {
                num1 = index;
                num2 = index;
                if (strB2 == null)
                  break;
              }
              else if (strB2 != null && string.Compare(table.Columns[index].Name, strB2, true) == 0)
              {
                num2 = index;
                if (num1 > num2)
                {
                  num1 = num2;
                  break;
                }
                break;
              }
            }
            IRange location = table.Location;
            if (num1 < location.Columns.Length && num2 < location.Columns.Length)
            {
              int num3 = isAll || isHeaders ? 0 : 1;
              int num4 = isHeaders || isFirstRow ? 0 : location.Rows.Length - table.TotalsRowCount - 1;
              tableRangeFromValues = location.Worksheet[location.Row + num3, location.Column + num1, location.Row + num4, location.Column + num2];
            }
            else
              tableRangeFromValues = location;
          }
        }
        else
          tableRangeFromValues = names[tableName + "[All]"].RefersToRange;
      }
      else if (isAll)
        tableRangeFromValues = names[tableName + "[All]"].RefersToRange;
      else if (isHeaders)
        tableRangeFromValues = names[tableName + "[Headers]"].RefersToRange;
      else if (isFirstRow)
      {
        tableRangeFromValues = names[tableName + "[Data]"].RefersToRange;
        if (tableRangeFromValues != null)
          tableRangeFromValues = tableRangeFromValues.Worksheet[tableRangeFromValues.Row, tableRangeFromValues.Column, tableRangeFromValues.Row, tableRangeFromValues.LastColumn];
      }
      else
        tableRangeFromValues = names[tableName + "[Data]"].RefersToRange;
    }
    return tableRangeFromValues;
  }

  private IListObject GetTable(WorksheetImpl workSheet, string tableName)
  {
    IListObject table = (IListObject) null;
    if (workSheet == null)
    {
      foreach (WorksheetImpl worksheet in (Syncfusion.XlsIO.Implementation.Collections.CollectionBase<IWorksheet>) this.m_worksheets)
      {
        if (worksheet.ListObjects.Count > 0)
          table = worksheet.InnerListObjects[tableName];
        if (table != null)
          break;
      }
    }
    else
      table = workSheet.InnerListObjects[tableName];
    return table;
  }

  public IStyles Styles
  {
    [DebuggerStepThrough] get => (IStyles) this.m_styles;
  }

  public IWorksheets Worksheets
  {
    [DebuggerStepThrough] get => (IWorksheets) this.m_worksheets;
  }

  public IConnections Connections
  {
    [DebuggerStepThrough] get => (IConnections) this.m_connections;
  }

  public bool HasMacros
  {
    get => this.m_bHasMacros;
    internal set => this.m_bHasMacros = value;
  }

  public IConnections DeletedConnections
  {
    get
    {
      if (this.m_deletedConnections == null)
        this.m_deletedConnections = new ExternalConnectionCollection(this.Application, (object) this);
      return (IConnections) this.m_deletedConnections;
    }
  }

  public Color[] Palettte => this.m_colors.ToArray();

  public Color[] Palette => this.m_colors.ToArray();

  public int DisplayedTab
  {
    get => (int) this.WindowOne.DisplayedTab;
    set
    {
      if ((value < 0 || value > this.m_arrObjects.Count) && this.IsCreated)
        throw new ArgumentOutOfRangeException(nameof (DisplayedTab), "Displayed tab must be greater than zero and less than Worksheets count");
      this.WindowOne.DisplayedTab = (ushort) value;
    }
  }

  public ICharts Charts => (ICharts) this.m_charts;

  public bool ThrowOnUnknownNames
  {
    get => this.m_bThrowInFormula;
    set => this.m_bThrowInFormula = value;
  }

  public bool IsHScrollBarVisible
  {
    get => this.WindowOne.IsHScroll;
    set => this.WindowOne.IsHScroll = value;
  }

  public bool IsVScrollBarVisible
  {
    get => this.WindowOne.IsVScroll;
    set => this.WindowOne.IsVScroll = value;
  }

  public bool DisableMacrosStart
  {
    get => this.m_bMacrosDisable;
    set
    {
      if (value == this.m_bMacrosDisable)
        return;
      this.m_bMacrosDisable = value;
      this.Saved = false;
    }
  }

  public double StandardFontSize
  {
    get => ((FontImpl) this.m_fonts[0]).Size;
    set
    {
      if (value == this.StandardFontSize)
        return;
      this.m_hasStandardFont = true;
      ((FontImpl) this.m_fonts[0]).Size = (double) (int) value;
      this.UpdateStandardRowHeight();
      FontWrapper font = this.Styles["Normal"].Font as FontWrapper;
      this.m_dMaxDigitWidth = -1.0;
      if (font.Index >= 4)
        return;
      font.InvokeAfterChange();
    }
  }

  internal bool HasStandardFont => this.m_hasStandardFont;

  public string StandardFont
  {
    get => ((FontImpl) this.m_fonts[0]).FontName;
    set
    {
      if (!(value != this.StandardFont))
        return;
      this.m_hasStandardFont = true;
      ((FontImpl) this.m_fonts[0]).FontName = value;
      this.UpdateStandardRowHeight();
    }
  }

  public bool Allow3DRangesInDataValidation
  {
    get => this.m_b3dRangesInDV;
    set => this.m_b3dRangesInDV = value;
  }

  public IAddInFunctions AddInFunctions => (IAddInFunctions) this.m_addinFunctions;

  public ICalculationOptions CalculationOptions => (ICalculationOptions) this.m_calcution;

  public string RowSeparator => this.FormulaUtil.ArrayRowSeparator;

  public string ArgumentsSeparator => this.FormulaUtil.OperandsSeparator;

  public IWorksheetGroup WorksheetGroup => (IWorksheetGroup) this.m_sheetGroup;

  public bool IsRightToLeft
  {
    get => this.m_worksheets.IsRightToLeft;
    set => this.m_worksheets.IsRightToLeft = value;
  }

  public bool DisplayWorkbookTabs
  {
    get => this.WindowOne.IsTabs;
    set => this.WindowOne.IsTabs = value;
  }

  public ITabSheets TabSheets => (ITabSheets) this.m_arrObjects;

  public bool DetectDateTimeInValue
  {
    get => this.m_bDetectDateTimeInValue;
    set => this.m_bDetectDateTimeInValue = value;
  }

  public bool UseFastStringSearching
  {
    get => this.m_SSTDictionary.UseHashForSearching;
    set => this.m_SSTDictionary.UseHashForSearching = value;
  }

  public bool ReadOnlyRecommended
  {
    get => this.m_fileSharing != null && this.m_fileSharing.RecommendReadOnly != (ushort) 0;
    set
    {
      if (value)
      {
        if (this.m_fileSharing == null)
          this.m_fileSharing = (FileSharingRecord) BiffRecordFactory.GetRecord(TBIFFRecord.FileSharing);
        this.m_fileSharing.RecommendReadOnly = (ushort) 1;
      }
      else
      {
        if (this.m_fileSharing == null)
          return;
        this.m_fileSharing.RecommendReadOnly = (ushort) 0;
      }
    }
  }

  public string PasswordToOpen
  {
    get => this.m_strEncryptionPassword;
    set
    {
      this.m_strEncryptionPassword = value;
      if (value == null || value.Length == 0)
        this.m_encryptionType = ExcelEncryptionType.None;
      else
        this.m_encryptionType = ExcelEncryptionType.Standard;
    }
  }

  public int MaxRowCount => this.m_iMaxRowCount;

  public int MaxColumnCount => this.m_iMaxColumnCount;

  public int MaxXFCount => this.m_iMaxXFCount;

  public int MaxIndent => this.m_iMaxIndent;

  public int MaxImportColumns
  {
    get => this.m_maxImportColumns;
    set => this.m_maxImportColumns = value;
  }

  internal int StyleArrayCount
  {
    get => this.m_xfStyleArrayCount;
    set => this.m_xfStyleArrayCount = value;
  }

  internal int PivotTableCount
  {
    get => this.m_pivotTableCount;
    set => this.m_pivotTableCount = value;
  }

  internal bool IsUnusedXFRemoved => this.m_bisUnusedXFRemoved;

  public ITableStyles TableStyles
  {
    get
    {
      if (this.m_tableStyles == null)
        this.m_tableStyles = new Syncfusion.XlsIO.Implementation.TableStyles(this, this.Application);
      return (ITableStyles) this.m_tableStyles;
    }
  }

  internal Dictionary<string, List<int>> WorkbookFormulas
  {
    get => this.m_workbookFormulas;
    set => this.m_workbookFormulas = value;
  }

  internal int BookCFPriorityCount
  {
    get => this.m_dxfPriority;
    set => this.m_dxfPriority = value;
  }

  internal bool OwnPalette
  {
    get => this.m_bOwnPalette;
    set => this.m_bOwnPalette = value;
  }

  internal bool EnabledCalcEngine
  {
    get => this.m_enabledCalcEngine;
    set => this.m_enabledCalcEngine = value;
  }

  internal ExcelParseOptions Options
  {
    get => this.m_options;
    set => this.m_options = value;
  }

  internal int LastPivotTableIndex
  {
    get => this.m_iLastPivotTableIndex;
    set => this.m_iLastPivotTableIndex = value;
  }

  internal List<Stream> PreservesPivotCache
  {
    get
    {
      if (this.m_preservesPivotCache == null)
        this.m_preservesPivotCache = new List<Stream>();
      return this.m_preservesPivotCache;
    }
  }

  internal List<int> ArrayFontIndex
  {
    get => this.m_arrFontIndexes;
    set => this.m_arrFontIndexes = value;
  }

  public FileDataHolder DataHolder => this.m_fileDataHolder;

  public WorkbookNamesCollection InnerNamesColection
  {
    [DebuggerStepThrough] get => this.m_names;
  }

  public MetaPropertiesImpl InnerContentTypeProperties
  {
    [DebuggerStepThrough] get => this.m_contentTypeProperties;
  }

  public CustomXmlPartCollection InnerCustomXmlParts
  {
    [DebuggerStepThrough] get => this.m_customXmlPartCollection;
  }

  public AddInFunctionsCollection InnerAddInFunctions
  {
    [DebuggerStepThrough] get => this.m_addinFunctions;
  }

  public string FullFileName
  {
    [DebuggerStepThrough] get => this.m_strFullName;
    [DebuggerStepThrough] internal set => this.m_strFullName = value;
  }

  public FontsCollection InnerFonts
  {
    [DebuggerStepThrough] get => this.m_fonts;
  }

  public ExtendedFormatsCollection InnerExtFormats
  {
    [DebuggerStepThrough] get
    {
      if (!this.Loading && !this.m_bisUnusedXFRemoved && !this.IsConverting)
        this.RemoveUnusedXFRecord();
      return this.m_extFormats;
    }
  }

  public FormatsCollection InnerFormats
  {
    [DebuggerStepThrough] get => this.m_rawFormats;
  }

  public SSTDictionary InnerSST
  {
    [DebuggerStepThrough] get => this.m_SSTDictionary;
  }

  public bool Loading
  {
    [DebuggerStepThrough] get => this.m_bLoading;
    [DebuggerStepThrough] set
    {
      this.m_bLoading = value;
      if (value || !this.m_calcEngineEnabledOnReadWrite || this.Worksheets.Count <= 0)
        return;
      this.Worksheets[0].DisableSheetCalculations();
      this.m_calcEngineEnabledOnReadWrite = false;
    }
  }

  public bool Saving
  {
    [DebuggerStepThrough] get => this.m_bSaving;
    [DebuggerStepThrough] internal set
    {
      this.m_bSaving = value;
      if (!value)
        this.CalcEngineMemberValuesOnSheet(true);
      if (value || !this.m_calcEngineEnabledOnReadWrite || this.Worksheets.Count <= 0)
        return;
      this.Worksheets[0].DisableSheetCalculations();
      this.m_calcEngineEnabledOnReadWrite = false;
    }
  }

  [CLSCompliant(false)]
  public WindowOneRecord WindowOne
  {
    get
    {
      if (this.m_windowOne == null)
        this.m_windowOne = (WindowOneRecord) BiffRecordFactory.GetRecord(TBIFFRecord.WindowOne);
      return this.m_windowOne;
    }
  }

  public int ObjectCount => this.m_arrObjects.Count;

  public double MaxDigitWidth
  {
    get
    {
      if (this.m_dMaxDigitWidth <= 0.0)
        this.m_dMaxDigitWidth = this.GetMaxDigitWidth();
      return this.m_dMaxDigitWidth;
    }
    internal set => this.m_dMaxDigitWidth = value;
  }

  internal Stream SSTStream
  {
    get => this.m_sstStream;
    set => this.m_sstStream = value;
  }

  internal bool HasInlineStrings
  {
    get => this.m_hasInlineString;
    set => this.m_hasInlineString = value;
  }

  internal PasswordRecord Password
  {
    get
    {
      if (this.m_password == null)
        this.m_password = (PasswordRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Password);
      return this.m_password;
    }
    set => this.m_password = value;
  }

  [CLSCompliant(false)]
  protected PasswordRev4Record PasswordRev4
  {
    get
    {
      if (this.m_passwordRev4 == null)
        this.m_passwordRev4 = (PasswordRev4Record) BiffRecordFactory.GetRecord(TBIFFRecord.PasswordRev4);
      return this.m_passwordRev4;
    }
    set => this.m_passwordRev4 = value;
  }

  [CLSCompliant(false)]
  protected ProtectionRev4Record ProtectionRev4
  {
    get
    {
      if (this.m_protectionRev4 == null)
        this.m_protectionRev4 = (ProtectionRev4Record) BiffRecordFactory.GetRecord(TBIFFRecord.ProtectionRev4);
      return this.m_protectionRev4;
    }
    set => this.m_protectionRev4 = value;
  }

  public int CurrentObjectId
  {
    get => this.m_iCurrentObjectId;
    set
    {
      if (value <= 0)
        return;
      this.m_iCurrentObjectId = value;
    }
  }

  public int CurrentHeaderId
  {
    get => this.m_iCurrentHeaderId;
    set
    {
      this.m_iCurrentHeaderId = value >= 0 ? value : throw new ArgumentOutOfRangeException("shape id");
    }
  }

  protected internal List<ExtendedFormatRecord> InnerExtFormatRecords => this.m_arrExtFormatRecords;

  protected internal List<ExtendedXFRecord> InnerXFExtRecords => this.m_arrXFExtRecords;

  protected internal WorkbookObjectsCollection Objects
  {
    get => this.m_arrObjects;
    internal set => this.m_arrObjects = value;
  }

  protected internal StylesCollection InnerStyles => this.m_styles;

  protected internal WorksheetsCollection InnerWorksheets => this.m_worksheets;

  protected internal ChartsCollection InnerCharts => this.m_charts;

  internal List<DialogSheet> InnerDialogs
  {
    get
    {
      if (this.m_dialogs == null)
        this.m_dialogs = new List<DialogSheet>();
      return this.m_dialogs;
    }
  }

  internal List<MacroSheet> InnerMacros
  {
    get
    {
      if (this.m_macros == null)
        this.m_macros = new List<MacroSheet>();
      return this.m_macros;
    }
  }

  public ExternBookCollection ExternWorkbooks => this.m_externBooks;

  public CalculationOptionsImpl InnerCalculation => this.m_calcution;

  public Graphics InnerGraphics => this.m_graphics;

  internal string FullOutputFileName
  {
    get => this.m_fullOutputFileName;
    set => this.m_fullOutputFileName = value;
  }

  [CLSCompliant(false)]
  public FormulaUtil FormulaUtil
  {
    get
    {
      if (this.m_formulaUtil == null)
        this.m_formulaUtil = new FormulaUtil(this.Application, (object) this);
      return this.m_formulaUtil;
    }
  }

  public Syncfusion.XlsIO.Implementation.Collections.Grouping.WorksheetGroup InnerWorksheetGroup
  {
    get => this.m_sheetGroup;
  }

  internal bool? IsStartsOrEndsWith
  {
    get => this.m_isStartsOrEndsWith;
    set => this.m_isStartsOrEndsWith = value;
  }

  public bool HasDuplicatedNames
  {
    get => this.m_bDuplicatedNames;
    set => this.m_bDuplicatedNames = value;
  }

  public WorkbookShapeDataImpl ShapesData => this.m_shapesData;

  public WorkbookShapeDataImpl HeaderFooterData => this.m_headerFooterPictures;

  internal ExternSheetRecord ExternSheet => this.m_externSheet;

  protected internal bool InternalSaved
  {
    get => this.m_bSaved;
    set => this.m_bSaved = value;
  }

  public int FirstCharSize
  {
    get => this.m_iFirstCharSize;
    set => this.m_iFirstCharSize = value;
  }

  public int SecondCharSize
  {
    get => this.m_iSecondCharSize;
    set => this.m_iSecondCharSize = value;
  }

  internal bool IsConverted => this.m_isConverted;

  public int BeginVersion
  {
    get => this.beginversion;
    set => this.beginversion = value;
  }

  internal DataSorter DataSorter
  {
    get => this.m_dataSorter;
    set => this.m_dataSorter = value;
  }

  internal bool IsStrict
  {
    get => this.m_strict;
    set => this.m_strict = value;
  }

  public ExcelVersion Version
  {
    get => this.m_version;
    set
    {
      if (value != ExcelVersion.Excel97to2003 && !this.m_bisVersionSet)
      {
        this.m_bisXml = true;
        this.m_bisVersionSet = true;
      }
      else
      {
        this.m_bisXml = false;
        this.m_bisVersionSet = true;
      }
      bool flag = false;
      if (this.m_checkFirst && this.m_version == ExcelVersion.Excel97to2003 && value != ExcelVersion.Excel97to2003)
        this.m_isConverted = true;
      else if (!this.m_checkFirst)
        this.m_checkFirst = true;
      if (value == ExcelVersion.Excel97to2003 && this.m_versioncheck == 0)
      {
        this.beginversion = 1;
        ++this.m_versioncheck;
      }
      else if ((this.m_versioncheck == 0 || this.beginversion == 2) && value != ExcelVersion.Excel97to2003)
      {
        this.beginversion = 0;
        ++this.m_versioncheck;
      }
      if (value == ExcelVersion.Excel2007 && this.DataHolder != null && this.DataHolder.FileVersion != null)
      {
        this.DataHolder.FileVersion.LastEdited = "4";
        this.DataHolder.FileVersion.BuildVersion = "4506";
        this.DataHolder.FileVersion.LowestEdited = "4";
      }
      if (this.m_version >= ExcelVersion.Excel2007 && value >= ExcelVersion.Excel2007)
      {
        this.m_version = value;
      }
      else
      {
        if (this.m_version == value)
          return;
        this.originalVersion = this.m_version;
        this.m_version = value;
        switch (value)
        {
          case ExcelVersion.Excel97to2003:
            this.m_iMaxRowCount = 65536 /*0x010000*/;
            this.m_iMaxColumnCount = 256 /*0x0100*/;
            this.m_extFormats.SetMaxCount(4075);
            this.m_iMaxXFCount = 4075;
            this.m_extFormats.SetMaxCount(4095 /*0x0FFF*/);
            this.m_iMaxXFCount = 4095 /*0x0FFF*/;
            this.m_iMaxIndent = 15;
            this.ChangeStylesTo97();
            this.ClearPivotCaches();
            break;
          case ExcelVersion.Excel2007:
          case ExcelVersion.Excel2010:
          case ExcelVersion.Excel2013:
          case ExcelVersion.Excel2016:
          case ExcelVersion.Xlsx:
            this.m_iMaxRowCount = 1048576 /*0x100000*/;
            this.m_iMaxColumnCount = 16384 /*0x4000*/;
            this.m_extFormats.SetMaxCount(65000);
            this.m_iMaxXFCount = 65000;
            this.m_iMaxIndent = 250;
            if (this.originalVersion == ExcelVersion.Excel97to2003)
            {
              this.ClearPivotCaches();
              flag = true;
            }
            this.m_names.Validate();
            break;
        }
        int Index = 0;
        for (int count = this.m_worksheets.Count; Index < count; ++Index)
        {
          WorksheetImpl worksheet = (WorksheetImpl) this.m_worksheets[Index];
          PivotTableCollection pivotTables = worksheet.PivotTables as PivotTableCollection;
          if (flag)
            pivotTables.ClearWithoutCheck();
          worksheet.Version = value;
        }
        if (value == ExcelVersion.Excel97to2003)
          this.m_SSTDictionary.RemoveUnnecessaryStrings();
        WorkbookNamesCollection innerNamesColection = this.InnerNamesColection;
        if (this.originalVersion == ExcelVersion.Excel97to2003 && value != ExcelVersion.Excel97to2003 && innerNamesColection != null)
        {
          foreach (IName name in (Syncfusion.XlsIO.Implementation.Collections.CollectionBase<IName>) innerNamesColection)
          {
            if (name.Name == PageSetupImpl.DEF_AREA_XlS)
              name.Name = PageSetupImpl.DEF_AREA_XlSX;
            else if (name.Name == PageSetupImpl.DEF_TITLE_XLS)
              name.Name = PageSetupImpl.DEF_TITLE_XLSX;
          }
        }
        if (this.originalVersion != ExcelVersion.Excel97to2003 && value == ExcelVersion.Excel97to2003 && innerNamesColection != null)
        {
          foreach (IName name in (Syncfusion.XlsIO.Implementation.Collections.CollectionBase<IName>) innerNamesColection)
          {
            if (name.Name == PageSetupImpl.DEF_AREA_XlSX)
              name.Name = PageSetupImpl.DEF_AREA_XlS;
            else if (name.Name == PageSetupImpl.DEF_TITLE_XLSX)
              name.Name = PageSetupImpl.DEF_TITLE_XLS;
          }
        }
        innerNamesColection?.ConvertFullRowColumnNames(value);
      }
    }
  }

  private void ClearPivotCaches()
  {
    if (this.m_pivotCaches == null)
      return;
    this.m_pivotCaches.Clear();
  }

  public int DefaultXFIndex
  {
    get => this.m_iDefaultXFIndex;
    set
    {
      this.m_iDefaultXFIndex = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof (DefaultXFIndex));
    }
  }

  public List<Color> InnerPalette => this.m_colors;

  public IntPtr HeapHandle
  {
    get
    {
      if (this.m_ptrHeapHandle == IntPtr.Zero)
        this.m_ptrHeapHandle = Heap.HeapCreate(0, 131072 /*0x020000*/, 0);
      return this.m_ptrHeapHandle;
    }
  }

  public PivotCacheCollection PivotCaches
  {
    get
    {
      if (this.m_pivotCaches == null)
        this.m_pivotCaches = new PivotCacheCollection(this.Application, (object) this);
      return this.m_pivotCaches;
    }
  }

  IPivotCaches IWorkbook.PivotCaches
  {
    get
    {
      if (this.m_pivotCaches == null)
        this.m_pivotCaches = new PivotCacheCollection(this.Application, (object) this);
      return (IPivotCaches) this.m_pivotCaches;
    }
  }

  public Stream ControlsStream
  {
    get => this.m_controlsStream;
    internal set => this.m_controlsStream = value;
  }

  internal Stream CustomTableStylesStream
  {
    get => this.m_CustomTableStylesStream;
    set => this.m_CustomTableStylesStream = value;
  }

  public int MaxTableIndex
  {
    get => this.m_iMaxTableIndex;
    set => this.m_iMaxTableIndex = value;
  }

  internal bool IsCreated => this.m_bIsCreated;

  internal bool IsConverting
  {
    get => this.m_isConverting;
    set => this.m_isConverting = value;
  }

  public bool IsLoaded => this.m_bIsLoaded;

  internal Dictionary<string, FontImpl> MajorFonts
  {
    get => this.m_majorFonts;
    set => this.m_majorFonts = value;
  }

  internal Dictionary<string, FontImpl> MinorFonts
  {
    get => this.m_minorFonts;
    set => this.m_minorFonts = value;
  }

  internal Dictionary<int, ShapeLineFormatImpl> LineStyles
  {
    get => this.m_lineStyles;
    set => this.m_lineStyles = value;
  }

  public bool CheckCompability
  {
    get => this.m_compatibility != null && this.m_compatibility.NoComptabilityCheck != 0U;
    set
    {
      if (this.m_compatibility == null)
        this.m_compatibility = new CompatibilityRecord();
      this.m_compatibility.NoComptabilityCheck = !value ? 1U : 0U;
    }
  }

  internal bool HasApostrophe
  {
    get => this.m_hasApostrophe;
    set => this.m_hasApostrophe = value;
  }

  public bool HasOleObjects
  {
    get => this.m_hasOleObjects;
    set => this.m_hasOleObjects = value;
  }

  public bool IsOleObjectCopied
  {
    get => this.m_isOleObjectCopied;
    set => this.m_isOleObjectCopied = value;
  }

  public OleStorageCollection OleStorageCollection
  {
    get
    {
      if (this.m_OleStorageCollection == null)
        this.m_OleStorageCollection = new OleStorageCollection();
      return this.m_OleStorageCollection;
    }
  }

  internal uint CalcIdentifier => this.m_uCalcIdentifier;

  internal bool ParseOnDemand
  {
    get => this.m_bParseOnDemand;
    set => this.m_bParseOnDemand = value;
  }

  internal bool IsCellModified
  {
    get => this.m_isCellModified;
    set => this.m_isCellModified = value;
  }

  public string AlgorithmName
  {
    get => this.m_algorithmName;
    set => this.m_algorithmName = value;
  }

  public byte[] HashValue
  {
    get => this.m_hashValue;
    set => this.m_hashValue = value;
  }

  public byte[] SaltValue
  {
    get => this.m_saltValue;
    set => this.m_saltValue = value;
  }

  public uint SpinCount
  {
    get => this.m_spinCount;
    set => this.m_spinCount = value;
  }

  internal string[] DateTimePatterns
  {
    get
    {
      if (this.m_DateTimePatterns != null)
        return this.m_DateTimePatterns;
      string[] dateTimePatterns = CultureInfo.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns();
      this.m_DateTimePatterns = new string[dateTimePatterns.Length + this.m_customPatterns.Length];
      dateTimePatterns.CopyTo((Array) this.m_DateTimePatterns, 0);
      this.m_customPatterns.CopyTo((Array) this.m_DateTimePatterns, dateTimePatterns.Length);
      return this.m_DateTimePatterns;
    }
  }

  public XmlMapCollection XmlMaps => this.m_xmlMaps;

  internal bool HasFileSharing => this.m_fileSharing != null;

  internal Dictionary<string, List<string>> PrecedentsCache
  {
    get => this.m_precedentsCache;
    set => this.m_precedentsCache = value;
  }

  internal Dictionary<int, int> ArrNewNumberFormatIndexes
  {
    get => this.m_arrNewNumberFormatIndexes;
    set => this.m_arrNewNumberFormatIndexes = value;
  }

  internal bool CalcEngineEnabledOnReadWrite
  {
    get => this.m_calcEngineEnabledOnReadWrite;
    set => this.m_calcEngineEnabledOnReadWrite = value;
  }

  internal void RaiseWarning(string description, WarningType type)
  {
    if (this.WarningCallBack == null)
      return;
    this.WarningCallBack(description, type);
  }

  public Color GetThemeColor(int color) => this.m_themeColors[color];

  internal Color GetThemeColor2013(int color) => WorkbookImpl.DefaultThemeColors2013[color];

  protected internal IExtendedFormat CreateExtFormat(bool bForceAdd)
  {
    ExtendedFormatImpl format = new ExtendedFormatImpl(this.Application, (object) this);
    format.Index = (int) (ushort) this.m_extFormats.Count;
    if (bForceAdd)
      this.m_extFormats.ForceAdd(format);
    else
      this.m_extFormats.Add(format);
    return (IExtendedFormat) format;
  }

  protected internal IExtendedFormat CreateExtFormat(IExtendedFormat baseFormat, bool bForceAdd)
  {
    ExtendedFormatImpl format = baseFormat != null ? this.CreateExtFormatWithoutRegister(baseFormat) : throw new ArgumentNullException(nameof (baseFormat));
    if (bForceAdd)
      this.m_extFormats.ForceAdd(format);
    else
      format = this.m_extFormats.Add(format);
    return (IExtendedFormat) format;
  }

  internal bool IsEqualColor => this.isEqualColor;

  protected internal ExtendedFormatImpl CreateExtFormatWithoutRegister(IExtendedFormat baseFormat)
  {
    ShapeFillImpl shapeFillImpl = (ShapeFillImpl) null;
    ExtendedFormatImpl parent;
    ExtendedFormatRecord format;
    ExtendedXFRecord xfExt;
    switch (baseFormat)
    {
      case null:
        throw new ArgumentNullException(nameof (baseFormat));
      case ExtendedFormatImpl _:
        parent = (ExtendedFormatImpl) baseFormat;
        format = (ExtendedFormatRecord) parent.Record.Clone();
        xfExt = (ExtendedXFRecord) parent.XFRecord.Clone();
        break;
      case ExtendedFormatWrapper _:
        parent = ((ExtendedFormatWrapper) baseFormat).Wrapped;
        format = (ExtendedFormatRecord) parent.Record.Clone();
        xfExt = (ExtendedXFRecord) parent.XFRecord.Clone();
        break;
      default:
        throw new ArgumentException("baseFormat can be only ExtendedFormatImpl or ExtendedFormatImplWrapper classes");
    }
    if (parent.Gradient != null)
      shapeFillImpl = ((ShapeFillImpl) parent.Gradient).Clone((object) parent);
    ExtendedFormatImpl extendedFormatImpl1 = parent;
    ExtendedFormatImpl extendedFormatImpl2;
    ExtendedFormatImpl formatWithoutRegister = extendedFormatImpl2 = new ExtendedFormatImpl(this.Application, (object) this, format, xfExt);
    formatWithoutRegister.ColorObject.CopyFrom(extendedFormatImpl1.ColorObject, false);
    formatWithoutRegister.PatternColorObject.CopyFrom(extendedFormatImpl1.PatternColorObject, false);
    formatWithoutRegister.DiagonalBorderColor.CopyFrom(extendedFormatImpl1.DiagonalBorderColor, false);
    formatWithoutRegister.BottomBorderColor.CopyFrom(extendedFormatImpl1.BottomBorderColor, false);
    formatWithoutRegister.TopBorderColor.CopyFrom(extendedFormatImpl1.TopBorderColor, false);
    formatWithoutRegister.LeftBorderColor.CopyFrom(extendedFormatImpl1.LeftBorderColor, false);
    formatWithoutRegister.RightBorderColor.CopyFrom(extendedFormatImpl1.RightBorderColor, false);
    formatWithoutRegister.Gradient = (IGradient) shapeFillImpl;
    return formatWithoutRegister;
  }

  protected internal ExtendedFormatImpl RegisterExtFormat(ExtendedFormatImpl format)
  {
    return this.RegisterExtFormat(format, false);
  }

  protected internal ExtendedFormatImpl RegisterExtFormat(ExtendedFormatImpl format, bool forceAdd)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    format = forceAdd ? this.m_extFormats.ForceAdd(format) : this.m_extFormats.Add(format);
    return format;
  }

  protected internal void CopyToClipboard(WorksheetImpl sheet)
  {
    if (sheet == null)
      sheet = this.m_ActiveSheet as WorksheetImpl;
    this.AppImplementation.CreateClipboardProvider(sheet == null ? this.Worksheets[0] : (IWorksheet) sheet).SetClipboard();
  }

  protected internal void Paste()
  {
    IDataObject dataObject = Clipboard.GetDataObject();
    if (dataObject == null || !dataObject.GetDataPresent("Biff8", true))
      return;
    object data = dataObject.GetData("Biff8");
    if (data == null)
      return;
    BiffReader reader = new BiffReader((Stream) data);
    reader.SeekOnBOFRecord();
    this.Parse(reader);
  }

  internal int InsertSelfSupbook()
  {
    return this.m_externBooks != null ? this.m_externBooks.InsertSelfSupbook() : 0;
  }

  protected internal int AddSheetReference(string inputSheetName)
  {
    string[] strArray1 = inputSheetName.Split(':');
    string sheetName1 = strArray1[0];
    string sheetName2 = strArray1[strArray1.Length - 1];
    if (this.Worksheets != null)
    {
      IWorksheet worksheet1 = this.Worksheets[sheetName1];
      IWorksheet worksheet2 = this.Worksheets[sheetName2];
      if (worksheet1 != null && worksheet2 != null)
        return this.AddSheetReference(worksheet1, worksheet2);
    }
    Match match = WorkbookImpl.ExternSheetRegExComplete.Match(inputSheetName);
    if (!match.Success || !(match.Value == inputSheetName))
      return 0;
    string str1 = "";
    if (match.Groups["DirectoryName"].Value != null && match.Groups["DirectoryName"].Value != "")
      str1 = match.Groups["DirectoryName"].Value;
    string strBookName = match.Groups["BookName"].Value;
    int length = strBookName.Length;
    string str2 = (string) null;
    string str3 = this.IsCreated ? this.GetWorkbookName(this) : this.FullFileName;
    if (this.FullFileName != null)
    {
      string[] strArray2 = this.FullFileName.Split('/');
      string[] strArray3 = strArray2[strArray2.Length - 1].Split('\\');
      str2 = strArray3[strArray3.Length - 1];
    }
    int num;
    if (str2 == inputSheetName || str3 == inputSheetName)
      num = this.AddBrokenBookReference();
    else if (length == 0)
    {
      num = this.AddBrokenSheetReference();
    }
    else
    {
      if (length >= 2)
        strBookName = str1 + strBookName.Substring(1, length - 2);
      string strSheetName = match.Groups["SheetName"].Value;
      num = this.AddExternSheetReference(strBookName, strSheetName);
    }
    return num;
  }

  internal string GetFilePath(string strUrl)
  {
    string empty = string.Empty;
    if (string.IsNullOrEmpty(strUrl))
      return empty;
    string[] strArray1 = strUrl.Split('/');
    string[] strArray2 = strArray1[strArray1.Length - 1].Split('\\');
    string str = strArray2[strArray2.Length - 1];
    return strUrl.Substring(0, strUrl.Length - str.Length);
  }

  internal string GetWorkbookName(WorkbookImpl workbook)
  {
    int num1 = (workbook.Application.Workbooks as WorkbooksCollection).IndexOf((IWorkbook) workbook);
    int num2 = 0;
    for (int Index = num1 - 1; Index >= 0; --Index)
    {
      if (!(workbook.Application.Workbooks[Index] as WorkbookImpl).IsCreated)
        ++num2;
    }
    return "Book" + (num1 - num2 + 1).ToString();
  }

  internal string GetFileName(string url)
  {
    string empty = string.Empty;
    if (url != null)
    {
      string[] strArray1 = url.Split('/');
      string[] strArray2 = strArray1[strArray1.Length - 1].Split('\\');
      empty = strArray2[strArray2.Length - 1];
    }
    return empty;
  }

  private int AddExternSheetReference(string strBookName, string strSheetName)
  {
    if (strBookName == null)
      throw new ArgumentNullException(nameof (strBookName));
    if (strSheetName == null)
      throw new ArgumentNullException(nameof (strSheetName));
    int num1 = 65534;
    Match match = WorkbookImpl.ExternSheetRegExComplete.Match(strBookName);
    if (strBookName == null || strBookName.Length == 0)
    {
      strBookName = strSheetName;
      strSheetName = (string) null;
    }
    ExternWorkbookImpl externWorkbookImpl = this.m_externBooks[strBookName];
    int num2;
    if (externWorkbookImpl == null)
    {
      int result;
      if (this.Loading && this.Version != ExcelVersion.Excel97to2003 && int.TryParse(strBookName, out result))
      {
        if (result == 0)
          return 0;
        num2 = result - 1;
        externWorkbookImpl = this.m_externBooks[num2];
      }
      else
      {
        if (strBookName.Contains("\\"))
        {
          externWorkbookImpl = this.m_externBooks[strBookName.Replace("\\", "/")];
          if (match.Success)
          {
            foreach (ExternWorkbookImpl externBook in (Syncfusion.XlsIO.Implementation.Collections.CollectionBase<ExternWorkbookImpl>) this.m_externBooks)
            {
              if (externBook.URL != null && externBook.URL.EndsWith(match.Groups["BookName"].Value))
              {
                externWorkbookImpl = externBook;
                break;
              }
            }
          }
        }
        else if (strBookName.Contains("/"))
        {
          externWorkbookImpl = this.m_externBooks[strBookName.Replace("/", "\\")];
        }
        else
        {
          foreach (ExternWorkbookImpl externBook in (Syncfusion.XlsIO.Implementation.Collections.CollectionBase<ExternWorkbookImpl>) this.m_externBooks)
          {
            if (externBook.URL.Contains(strBookName))
            {
              externWorkbookImpl = externBook;
              break;
            }
          }
        }
        num2 = externWorkbookImpl != null ? externWorkbookImpl.Index : throw new ArgumentNullException("Can't find extern workbook");
      }
    }
    else
      num2 = externWorkbookImpl.Index;
    if (strSheetName != null)
      num1 = externWorkbookImpl.IndexOf(strSheetName);
    return this.AddSheetReference(num2, num1, num1);
  }

  protected internal int AddSheetReference(IWorksheet sheet)
  {
    return this.AddSheetReference(sheet, sheet);
  }

  protected internal int AddSheetReference(IWorksheet sheet, IWorksheet lastSheet)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (sheet.Workbook != this)
      throw new ArgumentException("Can't refer to external worksheets");
    return this.m_externSheet.AddReference(this.InsertSelfSupbook(), ((ISerializableNamedObject) sheet).RealIndex, ((ISerializableNamedObject) lastSheet).RealIndex);
  }

  protected internal int AddSheetReference(ITabSheet sheet)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (sheet.Workbook != this)
      throw new ArgumentException("Can't refer to external worksheets");
    if (!(sheet is IWorksheet))
      return -1;
    int supIndex = this.InsertSelfSupbook();
    int realIndex = ((ISerializableNamedObject) sheet).RealIndex;
    return this.m_externSheet.AddReference(supIndex, realIndex, realIndex);
  }

  protected internal int AddSheetReference(int supIndex, int firstSheetIndex, int lastSheetIndex)
  {
    return this.m_externSheet.AddReference(supIndex, firstSheetIndex, lastSheetIndex);
  }

  protected internal int AddBrokenSheetReference()
  {
    int supIndex = this.InsertSelfSupbook();
    return this.m_externSheet != null ? this.m_externSheet.AddReference(supIndex, (int) ushort.MaxValue, (int) ushort.MaxValue) : 0;
  }

  internal int AddBrokenBookReference()
  {
    return this.m_externSheet.AddReference(this.InsertSelfSupbook(), 65534, 65534);
  }

  protected internal void DecreaseSheetIndex(int index)
  {
    if (this.m_externSheet == null || this.m_externSheet.Refs == null)
      return;
    int firstInternalIndex = this.m_externBooks.GetFirstInternalIndex();
    ExternSheetRecord.TREF[] refs = this.m_externSheet.Refs;
    int index1 = 0;
    for (int length = refs.Length; index1 < length; ++index1)
    {
      ExternSheetRecord.TREF tref = refs[index1];
      if ((int) tref.SupBookIndex == firstInternalIndex)
      {
        int firstSheet = (int) tref.FirstSheet;
        if (firstSheet > index && firstSheet != (int) ushort.MaxValue && firstSheet != 65534)
          --tref.FirstSheet;
        else if (firstSheet == index && this.Application.UpdateSheetFormulaReference)
          tref.FirstSheet = ushort.MaxValue;
        int lastSheet = (int) tref.LastSheet;
        if (lastSheet > index && lastSheet != (int) ushort.MaxValue && lastSheet != 65534)
          --tref.LastSheet;
        else if (lastSheet == index && this.Application.UpdateSheetFormulaReference)
          tref.LastSheet = ushort.MaxValue;
      }
    }
  }

  protected internal void IncreaseSheetIndex(int index)
  {
    if (this.m_externSheet == null || this.m_externSheet.Refs == null)
      return;
    ExternSheetRecord.TREF[] refs = this.m_externSheet.Refs;
    int index1 = 0;
    for (int length = refs.Length; index1 < length; ++index1)
    {
      ExternSheetRecord.TREF tref = refs[index1];
      if ((int) tref.FirstSheet >= index)
        ++tref.FirstSheet;
      if ((int) tref.LastSheet >= index)
        ++tref.LastSheet;
    }
  }

  protected internal void MoveSheetIndex(int iOldIndex, int iNewIndex)
  {
    if (this.m_externSheet == null || this.m_externSheet.Refs == null || iOldIndex == iNewIndex)
      return;
    ExternSheetRecord.TREF[] refs = this.m_externSheet.Refs;
    int reference = 0;
    for (int length = refs.Length; reference < length; ++reference)
    {
      if (this.IsLocalReference(reference))
      {
        ExternSheetRecord.TREF tref = refs[reference];
        tref.FirstSheet = (ushort) this.GetMovedSheetIndex((int) tref.FirstSheet, iOldIndex, iNewIndex);
        tref.LastSheet = (ushort) this.GetMovedSheetIndex((int) tref.LastSheet, iOldIndex, iNewIndex);
      }
    }
  }

  protected internal void UpdateActiveSheetAfterMove(int iOldIndex, int iNewIndex)
  {
    int num = this.ActiveSheetIndex;
    if (iOldIndex == num)
      num = iNewIndex;
    else if (iOldIndex < iNewIndex)
    {
      if (num < iOldIndex && num >= iNewIndex)
        ++num;
    }
    else if (num <= iNewIndex && num > iOldIndex)
      --num;
    this.ActiveSheetIndex = num;
  }

  private int GetMovedSheetIndex(int iCurIndex, int iOldIndex, int iNewIndex)
  {
    if (iOldIndex == iNewIndex)
      return iCurIndex;
    if (iCurIndex == iOldIndex)
      return iNewIndex;
    int num1 = Math.Min(iOldIndex, iNewIndex);
    int num2 = Math.Max(iOldIndex, iNewIndex);
    if (iCurIndex < num1 || iCurIndex > num2)
      return iCurIndex;
    return iOldIndex > iNewIndex ? iCurIndex + 1 : iCurIndex - 1;
  }

  protected internal string GetSheetNameByReference(int reference)
  {
    return this.GetSheetNameByReference(reference, false);
  }

  protected internal string GetSheetNameByReference(int reference, bool completePath)
  {
    return this.GetSheetNameByReference(reference, false, completePath);
  }

  internal string GetSheetNameByReference(
    int reference,
    bool throwArgumentOutOfRange,
    bool completePath)
  {
    string sheetNameByReference = (string) null;
    if ((int) this.m_externSheet.RefCount <= reference || reference < 0)
    {
      if (throwArgumentOutOfRange)
        throw new ArgumentOutOfRangeException(nameof (reference));
      return (string) null;
    }
    ExternSheetRecord.TREF reference1 = this.m_externSheet.Refs[reference];
    int supBookIndex = (int) reference1.SupBookIndex;
    if (supBookIndex > this.m_externBooks.Count)
      throw new ParseException();
    ExternWorkbookImpl externBook = this.m_externBooks[supBookIndex];
    try
    {
      sheetNameByReference = !externBook.IsInternalReference ? this.GetExternalSheetNameByReference(externBook, reference1, supBookIndex, completePath) : this.GetInternalSheetNameByReference(reference1);
    }
    catch (Exception ex)
    {
    }
    return sheetNameByReference;
  }

  private string GetExternalSheetNameByReference(
    ExternWorkbookImpl book,
    ExternSheetRecord.TREF reference,
    int iSupBook,
    bool completePath)
  {
    int firstSheet = (int) reference.FirstSheet;
    string directoryName = this.GetDirectoryName(book.URL);
    string fileName = Path.GetFileName(book.URL);
    string str;
    if (this.Saving)
    {
      int num = 0;
      for (int index = iSupBook - 1; index >= 0; --index)
      {
        ExternWorkbookImpl externBook = this.m_externBooks[index];
        if (externBook.IsInternalReference || string.IsNullOrEmpty(externBook.URL))
          ++num;
      }
      str = $"[{iSupBook - num + 1}]";
    }
    else if (firstSheet == 65534)
      str = directoryName + fileName;
    else
      str = directoryName + (object) '[' + fileName + (object) ']';
    return completePath ? str + book.GetSheetName(firstSheet) : book.GetSheetName(firstSheet);
  }

  private string GetInternalSheetNameByReference(ExternSheetRecord.TREF reference)
  {
    string sheetNameByReference = (string) null;
    int firstSheet = (int) reference.FirstSheet;
    switch (firstSheet)
    {
      case 65534:
      case (int) ushort.MaxValue:
        sheetNameByReference = "#REF";
        break;
      default:
        object obj1 = this.ObjectCount > firstSheet && firstSheet >= 0 ? (object) this.Objects[firstSheet] : throw new ParseException();
        if (obj1 is IWorksheet)
          sheetNameByReference = ((ITabSheet) obj1).Name;
        if ((int) reference.FirstSheet != (int) reference.LastSheet)
        {
          object obj2 = (object) this.Objects[(int) reference.LastSheet];
          if (obj2 is IWorksheet)
          {
            sheetNameByReference = $"{sheetNameByReference}:{((ITabSheet) obj2).Name}";
            break;
          }
          break;
        }
        break;
    }
    return sheetNameByReference;
  }

  private string GetDirectoryName(string url)
  {
    if (url == null)
      return (string) null;
    string directoryName;
    if (url.StartsWith("http://") || url.StartsWith("https://"))
    {
      int num = url.LastIndexOf('/');
      directoryName = url.Substring(0, num + 1);
    }
    else
    {
      Match match = Regex.Match(url, "(?<DirectoryName>[\\S ]*\\\\)?");
      directoryName = !match.Success || match.Groups["DirectoryName"].Value == null || !(match.Groups["DirectoryName"].Value != "") ? Path.GetDirectoryName(url) : match.Groups["DirectoryName"].Value;
      if (directoryName != null && directoryName.Length > 0 && directoryName[directoryName.Length - 1] != '\\')
        directoryName += (string) (object) '\\';
    }
    return directoryName;
  }

  protected internal IWorksheet GetSheetByReference(int reference)
  {
    return this.GetSheetByReference(reference, true);
  }

  protected internal IWorksheet GetSheetByReference(int reference, bool bThrowExceptions)
  {
    if ((int) this.m_externSheet.RefCount <= reference || reference < 0)
    {
      if (bThrowExceptions)
        throw new ArgumentOutOfRangeException(nameof (reference));
      return (IWorksheet) null;
    }
    ExternSheetRecord.TREF tref = this.m_externSheet.Refs[reference];
    int supBookIndex = (int) tref.SupBookIndex;
    if (supBookIndex > this.m_externBooks.Count)
    {
      if (bThrowExceptions)
        throw new ParseException();
      return (IWorksheet) null;
    }
    if (!this.m_externBooks[supBookIndex].IsInternalReference)
    {
      if (bThrowExceptions)
        throw new ParseException();
      return (IWorksheet) null;
    }
    int firstSheet = (int) tref.FirstSheet;
    if (this.ObjectCount <= firstSheet || firstSheet < 0)
    {
      if (bThrowExceptions)
        throw new ParseException();
      return (IWorksheet) null;
    }
    object sheetByReference = (object) this.Objects[firstSheet];
    if (sheetByReference is IWorksheet)
      return (IWorksheet) sheetByReference;
    if (bThrowExceptions)
      throw new ArgumentOutOfRangeException("Can't find worksheet at the specified index");
    return (IWorksheet) null;
  }

  protected internal void CheckForInternalReference(int iRef)
  {
    if ((int) this.m_externSheet.RefCount <= iRef || iRef < 0)
      throw new ArgumentOutOfRangeException(nameof (iRef));
    int supBookIndex = (int) this.m_externSheet.Refs[iRef].SupBookIndex;
    ExternSheetRecord.TREF tref = this.m_externSheet.Refs[iRef];
    if (supBookIndex > this.m_externBooks.Count)
      throw new ParseException();
    if (!this.m_externBooks[supBookIndex].IsInternalReference)
      throw new NotSupportedException("External indexes are not supported in current version.");
  }

  protected internal bool IsLocalReference(int reference)
  {
    if ((int) this.m_externSheet.RefCount <= reference || reference < 0)
      return false;
    int supBookIndex = (int) this.m_externSheet.Refs[reference].SupBookIndex;
    return supBookIndex <= this.m_externBooks.Count && this.m_externBooks[supBookIndex].IsInternalReference;
  }

  public bool IsExternalReference(int reference)
  {
    if (reference == (int) ushort.MaxValue || this.m_externSheet.RefCount < (ushort) 0 || (int) this.m_externSheet.RefCount <= reference)
      return false;
    ExternSheetRecord.TREF tref = this.m_externSheet.Refs[reference];
    if (tref.FirstSheet == ushort.MaxValue)
      return false;
    int supBookIndex = (int) tref.SupBookIndex;
    if (supBookIndex < 0 || supBookIndex >= this.m_externBooks.Count)
      throw new ArgumentOutOfRangeException("supbookIndex");
    return !this.m_externBooks[supBookIndex].IsInternalReference;
  }

  internal void AddForReparse(IReparse reparse) => this.m_arrReparse.Add(reparse);

  protected internal int CurrentStyleNumber(string pre)
  {
    int num1 = 0;
    IStyles styles = this.Styles;
    int Index = 0;
    for (int count = styles.Count; Index < count; ++Index)
    {
      string name = styles[Index].Name;
      int num2 = name.IndexOf(pre);
      double result;
      if (num2 >= 0 && double.TryParse(name.Substring(num2 + pre.Length, name.Length - pre.Length - num2), NumberStyles.Integer, (IFormatProvider) null, out result))
      {
        int num3 = (int) result;
        if (num3 > num1)
          num1 = num3;
      }
    }
    return num1;
  }

  protected double Sqr(double value) => value * value;

  protected internal double ColorDistance(Color color1, Color color2)
  {
    return Math.Sqrt(this.Sqr((double) ((int) color1.R - (int) color2.R)) + this.Sqr((double) ((int) color1.B - (int) color2.B)) + this.Sqr((double) ((int) color1.G - (int) color2.G)));
  }

  public void ClearInternalReferences() => this.m_externSheet.Refs = new ExternSheetRecord.TREF[0];

  private void RaiseSavedEvent()
  {
    if (this.OnFileSaved == null)
      return;
    this.OnFileSaved((object) this, EventArgs.Empty);
  }

  private void RaiseReadOnlyFileEvent(string strFullPath)
  {
    if (this.OnReadOnlyFile == null)
      throw new ApplicationException($"File {strFullPath} is read-only");
    ReadOnlyFileEventArgs e = new ReadOnlyFileEventArgs();
    this.OnReadOnlyFile((object) this, e);
    if (!e.ShouldRewrite)
      return;
    FileAttributes fileAttributes = File.GetAttributes(strFullPath) & ~FileAttributes.ReadOnly;
    File.SetAttributes(strFullPath, fileAttributes);
  }

  public IExtendedFormat GetExtFormat(int index) => (IExtendedFormat) this.m_extFormats[index];

  public void UpdateFormula(IRange sourceRange, IRange destRange)
  {
    if (sourceRange == null)
      throw new ArgumentNullException(nameof (sourceRange));
    if (destRange == null)
      throw new ArgumentNullException(nameof (destRange));
    RangeImpl rangeImpl1 = (RangeImpl) sourceRange;
    RangeImpl rangeImpl2 = (RangeImpl) destRange;
    WorksheetImpl innerWorksheet1 = rangeImpl1.InnerWorksheet;
    WorksheetImpl innerWorksheet2 = rangeImpl2.InnerWorksheet;
    int iSourceIndex = this.AddSheetReference((IWorksheet) innerWorksheet1);
    int iDestIndex = this.AddSheetReference((IWorksheet) innerWorksheet2);
    Rectangle rectSource = Rectangle.FromLTRB(rangeImpl1.FirstColumn - 1, rangeImpl1.FirstRow - 1, rangeImpl1.LastColumn - 1, rangeImpl1.LastRow - 1);
    Rectangle rectDest = Rectangle.FromLTRB(rangeImpl2.FirstColumn - 1, rangeImpl2.FirstRow - 1, rangeImpl2.LastColumn - 1, rangeImpl2.LastRow - 1);
    this.UpdateFormula(iSourceIndex, rectSource, iDestIndex, rectDest);
  }

  public void UpdateFormula(
    int iSourceIndex,
    Rectangle rectSource,
    int iDestIndex,
    Rectangle rectDest)
  {
    int index = 0;
    for (int count = this.m_arrObjects.Count; index < count; ++index)
    {
      WorksheetBaseImpl arrObject = this.m_arrObjects[index] as WorksheetBaseImpl;
      int iCurIndex = this.AddSheetReference((ITabSheet) arrObject);
      arrObject.UpdateFormula(iCurIndex, iSourceIndex, rectSource, iDestIndex, rectDest);
    }
  }

  public int GetReferenceIndex(int iNameBookIndex)
  {
    return this.m_externSheet.GetBookReference(iNameBookIndex);
  }

  public int GetBookIndex(int iReferenceIndex)
  {
    ExternSheetRecord.TREF[] refs = this.m_externSheet.Refs;
    if (iReferenceIndex < 0 || iReferenceIndex > refs.Length - 1)
      throw new ArgumentOutOfRangeException(nameof (iReferenceIndex), "Value cannot be less than 0 and greater than arrRefs.Count - 1");
    return (int) this.m_externSheet.Refs[iReferenceIndex].SupBookIndex;
  }

  public ExternWorksheetImpl GetExternSheet(int referenceIndex)
  {
    ExternSheetRecord.TREF[] refs = this.m_externSheet.Refs;
    if (referenceIndex < 0 || referenceIndex > refs.Length - 1)
      throw new ArgumentOutOfRangeException(nameof (referenceIndex), "Value cannot be less than 0 and greater than arrRefs.Count - 1");
    ExternWorksheetImpl externSheet = (ExternWorksheetImpl) null;
    ExternSheetRecord.TREF tref = this.m_externSheet.Refs[referenceIndex];
    int firstSheet;
    if ((firstSheet = (int) tref.FirstSheet) == (int) tref.LastSheet && firstSheet != (int) ushort.MaxValue)
      externSheet = this.m_externBooks[(int) tref.SupBookIndex].Worksheets.Values[firstSheet];
    return externSheet;
  }

  public string DecodeName(string strName)
  {
    switch (strName)
    {
      case null:
        throw new ArgumentNullException(nameof (strName));
      case "":
        throw new ArgumentException("strName - string cannot be empty");
      default:
        switch (strName[0])
        {
          case char.MinValue:
            return string.Empty;
          case '\u0001':
            StringBuilder stringBuilder = new StringBuilder();
            int length1 = strName.Length;
            int index = 1;
            char ch1 = this.m_strFullName != null ? this.m_strFullName[0] : WorkbookImpl.GetDriveName();
            char ch2 = Path.DirectorySeparatorChar;
            for (; index < length1; ++index)
            {
              char ch3 = strName[index];
              if (stringBuilder.Length == 5 && stringBuilder.ToString() == "http:")
                ch2 = '/';
              switch (ch3)
              {
                case '\u0001':
                  ++index;
                  if (strName[index] != '@')
                  {
                    stringBuilder.Append(strName[index]);
                    stringBuilder.Append(Path.VolumeSeparatorChar);
                    stringBuilder.Append(ch2);
                    break;
                  }
                  stringBuilder.Append("\\\\");
                  break;
                case '\u0002':
                  stringBuilder.Append(ch1);
                  stringBuilder.Append(Path.VolumeSeparatorChar);
                  stringBuilder.Append(ch2);
                  break;
                case '\u0003':
                  stringBuilder.Append(ch2);
                  break;
                case '\u0004':
                  throw new NotImplementedException();
                case '\u0005':
                  int length2 = (int) strName[index + 1];
                  stringBuilder.Append(strName.Substring(index + 2, length2));
                  index += length2;
                  break;
                case '\u0006':
                  stringBuilder.Append('\\');
                  break;
                case '\a':
                  stringBuilder.Append(strName[index]);
                  break;
                case '\b':
                  stringBuilder.Append(strName[index]);
                  break;
                default:
                  stringBuilder.Append(strName[index]);
                  break;
              }
            }
            return stringBuilder.ToString();
          case '\u0002':
            return strName;
          default:
            return strName.Replace('\u0003', '|');
        }
    }
  }

  private static char GetDriveName() => Environment.CurrentDirectory[0];

  public string EncodeName(string strName)
  {
    if (strName == null || strName.Length == 0)
      return strName;
    if (strName.IndexOfAny(WorkbookImpl.DEF_RESERVED_BOOK_CHARS) != -1 || strName == " ")
      return strName.Replace('|', '\u0003');
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append('\u0001');
    bool flag = strName.StartsWith("http:");
    int startIndex = 0;
    int length1 = strName != null ? strName.Length : 0;
    if (strName.StartsWith("\\\\"))
    {
      stringBuilder.Append('\u0001');
      stringBuilder.Append('@');
      startIndex = "\\\\".Length;
    }
    else if (flag)
    {
      stringBuilder.Append('\u0005');
      stringBuilder.Append((char) strName.Length);
      stringBuilder.Append(strName);
    }
    else if (length1 > 2 && strName[2] == '\\')
    {
      stringBuilder.Append('\u0001');
      char ch = strName[0];
      stringBuilder.Append(ch);
      startIndex = 3;
    }
    else if (strName[0] == '\\')
    {
      stringBuilder.Append('\u0006');
      strName = UtilityMethods.RemoveFirstCharUnsafe(strName);
    }
    if (!flag)
    {
      int length2 = strName.Length;
      strName = strName.Substring(startIndex);
      string[] strArray = strName.Split(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
      int length3 = strArray.Length;
      for (int index = 0; index < length3; ++index)
      {
        stringBuilder.Append(strArray[index]);
        if (index != length3 - 1)
          stringBuilder.Append('\u0003');
      }
    }
    return stringBuilder.ToString();
  }

  [CLSCompliant(false)]
  public bool ModifyRecordToSkipStyle(BiffRecordRaw record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    FontRecord record1 = ((FontImpl) this.InnerFonts[0]).Record;
    switch (record.TypeCode)
    {
      case TBIFFRecord.ChartFontx:
        ((ChartFontxRecord) record).FontIndex = (ushort) 0;
        break;
      case TBIFFRecord.ChartAlruns:
        ChartAlrunsRecord.TRuns[] runs = ((ChartAlrunsRecord) record).Runs;
        int index = 0;
        for (int length = runs.Length; index < length; ++index)
          runs[index].FontIndex = (ushort) 0;
        break;
      case TBIFFRecord.ChartFbi:
        ChartFbiRecord chartFbiRecord = (ChartFbiRecord) record;
        chartFbiRecord.FontIndex = (ushort) 0;
        chartFbiRecord.AppliedFontHeight = record1.FontHeight;
        return false;
    }
    return true;
  }

  [CLSCompliant(false)]
  public void ModifyRecordToSkipStyle(BiffRecordRaw[] arrRecords)
  {
    if (arrRecords == null)
      throw new ArgumentNullException(nameof (arrRecords));
    FontRecord record = ((FontImpl) this.m_fonts[0]).Record;
    int index1 = 0;
    for (int length1 = arrRecords.Length; index1 < length1; ++index1)
    {
      BiffRecordRaw arrRecord = arrRecords[index1];
      switch (arrRecord.TypeCode)
      {
        case TBIFFRecord.ChartFontx:
          ((ChartFontxRecord) arrRecord).FontIndex = (ushort) 0;
          break;
        case TBIFFRecord.ChartAlruns:
          ChartAlrunsRecord.TRuns[] runs = ((ChartAlrunsRecord) arrRecord).Runs;
          int index2 = 0;
          for (int length2 = runs.Length; index2 < length2; ++index2)
            runs[index2].FontIndex = (ushort) 0;
          break;
        case TBIFFRecord.ChartFbi:
          ChartFbiRecord chartFbiRecord = (ChartFbiRecord) arrRecord;
          chartFbiRecord.FontIndex = (ushort) 0;
          chartFbiRecord.AppliedFontHeight = record.FontHeight;
          break;
      }
    }
  }

  private bool CompareColors(Color color1, Color color2)
  {
    return (int) color1.R == (int) color2.R && (int) color1.G == (int) color2.G && (int) color1.B == (int) color2.B;
  }

  [Obsolete("This method is obsolete and will be removed soon. Please use RemoveExtendedFormatIndex(int xfIndex) method. Sorry for inconvenience.")]
  public void RemoveExtenededFormatIndex(int xfIndex)
  {
    Dictionary<int, int> dictFormats = this.m_extFormats.RemoveAt(xfIndex);
    int index = 0;
    for (int count = this.m_arrObjects.Count; index < count; ++index)
      (this.m_arrObjects[index] as WorksheetBaseImpl).UpdateExtendedFormatIndex(dictFormats);
    this.m_styles.UpdateStyleRecords();
  }

  public void RemoveExtendedFormatIndex(int xfIndex)
  {
    Dictionary<int, int> dictFormats = this.m_extFormats.RemoveAt(xfIndex);
    int index = 0;
    for (int count = this.m_arrObjects.Count; index < count; ++index)
      (this.m_arrObjects[index] as WorksheetBaseImpl).UpdateExtendedFormatIndex(dictFormats);
    this.m_styles.UpdateStyleRecords();
  }

  private bool CheckProtectionContent(IWorksheet sheet)
  {
    IRange range = sheet["A1"];
    return range.Text == "Created with a trial version of Syncfusion Essential XlsIO" && range.ColumnWidth > 8.0 && range.RowHeight > 10.0 && range.CellStyle.Font.Size > 10.0 && sheet.TopVisibleRow == 1 && sheet.LeftVisibleColumn == 1 && sheet.Visibility == WorksheetVisibility.Visible;
  }

  private void OptimizeReferences()
  {
    int refCount = (int) this.m_externSheet.RefCount;
    bool[] usedItems = new bool[refCount];
    int index1 = 0;
    for (int count = this.m_arrObjects.Count; index1 < count; ++index1)
      (this.m_arrObjects[index1] as WorksheetBaseImpl).MarkUsedReferences(usedItems);
    this.m_names.MarkUsedReferences(usedItems);
    int[] arrUpdatedIndexes = new int[refCount];
    int num = 0;
    for (int index2 = 0; index2 < refCount; ++index2)
    {
      if (usedItems[index2])
      {
        arrUpdatedIndexes[index2] = index2 - num;
      }
      else
      {
        arrUpdatedIndexes[index2] = -1;
        ++num;
      }
    }
    this.UpdateReferenceIndexes(arrUpdatedIndexes);
  }

  private void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    int index1 = 0;
    for (int count = this.m_arrObjects.Count; index1 < count; ++index1)
      (this.m_arrObjects[index1] as WorksheetBaseImpl).UpdateReferenceIndexes(arrUpdatedIndexes);
    this.m_names.UpdateReferenceIndexes(arrUpdatedIndexes);
    ExternSheetRecord.TREF[] refs = this.m_externSheet.Refs;
    List<ExternSheetRecord.TREF> trefList = new List<ExternSheetRecord.TREF>();
    int index2 = 0;
    for (int length = refs.Length; index2 < length; ++index2)
    {
      if (arrUpdatedIndexes[index2] >= 0)
        trefList.Add(refs[index2]);
    }
    this.m_externSheet.Refs = trefList.ToArray();
  }

  public void UpdatePivotCachesAfterInsertRemove(
    WorksheetImpl worksheet,
    int index,
    int count,
    bool isRow,
    bool isRemove)
  {
    if (this.m_pivotCaches == null || this.m_pivotCaches.Count <= 0)
      return;
    foreach (PivotCacheImpl pivotCach in this.m_pivotCaches)
      pivotCach.UpdateAfterInsertRemove(worksheet, index, count, isRow, isRemove);
  }

  internal Stack<object> GetStackOfRange(Ptg[] arrPtgs, WorksheetImpl sheet)
  {
    Stack<object> stackOfRange = new Stack<object>();
    List<IRange> collection = new List<IRange>();
    int num = 0;
    int index = 0;
    for (int length = arrPtgs.Length; index < length; ++index)
    {
      if (arrPtgs[index] is IRangeGetter)
      {
        List<IRange> rangeList = collection ?? new List<IRange>();
        IRange range = ((IRangeGetter) arrPtgs[index]).GetRange((IWorkbook) this, (IWorksheet) sheet);
        rangeList.Add(range);
        stackOfRange.Push((object) rangeList);
        ++num;
        collection = (List<IRange>) null;
      }
      else if (arrPtgs[index].TokenCode == FormulaToken.tCellRangeList && num > 0)
      {
        collection = (List<IRange>) stackOfRange.Pop();
        if (stackOfRange.Count > 0)
          ((List<IRange>) stackOfRange.Peek()).AddRange((IEnumerable<IRange>) collection);
        collection.Clear();
        --num;
      }
    }
    return stackOfRange;
  }

  internal void InitializePrecedentsCache()
  {
    this.m_precedentsCache = new Dictionary<string, List<string>>();
  }

  static WorkbookImpl()
  {
    WorkbookImpl.SheetTypeToName.Add(ExcelSheetType.Chart, nameof (Charts));
    WorkbookImpl.SheetTypeToName.Add(ExcelSheetType.DialogSheet, "Dialogs");
    WorkbookImpl.SheetTypeToName.Add(ExcelSheetType.Excel4IntlMacroSheet, "Excel 4.0 Intl Marcos");
    WorkbookImpl.SheetTypeToName.Add(ExcelSheetType.Excel4MacroSheet, "Excel 4.0 Macros");
    WorkbookImpl.SheetTypeToName.Add(ExcelSheetType.Worksheet, nameof (Worksheets));
  }

  public WorkbookImpl(IApplication application, object parent, ExcelVersion version)
    : this(application, parent, application.SheetsInNewWorkbook, version)
  {
  }

  public WorkbookImpl(
    IApplication application,
    object parent,
    int sheetQuantity,
    ExcelVersion version)
    : base(application, parent)
  {
    this.InitializeCollections();
    this.Version = version;
    this.InsertDefaultFonts();
    this.InsertDefaultValues();
    this.m_bReadOnly = false;
    this.m_bIsCreated = true;
    if (sheetQuantity <= 0)
      return;
    int size = sheetQuantity;
    this.m_worksheets.EnsureCapacity(size);
    for (int index = 0; index < size; ++index)
      this.m_worksheets.Add($"Sheet{index + 1}");
    this.m_worksheets[0].Activate();
  }

  public WorkbookImpl(
    IApplication application,
    object parent,
    string FileName,
    ExcelVersion version)
    : this(application, parent, FileName, ExcelParseOptions.Default, version)
  {
  }

  public WorkbookImpl(
    IApplication application,
    object parent,
    string strFileName,
    ExcelParseOptions options,
    ExcelVersion version)
    : this(application, parent, strFileName, options, false, (string) null, version)
  {
  }

  public WorkbookImpl(
    IApplication application,
    object parent,
    string strFileName,
    ExcelParseOptions options,
    bool bReadOnly,
    string password,
    ExcelVersion version)
    : base(application, parent)
  {
    this.m_bOptimization = application.OptimizeFonts;
    this.InitializeCollections();
    this.Version = version;
    this.ParseFile(strFileName, password, version, options, bReadOnly);
    this.m_strFullName = Path.GetFullPath(strFileName);
  }

  public WorkbookImpl(
    IApplication application,
    object parent,
    Stream stream,
    ExcelParseOptions options,
    bool bReadOnly,
    string password,
    ExcelVersion version)
    : base(application, parent)
  {
    this.m_bOptimization = application.OptimizeFonts;
    this.InitializeCollections();
    this.Version = version;
    this.ParseStream(stream, password, version, options);
  }

  public WorkbookImpl(
    IApplication application,
    object parent,
    Stream stream,
    string separator,
    int row,
    int column,
    ExcelVersion version,
    string fileName,
    Encoding encoding)
    : this(application, parent, 1, version)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    switch (separator)
    {
      case null:
        throw new ArgumentNullException(nameof (separator));
      case "":
        throw new ArgumentException(nameof (separator));
      default:
        if (encoding == null)
          encoding = Encoding.Default;
        this.m_bIsLoaded = true;
        StreamReader streamToRead = new StreamReader(stream, encoding);
        this.Loading = true;
        this.ThrowOnUnknownNames = false;
        if (this.m_ActiveSheet != null)
        {
          ((WorksheetImpl) this.m_ActiveSheet).Parse((TextReader) streamToRead, separator, row, column, true);
          if (fileName != null && fileName.Length > 0)
            this.m_ActiveSheet.Name = Path.GetFileNameWithoutExtension(fileName);
        }
        this.ThrowOnUnknownNames = true;
        this.Loading = false;
        break;
    }
  }

  public WorkbookImpl(
    IApplication application,
    object parent,
    Stream stream,
    ExcelVersion version)
    : this(application, parent, stream, ExcelParseOptions.Default, version)
  {
  }

  public WorkbookImpl(
    IApplication application,
    object parent,
    Stream stream,
    ExcelParseOptions options,
    ExcelVersion version)
    : base(application, parent)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (stream is FileStream)
      this.FullFileName = (stream as FileStream).Name;
    this.m_bOptimization = application.OptimizeFonts;
    this.InitializeCollections();
    this.Version = version;
    this.ParseStream(stream, (string) null, version, options);
  }

  public WorkbookImpl(
    IApplication application,
    object parent,
    XmlReader reader,
    ExcelXmlOpenType openType)
    : base(application, parent)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    this.InitializeCollections();
    this.Version = application.DefaultVersion;
    this.InsertDefaultFonts();
    this.InsertDefaultValues();
    this.m_bReadOnly = false;
    if (openType != ExcelXmlOpenType.MSExcel)
      throw new ArgumentOutOfRangeException("cannot specified xml open type.");
    this.Loading = true;
    MSXmlReader msXmlReader = new MSXmlReader(this.Application, (object) this);
    bool bThrowInFormula = this.m_bThrowInFormula;
    this.m_bThrowInFormula = false;
    msXmlReader.FillWorkbook(reader, this);
    this.m_bThrowInFormula = bThrowInFormula;
    this.Loading = false;
  }

  protected void InitializeCollections()
  {
    this.CreateGraphics();
    this.m_arrObjects = new WorkbookObjectsCollection(this.Application, (object) this);
    this.m_worksheets = new WorksheetsCollection(this.Application, (object) this);
    this.m_styles = new StylesCollection(this.Application, (object) this);
    this.m_colors = new List<Color>((IEnumerable<Color>) WorkbookImpl.DEF_PALETTE);
    this.m_names = new WorkbookNamesCollection(this.Application, (object) this);
    this.m_charts = new ChartsCollection(this.Application, (object) this);
    this.m_SSTDictionary = new SSTDictionary(this);
    this.m_fonts = new FontsCollection(this.Application, (object) this);
    this.m_externBooks = new ExternBookCollection(this.Application, (object) this);
    this.m_addinFunctions = new AddInFunctionsCollection(this.Application, (object) this);
    this.m_calcution = new CalculationOptionsImpl(this.Application, (object) this);
    this.m_extFormats = new ExtendedFormatsCollection(this.Application, (object) this);
    this.m_shapesData = new WorkbookShapeDataImpl(this.Application, (object) this, new WorkbookImpl.ShapesGetterMethod(this.GetWorksheetShapes));
    this.m_customDocumentProperties = new Syncfusion.XlsIO.Implementation.Collections.CustomDocumentProperties(this.Application, (object) this);
    this.m_builtInDocumentProperties = new Syncfusion.XlsIO.Implementation.Collections.BuiltInDocumentProperties(this.Application, (object) this);
    this.m_contentTypeProperties = new MetaPropertiesImpl(this.Application, (object) this);
    this.m_customXmlPartCollection = new CustomXmlPartCollection(this.Application, (object) this);
    this.m_connections = new ExternalConnectionCollection(this.Application, (object) this);
    string str = "";
    if (ExcelEngine.IsSecurityGranted)
      str = Environment.UserName;
    this.m_builtInDocumentProperties[ExcelBuiltInProperty.Author].Text = str;
    this.m_arrNames = new List<NameRecord>();
    this.m_rawFormats = new FormatsCollection(this.Application, (object) this);
    this.m_arrBound = new List<BoundSheetRecord>();
    this.m_arrReparse = new List<IReparse>();
    this.m_arrExtFormatRecords = new List<ExtendedFormatRecord>();
    this.m_arrXFExtRecords = new List<ExtendedXFRecord>();
    this.m_arrStyleExtRecords = new List<StyleExtRecord>();
    this.m_headerFooterPictures = new WorkbookShapeDataImpl(this.Application, (object) this, new WorkbookImpl.ShapesGetterMethod(this.GetHeaderFooterShapes));
    this.m_sheetGroup = new Syncfusion.XlsIO.Implementation.Collections.Grouping.WorksheetGroup(this.Application, (object) this);
    this.WindowOne.SelectedTab = ushort.MaxValue;
  }

  private void CreateGraphics()
  {
    using (Image image = (Image) new Bitmap(1, 1))
      this.m_graphics = Graphics.FromImage(image);
  }

  internal void InsertDefaultValues()
  {
    this.m_rawFormats.InsertDefaultFormats();
    this.InsertDefaultExtFormats();
    this.InsertDefaultStyles();
  }

  protected void InsertDefaultExtFormats()
  {
    int count = this.m_extFormats.Count;
    ExtendedXFRecord defaultXfExt = this.GetDefaultXFExt();
    if (count <= 0)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(0), defaultXfExt));
    if (count <= 1)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(1), defaultXfExt));
    if (count <= 2)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(2), defaultXfExt));
    if (count <= 3)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(3), defaultXfExt));
    if (count <= 4)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(4), defaultXfExt));
    if (count <= 5)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(5), defaultXfExt));
    if (count <= 6)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(6), defaultXfExt));
    if (count <= 7)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(7), defaultXfExt));
    if (count <= 8)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(8), defaultXfExt));
    if (count <= 9)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(9), defaultXfExt));
    if (count <= 10)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(10), defaultXfExt));
    if (count <= 11)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(11), defaultXfExt));
    if (count <= 12)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(12), defaultXfExt));
    if (count <= 13)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(13), defaultXfExt));
    if (count <= 14)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(14), defaultXfExt));
    if (count <= 15)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(15), defaultXfExt));
    if (count <= 16 /*0x10*/)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(16 /*0x10*/), defaultXfExt));
    if (count <= 17)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(17), defaultXfExt));
    if (count <= 18)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(18), defaultXfExt));
    if (count <= 19)
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(19), defaultXfExt));
    if (count > 20)
      return;
    this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, this.GetDefaultXF(20), defaultXfExt));
  }

  protected void InsertDefaultStyles() => this.InsertDefaultStyles((List<StyleRecord>) null);

  protected void InsertDefaultStyles(List<StyleRecord> arrStyles)
  {
    StyleRecord record1 = (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style);
    record1.ExtendedFormatIndex = (ushort) 0;
    record1.BuildInOrNameLen = (byte) 0;
    StyleImpl style = this.AppImplementation.CreateStyle(this, this.FindStyle(arrStyles, record1));
    if (!this.m_styles.ContainsName(style.Name))
      this.m_styles.Add((IStyle) style, true);
    StyleRecord record2 = (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style);
    record2.ExtendedFormatIndex = (ushort) 16 /*0x10*/;
    record2.BuildInOrNameLen = (byte) 3;
    this.AddDefaultStyle(this.AppImplementation.CreateStyle(this, this.FindStyle(arrStyles, record2)));
    StyleRecord record3 = (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style);
    record3.ExtendedFormatIndex = (ushort) 17;
    record3.BuildInOrNameLen = (byte) 6;
    this.AddDefaultStyle(this.AppImplementation.CreateStyle(this, this.FindStyle(arrStyles, record3)));
    StyleRecord record4 = (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style);
    record4.ExtendedFormatIndex = (ushort) 18;
    record4.BuildInOrNameLen = (byte) 4;
    this.AddDefaultStyle(this.AppImplementation.CreateStyle(this, this.FindStyle(arrStyles, record4)));
    StyleRecord record5 = (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style);
    record5.ExtendedFormatIndex = (ushort) 19;
    record5.BuildInOrNameLen = (byte) 7;
    this.AddDefaultStyle(this.AppImplementation.CreateStyle(this, this.FindStyle(arrStyles, record5)));
    StyleRecord record6 = (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style);
    this.AddDefaultStyle(this.AppImplementation.CreateStyle(this, this.FindStyle(arrStyles, record6)));
    StyleRecord record7 = (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style);
    record7.ExtendedFormatIndex = (ushort) 20;
    record7.BuildInOrNameLen = (byte) 5;
    this.AddDefaultStyle(this.AppImplementation.CreateStyle(this, this.FindStyle(arrStyles, record7)));
    (this.Styles["Normal"].Font as FontWrapper).AfterChangeEvent += new EventHandler(this.WorkbookImpl_AfterChangeEvent);
  }

  private void AddDefaultStyle(StyleImpl stout)
  {
    if (stout == null)
      throw new ArgumentNullException(nameof (stout));
    if (this.InnerExtFormats[stout.XFormatIndex].HasParent)
    {
      ExtendedFormatImpl extendedFormatImpl = this.RegisterExtFormat(this.CreateExtFormatWithoutRegister((IExtendedFormat) this.InnerExtFormats[0]), true);
      stout.SetFormatIndex(extendedFormatImpl.Index);
    }
    if (this.m_styles.ContainsName(stout.Name))
      return;
    this.m_styles.Add((IStyle) stout);
  }

  [CLSCompliant(false)]
  protected ExtendedFormatRecord GetDefaultXF(int index)
  {
    ExtendedFormatRecord defaultXf = (ExtendedFormatRecord) null;
    switch (index)
    {
      case 0:
        defaultXf = (ExtendedFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExtendedFormat);
        defaultXf.IsLocked = true;
        defaultXf.ParentIndex = (ushort) this.MaxXFCount;
        defaultXf.XFType = ExtendedFormatRecord.TXFType.XF_CELL;
        defaultXf.FillBackground = (ushort) 65;
        defaultXf.FillForeground = (ushort) 64 /*0x40*/;
        break;
      case 1:
      case 2:
        defaultXf = (ExtendedFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExtendedFormat);
        defaultXf.FontIndex = (ushort) 1;
        defaultXf.IsLocked = true;
        defaultXf.XFType = ExtendedFormatRecord.TXFType.XF_CELL;
        defaultXf.ParentIndex = (ushort) this.MaxXFCount;
        defaultXf.VAlignmentType = ExcelVAlign.VAlignBottom;
        defaultXf.IsNotParentFormat = true;
        defaultXf.IsNotParentAlignment = true;
        defaultXf.IsNotParentBorder = true;
        defaultXf.IsNotParentPattern = true;
        defaultXf.IsNotParentCellOptions = true;
        break;
      case 3:
      case 4:
        defaultXf = (ExtendedFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExtendedFormat);
        if (this.InnerFonts.Count > 2)
          defaultXf.FontIndex = (ushort) 2;
        defaultXf.IsLocked = true;
        defaultXf.XFType = ExtendedFormatRecord.TXFType.XF_CELL;
        defaultXf.ParentIndex = (ushort) this.MaxXFCount;
        defaultXf.VAlignmentType = ExcelVAlign.VAlignBottom;
        defaultXf.IsNotParentFormat = true;
        defaultXf.IsNotParentAlignment = true;
        defaultXf.IsNotParentBorder = true;
        defaultXf.IsNotParentPattern = true;
        defaultXf.IsNotParentCellOptions = true;
        break;
      case 5:
      case 6:
      case 7:
      case 8:
      case 9:
      case 10:
      case 11:
      case 12:
      case 13:
      case 14:
        defaultXf = (ExtendedFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExtendedFormat);
        defaultXf.IsLocked = true;
        defaultXf.XFType = ExtendedFormatRecord.TXFType.XF_CELL;
        defaultXf.ParentIndex = (ushort) this.MaxXFCount;
        defaultXf.VAlignmentType = ExcelVAlign.VAlignBottom;
        defaultXf.IsNotParentFormat = true;
        defaultXf.IsNotParentAlignment = true;
        defaultXf.IsNotParentBorder = true;
        defaultXf.IsNotParentPattern = true;
        defaultXf.IsNotParentCellOptions = true;
        break;
      case 15:
        defaultXf = (ExtendedFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExtendedFormat);
        defaultXf.IsLocked = true;
        defaultXf.HAlignmentType = ExcelHAlign.HAlignGeneral;
        defaultXf.VAlignmentType = ExcelVAlign.VAlignBottom;
        defaultXf.FillBackground = (ushort) 65;
        defaultXf.FillForeground = (ushort) 64 /*0x40*/;
        break;
      case 16 /*0x10*/:
        defaultXf = (ExtendedFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExtendedFormat);
        defaultXf.IsNotParentFont = true;
        defaultXf.IsNotParentAlignment = true;
        defaultXf.IsNotParentBorder = true;
        defaultXf.IsNotParentPattern = true;
        defaultXf.IsNotParentCellOptions = true;
        defaultXf.FontIndex = (ushort) 1;
        defaultXf.FormatIndex = (ushort) 43;
        defaultXf.XFType = ExtendedFormatRecord.TXFType.XF_CELL;
        defaultXf.ParentIndex = (ushort) this.MaxXFCount;
        break;
      case 17:
        defaultXf = (ExtendedFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExtendedFormat);
        defaultXf.IsNotParentFont = true;
        defaultXf.IsNotParentAlignment = true;
        defaultXf.IsNotParentBorder = true;
        defaultXf.IsNotParentPattern = true;
        defaultXf.IsNotParentCellOptions = true;
        defaultXf.FontIndex = (ushort) 1;
        defaultXf.FormatIndex = (ushort) 41;
        defaultXf.ParentIndex = (ushort) this.MaxXFCount;
        defaultXf.XFType = ExtendedFormatRecord.TXFType.XF_CELL;
        break;
      case 18:
        defaultXf = (ExtendedFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExtendedFormat);
        defaultXf.IsLocked = true;
        defaultXf.ParentIndex = (ushort) this.MaxXFCount;
        defaultXf.XFType = ExtendedFormatRecord.TXFType.XF_CELL;
        defaultXf.IsNotParentFont = true;
        defaultXf.IsNotParentAlignment = true;
        defaultXf.IsNotParentBorder = true;
        defaultXf.IsNotParentPattern = true;
        defaultXf.IsNotParentCellOptions = true;
        defaultXf.FontIndex = (ushort) 1;
        defaultXf.FormatIndex = (ushort) 44;
        break;
      case 19:
        defaultXf = (ExtendedFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExtendedFormat);
        defaultXf.IsLocked = true;
        defaultXf.ParentIndex = (ushort) this.MaxXFCount;
        defaultXf.XFType = ExtendedFormatRecord.TXFType.XF_CELL;
        defaultXf.IsNotParentFont = true;
        defaultXf.IsNotParentAlignment = true;
        defaultXf.IsNotParentBorder = true;
        defaultXf.IsNotParentPattern = true;
        defaultXf.IsNotParentCellOptions = true;
        defaultXf.FontIndex = (ushort) 1;
        defaultXf.FormatIndex = (ushort) 42;
        break;
      case 20:
        defaultXf = (ExtendedFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExtendedFormat);
        defaultXf.IsLocked = true;
        defaultXf.ParentIndex = (ushort) this.MaxXFCount;
        defaultXf.XFType = ExtendedFormatRecord.TXFType.XF_CELL;
        defaultXf.IsNotParentFont = true;
        defaultXf.IsNotParentAlignment = true;
        defaultXf.IsNotParentBorder = true;
        defaultXf.IsNotParentPattern = true;
        defaultXf.IsNotParentCellOptions = true;
        defaultXf.FontIndex = (ushort) 1;
        defaultXf.FormatIndex = (ushort) 9;
        break;
    }
    return defaultXf;
  }

  [CLSCompliant(false)]
  protected ExtendedXFRecord GetDefaultXFExt()
  {
    return (ExtendedXFRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExtendedXFRecord);
  }

  private StyleRecord FindStyle(List<StyleRecord> arrStyles, StyleRecord style)
  {
    if (arrStyles == null)
      return style;
    int index = 0;
    for (int count = arrStyles.Count; index < count; ++index)
    {
      StyleRecord arrStyle = arrStyles[index];
      if (this.CompareDefaultStyleRecords(arrStyle, style))
        return arrStyle;
    }
    return style;
  }

  private bool CompareDefaultStyleRecords(StyleRecord style1, StyleRecord style2)
  {
    return style1.IsBuildInStyle && style2.IsBuildInStyle && (int) style1.BuildInOrNameLen == (int) style2.BuildInOrNameLen;
  }

  internal void InsertDefaultFonts() => this.m_fonts.InsertDefaultFonts();

  internal void DisposeAll()
  {
    if (this.m_IsDisposed)
      return;
    this.m_arrObjects.DisposeInternalData();
    this.m_arrObjects.Clear();
    this.m_arrObjects = (WorkbookObjectsCollection) null;
    this.m_externBooks.Dispose();
    this.ClearAll();
    if (this.m_drawGroup != null)
    {
      this.m_drawGroup.m_data = (byte[]) null;
      this.m_drawGroup.Dispose();
    }
    if (this.m_shapesData != null)
      this.m_shapesData.Dispose();
    if (this.m_SSTDictionary != null)
    {
      this.m_SSTDictionary.Dispose();
      this.m_SSTDictionary = (SSTDictionary) null;
    }
    if (this.m_DateTimePatterns != null)
      this.m_DateTimePatterns = (string[]) null;
    if (this.m_tableStyles != null)
    {
      this.m_tableStyles.Dispose();
      this.m_tableStyles = (Syncfusion.XlsIO.Implementation.TableStyles) null;
    }
    if (this.m_vbaProject != null)
    {
      if (this.VbaModule != null)
        this.VbaModule.CodeNameChanged -= new NameChangedEventHandler(this.CodeNameChanged);
      (this.m_vbaProject as Syncfusion.Office.VbaProject).Dispose();
    }
    if (this.m_macroStorage != null)
    {
      this.m_macroStorage.Dispose();
      this.m_macroStorage = (ICompoundStorage) null;
    }
    if (this.m_ptrHeapHandle != IntPtr.Zero)
    {
      Heap.HeapDestroy(this.m_ptrHeapHandle);
      this.m_ptrHeapHandle = IntPtr.Zero;
    }
    if (this.m_workbookFile != null)
      this.m_workbookFile.Dispose();
    if (this.m_graphics != null)
      this.m_graphics.Dispose();
    if (this.m_xmlMaps != null)
    {
      this.m_xmlMaps.Dispose();
      this.m_xmlMaps = (XmlMapCollection) null;
    }
    this.Dispose();
    this.m_IsDisposed = true;
  }

  protected void ClearAll()
  {
    if (this.m_bIsDisposed)
      return;
    if (this.m_ActiveSheet != null && this.m_ActiveSheet is WorksheetImpl)
    {
      (this.m_ActiveSheet as WorksheetImpl).ClearAllData();
      this.m_ActiveSheet = (WorksheetBaseImpl) null;
    }
    foreach (IWorksheet worksheet in (Syncfusion.XlsIO.Implementation.Collections.CollectionBase<IWorksheet>) this.m_worksheets)
      (worksheet as WorksheetImpl).ClearAllData();
    if (this.m_worksheets != null)
      this.m_worksheets.Clear();
    if (this.m_arrBound != null)
    {
      this.m_arrBound.Clear();
      this.m_arrBound = (List<BoundSheetRecord>) null;
    }
    if (this.m_arrNames != null)
    {
      this.m_arrNames.Clear();
      this.m_arrNames = (List<NameRecord>) null;
    }
    if (this.m_arrReparse != null)
    {
      this.m_arrReparse.Clear();
      this.m_arrReparse = (List<IReparse>) null;
    }
    if (this.m_colors != null)
    {
      this.m_colors.Clear();
      this.m_colors = (List<Color>) null;
    }
    if (this.m_extFormats != null)
    {
      this.m_extFormats.Dispose();
      this.m_extFormats.Clear();
      this.m_extFormats = (ExtendedFormatsCollection) null;
    }
    if (this.m_styles != null)
    {
      this.m_styles.Clear();
      this.m_styles = (StylesCollection) null;
    }
    if (this.m_fonts != null)
    {
      this.m_fonts.Dispose();
      this.m_fonts.Clear();
      this.m_fonts = (FontsCollection) null;
    }
    if (this.m_rawFormats != null)
    {
      this.m_rawFormats.Clear();
      this.m_rawFormats = (FormatsCollection) null;
    }
    if (this.m_shapesData != null)
    {
      this.m_shapesData.Clear();
      this.m_shapesData = (WorkbookShapeDataImpl) null;
    }
    if (this.m_SSTDictionary != null)
    {
      this.m_SSTDictionary.Clear();
      this.m_SSTDictionary.Dispose();
      this.m_SSTDictionary = (SSTDictionary) null;
    }
    if (this.m_sstStream != null)
    {
      this.m_sstStream.Dispose();
      this.m_sstStream = (Stream) null;
    }
    if (this.m_arrExtFormatRecords != null)
    {
      this.m_arrExtFormatRecords.Clear();
      this.m_arrExtFormatRecords = (List<ExtendedFormatRecord>) null;
    }
    if (this.m_arrXFExtRecords != null)
    {
      this.m_arrXFExtRecords.Clear();
      this.m_arrXFExtRecords = (List<ExtendedXFRecord>) null;
    }
    if (this.m_styles != null)
    {
      this.m_styles.Clear();
      this.m_styles = (StylesCollection) null;
    }
    if (this.m_fonts != null)
    {
      this.m_fonts.Clear();
      this.m_fonts = (FontsCollection) null;
    }
    if (this.m_externBooks != null)
    {
      this.m_externBooks.Clear();
      this.m_externBooks = (ExternBookCollection) null;
    }
    if (this.m_addinFunctions != null)
    {
      this.m_addinFunctions.Clear();
      this.m_addinFunctions = (AddInFunctionsCollection) null;
    }
    if (this.m_builtInDocumentProperties != null)
    {
      this.m_builtInDocumentProperties.Clear();
      this.m_builtInDocumentProperties = (Syncfusion.XlsIO.Implementation.Collections.BuiltInDocumentProperties) null;
    }
    if (this.m_customDocumentProperties != null)
    {
      this.m_customDocumentProperties.Clear();
      this.m_customDocumentProperties = (Syncfusion.XlsIO.Implementation.Collections.CustomDocumentProperties) null;
    }
    if (this.m_majorFonts != null)
    {
      this.m_majorFonts.Clear();
      this.m_majorFonts = (Dictionary<string, FontImpl>) null;
    }
    if (this.m_minorFonts != null)
    {
      this.m_minorFonts.Clear();
      this.m_minorFonts = (Dictionary<string, FontImpl>) null;
    }
    if (this.m_fileDataHolder != null)
    {
      this.m_fileDataHolder.Dispose();
      this.m_fileDataHolder = (FileDataHolder) null;
    }
    if (this.m_arrObjects != null)
    {
      this.m_arrObjects.Clear();
      this.m_arrObjects = (WorkbookObjectsCollection) null;
    }
    this.m_controlsStream = (Stream) null;
    this.m_sstStream = (Stream) null;
    if (this.m_names != null)
    {
      foreach (NameImpl name in (Syncfusion.XlsIO.Implementation.Collections.CollectionBase<IName>) this.m_names)
        name.ClearAll();
      this.m_names.Clear();
      this.m_names = (WorkbookNamesCollection) null;
    }
    if (this.m_connections != null)
    {
      this.m_connections.Dispose();
      this.m_connections.Clear();
      this.m_connections = (ExternalConnectionCollection) null;
    }
    if (this.m_deletedConnections != null)
    {
      this.m_deletedConnections.Clear();
      this.m_deletedConnections.Dispose();
      this.m_deletedConnections = (ExternalConnectionCollection) null;
    }
    if (this.m_graphics != null)
    {
      this.m_graphics.Dispose();
      this.m_graphics = (Graphics) null;
    }
    if (this.m_calcution != null)
    {
      this.m_calcution.Dispose();
      this.m_calcution = (CalculationOptionsImpl) null;
    }
    if (this.m_bookExt != null)
    {
      this.m_bookExt.ClearData();
      this.m_bookExt = (BiffRecordRaw) null;
    }
    if (this.m_charts != null)
    {
      this.m_charts.Clear();
      this.m_charts = (ChartsCollection) null;
    }
    if (this.m_childElements != null)
    {
      this.m_childElements.Clear();
      this.m_childElements = (Dictionary<string, List<Stream>>) null;
    }
    if (this.m_childElementValues != null)
    {
      this.m_childElementValues.Clear();
      this.m_childElementValues = (Dictionary<string, List<Stream>>) null;
    }
    if (this.m_compatibility != null)
    {
      this.m_compatibility.ClearData();
      this.m_compatibility = (CompatibilityRecord) null;
    }
    if (this.m_contentTypeProperties != null)
    {
      this.m_contentTypeProperties.Clear();
      this.m_contentTypeProperties = (MetaPropertiesImpl) null;
    }
    if (this.m_customXmlPartCollection != null)
    {
      foreach (CommonObject customXmlPart in (Syncfusion.XlsIO.Implementation.Collections.CollectionBase<ICustomXmlPart>) this.m_customXmlPartCollection)
        customXmlPart.Dispose();
      this.m_customXmlPartCollection.Clear();
      this.m_CustomTableStylesStream = (Stream) null;
      this.m_customXmlPartCollection = (CustomXmlPartCollection) null;
    }
    if (this.m_drawGroup != null)
    {
      this.m_drawGroup.Dispose();
      this.m_drawGroup = (MSODrawingGroupRecord) null;
    }
    if (this.m_externSheet != null)
    {
      this.m_externSheet.Dispose();
      this.m_externSheet = (ExternSheetRecord) null;
    }
    if (this.m_formulaUtil != null)
    {
      this.m_formulaUtil.Dispose();
      this.m_formulaUtil = (FormulaUtil) null;
    }
    if (this.m_headerFooterPictures != null)
    {
      this.m_headerFooterPictures.Clear();
      this.m_headerFooterPictures = (WorkbookShapeDataImpl) null;
    }
    if (this.m_modifiedFormatRecord != null)
    {
      this.m_modifiedFormatRecord.Clear();
      this.m_modifiedFormatRecord = (Dictionary<int, int>) null;
    }
    if (this.m_password != null)
    {
      this.m_password.ClearData();
      this.m_password = (PasswordRecord) null;
    }
    if (this.m_passwordRev4 != null)
    {
      this.m_passwordRev4.ClearData();
      this.m_passwordRev4 = (PasswordRev4Record) null;
    }
    if (this.m_windowOne != null)
      this.m_windowOne = (WindowOneRecord) null;
    if (this.m_themeColors != null)
    {
      this.m_themeColors.Clear();
      this.m_themeColors = (List<Color>) null;
    }
    if (this.m_arrStyleExtRecords != null)
    {
      this.m_arrStyleExtRecords.Clear();
      this.m_arrStyleExtRecords = (List<StyleExtRecord>) null;
    }
    if (this.m_xfCellCount != null)
    {
      this.m_xfCellCount.Clear();
      this.m_xfCellCount = (Dictionary<int, int>) null;
    }
    if (this.m_reCalcId != null)
    {
      this.m_reCalcId.ClearData();
      this.m_reCalcId = (RecalcIdRecord) null;
    }
    if (WorkbookImpl.SheetTypeToName != null)
    {
      WorkbookImpl.SheetTypeToName.Clear();
      WorkbookImpl.SheetTypeToName = (Dictionary<ExcelSheetType, string>) null;
    }
    if (this.m_preservesPivotCache != null)
    {
      this.m_preservesPivotCache.Clear();
      this.m_preservesPivotCache = (List<Stream>) null;
    }
    if (this.m_DateTimePatterns != null)
      this.m_DateTimePatterns = (string[]) null;
    if (this.m_worksheets != null)
    {
      this.m_worksheets.Clear();
      this.m_worksheets = (WorksheetsCollection) null;
    }
    GC.SuppressFinalize((object) this);
    this.m_bIsDisposed = true;
  }

  internal void ClearExtendedFormats()
  {
    if (this.m_extFormats == null)
      return;
    foreach (ExtendedFormatImpl extFormat in (Syncfusion.XlsIO.Implementation.Collections.CollectionBase<ExtendedFormatImpl>) this.m_extFormats)
      extFormat.Clear();
  }

  private void CreatePivotCache(ICompoundStorage storage, IDecryptor decryptor)
  {
    if (storage == null)
      throw new ArgumentNullException(nameof (storage));
    this.m_pivotCaches = new PivotCacheCollection(this.Application, (object) this, storage, decryptor);
  }

  private void ParseStgStream(ICompoundStorage storage, ExcelParseOptions options, string password)
  {
    string streamCaseInsensitive = WorkbookImpl.FindStreamCaseInsensitive(storage, "Workbook");
    if (streamCaseInsensitive == null && WorkbookImpl.FindStreamCaseInsensitive(storage, "Book") == "Book")
      throw new NotImplementedException("Old Binary format file has no Support in XlsIO");
    if (streamCaseInsensitive == null)
      throw new ApplicationException("File does not contain workbook stream");
    this.ReadControlsData(storage);
    this.ReadDocumentProperties(storage);
    bool flag = storage.ContainsStorage("_VBA_PROJECT_CUR");
    if (flag)
    {
      this.MacroStorage = storage.OpenStorage("_VBA_PROJECT_CUR");
      this.HasMacros = true;
    }
    this.m_bHasSummaryInformation = storage.ContainsStream("\u0005SummaryInformation");
    this.m_bHasDocumentSummaryInformation = storage.ContainsStream("\u0005DocumentSummaryInformation");
    IDecryptor decryptor = (IDecryptor) null;
    string[] storages = storage.Storages;
    if (storage.ContainsStorage("_SX_DB_CUR"))
      this.CreatePivotCache(storage, decryptor);
    this.HasOleObjects = this.CreateOleCache(storage);
    using (CompoundStream compoundStream = storage.OpenStream(streamCaseInsensitive))
    {
      using (BiffReader reader = new BiffReader((Stream) compoundStream))
        this.Parse(reader, options, password);
    }
    if (storage.ContainsStorage("MsoDataStore"))
      new MsoDataStore(storage.OpenStorage("MsoDataStore"), this).ParseMsoDataStore();
    this.m_bHasMacros = flag;
  }

  private bool CreateOleCache(ICompoundStorage storage)
  {
    string[] storages = storage.Storages;
    string[] array = new string[3]
    {
      "_VBA_PROJECT_CUR",
      "_SX_DB_CUR",
      "MsoDataStore"
    };
    bool oleCache = false;
    List<string> stringList = new List<string>();
    foreach (string str in storages)
    {
      if (Array.IndexOf<string>(array, str) == -1)
      {
        stringList.Add(str);
        oleCache = true;
      }
    }
    if (oleCache)
    {
      this.m_OleStorageCollection = new OleStorageCollection();
      foreach (string storageName in stringList)
      {
        using (ICompoundStorage CompoundStorage = storage.OpenStorage(storageName))
          this.m_OleStorageCollection.ParseStorage(CompoundStorage);
      }
    }
    return oleCache;
  }

  private void ReadControlsData(ICompoundStorage storage)
  {
    if (!storage.ContainsStream("Ctls"))
      return;
    CompoundStream source = storage.OpenStream("Ctls");
    this.m_controlsStream = (Stream) new MemoryStream((int) source.Length);
    UtilityMethods.CopyStreamTo((Stream) source, this.m_controlsStream);
    source.Position = 0L;
    source.Close();
  }

  private void ExtractControlProperties(ICompoundStorage storage)
  {
  }

  private static string FindStreamCaseInsensitive(ICompoundStorage storage, string streamName)
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

  private void ParseExcel2007Stream(Stream stream, string password, bool parseOnDemand)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.m_rawFormats.InsertDefaultFormats();
    this.m_fileDataHolder = new FileDataHolder(this, stream, password);
    if (this.m_fileDataHolder.Archive["xl/styles.bin"] != null)
    {
      stream.Position = 0L;
      this.m_xlsbDataHolder = new XlsbDataHolder(this, this.m_fileDataHolder.Archive);
      this.m_xlsbDataHolder.ParseDocument(ref this.m_themeColors, stream);
    }
    else
      this.m_fileDataHolder.ParseDocument(ref this.m_themeColors, parseOnDemand);
  }

  private void ParseFile(
    string fileName,
    string password,
    ExcelVersion version,
    ExcelParseOptions options,
    bool isReadOnly)
  {
    if (isReadOnly)
    {
      using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        this.ParseStream((Stream) fileStream, password, version, options);
      this.ReadOnly = true;
    }
    else
    {
      using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        this.ParseStream((Stream) fileStream, password, version, options);
    }
  }

  private void ParseStream(
    Stream stream,
    string password,
    ExcelVersion version,
    ExcelParseOptions options)
  {
    this.m_bIsLoaded = true;
    this.m_options = options;
    if (version == ExcelVersion.Excel97to2003)
    {
      this.m_workbookFile = this.AppImplementation.CreateCompoundFile(stream);
      this.ParseStgStream(this.m_workbookFile.RootStorage, options, password);
    }
    else
    {
      if (version == ExcelVersion.Excel97to2003)
        throw new ArgumentOutOfRangeException(nameof (version));
      this.ParseExcel2007Stream(stream, password, (options & ExcelParseOptions.ParseWorksheetsOnDemand) == ExcelParseOptions.ParseWorksheetsOnDemand);
      this.Activate();
    }
  }

  ~WorkbookImpl()
  {
    if (this.AppImplementation != null)
      (this.AppImplementation.GetParent() as ExcelEngine).ThrowNotSavedOnDestroy = false;
    this.Close();
  }

  private void Parse(BiffReader reader)
  {
    this.Parse(reader, ExcelParseOptions.Default, (string) null);
  }

  private IDecryptor Parse(BiffReader reader, ExcelParseOptions options, string password)
  {
    this.Loading = true;
    bool flag1 = true;
    int m_newValue = 1;
    List<StyleRecord> arrStyles = new List<StyleRecord>();
    IDecryptor decryptor = (IDecryptor) null;
    this.m_records = new List<BiffRecordRaw>(128 /*0x80*/);
    this.m_arrBound.Clear();
    this.m_externBooks.Clear();
    this.m_headerFooterPictures.Clear();
    this.m_shapesData.Clear();
    this.m_fonts.Clear();
    this.m_extFormats.Clear();
    reader.SeekOnBOFRecord();
    bool bIgnoreStyles = false;
    bool flag2 = false;
    Dictionary<int, int> hashNewXFormatIndexes = bIgnoreStyles ? new Dictionary<int, int>() : (Dictionary<int, int>) null;
    List<BiffRecordRaw> arrPivotRecords = new List<BiffRecordRaw>();
    uint[] crcCache = WorkbookImpl.InitCRC();
    uint crcValue = 0;
    uint num = 0;
    while (!reader.IsEOF && flag1)
    {
      long position = reader.BaseStream.Position;
      BiffRecordRaw record = reader.GetRecord(decryptor);
      this.m_records.Add(record);
      if (Array.IndexOf<TBIFFRecord>(WorkbookImpl.DEF_PIVOTRECORDS, record.TypeCode) != -1)
        arrPivotRecords.Add(record);
      switch (record.TypeCode)
      {
        case TBIFFRecord.EOF:
          if (!flag2)
            this.PrepareStyles(bIgnoreStyles, arrStyles, hashNewXFormatIndexes);
          this.ExtractWorksheetsFromStream(reader, options, -1, -1, hashNewXFormatIndexes, decryptor);
          flag1 = false;
          continue;
        case TBIFFRecord.Precision:
          this.PrecisionAsDisplayed = ((PrecisionRecord) record).IsPrecision == (ushort) 0;
          continue;
        case TBIFFRecord.Protect:
          this.m_bCellProtect = ((ProtectRecord) record).IsProtected;
          continue;
        case TBIFFRecord.Password:
          this.m_password = (PasswordRecord) record;
          continue;
        case TBIFFRecord.ExternSheet:
          ExternSheetRecord externSheetRecord = (ExternSheetRecord) record;
          if (this.m_externSheet == null || this.m_externSheet.RefCount == (ushort) 0)
          {
            this.m_externSheet = externSheetRecord;
            continue;
          }
          this.m_externSheet.PrependReferences((IList<ExternSheetRecord.TREF>) externSheetRecord.RefList);
          continue;
        case TBIFFRecord.Name:
          this.m_arrNames.Add((NameRecord) record);
          continue;
        case TBIFFRecord.WindowProtect:
          this.m_bWindowProtect = ((WindowProtectRecord) record).IsProtected;
          continue;
        case TBIFFRecord.DateWindow1904:
          this.m_bDate1904 = ((DateWindow1904Record) record).Is1904Windowing;
          continue;
        case TBIFFRecord.FilePass:
          FilePassRecord filePass = (FilePassRecord) record;
          decryptor = this.CreateDecryptor(password, filePass);
          continue;
        case TBIFFRecord.Font:
          if (!bIgnoreStyles)
          {
            this.m_fonts.ForceAdd(this.AppImplementation.CreateFont((object) this, (FontRecord) record));
            continue;
          }
          continue;
        case TBIFFRecord.Continue:
          ContinueRecord continueRecord = (ContinueRecord) record;
          if (this.m_continue == null)
            this.m_continue = new List<ContinueRecord>();
          this.m_continue.Add(continueRecord);
          continue;
        case TBIFFRecord.WindowOne:
          this.m_windowOne = (WindowOneRecord) record;
          continue;
        case TBIFFRecord.FileSharing:
          this.m_fileSharing = (FileSharingRecord) record;
          continue;
        case TBIFFRecord.BoundSheet:
          this.m_arrBound.Add((BoundSheetRecord) record);
          continue;
        case TBIFFRecord.WriteProtection:
          this.m_bWriteProtection = true;
          continue;
        case TBIFFRecord.Country:
          this.m_iCountry = (int) ((CountryRecord) record).CurrentCountry;
          this.m_rawFormats.AddDefaultFormats(this.m_iCountry);
          continue;
        case TBIFFRecord.Palette:
          PaletteRecord.TColor[] colors = ((PaletteRecord) record).Colors;
          int length = colors != null ? colors.Length : 0;
          if (length > 0)
          {
            int index1 = 8;
            for (int index2 = 0; index2 < length; ++index2)
            {
              PaletteRecord.TColor tcolor = colors[index2];
              this.SetPaletteColor(index1, Color.FromArgb((int) tcolor.A, (int) tcolor.R, (int) tcolor.G, (int) tcolor.B));
              ++index1;
            }
            continue;
          }
          continue;
        case TBIFFRecord.HasBasic:
          this.m_bHasMacros = true;
          continue;
        case TBIFFRecord.ExtendedFormat:
          ExtendedFormatRecord xf = (ExtendedFormatRecord) record;
          this.m_arrExtFormatRecords.Add(this.RecheckExtendedFormatRecord(xf));
          crcValue = this.CalculateCRC(crcValue, xf.Data, crcCache);
          continue;
        case TBIFFRecord.MSODrawingGroup:
          this.m_shapesData.ParseDrawGroup(this.m_drawGroup = (MSODrawingGroupRecord) record);
          continue;
        case TBIFFRecord.SST:
          this.PrepareStyles(bIgnoreStyles, arrStyles, hashNewXFormatIndexes);
          flag2 = true;
          this.ParseSSTRecord((SSTRecord) record, options);
          continue;
        case TBIFFRecord.ExtSST:
          if (!((ExtSSTRecord) record).IsEnd)
            continue;
          goto case TBIFFRecord.EOF;
        case TBIFFRecord.UseSelFS:
          this.m_bSelFSUsed = ((UseSelFSRecord) record).Flags;
          continue;
        case TBIFFRecord.SupBook:
          reader.BaseStream.Position = position;
          this.m_externBooks.Parse(reader, decryptor);
          continue;
        case TBIFFRecord.CodeName:
          this.m_strCodeName = ((CodeNameRecord) record).CodeName;
          continue;
        case TBIFFRecord.UnkMacrosDisable:
          this.m_bMacrosDisable = true;
          continue;
        case TBIFFRecord.RecalcId:
          this.m_reCalcId = (RecalcIdRecord) record;
          continue;
        case TBIFFRecord.Style:
          StyleRecord styleRecord = (StyleRecord) record;
          arrStyles.Add(styleRecord);
          continue;
        case TBIFFRecord.Format:
          FormatRecord format = (FormatRecord) record;
          format.FormatString = this.InnerFormats.GetCustomizedString(format.FormatString);
          this.m_rawFormats.Add(this.RecheckFormatRecord(format, ref m_newValue));
          continue;
        case TBIFFRecord.BOF:
          if (((BOFRecord) record).Type != BOFRecord.TType.TYPE_WORKBOOK)
            throw new WrongBiffStreamPartException();
          continue;
        case TBIFFRecord.BookExt:
          this.m_bookExt = record;
          continue;
        case TBIFFRecord.HeaderFooterImage:
          this.m_headerFooterPictures.ParseDrawGroup((MSODrawingGroupRecord) record);
          continue;
        case TBIFFRecord.DConn:
          this.PreserveExternalConnectionDetails.Add(record);
          continue;
        case TBIFFRecord.ExtendedFormatCRC:
          num = ((ExtendedFormatCRC) record).CRCChecksum;
          continue;
        case TBIFFRecord.ExtendedXFRecord:
          this.m_arrXFExtRecords.Add((ExtendedXFRecord) record);
          continue;
        case TBIFFRecord.Compatibility:
          this.m_compatibility = (CompatibilityRecord) record;
          continue;
        case TBIFFRecord.StyleExt:
          this.m_arrStyleExtRecords.Add((StyleExtRecord) record);
          continue;
        case TBIFFRecord.Theme:
          this.m_theme = record;
          continue;
        default:
          continue;
      }
    }
    if ((int) num != (int) crcValue)
      this.IsCRCSucceed = true;
    this.m_records = (List<BiffRecordRaw>) null;
    this.m_arrBound.Clear();
    ((ApplicationImpl) this.Application).SetActiveWorkbook((IWorkbook) this);
    this.Loading = false;
    this.Reparse();
    this.ParseAutoFilters();
    this.ParsePivotRecords(arrPivotRecords);
    return decryptor;
  }

  private void ParsePivotRecords(List<BiffRecordRaw> arrPivotRecords)
  {
    List<PivotCacheInfo> pivotCacheInfos = this.CreatePivotCacheInfos(arrPivotRecords);
    int index = 0;
    for (int count = pivotCacheInfos.Count; index < count; ++index)
    {
      PivotCacheInfo pivotCacheInfo = pivotCacheInfos[index];
      int streamId = pivotCacheInfo.StreamId;
      this.m_pivotCaches[streamId].Info = pivotCacheInfo;
      this.m_pivotCaches.Order.Add(streamId);
    }
  }

  private ExtendedFormatRecord RecheckExtendedFormatRecord(ExtendedFormatRecord xf)
  {
    if (xf == null)
      throw new ArgumentNullException("ExtendedFormatRecord");
    int num;
    if (this.m_modifiedFormatRecord.TryGetValue((int) xf.FormatIndex, out num))
      xf.FormatIndex = Convert.ToUInt16(num);
    return xf;
  }

  private FormatRecord RecheckFormatRecord(FormatRecord format, ref int m_newValue)
  {
    int key = format != null ? format.Index : throw new ArgumentNullException("FormatRecord");
    switch (key)
    {
      case 50:
      case 51:
      case 52:
        int num = 163 + m_newValue++;
        this.m_modifiedFormatRecord.Add(key, num);
        format.Index = num;
        break;
    }
    return format;
  }

  private List<PivotCacheInfo> CreatePivotCacheInfos(List<BiffRecordRaw> arrPivotRecords)
  {
    int num = 0;
    int count = arrPivotRecords.Count;
    List<PivotCacheInfo> pivotCacheInfos = new List<PivotCacheInfo>();
    while (num < count)
    {
      if (arrPivotRecords[num].TypeCode == TBIFFRecord.StreamId)
      {
        PivotCacheInfo pivotCacheInfo = new PivotCacheInfo();
        num = pivotCacheInfo.Parse((IList<BiffRecordRaw>) arrPivotRecords, num);
        pivotCacheInfos.Add(pivotCacheInfo);
      }
      else
        ++num;
    }
    return pivotCacheInfos;
  }

  private void NormalizeBorders(ExtendedFormatRecord xf)
  {
    if (xf.XFType != ExtendedFormatRecord.TXFType.XF_STYLE || xf.IsNotParentBorder || (int) xf.ParentIndex == this.MaxXFCount)
      return;
    ExtendedFormatRecord arrExtFormatRecord = this.m_arrExtFormatRecords[(int) xf.ParentIndex];
    if (xf.BorderBottom == arrExtFormatRecord.BorderBottom && xf.BorderLeft == arrExtFormatRecord.BorderLeft && xf.BorderRight == arrExtFormatRecord.BorderRight && xf.BorderTop == arrExtFormatRecord.BorderTop)
      return;
    xf.IsNotParentBorder = true;
  }

  private IDecryptor CreateDecryptor(string password, FilePassRecord filePass)
  {
    if (filePass == null)
      throw new ArgumentNullException(nameof (filePass));
    FilePassStandardBlock standardBlock = !filePass.IsWeakEncryption ? filePass.StandardBlock : throw new NotSupportedException("Weak encryption algorithm is not supported.");
    if (standardBlock == null)
      throw new NotSupportedException("Strong encryption algorithms are not supported.");
    IDecryptor decryptor = (IDecryptor) null;
    this.CheckPasswordFirstTime(ref password, ref decryptor, standardBlock);
    if (decryptor == null)
    {
      decryptor = (IDecryptor) new MD5Decryptor();
      byte[] documentId = standardBlock.DocumentID;
      this.m_arrDocId = documentId;
      byte[] encyptedDocumentId = standardBlock.EncyptedDocumentID;
      for (byte[] digest = standardBlock.Digest; !decryptor.SetDecryptionInfo(documentId, encyptedDocumentId, digest, password); decryptor = (IDecryptor) new MD5Decryptor())
      {
        PasswordRequiredEventArgs e = new PasswordRequiredEventArgs();
        if (!this.AppImplementation.RaiseOnWrongPassword((object) this, e) || e.StopParsing)
          throw new ArgumentOutOfRangeException(nameof (password), "Wrong password.");
        password = e.NewPassword;
      }
    }
    this.m_strEncryptionPassword = password;
    this.m_encryptionType = ExcelEncryptionType.Standard;
    return decryptor;
  }

  private void CheckPasswordFirstTime(
    ref string password,
    ref IDecryptor decryptor,
    FilePassStandardBlock standardBlock)
  {
    if (password != null)
      return;
    this.CheckStandardPassword(ref decryptor, standardBlock);
    if (decryptor != null)
      return;
    PasswordRequiredEventArgs e = new PasswordRequiredEventArgs();
    if (this.AppImplementation.RaiseOnPasswordRequired((object) this, e))
      password = e.NewPassword;
    else
      e = (PasswordRequiredEventArgs) null;
    if (e == null || e.StopParsing || password == null)
      throw new ArgumentException("Workbook is protected and password wasn't specified.");
  }

  private bool CheckStandardPassword(ref IDecryptor result, FilePassStandardBlock standardBlock)
  {
    result = (IDecryptor) new MD5Decryptor();
    byte[] documentId = standardBlock.DocumentID;
    this.m_arrDocId = documentId;
    byte[] encyptedDocumentId = standardBlock.EncyptedDocumentID;
    byte[] digest = standardBlock.Digest;
    if (!result.SetDecryptionInfo(documentId, encyptedDocumentId, digest, "VelvetSweatshop"))
      result = (IDecryptor) null;
    return result != null;
  }

  private IEncryptor CreateEncryptor()
  {
    if (this.m_encryptionType != ExcelEncryptionType.Standard)
      throw new NotSupportedException("Not supported encryption type.");
    IEncryptor encryptor = (IEncryptor) new MD5Decryptor();
    if (this.m_arrDocId == null)
      this.m_arrDocId = Guid.NewGuid().ToByteArray();
    encryptor.SetEncryptionInfo(this.m_arrDocId, this.m_strEncryptionPassword);
    return encryptor;
  }

  private void ParseSSTRecord(SSTRecord sst, ExcelParseOptions options)
  {
    this.m_SSTDictionary.OriginalSST = sst;
  }

  internal void PrepareStyles(
    bool bIgnoreStyles,
    List<StyleRecord> arrStyles,
    Dictionary<int, int> hashNewXFormatIndexes)
  {
    this.PrepareExtendedFormats(bIgnoreStyles, arrStyles);
    this.m_rawFormats.InsertDefaultFormats();
    if (!bIgnoreStyles)
    {
      this.CreateAllStyles(arrStyles);
      this.InsertDefaultExtFormats();
      this.InsertDefaultStyles(arrStyles);
    }
    else
    {
      this.InsertDefaultFonts();
      this.InsertDefaultExtFormats();
      this.InsertDefaultStyles();
      this.CreateStyleForEachFormat(hashNewXFormatIndexes);
    }
  }

  private void PrepareExtendedFormats(bool bIgnoreStyle, List<StyleRecord> arrStyles)
  {
    if (bIgnoreStyle)
      return;
    int count = this.m_arrExtFormatRecords.Count;
    for (int index1 = 0; index1 < count; ++index1)
    {
      ExtendedFormatRecord arrExtFormatRecord1 = this.m_arrExtFormatRecords[index1];
      this.NormalizeBorders(arrExtFormatRecord1);
      if (arrExtFormatRecord1.BorderBottom != ExcelLineStyle.None || arrExtFormatRecord1.BorderLeft != ExcelLineStyle.None || arrExtFormatRecord1.BorderRight != ExcelLineStyle.None || arrExtFormatRecord1.BorderTop != ExcelLineStyle.None)
        arrExtFormatRecord1.IsNotParentBorder = true;
      ExtendedXFRecord xfext = (ExtendedXFRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExtendedXFRecord);
      for (int index2 = 0; index2 < this.m_arrXFExtRecords.Count; ++index2)
      {
        if ((int) this.m_arrXFExtRecords[index2].XFIndex == index1)
        {
          xfext = this.m_arrXFExtRecords[index2];
          break;
        }
      }
      if (arrExtFormatRecord1.XFType == ExtendedFormatRecord.TXFType.XF_STYLE && (int) arrExtFormatRecord1.ParentIndex >= this.MaxXFCount)
        arrExtFormatRecord1.ParentIndex = (ushort) 0;
      int parentIndex = (int) arrExtFormatRecord1.ParentIndex;
      if (parentIndex != this.MaxXFCount)
      {
        ExtendedFormatRecord arrExtFormatRecord2 = this.m_arrExtFormatRecords[parentIndex];
        if (!arrExtFormatRecord1.IsNotParentFont && (int) arrExtFormatRecord1.FontIndex != (int) arrExtFormatRecord2.FontIndex)
          arrExtFormatRecord1.IsNotParentFont = true;
        if (!arrExtFormatRecord1.IsNotParentAlignment && arrExtFormatRecord1.AlignmentOptions != arrExtFormatRecord2.AlignmentOptions)
          arrExtFormatRecord1.IsNotParentAlignment = true;
        if (!arrExtFormatRecord1.IsNotParentFormat && (int) arrExtFormatRecord1.FormatIndex != (int) arrExtFormatRecord2.FormatIndex)
          arrExtFormatRecord1.IsNotParentFormat = true;
      }
      this.m_extFormats.ForceAdd(new ExtendedFormatImpl(this.Application, (object) this, arrExtFormatRecord1, xfext, true));
    }
    for (int index = 0; index < count; ++index)
      this.m_extFormats[index].UpdateFromParent();
  }

  private void ParseAutoFilters()
  {
    int Index = 0;
    for (int count = this.m_worksheets.Count; Index < count; ++Index)
      ((WorksheetImpl) this.m_worksheets[Index]).ParseAutoFilters();
  }

  private void ExtractWorksheetsFromStream(
    BiffReader reader,
    ExcelParseOptions options,
    int iFirstSheet,
    int iLastSheet,
    Dictionary<int, int> hashNewXFormatIndexes,
    IDecryptor decryptor)
  {
    this.ReadWorksheetsData(reader, options, iFirstSheet, iLastSheet, hashNewXFormatIndexes, decryptor);
    this.PrepareNames();
    this.ParseNames();
    if (options == ExcelParseOptions.ParseWorksheetsOnDemand)
      return;
    this.ParseWorksheets();
  }

  private void ReadWorksheetsData(
    BiffReader reader,
    ExcelParseOptions options,
    int iFirstSheet,
    int iLastSheet,
    Dictionary<int, int> hashNewXFormatIndexes,
    IDecryptor decryptor)
  {
    long length = reader.BaseStream.Length;
    int index = -1;
    if (iLastSheet == -1)
      iLastSheet = int.MaxValue;
    BOFRecord bofRecord = (BOFRecord) null;
    int count = this.m_arrBound.Count;
    this.m_arrObjects.EnsureCapacity(count);
    this.m_worksheets.EnsureCapacity(count);
    do
    {
      if (!reader.IsEOF)
      {
        BiffRecordRaw biffRecordRaw = reader.PeekRecord();
        bofRecord = biffRecordRaw.TypeCode == TBIFFRecord.BOF ? (BOFRecord) biffRecordRaw : throw new WrongBiffStreamFormatException();
      }
      ++index;
      bool flag = index < iFirstSheet || index > iLastSheet;
      BoundSheetRecord boundSheetRecord = this.m_arrBound[index];
      ITabSheet tabSheet;
      switch (bofRecord.Type)
      {
        case BOFRecord.TType.TYPE_WORKSHEET:
          IWorksheet worksheet1 = this.m_worksheets.Add(reader, options, flag, hashNewXFormatIndexes, decryptor);
          tabSheet = (ITabSheet) worksheet1;
          ((WorksheetImpl) worksheet1).Type = (ExcelSheetType) boundSheetRecord.BoundSheetType;
          break;
        case BOFRecord.TType.TYPE_CHART:
          tabSheet = (ITabSheet) this.m_charts.Add(reader, options, flag, hashNewXFormatIndexes, decryptor);
          break;
        default:
          WorksheetImpl worksheet2 = this.AppImplementation.CreateWorksheet((object) this, reader, options, flag, hashNewXFormatIndexes, decryptor);
          worksheet2.Type = (ExcelSheetType) boundSheetRecord.BoundSheetType;
          tabSheet = (ITabSheet) worksheet2;
          if (worksheet2.Type == ExcelSheetType.Worksheet)
          {
            this.m_worksheets.Add((IWorksheet) worksheet2);
            break;
          }
          this.m_arrObjects.Add((ISerializableNamedObject) worksheet2);
          break;
      }
      tabSheet.Name = boundSheetRecord.SheetName;
      tabSheet.Visibility = boundSheetRecord.Visibility;
      this.AppImplementation.RaiseProgressEvent(reader.BaseStream.Position, length);
    }
    while (!reader.IsEOF && index < this.m_arrBound.Count - 1);
  }

  private void PrepareNames()
  {
    int index = 0;
    for (int count = this.m_arrNames.Count; index < count; ++index)
    {
      NameRecord arrName = this.m_arrNames[index];
      if (arrName.IndexOrGlobal == (ushort) 0)
        this.m_names.Add(arrName);
      else
        ((WorksheetNamesCollection) ((IWorksheet) this.m_arrObjects[(int) arrName.IndexOrGlobal - 1]).Names).Add(arrName);
    }
  }

  private void ParseNames() => this.m_names.ParseNames();

  private void ParseWorksheets()
  {
    int index = 0;
    for (int count = this.m_arrObjects.Count; index < count; ++index)
      ((IParseable) this.m_arrObjects[index]).Parse();
    if (this.m_windowOne.SelectedTab != ushort.MaxValue)
      this.m_ActiveSheet = (WorksheetBaseImpl) this.m_arrObjects[(int) this.m_windowOne.SelectedTab];
    else
      ((WorksheetBaseImpl) this.m_arrObjects[0]).Activate();
  }

  internal void ParseWorksheetsOnDemand()
  {
    if (this.m_windowOne.SelectedTab != ushort.MaxValue)
      this.m_ActiveSheet = (WorksheetBaseImpl) this.m_arrObjects[(int) this.m_windowOne.SelectedTab];
    else
      ((WorksheetBaseImpl) this.m_arrObjects[0]).Activate();
  }

  private void Reparse()
  {
    if (this.Loading)
      return;
    int index = 0;
    for (int count = this.m_arrReparse.Count; index < count; ++index)
      this.m_arrReparse[index].Reparse();
    this.m_arrReparse.Clear();
  }

  private void CreateAllStyles(List<StyleRecord> arrStyles)
  {
    int index = 0;
    for (int count = arrStyles.Count; index < count; ++index)
    {
      StyleRecord arrStyle = arrStyles[index];
      StyleExtRecord styleExtRecord = (StyleExtRecord) null;
      if (this.m_arrStyleExtRecords.Count > 0 && index < this.m_arrStyleExtRecords.Count)
        styleExtRecord = this.m_arrStyleExtRecords[index];
      ExtendedFormatImpl innerExtFormat = this.InnerExtFormats[(int) arrStyle.ExtendedFormatIndex];
      if (innerExtFormat.HasParent)
      {
        ExtendedFormatImpl format = (ExtendedFormatImpl) innerExtFormat.Clone();
        format.ParentIndex = this.MaxXFCount;
        format.Record.XFType = ExtendedFormatRecord.TXFType.XF_CELL;
        ExtendedFormatImpl extendedFormatImpl = this.m_extFormats.ForceAdd(format);
        innerExtFormat.ParentIndex = extendedFormatImpl.Index;
        arrStyle.ExtendedFormatIndex = (ushort) extendedFormatImpl.Index;
      }
      if (arrStyle.IsBuildInStyle)
      {
        StyleImpl style = this.AppImplementation.CreateStyle(this, arrStyle);
        if (styleExtRecord != null)
          style.StyleExt = styleExtRecord;
        if (!this.m_styles.ContainsName(style.Name))
          this.m_styles.Add((IStyle) style, true);
      }
      else
      {
        if (arrStyle.Name == null || arrStyle.Name.Length == 0)
          arrStyle.StyleName = CollectionBaseEx<WorksheetImpl>.GenerateDefaultName((ICollection) arrStyles, "Style");
        StyleImpl styleImpl = this.m_styles.Add(arrStyle);
        if (styleExtRecord != null)
          styleImpl.StyleExt = styleExtRecord;
      }
    }
  }

  private StyleRecord FindStyleRecord(
    List<StyleRecord> arrStyles,
    int formatIndex,
    out int iStyleIndex)
  {
    iStyleIndex = -1;
    int index1 = 0;
    for (int count = arrStyles.Count; index1 < count; ++index1)
    {
      StyleRecord arrStyle = arrStyles[index1];
      if ((int) arrStyle.ExtendedFormatIndex == formatIndex)
      {
        iStyleIndex = index1;
        return arrStyle;
      }
    }
    StyleRecord[] defaultStyles = this.GetDefaultStyles();
    int index2 = 0;
    for (int length = defaultStyles.Length; index2 < length; ++index2)
    {
      StyleRecord styleRecord = defaultStyles[index2];
      if ((int) styleRecord.ExtendedFormatIndex == formatIndex)
      {
        iStyleIndex = -index2 - 1;
        return styleRecord;
      }
    }
    return (StyleRecord) null;
  }

  private StyleRecord[] GetDefaultStyles()
  {
    List<StyleRecord> styleRecordList = new List<StyleRecord>(7);
    StyleRecord record1 = (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style);
    record1.ExtendedFormatIndex = (ushort) 16 /*0x10*/;
    record1.BuildInOrNameLen = (byte) 3;
    styleRecordList.Add(record1);
    StyleRecord record2 = (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style);
    record2.ExtendedFormatIndex = (ushort) 17;
    record2.BuildInOrNameLen = (byte) 6;
    styleRecordList.Add(record2);
    StyleRecord record3 = (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style);
    record3.ExtendedFormatIndex = (ushort) 18;
    record3.BuildInOrNameLen = (byte) 4;
    styleRecordList.Add(record3);
    StyleRecord record4 = (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style);
    record4.ExtendedFormatIndex = (ushort) 19;
    record4.BuildInOrNameLen = (byte) 7;
    styleRecordList.Add(record4);
    StyleRecord record5 = (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style);
    styleRecordList.Add(record5);
    StyleRecord record6 = (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style);
    record6.ExtendedFormatIndex = (ushort) 20;
    record6.BuildInOrNameLen = (byte) 5;
    styleRecordList.Add(record6);
    return styleRecordList.ToArray();
  }

  private void CreateStyleForEachFormat(Dictionary<int, int> hashNewXFormatIndexes)
  {
    foreach (KeyValuePair<int, FormatImpl> rawFormat in this.m_rawFormats)
    {
      int key = rawFormat.Key;
      string str = "Format_" + (object) key;
      if (!this.m_styles.ContainsName(str))
      {
        StyleImpl styleImpl = (StyleImpl) this.m_styles.Add(str, (object) "Normal");
        styleImpl.NumberFormat = rawFormat.Value.FormatString;
        int index = styleImpl.Wrapped.CreateChildFormat().Index;
        hashNewXFormatIndexes.Add(key, index);
      }
    }
  }

  private void ReadDocumentProperties(ICompoundStorage storage)
  {
    try
    {
      bool flag = false;
      if (storage is Storage storage1)
      {
        this.ReadDocumentPropertiesNative(storage1);
        flag = true;
      }
      if (flag)
        return;
      this.ReadDocumentPropertiesManaged(storage);
    }
    catch (Exception ex)
    {
    }
  }

  private void ReadDocumentPropertiesNative(Storage storage)
  {
    IPropertySetStorage ppPropSetStg;
    if (Syncfusion.CompoundFile.XlsIO.Native.API.StgCreatePropSetStg(storage.COMStorage, 0U, out ppPropSetStg) != 0)
      return;
    this.m_builtInDocumentProperties.Parse(ppPropSetStg);
    this.m_customDocumentProperties.Parse(ppPropSetStg);
    Marshal.FinalReleaseComObject((object) ppPropSetStg);
  }

  private void ReadDocumentPropertiesManaged(ICompoundStorage storage)
  {
    if (storage.ContainsStream("\u0005SummaryInformation"))
    {
      using (Stream stream = (Stream) storage.OpenStream("\u0005SummaryInformation"))
        this.m_builtInDocumentProperties.Parse(new DocumentPropertyCollection(stream));
    }
    if (!storage.ContainsStream("\u0005DocumentSummaryInformation"))
      return;
    using (Stream stream = (Stream) storage.OpenStream("\u0005DocumentSummaryInformation"))
    {
      DocumentPropertyCollection properties = new DocumentPropertyCollection(stream);
      this.m_builtInDocumentProperties.Parse(properties);
      this.m_customDocumentProperties.Parse(properties);
    }
  }

  public IDataSort CreateDataSorter()
  {
    this.m_dataSorter = new DataSorter((object) this);
    return (IDataSort) this.m_dataSorter;
  }

  public void CopyToClipboard() => this.CopyToClipboard((WorksheetImpl) null);

  public void Activate()
  {
    ((ApplicationImpl) this.Application).SetActiveWorkbook((IWorkbook) this);
  }

  public void Close(string Filename)
  {
    this.Close(Filename != null && Filename.Length > 0, Filename);
  }

  public void Close(bool SaveChanges, string Filename)
  {
    if (this.AppImplementation != null && (this.AppImplementation.GetParent() as ExcelEngine).ThrowNotSavedOnDestroy && !this.AppImplementation.IsSaved)
      throw new ExcelWorkbookNotSavedException("Object cannot be disposed. Save workbook or set property ThrowNotSavedOnDestoy to false.");
    if (SaveChanges)
    {
      if (Filename != null)
        this.SaveAs(Filename);
      else if (this.m_strFullName != null)
        this.Save();
    }
    if (this.Parent is IList)
    {
      IList parent = (IList) this.Parent;
      int index = parent.IndexOf((object) this);
      if (index >= 0)
        parent.RemoveAt(index);
    }
    this.DisposeAll();
    if (this.AppImplementation != null && this.AppImplementation.Workbooks != null)
      (this.AppImplementation.Workbooks as WorkbooksCollection).Remove((IWorkbook) this);
    GC.SuppressFinalize((object) this);
  }

  public void Close(bool saveChanges) => this.Close(saveChanges, (string) null);

  public void Close() => this.Close(false);

  public ITemplateMarkersProcessor CreateTemplateMarkersProcessor()
  {
    return (ITemplateMarkersProcessor) this.AppImplementation.CreateTemplateMarkers((object) this);
  }

  public void MarkAsFinal()
  {
    ((DocumentPropertyImpl) this.m_customDocumentProperties.Add("_MarkAsFinal")).Boolean = true;
  }

  public void Save()
  {
    if (this.m_strFullName == null || this.m_strFullName.Length == 0)
      throw new ApplicationException("Workbook was not created from file. That is why woorkbook file not specified. You must use SaveAs method instead.");
    this.SaveAs(this.m_strFullName, this.m_fileDataHolder != null ? this.m_fileDataHolder.GetWorkbookPartType() : ExcelSaveType.SaveAsXLS);
  }

  public void SaveAs(string FileName)
  {
    this.SaveAs(FileName, this.GetExcelSaveType(FileName, ExcelSaveType.SaveAsXLS), this.GetExcelVersion(FileName));
  }

  public void SaveAs(string FileName, ExcelSaveType saveType)
  {
    this.SaveAs(FileName, this.GetExcelSaveType(FileName, saveType), this.GetExcelVersion(FileName));
  }

  public void SaveAsODS(string FileName)
  {
    ExcelToODSConverter excelToOdsConverter = new ExcelToODSConverter(this);
    excelToOdsConverter.ConvertToODF(FileName);
    excelToOdsConverter.Dispose();
  }

  public void SaveAs(string FileName, ExcelSaveType saveType, ExcelVersion version)
  {
    switch (FileName)
    {
      case null:
        throw new ArgumentNullException("Filename");
      case "":
        throw new ArgumentException("FileName cannot be empty.");
      default:
        saveType = this.GetExcelSaveType(FileName, saveType);
        string fullPath = Path.GetFullPath(FileName);
        string directoryName = Path.GetDirectoryName(fullPath);
        this.Saving = true;
        if (File.Exists(fullPath))
        {
          if ((File.GetAttributes(fullPath) & FileAttributes.ReadOnly) != (FileAttributes) 0)
            this.RaiseReadOnlyFileEvent(fullPath);
          if (this.Application.DeleteDestinationFile && fullPath != this.m_strFullName)
            File.Delete(fullPath);
        }
        if (directoryName.Length != 0 && directoryName != null && directoryName.Length > 0 && !System.IO.Directory.Exists(directoryName))
          System.IO.Directory.CreateDirectory(directoryName);
        this.m_fullOutputFileName = fullPath;
        if (Path.GetExtension(FileName).Equals(".ods", StringComparison.OrdinalIgnoreCase))
          this.SaveAsODS(FileName);
        else if (saveType == ExcelSaveType.SaveAsXLSB)
        {
          if (this.m_fileDataHolder == null)
            this.m_fileDataHolder = new FileDataHolder(this);
          this.m_xlsbDataHolder = new XlsbDataHolder();
          this.m_xlsbDataHolder.Serialize(fullPath, this);
        }
        else
        {
          if (this.Styles.Count > 0)
          {
            FontImpl wrapped = (this.Styles["Normal"].Font as FontWrapper).Wrapped;
            this.m_iFirstCharSize = (int) Math.Round((double) wrapped.MeasureString('0'.ToString()).Width);
            this.m_iSecondCharSize = (int) Math.Round((double) wrapped.MeasureCharacter('0').Width);
          }
          IdReserver shapeIds = this.PrepareShapes(new WorkbookImpl.ShapesGetterMethod(this.GetWorksheetShapes));
          this.PrepareShapes(new WorkbookImpl.ShapesGetterMethod(this.GetHeaderFooterShapes));
          this.CreateSerializator(version, shapeIds).Serialize(fullPath, this, saveType);
        }
        this.m_strFullName = fullPath;
        this.Saving = false;
        this.m_bSaved = true;
        this.RaiseSavedEvent();
        break;
    }
  }

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
        {
          if ((File.GetAttributes(fullPath) & FileAttributes.ReadOnly) != (FileAttributes) 0)
            this.RaiseReadOnlyFileEvent(fullPath);
          if (this.Application.DeleteDestinationFile && fullPath != this.m_strFullName)
            File.Delete(fullPath);
        }
        string str = $"{Path.Combine(Path.GetDirectoryName(fullPath), Path.GetFileNameWithoutExtension(fullPath))}_files";
        if (saveOption.ImagePath == null)
          saveOption.ImagePath = str;
        if (System.IO.Directory.Exists(str))
        {
          System.IO.Directory.Delete(str, true);
          System.IO.Directory.CreateDirectory(str);
        }
        else
          System.IO.Directory.CreateDirectory(str);
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
    new ExcelToHtmlConverter().ConvertToHtml(stream, this, saveOption);
    stream.Flush();
  }

  public void SaveAsHtml(Stream stream) => this.SaveAsHtml(stream, HtmlSaveOptions.Default);

  private IdReserver PrepareShapes(WorkbookImpl.ShapesGetterMethod shapesGetter)
  {
    bool flag = this.ReIndexShapeCollections(shapesGetter);
    bool bChanged;
    IdReserver shapeIdReserver = this.FillReserverFromShapes(shapesGetter, out bChanged);
    if (flag)
    {
      if (this.m_shapesData != null && bChanged)
        this.m_shapesData.ClearPreservedClusters();
      this.RegisterNewShapes(shapeIdReserver, shapesGetter);
    }
    return shapeIdReserver;
  }

  private void RegisterNewShapes(
    IdReserver shapeIdReserver,
    WorkbookImpl.ShapesGetterMethod shapesGetter)
  {
    if (shapeIdReserver == null)
      throw new ArgumentNullException(nameof (shapeIdReserver));
    this.UpdateAddedShapes(shapeIdReserver, shapesGetter);
    this.RegisterNewShapeCollections(shapeIdReserver, shapesGetter);
  }

  private void UpdateAddedShapes(
    IdReserver shapeIdReserver,
    WorkbookImpl.ShapesGetterMethod shapesGetter)
  {
    foreach (ShapeCollectionBase enumerateShape in this.EnumerateShapes(shapesGetter))
    {
      if (enumerateShape.StartId != 0)
      {
        int shapesWithoutId = this.GetShapesWithoutId(enumerateShape);
        if (shapesWithoutId > 0)
        {
          int shapesFreeIndexes = this.GetShapesFreeIndexes(shapeIdReserver, enumerateShape);
          if (shapesWithoutId > shapesFreeIndexes)
          {
            shapeIdReserver.FreeSequence(enumerateShape.CollectionIndex);
            this.AssignIndexes(shapeIdReserver, enumerateShape);
          }
          else
            this.AssignNewIndexes(shapeIdReserver, enumerateShape);
        }
      }
    }
  }

  private void AssignNewIndexes(IdReserver shapeIdReserver, ShapeCollectionBase shapes)
  {
    int lastId = shapes.LastId;
    int index = 0;
    for (int count = shapes.Count; index < count; ++index)
    {
      ShapeImpl shape = shapes[index] as ShapeImpl;
      if (shape.ShapeId == 0)
        shape.ShapeId = ++lastId;
    }
    shapes.LastId = lastId;
  }

  private int GetShapesFreeIndexes(IdReserver shapeIdReserver, ShapeCollectionBase shapes)
  {
    if (shapeIdReserver == null)
      throw new ArgumentNullException(nameof (shapeIdReserver));
    if (shapes == null)
      throw new ArgumentNullException(nameof (shapes));
    return shapeIdReserver.GetReservedCount(shapes.CollectionIndex) + shapes.StartId - shapes.LastId;
  }

  private int GetShapesWithoutId(ShapeCollectionBase shapes)
  {
    if (shapes == null)
      throw new ArgumentNullException(nameof (shapes));
    int shapesWithoutId = 0;
    int index = 0;
    for (int count = shapes.Count; index < count; ++index)
    {
      if ((shapes[index] as ShapeImpl).ShapeId <= 0)
        ++shapesWithoutId;
    }
    return shapesWithoutId;
  }

  private void RegisterNewShapeCollections(
    IdReserver shapeIdReserver,
    WorkbookImpl.ShapesGetterMethod shapesGetter)
  {
    if (shapeIdReserver == null)
      throw new ArgumentNullException(nameof (shapeIdReserver));
    foreach (ShapeCollectionBase enumerateShape in this.EnumerateShapes(shapesGetter))
    {
      int count = enumerateShape.Count;
      int shapeId = (enumerateShape[0] as ShapeImpl).ShapeId;
      if (shapeId != 0)
      {
        int num = shapeId % 1024 /*0x0400*/;
        int key = shapeId - num;
        if (!shapeIdReserver.m_id.ContainsKey(key))
          shapeIdReserver.m_id.Add(key, enumerateShape.CollectionIndex);
      }
    }
    foreach (ShapeCollectionBase enumerateShape in this.EnumerateShapes(shapesGetter))
    {
      if (enumerateShape.StartId == 0)
      {
        this.AssignIndexes(shapeIdReserver, enumerateShape);
        shapeIdReserver.AddAdditionalShapes(enumerateShape.CollectionIndex, enumerateShape.Count);
      }
    }
  }

  private void AssignIndexes(IdReserver shapeIdReserver, ShapeCollectionBase shapes)
  {
    if (shapeIdReserver == null)
      throw new ArgumentNullException(nameof (shapeIdReserver));
    int num1 = shapes != null ? shapes.Count : throw new ArgumentNullException(nameof (shapes));
    int num2 = shapeIdReserver.Allocate(num1 + 1, shapes.CollectionIndex, shapes);
    int num3 = num2 + shapes.Count;
    shapes.StartId = num2;
    shapes.LastId = num3;
    int num4 = num3 + 1;
    for (int index = 0; index < num1; ++index)
    {
      if ((shapes[index] as ShapeImpl).ShapeId == 0)
        (shapes[index] as ShapeImpl).ShapeId = num4 + index;
    }
  }

  private IdReserver FillReserverFromShapes(
    WorkbookImpl.ShapesGetterMethod shapesGetter,
    out bool bChanged)
  {
    IdReserver idReserver = new IdReserver();
    bChanged = false;
    foreach (ShapeCollectionBase enumerateShape in this.EnumerateShapes(shapesGetter))
    {
      if (enumerateShape != null)
      {
        int startId = enumerateShape.StartId;
        int lastId = enumerateShape.LastId;
        int collectionIndex = enumerateShape.CollectionIndex;
        if (startId > 0)
        {
          int index = 0;
          for (int count = enumerateShape.Count; index < count; ++index)
          {
            int shapeId = (enumerateShape[index] as ShapeImpl).ShapeId;
            if (shapeId > 0 && !idReserver.TryReserve(shapeId, shapeId, collectionIndex))
            {
              enumerateShape.StartId = 0;
              enumerateShape.LastId = 0;
            }
            else if (shapeId <= 0 && enumerateShape[index].ShapeType != ExcelShapeType.Unknown)
              bChanged = true;
          }
        }
        if (enumerateShape != null && enumerateShape.Count > 0)
        {
          WorksheetImpl worksheet = enumerateShape.Worksheet;
          if (worksheet != null && worksheet.InnerDVTable != null)
            idReserver.AddAdditionalShapes(collectionIndex, worksheet.InnerDVTable.ShapesCount + 1);
          idReserver.AddAdditionalShapes(collectionIndex, enumerateShape.Count);
        }
      }
    }
    return idReserver;
  }

  private bool ReIndexShapeCollections(WorkbookImpl.ShapesGetterMethod shapesGetter)
  {
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    int num1 = this.GetMaxCollectionIndex(shapesGetter);
    foreach (ShapeCollectionBase enumerateShape in this.EnumerateShapes(shapesGetter))
    {
      if (enumerateShape != null && enumerateShape.Count > 0)
      {
        int key = enumerateShape.CollectionIndex;
        if (dictionary.ContainsKey(key))
        {
          ShapeCollectionBase shapeCollectionBase = enumerateShape;
          int num2;
          num1 = num2 = num1 + 1;
          key = num2;
          shapeCollectionBase.CollectionIndex = num2;
        }
        dictionary.Add(key, key);
      }
    }
    return num1 >= 0;
  }

  private int GetMaxCollectionIndex(WorkbookImpl.ShapesGetterMethod shapesGetter)
  {
    int maxCollectionIndex = -1;
    foreach (ShapeCollectionBase enumerateShape in this.EnumerateShapes(shapesGetter))
    {
      int num = enumerateShape != null ? enumerateShape.CollectionIndex : -1;
      if (num > maxCollectionIndex)
        maxCollectionIndex = num;
    }
    return maxCollectionIndex;
  }

  internal IEnumerable<ShapeCollectionBase> EnumerateShapes(
    WorkbookImpl.ShapesGetterMethod shapesGetter)
  {
    int i = 0;
    for (int len = this.m_arrObjects.Count; i < len; ++i)
    {
      if (this.m_arrObjects[i] is ITabSheet tabSheet1)
      {
        ShapeCollectionBase shapes = shapesGetter(tabSheet1);
        if (shapes != null && shapes.Count > 0)
          yield return shapes;
        shapes = tabSheet1.Shapes as ShapeCollectionBase;
        int j = 0;
        for (int lenJ = shapes.Count; j < lenJ; ++j)
        {
          if (shapes[j] is ITabSheet tabSheet1)
          {
            ShapeCollectionBase shapesResult = shapesGetter(tabSheet1);
            if (shapesResult != null && shapesResult.Count > 0)
              yield return shapesResult;
          }
        }
      }
    }
  }

  private ShapeCollectionBase GetWorksheetShapes(ITabSheet sheet)
  {
    return sheet.Shapes as ShapeCollectionBase;
  }

  private ShapeCollectionBase GetHeaderFooterShapes(ITabSheet sheet)
  {
    if (sheet is ChartShapeImpl chartShapeImpl)
      sheet = (ITabSheet) (WorksheetBaseImpl) chartShapeImpl;
    return (ShapeCollectionBase) ((WorksheetBaseImpl) sheet).HeaderFooterShapes;
  }

  private void PrepareShapes()
  {
  }

  public void SaveAs(string fileName, HttpResponse response)
  {
    this.SaveAs(fileName, ExcelSaveType.SaveAsXLS, response);
  }

  public void SaveAs(string fileName, string separator)
  {
    this.SaveAs(fileName, separator, Encoding.UTF8);
  }

  public void SaveAs(string fileName, string separator, Encoding encoding)
  {
    switch (fileName)
    {
      case null:
        throw new ArgumentNullException("Filename");
      case "":
        throw new ArgumentException("FileName cannot be empty.");
      default:
        if (separator == null || separator.Length == 0)
          throw new ArgumentNullException(nameof (separator));
        if (this.m_ActiveSheet == null)
          throw new ArgumentNullException("Active worksheet.");
        if (!(this.m_ActiveSheet is WorksheetImpl activeSheet))
          throw new ArgumentNullException("ActiveSheet");
        activeSheet.SaveAs(fileName, separator, encoding);
        break;
    }
  }

  public void SaveAs(Stream stream, string separator)
  {
    this.SaveAs(stream, separator, Encoding.UTF8);
  }

  public void SaveAs(Stream stream, string separator, Encoding encoding)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (separator == null || separator.Length == 0)
      throw new ArgumentNullException(nameof (separator));
    this.SaveAsInternal(stream, separator, encoding);
  }

  private void SaveAsInternal(Stream stream, string separator, Encoding encoding)
  {
    if (!(this.ActiveSheet is WorksheetImpl activeSheet))
      return;
    activeSheet.SaveAs(stream, separator, encoding);
  }

  public void SaveAs(string fileName, HttpResponse response, ExcelHttpContentType contentType)
  {
    this.SaveAs(fileName, ExcelSaveType.SaveAsXLS, response, contentType);
  }

  public void SaveAs(string fileName, ExcelSaveType saveType, HttpResponse response)
  {
    this.SaveAs(fileName, saveType, response, ExcelDownloadType.PromptDialog);
  }

  public void SaveAs(
    string fileName,
    ExcelSaveType saveType,
    HttpResponse response,
    ExcelHttpContentType contentType)
  {
    this.SaveAs(fileName, saveType, response, ExcelDownloadType.PromptDialog, contentType);
  }

  public void SaveAs(string fileName, HttpResponse response, ExcelDownloadType downloadType)
  {
    this.SaveAs(fileName, ExcelSaveType.SaveAsXLS, response, downloadType);
  }

  public void SaveAs(
    string fileName,
    HttpResponse response,
    ExcelDownloadType downloadType,
    ExcelHttpContentType contentType)
  {
    this.SaveAs(fileName, ExcelSaveType.SaveAsXLS, response, downloadType, contentType);
  }

  public void SaveAs(
    string fileName,
    ExcelSaveType saveType,
    HttpResponse response,
    ExcelDownloadType downloadType)
  {
    ExcelHttpContentType contentType = ExcelHttpContentType.Excel2000;
    switch (this.GetExcelVersion(fileName))
    {
      case ExcelVersion.Excel97to2003:
        contentType = ExcelHttpContentType.Excel97;
        break;
      case ExcelVersion.Excel2007:
        contentType = ExcelHttpContentType.Excel2007;
        break;
      case ExcelVersion.Excel2010:
        contentType = ExcelHttpContentType.Excel2010;
        break;
      case ExcelVersion.Excel2013:
        contentType = ExcelHttpContentType.Excel2013;
        break;
      case ExcelVersion.Excel2016:
        contentType = ExcelHttpContentType.Excel2016;
        break;
      case ExcelVersion.Xlsx:
        contentType = ExcelHttpContentType.Xlsx;
        break;
    }
    if (this.IsODS(fileName))
      contentType = ExcelHttpContentType.ODS;
    this.SaveAs(fileName, saveType, response, downloadType, contentType);
  }

  private bool IsODS(string fileName) => Path.GetExtension(fileName).ToLower().Equals(".ods");

  public void SaveAs(
    string fileName,
    string separator,
    HttpResponse response,
    ExcelDownloadType downloadType,
    ExcelHttpContentType contentType)
  {
    this.SaveAs(fileName, separator, response, downloadType, contentType, Encoding.UTF8);
  }

  public void SaveAs(
    string fileName,
    string separator,
    HttpResponse response,
    ExcelDownloadType downloadType,
    ExcelHttpContentType contentType,
    Encoding encoding)
  {
    this.PrepareResponse(fileName, response, downloadType, contentType);
    this.SaveAs(response.OutputStream, separator, encoding);
    response.End();
  }

  public void SaveAs(
    string fileName,
    ExcelSaveType saveType,
    HttpResponse response,
    ExcelDownloadType downloadType,
    ExcelHttpContentType contentType)
  {
    this.PrepareResponse(fileName, response, downloadType, contentType);
    saveType = this.GetExcelSaveType(fileName, saveType);
    if (contentType == ExcelHttpContentType.ODS)
      saveType = ExcelSaveType.SaveAsODS;
    this.SaveAs(response.OutputStream, saveType);
    if (this.Version != ExcelVersion.Excel97to2003)
      response.End();
    else
      response.Flush();
  }

  private void PrepareResponse(
    string fileName,
    HttpResponse response,
    ExcelDownloadType downloadType,
    ExcelHttpContentType contentType)
  {
    switch (fileName)
    {
      case null:
        throw new ArgumentNullException(nameof (fileName));
      case "":
        throw new ArgumentOutOfRangeException(nameof (fileName));
      default:
        if (response == null)
          throw new ArgumentNullException(nameof (response));
        string empty = string.Empty;
        string str;
        switch (downloadType)
        {
          case ExcelDownloadType.Open:
            str = "inline";
            break;
          case ExcelDownloadType.PromptDialog:
            str = "attachment";
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (downloadType));
        }
        string contentTypeString = this.GetContentTypeString(contentType);
        fileName = Path.GetFileName(fileName);
        response.Clear();
        response.ContentType = contentTypeString;
        response.AddHeader("Content-Disposition", $"{str}; filename=\"{fileName}\";");
        break;
    }
  }

  public void SaveAsXmlInternal(XmlWriter writer, ExcelXmlSaveType saveType)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    this.Saving = true;
    IXmlSerializator serializator = XmlSerializatorFactory.GetSerializator(saveType);
    if (this.AppImplementation.EvalExpired)
      this.AddWatermark((IWorkbook) this);
    serializator.Serialize(writer, (IWorkbook) this);
    this.Saving = false;
  }

  public void SaveAsXml(XmlWriter writer, ExcelXmlSaveType saveType)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    this.SaveAsXmlInternal(writer, saveType);
  }

  public void SaveAsXml(string strFileName, ExcelXmlSaveType saveType)
  {
    switch (strFileName)
    {
      case null:
        throw new ArgumentNullException(nameof (strFileName));
      case "":
        throw new ArgumentException("strFileName - string cannot be empty.");
      default:
        this.Saving = true;
        Encoding encoding = (Encoding) new UTF8Encoding(false);
        XmlTextWriter writer = new XmlTextWriter(strFileName, encoding);
        writer.Formatting = Formatting.Indented;
        this.SaveAsXml((XmlWriter) writer, saveType);
        writer.Close();
        this.Saving = false;
        break;
    }
  }

  public void SaveAsXml(Stream stream, ExcelXmlSaveType saveType)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.Saving = true;
    Encoding encoding = (Encoding) new UTF8Encoding(false);
    XmlTextWriter writer = new XmlTextWriter(stream, encoding);
    writer.Formatting = Formatting.Indented;
    this.SaveAsXml((XmlWriter) writer, saveType);
    writer.Flush();
    this.Saving = false;
  }

  private string GetContentTypeString(ExcelHttpContentType contentType)
  {
    switch (contentType)
    {
      case ExcelHttpContentType.Excel97:
        return "Application/x-msexcel";
      case ExcelHttpContentType.Excel2000:
        return "Application/vnd.ms-excel";
      case ExcelHttpContentType.Excel2007:
      case ExcelHttpContentType.Excel2010:
      case ExcelHttpContentType.Excel2013:
      case ExcelHttpContentType.Excel2016:
      case ExcelHttpContentType.Xlsx:
        return "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
      case ExcelHttpContentType.CSV:
        return "text/csv";
      case ExcelHttpContentType.ODS:
        return "application/vnd.oasis.opendocument.spreadsheet";
      default:
        throw new ArgumentOutOfRangeException(nameof (contentType));
    }
  }

  public void SaveAs(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.SaveAs(stream, this.HasMacros ? ExcelSaveType.SaveAsMacro : ExcelSaveType.SaveAsXLS);
  }

  public void SaveAs(Stream stream, ExcelSaveType saveType)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.SaveAsInternal(stream, saveType);
  }

  private void SaveAsInternal(Stream stream, ExcelSaveType saveType)
  {
    this.Saving = true;
    if (stream is FileStream)
      saveType = this.GetExcelSaveType((stream as FileStream).Name, saveType);
    switch (saveType)
    {
      case ExcelSaveType.SaveAsODS:
        this.SaveAsODS(stream);
        break;
      case ExcelSaveType.SaveAsXLSB:
        if (this.m_fileDataHolder == null)
          this.m_fileDataHolder = new FileDataHolder(this);
        this.m_xlsbDataHolder = new XlsbDataHolder();
        this.m_xlsbDataHolder.Serialize(stream, this);
        break;
      default:
        IdReserver shapeIds = this.PrepareShapes(new WorkbookImpl.ShapesGetterMethod(this.GetWorksheetShapes));
        this.CreateSerializator(this.GetExcelVersion(stream), shapeIds).Serialize(stream, this, saveType);
        break;
    }
    stream.Flush();
    this.Saving = false;
    this.m_bSaved = true;
    this.RaiseSavedEvent();
  }

  internal void SaveAsODS(Stream stream) => new ExcelToODSConverter(this).ConvertToODF(stream);

  public void SaveAsJson(string fileName)
  {
    if (fileName == null)
      throw new ArgumentNullException("FileName");
    this.SaveAsJson(fileName, true);
  }

  public void SaveAsJson(string fileName, bool isSchema)
  {
    switch (fileName)
    {
      case null:
        throw new ArgumentNullException("FileName");
      case "":
        throw new ArgumentException("FileName cannot be empty.");
      default:
        using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
        {
          this.SaveAsJson((Stream) fileStream, isSchema);
          break;
        }
    }
  }

  public void SaveAsJson(string fileName, IWorksheet worksheet)
  {
    this.SaveAsJson(fileName, worksheet, true);
  }

  public void SaveAsJson(string fileName, IWorksheet worksheet, bool isSchema)
  {
    switch (fileName)
    {
      case null:
        throw new ArgumentNullException("FileName");
      case "":
        throw new ArgumentException("FileName cannot be empty.");
      default:
        using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
        {
          this.SaveAsJson((Stream) fileStream, worksheet, isSchema);
          break;
        }
    }
  }

  public void SaveAsJson(string fileName, IRange range) => this.SaveAsJson(fileName, range, true);

  public void SaveAsJson(string fileName, IRange range, bool isSchema)
  {
    switch (fileName)
    {
      case null:
        throw new ArgumentNullException("FileName");
      case "":
        throw new ArgumentException("FileName cannot be empty.");
      default:
        using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
        {
          this.SaveAsJson((Stream) fileStream, range, isSchema);
          break;
        }
    }
  }

  public void SaveAsJson(Stream stream) => this.SaveAsJson(stream, true);

  public void SaveAsJson(Stream stream, bool isSchema)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    using (JsonWriter writer = new JsonWriter(stream))
    {
      writer.Formatting = JsonFormatting.Indented;
      writer.IsNonSchema = !isSchema;
      if (isSchema)
      {
        writer.WriteStartObject();
        for (int Index = 0; Index < this.Worksheets.Count; ++Index)
        {
          IWorksheet worksheet = this.Worksheets[Index];
          WorkbookImpl.SerializeJsonWithSchema(writer, worksheet);
        }
        writer.WriteEndObject();
      }
      else
      {
        writer.WriteStartArray();
        for (int Index = 0; Index < this.Worksheets.Count; ++Index)
        {
          writer.WriteStartArray();
          IRange usedRange = this.Worksheets[Index].UsedRange;
          WorkbookImpl.SerializeJsonWithoutSchema(writer, usedRange);
          writer.WriteEndArray();
        }
        writer.WriteEndArray();
      }
    }
  }

  public void SaveAsJson(Stream stream, IWorksheet worksheet)
  {
    this.SaveAsJson(stream, worksheet, true);
  }

  public void SaveAsJson(Stream stream, IWorksheet worksheet, bool isSchema)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    using (JsonWriter writer = new JsonWriter(stream))
    {
      writer.Formatting = JsonFormatting.Indented;
      writer.IsNonSchema = !isSchema;
      if (isSchema)
      {
        writer.WriteStartObject();
        WorkbookImpl.SerializeJsonWithSchema(writer, worksheet);
        writer.WriteEndObject();
      }
      else
      {
        writer.WriteStartArray();
        IRange usedRange = worksheet.UsedRange;
        WorkbookImpl.SerializeJsonWithoutSchema(writer, usedRange);
        writer.WriteEndArray();
      }
    }
  }

  public void SaveAsJson(Stream stream, IRange range) => this.SaveAsJson(stream, range, true);

  public void SaveAsJson(Stream stream, IRange range, bool isSchema)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    using (JsonWriter writer = new JsonWriter(stream))
    {
      writer.Formatting = JsonFormatting.Indented;
      writer.IsNonSchema = !isSchema;
      if (isSchema)
      {
        writer.WriteStartObject();
        writer.WritePropertyName(range.Worksheet.Name);
        writer.WriteStartArray();
        WorkbookImpl.SerializeJsonWithSchema(writer, range);
        writer.WriteEndArray();
        writer.WriteEndObject();
      }
      else
      {
        writer.WriteStartArray();
        WorkbookImpl.SerializeJsonWithoutSchema(writer, range);
        writer.WriteEndArray();
      }
    }
  }

  private static void SerializeJsonWithSchema(JsonWriter writer, IWorksheet worksheet)
  {
    writer.WritePropertyName(worksheet.Name);
    writer.WriteStartArray();
    WorkbookImpl.SerializeJsonWithSchema(writer, worksheet.UsedRange);
    writer.WriteEndArray();
  }

  private static void SerializeJsonWithSchema(JsonWriter writer, IRange range)
  {
    IWorksheet worksheet = range.Worksheet;
    int row1 = 0;
    int num1 = 0;
    for (int row2 = range.Row; row2 <= range.LastRow; ++row2)
    {
      for (int column = range.Column; column <= range.LastColumn; ++column)
      {
        if (row2 != 0 && column != 0 && worksheet[row2, column].Value.ToString() != string.Empty)
        {
          row1 = row2;
          break;
        }
      }
      if (row1 != 0)
        break;
    }
    for (int column = range.Column; column <= range.LastColumn; ++column)
    {
      for (int row3 = range.Row; row3 <= range.LastRow; ++row3)
      {
        if (row3 != 0 && column != 0 && worksheet[row3, column].Value.ToString() != string.Empty)
        {
          num1 = column;
          break;
        }
      }
      if (num1 != 0)
        break;
    }
    if (row1 == 0 || num1 == 0)
      return;
    string[] strArray = new string[range.LastColumn + 1];
    for (int row4 = row1 + 1; row4 <= range.LastRow; ++row4)
    {
      int num2 = num1;
      int row5 = row1 + 1;
      int num3 = 1;
      string str = (string) null;
      writer.WriteStartObject();
      for (int column1 = num1; column1 <= range.LastColumn; ++column1)
      {
        if (worksheet[row1, column1].IsMerged)
        {
          if (worksheet[row1, column1].Value.ToString() != string.Empty)
          {
            writer.WritePropertyName(worksheet[row1, column1].Value.ToString());
            writer.WriteStartArray();
            writer.WriteStartObject();
            for (int column2 = num2; column2 <= range.LastColumn; ++column2)
            {
              if (worksheet[row1, column1].Value.ToString() == worksheet[row1, column2].Value.ToString() || worksheet[row1, column2].Value.ToString() == string.Empty)
              {
                if (worksheet[row5, column2].Value.ToString() != string.Empty)
                {
                  if (worksheet[row4, column2].Value.ToString() != string.Empty)
                  {
                    writer.WritePropertyName(worksheet[row5, column2].Value.ToString());
                    if (worksheet[row4, column2].IsMerged)
                      str = !worksheet[row4, column2].HasFormula ? worksheet[row4, column2].Value.ToString() : worksheet[row4, column2].FormulaNumberValue.ToString();
                    if (worksheet[row4, column2].IsMerged && worksheet[row4 + 1, column2].IsMerged)
                      strArray[column2] = !worksheet[row4, column2].HasFormula ? worksheet[row4, column2].Value.ToString() : worksheet[row4, column2].FormulaNumberValue.ToString();
                    if (worksheet[row4, column2].HasFormula)
                      writer.WriteValue(worksheet[row4, column2].FormulaNumberValue.ToString());
                    else
                      writer.WriteValue(worksheet[row4, column2].Value.ToString());
                  }
                  else if (worksheet[row4, column2].IsMerged && worksheet[row4, column2].Value.ToString() == string.Empty)
                  {
                    if (strArray[column2] != null)
                    {
                      writer.WritePropertyName(worksheet[row5, column2].Value.ToString());
                      writer.WriteValue(strArray[column2]);
                    }
                    if (str != null)
                    {
                      writer.WritePropertyName(worksheet[row5, column2].Value.ToString());
                      writer.WriteValue(str);
                    }
                  }
                }
                else if (row5 <= range.LastRow)
                {
                  ++row5;
                  --column2;
                }
              }
              else
              {
                num2 = column2;
                break;
              }
            }
            writer.WriteEndObject();
            writer.WriteEndArray();
          }
        }
        else if (worksheet[row1, column1].Value.ToString() != string.Empty && worksheet[row4, column1].Value.ToString() != string.Empty)
        {
          writer.WritePropertyName(worksheet[row1, column1].Value.ToString());
          if (worksheet[row4, column1].HasFormula)
            writer.WriteValue(worksheet[row4, column1].FormulaNumberValue.ToString());
          else
            writer.WriteValue(worksheet[row4, column1].Value.ToString());
          ++num2;
        }
        else if (worksheet[row4, column1].Value.ToString() != string.Empty)
        {
          writer.WritePropertyName("undefined" + (object) num3);
          if (worksheet[row4, column1].HasFormula)
            writer.WriteValue(worksheet[row4, column1].FormulaNumberValue.ToString());
          else
            writer.WriteValue(worksheet[row4, column1].Value.ToString());
          ++num2;
          ++num3;
        }
      }
      writer.WriteEndObject();
    }
  }

  private static void SerializeJsonWithoutSchema(JsonWriter writer, IRange range)
  {
    IWorksheet worksheet = range.Worksheet;
    int num1 = 0;
    int num2 = 0;
    for (int row = range.Row; row <= range.LastRow; ++row)
    {
      for (int column = range.Column; column <= range.LastColumn; ++column)
      {
        if (row != 0 && column != 0 && worksheet[row, column].Value.ToString() != string.Empty)
        {
          num1 = row;
          break;
        }
      }
      if (num1 != 0)
        break;
    }
    for (int column = range.Column; column <= range.LastColumn; ++column)
    {
      for (int row = range.Row; row <= range.LastRow; ++row)
      {
        if (row != 0 && column != 0 && worksheet[row, column].Value.ToString() != string.Empty)
        {
          num2 = column;
          break;
        }
      }
      if (num2 != 0)
        break;
    }
    if (num1 == 0 || num2 == 0)
      return;
    for (int row = num1; row <= range.LastRow; ++row)
    {
      writer.WriteStartArray();
      for (int column = num2; column <= range.LastColumn; ++column)
      {
        if (worksheet[row, column].HasFormula)
          writer.WriteValue(worksheet[row, column].FormulaNumberValue.ToString());
        else
          writer.WriteValue(worksheet[row, column].Value.ToString());
      }
      writer.WriteEndArray();
    }
  }

  private string[] GetDocParts()
  {
    string[] docParts = new string[this.Worksheets.Count];
    IWorksheets worksheets = this.Worksheets;
    int Index = 0;
    for (int count = worksheets.Count; Index < count; ++Index)
      docParts[Index] = worksheets[Index].Name;
    return docParts;
  }

  private object[] GetHeadingPairs()
  {
    List<object> objectList = new List<object>();
    foreach (KeyValuePair<ExcelSheetType, int> hashHeadingPair in this.GetHashHeadingPairs())
    {
      ExcelSheetType key = hashHeadingPair.Key;
      int num = hashHeadingPair.Value;
      if (WorkbookImpl.SheetTypeToName.ContainsKey(key))
      {
        objectList.Add((object) WorkbookImpl.SheetTypeToName[key]);
        objectList.Add((object) num);
      }
    }
    return objectList.ToArray();
  }

  private Dictionary<ExcelSheetType, int> GetHashHeadingPairs()
  {
    Dictionary<ExcelSheetType, int> hashHeadingPairs = new Dictionary<ExcelSheetType, int>();
    IWorksheets worksheets = this.Worksheets;
    int Index = 0;
    for (int count = worksheets.Count; Index < count; ++Index)
    {
      ExcelSheetType type = (worksheets[Index] as WorksheetImpl).Type;
      if (hashHeadingPairs.ContainsKey(type))
        ++hashHeadingPairs[type];
      else
        hashHeadingPairs.Add(type, 1);
    }
    return hashHeadingPairs;
  }

  public void SetPaletteColor(int index, Color color)
  {
    if (!this.Loading && index < 8 || index >= this.m_colors.Count)
      throw new ArgumentOutOfRangeException(nameof (index), "Index cannot be less than 0 and larger than Palette colors array size.");
    if (!(this.m_colors[index] != color))
      return;
    this.m_bOwnPalette = true;
    this.m_colors[index] = Color.FromArgb((int) color.A, (int) color.R, (int) color.G, (int) color.B);
  }

  public void CopyPaletteColorTo(WorkbookImpl destinationWorkbook)
  {
    destinationWorkbook.InnerPalette.Clear();
    destinationWorkbook.InnerPalette.AddRange((IEnumerable<Color>) this.InnerPalette);
    destinationWorkbook.m_bOwnPalette = this.m_bOwnPalette;
  }

  public void ResetPalette()
  {
    this.m_bOwnPalette = false;
    this.m_colors = new List<Color>((IEnumerable<Color>) WorkbookImpl.DEF_PALETTE);
  }

  public Color GetPaletteColor(ExcelKnownColors color)
  {
    int num = (int) color;
    return num < 77 || num > 79 ? (num != 80 /*0x50*/ ? (num != (int) short.MaxValue || this.m_colors.Count <= 0 ? this.m_colors[num % this.m_colors.Count] : this.m_colors[0]) : ShapeFillImpl.DEF_COMENT_PARSE_COLOR) : WorkbookImpl.m_chartColors[num - 77];
  }

  public ExcelKnownColors GetNearestColor(Color color) => this.GetNearestColor(color, 0);

  public ExcelKnownColors GetNearestColor(Color color, int iStartIndex)
  {
    if (iStartIndex < 0 || iStartIndex > this.m_colors.Count)
      throw new ArgumentOutOfRangeException(nameof (iStartIndex));
    int nearestColor = iStartIndex;
    double num1 = this.ColorDistance(this.m_colors[iStartIndex], color);
    for (int index = iStartIndex + 1; index < this.m_colors.Count; ++index)
    {
      double num2 = this.ColorDistance(this.m_colors[index], color);
      if (this.IsCreated)
      {
        if (num2 <= num1)
        {
          num1 = num2;
          nearestColor = index;
          if (num2 == 0.0)
            break;
        }
      }
      else if (num2 < num1)
      {
        num1 = num2;
        nearestColor = index;
        if (num2 == 0.0)
          break;
      }
    }
    return (ExcelKnownColors) nearestColor;
  }

  public ExcelKnownColors GetNearestColor(int r, int g, int b)
  {
    Color color2 = Color.FromArgb((int) byte.MaxValue, (int) (byte) r, (int) (byte) g, (int) (byte) b);
    int nearestColor = 0;
    double num1 = this.ColorDistance(this.m_colors[0], color2);
    for (int index = 1; index < this.m_colors.Count; ++index)
    {
      double num2 = this.ColorDistance(this.m_colors[index], color2);
      if (num2 < num1)
      {
        num1 = num2;
        nearestColor = index;
      }
    }
    return (ExcelKnownColors) nearestColor;
  }

  public ExcelKnownColors SetColorOrGetNearest(Color color)
  {
    ExcelKnownColors nearestColor = this.GetNearestColor(color);
    if ((this.isEqualColor = this.CompareColors(this.m_colors[(int) nearestColor], color)) || this.m_iFirstUnusedColor >= this.m_colors.Count)
      return nearestColor;
    this.SetPaletteColor(this.m_iFirstUnusedColor, color);
    ExcelKnownColors firstUnusedColor = (ExcelKnownColors) this.m_iFirstUnusedColor;
    ++this.m_iFirstUnusedColor;
    return firstUnusedColor;
  }

  public ExcelKnownColors SetColorOrGetNearest(int r, int g, int b)
  {
    return this.SetColorOrGetNearest(Color.FromArgb((int) byte.MaxValue, (int) (byte) r, (int) (byte) g, (int) (byte) b));
  }

  public void Replace(string oldValue, string newValue)
  {
    int Index = 0;
    for (int count = this.m_worksheets.Count; Index < count; ++Index)
      this.m_worksheets[Index].Replace(oldValue, newValue);
  }

  public void Replace(string oldValue, string newValue, ExcelFindOptions findOptions)
  {
    int Index = 0;
    for (int count = this.m_worksheets.Count; Index < count; ++Index)
      this.m_worksheets[Index].Replace(oldValue, newValue, findOptions);
  }

  public void Replace(string oldValue, DateTime newValue)
  {
    int Index = 0;
    for (int count = this.m_worksheets.Count; Index < count; ++Index)
      this.m_worksheets[Index].Replace(oldValue, newValue);
  }

  public void Replace(string oldValue, double newValue)
  {
    int Index = 0;
    for (int count = this.m_worksheets.Count; Index < count; ++Index)
      this.m_worksheets[Index].Replace(oldValue, newValue);
  }

  public void Replace(string oldValue, string[] newValues, bool isVertical)
  {
    int Index = 0;
    for (int count = this.m_worksheets.Count; Index < count; ++Index)
      this.m_worksheets[Index].Replace(oldValue, newValues, isVertical);
  }

  public void Replace(string oldValue, int[] newValues, bool isVertical)
  {
    int Index = 0;
    for (int count = this.m_worksheets.Count; Index < count; ++Index)
      this.m_worksheets[Index].Replace(oldValue, newValues, isVertical);
  }

  public void Replace(string oldValue, double[] newValues, bool isVertical)
  {
    int Index = 0;
    for (int count = this.m_worksheets.Count; Index < count; ++Index)
      this.m_worksheets[Index].Replace(oldValue, newValues, isVertical);
  }

  public void Replace(string oldValue, DataTable newValues, bool isFieldNamesShown)
  {
    int Index = 0;
    for (int count = this.m_worksheets.Count; Index < count; ++Index)
      this.m_worksheets[Index].Replace(oldValue, newValues, isFieldNamesShown);
  }

  public void Replace(string oldValue, DataColumn newValues, bool isFieldNamesShown)
  {
    int Index = 0;
    for (int count = this.m_worksheets.Count; Index < count; ++Index)
      this.m_worksheets[Index].Replace(oldValue, newValues, isFieldNamesShown);
  }

  public IFont CreateFont()
  {
    return (IFont) new FontWrapper(this.AppImplementation.CreateFont((object) this), false, false);
  }

  public IFont CreateFont(Font nativeFont)
  {
    return (IFont) new FontWrapper(this.AppImplementation.CreateFont((object) this, nativeFont), false, false);
  }

  public IFont AddFont(IFont fontToAdd)
  {
    bool flag = fontToAdd is FontWrapper;
    fontWrapper = (FontWrapper) null;
    FontImpl fontImpl1;
    if (flag)
      fontImpl1 = fontToAdd is FontWrapper fontWrapper ? fontWrapper.Wrapped : throw new ArgumentNullException(nameof (fontToAdd));
    else
      fontImpl1 = fontToAdd as FontImpl;
    FontImpl fontImpl2 = this.m_fonts.Add((IFont) fontImpl1) as FontImpl;
    if (!flag)
      return (IFont) fontImpl2;
    fontWrapper.Wrapped = fontImpl2;
    fontWrapper.IsReadOnly = true;
    return (IFont) fontWrapper;
  }

  public IFont CreateFont(IFont baseFont) => this.CreateFont(baseFont, true);

  public IFont CreateFont(IFont baseFont, bool bAddToCollection)
  {
    IFont font = baseFont != null ? (IFont) this.AppImplementation.CreateFont(baseFont) : (IFont) this.AppImplementation.CreateFont((object) this);
    if (baseFont != null && baseFont is FontImpl)
    {
      (font as FontImpl).ParaAlign = (baseFont as FontImpl).ParaAlign;
      (font as FontImpl).HasParagrapAlign = (baseFont as FontImpl).HasParagrapAlign;
    }
    if (bAddToCollection)
    {
      ((FontImpl) font).Index = this.m_fonts.Count;
      this.m_fonts.Add(font);
    }
    return font;
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
    this.m_isStartsOrEndsWith = new bool?(true);
    ExcelFindOptions findOptions = ignoreCase ? ExcelFindOptions.None : ExcelFindOptions.MatchCase;
    return this.FindFirst(findValue, flags, findOptions);
  }

  public IRange FindStringEndsWith(string findValue, ExcelFindType flags)
  {
    return this.FindStringEndsWith(findValue, flags, false);
  }

  public IRange FindStringEndsWith(string findValue, ExcelFindType flags, bool ignoreCase)
  {
    this.m_isStartsOrEndsWith = new bool?(false);
    ExcelFindOptions findOptions = ignoreCase ? ExcelFindOptions.None : ExcelFindOptions.MatchCase;
    return this.FindFirst(findValue, flags, findOptions);
  }

  public IRange FindFirst(string findValue, ExcelFindType flags, ExcelFindOptions findOptions)
  {
    if (findValue == null)
      return (IRange) null;
    bool flag1 = (flags & ExcelFindType.Formula) == ExcelFindType.Formula;
    bool flag2 = (flags & ExcelFindType.Text) == ExcelFindType.Text;
    bool flag3 = (flags & ExcelFindType.FormulaStringValue) == ExcelFindType.FormulaStringValue;
    bool flag4 = (flags & ExcelFindType.Error) == ExcelFindType.Error;
    bool flag5 = (flags & ExcelFindType.Comments) == ExcelFindType.Comments;
    if (!flag1 && !flag2 && !flag3 && !flag4 && !flag5)
      throw new ArgumentException("Parameter flags is not valid.", nameof (flags));
    return this.Worksheets.FindFirst(findValue, flags, findOptions);
  }

  public IRange FindFirst(double findValue, ExcelFindType flags)
  {
    bool flag1 = (flags & ExcelFindType.FormulaValue) == ExcelFindType.FormulaValue;
    bool flag2 = (flags & ExcelFindType.Number) == ExcelFindType.Number;
    bool flag3 = (flags & ExcelFindType.Comments) == ExcelFindType.Comments;
    if (!flag1 && !flag2 && !flag3)
      throw new ArgumentException("Parameter flags is not valid.", nameof (flags));
    return this.Worksheets.FindFirst(findValue, flags);
  }

  public IRange FindFirst(bool findValue) => this.Worksheets.FindFirst(findValue);

  public IRange FindFirst(DateTime findValue) => this.Worksheets.FindFirst(findValue);

  public IRange FindFirst(TimeSpan findValue) => this.Worksheets.FindFirst(findValue);

  public IRange[] FindAll(string findValue, ExcelFindType flags)
  {
    return this.FindAll(findValue, flags, ExcelFindOptions.None);
  }

  public IRange[] FindAll(string findValue, ExcelFindType flags, ExcelFindOptions findOptions)
  {
    if (findValue == null)
      return (IRange[]) null;
    bool flag1 = (flags & ExcelFindType.Formula) == ExcelFindType.Formula;
    bool flag2 = (flags & ExcelFindType.Text) == ExcelFindType.Text;
    bool flag3 = (flags & ExcelFindType.FormulaStringValue) == ExcelFindType.FormulaStringValue;
    bool flag4 = (flags & ExcelFindType.Error) == ExcelFindType.Error;
    bool flag5 = (flags & ExcelFindType.Values) == ExcelFindType.Values;
    bool flag6 = (flags & ExcelFindType.Comments) == ExcelFindType.Comments;
    if (!flag1 && !flag2 && !flag3 && !flag4 && !flag5 && !flag6)
      throw new ArgumentException("Parameter flags is not valid.", nameof (flags));
    return this.Worksheets.FindAll(findValue, flags, findOptions);
  }

  public IRange[] FindAll(double findValue, ExcelFindType flags)
  {
    bool flag1 = (flags & ExcelFindType.FormulaValue) == ExcelFindType.FormulaValue;
    bool flag2 = (flags & ExcelFindType.Number) == ExcelFindType.Number;
    bool flag3 = (flags & ExcelFindType.Comments) == ExcelFindType.Comments;
    if (!flag1 && !flag2 && !flag3)
      throw new ArgumentException("Parameter flags is not valid.", nameof (flags));
    return this.Worksheets.FindAll(findValue, flags);
  }

  public IRange[] FindAll(bool findValue) => this.Worksheets.FindAll(findValue);

  public IRange[] FindAll(DateTime findValue) => this.Worksheets.FindAll(findValue);

  public IRange[] FindAll(TimeSpan findValue) => this.Worksheets.FindAll(findValue);

  public void SetSeparators(char argumentsSeparator, char arrayRowsSeparator)
  {
    CalcEngine.ParseArgumentSeparator = argumentsSeparator;
    this.FormulaUtil.SetSeparators(argumentsSeparator, arrayRowsSeparator);
  }

  public IHFEngine CreateHFEngine() => (IHFEngine) new HFEngine(this.Application, (object) this);

  public void Protect(bool bIsProtectWindow, bool bIsProtectContent)
  {
    this.Protect(bIsProtectWindow, bIsProtectContent, (string) null);
  }

  public void Protect(bool bIsProtectWindow, bool bIsProtectContent, string password)
  {
    if (!bIsProtectWindow && !bIsProtectContent)
      throw new ArgumentOutOfRangeException("One of params must be TRUE.");
    this.m_bCellProtect = !this.m_bCellProtect && !this.m_bWindowProtect ? bIsProtectContent : throw new NotSupportedException("Workbook is already protected. Use Unprotect before calling method.");
    this.m_bWindowProtect = bIsProtectWindow;
    this.m_encryptionType = ExcelEncryptionType.Standard;
    if (password == null)
      return;
    this.Password.IsPassword = password.Length > 0 ? WorksheetBaseImpl.GetPasswordHash(password) : (ushort) 0;
  }

  public void Unprotect() => this.Unprotect((string) null);

  public void Unprotect(string password)
  {
    if (this.AlgorithmName != null || this.m_password != null)
    {
      if (password == null)
        throw new ArgumentNullException(nameof (password));
      if (this.AlgorithmName != null)
      {
        if (!SecurityHelper2010.VerifyPassword(password, this.AlgorithmName, this.SaltValue, this.HashValue, this.SpinCount))
          throw new ArgumentException("Wrong password");
        this.m_bCellProtect = false;
        this.m_bWindowProtect = false;
        this.AlgorithmName = (string) null;
        this.HashValue = (byte[]) null;
        this.SaltValue = (byte[]) null;
      }
      else
      {
        if ((int) this.m_password.IsPassword != (int) WorksheetBaseImpl.GetPasswordHash(password) && (this.m_password.IsPassword != (ushort) 0 && this.m_password.IsPassword != (ushort) 1 || !(password == string.Empty)))
          throw new ArgumentException("Wrong password");
        this.m_password = (PasswordRecord) null;
        this.m_bCellProtect = false;
        this.m_bWindowProtect = false;
      }
    }
    else
    {
      if (this.m_password != null)
        throw new ArgumentException("Wrong password");
      this.m_bCellProtect = false;
      this.m_bWindowProtect = false;
    }
  }

  public IWorkbook Clone()
  {
    WorkbookImpl workbookImpl = (WorkbookImpl) this.MemberwiseClone();
    workbookImpl.m_ptrHeapHandle = IntPtr.Zero;
    workbookImpl.m_fonts = this.m_fonts.Clone(workbookImpl);
    workbookImpl.m_themeColors = new List<Color>((IEnumerable<Color>) this.m_themeColors);
    workbookImpl.m_rawFormats = this.m_rawFormats.Clone((object) workbookImpl);
    workbookImpl.m_extFormats = (ExtendedFormatsCollection) this.m_extFormats.Clone((object) workbookImpl);
    workbookImpl.m_styles = (StylesCollection) this.m_styles.Clone((object) workbookImpl);
    workbookImpl.m_colors = this.ClonePalette();
    if (this.m_tableStyles != null)
      workbookImpl.m_tableStyles = this.m_tableStyles.Clone(workbookImpl, this.Application);
    if (this.m_CustomTableStylesStream != null)
    {
      this.m_CustomTableStylesStream.Position = 0L;
      byte[] buffer = new byte[this.m_CustomTableStylesStream.Length];
      this.m_CustomTableStylesStream.Read(buffer, 0, buffer.Length);
      this.m_CustomTableStylesStream.Position = 0L;
      workbookImpl.m_CustomTableStylesStream = (Stream) new MemoryStream(buffer);
    }
    if (this.m_controlsStream != null)
    {
      this.m_controlsStream.Position = 0L;
      byte[] buffer = new byte[this.m_controlsStream.Length];
      this.m_controlsStream.Read(buffer, 0, buffer.Length);
      this.m_controlsStream.Position = 0L;
      workbookImpl.m_controlsStream = (Stream) new MemoryStream(buffer);
    }
    if (this.m_fileDataHolder != null)
      workbookImpl.m_fileDataHolder = this.m_fileDataHolder.Clone(workbookImpl);
    workbookImpl.m_worksheets = new WorksheetsCollection(this.Application, (object) workbookImpl);
    workbookImpl.m_charts = new ChartsCollection(this.Application, (object) workbookImpl);
    workbookImpl.m_addinFunctions = new AddInFunctionsCollection(this.Application, (object) workbookImpl);
    workbookImpl.m_externSheet = (ExternSheetRecord) Syncfusion.XlsIO.Parser.Biff_Records.CloneUtils.CloneCloneable((ICloneable) this.m_externSheet);
    workbookImpl.m_externBooks = (ExternBookCollection) this.m_externBooks.Clone((object) workbookImpl);
    workbookImpl.m_arrObjects = (WorkbookObjectsCollection) this.m_arrObjects.Clone((object) workbookImpl);
    workbookImpl.m_SSTDictionary = (SSTDictionary) this.m_SSTDictionary.Clone(workbookImpl);
    workbookImpl.m_calcution = (CalculationOptionsImpl) this.m_calcution.Clone((object) workbookImpl);
    workbookImpl.m_builtInDocumentProperties = (Syncfusion.XlsIO.Implementation.Collections.BuiltInDocumentProperties) this.m_builtInDocumentProperties.Clone((object) workbookImpl);
    workbookImpl.m_customDocumentProperties = (Syncfusion.XlsIO.Implementation.Collections.CustomDocumentProperties) this.m_customDocumentProperties.Clone((object) workbookImpl);
    workbookImpl.m_names = (WorkbookNamesCollection) this.m_names.Clone((object) workbookImpl);
    workbookImpl.m_shapesData = (WorkbookShapeDataImpl) this.m_shapesData.Clone((object) workbookImpl);
    workbookImpl.m_headerFooterPictures = (WorkbookShapeDataImpl) this.m_headerFooterPictures.Clone((object) workbookImpl);
    workbookImpl.m_pivotCaches = (PivotCacheCollection) Syncfusion.XlsIO.Parser.Biff_Records.CloneUtils.CloneCloneable((ICloneParent) this.m_pivotCaches, (object) workbookImpl);
    workbookImpl.m_sheetGroup = (Syncfusion.XlsIO.Implementation.Collections.Grouping.WorksheetGroup) this.m_sheetGroup.Clone((object) workbookImpl);
    workbookImpl.m_addinFunctions.CopyFrom(this.m_addinFunctions);
    workbookImpl.m_externSheet = (ExternSheetRecord) Syncfusion.XlsIO.Parser.Biff_Records.CloneUtils.CloneCloneable((ICloneable) this.m_externSheet);
    workbookImpl.m_windowOne = (WindowOneRecord) Syncfusion.XlsIO.Parser.Biff_Records.CloneUtils.CloneCloneable((ICloneable) this.m_windowOne);
    workbookImpl.m_password = (PasswordRecord) Syncfusion.XlsIO.Parser.Biff_Records.CloneUtils.CloneCloneable((ICloneable) this.m_password);
    workbookImpl.m_passwordRev4 = (PasswordRev4Record) Syncfusion.XlsIO.Parser.Biff_Records.CloneUtils.CloneCloneable((ICloneable) this.m_passwordRev4);
    workbookImpl.m_protectionRev4 = (ProtectionRev4Record) Syncfusion.XlsIO.Parser.Biff_Records.CloneUtils.CloneCloneable((ICloneable) this.m_protectionRev4);
    workbookImpl.m_fileSharing = (FileSharingRecord) Syncfusion.XlsIO.Parser.Biff_Records.CloneUtils.CloneCloneable((ICloneable) this.m_fileSharing);
    workbookImpl.m_arrNames = Syncfusion.XlsIO.Parser.Biff_Records.CloneUtils.CloneCloneable<NameRecord>(this.m_arrNames);
    workbookImpl.m_arrBound = Syncfusion.XlsIO.Parser.Biff_Records.CloneUtils.CloneCloneable<BoundSheetRecord>(this.m_arrBound);
    workbookImpl.m_arrReparse = new List<IReparse>();
    workbookImpl.m_arrExtFormatRecords = Syncfusion.XlsIO.Parser.Biff_Records.CloneUtils.CloneCloneable<ExtendedFormatRecord>(this.m_arrExtFormatRecords);
    workbookImpl.m_arrXFExtRecords = Syncfusion.XlsIO.Parser.Biff_Records.CloneUtils.CloneCloneable<ExtendedXFRecord>(this.m_arrXFExtRecords);
    workbookImpl.m_ActiveSheet = (WorksheetBaseImpl) null;
    workbookImpl.ActiveSheetIndex = this.ActiveSheetIndex;
    workbookImpl.CreateGraphics();
    workbookImpl.m_formulaUtil = (FormulaUtil) null;
    int Index = 0;
    foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) this.Worksheets)
    {
      if ((worksheet as WorksheetImpl).m_hasSheetCalculation)
      {
        workbookImpl.Worksheets[Index].CalcEngine = (CalcEngine) null;
        workbookImpl.Worksheets[Index].EnableSheetCalculations();
      }
      ++Index;
    }
    if (this.HasMacros && this.VbaProject != null)
      workbookImpl.m_vbaProject = (IVbaProject) (this.m_vbaProject as Syncfusion.Office.VbaProject).Clone((object) workbookImpl);
    return (IWorkbook) workbookImpl;
  }

  internal List<string> GetDrawString(
    string cellText,
    RichTextString RTF,
    out List<IFont> richTextFonts,
    IFont excelFont)
  {
    List<IFont> pdfFonts = new List<IFont>();
    IList<int> values = RTF.TextObject.FormattingRuns.Values;
    int formattingRunsCount = RTF.TextObject.FormattingRunsCount;
    IList<int> keys = RTF.TextObject.FormattingRuns.Keys;
    List<string> drawString = new List<string>();
    string empty = string.Empty;
    int num = 0;
    if (formattingRunsCount == 0)
    {
      drawString.Add(RTF.Text);
      richTextFonts = new List<IFont>();
      richTextFonts.Add(excelFont);
      return drawString;
    }
    if (keys[0] != 0)
    {
      num = 1;
      string rtfText = cellText.Substring(0, keys[0]);
      this.UpdateRTFText(excelFont, pdfFonts, drawString, rtfText);
    }
    for (int index = 0; index < formattingRunsCount - 1; ++index)
    {
      string rtfText = cellText.Substring(keys[index], keys[index + 1] - keys[index]);
      this.UpdateRTFText(this.InnerFonts[values[index]], pdfFonts, drawString, rtfText);
      ++num;
    }
    string rtfText1 = cellText.Substring(keys[formattingRunsCount - 1], cellText.Length - keys[formattingRunsCount - 1]);
    this.UpdateRTFText((IFont) RTF.GetFontByIndex(values[formattingRunsCount - 1]), pdfFonts, drawString, rtfText1);
    richTextFonts = pdfFonts;
    return drawString;
  }

  private void UpdateRTFText(
    IFont excelFont,
    List<IFont> pdfFonts,
    List<string> drawString,
    string rtfText)
  {
    if (rtfText.Contains("\n"))
    {
      char[] charArray = rtfText.ToCharArray();
      string empty1 = string.Empty;
      foreach (char ch in charArray)
      {
        if (ch.ToString().Equals("\n"))
        {
          if (empty1 != string.Empty)
          {
            drawString.Add(empty1);
            pdfFonts.Add(excelFont);
            empty1 = string.Empty;
          }
          drawString.Add(ch.ToString());
          pdfFonts.Add(excelFont);
        }
        else
          empty1 += ch.ToString();
      }
      if (!(empty1 != string.Empty))
        return;
      drawString.Add(empty1);
      pdfFonts.Add(excelFont);
      string empty2 = string.Empty;
    }
    else
    {
      drawString.Add(rtfText);
      pdfFonts.Add(excelFont);
    }
  }

  public void SetWriteProtectionPassword(string password)
  {
    if (password == null || password.Length == 0)
    {
      if (this.m_fileSharing == null)
        return;
      this.m_fileSharing.HashPassword = (ushort) 0;
      this.m_fileSharing.CreatorName = (string) null;
    }
    else
    {
      if (this.m_fileSharing == null)
        this.m_fileSharing = (FileSharingRecord) BiffRecordFactory.GetRecord(TBIFFRecord.FileSharing);
      this.m_fileSharing.HashPassword = WorksheetBaseImpl.GetPasswordHash(password);
      this.m_fileSharing.CreatorName = this.Author;
      this.AdvancedWorkbookProtection(password);
    }
  }

  private void AdvancedWorkbookProtection(string password)
  {
    this.AlgorithmName = "SHA-512";
    this.SaltValue = this.CreateSalt(16 /*0x10*/);
    this.SpinCount = 100000U;
    HashAlgorithm algorithm = SecurityHelper2010.GetAlgorithm(this.AlgorithmName);
    byte[] buffer1 = SecurityHelper.CombineArray(this.SaltValue, Encoding.Unicode.GetBytes(password));
    byte[] hash = algorithm.ComputeHash(buffer1);
    for (uint index = 0; index < this.SpinCount; ++index)
    {
      byte[] bytes = BitConverter.GetBytes(index);
      byte[] buffer2 = SecurityHelper.CombineArray(hash, bytes);
      hash = algorithm.ComputeHash(buffer2);
    }
    this.HashValue = hash;
  }

  protected byte[] CreateSalt(int length)
  {
    byte[] salt = length > 0 ? new byte[length] : throw new ArgumentOutOfRangeException(nameof (length));
    Random random = new Random((int) DateTime.Now.Ticks);
    int maxValue = 256 /*0x0100*/;
    for (int index = 0; index < length; ++index)
      salt[index] = (byte) random.Next(maxValue);
    return salt;
  }

  private List<Color> ClonePalette() => new List<Color>((IEnumerable<Color>) this.m_colors);

  private void SaveInExcel2007(Stream stream, ExcelSaveType saveType)
  {
    if (this.m_fileDataHolder == null)
      this.m_fileDataHolder = new FileDataHolder(this);
    this.m_fileDataHolder.SaveDocument(stream, saveType);
  }

  private IWorkbookSerializator CreateSerializator(ExcelVersion version, IdReserver shapeIds)
  {
    if (this.AppImplementation.EvalExpired)
      this.AddWatermark((IWorkbook) this);
    if (this.m_externSheet.RefCount > (ushort) 1370)
      this.OptimizeReferences();
    switch (version)
    {
      case ExcelVersion.Excel97to2003:
        return (IWorkbookSerializator) new WorkbookImpl.WorkbookExcel97Serializator(shapeIds);
      case ExcelVersion.Excel2007:
      case ExcelVersion.Excel2010:
      case ExcelVersion.Excel2013:
      case ExcelVersion.Excel2016:
      case ExcelVersion.Xlsx:
        if (this.m_fileDataHolder == null)
          this.m_fileDataHolder = new FileDataHolder(this);
        return (IWorkbookSerializator) this.m_fileDataHolder;
      default:
        throw new ArgumentOutOfRangeException(nameof (version));
    }
  }

  private void ChangeStylesTo97()
  {
    if (!this.CheckIfStyleChangeNeeded())
      return;
    this.UpdateStyleIndexes(this.FixStyles97(this.PredefidedStylesPositions()));
  }

  private void UpdateStyleIndexes(int[] styleIndexes)
  {
    if (styleIndexes == null)
      throw new ArgumentNullException(nameof (styleIndexes));
    int index = 0;
    for (int count = this.m_arrObjects.Count; index < count; ++index)
      ((WorksheetBaseImpl) this.m_arrObjects[index]).UpdateStyleIndexes(styleIndexes);
  }

  private List<int> PredefidedStylesPositions()
  {
    List<int> intList = new List<int>();
    int index = 0;
    for (int length = WorkbookImpl.PredefinedStyleOutlines.Length; index < length; ++index)
    {
      string defaultStyleName = this.AppImplementation.DefaultStyleNames[WorkbookImpl.PredefinedStyleOutlines[index]];
      StyleImpl style = this.m_styles.Contains(defaultStyleName) ? (StyleImpl) this.m_styles[defaultStyleName] : (StyleImpl) null;
      int num = style != null ? style.XFormatIndex : -1;
      intList.Add(num);
    }
    return intList;
  }

  private bool CheckIfStyleChangeNeeded()
  {
    bool flag = false;
    if (!flag)
      flag = this.DefaultXFIndex != 15;
    int num = flag ? 1 : 0;
    return flag;
  }

  private int[] FixStyles97(List<int> defaultStyleIndexes)
  {
    if (defaultStyleIndexes == null)
      throw new ArgumentNullException(nameof (defaultStyleIndexes));
    int count1 = this.m_extFormats.Count;
    int[] numArray = new int[count1];
    for (int index = 0; index < count1; ++index)
      numArray[index] = -1;
    List<int> fontIndexes = this.ConvertFonts();
    this.ArrayFontIndex = fontIndexes;
    ExtendedFormatsCollection extFormats = this.m_extFormats;
    this.m_extFormats = new ExtendedFormatsCollection(this.Application, (object) this);
    this.InsertDefaultExtFormats();
    int index1 = 0;
    for (int count2 = defaultStyleIndexes.Count; index1 < count2; ++index1)
    {
      int defaultStyleIndex = defaultStyleIndexes[index1];
      if (defaultStyleIndex >= 0)
      {
        int predefinedXf = WorkbookImpl.PredefinedXFs[index1];
        numArray[defaultStyleIndex] = predefinedXf;
        this.m_extFormats.SetXF(predefinedXf, extFormats[defaultStyleIndex]);
      }
    }
    int index2 = 1;
    for (int count3 = extFormats.Count; index2 < count3; ++index2)
    {
      ExtendedFormatImpl format = extFormats[index2];
      this.ConvertColors(format, fontIndexes);
      format.IndentLevel = Math.Min(format.IndentLevel, this.m_iMaxIndent);
      if (format.FillPattern == ExcelPattern.Gradient)
      {
        format.FillPattern = ExcelPattern.Solid;
        ExcelKnownColors indexed = format.Gradient.BackColorObject.GetIndexed((IWorkbook) this);
        format.ColorObject.SetIndexed(indexed);
      }
      if (!format.HasParent && numArray[index2] < 0)
      {
        ExtendedFormatImpl extendedFormatImpl = this.m_extFormats.ForceAdd(format);
        numArray[index2] = extendedFormatImpl.Index;
      }
    }
    int index3 = 1;
    for (int count4 = extFormats.Count; index3 < count4; ++index3)
    {
      ExtendedFormatImpl format = extFormats[index3];
      if (format.HasParent && format.Index != this.m_iDefaultXFIndex)
      {
        format.ParentIndex = numArray[format.ParentIndex];
        ExtendedFormatImpl extendedFormatImpl = this.m_extFormats.Add(format);
        numArray[index3] = extendedFormatImpl.Index;
      }
    }
    this.m_extFormats.SetXF(15, extFormats[this.DefaultXFIndex]);
    numArray[this.m_iDefaultXFIndex] = 15;
    this.m_iDefaultXFIndex = 15;
    return numArray;
  }

  private void ConvertColors(ExtendedFormatImpl format, List<int> fontIndexes)
  {
    format.ColorObject.ConvertToIndexed((IWorkbook) this);
    format.PatternColorObject.ConvertToIndexed((IWorkbook) this);
    format.TopBorderColor.ConvertToIndexed((IWorkbook) this);
    format.BottomBorderColor.ConvertToIndexed((IWorkbook) this);
    format.LeftBorderColor.ConvertToIndexed((IWorkbook) this);
    format.RightBorderColor.ConvertToIndexed((IWorkbook) this);
    format.DiagonalBorderColor.ConvertToIndexed((IWorkbook) this);
    int fontIndex = format.FontIndex;
    format.FontIndex = fontIndexes[fontIndex];
  }

  private List<int> ConvertFonts()
  {
    int count1 = this.m_fonts.Count;
    FontsCollection fontsCollection = new FontsCollection(this.Application, (object) this);
    List<int> intList = new List<int>(count1);
    for (int index = 0; index < count1; ++index)
    {
      FontImpl font = (FontImpl) this.m_fonts[index];
      font.ColorObject.ConvertToIndexed((IWorkbook) this);
      FontImpl fontImpl = (FontImpl) fontsCollection.Add((IFont) font);
      intList.Add(fontImpl.Index);
    }
    int count2 = fontsCollection.Count;
    if (count2 < 5)
    {
      FontImpl font = (FontImpl) fontsCollection[0];
      for (int index = count2; index <= 5; ++index)
      {
        font = (FontImpl) font.Clone();
        fontsCollection.ForceAdd(font);
      }
    }
    this.m_fonts = fontsCollection;
    return intList;
  }

  private bool IsValidDocument(Stream stream, Encoding encoding, string separator)
  {
    string end = new StreamReader(stream, encoding).ReadToEnd();
    int startIndex = 0;
    int num1 = 0;
    int num2 = 1;
    bool flag = true;
    int length1 = separator.Length;
    double length2 = (double) end.Length;
    while (flag && num2 != 0 && (double) startIndex < length2)
    {
      startIndex = end.IndexOf('"', startIndex) + 1;
      num2 = startIndex;
      ++num1;
      if ((double) (startIndex + length1) <= length2 && end.Substring(startIndex, length1) == separator && num1 % 2 != 0)
        flag = false;
    }
    stream.Position = 0L;
    return flag;
  }

  private ExcelVersion GetExcelVersion(string fileName)
  {
    string lower = Path.GetExtension(fileName).ToLower();
    if ((lower.Equals(".xls") || lower.Equals(".xlt")) && this.Version != ExcelVersion.Excel97to2003)
      return this.Version = ExcelVersion.Excel97to2003;
    return (lower.Equals(".xlsx") || lower.Equals(".xlsm") || lower.Equals(".xltx") || lower.Equals(".xltm") || lower.Equals(".xlsb")) && this.Version == ExcelVersion.Excel97to2003 ? (this.Version = ExcelVersion.Xlsx) : this.Version;
  }

  private ExcelSaveType GetExcelSaveType(string fileName, ExcelSaveType savetype)
  {
    switch (Path.GetExtension(fileName).ToLower())
    {
      case ".xltx":
        savetype = ExcelSaveType.SaveAsTemplate;
        break;
      case ".ods":
        savetype = ExcelSaveType.SaveAsODS;
        break;
      case ".xlsm":
        savetype = ExcelSaveType.SaveAsMacro;
        break;
      case ".xltm":
        savetype = ExcelSaveType.SaveAsMacroTemplate;
        break;
      case ".xlsb":
        savetype = ExcelSaveType.SaveAsXLSB;
        break;
      default:
        savetype = ExcelSaveType.SaveAsXLS;
        break;
    }
    return savetype;
  }

  private ExcelVersion GetExcelVersion(Stream stream)
  {
    return stream is FileStream ? this.GetExcelVersion((stream as FileStream).Name) : this.Version;
  }

  [CLSCompliant(false)]
  protected internal void SerializeForClipboard(OffsetArrayList records, WorksheetImpl sheet)
  {
    this.SerializeForClipboard(records, sheet, (IRange) null);
  }

  internal void SerializeForClipboard(OffsetArrayList records, WorksheetImpl sheet, IRange range)
  {
    new WorkbookImpl.WorkbookExcel97Serializator((IdReserver) null).Serialize(records, ExcelSaveType.SaveAsXLS, (IEncryptor) null, this, sheet, true, range);
  }

  public void SetActiveWorksheet(WorksheetBaseImpl sheet)
  {
    this.m_ActiveSheet = sheet;
    this.m_ActiveTabSheet = (ITabSheet) sheet;
    int realIndex = sheet.RealIndex;
    WindowOneRecord windowOne = this.WindowOne;
    windowOne.SelectedTab = (ushort) realIndex;
    if ((int) windowOne.DisplayedTab <= (int) (ushort) realIndex)
      return;
    windowOne.DisplayedTab = (ushort) realIndex;
  }

  public bool ContainsFont(FontImpl font) => this.m_fonts.Contains(font);

  public void UpdateNamedRangeIndexes(int[] arrNewIndex)
  {
    if (arrNewIndex == null)
      throw new ArgumentNullException(nameof (arrNewIndex));
    int Index = 0;
    for (int count = this.m_worksheets.Count; Index < count; ++Index)
      ((WorksheetImpl) this.m_worksheets[Index]).UpdateNamedRangeIndexes(arrNewIndex);
  }

  public void UpdateNamedRangeIndexes(IDictionary<int, int> dicNewIndex)
  {
    if (dicNewIndex == null)
      throw new ArgumentNullException(nameof (dicNewIndex));
    int Index = 0;
    for (int count = this.m_worksheets.Count; Index < count; ++Index)
      ((WorksheetImpl) this.m_worksheets[Index]).UpdateNamedRangeIndexes(dicNewIndex);
  }

  public void SetChanged() => this.Saved = false;

  public void UpdateStringIndexes(List<int> arrNewIndexes)
  {
    if (arrNewIndexes == null)
      throw new ArgumentNullException(nameof (arrNewIndexes));
    this.m_worksheets.UpdateStringIndexes(arrNewIndexes);
  }

  [CLSCompliant(false)]
  public Dictionary<int, int> CopyExternSheets(
    ExternSheetRecord externSheet,
    Dictionary<int, int> hashSubBooks)
  {
    if (externSheet == null)
      throw new ArgumentNullException(nameof (externSheet));
    if (hashSubBooks == null)
      throw new ArgumentNullException(nameof (hashSubBooks));
    ExternSheetRecord externSheet1 = this.ExternSheet;
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    int count = this.Worksheets.Count;
    int key = 0;
    for (int refCount = (int) externSheet.RefCount; key < refCount; ++key)
    {
      ExternSheetRecord.TREF tref = externSheet.Refs[key];
      int num1 = (int) tref.SupBookIndex;
      if (hashSubBooks.ContainsKey(num1))
        num1 = hashSubBooks[num1];
      int num2 = !this.m_externBooks[num1].IsInternalReference || tref.FirstSheet == (ushort) 65534 ? externSheet1.AddReference(num1, (int) tref.FirstSheet, (int) tref.LastSheet) : externSheet1.AddReference(num1, count, count);
      dictionary.Add(key, num2);
    }
    return dictionary;
  }

  public void ReAddAllStrings()
  {
    int Index = 0;
    for (int count = this.m_worksheets.Count; Index < count; ++Index)
      ((WorksheetImpl) this.m_worksheets[Index]).ReAddAllStrings();
  }

  public void UpdateXFIndexes(int maxCount)
  {
    if (maxCount <= 0)
      throw new ArgumentOutOfRangeException(nameof (maxCount));
    int Index = 0;
    for (int count = this.m_worksheets.Count; Index < count; ++Index)
      ((WorksheetImpl) this.m_worksheets[Index]).UpdateExtendedFormatIndex(maxCount);
  }

  public bool IsFormatted(int xfIndex) => xfIndex != this.DefaultXFIndex;

  public double GetMaxDigitWidth()
  {
    Font nativeFont = this.m_styles["Normal"].Font.GenerateNativeFont();
    return this.HasStandardFont && Array.IndexOf<float>(this.DEF_FONT_WIDTH_SINGLE_INCR, nativeFont.Size) >= 0 ? this.GetMaxDigitWidth(nativeFont) + 1.0 : this.GetMaxDigitWidth(nativeFont);
  }

  public double GetMaxDigitHeight()
  {
    Font nativeFont = this.m_styles["Normal"].Font.GenerateNativeFont();
    return this.HasStandardFont ? (double) this.GetActualValue(nativeFont) : (double) (int) Math.Ceiling((double) this.m_graphics.MeasureString("pP", nativeFont).Height);
  }

  private int GetActualValue(Font font)
  {
    if (Array.IndexOf<float>(this.DEF_FONT_HEIGHT_SINGLE_INCR, font.Size) >= 0)
      return (int) Math.Ceiling((double) this.m_graphics.MeasureString("pP", font).Height) + 1;
    return Array.IndexOf<float>(this.DEF_FONT_HEIGHT_DOUBLE_INCR, font.Size) >= 0 ? (int) Math.Ceiling((double) this.m_graphics.MeasureString("pP", font).Height) + 2 : (int) Math.Ceiling((double) this.m_graphics.MeasureString("pP", font).Height);
  }

  public double GetMaxDigitWidth(Font font)
  {
    return this.EnumerateDigits(font, new WorkbookImpl.DigitSizeCallback(this.GetDigitWidth));
  }

  public double GetMaxDigitHeight(Font font)
  {
    return this.EnumerateChars(font, new WorkbookImpl.DigitSizeCallback(this.GetDigitHeight), new char[2]
    {
      'p',
      'P'
    });
  }

  private void GetDigitWidth(RectangleF rect, ref double maxValue)
  {
    if ((double) rect.Width <= maxValue)
      return;
    maxValue = (double) rect.Width;
  }

  private void GetDigitHeight(RectangleF rect, ref double maxValue)
  {
    if ((double) rect.Height <= maxValue)
      return;
    maxValue = (double) rect.Height;
  }

  private double EnumerateDigits(Font font, WorkbookImpl.DigitSizeCallback digitProcessor)
  {
    return this.EnumerateChars(font, digitProcessor, new char[10]
    {
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9'
    });
  }

  private double EnumerateChars(
    Font font,
    WorkbookImpl.DigitSizeCallback digitProcessor,
    char[] chars)
  {
    StringFormat stringFormat = new StringFormat(StringFormat.GenericTypographic);
    stringFormat.Alignment = StringAlignment.Near;
    stringFormat.SetMeasurableCharacterRanges(new CharacterRange[1]
    {
      new CharacterRange(1, 1)
    });
    RectangleF rectangleF = new RectangleF(0.0f, 0.0f, 1000f, 1000f);
    double currentMax = 0.0;
    int index = 0;
    for (int length = chars.Length; index < length; ++index)
    {
      rectangleF = this.m_graphics.MeasureCharacterRanges("0" + (object) chars[index], font, rectangleF, stringFormat)[0].GetBounds(this.m_graphics);
      digitProcessor(rectangleF, ref currentMax);
    }
    if (!this.HasStandardFont)
      return currentMax;
    return (double) font.Size > 10.0 ? ((double) font.Size % 2.0 != 0.0 ? currentMax - 1.0 : currentMax) : ((double) font.Size >= 10.0 || (double) font.Size > currentMax ? currentMax : currentMax - 2.0);
  }

  public double WidthToFileWidth(double width)
  {
    double maxDigitWidth = this.MaxDigitWidth;
    return width <= 1.0 ? width * (maxDigitWidth + 5.0) / maxDigitWidth * 256.0 / 256.0 : (width * maxDigitWidth + 5.0) / maxDigitWidth * 256.0 / 256.0;
  }

  public double FileWidthToPixels(double fileWidth)
  {
    double maxDigitWidth = this.MaxDigitWidth;
    return MathGeneral.Truncate((256.0 * fileWidth + MathGeneral.Truncate(128.0 / maxDigitWidth)) / 256.0 * maxDigitWidth);
  }

  private static double Truncate(double d) => d <= 0.0 ? -Math.Floor(-d) : Math.Floor(d);

  public double PixelsToWidth(double pixels)
  {
    double maxDigitWidth = this.MaxDigitWidth;
    return pixels <= maxDigitWidth + 5.0 ? pixels / (maxDigitWidth + 5.0) : Math.Truncate((pixels - 5.0) / maxDigitWidth * 100.0 + 0.5) / 100.0;
  }

  internal void RemoveUnusedCaches()
  {
    if (this.m_pivotCaches == null)
      return;
    int[] indexes = this.m_pivotCaches.GetIndexes();
    this.pivotCacheIndexes = indexes;
    foreach (int index in indexes)
      this.RemoveCache(index);
  }

  internal void RemoveCache(int index)
  {
    bool flag = false;
    foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) this.Worksheets)
    {
      IPivotTables pivotTables = worksheet.PivotTables;
      for (int index1 = 0; index1 < pivotTables.Count; ++index1)
      {
        IPivotTable pivotTable = pivotTables[index1];
        if (this.Version != ExcelVersion.Excel97to2003)
        {
          if (pivotTable.CacheIndex == index)
          {
            flag = true;
            break;
          }
        }
        else if (Array.IndexOf<int>(this.pivotCacheIndexes, index) == pivotTable.CacheIndex)
        {
          flag = true;
          break;
        }
      }
      if (flag)
        break;
    }
    if (flag)
      return;
    this.m_pivotCaches.RemoveAt(index);
    this.m_pivotCaches.Order.Remove(index);
  }

  public void DeleteConnection(IConnection Connection) => this.m_connections.Remove(Connection);

  internal void RemoveUnusedXFRecord()
  {
    if (this.m_bisXml && this.Options != ExcelParseOptions.ParseWorksheetsOnDemand && !this.m_bisCopy)
    {
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      bool flag1 = true;
      bool flag2 = false;
      bool flag3 = true;
      int num1 = 0;
      for (int xfstartIndex = this.m_XFstartIndex; xfstartIndex < this.m_extFormats.Count; ++xfstartIndex)
      {
        ExtendedFormatImpl extFormat = this.m_extFormats[xfstartIndex];
        if (extFormat.HasParent)
        {
          if (!flag1 || this.m_bisXFStartIndexFound)
          {
            if (!this.m_bisXFStartIndexFound)
            {
              this.m_XFstartIndex = xfstartIndex;
              this.m_bisXFStartIndexFound = true;
            }
            if (!this.m_usedCellStyleIndex.ContainsKey(extFormat.Index + num1) && extFormat.ParentIndex < 1)
            {
              this.RemoveExtenededFormatIndex(extFormat.Index);
              ++this.m_XFRemovedCount;
              ++num1;
              flag2 = true;
              flag3 = false;
              --xfstartIndex;
            }
            if (flag3)
            {
              int num2;
              this.m_usedCellStyleIndex.TryGetValue(extFormat.Index + num1, out num2);
              dictionary.Add(extFormat.Index, num2);
            }
            flag3 = true;
          }
          flag1 = false;
        }
        else if (this.m_bisXFStartIndexFound)
        {
          if (this.m_usedCellStyleIndex.ContainsKey(extFormat.Index))
          {
            int num3;
            this.m_usedCellStyleIndex.TryGetValue(extFormat.Index + num1, out num3);
            dictionary.Add(extFormat.Index, num3);
          }
          else
            dictionary.Add(extFormat.Index, 1);
        }
      }
      if (flag2)
      {
        this.m_usedCellStyleIndex.Clear();
        foreach (KeyValuePair<int, int> keyValuePair in dictionary)
          this.m_usedCellStyleIndex.Add(keyValuePair.Key, keyValuePair.Value);
      }
    }
    this.m_bisUnusedXFRemoved = true;
  }

  internal void AddUsedStyleIndex(int iStyleIndex)
  {
    int num1 = 1;
    if (this.m_usedCellStyleIndex.ContainsKey(iStyleIndex))
    {
      this.m_usedCellStyleIndex.TryGetValue(iStyleIndex, out num1);
      int num2 = num1 + 1;
      this.m_usedCellStyleIndex[iStyleIndex] = num2;
    }
    else
      this.m_usedCellStyleIndex.Add(iStyleIndex, num1);
  }

  internal void UpdateUsedStyleIndex(int iStyleIndex, int count)
  {
    int num1 = count;
    if (this.m_usedCellStyleIndex.ContainsKey(iStyleIndex))
    {
      this.m_usedCellStyleIndex.TryGetValue(iStyleIndex, out num1);
      int num2 = count + num1;
      this.m_usedCellStyleIndex[iStyleIndex] = num2;
    }
    else
      this.m_usedCellStyleIndex.Add(iStyleIndex, num1);
  }

  public event EventHandler OnFileSaved;

  public event ReadOnlyFileEventHandler OnReadOnlyFile;

  private void WorkbookImpl_AfterChangeEvent(object sender, EventArgs e)
  {
    this.m_iFirstCharSize = -1;
    this.m_iSecondCharSize = -1;
    this.m_dMaxDigitWidth = this.GetMaxDigitWidth();
    this.StandardRowHeightInPixels = (int) this.GetMaxDigitHeight();
  }

  public double StandardRowHeight
  {
    get
    {
      return this.m_worksheets.Count <= 0 ? this.GetMaxDigitHeight() : this.m_worksheets[0].StandardHeight;
    }
    set
    {
      if (value == this.StandardRowHeight)
        return;
      int Index = 0;
      for (int count = this.m_worksheets.Count; Index < count; ++Index)
        this.m_worksheets[Index].StandardHeight = value;
    }
  }

  public int StandardRowHeightInPixels
  {
    get => (int) ApplicationImpl.ConvertToPixels(this.StandardRowHeight, MeasureUnits.Point);
    set
    {
      this.StandardRowHeight = ApplicationImpl.ConvertFromPixel((double) value, MeasureUnits.Point);
    }
  }

  internal void CalcEngineMemberValuesOnSheet(bool isClear)
  {
    if (isClear)
    {
      if (this.m_calcEnginePreviousValues == null)
        return;
      for (int Index = 0; Index < this.m_worksheets.Count; ++Index)
      {
        CalcEngine calcEngine = this.m_worksheets[Index].CalcEngine;
        if (calcEngine != null)
        {
          calcEngine.UseFormulaValues = ((int) this.m_calcEnginePreviousValues[Index] & 1) == 1;
          calcEngine.AllowShortCircuitIFs = ((int) this.m_calcEnginePreviousValues[Index] & 2) == 2;
        }
      }
      this.m_calcEnginePreviousValues = (byte[]) null;
    }
    else
    {
      this.m_calcEnginePreviousValues = new byte[this.m_worksheets.Count];
      for (int Index = 0; Index < this.m_worksheets.Count; ++Index)
      {
        CalcEngine calcEngine = this.m_worksheets[Index].CalcEngine;
        if (calcEngine != null)
        {
          if (calcEngine.UseFormulaValues)
            this.m_calcEnginePreviousValues[Index] |= (byte) 1;
          if (calcEngine.AllowShortCircuitIFs)
            this.m_calcEnginePreviousValues[Index] |= (byte) 2;
          calcEngine.UseFormulaValues = true;
          calcEngine.AllowShortCircuitIFs = true;
        }
      }
    }
  }

  internal void ExtractControlProperties()
  {
    throw new Exception("The method or operation is not implemented.");
  }

  internal uint CalculateCRC(uint crcValue, byte[] arrData, uint[] crcCache)
  {
    foreach (byte num in arrData)
    {
      uint index = crcValue >> 24 ^ (uint) num;
      crcValue <<= 8;
      crcValue ^= crcCache[(IntPtr) index];
    }
    return crcValue;
  }

  internal static uint[] InitCRC()
  {
    uint[] numArray = new uint[256 /*0x0100*/];
    uint num1 = 2147483648 /*0x80000000*/;
    for (uint index1 = 0; index1 <= (uint) byte.MaxValue; ++index1)
    {
      uint num2 = index1 << 24;
      for (int index2 = 0; index2 <= 7; ++index2)
      {
        if (((int) num2 & (int) num1) == (int) num1)
          num2 = num2 << 1 ^ 175U;
        else
          num2 <<= 1;
      }
      uint num3 = num2 & (uint) ushort.MaxValue;
      numArray[(IntPtr) index1] = num3;
    }
    return numArray;
  }

  private IVbaProject ParseVbaProject()
  {
    this.m_vbaProject = (IVbaProject) new VbaProjectImpl(this, this.MacroStorage);
    if (this.VbaModule == null)
      this.AddVbaModule();
    this.VbaModule.CodeNameChanged += new NameChangedEventHandler(this.CodeNameChanged);
    foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) this.Worksheets)
    {
      if ((worksheet as WorksheetImpl).ParseOnDemand)
        (worksheet as WorksheetImpl).Parse();
      if ((worksheet as WorksheetImpl).VbaModule == null)
        (worksheet as WorksheetImpl).AddVbaModule();
      (worksheet as WorksheetImpl).VbaModule.CodeNameChanged += new NameChangedEventHandler(this.CodeNameChanged);
    }
    return this.m_vbaProject;
  }

  private void CodeNameChanged(object sender, string name) => this.m_strCodeName = name;

  private VbaModule AddVbaModule()
  {
    VbaModule vbaModule = (VbaModule) null;
    if (this.m_vbaProject != null)
    {
      if (string.IsNullOrEmpty(this.CodeName))
        this.m_strCodeName = "ThisWorkbook";
      vbaModule = this.m_vbaProject.Modules.Add(this.CodeName, VbaModuleType.Document) as VbaModule;
      vbaModule.InitializeAttributes(this.CodeName, "0{00020819-0000-0000-C000-000000000046}");
      vbaModule.CodeNameChanged += new NameChangedEventHandler(this.CodeNameChanged);
    }
    return vbaModule;
  }

  private void CreateVbaProject()
  {
    this.VbaProject = (IVbaProject) new VbaProjectImpl(this);
    this.AddVbaModule();
    foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) this.Worksheets)
      (worksheet as WorksheetImpl).AddVbaModule();
    this.HasMacros = true;
  }

  internal void SaveMacroStorage(ICompoundStorage rootStorage, bool isBinary)
  {
    ICompoundStorage vbaRootStorage = !isBinary ? rootStorage : rootStorage.CreateStorage("_VBA_PROJECT_CUR");
    if (this.m_vbaProject == null && this.MacroStorage != null)
    {
      foreach (string storage in this.MacroStorage.Storages)
      {
        using (ICompoundStorage storageToCopy = this.MacroStorage.OpenStorage(storage))
          vbaRootStorage.InsertCopy(storageToCopy);
      }
      foreach (string stream in this.MacroStorage.Streams)
      {
        using (CompoundStream streamToCopy = this.MacroStorage.OpenStream(stream))
          vbaRootStorage.InsertCopy(streamToCopy);
      }
    }
    else
    {
      if (this.VbaProject == null)
        return;
      (this.VbaProject as VbaProjectImpl).Save(vbaRootStorage);
    }
  }

  internal ExtendedFormatImpl AddExtendedProperties(ExtendedFormatImpl m_xFormat)
  {
    if (m_xFormat.Index != this.DefaultXFIndex)
    {
      if (m_xFormat.Properties.Count > 0)
        m_xFormat.Properties.Clear();
      if (m_xFormat.FillPattern == ExcelPattern.Solid)
      {
        if (m_xFormat.ColorObject.ColorType == ColorType.RGB || m_xFormat.ColorObject.ColorType == ColorType.Theme)
          this.AddExtendedProperty(CellPropertyExtensionType.ForeColor, m_xFormat.Color, m_xFormat);
        if (m_xFormat.PatternColorObject.ColorType == ColorType.RGB || m_xFormat.PatternColorObject.ColorType == ColorType.Theme)
          this.AddExtendedProperty(CellPropertyExtensionType.BackColor, m_xFormat.PatternColor, m_xFormat);
      }
      else
      {
        if (m_xFormat.ColorObject.ColorType == ColorType.RGB || m_xFormat.ColorObject.ColorType == ColorType.Theme)
          this.AddExtendedProperty(CellPropertyExtensionType.BackColor, m_xFormat.Color, m_xFormat);
        if (m_xFormat.PatternColorObject.ColorType == ColorType.RGB || m_xFormat.PatternColorObject.ColorType == ColorType.Theme)
          this.AddExtendedProperty(CellPropertyExtensionType.ForeColor, m_xFormat.PatternColor, m_xFormat);
      }
      if ((m_xFormat.TopBorderColor.ColorType == ColorType.RGB || m_xFormat.TopBorderColor.ColorType == ColorType.Theme) && m_xFormat.TopBorderLineStyle != ExcelLineStyle.None)
        this.AddExtendedProperty(CellPropertyExtensionType.TopBorderColor, m_xFormat.Borders[ExcelBordersIndex.EdgeTop].ColorRGB, m_xFormat);
      if ((m_xFormat.BottomBorderColor.ColorType == ColorType.RGB || m_xFormat.BottomBorderColor.ColorType == ColorType.Theme) && m_xFormat.BottomBorderLineStyle != ExcelLineStyle.None)
        this.AddExtendedProperty(CellPropertyExtensionType.BottomBorderColor, m_xFormat.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB, m_xFormat);
      if ((m_xFormat.LeftBorderColor.ColorType == ColorType.RGB || m_xFormat.LeftBorderColor.ColorType == ColorType.Theme) && m_xFormat.LeftBorderLineStyle != ExcelLineStyle.None)
        this.AddExtendedProperty(CellPropertyExtensionType.LeftBorderColor, m_xFormat.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB, m_xFormat);
      if ((m_xFormat.RightBorderColor.ColorType == ColorType.RGB || m_xFormat.RightBorderColor.ColorType == ColorType.Theme) && m_xFormat.RightBorderLineStyle != ExcelLineStyle.None)
        this.AddExtendedProperty(CellPropertyExtensionType.RightBorderColor, m_xFormat.Borders[ExcelBordersIndex.EdgeRight].ColorRGB, m_xFormat);
      if ((m_xFormat.DiagonalBorderColor.ColorType == ColorType.RGB || m_xFormat.DiagonalBorderColor.ColorType == ColorType.Theme) && m_xFormat.DiagonalDownBorderLineStyle != ExcelLineStyle.None)
        this.AddExtendedProperty(CellPropertyExtensionType.DiagonalCellBorder, m_xFormat.Borders[ExcelBordersIndex.DiagonalDown].ColorRGB, m_xFormat);
      if (m_xFormat.IndentLevel > 15)
        m_xFormat.Properties.Add(new ExtendedProperty()
        {
          Type = CellPropertyExtensionType.TextIndentationLevel,
          Size = (ushort) 6,
          Indent = (ushort) m_xFormat.IndentLevel
        });
      FontImpl innerFont = this.InnerFonts[m_xFormat.FontIndex] as FontImpl;
      if (innerFont.ColorObject.ColorType == ColorType.RGB || innerFont.ColorObject.ColorType == ColorType.Theme)
        this.AddExtendedProperty(CellPropertyExtensionType.TextColor, m_xFormat.Font.RGBColor, m_xFormat);
    }
    return m_xFormat;
  }

  internal void AddExtendedProperty(
    CellPropertyExtensionType type,
    Color ColorValue,
    ExtendedFormatImpl m_xFormat)
  {
    ColorValue = this.ConvertARGBToRGBA(ColorValue);
    m_xFormat.Properties.Add(new ExtendedProperty()
    {
      Type = this.GetPropertyType(type),
      Size = (ushort) 20,
      ColorValue = this.ColorToUInt(ColorValue)
    });
  }

  internal ExtendedProperty CreateExtendedProperty(
    CellPropertyExtensionType type,
    Color colorValue,
    ColorType colorType)
  {
    colorValue = this.ConvertARGBToRGBA(colorValue);
    return new ExtendedProperty()
    {
      Type = this.GetPropertyType(type),
      Size = 20,
      ColorValue = this.ColorToUInt(colorValue)
    };
  }

  internal CellPropertyExtensionType GetPropertyType(CellPropertyExtensionType type)
  {
    switch (type)
    {
      case CellPropertyExtensionType.ForeColor:
        type = CellPropertyExtensionType.ForeColor;
        break;
      case CellPropertyExtensionType.BackColor:
        type = CellPropertyExtensionType.BackColor;
        break;
      case CellPropertyExtensionType.GradientFill:
        type = CellPropertyExtensionType.GradientFill;
        break;
      case CellPropertyExtensionType.TopBorderColor:
        type = CellPropertyExtensionType.TopBorderColor;
        break;
      case CellPropertyExtensionType.BottomBorderColor:
        type = CellPropertyExtensionType.BottomBorderColor;
        break;
      case CellPropertyExtensionType.LeftBorderColor:
        type = CellPropertyExtensionType.LeftBorderColor;
        break;
      case CellPropertyExtensionType.RightBorderColor:
        type = CellPropertyExtensionType.RightBorderColor;
        break;
      case CellPropertyExtensionType.DiagonalCellBorder:
        type = CellPropertyExtensionType.DiagonalCellBorder;
        break;
      case CellPropertyExtensionType.TextColor:
        type = CellPropertyExtensionType.TextColor;
        break;
      case CellPropertyExtensionType.FontScheme:
        type = CellPropertyExtensionType.FontScheme;
        break;
      case CellPropertyExtensionType.TextIndentationLevel:
        type = CellPropertyExtensionType.TextIndentationLevel;
        break;
    }
    return type;
  }

  internal Color ConvertARGBToRGBA(Color colorValue)
  {
    byte b = colorValue.B;
    byte g = colorValue.G;
    byte r = colorValue.R;
    colorValue = Color.FromArgb((int) colorValue.A, (int) b, (int) g, (int) r);
    return colorValue;
  }

  internal Color ConvertRGBAToARGB(Color colorValue)
  {
    colorValue = Color.FromArgb((int) colorValue.A, (int) colorValue.B, (int) colorValue.G, (int) colorValue.R);
    return colorValue;
  }

  internal uint ColorToUInt(Color color)
  {
    return (uint) ((int) color.A << 24 | (int) color.R << 16 /*0x10*/ | (int) color.G << 8) | (uint) color.B;
  }

  internal Color UIntToColor(uint color)
  {
    return Color.FromArgb((int) (byte) (color >> 24), (int) (byte) (color >> 16 /*0x10*/), (int) (byte) (color >> 8), (int) (byte) color);
  }

  private bool IsLegalXmlChar(int character)
  {
    if (character == 9 || character == 10 || character == 13 || character >= 32 /*0x20*/ && character <= 55295 || character >= 57344 /*0xE000*/ && character <= 65533)
      return true;
    return character >= 65536 /*0x010000*/ && character <= 1114111;
  }

  internal string RemoveInvalidXmlCharacters(string nameValue)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(nameValue);
    bool flag = false;
    for (int index = 0; index < bytes.Length; ++index)
    {
      if (!this.IsLegalXmlChar((int) bytes[index]) && bytes[index] != (byte) 0)
      {
        bytes[index] = (byte) 0;
        flag = true;
      }
    }
    string str = Encoding.UTF8.GetString(bytes, 0, bytes.Length).Replace("\0", string.Empty);
    if (flag)
    {
      str = $"_{(object) this.XmlInvalidCharCount}{str}";
      ++this.XmlInvalidCharCount;
    }
    return str;
  }

  private void UpdateStandardRowHeight()
  {
    IWorksheet sheet = this.Worksheets.Create();
    IRange range = sheet["A1"];
    range.Text = "A";
    range.WrapText = true;
    range.CellStyle.Font.FontName = this.StandardFont;
    range.CellStyle.Font.Size = this.StandardFontSize;
    range.AutofitRows();
    this.StandardRowHeight = sheet.GetRowHeight(1);
    this.Worksheets.Remove(sheet);
  }

  public void ImportXml(string fileName)
  {
    if (!File.Exists(fileName))
      throw new FileNotFoundException($"File {fileName} could not be found. Please verify the file path.");
    using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
      this.ImportXml((Stream) fileStream);
  }

  public void ImportXml(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException("Stream");
    IWorksheet sheet = this.Worksheets.Create();
    if (this.m_xmlMaps == null)
      this.m_xmlMaps = new XmlMapCollection(this);
    if (this.m_xmlMaps.Add(stream, sheet, 1, 1))
      return;
    this.Worksheets.Remove(sheet);
  }

  internal void ParseXmlMaps(XmlReader reader)
  {
    if (this.m_xmlMaps == null)
      this.m_xmlMaps = new XmlMapCollection(this);
    this.m_xmlMaps.Parse(reader);
  }

  internal double[] GetCellScaledWidthHeight(IWorksheet sheet)
  {
    string standardFont = this.StandardFont;
    double standardFontSize = this.StandardFontSize;
    double[] scaledWidthHeight = new double[2]{ 1.0, 1.0 };
    if (standardFont == "Calibri" && standardFontSize == 10.0)
    {
      scaledWidthHeight[0] = 649.0 / 675.0;
      scaledWidthHeight[1] = 186.0 / 181.0;
    }
    else if (standardFont == "Calibri" && standardFontSize == 11.0)
    {
      scaledWidthHeight[0] = 649.0 / 603.0;
      scaledWidthHeight[1] = 930.0 / 959.0;
    }
    else if (standardFont == "Arial" && standardFontSize == 9.0)
    {
      scaledWidthHeight[0] = 649.0 / 675.0;
      scaledWidthHeight[1] = 310.0 / 333.0;
    }
    else if (standardFont == "Arial" && standardFontSize == 10.0)
    {
      scaledWidthHeight[0] = sheet.PageSetup.Orientation != ExcelPageOrientation.Portrait ? 504.0 / 479.0 : 1.05504950495;
      scaledWidthHeight[1] = 0.97064110245;
    }
    else if (standardFont == "Arial" && standardFontSize == 11.0)
    {
      scaledWidthHeight[0] = 649.0 / 636.0;
      scaledWidthHeight[1] = 930.0 / 967.0;
    }
    return scaledWidthHeight;
  }

  internal double GetScaledHeight(string fontName, double fontSize, IWorksheet sheet)
  {
    if (fontName == "Calibri" && fontSize == 10.0)
      return 186.0 / 181.0;
    if (fontName == "Calibri" && fontSize == 11.0)
      return 0.89540004666666673;
    if (fontName == "Arial" && fontSize == 8.0)
      return 184.0 / 225.0;
    if (fontName == "Arial" && fontSize == 9.0)
      return 310.0 / 333.0;
    if (fontName == "Arial" && fontSize == 10.0)
      return 46.0 / 51.0;
    if (fontName == "Arial" && fontSize == 11.0)
      return 0.88771934035087718;
    if (fontName == "Arial" && fontSize == 12.0)
      return 0.9201500415802002;
    return fontName == "Verdana" && fontSize == 10.0 ? 0.95320166793524053 : this.GetCellScaledWidthHeight(sheet)[1];
  }

  internal void AddWatermark(IWorkbook workbook)
  {
    IWorksheet worksheet1 = workbook.Worksheets["Evaluation Warning"];
    if (worksheet1 != null && worksheet1["A10"].Text.Equals("Created with a trial version of Syncfusion Essential XlsIO", StringComparison.InvariantCultureIgnoreCase))
      workbook.Worksheets.Remove(worksheet1);
    IWorksheet worksheet2 = workbook.Worksheets.Create("Evaluation Warning");
    worksheet2.TabColorRGB = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 69, 0);
    worksheet2.Protect("Essential XlsIO");
    worksheet2.Activate();
    IRange range = worksheet2["A10"];
    range.Text = "Created with a trial version of Syncfusion Essential XlsIO";
    IStyle cellStyle = range.CellStyle;
    cellStyle.BeginUpdate();
    cellStyle.Font.Size = 14.0;
    cellStyle.Font.Bold = true;
    cellStyle.Font.RGBColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
    cellStyle.EndUpdate();
    range.RowHeight = 25.0;
  }

  internal Font GetSystemFont(IFont font, string fontName)
  {
    FontStyle fontStyle = this.GetFontStyle(font);
    return new Font(fontName, this.GetFontSize(font), fontStyle);
  }

  internal FontStyle GetFontStyle(IFont font)
  {
    FontStyle fontStyle = FontStyle.Regular;
    if (font.Bold)
      fontStyle |= FontStyle.Bold;
    if (font.Italic)
      fontStyle |= FontStyle.Italic;
    if (font.Strikethrough)
      fontStyle |= FontStyle.Strikeout;
    if (font.Underline != ExcelUnderline.None)
      fontStyle |= FontStyle.Underline;
    return fontStyle;
  }

  internal float GetFontSize(IFont font)
  {
    float size = (float) font.Size;
    float fontSize = (double) size == 0.0 ? 0.5f : size;
    if (font.Superscript || font.Subscript)
      fontSize /= 1.5f;
    return fontSize;
  }

  internal Font GetFont(IFont font)
  {
    FontStyle fontStyle = this.GetFontStyle(font);
    return new Font(font.FontName, (float) font.Size, fontStyle);
  }

  internal bool IsNullOrWhiteSpace(string text)
  {
    if (text == null)
      return true;
    foreach (char c in text)
    {
      if (!char.IsWhiteSpace(c))
        return false;
    }
    return true;
  }

  public class WorkbookExcel97Serializator : IWorkbookSerializator
  {
    private const int MaximumPassordLength = 15;
    private const string FOLLOWED_HYPERLINK = "Followed Hyperlink";
    private const string HYPERLINK = "Hyperlink";
    private IdReserver m_shapeIds;

    public WorkbookExcel97Serializator(IdReserver shapeIds) => this.m_shapeIds = shapeIds;

    public void Serialize(string fullName, WorkbookImpl book, ExcelSaveType saveType)
    {
      if (fullName == null || fullName.Length == 0)
        throw new ArgumentOutOfRangeException(nameof (fullName));
      if (book == null)
        throw new ArgumentNullException(nameof (book));
      IEncryptor encryptor = (IEncryptor) null;
      if (book.m_encryptionType != ExcelEncryptionType.None)
        encryptor = this.CreateEncryptor(book.m_encryptionType, book);
      OffsetArrayList records = new OffsetArrayList();
      this.Serialize(records, saveType, encryptor, book, (WorksheetImpl) null, false);
      records.UpdateBiffRecordsOffsets();
      using (ICompoundFile compoundFile = book.AppImplementation.CreateCompoundFile(fullName, STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE))
      {
        this.SaveToStgStream(compoundFile.RootStorage, false, records, encryptor, book);
        compoundFile.Flush();
      }
    }

    public void Serialize(Stream stream, WorkbookImpl book, ExcelSaveType saveType)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      if (book == null)
        throw new ArgumentNullException(nameof (book));
      IEncryptor encryptor = (IEncryptor) null;
      if (book.m_encryptionType != ExcelEncryptionType.None)
        encryptor = this.CreateEncryptor(book.m_encryptionType, book);
      OffsetArrayList records = new OffsetArrayList();
      this.Serialize(records, saveType, encryptor, book, (WorksheetImpl) null, false);
      records.UpdateBiffRecordsOffsets();
      using (ICompoundFile compoundFile = book.AppImplementation.CreateCompoundFile())
      {
        this.SaveToStgStream(compoundFile.RootStorage, false, records, encryptor, book);
        compoundFile.Save(stream);
      }
    }

    [CLSCompliant(false)]
    public void Serialize(
      OffsetArrayList records,
      ExcelSaveType saveType,
      IEncryptor encryptor,
      WorkbookImpl book,
      WorksheetImpl sheet,
      bool forClipboard)
    {
      this.Serialize(records, saveType, encryptor, book, sheet, forClipboard, (IRange) null);
    }

    internal void Serialize(
      OffsetArrayList records,
      ExcelSaveType saveType,
      IEncryptor encryptor,
      WorkbookImpl book,
      WorksheetImpl sheet,
      bool forClipboard,
      IRange range)
    {
      this.OptimizeStyles(book);
      this.OptimizeExtFormats();
      this.OptimizeFonts();
      records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.BOF));
      if (!forClipboard)
      {
        if (saveType == ExcelSaveType.SaveAsTemplate)
          records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.Template));
        if (book.m_bWriteProtection)
          records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.WriteProtection));
        if (encryptor != null)
          records.Add((IBiffStorage) encryptor.GetFilePassRecord());
        records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.InterfaceHdr));
        records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.MMS));
        records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.InterfaceEnd));
        WriteAccessRecord record = (WriteAccessRecord) BiffRecordFactory.GetRecord(TBIFFRecord.WriteAccess);
        record.UserName = book.Author;
        records.Add((IBiffStorage) record);
        if (book.m_fileSharing != null)
          records.Add((IBiffStorage) book.m_fileSharing);
      }
      records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.Codepage));
      records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.DSF));
      records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.UnkBegin));
      if (!forClipboard)
      {
        if (book.m_worksheets.Count == 0 && book.TabSheets.Count == 0)
          throw new ApplicationException("Workbook must contains at least one worksheet");
        TabIdRecord record = (TabIdRecord) BiffRecordFactory.GetRecord(TBIFFRecord.TabId);
        record.TabIds = new ushort[book.m_worksheets.Count + book.m_charts.Count];
        int Index = 0;
        for (int count = book.m_worksheets.Count; Index < count; ++Index)
          record.TabIds[Index] = (ushort) (book.m_worksheets[Index].Index + 1);
        int count1 = book.m_worksheets.Count;
        for (int length = record.TabIds.Length; count1 < length; ++count1)
          record.TabIds[count1] = (ushort) (count1 + 1);
        records.Add((IBiffStorage) record);
      }
      if (!forClipboard && (book.Application.SkipOnSave & SkipExtRecords.Macros) != SkipExtRecords.Macros && book.m_bHasMacros)
      {
        records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.HasBasic));
        if (book.m_bMacrosDisable)
          records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.UnkMacrosDisable));
        CodeNameRecord record = (CodeNameRecord) BiffRecordFactory.GetRecord(TBIFFRecord.CodeName);
        record.CodeName = book.CodeName;
        records.Add((IBiffStorage) record);
      }
      records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.FnGroupCount));
      if (!forClipboard)
      {
        WindowProtectRecord record1 = (WindowProtectRecord) BiffRecordFactory.GetRecord(TBIFFRecord.WindowProtect);
        record1.IsProtected = book.IsWindowProtection;
        records.Add((IBiffStorage) record1);
        ProtectRecord record2 = (ProtectRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Protect);
        record2.IsProtected = book.IsCellProtection;
        records.Add((IBiffStorage) record2);
        records.Add((IBiffStorage) book.Password);
        records.Add((IBiffStorage) book.ProtectionRev4);
        records.Add((IBiffStorage) book.PasswordRev4);
      }
      else if (sheet != null)
      {
        OleSizeRecord record = (OleSizeRecord) BiffRecordFactory.GetRecord(TBIFFRecord.OleSize);
        record.FirstRow = (ushort) (sheet.FirstRow - 1);
        record.FirstColumn = (byte) (sheet.FirstColumn - 1);
        record.LastRow = (ushort) (sheet.LastRow - 1);
        record.LastColumn = (byte) (sheet.LastColumn - 1);
        records.Add((IBiffStorage) record);
      }
      if (!forClipboard)
      {
        records.Add((IBiffStorage) book.WindowOne);
      }
      else
      {
        WindowOneRecord windowOneRecord = (WindowOneRecord) book.WindowOne.Clone();
        windowOneRecord.SelectedTab = (ushort) 0;
        windowOneRecord.DisplayedTab = (ushort) 0;
        records.Add((IBiffStorage) windowOneRecord);
      }
      if (!forClipboard)
      {
        records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.Backup));
        records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.HideObj));
        DateWindow1904Record record3 = (DateWindow1904Record) BiffRecordFactory.GetRecord(TBIFFRecord.DateWindow1904);
        record3.Is1904Windowing = book.Date1904;
        records.Add((IBiffStorage) record3);
        PrecisionRecord record4 = (PrecisionRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Precision);
        record4.IsPrecision = book.PrecisionAsDisplayed ? (ushort) 0 : (ushort) 1;
        records.Add((IBiffStorage) record4);
        records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.RefreshAll));
        records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.BookBool));
      }
      if (book.m_compatibility == null)
        book.m_compatibility = new CompatibilityRecord();
      records.Add((IBiffStorage) book.m_compatibility);
      book.m_fonts.Serialize(records);
      book.m_rawFormats.Serialize(records);
      uint[] crcCache = WorkbookImpl.InitCRC();
      int index1 = 0;
      for (int count = book.m_extFormats.Count; index1 < count; ++index1)
      {
        ExtendedFormatImpl extFormat = book.m_extFormats[index1];
        extFormat.Serialize(records, crcCache);
        if (extFormat.XFRecord != null)
          extFormat.XFRecord.XFIndex = (ushort) extFormat.Index;
      }
      records.Add((IBiffStorage) new ExtendedFormatCRC()
      {
        XFCount = (ushort) book.m_extFormats.Count,
        CRCChecksum = book.crcValue
      });
      int index2 = 0;
      for (int count = book.m_extFormats.Count; index2 < count; ++index2)
        book.m_extFormats[index2].SerializeXFormat(records);
      int num = 0;
      for (int count2 = book.m_styles.Count; num < count2; ++num)
      {
        StyleImpl style = book.m_styles[num] as StyleImpl;
        if (style.Name != "Followed Hyperlink" && style.Name != "Hyperlink")
          style.Serialize(records);
        int count3 = book.m_arrStyleExtRecords.Count;
        if (count3 > 0 && num < count3)
          records.Add((IBiffStorage) book.m_arrStyleExtRecords[num]);
      }
      if (book.m_bOwnPalette)
      {
        PaletteRecord record = (PaletteRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Palette);
        PaletteRecord.TColor[] tcolorArray = new PaletteRecord.TColor[book.m_colors.Count - 8];
        for (int index3 = 0; index3 < tcolorArray.Length; ++index3)
        {
          tcolorArray[index3].A = book.m_colors[index3 + 8].A;
          tcolorArray[index3].R = book.m_colors[index3 + 8].R;
          tcolorArray[index3].G = book.m_colors[index3 + 8].G;
          tcolorArray[index3].B = book.m_colors[index3 + 8].B;
        }
        record.Colors = tcolorArray;
        records.Add((IBiffStorage) record);
      }
      this.SerializePivotCachesInfo(records, book);
      if (!forClipboard)
      {
        UseSelFSRecord record = (UseSelFSRecord) BiffRecordFactory.GetRecord(TBIFFRecord.UseSelFS);
        record.Flags = book.m_bSelFSUsed;
        records.Add((IBiffStorage) record);
      }
      if (!forClipboard || sheet == null)
      {
        int index4 = 0;
        for (int count = book.m_arrObjects.Count; index4 < count; ++index4)
        {
          INamedObject arrObject = (INamedObject) book.m_arrObjects[index4];
          records.Add((IBiffStorage) this.CreateBoundSheet(arrObject));
        }
      }
      else
      {
        BoundSheetRecord boundSheet = this.CreateBoundSheet((INamedObject) sheet);
        boundSheet.SheetIndex = 0;
        records.Add((IBiffStorage) boundSheet);
      }
      CountryRecord record5 = (CountryRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Country);
      record5.DefaultCountry = (ushort) book.m_iCountry;
      record5.CurrentCountry = (ushort) book.m_iCountry;
      records.Add((IBiffStorage) record5);
      book.m_externBooks.Serialize(records);
      if (book.m_externSheet.RefList != null && book.m_externSheet.RefList.Count != 0)
        records.Add((IBiffStorage) book.m_externSheet);
      if (book.m_continue != null)
      {
        foreach (ContinueRecord continueRecord in book.m_continue)
          records.Add((IBiffStorage) continueRecord);
      }
      this.SerializeNames(records, book);
      if (book.EnabledCalcEngine)
      {
        book.m_reCalcId.CalcIdentifier = book.CalcIdentifier;
        records.Add((IBiffStorage) book.m_reCalcId);
      }
      else
        records.Add((IBiffStorage) book.m_reCalcId);
      if (book.m_drawGroup != null && book.m_shapesData.Pictures.Count == 0 && book.m_headerFooterPictures.PreservedClusters == null && book.m_headerFooterPictures.Pictures.Count == 0)
        records.Add((IBiffStorage) book.m_drawGroup);
      else if (!forClipboard)
        this.SerializeMsoDrawings(records, book);
      book.m_SSTDictionary.Serialize(records);
      if (book.m_bookExt != null)
        records.Add((IBiffStorage) book.m_bookExt);
      if (book.m_theme != null)
        records.Add((IBiffStorage) book.m_theme);
      records.AddList((IList) book.PreserveExternalConnectionDetails);
      records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.EOF));
      if (!forClipboard || sheet == null)
      {
        int index5 = 0;
        for (int count = book.m_arrObjects.Count; index5 < count; ++index5)
          ((WorksheetBaseImpl) book.m_arrObjects[index5]).Serialize(records);
      }
      else
        sheet.SerializeForClipboard(records, true, range);
    }

    private void SerializePivotCachesInfo(OffsetArrayList records, WorkbookImpl book)
    {
      if (records == null)
        throw new ArgumentNullException(nameof (records));
      if (book == null)
        throw new ArgumentNullException(nameof (book));
      if (book.m_pivotCaches == null)
        return;
      foreach (int id in book.m_pivotCaches.Order)
        book.m_pivotCaches[id].Info?.Serialize(records);
    }

    private void SerializeMsoDrawings(OffsetArrayList records, WorkbookImpl book)
    {
      if (records == null)
        throw new ArgumentNullException(nameof (records));
      if (book.m_headerFooterPictures != null)
        book.m_headerFooterPictures.SerializeMsoDrawingGroup(records, TBIFFRecord.HeaderFooterImage, (IdReserver) null);
      if (book.m_shapesData == null || (book.Application.SkipOnSave & SkipExtRecords.Drawings) == SkipExtRecords.Drawings)
        return;
      book.m_shapesData.SerializeMsoDrawingGroup(records, TBIFFRecord.MSODrawingGroup, this.m_shapeIds);
    }

    private IEncryptor CreateEncryptor(ExcelEncryptionType encryptionType, WorkbookImpl book)
    {
      if (encryptionType != ExcelEncryptionType.Standard)
        throw new NotSupportedException("Not supported encryption type.");
      IEncryptor encryptor = (IEncryptor) new MD5Decryptor();
      if (book.m_arrDocId == null)
      {
        Guid guid = Guid.NewGuid();
        book.m_arrDocId = guid.ToByteArray();
      }
      string password = book.m_strEncryptionPassword != null ? book.m_strEncryptionPassword : "VelvetSweatshop";
      if (password != null && password.Length > 15)
        throw new ArgumentOutOfRangeException("PasswordToOpen", "Password too long. Maximum password length is 15 characters.");
      encryptor.SetEncryptionInfo(book.m_arrDocId, password);
      return encryptor;
    }

    private void OptimizeExtFormats()
    {
    }

    private void OptimizeFonts()
    {
    }

    private void OptimizeStyles(WorkbookImpl book)
    {
    }

    private BoundSheetRecord CreateBoundSheet(INamedObject namedObject)
    {
      BoundSheetRecord record = (BoundSheetRecord) BiffRecordFactory.GetRecord(TBIFFRecord.BoundSheet);
      record.SheetName = namedObject.Name;
      switch (namedObject)
      {
        case IWorksheet _:
          this.FillBoundSheet(record, (WorksheetImpl) namedObject);
          break;
        case IChart _:
          this.FillBoundSheet(record, (ChartImpl) namedObject);
          break;
        default:
          throw new ArgumentOutOfRangeException("namedObject has wrong type");
      }
      return record;
    }

    private void SaveToStgStream(
      ICompoundStorage storage,
      bool bDisposeAfterSave,
      OffsetArrayList records,
      IEncryptor encryptor,
      WorkbookImpl book)
    {
      if (storage == null)
        throw new ArgumentNullException(nameof (storage));
      if (book.CalculationOptions.CalculationMode == ExcelCalculationMode.Automatic || book.CalculationOptions.CalculationMode == ExcelCalculationMode.AutomaticExceptTables)
      {
        int Index = 0;
        for (int count = book.Worksheets.Count; Index < count; ++Index)
        {
          WorksheetImpl worksheet = book.Worksheets[Index] as WorksheetImpl;
          if (!worksheet.ParseOnDemand && !worksheet.IsSubtotal && book.IsCellModified)
            worksheet.CellRecords.Table.UpdateFormulaFlags();
        }
      }
      try
      {
        using (BiffWriter biffWriter = new BiffWriter((Stream) storage.CreateStream("Workbook"), true))
          biffWriter.WriteRecord(records, encryptor);
        this.WritePivotCaches(storage, book, encryptor);
        this.CopySourceData(storage, book);
        this.CopyOleData(storage, book);
        if ((book.Application.SkipOnSave & SkipExtRecords.Macros) != SkipExtRecords.Macros && book.HasMacros)
          book.SaveMacroStorage(storage, true);
        bool flag = false;
        if (storage is Storage nativeStorage)
        {
          this.SerializeDocumentPropertiesNative(book, nativeStorage);
          flag = true;
          if (book.m_controlsStream != null)
          {
            CompoundStream stream = storage.CreateStream("Ctls");
            book.m_controlsStream.Position = 0L;
            stream.Position = 0L;
            UtilityMethods.CopyStreamTo(book.m_controlsStream, (Stream) stream);
            stream.Flush();
            stream.Close();
          }
        }
        if (!flag)
          this.SerializeDocumentPropertiesManaged(book, storage);
        if (book.CustomXmlparts == null || book.CustomXmlparts.Count <= 0)
          return;
        new MsoDataStore(storage, book).SerializeMetaStore();
      }
      catch (Exception ex)
      {
        throw;
      }
      finally
      {
        storage.Flush();
        if (bDisposeAfterSave)
          storage.Dispose();
      }
    }

    private void SerializeDocumentPropertiesNative(WorkbookImpl book, Storage nativeStorage)
    {
      if ((book.Application.SkipOnSave & SkipExtRecords.SummaryInfo) == SkipExtRecords.SummaryInfo)
        return;
      IPropertySetStorage ppPropSetStg = (IPropertySetStorage) null;
      Guid guid = new Guid("0000013a-0000-0000-c000-000000000046");
      int propSetStg = Syncfusion.CompoundFile.XlsIO.Native.API.StgCreatePropSetStg(nativeStorage.COMStorage, 0U, out ppPropSetStg);
      if (propSetStg != 0)
        throw new ExternalException("Cannot create Storage properties stream", propSetStg);
      book.m_builtInDocumentProperties.Serialize(ppPropSetStg);
      book.m_customDocumentProperties.Serialize(ppPropSetStg);
      Marshal.FinalReleaseComObject((object) ppPropSetStg);
    }

    private void CopyOleData(ICompoundStorage storage, WorkbookImpl book)
    {
      if (!book.HasOleObjects || !book.IsOleObjectCopied)
        return;
      foreach (string oleStoragesName in book.OleStorageCollection.OleStoragesNames)
      {
        OleStorage oleStorage = book.OleStorageCollection.OpenStorage(oleStoragesName);
        ICompoundStorage storage1 = storage.CreateStorage(oleStoragesName);
        foreach (string streamName in oleStorage.StreamNames)
        {
          MemoryStream source = oleStorage.OpenStream(streamName);
          CompoundStream stream = storage1.CreateStream(streamName);
          stream.Position = 0L;
          source.Position = 0L;
          UtilityMethods.CopyStreamTo((Stream) source, (Stream) stream);
          stream.Flush();
          stream.Close();
        }
      }
      foreach (string arrayStreamName in book.OleStorageCollection.ArrayStreamNames)
      {
        MemoryStream source = book.OleStorageCollection.OpenStream(arrayStreamName);
        CompoundStream stream = storage.CreateStream(arrayStreamName);
        stream.Position = 0L;
        source.Position = 0L;
        UtilityMethods.CopyStreamTo((Stream) source, (Stream) stream);
        stream.Flush();
        stream.Close();
      }
    }

    private void SerializeDocumentPropertiesManaged(WorkbookImpl book, ICompoundStorage storage)
    {
      DocumentPropertyCollection propertyCollection1 = new DocumentPropertyCollection();
      PropertySection summarySection = new PropertySection(Syncfusion.XlsIO.Implementation.Collections.BuiltInDocumentProperties.GuidSummary, -1);
      propertyCollection1.Sections.Add(summarySection);
      DocumentPropertyCollection propertyCollection2 = new DocumentPropertyCollection();
      PropertySection documentSection = new PropertySection(Syncfusion.XlsIO.Implementation.Collections.BuiltInDocumentProperties.GuidDocument, -1);
      PropertySection section = new PropertySection(Syncfusion.XlsIO.Implementation.Collections.CustomDocumentProperties.GuidCustom, -1);
      propertyCollection2.Sections.Add(documentSection);
      propertyCollection2.Sections.Add(section);
      book.m_builtInDocumentProperties.Serialize(summarySection, documentSection);
      book.m_customDocumentProperties.Serialize(section);
      if (storage.ContainsStream("\u0005SummaryInformation"))
        storage.DeleteStream("\u0005SummaryInformation");
      Stream stream1 = (Stream) storage.CreateStream("\u0005SummaryInformation");
      stream1.Position = 0L;
      propertyCollection1.Serialize(stream1);
      stream1.Close();
      if (storage.ContainsStream("\u0005DocumentSummaryInformation"))
        storage.DeleteStream("\u0005DocumentSummaryInformation");
      Stream stream2 = (Stream) storage.CreateStream("\u0005DocumentSummaryInformation");
      stream2.Position = 0L;
      propertyCollection2.Serialize(stream2);
      stream2.Close();
    }

    private void SerializeControlProperties(ICompoundStorage storage, WorkbookImpl book)
    {
    }

    private void WritePivotCaches(
      ICompoundStorage storage,
      WorkbookImpl book,
      IEncryptor encryptor)
    {
      PivotCacheCollection pivotCaches = book.PivotCaches;
      if (pivotCaches == null || pivotCaches.Count <= 0)
        return;
      using (ICompoundStorage storage1 = storage.CreateStorage("_SX_DB_CUR"))
      {
        foreach (PivotCacheImpl pivotCacheImpl in pivotCaches)
        {
          string streamName = pivotCacheImpl.StreamId.ToString("X4");
          using (CompoundStream stream = storage1.CreateStream(streamName))
            pivotCacheImpl.Serialize((Stream) stream, encryptor);
        }
      }
    }

    private void CopySourceData(ICompoundStorage storage, WorkbookImpl book)
    {
      if (storage == null)
        throw new ArgumentNullException(nameof (storage));
      if (book == null)
        throw new ArgumentNullException(nameof (book));
      if (book.m_workbookFile == null)
        return;
      ICompoundStorage rootStorage = book.m_workbookFile.RootStorage;
      this.CopySourceSubstorages(storage, rootStorage);
      this.CopySourceSubstreams(storage, rootStorage);
    }

    private void CopySourceSubstorages(ICompoundStorage storage, ICompoundStorage sourceStorage)
    {
      if (storage == null)
        throw new ArgumentNullException(nameof (storage));
      string[] strArray = sourceStorage != null ? sourceStorage.Storages : throw new ArgumentNullException(nameof (sourceStorage));
      int index = 0;
      for (int length = strArray.Length; index < length; ++index)
      {
        if (strArray[index] != "_SX_DB_CUR" && strArray[index] != "MsoDataStore" && strArray[index] != "_VBA_PROJECT_CUR")
        {
          using (ICompoundStorage storageToCopy = sourceStorage.OpenStorage(strArray[index]))
            storage.InsertCopy(storageToCopy);
        }
      }
    }

    private void CopySourceSubstreams(ICompoundStorage storage, ICompoundStorage sourceStorage)
    {
      if (storage == null)
        throw new ArgumentNullException(nameof (storage));
      string[] strArray = sourceStorage != null ? sourceStorage.Streams : throw new ArgumentNullException(nameof (sourceStorage));
      int index = 0;
      for (int length = strArray.Length; index < length; ++index)
      {
        string streamName = strArray[index];
        if (WorkbookImpl.FindStreamCaseInsensitive(storage, streamName) == null)
        {
          using (CompoundStream streamToCopy = sourceStorage.OpenStream(streamName))
            storage.InsertCopy(streamToCopy);
        }
      }
    }

    private void SerializeNames(OffsetArrayList records, WorkbookImpl book)
    {
      if (records == null)
        throw new ArgumentNullException(nameof (records));
      book.m_names.Serialize(records);
    }

    private void FillBoundSheet(BoundSheetRecord bound, WorksheetImpl worksheet)
    {
      bound.SheetIndex = worksheet.RealIndex;
      bound.Visibility = worksheet.Visibility;
      bound.BoundSheetType = (BoundSheetRecord.SheetType) worksheet.Type;
      bound.BOF = worksheet.BOF;
    }

    private void FillBoundSheet(BoundSheetRecord bound, ChartImpl chart)
    {
      bound.SheetIndex = chart.RealIndex;
      bound.Visibility = chart.Visibility;
      bound.BoundSheetType = BoundSheetRecord.SheetType.Chart;
      bound.BOF = chart.BOF;
    }
  }

  internal delegate void WarningEventHandler(string description, WarningType type);

  public delegate ShapeCollectionBase ShapesGetterMethod(ITabSheet sheet);

  public delegate void DigitSizeCallback(RectangleF rect, ref double currentMax);
}
