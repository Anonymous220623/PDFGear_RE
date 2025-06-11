// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.MeansImplicitUseAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace JetBrains.Annotations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.GenericParameter)]
internal sealed class MeansImplicitUseAttribute : Attribute
{
  public MeansImplicitUseAttribute()
    : this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
  {
  }

  public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags)
    : this(useKindFlags, ImplicitUseTargetFlags.Default)
  {
  }

  public MeansImplicitUseAttribute(ImplicitUseTargetFlags targetFlags)
    : this(ImplicitUseKindFlags.Default, targetFlags)
  {
  }

  public MeansImplicitUseAttribute(
    ImplicitUseKindFlags useKindFlags,
    ImplicitUseTargetFlags targetFlags)
  {
    this.UseKindFlags = useKindFlags;
    this.TargetFlags = targetFlags;
  }

  [UsedImplicitly]
  public ImplicitUseKindFlags UseKindFlags { get; private set; }

  [UsedImplicitly]
  public ImplicitUseTargetFlags TargetFlags { get; private set; }
}
