// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.StampUtils.StampTemplates.StampTemplateBase
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net.Annotations;
using System.Collections.Generic;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Utils.StampUtils.StampTemplates;

internal abstract class StampTemplateBase
{
  internal abstract bool IsMatch(PdfStampAnnotation annotation, PDFExtStampDictionary extDict);

  internal abstract bool RegenerateAppearances(
    PdfStampAnnotation annotation,
    PDFExtStampDictionary extDict);

  internal virtual string GetText(PdfStampAnnotation annotation, PDFExtStampDictionary extDict)
  {
    return string.Empty;
  }

  internal virtual Dictionary<string, string> GetContentDictionary(
    PdfStampAnnotation annotation,
    PDFExtStampDictionary extDict)
  {
    return ContentSerializer.Deserialize(extDict?.Content);
  }

  internal virtual Visual CreatePreviewVisual(StampAnnotationData data) => (Visual) null;
}
