// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.XmpMetadata
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public class XmpMetadata : IPdfWrapper
{
  protected internal const string c_xpathRdf = "/x:xmpmeta/rdf:RDF";
  protected internal const string c_xmlnsUri = "http://www.w3.org/2000/xmlns/";
  protected internal const string c_xmlUri = "http://www.w3.org/XML/1998/namespace";
  protected internal const string c_rdfPrefix = "rdf";
  protected internal const string c_customSchema = "http://ns.adobe.com/pdfx/1.3/";
  protected internal const string c_xmlnsPrefix = "xmlns";
  protected internal const string c_xmlPefix = "xml";
  protected internal const string c_rdfUri = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
  private const string c_startPacket = "begin=\"\uFEFF\" id=\"W5M0MpCehiHzreSzNTczkc9d\"";
  private const string c_xmpMetaUri = "adobe:ns:meta/";
  private const string c_endPacket = "end=\"r\"";
  protected internal const string c_rdfPdfa = "http://www.aiim.org/pdfa/ns/id/";
  protected internal const string c_xap = "http://ns.adobe.com/xap/1.0/";
  protected internal const string c_pdfschema = "http://ns.adobe.com/pdf/1.3/";
  protected internal const string c_dublinSchema = "http://purl.org/dc/elements/1.1/";
  private const string c_zugferd1_0 = "urn:ferd:pdfa:CrossIndustryDocument:invoice:1p0#";
  private const string c_zugferd2_0 = "urn:zugferd:pdfa:CrossIndustryDocument:invoice:2p0#";
  private const string c_zugferd2_1 = "urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#";
  private const string c_pdfaExtension = "http://www.aiim.org/pdfa/ns/extension/";
  private const string c_pdfaProperty = "http://www.aiim.org/pdfa/ns/property#";
  private const string c_pdfaSchema = "http://www.aiim.org/pdfa/ns/schema#";
  private XmlDocument m_xmlDocument;
  private XmlNamespaceManager m_nmpManager;
  private DublinCoreSchema m_dublinCoreSchema;
  private PagedTextSchema m_pagedTextSchemaSchema;
  private BasicJobTicketSchema m_basicJobTicketSchema;
  private BasicSchema m_basicSchema;
  private RightsManagementSchema m_rightsManagementSchema;
  private PDFSchema m_pdfSchema;
  private CustomSchema m_customSchema;
  private PdfStream m_stream;
  internal bool isLoadedDocument;
  private PdfDocumentInformation m_documentInfo;
  internal bool m_hasAttributes;

  public XmlDocument XmlData => this.m_xmlDocument;

  internal PdfStream XmpStream => this.m_stream;

  public XmlNamespaceManager NamespaceManager => this.m_nmpManager;

  internal PdfDocumentInformation DocumentInfo => this.m_documentInfo;

  public DublinCoreSchema DublinCoreSchema
  {
    get
    {
      if (this.m_dublinCoreSchema == null && this.Rdf != null)
        this.m_dublinCoreSchema = new DublinCoreSchema(this);
      return this.m_dublinCoreSchema;
    }
  }

  public PagedTextSchema PagedTextSchema
  {
    get
    {
      if (this.m_pagedTextSchemaSchema == null && this.Rdf != null)
        this.m_pagedTextSchemaSchema = new PagedTextSchema(this);
      return this.m_pagedTextSchemaSchema;
    }
  }

  public BasicJobTicketSchema BasicJobTicketSchema
  {
    get
    {
      if (this.m_basicJobTicketSchema == null && this.Rdf != null)
        this.m_basicJobTicketSchema = new BasicJobTicketSchema(this);
      return this.m_basicJobTicketSchema;
    }
  }

  public BasicSchema BasicSchema
  {
    get
    {
      if (this.m_basicSchema == null && this.Rdf != null)
        this.m_basicSchema = new BasicSchema(this);
      return this.m_basicSchema;
    }
  }

  public RightsManagementSchema RightsManagementSchema
  {
    get
    {
      if (this.m_rightsManagementSchema == null && this.Rdf != null)
        this.m_rightsManagementSchema = new RightsManagementSchema(this);
      return this.m_rightsManagementSchema;
    }
  }

  public PDFSchema PDFSchema
  {
    get
    {
      if (this.m_pdfSchema == null && this.Rdf != null)
        this.m_pdfSchema = new PDFSchema(this);
      return this.m_pdfSchema;
    }
  }

  internal CustomSchema CustomSchema
  {
    get
    {
      if (this.m_customSchema == null && this.Rdf != null)
        this.m_customSchema = new CustomSchema(this, "pdfx", "http://ns.adobe.com/pdfx/1.3/");
      return this.m_customSchema;
    }
  }

  internal XmlElement Xmpmeta
  {
    get
    {
      return (this.XmlData.SelectSingleNode("/x:xmpmeta", this.NamespaceManager) ?? throw new ArgumentNullException("node")) as XmlElement;
    }
  }

  internal XmlElement Rdf
  {
    get
    {
      XmlNode rdf = (XmlNode) null;
      string xpath = "/x:xmpmeta/rdf:RDF";
      if (!this.XmlData.DocumentElement.Prefix.Equals("x"))
        xpath = this.XmlData.DocumentElement.Name;
      if (this.XmlData.InnerXml.Contains("rdf"))
      {
        rdf = this.XmlData.SelectSingleNode(xpath, this.NamespaceManager);
        if (rdf == null)
        {
          rdf = this.XmlData.SelectSingleNode($"/{this.XmlData.DocumentElement.Name}/rdf:RDF", this.NamespaceManager);
          if (rdf == null)
            throw new ArgumentNullException("node");
        }
      }
      return rdf as XmlElement;
    }
  }

  public XmpMetadata(PdfDocumentInformation documentInfo) => this.Init(documentInfo);

  public XmpMetadata(XmlDocument xmp)
  {
    if (xmp == null)
      throw new ArgumentNullException("xmpMetadata");
    this.m_stream = new PdfStream();
    this.m_stream.BeginSave += new SavePdfPrimitiveEventHandler(this.BeginSave);
    this.m_stream.EndSave += new SavePdfPrimitiveEventHandler(this.EndSave);
    this.Load(xmp);
  }

  public void Load(XmlDocument xmp)
  {
    if (xmp == null)
      throw new ArgumentNullException(nameof (xmp));
    this.Reset();
    this.m_xmlDocument = xmp;
    this.m_nmpManager = new XmlNamespaceManager(this.m_xmlDocument.NameTable);
    this.ImportNamespaces(this.m_xmlDocument.DocumentElement, this.m_nmpManager);
  }

  public void Add(XmlElement schema)
  {
    schema = schema != null ? this.XmlData.ImportNode((XmlNode) schema, true) as XmlElement : throw new ArgumentNullException(nameof (schema));
    this.ImportNamespaces(schema, this.m_nmpManager);
    this.Rdf.AppendChild((XmlNode) schema);
  }

  private void Init(PdfDocumentInformation documentInfo)
  {
    this.m_xmlDocument = new XmlDocument();
    this.m_nmpManager = new XmlNamespaceManager(this.XmlData.NameTable);
    this.m_stream = new PdfStream();
    this.m_documentInfo = documentInfo;
    this.InitStream();
    this.CreateStartPacket();
    this.CreateXmpmeta();
    this.CreateRdf(documentInfo);
    this.CreateEndPacket();
  }

  private void InitStream()
  {
    this.m_stream.BeginSave += new SavePdfPrimitiveEventHandler(this.BeginSave);
    this.m_stream.EndSave += new SavePdfPrimitiveEventHandler(this.EndSave);
    this.m_stream["Type"] = (IPdfPrimitive) new PdfName("Metadata");
    this.m_stream["Subtype"] = (IPdfPrimitive) new PdfName("XML");
    this.m_stream.Compress = false;
  }

  private void CreateStartPacket()
  {
    this.XmlData.AppendChild((XmlNode) this.XmlData.CreateProcessingInstruction("xpacket", "begin=\"\uFEFF\" id=\"W5M0MpCehiHzreSzNTczkc9d\""));
  }

  private void CreateXmpmeta()
  {
    this.XmlData.AppendChild((XmlNode) this.CreateElement("x", "xmpmeta", "adobe:ns:meta/"));
  }

  private void CreateRdf(PdfDocumentInformation documentInfo)
  {
    XmlElement element1 = this.CreateElement("rdf", "RDF", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
    if (documentInfo.ConformanceEnabled)
    {
      this.NamespaceManager.AddNamespace("pdf", "http://ns.adobe.com/pdf/1.3/");
      XmlElement element2 = this.CreateElement("rdf", "Description", "http://ns.adobe.com/pdf/1.3/");
      XmlAttribute attribute1 = this.CreateAttribute("rdf", "about", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "");
      element2.Attributes.Append(attribute1);
      XmlAttribute attribute2 = this.CreateAttribute("xmlns", "pdf", "http://ns.adobe.com/pdf/1.3/", "http://ns.adobe.com/pdf/1.3/");
      element2.Attributes.Append(attribute2);
      XmlElement element3 = this.CreateElement("pdf", "Producer", "http://ns.adobe.com/pdf/1.3/");
      if (!string.IsNullOrEmpty(documentInfo.Producer))
        element3.InnerText = documentInfo.Producer;
      element2.AppendChild((XmlNode) element3);
      XmlElement element4 = this.CreateElement("pdf", "Keywords", "http://ns.adobe.com/pdf/1.3/");
      if (!string.IsNullOrEmpty(documentInfo.Keywords))
        element4.InnerText = documentInfo.Keywords;
      element2.AppendChild((XmlNode) element4);
      this.Xmpmeta.AppendChild((XmlNode) element2);
      element1.AppendChild((XmlNode) element2);
      this.NamespaceManager.AddNamespace("xap", "http://ns.adobe.com/xap/1.0/");
      XmlElement element5 = this.CreateElement("rdf", "Description", "http://ns.adobe.com/pdf/1.3/");
      XmlAttribute attribute3 = this.CreateAttribute("rdf", "about", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "");
      element5.Attributes.Append(attribute3);
      XmlAttribute attribute4 = this.CreateAttribute("xmlns", "xap", "http://ns.adobe.com/xap/1.0/", "http://ns.adobe.com/xap/1.0/");
      element5.Attributes.Append(attribute4);
      XmlElement element6 = this.CreateElement("xap", "CreatorTool", "http://ns.adobe.com/xap/1.0/");
      if (!string.IsNullOrEmpty(documentInfo.Creator))
        element6.InnerText = documentInfo.Creator;
      element5.AppendChild((XmlNode) element6);
      XmlElement element7 = this.CreateElement("xap", "CreateDate", "http://ns.adobe.com/xap/1.0/");
      if (documentInfo.CreationDate.ToString() != null)
      {
        string str1 = documentInfo.CreationDate.ToString("s");
        string str2;
        if (documentInfo.Dictionary.ContainsKey("CreationDate"))
        {
          if (PdfCrossTable.Dereference(documentInfo.Dictionary["CreationDate"]) is PdfString pdfString && pdfString.Value.Contains("+") && pdfString.Value.LastIndexOf("+") != pdfString.Value.Length)
          {
            string[] strArray = pdfString.Value.Substring(pdfString.Value.IndexOf("+")).Split('\'');
            str2 = $"{str1}{strArray[0]}:{strArray[1]}";
          }
          else if (pdfString != null && pdfString.Value.Contains("-") && pdfString.Value.LastIndexOf("-") != pdfString.Value.Length)
          {
            string[] strArray = pdfString.Value.Substring(pdfString.Value.IndexOf("-")).Split('\'');
            str2 = $"{str1}{strArray[0]}:{strArray[1]}";
          }
          else
            str2 = pdfString == null || !pdfString.Value.Contains("Z") ? str1 + documentInfo.CreationDate.ToString("zzz") : str1 + "+00:00";
        }
        else
          str2 = this.GetDateTime(documentInfo.m_creationDate);
        element7.InnerText = str2;
      }
      element5.AppendChild((XmlNode) element7);
      XmlElement element8 = this.CreateElement("xap", "ModifyDate", "http://ns.adobe.com/xap/1.0/");
      if (documentInfo.ModificationDate.ToString() != null && !documentInfo.isRemoveModifyDate)
      {
        string str = documentInfo.ModificationDate.ToString("s") + documentInfo.ModificationDate.ToString("zzz");
        element8.InnerText = str;
      }
      element5.AppendChild((XmlNode) element8);
      this.Xmpmeta.AppendChild((XmlNode) element5);
      element1.AppendChild((XmlNode) element5);
      XmlElement element9 = this.CreateElement("rdf", "Description", "http://ns.adobe.com/pdf/1.3/");
      XmlAttribute attribute5 = this.CreateAttribute("rdf", "about", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "");
      element9.Attributes.Append(attribute5);
      XmlAttribute attribute6 = this.CreateAttribute("xmlns", "dc", "http://purl.org/dc/elements/1.1/", "http://purl.org/dc/elements/1.1/");
      element9.Attributes.Append(attribute6);
      XmlElement element10 = this.CreateElement("dc", "format", "http://purl.org/dc/elements/1.1/");
      element10.InnerText = "application/pdf";
      element9.AppendChild((XmlNode) element10);
      this.CreateDublinCoreContainer(element1, element9, "title", documentInfo.Title, true, XmpArrayType.Alt);
      this.CreateDublinCoreContainer(element1, element9, "description", documentInfo.Subject, true, XmpArrayType.Alt);
      if (!string.IsNullOrEmpty(documentInfo.Subject))
        this.CreateDublinCoreContainer(element1, element9, "subject", documentInfo.Subject, false, XmpArrayType.Bag);
      this.CreateDublinCoreContainer(element1, element9, "creator", documentInfo.Author, false, XmpArrayType.Seq);
      this.Xmpmeta.AppendChild((XmlNode) element1);
    }
    else
    {
      if (!string.IsNullOrEmpty(documentInfo.Producer) || !string.IsNullOrEmpty(documentInfo.Keywords))
      {
        this.NamespaceManager.AddNamespace("pdf", "http://ns.adobe.com/pdf/1.3/");
        XmlElement element11 = this.CreateElement("rdf", "Description", "http://ns.adobe.com/pdf/1.3/");
        XmlAttribute attribute7 = this.CreateAttribute("rdf", "about", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "");
        element11.Attributes.Append(attribute7);
        XmlAttribute attribute8 = this.CreateAttribute("xmlns", "pdf", "http://ns.adobe.com/pdf/1.3/", "http://ns.adobe.com/pdf/1.3/");
        element11.Attributes.Append(attribute8);
        if (!string.IsNullOrEmpty(documentInfo.Producer))
        {
          XmlElement element12 = this.CreateElement("pdf", "Producer", "http://ns.adobe.com/pdf/1.3/");
          element12.InnerText = documentInfo.Producer;
          element11.AppendChild((XmlNode) element12);
        }
        if (!string.IsNullOrEmpty(documentInfo.Keywords))
        {
          XmlElement element13 = this.CreateElement("pdf", "Keywords", "http://ns.adobe.com/pdf/1.3/");
          element13.InnerText = documentInfo.Keywords;
          element11.AppendChild((XmlNode) element13);
        }
        this.Xmpmeta.AppendChild((XmlNode) element11);
        element1.AppendChild((XmlNode) element11);
      }
      if ((PdfDocument.ConformanceLevel == PdfConformanceLevel.None || string.IsNullOrEmpty(documentInfo.Creator)) && (!string.IsNullOrEmpty(documentInfo.Creator) || documentInfo.m_creationDate.ToString() != null || documentInfo.m_modificationDate.ToString() != null))
      {
        this.NamespaceManager.AddNamespace("xap", "http://ns.adobe.com/xap/1.0/");
        XmlElement element14 = this.CreateElement("rdf", "Description", "http://ns.adobe.com/pdf/1.3/");
        XmlAttribute attribute9 = this.CreateAttribute("xmlns", "xap", "http://ns.adobe.com/xap/1.0/", "http://ns.adobe.com/xap/1.0/");
        XmlAttribute attribute10 = this.CreateAttribute("rdf", "about", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "");
        element14.Attributes.Append(attribute10);
        element14.Attributes.Append(attribute9);
        if (!string.IsNullOrEmpty(documentInfo.Creator))
        {
          XmlElement element15 = this.CreateElement("xap", "CreatorTool", "http://ns.adobe.com/xap/1.0/");
          element15.InnerText = documentInfo.Creator;
          element14.AppendChild((XmlNode) element15);
        }
        if (documentInfo.m_creationDate.ToString() != null)
        {
          XmlElement element16 = this.CreateElement("xap", "CreateDate", "http://ns.adobe.com/xap/1.0/");
          string dateTime = this.GetDateTime(documentInfo.m_creationDate);
          element16.InnerText = dateTime;
          element14.AppendChild((XmlNode) element16);
        }
        if (documentInfo.m_modificationDate.ToString() != null && !documentInfo.isRemoveModifyDate)
        {
          XmlElement element17 = this.CreateElement("xap", "ModifyDate", "http://ns.adobe.com/xap/1.0/");
          string dateTime = this.GetDateTime(documentInfo.m_modificationDate);
          element17.InnerText = dateTime;
          element14.AppendChild((XmlNode) element17);
        }
        this.Xmpmeta.AppendChild((XmlNode) element14);
        element1.AppendChild((XmlNode) element14);
      }
      if (!string.IsNullOrEmpty(documentInfo.Label))
      {
        XmlElement element18 = this.CreateElement("rdf", "Description", "http://ns.adobe.com/pdf/1.3/");
        XmlAttribute attribute11 = this.CreateAttribute("xmlns", "xap", "http://ns.adobe.com/xap/1.0/", "http://ns.adobe.com/xap/1.0/");
        XmlAttribute attribute12 = this.CreateAttribute("rdf", "about", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "");
        element18.Attributes.Append(attribute12);
        element18.Attributes.Append(attribute11);
        XmlElement element19 = this.CreateElement("xap", "Label", "http://ns.adobe.com/xap/1.0/");
        string label = documentInfo.Label;
        element19.InnerText = label;
        element18.AppendChild((XmlNode) element19);
        this.Xmpmeta.AppendChild((XmlNode) element18);
        element1.AppendChild((XmlNode) element18);
      }
      XmlElement element20 = this.CreateElement("rdf", "Description", "http://ns.adobe.com/pdf/1.3/");
      XmlAttribute attribute13 = this.CreateAttribute("xmlns", "dc", "http://purl.org/dc/elements/1.1/", "http://purl.org/dc/elements/1.1/");
      XmlAttribute attribute14 = this.CreateAttribute("rdf", "about", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "");
      element20.Attributes.Append(attribute14);
      element20.Attributes.Append(attribute13);
      XmlElement element21 = this.CreateElement("dc", "format", "http://purl.org/dc/elements/1.1/");
      element21.InnerText = "application/pdf";
      element20.AppendChild((XmlNode) element21);
      this.CreateDublinCoreContainer(element1, element20, "title", documentInfo.Title, true, XmpArrayType.Alt);
      this.CreateDublinCoreContainer(element1, element20, "description", documentInfo.Subject, true, XmpArrayType.Alt);
      this.CreateDublinCoreContainer(element1, element20, "subject", documentInfo.Keywords, false, XmpArrayType.Bag);
      this.CreateDublinCoreContainer(element1, element20, "creator", documentInfo.Author, false, XmpArrayType.Seq);
      switch (PdfDocument.ConformanceLevel)
      {
        case PdfConformanceLevel.Pdf_A1B:
        case PdfConformanceLevel.Pdf_A1A:
          this.NamespaceManager.AddNamespace("pdfaid", "http://www.aiim.org/pdfa/ns/id/");
          XmlElement element22 = this.CreateElement("rdf", "Description", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
          XmlAttribute attribute15 = this.CreateAttribute("pdfaid", "part", "http://www.aiim.org/pdfa/ns/id/", "1");
          XmlAttribute node = PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_A1B ? this.CreateAttribute("pdfaid", "conformance", "http://www.aiim.org/pdfa/ns/id/", "A") : this.CreateAttribute("pdfaid", "conformance", "http://www.aiim.org/pdfa/ns/id/", "B");
          XmlAttribute attribute16 = this.CreateAttribute("rdf", "about", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "");
          element22.Attributes.Append(attribute16);
          element22.Attributes.Append(attribute15);
          element22.Attributes.Append(node);
          this.Xmpmeta.AppendChild((XmlNode) element22);
          element1.AppendChild((XmlNode) element22);
          break;
        case PdfConformanceLevel.Pdf_A2B:
        case PdfConformanceLevel.Pdf_A2A:
        case PdfConformanceLevel.Pdf_A2U:
          XmlElement conformanceElement1 = this.CreateConformanceElement();
          this.Xmpmeta.AppendChild((XmlNode) conformanceElement1);
          element1.AppendChild((XmlNode) conformanceElement1);
          break;
        case PdfConformanceLevel.Pdf_A3B:
        case PdfConformanceLevel.Pdf_A3A:
        case PdfConformanceLevel.Pdf_A3U:
          XmlAttribute about = (XmlAttribute) null;
          XmlElement conformanceElement2 = this.CreateConformanceElement();
          this.Xmpmeta.AppendChild((XmlNode) conformanceElement2);
          element1.AppendChild((XmlNode) conformanceElement2);
          if (documentInfo.ZugferdConformanceLevel != ZugferdConformanceLevel.None)
          {
            this.AddZugferdConformance(element1, conformanceElement2, about, documentInfo);
            break;
          }
          break;
        default:
          this.NamespaceManager.AddNamespace("pdfaid", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
          break;
      }
      this.Xmpmeta.AppendChild((XmlNode) element1);
      if (documentInfo.m_autoTag && PdfDocument.ConformanceLevel == PdfConformanceLevel.None)
      {
        XmlElement element23 = this.CreateElement("rdf", "Description", "http://ns.adobe.com/pdf/1.3/");
        this.NamespaceManager.AddNamespace("pdfuaid", "http://www.aiim.org/pdfua/ns/id/");
        XmlElement element24 = this.CreateElement("pdfuaid", "part", "");
        element24.InnerText = "1";
        element23.AppendChild((XmlNode) element24);
        element1.AppendChild((XmlNode) element23);
      }
      if (string.IsNullOrEmpty(documentInfo.Creator) || documentInfo.m_creationDate.ToString() == null || documentInfo.m_modificationDate.ToString() == null || this.BasicSchema == null)
        return;
      this.BasicSchema.CreatorTool = documentInfo.Creator;
      this.BasicSchema.CreateDate = documentInfo.CreationDate;
      if (documentInfo.isRemoveModifyDate)
        return;
      this.BasicSchema.ModifyDate = documentInfo.ModificationDate;
    }
  }

  private void AddZugferdConformance(
    XmlElement rdf,
    XmlElement pdfA,
    XmlAttribute about,
    PdfDocumentInformation documentInfo)
  {
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string str1;
    string str2;
    if (documentInfo.ZugferdVersion == ZugferdVersion.ZugferdVersion1_0)
    {
      str1 = "zf";
      str2 = "urn:ferd:pdfa:CrossIndustryDocument:invoice:1p0#";
      if (documentInfo.ZugferdConformanceLevel.Equals((object) ZugferdConformanceLevel.EN16931) || documentInfo.ZugferdConformanceLevel.Equals((object) ZugferdConformanceLevel.Minimum))
        documentInfo.ZugferdConformanceLevel = ZugferdConformanceLevel.Basic;
    }
    else if (documentInfo.ZugferdVersion == ZugferdVersion.ZugferdVersion2_1)
    {
      str1 = "fx";
      str2 = "urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#";
    }
    else
    {
      str1 = "fx";
      str2 = "urn:zugferd:pdfa:CrossIndustryDocument:invoice:2p0#";
    }
    this.NamespaceManager.AddNamespace(str1, str2);
    pdfA = this.CreateElement(nameof (rdf), "Description", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
    about = this.CreateAttribute(nameof (rdf), nameof (about), "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "");
    XmlAttribute attribute1 = this.CreateAttribute("xmlns", str1, "http://www.w3.org/1999/02/22-rdf-syntax-ns#", str2);
    XmlElement element1 = this.CreateElement(str1, "ConformanceLevel", str2);
    string str3 = documentInfo.ZugferdConformanceLevel.ToString().ToUpper();
    if (documentInfo.ZugferdConformanceLevel.Equals((object) ZugferdConformanceLevel.EN16931))
      str3 = "EN 16931";
    element1.InnerText = str3;
    XmlElement element2 = this.CreateElement(str1, "DocumentFileName", str2);
    if (documentInfo.ZugferdVersion == ZugferdVersion.ZugferdVersion1_0)
      element2.InnerText = "ZUGFeRD-invoice.xml";
    else if (documentInfo.ZugferdVersion == ZugferdVersion.ZugferdVersion2_1)
      element2.InnerText = "factur-x.xml";
    else
      element2.InnerText = "zugferd-invoice.xml";
    XmlElement element3 = this.CreateElement(str1, "DocumentType", str2);
    element3.InnerText = "INVOICE";
    XmlElement element4 = this.CreateElement(str1, "Version", str2);
    if (documentInfo.ZugferdVersion == ZugferdVersion.ZugferdVersion2_1)
      element4.InnerText = "2.1";
    else
      element4.InnerText = "1.0";
    pdfA.Attributes.Append(about);
    pdfA.Attributes.Append(attribute1);
    pdfA.AppendChild((XmlNode) element1);
    pdfA.AppendChild((XmlNode) element2);
    pdfA.AppendChild((XmlNode) element3);
    pdfA.AppendChild((XmlNode) element4);
    this.Xmpmeta.AppendChild((XmlNode) pdfA);
    rdf.AppendChild((XmlNode) pdfA);
    this.NamespaceManager.AddNamespace("pdfaExtension", "http://www.aiim.org/pdfa/ns/extension/");
    this.NamespaceManager.AddNamespace("pdfaProperty", "http://www.aiim.org/pdfa/ns/property#");
    pdfA = this.CreateElement(nameof (rdf), "Description", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
    about = this.CreateAttribute(nameof (rdf), nameof (about), "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "");
    XmlAttribute attribute2 = this.CreateAttribute("xmlns", "pdfaExtension", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "http://www.aiim.org/pdfa/ns/extension/");
    XmlAttribute attribute3 = this.CreateAttribute("xmlns", "pdfaSchema", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "http://www.aiim.org/pdfa/ns/schema#");
    XmlAttribute attribute4 = this.CreateAttribute("xmlns", "pdfaProperty", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "http://www.aiim.org/pdfa/ns/property#");
    XmlElement element5 = this.CreateElement("pdfaExtension", "schemas", "http://www.aiim.org/pdfa/ns/extension/");
    XmlElement element6 = this.CreateElement(nameof (rdf), "Bag", "");
    XmlElement element7 = this.CreateElement(nameof (rdf), "li", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
    XmlAttribute attribute5 = this.CreateAttribute(nameof (rdf), "parseType", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "Resource");
    element7.Attributes.Append(attribute5);
    XmlElement element8 = this.CreateElement("pdfaSchema", "schema", "http://www.aiim.org/pdfa/ns/schema#");
    element8.InnerText = "ZUGFeRD PDFA Extension Schema";
    element7.AppendChild((XmlNode) element8);
    XmlElement element9 = this.CreateElement("pdfaSchema", "namespaceURI", "http://www.aiim.org/pdfa/ns/schema#");
    element9.InnerText = str2;
    element7.AppendChild((XmlNode) element9);
    XmlElement element10 = this.CreateElement("pdfaSchema", "prefix", "http://www.aiim.org/pdfa/ns/schema#");
    element10.InnerText = str1;
    element7.AppendChild((XmlNode) element10);
    XmlElement element11 = this.CreateElement("pdfaSchema", "property", "http://www.aiim.org/pdfa/ns/schema#");
    XmlElement element12 = this.CreateElement(nameof (rdf), "Seq", "http://www.aiim.org/pdfa/ns/schema#");
    XmlElement element13 = this.CreateElement(nameof (rdf), "li", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
    XmlAttribute attribute6 = this.CreateAttribute(nameof (rdf), "parseType", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "Resource");
    XmlElement zugferdPdfaProperty1 = this.CreateZugferdPdfaProperty("pdfaProperty", "name", "DocumentFileName");
    XmlElement zugferdPdfaProperty2 = this.CreateZugferdPdfaProperty("pdfaProperty", "valueType", "Text");
    XmlElement zugferdPdfaProperty3 = this.CreateZugferdPdfaProperty("pdfaProperty", "category", "external");
    XmlElement zugferdPdfaProperty4 = this.CreateZugferdPdfaProperty("pdfaProperty", "description", "name of the embedded XML invoice file");
    element13.Attributes.Append(attribute6);
    element13.AppendChild((XmlNode) zugferdPdfaProperty1);
    element13.AppendChild((XmlNode) zugferdPdfaProperty2);
    element13.AppendChild((XmlNode) zugferdPdfaProperty3);
    element13.AppendChild((XmlNode) zugferdPdfaProperty4);
    element12.AppendChild((XmlNode) element13);
    XmlElement element14 = this.CreateElement(nameof (rdf), "li", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
    XmlAttribute attribute7 = this.CreateAttribute(nameof (rdf), "parseType", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "Resource");
    element14.Attributes.Append(attribute7);
    XmlElement zugferdPdfaProperty5 = this.CreateZugferdPdfaProperty("pdfaProperty", "name", "DocumentType");
    XmlElement zugferdPdfaProperty6 = this.CreateZugferdPdfaProperty("pdfaProperty", "valueType", "Text");
    XmlElement zugferdPdfaProperty7 = this.CreateZugferdPdfaProperty("pdfaProperty", "category", "external");
    XmlElement zugferdPdfaProperty8 = this.CreateZugferdPdfaProperty("pdfaProperty", "description", "INVOICE");
    element14.AppendChild((XmlNode) zugferdPdfaProperty5);
    element14.AppendChild((XmlNode) zugferdPdfaProperty6);
    element14.AppendChild((XmlNode) zugferdPdfaProperty7);
    element14.AppendChild((XmlNode) zugferdPdfaProperty8);
    element12.AppendChild((XmlNode) element14);
    XmlElement element15 = this.CreateElement(nameof (rdf), "li", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
    XmlAttribute attribute8 = this.CreateAttribute(nameof (rdf), "parseType", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "Resource");
    element15.Attributes.Append(attribute8);
    XmlElement zugferdPdfaProperty9 = this.CreateZugferdPdfaProperty("pdfaProperty", "name", "Version");
    XmlElement zugferdPdfaProperty10 = this.CreateZugferdPdfaProperty("pdfaProperty", "valueType", "Text");
    XmlElement zugferdPdfaProperty11 = this.CreateZugferdPdfaProperty("pdfaProperty", "category", "external");
    XmlElement zugferdPdfaProperty12 = this.CreateZugferdPdfaProperty("pdfaProperty", "description", "The actual version of the ZUGFeRD XML schema");
    element15.AppendChild((XmlNode) zugferdPdfaProperty9);
    element15.AppendChild((XmlNode) zugferdPdfaProperty10);
    element15.AppendChild((XmlNode) zugferdPdfaProperty11);
    element15.AppendChild((XmlNode) zugferdPdfaProperty12);
    element12.AppendChild((XmlNode) element15);
    XmlElement element16 = this.CreateElement(nameof (rdf), "li", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
    XmlAttribute attribute9 = this.CreateAttribute(nameof (rdf), "parseType", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "Resource");
    element16.Attributes.Append(attribute9);
    XmlElement zugferdPdfaProperty13 = this.CreateZugferdPdfaProperty("pdfaProperty", "name", "ConformanceLevel");
    XmlElement zugferdPdfaProperty14 = this.CreateZugferdPdfaProperty("pdfaProperty", "valueType", "Text");
    XmlElement zugferdPdfaProperty15 = this.CreateZugferdPdfaProperty("pdfaProperty", "category", "external");
    XmlElement zugferdPdfaProperty16 = this.CreateZugferdPdfaProperty("pdfaProperty", "description", "The conformance level of the embedded ZUGFeRD data");
    element16.AppendChild((XmlNode) zugferdPdfaProperty13);
    element16.AppendChild((XmlNode) zugferdPdfaProperty14);
    element16.AppendChild((XmlNode) zugferdPdfaProperty15);
    element16.AppendChild((XmlNode) zugferdPdfaProperty16);
    element12.AppendChild((XmlNode) element16);
    element11.AppendChild((XmlNode) element12);
    element7.AppendChild((XmlNode) element11);
    element6.AppendChild((XmlNode) element7);
    element5.AppendChild((XmlNode) element6);
    pdfA.AppendChild((XmlNode) element5);
    pdfA.Attributes.Append(about);
    pdfA.Attributes.Append(attribute2);
    pdfA.Attributes.Append(attribute3);
    pdfA.Attributes.Append(attribute4);
    this.Xmpmeta.AppendChild((XmlNode) pdfA);
    rdf.AppendChild((XmlNode) pdfA);
  }

  private XmlElement CreateConformanceElement()
  {
    this.NamespaceManager.AddNamespace("pdfaid", "http://www.aiim.org/pdfa/ns/id/");
    XmlElement element = this.CreateElement("rdf", "Description", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
    XmlAttribute attribute = this.CreateAttribute("rdf", "about", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "");
    XmlAttribute node1 = (XmlAttribute) null;
    XmlAttribute node2 = (XmlAttribute) null;
    switch (PdfDocument.ConformanceLevel)
    {
      case PdfConformanceLevel.Pdf_A2B:
      case PdfConformanceLevel.Pdf_A2A:
      case PdfConformanceLevel.Pdf_A2U:
        node1 = this.CreateAttribute("pdfaid", "part", "http://www.aiim.org/pdfa/ns/id/", "2");
        node2 = PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_A2B ? (PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_A2A ? this.CreateAttribute("pdfaid", "conformance", "http://www.aiim.org/pdfa/ns/id/", "U") : this.CreateAttribute("pdfaid", "conformance", "http://www.aiim.org/pdfa/ns/id/", "A")) : this.CreateAttribute("pdfaid", "conformance", "http://www.aiim.org/pdfa/ns/id/", "B");
        break;
      case PdfConformanceLevel.Pdf_A3B:
      case PdfConformanceLevel.Pdf_A3A:
      case PdfConformanceLevel.Pdf_A3U:
        node1 = this.CreateAttribute("pdfaid", "part", "http://www.aiim.org/pdfa/ns/id/", "3");
        node2 = PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_A3B ? (PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_A3A ? this.CreateAttribute("pdfaid", "conformance", "http://www.aiim.org/pdfa/ns/id/", "U") : this.CreateAttribute("pdfaid", "conformance", "http://www.aiim.org/pdfa/ns/id/", "A")) : this.CreateAttribute("pdfaid", "conformance", "http://www.aiim.org/pdfa/ns/id/", "B");
        break;
    }
    element.Attributes.Append(attribute);
    element.Attributes.Append(node1);
    element.Attributes.Append(node2);
    return element;
  }

  private XmlElement CreateZugferdPdfaProperty(string prefix, string localName, string innerText)
  {
    XmlElement element = this.CreateElement(prefix, localName, "http://www.aiim.org/pdfa/ns/property#");
    element.InnerText = innerText;
    return element;
  }

  private void CreateDublinCoreContainer(
    XmlElement rdf,
    XmlElement dublinDesc,
    string containerName,
    string value,
    bool defaultLang,
    XmpArrayType element)
  {
    if (!string.IsNullOrEmpty(value))
    {
      XmlElement element1 = this.CreateElement("dc", containerName, "http://purl.org/dc/elements/1.1/");
      XmlElement element2 = this.CreateElement(nameof (rdf), element.ToString(), "http://purl.org/dc/elements/1.1/");
      XmlElement element3 = this.CreateElement(nameof (rdf), "li", "http://purl.org/dc/elements/1.1/");
      if (containerName == "subject")
      {
        string[] strArray = value.Split(',');
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (index > 0)
            element3 = this.CreateElement(nameof (rdf), "li", "http://purl.org/dc/elements/1.1/");
          element3.InnerText = strArray[index];
          element2.AppendChild((XmlNode) element3);
        }
      }
      else
      {
        element3.InnerText = value;
        element2.AppendChild((XmlNode) element3);
      }
      element1.AppendChild((XmlNode) element2);
      dublinDesc.AppendChild((XmlNode) element1);
      if (defaultLang)
      {
        XmlAttribute attribute = this.CreateAttribute("xml", "lang", "http://purl.org/dc/elements/1.1/", "x-default");
        element3.Attributes.Append(attribute);
      }
    }
    else if (this.DocumentInfo.ConformanceEnabled)
    {
      XmlElement element4 = this.CreateElement("dc", containerName, "http://purl.org/dc/elements/1.1/");
      XmlElement element5 = this.CreateElement(nameof (rdf), element.ToString(), "http://purl.org/dc/elements/1.1/");
      XmlElement element6 = this.CreateElement(nameof (rdf), "li", "http://purl.org/dc/elements/1.1/");
      element5.AppendChild((XmlNode) element6);
      element4.AppendChild((XmlNode) element5);
      dublinDesc.AppendChild((XmlNode) element4);
      if (defaultLang)
      {
        XmlAttribute attribute = this.CreateAttribute("xml", "lang", "http://purl.org/dc/elements/1.1/", "x-default");
        element6.Attributes.Append(attribute);
      }
    }
    rdf.AppendChild((XmlNode) dublinDesc);
  }

  private void CreateEndPacket()
  {
    this.XmlData.AppendChild((XmlNode) this.XmlData.CreateProcessingInstruction("xpacket", "end=\"r\""));
  }

  private void Reset()
  {
    this.m_xmlDocument = (XmlDocument) null;
    this.m_nmpManager = (XmlNamespaceManager) null;
    this.m_dublinCoreSchema = (DublinCoreSchema) null;
  }

  private void ImportNamespaces(XmlElement elm, XmlNamespaceManager nsm)
  {
    if (elm == null)
      throw new ArgumentNullException(nameof (elm));
    if (nsm == null)
      throw new ArgumentNullException(nameof (nsm));
    string prefix1 = elm.Prefix;
    string namespaceUri1 = elm.NamespaceURI;
    if (prefix1 != null && prefix1.Length > 0 && namespaceUri1 != null && !nsm.HasNamespace(prefix1))
      nsm.AddNamespace(prefix1, namespaceUri1);
    if (!elm.HasChildNodes)
      return;
    for (XmlNode xmlNode = elm.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
    {
      XmlNode elm1 = xmlNode;
      if (elm1.NodeType == XmlNodeType.Element)
        this.ImportNamespaces(elm1 as XmlElement, nsm);
      if (elm1.Attributes != null && elm1.Attributes.Count > 1)
      {
        for (int i = 0; i < elm1.Attributes.Count; ++i)
        {
          this.m_hasAttributes = true;
          string prefix2 = elm1.Attributes[i].Prefix;
          string namespaceUri2 = elm1.Attributes[i].NamespaceURI;
          if (prefix2 != null && prefix2 != "xmlns" && prefix2.Length > 0 && namespaceUri2 != null && !nsm.HasNamespace(prefix2))
            nsm.AddNamespace(prefix2, namespaceUri2);
        }
      }
    }
  }

  private string GetDateTime(DateTime dateTime)
  {
    int minutes = new DateTimeOffset(dateTime).Offset.Minutes;
    string str1 = minutes.ToString();
    if (minutes >= 0 && minutes <= 9)
      str1 = "0" + str1;
    int hours = new DateTimeOffset(dateTime).Offset.Hours;
    string str2 = hours.ToString();
    if (hours >= 0 && hours <= 9)
      str2 = "0" + str2;
    string empty = string.Empty;
    string dateTime1;
    if (hours < 0)
    {
      if (str2.Length == 2)
        str2 = "-0" + (-hours).ToString();
      dateTime1 = $"{dateTime.ToString("s")}{str2}:{str1}";
    }
    else
      dateTime1 = $"{dateTime.ToString("s")}+{str2}:{str1}";
    return dateTime1;
  }

  internal XmlElement CreateElement(string name)
  {
    return name != null ? this.XmlData.CreateElement(name) : throw new ArgumentNullException(nameof (name));
  }

  internal XmlAttribute CreateAttribute(string name, string value)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    XmlAttribute attribute = this.XmlData.CreateAttribute(name);
    attribute.Value = value;
    return attribute;
  }

  internal XmlElement CreateElement(string prefix, string localName, string namespaceURI)
  {
    if (prefix == null)
      throw new ArgumentNullException(nameof (prefix));
    if (localName == null)
      throw new ArgumentNullException(nameof (localName));
    namespaceURI = this.AddNamespace(prefix, namespaceURI);
    return this.XmlData.CreateElement(prefix, localName, namespaceURI);
  }

  internal XmlAttribute CreateAttribute(
    string prefix,
    string localName,
    string namespaceURI,
    string value)
  {
    if (prefix == null)
      throw new ArgumentNullException(nameof (prefix));
    if (localName == null)
      throw new ArgumentNullException(nameof (localName));
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    namespaceURI = this.AddNamespace(prefix, namespaceURI);
    XmlAttribute attribute = this.XmlData.CreateAttribute(prefix, localName, namespaceURI);
    attribute.Value = value;
    return attribute;
  }

  internal string AddNamespace(string prefix, string namespaceURI)
  {
    if (prefix == null)
      throw new ArgumentNullException(nameof (prefix));
    string str = namespaceURI;
    if (!this.NamespaceManager.HasNamespace(prefix) && prefix != "xml" && prefix != "xmlns")
    {
      if (namespaceURI == null)
        throw new ArgumentNullException(nameof (namespaceURI));
      this.NamespaceManager.AddNamespace(prefix, namespaceURI);
    }
    else
      str = this.NamespaceManager.LookupNamespace(prefix);
    return str;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_stream;

  private void BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    this.XmlData.Save((Stream) this.m_stream.InternalStream);
  }

  private void EndSave(object sender, SavePdfPrimitiveEventArgs ars) => this.m_stream.Clear();
}
