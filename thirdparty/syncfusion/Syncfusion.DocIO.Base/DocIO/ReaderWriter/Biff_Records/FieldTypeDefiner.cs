// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.FieldTypeDefiner
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class FieldTypeDefiner
{
  [ThreadStatic]
  private static Dictionary<string, FieldType> m_hashStrType;
  [ThreadStatic]
  private static Dictionary<FieldType, string> m_hashTypeStr;

  internal static Dictionary<string, FieldType> StrTypeTable
  {
    get
    {
      if (FieldTypeDefiner.m_hashStrType == null)
        FieldTypeDefiner.InitStrTypeHash();
      return FieldTypeDefiner.m_hashStrType;
    }
  }

  internal static Dictionary<FieldType, string> TypeStrTable
  {
    get
    {
      if (FieldTypeDefiner.m_hashTypeStr == null)
        FieldTypeDefiner.InitTypeStrHash();
      return FieldTypeDefiner.m_hashTypeStr;
    }
  }

  internal FieldTypeDefiner()
  {
  }

  internal static FieldType GetFieldType(string fieldCode)
  {
    char[] chArray = new char[4]{ ' ', ' ', '"', '”' };
    string[] strArray = fieldCode.TrimStart().Split(chArray);
    string str = strArray.Length != 0 ? strArray[0].ToUpper() : throw new Exception(string.Format("Specified fieldcode is not valid.", (object) fieldCode));
    if (str.StartsWithExt("="))
      return FieldType.FieldFormula;
    if (FieldTypeDefiner.StrTypeTable.ContainsKey(str))
      return FieldTypeDefiner.StrTypeTable[str];
    return str == "PROPRIETEDOC" ? FieldType.FieldDocProperty : FieldType.FieldUnknown;
  }

  internal static string GetFieldCode(FieldType fieldType)
  {
    return FieldTypeDefiner.TypeStrTable.ContainsKey(fieldType) ? FieldTypeDefiner.TypeStrTable[fieldType] : (string) null;
  }

  internal static bool IsFormField(FieldType fieldType)
  {
    return fieldType == FieldType.FieldFormCheckBox || fieldType == FieldType.FieldFormDropDown || fieldType == FieldType.FieldFormTextInput;
  }

  private static void InitStrTypeHash()
  {
    FieldTypeDefiner.m_hashStrType = new Dictionary<string, FieldType>();
    FieldTypeDefiner.m_hashStrType.Add("=", FieldType.FieldFormula);
    FieldTypeDefiner.m_hashStrType.Add("ADVANCE", FieldType.FieldAdvance);
    FieldTypeDefiner.m_hashStrType.Add("ASK", FieldType.FieldAsk);
    FieldTypeDefiner.m_hashStrType.Add("AUTHOR", FieldType.FieldAuthor);
    FieldTypeDefiner.m_hashStrType.Add("AUTONUM", FieldType.FieldAutoNum);
    FieldTypeDefiner.m_hashStrType.Add("AUTONUMLGL", FieldType.FieldAutoNumLegal);
    FieldTypeDefiner.m_hashStrType.Add("AUTONUMOUT", FieldType.FieldAutoNumOutline);
    FieldTypeDefiner.m_hashStrType.Add("AUTOTEXT", FieldType.FieldAutoText);
    FieldTypeDefiner.m_hashStrType.Add("AUTOTEXTLIST", FieldType.FieldAutoTextList);
    FieldTypeDefiner.m_hashStrType.Add("BARCODE", FieldType.FieldBarCode);
    FieldTypeDefiner.m_hashStrType.Add("COMMENTS", FieldType.FieldComments);
    FieldTypeDefiner.m_hashStrType.Add("COMPARE", FieldType.FieldCompare);
    FieldTypeDefiner.m_hashStrType.Add("CREATEDATE", FieldType.FieldCreateDate);
    FieldTypeDefiner.m_hashStrType.Add("DATABASE", FieldType.FieldDatabase);
    FieldTypeDefiner.m_hashStrType.Add("DATE", FieldType.FieldDate);
    FieldTypeDefiner.m_hashStrType.Add("DOCPROPERTY", FieldType.FieldDocProperty);
    FieldTypeDefiner.m_hashStrType.Add("DOCVARIABLE", FieldType.FieldDocVariable);
    FieldTypeDefiner.m_hashStrType.Add("EDITTIME", FieldType.FieldEditTime);
    FieldTypeDefiner.m_hashStrType.Add("EQ", FieldType.FieldExpression);
    FieldTypeDefiner.m_hashStrType.Add("FILENAME", FieldType.FieldFileName);
    FieldTypeDefiner.m_hashStrType.Add("FILESIZE", FieldType.FieldFileSize);
    FieldTypeDefiner.m_hashStrType.Add("FILLIN", FieldType.FieldFillIn);
    FieldTypeDefiner.m_hashStrType.Add("GOTOBUTTON", FieldType.FieldGoToButton);
    FieldTypeDefiner.m_hashStrType.Add("HYPERLINK", FieldType.FieldHyperlink);
    FieldTypeDefiner.m_hashStrType.Add("IF", FieldType.FieldIf);
    FieldTypeDefiner.m_hashStrType.Add("INCLUDETEXT", FieldType.FieldIncludeText);
    FieldTypeDefiner.m_hashStrType.Add("INCLUDEPICTURE", FieldType.FieldIncludePicture);
    FieldTypeDefiner.m_hashStrType.Add("INDEX", FieldType.FieldIndex);
    FieldTypeDefiner.m_hashStrType.Add("INFO", FieldType.FieldInfo);
    FieldTypeDefiner.m_hashStrType.Add("KEYWORDS", FieldType.FieldKeyWord);
    FieldTypeDefiner.m_hashStrType.Add("LASTSAVEDBY", FieldType.FieldLastSavedBy);
    FieldTypeDefiner.m_hashStrType.Add("LINK", FieldType.FieldLink);
    FieldTypeDefiner.m_hashStrType.Add("LISTNUM", FieldType.FieldListNum);
    FieldTypeDefiner.m_hashStrType.Add("MACROBUTTON", FieldType.FieldMacroButton);
    FieldTypeDefiner.m_hashStrType.Add("MERGEFIELD", FieldType.FieldMergeField);
    FieldTypeDefiner.m_hashStrType.Add("MERGEREC", FieldType.FieldMergeRec);
    FieldTypeDefiner.m_hashStrType.Add("MERGESEQ", FieldType.FieldMergeSeq);
    FieldTypeDefiner.m_hashStrType.Add("NEXT", FieldType.FieldNext);
    FieldTypeDefiner.m_hashStrType.Add("NEXTIF", FieldType.FieldNextIf);
    FieldTypeDefiner.m_hashStrType.Add("NOTEREF", FieldType.FieldNoteRef);
    FieldTypeDefiner.m_hashStrType.Add("NUMCHARS", FieldType.FieldNumChars);
    FieldTypeDefiner.m_hashStrType.Add("NUMPAGES", FieldType.FieldNumPages);
    FieldTypeDefiner.m_hashStrType.Add("NUMWORDS", FieldType.FieldNumWords);
    FieldTypeDefiner.m_hashStrType.Add("PAGE", FieldType.FieldPage);
    FieldTypeDefiner.m_hashStrType.Add("PAGEREF", FieldType.FieldPageRef);
    FieldTypeDefiner.m_hashStrType.Add("PRINT", FieldType.FieldPrint);
    FieldTypeDefiner.m_hashStrType.Add("PRINTDATE", FieldType.FieldPrintDate);
    FieldTypeDefiner.m_hashStrType.Add("PRIVATE", FieldType.FieldPrivate);
    FieldTypeDefiner.m_hashStrType.Add("QUOTE", FieldType.FieldQuote);
    FieldTypeDefiner.m_hashStrType.Add("REF", FieldType.FieldRef);
    FieldTypeDefiner.m_hashStrType.Add("RD", FieldType.FieldRefDoc);
    FieldTypeDefiner.m_hashStrType.Add("REVNUM", FieldType.FieldRevisionNum);
    FieldTypeDefiner.m_hashStrType.Add("SAVEDATE", FieldType.FieldSaveDate);
    FieldTypeDefiner.m_hashStrType.Add("SECTION", FieldType.FieldSection);
    FieldTypeDefiner.m_hashStrType.Add("SECTIONPAGES", FieldType.FieldSectionPages);
    FieldTypeDefiner.m_hashStrType.Add("SEQ", FieldType.FieldSequence);
    FieldTypeDefiner.m_hashStrType.Add("SET", FieldType.FieldSet);
    FieldTypeDefiner.m_hashStrType.Add("SKIPIF", FieldType.FieldSkipIf);
    FieldTypeDefiner.m_hashStrType.Add("STYLEREF", FieldType.FieldStyleRef);
    FieldTypeDefiner.m_hashStrType.Add("SUBJECT", FieldType.FieldSubject);
    FieldTypeDefiner.m_hashStrType.Add("SYMBOL", FieldType.FieldSymbol);
    FieldTypeDefiner.m_hashStrType.Add("TEMPLATE", FieldType.FieldTemplate);
    FieldTypeDefiner.m_hashStrType.Add("TIME", FieldType.FieldTime);
    FieldTypeDefiner.m_hashStrType.Add("TITLE", FieldType.FieldTitle);
    FieldTypeDefiner.m_hashStrType.Add("TOA", FieldType.FieldTOA);
    FieldTypeDefiner.m_hashStrType.Add("TA", FieldType.FieldTOAEntry);
    FieldTypeDefiner.m_hashStrType.Add("TOC", FieldType.FieldTOC);
    FieldTypeDefiner.m_hashStrType.Add("TC", FieldType.FieldTOCEntry);
    FieldTypeDefiner.m_hashStrType.Add("USERADDRESS", FieldType.FieldUserAddress);
    FieldTypeDefiner.m_hashStrType.Add("USERINITIALS", FieldType.FieldUserInitials);
    FieldTypeDefiner.m_hashStrType.Add("USERNAME", FieldType.FieldUserName);
    FieldTypeDefiner.m_hashStrType.Add("XE", FieldType.FieldIndexEntry);
    FieldTypeDefiner.m_hashStrType.Add("SHAPE", FieldType.FieldShape);
    FieldTypeDefiner.m_hashStrType.Add("ADDIN", FieldType.FieldAddin);
    FieldTypeDefiner.m_hashStrType.Add("FORMCHECKBOX", FieldType.FieldFormCheckBox);
    FieldTypeDefiner.m_hashStrType.Add("FORMDROPDOWN", FieldType.FieldFormDropDown);
    FieldTypeDefiner.m_hashStrType.Add("FORMTEXT", FieldType.FieldFormTextInput);
    FieldTypeDefiner.m_hashStrType.Add("CONTROL", FieldType.FieldOCX);
    FieldTypeDefiner.m_hashStrType.Add("EMBED", FieldType.FieldEmbed);
    FieldTypeDefiner.m_hashStrType.Add("ADDRESSBLOCK", FieldType.FieldAddressBlock);
    FieldTypeDefiner.m_hashStrType.Add("BIDIOUTLINE", FieldType.FieldBidiOutline);
  }

  private static void InitTypeStrHash()
  {
    if (FieldTypeDefiner.m_hashStrType != null)
    {
      FieldTypeDefiner.m_hashStrType.Clear();
      FieldTypeDefiner.m_hashStrType = (Dictionary<string, FieldType>) null;
    }
    FieldTypeDefiner.m_hashTypeStr = new Dictionary<FieldType, string>();
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldFormula, "=");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldAdvance, "ADVANCE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldAsk, "ASK");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldAuthor, "AUTHOR");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldAutoNum, "AUTONUM");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldAutoNumLegal, "AUTONUMLGL");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldAutoNumOutline, "AUTONUMOUT");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldAutoText, "AUTOTEXT");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldAutoTextList, "AUTOTEXTLIST");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldBarCode, "BARCODE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldComments, "COMMENTS");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldCompare, "COMPARE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldCreateDate, "CREATEDATE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldDatabase, "DATABASE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldDate, "DATE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldDocProperty, "DOCPROPERTY");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldDocVariable, "DOCVARIABLE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldEditTime, "EDITTIME");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldExpression, "EQ");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldFileName, "FILENAME");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldFileSize, "FILESIZE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldFillIn, "FILLIN");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldGoToButton, "GOTOBUTTON");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldHyperlink, "HYPERLINK");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldIf, "IF");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldIncludeText, "INCLUDETEXT");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldIncludePicture, "INCLUDEPICTURE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldIndex, "INDEX");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldInfo, "INFO");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldKeyWord, "KEYWORDS");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldLastSavedBy, "LASTSAVEDBY");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldLink, "LINK");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldListNum, "LISTNUM");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldMacroButton, "MACROBUTTON");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldMergeField, "MERGEFIELD");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldMergeRec, "MERGEREC");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldMergeSeq, "MERGESEQ");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldNext, "NEXT");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldNextIf, "NEXTIF");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldNoteRef, "NOTEREF");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldNumChars, "NUMCHARS");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldNumPages, "NUMPAGES");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldNumWords, "NUMWORDS");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldPage, "PAGE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldPageRef, "PAGEREF");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldPrint, "PRINT");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldPrintDate, "PRINTDATE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldPrivate, "PRIVATE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldQuote, "QUOTE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldRef, "REF");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldRefDoc, "RD");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldRevisionNum, "REVNUM");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldSaveDate, "SAVEDATE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldSection, "SECTION");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldSectionPages, "SECTIONPAGES");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldSequence, "SEQ");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldSet, "SET");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldSkipIf, "SKIPIF");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldStyleRef, "STYLEREF");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldSubject, "SUBJECT");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldSymbol, "SYMBOL");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldTemplate, "TEMPLATE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldTime, "TIME");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldTitle, "TITLE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldTOA, "TOA");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldTOAEntry, "TA");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldTOC, "TOC");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldTOCEntry, "TC");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldUserAddress, "USERADDRESS");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldUserInitials, "USERINITIALS");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldUserName, "USERNAME");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldIndexEntry, "XE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldShape, "SHAPE");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldAddin, "ADDIN");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldFormCheckBox, "FORMCHECKBOX");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldFormDropDown, "FORMDROPDOWN");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldFormTextInput, "FORMTEXT");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldOCX, "CONTROL");
    FieldTypeDefiner.m_hashTypeStr.Add(FieldType.FieldEmbed, "EMBED");
  }
}
