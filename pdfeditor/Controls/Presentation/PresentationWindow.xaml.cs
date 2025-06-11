// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Presentation.PresentationWindow
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Patagames.Pdf.Net;
using pdfeditor.Utils;
using pdfeditor.Views;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace pdfeditor.Controls.Presentation;

public partial class PresentationWindow : Window, IComponentConnector
{
  private readonly PdfDocument doc;
  private bool closing;
  private Storyboard toolbarSb;
  private long lastShowToolbarTicks;
  private System.Windows.Point lastShowToolbarMousePosition;
  private bool shouldCloseWhenDeactivated = true;
  internal Grid MainGrid;
  internal TranslateTransform ImageViewTrans;
  internal PdfImageFlipView ImageView;
  internal Grid TopToolbarContainer;
  internal System.Windows.Controls.Button ExitButton;
  internal TextBlock FileNameText;
  internal System.Windows.Controls.Button ExitButton2;
  internal Border BottomFloatToolbarContainer;
  internal TextBlock PageText;
  internal System.Windows.Controls.Button FloatPrevButton;
  internal System.Windows.Controls.Button FloatNextButton;
  private bool _contentLoaded;

  public PresentationWindow(PdfDocument doc, string fileName)
  {
    this.InitializeComponent();
    this.doc = doc;
    this.FileNameText.Text = fileName?.Trim() ?? "";
    this.ImageView.Document = doc;
    this.ImageView.PageIndex = 1;
    this.ImageView.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.ImageView_PreviewMouseLeftButtonDown);
    this.Loaded += new RoutedEventHandler(this.PresentationWindow_Loaded);
    this.SizeChanged += new SizeChangedEventHandler(this.PresentationWindow_SizeChanged);
    this.StateChanged += new EventHandler(this.PresentationWindow_StateChanged);
    this.ExitButton.Click += (RoutedEventHandler) ((s, a) => this.Close());
    this.ExitButton2.Click += (RoutedEventHandler) ((s, a) => this.Close());
    this.FloatPrevButton.Click += (RoutedEventHandler) ((s, a) =>
    {
      this.ShowToolbar();
      this.PrevPage();
    });
    this.FloatNextButton.Click += (RoutedEventHandler) ((s, a) =>
    {
      this.ShowToolbar();
      this.NextPage();
    });
    this.UpdatePageIndexText();
  }

  protected override async void OnSourceInitialized(EventArgs e)
  {
    PresentationWindow presentationWindow = this;
    // ISSUE: reference to a compiler-generated method
    presentationWindow.\u003C\u003En__0(e);
    HwndSource hwndSource = (HwndSource) PresentationSource.FromVisual((Visual) presentationWindow);
    MainView mainView = App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
    if (Screen.AllScreens.Length > 1)
    {
      presentationWindow.shouldCloseWhenDeactivated = false;
      HandleRef hMonitor;
      Int32Rect workArea;
      if (mainView != null && ScreenUtils.TryGetMonitorForWindow((Window) mainView, out hMonitor) && ScreenUtils.GetMonitorInfo(hMonitor.Handle, out Int32Rect _, out workArea, out bool _))
      {
        System.Drawing.Point centerWorkArea = new System.Drawing.Point(workArea.X + workArea.Width / 2, workArea.Y + workArea.Height / 2);
        FullScreenHelper.SetWindowPositionAndSize(hwndSource.Handle, new System.Drawing.Point?(centerWorkArea), new System.Drawing.Size?());
        await presentationWindow.Dispatcher.InvokeAsync((Action) (() => FullScreenHelper.SetWindowPositionAndSize(hwndSource.Handle, new System.Drawing.Point?(centerWorkArea), new System.Drawing.Size?())));
      }
    }
    mainView?.Hide();
    FullScreenHelper.SetIsFullScreenEnabled((Window) presentationWindow, true);
    hwndSource.AddHook(new HwndSourceHook(presentationWindow.WndProc));
    mainView = (MainView) null;
  }

  private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
  {
    if (msg == 126 && !this.closing)
      this.Close();
    return IntPtr.Zero;
  }

  private void PresentationWindow_StateChanged(object sender, EventArgs e)
  {
    if (this.WindowState != WindowState.Maximized)
      return;
    this.ImageView.Visibility = Visibility.Visible;
  }

  private void PresentationWindow_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    System.Windows.Point pixelOffset = PresentationWindow.GetPixelOffset((UIElement) this.MainGrid);
    this.ImageViewTrans.X = pixelOffset.X;
    this.ImageViewTrans.Y = pixelOffset.Y;
  }

  private void PresentationWindow_Loaded(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent("Present", "Show", "Count", 1L);
    this.ImageView.Opacity = 1.0;
  }

  public int PageIndex
  {
    get => this.ImageView.PageIndex;
    set
    {
      if (this.ImageView.PageIndex == value)
        return;
      this.ImageView.PageIndex = value;
      this.UpdatePageIndexText();
    }
  }

  protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
  {
    base.OnKeyDown(e);
    if (e.Key == Key.Escape)
    {
      if (this.closing)
        return;
      this.Close();
    }
    else if (e.Key == Key.Left || e.Key == Key.Up || e.Key == Key.Prior)
      this.PrevPage();
    else if (e.Key == Key.Right || e.Key == Key.Down || e.Key == Key.Return || e.Key == Key.Next)
      this.NextPage();
    else if (e.Key == Key.Home)
    {
      this.PageIndex = 0;
    }
    else
    {
      if (e.Key != Key.End)
        return;
      this.PageIndex = this.doc.Pages.Count - 1;
    }
  }

  private void ImageView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (e.ChangedButton != MouseButton.Left)
      return;
    this.NextPage();
  }

  protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
  {
    base.OnPreviewMouseWheel(e);
    e.Handled = true;
    if (e.Delta > 0)
      this.PrevPage();
    else
      this.NextPage();
  }

  protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
  {
    base.OnMouseMove(e);
    System.Windows.Point position = e.GetPosition((IInputElement) this);
    if (Math.Abs(this.lastShowToolbarMousePosition.X - position.X) <= 10.0 && Math.Abs(this.lastShowToolbarMousePosition.Y - position.Y) <= 10.0)
      return;
    this.lastShowToolbarMousePosition = position;
    this.ShowToolbar();
  }

  private void PrevPage()
  {
    if (this.PageIndex <= 0)
      return;
    --this.PageIndex;
  }

  private void NextPage()
  {
    if (this.PageIndex >= this.doc.Pages.Count - 1)
      return;
    ++this.PageIndex;
  }

  private void UpdatePageIndexText()
  {
    int count = this.doc.Pages.Count;
    int pageIndex = this.PageIndex;
    bool flag1 = pageIndex > 0;
    bool flag2 = pageIndex < count - 1;
    this.PageText.Text = $"{pageIndex + 1} / {count}";
    this.FloatPrevButton.IsEnabled = flag1;
    this.FloatNextButton.IsEnabled = flag2;
  }

  private async Task CloseCoreAsync()
  {
    try
    {
      MainView mainView = App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
      if (mainView != null)
      {
        await Task.Delay(100);
        mainView.Show();
        mainView.Activate();
      }
      mainView = (MainView) null;
    }
    catch
    {
    }
  }

  protected override void OnActivated(EventArgs e)
  {
    base.OnActivated(e);
    this.ImageView.Focus();
  }

  protected override void OnDeactivated(EventArgs e)
  {
    base.OnDeactivated(e);
    if (!this.shouldCloseWhenDeactivated || this.closing)
      return;
    this.Close();
  }

  protected override async void OnClosed(EventArgs e)
  {
    base.OnClosed(e);
    await this.CloseCoreAsync();
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    this.closing = true;
    base.OnClosing(e);
  }

  private void ShowToolbar()
  {
    long timestamp = Stopwatch.GetTimestamp();
    if (timestamp - this.lastShowToolbarTicks < 10000000L)
      return;
    this.lastShowToolbarTicks = timestamp;
    this.toolbarSb?.Stop();
    TimeSpan timeSpan = TimeSpan.FromSeconds(3.0);
    if (this.toolbarSb == null)
    {
      Storyboard storyboard = new Storyboard();
      DoubleAnimationUsingKeyFrames element1 = new DoubleAnimationUsingKeyFrames()
      {
        KeyFrames = {
          (DoubleKeyFrame) new LinearDoubleKeyFrame(1.0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2))),
          (DoubleKeyFrame) new DiscreteDoubleKeyFrame(1.0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2) + timeSpan)),
          (DoubleKeyFrame) new LinearDoubleKeyFrame(0.0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.4) + timeSpan))
        }
      };
      Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath((object) UIElement.OpacityProperty));
      DoubleAnimationUsingKeyFrames element2 = element1.Clone();
      Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) this.TopToolbarContainer);
      Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) this.BottomFloatToolbarContainer);
      storyboard.Children.Add((Timeline) element1);
      storyboard.Children.Add((Timeline) element2);
      this.toolbarSb = storyboard;
    }
    this.toolbarSb.Begin();
  }

  private static System.Windows.Point GetPixelOffset(UIElement UI)
  {
    System.Windows.Point point1 = new System.Windows.Point();
    PresentationSource presentationSource = PresentationSource.FromVisual((Visual) UI);
    if (presentationSource != null)
    {
      Visual rootVisual = presentationSource.RootVisual;
      System.Windows.Point point2 = PresentationWindow.ApplyVisualTransform(UI.TransformToAncestor(rootVisual).Transform(point1), rootVisual, false);
      point1 = presentationSource.CompositionTarget.TransformToDevice.Transform(point2);
      point1.X = Math.Round(point1.X);
      point1.Y = Math.Round(point1.Y);
      point1 = presentationSource.CompositionTarget.TransformFromDevice.Transform(point1);
      point1 = PresentationWindow.ApplyVisualTransform(point1, rootVisual, true);
      GeneralTransform descendant = rootVisual.TransformToDescendant((Visual) UI);
      if (descendant != null)
        point1 = descendant.Transform(point1);
    }
    return point1;
  }

  private static System.Windows.Point ApplyVisualTransform(System.Windows.Point point, Visual v, bool inverse)
  {
    bool success = true;
    return PresentationWindow.TryApplyVisualTransform(point, v, inverse, true, out success);
  }

  private static System.Windows.Point TryApplyVisualTransform(
    System.Windows.Point point,
    Visual v,
    bool inverse,
    bool throwOnError,
    out bool success)
  {
    success = true;
    if (v != null)
    {
      Matrix visualTransform = PresentationWindow.GetVisualTransform(v);
      if (inverse)
      {
        if (!throwOnError && !visualTransform.HasInverse)
        {
          success = false;
          return new System.Windows.Point(0.0, 0.0);
        }
        visualTransform.Invert();
      }
      point = visualTransform.Transform(point);
    }
    return point;
  }

  private static Matrix GetVisualTransform(Visual v)
  {
    if (v == null)
      return Matrix.Identity;
    Matrix trans1 = Matrix.Identity;
    Transform transform = VisualTreeHelper.GetTransform(v);
    if (transform != null)
    {
      Matrix trans2 = transform.Value;
      trans1 = Matrix.Multiply(trans1, trans2);
    }
    Vector offset = VisualTreeHelper.GetOffset(v);
    trans1.Translate(offset.X, offset.Y);
    return trans1;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    System.Windows.Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/presentation/presentationwindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.MainGrid = (Grid) target;
        break;
      case 2:
        this.ImageViewTrans = (TranslateTransform) target;
        break;
      case 3:
        this.ImageView = (PdfImageFlipView) target;
        break;
      case 4:
        this.TopToolbarContainer = (Grid) target;
        break;
      case 5:
        this.ExitButton = (System.Windows.Controls.Button) target;
        break;
      case 6:
        this.FileNameText = (TextBlock) target;
        break;
      case 7:
        this.ExitButton2 = (System.Windows.Controls.Button) target;
        break;
      case 8:
        this.BottomFloatToolbarContainer = (Border) target;
        break;
      case 9:
        this.PageText = (TextBlock) target;
        break;
      case 10:
        this.FloatPrevButton = (System.Windows.Controls.Button) target;
        break;
      case 11:
        this.FloatNextButton = (System.Windows.Controls.Button) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
