// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.ChangeScope
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Controls;

#nullable disable
namespace HandyControl.Data;

internal class ChangeScope : DisposableObject
{
  private readonly GlowWindow _window;

  public ChangeScope(GlowWindow window)
  {
    this._window = window;
    ++this._window.DeferGlowChangesCount;
  }

  protected override void DisposeManagedResources()
  {
    --this._window.DeferGlowChangesCount;
    if (this._window.DeferGlowChangesCount != 0)
      return;
    this._window.EndDeferGlowChanges();
  }
}
