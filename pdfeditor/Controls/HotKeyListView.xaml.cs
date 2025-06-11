// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.HotKeyListView
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom.HotKeys;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls;

public partial class HotKeyListView : UserControl, IComponentConnector
{
  internal ItemsControl itemsControl;
  private bool _contentLoaded;

  public HotKeyListView()
  {
    this.InitializeComponent();
    this.itemsControl.ItemsSource = (IEnumerable) HotKeyManager.Names.Select<string, HotKeyModel>((Func<string, HotKeyModel>) (c => HotKeyManager.GetOrCreate(c))).Where<HotKeyModel>((Func<HotKeyModel, bool>) (c => c.IsVisible)).ToArray<HotKeyModel>();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/hotkeylistview.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId == 1)
      this.itemsControl = (ItemsControl) target;
    else
      this._contentLoaded = true;
  }
}
