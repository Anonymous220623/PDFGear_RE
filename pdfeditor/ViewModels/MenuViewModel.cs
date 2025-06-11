// Decompiled with JetBrains decompiler
// Type: pdfeditor.ViewModels.MenuViewModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using pdfeditor.Models.LeftNavigations;
using pdfeditor.Models.Menus;
using pdfeditor.Properties;
using pdfeditor.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.ViewModels;

public class MenuViewModel : ObservableObject
{
  private readonly MainViewModel mainViewModel;
  private bool toolbarInited;
  private bool isShowToolbar;
  private bool isShowFooter = true;
  private SearchModel searchModel;
  private NavigationModel selectedLeftNavItem;
  private RelayCommand closeLeftNavMenuCmd;
  private RelayCommand showToolbarCmd;

  public MenuViewModel(MainViewModel mainViewModel)
  {
    this.mainViewModel = mainViewModel;
    ObservableCollection<MainMenuGroup> observableCollection1 = new ObservableCollection<MainMenuGroup>();
    observableCollection1.Add(new MainMenuGroup()
    {
      Title = Resources.MenuGruopHomeTitle,
      Tag = "View"
    });
    observableCollection1.Add(new MainMenuGroup()
    {
      Title = Resources.MenuGruopAnnotateTitle,
      Tag = "Annotate"
    });
    observableCollection1.Add(new MainMenuGroup()
    {
      Title = Resources.WinScreenshotToolbarEditContent,
      Tag = "Insert"
    });
    observableCollection1.Add(new MainMenuGroup()
    {
      Title = Resources.MenuGruopFillFormTitle,
      Tag = "FillForm"
    });
    observableCollection1.Add(new MainMenuGroup()
    {
      Title = Resources.MenuGruopPageTitle,
      Tag = "Page"
    });
    observableCollection1.Add(new MainMenuGroup()
    {
      Title = Resources.MenuGruopToolsTitle,
      Tag = "Tools"
    });
    observableCollection1.Add(new MainMenuGroup()
    {
      Title = Resources.MenuGruopHelpTitle,
      Tag = "Help"
    });
    this.MainMenus = observableCollection1;
    ObservableCollection<NavigationModel> observableCollection2 = new ObservableCollection<NavigationModel>();
    observableCollection2.Add(new NavigationModel("Bookmark", Resources.LeftNavigationViewBookmarkDisplayName, ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/LeftNavIcon/Bookmark.png"), new Uri("pack://application:,,,/Style/DarkModeResources/LeftNavIcon/Bookmark.png")), ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/LeftNavIcon/Bookmark-Select.png"), new Uri("pack://application:,,,/Style/DarkModeResources/LeftNavIcon/Bookmark-Select.png"))));
    observableCollection2.Add(new NavigationModel("Thumbnail", Resources.LeftNavigationViewThumbnailDisplayName, ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/LeftNavIcon/Thumbnail.png"), new Uri("pack://application:,,,/Style/DarkModeResources/LeftNavIcon/Thumbnail.png")), ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/LeftNavIcon/Thumbnail-Select.png"), new Uri("pack://application:,,,/Style/DarkModeResources/LeftNavIcon/Thumbnail-Select.png"))));
    observableCollection2.Add(new NavigationModel("Annotation", Resources.LeftNavigationViewAnnotationDisplayName, ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/LeftNavIcon/Annotation.png"), new Uri("pack://application:,,,/Style/DarkModeResources/LeftNavIcon/Annotation.png")), ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/LeftNavIcon/Annotation-Select.png"), new Uri("pack://application:,,,/Style/DarkModeResources/LeftNavIcon/Annotation-Select.png"))));
    this.LeftNavList = observableCollection2;
  }

  public ObservableCollection<MainMenuGroup> MainMenus { get; private set; }

  public ObservableCollection<NavigationModel> LeftNavList { get; private set; }

  public bool ToolbarInited
  {
    get => this.toolbarInited;
    set
    {
      if (!(this.SetProperty<bool>(ref this.toolbarInited, value, nameof (ToolbarInited)) & value))
        return;
      this.IsShowToolbar = true;
    }
  }

  public bool IsShowToolbar
  {
    get => this.isShowToolbar;
    set => this.SetProperty<bool>(ref this.isShowToolbar, value, nameof (IsShowToolbar));
  }

  public bool IsShowFooter
  {
    get => this.isShowFooter;
    set => this.SetProperty<bool>(ref this.isShowFooter, value, nameof (IsShowFooter));
  }

  public SearchModel SearchModel
  {
    get => this.searchModel ?? (this.SearchModel = new SearchModel(this.mainViewModel.Document));
    set
    {
      SearchModel searchModel = this.searchModel;
      if (!this.SetProperty<SearchModel>(ref this.searchModel, value, nameof (SearchModel)) || searchModel == null)
        return;
      __nonvirtual (searchModel.Dispose());
    }
  }

  public NavigationModel SelectedLeftNavItem
  {
    get => this.selectedLeftNavItem;
    set
    {
      if (!this.SetProperty<NavigationModel>(ref this.selectedLeftNavItem, value, nameof (SelectedLeftNavItem)) || !(this.selectedLeftNavItem?.Name == "Annotation"))
        return;
      this.mainViewModel.PageCommetSource?.StartLoad();
    }
  }

  public RelayCommand CloseLeftNavMenuCmd
  {
    get
    {
      return this.closeLeftNavMenuCmd ?? (this.closeLeftNavMenuCmd = new RelayCommand((Action) (() => this.SelectedLeftNavItem = (NavigationModel) null)));
    }
  }

  public RelayCommand ShowToolbarCmd
  {
    get
    {
      return this.showToolbarCmd ?? (this.showToolbarCmd = new RelayCommand(new Action(this.ShowToolbar)));
    }
  }

  private void ShowToolbar() => this.IsShowToolbar = !this.IsShowToolbar;

  public async Task ShowLeftNavMenuAsync(string menuName)
  {
    if (this.SelectedLeftNavItem?.Name == menuName)
      return;
    this.SelectedLeftNavItem = this.LeftNavList.FirstOrDefault<NavigationModel>((Func<NavigationModel, bool>) (c => c.Name == menuName));
    await DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() => { }));
  }
}
