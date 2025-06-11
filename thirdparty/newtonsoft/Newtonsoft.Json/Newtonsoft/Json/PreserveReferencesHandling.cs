// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.PreserveReferencesHandling
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;

#nullable disable
namespace Newtonsoft.Json;

[Flags]
public enum PreserveReferencesHandling
{
  None = 0,
  Objects = 1,
  Arrays = 2,
  All = Arrays | Objects, // 0x00000003
}
