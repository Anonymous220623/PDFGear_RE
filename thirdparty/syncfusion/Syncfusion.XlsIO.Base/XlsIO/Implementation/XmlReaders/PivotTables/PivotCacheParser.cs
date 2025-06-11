// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlReaders.PivotTables.PivotCacheParser
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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlReaders.PivotTables;

internal class PivotCacheParser
{
  private const RegexOptions DEF_REGEX = RegexOptions.Compiled;

  public static void ParsePivotCacheDefinition(
    XmlReader reader,
    PivotCacheImpl cache,
    IWorkbook book,
    string path,
    RelationCollection relations,
    out string cacheRecordRelationID)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (cache == null)
      throw new ArgumentNullException("pivot cache");
    if (book == null)
      throw new ArgumentNullException("workbook");
    if (reader.LocalName != "pivotCacheDefinition")
    {
      XmlException xmlException = new XmlException("Unexpected xml tag.");
    }
    cacheRecordRelationID = (string) null;
    WorkbookImpl bookImpl = book as WorkbookImpl;
    if (bookImpl.Options == ExcelParseOptions.DoNotParsePivotTable)
    {
      bookImpl.PreservesPivotCache.Add(ShapeParser.ReadNodeAsStream(reader));
    }
    else
    {
      if (reader.MoveToAttribute("id", (book as WorkbookImpl).IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      {
        string str = reader.Value;
        cache.RelationId = reader.Value;
        cacheRecordRelationID = reader.Value;
      }
      cache.IsBackgroundQuery = PivotCacheParser.ParseBoolAttribute(reader, "backgroundQuery");
      cache.CreatedVersion = PivotCacheParser.ParseIntAttribute(reader, "createdVersion");
      cache.EnableRefresh = PivotCacheParser.ParseBoolAttribute(reader, "enableRefresh");
      cache.IsRefreshOnLoad = PivotCacheParser.ParseBoolAttribute(reader, "refreshOnLoad");
      cache.IsInvalidData = PivotCacheParser.ParseBoolAttribute(reader, "invalid");
      cache.MinRefreshableVersion = PivotCacheParser.ParseIntAttribute(reader, "minRefreshableVersion");
      cache.IsOptimizedCache = PivotCacheParser.ParseBoolAttribute(reader, "optimizeMemory");
      cache.MissingItemsLimit = reader.MoveToAttribute("missingItemsLimit") ? XmlConvertExtension.ToDouble(reader.Value) : double.MinValue;
      if (reader.MoveToAttribute("refreshedBy"))
        cache.RefreshedBy = reader.Value;
      if (reader.MoveToAttribute("refreshedDate"))
      {
        double result;
        cache.RefreshDate = !double.TryParse(reader.Value, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? Convert.ToDateTime(reader.Value) : DateTime.FromOADate(result);
      }
      cache.RefreshedVersion = PivotCacheParser.ParseIntAttribute(reader, "refreshedVersion");
      cache.IsRefreshOnLoad = PivotCacheParser.ParseBoolAttribute(reader, "refreshOnLoad");
      cache.IsSaveData = PivotCacheParser.ParseBoolAttribute(reader, "saveData");
      cache.SupportAdvancedDrill = PivotCacheParser.ParseBoolAttribute(reader, "supportAdvancedDrill");
      cache.IsSupportSubQuery = PivotCacheParser.ParseBoolAttribute(reader, "supportSubquery");
      cache.IsUpgradeOnRefresh = PivotCacheParser.ParseBoolAttribute(reader, "upgradeOnRefresh");
      cache.TupleCache = PivotCacheParser.ParseBoolAttribute(reader, "tupleCache");
      reader.Read();
      PivotCacheParser.ParseCacheSource(reader, cache, book, relations);
      PivotCacheParser.ParseCacheFields(reader, cache, bookImpl);
      PivotCacheParser.ParseCalculatedItems(reader, cache.CacheFields);
      PivotCacheParser.ParseCacheHierarchies(reader, cache);
      PivotCacheParser.ParseOLAPKPIs(reader, cache);
      PivotCacheParser.ParseOLAPDimensions(reader, cache);
      PivotCacheParser.ParseOLAPMeasureGroups(reader, cache);
      PivotCacheParser.ParseOLAPMaps(reader, cache);
      PivotCacheParser.ParseCacheExtension(reader, cache);
    }
  }

  private static XmlReader CreateCacheRecordReader(
    string relationID,
    string path,
    WorkbookImpl book,
    RelationCollection relations,
    out string itemName)
  {
    if (relationID == null)
      throw new ArgumentNullException("relationId");
    if (book == null)
      throw new ArgumentNullException("workbook");
    Relation relation = relations != null ? relations[relationID] : throw new ArgumentNullException(nameof (relations));
    string target = relation.Target;
    return book.DataHolder.CreateReader(relation, path, out itemName);
  }

  private static void ParseCacheSource(
    XmlReader reader,
    PivotCacheImpl cache,
    IWorkbook book,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (cache == null)
      throw new ArgumentNullException("pivot cache");
    if (book == null)
      throw new ArgumentNullException("workbook");
    Excel2007Parser.SkipWhiteSpaces(reader);
    if (reader.LocalName != "cacheSource")
    {
      XmlException xmlException = new XmlException("Unexpected xml tag.");
    }
    if (reader.MoveToAttribute("type"))
      cache.SourceType = PivotCacheParser.GetSourceType(reader.Value);
    switch (cache.SourceType)
    {
      case ExcelDataSourceType.Worksheet:
        while (reader.NodeType != XmlNodeType.EndElement)
        {
          if (reader.NodeType == XmlNodeType.Element)
          {
            if (reader.LocalName == "worksheetSource")
              PivotCacheParser.ParseWorksheetSource(reader, cache, book, relations);
          }
          else
            reader.Read();
        }
        break;
      case ExcelDataSourceType.ExternalData:
        PivotCacheParser.ParseExternalSource(reader, cache, book, relations);
        break;
      case ExcelDataSourceType.Consolidation:
        PivotCacheParser.ParseConsolidation(reader, cache, book, relations);
        break;
      case ExcelDataSourceType.ScenarioPivotTable:
        reader.Read();
        break;
    }
    reader.MoveToElement();
    if (!(reader.LocalName == "cacheSource"))
      return;
    reader.Read();
  }

  private static void ParseConsolidation(
    XmlReader reader,
    PivotCacheImpl cache,
    IWorkbook book,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (cache == null)
      throw new ArgumentNullException("pivot cache");
    if (book == null)
      throw new ArgumentNullException("workbook");
    if (cache.PreservedElements == null)
      return;
    reader.MoveToElement();
    Stream stream = ShapeParser.ReadNodeAsStream(reader);
    cache.PreservedElements.Add("consolidation", stream);
  }

  private static void ParseWorksheetSource(
    XmlReader reader,
    PivotCacheImpl cache,
    IWorkbook book,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (cache == null)
      throw new ArgumentNullException("pivot cache");
    if (book == null)
      throw new ArgumentNullException("workbook");
    if (reader.LocalName != "worksheetSource")
    {
      XmlException xmlException = new XmlException("Unexpected xml tag.");
    }
    WorkbookImpl workbookImpl = book as WorkbookImpl;
    IRange range = (IRange) null;
    string id = (string) null;
    string str1 = (string) null;
    string str2 = (string) null;
    string sheetName = (string) null;
    IWorksheet sheet = (IWorksheet) null;
    if (reader.MoveToAttribute("id", (book as WorkbookImpl).IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      id = reader.Value;
    if (reader.MoveToAttribute("ref"))
      str1 = reader.Value;
    if (reader.MoveToAttribute("name"))
      str2 = reader.Value;
    if (reader.MoveToAttribute("sheet"))
      sheetName = reader.Value;
    bool flag1 = id != null;
    bool flag2 = str2 != null;
    if (sheetName != null && book.Worksheets[sheetName] != null)
      sheet = book.Worksheets[sheetName];
    if (flag1)
    {
      string target = relations[id].Target;
      string fullFileName = (book as WorkbookImpl).FullFileName;
      if (!string.IsNullOrEmpty(fullFileName) && Path.GetFileName(fullFileName).Equals(target, StringComparison.OrdinalIgnoreCase))
        flag1 = false;
    }
    if (!flag1)
    {
      if (flag2)
      {
        IName name = book.Names[str2];
        if (name != null)
        {
          range = name.RefersToRange;
        }
        else
        {
          for (int Index = 0; Index < book.Worksheets.Count; ++Index)
          {
            for (int index = 0; index < book.Worksheets[Index].ListObjects.Count; ++index)
            {
              IListObject listObject = book.Worksheets[Index].ListObjects[index];
              if (listObject.Name.ToString() == str2)
              {
                range = listObject.Location;
              }
              else
              {
                IName tableNamedRange = PivotCacheParser.GetTableNamedRange(str2, listObject, book as WorkbookImpl);
                if (tableNamedRange != null)
                  range = tableNamedRange.RefersToRange;
              }
            }
          }
        }
        cache.RangeName = str2;
      }
      else
      {
        IRangeGetter rangeGetter = workbookImpl.DataHolder.Parser.FormulaUtil.ParseString(str1)[0] as IRangeGetter;
        sheet = book.Worksheets[sheetName];
        if (sheet != null)
        {
          range = rangeGetter.GetRange(book, sheet);
        }
        else
        {
          reader.MoveToElement();
          Stream stream = ShapeParser.ReadNodeAsStream(reader);
          cache.PreservedElements.Add("worksheetSource", stream);
        }
      }
    }
    else
    {
      cache.RelationId = id;
      cache.PreservedExtenalRelation = relations[id];
      cache.RangeName = str2;
      reader.MoveToElement();
      Stream stream = ShapeParser.ReadNodeAsStream(reader);
      cache.PreservedElements.Add("worksheetSource", stream);
    }
    cache.SourceRange = range != null || str1 == null || sheet == null ? range : sheet.Range[str1];
    if (reader.NodeType == XmlNodeType.EndElement)
      return;
    reader.Read();
  }

  private static IName GetTableNamedRange(
    string tableNamedRange,
    IListObject listObject,
    WorkbookImpl book)
  {
    if (tableNamedRange.StartsWith(listObject.Name))
    {
      foreach (Capture match in Regex.Matches(tableNamedRange, "\\[.*?\\]", RegexOptions.Compiled))
      {
        string str = Regex.Replace(match.Value, "\\[|\\]", "");
        switch (str)
        {
          case "#All":
            return book.InnerNamesColection[listObject.Name + "[All]"];
          case "#Data":
            return book.InnerNamesColection[listObject.Name];
          default:
            using (IEnumerator<IListObjectColumn> enumerator = listObject.Columns.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                IListObjectColumn current = enumerator.Current;
                if (current.Name == str)
                  return book.InnerNamesColection[$"{listObject.Name}[{current.Name}]"];
              }
              continue;
            }
        }
      }
    }
    return (IName) null;
  }

  private static void ParseExternalSource(
    XmlReader reader,
    PivotCacheImpl cache,
    IWorkbook book,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (cache == null)
      throw new ArgumentNullException("pivot cache");
    if (book == null)
      throw new ArgumentNullException("workbook");
    if (cache.PreservedElements == null)
      return;
    reader.MoveToElement();
    Stream stream = ShapeParser.ReadNodeAsStream(reader);
    cache.PreservedElements.Add("external", stream);
  }

  private static void ParseCacheFields(
    XmlReader reader,
    PivotCacheImpl cache,
    WorkbookImpl bookImpl)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (cache == null)
      throw new ArgumentNullException("pivot cache ");
    Excel2007Parser.SkipWhiteSpaces(reader);
    if (reader.LocalName != "cacheFields")
      throw new XmlException("CacheFields");
    if (reader.IsEmptyElement)
    {
      reader.Read();
    }
    else
    {
      PivotCacheFieldsCollection cacheFields = cache.CacheFields;
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          if (reader.LocalName == "cacheField")
          {
            PivotCacheFieldImpl field = new PivotCacheFieldImpl();
            PivotCacheParser.ParseCacheField(reader, field, cache, bookImpl);
            cacheFields.Add(field);
          }
          reader.Read();
        }
        else
          reader.Read();
      }
      reader.Read();
    }
  }

