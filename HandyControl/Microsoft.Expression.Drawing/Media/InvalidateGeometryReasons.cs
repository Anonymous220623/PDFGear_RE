// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Media.InvalidateGeometryReasons
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;

#nullable disable
namespace HandyControl.Expression.Media;

[Flags]
public enum InvalidateGeometryReasons
{
  ChildInvalidated = 4,
  IsAnimated = 2,
  ParentInvalidated = 8,
  PropertyChanged = 1,
  TemplateChanged = 16, // 0x00000010
}
