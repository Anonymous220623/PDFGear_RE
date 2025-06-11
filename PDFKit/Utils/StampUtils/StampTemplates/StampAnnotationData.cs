// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.StampUtils.StampTemplates.StampAnnotationData
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net.Annotations;
using System;
using System.Collections.Generic;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Utils.StampUtils.StampTemplates;

public class StampAnnotationData
{
  public string Content { get; set; }

  public Dictionary<string, string> ContentDictionary { get; set; }

  public double Width { get; set; }

  public double Height { get; set; }

  public Color Color { get; set; }

  public DateTimeOffset? CreateDate { get; set; }

  public DateTimeOffset? ModificationDate { get; set; }

  public string Name { get; set; }

  public string Subject { get; set; }

  public static StampAnnotationData Create(PdfStampAnnotation annotation)
  {
    PDFExtStampDictionary extStampDictionary = StampUtil.GetPDFExtStampDictionary(annotation);
    return StampAnnotationData.Create(annotation, extStampDictionary);
  }

  public static StampAnnotationData Create(
    PdfStampAnnotation annotation,
    PDFExtStampDictionary extDict)
  {
    StampTemplateBase template = StampUtil.GetTemplate(annotation, extDict);
    if (template == null)
      return (StampAnnotationData) null;
    FS_COLOR color = annotation.Color;
    FS_RECTF rect = annotation.GetRECT();
    DateTimeOffset? nullable1 = new DateTimeOffset?();
    DateTimeOffset? nullable2 = new DateTimeOffset?();
    DateTimeOffset dateTime1;
    if (PdfAttributeUtils.TryParseModificationDate(annotation.CreationDate, out dateTime1))
      nullable1 = new DateTimeOffset?(dateTime1);
    DateTimeOffset dateTime2;
    if (PdfAttributeUtils.TryParseModificationDate(annotation.ModificationDate, out dateTime2))
      nullable2 = new DateTimeOffset?(dateTime2);
    Dictionary<string, string> contentDictionary = template.GetContentDictionary(annotation, extDict);
    return new StampAnnotationData()
    {
      Content = StampDefaultTextTemplate.GetDefaultText(annotation, extDict, contentDictionary),
      ContentDictionary = contentDictionary,
      Width = (double) rect.Width,
      Height = (double) rect.Height,
      Color = Color.FromArgb((byte) Math.Min((float) byte.MaxValue, (float) color.A * annotation.Opacity), (byte) Math.Min((int) byte.MaxValue, color.R), (byte) Math.Min((int) byte.MaxValue, color.G), (byte) Math.Min((int) byte.MaxValue, color.B)),
      CreateDate = nullable1,
      ModificationDate = nullable2,
      Name = annotation.Name,
      Subject = annotation.Subject
    };
  }
}
