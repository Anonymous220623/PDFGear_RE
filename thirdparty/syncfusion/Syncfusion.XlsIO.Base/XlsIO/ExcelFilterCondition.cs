// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ExcelFilterCondition
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public enum ExcelFilterCondition
{
  Less = 1,
  Equal = 2,
  LessOrEqual = 3,
  Greater = 4,
  NotEqual = 5,
  GreaterOrEqual = 6,
  Contains = 7,
  DoesNotContain = 8,
  BeginsWith = 9,
  DoesNotBeginWith = 10, // 0x0000000A
  EndsWith = 11, // 0x0000000B
  DoesNotEndWith = 12, // 0x0000000C
}
