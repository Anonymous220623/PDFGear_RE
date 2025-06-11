// Decompiled with JetBrains decompiler
// Type: NLog.Internal.SetupLoadConfigurationBuilder
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;

#nullable disable
namespace NLog.Internal;

internal class SetupLoadConfigurationBuilder : ISetupLoadConfigurationBuilder
{
  internal LoggingConfiguration _configuration;

  internal SetupLoadConfigurationBuilder(LogFactory logFactory, LoggingConfiguration configuration)
  {
    this.LogFactory = logFactory;
    this.Configuration = configuration;
  }

  public LogFactory LogFactory { get; }

  public LoggingConfiguration Configuration
  {
    get => this._configuration ?? (this._configuration = new LoggingConfiguration(this.LogFactory));
    set => this._configuration = value;
  }
}
