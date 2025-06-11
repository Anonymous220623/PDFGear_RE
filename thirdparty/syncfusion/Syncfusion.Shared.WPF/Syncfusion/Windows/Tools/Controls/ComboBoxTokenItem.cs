// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.ComboBoxTokenItem
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public class ComboBoxTokenItem : ContentControl
{
  private Button tokenButton;

  private ItemsControl Parent
  {
    get => ItemsControl.ItemsControlFromItemContainer((DependencyObject) this);
  }

  public ComboBoxTokenItem() => this.DefaultStyleKey = (object) typeof (ComboBoxTokenItem);

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.tokenButton = this.GetTemplateChild("Token_CloseButton") as Button;
    this.tokenButton.Unloaded += new RoutedEventHandler(this.TokenButton_Unloaded);
    this.tokenButton.PreviewMouseDown += new MouseButtonEventHandler(this.TokenButton_MouseDown);
  }

  private void TokenButton_Unloaded(object sender, RoutedEventArgs e)
  {
    Button button = sender as Button;
    button.PreviewMouseDown -= new MouseButtonEventHandler(this.TokenButton_MouseDown);
    button.Unloaded -= new RoutedEventHandler(this.TokenButton_Unloaded);
  }

  private void TokenButton_MouseDown(object sender, MouseButtonEventArgs e)
  {
    (this.Parent.TemplatedParent as ComboBoxAdv).RemoveToken(this.Content);
  }
}
