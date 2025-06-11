// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Excel2010Serializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using Syncfusion.XlsIO.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

public class Excel2010Serializator(WorkbookImpl book) : Excel2007Serializator(book)
{
  private const string VersionValue = "14.0300";
  public const string DataBarUri = "{B025F937-C7B1-47D3-B67F-A62EFF666E3E}";
  public const string DataBarExtUri = "{78C0D931-6437-407d-A8EE-F0AAD7539E65}";

  public override ExcelVersion Version => ExcelVersion.Excel2010;

  protected override void SerilaizeExtensions(XmlWriter writer, WorksheetImpl sheet)
  {
    this.SerializeConditionalFormattings(writer, sheet);
    if (sheet.SparklineGroups.Count > 0)
      this.SerializeSparklineGroups(writer, sheet);
    if (sheet.WorksheetSlicerStream == null)
      return;
    writer.WriteStartElement("ext");
    writer.WriteAttributeString("uri", "{A8765BA9-456A-4dab-B4F3-ACF838C121DE}");
    writer.WriteAttributeString("xmlns", "x14", (string) null, "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    sheet.WorksheetSlicerStream.Position = 0L;
    ShapeParser.WriteNodeFromStream(writer, sheet.WorksheetSlicerStream);
    writer.WriteEndElement();
  }

  public void SerializeSparklineGroups(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (sheet.SparklineGroups.Count == 0)
      return;
    writer.WriteStartElement("ext", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer.WriteAttributeString("uri", "{05C60535-1F16-4fd2-B633-F4F36F0B64E0}");
    writer.WriteAttributeString("xmlns", "x14", (string) null, "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    writer.WriteStartElement("sparklineGroups", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    writer.WriteAttributeString("xmlns", "xm", (string) null, "http://schemas.microsoft.com/office/excel/2006/main");
    foreach (SparklineGroup sparklineGroup in (IEnumerable<ISparklineGroup>) sheet.SparklineGroups)
      this.SerializeSparklineGroup(writer, sheet, sparklineGroup);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeSparklineGroup(
    XmlWriter writer,
    WorksheetImpl sheet,
    SparklineGroup sparklineGroup)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (sparklineGroup == null)
      throw new ArgumentNullException("SparklineGroup");
    writer.WriteStartElement(nameof (sparklineGroup), "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    if (sparklineGroup.VerticalAxisMaximum != null && sparklineGroup.VerticalAxisMaximum.VerticalAxisOptions == SparklineVerticalAxisOptions.Custom)
      writer.WriteAttributeString("manualMax", sparklineGroup.VerticalAxisMaximum.CustomValue.ToString());
    if (sparklineGroup.VerticalAxisMinimum != null && sparklineGroup.VerticalAxisMinimum.VerticalAxisOptions == SparklineVerticalAxisOptions.Custom)
      writer.WriteAttributeString("manualMin", sparklineGroup.VerticalAxisMinimum.CustomValue.ToString());
    if (sparklineGroup.SparklineType == SparklineType.Line)
      writer.WriteAttributeString("lineWeight", XmlConvert.ToString(sparklineGroup.LineWeight));
    switch (sparklineGroup.SparklineType)
    {
      case SparklineType.ColumnStacked100:
        writer.WriteAttributeString("type", "stacked");
        break;
      case SparklineType.Column:
        writer.WriteAttributeString("type", "column");
        break;
    }
    if (sparklineGroup.HorizontalDateAxis && sparklineGroup.HorizontalDateAxisRange != null)
      writer.WriteAttributeString("dateAxis", "1");
    switch (sparklineGroup.DisplayEmptyCellsAs)
    {
      case SparklineEmptyCells.Gaps:
        writer.WriteAttributeString("displayEmptyCellsAs", "gap");
        break;
      case SparklineEmptyCells.Line:
        if (sparklineGroup.SparklineType == SparklineType.Line)
        {
          writer.WriteAttributeString("displayEmptyCellsAs", "span");
          break;
        }
        writer.WriteAttributeString("displayEmptyCellsAs", "zero");
        break;
    }
    if (sparklineGroup.ShowMarkers && sparklineGroup.SparklineType == SparklineType.Line)
      writer.WriteAttributeString("markers", "1");
    if (sparklineGroup.ShowHighPoint)
      writer.WriteAttributeString("high", "1");
    if (sparklineGroup.ShowLowPoint)
      writer.WriteAttributeString("low", "1");
    if (sparklineGroup.ShowFirstPoint)
      writer.WriteAttributeString("first", "1");
    if (sparklineGroup.ShowLastPoint)
      writer.WriteAttributeString("last", "1");
    if (sparklineGroup.ShowNegativePoint)
      writer.WriteAttributeString("negative", "1");
    if (sparklineGroup.DisplayAxis)
      writer.WriteAttributeString("displayXAxis", "1");
    if (sparklineGroup.DisplayHiddenRC)
      writer.WriteAttributeString("displayHidden", "1");
    if (sparklineGroup.VerticalAxisMaximum != null)
    {
      switch (sparklineGroup.VerticalAxisMaximum.VerticalAxisOptions)
      {
        case SparklineVerticalAxisOptions.Same:
          writer.WriteAttributeString("maxAxisType", "group");
          break;
        case SparklineVerticalAxisOptions.Custom:
          writer.WriteAttributeString("maxAxisType", "custom");
          break;
      }
    }
    if (sparklineGroup.VerticalAxisMinimum != null)
    {
      switch (sparklineGroup.VerticalAxisMinimum.VerticalAxisOptions)
      {
        case SparklineVerticalAxisOptions.Same:
          writer.WriteAttributeString("minAxisType", "group");
          break;
        case SparklineVerticalAxisOptions.Custom:
          writer.WriteAttributeString("minAxisType", "custom");
          break;
      }
    }
    if (sparklineGroup.PlotRightToLeft)
      writer.WriteAttributeString("rightToLeft", "1");
    writer.WriteStartElement("colorSeries", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    ColorObject colorObject1 = new ColorObject(sparklineGroup.SparklineColor);
    writer.WriteAttributeString("rgb", colorObject1.Value.ToString("X6"));
    writer.WriteEndElement();
    writer.WriteStartElement("colorNegative", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    ColorObject colorObject2 = new ColorObject(sparklineGroup.NegativePointColor);
    writer.WriteAttributeString("rgb", colorObject2.Value.ToString("X6"));
    writer.WriteEndElement();
    writer.WriteStartElement("colorAxis", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    ColorObject colorObject3 = new ColorObject(sparklineGroup.AxisColor);
    writer.WriteAttributeString("rgb", colorObject3.Value.ToString("X6"));
    writer.WriteEndElement();
    if (sparklineGroup.SparklineType == SparklineType.Line)
    {
      writer.WriteStartElement("colorMarkers", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
      ColorObject colorObject4 = new ColorObject(sparklineGroup.MarkersColor);
      writer.WriteAttributeString("rgb", colorObject4.Value.ToString("X6"));
      writer.WriteEndElement();
    }
    writer.WriteStartElement("colorFirst", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    ColorObject colorObject5 = new ColorObject(sparklineGroup.FirstPointColor);
    writer.WriteAttributeString("rgb", colorObject5.Value.ToString("X6"));
    writer.WriteEndElement();
    writer.WriteStartElement("colorLast", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    ColorObject colorObject6 = new ColorObject(sparklineGroup.LastPointColor);
    writer.WriteAttributeString("rgb", colorObject6.Value.ToString("X6"));
    writer.WriteEndElement();
    writer.WriteStartElement("colorHigh", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    ColorObject colorObject7 = new ColorObject(sparklineGroup.HighPointColor);
    writer.WriteAttributeString("rgb", colorObject7.Value.ToString("X6"));
    writer.WriteEndElement();
    writer.WriteStartElement("colorLow", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    ColorObject colorObject8 = new ColorObject(sparklineGroup.LowPointColor);
    writer.WriteAttributeString("rgb", colorObject8.Value.ToString("X6"));
    writer.WriteEndElement();
    if (sparklineGroup.HorizontalDateAxisRange != null)
    {
      writer.WriteStartElement("f", "http://schemas.microsoft.com/office/excel/2006/main");
      writer.WriteString(sparklineGroup.HorizontalDateAxisRange.Address.Replace("'", ""));
      writer.WriteEndElement();
    }
    foreach (Sparklines sparklines in (List<ISparklines>) sparklineGroup)
      this.SerializeSparklines(writer, sheet, sparklines);
    writer.WriteEndElement();
  }

  private void SerializeSparklines(XmlWriter writer, WorksheetImpl sheet, Sparklines sparklines)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (sparklines == null)
      throw new ArgumentNullException("Sparlines");
    writer.WriteStartElement(nameof (sparklines), "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    foreach (Sparkline sparkline in (List<ISparkline>) sparklines)
      this.SerializeSparkline(writer, sheet, sparkline);
    writer.WriteEndElement();
  }

  private void SerializeSparkline(XmlWriter writer, WorksheetImpl sheet, Sparkline sparkline)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (sparkline == null)
      throw new ArgumentNullException(nameof (sparkline));
    writer.WriteStartElement(nameof (sparkline), "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    if (sparkline.DataRange != null)
    {
      writer.WriteStartElement("f", "http://schemas.microsoft.com/office/excel/2006/main");
      if (sparkline.DataRange != null && sparkline.DataRange.AddressGlobal.Contains(","))
      {
        for (int index = 0; index < sheet.Names.Count; ++index)
        {
          if (sparkline.DataRange.AddressGlobal == sheet.Names[index].RefersToRange.AddressGlobal)
          {
            writer.WriteString(sheet.Names[index].Name);
            break;
          }
        }
      }
      else
        writer.WriteString(sparkline.DataRange.AddressGlobal);
      writer.WriteEndElement();
    }
    writer.WriteStartElement("sqref", "http://schemas.microsoft.com/office/excel/2006/main");
    writer.WriteString(sparkline.ReferenceRange.AddressLocal);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  protected override void SerializeAppVersion(XmlWriter writer)
  {
    Excel2007Serializator.SerializeElementString(writer, "AppVersion", "14.0300", (string) null);
  }

  internal bool HasExtensionListOnCF(WorksheetImpl sheet, out bool isStreamPreserved)
  {
    isStreamPreserved = false;
    if (sheet.UsedRange.ConditionalFormats.Count == 0 && sheet.ConditionalFormats.Count == 0)
      return false;
    bool flag = false;
    foreach (IConditionalFormats conditionalFormat1 in (CollectionBase<ConditionalFormats>) sheet.ConditionalFormats)
    {
      for (int index = 0; index < conditionalFormat1.Count; ++index)
      {
        IConditionalFormat conditionalFormat2 = conditionalFormat1[index];
        if (conditionalFormat2.DataBar != null && (conditionalFormat2.DataBar as DataBarImpl).HasExtensionList)
        {
          flag = true;
          break;
        }
        if ((conditionalFormat2 as ConditionalFormatImpl).CFHasExtensionList && conditionalFormat2.FormatType == ExcelCFType.SpecificText)
        {
          flag = true;
          break;
        }
        if (conditionalFormat2.IconSet != null && (conditionalFormat2 as ConditionalFormatImpl).CFHasExtensionList)
        {
          flag = true;
          break;
        }
        if ((conditionalFormat2 as ConditionalFormatImpl).CFHasExtensionList && conditionalFormat2.FormatType == ExcelCFType.Formula && conditionalFormat2.FirstFormula != null && conditionalFormat2.FirstFormula.Contains("!"))
        {
          flag = true;
          break;
        }
        if ((conditionalFormat2 as ConditionalFormatImpl).CFHasExtensionList && conditionalFormat2.FormatType == ExcelCFType.CellValue)
        {
          flag = true;
          break;
        }
      }
    }
    if (flag)
      return true;
    if (sheet.DataHolder.m_cfsStream != null && sheet.DataHolder.m_cfsStream.Length != 0L)
      isStreamPreserved = true;
    return false;
  }

  public void SerializeConditionalFormattings(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    bool isStreamPreserved;
    bool flag = sheet != null ? this.HasExtensionListOnCF(sheet, out isStreamPreserved) : throw new ArgumentNullException(nameof (sheet));
    if (!flag && !isStreamPreserved)
      return;
    int iPriority = 1;
    int iPriorityCount = 0;
    string empty = string.Empty;
    if (flag)
    {
      writer.WriteStartElement("ext", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      writer.WriteAttributeString("uri", "{78C0D931-6437-407d-A8EE-F0AAD7539E65}");
      writer.WriteAttributeString("xmlns", "x14", (string) null, "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
      writer.WriteStartElement("x14", "conditionalFormattings", (string) null);
      foreach (IConditionalFormats conditionalFormat in (CollectionBase<ConditionalFormats>) sheet.ConditionalFormats)
      {
        for (int index = 0; index < conditionalFormat.Count; ++index)
        {
          IConditionalFormat condition = conditionalFormat[index];
          ConditionalFormatImpl conditionalFormatImpl1 = condition as ConditionalFormatImpl;
          IconSetImpl iconSet = conditionalFormatImpl1.IconSet as IconSetImpl;
          if (conditionalFormatImpl1.CFHasExtensionList)
          {
            ConditionalFormats conditionalFormats = conditionalFormat as ConditionalFormats;
            conditionalFormatImpl1.RangeRefernce = conditionalFormats.CellsList.Length > 0 ? string.Join(" ", conditionalFormats.CellsList) : conditionalFormats.Address.ToString();
            if (condition.FormatType == ExcelCFType.Formula && condition.FirstFormula != null || condition.FormatType == ExcelCFType.SpecificText && condition.FirstFormula != null && (condition.Operator == ExcelComparisonOperator.BeginsWith || condition.Operator == ExcelComparisonOperator.EndsWith || condition.Operator == ExcelComparisonOperator.ContainsText || condition.Operator == ExcelComparisonOperator.NotContainsText) || condition.FormatType == ExcelCFType.CellValue && condition.FirstFormula != null && (condition.Operator == ExcelComparisonOperator.Equal || condition.Operator == ExcelComparisonOperator.NotEqual || condition.Operator == ExcelComparisonOperator.Greater || condition.Operator == ExcelComparisonOperator.GreaterOrEqual || condition.Operator == ExcelComparisonOperator.Less || condition.Operator == ExcelComparisonOperator.LessOrEqual || condition.SecondFormula != null && (condition.Operator == ExcelComparisonOperator.Between || condition.Operator == ExcelComparisonOperator.NotBetween)))
            {
              writer.WriteStartElement("x14", "conditionalFormatting", (string) null);
              writer.WriteAttributeString("xmlns", "xm", (string) null, "http://schemas.microsoft.com/office/excel/2006/main");
              writer.WriteStartElement("x14", "cfRule", (string) null);
              writer.WriteAttributeString("type", this.GetCFType(condition.FormatType, condition.Operator));
              if (conditionalFormatImpl1.Priority > 1)
              {
                writer.WriteAttributeString("priority", conditionalFormatImpl1.Priority.ToString());
              }
              else
              {
                writer.WriteAttributeString("priority", iPriority.ToString());
                ++iPriority;
              }
              if (condition.FormatType == ExcelCFType.CellValue || condition.FormatType == ExcelCFType.SpecificText)
                writer.WriteAttributeString("operator", this.GetCFComparisonOperatorName(condition.Operator));
              writer.WriteAttributeString("id", conditionalFormatImpl1.ST_GUID.ToString());
              ConditionalFormatImpl conditionalFormatImpl2 = (ConditionalFormatImpl) condition;
              string firstSecondFormula1 = conditionalFormatImpl2.GetFirstSecondFormula(this.m_formulaUtil, true);
              string firstSecondFormula2 = conditionalFormatImpl2.GetFirstSecondFormula(this.m_formulaUtil, false);
              if (firstSecondFormula1 != null && firstSecondFormula1 != string.Empty)
                writer.WriteElementString("xm", "f", (string) null, firstSecondFormula1);
              if (firstSecondFormula2 != null && firstSecondFormula2 != string.Empty)
                writer.WriteElementString("xm", "f", (string) null, firstSecondFormula2);
              else if (conditionalFormatImpl1.Range != null)
                writer.WriteElementString("xm", "f", (string) null, (conditionalFormatImpl1.Range as RangeImpl).AddressGlobalWithoutSheetName);
              else if (conditionalFormatImpl1.AsteriskRange != null)
                writer.WriteElementString("xm", "f", (string) null, conditionalFormatImpl1.AsteriskRange);
              Excel2007Serializator excel2007Serializator = new Excel2007Serializator(conditionalFormatImpl1.Workbook);
              writer.WriteStartElement("x14", "dxf", (string) null);
              this.SerializeDxfFont(writer, condition as IInternalConditionalFormat);
              this.SerializeDxfFill(writer, condition as IInternalConditionalFormat);
              this.SerializeDxfBorders(writer, condition as IInternalConditionalFormat);
              writer.WriteEndElement();
              writer.WriteEndElement();
              writer.WriteElementString("xm", "sqref", (string) null, conditionalFormatImpl1.RangeRefernce);
              writer.WriteEndElement();
            }
          }
          if (condition.IconSet != null && (conditionalFormatImpl1.CFHasExtensionList || conditionalFormatImpl1.IconSet.IconSet == ExcelIconSetType.ThreeStars || iconSet.IsCustom || conditionalFormatImpl1.IconSet.IconSet == ExcelIconSetType.FiveBoxes || conditionalFormatImpl1.IconSet.IconSet == ExcelIconSetType.ThreeTriangles))
          {
            int parsedDxfsCount = this.m_book.DataHolder.ParsedDxfsCount != int.MinValue ? this.m_book.DataHolder.ParsedDxfsCount : 0;
            writer.WriteStartElement("x14", "conditionalFormatting", (string) null);
            writer.WriteAttributeString("xmlns", "xm", (string) null, "http://schemas.microsoft.com/office/excel/2006/main");
            this.SerializeCondition(writer, (XmlWriter) null, condition as IInternalConditionalFormat, ref parsedDxfsCount, ref iPriority, ref iPriorityCount);
            ConditionalFormats conditionalFormats = conditionalFormat as ConditionalFormats;
            string str = conditionalFormats.CellsList.Length > 0 ? string.Join(" ", conditionalFormats.CellsList) : conditionalFormats.Address;
            writer.WriteElementString("xm", "sqref", (string) null, str);
            writer.WriteEndElement();
          }
          if (condition.DataBar != null && (condition.DataBar as DataBarImpl).HasExtensionList)
          {
            int formatType = (int) condition.FormatType;
            IDataBar dataBar = condition.DataBar;
            DataBarImpl dataBarImpl = dataBar as DataBarImpl;
            writer.WriteStartElement("x14", "conditionalFormatting", (string) null);
            writer.WriteStartElement("x14", "cfRule", (string) null);
            writer.WriteAttributeString("type", "dataBar");
            writer.WriteAttributeString("id", dataBarImpl.ST_GUID.ToString());
            writer.WriteStartElement("x14", "dataBar", (string) null);
            writer.WriteAttributeString("border", dataBar.HasBorder ? "1" : "0");
            writer.WriteAttributeString("gradient", dataBar.HasGradientFill ? "1" : "0");
            writer.WriteAttributeString("minLength", dataBar.PercentMin.ToString());
            writer.WriteAttributeString("maxLength", dataBar.PercentMax.ToString());
            writer.WriteAttributeString("direction", dataBar.DataBarDirection.ToString());
            writer.WriteAttributeString("negativeBarColorSameAsPositive", dataBarImpl.HasDiffNegativeBarColor ? "0" : "1");
            writer.WriteAttributeString("negativeBarBorderColorSameAsPositive", dataBarImpl.HasDiffNegativeBarBorderColor ? "0" : "1");
            writer.WriteAttributeString("axisPosition", dataBar.DataBarAxisPosition.ToString());
            this.SerializeConditionValueObject(writer, dataBar.MinPoint, false, true);
            this.SerializeConditionValueObject(writer, dataBar.MaxPoint, false, false);
            if (dataBar.BorderColor.Name != "0")
              this.SerializeRgbColor(writer, "borderColor", (ColorObject) dataBar.BorderColor);
            if (dataBar.NegativeFillColor.Name != "0" && dataBarImpl.HasDiffNegativeBarColor)
              this.SerializeRgbColor(writer, "negativeFillColor", (ColorObject) dataBar.NegativeFillColor);
            if (dataBar.HasBorder && dataBarImpl.HasDiffNegativeBarBorderColor && dataBar.NegativeBorderColor.Name != "0")
              this.SerializeRgbColor(writer, "negativeBorderColor", (ColorObject) dataBar.NegativeBorderColor);
            if (dataBar.DataBarAxisPosition != DataBarAxisPosition.none && dataBar.BarAxisColor.Name != "0")
              this.SerializeRgbColor(writer, "axisColor", (ColorObject) dataBar.BarAxisColor);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
          }
        }
      }
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    else
    {
      if (!isStreamPreserved)
        return;
      if (sheet.SparklineGroups.Count == 0)
        writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      writer.WriteStartElement("ext", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      writer.WriteAttributeString("uri", "{78C0D931-6437-407d-A8EE-F0AAD7539E65}");
      writer.WriteAttributeString("xmlns", "x14", (string) null, "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
      Excel2007Serializator.SerializeStream(writer, sheet.DataHolder.m_cfsStream, "root");
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
  }

  public new void SerializeConditionValueObject(
    XmlWriter writer,
    IConditionValue conditionValue,
    bool isIconSet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("x14", "cfvo", (string) null);
    int type = (int) conditionValue.Type;
    string valueType = CF.ValueTypes[type];
    writer.WriteAttributeString("type", valueType);
    writer.WriteAttributeString("val", conditionValue.Value);
    if (isIconSet)
      writer.WriteAttributeString("gte", ((int) conditionValue.Operator).ToString());
    writer.WriteEndElement();
  }

  public new void SerializeRgbColor(XmlWriter writer, string tagName, ColorObject color)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tagName == null || tagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (tagName));
    int num = color.Value;
    writer.WriteStartElement("x14", tagName, (string) null);
    writer.WriteAttributeString("rgb", num.ToString("X8"));
    Excel2007Serializator.SerializeAttribute(writer, "tint", color.Tint, 0.0);
    writer.WriteEndElement();
  }

  public new void SerializeConditionValueObject(
    XmlWriter writer,
    IConditionValue conditionValue,
    bool isIconSet,
    bool isMinPoint)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("x14", "cfvo", (string) null);
    int index = (int) conditionValue.Type;
    if (!isIconSet)
      index = index == 7 ? (isMinPoint ? 7 : 8) : index;
    string valueType = CF.ValueTypes[index];
    writer.WriteAttributeString("type", valueType);
    writer.WriteAttributeString("val", conditionValue.Value);
    if (isIconSet)
      writer.WriteAttributeString("gte", ((int) conditionValue.Operator).ToString());
    writer.WriteEndElement();
  }
}
