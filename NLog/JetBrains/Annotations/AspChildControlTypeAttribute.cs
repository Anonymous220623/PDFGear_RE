// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.AspChildControlTypeAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace JetBrains.Annotations;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal sealed class AspChildControlTypeAttribute : Attribute
{
  public AspChildControlTypeAttribute([NotNull] string tagName, [NotNull] Type controlType)
  {
    this.TagName = tagName;
    this.ControlType = controlType;
  }

  [NotNull]
  public string TagName { get; private set; }

  [NotNull]
  public Type ControlType { get; private set; }
}
