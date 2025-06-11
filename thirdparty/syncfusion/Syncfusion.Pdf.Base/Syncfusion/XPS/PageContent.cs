// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.PageContent
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

[DesignerCategory("code")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06")]
[XmlRoot("PageContent", Namespace = "http://schemas.microsoft.com/xps/2005/06", IsNullable = false)]
[Serializable]
public class PageContent
{
  private LinkTarget[] pageContentLinkTargetsField;
  private string sourceField;
  private double widthField;
  private bool widthFieldSpecified;
  private double heightField;
  private bool heightFieldSpecified;

  [XmlArray("PageContent.LinkTargets")]
  [XmlArrayItem("LinkTarget", IsNullable = false)]
  public LinkTarget[] PageContentLinkTargets
  {
    get => this.pageContentLinkTargetsField;
    set => this.pageContentLinkTargetsField = value;
  }

  [XmlAttribute(DataType = "anyURI")]
  public string Source
  {
    get => this.sourceField;
    set => this.sourceField = value;
  }

  [XmlAttribute]
  public double Width
  {
    get => this.widthField;
    set => this.widthField = value;
  }

  [XmlIgnore]
  public bool WidthSpecified
  {
    get => this.widthFieldSpecified;
    set => this.widthFieldSpecified = value;
  }

  [XmlAttribute]
  public double Height
  {
    get => this.heightField;
    set => this.heightField = value;
  }

  [XmlIgnore]
  public bool HeightSpecified
  {
    get => this.heightFieldSpecified;
    set => this.heightFieldSpecified = value;
  }
}
