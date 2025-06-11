// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ToolbarSettings.ToolbarSettingInkEraserModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf.Net.Annotations;
using pdfeditor.ViewModels;
using System;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Models.Menus.ToolbarSettings;

public class ToolbarSettingInkEraserModel : ToolbarSettingItemModel
{
  private bool isCheckable = true;
  private bool isChecked;
  private string inkName = "Ink";
  private ToolbarSettingInkEraserModel.EraserType isPartial;
  private int selectSize;

  public ToolbarSettingInkEraserModel()
    : base(ContextMenuItemType.None)
  {
  }

  public bool IsCheckable
  {
    get => this.isCheckable;
    set
    {
      if (!this.SetProperty<bool>(ref this.isCheckable, value, nameof (IsCheckable)) || value || !this.IsChecked)
        return;
      this.IsChecked = false;
    }
  }

  public int SelectSize
  {
    get => this.selectSize;
    set
    {
      ConfigManager.SetEraserSize(value);
      this.SetProperty<int>(ref this.selectSize, value, nameof (SelectSize));
    }
  }

  public ToolbarSettingInkEraserModel.EraserType IsPartial
  {
    get => this.isPartial;
    set
    {
      this.SetProperty<ToolbarSettingInkEraserModel.EraserType>(ref this.isPartial, value, nameof (IsPartial));
    }
  }

  public string InkName
  {
    get => this.inkName;
    set => this.SetProperty<string>(ref this.inkName, value, nameof (InkName));
  }

  public ImageSource EraserImage
  {
    get
    {
      return ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/Eraser.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/Eraser.png"));
    }
  }

  public bool IsChecked
  {
    get => this.isChecked;
    set
    {
      MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
      if (requiredService.AnnotationToolbar != null)
      {
        if (value)
        {
          requiredService.AnnotationToolbar.InkButtonModel.IsChecked = value;
          requiredService.SelectedAnnotation = (PdfAnnotation) null;
          if (this.IsPartial == ToolbarSettingInkEraserModel.EraserType.None)
            this.IsPartial = !(ConfigManager.GetEraserMode() == ToolbarSettingInkEraserModel.EraserType.Partial.ToString()) ? ToolbarSettingInkEraserModel.EraserType.Whole : ToolbarSettingInkEraserModel.EraserType.Partial;
          ConfigManager.SetEraserMode(this.IsPartial.ToString());
        }
        if (!value && this.isPartial != ToolbarSettingInkEraserModel.EraserType.None)
          this.IsPartial = ToolbarSettingInkEraserModel.EraserType.None;
      }
      this.SetProperty<bool>(ref this.isChecked, value, nameof (IsChecked));
    }
  }

  public enum EraserType
  {
    None,
    Partial,
    Whole,
  }
}
