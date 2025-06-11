// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.LeftNavigations.NavigationModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Models.LeftNavigations;

public class NavigationModel : ObservableObject
{
  private string name;
  private string displayName;
  private ImageSource icon;
  private ImageSource selectedIcon;

  public NavigationModel()
  {
  }

  public NavigationModel(
    string name,
    string displayName,
    ImageSource icon,
    ImageSource selectedIcon)
    : this()
  {
    this.name = name;
    this.displayName = displayName;
    this.icon = icon;
    this.selectedIcon = selectedIcon;
  }

  public string Name
  {
    get => this.name;
    set => this.SetProperty<string>(ref this.name, value, nameof (Name));
  }

  public string DisplayName
  {
    get => this.displayName;
    set => this.SetProperty<string>(ref this.displayName, value, nameof (DisplayName));
  }

  public ImageSource Icon
  {
    get => this.icon;
    set => this.SetProperty<ImageSource>(ref this.icon, value, nameof (Icon));
  }

  public ImageSource SelectedIcon
  {
    get => this.selectedIcon;
    set => this.SetProperty<ImageSource>(ref this.selectedIcon, value, nameof (SelectedIcon));
  }
}
