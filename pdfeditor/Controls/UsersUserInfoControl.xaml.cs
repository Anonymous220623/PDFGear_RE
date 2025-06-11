// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Users.UserInfoControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using CommomLib.IAP;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace pdfeditor.Controls.Users;

public partial class UserInfoControl : Control
{
  private Grid LayoutRoot;
  private ContentControl ContentText;
  private Image PremiumBadge1;
  private Image PremiumBadge2;
  private TextBlock EmailText;
  private TextBlock PlanText;
  private TextBlock ExpireText;
  private Button PlanButton;
  private Button LogoutButton;
  private Popup popup;
  public static readonly DependencyProperty UserInfoProperty = DependencyProperty.Register(nameof (UserInfo), typeof (UserInfo), typeof (UserInfoControl), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is UserInfoControl userInfoControl2) || object.Equals(a.NewValue, a.OldValue))
      return;
    userInfoControl2.UpdateUserInfo();
  })));

  static UserInfoControl()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (UserInfoControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (UserInfoControl)));
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (this.PlanButton != null)
      this.PlanButton.Click -= new RoutedEventHandler(this.PlanButton_Click);
    if (this.LogoutButton != null)
      this.LogoutButton.Click -= new RoutedEventHandler(this.LogoutButton_Click);
    if (this.popup != null)
      this.popup.Opened -= new EventHandler(this.Popup_Opened);
    this.LayoutRoot = this.GetTemplateChild("LayoutRoot") as Grid;
    this.ContentText = this.GetTemplateChild("ContentText") as ContentControl;
    this.PremiumBadge1 = this.GetTemplateChild("PremiumBadge1") as Image;
    this.PremiumBadge2 = this.GetTemplateChild("PremiumBadge2") as Image;
    this.EmailText = this.GetTemplateChild("EmailText") as TextBlock;
    this.PlanText = this.GetTemplateChild("PlanText") as TextBlock;
    this.ExpireText = this.GetTemplateChild("ExpireText") as TextBlock;
    this.PlanButton = this.GetTemplateChild("PlanButton") as Button;
    this.LogoutButton = this.GetTemplateChild("LogoutButton") as Button;
    this.popup = this.GetTemplateChild("popup") as Popup;
    if (this.PlanButton != null)
      this.PlanButton.Click += new RoutedEventHandler(this.PlanButton_Click);
    if (this.LogoutButton != null)
      this.LogoutButton.Click += new RoutedEventHandler(this.LogoutButton_Click);
    if (this.popup != null)
      this.popup.Opened += new EventHandler(this.Popup_Opened);
    this.UpdateUserInfo();
  }

  public UserInfo UserInfo
  {
    get => (UserInfo) this.GetValue(UserInfoControl.UserInfoProperty);
    set => this.SetValue(UserInfoControl.UserInfoProperty, (object) value);
  }

  private void UpdateUserInfo()
  {
    UserInfo userInfo = this.UserInfo;
    if (!string.IsNullOrEmpty(userInfo?.Email))
    {
      if (this.LayoutRoot != null)
        this.LayoutRoot.Visibility = Visibility.Visible;
      string nextTextElement = StringInfo.GetNextTextElement(userInfo.Email);
      if (this.ContentText != null)
        this.ContentText.Content = (object) nextTextElement;
      bool flag = userInfo.Premium;
      if (flag && userInfo.IsSubscription && (!userInfo.ExpireTime.HasValue || (userInfo.ExpireTime.Value - DateTime.UtcNow).TotalSeconds < 0.0))
        flag = false;
      if (this.PremiumBadge1 != null)
        this.PremiumBadge1.Visibility = flag ? Visibility.Visible : Visibility.Collapsed;
      if (this.PremiumBadge2 != null)
        this.PremiumBadge2.Visibility = flag ? Visibility.Visible : Visibility.Collapsed;
      if (this.EmailText != null)
        this.EmailText.Text = userInfo.Email;
      if (flag)
      {
        if (this.PlanText != null)
          this.PlanText.Text = "Plan";
        if (userInfo.IsSubscription)
        {
          if (this.ExpireText == null)
            return;
          this.ExpireText.Text = $"Expire on {userInfo.ExpireTime.Value:d}";
        }
        else
        {
          if (this.ExpireText == null)
            return;
          this.ExpireText.Text = "Life-time";
        }
      }
      else
      {
        if (this.PlanText != null)
          this.PlanText.Text = "Buy Plan";
        if (this.ExpireText == null)
          return;
        this.ExpireText.Text = "";
      }
    }
    else
    {
      if (this.LayoutRoot == null)
        return;
      this.LayoutRoot.Visibility = Visibility.Collapsed;
    }
  }

  public void Open()
  {
    if (this.popup == null || this.popup.IsOpen || this.LayoutRoot.Visibility != Visibility.Visible)
      return;
    this.popup.IsOpen = true;
  }

  private async void LogoutButton_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent("MainWindow", "Logout", "Count", 1L);
    this.popup.IsOpen = false;
    await IAPHelper.LogoutAsync();
  }

  private void PlanButton_Click(object sender, RoutedEventArgs e)
  {
    IAPHelper.LaunchBuyPlanUri(email: this.UserInfo?.Email);
  }

  private void Popup_Opened(object sender, EventArgs e)
  {
    GAManager.SendEvent("MainWindow", "ShowPurchaseInfo", "Count", 1L);
  }
}
