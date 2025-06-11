// Decompiled with JetBrains decompiler
// Type: NLog.Internal.AsyncHelpersTask
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System.Threading;

#nullable disable
namespace NLog.Internal;

internal struct AsyncHelpersTask(WaitCallback asyncDelegate)
{
  public readonly WaitCallback AsyncDelegate = asyncDelegate;
}
