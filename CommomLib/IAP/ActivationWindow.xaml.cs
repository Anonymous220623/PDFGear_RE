// Decompiled with JetBrains decompiler
// Type: CommomLib.IAP.ActivationWindow
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Threading;

#nullable disable
namespace CommomLib.IAP;

public partial class ActivationWindow : Window, IComponentConnector
{
  private readonly string source;
  private readonly string ext;
  internal Grid Step1Grid;
  internal TextBlock AnnualPlanPrice;
  internal TextBlock PerpetualPlanPrice;
  internal ItemsControl PlanFeaturesList;
  internal Grid Step2Grid;
  internal TextBox EmailTextBox;
  internal TextBlock SendCodeErrText2;
  internal Button Step2GoBackButton;
  internal Grid Step3Grid;
  internal TextBox AccessCodeTextBox;
  internal CountdownButton SendCodeBtn;
  internal TextBlock ValidCodeErrText;
  internal TextBlock SendCodeErrText;
  internal Grid ProgressDismiss;
  private bool _contentLoaded;

  public ActivationWindow(string source, string ext)
  {
    this.InitializeComponent();
    this.source = source;
    this.ext = ext;
    this.PlanFeaturesList.ItemsSource = (IEnumerable) this.CreatePlanFeaturesList();
    GAManager.SendEvent(nameof (ActivationWindow), "Show", source, 1L);
  }

  public AccessTokenInfo AccessTokenInfo { get; private set; }

