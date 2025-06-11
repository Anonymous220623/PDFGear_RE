// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Presentation.PdfImageFlipView
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Net;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;

#nullable disable
namespace pdfeditor.Controls.Presentation;

public partial class PdfImageFlipView : UserControl, IComponentConnector
{
  private PdfImageView prevImage;
  private PdfImageView curImage;
  private PdfImageView nextImage;
  private Storyboard curSb;
  public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register(nameof (Document), typeof (PdfDocument), typeof (PdfImageFlipView), new PropertyMetadata((object) null, new PropertyChangedCallback(PdfImageFlipView.OnDocumentPropertyChanged)));
  public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(nameof (PageIndex), typeof (int), typeof (PdfImageFlipView), new PropertyMetadata((object) -1, new PropertyChangedCallback(PdfImageFlipView.OnPageIndexPropertyChanged)));
  internal PdfImageView PdfImageView1;
  internal PdfImageView PdfImageView2;
  internal PdfImageView PdfImageView3;
  private bool _contentLoaded;

  public PdfImageFlipView()
  {
    this.InitializeComponent();
    this.prevImage = this.PdfImageView1;
    this.curImage = this.PdfImageView2;
    this.nextImage = this.PdfImageView3;
    this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.PdfImageFlipView_IsVisibleChanged);
  }

  private void PdfImageFlipView_IsVisibleChanged(
    object sender,
    DependencyPropertyChangedEventArgs e)
  {
    this.UpdateDocument();
  }

  public PdfDocument Document
  {
    get => (PdfDocument) this.GetValue(PdfImageFlipView.DocumentProperty);
    set => this.SetValue(PdfImageFlipView.DocumentProperty, (object) value);
  }

  private static void OnDocumentPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is PdfImageFlipView pdfImageFlipView))
      return;
    pdfImageFlipView.UpdateDocument();
  }

  public int PageIndex
  {
    get => (int) this.GetValue(PdfImageFlipView.PageIndexProperty);
    set => this.SetValue(PdfImageFlipView.PageIndexProperty, (object) value);
  }

  private static void OnPageIndexPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (object.Equals(e.NewValue, e.OldValue) || !(d is PdfImageFlipView pdfImageFlipView))
      return;
    pdfImageFlipView.UpdatePage((int) e.NewValue, (int) e.OldValue);
  }

  private void UpdateDocument()
  {
    this.curSb?.SkipToFill();
    this.curSb = (Storyboard) null;
    this.prevImage.Opacity = 0.0;
    this.curImage.Opacity = 0.0;
    this.nextImage.Opacity = 0.0;
    this.prevImage.Document = this.Document;
    this.curImage.Document = this.Document;
    this.nextImage.Document = this.Document;
    this.prevImage.PageIndex = this.PageIndex - 1;
    this.curImage.PageIndex = this.PageIndex;
    this.nextImage.PageIndex = this.PageIndex + 1;
    Panel.SetZIndex((UIElement) this.prevImage, 0);
    Panel.SetZIndex((UIElement) this.nextImage, 1);
    Panel.SetZIndex((UIElement) this.curImage, 2);
    this.AnimationShow();
  }

  private void UpdatePage(int newPage, int oldPage)
  {
    this.curSb.SkipToFill();
    this.curSb = (Storyboard) null;
    int num = newPage - oldPage;
    if (num == 0 || num > 1 || num < -1)
    {
      this.UpdateDocument();
    }
    else
    {
      PdfImageView curImage = this.curImage;
      Panel.SetZIndex((UIElement) curImage, 1);
      PdfImageView to = (PdfImageView) null;
      switch (num)
      {
        case -1:
          to = this.prevImage;
          PdfImageView nextImage = this.nextImage;
          this.nextImage = this.curImage;
          this.curImage = this.prevImage;
          this.prevImage = nextImage;
          this.prevImage.PageIndex = newPage - 1;
          Panel.SetZIndex((UIElement) this.curImage, 2);
          Panel.SetZIndex((UIElement) this.prevImage, 0);
          break;
        case 1:
          to = this.nextImage;
          PdfImageView prevImage = this.prevImage;
          this.prevImage = this.curImage;
          this.curImage = this.nextImage;
          this.nextImage = prevImage;
          this.nextImage.PageIndex = newPage + 1;
          Panel.SetZIndex((UIElement) this.curImage, 2);
          Panel.SetZIndex((UIElement) this.nextImage, 0);
          break;
      }
      this.AnimationBetween(curImage, to);
    }
  }

  private void AnimationBetween(PdfImageView from, PdfImageView to, TimeSpan? duration = null)
  {
    TimeSpan timeSpan = TimeSpan.FromSeconds(0.2);
    if (duration.HasValue)
      timeSpan = duration.Value;
    to.Opacity = 0.0;
    Storyboard storyboard = new Storyboard();
    DoubleAnimation doubleAnimation = new DoubleAnimation();
    doubleAnimation.From = new double?(0.0);
    doubleAnimation.To = new double?(1.0);
    doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(timeSpan.TotalSeconds));
    DoubleAnimation element = doubleAnimation;
    Storyboard.SetTarget((DependencyObject) element, (DependencyObject) to);
    Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath((object) UIElement.OpacityProperty));
    storyboard.Children.Add((Timeline) element);
    storyboard.Begin();
    this.curSb = storyboard;
  }

  private void AnimationShow(TimeSpan? duration = null)
  {
    TimeSpan _duration = TimeSpan.FromSeconds(0.2);
    if (duration.HasValue)
      _duration = duration.Value;
    Storyboard storyboard1 = new Storyboard();
    storyboard1.Children.Add((Timeline) CreateAnimationCore((DependencyObject) this.PdfImageView1, _duration));
    storyboard1.Children.Add((Timeline) CreateAnimationCore((DependencyObject) this.PdfImageView2, _duration));
    storyboard1.Children.Add((Timeline) CreateAnimationCore((DependencyObject) this.PdfImageView3, _duration));
    Storyboard storyboard2 = storyboard1;
    storyboard2.Begin();
    this.curSb = storyboard2;

    static DoubleAnimation CreateAnimationCore(DependencyObject _obj, TimeSpan _duration)
    {
      DoubleAnimation element = new DoubleAnimation();
      element.From = new double?(0.0);
      element.To = new double?(1.0);
      element.Duration = new Duration(_duration);
      Storyboard.SetTarget((DependencyObject) element, _obj);
      Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath((object) UIElement.OpacityProperty));
      return element;
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/presentation/pdfimageflipview.xaml", UriKind.Relative));
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
        this.PdfImageView1 = (PdfImageView) target;
        break;
      case 2:
        this.PdfImageView2 = (PdfImageView) target;
        break;
      case 3:
        this.PdfImageView3 = (PdfImageView) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
