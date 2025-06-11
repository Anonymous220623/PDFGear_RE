// Decompiled with JetBrains decompiler
// Type: NLog.Config.NameBaseAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using System;

#nullable disable
namespace NLog.Config;

[MeansImplicitUse]
public abstract class NameBaseAttribute : Attribute
{
  protected NameBaseAttribute(string name) => this.Name = name;

  public string Name { get; private set; }
}
