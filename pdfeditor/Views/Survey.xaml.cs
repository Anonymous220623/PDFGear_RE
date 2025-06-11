// Decompiled with JetBrains decompiler
// Type: pdfeditor.Views.Survey
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
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

public partial class Survey : Window, IComponentConnector
{
  private bool _contentLoaded;

  public Survey()
  {
    this.InitializeComponent();
    GAManager.SendEvent("SurveyWindow", "Show", "Count", 1L);
  }

  private void Button_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent("SurveyWindow", "SurveyBtn", "Count", 1L);
    string str1 = CultureInfoUtils.ActualAppLanguage;
    string path1 = "https://www.pdfgear.com/";
    string str2 = "share";
    if (str1.ToLower() == "ko")
      str1 = "kr";
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
    }
    this.Close();
  }

  private void Button_Click_1(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent("SurveyWindow", "CloseBtn", "Count", 1L);
    this.Close();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/views/survey.xaml", UriKind.Relative));
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
        ((ButtonBase) target).Click += new RoutedEventHandler(this.Button_Click);
      else
        this._contentLoaded = true;
    }
    else
      ((ButtonBase) target).Click += new RoutedEventHandler(this.Button_Click_1);
  }
}
