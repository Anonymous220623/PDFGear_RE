// Decompiled with JetBrains decompiler
// Type: NLog.Internal.AppendBuilderCreator
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Text;

#nullable disable
namespace NLog.Internal;

internal struct AppendBuilderCreator : IDisposable
{
  private static readonly StringBuilderPool _builderPool = new StringBuilderPool(Environment.ProcessorCount * 2);
  private readonly StringBuilder _appendTarget;
  private StringBuilderPool.ItemHolder _builder;

  public StringBuilder Builder => this._builder.Item;

  public AppendBuilderCreator(StringBuilder appendTarget, bool mustBeEmpty)
  {
    this._appendTarget = appendTarget;
    if (this._appendTarget.Length > 0 & mustBeEmpty)
      this._builder = AppendBuilderCreator._builderPool.Acquire();
    else
      this._builder = new StringBuilderPool.ItemHolder(this._appendTarget, (StringBuilderPool) null, 0);
  }

  public void Dispose()
  {
    if (this._builder.Item == this._appendTarget)
      return;
    this._builder.Item.CopyTo(this._appendTarget);
    this._builder.Dispose();
  }
}
