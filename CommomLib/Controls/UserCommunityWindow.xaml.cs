// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.UserCommunityWindow
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom;
using QRCoder;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace CommomLib.Controls;

public partial class UserCommunityWindow : Window, IComponentConnector
{
  private string qqGroupLink;
  private bool submiting;
  private bool isDialog;
  internal Grid Triangles;
  internal Grid EmailContainer;
  internal TextBox EmailTextBox;
  internal Button SubmitButton;
  internal Grid ProgressDismiss;
  internal Grid QQGroupContainer;
  internal Border QRImageContainer;
  internal System.Windows.Controls.Image QRImage;
  internal TextBlock QQGroupName;
  internal Run QQGroupNumber;
  private bool _contentLoaded;

  private UserCommunityWindow() => this.InitializeComponent();

  public UserCommunityWindow(
    UserCommunityWindowType type,
    string qqGroupName,
    string qqGroupNumber,
    string qqGroupLink)
    : this()
  {
    switch (type)
    {
      case UserCommunityWindowType.Email:
        this.EmailContainer.Visibility = Visibility.Visible;
        this.EmailTextBox.Focus();
        Keyboard.Focus((IInputElement) this.EmailTextBox);
        break;
      case UserCommunityWindowType.QQGroup:
        this.QQGroupContainer.Visibility = Visibility.Visible;
        this.QQGroupName.Text = qqGroupName;
        this.QQGroupNumber.Text = qqGroupNumber;
        this.qqGroupLink = qqGroupLink;
        try
        {
          this.UpdateQRImage();
          break;
        }
        catch
        {
          this.QQGroupContainer.Visibility = Visibility.Collapsed;
          this.EmailContainer.Visibility = Visibility.Visible;
          break;
        }
    }
  }

  public UserCommunityWindow(UserCommunityWindowType type)
    : this(type, (string) null, (string) null, (string) null)
  {
  }

  public new bool? ShowDialog()
  {
    this.isDialog = true;
    return base.ShowDialog();
  }

  private async void SubmitButton_Click(object sender, RoutedEventArgs e)
  {
    UserCommunityWindow userCommunityWindow = this;
    if (userCommunityWindow.CanSubmit())
    {
      userCommunityWindow.ProgressDismiss.Visibility = Visibility.Visible;
      userCommunityWindow.submiting = true;
      int num = await NotifyUtils.NotifyServerUtils.SubmitEmailAsync(userCommunityWindow.EmailTextBox.Text) ? 1 : 0;
      userCommunityWindow.submiting = false;
      if (userCommunityWindow.isDialog)
        userCommunityWindow.DialogResult = new bool?(true);
      else
        userCommunityWindow.Close();
    }
    else
    {
      userCommunityWindow.EmailTextBox.Focus();
      Keyboard.Focus((IInputElement) userCommunityWindow.EmailTextBox);
    }
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    if (this.submiting)
      e.Cancel = true;
    base.OnClosing(e);
  }

  private bool CanSubmit()
  {
    string text = this.EmailTextBox.Text;
    int num = text.IndexOf('@');
    return num > 0 && num < text.Length - 1;
  }

  private void QRImageContainer_MouseDown(object sender, MouseButtonEventArgs e)
  {
    GAManager.SendEvent(nameof (UserCommunityWindow), "QQClick", "QRImageContainer", 1L);
    this.LaunchQQGroupLink();
  }

  private void QQGroupName_MouseDown(object sender, MouseButtonEventArgs e)
  {
    GAManager.SendEvent(nameof (UserCommunityWindow), "QQClick", "QQGroupName", 1L);
    this.LaunchQQGroupLink();
  }

  private void QQGroupNumber_MouseDown(object sender, MouseButtonEventArgs e)
  {
    GAManager.SendEvent(nameof (UserCommunityWindow), "QQClick", "QQGroupNumber", 1L);
    this.LaunchQQGroupLink();
  }

