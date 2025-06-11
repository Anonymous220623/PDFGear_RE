// Decompiled with JetBrains decompiler
// Type: pdfeditor.ViewModels.ShareTabViewModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using pdfeditor.Controls;
using pdfeditor.Models.Menus;
using pdfeditor.Properties;
using pdfeditor.Utils;
using pdfeditor.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable enable
namespace pdfeditor.ViewModels;

public class ShareTabViewModel : ObservableObject
{
  private readonly 
  #nullable disable
  MainViewModel mainViewModel;
  private ToolbarButtonModel fileButtonModel;
  private ToolbarButtonModel shareButtonModel;
  private ToolbarButtonModel emailButtonModel;

  public ShareTabViewModel(MainViewModel mainViewModel) => this.mainViewModel = mainViewModel;

  public ToolbarButtonModel FileButtonModel
  {
    get
    {
      ToolbarButtonModel fileButtonModel1 = this.fileButtonModel;
      if (fileButtonModel1 != null)
        return fileButtonModel1;
      ToolbarButtonModel toolbarButtonModel = new ToolbarButtonModel();
      toolbarButtonModel.Caption = Resources.MenuShareSendFileContent;
      toolbarButtonModel.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Share_SendFile.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Share_SendFile.png"));
      toolbarButtonModel.Command = (ICommand) new RelayCommand((Action) (() =>
      {
        if (this.mainViewModel.Document == null)
          return;
        if (this.mainViewModel.CanSave)
        {
          int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeConvertMsg, UtilManager.GetProductName());
        }
        else
        {
          MainView mainView = App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
          new ShareSendFileDialog(this.mainViewModel.DocumentWrapper.DocumentPath)
          {
            Owner = ((Window) mainView),
            WindowStartupLocation = (mainView == null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner)
          }.ShowDialog();
        }
      }));
      ToolbarButtonModel fileButtonModel2 = toolbarButtonModel;
      this.fileButtonModel = toolbarButtonModel;
      return fileButtonModel2;
    }
  }

  public ToolbarButtonModel ShareButtonModel
  {
    get
    {
      ToolbarButtonModel shareButtonModel1 = this.shareButtonModel;
      if (shareButtonModel1 != null)
        return shareButtonModel1;
      ToolbarButtonModel toolbarButtonModel = new ToolbarButtonModel();
      toolbarButtonModel.Caption = Resources.MenuShareShareContent;
      toolbarButtonModel.Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Share_Share.png"));
      toolbarButtonModel.Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () =>
      {
        if (this.mainViewModel.Document == null)
          return;
        if (this.mainViewModel.CanSave)
        {
          int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeConvertMsg, UtilManager.GetProductName());
        }
        else
          await ShareUtils.WindowsShareAsync(this.mainViewModel.DocumentWrapper.DocumentPath);
      }));
      ToolbarButtonModel shareButtonModel2 = toolbarButtonModel;
      this.shareButtonModel = toolbarButtonModel;
      return shareButtonModel2;
    }
  }

  public ToolbarButtonModel EmailButtonModel
  {
    get
    {
      ToolbarButtonModel emailButtonModel1 = this.emailButtonModel;
      if (emailButtonModel1 != null)
        return emailButtonModel1;
      ToolbarButtonModel toolbarButtonModel = new ToolbarButtonModel();
      toolbarButtonModel.Caption = Resources.MenuShareEmailContent;
      toolbarButtonModel.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Share_Email.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Share_Email.png"));
      toolbarButtonModel.Command = (ICommand) new AsyncRelayCommand((Func<Task>) (async () =>
      {
        if (this.mainViewModel.Document == null)
          return;
        if (this.mainViewModel.CanSave)
        {
          int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeConvertMsg, UtilManager.GetProductName());
        }
        else
          await ShareUtils.SendMailAsync(this.mainViewModel.DocumentWrapper.DocumentPath);
      }));
      ToolbarButtonModel emailButtonModel2 = toolbarButtonModel;
      this.emailButtonModel = toolbarButtonModel;
      return emailButtonModel2;
    }
  }
}
