// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.AF
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

public sealed class AF
{
  public const string AutoFilterSettingsTagName = "autoFilter";
  public const string CellOrRangeReferenceAttributeName = "ref";
  public const string AutoFilterColumnTagName = "filterColumn";
  public const string FilterColumnDataAttributeName = "colId";
  internal const string IconFilterTagName = "iconFilter";
  internal const string ColorFilterTagName = "colorFilter";
  internal const string CellColorAttributeName = "cellColor";
  public const string AutoFilterTopTenTagName = "top10";
  public const string TopOrBottomValueAttributeName = "val";
  public const string TopAttributeAttributeName = "top";
  public const string FilterValAttributeName = "filterVal";
  public const string FilterByPercentAttributeName = "percent";
  public const string FilterCriteriaTagName = "filters";
  public const string FilterBlankAttributeName = "blank";
  public const string FilterTagName = "filter";
  public const string FilterValueAttributeName = "val";
  public const string CustomFiltersCriteriaTagName = "customFilters";
  public const string AndCriteriaAttributeName = "and";
  public const string CustomFilterCriteriaTagName = "customFilter";
  public const string FilterComparisonOperatorAttributeName = "operator";
  internal const string ShowButtonAttributeName = "showButton";
  internal const string HiddenButtonAttributeName = "hiddenButton";
  internal const string DateGroupItemTagName = "dateGroupItem";
  internal const string DateTimeGroupingAttributeName = "dateTimeGrouping";
  internal const string YearAttributeName = "year";
  internal const string MonthAttributeName = "month";
  internal const string DayAttributeName = "day";
  internal const string HourAttributeName = "hour";
  internal const string MinuteAttributeName = "minute";
  internal const string SecondAttributeName = "second";
  internal const string DynamicFilterTagName = "dynamicFilter";
  internal const string DynamicFilterTypeAttributeName = "type";
  public const string OperatorEqual = "equal";
  public const string OperatorGreaterThan = "greaterThan";
  public const string OperatorGreaterThanOrEqual = "greaterThanOrEqual";
  public const string OperatorLessThan = "lessThan";
  public const string OperatorLessThanOrEqual = "lessThanOrEqual";
  public const string OperatorNotEqual = "notEqual";

  internal static DynamicFilterType ConvertToDateFilterType(string filterType)
  {
    switch (filterType)
    {
      case "tomorrow":
        return DynamicFilterType.Tomorrow;
      case "today":
        return DynamicFilterType.Today;
      case "yesterday":
        return DynamicFilterType.Yesterday;
      case "nextWeek":
        return DynamicFilterType.NextWeek;
      case "thisWeek":
        return DynamicFilterType.ThisWeek;
      case "lastWeek":
        return DynamicFilterType.LastWeek;
      case "nextMonth":
        return DynamicFilterType.NextMonth;
      case "thisMonth":
        return DynamicFilterType.ThisMonth;
      case "lastMonth":
        return DynamicFilterType.LastMonth;
      case "nextQuarter":
        return DynamicFilterType.NextQuarter;
      case "thisQuarter":
        return DynamicFilterType.ThisQuarter;
      case "lastQuarter":
        return DynamicFilterType.LastQuarter;
      case "nextYear":
        return DynamicFilterType.NextYear;
      case "thisYear":
        return DynamicFilterType.ThisYear;
      case "lastYear":
        return DynamicFilterType.LastYear;
      case "yearToDate":
        return DynamicFilterType.YearToDate;
      case "M1":
        return DynamicFilterType.January;
      case "M2":
        return DynamicFilterType.February;
      case "M3":
        return DynamicFilterType.March;
      case "M4":
        return DynamicFilterType.April;
      case "M5":
        return DynamicFilterType.May;
      case "M6":
        return DynamicFilterType.June;
      case "M7":
        return DynamicFilterType.July;
      case "M8":
        return DynamicFilterType.August;
      case "M9":
        return DynamicFilterType.September;
      case "M10":
        return DynamicFilterType.October;
      case "M11":
        return DynamicFilterType.November;
      case "M12":
        return DynamicFilterType.December;
      case "Q1":
        return DynamicFilterType.Quarter1;
      case "Q2":
        return DynamicFilterType.Quarter2;
      case "Q3":
        return DynamicFilterType.Quarter3;
      case "Q4":
        return DynamicFilterType.Quarter4;
      default:
        return DynamicFilterType.None;
    }
  }

  internal static string ConvertDateFilterTypeToString(DynamicFilterType filterType)
  {
    switch (filterType)
    {
      case DynamicFilterType.Tomorrow:
        return "tomorrow";
      case DynamicFilterType.Today:
        return "today";
      case DynamicFilterType.Yesterday:
        return "yesterday";
      case DynamicFilterType.NextWeek:
        return "nextWeek";
      case DynamicFilterType.ThisWeek:
        return "thisWeek";
      case DynamicFilterType.LastWeek:
        return "lastWeek";
      case DynamicFilterType.NextMonth:
        return "nextMonth";
      case DynamicFilterType.ThisMonth:
        return "thisMonth";
      case DynamicFilterType.LastMonth:
        return "lastMonth";
      case DynamicFilterType.NextQuarter:
        return "nextQuarter";
      case DynamicFilterType.ThisQuarter:
        return "thisQuarter";
      case DynamicFilterType.LastQuarter:
        return "lastQuarter";
      case DynamicFilterType.NextYear:
        return "nextYear";
      case DynamicFilterType.ThisYear:
        return "thisYear";
      case DynamicFilterType.LastYear:
        return "lastYear";
      case DynamicFilterType.Quarter1:
        return "Q1";
      case DynamicFilterType.Quarter2:
        return "Q2";
      case DynamicFilterType.Quarter3:
        return "Q3";
      case DynamicFilterType.Quarter4:
        return "Q4";
      case DynamicFilterType.January:
        return "M1";
      case DynamicFilterType.February:
        return "M2";
      case DynamicFilterType.March:
        return "M3";
      case DynamicFilterType.April:
        return "M4";
      case DynamicFilterType.May:
        return "M5";
      case DynamicFilterType.June:
        return "M6";
      case DynamicFilterType.July:
        return "M7";
      case DynamicFilterType.August:
        return "M8";
      case DynamicFilterType.September:
        return "M9";
      case DynamicFilterType.October:
        return "M10";
      case DynamicFilterType.November:
        return "M11";
      case DynamicFilterType.December:
        return "M12";
      case DynamicFilterType.YearToDate:
        return "yearToDate";
      default:
        return (string) null;
    }
  }
}
