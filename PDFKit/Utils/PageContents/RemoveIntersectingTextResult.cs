// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PageContents.RemoveIntersectingTextResult
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net;
using System;

#nullable disable
namespace PDFKit.Utils.PageContents;

public class RemoveIntersectingTextResult
{
  internal RemoveIntersectingTextResult(
    bool success,
    System.Collections.Generic.IReadOnlyList<PdfTextObject> newTextObjects,
    System.Collections.Generic.IReadOnlyList<PdfTextObject> notIntersectingTextObjects)
  {
    this.Success = success;
    this.NewTextObjects = (System.Collections.Generic.IReadOnlyList<PdfTextObject>) ((object) newTextObjects ?? (object) Array.Empty<PdfTextObject>());
    this.NotIntersectingTextObjects = (System.Collections.Generic.IReadOnlyList<PdfTextObject>) ((object) notIntersectingTextObjects ?? (object) Array.Empty<PdfTextObject>());
  }

  public bool Success { get; }

  public System.Collections.Generic.IReadOnlyList<PdfTextObject> NewTextObjects { get; }

  public System.Collections.Generic.IReadOnlyList<PdfTextObject> NotIntersectingTextObjects { get; }
}
