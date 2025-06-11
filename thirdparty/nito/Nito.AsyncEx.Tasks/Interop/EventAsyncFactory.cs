// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.Interop.EventAsyncFactory
// Assembly: Nito.AsyncEx.Tasks, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A9818263-2EB2-45AD-B24E-66ECAA8EEAB5
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Tasks.dll

using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx.Interop;

public static class EventAsyncFactory
{
  public static async Task<TEventArguments> FromAnyEvent<TDelegate, TEventArguments>(
    Func<Action<TEventArguments>, TDelegate> convert,
    Action<TDelegate> subscribe,
    Action<TDelegate> unsubscribe,
    CancellationToken cancellationToken,
    bool unsubscribeOnCapturedContext)
  {
    if (convert == null)
      throw new ArgumentNullException(nameof (convert));
    if (subscribe == null)
      throw new ArgumentNullException(nameof (subscribe));
    if (unsubscribe == null)
      throw new ArgumentNullException(nameof (unsubscribe));
    cancellationToken.ThrowIfCancellationRequested();
    TaskCompletionSource<TEventArguments> tcs = TaskCompletionSourceExtensions.CreateAsyncTaskSource<TEventArguments>();
    TDelegate subscription = convert((Action<TEventArguments>) (result => tcs.TrySetResult(result)));
    TEventArguments eventArguments;
    try
    {
      using (cancellationToken.Register((Action) (() => tcs.TrySetCanceled()), false))
      {
        subscribe(subscription);
        eventArguments = await tcs.Task.ConfigureAwait(unsubscribeOnCapturedContext);
      }
    }
    finally
    {
      unsubscribe(subscription);
    }
    subscription = default (TDelegate);
    return eventArguments;
  }

  public static Task<TEventArguments> FromAnyEvent<TDelegate, TEventArguments>(
    Func<Action<TEventArguments>, TDelegate> convert,
    Action<TDelegate> subscribe,
    Action<TDelegate> unsubscribe,
    CancellationToken cancellationToken)
  {
    return EventAsyncFactory.FromAnyEvent<TDelegate, TEventArguments>(convert, subscribe, unsubscribe, cancellationToken, true);
  }

  public static Task<TEventArguments> FromAnyEvent<TDelegate, TEventArguments>(
    Func<Action<TEventArguments>, TDelegate> convert,
    Action<TDelegate> subscribe,
    Action<TDelegate> unsubscribe)
  {
    return EventAsyncFactory.FromAnyEvent<TDelegate, TEventArguments>(convert, subscribe, unsubscribe, CancellationToken.None, true);
  }

  public static Task<EventArguments<object, EventArgs>> FromEvent(
    Action<EventHandler> subscribe,
    Action<EventHandler> unsubscribe,
    CancellationToken cancellationToken,
    bool unsubscribeOnCapturedContext)
  {
    return EventAsyncFactory.FromAnyEvent<EventHandler, EventArguments<object, EventArgs>>((Func<Action<EventArguments<object, EventArgs>>, EventHandler>) (x => (EventHandler) ((sender, args) => x(EventAsyncFactory.CreateEventArguments<object, EventArgs>(sender, args)))), subscribe, unsubscribe, cancellationToken, unsubscribeOnCapturedContext);
  }

  public static Task<EventArguments<object, EventArgs>> FromEvent(
    Action<EventHandler> subscribe,
    Action<EventHandler> unsubscribe,
    CancellationToken cancellationToken)
  {
    return EventAsyncFactory.FromEvent(subscribe, unsubscribe, cancellationToken, true);
  }

  public static Task<EventArguments<object, EventArgs>> FromEvent(
    Action<EventHandler> subscribe,
    Action<EventHandler> unsubscribe)
  {
    return EventAsyncFactory.FromEvent(subscribe, unsubscribe, CancellationToken.None, true);
  }

  public static Task<EventArguments<object, TEventArgs>> FromEvent<TEventArgs>(
    Action<EventHandler<TEventArgs>> subscribe,
    Action<EventHandler<TEventArgs>> unsubscribe,
    CancellationToken cancellationToken,
    bool unsubscribeOnCapturedContext)
  {
    return EventAsyncFactory.FromAnyEvent<EventHandler<TEventArgs>, EventArguments<object, TEventArgs>>((Func<Action<EventArguments<object, TEventArgs>>, EventHandler<TEventArgs>>) (x => (EventHandler<TEventArgs>) ((sender, args) => x(EventAsyncFactory.CreateEventArguments<object, TEventArgs>(sender, args)))), subscribe, unsubscribe, cancellationToken, unsubscribeOnCapturedContext);
  }

  public static Task<EventArguments<object, TEventArgs>> FromEvent<TEventArgs>(
    Action<EventHandler<TEventArgs>> subscribe,
    Action<EventHandler<TEventArgs>> unsubscribe,
    CancellationToken cancellationToken)
  {
    return EventAsyncFactory.FromEvent<TEventArgs>(subscribe, unsubscribe, cancellationToken, true);
  }

  public static Task<EventArguments<object, TEventArgs>> FromEvent<TEventArgs>(
    Action<EventHandler<TEventArgs>> subscribe,
    Action<EventHandler<TEventArgs>> unsubscribe)
  {
    return EventAsyncFactory.FromEvent<TEventArgs>(subscribe, unsubscribe, CancellationToken.None, true);
  }

