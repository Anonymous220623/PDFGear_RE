// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.BasicJobTicketSchema
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public class BasicJobTicketSchema : XmpSchema
{
  private const string c_prefix = "xmpBJ";
  private const string c_name = "http://ns.adobe.com/xap/1.0/bj/";
  private const string c_propJobRef = "JobRef";

  public override XmpSchemaType SchemaType => XmpSchemaType.BasicJobTicketSchema;

  protected override string Name => "http://ns.adobe.com/xap/1.0/bj/";

  protected override string Prefix => "xmpBJ";

  public XmpArray JobRef => this.GetArray(nameof (JobRef), XmpArrayType.Bag);

  protected internal BasicJobTicketSchema(XmpMetadata xmp)
    : base(xmp)
  {
  }
}
