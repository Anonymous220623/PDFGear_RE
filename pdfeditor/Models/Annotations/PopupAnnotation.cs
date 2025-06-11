// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.PopupAnnotation
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Newtonsoft.Json;
using Patagames.Pdf.Net.Annotations;
using System;

#nullable disable
namespace pdfeditor.Models.Annotations;

public class PopupAnnotation : BaseAnnotation
{
  [JsonIgnore]
  public BaseAnnotation Parent { get; set; }

  public bool IsOpen { get; protected set; }

  public string Text { get; protected set; }

  [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
  public int? ParentAnnotationIndex => this.Parent?.AnnotIndex;

  protected override void Init(PdfAnnotation pdfAnnotation)
  {
    base.Init(pdfAnnotation);
    PdfPopupAnnotation annot = pdfAnnotation as PdfPopupAnnotation;
    if (annot == null)
      return;
    this.IsOpen = BaseAnnotation.ReturnValueOrDefault<bool>((Func<bool>) (() => annot.IsOpen));
    this.Text = BaseAnnotation.ReturnValueOrDefault<string>((Func<string>) (() => annot.Text));
  }

  protected override void ApplyCore(PdfAnnotation annot)
  {
    base.ApplyCore(annot);
    if (!(annot is PdfPopupAnnotation pdfPopupAnnotation))
      return;
    pdfPopupAnnotation.IsOpen = this.IsOpen;
    pdfPopupAnnotation.Text = this.Text;
    pdfPopupAnnotation.Rectangle = this.Rectangle;
  }

  protected override bool EqualsCore(BaseAnnotation other)
  {
    return base.EqualsCore(other) && other is PopupAnnotation popupAnnotation && popupAnnotation.IsOpen == this.IsOpen && popupAnnotation.Text == this.Text && popupAnnotation.Rectangle == this.Rectangle;
  }
}
