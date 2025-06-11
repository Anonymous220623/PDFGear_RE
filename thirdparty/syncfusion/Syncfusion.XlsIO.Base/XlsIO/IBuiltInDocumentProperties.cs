// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IBuiltInDocumentProperties
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using System;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IBuiltInDocumentProperties
{
  IDocumentProperty this[ExcelBuiltInProperty index] { get; }

  IDocumentProperty this[int iIndex] { get; }

  int Count { get; }

  void Clear();

  bool Contains(ExcelBuiltInProperty index);

  string Title { get; set; }

  string Subject { get; set; }

  string Author { get; set; }

  string Keywords { get; set; }

  string Comments { get; set; }

  string Template { get; set; }

  string LastAuthor { get; set; }

  string RevisionNumber { get; set; }

  TimeSpan EditTime { get; set; }

  DateTime LastPrinted { get; set; }

  DateTime CreationDate { get; set; }

  DateTime LastSaveDate { get; set; }

  int PageCount { get; set; }

  int WordCount { get; set; }

  int CharCount { get; set; }

  string ApplicationName { get; set; }

  string Category { get; set; }

  string PresentationTarget { get; set; }

  int ByteCount { get; set; }

  int LineCount { get; set; }

  int ParagraphCount { get; set; }

  int SlideCount { get; set; }

  int NoteCount { get; set; }

  int HiddenCount { get; set; }

  int MultimediaClipCount { get; set; }

  string Manager { get; set; }

  string Company { get; set; }

  bool LinksDirty { get; set; }
}