  public static Task<EventArguments<object, TEventArgs>> FromEvent<TDelegate, TEventArgs>(
    Func<EventHandler<TEventArgs>, TDelegate> convert,
    Action<TDelegate> subscribe,
    Action<TDelegate> unsubscribe,
    CancellationToken cancellationToken,
    bool unsubscribeOnCapturedContext)
  {
    return EventAsyncFactory.FromAnyEvent<TDelegate, EventArguments<object, TEventArgs>>((Func<Action<EventArguments<object, TEventArgs>>, TDelegate>) (x => convert((EventHandler<TEventArgs>) ((sender, args) => x(EventAsyncFactory.CreateEventArguments<object, TEventArgs>(sender, args))))), subscribe, unsubscribe, cancellationToken, unsubscribeOnCapturedContext);
  }

  public static Task<EventArguments<object, TEventArgs>> FromEvent<TDelegate, TEventArgs>(
    Func<EventHandler<TEventArgs>, TDelegate> convert,
    Action<TDelegate> subscribe,
    Action<TDelegate> unsubscribe,
    CancellationToken cancellationToken)
  {
    return EventAsyncFactory.FromEvent<TDelegate, TEventArgs>(convert, subscribe, unsubscribe, cancellationToken, true);
  }

  public static Task<EventArguments<object, TEventArgs>> FromEvent<TDelegate, TEventArgs>(
    Func<EventHandler<TEventArgs>, TDelegate> convert,
    Action<TDelegate> subscribe,
    Action<TDelegate> unsubscribe)
  {
    return EventAsyncFactory.FromEvent<TDelegate, TEventArgs>(convert, subscribe, unsubscribe, CancellationToken.None, true);
  }

  public static Task<EventArguments<TSender, TEventArgs>> FromActionEvent<TSender, TEventArgs>(
    Action<Action<TSender, TEventArgs>> subscribe,
    Action<Action<TSender, TEventArgs>> unsubscribe,
    CancellationToken cancellationToken,
    bool unsubscribeOnCapturedContext)
  {
    return EventAsyncFactory.FromAnyEvent<Action<TSender, TEventArgs>, EventArguments<TSender, TEventArgs>>((Func<Action<EventArguments<TSender, TEventArgs>>, Action<TSender, TEventArgs>>) (x => (Action<TSender, TEventArgs>) ((sender, args) => x(EventAsyncFactory.CreateEventArguments<TSender, TEventArgs>(sender, args)))), subscribe, unsubscribe, cancellationToken, unsubscribeOnCapturedContext);
  }

  public static Task<EventArguments<TSender, TEventArgs>> FromActionEvent<TSender, TEventArgs>(
    Action<Action<TSender, TEventArgs>> subscribe,
    Action<Action<TSender, TEventArgs>> unsubscribe,
    CancellationToken cancellationToken)
  {
    return EventAsyncFactory.FromActionEvent<TSender, TEventArgs>(subscribe, unsubscribe, cancellationToken, true);
  }

  public static Task<EventArguments<TSender, TEventArgs>> FromActionEvent<TSender, TEventArgs>(
    Action<Action<TSender, TEventArgs>> subscribe,
    Action<Action<TSender, TEventArgs>> unsubscribe)
  {
    return EventAsyncFactory.FromActionEvent<TSender, TEventArgs>(subscribe, unsubscribe, CancellationToken.None, true);
  }

  public static Task<TEventArgs> FromActionEvent<TEventArgs>(
    Action<Action<TEventArgs>> subscribe,
    Action<Action<TEventArgs>> unsubscribe,
    CancellationToken cancellationToken,
    bool unsubscribeOnCapturedContext)
  {
    return EventAsyncFactory.FromAnyEvent<Action<TEventArgs>, TEventArgs>((Func<Action<TEventArgs>, Action<TEventArgs>>) (x => x), subscribe, unsubscribe, cancellationToken, unsubscribeOnCapturedContext);
  }

  public static Task<TEventArgs> FromActionEvent<TEventArgs>(
    Action<Action<TEventArgs>> subscribe,
    Action<Action<TEventArgs>> unsubscribe,
    CancellationToken cancellationToken)
  {
    return EventAsyncFactory.FromActionEvent<TEventArgs>(subscribe, unsubscribe, cancellationToken, true);
  }

  public static Task<TEventArgs> FromActionEvent<TEventArgs>(
    Action<Action<TEventArgs>> subscribe,
    Action<Action<TEventArgs>> unsubscribe)
  {
    return EventAsyncFactory.FromActionEvent<TEventArgs>(subscribe, unsubscribe, CancellationToken.None, true);
  }

  public static Task FromActionEvent(
    Action<Action> subscribe,
    Action<Action> unsubscribe,
    CancellationToken cancellationToken,
    bool unsubscribeOnCapturedContext)
  {
    return (Task) EventAsyncFactory.FromAnyEvent<Action, object>((Func<Action<object>, Action>) (x => (Action) (() => x((object) null))), subscribe, unsubscribe, cancellationToken, unsubscribeOnCapturedContext);
  }

  public static Task FromActionEvent(
    Action<Action> subscribe,
    Action<Action> unsubscribe,
    CancellationToken cancellationToken)
  {
    return EventAsyncFactory.FromActionEvent(subscribe, unsubscribe, cancellationToken, true);
  }

  public static Task FromActionEvent(Action<Action> subscribe, Action<Action> unsubscribe)
  {
    return EventAsyncFactory.FromActionEvent(subscribe, unsubscribe, CancellationToken.None, true);
  }

  private static EventArguments<TSender, TEventArgs> CreateEventArguments<TSender, TEventArgs>(
    TSender sender,
    TEventArgs eventArgs)
  {
    return new EventArguments<TSender, TEventArgs>()
    {
      Sender = sender,
      EventArgs = eventArgs
    };
  }
}
