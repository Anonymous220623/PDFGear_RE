// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.Holders.UnderlineAnnotationHoder
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Annotations.Holders;

public class UnderlineAnnotationHoder(AnnotationCanvas annotationCanvas) : 
  TextMarkupAnnotationHolder<PdfUnderlineAnnotation>(annotationCanvas)
{
  public override System.Collections.Generic.IReadOnlyList<PdfUnderlineAnnotation> CreateAnnotation(
    PdfDocument document,
    SelectInfo selectInfo)
  {
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    TextInfo[] array = PdfTextMarkupAnnotationUtils.GetTextInfos(document, selectInfo).ToArray<TextInfo>();
    if (array.Length == 0)
      return (System.Collections.Generic.IReadOnlyList<PdfUnderlineAnnotation>) null;
    Color color = (Color) ColorConverter.ConvertFromString(requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.UnderlineStroke);
    FS_COLOR fsColor = new FS_COLOR((int) color.A, (int) color.R, (int) color.G, (int) color.B);
    List<PdfUnderlineAnnotation> annotation = new List<PdfUnderlineAnnotation>(array.Length);
    foreach (TextInfo textInfo in array)
    {
      PdfQuadPointsCollection pointsCollection = new PdfQuadPointsCollection();
      foreach (FS_RECTF normalizedRect in (IEnumerable<FS_RECTF>) PdfTextMarkupAnnotationUtils.GetNormalizedRects(this.AnnotationCanvas.PdfViewer, textInfo))
      {
        FS_QUADPOINTSF quadPoints = normalizedRect.ToQuadPoints(0.0f, 2f);
        pointsCollection.Add(quadPoints);
      }
      if (pointsCollection.Count > 0)
      {
        PdfPage page = document.Pages[textInfo.PageIndex];
        PdfUnderlineAnnotation underlineAnnotation = new PdfUnderlineAnnotation(page);
        underlineAnnotation.Color = fsColor;
        underlineAnnotation.Text = string.IsNullOrEmpty(Environment.UserName) ? "unknown" : Environment.UserName;
        underlineAnnotation.QuadPoints = pointsCollection;
        underlineAnnotation.Flags |= AnnotationFlags.Print;
        if (page.Annots == null)
          page.CreateAnnotations();
        if (page.Annots != null)
        {
          page.Annots.Add((PdfAnnotation) underlineAnnotation);
          underlineAnnotation.RegenerateAppearances();
          annotation.Add(underlineAnnotation);
        }
      }
    }
    return (System.Collections.Generic.IReadOnlyList<PdfUnderlineAnnotation>) annotation;
  }

  public override bool OnPropertyChanged(string propertyName)
  {
    if (this.SelectedAnnotation is PdfUnderlineAnnotation selectedAnnotation && propertyName == "UnderlineStroke")
    {
      MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
      using (requiredService.OperationManager.TraceAnnotationChange(selectedAnnotation.Page))
      {
        FS_COLOR pdfColor = ((Color) ColorConverter.ConvertFromString(requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.UnderlineStroke)).ToPdfColor();
        selectedAnnotation.Color = pdfColor;
      }
      selectedAnnotation.TryRedrawAnnotation();
    }
    return false;
  }
}
