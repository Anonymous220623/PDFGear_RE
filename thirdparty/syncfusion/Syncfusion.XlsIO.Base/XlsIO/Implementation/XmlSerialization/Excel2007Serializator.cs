// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Excel2007Serializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using Syncfusion.Compression.Zip;
using Syncfusion.Drawing;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.PivotTables;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.Sorting;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Constants;
using Syncfusion.XlsIO.Implementation.XmlSerialization.PivotTables;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

public class Excel2007Serializator : IDisposable
{
  private const int MaximumFormulaLength = 8000;
  public const string XmlFileHeading = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?>";
  public const string ContentTypesNamespace = "http://schemas.openxmlformats.org/package/2006/content-types";
  public const string HyperlinkNamespace = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink";
  public const string RelationNamespace = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
  internal const string StrictOpenXmlRelationNamespace = "http://purl.oclc.org/ooxml/officeDocument/relationships";
  public const string XmlNamespaceMain = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
  public const string WorksheetPartType = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet";
  internal const string MacrosheetPerType = "http://schemas.microsoft.com/office/2006/relationships/xlMacrosheet";
  internal const string IntlMacrosheetPerType = "http://schemas.microsoft.com/office/2006/relationships/xlIntlMacrosheet";
  public const string ChartSheetPartType = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chartsheet";
  public const string DialogSheetPartType = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/dialogsheet";
  public const string ExtendedPropertiesPartType = "http://schemas.openxmlformats.org/officeDocument/2006/extended-properties";
  public const string CorePropertiesPartType = "http://schemas.openxmlformats.org/package/2006/metadata/core-properties";
  public const string X14Namespace = "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main";
  internal const string X12Namespace = "http://schemas.microsoft.com/office/spreadsheetml/2011/1/ac";
  internal const string C15Namespace = "http://schemas.microsoft.com/office/drawing/2012/chart";
  public const string PivotFieldUri = "{E15A36E0-9728-4e99-A89B-3F7291B0FE68}";
  internal const string PivotCacheUri = "{725AE2AE-9491-48be-B2B4-4EB974FC3084}";
  internal const string ChartExtensionUri = "{CE6537A1-D6FC-4f65-9D91-7224C49458BB}";
  public const string ExternListUri = "{962EF5D1-5CA2-4c93-8EF4-DBF5C05439D2}";
  internal const string PivotFieldExternListUri = "{2946ED86-A175-432a-8AC1-64E0C546D7DE}";
  public const string SlicerExtensionUri = "{A8765BA9-456A-4dab-B4F3-ACF838C121DE}";
  public const string X14NameSpaceAttribute = "xmlns";
  public const string HideValuesRowAttribute = "hideValuesRow";
  public const string MSNamespaceMain = "http://schemas.microsoft.com/office/excel/2006/main";
  public const string MSNamespaceMainAttribute = "xmlns";
  public const string XMNamespaceMain = "http://schemas.microsoft.com/office/excel/2006/main";
  public const string XMNamespaceMainAttribute = "xmlns";
  public const string x14PivotTableDefinitionAttributes = "pivotTableDefinition";
  public const string SparklineUri = "{05C60535-1F16-4fd2-B633-F4F36F0B64E0}";
  public const string MCPrefix = "mc";
  public const string MCNamespace = "http://schemas.openxmlformats.org/markup-compatibility/2006";
  public const string CorePropertiesPrefix = "cp";
  public const string DublinCorePartType = "http://purl.org/dc/elements/1.1/";
  public const string DublinCorePrefix = "dc";
  public const string DublinCoreTermsPartType = "http://purl.org/dc/terms/";
  public const string DublinCoreTermsPrefix = "dcterms";
  public const string DCMITypePartType = "http://purl.org/dc/dcmitype/";
  public const string DCMITypePrefix = "dcmitype";
  public const string XSIPartType = "http://www.w3.org/2001/XMLSchema-instance";
  public const string XSIPrefix = "xsi";
  public const string CustomPropertiesPartType = "http://schemas.openxmlformats.org/officeDocument/2006/custom-properties";
  public const string DocPropsVTypesPartType = "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes";
  public const string OleObjectContentType = "application/vnd.openxmlformats-officedocument.oleObject";
  public const string OleObjectFileExtension = "bin";
  public const string DocPropsVTypesPrefix = "vt";
  public const string RelationPrefix = "r";
  public const string X14Prefix = "x14";
  internal const string X12Prefix = "x12ac";
  internal const string C15Prefix = "c15";
  public const string MSPrefix = "xm";
  public const string TypesTagName = "Types";
  public const string ExtensionAttributeName = "Extension";
  public const string DefaultTagName = "Default";
  public const string ContentTypeAttributeName = "ContentType";
  public const string OverrideTagName = "Override";
  public const string PartNameAttributeName = "PartName";
  public const string WorkbookTagName = "workbook";
  public const string SheetsTagName = "sheets";
  internal const string FileSharingTagName = "fileSharing";
  internal const string ReadOnlyRecommendedAttribute = "readOnlyRecommended";
  public const string SheetTagName = "sheet";
  public const string SheetNameAttribute = "name";
  public const string DefaultWorksheetPathFormat = "worksheets/sheet{0}.xml";
  public const string RelationIdFormat = "rId{0}";
  public const string SheetIdAttribute = "sheetId";
  public const string RelationAttribute = "id";
  public const string RelationIdAttribute = "Id";
  public const string SheetStateAttributeName = "state";
  public const string CalcProperties = "calcPr";
  public const string CalculationId = "calcId";
  public const string CalculationMode = "calcMode";
  public const string TabSelected = "tabSelected";
  public const string ManualCalcModeValue = "manual";
  public const string AutoNoTableCalcModeValue = "autoNoTable";
  public const string DEF_DEFAULT_ROW_DELIMITER = "\r\n";
  internal const string LineFeed = "_x000a_";
  public const string StateHidden = "hidden";
  public const string StateVeryHidden = "veryHidden";
  public const string StateVisible = "visible";
  public const string RelationsTagName = "Relationships";
  public const string RelationTagName = "Relationship";
  public const string RelationTypeAttribute = "Type";
  public const string RelationTargetAttribute = "Target";
  public const string RelationTargetModeAttribute = "TargetMode";
  public const string RelationExternalTargetMode = "External";
  public const string MergeCellsXmlTagName = "mergeCells";
  public const string CountAttributeName = "count";
  internal const string DefaultTableStyleTagName = "defaultTableStyle";
  internal const string DefaultPivotStyleTagName = "defaultPivotStyle";
  public const string MergeCellXmlTagName = "mergeCell";
  public const string RefAttributeName = "ref";
  public const string DefinedNamesXmlTagName = "definedNames";
  public const string DefinedNameXmlTagName = "definedName";
  public const string NameAttributeName = "name";
  public const string CommentAttributeName = "comment";
  public const string NameSheetIdAttribute = "localSheetId";
  public const string StyleSheetTagName = "styleSheet";
  public const string SlicerList = "slicerList";
  public const string FontsTagName = "fonts";
  public const string FontTagName = "font";
  public const string FontBoldTagName = "b";
  public const string FontItalicTagName = "i";
  public const string FontUnderlineTagName = "u";
  public const string ValueAttributeName = "val";
  public const string FontSizeTagName = "sz";
  public const string FontStrikeTagName = "strike";
  public const string FontNameTagName = "name";
  public const string ColorTagName = "color";
  public const string ColorIndexedAttributeName = "indexed";
  public const string ColorThemeAttributeName = "theme";
  public const string ColorTintAttributeName = "tint";
  public const string ColorRgbAttribute = "rgb";
  public const string Auto = "auto";
  public const string IndexedColorsTagName = "indexedColors";
  public const string ColorsTagName = "colors";
  public const string RgbColorTagName = "rgbColor";
  public const string MacOSShadowTagName = "shadow";
  public const string FontVerticalAlignmentTagName = "vertAlign";
  public const string FontFamilyTagName = "family";
  public const string FontCharsetTagName = "charset";
  public const string NumberFormatsTagName = "numFmts";
  public const string NumberFormatTagName = "numFmt";
  public const string NumberFormatIdAttributeName = "numFmtId";
  public const string NumberFormatStringAttributeName = "formatCode";
  public const string FillsTagName = "fills";
  public const string FillTagName = "fill";
  public const string PatternFillTagName = "patternFill";
  public const string GradientFillTagName = "gradientFill";
  public const string GradientFillTypeAttributeName = "type";
  public const string GradientFillTypeLinear = "linear";
  public const string GradientFillTypePath = "path";
  public const string LinearGradientDegreeAttributeName = "degree";
  public const string BottomConvergenceAttributeName = "bottom";
  public const string LeftConvergenceAttributeName = "left";
  public const string RightConvergenceAttributeName = "right";
  public const string TopConvergenceAttributeName = "top";
  public const string GradientStopTagName = "stop";
  public const string GradientStopPositionAttributeName = "position";
  public const string PatternAttributeName = "patternType";
  public const string BackgroundColorTagName = "bgColor";
  public const string ForegroundColorTagName = "fgColor";
  public const string BordersTagName = "borders";
  public const string BordersCollectionTagName = "border";
  public const string BorderStyleAttributeName = "style";
  public const string BorderColorTagName = "color";
  public const string WorksheetTagName = "worksheet";
  public const string DimensionTagName = "dimension";
  public const string SheetDataTagName = "sheetData";
  public const string CellTagName = "c";
  public const string CellMetadataIndexAttributeName = "cm";
  public const string ShowPhoneticAttributeName = "ph";
  public const string ReferenceAttributeName = "r";
  public const string StyleIndexAttributeName = "s";
  public const string CellDataTypeAttributeName = "t";
  public const string ValueMetadataIndexAttributeName = "vm";
  public const string FormulaTagName = "f";
  public const string CellValueTagName = "v";
  public const string RichTextInlineTagName = "is";
  public const string RichTextRunPropertiesTagName = "rPr";
  public const string RichTextRunFontTagName = "rFont";
  public const string ColsTagName = "cols";
  public const string ColTagName = "col";
  public const string ColumnMinAttribute = "min";
  public const string ColumnMaxAttribute = "max";
  public const string ColumnWidthAttribute = "width";
  public const string ColumnStyleAttribute = "style";
  public const string ColumnCustomWidthAttribute = "customWidth";
  public const string BestFitAttribute = "bestFit";
  public const string RowTagName = "row";
  public const string RowIndexAttributeName = "r";
  public const string RowHeightAttributeName = "ht";
  public const string RowHiddenAttributeName = "hidden";
  public const string RowCustomFormatAttributeName = "customFormat";
  public const string RowCustomHeightAttributeName = "customHeight";
  public const string RowColumnCollapsedAttribute = "collapsed";
  public const string DyDescent = "x14ac:dyDescent";
  public const string RowColumnOutlineLevelAttribute = "outlineLevel";
  public const string RowThickBottomAttributeName = "thickBot";
  public const string RowThickTopAttributeName = "thickTop";
  public const string FormulaTypeAttributeName = "t";
  public const string AlwaysCalculateArray = "aca";
  public const string SharedGroupIndexAttributeName = "si";
  public const string RangeOfCellsAttributeName = "ref";
  public const string CommentAuthorsTagName = "authors";
  public const string CommentAuthorTagName = "author";
  public const string CommentListTagName = "commentList";
  public const string CommentTagName = "comment";
  public const string CommentTextTagName = "text";
  public const string CommentsTagName = "comments";
  public const string AuthorIdAttributeName = "authorId";
  public const string DefaultCellDataType = "n";
  public const string NamedStyleXFsTagName = "cellStyleXfs";
  public const string CellFormatXFsTagName = "cellXfs";
  public const string DiffXFsTagName = "dxfs";
  public const string TableStylesTagName = "tableStyles";
  internal const string TableStyleTagName = "tableStyle";
  internal const string TableStyleElementTagName = "tableStyleElement";
  internal const string TableStyleNameTagName = "name";
  internal const string TypeAttributeName = "type";
  internal const string SizeTagName = "size";
  internal const string PivotTagName = "pivot";
  internal const string DifferentialFormattingIdAttributeName = "dxfId";
  public const string ExtendedFormatTagName = "xf";
  public const string FontIdAttributeName = "fontId";
  public const string FillIdAttributeName = "fillId";
  public const string BorderIdAttributeName = "borderId";
  public const string XFIdAttributeName = "xfId";
  public const string CellStylesTagName = "cellStyles";
  public const string CellStyleTagName = "cellStyle";
  public const string StyleBuiltinIdAttributeName = "builtinId";
  public const string StyleCustomizedAttributeName = "customBuiltin";
  public const string OutlineLevelAttribute = "iLevel";
  public const string IncludeAlignmentAttributeName = "applyAlignment";
  public const string IncludeBorderAttributeName = "applyBorder";
  public const string IncludeFontAttributeName = "applyFont";
  public const string IncludeNumberFormatAttributeName = "applyNumberFormat";
  public const string IncludePatternsAttributeName = "applyFill";
  public const string IncludeProtectionAttributeName = "applyProtection";
  public const string AlignmentTagName = "alignment";
  public const string ProtectionTagName = "protection";
  public const string IndentAttributeName = "indent";
  public const string HAlignAttributeName = "horizontal";
  public const string JustifyLastLineAttributeName = "justifyLastLine";
  public const string ReadingOrderAttributeName = "readingOrder";
  public const string ShrinkToFitAttributeName = "shrinkToFit";
  public const string TextRotationAttributeName = "textRotation";
  public const string WrapTextAttributeName = "wrapText";
  public const string VerticalAttributeName = "vertical";
  public const string HiddenAttributeName = "hidden";
  public const string LockedAttributeName = "locked";
  public const bool HiddenDefaultValue = false;
  public const bool LockedDefaultValue = true;
  public const string QuotePreffixAttributeName = "quotePrefix";
  public const string DiagonalDownAttributeName = "diagonalDown";
  public const string DiagonalUpAttributeName = "diagonalUp";
  public const string SharedStringTableTagName = "sst";
  public const string UniqueStringCountAttributeName = "uniqueCount";
  public const string StringItemTagName = "si";
  public const string TextTagName = "t";
  public const string RichTextRunTagName = "r";
  public const string TemporaryRoot = "root";
  public const string SpaceAttributeName = "space";
  public const string XmlPrefix = "xml";
  public const string PreserveValue = "preserve";
  private const string CellTypeNumber = "n";
  private const string CellTypeString = "s";
  private const string CellTypeBool = "b";
  private const string CellTypeError = "e";
  private const string CellTypeFormulaString = "str";
  private const string CellTypeInlineString = "inlineStr";
  internal const string SchemeTagName = "scheme";
  internal const string SortState = "sortState";
  internal const string SortCondition = "sortCondition";
  internal const string Descending = "descending";
  internal const string SortBy = "sortBy";
  internal const string CellColor = "cellColor";
  internal const string FontColor = "fontColor";
  public const string ThemeTagName = "theme";
  internal const string ThemeOverrideTagName = "themeOverride";
  public const string ThemeElementsTagName = "themeElements";
  public const string ColorSchemeTagName = "clrScheme";
  public const string RGBHexColorValueAttributeName = "val";
  public const string SystemColorTagName = "sysClr";
  public const string SystemColorValueAttributeName = "val";
  public const string SystemColorLastColorAttributeName = "lastClr";
  public const string DxfFormattingTagName = "dxf";
  public const string PhoneticPr = "phoneticPr";
  public const string Phonetic = "phonetic";
  public const string HyperlinksTagName = "hyperlinks";
  public const string HyperlinkTagName = "hyperlink";
  public const string DisplayStringAttributeName = "display";
  public const string RelationshipIdAttributeName = "id";
  public const string LocationAttributeName = "location";
  public const string HyperlinkReferenceAttributeName = "ref";
  public const string ToolTipAttributeName = "tooltip";
  public const string SheetLevelPropertiesTagName = "sheetPr";
  public const string PageSetupPropertiesTag = "pageSetUpPr";
  public const string FitToPageAttribute = "fitToPage";
  public const string SheetTabColorTagName = "tabColor";
  public const string SheetOutlinePropertiesTagName = "outlinePr";
  public const string SummaryRowBelow = "summaryBelow";
  public const string SummaryColumnRight = "summaryRight";
  public const string BackgroundImageTagName = "picture";
  public const string FileHyperlinkStartString = "file:///";
  public const string HttpStartString = "http://";
  public const string SheetFormatPropertiesTag = "sheetFormatPr";
  public const string ZeroHeightAttribute = "zeroHeight";
  public const string DefaultRowHeightAttribute = "defaultRowHeight";
  public const string DefaultColumWidthAttribute = "defaultColWidth";
  public const string BaseColWidthAttribute = "baseColWidth";
  public const string ThickBottomAttribute = "thickBottom";
  public const string ThickTopAttribute = "thickTop";
  public const string OutlineLevelColAttribute = "outlineLevelCol";
  public const string OutlineLevelRowAttribute = "outlineLevelRow";
  public const string WorkbookViewsTagName = "bookViews";
  public const string WorkbookViewTagName = "workbookView";
  public const string ActiveSheetIndexAttributeName = "activeTab";
  public const string AutoFilterDateGroupingAttributeName = "autoFilterDateGrouping";
  public const string FirstSheetAttributeName = "firstSheet";
  public const string MinimizedAttributeName = "minimized";
  public const string ShowHorizontalScrollAttributeName = "showHorizontalScroll";
  public const string ShowSheetTabsAttributeName = "showSheetTabs";
  public const string ShowVerticalScrollAttributeName = "showVerticalScroll";
  public const string SheetTabRatioAttributeName = "tabRatio";
  public const string VisibilityAttributeName = "visibility";
  public const string WindowHeightAttributeName = "windowHeight";
  public const string WindowWidthAttributeName = "windowWidth";
  public const string UpperLeftCornerXAttributeName = "xWindow";
  public const string UpperLeftCornerYAttributeName = "yWindow";
  public const string HorizontalPageBreaksTagName = "rowBreaks";
  public const string VerticalPageBreaksTagName = "colBreaks";
  public const string PageBreakCountAttributeName = "count";
  public const string ManualBreakCountAttributeName = "manualBreakCount";
  public const string BreakTagName = "brk";
  public const string IdAttributeName = "id";
  public const string ManualPageBreakAttributeName = "man";
  public const string MaximumAttributeName = "max";
  public const string MinimumAttributeName = "min";
  public const string SheetViewsTag = "sheetViews";
  public const string SheetViewTag = "sheetView";
  public const string ShowZeros = "showZeros";
  public const string WorkbookViewIdAttribute = "workbookViewId";
  public const string SheetZoomScale = "zoomScale";
  public const string ViewTag = "view";
  public const string Layout = "pageLayout";
  public const string PageBreakPreview = "pageBreakPreview";
  public const string Normal = "normal";
  public const string TrueValue = "1";
  public const string FalseValue = "0";
  public const string SparklineColumnValue = "column";
  public const string SparklineWinLossValue = "stacked";
  public const string EmptyCellsGapValue = "gap";
  public const string EmptyCellsZeroValue = "zero";
  public const string EmptyCellsLineValue = "span";
  public const string VerticalCustomTypeValue = "custom";
  public const string VerticalSameTypeValue = "group";
  public const string ShowGridLines = "showGridLines";
  public const string RightToLeft = "rightToLeft";
  public const string SheetGridColor = "defaultGridColor";
  public const string ColorID = "colorId";
  public const string CustomPropertiesTagName = "customProperties";
  public const string CustomPropertyTagName = "customPr";
  public const string IgnoredErrorsTag = "ignoredErrors";
  public const string IgnoredErrorTag = "ignoredError";
  public const string OnCall = "OLEUPDATE_ONCALL";
  public const string Always = "OLEUPDATE_ALWAYS";
  public const string RangeReferenceAttribute = "sqref";
  public const string FileVersionTag = "fileVersion";
  public const string RupBuild = "rupBuild";
  public const string LastEdited = "lastEdited";
  public const string LowestEdited = "lowestEdited";
  public const string WorkbookPr = "workbookPr";
  public const string WorkbookDate1904 = "date1904";
  public const string WorkbookPrecision = "fullPrecision";
  public const string ApplicationNameAttribute = "appName";
  public const string ApplicationNameValue = "xl";
  private const string WindowProtection = "windowProtection";
  public const string FunctionGroups = "functionGroups";
  public const string CodeName = "codeName";
  public const string HidePivotFieldList = "hidePivotFieldList";
  private const string SpansTag = "spans";
  public const string Extensionlist = "extLst";
  public const string Extension = "ext";
  public const string CalculateOnOpen = "ca";
  public const string dataTable2DTag = "dt2D";
  public const string dataTableRowTag = "dtr";
  public const string dataTableCellTag = "r1";
  internal const string SecondDataTableCellTag = "r2";
  private const int FirstVisibleChar = 32 /*0x20*/;
  public const string TransitionEvaluation = "transitionEvaluation";
  public const string ShowRowColHeaders = "showRowColHeaders";
  private const string VersionValue = "12.0000";
  public const string Properties = "properties";
  public const string DocumentManagement = "documentManagement";
  public const string PCPrefix = "pc";
  public const string PPrefix = "p";
  public const string PropertiesNameSpace = "http://schemas.microsoft.com/office/2006/metadata/properties";
  public const string PartnerControlsNameSpace = "http://schemas.microsoft.com/office/infopath/2007/PartnerControls";
  public const string DefaultThemeVersion = "defaultThemeVersion";
  public const string ConnectionsTag = "connections";
  public const string ConnectionTag = "connection";
  public const string OdbcFileAttribute = "odcFile";
  public const string DataBaseNameAttribute = "name";
  public const string DataBaseTypeAttribute = "type";
  public const string RefreshedVersionAttribute = "refreshedVersion";
  public const string BackGroundAttribute = "background";
  public const string SaveData = "saveData";
  public const string DataBasePrTag = "dbPr";
  public const string CommandTextAttribute = "command";
  public const string CommandTypeAttribute = "commandType";
  public const string ConnectionIdAttribute = "id";
  public const string SourceFile = "sourceFile";
  public const string DescriptionTag = "description";
  public const string Interval = "interval";
  public const string SavePassword = "savePassword";
  public const string OnlyUseConnectionFile = "onlyUseConnectionFile";
  public const string BackgroundRefresh = "backgroundRefresh";
  public const string Credentials = "credentials";
  public const string Deleted = "deleted";
  public const string TextPr = "textPr";
  public const string WebPrTag = "webPr";
  public const string Xml = "xml";
  public const string URL = "url";
  public const string OlapPrTag = "olapPr";
  public const string PivotButton = "pivotButton";
  public const string CustomWorkbookViews = "customWorkbookViews";
  public const string CustomWorkbookView = "customWorkbookView";
  internal const string ParamertersTag = "parameters";
  internal const string ParamerterTag = "parameter";
  internal const string ParamerterTypeAttribute = "parameterType";
  internal const string SqlTypeAttribute = "sqlType";
  internal const string PromptAttribute = "prompt";
  internal const string BooleanAttribute = "boolean";
  internal const string CellAttribute = "cell";
  internal const string DoubleAttribute = "double";
  internal const string IntegerAttribute = "integer";
  internal const string StringAttribute = "string";
  internal const string RefreshOnChangeAttribute = "refreshOnChange";
  public const string CustomXmlPartName = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/customXml";
  public const string CustomXmlName = "customXml/item{0}.xml";
  public const string CustomXmlPropertiesName = "customXml/itemProps{0}.xml";
  public const string CustomXmlRelation = "{0}/_rels/item{1}.xml.rels";
  public const string DataStoreItem = "datastoreItem";
  public const string ItemIdAttribute = "ds:itemID";
  public const string CustomXmlSchemaReferences = "schemaRefs";
  public const string CustomXmlSchemaReference = "schemaRef";
  public const string CustomXmlUriAttribute = "ds:uri";
  public const string XmlItemName = "item{0}.xml";
  public const string XmlPropertiesName = "itemProps{0}.xml";
  public const string CustomXmlNameSpace = "http://schemas.openxmlformats.org/officeDocument/2006/customXml";
  public const string ItemPropertiesPrefix = "ds";
  public const string CustomXmlItemID = "itemID";
  public const string ItemPrpertiesUri = "uri";
  public const string CustomXmlItemPropertiesRelation = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/customXmlProps";
  public static ExcelIgnoreError[] ErrorsSequence = new ExcelIgnoreError[7]
  {
    ExcelIgnoreError.EmptyCellReferences,
    ExcelIgnoreError.EvaluateToError,
    ExcelIgnoreError.InconsistentFormula,
    ExcelIgnoreError.NumberAsText,
    ExcelIgnoreError.OmittedCells,
    ExcelIgnoreError.TextDate,
    ExcelIgnoreError.UnlockedFormulaCells
  };
  private static string[] s_arrFormulas = new string[12]
  {
    "if lineDrawn pixelLineWidth 0",
    "sum @0 1 0",
    "sum 0 0 @1",
    "prod @2 1 2",
    "prod @3 21600 pixelWidth",
    "prod @3 21600 pixelHeight",
    "sum @0 0 1",
    "prod @6 1 2",
    "prod @7 21600 pixelWidth",
    "sum @8 21600 0",
    "prod @7 21600 pixelHeight",
    "sum @10 21600 0"
  };
  private static string s_color = "windowText [{0}]";
  public static string[] ErrorTagsSequence = new string[7]
  {
    "emptyCellReference",
    "evalError",
    "formula",
    "numberStoredAsText",
    "formulaRange",
    "twoDigitTextYear",
    "unlockedFormula"
  };
  private static readonly char[] allowedChars = new char[3]
  {
    '\n',
    '\r',
    '\t'
  };
  internal WorkbookImpl m_book;
  internal FormulaUtil m_formulaUtil;
  private RecordExtractor m_recordExtractor;
  private Dictionary<int, ShapeSerializator> m_shapesVmlSerializators = new Dictionary<int, ShapeSerializator>();
  private static object m_object = new object();
  private Dictionary<int, ShapeSerializator> m_shapesHFVmlSerializators = new Dictionary<int, ShapeSerializator>();
  private Dictionary<Type, ShapeSerializator> m_shapesSerializators = new Dictionary<Type, ShapeSerializator>();
  private List<Stream> m_streamsSheetsCF;
  private List<Stream> m_streamsTableStyles;
  private List<Stream> m_streamsSheetsSort;
  private List<Stream> m_colorFilterStreamList;
  private int m_colorFilterStreamListIndex = -1;
  private List<Stream> m_streamsFiltersSort;
  private List<Dictionary<int, Stream>> m_streamsTableFiltersSort;
  private List<string> m_sheetNames;
  private WorksheetImpl m_worksheetImpl;
  private bool hasTextRotation;
  private char[] SpecialChars = new char[23]
  {
    '�',
    '�',
    '�',
    '#',
    '%',
    '(',
    ')',
    '-',
    '+',
    '.',
    ';',
    '=',
    '^',
    '`',
    '|',
    '~',
    '�',
    '�',
    '�',
    '�',
    '�',
    '�',
    '�'
  };
  private int m_sheetPropertyIndex;

  public Excel2007Serializator(WorkbookImpl book)
  {
    this.m_book = book != null ? book : throw new ArgumentNullException(nameof (book));
    this.m_formulaUtil = new FormulaUtil(this.m_book.Application, (object) this.m_book, NumberFormatInfo.InvariantInfo, ',', ';');
    this.m_recordExtractor = new RecordExtractor();
    this.m_shapesVmlSerializators.Add(202, (ShapeSerializator) new CommentShapeSerializator());
    this.m_shapesVmlSerializators.Add(201, (ShapeSerializator) new VmlFormControlsSerializator());
    this.m_shapesVmlSerializators.Add(75, (ShapeSerializator) new VmlBitmapSerializator());
    this.m_shapesHFVmlSerializators.Add(75, (ShapeSerializator) new HFImageSerializator());
    this.m_shapesSerializators.Add(typeof (BitmapShapeImpl), (ShapeSerializator) new BitmapShapeSerializator());
    this.m_shapesSerializators.Add(typeof (ChartShapeImpl), (ShapeSerializator) new ChartShapeSerializator());
    this.m_shapesSerializators.Add(typeof (TextBoxShapeImpl), (ShapeSerializator) new TextBoxSerializator());
  }

  public void Dispose()
  {
    this.m_shapesVmlSerializators.Clear();
    this.m_shapesVmlSerializators = (Dictionary<int, ShapeSerializator>) null;
    foreach (ShapeSerializator shapeSerializator in this.m_shapesSerializators.Values)
      shapeSerializator.Clear();
    foreach (HFImageSerializator imageSerializator in this.HFVmlSerializators.Values)
      imageSerializator.Clear();
    this.m_shapesSerializators.Clear();
    this.m_shapesSerializators = (Dictionary<Type, ShapeSerializator>) null;
    this.m_shapesHFVmlSerializators.Clear();
    this.m_shapesHFVmlSerializators = (Dictionary<int, ShapeSerializator>) null;
    if (this.m_sheetNames == null)
      return;
    this.m_sheetNames.Clear();
    this.m_sheetNames = (List<string>) null;
  }

  public Dictionary<int, ShapeSerializator> HFVmlSerializators => this.m_shapesHFVmlSerializators;

  public Dictionary<int, ShapeSerializator> VmlSerializators => this.m_shapesVmlSerializators;

  public virtual ExcelVersion Version => ExcelVersion.Excel2007;

  internal WorksheetImpl Worksheet
  {
    get => this.m_worksheetImpl;
    set => this.m_worksheetImpl = value;
  }

