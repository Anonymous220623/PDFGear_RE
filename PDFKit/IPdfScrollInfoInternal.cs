// Decompiled with JetBrains decompiler
// Type: PDFKit.IPdfScrollInfoInternal
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net;
using System.Windows;

#nullable disable
namespace PDFKit;

public interface IPdfScrollInfoInternal
{
  bool CanVerticallyScroll { get; }

  bool CanHorizontallyScroll { get; }

  double ExtentWidth { get; }

  double ExtentHeight { get; }

  double ViewportWidth { get; }

  double ViewportHeight { get; }

  double HorizontalOffset { get; }

  double VerticalOffset { get; }

  float Zoom { get; }

  PdfDocument Document { get; }

  int StartPage { get; }

  int EndPage { get; }

  Rect CalcActualRect(int index);

  Point ClientToPage(int pageIndex, Point pt);

  FS_RECTF ClientToPageRect(Rect rect, int pageIndex);

  Point PageToClient(int pageIndex, Point pt);

  Rect PageToClientRect(FS_RECTF rc, int pageIndex);

  int DeviceToPage(double x, double y, out Point pagePoint);

  void ScrollToPage(int index);
}
