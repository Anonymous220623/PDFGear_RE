// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DatesCollection
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class DatesCollection : ObservableCollection<DateTime>, IDisposable
{
  private bool m_allowInsert = true;

  protected internal bool AllowInsert
  {
    get => this.m_allowInsert;
    set => this.m_allowInsert = value;
  }

  public void Dispose()
  {
    this.m_allowInsert = true;
    this.ClearItems();
    this.Clear();
  }

  protected override void InsertItem(int index, DateTime item)
  {
    if (!this.AllowInsert)
      throw new NotSupportedException("Cannot add new Date to collection because AllowSellection is false");
    if (this.Contains(item))
      return;
    base.InsertItem(index, item);
  }
}
