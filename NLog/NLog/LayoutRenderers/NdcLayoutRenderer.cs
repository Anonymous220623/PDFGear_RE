// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.NdcLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("ndc")]
[ThreadSafe]
public class NdcLayoutRenderer : LayoutRenderer
{
  public NdcLayoutRenderer()
  {
    this.Separator = " ";
    this.BottomFrames = -1;
    this.TopFrames = -1;
  }

  public int TopFrames { get; set; }

  public int BottomFrames { get; set; }

  public string Separator { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    if (this.TopFrames == 1)
    {
      object message = NestedDiagnosticsContext.PeekObject();
      if (message == null)
        return;
      NdcLayoutRenderer.AppendAsString(message, this.GetFormatProvider(logEvent), builder);
    }
    else
    {
      object[] allObjects = NestedDiagnosticsContext.GetAllObjects();
      if (allObjects.Length == 0)
        return;
      int num1 = 0;
      int num2 = allObjects.Length;
      if (this.TopFrames != -1)
        num2 = Math.Min(this.TopFrames, allObjects.Length);
      else if (this.BottomFrames != -1)
        num1 = allObjects.Length - Math.Min(this.BottomFrames, allObjects.Length);
      IFormatProvider formatProvider = this.GetFormatProvider(logEvent);
      string str = string.Empty;
      for (int index = num2 - 1; index >= num1; --index)
      {
        builder.Append(str);
        NdcLayoutRenderer.AppendAsString(allObjects[index], formatProvider, builder);
        str = this.Separator;
      }
    }
  }

  private static void AppendAsString(
    object message,
    IFormatProvider formatProvider,
    StringBuilder builder)
  {
    string str = Convert.ToString(message, formatProvider);
    builder.Append(str);
  }
}
