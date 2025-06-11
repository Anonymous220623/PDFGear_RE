// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaDocument
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfLoadedXfaDocument
{
  private PdfLoadedXfaForm m_form;
  private PdfFileStructure m_fileStructure = new PdfFileStructure();
  private PdfLoadedDocument m_document;
  private bool m_flatten;
  private XmlDocument m_xmlData;

  public bool Flatten
  {
    get => this.m_flatten;
    set
    {
      this.m_flatten = value;
      if (this.XfaForm == null)
        return;
      this.XfaForm.fDocument = new PdfDocument();
    }
  }

  public PdfLoadedXfaForm XfaForm
  {
    get => this.m_form == null ? (PdfLoadedXfaForm) null : this.m_form;
    internal set => this.m_form = value;
  }

  public XmlDocument XmlData
  {
    get
    {
      if (this.m_xmlData == null && this.XfaForm != null)
        this.LoadXDP(this.XfaForm.XFAArray);
      return this.m_xmlData;
    }
  }

  public PdfLoadedXfaDocument(string fileName)
  {
    this.m_document = new PdfLoadedDocument(fileName, false, true);
    this.m_fileStructure = this.m_document.FileStructure;
    if (this.m_document.Form != null)
    {
      this.m_form = new PdfLoadedXfaForm();
      this.XfaForm.Load(this.m_document.Catalog);
      this.m_document.Form.LoadedXfa = this.XfaForm;
    }
    this.m_document.m_isXfaDocument = true;
  }

  public PdfLoadedXfaDocument(string fileName, string password)
  {
    this.m_document = new PdfLoadedDocument(fileName, password, false, true);
    this.m_fileStructure = this.m_document.FileStructure;
    if (this.m_document.Form != null)
    {
      this.m_form = new PdfLoadedXfaForm();
      this.XfaForm.Load(this.m_document.Catalog);
      this.m_document.Form.LoadedXfa = this.XfaForm;
    }
    this.m_document.m_isXfaDocument = true;
  }

  public PdfLoadedXfaDocument(Stream file)
  {
    file.Position = 0L;
    this.m_document = new PdfLoadedDocument(file, false, true);
    this.m_fileStructure = this.m_document.FileStructure;
    if (this.m_document.Form != null)
    {
      this.m_form = new PdfLoadedXfaForm();
      this.XfaForm.Load(this.m_document.Catalog);
      this.m_document.Form.LoadedXfa = this.XfaForm;
    }
    this.m_document.m_isXfaDocument = true;
  }

  public PdfLoadedXfaDocument(Stream file, string password)
  {
    file.Position = 0L;
    this.m_document = new PdfLoadedDocument(file, password, false, true);
    this.m_fileStructure = this.m_document.FileStructure;
    if (this.m_document.Form != null)
    {
      this.m_form = new PdfLoadedXfaForm();
      this.XfaForm.Load(this.m_document.Catalog);
      this.m_document.Form.LoadedXfa = this.XfaForm;
    }
    this.m_document.m_isXfaDocument = true;
  }

  public void Save(string fileName)
  {
    if (this.Flatten)
    {
      this.XfaForm.Save(true, this.m_document, this);
      this.XfaForm.fDocument.Save(fileName);
    }
    else
      this.m_document.Save(fileName);
  }

  public void Save(Stream stream, HttpContext response)
  {
    if (this.Flatten)
    {
      this.XfaForm.Save(true, this.m_document, this);
      this.XfaForm.fDocument.Save(stream, response);
    }
    else
      this.m_document.Save(stream, response);
  }

  public void Save(string fileName, HttpResponse response, HttpReadType type)
  {
    if (this.Flatten)
    {
      this.XfaForm.Save(true, this.m_document, this);
      this.XfaForm.fDocument.Save(fileName, response, type);
    }
    else
      this.m_document.Save(fileName, response, type);
  }

  public void Save(Stream stream)
  {
    if (this.Flatten)
    {
      this.XfaForm.Save(true, this.m_document, this);
      this.XfaForm.fDocument.Save(stream);
    }
    else
      this.m_document.Save(stream);
  }

  public void Close()
  {
    this.m_document.Close(true);
    if (this.XfaForm == null || this.XfaForm.fDocument == null)
      return;
    this.XfaForm.fDocument.Close(true);
  }

  private void LoadXDP(Dictionary<string, PdfStream> xfaArray)
  {
    using (MemoryStream inStream = new MemoryStream())
    {
      foreach (KeyValuePair<string, PdfStream> xfa in xfaArray)
      {
        byte[] decompressedData = xfa.Value.GetDecompressedData();
        inStream.Write(decompressedData, 0, decompressedData.Length);
      }
      if (!inStream.CanRead || inStream.Length <= 0L)
        return;
      inStream.Position = 0L;
      this.m_xmlData = new XmlDocument();
      this.m_xmlData.Load((Stream) inStream);
    }
  }
}
