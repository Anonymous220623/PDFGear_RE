// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.PivotTables.PivotCacheSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.PivotTables;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.PivotTables;

internal class PivotCacheSerializator
{
  private const int ApplicationVersion = 3;

  public static void SerializePivotCacheDefinition(
    XmlWriter writer,
    PivotCacheImpl cache,
    IWorkbook book,
    string relationId,
    RelationCollection relations)
  {
    WorkbookImpl workbookImpl = book as WorkbookImpl;
    if (workbookImpl.PreservesPivotCache.Count > 0)
    {
      Stream stream = workbookImpl.PreservesPivotCache[0];
      stream.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, stream);
      workbookImpl.PreservesPivotCache.RemoveAt(0);
    }
    else
    {
      writer.WriteStartElement("pivotCacheDefinition", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      if (relationId != null || cache.RelationId != null)
      {
        if (cache.RelationId != null)
        {
          relationId = cache.RelationId;
          cache.IsSaveData = true;
        }
        writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
      }
      writer.WriteAttributeString("refreshedBy", book.Author);
      double oaDate = cache.RefreshDate.ToOADate();
      writer.WriteAttributeString("refreshedDate", XmlConvert.ToString(oaDate));
      writer.WriteAttributeString("createdVersion", 3.ToString());
      writer.WriteAttributeString("refreshedVersion", 3.ToString());
      writer.WriteAttributeString("minRefreshableVersion", 3.ToString());
      Excel2007Serializator.SerializeAttribute(writer, "backgroundQuery", cache.IsBackgroundQuery, false);
      Excel2007Serializator.SerializeAttribute(writer, "enableRefresh", cache.EnableRefresh, false);
      Excel2007Serializator.SerializeAttribute(writer, "refreshOnLoad", cache.IsRefreshOnLoad, false);
      Excel2007Serializator.SerializeAttribute(writer, "invalid", cache.IsInvalidData, false);
      Excel2007Serializator.SerializeAttribute(writer, "optimizeMemory", cache.IsOptimizedCache, false);
      Excel2007Serializator.SerializeAttribute(writer, "upgradeOnRefresh", cache.IsUpgradeOnRefresh, false);
      Excel2007Serializator.SerializeAttribute(writer, "supportAdvancedDrill", cache.SupportAdvancedDrill, false);
      Excel2007Serializator.SerializeAttribute(writer, "supportSubquery", cache.IsSupportSubQuery, false);
      Excel2007Serializator.SerializeAttribute(writer, "saveData", cache.IsSaveData, true);
      Excel2007Serializator.SerializeAttribute(writer, "missingItemsLimit", cache.MissingItemsLimit, double.MinValue);
      Excel2007Serializator.SerializeAttribute(writer, "tupleCache", cache.TupleCache, false);
      writer.WriteAttributeString("recordCount", cache.RecordCount.ToString());
      PivotCacheSerializator.SerializeCacheSource(writer, cache, relations);
      PivotCacheSerializator.SerializeCacheFields(writer, cache.CacheFields, cache.HasNamedRange);
      PivotCacheSerializator.SerializeCalculatdItems(writer, cache.CacheFields);
      PivotCacheSerializator.SerializeCacheHierarchies(writer, cache);
      PivotCacheSerializator.SerializeOLAPKPIs(writer, cache);
      PivotCacheSerializator.SerializeOLAPDimesions(writer, cache);
      PivotCacheSerializator.SerializeOLAPMeasureGroups(writer, cache);
      PivotCacheSerializator.SerializeOLAPMaps(writer, cache);
      PivotCacheSerializator.SerializeCacheExtensions(writer, cache);
      writer.WriteEndElement();
      if (!(cache.SourceRange is ExternalRange sourceRange))
        return;
      PivotCacheSerializator.RegisterStrings(book as WorkbookImpl, sourceRange);
    }
  }

