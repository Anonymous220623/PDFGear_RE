// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.PauseToken
// Assembly: Nito.AsyncEx.Coordination, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5843C369-F4A6-4604-9695-EBAC5AF4AB11
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Coordination.dll

using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

public struct PauseToken
{
  private readonly AsyncManualResetEvent _mre;

  internal PauseToken(AsyncManualResetEvent mre) => this._mre = mre;

  public bool CanBePaused => this._mre != null;

  public bool IsPaused => this._mre != null && !this._mre.IsSet;

  public Task WaitWhilePausedAsync()
  {
    return this._mre == null ? TaskConstants.Completed : this._mre.WaitAsync();
  }

  public Task WaitWhilePausedAsync(CancellationToken token)
  {
    return this._mre == null ? TaskConstants.Completed : this._mre.WaitAsync(token);
  }

  public void WaitWhilePaused()
  {
    if (this._mre == null)
      return;
    this._mre.Wait();
  }

  public void WaitWhilePaused(CancellationToken token)
  {
    if (this._mre == null)
      return;
    this._mre.Wait(token);
  }
}
