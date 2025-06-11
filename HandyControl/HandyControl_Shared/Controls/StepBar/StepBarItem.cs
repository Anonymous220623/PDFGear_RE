// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.StepBarItem
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;

#nullable disable
namespace HandyControl.Controls;

public class StepBarItem : SelectableItem
{
  public static readonly DependencyProperty IndexProperty = DependencyProperty.Register(nameof (Index), typeof (int), typeof (StepBarItem), new PropertyMetadata((object) -1));
  public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(nameof (Status), typeof (StepStatus), typeof (StepBarItem), new PropertyMetadata((object) StepStatus.Waiting));

  public int Index
  {
    get => (int) this.GetValue(StepBarItem.IndexProperty);
    internal set => this.SetValue(StepBarItem.IndexProperty, (object) value);
  }

  public StepStatus Status
  {
    get => (StepStatus) this.GetValue(StepBarItem.StatusProperty);
    internal set => this.SetValue(StepBarItem.StatusProperty, (object) value);
  }
}
