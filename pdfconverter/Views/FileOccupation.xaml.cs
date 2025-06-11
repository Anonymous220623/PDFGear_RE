// Decompiled with JetBrains decompiler
// Type: pdfconverter.Views.FileOccupation
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using pdfconverter.Utils;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfconverter.Views;

public partial class FileOccupation : Window, IComponentConnector
{
  private string FileName = string.Empty;
  internal TextBlock textBlockmsg;
  internal TextBlock textBlockExplain;
  private bool _contentLoaded;

  public FileOccupation(string fileName)
  {
    this.InitializeComponent();
    this.FileName = fileName;
    this.textBlockExplain.Text = pdfconverter.Properties.Resources.WinFileOccupationExplain.Replace("xxx", Path.GetFileName(fileName));
  }

  private void Retry_Click(object sender, RoutedEventArgs e)
  {
    if (!ConToPDFUtils.CheckAccess(this.FileName).GetValueOrDefault())
      return;
    this.DialogResult = new bool?(true);
    this.Close();
  }

  private void Cancel_Click(object sender, RoutedEventArgs e)
  {
    this.DialogResult = new bool?(false);
    this.Close();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfconverter;component/views/fileoccupation.xaml", UriKind.Relative));
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
        this.textBlockExplain = (TextBlock) target;
      else
        this._contentLoaded = true;
    }
    else
      this.textBlockmsg = (TextBlock) target;
  }
}
