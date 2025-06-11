// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.PagedTextSchema
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public class PagedTextSchema : XmpSchema
{
  private const string c_prefix = "xmpTPg";
  private const string c_name = "http://ns.adobe.com/xap/1.0/t/pg/";
  private const string c_NPages = "NPages";
  private const string c_Fonts = "Fonts";
  private const string c_PlateName = "PlateNames";
  private const string c_Colorants = "Colorants";
  private const string c_MaxPageSize = "MaxPageSize";

  public override XmpSchemaType SchemaType => XmpSchemaType.PagedTextSchema;

  protected override string Name => "http://ns.adobe.com/xap/1.0/t/pg/";

  protected override string Prefix => "xmpTPg";

  public XmpDimensionsStruct MaxPageSize
  {
    get
    {
      return this.GetStructure(nameof (MaxPageSize), XmpStructureType.Dimensions) as XmpDimensionsStruct;
    }
  }

  public int NPages
  {
    get => this.GetSimpleProperty(nameof (NPages)).GetInt();
    set => this.GetSimpleProperty(nameof (NPages)).SetInt(value);
  }

  public XmpArray Fonts => this.GetArray(nameof (Fonts), XmpArrayType.Bag);

  public XmpArray PlateNames => this.GetArray(nameof (PlateNames), XmpArrayType.Seq);

  public XmpArray Colorants => this.GetArray(nameof (Colorants), XmpArrayType.Seq);

  protected internal PagedTextSchema(XmpMetadata xmp)
    : base(xmp)
  {
  }
}
