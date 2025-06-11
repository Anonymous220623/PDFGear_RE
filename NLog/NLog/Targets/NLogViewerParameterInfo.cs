// Decompiled with JetBrains decompiler
// Type: NLog.Targets.NLogViewerParameterInfo
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Layouts;

#nullable disable
namespace NLog.Targets;

[NLogConfigurationItem]
public class NLogViewerParameterInfo
{
  [RequiredParameter]
  public string Name { get; set; }

  [RequiredParameter]
  public Layout Layout { get; set; }

  public bool IncludeEmptyValue { get; set; } = true;
}
