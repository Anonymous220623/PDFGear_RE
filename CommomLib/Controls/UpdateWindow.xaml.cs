// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.UpdateWindow
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace CommomLib.Controls;

public partial class UpdateWindow : Window, IComponentConnector
{
  private bool downloading;
  private Version newVer;
  private CancellationTokenSource cts = new CancellationTokenSource();
  internal Grid Downloading;
  internal TextBlock DownloadFileSizeText;
  internal TextBlock Downloaded;
  internal ProgressBar DownloadProgressBar;
  internal TextBlock Downladed1;
  internal Button btnOk;
  private bool _contentLoaded;

  public UpdateWindow(string downloadUrl, string validationMD5, Version newVer)
  {
    if (string.IsNullOrEmpty(downloadUrl))
      throw new ArgumentException(nameof (downloadUrl));
    if (string.IsNullOrEmpty(validationMD5))
      throw new ArgumentException(nameof (validationMD5));
    this.InitializeComponent();
    this.DownloadFileSizeText.Text = "";
    this.DownloadUrl = downloadUrl;
    this.ValidationMD5 = validationMD5;
    this.UpdateResult = new CommomLib.Commom.UpdateResult();
    this.newVer = newVer;
    this.Install = false;
    this.Loaded += new RoutedEventHandler(this.UpdateWindow_Loaded);
  }

  public CommomLib.Commom.UpdateResult UpdateResult { get; private set; }

  public string DownloadUrl { get; }

  public string ValidationMD5 { get; }

  public bool Canceled { get; private set; }

  public bool Install { get; private set; }

  private async void UpdateWindow_Loaded(object sender, RoutedEventArgs e)
  {
    UpdateWindow updateWindow = this;
    try
    {
      updateWindow.downloading = true;
      // ISSUE: reference to a compiler-generated method
      string str = await UpdateHelper.DownloadUpdateFile(updateWindow.DownloadUrl, updateWindow.ValidationMD5, new Action<HttpHelperDownloadResponse>(updateWindow.\u003CUpdateWindow_Loaded\u003Eb__22_0), updateWindow.cts.Token);
      updateWindow.downloading = false;
      updateWindow.cts.Token.ThrowIfCancellationRequested();
      updateWindow.UpdateResult.SetupFilePath = str;
      updateWindow.UpdateResult.UpdateSuccess = !string.IsNullOrEmpty(str);
      if (!updateWindow.UpdateResult.UpdateSuccess)
        return;
      updateWindow.Downloading.Visibility = Visibility.Collapsed;
      updateWindow.Downloaded.Visibility = Visibility.Visible;
      updateWindow.Downladed1.Visibility = Visibility.Visible;
      updateWindow.btnOk.Visibility = Visibility.Visible;
      updateWindow.DownloadProgressBar.Value = 1.0;
      GAManager.SendEvent("AppUpdate", "DownloadSuccess", updateWindow.newVer.ToString(), 1L);
    }
    catch
    {
      updateWindow.downloading = false;
      if (updateWindow.DialogResult.HasValue)
        return;
      updateWindow.Install = true;
      updateWindow.DialogResult = new bool?(false);
    }
  }

  private void UpdateProgress(HttpHelperDownloadResponse resp)
  {
    if (!resp.ContentLength.HasValue)
      return;
    string str1 = UpdateHelper.FormatFileSize(resp.ContentLength.Value, (IFormatProvider) null, UpdateHelper.FileSizeFormatType.MB);
    string str2 = UpdateHelper.FormatFileSize(resp.Position, (IFormatProvider) null, UpdateHelper.FileSizeFormatType.MB);
    this.DownloadProgressBar.Value = resp.Progress;
    this.DownloadFileSizeText.Text = $"{str2} / {str1}";
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    base.OnClosing(e);
    if (!this.downloading)
      return;
    bool? nullable = UpdateMessage.CreateCancelMessageDialog((Window) this).ShowDialog();
    if ((!nullable.HasValue ? 0 : (nullable.GetValueOrDefault() ? 1 : 0)) != 0)
    {
      this.Canceled = true;
      this.Install = true;
      this.cts.Cancel();
    }
    else
      e.Cancel = true;
  }

  private void btnOk_Click(object sender, RoutedEventArgs e)
  {
    this.DialogResult = new bool?(this.UpdateResult.UpdateSuccess);
    this.Install = true;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/CommomLib;component/controls/updatewindow.xaml", UriKind.Relative));
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
        this.Downloading = (Grid) target;
        break;
      case 2:
        this.DownloadFileSizeText = (TextBlock) target;
        break;
      case 3:
        this.Downloaded = (TextBlock) target;
        break;
      case 4:
        this.DownloadProgressBar = (ProgressBar) target;
        break;
      case 5:
        this.Downladed1 = (TextBlock) target;
        break;
      case 6:
        this.btnOk = (Button) target;
        this.btnOk.Click += new RoutedEventHandler(this.btnOk_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
