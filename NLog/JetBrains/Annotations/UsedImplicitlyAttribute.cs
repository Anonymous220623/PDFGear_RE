// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.UsedImplicitlyAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace JetBrains.Annotations;

[AttributeUsage(AttributeTargets.All)]
internal sealed class UsedImplicitlyAttribute : Attribute
{
  public UsedImplicitlyAttribute()
    : this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
  {
  }

  public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags)
    : this(useKindFlags, ImplicitUseTargetFlags.Default)
  {
  }

  public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
    : this(ImplicitUseKindFlags.Default, targetFlags)
  {
  }

  public UsedImplicitlyAttribute(
    ImplicitUseKindFlags useKindFlags,
    ImplicitUseTargetFlags targetFlags)
  {
    this.UseKindFlags = useKindFlags;
    this.TargetFlags = targetFlags;
  }

  public ImplicitUseKindFlags UseKindFlags { get; private set; }

  public ImplicitUseTargetFlags TargetFlags { get; private set; }
}
