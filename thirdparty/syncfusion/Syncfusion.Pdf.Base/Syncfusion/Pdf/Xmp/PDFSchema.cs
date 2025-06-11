// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.PDFSchema
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public class PDFSchema : XmpSchema
{
  private const string c_prefix = "pdf";
  private const string c_name = "http://ns.adobe.com/pdf/1.3/";
  private const string c_Keywords = "Keywords";
  private const string c_PDFVersion = "PDFVersion";
  private const string c_Producer = "Producer";

  public override XmpSchemaType SchemaType => XmpSchemaType.PDFSchema;

  protected override string Name => "http://ns.adobe.com/pdf/1.3/";

  protected override string Prefix => "pdf";

  public string Keywords
  {
    get => this.GetSimpleProperty(nameof (Keywords)).Value;
    set
    {
      this.GetSimpleProperty(nameof (Keywords)).Value = value != null ? value : throw new ArgumentNullException(nameof (Keywords));
    }
  }

  public string PDFVersion
  {
    get => this.GetSimpleProperty(nameof (PDFVersion)).Value;
    set
    {
      this.GetSimpleProperty(nameof (PDFVersion)).Value = value != null ? value : throw new ArgumentNullException(nameof (PDFVersion));
    }
  }

  public string Producer
  {
    get => this.GetSimpleProperty(nameof (Producer)).Value;
    set
    {
      this.GetSimpleProperty(nameof (Producer)).Value = value != null ? value : throw new ArgumentNullException(nameof (Producer));
    }
  }

  protected internal PDFSchema(XmpMetadata xmp)
    : base(xmp)
  {
  }
}
