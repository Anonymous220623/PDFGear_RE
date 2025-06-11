// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.DirectorySeparatorLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("dir-separator")]
[ThreadAgnostic]
[ThreadSafe]
[AppDomainFixedOutput]
public class DirectorySeparatorLayoutRenderer : LayoutRenderer, IRawValue
{
  private static readonly char SeparatorChar = Path.DirectorySeparatorChar;

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    builder.Append(DirectorySeparatorLayoutRenderer.SeparatorChar);
  }

  public bool TryGetRawValue(LogEventInfo logEvent, out object value)
  {
    value = (object) DirectorySeparatorLayoutRenderer.SeparatorChar;
    return true;
  }
}
