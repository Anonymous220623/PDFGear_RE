// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.TextAnnotation
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Annotations;
using System;

#nullable disable
namespace pdfeditor.Models.Annotations;

public class TextAnnotation : BaseMarkupAnnotation
{
  public bool IsOpen { get; protected set; }

  public IconNames StandardIconName { get; protected set; }

  public string ExtendedIconName { get; protected set; }

  public StateModels StateModel { get; protected set; }

  public AnnotationStates State { get; protected set; }

  protected override void ApplyCore(PdfAnnotation annot)
  {
    base.ApplyCore(annot);
    if (!(annot is PdfTextAnnotation pdfTextAnnotation))
      return;
    pdfTextAnnotation.IsOpen = this.IsOpen;
    pdfTextAnnotation.StandardIconName = this.StandardIconName;
    pdfTextAnnotation.ExtendedIconName = this.ExtendedIconName;
    pdfTextAnnotation.StateModel = this.StateModel;
    pdfTextAnnotation.State = this.State;
    pdfTextAnnotation.Rectangle = this.Rectangle;
  }

  protected override void Init(PdfAnnotation pdfAnnotation)
  {
    base.Init(pdfAnnotation);
    PdfTextAnnotation text = pdfAnnotation as PdfTextAnnotation;
    if (text == null)
      return;
    this.IsOpen = BaseAnnotation.ReturnValueOrDefault<bool>((Func<bool>) (() => text.IsOpen));
    this.StandardIconName = BaseAnnotation.ReturnValueOrDefault<IconNames>((Func<IconNames>) (() => text.StandardIconName));
    this.ExtendedIconName = BaseAnnotation.ReturnValueOrDefault<string>((Func<string>) (() => text.ExtendedIconName));
    this.StateModel = BaseAnnotation.ReturnValueOrDefault<StateModels>((Func<StateModels>) (() => text.StateModel));
    this.State = BaseAnnotation.ReturnValueOrDefault<AnnotationStates>((Func<AnnotationStates>) (() => text.State));
  }

  protected override bool EqualsCore(BaseAnnotation other)
  {
    return base.EqualsCore(other) && other is TextAnnotation textAnnotation && textAnnotation.IsOpen == this.IsOpen && textAnnotation.StandardIconName == this.StandardIconName && textAnnotation.ExtendedIconName == this.ExtendedIconName && textAnnotation.StateModel == this.StateModel && textAnnotation.State == this.State && textAnnotation.Rectangle == this.Rectangle;
  }
}
