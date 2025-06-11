// Decompiled with JetBrains decompiler
// Type: pdfconverter.FeedbackWindow
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using CommomLib.Controls;
using Ionic.Zip;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfconverter;

public partial class FeedbackWindow : Window, IComponentConnector
{
  public string source = "";
  public List<string> flist = new List<string>();
  internal TextBox txtEmail;
  internal TextBox txtContent;
  internal StackPanel sendSampleGrid;
  internal CheckBox sendSampleCB;
  internal TextBox filesTB;
  internal Button sendBtn;
  internal Button closeBtn;
  internal ProgressRing sendingProgessBar;
  private bool _contentLoaded;

  public FeedbackWindow()
  {
    this.InitializeComponent();
    GAManager.SendEvent("ConvertFeedback", "Show", "Count", 1L);
  }

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
    if (this.flist == null || this.flist.Count<string>() <= 0)
      return "";
    string localCacheFolder = AppDataHelper.LocalCacheFolder;
    string str1 = Path.Combine(localCacheFolder, "files");
    try
    {
      if (Directory.Exists(str1))
        Directory.Delete(str1, true);
    }
    catch
    {
    }
    Directory.CreateDirectory(str1);
    if (!Directory.Exists(str1))
      return "";
    foreach (string str2 in this.flist)
    {
      if (System.IO.File.Exists(str2))
      {
        FileInfo fileInfo = new FileInfo(str2);
        if (fileInfo.Length <= 15728640L /*0xF00000*/)
        {
          string fileName = Path.GetFileName(str2);
          string str3 = Path.Combine(str1, fileName);
          if (!System.IO.File.Exists(str3))
            fileInfo.CopyTo(str3, false);
        }
      }
    }
    string str4 = "";
    using (ZipFile zipFile = new ZipFile())
    {
      str4 = Path.Combine(localCacheFolder, "debug.zip");
      if (System.IO.File.Exists(str4))
        System.IO.File.Delete(str4);
      zipFile.Encryption = EncryptionAlgorithm.WinZipAes256;
      zipFile.Password = "pdfis~pdf";
      zipFile.AddDirectory(str1);
      zipFile.Save(str4);
    }
    return str4.Length > 0 && System.IO.File.Exists(str4) && new FileInfo(str4).Length < 15728640L /*0xF00000*/ ? str4 : "";
  }

  private void TxtContent_TextChanged(object sender, TextChangedEventArgs e)
  {
    if (this.txtContent.Text.Trim().Length > 0)
      this.sendBtn.IsEnabled = true;
    else
      this.sendBtn.IsEnabled = false;
  }

  private void CloseBtn_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent("ConvertFeedback", "CloseBtn", "Count", 1L);
    this.Close();
  }

  private async void SendBtn_Click(object sender, RoutedEventArgs e)
  {
    bool sendOk = false;
    string strPrio = "3";
    string strEmail = this.txtEmail.Text.Trim();
    if (strEmail.Length > 0 && !this.IsEmailValid(strEmail))
    {
      int num1 = (int) CommomLib.Commom.ModernMessageBox.Show(pdfconverter.Properties.Resources.FeedbackWindowInvalidEmailMsgContent, pdfconverter.Properties.Resources.FeedbackWindowInvalidEmailMsgTitle);
    }
    else
    {
      string strSubject = $"[User Feedback][Win] {UtilManager.GetProductName()} ({UtilManager.GetAppVersion()})";
      string strDescription = this.txtContent.Text;
      string strEmail_r = this.txtEmail.Text.Trim().Length > 0 ? this.txtEmail.Text : "noreply@pdfgear.com";
      bool shoudSendAttachment = false;
      if (this.sendSampleCB.IsChecked.GetValueOrDefault())
        shoudSendAttachment = true;
      GAManager.SendEvent("ConvertFeedback", "ClickBtn", "Count", 1L);
      await Task.Run(TaskExceptionHelper.ExceptionBoundary((Action) (() =>
      {
        try
        {
          this.Dispatcher.Invoke((Action) (() => this.updateSendStatus(true)));
          string path = "";
          if (shoudSendAttachment)
          {
            try
            {
              path = this.CreateZipFile();
            }
            catch
            {
            }
          }
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
            int num2 = (int) MessageBox.Show(pdfconverter.Properties.Resources.FeedbackWindowSendSuccMsgContent, pdfconverter.Properties.Resources.FeedbackWindowSendSuccMsgTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            this.Dispatcher.Invoke((Action) (() => this.Close()));
          }
          else
          {
            int num3 = (int) MessageBox.Show(pdfconverter.Properties.Resources.WinFeedBackSendTryagainMsg, UtilManager.GetProductName(), MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
          }
        }
      }))).ConfigureAwait(false);
    }
  }

  private bool IsEmailValid(string strEmail)
  {
    return Regex.Match(strEmail, "[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2}").Success;
  }

  private void updateSendStatus(bool showSending)
  {
    if (showSending)
    {
      this.txtEmail.IsEnabled = false;
      this.txtContent.IsEnabled = false;
      this.sendBtn.IsEnabled = false;
      this.sendingProgessBar.IsActive = true;
    }
    else
    {
      this.txtEmail.IsEnabled = true;
      this.txtContent.IsEnabled = true;
      this.sendBtn.IsEnabled = true;
      this.sendingProgessBar.IsActive = false;
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfconverter;component/feedbackwindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.txtEmail = (TextBox) target;
        break;
      case 2:
        this.txtContent = (TextBox) target;
        this.txtContent.TextChanged += new TextChangedEventHandler(this.TxtContent_TextChanged);
        break;
      case 3:
        this.sendSampleGrid = (StackPanel) target;
        break;
      case 4:
        this.sendSampleCB = (CheckBox) target;
        break;
      case 5:
        this.filesTB = (TextBox) target;
        break;
      case 6:
        this.sendBtn = (Button) target;
        this.sendBtn.Click += new RoutedEventHandler(this.SendBtn_Click);
        break;
      case 7:
        this.closeBtn = (Button) target;
        this.closeBtn.Click += new RoutedEventHandler(this.CloseBtn_Click);
        break;
      case 8:
        this.sendingProgessBar = (ProgressRing) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
