// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ReusableAsyncLogEventList
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace NLog.Internal;

internal class ReusableAsyncLogEventList(int capacity) : 
  ReusableObjectCreator<IList<AsyncLogEventInfo>>(capacity, (Func<int, IList<AsyncLogEventInfo>>) (cap => (IList<AsyncLogEventInfo>) new List<AsyncLogEventInfo>(cap)), (Action<IList<AsyncLogEventInfo>>) (l => l.Clear()))
{
}
