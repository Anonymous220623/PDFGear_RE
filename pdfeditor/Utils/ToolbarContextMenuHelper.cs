// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.ToolbarContextMenuHelper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using pdfeditor.Controls.Menus;
using pdfeditor.Controls.Speech;
using pdfeditor.Models.Menus;
using pdfeditor.Models.Viewer;
using pdfeditor.Properties;
using pdfeditor.ViewModels;
using PDFKit.Utils;
using PDFKit.Utils.StampUtils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfeditor.Utils;

public static class ToolbarContextMenuHelper
{
  private const string StampTextApproved = "Approved";
  private const string StampTextDraft = "Draft";
  private const string StampTextNotApproved = "NotApproved";
  private const string StampTextConfidential = "Confidential";
  private static AnnotationMode[] allAnnotationMode;
  private static ToolbarContextMenuHelper.MenuValueProvider[] strokeThicknessMenuValues;
  private static ToolbarContextMenuHelper.MenuValueProvider[] strokeColorMenuValues;
  private static ToolbarContextMenuHelper.MenuValueProvider[] fillColorMenuValues;
  private static ToolbarContextMenuHelper.MenuValueProvider[] fontColorMenuValues;
  private static ToolbarContextMenuHelper.MenuValueProvider[] fontSizeMenuValues;
  private static IStampTextModel[] stampPresetsMenuValues;
  private static BackgroundColorSetting[] viewerBackgroundColorValues;
  private static IReadOnlyDictionary<string, BackgroundColorSetting> viewerBackgroundColorDict;

  static ToolbarContextMenuHelper()
  {
    ToolbarContextMenuHelper.InitAllAnnotationMode();
    ToolbarContextMenuHelper.InitFillColorMenuValues();
    ToolbarContextMenuHelper.InitStrokeColorMenuValues();
    ToolbarContextMenuHelper.InitStrokeThicknessMenuValues();
    ToolbarContextMenuHelper.InitFontColorMenuValues();
    ToolbarContextMenuHelper.InitFontSizeMenuValue();
    ToolbarContextMenuHelper.InitStampPresetsMenuValues();
    ToolbarContextMenuHelper.InitViewerBackgroundColorValues();
  }

