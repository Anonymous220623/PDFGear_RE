// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.ResourceDictionary
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

[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlRoot("ResourceDictionary", Namespace = "http://schemas.microsoft.com/xps/2005/06", IsNullable = false)]
[Serializable]
public class ResourceDictionary
{
  private object[] itemsField;
  private string sourceField;

  [XmlElement("LinearGradientBrush", typeof (LinearGradientBrush))]
  [XmlElement("Canvas", typeof (Canvas))]
  [XmlElement("Glyphs", typeof (Glyphs))]
  [XmlElement("ImageBrush", typeof (ImageBrush))]
  [XmlElement("MatrixTransform", typeof (MatrixTransform))]
  [XmlElement("Path", typeof (Path))]
  [XmlElement("PathGeometry", typeof (PathGeometry))]
  [XmlElement("RadialGradientBrush", typeof (RadialGradientBrush))]
  [XmlElement("SolidColorBrush", typeof (SolidColorBrush))]
  [XmlElement("VisualBrush", typeof (VisualBrush))]
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
