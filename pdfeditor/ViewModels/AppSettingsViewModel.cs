// Decompiled with JetBrains decompiler
// Type: pdfeditor.ViewModels.AppSettingsViewModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using pdfeditor.Properties;
using PDFKit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace pdfeditor.ViewModels;

public class AppSettingsViewModel : ObservableObject
{
  private 
  #nullable disable
  string initLanguageName;
  private AsyncRelayCommand restartCommand;
  private ObservableCollection<LanguageModel> languages;
  private LanguageModel selectedLanguage;
  private string _selectedSizemode;
  private bool setAsDefaultApp;
  public bool _defaultAppOriginalSettings;
  public bool _chatButtonSettings = true;
  public bool _isFillFormHighlightedSettings = true;

  public FileWatcherHelper RecentFilesHelper => FileWatcherHelper.Instance;

  public RenderUtils RenderUtils => RenderUtils.Instance;

  public bool LanguageChangedFlag { get; private set; }

  public bool SetAsDefaultApp
  {
    get => this.setAsDefaultApp;
    set => this.SetProperty<bool>(ref this.setAsDefaultApp, value, nameof (SetAsDefaultApp));
  }

  public bool ChatButtonSettings
  {
    get => this._chatButtonSettings;
    set => this.SetProperty<bool>(ref this._chatButtonSettings, value, nameof (ChatButtonSettings));
  }

  public bool IsFillFormHighlightedSettings
  {
    get => this._isFillFormHighlightedSettings;
    set
    {
      this.SetProperty<bool>(ref this._isFillFormHighlightedSettings, value, nameof (IsFillFormHighlightedSettings));
    }
  }

  public Task RefreshSettingsAsync()
  {
    string appLang = ConfigManager.GetApplicationLanugageName();
    this.selectedLanguage = this.Languages.FirstOrDefault<LanguageModel>((Func<LanguageModel, bool>) (c => c.Name == appLang)) ?? LanguageModel.Fallback;
    this.OnPropertyChanged("SelectedLanguage");
    this.initLanguageName = this.selectedLanguage?.Name;
    FileWatcherHelper.Instance.Refresh();
    this.setAsDefaultApp = ((IEnumerable<string>) AppManager.GetDefaultFileExts()).All<string>((Func<string, bool>) (c => AppIdHelper.GetDefaultAppProgId(c) == "PdfGear.App.1"));
    this._defaultAppOriginalSettings = this.setAsDefaultApp;
    this.ChatButtonSettings = ConfigManager.GetShowcaseChatButtonFlag();
    this.IsFillFormHighlightedSettings = ConfigManager.GetIsFillFormHighlightedFlag();
    return Task.CompletedTask;
  }

  public ObservableCollection<LanguageModel> Languages
  {
    get
    {
      if (this.languages == null)
      {
        this.languages = new ObservableCollection<LanguageModel>();
        this.languages.Add(LanguageModel.Fallback);
        foreach (LanguageModel languageModel in (IEnumerable<LanguageModel>) LanguageModel.AllLanguageModel)
          this.languages.Add(languageModel);
      }
      return this.languages;
    }
  }

  public string[] SizeModes
  {
    get
    {
      return new List<string>()
      {
        Resources.MenuViewFullSizeContent,
        Resources.MenuViewFitPageContent,
        Resources.MenuViewHeightContent,
        Resources.MenuViewWidthContent
      }.ToArray();
    }
  }

  public string SelectedSizeMode
  {
    get
    {
      switch (ConfigManager.GetPageDefaultSize())
      {
        case "ZoomActualSize":
          return Resources.MenuViewFullSizeContent;
        case "FitToSize":
          return Resources.MenuViewFitPageContent;
        case "FitToWidth":
          return Resources.MenuViewWidthContent;
        case "FitToHeight":
          return Resources.MenuViewHeightContent;
        default:
          return Resources.MenuViewFullSizeContent;
      }
    }
    set
    {
      if (value == Resources.MenuViewFullSizeContent)
        this._selectedSizemode = "ZoomActualSize";
      else if (value == Resources.MenuViewFitPageContent)
        this._selectedSizemode = "FitToSize";
      else if (value == Resources.MenuViewWidthContent)
        this._selectedSizemode = "FitToWidth";
      else if (value == Resources.MenuViewHeightContent)
        this._selectedSizemode = "FitToHeight";
      else
        this._selectedSizemode = "ZoomActualSize";
    }
  }

  public LanguageModel SelectedLanguage
  {
    get => this.selectedLanguage;
    set
    {
      this.SetProperty<LanguageModel>(ref this.selectedLanguage, value, nameof (SelectedLanguage));
    }
  }

  public LanguageModel ActualLanguage
  {
    get
    {
      string appLang = ConfigManager.GetApplicationLanugageName();
      return this.Languages.FirstOrDefault<LanguageModel>((Func<LanguageModel, bool>) (c => c.Name == appLang)) ?? LanguageModel.Fallback;
    }
  }

  public AsyncRelayCommand RestartCommand
  {
    get
    {
      return this.restartCommand ?? (this.restartCommand = new AsyncRelayCommand((Func<Task>) (async () => { })));
    }
  }

  public void OnAppSettingsWindowClosing(bool result)
  {
    if (!result)
      return;
    if (this.initLanguageName != null && this.initLanguageName != this.selectedLanguage?.Name)
    {
      int num = (int) ModernMessageBox.Show(Resources.AppSettingsLanguageRestartTips, UtilManager.GetProductName());
      ConfigManager.SetApplicationLanugageName(this.selectedLanguage?.Name ?? "");
      this.LanguageChangedFlag = true;
      CommomLib.Commom.GAManager.SendEvent("SettingsWindow", "ChangeLanguage", "Count", 1L);
    }
    if (this.SelectedSizeMode != this._selectedSizemode)
    {
      ConfigManager.SetPageDefaultSize(this._selectedSizemode);
      CommomLib.Commom.GAManager.SendEvent("SettingsWindow", "ChangeDefaultSizemode", this._selectedSizemode, 1L);
    }
    if (this.SetAsDefaultApp != this._defaultAppOriginalSettings)
    {
      AppManager.RegisterFileAssociations(this.SetAsDefaultApp);
      ConfigManager.SetDefaultAppActionAsync((string) null);
    }
    if (this.ChatButtonSettings != ConfigManager.GetShowcaseChatButtonFlag())
    {
      ConfigManager.SetShowcaseChatButtonFlag(this.ChatButtonSettings);
      MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
      requiredService.ChatButtonVisible = this.ChatButtonSettings;
      if (!this.ChatButtonSettings)
      {
        CommomLib.Commom.GAManager.SendEvent("SettingsWindow", "EnableChat", "False", 1L);
        requiredService.ChatPanelVisible = false;
      }
      else
      {
        CommomLib.Commom.GAManager.SendEvent("SettingsWindow", "EnableChat", "True", 1L);
        requiredService.ChatPanelVisible = true;
      }
    }
    if (this.IsFillFormHighlightedSettings == ConfigManager.GetIsFillFormHighlightedFlag())
      return;
    CommomLib.Commom.GAManager.SendEvent("SettingsWindow", "FillFormHighlightedSettings", $"{this.IsFillFormHighlightedSettings}", 1L);
    ConfigManager.SetIsFillFormHighlightedFlag(this.IsFillFormHighlightedSettings);
    PdfControl pdfControl = PdfControl.GetPdfControl(Ioc.Default.GetRequiredService<MainViewModel>().Document);
    if (pdfControl == null)
      return;
    pdfControl.Viewer.IsFillFormHighlighted = this.IsFillFormHighlightedSettings;
  }
}
