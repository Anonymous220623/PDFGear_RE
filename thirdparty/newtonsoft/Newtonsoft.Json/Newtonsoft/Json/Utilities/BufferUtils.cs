// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.BufferUtils
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

#nullable enable
namespace Newtonsoft.Json.Utilities;

internal static class BufferUtils
{
  public static char[] RentBuffer(IArrayPool<char>? bufferPool, int minSize)
  {
    return bufferPool == null ? new char[minSize] : bufferPool.Rent(minSize);
  }

  public static void ReturnBuffer(IArrayPool<char>? bufferPool, char[]? buffer)
  {
    bufferPool?.Return(buffer);
  }

  public static char[] EnsureBufferSize(IArrayPool<char>? bufferPool, int size, char[]? buffer)
  {
    if (bufferPool == null)
      return new char[size];
    if (buffer != null)
      bufferPool.Return(buffer);
    return bufferPool.Rent(size);
  }
}
