// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.AnnotationMenuPropertyAccessor
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using pdfeditor.Models.Menus.ToolbarSettings;
using pdfeditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace pdfeditor.Models.Menus;

public class AnnotationMenuPropertyAccessor : ObservableObject
{
  private static Dictionary<(AnnotationMode, ContextMenuItemType), Func<AnnotationMenuPropertyAccessor, object>> dict;
  private readonly AnnotationToolbarViewModel vm;

  public AnnotationMenuPropertyAccessor(AnnotationToolbarViewModel vm)
  {
    this.vm = vm ?? throw new ArgumentNullException(nameof (vm));
  }

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.Line, ContextMenuItemType.StrokeColor)]
  public string LineStroke => this.StrokeColor(this.vm.LineButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.Line, ContextMenuItemType.StrokeThickness)]
  public float LineWidth => this.StrokeThickness(this.vm.LineButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.Underline, ContextMenuItemType.StrokeColor)]
  public string UnderlineStroke => this.StrokeColor(this.vm.UnderlineButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.Highlight, ContextMenuItemType.StrokeColor)]
  public string HighlightStroke => this.StrokeColor(this.vm.HighlightButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.HighlightArea, ContextMenuItemType.StrokeColor)]
  public string HighlightAreaStroke => this.StrokeColor(this.vm.HighlightAreaButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.Strike, ContextMenuItemType.StrokeColor)]
  public string StrikeStroke => this.StrokeColor(this.vm.StrikeButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.Ink, ContextMenuItemType.StrokeColor)]
  public string InkStroke => this.StrokeColor(this.vm.InkButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.Ink, ContextMenuItemType.StrokeThickness)]
  public float InkWidth => this.StrokeThickness(this.vm.InkButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.Shape, ContextMenuItemType.StrokeColor)]
  public string ShapeStroke => this.StrokeColor(this.vm.SquareButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.Shape, ContextMenuItemType.FillColor)]
  public string ShapeFill => this.FillColor(this.vm.SquareButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.Shape, ContextMenuItemType.StrokeThickness)]
  public float ShapeThickness => this.StrokeThickness(this.vm.SquareButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.Ellipse, ContextMenuItemType.FillColor)]
  public string EllipseFill => this.FillColor(this.vm.CircleButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.Ellipse, ContextMenuItemType.StrokeColor)]
  public string EllipseStroke => this.StrokeColor(this.vm.CircleButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.Ellipse, ContextMenuItemType.StrokeThickness)]
  public float EllipseThickness => this.StrokeThickness(this.vm.CircleButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.TextBox, ContextMenuItemType.StrokeColor)]
  public string TextBoxStroke => this.StrokeColor(this.vm.TextBoxButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.TextBox, ContextMenuItemType.FillColor)]
  public string TextBoxFill => this.FillColor(this.vm.TextBoxButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.TextBox, ContextMenuItemType.StrokeThickness)]
  public float TextBoxThickness => this.StrokeThickness(this.vm.TextBoxButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.TextBox, ContextMenuItemType.FontSize)]
  public float TextBoxFontSize => this.FontSize(this.vm.TextBoxButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.TextBox, ContextMenuItemType.FontColor)]
  public string TextBoxFontColor => this.FontColor(this.vm.TextBoxButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.TextBox, ContextMenuItemType.FontName)]
  public string TextBoxFontName => "Arial";

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.Text, ContextMenuItemType.FontSize)]
  public float TextFontSize => this.FontSize(this.vm.TextButtonModel);

  [AnnotationMenuPropertyAccessor.MenuPropertyBind(AnnotationMode.Text, ContextMenuItemType.FontColor)]
  public string TextFontColor => this.FontColor(this.vm.TextButtonModel);

  private string StrokeColor(ToolbarAnnotationButtonModel buttonModel)
  {
    return buttonModel.ToolbarSettingModel.FirstOrDefault<ToolbarSettingItemModel>((Func<ToolbarSettingItemModel, bool>) (c => c.Type == ContextMenuItemType.StrokeColor))?.SelectedValue as string;
  }

  private string FillColor(ToolbarAnnotationButtonModel buttonModel)
  {
    return buttonModel.ToolbarSettingModel.FirstOrDefault<ToolbarSettingItemModel>((Func<ToolbarSettingItemModel, bool>) (c => c.Type == ContextMenuItemType.FillColor))?.SelectedValue as string;
  }

  private string FontColor(ToolbarAnnotationButtonModel buttonModel)
  {
    return buttonModel.ToolbarSettingModel.FirstOrDefault<ToolbarSettingItemModel>((Func<ToolbarSettingItemModel, bool>) (c => c.Type == ContextMenuItemType.FontColor))?.SelectedValue as string;
  }

  private float StrokeThickness(ToolbarAnnotationButtonModel buttonModel, float defaultValue = 1f)
  {
    return this.ToFloat(buttonModel.ToolbarSettingModel.FirstOrDefault<ToolbarSettingItemModel>((Func<ToolbarSettingItemModel, bool>) (c => c.Type == ContextMenuItemType.StrokeThickness))?.SelectedValue, defaultValue);
  }

  private float FontSize(ToolbarAnnotationButtonModel buttonModel, float defaultValue = 12f)
  {
    return this.ToFloat(buttonModel.ToolbarSettingModel.FirstOrDefault<ToolbarSettingItemModel>((Func<ToolbarSettingItemModel, bool>) (c => c.Type == ContextMenuItemType.FontSize))?.SelectedValue, defaultValue);
  }

  private float ToFloat(object value, float defaultValue = 1f)
  {
    try
    {
      if (!(value is string str))
        return Convert.ToSingle(value);
      if (str.ToLowerInvariant().EndsWith("pt"))
        str = str.Substring(0, str.Length - 2);
      return Convert.ToSingle(str);
    }
    catch
    {
    }
    return defaultValue;
  }

  public object GetTagDataValue(AnnotationMode mode, ContextMenuItemType type)
  {
    AnnotationMenuPropertyAccessor.InitPropertiesTable();
    Func<AnnotationMenuPropertyAccessor, object> func;
    return AnnotationMenuPropertyAccessor.dict.TryGetValue((mode, type), out func) ? func(this) : (object) null;
  }

  public static string BuildPropertyName(
    ToolbarAnnotationButtonModel model,
    SelectedAccessorSelectionChangedEventArgs args)
  {
    if (model == null)
      throw new ArgumentNullException(nameof (model));
    if (args == null)
      throw new ArgumentNullException(nameof (args));
    return AnnotationMenuPropertyAccessor.BuildPropertyNameCore(model.Mode, args.Type);
  }

  public static string BuildPropertyName(AnnotationMode mode, ContextMenuItemType type)
  {
    return mode != AnnotationMode.None ? AnnotationMenuPropertyAccessor.BuildPropertyNameCore(mode, type) : throw new ArgumentNullException(nameof (mode));
  }

  private static string BuildPropertyNameCore(AnnotationMode mode, ContextMenuItemType type)
  {
    string str1 = mode.ToString();
    string str2;
    switch (type)
    {
      case ContextMenuItemType.StrokeColor:
        str2 = "Stroke";
        break;
      case ContextMenuItemType.FillColor:
        str2 = "Fill";
        break;
      case ContextMenuItemType.StrokeThickness:
        str2 = mode == AnnotationMode.Line || mode == AnnotationMode.Arrow || mode == AnnotationMode.Ink ? "Width" : "Thickness";
        break;
      case ContextMenuItemType.FontSize:
        str2 = "FontSize";
        break;
      case ContextMenuItemType.FontName:
        str2 = "FontName";
        break;
      case ContextMenuItemType.FontColor:
        str2 = "FontColor";
        break;
      default:
        str2 = "_err";
        break;
    }
    string str3 = str2;
    return str1 + str3;
  }

  private static void InitPropertiesTable()
  {
    if (AnnotationMenuPropertyAccessor.dict != null)
      return;
    lock (typeof (AnnotationMenuPropertyAccessor))
    {
      if (AnnotationMenuPropertyAccessor.dict != null)
        return;
      AnnotationMenuPropertyAccessor.dict = ((IEnumerable<(PropertyInfo, AnnotationMenuPropertyAccessor.MenuPropertyBindAttribute)>) ((IEnumerable<PropertyInfo>) typeof (AnnotationMenuPropertyAccessor).GetProperties()).Select<PropertyInfo, (PropertyInfo, AnnotationMenuPropertyAccessor.MenuPropertyBindAttribute)>((Func<PropertyInfo, (PropertyInfo, AnnotationMenuPropertyAccessor.MenuPropertyBindAttribute)>) (c => (c, c.GetCustomAttribute<AnnotationMenuPropertyAccessor.MenuPropertyBindAttribute>()))).Where<(PropertyInfo, AnnotationMenuPropertyAccessor.MenuPropertyBindAttribute)>((Func<(PropertyInfo, AnnotationMenuPropertyAccessor.MenuPropertyBindAttribute), bool>) (c => c.attribute != null)).ToArray<(PropertyInfo, AnnotationMenuPropertyAccessor.MenuPropertyBindAttribute)>()).Select<(PropertyInfo, AnnotationMenuPropertyAccessor.MenuPropertyBindAttribute), ((PropertyInfo, AnnotationMenuPropertyAccessor.MenuPropertyBindAttribute), Func<AnnotationMenuPropertyAccessor, object>)>((Func<(PropertyInfo, AnnotationMenuPropertyAccessor.MenuPropertyBindAttribute), ((PropertyInfo, AnnotationMenuPropertyAccessor.MenuPropertyBindAttribute), Func<AnnotationMenuPropertyAccessor, object>)>) (c => (c, BuildFunc(c.property)))).Where<((PropertyInfo, AnnotationMenuPropertyAccessor.MenuPropertyBindAttribute), Func<AnnotationMenuPropertyAccessor, object>)>((Func<((PropertyInfo, AnnotationMenuPropertyAccessor.MenuPropertyBindAttribute), Func<AnnotationMenuPropertyAccessor, object>), bool>) (c => c.v != null)).ToDictionary<((PropertyInfo, AnnotationMenuPropertyAccessor.MenuPropertyBindAttribute), Func<AnnotationMenuPropertyAccessor, object>), (AnnotationMode, ContextMenuItemType), Func<AnnotationMenuPropertyAccessor, object>>((Func<((PropertyInfo, AnnotationMenuPropertyAccessor.MenuPropertyBindAttribute), Func<AnnotationMenuPropertyAccessor, object>), (AnnotationMode, ContextMenuItemType)>) (c => (c.k.attribute.Mode, c.k.attribute.Type)), (Func<((PropertyInfo, AnnotationMenuPropertyAccessor.MenuPropertyBindAttribute), Func<AnnotationMenuPropertyAccessor, object>), Func<AnnotationMenuPropertyAccessor, object>>) (c => c.v));
    }

    static Func<AnnotationMenuPropertyAccessor, object> BuildFunc(PropertyInfo property)
    {
      return TypeHelper.CreateFieldOrPropertyGetter<AnnotationMenuPropertyAccessor, object>(property.Name, BindingFlags.Instance | BindingFlags.Public);
    }
  }

  [AttributeUsage(AttributeTargets.Property)]
  public class MenuPropertyBindAttribute : Attribute
  {
    public MenuPropertyBindAttribute(AnnotationMode mode, ContextMenuItemType type)
    {
      this.Mode = mode;
      this.Type = type;
    }

    public AnnotationMode Mode { get; }

    public ContextMenuItemType Type { get; }
  }
}
