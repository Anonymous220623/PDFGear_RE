// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.IVbaModules
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.Office;

public interface IVbaModules : IEnumerable
{
  int Count { get; }

  IVbaModule this[int index] { get; }

  IVbaModule this[string name] { get; }

  IVbaModule Add(string name, VbaModuleType type);

  void Remove(string name);

  void RemoveAt(int index);

  void Clear();
}
