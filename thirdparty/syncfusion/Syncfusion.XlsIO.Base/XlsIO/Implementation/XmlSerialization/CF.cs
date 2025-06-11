// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.CF
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

public sealed class CF
{
  public const string ConditionalFormattingsTagName = "conditionalFormattings";
  public const string ConditionalFormattingTagName = "conditionalFormatting";
  public const string RuleTagName = "cfRule";
  public const string EndsWith = "endsWith";
  public const string BeginsWith = "beginsWith";
  public const string ContainsText = "containsText";
  public const string NotContainsText = "notContainsText";
  public const string TypeContainsError = "containsErrors";
  public const string TypeNotContainsError = "notContainsErrors";
  public const string TextAttributeName = "text";
  public const string TypeAttributeName = "type";
  public const string TimePeriodTypeName = "timePeriod";
  public const string TimePeriodAttributeName = "timePeriod";
  public const string DifferentialFormattingIdAttributeName = "dxfId";
  public const string OperatorAttributeName = "operator";
  public const string BorderColorTagName = "borderColor";
  public const string NegativeFillColorTagName = "negativeFillColor";
  public const string NegativeBorderColorTagName = "negativeBorderColor";
  public const string AxisColorTagName = "axisColor";
  public const string BorderAttributeName = "border";
  public const string GradientAttributeName = "gradient";
  public const string DirectionAttributeName = "direction";
  public const string NegativeBarColorSameAsPositiveAttributeName = "negativeBarColorSameAsPositive";
  public const string NegativeBarBorderColorSameAsPositiveAttributeName = "negativeBarBorderColorSameAsPositive";
  public const string AxisPositionAttributeName = "axisPosition";
  public const string ExtentionList = "extLst";
  public const string Extenstion = "ext";
  internal const string Percent = "percent";
  internal const string Bottom = "bottom";
  internal const string Rank = "rank";
  internal const string EqualAverage = "equalAverage";
  internal const string StandardDeviation = "stdDev";
  public const string TimePeriodToday = "today";
  public const string TimePeriodYesterday = "yesterday";
  public const string TimePeriodTomorrow = "tomorrow";
  public const string TimePeriodLastsevenDays = "last7Days";
  public const string TimePeriodLastWeek = "lastWeek";
  public const string TimePeriodThisWeek = "thisWeek";
  public const string TimePeriodNextWeek = "nextWeek";
  public const string TimePeriodLastMonth = "lastMonth";
  public const string TimePeriodThisMonth = "thisMonth";
  public const string TimePeriodNextMonth = "nextMonth";
  public const string OperatorBeginsWith = "beginsWith";
  public const string OperatorBetween = "between";
  public const string OperatorContains = "containsText";
  public const string OperatorEndsWith = "endsWith";
  public const string OperatorEqual = "equal";
  public const string OperatorGreaterThan = "greaterThan";
  public const string OperatorGreaterThanOrEqual = "greaterThanOrEqual";
  public const string OperatorLessThan = "lessThan";
  public const string OperatorLessThanOrEqual = "lessThanOrEqual";
  public const string OperatorNotBetween = "notBetween";
  public const string OperatorDoesNotContain = "notContains";
  public const string OperatorNotEqual = "notEqual";
  public const string StopIfTrueAttributeName = "stopIfTrue";
  public const string PriorityAttributeName = "priority";
  public const string FormulaTagName = "formula";
  public const string TypeCellIs = "cellIs";
  public const string TypeExpression = "expression";
  public const string TypeDataBar = "dataBar";
  public const string Pivot = "pivot";
  public const string TypeIconSet = "iconSet";
  public const string TypeColorScale = "colorScale";
  public const string TypeContainsBlank = "containsBlanks";
  public const string TypeNotContainsBlank = "notContainsBlanks";
  public const string DataBarTag = "dataBar";
  public const string ValueObjectTag = "cfvo";
  internal const string ValueObjectFormulaTag = "f";
  internal const string CFRuleID = "id";
  internal const string IconObjectTag = "cfIcon";
  public const int DefaultDataBarMinLength = 0;
  public const int DefaultDataBarMaxLength = 100;
  public const string MaxLengthTag = "maxLength";
  public const string MinLengthTag = "minLength";
  public const string ShowValueAttribute = "showValue";
  public const string IconSetTag = "iconSet";
  public const string IconSetAttribute = "iconSet";
  internal const string IconIdAttribute = "iconId";
  internal const string CustomAttribute = "custom";
  public const string ColorScaleTag = "colorScale";
  internal const string Unique = "uniqueValues";
  internal const string Duplicate = "duplicateValues";
  public const string PercentAttribute = "percent";
  public const string ReverseAttribute = "reverse";
  public const string GreaterAttribute = "gte";
  internal const int RightAndLeftFormulaLength = 7;
  internal const string Top10 = "top10";
  internal const string AboveAverage = "aboveAverage";
  public static string[] ValueTypes = new string[9]
  {
    "none",
    "num",
    "min",
    "max",
    "percent",
    "percentile",
    "formula",
    "autoMin",
    "autoMax"
  };
  public static string[] IconSetTypeNames = new string[20]
  {
    "3Arrows",
    "3ArrowsGray",
    "3Flags",
    "3TrafficLights1",
    "3TrafficLights2",
    "3Signs",
    "3Symbols",
    "3Symbols2",
    "4Arrows",
    "4ArrowsGray",
    "4RedToBlack",
    "4Rating",
    "4TrafficLights",
    "5Arrows",
    "5ArrowsGray",
    "5Rating",
    "5Quarters",
    "3Stars",
    "3Triangles",
    "5Boxes"
  };

  private CF()
  {
  }
}
