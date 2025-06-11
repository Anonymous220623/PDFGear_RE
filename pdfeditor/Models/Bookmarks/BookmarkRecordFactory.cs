// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Bookmarks.BookmarkRecordFactory
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace pdfeditor.Models.Bookmarks;

public static class BookmarkRecordFactory
{
  private static Func<PdfDocument, IntPtr, string, PdfDestination> createPdfDestinationFunc;
  private static object locker = new object();

  public static BookmarkRecord CreateRecord(PdfDocument doc, PdfBookmark bookmark)
  {
    if (bookmark == null)
      return (BookmarkRecord) null;
    int num = (bookmark.Parent?.Childs ?? doc.Bookmarks).IndexOf(bookmark);
    if (num == -1)
      return (BookmarkRecord) null;
    BookmarkRecord record1 = new BookmarkRecord()
    {
      Index = num,
      Title = bookmark.Title,
      Action = bookmark.Action,
      Destination = BookmarkRecordFactory.CreateDestination(bookmark.Destination)
    };
    if (bookmark.Childs != null && bookmark.Childs.Count > 0)
    {
      foreach (PdfBookmark child in bookmark.Childs)
      {
        BookmarkRecord record2 = BookmarkRecordFactory.CreateRecord(doc, child);
        if (record2 != null)
        {
          record2.Parent = record1;
          record1.Childs.Add(record2);
        }
      }
    }
    return record1;
  }

  public static PdfBookmark Insert(
    PdfDocument doc,
    BookmarkRecord record,
    PdfBookmarkCollections collection)
  {
    if (record == null || collection == null)
      return (PdfBookmark) null;
    PdfBookmark pdfBookmark = (PdfBookmark) null;
    if (record.Destination != null)
    {
      PdfDestination destination = new PdfDestination(record.Destination.PageIndex < 0 || doc == null || record.Destination.PageIndex >= doc.Pages.Count ? (PdfDocument) null : doc, record.Destination.Name)
      {
        DestinationType = record.Destination.DestinationType,
        PageIndex = record.Destination.PageIndex,
        Left = record.Destination.Left,
        Top = record.Destination.Top,
        Right = record.Destination.Right,
        Bottom = record.Destination.Bottom,
        Zoom = record.Destination.Zoom
      };
      pdfBookmark = collection.InsertAt(record.Index, record.Title, destination);
    }
    else if (record.Action != null)
    {
      int num = Pdfium.FPDFOBJ_GetParentObj(record.Action.Dictionary.Handle) != IntPtr.Zero ? 1 : 0;
      pdfBookmark = collection.InsertAt(record.Index, record.Title, record.Action);
    }
    if (record.Childs != null && record.Childs.Count > 0)
    {
      foreach (BookmarkRecord child in (IEnumerable<BookmarkRecord>) record.Childs)
        BookmarkRecordFactory.Insert(doc, child, pdfBookmark.Childs);
    }
    return pdfBookmark;
  }

  public static void Remove(
    PdfDocument doc,
    PdfBookmark pdfBookmark,
    PdfBookmarkCollections collection)
  {
    pdfBookmark.Action = (PdfAction) null;
    if (pdfBookmark.Childs != null && pdfBookmark.Childs.Count > 0)
    {
      foreach (PdfBookmark child in pdfBookmark.Childs)
        BookmarkRecordFactory.Remove(doc, child, pdfBookmark.Childs);
    }
    PdfTypeDictionary pdfTypeDictionary = pdfBookmark.Parent?.Dictionary ?? (doc.Root["Outlines"] as PdfTypeIndirect).Direct as PdfTypeDictionary;
    if (pdfTypeDictionary != null && !pdfTypeDictionary.ContainsKey("Count"))
      pdfTypeDictionary["Count"] = (PdfTypeBase) PdfTypeNumber.Create(collection.Count);
    collection.Remove(pdfBookmark);
  }

  private static BookmarkRecord.BookmarkDestination CreateDestination(PdfDestination destination)
  {
    if (destination == null)
      return (BookmarkRecord.BookmarkDestination) null;
    return new BookmarkRecord.BookmarkDestination()
    {
      DestinationType = destination.DestinationType,
      PageIndex = destination.PageIndex,
      Name = destination.Name,
      Left = destination.Left,
      Top = destination.Top,
      Right = destination.Right,
      Bottom = destination.Bottom,
      Zoom = destination.Zoom
    };
  }

  private static PdfDestination CreatePdfDestination(PdfDocument doc, IntPtr handle, string name = null)
  {
    if (BookmarkRecordFactory.createPdfDestinationFunc == null)
    {
      lock (BookmarkRecordFactory.locker)
      {
        if (BookmarkRecordFactory.createPdfDestinationFunc == null)
        {
          ConstructorInfo constructor = typeof (PdfDestination).GetConstructor(BindingFlags.NonPublic, (Binder) null, new Type[3]
          {
            typeof (PdfDocument),
            typeof (IntPtr),
            typeof (string)
          }, (ParameterModifier[]) null);
          if (constructor != (ConstructorInfo) null)
          {
            ParameterExpression parameterExpression4 = Expression.Parameter(typeof (PdfDocument), nameof (doc));
            ParameterExpression parameterExpression5 = Expression.Parameter(typeof (IntPtr), nameof (handle));
            ParameterExpression parameterExpression6 = Expression.Parameter(typeof (string), nameof (name));
            Expression expression = (Expression) Expression.Convert((Expression) Expression.New(constructor, (Expression) parameterExpression4, (Expression) parameterExpression5, (Expression) parameterExpression6), typeof (PdfDestination));
            BookmarkRecordFactory.createPdfDestinationFunc = ((Expression<Func<PdfDocument, IntPtr, string, PdfDestination>>) ((parameterExpression1, parameterExpression2, parameterExpression3) => Expression.Return(Expression.Label(), expression))).Compile();
          }
          else
            BookmarkRecordFactory.createPdfDestinationFunc = (Func<PdfDocument, IntPtr, string, PdfDestination>) ((_p1, _p2, _p3) => (PdfDestination) null);
        }
      }
    }
    return BookmarkRecordFactory.createPdfDestinationFunc(doc, handle, name);
  }
}
