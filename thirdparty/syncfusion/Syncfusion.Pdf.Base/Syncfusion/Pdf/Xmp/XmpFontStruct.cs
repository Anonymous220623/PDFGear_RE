// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.XmpFontStruct
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public class XmpFontStruct : XmpStructure
{
  private const string c_prefix = "stFnt";
  private const string c_name = "http:ns.adobe.com/xap/1.0/sType/Font#";
  private const string c_fontName = "fontName";
  private const string c_fontFamily = "fontFamily";
  private const string c_fontFace = "fontFace";
  private const string c_fontType = "fontType";
  private const string c_versionString = "versionString";
  private const string c_composite = "composite";
  private const string c_fontFileName = "fontFileName";
  private const string c_childFontFiles = "childFontFiles";

  protected override string StructurePrefix => "stFnt";

  protected override string StructureURI => "http:ns.adobe.com/xap/1.0/sType/Font#";

  public string FontName
  {
    get => this.GetSimpleProperty("fontName").Value;
    set
    {
      this.GetSimpleProperty("fontName").Value = value != null ? value : throw new ArgumentNullException("fontName");
    }
  }

  public string FontFamily
  {
    get => this.GetSimpleProperty("fontFamily").Value;
    set
    {
      this.GetSimpleProperty("fontFamily").Value = value != null ? value : throw new ArgumentNullException("fontFamily");
    }
  }

  public string FontFace
  {
    get => this.GetSimpleProperty("fontFace").Value;
    set
    {
      this.GetSimpleProperty("fontFace").Value = value != null ? value : throw new ArgumentNullException("fontFace");
    }
  }

  public string FontType
  {
    get => this.GetSimpleProperty("fontType").Value;
    set
    {
      this.GetSimpleProperty("fontType").Value = value != null ? value : throw new ArgumentNullException("fontType");
    }
  }

  public string VersionString
  {
    get => this.GetSimpleProperty("versionString").Value;
    set
    {
      this.GetSimpleProperty("versionString").Value = value != null ? value : throw new ArgumentNullException("versionString");
    }
  }

  public bool Composite
  {
    get => this.GetSimpleProperty("composite").GetBool();
    set => this.GetSimpleProperty("composite").SetBool(value);
  }

  public string FontFileName
  {
    get => this.GetSimpleProperty("fontFileName").Value;
    set
    {
      this.GetSimpleProperty("fontFileName").Value = value != null ? value : throw new ArgumentNullException("fontFileName");
    }
  }

  public XmpArray ChildFontFiles => this.GetArray("childFontFiles", XmpArrayType.Seq);

  internal XmpFontStruct(
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
