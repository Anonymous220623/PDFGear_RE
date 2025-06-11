// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Bookmarks.BookmarkRecord
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using System.Collections.Generic;

#nullable disable
namespace pdfeditor.Models.Bookmarks;

public class BookmarkRecord
{
  public int Index { get; set; }

  public string Title { get; set; }

  public BookmarkRecord Parent { get; set; }

  public IList<BookmarkRecord> Childs { get; set; } = (IList<BookmarkRecord>) new List<BookmarkRecord>();

  public BookmarkRecord.BookmarkDestination Destination { get; set; }

  public PdfAction Action { get; set; }

  public class BookmarkDestination
  {
    public string Name { get; set; }

    public int PageIndex { get; set; }

    public DestinationTypes DestinationType { get; set; }

    public float? Left { get; set; }

    public float? Top { get; set; }

    public float? Right { get; set; }

    public float? Bottom { get; set; }

    public float? Zoom { get; set; }
  }
}
