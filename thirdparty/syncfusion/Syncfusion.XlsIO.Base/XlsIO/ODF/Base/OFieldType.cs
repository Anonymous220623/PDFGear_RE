// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.OFieldType
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base;

internal enum OFieldType
{
  FieldEmpty = -1, // 0xFFFFFFFF
  FieldNone = 0,
  FieldRef = 3,
  FieldIndexEntry = 4,
  FieldFootnoteRef = 5,
  FieldSet = 6,
  FieldIf = 7,
  FieldIndex = 8,
  FieldTOCEntry = 9,
  FieldStyleRef = 10, // 0x0000000A
  FieldRefDoc = 11, // 0x0000000B
  FieldSequence = 12, // 0x0000000C
  FieldTOC = 13, // 0x0000000D
  FieldInfo = 14, // 0x0000000E
  FieldTitle = 15, // 0x0000000F
  FieldSubject = 16, // 0x00000010
  FieldAuthor = 17, // 0x00000011
  FieldKeyWord = 18, // 0x00000012
  FieldComments = 19, // 0x00000013
  FieldLastSavedBy = 20, // 0x00000014
  FieldCreateDate = 21, // 0x00000015
  FieldSaveDate = 22, // 0x00000016
  FieldPrintDate = 23, // 0x00000017
  FieldRevisionNum = 24, // 0x00000018
  FieldEditTime = 25, // 0x00000019
  FieldNumPages = 26, // 0x0000001A
  FieldNumWords = 27, // 0x0000001B
  FieldNumChars = 28, // 0x0000001C
  FieldFileName = 29, // 0x0000001D
  FieldTemplate = 30, // 0x0000001E
  FieldDate = 31, // 0x0000001F
  FieldTime = 32, // 0x00000020
  FieldPage = 33, // 0x00000021
  FieldExpression = 34, // 0x00000022
  FieldQuote = 35, // 0x00000023
  FieldInclude = 36, // 0x00000024
  FieldPageRef = 37, // 0x00000025
  FieldAsk = 38, // 0x00000026
  FieldFillIn = 39, // 0x00000027
  FieldData = 40, // 0x00000028
  FieldNext = 41, // 0x00000029
  FieldNextIf = 42, // 0x0000002A
  FieldSkipIf = 43, // 0x0000002B
  FieldMergeRec = 44, // 0x0000002C
  FieldDDE = 45, // 0x0000002D
  FieldDDEAuto = 46, // 0x0000002E
  FieldGlossary = 47, // 0x0000002F
  FieldPrint = 48, // 0x00000030
  FieldFormula = 49, // 0x00000031
  FieldGoToButton = 50, // 0x00000032
  FieldMacroButton = 51, // 0x00000033
  FieldAutoNumOutline = 52, // 0x00000034
  FieldAutoNumLegal = 53, // 0x00000035
  FieldAutoNum = 54, // 0x00000036
  FieldImport = 55, // 0x00000037
  FieldLink = 56, // 0x00000038
  FieldSymbol = 57, // 0x00000039
  FieldEmbed = 58, // 0x0000003A
  FieldMergeField = 59, // 0x0000003B
  FieldUserName = 60, // 0x0000003C
  FieldUserInitials = 61, // 0x0000003D
  FieldUserAddress = 62, // 0x0000003E
  FieldBarCode = 63, // 0x0000003F
  FieldDocVariable = 64, // 0x00000040
  FieldSection = 65, // 0x00000041
  FieldSectionPages = 66, // 0x00000042
  FieldIncludePicture = 67, // 0x00000043
  FieldIncludeText = 68, // 0x00000044
  FieldFileSize = 69, // 0x00000045
  FieldFormTextInput = 70, // 0x00000046
  FieldFormCheckBox = 71, // 0x00000047
  FieldNoteRef = 72, // 0x00000048
  FieldTOA = 73, // 0x00000049
  FieldTOAEntry = 74, // 0x0000004A
  FieldMergeSeq = 75, // 0x0000004B
  FieldPrivate = 77, // 0x0000004D
  FieldDatabase = 78, // 0x0000004E
  FieldAutoText = 79, // 0x0000004F
  FieldCompare = 80, // 0x00000050
  FieldAddin = 81, // 0x00000051
  FieldSubscriber = 82, // 0x00000052
  FieldFormDropDown = 83, // 0x00000053
  FieldAdvance = 84, // 0x00000054
  FieldDocProperty = 85, // 0x00000055
  FieldOCX = 87, // 0x00000057
  FieldHyperlink = 88, // 0x00000058
  FieldAutoTextList = 89, // 0x00000059
  FieldListNum = 90, // 0x0000005A
  FieldHTMLActiveX = 91, // 0x0000005B
  FieldBidiOutline = 92, // 0x0000005C
  FieldAddressBlock = 93, // 0x0000005D
  FieldShape = 95, // 0x0000005F
  FieldUnknown = 1000, // 0x000003E8
}
