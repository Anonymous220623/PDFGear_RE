// Decompiled with JetBrains decompiler
// Type: PDFKit.PdfViewerDecorators.IPdfViewerDecorator
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

#nullable disable
namespace PDFKit.PdfViewerDecorators;

internal interface IPdfViewerDecorator
{
  bool CanDrawPdfBitmap(PdfViewerDecoratorDrawingArgs args);

  void DrawPdfBitmap(PdfViewerDecoratorDrawingArgs args);

  bool CanDrawVisual(PdfViewerDecoratorDrawingArgs args);

  void DrawVisual(PdfViewerDecoratorDrawingArgs args);
}
