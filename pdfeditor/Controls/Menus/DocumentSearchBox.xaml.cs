// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.DocumentSearchBox
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models.Menus;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Menus;

public partial class DocumentSearchBox : UserControl, IComponentConnector
{
  public static readonly DependencyProperty SearchModelProperty = DependencyProperty.Register(nameof (SearchModel), typeof (SearchModel), typeof (DocumentSearchBox), new PropertyMetadata((object) null, new PropertyChangedCallback(DocumentSearchBox.OnSearchModelPropertyChanged)));
  private static Action<Button> invokeFunc;
  internal Grid SearchContainer;
  internal VisualStateGroup SearchVisibleStates;
  internal VisualState SearchVisible;
  internal VisualState SearchInvisible;
  internal DoubleAnimation HideSearchAnimation2;
  internal DoubleAnimation ShowSearchAnimation;
  internal DoubleAnimation HideSearchAnimation;
  internal Grid SearchContentLayoutRoot;
  internal TranslateTransform SearchTrans;
  internal Rectangle ProgressBorder;
  internal RectangleGeometry ProgressClip;
  internal TextBox SearchTextBox;
  internal StackPanel SearchCommandPanel;
  internal StackPanel RecordCountContainer;
  internal Button PrevBtn;
  internal Button NextBtn;
  internal Button CancelBtn;
  private bool _contentLoaded;

  public DocumentSearchBox()
  {
    this.InitializeComponent();
    this.UpdateVisibleState(true);
    this.UpdateRecordCountVisibility();
    this.UpdateProgress();
  }

  public SearchModel SearchModel
  {
    get => (SearchModel) this.GetValue(DocumentSearchBox.SearchModelProperty);
    set => this.SetValue(DocumentSearchBox.SearchModelProperty, (object) value);
  }

