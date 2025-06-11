// Decompiled with JetBrains decompiler
// Type: PDFLauncher.Discardconfirm
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using PDFLauncher.CustomControl;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

#nullable disable
namespace PDFLauncher;

public partial class Discardconfirm : Window, IComponentConnector
{
  internal ButtonEx DicardBtn;
  internal ButtonEx CancelBtn;
  private bool _contentLoaded;

  public Discardconfirm() => this.InitializeComponent();

  private void DicardBtn_Click(object sender, RoutedEventArgs e)
  {
    this.DialogResult = new bool?(true);
  }

  private void CancelBtn_Click(object sender, RoutedEventArgs e)
  {
    this.DialogResult = new bool?(false);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/PDFLauncher;component/discardconfirm.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 1)
    {
      if (connectionId == 2)
        this.CancelBtn = (ButtonEx) target;
      else
        this._contentLoaded = true;
    }
    else
      this.DicardBtn = (ButtonEx) target;
  }
}
