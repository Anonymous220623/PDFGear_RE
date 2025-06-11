// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Watermark.UIElementAligent
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Enums;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.Watermark;

public partial class UIElementAligent : UserControl, IComponentConnector
{
  internal Grid GridWaterMarkLocation;
  internal RadioButton rbox0;
  internal RadioButton rbox1;
  internal RadioButton rbox2;
  internal RadioButton rbox3;
  internal RadioButton rbox4;
  internal RadioButton rbox5;
  internal RadioButton rbox6;
  internal RadioButton rbox7;
  internal RadioButton rbox8;
  private bool _contentLoaded;

  public PdfContentAlignment Alignment { get; private set; }

  public UIElementAligent() => this.InitializeComponent();

  private void rbox_Checked(object sender, RoutedEventArgs e)
  {
    string name = (sender as RadioButton).Name;
    if (name == null || name.Length != 5)
      return;
    switch (name[4])
    {
      case '0':
        if (!(name == "rbox0"))
          break;
        this.Alignment = PdfContentAlignment.TopLeft;
        break;
      case '1':
        if (!(name == "rbox1"))
          break;
        this.Alignment = PdfContentAlignment.TopCenter;
        break;
      case '2':
        if (!(name == "rbox2"))
          break;
        this.Alignment = PdfContentAlignment.TopRight;
        break;
      case '3':
        if (!(name == "rbox3"))
          break;
        this.Alignment = PdfContentAlignment.MiddleLeft;
        break;
      case '4':
        if (!(name == "rbox4"))
          break;
        this.Alignment = PdfContentAlignment.MiddleCenter;
        break;
      case '5':
        if (!(name == "rbox5"))
          break;
        this.Alignment = PdfContentAlignment.MiddleRight;
        break;
      case '6':
        if (!(name == "rbox6"))
          break;
        this.Alignment = PdfContentAlignment.BottomLeft;
        break;
      case '7':
        if (!(name == "rbox7"))
          break;
        this.Alignment = PdfContentAlignment.BottomCenter;
        break;
      case '8':
        if (!(name == "rbox8"))
          break;
        this.Alignment = PdfContentAlignment.BottomRight;
        break;
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/watermark/uielementaligent.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.GridWaterMarkLocation = (Grid) target;
        break;
      case 2:
        this.rbox0 = (RadioButton) target;
        this.rbox0.Checked += new RoutedEventHandler(this.rbox_Checked);
        break;
      case 3:
        this.rbox1 = (RadioButton) target;
        this.rbox1.Checked += new RoutedEventHandler(this.rbox_Checked);
        break;
      case 4:
        this.rbox2 = (RadioButton) target;
        this.rbox2.Checked += new RoutedEventHandler(this.rbox_Checked);
        break;
      case 5:
        this.rbox3 = (RadioButton) target;
        this.rbox3.Checked += new RoutedEventHandler(this.rbox_Checked);
        break;
      case 6:
        this.rbox4 = (RadioButton) target;
        this.rbox4.Checked += new RoutedEventHandler(this.rbox_Checked);
        break;
      case 7:
        this.rbox5 = (RadioButton) target;
        this.rbox5.Checked += new RoutedEventHandler(this.rbox_Checked);
        break;
      case 8:
        this.rbox6 = (RadioButton) target;
        this.rbox6.Checked += new RoutedEventHandler(this.rbox_Checked);
        break;
      case 9:
        this.rbox7 = (RadioButton) target;
        this.rbox7.Checked += new RoutedEventHandler(this.rbox_Checked);
        break;
      case 10:
        this.rbox8 = (RadioButton) target;
        this.rbox8.Checked += new RoutedEventHandler(this.rbox_Checked);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
