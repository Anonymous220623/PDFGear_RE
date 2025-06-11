// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.FilteringRule
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Conditions;
using NLog.Config;

#nullable disable
namespace NLog.Targets.Wrappers;

[NLogConfigurationItem]
public class FilteringRule
{
  public FilteringRule()
    : this((ConditionExpression) null, (ConditionExpression) null)
  {
  }

  public FilteringRule(ConditionExpression whenExistsExpression, ConditionExpression filterToApply)
  {
    this.Exists = whenExistsExpression;
    this.Filter = filterToApply;
  }

  [RequiredParameter]
  public ConditionExpression Exists { get; set; }

  [RequiredParameter]
  public ConditionExpression Filter { get; set; }
}
