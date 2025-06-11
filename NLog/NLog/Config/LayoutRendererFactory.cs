// Decompiled with JetBrains decompiler
// Type: NLog.Config.LayoutRendererFactory
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;

#nullable disable
namespace NLog.Config;

internal class LayoutRendererFactory(ConfigurationItemFactory parentFactory) : 
  Factory<LayoutRenderer, LayoutRendererAttribute>(parentFactory)
{
  private Dictionary<string, FuncLayoutRenderer> _funcRenderers;

  public void ClearFuncLayouts()
  {
    this._funcRenderers = (Dictionary<string, FuncLayoutRenderer>) null;
  }

  public void RegisterFuncLayout(string name, FuncLayoutRenderer renderer)
  {
    this._funcRenderers = this._funcRenderers ?? new Dictionary<string, FuncLayoutRenderer>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    this._funcRenderers[name] = renderer;
  }

  public override bool TryCreateInstance(string itemName, out LayoutRenderer result)
  {
    FuncLayoutRenderer funcLayoutRenderer;
    if (this._funcRenderers == null || !this._funcRenderers.TryGetValue(itemName, out funcLayoutRenderer))
      return base.TryCreateInstance(itemName, out result);
    result = (LayoutRenderer) funcLayoutRenderer;
    return true;
  }
}