  public static IContextMenuModel CreateStrokeColorMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action,
    bool addMoreItem = false)
  {
    TypedContextMenuItemModel contextMenuItemModel = new TypedContextMenuItemModel(ContextMenuItemType.StrokeColor);
    contextMenuItemModel.Name = "Color";
    contextMenuItemModel.Caption = Resources.LabelColorContent;
    contextMenuItemModel.IsChecked = true;
    contextMenuItemModel.Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/linecolor.png"));
    contextMenuItemModel.TagData = new TagDataModel()
    {
      AnnotationMode = mode,
      MenuItemType = ContextMenuItemType.StrokeColor
    };
    TypedContextMenuItemModel strokeColorMenu = contextMenuItemModel;
    foreach (IContextMenuModel contextMenuItem in ToolbarContextMenuHelper.CreateContextMenuItems(mode, ContextMenuItemType.StrokeColor, action))
      strokeColorMenu.Add(contextMenuItem);
    if (addMoreItem)
      strokeColorMenu.Add((IContextMenuModel) ToolbarContextMenuHelper.CreateColorMoreItem(mode, ContextMenuItemType.StrokeColor, action));
    return (IContextMenuModel) strokeColorMenu;
  }

  public static IContextMenuModel CreateThicknessMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action)
  {
    TypedContextMenuItemModel contextMenuItemModel = new TypedContextMenuItemModel(ContextMenuItemType.StrokeThickness);
    contextMenuItemModel.Name = "Thickness";
    contextMenuItemModel.Caption = Resources.MenuSubThicknessItem;
    contextMenuItemModel.IsChecked = true;
    contextMenuItemModel.TagData = new TagDataModel()
    {
      AnnotationMode = mode,
      MenuItemType = ContextMenuItemType.StrokeThickness
    };
    contextMenuItemModel.Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/linewidth.png"));
    TypedContextMenuItemModel thicknessMenu = contextMenuItemModel;
    foreach (IContextMenuModel contextMenuItem in ToolbarContextMenuHelper.CreateContextMenuItems(mode, ContextMenuItemType.StrokeThickness, action))
      thicknessMenu.Add(contextMenuItem);
    return (IContextMenuModel) thicknessMenu;
  }

  public static IContextMenuModel CreateFillMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action,
    bool addMoreItem = false)
  {
    TypedContextMenuItemModel contextMenuItemModel = new TypedContextMenuItemModel(ContextMenuItemType.FillColor);
    contextMenuItemModel.Name = "FillColor";
    contextMenuItemModel.Caption = Resources.MenuSubFillColorItem;
    contextMenuItemModel.IsChecked = true;
    contextMenuItemModel.Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/linecolor.png"));
    contextMenuItemModel.TagData = new TagDataModel()
    {
      AnnotationMode = mode,
      MenuItemType = ContextMenuItemType.FillColor
    };
    TypedContextMenuItemModel fillMenu = contextMenuItemModel;
    foreach (IContextMenuModel contextMenuItem in ToolbarContextMenuHelper.CreateContextMenuItems(mode, ContextMenuItemType.FillColor, action))
      fillMenu.Add(contextMenuItem);
    if (addMoreItem)
      fillMenu.Add((IContextMenuModel) ToolbarContextMenuHelper.CreateColorMoreItem(mode, ContextMenuItemType.FillColor, action));
    return (IContextMenuModel) fillMenu;
  }

  public static IContextMenuModel CreateFontColorMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action,
    bool addMoreItem = false)
  {
    TypedContextMenuItemModel contextMenuItemModel = new TypedContextMenuItemModel(ContextMenuItemType.FontColor);
    contextMenuItemModel.Name = "FontColor";
    contextMenuItemModel.Caption = Resources.MenuSubFontColorItem;
    contextMenuItemModel.IsChecked = true;
    contextMenuItemModel.Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/linecolor.png"));
    contextMenuItemModel.TagData = new TagDataModel()
    {
      AnnotationMode = mode,
      MenuItemType = ContextMenuItemType.FontColor
    };
    TypedContextMenuItemModel fontColorMenu = contextMenuItemModel;
    foreach (IContextMenuModel contextMenuItem in ToolbarContextMenuHelper.CreateContextMenuItems(mode, ContextMenuItemType.FontColor, action))
      fontColorMenu.Add(contextMenuItem);
    if (addMoreItem)
      fontColorMenu.Add((IContextMenuModel) ToolbarContextMenuHelper.CreateColorMoreItem(mode, ContextMenuItemType.FontColor, action));
    return (IContextMenuModel) fontColorMenu;
  }

  public static IContextMenuModel CreateFontSizeMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action)
  {
    TypedContextMenuItemModel contextMenuItemModel = new TypedContextMenuItemModel(ContextMenuItemType.FontSize);
    contextMenuItemModel.Name = "FontSize";
    contextMenuItemModel.Caption = Resources.MenuSubFontSizeItem;
    contextMenuItemModel.IsChecked = true;
    contextMenuItemModel.Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/linecolor.png"));
    contextMenuItemModel.TagData = new TagDataModel()
    {
      AnnotationMode = mode,
      MenuItemType = ContextMenuItemType.FontSize
    };
    TypedContextMenuItemModel fontSizeMenu = contextMenuItemModel;
    foreach (IContextMenuModel contextMenuItem in ToolbarContextMenuHelper.CreateContextMenuItems(mode, ContextMenuItemType.FontSize, action))
      fontSizeMenu.Add(contextMenuItem);
    return (IContextMenuModel) fontSizeMenu;
  }

  public static ContextMenuItemModel GetDefaultMenuItem(
    AnnotationMode mode,
    TypedContextMenuItemModel menu)
  {
    if (mode == AnnotationMode.None)
      return (ContextMenuItemModel) null;
    if (menu == null)
      return (ContextMenuItemModel) null;
    ContextMenuItemType type = menu.Type;
    object defaultValue = ToolbarContextMenuHelper.GetDefaultValue(mode, type);
    return menu.OfType<ContextMenuItemModel>().FirstOrDefault<ContextMenuItemModel>((Func<ContextMenuItemModel, bool>) (c => ToolbarContextMenuValueEqualityComparer.MenuValueEquals(mode, type, defaultValue, c.TagData?.MenuItemValue)));
  }

  public static IContextMenuModel CreateAddImgMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action)
  {
    StampContextMenuItemModel addImgMenu = new StampContextMenuItemModel();
    addImgMenu.Name = "Insert local image";
    addImgMenu.Caption = Resources.MenuStampSubInsertlocalImageContent;
    addImgMenu.IsChecked = false;
    addImgMenu.Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/addimg.png"));
    addImgMenu.TagData = new TagDataModel()
    {
      AnnotationMode = mode
    };
    addImgMenu.Command = action == null ? (ICommand) null : (ICommand) new RelayCommand<ContextMenuItemModel>(action);
    return (IContextMenuModel) addImgMenu;
  }

  public static IContextMenuModel CreatePresetsMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action)
  {
    StampContextMenuItemModel contextMenuItemModel = new StampContextMenuItemModel();
    contextMenuItemModel.Name = "Presets";
    contextMenuItemModel.Caption = Resources.MenuStampSubPresetsContent;
    contextMenuItemModel.IsChecked = false;
    contextMenuItemModel.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/presets.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/presets.png"));
    contextMenuItemModel.TagData = new TagDataModel()
    {
      AnnotationMode = mode
    };
    StampContextMenuItemModel presetsMenu = contextMenuItemModel;
    foreach (IStampTextModel presetsMenuValue in ToolbarContextMenuHelper.stampPresetsMenuValues)
    {
      PresetsItemContextMenuItemModel contextMenuItemCore2 = ToolbarContextMenuHelper.CreateContextMenuItemCore2<PresetsItemContextMenuItemModel>(mode, ContextMenuItemType.None, presetsMenuValue.TextContent, (object) presetsMenuValue, false, false, action);
      presetsMenu.Add((IContextMenuModel) contextMenuItemCore2);
    }
    return (IContextMenuModel) presetsMenu;
  }

  public static IContextMenuModel CreteStampMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action)
  {
    StampContextMenuItemModel contextMenuItemModel = new StampContextMenuItemModel();
    contextMenuItemModel.Name = "CustomStamp";
    contextMenuItemModel.Caption = Resources.MenuStampSubCustomizeContent;
    contextMenuItemModel.IsChecked = false;
    contextMenuItemModel.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/addsignature.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/addsignature.png"));
    contextMenuItemModel.TagData = new TagDataModel()
    {
      AnnotationMode = mode
    };
    contextMenuItemModel.Command = action == null ? (ICommand) null : (ICommand) new RelayCommand<ContextMenuItemModel>(action);
    contextMenuItemModel.Add((IContextMenuModel) ToolbarContextMenuHelper.CreateContextMenuItemCore2<StampCustomMenuItemModel>(mode, ContextMenuItemType.None, (string) null, (object) null, false, false, action));
    return (IContextMenuModel) contextMenuItemModel;
  }

  public static IContextMenuModel SpeakCurrent(Action<ContextMenuItemModel> action)
  {
    SpeedContextMenuItemModel contextMenuItemModel = new SpeedContextMenuItemModel();
    contextMenuItemModel.Name = "Read Current Page";
    contextMenuItemModel.Caption = Resources.ReadCurrentBtn;
    contextMenuItemModel.IsChecked = false;
    contextMenuItemModel.IsEnabled = false;
    contextMenuItemModel.Command = (ICommand) new RelayCommand((Action) (() =>
    {
      MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
      requiredService.ViewToolbar.ReadButtonModel.IsChecked = true;
      ContextMenuModel contextMenu = (requiredService.ViewToolbar.ReadButtonModel.ChildButtonModel as ToolbarChildCheckableButtonModel).ContextMenu as ContextMenuModel;
      (contextMenu[2] as SpeedContextMenuItemModel).IsEnabled = false;
      (contextMenu[1] as SpeedContextMenuItemModel).IsEnabled = false;
      (contextMenu[0] as SpeedContextMenuItemModel).IsEnabled = false;
      (contextMenu[2] as SpeedContextMenuItemModel).IsChecked = false;
      (contextMenu[1] as SpeedContextMenuItemModel).IsChecked = false;
      (contextMenu[0] as SpeedContextMenuItemModel).IsChecked = true;
      if (requiredService.speechUtils != null)
      {
        requiredService.speechUtils.Activated();
        requiredService.speechUtils.SpeakCurrentPage(requiredService.CurrnetPageIndex - 1);
      }
      else
      {
        requiredService.ViewToolbar.ReadButtonModel.IsChecked = true;
        PdfDocument document = requiredService.Document == null ? (PdfDocument) null : requiredService.Document;
        requiredService.IsReading = true;
        requiredService.speechUtils?.Dispose();
        requiredService.speechUtils = new SpeechUtils(document);
        if (requiredService.speechControl != null)
        {
          requiredService.speechUtils.Rate = requiredService.speechControl.SpeedSli.Value * 2.0 - 10.0;
          requiredService.speechUtils.SpeechVolume = (float) Convert.ToInt32(requiredService.speechControl.VolumeSlider.Value);
          requiredService.speechUtils.Pitch = (double) Convert.ToInt32(requiredService.speechControl.ToneSli.Value - 5.0);
          requiredService.speechUtils.CultureIndex = requiredService.speechControl.CultureListBox.SelectedIndex >= 0 ? requiredService.speechControl.CultureListBox.SelectedIndex : requiredService.speechUtils.GetcultureIndex();
        }
        requiredService.speechUtils.SpeakCurrentPage(requiredService.CurrnetPageIndex - 1);
      }
      GAManager.SendEvent("PDFReader", "Read", "CurrentPage", 1L);
    }));
    return (IContextMenuModel) contextMenuItemModel;
  }

  public static IContextMenuModel SpeakFormCurrent(Action<ContextMenuItemModel> action)
  {
    SpeedContextMenuItemModel contextMenuItemModel = new SpeedContextMenuItemModel();
    contextMenuItemModel.Name = "Read From Current Page";
    contextMenuItemModel.Caption = Resources.ReadFromCurrentBtn;
    contextMenuItemModel.IsChecked = false;
    contextMenuItemModel.IsEnabled = false;
    contextMenuItemModel.Command = (ICommand) new RelayCommand((Action) (() =>
    {
      MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
      requiredService.ViewToolbar.ReadButtonModel.IsChecked = true;
      ContextMenuModel contextMenu = (requiredService.ViewToolbar.ReadButtonModel.ChildButtonModel as ToolbarChildCheckableButtonModel).ContextMenu as ContextMenuModel;
      (contextMenu[2] as SpeedContextMenuItemModel).IsEnabled = false;
      (contextMenu[1] as SpeedContextMenuItemModel).IsEnabled = false;
      (contextMenu[0] as SpeedContextMenuItemModel).IsEnabled = false;
      (contextMenu[2] as SpeedContextMenuItemModel).IsChecked = false;
      (contextMenu[0] as SpeedContextMenuItemModel).IsChecked = false;
      (contextMenu[1] as SpeedContextMenuItemModel).IsChecked = true;
      if (requiredService.speechUtils != null)
      {
        requiredService.speechUtils.Activated();
        requiredService.speechUtils.SpeakPages(requiredService.CurrnetPageIndex - 1, requiredService.Document.Pages.Count - 1);
      }
      else
      {
        requiredService.ViewToolbar.ReadButtonModel.IsChecked = true;
        PdfDocument document = requiredService.Document == null ? (PdfDocument) null : requiredService.Document;
        requiredService.IsReading = true;
        requiredService.speechUtils?.Dispose();
        requiredService.speechUtils = new SpeechUtils(document);
        if (requiredService.speechControl != null)
        {
          requiredService.speechUtils.Rate = requiredService.speechControl.SpeedSli.Value * 2.0 - 10.0;
          requiredService.speechUtils.SpeechVolume = (float) Convert.ToInt32(requiredService.speechControl.VolumeSlider.Value);
          requiredService.speechUtils.Pitch = (double) Convert.ToInt32(requiredService.speechControl.ToneSli.Value - 5.0);
          requiredService.speechUtils.CultureIndex = requiredService.speechControl.CultureListBox.SelectedIndex >= 0 ? requiredService.speechControl.CultureListBox.SelectedIndex : requiredService.speechUtils.GetcultureIndex();
        }
        requiredService.speechUtils.SpeakPages(requiredService.CurrnetPageIndex - 1, requiredService.Document.Pages.Count - 1);
      }
      GAManager.SendEvent("PDFReader", "Read", "FromCurrentPage", 1L);
    }));
    return (IContextMenuModel) contextMenuItemModel;
  }

  public static IContextMenuModel SpeakAll(Action<ContextMenuItemModel> action)
  {
    SpeedContextMenuItemModel contextMenuItemModel = new SpeedContextMenuItemModel();
    contextMenuItemModel.Name = "Read All Pages";
    contextMenuItemModel.Caption = Resources.ReadAllBtn;
    contextMenuItemModel.IsChecked = false;
    contextMenuItemModel.IsEnabled = false;
    contextMenuItemModel.Command = (ICommand) new RelayCommand((Action) (() =>
    {
      MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
      requiredService.ViewToolbar.ReadButtonModel.IsChecked = true;
      ContextMenuModel contextMenu = (requiredService.ViewToolbar.ReadButtonModel.ChildButtonModel as ToolbarChildCheckableButtonModel).ContextMenu as ContextMenuModel;
      (contextMenu[2] as SpeedContextMenuItemModel).IsEnabled = false;
      (contextMenu[1] as SpeedContextMenuItemModel).IsEnabled = false;
      (contextMenu[0] as SpeedContextMenuItemModel).IsEnabled = false;
      (contextMenu[0] as SpeedContextMenuItemModel).IsChecked = false;
      (contextMenu[1] as SpeedContextMenuItemModel).IsChecked = false;
      (contextMenu[2] as SpeedContextMenuItemModel).IsChecked = true;
      if (requiredService.speechUtils != null)
      {
        requiredService.speechUtils.Activated();
        requiredService.speechUtils.SpeakPages(0, requiredService.Document.Pages.Count - 1);
      }
      else
      {
        if (requiredService.Document == null)
          return;
        requiredService.ViewToolbar.ReadButtonModel.IsChecked = true;
        PdfDocument document = requiredService.Document == null ? (PdfDocument) null : requiredService.Document;
        requiredService.IsReading = true;
        requiredService.speechUtils?.Dispose();
        requiredService.speechUtils = new SpeechUtils(document);
        if (requiredService.speechControl != null)
        {
          requiredService.speechUtils.Rate = requiredService.speechControl.SpeedSli.Value * 2.0 - 10.0;
          requiredService.speechUtils.SpeechVolume = (float) Convert.ToInt32(requiredService.speechControl.VolumeSlider.Value);
          requiredService.speechUtils.Pitch = (double) Convert.ToInt32(requiredService.speechControl.ToneSli.Value - 5.0);
          requiredService.speechUtils.CultureIndex = requiredService.speechControl.CultureListBox.SelectedIndex >= 0 ? requiredService.speechControl.CultureListBox.SelectedIndex : requiredService.speechUtils.GetcultureIndex();
        }
        requiredService.speechUtils.SpeakPages(0, requiredService.Document.Pages.Count - 1);
      }
      GAManager.SendEvent("PDFReader", "Read", "AllPages", 1L);
    }));
    return (IContextMenuModel) contextMenuItemModel;
  }

  public static IContextMenuModel SpeechToolbarMenu(Action<ContextMenuItemModel> action)
  {
    SpeechToolContextMenuItemModel contextMenuItemModel = new SpeechToolContextMenuItemModel();
    contextMenuItemModel.Name = "ToolBar";
    contextMenuItemModel.Caption = Resources.ReadToolBarBtn;
    contextMenuItemModel.IsEnabled = true;
    contextMenuItemModel.IsChecked = false;
    contextMenuItemModel.Command = (ICommand) new RelayCommand((Action) (() =>
    {
      MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
      bool flag = false;
      foreach (Window window in Application.Current.Windows)
      {
        if (window.GetType() == typeof (SpeechControl))
          flag = true;
      }
      if (flag)
      {
        requiredService.speechControl.Close();
      }
      else
      {
        requiredService.speechControl = new SpeechControl();
        requiredService.speechControl.Owner = App.Current.MainWindow;
        if (requiredService.speechControl.Owner.WindowState == WindowState.Normal)
        {
          requiredService.speechControl.Top = requiredService.speechControl.Owner.Top + 152.0;
          requiredService.speechControl.Left = requiredService.speechControl.Owner.Left + requiredService.speechControl.Owner.ActualWidth - 520.0;
        }
        else if (requiredService.speechControl.Owner.WindowState == WindowState.Maximized)
        {
          requiredService.speechControl.Top = 152.0;
          requiredService.speechControl.Left = requiredService.speechControl.Owner.ActualWidth - 520.0;
        }
        requiredService.speechControl.Show();
      }
    }));
    return (IContextMenuModel) contextMenuItemModel;
  }

  public static IContextMenuModel CreateCustomMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action)
  {
    StampContextMenuItemModel contextMenuItemModel = new StampContextMenuItemModel();
    contextMenuItemModel.IsChecked = false;
    contextMenuItemModel.Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/add.png"));
    contextMenuItemModel.TagData = new TagDataModel()
    {
      AnnotationMode = mode
    };
    contextMenuItemModel.Command = action == null ? (ICommand) null : (ICommand) new RelayCommand<ContextMenuItemModel>(action);
    StampContextMenuItemModel customMenu = contextMenuItemModel;
    if (mode == AnnotationMode.Stamp)
    {
      customMenu.Name = "Customize stamp";
      customMenu.Caption = Resources.MenuStampSubCustomizeContent;
    }
    else
    {
      customMenu.Name = "Customize";
      customMenu.Caption = "Customize";
    }
    return (IContextMenuModel) customMenu;
  }

  public static IContextMenuModel CreateShareEmailMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action)
  {
    StampContextMenuItemModel shareEmailMenu = new StampContextMenuItemModel();
    shareEmailMenu.Name = "Email";
    shareEmailMenu.Caption = Resources.MenuShareEmailContent;
    shareEmailMenu.IsChecked = false;
    shareEmailMenu.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/mail.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/mail.png"));
    shareEmailMenu.TagData = new TagDataModel()
    {
      AnnotationMode = mode
    };
    shareEmailMenu.Command = action == null ? (ICommand) null : (ICommand) new RelayCommand<ContextMenuItemModel>(action);
    return (IContextMenuModel) shareEmailMenu;
  }

  public static IContextMenuModel CreateShareMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action)
  {
    StampContextMenuItemModel shareMenu = new StampContextMenuItemModel();
    shareMenu.Name = "Share";
    shareMenu.Caption = Resources.MenuShareShareContent;
    shareMenu.IsChecked = false;
    shareMenu.Icon = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/share.png"));
    shareMenu.TagData = new TagDataModel()
    {
      AnnotationMode = mode
    };
    shareMenu.Command = action == null ? (ICommand) null : (ICommand) new RelayCommand<ContextMenuItemModel>(action);
    return (IContextMenuModel) shareMenu;
  }

  public static IContextMenuModel CreateAddLinkMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action)
  {
    StampContextMenuItemModel addLinkMenu = new StampContextMenuItemModel();
    addLinkMenu.Name = "Create/Edit Link";
    addLinkMenu.Caption = Resources.LinkCreateBtn;
    addLinkMenu.IsChecked = false;
    addLinkMenu.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/LinkCE.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/LinkCE.png"));
    addLinkMenu.TagData = new TagDataModel()
    {
      AnnotationMode = mode
    };
    addLinkMenu.Command = action == null ? (ICommand) null : (ICommand) new RelayCommand<ContextMenuItemModel>(action);
    addLinkMenu.HotKeyInvokeWhen = "Editor_CreateOrEditLink";
    return (IContextMenuModel) addLinkMenu;
  }

  public static IContextMenuModel CreateDeleteAllLinkMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action)
  {
    StampContextMenuItemModel deleteAllLinkMenu = new StampContextMenuItemModel();
    deleteAllLinkMenu.Name = "DeleteAllLink";
    deleteAllLinkMenu.Caption = Resources.LinkDeleteAllBtn;
    deleteAllLinkMenu.IsChecked = false;
    deleteAllLinkMenu.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/deleteLink.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/deleteLink.png"));
    deleteAllLinkMenu.TagData = new TagDataModel()
    {
      AnnotationMode = mode
    };
    deleteAllLinkMenu.Command = action == null ? (ICommand) null : (ICommand) new RelayCommand<ContextMenuItemModel>(action);
    deleteAllLinkMenu.HotKeyInvokeWhen = "Editor_DeleteLink";
    return (IContextMenuModel) deleteAllLinkMenu;
  }

  public static IContextMenuModel CreateShareFileMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action)
  {
    StampContextMenuItemModel shareFileMenu = new StampContextMenuItemModel();
    shareFileMenu.Name = "Sharefile";
    shareFileMenu.Caption = Resources.MenuShareSendFileContent;
    shareFileMenu.IsChecked = false;
    shareFileMenu.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/file.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/file.png"));
    shareFileMenu.TagData = new TagDataModel()
    {
      AnnotationMode = mode
    };
    shareFileMenu.Command = action == null ? (ICommand) null : (ICommand) new RelayCommand<ContextMenuItemModel>(action);
    return (IContextMenuModel) shareFileMenu;
  }

  public static IContextMenuModel CreateSignatureMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action)
  {
    return (IContextMenuModel) ToolbarContextMenuHelper.CreateSignatureItem(mode, ContextMenuItemType.None, action);
  }

  public static IContextMenuModel CreateAddWatermarkMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action)
  {
    StampContextMenuItemModel addWatermarkMenu = new StampContextMenuItemModel();
    addWatermarkMenu.Name = "Create Watermark";
    addWatermarkMenu.Caption = Resources.MenuWatermarkSubCreateContent;
    addWatermarkMenu.IsChecked = false;
    addWatermarkMenu.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/addwatermark.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/addwatermark.png"));
    addWatermarkMenu.TagData = new TagDataModel()
    {
      AnnotationMode = mode
    };
    addWatermarkMenu.Command = action == null ? (ICommand) null : (ICommand) new RelayCommand<ContextMenuItemModel>(action);
    addWatermarkMenu.HotKeyInvokeWhen = "Editor_CreateWatermark";
    return (IContextMenuModel) addWatermarkMenu;
  }

  public static IContextMenuModel CreateDeleteCurrentPageWatermarkMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action)
  {
    StampContextMenuItemModel pageWatermarkMenu = new StampContextMenuItemModel();
    pageWatermarkMenu.Name = "DeleteCurrentPageWatermark";
    pageWatermarkMenu.Caption = "Delete current page watermark";
    pageWatermarkMenu.IsChecked = false;
    pageWatermarkMenu.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/addwatermark.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/addwatermark.png"));
    pageWatermarkMenu.TagData = new TagDataModel()
    {
      AnnotationMode = mode
    };
    pageWatermarkMenu.Command = action == null ? (ICommand) null : (ICommand) new RelayCommand<ContextMenuItemModel>(action);
    return (IContextMenuModel) pageWatermarkMenu;
  }

  public static IContextMenuModel CreateDeleteAllWatermarkMenu(
    AnnotationMode mode,
    Action<ContextMenuItemModel> action)
  {
    StampContextMenuItemModel allWatermarkMenu = new StampContextMenuItemModel();
    allWatermarkMenu.Name = "DeleteAllWatermark";
    allWatermarkMenu.Caption = Resources.MenuWatermarkSubDeleteContent;
    allWatermarkMenu.IsChecked = false;
    allWatermarkMenu.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/addwatermark.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/addwatermark.png"));
    allWatermarkMenu.TagData = new TagDataModel()
    {
      AnnotationMode = mode
    };
    allWatermarkMenu.Command = action == null ? (ICommand) null : (ICommand) new RelayCommand<ContextMenuItemModel>(action);
    allWatermarkMenu.HotKeyInvokeWhen = "Editor_DeleteWatermark";
    return (IContextMenuModel) allWatermarkMenu;
  }

  public static SelectableContextMenuItemModel CreateConverterContextMenu(
    Action<ContextMenuItemModel> action)
  {
    BitmapImage _icon1 = new BitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/wordmenu.png"));
    BitmapImage _icon2 = new BitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/excelmenu.png"));
    BitmapImage _icon3 = new BitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/pptmenu.png"));
    BitmapImage _icon4 = new BitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/imagemenu.png"));
    BitmapImage _icon5 = new BitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/Rtfmenu.png"));
    BitmapImage _icon6 = new BitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/txtmenu.png"));
    BitmapImage _icon7 = new BitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/Htmlmenu.png"));
    BitmapImage _icon8 = new BitmapImage(new Uri("pack://application:,,,/Style/Resources/converter/Xmlmenu.png"));
    SelectableContextMenuItemModel converterContextMenu = new SelectableContextMenuItemModel();
    converterContextMenu.Add(CreateItem("PDFtoWord", Resources.MenuConvertPdfToWordContent, _icon1, action));
    converterContextMenu.Add(CreateItem("PDFtoExcel", Resources.MenuConvertPdfToExcelContent, _icon2, action));
    converterContextMenu.Add(CreateItem("PDFtoPPT", Resources.MenuConvertPdfToPPTContent, _icon3, action));
    converterContextMenu.Add(CreateItem("PDFtoImage", Resources.MenuConvertPdfToImageContent, _icon4, action));
    converterContextMenu.Add(CreateItem("PDFtoJpeg", Resources.MenuConvertPdfToJpegContent, _icon4, action));
    converterContextMenu.Add(CreateItem("PDFtoText", Resources.MenuConvertPdfToTxtContent, _icon6, action));
    converterContextMenu.Add(CreateItem("PDFtoHtml", Resources.MenuConvertPdfToHtmlContent, _icon7, action));
    converterContextMenu.Add(CreateItem("PDFtoRtf", Resources.MenuConvertPdfToRtfContent, _icon5, action));
    converterContextMenu.Add(CreateItem("PDFtoXml", Resources.MenuConvertPdfToXmlContent, _icon8, action));
    converterContextMenu.Add((IContextMenuModel) new ContextMenuSeparator());
    converterContextMenu.Add(CreateItem("WordtoPDF", Resources.WinConvertToolBtnWordToPDF, _icon1, action));
    converterContextMenu.Add(CreateItem("ExceltoPDF", Resources.WinConvertToolBtnExcelToPDFText, _icon2, action));
    converterContextMenu.Add(CreateItem("ImagetoPDF", Resources.WinConvertToolBtnImageToPDFText, _icon4, action));
    converterContextMenu.Add(CreateItem("PPTtoPDF", Resources.WinConvertToolBtnPPTToPDFText, _icon3, action));
    converterContextMenu.Add(CreateItem("RtftoPDF", Resources.WinConvertToolBtnRTFToPDFText, _icon5, action));
    converterContextMenu.Add(CreateItem("TxttoPDF", Resources.WinConvertToolBtnTXTToPDFText, _icon6, action));
    return converterContextMenu;

    static IContextMenuModel CreateItem(
      string _name,
      string _caption,
      BitmapImage _icon,
      Action<ContextMenuItemModel> _action)
    {
      ConvertContextMenuItemModel contextMenuItemModel = new ConvertContextMenuItemModel();
      contextMenuItemModel.Name = _name;
      contextMenuItemModel.Caption = _caption;
      contextMenuItemModel.IsChecked = false;
      contextMenuItemModel.Icon = (ImageSource) _icon;
      contextMenuItemModel.TagData = new TagDataModel()
      {
        AnnotationMode = AnnotationMode.None
      };
      contextMenuItemModel.Command = _action == null ? (ICommand) null : (ICommand) new RelayCommand<ContextMenuItemModel>(_action);
      return (IContextMenuModel) contextMenuItemModel;
    }
  }

  public static SelectableContextMenuItemModel CreateBackgroundContextMenu(
    string selectedName,
    Action<ContextMenuItemModel> action)
  {
    SelectableContextMenuItemModel backgroundContextMenu = new SelectableContextMenuItemModel();
    if (!ToolbarContextMenuHelper.viewerBackgroundColorDict.ContainsKey(selectedName))
      selectedName = ToolbarContextMenuHelper.viewerBackgroundColorValues[0].Name;
    foreach (BackgroundColorSetting backgroundColorValue in ToolbarContextMenuHelper.viewerBackgroundColorValues)
    {
      SelectableContextMenuItemModel contextMenuItemModel1 = backgroundContextMenu;
      BackgroundContextMenuItemModel contextMenuItemModel2 = new BackgroundContextMenuItemModel();
      contextMenuItemModel2.Name = backgroundColorValue.Name;
      contextMenuItemModel2.Caption = backgroundColorValue.DisplayName;
      contextMenuItemModel2.IsChecked = backgroundColorValue.Name == selectedName;
      contextMenuItemModel2.Icon = (ImageSource) null;
      contextMenuItemModel2.TagData = new TagDataModel()
      {
        MenuItemValue = (object) backgroundColorValue
      };
      contextMenuItemModel2.Command = action == null ? (ICommand) null : (ICommand) new RelayCommand<ContextMenuItemModel>(action);
      contextMenuItemModel1.Add((IContextMenuModel) contextMenuItemModel2);
    }
    return backgroundContextMenu;
  }

  public static SelectableContextMenuItemModel CreateAutoScrollContextMenu(
    int selectedValue,
    Action<ContextMenuItemModel> action)
  {
    SelectableContextMenuItemModel model = new SelectableContextMenuItemModel();
    if (selectedValue < -4 || selectedValue == 0 || selectedValue > 4)
      selectedValue = 1;
    for (int i = -4; i < 0; ++i)
      AddItem(i);
    for (int i = 1; i <= 4; ++i)
      AddItem(i);
    return model;

    void AddItem(int i)
    {
      model.Add((IContextMenuModel) new ContextMenuItemModel()
      {
        Name = $"{i}",
        Caption = $"{i}",
        IsChecked = (i == selectedValue),
        Icon = (ImageSource) null,
        TagData = new TagDataModel()
        {
          MenuItemValue = (object) i
        },
        Command = (action == null ? (ICommand) null : (ICommand) new RelayCommand<ContextMenuItemModel>(action))
      });
    }
  }

  public static object GetDefaultValue(AnnotationMode mode, ContextMenuItemType type)
  {
    System.Collections.Generic.IReadOnlyList<ToolbarContextMenuHelper.MenuValueProvider> values = ToolbarContextMenuHelper.GetValues(type);
    if (values == null || values.Count == 0)
      return (object) null;
    return values.FirstOrDefault<ToolbarContextMenuHelper.MenuValueProvider>((Func<ToolbarContextMenuHelper.MenuValueProvider, bool>) (c => c.IsDefaultValue(mode)))?.Value;
  }

  public static ContextMenuItemModel CreateContextMenuItem(
    AnnotationMode mode,
    ContextMenuItemType type,
    object value,
    bool isTransient,
    Action<ContextMenuItemModel> action)
  {
    string caption;
    object result;
    return ToolbarContextMenuHelper.TryParseMenuValue(mode, type, value, out caption, out result) ? ToolbarContextMenuHelper.CreateContextMenuItemCore(mode, type, caption, result, false, isTransient, action) : (ContextMenuItemModel) null;
  }

  private static ContextMenuItemModel CreateContextMenuItemCore(
    AnnotationMode mode,
    ContextMenuItemType type,
    string caption,
    object value,
    bool isChecked,
    bool isTransient,
    Action<ContextMenuItemModel> action)
  {
    return ToolbarContextMenuHelper.CreateContextMenuItemCore2<ContextMenuItemModel>(mode, type, caption, value, isChecked, isTransient, action);
  }

  private static T CreateContextMenuItemCore2<T>(
    AnnotationMode mode,
    ContextMenuItemType type,
    string caption,
    object value,
    bool isChecked,
    bool isTransient,
    Action<ContextMenuItemModel> action)
    where T : ContextMenuItemModel, new()
  {
    T contextMenuItemCore2 = new T();
    contextMenuItemCore2.Name = "";
    contextMenuItemCore2.Caption = caption;
    contextMenuItemCore2.IsChecked = isChecked;
    contextMenuItemCore2.TagData = new TagDataModel(isTransient)
    {
      MenuItemType = type,
      MenuItemValue = value,
      AnnotationMode = mode
    };
    contextMenuItemCore2.Command = action == null ? (ICommand) null : (ICommand) new RelayCommand<ContextMenuItemModel>(action);
    return contextMenuItemCore2;
  }

  public static IEnumerable<IContextMenuModel> CreateContextMenuItems(
    AnnotationMode mode,
    ContextMenuItemType type,
    Action<ContextMenuItemModel> action)
  {
    System.Collections.Generic.IReadOnlyList<ToolbarContextMenuHelper.MenuValueProvider> values = ToolbarContextMenuHelper.GetValues(type);
    return values == null || values.Count == 0 ? Enumerable.Empty<IContextMenuModel>() : (IEnumerable<IContextMenuModel>) values.Where<ToolbarContextMenuHelper.MenuValueProvider>((Func<ToolbarContextMenuHelper.MenuValueProvider, bool>) (c => c.IsValueOwner(mode))).Select<ToolbarContextMenuHelper.MenuValueProvider, ContextMenuItemModel>((Func<ToolbarContextMenuHelper.MenuValueProvider, ContextMenuItemModel>) (c => ToolbarContextMenuHelper.CreateContextMenuItemCore(mode, type, c.Caption, c.Value, c.IsDefaultValue(mode), false, action)));
  }

  public static ContextMenuItemModel CreateColorMoreItem(
    AnnotationMode mode,
    ContextMenuItemType type,
    Action<ContextMenuItemModel> action)
  {
    ColorMoreItemContextMenuItemModel contextMenuItemCore2 = ToolbarContextMenuHelper.CreateContextMenuItemCore2<ColorMoreItemContextMenuItemModel>(mode, type, "More", (object) "More", false, false, action);
    contextMenuItemCore2.Name = "More";
    contextMenuItemCore2.RecentColorsKey = $"{mode}_{type}";
    if (type == ContextMenuItemType.StrokeColor || type == ContextMenuItemType.FillColor || type == ContextMenuItemType.FontColor)
    {
      ContextMenuItemModel contextMenuItemCore = ToolbarContextMenuHelper.CreateContextMenuItemCore(mode, type, "ColorPicker", (object) "ColorPicker", false, false, (Action<ContextMenuItemModel>) null);
      contextMenuItemCore.Name = "ColorPicker";
      contextMenuItemCore2.Add((IContextMenuModel) contextMenuItemCore);
    }
    return (ContextMenuItemModel) contextMenuItemCore2;
  }

  private static ContextMenuItemModel CreateSignatureItem(
    AnnotationMode mode,
    ContextMenuItemType type,
    Action<ContextMenuItemModel> action)
  {
    ContextMenuItemModel contextMenuItemCore = ToolbarContextMenuHelper.CreateContextMenuItemCore(mode, type, "SignaturePicker", (object) "SignaturePicker", false, false, (Action<ContextMenuItemModel>) null);
    contextMenuItemCore.Name = "SignaturePicker";
    return contextMenuItemCore;
  }

  private static System.Collections.Generic.IReadOnlyList<ToolbarContextMenuHelper.MenuValueProvider> GetValues(
    ContextMenuItemType type)
  {
    ToolbarContextMenuHelper.MenuValueProvider[] menuValueProviderArray = (ToolbarContextMenuHelper.MenuValueProvider[]) null;
    switch (type)
    {
      case ContextMenuItemType.StrokeColor:
        menuValueProviderArray = ToolbarContextMenuHelper.strokeColorMenuValues;
        break;
      case ContextMenuItemType.FillColor:
        menuValueProviderArray = ToolbarContextMenuHelper.fillColorMenuValues;
        break;
      case ContextMenuItemType.StrokeThickness:
        menuValueProviderArray = ToolbarContextMenuHelper.strokeThicknessMenuValues;
        break;
      case ContextMenuItemType.FontSize:
        menuValueProviderArray = ToolbarContextMenuHelper.fontSizeMenuValues;
        break;
      case ContextMenuItemType.FontColor:
        menuValueProviderArray = ToolbarContextMenuHelper.fontColorMenuValues;
        break;
    }
    return (System.Collections.Generic.IReadOnlyList<ToolbarContextMenuHelper.MenuValueProvider>) menuValueProviderArray ?? (System.Collections.Generic.IReadOnlyList<ToolbarContextMenuHelper.MenuValueProvider>) Array.Empty<ToolbarContextMenuHelper.MenuValueProvider>();
  }

  public static List<StampTextModel> ReadStamp()
  {
    string str = Path.Combine(AppDataHelper.LocalCacheFolder, "Config");
    if (!Directory.Exists(str))
      Directory.CreateDirectory(str);
    string path = Path.Combine(str, "Stamp.json");
    if (!File.Exists(path))
      return (List<StampTextModel>) null;
    JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
    {
      TypeNameHandling = TypeNameHandling.Auto,
      NullValueHandling = NullValueHandling.Ignore
    };
    using (StreamReader streamReader = new StreamReader(path))
      return JsonConvert.DeserializeObject<List<StampTextModel>>(streamReader.ReadToEnd(), new JsonSerializerSettings()
      {
        TypeNameHandling = TypeNameHandling.Auto
      });
  }

  private static void InitAllAnnotationMode()
  {
    ToolbarContextMenuHelper.allAnnotationMode = EnumHelper<AnnotationMode>.AllValues.Where<AnnotationMode>((Func<AnnotationMode, bool>) (c => c != AnnotationMode.None)).ToArray<AnnotationMode>();
  }

  private static void InitStrokeThicknessMenuValues()
  {
    ToolbarContextMenuHelper.strokeThicknessMenuValues = Enumerable.Range(1, 12).Select<int, ToolbarContextMenuHelper.MenuValueProvider>((Func<int, ToolbarContextMenuHelper.MenuValueProvider>) (c =>
    {
      AnnotationMode[] allAnnotationMode = c == 1 ? ToolbarContextMenuHelper.allAnnotationMode : (AnnotationMode[]) null;
      return new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeThickness, (object) (float) c, $"{c} pt", allAnnotationMode);
    })).ToArray<ToolbarContextMenuHelper.MenuValueProvider>();
  }

  private static void InitStrokeColorMenuValues()
  {
    ToolbarContextMenuHelper.strokeColorMenuValues = new ToolbarContextMenuHelper.MenuValueProvider[9]
    {
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FFFFFFFF", "", new AnnotationMode[1]
      {
        AnnotationMode.Shape
      }),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FF000000", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FFFB302F", "", ((IEnumerable<AnnotationMode>) ToolbarContextMenuHelper.allAnnotationMode).Where<AnnotationMode>((Func<AnnotationMode, bool>) (c => c != AnnotationMode.Highlight && c != AnnotationMode.Text)).ToArray<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FFFD9927", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FFFAFF00", "", new AnnotationMode[2]
      {
        AnnotationMode.Highlight,
        AnnotationMode.HighlightArea
      }),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FFA5DE50", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FF43D9EF", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FF52AAEC", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FF9573E4", "", Array.Empty<AnnotationMode>())
    };
  }

  private static void InitFillColorMenuValues()
  {
    AnnotationMode[] transparentOwner = new AnnotationMode[2]
    {
      AnnotationMode.Ellipse,
      AnnotationMode.Shape
    };
    AnnotationMode[] array = ((IEnumerable<AnnotationMode>) ToolbarContextMenuHelper.allAnnotationMode).Where<AnnotationMode>((Func<AnnotationMode, bool>) (c => !((IEnumerable<AnnotationMode>) transparentOwner).Contains<AnnotationMode>(c) && c != AnnotationMode.TextBox && c != AnnotationMode.Text)).ToArray<AnnotationMode>();
    ToolbarContextMenuHelper.fillColorMenuValues = new ToolbarContextMenuHelper.MenuValueProvider[10]
    {
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#00FFFFFF", "", transparentOwner, transparentOwner),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FFFFFFFF", "", new AnnotationMode[1]
      {
        AnnotationMode.TextBox
      }),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FF000000", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FFFB302F", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FFFD9927", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FFFAFF00", "", array),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FFA5DE50", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FF43D9EF", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FF52AAEC", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeColor, (object) "#FF9573E4", "", Array.Empty<AnnotationMode>())
    };
  }

  private static void InitFontColorMenuValues()
  {
    ToolbarContextMenuHelper.fontColorMenuValues = new ToolbarContextMenuHelper.MenuValueProvider[9]
    {
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.FontColor, (object) "#FFFFFFFF", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.FontColor, (object) "#FF000000", "", new AnnotationMode[2]
      {
        AnnotationMode.TextBox,
        AnnotationMode.Text
      }),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.FontColor, (object) "#FFFB302F", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.FontColor, (object) "#FFFD9927", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.FontColor, (object) "#FFFAFF00", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.FontColor, (object) "#FFA5DE50", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.FontColor, (object) "#FF43D9EF", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.FontColor, (object) "#FF52AAEC", "", Array.Empty<AnnotationMode>()),
      new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.FontColor, (object) "#FF9573E4", "", Array.Empty<AnnotationMode>())
    };
  }

  private static void InitFontSizeMenuValue()
  {
    AnnotationMode[] annots = new AnnotationMode[2]
    {
      AnnotationMode.TextBox,
      AnnotationMode.Text
    };
    ToolbarContextMenuHelper.fontSizeMenuValues = ((IEnumerable<int>) new int[7]
    {
      8,
      10,
      12,
      14,
      18,
      24,
      36
    }).Select<int, ToolbarContextMenuHelper.MenuValueProvider>((Func<int, ToolbarContextMenuHelper.MenuValueProvider>) (c =>
    {
      AnnotationMode[] annotationModeArray = c == 12 ? annots : (AnnotationMode[]) null;
      return new ToolbarContextMenuHelper.MenuValueProvider(ContextMenuItemType.StrokeThickness, (object) (float) c, $"{c} pt", annotationModeArray);
    })).ToArray<ToolbarContextMenuHelper.MenuValueProvider>();
  }

  private static void InitStampPresetsMenuValues()
  {
    ToolbarContextMenuHelper.stampPresetsMenuValues = new IStampTextModel[12]
    {
      (IStampTextModel) new PresetStampTextModel(StampIconNames.Approved, "#20C48F", "Approved"),
      (IStampTextModel) new PresetStampTextModel(StampIconNames.Final, "#20C48F", "Approved"),
      (IStampTextModel) new PresetStampTextModel(StampIconNames.ForPublicRelease, "#20C48F", "Approved"),
      (IStampTextModel) new PresetStampTextModel(StampIconNames.Draft, "#298FEE", "Draft"),
      (IStampTextModel) new PresetStampTextModel(StampIconNames.AsIs, "#298FEE", "Draft"),
      (IStampTextModel) new PresetStampTextModel(StampIconNames.Experimental, "#298FEE", "Draft"),
      (IStampTextModel) new PresetStampTextModel(StampIconNames.NotApproved, "#FF6932", "NotApproved"),
      (IStampTextModel) new PresetStampTextModel(StampIconNames.Expired, "#FF6932", "NotApproved"),
      (IStampTextModel) new PresetStampTextModel(StampIconNames.NotForPublicRelease, "#FF6932", "NotApproved"),
      (IStampTextModel) new PresetStampTextModel(StampIconNames.Confidential, "#FF6932", "Confidential"),
      (IStampTextModel) new PresetStampTextModel(StampIconNames.Sold, "#FF6932", "Confidential"),
      (IStampTextModel) new PresetStampTextModel(StampIconNames.TopSecret, "#FF6932", "Confidential")
    };
    StampUtil.InitStandardIconNameContent(ToolbarContextMenuHelper.stampPresetsMenuValues.OfType<PresetStampTextModel>().ToDictionary<PresetStampTextModel, StampIconNames, string>((Func<PresetStampTextModel, StampIconNames>) (c => c.IconName), (Func<PresetStampTextModel, string>) (c => ToolbarContextMenuHelper.GetPresetStampTextContext(c.IconName))));
  }

  private static void InitViewerBackgroundColorValues()
  {
    ToolbarContextMenuHelper.viewerBackgroundColorValues = new BackgroundColorSetting[5]
    {
      new BackgroundColorSetting("Default", "", Resources.WinViewToolBackgroundDefaultText, App.Current.GetCurrentActualAppTheme() == "Dark" ? "#444444" : "#E2E2E2", "#00FFFFFF"),
      new BackgroundColorSetting("DayMode", "", Resources.WinViewToolBackgroundDayModeText, "#FCFCFC", "#00FFFFFF"),
      new BackgroundColorSetting("NightMode", "", Resources.WinViewToolBackgroundNightModeText, "#CECECE", "#400F0F0F"),
      new BackgroundColorSetting("EyeProtectionMode", "", Resources.WinViewToolBackgroundEyeProtectionModeText, "#D2E2C8", "#404B7430"),
      new BackgroundColorSetting("YellowBackground", "", Resources.WinViewToolBackgroundYellowBackgroundText, "#E4DDC4", "#40775F13")
    };
    ToolbarContextMenuHelper.viewerBackgroundColorDict = (IReadOnlyDictionary<string, BackgroundColorSetting>) ((IEnumerable<BackgroundColorSetting>) ToolbarContextMenuHelper.viewerBackgroundColorValues).ToDictionary<BackgroundColorSetting, string, BackgroundColorSetting>((Func<BackgroundColorSetting, string>) (c => c.Name), (Func<BackgroundColorSetting, BackgroundColorSetting>) (c => c));
  }

  public static string GetPresetStampTextContext(StampIconNames stampIconName)
  {
    switch (stampIconName)
    {
      case StampIconNames.Draft:
        return Resources.MenuStampPresetsDraftItemName;
      case StampIconNames.Approved:
        return Resources.MenuStampPresetsApprovedItemName;
      case StampIconNames.Experimental:
        return Resources.MenuStampPresetsExperimentalItemName;
      case StampIconNames.NotApproved:
        return Resources.MenuStampPresetsNotApprovedItemName;
      case StampIconNames.AsIs:
        return Resources.MenuStampPresetsAsIsItemName;
      case StampIconNames.Expired:
        return Resources.MenuStampPresetsExpiredItemName;
      case StampIconNames.NotForPublicRelease:
        return Resources.MenuStampPresetsNotForPublicReleaseItemName;
      case StampIconNames.Confidential:
        return Resources.MenuStampPresetsConfidentialItemName;
      case StampIconNames.Final:
        return Resources.MenuStampPresetsFinalItemName;
      case StampIconNames.Sold:
        return Resources.MenuStampPresetsSoldItemName;
      case StampIconNames.TopSecret:
        return Resources.MenuStampPresetsTopSecretItemName;
      case StampIconNames.ForPublicRelease:
        return Resources.MenuStampPresetsNotForPublicReleaseItemName;
      default:
        return string.Empty;
    }
  }

  public static bool TryParseMenuValue(
    AnnotationMode mode,
    ContextMenuItemType type,
    object value,
    out string caption,
    out object result)
  {
    caption = (string) null;
    result = (object) null;
    if (value != null)
    {
      switch (type)
      {
        case ContextMenuItemType.None:
          break;
        case ContextMenuItemType.StrokeColor:
        case ContextMenuItemType.FillColor:
        case ContextMenuItemType.FontColor:
          return ToolbarContextMenuHelper.TryParseMenuColorValue(mode, type, value, out caption, out result);
        case ContextMenuItemType.StrokeThickness:
        case ContextMenuItemType.FontSize:
          return ToolbarContextMenuHelper.TryParseMenuThicknessValue(mode, type, value, out caption, out result);
        default:
          return false;
      }
    }
    return false;
  }

  private static bool TryParseMenuColorValue(
    AnnotationMode mode,
    ContextMenuItemType type,
    object value,
    out string caption,
    out object result)
  {
    caption = (string) null;
    result = (object) null;
    switch (value)
    {
      case FS_COLOR fsColor:
        caption = "";
        if (fsColor.A == 0)
          result = (object) "#00FFFFFF";
        else
          result = (object) $"#{fsColor.A:X2}{fsColor.R:X2}{fsColor.G:X2}{fsColor.B:X2}";
        return true;
      case string str:
        try
        {
          Color color = (Color) ColorConverter.ConvertFromString(str);
          caption = "";
          result = (object) $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
          return true;
        }
        catch
        {
          break;
        }
    }
    return false;
  }

  private static bool TryParseMenuThicknessValue(
    AnnotationMode mode,
    ContextMenuItemType type,
    object value,
    out string caption,
    out object result)
  {
    caption = (string) null;
    result = (object) null;
    try
    {
      float single = Convert.ToSingle(value);
      caption = $"{single} pt";
      result = (object) single;
      return true;
    }
    catch
    {
    }
    return false;
  }

  private class MenuValueProvider
  {
    private ImmutableHashSet<AnnotationMode> valueOwner;
    private ImmutableHashSet<AnnotationMode> defaultValueOwner;

    public MenuValueProvider(
      ContextMenuItemType type,
      object value,
      string caption,
      params AnnotationMode[] defaultValueOwner)
      : this(type, value, caption, (AnnotationMode[]) null, defaultValueOwner)
    {
    }

    public MenuValueProvider(
      ContextMenuItemType type,
      object value,
      string caption,
      AnnotationMode[] valueOwner,
      params AnnotationMode[] defaultValueOwner)
    {
      this.Type = type;
      this.Value = value;
      this.Caption = caption;
      if (valueOwner != null && valueOwner.Length != 0)
        this.valueOwner = ((IEnumerable<AnnotationMode>) valueOwner).Distinct<AnnotationMode>().ToImmutableHashSet<AnnotationMode>();
      if (defaultValueOwner == null || defaultValueOwner.Length == 0)
        return;
      this.defaultValueOwner = ((IEnumerable<AnnotationMode>) defaultValueOwner).Distinct<AnnotationMode>().ToImmutableHashSet<AnnotationMode>();
    }

    public ContextMenuItemType Type { get; }

    public object Value { get; }

    public string Caption { get; }

    public bool IsValueOwner(AnnotationMode mode)
    {
      return this.valueOwner == null || this.valueOwner.Contains(mode);
    }

    public bool IsDefaultValue(AnnotationMode mode)
    {
      return this.defaultValueOwner != null && this.defaultValueOwner.Contains(mode);
    }
  }
}
