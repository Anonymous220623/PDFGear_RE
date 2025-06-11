// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfSubmitAction
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfSubmitAction : PdfFormAction
{
  private string m_fileName = string.Empty;
  private PdfSubmitFormFlags m_flags;
  private HttpMethod m_httpMethod = HttpMethod.Post;
  private bool m_canonicalDateTimeFormat;
  private bool m_submitCoordinates;
  private bool m_includeNoValueFields;
  private bool m_includeIncrementalUpdates;
  private bool m_includeAnnotations;
  private bool m_excludeNonUserAnnotations;
  private bool m_embedForm;
  private SubmitDataFormat m_dataFormat = SubmitDataFormat.Fdf;

  public PdfSubmitAction(string url)
  {
    if (url == null)
      throw new ArgumentNullException(nameof (url));
    this.m_fileName = url.Length > 0 ? url : throw new ArgumentException("The URL can't be an empty string.", nameof (url));
    this.Dictionary.SetProperty("F", (IPdfPrimitive) new PdfString(this.m_fileName));
  }

  public string Url => this.m_fileName;

  public HttpMethod HttpMethod
  {
    get => this.m_httpMethod;
    set
    {
      if (this.m_httpMethod == value)
        return;
      this.m_httpMethod = value;
      if (this.m_httpMethod == HttpMethod.Get)
        this.m_flags |= PdfSubmitFormFlags.GetMethod;
      else
        this.m_flags &= ~PdfSubmitFormFlags.GetMethod;
    }
  }

  public bool CanonicalDateTimeFormat
  {
    get => this.m_canonicalDateTimeFormat;
    set
    {
      if (this.m_canonicalDateTimeFormat == value)
        return;
      this.m_canonicalDateTimeFormat = value;
      if (this.m_canonicalDateTimeFormat)
        this.m_flags |= PdfSubmitFormFlags.CanonicalFormat;
      else
        this.m_flags &= ~PdfSubmitFormFlags.CanonicalFormat;
    }
  }

  public bool SubmitCoordinates
  {
    get => this.m_submitCoordinates;
    set
    {
      if (this.m_submitCoordinates == value)
        return;
      this.m_submitCoordinates = value;
      if (this.m_submitCoordinates)
        this.m_flags |= PdfSubmitFormFlags.SubmitCoordinates;
      else
        this.m_flags &= ~PdfSubmitFormFlags.SubmitCoordinates;
    }
  }

  public bool IncludeNoValueFields
  {
    get => this.m_includeNoValueFields;
    set
    {
      if (this.m_includeNoValueFields == value)
        return;
      this.m_includeNoValueFields = value;
      if (this.m_includeNoValueFields)
        this.m_flags |= PdfSubmitFormFlags.IncludeNoValueFields;
      else
        this.m_flags &= ~PdfSubmitFormFlags.IncludeNoValueFields;
    }
  }

  public bool IncludeIncrementalUpdates
  {
    get => this.m_includeIncrementalUpdates;
    set
    {
      if (this.m_includeIncrementalUpdates == value)
        return;
      this.m_includeIncrementalUpdates = value;
      if (this.m_includeIncrementalUpdates)
        this.m_flags |= PdfSubmitFormFlags.IncludeAppendSaves;
      else
        this.m_flags &= ~PdfSubmitFormFlags.IncludeAppendSaves;
    }
  }

  public bool IncludeAnnotations
  {
    get => this.m_includeAnnotations;
    set
    {
      if (this.m_includeAnnotations == value)
        return;
      this.m_includeAnnotations = value;
      if (this.m_includeAnnotations)
        this.m_flags |= PdfSubmitFormFlags.IncludeAnnotations;
      else
        this.m_flags &= ~PdfSubmitFormFlags.IncludeAnnotations;
    }
  }

  public bool ExcludeNonUserAnnotations
  {
    get => this.m_excludeNonUserAnnotations;
    set
    {
      if (this.m_excludeNonUserAnnotations == value)
        return;
      this.m_excludeNonUserAnnotations = value;
      if (this.m_excludeNonUserAnnotations)
        this.m_flags |= PdfSubmitFormFlags.ExclNonUserAnnots;
      else
        this.m_flags &= ~PdfSubmitFormFlags.ExclNonUserAnnots;
    }
  }

  public bool EmbedForm
  {
    get => this.m_embedForm;
    set
    {
      if (this.m_embedForm == value)
        return;
      this.m_embedForm = value;
      if (this.m_embedForm)
        this.m_flags |= PdfSubmitFormFlags.EmbedForm;
      else
        this.m_flags &= ~PdfSubmitFormFlags.EmbedForm;
    }
  }

  public SubmitDataFormat DataFormat
  {
    get => this.m_dataFormat;
    set
    {
      if (this.m_dataFormat == value)
        return;
      this.m_dataFormat = value;
      switch (this.m_dataFormat)
      {
        case SubmitDataFormat.Html:
          this.m_flags |= PdfSubmitFormFlags.ExportFormat;
          this.m_flags &= (PdfSubmitFormFlags) -1;
          break;
        case SubmitDataFormat.Pdf:
          this.m_flags |= PdfSubmitFormFlags.SubmitPdf;
          this.m_flags &= (PdfSubmitFormFlags) -1;
          break;
        case SubmitDataFormat.Fdf:
          this.m_flags &= (PdfSubmitFormFlags) -1;
          break;
        case SubmitDataFormat.Xfdf:
          this.m_flags |= PdfSubmitFormFlags.Xfdf;
          this.m_flags &= (PdfSubmitFormFlags) -1;
          break;
      }
    }
  }

  public override bool Include
  {
    get => base.Include;
    set
    {
      if (base.Include == value)
        return;
      base.Include = value;
      if (base.Include)
        this.m_flags &= ~PdfSubmitFormFlags.IncludeExclude;
      else
        this.m_flags |= PdfSubmitFormFlags.IncludeExclude;
    }
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    this.Dictionary.SetProperty("S", (IPdfPrimitive) new PdfName("SubmitForm"));
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    this.Dictionary.SetProperty("Flags", (IPdfPrimitive) new PdfNumber((int) this.m_flags));
  }
}
