// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DomainUpDown
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class DomainUpDown : Control
{
  protected const double DEF_ANIMATION_SPEED = 0.2;
  public static RoutedCommand m_downValue;
  public static RoutedCommand m_upValue;
  private TextBox m_firstBlock;
  private TextBox m_secondBlock;
  private TextBox m_textBox;
  private bool m_isAnimated;
  private bool m_isUp;
  private int m_exIndex;
  private int m_index;
  private ArrayList m_list;
  public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (string), typeof (DomainUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(DomainUpDown.OnValueChanged)));
  public static readonly DependencyProperty AnimationShiftProperty = DependencyProperty.Register(nameof (AnimationShift), typeof (double), typeof (DomainUpDown), (PropertyMetadata) new UIPropertyMetadata((object) 1.0));

  public string this[int index] => (string) this.m_list[index];

  public string Value
  {
    get => (string) this.GetValue(DomainUpDown.ValueProperty);
    set => this.SetValue(DomainUpDown.ValueProperty, (object) value);
  }

  public double AnimationShift
  {
    get => (double) this.GetValue(DomainUpDown.AnimationShiftProperty);
    set => this.SetValue(DomainUpDown.AnimationShiftProperty, (object) value);
  }

  static DomainUpDown()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (DomainUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (DomainUpDown)));
    DomainUpDown.m_downValue = new RoutedCommand();
    DomainUpDown.m_upValue = new RoutedCommand();
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public DomainUpDown()
  {
    this.Initialize();
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  private void Initialize()
  {
    this.m_list = new ArrayList();
    this.m_exIndex = 0;
    this.m_index = 0;
    CommandBinding commandBinding1 = new CommandBinding((ICommand) DomainUpDown.m_downValue);
    commandBinding1.Executed += new ExecutedRoutedEventHandler(this.ChangeDownValue);
    CommandBinding commandBinding2 = new CommandBinding((ICommand) DomainUpDown.m_upValue);
    commandBinding2.Executed += new ExecutedRoutedEventHandler(this.ChangeUpValue);
    this.CommandBindings.Add(commandBinding1);
    this.CommandBindings.Add(commandBinding2);
  }

  public int Add(string item)
  {
    if (this.m_list != null && this.m_list.Count == 0 && this.m_textBox != null)
    {
      int num = this.m_list.Add((object) item);
      this.m_textBox.Text = (string) this.m_list[0];
      return num;
    }
    return this.m_list == null ? 0 : this.m_list.Add((object) item);
  }

  public void RemoveAt(int index)
  {
    if (this.m_list == null || this.m_list.Count <= index)
      return;
    this.m_list.RemoveAt(index);
  }

  public void Remove(string item)
  {
    if (this.m_list == null || item == null || !this.m_list.Contains((object) item))
      return;
    this.m_list.Remove((object) item);
  }

  public void AddRange(string[] range)
  {
    if (this.m_list != null && this.m_list.Count == 0 && this.m_textBox != null)
    {
      if (range != null)
        this.m_list.AddRange((ICollection) range);
      this.m_textBox.Text = (string) this.m_list[0];
    }
    else
    {
      if (this.m_list == null || range == null)
        return;
      this.m_list.AddRange((ICollection) range);
    }
  }

  public event PropertyChangedCallback ValueChanged;

  private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((DomainUpDown) d)?.OnValueChanged(e);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.m_textBox = this.GetTemplateChild("TextBox") as TextBox;
    if (this.m_textBox != null)
    {
      this.m_textBox.FontSize = this.FontSize;
      if (this.m_list != null && this.m_list.Count != 0)
        this.m_textBox.Text = (string) this.m_list[0];
    }
    this.m_firstBlock = this.GetTemplateChild("FirstBlock") as TextBox;
    if (this.m_firstBlock != null)
      this.m_firstBlock.FontSize = this.FontSize;
    this.m_secondBlock = this.GetTemplateChild("SecondBlock") as TextBox;
    if (this.m_secondBlock == null)
      return;
    this.m_secondBlock.FontSize = this.FontSize;
  }

  protected override void OnKeyDown(KeyEventArgs e)
  {
    if (Key.Up == e.Key)
    {
      this.UpdateCounter(true);
      e.Handled = true;
    }
    if (Key.Down == e.Key)
    {
      this.UpdateCounter(false);
      e.Handled = true;
    }
    base.OnKeyDown(e);
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    if (e.Delta > 0)
      this.UpdateCounter(true);
    if (e.Delta < 0)
      this.UpdateCounter(false);
    base.OnMouseWheel(e);
  }

  protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.ValueChanged == null)
      return;
    this.ValueChanged((DependencyObject) this, e);
  }

  private void ChangeDownValue(object sender, ExecutedRoutedEventArgs e)
  {
    this.UpdateCounter(false);
  }

  private void ChangeUpValue(object sender, ExecutedRoutedEventArgs e) => this.UpdateCounter(true);

  private void UpdateCounter(bool isUp)
  {
    if (isUp)
    {
      if (this.m_list != null && this.m_index < this.m_list.Count - 1)
        ++this.m_index;
      else
        this.m_index = 0;
      this.m_isUp = true;
    }
    else
    {
      if (this.m_index > 0)
        --this.m_index;
      else if (this.m_list != null)
        this.m_index = this.m_list.Count - 1;
      this.m_isUp = false;
    }
    if (this.m_list != null && this.m_textBox != null && this.m_list.Count > this.m_exIndex && this.m_textBox.Text != (string) this.m_list[this.m_exIndex] && !this.m_isAnimated)
      this.m_list[this.m_exIndex] = (object) this.m_textBox.Text;
    this.Animation();
  }

  private void Animation()
  {
    if (this.m_isAnimated)
      return;
    if (this.m_textBox != null)
      this.m_textBox.Visibility = Visibility.Hidden;
    if (this.m_firstBlock != null)
      this.m_firstBlock.Visibility = Visibility.Visible;
    if (this.m_secondBlock != null)
      this.m_secondBlock.Visibility = Visibility.Visible;
    DoubleAnimation doubleAnimation = new DoubleAnimation();
    doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
    DoubleAnimation animation = doubleAnimation;
    animation.Completed += new EventHandler(this.Animation_Completed);
    if (this.m_isUp)
    {
      animation.From = new double?(0.0);
      if (this.m_secondBlock != null)
      {
        animation.To = new double?(this.m_secondBlock.ActualHeight * -1.0);
        this.m_secondBlock.Text = (string) this.m_list[this.m_index];
      }
      if (this.m_firstBlock != null)
        this.m_firstBlock.Text = (string) this.m_list[this.m_exIndex];
    }
    else
    {
      if (this.m_secondBlock != null)
      {
        animation.From = new double?(this.m_secondBlock.ActualHeight * -1.0);
        this.m_secondBlock.Text = (string) this.m_list[this.m_exIndex];
      }
      animation.To = new double?(0.0);
      if (this.m_firstBlock != null)
        this.m_firstBlock.Text = (string) this.m_list[this.m_index];
    }
    this.m_isAnimated = true;
    this.m_exIndex = this.m_index;
    this.BeginAnimation(DomainUpDown.AnimationShiftProperty, (AnimationTimeline) animation);
  }

  private void Animation_Completed(object sender, EventArgs e)
  {
    this.m_isAnimated = false;
    if (this.m_list == null)
      return;
    if (this.m_list.Count > this.m_index)
      this.Value = (string) this.m_list[this.m_index];
    if (this.m_secondBlock != null && this.m_firstBlock != null && (this.m_list.Count > this.m_index && (string) this.m_list[this.m_index] != this.m_firstBlock.Text && !this.m_isUp || this.m_list.Count > this.m_index && (string) this.m_list[this.m_index] != this.m_secondBlock.Text && this.m_isUp))
    {
      this.Animation();
    }
    else
    {
      if (this.m_textBox != null)
        this.m_textBox.Visibility = Visibility.Visible;
      if (this.m_firstBlock != null)
        this.m_firstBlock.Visibility = Visibility.Hidden;
      if (this.m_secondBlock == null)
        return;
      this.m_secondBlock.Visibility = Visibility.Hidden;
    }
  }
}