  private static void ParseCacheField(
    XmlReader reader,
    PivotCacheFieldImpl field,
    PivotCacheImpl cache,
    WorkbookImpl bookImpl)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (field == null)
      throw new ArgumentNullException(nameof (field));
    if (reader.LocalName != "cacheField")
    {
      XmlException xmlException = new XmlException("Unexpected xml tag.");
    }
    if (reader.MoveToAttribute("name"))
      field.Name = reader.Value;
    if (reader.MoveToAttribute("databaseField"))
      field.IsDataBaseField = new bool?(XmlConvertExtension.ToBoolean(reader.Value));
    if (reader.MoveToAttribute("formula"))
      field.Formula = reader.Value;
    if (reader.MoveToAttribute("numFmtId"))
    {
      int int16 = (int) XmlConvertExtension.ToInt16(reader.Value);
      Dictionary<int, int> numberFormatIndexes = bookImpl.ArrNewNumberFormatIndexes;
      field.NumFormatIndex = numberFormatIndexes == null || !numberFormatIndexes.ContainsKey(int16) ? (int) (ushort) int16 : (int) (ushort) numberFormatIndexes[int16];
    }
    if (reader.MoveToAttribute("cap"))
      field.Caption = reader.Value;
    if (reader.MoveToAttribute("hierarchy"))
      field.Hierarchy = PivotCacheParser.ParseIntAttribute(reader, "hierarchy");
    if (reader.MoveToAttribute("level"))
      field.Level = PivotCacheParser.ParseIntAttribute(reader, "level");
    if (reader.MoveToAttribute("memberPropertyField"))
      field.IsMemberPropertyField = new bool?(XmlConvertExtension.ToBoolean(reader.Value));
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    Excel2007Parser.SkipWhiteSpaces(reader);
    PivotCacheParser.ParseSharedItems(reader, field);
    PivotCacheParser.ParseFieldGroup(reader, field, cache);
    while (reader.LocalName != "cacheField")
      reader.Skip();
  }

  private static void ParseSharedItems(XmlReader reader, PivotCacheFieldImpl field)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (field == null)
      throw new ArgumentNullException("cache field");
    if (reader.LocalName != "sharedItems")
      return;
    PivotDataType pivotDataType = (PivotDataType) 0;
    if (PivotCacheParser.ParseBoolAttribute(reader, "containsBlank"))
      pivotDataType |= PivotDataType.Blank;
    if (PivotCacheParser.ParseBoolAttribute(reader, "containsNumber"))
      pivotDataType |= PivotDataType.Number;
    if (PivotCacheParser.ParseBoolAttribute(reader, "containsDate"))
      pivotDataType |= PivotDataType.Date;
    if (PivotCacheParser.ParseBoolAttribute(reader, "containsString"))
      pivotDataType |= PivotDataType.String;
    if (PivotCacheParser.ParseBoolAttribute(reader, "containsInteger"))
      pivotDataType |= PivotDataType.Integer;
    if (PivotCacheParser.ParseBoolAttribute(reader, "longText"))
      pivotDataType |= PivotDataType.LongText;
    field.IsMixedType = PivotCacheParser.ParseBoolAttribute(reader, "containsMixedTypes");
    field.DataType = pivotDataType;
    reader.MoveToElement();
    string s = (string) null;
    field.IsParsed = new bool?(false);
    if (reader.LocalName == "sharedItems" && !reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        Excel2007Parser.SkipWhiteSpaces(reader);
        string localName = reader.LocalName;
        if (reader.MoveToAttribute("v"))
        {
          s = reader.Value;
          field.IsParsed = new bool?(true);
        }
        switch (localName)
        {
          case "n":
            double num = XmlConvertExtension.ToDouble(s);
            field.AddValue((object) num);
            break;
          case "s":
            field.AddValue((object) s);
            break;
          case "d":
            DateTime dateTime = XmlConvertExtension.ToDateTime(reader.Value, XmlDateTimeSerializationMode.Unspecified);
            field.AddValue((object) dateTime);
            break;
          case "b":
            bool boolean = XmlConvertExtension.ToBoolean(s);
            field.AddValue((object) boolean);
            break;
          case "m":
            field.IsParsed = new bool?(true);
            field.AddValue((object) null);
            break;
          case "e":
            field.AddValue((object) s);
            break;
          default:
            reader.Skip();
            break;
        }
        reader.Read();
      }
    }
    if (!reader.IsEmptyElement && (!(reader.LocalName == "sharedItems") || reader.NodeType != XmlNodeType.EndElement))
      return;
    reader.Read();
  }

  public static void ParseFieldGroup(
    XmlReader reader,
    PivotCacheFieldImpl field,
    PivotCacheImpl cache)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "fieldGroup")
      return;
    if (cache == null)
      throw new ArgumentNullException("cache ");
    PivotCacheFieldImpl baseField = (PivotCacheFieldImpl) null;
    int parentFieldIndex = -1;
    int i = -1;
    if (reader.MoveToAttribute("par"))
      parentFieldIndex = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("base"))
    {
      i = XmlConvertExtension.ToInt32(reader.Value);
      baseField = i != cache.CacheFields.Count ? cache.CacheFields[i] : field;
    }
    if (parentFieldIndex == -1)
    {
      field.FieldGroup = new FieldGroupImpl(baseField);
    }
    else
    {
      if (i == -1)
      {
        field.ParentFeildGroupIndex = parentFieldIndex;
        reader.Read();
        return;
      }
      field.FieldGroup = new FieldGroupImpl(baseField, parentFieldIndex);
    }
    reader.Read();
    int[] discretePropeties = PivotCacheParser.ParseDiscretePropeties(reader, field);
    PivotCacheParser.ParseRangeProperties(reader, field);
    string[] groupItems = PivotCacheParser.ParseGroupItems(reader, field);
    if (discretePropeties != null && groupItems != null)
      field.FieldGroup.FillDiscreteGroup(discretePropeties, groupItems);
    else
      field.FieldGroup.FillRangeGroup(groupItems);
    reader.Read();
    reader.Read();
  }

  public static int[] ParseDiscretePropeties(XmlReader reader, PivotCacheFieldImpl field)
  {
    if (reader.LocalName != "discretePr")
      return (int[]) null;
    List<int> intList = new List<int>();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "x" && reader.MoveToAttribute("v"))
        intList.Add((int) XmlConvertExtension.ToByte(reader.Value));
      reader.Read();
    }
    reader.Read();
    return intList.ToArray();
  }

  public static string[] ParseGroupItems(XmlReader reader, PivotCacheFieldImpl field)
  {
    if (reader.LocalName != "groupItems")
      return (string[]) null;
    reader.Read();
    List<string> stringList = new List<string>();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "s" && reader.MoveToAttribute("v"))
        stringList.Add(reader.Value);
      if (reader.LocalName == "m")
        field.FieldGroup.HasMissingAttribute = true;
      reader.Read();
    }
    return stringList.ToArray();
  }

  public static void ParseRangeProperties(XmlReader reader, PivotCacheFieldImpl field)
  {
    if (reader.LocalName != "rangePr")
      return;
    FieldGroupImpl fieldGroup = field.FieldGroup;
    if (reader.MoveToAttribute("autoEnd"))
      fieldGroup.AutoEndRange = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("endDate"))
      fieldGroup.EndDate = Convert.ToDateTime(reader.Value);
    if (reader.MoveToAttribute("endNum"))
      fieldGroup.EndNumber = XmlConvertExtension.ToDouble(reader.Value);
    if (reader.MoveToAttribute("groupBy"))
      fieldGroup.GroupBy = (PivotFieldGroupType) Enum.Parse(typeof (PivotFieldGroupType), reader.Value, true);
    if (reader.MoveToAttribute("groupInterval"))
    {
      fieldGroup.GroupInterval = XmlConvertExtension.ToDouble(reader.Value);
      fieldGroup.HasGroupInterval = true;
    }
    if (reader.MoveToAttribute("startDate"))
      fieldGroup.StartDate = Convert.ToDateTime(reader.Value);
    if (reader.MoveToAttribute("startNum"))
      fieldGroup.StartNumber = XmlConvertExtension.ToDouble(reader.Value);
    reader.Read();
  }

  public static void ParsePivotCacheRecords(XmlReader reader, PivotCacheImpl cache)
  {
    int rowCount = 0;
    PivotCacheParser.ParseCacheRecordRows(reader, cache, rowCount);
  }

  public static void ParseCacheRecordRows(XmlReader reader, PivotCacheImpl cache, int rowCount)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (cache == null)
      throw new ArgumentNullException("pivot cache");
    reader.Read();
    int num = 0;
    int ordinaryFieldCount = cache.CacheFields.GetOrdinaryFieldCount();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "r")
      {
        PivotCacheParser.ParseCacheRecordRow(reader, cache, rowCount, ordinaryFieldCount);
        ++num;
        reader.Read();
      }
    }
  }

  public static byte[] ParseCacheRecordRow(
    XmlReader reader,
    PivotCacheImpl cache,
    int currentRow,
    int iFieldsCount)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (cache == null)
      throw new ArgumentNullException("pivot cache");
    int fieldIndex = 0;
    string s = (string) null;
    byte[] cacheRecordRow = new byte[iFieldsCount];
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      string localName = reader.LocalName;
      if (reader.MoveToAttribute("v"))
        s = reader.Value;
      switch (localName)
      {
        case "x":
          uint uint32 = XmlConvertExtension.ToUInt32(s);
          cacheRecordRow[fieldIndex] = (byte) uint32;
          break;
        case "n":
          double num = XmlConvertExtension.ToDouble(s);
          cacheRecordRow[fieldIndex] = cache.PutValue(fieldIndex, (object) num);
          break;
        case "s":
          cacheRecordRow[fieldIndex] = cache.PutValue(fieldIndex, (object) s);
          break;
        case "d":
          DateTime dateTime = XmlConvertExtension.ToDateTime(reader.Value, XmlDateTimeSerializationMode.Unspecified);
          cacheRecordRow[fieldIndex] = cache.PutValue(fieldIndex, (object) dateTime);
          break;
        case "b":
          bool boolean = XmlConvertExtension.ToBoolean(s);
          cacheRecordRow[fieldIndex] = cache.PutValue(fieldIndex, (object) boolean);
          break;
        case "m":
          cacheRecordRow[fieldIndex] = cache.PutValue(fieldIndex, (object) null);
          break;
      }
      ++fieldIndex;
      reader.Read();
    }
    return cacheRecordRow;
  }

  private static void ParseCalculatedItems(XmlReader reader, PivotCacheFieldsCollection cacheFields)
  {
    if (reader.LocalName != "calculatedItems")
      return;
    if (cacheFields == null)
      throw new ArgumentNullException("cacheField");
    reader.Read();
    string formula = (string) null;
    PivotCacheFieldImpl cacheField = cacheFields[0];
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "calculatedItem")
      {
        int num = PivotCacheParser.ParseIntAttribute(reader, "fld", -1);
        if (reader.MoveToAttribute("formula"))
          formula = reader.Value;
        if (num == -1)
        {
          string fieldName = PivotCacheParser.GetFieldName(formula);
          cacheField = cacheFields[fieldName];
          num = cacheField.Index;
        }
        PivotCalculatedItemImpl calculatedItemImpl = new PivotCalculatedItemImpl(cacheField);
        PivotArea pivotArea = calculatedItemImpl.PivotArea;
        if (formula != null)
          calculatedItemImpl.Formula = formula;
        PivotCacheParser.ParsePivotArea(reader, pivotArea);
        if (num == -1)
        {
          int fieldIndex = pivotArea.References[0].FieldIndex;
          cacheField = cacheFields[fieldIndex];
          calculatedItemImpl.FieldIndex = fieldIndex;
        }
        cacheField.CalculatedItems.Add(calculatedItemImpl);
        reader.Read();
      }
    }
  }

  internal static void ParsePivotArea(XmlReader reader, PivotArea area)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (area == null)
      throw new ArgumentNullException("Pivot Area");
    reader.Read();
    if (reader.MoveToAttribute("axis"))
      area.Axis = (PivotAxisTypes) Enum.Parse(typeof (PivotAxisTypes2007), reader.Value, true);
    area.CollapsedLevelsAreSubtotals = PivotCacheParser.ParseBoolAttribute(reader, "collapsedLevelsAreSubtotals");
    area.IsCacheIndex = PivotCacheParser.ParseBoolAttribute(reader, "cacheIndex");
    area.IsDataOnly = PivotCacheParser.ParseBoolAttribute(reader, "dataOnly");
    area.FieldIndex = PivotCacheParser.ParseIntAttribute(reader, "field", -1);
    area.FieldPosition = PivotCacheParser.ParseIntAttribute(reader, "fieldPosition", -1);
    area.HasColumnGrand = PivotCacheParser.ParseBoolAttribute(reader, "grandCol");
    area.HasRowGrand = PivotCacheParser.ParseBoolAttribute(reader, "grandRow");
    area.IsLableOnly = PivotCacheParser.ParseBoolAttribute(reader, "labelOnly");
    area.IsOutline = PivotCacheParser.ParseBoolAttribute(reader, "outline", true);
    if (reader.MoveToAttribute("offset"))
      area.Offset = reader.Value;
    if (reader.MoveToAttribute("type"))
      area.AreaType = !(reader.Value == "button") ? (!(reader.Value == "origin") ? (PivotAreaType) Enum.Parse(typeof (PivotAreaType), Excel2007Serializator.CapitalizeFirstLetter(reader.Value), false) : PivotAreaType.Orgin) : PivotAreaType.FieldButton;
    reader.Read();
    if (reader.NodeType == XmlNodeType.EndElement)
      return;
    PivotCacheParser.ParsePivotAreaRefereces(reader, area);
    reader.Read();
  }

  private static void ParsePivotAreaRefereces(XmlReader reader, PivotArea pivotArea)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pivotArea == null)
      throw new ArgumentNullException("Pivot Area");
    reader.Read();
    InternalReference parent = new InternalReference();
    List<InternalReference> internalReferenceList1 = parent.Items;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "reference")
      {
        PivotAreaReference pivotAreaReference = new PivotAreaReference();
        PivotCacheParser.ParsePivotAreaReference(reader, pivotAreaReference);
        if (pivotArea.PivotTable != null && pivotAreaReference.Indexes.Count == 0 && pivotAreaReference.FieldIndex < pivotArea.PivotTable.Fields.Count)
        {
          int count = pivotArea.PivotTable.Fields[pivotAreaReference.FieldIndex].Items.Count;
          for (int index = 0; index < count; ++index)
            pivotAreaReference.Indexes.Add(index);
        }
        if (pivotAreaReference.Indexes.Count > 0)
        {
          List<InternalReference> collection = new List<InternalReference>();
          foreach (int index in pivotAreaReference.Indexes)
          {
            InternalReference internalReference = new InternalReference(index, pivotAreaReference);
            collection.Add(internalReference);
          }
          if (internalReferenceList1 == null)
          {
            List<InternalReference> internalReferenceList2 = new List<InternalReference>();
            internalReferenceList2.AddRange((IEnumerable<InternalReference>) collection);
            parent.Items = internalReferenceList2;
          }
          else
          {
            foreach (InternalReference internalReference in internalReferenceList1)
            {
              internalReference.Items = new List<InternalReference>();
              internalReference.Items.AddRange((IEnumerable<InternalReference>) collection);
            }
          }
          internalReferenceList1 = collection;
        }
        pivotArea.References.Add(pivotAreaReference);
        reader.Read();
      }
    }
    reader.Read();
    List<List<InternalReference>> internalReferences = PivotCacheParser.GetInternalReferences(parent, pivotArea.References.Count);
    if (internalReferences.Count <= 1)
      return;
    pivotArea.InternalReferences = internalReferences;
  }

  private static List<List<InternalReference>> GetInternalReferences(
    InternalReference parent,
    int refCount)
  {
    List<List<InternalReference>> internalReferences1 = new List<List<InternalReference>>();
    if (parent.Items != null && parent.Items.Count > 0)
    {
      for (int index = 0; index < parent.Items.Count; ++index)
      {
        InternalReference parent1 = parent.Items[index];
        List<InternalReference> internalReferenceList = new List<InternalReference>();
        List<InternalReference> internalReferences2;
        do
        {
          internalReferences2 = PivotCacheParser.GetInternalReferences(parent1);
          if (internalReferences2.Count != 0)
          {
            if (internalReferences2.Count > 0 && internalReferences2.Count == refCount)
              internalReferences1.Add(internalReferences2);
          }
          else
            break;
        }
        while (internalReferences2.Count > 0);
      }
    }
    return internalReferences1;
  }

  private static List<InternalReference> GetInternalReferences(InternalReference parent)
  {
    List<InternalReference> internalReferences1 = new List<InternalReference>();
    if (parent.Items != null && parent.Items.Count > 0 && parent.ChildCurrentIndex < parent.Items.Count)
    {
      internalReferences1.Add(parent);
      InternalReference parent1 = parent.Items[parent.ChildCurrentIndex];
      if (parent1 != null)
      {
        List<InternalReference> internalReferences2 = PivotCacheParser.GetInternalReferences(parent1);
        if (internalReferences2.Count > 0)
        {
          internalReferences1.AddRange((IEnumerable<InternalReference>) internalReferences2);
        }
        else
        {
          internalReferences1.Add(parent1);
          if (parent1.Items == null || parent1.ChildCurrentIndex < parent1.Items.Count)
          {
            ++parent.ChildCurrentIndex;
          }
          else
          {
            parent1.ChildCurrentIndex = 0;
            ++parent.ChildCurrentIndex;
          }
        }
      }
    }
    return internalReferences1;
  }

  private static void ParsePivotAreaReference(XmlReader reader, PivotAreaReference reference)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reference == null)
      throw new ArgumentNullException(nameof (reference));
    bool isEmptyElement = reader.IsEmptyElement;
    reference.FieldIndex = PivotCacheParser.ParseIntAttribute(reader, "field", -1);
    reference.Subtotal = PivotTableParser.ParseSubtotalFlags(reader);
    reference.IsReferByPosition = PivotCacheParser.ParseBoolAttribute(reader, "byPosition");
    reference.IsRelativeReference = PivotCacheParser.ParseBoolAttribute(reader, "relative");
    reference.IsSelected = PivotCacheParser.ParseBoolAttribute(reader, "selected");
    reference.IsDefaultSubTotal = PivotCacheParser.ParseBoolAttribute(reader, "defaultSubtotal");
    if (isEmptyElement)
      return;
    reader.Read();
    if (reader.LocalName != "x")
      throw new XmlException("unexpected Element tag");
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "x")
      {
        int intAttribute = PivotCacheParser.ParseIntAttribute(reader, "v");
        reference.Indexes.Add(intAttribute);
        reader.Read();
      }
    }
  }

  private static void ParseCacheExtension(XmlReader reader, PivotCacheImpl cache)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    Excel2007Parser.SkipWhiteSpaces(reader);
    if (reader.LocalName != "extLst")
      return;
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    if (cache.PreservedElements == null)
      return;
    reader.MoveToElement();
    Stream stream = ShapeParser.ReadNodeAsStream(reader);
    cache.PreservedElements.Add("extLst", stream);
  }

  private static void ParseCacheHierarchies(XmlReader reader, PivotCacheImpl cache)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "cacheHierarchies")
      return;
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    if (cache.PreservedElements == null)
      return;
    reader.MoveToElement();
    Stream stream = ShapeParser.ReadNodeAsStream(reader);
    cache.PreservedElements.Add("cacheHierarchies", stream);
  }

  private static void ParseOLAPKPIs(XmlReader reader, PivotCacheImpl cache)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "kpis")
      return;
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    if (cache.PreservedElements == null)
      return;
    reader.MoveToElement();
    Stream stream = ShapeParser.ReadNodeAsStream(reader);
    cache.PreservedElements.Add("kpis", stream);
  }

  private static void ParseOLAPDimensions(XmlReader reader, PivotCacheImpl cache)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "dimensions")
      return;
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    if (cache.PreservedElements == null)
      return;
    reader.MoveToElement();
    Stream stream = ShapeParser.ReadNodeAsStream(reader);
    cache.PreservedElements.Add("dimensions", stream);
  }

  private static void ParseOLAPMeasureGroups(XmlReader reader, PivotCacheImpl cache)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "measureGroups")
      return;
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    if (cache.PreservedElements == null)
      return;
    reader.MoveToElement();
    Stream stream = ShapeParser.ReadNodeAsStream(reader);
    cache.PreservedElements.Add("measureGroups", stream);
  }

  private static void ParseOLAPMaps(XmlReader reader, PivotCacheImpl cache)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "maps")
      return;
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    if (cache.PreservedElements == null)
      return;
    reader.MoveToElement();
    Stream stream = ShapeParser.ReadNodeAsStream(reader);
    cache.PreservedElements.Add("maps", stream);
  }

  internal static ExcelDataSourceType GetSourceType(string strSourceType)
  {
    ExcelDataSourceType sourceType = ExcelDataSourceType.Worksheet;
    switch (strSourceType)
    {
      case "worksheet":
        sourceType = ExcelDataSourceType.Worksheet;
        break;
      case "consolidation":
        sourceType = ExcelDataSourceType.Consolidation;
        break;
      case "external":
        sourceType = ExcelDataSourceType.ExternalData;
        break;
      case "scenario":
        sourceType = ExcelDataSourceType.ScenarioPivotTable;
        break;
    }
    return sourceType;
  }

  private static int ParseIntAttribute(XmlReader reader, string attributeName)
  {
    return reader.MoveToAttribute(attributeName) ? XmlConvertExtension.ToInt32(reader.Value) : 0;
  }

  private static int ParseIntAttribute(XmlReader reader, string attributeName, int defaultValue)
  {
    if (!reader.MoveToAttribute(attributeName))
      return defaultValue;
    string s = reader.Value;
    return XmlConvertExtension.ToDouble(s) == 4294967294.0 ? int.MaxValue : XmlConvertExtension.ToInt32(s);
  }

  private static bool ParseBoolAttribute(XmlReader reader, string attributeName)
  {
    return reader.MoveToAttribute(attributeName) && XmlConvertExtension.ToBoolean(reader.Value);
  }

  private static bool ParseBoolAttribute(XmlReader reader, string attributeName, bool defaultValue)
  {
    return reader.MoveToAttribute(attributeName) ? XmlConvertExtension.ToBoolean(reader.Value) : defaultValue;
  }

  private static string ParseStringAttribute(XmlReader reader, string attributeName)
  {
    return reader.MoveToAttribute(attributeName) ? reader.Value : (string) null;
  }

  private static string GetFieldName(string formula)
  {
    char ch = '[';
    string fieldName = formula.Split(ch)[0];
    if (fieldName.Contains("'"))
      fieldName = fieldName.Split('\'')[1];
    return fieldName;
  }
}
