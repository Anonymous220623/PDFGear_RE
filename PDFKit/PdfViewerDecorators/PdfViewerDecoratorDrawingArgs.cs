// Decompiled with JetBrains decompiler
// Type: PDFKit.PdfViewerDecorators.PdfViewerDecoratorDrawingArgs
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace PDFKit.PdfViewerDecorators;

internal class PdfViewerDecoratorDrawingArgs
{
  private Dictionary<string, object> context;

  internal PdfViewer Viewer { get; set; }

  internal PdfPage PdfPage { get; set; }

  internal DrawingContext DrawingContext { get; set; }

  internal PdfBitmap PdfBitmap { get; set; }

  internal Rect PageInViewerActualRect { get; set; }

  internal void Reset()
  {
    this.ClearContext();
    this.PdfPage = (PdfPage) null;
    this.DrawingContext = (DrawingContext) null;
    this.PdfBitmap = (PdfBitmap) null;
    this.PageInViewerActualRect = new Rect();
  }

  internal void ClearContext() => this.context?.Clear();

  internal bool GetContext<T>(string name, out T value)
  {
    value = default (T);
    object obj1;
    if (string.IsNullOrEmpty(name) || this.context == null || !this.context.TryGetValue(name, out obj1))
      return false;
    if (obj1 is T obj2)
    {
      value = obj2;
      return true;
    }
    if (obj1 == null || !PdfViewerDecoratorDrawingArgs.IsNumber(obj1.GetType()) || !PdfViewerDecoratorDrawingArgs.IsNumber(typeof (T)))
      return false;
    value = (T) Convert.ChangeType(obj1, typeof (T));
    return true;
  }

  internal void SetContext<T>(string name, T value)
  {
    if (string.IsNullOrEmpty(name))
      return;
    if (this.context == null)
      this.context = new Dictionary<string, object>();
    this.context[name] = (object) value;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static bool IsNumber(Type type)
  {
    return type.IsValueType && (type == typeof (byte) || type == typeof (char) || type == typeof (Decimal) || type == typeof (double) || type == typeof (short) || type == typeof (int) || type == typeof (long) || type == typeof (sbyte) || type == typeof (float) || type == typeof (ushort) || type == typeof (uint) || type == typeof (ulong));
  }
}
