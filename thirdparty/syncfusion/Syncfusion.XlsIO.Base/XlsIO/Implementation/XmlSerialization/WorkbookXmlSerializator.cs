// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.WorkbookXmlSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Compression;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Interfaces.XmlSerialization;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

[XmlSerializator(ExcelXmlSaveType.MSExcel)]
public class WorkbookXmlSerializator : IXmlSerializator
{
  public const string DEF_VERSION_STRING = "<?xml version=\"1.0\"?>";
  public const string DEF_APPLICATION_STRING = "<?mso-application progid=\"Excel.Sheet\"?>";
  private const string DEF_O_NAMESPACE = "urn:schemas-microsoft-com:office:office";
  private const string DEF_X_NAMESPACE = "urn:schemas-microsoft-com:office:excel";
  private const string DEF_SS_NAMESPACE = "urn:schemas-microsoft-com:office:spreadsheet";
  private const string DEF_HTML_NAMESPACE = "http://www.w3.org/TR/REC-html40";
  private const string DEF_SS_PREF = "ss";
  private const string DEF_HTML_PREF = "html";
  private const string DEF_O_PREF = "o";
  private const string DEF_X_PREF = "x";
  private const string DEF_NAMESPACE_PREF = "xmlns:";
  internal const string DEF_XMLNS_PREF = "xmlns";
  public const string DEF_WORKBOOK_PREF = "Workbook";
  public const string DEF_WORKSHEET_PREF = "Worksheet";
  public const string DEF_NAME_PREF = "Name";
  public const string DEF_TABLE_PREF = "Table";
  public const string DEF_ROW_PREF = "Row";
  public const string DEF_CELL_PREF = "Cell";
  public const string DEF_DATA_PREF = "Data";
  public const string DEF_NAMES_PREF = "Names";
  public const string DEF_NAMEDRANGE_PREF = "NamedRange";
  public const string DEF_STYLES_PREF = "Styles";
  public const string DEF_STYLE_PREF = "Style";
  public const string DEF_FONT_PREF = "Font";
  public const string DEF_PROTECTION_PREF = "Protection";
  public const string DEF_ALIGNMENT_PREF = "Alignment";
  public const string DEF_NUMBERFORMAT_PREF = "NumberFormat";
  public const string DEF_INTERIOR_PREF = "Interior";
  public const string DEF_BORDERS_PREF = "Borders";
  public const string DEF_BORDER_PREF = "Border";
  private const string DEF_AUTOFILTER_PREF = "AutoFilter";
  private const string DEF_AUTOFILTERCOLUMN_PREF = "AutoFilterColumn";
  private const string DEF_AUTOFILTERAND_PREF = "AutoFilterAnd";
  private const string DEF_AUTOFILTERCONDITION_PREF = "AutoFilterCondition";
  private const string DEF_AUTOFILTEROR_PREF = "AutoFilterOr";
  public const string DEF_COMMENT_PREF = "Comment";
  private const string DEF_B_TAG = "<B>";
  private const string DEF_B_END_TAG = "</B>";
  private const string DEF_I_TAG = "<I>";
  private const string DEF_I_END_TAG = "</I>";
  private const string DEF_U_TAG = "<U>";
  private const string DEF_U_END_TAG = "</U>";
  private const string DEF_S_TAG = "<S>";
  private const string DEF_S_END_TAG = "</S>";
  private const string DEF_SUB_TAG = "<Sub>";
  private const string DEF_SUB_END_TAG = "</Sub>";
  private const string DEF_SUP_TAG = "<Sup>";
  private const string DEF_SUP_END_TAG = "</Sup>";
  private const string DEF_FONT_END_TAG = "</Font>";
  private const string DEF_FONT_TAG = "<Font";
  public const string DEF_SPAN_PREF = "Span";
  public const string DEF_COLUMN_PREF = "Column";
  public const string DEF_CONDITIONAL_FORMATTING_PREF = "ConditionalFormatting";
  public const string DEF_CONDITIONAL_PREF = "Condition";
  public const string DEF_QUALIFIER_PREF = "Qualifier";
  public const string DEF_VALUE1_PREF = "Value1";
  public const string DEF_VALUE2_PREF = "Value2";
  public const string DEF_WORKSHEET_OPTIONS_PREF = "WorksheetOptions";
  public const string DEF_PAGE_SETUP_PREF = "PageSetup";
  public const string DEF_FOOTER_PREF = "Footer";
  public const string DEF_HEADER_PREF = "Header";
  public const string DEF_LAYOUT_PREF = "Layout";
  public const string DEF_PAGE_MARGINS_PREF = "PageMargins";
  public const string DEF_PRINT_PREF = "Print";
  public const string DEF_COMMENTS_LAYOUT_PREF = "CommentsLayout";
  public const string DEF_PRINT_ERRORS_PREF = "PrintErrors";
  private const string DEF_FIT_TO_PAGE_PREF = "FitToPage";
  public const string DEF_LEFT_TO_RIGHT_PREF = "LeftToRight";
  public const string DEF_ACTIVE_PANE_PREF = "ActivePane";
  public const string DEF_FIRST_VISIBLE_ROW_PREF = "TopRowVisible";
  public const string DEF_SPLIT_HORIZONTAL_PANE_PREF = "SplitHorizontal";
  public const string DEF_SPLIT_VERTICAL_PANE_PREF = "SplitVertical";
  public const string DEF_TOPROW_BOTTOM_PANE_PREF = "TopRowBottomPane";
  public const string DEF_LEFTCOLUMN_RIGHT_PANE_PREF = "LeftColumnRightPane";
  public const string DEF_FREEZE_PANES_PREF = "FreezePanes";
  public const string DEF_FROZEN_NOSPLIT_PANES_PREF = "FrozenNoSplit";
  public const string DEF_PANES_PREF = "Panes";
  public const string DEF_PANE_PREF = "Pane";
  public const string DEF_NUMBER_PANE_PREF = "Number";
  public const string DEF_ACTIVECOL_PANE_PREF = "ActiveCol";
  public const string DEF_ACTIVEROW_PANE_PREF = "ActiveRow";
  internal const string DEF_RANGESELECTION_PANE_PREF = "RangeSelection";
  public const string DEF_TABCOLOR_INDEX_PREF = "TabColorIndex";
  public const string DEF_ZOOM_PREF = "Zoom";
  public const string DEF_DISPLAY_GRIDLINES_PREF = "DoNotDisplayGridlines";
  public const string DEF_VISIBLE_PREF = "Visible";
  private const string DEF_DISPLAY_HEADINGS_PREF = "DoNotDisplayHeadings";
  public const string DEF_EXCELWORKBOOK_PREF = "ExcelWorkbook";
  public const string DEF_ACTIVE_SHEET_PREF = "ActiveSheet";
  private const string DEF_SELECTED_PREF = "Selected";
  private const string DEF_SELECTED_SHEETS_PREF = "SelectedSheets";
  public const string DEF_FIRST_VISIBLE_SHEET_PREF = "FirstVisibleSheet";
  public const string DEF_DATAVALIDATION_PREF = "DataValidation";
  internal const string DEF_HIDE_WORKBOOK_TABS = "HideWorkbookTabs";
  public const string DEF_RIGHTTOLEFT_PREF = "RightToLeft";
  public const string DEF_INDEX_PREF = "Index";
  public const string DEF_TYPE_PREF = "Type";
  private const string DEF_TICKED_PREF = "Ticked";
  public const string DEF_FORMULA_PREF = "Formula";
  public const string DEF_REFERSTO_PREF = "RefersTo";
  public const string DEF_ID_PREF = "ID";
  public const string DEF_PARENT_PREF = "Parent";
  public const string DEF_BOLD_PREF = "Bold";
  public const string DEF_FONTNAME_PREF = "FontName";
  public const string DEF_COLOR_PREF = "Color";
  public const string DEF_ITALIC_PREF = "Italic";
  public const string DEF_OUTLINE_PREF = "Outline";
  public const string DEF_SHADOW_PREF = "Shadow";
  public const string DEF_SIZE_PREF = "Size";
  public const string DEF_STRIKETHROUGH_PREF = "StrikeThrough";
  public const string DEF_UNDERLINE_PREF = "Underline";
  public const string DEF_PROTECTED_PREF = "Protected";
  public const string DEF_HIDEFORMULA_PREF = "HideFormula";
  public const string DEF_HORIZONTAL_PREF = "Horizontal";
  public const string DEF_INDENT_PREF = "Indent";
  public const string DEF_READINGORDER_PREF = "ReadingOrder";
  public const string DEF_ROTATE_PREF = "Rotate";
  public const string DEF_SHRINKTOFIT_PREF = "ShrinkToFit";
  public const string DEF_VERTICAL_PREF = "Vertical";
  public const string DEF_VERTICALTEXT_PREF = "VerticalText";
  public const string DEF_WRAPTEXT_PREF = "WrapText";
  public const string DEF_FORMAT_PREF = "Format";
  public const string DEF_PATTERNCOLOR_PREF = "PatternColor";
  public const string DEF_PATTERN_PREF = "Pattern";
  public const string DEF_POSITION_PREF = "Position";
  public const string DEF_RANGE_PREF = "Range";
  private const string DEF_OPERATOR_PREF = "Operator";
  private const string DEF_VALUE_PREF = "Value";
  public const string DEF_AUTHOR_PREF = "Author";
  public const string DEF_SHOWALWAYS_PREF = "ShowAlways";
  public const string DEF_DEFAULTCOLUMNWIDTH_PREF = "DefaultColumnWidth";
  public const string DEF_DEFAULTROWHEIGHT_PREF = "DefaultRowHeight";
  public const string DEF_WIDTH_PREF = "Width";
  public const string DEF_HIDDEN_PREF = "Hidden";
  public const string DEF_STYLEID_PREF = "StyleID";
  public const string DEF_AUTOFIT_WIDTH_PREF = "AutoFitWidth";
  public const string DEF_AUTOFIT_HEIGHT_PREF = "AutoFitHeight";
  public const string DEF_HEIGHT_PREF = "Height";
  public const string DEF_FACE_PREF = "Face";
  public const string DEF_LINE_STYLE_PREF = "LineStyle";
  public const string DEF_WEIGHT_PREF = "Weight";
  public const string DEF_VERTICAL_ALIGN_PREF = "VerticalAlign";
  public const string DEF_MERGE_ACROSS_PREF = "MergeAcross";
  public const string DEF_MERGE_DOWN_PREF = "MergeDown";
  public const string DEF_HYPRER_TIP_PREF = "HRefScreenTip";
  public const string DEF_HREF_PREF = "HRef";
  public const string DEF_MARGIN_PREF = "Margin";
  public const string DEF_MARGIN_TOP_PREF = "Top";
  public const string DEF_MARGIN_RIGHT_PREF = "Right";
  public const string DEF_MARGIN_LEFT_PREF = "Left";
  public const string DEF_MARGIN_BOTTOM_PREF = "Bottom";
  public const string DEF_CENTER_HORIZONTAL_PREF = "CenterHorizontal";
  public const string DEF_CENTER_VERTICAL_PREF = "CenterVertical";
  public const string DEF_ORIENTATION_PREF = "Orientation";
  public const string DEF_START_PAGE_NUMBER_PREF = "StartPageNumber";
  public const string DEF_NUMBER_OF_COPIES_PREF = "NumberofCopies";
  public const int DEF_NUMBER_OF_COPIES = 1;
  public const string DEF_HORIZONTAL_RESOLUTION_PREF = "HorizontalResolution";
  public const string DEF_PAPER_SIZE_INDEX_PREF = "PaperSizeIndex";
  public const string DEF_SCALE_PREF = "Scale";
  public const string DEF_FIT_WIDTH_PREF = "FitWidth";
  public const string DEF_FIT_HEIGHT_PREF = "FitHeight";
  public const string DEF_GRIDLINES_PREF = "Gridlines";
  public const string DEF_BLACK_AND_WHITE_PREF = "BlackAndWhite";
  public const string DEF_DRAFT_QUALITY_PREF = "DraftQuality";
  public const string DEF_ROWCOL_HEADINGS_PREF = "RowColHeadings";
  public const string DEF_COLON = ":";
  public const string DEF_SEMICOLON = ";";
  public const string DEF_FONT_COLOR_CF = "color";
  public const string DEF_FONT_STYLE_CF = "font-style";
  public const string DEF_FONT_WEIGHT_CF = "font-weight";
  public const string DEF_FONT_BOLD_CF = "700";
  public const string DEF_FONT_REGULAR_CF = "400";
  private const string DEF_FONT_ITALIC_CF = "font-style:italic;";
  private const string DEF_FONT_STRIKE_CF = "text-line-through:single;";
  public const string DEF_FONT_STRIKETHROUGH_CF = "text-line-through";
  public const string DEF_FONT_STRIKETHROUGH_SINGLE_CF = "single";
  public const string DEF_FONT_UNDERLINE_CF = "text-underline-style";
  public const string DEF_PATTERN_BACKGROUND_CF = "background";
  public const string DEF_PATTERN_FILL_CF = "mso-pattern";
  private const string DEF_BORDER_CF = "border-";
  public const string DEF_BORDERTOP_CF = "border-top";
  public const string DEF_BORDERBOTTOM_CF = "border-bottom";
  public const string DEF_BORDERLEFT_CF = "border-left";
  public const string DEF_BORDERRIGHT_CF = "border-right";
  private const string DEF_FONT_NAME = "Arial";
  private const string DEF_STYLE_NONE = "None";
  public const int DEF_FONT_SIZE = 8;
  private const int DEF_LEFT_DIAGONAL_BORDER = 5;
  private const int DEF_RIGHT_DIAGONAL_BORDER = 6;
  public const int DEF_STYLE_ZERO = 0;
  public const int DEF_STYLE_ROTATION = 90;
  private const int DEF_STYLE_FONT_SIZE = 10;
  public const int DEF_ROTATION_TEXT = 255 /*0xFF*/;
  private const int DEF_BORDER_INCR = 5;
  public const string DEF_STYLE_NAME = "Default";
  private const string DEF_UNIQUE_STRING = "s";
  private const string DEF_STYLE_ALIGN_NONE = "None";
  private const string DEF_STYLE_ALIGN_SUBSCRIPT = "Subscript";
  private const string DEF_STYLE_ALIGN_SUPERSCRIPT = "Superscript";
  private const double DEF_MARGIN = 0.5;
  private const int DEF_SCALE = 100;
  private const int DEF_FIT = 1;
  private const int DEF_ZOOM = 100;
  private const string DEF_XML_TRUE = "1";
  private const string DEF_XML_FALSE = "0";
  private const string DEF_AUTOFILTER_ALL_TYPE = "All";
  public const string DEF_COLOR_STRING = "#";
  private const string DEF_AUTOFILTER_BOTTOM_TYPE = "Bottom";
  private const string DEF_AUTOFILTER_TOP_TYPE = "Top";
  private const string DEF_AUTOFILTER_PERCENT_TYPE = "Percent";
  private const string DEF_AUTOFILTER_BLANKS_TYPE = "Blanks";
  private const string DEF_AUTOFILTER_CUSTOM_TYPE = "Custom";
  private const string DEF_AUTOFILTER_NON_BLANKS_TYPE = "NonBlanks";
  private const double DEF_COLUMN_WIDTH = 48.0;
  public const double DEF_ROW_HEIGHT = 12.75;
  public const double DEF_COLUMN_DIV = 256.0;
  public const double DEF_ROW_DIV = 20.0;
  private const string DEF_DATATIME_MASK = "yyyy-MM-ddTHH:mm:ss";
  private const int DEF_MERGED_STYLE = 5000;
  private const string DEF_10_CHAR = "&#10;";
  public const string DEF_BAD_REF = "#REF";
  public const string DEF_BAD_REF_UPDATE = "#REF!";
  public const string DEF_BAD_FORMULA = "=#REF!";
  public const int DEF_MAX_COLUMN = 256 /*0x0100*/;
  public const int DEF_MIN_COLUMN = 0;
  [CLSCompliant(false)]
  public const long DEF_MERGE_COD = 10000000000;
  public static string[] DEF_PATTERN_STRING_CF = new string[19]
  {
    "none",
    "solid",
    "gray-50",
    "gray-75",
    "gray-25",
    "horz-stripe",
    "vert-stripe",
    "reverse-diag-stripe",
    "diag-stripe",
    "diag-cross",
    "thick-diag-cross",
    "thin-horz-stripe",
    "thin-vert-stripe",
    "thin-reverse-diag-stripe",
    "thin-diag-stripe",
    "thin-horz-cross",
    "thin-diag-cross",
    "gray-125",
    "gray-0625"
  };
  public static string[] DEF_BORDER_LINE_CF = new string[14]
  {
    "none",
    ".5pt solid",
    "1.0pt solid",
    ".5pt dashed",
    ".5pt dotted",
    "1.5pt solid",
    "2.0pt double",
    ".5pt hairline",
    "1.0pt dashed",
    ".5pt dot-dash",
    "1.0pt dot-dash",
    ".5pt dot-dot-dash",
    "1.0pt dot-dot-dash",
    "1.0pt dot-dash-slanted"
  };
  public static string[] DEF_COMPARISION_OPERATORS_PREF = new string[9]
  {
    "None",
    "Between",
    "NotBetween",
    "Equal",
    "NotEqual",
    "Greater",
    "Less",
    "GreaterOrEqual",
    "LessOrEqual"
  };
  public static string[] DEF_BORDER_POSITION_STRING = new string[11]
  {
    "",
    "",
    "",
    "",
    "",
    "DiagonalLeft",
    "DiagonalRight",
    "Left",
    "Top",
    "Bottom",
    "Right"
  };
  public static string[] DEF_BORDER_LINE_TYPE_STRING = new string[14]
  {
    "None",
    "1 Continuous",
    "2 Continuous",
    "1 Dash",
    "1 Dot",
    "3 Continuous",
    "3 Double",
    "Continuous",
    "2 Dash",
    "1 DashDot",
    "2 DashDot",
    "1 DashDotDot",
    "2 DashDotDot",
    "2 SlantDashDot"
  };
  internal static string[] DEF_BORDER_LINE_TYPES = new string[8]
  {
    "None",
    "Dot",
    "Dash",
    "Continuous",
    "Double",
    "DashDot",
    "DashDotDot",
    "SlantDashDot"
  };
  public static string[] DEF_PRINT_LOCATION_STRING = new string[3]
  {
    "InPlace",
    "NoComments",
    "SheetEnd"
  };
  public static string[] DEF_PRINT_ERROR_STRING = new string[4]
  {
    "none",
    "Blank",
    "Dash",
    "NA"
  };
  public static string[] DEF_VISIBLE_STRING = new string[3]
  {
    "error",
    "SheetHidden",
    "SheetVeryHidden"
  };
  public static string[] DEF_PATTERN_STRING = new string[19]
  {
    "None",
    "Solid",
    "Gray50",
    "Gray75",
    "Gray25",
    "HorzStripe",
    "VertStripe",
    "ReverseDiagStripe",
    "DiagStripe",
    "DiagCross",
    "ThickDiagCross",
    "ThinHorzStripe",
    "ThinVertStripe",
    "ThinReverseDiagStripe",
    "ThinDiagStripe",
    "ThinHorzCross",
    "ThinDiagCross",
    "Gray125",
    "Gray0625"
  };
  private readonly string[] DEF_AUTOFILTER_OPERATION_STRING = new string[7]
  {
    "",
    "LessThan",
    "Equals",
    "LessThanOrEqual",
    "GreaterThan",
    "DoesNotEqual",
    "GreaterThanOrEqual"
  };
  public static string[] DEF_ERRORSTYLE = new string[3]
  {
    "Stop",
    "Warn",
    "Info"
  };
  public static string[] DEF_ALLOWTYPE_STRING = new string[8]
  {
    "Any",
    "Whole",
    "Decimal",
    "List",
    "Date",
    "Time",
    "TextLength",
    "Custom"
  };
  private Dictionary<long, int> m_mergeStyles = new Dictionary<long, int>();
  private StringBuilder m_builderStart;
  private StringBuilder m_builderEnd;
  private FormulaUtil m_formulaUtil;

