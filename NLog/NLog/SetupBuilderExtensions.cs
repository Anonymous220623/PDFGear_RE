// Decompiled with JetBrains decompiler
// Type: NLog.SetupBuilderExtensions
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace NLog;

public static class SetupBuilderExtensions
{
  [MethodImpl(MethodImplOptions.NoInlining)]
  public static Logger GetCurrentClassLogger(this ISetupBuilder setupBuilder)
  {
    return setupBuilder.LogFactory.GetLogger(StackTraceUsageUtils.GetClassFullName());
  }

  public static Logger GetLogger(this ISetupBuilder setupBuilder, string name)
  {
    return setupBuilder.LogFactory.GetLogger(name);
  }

  public static ISetupBuilder SetupExtensions(
    this ISetupBuilder setupBuilder,
    Action<ISetupExtensionsBuilder> extensionsBuilder)
  {
    extensionsBuilder((ISetupExtensionsBuilder) new SetupExtensionsBuilder(setupBuilder.LogFactory));
    return setupBuilder;
  }

  public static ISetupBuilder SetupInternalLogger(
    this ISetupBuilder setupBuilder,
    Action<ISetupInternalLoggerBuilder> internalLoggerBuilder)
  {
    internalLoggerBuilder((ISetupInternalLoggerBuilder) new SetupInternalLoggerBuilder(setupBuilder.LogFactory));
    return setupBuilder;
  }

  public static ISetupBuilder SetupSerialization(
    this ISetupBuilder setupBuilder,
    Action<ISetupSerializationBuilder> serializationBuilder)
  {
    serializationBuilder((ISetupSerializationBuilder) new SetupSerializationBuilder(setupBuilder.LogFactory));
    return setupBuilder;
  }

  public static ISetupBuilder LoadConfiguration(
    this ISetupBuilder setupBuilder,
    Action<ISetupLoadConfigurationBuilder> configBuilder)
  {
    LoggingConfiguration config = setupBuilder.LogFactory._config;
    SetupLoadConfigurationBuilder configurationBuilder = new SetupLoadConfigurationBuilder(setupBuilder.LogFactory, config);
    configBuilder((ISetupLoadConfigurationBuilder) configurationBuilder);
    LoggingConfiguration configuration = configurationBuilder._configuration;
    bool flag = config != setupBuilder.LogFactory._config;
    if (configuration == setupBuilder.LogFactory._config)
      setupBuilder.LogFactory.ReconfigExistingLoggers();
    else if (!flag || config != configuration)
      setupBuilder.LogFactory.Configuration = configuration;
    return setupBuilder;
  }

  public static ISetupBuilder LoadConfiguration(
    this ISetupBuilder setupBuilder,
    LoggingConfiguration loggingConfiguration)
  {
    setupBuilder.LogFactory.Configuration = loggingConfiguration;
    return setupBuilder;
  }

  public static ISetupBuilder LoadConfigurationFromFile(
    this ISetupBuilder setupBuilder,
    string configFile = null,
    bool optional = true)
  {
    setupBuilder.LogFactory.LoadConfiguration(configFile, optional);
    return setupBuilder;
  }

  public static ISetupBuilder LoadConfigurationFromXml(
    this ISetupBuilder setupBuilder,
    string configXml)
  {
    setupBuilder.LogFactory.Configuration = (LoggingConfiguration) XmlLoggingConfiguration.CreateFromXmlString(configXml, setupBuilder.LogFactory);
    return setupBuilder;
  }
}
