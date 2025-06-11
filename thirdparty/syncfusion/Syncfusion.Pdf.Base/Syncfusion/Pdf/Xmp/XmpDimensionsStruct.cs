// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.XmpDimensionsStruct
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public class XmpDimensionsStruct : XmpStructure
{
  private const string c_prefix = "stDim";
  private const string c_name = "http://ns.adobe.com/xap/1.0/sType/Dimensions#";
  private const string c_width = "w";
  private const string c_height = "h";
  private const string c_unit = "unit";

  public float Width
  {
    get => this.GetSimpleProperty("w").GetReal();
    set => this.GetSimpleProperty("w").SetReal(value);
  }

  public float Height
  {
    get => this.GetSimpleProperty("h").GetReal();
    set => this.GetSimpleProperty("h").SetReal(value);
  }

  public string Unit
  {
    get => this.GetSimpleProperty("unit").Value;
    set
    {
      this.GetSimpleProperty("unit").Value = value != null ? value : throw new ArgumentNullException(nameof (Unit));
    }
  }

  protected override string StructurePrefix => "stDim";

  protected override string StructureURI => "http://ns.adobe.com/xap/1.0/sType/Dimensions#";

  internal XmpDimensionsStruct(
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
