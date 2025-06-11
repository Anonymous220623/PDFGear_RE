// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ExcelCopyRangeOptions
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

[Flags]
public enum ExcelCopyRangeOptions
{
  None = 0,
  UpdateFormulas = 1,
  UpdateMerges = 2,
  CopyStyles = 4,
  CopyShapes = 8,
  CopyErrorIndicators = 16, // 0x00000010
  CopyConditionalFormats = 32, // 0x00000020
  CopyDataValidations = 64, // 0x00000040
  All = CopyDataValidations | CopyConditionalFormats | CopyErrorIndicators | CopyShapes | CopyStyles | UpdateMerges | UpdateFormulas, // 0x0000007F
  CopyValueAndSourceFormatting = 256, // 0x00000100
}
