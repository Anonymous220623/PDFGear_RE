// Decompiled with JetBrains decompiler
// Type: NLog.Config.ArrayParameterAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using System;

#nullable disable
namespace NLog.Config;

[AttributeUsage(AttributeTargets.Property)]
[MeansImplicitUse]
public sealed class ArrayParameterAttribute : Attribute
{
  public ArrayParameterAttribute(Type itemType, string elementName)
  {
    this.ItemType = itemType;
    this.ElementName = elementName;
  }

  public Type ItemType { get; private set; }

  public string ElementName { get; private set; }
}
