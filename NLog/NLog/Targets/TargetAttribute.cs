// Decompiled with JetBrains decompiler
// Type: NLog.Targets.TargetAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System;

#nullable disable
namespace NLog.Targets;

[AttributeUsage(AttributeTargets.Class)]
public sealed class TargetAttribute(string name) : NameBaseAttribute(name)
{
  public bool IsWrapper { get; set; }

  public bool IsCompound { get; set; }
}
