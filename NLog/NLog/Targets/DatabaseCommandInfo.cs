// Decompiled with JetBrains decompiler
// Type: NLog.Targets.DatabaseCommandInfo
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Layouts;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

#nullable disable
namespace NLog.Targets;

[NLogConfigurationItem]
public class DatabaseCommandInfo
{
  public DatabaseCommandInfo()
  {
    this.Parameters = (IList<DatabaseParameterInfo>) new List<DatabaseParameterInfo>();
    this.CommandType = CommandType.Text;
  }

  [RequiredParameter]
  [DefaultValue(CommandType.Text)]
  public CommandType CommandType { get; set; }

  public Layout ConnectionString { get; set; }

  [RequiredParameter]
  public Layout Text { get; set; }

  public bool IgnoreFailures { get; set; }

  [ArrayParameter(typeof (DatabaseParameterInfo), "parameter")]
  public IList<DatabaseParameterInfo> Parameters { get; private set; }
}
