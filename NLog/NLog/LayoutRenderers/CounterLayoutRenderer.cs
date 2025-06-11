// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.CounterLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using NLog.Layouts;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("counter")]
public class CounterLayoutRenderer : LayoutRenderer
{
  private static Dictionary<string, int> sequences = new Dictionary<string, int>();

  [DefaultValue(1)]
  public int Value { get; set; } = 1;

  [DefaultValue(1)]
  public int Increment { get; set; } = 1;

  public Layout Sequence { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    int nextValue = this.GetNextValue(logEvent);
    builder.AppendInvariant(nextValue);
  }

  private int GetNextValue(LogEventInfo logEvent)
  {
    int nextSequenceValue;
    if (this.Sequence != null)
    {
      nextSequenceValue = CounterLayoutRenderer.GetNextSequenceValue(this.Sequence.Render(logEvent), this.Value, this.Increment);
    }
    else
    {
      nextSequenceValue = this.Value;
      this.Value += this.Increment;
    }
    return nextSequenceValue;
  }

  private static int GetNextSequenceValue(string sequenceName, int defaultValue, int increment)
  {
    lock (CounterLayoutRenderer.sequences)
    {
      int num1;
      if (!CounterLayoutRenderer.sequences.TryGetValue(sequenceName, out num1))
        num1 = defaultValue;
      int nextSequenceValue = num1;
      int num2 = num1 + increment;
      CounterLayoutRenderer.sequences[sequenceName] = num2;
      return nextSequenceValue;
    }
  }
}
