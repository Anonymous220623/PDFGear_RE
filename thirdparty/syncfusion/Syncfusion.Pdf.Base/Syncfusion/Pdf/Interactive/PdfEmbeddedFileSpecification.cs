// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfEmbeddedFileSpecification
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfEmbeddedFileSpecification : PdfFileSpecificationBase
{
  private string m_description = string.Empty;
  private EmbeddedFile m_embeddedFile;
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfPortfolioAttributes m_portfolioAttributes;
  private PdfAttachmentRelationship m_relationShip;

  public PdfEmbeddedFileSpecification(string fileName)
    : base(fileName)
  {
    this.m_embeddedFile = new EmbeddedFile(fileName);
    this.Description = fileName;
  }

  public PdfEmbeddedFileSpecification(string fileName, byte[] data)
    : base(fileName)
  {
    this.m_embeddedFile = data != null ? new EmbeddedFile(fileName, data) : throw new ArgumentNullException(nameof (data));
    this.Description = fileName;
  }

  public PdfEmbeddedFileSpecification(string fileName, Stream stream)
    : base(fileName)
  {
    this.m_embeddedFile = stream != null ? new EmbeddedFile(fileName, stream) : throw new ArgumentNullException(nameof (stream));
    this.Description = fileName;
  }

  public override string FileName
  {
    get => this.m_embeddedFile.FileName;
    set => this.m_embeddedFile.FileName = value;
  }

  public byte[] Data
  {
    get => this.m_embeddedFile.Data;
    set => this.m_embeddedFile.Data = value;
  }

  public string Description
  {
    get => this.m_description;
    set
    {
      if (!(this.m_description != value))
        return;
      this.m_description = value;
      this.Dictionary.SetString("Desc", this.m_description);
    }
  }

  public string MimeType
  {
    get => this.m_embeddedFile.MimeType;
    set => this.m_embeddedFile.MimeType = value;
  }

  public DateTime CreationDate
  {
    get => this.m_embeddedFile.Params.CreationDate;
    set => this.m_embeddedFile.Params.CreationDate = value;
  }

  public DateTime ModificationDate
  {
    get => this.m_embeddedFile.Params.ModificationDate;
    set => this.m_embeddedFile.Params.ModificationDate = value;
  }

  public PdfPortfolioAttributes PortfolioAttributes
  {
    get => this.m_portfolioAttributes;
    set
    {
      this.m_portfolioAttributes = value;
      this.Dictionary.SetProperty("CI", (IPdfWrapper) this.m_portfolioAttributes);
    }
  }

  internal EmbeddedFile EmbeddedFile => this.m_embeddedFile;

  public PdfAttachmentRelationship Relationship
  {
    get => this.m_relationShip;
    set
    {
      this.m_relationShip = value;
      this.Dictionary.SetProperty("AFRelationship", (IPdfPrimitive) new PdfName((Enum) this.m_relationShip));
    }
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("EF", (IPdfPrimitive) this.m_dictionary);
  }

  protected override void Save()
  {
    this.m_dictionary["F"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_embeddedFile);
    PdfString primitive = new PdfString(this.FormatFileName(Path.GetFileName(this.FileName), false));
    this.Dictionary.SetProperty("F", (IPdfPrimitive) primitive);
    this.Dictionary.SetProperty("UF", (IPdfPrimitive) primitive);
  }
}
