// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.UpdateMessage
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom;
using CommomLib.IAP;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace CommomLib.Controls;

public partial class UpdateMessage : Window, IComponentConnector
{
  private Version version;
  private bool updateDoNotShowVersionFlag;
  internal Grid Newver;
  internal TextBlock NewVer;
  internal CheckBox NewVerShow;
  internal Button btnOk;
  internal Button btnCancel;
  internal Grid CancelMessage;
  internal Grid UpdateFailed;
  internal TextBlock DownloadUri;
  internal TextBlock underlinetext;
  internal Grid Uptodate;
  internal TextBlock Newer;
  private bool _contentLoaded;

  private UpdateMessage()
  {
    this.InitializeComponent();
    this.Closed += new EventHandler(this.UpdateMessage_Closed);
  }

  private void UpdateMessage_Closed(object sender, EventArgs e)
  {
    if (!this.updateDoNotShowVersionFlag || !this.NewVerShow.IsChecked.GetValueOrDefault())
      return;
    ConfigManager.setNotShowVersion(this.version);
  }

  private void btnCancel_Click(object sender, RoutedEventArgs e)
  {
    this.DialogResult = new bool?(false);
  }

  private void btnOk_Click(object sender, RoutedEventArgs e) => this.DialogResult = new bool?(true);

  private void Underline_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    IAPHelper.LaunchDownloadUri();
    this.DialogResult = new bool?(true);
  }

  private void Underline_MouseEnter(object sender, MouseEventArgs e)
  {
    this.underlinetext.Background = (Brush) new SolidColorBrush(Colors.Azure);
  }

  private void underlinetext_MouseLeave(object sender, MouseEventArgs e)
  {
    this.underlinetext.Background = (Brush) new SolidColorBrush(Colors.Transparent);
  }

  public static UpdateMessage CreateHasNewVersionDialog(Version version, bool checkUpdateManually)
  {
    Window mainWindow = Application.Current.MainWindow;
    UpdateMessage updateMessage = new UpdateMessage();
    updateMessage.Owner = mainWindow;
    updateMessage.WindowStartupLocation = mainWindow != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
    UpdateMessage newVersionDialog = updateMessage;
    newVersionDialog.Newver.Visibility = Visibility.Visible;
    newVersionDialog.version = version;
    newVersionDialog.updateDoNotShowVersionFlag = true;
    string updateMessageNewVer = CommomLib.Properties.Resources.UpdateMessageNewVer;
    int startIndex = updateMessageNewVer.IndexOf('[');
    int num = -1;
    if (startIndex >= 0)
      num = updateMessageNewVer.IndexOf(']', startIndex);
    if (startIndex == -1 || num == -1)
    {
      updateMessageNewVer.Replace("[", "").Replace("]", "");
      newVersionDialog.NewVer.Text = updateMessageNewVer;
    }
    else
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < updateMessageNewVer.Length + 1; ++index)
      {
        if (index == updateMessageNewVer.Length)
        {
          newVersionDialog.NewVer.Inlines.Add((Inline) new Run(stringBuilder.ToString()));
          stringBuilder.Length = 0;
        }
        else if (index == startIndex)
        {
          newVersionDialog.NewVer.Inlines.Add((Inline) new Run(stringBuilder.ToString()));
          stringBuilder.Length = 0;
        }
        else if (index == num)
        {
          Hyperlink hyperlink = new Hyperlink((Inline) new Run(stringBuilder.ToString()));
          stringBuilder.Length = 0;
          hyperlink.Click += (RoutedEventHandler) ((s, a) =>
          {
            string uri = "https://www.pdfgear.com/whats-new.htm";
            Task.Run<Process>((Func<Process>) (() => Process.Start(uri)));
          });
          newVersionDialog.NewVer.Inlines.Add((Inline) hyperlink);
        }
        else
          stringBuilder.Append(updateMessageNewVer[index]);
      }
    }
    if (checkUpdateManually)
      newVersionDialog.NewVerShow.Visibility = Visibility.Collapsed;
    return newVersionDialog;
  }

  public static UpdateMessage CreateUptodateWindows()
  {
    Window mainWindow = Application.Current.MainWindow;
    UpdateMessage uptodateWindows = new UpdateMessage();
    uptodateWindows.Owner = mainWindow;
    uptodateWindows.WindowStartupLocation = mainWindow != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
    uptodateWindows.Uptodate.Visibility = Visibility.Visible;
    return uptodateWindows;
  }

  public static UpdateMessage CreateCancelMessageDialog(Window owner)
  {
    UpdateMessage cancelMessageDialog = new UpdateMessage();
    cancelMessageDialog.Owner = owner;
    cancelMessageDialog.WindowStartupLocation = owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
    cancelMessageDialog.CancelMessage.Visibility = Visibility.Visible;
    return cancelMessageDialog;
  }

  public static UpdateMessage CreateUpdateFailedDialog()
  {
    Window mainWindow = Application.Current.MainWindow;
    UpdateMessage updateFailedDialog = new UpdateMessage();
    updateFailedDialog.Owner = mainWindow;
    updateFailedDialog.WindowStartupLocation = mainWindow != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
    updateFailedDialog.UpdateFailed.Visibility = Visibility.Visible;
    return updateFailedDialog;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/CommomLib;component/controls/updatemessage.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.Newver = (Grid) target;
        break;
      case 2:
        this.NewVer = (TextBlock) target;
        break;
      case 3:
        this.NewVerShow = (CheckBox) target;
        break;
      case 4:
        this.btnOk = (Button) target;
        this.btnOk.Click += new RoutedEventHandler(this.btnOk_Click);
        break;
      case 5:
        this.btnCancel = (Button) target;
        this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
        break;
      case 6:
        this.CancelMessage = (Grid) target;
        break;
      case 7:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.btnOk_Click);
        break;
      case 8:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.btnCancel_Click);
        break;
      case 9:
        this.UpdateFailed = (Grid) target;
        break;
      case 10:
        this.DownloadUri = (TextBlock) target;
        break;
      case 11:
        ((ContentElement) target).MouseLeave += new MouseEventHandler(this.underlinetext_MouseLeave);
        ((ContentElement) target).MouseEnter += new MouseEventHandler(this.Underline_MouseEnter);
        ((ContentElement) target).MouseLeftButtonDown += new MouseButtonEventHandler(this.Underline_MouseLeftButtonDown);
        break;
      case 12:
        this.underlinetext = (TextBlock) target;
        break;
      case 13:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.btnCancel_Click);
        break;
      case 14:
        this.Uptodate = (Grid) target;
        break;
      case 15:
        this.Newer = (TextBlock) target;
        break;
      case 16 /*0x10*/:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.btnCancel_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
