// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.IAsyncWaitQueue`1
// Assembly: Nito.AsyncEx.Coordination, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5843C369-F4A6-4604-9695-EBAC5AF4AB11
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Coordination.dll

using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

internal interface IAsyncWaitQueue<T>
{
  bool IsEmpty { get; }

  Task<T> Enqueue();

  void Dequeue(T result = null);

  void DequeueAll(T result = null);

  bool TryCancel(Task task, CancellationToken cancellationToken);

  void CancelAll(CancellationToken cancellationToken);
}