  public void SerializeContentTypes(
    XmlWriter writer,
    IDictionary<string, string> contentDefaults,
    IDictionary<string, string> contentOverrides)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (contentDefaults == null)
      throw new ArgumentNullException(nameof (contentDefaults));
    if (contentOverrides == null)
      throw new ArgumentNullException(nameof (contentOverrides));
    writer.WriteStartDocument(true);
    writer.WriteStartElement("Types", "http://schemas.openxmlformats.org/package/2006/content-types");
    this.SerializeDictionary(writer, contentDefaults, "Default", "Extension", "ContentType", (IFileNamePreprocessor) null);
    bool flag = false;
    foreach (KeyValuePair<string, string> contentOverride in (IEnumerable<KeyValuePair<string, string>>) contentOverrides)
    {
      if (contentOverride.Key.Contains("xl/styles.xml"))
      {
        if (contentOverride.Key.Split('/').Length > 3 && flag)
        {
          contentOverrides.Remove(contentOverride.Key);
          break;
        }
        flag = true;
      }
    }
    this.SerializeDictionary(writer, contentOverrides, "Override", "PartName", "ContentType", (IFileNamePreprocessor) new AddSlashPreprocessor());
    writer.WriteEndElement();
  }

  public void SerializeWorkbook(
    XmlWriter writer,
    Stream streamStart,
    Stream streamEnd,
    List<Dictionary<string, string>> lstBookViews,
    RelationCollection relations,
    Dictionary<PivotCacheImpl, string> cacheFiles,
    Stream functionGroups,
    List<Dictionary<string, string>> lstCustomBookViews)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    this.m_sheetPropertyIndex = 0;
    writer.WriteStartElement("workbook", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    this.SerializeFileVersion(writer, this.m_book.DataHolder.FileVersion);
    if (this.m_book.HasFileSharing)
      this.SerializeFileSharing(writer);
    this.SerializeWorkbookPr(writer);
    this.SerializeWorkbookProtection(writer);
    this.SerializeBookViews(writer, lstBookViews);
    this.SerializeSheets(writer);
    this.SerializeStream(writer, functionGroups);
    this.SerializeBookExternalLinks(writer, relations);
    this.SerializeNamedRanges(writer);
    this.SerializeCalculation(writer);
    this.SerializeCustomBookViews(writer, lstCustomBookViews);
    this.SerializePivotCaches(writer, cacheFiles, relations);
    this.SerializeStream(writer, streamEnd);
    writer.WriteEndElement();
  }

  private void SerializeCalculation(XmlWriter writer)
  {
    writer.WriteStartElement("calcPr");
    bool flag = !this.m_book.PrecisionAsDisplayed;
    Excel2007Serializator.SerializeAttribute(writer, "fullPrecision", flag, !flag);
    writer.WriteAttributeString("calcId", this.m_book.DataHolder.CalculationId);
    if (this.m_book.CalculationOptions.CalculationMode != ExcelCalculationMode.Automatic)
    {
      switch (this.m_book.CalculationOptions.CalculationMode)
      {
        case ExcelCalculationMode.Manual:
          writer.WriteAttributeString("calcMode", "manual");
          break;
        case ExcelCalculationMode.AutomaticExceptTables:
          writer.WriteAttributeString("calcMode", "autoNoTable");
          break;
      }
    }
    writer.WriteEndElement();
  }

  private void SerializeWorkbookPr(XmlWriter writer)
  {
    int num = this.m_book.Date1904 ? 1 : 0;
    writer.WriteStartElement("workbookPr");
    Excel2007Serializator.SerializeAttribute(writer, "date1904", this.m_book.Date1904, false);
    if (this.m_book.CodeName != null)
      writer.WriteAttributeString("codeName", this.m_book.CodeName);
    string defaultThemeVersion = this.m_book.DefaultThemeVersion;
    if (defaultThemeVersion != null && defaultThemeVersion.Length > 0)
      writer.WriteAttributeString("defaultThemeVersion", defaultThemeVersion);
    else if (this.m_book.IsCreated)
    {
      if (this.m_book.Version == ExcelVersion.Xlsx)
        writer.WriteAttributeString("defaultThemeVersion", "166925");
      else if (this.m_book.Version == ExcelVersion.Excel2016)
        writer.WriteAttributeString("defaultThemeVersion", "164011");
      else if (this.m_book.Version == ExcelVersion.Excel2013)
        writer.WriteAttributeString("defaultThemeVersion", "153222");
      else
        writer.WriteAttributeString("defaultThemeVersion", "124226");
    }
    if (!this.m_book.HidePivotFieldList)
      writer.WriteAttributeString("hidePivotFieldList", "1");
    writer.WriteEndElement();
  }

  private void SerializePivotCaches(
    XmlWriter writer,
    Dictionary<PivotCacheImpl, string> cacheFiles,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (cacheFiles == null)
      throw new ArgumentNullException(nameof (cacheFiles));
    if (relations == null)
      throw new ArgumentNullException(nameof (relations));
    PivotCacheCollection pivotCaches = this.m_book.PivotCaches;
    int count = pivotCaches != null ? pivotCaches.Count : 0;
    FileDataHolder dataHolder = this.m_book.DataHolder;
    if (count <= 0 && dataHolder.PreservedCaches.Count <= 0)
      return;
    writer.WriteStartElement("pivotCaches");
    foreach (KeyValuePair<string, string> preservedCach in dataHolder.PreservedCaches)
      this.SerializePivotCache(writer, preservedCach.Key, preservedCach.Value);
    if (count > 0)
    {
      foreach (PivotCacheImpl key in pivotCaches)
      {
        string cacheFile = cacheFiles[key];
        string relationId = relations.GenerateRelationId();
        this.SerializePivotCache(writer, key.Index.ToString(), relationId);
        relations[relationId] = new Relation('/'.ToString() + cacheFile, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/pivotCacheDefinition");
      }
    }
    writer.WriteEndElement();
  }

  private void SerializePivotCache(XmlWriter writer, string cacheId, string relationId)
  {
    writer.WriteStartElement("pivotCache");
    writer.WriteAttributeString(nameof (cacheId), cacheId);
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
    writer.WriteEndElement();
  }

  private void SerializeFileVersion(XmlWriter writer, FileVersion fileVersion)
  {
    writer.WriteStartElement(nameof (fileVersion));
    if (fileVersion.ApplicationName != null)
      writer.WriteAttributeString("appName", fileVersion.ApplicationName);
    if (fileVersion.LastEdited != null)
      writer.WriteAttributeString("lastEdited", fileVersion.LastEdited);
    if (fileVersion.LowestEdited != null)
      writer.WriteAttributeString("lowestEdited", fileVersion.LowestEdited);
    if (fileVersion.BuildVersion != null)
      writer.WriteAttributeString("rupBuild", fileVersion.BuildVersion);
    if (fileVersion.CodeName != null)
      writer.WriteAttributeString("codeName", fileVersion.CodeName);
    writer.WriteEndElement();
  }

  private void SerializeFileSharing(XmlWriter writer)
  {
    writer.WriteStartElement("fileSharing");
    Excel2007Serializator.SerializeAttribute(writer, "userName", this.m_book.Author, string.Empty);
    if (this.m_book.AlgorithmName != null)
      Excel2007Serializator.SerializeAttribute(writer, "algorithmName", this.m_book.AlgorithmName, string.Empty);
    if (this.m_book.HashValue != null)
      Excel2007Serializator.SerializeAttribute(writer, "hashValue", Convert.ToBase64String(this.m_book.HashValue), string.Empty);
    if (this.m_book.SaltValue != null)
      Excel2007Serializator.SerializeAttribute(writer, "saltValue", Convert.ToBase64String(this.m_book.SaltValue), string.Empty);
    if (this.m_book.SpinCount != 0U)
      Excel2007Serializator.SerializeAttribute(writer, "spinCount", Convert.ToString(this.m_book.SpinCount), string.Empty);
    Excel2007Serializator.SerializeBool(writer, "readOnlyRecommended", this.m_book.ReadOnlyRecommended);
    writer.WriteEndElement();
  }

  private void SerializeWorkbookProtection(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (!this.m_book.IsWindowProtection && !this.m_book.IsCellProtection)
      return;
    writer.WriteStartElement("workbookProtection");
    PasswordRecord password = this.m_book.Password;
    if (password != null && password.IsPassword != (ushort) 0)
      writer.WriteAttributeString("workbookPassword", password.IsPassword.ToString("X4"));
    if (this.m_book.AlgorithmName != null)
      Excel2007Serializator.SerializeAttribute(writer, "workbookAlgorithmName", this.m_book.AlgorithmName, string.Empty);
    if (this.m_book.HashValue != null)
      Excel2007Serializator.SerializeAttribute(writer, "workbookHashValue", Convert.ToBase64String(this.m_book.HashValue), string.Empty);
    if (this.m_book.SaltValue != null)
      Excel2007Serializator.SerializeAttribute(writer, "workbookSaltValue", Convert.ToBase64String(this.m_book.SaltValue), string.Empty);
    if (this.m_book.SpinCount != 0U)
      Excel2007Serializator.SerializeAttribute(writer, "workbookSpinCount", Convert.ToString(this.m_book.SpinCount), string.Empty);
    Excel2007Serializator.SerializeAttribute(writer, "lockStructure", this.m_book.IsCellProtection, false);
    Excel2007Serializator.SerializeAttribute(writer, "lockWindows", this.m_book.IsWindowProtection, false);
    writer.WriteEndElement();
  }

  public void SerializeMerges(XmlWriter writer, MergeCellsImpl mergedCells)
  {
    if (mergedCells == null)
      return;
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    HashSet<Rectangle> mergedRegions = mergedCells.MergedRegions;
    if (mergedRegions == null || mergedRegions.Count == 0)
      return;
    writer.WriteStartElement("mergeCells");
    writer.WriteAttributeString("count", mergedRegions.Count.ToString());
    foreach (Rectangle rect in mergedRegions)
    {
      MergeCellsRecord.MergedRegion mergeRegion = mergedCells.RectangleToMergeRegion(rect);
      writer.WriteStartElement("mergeCell");
      writer.WriteAttributeString("ref", this.GetRangeName(mergeRegion));
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  public void SerializeNamedRanges(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    WorkbookNamesCollection innerNamesColection = this.m_book.InnerNamesColection;
    if ((innerNamesColection != null ? innerNamesColection.Count : 0) <= 0)
      return;
    innerNamesColection.SortForSerialization();
    writer.WriteStartElement("definedNames");
    foreach (NameImpl nameImpl in (CollectionBase<IName>) innerNamesColection)
    {
      if (nameImpl != null && !nameImpl.Record.IsFunctionOrCommandMacro && !nameImpl.m_isTableNamedRange && !nameImpl.m_isFormulaNamedRange && !nameImpl.IsDeleted)
        this.SerializeNamedRange(writer, (IName) nameImpl);
    }
    writer.WriteEndElement();
  }

  public void SerializeCustomXmlPartProperty(XmlWriter writer, ICustomXmlPart customXmlPart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    string str1 = customXmlPart != null ? customXmlPart.Id : throw new ArgumentNullException(nameof (customXmlPart));
    ICustomXmlSchemaCollection schemas = customXmlPart.Schemas;
    writer.WriteStartDocument(true);
    writer.WriteStartElement("ds", "datastoreItem", "http://schemas.openxmlformats.org/officeDocument/2006/customXml");
    writer.WriteAttributeString("ds", "itemID", (string) null, str1);
    if (schemas != null && schemas.Count > 0)
    {
      writer.WriteStartElement("ds", "schemaRefs", (string) null);
      foreach (string str2 in (IEnumerable) schemas)
      {
        writer.WriteStartElement("ds", "schemaRef", (string) null);
        writer.WriteAttributeString("ds", "uri", (string) null, str2);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  public Dictionary<int, int> SerializeStyles(XmlWriter writer, ref Stream streamDxfs)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartDocument(true);
    writer.WriteStartElement("styleSheet", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer.WriteAttributeString("xmlns", "mc", (string) null, "http://schemas.openxmlformats.org/markup-compatibility/2006");
    writer.WriteAttributeString("Ignorable", "http://schemas.openxmlformats.org/markup-compatibility/2006", "x14ac");
    writer.WriteAttributeString("xmlns", "x14ac", (string) null, "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
    this.SerializeNumberFormats(writer);
    this.SerializeFonts(writer);
    int[] arrFillIndexes = this.SerializeFills(writer);
    int[] arrBorderIndexes = this.SerializeBorders(writer);
    int count = this.m_book.InnerExtFormats.Count;
    Dictionary<int, int> hashNewParentIndexes = this.SerializeNamedStyleXFs(writer, arrFillIndexes, arrBorderIndexes);
    Dictionary<int, int> dictionary1 = this.SerializeNotNamedXFs(writer, arrFillIndexes, arrBorderIndexes, hashNewParentIndexes);
    this.SerializeStyles(writer, hashNewParentIndexes);
    if (streamDxfs == null || this.m_book.DataHolder.ParsedDxfsCount != int.MinValue || this.m_book.DataSorter != null || this.Worksheet != null && this.Worksheet.AutoFilters.Count > 0)
    {
      Stream stream1 = (Stream) new MemoryStream();
      int parsedDxfsCount = this.m_book.DataHolder.ParsedDxfsCount != int.MinValue ? this.m_book.DataHolder.ParsedDxfsCount : 0;
      if (streamDxfs != null)
        this.GetTextRotation(streamDxfs);
      if (streamDxfs != null)
        stream1 = streamDxfs;
      this.m_streamsSheetsCF = new List<Stream>();
      this.m_streamsSheetsSort = new List<Stream>();
      this.m_streamsFiltersSort = new List<Stream>();
      this.m_colorFilterStreamList = new List<Stream>();
      this.m_streamsTableFiltersSort = new List<Dictionary<int, Stream>>();
      foreach (WorksheetImpl innerWorksheet in (CollectionBase<IWorksheet>) this.m_book.InnerWorksheets)
      {
        if (!innerWorksheet.ParseOnDemand || !innerWorksheet.ParseDataOnDemand)
        {
          bool flag1 = innerWorksheet.DataHolder != null;
          if ((this.m_book.IsCreated ? 1 : (flag1 && (innerWorksheet.DataHolder.m_cfStream == null || innerWorksheet.DataHolder.m_cfStream.Length == 0L) || innerWorksheet.CondFmtPos >= 0 ? 1 : (!innerWorksheet.m_parseCondtionalFormats ? 1 : 0))) != 0 || !flag1 && innerWorksheet.ConditionalFormats.Count > 0 || this.m_book.TableStyles.Count != 0)
          {
            MemoryStream memoryStream = new MemoryStream();
            bool flag2 = innerWorksheet.DataHolder != null && innerWorksheet.DataHolder.m_cfStream != null;
            if (flag2)
              memoryStream = new MemoryStream(innerWorksheet.DataHolder.m_cfStream.ToArray());
            WorksheetConditionalFormats conditionalFormats = innerWorksheet.ConditionalFormats;
            if (conditionalFormats.Count == 0 && flag2)
              this.m_streamsSheetsCF.Add((Stream) memoryStream);
            else
              this.m_streamsSheetsCF.Add(this.SerializeDxfs(ref stream1, conditionalFormats, ref parsedDxfsCount));
          }
          else
            this.m_streamsSheetsCF.Add((Stream) null);
          this.m_streamsSheetsSort.Add(this.SerializeDxfsColorFilterAndSorting(ref stream1, innerWorksheet.DataSorter, (AutoFilterImpl) null, ref parsedDxfsCount));
          this.m_streamsFiltersSort.Add(this.SerializeDxfsColorFilterAndSorting(ref stream1, (innerWorksheet.AutoFilters as AutoFiltersCollection).DataSorter, (AutoFilterImpl) null, ref parsedDxfsCount));
          Dictionary<int, Stream> dictionary2 = new Dictionary<int, Stream>();
          foreach (IListObject listObject in (IEnumerable<IListObject>) innerWorksheet.ListObjects)
          {
            if (listObject.ShowAutoFilter)
            {
              Stream stream2 = this.SerializeDxfsColorFilterAndSorting(ref stream1, (listObject.AutoFilters as AutoFiltersCollection).DataSorter, (AutoFilterImpl) null, ref parsedDxfsCount);
              dictionary2.Add(listObject.Index, stream2);
              this.ColorFilterStream(listObject.AutoFilters as AutoFiltersCollection, ref stream1, ref parsedDxfsCount);
            }
          }
          this.m_streamsTableFiltersSort.Add(dictionary2);
          this.ColorFilterStream(innerWorksheet.AutoFilters as AutoFiltersCollection, ref stream1, ref parsedDxfsCount);
          for (int index = 0; index < innerWorksheet.PivotTables.Count; ++index)
          {
            PivotTableImpl pivotTable = innerWorksheet.PivotTables[0] as PivotTableImpl;
            Stream stream3 = this.SerializeDxfsPivotCellFormats(ref stream1, pivotTable, ref parsedDxfsCount);
            if (pivotTable.PivotFormatsStream == null)
              pivotTable.PivotFormatsStream = stream3;
          }
        }
        else
        {
          this.m_streamsFiltersSort.Add((Stream) null);
          this.m_streamsSheetsCF.Add((Stream) null);
          this.m_streamsSheetsSort.Add((Stream) null);
          this.m_streamsTableFiltersSort.Add((Dictionary<int, Stream>) null);
        }
      }
      if (this.m_book.TableStyles != null)
        this.m_book.CustomTableStylesStream = this.SerializeDxfsTableStyles(ref stream1, this.m_book.TableStyles, ref parsedDxfsCount);
      this.m_book.BookCFPriorityCount = 0;
      if (stream1.Length > 0L)
        streamDxfs = stream1;
    }
    this.SerializeStream(writer, streamDxfs);
    this.SerializeStream(writer, this.m_book.CustomTableStylesStream);
    this.SerializeColors(writer);
    Stream extensionStream = this.m_book.DataHolder.ExtensionStream;
    if (extensionStream != null)
    {
      extensionStream.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, extensionStream, true);
    }
    writer.WriteEndElement();
    return dictionary1;
  }

  private void ColorFilterStream(
    AutoFiltersCollection autoFiltersCollection,
    ref Stream tempStreamDxfx,
    ref int iDxfIndex)
  {
    foreach (AutoFilterImpl autoFilters in (CollectionBase<object>) autoFiltersCollection)
    {
      if (autoFilters.IsColorFilter)
        this.m_colorFilterStreamList.Add(this.SerializeDxfsColorFilterAndSorting(ref tempStreamDxfx, (IDataSort) null, autoFilters, ref iDxfIndex));
    }
  }

  private int GetTextRotation(Stream streamDxfs)
  {
    XmlDocument xmlDocument = new XmlDocument();
    XmlReader reader = UtilityMethods.CreateReader(streamDxfs);
    xmlDocument.Load(reader);
    int textRotation = 0;
    if (xmlDocument.DocumentElement.Name == "dxfs" && xmlDocument.InnerXml.Contains("textRotation"))
    {
      XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("dxf");
      for (int i1 = 0; i1 < elementsByTagName.Count; ++i1)
      {
        for (int i2 = 0; i2 < elementsByTagName[i1].ChildNodes.Count; ++i2)
        {
          for (int i3 = 0; i3 < elementsByTagName[i1].ChildNodes[i2].Attributes.Count; ++i3)
          {
            if (elementsByTagName[i1].ChildNodes[i2].Attributes[i3].Name == "textRotation")
              ++textRotation;
          }
        }
      }
    }
    return textRotation;
  }

  public void SerializeRelations(
    XmlWriter writer,
    RelationCollection relations,
    WorksheetDataHolder holder)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (relations == null || relations.Count == 0)
      return;
    writer.WriteStartDocument(true);
    writer.WriteStartElement("Relationships", "http://schemas.openxmlformats.org/package/2006/relationships");
    foreach (KeyValuePair<string, Relation> relation in relations)
      this.SerializeRelation(writer, relation.Key, relation.Value);
    writer.WriteEndElement();
  }

  public void SerializeWorksheet(
    XmlWriter writer,
    WorksheetImpl sheet,
    Stream streamStart,
    Stream streamConFormats,
    Dictionary<int, int> hashXFIndexes,
    Stream streamExtCondFormats)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (streamStart == null)
      throw new ArgumentNullException(nameof (streamStart));
    this.m_worksheetImpl = sheet;
    writer.WriteStartDocument(true);
    writer.WriteStartElement("worksheet", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    writer.WriteAttributeString("xmlns", "x14", (string) null, "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    writer.WriteAttributeString("xmlns", "mc", (string) null, "http://schemas.openxmlformats.org/markup-compatibility/2006");
    writer.WriteAttributeString("xmlns", "xdr", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    this.SerializeSheetlevelProperties(writer, sheet);
    this.SerializeDimensions(writer, sheet);
    this.SerializeSheetViews(writer, sheet);
    writer.WriteStartElement("sheetFormatPr");
    if (sheet.DefaultColumnWidth != 8.43)
      writer.WriteAttributeString("defaultColWidth", XmlConvert.ToString(sheet.DefaultColumnWidth));
    else if (sheet.StandardWidth != 8.43 && !sheet.HasBaseColWidth)
      writer.WriteAttributeString("defaultColWidth", XmlConvert.ToString(sheet.StandardWidth));
    if (this.m_worksheetImpl.IsZeroHeight)
      writer.WriteAttributeString("zeroHeight", XmlConvert.ToString(true));
    if (sheet.CustomHeight && sheet.ListObjects.Count == 0)
      writer.WriteAttributeString("customHeight", XmlConvert.ToString(true));
    writer.WriteAttributeString("defaultRowHeight", XmlConvert.ToString(sheet.StandardHeight));
    if (sheet.OutlineLevelColumn > (byte) 0)
      writer.WriteAttributeString("outlineLevelCol", XmlConvert.ToString(sheet.OutlineLevelColumn));
    if (sheet.OutlineLevelRow > (byte) 0)
      writer.WriteAttributeString("outlineLevelRow", XmlConvert.ToString(sheet.OutlineLevelRow));
    if (sheet.BaseColumnWidth >= 0)
      writer.WriteAttributeString("baseColWidth", XmlConvert.ToString(sheet.BaseColumnWidth));
    if (sheet.IsThickTop)
      writer.WriteAttributeString("thickTop", XmlConvert.ToString(true));
    if (sheet.IsThickBottom)
      writer.WriteAttributeString("thickBottom", XmlConvert.ToString(true));
    writer.WriteEndElement();
    if (sheet.PivotTables.Count > 0)
    {
      for (int index = 0; index < sheet.PivotTables.Count; ++index)
      {
        PivotTableImpl pivotTable = sheet.PivotTables[index] as PivotTableImpl;
        PivotTableOptions options = pivotTable.Options as PivotTableOptions;
        if (pivotTable.IsChanged && options.RowLayout == PivotTableRowLayout.Tabular)
          pivotTable.AutoFitPivotTable(pivotTable);
      }
    }
    this.SerializeColumns(writer, sheet, hashXFIndexes);
    if (sheet.ImportDTHelper != null)
      this.SerializeDataTable(writer, sheet);
    else
      this.SerializeSheetData(writer, sheet.CellRecords, hashXFIndexes, "c", (Dictionary<string, string>) null, true);
    this.SerializeSheetProtection(writer, (WorksheetBaseImpl) sheet);
    this.SerializeAutoFilters(writer, sheet.AutoFilters);
    Stream worksheetSortStream = this.GetWorksheetSortStream(sheet.Index);
    this.SerializeStream(writer, worksheetSortStream);
    this.SerializeMerges(writer, sheet.MergeCells);
    Stream data = streamConFormats == null || streamConFormats.Length == 0L || !sheet.m_parseCondtionalFormats ? this.GetWorksheetCFStream(sheet.Index) : streamConFormats;
    this.SerializeStream(writer, data);
    this.SerializeDataValidations(writer, sheet.DVTable);
    this.SerializeHyperlinks(writer, sheet);
    IPageSetupConstantsProvider constants = (IPageSetupConstantsProvider) new WorksheetPageSetupConstants();
    Excel2007Serializator.SerializePrintSettings(writer, (IPageSetupBase) sheet.PageSetup, constants, false);
    this.SerializePagebreaks(writer, (IWorksheet) sheet);
    this.SerializeCustomProperties(writer, sheet);
    this.SerializeIgnoreErrors(writer, sheet);
    this.SerializeDrawingsWorksheetPart(writer, sheet);
    this.SerializeVmlShapesWorksheetPart(writer, (WorksheetBaseImpl) sheet);
    Excel2007Serializator.SerializeVmlHFShapesWorksheetPart(writer, (WorksheetBaseImpl) sheet, constants, (RelationCollection) null);
    this.SerializeOle(writer, sheet);
    this.SerilizeBackgroundImage(writer, (WorksheetBaseImpl) sheet);
    this.SerializeControls(writer, sheet);
    if (sheet.ImportDTHelper == null)
      sheet.DataHolder.SerializeTables(writer, sheet);
    if (sheet.Version >= ExcelVersion.Excel2010 || sheet.WorksheetSlicerStream != null)
    {
      Excel2010Serializator excel2010Serializator = new Excel2010Serializator(this.m_book);
      writer.WriteStartElement("extLst");
      excel2010Serializator.SerilaizeExtensions(writer, sheet);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  internal void SerializeDataTable(XmlWriter writer, WorksheetImpl sheet)
  {
    Dictionary<object, int> hashKeyToIndex = sheet.ParentWorkbook.InnerSST.HashKeyToIndex;
    DataTable dataTable = sheet.ImportDTHelper.DataTable;
    int firstRow = sheet.ImportDTHelper.FirstRow;
    int firstColumn = sheet.ImportDTHelper.FirstColumn;
    int lastColumn = dataTable.Columns.Count + firstColumn;
    int lastRow = dataTable.Rows.Count + firstRow;
    bool isFieldNameShown = sheet.ImportDTHelper.IsFieldNameShown;
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("sheetData");
    if (isFieldNameShown)
      this.SerializeCells(writer, sheet, dataTable, firstRow, firstColumn, lastRow, lastColumn, hashKeyToIndex, (DataRow) null, 0, isFieldNameShown);
    for (int index = 0; index < dataTable.Rows.Count; ++index)
    {
      DataRow row1 = dataTable.Rows[index];
      int row2 = isFieldNameShown ? index + 1 : index;
      this.SerializeCells(writer, sheet, dataTable, firstRow, firstColumn, lastRow, lastColumn, hashKeyToIndex, row1, row2, false);
    }
    writer.WriteEndElement();
  }

  internal void SerializeCells(
    XmlWriter writer,
    WorksheetImpl sheet,
    DataTable dataTable,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    Dictionary<object, int> sstDictionary,
    DataRow curRow,
    int row,
    bool isFieldNameShown)
  {
    if (writer == null)
      throw new ArgumentException(nameof (writer));
    if (dataTable == null)
      throw new ArgumentException(nameof (dataTable));
    if (sheet == null)
      throw new ArgumentException("worksheet");
    writer.WriteStartElement(nameof (row));
    Excel2007Serializator.SerializeAttribute(writer, "r", (firstRow + row).ToString(), string.Empty);
    string str = $"{firstColumn.ToString()}:{(lastColumn - 1).ToString()}";
    Excel2007Serializator.SerializeAttribute(writer, "spans", str, string.Empty);
    for (int index = 0; index < dataTable.Columns.Count; ++index)
    {
      DataColumn column = dataTable.Columns[index];
      object key = !isFieldNameShown || row != 0 ? curRow[column] : (object) column.ColumnName;
      if (!key.Equals((object) DBNull.Value) && !key.Equals((object) string.Empty))
      {
        writer.WriteStartElement("c");
        string cellName = RangeImpl.GetCellName(firstColumn + index, firstRow + row);
        Excel2007Serializator.SerializeAttribute(writer, "r", cellName, (string) null);
        if (isFieldNameShown && row == 0)
        {
          Excel2007Serializator.SerializeAttribute(writer, "t", "s", "n");
          int num = 0;
          if (sstDictionary.TryGetValue(key, out num))
          {
            writer.WriteElementString("v", num.ToString());
            writer.WriteEndElement();
          }
        }
        else
        {
          switch (column.DataType.Name)
          {
            case "String":
              Excel2007Serializator.SerializeAttribute(writer, "t", "s", "n");
              int num = 0;
              if (sstDictionary.TryGetValue(key, out num))
              {
                writer.WriteElementString("v", num.ToString());
                break;
              }
              break;
            case "DateTime":
              Excel2007Serializator.SerializeAttribute(writer, "s", sheet.ImportDTHelper.DateStyleIndex, 0);
              double oaDate = ((DateTime) curRow[column]).ToOADate();
              writer.WriteElementString("v", oaDate.ToString());
              break;
            default:
              object obj = curRow[column];
              if (obj.ToString() == "NaN")
                obj = (object) 0;
              writer.WriteElementString("v", obj.ToString());
              break;
          }
          writer.WriteEndElement();
        }
      }
    }
    writer.WriteEndElement();
  }

  protected virtual void SerilaizeExtensions(XmlWriter writer, WorksheetImpl sheet)
  {
  }

  private void SerializeControls(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    WorksheetDataHolder worksheetDataHolder = sheet != null ? sheet.DataHolder : throw new ArgumentNullException(nameof (sheet));
    Stream stream = worksheetDataHolder.ControlsStream;
    IList<string> stringList = (IList<string>) new List<string>();
    foreach (KeyValuePair<string, Relation> dicRelation in worksheetDataHolder.Relations.DicRelations)
    {
      if (dicRelation.Value != null && dicRelation.Value.Target != null && dicRelation.Value.Target.Contains("ctrlProps/ctrlProp"))
        stringList.Add(dicRelation.Value.Target.Replace("../", "xl/"));
    }
    Stream[] streamArray = new Stream[stringList.Count];
    Dictionary<string, ZipArchiveItem> dictionary = new Dictionary<string, ZipArchiveItem>();
    foreach (ZipArchiveItem zipArchiveItem in worksheetDataHolder.ParentHolder.Archive.Items)
    {
      if (stringList.Contains(zipArchiveItem.ItemName))
        dictionary.Add(zipArchiveItem.ItemName, zipArchiveItem);
    }
    int index1 = 0;
    for (int index2 = 1; index2 < worksheetDataHolder.ParentHolder.Archive.Items.Length; ++index2)
    {
      ZipArchiveItem zipArchiveItem = (ZipArchiveItem) null;
      if (dictionary.TryGetValue($"xl/ctrlProps/ctrlProp{(object) index2}.xml", out zipArchiveItem))
      {
        streamArray[index1] = zipArchiveItem.DataStream;
        ++index1;
      }
    }
    if (stream == null)
      return;
    bool flag1 = Excel2007Serializator.HasAlternateContent(sheet.Shapes);
    if (flag1 && sheet.HasAlternateContent)
      Excel2007Serializator.WriteAlternateContentControlsHeader(writer);
    stream.Position = 0L;
    XmlDocument xmlDocument1 = new XmlDocument();
    xmlDocument1.Load(stream);
    if (xmlDocument1.FirstChild != null && xmlDocument1.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
      xmlDocument1.RemoveChild(xmlDocument1.FirstChild);
    XmlNode xmlNode1 = (XmlNode) null;
    for (int i = 0; i < xmlDocument1.ChildNodes.Count; ++i)
    {
      if (xmlDocument1.ChildNodes[i].Name == "controls")
      {
        xmlNode1 = xmlDocument1.ChildNodes[i];
        break;
      }
    }
    bool flag2 = false;
    if (xmlNode1 != null)
    {
      IRange range = (IRange) null;
      int num = 1;
      for (int i1 = 0; i1 < xmlNode1.ChildNodes.Count; ++i1)
      {
        XmlNode childNode = xmlNode1.ChildNodes[i1];
        if (childNode.Name == "control")
        {
          flag2 = true;
          break;
        }
        XmlNode childNodeByName1 = this.GetChildNodeByName(childNode, "control");
        ShapeImpl implFromControlNode = this.GetShapeImplFromControlNode(childNodeByName1, sheet.Shapes, sheet);
        if (implFromControlNode == null)
        {
          childNodeByName1.ParentNode.ParentNode.ParentNode.RemoveChild(childNodeByName1.ParentNode.ParentNode);
          flag2 = true;
          --i1;
        }
        int count = xmlNode1.ChildNodes.Count;
        if (implFromControlNode != null && childNodeByName1 != null)
        {
          XmlNode childNodeByName2 = this.GetChildNodeByName(childNodeByName1, "from");
          this.UpdateNodeAnchors(childNodeByName2, true, implFromControlNode, sheet);
          this.UpdateNodeAnchors(childNodeByName2.NextSibling.Name == "to" ? childNodeByName2.NextSibling : (XmlNode) null, false, implFromControlNode, sheet);
          flag2 = true;
        }
        if (implFromControlNode is OptionButtonShapeImpl optionButtonShapeImpl && streamArray != null && streamArray[i1] != null)
        {
          if (optionButtonShapeImpl.LinkedCell != null)
          {
            if (range != null && range != optionButtonShapeImpl.LinkedCell)
              num = 1;
            range = optionButtonShapeImpl.LinkedCell;
          }
          if (optionButtonShapeImpl.LinkedCell != null && optionButtonShapeImpl.LinkedCell.Value == num.ToString() || range != null && range.Value == num.ToString())
            optionButtonShapeImpl.CheckState = ExcelCheckState.Checked;
          else if (optionButtonShapeImpl.LinkedCell != null && optionButtonShapeImpl.LinkedCell.Value != num.ToString() || range != null && range.Value != num.ToString())
            optionButtonShapeImpl.CheckState = ExcelCheckState.Unchecked;
          streamArray[i1].Position = 0L;
          XmlDocument xmlDocument2 = new XmlDocument();
          xmlDocument2.Load(streamArray[i1]);
          XmlNode xmlNode2 = (XmlNode) null;
          for (int i2 = 0; i2 < xmlDocument2.ChildNodes.Count; ++i2)
          {
            if (xmlDocument2.ChildNodes[i2].Name == "formControlPr")
            {
              xmlNode2 = xmlDocument2.ChildNodes[i2];
              break;
            }
          }
          if (xmlNode2.Name == "formControlPr")
          {
            bool flag3 = false;
            for (int i3 = 0; i3 < xmlNode2.Attributes.Count; ++i3)
            {
              if (xmlNode2.Attributes[i3].Name == "checked")
              {
                if (optionButtonShapeImpl.CheckState == ExcelCheckState.Checked)
                  xmlNode2.Attributes[i3].Value = "Checked";
                else
                  xmlNode2.Attributes[i3].Value = "UnChecked";
                flag3 = true;
              }
            }
            if (!flag3)
            {
              XmlAttribute attribute = xmlDocument2.CreateAttribute("checked");
              if (optionButtonShapeImpl.CheckState == ExcelCheckState.Checked)
                attribute.Value = "Checked";
              else
                attribute.Value = "UnChecked";
              xmlNode2.Attributes.InsertAfter(attribute, xmlNode2.Attributes[0]);
            }
            streamArray[i1].Position = 0L;
            xmlDocument2.Save(streamArray[i1]);
            ++num;
          }
        }
        if (count == 0)
        {
          stream = (Stream) null;
          flag2 = false;
        }
      }
    }
    if (flag2)
      xmlDocument1.WriteTo(writer);
    else if (stream != null)
    {
      XmlReader reader = UtilityMethods.CreateReader(stream);
      writer.WriteNode(reader, false);
    }
    writer.Flush();
    if (!flag1 || !sheet.HasAlternateContent)
      return;
    Excel2007Serializator.WriteAlternateContentFooter(writer);
  }

  private void UpdateNodeAnchors(
    XmlNode node,
    bool isFrom,
    ShapeImpl shapeImpl,
    WorksheetImpl sheet)
  {
    if (node == null)
      return;
    int iColumnIndex = isFrom ? shapeImpl.LeftColumn : shapeImpl.RightColumn;
    int num1 = isFrom ? shapeImpl.LeftColumnOffset : shapeImpl.RightColumnOffset;
    int iRowIndex = isFrom ? shapeImpl.TopRow : shapeImpl.BottomRow;
    int num2 = isFrom ? shapeImpl.TopRowOffset : shapeImpl.BottomRowOffset;
    double num3 = Math.Round(ApplicationImpl.ConvertFromPixel((sheet != null ? (double) sheet.GetColumnWidthInPixels(iColumnIndex) : 1.0) * (double) num1 / 1024.0, MeasureUnits.EMU));
    int num4 = iColumnIndex - 1;
    double num5 = (double) (int) ApplicationImpl.ConvertFromPixel(Math.Round((sheet != null ? (double) sheet.GetRowHeightInPixels(iRowIndex) : 1.0) * (double) num2 / 256.0, 1), MeasureUnits.EMU);
    int num6 = iRowIndex - 1;
    for (int i = 0; i < node.ChildNodes.Count; ++i)
    {
      XmlNode childNode = node.ChildNodes[i];
      if (childNode.LocalName == "col")
        childNode.FirstChild.Value = num4.ToString();
      if (childNode.LocalName == "row")
        childNode.FirstChild.Value = num6.ToString();
      if (childNode.LocalName == "rowOff")
        childNode.FirstChild.Value = ((int) num5).ToString();
      if (childNode.LocalName == "colOff")
        childNode.FirstChild.Value = ((int) num3).ToString();
    }
  }

  private ShapeImpl GetShapeImplFromControlNode(XmlNode child, IShapes shapes, WorksheetImpl sheet)
  {
    if (child == null)
      return (ShapeImpl) null;
    string s = (string) null;
    for (int i = 0; i < child.Attributes.Count; ++i)
    {
      XmlAttribute attribute = child.Attributes[i];
      if (attribute.LocalName == "name")
      {
        string str = attribute.Value;
      }
      else if (attribute.LocalName == "shapeId")
        s = attribute.Value;
    }
    return sheet.InnerShapes.GetShapeById(int.Parse(s)) as ShapeImpl;
  }

  private XmlNode GetChildNodeByName(XmlNode child, string name)
  {
    if (child == null)
      return (XmlNode) null;
    do
    {
      child = child.FirstChild;
    }
    while ((child == null || !(child.Name == name)) && child != null);
    return child;
  }

  public static void WriteAlternateContentFooter(XmlWriter writer)
  {
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  public static void WriteAlternateContentHeader(XmlWriter writer)
  {
    writer.WriteStartElement("mc", "AlternateContent", "http://schemas.openxmlformats.org/markup-compatibility/2006");
    writer.WriteStartElement("mc", "Choice", "http://schemas.openxmlformats.org/markup-compatibility/2006");
    writer.WriteAttributeString("xmlns", "a14", (string) null, "http://schemas.microsoft.com/office/drawing/2010/main");
    writer.WriteAttributeString("Requires", "a14");
  }

  public static void WriteAlternateContentControlsHeader(XmlWriter writer)
  {
    writer.WriteStartElement("mc", "AlternateContent", "http://schemas.openxmlformats.org/markup-compatibility/2006");
    writer.WriteAttributeString("xmlns", "mc", (string) null, "http://schemas.openxmlformats.org/markup-compatibility/2006");
    writer.WriteStartElement("mc", "Choice", "http://schemas.openxmlformats.org/markup-compatibility/2006");
    writer.WriteAttributeString("xmlns", "a14", (string) null, "http://schemas.microsoft.com/office/drawing/2010/main");
    writer.WriteAttributeString("Requires", "x14");
  }

  public void SerializeSheetProtection(XmlWriter writer, WorksheetBaseImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ExcelSheetProtection protection = sheet != null ? sheet.InnerProtection : throw new ArgumentNullException(nameof (sheet));
    bool flag = sheet is ChartImpl;
    if (!sheet.ProtectContents && (!flag || protection == ExcelSheetProtection.None))
      return;
    writer.WriteStartElement("sheetProtection");
    if (sheet.IsPasswordProtected)
    {
      if (sheet.AlgorithmName != null)
      {
        writer.WriteAttributeString("algorithmName", sheet.AlgorithmName);
        writer.WriteAttributeString("hashValue", Convert.ToBase64String(sheet.HashValue));
        writer.WriteAttributeString("saltValue", Convert.ToBase64String(sheet.SaltValue));
        writer.WriteAttributeString("spinCount", sheet.SpinCount.ToString());
      }
      else if (sheet.Password.IsPassword != (ushort) 1)
      {
        string str = sheet.Password.IsPassword.ToString("X");
        writer.WriteAttributeString("password", str);
      }
    }
    Excel2007Serializator.ProtectionAttributeSerializator attributeSerializator;
    string[] protectionAttributes;
    bool[] flagArray;
    if (!flag)
    {
      attributeSerializator = new Excel2007Serializator.ProtectionAttributeSerializator(this.SerializeProtectionAttribute);
      protectionAttributes = Protection.ProtectionAttributes;
      flagArray = Protection.DefaultValues;
    }
    else
    {
      attributeSerializator = new Excel2007Serializator.ProtectionAttributeSerializator(this.SerializeChartProtectionAttribute);
      protectionAttributes = Protection.ChartProtectionAttributes;
      flagArray = Protection.ChartDefaultValues;
    }
    int index = 0;
    for (int length = protectionAttributes.Length; index < length; ++index)
      attributeSerializator(writer, protectionAttributes[index], Protection.ProtectionFlags[index], flagArray[index], protection);
    writer.WriteEndElement();
  }

  private void SerializeProtectionAttribute(
    XmlWriter writer,
    string attributeName,
    ExcelSheetProtection flag,
    bool defaultValue,
    ExcelSheetProtection protection)
  {
    bool flag1 = (protection & flag) == ExcelSheetProtection.None;
    Excel2007Serializator.SerializeAttribute(writer, attributeName, flag1, defaultValue);
  }

  private void SerializeChartProtectionAttribute(
    XmlWriter writer,
    string attributeName,
    ExcelSheetProtection flag,
    bool defaultValue,
    ExcelSheetProtection protection)
  {
    bool flag1 = (protection & flag) != ExcelSheetProtection.None;
    Excel2007Serializator.SerializeAttribute(writer, attributeName, flag1, defaultValue);
  }

  private void SerializeIgnoreErrors(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ErrorIndicatorsCollection indicatorsCollection = sheet != null ? sheet.ErrorIndicators : throw new ArgumentNullException(nameof (sheet));
    if (indicatorsCollection == null || indicatorsCollection.Count == 0)
      return;
    int count1 = indicatorsCollection.Count;
    bool flag = false;
    for (int i = 0; i < count1; ++i)
    {
      if (indicatorsCollection[i].IgnoreOptions != ExcelIgnoreError.None)
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      return;
    writer.WriteStartElement("ignoredErrors");
    int i1 = 0;
    for (int count2 = indicatorsCollection.Count; i1 < count2; ++i1)
    {
      ErrorIndicatorImpl indicator = indicatorsCollection[i1];
      this.SerializeErrorIndicator(writer, indicator);
    }
    writer.WriteEndElement();
  }

  private void SerializeErrorIndicator(XmlWriter writer, ErrorIndicatorImpl indicator)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (indicator == null)
      throw new ArgumentNullException(nameof (indicator));
    if (indicator.IgnoreOptions == ExcelIgnoreError.None)
      return;
    writer.WriteStartElement("ignoredError");
    string cellList = this.GetCellList(indicator);
    writer.WriteAttributeString("sqref", cellList);
    this.SerializeErrorType(writer, indicator.IgnoreOptions);
    writer.WriteEndElement();
  }

  private void SerializeErrorType(XmlWriter writer, ExcelIgnoreError excelIgnoreError)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    int index = 0;
    for (int length = Excel2007Serializator.ErrorsSequence.Length; index < length; ++index)
    {
      if ((excelIgnoreError & Excel2007Serializator.ErrorsSequence[index]) != ExcelIgnoreError.None)
        writer.WriteAttributeString(Excel2007Serializator.ErrorTagsSequence[index], "1");
    }
  }

  private string GetCellList(ErrorIndicatorImpl indicator)
  {
    List<Rectangle> rectangleList = indicator != null ? indicator.CellList : throw new ArgumentNullException(nameof (indicator));
    StringBuilder stringBuilder = new StringBuilder();
    int index = 0;
    for (int count = rectangleList.Count; index < count; ++index)
    {
      Rectangle rectangle = rectangleList[index];
      string cellName1 = RangeImpl.GetCellName(rectangle.Left + 1, rectangle.Top + 1);
      stringBuilder.Append(cellName1);
      if (rectangle.Left != rectangle.Right || rectangle.Top != rectangle.Bottom)
      {
        stringBuilder.Append(':');
        string cellName2 = RangeImpl.GetCellName(rectangle.Right + 1, rectangle.Bottom + 1);
        stringBuilder.Append(cellName2);
      }
      if (index != count - 1)
        stringBuilder.Append(' ');
    }
    return stringBuilder.ToString();
  }

  private void SerializeCustomProperties(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    IWorksheetCustomProperties customProperties = sheet != null ? (IWorksheetCustomProperties) sheet.InnerCustomProperties : throw new ArgumentNullException(nameof (sheet));
    if (customProperties == null || customProperties.Count == 0)
      return;
    writer.WriteStartElement("customProperties");
    int index = 0;
    for (int count = customProperties.Count; index < count; ++index)
    {
      this.SerializeWorksheetProperty(writer, sheet, customProperties[index], this.m_sheetPropertyIndex);
      ++this.m_sheetPropertyIndex;
    }
    writer.WriteEndElement();
  }

  private void SerializeWorksheetProperty(
    XmlWriter writer,
    WorksheetImpl sheet,
    ICustomProperty property,
    int counter)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (property == null)
      throw new ArgumentNullException(nameof (property));
    WorksheetDataHolder dataHolder = sheet.DataHolder;
    FileDataHolder parentHolder = dataHolder.ParentHolder;
    RelationCollection relations = dataHolder.Relations;
    writer.WriteStartElement("customPr");
    writer.WriteAttributeString("name", property.Name);
    ZipArchiveItem zipArchiveItem;
    string str = parentHolder.PrepareNewItem("xl/customProperty", "bin", "application/vnd.openxmlformats-officedocument.spreadsheetml.customProperty", relations, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/customProperty", ref counter, out zipArchiveItem);
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", str);
    this.WritePropertyValue(sheet, zipArchiveItem, property);
    writer.WriteEndElement();
  }

  private void WritePropertyValue(
    WorksheetImpl sheet,
    ZipArchiveItem item,
    ICustomProperty property)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    if (property == null)
      throw new ArgumentNullException(nameof (property));
    byte[] bytes = Encoding.Unicode.GetBytes(property.Value);
    item.DataStream.Write(bytes, 0, bytes.Length);
  }

  private Stream GetWorksheetCFStream(int iSheetIndex)
  {
    return this.m_streamsSheetsCF == null ? (Stream) null : this.m_streamsSheetsCF[iSheetIndex];
  }

  private Stream GetWorksheetSortStream(int iSheetIndex)
  {
    return this.m_streamsSheetsSort == null ? (Stream) null : this.m_streamsSheetsSort[iSheetIndex];
  }

  private Stream GetFiltersSortStream(int iSheetIndex)
  {
    return this.m_streamsFiltersSort == null ? (Stream) null : this.m_streamsFiltersSort[iSheetIndex];
  }

  private Stream GetColorFilterStream(int listIndex)
  {
    return this.m_colorFilterStreamList == null ? (Stream) null : this.m_colorFilterStreamList[listIndex];
  }

  private Stream GetTableFiltersSortStream(int iSheetIndex, int iFilterIndex)
  {
    return this.m_streamsTableFiltersSort == null || this.m_streamsTableFiltersSort[iSheetIndex] == null ? (Stream) null : this.m_streamsTableFiltersSort[iSheetIndex][iFilterIndex];
  }

  public void SerializeCommentNotes(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    writer.WriteStartDocument(true);
    writer.WriteStartElement("comments", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    IDictionary<string, int> dicAuthors = this.SerializeAuthors(writer, sheet);
    this.SerializeCommentsList(writer, sheet, dicAuthors);
    writer.WriteEndElement();
    WorksheetDataHolder dataHolder = sheet.DataHolder;
    if (dataHolder.CommentNotesId != null)
      return;
    dataHolder.CommentNotesId = dataHolder.Relations.GenerateRelationId();
  }

  public void SerializeVmlShapes(
    XmlWriter writer,
    ShapeCollectionBase shapes,
    WorksheetDataHolder holder,
    Dictionary<int, ShapeSerializator> dictSerializators,
    RelationCollection vmlRelations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (shapes == null)
      throw new ArgumentNullException(nameof (shapes));
    writer.WriteStartElement("xml");
    writer.WriteAttributeString("xmlns", "v", (string) null, "urn:schemas-microsoft-com:vml");
    writer.WriteAttributeString("xmlns", "o", (string) null, "urn:schemas-microsoft-com:office:office");
    writer.WriteAttributeString("xmlns", "x", (string) null, "urn:schemas-microsoft-com:office:excel");
    UniqueInstanceTypeList instanceTypeList = new UniqueInstanceTypeList();
    Dictionary<Stream, object> dictionary = new Dictionary<Stream, object>();
    int index1 = 0;
    for (int count = shapes.Count; index1 < count; ++index1)
    {
      ShapeImpl shape = shapes[index1] as ShapeImpl;
      shape.PrepareForSerialization();
      if (shape.VmlShape)
      {
        if (shape.XmlTypeStream == null)
        {
          instanceTypeList.AddShape(shape);
        }
        else
        {
          dictionary[shape.XmlTypeStream] = (object) null;
          if (shape.ImageRelation != null)
            vmlRelations[shape.ImageRelationId] = shape.ImageRelation;
        }
      }
    }
    if (shapes.ShapeLayoutStream != null)
    {
      Stream shapeLayoutStream = shapes.ShapeLayoutStream;
      shapeLayoutStream.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, shapeLayoutStream);
    }
    foreach (KeyValuePair<int, Type> uniquePair in instanceTypeList.UniquePairs())
    {
      int key = uniquePair.Key;
      Type shapeType = uniquePair.Value;
      ShapeSerializator shapeSerializator;
      if (dictSerializators.TryGetValue(key, out shapeSerializator))
        shapeSerializator.SerializeShapeType(writer, shapeType);
    }
    foreach (Stream key in dictionary.Keys)
    {
      key.Position = 0L;
      XmlReader reader = UtilityMethods.CreateReader(key);
      writer.WriteNode(reader, false);
    }
    int index2 = 0;
    for (int count = shapes.Count; index2 < count; ++index2)
    {
      ShapeImpl shape = (ShapeImpl) shapes[index2];
      shape.PrepareForSerialization();
      int instance = shape.Instance;
      if (shape.VmlShape)
      {
        ShapeSerializator shapeSerializator;
        if ((shape.XmlDataStream == null || shape.EnableAlternateContent) && dictSerializators.TryGetValue(instance, out shapeSerializator))
          shapeSerializator.Serialize(writer, shape, holder, vmlRelations);
        else if (shape.XmlDataStream != null)
        {
          Stream xmlDataStream = shape.XmlDataStream;
          xmlDataStream.Position = 0L;
          XmlReader reader = UtilityMethods.CreateReader(xmlDataStream);
          writer.WriteNode(reader, false);
        }
      }
    }
    writer.WriteEndElement();
    writer.Flush();
  }

  public void SerializeDrawings(
    XmlWriter writer,
    ShapesCollection shapes,
    WorksheetDataHolder holder)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (shapes == null)
      throw new ArgumentNullException(nameof (shapes));
    if (shapes.Count - shapes.WorksheetBase.VmlShapesCount <= 0 && !Excel2007Serializator.HasAlternateContent((IShapes) shapes))
      return;
    bool flag = shapes.Worksheet == null;
    string localName1;
    string str;
    string prefix;
    string localName2;
    string ns;
    if (flag)
    {
      localName1 = "cdr";
      str = "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
      prefix = "c";
      localName2 = "userShapes";
      ns = "http://schemas.openxmlformats.org/drawingml/2006/chart";
    }
    else
    {
      localName1 = "xdr";
      str = "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing";
      prefix = "xdr";
      localName2 = "wsDr";
      ns = str;
    }
    writer.WriteStartDocument(true);
    writer.WriteStartElement(prefix, localName2, ns);
    writer.WriteAttributeString("xmlns", localName1, (string) null, str);
    writer.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    int index1 = 0;
    int index2 = 0;
    for (int count = shapes.Count; index2 < count; ++index2)
    {
      ShapeImpl shape1 = (ShapeImpl) shapes[index2];
      switch (shape1.ShapeType)
      {
        case ExcelShapeType.AutoShape:
          Serializator serializator = new Serializator();
          AutoShapeImpl shape2 = shape1 as AutoShapeImpl;
          if (shape2.ShapeExt.ShapeID <= 0)
            shape2.ShapeExt.ShapeID = index2 + 1;
          if (shape2.ShapeExt.Rotation == 0.0)
            shape2.ShapeExt.Rotation = (double) shape1.ShapeRotation;
          if (shapes.Worksheet != null)
            shape2.ShapeExt.ParentSheet = (WorksheetBaseImpl) shapes.Worksheet;
          shape2.ShapeExt.Macro = shape1.OnAction;
          serializator.AddShape(shape2, writer);
          if (shape2.ImageRelationId != null && holder.DrawingsRelations[shape2.ImageRelationId] == null && shape2.ImageRelation != null)
          {
            holder.DrawingsRelations[shape2.ImageRelationId] = new Relation(shape2.ImageRelation.Target, shape2.ImageRelation.Type, shape2.ImageRelation.IsExternal);
            break;
          }
          break;
        case ExcelShapeType.Group:
          this.SerializeGroupShape(writer, shape1 as GroupShapeImpl, holder);
          break;
        default:
          if (!shape1.VmlShape || shape1.EnableAlternateContent)
          {
            ShapeSerializator shapeSerializator;
            if (this.m_shapesSerializators.TryGetValue(shape1.GetType(), out shapeSerializator))
            {
              shapeSerializator.Serialize(writer, shape1, holder, holder.DrawingsRelations);
              break;
            }
            if ((!shape1.VmlShape || shape1.EnableAlternateContent) && shape1.XmlDataStream != null && !flag)
            {
              new DrawingShapeSerializator().Serialize(writer, shape1, holder, holder.DrawingsRelations);
              break;
            }
            if (this.Worksheet.preservedStreams != null || shape1.preservedCnxnShapeStreams != null || shape1.preservedPictureStreams != null || shape1.preservedShapeStreams != null)
            {
              this.SerializeShape(writer, shape1, holder, holder.DrawingsRelations, index1);
              ++index1;
              break;
            }
            break;
          }
          break;
      }
    }
    writer.WriteEndElement();
  }

  public void SerializeShape(
    XmlWriter writer,
    ShapeImpl shape,
    WorksheetDataHolder holder,
    RelationCollection vmlRelations,
    int index)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    shape.GetType();
    if (shape.IsAbsoluteAnchor)
    {
      writer.WriteStartElement("absoluteAnchor", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    }
    else
    {
      writer.WriteStartElement("twoCellAnchor", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      DrawingShapeSerializator shapeSerializator = new DrawingShapeSerializator();
      shapeSerializator.SerializeAnchorPoint(writer, "from", shape.LeftColumn, shape.LeftColumnOffset, shape.TopRow, shape.TopRowOffset, shape.Worksheet, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      shapeSerializator.SerializeAnchorPoint(writer, "to", shape.RightColumn, shape.RightColumnOffset, shape.BottomRow, shape.BottomRowOffset, shape.Worksheet, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    }
    if (shape.preservedCnxnShapeStreams != null)
    {
      for (int index1 = 0; index1 < shape.preservedCnxnShapeStreams.Count; ++index1)
        this.SerializeStream(writer, shape.preservedCnxnShapeStreams[index1]);
    }
    if (shape.GraphicFrameStream != null)
    {
      ChartShapeImpl childShape = (ChartShapeImpl) shape.ChildShapes[0];
      ChartImpl chartObject = childShape.ChartObject;
      ChartShapeSerializator shapeSerializator = new ChartShapeSerializator();
      shapeSerializator.SerializeChartFile(holder, chartObject, out string _);
      writer.WriteStartElement("graphicFrame", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      writer.WriteAttributeString("macro", string.Empty);
      shapeSerializator.SerializeNonVisualGraphicFrameProperties(writer, childShape, holder);
      DrawingShapeSerializator.SerializeForm(writer, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing", "http://schemas.openxmlformats.org/drawingml/2006/main", childShape.OffsetX, childShape.OffsetY, childShape.ExtentsX, childShape.ExtentsY);
      shapeSerializator.SerializeSlicerGraphics(writer, childShape);
      shape.ChildShapes.Remove((ShapeImpl) childShape);
    }
    else if (shape.preservedShapeStreams != null || shape.preservedPictureStreams != null || shape.ChildShapes.Count > 0)
    {
      if (!shape.IsAbsoluteAnchor)
      {
        writer.WriteStartElement("grpSp", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      }
      else
      {
        writer.WriteStartElement("pos", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
        int num1 = (int) ApplicationImpl.ConvertFromPixel((double) shape.Left, MeasureUnits.EMU);
        writer.WriteAttributeString("x", num1.ToString());
        int num2 = (int) ApplicationImpl.ConvertFromPixel((double) shape.Top, MeasureUnits.EMU);
        writer.WriteAttributeString("y", num2.ToString());
        writer.WriteEndElement();
        writer.WriteStartElement("ext", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
        num2 = (int) ApplicationImpl.ConvertFromPixel((double) shape.Width, MeasureUnits.EMU);
        writer.WriteAttributeString("cx", num2.ToString());
        num2 = (int) ApplicationImpl.ConvertFromPixel((double) shape.Height, MeasureUnits.EMU);
        writer.WriteAttributeString("cy", num2.ToString());
        writer.WriteEndElement();
      }
      index *= 2;
      if (this.Worksheet.preservedStreams != null && index < this.Worksheet.preservedStreams.Count)
      {
        this.SerializeStream(writer, this.Worksheet.preservedStreams[index]);
        this.SerializeStream(writer, this.Worksheet.preservedStreams[index + 1]);
      }
      if (shape.IsAbsoluteAnchor || shape.preserveStreamOrder == null || shape.preserveStreamOrder.Count == 0)
      {
        if (shape.preservedShapeStreams != null)
        {
          for (int index2 = 0; index2 < shape.preservedShapeStreams.Count; ++index2)
            this.SerializeStream(writer, shape.preservedShapeStreams[index2]);
        }
        if (shape.preservedPictureStreams != null)
        {
          for (int index3 = 0; index3 < shape.preservedPictureStreams.Count; ++index3)
            this.SerializeStream(writer, shape.preservedPictureStreams[index3]);
        }
        if (shape.preservedInnerCnxnShapeStreams != null)
        {
          for (int index4 = 0; index4 < shape.preservedInnerCnxnShapeStreams.Count; ++index4)
            this.SerializeStream(writer, shape.preservedInnerCnxnShapeStreams[index4]);
        }
        for (int index5 = 0; index5 < shape.ChildShapes.Count; ++index5)
        {
          ChartShapeImpl childShape = (ChartShapeImpl) shape.ChildShapes[index5];
          ChartImpl chartObject = childShape.ChartObject;
          ChartShapeSerializator shapeSerializator = new ChartShapeSerializator();
          string chartFileName;
          string strRelationId = shapeSerializator.SerializeChartFile(holder, chartObject, out chartFileName);
          shapeSerializator.SerializeChartProperties(writer, childShape, strRelationId, holder, true);
          holder.SerializeRelations(chartObject.Relations, chartFileName.Substring(1), (WorksheetDataHolder) null, (WorksheetBaseImpl) chartObject);
        }
      }
      else
      {
        for (int index6 = 0; index6 < shape.preserveStreamOrder.Count; ++index6)
        {
          string[] strArray = shape.preserveStreamOrder[index6].Split('-');
          int result;
          if (int.TryParse(strArray[1], out result))
          {
            --result;
            switch (strArray[0])
            {
              case "cxnSp":
                this.SerializeStream(writer, shape.preservedInnerCnxnShapeStreams[result]);
                continue;
              case "chart":
                ChartShapeImpl childShape = (ChartShapeImpl) shape.ChildShapes[result];
                ChartImpl chartObject = childShape.ChartObject;
                ChartShapeSerializator shapeSerializator = new ChartShapeSerializator();
                string chartFileName;
                string strRelationId = shapeSerializator.SerializeChartFile(holder, chartObject, out chartFileName);
                shapeSerializator.SerializeChartProperties(writer, childShape, strRelationId, holder, true);
                holder.SerializeRelations(chartObject.Relations, chartFileName.Substring(1), (WorksheetDataHolder) null, (WorksheetBaseImpl) chartObject);
                continue;
              case "sp":
                this.SerializeStream(writer, shape.preservedShapeStreams[result]);
                continue;
              case "pic":
                this.SerializeStream(writer, shape.preservedPictureStreams[result]);
                continue;
              case "grpSp":
                this.SerializeStream(writer, shape.preservedShapeStreams[result]);
                continue;
              case "grpSpPr":
                this.SerializeStream(writer, shape.preservedShapeStreams[result]);
                continue;
              case "nvGrpSpPr":
                this.SerializeStream(writer, shape.preservedShapeStreams[result]);
                continue;
              default:
                continue;
            }
          }
        }
        writer.WriteEndElement();
      }
    }
    writer.WriteElementString("clientData", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing", string.Empty);
    writer.WriteEndElement();
  }

  internal void SerializeGroupShape(
    XmlWriter writer,
    GroupShapeImpl groupShape,
    WorksheetDataHolder holder)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (groupShape == null)
      throw new ArgumentNullException("shape");
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    if (groupShape.EnableAlternateContent)
      Excel2007Serializator.WriteAlternateContentHeader(writer);
    GroupShapeSerializator groupShapeSerializator = new GroupShapeSerializator(groupShape);
    groupShapeSerializator.SerializeAncor(writer);
    this.SerializeGroupShapeProperties(writer, groupShape, holder, groupShapeSerializator);
    writer.WriteEndElement();
    if (!groupShape.EnableAlternateContent)
      return;
    Excel2007Serializator.WriteAlternateContentFooter(writer);
  }

  internal void SerializeGroupShapeProperties(
    XmlWriter writer,
    GroupShapeImpl groupShape,
    WorksheetDataHolder holder,
    GroupShapeSerializator groupShapeSerializator)
  {
    string prefix = groupShape.Worksheet is WorksheetImpl ? "xdr" : "cdr";
    string str = groupShape.Worksheet is WorksheetImpl ? "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing" : "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
    writer.WriteStartElement(prefix, "grpSp", str);
    groupShapeSerializator.SerializeGroupShapeNVProps(writer);
    groupShapeSerializator.SerializeGroupShapeProperties(writer, groupShape, holder.ParentHolder, holder.Relations);
    if (groupShape.Items.Length > 0)
    {
      for (int index = 0; index < groupShape.Items.Length; ++index)
      {
        ShapeImpl shapeImpl = (ShapeImpl) groupShape.Items[index];
        switch (shapeImpl.ShapeType)
        {
          case ExcelShapeType.AutoShape:
            AutoShapeImpl parentShape = shapeImpl as AutoShapeImpl;
            AutoShapeSerializator shapeSerializator1 = new AutoShapeSerializator(parentShape);
            if (parentShape.ShapeExt.ShapeID <= 0)
              parentShape.ShapeExt.ShapeID = index + 1;
            if (parentShape.ShapeExt.Rotation == 0.0)
              parentShape.ShapeExt.Rotation = (double) shapeImpl.ShapeRotation;
            if (groupShape.Worksheet != null)
              parentShape.ShapeExt.ParentSheet = groupShape.Worksheet;
            shapeSerializator1.SerializeShapeChoices(writer);
            if (parentShape.ImageRelationId != null && holder.DrawingsRelations[parentShape.ImageRelationId] == null && parentShape.ImageRelation != null)
            {
              holder.DrawingsRelations[parentShape.ImageRelationId] = new Relation(parentShape.ImageRelation.Target, parentShape.ImageRelation.Type, parentShape.ImageRelation.IsExternal);
              break;
            }
            break;
          case ExcelShapeType.Group:
            GroupShapeSerializator groupShapeSerializator1 = new GroupShapeSerializator(shapeImpl as GroupShapeImpl);
            this.SerializeGroupShapeProperties(writer, shapeImpl as GroupShapeImpl, holder, groupShapeSerializator1);
            break;
          default:
            ShapeSerializator shapeSerializator2;
            if (this.m_shapesSerializators.TryGetValue(shapeImpl.GetType(), out shapeSerializator2))
            {
              if (shapeImpl.GetType() == typeof (BitmapShapeImpl))
              {
                string relationId = (shapeSerializator2 as BitmapShapeSerializator).SerializePictureFile(holder, shapeImpl as BitmapShapeImpl, holder.DrawingsRelations);
                (shapeSerializator2 as BitmapShapeSerializator).SerializePicture(writer, shapeImpl as BitmapShapeImpl, relationId, holder, str);
                break;
              }
              if (shapeImpl.GetType() == typeof (TextBoxShapeImpl))
              {
                (shapeSerializator2 as TextBoxSerializator).SerializeTextBox(writer, shapeImpl, holder, str);
                break;
              }
              if (shapeImpl.GetType() == typeof (ChartShapeImpl))
              {
                ChartShapeImpl chart = (ChartShapeImpl) shapeImpl;
                ChartImpl chartObject = chart.ChartObject;
                if (chartObject.Relations.Count != 0)
                {
                  foreach (KeyValuePair<string, Relation> relation in chartObject.Relations)
                  {
                    if (relation.Value.Target.Contains("drawing"))
                    {
                      chartObject.Relations.Remove(relation.Key);
                      break;
                    }
                  }
                }
                if (chart.ChartObject == null && holder != null)
                  chart.ChartObject.DataHolder = holder;
                string chartFileName;
                string strRelationId = (shapeSerializator2 as ChartShapeSerializator).SerializeChartFile(holder, chart.ChartObject, out chartFileName);
                (shapeSerializator2 as ChartShapeSerializator).SerializeChartProperties(writer, chart, strRelationId, holder, true);
                holder.SerializeRelations(chartObject.Relations, chartFileName.Substring(1), holder, (WorksheetBaseImpl) chartObject);
                break;
              }
              break;
            }
            if ((!shapeImpl.VmlShape || shapeImpl.EnableAlternateContent) && shapeImpl.XmlDataStream != null)
            {
              DrawingShapeSerializator shapeSerializator3 = new DrawingShapeSerializator();
              if (shapeImpl.EnableAlternateContent && !groupShape.EnableAlternateContent)
                Excel2007Serializator.WriteAlternateContentHeader(writer);
              shapeSerializator3.SerializeChildShape(writer, shapeImpl, holder, holder.DrawingsRelations, str, prefix);
              if (shapeImpl.EnableAlternateContent && !groupShape.EnableAlternateContent)
              {
                Excel2007Serializator.WriteAlternateContentFooter(writer);
                break;
              }
              break;
            }
            if (shapeImpl.VmlShape)
            {
              new DrawingShapeSerializator().SerializeChildShape(writer, shapeImpl, holder, holder.DrawingsRelations, str, prefix);
              break;
            }
            break;
        }
      }
    }
    writer.WriteEndElement();
    writer.WriteStartElement(prefix, "clientData", str);
    writer.WriteEndElement();
  }

  public static bool HasAlternateContent(IShapes shapes)
  {
    foreach (ShapeImpl shape in (IEnumerable) shapes)
    {
      if (shape.EnableAlternateContent)
        return true;
    }
    return false;
  }

  public RelationCollection SerializeLinkItem(XmlWriter writer, ExternWorkbookImpl book)
  {
    if (book.IsAddInFunctions || book.IsInternalReference)
      return (RelationCollection) null;
    if (book.IsOleLink)
      return this.SerializeOleObjectLink(writer, book);
    return !book.IsDdeLink ? this.SerializeExternalLink(writer, book) : this.SerializeDdeObjectLink(writer, book);
  }

  public RelationCollection SerializeExternalLink(XmlWriter writer, ExternWorkbookImpl book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    if (book.IsInternalReference || book.IsAddInFunctions)
      return (RelationCollection) null;
    RelationCollection relationCollection = new RelationCollection();
    string relationId = relationCollection.GenerateRelationId();
    string str = this.ConvertAddressString(book.URL);
    bool flag = true;
    if (!str.StartsWith("file:///") && !str.StartsWith("http://") && str[0] != '/')
    {
      if (!str.Contains(":\\") && !str.StartsWith("\\"))
        flag = File.Exists(str);
      if (flag)
        str = "file:///" + str;
    }
    string type = flag ? "http://schemas.openxmlformats.org/officeDocument/2006/relationships/externalLinkPath" : "http://schemas.microsoft.com/office/2006/relationships/xlExternalLinkPath/xlPathMissing";
    relationCollection[relationId] = new Relation(str, type, true);
    writer.WriteStartElement("externalLink", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer.WriteStartElement("externalBook");
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
    this.SerializeSheetNames(writer, book);
    this.SerializeExternNames(writer, book);
    this.SerializeSheetDataSet(writer, book);
    writer.WriteEndElement();
    writer.WriteEndElement();
    return relationCollection;
  }

  public RelationCollection SerializeDdeObjectLink(XmlWriter writer, ExternWorkbookImpl book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    RelationCollection relationCollection = new RelationCollection();
    string relationId = relationCollection.GenerateRelationId();
    string target = this.ConvertAddressString(book.URL);
    string type = true ? "http://schemas.openxmlformats.org/officeDocument/2006/relationships/ddeObject" : "http://schemas.microsoft.com/office/2006/relationships/xlExternalLinkPath/xlPathMissing";
    relationCollection[relationId] = new Relation(target, type, true);
    writer.WriteStartDocument(true);
    writer.WriteStartElement("externalLink", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer.WriteStartElement("ddeLink");
    writer.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
    string[] strArray = book.ShortName.Split('|');
    writer.WriteAttributeString("ddeService", strArray[0]);
    writer.WriteAttributeString("ddeTopic", strArray[1]);
    writer.WriteStartElement("ddeItems");
    foreach (ExternNameImpl externName in (CollectionBase<ExternNameImpl>) book.ExternNames)
    {
      writer.WriteStartElement("ddeItem");
      writer.WriteAttributeString("name", externName.Name);
      if (externName.isAdvise)
        writer.WriteAttributeString("advise", "1");
      if (externName.isOle)
        writer.WriteAttributeString("ole", "1");
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    return relationCollection;
  }

  public RelationCollection SerializeOleObjectLink(XmlWriter writer, ExternWorkbookImpl book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    RelationCollection relationCollection = new RelationCollection();
    string relationId = relationCollection.GenerateRelationId();
    string str = this.ConvertAddressString(book.URL);
    bool flag = true;
    if (!str.StartsWith("file:///") && !str.StartsWith("http://") && str[0] != '/')
    {
      if (!str.Contains(":\\") && !str.StartsWith("\\"))
        flag = File.Exists(str);
      if (flag)
        str = "file:///" + str;
    }
    string type = flag ? "http://schemas.openxmlformats.org/officeDocument/2006/relationships/oleObject" : "http://schemas.microsoft.com/office/2006/relationships/xlExternalLinkPath/xlPathMissing";
    relationCollection[relationId] = new Relation(str, type, true);
    writer.WriteStartDocument(true);
    writer.WriteStartElement("externalLink", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer.WriteStartElement("oleLink");
    writer.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
    writer.WriteAttributeString("progId", book.ProgramId);
    writer.WriteStartElement("oleItems");
    writer.WriteStartElement("oleItem");
    writer.WriteAttributeString("name", "'");
    writer.WriteAttributeString("advise", "1");
    writer.WriteAttributeString("preferPic", "1");
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    return relationCollection;
  }

  private void SerializeSheetDataSet(XmlWriter writer, ExternWorkbookImpl book)
  {
    int count = book.Worksheets.Count;
    if (count <= 0)
      return;
    writer.WriteStartElement("sheetDataSet");
    for (int index = 0; index < count; ++index)
    {
      ExternWorksheetImpl externWorksheetImpl = book.Worksheets.Values[index];
      Dictionary<string, string> additionalAttributes = externWorksheetImpl.AdditionalAttributes ?? new Dictionary<string, string>();
      additionalAttributes["sheetId"] = externWorksheetImpl.Index.ToString();
      this.SerializeSheetData(writer, externWorksheetImpl.CellRecords, (Dictionary<int, int>) null, "cell", additionalAttributes, false);
    }
    writer.WriteEndElement();
  }

  private void SerializeSheetNames(XmlWriter writer, ExternWorkbookImpl book)
  {
    int sheetNumber = book.SheetNumber;
    if (sheetNumber <= 0)
      return;
    writer.WriteStartElement("sheetNames");
    for (int index = 0; index < sheetNumber; ++index)
    {
      string sheetName = book.GetSheetName(index);
      writer.WriteStartElement("sheetName");
      writer.WriteAttributeString("val", sheetName);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeExternNames(XmlWriter writer, ExternWorkbookImpl book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ExternNamesCollection externNamesCollection = book != null ? book.ExternNames : throw new ArgumentNullException(nameof (book));
    int count = externNamesCollection.Count;
    if (count <= 0)
      return;
    writer.WriteStartElement("definedNames");
    for (int index = 0; index < count; ++index)
      this.SerializeExternName(writer, externNamesCollection[index]);
    writer.WriteEndElement();
  }

  private void SerializeExternName(XmlWriter writer, ExternNameImpl externName)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ExternNameRecord externNameRecord = externName != null ? externName.Record : throw new ArgumentNullException(nameof (externName));
    writer.WriteStartElement("definedName");
    string empty = string.Empty;
    string str = !externName.Name.Contains("\0") ? externName.Name : externName.Name.Replace("\0", string.Empty);
    writer.WriteAttributeString("name", str);
    if (externName.RefersTo != null)
      writer.WriteAttributeString("refersTo", externName.RefersTo);
    int sheetId = externName.sheetId;
    if (sheetId >= 0)
      writer.WriteAttributeString("sheetId", sheetId.ToString());
    writer.WriteEndElement();
  }

  private void SerializeDrawingsWorksheetPart(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    int num = 0;
    if (sheet.ListObjects.Count > 0)
    {
      foreach (IListObject listObject in (IEnumerable<IListObject>) sheet.ListObjects)
      {
        if (listObject.ShowAutoFilter)
          num += listObject.AutoFilters.Count;
      }
    }
    if (sheet.Shapes.Count - sheet.VmlShapesCount - sheet.AutoFilters.Count - num <= 0 && !Excel2007Serializator.HasAlternateContent(sheet.Shapes))
      return;
    WorksheetDataHolder dataHolder = sheet.DataHolder;
    string id = dataHolder.DrawingsId;
    if (id == null)
    {
      dataHolder.DrawingsId = id = dataHolder.Relations.GenerateRelationId();
      dataHolder.Relations[id] = (Relation) null;
    }
    writer.WriteStartElement("drawing");
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", id);
    writer.WriteEndElement();
  }

  public void SerializeVmlShapesWorksheetPart(XmlWriter writer, WorksheetBaseImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (!sheet.HasVmlShapes)
      return;
    WorksheetDataHolder dataHolder = sheet.DataHolder;
    string id = dataHolder.VmlDrawingsId;
    if (id == null)
    {
      dataHolder.VmlDrawingsId = id = dataHolder.Relations.GenerateRelationId();
      dataHolder.Relations[id] = (Relation) null;
    }
    writer.WriteStartElement("legacyDrawing");
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", id);
    writer.WriteEndElement();
  }

  private void SerializeOle(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (!sheet.HasOleObject || sheet.OleObjects.Count == 0)
      return;
    writer.WriteStartElement("oleObjects");
    OleObjects oleObjects = (OleObjects) sheet.OleObjects;
    int index = 0;
    for (int count = oleObjects.Count; index < count; ++index)
    {
      if (!string.IsNullOrEmpty(oleObjects.Requries))
      {
        writer.WriteStartElement("mc", "AlternateContent", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        writer.WriteAttributeString("xmlns", "mc", (string) null, "http://schemas.openxmlformats.org/markup-compatibility/2006");
        writer.WriteStartElement("mc", "Choice", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        writer.WriteAttributeString("Requires", oleObjects.Requries);
      }
      OleObject oleObject = (OleObject) oleObjects[index];
      this.SerializeOleObject(writer, sheet, oleObject, false);
      if (!string.IsNullOrEmpty(oleObjects.Requries))
        writer.WriteEndElement();
      if (!string.IsNullOrEmpty(oleObject.FallbackShapeId))
      {
        writer.WriteStartElement("mc", "Fallback", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        this.SerializeOleObject(writer, sheet, oleObject, true);
        writer.WriteEndElement();
      }
      if (!string.IsNullOrEmpty(oleObjects.Requries))
        writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeOleObject(
    XmlWriter writer,
    WorksheetImpl sheet,
    OleObject oleObject,
    bool isFallback)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (oleObject == null)
      throw new ArgumentNullException(nameof (oleObject));
    WorksheetDataHolder dataHolder = sheet.DataHolder;
    string id = (string) null;
    if (oleObject.OleType == OleLinkType.Embed)
    {
      id = oleObject.ShapeRId;
      if (id == null)
      {
        id = dataHolder.Relations.GenerateRelationId();
        oleObject.ShapeRId = id;
      }
      dataHolder.Relations[id] = (Relation) null;
    }
    writer.WriteStartElement(nameof (oleObject));
    writer.WriteAttributeString("progId", OleTypeConvertor.ToOleString(oleObject.OleObjectType));
    if (oleObject.DvAspect == DVAspect.DVASPECT_ICON)
      writer.WriteAttributeString("dvAspect", oleObject.DvAspect.ToString());
    if (oleObject.OleType == OleLinkType.Link)
    {
      string str1 = $"[{oleObject.GetWorkbookIndex() + 1}]!''''";
      writer.WriteAttributeString("link", str1);
      string str2 = oleObject.DvAspect == DVAspect.DVASPECT_ICON ? "OLEUPDATE_ONCALL" : "OLEUPDATE_ALWAYS";
      writer.WriteAttributeString("oleUpdate", str2);
    }
    if (!isFallback)
    {
      int num = oleObject.Shape is ShapeImpl ? (oleObject.Shape as ShapeImpl).ShapeId : oleObject.ShapeID;
      writer.WriteAttributeString("shapeId", num.ToString());
    }
    else
      writer.WriteAttributeString("shapeId", oleObject.FallbackShapeId);
    if (oleObject.OleType == OleLinkType.Embed)
      writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", id);
    if (!isFallback && !string.IsNullOrEmpty(oleObject.DefaultSizeValue))
      this.SerializeObjectPr(writer, sheet, oleObject);
    writer.WriteEndElement();
  }

  private void SerializeObjectPr(XmlWriter writer, WorksheetImpl sheet, OleObject oleObject)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (oleObject == null)
      throw new ArgumentNullException(nameof (oleObject));
    writer.WriteStartElement("objectPr");
    writer.WriteAttributeString("defaultSize", oleObject.DefaultSizeValue);
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", oleObject.ObjectPrRelationId);
    this.SerializeOleObjectAnchor(writer, sheet, oleObject);
    writer.WriteEndElement();
  }

  private void SerializeOleObjectAnchor(XmlWriter writer, WorksheetImpl sheet, OleObject oleObject)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (oleObject == null)
      throw new ArgumentNullException(nameof (oleObject));
    ShapeImpl shapeImpl = (ShapeImpl) null;
    for (int index = 0; index < sheet.Shapes.Count; ++index)
    {
      if ((sheet.Shapes[index] as ShapeImpl).ShapeId.Equals(oleObject.ShapeID))
      {
        shapeImpl = sheet.Shapes[index] as ShapeImpl;
        break;
      }
    }
    writer.WriteStartElement("anchor");
    writer.WriteAttributeString("moveWithCells", oleObject.MoveWithCellsValue);
    writer.WriteAttributeString("sizeWithCells", oleObject.SizeWithCellsValue);
    DrawingShapeSerializator shapeSerializator = new DrawingShapeSerializator();
    shapeSerializator.SerializeColRowAnchor(writer, "from", shapeImpl.LeftColumn, shapeImpl.LeftColumnOffset, shapeImpl.TopRow, shapeImpl.TopRowOffset, (WorksheetBaseImpl) sheet, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing", false);
    shapeSerializator.SerializeColRowAnchor(writer, "to", shapeImpl.RightColumn, shapeImpl.RightColumnOffset, shapeImpl.BottomRow, shapeImpl.BottomRowOffset, shapeImpl.Worksheet, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing", false);
    writer.WriteEndElement();
  }

  public static void SerializeVmlHFShapesWorksheetPart(
    XmlWriter writer,
    WorksheetBaseImpl sheet,
    IPageSetupConstantsProvider constants,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    HeaderFooterShapeCollection footerShapeCollection = sheet != null ? sheet.InnerHeaderFooterShapes : throw new ArgumentNullException(nameof (sheet));
    if (footerShapeCollection == null || footerShapeCollection.Count == 0)
      return;
    WorksheetDataHolder dataHolder = sheet.DataHolder;
    string id = dataHolder.VmlHFDrawingsId;
    if (id == null)
    {
      if (relations == null)
        relations = dataHolder.Relations;
      dataHolder.VmlHFDrawingsId = id = relations.GenerateRelationId();
      relations[id] = (Relation) null;
    }
    if (sheet is ChartImpl && (sheet as ChartImpl).IsEmbeded)
      writer.WriteStartElement("legacyDrawingHF", constants.Namespace);
    else
      writer.WriteStartElement("legacyDrawingHF");
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", id);
    writer.WriteEndElement();
  }

  private void SerializeCommentsList(
    XmlWriter writer,
    WorksheetImpl sheet,
    IDictionary<string, int> dicAuthors)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (dicAuthors == null)
      throw new ArgumentNullException(nameof (dicAuthors));
    CommentsCollection innerComments = sheet.InnerComments;
    if (innerComments.Count <= 0)
      return;
    writer.WriteStartElement("commentList");
    int index = 0;
    for (int count = innerComments.Count; index < count; ++index)
    {
      ICommentShape comment = innerComments[index];
      this.SerializeComment(writer, comment, dicAuthors);
    }
    writer.WriteEndElement();
  }

  private void SerializeComment(
    XmlWriter writer,
    ICommentShape comment,
    IDictionary<string, int> dicAuthors)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (comment == null)
      throw new ArgumentNullException(nameof (comment));
    if (dicAuthors == null)
      throw new ArgumentNullException(nameof (dicAuthors));
    string cellName = RangeImpl.GetCellName(comment.Column, comment.Row);
    int dicAuthor = dicAuthors[comment.Author];
    CommentShapeImpl commentShapeImpl = (CommentShapeImpl) comment;
    writer.WriteStartElement(nameof (comment));
    writer.WriteAttributeString("ref", cellName);
    writer.WriteAttributeString("authorId", dicAuthor.ToString());
    writer.WriteStartElement("text");
    if (commentShapeImpl.Text != string.Empty)
      this.SerializeRichTextRun(writer, commentShapeImpl.InnerRichText.TextObject);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private IDictionary<string, int> SerializeAuthors(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    CommentsCollection commentsCollection = sheet != null ? sheet.InnerComments : throw new ArgumentNullException(nameof (sheet));
    int num = 0;
    IDictionary<string, int> dictionary = (IDictionary<string, int>) null;
    int count = commentsCollection.Count;
    if (count > 0)
    {
      dictionary = (IDictionary<string, int>) new Dictionary<string, int>();
      writer.WriteStartElement("authors");
      for (int index = 0; index < count; ++index)
      {
        string author = commentsCollection[index].Author;
        if (!dictionary.ContainsKey(author))
        {
          if (author != string.Empty && (author == " " || author[0] == ' ' || author.StartsWith("\r\n") || author.EndsWith("\r\n") || author.EndsWith("\t") || author.StartsWith("\t")))
          {
            writer.WriteStartElement("author");
            writer.WriteAttributeString("xml", "space", (string) null, "preserve");
            writer.WriteString(author);
            writer.WriteEndElement();
          }
          else
            writer.WriteElementString("author", author);
          dictionary.Add(author, num);
          ++num;
        }
      }
      writer.WriteEndElement();
    }
    return dictionary;
  }

  private void SerializeDimensions(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet.FirstRow <= 0 || sheet.FirstColumn <= 0 || sheet.LastColumn > sheet.Workbook.MaxColumnCount)
      return;
    writer.WriteStartElement("dimension");
    writer.WriteAttributeString("ref", sheet.UsedRange.AddressLocal);
    writer.WriteEndElement();
  }

  private void SerializeSheetViews(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    writer.WriteStartElement("sheetViews");
    writer.WriteStartElement("sheetView");
    IWorkbook workbook = sheet.Workbook;
    Excel2007Serializator.SerializeAttribute(writer, "windowProtection", workbook.IsWindowProtection, false);
    IRange topLeftCell = sheet.TopLeftCell;
    if (sheet.IsFreezePanes || topLeftCell != null && (topLeftCell.Row != 1 || topLeftCell.Column != 1))
      writer.WriteAttributeString("topLeftCell", topLeftCell.AddressLocal);
    Excel2007Serializator.SerializeAttribute(writer, "showGridLines", sheet.IsGridLinesVisible, true);
    Excel2007Serializator.SerializeAttribute(writer, "showRowColHeaders", sheet.IsRowColumnHeadersVisible, true);
    Excel2007Serializator.SerializeAttribute(writer, "showZeros", sheet.IsDisplayZeros, true);
    Excel2007Serializator.SerializeAttribute(writer, "zoomScale", sheet.Zoom, 100);
    Excel2007Serializator.SerializeAttribute(writer, "rightToLeft", sheet.IsRightToLeft, false);
    if (!sheet.WindowTwo.IsDefaultHeader)
    {
      writer.WriteAttributeString("defaultGridColor", "0");
      writer.WriteAttributeString("colorId", ((int) sheet.GridLineColor).ToString());
    }
    if (sheet.WindowTwo.IsSavedInPageBreakPreview)
    {
      writer.WriteAttributeString("view", "pageBreakPreview");
    }
    else
    {
      string str = "normal";
      if (sheet.View == SheetView.PageLayout)
        str = "pageLayout";
      writer.WriteAttributeString("view", str);
    }
    if (sheet.WindowTwo.IsSelected)
      Excel2007Serializator.SerializeBool(writer, "tabSelected", sheet.WindowTwo.IsSelected);
    writer.WriteAttributeString("workbookViewId", "0");
    this.SerializePane(writer, sheet);
    this.SerializeSelection(writer, sheet);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeSelection(XmlWriter writer, WorksheetImpl sheet)
  {
    IRange activeCell = sheet.GetActiveCell();
    if (activeCell == null)
      return;
    string addressLocal = activeCell.AddressLocal;
    writer.WriteStartElement("selection");
    if (sheet.Pane != null)
      writer.WriteAttributeString("pane", ((Pane.ActivePane) this.GetActivePane(sheet.Pane)).ToString());
    writer.WriteAttributeString("activeCell", addressLocal);
    SelectionRecord activeSelection = sheet.GetActiveSelection();
    if (activeSelection != null)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < activeSelection.Addr.Length; ++index)
      {
        SelectionRecord.TAddr taddr = activeSelection.Addr[index];
        IRange range = sheet[taddr.EFirstRow + 1, taddr.EFirstCol + 1, taddr.ELastRow + 1, taddr.ELastCol + 1];
        stringBuilder.Append(range.AddressLocal + " ");
      }
      string str = stringBuilder.ToString().TrimEnd();
      if (!string.IsNullOrEmpty(str) && str.Trim().Length != 0)
        writer.WriteAttributeString("sqref", str);
      else
        writer.WriteAttributeString("sqref", addressLocal);
    }
    else
      writer.WriteAttributeString("sqref", addressLocal);
    writer.WriteEndElement();
  }

  private void SerializePane(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (!sheet.IsFreezePanes && sheet.VerticalSplit == 0 && sheet.HorizontalSplit == 0)
      return;
    PaneRecord pane = sheet.Pane;
    if (pane == null || pane.VerticalSplit <= 0 && pane.HorizontalSplit <= 0)
      return;
    writer.WriteStartElement("pane");
    Excel2007Serializator.SerializeAttribute(writer, "xSplit", pane.VerticalSplit, 0);
    Excel2007Serializator.SerializeAttribute(writer, "ySplit", pane.HorizontalSplit, 0);
    string cellName = RangeImpl.GetCellName(pane.FirstColumn + 1, pane.FirstRow + 1);
    writer.WriteAttributeString("topLeftCell", cellName);
    string str1 = ((Pane.ActivePane) pane.ActivePane).ToString();
    writer.WriteAttributeString("activePane", str1);
    WindowTwoRecord windowTwo = sheet.WindowTwo;
    if (windowTwo.IsFreezePanes && !windowTwo.IsFreezePanesNoSplit)
    {
      string str2 = "frozenSplit";
      writer.WriteAttributeString("state", str2);
    }
    else if (windowTwo.IsFreezePanes && windowTwo.IsFreezePanesNoSplit)
    {
      string str3 = "frozen";
      writer.WriteAttributeString("state", str3);
    }
    writer.WriteEndElement();
  }

  private void SerializeStream(XmlWriter writer, Stream data)
  {
    Excel2007Serializator.SerializeStream(writer, data, "root");
  }

  public static void SerializeStream(XmlWriter writer, Stream data, string strRootName)
  {
    if (data == null || data.Length <= 0L)
      return;
    data.Position = 0L;
    XmlReader reader = UtilityMethods.CreateReader(data);
    while (reader.Name == strRootName || reader.Name == "root")
      reader.Read();
    while (!reader.EOF && (reader.Name != strRootName && reader.Name != "root" || reader.NodeType != XmlNodeType.EndElement))
      writer.WriteNode(reader, false);
  }

  internal void SerializeDialogMacroStream(XmlWriter writer, Stream data)
  {
    this.SerializeStream(writer, data);
  }

  private void SerializeRelation(XmlWriter writer, string key, Relation relation)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    if (relation == null)
      throw new ArgumentNullException(nameof (relation));
    writer.WriteStartElement("Relationship");
    writer.WriteAttributeString("Id", key);
    writer.WriteAttributeString("Type", relation.Type);
    writer.WriteAttributeString("Target", relation.Target);
    if (relation.IsExternal)
      writer.WriteAttributeString("TargetMode", "External");
    writer.WriteEndElement();
  }

  private void SerializeSheets(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("sheets");
    ITabSheets tabSheets = this.m_book.TabSheets;
    int index = 0;
    for (int count = tabSheets.Count; index < count; ++index)
    {
      if (tabSheets[index] is WorksheetBaseImpl && ((WorksheetBaseImpl) tabSheets[index]).m_dataHolder != null)
        this.SerializeSheetTag(writer, tabSheets[index]);
      else if (tabSheets[index] == null && this.m_book.Objects.InnerList[index] is DialogSheet)
        this.SerializeDialogSheetTag(writer, this.m_book.Objects.InnerList[index] as DialogSheet);
      else if (tabSheets[index] == null && this.m_book.Objects.InnerList[index] is MacroSheet)
        this.SerializeMacroSheetTag(writer, this.m_book.Objects.InnerList[index] as MacroSheet);
    }
    writer.WriteEndElement();
  }

  private void SerializeSheetTag(XmlWriter writer, ITabSheet sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    WorksheetDataHolder worksheetDataHolder = sheet != null ? ((WorksheetBaseImpl) sheet).m_dataHolder : throw new ArgumentNullException(nameof (sheet));
    string sheetId = worksheetDataHolder?.SheetId;
    if (sheetId == null)
    {
      sheetId = this.GenerateSheetId();
      if (worksheetDataHolder != null)
        worksheetDataHolder.SheetId = sheetId;
    }
    writer.WriteStartElement(nameof (sheet));
    writer.WriteAttributeString("name", sheet.Name);
    writer.WriteAttributeString("sheetId", sheetId);
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", worksheetDataHolder.RelationId);
    Worksheet2007Visibility visibility = (Worksheet2007Visibility) sheet.Visibility;
    if (visibility != Worksheet2007Visibility.Visible)
    {
      string str = Excel2007Serializator.LowerFirstLetter(visibility.ToString());
      writer.WriteAttributeString("state", str);
    }
    writer.WriteEndElement();
  }

  private void SerializeDialogSheetTag(XmlWriter writer, DialogSheet sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    WorksheetDataHolder worksheetDataHolder = sheet != null ? sheet.DataHolder : throw new ArgumentNullException(nameof (sheet));
    string sheetId = worksheetDataHolder?.SheetId;
    if (sheetId == null)
    {
      sheetId = this.GenerateSheetId();
      if (worksheetDataHolder != null)
        worksheetDataHolder.SheetId = sheetId;
    }
    writer.WriteStartElement(nameof (sheet));
    writer.WriteAttributeString("name", sheet.SheetName);
    writer.WriteAttributeString("sheetId", sheetId);
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", worksheetDataHolder.RelationId);
    writer.WriteEndElement();
  }

  private void SerializeMacroSheetTag(XmlWriter writer, MacroSheet sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    WorksheetDataHolder worksheetDataHolder = sheet != null ? sheet.DataHolder : throw new ArgumentNullException(nameof (sheet));
    string sheetId = worksheetDataHolder?.SheetId;
    if (sheetId == null)
    {
      sheetId = this.GenerateSheetId();
      if (worksheetDataHolder != null)
        worksheetDataHolder.SheetId = sheetId;
    }
    writer.WriteStartElement(nameof (sheet));
    writer.WriteAttributeString("name", sheet.SheetName);
    writer.WriteAttributeString("sheetId", sheetId);
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", worksheetDataHolder.RelationId);
    writer.WriteEndElement();
  }

  private string GenerateSheetId()
  {
    WorkbookObjectsCollection objects = this.m_book.Objects;
    int num = 0;
    int index = 0;
    for (int count = objects.Count; index < count; ++index)
    {
      WorksheetDataHolder dataHolder = ((WorksheetBaseImpl) objects[index]).DataHolder;
      if (dataHolder != null)
      {
        string sheetId = dataHolder.SheetId;
        int result;
        if (sheetId != null && int.TryParse(sheetId, out result) && result > num)
          num = result;
      }
    }
    return (num + 1).ToString();
  }

  private string GetRangeName(MergeCellsRecord.MergedRegion region)
  {
    if (region == null)
      throw new ArgumentNullException(nameof (region));
    return $"{RangeImpl.GetCellName(region.ColumnFrom + 1, region.RowFrom + 1)}:{RangeImpl.GetCellName(region.ColumnTo + 1, region.RowTo + 1)}";
  }

  private void SerializeNamedRange(XmlWriter writer, IName name)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    string str1 = this.m_formulaUtil.ParsePtgArray((name as NameImpl).NameRecord.FormulaTokens, 0, 0, false, true);
    if (string.IsNullOrEmpty(str1))
      return;
    writer.WriteStartElement("definedName");
    writer.WriteAttributeString(nameof (name), this.m_book.RemoveInvalidXmlCharacters(name.Name));
    writer.WriteAttributeString("comment", name.Description);
    if (name.IsLocal)
    {
      NameImpl nameImpl = (NameImpl) name;
      WorksheetImpl worksheet = nameImpl.Worksheet;
      if (worksheet != null)
      {
        string str2 = this.GetLocalSheetIndex(worksheet).ToString();
        writer.WriteAttributeString("localSheetId", str2);
      }
      else
        writer.WriteAttributeString("localSheetId", nameImpl.SheetIndex.ToString());
    }
    else if (name is NameImpl && (name as NameImpl).IsQueryTableRange)
      writer.WriteAttributeString("localSheetId", (name as NameImpl).SheetIndex.ToString());
    Excel2007Serializator.SerializeAttribute(writer, "hidden", !name.Visible, false);
    if (str1 == null)
      str1 = "#NAME?";
    if (!this.m_book.HasApostrophe && !this.CheckSheetName(str1))
      str1 = str1.Replace("'", "");
    if (this.m_sheetNames == null)
    {
      this.m_sheetNames = new List<string>();
      if (this.Worksheet != null)
      {
        for (int Index = 0; Index < this.Worksheet.Workbook.Worksheets.Count; ++Index)
          this.m_sheetNames.Add(this.Worksheet.Workbook.Worksheets[Index].Name);
      }
      else if (name.Worksheet != null)
      {
        for (int Index = 0; Index < name.Worksheet.Workbook.Worksheets.Count; ++Index)
          this.m_sheetNames.Add(name.Worksheet.Workbook.Worksheets[Index].Name);
      }
    }
    if (str1.StartsWith("#REF"))
      str1 = "#REF!";
    if (str1 != null && str1.Contains("!$") && !str1.Contains("#REF"))
    {
      string str3 = str1.Substring(0, str1.IndexOf("!$"));
      if (this.CheckSpecialCharacters(str3) && this.m_sheetNames.Contains(str3))
        str1 = str1.Replace(str3, $"'{str3}'");
    }
    if ((name as NameImpl).IsCommon)
    {
      string text = str1.Substring(str1.IndexOf("!"));
      writer.WriteString(text);
    }
    else
      writer.WriteString(str1);
    writer.WriteEndElement();
  }

  private int GetLocalSheetIndex(WorksheetImpl sheet)
  {
    int num = -1;
    ITabSheets tabSheets = this.m_book.TabSheets;
    for (int index = 0; index < tabSheets.Count; ++index)
    {
      if (tabSheets[index] != null && tabSheets[index].Name == sheet.Name)
      {
        num = index;
        break;
      }
    }
    return num != -1 ? num : throw new ArgumentException("Invalid Sheet");
  }

  private bool CheckSheetName(string strNameValue)
  {
    char[] charArray = strNameValue.ToCharArray();
    int index = 0;
    for (int length = charArray.Length; index < length; ++index)
    {
      if (!char.IsLetterOrDigit(charArray[index]))
        return true;
    }
    return false;
  }

  private void SerializeFonts(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    FontsCollection innerFonts = this.m_book.InnerFonts;
    int count = innerFonts.Count;
    writer.WriteStartElement("fonts");
    writer.WriteAttributeString("count", count.ToString());
    for (int index = 0; index < count; ++index)
    {
      IFont font = innerFonts[index];
      this.SerializeFont(writer, font, "font");
    }
    writer.WriteEndElement();
  }

  private void SerializeFont(XmlWriter writer, IFont font, string strElement)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    writer.WriteStartElement(strElement);
    if (font.Bold)
      writer.WriteElementString("b", string.Empty);
    if (font.VerticalAlignment != ExcelFontVertialAlignment.Baseline)
    {
      writer.WriteStartElement("vertAlign");
      writer.WriteAttributeString("val", font.VerticalAlignment.ToString().ToLower(CultureInfo.InvariantCulture));
      writer.WriteEndElement();
    }
    if (font.Italic)
      writer.WriteElementString("i", string.Empty);
    ExcelUnderline underline = font.Underline;
    if (underline != ExcelUnderline.None)
    {
      writer.WriteStartElement("u");
      string str1 = underline.ToString();
      string str2 = char.ToLower(str1[0]).ToString() + UtilityMethods.RemoveFirstCharUnsafe(str1);
      writer.WriteAttributeString("val", str2);
      writer.WriteEndElement();
    }
    if (font.Strikethrough)
      writer.WriteElementString("strike", string.Empty);
    writer.WriteStartElement("sz");
    writer.WriteAttributeString("val", XmlConvert.ToString(font.Size));
    writer.WriteEndElement();
    if (font.Color != (ExcelKnownColors) 32767 /*0x7FFF*/)
      this.SerializeFontColor(writer, "color", (font as IInternalFont).Font.ColorObject);
    string localName = "name";
    if (strElement == "rPr")
      localName = "rFont";
    writer.WriteStartElement(localName);
    writer.WriteAttributeString("val", font.FontName);
    writer.WriteEndElement();
    if (((FontImpl) font).Family != (byte) 0)
    {
      writer.WriteStartElement("family");
      writer.WriteAttributeString("val", ((FontImpl) font).Family.ToString());
      writer.WriteEndElement();
    }
    int charSet = (int) ((FontImpl) font).CharSet;
    if (charSet != 1)
    {
      writer.WriteStartElement("charset");
      writer.WriteAttributeString("val", charSet.ToString());
      writer.WriteEndElement();
    }
    if (((FontImpl) font).Scheme != null)
    {
      writer.WriteStartElement("scheme");
      writer.WriteAttributeString("val", ((FontImpl) font).Scheme);
      writer.WriteEndElement();
    }
    if (font.MacOSShadow)
      writer.WriteElementString("shadow", string.Empty);
    writer.WriteEndElement();
  }

  private void SerializeFontColor(XmlWriter writer, string tagName, ColorObject color)
  {
    writer.WriteStartElement(tagName);
    switch (color.ColorType)
    {
      case ColorType.Indexed:
        writer.WriteAttributeString("indexed", color.Value.ToString());
        break;
      case ColorType.RGB:
        writer.WriteAttributeString("rgb", color.Value.ToString("X8"));
        break;
      case ColorType.Theme:
        writer.WriteAttributeString("theme", color.Value.ToString());
        break;
    }
    Excel2007Serializator.SerializeAttribute(writer, "tint", color.Tint, 0.0);
    writer.WriteEndElement();
  }

  private void SerializeNumberFormats(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    List<FormatRecord> usedFormats = this.m_book.InnerFormats.GetUsedFormats(ExcelVersion.Excel2007);
    int count = usedFormats.Count;
    if (count == 0)
      return;
    writer.WriteStartElement("numFmts");
    writer.WriteAttributeString("count", count.ToString());
    for (int index = 0; index < count; ++index)
      this.SerializeNumberFormat(writer, usedFormats[index]);
    writer.WriteEndElement();
  }

  private void SerializeNumberFormat(XmlWriter writer, FormatRecord format)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    writer.WriteStartElement("numFmt");
    writer.WriteAttributeString("numFmtId", format.Index.ToString());
    string str = this.GetCustomizedString(format.FormatString);
    if (format.FormatString.Equals("Standard"))
      str = "General";
    writer.WriteAttributeString("formatCode", str);
    writer.WriteEndElement();
  }

  internal string GetCustomizedString(string numberFormatString)
  {
    string currencySymbol = CultureInfo.InstalledUICulture.NumberFormat.CurrencySymbol;
    if (numberFormatString.Contains(currencySymbol))
    {
      string str = currencySymbol;
      if (str == "$")
        str = "\\$";
      if (!Regex.IsMatch(numberFormatString, $"\\\\|\\*|\\\"|\\[{str}|\\[\\$[\\s]*{str}"))
        numberFormatString = numberFormatString.Replace(currencySymbol, $"\"{currencySymbol}\"");
    }
    return numberFormatString;
  }

  private int[] SerializeFills(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    Dictionary<FillImpl, int> dictionary = new Dictionary<FillImpl, int>();
    if (this.m_book.m_bisUnusedXFRemoved)
      this.m_book.m_bisUnusedXFRemoved = false;
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    int count = innerExtFormats.Count;
    int[] numArray = new int[count];
    FillImpl[] array = new FillImpl[count];
    int index1 = -1;
    for (int index2 = 0; index2 < count; ++index2)
    {
      FillImpl key;
      switch (index1)
      {
        case -1:
          key = new FillImpl();
          key.Pattern = ExcelPattern.None;
          key.PatternColorObject.SetIndexed(ExcelKnownColors.BlackCustom);
          key.ColorObject.SetIndexed(ExcelKnownColors.None);
          break;
        case 0:
          key = new FillImpl();
          key.Pattern = ExcelPattern.Percent10;
          key.PatternColorObject.SetIndexed(ExcelKnownColors.BlackCustom);
          key.ColorObject.SetIndexed(ExcelKnownColors.None);
          break;
        default:
          key = new FillImpl(innerExtFormats[index2]);
          break;
      }
      if (dictionary.ContainsKey(key))
      {
        numArray[index2] = dictionary[key];
      }
      else
      {
        index1 = dictionary.Count;
        dictionary.Add(key, index1);
        if (index1 >= array.Length)
          Array.Resize<FillImpl>(ref array, index1 + 1);
        array[index1] = key;
        numArray[index2] = index1;
        if (index1 == 0 || index1 == 1)
          --index2;
      }
    }
    writer.WriteStartElement("fills");
    writer.WriteAttributeString("count", (index1 + 1).ToString());
    for (int index3 = 0; index3 <= index1; ++index3)
      this.SerializeFill(writer, array[index3]);
    writer.WriteEndElement();
    return numArray;
  }

  internal void SerializeFill(XmlWriter writer, FillImpl fill)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (fill == null)
      throw new ArgumentNullException(nameof (fill));
    writer.WriteStartElement(nameof (fill));
    if (fill.Pattern == ExcelPattern.Gradient)
      this.SerializeGradientFill(writer, fill);
    else
      this.SerializePatternFill(writer, fill);
    writer.WriteEndElement();
  }

  private void SerializePatternFill(XmlWriter writer, FillImpl fill)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (fill == null)
      throw new ArgumentNullException(nameof (fill));
    writer.WriteStartElement("patternFill");
    writer.WriteAttributeString("patternType", this.ConvertPatternToString(fill.Pattern));
    if (fill.Pattern == ExcelPattern.Solid)
    {
      this.SerializeColorObject(writer, "fgColor", fill.ColorObject);
      this.SerializeColorObject(writer, "bgColor", fill.PatternColorObject);
    }
    else
    {
      ColorObject patternColorObject = fill.PatternColorObject;
      if (patternColorObject.ColorType != ColorType.Indexed || patternColorObject.GetIndexed((IWorkbook) this.m_book) != ExcelKnownColors.None)
        this.SerializeColorObject(writer, "fgColor", patternColorObject);
      ColorObject colorObject = fill.ColorObject;
      if (colorObject.ColorType != ColorType.Indexed || colorObject.GetIndexed((IWorkbook) this.m_book) != ExcelKnownColors.BlackCustom)
        this.SerializeColorObject(writer, "bgColor", colorObject);
    }
    writer.WriteEndElement();
  }

  private void SerializeGradientFill(XmlWriter writer, FillImpl fill)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (fill == null)
      throw new ArgumentNullException(nameof (fill));
    writer.WriteStartElement("gradientFill");
    switch (fill.GradientStyle)
    {
      case ExcelGradientStyle.From_Corner:
      case ExcelGradientStyle.From_Center:
        this.SerializeFromCenterCornerGradientFill(writer, fill);
        break;
      default:
        this.SerializeDegreeGradientFill(writer, fill);
        break;
    }
    writer.WriteEndElement();
  }

  private void SerializeDegreeGradientFill(XmlWriter writer, FillImpl fill)
  {
    ExcelGradientStyle gradientStyle = fill.GradientStyle;
    ExcelGradientVariants gradientVariant = fill.GradientVariant;
    double num1 = 0.0;
    if (gradientVariant == ExcelGradientVariants.ShadingVariants_3)
    {
      switch (gradientStyle)
      {
        case ExcelGradientStyle.Horizontal:
          num1 = 90.0;
          goto case ExcelGradientStyle.Vertical;
        case ExcelGradientStyle.Vertical:
          Excel2007Serializator.SerializeAttribute(writer, "degree", num1, 0.0);
          this.SerializeStopColorElements(writer, 0.0, fill.ColorObject);
          this.SerializeStopColorElements(writer, 0.5, fill.PatternColorObject);
          this.SerializeStopColorElements(writer, 1.0, fill.ColorObject);
          break;
        case ExcelGradientStyle.Diagonl_Up:
          num1 = 45.0;
          goto case ExcelGradientStyle.Vertical;
        case ExcelGradientStyle.Diagonl_Down:
          num1 = 135.0;
          goto case ExcelGradientStyle.Vertical;
        default:
          throw new ArgumentException("Unknown gradient style");
      }
    }
    else
    {
      double num2;
      switch (gradientStyle)
      {
        case ExcelGradientStyle.Horizontal:
          num2 = gradientVariant == ExcelGradientVariants.ShadingVariants_1 ? 90.0 : 270.0;
          break;
        case ExcelGradientStyle.Vertical:
          num2 = gradientVariant == ExcelGradientVariants.ShadingVariants_1 ? 0.0 : 180.0;
          break;
        case ExcelGradientStyle.Diagonl_Up:
          num2 = gradientVariant == ExcelGradientVariants.ShadingVariants_1 ? 45.0 : 225.0;
          break;
        case ExcelGradientStyle.Diagonl_Down:
          num2 = gradientVariant == ExcelGradientVariants.ShadingVariants_1 ? 135.0 : 315.0;
          break;
        default:
          throw new ArgumentException("Unknown gradient style");
      }
      Excel2007Serializator.SerializeAttribute(writer, "degree", num2, 0.0);
      this.SerializeStopColorElements(writer, 0.0, fill.ColorObject);
      this.SerializeStopColorElements(writer, 1.0, fill.PatternColorObject);
    }
  }

  private void SerializeFromCenterCornerGradientFill(XmlWriter writer, FillImpl fill)
  {
    ExcelGradientStyle gradientStyle = fill.GradientStyle;
    ExcelGradientVariants gradientVariant = fill.GradientVariant;
    Excel2007Serializator.SerializeAttribute(writer, "type", "path", string.Empty);
    double num1 = double.MinValue;
    double num2 = double.MinValue;
    double num3 = double.MinValue;
    double num4 = double.MinValue;
    if (gradientStyle == ExcelGradientStyle.From_Center)
    {
      double num5;
      num4 = num5 = 0.5;
      num3 = num5;
      num2 = num5;
      num1 = num5;
    }
    else
    {
      switch (gradientVariant)
      {
        case ExcelGradientVariants.ShadingVariants_1:
          break;
        case ExcelGradientVariants.ShadingVariants_2:
          num3 = num4 = 1.0;
          break;
        case ExcelGradientVariants.ShadingVariants_3:
          num1 = num2 = 1.0;
          break;
        case ExcelGradientVariants.ShadingVariants_4:
          double num6;
          num4 = num6 = 1.0;
          num3 = num6;
          num2 = num6;
          num1 = num6;
          break;
        default:
          throw new ArgumentException("Unknown gradient variant");
      }
    }
    Excel2007Serializator.SerializeAttribute(writer, "top", num1, double.MinValue);
    Excel2007Serializator.SerializeAttribute(writer, "bottom", num2, double.MinValue);
    Excel2007Serializator.SerializeAttribute(writer, "left", num3, double.MinValue);
    Excel2007Serializator.SerializeAttribute(writer, "right", num4, double.MinValue);
    this.SerializeStopColorElements(writer, 0.0, fill.ColorObject);
    this.SerializeStopColorElements(writer, 1.0, fill.PatternColorObject);
  }

  private void SerializeStopColorElements(XmlWriter writer, double dPosition, ColorObject color)
  {
    writer.WriteStartElement("stop");
    Excel2007Serializator.SerializeAttribute(writer, "position", dPosition, double.MinValue);
    this.SerializeColorObject(writer, nameof (color), color);
    writer.WriteEndElement();
  }

  private string ConvertPatternToString(ExcelPattern pattern)
  {
    return ((Excel2007Pattern) pattern).ToString();
  }

  private int[] SerializeBorders(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    Dictionary<BordersCollection, int> dictionary = new Dictionary<BordersCollection, int>();
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    int count = innerExtFormats.Count;
    int[] numArray = new int[count];
    BordersCollection[] bordersCollectionArray = new BordersCollection[count];
    int index1 = -1;
    for (int index2 = 0; index2 < count; ++index2)
    {
      BordersCollection key;
      if (index1 == -1)
      {
        WorkbookImpl book = this.m_book;
        BordersCollection bordersCollection = new BordersCollection(book.Application, (object) book, true);
        ExtendedFormatWrapper impl = new ExtendedFormatWrapper(book, 0);
        bordersCollection.InnerList.Clear();
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(book.Application, (object) book, (IInternalExtendedFormat) impl, ExcelBordersIndex.DiagonalDown));
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(book.Application, (object) book, (IInternalExtendedFormat) impl, ExcelBordersIndex.DiagonalUp));
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(book.Application, (object) book, (IInternalExtendedFormat) impl, ExcelBordersIndex.EdgeBottom));
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(book.Application, (object) book, (IInternalExtendedFormat) impl, ExcelBordersIndex.EdgeLeft));
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(book.Application, (object) book, (IInternalExtendedFormat) impl, ExcelBordersIndex.EdgeRight));
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(book.Application, (object) book, (IInternalExtendedFormat) impl, ExcelBordersIndex.EdgeTop));
        key = bordersCollection;
      }
      else
        key = (BordersCollection) innerExtFormats[index2].Borders;
      if (dictionary.ContainsKey(key))
      {
        numArray[index2] = dictionary[key];
      }
      else
      {
        index1 = dictionary.Count;
        dictionary.Add(key, index1);
        bordersCollectionArray[index1] = key;
        numArray[index2] = index1;
        if (index1 == 0)
          --index2;
      }
    }
    writer.WriteStartElement("borders");
    writer.WriteAttributeString("count", (index1 + 1).ToString());
    for (int index3 = 0; index3 <= index1; ++index3)
      this.SerializeBordersCollection(writer, bordersCollectionArray[index3]);
    writer.WriteEndElement();
    return numArray;
  }

  private void SerializeIndexedColor(XmlWriter writer, string tagName, ExcelKnownColors color)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tagName == null || tagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (tagName));
    writer.WriteStartElement(tagName);
    if (color > ExcelKnownColors.None)
      writer.WriteAttributeString("auto", "1");
    else
      writer.WriteAttributeString("indexed", ((int) color).ToString());
    writer.WriteEndElement();
  }

  public void SerializeRgbColor(XmlWriter writer, string tagName, ColorObject color)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tagName == null || tagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (tagName));
    int num = color.Value;
    writer.WriteStartElement(tagName);
    writer.WriteAttributeString("rgb", num.ToString("X8"));
    Excel2007Serializator.SerializeAttribute(writer, "tint", color.Tint, 0.0);
    writer.WriteEndElement();
  }

  private void SerializeThemeColor(XmlWriter writer, string tagName, ColorObject color)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tagName == null || tagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (tagName));
    writer.WriteStartElement(tagName);
    writer.WriteAttributeString("theme", color.Value.ToString());
    Excel2007Serializator.SerializeAttribute(writer, "tint", color.Tint, 0.0);
    writer.WriteEndElement();
  }

  private void SerializeColorObject(XmlWriter writer, string tagName, ColorObject color)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tagName == null || tagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (tagName));
    switch (color.ColorType)
    {
      case ColorType.Indexed:
        this.SerializeIndexedColor(writer, tagName, (ExcelKnownColors) color.Value);
        break;
      case ColorType.RGB:
        this.SerializeRgbColor(writer, tagName, color);
        break;
      case ColorType.Theme:
        this.SerializeThemeColor(writer, tagName, color);
        break;
      default:
        throw new NotImplementedException();
    }
  }

  private void SerializeBordersCollection(XmlWriter writer, BordersCollection borders)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (borders == null)
      throw new ArgumentNullException(nameof (borders));
    writer.WriteStartElement("border");
    Excel2007Serializator.SerializeAttribute(writer, "diagonalUp", borders[ExcelBordersIndex.DiagonalUp].ShowDiagonalLine, false);
    Excel2007Serializator.SerializeAttribute(writer, "diagonalDown", borders[ExcelBordersIndex.DiagonalDown].ShowDiagonalLine, false);
    this.SerializeBorder(writer, (BorderImpl) borders[ExcelBordersIndex.EdgeLeft]);
    this.SerializeBorder(writer, (BorderImpl) borders[ExcelBordersIndex.EdgeRight]);
    this.SerializeBorder(writer, (BorderImpl) borders[ExcelBordersIndex.EdgeTop]);
    this.SerializeBorder(writer, (BorderImpl) borders[ExcelBordersIndex.EdgeBottom]);
    this.SerializeBorder(writer, (BorderImpl) borders[ExcelBordersIndex.DiagonalUp]);
    writer.WriteEndElement();
  }

  private void SerializeBorder(XmlWriter writer, BorderImpl border)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    string localName = border != null ? Excel2007Serializator.GetBorderTag(border.BorderIndex) : throw new ArgumentNullException(nameof (border));
    if (localName == null)
      return;
    writer.WriteStartElement(localName);
    if (border.LineStyle != ExcelLineStyle.None)
    {
      writer.WriteAttributeString("style", this.GetBorderLineStyle(border));
      this.SerializeColorObject(writer, "color", border.ColorObject);
    }
    writer.WriteEndElement();
  }

  private static string GetBorderTag(ExcelBordersIndex borderIndex)
  {
    string borderTag = (string) null;
    Excel2007BorderIndex excel2007BorderIndex = (Excel2007BorderIndex) borderIndex;
    if (excel2007BorderIndex != Excel2007BorderIndex.none)
      borderTag = excel2007BorderIndex.ToString();
    return borderTag;
  }

  private string GetBorderLineStyle(BorderImpl border)
  {
    return Excel2007Serializator.LowerFirstLetter(((Excel2007BorderLineStyle) border.LineStyle).ToString());
  }

  private Dictionary<int, int> SerializeNamedStyleXFs(
    XmlWriter writer,
    int[] arrFillIndexes,
    int[] arrBorderIndexes)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (arrFillIndexes == null)
      throw new ArgumentNullException(nameof (arrFillIndexes));
    if (arrBorderIndexes == null)
      throw new ArgumentNullException(nameof (arrBorderIndexes));
    writer.WriteStartElement("cellStyleXfs");
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    int count1 = innerExtFormats.Count;
    writer.WriteAttributeString("count", count1.ToString());
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    int num = 0;
    int index = 0;
    for (int count2 = innerExtFormats.Count; index < count2; ++index)
    {
      ExtendedFormatImpl format = innerExtFormats[index];
      if (!format.HasParent)
      {
        this.SerializeExtendedFormat(writer, arrFillIndexes, arrBorderIndexes, format, (Dictionary<int, int>) null, true);
        dictionary.Add(format.Index, num);
        ++num;
      }
    }
    writer.WriteEndElement();
    return dictionary;
  }

  private Dictionary<int, int> SerializeNotNamedXFs(
    XmlWriter writer,
    int[] arrFillIndexes,
    int[] arrBorderIndexes,
    Dictionary<int, int> hashNewParentIndexes)
  {
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    int count1 = innerExtFormats.Count;
    int count2 = this.m_book.InnerStyles.Count;
    writer.WriteStartElement("cellXfs");
    int num = 0;
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    for (int index = 0; index < count1; ++index)
    {
      ExtendedFormatImpl format = innerExtFormats[index];
      if (format.HasParent)
      {
        if (!dictionary.ContainsKey(format.Index))
        {
          dictionary.Add(format.Index, num);
          this.SerializeExtendedFormat(writer, arrFillIndexes, arrBorderIndexes, format, hashNewParentIndexes, false);
        }
        ++num;
      }
    }
    writer.WriteEndElement();
    return dictionary;
  }

  private void SerializeExtendedFormat(
    XmlWriter writer,
    int[] arrFillIndexes,
    int[] arrBorderIndexes,
    ExtendedFormatImpl format,
    Dictionary<int, int> newParentIndexes,
    bool defaultApplyValue)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (arrFillIndexes == null)
      throw new ArgumentNullException(nameof (arrFillIndexes));
    if (arrBorderIndexes == null)
      throw new ArgumentNullException(nameof (arrBorderIndexes));
    int index = format != null ? format.Index : throw new ArgumentNullException(nameof (format));
    writer.WriteStartElement("xf");
    writer.WriteAttributeString("numFmtId", format.NumberFormatIndex.ToString());
    writer.WriteAttributeString("fontId", format.FontIndex.ToString());
    writer.WriteAttributeString("fillId", arrFillIndexes[index].ToString());
    writer.WriteAttributeString("borderId", arrBorderIndexes[index].ToString());
    Excel2007Serializator.SerializeAttribute(writer, "pivotButton", format.PivotButton, false);
    if (format.HasParent)
    {
      int parentIndex;
      if (!newParentIndexes.TryGetValue(format.ParentIndex, out parentIndex))
        parentIndex = format.ParentIndex;
      if (newParentIndexes.Count - 1 < parentIndex)
        writer.WriteAttributeString("xfId", newParentIndexes[0].ToString());
      else
        writer.WriteAttributeString("xfId", parentIndex.ToString());
    }
    Excel2007Serializator.SerializeAttribute(writer, "applyAlignment", format.IncludeAlignment, defaultApplyValue);
    Excel2007Serializator.SerializeAttribute(writer, "applyBorder", format.IncludeBorder, defaultApplyValue);
    Excel2007Serializator.SerializeAttribute(writer, "applyFont", format.IncludeFont, defaultApplyValue);
    Excel2007Serializator.SerializeAttribute(writer, "applyNumberFormat", format.IncludeNumberFormat, defaultApplyValue);
    Excel2007Serializator.SerializeAttribute(writer, "applyFill", format.IncludePatterns, defaultApplyValue);
    Excel2007Serializator.SerializeAttribute(writer, "applyProtection", format.IncludeProtection, defaultApplyValue);
    Excel2007Serializator.SerializeAttribute(writer, "quotePrefix", format.IsFirstSymbolApostrophe, false);
    this.SerializeAlignment(writer, format);
    this.SerializeProtection(writer, format);
    writer.WriteEndElement();
  }

  private void SerializeAlignment(XmlWriter writer, ExtendedFormatImpl format)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (this.IsDefaultAlignment(format))
      return;
    writer.WriteStartElement("alignment");
    if (format.HorizontalAlignment != ExcelHAlign.HAlignGeneral)
    {
      string str = ((Excel2007HAlign) format.HorizontalAlignment).ToString();
      writer.WriteAttributeString("horizontal", str);
    }
    if (format.VerticalAlignment != ExcelVAlign.VAlignBottom)
    {
      string str = ((Excel2007VAlign) format.VerticalAlignment).ToString();
      writer.WriteAttributeString("vertical", str);
    }
    Excel2007Serializator.SerializeAttribute(writer, "textRotation", format.Rotation, 0);
    Excel2007Serializator.SerializeAttribute(writer, "wrapText", format.WrapText, false);
    Excel2007Serializator.SerializeAttribute(writer, "indent", format.IndentLevel, 0);
    Excel2007Serializator.SerializeAttribute(writer, "justifyLastLine", format.JustifyLast, false);
    Excel2007Serializator.SerializeAttribute(writer, "shrinkToFit", format.ShrinkToFit, false);
    Excel2007Serializator.SerializeAttribute(writer, "readingOrder", (int) format.ReadingOrder, 0);
    writer.WriteEndElement();
  }

  private bool IsDefaultAlignment(ExtendedFormatImpl format)
  {
    return format.HorizontalAlignment == ExcelHAlign.HAlignGeneral && format.IndentLevel == 0 && !format.JustifyLast && format.ReadingOrder == ExcelReadingOrderType.Context && !format.ShrinkToFit && format.Rotation == 0 && !format.WrapText && format.VerticalAlignment == ExcelVAlign.VAlignBottom;
  }

  private bool IsDefaultAlignment(IPivotCellFormat format)
  {
    return format.HorizontalAlignment == ExcelHAlign.HAlignGeneral && format.IndentLevel == 0 && format.ReadingOrder == ExcelReadingOrderType.Context && !format.ShrinkToFit && format.Rotation == 0 && !format.WrapText && format.VerticalAlignment == ExcelVAlign.VAlignBottom;
  }

  private void SerializeProtection(XmlWriter writer, ExtendedFormatImpl format)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (!format.FormulaHidden && format.Locked)
      return;
    writer.WriteStartElement("protection");
    Excel2007Serializator.SerializeAttribute(writer, "hidden", format.FormulaHidden, false);
    Excel2007Serializator.SerializeAttribute(writer, "locked", format.Locked, true);
    writer.WriteEndElement();
  }

  private void SerializeStyles(XmlWriter writer, Dictionary<int, int> hashNewParentIndexes)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (hashNewParentIndexes == null)
      throw new ArgumentNullException(nameof (hashNewParentIndexes));
    StylesCollection innerStyles = this.m_book.InnerStyles;
    int count = innerStyles.Count;
    writer.WriteStartElement("cellStyles");
    writer.WriteAttributeString("count", count.ToString());
    for (int i = 0; i < count; ++i)
    {
      StyleImpl style = (StyleImpl) innerStyles[i];
      StyleExtRecord styleExt = style.StyleExt;
      if ((styleExt == null || (style.BuiltIn || !styleExt.IsBuildInStyle) && !styleExt.IsHidden) && hashNewParentIndexes.ContainsKey(style.XFormatIndex))
        this.SerializeStyle(writer, style, hashNewParentIndexes);
    }
    writer.WriteEndElement();
  }

  private void SerializeStyle(
    XmlWriter writer,
    StyleImpl style,
    Dictionary<int, int> hashNewParentIndexes)
  {
    if (writer == null)
      throw new ArgumentNullException();
    if (style == null)
      throw new ArgumentNullException(nameof (style));
    if (hashNewParentIndexes == null)
      throw new ArgumentNullException(nameof (hashNewParentIndexes));
    writer.WriteStartElement("cellStyle");
    if (style.IsAsciiConverted)
      writer.WriteAttributeString("name", style.StyleNameCache);
    else
      writer.WriteAttributeString("name", style.Name);
    int hashNewParentIndex = hashNewParentIndexes[style.XFormatIndex];
    writer.WriteAttributeString("xfId", hashNewParentIndex.ToString());
    if (style.BuiltIn)
    {
      StyleRecord record = style.Record;
      writer.WriteAttributeString("builtinId", record.BuildInOrNameLen.ToString());
      if (record.OutlineStyleLevel != byte.MaxValue)
        writer.WriteAttributeString("iLevel", record.OutlineStyleLevel.ToString());
    }
    writer.WriteEndElement();
  }

  private void SerializeDictionary(
    XmlWriter writer,
    IDictionary<string, string> toSerialize,
    string tagName,
    string keyAttributeName,
    string valueAttributeName,
    IFileNamePreprocessor keyPreprocessor)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (toSerialize == null)
      throw new ArgumentNullException(nameof (toSerialize));
    if (tagName == null || tagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (tagName));
    if (keyAttributeName == null || keyAttributeName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (keyAttributeName));
    if (valueAttributeName == null || valueAttributeName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (valueAttributeName));
    foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) toSerialize)
    {
      string key = keyValuePair.Key;
      string str = keyValuePair.Value;
      writer.WriteStartElement(tagName);
      writer.WriteAttributeString(keyAttributeName, key);
      writer.WriteAttributeString(valueAttributeName, str);
      writer.WriteEndElement();
    }
  }

  public static string LowerFirstLetter(string value)
  {
    return char.ToLower(value[0]).ToString() + value.Remove(0, 1);
  }

  public static string CapitalizeFirstLetter(string value)
  {
    return value.Length > 0 ? char.ToUpper(value[0]).ToString() + value.Remove(0, 1) : value;
  }

  internal static void SerializeAttribute(
    XmlWriter writer,
    string attributeName,
    bool value,
    bool defaultValue)
  {
    if (value == defaultValue)
      return;
    string str = value ? "1" : "0";
    writer.WriteAttributeString(attributeName, str);
  }

  internal static void SerializeBool(XmlWriter writer, string attributeName, bool value)
  {
    string str = value ? "1" : "0";
    writer.WriteAttributeString(attributeName, str);
  }

  internal static void SerializeAttribute(
    XmlWriter writer,
    string attributeName,
    int value,
    int defaultValue)
  {
    if (value == defaultValue)
      return;
    string str = value.ToString();
    writer.WriteAttributeString(attributeName, str);
  }

  internal static void SerializeAttribute(
    XmlWriter writer,
    string attributeName,
    double value,
    double defaultValue)
  {
    Excel2007Serializator.SerializeAttribute(writer, attributeName, value, defaultValue, (string) null);
  }

  internal static void SerializeAttribute(
    XmlWriter writer,
    string attributeName,
    double value,
    double defaultValue,
    string attributeNamespace)
  {
    if (value == defaultValue)
      return;
    string str = XmlConvert.ToString(value);
    writer.WriteAttributeString(attributeName, attributeNamespace, str);
  }

  internal static void SerializeAttribute(
    XmlWriter writer,
    string attributeName,
    string value,
    string defaultValue)
  {
    if (!(value != defaultValue))
      return;
    writer.WriteAttributeString(attributeName, value);
  }

  internal static void SerializeAttribute(
    XmlWriter writer,
    string attributeName,
    Enum value,
    Enum defaultValue)
  {
    if (value == defaultValue)
      return;
    writer.WriteAttributeString(attributeName, Excel2007Serializator.LowerFirstLetter(value.ToString()));
  }

  protected static void SerializeElementString(
    XmlWriter writer,
    string elementName,
    string value,
    string defaultValue)
  {
    if (!(value != defaultValue))
      return;
    writer.WriteElementString(elementName, value);
  }

  private static void SerializeElementString(
    XmlWriter writer,
    string elementName,
    string value,
    string defaultValue,
    string prefix)
  {
    if (!(value != defaultValue))
      return;
    writer.WriteElementString(prefix, elementName, (string) null, value);
  }

  private static void SerializeElementString(
    XmlWriter writer,
    string elementName,
    int value,
    int defaultValue)
  {
    if (value == defaultValue)
      return;
    string str = value.ToString();
    writer.WriteElementString(elementName, str);
  }

  [Obsolete("This method is obsolete and will be removed soon. Please use SerializeSheet(XmlWriter writer, WorksheetBaseImpl sheet) method. Sorry for inconvenience.")]
  public void SeiralizeSheet(XmlWriter writer, WorksheetBaseImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    throw new NotImplementedException();
  }

  public void SerializeSheet(XmlWriter writer, WorksheetBaseImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    throw new NotImplementedException();
  }

  public void SerializeSheetData(
    XmlWriter writer,
    CellRecordCollection cells,
    Dictionary<int, int> hashNewParentIndexes,
    string cellTag,
    Dictionary<string, string> additionalAttributes,
    bool isSpansNeeded)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (cells == null)
      throw new ArgumentNullException(nameof (cells));
    writer.WriteStartElement("sheetData");
    this.SerializeAttributes(writer, additionalAttributes);
    int firstRow = cells.FirstRow;
    for (int lastRow = cells.LastRow; firstRow <= lastRow; ++firstRow)
    {
      if (cells.ContainsRow(firstRow - 1))
      {
        RowStorage row = cells.Table.Rows[firstRow - 1];
        this.SerializeRow(writer, row, cells, firstRow - 1, hashNewParentIndexes, cellTag, isSpansNeeded);
      }
    }
    writer.WriteEndElement();
  }

  private void SerializeAttributes(
    XmlWriter writer,
    Dictionary<string, string> additionalAttributes)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (additionalAttributes == null || additionalAttributes.Count == 0)
      return;
    foreach (KeyValuePair<string, string> additionalAttribute in additionalAttributes)
      writer.WriteAttributeString(additionalAttribute.Key, additionalAttribute.Value);
  }

  private void SerializeRow(
    XmlWriter writer,
    RowStorage row,
    CellRecordCollection cells,
    int iRowIndex,
    Dictionary<int, int> hashNewParentIndexes,
    string cellTag,
    bool isSpansNeeded)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (row == null)
      throw new ArgumentNullException(nameof (row));
    writer.WriteStartElement(nameof (row));
    Excel2007Serializator.SerializeAttribute(writer, "r", (iRowIndex + 1).ToString(), string.Empty);
    if (isSpansNeeded && row.FirstColumn >= 0)
    {
      string str = $"{(row.FirstColumn + 1).ToString()}:{(row.LastColumn + 1).ToString()}";
      Excel2007Serializator.SerializeAttribute(writer, "spans", str, string.Empty);
    }
    if (hashNewParentIndexes != null && hashNewParentIndexes.Count > 0)
    {
      if (hashNewParentIndexes.ContainsKey((int) row.ExtendedFormatIndex) && (int) row.ExtendedFormatIndex != this.m_book.DefaultXFIndex)
        Excel2007Serializator.SerializeAttribute(writer, "s", hashNewParentIndexes[(int) row.ExtendedFormatIndex], 0);
      Excel2007Serializator.SerializeAttribute(writer, "customFormat", row.IsFormatted, false);
      Excel2007Serializator.SerializeAttribute(writer, "ht", (double) row.Height / 20.0, this.m_worksheetImpl.StandardHeight);
    }
    Excel2007Serializator.SerializeAttribute(writer, "collapsed", row.IsCollapsed, false);
    Excel2007Serializator.SerializeAttribute(writer, "customHeight", row.IsBadFontHeight, false);
    Excel2007Serializator.SerializeAttribute(writer, "hidden", row.IsHidden, false);
    Excel2007Serializator.SerializeAttribute(writer, "outlineLevel", (int) row.OutlineLevel, 0);
    Excel2007Serializator.SerializeAttribute(writer, "thickTop", row.IsSpaceAboveRow, false);
    Excel2007Serializator.SerializeAttribute(writer, "thickBot", row.IsSpaceBelowRow, false);
    this.SerializeCells(writer, row, cells, hashNewParentIndexes, cellTag);
    writer.WriteEndElement();
  }

  private void SerializeCells(
    XmlWriter writer,
    RowStorage row,
    CellRecordCollection cells,
    Dictionary<int, int> hashNewParentIndexes,
    string cellTag)
  {
    lock (Excel2007Serializator.m_object)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (row == null)
        throw new ArgumentNullException(nameof (row));
      if (cells == null)
        throw new ArgumentNullException(nameof (cells));
      RowStorageEnumerator enumerator = row.GetEnumerator(this.m_recordExtractor) as RowStorageEnumerator;
      while (enumerator.MoveNext())
      {
        if (enumerator.Current is BiffRecordRaw current)
        {
          switch (current.TypeCode)
          {
            case TBIFFRecord.MulRK:
              this.SerializeMulRKRecordValues(writer, (MulRKRecord) current, hashNewParentIndexes);
              continue;
            case TBIFFRecord.MulBlank:
              this.SerializeMulBlankRecord(writer, (MulBlankRecord) current, hashNewParentIndexes);
              continue;
            case TBIFFRecord.Blank:
              BlankRecord blankRecord = (BlankRecord) current;
              this.SerializeBlankCell(writer, blankRecord.Row + 1, blankRecord.Column + 1, (int) blankRecord.ExtendedFormatIndex, hashNewParentIndexes);
              continue;
            case TBIFFRecord.Table:
              continue;
            default:
              this.SerializeCell(writer, current, enumerator, cells, hashNewParentIndexes, cellTag);
              continue;
          }
        }
      }
    }
  }

  private void SerializeCell(
    XmlWriter writer,
    BiffRecordRaw record,
    RowStorageEnumerator rowStorageEnumerator,
    CellRecordCollection cells,
    Dictionary<int, int> hashNewParentIndexes,
    string cellTag)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    if (cells == null)
      throw new ArgumentNullException(nameof (cells));
    if (rowStorageEnumerator == null)
      throw new ArgumentNullException(nameof (rowStorageEnumerator));
    writer.WriteStartElement(cellTag);
    ICellPositionFormat cellPositionFormat = record as ICellPositionFormat;
    string cellName = RangeImpl.GetCellName(cellPositionFormat.Column + 1, cellPositionFormat.Row + 1);
    Excel2007Serializator.SerializeAttribute(writer, "r", cellName, (string) null);
    if (hashNewParentIndexes != null && hashNewParentIndexes.Count > 0)
    {
      int extendedFormatIndex = (int) cellPositionFormat.ExtendedFormatIndex;
      int num = !hashNewParentIndexes.ContainsKey(extendedFormatIndex) ? extendedFormatIndex - 1 : hashNewParentIndexes[extendedFormatIndex];
      Excel2007Serializator.SerializeAttribute(writer, "s", num, 0);
    }
    string strCellType;
    Excel2007Serializator.CellType cellType = Excel2007Serializator.GetCellDataType(record, out strCellType);
    string inlineValue = (string) null;
    if (this.Worksheet.InlineStrings.TryGetValue(cellName, out inlineValue))
    {
      cellType = Excel2007Serializator.CellType.inlineStr;
      strCellType = "inlineStr";
    }
    Excel2007Serializator.SerializeAttribute(writer, "t", strCellType, "n");
    if (record.TypeCode == TBIFFRecord.Formula)
    {
      FormulaRecord formulaRecord = (FormulaRecord) record;
      ArrayRecord arrayRecord;
      if ((arrayRecord = rowStorageEnumerator.GetArrayRecord()) != null)
      {
        if (this.m_formulaUtil.ParsePtgArray(arrayRecord.Formula, 0, 0, false, true).Trim().StartsWith("_xlfn.UNIQUE"))
          Excel2007Serializator.SerializeAttribute(writer, "cm", true, false);
        this.SerializeArrayFormula(writer, arrayRecord);
      }
      else
        this.SerializeSimpleFormula(writer, formulaRecord, cells, cellPositionFormat.Row + 1, cellPositionFormat.Column + 1);
      this.SerializeFormulaValue(writer, formulaRecord, cellType, rowStorageEnumerator);
    }
    else if (cells.Sheet != null && cells.Sheet is WorksheetImpl && (cells.Sheet as WorksheetImpl).CellFormulas != null && (cells.Sheet as WorksheetImpl).CellFormulas.ContainsKey(RangeImpl.GetCellIndex(cellPositionFormat.Column + 1, cellPositionFormat.Row + 1)))
      this.SerializeDataTableFormula(writer, (cells.Sheet as WorksheetImpl).CellFormulas[RangeImpl.GetCellIndex(cellPositionFormat.Column + 1, cellPositionFormat.Row + 1)]);
    this.SerializeCellValue(writer, record, cellType, inlineValue);
    writer.WriteEndElement();
  }

  private void SerializeBlankCell(
    XmlWriter writer,
    int iRowIndex,
    int iColumnIndex,
    int iXFIndex,
    Dictionary<int, int> hashNewParentIndexes)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (hashNewParentIndexes == null)
      throw new ArgumentNullException(nameof (hashNewParentIndexes));
    writer.WriteStartElement("c");
    Excel2007Serializator.SerializeAttribute(writer, "r", RangeImpl.GetCellName(iColumnIndex, iRowIndex), string.Empty);
    int num;
    if (!hashNewParentIndexes.TryGetValue(iXFIndex, out num))
      num = !hashNewParentIndexes.ContainsKey(iXFIndex - 1) ? iXFIndex - 1 : this.m_book.DefaultXFIndex;
    Excel2007Serializator.SerializeAttribute(writer, "s", num, 0);
    writer.WriteEndElement();
  }

  private void SerializeMulBlankRecord(
    XmlWriter writer,
    MulBlankRecord mulBlankRecord,
    Dictionary<int, int> hashNewParentIndexes)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (mulBlankRecord == null)
      throw new ArgumentNullException(nameof (mulBlankRecord));
    if (hashNewParentIndexes == null)
      throw new ArgumentNullException(nameof (hashNewParentIndexes));
    int iRowIndex = mulBlankRecord.Row + 1;
    int num = mulBlankRecord.FirstColumn + 1;
    List<ushort> extendedFormatIndexes = mulBlankRecord.ExtendedFormatIndexes;
    int index = 0;
    for (int count = extendedFormatIndexes.Count; index < count; ++index)
      this.SerializeBlankCell(writer, iRowIndex, num + index, Convert.ToInt32(extendedFormatIndexes[index]), hashNewParentIndexes);
  }

  private void SerializeMulRKRecordValues(
    XmlWriter writer,
    MulRKRecord mulRKRecord,
    Dictionary<int, int> hashNewParentIndexes)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (mulRKRecord == null)
      throw new ArgumentNullException("mulRkRecord");
    if (hashNewParentIndexes == null)
      throw new ArgumentNullException(nameof (hashNewParentIndexes));
    int firstRow = mulRKRecord.Row + 1;
    int num1 = mulRKRecord.FirstColumn + 1;
    List<MulRKRecord.RkRec> records = mulRKRecord.Records;
    int index = 0;
    for (int count = records.Count; index < count; ++index)
    {
      writer.WriteStartElement("c");
      MulRKRecord.RkRec rkRec = records[index];
      Excel2007Serializator.SerializeAttribute(writer, "r", RangeImpl.GetCellName(num1 + index, firstRow), string.Empty);
      int extFormatIndex = (int) rkRec.ExtFormatIndex;
      int num2 = 0;
      if (hashNewParentIndexes.ContainsKey(extFormatIndex))
        num2 = hashNewParentIndexes[extFormatIndex];
      Excel2007Serializator.SerializeAttribute(writer, "s", num2, 0);
      writer.WriteElementString("v", XmlConvert.ToString(rkRec.RkNumber));
      writer.WriteEndElement();
    }
  }

  private void SerializeArrayFormula(XmlWriter writer, ArrayRecord arrayRecord)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (arrayRecord == null)
      throw new ArgumentNullException(nameof (arrayRecord));
    writer.WriteStartElement("f");
    writer.WriteAttributeString("t", Excel2007Serializator.FormulaType.array.ToString());
    writer.WriteAttributeString("aca", "true");
    string ptgArray = this.m_formulaUtil.ParsePtgArray(arrayRecord.Formula, 0, 0, false, true);
    if (ptgArray.Length > 8000)
      throw new ApplicationException($"Formula length is too big. Maximum formula length is {(object) 8000}.");
    string addressLocal = RangeImpl.GetAddressLocal(arrayRecord.FirstRow + 1, arrayRecord.FirstColumn + 1, arrayRecord.LastRow + 1, arrayRecord.LastColumn + 1);
    writer.WriteAttributeString("ref", addressLocal);
    writer.WriteString(ptgArray);
    writer.WriteEndElement();
  }

  private void SerializeSimpleFormula(
    XmlWriter writer,
    FormulaRecord formulaRecord,
    CellRecordCollection cells,
    int row,
    int column)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (formulaRecord == null)
      throw new ArgumentNullException(nameof (formulaRecord));
    if (cells == null)
      throw new ArgumentNullException(nameof (cells));
    string text = string.Empty;
    Ptg ptg = (Ptg) null;
    if (formulaRecord.Formula != null && formulaRecord.Formula.Length != 0)
    {
      ptg = formulaRecord.Formula[0];
      if (ptg.TokenCode == FormulaToken.tExp)
      {
        ControlPtg controlPtg = ptg as ControlPtg;
        if (cells.Table.Rows[controlPtg.RowIndex].HasFormulaArrayRecord(controlPtg.ColumnIndex))
          return;
      }
      this.m_formulaUtil.CheckFormulaVersion(formulaRecord.Formula);
      text = this.m_formulaUtil.ParsePtgArray(formulaRecord.Formula, 0, 0, false, (NumberFormatInfo) null, false, true, (IWorksheet) cells.Sheet);
    }
    else
      this.m_worksheetImpl.FormulaValues.TryGetValue(RangeImpl.GetCellIndex(column, row), out text);
    if (!(text != string.Empty) || text == null || ptg != null && (ptg == null || ptg.TokenCode == FormulaToken.tTbl))
      return;
    if (text[0] == '=')
      text = UtilityMethods.RemoveFirstCharUnsafe(text);
    if (text.Length > 8000)
      throw new ApplicationException($"Formula length is too big. Maximum formula length is {(object) 8000}.");
    writer.WriteStartElement("f");
    bool flag = !this.m_formulaUtil.HasExternalReference(formulaRecord.Formula) && (cells.Sheet.Workbook as WorkbookImpl).IsCellModified || formulaRecord.CalculateOnOpen;
    Excel2007Serializator.SerializeAttribute(writer, "ca", flag, false);
    writer.WriteString(text);
    writer.WriteEndElement();
  }

  private void SerializeDataTableFormula(XmlWriter writer, CellFormula cellFormula)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (cellFormula == null)
      throw new ArgumentNullException(nameof (cellFormula));
    writer.WriteStartElement("f");
    writer.WriteAttributeString("t", Excel2007Serializator.FormulaType.dataTable.ToString());
    writer.WriteAttributeString("ref", cellFormula.Reference);
    writer.WriteAttributeString("dt2D", cellFormula.DataTable2D ? "1" : "0");
    writer.WriteAttributeString("dtr", cellFormula.DataTableRow ? "1" : "0");
    Excel2007Serializator.SerializeAttribute(writer, "r1", cellFormula.FirstDataTableCell, string.Empty);
    Excel2007Serializator.SerializeAttribute(writer, "r2", cellFormula.SecondDataTableCell, string.Empty);
    writer.WriteEndElement();
  }

  private void SerializeCellValue(
    XmlWriter writer,
    BiffRecordRaw record,
    Excel2007Serializator.CellType cellType,
    string inlineValue)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    switch (record.TypeCode)
    {
      case TBIFFRecord.LabelSST:
        if (cellType == Excel2007Serializator.CellType.inlineStr)
        {
          writer.WriteStartElement("is");
          writer.WriteElementString("t", inlineValue);
          writer.WriteEndElement();
          break;
        }
        writer.WriteElementString("v", (record as LabelSSTRecord).SSTIndex.ToString());
        break;
      case TBIFFRecord.Number:
      case TBIFFRecord.RK:
        writer.WriteElementString("v", XmlConvert.ToString(((IDoubleValue) record).DoubleValue));
        break;
      case TBIFFRecord.Label:
        writer.WriteElementString("v", ((LabelRecord) record).Label);
        break;
      case TBIFFRecord.BoolErr:
        BoolErrRecord boolErrRecord = (BoolErrRecord) record;
        string str = !boolErrRecord.IsErrorCode ? boolErrRecord.BoolOrError.ToString() : FormulaUtil.ErrorCodeToName[(int) boolErrRecord.BoolOrError];
        writer.WriteElementString("v", str);
        break;
    }
  }

  private void SerializeFormulaValue(
    XmlWriter writer,
    FormulaRecord record,
    Excel2007Serializator.CellType cellType,
    RowStorageEnumerator rowStorageEnumerator)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    if (rowStorageEnumerator == null)
      throw new ArgumentNullException(nameof (rowStorageEnumerator));
    switch (cellType)
    {
      case Excel2007Serializator.CellType.b:
        string str1 = record.BooleanValue ? "1" : "0";
        writer.WriteElementString("v", str1);
        break;
      case Excel2007Serializator.CellType.e:
        string str2 = FormulaUtil.ErrorCodeToName[(int) record.ErrorValue];
        writer.WriteElementString("v", str2);
        break;
      case Excel2007Serializator.CellType.n:
        if (double.IsNaN(record.DoubleValue))
          break;
        writer.WriteElementString("v", XmlConvert.ToString(record.DoubleValue));
        break;
      case Excel2007Serializator.CellType.str:
        writer.WriteElementString("v", rowStorageEnumerator.GetFormulaStringValue());
        break;
    }
  }

  internal static Excel2007Serializator.CellType GetCellDataType(
    BiffRecordRaw record,
    out string strCellType)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    Excel2007Serializator.CellType cellDataType;
    switch (record.TypeCode)
    {
      case TBIFFRecord.Formula:
        FormulaRecord formulaRecord = (FormulaRecord) record;
        if (formulaRecord.IsBool)
        {
          cellDataType = Excel2007Serializator.CellType.b;
          strCellType = "b";
          break;
        }
        if (formulaRecord.IsError)
        {
          cellDataType = Excel2007Serializator.CellType.e;
          strCellType = "e";
          break;
        }
        if (formulaRecord.HasString)
        {
          cellDataType = Excel2007Serializator.CellType.str;
          strCellType = "str";
          break;
        }
        cellDataType = Excel2007Serializator.CellType.n;
        strCellType = "n";
        break;
      case TBIFFRecord.MulRK:
      case TBIFFRecord.Number:
      case TBIFFRecord.RK:
        cellDataType = Excel2007Serializator.CellType.n;
        strCellType = "n";
        break;
      case TBIFFRecord.RString:
      case TBIFFRecord.LabelSST:
        cellDataType = Excel2007Serializator.CellType.s;
        strCellType = "s";
        break;
      case TBIFFRecord.Label:
        cellDataType = Excel2007Serializator.CellType.str;
        strCellType = "str";
        break;
      case TBIFFRecord.BoolErr:
        if (((BoolErrRecord) record).IsErrorCode)
        {
          cellDataType = Excel2007Serializator.CellType.e;
          strCellType = "e";
          break;
        }
        cellDataType = Excel2007Serializator.CellType.b;
        strCellType = "b";
        break;
      default:
        throw new NotImplementedException("type");
    }
    return cellDataType;
  }

  public void SerializeSST(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (this.m_book.HasInlineStrings && this.m_book.SSTStream != null)
    {
      this.m_book.SSTStream.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, this.m_book.SSTStream);
    }
    else
    {
      SSTDictionary innerSst = this.m_book.InnerSST;
      writer.WriteStartDocument();
      writer.WriteStartElement("sst", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      int count = innerSst.Count;
      int labelSstCount = innerSst.GetLabelSSTCount();
      writer.WriteAttributeString("uniqueCount", count.ToString());
      writer.WriteAttributeString("count", labelSstCount.ToString());
      for (int index = 0; index < count; ++index)
      {
        object sstContentByIndex = innerSst.GetSSTContentByIndex(index);
        this.SerializeStringItem(writer, sstContentByIndex);
      }
      writer.WriteEndElement();
    }
  }

  private void SerializeStringItem(XmlWriter writer, object objTextOrString)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (objTextOrString == null)
      throw new ArgumentNullException("text");
    writer.WriteStartElement("si");
    if (objTextOrString is TextWithFormat text && text.FormattingRunsCount > 0)
    {
      this.SerializeRichTextRun(writer, text);
    }
    else
    {
      string text1 = text == null ? objTextOrString.ToString() : text.Text;
      if (text1.Contains("_x000a_"))
        text1 = text1.Replace("_x000a_", '\n'.ToString());
      int length = text1.Length;
      writer.WriteStartElement("t");
      if (length > 0 && (text1[0] == ' ' || text1[length - 1] == ' ' || text1[0] == '\n' || text1[length - 1] == '\n' || text1[0] == '\t' || text1[length - 1] == '\t') || text != null && text.IsPreserved)
        writer.WriteAttributeString("xml", "space", (string) null, "preserve");
      string text2 = this.ReplaceWrongChars(this.PrepareString(text1));
      if (text2.Contains("\r"))
        text2 = text2.Replace("\r", "_x000d_");
      writer.WriteString(text2);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private string ReplaceWrongChars(string strText)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (strText != null)
    {
      int length = strText.Length;
    }
    if (strText.Contains("_x005F_x000d_"))
      strText = strText.Replace("_x005F_x000d_", "_x000d_");
    foreach (char c in strText.ToCharArray())
    {
      int num = (int) c;
      if (num < 32 /*0x20*/ && Array.IndexOf<char>(Excel2007Serializator.allowedChars, c) < 0 || char.IsSurrogate(c))
        stringBuilder.Append($"_x{num.ToString("X4")}_");
      else
        stringBuilder.Append(c);
    }
    return stringBuilder.ToString();
  }

  private string PrepareString(string text)
  {
    StringBuilder stringBuilder = new StringBuilder(text);
    if (text.ToLower() == "_x000d_")
      return stringBuilder.ToString();
    int num1 = 0;
    int num2;
    for (int startIndex1 = 0; startIndex1 < text.Length; startIndex1 = num2)
    {
      int num3 = text.IndexOf("_x", startIndex1);
      if (num3 != -1)
      {
        int startIndex2 = num3 + 2;
        num2 = text.IndexOf("_", startIndex2);
        if (num2 != -1)
        {
          if (num2 - startIndex2 == 4 && Excel2007Serializator.IsHexa(text.Substring(startIndex2, 4)))
          {
            stringBuilder.Insert(startIndex2 - 2 + num1, "_x005F");
            num1 += "_x005F".Length;
          }
        }
        else
          break;
      }
      else
        break;
    }
    return stringBuilder.ToString();
  }

  private static bool IsHexa(string value)
  {
    return int.TryParse(value, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out int _);
  }

  private void SerializeRichTextRun(XmlWriter writer, TextWithFormat text)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    text.Defragment();
    FontsCollection innerFonts = this.m_book.InnerFonts;
    SortedList<int, int> formattingRuns = text.FormattingRuns;
    string text1 = text.Text;
    string str = string.Empty;
    int iFontIndex = -1;
    int startIndex = 0;
    int length1 = text1.Length;
    foreach (KeyValuePair<int, int> keyValuePair in formattingRuns)
    {
      int length2 = keyValuePair.Key - startIndex;
      if (length1 >= length2)
        str = text1.Substring(startIndex, length2);
      str = this.ReplaceWrongChars(str);
      if (iFontIndex >= this.m_book.InnerFonts.Count)
        iFontIndex = text.DefaultFontIndex;
      this.SerializeRichTextRunSingleEntry(writer, innerFonts, str, iFontIndex);
      iFontIndex = keyValuePair.Value;
      startIndex += length2;
    }
    if (length1 >= startIndex)
      str = text1.Substring(startIndex);
    string strString = this.ReplaceWrongChars(str);
    if (iFontIndex >= this.m_book.InnerFonts.Count || iFontIndex == -1)
      iFontIndex = text.DefaultFontIndex;
    this.SerializeRichTextRunSingleEntry(writer, innerFonts, strString, iFontIndex);
  }

  private void SerializeRichTextRunSingleEntry(
    XmlWriter writer,
    FontsCollection fonts,
    string strString,
    int iFontIndex)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (fonts == null)
      throw new ArgumentNullException(nameof (fonts));
    if (strString == null)
      throw new ArgumentNullException(nameof (strString));
    if (strString.Contains("_x000a_"))
      strString = strString.Replace("_x000a_", '\n'.ToString());
    if (!strString.Contains("\r\n"))
      strString = strString.Replace("\n", "\r\n");
    writer.WriteStartElement("r");
    if (iFontIndex != -1)
    {
      IFont font = fonts[iFontIndex];
      this.SerializeFont(writer, font, "rPr");
    }
    writer.WriteStartElement("t");
    int length = strString.Length;
    if (length > 0)
    {
      char ch = strString[length - 1];
      if (strString[0] == ' ' || ch == ' ' || strString.StartsWith("\r\n") || strString.EndsWith("\r\n") || strString.EndsWith("\t") || strString.StartsWith("\t"))
        writer.WriteAttributeString("xml", "space", (string) null, "preserve");
    }
    writer.WriteValue(strString);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeColors(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (this.IsPaletteDefault())
      return;
    writer.WriteStartElement("colors");
    this.SerializePalette(writer);
    writer.WriteEndElement();
  }

  private void SerializePalette(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("indexedColors");
    Color[] palette = this.m_book.Palette;
    int index = 0;
    for (int length = palette.Length; index < length; ++index)
    {
      Color color = palette[index];
      this.SerializeRgbColor(writer, "rgbColor", (ColorObject) color);
    }
    writer.WriteEndElement();
  }

  private bool IsPaletteDefault()
  {
    List<Color> innerPalette = this.m_book.InnerPalette;
    Color[] defPalette = WorkbookImpl.DEF_PALETTE;
    bool flag = true;
    int index = 0;
    for (int count = innerPalette.Count; index < count; ++index)
    {
      Color color1 = innerPalette[index];
      Color color2 = defPalette[index];
      if (color1.ToArgb() != color2.ToArgb())
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  private void SerializeColumns(
    XmlWriter writer,
    WorksheetImpl sheet,
    Dictionary<int, int> dicStyles)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (dicStyles == null)
      throw new ArgumentNullException(nameof (dicStyles));
    ColumnInfoRecord[] columnInformation = sheet.ColumnInformation;
    bool flag = true;
    double standardWidth = sheet.StandardWidth;
    int index = 1;
    for (int maxColumnCount = this.m_book.MaxColumnCount; index <= maxColumnCount; ++index)
    {
      ColumnInfoRecord columnInfo = columnInformation[index];
      if (columnInfo != null)
      {
        if (flag)
          writer.WriteStartElement("cols");
        index = this.SerializeColumn(writer, columnInfo, dicStyles, standardWidth, sheet);
        flag = false;
      }
    }
    if (flag)
      return;
    writer.WriteEndElement();
  }

  private int SerializeColumn(
    XmlWriter writer,
    ColumnInfoRecord columnInfo,
    Dictionary<int, int> dicStyles,
    double defaultWidth,
    WorksheetImpl sheet)
  {
    if (columnInfo == null)
      return int.MaxValue;
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dicStyles == null)
      throw new ArgumentNullException(nameof (dicStyles));
    int sameColumns = Excel2007Serializator.FindSameColumns(sheet, (int) columnInfo.FirstColumn + 1, this.m_book);
    writer.WriteStartElement("col");
    writer.WriteAttributeString("min", ((int) columnInfo.FirstColumn + 1).ToString());
    writer.WriteAttributeString("max", sameColumns.ToString());
    double num1 = (double) sheet.EvaluateFileColumnWidth((int) columnInfo.ColumnWidth) / 256.0;
    if (num1 > (double) sheet.MaxColumnWidth)
      writer.WriteAttributeString("width", sheet.MaxColumnWidth.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo));
    else
      writer.WriteAttributeString("width", num1.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo));
    if ((int) columnInfo.ExtendedFormatIndex != sheet.ParentWorkbook.DefaultXFIndex)
    {
      int extendedFormatIndex = (int) columnInfo.ExtendedFormatIndex;
      int num2;
      if (!dicStyles.TryGetValue(extendedFormatIndex, out num2))
        num2 = extendedFormatIndex;
      writer.WriteAttributeString("style", num2.ToString());
    }
    Excel2007Serializator.SerializeAttribute(writer, "hidden", columnInfo.IsHidden, false);
    Excel2007Serializator.SerializeAttribute(writer, "bestFit", columnInfo.IsBestFit, false);
    Excel2007Serializator.SerializeAttribute(writer, "phonetic", columnInfo.IsPhenotic, false);
    Excel2007Serializator.SerializeAttribute(writer, "customWidth", columnInfo.IsUserSet || defaultWidth != num1, false);
    Excel2007Serializator.SerializeAttribute(writer, "collapsed", columnInfo.IsCollapsed, false);
    Excel2007Serializator.SerializeAttribute(writer, "outlineLevel", (int) columnInfo.OutlineLevel, 0);
    writer.WriteEndElement();
    return sameColumns;
  }

  internal static int FindSameColumns(WorksheetImpl sheet, int iColumnIndex, WorkbookImpl workbook)
  {
    ColumnInfoRecord[] columnInformation = sheet.ColumnInformation;
    ColumnInfoRecord columnInfoRecord1 = columnInformation[iColumnIndex];
    for (; iColumnIndex < workbook.MaxColumnCount; ++iColumnIndex)
    {
      int index = iColumnIndex + 1;
      ColumnInfoRecord columnInfoRecord2 = columnInformation[index];
      if (columnInfoRecord2 == null || (int) columnInfoRecord2.ExtendedFormatIndex != (int) columnInfoRecord1.ExtendedFormatIndex || (int) columnInfoRecord2.ColumnWidth != (int) columnInfoRecord1.ColumnWidth || columnInfoRecord2.IsCollapsed != columnInfoRecord1.IsCollapsed || columnInfoRecord2.IsHidden != columnInfoRecord1.IsHidden || (int) columnInfoRecord2.OutlineLevel != (int) columnInfoRecord1.OutlineLevel)
        break;
    }
    return iColumnIndex;
  }

  private void SerializeDataValidations(XmlWriter writer, DataValidationTable dataValidationTable)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dataValidationTable == null || dataValidationTable.Count == 0)
      return;
    int index = 0;
    for (int count = dataValidationTable.Count; index < count; ++index)
    {
      DataValidationCollection dataValidationCollection = dataValidationTable[index];
      this.SerializeDataValidationCollection(writer, dataValidationCollection);
    }
  }

  private void SerializeDataValidationCollection(
    XmlWriter writer,
    DataValidationCollection dataValidationCollection)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dataValidationCollection == null || dataValidationCollection.Count == 0)
      return;
    writer.WriteStartElement("dataValidations");
    int num = 0;
    for (int index = 0; index < dataValidationCollection.Count; ++index)
    {
      if (dataValidationCollection[index].DVRanges.Length > 0)
        ++num;
    }
    Excel2007Serializator.SerializeAttribute(writer, "count", num, 0);
    Excel2007Serializator.SerializeAttribute(writer, "disablePrompts", dataValidationCollection.IsPromptBoxVisible, false);
    if (dataValidationCollection.IsPromptBoxPositionFixed)
    {
      Excel2007Serializator.SerializeAttribute(writer, "xWindow", dataValidationCollection.PromptBoxVPosition, 0);
      Excel2007Serializator.SerializeAttribute(writer, "yWindow", dataValidationCollection.PromptBoxHPosition, 0);
    }
    for (int index = 0; index < dataValidationCollection.Count; ++index)
    {
      if (dataValidationCollection[index].DVRanges.Length > 0)
        this.SerializeDataValidation(writer, dataValidationCollection[index]);
    }
    writer.WriteEndElement();
  }

  private void SerializeDataValidation(XmlWriter writer, DataValidationImpl dataValidation)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dataValidation == null)
      throw new ArgumentNullException(nameof (dataValidation));
    writer.WriteStartElement(nameof (dataValidation));
    ExcelDataType allowType = dataValidation.AllowType;
    if (allowType != ExcelDataType.Any)
      writer.WriteAttributeString("type", this.GetDVTypeName(allowType));
    ExcelErrorStyle errorStyle = dataValidation.ErrorStyle;
    if (errorStyle != ExcelErrorStyle.Stop)
      writer.WriteAttributeString("errorStyle", this.GetDVErrorStyleType(errorStyle));
    ExcelDataValidationComparisonOperator compareOperator = dataValidation.CompareOperator;
    if (compareOperator != ExcelDataValidationComparisonOperator.Between)
      writer.WriteAttributeString("operator", this.GetDVCompareOperatorType(compareOperator));
    Excel2007Serializator.SerializeAttribute(writer, "allowBlank", dataValidation.IsEmptyCellAllowed, false);
    Excel2007Serializator.SerializeAttribute(writer, "showDropDown", dataValidation.IsSuppressDropDownArrow, false);
    Excel2007Serializator.SerializeAttribute(writer, "showInputMessage", dataValidation.ShowPromptBox, false);
    Excel2007Serializator.SerializeAttribute(writer, "showErrorMessage", dataValidation.ShowErrorBox, false);
    Excel2007Serializator.SerializeAttribute(writer, "errorTitle", dataValidation.ErrorBoxTitle, string.Empty);
    Excel2007Serializator.SerializeAttribute(writer, "error", dataValidation.ErrorBoxText, string.Empty);
    Excel2007Serializator.SerializeAttribute(writer, "promptTitle", dataValidation.PromptBoxTitle, string.Empty);
    Excel2007Serializator.SerializeAttribute(writer, "prompt", dataValidation.PromptBoxText, string.Empty);
    string str1 = string.Join(" ", dataValidation.DVRanges);
    Excel2007Serializator.SerializeAttribute(writer, "sqref", str1, (string) null);
    if (dataValidation.ChoiceTokens != null)
    {
      this.SerializeAlternateContent(writer, dataValidation);
    }
    else
    {
      string firstSecondFormula1 = dataValidation.GetFirstSecondFormula(this.m_formulaUtil, true);
      string firstSecondFormula2 = dataValidation.GetFirstSecondFormula(this.m_formulaUtil, false);
      if (firstSecondFormula1 != null && firstSecondFormula1 != string.Empty)
      {
        string str2 = firstSecondFormula1.Replace(this.Worksheet.Application.ArgumentsSeparator, ',');
        writer.WriteElementString("formula1", str2);
      }
      if (firstSecondFormula2 != null && firstSecondFormula2 != string.Empty)
      {
        string str3 = firstSecondFormula2.Replace(this.Worksheet.Application.ArgumentsSeparator, ',');
        writer.WriteElementString("formula2", str3);
      }
    }
    writer.WriteEndElement();
  }

  private void SerializeAlternateContent(XmlWriter writer, DataValidationImpl dataValidation)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dataValidation == null)
      throw new ArgumentNullException(nameof (dataValidation));
    writer.WriteStartElement("mc", "AlternateContent", "http://schemas.openxmlformats.org/markup-compatibility/2006");
    writer.WriteAttributeString("xmlns", "x12ac", (string) null, "http://schemas.microsoft.com/office/spreadsheetml/2011/1/ac");
    writer.WriteAttributeString("xmlns", "mc", (string) null, "http://schemas.openxmlformats.org/markup-compatibility/2006");
    writer.WriteStartElement("mc", "Choice", "http://schemas.openxmlformats.org/markup-compatibility/2006");
    writer.WriteAttributeString("Requires", "x12ac");
    writer.WriteStartElement("x12ac", "list", "http://schemas.microsoft.com/office/spreadsheetml/2011/1/ac");
    writer.WriteString(dataValidation.ChoiceTokens);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteStartElement("mc", "Fallback", "http://schemas.openxmlformats.org/markup-compatibility/2006");
    string firstSecondFormula1 = dataValidation.GetFirstSecondFormula(this.m_formulaUtil, true);
    string firstSecondFormula2 = dataValidation.GetFirstSecondFormula(this.m_formulaUtil, false);
    if (firstSecondFormula1 != null && firstSecondFormula1 != string.Empty)
    {
      string str = firstSecondFormula1.Replace(this.Worksheet.Application.ArgumentsSeparator, ',');
      writer.WriteElementString("formula1", str);
    }
    if (firstSecondFormula2 != null && firstSecondFormula2 != string.Empty)
    {
      string str = firstSecondFormula2.Replace(this.Worksheet.Application.ArgumentsSeparator, ',');
      writer.WriteElementString("formula2", str);
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private string GetDVTypeName(ExcelDataType dataType)
  {
    switch (dataType)
    {
      case ExcelDataType.Any:
        return "none";
      case ExcelDataType.Integer:
        return "whole";
      case ExcelDataType.Decimal:
        return "decimal";
      case ExcelDataType.User:
        return "list";
      case ExcelDataType.Date:
        return "date";
      case ExcelDataType.Time:
        return "time";
      case ExcelDataType.TextLength:
        return "textLength";
      case ExcelDataType.Formula:
        return "custom";
      default:
        throw new ArgumentOutOfRangeException(nameof (dataType));
    }
  }

  private string GetDVErrorStyleType(ExcelErrorStyle errorStyle)
  {
    switch (errorStyle)
    {
      case ExcelErrorStyle.Stop:
        return "stop";
      case ExcelErrorStyle.Warning:
        return "warning";
      case ExcelErrorStyle.Info:
        return "information";
      default:
        throw new ArgumentOutOfRangeException(nameof (errorStyle));
    }
  }

  private string GetDVCompareOperatorType(
    ExcelDataValidationComparisonOperator compareOperator)
  {
    switch (compareOperator)
    {
      case ExcelDataValidationComparisonOperator.Between:
        return "between";
      case ExcelDataValidationComparisonOperator.NotBetween:
        return "notBetween";
      case ExcelDataValidationComparisonOperator.Equal:
        return "equal";
      case ExcelDataValidationComparisonOperator.NotEqual:
        return "notEqual";
      case ExcelDataValidationComparisonOperator.Greater:
        return "greaterThan";
      case ExcelDataValidationComparisonOperator.Less:
        return "lessThan";
      case ExcelDataValidationComparisonOperator.GreaterOrEqual:
        return "greaterThanOrEqual";
      case ExcelDataValidationComparisonOperator.LessOrEqual:
        return "lessThanOrEqual";
      default:
        throw new ArgumentOutOfRangeException(nameof (compareOperator));
    }
  }

  public void SerializeAutoFilters(XmlWriter writer, IAutoFilters autoFilters)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (autoFilters == null || autoFilters.Count == 0 || autoFilters.FilterRange == null)
      return;
    writer.WriteStartElement("autoFilter");
    writer.WriteAttributeString("ref", autoFilters.FilterRange.AddressLocal);
    Dictionary<int, int> mergedIndices = new Dictionary<int, int>();
    int num = 0;
    IRange filterRange = autoFilters.FilterRange;
    for (int column = filterRange.Column; column <= filterRange.LastColumn; ++column)
    {
      IRange mergeArea = filterRange.Worksheet[filterRange.Row, column].MergeArea;
      if (mergeArea != null && mergeArea.LastColumn != column && column != filterRange.LastColumn)
      {
        if (column == mergeArea.Column || column == filterRange.Column)
        {
          (autoFilters[num] as AutoFilterImpl).m_showButton = false;
          mergedIndices.Add(num, column - filterRange.Column);
        }
        else
        {
          writer.WriteStartElement("filterColumn");
          Excel2007Serializator.SerializeAttribute(writer, "colId", column - filterRange.Column, 0);
          Excel2007Serializator.SerializeAttribute(writer, "showButton", 0, 1);
          writer.WriteEndElement();
          continue;
        }
      }
      if (mergeArea == null || mergeArea.LastColumn != column && column != filterRange.LastColumn)
        ++num;
    }
    int columnIndex = 0;
    for (int count = autoFilters.Count; columnIndex < count; ++columnIndex)
    {
      AutoFilterImpl autoFilter = (AutoFilterImpl) autoFilters[columnIndex];
      if (autoFilter.IsFiltered || !autoFilter.m_showButton)
        this.SerializeFilterColumn(writer, autoFilter, mergedIndices);
    }
    if (autoFilters.Parent is IListObject)
    {
      writer.WriteEndElement();
      Stream filtersSortStream = this.GetTableFiltersSortStream(autoFilters.FilterRange.Worksheet.Index, (autoFilters.Parent as IListObject).Index);
      this.SerializeStream(writer, filtersSortStream);
    }
    else
    {
      Stream filtersSortStream = this.GetFiltersSortStream(autoFilters.FilterRange.Worksheet.Index);
      this.SerializeStream(writer, filtersSortStream);
      writer.WriteEndElement();
    }
  }

  private void SerializeFilterColumn(
    XmlWriter writer,
    AutoFilterImpl autoFilter,
    Dictionary<int, int> mergedIndices)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (autoFilter == null)
      throw new ArgumentNullException(nameof (autoFilter));
    int key = autoFilter.Index - 1;
    if (mergedIndices.ContainsKey(key))
    {
      key = mergedIndices[autoFilter.Index - 1];
      autoFilter.m_showButton = false;
    }
    writer.WriteStartElement("filterColumn");
    Excel2007Serializator.SerializeAttribute(writer, "colId", key, -1);
    if (!autoFilter.m_showButton)
    {
      int int32 = Convert.ToInt32(!autoFilter.m_showButton);
      Excel2007Serializator.SerializeAttribute(writer, "hiddenButton", int32, 0);
    }
    if (autoFilter.IsTop10)
      this.SerializeAutoFilterTopTen(writer, autoFilter);
    else if (autoFilter.FilterType == ExcelFilterType.CustomFilter && (autoFilter.IsTop && autoFilter.Top10Number > 0 || autoFilter.FirstCondition.DataType != ExcelFilterDataType.NotUsed || autoFilter.SecondCondition.DataType != ExcelFilterDataType.NotUsed))
    {
      this.SerializeCustomFilters(writer, autoFilter);
    }
    else
    {
      switch (autoFilter.FilterType)
      {
        case ExcelFilterType.CombinationFilter:
          this.SerializeFilters(writer, autoFilter);
          break;
        case ExcelFilterType.DynamicFilter:
          this.SerializeDateFilter(writer, autoFilter.FilteredItems as DynamicFilter);
          break;
        case ExcelFilterType.ColorFilter:
          this.SerializeColorFilter(writer);
          break;
        case ExcelFilterType.IconFilter:
          this.SerializeAlternateIconFilter(writer, autoFilter.FilteredItems as IconFilter);
          break;
      }
    }
    writer.WriteEndElement();
  }

  private void SerializeColorFilter(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException("Writer");
    this.SerializeStream(writer, this.GetColorFilterStream(this.m_colorFilterStreamListIndex + 1));
    ++this.m_colorFilterStreamListIndex;
  }

  private void SerializeIconFilter(XmlWriter writer, IconFilter iconFilter)
  {
    if (writer == null)
      throw new ArgumentNullException("Writer");
    if (iconFilter == null)
      throw new ArgumentNullException("IconFilter");
    if (iconFilter.IconSetType.GetHashCode() >= 17 && iconFilter.IconSetType.GetHashCode() <= 19)
      writer.WriteStartElement("x14", nameof (iconFilter), (string) null);
    else
      writer.WriteStartElement(nameof (iconFilter));
    writer.WriteAttributeString("iconSet", iconFilter.IconSetType.GetHashCode() == -1 ? CF.IconSetTypeNames[0] : CF.IconSetTypeNames[(int) iconFilter.IconSetType]);
    if (iconFilter.IconId != -1)
      writer.WriteAttributeString("iconId", iconFilter.IconId.ToString());
    writer.WriteEndElement();
  }

  private void SerializeAlternateIconFilter(XmlWriter writer, IconFilter iconFilter)
  {
    if (iconFilter.IconSetType.GetHashCode() >= 17 && iconFilter.IconSetType.GetHashCode() <= 19)
    {
      writer.WriteStartElement("mc", "AlternateContent", "http://schemas.openxmlformats.org/markup-compatibility/2006");
      writer.WriteStartElement("mc", "Choice", (string) null);
      writer.WriteAttributeString("Requires", "x14");
    }
    this.SerializeIconFilter(writer, iconFilter);
    if (iconFilter.IconSetType.GetHashCode() < 17 || iconFilter.IconSetType.GetHashCode() > 19)
      return;
    writer.WriteEndElement();
    writer.WriteStartElement("mc", "Fallback", (string) null);
    writer.WriteStartElement("customFilters");
    writer.WriteStartElement("customFilter");
    writer.WriteAttributeString("val", "");
    writer.WriteEndElement();
    writer.WriteStartElement("customFilter");
    writer.WriteAttributeString("operator", "notEqual");
    writer.WriteAttributeString("val", " ");
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeFilters(XmlWriter writer, AutoFilterImpl autoFilter)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("filters");
    if (autoFilter.FilteredItems.FilterType == ExcelFilterType.CombinationFilter)
      this.SerializeCombinationFilters(writer, autoFilter.FilteredItems as CombinationFilter);
    writer.WriteEndElement();
  }

  private void SerializeFilter(XmlWriter writer, string strFilterValue)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("filter");
    writer.WriteAttributeString("val", strFilterValue);
    writer.WriteEndElement();
  }

  private void SerializeAutoFilterTopTen(XmlWriter writer, AutoFilterImpl autoFilter)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (autoFilter == null)
      throw new ArgumentNullException(nameof (autoFilter));
    writer.WriteStartElement("top10");
    Excel2007Serializator.SerializeAttribute(writer, "top", autoFilter.IsTop, true);
    Excel2007Serializator.SerializeAttribute(writer, "percent", autoFilter.IsPercent, false);
    Excel2007Serializator.SerializeAttribute(writer, "val", autoFilter.Top10Number, -1);
    writer.WriteEndElement();
  }

  private void SerializeCustomFilters(XmlWriter writer, AutoFilterImpl autoFilter)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (autoFilter == null)
      throw new ArgumentNullException(nameof (autoFilter));
    if (autoFilter.FirstCondition.DataType == ExcelFilterDataType.MatchAllBlanks || autoFilter.SecondCondition.DataType == ExcelFilterDataType.MatchAllBlanks)
    {
      writer.WriteStartElement("filters");
      writer.WriteAttributeString("blank", "1");
      writer.WriteEndElement();
    }
    else
    {
      writer.WriteStartElement("customFilters");
      Excel2007Serializator.SerializeAttribute(writer, "and", autoFilter.IsAnd, false);
      if (autoFilter.IsFirstCondition)
        this.SerializeCustomFilter(writer, autoFilter.FirstCondition);
      if (autoFilter.IsSecondCondition)
        this.SerializeCustomFilter(writer, autoFilter.SecondCondition);
      writer.WriteEndElement();
    }
  }

  private void SerializeCustomFilter(XmlWriter writer, IAutoFilterCondition autoFilterCondition)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (autoFilterCondition == null)
      throw new ArgumentNullException(nameof (autoFilterCondition));
    writer.WriteStartElement("customFilter");
    if (autoFilterCondition.DataType == ExcelFilterDataType.MatchAllNonBlanks)
    {
      Excel2007Serializator.SerializeAttribute(writer, "operator", "notEqual", "equal");
      Excel2007Serializator.SerializeAttribute(writer, "val", " ", (string) null);
    }
    else if (autoFilterCondition.ConditionOperator != (ExcelFilterCondition) 0)
    {
      ExcelFilterCondition conditionOperator = autoFilterCondition.ConditionOperator;
      string conditionOperatorName = this.GetAFConditionOperatorName(conditionOperator);
      Excel2007Serializator.SerializeAttribute(writer, "operator", conditionOperatorName, "equal");
      string str = this.GetAFFilterValue(autoFilterCondition);
      if (conditionOperator == ExcelFilterCondition.Contains || conditionOperator == ExcelFilterCondition.DoesNotContain || conditionOperator == ExcelFilterCondition.BeginsWith || conditionOperator == ExcelFilterCondition.DoesNotBeginWith)
        str += "*";
      if (conditionOperator == ExcelFilterCondition.Contains || conditionOperator == ExcelFilterCondition.DoesNotContain || conditionOperator == ExcelFilterCondition.EndsWith || conditionOperator == ExcelFilterCondition.DoesNotEndWith)
        str = "*" + str;
      Excel2007Serializator.SerializeAttribute(writer, "val", str, (string) null);
    }
    writer.WriteEndElement();
  }

  private string GetAFConditionOperatorName(ExcelFilterCondition filterCondition)
  {
    switch (filterCondition)
    {
      case ExcelFilterCondition.Less:
        return "lessThan";
      case ExcelFilterCondition.Equal:
      case ExcelFilterCondition.Contains:
      case ExcelFilterCondition.BeginsWith:
      case ExcelFilterCondition.EndsWith:
        return "equal";
      case ExcelFilterCondition.LessOrEqual:
        return "lessThanOrEqual";
      case ExcelFilterCondition.Greater:
        return "greaterThan";
      case ExcelFilterCondition.NotEqual:
      case ExcelFilterCondition.DoesNotContain:
      case ExcelFilterCondition.DoesNotBeginWith:
      case ExcelFilterCondition.DoesNotEndWith:
        return "notEqual";
      case ExcelFilterCondition.GreaterOrEqual:
        return "greaterThanOrEqual";
      default:
        throw new ArgumentOutOfRangeException(nameof (filterCondition));
    }
  }

  private ushort GetActivePane(PaneRecord paneRecord)
  {
    if (paneRecord != null)
    {
      if (paneRecord.VerticalSplit == 0 && paneRecord.HorizontalSplit == 0)
        paneRecord.ActivePane = (ushort) 3;
      else if (paneRecord.VerticalSplit == 0)
        paneRecord.ActivePane = (ushort) 2;
      else if (paneRecord.HorizontalSplit == 0)
        paneRecord.ActivePane = (ushort) 1;
    }
    return paneRecord.ActivePane;
  }

  private string GetAFFilterValue(IAutoFilterCondition autoFilterCondition)
  {
    switch (autoFilterCondition.DataType)
    {
      case ExcelFilterDataType.FloatingPoint:
        return autoFilterCondition.Double.ToString();
      case ExcelFilterDataType.String:
        return autoFilterCondition.String;
      case ExcelFilterDataType.Boolean:
        return !autoFilterCondition.Boolean ? "0" : "1";
      case ExcelFilterDataType.ErrorCode:
        return FormulaUtil.ErrorCodeToName[(int) autoFilterCondition.ErrorCode];
      default:
        throw new ArgumentOutOfRangeException("dataType");
    }
  }

  private void SerializeCondionalFormats(
    XmlWriter writer,
    XmlWriter writerDxf,
    ConditionalFormats conditionalFormats,
    ref int iDxfIndex,
    ref int iPriority,
    ref int iPriorityCount)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (writerDxf == null)
      throw new ArgumentNullException(nameof (writerDxf));
    int num = conditionalFormats != null ? conditionalFormats.Count : throw new ArgumentNullException(nameof (conditionalFormats));
    if (num == 0)
      return;
    bool flag = false;
    foreach (ConditionalFormatImpl conditionalFormat in (CollectionBase<IConditionalFormat>) conditionalFormats)
    {
      if (conditionalFormat.Range == null && conditionalFormats.CellsList.Length <= 1 && conditionalFormat.FormatType != ExcelCFType.SpecificText)
        conditionalFormat.Range = conditionalFormats.sheet.Range[conditionalFormats.Address];
      if (conditionalFormat.FormatType == ExcelCFType.IconSet)
      {
        if ((conditionalFormat.IconSet as IconSetImpl).IsCustom)
          conditionalFormat.CFHasExtensionList = true;
        else if (conditionalFormat.IconSet.IconSet == ExcelIconSetType.ThreeTriangles || conditionalFormat.IconSet.IconSet == ExcelIconSetType.ThreeStars || conditionalFormat.IconSet.IconSet == ExcelIconSetType.FiveBoxes)
          conditionalFormat.CFHasExtensionList = true;
      }
      if (!conditionalFormat.CFHasExtensionList)
        flag = true;
    }
    if (conditionalFormats.IsFutureRecord || !flag)
      return;
    writer.WriteStartElement("conditionalFormatting");
    string[] cellsList = conditionalFormats.CellsList;
    string str = cellsList.Length > 0 ? string.Join(" ", cellsList) : conditionalFormats.Address.ToString();
    Excel2007Serializator.SerializeAttribute(writer, "sqref", str, (string) null);
    Excel2007Serializator.SerializeAttribute(writer, "pivot", conditionalFormats.Pivot, false);
    int iPriorityCount1 = num + iPriorityCount;
    iPriorityCount += num;
    for (int i = 0; i < num; ++i)
    {
      IInternalConditionalFormat conditionalFormat = conditionalFormats[i] as IInternalConditionalFormat;
      if (!(conditionalFormat as ConditionalFormatImpl).CFHasExtensionList)
        this.SerializeCondition(writer, writerDxf, conditionalFormat, ref iDxfIndex, ref iPriority, ref iPriorityCount1);
    }
    writer.WriteEndElement();
  }

  internal void SerializeCondition(
    XmlWriter writer,
    XmlWriter writerDxf,
    IInternalConditionalFormat condition,
    ref int iDxfIndex,
    ref int iPriority,
    ref int iPriorityCount)
  {
    ConditionalFormatImpl condition1 = condition as ConditionalFormatImpl;
    IconSetImpl iconSet = condition1.IconSet as IconSetImpl;
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (writerDxf == null && (condition1.FormatType != ExcelCFType.IconSet || !condition1.CFHasExtensionList && !iconSet.IsCustom))
      throw new ArgumentNullException(nameof (writerDxf));
    if (condition == null)
      throw new ArgumentNullException(nameof (condition));
    ExcelCFType formatType = condition1.FormatType;
    ExcelComparisonOperator compOperator = condition1.Operator;
    CFTimePeriods timePeriodType = condition1.TimePeriodType;
    if (condition1.FormatType == ExcelCFType.IconSet && (condition1.CFHasExtensionList || iconSet.IsCustom))
      writer.WriteStartElement("x14", "cfRule", (string) null);
    else
      writer.WriteStartElement("cfRule");
    writer.WriteAttributeString("type", this.GetCFType(formatType, compOperator));
    if (condition1.IsBackgroundColorPresent || condition1.IsFontFormatPresent || condition1.IsBorderFormatPresent || condition1.IsPatternFormatPresent || condition1.HasNumberFormatPresent)
    {
      this.SerializeDxf(writerDxf, condition1);
      Excel2007Serializator.SerializeAttribute(writer, "dxfId", iDxfIndex, int.MinValue);
      if (this.hasTextRotation)
        --iDxfIndex;
      else
        ++iDxfIndex;
    }
    Excel2007Serializator.SerializeAttribute(writer, "stopIfTrue", condition.StopIfTrue, false);
    if (formatType == ExcelCFType.CellValue)
      Excel2007Serializator.SerializeAttribute(writer, "operator", this.GetCFComparisonOperatorName(condition.Operator), string.Empty);
    if (formatType == ExcelCFType.SpecificText)
    {
      Excel2007Serializator.SerializeAttribute(writer, "operator", this.GetCFComparisonOperatorName(condition.Operator), string.Empty);
      writer.WriteAttributeString("text", condition1.Text);
    }
    if (formatType == ExcelCFType.TimePeriod)
      Excel2007Serializator.SerializeAttribute(writer, "timePeriod", this.GetCFTimePeriodType(timePeriodType), string.Empty);
    ConditionalFormatImpl conditionalFormatImpl1 = condition as ConditionalFormatImpl;
    if (conditionalFormatImpl1 != null & conditionalFormatImpl1.Priority != 0)
    {
      iPriority = conditionalFormatImpl1.Priority;
      Excel2007Serializator.SerializeAttribute(writer, "priority", iPriority, int.MinValue);
      ++iPriority;
    }
    else if (conditionalFormatImpl1 != null & conditionalFormatImpl1.Priority == 0 && this.m_book.Version != ExcelVersion.Excel97to2003)
    {
      Excel2007Serializator.SerializeAttribute(writer, "priority", iPriorityCount, int.MinValue);
      ++iPriorityCount;
    }
    if (conditionalFormatImpl1 != null && !string.IsNullOrEmpty(conditionalFormatImpl1.CFRuleID))
      Excel2007Serializator.SerializeAttribute(writer, "id", conditionalFormatImpl1.CFRuleID, string.Empty);
    if (formatType == ExcelCFType.TopBottom)
    {
      Excel2007Serializator.SerializeAttribute(writer, "bottom", condition.TopBottom.Type == ExcelCFTopBottomType.Bottom, false);
      Excel2007Serializator.SerializeAttribute(writer, "percent", condition.TopBottom.Percent, false);
      Excel2007Serializator.SerializeAttribute(writer, "rank", condition.TopBottom.Rank, int.MinValue);
    }
    if (formatType == ExcelCFType.AboveBelowAverage)
    {
      Excel2007Serializator.SerializeAttribute(writer, "aboveAverage", !condition.AboveBelowAverage.AverageType.ToString().Contains("Below"), true);
      Excel2007Serializator.SerializeAttribute(writer, "equalAverage", condition.AboveBelowAverage.AverageType.ToString().Contains("Equal"), false);
      if (condition.AboveBelowAverage.AverageType.ToString().Contains("StdDev"))
        Excel2007Serializator.SerializeAttribute(writer, "stdDev", condition.AboveBelowAverage.StdDevValue, int.MinValue);
    }
    ConditionalFormatImpl conditionalFormatImpl2 = (ConditionalFormatImpl) condition;
    bool isFormulaParsed = this.m_book.AppImplementation.IsFormulaParsed;
    if (condition.FirstFormula != null && condition.FirstFormula != string.Empty)
    {
      this.m_book.AppImplementation.IsFormulaParsed = false;
      string firstSecondFormula = conditionalFormatImpl2.GetFirstSecondFormula(this.m_formulaUtil, true, true);
      writer.WriteElementString("formula", firstSecondFormula);
      this.m_book.AppImplementation.IsFormulaParsed = isFormulaParsed;
    }
    if (condition.SecondFormula != null && condition.SecondFormula != string.Empty)
    {
      this.m_book.AppImplementation.IsFormulaParsed = false;
      string firstSecondFormula = conditionalFormatImpl2.GetFirstSecondFormula(this.m_formulaUtil, false, true);
      writer.WriteElementString("formula", firstSecondFormula);
      this.m_book.AppImplementation.IsFormulaParsed = isFormulaParsed;
    }
    switch (formatType)
    {
      case ExcelCFType.ColorScale:
        this.SerializeColorScale(writer, condition.ColorScale);
        break;
      case ExcelCFType.DataBar:
        this.SerializeDataBar(writer, condition.DataBar);
        break;
      case ExcelCFType.IconSet:
        if (condition1.CFHasExtensionList || iconSet.IsCustom)
        {
          this.SerializeIconSet(writer, condition.IconSet, condition1.CFHasExtensionList, iconSet.IsCustom);
          break;
        }
        this.SerializeIconSet(writer, condition.IconSet, false, false);
        break;
    }
    if (condition.DataBar != null && (condition.DataBar as DataBarImpl).HasExtensionList)
    {
      writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      writer.WriteStartElement("ext", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      writer.WriteAttributeString("uri", "{B025F937-C7B1-47D3-B67F-A62EFF666E3E}");
      writer.WriteAttributeString("xmlns", "x14", (string) null, "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
      writer.WriteElementString("x14", "id", (string) null, (condition.DataBar as DataBarImpl).ST_GUID);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeColorScale(XmlWriter writer, IColorScale colorScale)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (colorScale == null)
      throw new ArgumentNullException(nameof (colorScale));
    writer.WriteStartElement(nameof (colorScale));
    IList<IColorConditionValue> criteria = colorScale.Criteria;
    int index1 = 0;
    for (int count = criteria.Count; index1 < count; ++index1)
      this.SerializeConditionValueObject(writer, (IConditionValue) criteria[index1], false, false, false);
    int index2 = 0;
    for (int count = criteria.Count; index2 < count; ++index2)
      this.SerializeRgbColor(writer, "color", (ColorObject) criteria[index2].FormatColorRGB);
    writer.WriteEndElement();
  }

  private void SerializeIconSet(
    XmlWriter writer,
    IIconSet iconSet,
    bool cfHasExtensionList,
    bool isCustom)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (iconSet == null)
      throw new ArgumentNullException(nameof (iconSet));
    if (cfHasExtensionList || isCustom)
      writer.WriteStartElement("x14", nameof (iconSet), (string) null);
    else
      writer.WriteStartElement(nameof (iconSet));
    writer.WriteAttributeString(nameof (iconSet), CF.IconSetTypeNames[(int) iconSet.IconSet]);
    Excel2007Serializator.SerializeAttribute(writer, "percent", iconSet.PercentileValues, false);
    Excel2007Serializator.SerializeAttribute(writer, "reverse", iconSet.ReverseOrder, false);
    Excel2007Serializator.SerializeAttribute(writer, "showValue", !iconSet.ShowIconOnly, true);
    if (isCustom)
      Excel2007Serializator.SerializeAttribute(writer, "custom", true, false);
    IList<IConditionValue> iconCriteria = iconSet.IconCriteria;
    int index1 = 0;
    for (int count = iconCriteria.Count; index1 < count; ++index1)
      this.SerializeConditionValueObject(writer, iconCriteria[index1], true, cfHasExtensionList, isCustom);
    if (isCustom)
    {
      int index2 = 0;
      for (int count = iconCriteria.Count; index2 < count; ++index2)
        this.SerializeCustomCFIcon(writer, iconCriteria[index2] as IIconConditionValue, true);
    }
    writer.WriteEndElement();
  }

  private void SerializeDataBar(XmlWriter writer, IDataBar dataBar)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dataBar == null)
      throw new ArgumentNullException(nameof (dataBar));
    writer.WriteStartElement(nameof (dataBar));
    Excel2007Serializator.SerializeAttribute(writer, "minLength", dataBar.PercentMin, 0);
    Excel2007Serializator.SerializeAttribute(writer, "maxLength", dataBar.PercentMax, 100);
    Excel2007Serializator.SerializeAttribute(writer, "showValue", dataBar.ShowValue, true);
    this.SerializeConditionValueObject(writer, dataBar.MinPoint, false, true);
    this.SerializeConditionValueObject(writer, dataBar.MaxPoint, false, false);
    this.SerializeRgbColor(writer, "color", (ColorObject) dataBar.BarColor);
    writer.WriteEndElement();
  }

  public void SerializeConditionValueObject(
    XmlWriter writer,
    IConditionValue conditionValue,
    bool isIconSet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (conditionValue == null)
      throw new ArgumentNullException(nameof (conditionValue));
    this.SerializeConditionValueObject(writer, conditionValue, isIconSet, false, false);
  }

  internal void SerializeConditionValueObject(
    XmlWriter writer,
    IConditionValue conditionValue,
    bool isIconSet,
    bool cfHasExtensionList,
    bool isCustom)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (cfHasExtensionList || isCustom)
      writer.WriteStartElement("x14", "cfvo", (string) null);
    else
      writer.WriteStartElement("cfvo");
    int type = (int) conditionValue.Type;
    string valueType = CF.ValueTypes[type];
    ConditionValue conditionValue1 = conditionValue as ConditionValue;
    if (conditionValue1.RefPtg != null && conditionValue1.RefPtg.Length > 1)
      conditionValue.Value = this.m_formulaUtil.ParsePtgArray(conditionValue1.ref3Dptg);
    string str = conditionValue.Value;
    writer.WriteAttributeString("type", valueType);
    if (valueType == "formula" && str.StartsWith("="))
      str = str.Replace("=", "");
    if (!cfHasExtensionList && str != null)
      writer.WriteAttributeString("val", str.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, "."));
    else if (!cfHasExtensionList)
      writer.WriteAttributeString("val", str);
    writer.WriteAttributeString("gte", ((int) conditionValue.Operator).ToString());
    if (cfHasExtensionList || isCustom)
    {
      writer.WriteStartElement("xm", "f", (string) null);
      writer.WriteValue(str);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeCustomCFIcon(
    XmlWriter writer,
    IIconConditionValue conditionValue,
    bool isIconSet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (conditionValue == null)
      throw new ArgumentNullException(nameof (conditionValue));
    writer.WriteStartElement("x14", "cfIcon", (string) null);
    string str1 = !(conditionValue.IconSet.ToString() == "-1") ? CF.IconSetTypeNames[(int) conditionValue.IconSet].ToString() : "NoIcons";
    string str2 = conditionValue.Index.ToString();
    writer.WriteAttributeString("iconSet", str1);
    writer.WriteAttributeString("iconId", str2);
    writer.WriteEndElement();
  }

  internal string GetCFComparisonOperatorName(ExcelComparisonOperator comparisonOperator)
  {
    switch (comparisonOperator)
    {
      case ExcelComparisonOperator.None:
        return "notContains";
      case ExcelComparisonOperator.Between:
        return "between";
      case ExcelComparisonOperator.NotBetween:
        return "notBetween";
      case ExcelComparisonOperator.Equal:
        return "equal";
      case ExcelComparisonOperator.NotEqual:
        return "notEqual";
      case ExcelComparisonOperator.Greater:
        return "greaterThan";
      case ExcelComparisonOperator.Less:
        return "lessThan";
      case ExcelComparisonOperator.GreaterOrEqual:
        return "greaterThanOrEqual";
      case ExcelComparisonOperator.LessOrEqual:
        return "lessThanOrEqual";
      case ExcelComparisonOperator.BeginsWith:
        return "beginsWith";
      case ExcelComparisonOperator.ContainsText:
        return "containsText";
      case ExcelComparisonOperator.EndsWith:
        return "endsWith";
      case ExcelComparisonOperator.NotContainsText:
        return "notContains";
      default:
        throw new ArgumentOutOfRangeException("filterCondition");
    }
  }

  internal string GetCFTimePeriodType(CFTimePeriods timePeriod)
  {
    switch (timePeriod)
    {
      case CFTimePeriods.Today:
        return "today";
      case CFTimePeriods.Yesterday:
        return "yesterday";
      case CFTimePeriods.Tomorrow:
        return "tomorrow";
      case CFTimePeriods.Last7Days:
        return "last7Days";
      case CFTimePeriods.ThisMonth:
        return "thisMonth";
      case CFTimePeriods.LastMonth:
        return "lastMonth";
      case CFTimePeriods.NextMonth:
        return "nextMonth";
      case CFTimePeriods.ThisWeek:
        return "thisWeek";
      case CFTimePeriods.LastWeek:
        return "lastWeek";
      case CFTimePeriods.NextWeek:
        return "nextWeek";
      default:
        throw new ArgumentOutOfRangeException(nameof (timePeriod));
    }
  }

  internal string GetCFType(ExcelCFType typeCF, ExcelComparisonOperator compOperator)
  {
    switch (typeCF)
    {
      case ExcelCFType.CellValue:
        return "cellIs";
      case ExcelCFType.Formula:
        return "expression";
      case ExcelCFType.ColorScale:
        return "colorScale";
      case ExcelCFType.DataBar:
        return "dataBar";
      case ExcelCFType.IconSet:
        return "iconSet";
      case ExcelCFType.Blank:
        return "containsBlanks";
      case ExcelCFType.NoBlank:
        return "notContainsBlanks";
      case ExcelCFType.SpecificText:
        switch (compOperator)
        {
          case ExcelComparisonOperator.BeginsWith:
            return "beginsWith";
          case ExcelComparisonOperator.ContainsText:
            return "containsText";
          case ExcelComparisonOperator.EndsWith:
            return "endsWith";
          case ExcelComparisonOperator.NotContainsText:
            return "notContainsText";
          default:
            throw new ArgumentException("ComOperator");
        }
      case ExcelCFType.ContainsErrors:
        return "containsErrors";
      case ExcelCFType.NotContainsErrors:
        return "notContainsErrors";
      case ExcelCFType.TimePeriod:
        return "timePeriod";
      case ExcelCFType.Duplicate:
        return "duplicateValues";
      case ExcelCFType.Unique:
        return "uniqueValues";
      case ExcelCFType.TopBottom:
        return "top10";
      case ExcelCFType.AboveBelowAverage:
        return "aboveAverage";
      default:
        throw new ArgumentOutOfRangeException(nameof (typeCF));
    }
  }

  public Stream SerializeDxfs(
    ref Stream streamDxfs,
    WorksheetConditionalFormats conditionalFormats,
    ref int iDxfIndex)
  {
    int count = conditionalFormats.Count;
    if (conditionalFormats == null)
      throw new ArgumentNullException(nameof (conditionalFormats));
    if (count == 0)
      return (Stream) null;
    int bookCfPriorityCount = this.m_book.BookCFPriorityCount;
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer1 = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) memoryStream));
    Stream stream1 = (Stream) new MemoryStream();
    XmlWriter writer2 = UtilityMethods.CreateWriter((TextWriter) new StreamWriter(stream1));
    writer1.WriteStartElement("root", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    string prefix = string.Empty;
    if (streamDxfs != null && streamDxfs.Length > 0L)
    {
      Stream stream2 = (Stream) new MemoryStream();
      streamDxfs.Position = 0L;
      UtilityMethods.CopyStreamTo(streamDxfs, stream2);
      XmlReader reader = UtilityMethods.CreateReader(stream2);
      prefix = reader.Prefix;
      reader.Close();
    }
    writer1.WriteStartElement(prefix, "dxfs", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    string str = string.IsNullOrEmpty(prefix) ? prefix : prefix + ":";
    Excel2007Serializator.SerializeStream(writer1, streamDxfs, str + "dxfs");
    writer2.WriteStartElement("root", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    int iPriority = 1;
    int iPriorityCount = 0;
    if (bookCfPriorityCount != int.MinValue && iDxfIndex < bookCfPriorityCount)
    {
      ++this.m_book.BookCFPriorityCount;
      iDxfIndex = this.m_book.BookCFPriorityCount;
      iPriority = this.m_book.BookCFPriorityCount;
    }
    for (int i = 0; i < count; ++i)
    {
      ConditionalFormats conditionalFormat = conditionalFormats[i];
      if (this.hasTextRotation && i == 0)
        iDxfIndex = (conditionalFormat[i] as ConditionalFormatImpl).StartDxf;
      this.SerializeCondionalFormats(writer2, writer1, conditionalFormat, ref iDxfIndex, ref iPriority, ref iPriorityCount);
    }
    writer2.WriteEndElement();
    writer2.Flush();
    writer1.WriteEndElement();
    writer1.WriteEndElement();
    writer1.Flush();
    this.m_book.BookCFPriorityCount = iDxfIndex;
    streamDxfs = (Stream) memoryStream;
    return stream1;
  }

  internal Stream SerializeDxfsTableStyles(
    ref Stream streamDxfs,
    ITableStyles tableStyles,
    ref int iDxfIndex)
  {
    int count = tableStyles.Count;
    if (tableStyles == null)
      return (Stream) null;
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer1 = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) memoryStream));
    Stream stream = (Stream) new MemoryStream();
    XmlWriter writer2 = UtilityMethods.CreateWriter((TextWriter) new StreamWriter(stream));
    writer1.WriteStartElement("root", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer1.WriteStartElement("dxfs");
    Excel2007Serializator.SerializeStream(writer1, streamDxfs, "dxfs");
    writer2.WriteStartElement("root", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer2.WriteStartElement(nameof (tableStyles));
    this.SerializeTableStylesAttributes(writer2, count, tableStyles as TableStyles);
    if (count != 0)
    {
      foreach (ITableStyle tableStyle in (IEnumerable) tableStyles)
        this.SerializeTableStyle(writer2, tableStyle, writer1, ref iDxfIndex);
    }
    writer1.WriteEndElement();
    writer1.WriteEndElement();
    writer1.Flush();
    writer2.WriteEndElement();
    writer2.WriteEndElement();
    writer2.Flush();
    streamDxfs = (Stream) memoryStream;
    return stream;
  }

  internal Stream SerializeDxfsPivotCellFormats(
    ref Stream streamDxfs,
    PivotTableImpl pivotTable,
    ref int iDxfIndex)
  {
    if (pivotTable.PivotFormats == null || pivotTable.PivotFormats.Formats.Count == 0)
      return (Stream) null;
    PivotFormats pivotFormats = pivotTable.PivotFormats;
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer1 = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) memoryStream));
    Stream stream = (Stream) new MemoryStream();
    XmlWriter writer2 = UtilityMethods.CreateWriter((TextWriter) new StreamWriter(stream));
    writer1.WriteStartElement("root", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer1.WriteStartElement("dxfs");
    Excel2007Serializator.SerializeStream(writer1, streamDxfs, "dxfs");
    writer2.WriteStartElement("root", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer2.WriteStartElement("formats");
    writer2.WriteAttributeString("count", pivotFormats.Count.ToString());
    for (int index = 0; index < pivotFormats.Count; ++index)
      this.SerializeCustomFormat(writer2, pivotFormats[index], writer1, ref iDxfIndex);
    writer1.WriteEndElement();
    writer1.WriteEndElement();
    writer1.Flush();
    writer2.WriteEndElement();
    writer2.WriteEndElement();
    writer2.Flush();
    streamDxfs = (Stream) memoryStream;
    return stream;
  }

  private void SerializeCustomFormat(
    XmlWriter writer,
    PivotFormat pivotformat,
    XmlWriter WriterDxfs,
    ref int iDxfIndex)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotformat == null)
      throw new ArgumentNullException(nameof (pivotformat));
    writer.WriteStartElement("format");
    IInternalPivotCellFormat pivotCellFormat = pivotformat.PivotCellFormat as IInternalPivotCellFormat;
    if (pivotCellFormat.IsBackgroundColorPresent || pivotCellFormat.IsFontFormatPresent || pivotCellFormat.IsBorderFormatPresent || pivotCellFormat.IsPatternFormatPresent || pivotCellFormat.IncludeAlignment || pivotCellFormat.IsNumberFormatPresent || pivotCellFormat.IncludeProtection)
    {
      this.SerializeDxf(WriterDxfs, pivotCellFormat);
      Excel2007Serializator.SerializeAttribute(writer, "dxfId", iDxfIndex, int.MinValue);
      ++iDxfIndex;
    }
    PivotCacheSerializator.SerializePivotArea(writer, pivotformat.PivotArea, false);
    writer.WriteEndElement();
  }

  private Stream SerializeDxfsColorFilterAndSorting(
    ref Stream streamDxfs,
    IDataSort sortState,
    AutoFilterImpl autoFilter,
    ref int iDxfIndex)
  {
    if ((sortState == null || sortState.SortFields.Count == 0) && autoFilter == null)
      return (Stream) null;
    int bookCfPriorityCount = this.m_book.BookCFPriorityCount;
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer1 = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) memoryStream));
    Stream stream = (Stream) new MemoryStream();
    XmlWriter writer2 = UtilityMethods.CreateWriter((TextWriter) new StreamWriter(stream));
    writer1.WriteStartElement("root", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer1.WriteStartElement("dxfs");
    Excel2007Serializator.SerializeStream(writer1, streamDxfs, "dxfs");
    writer2.WriteStartElement("root", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    if (bookCfPriorityCount != int.MinValue && iDxfIndex < bookCfPriorityCount)
    {
      ++this.m_book.BookCFPriorityCount;
      iDxfIndex = this.m_book.BookCFPriorityCount;
    }
    if (sortState != null)
      this.SerializeSortData(writer2, writer1, sortState, ref iDxfIndex);
    else
      this.SerializeFilterColorStream(writer2, writer1, autoFilter.FilteredItems as ColorFilter, ref iDxfIndex);
    writer2.WriteEndElement();
    writer2.Flush();
    writer1.WriteEndElement();
    writer1.WriteEndElement();
    writer1.Flush();
    this.m_book.BookCFPriorityCount = iDxfIndex;
    streamDxfs = (Stream) memoryStream;
    return stream;
  }

  private void SerializeDxf(XmlWriter writer, ConditionalFormatImpl condition)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (condition == null)
      throw new ArgumentNullException(nameof (condition));
    writer.WriteStartElement("dxf");
    this.SerializeDxfFont(writer, (IInternalConditionalFormat) condition);
    this.SerializeDxfNumberFormat(writer, condition);
    this.SerializeDxfFill(writer, (IInternalConditionalFormat) condition);
    this.SerializeDxfBorders(writer, (IInternalConditionalFormat) condition);
    writer.WriteEndElement();
  }

  private void SerializeDxf(XmlWriter writer, TableStyleElement tableStyleElement)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tableStyleElement == null)
      throw new ArgumentNullException("tablestyleElement");
    writer.WriteStartElement("dxf");
    this.SerializeDxfFont(writer, tableStyleElement);
    this.SerializeDxfFill(writer, tableStyleElement);
    this.SerializeDxfBorders(writer, tableStyleElement);
    writer.WriteEndElement();
  }

  private void SerializeDxf(XmlWriter writer, ISortField sortField)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sortField == null)
      throw new ArgumentNullException("condition");
    writer.WriteStartElement("dxf");
    if (sortField.SortOn == SortOn.FontColor)
      this.SerializeDxfFont(writer, sortField);
    else if (sortField.SortOn == SortOn.CellColor)
      this.SerializeDxfFill(writer, sortField.Color);
    writer.WriteEndElement();
  }

  private void SerializeDxf(XmlWriter writer, IInternalPivotCellFormat pivotCellFormat)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotCellFormat == null)
      throw new ArgumentNullException(nameof (pivotCellFormat));
    writer.WriteStartElement("dxf");
    this.SerializeDxfFont(writer, pivotCellFormat);
    this.SerializeDxfNumberFormat(writer, pivotCellFormat);
    this.SerializeDxfFill(writer, pivotCellFormat);
    this.SerializeDxfAlignment(writer, pivotCellFormat);
    this.SerializeDxfBorders(writer, pivotCellFormat);
    this.SerializeDxfProtection(writer, pivotCellFormat);
    writer.WriteEndElement();
  }

  private void SerializeDxfNumberFormat(XmlWriter writer, ConditionalFormatImpl condition)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (condition == null)
      throw new ArgumentNullException(nameof (condition));
    if (!condition.HasNumberFormatPresent)
      return;
    FormatImpl formatImpl = (FormatImpl) null;
    FormatRecord format = (FormatRecord) null;
    if (this.m_book.InnerFormats.ContainsFormat(condition.NumberFormat))
      formatImpl = this.m_book.InnerFormats[(int) condition.NumberFormatIndex];
    else if (this.m_book.InnerFormats.GetUsedFormats(ExcelVersion.Excel2007).Count > 0)
    {
      List<FormatRecord> usedFormats = this.m_book.InnerFormats.GetUsedFormats(ExcelVersion.Excel2007);
      for (int index = 0; index < usedFormats.Count; ++index)
      {
        if (usedFormats[index].FormatString == condition.NumberFormat)
        {
          format = usedFormats[index];
          break;
        }
      }
    }
    if (formatImpl == null && format == null)
      return;
    if (formatImpl != null)
      this.SerializeNumberFormat(writer, formatImpl.Record);
    else
      this.SerializeNumberFormat(writer, format);
  }

  private void SerializeDxfNumberFormat(XmlWriter writer, IInternalPivotCellFormat pivotCellFormat)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotCellFormat == null)
      throw new ArgumentNullException("condition");
    if (!pivotCellFormat.IsNumberFormatPresent)
      return;
    FormatImpl formatImpl = (FormatImpl) null;
    FormatRecord format = (FormatRecord) null;
    if (this.m_book.InnerFormats.ContainsFormat(pivotCellFormat.NumberFormat))
      formatImpl = this.m_book.InnerFormats[(int) pivotCellFormat.NumberFormatIndex];
    else if (this.m_book.InnerFormats.GetUsedFormats(ExcelVersion.Excel2007).Count > 0)
    {
      List<FormatRecord> usedFormats = this.m_book.InnerFormats.GetUsedFormats(ExcelVersion.Excel2007);
      for (int index = 0; index < usedFormats.Count; ++index)
      {
        if (usedFormats[index].FormatString == pivotCellFormat.NumberFormat)
        {
          format = usedFormats[index];
          break;
        }
      }
    }
    if (formatImpl == null && format == null)
      return;
    if (formatImpl != null)
      this.SerializeNumberFormat(writer, formatImpl.Record);
    else
      this.SerializeNumberFormat(writer, format);
  }

  internal void SerializeDxfBorders(XmlWriter writer, IInternalConditionalFormat condition)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (condition == null)
      throw new ArgumentNullException(nameof (condition));
    if (!condition.IsBorderFormatPresent)
      return;
    writer.WriteStartElement("border");
    this.SerializeDxfBorder(writer, Excel2007BorderIndex.left, condition.LeftBorderStyle, condition.LeftBorderColorObject, condition.IsLeftBorderModified);
    this.SerializeDxfBorder(writer, Excel2007BorderIndex.right, condition.RightBorderStyle, condition.RightBorderColorObject, condition.IsRightBorderModified);
    this.SerializeDxfBorder(writer, Excel2007BorderIndex.top, condition.TopBorderStyle, condition.TopBorderColorObject, condition.IsTopBorderModified);
    this.SerializeDxfBorder(writer, Excel2007BorderIndex.bottom, condition.BottomBorderStyle, condition.BottomBorderColorObject, condition.IsBottomBorderModified);
    writer.WriteEndElement();
  }

  internal void SerializeDxfBorders(XmlWriter writer, TableStyleElement tableStyleElement)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tableStyleElement == null)
      throw new ArgumentNullException(nameof (tableStyleElement));
    if (!tableStyleElement.IsBorderFormatPresent)
      return;
    writer.WriteStartElement("border");
    this.SerializeDxfBorder(writer, Excel2007BorderIndex.left, tableStyleElement.LeftBorderStyle, tableStyleElement.LeftBorderColorObject, tableStyleElement.IsLeftBorderModified);
    this.SerializeDxfBorder(writer, Excel2007BorderIndex.right, tableStyleElement.RightBorderStyle, tableStyleElement.RightBorderColorObject, tableStyleElement.IsRightBorderModified);
    this.SerializeDxfBorder(writer, Excel2007BorderIndex.top, tableStyleElement.TopBorderStyle, tableStyleElement.TopBorderColorObject, tableStyleElement.IsTopBorderModified);
    this.SerializeDxfBorder(writer, Excel2007BorderIndex.bottom, tableStyleElement.BottomBorderStyle, tableStyleElement.BottomBorderColorObject, tableStyleElement.IsBottomBorderModified);
    this.SerializeDxfBorder(writer, Excel2007BorderIndex.vertical, tableStyleElement.VerticalBorderStyle, tableStyleElement.VerticalBorderColorObject, tableStyleElement.IsVerticalBorderModified);
    this.SerializeDxfBorder(writer, Excel2007BorderIndex.horizontal, tableStyleElement.HorizontalBorderStyle, tableStyleElement.HorizontalBorderColorObject, tableStyleElement.IsHorizontalBorderModified);
    writer.WriteEndElement();
  }

  internal void SerializeDxfBorders(XmlWriter writer, IInternalPivotCellFormat pivotCellFormat)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotCellFormat == null)
      throw new ArgumentNullException("tableStyleElement");
    if (!pivotCellFormat.IsBorderFormatPresent)
      return;
    writer.WriteStartElement("border");
    if (pivotCellFormat.IsDiagonalBorderModified && pivotCellFormat.DiagonalBorderStyle != ExcelLineStyle.None)
    {
      writer.WriteAttributeString("diagonalUp", "1");
      writer.WriteAttributeString("diagonalDown", "1");
    }
    this.SerializeDxfBorder(writer, Excel2007BorderIndex.left, pivotCellFormat.LeftBorderStyle, pivotCellFormat.LeftBorderColorObject, pivotCellFormat.IsLeftBorderModified);
    this.SerializeDxfBorder(writer, Excel2007BorderIndex.right, pivotCellFormat.RightBorderStyle, pivotCellFormat.RightBorderColorObject, pivotCellFormat.IsRightBorderModified);
    this.SerializeDxfBorder(writer, Excel2007BorderIndex.top, pivotCellFormat.TopBorderStyle, pivotCellFormat.TopBorderColorObject, pivotCellFormat.IsTopBorderModified);
    this.SerializeDxfBorder(writer, Excel2007BorderIndex.bottom, pivotCellFormat.BottomBorderStyle, pivotCellFormat.BottomBorderColorObject, pivotCellFormat.IsBottomBorderModified);
    this.SerializeDxfBorder(writer, Excel2007BorderIndex.vertical, pivotCellFormat.VerticalBorderStyle, pivotCellFormat.VerticalBorderColorObject, pivotCellFormat.IsVerticalBorderModified);
    this.SerializeDxfBorder(writer, Excel2007BorderIndex.horizontal, pivotCellFormat.HorizontalBorderStyle, pivotCellFormat.HorizontalBorderColorObject, pivotCellFormat.IsHorizontalBorderModified);
    this.SerializeDxfBorder(writer, Excel2007BorderIndex.diagonal, pivotCellFormat.DiagonalBorderStyle, pivotCellFormat.DiagonalBorderColorObject, pivotCellFormat.IsDiagonalBorderModified);
    writer.WriteEndElement();
  }

  private void SerializeDxfBorder(
    XmlWriter writer,
    Excel2007BorderIndex borderIndex,
    ExcelLineStyle lineStyle,
    ColorObject color,
    bool isBorderModified)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (!isBorderModified)
      return;
    writer.WriteStartElement(borderIndex.ToString());
    if (lineStyle != ExcelLineStyle.None)
    {
      string str = Excel2007Serializator.LowerFirstLetter(((Excel2007BorderLineStyle) lineStyle).ToString());
      writer.WriteAttributeString("style", str);
      this.SerializeColorObject(writer, nameof (color), color);
    }
    writer.WriteEndElement();
  }

  internal void SerializeDxfFill(XmlWriter writer, IInternalConditionalFormat condition)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (condition == null)
      throw new ArgumentNullException(nameof (condition));
    if (!condition.IsPatternFormatPresent)
      return;
    writer.WriteStartElement("fill");
    writer.WriteStartElement("patternFill");
    if (condition.FillPattern != ExcelPattern.None || condition is ConditionalFormatImpl && (condition as ConditionalFormatImpl).IsDxfPatternNone)
      writer.WriteAttributeString("patternType", this.ConvertPatternToString(condition.FillPattern));
    ColorObject colorObject = condition.ColorObject;
    ColorObject backColorObject = condition.BackColorObject;
    if (colorObject.ColorType != ColorType.Indexed || colorObject.GetIndexed((IWorkbook) this.m_book) != ExcelKnownColors.None)
      this.SerializeColorObject(writer, "fgColor", colorObject);
    if (backColorObject.ColorType != ColorType.Indexed || backColorObject.GetIndexed((IWorkbook) this.m_book) != ExcelKnownColors.BlackCustom)
      this.SerializeColorObject(writer, "bgColor", backColorObject);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  internal void SerializeDxfFill(XmlWriter writer, TableStyleElement tableStyleElement)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tableStyleElement == null)
      throw new ArgumentNullException(nameof (tableStyleElement));
    if (!tableStyleElement.IsPatternFormatPresent)
      return;
    writer.WriteStartElement("fill");
    writer.WriteStartElement("patternFill");
    if (tableStyleElement.PatternStyle != ExcelPattern.None)
      writer.WriteAttributeString("patternType", this.ConvertPatternToString(tableStyleElement.PatternStyle));
    ColorObject colorObject = tableStyleElement.ColorObject;
    ColorObject backColorObject = tableStyleElement.BackColorObject;
    if (colorObject.ColorType != ColorType.Indexed || colorObject.Value != 65)
      this.SerializeColorObject(writer, "fgColor", colorObject);
    this.SerializeColorObject(writer, "bgColor", backColorObject);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  internal void SerializeDxfFill(XmlWriter writer, Color Color)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("fill");
    writer.WriteStartElement("patternFill");
    writer.WriteAttributeString("patternType", "solid");
    this.SerializeColorObject(writer, "fgColor", (ColorObject) Color);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  internal void SerializeDxfFill(XmlWriter writer, IInternalPivotCellFormat pivotCellFormat)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotCellFormat == null)
      throw new ArgumentNullException("tableStyleElement");
    if (!pivotCellFormat.IsPatternFormatPresent)
      return;
    writer.WriteStartElement("fill");
    writer.WriteStartElement("patternFill");
    if (pivotCellFormat.PatternStyle != ExcelPattern.None)
      writer.WriteAttributeString("patternType", this.ConvertPatternToString(pivotCellFormat.PatternStyle));
    ColorObject colorObject = pivotCellFormat.ColorObject;
    ColorObject backColorObject = pivotCellFormat.BackColorObject;
    if (colorObject.ColorType != ColorType.Indexed || colorObject.Value != 65)
      this.SerializeColorObject(writer, "fgColor", colorObject);
    this.SerializeColorObject(writer, "bgColor", backColorObject);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  internal void SerializeDxfFont(XmlWriter writer, IInternalConditionalFormat condition)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (condition == null)
      throw new ArgumentNullException(nameof (condition));
    if (!condition.IsFontFormatPresent)
      return;
    writer.WriteStartElement("font");
    if (condition.IsBold)
      writer.WriteElementString("b", string.Empty);
    if (condition.IsItalic)
      writer.WriteElementString("i", string.Empty);
    ExcelUnderline excelUnderline = condition.Underline;
    if (excelUnderline != ExcelUnderline.None)
    {
      writer.WriteStartElement("u");
      if (!Enum.IsDefined(typeof (ExcelUnderline), (object) excelUnderline))
        excelUnderline = ExcelUnderline.Single;
      string str = Excel2007Serializator.LowerFirstLetter(excelUnderline.ToString());
      writer.WriteAttributeString("val", str);
      writer.WriteEndElement();
    }
    if (condition.IsStrikeThrough)
      writer.WriteElementString("strike", string.Empty);
    if (condition.FontColor != ~ExcelKnownColors.Black && condition.IsFontColorPresent)
      this.SerializeColorObject(writer, "color", condition.FontColorObject);
    writer.WriteEndElement();
  }

  internal void SerializeDxfFont(XmlWriter writer, TableStyleElement tableStyleElement)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tableStyleElement == null)
      throw new ArgumentNullException("condition");
    if (!tableStyleElement.IsFontFormatPresent)
      return;
    writer.WriteStartElement("font");
    if (tableStyleElement.Bold)
      writer.WriteElementString("b", string.Empty);
    if (tableStyleElement.Italic)
      writer.WriteElementString("i", string.Empty);
    ExcelUnderline excelUnderline = tableStyleElement.Underline;
    if (excelUnderline != ExcelUnderline.None)
    {
      writer.WriteStartElement("u");
      if (!Enum.IsDefined(typeof (ExcelUnderline), (object) excelUnderline))
        excelUnderline = ExcelUnderline.Single;
      string str = Excel2007Serializator.LowerFirstLetter(excelUnderline.ToString());
      writer.WriteAttributeString("val", str);
      writer.WriteEndElement();
    }
    if (tableStyleElement.StrikeThrough)
      writer.WriteElementString("strike", string.Empty);
    if (tableStyleElement.IsFontColorPresent)
      this.SerializeColorObject(writer, "color", tableStyleElement.FontColorObject);
    writer.WriteEndElement();
  }

  internal void SerializeDxfFont(XmlWriter writer, ISortField sortField)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sortField == null)
      throw new ArgumentNullException(nameof (sortField));
    if (sortField.Color == ColorExtension.Empty)
      return;
    writer.WriteStartElement("font");
    this.SerializeColorObject(writer, "color", (ColorObject) sortField.Color);
    writer.WriteEndElement();
  }

  private void SerializeDxfAlignment(XmlWriter writer, IInternalPivotCellFormat pivotCellFormat)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotCellFormat == null)
      throw new ArgumentNullException(nameof (pivotCellFormat));
    if (this.IsDefaultAlignment((IPivotCellFormat) pivotCellFormat))
      return;
    writer.WriteStartElement("alignment");
    if (pivotCellFormat.HorizontalAlignment != ExcelHAlign.HAlignGeneral)
    {
      string str = ((Excel2007HAlign) pivotCellFormat.HorizontalAlignment).ToString();
      writer.WriteAttributeString("horizontal", str);
    }
    if (pivotCellFormat.VerticalAlignment != ExcelVAlign.VAlignBottom)
    {
      string str = ((Excel2007VAlign) pivotCellFormat.VerticalAlignment).ToString();
      writer.WriteAttributeString("vertical", str);
    }
    Excel2007Serializator.SerializeAttribute(writer, "textRotation", pivotCellFormat.Rotation, 0);
    Excel2007Serializator.SerializeAttribute(writer, "wrapText", pivotCellFormat.WrapText, false);
    Excel2007Serializator.SerializeAttribute(writer, "indent", pivotCellFormat.IndentLevel, 0);
    Excel2007Serializator.SerializeAttribute(writer, "shrinkToFit", pivotCellFormat.ShrinkToFit, false);
    Excel2007Serializator.SerializeAttribute(writer, "readingOrder", (int) pivotCellFormat.ReadingOrder, 0);
    writer.WriteEndElement();
  }

  private void SerializeDxfProtection(XmlWriter writer, IInternalPivotCellFormat pivotCellFormat)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotCellFormat == null)
      throw new ArgumentNullException(nameof (pivotCellFormat));
    if (!pivotCellFormat.FormulaHidden && pivotCellFormat.Locked)
      return;
    writer.WriteStartElement("protection");
    Excel2007Serializator.SerializeAttribute(writer, "hidden", pivotCellFormat.FormulaHidden, false);
    Excel2007Serializator.SerializeAttribute(writer, "locked", pivotCellFormat.Locked, true);
    writer.WriteEndElement();
  }

  internal void SerializeDxfFont(XmlWriter writer, IInternalPivotCellFormat pivotCellFormat)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotCellFormat == null)
      throw new ArgumentNullException("condition");
    if (!pivotCellFormat.IsFontFormatPresent)
      return;
    writer.WriteStartElement("font");
    if (pivotCellFormat.FontSize != 0.0)
    {
      writer.WriteStartElement("sz");
      writer.WriteAttributeString("val", pivotCellFormat.FontSize.ToString());
      writer.WriteEndElement();
    }
    if (pivotCellFormat.Bold)
      writer.WriteElementString("b", string.Empty);
    if (pivotCellFormat.Italic)
      writer.WriteElementString("i", string.Empty);
    ExcelUnderline excelUnderline = pivotCellFormat.Underline;
    if (excelUnderline != ExcelUnderline.None)
    {
      writer.WriteStartElement("u");
      if (!Enum.IsDefined(typeof (ExcelUnderline), (object) excelUnderline))
        excelUnderline = ExcelUnderline.Single;
      string str = Excel2007Serializator.LowerFirstLetter(excelUnderline.ToString());
      writer.WriteAttributeString("val", str);
      writer.WriteEndElement();
    }
    if (pivotCellFormat.StrikeThrough)
      writer.WriteElementString("strike", string.Empty);
    if (pivotCellFormat.IsFontColorPresent)
      this.SerializeColorObject(writer, "color", pivotCellFormat.FontColorObject);
    writer.WriteEndElement();
  }

  internal void SerializeTableStylesAttributes(
    XmlWriter writer,
    int count,
    TableStyles tableStyles)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    string str = count.ToString();
    writer.WriteAttributeString(nameof (count), str);
    if (!string.IsNullOrEmpty(tableStyles.DefaultTablesStyle))
      writer.WriteAttributeString("defaultTableStyle", tableStyles.DefaultTablesStyle);
    if (string.IsNullOrEmpty(tableStyles.DefaultPivotTableStyle))
      return;
    writer.WriteAttributeString("defaultPivotStyle", tableStyles.DefaultPivotTableStyle);
  }

  internal void SerializeTableStyle(
    XmlWriter writer,
    ITableStyle tableStyle,
    XmlWriter WriterDxfs,
    ref int iDxfIndex)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tableStyle == null)
      throw new ArgumentNullException(nameof (tableStyle));
    int count = tableStyle.TableStyleElements.Count;
    string name = tableStyle.Name;
    writer.WriteStartElement(nameof (tableStyle));
    this.SerializeTableStyleAttributes(writer, count, name);
    if (tableStyle.TableStyleElements.Count != 0)
    {
      foreach (ITableStyleElement tableStyleElement in (IEnumerable) tableStyle.TableStyleElements)
        this.SerializeTableStyleElement(writer, tableStyleElement as TableStyleElement, WriterDxfs, ref iDxfIndex);
    }
    writer.WriteEndElement();
  }

  internal void SerializeTableStyleAttributes(XmlWriter writer, int Count, string tableStyleName)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    string str = Count.ToString();
    writer.WriteAttributeString("name", tableStyleName);
    writer.WriteAttributeString("pivot", 0.ToString());
    writer.WriteAttributeString("count", str);
  }

  internal void SerializeTableStyleElement(
    XmlWriter writer,
    TableStyleElement tableStyleElement,
    XmlWriter WriterDxfs,
    ref int iDxfIndex)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    string tableStyleElementType = tableStyleElement != null ? Excel2007Serializator.LowerFirstLetter(tableStyleElement.TableStyleElementType.ToString()) : throw new ArgumentNullException("table style element");
    writer.WriteStartElement(nameof (tableStyleElement));
    if (tableStyleElement.TableStyleElementName == null)
      this.SerializeTableStyleElementAttributes(writer, tableStyleElementType);
    else
      this.SerializeTableStyleElementAttributes(writer, tableStyleElement.TableStyleElementName);
    if (tableStyleElement.StripeSize != 1)
      this.SerializeTableStyleElementAttributes(writer, tableStyleElement.StripeSize);
    if (tableStyleElement.IsBackgroundColorPresent || tableStyleElement.IsFontFormatPresent || tableStyleElement.IsBorderFormatPresent || tableStyleElement.IsPatternFormatPresent || tableStyleElement.TableStyleElementName != null)
    {
      this.SerializeDxf(WriterDxfs, tableStyleElement);
      this.SerializableTableStyleElementAttribute(writer, ref iDxfIndex);
      ++iDxfIndex;
    }
    writer.WriteEndElement();
  }

  internal void SerializeTableStyleElementAttributes(XmlWriter writer, string tableStyleElementType)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteAttributeString("type", tableStyleElementType);
  }

  internal void SerializeTableStyleElementAttributes(XmlWriter writer, int stripeSize)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteAttributeString("size", stripeSize.ToString());
  }

  internal void SerializableTableStyleElementAttribute(XmlWriter writer, ref int iDxfIndex)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    string str = iDxfIndex.ToString();
    writer.WriteAttributeString("dxfId", str);
  }

  private void SerializeHyperlinks(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    HyperLinksCollection hyperLinksCollection = sheet != null ? sheet.InnerHyperLinksOrNull : throw new ArgumentNullException(nameof (sheet));
    if (hyperLinksCollection == null)
      return;
    RelationCollection relations = sheet.DataHolder.Relations;
    int count = hyperLinksCollection.Count;
    string[] array = new string[count];
    for (int index = 0; index < count; ++index)
    {
      HyperLinkImpl hyperLinkImpl = (HyperLinkImpl) hyperLinksCollection[index];
      array[index] = hyperLinkImpl.AttachedType.ToString();
    }
    if (count == 0 || Array.IndexOf<string>(array, "Range") == -1)
      return;
    writer.WriteStartElement("hyperlinks");
    for (int index = 0; index < count; ++index)
    {
      HyperLinkImpl hyperlink = (HyperLinkImpl) hyperLinksCollection[index];
      if (hyperlink.Range != null)
        this.SerializeHyperlink(writer, hyperlink, relations);
    }
    writer.WriteEndElement();
  }

  private void SerializeHyperlink(
    XmlWriter writer,
    HyperLinkImpl hyperlink,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (hyperlink == null)
      throw new ArgumentNullException(nameof (hyperlink));
    writer.WriteStartElement(nameof (hyperlink));
    Excel2007Serializator.SerializeAttribute(writer, "ref", hyperlink.Range.AddressLocal, string.Empty);
    if (hyperlink.Type == ExcelHyperLinkType.Workbook)
    {
      string empty = string.Empty;
      string str = !hyperlink.Address.EndsWith("\0") ? hyperlink.Address : hyperlink.Address.Replace("\0", string.Empty);
      Excel2007Serializator.SerializeAttribute(writer, "location", str.Trim(), string.Empty);
    }
    else
    {
      string hyperlinkRelationId = relations.GenerateHyperlinkRelationId();
      string str = hyperlink.Address;
      if (hyperlink.Type == ExcelHyperLinkType.File && !str.StartsWith("..") && str.Contains(":\\") && !str.StartsWith("file:///") || hyperlink.Type == ExcelHyperLinkType.Unc)
        str = "file:///" + str;
      if (hyperlink.Type != ExcelHyperLinkType.Workbook)
        str = this.ConvertAddressString(str);
      if (str != null)
      {
        Relation relation = new Relation(str, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink", true);
        relations.Add(relation, hyperlinkRelationId);
        writer.WriteAttributeString("r", "id", (string) null, hyperlinkRelationId);
        Excel2007Serializator.SerializeAttribute(writer, "location", hyperlink.SubAddress, string.Empty);
      }
    }
    Excel2007Serializator.SerializeAttribute(writer, "tooltip", hyperlink.ScreenTip, (string) null);
    Excel2007Serializator.SerializeAttribute(writer, "display", hyperlink.TextToDisplay, string.Empty);
    writer.WriteEndElement();
  }

  public static void SerializePrintSettings(
    XmlWriter writer,
    IPageSetupBase pageSetup,
    IPageSetupConstantsProvider constants,
    bool isChartSettings)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pageSetup == null)
      throw new ArgumentNullException(nameof (pageSetup));
    if (isChartSettings)
    {
      Excel2007Serializator.SerializePrintOptions(writer, pageSetup, constants);
      Excel2007Serializator.SerializeHeaderFooter(writer, pageSetup, constants);
      Excel2007Serializator.SerializePageMargins(writer, pageSetup, constants);
      Excel2007Serializator.SerializePageSetup(writer, pageSetup, constants);
    }
    else
    {
      Excel2007Serializator.SerializePrintOptions(writer, pageSetup, constants);
      Excel2007Serializator.SerializePageMargins(writer, pageSetup, constants);
      Excel2007Serializator.SerializePageSetup(writer, pageSetup, constants);
      Excel2007Serializator.SerializeHeaderFooter(writer, pageSetup, constants);
    }
  }

  private static void SerializePrintOptions(
    XmlWriter writer,
    IPageSetupBase pageSetup,
    IPageSetupConstantsProvider constants)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pageSetup == null)
      throw new ArgumentNullException(nameof (pageSetup));
    bool flag1 = pageSetup is IPageSetup pageSetup1 && pageSetup1.PrintGridlines;
    bool flag2 = pageSetup1 != null && pageSetup1.PrintHeadings;
    if (!flag1 && !flag2 && !pageSetup.CenterHorizontally && !pageSetup.CenterVertically)
      return;
    writer.WriteStartElement("printOptions", constants.Namespace);
    Excel2007Serializator.SerializeAttribute(writer, "gridLines", flag1, false);
    Excel2007Serializator.SerializeAttribute(writer, "headings", flag2, false);
    Excel2007Serializator.SerializeAttribute(writer, "horizontalCentered", pageSetup.CenterHorizontally, false);
    Excel2007Serializator.SerializeAttribute(writer, "verticalCentered", pageSetup.CenterVertically, false);
    writer.WriteEndElement();
  }

  public static void SerializePageMargins(
    XmlWriter writer,
    IPageSetupBase pageSetup,
    IPageSetupConstantsProvider constants)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pageSetup == null)
      throw new ArgumentNullException(nameof (pageSetup));
    if (constants == null)
      throw new ArgumentNullException(nameof (constants));
    Excel2007Serializator.ValidatePageMargins(pageSetup as PageSetupBaseImpl);
    writer.WriteStartElement(constants.PageMarginsTag, constants.Namespace);
    Excel2007Serializator.SerializeAttribute(writer, constants.LeftMargin, pageSetup.LeftMargin, double.MinValue);
    Excel2007Serializator.SerializeAttribute(writer, constants.RightMargin, pageSetup.RightMargin, double.MinValue);
    Excel2007Serializator.SerializeAttribute(writer, constants.TopMargin, pageSetup.TopMargin, double.MinValue);
    Excel2007Serializator.SerializeAttribute(writer, constants.BottomMargin, pageSetup.BottomMargin, double.MinValue);
    Excel2007Serializator.SerializeAttribute(writer, constants.HeaderMargin, pageSetup.HeaderMargin, double.MinValue);
    Excel2007Serializator.SerializeAttribute(writer, constants.FooterMargin, pageSetup.FooterMargin, double.MinValue);
    writer.WriteEndElement();
  }

  private static void ValidatePageMargins(PageSetupBaseImpl pageSetup)
  {
    if (!pageSetup.dictPaperHeight.ContainsKey(pageSetup.PaperSize) || !pageSetup.dictPaperWidth.ContainsKey(pageSetup.PaperSize))
      return;
    double num1 = pageSetup.dictPaperWidth[pageSetup.PaperSize];
    double num2 = pageSetup.dictPaperHeight[pageSetup.PaperSize];
    if (pageSetup.LeftMargin + pageSetup.RightMargin > num1)
      throw new ArgumentException("Left Margin and Right Margin size exceeds the allowed size");
    if (pageSetup.TopMargin + pageSetup.BottomMargin > num2)
      throw new ArgumentException("Top Margin and Bottom Margin size exceeds the allowed size");
  }

  public static void SerializePageSetup(
    XmlWriter writer,
    IPageSetupBase pageSetup,
    IPageSetupConstantsProvider constants)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pageSetup == null)
      throw new ArgumentNullException(nameof (pageSetup));
    if ((pageSetup as PageSetupBaseImpl).IsNotValidSettings)
      return;
    writer.WriteStartElement(nameof (pageSetup), constants.Namespace);
    Excel2007Serializator.SerializeAttribute(writer, "paperSize", (int) pageSetup.PaperSize, 1);
    if (pageSetup.Zoom != 0)
      Excel2007Serializator.SerializeAttribute(writer, "scale", pageSetup.Zoom, 100);
    Excel2007Serializator.SerializeAttribute(writer, "firstPageNumber", (double) (uint) pageSetup.FirstPageNumber, 1.0);
    if (pageSetup is PageSetupImpl pageSetupImpl)
    {
      Excel2007Serializator.SerializeAttribute(writer, "fitToWidth", pageSetupImpl.FitToPagesWide, 1);
      Excel2007Serializator.SerializeAttribute(writer, "fitToHeight", pageSetupImpl.FitToPagesTall, 1);
    }
    if (pageSetup.Order.ToString() != ExcelOrder.DownThenOver.ToString())
      writer.WriteAttributeString("pageOrder", Excel2007Serializator.LowerFirstLetter(pageSetup.Order.ToString()));
    Excel2007Serializator.SerializeAttribute(writer, "orientation", (Enum) pageSetup.Orientation, (Enum) (ExcelPageOrientation) 0);
    Excel2007Serializator.SerializeAttribute(writer, "blackAndWhite", pageSetup.BlackAndWhite, false);
    Excel2007Serializator.SerializeAttribute(writer, "draft", pageSetup.Draft, false);
    string str1 = Excel2007Serializator.PrintCommentsToString(pageSetup.PrintComments);
    Excel2007Serializator.SerializeAttribute(writer, "cellComments", str1, "none");
    Excel2007Serializator.SerializeAttribute(writer, "useFirstPageNumber", !pageSetup.AutoFirstPageNumber, false);
    string str2 = Excel2007Serializator.PrintErrorsToString(pageSetup.PrintErrors);
    Excel2007Serializator.SerializeAttribute(writer, "errors", str2, "displayed");
    PageSetupBaseImpl pageSetupBaseImpl = (PageSetupBaseImpl) pageSetup;
    if (pageSetupBaseImpl.HResolution > 0)
      Excel2007Serializator.SerializeAttribute(writer, "horizontalDpi", pageSetupBaseImpl.HResolution, 600);
    if (pageSetupBaseImpl.VResolution > 0)
      Excel2007Serializator.SerializeAttribute(writer, "verticalDpi", pageSetupBaseImpl.VResolution, 600);
    if (!pageSetupBaseImpl.IsNotValidSettings)
      Excel2007Serializator.SerializeAttribute(writer, "copies", pageSetup.Copies, 1);
    if (pageSetupImpl != null)
    {
      string relationId = pageSetupImpl.RelationId;
      if (relationId != null)
        writer.WriteAttributeString("id", relationId);
    }
    writer.WriteEndElement();
  }

  internal static void SerializeHeaderFooter(
    XmlWriter writer,
    IPageSetupBase pageSetup,
    IPageSetupConstantsProvider constants)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    PageSetupBaseImpl pageSetupBaseImpl = pageSetup != null ? (PageSetupBaseImpl) pageSetup : throw new ArgumentNullException(nameof (pageSetup));
    Page oddPage = pageSetupBaseImpl.OddPage as Page;
    string fullHeaderString1 = oddPage.FullHeaderString;
    string fullFooterString1 = oddPage.FullFooterString;
    Page evenPage = pageSetupBaseImpl.EvenPage as Page;
    string fullHeaderString2 = evenPage.FullHeaderString;
    string fullFooterString2 = evenPage.FullFooterString;
    Page firstPage = pageSetupBaseImpl.FirstPage as Page;
    string fullHeaderString3 = firstPage.FullHeaderString;
    string fullFooterString3 = firstPage.FullFooterString;
    writer.WriteStartElement("headerFooter", constants.Namespace);
    if (constants.Namespace != "http://schemas.microsoft.com/office/drawing/2014/chartex")
      Excel2007Serializator.SerializeBool(writer, "scaleWithDoc", pageSetupBaseImpl.HFScaleWithDoc);
    Excel2007Serializator.SerializeBool(writer, "alignWithMargins", pageSetupBaseImpl.AlignHFWithPageMargins);
    Excel2007Serializator.SerializeBool(writer, "differentFirst", pageSetupBaseImpl.DifferentFirstPageHF);
    Excel2007Serializator.SerializeBool(writer, "differentOddEven", pageSetupBaseImpl.DifferentOddAndEvenPagesHF);
    if (fullHeaderString1 != null && fullHeaderString1.Length > 0)
      writer.WriteElementString("oddHeader", constants.Namespace, fullHeaderString1);
    if (fullFooterString1 != null && fullFooterString1.Length > 0)
      writer.WriteElementString("oddFooter", constants.Namespace, fullFooterString1);
    if (fullHeaderString2 != null && fullHeaderString2.Length > 0)
      writer.WriteElementString("evenHeader", constants.Namespace, fullHeaderString2);
    if (fullFooterString2 != null && fullFooterString2.Length > 0)
      writer.WriteElementString("evenFooter", constants.Namespace, fullFooterString2);
    if (fullHeaderString3 != null && fullHeaderString3.Length > 0)
      writer.WriteElementString("firstHeader", constants.Namespace, fullHeaderString3);
    if (fullFooterString3 != null && fullFooterString3.Length > 0)
      writer.WriteElementString("firstFooter", constants.Namespace, fullFooterString3);
    writer.WriteEndElement();
  }

  private static string PrintCommentsToString(ExcelPrintLocation printLocation)
  {
    switch (printLocation)
    {
      case ExcelPrintLocation.PrintInPlace:
        return "asDisplayed";
      case ExcelPrintLocation.PrintNoComments:
        return "none";
      case ExcelPrintLocation.PrintSheetEnd:
        return "atEnd";
      default:
        throw new ArgumentOutOfRangeException(nameof (printLocation));
    }
  }

  private static string PrintErrorsToString(ExcelPrintErrors printErrors)
  {
    switch (printErrors)
    {
      case ExcelPrintErrors.PrintErrorsDisplayed:
        return "displayed";
      case ExcelPrintErrors.PrintErrorsBlank:
        return "blank";
      case ExcelPrintErrors.PrintErrorsDash:
        return "dash";
      case ExcelPrintErrors.PrintErrorsNA:
        return "NA";
      default:
        throw new ArgumentOutOfRangeException("printLocation");
    }
  }

  private string ConvertAddressString(string strAdress) => strAdress?.Replace(" ", "%20");

  private void SerializeSheetlevelProperties(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    writer.WriteStartElement("sheetPr");
    Excel2007Serializator.SerializeAttribute(writer, "transitionEvaluation", sheet.IsTransitionEvaluation, false);
    ExcelKnownColors tabColor = sheet.TabColor;
    if (sheet.HasCodeName)
    {
      string codeName = sheet.CodeName;
      if (codeName != null && codeName.Length > 0)
        writer.WriteAttributeString("codeName", codeName);
    }
    if (tabColor != ~ExcelKnownColors.Black)
      this.SerializeColorObject(writer, "tabColor", sheet.TabColorObject);
    IPageSetup pageSetup = sheet.PageSetup;
    if (!pageSetup.IsSummaryColumnRight || !pageSetup.IsSummaryRowBelow)
    {
      writer.WriteStartElement("outlinePr");
      Excel2007Serializator.SerializeAttribute(writer, "summaryRight", pageSetup.IsSummaryColumnRight, true);
      Excel2007Serializator.SerializeAttribute(writer, "summaryBelow", pageSetup.IsSummaryRowBelow, true);
      writer.WriteEndElement();
    }
    if (pageSetup.IsFitToPage)
    {
      writer.WriteStartElement("pageSetUpPr");
      Excel2007Serializator.SerializeAttribute(writer, "fitToPage", pageSetup.IsFitToPage, false);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  internal void SerilizeBackgroundImage(XmlWriter writer, WorksheetBaseImpl workSheetBase)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (workSheetBase == null)
      throw new ArgumentNullException("sheet");
    Bitmap bitmap = !(workSheetBase is WorksheetImpl) ? (workSheetBase as ChartImpl).PageSetup.BackgoundImage : (workSheetBase as WorksheetImpl).PageSetup.BackgoundImage;
    if (bitmap == null)
      return;
    WorksheetDataHolder dataHolder = workSheetBase.DataHolder;
    FileDataHolder parentHolder = dataHolder.ParentHolder;
    string strExtension;
    string pictureContentType = FileDataHolder.GetPictureContentType(bitmap.RawFormat, out strExtension);
    parentHolder.DefaultContentTypes[strExtension] = pictureContentType;
    string str;
    if (workSheetBase.sharedBgImageName == null)
    {
      parentHolder.LastImageIndex = 0;
      str = parentHolder.SaveImage((Image) bitmap, (string) null);
    }
    else
      str = workSheetBase.sharedBgImageName;
    RelationCollection relations = dataHolder.Relations;
    string relationId = relations.GenerateRelationId();
    relations[relationId] = new Relation('/'.ToString() + str, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
    writer.WriteStartElement("picture");
    writer.WriteAttributeString("r", "id", (string) null, relationId);
    writer.WriteEndElement();
  }

  public void SerializeExtendedProperties(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartDocument(true);
    writer.WriteStartElement("Properties", "http://schemas.openxmlformats.org/officeDocument/2006/extended-properties");
    IBuiltInDocumentProperties documentProperties = this.m_book.BuiltInDocumentProperties;
    if (documentProperties.ApplicationName == null && documentProperties.ApplicationName == string.Empty)
      Excel2007Serializator.SerializeElementString(writer, "Application", "Essential XlsIO", (string) null);
    else
      Excel2007Serializator.SerializeElementString(writer, "Application", documentProperties.ApplicationName, (string) null);
    Excel2007Serializator.SerializeElementString(writer, "Characters", documentProperties.CharCount, int.MinValue);
    Excel2007Serializator.SerializeElementString(writer, "Company", documentProperties.Company, (string) null);
    Excel2007Serializator.SerializeElementString(writer, "Lines", documentProperties.LineCount, int.MinValue);
    Excel2007Serializator.SerializeElementString(writer, "Manager", documentProperties.Manager, (string) null);
    Excel2007Serializator.SerializeElementString(writer, "MMClips", documentProperties.MultimediaClipCount, int.MinValue);
    Excel2007Serializator.SerializeElementString(writer, "Notes", documentProperties.SlideCount, int.MinValue);
    Excel2007Serializator.SerializeElementString(writer, "Pages", documentProperties.PageCount, int.MinValue);
    Excel2007Serializator.SerializeElementString(writer, "Paragraphs", documentProperties.ParagraphCount, int.MinValue);
    Excel2007Serializator.SerializeElementString(writer, "PresentationFormat", documentProperties.PresentationTarget, (string) null);
    Excel2007Serializator.SerializeElementString(writer, "Template", documentProperties.Template, (string) null);
    TimeSpan editTime = documentProperties.EditTime;
    if (editTime != TimeSpan.MinValue)
    {
      int totalMinutes = (int) editTime.TotalMinutes;
      writer.WriteElementString("TotalTime", totalMinutes.ToString());
    }
    Excel2007Serializator.SerializeElementString(writer, "Words", documentProperties.WordCount, int.MinValue);
    if (((CustomDocumentProperties) this.m_book.CustomDocumentProperties).GetProperty("_PID_LINKBASE") is DocumentPropertyImpl property)
    {
      byte[] blob = property.Blob;
      string str1 = Encoding.Unicode.GetString(blob, 0, blob.Length);
      string str2 = str1.Remove(str1.Length - 1);
      writer.WriteElementString("HyperlinkBase", str2);
    }
    this.SerializeAppVersion(writer);
    writer.WriteEndElement();
  }

  protected virtual void SerializeAppVersion(XmlWriter writer)
  {
    Excel2007Serializator.SerializeElementString(writer, "AppVersion", "12.0000", (string) null);
  }

  public void SerializeCoreProperties(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    IBuiltInDocumentProperties documentProperties = this.m_book.BuiltInDocumentProperties;
    writer.WriteStartDocument(true);
    writer.WriteStartElement("cp", "coreProperties", "http://schemas.openxmlformats.org/package/2006/metadata/core-properties");
    writer.WriteAttributeString("xmlns", "dc", (string) null, "http://purl.org/dc/elements/1.1/");
    writer.WriteAttributeString("xmlns", "dcterms", (string) null, "http://purl.org/dc/terms/");
    writer.WriteAttributeString("xmlns", "dcmitype", (string) null, "http://purl.org/dc/dcmitype/");
    writer.WriteAttributeString("xmlns", "xsi", (string) null, "http://www.w3.org/2001/XMLSchema-instance");
    Excel2007Serializator.SerializeElementString(writer, "category", documentProperties.Category, (string) null, "cp");
    Excel2007Serializator.SerializeElementString(writer, "creator", documentProperties.Author, (string) null, "dc");
    Excel2007Serializator.SerializeElementString(writer, "description", documentProperties.Comments, (string) null, "dc");
    Excel2007Serializator.SerializeElementString(writer, "keywords", documentProperties.Keywords, (string) null, "cp");
    Excel2007Serializator.SerializeElementString(writer, "lastModifiedBy", documentProperties.LastAuthor, (string) null, "cp");
    this.SerializeCreatedModifiedTimeElement(writer, "created", documentProperties.CreationDate);
    this.SerializeCreatedModifiedTimeElement(writer, "modified", documentProperties.LastSaveDate);
    Excel2007Serializator.SerializeElementString(writer, "subject", documentProperties.Subject, (string) null, "dc");
    DateTime lastPrinted = documentProperties.LastPrinted;
    if (lastPrinted != DateTime.MinValue)
    {
      string str = lastPrinted.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", (IFormatProvider) CultureInfo.InvariantCulture);
      writer.WriteElementString("cp", "lastPrinted", (string) null, str);
    }
    Excel2007Serializator.SerializeElementString(writer, "title", documentProperties.Title, (string) null, "dc");
    writer.WriteEndElement();
  }

  private void SerializeCreatedModifiedTimeElement(
    XmlWriter writer,
    string tagName,
    DateTime dateTime)
  {
    if (!(dateTime.Date != DateTime.MinValue))
      return;
    writer.WriteStartElement("dcterms", tagName, (string) null);
    writer.WriteAttributeString("xsi", "type", (string) null, "dcterms:W3CDTF");
    string data = dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", (IFormatProvider) CultureInfo.InvariantCulture);
    writer.WriteRaw(data);
    writer.WriteEndElement();
  }

  public void SerializeCustomProperties(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartDocument(true);
    writer.WriteStartElement("Properties", "http://schemas.openxmlformats.org/officeDocument/2006/custom-properties");
    writer.WriteAttributeString("xmlns", "vt", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");
    CustomDocumentProperties documentProperties = (CustomDocumentProperties) this.m_book.CustomDocumentProperties;
    int iPropertyId = 2;
    int i = 0;
    for (int count = documentProperties.Count; i < count; ++i)
    {
      DocumentPropertyImpl property = documentProperties[i];
      if (property.Name != "_PID_LINKBASE" && property.Name != "_PID_HLINKS")
      {
        this.SerializeCustomProperty(writer, property, iPropertyId);
        ++iPropertyId;
      }
    }
    writer.WriteEndElement();
  }

  public void SerializeContentTypeProperties(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartDocument(false);
    writer.WriteStartElement("p", "properties", "http://schemas.microsoft.com/office/2006/metadata/properties");
    writer.WriteAttributeString("xmlns", "xsi", (string) null, "http://www.w3.org/2001/XMLSchema-instance");
    writer.WriteAttributeString("xmlns", "pc", (string) null, "http://schemas.microsoft.com/office/infopath/2007/PartnerControls");
    writer.WriteStartElement("documentManagement");
    MetaPropertiesImpl contentTypeProperties = (MetaPropertiesImpl) this.m_book.ContentTypeProperties;
    for (int i = 0; i < contentTypeProperties.Count; ++i)
    {
      MetaPropertyImpl metaPropertyImpl = (MetaPropertyImpl) contentTypeProperties[i];
      string str = metaPropertyImpl.ElementName ?? metaPropertyImpl.InternalName;
      if (this.m_book.m_childElementValues.ContainsKey(metaPropertyImpl.InternalName) || this.m_book.m_childElementValues.ContainsKey(str))
      {
        writer.WriteStartElement(str, metaPropertyImpl.NameSpaceURI);
        List<Stream> streamList = new List<Stream>();
        this.m_book.m_childElementValues.TryGetValue(str, out streamList);
        if (streamList == null)
          this.m_book.m_childElementValues.TryGetValue(metaPropertyImpl.InternalName, out streamList);
        foreach (Stream data in streamList)
          this.SerializeStream(writer, data);
        writer.WriteEndElement();
      }
      else if (metaPropertyImpl.ElementName != null && !string.IsNullOrEmpty(metaPropertyImpl.NameSpaceURI))
      {
        writer.WriteStartElement(metaPropertyImpl.ElementName, metaPropertyImpl.NameSpaceURI);
        if (string.IsNullOrEmpty(metaPropertyImpl.Value))
          writer.WriteAttributeString("xsi", "nil", (string) null, "true");
        else
          writer.WriteValue(metaPropertyImpl.Value);
        writer.WriteEndElement();
      }
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeCustomProperty(
    XmlWriter writer,
    DocumentPropertyImpl property,
    int iPropertyId)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (property == null)
      throw new ArgumentNullException(nameof (property));
    writer.WriteStartElement(nameof (property));
    Excel2007Serializator.SerializeAttribute(writer, "fmtid", "{D5CDD505-2E9C-101B-9397-08002B2CF9AE}", string.Empty);
    Excel2007Serializator.SerializeAttribute(writer, "pid", iPropertyId, int.MinValue);
    Excel2007Serializator.SerializeAttribute(writer, "name", property.Name, string.Empty);
    if (property.LinkToContent)
    {
      Excel2007Serializator.SerializeAttribute(writer, "linkTarget", property.LinkSource, string.Empty);
      if (this.m_book.Names[property.LinkSource] != null && this.m_book.Names[property.LinkSource].RefersToRange != null)
      {
        IRange refersToRange = this.m_book.Names[property.LinkSource].RefersToRange;
        int row = refersToRange.Row;
        int column = refersToRange.Column;
        WorksheetImpl.TRangeValueType cellType = (this.m_book.Worksheets[refersToRange.Worksheet.Name] as WorksheetImpl).GetCellType(row, column, false);
        bool hasDateTime = refersToRange.HasDateTime;
        switch (cellType)
        {
          case WorksheetImpl.TRangeValueType.Boolean:
            property.Value = (object) refersToRange.DisplayText;
            property.PropertyType = PropertyType.Bool;
            break;
          case WorksheetImpl.TRangeValueType.Number:
            property.Value = (object) refersToRange.Value;
            property.PropertyType = PropertyType.Double;
            if (hasDateTime)
            {
              property.PropertyType = PropertyType.DateTime;
              break;
            }
            break;
          case WorksheetImpl.TRangeValueType.String:
            property.Value = (object) refersToRange.DisplayText;
            property.PropertyType = PropertyType.String;
            break;
          default:
            property.Value = (object) refersToRange.DisplayText;
            break;
        }
      }
    }
    switch (property.PropertyType)
    {
      case PropertyType.Int32:
        string str1 = property.Int32.ToString();
        writer.WriteElementString("vt", "i4", (string) null, str1);
        break;
      case PropertyType.Double:
        string str2 = property.Double.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo);
        writer.WriteElementString("vt", "r8", (string) null, str2);
        break;
      case PropertyType.Bool:
        string lower = property.Boolean.ToString().ToLower(CultureInfo.InvariantCulture);
        writer.WriteElementString("vt", "bool", (string) null, lower);
        break;
      case PropertyType.Int:
        string str3 = property.Integer.ToString();
        writer.WriteElementString("vt", "i4", (string) null, str3);
        break;
      case PropertyType.AsciiString:
        writer.WriteElementString("vt", "lpstr", (string) null, property.Text);
        break;
      case PropertyType.String:
        writer.WriteElementString("vt", "lpwstr", (string) null, property.Text);
        break;
      case PropertyType.DateTime:
        string str4 = property.DateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        writer.WriteElementString("vt", "filetime", (string) null, str4);
        break;
    }
    writer.WriteEndElement();
  }

  private void SerializeBookViews(XmlWriter writer, List<Dictionary<string, string>> lstBookViews)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    Dictionary<string, string> dicBookView = lstBookViews == null ? new Dictionary<string, string>() : lstBookViews[0];
    this.ChangeCreateAttributeValue(dicBookView, "activeTab", this.m_book.ActiveSheetIndex);
    this.ChangeCreateAttributeValue(dicBookView, "firstSheet", this.m_book.DisplayedTab);
    if (lstBookViews == null && dicBookView.Count != 0)
    {
      if (!this.m_book.ShowSheetTabs)
        dicBookView.Add("showSheetTabs", this.m_book.ShowSheetTabs.ToString().ToLower());
      lstBookViews = new List<Dictionary<string, string>>();
      lstBookViews.Add(dicBookView);
    }
    if (lstBookViews == null)
    {
      writer.WriteStartElement("bookViews");
      writer.WriteStartElement("workbookView");
      if (!this.m_book.ShowSheetTabs)
        Excel2007Serializator.SerializeAttribute(writer, "showSheetTabs", this.m_book.ShowSheetTabs, true);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    else
    {
      writer.WriteStartElement("bookViews");
      foreach (Dictionary<string, string> lstBookView in lstBookViews)
        this.SerializeWorkbookView(writer, lstBookView);
      writer.WriteEndElement();
    }
  }

  private void SerializeWorkbookView(XmlWriter writer, Dictionary<string, string> dicView)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dicView == null)
      throw new ArgumentNullException(nameof (dicView));
    writer.WriteStartElement("workbookView");
    foreach (KeyValuePair<string, string> keyValuePair in dicView)
      Excel2007Serializator.SerializeAttribute(writer, keyValuePair.Key, keyValuePair.Value, string.Empty);
    writer.WriteEndElement();
  }

  private void ChangeCreateAttributeValue(
    Dictionary<string, string> dicBookView,
    string strAttributeName,
    int iNewValue)
  {
    if (dicBookView.ContainsKey(strAttributeName))
    {
      if (iNewValue != 0)
        dicBookView[strAttributeName] = iNewValue.ToString();
      else
        dicBookView.Remove(strAttributeName);
    }
    else
    {
      if (iNewValue == 0)
        return;
      dicBookView.Add(strAttributeName, iNewValue.ToString());
    }
  }

  private void SerializeBookExternalLinks(XmlWriter writer, RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ExternBookCollection externWorkbooks = this.m_book.ExternWorkbooks;
    IWorksheets worksheets = this.m_book.Worksheets;
    bool flag = true;
    if (externWorkbooks.Count != 0)
    {
      int index = 0;
      for (int count = externWorkbooks.Count; index < count; ++index)
      {
        ExternWorkbookImpl externBook = externWorkbooks[index];
        if (!externBook.IsInternalReference && !string.IsNullOrEmpty(externBook.URL) && !externBook.IsAddInFunctions)
        {
          if (flag)
          {
            flag = false;
            writer.WriteStartElement("externalReferences");
          }
          this.SerializeLink(externBook, writer, relations);
        }
      }
    }
    if (this.m_book.PreservedExternalLinks.Count > 0)
    {
      writer.WriteStartElement("externalReferences");
      foreach (string preservedExternalLink in this.m_book.PreservedExternalLinks)
      {
        writer.WriteStartElement("externalReference");
        writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", preservedExternalLink);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
    if (flag)
      return;
    writer.WriteEndElement();
  }

  private void SerializeLink(
    ExternWorkbookImpl externBook,
    XmlWriter writer,
    RelationCollection relations)
  {
    if (externBook == null)
      throw new ArgumentNullException(nameof (externBook));
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    string str = this.m_book.DataHolder.SerializeExternalLink(externBook);
    writer.WriteStartElement("externalReference");
    string relationId = relations.GenerateRelationId();
    relations[relationId] = new Relation('/'.ToString() + str, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/externalLink");
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
    writer.WriteEndElement();
  }

  private void SerializePagebreaks(XmlWriter writer, IWorksheet sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    HPageBreaksCollection hPagebreaks = sheet != null ? (HPageBreaksCollection) sheet.HPageBreaks : throw new ArgumentNullException(nameof (sheet));
    if (hPagebreaks != null)
      this.SerializeHorizontalPageBreaks(writer, hPagebreaks);
    VPageBreaksCollection vpageBreaks = (VPageBreaksCollection) sheet.VPageBreaks;
    if (vpageBreaks == null)
      return;
    this.SerializeVerticalPageBreaks(writer, vpageBreaks);
  }

  private void SerializeFilterColorStream(
    XmlWriter writer,
    XmlWriter writerdxf,
    ColorFilter colorFilter,
    ref int iDxfIndex)
  {
    if (writer == null)
      throw new ArgumentNullException("Writer");
    if (colorFilter == null)
      throw new ArgumentNullException("ColorFilter");
    writer.WriteStartElement(nameof (colorFilter));
    writerdxf.WriteStartElement("dxf");
    writerdxf.WriteStartElement("fill");
    writerdxf.WriteStartElement("patternFill");
    if (colorFilter.ColorFilterType == ExcelColorFilterType.CellColor && colorFilter.Color.A == (byte) 0)
      writerdxf.WriteAttributeString("patternType", "none");
    else
      writerdxf.WriteAttributeString("patternType", "solid");
    if (colorFilter.Color.A == (byte) 0 && colorFilter.Color.R == (byte) 0 && colorFilter.Color.G == (byte) 0 && colorFilter.Color.B == (byte) 0)
    {
      if (colorFilter.ColorFilterType == ExcelColorFilterType.CellColor)
        this.SerializeColorObject(writerdxf, "fgColor", new ColorObject(ColorType.Indexed, 64 /*0x40*/));
      else
        this.SerializeColorObject(writerdxf, "fgColor", new ColorObject(ColorType.Indexed, 66));
    }
    else
      this.SerializeColorObject(writerdxf, "fgColor", (ColorObject) colorFilter.Color);
    writerdxf.WriteEndElement();
    writerdxf.WriteEndElement();
    writerdxf.WriteEndElement();
    writer.WriteAttributeString("dxfId", iDxfIndex.ToString());
    if (colorFilter.ColorFilterType == ExcelColorFilterType.FontColor)
      writer.WriteAttributeString("cellColor", "0");
    writer.WriteEndElement();
    ++iDxfIndex;
    this.m_colorFilterStreamListIndex = -1;
  }

  private void SerializeSortData(
    XmlWriter writer,
    XmlWriter xmlWriterDxfs,
    IDataSort dataSorter,
    ref int iDxfIndex)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dataSorter == null || dataSorter.SortFields.Count == 0)
      return;
    DataSorter sortState = dataSorter as DataSorter;
    IRange range = !this.m_book.DataSorter.HasHeader || sortState.SortRange.Row >= sortState.SortRange.LastRow ? sortState.SortRange : sortState.Worksheet[sortState.SortRange.Row + 1, sortState.SortRange.Column, sortState.SortRange.LastRow, sortState.SortRange.LastColumn];
    writer.WriteStartElement("sortState");
    writer.WriteAttributeString("ref", range.AddressLocal);
    for (int index = 0; index < sortState.SortFields.Count; ++index)
    {
      ISortField sortField = sortState.SortFields[index];
      this.SerializeSortCondition(writer, xmlWriterDxfs, sortState, sortField, ref iDxfIndex);
    }
    writer.WriteEndElement();
  }

  private void SerializeSortCondition(
    XmlWriter writer,
    XmlWriter writerDxf,
    DataSorter sortState,
    ISortField sortConditions,
    ref int iDxfIndex)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sortConditions == null)
      throw new ArgumentNullException("sortField");
    writer.WriteStartElement("sortCondition");
    if (sortConditions.Order == OrderBy.Descending || sortConditions.Order == OrderBy.OnBottom)
      writer.WriteAttributeString("descending", "1");
    if (sortConditions.SortOn != SortOn.Values)
    {
      switch (sortConditions.SortOn)
      {
        case SortOn.CellColor:
          writer.WriteAttributeString("sortBy", "cellColor");
          break;
        case SortOn.FontColor:
          writer.WriteAttributeString("sortBy", "fontColor");
          break;
      }
    }
    IRange range = !this.m_book.DataSorter.HasHeader || sortState.SortRange.Row >= sortState.SortRange.LastRow ? sortState.Worksheet[sortState.SortRange.Row, sortConditions.Key + 1, sortState.SortRange.LastRow, sortConditions.Key + 1] : sortState.Worksheet[sortState.SortRange.Row + 1, sortConditions.Key + 1, sortState.SortRange.LastRow, sortConditions.Key + 1];
    writer.WriteAttributeString("ref", range.AddressLocal);
    if (sortConditions.SortOn != SortOn.Values)
    {
      this.SerializeDxf(writerDxf, sortConditions);
      writer.WriteAttributeString("dxfId", iDxfIndex.ToString());
      if (this.hasTextRotation)
        --iDxfIndex;
      else
        ++iDxfIndex;
    }
    writer.WriteEndElement();
  }

  private void SerializeHorizontalPageBreaks(XmlWriter writer, HPageBreaksCollection hPagebreaks)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    int num = hPagebreaks != null ? hPagebreaks.Count : throw new ArgumentNullException(nameof (hPagebreaks));
    if (num == 0)
      return;
    writer.WriteStartElement("rowBreaks");
    Excel2007Serializator.SerializeAttribute(writer, "count", num, 0);
    Excel2007Serializator.SerializeAttribute(writer, "manualBreakCount", hPagebreaks.ManualBreakCount, 0);
    SortedList<int, List<HPageBreakImpl>> sortedList = new SortedList<int, List<HPageBreakImpl>>();
    for (int i = 0; i < num; ++i)
    {
      HPageBreakImpl hPagebreak = (HPageBreakImpl) hPagebreaks[i];
      int row = (int) hPagebreak.HPageBreak.Row;
      List<HPageBreakImpl> hpageBreakImplList;
      if (!sortedList.TryGetValue(row, out hpageBreakImplList))
      {
        hpageBreakImplList = new List<HPageBreakImpl>();
        sortedList.Add(row, hpageBreakImplList);
      }
      hpageBreakImplList.Add(hPagebreak);
    }
    int index1 = 0;
    for (int count1 = sortedList.Count; index1 < count1; ++index1)
    {
      List<HPageBreakImpl> hpageBreakImplList = sortedList.Values[index1];
      int index2 = 0;
      for (int count2 = hpageBreakImplList.Count; index2 < count2; ++index2)
      {
        HPageBreakImpl hpageBreakImpl = hpageBreakImplList[index2];
        HorizontalPageBreaksRecord.THPageBreak hpageBreak = hpageBreakImpl.HPageBreak;
        this.SerializeSinglePagebreak(writer, (int) hpageBreak.Row, (int) hpageBreak.StartColumn, (int) hpageBreak.EndColumn, hpageBreakImpl.Type);
      }
    }
    writer.WriteEndElement();
  }

  private void SerializeVerticalPageBreaks(XmlWriter writer, VPageBreaksCollection vPagebreaks)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    int num = vPagebreaks != null ? vPagebreaks.Count : throw new ArgumentNullException(nameof (vPagebreaks));
    if (num == 0)
      return;
    writer.WriteStartElement("colBreaks");
    Excel2007Serializator.SerializeAttribute(writer, "count", num, 0);
    Excel2007Serializator.SerializeAttribute(writer, "manualBreakCount", vPagebreaks.ManualBreakCount, 0);
    SortedList<int, List<VPageBreakImpl>> sortedList = new SortedList<int, List<VPageBreakImpl>>();
    for (int i = 0; i < num; ++i)
    {
      VPageBreakImpl vPagebreak = (VPageBreakImpl) vPagebreaks[i];
      int column = (int) vPagebreak.VPageBreak.Column;
      List<VPageBreakImpl> vpageBreakImplList;
      if (!sortedList.TryGetValue(column, out vpageBreakImplList))
      {
        vpageBreakImplList = new List<VPageBreakImpl>();
        sortedList.Add(column, vpageBreakImplList);
      }
      vpageBreakImplList.Add(vPagebreak);
    }
    int index1 = 0;
    for (int count1 = sortedList.Count; index1 < count1; ++index1)
    {
      List<VPageBreakImpl> vpageBreakImplList = sortedList.Values[index1];
      int index2 = 0;
      for (int count2 = vpageBreakImplList.Count; index2 < count2; ++index2)
      {
        VPageBreakImpl vpageBreakImpl = vpageBreakImplList[index2];
        VerticalPageBreaksRecord.TVPageBreak vpageBreak = vpageBreakImpl.VPageBreak;
        this.SerializeSinglePagebreak(writer, (int) vpageBreak.Column, (int) vpageBreak.StartRow, (int) vpageBreak.EndRow, vpageBreakImpl.Type);
      }
    }
    writer.WriteEndElement();
  }

  private void SerializeSinglePagebreak(
    XmlWriter writer,
    int iRowColumn,
    int iStart,
    int iEnd,
    ExcelPageBreak type)
  {
    writer.WriteStartElement("brk");
    Excel2007Serializator.SerializeAttribute(writer, "id", iRowColumn, 0);
    Excel2007Serializator.SerializeAttribute(writer, "min", iStart, 0);
    Excel2007Serializator.SerializeAttribute(writer, "max", iEnd, 0);
    Excel2007Serializator.SerializeAttribute(writer, "man", type == ExcelPageBreak.PageBreakManual, false);
    writer.WriteEndElement();
  }

  public static void SerializeExtent(XmlWriter writer, Size extent)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    int width = extent.Width;
    int height = extent.Height;
    int num1 = (int) ApplicationImpl.ConvertFromPixel((double) width, MeasureUnits.EMU);
    int num2 = (int) ApplicationImpl.ConvertFromPixel((double) height, MeasureUnits.EMU);
    writer.WriteStartElement("ext", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    if (num1 <= 0)
      num1 = 8666049;
    if (num2 <= 0)
      num2 = 6293304;
    writer.WriteAttributeString("cx", num1.ToString());
    writer.WriteAttributeString("cy", num2.ToString());
    writer.WriteEndElement();
  }

  public void SerializeConnections(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException("Writer");
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartDocument(true);
    writer.WriteStartElement("connections", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    for (int index = 0; index < this.m_book.Connections.Count; ++index)
      this.SerializeConnection(writer, this.m_book.Connections[index] as ExternalConnection);
    for (int index = 0; index < this.m_book.DeletedConnections.Count; ++index)
      this.SerializeConnection(writer, this.m_book.DeletedConnections[index] as ExternalConnection);
    writer.WriteEndElement();
  }

  public void SerializeConnection(XmlWriter writer, ExternalConnection connection)
  {
    if (connection == null)
      return;
    DataBaseProperty dbProperty = new DataBaseProperty();
    if (connection.DataBaseType == ExcelConnectionsType.ConnectionTypeODBC)
      dbProperty = (DataBaseProperty) connection.ODBCConnection;
    else if (connection.DataBaseType == ExcelConnectionsType.ConnectionTypeOLEDB)
      dbProperty = (DataBaseProperty) connection.OLEDBConnection;
    writer.WriteStartElement(nameof (connection));
    writer.WriteAttributeString("id", connection.ConncetionId.ToString());
    writer.WriteAttributeString("sourceFile", connection.SourceFile);
    if (connection.Deleted)
      writer.WriteAttributeString("deleted", "1");
    if (connection.ConnectionFile != null && connection.ConnectionFile != "")
      writer.WriteAttributeString("odcFile", connection.ConnectionFile);
    writer.WriteAttributeString("type", ((int) connection.DataBaseType).ToString());
    writer.WriteAttributeString("name", connection.Name);
    writer.WriteAttributeString("refreshedVersion", connection.RefershedVersion.ToString());
    if (dbProperty.AlwaysUseConnectionFile)
      writer.WriteAttributeString("onlyUseConnectionFile", "1");
    if (dbProperty.SavePassword)
      writer.WriteAttributeString("savePassword", "1");
    if (dbProperty.RefreshPeriod > 0)
      writer.WriteAttributeString("interval", dbProperty.RefreshPeriod.ToString());
    if (dbProperty.ServerCredentialsMethod != ExcelCredentialsMethod.integrated)
      writer.WriteAttributeString("credentials", dbProperty.ServerCredentialsMethod.ToString());
    if (dbProperty.RefreshOnFileOpen)
      writer.WriteAttributeString("refreshOnLoad", "1");
    if (dbProperty.BackgroundQuery)
      Excel2007Serializator.SerializeBool(writer, "background", connection.BackgroundQuery);
    if (connection.Description != null)
      writer.WriteAttributeString("description", connection.Description);
    if (connection.DataBaseType == ExcelConnectionsType.ConnectionTypeODBC || connection.DataBaseType == ExcelConnectionsType.ConnectionTypeOLEDB)
      this.SerializeDataBaseProerty(writer, dbProperty);
    else if (connection.DataBaseType == ExcelConnectionsType.ConnectionTypeWEB)
      this.SerializeWebProperty(writer, connection);
    if (connection.OlapProperty != null)
    {
      connection.OlapProperty.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, connection.OlapProperty);
    }
    if (connection.ExtLstProperty != null)
    {
      connection.ExtLstProperty.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, connection.ExtLstProperty);
    }
    if (connection.m_textPr != null)
    {
      connection.m_textPr.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, connection.m_textPr);
    }
    if (connection.Parameters.Count > 0)
      this.SerializeParameters(writer, connection.Parameters);
    writer.WriteEndElement();
  }

  private void SerializeParameters(XmlWriter writer, IParameters parameters)
  {
    if (writer == null)
      throw new ArgumentNullException("Writer");
    writer.WriteStartElement(nameof (parameters), "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer.WriteAttributeString("count", parameters.Count.ToString());
    for (int index = 0; index < parameters.Count; ++index)
      this.SerializeParameter(writer, parameters[index]);
    writer.WriteEndElement();
  }

  private void SerializeParameter(XmlWriter writer, IParameter parameter)
  {
    ParameterImpl parameterImpl = parameter as ParameterImpl;
    writer.WriteStartElement(nameof (parameter));
    writer.WriteAttributeString("name", parameterImpl.Name);
    if (parameterImpl.DataType != ExcelParameterDataType.ParamTypeUnknown)
      writer.WriteAttributeString("sqlType", ((int) parameterImpl.DataType).ToString());
    if (parameterImpl.Type == ExcelParameterType.Constant)
      writer.WriteAttributeString("parameterType", "value");
    else if (parameterImpl.Type == ExcelParameterType.Range)
      writer.WriteAttributeString("parameterType", "cell");
    else if (parameterImpl.Type == ExcelParameterType.Prompt)
      writer.WriteAttributeString("prompt", parameterImpl.PromptString);
    if (parameter.RefreshOnChange)
      Excel2007Serializator.SerializeBool(writer, "refreshOnChange", parameter.RefreshOnChange);
    switch (parameterImpl.Flag)
    {
      case 1:
        writer.WriteAttributeString("boolean", parameterImpl.Value.ToString());
        break;
      case 2:
        if (parameterImpl.SourceRange == null)
        {
          writer.WriteAttributeString("cell", parameterImpl.CellRange);
          break;
        }
        writer.WriteAttributeString("cell", parameterImpl.SourceRange.Address);
        break;
      case 3:
        writer.WriteAttributeString("double", parameterImpl.Value.ToString());
        break;
      case 4:
        writer.WriteAttributeString("integer", parameterImpl.Value.ToString());
        break;
      case 5:
        writer.WriteAttributeString("string", parameterImpl.Value.ToString());
        break;
    }
    writer.WriteEndElement();
  }

  [Obsolete("This method is obsolete and will be removed soon. Please use SerializeDatabaseProperty(XmlWriter writer, DataBaseProperty dbProperty) method. Sorry for inconvenience.")]
  public void SerializeDataBaseProerty(XmlWriter writer, DataBaseProperty dbProperty)
  {
    writer.WriteStartElement("dbPr");
    writer.WriteAttributeString("connection", (string) dbProperty.ConnectionString);
    writer.WriteAttributeString("command", (string) dbProperty.CommandText);
    if (dbProperty.CommandType != ExcelCommandType.Default)
    {
      int commandType = (int) dbProperty.CommandType;
      writer.WriteAttributeString("commandType", commandType.ToString());
    }
    writer.WriteEndElement();
  }

  public void SerializeDatabaseProperty(XmlWriter writer, DataBaseProperty dbProperty)
  {
    writer.WriteStartElement("dbPr");
    writer.WriteAttributeString("connection", (string) dbProperty.ConnectionString);
    writer.WriteAttributeString("command", (string) dbProperty.CommandText);
    int commandType = (int) dbProperty.CommandType;
    writer.WriteAttributeString("commandType", commandType.ToString());
    writer.WriteEndElement();
  }

  public void SerializeWebProperty(XmlWriter writer, ExternalConnection Connection)
  {
    writer.WriteStartElement("webPr");
    if (Connection.IsXml)
      Excel2007Serializator.SerializeBool(writer, "xml", Connection.IsXml);
    writer.WriteAttributeString("url", Connection.ConnectionURL);
    writer.WriteEndElement();
  }

  private bool CheckSpecialCharacters(string sheetName)
  {
    if (sheetName.StartsWith("'") && sheetName.EndsWith("'"))
      return false;
    foreach (char specialChar in this.SpecialChars)
    {
      if (sheetName.IndexOf(specialChar) != -1)
        return true;
    }
    return false;
  }

  private void SerializeCustomBookViews(
    XmlWriter writer,
    List<Dictionary<string, string>> lstCustomBookViews)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (lstCustomBookViews == null)
      return;
    writer.WriteStartElement("customWorkbookViews");
    foreach (Dictionary<string, string> lstCustomBookView in lstCustomBookViews)
      this.SerializeCustomWorkbookView(writer, lstCustomBookView);
    writer.WriteEndElement();
  }

  private void SerializeCustomWorkbookView(XmlWriter writer, Dictionary<string, string> dicView)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dicView == null)
      throw new ArgumentNullException(nameof (dicView));
    writer.WriteStartElement("customWorkbookView");
    foreach (KeyValuePair<string, string> keyValuePair in dicView)
      Excel2007Serializator.SerializeAttribute(writer, keyValuePair.Key, keyValuePair.Value, string.Empty);
    writer.WriteEndElement();
  }

  public void SerializeConditionValueObject(
    XmlWriter writer,
    IConditionValue conditionValue,
    bool isIconSet,
    bool isMinPoint)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("cfvo");
    int index = (int) conditionValue.Type;
    if (index == 7)
      index = !isMinPoint ? 3 : 2;
    string valueType = CF.ValueTypes[index];
    writer.WriteAttributeString("type", valueType);
    writer.WriteAttributeString("val", conditionValue.Value);
    if (isIconSet)
      writer.WriteAttributeString("gte", ((int) conditionValue.Operator).ToString());
    writer.WriteEndElement();
  }

  private void SerializeCombinationFilters(XmlWriter writer, CombinationFilter combinationFilter)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (combinationFilter == null)
      throw new ArgumentNullException(nameof (combinationFilter));
    if (combinationFilter.IsBlank)
      writer.WriteAttributeString("blank", "1");
    foreach (IMultipleFilter filter in combinationFilter.m_filterCollection)
    {
      switch (filter.CombinationFilterType)
      {
        case ExcelCombinationFilterType.TextFilter:
          this.SerializeFilter(writer, (filter as TextFilter).Text);
          continue;
        case ExcelCombinationFilterType.DateTimeFilter:
          this.SerializeDateTimeFilter(writer, filter as DateTimeFilter);
          continue;
        default:
          continue;
      }
    }
  }

  private void SerializeDateTimeFilter(XmlWriter writer, DateTimeFilter filter)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    DateTimeGroupingType timeGroupingType = filter != null ? filter.GroupingType : throw new ArgumentNullException(nameof (filter));
    DateTime dateTimeValue = filter.DateTimeValue;
    writer.WriteStartElement("dateGroupItem");
    if (timeGroupingType >= DateTimeGroupingType.year)
      writer.WriteAttributeString("year", XmlConvert.ToString(dateTimeValue.Year));
    if (timeGroupingType >= DateTimeGroupingType.month)
      writer.WriteAttributeString("month", XmlConvert.ToString(dateTimeValue.Month));
    if (timeGroupingType >= DateTimeGroupingType.day)
      writer.WriteAttributeString("day", XmlConvert.ToString(dateTimeValue.Day));
    if (timeGroupingType >= DateTimeGroupingType.hour)
      writer.WriteAttributeString("hour", XmlConvert.ToString(dateTimeValue.Hour));
    if (timeGroupingType >= DateTimeGroupingType.minute)
      writer.WriteAttributeString("minute", XmlConvert.ToString(dateTimeValue.Minute));
    if (timeGroupingType >= DateTimeGroupingType.second)
      writer.WriteAttributeString("second", XmlConvert.ToString(dateTimeValue.Second));
    writer.WriteAttributeString("dateTimeGrouping", timeGroupingType.ToString());
    writer.WriteEndElement();
  }

  private void SerializeDateFilter(XmlWriter writer, DynamicFilter filter)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (filter == null)
      throw new ArgumentNullException(nameof (filter));
    if (filter.DateFilterType == DynamicFilterType.None)
      return;
    writer.WriteStartElement("dynamicFilter");
    writer.WriteAttributeString("type", AF.ConvertDateFilterTypeToString(filter.DateFilterType));
    writer.WriteEndElement();
  }

  public enum CellType
  {
    b,
    e,
    inlineStr,
    n,
    s,
    str,
  }

  public enum FormulaType
  {
    array,
    dataTable,
    normal,
    shared,
  }

  private delegate void ProtectionAttributeSerializator(
    XmlWriter writer,
    string attributeName,
    ExcelSheetProtection flag,
    bool defaultValue,
    ExcelSheetProtection protection);
}
