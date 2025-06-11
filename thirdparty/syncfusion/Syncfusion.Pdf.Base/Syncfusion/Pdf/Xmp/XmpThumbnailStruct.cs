// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.XmpThumbnailStruct
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public class XmpThumbnailStruct : XmpStructure
{
  private const string c_prefix = "xapG";
  private const string c_name = "http://ns.adobe.com/xap/1.0/g/img/";
  private const string c_height = "height";
  private const string c_width = "width";
  private const string c_format = "format";
  private const string c_image = "image";

  protected override string StructurePrefix => "xapG";

  protected override string StructureURI => "http://ns.adobe.com/xap/1.0/g/img/";

  public float Height
  {
    get => this.GetSimpleProperty("height").GetReal();
    set => this.GetSimpleProperty("height").SetReal(value);
  }

  public float Width
  {
    get => this.GetSimpleProperty("width").GetReal();
    set => this.GetSimpleProperty("width").SetReal(value);
  }

  public string Format
  {
    get => this.GetSimpleProperty("format").Value;
    set
    {
      if (this.Format == null)
        throw new ArgumentNullException("format");
      this.GetSimpleProperty("format").Value = value;
    }
  }

  public byte[] Image
  {
    get => Convert.FromBase64String(this.GetSimpleProperty("image").Value);
    set
    {
      if (this.Image == null)
        throw new ArgumentNullException(nameof (Image));
      this.GetSimpleProperty("image").Value = Convert.ToBase64String(value);
    }
  }

  internal XmpThumbnailStruct(
    XmpMetadata xmp,
    XmlNode parent,
    string prefix,
    string localName,
    string namespaceURI,
    bool insideArray)
    : base(xmp, parent, prefix, localName, namespaceURI, insideArray)
  {
  }

  protected override void InitializeEntities()
  {
  }
}
