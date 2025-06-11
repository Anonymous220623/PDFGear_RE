// Decompiled with JetBrains decompiler
// Type: pdfeditor.Views.RateWindow
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using CommomLib.Views;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Views;

public partial class RateWindow : Window, IComponentConnector
{
  private bool _contentLoaded;

  public RateWindow()
  {
    this.InitializeComponent();
    GAManager.SendEvent(nameof (RateWindow), "Show", "Count", 1L);
  }

  private void RateButton_Click(object sender, RoutedEventArgs e)
  {
    string str1 = CultureInfoUtils.ActualAppLanguage;
    string path1 = "https://www.pdfgear.com/";
    string str2 = "review-us";
    if (str1.ToLower() == "ko")
      str1 = "kr";
    if (str1.ToLower() == "zh-cn")
      str1 = "zh";
    if (str1.ToLower() == "ja")
      str1 = "jp";
    string gearRate = Path.Combine(path1, str2).Replace("\\", "/");
    if (str1 != "en")
      gearRate = Path.Combine(path1, str1.ToLower(), str2).Replace("\\", "/");
    object locker = new object();
    bool result = false;
    new Thread((ThreadStart) (() =>
    {
      try
      {
        Process.Start(gearRate);
        result = true;
      }
      catch
      {
        result = false;
      }
      finally
      {
        lock (locker)
          Monitor.PulseAll(locker);
      }
    }))
    {
      IsBackground = true
    }.Start();
    lock (locker)
    {
      Monitor.Wait(locker, 5000);
      int num = result ? 1 : 0;
      GAManager.SendEvent(nameof (RateWindow), "BlockExit", "Count", 1L);
    }
    this.Close();
  }

  public bool GenerateBoolRandom()
  {
    return new bool[2]{ true, false }[new Random().Next(2)];
  }

  private void FeedBackButton_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent(nameof (RateWindow), "FeedbackBtn", "Count", 1L);
    FeedbackWindow feedbackWindow = new FeedbackWindow();
    feedbackWindow.HideFaq();
    feedbackWindow.Owner = (Window) this;
    feedbackWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    feedbackWindow.ShowDialog();
    this.Close();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/views/ratewindow.xaml", UriKind.Relative));
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
        ((ButtonBase) target).Click += new RoutedEventHandler(this.FeedBackButton_Click);
      else
        this._contentLoaded = true;
    }
    else
      ((ButtonBase) target).Click += new RoutedEventHandler(this.RateButton_Click);
  }
}
