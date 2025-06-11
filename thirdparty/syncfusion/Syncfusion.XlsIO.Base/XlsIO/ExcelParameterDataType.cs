// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ExcelParameterDataType
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public enum ExcelParameterDataType
{
  ParamTypeWChar = -8, // 0xFFFFFFF8
  ParamTypeBit = -7, // 0xFFFFFFF9
  ParamTypeTinyInt = -6, // 0xFFFFFFFA
  ParamTypeBigInt = -5, // 0xFFFFFFFB
  ParamTypeLongVarBinary = -4, // 0xFFFFFFFC
  ParamTypeVarBinary = -3, // 0xFFFFFFFD
  ParamTypeBinary = -2, // 0xFFFFFFFE
  ParamTypeLongVarChar = -1, // 0xFFFFFFFF
  ParamTypeUnknown = 0,
  ParamTypeChar = 1,
  ParamTypeNumeric = 2,
  ParamTypeDecimal = 3,
  ParamTypeInteger = 4,
  ParamTypeSmallInt = 5,
  ParamTypeFloat = 6,
  ParamTypeReal = 7,
  ParamTypeDouble = 8,
  ParamTypeDate = 9,
  ParamTypeTime = 10, // 0x0000000A
  ParamTypeTimestamp = 11, // 0x0000000B
  ParamTypeVarChar = 12, // 0x0000000C
}
