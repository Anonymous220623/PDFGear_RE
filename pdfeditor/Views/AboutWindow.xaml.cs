// Decompiled with JetBrains decompiler
// Type: pdfeditor.Views.AboutWindow
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using CommomLib.Config;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Views;

public partial class AboutWindow : Window, IComponentConnector
{
  private DebugHelper pdfopen;
  private DebugHelper pdfconf;
  private DebugHelper pdfdebug;
  internal TextBlock app_ver;
  internal Hyperlink privacypolicyLnk;
  internal TextBlock copyright;
  internal Button okBtn;
  private bool _contentLoaded;

  public AboutWindow()
  {
    this.InitializeComponent();
    this.pdfopen = new DebugHelper(nameof (pdfopen));
    this.pdfconf = new DebugHelper(nameof (pdfconf));
    this.pdfdebug = new DebugHelper(nameof (pdfdebug));
    this.pdfopen.Processed += new EventHandler(this.Pdfopen_Processed);
    this.pdfconf.Processed += new EventHandler(this.Pdfconf_Processed);
    this.pdfdebug.Processed += new EventHandler(this.Pdfdebug_Processed);
  }

  private void Window_Loaded(object sender, RoutedEventArgs e)
  {
    string str1 = "Version: " + UtilManager.GetAppVersion();
    string str2 = $"Copyright © 2024 {UtilManager.GetProductName()}. All rights reserved.";
    this.app_ver.Text = str1;
    this.copyright.Text = str2;
  }

  private void PrivacypolicyLnk_Click(object sender, RoutedEventArgs e)
  {
    Process.Start(new ProcessStartInfo((sender as Hyperlink).NavigateUri.AbsoluteUri));
  }

  private void okBtn_Click(object sender, RoutedEventArgs e) => this.Close();

  private void Pdfopen_Processed(object sender, EventArgs e)
  {
    string folderPath = AppDataHelper.LocalFolder;
    try
    {
      folderPath = Directory.GetParent(AppDataHelper.LocalFolder).FullName;
    }
    catch
    {
    }
    ExplorerUtils.OpenFolderAsync(folderPath, new CancellationToken());
  }

  private void Pdfconf_Processed(object sender, EventArgs e)
  {
    try
    {
      string[] array = SqliteUtils.GetAllKeysAsync(new CancellationToken()).GetAwaiter().GetResult().ToArray<string>();
      if (array == null || array.Length == 0)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string key in array)
      {
        string str;
        if (SqliteUtils.TryGet(key, out str))
          stringBuilder.Append("Key: ").AppendLine(key).AppendLine("Value: ").AppendLine(str).AppendLine();
      }
      string str1 = Path.Combine(AppDataHelper.TemporaryFolder, "pdfconf");
      if (!Directory.Exists(str1))
      {
        Directory.CreateDirectory(str1);
      }
      else
      {
        try
        {
          foreach (string file in Directory.GetFiles(str1))
          {
            try
            {
              File.Delete(file);
            }
            catch
            {
            }
          }
        }
        catch
        {
        }
      }
      string str2 = Path.Combine(str1, $"{Guid.NewGuid():N}.log");
      File.WriteAllText(str2, stringBuilder.ToString());
      Process.Start(str2);
    }
    catch
    {
    }
  }

  private void Pdfdebug_Processed(object sender, EventArgs e)
  {
    bool debugMode = ConfigManager.GetDebugMode();
    ConfigManager.SetDebugMode(!debugMode);
    int num = (int) ModernMessageBox.Show($"Debug Mode: {!debugMode}", "PDFgear");
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    this.pdfopen.ProcessKeyDown(e.Key);
    this.pdfconf.ProcessKeyDown(e.Key);
    this.pdfdebug.ProcessKeyDown(e.Key);
    base.OnPreviewKeyDown(e);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/views/aboutwindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
        break;
      case 2:
        this.app_ver = (TextBlock) target;
        break;
      case 3:
        this.privacypolicyLnk = (Hyperlink) target;
        this.privacypolicyLnk.Click += new RoutedEventHandler(this.PrivacypolicyLnk_Click);
        break;
      case 4:
        this.copyright = (TextBlock) target;
        break;
      case 5:
        this.okBtn = (Button) target;
        this.okBtn.Click += new RoutedEventHandler(this.okBtn_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
