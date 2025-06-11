// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.BlendEffectBox
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Effects;

#nullable disable
namespace HandyControl.Controls;

[DefaultProperty("Content")]
[System.Windows.Markup.ContentProperty("Content")]
public class BlendEffectBox : Control
{
  private readonly ContentPresenter _effectBottomPresenter;
  private readonly ContentPresenter _effectTopPresenter;
  private bool _isInternalAction;
  public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof (Content), typeof (FrameworkElement), typeof (BlendEffectBox), new PropertyMetadata((object) null));
  internal static readonly DependencyProperty ActualContentProperty = DependencyProperty.Register(nameof (ActualContent), typeof (FrameworkElement), typeof (BlendEffectBox), new PropertyMetadata((object) null));

  public BlendEffectBox()
  {
    this._effectTopPresenter = new ContentPresenter();
    this.ActualContent = (FrameworkElement) this._effectTopPresenter;
    this._effectBottomPresenter = new ContentPresenter();
    this._effectBottomPresenter.SetBinding(ContentPresenter.ContentProperty, (BindingBase) new Binding(BlendEffectBox.ContentProperty.Name)
    {
      Source = (object) this
    });
    ObservableCollection<Effect> observableCollection = new ObservableCollection<Effect>();
    observableCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnEffectsChanged);
    this.Effects = (Collection<Effect>) observableCollection;
  }

  private void OnEffectsChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (this.Effects == null || this.Effects.Count == 0)
    {
      this.ClearEffect(this._effectTopPresenter);
    }
    else
    {
      if (this._isInternalAction)
        return;
      this._isInternalAction = true;
      this.AddEffect(this._effectTopPresenter, this.Effects.Count);
      this._isInternalAction = false;
    }
  }

  private void ClearEffect(ContentPresenter presenter)
  {
    if (presenter == null)
      return;
    if (this._effectBottomPresenter == presenter)
    {
      this._effectBottomPresenter.SetCurrentValue(UIElement.EffectProperty, (object) null);
    }
    else
    {
      presenter.SetCurrentValue(UIElement.EffectProperty, (object) null);
      this.ClearEffect(presenter.Content as ContentPresenter);
    }
  }

  private void AddEffect(ContentPresenter presenter, int count)
  {
    int num1 = --count;
    if (num1 < 0)
      return;
    presenter.Effect = this.Effects[num1];
    int num2 = --count;
    if (num2 >= 1)
    {
      ContentPresenter presenter1 = new ContentPresenter();
      presenter.Content = (object) presenter1;
      this.AddEffect(presenter1, num1);
    }
    else if (num2 >= 0)
    {
      this._effectBottomPresenter.Effect = this.Effects[0];
      presenter.Content = (object) this._effectBottomPresenter;
    }
    else
      presenter.Content = (object) this._effectBottomPresenter;
  }

  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  [Bindable(true)]
  public Collection<Effect> Effects { get; }

  public FrameworkElement Content
  {
    get => (FrameworkElement) this.GetValue(BlendEffectBox.ContentProperty);
    set => this.SetValue(BlendEffectBox.ContentProperty, (object) value);
  }

  internal FrameworkElement ActualContent
  {
    get => (FrameworkElement) this.GetValue(BlendEffectBox.ActualContentProperty);
    set => this.SetValue(BlendEffectBox.ActualContentProperty, (object) value);
  }
}
