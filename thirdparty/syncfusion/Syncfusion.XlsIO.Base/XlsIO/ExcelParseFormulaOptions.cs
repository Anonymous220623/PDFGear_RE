// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ExcelParseFormulaOptions
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

[Flags]
public enum ExcelParseFormulaOptions
{
  None = 0,
  RootLevel = 1,
  InArray = 2,
  InName = 4,
  ParseOperand = 8,
  ParseComplexOperand = 16, // 0x00000010
  UseR1C1 = 32, // 0x00000020
}
