// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.PauseTokenSource
// Assembly: Nito.AsyncEx.Coordination, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5843C369-F4A6-4604-9695-EBAC5AF4AB11
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Coordination.dll

#nullable enable
namespace Nito.AsyncEx;

public sealed class PauseTokenSource
{
  private readonly AsyncManualResetEvent _mre = new AsyncManualResetEvent(true);

  public bool IsPaused
  {
    get => !this._mre.IsSet;
    set
    {
      if (value)
        this._mre.Reset();
      else
        this._mre.Set();
    }
  }

  public PauseToken Token => new PauseToken(this._mre);
}