  private static void RegisterStrings(WorkbookImpl workbookImpl, ExternalRange range)
  {
    IWorksheet worksheet = range.Worksheet;
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(worksheet.Application, worksheet);
    for (int row = range.Row; row <= range.LastRow; ++row)
    {
      for (int column = range.Column; column <= range.LastColumn; ++column)
      {
        migrantRangeImpl.ResetRowColumn(row, column);
        if (migrantRangeImpl.HasString)
          workbookImpl.InnerSST.AddIncrease((object) migrantRangeImpl.Text, true);
      }
    }
  }

  private static void SerializeCacheFields(
    XmlWriter writer,
    PivotCacheFieldsCollection fieldsCollection,
    bool hasNamedRange)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    int num = fieldsCollection != null ? fieldsCollection.Count : throw new ArgumentNullException(nameof (fieldsCollection));
    writer.WriteStartElement("cacheFields");
    writer.WriteAttributeString("count", num.ToString());
    for (int i = 0; i < num; ++i)
    {
      PivotCacheFieldImpl fields = fieldsCollection[i];
      PivotCacheSerializator.SerializeCacheField(writer, fields, hasNamedRange);
    }
    writer.WriteEndElement();
  }

  private static void SerializeCacheField(
    XmlWriter writer,
    PivotCacheFieldImpl field,
    bool hasNamedRange)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (field == null)
      throw new ArgumentNullException(nameof (field));
    writer.WriteStartElement("cacheField");
    writer.WriteAttributeString("name", field.Name);
    writer.WriteAttributeString("numFmtId", field.NumFormatIndex.ToString());
    if (field.Hierarchy > 0)
      writer.WriteAttributeString("hierarchy", field.Hierarchy.ToString());
    if (field.Level > 0)
      writer.WriteAttributeString("level", field.Level.ToString());
    if (field.IsFormulaField)
      writer.WriteAttributeString("formula", field.Formula);
    if (field.Caption != null)
      writer.WriteAttributeString("caption", field.Caption);
    if (field.IsMemberPropertyField.HasValue)
    {
      string str = field.IsMemberPropertyField.Value ? "1" : "0";
      writer.WriteAttributeString("memberPropertyField", str);
    }
    if (field.IsDataBaseField.HasValue || field.IsFieldGroup && field.FieldGroup.IsDiscrete || field.IsFormulaField)
    {
      if (field.IsDataBaseField.HasValue)
      {
        string str = field.IsDataBaseField.Value ? "1" : "0";
        writer.WriteAttributeString("databaseField", str);
      }
      else
        Excel2007Serializator.SerializeAttribute(writer, "databaseField", false, true);
    }
    else
      PivotCacheSerializator.SerializeSharedItems(writer, field, hasNamedRange);
    if (field.IsFieldGroup)
      PivotCacheSerializator.SerializeFieldGroup(writer, field);
    else if (field.ParentFeildGroupIndex != -1)
      PivotCacheSerializator.SerializeFieldGroupParent(writer, field, field.ParentFeildGroupIndex);
    writer.WriteEndElement();
  }

  private static void SerializeSharedItems(
    XmlWriter writer,
    PivotCacheFieldImpl field,
    bool HasNamedRange)
  {
    IList<object> items = field.Items;
    writer.WriteStartElement("sharedItems");
    PivotDataType dataType = field.DataType;
    bool flag1 = (dataType & PivotDataType.String) != (PivotDataType) 0;
    bool flag2 = (dataType & PivotDataType.Integer) != (PivotDataType) 0 && (dataType & PivotDataType.Float) == (PivotDataType) 0;
    bool flag3 = (dataType & PivotDataType.Number) != (PivotDataType) 0;
    bool flag4 = (dataType & PivotDataType.Date) != (PivotDataType) 0;
    bool flag5 = (dataType & ~(PivotDataType.Blank | PivotDataType.Date)) != (PivotDataType) 0;
    bool flag6 = (dataType & PivotDataType.Blank) != (PivotDataType) 0;
    bool flag7 = (dataType & PivotDataType.Boolean) != (PivotDataType) 0;
    bool flag8 = flag3 && flag1 || flag3 && flag4 || flag4 && flag1 || flag4 && flag7;
    bool flag9 = false;
    bool flag10 = (dataType & PivotDataType.LongText) != (PivotDataType) 0;
    if ((dataType & (PivotDataType.Number | PivotDataType.Date)) != (PivotDataType) 0)
    {
      if (!flag6 && dataType != (PivotDataType.Blank | PivotDataType.Date) && dataType != (PivotDataType.Date | PivotDataType.Boolean) && dataType != (PivotDataType.Number | PivotDataType.Blank) && dataType != (PivotDataType.Number | PivotDataType.Integer | PivotDataType.Blank) && dataType != (PivotDataType.Number | PivotDataType.Integer | PivotDataType.Blank | PivotDataType.Float) && !flag8)
      {
        writer.WriteAttributeString("containsSemiMixedTypes", "0");
        flag9 = false;
      }
      if (!flag4)
      {
        Excel2007Serializator.SerializeAttribute(writer, "containsMixedTypes", flag8, false);
        if (!flag8)
          Excel2007Serializator.SerializeAttribute(writer, "containsString", flag1, !flag1);
        else
          flag9 = false;
        Excel2007Serializator.SerializeAttribute(writer, "containsBlank", flag6, false);
        Excel2007Serializator.SerializeAttribute(writer, "containsNumber", flag3, !flag3);
        Excel2007Serializator.SerializeAttribute(writer, "containsInteger", flag2, !flag2);
      }
      else if (flag4 && (flag1 || flag7))
      {
        Excel2007Serializator.SerializeAttribute(writer, "containsDate", flag4, !flag4);
        Excel2007Serializator.SerializeAttribute(writer, "containsMixedTypes", flag8, false);
        Excel2007Serializator.SerializeAttribute(writer, "containsBlank", flag6, false);
        flag9 = false;
      }
      else
      {
        Excel2007Serializator.SerializeAttribute(writer, "containsNonDate", flag5, !flag5);
        Excel2007Serializator.SerializeAttribute(writer, "containsDate", flag4, !flag4);
        Excel2007Serializator.SerializeAttribute(writer, "containsString", flag1, !flag1);
        Excel2007Serializator.SerializeAttribute(writer, "containsBlank", flag6, false);
        Excel2007Serializator.SerializeAttribute(writer, "containsMixedTypes", flag8, false);
        flag9 = false;
      }
    }
    else
      Excel2007Serializator.SerializeAttribute(writer, "containsBlank", flag6, false);
    if (flag6)
      flag9 = false;
    if (flag10)
    {
      Excel2007Serializator.SerializeAttribute(writer, "longText", flag10, false);
      flag9 = false;
    }
    if (field.IsParsed.HasValue)
    {
      bool? isParsed = field.IsParsed;
      flag9 = (!isParsed.GetValueOrDefault() ? 0 : (isParsed.HasValue ? 1 : 0)) == 0;
    }
    List<int> intList = PivotCacheSerializator.PrepareCalculatedItemOption(field);
    if (intList.Count > 0 || !flag9)
    {
      int index = 0;
      for (int count = items.Count; index < count; ++index)
      {
        bool isCalculated = intList.Contains(index);
        object obj = items[index];
        PivotCacheSerializator.SerializePivotCacheValue(writer, obj, isCalculated);
      }
    }
    writer.WriteEndElement();
  }

  private static void SerializeCacheSource(
    XmlWriter writer,
    PivotCacheImpl cache,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    switch (cache.SourceType)
    {
      case ExcelDataSourceType.Worksheet:
        PivotCacheSerializator.SerializeWorksheetSource(writer, cache, relations);
        break;
      case ExcelDataSourceType.ExternalData:
        PivotCacheSerializator.SerializeExternalSource(writer, cache);
        break;
      case ExcelDataSourceType.Consolidation:
        PivotCacheSerializator.SerializeConsolidation(writer, cache);
        break;
      case ExcelDataSourceType.ScenarioPivotTable:
        PivotCacheSerializator.SerializeScenarioSource(writer, cache);
        break;
    }
  }

  private static void SerializeScenarioSource(XmlWriter writer, PivotCacheImpl cache)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    writer.WriteStartElement("cacheSource");
    writer.WriteAttributeString("type", "scenario");
    writer.WriteEndElement();
  }

  private static void SerializeWorksheetSource(
    XmlWriter writer,
    PivotCacheImpl cache,
    RelationCollection relations)
  {
    writer.WriteStartElement("cacheSource");
    writer.WriteAttributeString("type", "worksheet");
    if (cache.PreservedElements.ContainsKey("worksheetSource"))
    {
      Stream preservedElement = cache.PreservedElements["worksheetSource"];
      preservedElement.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, preservedElement);
    }
    else
    {
      writer.WriteStartElement("worksheetSource");
      IRange sourceRange = cache.SourceRange;
      ExternalRange externalRange = sourceRange as ExternalRange;
      string str = (string) null;
      if (externalRange != null)
        str = PivotCacheSerializator.SerializeExternalRelation(externalRange, relations);
      if (cache.HasNamedRange)
      {
        writer.WriteAttributeString("name", cache.RangeName);
      }
      else
      {
        writer.WriteAttributeString("ref", sourceRange.AddressLocal);
        writer.WriteAttributeString("sheet", (sourceRange as ICombinedRange).WorksheetName);
      }
      if (str != null)
        writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", str);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static string SerializeExternalRelation(
    ExternalRange externalRange,
    RelationCollection relations)
  {
    string relationId = relations.GenerateRelationId();
    string fileName = Path.GetFileName(externalRange.ExternSheet.Workbook.URL);
    relations[relationId] = new Relation(fileName, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/externalLinkPath", true);
    return relationId;
  }

  public static void SerializeExternalSource(XmlWriter writer, PivotCacheImpl cache)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    if (!cache.PreservedElements.ContainsKey("external"))
      return;
    Stream preservedElement = cache.PreservedElements["external"];
    preservedElement.Position = 0L;
    ShapeParser.WriteNodeFromStream(writer, preservedElement);
  }

  public static void SerializeConsolidation(XmlWriter writer, PivotCacheImpl cache)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    if (!cache.PreservedElements.ContainsKey("consolidation"))
      return;
    Stream preservedElement = cache.PreservedElements["consolidation"];
    preservedElement.Position = 0L;
    ShapeParser.WriteNodeFromStream(writer, preservedElement);
  }

  public static void SerializePivotCacheRecords(
    XmlWriter writer,
    PivotCacheImpl cache,
    MemoryStream memory)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    bool flag = false;
    writer.WriteStartElement("pivotCacheRecords", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    PivotCacheFieldsCollection cacheFields = cache.CacheFields;
    int ordinaryFieldCount = cache.CacheFields.GetOrdinaryFieldCount();
    if (cache.SourceRange != null && cache.SourceRange.Worksheet.Workbook.Worksheets[cache.SourceRange.Worksheet.Name] != null && cache.RecordCount > 0)
    {
      IMigrantRange migrantRange = cache.SourceRange.Worksheet.MigrantRange;
      IRange sourceRange = cache.SourceRange;
      int num = 1;
      for (int length = cache.SourceRange.Rows.Length; num < length; ++num)
      {
        writer.WriteStartElement("r");
        for (int index1 = 0; index1 < ordinaryFieldCount; ++index1)
        {
          migrantRange.ResetRowColumn(num + 1, index1 + 1);
          object obj = migrantRange.Value2;
          if (migrantRange.HasFormula)
          {
            if (migrantRange.HasFormulaNumberValue)
              obj = (object) migrantRange.FormulaNumberValue;
            else if (migrantRange.HasFormulaDateTime)
              obj = (object) migrantRange.FormulaDateTime;
            else if (migrantRange.HasFormulaBoolValue)
              obj = (object) migrantRange.FormulaBoolValue;
            else if (migrantRange.HasFormulaStringValue)
              obj = (object) migrantRange.FormulaStringValue;
          }
          for (int index2 = 0; index2 < cache.CacheFields.InnerList[index1].Items.Count; ++index2)
          {
            if (cache.CacheFields.InnerList[index1].Items[index2] != null && cache.CacheFields.InnerList[index1].Items[index2].Equals(obj))
            {
              obj = (object) index2;
              flag = true;
            }
          }
          if (flag)
          {
            writer.WriteStartElement("x");
            writer.WriteAttributeString("v", obj.ToString());
            writer.WriteEndElement();
            flag = false;
          }
          else
            PivotCacheSerializator.SerializePivotCacheValue(writer, obj, false);
        }
        writer.WriteEndElement();
        writer.Flush();
      }
    }
    writer.WriteEndElement();
  }

  private static void SerializePivotCacheValue(XmlWriter writer, object value, bool isCalculated)
  {
    string localName;
    string str;
    switch (value)
    {
      case double num1:
        localName = "n";
        str = XmlConvert.ToString(num1);
        break;
      case string _:
        str = (string) value;
        localName = "s";
        break;
      case DateTime dateTime:
        localName = "d";
        str = dateTime.ToString("yyyy-MM-dd\\THH:mm:ss");
        if (CultureInfo.CurrentCulture.DateTimeFormat.TimeSeparator == ".")
        {
          str = str.Replace('.', ':');
          break;
        }
        break;
      case bool flag:
        localName = "b";
        str = XmlConvert.ToString(flag);
        break;
      case ushort num2:
        localName = "e";
        str = PivotCacheSerializator.GetErrorString(num2);
        break;
      case null:
        str = (string) null;
        localName = "m";
        break;
      default:
        throw new NotImplementedException();
    }
    writer.WriteStartElement(localName);
    if (str != null)
      writer.WriteAttributeString("v", str);
    if (isCalculated)
      writer.WriteAttributeString("f", "1");
    writer.WriteEndElement();
  }

  private static void SerializeCalculatdItems(XmlWriter writer, PivotCacheFieldsCollection fields)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (fields == null)
      throw new ArgumentNullException(nameof (fields));
    List<PivotCalculatedItems> pivotCalculatedItemsList = new List<PivotCalculatedItems>();
    foreach (PivotCacheFieldImpl field in (CollectionBase<PivotCacheFieldImpl>) fields)
    {
      if (field.CalculatedItems != null && field.CalculatedItems.Count > 0)
        pivotCalculatedItemsList.Add(field.CalculatedItems);
    }
    if (pivotCalculatedItemsList.Count <= 0)
      return;
    writer.WriteStartElement("calculatedItems");
    foreach (PivotCalculatedItems items in pivotCalculatedItemsList)
      PivotCacheSerializator.SerializeCalculatedItems(writer, items);
    writer.WriteEndElement();
  }

  private static void SerializeCalculatedItems(XmlWriter writer, PivotCalculatedItems items)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (items == null)
      throw new ArgumentNullException("cache");
    if (items.Count == 0)
      return;
    foreach (PivotCalculatedItemImpl calculatedItemImpl in (List<PivotCalculatedItemImpl>) items)
      PivotCacheSerializator.SerializeCalculatedItem(writer, calculatedItemImpl);
  }

  private static void SerializeCalculatedItem(XmlWriter writer, PivotCalculatedItemImpl item)
  {
    writer.WriteStartElement("calculatedItem");
    Excel2007Serializator.SerializeAttribute(writer, "field", item.FieldIndex, -1);
    Excel2007Serializator.SerializeAttribute(writer, "formula", item.Formula, (string) null);
    PivotCacheSerializator.SerializePivotArea(writer, item.PivotArea, false);
    writer.WriteEndElement();
  }

  internal static void SerializePivotArea(XmlWriter writer, PivotArea area, bool isAutoSort)
  {
    writer.WriteStartElement("pivotArea");
    if (area.Axis != PivotAxisTypes.None)
    {
      PivotAxisTypes2007 axis = (PivotAxisTypes2007) area.Axis;
      writer.WriteAttributeString("axis", axis.ToString());
    }
    Excel2007Serializator.SerializeAttribute(writer, "cacheIndex", area.IsCacheIndex, false);
    if (area.IsLableOnly || area.AreaType != PivotAreaType.None)
      Excel2007Serializator.SerializeBool(writer, "dataOnly", area.IsDataOnly);
    else
      Excel2007Serializator.SerializeAttribute(writer, "dataOnly", area.IsDataOnly, false);
    Excel2007Serializator.SerializeAttribute(writer, "collapsedLevelsAreSubtotals", area.CollapsedLevelsAreSubtotals, false);
    Excel2007Serializator.SerializeAttribute(writer, "field", area.FieldIndex, -1);
    Excel2007Serializator.SerializeAttribute(writer, "fieldPosition", area.FieldPosition, -1);
    Excel2007Serializator.SerializeAttribute(writer, "grandCol", area.HasColumnGrand, false);
    Excel2007Serializator.SerializeAttribute(writer, "grandRow", area.HasRowGrand, false);
    Excel2007Serializator.SerializeAttribute(writer, "labelOnly", area.IsLableOnly, false);
    Excel2007Serializator.SerializeAttribute(writer, "outline", area.IsOutline, true);
    if (area.Range != null)
      Excel2007Serializator.SerializeAttribute(writer, "offset", area.Range.AddressLocal, string.Empty);
    PivotCacheSerializator.SerializeAttribute(writer, "type", (Enum) area.AreaType, (Enum) PivotAreaType.None);
    if (area.References.Count > 0)
      PivotCacheSerializator.SerializePivotAreaReferences(writer, area.References, isAutoSort);
    writer.WriteEndElement();
  }

  internal static void SerializePivotAreaReferences(
    XmlWriter writer,
    PivotAreaReferences references,
    bool isAutoSort)
  {
    writer.WriteStartElement(nameof (references));
    writer.WriteAttributeString("count", isAutoSort ? (references.Count + 1).ToString() : references.Count.ToString());
    if (isAutoSort && (references.Count != 1 || references[0].FieldIndex != int.MaxValue))
      PivotCacheSerializator.SerializePivotAreaReference(writer, new PivotAreaReference()
      {
        FieldIndex = int.MaxValue,
        Indexes = {
          0
        }
      });
    foreach (PivotAreaReference areaReference in references.AreaReferences)
      PivotCacheSerializator.SerializePivotAreaReference(writer, areaReference);
    writer.WriteEndElement();
  }

  internal static void SerializePivotAreaReference(XmlWriter writer, PivotAreaReference reference)
  {
    if (reference.FieldIndex == (int) short.MaxValue)
      return;
    writer.WriteStartElement(nameof (reference));
    if (reference.FieldIndex == int.MaxValue)
      Excel2007Serializator.SerializeAttribute(writer, "field", 4294967294.0, -1.0);
    else
      Excel2007Serializator.SerializeAttribute(writer, "field", reference.FieldIndex, -1);
    Excel2007Serializator.SerializeAttribute(writer, "count", reference.Indexes.Count, 0);
    if (reference.Subtotal != PivotSubtotalTypes.None)
      PivotTableSerializator.SerializeSubtotal(writer, reference.Subtotal);
    Excel2007Serializator.SerializeAttribute(writer, "byPosition", reference.IsReferByPosition, false);
    Excel2007Serializator.SerializeAttribute(writer, "relative", reference.IsRelativeReference, false);
    Excel2007Serializator.SerializeAttribute(writer, "selected", reference.IsSelected, false);
    Excel2007Serializator.SerializeAttribute(writer, "defaultSubtotal", reference.IsDefaultSubTotal, false);
    foreach (int index in reference.Indexes)
    {
      writer.WriteStartElement("x");
      writer.WriteAttributeString("v", index.ToString());
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeFieldGroupParent(
    XmlWriter writer,
    PivotCacheFieldImpl field,
    int parentIndex)
  {
    writer.WriteStartElement("fieldGroup");
    Excel2007Serializator.SerializeAttribute(writer, "par", parentIndex, -1);
    writer.WriteEndElement();
  }

  private static void SerializeFieldGroup(XmlWriter writer, PivotCacheFieldImpl field)
  {
    FieldGroupImpl fieldGroup = field.FieldGroup;
    writer.WriteStartElement("fieldGroup");
    Excel2007Serializator.SerializeAttribute(writer, "par", fieldGroup.ParentFieldIndex, -1);
    Excel2007Serializator.SerializeAttribute(writer, "base", fieldGroup.PivotCacheFieldIndex, -1);
    if (fieldGroup.IsDiscrete)
    {
      PivotCacheSerializator.SerializeDiscreteProperties(writer, fieldGroup);
      PivotCacheSerializator.SerializeGroupItems(writer, fieldGroup);
    }
    else
    {
      PivotCacheSerializator.SerializeRangeProperties(writer, fieldGroup);
      PivotCacheSerializator.SerializeGroupItems(writer, fieldGroup);
    }
    writer.WriteEndElement();
  }

  private static void SerializeGroupItems(XmlWriter writer, FieldGroupImpl fieldGroup)
  {
    List<string> stringList = !fieldGroup.IsDiscrete ? fieldGroup.PivotRangeGroupNames : fieldGroup.PivotDiscreteGroupNames;
    if (stringList.Count == 0)
      return;
    writer.WriteStartElement("groupItems");
    foreach (string str in stringList)
    {
      writer.WriteStartElement("s");
      writer.WriteAttributeString("v", str);
      writer.WriteEndElement();
    }
    if (fieldGroup.HasMissingAttribute)
    {
      writer.WriteStartElement("m");
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeRangeProperties(XmlWriter writer, FieldGroupImpl fieldGroup)
  {
    writer.WriteStartElement("rangePr");
    if (fieldGroup.HasDateTime)
    {
      string str1 = fieldGroup.EndDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", (IFormatProvider) CultureInfo.InvariantCulture);
      string str2 = fieldGroup.StartDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", (IFormatProvider) CultureInfo.InvariantCulture);
      writer.WriteAttributeString("endDate", str1);
      writer.WriteAttributeString("startDate", str2);
    }
    else if (fieldGroup.HasNumber)
    {
      writer.WriteAttributeString("endNum", fieldGroup.EndNumber.ToString());
      writer.WriteAttributeString("startNum", fieldGroup.StartNumber.ToString());
    }
    else
    {
      writer.WriteAttributeString("autoEnd", fieldGroup.AutoEndRange.ToString());
      writer.WriteAttributeString("autoStart", fieldGroup.AutoStartRange.ToString());
    }
    Excel2007Serializator.SerializeAttribute(writer, "groupBy", (Enum) fieldGroup.GroupBy, (Enum) PivotFieldGroupType.None);
    if (fieldGroup.HasGroupInterval)
      Excel2007Serializator.SerializeAttribute(writer, "groupInterval", fieldGroup.GroupInterval, -1.0);
    writer.WriteEndElement();
  }

  private static void SerializeDiscreteProperties(XmlWriter writer, FieldGroupImpl fieldGroup)
  {
    writer.WriteStartElement("discretePr");
    foreach (byte discreteGroupIndex in fieldGroup.DiscreteGroupIndexes)
    {
      writer.WriteStartElement("x");
      writer.WriteAttributeString("v", discreteGroupIndex.ToString());
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeCacheHierarchies(XmlWriter writer, PivotCacheImpl cache)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    if (!cache.PreservedElements.ContainsKey("cacheHierarchies"))
      return;
    Stream preservedElement = cache.PreservedElements["cacheHierarchies"];
    preservedElement.Position = 0L;
    ShapeParser.WriteNodeFromStream(writer, preservedElement);
  }

  private static void SerializeCacheExtensions(XmlWriter writer, PivotCacheImpl cache)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (cache == null)
      throw new ArgumentNullException("Extension");
    if (cache.PreservedElements.ContainsKey("extLst"))
    {
      Stream preservedElement = cache.PreservedElements["extLst"];
      preservedElement.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, preservedElement);
    }
    else
    {
      writer.WriteStartElement("extLst");
      writer.WriteStartElement("ext");
      writer.WriteAttributeString("uri", "{725AE2AE-9491-48be-B2B4-4EB974FC3084}");
      writer.WriteAttributeString("xmlns", "x14", (string) null, "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
      writer.WriteStartElement("x14", "pivotCacheDefinition", (string) null);
      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
  }

  private static void SerializeOLAPKPIs(XmlWriter writer, PivotCacheImpl cache)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    if (!cache.PreservedElements.ContainsKey("kpis"))
      return;
    Stream preservedElement = cache.PreservedElements["kpis"];
    preservedElement.Position = 0L;
    ShapeParser.WriteNodeFromStream(writer, preservedElement);
  }

  private static void SerializeOLAPDimesions(XmlWriter writer, PivotCacheImpl cache)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    if (!cache.PreservedElements.ContainsKey("dimensions"))
      return;
    Stream preservedElement = cache.PreservedElements["dimensions"];
    preservedElement.Position = 0L;
    ShapeParser.WriteNodeFromStream(writer, preservedElement);
  }

  private static void SerializeOLAPMeasureGroups(XmlWriter writer, PivotCacheImpl cache)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    if (!cache.PreservedElements.ContainsKey("measureGroups"))
      return;
    Stream preservedElement = cache.PreservedElements["measureGroups"];
    preservedElement.Position = 0L;
    ShapeParser.WriteNodeFromStream(writer, preservedElement);
  }

  private static void SerializeOLAPMaps(XmlWriter writer, PivotCacheImpl cache)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    if (!cache.PreservedElements.ContainsKey("maps"))
      return;
    Stream preservedElement = cache.PreservedElements["maps"];
    preservedElement.Position = 0L;
    ShapeParser.WriteNodeFromStream(writer, preservedElement);
  }

  private static List<int> PrepareCalculatedItemOption(PivotCacheFieldImpl field)
  {
    PivotCalculatedItems calculatedItems = field.CalculatedItems;
    List<int> intList = new List<int>();
    foreach (PivotCalculatedItemImpl calculatedItemImpl in (List<PivotCalculatedItemImpl>) calculatedItems)
      intList.Add(calculatedItemImpl.PivotArea.References[0].Indexes[0]);
    return intList;
  }

  internal static void SerializeAttribute(
    XmlWriter writer,
    string attributeName,
    Enum value,
    Enum defaultValue)
  {
    if (value.CompareTo((object) defaultValue) == 0)
      return;
    if (value.ToString() == "FieldButton")
      writer.WriteAttributeString(attributeName, "button");
    else if (value.ToString() == "Orgin")
      writer.WriteAttributeString(attributeName, "origin");
    else
      writer.WriteAttributeString(attributeName, Excel2007Serializator.LowerFirstLetter(value.ToString()));
  }

  private static string GetErrorString(ushort value)
  {
    string errorString = "#N/A";
    if (value == (ushort) 1)
      return errorString;
    throw new NotImplementedException("Pivot Error String");
  }
}
