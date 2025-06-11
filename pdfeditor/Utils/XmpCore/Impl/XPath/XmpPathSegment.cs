// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.XPath.XmpPathSegment
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace XmpCore.Impl.XPath;

public sealed class XmpPathSegment
{
  public XmpPathSegment(string name) => this.Name = name;

  public XmpPathSegment(string name, XmpPathStepType kind)
  {
    this.Name = name;
    this.Kind = kind;
  }

  public XmpPathStepType Kind { get; set; }

  public string Name { get; set; }

  public bool IsAlias { get; set; }

  public int AliasForm { get; set; }

  public override string ToString()
  {
    switch (this.Kind)
    {
      case XmpPathStepType.StructFieldStep:
      case XmpPathStepType.QualifierStep:
      case XmpPathStepType.ArrayIndexStep:
      case XmpPathStepType.ArrayLastStep:
        return this.Name;
      case XmpPathStepType.QualSelectorStep:
      case XmpPathStepType.FieldSelectorStep:
        return this.Name;
      default:
        return this.Name;
    }
  }
}
