// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.ODFParagraph
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class ODFParagraph
{
  private string m_styleName;
  private string m_alphabeticalIndexMark;
  private string m_alphabeticalIndexMarkStart;
  private string m_alphabeticalIndexMarkEnd;
  private string m_authorInitals;
  private string m_authorName;
  private string m_bibliographyMark;
  private string m_bookMark;
  private string m_bookMarkStart;
  private string m_bookMarkEnd;
  private string m_bookMarkRef;
  private string m_change;
  private string m_changeStart;
  private string m_changeEnd;
  private string m_chapter;
  private string m_chapterCount;
  private string m_conditionalText;
  private DateTime m_creationDate;
  private DateTime m_creationTime;
  private string m_creator;
  private string m_databaseDisplay;
  private string m_databaseName;
  private string m_databaseNext;
  private string m_databaseRowNumber;
  private string m_databaseRowSelect;
  private DateTime m_date;
  private string m_ddeConnection;
  private string m_description;
  private string m_editingCycles;
  private string m_editingDuration;
  private string m_executeMacro;
  private string m_expression;
  private string m_fileName;
  private string m_hiddenParagraph;
  private string m_hiddenText;
  private int m_imageCount;
  private string m_initialCreator;
  private string m_keywords;
  private string m_lineBreak;
  private DateTime m_modificationDate;
  private DateTime m_modificationTime;
  private string m_modificationNote;
  private string m_modificationNoteRef;
  private int m_objectCount;
  private string m_pageContinuation;
  private int m_pageCount;
  private PageNumber m_pageNumber;
  private string m_setvariable;
  private int m_paragraphCount;
  private string m_placeHolder;
  private DateTime m_printDate;
  private string m_printedBy;
  private DateTime m_printTime;
  private string m_referenceMark;
  private string m_referenceMarkStart;
  private string m_referenceMarkEnd;
  private string m_referenceRef;
  private string m_ruby;
  private string m_script;
  private string m_senderCity;
  private string m_senderCompany;
  private string m_senderCountry;
  private string m_senderEmail;
  private string m_senderFax;
  private string m_senderFirstName;
  private string m_senderinitials;
  private string m_senderLastName;
  private string m_senderPhonePrivate;
  private string m_senderPhoneWork;
  private string m_senderPosition;
  private string m_senderPostalCode;
  private string m_stateOrProvince;
  private string m_senderStreet;
  private string m_senderTitle;
  private string m_sequence;
  private string m_sequenceRef;
  private string m_sheetName;
  private string m_softPageBreak;
  private string m_span;
  private string m_subject;
  private string m_tab;
  private int m_tableCount;
  private string m_tableFormula;
  private string m_templateName;
  private string m_textInput;
  private DateTime m_time;
  private string m_title;
  private string m_tocMark;
  private string m_tocMarkStart;
  private string m_tocMarkEnd;
  private string m_userDefined;
  private string m_userFieldGet;
  private string m_userFieldInput;
  private string m_userIndexMark;
  private string m_getVariable;
  private string m_setVariable;
  private string m_getVariableInput;
  private int m_wordCount;

  internal int WordCount
  {
    get => this.m_wordCount;
    set => this.m_wordCount = value;
  }

  internal string GetVariableInput
  {
    get => this.m_getVariableInput;
    set => this.m_getVariableInput = value;
  }

  internal string SetVariable
  {
    get => this.m_setVariable;
    set => this.m_setVariable = value;
  }

  internal string GetVariable1
  {
    get => this.m_getVariable;
    set => this.m_getVariable = value;
  }

  internal string UserIndexMark
  {
    get => this.m_userIndexMark;
    set => this.m_userIndexMark = value;
  }

  internal string UserFieldInput
  {
    get => this.m_userFieldInput;
    set => this.m_userFieldInput = value;
  }

  internal string UserFieldGet
  {
    get => this.m_userFieldGet;
    set => this.m_userFieldGet = value;
  }

  internal string UserDefined
  {
    get => this.m_userDefined;
    set => this.m_userDefined = value;
  }

  internal string TocMarkEnd
  {
    get => this.m_tocMarkEnd;
    set => this.m_tocMarkEnd = value;
  }

  internal string TocMarkStart
  {
    get => this.m_tocMarkStart;
    set => this.m_tocMarkStart = value;
  }

  internal string TocMark
  {
    get => this.m_tocMark;
    set => this.m_tocMark = value;
  }

  internal string Title
  {
    get => this.m_title;
    set => this.m_title = value;
  }

  internal DateTime Time
  {
    get => this.m_time;
    set => this.m_time = value;
  }

  internal string TextInput
  {
    get => this.m_textInput;
    set => this.m_textInput = value;
  }

  internal string TemplateName
  {
    get => this.m_templateName;
    set => this.m_templateName = value;
  }

  internal string TableFormula
  {
    get => this.m_tableFormula;
    set => this.m_tableFormula = value;
  }

  internal int TableCount
  {
    get => this.m_tableCount;
    set => this.m_tableCount = value;
  }

  internal string Tab
  {
    get => this.m_tab;
    set => this.m_tab = value;
  }

  internal string Subject
  {
    get => this.m_subject;
    set => this.m_subject = value;
  }

  internal string Span
  {
    get => this.m_span;
    set => this.m_span = value;
  }

  internal string SheetName
  {
    get => this.m_sheetName;
    set => this.m_sheetName = value;
  }

  internal string SoftPageBreak
  {
    get => this.m_softPageBreak;
    set => this.m_softPageBreak = value;
  }

  internal string SequenceRef
  {
    get => this.m_sequenceRef;
    set => this.m_sequenceRef = value;
  }

  internal string Sequence
  {
    get => this.m_sequence;
    set => this.m_sequence = value;
  }

  internal string SenderTitle
  {
    get => this.m_senderTitle;
    set => this.m_senderTitle = value;
  }

  internal string SenderStreet
  {
    get => this.m_senderStreet;
    set => this.m_senderStreet = value;
  }

  internal string StateOrProvince
  {
    get => this.m_stateOrProvince;
    set => this.m_stateOrProvince = value;
  }

  internal string SenderPosition
  {
    get => this.m_senderPosition;
    set => this.m_senderPosition = value;
  }

  internal string SenderPostalCode
  {
    get => this.m_senderPostalCode;
    set => this.m_senderPostalCode = value;
  }

  internal string SenderPhoneWork
  {
    get => this.m_senderPhoneWork;
    set => this.m_senderPhoneWork = value;
  }

  internal string SenderPhonePrivate
  {
    get => this.m_senderPhonePrivate;
    set => this.m_senderPhonePrivate = value;
  }

  internal string SenderLastName
  {
    get => this.m_senderLastName;
    set => this.m_senderLastName = value;
  }

  internal string Senderinitials
  {
    get => this.m_senderinitials;
    set => this.m_senderinitials = value;
  }

  internal string SenderFirstName
  {
    get => this.m_senderFirstName;
    set => this.m_senderFirstName = value;
  }

  internal string SenderFax
  {
    get => this.m_senderFax;
    set => this.m_senderFax = value;
  }

  internal string SenderEmail
  {
    get => this.m_senderEmail;
    set => this.m_senderEmail = value;
  }

  internal string SenderCountry
  {
    get => this.m_senderCountry;
    set => this.m_senderCountry = value;
  }

  internal string SenderCompany
  {
    get => this.m_senderCompany;
    set => this.m_senderCompany = value;
  }

  internal string SenderCity
  {
    get => this.m_senderCity;
    set => this.m_senderCity = value;
  }

  internal string Script
  {
    get => this.m_script;
    set => this.m_script = value;
  }

  internal string Ruby
  {
    get => this.m_ruby;
    set => this.m_ruby = value;
  }

  internal string ReferenceRef
  {
    get => this.m_referenceRef;
    set => this.m_referenceRef = value;
  }

  internal string ReferenceMarkEnd
  {
    get => this.m_referenceMarkEnd;
    set => this.m_referenceMarkEnd = value;
  }

  internal string ReferenceMarkStart
  {
    get => this.m_referenceMarkStart;
    set => this.m_referenceMarkStart = value;
  }

  internal string ReferenceMark
  {
    get => this.m_referenceMark;
    set => this.m_referenceMark = value;
  }

  internal DateTime PrintTime
  {
    get => this.m_printTime;
    set => this.m_printTime = value;
  }

  internal string PrintedBy
  {
    get => this.m_printedBy;
    set => this.m_printedBy = value;
  }

  internal DateTime PrintDate
  {
    get => this.m_printDate;
    set => this.m_printDate = value;
  }

  internal string PlaceHolder
  {
    get => this.m_placeHolder;
    set => this.m_placeHolder = value;
  }

  internal int ParagraphCount
  {
    get => this.m_paragraphCount;
    set => this.m_paragraphCount = value;
  }

  internal string Setvariable
  {
    get => this.m_setvariable;
    set => this.m_setvariable = value;
  }

  internal string GetVariable
  {
    get => this.m_getVariable;
    set => this.m_getVariable = value;
  }

  internal PageNumber PageNumber
  {
    get => this.m_pageNumber;
    set => this.m_pageNumber = value;
  }

  internal int PageCount
  {
    get => this.m_pageCount;
    set => this.m_pageCount = value;
  }

  internal string PageContinuation
  {
    get => this.m_pageContinuation;
    set => this.m_pageContinuation = value;
  }

  internal int ObjectCount
  {
    get => this.m_objectCount;
    set => this.m_objectCount = value;
  }

  internal string ModificationNoteRef
  {
    get => this.m_modificationNoteRef;
    set => this.m_modificationNoteRef = value;
  }

  internal string ModificationNote
  {
    get => this.m_modificationNote;
    set => this.m_modificationNote = value;
  }

  internal DateTime ModificationTime
  {
    get => this.m_modificationTime;
    set => this.m_modificationTime = value;
  }

  internal DateTime ModificationDate
  {
    get => this.m_modificationDate;
    set => this.m_modificationDate = value;
  }

  internal string LineBreak
  {
    get => this.m_lineBreak;
    set => this.m_lineBreak = value;
  }

  internal string Keywords
  {
    get => this.m_keywords;
    set => this.m_keywords = value;
  }

  internal string InitialCreator
  {
    get => this.m_initialCreator;
    set => this.m_initialCreator = value;
  }

  internal int ImageCount
  {
    get => this.m_imageCount;
    set => this.m_imageCount = value;
  }

  internal string HiddenText
  {
    get => this.m_hiddenText;
    set => this.m_hiddenText = value;
  }

  internal string HiddenParagraph
  {
    get => this.m_hiddenParagraph;
    set => this.m_hiddenParagraph = value;
  }

  internal string FileName
  {
    get => this.m_fileName;
    set => this.m_fileName = value;
  }

  internal string Expression
  {
    get => this.m_expression;
    set => this.m_expression = value;
  }

  internal string ExecuteMacro
  {
    get => this.m_executeMacro;
    set => this.m_executeMacro = value;
  }

  internal string EditingDuration
  {
    get => this.m_editingDuration;
    set => this.m_editingDuration = value;
  }

  internal string EditingCycles
  {
    get => this.m_editingCycles;
    set => this.m_editingCycles = value;
  }

  internal string Description
  {
    get => this.m_description;
    set => this.m_description = value;
  }

  internal string DdeConnection
  {
    get => this.m_ddeConnection;
    set => this.m_ddeConnection = value;
  }

  internal DateTime Date
  {
    get => this.m_date;
    set => this.m_date = value;
  }

  internal string DatabaseRowSelect
  {
    get => this.m_databaseRowSelect;
    set => this.m_databaseRowSelect = value;
  }

  internal string DatabaseRowNumber
  {
    get => this.m_databaseRowNumber;
    set => this.m_databaseRowNumber = value;
  }

  internal string DatabaseNext
  {
    get => this.m_databaseNext;
    set => this.m_databaseNext = value;
  }

  internal string DatabaseDisplay
  {
    get => this.m_databaseDisplay;
    set => this.m_databaseDisplay = value;
  }

  internal string Creator
  {
    get => this.m_creator;
    set => this.m_creator = value;
  }

  internal DateTime CreationTime
  {
    get => this.m_creationTime;
    set => this.m_creationTime = value;
  }

  internal DateTime CreationDate
  {
    get => this.m_creationDate;
    set => this.m_creationDate = value;
  }

  internal string ConditionalText
  {
    get => this.m_conditionalText;
    set => this.m_conditionalText = value;
  }

  internal string ChapterCount
  {
    get => this.m_chapterCount;
    set => this.m_chapterCount = value;
  }

  internal string Chapter
  {
    get => this.m_chapter;
    set => this.m_chapter = value;
  }

  internal string ChangeEnd
  {
    get => this.m_changeEnd;
    set => this.m_changeEnd = value;
  }

  internal string ChangeStart
  {
    get => this.m_changeStart;
    set => this.m_changeStart = value;
  }

  internal string Change
  {
    get => this.m_change;
    set => this.m_change = value;
  }

  internal string BookMarkRef
  {
    get => this.m_bookMarkRef;
    set => this.m_bookMarkRef = value;
  }

  internal string BookMarkEnd
  {
    get => this.m_bookMarkEnd;
    set => this.m_bookMarkEnd = value;
  }

  internal string BookMarkStart
  {
    get => this.m_bookMarkStart;
    set => this.m_bookMarkStart = value;
  }

  internal string BookMark
  {
    get => this.m_bookMark;
    set => this.m_bookMark = value;
  }

  internal string BibliographyMark
  {
    get => this.m_bibliographyMark;
    set => this.m_bibliographyMark = value;
  }

  internal string AuthorName
  {
    get => this.m_authorName;
    set => this.m_authorName = value;
  }

  internal string AuthorInitals
  {
    get => this.m_authorInitals;
    set => this.m_authorInitals = value;
  }

  internal string AlphabeticalIndexMarkEnd
  {
    get => this.m_alphabeticalIndexMarkEnd;
    set => this.m_alphabeticalIndexMarkEnd = value;
  }

  internal string AlphabeticalIndexMarkStart
  {
    get => this.m_alphabeticalIndexMarkStart;
    set => this.m_alphabeticalIndexMarkStart = value;
  }

  internal string AlphabeticalIndexMark
  {
    get => this.m_alphabeticalIndexMark;
    set => this.m_alphabeticalIndexMark = value;
  }

  internal string StyleName
  {
    get => this.m_styleName;
    set => this.m_styleName = value;
  }
}
