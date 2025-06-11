// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.IOfficeMathGroupCharacter
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

public interface IOfficeMathGroupCharacter : IOfficeMathFunctionBase, IOfficeMathEntity
{
  bool HasAlignTop { get; set; }

  string GroupCharacter { get; set; }

  bool HasCharacterTop { get; set; }

  IOfficeMath Equation { get; }

  IOfficeRunFormat ControlProperties { get; set; }
}
