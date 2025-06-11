// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.Excel2007Serializator
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.Compression.Zip;
using Syncfusion.Drawing;
using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlReaders;
using Syncfusion.OfficeChart.Implementation.XmlReaders.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlSerialization.Constants;
using Syncfusion.OfficeChart.Implementation.XmlSerialization.Shapes;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization;

internal class Excel2007Serializator : IDisposable
{
  private const int MaximumFormulaLength = 8000;
  public const string XmlFileHeading = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?>";
  public const string ContentTypesNamespace = "http://schemas.openxmlformats.org/package/2006/content-types";
  public const string HyperlinkNamespace = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink";
  public const string RelationNamespace = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
  public const string XmlNamespaceMain = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
  public const string WorksheetPartType = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet";
  public const string ChartSheetPartType = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chartsheet";
  public const string ExtendedPropertiesPartType = "http://schemas.openxmlformats.org/officeDocument/2006/extended-properties";
  public const string CorePropertiesPartType = "http://schemas.openxmlformats.org/package/2006/metadata/core-properties";
  public const string X14Namespace = "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main";
  public const string PivotFieldUri = "{E15A36E0-9728-4e99-A89B-3F7291B0FE68}";
  public const string ExternListUri = "{962EF5D1-5CA2-4c93-8EF4-DBF5C05439D2}";
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
  public const string MSPrefix = "xm";
  public const string TypesTagName = "Types";
  public const string ExtensionAttributeName = "Extension";
  public const string DefaultTagName = "Default";
  public const string ContentTypeAttributeName = "ContentType";
  public const string OverrideTagName = "Override";
  public const string PartNameAttributeName = "PartName";
  public const string WorkbookTagName = "workbook";
  public const string SheetsTagName = "sheets";
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
  public const string TabSelected = "tabSelected";
  public const string DEF_DEFAULT_ROW_DELIMITER = "\r\n";
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
  public const string MergeCellXmlTagName = "mergeCell";
  public const string RefAttributeName = "ref";
  public const string DefinedNamesXmlTagName = "definedNames";
  public const string DefinedNameXmlTagName = "definedName";
  public const string NameAttributeName = "name";
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
  internal const string SortState = "sortState";
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
  private WorkbookImpl m_book;
  private FormulaUtil m_formulaUtil;
  private RecordExtractor m_recordExtractor;
  private Dictionary<int, ShapeSerializator> m_shapesVmlSerializators = new Dictionary<int, ShapeSerializator>();
  private Dictionary<int, ShapeSerializator> m_shapesHFVmlSerializators = new Dictionary<int, ShapeSerializator>();
  private Dictionary<Type, ShapeSerializator> m_shapesSerializators = new Dictionary<Type, ShapeSerializator>();
  private List<Stream> m_streamsSheetsCF;
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

