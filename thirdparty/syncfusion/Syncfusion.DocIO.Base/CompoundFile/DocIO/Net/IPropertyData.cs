// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Net.IPropertyData
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Net;

public interface IPropertyData
{
  object Value { get; }

  VarEnum Type { get; }

  string Name { get; }

  int Id { get; set; }

  bool SetValue(object value, PropertyType type);
}