  private void CreateNewEmail_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent(nameof (ActivationWindow), "BuyPlan2BtnClick", this.source, 1L);
    IAPHelper.LaunchBuyPlanUri(email: this.EmailTextBox.Text);
  }

  private void ActivateNow_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent(nameof (ActivationWindow), "ActivateNowBtnClick", this.source, 1L);
    double oldWidth = this.ActualWidth;
    double oldHeight = this.ActualHeight;
    this.Step1Grid.Visibility = Visibility.Collapsed;
    this.Step2Grid.Visibility = Visibility.Visible;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() =>
    {
      this.Left += (oldWidth - this.ActualWidth) / 2.0;
      this.Top += (oldHeight - this.ActualHeight) / 2.0;
    }));
  }

  private void BuyPlan_Click(object sender, RoutedEventArgs e)
  {
    if (!((sender is FrameworkElement frameworkElement ? frameworkElement.Tag : (object) null) is string type))
      type = (string) null;
    GAManager.SendEvent(nameof (ActivationWindow), string.IsNullOrEmpty(type) ? "BuyPlanBtnClick" : "BuyPlanBtnClick_" + type, this.source, 1L);
    IAPHelper.LaunchBuyPlanUri(type);
  }

  private void Step2Back_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent(nameof (ActivationWindow), "BackFromSendLoginCodeBtnClick", this.source, 1L);
    this.Step2Grid.Visibility = Visibility.Collapsed;
    this.Step1Grid.Visibility = Visibility.Visible;
  }

  private async void SendLoginCode_Click(object sender, RoutedEventArgs e)
  {
    ActivationWindow activationWindow = this;
    if (string.IsNullOrEmpty(activationWindow.EmailTextBox.Text))
    {
      // ISSUE: reference to a compiler-generated method
      activationWindow.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) new Action(activationWindow.\u003CSendLoginCode_Click\u003Eb__11_0));
    }
    else
    {
      GAManager.SendEvent(nameof (ActivationWindow), "SendLoginCodeBtnClick", activationWindow.source, 1L);
      activationWindow.ValidCodeErrText.Visibility = Visibility.Collapsed;
      activationWindow.SendCodeErrText.Visibility = Visibility.Collapsed;
      activationWindow.SendCodeErrText2.Visibility = Visibility.Collapsed;
      activationWindow.ShowProgressRing();
      try
      {
        if (await InternalActivateHelper.SendLoginCodeAsync(activationWindow.EmailTextBox.Text, false))
        {
          GAManager.SendEvent(nameof (ActivationWindow), "SendLoginCodeSucc", activationWindow.source, 1L);
          activationWindow.SendCodeBtn.CountdownSeconds = 0;
          activationWindow.SendCodeBtn.Countdown();
          activationWindow.Step2Grid.Visibility = Visibility.Collapsed;
          activationWindow.Step3Grid.Visibility = Visibility.Visible;
        }
        else
        {
          GAManager.SendEvent(nameof (ActivationWindow), "SendLoginCodeFailed", activationWindow.source, 1L);
          activationWindow.SendCodeErrText2.Visibility = Visibility.Visible;
          activationWindow.SendCodeErrText.Visibility = Visibility.Visible;
        }
      }
      catch
      {
      }
      activationWindow.HideProgressRing();
    }
  }

  private async void CountdownButton_Click(object sender, RoutedEventArgs e)
  {
    this.ValidCodeErrText.Visibility = Visibility.Collapsed;
    this.SendCodeErrText.Visibility = Visibility.Collapsed;
    this.SendCodeErrText2.Visibility = Visibility.Collapsed;
    GAManager.SendEvent(nameof (ActivationWindow), "SendLoginCodeAgainBtnClick", this.source, 1L);
    this.ShowProgressRing();
    try
    {
      if (await InternalActivateHelper.SendLoginCodeAsync(this.EmailTextBox.Text, true))
      {
        GAManager.SendEvent(nameof (ActivationWindow), "SendLoginCodeAgainSucc", this.source, 1L);
        ((CountdownButton) sender).Countdown();
      }
      else
      {
        GAManager.SendEvent(nameof (ActivationWindow), "SendLoginCodeAgainFailed", this.source, 1L);
        this.SendCodeErrText.Visibility = Visibility.Visible;
      }
    }
    catch
    {
    }
    this.HideProgressRing();
  }

  private async void ValidCodeButton_Click(object sender, RoutedEventArgs e)
  {
    ActivationWindow activationWindow = this;
    activationWindow.ValidCodeErrText.Visibility = Visibility.Collapsed;
    if (string.IsNullOrEmpty(activationWindow.AccessCodeTextBox.Text) || activationWindow.SendCodeErrText.Visibility == Visibility.Visible)
    {
      // ISSUE: reference to a compiler-generated method
      activationWindow.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) new Action(activationWindow.\u003CValidCodeButton_Click\u003Eb__13_0));
    }
    else
    {
      GAManager.SendEvent(nameof (ActivationWindow), "LoginBtnClick", activationWindow.source, 1L);
      activationWindow.ShowProgressRing();
      try
      {
        AccessTokenInfo tokenAsync = await InternalActivateHelper.GetTokenAsync(activationWindow.EmailTextBox.Text, activationWindow.AccessCodeTextBox.Text);
        if (tokenAsync != null)
        {
          GAManager.SendEvent(nameof (ActivationWindow), "LoginSucc", activationWindow.source, 1L);
          activationWindow.Step3Grid.Visibility = Visibility.Collapsed;
          activationWindow.AccessTokenInfo = tokenAsync;
          activationWindow.DialogResult = new bool?(true);
        }
        else
        {
          GAManager.SendEvent(nameof (ActivationWindow), "LoginFailed", activationWindow.source, 1L);
          activationWindow.ValidCodeErrText.Visibility = Visibility.Visible;
        }
      }
      catch
      {
      }
      activationWindow.HideProgressRing();
    }
  }

  private void Step3Back_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent(nameof (ActivationWindow), "BackFromLoginBtnClick", this.source, 1L);
    this.SendCodeErrText.Visibility = Visibility.Collapsed;
    this.SendCodeErrText.Visibility = Visibility.Collapsed;
    this.ValidCodeErrText.Visibility = Visibility.Collapsed;
    this.Step3Grid.Visibility = Visibility.Collapsed;
    this.Step2Grid.Visibility = Visibility.Visible;
  }

  private void ShowProgressRing()
  {
    this.ProgressDismiss.Visibility = Visibility.Visible;
    this.IsEnabled = false;
  }

  private void HideProgressRing()
  {
    this.IsEnabled = true;
    this.ProgressDismiss.Visibility = Visibility.Collapsed;
  }

  public static AccessTokenInfo CreateDialog(string source, string ext, bool onlyLogin)
  {
    Window[] array = Application.Current.Windows.OfType<Window>().ToArray<Window>();
    ActivationWindow activationWindow1 = array.OfType<ActivationWindow>().FirstOrDefault<ActivationWindow>();
    if (activationWindow1 != null)
    {
      activationWindow1.Activate();
      return (AccessTokenInfo) null;
    }
    ActivationWindow activationWindow2 = new ActivationWindow(source, ext);
    activationWindow2.Owner = ((IEnumerable<Window>) array).FirstOrDefault<Window>((Func<Window, bool>) (c =>
    {
      string name = c.GetType().Name;
      return name == "MainView" || name == "MainWindow";
    }));
    if (activationWindow2.Owner != null)
      activationWindow2.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    if (onlyLogin)
    {
      activationWindow2.Step2Grid.Visibility = Visibility.Visible;
      activationWindow2.Step1Grid.Visibility = Visibility.Collapsed;
      activationWindow2.Step2GoBackButton.Visibility = Visibility.Collapsed;
    }
    return activationWindow2.ShowDialog().GetValueOrDefault() && activationWindow2.AccessTokenInfo != null ? activationWindow2.AccessTokenInfo : (AccessTokenInfo) null;
  }

  private IReadOnlyList<ActivationWindow.PlanFeaturesGroupModel> CreatePlanFeaturesList()
  {
    return (IReadOnlyList<ActivationWindow.PlanFeaturesGroupModel>) new List<ActivationWindow.PlanFeaturesGroupModel>()
    {
      new ActivationWindow.PlanFeaturesGroupModel("Convert PDF")
      {
        Items = {
          new ActivationWindow.PlanFeaturesListItemModel("Convert PDF files to Word, Excel, PNG, JPG, RTF, TXT, HTML & XML", false, true, true),
          new ActivationWindow.PlanFeaturesListItemModel("Convert Word, Excel, PowerPoint, PNG & JPEG to PDF", false, true, true)
        }
      },
      new ActivationWindow.PlanFeaturesGroupModel("Edit PDF Page")
      {
        Items = {
          new ActivationWindow.PlanFeaturesListItemModel("Delete, insert, extract, reorder pages", false, true, true),
          new ActivationWindow.PlanFeaturesListItemModel("Sign PDF", false, true, true),
          new ActivationWindow.PlanFeaturesListItemModel("Merge, split, compress PDF", false, true, true)
        }
      },
      new ActivationWindow.PlanFeaturesGroupModel("View PDF")
      {
        Items = {
          new ActivationWindow.PlanFeaturesListItemModel("Reading modes, search text, thumbnails, bookmark", true, true, true),
          new ActivationWindow.PlanFeaturesListItemModel("Fill out PDF forms", true, true, true),
          new ActivationWindow.PlanFeaturesListItemModel("Print PDF documents", true, true, true)
        }
      },
      new ActivationWindow.PlanFeaturesGroupModel("Annotate PDF")
      {
        Items = {
          new ActivationWindow.PlanFeaturesListItemModel("Highlight, underline, strike-out specific text", true, true, true),
          new ActivationWindow.PlanFeaturesListItemModel("Freehand shapes, text box & anchored note", true, true, true)
        }
      }
    };
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/CommomLib;component/iap/activationwindow.xaml", UriKind.Relative));
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
        this.Step1Grid = (Grid) target;
        break;
      case 2:
        this.AnnualPlanPrice = (TextBlock) target;
        break;
      case 3:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.BuyPlan_Click);
        break;
      case 4:
        this.PerpetualPlanPrice = (TextBlock) target;
        break;
      case 5:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.BuyPlan_Click);
        break;
      case 6:
        this.PlanFeaturesList = (ItemsControl) target;
        break;
      case 7:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.ActivateNow_Click);
        break;
      case 8:
        this.Step2Grid = (Grid) target;
        break;
      case 9:
        this.EmailTextBox = (TextBox) target;
        break;
      case 10:
        this.SendCodeErrText2 = (TextBlock) target;
        break;
      case 11:
        ((Hyperlink) target).Click += new RoutedEventHandler(this.CreateNewEmail_Click);
        break;
      case 12:
        this.Step2GoBackButton = (Button) target;
        this.Step2GoBackButton.Click += new RoutedEventHandler(this.Step2Back_Click);
        break;
      case 13:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.SendLoginCode_Click);
        break;
      case 14:
        this.Step3Grid = (Grid) target;
        break;
      case 15:
        this.AccessCodeTextBox = (TextBox) target;
        break;
      case 16 /*0x10*/:
        this.SendCodeBtn = (CountdownButton) target;
        break;
      case 17:
        this.ValidCodeErrText = (TextBlock) target;
        break;
      case 18:
        this.SendCodeErrText = (TextBlock) target;
        break;
      case 19:
        ((Hyperlink) target).Click += new RoutedEventHandler(this.CreateNewEmail_Click);
        break;
      case 20:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.Step3Back_Click);
        break;
      case 21:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.ValidCodeButton_Click);
        break;
      case 22:
        this.ProgressDismiss = (Grid) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }

  private class PlanFeaturesListItemModel
  {
    public PlanFeaturesListItemModel(string title, bool free, bool annualPlan, bool perpetualPlan)
    {
      this.Title = title;
      this.Free = free;
      this.AnnualPlan = annualPlan;
      this.PerpetualPlan = perpetualPlan;
    }

    public string Title { get; }

    public bool Free { get; }

    public bool AnnualPlan { get; }

    public bool PerpetualPlan { get; }
  }

  private class PlanFeaturesGroupModel
  {
    public PlanFeaturesGroupModel(string title) => this.Title = title;

    public string Title { get; }

    public List<ActivationWindow.PlanFeaturesListItemModel> Items { get; } = new List<ActivationWindow.PlanFeaturesListItemModel>();
  }
}
