// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.StringFormatMethodAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace JetBrains.Annotations;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Delegate)]
internal sealed class StringFormatMethodAttribute : Attribute
{
  public StringFormatMethodAttribute([NotNull] string formatParameterName)
  {
    this.FormatParameterName = formatParameterName;
  }

  [NotNull]
  public string FormatParameterName { get; private set; }
}
