// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.Holders.StrikeoutAnnotationHoder
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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Annotations.Holders;

public class StrikeoutAnnotationHoder(AnnotationCanvas annotationCanvas) : 
  TextMarkupAnnotationHolder<PdfStrikeoutAnnotation>(annotationCanvas)
{
  public override System.Collections.Generic.IReadOnlyList<PdfStrikeoutAnnotation> CreateAnnotation(
    PdfDocument document,
    SelectInfo selectInfo)
  {
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    TextInfo[] array = PdfTextMarkupAnnotationUtils.GetTextInfos(document, selectInfo).ToArray<TextInfo>();
    if (array.Length == 0)
      return (System.Collections.Generic.IReadOnlyList<PdfStrikeoutAnnotation>) null;
    Color color = (Color) ColorConverter.ConvertFromString(requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.StrikeStroke);
    FS_COLOR fsColor = new FS_COLOR((int) color.A, (int) color.R, (int) color.G, (int) color.B);
    List<PdfStrikeoutAnnotation> annotation = new List<PdfStrikeoutAnnotation>(array.Length);
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
        PdfStrikeoutAnnotation strikeoutAnnotation = new PdfStrikeoutAnnotation(page);
        strikeoutAnnotation.Color = fsColor;
        strikeoutAnnotation.QuadPoints = pointsCollection;
        strikeoutAnnotation.Text = AnnotationAuthorUtil.GetAuthorName();
        strikeoutAnnotation.Flags |= AnnotationFlags.Print;
        if (page.Annots == null)
          page.CreateAnnotations();
        if (page.Annots != null)
        {
          page.Annots.Add((PdfAnnotation) strikeoutAnnotation);
          strikeoutAnnotation.RegenerateAppearances();
          annotation.Add(strikeoutAnnotation);
        }
      }
    }
    return (System.Collections.Generic.IReadOnlyList<PdfStrikeoutAnnotation>) annotation;
  }

  public override bool OnPropertyChanged(string propertyName)
  {
    if (this.SelectedAnnotation is PdfStrikeoutAnnotation selectedAnnotation && propertyName == "StrikeStroke")
    {
      MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
      using (requiredService.OperationManager.TraceAnnotationChange(selectedAnnotation.Page))
      {
        FS_COLOR pdfColor = ((Color) ColorConverter.ConvertFromString(requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.StrikeStroke)).ToPdfColor();
        selectedAnnotation.Color = pdfColor;
      }
      selectedAnnotation.TryRedrawAnnotation();
    }
    return false;
  }
}
