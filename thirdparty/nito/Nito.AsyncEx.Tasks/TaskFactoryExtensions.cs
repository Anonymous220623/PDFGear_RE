// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.TaskFactoryExtensions
// Assembly: Nito.AsyncEx.Tasks, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A9818263-2EB2-45AD-B24E-66ECAA8EEAB5
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Tasks.dll

using System;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

public static class TaskFactoryExtensions
{
  public static Task Run(this TaskFactory @this, Action action)
  {
    if (@this == null)
      throw new ArgumentNullException(nameof (@this));
    if (action == null)
      throw new ArgumentNullException(nameof (action));
    return @this.StartNew(action, @this.CancellationToken, @this.CreationOptions | TaskCreationOptions.DenyChildAttach, @this.Scheduler ?? TaskScheduler.Default);
  }

  public static Task<TResult> Run<TResult>(this TaskFactory @this, Func<TResult> action)
  {
    if (@this == null)
      throw new ArgumentNullException(nameof (@this));
    if (action == null)
      throw new ArgumentNullException(nameof (action));
    return @this.StartNew<TResult>(action, @this.CancellationToken, @this.CreationOptions | TaskCreationOptions.DenyChildAttach, @this.Scheduler ?? TaskScheduler.Default);
  }

  public static Task Run(this TaskFactory @this, Func<Task> action)
  {
    if (@this == null)
      throw new ArgumentNullException(nameof (@this));
    if (action == null)
      throw new ArgumentNullException(nameof (action));
    return @this.StartNew<Task>(action, @this.CancellationToken, @this.CreationOptions | TaskCreationOptions.DenyChildAttach, @this.Scheduler ?? TaskScheduler.Default).Unwrap();
  }

  public static Task<TResult> Run<TResult>(this TaskFactory @this, Func<Task<TResult>> action)
  {
    if (@this == null)
      throw new ArgumentNullException(nameof (@this));
    if (action == null)
      throw new ArgumentNullException(nameof (action));
    return @this.StartNew<Task<TResult>>(action, @this.CancellationToken, @this.CreationOptions | TaskCreationOptions.DenyChildAttach, @this.Scheduler ?? TaskScheduler.Default).Unwrap<TResult>();
  }
}
