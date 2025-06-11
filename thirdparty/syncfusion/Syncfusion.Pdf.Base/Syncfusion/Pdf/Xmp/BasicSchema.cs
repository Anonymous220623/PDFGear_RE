// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.BasicSchema
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public class BasicSchema : XmpSchema
{
  private const string c_prefix = "xap";
  private const string c_name = "http://ns.adobe.com/xap/1.0/";
  private const string c_propAdvisory = "Advisory";
  private const string c_propIdentifier = "Identifier";
  private const string c_propLabel = "Label";
  private const string c_propNickname = "Nickname";
  private const string c_propBaseUrl = "BaseURL";
  private const string c_propCreatorTool = "CreatorTool";
  private const string c_propCreateData = "CreateDate";
  private const string c_propMetadataDate = "MetadataDate";
  private const string c_propModifyDate = "ModifyDate";
  private const string c_propThumbnail = "Thumbnails";
  private const string c_propRating = "Rating";
  internal bool m_externalCreationDate;
  internal bool m_externalModifyDate;

  public override XmpSchemaType SchemaType => XmpSchemaType.BasicSchema;

  protected override string Name => "http://ns.adobe.com/xap/1.0/";

  protected override string Prefix => "xap";

  public XmpArray Advisory => this.GetArray(nameof (Advisory), XmpArrayType.Bag);

  public XmpArray Identifier => this.GetArray(nameof (Identifier), XmpArrayType.Bag);

  public string Label
  {
    get => this.GetSimpleProperty(nameof (Label)).Value;
    set
    {
      this.GetSimpleProperty(nameof (Label)).Value = value != null ? value : throw new ArgumentNullException(nameof (Label));
    }
  }

  public string Nickname
  {
    get => this.GetSimpleProperty(nameof (Nickname)).Value;
    set
    {
      this.GetSimpleProperty(nameof (Nickname)).Value = value != null ? value : throw new ArgumentNullException(nameof (Nickname));
    }
  }

  public Uri BaseURL
  {
    get => this.GetSimpleProperty(nameof (BaseURL)).GetUri();
    set
    {
      if (value == (Uri) null)
        throw new ArgumentNullException(nameof (BaseURL));
      this.GetSimpleProperty(nameof (BaseURL)).SetUri(value);
    }
  }

  public string CreatorTool
  {
    get => this.GetSimpleProperty(nameof (CreatorTool)).Value;
    set
    {
      this.GetSimpleProperty(nameof (CreatorTool)).Value = value != null ? value : throw new ArgumentNullException(nameof (CreatorTool));
    }
  }

  public DateTime CreateDate
  {
    get => this.GetSimpleProperty(nameof (CreateDate)).GetDateTime();
    set
    {
      this.GetSimpleProperty(nameof (CreateDate)).SetDateTime(value);
      this.m_externalCreationDate = true;
    }
  }

  public DateTime MetadataDate
  {
    get => this.GetSimpleProperty(nameof (MetadataDate)).GetDateTime();
    set => this.GetSimpleProperty(nameof (MetadataDate)).SetDateTime(value);
  }

  public DateTime ModifyDate
  {
    get => this.GetSimpleProperty(nameof (ModifyDate)).GetDateTime();
    set
    {
      this.GetSimpleProperty(nameof (ModifyDate)).SetDateTime(value);
      this.m_externalModifyDate = true;
    }
  }

  public XmpArray Thumbnails => this.GetArray(nameof (Thumbnails), XmpArrayType.Alt);

  public XmpArray Rating => this.GetArray(nameof (Rating), XmpArrayType.Bag);

  protected internal BasicSchema(XmpMetadata xmp)
    : base(xmp)
  {
  }
}