  private void Hyperlink_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent(nameof (UserCommunityWindow), "QQClick", "QQGroupAdd", 1L);
    this.LaunchQQGroupLink();
  }

  private void LaunchQQGroupLink()
  {
    if (string.IsNullOrEmpty(this.qqGroupLink))
      return;
    try
    {
      Process.Start(new ProcessStartInfo()
      {
        FileName = this.qqGroupLink,
        UseShellExecute = true
      });
    }
    catch
    {
    }
  }

  private void UpdateQRImage()
  {
    if (string.IsNullOrEmpty(this.qqGroupLink))
      return;
    using (QRCodeGenerator qrCodeGenerator = new QRCodeGenerator())
    {
      using (QRCodeData qrCode1 = qrCodeGenerator.CreateQrCode(this.qqGroupLink, QRCodeGenerator.ECCLevel.Q))
      {
        using (QRCode qrCode2 = new QRCode(qrCode1))
        {
          using (Bitmap graphic = qrCode2.GetGraphic(20))
          {
            IntPtr hbitmap = graphic.GetHbitmap();
            Int32Rect sourceRect = new Int32Rect(0, 0, graphic.Width, graphic.Height);
            try
            {
              this.QRImage.Source = (ImageSource) new WriteableBitmap(System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, sourceRect, BitmapSizeOptions.FromEmptyOptions()));
            }
            finally
            {
              try
              {
                if (hbitmap != IntPtr.Zero)
                  UserCommunityWindow.DeleteObject(hbitmap);
              }
              catch
              {
              }
            }
          }
        }
      }
    }
  }

  public static UserCommunityWindow CreateUserCommunityWindow()
  {
    CultureInfo currentUiCulture = CultureInfo.CurrentUICulture;
    AdManager.UserCommunityConfig userCommunityConfig = AdManager.GetUserCommunityConfig();
    if (userCommunityConfig == null)
      return (UserCommunityWindow) null;
    string name = currentUiCulture.Name;
    bool flag = name != null && name.Equals("zh-cn", StringComparison.OrdinalIgnoreCase);
    if (flag)
    {
      if (!userCommunityConfig.IsQQUCEnabled)
        return (UserCommunityWindow) null;
      if (string.IsNullOrEmpty(userCommunityConfig.QQGroupName) || string.IsNullOrEmpty(userCommunityConfig.QQGroupNumber) || string.IsNullOrEmpty(userCommunityConfig.QQGroupLink))
        return (UserCommunityWindow) null;
    }
    else if (!userCommunityConfig.IsEmailUCEnabled)
      return (UserCommunityWindow) null;
    UserCommunityWindow userCommunityWindow = new UserCommunityWindow(flag ? UserCommunityWindowType.QQGroup : UserCommunityWindowType.Email, userCommunityConfig?.QQGroupName ?? "", userCommunityConfig?.QQGroupNumber ?? "", userCommunityConfig?.QQGroupLink ?? "");
    if (userCommunityWindow == null)
      return userCommunityWindow;
    GAManager.SendEvent(nameof (UserCommunityWindow), "Show", flag.ToString(), 1L);
    return userCommunityWindow;
  }

  [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern bool DeleteObject(IntPtr hObject);

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/CommomLib;component/controls/usercommunitywindow.xaml", UriKind.Relative));
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
        this.Triangles = (Grid) target;
        break;
      case 2:
        this.EmailContainer = (Grid) target;
        break;
      case 3:
        this.EmailTextBox = (TextBox) target;
        break;
      case 4:
        this.SubmitButton = (Button) target;
        this.SubmitButton.Click += new RoutedEventHandler(this.SubmitButton_Click);
        break;
      case 5:
        this.ProgressDismiss = (Grid) target;
        break;
      case 6:
        this.QQGroupContainer = (Grid) target;
        break;
      case 7:
        this.QRImageContainer = (Border) target;
        this.QRImageContainer.MouseDown += new MouseButtonEventHandler(this.QRImageContainer_MouseDown);
        break;
      case 8:
        this.QRImage = (System.Windows.Controls.Image) target;
        break;
      case 9:
        ((Hyperlink) target).Click += new RoutedEventHandler(this.Hyperlink_Click);
        break;
      case 10:
        this.QQGroupName = (TextBlock) target;
        this.QQGroupName.MouseDown += new MouseButtonEventHandler(this.QQGroupName_MouseDown);
        break;
      case 11:
        ((UIElement) target).MouseDown += new MouseButtonEventHandler(this.QQGroupNumber_MouseDown);
        break;
      case 12:
        this.QQGroupNumber = (Run) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
