// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.FormulaToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

internal enum FormulaToken
{
  None = 0,
  tExp = 1,
  tTbl = 2,
  tAdd = 3,
  tSub = 4,
  tMul = 5,
  tDiv = 6,
  tPower = 7,
  tConcat = 8,
  tLessThan = 9,
  tLessEqual = 10, // 0x0000000A
  tEqual = 11, // 0x0000000B
  tGreaterEqual = 12, // 0x0000000C
  tGreater = 13, // 0x0000000D
  tNotEqual = 14, // 0x0000000E
  tCellRangeIntersection = 15, // 0x0000000F
  tCellRangeList = 16, // 0x00000010
  tCellRange = 17, // 0x00000011
  tUnaryPlus = 18, // 0x00000012
  tUnaryMinus = 19, // 0x00000013
  tPercent = 20, // 0x00000014
  tParentheses = 21, // 0x00000015
  tMissingArgument = 22, // 0x00000016
  tStringConstant = 23, // 0x00000017
  tExtended = 24, // 0x00000018
  tAttr = 25, // 0x00000019
  tSheet = 26, // 0x0000001A
  tEndSheet = 27, // 0x0000001B
  tError = 28, // 0x0000001C
  tBoolean = 29, // 0x0000001D
  tInteger = 30, // 0x0000001E
  tNumber = 31, // 0x0000001F
  tArray1 = 32, // 0x00000020
  tFunction1 = 33, // 0x00000021
  tFunctionVar1 = 34, // 0x00000022
  tName1 = 35, // 0x00000023
  tRef1 = 36, // 0x00000024
  tArea1 = 37, // 0x00000025
  tMemArea1 = 38, // 0x00000026
  tMemErr1 = 39, // 0x00000027
  tMemNoMem1 = 40, // 0x00000028
  tMemFunc1 = 41, // 0x00000029
  tRefErr1 = 42, // 0x0000002A
  tAreaErr1 = 43, // 0x0000002B
  tRefN1 = 44, // 0x0000002C
  tAreaN1 = 45, // 0x0000002D
  tMemAreaN1 = 46, // 0x0000002E
  tMemNoMemN1 = 47, // 0x0000002F
  tFunctionCE1 = 56, // 0x00000038
  tNameX1 = 57, // 0x00000039
  tRef3d1 = 58, // 0x0000003A
  tArea3d1 = 59, // 0x0000003B
  tRefErr3d1 = 60, // 0x0000003C
  tAreaErr3d1 = 61, // 0x0000003D
  tArray2 = 64, // 0x00000040
  tFunction2 = 65, // 0x00000041
  tFunctionVar2 = 66, // 0x00000042
  tName2 = 67, // 0x00000043
  tRef2 = 68, // 0x00000044
  tArea2 = 69, // 0x00000045
  tMemArea2 = 70, // 0x00000046
  tMemErr2 = 71, // 0x00000047
  tMemNoMem2 = 72, // 0x00000048
  tMemFunc2 = 73, // 0x00000049
  tRefErr2 = 74, // 0x0000004A
  tAreaErr2 = 75, // 0x0000004B
  tRefN2 = 76, // 0x0000004C
  tAreaN2 = 77, // 0x0000004D
  tMemAreaN2 = 78, // 0x0000004E
  tMemNoMemN2 = 79, // 0x0000004F
  tFunctionCE2 = 88, // 0x00000058
  tNameX2 = 89, // 0x00000059
  tRef3d2 = 90, // 0x0000005A
  tArea3d2 = 91, // 0x0000005B
  tRefErr3d2 = 92, // 0x0000005C
  tAreaErr3d2 = 93, // 0x0000005D
  tArray3 = 96, // 0x00000060
  tFunction3 = 97, // 0x00000061
  tFunctionVar3 = 98, // 0x00000062
  tName3 = 99, // 0x00000063
  tRef3 = 100, // 0x00000064
  tArea3 = 101, // 0x00000065
  tMemArea3 = 102, // 0x00000066
  tMemErr3 = 103, // 0x00000067
  tMemNoMem3 = 104, // 0x00000068
  tMemFunc3 = 105, // 0x00000069
  tRefErr3 = 106, // 0x0000006A
  tAreaErr3 = 107, // 0x0000006B
  tRefN3 = 108, // 0x0000006C
  tAreaN3 = 109, // 0x0000006D
  tMemAreaN3 = 110, // 0x0000006E
  tMemNoMemN3 = 111, // 0x0000006F
  tFunctionCE3 = 120, // 0x00000078
  tNameX3 = 121, // 0x00000079
  tRef3d3 = 122, // 0x0000007A
  tArea3d3 = 123, // 0x0000007B
  tRefErr3d3 = 124, // 0x0000007C
  tAreaErr3d3 = 125, // 0x0000007D
  EndOfFormula = 4097, // 0x00001001
  CloseParenthesis = 4098, // 0x00001002
  Comma = 4099, // 0x00001003
  OpenBracket = 4100, // 0x00001004
  CloseBracket = 4101, // 0x00001005
  ValueTrue = 4102, // 0x00001006
  ValueFalse = 4103, // 0x00001007
  Space = 4104, // 0x00001008
  Identifier = 4105, // 0x00001009
  DDELink = 4106, // 0x0000100A
  Identifier3D = 4107, // 0x0000100B
}
