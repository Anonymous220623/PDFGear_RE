// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.BaseMarkupAnnotation
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Newtonsoft.Json;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using PDFKit.Utils;
using System;

#nullable disable
namespace pdfeditor.Models.Annotations;

public abstract class BaseMarkupAnnotation : BaseAnnotation
{
  public string Text { get; protected set; }

  [JsonIgnore]
  public PopupAnnotation Popup { get; set; }

  public float Opacity { get; protected set; }

  public string RichText { get; protected set; }

  public string CreationDate { get; protected set; }

  [JsonIgnore]
  public BaseAnnotation RelationshipAnnotation { get; set; }

  public string Subject { get; protected set; }

  public RelationTypes Relationship { get; protected set; }

  [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
  public int? PopupAnnotationIndex => this.Popup?.AnnotIndex;

  public float Rotate { get; protected set; }

  protected override void Init(PdfAnnotation pdfAnnotation)
  {
    base.Init(pdfAnnotation);
    PdfMarkupAnnotation annot = pdfAnnotation as PdfMarkupAnnotation;
    if (annot == null)
      return;
    this.Text = BaseAnnotation.ReturnValueOrDefault<string>((Func<string>) (() => annot.Text));
    this.Opacity = BaseAnnotation.ReturnValueOrDefault<float>((Func<float>) (() => annot.Opacity));
    this.RichText = BaseAnnotation.ReturnValueOrDefault<string>((Func<string>) (() => annot.RichText));
    this.CreationDate = BaseAnnotation.ReturnValueOrDefault<string>((Func<string>) (() => annot.CreationDate));
    this.Subject = BaseAnnotation.ReturnValueOrDefault<string>((Func<string>) (() => annot.Subject));
    this.Relationship = BaseAnnotation.ReturnValueOrDefault<RelationTypes>((Func<RelationTypes>) (() => annot.Relationship));
    this.Rotate = BaseAnnotation.ReturnValueOrDefault<float>((Func<float>) (() => annot.GetRotate()));
  }

  protected override void ApplyCore(PdfAnnotation annot)
  {
    base.ApplyCore(annot);
    if (!(annot is PdfMarkupAnnotation markupAnnotation))
      return;
    markupAnnotation.Text = this.Text;
    markupAnnotation.Opacity = this.Opacity;
    markupAnnotation.RichText = this.RichText;
    markupAnnotation.CreationDate = this.CreationDate;
    markupAnnotation.Subject = this.Subject;
    markupAnnotation.Relationship = this.Relationship;
    if (markupAnnotation.Dictionary == null)
      return;
    markupAnnotation.Dictionary["Rotate"] = (PdfTypeBase) PdfTypeNumber.Create(this.Rotate);
  }

  protected override bool EqualsCore(BaseAnnotation other)
  {
    return base.EqualsCore(other) && other is BaseMarkupAnnotation markupAnnotation && markupAnnotation.Text == this.Text && (double) markupAnnotation.Opacity == (double) this.Opacity && markupAnnotation.RichText == this.RichText && markupAnnotation.CreationDate == this.CreationDate && markupAnnotation.Subject == this.Subject && markupAnnotation.Relationship == this.Relationship;
  }
}
