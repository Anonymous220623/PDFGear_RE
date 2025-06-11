// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.IOfficeMathNArray
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

public interface IOfficeMathNArray : IOfficeMathFunctionBase, IOfficeMathEntity
{
  string NArrayCharacter { get; set; }

  bool HasGrow { get; set; }

  bool HideLowerLimit { get; set; }

  bool HideUpperLimit { get; set; }

  bool SubSuperscriptLimit { get; set; }

  IOfficeMath Equation { get; }

  IOfficeMath Subscript { get; }

  IOfficeMath Superscript { get; }

  IOfficeRunFormat ControlProperties { get; set; }
}
