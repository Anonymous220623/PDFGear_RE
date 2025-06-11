// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.MarginChangedEventArgs
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows;

public class MarginChangedEventArgs : EventArgs
{
  public readonly Thickness Margin;

  public MarginChangedEventArgs(Thickness margin) => this.Margin = margin;
}
