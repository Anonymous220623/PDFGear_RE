// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OutlineEntry
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
[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06/documentstructure")]
[XmlRoot("OutlineEntry", Namespace = "http://schemas.microsoft.com/xps/2005/06/documentstructure", IsNullable = false)]
[Serializable]
public class OutlineEntry
{
  private int outlineLevelField;
  private string outlineTargetField;
  private string descriptionField;
  private string langField;

  public OutlineEntry() => this.outlineLevelField = 1;

  [XmlAttribute]
  [DefaultValue(1)]
  public int OutlineLevel
  {
    get => this.outlineLevelField;
    set => this.outlineLevelField = value;
  }

  [XmlAttribute(DataType = "anyURI")]
  public string OutlineTarget
  {
    get => this.outlineTargetField;
    set => this.outlineTargetField = value;
  }

  [XmlAttribute]
  public string Description
  {
    get => this.descriptionField;
    set => this.descriptionField = value;
  }

  [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
  public string lang
  {
    get => this.langField;
    set => this.langField = value;
  }
}
