// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.ContractAnnotationAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace JetBrains.Annotations;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
internal sealed class ContractAnnotationAttribute : Attribute
{
  public ContractAnnotationAttribute([NotNull] string contract)
    : this(contract, false)
  {
  }

  public ContractAnnotationAttribute([NotNull] string contract, bool forceFullStates)
  {
    this.Contract = contract;
    this.ForceFullStates = forceFullStates;
  }

  [NotNull]
  public string Contract { get; private set; }

  public bool ForceFullStates { get; private set; }
}
