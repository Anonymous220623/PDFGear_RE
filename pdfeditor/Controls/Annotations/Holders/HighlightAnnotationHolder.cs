// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.Holders.HighlightAnnotationHolder
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
using PDFKit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Annotations.Holders;

public class HighlightAnnotationHolder(AnnotationCanvas annotationCanvas) : 
  TextMarkupAnnotationHolder<PdfHighlightAnnotation>(annotationCanvas)
{
  public override System.Collections.Generic.IReadOnlyList<PdfHighlightAnnotation> CreateAnnotation(
    PdfDocument document,
    SelectInfo selectInfo)
  {
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    TextInfo[] array = PdfTextMarkupAnnotationUtils.GetTextInfos(document, selectInfo).ToArray<TextInfo>();
    if (array.Length == 0)
      return (System.Collections.Generic.IReadOnlyList<PdfHighlightAnnotation>) null;
    Color color = (Color) ColorConverter.ConvertFromString(requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.HighlightStroke);
    FS_COLOR fsColor = new FS_COLOR((int) color.A, (int) color.R, (int) color.G, (int) color.B);
    List<PdfHighlightAnnotation> annotation = new List<PdfHighlightAnnotation>(array.Length);
    foreach (TextInfo textInfo in array)
    {
      PdfQuadPointsCollection pointsCollection = new PdfQuadPointsCollection();
      foreach (FS_RECTF normalizedRect in (IEnumerable<FS_RECTF>) PdfTextMarkupAnnotationUtils.GetNormalizedRects(this.AnnotationCanvas.PdfViewer, textInfo, true, true))
      {
        FS_QUADPOINTSF quadPoints = normalizedRect.ToQuadPoints();
        pointsCollection.Add(quadPoints);
      }
      if (pointsCollection.Count > 0)
      {
        PdfPage page = document.Pages[textInfo.PageIndex];
        PdfHighlightAnnotation highlight = new PdfHighlightAnnotation(page);
        highlight.Color = fsColor;
        highlight.QuadPoints = pointsCollection;
        highlight.Text = AnnotationAuthorUtil.GetAuthorName();
        highlight.Flags |= AnnotationFlags.Print;
        if (page.Annots == null)
          page.CreateAnnotations();
        if (page.Annots != null)
        {
          page.Annots.Add((PdfAnnotation) highlight);
          highlight.RegenerateAppearancesWithoutRound();
          annotation.Add(highlight);
        }
      }
    }
    return (System.Collections.Generic.IReadOnlyList<PdfHighlightAnnotation>) annotation;
  }

  protected override bool CheckPointMoved(FS_POINTF point1, FS_POINTF point2)
  {
    return (double) Math.Abs(point1.X - point2.X) > 5.0 || (double) Math.Abs(point1.Y - point2.Y) > 5.0;
  }

  public override bool OnPropertyChanged(string propertyName)
  {
    if (this.SelectedAnnotation is PdfHighlightAnnotation selectedAnnotation && propertyName == "HighlightStroke")
    {
      MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
      using (requiredService.OperationManager.TraceAnnotationChange(selectedAnnotation.Page))
      {
        FS_COLOR pdfColor = ((Color) ColorConverter.ConvertFromString(requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.HighlightStroke)).ToPdfColor();
        selectedAnnotation.Color = pdfColor;
      }
      selectedAnnotation.TryRedrawAnnotation();
    }
    return false;
  }
}
