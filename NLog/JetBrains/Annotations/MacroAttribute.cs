// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.MacroAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace JetBrains.Annotations;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
internal sealed class MacroAttribute : Attribute
{
  [CanBeNull]
  public string Expression { get; set; }

  public int Editable { get; set; }

  [CanBeNull]
  public string Target { get; set; }
}
