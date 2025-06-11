// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.FormulaUtil
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

[Preserve(AllMembers = true)]
internal class FormulaUtil : CommonObject
{
  internal const int DEF_TYPE_REF = 0;
  internal const int DEF_TYPE_VALUE = 1;
  private const int DEF_TYPE_ARRAY = 2;
  private const int DEF_INDEX_DEFAULT = 0;
  private const int DEF_INDEX_ARRAY = 1;
  private const int DEF_INDEX_NAME = 2;
  private const int DEF_INDEX_ROOT_LEVEL = 3;
  private const RegexOptions DEF_REGEX = RegexOptions.Compiled;
  public const int DEF_NAME_INDEX = 1;
  public const int DEF_REFERENCE_INDEX = 2;
  public const int DEF_ARRAY_INDEX = 2;
  internal const int DEF_OPTIONS_OPT_GOTO = 8;
  internal const int DEF_OPTIONS_NOT_OPT_GOTO = 0;
  private const char DEF_BOOKNAME_OPENBRACKET = '[';
  private const char DEF_BOOKNAME_CLOSEBRACKET = ']';
  public const string DEF_GROUP_COLUMN1 = "Column1";
  public const string DEF_GROUP_COLUMN2 = "Column2";
  public const string DEF_GROUP_ROW1 = "Row1";
  public const string DEF_GROUP_ROW2 = "Row2";
  private const char DEF_SHEET_NAME_DELIM = '\'';
  public const string Excel2010FunctionPrefix = "_xlfn.";
  public const string DEF_SHEETNAME_GROUP = "SheetName";
  public const string DEF_BOOKNAME_GROUP = "BookName";
  public const string DEF_RANGENAME_GROUP = "RangeName";
  public const string DEF_ROW_GROUP = "Row1";
  public const string DEF_COLUMN_GROUP = "Column1";
  public const string DEF_PATH_GROUP = "Path";
  private const string DEF_SHEET_NAME = "SheetName";
  private const string DEF_SHEET_NAME_REG_EXPR = "[^][:\\/?]*";
  private static readonly int[][][] DEF_INDEXES_CONVERTION = new int[4][][]
  {
    new int[3][]
    {
      new int[3]{ 1, 1, 1 },
      new int[3]{ 2, 3, 3 },
      new int[3]{ 3, 3, 3 }
    },
    new int[3][]
    {
      new int[3]{ 2, 2, 3 },
      new int[3]{ 2, 2, 3 },
      new int[3]{ 2, 2, 3 }
    },
    new int[3][]
    {
      new int[3]{ 3, 3, 3 },
      new int[3]{ 3, 3, 3 },
      new int[3]{ 3, 3, 3 }
    },
    new int[3][]
    {
      new int[3]{ 2, 2, 1 },
      new int[3]{ 2, 2, 3 },
      new int[3]{ 2, 2, 3 }
    }
  };
  public static readonly char[] OpenBrackets = new char[5]
  {
    '{',
    '(',
    '"',
    '\'',
    '['
  };
  public static readonly char[] CloseBrackets = new char[5]
  {
    '}',
    ')',
    '"',
    '\'',
    ']'
  };
  public static readonly char[] StringBrackets = new char[1]
  {
    '"'
  };
  public static readonly string[] UnaryOperations = new string[4]
  {
    "%",
    "(",
    "+",
    "-"
  };
  public static readonly string[] PlusMinusArray = new string[2]
  {
    "+",
    "-"
  };
  private static readonly SortedList m_listPlusMinus = FormulaUtil.GetSortedList(FormulaUtil.PlusMinusArray);
  public static readonly Dictionary<ExcelFunction, string> FunctionIdToAlias = new Dictionary<ExcelFunction, string>(407);
  public static readonly Dictionary<ExcelFunction, int> FunctionIdToParamCount = new Dictionary<ExcelFunction, int>(407);
  public static readonly Dictionary<string, ExcelFunction> FunctionAliasToId = new Dictionary<string, ExcelFunction>();
  public static readonly Dictionary<ExcelFunction, Dictionary<Type, ReferenceIndexAttribute>> FunctionIdToIndex = new Dictionary<ExcelFunction, Dictionary<Type, ReferenceIndexAttribute>>(407);
  public static readonly Dictionary<string, ConstructorInfo> ErrorNameToConstructor = new Dictionary<string, ConstructorInfo>(7);
  private static readonly Dictionary<int, string> s_hashErrorCodeToName = new Dictionary<int, string>(7);
  private static readonly Dictionary<string, int> s_hashNameToErrorCode = new Dictionary<string, int>(7);
  private static readonly Dictionary<FormulaToken, FormulaUtil.TokenConstructor> TokenCodeToConstructor = new Dictionary<FormulaToken, FormulaUtil.TokenConstructor>(25);
  private static readonly Dictionary<FormulaToken, Ptg> s_hashTokenCodeToPtg = new Dictionary<FormulaToken, Ptg>();
  public static readonly Regex CellRegex = new Regex("(?<Column1>[\\$]?[A-Za-z]{1,3})(?<Row1>[\\$]?\\d+)", RegexOptions.Compiled);
  public static readonly Regex CellR1C1Regex = new Regex("(?<Row1>R[\\[]?[\\-]?[0-9]*[\\]]?)(?<Column1>C[\\[]?[\\-]?[0-9]*[\\]]?)", RegexOptions.Compiled);
  public static readonly Regex CellRangeRegex = new Regex("(?<Column1>[\\$]?[A-Za-z]{1,3})(?<Row1>[\\$]?\\d+):(?<Column2>[\\$]?[A-Za-z]{1,3})(?<Row2>[\\$]?\\d+)", RegexOptions.Compiled);
  public static readonly Regex FullRowRangeRegex = new Regex("(?<Row1>[\\$]?\\d+):(?<Row2>[\\$]?\\d+)", RegexOptions.Compiled);
  public static readonly Regex FullColumnRangeRegex = new Regex("(?<Column1>[\\$]?[A-Za-z]{1,3}):(?<Column2>[\\$]?[A-Za-z]{1,3})", RegexOptions.Compiled);
  public static readonly Regex FullRowRangeR1C1Regex = new Regex("(?<Row1>R[\\[]?[\\-]?[0-9]*[\\]]?):(?<Row2>R[\\[]?[\\-]?[0-9]*[\\]]?)", RegexOptions.Compiled);
  public static readonly Regex FullColumnRangeR1C1Regex = new Regex("(?<Column1>C[\\[]?[\\-]?[0-9]*[\\]]?):(?<Column2>C[\\[]?[\\-]?[0-9]*[\\]]?)", RegexOptions.Compiled);
  public static readonly Regex Full3DRowRangeRegex = new Regex("(?<SheetName>[^][:\\/?]*)[\\!](?<Row1>[\\$]\\d+):(?<Row2>[\\$]\\d+)", RegexOptions.Compiled);
  public static readonly Regex Full3DColumnRangeRegex = new Regex("(?<SheetName>[^][:\\/?]*)[\\!](?<Column1>[\\$][A-Za-z]{1,3}):(?<Column2>[\\$][A-Za-z]{1,3})", RegexOptions.Compiled);
  public static readonly Regex CellRangeR1C1Regex = new Regex("(?<Row1>R[\\[]?[\\-]?[0-9]*[\\]]?)(?<Column1>C[\\[]?[\\-]?[0-9]*[\\]]?):(?<Row2>R[\\[]?[\\-]?[0-9]*[\\]]?)(?<Column2>C[\\[]?[\\-]?[0-9]*[\\]]?)", RegexOptions.Compiled);
  public static readonly Regex CellRangeR1C1ShortRegex = new Regex("[R|C][\\[]?[\\-]?[\\-0-9]*[\\]]?", RegexOptions.Compiled);
  public static readonly Regex CellRangeR1C13DShortRegex = new Regex("(?<SheetName>[^][:\\/?]*)[\\!][R|C][\\[]?[\\-]?[0-9]*[\\]]?", RegexOptions.Compiled);
  public static readonly Regex Cell3DRegex = new Regex("(?<SheetName>[^][:\\/?]*)[\\!](?<Column1>[\\$]?[A-Za-z]{1,3})(?<Row1>[\\$]?\\d+)", RegexOptions.Compiled);
  public static readonly Regex CellR1C13DRegex = new Regex("(?<SheetName>[^][:\\/?]*)[\\!](?<Row1>R[\\[]?[\\-]?[0-9]*[\\]]?)(?<Column1>C[\\[]?[\\-]?[0-9]*[\\]]?)", RegexOptions.Compiled);
  public static readonly Regex CellRange3DRegex = new Regex("(?<SheetName>[^][:\\/?]*)[\\!](?<Column1>[\\$]?[A-Za-z]{1,3})(?<Row1>[\\$]?\\d+):(?<Column2>[\\$]?[A-Za-z]{1,3})(?<Row2>[\\$]?\\d+)", RegexOptions.Compiled);
  public static readonly Regex CellRange3DRegex2 = new Regex("(?<SheetName>[^][:\\/?]*)[\\!](?<Column1>[\\$]?[A-Za-z]{1,3})(?<Row1>[\\$]?\\d+):(?<SheetName2>[^][:\\/?]*)[\\!](?<Column2>[\\$]?[A-Za-z]{1,3})(?<Row2>[\\$]?\\d+)", RegexOptions.Compiled);
  public static readonly Regex CellRangeR1C13DRegex = new Regex("(?<SheetName>[^][:\\/?]*)[\\!](?<Row1>[R]?[\\[]?[\\-]?[0-9]*[\\]]?)(?<Column1>[C]?[\\[]?[\\-]?[0-9]*[\\]]?):(?<Row2>[R]?[\\[]?[\\-]?[0-9]*[\\]]?)(?<Column2>[C]?[\\[]?[\\-]?[0-9]*[\\]]?)", RegexOptions.Compiled);
  public static readonly Regex CellRangeR1C13DRegex2 = new Regex("(?<SheetName>[^][:\\/?]*)[\\!](?<Row1>[R]?[\\[]?[\\-]?[0-9]*[\\]]?)(?<Column1>[C]?[\\[]?[\\-]?[0-9]*[\\]]?):(?<SheetName2>[^][:\\/?]*)[\\!](?<Row2>[R]?[\\[]?[\\-]?[0-9]*[\\]]?)(?<Column2>[C]?[\\[]?[\\-]?[0-9]*[\\]]?)", RegexOptions.Compiled);
  private static readonly Regex AddInFunctionRegEx = new Regex("('?)(?<Path>[^'][^\\[]+\\\\)?(?<BookName>[^\\]]+\\])?(?<SheetName>[^][:\\/?]*)\\1!(?<RangeName>[^][:\\/?]*)", RegexOptions.Compiled);
  internal static readonly ExcelFunction[] SemiVolatileFunctions = new ExcelFunction[4]
  {
    ExcelFunction.CELL,
    ExcelFunction.INFO,
    ExcelFunction.NOW,
    ExcelFunction.TODAY
  };
  public static readonly FormulaToken[] NameXCodes = new FormulaToken[3]
  {
    FormulaToken.tNameX1,
    FormulaToken.tNameX2,
    FormulaToken.tNameX3
  };
  public static readonly FormulaToken[] NameCodes = new FormulaToken[3]
  {
    FormulaToken.tName1,
    FormulaToken.tName2,
    FormulaToken.tName3
  };
  private static readonly ExcelFunction[] m_excel2007Supported = new ExcelFunction[64 /*0x40*/]
  {
    ExcelFunction.HEX2BIN,
    ExcelFunction.HEX2DEC,
    ExcelFunction.HEX2OCT,
    ExcelFunction.COUNTIFS,
    ExcelFunction.BIN2DEC,
    ExcelFunction.BIN2HEX,
    ExcelFunction.BIN2OCT,
    ExcelFunction.DEC2BIN,
    ExcelFunction.DEC2HEX,
    ExcelFunction.DEC2OCT,
    ExcelFunction.OCT2BIN,
    ExcelFunction.OCT2DEC,
    ExcelFunction.OCT2HEX,
    ExcelFunction.ODDFPRICE,
    ExcelFunction.ODDFYIELD,
    ExcelFunction.ODDLPRICE,
    ExcelFunction.ODDLYIELD,
    ExcelFunction.ISODD,
    ExcelFunction.ISEVEN,
    ExcelFunction.LCM,
    ExcelFunction.GCD,
    ExcelFunction.SUMIFS,
    ExcelFunction.AVERAGEIF,
    ExcelFunction.AVERAGEIFS,
    ExcelFunction.CONVERT,
    ExcelFunction.COMPLEX,
    ExcelFunction.COUPDAYBS,
    ExcelFunction.COUPDAYS,
    ExcelFunction.COUPDAYSNC,
    ExcelFunction.COUPNCD,
    ExcelFunction.COUPNUM,
    ExcelFunction.COUPPCD,
    ExcelFunction.DELTA,
    ExcelFunction.DISC,
    ExcelFunction.DOLLARDE,
    ExcelFunction.DOLLARFR,
    ExcelFunction.DURATION,
    ExcelFunction.EDATE,
    ExcelFunction.EFFECT,
    ExcelFunction.EOMONTH,
    ExcelFunction.ERF,
    ExcelFunction.ERFC,
    ExcelFunction.FACTDOUBLE,
    ExcelFunction.GESTEP,
    ExcelFunction.IFERROR,
    ExcelFunction.IMABS,
    ExcelFunction.IMAGINARY,
    ExcelFunction.IMARGUMENT,
    ExcelFunction.IMCONJUGATE,
    ExcelFunction.IMCOS,
    ExcelFunction.IMEXP,
    ExcelFunction.IMLN,
    ExcelFunction.IMLOG10,
    ExcelFunction.IMLOG2,
    ExcelFunction.IMREAL,
    ExcelFunction.IMSIN,
    ExcelFunction.IMSQRT,
    ExcelFunction.IMSUB,
    ExcelFunction.IMSUM,
    ExcelFunction.IMDIV,
    ExcelFunction.IMPOWER,
    ExcelFunction.IMPRODUCT,
    ExcelFunction.ACCRINT,
    ExcelFunction.ACCRINTM
  };
  private static readonly ExcelFunction[] m_excel2010Supported = new ExcelFunction[59]
  {
    ExcelFunction.AGGREGATE,
    ExcelFunction.CHISQ_DIST,
    ExcelFunction.CHISQ_DIST,
    ExcelFunction.BETA_INV,
    ExcelFunction.BETA_DIST,
    ExcelFunction.BINOM_DIST,
    ExcelFunction.BINOM_INV,
    ExcelFunction.CEILING_PRECISE,
    ExcelFunction.CHISQ_DIST_RT,
    ExcelFunction.CHISQ_INV_RT,
    ExcelFunction.CHISQ_TEST,
    ExcelFunction.CONFIDENCE_NORM,
    ExcelFunction.CONFIDENCE_T,
    ExcelFunction.COVARIANCE_P,
    ExcelFunction.COVARIANCE_S,
    ExcelFunction.ERF_PRECISE,
    ExcelFunction.ERFC_PRECISE,
    ExcelFunction.EXPON_DIST,
    ExcelFunction.F_DIST,
    ExcelFunction.F_DIST_RT,
    ExcelFunction.F_INV,
    ExcelFunction.F_INV_RT,
    ExcelFunction.F_TEST,
    ExcelFunction.FLOOR_PRECISE,
    ExcelFunction.GAMMA_DIST,
    ExcelFunction.GAMMA_INV,
    ExcelFunction.GAMMALN_PRECISE,
    ExcelFunction.HYPGEOM_DIST,
    ExcelFunction.LOGNORM_DIST,
    ExcelFunction.LOGNORM_INV,
    ExcelFunction.MODE_MULT,
    ExcelFunction.MODE_SNGL,
    ExcelFunction.NEGBINOM_DIST,
    ExcelFunction.NETWORKDAYS_INTL,
    ExcelFunction.NORM_DIST,
    ExcelFunction.NORM_INV,
    ExcelFunction.NORM_S_DIST,
    ExcelFunction.PERCENTILE_EXC,
    ExcelFunction.PERCENTILE_INC,
    ExcelFunction.PERCENTRANK_EXC,
    ExcelFunction.PERCENTRANK_INC,
    ExcelFunction.POISSON_DIST,
    ExcelFunction.QUARTILE_EXC,
    ExcelFunction.QUARTILE_INC,
    ExcelFunction.RANK_AVG,
    ExcelFunction.RANK_EQ,
    ExcelFunction.STDEV_P,
    ExcelFunction.STDEV_S,
    ExcelFunction.T_DIST,
    ExcelFunction.T_DIST_2T,
    ExcelFunction.T_DIST_RT,
    ExcelFunction.T_INV,
    ExcelFunction.T_INV_2T,
    ExcelFunction.T_TEST,
    ExcelFunction.VAR_P,
    ExcelFunction.VAR_S,
    ExcelFunction.WEIBULL_DIST,
    ExcelFunction.WORKDAY_INTL,
    ExcelFunction.Z_TEST
  };
  private static readonly ExcelFunction[] m_excel2013Supported = new ExcelFunction[51]
  {
    ExcelFunction.DAYS,
    ExcelFunction.ISOWEEKNUM,
    ExcelFunction.BITAND,
    ExcelFunction.BITLSHIFT,
    ExcelFunction.BITOR,
    ExcelFunction.BITRSHIFT,
    ExcelFunction.BITXOR,
    ExcelFunction.IMCOSH,
    ExcelFunction.IMCOT,
    ExcelFunction.IMCSC,
    ExcelFunction.IMCSCH,
    ExcelFunction.IMSEC,
    ExcelFunction.IMSECH,
    ExcelFunction.IMSINH,
    ExcelFunction.IMTAN,
    ExcelFunction.PDURATION,
    ExcelFunction.RRI,
    ExcelFunction.ISFORMULA,
    ExcelFunction.SHEET,
    ExcelFunction.SHEETS,
    ExcelFunction.IFNA,
    ExcelFunction.XOR,
    ExcelFunction.FORMULATEXT,
    ExcelFunction.ACOT,
    ExcelFunction.ACOTH,
    ExcelFunction.ARABIC,
    ExcelFunction.BASE,
    ExcelFunction.CEILING_MATH,
    ExcelFunction.COMBINA,
    ExcelFunction.COT,
    ExcelFunction.COTH,
    ExcelFunction.CSC,
    ExcelFunction.CSCH,
    ExcelFunction.DECIMAL,
    ExcelFunction.FLOOR_MATH,
    ExcelFunction.ISO_CEILING,
    ExcelFunction.MUNIT,
    ExcelFunction.SEC,
    ExcelFunction.SECH,
    ExcelFunction.BINOM_DIST_RANGE,
    ExcelFunction.GAMMA,
    ExcelFunction.GAUSS,
    ExcelFunction.PERMUTATIONA,
    ExcelFunction.PHI,
    ExcelFunction.SKEW_P,
    ExcelFunction.NUMBERVALUE,
    ExcelFunction.UNICHAR,
    ExcelFunction.UNICODE,
    ExcelFunction.ENCODEURL,
    ExcelFunction.FILTERXML,
    ExcelFunction.WEBSERVICE
  };
  private NumberFormatInfo m_numberFormat;
  private WorkbookImpl m_book;
  private static readonly string[] m_arrAllOperationsDefault = new string[14]
  {
    " ",
    "&",
    "*",
    "+",
    ",",
    "-",
    "/",
    "<>",
    "<=",
    "<",
    "=",
    ">=",
    ">",
    "^"
  };
  private string[][] m_arrOperationGroups = new string[6][]
  {
    new string[2]{ " ", "," },
    new string[1]{ "^" },
    new string[2]{ "*", "/" },
    new string[2]{ "+", "-" },
    new string[1]{ "&" },
    new string[6]{ "<", "<=", "<>", "=", ">", ">=" }
  };
  private SortedList m_arrAllOperations = new SortedList((IComparer) new StringComparer());
  private SortedList[] m_arrOperationsWithPriority;
  private string m_strArrayRowSeparator = ";";
  private string m_strOperandsSeparator = ",";
  private FormulaParser m_parser;

