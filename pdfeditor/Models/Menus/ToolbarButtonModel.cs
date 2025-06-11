// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ToolbarButtonModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Models.Menus;

public class ToolbarButtonModel : ObservableObject
{
  private string name;
  private bool isCheckable = true;
  private bool isChecked;
  private ToolbarChildButtonModel childButtonModel;
  private ImageSource icon;
  private string caption;
  private string tooltip;
  private bool isEnabled;
  private ICommand command;
  private object commandParameter;
  private SolidColorBrush indicatorBrush;

  public ToolbarButtonModel()
  {
    this.caption = "";
    this.tooltip = "";
  }

  public string Name
  {
    get => this.name;
    set => this.SetProperty<string>(ref this.name, value, nameof (Name));
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

  public bool IsChecked
  {
    get => this.isChecked;
    set => this.SetProperty<bool>(ref this.isChecked, value, nameof (IsChecked));
  }

  public ToolbarChildButtonModel ChildButtonModel
  {
    get => this.childButtonModel;
    set
    {
      ToolbarChildButtonModel childButtonModel = this.childButtonModel;
      if (!this.SetProperty<ToolbarChildButtonModel>(ref this.childButtonModel, value, nameof (ChildButtonModel)))
        return;
      this.OnChildButtonModelChanged(value, childButtonModel);
    }
  }

  protected virtual void OnChildButtonModelChanged(
    ToolbarChildButtonModel newValue,
    ToolbarChildButtonModel oldValue)
  {
    if (oldValue != null)
      oldValue.Parent = (ToolbarButtonModel) null;
    if (newValue == null)
      return;
    newValue.Parent = this;
  }

  public ImageSource Icon
  {
    get => this.icon;
    set => this.SetProperty<ImageSource>(ref this.icon, value, nameof (Icon));
  }

  public string Caption
  {
    get => this.caption;
    set => this.SetProperty<string>(ref this.caption, value, nameof (Caption));
  }

  public string Tooltip
  {
    get => this.tooltip;
    set => this.SetProperty<string>(ref this.tooltip, value, nameof (Tooltip));
  }

  public bool IsEnabled
  {
    get => this.isEnabled;
    set => this.SetProperty<bool>(ref this.isEnabled, value, nameof (IsEnabled));
  }

  public ICommand Command
  {
    get => this.command;
    set => this.SetProperty<ICommand>(ref this.command, value, nameof (Command));
  }

  public object CommandParameter
  {
    get => this.commandParameter;
    set => this.SetProperty<object>(ref this.commandParameter, value, nameof (CommandParameter));
  }

  public SolidColorBrush IndicatorBrush
  {
    get => this.indicatorBrush;
    set
    {
      this.SetProperty<SolidColorBrush>(ref this.indicatorBrush, value, nameof (IndicatorBrush));
    }
  }

  public void Tap()
  {
    if (this.IsCheckable && !this.IsChecked)
      this.IsChecked = true;
    this.Command?.Execute(this.CommandParameter ?? (object) this);
  }
}
