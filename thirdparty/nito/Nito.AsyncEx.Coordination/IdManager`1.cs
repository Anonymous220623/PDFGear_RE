// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.IdManager`1
// Assembly: Nito.AsyncEx.Coordination, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5843C369-F4A6-4604-9695-EBAC5AF4AB11
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Coordination.dll

using System.Threading;

#nullable enable
namespace Nito.AsyncEx;

internal static class IdManager<TTag>
{
  private static int _lastId;

  public static int GetId(ref int id)
  {
    if (id != 0)
      return id;
    int num;
    do
    {
      num = Interlocked.Increment(ref IdManager<TTag>._lastId);
    }
    while (num == 0);
    Interlocked.CompareExchange(ref id, num, 0);
    return id;
  }
}
