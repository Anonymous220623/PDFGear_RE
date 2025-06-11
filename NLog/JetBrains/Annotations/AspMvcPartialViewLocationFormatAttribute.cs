// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.AspMvcPartialViewLocationFormatAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace JetBrains.Annotations;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
internal sealed class AspMvcPartialViewLocationFormatAttribute : Attribute
{
  public AspMvcPartialViewLocationFormatAttribute([NotNull] string format) => this.Format = format;

  [NotNull]
  public string Format { get; private set; }
}
