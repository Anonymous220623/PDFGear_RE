// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Net.IPropertyData
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Net;

public interface IPropertyData
{
  object Value { get; }

  VarEnum Type { get; }

  string Name { get; }

  int Id { get; set; }

  bool SetValue(object value, PropertyType type);
}
