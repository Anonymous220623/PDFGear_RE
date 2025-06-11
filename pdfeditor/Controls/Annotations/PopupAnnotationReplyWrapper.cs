// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.PopupAnnotationReplyWrapper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Utils;
using System;

#nullable disable
namespace pdfeditor.Controls.Annotations;

public class PopupAnnotationReplyWrapper : ObservableObject
{
  private PdfMarkupAnnotation rawObject;
  private Lazy<PdfAnnotation> parent;

  public PopupAnnotationReplyWrapper(PdfMarkupAnnotation annot)
  {
    this.rawObject = annot;
    this.parent = new Lazy<PdfAnnotation>((Func<PdfAnnotation>) (() =>
    {
      PdfAnnotation relationshipAnnotation = this.rawObject.RelationshipAnnotation;
      while (relationshipAnnotation is PdfTextAnnotation pdfTextAnnotation2 && pdfTextAnnotation2.Relationship == RelationTypes.Reply && (PdfWrapper) pdfTextAnnotation2.RelationshipAnnotation != (PdfWrapper) null)
        relationshipAnnotation = pdfTextAnnotation2.RelationshipAnnotation;
      return relationshipAnnotation;
    }));
  }

  public PdfMarkupAnnotation Annotation => this.rawObject;

  public PdfPage Page => this.rawObject.Page;

  public PdfAnnotation Parent => this.parent.Value;

  public int AnnotationIndex
  {
    get
    {
      PdfAnnotationCollection annots = this.rawObject.Page.Annots;
      return annots == null ? -1 : __nonvirtual (annots.IndexOf((PdfAnnotation) this.rawObject));
    }
  }

  public string Contents
  {
    get => this.rawObject.Contents ?? "";
    set
    {
      string str = value ?? "";
      if (!(this.rawObject.Contents != str))
        return;
      this.rawObject.Contents = str;
      this.OnPropertyChanged(nameof (Contents));
    }
  }

  public string Text => this.rawObject.Text;

  public DateTimeOffset? ModificationDate
  {
    get
    {
      DateTimeOffset modificationDate;
      return this.rawObject.TryGetModificationDate(out modificationDate) ? new DateTimeOffset?(modificationDate) : new DateTimeOffset?();
    }
    set
    {
      this.rawObject.ModificationDate = value.Value.ToModificationDateString();
      this.OnPropertyChanged(nameof (ModificationDate));
      this.OnPropertyChanged("ModificationDateText");
      this.OnPropertyChanged("ModificationDateTextShort");
    }
  }

  public string ModificationDateText
  {
    get
    {
      DateTimeOffset? modificationDate = this.ModificationDate;
      return modificationDate.HasValue ? modificationDate.Value.ToString("G") : string.Empty;
    }
  }

  public string ModificationDateTextShort
  {
    get
    {
      DateTimeOffset? modificationDate = this.ModificationDate;
      return modificationDate.HasValue ? modificationDate.Value.ToString("d") : string.Empty;
    }
  }

  public void NotifyAnnotationChanged()
  {
    this.OnPropertyChanged("Contents");
    this.OnPropertyChanged("ModificationDate");
    this.OnPropertyChanged("ModificationDateText");
    this.OnPropertyChanged("ModificationDateTextShort");
  }
}
