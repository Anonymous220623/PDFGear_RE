// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.EnvironmentLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("environment")]
[ThreadSafe]
public class EnvironmentLayoutRenderer : LayoutRenderer, IStringValueRenderer
{
  private KeyValuePair<string, SimpleLayout> _cachedValue;

  [RequiredParameter]
  [DefaultParameter]
  public string Variable { get; set; }

  public string Default { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    this.GetSimpleLayout()?.RenderAppendBuilder(logEvent, builder);
  }

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent)
  {
    SimpleLayout simpleLayout = this.GetSimpleLayout();
    if (simpleLayout == null)
      return string.Empty;
    return simpleLayout.IsFixedText || simpleLayout.IsSimpleStringText ? simpleLayout.Render(logEvent) : (string) null;
  }

  private SimpleLayout GetSimpleLayout()
  {
    if (this.Variable != null)
    {
      string environmentVariable = EnvironmentHelper.GetSafeEnvironmentVariable(this.Variable);
      if (string.IsNullOrEmpty(environmentVariable))
        environmentVariable = this.Default;
      if (!string.IsNullOrEmpty(environmentVariable))
      {
        KeyValuePair<string, SimpleLayout> keyValuePair = this._cachedValue;
        if (string.CompareOrdinal(keyValuePair.Key, environmentVariable) != 0)
        {
          keyValuePair = new KeyValuePair<string, SimpleLayout>(environmentVariable, new SimpleLayout(environmentVariable));
          this._cachedValue = keyValuePair;
        }
        return keyValuePair.Value;
      }
    }
    return (SimpleLayout) null;
  }
}
