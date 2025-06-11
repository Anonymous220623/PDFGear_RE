// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.XmpSimpleType
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public class XmpSimpleType : XmpType
{
  public string Value
  {
    get => this.XmlData != null ? this.XmlData.InnerXml : string.Empty;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Value));
      XmpUtils.SetTextValue(this.XmlData, value);
    }
  }

  internal bool RemoveNode
  {
    get
    {
      if (this.XmlData == null)
        return false;
      this.EntityParent.RemoveChild((XmlNode) this.XmlData);
      return true;
    }
  }

  internal XmpSimpleType(
    XmpMetadata xmp,
    XmlNode parent,
    string prefix,
    string localName,
    string namespaceURI)
    : base(xmp, parent, prefix, localName, namespaceURI)
  {
  }

  protected internal void SetBool(bool value) => XmpUtils.SetBoolValue(this.XmlData, value);

  protected internal bool GetBool() => XmpUtils.GetBoolValue(this.Value);

  protected internal void SetReal(float value) => XmpUtils.SetRealValue(this.XmlData, value);

  protected internal float GetReal() => XmpUtils.GetRealValue(this.Value);

  protected internal void SetInt(int value) => XmpUtils.SetIntValue(this.XmlData, value);

  protected internal int GetInt() => XmpUtils.GetIntValue(this.Value);

  protected internal void SetUri(Uri value) => XmpUtils.SetUriValue(this.XmlData, value);

  protected internal Uri GetUri() => XmpUtils.GetUriValue(this.Value);

  protected internal void SetDateTime(DateTime value)
  {
    XmpUtils.SetDateTimeValue(this.XmlData, value);
  }

  protected internal DateTime GetDateTime() => XmpUtils.GetDateTimeValue(this.Value);

  protected override void CreateEntity()
  {
    this.EntityParent.AppendChild((XmlNode) this.Xmp.CreateElement(this.EntityPrefix, this.EntityName, this.EntityNamespaceURI));
  }
}
