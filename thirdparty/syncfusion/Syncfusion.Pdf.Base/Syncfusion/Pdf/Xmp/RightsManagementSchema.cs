// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.RightsManagementSchema
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public class RightsManagementSchema : XmpSchema
{
  private const string c_prefix = "xmpRights";
  private const string c_name = "http://ns.adobe.com/xap/1.0/rights/";
  private const string c_Certificate = "Certificate";
  private const string c_Marked = "Marked";
  private const string c_Owner = "Owner";
  private const string c_UsageTerms = "UsageTerms";
  private const string c_WebStatement = "WebStatement";

  public override XmpSchemaType SchemaType => XmpSchemaType.RightsManagementSchema;

  protected override string Name => "http://ns.adobe.com/xap/1.0/rights/";

  protected override string Prefix => "xmpRights";

  public Uri Certificate
  {
    get => this.GetSimpleProperty(nameof (Certificate)).GetUri();
    set
    {
      if (value == (Uri) null)
        throw new ArgumentNullException(nameof (Certificate));
      this.GetSimpleProperty(nameof (Certificate)).SetUri(value);
    }
  }

  public bool Marked
  {
    get => this.GetSimpleProperty(nameof (Marked)).GetBool();
    set => this.GetSimpleProperty(nameof (Marked)).SetBool(value);
  }

  public XmpArray Owner => this.GetArray(nameof (Owner), XmpArrayType.Bag);

  public XmpLangArray UsageTerms => this.GetLangArray(nameof (UsageTerms));

  public Uri WebStatement
  {
    get => this.GetSimpleProperty(nameof (WebStatement)).GetUri();
    set
    {
      if (value == (Uri) null)
        throw new ArgumentNullException(nameof (WebStatement));
      this.GetSimpleProperty(nameof (WebStatement)).SetUri(value);
    }
  }

  protected internal RightsManagementSchema(XmpMetadata xmp)
    : base(xmp)
  {
  }
}
