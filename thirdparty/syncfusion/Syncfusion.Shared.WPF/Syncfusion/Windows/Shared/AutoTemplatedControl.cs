// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.AutoTemplatedControl
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class AutoTemplatedControl : Control
{
  public AutoTemplatedControl(Type keyType)
  {
    if (!(keyType != (Type) null))
      throw new ArgumentNullException(nameof (keyType));
    this.SetValue(FrameworkElement.DefaultStyleKeyProperty, (object) keyType);
  }
}
