// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.WiaItemBase
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using NAPS2.Wia.Native;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace NAPS2.Wia;

public class WiaItemBase : NativeWiaObject
{
  private WiaPropertyCollection? _properties;

  protected internal WiaItemBase(WiaVersion version, IntPtr handle)
    : base(version, handle)
  {
  }

  public WiaPropertyCollection Properties
  {
    get
    {
      if (this._properties == null)
        this._properties = new WiaPropertyCollection(this.Version, this.Handle);
      return this._properties;
    }
  }

  public List<WiaItem> GetSubItems()
  {
    List<WiaItem> items = new List<WiaItem>();
    WiaException.Check(this.Version == WiaVersion.Wia10 ? NativeWiaMethods.EnumerateItems1(this.Handle, (Action<IntPtr>) (itemHandle => items.Add(new WiaItem(this.Version, itemHandle)))) : NativeWiaMethods.EnumerateItems2(this.Handle, (Action<IntPtr>) (itemHandle => items.Add(new WiaItem(this.Version, itemHandle)))));
    return items;
  }

  public WiaItem? FindSubItem(string name)
  {
    return this.GetSubItems().FirstOrDefault<WiaItem>((Func<WiaItem, bool>) (x => x.Name() == name));
  }

  protected override void Dispose(bool disposing)
  {
    base.Dispose(disposing);
    if (!disposing)
      return;
    this._properties?.Dispose();
  }
}
