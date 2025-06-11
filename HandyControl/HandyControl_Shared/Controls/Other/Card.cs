// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Card
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class Card : ContentControl
{
  public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (Card), new PropertyMetadata((object) null));
  public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (Card), new PropertyMetadata((object) null));
  public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register(nameof (HeaderTemplateSelector), typeof (DataTemplateSelector), typeof (Card), new PropertyMetadata((object) null));
  public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register(nameof (HeaderStringFormat), typeof (string), typeof (Card), new PropertyMetadata((object) null));
  public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(nameof (Footer), typeof (object), typeof (Card), new PropertyMetadata((object) null));
  public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register(nameof (FooterTemplate), typeof (DataTemplate), typeof (Card), new PropertyMetadata((object) null));
  public static readonly DependencyProperty FooterTemplateSelectorProperty = DependencyProperty.Register(nameof (FooterTemplateSelector), typeof (DataTemplateSelector), typeof (Card), new PropertyMetadata((object) null));
  public static readonly DependencyProperty FooterStringFormatProperty = DependencyProperty.Register(nameof (FooterStringFormat), typeof (string), typeof (Card), new PropertyMetadata((object) null));

  public object Header
  {
    get => this.GetValue(Card.HeaderProperty);
    set => this.SetValue(Card.HeaderProperty, value);
  }

  [Bindable(true)]
  [Category("Content")]
  public DataTemplate HeaderTemplate
  {
    get => (DataTemplate) this.GetValue(Card.HeaderTemplateProperty);
    set => this.SetValue(Card.HeaderTemplateProperty, (object) value);
  }

  [Bindable(true)]
  [Category("Content")]
  public DataTemplateSelector HeaderTemplateSelector
  {
    get => (DataTemplateSelector) this.GetValue(Card.HeaderTemplateSelectorProperty);
    set => this.SetValue(Card.HeaderTemplateSelectorProperty, (object) value);
  }

  [Bindable(true)]
  [Category("Content")]
  public string HeaderStringFormat
  {
    get => (string) this.GetValue(Card.HeaderStringFormatProperty);
    set => this.SetValue(Card.HeaderStringFormatProperty, (object) value);
  }

  public object Footer
  {
    get => this.GetValue(Card.FooterProperty);
    set => this.SetValue(Card.FooterProperty, value);
  }

  [Bindable(true)]
  [Category("Content")]
  public DataTemplate FooterTemplate
  {
    get => (DataTemplate) this.GetValue(Card.FooterTemplateProperty);
    set => this.SetValue(Card.FooterTemplateProperty, (object) value);
  }

  [Bindable(true)]
  [Category("Content")]
  public DataTemplateSelector FooterTemplateSelector
  {
    get => (DataTemplateSelector) this.GetValue(Card.FooterTemplateSelectorProperty);
    set => this.SetValue(Card.FooterTemplateSelectorProperty, (object) value);
  }

  [Bindable(true)]
  [Category("Content")]
  public string FooterStringFormat
  {
    get => (string) this.GetValue(Card.FooterStringFormatProperty);
    set => this.SetValue(Card.FooterStringFormatProperty, (object) value);
  }
}
