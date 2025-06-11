// Decompiled with JetBrains decompiler
// Type: NLog.Common.LogEventInfoBuffer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using System;

#nullable disable
namespace NLog.Common;

public class LogEventInfoBuffer
{
  private readonly object _lockObject = new object();
  private readonly bool _growAsNeeded;
  private readonly int _growLimit;
  private AsyncLogEventInfo[] _buffer;
  private int _getPointer;
  private int _putPointer;
  private int _count;

  public LogEventInfoBuffer(int size, bool growAsNeeded, int growLimit)
  {
    this._growAsNeeded = growAsNeeded;
    this._buffer = new AsyncLogEventInfo[size];
    this._growLimit = growLimit;
    this._getPointer = 0;
    this._putPointer = 0;
  }

  public int Size => this._buffer.Length;

  internal int Count
  {
    get
    {
      lock (this._lockObject)
        return this._count;
    }
  }

  public int Append(AsyncLogEventInfo eventInfo)
  {
    lock (this._lockObject)
    {
      if (this._count >= this._buffer.Length)
      {
        if (this._growAsNeeded && this._buffer.Length < this._growLimit)
        {
          int length = this._buffer.Length * 2;
          if (length >= this._growLimit)
            length = this._growLimit;
          InternalLogger.Trace<int, int>("Enlarging LogEventInfoBuffer from {0} to {1}", this._buffer.Length, length);
          AsyncLogEventInfo[] destinationArray = new AsyncLogEventInfo[length];
          Array.Copy((Array) this._buffer, 0, (Array) destinationArray, 0, this._buffer.Length);
          this._buffer = destinationArray;
        }
        else
          ++this._getPointer;
      }
      this._putPointer %= this._buffer.Length;
      this._buffer[this._putPointer] = eventInfo;
      ++this._putPointer;
      ++this._count;
      if (this._count >= this._buffer.Length)
        this._count = this._buffer.Length;
      return this._count;
    }
  }

  public AsyncLogEventInfo[] GetEventsAndClear()
  {
    lock (this._lockObject)
    {
      int count = this._count;
      if (count == 0)
        return ArrayHelper.Empty<AsyncLogEventInfo>();
      AsyncLogEventInfo[] eventsAndClear = new AsyncLogEventInfo[count];
      for (int index1 = 0; index1 < count; ++index1)
      {
        int index2 = (this._getPointer + index1) % this._buffer.Length;
        AsyncLogEventInfo asyncLogEventInfo = this._buffer[index2];
        this._buffer[index2] = new AsyncLogEventInfo();
        eventsAndClear[index1] = asyncLogEventInfo;
      }
      this._count = 0;
      this._getPointer = 0;
      this._putPointer = 0;
      return eventsAndClear;
    }
  }
}
