// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.WorkbookImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.Exceptions;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlSerialization;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class WorkbookImpl : CommonObject, IWorkbook, IParentApplication
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
  internal const string StandardPassword = "VelvetSweatshop";
  internal const char TextQualifier = '"';
  private const RegexOptions DEF_REGEX = RegexOptions.Compiled;
  private const string DEF_BOOK_GROUP = "BookName";
  private const string DEF_SHEET_GROUP = "SheetName";
  internal const int DEF_BOOK_SHEET_INDEX = 65534;
  private const string DEF_FORMAT_STYLE_NAME_START = "Format_";
  private const string EvaluationWarning = "This file was created using the evaluation version of Syncfusion Essential XlsIO.";
  private const string EvaluationSheetName = "Evaluation expired";
  private const int FirstChartColor = 77;
  private const int LastChartColor = 79;
  private const char SheetRangeSeparator = ':';
  internal const int Date1904SystemDifference = 1462;
  internal const string DEF_EXCEL_2013_THEME_VERSION = "153222";
  internal const string DEF_EXCEL_2007_THEME_VERSION = "124226";
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
  private bool m_enabledCalcEngine;
  private List<BiffRecordRaw> m_records;
  private WorksheetBaseImpl m_ActiveSheet;
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
  private bool m_bWorkbookOpening;
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
  private bool m_bOptimization;
  private bool m_b3dRangesInDV;
  private ExternBookCollection m_externBooks;
  private WorkbookShapeDataImpl m_headerFooterPictures;
  private CalculationOptionsImpl m_calcution;
  private int[] pivotCacheIndexes;
  private int m_dxfPriority;
  private FormulaUtil m_formulaUtil;
  private Syncfusion.OfficeChart.Implementation.Collections.Grouping.WorksheetGroup m_sheetGroup;
  private bool m_bDuplicatedNames;
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
  private OfficeVersion m_version;
  private int m_iDefaultXFIndex = 15;
  private FileDataHolder m_fileDataHolder;
  private double m_dMaxDigitWidth;
  private IntPtr m_ptrHeapHandle;
  private BiffRecordRaw m_bookExt;
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
  private bool m_isConverted;
  private List<Stream> m_preservesPivotCache;
  private List<int> m_arrFontIndexes;
  private OfficeParseOptions m_options;
  internal Dictionary<int, int> m_xfCellCount = new Dictionary<int, int>();
  internal bool IsCRCSucceed;
  internal uint crcValue;
  private int beginversion;
  private int m_iLastPivotTableIndex;
  internal Dictionary<string, List<Stream>> m_childElements = new Dictionary<string, List<Stream>>();
  internal int XmlInvalidCharCount = 1;
  private bool m_IsDisposed;
  private List<BiffRecordRaw> m_externalConnection;
  private bool m_bParseOnDemand;
  private RecalcIdRecord m_reCalcId = new RecalcIdRecord();
  private uint m_uCalcIdentifier = 152511;
  private bool m_isCellModified;
  internal OfficeVersion originalVersion;
  private List<string> m_preservedExternalLinks;
  private string m_algorithmName;
  private byte[] m_hashValue;
  private byte[] m_saltValue;
  private uint m_spinCount;
  private readonly string[] m_customPatterns = new string[28]
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
    "MMM/yyyy"
  };
  private string[] m_DateTimePatterns;
  private static Dictionary<OfficeSheetType, string> SheetTypeToName = new Dictionary<OfficeSheetType, string>(5);

  public IWorksheet ActiveSheet
  {
    get => this.m_ActiveSheet as IWorksheet;
    internal set => this.m_ActiveSheet = value as WorksheetBaseImpl;
  }

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
      WorksheetBaseImpl activeSheet = this.m_ActiveSheet;
      this.m_ActiveSheet = this.Objects[value] as WorksheetBaseImpl;
      this.WindowOne.SelectedTab = (ushort) ((ISerializableNamedObject) this.m_ActiveSheet).RealIndex;
      activeSheet?.Unselect(false);
    }
  }

  public string Author
  {
    get => (string) null;
    set
    {
    }
  }

  public string CodeName
  {
    get => this.m_strCodeName;
    set => this.m_strCodeName = value;
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
    get => this.m_bReadOnly;
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

  public IStyles Styles
  {
    [DebuggerStepThrough] get => (IStyles) this.m_styles;
  }

  public IWorksheets Worksheets
  {
    [DebuggerStepThrough] get => (IWorksheets) this.m_worksheets;
  }

  public bool HasMacros
  {
    get => this.m_bHasMacros;
    internal set => this.m_bHasMacros = value;
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
      this.WindowOne.SelectedTab = (ushort) value;
    }
  }

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
      if (value != this.StandardFont)
        this.m_hasStandardFont = true;
      for (int index = 0; index < 4; ++index)
        ((FontImpl) this.m_fonts[0]).FontName = value;
    }
  }

  public bool Allow3DRangesInDataValidation
  {
    get => this.m_b3dRangesInDV;
    set => this.m_b3dRangesInDV = value;
  }

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

  internal ICharts Charts => (ICharts) this.m_charts;

  internal int BookCFPriorityCount
  {
    get => this.m_dxfPriority;
    set => this.m_dxfPriority = value;
  }

  internal bool EnabledCalcEngine
  {
    get => this.m_enabledCalcEngine;
    set => this.m_enabledCalcEngine = value;
  }

  internal OfficeParseOptions Options
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

  public FileDataHolder DataHolder
  {
    get => this.m_fileDataHolder;
    set => this.m_fileDataHolder = value;
  }

  public WorkbookNamesCollection InnerNamesColection
  {
    [DebuggerStepThrough] get => this.m_names;
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
    [DebuggerStepThrough] get => this.m_extFormats;
  }

  public FormatsCollection InnerFormats
  {
    [DebuggerStepThrough] get => this.m_rawFormats;
  }

  public SSTDictionary InnerSST
  {
    [DebuggerStepThrough] get => this.m_SSTDictionary;
  }

  public bool IsWorkbookOpening
  {
    [DebuggerStepThrough] get => this.m_bWorkbookOpening;
    [DebuggerStepThrough] set => this.m_bWorkbookOpening = value;
  }

  public bool Saving
  {
    [DebuggerStepThrough] get => this.m_bSaving;
    [DebuggerStepThrough] internal set => this.m_bSaving = value;
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

  public ExternBookCollection ExternWorkbooks => this.m_externBooks;

  public CalculationOptionsImpl InnerCalculation => this.m_calcution;

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

  public Syncfusion.OfficeChart.Implementation.Collections.Grouping.WorksheetGroup InnerWorksheetGroup
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

  public OfficeVersion Version
  {
    get => this.m_version;
    set
    {
      if (this.m_checkFirst && this.m_version == OfficeVersion.Excel97to2003 && value != OfficeVersion.Excel97to2003)
        this.m_isConverted = true;
      else if (!this.m_checkFirst)
        this.m_checkFirst = true;
      if (value == OfficeVersion.Excel97to2003 && this.m_versioncheck == 0)
      {
        this.beginversion = 1;
        ++this.m_versioncheck;
      }
      else if ((this.m_versioncheck == 0 || this.beginversion == 2) && value != OfficeVersion.Excel97to2003)
      {
        this.beginversion = 0;
        ++this.m_versioncheck;
      }
      if (this.m_version >= OfficeVersion.Excel2007 && value >= OfficeVersion.Excel2007)
      {
        this.m_version = value;
      }
      else
      {
        if (this.m_version == value)
          return;
        this.originalVersion = this.m_version;
        this.m_version = value;
        this.m_bHasMacros = false;
        switch (value)
        {
          case OfficeVersion.Excel97to2003:
            this.m_iMaxRowCount = 65536 /*0x010000*/;
            this.m_iMaxColumnCount = 256 /*0x0100*/;
            this.m_extFormats.SetMaxCount(4075);
            this.m_iMaxXFCount = 4075;
            this.m_extFormats.SetMaxCount(4095 /*0x0FFF*/);
            this.m_iMaxXFCount = 4095 /*0x0FFF*/;
            this.m_iMaxIndent = 15;
            this.ChangeStylesTo97();
            break;
          case OfficeVersion.Excel2007:
          case OfficeVersion.Excel2010:
          case OfficeVersion.Excel2013:
            this.m_iMaxRowCount = 1048576 /*0x100000*/;
            this.m_iMaxColumnCount = 16384 /*0x4000*/;
            this.m_extFormats.SetMaxCount(65000);
            this.m_iMaxXFCount = 65000;
            this.m_iMaxIndent = 250;
            int originalVersion = (int) this.originalVersion;
            this.m_names.Validate();
            break;
        }
        int Index = 0;
        for (int count = this.m_worksheets.Count; Index < count; ++Index)
          ((WorksheetImpl) this.m_worksheets[Index]).Version = value;
        if (value == OfficeVersion.Excel97to2003)
          this.m_SSTDictionary.RemoveUnnecessaryStrings();
        this.InnerNamesColection?.ConvertFullRowColumnNames(value);
      }
    }
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

  protected int InsertSelfSupbook() => this.m_externBooks.InsertSelfSupbook();

  protected internal int AddSheetReference(string sheetName)
  {
    string[] strArray = sheetName.Split(':');
    sheetName = strArray.Length <= 2 ? strArray[0] : throw new ArgumentException(nameof (sheetName));
    string sheetName1 = strArray[strArray.Length - 1];
    IWorksheet worksheet1 = this.Worksheets[sheetName];
    IWorksheet worksheet2 = this.Worksheets[sheetName1];
    if (worksheet1 != null && worksheet2 != null)
      return this.AddSheetReference(worksheet1, worksheet2);
    Match match = WorkbookImpl.ExternSheetRegEx.Match(sheetName);
    if (!match.Success || !(match.Value == sheetName))
      return 0;
    string strBookName = match.Groups["BookName"].Value;
    int length = strBookName.Length;
    int num;
    if (length == 0)
    {
      num = this.AddBrokenSheetReference();
    }
    else
    {
      if (length >= 2)
        strBookName = strBookName.Substring(1, length - 2);
      string strSheetName = match.Groups["SheetName"].Value;
      num = this.AddExternSheetReference(strBookName, strSheetName);
    }
    return num;
  }

  private int AddExternSheetReference(string strBookName, string strSheetName)
  {
    if (strBookName == null)
      throw new ArgumentNullException(nameof (strBookName));
    if (strSheetName == null)
      throw new ArgumentNullException(nameof (strSheetName));
    int num1 = 65534;
    if (strBookName == null || strBookName.Length == 0)
    {
      strBookName = strSheetName;
      strSheetName = (string) null;
    }
    ExternWorkbookImpl externBook = this.m_externBooks[strBookName];
    int num2;
    if (externBook == null)
    {
      int result;
      if (!this.IsWorkbookOpening || this.Version == OfficeVersion.Excel97to2003 || !int.TryParse(strBookName, out result))
        throw new ArgumentNullException("Can't find extern workbook");
      num2 = result - 1;
      externBook = this.m_externBooks[num2];
    }
    else
      num2 = externBook.Index;
    if (strSheetName != null)
      num1 = externBook.IndexOf(strSheetName);
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
    return this.m_externSheet.AddReference(this.InsertSelfSupbook(), (int) ushort.MaxValue, (int) ushort.MaxValue);
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
        else if (firstSheet == index)
          tref.FirstSheet = ushort.MaxValue;
        int lastSheet = (int) tref.LastSheet;
        if (lastSheet > index && lastSheet != (int) ushort.MaxValue && lastSheet != 65534)
          --tref.LastSheet;
        else if (lastSheet == index)
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

  protected internal string GetSheetNameByReference(int reference, bool throwArgumentOutOfRange)
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
      sheetNameByReference = !externBook.IsInternalReference ? this.GetExternalSheetNameByReference(externBook, reference1, supBookIndex) : this.GetInternalSheetNameByReference(reference1);
    }
    catch (Exception ex)
    {
    }
    return sheetNameByReference;
  }

  private string GetExternalSheetNameByReference(
    ExternWorkbookImpl book,
    ExternSheetRecord.TREF reference,
    int iSupBook)
  {
    int firstSheet = (int) reference.FirstSheet;
    string directoryName = this.GetDirectoryName(book.URL);
    string fileName = Path.GetFileName(book.URL);
    string str;
    if (this.m_bSaving)
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
    else
      str = directoryName + (object) '[' + fileName + (object) ']';
    return str + book.GetSheetName(firstSheet);
  }

  private string GetInternalSheetNameByReference(ExternSheetRecord.TREF reference)
  {
    string sheetNameByReference = (string) null;
    int firstSheet = (int) reference.FirstSheet;
    if (firstSheet == (int) ushort.MaxValue)
    {
      sheetNameByReference = "#REF";
    }
    else
    {
      object obj1 = this.ObjectCount > firstSheet && firstSheet >= 0 ? (object) this.Objects[firstSheet] : throw new ParseException();
      if (obj1 is IWorksheet)
        sheetNameByReference = ((ITabSheet) obj1).Name;
      if ((int) reference.FirstSheet != (int) reference.LastSheet)
      {
        object obj2 = (object) this.Objects[(int) reference.LastSheet];
        if (obj2 is IWorksheet)
          sheetNameByReference = $"{sheetNameByReference}:{((ITabSheet) obj2).Name}";
      }
    }
    return sheetNameByReference;
  }

  private string GetDirectoryName(string url)
  {
    if (url == null)
      return (string) null;
    string directoryName;
    if (url.StartsWith("http://"))
    {
      int num = url.LastIndexOf('/');
      directoryName = url.Substring(0, num + 1);
    }
    else
    {
      directoryName = Path.GetDirectoryName(url);
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

  public void RemoveExtenededFormatIndex(int xfIndex)
  {
    Dictionary<int, int> dictFormats = this.m_extFormats.RemoveAt(xfIndex);
    int index = 0;
    for (int count = this.m_arrObjects.Count; index < count; ++index)
      (this.m_arrObjects[index] as WorksheetBaseImpl).UpdateExtendedFormatIndex(dictFormats);
    this.m_styles.UpdateStyleRecords();
  }

  private void AddLicenseWorksheet()
  {
    if (!this.AppImplementation.EvalExpired)
      return;
    IWorksheet worksheet = this.Worksheets.Create("Evaluation expired");
    worksheet.TabColorRGB = ColorExtension.Red;
    worksheet["A1"].Text = "This file was created using the evaluation version of Syncfusion Essential XlsIO.";
    IOfficeFont font = worksheet["A1"].CellStyle.Font;
    font.Size = 14.0;
    font.Bold = true;
    font.RGBColor = ColorExtension.Red;
    string empty = string.Empty;
    Random random = new Random((int) DateTime.Now.Ticks);
    for (int index = 0; index < 10; ++index)
    {
      byte num = (byte) (random.Next(26) + 65);
      empty += (string) (object) (char) num;
    }
    worksheet.Protect(empty, OfficeSheetProtection.All);
    worksheet.Activate();
  }

  private void CheckLicensingSheet()
  {
    if (!this.AppImplementation.EvalExpired)
      return;
    this.m_worksheets["Evaluation expired"]?.Remove();
  }

  private bool CheckProtectionContent(IWorksheet sheet)
  {
    IRange range = sheet["A1"];
    return range.Text == "This file was created using the evaluation version of Syncfusion Essential XlsIO." && range.ColumnWidth > 8.0 && range.RowHeight > 10.0 && range.CellStyle.Font.Size > 10.0 && sheet.TopVisibleRow == 1 && sheet.LeftVisibleColumn == 1 && sheet.Visibility == OfficeWorksheetVisibility.Visible;
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

  static WorkbookImpl()
  {
    WorkbookImpl.SheetTypeToName.Add(OfficeSheetType.Chart, nameof (Charts));
    WorkbookImpl.SheetTypeToName.Add(OfficeSheetType.DialogSheet, "Dialogs");
    WorkbookImpl.SheetTypeToName.Add(OfficeSheetType.Excel4IntlMacroSheet, "Excel 4.0 Intl Marcos");
    WorkbookImpl.SheetTypeToName.Add(OfficeSheetType.Excel4MacroSheet, "Excel 4.0 Macros");
    WorkbookImpl.SheetTypeToName.Add(OfficeSheetType.Worksheet, nameof (Worksheets));
  }

  public WorkbookImpl(IApplication application, object parent, OfficeVersion version)
    : this(application, parent, application.SheetsInNewWorkbook, version)
  {
  }

  public WorkbookImpl(
    IApplication application,
    object parent,
    int sheetQuantity,
    OfficeVersion version)
    : base(application, parent)
  {
    this.InitializeCollections();
    this.Version = version;
    this.InsertDefaultFonts();
    this.InsertDefaultValues();
    this.m_bReadOnly = false;
    this.m_bIsCreated = true;
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
    OfficeVersion version)
    : this(application, parent, FileName, OfficeParseOptions.Default, version)
  {
  }

  public WorkbookImpl(
    IApplication application,
    object parent,
    string strFileName,
    OfficeParseOptions options,
    OfficeVersion version)
    : this(application, parent, strFileName, options, false, (string) null, version)
  {
  }

  public WorkbookImpl(
    IApplication application,
    object parent,
    string strFileName,
    OfficeParseOptions options,
    bool bReadOnly,
    string password,
    OfficeVersion version)
    : base(application, parent)
  {
    this.m_bOptimization = application.OptimizeFonts;
    this.InitializeCollections();
    this.Version = version;
    this.m_strFullName = Path.GetFullPath(strFileName);
  }

  public WorkbookImpl(
    IApplication application,
    object parent,
    Stream stream,
    OfficeParseOptions options,
    bool bReadOnly,
    string password,
    OfficeVersion version)
    : base(application, parent)
  {
    this.m_bOptimization = application.OptimizeFonts;
    this.InitializeCollections();
    this.Version = version;
  }

  public WorkbookImpl(
    IApplication application,
    object parent,
    Stream stream,
    string separator,
    int row,
    int column,
    OfficeVersion version,
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
        this.m_bWorkbookOpening = true;
        if (this.m_ActiveSheet != null)
        {
          bool isValid = this.IsValidDocument(stream, encoding, separator);
          ((WorksheetImpl) this.m_ActiveSheet).Parse((TextReader) streamToRead, separator, row, column, isValid);
          if (fileName != null && fileName.Length > 0)
            this.m_ActiveSheet.Name = Path.GetFileNameWithoutExtension(fileName);
        }
        this.m_bWorkbookOpening = false;
        break;
    }
  }

  public WorkbookImpl(
    IApplication application,
    object parent,
    Stream stream,
    OfficeVersion version)
    : this(application, parent, stream, OfficeParseOptions.Default, version)
  {
  }

  public WorkbookImpl(
    IApplication application,
    object parent,
    Stream stream,
    OfficeParseOptions options,
    OfficeVersion version)
    : base(application, parent)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.m_bOptimization = application.OptimizeFonts;
    this.m_options = options;
    this.InitializeCollections();
    this.Version = version;
    this.ParseStream(stream, (string) null, version, options);
  }

  public WorkbookImpl(
    IApplication application,
    object parent,
    XmlReader reader,
    OfficeXmlOpenType openType)
    : base(application, parent)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    this.InitializeCollections();
    this.Version = application.DefaultVersion;
    this.InsertDefaultFonts();
    this.InsertDefaultValues();
    this.m_bReadOnly = false;
    if (openType != OfficeXmlOpenType.MSExcel)
      throw new ArgumentOutOfRangeException("cannot specified xml open type.");
  }

  protected void InitializeCollections()
  {
    this.m_arrObjects = new WorkbookObjectsCollection(this.Application, (object) this);
    this.m_worksheets = new WorksheetsCollection(this.Application, (object) this);
    this.m_styles = new StylesCollection(this.Application, (object) this);
    this.m_colors = new List<Color>((IEnumerable<Color>) WorkbookImpl.DEF_PALETTE);
    this.m_names = new WorkbookNamesCollection(this.Application, (object) this);
    this.m_charts = new ChartsCollection(this.Application, (object) this);
    this.m_SSTDictionary = new SSTDictionary(this);
    this.m_fonts = new FontsCollection(this.Application, (object) this);
    this.m_externBooks = new ExternBookCollection(this.Application, (object) this);
    this.m_calcution = new CalculationOptionsImpl(this.Application, (object) this);
    this.m_extFormats = new ExtendedFormatsCollection(this.Application, (object) this);
    this.m_shapesData = new WorkbookShapeDataImpl(this.Application, (object) this, new WorkbookImpl.ShapesGetterMethod(this.GetWorksheetShapes));
    this.m_arrNames = new List<NameRecord>();
    this.m_rawFormats = new FormatsCollection(this.Application, (object) this);
    this.m_arrBound = new List<BoundSheetRecord>();
    this.m_arrReparse = new List<IReparse>();
    this.m_arrExtFormatRecords = new List<ExtendedFormatRecord>();
    this.m_arrXFExtRecords = new List<ExtendedXFRecord>();
    this.m_arrStyleExtRecords = new List<StyleExtRecord>();
    this.m_headerFooterPictures = new WorkbookShapeDataImpl(this.Application, (object) this, new WorkbookImpl.ShapesGetterMethod(this.GetHeaderFooterShapes));
    this.m_sheetGroup = new Syncfusion.OfficeChart.Implementation.Collections.Grouping.WorksheetGroup(this.Application, (object) this);
    this.WindowOne.SelectedTab = ushort.MaxValue;
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
        defaultXf.VAlignmentType = OfficeVAlign.VAlignBottom;
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
        defaultXf.VAlignmentType = OfficeVAlign.VAlignBottom;
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
        defaultXf.VAlignmentType = OfficeVAlign.VAlignBottom;
        defaultXf.IsNotParentFormat = true;
        defaultXf.IsNotParentAlignment = true;
        defaultXf.IsNotParentBorder = true;
        defaultXf.IsNotParentPattern = true;
        defaultXf.IsNotParentCellOptions = true;
        break;
      case 15:
        defaultXf = (ExtendedFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExtendedFormat);
        defaultXf.IsLocked = true;
        defaultXf.HAlignmentType = OfficeHAlign.HAlignGeneral;
        defaultXf.VAlignmentType = OfficeVAlign.VAlignBottom;
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
    this.Dispose();
    this.m_IsDisposed = true;
  }

  protected void ClearAll()
  {
    if (this.m_bIsDisposed)
      return;
    if (this.m_ActiveSheet != null && this.m_ActiveSheet is WorksheetImpl)
      this.m_ActiveSheet = (WorksheetBaseImpl) null;
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
      foreach (NameImpl name in (CollectionBase<IName>) this.m_names)
        name.ClearAll();
      this.m_names.Clear();
      this.m_names = (WorkbookNamesCollection) null;
    }
    if (this.m_bookExt != null)
    {
      this.m_bookExt.ClearData();
      this.m_bookExt = (BiffRecordRaw) null;
    }
    if (this.m_childElements != null)
    {
      this.m_childElements.Clear();
      this.m_childElements = (Dictionary<string, List<Stream>>) null;
    }
    if (this.m_compatibility != null)
    {
      this.m_compatibility.ClearData();
      this.m_compatibility = (CompatibilityRecord) null;
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
      WorkbookImpl.SheetTypeToName = (Dictionary<OfficeSheetType, string>) null;
    }
    if (this.m_preservesPivotCache != null)
    {
      this.m_preservesPivotCache.Clear();
      this.m_preservesPivotCache = (List<Stream>) null;
    }
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
    foreach (ExtendedFormatImpl extFormat in (CollectionBase<ExtendedFormatImpl>) this.m_extFormats)
      extFormat.Clear();
  }

  private void ParseExcel2007Stream(Stream stream, string password, bool parseOnDemand)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.m_rawFormats.InsertDefaultFormats();
    this.m_fileDataHolder.ParseDocument(ref this.m_themeColors, parseOnDemand);
  }

  private void ParseFile(
    string fileName,
    string password,
    OfficeVersion version,
    OfficeParseOptions options,
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
    OfficeVersion version,
    OfficeParseOptions options)
  {
    this.m_bIsLoaded = true;
    if (version == OfficeVersion.Excel97to2003)
      return;
    if (version == OfficeVersion.Excel97to2003)
      throw new ArgumentOutOfRangeException(nameof (version));
    this.ParseExcel2007Stream(stream, password, options == OfficeParseOptions.ParseWorksheetsOnDemand);
    this.Activate();
  }

  ~WorkbookImpl() => this.Close();

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

  private void NormalizeBorders(ExtendedFormatRecord xf)
  {
    if (xf.XFType != ExtendedFormatRecord.TXFType.XF_STYLE || xf.IsNotParentBorder || (int) xf.ParentIndex == this.MaxXFCount)
      return;
    ExtendedFormatRecord arrExtFormatRecord = this.m_arrExtFormatRecords[(int) xf.ParentIndex];
    if (xf.BorderBottom == arrExtFormatRecord.BorderBottom && xf.BorderLeft == arrExtFormatRecord.BorderLeft && xf.BorderRight == arrExtFormatRecord.BorderRight && xf.BorderTop == arrExtFormatRecord.BorderTop)
      return;
    xf.IsNotParentBorder = true;
  }

  private void ParseSSTRecord(SSTRecord sst, OfficeParseOptions options)
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
    if (this.m_bWorkbookOpening)
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
          arrStyle.StyleName = CollectionBaseEx<WorksheetImpl>.GenerateDefaultName((ICollection) arrStyles, "UNKNOWNSTYLE_");
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
    GC.Collect(GC.MaxGeneration);
    GC.WaitForPendingFinalizers();
    if (this.AppImplementation != null && this.AppImplementation.Workbooks != null)
      (this.AppImplementation.Workbooks as WorkbooksCollection).Remove((object) this);
    GC.SuppressFinalize((object) this);
  }

  public void Close(bool saveChanges) => this.Close(saveChanges, (string) null);

  public void Close() => this.Close(false);

  public void Save()
  {
    if (this.m_strFullName == null || this.m_strFullName.Length == 0)
      throw new ApplicationException("Workbook was not created from file. That is why woorkbook file not specified. You must use SaveAs method instead.");
    this.SaveAs(this.m_strFullName, this.m_fileDataHolder != null ? this.m_fileDataHolder.GetWorkbookPartType() : OfficeSaveType.SaveAsXLS);
  }

  public void SaveAs(string FileName)
  {
    this.SaveAs(FileName, OfficeSaveType.SaveAsXLS, this.Version);
  }

  public void SaveAs(string FileName, OfficeSaveType saveType)
  {
    this.SaveAs(FileName, saveType, this.Version);
  }

  public void SaveAs(string FileName, OfficeSaveType saveType, OfficeVersion version)
  {
    switch (FileName)
    {
      case null:
        throw new ArgumentNullException("Filename");
      case "":
        throw new ArgumentException("FileName cannot be empty.");
      default:
        string fullPath = Path.GetFullPath(FileName);
        string directoryName = Path.GetDirectoryName(fullPath);
        this.m_bSaving = true;
        if (File.Exists(fullPath))
        {
          if ((File.GetAttributes(fullPath) & FileAttributes.ReadOnly) != (FileAttributes) 0)
            this.RaiseReadOnlyFileEvent(fullPath);
          if (this.Application.DeleteDestinationFile && fullPath != this.m_strFullName)
            File.Delete(fullPath);
        }
        if (directoryName.Length != 0 && directoryName != null && directoryName.Length > 0 && !Directory.Exists(directoryName))
          Directory.CreateDirectory(directoryName);
        if (this.Styles.Count > 0)
        {
          FontImpl wrapped = (this.Styles["Normal"].Font as FontWrapper).Wrapped;
          this.m_iFirstCharSize = (int) Math.Round((double) wrapped.MeasureString('0'.ToString()).Width);
          this.m_iSecondCharSize = (int) Math.Round((double) wrapped.MeasureCharacter('0').Width);
        }
        IdReserver shapeIds = this.PrepareShapes(new WorkbookImpl.ShapesGetterMethod(this.GetWorksheetShapes));
        this.PrepareShapes(new WorkbookImpl.ShapesGetterMethod(this.GetHeaderFooterShapes));
        this.CreateSerializator(version, shapeIds);
        this.m_strFullName = fullPath;
        this.m_bSaving = false;
        this.m_bSaved = true;
        this.RaiseSavedEvent();
        break;
    }
  }

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
    int num2 = shapeIdReserver.Allocate(num1 + 1, shapes.CollectionIndex);
    int num3 = num2 + shapes.Count;
    shapes.StartId = num2;
    shapes.LastId = num3;
    int num4 = num2 + 1;
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
            else if (shapeId <= 0 && enumerateShape[index].ShapeType != OfficeShapeType.Unknown)
              bChanged = true;
          }
        }
        if (enumerateShape != null && enumerateShape.Count > 0)
        {
          WorksheetImpl worksheet = enumerateShape.Worksheet;
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
      tabSheet = this.m_arrObjects[i] as ITabSheet;
      ShapeCollectionBase shapes = shapesGetter(tabSheet);
      if (shapes != null && shapes.Count > 0)
        yield return shapes;
      shapes = tabSheet.Shapes as ShapeCollectionBase;
      int j = 0;
      for (int lenJ = shapes.Count; j < lenJ; ++j)
      {
        if (shapes[j] is ITabSheet tabSheet)
        {
          ShapeCollectionBase shapesResult = shapesGetter(tabSheet);
          if (shapesResult != null && shapesResult.Count > 0)
            yield return shapesResult;
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

  public void SaveAs(string fileName, string separator)
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
        activeSheet.SaveAs(fileName, separator);
        break;
    }
  }

  public void SaveAs(Stream stream, string separator)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (separator == null || separator.Length == 0)
      throw new ArgumentNullException(nameof (separator));
    this.SaveAsInternal(stream, separator);
  }

  private void SaveAsInternal(Stream stream, string separator)
  {
    if (!(this.ActiveSheet is WorksheetImpl activeSheet))
      return;
    activeSheet.SaveAs(stream, separator);
  }

  public void SaveAsXmlInternal(XmlWriter writer, OfficeXmlSaveType saveType)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    this.m_bSaving = true;
    XmlSerializatorFactory.GetSerializator(saveType).Serialize(writer, (IWorkbook) this);
    this.m_bSaving = false;
  }

  public void SaveAsXml(XmlWriter writer, OfficeXmlSaveType saveType)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    this.SaveAsXmlInternal(writer, saveType);
  }

  public void SaveAsXml(string strFileName, OfficeXmlSaveType saveType)
  {
    switch (strFileName)
    {
      case null:
        throw new ArgumentNullException(nameof (strFileName));
      case "":
        throw new ArgumentException("strFileName - string cannot be empty.");
      default:
        this.m_bSaving = true;
        Encoding encoding = (Encoding) new UTF8Encoding(false);
        XmlTextWriter writer = new XmlTextWriter(strFileName, encoding);
        writer.Formatting = Formatting.Indented;
        this.SaveAsXml((XmlWriter) writer, saveType);
        writer.Close();
        this.m_bSaving = false;
        break;
    }
  }

  public void SaveAsXml(Stream stream, OfficeXmlSaveType saveType)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.m_bSaving = true;
    Encoding encoding = (Encoding) new UTF8Encoding(false);
    XmlTextWriter writer = new XmlTextWriter(stream, encoding);
    writer.Formatting = Formatting.Indented;
    this.SaveAsXml((XmlWriter) writer, saveType);
    writer.Flush();
    this.m_bSaving = false;
  }

  private string GetContentTypeString(OfficeHttpContentType contentType)
  {
    switch (contentType)
    {
      case OfficeHttpContentType.Excel97:
        return "Application/x-msexcel";
      case OfficeHttpContentType.Excel2000:
        return "Application/vnd.ms-excel";
      case OfficeHttpContentType.Excel2007:
      case OfficeHttpContentType.Excel2010:
      case OfficeHttpContentType.Excel2013:
        return "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
      case OfficeHttpContentType.CSV:
        return "text/csv";
      default:
        throw new ArgumentOutOfRangeException(nameof (contentType));
    }
  }

  public void SaveAs(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.SaveAs(stream, OfficeSaveType.SaveAsXLS);
  }

  public void SaveAs(Stream stream, OfficeSaveType saveType)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.SaveAsInternal(stream, saveType);
  }

  private void SaveAsInternal(Stream stream, OfficeSaveType saveType)
  {
    this.m_bSaving = true;
    this.CreateSerializator(this.m_version, this.PrepareShapes(new WorkbookImpl.ShapesGetterMethod(this.GetWorksheetShapes)));
    stream.Flush();
    this.m_bSaving = false;
    this.m_bSaved = true;
    this.RaiseSavedEvent();
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
    foreach (KeyValuePair<OfficeSheetType, int> hashHeadingPair in this.GetHashHeadingPairs())
    {
      OfficeSheetType key = hashHeadingPair.Key;
      int num = hashHeadingPair.Value;
      if (WorkbookImpl.SheetTypeToName.ContainsKey(key))
      {
        objectList.Add((object) WorkbookImpl.SheetTypeToName[key]);
        objectList.Add((object) num);
      }
    }
    return objectList.ToArray();
  }

  private Dictionary<OfficeSheetType, int> GetHashHeadingPairs()
  {
    Dictionary<OfficeSheetType, int> hashHeadingPairs = new Dictionary<OfficeSheetType, int>();
    IWorksheets worksheets = this.Worksheets;
    int Index = 0;
    for (int count = worksheets.Count; Index < count; ++Index)
    {
      OfficeSheetType type = (worksheets[Index] as WorksheetImpl).Type;
      if (hashHeadingPairs.ContainsKey(type))
        ++hashHeadingPairs[type];
      else
        hashHeadingPairs.Add(type, 1);
    }
    return hashHeadingPairs;
  }

  public void SetPaletteColor(int index, Color color)
  {
    if (!this.m_bWorkbookOpening && index < 8 || index >= this.m_colors.Count)
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

  public Color GetPaletteColor(OfficeKnownColors color)
  {
    int num = (int) color;
    return num < 77 || num > 79 ? (num != 80 /*0x50*/ ? (num != (int) short.MaxValue || this.m_colors.Count <= 0 ? this.m_colors[num % this.m_colors.Count] : this.m_colors[0]) : ShapeFillImpl.DEF_COMENT_PARSE_COLOR) : WorkbookImpl.m_chartColors[num - 77];
  }

  public OfficeKnownColors GetNearestColor(Color color) => this.GetNearestColor(color, 0);

  public OfficeKnownColors GetNearestColor(Color color, int iStartIndex)
  {
    if (iStartIndex < 0 || iStartIndex > this.m_colors.Count)
      throw new ArgumentOutOfRangeException(nameof (iStartIndex));
    int nearestColor = iStartIndex;
    double num1 = this.ColorDistance(this.m_colors[iStartIndex], color);
    for (int index = iStartIndex + 1; index < this.m_colors.Count; ++index)
    {
      double num2 = this.ColorDistance(this.m_colors[index], color);
      if (num2 < num1)
      {
        num1 = num2;
        nearestColor = index;
        if (num2 == 0.0)
          break;
      }
    }
    return (OfficeKnownColors) nearestColor;
  }

  public OfficeKnownColors GetNearestColor(int r, int g, int b)
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
    return (OfficeKnownColors) nearestColor;
  }

  public OfficeKnownColors SetColorOrGetNearest(Color color)
  {
    OfficeKnownColors nearestColor = this.GetNearestColor(color);
    if ((this.isEqualColor = this.CompareColors(this.m_colors[(int) nearestColor], color)) || this.m_iFirstUnusedColor >= this.m_colors.Count)
      return nearestColor;
    this.SetPaletteColor(this.m_iFirstUnusedColor, color);
    OfficeKnownColors firstUnusedColor = (OfficeKnownColors) this.m_iFirstUnusedColor;
    ++this.m_iFirstUnusedColor;
    return firstUnusedColor;
  }

  public OfficeKnownColors SetColorOrGetNearest(int r, int g, int b)
  {
    return this.SetColorOrGetNearest(Color.FromArgb((int) byte.MaxValue, (int) (byte) r, (int) (byte) g, (int) (byte) b));
  }

  public void Replace(string oldValue, string newValue)
  {
    int Index = 0;
    for (int count = this.m_worksheets.Count; Index < count; ++Index)
      this.m_worksheets[Index].Replace(oldValue, newValue);
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

  public IOfficeFont CreateFont()
  {
    return (IOfficeFont) new FontWrapper(this.AppImplementation.CreateFont((object) this), false, false);
  }

  public IOfficeFont CreateFont(Font nativeFont)
  {
    return (IOfficeFont) new FontWrapper(this.AppImplementation.CreateFont((object) this, nativeFont), false, false);
  }

  public IOfficeFont AddFont(IOfficeFont fontToAdd)
  {
    bool flag = fontToAdd is FontWrapper;
    fontWrapper = (FontWrapper) null;
    FontImpl font;
    if (flag)
      font = fontToAdd is FontWrapper fontWrapper ? fontWrapper.Wrapped : throw new ArgumentNullException(nameof (fontToAdd));
    else
      font = fontToAdd as FontImpl;
    FontImpl fontImpl = this.m_fonts.Add((IOfficeFont) font) as FontImpl;
    if (!flag)
      return (IOfficeFont) fontImpl;
    fontWrapper.Wrapped = fontImpl;
    fontWrapper.IsReadOnly = true;
    return (IOfficeFont) fontWrapper;
  }

  public IOfficeFont CreateFont(IOfficeFont baseFont) => this.CreateFont(baseFont, true);

  public IOfficeFont CreateFont(IOfficeFont baseFont, bool bAddToCollection)
  {
    IOfficeFont font = baseFont != null ? (IOfficeFont) this.AppImplementation.CreateFont(baseFont) : (IOfficeFont) this.AppImplementation.CreateFont((object) this);
    if (baseFont != null && baseFont is FontImpl)
    {
      (font as FontImpl).ParaAlign = (baseFont as FontImpl).ParaAlign;
      font.RGBColor = baseFont.RGBColor;
    }
    if (bAddToCollection)
    {
      ((FontImpl) font).Index = this.m_fonts.Count;
      this.m_fonts.Add(font);
    }
    return font;
  }

  public IRange FindFirst(string findValue, OfficeFindType flags)
  {
    return this.FindFirst(findValue, flags, OfficeFindOptions.None);
  }

  public IRange FindStringStartsWith(string findValue, OfficeFindType flags)
  {
    return this.FindStringStartsWith(findValue, flags, false);
  }

  public IRange FindStringStartsWith(string findValue, OfficeFindType flags, bool ignoreCase)
  {
    this.m_isStartsOrEndsWith = new bool?(true);
    OfficeFindOptions findOptions = ignoreCase ? OfficeFindOptions.None : OfficeFindOptions.MatchCase;
    return this.FindFirst(findValue, flags, findOptions);
  }

  public IRange FindStringEndsWith(string findValue, OfficeFindType flags)
  {
    return this.FindStringEndsWith(findValue, flags, false);
  }

  public IRange FindStringEndsWith(string findValue, OfficeFindType flags, bool ignoreCase)
  {
    this.m_isStartsOrEndsWith = new bool?(false);
    OfficeFindOptions findOptions = ignoreCase ? OfficeFindOptions.None : OfficeFindOptions.MatchCase;
    return this.FindFirst(findValue, flags, findOptions);
  }

  public IRange FindFirst(string findValue, OfficeFindType flags, OfficeFindOptions findOptions)
  {
    if (findValue == null)
      return (IRange) null;
    bool flag1 = (flags & OfficeFindType.Formula) == OfficeFindType.Formula;
    bool flag2 = (flags & OfficeFindType.Text) == OfficeFindType.Text;
    bool flag3 = (flags & OfficeFindType.FormulaStringValue) == OfficeFindType.FormulaStringValue;
    bool flag4 = (flags & OfficeFindType.Error) == OfficeFindType.Error;
    if (!flag1 && !flag2 && !flag3 && !flag4)
      throw new ArgumentException("Parameter flags is not valid.", nameof (flags));
    return this.Worksheets.FindFirst(findValue, flags, findOptions);
  }

  public IRange FindFirst(double findValue, OfficeFindType flags)
  {
    bool flag1 = (flags & OfficeFindType.FormulaValue) == OfficeFindType.FormulaValue;
    bool flag2 = (flags & OfficeFindType.Number) == OfficeFindType.Number;
    if (!flag1 && !flag2)
      throw new ArgumentException("Parameter flags is not valid.", nameof (flags));
    return this.Worksheets.FindFirst(findValue, flags);
  }

  public IRange FindFirst(bool findValue) => this.Worksheets.FindFirst(findValue);

  public IRange FindFirst(DateTime findValue) => this.Worksheets.FindFirst(findValue);

  public IRange FindFirst(TimeSpan findValue) => this.Worksheets.FindFirst(findValue);

  public IRange[] FindAll(string findValue, OfficeFindType flags)
  {
    return this.FindAll(findValue, flags, OfficeFindOptions.None);
  }

  public IRange[] FindAll(string findValue, OfficeFindType flags, OfficeFindOptions findOptions)
  {
    if (findValue == null)
      return (IRange[]) null;
    bool flag1 = (flags & OfficeFindType.Formula) == OfficeFindType.Formula;
    bool flag2 = (flags & OfficeFindType.Text) == OfficeFindType.Text;
    bool flag3 = (flags & OfficeFindType.FormulaStringValue) == OfficeFindType.FormulaStringValue;
    bool flag4 = (flags & OfficeFindType.Error) == OfficeFindType.Error;
    if (!flag1 && !flag2 && !flag3 && !flag4)
      throw new ArgumentException("Parameter flags is not valid.", nameof (flags));
    return this.Worksheets.FindAll(findValue, flags, findOptions);
  }

  public IRange[] FindAll(double findValue, OfficeFindType flags)
  {
    bool flag1 = (flags & OfficeFindType.FormulaValue) == OfficeFindType.FormulaValue;
    bool flag2 = (flags & OfficeFindType.Number) == OfficeFindType.Number;
    if (!flag1 && !flag2)
      throw new ArgumentException("Parameter flags is not valid.", nameof (flags));
    return this.Worksheets.FindAll(findValue, flags);
  }

  public IRange[] FindAll(bool findValue) => this.Worksheets.FindAll(findValue);

  public IRange[] FindAll(DateTime findValue) => this.Worksheets.FindAll(findValue);

  public IRange[] FindAll(TimeSpan findValue) => this.Worksheets.FindAll(findValue);

  public void SetSeparators(char argumentsSeparator, char arrayRowsSeparator)
  {
    this.FormulaUtil.SetSeparators(argumentsSeparator, arrayRowsSeparator);
  }

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
    if (password == null)
      throw new ArgumentNullException(nameof (password));
  }

  public IWorkbook Clone()
  {
    WorkbookImpl workbookImpl = (WorkbookImpl) this.MemberwiseClone();
    if (this.m_fileDataHolder != null)
      workbookImpl.m_fileDataHolder = this.m_fileDataHolder.Clone(workbookImpl);
    workbookImpl.m_ptrHeapHandle = IntPtr.Zero;
    workbookImpl.m_fonts = this.m_fonts.Clone(workbookImpl);
    workbookImpl.m_rawFormats = this.m_rawFormats.Clone((object) workbookImpl);
    workbookImpl.m_extFormats = (ExtendedFormatsCollection) this.m_extFormats.Clone((object) workbookImpl);
    workbookImpl.m_styles = (StylesCollection) this.m_styles.Clone((object) workbookImpl);
    workbookImpl.m_colors = this.ClonePalette();
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
    workbookImpl.m_worksheets = new WorksheetsCollection(this.Application, (object) workbookImpl);
    workbookImpl.m_externSheet = (ExternSheetRecord) CloneUtils.CloneCloneable((ICloneable) this.m_externSheet);
    workbookImpl.m_externBooks = (ExternBookCollection) this.m_externBooks.Clone((object) workbookImpl);
    workbookImpl.m_arrObjects = (WorkbookObjectsCollection) this.m_arrObjects.Clone((object) workbookImpl);
    workbookImpl.m_SSTDictionary = (SSTDictionary) this.m_SSTDictionary.Clone(workbookImpl);
    workbookImpl.m_calcution = (CalculationOptionsImpl) this.m_calcution.Clone((object) workbookImpl);
    workbookImpl.m_names = (WorkbookNamesCollection) this.m_names.Clone((object) workbookImpl);
    workbookImpl.m_shapesData = (WorkbookShapeDataImpl) this.m_shapesData.Clone((object) workbookImpl);
    workbookImpl.m_headerFooterPictures = (WorkbookShapeDataImpl) this.m_headerFooterPictures.Clone((object) workbookImpl);
    workbookImpl.m_sheetGroup = (Syncfusion.OfficeChart.Implementation.Collections.Grouping.WorksheetGroup) this.m_sheetGroup.Clone((object) workbookImpl);
    workbookImpl.m_externSheet = (ExternSheetRecord) CloneUtils.CloneCloneable((ICloneable) this.m_externSheet);
    workbookImpl.m_windowOne = (WindowOneRecord) CloneUtils.CloneCloneable((ICloneable) this.m_windowOne);
    workbookImpl.m_password = (PasswordRecord) CloneUtils.CloneCloneable((ICloneable) this.m_password);
    workbookImpl.m_passwordRev4 = (PasswordRev4Record) CloneUtils.CloneCloneable((ICloneable) this.m_passwordRev4);
    workbookImpl.m_protectionRev4 = (ProtectionRev4Record) CloneUtils.CloneCloneable((ICloneable) this.m_protectionRev4);
    workbookImpl.m_fileSharing = (FileSharingRecord) CloneUtils.CloneCloneable((ICloneable) this.m_fileSharing);
    workbookImpl.m_arrNames = CloneUtils.CloneCloneable<NameRecord>(this.m_arrNames);
    workbookImpl.m_arrBound = CloneUtils.CloneCloneable<BoundSheetRecord>(this.m_arrBound);
    workbookImpl.m_arrReparse = new List<IReparse>();
    workbookImpl.m_arrExtFormatRecords = CloneUtils.CloneCloneable<ExtendedFormatRecord>(this.m_arrExtFormatRecords);
    workbookImpl.m_arrXFExtRecords = CloneUtils.CloneCloneable<ExtendedXFRecord>(this.m_arrXFExtRecords);
    workbookImpl.m_ActiveSheet = (WorksheetBaseImpl) null;
    workbookImpl.ActiveSheetIndex = this.ActiveSheetIndex;
    workbookImpl.m_formulaUtil = (FormulaUtil) null;
    return (IWorkbook) workbookImpl;
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
    }
  }

  private List<Color> ClonePalette() => new List<Color>((IEnumerable<Color>) this.m_colors);

  private void SaveInExcel2007(Stream stream, OfficeSaveType saveType)
  {
    if (this.m_fileDataHolder != null)
      return;
    this.m_fileDataHolder = new FileDataHolder(this);
  }

  private IWorkbookSerializator CreateSerializator(OfficeVersion version, IdReserver shapeIds)
  {
    this.CheckLicensingSheet();
    this.AddLicenseWorksheet();
    if (this.m_externSheet.RefCount > (ushort) 1370)
      this.OptimizeReferences();
    switch (version)
    {
      case OfficeVersion.Excel97to2003:
        return (IWorkbookSerializator) null;
      case OfficeVersion.Excel2007:
      case OfficeVersion.Excel2010:
      case OfficeVersion.Excel2013:
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
      if (format.FillPattern == OfficePattern.Gradient)
      {
        format.FillPattern = OfficePattern.Solid;
        OfficeKnownColors indexed = format.Gradient.BackColorObject.GetIndexed((IWorkbook) this);
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
      FontImpl fontImpl = (FontImpl) fontsCollection.Add((IOfficeFont) font);
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

  [CLSCompliant(false)]
  protected internal void SerializeForClipboard(OffsetArrayList records, WorksheetImpl sheet)
  {
  }

  public void SetActiveWorksheet(WorksheetBaseImpl sheet)
  {
    this.m_ActiveSheet = sheet;
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
    int key = 0;
    for (int refCount = (int) externSheet.RefCount; key < refCount; ++key)
    {
      ExternSheetRecord.TREF tref = externSheet.Refs[key];
      int num1 = (int) tref.SupBookIndex;
      if (hashSubBooks.ContainsKey(num1))
        num1 = hashSubBooks[num1];
      int num2 = externSheet1.AddReference(num1, (int) tref.FirstSheet, (int) tref.LastSheet);
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
    FontImpl font = ((IInternalFont) this.m_styles["Normal"].Font).Font;
    double maxDigitHeight = 0.0;
    for (char c = '0'; c <= '9'; ++c)
    {
      SizeF sizeF = font.MeasureString(new string(c, 1));
      if ((double) sizeF.Height > maxDigitHeight)
        maxDigitHeight = (double) sizeF.Height;
    }
    return maxDigitHeight;
  }

  private int GetActualValue(Font font) => 0;

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
    new StringFormat(StringFormat.GenericTypographic)
    {
      Alignment = StringAlignment.Near
    }.SetMeasurableCharacterRanges(new CharacterRange[1]
    {
      new CharacterRange(1, 1)
    });
    RectangleF rectangleF = new RectangleF(0.0f, 0.0f, 1000f, 1000f);
    double num = 0.0;
    if (!this.HasStandardFont)
      return num;
    return (double) font.Size > 10.0 ? ((double) font.Size % 2.0 != 0.0 ? num - 1.0 : num) : ((double) font.Size >= 10.0 ? num - 2.0 : num);
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

  internal void ExtractControlProperties()
  {
    throw new Exception("The method or operation is not implemented.");
  }

  internal string[] DateTimePatterns
  {
    get
    {
      if (this.m_DateTimePatterns != null)
        return this.m_DateTimePatterns;
      string[] dateTimePatterns = CultureInfo.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns();
      this.m_DateTimePatterns = new string[dateTimePatterns.Length + this.m_customPatterns.Length];
      this.m_customPatterns.CopyTo((Array) this.m_DateTimePatterns, 0);
      dateTimePatterns.CopyTo((Array) this.m_DateTimePatterns, this.m_customPatterns.Length);
      return this.m_DateTimePatterns;
    }
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

  internal ExtendedFormatImpl AddExtendedProperties(ExtendedFormatImpl m_xFormat)
  {
    if (m_xFormat.Index != this.DefaultXFIndex)
    {
      if (m_xFormat.Properties.Count > 0)
        m_xFormat.Properties.Clear();
      if (m_xFormat.FillPattern == OfficePattern.Solid)
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
      if ((m_xFormat.TopBorderColor.ColorType == ColorType.RGB || m_xFormat.TopBorderColor.ColorType == ColorType.Theme) && m_xFormat.TopBorderLineStyle != OfficeLineStyle.None)
        this.AddExtendedProperty(CellPropertyExtensionType.TopBorderColor, m_xFormat.Borders[OfficeBordersIndex.EdgeTop].ColorRGB, m_xFormat);
      if ((m_xFormat.BottomBorderColor.ColorType == ColorType.RGB || m_xFormat.BottomBorderColor.ColorType == ColorType.Theme) && m_xFormat.BottomBorderLineStyle != OfficeLineStyle.None)
        this.AddExtendedProperty(CellPropertyExtensionType.BottomBorderColor, m_xFormat.Borders[OfficeBordersIndex.EdgeBottom].ColorRGB, m_xFormat);
      if ((m_xFormat.LeftBorderColor.ColorType == ColorType.RGB || m_xFormat.LeftBorderColor.ColorType == ColorType.Theme) && m_xFormat.LeftBorderLineStyle != OfficeLineStyle.None)
        this.AddExtendedProperty(CellPropertyExtensionType.LeftBorderColor, m_xFormat.Borders[OfficeBordersIndex.EdgeLeft].ColorRGB, m_xFormat);
      if ((m_xFormat.RightBorderColor.ColorType == ColorType.RGB || m_xFormat.RightBorderColor.ColorType == ColorType.Theme) && m_xFormat.RightBorderLineStyle != OfficeLineStyle.None)
        this.AddExtendedProperty(CellPropertyExtensionType.RightBorderColor, m_xFormat.Borders[OfficeBordersIndex.EdgeRight].ColorRGB, m_xFormat);
      if ((m_xFormat.DiagonalBorderColor.ColorType == ColorType.RGB || m_xFormat.DiagonalBorderColor.ColorType == ColorType.Theme) && m_xFormat.DiagonalDownBorderLineStyle != OfficeLineStyle.None)
        this.AddExtendedProperty(CellPropertyExtensionType.DiagonalCellBorder, m_xFormat.Borders[OfficeBordersIndex.DiagonalDown].ColorRGB, m_xFormat);
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

  public delegate ShapeCollectionBase ShapesGetterMethod(ITabSheet sheet);

  public delegate void DigitSizeCallback(RectangleF rect, ref double currentMax);
}
