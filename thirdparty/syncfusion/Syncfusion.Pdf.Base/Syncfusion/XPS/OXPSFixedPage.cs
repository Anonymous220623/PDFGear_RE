// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSFixedPage
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

[XmlType(Namespace = "http://schemas.openxps.org/oxps/v1.0")]
[XmlRoot("FixedPage", Namespace = "http://schemas.openxps.org/oxps/v1.0", IsNullable = false)]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[Serializable]
public class OXPSFixedPage
{
  private OXPSResources fixedPageResourcesField;
  private object[] itemsField;
  private double widthField;
  private double heightField;
  private string contentBoxField;
  private string bleedBoxField;
  private string langField;
  private string nameField;

  [XmlElement("FixedPage.Resources")]
  public OXPSResources FixedPageResources
  {
    get => this.fixedPageResourcesField;
    set => this.fixedPageResourcesField = value;
  }

  [XmlElement("Path", typeof (OXPSPath))]
  [XmlElement("Glyphs", typeof (OXPSGlyphs))]
  [XmlElement("Canvas", typeof (OXPSCanvas))]
  public object[] Items
  {
    get => this.itemsField;
    set => this.itemsField = value;
  }

  [XmlAttribute]
  public double Width
  {
    get => this.widthField;
    set => this.widthField = value;
  }

  [XmlAttribute]
  public double Height
  {
    get => this.heightField;
    set => this.heightField = value;
  }

  [XmlAttribute]
  public string ContentBox
  {
    get => this.contentBoxField;
    set => this.contentBoxField = value;
  }

  [XmlAttribute]
  public string BleedBox
  {
    get => this.bleedBoxField;
    set => this.bleedBoxField = value;
  }

  [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
  public string lang
  {
    get => this.langField;
    set => this.langField = value;
  }

  [XmlAttribute(DataType = "ID")]
  public string Name
  {
    get => this.nameField;
    set => this.nameField = value;
  }
}
