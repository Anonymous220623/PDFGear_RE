// Decompiled with JetBrains decompiler
// Type: CommomLib.Views.FeedbackWindow
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom;
using CommomLib.Controls;
using CommomLib.IAP;
using Ionic.Zip;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Navigation;

#nullable disable
namespace CommomLib.Views;

public partial class FeedbackWindow : Window, IComponentConnector
{
  private string FeedbackWinErrorInvalidEmailCap = CommomLib.Properties.Resources.WinFeedBackErrorInvalidEmailCapMsg;
  private string FeedbackWinErrorInvalidEmailText = CommomLib.Properties.Resources.WinFeedBackInvalidEmailMsg;
  private string FeedbackWinSendSuccCaption = CommomLib.Properties.Resources.WinFeedBackSendSuccessCapMsg;
  private string FeedbackWinSendSuccText = CommomLib.Properties.Resources.WinFeedBackSendSuccessMsg;
  private string FeedbackWinSendTryagainText = CommomLib.Properties.Resources.WinFeedBackSendTryagainMsg;
  public List<string> flist = new List<string>();
  public string source = "";
  private bool bSendSampleCBIsChecked;
  private bool hasSampleFilesInAttachment;
  internal Grid FaqContainer;
  internal Button faq1Btn;
  internal Grid faq1Content;
  internal TextBox txtEmail;
  internal TextBox txtContent;
  internal StackPanel sendSampleGrid;
  internal CheckBox sendSampleCB;
  internal TextBlock filesTB;
  internal ProgressRing sendPgb;
  internal Button btnSend;
  private bool _contentLoaded;

  public FeedbackWindow() => this.InitializeComponent();

  public void SetChatDislike()
  {
    this.txtContent.Tag = (object) CommomLib.Properties.Resources.WinFeedBackCopilotContent;
  }

  public void HideFaq() => this.FaqContainer.Visibility = Visibility.Collapsed;

  public void showAttachmentCB(bool isShowing)
  {
    if (isShowing)
    {
      string str = "";
      foreach (string path in this.flist)
      {
        string fileName = Path.GetFileName(path);
        str = str.Length <= 0 ? fileName : $"{str}, {fileName}";
      }
      if (!string.IsNullOrWhiteSpace(str))
      {
        this.filesTB.Text = str;
        this.sendSampleGrid.Visibility = Visibility.Visible;
        this.sendSampleCB.IsChecked = new bool?(false);
        return;
      }
    }
    this.sendSampleGrid.Visibility = Visibility.Collapsed;
    this.sendSampleCB.IsChecked = new bool?(false);
  }

  private string CreateZipFile()
  {
    string localCachePath = UtilManager.GetLocalCachePath();
    string str1 = Path.Combine(UtilManager.GetAppDataPath(), "Logs");
    string str2 = Path.Combine(str1, "files");
    try
    {
      if (Directory.Exists(str2))
        Directory.Delete(str2, true);
    }
    catch
    {
    }
    Directory.CreateDirectory(str2);
    if (!Directory.Exists(str2))
      return "";
    try
    {
      CommomLib.Commom.Log.WriteLog($"{ConfigManager.GetOriginVersion()}, {UtilManager.GetAppVersion()}, {UtilManager.GetUUID()}, IAP: {(ValueType) false}, Editor");
      (string name, string nativeName, string englishName) cultureInfoNames = this.GetCurrentUICultureInfoNames();
      CommomLib.Commom.Log.WriteLog($"CultureInfo: {cultureInfoNames.englishName} | {cultureInfoNames.nativeName} | {cultureInfoNames.name}");
      Stopwatch stopwatch = Stopwatch.StartNew();
      string logStr = new DeviceInfo().ToString();
      stopwatch.Stop();
      CommomLib.Commom.Log.WriteLog(logStr);
    }
    catch
    {
    }
    if (this.bSendSampleCBIsChecked)
    {
      foreach (string str3 in this.flist)
      {
        if (System.IO.File.Exists(str3))
        {
          FileInfo fileInfo = new FileInfo(str3);
          if (fileInfo.Length <= 15728640L /*0xF00000*/)
          {
            string fileName = Path.GetFileName(str3);
            string str4 = Path.Combine(str2, fileName);
            if (!System.IO.File.Exists(str4))
            {
              fileInfo.CopyTo(str4, false);
              this.hasSampleFilesInAttachment = true;
            }
          }
        }
      }
    }
    string str5 = "";
    using (ZipFile zipFile = new ZipFile())
    {
      str5 = Path.Combine(localCachePath, "debug.zip");
      if (System.IO.File.Exists(str5))
        System.IO.File.Delete(str5);
      zipFile.Encryption = EncryptionAlgorithm.WinZipAes256;
      zipFile.Password = "pdfis~pdf";
      zipFile.AddDirectory(str1);
      zipFile.Save(str5);
    }
    return str5.Length > 0 && System.IO.File.Exists(str5) && new FileInfo(str5).Length < 15728640L /*0xF00000*/ ? str5 : "";
  }

