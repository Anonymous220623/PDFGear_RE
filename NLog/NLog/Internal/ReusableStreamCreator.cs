// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ReusableStreamCreator
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.IO;

#nullable disable
namespace NLog.Internal;

internal sealed class ReusableStreamCreator(int capacity) : 
  ReusableObjectCreator<MemoryStream>(capacity, (Func<int, MemoryStream>) (cap => new MemoryStream(cap)), (Action<MemoryStream>) (m =>
  {
    m.Position = 0L;
    m.SetLength(0L);
  })),
  IDisposable
{
  void IDisposable.Dispose() => this._reusableObject.Dispose();
}
