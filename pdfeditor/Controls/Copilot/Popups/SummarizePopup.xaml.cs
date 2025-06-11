// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Copilot.Popups.SummarizePopup
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Utils.Copilot;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Copilot.Popups;

public partial class SummarizePopup : PopupWindow, IComponentConnector
{
  private readonly CopilotHelper copilotHelper;
  private readonly string input;
  private CancellationTokenSource cts;
  internal TextBox SourceText;
  internal TextBlock Test;
  internal TextBox ResultText;
  private bool _contentLoaded;

  public SummarizePopup(CopilotHelper copilotHelper, string input)
  {
    this.copilotHelper = copilotHelper;
    this.input = input;
    this.cts = new CancellationTokenSource();
    this.InitializeComponent();
    this.SourceText.Text = input;
    this.Loaded += new RoutedEventHandler(this.SummarizePopup_Loaded);
  }

  private async void SummarizePopup_Loaded(object sender, RoutedEventArgs e)
  {
    this.Test.Text = "Loading...";
    try
    {
      StringBuilder sb = new StringBuilder();
      CopilotHelper.CopilotResult copilotResult = await this.copilotHelper.SummarizeAsync(this.input, (Func<string, CancellationToken, Task>) (async (s, ct) =>
      {
        sb.Append(s);
        await this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) (() => this.ResultText.Text = sb.ToString()));
      }), this.cts.Token);
    }
    catch (OperationCanceledException ex)
    {
    }
    catch (Exception ex)
    {
      CommomLib.Commom.Log.WriteLog(ex.ToString());
      this.ResultText.Text = "Error";
    }
    this.Test.Text = "Loaded";
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/copilot/popups/summarizepopup.xaml", UriKind.Relative));
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
    switch (connectionId)
    {
      case 1:
        this.SourceText = (TextBox) target;
        break;
      case 2:
        this.Test = (TextBlock) target;
        break;
      case 3:
        this.ResultText = (TextBox) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