  public Excel2007Serializator(WorkbookImpl book)
  {
    this.m_book = book != null ? book : throw new ArgumentNullException(nameof (book));
    this.m_formulaUtil = new FormulaUtil(this.m_book.Application, (object) this.m_book, NumberFormatInfo.InvariantInfo, ',', ';');
    this.m_recordExtractor = new RecordExtractor();
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

  public virtual OfficeVersion Version => OfficeVersion.Excel2007;

  internal WorksheetImpl Worksheet => this.m_worksheetImpl;

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

  private void SerializeCalculation(XmlWriter writer)
  {
    writer.WriteStartElement("calcPr");
    bool flag = !this.m_book.PrecisionAsDisplayed;
    Excel2007Serializator.SerializeAttribute(writer, "fullPrecision", flag, !flag);
    writer.WriteAttributeString("calcId", this.m_book.DataHolder.CalculationId);
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
      if (this.m_book.Version == OfficeVersion.Excel2013)
        writer.WriteAttributeString("defaultThemeVersion", "153222");
      else
        writer.WriteAttributeString("defaultThemeVersion", "124226");
    }
    if (!this.m_book.HidePivotFieldList)
      writer.WriteAttributeString("hidePivotFieldList", "1");
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
    List<Rectangle> mergedRegions = mergedCells.MergedRegions;
    if (mergedRegions == null || mergedRegions.Count == 0)
      return;
    int count = mergedRegions.Count;
    writer.WriteStartElement("mergeCells");
    writer.WriteAttributeString("count", count.ToString());
    for (int index = 0; index < count; ++index)
    {
      Rectangle rect = mergedRegions[index];
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
    int count = innerNamesColection != null ? innerNamesColection.Count : 0;
    if (count <= 0)
      return;
    innerNamesColection.SortForSerialization();
    writer.WriteStartElement("definedNames");
    for (int index = 0; index < count; ++index)
    {
      NameImpl nameImpl = (NameImpl) innerNamesColection[index];
      if (nameImpl != null && !nameImpl.Record.IsFunctionOrCommandMacro)
        this.SerializeNamedRange(writer, (IName) nameImpl);
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
    Dictionary<int, int> dictionary = this.SerializeNotNamedXFs(writer, arrFillIndexes, arrBorderIndexes, hashNewParentIndexes);
    this.SerializeStyles(writer, hashNewParentIndexes);
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
    return dictionary;
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
    {
      if (holder != null)
      {
        if (Array.IndexOf((Array) holder.ParentHolder.Archive.Items, (object) relation.Value.Target) >= 0 || !(relation.Value.Type == "http://schemas.microsoft.com/office/2011/relationships/chartColorStyle") && !(relation.Value.Type == "http://schemas.microsoft.com/office/2011/relationships/chartStyle"))
          this.SerializeRelation(writer, relation.Key, relation.Value);
      }
      else
        this.SerializeRelation(writer, relation.Key, relation.Value);
    }
    writer.WriteEndElement();
  }

  internal void serializeSheet(
    XmlWriter writer,
    WorksheetImpl sheet,
    Stream sheetStream,
    Dictionary<int, int> hashXFIndexes,
    Stream sheetAfterDataStream,
    Dictionary<string, string> extraAttributes,
    bool isCustomFile)
  {
    this.m_worksheetImpl = sheet;
    writer.WriteStartElement("worksheet", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    writer.WriteAttributeString("xmlns", "x14", (string) null, "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    writer.WriteAttributeString("xmlns", "mc", (string) null, "http://schemas.openxmlformats.org/markup-compatibility/2006");
    if (isCustomFile && extraAttributes != null)
    {
      foreach (KeyValuePair<string, string> extraAttribute in extraAttributes)
      {
        if (extraAttribute.Key != "xmlns" && extraAttribute.Key != "xmlns:r" && extraAttribute.Key != "xmlns:x14" && extraAttribute.Key != "xmlns:mc")
          writer.WriteAttributeString(extraAttribute.Key, extraAttribute.Value);
      }
    }
    if (isCustomFile)
      this.SerializeSheetlevelProperties(writer, sheet);
    this.SerializeDimensions(writer, this.m_worksheetImpl);
    if (isCustomFile)
    {
      this.SerializeSheetViews(writer, sheet);
      this.SerializeSheetFormatProperties(writer, sheet);
      this.SerializeColumns(writer, sheet, hashXFIndexes);
    }
    this.SerializeSheetData(writer, sheet.CellRecords, hashXFIndexes, "c", (Dictionary<string, string>) null, true);
    if (!isCustomFile || sheetAfterDataStream == null || sheetAfterDataStream.Length <= 0L)
      return;
    sheetAfterDataStream.Position = 0L;
    XmlReader reader = UtilityMethods.CreateReader(sheetAfterDataStream);
    while (reader.Name == "root")
    {
      reader.Read();
      if (reader.EOF)
        break;
    }
    while (!reader.EOF && (reader.Name != "root" || reader.NodeType != XmlNodeType.EndElement))
      writer.WriteNode(reader, false);
  }

  [SecurityCritical]
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
    this.SerializeSheetlevelProperties(writer, sheet);
    this.SerializeDimensions(writer, sheet);
    this.SerializeSheetViews(writer, sheet);
    this.SerializeSheetFormatProperties(writer, sheet);
    this.SerializeColumns(writer, sheet, hashXFIndexes);
    if (sheet.ImportDTHelper != null)
      this.SerializeDataTable(writer, sheet);
    else
      this.SerializeSheetData(writer, sheet.CellRecords, hashXFIndexes, "c", (Dictionary<string, string>) null, true);
    this.SerializeSheetProtection(writer, (WorksheetBaseImpl) sheet);
    this.SerializeMerges(writer, sheet.MergeCells);
    Stream data = streamConFormats == null || streamConFormats.Length == 0L ? this.GetWorksheetCFStream(sheet.Index) : streamConFormats;
    this.SerializeStream(writer, data);
    IPageSetupConstantsProvider constants = (IPageSetupConstantsProvider) new WorksheetPageSetupConstants();
    Excel2007Serializator.SerializePrintSettings(writer, (IPageSetupBase) sheet.PageSetup, constants, false);
    this.SerializePagebreaks(writer, (IWorksheet) sheet);
    this.SerializeDrawingsWorksheetPart(writer, sheet);
    this.SerializeVmlShapesWorksheetPart(writer, (WorksheetBaseImpl) sheet);
    Excel2007Serializator.SerializeVmlHFShapesWorksheetPart(writer, (WorksheetBaseImpl) sheet, constants, (RelationCollection) null);
    this.SerilizeBackgroundImage(writer, sheet);
    this.SerializeControls(writer, sheet);
    if (sheet.ImportDTHelper == null && sheet.Version != OfficeVersion.Excel97to2003 && sheet.Version != OfficeVersion.Excel2007)
      new Excel2010Serializator(this.m_book).SerilaizeExtensions(writer, sheet);
    writer.WriteEndElement();
  }

  private void SerializeSheetFormatProperties(XmlWriter writer, WorksheetImpl sheet)
  {
    writer.WriteStartElement("sheetFormatPr");
    if (sheet.DefaultColumnWidth != 8.43)
      writer.WriteAttributeString("defaultColWidth", XmlConvert.ToString(sheet.DefaultColumnWidth));
    else if (sheet.StandardWidth != 8.43)
      writer.WriteAttributeString("defaultColWidth", XmlConvert.ToString(sheet.StandardWidth));
    if (this.m_worksheetImpl.IsZeroHeight)
      writer.WriteAttributeString("zeroHeight", XmlConvert.ToString(true));
    if (sheet.CustomHeight)
      writer.WriteAttributeString("customHeight", XmlConvert.ToString(true));
    writer.WriteAttributeString("defaultRowHeight", XmlConvert.ToString(sheet.StandardHeight));
    if (sheet.OutlineLevelColumn > (byte) 0)
      writer.WriteAttributeString("outlineLevelCol", XmlConvert.ToString(sheet.OutlineLevelColumn));
    if (sheet.OutlineLevelRow > (byte) 0)
      writer.WriteAttributeString("outlineLevelRow", XmlConvert.ToString(sheet.OutlineLevelRow));
    if (sheet.BaseColumnWidth != 8)
      writer.WriteAttributeString("baseColWidth", XmlConvert.ToString(sheet.BaseColumnWidth));
    if (sheet.IsThickTop)
      writer.WriteAttributeString("thickTop", XmlConvert.ToString(true));
    if (sheet.IsThickBottom)
      writer.WriteAttributeString("thickBottom", XmlConvert.ToString(true));
    writer.WriteEndElement();
  }

  internal void SerializeDataTable(XmlWriter writer, WorksheetImpl sheet)
  {
    DataTable dataTable = sheet.ImportDTHelper.DataTable;
    int firstRow = sheet.ImportDTHelper.FirstRow;
    int firstColumn = sheet.ImportDTHelper.FirstColumn;
    int num = dataTable.Columns.Count + firstColumn;
    int count = dataTable.Rows.Count;
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("sheetData");
    for (int index1 = 0; index1 < dataTable.Rows.Count; ++index1)
    {
      DataRow row = dataTable.Rows[index1];
      writer.WriteStartElement("row");
      Excel2007Serializator.SerializeAttribute(writer, "r", (firstRow + index1).ToString(), string.Empty);
      string str = $"{firstColumn.ToString()}:{(num - 1).ToString()}";
      Excel2007Serializator.SerializeAttribute(writer, "spans", str, string.Empty);
      for (int index2 = 0; index2 < dataTable.Columns.Count; ++index2)
      {
        DataColumn column = dataTable.Columns[index2];
        writer.WriteStartElement("c");
        string cellName = RangeImpl.GetCellName(firstColumn + index2, firstRow + index1);
        Excel2007Serializator.SerializeAttribute(writer, "r", cellName, (string) null);
        switch (column.DataType.Name)
        {
          case "String":
            Excel2007Serializator.SerializeAttribute(writer, "t", "s", "n");
            object obj1 = row[column];
            writer.WriteElementString("v", obj1.ToString());
            break;
          case "DateTime":
            Excel2007Serializator.SerializeAttribute(writer, "s", sheet.ImportDTHelper.DateStyleIndex, 0);
            double oaDate = ((DateTime) row[column]).ToOADate();
            writer.WriteElementString("v", oaDate.ToString());
            break;
          default:
            object obj2 = row[column];
            if (obj2.ToString() == "NaN")
              obj2 = (object) 0;
            writer.WriteElementString("v", obj2.ToString());
            break;
        }
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
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
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    Stream controlsStream = sheet.DataHolder.ControlsStream;
    if (controlsStream == null)
      return;
    bool flag = Excel2007Serializator.HasAlternateContent(sheet.Shapes);
    if (flag && sheet.HasAlternateContent)
      Excel2007Serializator.WriteAlternateContentControlsHeader(writer);
    controlsStream.Position = 0L;
    XmlReader reader = UtilityMethods.CreateReader(controlsStream);
    writer.WriteNode(reader, false);
    writer.Flush();
    if (!flag || !sheet.HasAlternateContent)
      return;
    Excel2007Serializator.WriteAlternateContentFooter(writer);
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
    writer.WriteAttributeString("Requires", "x14");
  }

  public void SerializeSheetProtection(XmlWriter writer, WorksheetBaseImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    OfficeSheetProtection protection = sheet != null ? sheet.InnerProtection : throw new ArgumentNullException(nameof (sheet));
    bool flag = sheet is ChartImpl;
    if (!sheet.ProtectContents && (!flag || protection == OfficeSheetProtection.None))
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
    OfficeSheetProtection flag,
    bool defaultValue,
    OfficeSheetProtection protection)
  {
    bool flag1 = (protection & flag) == OfficeSheetProtection.None;
    Excel2007Serializator.SerializeAttribute(writer, attributeName, flag1, defaultValue);
  }

  private void SerializeChartProtectionAttribute(
    XmlWriter writer,
    string attributeName,
    OfficeSheetProtection flag,
    bool defaultValue,
    OfficeSheetProtection protection)
  {
    bool flag1 = (protection & flag) != OfficeSheetProtection.None;
    Excel2007Serializator.SerializeAttribute(writer, attributeName, flag1, defaultValue);
  }

  private void SerializeIgnoreErrors(XmlWriter writer, WorksheetImpl sheet)
  {
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

  public void SerializeCommentNotes(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    writer.WriteStartDocument(true);
    writer.WriteStartElement("comments", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
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

  [SecurityCritical]
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
    string prefix = (string) null;
    string localName1;
    string str;
    string localName2;
    string ns;
    if (flag)
    {
      localName1 = "cdr";
      str = "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
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
      ShapeImpl shape = (ShapeImpl) shapes[index2];
      if (shape.ShapeType == OfficeShapeType.AutoShape)
      {
        Serializator serializator = new Serializator();
        AutoShapeImpl autoShapeImpl = shape as AutoShapeImpl;
        if (autoShapeImpl.ShapeExt.ShapeID <= 0)
          autoShapeImpl.ShapeExt.ShapeID = index2 + 1;
        serializator.AddShape(autoShapeImpl.ShapeExt, writer);
      }
      else if (!shape.VmlShape || shape.EnableAlternateContent)
      {
        ShapeSerializator shapeSerializator;
        if (this.m_shapesSerializators.TryGetValue(shape.GetType(), out shapeSerializator))
          shapeSerializator.Serialize(writer, shape, holder, holder.DrawingsRelations);
        else if ((!shape.VmlShape || shape.EnableAlternateContent) && shape.XmlDataStream != null && !flag)
          new DrawingShapeSerializator().Serialize(writer, shape, holder, holder.DrawingsRelations);
        else if (this.Worksheet.preservedStreams != null || shape.preservedCnxnShapeStreams != null || shape.preservedPictureStreams != null || shape.preservedShapeStreams != null)
        {
          this.SerializeShape(writer, shape, holder, holder.DrawingsRelations, index1);
          ++index1;
        }
      }
    }
    writer.WriteEndElement();
  }

  [SecurityCritical]
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
    writer.WriteStartElement("twoCellAnchor", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    DrawingShapeSerializator shapeSerializator1 = new DrawingShapeSerializator();
    shapeSerializator1.SerializeAnchorPoint(writer, "from", shape.LeftColumn, shape.LeftColumnOffset, shape.TopRow, shape.TopRowOffset, shape.Worksheet, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    shapeSerializator1.SerializeAnchorPoint(writer, "to", shape.RightColumn, shape.RightColumnOffset, shape.BottomRow, shape.BottomRowOffset, shape.Worksheet, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    if (shape.preservedCnxnShapeStreams != null)
    {
      for (int index1 = 0; index1 < shape.preservedCnxnShapeStreams.Count; ++index1)
        this.SerializeStream(writer, shape.preservedCnxnShapeStreams[index1]);
    }
    if (shape.GraphicFrameStream != null)
    {
      ChartShapeImpl childShape = (ChartShapeImpl) shape.ChildShapes[0];
      ChartImpl chartObject = childShape.ChartObject;
      ChartShapeSerializator shapeSerializator2 = new ChartShapeSerializator();
      shapeSerializator2.SerializeChartFile(holder, chartObject, out string _);
      writer.WriteStartElement("graphicFrame", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      writer.WriteAttributeString("macro", string.Empty);
      shapeSerializator2.SerializeNonVisualGraphicFrameProperties(writer, childShape, holder);
      DrawingShapeSerializator.SerializeForm(writer, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing", "http://schemas.openxmlformats.org/drawingml/2006/main", childShape.OffsetX, childShape.OffsetY, childShape.ExtentsX, childShape.ExtentsY);
      shapeSerializator2.SerializeSlicerGraphics(writer, childShape);
      shape.ChildShapes.Remove((ShapeImpl) childShape);
    }
    else if (shape.preservedShapeStreams != null || shape.preservedPictureStreams != null || shape.ChildShapes.Count > 0)
    {
      writer.WriteStartElement("grpSp", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      index *= 2;
      if (this.Worksheet.preservedStreams != null && index < this.Worksheet.preservedStreams.Count)
      {
        this.SerializeStream(writer, this.Worksheet.preservedStreams[index]);
        this.SerializeStream(writer, this.Worksheet.preservedStreams[index + 1]);
      }
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
        ChartShapeSerializator shapeSerializator3 = new ChartShapeSerializator();
        string chartFileName;
        string strRelationId = shapeSerializator3.SerializeChartFile(holder, chartObject, out chartFileName);
        shapeSerializator3.SerializeChartProperties(writer, childShape, strRelationId, holder, true);
        holder.SerializeRelations(chartObject.Relations, chartFileName.Substring(1), (WorksheetDataHolder) null);
      }
      writer.WriteEndElement();
    }
    writer.WriteElementString("clientData", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing", string.Empty);
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
    return !book.IsOleLink ? this.SerializeExternalLink(writer, book) : this.SerializeOleObjectLink(writer, book);
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
    writer.WriteAttributeString("sheetId", sheetId.ToString());
    writer.WriteEndElement();
  }

  private void SerializeDrawingsWorksheetPart(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (sheet.Shapes.Count - sheet.VmlShapesCount <= 0 && !Excel2007Serializator.HasAlternateContent(sheet.Shapes))
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
    WorksheetDataHolder worksheetDataHolder = sheet != null ? sheet.DataHolder : throw new ArgumentNullException(nameof (sheet));
    string id = worksheetDataHolder.VmlDrawingsId;
    if (id == null)
    {
      worksheetDataHolder.VmlDrawingsId = id = worksheetDataHolder.Relations.GenerateRelationId();
      worksheetDataHolder.Relations[id] = (Relation) null;
    }
    writer.WriteStartElement("legacyDrawing");
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", id);
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
    writer.WriteStartElement("legacyDrawingHF");
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", id);
    writer.WriteEndElement();
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
    if (sheet.IsFreezePanes && topLeftCell != null && (topLeftCell.Row != 1 || topLeftCell.Column != 1))
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
      if (sheet.View == OfficeSheetView.PageLayout)
        str = "pageLayout";
      writer.WriteAttributeString("view", str);
    }
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
      if (((WorksheetBaseImpl) tabSheets[index]).m_dataHolder != null)
        this.SerializeSheetTag(writer, tabSheets[index]);
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
    string str1 = name != null ? ((NameImpl) name).GetValue(this.m_formulaUtil) : throw new ArgumentNullException(nameof (name));
    if (string.IsNullOrEmpty(str1))
      return;
    writer.WriteStartElement("definedName");
    writer.WriteAttributeString(nameof (name), this.m_book.RemoveInvalidXmlCharacters(name.Name));
    if (name.IsLocal)
    {
      string str2 = this.GetLocalSheetIndex(((NameImpl) name).Worksheet).ToString();
      writer.WriteAttributeString("localSheetId", str2);
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
      if (tabSheets[index].Name == sheet.Name)
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
      IOfficeFont font = innerFonts[index];
      this.SerializeFont(writer, font, "font");
    }
    writer.WriteEndElement();
  }

  private void SerializeFont(XmlWriter writer, IOfficeFont font, string strElement)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    writer.WriteStartElement(strElement);
    if (font.Bold)
      writer.WriteElementString("b", string.Empty);
    if (font.VerticalAlignment != OfficeFontVerticalAlignment.Baseline)
    {
      writer.WriteStartElement("vertAlign");
      writer.WriteAttributeString("val", font.VerticalAlignment.ToString().ToLower(CultureInfo.InvariantCulture));
      writer.WriteEndElement();
    }
    if (font.Italic)
      writer.WriteElementString("i", string.Empty);
    OfficeUnderline underline = font.Underline;
    if (underline != OfficeUnderline.None)
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
    if (font.Color != (OfficeKnownColors) 32767 /*0x7FFF*/)
      this.SerializeFontColor(writer, "color", (font as IInternalFont).Font.ColorObject);
    string localName = "name";
    if (strElement == "rPr")
      localName = "rFont";
    writer.WriteStartElement(localName);
    writer.WriteAttributeString("val", font.FontName);
    writer.WriteEndElement();
    int charSet = (int) ((FontImpl) font).CharSet;
    if (charSet != 1)
    {
      writer.WriteStartElement("charset");
      writer.WriteAttributeString("val", charSet.ToString());
      writer.WriteEndElement();
    }
    if (font.MacOSShadow)
      writer.WriteElementString("shadow", string.Empty);
    writer.WriteEndElement();
  }

  private void SerializeFontColor(XmlWriter writer, string tagName, ChartColor color)
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
    List<FormatRecord> usedFormats = this.m_book.InnerFormats.GetUsedFormats(OfficeVersion.Excel2007);
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
    string str = format.FormatString;
    if (format.FormatString.Equals("Standard"))
      str = "General";
    writer.WriteAttributeString("formatCode", str);
    writer.WriteEndElement();
  }

  private int[] SerializeFills(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    Dictionary<FillImpl, int> dictionary = new Dictionary<FillImpl, int>();
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
          key.Pattern = OfficePattern.None;
          key.PatternColorObject.SetIndexed(OfficeKnownColors.BlackCustom);
          key.ColorObject.SetIndexed(OfficeKnownColors.White | OfficeKnownColors.BlackCustom);
          break;
        case 0:
          key = new FillImpl();
          key.Pattern = OfficePattern.Percent10;
          key.PatternColorObject.SetIndexed(OfficeKnownColors.BlackCustom);
          key.ColorObject.SetIndexed(OfficeKnownColors.White | OfficeKnownColors.BlackCustom);
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
    if (fill.Pattern == OfficePattern.Gradient)
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
    if (fill.Pattern == OfficePattern.Solid)
    {
      this.SerializeColorObject(writer, "fgColor", fill.ColorObject);
      this.SerializeColorObject(writer, "bgColor", fill.PatternColorObject);
    }
    else
    {
      ChartColor patternColorObject = fill.PatternColorObject;
      if (patternColorObject.ColorType != ColorType.Indexed || patternColorObject.GetIndexed((IWorkbook) this.m_book) != (OfficeKnownColors.White | OfficeKnownColors.BlackCustom))
        this.SerializeColorObject(writer, "fgColor", patternColorObject);
      ChartColor colorObject = fill.ColorObject;
      if (colorObject.ColorType != ColorType.Indexed || colorObject.GetIndexed((IWorkbook) this.m_book) != OfficeKnownColors.BlackCustom)
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
      case OfficeGradientStyle.FromCorner:
      case OfficeGradientStyle.FromCenter:
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
    OfficeGradientStyle gradientStyle = fill.GradientStyle;
    OfficeGradientVariants gradientVariant = fill.GradientVariant;
    double num1 = 0.0;
    if (gradientVariant == OfficeGradientVariants.ShadingVariants_3)
    {
      switch (gradientStyle)
      {
        case OfficeGradientStyle.Horizontal:
          num1 = 90.0;
          goto case OfficeGradientStyle.Vertical;
        case OfficeGradientStyle.Vertical:
          Excel2007Serializator.SerializeAttribute(writer, "degree", num1, 0.0);
          this.SerializeStopColorElements(writer, 0.0, fill.ColorObject);
          this.SerializeStopColorElements(writer, 0.5, fill.PatternColorObject);
          this.SerializeStopColorElements(writer, 1.0, fill.ColorObject);
          break;
        case OfficeGradientStyle.DiagonalUp:
          num1 = 45.0;
          goto case OfficeGradientStyle.Vertical;
        case OfficeGradientStyle.DiagonalDown:
          num1 = 135.0;
          goto case OfficeGradientStyle.Vertical;
        default:
          throw new ArgumentException("Unknown gradient style");
      }
    }
    else
    {
      double num2;
      switch (gradientStyle)
      {
        case OfficeGradientStyle.Horizontal:
          num2 = gradientVariant == OfficeGradientVariants.ShadingVariants_1 ? 90.0 : 270.0;
          break;
        case OfficeGradientStyle.Vertical:
          num2 = gradientVariant == OfficeGradientVariants.ShadingVariants_1 ? 0.0 : 180.0;
          break;
        case OfficeGradientStyle.DiagonalUp:
          num2 = gradientVariant == OfficeGradientVariants.ShadingVariants_1 ? 45.0 : 225.0;
          break;
        case OfficeGradientStyle.DiagonalDown:
          num2 = gradientVariant == OfficeGradientVariants.ShadingVariants_1 ? 135.0 : 315.0;
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
    OfficeGradientStyle gradientStyle = fill.GradientStyle;
    OfficeGradientVariants gradientVariant = fill.GradientVariant;
    Excel2007Serializator.SerializeAttribute(writer, "type", "path", string.Empty);
    double num1 = double.MinValue;
    double num2 = double.MinValue;
    double num3 = double.MinValue;
    double num4 = double.MinValue;
    if (gradientStyle == OfficeGradientStyle.FromCenter)
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
        case OfficeGradientVariants.ShadingVariants_1:
          break;
        case OfficeGradientVariants.ShadingVariants_2:
          num3 = num4 = 1.0;
          break;
        case OfficeGradientVariants.ShadingVariants_3:
          num1 = num2 = 1.0;
          break;
        case OfficeGradientVariants.ShadingVariants_4:
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

  private void SerializeStopColorElements(XmlWriter writer, double dPosition, ChartColor color)
  {
    writer.WriteStartElement("stop");
    Excel2007Serializator.SerializeAttribute(writer, "position", dPosition, double.MinValue);
    this.SerializeColorObject(writer, nameof (color), color);
    writer.WriteEndElement();
  }

  private string ConvertPatternToString(OfficePattern pattern)
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
        WorkbookImpl workbookImpl = new WorkbookImpl(this.m_book.Application, (object) this.m_book, OfficeVersion.Excel2007);
        BordersCollection bordersCollection = new BordersCollection(workbookImpl.Application, (object) workbookImpl, true);
        ExtendedFormatWrapper impl = new ExtendedFormatWrapper(workbookImpl, 0);
        bordersCollection.InnerList.Clear();
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(workbookImpl.Application, (object) workbookImpl, (IInternalExtendedFormat) impl, OfficeBordersIndex.DiagonalDown));
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(workbookImpl.Application, (object) workbookImpl, (IInternalExtendedFormat) impl, OfficeBordersIndex.DiagonalUp));
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(workbookImpl.Application, (object) workbookImpl, (IInternalExtendedFormat) impl, OfficeBordersIndex.EdgeBottom));
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(workbookImpl.Application, (object) workbookImpl, (IInternalExtendedFormat) impl, OfficeBordersIndex.EdgeLeft));
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(workbookImpl.Application, (object) workbookImpl, (IInternalExtendedFormat) impl, OfficeBordersIndex.EdgeRight));
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(workbookImpl.Application, (object) workbookImpl, (IInternalExtendedFormat) impl, OfficeBordersIndex.EdgeTop));
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

  private void SerializeIndexedColor(XmlWriter writer, string tagName, OfficeKnownColors color)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tagName == null || tagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (tagName));
    writer.WriteStartElement(tagName);
    if (color > (OfficeKnownColors.White | OfficeKnownColors.BlackCustom))
      writer.WriteAttributeString("auto", "1");
    else
      writer.WriteAttributeString("indexed", ((int) color).ToString());
    writer.WriteEndElement();
  }

  public void SerializeRgbColor(XmlWriter writer, string tagName, ChartColor color)
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

  private void SerializeThemeColor(XmlWriter writer, string tagName, ChartColor color)
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

  private void SerializeColorObject(XmlWriter writer, string tagName, ChartColor color)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tagName == null || tagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (tagName));
    switch (color.ColorType)
    {
      case ColorType.Indexed:
        this.SerializeIndexedColor(writer, tagName, (OfficeKnownColors) color.Value);
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
    Excel2007Serializator.SerializeAttribute(writer, "diagonalUp", borders[OfficeBordersIndex.DiagonalUp].ShowDiagonalLine, false);
    Excel2007Serializator.SerializeAttribute(writer, "diagonalDown", borders[OfficeBordersIndex.DiagonalDown].ShowDiagonalLine, false);
    this.SerializeBorder(writer, (BorderImpl) borders[OfficeBordersIndex.EdgeLeft]);
    this.SerializeBorder(writer, (BorderImpl) borders[OfficeBordersIndex.EdgeRight]);
    this.SerializeBorder(writer, (BorderImpl) borders[OfficeBordersIndex.EdgeTop]);
    this.SerializeBorder(writer, (BorderImpl) borders[OfficeBordersIndex.EdgeBottom]);
    this.SerializeBorder(writer, (BorderImpl) borders[OfficeBordersIndex.DiagonalUp]);
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
    if (border.LineStyle != OfficeLineStyle.None)
    {
      writer.WriteAttributeString("style", this.GetBorderLineStyle(border));
      this.SerializeColorObject(writer, "color", border.ColorObject);
    }
    writer.WriteEndElement();
  }

  private static string GetBorderTag(OfficeBordersIndex borderIndex)
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
        dictionary.Add(format.Index, num);
        this.SerializeExtendedFormat(writer, arrFillIndexes, arrBorderIndexes, format, hashNewParentIndexes, false);
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
    if (format.HorizontalAlignment != OfficeHAlign.HAlignGeneral)
    {
      string str = ((Excel2007HAlign) format.HorizontalAlignment).ToString();
      writer.WriteAttributeString("horizontal", str);
    }
    if (format.VerticalAlignment != OfficeVAlign.VAlignBottom)
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
    return format.HorizontalAlignment == OfficeHAlign.HAlignGeneral && format.IndentLevel == 0 && !format.JustifyLast && format.ReadingOrder == OfficeReadingOrderType.Context && !format.ShrinkToFit && format.Rotation == 0 && !format.WrapText && format.VerticalAlignment == OfficeVAlign.VAlignBottom;
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
      if (styleExt == null || (style.BuiltIn || !styleExt.IsBuildInStyle) && !styleExt.IsHidden)
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

  public void SeiralizeSheet(XmlWriter writer, WorksheetBaseImpl sheet)
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
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (row == null)
      throw new ArgumentNullException(nameof (row));
    if (cells == null)
      throw new ArgumentNullException(nameof (cells));
    RowStorageEnumerator enumerator = row.GetEnumerator(this.m_recordExtractor) as RowStorageEnumerator;
    while (enumerator.MoveNext())
    {
      BiffRecordRaw current = enumerator.Current as BiffRecordRaw;
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
        default:
          this.SerializeCell(writer, current, enumerator, cells, hashNewParentIndexes, cellTag);
          continue;
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
    Excel2007Serializator.CellType cellType = this.GetCellDataType(record, out strCellType);
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
        this.SerializeArrayFormula(writer, arrayRecord);
      else
        this.SerializeSimpleFormula(writer, formulaRecord, cells);
      this.SerializeFormulaValue(writer, formulaRecord, cellType, rowStorageEnumerator);
    }
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
    string ptgArray = this.m_formulaUtil.ParsePtgArray(arrayRecord.Formula);
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
    CellRecordCollection cells)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (formulaRecord == null)
      throw new ArgumentNullException(nameof (formulaRecord));
    if (cells == null)
      throw new ArgumentNullException(nameof (cells));
    string empty = string.Empty;
    string text;
    if (formulaRecord.Formula != null && formulaRecord.Formula.Length == 0)
    {
      text = (cells.Sheet as WorksheetImpl).m_formulaString;
    }
    else
    {
      Ptg ptg = formulaRecord.Formula[0];
      if (ptg.TokenCode == FormulaToken.tExp)
      {
        ControlPtg controlPtg = ptg as ControlPtg;
        if (cells.Table.Rows[controlPtg.RowIndex].HasFormulaArrayRecord(controlPtg.ColumnIndex))
          return;
      }
      this.m_formulaUtil.CheckFormulaVersion(formulaRecord.Formula);
      text = this.m_formulaUtil.ParsePtgArray(formulaRecord.Formula, 0, 0, false, (NumberFormatInfo) null, false, true, (IWorksheet) cells.Sheet);
    }
    if (text[0] == '=')
      text = UtilityMethods.RemoveFirstCharUnsafe(text);
    if (text.Length > 8000)
      throw new ApplicationException($"Formula length is too big. Maximum formula length is {(object) 8000}.");
    writer.WriteStartElement("f");
    bool flag = !this.m_formulaUtil.HasExternalReference(formulaRecord.Formula) || formulaRecord.CalculateOnOpen;
    Excel2007Serializator.SerializeAttribute(writer, "ca", flag, false);
    writer.WriteString(text);
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

  private Excel2007Serializator.CellType GetCellDataType(
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
      if (!text1.Contains("\r\n"))
        text1 = text1.Replace("\n", "\r\n");
      int length = text1.Length;
      writer.WriteStartElement("t");
      if (length > 0 && (text1[0] == ' ' || text1[length - 1] == ' ') || text != null && text.IsPreserved)
        writer.WriteAttributeString("xml", "space", (string) null, "preserve");
      string text2 = this.ReplaceWrongChars(this.PrepareString(text1));
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
    string strString = string.Empty;
    int iFontIndex = -1;
    int startIndex = 0;
    int length1 = text1.Length;
    foreach (KeyValuePair<int, int> keyValuePair in formattingRuns)
    {
      int length2 = keyValuePair.Key - startIndex;
      if (length1 >= length2)
        strString = text1.Substring(startIndex, length2);
      this.SerializeRichTextRunSingleEntry(writer, innerFonts, strString, iFontIndex);
      iFontIndex = keyValuePair.Value;
      startIndex += length2;
    }
    if (length1 >= startIndex)
      strString = text1.Substring(startIndex);
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
    if (!strString.Contains("\r\n"))
      strString = strString.Replace("\n", "\r\n");
    writer.WriteStartElement("r");
    if (iFontIndex != -1)
    {
      IOfficeFont font = fonts[iFontIndex];
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
      this.SerializeRgbColor(writer, "rgbColor", (ChartColor) color);
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
    int sameColumns = this.FindSameColumns(sheet, (int) columnInfo.FirstColumn + 1);
    writer.WriteStartElement("col");
    writer.WriteAttributeString("min", ((int) columnInfo.FirstColumn + 1).ToString());
    writer.WriteAttributeString("max", sameColumns.ToString());
    double num1 = (double) columnInfo.ColumnWidth / 256.0;
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

  private int FindSameColumns(WorksheetImpl sheet, int iColumnIndex)
  {
    ColumnInfoRecord[] columnInformation = sheet.ColumnInformation;
    ColumnInfoRecord columnInfoRecord1 = columnInformation[iColumnIndex];
    for (; iColumnIndex < this.m_book.MaxColumnCount; ++iColumnIndex)
    {
      int index = iColumnIndex + 1;
      ColumnInfoRecord columnInfoRecord2 = columnInformation[index];
      if (columnInfoRecord2 == null || (int) columnInfoRecord2.ExtendedFormatIndex != (int) columnInfoRecord1.ExtendedFormatIndex || (int) columnInfoRecord2.ColumnWidth != (int) columnInfoRecord1.ColumnWidth || columnInfoRecord2.IsCollapsed != columnInfoRecord1.IsCollapsed || columnInfoRecord2.IsHidden != columnInfoRecord1.IsHidden || (int) columnInfoRecord2.OutlineLevel != (int) columnInfoRecord1.OutlineLevel)
        break;
    }
    return iColumnIndex;
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

  private string GetAFConditionOperatorName(OfficeFilterCondition filterCondition)
  {
    switch (filterCondition)
    {
      case OfficeFilterCondition.Less:
        return "lessThan";
      case OfficeFilterCondition.Equal:
        return "equal";
      case OfficeFilterCondition.LessOrEqual:
        return "lessThanOrEqual";
      case OfficeFilterCondition.Greater:
        return "greaterThan";
      case OfficeFilterCondition.NotEqual:
        return "notEqual";
      case OfficeFilterCondition.GreaterOrEqual:
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
      case OfficeFilterDataType.FloatingPoint:
        return autoFilterCondition.Double.ToString();
      case OfficeFilterDataType.String:
        return autoFilterCondition.String;
      case OfficeFilterDataType.Boolean:
        return !autoFilterCondition.Boolean ? "0" : "1";
      case OfficeFilterDataType.ErrorCode:
        return FormulaUtil.ErrorCodeToName[(int) autoFilterCondition.ErrorCode];
      default:
        throw new ArgumentOutOfRangeException("dataType");
    }
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
      default:
        throw new ArgumentOutOfRangeException(nameof (typeCF));
    }
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
    if (pageSetup.Order.ToString() != OfficeOrder.DownThenOver.ToString())
      writer.WriteAttributeString("pageOrder", Excel2007Serializator.LowerFirstLetter(pageSetup.Order.ToString()));
    Excel2007Serializator.SerializeAttribute(writer, "orientation", (Enum) pageSetup.Orientation, (Enum) (OfficePageOrientation) 0);
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
    string fullHeaderString = pageSetupBaseImpl.FullHeaderString;
    string fullFooterString = pageSetupBaseImpl.FullFooterString;
    if ((fullHeaderString == null || fullHeaderString.Length <= 0) && (fullFooterString == null || fullFooterString.Length <= 0))
      return;
    writer.WriteStartElement("headerFooter", constants.Namespace);
    Excel2007Serializator.SerializeBool(writer, "scaleWithDoc", pageSetupBaseImpl.HFScaleWithDoc);
    Excel2007Serializator.SerializeBool(writer, "alignWithMargins", pageSetupBaseImpl.AlignHFWithPageMargins);
    Excel2007Serializator.SerializeBool(writer, "differentFirst", pageSetupBaseImpl.DifferentFirstPageHF);
    Excel2007Serializator.SerializeBool(writer, "differentOddEven", pageSetupBaseImpl.DifferentOddAndEvenPagesHF);
    writer.WriteElementString("oddHeader", constants.Namespace, fullHeaderString);
    writer.WriteElementString("oddFooter", constants.Namespace, fullFooterString);
    writer.WriteEndElement();
  }

  private static string PrintCommentsToString(OfficePrintLocation printLocation)
  {
    switch (printLocation)
    {
      case OfficePrintLocation.PrintInPlace:
        return "asDisplayed";
      case OfficePrintLocation.PrintNoComments:
        return "none";
      case OfficePrintLocation.PrintSheetEnd:
        return "atEnd";
      default:
        throw new ArgumentOutOfRangeException(nameof (printLocation));
    }
  }

  private static string PrintErrorsToString(OfficePrintErrors printErrors)
  {
    switch (printErrors)
    {
      case OfficePrintErrors.PrintErrorsDisplayed:
        return "displayed";
      case OfficePrintErrors.PrintErrorsBlank:
        return "blank";
      case OfficePrintErrors.PrintErrorsDash:
        return "dash";
      case OfficePrintErrors.PrintErrorsNA:
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
    OfficeKnownColors tabColor = sheet.TabColor;
    string codeName = sheet.CodeName;
    if (this.m_book.HasMacros && codeName != null && codeName.Length > 0 || sheet.HasCodeName)
      writer.WriteAttributeString("codeName", codeName);
    if (tabColor != ~OfficeKnownColors.Black)
    {
      writer.WriteStartElement("tabColor");
      writer.WriteAttributeString("indexed", ((int) tabColor).ToString());
      writer.WriteEndElement();
    }
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

  [SecurityCritical]
  private void SerilizeBackgroundImage(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    Bitmap backgoundImage = sheet.PageSetup.BackgoundImage;
    if (backgoundImage == null)
      return;
    WorksheetDataHolder dataHolder = sheet.DataHolder;
    FileDataHolder parentHolder = dataHolder.ParentHolder;
    string strExtension;
    string pictureContentType = FileDataHolder.GetPictureContentType(backgoundImage.RawFormat, out strExtension);
    parentHolder.DefaultContentTypes[strExtension] = pictureContentType;
    string str = parentHolder.SaveImage((Image) backgoundImage, (string) null);
    RelationCollection relations = dataHolder.Relations;
    string relationId = relations.GenerateRelationId();
    relations[relationId] = new Relation('/'.ToString() + str, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
    writer.WriteStartElement("picture");
    writer.WriteAttributeString("r", "id", (string) null, relationId);
    writer.WriteEndElement();
  }

  protected virtual void SerializeAppVersion(XmlWriter writer)
  {
    Excel2007Serializator.SerializeElementString(writer, "AppVersion", "12.0000", (string) null);
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

  private void SerializeBookViews(XmlWriter writer, List<Dictionary<string, string>> lstBookViews)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    Dictionary<string, string> dicBookView = lstBookViews == null ? new Dictionary<string, string>() : lstBookViews[0];
    this.ChangeCreateAttributeValue(dicBookView, "activeTab", this.m_book.ActiveSheetIndex);
    this.ChangeCreateAttributeValue(dicBookView, "firstSheet", this.m_book.DisplayedTab);
    if (lstBookViews == null && dicBookView.Count != 0)
    {
      lstBookViews = new List<Dictionary<string, string>>();
      lstBookViews.Add(dicBookView);
    }
    if (lstBookViews == null)
    {
      writer.WriteStartElement("bookViews");
      writer.WriteStartElement("workbookView");
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
      foreach (string preservedExternalLink in this.m_book.PreservedExternalLinks)
      {
        writer.WriteStartElement("externalReference");
        writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", preservedExternalLink);
        writer.WriteEndElement();
      }
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
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
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

  internal void SerializeWorksheets(
    ZipArchive archive,
    IWorkbook workbook,
    ChartImpl chart,
    int workSheetIndex,
    Dictionary<int, int> styleIndex,
    bool defaultExcelFile)
  {
    if (!defaultExcelFile)
    {
      try
      {
        for (int Index = 0; Index < workbook.Worksheets.Count; ++Index)
        {
          WorksheetImpl worksheet = workbook.Worksheets[Index] as WorksheetImpl;
          if (worksheet.IsParsed && !worksheet.ParseDataOnDemand)
          {
            ZipArchiveItem zipArchiveItem = archive[worksheet.ArchiveItemName];
            if (zipArchiveItem != null)
            {
              Dictionary<string, string> extraAttributes = (Dictionary<string, string>) null;
              Stream worksheetStreamData = this.GetWorksheetStreamData(zipArchiveItem.DataStream, extraAttributes);
              MemoryStream memoryStream = new MemoryStream();
              using (XmlWriter writer = XmlWriter.Create((Stream) memoryStream))
                this.serializeSheet(writer, worksheet, (Stream) memoryStream, styleIndex, worksheetStreamData, extraAttributes, true);
              archive.RemoveItem(worksheet.ArchiveItemName);
              archive.AddItem(worksheet.ArchiveItemName, (Stream) memoryStream, true, FileAttributes.Archive);
            }
          }
        }
      }
      catch (Exception ex)
      {
      }
    }
    else
      this.SerializeDefaultExcelWorksheet(archive, workbook, chart, workSheetIndex, styleIndex);
  }

  private Stream GetWorksheetStreamData(Stream stream, Dictionary<string, string> extraAttributes)
  {
    extraAttributes = new Dictionary<string, string>();
    stream.Position = 0L;
    XmlReader reader = UtilityMethods.CreateReader(stream);
    Stream worksheetStreamData = (Stream) null;
    if (reader == null)
      throw new ArgumentNullException("reader");
    while (reader.NodeType != XmlNodeType.Element && reader.LocalName != "worksheet" && !reader.EOF)
      reader.Read();
    if (reader.HasAttributes)
    {
      while (reader.MoveToNextAttribute())
        extraAttributes.Add($"{reader.Prefix}:{reader.LocalName}", reader.Value);
      reader.MoveToElement();
    }
    if (!reader.EOF)
    {
      reader.Read();
      worksheetStreamData = Excel2007Parser.ParseMiscSheetElements(reader);
    }
    return worksheetStreamData;
  }

  internal void SerializeDefaultExcelWorksheet(
    ZipArchive archive,
    IWorkbook workbook,
    ChartImpl chart,
    int workSheetIndex,
    Dictionary<int, int> styleIndex)
  {
    archive.RemoveItem($"xl/worksheets/sheet{(object) (chart.Workbook.ActiveSheetIndex + 1)}.xml");
    MemoryStream memoryStream = new MemoryStream();
    using (XmlWriter writer = XmlWriter.Create((Stream) memoryStream))
    {
      try
      {
        this.serializeSheet(writer, chart.Workbook.Worksheets[workSheetIndex] as WorksheetImpl, (Stream) memoryStream, styleIndex, (Stream) null, (Dictionary<string, string>) null, false);
      }
      catch
      {
        this.serializeSheet(writer, chart.Workbook.Worksheets[0] as WorksheetImpl, (Stream) memoryStream, styleIndex, (Stream) null, (Dictionary<string, string>) null, false);
      }
    }
    if (chart.Workbook.ActiveSheetIndex != -1)
      archive.AddItem($"xl/worksheets/sheet{(object) (chart.Workbook.ActiveSheetIndex + 1)}.xml", (Stream) memoryStream, true, FileAttributes.Archive);
    else
      archive.AddItem("xl/worksheets/sheet1.xml", (Stream) memoryStream, true, FileAttributes.Archive);
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
    OfficeSheetProtection flag,
    bool defaultValue,
    OfficeSheetProtection protection);
}
