// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.IOfficeMathPhantom
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

public interface IOfficeMathPhantom : IOfficeMathFunctionBase, IOfficeMathEntity
{
  bool Show { get; set; }

  bool Transparent { get; set; }

  bool ZeroAscent { get; set; }

  bool ZeroDescent { get; set; }

  bool ZeroWidth { get; set; }

  IOfficeMath Equation { get; }

  IOfficeRunFormat ControlProperties { get; set; }
}
