// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.MustUseReturnValueAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace JetBrains.Annotations;

[AttributeUsage(AttributeTargets.Method)]
internal sealed class MustUseReturnValueAttribute : Attribute
{
  public MustUseReturnValueAttribute()
  {
  }

  public MustUseReturnValueAttribute([NotNull] string justification)
  {
    this.Justification = justification;
  }

  [CanBeNull]
  public string Justification { get; private set; }
}
