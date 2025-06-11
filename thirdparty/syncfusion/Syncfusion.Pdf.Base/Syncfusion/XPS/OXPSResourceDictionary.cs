// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSResourceDictionary
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace Syncfusion.XPS;

[XmlRoot("ResourceDictionary", Namespace = "http://schemas.openxps.org/oxps/v1.0", IsNullable = false)]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.openxps.org/oxps/v1.0")]
[Serializable]
public class OXPSResourceDictionary
{
  private object[] itemsField;
  private string sourceField;

  [XmlElement("MatrixTransform", typeof (OXPSMatrixTransform))]
  [XmlElement("Canvas", typeof (OXPSCanvas))]
  [XmlElement("Glyphs", typeof (OXPSGlyphs))]
  [XmlElement("ImageBrush", typeof (OXPSImageBrush))]
  [XmlElement("LinearGradientBrush", typeof (OXPSLinearGradientBrush))]
  [XmlElement("Path", typeof (OXPSPath))]
  [XmlElement("PathGeometry", typeof (OXPSPathGeometry))]
  [XmlElement("RadialGradientBrush", typeof (OXPSRadialGradientBrush))]
  [XmlElement("SolidColorBrush", typeof (OXPSSolidColorBrush))]
  [XmlElement("VisualBrush", typeof (OXPSVisualBrush))]
  public object[] Items
  {
    get => this.itemsField;
    set => this.itemsField = value;
  }

  [XmlAttribute(DataType = "anyURI")]
  public string Source
  {
    get => this.sourceField;
    set => this.sourceField = value;
  }
}