  public static long GetUniqueID(int iSheetIndex, long lCellIndex)
  {
    return ((long) RangeImpl.GetRowFromCellIndex(lCellIndex) << 32 /*0x20*/) + ((long) RangeImpl.GetColumnFromCellIndex(lCellIndex) << 16 /*0x10*/) + (long) iSheetIndex;
  }

  public static int GetSheetIndexByUniqueId(long lUniqueId)
  {
    return (int) (lUniqueId & (long) ushort.MaxValue);
  }

  public static long GetCellIndexByUniqueId(long lUniqueId)
  {
    int firstRow = (int) (lUniqueId >> 32 /*0x20*/ & (long) uint.MaxValue);
    return RangeImpl.GetCellIndex((int) (lUniqueId >> 16 /*0x10*/ & (long) ushort.MaxValue), firstRow);
  }

  private void SerializeNames(XmlWriter writer, INames names, bool isLocal)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (names == null)
      throw new ArgumentNullException(nameof (names));
    WorkbookNamesCollection workbookNamesCollection = (WorkbookNamesCollection) null;
    WorksheetNamesCollection worksheetNamesCollection = (WorksheetNamesCollection) null;
    if (isLocal)
      worksheetNamesCollection = names as WorksheetNamesCollection;
    else
      workbookNamesCollection = names as WorkbookNamesCollection;
    int count = names.Count;
    if (count == 0)
      return;
    writer.WriteStartElement("ss", "Names", (string) null);
    for (int index = 0; index < count; ++index)
    {
      IName name = isLocal ? worksheetNamesCollection[index] : workbookNamesCollection[index];
      if (name != null && !(name as NameImpl).IsDeleted && name.IsLocal == isLocal)
        this.SerializeName(writer, name);
    }
    writer.WriteEndElement();
  }

  private void SerializeName(XmlWriter writer, IName name)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    writer.WriteStartElement("ss", "NamedRange", (string) null);
    writer.WriteAttributeString("ss", "Name", (string) null, name.Name);
    string refersToR1C1 = name.RefersToR1C1;
    string str = refersToR1C1 == null || refersToR1C1.Length <= 0 ? "=#REF!" : refersToR1C1;
    if (str.IndexOf("#REF") != -1)
      str = "=#REF!";
    writer.WriteAttributeString("ss", "RefersTo", (string) null, str);
    if (!name.Visible)
      writer.WriteAttributeString("ss", "Hidden", (string) null, "1");
    writer.WriteEndElement();
  }

  private void SerializeStyles(
    XmlWriter writer,
    ExtendedFormatsCollection extends,
    List<ExtendedFormatImpl> listToReparse)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (extends == null)
      throw new ArgumentNullException("styles");
    writer.WriteStartElement("ss", "Styles", (string) null);
    int index = 0;
    for (int count = extends.Count; index < count; ++index)
    {
      ExtendedFormatImpl extend = extends[index];
      int parentIndex = extend.ParentIndex;
      if (extend.HasParent && parentIndex > extend.XFormatIndex)
        listToReparse.Add(extend);
      else
        this.SerializeStyle(writer, extend);
    }
    if (listToReparse.Count > 0)
      this.ReSerializeStyle(writer, listToReparse);
    writer.WriteEndElement();
  }

  private void SerializeStyle(XmlWriter writer, ExtendedFormatImpl format)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    writer.WriteStartElement("ss", "Style", (string) null);
    bool flag = format.HasParent && format.ParentIndex != 0;
    string str = format.XFormatIndex == 0 ? "Default" : "s" + format.XFormatIndex.ToString();
    writer.WriteAttributeString("ss", "ID", (string) null, str);
    if (format.XFType == ExtendedFormatRecord.TXFType.XF_CELL && !flag)
    {
      string name = ((StylesCollection) format.Workbook.Styles).GetByXFIndex(format.Index)?.Name;
      if (name != null && name.Length > 0)
      {
        writer.WriteAttributeString("ss", "Name", (string) null, name);
        flag = false;
      }
    }
    if (flag)
      writer.WriteAttributeString("ss", "Parent", (string) null, "s" + format.ParentIndex.ToString());
    this.SerializeStyleElements(writer, format);
    writer.WriteEndElement();
  }

  private void SerializeStyleElements(XmlWriter writer, ExtendedFormatImpl format)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    int num = format.HasParent ? 1 : 0;
    if (format.IncludeFont)
      this.SerializeFont(writer, format.Font);
    if (format.IncludeProtection)
      this.SerializeProtection(writer, format);
    if (format.IncludeAlignment)
      this.SerializeAlignment(writer, format);
    if (format.IncludeNumberFormat)
      this.SerializeNumberFormat(writer, format.NumberFormat);
    if (format.IncludePatterns)
      this.SerializeInterior(writer, format);
    if (!format.IncludeBorder)
      return;
    this.SerializeBorders(writer, format.Borders);
  }

  private void SerializeFont(XmlWriter writer, IFont font)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    writer.WriteStartElement("ss", "Font", (string) null);
    if (font.Bold)
      writer.WriteAttributeString("ss", "Bold", (string) null, "1");
    if (font.FontName != "Arial")
      writer.WriteAttributeString("ss", "FontName", (string) null, font.FontName);
    writer.WriteAttributeString("ss", "Color", (string) null, this.GetColorString(font.RGBColor));
    if (font.Italic)
      writer.WriteAttributeString("ss", "Italic", (string) null, "1");
    if (font.MacOSOutlineFont)
      writer.WriteAttributeString("ss", "Outline", (string) null, "1");
    if (font.MacOSShadow)
      writer.WriteAttributeString("ss", "Shadow", (string) null, "1");
    if (font.Size != 10.0)
      writer.WriteAttributeString("ss", "Size", (string) null, XmlConvert.ToString(font.Size));
    if (font.Strikethrough)
      writer.WriteAttributeString("ss", "StrikeThrough", (string) null, "1");
    if (font.Underline != ExcelUnderline.None)
      writer.WriteAttributeString("ss", "Underline", (string) null, font.Underline.ToString());
    string styleFontAlign = this.GetStyleFontAlign(font);
    if (styleFontAlign != "None")
      writer.WriteAttributeString("ss", "VerticalAlign", (string) null, styleFontAlign);
    writer.WriteEndElement();
  }

  private void SerializeProtection(XmlWriter writer, ExtendedFormatImpl format)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    writer.WriteStartElement("ss", "Protection", (string) null);
    if (!format.Locked)
      writer.WriteAttributeString("ss", "Protected", (string) null, "0");
    if (format.FormulaHidden)
      writer.WriteAttributeString("x", "HideFormula", (string) null, "1");
    writer.WriteEndElement();
  }

  private void SerializeAlignment(XmlWriter writer, ExtendedFormatImpl format)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    writer.WriteStartElement("ss", "Alignment", (string) null);
    if (format.HorizontalAlignment != ExcelHAlign.HAlignGeneral)
    {
      string styleHalignString = this.GetStyleHAlignString(format.HorizontalAlignment);
      writer.WriteAttributeString("ss", "Horizontal", (string) null, styleHalignString);
    }
    if (format.IndentLevel != 0)
      writer.WriteAttributeString("ss", "Indent", (string) null, format.IndentLevel.ToString());
    if (format.ReadingOrder != ExcelReadingOrderType.Context)
      writer.WriteAttributeString("ss", "ReadingOrder", (string) null, format.ReadingOrder.ToString());
    int rotation = format.Rotation;
    switch (rotation)
    {
      case 0:
        if (format.ShrinkToFit)
          writer.WriteAttributeString("ss", "ShrinkToFit", (string) null, "1");
        string styleValignString = this.GetStyleVAlignString(format.VerticalAlignment);
        writer.WriteAttributeString("ss", "Vertical", (string) null, styleValignString);
        if (format.WrapText)
          writer.WriteAttributeString("ss", "WrapText", (string) null, "1");
        writer.WriteEndElement();
        break;
      case (int) byte.MaxValue:
        writer.WriteAttributeString("ss", "VerticalText", (string) null, "1");
        goto case 0;
      default:
        int num = rotation > 90 ? 90 - rotation : rotation;
        writer.WriteAttributeString("ss", "Rotate", (string) null, num.ToString());
        goto case 0;
    }
  }

  private void SerializeNumberFormat(XmlWriter writer, string strNumber)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (strNumber == null)
      throw new ArgumentNullException(nameof (strNumber));
    writer.WriteStartElement("ss", "NumberFormat", (string) null);
    writer.WriteAttributeString("ss", "Format", (string) null, strNumber);
    writer.WriteEndElement();
  }

  private void SerializeInterior(XmlWriter writer, ExtendedFormatImpl format)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    writer.WriteStartElement("ss", "Interior", (string) null);
    if (!format.IsDefaultColor)
    {
      string colorString = this.GetColorString(format.Color);
      writer.WriteAttributeString("ss", "Color", (string) null, colorString);
    }
    if (!format.IsDefaultPatternColor)
    {
      string colorString = this.GetColorString(format.PatternColor);
      writer.WriteAttributeString("ss", "PatternColor", (string) null, colorString);
    }
    if (format.FillPattern != ExcelPattern.None)
    {
      string str = WorkbookXmlSerializator.DEF_PATTERN_STRING[(int) format.FillPattern];
      writer.WriteAttributeString("ss", "Pattern", (string) null, str);
    }
    writer.WriteEndElement();
  }

  private void SerializeBorders(XmlWriter writer, IBorders borders)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (borders == null)
      throw new ArgumentNullException(nameof (borders));
    writer.WriteStartElement("ss", "Borders", (string) null);
    ExcelBordersIndex[] excelBordersIndexArray = new ExcelBordersIndex[6]
    {
      ExcelBordersIndex.DiagonalDown,
      ExcelBordersIndex.DiagonalUp,
      ExcelBordersIndex.EdgeBottom,
      ExcelBordersIndex.EdgeLeft,
      ExcelBordersIndex.EdgeRight,
      ExcelBordersIndex.EdgeTop
    };
    int index = 0;
    for (int length = excelBordersIndexArray.Length; index < length; ++index)
    {
      ExcelBordersIndex Index = excelBordersIndexArray[index];
      IBorder border = borders[Index];
      if (border != null)
      {
        int iBorderIndex = (int) Index;
        this.SerializeBorder(writer, border, iBorderIndex);
      }
    }
    writer.WriteEndElement();
  }

  private void SerializeBorder(XmlWriter writer, IBorder border, int iBorderIndex)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (border == null)
      throw new ArgumentNullException(nameof (border));
    if ((iBorderIndex == 5 || iBorderIndex == 6) && !border.ShowDiagonalLine)
      return;
    writer.WriteStartElement("ss", "Border", (string) null);
    string str1 = WorkbookXmlSerializator.DEF_BORDER_POSITION_STRING[iBorderIndex];
    writer.WriteAttributeString("ss", "Position", (string) null, str1);
    string colorString = this.GetColorString(border.ColorRGB);
    writer.WriteAttributeString("ss", "Color", (string) null, colorString);
    if (border.LineStyle != ExcelLineStyle.None)
    {
      string str2 = WorkbookXmlSerializator.DEF_BORDER_LINE_TYPE_STRING[(int) border.LineStyle];
      int length = str2.IndexOf(" ");
      string str3 = length != -1 ? str2.Substring(length + 1) : str2;
      writer.WriteAttributeString("ss", "LineStyle", (string) null, str3);
      if (length != -1)
        writer.WriteAttributeString("ss", "Weight", (string) null, str2.Substring(0, length));
    }
    writer.WriteEndElement();
  }

  private void SerializeAutoFilters(XmlWriter writer, IAutoFilters autofilters)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (autofilters == null)
      throw new ArgumentNullException(nameof (autofilters));
    writer.WriteStartElement("x", "AutoFilter", (string) null);
    writer.WriteAttributeString("x", "Range", (string) null, ((AutoFiltersCollection) autofilters).AddressR1C1);
    int columnIndex = 0;
    for (int count = autofilters.Count; columnIndex < count; ++columnIndex)
    {
      if (autofilters[columnIndex].IsFiltered)
        this.SerializeAutoFilter(writer, autofilters[columnIndex]);
    }
    writer.WriteEndElement();
  }

  private void SerializeAutoFilter(XmlWriter writer, IAutoFilter autofilter)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (autofilter == null)
      throw new ArgumentNullException(nameof (autofilter));
    writer.WriteStartElement("x", "AutoFilterColumn", (string) null);
    AutoFilterImpl autoFilterImpl = (AutoFilterImpl) autofilter;
    writer.WriteAttributeString("x", "Index", (string) null, autoFilterImpl.Index.ToString());
    if (autoFilterImpl.IsTop10)
    {
      string str = autoFilterImpl.IsTop ? "Top" : "Bottom";
      if (autoFilterImpl.IsPercent)
        str += "Percent";
      writer.WriteAttributeString("x", "Value", (string) null, autoFilterImpl.Top10Number.ToString());
      writer.WriteAttributeString("x", "Type", (string) null, str);
    }
    if (autoFilterImpl.IsBlanks)
      writer.WriteAttributeString("x", "Type", (string) null, "Blanks");
    if (autoFilterImpl.IsNonBlanks)
      writer.WriteAttributeString("x", "Type", (string) null, "NonBlanks");
    if (autoFilterImpl.IsFirstCondition && autoFilterImpl.FirstCondition.DataType != ExcelFilterDataType.MatchAllNonBlanks)
    {
      writer.WriteAttributeString("x", "Type", (string) null, "Custom");
      this.SerializeAFCondition(writer, autoFilterImpl.FirstCondition);
    }
    if (autoFilterImpl.IsSecondCondition && autoFilterImpl.SecondCondition.DataType != ExcelFilterDataType.MatchAllNonBlanks)
    {
      string localName = autoFilterImpl.IsAnd ? "AutoFilterAnd" : "AutoFilterOr";
      writer.WriteStartElement("x", localName, (string) null);
      this.SerializeAFCondition(writer, autoFilterImpl.SecondCondition);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeAFCondition(XmlWriter writer, IAutoFilterCondition condition)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (condition == null)
      throw new ArgumentNullException(nameof (condition));
    writer.WriteStartElement("x", "AutoFilterCondition", (string) null);
    string str = this.DEF_AUTOFILTER_OPERATION_STRING[(int) condition.ConditionOperator];
    string filterConditionValue = this.GetAutoFilterConditionValue(condition);
    writer.WriteAttributeString("x", "Operator", (string) null, str);
    writer.WriteAttributeString("x", "Value", (string) null, filterConditionValue);
    writer.WriteEndElement();
  }

  private void SerializeCell(XmlWriter writer, WorksheetImpl sheet, int iRowIndex)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    CellRecordCollection cells = sheet != null ? sheet.CellRecords : throw new ArgumentNullException(nameof (sheet));
    HyperLinksCollection hyperLinks = (HyperLinksCollection) null;
    if (sheet.HasHyperlinks)
      hyperLinks = (HyperLinksCollection) sheet.HyperLinks;
    MergeCellsImpl mergeCells = sheet.MergeCells;
    int num = 0;
    int firstColumn = sheet.FirstColumn;
    for (int lastColumn = sheet.LastColumn; firstColumn <= lastColumn; ++firstColumn)
    {
      long cellIndex = RangeImpl.GetCellIndex(firstColumn, iRowIndex);
      Rectangle leftTopCell = mergeCells.GetLeftTopCell(new Rectangle(firstColumn - 1, iRowIndex - 1, 0, 0));
      bool bMerge = RangeImpl.GetCellIndex(leftTopCell.X + 1, leftTopCell.Y + 1) == cellIndex;
      MergeCellsRecord.MergedRegion mergedRegion = mergeCells.FindMergedRegion(new Rectangle(firstColumn - 1, iRowIndex - 1, 0, 0));
      bool flag1 = cells.Contains(cellIndex) && (mergedRegion == null || bMerge);
      bool flag2 = sheet.Comments[iRowIndex, firstColumn] != null;
      if (flag1 || flag2 || bMerge)
      {
        writer.WriteStartElement("Cell", (string) null);
        if (num + 1 != firstColumn)
          writer.WriteAttributeString("ss", "Index", (string) null, firstColumn.ToString());
        num = firstColumn;
        bool bFormatted = this.DisableFormatting(writer);
        if (sheet.HasHyperlinks)
          this.SerializeHyperlink(writer, cellIndex, hyperLinks);
        this.SerializeCellStyle(writer, cellIndex, bMerge, cells, sheet);
        this.SerializeMerge(writer, iRowIndex, firstColumn, mergeCells, bMerge);
        if (flag1 && cells.GetCellRecord(cellIndex).TypeCode != TBIFFRecord.Blank)
          this.SerializeData(writer, cells, cellIndex);
        if (flag2)
          this.SerializeComment(writer, (IComment) sheet.Comments[iRowIndex, firstColumn], sheet.ParentWorkbook.InnerFonts, cells.GetCellFont(cellIndex));
        writer.WriteEndElement();
        this.EnableFormatting(writer, bFormatted);
      }
    }
  }

  private void SerializeMerge(
    XmlWriter writer,
    int iRowIndex,
    int i,
    MergeCellsImpl mergeCells,
    bool bMerge)
  {
    if (!bMerge)
      return;
    Rectangle rect = Rectangle.FromLTRB(i - 1, iRowIndex - 1, i - 1, iRowIndex - 1);
    this.SerializeMergedRange(writer, mergeCells[rect]);
  }

  private void SerializeHyperlink(XmlWriter writer, long index, HyperLinksCollection hyperLinks)
  {
    IHyperLink hyperlinkByCellIndex = hyperLinks.GetHyperlinkByCellIndex(index);
    if (hyperlinkByCellIndex == null)
      return;
    string str = hyperlinkByCellIndex.Type == ExcelHyperLinkType.Workbook ? "#" + hyperlinkByCellIndex.Address : hyperlinkByCellIndex.Address;
    writer.WriteAttributeString("ss", "HRef", (string) null, str);
    if (hyperlinkByCellIndex.ScreenTip == null || hyperlinkByCellIndex.ScreenTip.Length == 0)
      return;
    writer.WriteAttributeString("x", "HRefScreenTip", (string) null, hyperlinkByCellIndex.ScreenTip);
  }

  private void SerializeCellStyle(
    XmlWriter writer,
    long index,
    bool bMerge,
    CellRecordCollection cells,
    WorksheetImpl sheet)
  {
    int num = cells.GetExtendedFormatIndex(index);
    if (bMerge)
      num = this.m_mergeStyles[WorkbookXmlSerializator.GetUniqueID(sheet.Index, index)];
    WorkbookImpl parentWorkbook = sheet.ParentWorkbook;
    if (num == parentWorkbook.DefaultXFIndex || num == 0 || num == int.MinValue)
      return;
    writer.WriteAttributeString("ss", "StyleID", (string) null, "s" + num.ToString());
  }

  private void EnableFormatting(XmlWriter writer, bool bFormatted)
  {
    if (!bFormatted || !(writer is XmlTextWriter xmlTextWriter))
      return;
    xmlTextWriter.Formatting = Formatting.Indented;
  }

  private bool DisableFormatting(XmlWriter writer)
  {
    bool flag = false;
    if (writer is XmlTextWriter xmlTextWriter)
    {
      flag = xmlTextWriter.Formatting != Formatting.None;
      xmlTextWriter.Formatting = Formatting.None;
    }
    return flag;
  }

  private void SerializeData(XmlWriter writer, CellRecordCollection cells, long index)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (cells == null)
      throw new ArgumentNullException(nameof (cells));
    NumberFormatInfo numberFormat = CultureInfo.InvariantCulture.NumberFormat;
    string strFormula = cells.GetFormula(index, true, numberFormat);
    IStyle style = (IStyle) null;
    TextWithFormat rtf = (TextWithFormat) null;
    WorksheetImpl sheet = (WorksheetImpl) cells.Sheet;
    string cellTypeValue;
    WorkbookXmlSerializator.XmlSerializationCellType type;
    if (strFormula != null && strFormula.Length > 0)
    {
      strFormula = this.UpdateFormulaError(strFormula);
      writer.WriteAttributeString("ss", "Formula", (string) null, strFormula);
      type = this.GetFormulaType((IWorksheet) sheet, index, out cellTypeValue);
    }
    else
    {
      cellTypeValue = this.GetCellTypeValue(cells, index, out type);
      if (type == WorkbookXmlSerializator.XmlSerializationCellType.String)
      {
        style = cells.GetCellStyle(index);
        rtf = sheet.GetTextWithFormat(index);
      }
    }
    if (!(strFormula != "=#REF!"))
      return;
    this.SerializeData(writer, type, cellTypeValue, style, rtf, cells, index);
  }

  private WorkbookXmlSerializator.XmlSerializationCellType GetFormulaType(
    IWorksheet sheet,
    long index,
    out string value)
  {
    int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(index);
    int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(index);
    IRange range = sheet[rowFromCellIndex, columnFromCellIndex];
    WorkbookXmlSerializator.XmlSerializationCellType formulaType;
    if (range.HasFormulaBoolValue)
    {
      formulaType = WorkbookXmlSerializator.XmlSerializationCellType.Boolean;
      value = XmlConvert.ToString(range.FormulaBoolValue);
    }
    else if (range.HasFormulaDateTime)
    {
      formulaType = WorkbookXmlSerializator.XmlSerializationCellType.DateTime;
      value = XmlConvert.ToString(range.FormulaDateTime, "yyyy-MM-ddTHH:mm:ss");
    }
    else if (range.HasFormulaErrorValue)
    {
      formulaType = WorkbookXmlSerializator.XmlSerializationCellType.Error;
      value = range.FormulaErrorValue;
    }
    else if ((value = range.FormulaStringValue) != null)
    {
      formulaType = WorkbookXmlSerializator.XmlSerializationCellType.String;
    }
    else
    {
      formulaType = WorkbookXmlSerializator.XmlSerializationCellType.Number;
      double formulaNumberValue = range.FormulaNumberValue;
      value = !double.IsNaN(formulaNumberValue) ? XmlConvert.ToString(formulaNumberValue) : (string) null;
    }
    return formulaType;
  }

  private void SerializeData(
    XmlWriter writer,
    WorkbookXmlSerializator.XmlSerializationCellType cellType,
    string value,
    IStyle style,
    TextWithFormat rtf,
    CellRecordCollection cells,
    long cellIndex)
  {
    if (value == null || cellType == WorkbookXmlSerializator.XmlSerializationCellType.String && value.Length == 0)
      return;
    writer.WriteStartElement("Data", (string) null);
    bool flag = false;
    writer.WriteAttributeString("ss", "Type", (string) null, cellType.ToString());
    if (cellType == WorkbookXmlSerializator.XmlSerializationCellType.String && value.Length != 0)
    {
      if (style != null && style.IsFirstSymbolApostrophe)
        writer.WriteAttributeString("x", "Ticked", (string) null, "1");
      if (rtf != null && rtf.FormattingRunsCount != 0)
      {
        WorksheetImpl sheet = (WorksheetImpl) cells.Sheet;
        writer.WriteAttributeString("xmlns", (string) null, (string) null, "http://www.w3.org/TR/REC-html40");
        IFont cellFont = cells.GetCellFont(cellIndex);
        FontsCollection innerFonts = sheet.ParentWorkbook.InnerFonts;
        this.SerializeRichText(writer, rtf, value, innerFonts, cellFont);
        flag = true;
      }
    }
    if (!flag)
    {
      if (cellType == WorkbookXmlSerializator.XmlSerializationCellType.Boolean)
        value = XmlConvertExtension.ToBoolean(value) ? "1" : "0";
      writer.WriteString(value);
    }
    writer.WriteEndElement();
  }

  private void SerializeWorksheets(XmlWriter writer, IWorksheets worksheets)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (worksheets == null)
      throw new ArgumentNullException(nameof (worksheets));
    int Index = 0;
    for (int count = worksheets.Count; Index < count; ++Index)
      this.SerializeWorksheet(writer, (WorksheetImpl) worksheets[Index]);
  }

  private void SerializeWorksheet(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    writer.WriteStartElement("ss", "Worksheet", (string) null);
    writer.WriteAttributeString("ss", "Name", (string) null, sheet.Name);
    if (sheet.IsRightToLeft)
      writer.WriteAttributeString("ss", "RightToLeft", (string) null, "1");
    IAutoFilters autoFilters = sheet.AutoFilters;
    DataValidationTable dvTable = sheet.DVTable;
    INames names = sheet.Names;
    if (names.Count > 0)
      this.SerializeNames(writer, names, true);
    this.SerializeTable(writer, sheet);
    if (autoFilters.Count > 0)
      this.SerializeAutoFilters(writer, autoFilters);
    if (dvTable.Count > 0)
      this.SerializeDataValidations(writer, dvTable);
    WorksheetConditionalFormats conditionalFormats = sheet.ConditionalFormats;
    if (conditionalFormats.Count > 0)
      this.SerializeConditionFormats(writer, conditionalFormats);
    this.SerializeWorksheetOption(writer, sheet);
    writer.WriteEndElement();
  }

  private void SerializeTable(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    writer.WriteStartElement("ss", "Table", (string) null);
    if (sheet.StandardHeight != 12.75)
      writer.WriteAttributeString("ss", "DefaultRowHeight", (string) null, XmlConvert.ToString(sheet.StandardHeight));
    float num = (float) sheet.Application.ConvertUnits((double) sheet.ColumnWidthToPixels(sheet.StandardWidth), MeasureUnits.Pixel, MeasureUnits.Point);
    if ((double) num != 48.0)
      writer.WriteAttributeString("ss", "DefaultColumnWidth", (string) null, XmlConvert.ToString(num));
    this.SerializeColumns(writer, sheet);
    this.SerializeRows(writer, sheet);
    writer.WriteEndElement();
  }

  private void SerializeColumns(XmlWriter writer, WorksheetImpl worksheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ColumnInfoRecord[] columnInfoRecordArray = worksheet != null ? worksheet.ColumnInformation : throw new ArgumentNullException(nameof (worksheet));
    for (int index = 1; index <= columnInfoRecordArray.Length - 1; ++index)
    {
      ColumnInfoRecord record = columnInfoRecordArray[index];
      if (record != null)
      {
        int num1 = (int) record.FirstColumn + 1;
        if (num1 <= 256 /*0x0100*/)
        {
          writer.WriteStartElement("ss", "Column", (string) null);
          writer.WriteAttributeString("ss", "Index", (string) null, num1.ToString());
          float pixels = (float) worksheet.ColumnWidthToPixels((double) record.ColumnWidth / 256.0);
          int num2 = (int) record.LastColumn - (int) record.FirstColumn;
          this.SerializeRowColumnCommonAttributes(writer, (IOutline) record, (int) record.LastColumn, worksheet.ParentWorkbook);
          if (num2 > 0)
            writer.WriteAttributeString("ss", "Span", (string) null, num2.ToString());
          writer.WriteAttributeString("ss", "AutoFitWidth", (string) null, "0");
          if ((double) pixels != worksheet.StandardWidth)
          {
            float num3 = (float) worksheet.Application.ConvertUnits((double) pixels, MeasureUnits.Pixel, MeasureUnits.Point);
            writer.WriteAttributeString("ss", "Width", (string) null, XmlConvert.ToString(num3));
          }
          writer.WriteEndElement();
        }
      }
    }
  }

  private void SerializeRows(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    CellRecordCollection recordCollection = sheet != null ? sheet.CellRecords : throw new ArgumentNullException(nameof (sheet));
    int num = 0;
    int firstRow = sheet.FirstRow;
    for (int lastRow = sheet.LastRow; firstRow <= lastRow; ++firstRow)
    {
      if (recordCollection.ContainsRow(firstRow - 1))
      {
        writer.WriteStartElement("Row");
        if (num + 1 != firstRow)
          writer.WriteAttributeString("ss", "Index", (string) null, firstRow.ToString());
        num = firstRow;
        RowStorage row = recordCollection.Table.Rows[firstRow - 1];
        if (row != null)
        {
          writer.WriteAttributeString("ss", "Height", (string) null, XmlConvert.ToString((double) row.Height / 20.0));
          writer.WriteAttributeString("ss", "AutoFitHeight", (string) null, "0");
          this.SerializeRowColumnCommonAttributes(writer, (IOutline) row, row.LastColumn, sheet.ParentWorkbook);
        }
        this.SerializeCell(writer, sheet, firstRow);
        writer.WriteEndElement();
      }
    }
  }

  private void SerializeRowColumnCommonAttributes(
    XmlWriter writer,
    IOutline record,
    int iLastIndex,
    WorkbookImpl book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    int num = record != null ? (int) record.ExtendedFormatIndex : throw new ArgumentNullException(nameof (record));
    if (num != book.DefaultXFIndex && num != 0)
      writer.WriteAttributeString("ss", "StyleID", (string) null, "s" + record.ExtendedFormatIndex.ToString());
    if (!record.IsHidden && !record.IsCollapsed)
      return;
    writer.WriteAttributeString("ss", "Hidden", (string) null, "1");
  }

  private void SerializeMergedRange(XmlWriter writer, MergeCellsRecord.MergedRegion region)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (region == null)
      throw new ArgumentNullException(nameof (region));
    int num1 = region.RowTo - region.RowFrom;
    int num2 = region.ColumnTo - region.ColumnFrom;
    writer.WriteAttributeString("ss", "MergeDown", (string) null, num1.ToString());
    writer.WriteAttributeString("ss", "MergeAcross", (string) null, num2.ToString());
  }

  private void SerializeComment(
    XmlWriter writer,
    IComment comment,
    FontsCollection fonts,
    IFont defFont)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (comment == null)
      throw new ArgumentNullException(nameof (comment));
    if (fonts == null)
      throw new ArgumentNullException(nameof (fonts));
    writer.WriteStartElement("ss", "Comment", (string) null);
    string author = comment.Author;
    string text = comment.Text;
    if (author.Length != 0)
      writer.WriteAttributeString("ss", "Author", (string) null, author);
    if ((comment as ShapeImpl).IsShapeVisible)
      writer.WriteAttributeString("ss", "ShowAlways", (string) null, "1");
    writer.WriteStartElement("ss", "Data", (string) null);
    writer.WriteAttributeString("xmlns", (string) null, (string) null, "http://www.w3.org/TR/REC-html40");
    if (text.Length != 0)
    {
      TextWithFormat textObject = ((RichTextString) comment.RichText).TextObject;
      if (textObject.FormattingRuns.Count > 0)
        this.SerializeRichText(writer, textObject, text, fonts, defFont);
      else
        writer.WriteString(comment.Text);
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeRichText(
    XmlWriter writer,
    TextWithFormat rtf,
    string text,
    FontsCollection fonts,
    IFont defFont)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (rtf == null)
      throw new ArgumentNullException(nameof (rtf));
    if (fonts == null)
      throw new ArgumentNullException("Fonts");
    if (text.Length == 0)
      throw new ArgumentNullException(nameof (text));
    SortedList<int, int> formattingRuns = rtf.FormattingRuns;
    IList<int> values = formattingRuns.Values;
    IList<int> keys = formattingRuns.Keys;
    if (formattingRuns.Count <= 0)
      return;
    int num1 = keys[0];
    int length = text.Length;
    if (num1 != 0)
      this.SerializeRtfFont(writer, defFont, text.Substring(0, num1));
    int index1 = 0;
    for (int count = formattingRuns.Count; index1 < count; ++index1)
    {
      int index2 = values[index1];
      int num2 = keys[index1];
      IFont font = fonts[index2];
      int num3 = count - index1 == 1 ? length : keys[index1 + 1];
      this.SerializeRtfFont(writer, font, text.Substring(num1, num3 - num1));
      num1 = num3;
    }
  }

  private void SerializeRtfFont(XmlWriter writer, IFont RTFFont, string strValue)
  {
    if (writer == null)
      throw new ArgumentNullException();
    if (RTFFont == null)
      throw new ArgumentNullException("rtfString");
    if (strValue.Length == 0)
      return;
    StringBuilder startBuilder = this.GetStartBuilder();
    StringBuilder endBuilder = this.GetEndBuilder();
    if (RTFFont.Bold)
      this.AddTagToString("<B>", "</B>", startBuilder, endBuilder);
    if (RTFFont.Italic)
      this.AddTagToString("<I>", "</I>", startBuilder, endBuilder);
    if (RTFFont.Underline == ExcelUnderline.Single)
      this.AddTagToString("<U>", "</U>", startBuilder, endBuilder);
    if (RTFFont.Strikethrough)
      this.AddTagToString("<S>", "</S>", startBuilder, endBuilder);
    if (RTFFont.Subscript)
      this.AddTagToString("<Sub>", "</Sub>", startBuilder, endBuilder);
    if (RTFFont.Superscript)
      this.AddTagToString("<Sup>", "</Sup>", startBuilder, endBuilder);
    startBuilder.Append("<Font");
    endBuilder.Insert(0, "</Font>");
    this.AddAttributeToString("x:Color", this.GetColorString(RTFFont.RGBColor), startBuilder, endBuilder);
    this.AddAttributeToString("x:Face", RTFFont.FontName, startBuilder, endBuilder);
    this.AddAttributeToString("x:Size", RTFFont.Size.ToString(), startBuilder, endBuilder);
    startBuilder.Append('>');
    writer.WriteRaw(startBuilder.ToString());
    strValue = strValue.Replace("&", "&amp;");
    strValue = strValue.Replace("\n", "&#10;");
    writer.WriteRaw(strValue);
    writer.WriteRaw(endBuilder.ToString());
  }

  private void SerializeDataValidations(XmlWriter writer, DataValidationTable dvTable)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dvTable == null)
      throw new ArgumentNullException(nameof (dvTable));
    int index1 = 0;
    for (int count = dvTable.Count; index1 < count; ++index1)
    {
      dvTable.Worksheet.DVTable.Add((DValRecord) BiffRecordFactory.GetRecord(TBIFFRecord.DVal)).AddDVRecord((DVRecord) BiffRecordFactory.GetRecord(TBIFFRecord.DV));
      this.m_formulaUtil = new FormulaUtil(dvTable.Application, (object) dvTable.Workbook, NumberFormatInfo.InvariantInfo, ',', ';');
      DataValidationCollection validationCollection = dvTable[index1];
      for (int index2 = 0; index2 < validationCollection.Count; ++index2)
      {
        IDataValidation dv = (IDataValidation) validationCollection[index2];
        if ((dv as DataValidationImpl).DVRanges.Length > 0)
        {
          writer.WriteStartElement("x", "DataValidation", (string) null);
          this.SerializeDataValidation(writer, dv);
          writer.WriteEndElement();
        }
      }
    }
  }

  private void SerializeDataValidation(XmlWriter writer, IDataValidation dv)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    DataValidationImpl dataValidation = dv != null ? (DataValidationImpl) dv : throw new ArgumentNullException(nameof (dv));
    this.SerializeRanges(writer, dataValidation);
    this.SerializeFormulas(writer, dataValidation);
    if (dataValidation.ErrorStyle != ExcelErrorStyle.Stop)
    {
      writer.WriteStartElement("x", "ErrorStyle", (string) null);
      writer.WriteString(this.ConvertDataValidationErrorStyle(dataValidation.ErrorStyle));
      writer.WriteEndElement();
    }
    if (dataValidation.PromptBoxTitle != null && dataValidation.PromptBoxTitle.Length > 0)
    {
      writer.WriteStartElement("x", "InputTitle", (string) null);
      writer.WriteString(dataValidation.PromptBoxTitle);
      writer.WriteEndElement();
    }
    if (dataValidation.PromptBoxText != null && dataValidation.PromptBoxText.Length > 0)
    {
      writer.WriteStartElement("x", "InputMessage", (string) null);
      writer.WriteString(dataValidation.PromptBoxText);
      writer.WriteEndElement();
    }
    if (dataValidation.ErrorBoxTitle != null && dataValidation.ErrorBoxTitle.Length > 0)
    {
      writer.WriteStartElement("x", "ErrorTitle", (string) null);
      writer.WriteString(dataValidation.ErrorBoxTitle);
      writer.WriteEndElement();
    }
    if (dataValidation.ErrorBoxTitle != null && dataValidation.ErrorBoxTitle.Length > 0)
    {
      writer.WriteStartElement("x", "ErrorMessage", (string) null);
      writer.WriteString(dataValidation.ErrorBoxText);
      writer.WriteEndElement();
    }
    if (dataValidation.AllowType != ExcelDataType.User)
      return;
    writer.WriteStartElement("x", "CellRangeList", (string) null);
    writer.WriteEndElement();
  }

  private void SerializeRanges(XmlWriter writer, DataValidationImpl dataValidation)
  {
    string[] dvRanges = dataValidation.DVRanges;
    if (dvRanges == null)
      throw new ArgumentNullException("strRange");
    int iFirstRow = 0;
    int iFirstColumn = 0;
    int iLastRow = 0;
    int iLastColumn = 0;
    writer.WriteStartElement("x", "Range", (string) null);
    foreach (string range in dvRanges)
    {
      RangeImpl.ParseRangeString(range, (IWorkbook) dataValidation.Workbook, out iFirstRow, out iFirstColumn, out iLastRow, out iLastColumn);
      string str = RangeImpl.GetAddressLocal(iFirstRow, iFirstColumn, iLastRow, iLastColumn, true) + ",";
      if (dvRanges[dvRanges.Length - 1] != range)
        writer.WriteString(str.ToString());
      else
        writer.WriteString(str.Substring(0, str.Length - 1));
    }
    writer.WriteEndElement();
  }

  private void SerializeFormulas(XmlWriter writer, DataValidationImpl dataValidation)
  {
    ExcelDataType allowType = dataValidation.AllowType;
    ExcelDataValidationComparisonOperator compareOperator = dataValidation.CompareOperator;
    string firstSecondFormula1 = dataValidation.GetR1C1FirstSecondFormula(this.m_formulaUtil, true);
    string firstSecondFormula2 = dataValidation.GetR1C1FirstSecondFormula(this.m_formulaUtil, false);
    if (allowType == ExcelDataType.Any)
      return;
    writer.WriteStartElement("x", "Type", (string) null);
    writer.WriteString(this.ConvertDataValidationType(dataValidation.AllowType));
    writer.WriteEndElement();
    switch (allowType)
    {
      case ExcelDataType.User:
      case ExcelDataType.Formula:
        string text = firstSecondFormula1.Replace(char.MinValue, ',');
        writer.WriteStartElement("x", "Value", (string) null);
        writer.WriteString(text);
        writer.WriteEndElement();
        break;
      default:
        writer.WriteStartElement("x", "Qualifier", (string) null);
        writer.WriteString(dataValidation.CompareOperator.ToString());
        writer.WriteEndElement();
        if (compareOperator != ExcelDataValidationComparisonOperator.Between && compareOperator != ExcelDataValidationComparisonOperator.NotBetween)
        {
          writer.WriteStartElement("x", "Value", (string) null);
          writer.WriteString(firstSecondFormula1);
          writer.WriteEndElement();
          break;
        }
        writer.WriteStartElement("x", "Min", (string) null);
        writer.WriteString(firstSecondFormula1);
        writer.WriteEndElement();
        writer.WriteStartElement("x", "Max", (string) null);
        writer.WriteString(firstSecondFormula2);
        writer.WriteEndElement();
        break;
    }
  }

  private void SerializeConditionFormats(XmlWriter writer, WorksheetConditionalFormats conditions)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (conditions == null)
      throw new ArgumentNullException(nameof (conditions));
    int i = 0;
    for (int count = conditions.Count; i < count; ++i)
    {
      ConditionalFormats condition = conditions[i];
      writer.WriteStartElement("x", "ConditionalFormatting", (string) null);
      writer.WriteStartElement("x", "Range", (string) null);
      writer.WriteString(condition.AddressR1C1);
      writer.WriteEndElement();
      IConditionalFormats format = (IConditionalFormats) condition;
      this.SerializeConditionFormat(writer, format);
      writer.WriteEndElement();
    }
  }

  private void SerializeConditionFormat(XmlWriter writer, IConditionalFormats format)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    int index = 0;
    for (int count = format.Count; index < count; ++index)
    {
      IConditionalFormat condition = format[index];
      this.SerializeCondition(writer, condition);
    }
  }

  private void SerializeCondition(XmlWriter writer, IConditionalFormat condition)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (condition == null)
      throw new ArgumentNullException(nameof (condition));
    writer.WriteStartElement("x", "Condition", (string) null);
    string firstFormulaR1C1 = condition.FirstFormulaR1C1;
    string secondFormulaR1C1 = condition.SecondFormulaR1C1;
    if (condition.FormatType == ExcelCFType.CellValue)
    {
      writer.WriteStartElement("x", "Qualifier", (string) null);
      writer.WriteString(condition.Operator.ToString());
      writer.WriteEndElement();
    }
    if (firstFormulaR1C1 != null && firstFormulaR1C1.Length > 0)
    {
      writer.WriteStartElement("x", "Value1", (string) null);
      writer.WriteString(firstFormulaR1C1);
      writer.WriteEndElement();
    }
    if (secondFormulaR1C1 != null && secondFormulaR1C1.Length > 0)
    {
      writer.WriteStartElement("x", "Value2", (string) null);
      writer.WriteString(secondFormulaR1C1);
      writer.WriteEndElement();
    }
    writer.WriteStartElement("x", "Format", (string) null);
    writer.WriteAttributeString("x", "Style", (string) null, this.GetConditionaFormatString(condition));
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeWorksheetOption(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    writer.WriteStartElement("x", "WorksheetOptions", (string) null);
    if (sheet.Visibility != WorksheetVisibility.Visible)
    {
      int visibility = (int) sheet.Visibility;
      this.WriteElement(writer, "x", "Visible", WorkbookXmlSerializator.DEF_VISIBLE_STRING[visibility]);
    }
    this.SerializeWindowTwoProperties(writer, sheet);
    PageSetupImpl pageSetup = (PageSetupImpl) sheet.PageSetup;
    if (pageSetup.IsFitToPage)
      this.WriteElement(writer, "x", "FitToPage");
    int tabColor = (int) sheet.TabColor;
    if (tabColor != -1)
      this.WriteElement(writer, "x", "TabColorIndex", tabColor.ToString());
    if (sheet.Zoom != 100)
      this.WriteElement(writer, "x", "Zoom", sheet.Zoom.ToString());
    this.SerializePageSetup(writer, (IPageSetup) pageSetup);
    this.SerializePanes(writer, sheet);
    if (!pageSetup.IsNotValidSettings)
      this.SerializePrint(writer, (IPageSetup) pageSetup);
    writer.WriteEndElement();
  }

  private void SerializePageSetup(XmlWriter writer, IPageSetup pageSetup)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    PageSetupImpl pageSetup1 = pageSetup != null ? (PageSetupImpl) pageSetup : throw new ArgumentNullException(nameof (pageSetup));
    writer.WriteStartElement("x", "PageSetup", (string) null);
    this.SerializeHeaderFooter(writer, pageSetup1, true);
    this.SerializeHeaderFooter(writer, pageSetup1, false);
    this.SerializeLayout(writer, pageSetup1);
    this.SerializePageMargins(writer, pageSetup1);
    writer.WriteEndElement();
  }

  private void SerializeHeaderFooter(XmlWriter writer, PageSetupImpl pageSetup, bool isFooter)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pageSetup == null)
      throw new ArgumentNullException(nameof (pageSetup));
    double num = isFooter ? pageSetup.FooterMargin : pageSetup.HeaderMargin;
    bool flag1 = num != 0.5;
    string str = isFooter ? pageSetup.FullFooterString : pageSetup.FullHeaderString;
    bool flag2 = str != null && str.Length > 0;
    if (!flag1 && !flag2)
      return;
    string localName = isFooter ? "Footer" : "Header";
    writer.WriteStartElement("x", localName, (string) null);
    writer.WriteAttributeString("x", "Margin", (string) null, XmlConvert.ToString(num));
    if (flag2)
      writer.WriteAttributeString("x", "Data", (string) null, str);
    writer.WriteEndElement();
  }

  private void SerializeLayout(XmlWriter writer, PageSetupImpl pageSetup)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ExcelPageOrientation excelPageOrientation = pageSetup != null ? pageSetup.Orientation : throw new ArgumentNullException(nameof (pageSetup));
    writer.WriteStartElement("x", "Layout", (string) null);
    if (!pageSetup.AutoFirstPageNumber)
      writer.WriteAttributeString("x", "StartPageNumber", (string) null, pageSetup.FirstPageNumber.ToString());
    if (excelPageOrientation != ExcelPageOrientation.Portrait)
      writer.WriteAttributeString("x", "Orientation", (string) null, excelPageOrientation.ToString());
    if (pageSetup.CenterHorizontally)
      writer.WriteAttributeString("x", "CenterHorizontal", (string) null, "1");
    if (pageSetup.CenterVertically)
      writer.WriteAttributeString("x", "CenterVertical", (string) null, "1");
    writer.WriteEndElement();
  }

  private void SerializePageMargins(XmlWriter writer, PageSetupImpl pageSetup)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pageSetup == null)
      throw new ArgumentNullException(nameof (pageSetup));
    writer.WriteStartElement("x", "PageMargins", (string) null);
    double rightMargin = pageSetup.RightMargin;
    double leftMargin = pageSetup.LeftMargin;
    double topMargin = pageSetup.TopMargin;
    double bottomMargin = pageSetup.BottomMargin;
    if (rightMargin != 0.75)
      writer.WriteAttributeString("x", "Right", (string) null, XmlConvert.ToString(rightMargin));
    if (leftMargin != 0.75)
      writer.WriteAttributeString("x", "Left", (string) null, XmlConvert.ToString(leftMargin));
    if (bottomMargin != 1.0)
      writer.WriteAttributeString("x", "Bottom", (string) null, XmlConvert.ToString(bottomMargin));
    if (topMargin != 1.0)
      writer.WriteAttributeString("x", "Top", (string) null, XmlConvert.ToString(topMargin));
    writer.WriteEndElement();
  }

  private void SerializePanes(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    PaneRecord paneRecord = sheet != null ? sheet.Pane : throw new ArgumentNullException(nameof (sheet));
    List<SelectionRecord> selections = sheet.Selections;
    if (selections != null)
    {
      if (selections.Count > 1)
        this.WriteElement(writer, "x", "ActivePane", paneRecord.ActivePane.ToString());
      this.SerializeSelectionPane(writer, selections);
    }
    if (paneRecord == null)
      return;
    int horizontalSplit = paneRecord.HorizontalSplit;
    int verticalSplit = paneRecord.VerticalSplit;
    if (sheet.WindowTwo.TopRow != (ushort) 0)
      this.WriteElement(writer, "x", "TopRowVisible", sheet.WindowTwo.TopRow.ToString());
    if (horizontalSplit != 0)
    {
      this.WriteElement(writer, "x", "SplitHorizontal", horizontalSplit.ToString());
      this.WriteElement(writer, "x", "TopRowBottomPane", paneRecord.FirstRow.ToString());
    }
    if (verticalSplit != 0)
    {
      this.WriteElement(writer, "x", "SplitVertical", verticalSplit.ToString());
      this.WriteElement(writer, "x", "LeftColumnRightPane", paneRecord.FirstColumn.ToString());
    }
    WindowTwoRecord windowTwo = sheet.WindowTwo;
    if (windowTwo.IsFreezePanes)
      this.WriteElement(writer, "x", "FreezePanes");
    if (!windowTwo.IsFreezePanesNoSplit)
      return;
    this.WriteElement(writer, "x", "FrozenNoSplit");
  }

  private void SerializeSelectionPane(XmlWriter writer, List<SelectionRecord> arrSelection)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    int num = arrSelection != null ? arrSelection.Count : throw new ArgumentNullException(nameof (arrSelection));
    if (num > 4)
      throw new ArgumentOutOfRangeException("Array cannot contain more than 4 selection records");
    writer.WriteStartElement("x", "Panes", (string) null);
    for (int index = 0; index < num; ++index)
    {
      SelectionRecord selectionRecord = arrSelection[index];
      writer.WriteStartElement("x", "Pane", (string) null);
      this.WriteElement(writer, "x", "Number", selectionRecord.Pane.ToString());
      if (selectionRecord.ColumnActiveCell != (ushort) 0)
        this.WriteElement(writer, "x", "ActiveCol", selectionRecord.ColumnActiveCell.ToString());
      if (selectionRecord.RowActiveCell != (ushort) 0)
        this.WriteElement(writer, "x", "ActiveRow", selectionRecord.RowActiveCell.ToString());
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializePrint(XmlWriter writer, IPageSetup pageSetup)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    int num = pageSetup != null ? (int) pageSetup.PaperSize : throw new ArgumentNullException(nameof (pageSetup));
    writer.WriteStartElement("x", "Print", (string) null);
    if (pageSetup.Copies != 1)
      this.WriteElement(writer, "x", "NumberofCopies", pageSetup.Copies.ToString());
    if (pageSetup.PrintQuality <= (int) short.MaxValue)
      this.WriteElement(writer, "x", "HorizontalResolution", pageSetup.PrintQuality.ToString());
    if (pageSetup.PaperSize != ExcelPaperSize.PaperLetter)
      this.WriteElement(writer, "x", "PaperSizeIndex", num.ToString());
    if (pageSetup.IsFitToPage)
    {
      if (pageSetup.FitToPagesWide != 1)
        this.WriteElement(writer, "x", "FitWidth", pageSetup.FitToPagesWide.ToString());
      if (pageSetup.FitToPagesTall != 1)
        this.WriteElement(writer, "x", "FitHeight", pageSetup.FitToPagesTall.ToString());
    }
    else if (pageSetup.Zoom != 100)
      this.WriteElement(writer, "x", "Scale", pageSetup.Zoom.ToString());
    if (pageSetup.PrintGridlines)
      this.WriteElement(writer, "x", "Gridlines");
    if (pageSetup.BlackAndWhite)
      this.WriteElement(writer, "x", "BlackAndWhite");
    if (pageSetup.Draft)
      this.WriteElement(writer, "x", "DraftQuality");
    if (pageSetup.PrintHeadings)
      this.WriteElement(writer, "x", "RowColHeadings");
    if (pageSetup.PrintComments != ExcelPrintLocation.PrintNoComments)
    {
      int printComments = (int) pageSetup.PrintComments;
      this.WriteElement(writer, "x", "CommentsLayout", WorkbookXmlSerializator.DEF_PRINT_LOCATION_STRING[printComments]);
    }
    if (pageSetup.PrintErrors != ExcelPrintErrors.PrintErrorsDisplayed)
    {
      int printErrors = (int) pageSetup.PrintErrors;
      this.WriteElement(writer, "x", "PrintErrors", WorkbookXmlSerializator.DEF_PRINT_ERROR_STRING[printErrors]);
    }
    if (pageSetup.Order == ExcelOrder.OverThenDown)
      this.WriteElement(writer, "x", "LeftToRight");
    writer.WriteEndElement();
  }

  private void SerializeWindowTwoProperties(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    WindowTwoRecord windowTwoRecord = sheet != null ? sheet.WindowTwo : throw new ArgumentNullException(nameof (sheet));
    if (windowTwoRecord == null)
      return;
    if (!windowTwoRecord.IsDisplayGridlines)
      this.WriteElement(writer, "x", "DoNotDisplayGridlines");
    if (!windowTwoRecord.IsDisplayRowColHeadings)
      this.WriteElement(writer, "x", "DoNotDisplayHeadings");
    if (!windowTwoRecord.IsSelected)
      return;
    this.WriteElement(writer, "x", "Selected");
  }

  private void SerializeExcelWorkbook(XmlWriter writer, WorkbookImpl book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    writer.WriteStartElement("x", "ExcelWorkbook", (string) null);
    int index = book.ActiveSheet.Index;
    int count = book.WorksheetGroup.Count;
    if (index > 0)
    {
      this.WriteElement(writer, "x", "ActiveSheet", index.ToString());
      this.WriteElement(writer, "x", "FirstVisibleSheet", book.DisplayedTab.ToString());
    }
    if (count > 1)
      this.WriteElement(writer, "x", "SelectedSheets", count.ToString());
    if (!book.ShowSheetTabs)
      this.WriteElement(writer, "x", "HideWorkbookTabs", string.Empty);
    writer.WriteEndElement();
  }

  private void SerializeDocumentProperties(XmlWriter writer, IWorkbook book)
  {
    throw new NotImplementedException();
  }

  private void SerializeWorkbook(XmlWriter writer, IWorkbook book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    this.m_mergeStyles.Clear();
    writer.WriteStartElement("Workbook", (string) null);
    writer.WriteAttributeString("xmlns", (string) null, (string) null, "urn:schemas-microsoft-com:office:spreadsheet");
    writer.WriteAttributeString("xmlns", "ss", (string) null, "urn:schemas-microsoft-com:office:spreadsheet");
    writer.WriteAttributeString("xmlns", "x", (string) null, "urn:schemas-microsoft-com:office:excel");
    writer.WriteAttributeString("xmlns", "o", (string) null, "urn:schemas-microsoft-com:office:office");
    writer.WriteAttributeString("xmlns", "html", (string) null, "http://www.w3.org/TR/REC-html40");
    List<ExtendedFormatImpl> mergedList = this.GetMergedList(book.Worksheets);
    if (book.Styles.Count > 0)
      this.SerializeStyles(writer, ((WorkbookImpl) book).InnerExtFormats, mergedList);
    this.SerializeExcelWorkbook(writer, (WorkbookImpl) book);
    this.SerializeNames(writer, book.Names, false);
    this.SerializeWorksheets(writer, book.Worksheets);
    this.m_mergeStyles.Clear();
    writer.WriteEndElement();
  }

  public void Serialize(XmlWriter writer, IWorkbook book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    writer.WriteRaw("<?xml version=\"1.0\"?>");
    writer.WriteRaw("<?mso-application progid=\"Excel.Sheet\"?>");
    this.SerializeWorkbook(writer, book);
  }

  private StringBuilder GetStartBuilder() => this.InitializeBuilder(ref this.m_builderStart);

  private StringBuilder GetEndBuilder() => this.InitializeBuilder(ref this.m_builderEnd);

  private StringBuilder InitializeBuilder(ref StringBuilder builder)
  {
    if (builder == null)
      builder = new StringBuilder();
    else
      builder.Length = 0;
    return builder;
  }

  private string GetColorString(Color col)
  {
    return "#" + (col.ToArgb() & 16777215 /*0xFFFFFF*/).ToString("X6");
  }

  private void AddTagToString(
    string strOpenTag,
    string strCloseTag,
    StringBuilder builderStart,
    StringBuilder builderEnd)
  {
    if (builderStart == null)
      throw new ArgumentNullException(nameof (builderStart));
    if (builderEnd == null)
      throw new ArgumentNullException(nameof (builderEnd));
    builderStart.Append(strOpenTag);
    builderEnd.Insert(0, strCloseTag);
  }

  private void AddAttributeToString(
    string name,
    string value,
    StringBuilder builderStart,
    StringBuilder builderEnd)
  {
    if (builderStart == null)
      throw new ArgumentNullException(nameof (builderStart));
    if (builderEnd == null)
      throw new ArgumentNullException(nameof (builderEnd));
    builderStart.Append(" ");
    builderStart.Append(name);
    builderStart.Append("=\"");
    builderStart.Append(value);
    builderStart.Append("\" ");
  }

  private string GetConditionaFormatString(IConditionalFormat cond)
  {
    if (cond == null)
      throw new ArgumentNullException(nameof (cond));
    string conditionaFormatString = "";
    if (cond.IsFontFormatPresent)
    {
      if (cond.IsFontColorPresent)
      {
        string colorString = this.GetColorString(cond.FontColorRGB);
        conditionaFormatString = $"{conditionaFormatString}color:{colorString};";
      }
      string str1 = cond.IsBold ? "700" : "400";
      string str2 = $"{conditionaFormatString}font-weight:{str1};";
      if (cond.IsItalic)
        str2 += "font-style:italic;";
      string str3 = cond.Underline.ToString();
      conditionaFormatString = $"{str2}text-underline-style:{str3};";
      if (cond.IsStrikeThrough)
        conditionaFormatString += "text-line-through:single;";
    }
    if (cond.IsPatternFormatPresent)
    {
      if (cond.IsBackgroundColorPresent)
      {
        string colorString = this.GetColorString(cond.BackColorRGB);
        conditionaFormatString = $"{conditionaFormatString}background:{colorString};";
      }
      int fillPattern = (int) cond.FillPattern;
      string str = $"{WorkbookXmlSerializator.DEF_PATTERN_STRING_CF[fillPattern]} {this.GetColorString(cond.ColorRGB)}";
      conditionaFormatString = $"{conditionaFormatString}mso-pattern:{str};";
    }
    if (cond.IsBorderFormatPresent)
    {
      if (cond.IsTopBorderModified)
        conditionaFormatString += this.GetBorderString("top", cond.TopBorderColorRGB, cond.TopBorderStyle);
      if (cond.IsLeftBorderModified)
        conditionaFormatString += this.GetBorderString("left", cond.LeftBorderColorRGB, cond.LeftBorderStyle);
      if (cond.IsBottomBorderModified)
        conditionaFormatString += this.GetBorderString("bottom", cond.BottomBorderColorRGB, cond.BottomBorderStyle);
      if (cond.IsRightBorderModified)
        conditionaFormatString += this.GetBorderString("right", cond.RightBorderColorRGB, cond.RightBorderStyle);
    }
    return conditionaFormatString;
  }

  private string GetBorderString(string strBorder, Color borderCol, ExcelLineStyle style)
  {
    return $"{$"{$"border-{strBorder}:"} {WorkbookXmlSerializator.DEF_BORDER_LINE_CF[(int) style]}"} {this.GetColorString(borderCol)};";
  }

  private string GetStyleHAlignString(ExcelHAlign hAlign)
  {
    return hAlign == ExcelHAlign.HAlignGeneral ? "Automatic" : hAlign.ToString().Remove(0, 6);
  }

  private string GetStyleVAlignString(ExcelVAlign vAlign) => vAlign.ToString().Remove(0, 6);

  private string GetStyleFontAlign(IFont font)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    string styleFontAlign = "None";
    if (font.Subscript)
      styleFontAlign = "Subscript";
    if (font.Superscript)
      styleFontAlign = "Superscript";
    return styleFontAlign;
  }

  private string GetAutoFilterConditionValue(IAutoFilterCondition cond)
  {
    if (cond == null)
      throw new ArgumentNullException(nameof (cond));
    if (cond.DataType == ExcelFilterDataType.String)
      return cond.String;
    if (cond.DataType == ExcelFilterDataType.FloatingPoint)
      return cond.Double.ToString();
    if (cond.DataType == ExcelFilterDataType.ErrorCode)
      return cond.ErrorCode.ToString();
    if (cond.DataType != ExcelFilterDataType.Boolean)
      throw new ArgumentException("Unassigned conditonal type");
    return !cond.Boolean ? "0" : "1";
  }

  private string GetCellTypeValue(
    CellRecordCollection cells,
    long index,
    out WorkbookXmlSerializator.XmlSerializationCellType type)
  {
    string cellTypeValue = cells != null ? cells.GetText(index) : throw new ArgumentNullException("cell");
    if (cellTypeValue != null)
    {
      type = WorkbookXmlSerializator.XmlSerializationCellType.String;
      return cellTypeValue;
    }
    double numberWithoutFormula = cells.GetNumberWithoutFormula(index);
    if (numberWithoutFormula != double.MinValue)
    {
      type = WorkbookXmlSerializator.XmlSerializationCellType.Number;
      return XmlConvert.ToString(numberWithoutFormula);
    }
    DateTime dateTime = cells.GetDateTime(index);
    if (dateTime != DateTime.MinValue)
    {
      type = WorkbookXmlSerializator.XmlSerializationCellType.DateTime;
      return XmlConvert.ToString(dateTime, "yyyy-MM-ddTHH:mm:ss");
    }
    string error = cells.GetError(index);
    if (error != null)
    {
      type = WorkbookXmlSerializator.XmlSerializationCellType.Error;
      return error;
    }
    bool flag;
    if (!cells.GetBool(index, out flag))
      throw new ApplicationException("Cell dosn't contain value");
    type = WorkbookXmlSerializator.XmlSerializationCellType.Boolean;
    return !flag ? "0" : "1";
  }

  private void WriteElement(XmlWriter writer, string strPrefix, string strName)
  {
    this.WriteElement(writer, strPrefix, strName, (string) null);
  }

  private void WriteElement(XmlWriter writer, string strPrefix, string strName, string strValue)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (strName == null || strName.Length == 0)
      throw new ArgumentNullException(nameof (strName));
    if (strPrefix == null || strPrefix.Length == 0)
      throw new ArgumentNullException(nameof (strPrefix));
    writer.WriteStartElement(strPrefix, strName, (string) null);
    if (strValue != null)
      writer.WriteString(strValue);
    writer.WriteEndElement();
  }

  private void ReSerializeStyle(XmlWriter writer, List<ExtendedFormatImpl> list)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    int index = 0;
    for (int count = list.Count; index < count; ++index)
      this.SerializeStyle(writer, list[index]);
  }

  private List<ExtendedFormatImpl> GetMergedList(IWorksheets sheets)
  {
    List<ExtendedFormatImpl> mergedList = new List<ExtendedFormatImpl>();
    if (sheets == null)
      throw new ArgumentNullException(nameof (sheets));
    int Index = 0;
    for (int count1 = sheets.Count; Index < count1; ++Index)
    {
      WorksheetImpl sheet = (WorksheetImpl) sheets[Index];
      MergeCellsImpl mergeCells = sheet.MergeCells;
      IList<ExtendedFormatImpl> mergedExtendedFormats = mergeCells.GetMergedExtendedFormats();
      int index = 0;
      int count2 = mergedExtendedFormats.Count;
      foreach (Rectangle mergedRegion in mergeCells.MergedRegions)
      {
        if (index < count2)
        {
          ExtendedFormatImpl extendedFormatImpl = mergedExtendedFormats[index];
          long cellIndex = RangeImpl.GetCellIndex(mergedRegion.X + 1, mergedRegion.Y + 1);
          long uniqueId = WorkbookXmlSerializator.GetUniqueID(sheet.Index, cellIndex);
          int num = 5000 + this.m_mergeStyles.Count;
          extendedFormatImpl.Index = (int) (ushort) num;
          this.m_mergeStyles.Add(uniqueId, num);
          ++index;
        }
        else
          break;
      }
      mergedList.AddRange((IEnumerable<ExtendedFormatImpl>) mergedExtendedFormats);
    }
    return mergedList;
  }

  private string UpdateFormulaError(string strFormula)
  {
    if (strFormula == null || strFormula.Length == 0)
      throw new ArgumentNullException(nameof (strFormula));
    return !strFormula.EndsWith("#REF") ? strFormula : "=#REF!";
  }

  private string ConvertDataValidationType(ExcelDataType dataValidationType)
  {
    int index = (int) dataValidationType;
    return index <= 0 ? WorkbookXmlSerializator.DEF_ALLOWTYPE_STRING[0] : WorkbookXmlSerializator.DEF_ALLOWTYPE_STRING[index];
  }

  private string ConvertDataValidationErrorStyle(ExcelErrorStyle strErrorStyle)
  {
    int index = (int) strErrorStyle;
    return index <= 0 ? WorkbookXmlSerializator.DEF_ERRORSTYLE[0] : WorkbookXmlSerializator.DEF_ERRORSTYLE[index];
  }

  public enum XmlSerializationCellType
  {
    Number,
    DateTime,
    Boolean,
    String,
    Error,
  }
}
