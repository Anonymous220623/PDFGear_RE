// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.MessageBoxHelper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using CommomLib.Commom.MessageBoxHelper;
using pdfeditor.Properties;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace pdfeditor.Utils;

public static class MessageBoxHelper
{
  private const string InternalCreateObjectTag = "MessageBoxHelperCreate";

  public static pdfeditor.Utils.MessageBoxHelper.RichMessageBoxResult Show(
    pdfeditor.Utils.MessageBoxHelper.RichMessageBoxContent messageBoxContent,
    string caption = "",
    MessageBoxButton button = MessageBoxButton.OK,
    MessageBoxResult defaultResult = MessageBoxResult.None,
    CultureInfo cultureInfo = null,
    bool isButtonReversed = false)
  {
    UIElement element1 = messageBoxContent != null ? CreateUIElement(messageBoxContent.Title, true) : throw new ArgumentException((string) null, nameof (messageBoxContent));
    UIElement uiElement1 = CreateUIElement(messageBoxContent.Content, false);
    if (element1 == null && uiElement1 == null)
      throw new ArgumentException((string) null, nameof (messageBoxContent));
    if (uiElement1 != null)
    {
      Grid.SetRow(uiElement1, 1);
      if (uiElement1 is FrameworkElement frameworkElement && element1 != null && object.Equals(frameworkElement.Tag, (object) "MessageBoxHelperCreate"))
        frameworkElement.Margin = new Thickness(0.0, 12.0, 0.0, 12.0);
    }
    CheckBox element2 = (CheckBox) null;
    if (messageBoxContent.ShowLeftBottomCheckbox)
    {
      CheckBox checkBox = new CheckBox();
      checkBox.HorizontalAlignment = HorizontalAlignment.Left;
      checkBox.VerticalAlignment = VerticalAlignment.Bottom;
      checkBox.Margin = new Thickness(0.0, 0.0, 0.0, -36.0);
      element2 = checkBox;
      element2.SetResourceReference(FrameworkElement.StyleProperty, (object) "DefaultCheckBoxStyle");
      if (!string.IsNullOrEmpty(messageBoxContent.LeftBottomCheckboxContent))
        element2.Content = (object) messageBoxContent.LeftBottomCheckboxContent;
      Grid.SetRow((UIElement) element2, 2);
    }
    Grid grid1 = new Grid();
    grid1.RowDefinitions.Add(new RowDefinition()
    {
      Height = new GridLength(1.0, GridUnitType.Auto)
    });
    grid1.RowDefinitions.Add(new RowDefinition()
    {
      Height = new GridLength(1.0, GridUnitType.Star)
    });
    grid1.RowDefinitions.Add(new RowDefinition()
    {
      Height = new GridLength(1.0, GridUnitType.Auto)
    });
    grid1.MinWidth = 384.0;
    Grid grid2 = grid1;
    if (element1 != null)
      grid2.Children.Add(element1);
    if (uiElement1 != null)
      grid2.Children.Add(uiElement1);
    if (element2 != null)
      grid2.Children.Add((UIElement) element2);
    return new pdfeditor.Utils.MessageBoxHelper.RichMessageBoxResult(ModernMessageBox.Show(new ModernMessageBoxOptions()
    {
      Caption = caption,
      MessageBoxContent = (object) grid2,
      Button = button,
      DefaultResult = defaultResult,
      CultureInfo = cultureInfo,
      UIOverrides = {
        IsButtonsReversed = isButtonReversed
      }
    }), (bool?) element2?.IsChecked);

    static UIElement CreateUIElement(object _obj, bool _bold)
    {
      switch (_obj)
      {
        case UIElement uiElement2:
          return uiElement2;
        case string str:
          TextBlock uiElement1 = new TextBlock();
          uiElement1.Text = str;
          uiElement1.LineHeight = 18.0;
          uiElement1.FontWeight = _bold ? FontWeights.Bold : FontWeights.Normal;
          uiElement1.TextWrapping = TextWrapping.Wrap;
          uiElement1.Tag = (object) "MessageBoxHelperCreate";
          return (UIElement) uiElement1;
        default:
          return (UIElement) null;
      }
    }
  }

  public class RichMessageBoxContent
  {
    public static string DefaultLeftBottomCheckboxContent
    {
      get => Resources.WinPwdPasswordSaveTipNotshowagainContent;
    }

    public object Title { get; set; }

    public object Content { get; set; }

    public bool ShowLeftBottomCheckbox { get; set; }

    public string LeftBottomCheckboxContent { get; set; } = pdfeditor.Utils.MessageBoxHelper.RichMessageBoxContent.DefaultLeftBottomCheckboxContent;
  }

  public struct RichMessageBoxResult(MessageBoxResult result, bool? checkboxResult)
  {
    public MessageBoxResult Result { get; } = result;

    public bool? CheckboxResult { get; } = checkboxResult;
  }
}
