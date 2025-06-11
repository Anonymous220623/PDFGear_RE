// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.Log4JXmlEventLayout
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.LayoutRenderers;
using NLog.Targets;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NLog.Layouts;

[Layout("Log4JXmlEventLayout")]
[NLog.Config.ThreadAgnostic]
[NLog.Config.ThreadSafe]
[AppDomainFixedOutput]
public class Log4JXmlEventLayout : Layout, IIncludeContext
{
  public Log4JXmlEventLayout()
  {
    this.Renderer = new Log4JXmlEventLayoutRenderer();
    this.Parameters = (IList<NLogViewerParameterInfo>) new List<NLogViewerParameterInfo>();
    this.Renderer.Parameters = this.Parameters;
  }

  public Log4JXmlEventLayoutRenderer Renderer { get; }

  [ArrayParameter(typeof (NLogViewerParameterInfo), "parameter")]
  public IList<NLogViewerParameterInfo> Parameters
  {
    get => this.Renderer.Parameters;
    set => this.Renderer.Parameters = value;
  }

  public bool IncludeMdc
  {
    get => this.Renderer.IncludeMdc;
    set => this.Renderer.IncludeMdc = value;
  }

  public bool IncludeAllProperties
  {
    get => this.Renderer.IncludeAllProperties;
    set => this.Renderer.IncludeAllProperties = value;
  }

  public bool IncludeNdc
  {
    get => this.Renderer.IncludeNdc;
    set => this.Renderer.IncludeNdc = value;
  }

  public bool IncludeMdlc
  {
    get => this.Renderer.IncludeMdlc;
    set => this.Renderer.IncludeMdlc = value;
  }

  public bool IncludeNdlc
  {
    get => this.Renderer.IncludeNdlc;
    set => this.Renderer.IncludeNdlc = value;
  }

  public bool IncludeCallSite
  {
    get => this.Renderer.IncludeCallSite;
    set => this.Renderer.IncludeCallSite = value;
  }

  public bool IncludeSourceInfo
  {
    get => this.Renderer.IncludeSourceInfo;
    set => this.Renderer.IncludeSourceInfo = value;
  }

  internal override void PrecalculateBuilder(LogEventInfo logEvent, StringBuilder target)
  {
    this.PrecalculateBuilderInternal(logEvent, target);
  }

  protected override string GetFormattedMessage(LogEventInfo logEvent)
  {
    return this.RenderAllocateBuilder(logEvent);
  }

  protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
  {
    this.Renderer.RenderAppendBuilder(logEvent, target);
  }
}
