// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.MatrixTransform
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

[DesignerCategory("code")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06")]
[XmlRoot("MatrixTransform", Namespace = "http://schemas.microsoft.com/xps/2005/06", IsNullable = false)]
[GeneratedCode("xsd", "2.0.50727.3038")]
[Serializable]
public class MatrixTransform
{
  private string matrixField;
  private string keyField;

  [XmlAttribute]
  public string Matrix
  {
    get => this.matrixField;
    set => this.matrixField = value;
  }

  [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://schemas.microsoft.com/xps/2005/06/resourcedictionary-key")]
  public string Key
  {
    get => this.keyField;
    set => this.keyField = value;
  }
}
