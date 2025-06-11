// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.IVbaModule
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

public interface IVbaModule
{
  string Name { get; set; }

  VbaModuleType Type { get; }

  string Code { get; set; }

  object DesignerStorage { get; set; }
}
