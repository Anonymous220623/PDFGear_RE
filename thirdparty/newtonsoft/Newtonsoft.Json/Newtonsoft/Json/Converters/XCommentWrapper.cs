// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XCommentWrapper
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Xml.Linq;

#nullable enable
namespace Newtonsoft.Json.Converters;

internal class XCommentWrapper(XComment text) : XObjectWrapper((XObject) text)
{
  private XComment Text => (XComment) this.WrappedNode;

  public override string? Value
  {
    get => this.Text.Value;
    set => this.Text.Value = value;
  }

  public override IXmlNode? ParentNode
  {
    get
    {
      return this.Text.Parent == null ? (IXmlNode) null : XContainerWrapper.WrapNode((XObject) this.Text.Parent);
    }
  }
}
