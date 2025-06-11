// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.ButtonGroupItemStyleSelector
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace HandyControl.Tools;

public class ButtonGroupItemStyleSelector : StyleSelector
{
  private static readonly Dictionary<string, Style> StyleDict = new Dictionary<string, Style>()
  {
    ["RadioGroupItemSingle"] = ResourceHelper.GetResourceInternal<Style>("RadioGroupItemSingle"),
    ["RadioGroupItemHorizontalFirst"] = ResourceHelper.GetResourceInternal<Style>("RadioGroupItemHorizontalFirst"),
    ["RadioGroupItemHorizontalLast"] = ResourceHelper.GetResourceInternal<Style>("RadioGroupItemHorizontalLast"),
    ["RadioGroupItemVerticalFirst"] = ResourceHelper.GetResourceInternal<Style>("RadioGroupItemVerticalFirst"),
    ["RadioGroupItemVerticalLast"] = ResourceHelper.GetResourceInternal<Style>("RadioGroupItemVerticalLast"),
    ["RadioGroupItemDefault"] = ResourceHelper.GetResourceInternal<Style>("RadioGroupItemDefault"),
    ["ButtonGroupItemSingle"] = ResourceHelper.GetResourceInternal<Style>("ButtonGroupItemSingle"),
    ["ButtonGroupItemHorizontalFirst"] = ResourceHelper.GetResourceInternal<Style>("ButtonGroupItemHorizontalFirst"),
    ["ButtonGroupItemHorizontalLast"] = ResourceHelper.GetResourceInternal<Style>("ButtonGroupItemHorizontalLast"),
    ["ButtonGroupItemVerticalFirst"] = ResourceHelper.GetResourceInternal<Style>("ButtonGroupItemVerticalFirst"),
    ["ButtonGroupItemVerticalLast"] = ResourceHelper.GetResourceInternal<Style>("ButtonGroupItemVerticalLast"),
    ["ButtonGroupItemDefault"] = ResourceHelper.GetResourceInternal<Style>("ButtonGroupItemDefault"),
    ["ToggleButtonGroupItemSingle"] = ResourceHelper.GetResourceInternal<Style>("ToggleButtonGroupItemSingle"),
    ["ToggleButtonGroupItemHorizontalFirst"] = ResourceHelper.GetResourceInternal<Style>("ToggleButtonGroupItemHorizontalFirst"),
    ["ToggleButtonGroupItemHorizontalLast"] = ResourceHelper.GetResourceInternal<Style>("ToggleButtonGroupItemHorizontalLast"),
    ["ToggleButtonGroupItemVerticalFirst"] = ResourceHelper.GetResourceInternal<Style>("ToggleButtonGroupItemVerticalFirst"),
    ["ToggleButtonGroupItemVerticalLast"] = ResourceHelper.GetResourceInternal<Style>("ToggleButtonGroupItemVerticalLast"),
    ["ToggleButtonGroupItemDefault"] = ResourceHelper.GetResourceInternal<Style>("ToggleButtonGroupItemDefault")
  };

  public override Style SelectStyle(object item, DependencyObject container)
  {
    if (container is ButtonGroup buttonGroup && item is ButtonBase button)
    {
      int visibleButtonsCount = ButtonGroupItemStyleSelector.GetVisibleButtonsCount(buttonGroup);
      switch (button)
      {
        case RadioButton _:
          return ButtonGroupItemStyleSelector.GetRadioButtonStyle(visibleButtonsCount, buttonGroup, button);
        case Button _:
          return ButtonGroupItemStyleSelector.GetButtonStyle(visibleButtonsCount, buttonGroup, button);
        case ToggleButton _:
          return ButtonGroupItemStyleSelector.GetToggleButtonStyle(visibleButtonsCount, buttonGroup, button);
      }
    }
    return (Style) null;
  }

  private static int GetVisibleButtonsCount(ButtonGroup buttonGroup)
  {
    return buttonGroup.Items.OfType<ButtonBase>().Count<ButtonBase>((Func<ButtonBase, bool>) (button => button.IsVisible));
  }

  private static Style GetToggleButtonStyle(int count, ButtonGroup buttonGroup, ButtonBase button)
  {
    if (count == 1)
      return ButtonGroupItemStyleSelector.StyleDict["ToggleButtonGroupItemSingle"];
    int num = buttonGroup.Items.IndexOf((object) button);
    return buttonGroup.Orientation != Orientation.Horizontal ? (num != 0 ? ButtonGroupItemStyleSelector.StyleDict[num == count - 1 ? "ToggleButtonGroupItemVerticalLast" : "ToggleButtonGroupItemDefault"] : ButtonGroupItemStyleSelector.StyleDict["ToggleButtonGroupItemVerticalFirst"]) : (num != 0 ? ButtonGroupItemStyleSelector.StyleDict[num == count - 1 ? "ToggleButtonGroupItemHorizontalLast" : "ToggleButtonGroupItemDefault"] : ButtonGroupItemStyleSelector.StyleDict["ToggleButtonGroupItemHorizontalFirst"]);
  }

  private static Style GetButtonStyle(int count, ButtonGroup buttonGroup, ButtonBase button)
  {
    if (count == 1)
      return ButtonGroupItemStyleSelector.StyleDict["ButtonGroupItemSingle"];
    int num = buttonGroup.Items.IndexOf((object) button);
    return buttonGroup.Orientation != Orientation.Horizontal ? (num != 0 ? ButtonGroupItemStyleSelector.StyleDict[num == count - 1 ? "ButtonGroupItemVerticalLast" : "ButtonGroupItemDefault"] : ButtonGroupItemStyleSelector.StyleDict["ButtonGroupItemVerticalFirst"]) : (num != 0 ? ButtonGroupItemStyleSelector.StyleDict[num == count - 1 ? "ButtonGroupItemHorizontalLast" : "ButtonGroupItemDefault"] : ButtonGroupItemStyleSelector.StyleDict["ButtonGroupItemHorizontalFirst"]);
  }

  private static Style GetRadioButtonStyle(int count, ButtonGroup buttonGroup, ButtonBase button)
  {
    if (count == 1)
      return ButtonGroupItemStyleSelector.StyleDict["RadioGroupItemSingle"];
    int num = buttonGroup.Items.IndexOf((object) button);
    return buttonGroup.Orientation != Orientation.Horizontal ? (num != 0 ? ButtonGroupItemStyleSelector.StyleDict[num == count - 1 ? "RadioGroupItemVerticalLast" : "RadioGroupItemDefault"] : ButtonGroupItemStyleSelector.StyleDict["RadioGroupItemVerticalFirst"]) : (num != 0 ? ButtonGroupItemStyleSelector.StyleDict[num == count - 1 ? "RadioGroupItemHorizontalLast" : "RadioGroupItemDefault"] : ButtonGroupItemStyleSelector.StyleDict["RadioGroupItemHorizontalFirst"]);
  }
}
