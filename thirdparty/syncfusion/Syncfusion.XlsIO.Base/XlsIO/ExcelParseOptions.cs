// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ExcelParseOptions
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

[Flags]
public enum ExcelParseOptions
{
  Default = 0,
  [Obsolete("This value is obsolete and won't affect on the XlsIO. It will be removed in next release. Sorry for inconvenience.")] SkipStyles = 1,
  DoNotParseCharts = 2,
  [Obsolete("This value is obsolete and won't affect on the XlsIO performance. It will be removed in next release. Sorry for inconvenience.")] StringsReadOnly = 4,
  DoNotParsePivotTable = 8,
  ParseWorksheetsOnDemand = 16, // 0x00000010
}
