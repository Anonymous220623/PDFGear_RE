// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.LocalizationRequiredAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace JetBrains.Annotations;

[AttributeUsage(AttributeTargets.All)]
internal sealed class LocalizationRequiredAttribute : Attribute
{
  public LocalizationRequiredAttribute()
    : this(true)
  {
  }

  public LocalizationRequiredAttribute(bool required) => this.Required = required;

  public bool Required { get; private set; }
}
