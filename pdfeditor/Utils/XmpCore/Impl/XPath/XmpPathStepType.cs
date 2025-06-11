// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.XPath.XmpPathStepType
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace XmpCore.Impl.XPath;

public enum XmpPathStepType
{
  SchemaNode = -2147483648, // 0x80000000
  StructFieldStep = 1,
  QualifierStep = 2,
  ArrayIndexStep = 3,
  ArrayLastStep = 4,
  QualSelectorStep = 5,
  FieldSelectorStep = 6,
}