  private static void OnSearchModelPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is DocumentSearchBox documentSearchBox))
      return;
    if (e.OldValue is INotifyPropertyChanged oldValue)
      WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.RemoveHandler(oldValue, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(documentSearchBox.OnSearchModelPropertyChanged));
    if (e.NewValue is INotifyPropertyChanged newValue)
      WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler(newValue, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(documentSearchBox.OnSearchModelPropertyChanged));
    documentSearchBox.UpdateVisibleState();
    documentSearchBox.UpdateRecordCountVisibility();
    documentSearchBox.UpdateProgress();
  }

  private void OnSearchModelPropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (e.PropertyName == "IsSearchVisible")
      this.UpdateVisibleState();
    else if (e.PropertyName == "SearchText")
      this.UpdateRecordCountVisibility();
    else if (e.PropertyName == "TotalRecords")
    {
      this.UpdateRecordCountVisibility();
    }
    else
    {
      if (!(e.PropertyName == "Progress"))
        return;
      this.UpdateProgress();
    }
  }

  private void UpdateVisibleState(bool disableAnimation = false)
  {
    bool flag = false;
    if (this.SearchModel != null && this.SearchModel.IsSearchEnabled && this.SearchModel.IsSearchVisible)
      flag = true;
    if (DesignerProperties.GetIsInDesignMode((DependencyObject) this))
      flag = true;
    if (!(VisualStateManager.GoToElementState((FrameworkElement) this.Content, flag ? "SearchVisible" : "SearchInvisible", !disableAnimation) & flag))
      return;
    this.FocusTextBox();
  }

  private void UpdateRecordCountVisibility()
  {
    Visibility visibility = Visibility.Collapsed;
    if (this.SearchModel != null)
    {
      if (this.SearchModel.TotalRecords > 0)
        visibility = Visibility.Visible;
      else if (this.SearchModel.TotalRecords == 0 && !string.IsNullOrEmpty(this.SearchModel.SearchText))
        visibility = Visibility.Visible;
    }
    this.RecordCountContainer.Visibility = visibility;
  }

  private void SearchContainer_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.ShowSearchAnimation.From = new double?(-e.NewSize.Height);
    this.HideSearchAnimation.To = new double?(-e.NewSize.Height);
    this.HideSearchAnimation2.To = new double?(-e.NewSize.Height);
  }

  private void SearchCommandPanel_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    Thickness padding = this.SearchTextBox.Padding;
    this.SearchTextBox.Padding = new Thickness(padding.Left, padding.Top, e.NewSize.Width, padding.Bottom);
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e) => this.Hide();

  private void Hide()
  {
    if (this.SearchModel == null)
      return;
    this.SearchModel.SearchText = "";
    this.SearchModel.IsSearchVisible = false;
  }

  private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key != Key.Return)
      return;
    e.Handled = true;
    if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None)
      this.InvokeButton(this.NextBtn);
    else
      this.InvokeButton(this.PrevBtn);
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    base.OnPreviewKeyDown(e);
    if (e.Key != Key.Escape)
      return;
    e.Handled = true;
    this.Hide();
  }

  protected override void OnGotFocus(RoutedEventArgs e)
  {
    base.OnGotFocus(e);
    if (e.OriginalSource != this)
      return;
    e.Handled = true;
    this.FocusTextBox();
  }

  private void FocusTextBox(bool selectAll = true)
  {
    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() =>
    {
      this.SearchTextBox.Focus();
      Keyboard.Focus((IInputElement) this.SearchTextBox);
      if (!selectAll || string.IsNullOrEmpty(this.SearchTextBox.Text))
        return;
      this.SearchTextBox.SelectAll();
    }));
  }

  private void InvokeButton(Button button)
  {
    if (button == null)
      return;
    DocumentSearchBox.InvokeCore(button);
  }

  private static void InvokeCore(Button button)
  {
    if (button == null)
      return;
    if (DocumentSearchBox.invokeFunc == null)
    {
      lock (typeof (ButtonAutomationPeer))
      {
        if (DocumentSearchBox.invokeFunc == null)
        {
          Type type1 = typeof (ButtonAutomationPeer);
          Type type2 = type1.GetInterface("IInvokeProvider");
          MethodInfo method = type2?.GetMethod("Invoke");
          if (method == (MethodInfo) null)
          {
            DocumentSearchBox.invokeFunc = (Action<Button>) (p =>
            {
              if (p == null || !p.IsEnabled)
                return;
              if (p.Command != null)
                p.Command.Execute(p.CommandParameter);
              p.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            });
          }
          else
          {
            ParameterExpression parameterExpression = System.Linq.Expressions.Expression.Parameter(type1, "p");
            Action<ButtonAutomationPeer> func = System.Linq.Expressions.Expression.Lambda<Action<ButtonAutomationPeer>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Convert((System.Linq.Expressions.Expression) parameterExpression, type2), method), parameterExpression).Compile();
            DocumentSearchBox.invokeFunc = (Action<Button>) (btn =>
            {
              if (btn == null || !btn.IsEnabled)
                return;
              if (!(UIElementAutomationPeer.CreatePeerForElement((UIElement) btn) is ButtonAutomationPeer peerForElement2))
                return;
              try
              {
                func(peerForElement2);
              }
              catch
              {
              }
            });
          }
        }
      }
    }
    DocumentSearchBox.invokeFunc(button);
  }

  private void SearchContentLayoutRoot_MouseUp(object sender, MouseButtonEventArgs e)
  {
    e.Handled = true;
    this.FocusTextBox(false);
  }

  private void UpdateProgress()
  {
    double num = 0.0;
    if (this.SearchModel != null)
      num = this.SearchModel.Progress;
    this.ProgressClip.Rect = new Rect(0.0, 0.0, num * this.ProgressBorder.ActualWidth, this.ProgressBorder.ActualHeight);
  }

  public void CloseSearchBox()
  {
    if (!this.SearchModel.IsSearchVisible)
      return;
    this.Hide();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/menus/documentsearchbox.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.SearchContainer = (Grid) target;
        this.SearchContainer.SizeChanged += new SizeChangedEventHandler(this.SearchContainer_SizeChanged);
        break;
      case 2:
        this.SearchVisibleStates = (VisualStateGroup) target;
        break;
      case 3:
        this.SearchVisible = (VisualState) target;
        break;
      case 4:
        this.SearchInvisible = (VisualState) target;
        break;
      case 5:
        this.HideSearchAnimation2 = (DoubleAnimation) target;
        break;
      case 6:
        this.ShowSearchAnimation = (DoubleAnimation) target;
        break;
      case 7:
        this.HideSearchAnimation = (DoubleAnimation) target;
        break;
      case 8:
        this.SearchContentLayoutRoot = (Grid) target;
        this.SearchContentLayoutRoot.MouseUp += new MouseButtonEventHandler(this.SearchContentLayoutRoot_MouseUp);
        break;
      case 9:
        this.SearchTrans = (TranslateTransform) target;
        break;
      case 10:
        this.ProgressBorder = (Rectangle) target;
        break;
      case 11:
        this.ProgressClip = (RectangleGeometry) target;
        break;
      case 12:
        this.SearchTextBox = (TextBox) target;
        this.SearchTextBox.KeyDown += new KeyEventHandler(this.SearchTextBox_KeyDown);
        break;
      case 13:
        this.SearchCommandPanel = (StackPanel) target;
        this.SearchCommandPanel.SizeChanged += new SizeChangedEventHandler(this.SearchCommandPanel_SizeChanged);
        break;
      case 14:
        this.RecordCountContainer = (StackPanel) target;
        break;
      case 15:
        this.PrevBtn = (Button) target;
        break;
      case 16 /*0x10*/:
        this.NextBtn = (Button) target;
        break;
      case 17:
        this.CancelBtn = (Button) target;
        this.CancelBtn.Click += new RoutedEventHandler(this.CancelButton_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
