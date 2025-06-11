// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ArrayHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

#nullable disable
namespace NLog.Internal;

internal static class ArrayHelper
{
  internal static T[] Empty<T>() => ArrayHelper.EmptyArray<T>.Instance;

  private static class EmptyArray<T>
  {
    internal static readonly T[] Instance = new T[0];
  }
}
