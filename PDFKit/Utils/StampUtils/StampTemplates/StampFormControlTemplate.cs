// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.StampUtils.StampTemplates.StampFormControlTemplate
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using System;
using System.Linq;

#nullable disable
namespace PDFKit.Utils.StampUtils.StampTemplates;

internal class StampFormControlTemplate : StampTemplateBase
{
  internal override bool IsMatch(PdfStampAnnotation annotation, PDFExtStampDictionary extDict)
  {
    if ((PdfWrapper) annotation == (PdfWrapper) null || extDict == null || !(extDict.Type == "FormControl"))
      return false;
    string extendedIconName = annotation.ExtendedIconName;
    return StampFormObjectDrawingHelper.FormObjectNames.Contains<string>(extendedIconName);
  }

  internal override bool RegenerateAppearances(
    PdfStampAnnotation annotation,
    PDFExtStampDictionary extDict)
  {
    if (annotation == null)
      return false;
    string extendedIconName = annotation.ExtendedIconName;
    if (annotation.NormalAppearance != null)
    {
      PdfPageObject[] array = annotation.NormalAppearance.ToArray<PdfPageObject>();
      annotation.NormalAppearance?.Clear();
      foreach (IDisposable disposable in array.OfType<IDisposable>())
        disposable.Dispose();
    }
    annotation.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    annotation.GetRECT();
    FS_COLOR color = annotation.Color;
    FS_COLOR fillColor = new FS_COLOR((int) ((double) color.A * (double) annotation.Opacity), color.R, color.G, color.B);
    FS_RECTF rect = annotation.GetRECT();
    foreach (PdfPageObject pdfPageObject in StampFormObjectDrawingHelper.CreatePageObject(extendedIconName, rect, new FS_COLOR(), fillColor, 0.0f))
      annotation.NormalAppearance.Add(pdfPageObject);
    annotation.GenerateAppearance(AppearanceStreamModes.Normal);
    return true;
  }
}
