// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.TransferItem
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class TransferItem : ListBoxItem
{
  public static readonly DependencyProperty IsTransferredProperty = DependencyProperty.Register(nameof (IsTransferred), typeof (bool), typeof (TransferItem), new PropertyMetadata(ValueBoxes.FalseBox));

  public bool IsTransferred
  {
    get => (bool) this.GetValue(TransferItem.IsTransferredProperty);
    set => this.SetValue(TransferItem.IsTransferredProperty, ValueBoxes.BooleanBox(value));
  }
}
