// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.Glyphs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

#nullable disable
namespace Syncfusion.XPS;

[DebuggerStepThrough]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06")]
[XmlRoot("Glyphs", Namespace = "http://schemas.microsoft.com/xps/2005/06", IsNullable = false)]
[Serializable]
public class Glyphs
{
  private Transform glyphsRenderTransformField;
  private Geometry glyphsClipField;
  private Brush glyphsOpacityMaskField;
  private Brush glyphsFillField;
  private string bidiLevelField;
  private string caretStopsField;
  private string deviceFontNameField;
  private string fillField;
  private double fontRenderingEmSizeField;
  private string fontUriField;
  private double originXField;
  private double originYField;
  private bool isSidewaysField;
  private string indicesField;
  private string unicodeStringField;
  private StyleSimulations styleSimulationsField;
  private string renderTransformField;
  private string clipField;
  private double opacityField;
  private string opacityMaskField;
  private string nameField;
  private string fixedPageNavigateUriField;
  private string langField;
  private string keyField;

  public Glyphs()
  {
    this.bidiLevelField = "0";
    this.isSidewaysField = false;
    this.styleSimulationsField = StyleSimulations.None;
    this.opacityField = 1.0;
  }

  [XmlElement("Glyphs.RenderTransform")]
  public Transform GlyphsRenderTransform
  {
    get => this.glyphsRenderTransformField;
    set => this.glyphsRenderTransformField = value;
  }

  [XmlElement("Glyphs.Clip")]
  public Geometry GlyphsClip
  {
    get => this.glyphsClipField;
    set => this.glyphsClipField = value;
  }

  [XmlElement("Glyphs.OpacityMask")]
  public Brush GlyphsOpacityMask
  {
    get => this.glyphsOpacityMaskField;
    set => this.glyphsOpacityMaskField = value;
  }

  [XmlElement("Glyphs.Fill")]
  public Brush GlyphsFill
  {
    get => this.glyphsFillField;
    set => this.glyphsFillField = value;
  }

  [XmlAttribute(DataType = "integer")]
  [DefaultValue("0")]
  public string BidiLevel
  {
    get => this.bidiLevelField;
    set => this.bidiLevelField = value;
  }

  [XmlAttribute]
  public string CaretStops
  {
    get => this.caretStopsField;
    set => this.caretStopsField = value;
  }

  [XmlAttribute]
  public string DeviceFontName
  {
    get => this.deviceFontNameField;
    set => this.deviceFontNameField = value;
  }

  [XmlAttribute]
  public string Fill
  {
    get => this.fillField;
    set => this.fillField = value;
  }

  [XmlAttribute]
  public double FontRenderingEmSize
  {
    get => this.fontRenderingEmSizeField;
    set => this.fontRenderingEmSizeField = value;
  }

  [XmlAttribute(DataType = "anyURI")]
  public string FontUri
  {
    get => this.fontUriField;
    set => this.fontUriField = value;
  }

  [XmlAttribute]
  public double OriginX
  {
    get => this.originXField;
    set => this.originXField = value;
  }

  [XmlAttribute]
  public double OriginY
  {
    get => this.originYField;
    set => this.originYField = value;
  }

  [DefaultValue(false)]
  [XmlAttribute]
  public bool IsSideways
  {
    get => this.isSidewaysField;
    set => this.isSidewaysField = value;
  }

  [XmlAttribute]
  public string Indices
  {
    get => this.indicesField;
    set => this.indicesField = value;
  }

  [XmlAttribute]
  public string UnicodeString
  {
    get => this.unicodeStringField;
    set => this.unicodeStringField = value;
  }

  [XmlAttribute]
  [DefaultValue(StyleSimulations.None)]
  public StyleSimulations StyleSimulations
  {
    get => this.styleSimulationsField;
    set => this.styleSimulationsField = value;
  }

  [XmlAttribute]
  public string RenderTransform
  {
    get => this.renderTransformField;
    set => this.renderTransformField = value;
  }

  [XmlAttribute]
  public string Clip
  {
    get => this.clipField;
    set => this.clipField = value;
  }

  [DefaultValue(1)]
  [XmlAttribute]
  public double Opacity
  {
    get => this.opacityField;
    set => this.opacityField = value;
  }

  [XmlAttribute]
  public string OpacityMask
  {
    get => this.opacityMaskField;
    set => this.opacityMaskField = value;
  }

  [XmlAttribute(DataType = "ID")]
  public string Name
  {
    get => this.nameField;
    set => this.nameField = value;
  }

  [XmlAttribute("FixedPage.NavigateUri", DataType = "anyURI")]
  public string FixedPageNavigateUri
  {
    get => this.fixedPageNavigateUriField;
    set => this.fixedPageNavigateUriField = value;
  }

  [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
  public string lang
  {
    get => this.langField;
    set => this.langField = value;
  }

  [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://schemas.microsoft.com/xps/2005/06/resourcedictionary-key")]
  public string Key
  {
    get => this.keyField;
    set => this.keyField = value;
  }
}
