// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PdfViewerDecorators.SelectTextContextMenuHolder
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using Microsoft.Toolkit.Mvvm.Input;
using Patagames.Pdf;
using Patagames.Pdf.Net;
using pdfeditor.Controls.Copilot.Popups;
using pdfeditor.Controls.Menus;
using pdfeditor.Models.Menus;
using pdfeditor.Models.Menus.ToolbarSettings;
using pdfeditor.Properties;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit;
using PDFKit.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.PdfViewerDecorators;

internal class SelectTextContextMenuHolder
{
  private readonly AnnotationCanvas annotationCanvas;
  private PdfViewerContextMenu selectTextContextMenu;

  public SelectTextContextMenuHolder(AnnotationCanvas annotationCanvas)
  {
    this.annotationCanvas = annotationCanvas ?? throw new ArgumentNullException(nameof (annotationCanvas));
    this.InitContextMenu();
  }

  private MainViewModel VM => this.annotationCanvas.DataContext as MainViewModel;

  public bool ShowRecentColorInContextMenu { get; set; }

  private void InitContextMenu()
  {
    if (SelectTextContextMenuHolder.IsDesignMode)
      return;
    string defaultValue;
    System.Collections.Generic.IReadOnlyList<string> standardStokeColors1 = ToolbarSettingsHelper.DefaultValues.GetStandardStokeColors(AnnotationMode.Highlight, out defaultValue);
    System.Collections.Generic.IReadOnlyList<string> standardStokeColors2 = ToolbarSettingsHelper.DefaultValues.GetStandardStokeColors(AnnotationMode.Strike, out defaultValue);
    System.Collections.Generic.IReadOnlyList<string> standardStokeColors3 = ToolbarSettingsHelper.DefaultValues.GetStandardStokeColors(AnnotationMode.Underline, out defaultValue);
    ContextMenuItemModel contextMenuItemModel1 = new ContextMenuItemModel()
    {
      Name = "Copy",
      Caption = Resources.WinScreenshotToolbarCopyContent,
      Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/Select_Copy.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/Select_Copy.png")),
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.OnContextMenuClick))
    };
    TypedContextMenuItemModel contextMenuItemModel2 = new TypedContextMenuItemModel(ContextMenuItemType.StrokeColor);
    contextMenuItemModel2.Name = "Highlight";
    contextMenuItemModel2.Caption = Resources.MenuAnnotateHighlightContent;
    contextMenuItemModel2.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/Select_Highlight.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/Select_Highlight.png"));
    contextMenuItemModel2.Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.OnContextMenuClick));
    TypedContextMenuItemModel contextMenuItemModel3 = contextMenuItemModel2;
    TypedContextMenuItemModel contextMenuItemModel4 = new TypedContextMenuItemModel(ContextMenuItemType.StrokeColor);
    contextMenuItemModel4.Name = "Strike";
    contextMenuItemModel4.Caption = Resources.MenuAnnotateStrikeContent;
    contextMenuItemModel4.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/Select_Strike.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/Select_Strike.png"));
    contextMenuItemModel4.Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.OnContextMenuClick));
    TypedContextMenuItemModel contextMenuItemModel5 = contextMenuItemModel4;
    TypedContextMenuItemModel contextMenuItemModel6 = new TypedContextMenuItemModel(ContextMenuItemType.StrokeColor);
    contextMenuItemModel6.Name = "Underline";
    contextMenuItemModel6.Caption = Resources.MenuAnnotateUnderlineContent;
    contextMenuItemModel6.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/ContextMenu/Select_Underline.png"), new Uri("pack://application:,,,/Style/DarkModeResources/ContextMenu/Select_Underline.png"));
    contextMenuItemModel6.Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.OnContextMenuClick));
    TypedContextMenuItemModel contextMenuItemModel7 = contextMenuItemModel6;
    ContextMenuItemModel contextMenuItemModel8 = new ContextMenuItemModel();
    contextMenuItemModel8.Name = "AI";
    contextMenuItemModel8.Caption = "AI";
    contextMenuItemModel8.Icon = (ImageSource) null;
    contextMenuItemModel8.Add((IContextMenuModel) new ContextMenuItemModel()
    {
      Name = "Summarize",
      Caption = "Summarize",
      Icon = (ImageSource) null,
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.OnAIItemClick))
    });
    contextMenuItemModel8.Add((IContextMenuModel) new ContextMenuItemModel()
    {
      Name = "Translate",
      Caption = "Translate",
      Icon = (ImageSource) null,
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.OnAIItemClick))
    });
    contextMenuItemModel8.Add((IContextMenuModel) new ContextMenuItemModel()
    {
      Name = "Rewrite",
      Caption = "Rewrite",
      Icon = (ImageSource) null,
      Command = (ICommand) new RelayCommand<ContextMenuItemModel>(new Action<ContextMenuItemModel>(this.OnAIItemClick))
    });
    foreach (object obj in (IEnumerable<string>) standardStokeColors1)
    {
      ContextMenuItemModel contextMenuItem = ToolbarContextMenuHelper.CreateContextMenuItem(AnnotationMode.Highlight, ContextMenuItemType.StrokeColor, obj, false, new Action<ContextMenuItemModel>(this.OnContextMenuItemClick));
      contextMenuItemModel3.Add((IContextMenuModel) contextMenuItem);
    }
    foreach (object obj in (IEnumerable<string>) standardStokeColors2)
    {
      ContextMenuItemModel contextMenuItem = ToolbarContextMenuHelper.CreateContextMenuItem(AnnotationMode.Strike, ContextMenuItemType.StrokeColor, obj, false, new Action<ContextMenuItemModel>(this.OnContextMenuItemClick));
      contextMenuItemModel5.Add((IContextMenuModel) contextMenuItem);
    }
    foreach (object obj in (IEnumerable<string>) standardStokeColors3)
    {
      ContextMenuItemModel contextMenuItem = ToolbarContextMenuHelper.CreateContextMenuItem(AnnotationMode.Underline, ContextMenuItemType.StrokeColor, obj, false, new Action<ContextMenuItemModel>(this.OnContextMenuItemClick));
      contextMenuItemModel7.Add((IContextMenuModel) contextMenuItem);
    }
    ContextMenuModel contextMenuModel1 = new ContextMenuModel();
    contextMenuModel1.Add((IContextMenuModel) contextMenuItemModel1);
    contextMenuModel1.Add((IContextMenuModel) contextMenuItemModel3);
    contextMenuModel1.Add((IContextMenuModel) contextMenuItemModel7);
    contextMenuModel1.Add((IContextMenuModel) contextMenuItemModel5);
    ContextMenuModel contextMenuModel2 = contextMenuModel1;
    PdfViewerContextMenu viewerContextMenu = new PdfViewerContextMenu();
    viewerContextMenu.ItemsSource = (IEnumerable) contextMenuModel2;
    viewerContextMenu.PlacementTarget = (UIElement) this.annotationCanvas;
    this.selectTextContextMenu = viewerContextMenu;
  }

  private void OnContextMenuItemClick(ContextMenuItemModel model)
  {
    if (!(model.Parent is TypedContextMenuItemModel parent))
      return;
    ToolbarAnnotationButtonModel annotationButtonModel = (ToolbarAnnotationButtonModel) null;
    if (parent.Name == "Highlight")
      annotationButtonModel = this.VM.AnnotationToolbar.HighlightButtonModel;
    else if (parent.Name == "Strike")
      annotationButtonModel = this.VM.AnnotationToolbar.StrikeButtonModel;
    else if (parent.Name == "Underline")
      annotationButtonModel = this.VM.AnnotationToolbar.UnderlineButtonModel;
    object menuItemValue = model.TagData?.MenuItemValue;
    if (menuItemValue == null)
      return;
    ToolbarSettingItemModel settingItemModel = annotationButtonModel.ToolbarSettingModel.FirstOrDefault<ToolbarSettingItemModel>((Func<ToolbarSettingItemModel, bool>) (c => c.Type == ContextMenuItemType.StrokeColor));
    if (settingItemModel != null)
      settingItemModel.SelectedValue = menuItemValue;
    this.OnContextMenuClick((ContextMenuItemModel) parent);
  }

  private void OnContextMenuClick(ContextMenuItemModel model)
  {
    this.selectTextContextMenu.IsOpen = false;
    PdfViewer pdfViewer = this.annotationCanvas.PdfViewer;
    if (pdfViewer == null || !this.annotationCanvas.HasSelectedText())
      return;
    if (model.Name == "Copy")
    {
      Clipboard.SetDataObject((object) pdfViewer.SelectedText);
      pdfViewer.DeselectText();
      CommomLib.Commom.GAManager.SendEvent("ContextMenu", "Copy", "Count", 1L);
    }
    else if (model.Name == "Highlight")
    {
      this.VM.AnnotationToolbar.HighlightButtonModel.Tap();
      CommomLib.Commom.GAManager.SendEvent("ContextMenu", "Highlight", "Count", 1L);
    }
    else if (model.Name == "Strike")
    {
      this.VM.AnnotationToolbar.StrikeButtonModel.Tap();
      CommomLib.Commom.GAManager.SendEvent("ContextMenu", "Strike", "Count", 1L);
    }
    else
    {
      if (!(model.Name == "Underline"))
        return;
      this.VM.AnnotationToolbar.UnderlineButtonModel.Tap();
      CommomLib.Commom.GAManager.SendEvent("ContextMenu", "Underline", "Count", 1L);
    }
  }

  private void OnAIItemClick(ContextMenuItemModel model)
  {
    PdfViewer pdfViewer = this.annotationCanvas.PdfViewer;
    if (pdfViewer == null || !this.annotationCanvas.HasSelectedText())
      return;
    string selectedText = pdfViewer.SelectedText;
    if (string.IsNullOrEmpty(selectedText) || selectedText.Length <= 0 || selectedText.Length >= 2000 || !(model.Name == "Summarize"))
      return;
    new SummarizePopup(this.VM.CopilotHelper, selectedText).Show();
  }

  public void Hide() => this.selectTextContextMenu.IsOpen = false;

  public async Task<bool> ShowAsync(bool autoOpen)
  {
    SelectTextContextMenuHolder contextMenuHolder = this;
    PdfViewer pdfViewer = contextMenuHolder.annotationCanvas.PdfViewer;
    if ((pdfViewer != null ? (pdfViewer.MouseMode == MouseModes.PanTool ? 1 : 0) : 0) != 0)
      return false;
    await Task.Delay(50);
    if (!contextMenuHolder.annotationCanvas.HasSelectedText() || contextMenuHolder.selectTextContextMenu.IsOpen)
      return false;
    foreach (TypedContextMenuItemModel source in (contextMenuHolder.selectTextContextMenu.ItemsSource as ContextMenuModel).OfType<TypedContextMenuItemModel>())
    {
      AnnotationMode mode = AnnotationMode.None;
      object selectedValue = (object) null;
      string[] recentColors = (string[]) null;
      if (source.Name == "Highlight")
      {
        mode = AnnotationMode.Highlight;
        selectedValue = GetToolbarButtonSelectedValue(contextMenuHolder.VM.AnnotationToolbar.HighlightButtonModel, out recentColors);
      }
      else if (source.Name == "Strike")
      {
        mode = AnnotationMode.Strike;
        selectedValue = GetToolbarButtonSelectedValue(contextMenuHolder.VM.AnnotationToolbar.StrikeButtonModel, out recentColors);
      }
      else if (source.Name == "Underline")
      {
        mode = AnnotationMode.Underline;
        selectedValue = GetToolbarButtonSelectedValue(contextMenuHolder.VM.AnnotationToolbar.UnderlineButtonModel, out recentColors);
      }
      if (selectedValue != null)
      {
        for (int index = source.Count - 1; index >= 0; --index)
        {
          if (source[index] is ContextMenuItemModel contextMenuItemModel)
          {
            TagDataModel tagData = contextMenuItemModel.TagData;
            if ((tagData != null ? (tagData.IsTransient ? 1 : 0) : 0) != 0)
              source.RemoveAt(index);
          }
        }
        if (contextMenuHolder.ShowRecentColorInContextMenu && recentColors != null)
        {
          foreach (string str in recentColors)
          {
            ContextMenuItemModel contextMenuItem = ToolbarContextMenuHelper.CreateContextMenuItem(mode, ContextMenuItemType.StrokeColor, (object) str, true, new Action<ContextMenuItemModel>(contextMenuHolder.OnContextMenuItemClick));
            source.Add((IContextMenuModel) contextMenuItem);
          }
        }
        ContextMenuItemModel contextMenuItemModel1 = source.OfType<ContextMenuItemModel>().FirstOrDefault<ContextMenuItemModel>((Func<ContextMenuItemModel, bool>) (c => ToolbarContextMenuValueEqualityComparer.MenuValueEquals(mode, ContextMenuItemType.StrokeColor, c.TagData?.MenuItemValue, selectedValue)));
        if (contextMenuItemModel1 != null)
        {
          contextMenuItemModel1.IsChecked = true;
        }
        else
        {
          ContextMenuItemModel contextMenuItem = ToolbarContextMenuHelper.CreateContextMenuItem(mode, ContextMenuItemType.StrokeColor, selectedValue, true, new Action<ContextMenuItemModel>(contextMenuHolder.OnContextMenuItemClick));
          source.Add((IContextMenuModel) contextMenuItem);
          contextMenuItem.IsChecked = true;
        }
      }
    }
    if (autoOpen)
    {
      contextMenuHolder.selectTextContextMenu.AutoCloseOnMouseLeave = true;
      contextMenuHolder.UpdateSelectedTextContextMenuPlacementRect();
    }
    else
    {
      contextMenuHolder.selectTextContextMenu.AutoCloseOnMouseLeave = false;
      contextMenuHolder.selectTextContextMenu.PlacementRectangle = Rect.Empty;
    }
    contextMenuHolder.selectTextContextMenu.IsOpen = true;
    return true;

    static object GetToolbarButtonSelectedValue(
      ToolbarAnnotationButtonModel _button,
      out string[] recentColors)
    {
      recentColors = (string[]) null;
      ToolbarSettingModel toolbarSettingModel = _button.ToolbarSettingModel;
      ToolbarSettingItemModel settingItemModel = toolbarSettingModel != null ? toolbarSettingModel.FirstOrDefault<ToolbarSettingItemModel>((Func<ToolbarSettingItemModel, bool>) (c => c.Type == ContextMenuItemType.StrokeColor)) : (ToolbarSettingItemModel) null;
      if (settingItemModel is ToolbarSettingItemColorModel settingItemColorModel)
      {
        ref string[] local = ref recentColors;
        ObservableCollection<string> recentColors1 = settingItemColorModel.RecentColors;
        string[] array = recentColors1 != null ? recentColors1.ToArray<string>() : (string[]) null;
        local = array;
      }
      return settingItemModel?.SelectedValue ?? ToolbarContextMenuHelper.GetDefaultValue(_button.Mode, ContextMenuItemType.StrokeColor);
    }
  }

  private void UpdateSelectedTextContextMenuPlacementRect()
  {
    Rect[] selectedTextRect = this.GetSelectedTextRect();
    if (selectedTextRect.Length == 0)
      return;
    Point position = Mouse.GetPosition((IInputElement) this.annotationCanvas.PdfViewer);
    bool flag = false;
    Rect rect1 = Rect.Empty;
    double num1 = double.MaxValue;
    foreach (Rect rect2 in selectedTextRect)
    {
      if (rect2.Contains(position))
      {
        this.selectTextContextMenu.PlacementRectangle = rect2;
        flag = true;
        break;
      }
      double num2 = Math.Min(Math.Min(Math.Abs(rect2.Left - position.X), Math.Abs(rect2.Right - position.X)), Math.Min(Math.Abs(rect2.Top - position.Y), Math.Abs(rect2.Bottom - position.Y)));
      if (num2 < num1)
      {
        num1 = num2;
        rect1 = rect2;
      }
    }
    if (flag)
      return;
    this.selectTextContextMenu.PlacementRectangle = rect1;
  }

  private Rect[] GetSelectedTextRect()
  {
    PdfViewer pdfViewer = this.annotationCanvas.PdfViewer;
    PdfDocument document = pdfViewer?.Document;
    if (document == null)
      return Array.Empty<Rect>();
    if (!this.annotationCanvas.HasSelectedText())
      return Array.Empty<Rect>();
    SelectInfo selectInfo = pdfViewer.SelectInfo;
    List<Rect> rectList = new List<Rect>();
    for (int startPage = selectInfo.StartPage; startPage <= selectInfo.EndPage; ++startPage)
    {
      try
      {
        int index1 = 0;
        if (startPage == selectInfo.StartPage)
          index1 = selectInfo.StartIndex;
        int count = (startPage != selectInfo.EndPage ? document.Pages[startPage].Text.CountChars - 1 : selectInfo.EndIndex) - index1 + 1;
        if (count > 0)
        {
          PdfTextInfo textInfo = pdfViewer.Document.Pages[startPage].Text.GetTextInfo(index1, count);
          if (textInfo.Rects.Count > 0)
          {
            FS_RECTF rect1 = textInfo.Rects[0];
            for (int index2 = 1; index2 < textInfo.Rects.Count; ++index2)
            {
              FS_RECTF rect2 = textInfo.Rects[index2];
              rect1.left = Math.Min(rect1.left, rect2.left);
              rect1.right = Math.Max(rect1.right, rect2.right);
              rect1.top = Math.Max(rect1.top, rect2.top);
              rect1.bottom = Math.Min(rect1.bottom, rect2.bottom);
            }
            Rect clientRect;
            if (pdfViewer.TryGetClientRect(startPage, rect1, out clientRect))
              rectList.Add(clientRect);
          }
        }
      }
      catch
      {
      }
    }
    return rectList.ToArray();
  }

  private static bool IsDesignMode
  {
    get
    {
      return (bool) DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof (DependencyObject)).DefaultValue;
    }
  }
}
