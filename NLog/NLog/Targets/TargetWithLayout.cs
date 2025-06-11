// Decompiled with JetBrains decompiler
// Type: NLog.Targets.TargetWithLayout
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Layouts;
using System.ComponentModel;

#nullable disable
namespace NLog.Targets;

public abstract class TargetWithLayout : Target
{
  protected TargetWithLayout()
  {
    this.Layout = (Layout) "${longdate}|${level:uppercase=true}|${logger}|${message}";
  }

  [RequiredParameter]
  [DefaultValue("${longdate}|${level:uppercase=true}|${logger}|${message}")]
  public virtual Layout Layout { get; set; }
}
