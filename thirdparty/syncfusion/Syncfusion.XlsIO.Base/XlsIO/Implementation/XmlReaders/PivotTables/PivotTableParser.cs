// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlReaders.PivotTables.PivotTableParser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Compression;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.PivotTables;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlReaders.PivotTables;

internal class PivotTableParser
{
  private const int DataFieldsIndex = -2;

  public static void ParsePivotTable(XmlReader reader, PivotTableImpl pivotTable)
  {
    PivotTableOptions options = pivotTable.Options as PivotTableOptions;
    WorksheetImpl worksheet = pivotTable.Worksheet;
    if ((worksheet.Workbook as WorkbookImpl).Options == ExcelParseOptions.DoNotParsePivotTable)
    {
      if (reader.MoveToAttribute("cacheId"))
        pivotTable.CacheIndex = XmlConvertExtension.ToInt32(reader.Value);
      reader.MoveToElement();
      worksheet.PreservePivotTables.Add(ShapeParser.ReadNodeAsStream(reader));
    }
    else
    {
      if (reader.MoveToAttribute("name"))
        pivotTable.Name = reader.Value;
      if (reader.MoveToAttribute("cacheId"))
        pivotTable.CacheIndex = XmlConvertExtension.ToInt32(reader.Value);
      if (reader.MoveToAttribute("applyNumberFormats"))
        options.IsNumberAutoFormat = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("applyBorderFormats"))
        options.IsBorderAutoFormat = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("applyFontFormats"))
        options.IsFontAutoFormat = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("applyPatternFormats"))
        options.IsPatternAutoFormat = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("applyAlignmentFormats"))
        options.IsAlignAutoFormat = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("applyWidthHeightFormats"))
        options.IsWHAutoFormat = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("asteriskTotals"))
        options.ShowAsteriskTotals = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("colHeaderCaption"))
        options.ColumnHeaderCaption = reader.Value;
      if (reader.MoveToAttribute("rowHeaderCaption"))
        options.RowHeaderCaption = reader.Value;
      if (reader.MoveToAttribute("createdVersion"))
        options.CreatedVersion = XmlConvertExtension.ToByte(reader.Value);
      if (reader.MoveToAttribute("updatedVersion"))
        options.UpdatedVersion = XmlConvertExtension.ToByte(reader.Value);
      if (reader.MoveToAttribute("minRefreshableVersion"))
        options.MiniRefreshVersion = XmlConvertExtension.ToByte(reader.Value);
      if (reader.MoveToAttribute("customListSort"))
        options.ShowCustomSortList = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("dataCaption"))
        options.DataCaption = reader.Value;
      if (reader.MoveToAttribute("grandTotalCaption"))
        options.GrandTotalCaption = reader.Value;
      if (reader.MoveToAttribute("dataOnRows"))
        pivotTable.ShowDataFieldInRow = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("dataPosition"))
        options.DataPosition = XmlConvertExtension.ToUInt16(reader.Value);
      if (reader.MoveToAttribute("disableFieldList"))
        options.ShowFieldList = !XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("editData"))
        options.IsDataEditable = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("enableDrill"))
        pivotTable.EnableDrilldown = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("enableFieldProperties"))
        options.EnableFieldProperties = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("enableWizard"))
        pivotTable.EnableWizard = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("errorCaption"))
        pivotTable.ErrorString = reader.Value;
      if (reader.MoveToAttribute("showHeaders"))
        options.DisplayFieldCaptions = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("showError"))
        pivotTable.DisplayErrorString = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("missingCaption"))
        pivotTable.NullString = reader.Value;
      if (reader.MoveToAttribute("showMissing "))
        pivotTable.DisplayNullString = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("showCalcMbrs"))
        options.ShowCalcMembers = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("itemPrintTitles"))
        pivotTable.RepeatItemsOnEachPrintedPage = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("indent"))
        options.Indent = XmlConvertExtension.ToUInt32(reader.Value);
      if (reader.MoveToAttribute("rowGrandTotals"))
        pivotTable.RowGrand = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("colGrandTotals"))
        pivotTable.ColumnGrand = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("gridDropZones"))
        options.ShowGridDropZone = XmlConvertExtension.ToBoolean(reader.Value);
      bool? nullable1 = new bool?();
      bool? nullable2 = new bool?();
      if (reader.MoveToAttribute("outline"))
        nullable1 = new bool?(XmlConvertExtension.ToBoolean(reader.Value));
      if (reader.MoveToAttribute("compact"))
        nullable2 = new bool?(XmlConvertExtension.ToBoolean(reader.Value));
      pivotTable.Options.RowLayout = nullable1.HasValue ? (nullable2.HasValue ? PivotTableRowLayout.Outline : PivotTableRowLayout.Compact) : PivotTableRowLayout.Tabular;
      if (reader.MoveToAttribute("pageOverThenDown") && XmlConvertExtension.ToBoolean(reader.Value))
        options.PageFieldsOrder = PivotPageAreaFieldsOrder.OverThenDown;
      if (reader.MoveToAttribute("preserveFormatting"))
        options.PreserveFormatting = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("showDrill"))
        options.ShowDrillIndicators = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("useAutoFormatting"))
        options.IsAutoFormat = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("showDataTips"))
        options.ShowTooltips = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("fieldPrintTitles"))
        options.PrintTitles = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("asteriskTotals"))
        options.ShowAsteriskTotals = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("mergeItem"))
        options.MergeLabels = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("pageWrap"))
        options.PageFieldWrapCount = XmlConvertExtension.ToInt32(reader.Value);
      if (reader.MoveToAttribute("multipleFieldFilters"))
        options.IsMultiFieldFilter = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("fieldListSortAscending"))
        options.IsDefaultAutoSort = XmlConvertExtension.ToBoolean(reader.Value);
      reader.Read();
      if (reader.NodeType == XmlNodeType.Whitespace)
        reader.Read();
      PivotTableParser.ParseLocation(reader, pivotTable);
      PivotTableParser.ParsePivotFields(reader, pivotTable);
      PivotTableParser.ParseRowFields(reader, pivotTable);
      PivotTableParser.ParseRowItems(reader, pivotTable);
      PivotTableParser.ParseColumnFields(reader, pivotTable);
      PivotTableParser.ParseColumnItems(reader, pivotTable);
      PivotTableParser.ParsePageFields(reader, pivotTable);
      PivotTableParser.ParseDataFields(reader, pivotTable);
      PivotTableParser.ParseCustomPivotFormats(reader, pivotTable);
      PivotTableParser.ParseConditionalFormats(reader, pivotTable);
      PivotTableParser.ParseChartFormats(reader, pivotTable);
      PivotTableParser.ParsePivotHierarchies(reader, pivotTable);
      PivotTableParser.ParsePivotStyle(reader, pivotTable);
      PivotTableParser.ParseFilters(reader, pivotTable);
      PivotTableParser.ParseRowHierarchies(reader, pivotTable);
      PivotTableParser.ParseColumnHierarchies(reader, pivotTable);
      PivotTableParser.ParseTableDefinitionExtensionList(reader, pivotTable);
    }
  }

  private static void ParseTableDefinitionExtensionList(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException("writer");
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (reader.LocalName != "extLst")
      return;
    Stream stream = ShapeParser.ReadNodeAsStream(reader);
    pivotTable.PreservedElements.Add("extLst", stream);
  }

  private static void ParseChartFormats(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException("writer");
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (reader.LocalName != "chartFormats")
      return;
    Stream stream = ShapeParser.ReadNodeAsStream(reader);
    pivotTable.PreservedElements.Add("chartFormats", stream);
  }

  private static void ParseCustomPivotFormats(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentException(nameof (reader));
    FileDataHolder dataHolder = pivotTable.Workbook.DataHolder;
    PivotTableParser.ParseCustomPivotFormatsFromExcel2007(reader, pivotTable, dataHolder);
  }

  private static void ParseCustomPivotFormatsFromExcel2007(
    XmlReader reader,
    PivotTableImpl pivotTable,
    FileDataHolder dataHolder)
  {
    if (dataHolder == null)
      return;
    List<DxfImpl> dxfsCollection = dataHolder.ParseDxfsCollection();
    if (dxfsCollection == null)
      return;
    PivotTableParser.ParseCustomFormats(reader, pivotTable, dxfsCollection);
  }

  private static void ParseCustomFormats(
    XmlReader reader,
    PivotTableImpl pivotTable,
    List<DxfImpl> lstDxfs)
  {
    if (reader == null)
      throw new ArgumentNullException("writer");
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (reader.LocalName != "formats")
      return;
    Stream input = ShapeParser.ReadNodeAsStream(reader);
    pivotTable.PivotFormatsStream = input;
    input.Position = 0L;
    reader = XmlReader.Create(input);
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "format")
      {
        PivotFormat pivotFormat1 = new PivotFormat(pivotTable);
        int iDxfIndex = -1;
        DxfImpl dxfImpl = (DxfImpl) null;
        PivotTableParser.ParseCustomFormat(reader, pivotFormat1, out iDxfIndex);
        if (iDxfIndex > -1 && iDxfIndex < lstDxfs.Count)
          dxfImpl = lstDxfs[iDxfIndex];
        if (pivotFormat1.PivotArea.InternalReferences != null && pivotFormat1.PivotArea.InternalReferences.Count <= 1000)
        {
          foreach (List<InternalReference> internalReference1 in pivotFormat1.PivotArea.InternalReferences)
          {
            PivotFormat pivotFormat2 = new PivotFormat(pivotTable);
            pivotFormat2.PivotArea.IsOutline = pivotFormat1.PivotArea.IsOutline;
            pivotFormat2.PivotArea.IsLableOnly = pivotFormat1.PivotArea.IsLableOnly;
            List<PivotAreaReference> references = new List<PivotAreaReference>();
            foreach (InternalReference internalReference2 in internalReference1)
            {
              PivotAreaReference pivotAreaReference = internalReference2.PivotAreaReference.Clone() as PivotAreaReference;
              pivotAreaReference.Indexes.Clear();
              pivotAreaReference.Indexes.Add(internalReference2.Index);
              references.Add(pivotAreaReference);
            }
            pivotFormat2.PivotArea.References.SetReferences(references);
            PivotCellFormat pivotCellFormat = new PivotCellFormat(pivotFormat2);
            pivotFormat2.PivotCellFormat = (IPivotCellFormat) pivotCellFormat;
            int index = pivotTable.PivotFormats.IndexOf(pivotFormat2);
            if (index == -1)
              pivotTable.PivotFormats.Add(pivotFormat2);
            else
              pivotFormat2 = pivotTable.PivotFormats[index];
            dxfImpl?.FillPivotCellFormat(pivotFormat2.PivotCellFormat as IInternalPivotCellFormat);
          }
        }
        else
        {
          int index = pivotTable.PivotFormats.IndexOf(pivotFormat1);
          if (pivotTable.PivotFormats.IndexOf(pivotFormat1) == -1)
            pivotTable.PivotFormats.Add(pivotFormat1);
          else
            pivotFormat1 = pivotTable.PivotFormats[index];
          dxfImpl?.FillPivotCellFormat(pivotFormat1.PivotCellFormat as IInternalPivotCellFormat);
        }
      }
      else
        reader.Read();
    }
    reader.Read();
  }

  private static void ParseCustomFormat(
    XmlReader reader,
    PivotFormat pivotFormat,
    out int iDxfIndex)
  {
    iDxfIndex = -1;
    if (reader == null)
      throw new ArgumentNullException("writer");
    if (pivotFormat == null)
      throw new ArgumentNullException("pivotformat");
    if (reader.LocalName != "format")
      return;
    if (reader.MoveToAttribute("dxfId"))
      iDxfIndex = XmlConvertExtension.ToInt32(reader.Value);
    PivotCellFormat pivotCellFormat = new PivotCellFormat(pivotFormat);
    pivotFormat.PivotCellFormat = (IPivotCellFormat) pivotCellFormat;
    pivotFormat.PivotArea = new PivotArea(pivotFormat.PivotTable);
    PivotCacheParser.ParsePivotArea(reader, pivotFormat.PivotArea);
    if (!string.IsNullOrEmpty(pivotFormat.PivotArea.Offset))
    {
      IRange rangeByString = pivotFormat.PivotTable.Worksheet.GetRangeByString(pivotFormat.PivotArea.Offset, false);
      if (rangeByString != null)
        pivotFormat.PivotArea.Range = rangeByString;
    }
    reader.Read();
  }

  private static void ParseFilters(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException("writer");
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (reader.LocalName != "filters")
      return;
    PivotTableFilters pivotTableFilters = new PivotTableFilters();
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement && reader.LocalName == "filter")
    {
      PivotTableFilter pivotTableFilter = new PivotTableFilter();
      PivotValueLableFilter valueFilter = new PivotValueLableFilter();
      PivotTableParser.ParseFilter(reader, pivotTableFilter, valueFilter);
      if (pivotTableFilter.Value1 != null)
        valueFilter.Value1 = pivotTableFilter.Value1;
      if (pivotTableFilter.Value2 != null)
        valueFilter.Value2 = pivotTableFilter.Value2;
      valueFilter.Type = pivotTableFilter.Type;
      valueFilter.DataField = (IPivotField) pivotTable.Fields[0];
      PivotTableFields fields = pivotTable.Fields;
      foreach (PivotFieldImpl pivotFieldImpl in (CollectionBase<PivotFieldImpl>) fields)
      {
        if (fields.IndexOf(pivotFieldImpl) == pivotTableFilter.Field)
          (pivotFieldImpl.PivotFilters as PivotFilterCollections).ValueFilter = (IPivotValueLableFilter) valueFilter;
      }
      pivotTableFilters.Add(pivotTableFilter);
    }
    pivotTable.Filters = pivotTableFilters;
    reader.Read();
  }

  private static void ParseFilter(
    XmlReader reader,
    PivotTableFilter pivotFilter,
    PivotValueLableFilter valueFilter)
  {
    if (reader == null)
      throw new ArgumentNullException("writer");
    if (pivotFilter == null)
      throw new ArgumentNullException("pivotFilters");
    if (reader.LocalName != "filter")
      return;
    if (reader.MoveToAttribute("description"))
      pivotFilter.DescriptionAttribute = Convert.ToString(reader.Value);
    if (reader.MoveToAttribute("evalOrder"))
      pivotFilter.EvalOrder = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("fld"))
      pivotFilter.Field = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("id"))
      pivotFilter.FilterId = (int) XmlConvertExtension.ToInt16(reader.Value);
    if (reader.MoveToAttribute("iMeasureFld"))
      pivotFilter.MeasureFld = (int) XmlConvertExtension.ToInt16(reader.Value);
    if (reader.MoveToAttribute("iMeasureHier"))
      pivotFilter.MeasureHier = (int) XmlConvertExtension.ToInt16(reader.Value);
    if (reader.MoveToAttribute("stringValue1"))
      pivotFilter.Value1 = Convert.ToString(reader.Value);
    if (reader.MoveToAttribute("stringValue2"))
      pivotFilter.Value2 = Convert.ToString(reader.Value);
    if (reader.MoveToAttribute("type"))
      pivotFilter.Type = !(reader.Value == "nextWeek") ? (PivotFilterType) Enum.Parse(typeof (PivotFilterType2007), reader.Value, true) : PivotFilterType.NexWeek;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "autoFilter")
      {
        PivotAutoFilter pivotAutoFilter = new PivotAutoFilter();
        PivotTableParser.ParseAutoFilter(reader, pivotAutoFilter, valueFilter);
        pivotFilter.Add(pivotAutoFilter);
      }
      else
        reader.Skip();
    }
    reader.Read();
  }

  private static void ParseAutoFilter(
    XmlReader reader,
    PivotAutoFilter autoFilter,
    PivotValueLableFilter valueFilter)
  {
    if (reader == null)
      throw new ArgumentNullException("writer");
    if (autoFilter == null)
      throw new ArgumentNullException("Filters");
    if (reader.MoveToAttribute("ref"))
      autoFilter.FilterRange = Convert.ToString(reader.Value);
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "filterColumn")
      {
        PivotFilterColumn filterColumn = new PivotFilterColumn();
        PivotTableParser.ParseFilterColumn(reader, filterColumn, valueFilter);
        autoFilter.Add(filterColumn);
      }
    }
    reader.Read();
  }

  private static void ParseFilterColumn(
    XmlReader reader,
    PivotFilterColumn filterColumn,
    PivotValueLableFilter valueFilter)
  {
    if (reader == null)
      throw new ArgumentNullException("writer");
    if (filterColumn == null)
      throw new ArgumentNullException(nameof (filterColumn));
    if (reader.MoveToAttribute("colId"))
      filterColumn.ColumnId = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("hiddenButton"))
      filterColumn.HiddenButton = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("showButton"))
      filterColumn.ShowButton = XmlConvertExtension.ToBoolean(reader.Value);
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      switch (reader.LocalName)
      {
        case "customFilters":
          PivotCustomFilters customFilters = new PivotCustomFilters();
          PivotTableParser.ParseCustomFilters(reader, customFilters, valueFilter);
          filterColumn.CustomFilters = customFilters;
          continue;
        case "filters":
          FilterColumnFilters columnFilters = new FilterColumnFilters();
          PivotTableParser.ParseFilterColumnFilters(reader, columnFilters, valueFilter);
          filterColumn.FilterColumnFilter = columnFilters;
          continue;
        case "top10":
          PivotTop10Filter top10Filter = new PivotTop10Filter();
          PivotTableParser.ParseTop10Filter(reader, top10Filter, valueFilter);
          filterColumn.Top10Filters = top10Filter;
          continue;
        case "dynamicFilter":
          PivotDynamicFilter dynamicFilter = new PivotDynamicFilter();
          PivotTableParser.ParseDynamicFilter(reader, dynamicFilter, valueFilter);
          if (dynamicFilter.DateFilterType != DynamicFilterType.None)
          {
            filterColumn.DynamicFilter = dynamicFilter;
            continue;
          }
          continue;
        default:
          reader.Read();
          continue;
      }
    }
    reader.Read();
  }

  private static void ParseCustomFilters(
    XmlReader reader,
    PivotCustomFilters customFilters,
    PivotValueLableFilter valueFilter)
  {
    if (reader == null)
      throw new ArgumentNullException("writer");
    if (customFilters == null)
      throw new ArgumentNullException(nameof (customFilters));
    if (reader.MoveToAttribute("and"))
      customFilters.HasAnd = XmlConvertExtension.ToBoolean(reader.Value);
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      PivotCustomFilter customFilter = new PivotCustomFilter();
      PivotTableParser.ParseCustomFilter(reader, customFilter, valueFilter);
      customFilters.Add(customFilter);
    }
    reader.Read();
  }

  private static void ParseCustomFilter(
    XmlReader reader,
    PivotCustomFilter customFilter,
    PivotValueLableFilter valueFilter)
  {
    if (reader == null)
      throw new ArgumentNullException("writer");
    if (customFilter == null)
      throw new ArgumentNullException("customFilters");
    if (reader.MoveToAttribute("operator"))
      customFilter.FilterOperator = (FilterOperator2007) Enum.Parse(typeof (FilterOperator), reader.Value, true);
    if (reader.MoveToAttribute("val"))
      customFilter.Value = reader.Value;
    string str = customFilter.Value;
    if (str.Contains("*"))
    {
      char[] separator = new char[1]{ '*' };
      str = str.Split(separator, StringSplitOptions.RemoveEmptyEntries)[0];
    }
    if (valueFilter.Value1 == null)
      valueFilter.Value1 = str;
    else
      valueFilter.Value2 = str;
    reader.Read();
  }

  private static void ParseFilterColumnFilters(
    XmlReader reader,
    FilterColumnFilters columnFilters,
    PivotValueLableFilter valueFilter)
  {
    if (reader == null)
      throw new ArgumentNullException("writer");
    if (columnFilters == null)
      throw new ArgumentNullException("filters");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "filter")
      {
        if (reader.MoveToAttribute("val"))
          columnFilters.Add(reader.Value);
        if (valueFilter.Value1 == null)
          valueFilter.Value1 = reader.Value;
        reader.Read();
      }
    }
    reader.Read();
  }

  private static void ParseTop10Filter(
    XmlReader reader,
    PivotTop10Filter top10Filter,
    PivotValueLableFilter valueFilter)
  {
    if (reader == null)
      throw new ArgumentNullException("writer");
    if (top10Filter == null)
      throw new ArgumentNullException(nameof (top10Filter));
    if (reader.MoveToAttribute("val"))
      top10Filter.FilterValue = (double) XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("filterVal"))
      top10Filter.Value = (double) XmlConvertExtension.ToInt32(reader.Value);
    valueFilter.Value1 = top10Filter.Value.ToString();
    if (reader.MoveToAttribute("percent"))
      top10Filter.IsPercent = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("top"))
      top10Filter.IsTop = XmlConvertExtension.ToBoolean(reader.Value);
    reader.Read();
  }

  private static void ParseDynamicFilter(
    XmlReader reader,
    PivotDynamicFilter dynamicFilter,
    PivotValueLableFilter valueFilter)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dynamicFilter == null)
      throw new ArgumentNullException(nameof (dynamicFilter));
    if (reader.MoveToAttribute("type"))
      dynamicFilter.DateFilterType = AF.ConvertToDateFilterType(reader.Value);
    if (!reader.MoveToAttribute("val"))
      return;
    valueFilter.Value1 = reader.Value;
  }

  private static void ParseLocation(XmlReader reader, PivotTableImpl pivotTable)
  {
    string strFormula = (string) null;
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (reader.MoveToAttribute("ref"))
      strFormula = reader.Value;
    if (strFormula == null)
      throw new Exception("Reference");
    if (reader.MoveToAttribute("colPageCount"))
      pivotTable.ColumnsPerPage = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("rowPageCount"))
      pivotTable.RowsPerPage = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("firstHeaderRow"))
      pivotTable.FirstHeaderRow = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("firstDataRow"))
      pivotTable.FirstDataRow = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("firstDataCol"))
      pivotTable.FirstDataCol = XmlConvertExtension.ToInt32(reader.Value);
    WorkbookImpl workbook = pivotTable.Workbook;
    IRangeGetter rangeGetter = workbook.DataHolder.Parser.FormulaUtil.ParseString(strFormula)[0] as IRangeGetter;
    IWorksheet worksheet = (IWorksheet) pivotTable.Worksheet;
    pivotTable.Location = rangeGetter.GetRange((IWorkbook) workbook, worksheet);
    reader.Read();
  }

  private static void ParsePivotFields(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException("writer");
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (reader.LocalName != "pivotFields")
      return;
    PivotTableFields internalFields = pivotTable.InternalFields;
    PivotCacheFieldsCollection cacheFields = pivotTable.Cache.CacheFields;
    reader.Read();
    int i = 0;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "pivotField")
      {
        PivotFieldImpl pivotField = internalFields[i];
        PivotTableParser.ParsePivotField(reader, pivotField, pivotTable);
        ++i;
      }
    }
    reader.Read();
  }

  private static void ParsePivotField(
    XmlReader reader,
    PivotFieldImpl pivotField,
    PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException("writer");
    if (pivotField == null)
      throw new ArgumentException("pivot Field");
    if (reader.MoveToAttribute("name"))
      pivotField.Name = reader.Value;
    if (reader.MoveToAttribute("subtotalCaption"))
      pivotField.SubTotalName = reader.Value;
    if (reader.MoveToAttribute("autoShow"))
      pivotField.IsAutoShow = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("axis"))
      pivotField.Axis = (PivotAxisTypes) Enum.Parse(typeof (PivotAxisTypes2007), reader.Value, false);
    pivotField.Subtotals = PivotTableParser.ParseSubtotalFlags(reader);
    if (reader.MoveToAttribute("compact"))
      pivotField.Compact = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("dragOff"))
      pivotField.CanDragOff = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("dragToCol"))
      pivotField.CanDragToColumn = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("dragToData"))
      pivotField.CanDragToData = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("dragToPage"))
      pivotField.CanDragToPage = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("dragToRow"))
      pivotField.CanDragToRow = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("hideNewItems"))
      pivotField.ShowNewItemsOnRefresh = !XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("includeNewItemsInFilter"))
      pivotField.ShowNewItemsInFilter = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("insertBlankRow"))
      pivotField.ShowBlankRow = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("insertPageBreak"))
      pivotField.ShowPageBreak = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("itemPageCount"))
      pivotField.ItemsPerPage = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("measureFilter"))
      pivotField.IsMeasureField = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("multipleItemSelectionAllowed"))
      pivotField.IsMultiSelected = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("outline"))
      pivotField.ShowOutline = XmlConvertExtension.ToBoolean(reader.Value);
    pivotField.IsShowAllItems = !reader.MoveToAttribute("showAll") || XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("showDropDowns"))
      pivotField.ShowDropDown = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("showPropAsCaption"))
      pivotField.ShowPropAsCaption = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("showPropTip"))
      pivotField.ShowToolTip = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("sortType"))
      pivotField.SortType = new PivotFieldSortType?((PivotFieldSortType) Enum.Parse(typeof (PivotFieldSortType), reader.Value, true));
    if (reader.MoveToAttribute("uniqueMemberProperty"))
      pivotField.Caption = reader.Value;
    if (reader.MoveToAttribute("dataField"))
      pivotField.IsDataField = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("numFmtId"))
      pivotField.NumberFormatIndex = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("dataSourceSort"))
      pivotField.IsDataSourceSorted = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("allDrilled"))
      pivotField.IsAllDrilled = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("defaultAttributeDrillState"))
      pivotField.IsDefaultDrill = XmlConvertExtension.ToBoolean(reader.Value);
    reader.Read();
    bool flag1 = false;
    bool fieldItems = PivotTableParser.ParseFieldItems(reader, pivotField);
    if (pivotField.ItemOptions.Count > 0 && !pivotField.ItemOptionSorted)
      pivotField.PreSort();
    bool flag2 = fieldItems | PivotTableParser.ParseAutoSortScope(reader, pivotField);
    if (reader.LocalName == "extLst")
    {
      PivotTableParser.ParsePivotFieldExtensionList(reader, pivotField);
      flag1 = true;
    }
    if (!flag2 && !flag1)
      return;
    reader.Read();
  }

  private static void ParsePivotFieldExtensionList(XmlReader reader, PivotFieldImpl pivotField)
  {
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "ext":
            PivotTableParser.ParsePivotFieldExtension(reader, pivotField);
            continue;
          default:
            continue;
        }
      }
    }
    reader.Read();
  }

  private static void ParsePivotFieldExtension(XmlReader reader, PivotFieldImpl pivotField)
  {
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case nameof (pivotField):
            PivotTableParser.ParsePivotFieldExtensionAttributes(reader, pivotField);
            continue;
          default:
            continue;
        }
      }
    }
    reader.Read();
  }

  private static void ParsePivotFieldExtensionAttributes(
    XmlReader reader,
    PivotFieldImpl pivotField)
  {
    if (reader.IsEmptyElement)
    {
      if (reader.MoveToAttribute("fillDownLabels"))
        pivotField.RepeatLabels = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("ignore"))
        pivotField.Ignore = XmlConvertExtension.ToBoolean(reader.Value);
    }
    reader.Read();
  }

  private static bool ParseAutoSortScope(XmlReader reader, PivotFieldImpl pivotField)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "autoSortScope")
      return false;
    pivotField.PivotArea = pivotField != null ? new PivotArea(pivotField.CacheField) : throw new ArgumentException("pivot Field");
    PivotCacheParser.ParsePivotArea(reader, pivotField.PivotArea);
    reader.Read();
    return true;
  }

  private static bool ParseFieldItems(XmlReader reader, PivotFieldImpl field)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "items")
      return false;
    if (field == null)
      throw new ArgumentException("pivot Field");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "item")
        PivotTableParser.ParseFieldItem(reader, field);
      reader.Read();
    }
    reader.Read();
    return true;
  }

  private static void ParseFieldItem(XmlReader reader, PivotFieldImpl field)
  {
    if (reader == null)
      throw new ArgumentNullException("writer");
    if (field == null)
      throw new ArgumentException("pivot Field");
    int num = 0;
    bool flag = false;
    PivotItemOptions pivotItemOptions = new PivotItemOptions();
    if (reader.MoveToAttribute("x"))
      num = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("d"))
    {
      flag = true;
      pivotItemOptions.IsExpaned = XmlConvertExtension.ToBoolean(reader.Value);
    }
    if (reader.MoveToAttribute("e"))
    {
      flag = true;
      pivotItemOptions.DrillAcross = XmlConvertExtension.ToBoolean(reader.Value);
    }
    if (reader.MoveToAttribute("f"))
    {
      flag = true;
      pivotItemOptions.IsCalculatedItem = XmlConvertExtension.ToBoolean(reader.Value);
    }
    if (reader.MoveToAttribute("h"))
    {
      flag = true;
      pivotItemOptions.IsHidden = XmlConvertExtension.ToBoolean(reader.Value);
    }
    if (reader.MoveToAttribute("m"))
    {
      flag = true;
      pivotItemOptions.IsMissing = XmlConvertExtension.ToBoolean(reader.Value);
    }
    if (reader.MoveToAttribute("n"))
    {
      flag = true;
      pivotItemOptions.UserCaption = reader.Value;
    }
    if (reader.MoveToAttribute("s"))
    {
      flag = true;
      pivotItemOptions.IsChar = XmlConvertExtension.ToBoolean(reader.Value);
    }
    if (reader.MoveToAttribute("sd"))
    {
      flag = true;
      pivotItemOptions.IsHiddenDetails = XmlConvertExtension.ToBoolean(reader.Value);
    }
    if (reader.MoveToAttribute("m"))
    {
      flag = true;
      pivotItemOptions.IsMissing = XmlConvertExtension.ToBoolean(reader.Value);
    }
    if (reader.MoveToAttribute("t"))
    {
      num = -1;
      flag = true;
      string str = reader.Value;
      if (str == "default")
        str += "s";
      pivotItemOptions.ItemType = (PivotItemType) Enum.Parse(typeof (PivotItemType2007), str, false);
    }
    if (field.ItemOptions.ContainsKey(num))
      return;
    if (flag)
      field.AddItemOption(num, pivotItemOptions);
    else
      field.AddItemOption(num);
  }

  public static PivotSubtotalTypes ParseSubtotalFlags(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    PivotSubtotalTypes subtotalFlags = PivotSubtotalTypes.Default;
    if (reader.MoveToAttribute("defaultSubtotal") && !XmlConvertExtension.ToBoolean(reader.Value))
      subtotalFlags = PivotSubtotalTypes.None;
    if (reader.MoveToAttribute("sumSubtotal"))
      subtotalFlags |= PivotTableParser.GetSubTotalTypes(PivotSubtotalTypes.Sum, reader.Value);
    if (reader.MoveToAttribute("countASubtotal"))
      subtotalFlags |= PivotTableParser.GetSubTotalTypes(PivotSubtotalTypes.Counta, reader.Value);
    if (reader.MoveToAttribute("avgSubtotal"))
      subtotalFlags |= PivotTableParser.GetSubTotalTypes(PivotSubtotalTypes.Average, reader.Value);
    if (reader.MoveToAttribute("maxSubtotal"))
      subtotalFlags |= PivotTableParser.GetSubTotalTypes(PivotSubtotalTypes.Max, reader.Value);
    if (reader.MoveToAttribute("minSubtotal"))
      subtotalFlags |= PivotTableParser.GetSubTotalTypes(PivotSubtotalTypes.Min, reader.Value);
    if (reader.MoveToAttribute("productSubtotal"))
      subtotalFlags |= PivotTableParser.GetSubTotalTypes(PivotSubtotalTypes.Product, reader.Value);
    if (reader.MoveToAttribute("countSubtotal"))
      subtotalFlags |= PivotTableParser.GetSubTotalTypes(PivotSubtotalTypes.Count, reader.Value);
    if (reader.MoveToAttribute("stdDevSubtotal"))
      subtotalFlags |= PivotTableParser.GetSubTotalTypes(PivotSubtotalTypes.Stdev, reader.Value);
    if (reader.MoveToAttribute("stdDevPSubtotal"))
      subtotalFlags |= PivotTableParser.GetSubTotalTypes(PivotSubtotalTypes.Stdevp, reader.Value);
    if (reader.MoveToAttribute("varSubtotal"))
      subtotalFlags |= PivotTableParser.GetSubTotalTypes(PivotSubtotalTypes.Var, reader.Value);
    if (reader.MoveToAttribute("varPSubtotal"))
      subtotalFlags |= PivotTableParser.GetSubTotalTypes(PivotSubtotalTypes.Varp, reader.Value);
    return subtotalFlags;
  }

  private static void ParseRowFields(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (reader.LocalName != "rowFields")
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "field")
      {
        int i = -1;
        if (reader.MoveToAttribute("x"))
          i = XmlConvertExtension.ToInt32(reader.Value);
        if (i == -2)
        {
          pivotTable.ShowDataFieldInRow = true;
        }
        else
        {
          pivotTable.PivotRowFields.Add((IPivotField) pivotTable.Fields[i]);
          pivotTable.RowFieldsOrder.Add(i);
        }
        reader.Read();
      }
    }
    reader.Read();
  }

  private static void ParseRowItems(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (reader.LocalName != "rowItems")
      return;
    pivotTable.RowItemsStream = ShapeParser.ReadNodeAsStream(reader);
  }

  private static void ParseColumnFields(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (reader.LocalName != "colFields")
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "field" && reader.MoveToAttribute("x"))
        pivotTable.ColFieldsOrder.Add((int) Convert.ToInt16(reader.Value));
      reader.Read();
    }
    reader.Read();
  }

  private static void ParseColumnItems(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (reader.LocalName != "colItems")
      return;
    pivotTable.ColumnItemsStream = ShapeParser.ReadNodeAsStream(reader);
  }

  private static void ParsePageFields(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (reader.LocalName != "pageFields")
      return;
    PivotTableFields fields = pivotTable.Fields;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "pageField")
        PivotTableParser.ParsePageField(reader, pivotTable);
      reader.Read();
    }
    reader.Read();
  }

  private static void ParsePageField(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    PivotTableFields pivotTableFields = pivotTable != null ? pivotTable.Fields : throw new ArgumentNullException(nameof (pivotTable));
    int i = 0;
    if (reader.MoveToAttribute("fld"))
      i = XmlConvertExtension.ToInt32(reader.Value);
    PivotFieldImpl pivotFieldImpl = pivotTableFields[i];
    if (reader.MoveToAttribute("hier"))
      pivotFieldImpl.PageFieldHierarchyIndex = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("name"))
      pivotFieldImpl.PageFieldName = reader.Value;
    if (reader.MoveToAttribute("cap"))
      pivotFieldImpl.PageFieldCaption = reader.Value;
    if (reader.MoveToAttribute("item"))
      pivotFieldImpl.ItemIndex = XmlConvertExtension.ToInt32(reader.Value);
    pivotTable.PivotPageFields.Add((IPivotField) pivotTableFields[i]);
  }

  private static void ParseDataFields(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (reader.LocalName != "dataFields")
      return;
    PivotDataFields dataFields = pivotTable.DataFields;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "dataField")
      {
        PivotDataField dataField = PivotTableParser.ParseDataField(reader, pivotTable);
        dataFields.Add(dataField);
      }
      reader.Read();
    }
    reader.Read();
  }

  private static PivotDataField ParseDataField(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    bool isEmptyElement = reader.IsEmptyElement;
    PivotCacheFieldsCollection cacheFields = pivotTable.Cache.CacheFields;
    string name = (string) null;
    PivotSubtotalTypes subtotal = PivotSubtotalTypes.Default;
    int i = 0;
    if (reader.MoveToAttribute("fld"))
      i = XmlConvertExtension.ToInt32(reader.Value);
    PivotFieldImpl parentField = new PivotFieldImpl(cacheFields[i], pivotTable);
    if (reader.MoveToAttribute("name"))
      name = reader.Value;
    if (reader.MoveToAttribute("subtotal"))
      subtotal = (PivotSubtotalTypes) Enum.Parse(typeof (PivotSubtotalTypes2007), reader.Value, false);
    PivotDataField dataField = new PivotDataField(name, subtotal, parentField);
    if (reader.MoveToAttribute("numFmtId"))
    {
      int int16 = (int) XmlConvertExtension.ToInt16(reader.Value);
      Dictionary<int, int> numberFormatIndexes = pivotTable.Workbook.ArrNewNumberFormatIndexes;
      dataField.NumberFormatIndex = numberFormatIndexes == null || !numberFormatIndexes.ContainsKey(int16) ? (int) (ushort) int16 : (int) (ushort) numberFormatIndexes[int16];
    }
    if (reader.MoveToAttribute("showDataAs"))
      dataField.ShowDataAs = dataField.SetShowData(reader.Value);
    if (reader.MoveToAttribute("baseField"))
    {
      int int32 = XmlConvertExtension.ToInt32(reader.Value);
      if (int32 != -1)
        dataField.BaseField = int32;
    }
    if (reader.MoveToAttribute("baseItem"))
      dataField.BaseItem = XmlConvertExtension.ToInt32(reader.Value);
    if (!isEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "extLst":
              PivotTableParser.ParseExtensionListCollection(reader, dataField);
              continue;
            default:
              continue;
          }
        }
      }
      reader.Read();
    }
    return dataField;
  }

  private static void ParseExtensionListCollection(XmlReader reader, PivotDataField dataField)
  {
    if (reader.LocalName != "extLst")
      throw new ArgumentException(nameof (reader));
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "ext":
            PivotTableParser.ParseExtensionList(reader, dataField);
            continue;
          default:
            continue;
        }
      }
    }
    reader.Read();
  }

  private static void ParseExtensionList(XmlReader reader, PivotDataField dataField)
  {
    if (reader.LocalName != "ext")
      throw new ArgumentException(nameof (reader));
    reader.Read();
    if (!reader.IsEmptyElement)
      return;
    reader.MoveToAttribute("pivotShowAs");
    dataField.ShowDataAs = dataField.SetShowData(reader.Value);
    reader.Read();
  }

  private static void ParseConditionalFormats(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (reader.LocalName != "conditionalFormats")
      return;
    Stream stream = ShapeParser.ReadNodeAsStream(reader);
    pivotTable.PreservedElements.Add("conditionalFormats", stream);
  }

  private static void ParsePivotHierarchies(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (reader.LocalName != "pivotHierarchies" || pivotTable.PreservedElements == null)
      return;
    reader.MoveToElement();
    Stream stream = ShapeParser.ReadNodeAsStream(reader);
    pivotTable.PreservedElements.Add("pivotHierarchies", stream);
  }

  private static void ParsePivotStyle(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (reader.MoveToAttribute("name"))
    {
      string str = reader.Value;
      pivotTable.BuiltInStyle = new PivotBuiltInStyles?();
      foreach (PivotBuiltInStyles pivotBuiltInStyles in Enum.GetValues(typeof (PivotBuiltInStyles)))
      {
        if (pivotBuiltInStyles.ToString().Equals(str))
        {
          pivotTable.BuiltInStyle = new PivotBuiltInStyles?((PivotBuiltInStyles) Enum.Parse(typeof (PivotBuiltInStyles), str, false));
          break;
        }
      }
      if (!pivotTable.BuiltInStyle.HasValue)
        pivotTable.CustomStyleName = reader.Value;
    }
    if (reader.MoveToAttribute("showRowHeaders"))
      pivotTable.ShowRowHeaderStyle = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("showColHeaders"))
      pivotTable.ShowColHeaderStyle = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("showRowStripes"))
      pivotTable.ShowRowStripes = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("showColStripes"))
      pivotTable.ShowColStripes = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("showLastColumn"))
      pivotTable.ShowLastCol = XmlConvertExtension.ToBoolean(reader.Value);
    reader.Read();
  }

  private static void ParseRowHierarchies(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (reader.LocalName != "rowHierarchiesUsage" || pivotTable.PreservedElements == null)
      return;
    reader.MoveToElement();
    Stream stream = ShapeParser.ReadNodeAsStream(reader);
    pivotTable.PreservedElements.Add("rowHierarchiesUsage", stream);
  }

  private static void ParseColumnHierarchies(XmlReader reader, PivotTableImpl pivotTable)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (reader.LocalName != "colHierarchiesUsage" || pivotTable.PreservedElements == null)
      return;
    reader.MoveToElement();
    Stream stream = ShapeParser.ReadNodeAsStream(reader);
    pivotTable.PreservedElements.Add("colHierarchiesUsage", stream);
  }

  public static PivotSubtotalTypes GetSubTotalTypes(
    PivotSubtotalTypes subtotalTypes,
    string isEnabled)
  {
    return XmlConvertExtension.ToBoolean(isEnabled) ? subtotalTypes : PivotSubtotalTypes.None;
  }

  private class ComparisonPair : IComparable
  {
    public object Value;
    public int Index;
    public IComparer Comparer = (IComparer) new PivotTableParser.ComparisonPair.GeneralComparer();

    public int CompareTo(object obj)
    {
      PivotTableParser.ComparisonPair comparisonPair = obj as PivotTableParser.ComparisonPair;
      int num = 1;
      if (comparisonPair != null)
      {
        num = this.Comparer.Compare(this.Value, comparisonPair.Value);
        if (num == 0)
          num = this.Comparer.Compare((object) this.Index, (object) comparisonPair.Index);
      }
      return num;
    }

    private class GeneralComparer : IComparer
    {
      private IComparer m_comparer = (IComparer) Comparer<object>.Default;

      public int Compare(object x, object y)
      {
        try
        {
          return this.m_comparer.Compare(x, y);
        }
        catch
        {
          return x.GetHashCode() - y.GetHashCode();
        }
      }
    }
  }
}
