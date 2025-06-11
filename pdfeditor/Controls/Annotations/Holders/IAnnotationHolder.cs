// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.Holders.IAnnotationHolder
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using System;
using System.Threading.Tasks;

#nullable disable
namespace pdfeditor.Controls.Annotations.Holders;

public interface IAnnotationHolder
{
  bool IsTextMarkupAnnotation { get; }

  AnnotationCanvas AnnotationCanvas { get; }

  AnnotationHolderState State { get; }

  PdfPage CurrentPage { get; }

  PdfAnnotation SelectedAnnotation { get; }

  event EventHandler Canceled;

  event EventHandler StateChanged;

  event EventHandler SelectedAnnotationChanged;

  void Cancel();

  Task<System.Collections.Generic.IReadOnlyList<PdfAnnotation>> CompleteCreateNewAsync();

  void OnPageClientBoundsChanged();

  void ProcessCreateNew(PdfPage page, FS_POINTF pagePoint);

  void StartCreateNew(PdfPage page, FS_POINTF pagePoint);

  void Select(PdfAnnotation annotation, bool afterCreate);

  bool OnPropertyChanged(string propertyName);
}