  public static event EvaluateEventHandler FormulaEvaluator;

  static FormulaUtil()
  {
    FormulaUtil.FillTokenConstructors();
    FormulaUtil.FillExcelFunctions();
    FormulaUtil.FillErrorNames();
  }

  public FormulaUtil(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
    this.FillDefaultOperations();
    this.m_arrOperationsWithPriority = new SortedList[this.m_arrOperationGroups.Length];
    this.FillPriorities();
    IApplication application1 = this.Application;
    this.m_parser = new FormulaParser(this.m_book);
    this.SetSeparators(application1.ArgumentsSeparator, application1.RowSeparator);
  }

  public FormulaUtil(
    IApplication application,
    object parent,
    NumberFormatInfo numberFormat,
    char chArgumentsSeparator,
    char chRowSeparator)
    : base(application, parent)
  {
    this.FindParents();
    this.FillDefaultOperations();
    this.m_arrOperationsWithPriority = new SortedList[this.m_arrOperationGroups.Length];
    this.FillPriorities();
    this.m_parser = new FormulaParser(this.m_book);
    this.m_parser.NumberFormat = numberFormat;
    this.m_numberFormat = numberFormat;
    this.SetSeparators(chArgumentsSeparator, chRowSeparator);
  }

  private void FindParents()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("Workbook", "Can't find parent workbook");
  }

  private void FillDefaultOperations()
  {
    int index = 0;
    for (int length = FormulaUtil.m_arrAllOperationsDefault.Length; index < length; ++index)
      this.m_arrAllOperations.Add((object) FormulaUtil.m_arrAllOperationsDefault[index], (object) null);
  }

  private static void FillTokenConstructors()
  {
    Type[] assemblyTypes = ApplicationImpl.AssemblyTypes;
    Type c = typeof (Ptg);
    for (int index = 0; index < assemblyTypes.Length; ++index)
    {
      if (assemblyTypes[index].IsSubclassOf(c))
        FormulaUtil.RegisterTokenClass(assemblyTypes[index]);
    }
  }

  private static void FillExcelFunctionsReflection()
  {
    string[] names = Enum.GetNames(typeof (ExcelFunction));
    Type type = typeof (ExcelFunction);
    int index1 = 0;
    for (int length = names.Length; index1 < length; ++index1)
    {
      string name = names[index1];
      ExcelFunction index2 = (ExcelFunction) Enum.Parse(typeof (ExcelFunction), name, true);
      MemberInfo element = type.GetMember(name)[0];
      DefaultValueAttribute customAttribute1 = (DefaultValueAttribute) Attribute.GetCustomAttribute(element, typeof (DefaultValueAttribute));
      DescriptionAttribute customAttribute2 = (DescriptionAttribute) Attribute.GetCustomAttribute(element, typeof (DescriptionAttribute));
      ReferenceIndexAttribute[] customAttributes = (ReferenceIndexAttribute[]) Attribute.GetCustomAttributes(element, typeof (ReferenceIndexAttribute));
      FormulaUtil.RegisterFunction(customAttribute2 != null ? customAttribute2.Description : name, index2, customAttributes, customAttribute1 != null ? (int) customAttribute1.Value : -1);
    }
  }

  private static void FillExcelFunctions()
  {
    FormulaUtil.RegisterFunction("COUNT", ExcelFunction.COUNT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("IF", ExcelFunction.IF, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("ISNA", ExcelFunction.ISNA, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ISERROR", ExcelFunction.ISERROR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("SUM", ExcelFunction.SUM, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("AVERAGE", ExcelFunction.AVERAGE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("MIN", ExcelFunction.MIN, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3),
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("MAX", ExcelFunction.MAX, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3),
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("ROW", ExcelFunction.ROW, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("COLUMN", ExcelFunction.COLUMN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("NA", ExcelFunction.NA, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 0);
    FormulaUtil.RegisterFunction("NPV", ExcelFunction.NPV, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2),
      new ReferenceIndexAttribute(typeof (AreaPtg), new int[2]
      {
        2,
        1
      })
    }, -1);
    FormulaUtil.RegisterFunction("STDEV", ExcelFunction.STDEV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("DOLLAR", ExcelFunction.DOLLAR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("FIXED", ExcelFunction.FIXED, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("SIN", ExcelFunction.SIN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("COS", ExcelFunction.COS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("TAN", ExcelFunction.TAN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ATAN", ExcelFunction.ATAN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("PI", ExcelFunction.PI, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 0);
    FormulaUtil.RegisterFunction("SQRT", ExcelFunction.SQRT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("EXP", ExcelFunction.EXP, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("LN", ExcelFunction.LN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("LOG10", ExcelFunction.LOG10, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ABS", ExcelFunction.ABS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("INT", ExcelFunction.INT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("SIGN", ExcelFunction.SIGN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ROUND", ExcelFunction.ROUND, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("HEX2BIN", ExcelFunction.HEX2BIN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("HEX2DEC", ExcelFunction.HEX2DEC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("HEX2OCT", ExcelFunction.HEX2OCT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        1,
        2
      })
    }, -1);
    FormulaUtil.RegisterFunction("BIN2DEC", ExcelFunction.BIN2DEC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("BIN2HEX", ExcelFunction.BIN2HEX, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("BIN2OCT", ExcelFunction.BIN2OCT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("DEC2BIN", ExcelFunction.DEC2BIN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        1,
        2
      })
    }, -1);
    FormulaUtil.RegisterFunction("DEC2HEX", ExcelFunction.DEC2HEX, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        1,
        2
      })
    }, -1);
    FormulaUtil.RegisterFunction("DEC2OCT", ExcelFunction.DEC2OCT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        1,
        2
      })
    }, -1);
    FormulaUtil.RegisterFunction("OCT2BIN", ExcelFunction.OCT2BIN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        1,
        2
      })
    }, -1);
    FormulaUtil.RegisterFunction("OCT2DEC", ExcelFunction.OCT2DEC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("OCT2HEX", ExcelFunction.OCT2HEX, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        1,
        2
      })
    }, -1);
    FormulaUtil.RegisterFunction("ODDFPRICE", ExcelFunction.ODDFPRICE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("ODDFYIELD", ExcelFunction.ODDFYIELD, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("ODDLPRICE", ExcelFunction.ODDLPRICE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("ODDLYIELD", ExcelFunction.ODDLYIELD, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("ISEVEN", ExcelFunction.ISEVEN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ISODD", ExcelFunction.ISODD, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("AVERAGEIFS", ExcelFunction.AVERAGEIFS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("AVERAGEIF", ExcelFunction.AVERAGEIF, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("CONVERT", ExcelFunction.CONVERT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("COMPLEX", ExcelFunction.COMPLEX, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("COUPDAYBS", ExcelFunction.COUPDAYBS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("COUPDAYS", ExcelFunction.COUPDAYS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("COUPDAYSNC", ExcelFunction.COUPDAYSNC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("COUPNCD", ExcelFunction.COUPNCD, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("COUPNUM", ExcelFunction.COUPNUM, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("COUPPCD", ExcelFunction.COUPPCD, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("DELTA", ExcelFunction.DELTA, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        1,
        2
      })
    }, -1);
    FormulaUtil.RegisterFunction("DISC", ExcelFunction.DISC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("DOLLARDE", ExcelFunction.DOLLARDE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("DOLLARFR", ExcelFunction.DOLLARFR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("DURATION", ExcelFunction.DURATION, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("EDATE", ExcelFunction.EDATE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("EFFECT", ExcelFunction.EFFECT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("EOMONTH", ExcelFunction.EOMONTH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("ERF", ExcelFunction.ERF, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        1,
        2
      })
    }, -1);
    FormulaUtil.RegisterFunction("ERFC", ExcelFunction.ERFC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("FACTDOUBLE", ExcelFunction.FACTDOUBLE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("GESTEP", ExcelFunction.GESTEP, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        1,
        2
      })
    }, -1);
    FormulaUtil.RegisterFunction("IFERROR", ExcelFunction.IFERROR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("IMABS", ExcelFunction.IMABS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMAGINARY", ExcelFunction.IMAGINARY, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMARGUMENT", ExcelFunction.IMARGUMENT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMCONJUGATE", ExcelFunction.IMCONJUGATE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMCOS", ExcelFunction.IMCOS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMEXP", ExcelFunction.IMEXP, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMLN", ExcelFunction.IMLN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMLOG10", ExcelFunction.IMLOG10, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMLOG2", ExcelFunction.IMLOG2, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMPOWER", ExcelFunction.IMPOWER, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("IMPRODUCT", ExcelFunction.IMPRODUCT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("IMREAL", ExcelFunction.IMREAL, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMSIN", ExcelFunction.IMSIN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMSQRT", ExcelFunction.IMSQRT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMSUB", ExcelFunction.IMSUB, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("IMSUM", ExcelFunction.IMSUM, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("IMDIV", ExcelFunction.IMDIV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("LCM", ExcelFunction.LCM, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("SUMIFS", ExcelFunction.SUMIFS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("GCD", ExcelFunction.GCD, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("COUNTIFS", ExcelFunction.COUNTIFS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("ACCRINT", ExcelFunction.ACCRINT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("ACCRINTM", ExcelFunction.ACCRINTM, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("AGGREGATE", ExcelFunction.AGGREGATE, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1),
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3)
    }, -1);
    FormulaUtil.RegisterFunction("AMORDEGRC", ExcelFunction.AMORDEGRC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("AMORLINC", ExcelFunction.AMORLINC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("BAHTTEXT", ExcelFunction.BAHTTEXT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("BESSELI", ExcelFunction.BESSELI, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("BESSELJ", ExcelFunction.BESSELJ, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("BESSELK", ExcelFunction.BESSELK, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("BESSELY", ExcelFunction.BESSELY, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("CUBEKPIMEMBER", ExcelFunction.CUBEKPIMEMBER, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("CUBEMEMBER", ExcelFunction.CUBEMEMBER, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("CUBERANKEDMEMBER", ExcelFunction.CUBERANKEDMEMBER, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("CUBESET", ExcelFunction.CUBESET, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("CUBESETCOUNT", ExcelFunction.CUBESETCOUNT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("CUBEMEMBERPROPERTY", ExcelFunction.CUBEMEMBERPROPERTY, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("CUMIPMT", ExcelFunction.CUMIPMT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 6);
    FormulaUtil.RegisterFunction("CUMPRINC", ExcelFunction.CUMPRINC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 6);
    FormulaUtil.RegisterFunction("FVSCHEDULE", ExcelFunction.FVSCHEDULE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("INTRATE", ExcelFunction.INTRATE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("CUBEVALUE", ExcelFunction.CUBEVALUE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("MDURATION", ExcelFunction.MDURATION, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("MROUND", ExcelFunction.MROUND, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("MULTINOMIAL", ExcelFunction.MULTINOMIAL, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("NETWORKDAYS", ExcelFunction.NETWORKDAYS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("NOMINAL", ExcelFunction.NOMINAL, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("PRICE", ExcelFunction.PRICE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("PRICEDISC", ExcelFunction.PRICEDISC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("PRICEMAT", ExcelFunction.PRICEMAT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("QUOTIENT", ExcelFunction.QUOTIENT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("RANDBETWEEN", ExcelFunction.RANDBETWEEN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("RECEIVED", ExcelFunction.RECEIVED, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("SERIESSUM", ExcelFunction.SERIESSUM, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("SQRTPI", ExcelFunction.SQRTPI, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("TBILLEQ", ExcelFunction.TBILLEQ, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("TBILLPRICE", ExcelFunction.TBILLPRICE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("TBILLYIELD", ExcelFunction.TBILLYIELD, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("WEEKNUM", ExcelFunction.WEEKNUM, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("WORKDAY", ExcelFunction.WORKDAY, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("XIRR", ExcelFunction.XIRR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("XNPV", ExcelFunction.XNPV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("YEARFRAC", ExcelFunction.YEARFRAC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("YIELD", ExcelFunction.YIELD, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("YIELDDISC", ExcelFunction.YIELDDISC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("YIELDMAT", ExcelFunction.YIELDMAT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("WORKDAY.INTL", ExcelFunction.WORKDAYINTL, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("BETA.INV", ExcelFunction.BETA_INV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("BINOM.DIST", ExcelFunction.BINOM_DIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("BINOM.INV", ExcelFunction.BINOM_INV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("CEILING.PRECISE", ExcelFunction.CEILING_PRECISE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("CHISQ.DIST", ExcelFunction.CHISQ_DIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("CHISQ.DIST.RT", ExcelFunction.CHISQ_DIST_RT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("CHISQ.INV", ExcelFunction.CHISQ_INV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("CHISQ.INV.RT", ExcelFunction.CHISQ_INV_RT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("CHISQ.TEST", ExcelFunction.CHISQ_TEST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("CONFIDENCE.NORM", ExcelFunction.CONFIDENCE_NORM, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("CONFIDENCE.T", ExcelFunction.CONFIDENCE_T, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("COVARIANCE.P", ExcelFunction.COVARIANCE_P, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("COVARIANCE.S", ExcelFunction.COVARIANCE_S, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("ERF.PRECISE", ExcelFunction.ERF_PRECISE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ERFC.PRECISE", ExcelFunction.ERFC_PRECISE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("F.DIST", ExcelFunction.F_DIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("F.DIST.RT", ExcelFunction.F_DIST_RT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("F.INV", ExcelFunction.F_INV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("F.TEST", ExcelFunction.F_TEST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("FLOOR.PRECISE", ExcelFunction.FLOOR_PRECISE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("F.INV.RT", ExcelFunction.F_INV_RT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("GAMMA.DIST", ExcelFunction.GAMMA_DIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("GAMMA.INV", ExcelFunction.GAMMA_INV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("GAMMALN.PRECISE", ExcelFunction.GAMMALN_PRECISE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("HYPGEOM.DIST", ExcelFunction.HYPGEOM_DIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 5);
    FormulaUtil.RegisterFunction("LOGNORM.DIST", ExcelFunction.LOGNORM_DIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("LOGNORM.INV", ExcelFunction.LOGNORM_INV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("MODE.MULT", ExcelFunction.MODE_MULT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("MODE.SNGL", ExcelFunction.MODE_SNGL, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("NEGBINOM.DIST", ExcelFunction.NEGBINOM_DIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("NETWORKDAYS.INTL", ExcelFunction.NETWORKDAYS_INTL, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("NORM.DIST", ExcelFunction.NORM_DIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("NORM.INV", ExcelFunction.NORM_INV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("NORM.S.DIST", ExcelFunction.NORM_S_DIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("PERCENTILE.EXC", ExcelFunction.PERCENTILE_EXC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("PERCENTILE.INC", ExcelFunction.PERCENTILE_INC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("PERCENTRANK.EXC", ExcelFunction.PERCENTRANK_EXC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("PERCENTRANK.INC", ExcelFunction.PERCENTRANK_INC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("POISSON.DIST", ExcelFunction.POISSON_DIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("QUARTILE.EXC", ExcelFunction.QUARTILE_EXC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("QUARTILE.INC", ExcelFunction.QUARTILE_INC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("RANK.AVG", ExcelFunction.RANK_AVG, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("RANK.EQ", ExcelFunction.RANK_EQ, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("STDEV.P", ExcelFunction.STDEV_P, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("STDEV.S", ExcelFunction.STDEV_S, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("T.DIST", ExcelFunction.T_DIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("T.DIST.2T", ExcelFunction.T_DIST_2T, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("T.DIST.RT", ExcelFunction.T_DIST_RT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("T.INV", ExcelFunction.T_INV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("VAR.P", ExcelFunction.VAR_P, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("VAR.S", ExcelFunction.VAR_S, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("WEIBULL.DIST", ExcelFunction.WEIBULL_DIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("Z.TEST", ExcelFunction.Z_TEST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("LOOKUP", ExcelFunction.LOOKUP, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[3]
      {
        2,
        1,
        1
      }),
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3)
    }, -1);
    FormulaUtil.RegisterFunction("INDEX", ExcelFunction.INDEX, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1),
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3)
    }, -1);
    FormulaUtil.RegisterFunction("REPT", ExcelFunction.REPT, new ReferenceIndexAttribute[0], 2);
    FormulaUtil.RegisterFunction("MID", ExcelFunction.MID, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("LEN", ExcelFunction.LEN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("VALUE", ExcelFunction.VALUE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("TRUE", ExcelFunction.TRUE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 0);
    FormulaUtil.RegisterFunction("FALSE", ExcelFunction.FALSE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 0);
    FormulaUtil.RegisterFunction("AND", ExcelFunction.AND, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("OR", ExcelFunction.OR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("NOT", ExcelFunction.NOT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("MOD", ExcelFunction.MOD, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("DCOUNT", ExcelFunction.DCOUNT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 3);
    FormulaUtil.RegisterFunction("DSUM", ExcelFunction.DSUM, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 3);
    FormulaUtil.RegisterFunction("DAVERAGE", ExcelFunction.DAVERAGE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 3);
    FormulaUtil.RegisterFunction("DMIN", ExcelFunction.DMIN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 3);
    FormulaUtil.RegisterFunction("DMAX", ExcelFunction.DMAX, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 3);
    FormulaUtil.RegisterFunction("DSTDEV", ExcelFunction.DSTDEV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 3);
    FormulaUtil.RegisterFunction("VAR", ExcelFunction.VAR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("DVAR", ExcelFunction.DVAR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 3);
    FormulaUtil.RegisterFunction("TEXT", ExcelFunction.TEXT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("LINEST", ExcelFunction.LINEST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("TREND", ExcelFunction.TREND, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1),
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3)
    }, -1);
    FormulaUtil.RegisterFunction("LOGEST", ExcelFunction.LOGEST, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1),
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3)
    }, -1);
    FormulaUtil.RegisterFunction("GROWTH", ExcelFunction.GROWTH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("GOTO", ExcelFunction.GOTO, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("HALT", ExcelFunction.HALT, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("PV", ExcelFunction.PV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("FV", ExcelFunction.FV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("NPER", ExcelFunction.NPER, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("PMT", ExcelFunction.PMT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("RATE", ExcelFunction.RATE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("MIRR", ExcelFunction.MIRR, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3),
      new ReferenceIndexAttribute(typeof (RefPtg), new int[3]
      {
        1,
        2,
        2
      })
    }, 3);
    FormulaUtil.RegisterFunction("IRR", ExcelFunction.IRR, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3),
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("RAND", ExcelFunction.RAND, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 0);
    FormulaUtil.RegisterFunction("MATCH", ExcelFunction.MATCH, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        2,
        1
      }),
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3)
    }, -1);
    FormulaUtil.RegisterFunction("DATE", ExcelFunction.DATE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("TIME", ExcelFunction.TIME, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("DAY", ExcelFunction.DAY, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("MONTH", ExcelFunction.MONTH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("YEAR", ExcelFunction.YEAR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("WEEKDAY", ExcelFunction.WEEKDAY, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("HOUR", ExcelFunction.HOUR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("MINUTE", ExcelFunction.MINUTE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("SECOND", ExcelFunction.SECOND, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("NOW", ExcelFunction.NOW, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 0);
    FormulaUtil.RegisterFunction("AREAS", ExcelFunction.AREAS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 1);
    FormulaUtil.RegisterFunction("ROWS", ExcelFunction.ROWS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 1);
    FormulaUtil.RegisterFunction("COLUMNS", ExcelFunction.COLUMNS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 1);
    FormulaUtil.RegisterFunction("OFFSET", ExcelFunction.OFFSET, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("ABSREF", ExcelFunction.ABSREF, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("RELREF", ExcelFunction.RELREF, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("ARGUMENT", ExcelFunction.ARGUMENT, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("SEARCH", ExcelFunction.SEARCH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("TRANSPOSE", ExcelFunction.TRANSPOSE, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3),
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ERROR", ExcelFunction.ERROR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("STEP", ExcelFunction.STEP, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("TYPE", ExcelFunction.TYPE, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2),
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3)
    }, 1);
    FormulaUtil.RegisterFunction("ECHO", ExcelFunction.ECHO, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("SETNAME", ExcelFunction.SETNAME, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("CALLER", ExcelFunction.CALLER, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("DEREF", ExcelFunction.DEREF, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("WINDOWS", ExcelFunction.WINDOWS, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("SERIES", ExcelFunction.SERIES, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("DOCUMENTS", ExcelFunction.DOCUMENTS, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("ACTIVECELL", ExcelFunction.ACTIVECELL, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("SELECTION", ExcelFunction.SELECTION, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("RESULT", ExcelFunction.RESULT, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("ATAN2", ExcelFunction.ATAN2, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("ASIN", ExcelFunction.ASIN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ACOS", ExcelFunction.ACOS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("CHOOSE", ExcelFunction.CHOOSE, new ReferenceIndexAttribute[0], -1);
    FormulaUtil.RegisterFunction("HLOOKUP", ExcelFunction.HLOOKUP, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        2,
        1
      })
    }, -1);
    FormulaUtil.RegisterFunction("VLOOKUP", ExcelFunction.VLOOKUP, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        2,
        1
      })
    }, -1);
    FormulaUtil.RegisterFunction("LINKS", ExcelFunction.LINKS, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("INPUT", ExcelFunction.INPUT, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("ISREF", ExcelFunction.ISREF, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 1);
    FormulaUtil.RegisterFunction("GETFORMULA", ExcelFunction.GETFORMULA, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("GETNAME", ExcelFunction.GETNAME, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("SETVALUE", ExcelFunction.SETVALUE, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("LOG", ExcelFunction.LOG, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("EXEC", ExcelFunction.EXEC, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("CHAR", ExcelFunction.CHAR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("LOWER", ExcelFunction.LOWER, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("UPPER", ExcelFunction.UPPER, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("PROPER", ExcelFunction.PROPER, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("LEFT", ExcelFunction.LEFT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("RIGHT", ExcelFunction.RIGHT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("EXACT", ExcelFunction.EXACT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("TRIM", ExcelFunction.TRIM, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("REPLACE", ExcelFunction.REPLACE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("SUBSTITUTE", ExcelFunction.SUBSTITUTE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("CODE", ExcelFunction.CODE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("NAMES", ExcelFunction.NAMES, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("DIRECTORY", ExcelFunction.DIRECTORY, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("FIND", ExcelFunction.FIND, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("CELL", ExcelFunction.CELL, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        2,
        1
      })
    }, -1);
    FormulaUtil.RegisterFunction("ISERR", ExcelFunction.ISERR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ISTEXT", ExcelFunction.ISTEXT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ISNUMBER", ExcelFunction.ISNUMBER, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ISBLANK", ExcelFunction.ISBLANK, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("T", ExcelFunction.T, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 1);
    FormulaUtil.RegisterFunction("N", ExcelFunction.N, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 1);
    FormulaUtil.RegisterFunction("FOPEN", ExcelFunction.FOPEN, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("FCLOSE", ExcelFunction.FCLOSE, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("FSIZE", ExcelFunction.FSIZE, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("FREADLN", ExcelFunction.FREADLN, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("FREAD", ExcelFunction.FREAD, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("FWRITELN", ExcelFunction.FWRITELN, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("FWRITE", ExcelFunction.FWRITE, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("FPOS", ExcelFunction.FPOS, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("DATEVALUE", ExcelFunction.DATEVALUE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("TIMEVALUE", ExcelFunction.TIMEVALUE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("SLN", ExcelFunction.SLN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("SYD", ExcelFunction.SYD, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("DDB", ExcelFunction.DDB, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("GETDEF", ExcelFunction.GETDEF, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("REFTEXT", ExcelFunction.REFTEXT, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("TEXTREF", ExcelFunction.TEXTREF, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("INDIRECT", ExcelFunction.INDIRECT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("REGISTER", ExcelFunction.REGISTER, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("CALL", ExcelFunction.CALL, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("ADDBAR", ExcelFunction.ADDBAR, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("ADDMENU", ExcelFunction.ADDMENU, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("ADDCOMMAND", ExcelFunction.ADDCOMMAND, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("ENABLECOMMAND", ExcelFunction.ENABLECOMMAND, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("CHECKCOMMAND", ExcelFunction.CHECKCOMMAND, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("RENAMECOMMAND", ExcelFunction.RENAMECOMMAND, new ReferenceIndexAttribute[0], 2);
    FormulaUtil.RegisterFunction("SHOWBAR", ExcelFunction.SHOWBAR, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("DELETEMENU", ExcelFunction.DELETEMENU, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("DELETECOMMAND", ExcelFunction.DELETECOMMAND, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("GETCHARTITEM", ExcelFunction.GETCHARTITEM, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("DIALOGBOX", ExcelFunction.DIALOGBOX, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("CLEAN", ExcelFunction.CLEAN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("MDETERM", ExcelFunction.MDETERM, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, 1);
    FormulaUtil.RegisterFunction("MINVERSE", ExcelFunction.MINVERSE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, 1);
    FormulaUtil.RegisterFunction("MMULT", ExcelFunction.MMULT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, 2);
    FormulaUtil.RegisterFunction("FILES", ExcelFunction.FILES, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("IPMT", ExcelFunction.IPMT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("PPMT", ExcelFunction.PPMT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("COUNTA", ExcelFunction.COUNTA, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("CANCELKEY", ExcelFunction.CANCELKEY, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("INITIATE", ExcelFunction.INITIATE, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("REQUEST", ExcelFunction.REQUEST, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("POKE", ExcelFunction.POKE, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("EXECUTE", ExcelFunction.EXECUTE, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("TERMINATE", ExcelFunction.TERMINATE, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("RESTART", ExcelFunction.RESTART, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("HELP", ExcelFunction.HELP, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("GETBAR", ExcelFunction.GETBAR, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("PRODUCT", ExcelFunction.PRODUCT, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1),
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3)
    }, -1);
    FormulaUtil.RegisterFunction("FACT", ExcelFunction.FACT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("GETCELL", ExcelFunction.GETCELL, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("GETWORKSPACE", ExcelFunction.GETWORKSPACE, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("GETWINDOW", ExcelFunction.GETWINDOW, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("GETDOCUMENT", ExcelFunction.GETDOCUMENT, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("DPRODUCT", ExcelFunction.DPRODUCT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 3);
    FormulaUtil.RegisterFunction("ISNONTEXT", ExcelFunction.ISNONTEXT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("GETNOTE", ExcelFunction.GETNOTE, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("NOTE", ExcelFunction.NOTE, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("STDEVP", ExcelFunction.STDEVP, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("VARP", ExcelFunction.VARP, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("DSTDEVP", ExcelFunction.DSTDEVP, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 3);
    FormulaUtil.RegisterFunction("DVARP", ExcelFunction.DVARP, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 3);
    FormulaUtil.RegisterFunction("TRUNC", ExcelFunction.TRUNC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("ISLOGICAL", ExcelFunction.ISLOGICAL, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("DCOUNTA", ExcelFunction.DCOUNTA, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 3);
    FormulaUtil.RegisterFunction("DELETEBAR", ExcelFunction.DELETEBAR, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("UNREGISTER", ExcelFunction.UNREGISTER, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("USDOLLAR", ExcelFunction.USDOLLAR, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("FINDB", ExcelFunction.FINDB, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("SEARCHB", ExcelFunction.SEARCHB, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("REPLACEB", ExcelFunction.REPLACEB, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("LEFTB", ExcelFunction.LEFTB, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("RIGHTB", ExcelFunction.RIGHTB, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("MIDB", ExcelFunction.MIDB, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("LENB", ExcelFunction.LENB, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ROUNDUP", ExcelFunction.ROUNDUP, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("ROUNDDOWN", ExcelFunction.ROUNDDOWN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("ASC", ExcelFunction.ASC, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("DBCS", ExcelFunction.DBCS, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("RANK", ExcelFunction.RANK, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        2,
        1
      })
    }, -1);
    FormulaUtil.RegisterFunction("ADDRESS", ExcelFunction.ADDRESS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("DAYS360", ExcelFunction.DAYS360, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("TODAY", ExcelFunction.TODAY, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 0);
    FormulaUtil.RegisterFunction("VDB", ExcelFunction.VDB, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("MEDIAN", ExcelFunction.MEDIAN, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1),
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3)
    }, -1);
    FormulaUtil.RegisterFunction("SUMPRODUCT", ExcelFunction.SUMPRODUCT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, -1);
    FormulaUtil.RegisterFunction("SINH", ExcelFunction.SINH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("COSH", ExcelFunction.COSH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("TANH", ExcelFunction.TANH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ASINH", ExcelFunction.ASINH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ACOSH", ExcelFunction.ACOSH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ATANH", ExcelFunction.ATANH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("DGET", ExcelFunction.DGET, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 3);
    FormulaUtil.RegisterFunction("CREATEOBJECT", ExcelFunction.CREATEOBJECT, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("VOLATILE", ExcelFunction.VOLATILE, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("LASTERROR", ExcelFunction.LASTERROR, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("CUSTOMUNDO", ExcelFunction.CUSTOMUNDO, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("CUSTOMREPEAT", ExcelFunction.CUSTOMREPEAT, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("FORMULACONVERT", ExcelFunction.FORMULACONVERT, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("GETLINKINFO", ExcelFunction.GETLINKINFO, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("TEXTBOX", ExcelFunction.TEXTBOX, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("INFO", ExcelFunction.INFO, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("GROUP", ExcelFunction.GROUP, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("GETOBJECT", ExcelFunction.GETOBJECT, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("DB", ExcelFunction.DB, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("PAUSE", ExcelFunction.PAUSE, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("RESUME", ExcelFunction.RESUME, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("FREQUENCY", ExcelFunction.FREQUENCY, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 2);
    FormulaUtil.RegisterFunction("ADDTOOLBAR", ExcelFunction.ADDTOOLBAR, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("DELETETOOLBAR", ExcelFunction.DELETETOOLBAR, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("CustomFunction", ExcelFunction.CustomFunction, new ReferenceIndexAttribute[0], -1);
    FormulaUtil.RegisterFunction("RESETTOOLBAR", ExcelFunction.RESETTOOLBAR, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("EVALUATE", ExcelFunction.EVALUATE, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("GETTOOLBAR", ExcelFunction.GETTOOLBAR, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("GETTOOL", ExcelFunction.GETTOOL, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("SPELLINGCHECK", ExcelFunction.SPELLINGCHECK, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("ERROR.TYPE", ExcelFunction.ERRORTYPE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("APPTITLE", ExcelFunction.APPTITLE, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("WINDOWTITLE", ExcelFunction.WINDOWTITLE, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("SAVETOOLBAR", ExcelFunction.SAVETOOLBAR, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("ENABLETOOL", ExcelFunction.ENABLETOOL, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("PRESSTOOL", ExcelFunction.PRESSTOOL, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("REGISTERID", ExcelFunction.REGISTERID, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("GETWORKBOOK", ExcelFunction.GETWORKBOOK, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("AVEDEV", ExcelFunction.AVEDEV, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1),
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3)
    }, -1);
    FormulaUtil.RegisterFunction("BETA.DIST", ExcelFunction.BETA_DIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("T.TEST", ExcelFunction.T_TEST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("T.INV.2T", ExcelFunction.T_INV_2T, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("EXPON.DIST", ExcelFunction.EXPON_DIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("BETADIST", ExcelFunction.BETADIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("EUROCONVERT", ExcelFunction.EUROCONVERT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 5);
    FormulaUtil.RegisterFunction("REGISTER.ID", ExcelFunction.REGISTER_ID, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("PHONETIC", ExcelFunction.PHONETIC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("SQL.REQUEST", ExcelFunction.SQL_REQUEST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("JIS", ExcelFunction.JIS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("GAMMALN", ExcelFunction.GAMMALN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("BETAINV", ExcelFunction.BETAINV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("BINOMDIST", ExcelFunction.BINOMDIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("CHIDIST", ExcelFunction.CHIDIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("CHIINV", ExcelFunction.CHIINV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("COMBIN", ExcelFunction.COMBIN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("CONFIDENCE", ExcelFunction.CONFIDENCE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("CRITBINOM", ExcelFunction.CRITBINOM, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("EVEN", ExcelFunction.EVEN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("EXPONDIST", ExcelFunction.EXPONDIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("FDIST", ExcelFunction.FDIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("FINV", ExcelFunction.FINV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("FISHER", ExcelFunction.FISHER, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("FISHERINV", ExcelFunction.FISHERINV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("FLOOR", ExcelFunction.FLOOR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("GAMMADIST", ExcelFunction.GAMMADIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("GAMMAINV", ExcelFunction.GAMMAINV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("CEILING", ExcelFunction.CEILING, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("HYPGEOMDIST", ExcelFunction.HYPGEOMDIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("LOGNORMDIST", ExcelFunction.LOGNORMDIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("LOGINV", ExcelFunction.LOGINV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("NEGBINOMDIST", ExcelFunction.NEGBINOMDIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("NORMDIST", ExcelFunction.NORMDIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("NORMSDIST", ExcelFunction.NORMSDIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("NORMINV", ExcelFunction.NORMINV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("NORMSINV", ExcelFunction.NORMSINV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("STANDARDIZE", ExcelFunction.STANDARDIZE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("ODD", ExcelFunction.ODD, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("PERMUT", ExcelFunction.PERMUT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("POISSON", ExcelFunction.POISSON, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("TDIST", ExcelFunction.TDIST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("WEIBULL", ExcelFunction.WEIBULL, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("SUMXMY2", ExcelFunction.SUMXMY2, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, 2);
    FormulaUtil.RegisterFunction("SUMX2MY2", ExcelFunction.SUMX2MY2, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, 2);
    FormulaUtil.RegisterFunction("SUMX2PY2", ExcelFunction.SUMX2PY2, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, 2);
    FormulaUtil.RegisterFunction("CHITEST", ExcelFunction.CHITEST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, 2);
    FormulaUtil.RegisterFunction("CORREL", ExcelFunction.CORREL, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, 2);
    FormulaUtil.RegisterFunction("COVAR", ExcelFunction.COVAR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, 2);
    FormulaUtil.RegisterFunction("FORECAST", ExcelFunction.FORECAST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[3]
      {
        2,
        3,
        3
      })
    }, 3);
    FormulaUtil.RegisterFunction("FTEST", ExcelFunction.FTEST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, 2);
    FormulaUtil.RegisterFunction("INTERCEPT", ExcelFunction.INTERCEPT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, 2);
    FormulaUtil.RegisterFunction("PEARSON", ExcelFunction.PEARSON, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, 2);
    FormulaUtil.RegisterFunction("RSQ", ExcelFunction.RSQ, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, 2);
    FormulaUtil.RegisterFunction("STEYX", ExcelFunction.STEYX, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, 2);
    FormulaUtil.RegisterFunction("SLOPE", ExcelFunction.SLOPE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, 2);
    FormulaUtil.RegisterFunction("TTEST", ExcelFunction.TTEST, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, 4);
    FormulaUtil.RegisterFunction("PROB", ExcelFunction.PROB, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[3]
      {
        3,
        3,
        2
      })
    }, -1);
    FormulaUtil.RegisterFunction("DEVSQ", ExcelFunction.DEVSQ, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("GEOMEAN", ExcelFunction.GEOMEAN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("HARMEAN", ExcelFunction.HARMEAN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("SUMSQ", ExcelFunction.SUMSQ, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("KURT", ExcelFunction.KURT, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3),
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("SKEW", ExcelFunction.SKEW, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("ZTEST", ExcelFunction.ZTEST, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1),
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3)
    }, -1);
    FormulaUtil.RegisterFunction("LARGE", ExcelFunction.LARGE, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1),
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3)
    }, 2);
    FormulaUtil.RegisterFunction("SMALL", ExcelFunction.SMALL, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 2);
    FormulaUtil.RegisterFunction("QUARTILE", ExcelFunction.QUARTILE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        1,
        2
      })
    }, 2);
    FormulaUtil.RegisterFunction("PERCENTILE", ExcelFunction.PERCENTILE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        1,
        2
      })
    }, 2);
    FormulaUtil.RegisterFunction("PERCENTRANK", ExcelFunction.PERCENTRANK, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        1,
        2
      })
    }, -1);
    FormulaUtil.RegisterFunction("MODE", ExcelFunction.MODE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, -1);
    FormulaUtil.RegisterFunction("TRIMMEAN", ExcelFunction.TRIMMEAN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 3)
    }, 2);
    FormulaUtil.RegisterFunction("TINV", ExcelFunction.TINV, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("MOVIECOMMAND", ExcelFunction.MOVIECOMMAND, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("GETMOVIE", ExcelFunction.GETMOVIE, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("CONCATENATE", ExcelFunction.CONCATENATE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("POWER", ExcelFunction.POWER, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("PIVOTADDDATA", ExcelFunction.PIVOTADDDATA, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("GETPIVOTTABLE", ExcelFunction.GETPIVOTTABLE, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("GETPIVOTFIELD", ExcelFunction.GETPIVOTFIELD, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("GETPIVOTITEM", ExcelFunction.GETPIVOTITEM, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("RADIANS", ExcelFunction.RADIANS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("DEGREES", ExcelFunction.DEGREES, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("SUBTOTAL", ExcelFunction.SUBTOTAL, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        2,
        1
      })
    }, -1);
    FormulaUtil.RegisterFunction("SUMIF", ExcelFunction.SUMIF, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[3]
      {
        1,
        2,
        1
      })
    }, -1);
    FormulaUtil.RegisterFunction("COUNTIF", ExcelFunction.COUNTIF, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), new int[2]
      {
        1,
        2
      })
    }, 2);
    FormulaUtil.RegisterFunction("COUNTBLANK", ExcelFunction.COUNTBLANK, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, 1);
    FormulaUtil.RegisterFunction("SCENARIOGET", ExcelFunction.SCENARIOGET, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("OPTIONSLISTSGET", ExcelFunction.OPTIONSLISTSGET, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("ISPMT", ExcelFunction.ISPMT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("DATEDIF", ExcelFunction.DATEDIF, new ReferenceIndexAttribute[0], 3);
    FormulaUtil.RegisterFunction("DATESTRING", ExcelFunction.DATESTRING, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("NUMBERSTRING", ExcelFunction.NUMBERSTRING, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("ROMAN", ExcelFunction.ROMAN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("OPENDIALOG", ExcelFunction.OPENDIALOG, new ReferenceIndexAttribute[0], 1);
    FormulaUtil.RegisterFunction("SAVEDIALOG", ExcelFunction.SAVEDIALOG, new ReferenceIndexAttribute[0], 0);
    FormulaUtil.RegisterFunction("GETPIVOTDATA", ExcelFunction.GETPIVOTDATA, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("HYPERLINK", ExcelFunction.HYPERLINK, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("AVERAGEA", ExcelFunction.AVERAGEA, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("MAXA", ExcelFunction.MAXA, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1),
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3)
    }, -1);
    FormulaUtil.RegisterFunction("MINA", ExcelFunction.MINA, new ReferenceIndexAttribute[2]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1),
      new ReferenceIndexAttribute(typeof (ArrayPtg), 3)
    }, -1);
    FormulaUtil.RegisterFunction("STDEVPA", ExcelFunction.STDEVPA, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("VARPA", ExcelFunction.VARPA, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("STDEVA", ExcelFunction.STDEVA, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("VARA", ExcelFunction.VARA, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 1)
    }, -1);
    FormulaUtil.RegisterFunction("NONE", ExcelFunction.NONE, new ReferenceIndexAttribute[0], -1);
    FormulaUtil.RegisterFunction("DAYS", ExcelFunction.DAYS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("ISOWEEKNUM", ExcelFunction.ISOWEEKNUM, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("BITAND", ExcelFunction.BITAND, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("BITLSHIFT", ExcelFunction.BITLSHIFT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("BITOR", ExcelFunction.BITOR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("BITRSHIFT", ExcelFunction.BITRSHIFT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("BITXOR", ExcelFunction.BITXOR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("IMCOSH", ExcelFunction.IMCOSH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMCOT", ExcelFunction.IMCOT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMCSC", ExcelFunction.IMCSC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMCSCH", ExcelFunction.IMCSCH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMSEC", ExcelFunction.IMSEC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMSECH", ExcelFunction.IMSECH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMSINH", ExcelFunction.IMSINH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IMTAN", ExcelFunction.IMTAN, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("PDURATION", ExcelFunction.PDURATION, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("RRI", ExcelFunction.RRI, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("ISFORMULA", ExcelFunction.ISFORMULA, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("SHEET", ExcelFunction.SHEET, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("SHEETS", ExcelFunction.SHEETS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("IFNA", ExcelFunction.IFNA, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("XOR", ExcelFunction.XOR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("FORMULATEXT", ExcelFunction.FORMULATEXT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ACOT", ExcelFunction.ACOT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ACOTH", ExcelFunction.ACOTH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ARABIC", ExcelFunction.ARABIC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("BASE", ExcelFunction.BASE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("CEILING.MATH", ExcelFunction.CEILING_MATH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("COMBINA", ExcelFunction.COMBINA, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("COT", ExcelFunction.COT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("COTH", ExcelFunction.COTH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("CSC", ExcelFunction.CSC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("CSCH", ExcelFunction.CSCH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("DECIMAL", ExcelFunction.DECIMAL, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("FLOOR.MATH", ExcelFunction.FLOOR_MATH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("ISO.CEILING", ExcelFunction.ISO_CEILING, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("MUNIT", ExcelFunction.MUNIT, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("SEC", ExcelFunction.SEC, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("SECH", ExcelFunction.SECH, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("BINOM.DIST.RANGE", ExcelFunction.BINOM_DIST_RANGE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 4);
    FormulaUtil.RegisterFunction("GAMMA", ExcelFunction.GAMMA, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("GAUSS", ExcelFunction.GAUSS, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("PERMUTATIONA", ExcelFunction.PERMUTATIONA, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("PHI", ExcelFunction.PHI, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("SKEW.P", ExcelFunction.SKEW_P, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, -1);
    FormulaUtil.RegisterFunction("NUMBERVALUE", ExcelFunction.NUMBERVALUE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 3);
    FormulaUtil.RegisterFunction("UNICHAR", ExcelFunction.UNICHAR, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("UNICODE", ExcelFunction.UNICODE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("ENCODEURL", ExcelFunction.ENCODEURL, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
    FormulaUtil.RegisterFunction("FILTERXML", ExcelFunction.FILTERXML, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 2);
    FormulaUtil.RegisterFunction("WEBSERVICE", ExcelFunction.WEBSERVICE, new ReferenceIndexAttribute[1]
    {
      new ReferenceIndexAttribute(typeof (RefPtg), 2)
    }, 1);
  }

  private static void FillErrorNames()
  {
    Type[] assemblyTypes = ApplicationImpl.AssemblyTypes;
    int index = 0;
    for (int length = assemblyTypes.Length; index < length; ++index)
      FormulaUtil.AddErrorNames(assemblyTypes[index]);
  }

  private static void AddErrorNames(Type type)
  {
    object[] customAttributes = type.GetCustomAttributes(typeof (ErrorCodeAttribute), false);
    int index = 0;
    for (int length = customAttributes.Length; index < length; ++index)
    {
      ErrorCodeAttribute errorCodeAttribute = customAttributes[index] as ErrorCodeAttribute;
      ConstructorInfo constructor = type.GetConstructor(new Type[1]
      {
        typeof (string)
      });
      FormulaUtil.ErrorNameToConstructor.Add(errorCodeAttribute.StringValue, constructor);
      FormulaUtil.s_hashErrorCodeToName.Add(errorCodeAttribute.ErrorCode, errorCodeAttribute.StringValue);
      FormulaUtil.s_hashNameToErrorCode.Add(errorCodeAttribute.StringValue, errorCodeAttribute.ErrorCode);
    }
  }

  private void FillPriorities()
  {
    int length1 = this.m_arrOperationGroups.Length;
    for (int index1 = 0; index1 < length1; ++index1)
    {
      int index2 = 0;
      for (int length2 = this.m_arrOperationGroups[index1].Length; index2 < length2; ++index2)
        this.m_arrAllOperations[(object) this.m_arrOperationGroups[index1][index2]] = (object) index1;
    }
    int length3 = 0;
    string[] strArray = (string[]) null;
    for (int index = length1 - 1; index >= 0; --index)
    {
      int length4 = this.m_arrOperationGroups[index].Length;
      length3 += length4;
      string[] arrStrings = new string[length3];
      if (index < length1 - 1)
        strArray.CopyTo((Array) arrStrings, length4);
      this.m_arrOperationGroups[index].CopyTo((Array) arrStrings, 0);
      this.m_arrOperationsWithPriority[index] = FormulaUtil.GetSortedList(arrStrings);
      strArray = arrStrings;
    }
  }

  private static SortedList GetSortedList(string[] arrStrings)
  {
    int capacity = arrStrings != null ? arrStrings.Length : throw new ArgumentNullException(nameof (arrStrings));
    SortedList sortedList = new SortedList((IComparer) new StringComparer(), capacity);
    for (int index = 0; index < capacity; ++index)
      sortedList.Add((object) arrStrings[index], (object) null);
    return sortedList;
  }

  public Ptg[] ParseSharedString(
    string strFormula,
    int iFirstRow,
    int iFirstColumn,
    IWorksheet sheet)
  {
    Ptg[] tokens = this.ParseString(strFormula, sheet, (Dictionary<string, string>) null);
    IWorkbook workbook = sheet.Workbook;
    return this.ConvertTokensToShared(tokens, iFirstRow, iFirstColumn, workbook);
  }

  public Ptg[] ConvertTokensToShared(Ptg[] tokens, int row, int column, IWorkbook book)
  {
    if (tokens != null)
    {
      int length = tokens.Length;
      for (int index = 0; index < length; ++index)
      {
        if (!(tokens[index] is IReference))
          tokens[index] = tokens[index].ConvertPtgToNPtg(book, row - 1, column - 1);
      }
    }
    return tokens;
  }

  public Ptg[] ParseString(string strFormula)
  {
    return this.ParseString(strFormula, (IWorksheet) null, (Dictionary<string, string>) null);
  }

  public Ptg[] ParseString(
    string strFormula,
    IWorksheet sheet,
    Dictionary<string, string> hashWorksheetNames)
  {
    return this.ParseString(strFormula, sheet, (Dictionary<Type, ReferenceIndexAttribute>) null, 0, hashWorksheetNames, OfficeParseFormulaOptions.RootLevel, 0, 0);
  }

  public Ptg[] ParseString(
    string strFormula,
    IWorksheet sheet,
    Dictionary<string, string> hashWorksheetNames,
    int iCellRow,
    int iCellColumn,
    bool bR1C1)
  {
    OfficeParseFormulaOptions options = bR1C1 ? OfficeParseFormulaOptions.RootLevel | OfficeParseFormulaOptions.UseR1C1 : OfficeParseFormulaOptions.RootLevel;
    ParseParameters arguments = new ParseParameters(sheet, hashWorksheetNames, bR1C1, iCellRow, iCellColumn, this, (IWorkbook) this.m_book);
    this.m_parser.Parse(strFormula, (Dictionary<Type, ReferenceIndexAttribute>) null, 0, options, arguments);
    return this.m_parser.Tokens.ToArray();
  }

  public Ptg[] ParseString(
    string strFormula,
    IWorksheet sheet,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    Dictionary<string, string> hashWorksheetNames,
    OfficeParseFormulaOptions options,
    int iCellRow,
    int iCellColumn)
  {
    bool r1C1 = (options & OfficeParseFormulaOptions.UseR1C1) != OfficeParseFormulaOptions.None;
    ParseParameters arguments = new ParseParameters(sheet, hashWorksheetNames, r1C1, iCellRow, iCellColumn, this, (IWorkbook) this.m_book);
    this.m_parser.Parse(strFormula, indexes, i, options, arguments);
    return this.m_parser.Tokens.ToArray();
  }

  public string GetLeftUnaryOperand(string strFormula, int OpIndex)
  {
    return FormulaUtil.GetOperand(strFormula, OpIndex, this.m_arrAllOperations, true);
  }

  public string GetRightUnaryOperand(string strFormula, int OpIndex)
  {
    string rightUnaryOperand = FormulaUtil.GetOperand(strFormula, OpIndex, this.m_arrAllOperations, false);
    int length = rightUnaryOperand.Length;
    if (length > 0 && rightUnaryOperand[length - 1] == '%')
      rightUnaryOperand = '%'.ToString() + rightUnaryOperand.Substring(0, length - 1);
    return rightUnaryOperand;
  }

  public string GetRightBinaryOperand(string strFormula, int iFirstChar, string operation)
  {
    int arrAllOperation = (int) this.m_arrAllOperations[(object) operation];
    return FormulaUtil.GetOperand(strFormula, iFirstChar - 1, this.m_arrOperationsWithPriority[arrAllOperation], false);
  }

  public string GetFunctionOperand(string strFormula, int iFirstChar)
  {
    SortedList sortedList = FormulaUtil.GetSortedList(new string[1]
    {
      this.OperandsSeparator
    });
    return FormulaUtil.GetOperand(strFormula, iFirstChar, sortedList, false);
  }

  [CLSCompliant(false)]
  public string ParseFormulaRecord(FormulaRecord formula)
  {
    return this.ParseFormulaRecord(formula, false);
  }

  [CLSCompliant(false)]
  public string ParseFormulaRecord(FormulaRecord formula, bool bR1C1)
  {
    return this.ParsePtgArray(formula.ParsedExpression, formula.Row, formula.Column, bR1C1, false);
  }

  [CLSCompliant(false)]
  public string ParseSharedFormula(ISharedFormula sharedFormula)
  {
    return this.ParsePtgArray(sharedFormula.Formula, 0, 0, false, false);
  }

  [CLSCompliant(false)]
  public string ParseSharedFormula(ISharedFormula sharedFormula, int row, int col)
  {
    return this.ParseSharedFormula(sharedFormula, row, col, false, false);
  }

  [CLSCompliant(false)]
  public string ParseSharedFormula(
    ISharedFormula sharedFormula,
    int row,
    int col,
    bool bR1C1,
    bool isForSerialization)
  {
    return this.ParsePtgArray(sharedFormula.Formula, row, col, bR1C1, isForSerialization);
  }

  public string ParsePtgArray(Ptg[] ptgs)
  {
    return ptgs == null ? (string) null : this.ParsePtgArray(ptgs, 0, 0, false, false);
  }

  public string ParsePtgArray(Ptg[] ptgs, int row, int col, bool bR1C1, bool isForSerialization)
  {
    return this.ParsePtgArray(ptgs, row, col, bR1C1, (NumberFormatInfo) null, isForSerialization);
  }

  public string ParsePtgArray(
    Ptg[] ptgs,
    int row,
    int col,
    bool bR1C1,
    NumberFormatInfo numberInfo,
    bool isForSerialization)
  {
    return this.ParsePtgArray(ptgs, row, col, bR1C1, numberInfo, false, isForSerialization, (IWorksheet) null);
  }

  public string ParsePtgArray(
    Ptg[] ptgs,
    int row,
    int col,
    bool bR1C1,
    NumberFormatInfo numberInfo,
    bool bRemoveSheetNames,
    bool isForSerialization,
    IWorksheet sheet)
  {
    if (ptgs == null)
      return (string) null;
    if (numberInfo == null)
      numberInfo = this.m_numberFormat;
    ptgs = FormulaUtil.SkipUnnecessaryTokens(ptgs);
    string empty = string.Empty;
    Stack<object> operands = new Stack<object>();
    int index = 0;
    for (int length = ptgs.Length; index < length; ++index)
    {
      Ptg ptg = ptgs[index];
      if (!ptg.IsOperation)
      {
        ISheetReference sheetReference = ptg as ISheetReference;
        string operand = !bRemoveSheetNames || sheetReference == null ? ptg.ToString(this, row, col, bR1C1, numberInfo, isForSerialization, sheet) : sheetReference.BaseToString(this, row, col, bR1C1);
        FormulaUtil.PushOperandToStack(operands, operand);
      }
      else
        ((OperationPtg) ptg).PushResultToStack(this, operands, isForSerialization);
    }
    if (operands.Count != 0)
      empty += operands.Pop().ToString();
    return empty;
  }

  public void CheckFormulaVersion(Ptg[] ptgs)
  {
    if (ptgs == null)
      return;
    int index = 0;
    for (int length = ptgs.Length; index < length; ++index)
    {
      if (ptgs[index] is FunctionPtg ptg && FormulaUtil.IsExcel2010Function(ptg.FunctionIndex) && this.m_book.Version != OfficeVersion.Excel2010 && this.m_book.Version != OfficeVersion.Excel2013)
        throw new NotSupportedException("The formula is not supported in this Version");
    }
  }

  public List<string> SplitArray(string strFormula, string strSeparator)
  {
    switch (strFormula)
    {
      case null:
        throw new ArgumentNullException(nameof (strFormula));
      case "":
        throw new ArgumentException("strFormula - string cannot be empty");
      default:
        List<string> stringList = new List<string>();
        SortedList arrBreakStrings = new SortedList(1);
        arrBreakStrings.Add((object) strSeparator, (object) null);
        int OpIndex = -1;
        int length = strFormula.Length;
        while (OpIndex < length)
        {
          string operand = FormulaUtil.GetOperand(strFormula, OpIndex, arrBreakStrings, false);
          OpIndex += operand.Length + 1;
          stringList.Add(operand);
        }
        return stringList;
    }
  }

  public bool UpdateNameIndex(Ptg ptg, int[] arrNewIndex)
  {
    if (ptg == null)
      return false;
    bool flag = false;
    if (FormulaUtil.IndexOf(FormulaUtil.NameCodes, ptg.TokenCode) != -1)
    {
      NamePtg namePtg = (NamePtg) ptg;
      int index = (int) namePtg.ExternNameIndex - 1;
      int num = arrNewIndex[index];
      if (index != num)
      {
        namePtg.ExternNameIndex = (ushort) (num + 1);
        flag = true;
      }
    }
    else if (FormulaUtil.IndexOf(FormulaUtil.NameXCodes, ptg.TokenCode) != -1)
    {
      NameXPtg nameXptg = (NameXPtg) ptg;
      if (this.m_book.IsLocalReference((int) nameXptg.RefIndex))
      {
        int index = (int) nameXptg.NameIndex - 1;
        int num = arrNewIndex[index];
        if (num != index)
        {
          nameXptg.NameIndex = (ushort) (num + 1);
          flag = true;
        }
      }
    }
    return flag;
  }

  public bool UpdateNameIndex(Ptg ptg, IDictionary<int, int> dicNewIndex)
  {
    if (ptg == null)
      throw new ArgumentNullException(nameof (ptg));
    bool flag = false;
    if (FormulaUtil.IndexOf(FormulaUtil.NameCodes, ptg.TokenCode) != -1)
    {
      NamePtg namePtg = (NamePtg) ptg;
      int key = (int) namePtg.ExternNameIndex - 1;
      int num = dicNewIndex.ContainsKey(key) ? dicNewIndex[key] : key;
      if (key != num)
      {
        namePtg.ExternNameIndex = (ushort) (num + 1);
        flag = true;
      }
    }
    else if (FormulaUtil.IndexOf(FormulaUtil.NameXCodes, ptg.TokenCode) != -1)
    {
      NameXPtg nameXptg = (NameXPtg) ptg;
      if (this.m_book.IsLocalReference((int) nameXptg.RefIndex))
      {
        int key = (int) nameXptg.NameIndex - 1;
        int num = dicNewIndex.ContainsKey(key) ? dicNewIndex[key] : key;
        if (num != key)
        {
          nameXptg.NameIndex = (ushort) (num + 1);
          flag = true;
        }
      }
    }
    return flag;
  }

  public bool UpdateNameIndex(Ptg[] arrExpression, IDictionary<int, int> dicNewIndex)
  {
    if (arrExpression == null)
      return false;
    if (dicNewIndex == null)
      throw new ArgumentNullException(nameof (dicNewIndex));
    bool flag = false;
    int index = 0;
    for (int length = arrExpression.Length; index < length; ++index)
      flag |= this.UpdateNameIndex(arrExpression[index], dicNewIndex);
    return flag;
  }

  public bool UpdateNameIndex(Ptg[] arrExpression, int[] arrNewIndex)
  {
    if (arrExpression == null)
      return false;
    if (arrNewIndex == null)
      throw new ArgumentNullException(nameof (arrNewIndex));
    bool flag = false;
    int index = 0;
    for (int length = arrExpression.Length; index < length; ++index)
      flag |= this.UpdateNameIndex(arrExpression[index], arrNewIndex);
    return flag;
  }

  public void SetSeparators(char operandsSeparator, char arrayRowsSeparator)
  {
    string str1 = operandsSeparator.ToString();
    string str2 = arrayRowsSeparator.ToString();
    if (str1 == this.m_strOperandsSeparator && str2 == this.m_strArrayRowSeparator)
      return;
    string[] arrOldKey = new string[2]
    {
      this.m_strOperandsSeparator,
      this.m_strArrayRowSeparator
    };
    string[] arrNewKey = new string[2]{ str1, str2 };
    this.ReplaceInDictionary((IDictionary) this.m_arrAllOperations, arrOldKey, arrNewKey);
    int index = 0;
    for (int length = this.m_arrOperationsWithPriority.Length; index < length; ++index)
      this.ReplaceInDictionary((IDictionary) this.m_arrOperationsWithPriority[index], arrOldKey, arrNewKey);
    this.m_strOperandsSeparator = str1;
    this.m_strArrayRowSeparator = str2;
    this.m_parser.SetSeparators(operandsSeparator, arrayRowsSeparator);
  }

  private void ReplaceInDictionary(IDictionary list, string[] arrOldKey, string[] arrNewKey)
  {
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    if (arrOldKey == null)
      throw new ArgumentNullException(nameof (arrOldKey));
    if (arrNewKey == null)
      throw new ArgumentNullException(nameof (arrNewKey));
    int length = arrOldKey.Length;
    if (length != arrNewKey.Length)
      throw new ArgumentException("arrOldKey and arrNewKey do not correspond each other");
    object[] objArray = new object[length];
    bool[] flagArray = new bool[length];
    for (int index = 0; index < length; ++index)
    {
      string key = arrOldKey[index];
      if (flagArray[index] = list.Contains((object) key))
      {
        objArray[index] = list[(object) key];
        list.Remove((object) key);
      }
    }
    for (int index = 0; index < length; ++index)
    {
      if (flagArray[index])
        list.Add((object) arrNewKey[index], objArray[index]);
    }
  }

  public static void MarkUsedReferences(Ptg[] tokens, bool[] usedItems)
  {
    if (tokens == null)
      return;
    int index = 0;
    for (int length = tokens.Length; index < length; ++index)
    {
      if (tokens[index] is IReference token)
        usedItems[(int) token.RefIndex] = true;
    }
  }

  public static bool UpdateReferenceIndexes(Ptg[] tokens, int[] arrUpdatedIndexes)
  {
    bool flag = false;
    if (tokens != null)
    {
      int index = 0;
      for (int length = tokens.Length; index < length; ++index)
      {
        if (tokens[index] is IReference token)
        {
          int refIndex = (int) token.RefIndex;
          int arrUpdatedIndex = arrUpdatedIndexes[refIndex];
          token.RefIndex = (ushort) arrUpdatedIndex;
          flag = true;
        }
      }
    }
    return flag;
  }

  private Ptg CreateConstantPtg(
    string strFormula,
    IWorksheet sheet,
    Dictionary<string, string> hashWorksheetNames,
    OfficeParseFormulaOptions options)
  {
    return this.CreateConstantPtg(strFormula, sheet, (Dictionary<Type, ReferenceIndexAttribute>) null, 0, hashWorksheetNames, options, 0, 0);
  }

  private Ptg CreateConstantPtg(
    string strFormula,
    IWorksheet sheet,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    Dictionary<string, string> hashWorksheetNames,
    OfficeParseFormulaOptions options,
    int iCellRow,
    int iCellColumn)
  {
    bool bR1C1 = (options & OfficeParseFormulaOptions.UseR1C1) != OfficeParseFormulaOptions.None;
    if (strFormula.Length == 0)
      return FormulaUtil.CreatePtg(FormulaToken.tMissingArgument);
    if (strFormula[0] == '"')
      return FormulaUtil.CreatePtg(FormulaToken.tStringConstant, strFormula.Substring(1, strFormula.Length - 2));
    if (strFormula[0] == '{')
      return FormulaUtil.CreatePtg(ArrayPtg.IndexToCode(FormulaUtil.GetIndex(typeof (ArrayPtg), 2, indexes, i, options)), (object) strFormula, (object) this);
    string strParam1;
    string strParam2;
    string strRow2;
    string strColumn2;
    if (this.IsCellRange(strFormula, bR1C1, out strParam1, out strParam2, out strRow2, out strColumn2))
      return FormulaUtil.CreatePtg(AreaPtg.IndexToCode(FormulaUtil.GetIndex(typeof (AreaPtg), 0, indexes, i, options)), iCellRow, iCellColumn, strParam1, strParam2, strRow2, strColumn2, bR1C1, (IWorkbook) this.m_book);
    if (FormulaUtil.IsCell(strFormula, bR1C1, out strParam1, out strParam2))
      return FormulaUtil.CreatePtg(RefPtg.IndexToCode(FormulaUtil.GetIndex(typeof (RefPtg), 0, indexes, i, options)), iCellRow, iCellColumn, strParam1, strParam2, bR1C1);
    string strSheetName;
    if (this.IsCellRange3D(strFormula, bR1C1, out strSheetName, out strParam1, out strParam2, out strRow2, out strColumn2))
    {
      FormulaToken code = Area3DPtg.IndexToCode(FormulaUtil.GetIndex(typeof (Area3DPtg), 0, indexes, i, options));
      if (hashWorksheetNames != null && hashWorksheetNames.ContainsKey(strSheetName))
        strFormula = strFormula.Replace(strSheetName, hashWorksheetNames[strSheetName]);
      int num = this.m_book.AddSheetReference(strSheetName);
      return FormulaUtil.CreatePtg(code, (object) iCellRow, (object) iCellColumn, (object) num, (object) strParam1, (object) strParam2, (object) strRow2, (object) strColumn2, (object) bR1C1);
    }
    if (FormulaUtil.IsCell3D(strFormula, bR1C1, out strSheetName, out strParam1, out strParam2))
    {
      FormulaToken code = Ref3DPtg.IndexToCode(FormulaUtil.GetIndex(typeof (Ref3DPtg), 0, indexes, i, options));
      if (hashWorksheetNames != null && hashWorksheetNames.ContainsKey(strSheetName))
        strSheetName = hashWorksheetNames[strSheetName];
      int num = this.m_book.AddSheetReference(strSheetName);
      return FormulaUtil.CreatePtg(code, (object) iCellRow, (object) iCellColumn, (object) num, (object) strParam1, (object) strParam2, (object) bR1C1);
    }
    if (FormulaUtil.IsNamedRange(strFormula, (IWorkbook) this.m_book, sheet))
    {
      FormulaToken code = NamePtg.IndexToCode(FormulaUtil.GetIndex(typeof (NamePtg), 0, indexes, i, options));
      if (sheet == null)
        return FormulaUtil.CreatePtg(code, strFormula, (IWorkbook) this.m_book);
      return FormulaUtil.CreatePtg(code, (object) strFormula, (object) this.m_book, (object) sheet);
    }
    double result;
    bool flag = double.TryParse(strFormula, NumberStyles.Integer, (IFormatProvider) null, out result);
    if (flag && result <= (double) ushort.MaxValue && result >= 0.0)
      return FormulaUtil.CreatePtg(FormulaToken.tInteger, strFormula);
    if (!flag)
      flag = double.TryParse(strFormula, NumberStyles.Any, (IFormatProvider) this.m_numberFormat, out result);
    if (flag)
      return FormulaUtil.CreatePtg(FormulaToken.tNumber, (object) result);
    try
    {
      bool.Parse(strFormula);
      return FormulaUtil.CreatePtg(FormulaToken.tBoolean, strFormula);
    }
    catch (FormatException ex)
    {
    }
    if (this.m_book.ThrowOnUnknownNames)
      throw new ArgumentException("Can't parse formula: " + strFormula);
    this.m_book.Names.Add(strFormula);
    return FormulaUtil.CreatePtg(NamePtg.IndexToCode(FormulaUtil.GetIndex(typeof (NamePtg), 0, indexes, i, options)), strFormula, (IWorkbook) this.m_book);
  }

  private static string NormalizeSheetName(string strSheetName)
  {
    if (strSheetName == null)
      return (string) null;
    int length = strSheetName.Length;
    if (length >= 2 && strSheetName[0] == '\'' && '\'' == strSheetName[length - 1])
      strSheetName = strSheetName.Substring(1, length - 2);
    return strSheetName;
  }

  public static Ptg[] ParseExpression(DataProvider provider, int iLength, OfficeVersion version)
  {
    List<Ptg> ptgList = new List<Ptg>();
    int offset = 0;
    while (offset < iLength)
      ptgList.Add(FormulaUtil.CreatePtg(provider, ref offset, version));
    return ptgList.ToArray();
  }

  public static Ptg[] ParseExpression(
    DataProvider provider,
    int offset,
    int iExpressionLength,
    out int finalOffset,
    OfficeVersion version)
  {
    List<Ptg> ptgList = new List<Ptg>();
    int num = offset + iExpressionLength;
    while (offset < num)
      ptgList.Add(FormulaUtil.CreatePtg(provider, ref offset, version));
    finalOffset = offset;
    for (int index = 0; index < ptgList.Count; ++index)
    {
      if (ptgList[index] is IAdditionalData)
        finalOffset = (ptgList[index] as IAdditionalData).ReadArray(provider, finalOffset);
    }
    return ptgList.ToArray();
  }

  public static byte[] PtgArrayToByteArray(Ptg[] tokens, OfficeVersion version)
  {
    return tokens == null ? (byte[]) null : FormulaUtil.PtgArrayToByteArray(tokens, out int _, version);
  }

  public static byte[] PtgArrayToByteArray(
    Ptg[] arrTokens,
    out int formulaLen,
    OfficeVersion version)
  {
    if (arrTokens == null)
      throw new ArgumentNullException(nameof (arrTokens));
    BytesList bytesList = new BytesList(true);
    int length = arrTokens.Length;
    int index1 = 0;
    for (int index2 = length; index1 < index2; ++index1)
      bytesList.AddRange(arrTokens[index1].ToByteArray(version));
    formulaLen = bytesList.Count;
    for (int index3 = 0; index3 < length; ++index3)
    {
      if (arrTokens[index3] is ArrayPtg)
      {
        BytesList listBytes = (arrTokens[index3] as ArrayPtg).GetListBytes();
        bytesList.AddRange(listBytes);
      }
    }
    return bytesList.InnerBuffer;
  }

  public static string GetLeftBinaryOperand(string strFormula, int OpIndex)
  {
    return FormulaUtil.GetOperand(strFormula, OpIndex, FormulaUtil.m_listPlusMinus, true);
  }

  public static int FindCorrespondingBracket(string strFormula, int BracketPos)
  {
    int delta;
    char[] StartBrackets;
    if (FormulaUtil.IndexOf(FormulaUtil.OpenBrackets, strFormula[BracketPos]) != -1)
    {
      delta = 1;
      StartBrackets = FormulaUtil.OpenBrackets;
    }
    else
    {
      if (FormulaUtil.IndexOf(FormulaUtil.CloseBrackets, strFormula[BracketPos]) == -1)
        throw new ArgumentOutOfRangeException("Specified position is not a position of bracket");
      delta = -1;
      StartBrackets = FormulaUtil.CloseBrackets;
    }
    return FormulaUtil.FindCorrespondingBracket(strFormula, BracketPos, StartBrackets, delta);
  }

  public static string GetOperand(
    string strFormula,
    int OpIndex,
    SortedList arrBreakStrings,
    bool IsLeft)
  {
    if (strFormula == null)
      throw new ArgumentNullException(nameof (strFormula));
    if (arrBreakStrings == null)
      throw new ArgumentNullException(nameof (arrBreakStrings));
    char[] array1 = IsLeft ? FormulaUtil.CloseBrackets : FormulaUtil.OpenBrackets;
    char[] array2 = IsLeft ? FormulaUtil.OpenBrackets : FormulaUtil.CloseBrackets;
    int num1 = IsLeft ? -1 : 1;
    int num2 = OpIndex + num1;
    if (!IsLeft && strFormula[num2] == '#')
    {
      string errorOperand = FormulaUtil.GetErrorOperand(strFormula, num2);
      num2 += errorOperand.Length;
      if (num2 >= strFormula.Length || FormulaUtil.IndexOf(strFormula, num2, arrBreakStrings) != -1 || FormulaUtil.IndexOf(array2, strFormula[num2]) != -1)
        return errorOperand;
    }
    int length = strFormula.Length;
    while (num2 >= 0 && num2 < length && FormulaUtil.IndexOf(FormulaUtil.PlusMinusArray, strFormula[num2].ToString()) != -1)
      num2 += num1;
    for (; num2 >= 0 && num2 < length; num2 += num1)
    {
      if (FormulaUtil.IndexOf(array1, strFormula[num2]) != -1)
        num2 = FormulaUtil.FindCorrespondingBracket(strFormula, num2);
      else if (FormulaUtil.IndexOf(array2, strFormula[num2]) != -1 && FormulaUtil.FindCorrespondingBracket(strFormula, num2) != -1 || FormulaUtil.IndexOf(strFormula, num2, arrBreakStrings) != -1)
        return IsLeft ? strFormula.Substring(num2 + 1, OpIndex - num2 - 1) : strFormula.Substring(OpIndex + 1, num2 - OpIndex - 1);
    }
    return !IsLeft ? strFormula.Substring(OpIndex + 1) : strFormula.Substring(0, OpIndex);
  }

  [CLSCompliant(false)]
  public static void RegisterFunction(
    string functionName,
    ExcelFunction index,
    ReferenceIndexAttribute[] paramIndexes)
  {
    FormulaUtil.RegisterFunction(functionName, index, paramIndexes, -1);
  }

  [CLSCompliant(false)]
  public static void RegisterFunction(
    string functionName,
    ExcelFunction index,
    ReferenceIndexAttribute[] paramIndexes,
    int paramCount)
  {
    Dictionary<Type, ReferenceIndexAttribute> dictionary = (Dictionary<Type, ReferenceIndexAttribute>) null;
    if (paramIndexes != null && paramIndexes.Length != 0)
      dictionary = new Dictionary<Type, ReferenceIndexAttribute>();
    for (int index1 = 0; index1 < paramIndexes.Length; ++index1)
      dictionary.Add(paramIndexes[index1].TargetType, paramIndexes[index1]);
    if (dictionary == null)
    {
      dictionary = new Dictionary<Type, ReferenceIndexAttribute>(1);
      dictionary.Add(typeof (RefPtg), new ReferenceIndexAttribute(2));
    }
    FormulaUtil.FunctionIdToIndex.Add(index, dictionary);
    FormulaUtil.FunctionIdToAlias.Add(index, functionName);
    FormulaUtil.FunctionAliasToId.Add(functionName, index);
    if (paramCount == -1)
      return;
    FormulaUtil.FunctionIdToParamCount.Add(index, paramCount);
  }

  [CLSCompliant(false)]
  public static void RegisterFunction(string functionName, ExcelFunction index, int paramCount)
  {
    FormulaUtil.RegisterFunction(functionName, index, (ReferenceIndexAttribute[]) null, paramCount);
  }

  [CLSCompliant(false)]
  public static void RegisterFunction(string functionName, ExcelFunction index)
  {
    FormulaUtil.RegisterFunction(functionName, index, -1);
  }

  public static void RaiseFormulaEvaluation(object sender, EvaluateEventArgs e)
  {
    if (FormulaUtil.FormulaEvaluator == null)
      return;
    FormulaUtil.FormulaEvaluator(sender, e);
  }

  public static void RegisterTokenClass(Type type)
  {
    if (type == (Type) null)
      throw new ArgumentNullException(nameof (type));
    TokenAttribute[] tokenAttributeArray = type.IsSubclassOf(typeof (Ptg)) ? (TokenAttribute[]) type.GetCustomAttributes(typeof (TokenAttribute), false) : throw new ArgumentException("class must be derived from Ptg class", nameof (type));
    if (tokenAttributeArray.Length == 0)
      return;
    FormulaUtil.TokenConstructor tokenConstructor = new FormulaUtil.TokenConstructor(type);
    Ptg ptg = tokenConstructor.CreatePtg();
    for (int index = 0; index < tokenAttributeArray.Length; ++index)
    {
      FormulaToken formulaType = tokenAttributeArray[index].FormulaType;
      FormulaUtil.TokenCodeToConstructor.Add(tokenAttributeArray[index].FormulaType, tokenConstructor);
      FormulaUtil.s_hashTokenCodeToPtg.Add(formulaType, ptg);
    }
  }

  [CLSCompliant(false)]
  public static void RegisterAdditionalAlias(string aliasName, ExcelFunction functionIndex)
  {
    switch (aliasName)
    {
      case null:
        throw new ArgumentNullException(nameof (aliasName));
      case "":
        throw new ArgumentException("aliasName - string cannot be empty");
      default:
        if (FormulaUtil.FunctionAliasToId.ContainsKey(aliasName))
          throw new ArgumentOutOfRangeException(nameof (aliasName), "Alias name already exists.");
        FormulaUtil.FunctionAliasToId.Add(aliasName, functionIndex);
        break;
    }
  }

  public static void UpdateNameIndex(Ptg ptg, int iOldIndex, int iNewIndex)
  {
    if (ptg == null)
      throw new ArgumentNullException(nameof (ptg));
    if (FormulaUtil.IndexOf(FormulaUtil.NameCodes, ptg.TokenCode) != -1)
    {
      NamePtg namePtg = (NamePtg) ptg;
      if ((int) namePtg.ExternNameIndex - 1 != iOldIndex)
        return;
      namePtg.ExternNameIndex = (ushort) (iNewIndex + 1);
    }
    else
    {
      if (FormulaUtil.IndexOf(FormulaUtil.NameXCodes, ptg.TokenCode) == -1)
        return;
      NameXPtg nameXptg = (NameXPtg) ptg;
      if ((int) nameXptg.NameIndex - 1 != iOldIndex)
        return;
      nameXptg.NameIndex = (ushort) (iNewIndex + 1);
    }
  }

  public static int IndexOf(FormulaToken[] array, FormulaToken value)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    int index = 0;
    for (int length = array.Length; index < length; ++index)
    {
      if (array[index] == value)
        return index;
    }
    return -1;
  }

  public static int IndexOf(string[] array, string value)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    int index = 0;
    for (int length = array.Length; index < length; ++index)
    {
      if (array[index] == value)
        return index;
    }
    return -1;
  }

  [CLSCompliant(false)]
  public static int IndexOf(ExcelFunction[] array, ExcelFunction value)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    int index = 0;
    for (int length = array.Length; index < length; ++index)
    {
      if (array[index] == value)
        return index;
    }
    return -1;
  }

  private static int IndexOf(char[] array, char value)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    int index = 0;
    for (int length = array.Length; index < length; ++index)
    {
      char ch = array[index];
      if ((int) value == (int) ch)
        return index;
    }
    return -1;
  }

  private static int IndexOf(string strFormula, int index, string[] arrBreakStrings)
  {
    if (strFormula == null)
      throw new ArgumentNullException(nameof (strFormula));
    int num = arrBreakStrings != null ? arrBreakStrings.Length : throw new ArgumentNullException(nameof (arrBreakStrings));
    if (num == 0)
      return -1;
    char chFirst = strFormula[index];
    int lowerBound = FormulaUtil.GetLowerBound(arrBreakStrings, chFirst);
    if (lowerBound < 0)
      return -1;
    for (int index1 = lowerBound; index1 < num; ++index1)
    {
      string arrBreakString = arrBreakStrings[index1];
      int length = arrBreakString.Length;
      if (arrBreakString == null)
        throw new ArgumentNullException();
      if (length == 0)
        throw new ArgumentException("String can't be empty");
      if ((int) arrBreakString[0] != (int) chFirst)
        return -1;
      if (length == 1 || string.Compare(strFormula, index + 1, arrBreakString, 1, length - 1) == 0)
        return index1;
    }
    return -1;
  }

  private static int IndexOf(string strFormula, int index, SortedList arrBreakStrings)
  {
    if (strFormula == null)
      throw new ArgumentNullException(nameof (strFormula));
    if (arrBreakStrings == null)
      throw new ArgumentNullException(nameof (arrBreakStrings));
    if (arrBreakStrings.Count == 0)
      return -1;
    char chFirst = strFormula[index];
    int upperBound = FormulaUtil.GetUpperBound(arrBreakStrings, chFirst);
    if (upperBound < 0)
      return -1;
    for (int index1 = upperBound; index1 >= 0; --index1)
    {
      string key = (string) arrBreakStrings.GetKey(index1);
      int length = key.Length;
      if (key == null)
        throw new ArgumentNullException();
      if (length == 0)
        throw new ArgumentException("String can't be empty");
      if ((int) key[0] != (int) chFirst)
        return -1;
      if (length == 1 || string.Compare(strFormula, index + 1, key, 1, length - 1) == 0)
        return index1;
    }
    return -1;
  }

  private static int GetLowerBound(string[] arrStringValues, char chFirst)
  {
    if (arrStringValues == null)
      throw new ArgumentNullException(nameof (arrStringValues));
    int lowerBound = 0;
    int index1 = arrStringValues.Length - 1;
    while (index1 != lowerBound)
    {
      int index2 = (index1 + lowerBound) / 2;
      char ch = (arrStringValues[index2] ?? throw new ArgumentNullException("String in the array can't be null."))[0];
      if ((int) ch >= (int) chFirst)
      {
        if (index1 != index2)
          index1 = index2;
        else
          break;
      }
      else if ((int) ch < (int) chFirst)
      {
        if (lowerBound != index2)
          lowerBound = index2;
        else
          break;
      }
    }
    string arrStringValue1 = arrStringValues[lowerBound];
    if (arrStringValue1 == null)
      throw new ArgumentNullException();
    if ((int) arrStringValue1[0] == (int) chFirst)
      return lowerBound;
    string arrStringValue2 = arrStringValues[index1];
    if (arrStringValue2 == null)
      throw new ArgumentNullException();
    return (int) arrStringValue2[0] == (int) chFirst ? index1 : -1;
  }

  private static int GetLowerBound(SortedList arrStringValues, char chFirst)
  {
    if (arrStringValues == null)
      throw new ArgumentNullException(nameof (arrStringValues));
    int index1 = 0;
    int index2 = arrStringValues.Count - 1;
    while (index2 != index1)
    {
      int index3 = (index2 + index1) / 2;
      char ch = ((string) arrStringValues.GetKey(index3) ?? throw new ArgumentNullException("String in the array can't be null."))[0];
      if ((int) ch >= (int) chFirst)
      {
        if (index2 != index3)
          index2 = index3;
        else
          break;
      }
      else if ((int) ch < (int) chFirst)
      {
        if (index1 != index3)
          index1 = index3;
        else
          break;
      }
    }
    string key1 = (string) arrStringValues.GetKey(index1);
    if (key1 == null)
      throw new ArgumentNullException();
    if ((int) key1[0] == (int) chFirst)
      return index1;
    string key2 = (string) arrStringValues.GetKey(index2);
    if (key2 == null)
      throw new ArgumentNullException();
    return (int) key2[0] == (int) chFirst ? index2 : -1;
  }

  private static int GetUpperBound(SortedList arrStringValues, char chFirst)
  {
    if (arrStringValues == null)
      throw new ArgumentNullException(nameof (arrStringValues));
    int index1 = 0;
    int index2 = arrStringValues.Count - 1;
    while (index2 != index1)
    {
      int index3 = (index2 + index1) / 2;
      char ch = ((string) arrStringValues.GetKey(index3) ?? throw new ArgumentNullException("String in the array can't be null."))[0];
      if ((int) ch > (int) chFirst)
      {
        if (index2 != index3)
          index2 = index3;
        else
          break;
      }
      else if ((int) ch <= (int) chFirst)
      {
        if (index1 != index3)
          index1 = index3;
        else
          break;
      }
    }
    string key1 = (string) arrStringValues.GetKey(index1);
    if (key1 == null)
      throw new ArgumentNullException();
    if ((int) key1[0] == (int) chFirst)
      return index1;
    string key2 = (string) arrStringValues.GetKey(index2);
    if (key2 == null)
      throw new ArgumentNullException();
    return (int) key2[0] == (int) chFirst ? index2 : -1;
  }

  [CLSCompliant(false)]
  public static Ptg[] ConvertSharedFormulaTokens(
    SharedFormulaRecord shared,
    IWorkbook book,
    int iRow,
    int iColumn)
  {
    Ptg[] ptgArray1 = shared != null ? shared.Formula : throw new ArgumentNullException(nameof (shared));
    int length = ptgArray1.Length;
    Ptg[] ptgArray2 = new Ptg[length];
    for (int index = 0; index < length; ++index)
      ptgArray2[index] = ptgArray1[index].ConvertSharedToken(book, iRow, iColumn);
    return ptgArray2;
  }

  public Ptg[] UpdateFormula(
    Ptg[] arrPtgs,
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect,
    int iRow,
    int iColumn)
  {
    int index = 0;
    for (int length = arrPtgs.Length; index < length; ++index)
    {
      Ptg arrPtg = arrPtgs[index];
      arrPtgs[index] = arrPtg.Offset(iCurIndex, iRow - 1, iColumn - 1, iSourceIndex, sourceRect, iDestIndex, destRect, out bool _, this.m_book);
    }
    return arrPtgs;
  }

  public Ptg[] UpdateFormula(Ptg[] arrPtgs, int iRowDelta, int iColumnDelta)
  {
    int index = 0;
    for (int length = arrPtgs.Length; index < length; ++index)
    {
      Ptg arrPtg = arrPtgs[index];
      arrPtgs[index] = arrPtg.Offset(iRowDelta, iColumnDelta, this.m_book);
    }
    return arrPtgs;
  }

  public static void PushOperandToStack(Stack<object> operands, string operand)
  {
    if (operands == null)
      throw new ArgumentNullException(nameof (operands));
    string str = operand != null ? operand : throw new ArgumentNullException(nameof (operand));
    if (operands.Count > 0 && operands.Peek() is AttrPtg attrPtg)
    {
      operands.Pop();
      str = attrPtg.ToString() + operand;
    }
    operands.Push((object) str);
  }

  private Ptg[] CreateFunction(
    string strFormula,
    int bracketIndex,
    IWorkbook parent,
    IWorksheet sheet,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int index,
    Dictionary<string, string> hashWorksheetNames,
    OfficeParseFormulaOptions options,
    int iCellRow,
    int iCellColumn)
  {
    List<Ptg> ptgList = new List<Ptg>();
    string strFunctionName = strFormula.Substring(0, bracketIndex);
    string upper = strFunctionName.ToUpper();
    int index1 = FormulaUtil.GetIndex(typeof (FunctionVarPtg), 1, indexes, index, options);
    if (upper == "IF")
      return this.CreateIfFunction(index1, strFormula, bracketIndex, parent, sheet, hashWorksheetNames, options, iCellRow, iCellColumn);
    if (FormulaUtil.FunctionAliasToId.ContainsKey(upper))
    {
      ExcelFunction excelFunction1 = FormulaUtil.FunctionAliasToId[upper];
      ExcelFunction excelFunction2 = FormulaUtil.FunctionAliasToId[upper];
      OperationPtg operationPtg = !FormulaUtil.FunctionIdToParamCount.ContainsKey(excelFunction2) ? (OperationPtg) FormulaUtil.CreatePtg(FunctionVarPtg.IndexToCode(FormulaUtil.GetIndex(typeof (FunctionVarPtg), 1, indexes, index, options)), excelFunction2) : (OperationPtg) FormulaUtil.CreatePtg(FunctionPtg.IndexToCode(FormulaUtil.GetIndex(typeof (FunctionPtg), 1, indexes, index, options)), excelFunction2);
      if (FormulaUtil.IndexOf(FormulaUtil.SemiVolatileFunctions, excelFunction2) != -1)
        ptgList.Add(FormulaUtil.CreatePtg(FormulaToken.tAttr, (ushort) 1, (ushort) 0));
      string[] operands = operationPtg.GetOperands(strFormula, ref bracketIndex, this);
      int i = 0;
      Dictionary<Type, ReferenceIndexAttribute> indexes1 = FormulaUtil.FunctionIdToIndex[excelFunction2];
      OfficeParseFormulaOptions parseFormulaOptions = options;
      if ((options & OfficeParseFormulaOptions.RootLevel) != OfficeParseFormulaOptions.None)
        --parseFormulaOptions;
      OfficeParseFormulaOptions options1 = parseFormulaOptions | OfficeParseFormulaOptions.ParseOperand;
      int index2 = 0;
      for (int length = operands.Length; index2 < length; ++index2)
      {
        string operand = operands[index2];
        if (indexes1 != null)
          ptgList.AddRange((IEnumerable<Ptg>) this.ParseOperandString(operand, parent, sheet, indexes1, i, hashWorksheetNames, options1, iCellRow, iCellColumn));
        else
          ptgList.AddRange((IEnumerable<Ptg>) this.ParseOperandString(operand, parent, sheet, (Dictionary<Type, ReferenceIndexAttribute>) null, i, hashWorksheetNames, options1, iCellRow, iCellColumn));
        ++i;
      }
      ptgList.Add((Ptg) operationPtg);
      return ptgList.ToArray();
    }
    int iBookIndex;
    int iNameIndex;
    if (!FormulaUtil.IsCustomFunction(strFunctionName, parent, out iBookIndex, out iNameIndex))
      throw new ArgumentException($"Unknown function name: '{upper}' formula: {strFormula}");
    return iNameIndex != -1 ? this.CreateCustomFunction(index1, strFormula, bracketIndex, iBookIndex, iNameIndex, parent, sheet, hashWorksheetNames, options, iCellRow, iCellColumn) : this.CreateCustomFunction(index1, strFormula, bracketIndex, parent, sheet, hashWorksheetNames, options, iCellRow, iCellColumn);
  }

  internal static bool IsCustomFunction(
    string strFunctionName,
    IWorkbook book,
    out int iBookIndex,
    out int iNameIndex)
  {
    switch (strFunctionName)
    {
      case null:
        throw new ArgumentNullException(nameof (strFunctionName));
      case "":
        throw new ArgumentException("strFunctionName - string cannot be empty");
      default:
        iBookIndex = -1;
        iNameIndex = -1;
        if (book == null)
          return false;
        WorkbookImpl workbookImpl = (WorkbookImpl) book;
        Match m = FormulaUtil.AddInFunctionRegEx.Match(strFunctionName);
        bool flag = !m.Success || !(m.Value == strFunctionName) ? FormulaUtil.IsLocalCustomFunction(workbookImpl, strFunctionName, ref iNameIndex) || workbookImpl.ExternWorkbooks.ContainsExternName(strFunctionName, ref iBookIndex, ref iNameIndex) : FormulaUtil.IsCustomFunction(workbookImpl, m, ref iBookIndex, ref iNameIndex);
        if (!flag && !workbookImpl.ThrowOnUnknownNames)
        {
          IName name = workbookImpl.Names[strFunctionName] ?? workbookImpl.Names.Add(strFunctionName);
          iNameIndex = (name as NameImpl).Index;
          flag = true;
        }
        return flag;
    }
  }

  private static bool IsLocalCustomFunction(
    WorkbookImpl book,
    string strFunctionName,
    ref int iNameIndex)
  {
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    switch (strFunctionName)
    {
      case null:
        throw new ArgumentNullException(nameof (strFunctionName));
      case "":
        throw new ArgumentException("strFunctionName - string cannot be empty");
      default:
        if (!(book.InnerNamesColection[strFunctionName] is NameImpl nameImpl))
          return false;
        iNameIndex = nameImpl.Index;
        return nameImpl.IsFunction;
    }
  }

  private static bool IsCustomFunction(
    WorkbookImpl workbook,
    Match m,
    ref int iBookIndex,
    ref int iNameIndex)
  {
    if (m == null)
      throw new ArgumentNullException(nameof (m));
    string str1 = m.Groups["Path"].Value;
    string strShortName = m.Groups["BookName"].Value;
    string str2 = m.Groups["SheetName"].Value;
    string strName = m.Groups["RangeName"].Value;
    if (strShortName == null || strShortName.Length == 0)
      strShortName = str2;
    int length = strShortName.Length;
    if (strShortName[0] == '[' && strShortName[length - 1] == ']')
      strShortName = strShortName.Substring(1, length - 2);
    ExternWorkbookImpl externWorkbookImpl;
    if (str1.Length > 0)
    {
      externWorkbookImpl = workbook.ExternWorkbooks[str1 + strShortName];
      if (externWorkbookImpl == null)
        return false;
    }
    else
    {
      externWorkbookImpl = workbook.ExternWorkbooks.GetBookByShortName(strShortName);
      if (externWorkbookImpl == null)
        return false;
    }
    iBookIndex = externWorkbookImpl.Index;
    iNameIndex = externWorkbookImpl.ExternNames.GetNameIndex(strName);
    return iNameIndex >= 0;
  }

  private Ptg[] CreateIfFunction(
    int iRefIndex,
    string strFormula,
    int bracketIndex,
    IWorkbook parent,
    IWorksheet sheet,
    Dictionary<string, string> hashWorksheetNames,
    OfficeParseFormulaOptions options,
    int iCellRow,
    int iCellColumn)
  {
    List<Ptg> ptgList = new List<Ptg>();
    ExcelFunction excelFunction = ExcelFunction.IF;
    OperationPtg ptg = (OperationPtg) FormulaUtil.CreatePtg(FunctionVarPtg.IndexToCode(iRefIndex), excelFunction);
    if (FormulaUtil.IndexOf(FormulaUtil.SemiVolatileFunctions, excelFunction) != -1)
      ptgList.Add(FormulaUtil.CreatePtg(FormulaToken.tAttr, (ushort) 1, (ushort) 0));
    string[] operands = ptg.GetOperands(strFormula, ref bracketIndex, this);
    if (operands.Length > 3 || operands.Length < 2)
      throw new ArgumentOutOfRangeException("Argument count must be 2 or 3", strFormula);
    int i = 0;
    Dictionary<Type, ReferenceIndexAttribute> indexes = FormulaUtil.FunctionIdToIndex[excelFunction];
    List<Ptg[]> ptgArrayList = new List<Ptg[]>(4);
    OfficeParseFormulaOptions options1 = options;
    if ((options & OfficeParseFormulaOptions.RootLevel) != OfficeParseFormulaOptions.None)
      --options1;
    int index1 = 0;
    for (int length = operands.Length; index1 < length; ++index1)
    {
      string operand = operands[index1];
      if (indexes != null)
        ptgArrayList.Add(this.ParseOperandString(operand, parent, sheet, indexes, i, hashWorksheetNames, options1, iCellRow, iCellColumn));
      else
        ptgArrayList.Add(this.ParseOperandString(operand, parent, sheet, (Dictionary<Type, ReferenceIndexAttribute>) null, i, hashWorksheetNames, options1, iCellRow, iCellColumn));
      ++i;
    }
    ptgList.AddRange((IEnumerable<Ptg>) ptgArrayList[0]);
    int num1 = 0;
    int num2 = 0;
    Ptg[] collection1 = ptgArrayList[1];
    Ptg[] collection2 = (Ptg[]) null;
    OfficeVersion version = ((WorkbookImpl) parent).Version;
    int index2 = 0;
    for (int length = collection1.Length; index2 < length; ++index2)
      num1 += collection1[index2].GetSize(version);
    if (operands.Length == 3)
    {
      collection2 = ptgArrayList[2];
      int index3 = 0;
      for (int length = collection2.Length; index3 < length; ++index3)
        num2 += collection2[index3].GetSize(version);
      num2 += 4;
    }
    ptgList.Add(FormulaUtil.CreatePtg(FormulaToken.tAttr, (object) 2, (object) (num1 + 4)));
    ptgList.AddRange((IEnumerable<Ptg>) collection1);
    int num3 = (options & OfficeParseFormulaOptions.InArray) == OfficeParseFormulaOptions.None ? 8 : 0;
    ptgList.Add(FormulaUtil.CreatePtg(FormulaToken.tAttr, (object) num3, (object) (num2 + 3)));
    if (collection2 != null)
    {
      ptgList.AddRange((IEnumerable<Ptg>) collection2);
      ptgList.Add(FormulaUtil.CreatePtg(FormulaToken.tAttr, (object) num3, (object) 3));
    }
    ptgList.Add((Ptg) ptg);
    return ptgList.ToArray();
  }

  private Ptg[] CreateCustomFunction(
    int iRefIndex,
    string strFormula,
    int bracketIndex,
    IWorkbook parent,
    IWorksheet sheet,
    Dictionary<string, string> hashWorksheetNames,
    OfficeParseFormulaOptions options,
    int iCellRow,
    int iCellColumn)
  {
    string leftUnaryOperand = this.GetLeftUnaryOperand(strFormula, bracketIndex);
    int iRefIndex1;
    int nameIndexes = ((WorkbookImpl) parent).ExternWorkbooks.GetNameIndexes(leftUnaryOperand, out iRefIndex1);
    return this.CreateCustomFunction(iRefIndex, strFormula, bracketIndex, nameIndexes, iRefIndex1, parent, sheet, hashWorksheetNames, options, iCellRow, iCellColumn);
  }

  private Ptg[] CreateCustomFunction(
    int iRefIndex,
    string strFormula,
    int bracketIndex,
    int iBookIndex,
    int iNameIndex,
    IWorkbook parent,
    IWorksheet sheet,
    Dictionary<string, string> hashWorksheetNames,
    OfficeParseFormulaOptions options,
    int iCellRow,
    int iCellColumn)
  {
    List<Ptg> ptgList = new List<Ptg>();
    ExcelFunction excelFunction = ExcelFunction.CustomFunction;
    OperationPtg ptg1 = (OperationPtg) FormulaUtil.CreatePtg(FunctionVarPtg.IndexToCode(iRefIndex), excelFunction);
    if (FormulaUtil.IndexOf(FormulaUtil.SemiVolatileFunctions, excelFunction) != -1)
      ptgList.Add(FormulaUtil.CreatePtg(FormulaToken.tAttr, (ushort) 1, (ushort) 0));
    string[] operands = ptg1.GetOperands(strFormula, ref bracketIndex, this);
    int i = 0;
    Dictionary<Type, ReferenceIndexAttribute> indexes = FormulaUtil.FunctionIdToIndex[excelFunction];
    OfficeParseFormulaOptions options1 = options;
    if ((options & OfficeParseFormulaOptions.RootLevel) != OfficeParseFormulaOptions.None)
      --options1;
    Ptg ptg2;
    if (iBookIndex == -1)
      ptg2 = FormulaUtil.CreatePtg(FormulaToken.tName1, (object) iNameIndex);
    else
      ptg2 = FormulaUtil.CreatePtg(FormulaToken.tNameX1, (object) iBookIndex, (object) iNameIndex);
    Ptg ptg3 = ptg2;
    ptgList.Add(ptg3);
    int index = 0;
    for (int length = operands.Length; index < length; ++index)
    {
      string operand = operands[index];
      if (indexes != null)
        ptgList.AddRange((IEnumerable<Ptg>) this.ParseOperandString(operand, parent, sheet, indexes, i, hashWorksheetNames, options1, iCellRow, iCellColumn));
      else
        ptgList.AddRange((IEnumerable<Ptg>) this.ParseOperandString(operand, parent, sheet, (Dictionary<Type, ReferenceIndexAttribute>) null, i, hashWorksheetNames, options1, iCellRow, iCellColumn));
      ++i;
    }
    ptgList.Add((Ptg) ptg1);
    return ptgList.ToArray();
  }

  public static Ptg CreateError(string strFormula, int errorIndex)
  {
    string errorOperand = FormulaUtil.GetErrorOperand(strFormula, errorIndex);
    return (Ptg) FormulaUtil.ErrorNameToConstructor[errorOperand].Invoke(new object[1]
    {
      (object) strFormula
    });
  }

  private Ptg[] ParseOperandString(
    string operand,
    IWorkbook parent,
    IWorksheet sheet,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    Dictionary<string, string> hashWorksheetNames,
    OfficeParseFormulaOptions options,
    int iCellRow,
    int iCellColumn)
  {
    if (operand.Length == 0)
      return new Ptg[1]
      {
        FormulaUtil.CreatePtg(FormulaToken.tMissingArgument, operand)
      };
    try
    {
      options |= OfficeParseFormulaOptions.ParseOperand;
      return this.ParseString(operand, sheet, indexes, i, hashWorksheetNames, options, iCellRow, iCellColumn);
    }
    catch (Exception ex)
    {
      throw;
    }
  }

  private static OperationPtg CreateUnaryOperation(char OperationSymbol)
  {
    string str = OperationSymbol.ToString();
    return OperationSymbol == '(' ? (OperationPtg) FormulaUtil.CreatePtg(FormulaToken.tParentheses, str) : (OperationPtg) FormulaUtil.CreatePtg(UnaryOperationPtg.GetTokenId(str), str);
  }

  private OperationPtg CreateOperation(string strFormula)
  {
    return (OperationPtg) FormulaUtil.CreatePtg(strFormula == this.m_strOperandsSeparator ? FormulaToken.tCellRangeList : BinaryOperationPtg.GetTokenId(strFormula), strFormula);
  }

  public static bool IsCell(
    string strFormula,
    bool bR1C1,
    out string strRow,
    out string strColumn)
  {
    Match match = (bR1C1 ? FormulaUtil.CellR1C1Regex : FormulaUtil.CellRegex).Match(strFormula);
    bool flag = match.Success && match.Value == strFormula;
    if (flag)
    {
      strRow = match.Groups["Row1"].Value;
      strColumn = match.Groups["Column1"].Value;
    }
    else
    {
      strRow = (string) null;
      strColumn = (string) null;
    }
    return flag;
  }

  internal static bool IsR1C1(string strFormula)
  {
    Match match = FormulaUtil.CellR1C1Regex.Match(strFormula);
    return match.Success && match.Value == strFormula;
  }

  public bool IsCellRange(
    string strFormula,
    bool bR1C1,
    out string strRow1,
    out string strColumn1,
    out string strRow2,
    out string strColumn2)
  {
    strColumn1 = (string) null;
    strColumn2 = (string) null;
    strRow1 = (string) null;
    strRow2 = (string) null;
    Match m1 = (bR1C1 ? FormulaUtil.CellRangeR1C1Regex : FormulaUtil.CellRangeRegex).Match(strFormula);
    bool flag = FormulaUtil.IsSuccess(m1, strFormula);
    if (flag)
    {
      strColumn1 = m1.Groups["Column1"].Value;
      strColumn2 = m1.Groups["Column2"].Value;
      strRow1 = m1.Groups["Row1"].Value;
      strRow2 = m1.Groups["Row2"].Value;
    }
    else if (bR1C1)
    {
      flag = FormulaUtil.IsSuccess(FormulaUtil.CellRangeR1C1ShortRegex.Match(strFormula), strFormula);
      if (flag)
      {
        if (strFormula[0].ToString() == "R")
          strRow2 = strRow1 = strFormula;
        else
          strColumn1 = strColumn2 = strFormula;
      }
      else
      {
        Match m2 = FormulaUtil.FullRowRangeR1C1Regex.Match(strFormula);
        flag = FormulaUtil.IsSuccess(m2, strFormula);
        if (flag)
        {
          strColumn1 = "C1";
          strColumn2 = "C" + this.m_book.MaxColumnCount.ToString();
          strRow1 = m2.Groups["Row1"].Value;
          strRow2 = m2.Groups["Row2"].Value;
        }
        else
        {
          Match m3 = FormulaUtil.FullColumnRangeR1C1Regex.Match(strFormula);
          flag = FormulaUtil.IsSuccess(m3, strFormula);
          if (flag)
          {
            strColumn1 = m3.Groups["Column1"].Value;
            strColumn2 = m3.Groups["Column2"].Value;
            strRow1 = "R1";
            strRow2 = "R" + this.m_book.MaxRowCount.ToString();
          }
        }
      }
    }
    else
    {
      Match m4 = FormulaUtil.FullRowRangeRegex.Match(strFormula);
      flag = FormulaUtil.IsSuccess(m4, strFormula);
      if (flag)
      {
        strColumn1 = "$A";
        strColumn2 = "$" + RangeImpl.GetColumnName(this.m_book.MaxColumnCount);
        strRow1 = m4.Groups["Row1"].Value;
        strRow2 = m4.Groups["Row2"].Value;
      }
      else
      {
        Match m5 = FormulaUtil.FullColumnRangeRegex.Match(strFormula);
        flag = FormulaUtil.IsSuccess(m5, strFormula);
        if (flag)
        {
          strColumn1 = m5.Groups["Column1"].Value;
          strColumn2 = m5.Groups["Column2"].Value;
          strRow1 = "$1";
          strRow2 = "$" + this.m_book.MaxRowCount.ToString();
        }
      }
    }
    return flag;
  }

  private static bool IsSuccess(Match m, string strFormula)
  {
    return m.Success && m.Index == 0 && m.Length == strFormula.Length;
  }

  public static bool IsCell3D(
    string strFormula,
    bool bR1C1,
    out string strSheetName,
    out string strRow,
    out string strColumn)
  {
    Match match = (bR1C1 ? FormulaUtil.CellR1C13DRegex : FormulaUtil.Cell3DRegex).Match(strFormula);
    if (match.Success && match.Index == 0 && match.Length == strFormula.Length)
    {
      strSheetName = match.Groups["SheetName"].Value;
      strSheetName = FormulaUtil.NormalizeSheetName(strSheetName);
      strRow = match.Groups["Row1"].Value;
      strColumn = match.Groups["Column1"].Value;
      return true;
    }
    strSheetName = (string) null;
    strRow = (string) null;
    strColumn = (string) null;
    return false;
  }

  public bool IsCellRange3D(
    string strFormula,
    bool bR1C1,
    out string strSheetName,
    out string strRow1,
    out string strColumn1,
    out string strRow2,
    out string strColumn2)
  {
    strSheetName = (string) null;
    strRow1 = (string) null;
    strColumn1 = (string) null;
    strRow2 = (string) null;
    strColumn2 = (string) null;
    Match m1 = (bR1C1 ? FormulaUtil.CellRangeR1C13DRegex : FormulaUtil.CellRange3DRegex).Match(strFormula);
    bool flag1 = m1.Success && m1.Index == 0 && m1.Length == strFormula.Length;
    if (!flag1)
    {
      m1 = (bR1C1 ? FormulaUtil.CellRangeR1C13DRegex2 : FormulaUtil.CellRange3DRegex2).Match(strFormula);
      flag1 = m1.Success && m1.Index == 0 && m1.Length == strFormula.Length;
    }
    if (!flag1)
    {
      Match m2 = FormulaUtil.Full3DRowRangeRegex.Match(strFormula);
      bool flag2 = FormulaUtil.IsSuccess(m2, strFormula);
      if (flag2)
      {
        strSheetName = m2.Groups["SheetName"].Value;
        strColumn1 = "$A";
        strColumn2 = "$" + RangeImpl.GetColumnName(this.m_book.MaxColumnCount);
        strRow1 = m2.Groups["Row1"].Value;
        strRow2 = m2.Groups["Row2"].Value;
        strSheetName = FormulaUtil.NormalizeSheetName(strSheetName);
        return flag2;
      }
      m1 = FormulaUtil.Full3DColumnRangeRegex.Match(strFormula);
      flag1 = FormulaUtil.IsSuccess(m1, strFormula);
      if (flag1)
      {
        strSheetName = m1.Groups["SheetName"].Value;
        strColumn1 = m1.Groups["Column1"].Value;
        strColumn2 = m1.Groups["Column2"].Value;
        strRow1 = "$1";
        strRow2 = "$" + this.m_book.MaxRowCount.ToString();
        strSheetName = FormulaUtil.NormalizeSheetName(strSheetName);
        return flag1;
      }
    }
    if (!flag1 && bR1C1)
    {
      Match m3 = FormulaUtil.CellRangeR1C13DShortRegex.Match(strFormula);
      flag1 = FormulaUtil.IsSuccess(m3, strFormula);
      if (flag1)
      {
        strSheetName = m3.Groups["SheetName"].Value;
        if (strFormula[strSheetName.Length + 1] == 'R')
          strRow2 = strRow1 = strFormula.Substring(strSheetName.Length + 1);
        else
          strColumn1 = strColumn2 = strFormula.Substring(strSheetName.Length + 1);
      }
    }
    else if (flag1)
    {
      strSheetName = m1.Groups["SheetName"].Value;
      strRow1 = m1.Groups["Row1"].Value;
      strColumn1 = m1.Groups["Column1"].Value;
      strRow2 = m1.Groups["Row2"].Value;
      strColumn2 = m1.Groups["Column2"].Value;
    }
    strSheetName = FormulaUtil.NormalizeSheetName(strSheetName);
    return flag1;
  }

  private static bool IsErrorString(string strFormula, int errorIndex)
  {
    foreach (string key in FormulaUtil.ErrorNameToConstructor.Keys)
    {
      if (string.Compare(strFormula, errorIndex, key, 0, key.Length) == 0)
        return true;
    }
    return false;
  }

  private static bool IsNamedRange(string strFormula, IWorkbook parent, IWorksheet sheet)
  {
    bool flag = false;
    if (sheet != null)
      flag = sheet.Names.Contains(strFormula);
    if (!flag && parent != null)
      flag = parent.Names.Contains(strFormula);
    return flag;
  }

  private static int FindCorrespondingBracket(
    string strFormula,
    int BracketPos,
    char[] StartBrackets,
    int delta)
  {
    bool flag = false;
    int index1 = FormulaUtil.IndexOf(FormulaUtil.OpenBrackets, strFormula[BracketPos]);
    char ch;
    if (index1 != -1)
    {
      ch = FormulaUtil.CloseBrackets[index1];
    }
    else
    {
      int index2 = FormulaUtil.IndexOf(FormulaUtil.CloseBrackets, strFormula[BracketPos]);
      ch = index2 != -1 ? FormulaUtil.OpenBrackets[index2] : throw new ArgumentOutOfRangeException("Specified position is not a position of bracket");
    }
    if (FormulaUtil.IndexOf(FormulaUtil.StringBrackets, ch) != -1)
      flag = true;
    int correspondingBracket = BracketPos + delta;
    for (int length = strFormula.Length; correspondingBracket < length && correspondingBracket >= 0; correspondingBracket += delta)
    {
      if ((int) strFormula[correspondingBracket] == (int) ch)
        return correspondingBracket;
      if (!flag && FormulaUtil.IndexOf(StartBrackets, strFormula[correspondingBracket]) != -1)
        correspondingBracket = FormulaUtil.FindCorrespondingBracket(strFormula, correspondingBracket, StartBrackets, delta);
    }
    throw new ArgumentException("Expression is invalid. Can't find corresponding bracket");
  }

  private static bool IsUnaryOperation(string strFormula, int OpIndex)
  {
    return FormulaUtil.IndexOf(strFormula, OpIndex, FormulaUtil.UnaryOperations) != -1;
  }

  private bool IsOperation(string strFormula, int index, out int iOperationIndex)
  {
    iOperationIndex = FormulaUtil.IndexOf(strFormula, index, this.m_arrAllOperations);
    return iOperationIndex != -1;
  }

  private static bool IsFunction(string strOperand, out int iBracketPos)
  {
    iBracketPos = -1;
    if (FormulaUtil.IndexOf(FormulaUtil.StringBrackets, strOperand[0]) != -1)
      return false;
    iBracketPos = strOperand.IndexOf('(');
    return iBracketPos != -1 && FormulaUtil.FindCorrespondingBracket(strOperand, iBracketPos) == strOperand.Length - 1;
  }

  private static string GetErrorOperand(string strFormula, int errorIndex)
  {
    if (strFormula[errorIndex] != '#')
      throw new ArgumentException("Not error string");
    foreach (string key in FormulaUtil.ErrorNameToConstructor.Keys)
    {
      if (string.Compare(strFormula, errorIndex, key, 0, key.Length) == 0)
        return key;
    }
    throw new ArgumentException("Error name was not found");
  }

  private static int GetExpectedIndex(
    Type targetType,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i)
  {
    if (indexes == null)
      return 2;
    ReferenceIndexAttribute referenceIndexAttribute;
    if (indexes.TryGetValue(targetType, out referenceIndexAttribute))
      return referenceIndexAttribute[i];
    return referenceIndexAttribute == null && targetType != typeof (RefPtg) ? FormulaUtil.GetExpectedIndex(typeof (RefPtg), indexes, i) : 2;
  }

  public static int GetIndex(
    Type targetType,
    int valueType,
    Dictionary<Type, ReferenceIndexAttribute> indexes,
    int i,
    OfficeParseFormulaOptions options)
  {
    bool flag1 = (options & OfficeParseFormulaOptions.InName) != OfficeParseFormulaOptions.None;
    if (flag1)
      return 1;
    if (indexes == null)
      return 2;
    int num = FormulaUtil.GetExpectedIndex(targetType, indexes, i) - 1;
    bool flag2 = (options & OfficeParseFormulaOptions.RootLevel) != OfficeParseFormulaOptions.None;
    bool flag3 = (options & OfficeParseFormulaOptions.InArray) != OfficeParseFormulaOptions.None;
    bool flag4 = (options & OfficeParseFormulaOptions.ParseComplexOperand) != OfficeParseFormulaOptions.None;
    int index1 = flag2 ? 3 : num;
    int index2 = flag1 ? 2 : (flag3 ? 2 : 0);
    int index3 = FormulaUtil.DEF_INDEXES_CONVERTION[index1][valueType][index2];
    if (index3 == 1 && flag4 && !flag2)
      index3 = 2;
    return index3;
  }

  public static Ptg CreatePtg(DataProvider provider, ref int offset, OfficeVersion version)
  {
    FormulaToken key = (FormulaToken) provider.ReadByte(offset);
    Ptg ptg1;
    if (!FormulaUtil.s_hashTokenCodeToPtg.TryGetValue(key, out ptg1))
      throw new ArgumentException("Cannot find Formula token with code: " + (object) key);
    Ptg ptg2 = (Ptg) ptg1.Clone();
    ptg2.TokenCode = key;
    ++offset;
    ptg2.InfillPTG(provider, ref offset, version);
    return ptg2;
  }

  public static Ptg CreatePtg(FormulaToken token)
  {
    Ptg ptg = FormulaUtil.TokenCodeToConstructor[token].CreatePtg();
    ptg.TokenCode = token;
    return ptg;
  }

  public static Ptg CreatePtgByType(FormulaToken token)
  {
    Ptg ptg = FormulaUtil.TokenCodeToConstructor[token].CreatePtg(token);
    ptg.TokenCode = token;
    return ptg;
  }

  public static Ptg CreatePtg(FormulaToken token, string tokenString)
  {
    Ptg ptg = FormulaUtil.TokenCodeToConstructor[token].CreatePtg(tokenString);
    ptg.TokenCode = token;
    return ptg;
  }

  public static Ptg CreatePtg(FormulaToken token, string tokenString, IWorkbook parent)
  {
    Ptg ptg = FormulaUtil.TokenCodeToConstructor[token].CreatePtg(tokenString, parent);
    ptg.TokenCode = token;
    return ptg;
  }

  public static Ptg CreatePtg(FormulaToken token, params object[] arrParams)
  {
    Ptg ptg = FormulaUtil.TokenCodeToConstructor[token].CreatePtg(arrParams);
    ptg.TokenCode = token;
    return ptg;
  }

  [CLSCompliant(false)]
  public static Ptg CreatePtg(FormulaToken token, ushort iParam1, ushort iParam2)
  {
    Ptg ptg = FormulaUtil.TokenCodeToConstructor[token].CreatePtg(iParam1, iParam2);
    ptg.TokenCode = token;
    return ptg;
  }

  [CLSCompliant(false)]
  public static Ptg CreatePtg(FormulaToken token, ExcelFunction functionIndex)
  {
    Ptg ptg = FormulaUtil.TokenCodeToConstructor[token].CreatePtg(functionIndex);
    ptg.TokenCode = token;
    return ptg;
  }

  public static Ptg CreatePtg(
    FormulaToken token,
    int iCellRow,
    int iCellColumn,
    string strParam1,
    string strParam2,
    bool bR1C1)
  {
    Ptg ptg = FormulaUtil.TokenCodeToConstructor[token].CreatePtg(iCellRow, iCellColumn, strParam1, strParam2, bR1C1);
    ptg.TokenCode = token;
    return ptg;
  }

  public static Ptg CreatePtg(
    FormulaToken token,
    int iCellRow,
    int iCellColumn,
    string strParam1,
    string strParam2,
    string strParam3,
    string strParam4,
    bool bR1C1,
    IWorkbook book)
  {
    Ptg ptg = FormulaUtil.TokenCodeToConstructor[token].CreatePtg(iCellRow, iCellColumn, strParam1, strParam2, strParam3, strParam4, bR1C1, book);
    ptg.TokenCode = token;
    return ptg;
  }

  public static Ptg CreatePtg(
    FormulaToken token,
    int iCellRow,
    int iCellColumn,
    int iRefIndex,
    string strParam1,
    string strParam2,
    string strParam3,
    string strParam4,
    bool bR1C1,
    IWorkbook book)
  {
    Ptg ptg = FormulaUtil.TokenCodeToConstructor[token].CreatePtg(iCellRow, iCellColumn, iRefIndex, strParam1, strParam2, strParam3, strParam4, bR1C1, book);
    ptg.TokenCode = token;
    return ptg;
  }

  private static Ptg[] SkipUnnecessaryTokens(Ptg[] ptgs)
  {
    List<Ptg> ptgList = new List<Ptg>();
    foreach (Ptg ptg in ptgs)
    {
      if (ptg is AttrPtg)
      {
        AttrPtg attrPtg = ptg as AttrPtg;
        if (attrPtg.HasOptGoto || attrPtg.HasOptimizedIf || attrPtg.HasSemiVolatile || attrPtg.HasOptimizedChoose)
          continue;
      }
      if (!(ptg is MemFuncPtg))
      {
        if (ptg is MemAreaPtg)
        {
          MemAreaPtg memAreaPtg = ptg as MemAreaPtg;
          ptgList.AddRange((IEnumerable<Ptg>) memAreaPtg.Subexpression);
        }
        ptgList.Add(ptg);
      }
    }
    return ptgList.ToArray();
  }

  private string PutUnaryOperationsAhead(string strFormula)
  {
    int num = strFormula != null ? strFormula.Length : throw new ArgumentNullException(nameof (strFormula));
    while (strFormula[strFormula.Length - 1] == '%')
    {
      string leftUnaryOperand = this.GetLeftUnaryOperand(strFormula, strFormula.Length - 1);
      int startIndex = strFormula.Length - leftUnaryOperand.Length - 1;
      strFormula = strFormula.Insert(startIndex, "%");
      strFormula = strFormula.Substring(0, strFormula.Length - 1);
      --num;
      if (num == 0)
        throw new ArgumentException(nameof (strFormula));
    }
    return strFormula;
  }

  [CLSCompliant(false)]
  public static void EditRegisteredFunction(
    string functionName,
    ExcelFunction index,
    ReferenceIndexAttribute[] paramIndexes,
    int paramCount)
  {
    FormulaUtil.FunctionAliasToId.Remove(functionName);
    FormulaUtil.FunctionIdToIndex.Remove(index);
    FormulaUtil.FunctionIdToAlias.Remove(index);
    FormulaUtil.FunctionIdToParamCount.Remove(index);
    FormulaUtil.RegisterFunction(functionName, index, paramIndexes, paramCount);
  }

  public static Dictionary<int, string> ErrorCodeToName => FormulaUtil.s_hashErrorCodeToName;

  public static Dictionary<string, int> ErrorNameToCode => FormulaUtil.s_hashNameToErrorCode;

  public string ArrayRowSeparator => this.m_strArrayRowSeparator;

  public string OperandsSeparator => this.m_strOperandsSeparator;

  public NumberFormatInfo NumberFormat
  {
    get => this.m_numberFormat;
    set => this.m_numberFormat = value;
  }

  public IWorkbook ParentWorkbook => (IWorkbook) this.m_book;

  public static bool IsExcel2013Function(ExcelFunction functionIndex)
  {
    return Array.IndexOf<ExcelFunction>(FormulaUtil.m_excel2013Supported, functionIndex) >= 0;
  }

  public static bool IsExcel2010Function(ExcelFunction functionIndex)
  {
    return Array.IndexOf<ExcelFunction>(FormulaUtil.m_excel2010Supported, functionIndex) >= 0;
  }

  public static bool IsExcel2007Function(ExcelFunction functionIndex)
  {
    return Array.IndexOf<ExcelFunction>(FormulaUtil.m_excel2007Supported, functionIndex) >= 0;
  }

  internal bool HasExternalReference(Ptg[] ptg)
  {
    if (ptg == null)
      return false;
    bool flag = false;
    int index = 0;
    for (int length = ptg.Length; index < length; ++index)
    {
      if (ptg[index] is IReference reference && this.m_book.IsExternalReference((int) reference.RefIndex))
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  internal enum ConstructorId
  {
    Default,
    String,
    ByteArrayOffset,
    StringParent,
    TwoUShorts,
    FunctionIndex,
    TwoStrings,
    FourStrings,
    Int3String4Bool,
    TokenType,
  }

  [Preserve(AllMembers = true)]
  internal class TokenConstructor
  {
    private Dictionary<int, ConstructorInfo> m_hashConstructorToId = new Dictionary<int, ConstructorInfo>();
    private Type m_type;

    private TokenConstructor()
    {
    }

    public TokenConstructor(Type type)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type), "Token type can't be null");
      this.m_type = type.IsSubclassOf(typeof (Ptg)) ? type : throw new ArgumentException("class should be descendant of Ptg", nameof (type));
      this.DefaultConstructor = type.GetConstructor(new Type[0]);
      this.StringConstructor = type.GetConstructor(new Type[1]
      {
        typeof (string)
      });
      this.ArrayConstructor = type.GetConstructor(new Type[2]
      {
        typeof (DataProvider),
        typeof (int)
      });
      this.StringParentConstructor = type.GetConstructor(new Type[2]
      {
        typeof (string),
        typeof (IWorkbook)
      });
      this.TwoUShortsConstructor = type.GetConstructor(new Type[2]
      {
        typeof (ushort),
        typeof (ushort)
      });
      this.FunctionIndexConstructor = type.GetConstructor(new Type[1]
      {
        typeof (ExcelFunction)
      });
      Type[] types1 = new Type[5]
      {
        typeof (int),
        typeof (int),
        typeof (string),
        typeof (string),
        typeof (bool)
      };
      this.TwoStringsConstructor = type.GetConstructor(types1);
      Type[] types2 = new Type[8]
      {
        typeof (int),
        typeof (int),
        typeof (string),
        typeof (string),
        typeof (string),
        typeof (string),
        typeof (bool),
        typeof (IWorkbook)
      };
      this.FourStringsConstructor = type.GetConstructor(types2);
      Type[] types3 = new Type[9]
      {
        typeof (int),
        typeof (int),
        typeof (int),
        typeof (string),
        typeof (string),
        typeof (string),
        typeof (string),
        typeof (bool),
        typeof (IWorkbook)
      };
      this.Int3String4BoolConstructor = type.GetConstructor(types3);
      Type[] types4 = new Type[1]{ typeof (FormulaToken) };
      this.TokenTypeConstructor = type.GetConstructor(types4);
    }

    public Ptg CreatePtg()
    {
      try
      {
        return (Ptg) this.DefaultConstructor.Invoke((object[]) null);
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }

    public Ptg CreatePtg(FormulaToken tokenType)
    {
      try
      {
        return (Ptg) this.TokenTypeConstructor.Invoke(new object[1]
        {
          (object) tokenType
        });
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }

    public Ptg CreatePtg(string strParam)
    {
      try
      {
        return (Ptg) this.StringConstructor.Invoke(new object[1]
        {
          (object) strParam
        });
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }

    public Ptg CreatePtg(DataProvider provider, ref int offset, ParseParameters arguments)
    {
      try
      {
        Ptg ptg = (Ptg) this.ArrayConstructor.Invoke(new object[2]
        {
          (object) provider,
          (object) offset
        });
        offset += ptg.GetSize(arguments.Version);
        return ptg;
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }

    public Ptg CreatePtg(params object[] arrParams)
    {
      Type[] types = new Type[arrParams.Length];
      for (int index = 0; index < arrParams.Length; ++index)
        types[index] = arrParams[index].GetType();
      ConstructorInfo constructor = this.m_type.GetConstructor(types);
      try
      {
        return (Ptg) constructor.Invoke(arrParams);
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }

    public Ptg CreatePtg(string strParam, IWorkbook parent)
    {
      try
      {
        return (Ptg) this.StringParentConstructor.Invoke(new object[2]
        {
          (object) strParam,
          (object) parent
        });
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }

    public Ptg CreatePtg(ushort iParam1, ushort iParam2)
    {
      try
      {
        return (Ptg) this.TwoUShortsConstructor.Invoke(new object[2]
        {
          (object) iParam1,
          (object) iParam2
        });
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }

    public Ptg CreatePtg(ExcelFunction functionIndex)
    {
      try
      {
        return (Ptg) this.FunctionIndexConstructor.Invoke(new object[1]
        {
          (object) functionIndex
        });
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }

    public Ptg CreatePtg(
      int iCellRow,
      int iCellColumn,
      string strParam1,
      string strParam2,
      bool bR1C1)
    {
      try
      {
        return (Ptg) this.TwoStringsConstructor.Invoke(new object[5]
        {
          (object) iCellRow,
          (object) iCellColumn,
          (object) strParam1,
          (object) strParam2,
          (object) bR1C1
        });
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }

    public Ptg CreatePtg(
      int iCellRow,
      int iCellColumn,
      string strParam1,
      string strParam2,
      string strParam3,
      string strParam4,
      bool bR1C1,
      IWorkbook book)
    {
      try
      {
        return (Ptg) this.FourStringsConstructor.Invoke(new object[8]
        {
          (object) iCellRow,
          (object) iCellColumn,
          (object) strParam1,
          (object) strParam2,
          (object) strParam3,
          (object) strParam4,
          (object) bR1C1,
          (object) book
        });
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }

    public Ptg CreatePtg(
      int iCellRow,
      int iCellColumn,
      int iRefIndex,
      string strParam1,
      string strParam2,
      string strParam3,
      string strParam4,
      bool bR1C1,
      IWorkbook book)
    {
      try
      {
        return (Ptg) this.Int3String4BoolConstructor.Invoke(new object[9]
        {
          (object) iCellRow,
          (object) iCellColumn,
          (object) iRefIndex,
          (object) strParam1,
          (object) strParam2,
          (object) strParam3,
          (object) strParam4,
          (object) bR1C1,
          (object) book
        });
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }

    private ConstructorInfo DefaultConstructor
    {
      get => this.GetConstructor(FormulaUtil.ConstructorId.Default);
      set => this.SetConstructor(FormulaUtil.ConstructorId.Default, value);
    }

    private ConstructorInfo StringConstructor
    {
      get => this.GetConstructor(FormulaUtil.ConstructorId.String);
      set => this.SetConstructor(FormulaUtil.ConstructorId.String, value);
    }

    private ConstructorInfo ArrayConstructor
    {
      get => this.GetConstructor(FormulaUtil.ConstructorId.ByteArrayOffset);
      set => this.SetConstructor(FormulaUtil.ConstructorId.ByteArrayOffset, value);
    }

    private ConstructorInfo StringParentConstructor
    {
      get => this.GetConstructor(FormulaUtil.ConstructorId.StringParent);
      set => this.SetConstructor(FormulaUtil.ConstructorId.StringParent, value);
    }

    private ConstructorInfo TwoUShortsConstructor
    {
      get => this.GetConstructor(FormulaUtil.ConstructorId.TwoUShorts);
      set => this.SetConstructor(FormulaUtil.ConstructorId.TwoUShorts, value);
    }

    private ConstructorInfo FunctionIndexConstructor
    {
      get => this.GetConstructor(FormulaUtil.ConstructorId.FunctionIndex);
      set => this.SetConstructor(FormulaUtil.ConstructorId.FunctionIndex, value);
    }

    private ConstructorInfo TwoStringsConstructor
    {
      get => this.GetConstructor(FormulaUtil.ConstructorId.TwoStrings);
      set => this.SetConstructor(FormulaUtil.ConstructorId.TwoStrings, value);
    }

    private ConstructorInfo FourStringsConstructor
    {
      get => this.GetConstructor(FormulaUtil.ConstructorId.FourStrings);
      set => this.SetConstructor(FormulaUtil.ConstructorId.FourStrings, value);
    }

    private ConstructorInfo Int3String4BoolConstructor
    {
      get => this.GetConstructor(FormulaUtil.ConstructorId.Int3String4Bool);
      set => this.SetConstructor(FormulaUtil.ConstructorId.Int3String4Bool, value);
    }

    private ConstructorInfo TokenTypeConstructor
    {
      get => this.GetConstructor(FormulaUtil.ConstructorId.TokenType);
      set => this.SetConstructor(FormulaUtil.ConstructorId.TokenType, value);
    }

    private ConstructorInfo GetConstructor(FormulaUtil.ConstructorId id)
    {
      ConstructorInfo constructor;
      this.m_hashConstructorToId.TryGetValue((int) id, out constructor);
      return constructor;
    }

    private void SetConstructor(FormulaUtil.ConstructorId id, ConstructorInfo value)
    {
      int key = (int) id;
      if (value != (ConstructorInfo) null)
        this.m_hashConstructorToId[key] = value;
      else
        this.m_hashConstructorToId.Remove(key);
    }
  }
}
