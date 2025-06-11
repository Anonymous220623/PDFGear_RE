// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSBrush
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

[XmlRoot("Glyphs.OpacityMask", Namespace = "http://schemas.openxps.org/oxps/v1.0", IsNullable = false)]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.openxps.org/oxps/v1.0")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[Serializable]
public class OXPSBrush
{
  private object itemField;

  [XmlElement("VisualBrush", typeof (OXPSVisualBrush))]
  [XmlElement("ImageBrush", typeof (OXPSImageBrush))]
  [XmlElement("LinearGradientBrush", typeof (OXPSLinearGradientBrush))]
  [XmlElement("RadialGradientBrush", typeof (OXPSRadialGradientBrush))]
  [XmlElement("SolidColorBrush", typeof (OXPSSolidColorBrush))]
  public object Item
  {
    get => this.itemField;
    set => this.itemField = value;
  }
}
