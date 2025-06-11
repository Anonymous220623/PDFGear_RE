// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.MachineNameLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("machinename")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public class MachineNameLayoutRenderer : LayoutRenderer
{
  private string _machineName;

  protected override void InitializeLayoutRenderer()
  {
    base.InitializeLayoutRenderer();
    try
    {
      this._machineName = EnvironmentHelper.GetMachineName();
      if (!string.IsNullOrEmpty(this._machineName))
        return;
      InternalLogger.Info("MachineName is not available.");
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "Error getting machine name.");
      if (ex.MustBeRethrown())
        throw;
      this._machineName = string.Empty;
    }
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    builder.Append(this._machineName);
  }
}
