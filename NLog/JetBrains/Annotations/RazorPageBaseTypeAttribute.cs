// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.RazorPageBaseTypeAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace JetBrains.Annotations;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
internal sealed class RazorPageBaseTypeAttribute : Attribute
{
  public RazorPageBaseTypeAttribute([NotNull] string baseType) => this.BaseType = baseType;

  public RazorPageBaseTypeAttribute([NotNull] string baseType, string pageName)
  {
    this.BaseType = baseType;
    this.PageName = pageName;
  }

  [NotNull]
  public string BaseType { get; private set; }

  [CanBeNull]
  public string PageName { get; private set; }
}
