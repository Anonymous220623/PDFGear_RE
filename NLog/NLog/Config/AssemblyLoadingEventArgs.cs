// Decompiled with JetBrains decompiler
// Type: NLog.Config.AssemblyLoadingEventArgs
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System.ComponentModel;
using System.Reflection;

#nullable disable
namespace NLog.Config;

public class AssemblyLoadingEventArgs : CancelEventArgs
{
  public AssemblyLoadingEventArgs(Assembly assembly) => this.Assembly = assembly;

  public Assembly Assembly { get; private set; }
}
