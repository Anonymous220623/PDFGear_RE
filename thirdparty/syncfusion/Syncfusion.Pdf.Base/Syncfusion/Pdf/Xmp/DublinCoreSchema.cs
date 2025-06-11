// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.DublinCoreSchema
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public class DublinCoreSchema : XmpSchema
{
  private const string c_prefix = "dc";
  private const string c_name = "http://purl.org/dc/elements/1.1/";
  private const string c_coverage = "coverage";
  private const string c_identifier = "identifier";
  private const string c_format = "format";
  private const string c_source = "source";
  private const string c_subject = "subject";
  private const string c_type = "type";
  private const string c_contributor = "contributor";
  private const string c_creator = "creator";
  private const string c_date = "date";
  private const string c_publisher = "publisher";
  private const string c_relation = "relation";
  private const string c_description = "description";
  private const string c_rights = "rights";
  private const string c_title = "title";
  private const string c_mimeType = "application/pdf";

  public override XmpSchemaType SchemaType => XmpSchemaType.DublinCoreSchema;

  protected override string Name => "http://purl.org/dc/elements/1.1/";

  protected override string Prefix => "dc";

  public XmpArray Contributor => this.GetArray("contributor", XmpArrayType.Bag);

  public string Coverage
  {
    get => this.GetSimpleProperty("coverage").Value;
    set
    {
      this.GetSimpleProperty("coverage").Value = value != null ? value : throw new ArgumentNullException(nameof (Coverage));
    }
  }

  public XmpArray Creator
  {
    get
    {
      return !this.XmlData.InnerXml.Contains("rdf:Bag") ? this.GetArray("creator", XmpArrayType.Seq) : this.GetArray("creator", XmpArrayType.Bag);
    }
  }

  public XmpArray Date => this.GetArray("date", XmpArrayType.Seq);

  public XmpLangArray Description => this.GetLangArray("description");

  public string Identifier
  {
    get => this.GetSimpleProperty("identifier").Value;
    set
    {
      this.GetSimpleProperty("identifier").Value = value != null ? value : throw new ArgumentNullException(nameof (Identifier));
    }
  }

  public XmpArray Publisher => this.GetArray("publisher", XmpArrayType.Bag);

  public XmpArray Relation => this.GetArray("relation", XmpArrayType.Bag);

  public XmpLangArray Rights => this.GetLangArray("rights");

  public string Source
  {
    get => this.GetSimpleProperty("source").Value;
    set
    {
      this.GetSimpleProperty("source").Value = value != null ? value : throw new ArgumentNullException(nameof (Source));
    }
  }

  public XmpArray Sublect => this.GetArray("subject", XmpArrayType.Bag);

  public XmpLangArray Title => this.GetLangArray("title");

  public XmpArray Type => this.GetArray("type", XmpArrayType.Bag);

  protected internal DublinCoreSchema(XmpMetadata xmp)
    : base(xmp)
  {
  }

  protected override void CreateEntity()
  {
    base.CreateEntity();
    this.GetSimpleProperty("format").Value = "application/pdf";
  }
}
