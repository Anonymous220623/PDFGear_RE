// Decompiled with JetBrains decompiler
// Type: NLog.Internal.Fakeables.IAppEnvironment
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System.Collections.Generic;

#nullable disable
namespace NLog.Internal.Fakeables;

internal interface IAppEnvironment : IFileSystem
{
  IAppDomain AppDomain { get; }

  string AppDomainBaseDirectory { get; }

  string AppDomainConfigurationFile { get; }

  string CurrentProcessFilePath { get; }

  string CurrentProcessBaseName { get; }

  int CurrentProcessId { get; }

  string EntryAssemblyLocation { get; }

  string EntryAssemblyFileName { get; }

  string UserTempFilePath { get; }

  IEnumerable<string> PrivateBinPath { get; }
}
