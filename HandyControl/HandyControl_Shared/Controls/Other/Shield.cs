// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Shield
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

[System.Windows.Markup.ContentProperty("Status")]
public class Shield : ButtonBase
{
  public static readonly DependencyProperty SubjectProperty = DependencyProperty.Register(nameof (Subject), typeof (string), typeof (Shield), new PropertyMetadata((object) null));
  public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(nameof (Status), typeof (object), typeof (Shield), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof (Color), typeof (Brush), typeof (Shield), new PropertyMetadata((object) null));

  public string Subject
  {
    get => (string) this.GetValue(Shield.SubjectProperty);
    set => this.SetValue(Shield.SubjectProperty, (object) value);
  }

  public object Status
  {
    get => this.GetValue(Shield.StatusProperty);
    set => this.SetValue(Shield.StatusProperty, value);
  }

  public Brush Color
  {
    get => (Brush) this.GetValue(Shield.ColorProperty);
    set => this.SetValue(Shield.ColorProperty, (object) value);
  }
}
