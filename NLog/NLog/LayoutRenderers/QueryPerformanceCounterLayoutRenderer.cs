// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.QueryPerformanceCounterLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("qpc")]
public class QueryPerformanceCounterLayoutRenderer : LayoutRenderer
{
  private bool raw;
  private ulong firstQpcValue;
  private ulong lastQpcValue;
  private double frequency = 1.0;

  [DefaultValue(true)]
  public bool Normalize { get; set; } = true;

  [DefaultValue(false)]
  public bool Difference { get; set; }

  [DefaultValue(true)]
  public bool Seconds
  {
    get => !this.raw;
    set => this.raw = !value;
  }

  [DefaultValue(4)]
  public int Precision { get; set; } = 4;

  [DefaultValue(true)]
  public bool AlignDecimalPoint { get; set; } = true;

  protected override void InitializeLayoutRenderer()
  {
    base.InitializeLayoutRenderer();
    ulong lpPerformanceFrequency;
    if (!NativeMethods.QueryPerformanceFrequency(out lpPerformanceFrequency))
      throw new InvalidOperationException("Cannot determine high-performance counter frequency.");
    ulong lpPerformanceCount;
    if (!NativeMethods.QueryPerformanceCounter(out lpPerformanceCount))
      throw new InvalidOperationException("Cannot determine high-performance counter value.");
    this.frequency = (double) lpPerformanceFrequency;
    this.firstQpcValue = lpPerformanceCount;
    this.lastQpcValue = lpPerformanceCount;
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    ulong? nullable = this.GetValue();
    if (!nullable.HasValue)
      return;
    string str;
    if (this.Seconds)
    {
      str = Convert.ToString(this.ToSeconds(nullable.Value), (IFormatProvider) CultureInfo.InvariantCulture);
      if (this.AlignDecimalPoint)
      {
        int num = str.IndexOf('.');
        str = num != -1 ? str + new string('0', this.Precision - (str.Length - 1 - num)) : $"{str}.{new string('0', this.Precision)}";
      }
    }
    else
      str = Convert.ToString((object) nullable, (IFormatProvider) CultureInfo.InvariantCulture);
    builder.Append(str);
  }

  private double ToSeconds(ulong qpcValue)
  {
    return Math.Round((double) qpcValue / this.frequency, this.Precision);
  }

  private ulong? GetValue()
  {
    ulong lpPerformanceCount;
    if (!NativeMethods.QueryPerformanceCounter(out lpPerformanceCount))
      return new ulong?();
    ulong num = lpPerformanceCount;
    if (this.Difference)
      lpPerformanceCount -= this.lastQpcValue;
    else if (this.Normalize)
      lpPerformanceCount -= this.firstQpcValue;
    this.lastQpcValue = num;
    return new ulong?(lpPerformanceCount);
  }
}
