// Decompiled with JetBrains decompiler
// Type: NLog.Internal.Fakeables.IAppDomain
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace NLog.Internal.Fakeables;

public interface IAppDomain
{
  string BaseDirectory { get; }

  string ConfigurationFile { get; }

  IEnumerable<string> PrivateBinPath { get; }

  string FriendlyName { get; }

  int Id { get; }

  IEnumerable<Assembly> GetAssemblies();

  event EventHandler<EventArgs> ProcessExit;

  event EventHandler<EventArgs> DomainUnload;
}
