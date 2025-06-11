// Decompiled with JetBrains decompiler
// Type: WpfToolkit.Controls.GridDetailsView
// Assembly: VirtualizingWrapPanel, Version=1.5.4.0, Culture=neutral, PublicKeyToken=null
// MVID: E61E2A8E-A00C-4FB4-9D6E-5B7404CFB214
// Assembly location: D:\PDFGear\bin\VirtualizingWrapPanel.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable enable
namespace WpfToolkit.Controls;

public partial class GridDetailsView : GridView, IComponentConnector, IStyleConnector
{
  public static readonly DependencyProperty ExpandedItemTemplateProperty = DependencyProperty.Register(nameof (ExpandedItemTemplate), typeof (DataTemplate), typeof (GridDetailsView), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ExpandedItemProperty = DependencyProperty.Register(nameof (ExpandedItem), typeof (object), typeof (GridDetailsView), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
  private FrameworkElement? expandedItemContainerRoot;
  private bool animateExpansion;
  private bool animateCloseExpansion;
  internal 
  #nullable disable
  GridDetailsView uc;
  private bool _contentLoaded;

  public 
  #nullable enable
  DataTemplate? ExpandedItemTemplate
  {
    get => (DataTemplate) this.GetValue(GridDetailsView.ExpandedItemTemplateProperty);
    set => this.SetValue(GridDetailsView.ExpandedItemTemplateProperty, (object) value);
  }

  public object? ExpandedItem
  {
    get => this.GetValue(GridDetailsView.ExpandedItemProperty);
    private set => this.SetValue(GridDetailsView.ExpandedItemProperty, value);
  }

  public GridDetailsView() => this.InitializeComponent();

  protected override DependencyObject GetContainerForItemOverride()
  {
    FrameworkElement containerForItemOverride = (FrameworkElement) base.GetContainerForItemOverride();
    containerForItemOverride.PreviewMouseDown += new MouseButtonEventHandler(this.Container_PreviewMouseDown);
    return (DependencyObject) containerForItemOverride;
  }

  private async void Container_PreviewMouseDown(object sender, MouseButtonEventArgs args)
  {
    if (args.LeftButton != MouseButtonState.Pressed)
      return;
    object dataContext = ((FrameworkElement) sender).DataContext;
    if (dataContext != this.ExpandedItem)
    {
      this.ExpandedItem = dataContext;
    }
    else
    {
      this.animateExpansion = false;
      this.animateCloseExpansion = true;
      if (this.MaxContainerSize == double.PositiveInfinity)
        this.MaxContainerSize = this.DesiredContainerSize;
      double sourceHeight = this.MaxContainerSize;
      for (int i = 20; i >= 0; --i)
      {
        if (!this.animateCloseExpansion)
          return;
        this.MaxContainerSize = sourceHeight / 20.0 * (double) i;
        if (i != 0)
          await Task.Delay(15);
      }
      this.expandedItemContainerRoot = (FrameworkElement) null;
      this.ExpandedItem = (object) null;
      this.animateCloseExpansion = false;
    }
  }

  private async void ExpandedItemContainerRoot_Loaded(object sender, RoutedEventArgs args)
  {
    this.animateCloseExpansion = false;
    if (this.expandedItemContainerRoot == null)
    {
      this.expandedItemContainerRoot = (FrameworkElement) sender;
      this.MaxContainerSize = 0.0;
      double targetHeight = this.DesiredContainerSize;
      this.animateExpansion = true;
      for (int i = 0; i <= 20; ++i)
      {
        if (!this.animateExpansion)
          return;
        this.MaxContainerSize = targetHeight / 20.0 * (double) i;
        if (i != 20)
          await Task.Delay(15);
      }
      this.MaxContainerSize = double.PositiveInfinity;
      this.animateExpansion = false;
    }
    else
    {
      this.expandedItemContainerRoot = (FrameworkElement) sender;
      this.MaxContainerSize = double.PositiveInfinity;
    }
  }

  private double DesiredContainerSize
  {
    get
    {
      if (this.expandedItemContainerRoot == null)
        throw new NullReferenceException("expandedItemContainerRoot is null");
      return this.Orientation == Orientation.Vertical ? this.expandedItemContainerRoot.DesiredSize.Height : this.expandedItemContainerRoot.DesiredSize.Width;
    }
  }

  private double MaxContainerSize
  {
    get
    {
      if (this.expandedItemContainerRoot == null)
        throw new NullReferenceException("expandedItemContainerRoot is null");
      return this.Orientation == Orientation.Vertical ? this.expandedItemContainerRoot.MaxHeight : this.expandedItemContainerRoot.MaxWidth;
    }
    set
    {
      if (this.expandedItemContainerRoot == null)
        throw new NullReferenceException("expandedItemContainerRoot is null");
      if (this.Orientation == Orientation.Vertical)
        this.expandedItemContainerRoot.MaxHeight = value;
      else
        this.expandedItemContainerRoot.MaxWidth = value;
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "5.0.4.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/VirtualizingWrapPanel;component/griddetailsview.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "5.0.4.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, 
  #nullable disable
  object target)
  {
    if (connectionId == 1)
      this.uc = (GridDetailsView) target;
    else
      this._contentLoaded = true;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "5.0.4.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IStyleConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 2)
      return;
    ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.ExpandedItemContainerRoot_Loaded);
  }
}
