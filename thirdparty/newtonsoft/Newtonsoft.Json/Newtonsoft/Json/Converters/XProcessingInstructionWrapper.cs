// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XProcessingInstructionWrapper
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Xml.Linq;

#nullable enable
namespace Newtonsoft.Json.Converters;

internal class XProcessingInstructionWrapper(XProcessingInstruction processingInstruction) : 
  XObjectWrapper((XObject) processingInstruction)
{
  private XProcessingInstruction ProcessingInstruction => (XProcessingInstruction) this.WrappedNode;

  public override string? LocalName => this.ProcessingInstruction.Target;

  public override string? Value
  {
    get => this.ProcessingInstruction.Data;
    set => this.ProcessingInstruction.Data = value;
  }
}
