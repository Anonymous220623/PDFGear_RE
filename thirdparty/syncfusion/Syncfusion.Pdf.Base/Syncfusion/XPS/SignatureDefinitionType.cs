// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.SignatureDefinitionType
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
[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06/signature-definitions")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[Serializable]
public class SignatureDefinitionType
{
  private SpotLocationType spotLocationField;
  private string intentField;
  private DateTime signByField;
  private bool signByFieldSpecified;
  private string signingLocationField;
  private string spotIDField;
  private string signerNameField;
  private string langField;

  public SpotLocationType SpotLocation
  {
    get => this.spotLocationField;
    set => this.spotLocationField = value;
  }

  public string Intent
  {
    get => this.intentField;
    set => this.intentField = value;
  }

  public DateTime SignBy
  {
    get => this.signByField;
    set => this.signByField = value;
  }

  [XmlIgnore]
  public bool SignBySpecified
  {
    get => this.signByFieldSpecified;
    set => this.signByFieldSpecified = value;
  }

  public string SigningLocation
  {
    get => this.signingLocationField;
    set => this.signingLocationField = value;
  }

  [XmlAttribute(DataType = "ID")]
  public string SpotID
  {
    get => this.spotIDField;
    set => this.spotIDField = value;
  }

  [XmlAttribute]
  public string SignerName
  {
    get => this.signerNameField;
    set => this.signerNameField = value;
  }

  [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
  public string lang
  {
    get => this.langField;
    set => this.langField = value;
  }
}
