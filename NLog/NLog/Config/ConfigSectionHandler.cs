// Decompiled with JetBrains decompiler
// Type: NLog.Config.ConfigSectionHandler
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal.Fakeables;
using System;
using System.Configuration;
using System.Xml;

#nullable disable
namespace NLog.Config;

public sealed class ConfigSectionHandler : IConfigurationSectionHandler
{
  private object Create(XmlNode section, IAppDomain appDomain)
  {
    try
    {
      string configurationFile = appDomain.ConfigurationFile;
      return (object) new XmlLoggingConfiguration(section.OuterXml, configurationFile, LogManager.LogFactory);
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "ConfigSectionHandler error.");
      throw;
    }
  }

  object IConfigurationSectionHandler.Create(object parent, object configContext, XmlNode section)
  {
    return this.Create(section, LogFactory.CurrentAppDomain);
  }
}
