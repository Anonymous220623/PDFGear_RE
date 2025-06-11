// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.GarbageCollectorInfoLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("gc")]
[ThreadSafe]
public class GarbageCollectorInfoLayoutRenderer : LayoutRenderer
{
  [DefaultValue("TotalMemory")]
  public GarbageCollectorProperty Property { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    long num = this.GetValue();
    if (num >= 0L && num < (long) uint.MaxValue)
      builder.AppendInvariant((uint) num);
    else
      builder.Append(num.ToString());
  }

  private long GetValue()
  {
    long num = 0;
    switch (this.Property)
    {
      case GarbageCollectorProperty.TotalMemory:
        num = GC.GetTotalMemory(false);
        break;
      case GarbageCollectorProperty.TotalMemoryForceCollection:
        num = GC.GetTotalMemory(true);
        break;
      case GarbageCollectorProperty.CollectionCount0:
        num = (long) GC.CollectionCount(0);
        break;
      case GarbageCollectorProperty.CollectionCount1:
        num = (long) GC.CollectionCount(1);
        break;
      case GarbageCollectorProperty.CollectionCount2:
        num = (long) GC.CollectionCount(2);
        break;
      case GarbageCollectorProperty.MaxGeneration:
        num = (long) GC.MaxGeneration;
        break;
    }
    return num;
  }
}
