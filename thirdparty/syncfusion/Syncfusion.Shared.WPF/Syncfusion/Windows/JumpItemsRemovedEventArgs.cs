// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.JumpItemsRemovedEventArgs
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Windows;

public sealed class JumpItemsRemovedEventArgs : EventArgs
{
  public JumpItemsRemovedEventArgs()
    : this((IList<JumpItem>) null)
  {
  }

  public JumpItemsRemovedEventArgs(IList<JumpItem> removedItems)
  {
    if (removedItems != null)
      this.RemovedItems = (IList<JumpItem>) new List<JumpItem>((IEnumerable<JumpItem>) removedItems).AsReadOnly();
    else
      this.RemovedItems = (IList<JumpItem>) new List<JumpItem>().AsReadOnly();
  }

  public IList<JumpItem> RemovedItems { get; private set; }
}
