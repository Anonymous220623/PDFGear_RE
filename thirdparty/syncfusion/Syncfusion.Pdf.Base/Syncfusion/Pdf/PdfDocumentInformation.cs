// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfDocumentInformation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Xmp;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfDocumentInformation : IPdfWrapper
{
  private XmpMetadata m_xmp;
  private PdfCatalog m_catalog;
  private string m_author;
  private string m_title;
  private string m_subject;
  private string m_keywords;
  private string m_creator;
  private string m_producer;
  internal DateTime m_creationDate = DateTime.Now;
  internal DateTime m_modificationDate = DateTime.Now;
  private PdfDictionary m_dictionary;
  internal bool ConformanceEnabled;
  private string m_arrayString = "";
  private string m_customValue = "";
  internal bool isRemoveModifyDate;
  private CustomMetadata m_customMetadata;
  private ZugferdConformanceLevel m_zugferdConformanceLevel;
  private ZugferdVersion m_zugferdVersion;
  private string m_language;
  internal bool isConformanceCheck;
  private string m_label;
  internal bool m_autoTag;

  internal PdfDocumentInformation(PdfCatalog catalog)
  {
    if (catalog == null)
      throw new ArgumentNullException(nameof (catalog));
    this.m_dictionary = new PdfDictionary();
    if (PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_A1B && PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_A1A)
      this.m_dictionary.SetDateTime(nameof (CreationDate), this.m_creationDate);
    this.m_catalog = catalog;
  }

  internal PdfDocumentInformation(PdfDictionary dictionary, PdfCatalog catalog)
  {
    if (dictionary == null)
      throw new ArgumentNullException(nameof (dictionary));
    if (catalog == null)
      throw new ArgumentNullException(nameof (catalog));
    this.m_dictionary = dictionary;
    this.m_catalog = catalog;
  }

  public DateTime CreationDate
  {
    get
    {
      if (!(this.m_dictionary[nameof (CreationDate)] is PdfString dateTimeStringValue))
        return this.m_creationDate = DateTime.Now;
      this.m_creationDate = this.m_dictionary.GetDateTime(dateTimeStringValue);
      return this.m_creationDate;
    }
    set
    {
      if (!(this.m_creationDate != value))
        return;
      this.m_creationDate = value;
      this.m_dictionary.SetDateTime(nameof (CreationDate), this.m_creationDate);
    }
  }

  public DateTime ModificationDate
  {
    get
    {
      if (!(this.m_dictionary["ModDate"] is PdfString dateTimeStringValue))
        return this.m_creationDate = DateTime.Now;
      this.m_modificationDate = this.m_dictionary.GetDateTime(dateTimeStringValue);
      return this.m_modificationDate;
    }
    set
    {
      this.m_modificationDate = value;
      this.m_dictionary.SetDateTime("ModDate", this.m_modificationDate);
    }
  }

  public string Title
  {
    get
    {
      if (!(this.m_dictionary[nameof (Title)] is PdfString pdfString))
        return this.m_title = string.Empty;
      this.m_title = pdfString.Value.Replace("\0", string.Empty);
      return this.m_title;
    }
    set
    {
      if (value == null)
        return;
      this.m_title = value;
      this.m_dictionary.SetString(nameof (Title), this.m_title);
    }
  }

  public string Author
  {
    get
    {
      if (!(this.m_dictionary[nameof (Author)] is PdfString pdfString))
        return this.m_author = string.Empty;
      this.m_author = pdfString.Value;
      return this.m_author;
    }
    set
    {
      if (value != null)
      {
        this.m_author = value;
        this.m_dictionary.SetString(nameof (Author), this.m_author);
      }
      if (this.m_xmp == null || this.m_xmp.DublinCoreSchema == null)
        return;
      this.m_xmp.DublinCoreSchema.Creator.Add(value);
    }
  }

  public string Subject
  {
    get
    {
      if (!(this.m_dictionary[nameof (Subject)] is PdfString pdfString))
        return this.m_subject = string.Empty;
      this.m_subject = pdfString.Value;
      return this.m_subject;
    }
    set
    {
      if (value == null)
        return;
      this.m_subject = value;
      this.m_dictionary.SetString(nameof (Subject), this.m_subject);
    }
  }

  public string Keywords
  {
    get
    {
      if (!(this.m_dictionary[nameof (Keywords)] is PdfString pdfString))
        return this.m_keywords = string.Empty;
      this.m_keywords = pdfString.Value;
      return this.m_keywords;
    }
    set
    {
      if (value != null)
      {
        this.m_keywords = value;
        this.m_dictionary.SetString(nameof (Keywords), this.m_keywords);
      }
      if (this.m_catalog == null || this.m_catalog.Metadata == null)
        return;
      this.m_xmp = this.XmpMetadata;
    }
  }

  public string Creator
  {
    get
    {
      if (!(this.m_dictionary[nameof (Creator)] is PdfString pdfString))
        return this.m_creator = string.Empty;
      this.m_creator = pdfString.Value;
      return this.m_creator;
    }
    set
    {
      if (value == null)
        return;
      this.m_creator = value;
      this.m_dictionary.SetString(nameof (Creator), this.m_creator);
    }
  }

  public string Producer
  {
    get
    {
      if (!(this.m_dictionary[nameof (Producer)] is PdfString pdfString))
        return this.m_producer = string.Empty;
      this.m_producer = pdfString.Value;
      return this.m_producer;
    }
    set
    {
      if (value == null)
        return;
      this.m_producer = value;
      this.m_dictionary.SetString(nameof (Producer), this.m_producer);
    }
  }

  public XmpMetadata XmpMetadata
  {
    get
    {
      if (this.m_xmp == null)
      {
        if (this.m_catalog.Metadata == null)
        {
          this.m_xmp = this.m_catalog.LoadedDocument == null ? new XmpMetadata(this.m_catalog.Pages.Document.DocumentInformation) : new XmpMetadata(this.m_catalog.LoadedDocument.DocumentInformation);
          if (!this.isConformanceCheck)
            this.m_catalog.SetProperty("Metadata", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_xmp));
        }
        else if (this.m_dictionary.Changed && !this.m_catalog.Changed)
        {
          this.m_xmp = new XmpMetadata(this.m_catalog.LoadedDocument.DocumentInformation);
          if (!this.isConformanceCheck)
            this.m_catalog.SetProperty("Metadata", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_xmp));
          if (this.m_customMetadata != null)
            this.m_customMetadata.Xmp = this.m_xmp;
        }
        else
        {
          this.m_xmp = this.m_catalog.Metadata;
          if (!this.isConformanceCheck)
            this.m_catalog.SetProperty("Metadata", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_xmp));
        }
      }
      else if (this.m_catalog.Metadata != null && this.m_catalog.LoadedDocument != null && this.m_dictionary.Changed && !this.m_catalog.Changed)
      {
        this.m_xmp = new XmpMetadata(this.m_catalog.LoadedDocument.DocumentInformation);
        if (!this.isConformanceCheck)
          this.m_catalog.SetProperty("Metadata", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_xmp));
      }
      if (this.m_customMetadata != null)
        this.m_customMetadata.Xmp = this.m_xmp;
      return this.m_xmp;
    }
  }

  public CustomMetadata CustomMetadata
  {
    get
    {
      if (this.m_customMetadata == null)
      {
        this.m_customMetadata = new CustomMetadata();
        this.m_customMetadata.Dictionary = this.m_dictionary;
        if (this.CustomMetadata.Dictionary != null)
        {
          System.Collections.Generic.Dictionary<PdfName, IPdfPrimitive> dictionary = new System.Collections.Generic.Dictionary<PdfName, IPdfPrimitive>();
          foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in new System.Collections.Generic.Dictionary<PdfName, IPdfPrimitive>((IDictionary<PdfName, IPdfPrimitive>) this.CustomMetadata.Dictionary.Items))
          {
            if (keyValuePair.Key.Value != "Author" && keyValuePair.Key.Value != "Title" && keyValuePair.Key.Value != "Subject" && keyValuePair.Key.Value != "Trapped" && keyValuePair.Key.Value != "Keywords" && keyValuePair.Key.Value != "Producer" && keyValuePair.Key.Value != "CreationDate" && keyValuePair.Key.Value != "ModDate" && keyValuePair.Key.Value != "Creator")
            {
              if ((object) (keyValuePair.Value as PdfReferenceHolder) != null)
              {
                PdfArray pdfArray = (keyValuePair.Value as PdfReferenceHolder).Object as PdfArray;
                for (int index = 0; index < pdfArray.Count; ++index)
                {
                  PdfString pdfString = pdfArray[index] as PdfString;
                  if (!(pdfString.Value == ""))
                    this.m_arrayString = pdfString.Value + this.m_arrayString;
                }
                if (!this.m_arrayString.Equals(""))
                  this.m_customValue = this.m_arrayString;
              }
              else if (keyValuePair.Value is PdfString)
              {
                this.m_customValue = (keyValuePair.Value as PdfString).Value;
                this.CustomMetadata[keyValuePair.Key.Value] = this.m_customValue;
                if (this.XmpMetadata != null && this.XmpMetadata.CustomSchema != null)
                {
                  this.XmpMetadata.CustomSchema[keyValuePair.Key.Value] = this.m_customValue;
                  this.XmpMetadata.CustomSchema.SetCustomPrefixNode();
                }
              }
            }
          }
        }
      }
      return this.m_customMetadata;
    }
    set
    {
      this.m_customMetadata = value;
      this.SetCustomDictionary(value);
    }
  }

  private void SetCustomDictionary(CustomMetadata value)
  {
    if (value == null || value.Dictionary == null)
      return;
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in value.Dictionary.Items)
    {
      if (keyValuePair.Key.Value != "Author" && keyValuePair.Key.Value != "Title" && keyValuePair.Key.Value != "Subject" && keyValuePair.Key.Value != "Trapped" && keyValuePair.Key.Value != "Keywords" && keyValuePair.Key.Value != "Producer" && keyValuePair.Key.Value != "CreationDate" && keyValuePair.Key.Value != "ModDate" && keyValuePair.Key.Value != "Creator")
        this.m_dictionary[keyValuePair.Key] = keyValuePair.Value;
    }
  }

  internal void AddCustomMetaDataInfo(string metaDataName, string metaDataValue)
  {
    this.Dictionary[metaDataName] = (IPdfPrimitive) new PdfString(metaDataValue);
    this.m_dictionary = this.Dictionary;
    if (this.CustomMetadata.ContainsKey(metaDataName))
    {
      if (!(this.CustomMetadata[metaDataName] != metaDataValue))
        return;
      this.CustomMetadata[metaDataName] = metaDataValue;
    }
    else
      this.CustomMetadata[metaDataName] = metaDataValue;
  }

  internal PdfDictionary Dictionary => this.m_dictionary;

  internal ZugferdConformanceLevel ZugferdConformanceLevel
  {
    get => this.m_zugferdConformanceLevel;
    set => this.m_zugferdConformanceLevel = value;
  }

  internal ZugferdVersion ZugferdVersion
  {
    get => this.m_zugferdVersion;
    set => this.m_zugferdVersion = value;
  }

  public string Language
  {
    get
    {
      if (!(PdfCrossTable.Dereference(this.m_catalog["Lang"]) is PdfString pdfString))
        return this.m_language = (string) null;
      this.m_language = pdfString.Value;
      return this.m_language;
    }
    set
    {
      if (value == null)
        return;
      this.m_language = value;
      this.m_catalog["Lang"] = (IPdfPrimitive) new PdfString(this.m_language);
    }
  }

  internal string Label
  {
    get => this.m_label;
    set
    {
      if (value == null)
        return;
      this.m_label = value;
    }
  }

  internal void ApplyPdfXConformance()
  {
    this.Dictionary["GTS_PDFXConformance"] = (IPdfPrimitive) new PdfString("PDF/X-1a:2001");
    this.Dictionary["Trapped"] = (IPdfPrimitive) new PdfName("False");
    this.Dictionary["GTS_PDFXVersion"] = (IPdfPrimitive) new PdfString("PDF/X-1:2001");
    this.ModificationDate = DateTime.Now;
    if (!(this.Title == string.Empty))
      return;
    this.Title = " ";
  }

  public void RemoveModificationDate()
  {
    if (this.m_dictionary == null || !this.m_dictionary.ContainsKey("ModDate"))
      return;
    this.m_dictionary.Remove("ModDate");
    if (!this.m_dictionary.Changed || this.m_catalog.Changed)
      return;
    this.m_catalog.LoadedDocument.DocumentInformation.Dictionary.Remove("ModDate");
    this.m_catalog.LoadedDocument.DocumentInformation.CustomMetadata.Remove("ModDate");
    this.m_catalog.LoadedDocument.DocumentInformation.isRemoveModifyDate = true;
    this.m_xmp = new XmpMetadata(this.m_catalog.LoadedDocument.DocumentInformation);
    this.m_catalog.SetProperty("Metadata", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_xmp));
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
