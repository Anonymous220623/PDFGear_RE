// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.LinkAnnotation
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using pdfeditor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Models.Annotations;

public class LinkAnnotation : BaseAnnotation
{
  public PdfLink Link { get; set; }

  public PdfAction PdfAction { get; set; }

  public PdfTypeBase PdfTypedictionary { get; set; }

  public PdfBorderStyleModel PdfBorderStyle { get; set; }

  protected override bool EqualsCore(BaseAnnotation other)
  {
    return base.EqualsCore(other) && other is LinkAnnotation linkAnnotation && linkAnnotation.Rectangle == this.Rectangle && linkAnnotation.Link == this.Link && linkAnnotation.PdfAction == this.PdfAction && EqualityComparer<PdfBorderStyleModel>.Default.Equals(linkAnnotation.PdfBorderStyle, this.PdfBorderStyle);
  }

  protected override void Init(PdfAnnotation pdfAnnotation)
  {
    base.Init(pdfAnnotation);
    PdfLinkAnnotation link = pdfAnnotation as PdfLinkAnnotation;
    if (link == null)
      return;
    this.PdfAction = BaseAnnotation.ReturnValueOrDefault<PdfAction>((Func<PdfAction>) (() => link.Link.Action));
    Patagames.Pdf.Net.Wrappers.PdfBorderStyle borderStyle = link.GetBorderStyle();
    this.PdfBorderStyle = BaseAnnotation.ReturnValueOrDefault<PdfBorderStyleModel>((Func<PdfBorderStyleModel>) (() => link.Link.Dictionary["BS"] == null ? (PdfBorderStyleModel) null : new PdfBorderStyleModel(borderStyle.Width, borderStyle.Style, borderStyle.DashPattern)));
  }

  protected override void ApplyCore(PdfAnnotation annot)
  {
    base.ApplyCore(annot);
    if (!(annot is PdfLinkAnnotation))
      return;
    PdfLinkAnnotation annot1 = (PdfLinkAnnotation) annot;
    annot1.Rectangle = this.Rectangle;
    try
    {
      annot1.Link.Action = this.PdfAction;
    }
    catch
    {
    }
    Patagames.Pdf.Net.Wrappers.PdfBorderStyle borderStyle = new Patagames.Pdf.Net.Wrappers.PdfBorderStyle();
    if (annot1.Link.Dictionary.ContainsKey("BS"))
      borderStyle = annot1.GetBorderStyle();
    if (this.PdfBorderStyle != null)
    {
      borderStyle.Width = this.PdfBorderStyle.Width;
      borderStyle.DashPattern = this.PdfBorderStyle.DashPattern.ToArray<float>();
      borderStyle.Style = this.PdfBorderStyle.Style;
    }
    else
    {
      borderStyle.Width = 1f;
      borderStyle.DashPattern = new float[2]{ 2f, 4f };
      borderStyle.Style = BorderStyles.Solid;
    }
    annot1.SetBorderStyle(borderStyle);
  }
}
