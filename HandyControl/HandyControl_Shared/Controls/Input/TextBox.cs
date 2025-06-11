// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.TextBox
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Interactivity;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Controls;

public class TextBox : System.Windows.Controls.TextBox
{
  public TextBox()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Clear, (ExecutedRoutedEventHandler) ((s, e) =>
    {
      if (this.IsReadOnly)
        return;
      this.SetCurrentValue(System.Windows.Controls.TextBox.TextProperty, (object) string.Empty);
    })));
  }
}
