// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.IBuiltInDocumentProperties
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;

#nullable disable
namespace Syncfusion.Presentation;

public interface IBuiltInDocumentProperties
{
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

  string ApplicationName { get; set; }

  string ContentStatus { get; set; }

  string Language { get; set; }

  string Version { get; set; }
}
