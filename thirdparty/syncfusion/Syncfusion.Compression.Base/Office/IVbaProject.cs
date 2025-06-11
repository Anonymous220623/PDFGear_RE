// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.IVbaProject
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

public interface IVbaProject
{
  string Name { get; set; }

  string Description { get; set; }

  string Constants { get; set; }

  string HelpFile { get; set; }

  uint HelpContextId { get; set; }

  IVbaModules Modules { get; }
}
