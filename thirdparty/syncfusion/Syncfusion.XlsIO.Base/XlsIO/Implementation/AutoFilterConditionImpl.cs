// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.AutoFilterConditionImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class AutoFilterConditionImpl : IAutoFilterCondition
{
  private ExcelFilterDataType m_dataType;
  private ExcelFilterCondition m_conditionOperator;
  private string m_strValue;
  private bool m_bValue;
  private byte m_btErrorCode;
  private double m_dValue;
  private AutoFiltersCollection m_autofilters;
  private AutoFilterImpl m_autoFilter;

  public AutoFilterConditionImpl(AutoFiltersCollection autofilters)
  {
    this.m_autofilters = autofilters;
    this.m_conditionOperator = ExcelFilterCondition.Equal;
  }

  internal AutoFilterConditionImpl(AutoFiltersCollection autofilters, AutoFilterImpl autofilter)
    : this(autofilters)
  {
    this.m_autoFilter = autofilter;
  }

  internal void Dispose()
  {
    this.m_autofilters = (AutoFiltersCollection) null;
    this.m_autoFilter = (AutoFilterImpl) null;
  }

  public ExcelFilterDataType DataType
  {
    get => this.m_dataType;
    set
    {
      this.m_dataType = value;
      IAutoFilterCondition firstCondition = this.m_autoFilter.FirstCondition;
      IAutoFilterCondition secondCondition = this.m_autoFilter.SecondCondition;
      int index = this.m_autoFilter.Index;
      if (firstCondition.DataType == ExcelFilterDataType.MatchAllBlanks)
      {
        this.m_autoFilter.SelectRangesToFilter();
        this.m_autoFilter.SetCondition(ExcelFilterCondition.Equal, this.m_dataType, (object) string.Empty, index, true);
      }
      if (secondCondition.DataType == ExcelFilterDataType.MatchAllBlanks)
      {
        this.m_autoFilter.SelectRangesToFilter();
        this.m_autoFilter.SetCondition(ExcelFilterCondition.Equal, this.m_dataType, (object) string.Empty, index, false);
      }
      if (firstCondition.DataType == ExcelFilterDataType.MatchAllNonBlanks)
      {
        this.m_autoFilter.SelectRangesToFilter();
        this.m_autoFilter.SetCondition(ExcelFilterCondition.NotEqual, this.m_dataType, (object) string.Empty, index, true);
      }
      if (secondCondition.DataType != ExcelFilterDataType.MatchAllNonBlanks)
        return;
      this.m_autoFilter.SelectRangesToFilter();
      this.m_autoFilter.SetCondition(ExcelFilterCondition.NotEqual, this.m_dataType, (object) string.Empty, index, false);
    }
  }

  public ExcelFilterCondition ConditionOperator
  {
    get => this.m_conditionOperator;
    set => this.m_conditionOperator = value;
  }

  public string String
  {
    get => this.m_strValue;
    set
    {
      this.m_strValue = value;
      this.m_dataType = ExcelFilterDataType.String;
      this.m_autoFilter.SelectRangesToFilter();
      this.m_autoFilter.FilterType = ExcelFilterType.CustomFilter;
      object strValue = (object) this.m_strValue;
      IAutoFilterCondition firstCondition = this.m_autoFilter.FirstCondition;
      IAutoFilterCondition secondCondition = this.m_autoFilter.SecondCondition;
      if (firstCondition.String == this.m_strValue && firstCondition.DataType == this.m_dataType)
        this.m_autoFilter.SetCondition(firstCondition.ConditionOperator, firstCondition.DataType, strValue, this.m_autoFilter.Index, true);
      if (!(secondCondition.String == this.m_strValue) || secondCondition.DataType != this.m_dataType)
        return;
      this.m_autoFilter.SetCondition(secondCondition.ConditionOperator, secondCondition.DataType, strValue, this.m_autoFilter.Index, false);
    }
  }

  public bool Boolean
  {
    get => this.m_bValue;
    set => this.m_bValue = value;
  }

  public byte ErrorCode
  {
    get => this.m_btErrorCode;
    set => this.m_btErrorCode = value;
  }

  public double Double
  {
    get => this.m_dValue;
    set
    {
      this.m_dValue = value;
      this.m_dataType = ExcelFilterDataType.FloatingPoint;
      object dValue = (object) this.m_dValue;
      IAutoFilterCondition firstCondition = this.m_autoFilter.FirstCondition;
      IAutoFilterCondition secondCondition = this.m_autoFilter.SecondCondition;
      if (firstCondition.Double == this.m_dValue && firstCondition.DataType == this.m_dataType && firstCondition.ConditionOperator == this.m_conditionOperator)
      {
        this.m_autoFilter.SelectRangesToFilter();
        this.m_autoFilter.FilterType = ExcelFilterType.CustomFilter;
        this.m_autoFilter.SetCondition(firstCondition.ConditionOperator, firstCondition.DataType, dValue, this.m_autoFilter.Index, true);
      }
      if (secondCondition.Double != this.m_dValue || secondCondition.DataType != this.m_dataType || secondCondition.ConditionOperator != this.m_conditionOperator)
        return;
      this.m_autoFilter.SelectRangesToFilter();
      this.m_autoFilter.FilterType = ExcelFilterType.CustomFilter;
      this.m_autoFilter.SetCondition(secondCondition.ConditionOperator, secondCondition.DataType, dValue, this.m_autoFilter.Index, false);
    }
  }

  [CLSCompliant(false)]
  public void Parse(AutoFilterRecord.DOPER condition)
  {
    switch (condition.DataType)
    {
      case AutoFilterRecord.DOPER.DOPERDataType.FilterNotUsed:
        this.m_dataType = ExcelFilterDataType.NotUsed;
        break;
      case AutoFilterRecord.DOPER.DOPERDataType.RKNumber:
        this.m_dataType = ExcelFilterDataType.FloatingPoint;
        this.m_dValue = RKRecord.ConvertToDouble(condition.RKNumber);
        break;
      case AutoFilterRecord.DOPER.DOPERDataType.Number:
        this.m_dataType = ExcelFilterDataType.FloatingPoint;
        this.m_dValue = condition.Number;
        break;
      case AutoFilterRecord.DOPER.DOPERDataType.String:
        this.m_dataType = ExcelFilterDataType.String;
        this.m_strValue = condition.StringValue;
        break;
      case AutoFilterRecord.DOPER.DOPERDataType.BoolOrError:
        if (condition.IsBool)
        {
          this.m_dataType = ExcelFilterDataType.Boolean;
          this.m_bValue = condition.Boolean;
          break;
        }
        this.m_dataType = ExcelFilterDataType.ErrorCode;
        this.m_btErrorCode = condition.ErrorCode;
        break;
      case AutoFilterRecord.DOPER.DOPERDataType.MatchBlanks:
        this.m_dataType = ExcelFilterDataType.MatchAllBlanks;
        break;
      case AutoFilterRecord.DOPER.DOPERDataType.MatchNonBlanks:
        this.m_dataType = ExcelFilterDataType.MatchAllNonBlanks;
        break;
      default:
        throw new ArgumentOutOfRangeException("condition.DataType");
    }
    this.m_conditionOperator = (ExcelFilterCondition) condition.ComparisonSign;
  }

  [CLSCompliant(false)]
  public void Serialize(AutoFilterRecord.DOPER condition)
  {
    switch (this.m_dataType)
    {
      case ExcelFilterDataType.NotUsed:
        condition.DataType = AutoFilterRecord.DOPER.DOPERDataType.FilterNotUsed;
        break;
      case ExcelFilterDataType.FloatingPoint:
        condition.DataType = AutoFilterRecord.DOPER.DOPERDataType.Number;
        condition.Number = this.m_dValue;
        break;
      case ExcelFilterDataType.String:
        condition.DataType = AutoFilterRecord.DOPER.DOPERDataType.String;
        condition.StringValue = this.m_strValue;
        break;
      case ExcelFilterDataType.Boolean:
        condition.DataType = AutoFilterRecord.DOPER.DOPERDataType.BoolOrError;
        condition.Boolean = this.m_bValue;
        break;
      case ExcelFilterDataType.ErrorCode:
        condition.DataType = AutoFilterRecord.DOPER.DOPERDataType.BoolOrError;
        condition.ErrorCode = this.m_btErrorCode;
        break;
      case ExcelFilterDataType.MatchAllBlanks:
        condition.DataType = AutoFilterRecord.DOPER.DOPERDataType.MatchBlanks;
        break;
      case ExcelFilterDataType.MatchAllNonBlanks:
        condition.DataType = AutoFilterRecord.DOPER.DOPERDataType.MatchNonBlanks;
        break;
      default:
        throw new ArgumentOutOfRangeException("m_dataType");
    }
    condition.ComparisonSign = (AutoFilterRecord.DOPER.DOPERComparisonSign) this.m_conditionOperator;
  }

  public AutoFilterConditionImpl Clone(object parent)
  {
    return (AutoFilterConditionImpl) this.MemberwiseClone();
  }
}
