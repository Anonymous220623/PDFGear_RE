// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.StampUtils.StampTemplates.StampImageTemplate
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace PDFKit.Utils.StampUtils.StampTemplates;

internal class StampImageTemplate : StampTemplateBase
{
  internal override bool IsMatch(PdfStampAnnotation annotation, PDFExtStampDictionary extDict)
  {
    return annotation != null && extDict == null && string.IsNullOrEmpty(StampDefaultTextTemplate.GetDefaultText(annotation, extDict, (Dictionary<string, string>) null)) && annotation.NormalAppearance.Any<PdfPageObject>((Func<PdfPageObject, bool>) (c => c.ObjectType == PageObjectTypes.PDFPAGE_IMAGE));
  }

  internal override bool RegenerateAppearances(
    PdfStampAnnotation annotation,
    PDFExtStampDictionary extDict)
  {
    if (annotation == null)
      return false;
    FS_RECTF fsRectf = annotation.GetRECT();
    if ((double) fsRectf.Width > 1000000.0 || (double) fsRectf.Height > 1000000.0 || float.IsNaN(fsRectf.Width) || float.IsNaN(fsRectf.Height))
    {
      fsRectf = new FS_RECTF(0.0f, 20f, 20f, 0.0f);
      annotation.Rectangle = fsRectf;
    }
    annotation.GenerateAppearance(AppearanceStreamModes.Normal);
    annotation.Rectangle = fsRectf;
    return true;
  }
}
