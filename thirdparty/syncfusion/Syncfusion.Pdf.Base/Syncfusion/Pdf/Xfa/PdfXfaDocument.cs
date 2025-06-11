// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaDocument
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.IO;
using System.Web;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaDocument : ICloneable
{
  private PdfXfaPageCollection m_pages = new PdfXfaPageCollection();
  private PdfXfaForm m_form;
  internal int m_pageCount;
  private PdfDocument m_document;
  private PdfFileStructure m_fileStructure;
  internal PdfXfaType formType;
  internal XmlWriter dataSetWriter;
  internal PdfArray m_imageArray = new PdfArray();
  internal string m_formName = string.Empty;
  private PdfXfaPageSettings m_pageSettings = new PdfXfaPageSettings();

  public PdfXfaPageSettings PageSettings
  {
    get => this.m_pageSettings;
    set
    {
      if (value == null)
        return;
      this.m_pageSettings = value;
    }
  }

  public PdfXfaPageCollection Pages
  {
    get
    {
      this.m_pages.m_parent = this;
      return this.m_pages;
    }
    internal set => this.m_pages = value;
  }

  public PdfXfaForm XfaForm
  {
    get => this.m_form;
    set => this.m_form = value;
  }

  public string FormName
  {
    get => this.m_formName;
    set
    {
      if (value == null)
        return;
      this.m_formName = value;
    }
  }

  internal void Save(PdfDocument doc)
  {
    if (this.XfaForm == null)
      return;
    this.XfaForm.Save(doc, this.formType);
  }

  public void Save(string fileName, PdfXfaType type)
  {
    this.m_document = new PdfDocument();
    this.formType = type;
    this.m_document.Form.Xfa = this;
    if (this.m_form != null)
    {
      this.m_form.m_xfaDocument = this;
      this.m_form.m_formType = type;
    }
    this.m_document.Save(fileName);
  }

  public void Save(Stream stream, PdfXfaType type, HttpContext response)
  {
    this.m_document = new PdfDocument();
    this.formType = type;
    this.m_document.Form.Xfa = this;
    if (this.XfaForm != null)
    {
      this.XfaForm.m_xfaDocument = this;
      this.XfaForm.m_formType = type;
    }
    this.m_document.Save(stream, response);
  }

  public void Save(string fileName, PdfXfaType type, HttpResponse response, HttpReadType httpType)
  {
    this.m_document = new PdfDocument();
    this.formType = type;
    this.m_document.Form.Xfa = this;
    if (this.XfaForm != null)
    {
      this.XfaForm.m_xfaDocument = this;
      this.XfaForm.m_formType = type;
    }
    this.m_document.Save(fileName, response, httpType);
  }

  public void Save(Stream stream, PdfXfaType type)
  {
    this.m_document = new PdfDocument();
    this.formType = type;
    this.m_document.Form.Xfa = this;
    if (this.XfaForm != null)
    {
      this.XfaForm.m_xfaDocument = this;
      this.XfaForm.m_formType = type;
    }
    this.m_document.Save(stream);
  }

  public void Close() => this.m_document.Close(true);

  public object Clone()
  {
    PdfXfaDocument pdfXfaDocument = this.MemberwiseClone() as PdfXfaDocument;
    pdfXfaDocument.XfaForm = this.XfaForm.Clone() as PdfXfaForm;
    pdfXfaDocument.FormName = this.FormName;
    pdfXfaDocument.Pages = this.Pages.Clone() as PdfXfaPageCollection;
    pdfXfaDocument.PageSettings = this.PageSettings;
    return (object) pdfXfaDocument;
  }
}
