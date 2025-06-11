// Decompiled with JetBrains decompiler
// Type: Nito.NormalizedCancellationToken
// Assembly: Nito.Cancellation, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8A0A7EC6-B362-493B-B8C5-5760E5B4B706
// Assembly location: D:\PDFGear\bin\Nito.Cancellation.dll

using Nito.Disposables;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable enable
namespace Nito;

public sealed class NormalizedCancellationToken : SingleDisposable<object>
{
  private readonly CancellationTokenSource? _cts;
  private readonly CancellationToken _token;

  public NormalizedCancellationToken()
    : this((CancellationTokenSource) null)
  {
  }

  public NormalizedCancellationToken(CancellationTokenSource? cts)
    : base(new object())
  {
    this._cts = cts;
    if (cts == null)
      return;
    this._token = cts.Token;
  }

  public NormalizedCancellationToken(CancellationToken token)
    : base((object) null)
  {
    this._token = token;
  }

  protected override void Dispose(object context) => this._cts?.Dispose();

  public CancellationToken Token => this._token;

  public static NormalizedCancellationToken Timeout(TimeSpan dueTime)
  {
    CancellationTokenSource cts = new CancellationTokenSource();
    cts.CancelAfter(dueTime);
    return new NormalizedCancellationToken(cts);
  }

  public static NormalizedCancellationToken Timeout(int dueTime)
  {
    CancellationTokenSource cts = new CancellationTokenSource();
    cts.CancelAfter(dueTime);
    return new NormalizedCancellationToken(cts);
  }

  public static NormalizedCancellationToken Normalize(params CancellationToken[] cancellationTokens)
  {
    return cancellationTokens != null ? NormalizedCancellationToken.Normalize((IEnumerable<CancellationToken>) cancellationTokens) : throw new ArgumentNullException(nameof (cancellationTokens));
  }

  public static NormalizedCancellationToken Normalize(
    IEnumerable<CancellationToken> cancellationTokens)
  {
    List<CancellationToken> tokens = cancellationTokens != null ? new List<CancellationToken>(NormalizedCancellationToken.CancelableTokens(cancellationTokens)) : throw new ArgumentNullException(nameof (cancellationTokens));
    if (tokens.Count == 0)
      return new NormalizedCancellationToken();
    if (tokens.Count == 1)
      return new NormalizedCancellationToken(tokens[0]);
    CancellationToken canceledToken = NormalizedCancellationToken.FindCanceledToken((IEnumerable<CancellationToken>) tokens);
    if (canceledToken.IsCancellationRequested)
      return new NormalizedCancellationToken(canceledToken);
    CancellationToken[] array = new CancellationToken[tokens.Count];
    tokens.CopyTo(array, 0);
    return new NormalizedCancellationToken(CancellationTokenSource.CreateLinkedTokenSource(array));
  }

  private static IEnumerable<CancellationToken> CancelableTokens(
    IEnumerable<CancellationToken> tokens)
  {
    foreach (CancellationToken token in tokens)
    {
      if (token.CanBeCanceled)
        yield return token;
    }
  }

  private static CancellationToken FindCanceledToken(IEnumerable<CancellationToken> tokens)
  {
    foreach (CancellationToken token in tokens)
    {
      if (token.IsCancellationRequested)
        return token;
    }
    return CancellationToken.None;
  }
}
