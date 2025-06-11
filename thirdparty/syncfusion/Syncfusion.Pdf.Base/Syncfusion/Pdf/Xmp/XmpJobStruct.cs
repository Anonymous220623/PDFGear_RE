// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.XmpJobStruct
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public class XmpJobStruct : XmpStructure
{
  private const string c_prefix = "stJob";
  private const string c_structName = "http://ns.adobe.com/xap/1.0/sType/Job#";
  private const string c_name = "name";
  private const string c_id = "id";
  private const string c_url = "url";

  public string Name
  {
    get => this.GetSimpleProperty("name").Value;
    set
    {
      this.GetSimpleProperty("name").Value = value != null ? value : throw new ArgumentNullException(nameof (Name));
    }
  }

  public string ID
  {
    get => this.GetSimpleProperty("id").Value;
    set
    {
      this.GetSimpleProperty("id").Value = value != null ? value : throw new ArgumentNullException(nameof (ID));
    }
  }

  public Uri Url
  {
    get => this.GetSimpleProperty("url").GetUri();
    set
    {
      if (value == (Uri) null)
        throw new ArgumentNullException(nameof (Url));
      this.GetSimpleProperty("url").SetUri(value);
    }
  }

  protected override string StructurePrefix => "stJob";

  protected override string StructureURI => "http://ns.adobe.com/xap/1.0/sType/Job#";

  internal XmpJobStruct(
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
