// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.ProgressBar
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System.Windows;

#nullable disable
namespace CommomLib.Controls;

public partial class ProgressBar : System.Windows.Controls.ProgressBar
{
  static ProgressBar()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ProgressBar), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ProgressBar)));
  }
}
