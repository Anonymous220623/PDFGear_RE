// Decompiled with JetBrains decompiler
// Type: PDFKit.ToolBars.PrintProgress
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace PDFKit.ToolBars;

internal class PrintProgress : Window, IComponentConnector
{
  internal TextBlock textBlock;
  internal Button button;
  private bool _contentLoaded;

  public event EventHandler NeedStopPrinting;

  public PrintProgress()
  {
    this.InitializeComponent();
    this.Closing += new CancelEventHandler(this.PrintProgress_Closing);
  }

  private void PrintProgress_Closing(object sender, CancelEventArgs e)
  {
    e.Cancel = true;
    this.Dispatcher.BeginInvoke((Delegate) new PrintProgress.DelegateType1(this.ShowMessageBoxOnClose));
  }

  private void ShowMessageBoxOnClose()
  {
    PrintProgress.DelegateType1 method = new PrintProgress.DelegateType1(this.SendCancelationEvent);
    if (MessageBox.Show(PDFKit.Properties.Resources.txtPromptPrintAbort, PDFKit.Properties.Resources.InfoHeader, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
      return;
    this.Dispatcher.BeginInvoke((Delegate) method);
  }

  private void SendCancelationEvent()
  {
    if (this.NeedStopPrinting == null)
      return;
    this.NeedStopPrinting((object) this, EventArgs.Empty);
  }

  internal void CloseWithoutPrompt()
  {
    this.Closing -= new CancelEventHandler(this.PrintProgress_Closing);
    this.Owner = (Window) null;
    this.Close();
  }

  internal void SetText(int pageNumber, int count)
  {
    this.textBlock.Text = string.Format(PDFKit.Properties.Resources.txtPrinting, (object) pageNumber, (object) count);
  }

  internal void SetText(string txt) => this.textBlock.Text = txt;

  private void button_Click(object sender, RoutedEventArgs e) => this.Close();

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "9.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/PDFKit;component/print/printprogress.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "9.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.textBlock = (TextBlock) target;
        break;
      case 2:
        this.button = (Button) target;
        this.button.Click += new RoutedEventHandler(this.button_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }

  private delegate void DelegateType1();
}
