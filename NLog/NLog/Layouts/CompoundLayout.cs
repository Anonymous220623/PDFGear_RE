// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.CompoundLayout
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NLog.Layouts;

[Layout("CompoundLayout")]
[NLog.Config.ThreadAgnostic]
[NLog.Config.ThreadSafe]
[AppDomainFixedOutput]
public class CompoundLayout : Layout
{
  public CompoundLayout() => this.Layouts = (IList<Layout>) new List<Layout>();

  [ArrayParameter(typeof (Layout), "layout")]
  public IList<Layout> Layouts { get; private set; }

  protected override void InitializeLayout()
  {
    base.InitializeLayout();
    foreach (Layout layout in (IEnumerable<Layout>) this.Layouts)
      layout.Initialize(this.LoggingConfiguration);
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
    for (int index = 0; index < this.Layouts.Count; ++index)
      this.Layouts[index].RenderAppendBuilder(logEvent, target);
  }

  protected override void CloseLayout()
  {
    foreach (Layout layout in (IEnumerable<Layout>) this.Layouts)
      layout.Close();
    base.CloseLayout();
  }

  public override string ToString()
  {
    return this.ToStringWithNestedItems<Layout>(this.Layouts, (Func<Layout, string>) (l => l.ToString()));
  }
}
