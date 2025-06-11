// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.IStyleCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public interface IStyleCollection : ICollectionBase, IEnumerable
{
  IStyle this[int index] { get; }

  bool FixedIndex13HasStyle { get; set; }

  bool FixedIndex14HasStyle { get; set; }

  string FixedIndex13StyleName { get; set; }

  string FixedIndex14StyleName { get; set; }

  int Add(IStyle style);

  IStyle FindByName(string name);

  IStyle FindByName(string name, StyleType styleType);

  IStyle FindById(int styleId);
}