  private (string name, string nativeName, string englishName) GetCurrentUICultureInfoNames()
  {
    CultureInfo currentUiCulture = CultureInfo.CurrentUICulture;
    string str1 = "";
    string str2 = "";
    string str3 = "";
    try
    {
      str1 = currentUiCulture.Name;
    }
    catch
    {
    }
    try
    {
      str2 = currentUiCulture.NativeName;
    }
    catch
    {
    }
    try
    {
      str3 = currentUiCulture.EnglishName;
    }
    catch
    {
    }
    return (str1, str2, str3);
  }

  private async void btnSend_Click(object sender, RoutedEventArgs e)
  {
    bool sendOk = false;
    string strPrio = "1";
    string strEmail = this.txtEmail.Text.Trim();
    if (strEmail.Length > 0 && !UtilManager.IsEmailValid(strEmail))
    {
      int num1 = (int) CommomLib.Commom.ModernMessageBox.Show(this.FeedbackWinErrorInvalidEmailText, this.FeedbackWinErrorInvalidEmailCap);
    }
    else
    {
      string strSubject = $"[User Feedback][Win] {UtilManager.GetProductName()} ({UtilManager.GetAppVersion()}";
      if (!string.IsNullOrEmpty(this.source))
        strSubject = $"{strSubject} {this.source}";
      strSubject += ")";
      string strDescription = this.txtContent.Text;
      string strEmail_r = this.txtEmail.Text.Trim().Length > 0 ? this.txtEmail.Text : "noreply@pdfgear.com";
      this.bSendSampleCBIsChecked = this.sendSampleCB.IsChecked.GetValueOrDefault();
      bool shoudSendAttachment = false;
      GAManager.SendEvent("ConvertFeedback", "ClickBtn", "Count", 1L);
      await Task.Run(TaskExceptionHelper.ExceptionBoundary((Action) (() =>
      {
        try
        {
          this.Dispatcher.Invoke((Action) (() => this.updateSendStatus(true)));
          string path = "";
          try
          {
            path = this.CreateZipFile();
            if (!string.IsNullOrWhiteSpace(path))
            {
              if (this.hasSampleFilesInAttachment)
                strPrio = "2";
              shoudSendAttachment = true;
            }
            else
              shoudSendAttachment = false;
          }
          catch
          {
          }
          if (IAPHelper.IsPaidUser)
            strPrio = "3";
          string str = "----------------------------" + DateTime.Now.Ticks.ToString("x");
          HttpWebRequest request = RequestHelper.CreateRequest("feedback", str);
          using (Stream requestStream = request.GetRequestStream())
          {
            RequestHelper.WriteBoundaryBytes(requestStream, str, false);
            RequestHelper.WriteContentDispositionFormDataHeader(requestStream, "email");
            RequestHelper.WriteString(requestStream, strEmail_r);
            RequestHelper.WriteCRLF(requestStream);
            RequestHelper.WriteBoundaryBytes(requestStream, str, false);
            RequestHelper.WriteContentDispositionFormDataHeader(requestStream, "subject");
            RequestHelper.WriteString(requestStream, strSubject);
            RequestHelper.WriteCRLF(requestStream);
            RequestHelper.WriteBoundaryBytes(requestStream, str, false);
            RequestHelper.WriteContentDispositionFormDataHeader(requestStream, "description");
            RequestHelper.WriteString(requestStream, strDescription);
            RequestHelper.WriteCRLF(requestStream);
            RequestHelper.WriteBoundaryBytes(requestStream, str, false);
            RequestHelper.WriteContentDispositionFormDataHeader(requestStream, "status");
            RequestHelper.WriteString(requestStream, "2");
            RequestHelper.WriteCRLF(requestStream);
            RequestHelper.WriteBoundaryBytes(requestStream, str, false);
            RequestHelper.WriteContentDispositionFormDataHeader(requestStream, "priority");
            RequestHelper.WriteString(requestStream, strPrio);
            RequestHelper.WriteCRLF(requestStream);
            RequestHelper.WriteBoundaryBytes(requestStream, str, false);
            RequestHelper.WriteContentDispositionFormDataHeader(requestStream, "source");
            RequestHelper.WriteString(requestStream, "1");
            RequestHelper.WriteCRLF(requestStream);
            RequestHelper.WriteBoundaryBytes(requestStream, str, false);
            RequestHelper.WriteContentDispositionFormDataHeader(requestStream, "app_id");
            RequestHelper.WriteString(requestStream, UtilManager.GetProductID());
            RequestHelper.WriteCRLF(requestStream);
            if (shoudSendAttachment)
            {
              GAManager.SendEvent("FeedbackWin", "SendAttach", "Click", 1L);
              string fileName = "log.zip";
              RequestHelper.WriteBoundaryBytes(requestStream, str, false);
              RequestHelper.WriteContentDispositionFileHeader(requestStream, "attachment", fileName, "text/plain");
              FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
              byte[] buffer = new byte[fileStream.Length];
              fileStream.Read(buffer, 0, buffer.Length);
              fileStream.Close();
              requestStream.Write(buffer, 0, buffer.Length);
              RequestHelper.WriteCRLF(requestStream);
            }
            RequestHelper.WriteBoundaryBytes(requestStream, str, true);
            requestStream.Close();
            try
            {
              HttpWebResponse response = (HttpWebResponse) request.GetResponse();
              Console.WriteLine("Status Code: {1} {0}", (object) response.StatusCode, (object) (int) response.StatusCode);
              if (response.StatusCode != HttpStatusCode.OK)
                return;
              sendOk = true;
            }
            catch (WebException ex)
            {
              Console.Write("Error WebException.");
            }
            catch (Exception ex)
            {
              Console.WriteLine("ERROR Exception.");
            }
          }
        }
        catch (Exception ex)
        {
        }
        finally
        {
          this.Dispatcher.Invoke((Action) (() => this.updateSendStatus(false)));
          if (sendOk)
          {
            int num2 = (int) MessageBox.Show(this.FeedbackWinSendSuccText, this.FeedbackWinSendSuccCaption, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            this.Dispatcher.Invoke((Action) (() => this.Close()));
          }
          else
          {
            int num3 = (int) MessageBox.Show(this.FeedbackWinSendTryagainText, "", MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
          }
        }
      }))).ConfigureAwait(false);
    }
  }

  private void updateSendStatus(bool showSending)
  {
    if (showSending)
    {
      this.txtEmail.IsEnabled = false;
      this.txtContent.IsEnabled = false;
      this.btnSend.IsEnabled = false;
      this.sendPgb.IsActive = true;
    }
    else
    {
      this.txtEmail.IsEnabled = true;
      this.txtContent.IsEnabled = true;
      this.btnSend.IsEnabled = true;
      this.sendPgb.IsActive = false;
    }
  }

  private void txtContent_TextChanged(object sender, TextChangedEventArgs e)
  {
    if (this.txtContent.Text.Trim().Length > 0)
      this.btnSend.IsEnabled = true;
    else
      this.btnSend.IsEnabled = false;
  }

  private void faq1Btn_Click(object sender, RoutedEventArgs e)
  {
    if (this.faq1Content.Visibility == Visibility.Visible)
    {
      this.faq1Btn.Content = (object) "\uE109";
      this.faq1Content.Visibility = Visibility.Collapsed;
    }
    else
    {
      this.faq1Btn.Content = (object) "\uE10A";
      this.faq1Content.Visibility = Visibility.Visible;
    }
  }

  private void Window_Loaded(object sender, RoutedEventArgs e)
  {
  }

  public static string GetLocationName()
  {
    try
    {
      RegionInfo homeLocation = CultureInfoUtils.GetHomeLocation();
      return homeLocation == null ? RegionInfo.CurrentRegion.DisplayName : homeLocation.DisplayName;
    }
    catch
    {
    }
    return string.Empty;
  }

  private void LearnMorelink_Click(object sender, RequestNavigateEventArgs e)
  {
    try
    {
      GAManager.SendEvent("FeedbackdWindow", "LearnMoreBtnClick", "Count", 1L);
      Process.Start((sender as Hyperlink).NavigateUri.AbsoluteUri);
    }
    catch
    {
    }
  }

  private void Hyperlink_Click(object sender, RequestNavigateEventArgs e)
  {
    try
    {
      GAManager.SendEvent("FeedbackdWindow", "RegionBtnClick", "Count", 1L);
      Process.Start("explorer.exe", "ms-settings:regionformatting");
    }
    catch
    {
    }
  }

  private async void RestoreLink_Click(object sender, RequestNavigateEventArgs e)
  {
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/CommomLib;component/views/feedbackwindow.xaml", UriKind.Relative));
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
        ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
        break;
      case 2:
        this.FaqContainer = (Grid) target;
        break;
      case 3:
        this.faq1Btn = (Button) target;
        this.faq1Btn.Click += new RoutedEventHandler(this.faq1Btn_Click);
        break;
      case 4:
        this.faq1Content = (Grid) target;
        break;
      case 5:
        this.txtEmail = (TextBox) target;
        break;
      case 6:
        this.txtContent = (TextBox) target;
        this.txtContent.TextChanged += new TextChangedEventHandler(this.txtContent_TextChanged);
        break;
      case 7:
        this.sendSampleGrid = (StackPanel) target;
        break;
      case 8:
        this.sendSampleCB = (CheckBox) target;
        break;
      case 9:
        this.filesTB = (TextBlock) target;
        break;
      case 10:
        this.sendPgb = (ProgressRing) target;
        break;
      case 11:
        this.btnSend = (Button) target;
        this.btnSend.Click += new RoutedEventHandler(this.btnSend_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
