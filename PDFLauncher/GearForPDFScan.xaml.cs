// Decompiled with JetBrains decompiler
// Type: PDFLauncher.GearForPDFScan
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using CommomLib.Commom;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

#nullable disable
namespace PDFLauncher;

public partial class GearForPDFScan : Window, IComponentConnector
{
  private bool _contentLoaded;

  public GearForPDFScan()
  {
    this.InitializeComponent();
    GAManager.SendEvent("Ads", "ScanPDF", "Show", 1L);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/PDFLauncher;component/gearforpdfscan.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;
}
