// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.TaskConstants`1
// Assembly: Nito.AsyncEx.Tasks, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A9818263-2EB2-45AD-B24E-66ECAA8EEAB5
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Tasks.dll

using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

public static class TaskConstants<T>
{
  private static readonly Task<T> defaultValue = Task.FromResult<T>(default (T));
  private static readonly Task<T> canceled = Task.FromCanceled<T>(new CancellationToken(true));

  public static Task<T> Default => TaskConstants<T>.defaultValue;

  public static Task<T> Canceled => TaskConstants<T>.canceled;
}
