// Decompiled with JetBrains decompiler
// Type: NLog.Config.ILoggingConfigurationLoader
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NLog.Config;

internal interface ILoggingConfigurationLoader : IDisposable
{
  LoggingConfiguration Load(LogFactory logFactory, string filename = null);

  void Activated(LogFactory logFactory, LoggingConfiguration config);

  IEnumerable<string> GetDefaultCandidateConfigFilePaths(string filename = null);
}
