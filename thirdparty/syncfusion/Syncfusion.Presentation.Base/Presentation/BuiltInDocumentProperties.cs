// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.BuiltInDocumentProperties
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation;

internal class BuiltInDocumentProperties : IBuiltInDocumentProperties
{
  public static readonly Guid GuidSummary = new Guid("F29F85E0-4FF9-1068-AB91-08002B27B3D9");
  public static readonly Guid GuidDocument = new Guid("D5CDD502-2E9C-101B-9397-08002B2CF9AE");
  private Dictionary<int, DocumentPropertyImpl> _documentHash = new Dictionary<int, DocumentPropertyImpl>();
  private Dictionary<int, DocumentPropertyImpl> _summaryHash = new Dictionary<int, DocumentPropertyImpl>();
  private Syncfusion.Presentation.Presentation _presentation;
  private string _applicationName;
  private string _title;
  private string _subject;
  private string _author;
  private string _keywords;
  private string _comments;
  private string _template;
  private string _lastAuthor;
  private string _revisionNumber;
  private TimeSpan _editTime;
  private DateTime _lastPrinted;
  private DateTime _creationDate;
  private DateTime _lastSaveDate = DateTime.Now;
  private int _pageCount;
  private int _wordCount;
  private int _charCount;
  private bool _hasHeadingPair;
  private string _category;
  private string _presentationTarget;
  private int _byteCount;
  private int _lineCount;
  private int _paragraphCount;
  private int _slideCount;
  private int _noteCount;
  private int _hiddenCount;
  private int _multimediaClipCount;
  private bool _scaleCrop;
  private string _manager;
  private string _company;
  private bool _linksDirty;
  private string _contentStatus;
  private string _language;
  private string _version;

  public BuiltInDocumentProperties(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
  }

  public string Version
  {
    get => this._version;
    set => this._version = value;
  }

  public string ContentStatus
  {
    get => this._contentStatus;
    set => this._contentStatus = value;
  }

  public string Language
  {
    get => this._language;
    set => this._language = value;
  }

  public string Title
  {
    get => this._title;
    set => this._title = value;
  }

  public string Subject
  {
    get => this._subject;
    set => this._subject = value;
  }

  public string Author
  {
    get => this._author;
    set => this._author = value;
  }

  public string Keywords
  {
    get => this._keywords;
    set => this._keywords = value;
  }

  public string Comments
  {
    get => this._comments;
    set => this._comments = value;
  }

  public string Template
  {
    get => this._template;
    set => this._template = value;
  }

  public string LastAuthor
  {
    get => this._lastAuthor;
    set => this._lastAuthor = value;
  }

  public string RevisionNumber
  {
    get => this._revisionNumber;
    set => this._revisionNumber = value;
  }

  public TimeSpan EditTime
  {
    get => this._editTime;
    set
    {
      this._editTime = value.TotalMinutes <= (double) uint.MaxValue ? value : throw new ArgumentException("Invalid EditTime Value.");
    }
  }

  public DateTime LastPrinted
  {
    get => this._lastPrinted;
    set => this._lastPrinted = value;
  }

  public DateTime CreationDate
  {
    get => this._creationDate;
    set => this._creationDate = value;
  }

  public DateTime LastSaveDate
  {
    get => this._lastSaveDate;
    set => this._lastSaveDate = value;
  }

  public int PageCount
  {
    get => this._pageCount;
    set => this._pageCount = value;
  }

  public int WordCount
  {
    get => this._wordCount;
    set => this._wordCount = value;
  }

  public int CharCount
  {
    get => this._charCount;
    set => this._charCount = value;
  }

  internal bool HasHeadingPair
  {
    get => this._hasHeadingPair;
    set => this._hasHeadingPair = value;
  }

  public string Category
  {
    get => this._category;
    set => this._category = value;
  }

  public string PresentationTarget
  {
    get => this._presentationTarget;
    set => this._presentationTarget = value;
  }

  public int ByteCount
  {
    get => this._byteCount;
    set => this._byteCount = value;
  }

  public int LineCount
  {
    get => this._lineCount;
    set => this._lineCount = value;
  }

  public int ParagraphCount
  {
    get => this._paragraphCount;
    set => this._paragraphCount = value;
  }

  public int SlideCount
  {
    get => this._slideCount;
    set => this._slideCount = value;
  }

  public int NoteCount
  {
    get => this._noteCount;
    set => this._noteCount = value;
  }

  public int HiddenCount
  {
    get => this._hiddenCount;
    set => this._hiddenCount = value;
  }

  public int MultimediaClipCount
  {
    get => this._multimediaClipCount;
    set => this._multimediaClipCount = value;
  }

  public bool ScaleCrop
  {
    get => this._scaleCrop;
    set => this._scaleCrop = value;
  }

  public string Manager
  {
    get => this._manager;
    set => this._manager = value;
  }

  public string Company
  {
    get => this._company;
    set => this._company = value;
  }

  public bool LinksDirty
  {
    get => this._linksDirty;
    set => this._linksDirty = value;
  }

  public string ApplicationName
  {
    get => this._applicationName;
    set => this._applicationName = value;
  }

  internal void Close()
  {
    if (this._documentHash != null)
    {
      this._documentHash.Clear();
      this._documentHash = (Dictionary<int, DocumentPropertyImpl>) null;
    }
    if (this._summaryHash == null)
      return;
    this._summaryHash.Clear();
    this._summaryHash = (Dictionary<int, DocumentPropertyImpl>) null;
  }

  public IBuiltInDocumentProperties Clone()
  {
    BuiltInDocumentProperties documentProperties = (BuiltInDocumentProperties) this.MemberwiseClone();
    documentProperties._creationDate = this._creationDate;
    if (this._documentHash != null)
      documentProperties._documentHash = this.CloneHash(this._documentHash);
    documentProperties._editTime = this._editTime;
    documentProperties._lastPrinted = this._lastPrinted;
    documentProperties._lastSaveDate = this._lastSaveDate;
    if (this._summaryHash != null)
      documentProperties._summaryHash = this.CloneHash(this._summaryHash);
    return (IBuiltInDocumentProperties) documentProperties;
  }

  private Dictionary<int, DocumentPropertyImpl> CloneHash(
    Dictionary<int, DocumentPropertyImpl> dictionary)
  {
    Dictionary<int, DocumentPropertyImpl> dictionary1 = new Dictionary<int, DocumentPropertyImpl>();
    foreach (KeyValuePair<int, DocumentPropertyImpl> keyValuePair in dictionary)
    {
      int key = keyValuePair.Key;
      DocumentPropertyImpl documentPropertyImpl = (DocumentPropertyImpl) keyValuePair.Value.Clone();
      dictionary1.Add(key, documentPropertyImpl);
    }
    return dictionary1;
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
  }
}
