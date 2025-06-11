// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ImageBlock
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

#nullable disable
namespace HandyControl.Controls;

public class ImageBlock : FrameworkElement
{
  private readonly DispatcherTimer _dispatcherTimer;
  private BitmapSource _source;
  private int _indexMax;
  private int _indexMin;
  private int _currentIndex;
  private int _blockWidth;
  private int _blockHeight;
  private bool _isDisposed;
  private int _columns = 1;
  public static readonly DependencyProperty StartColumnProperty = DependencyProperty.Register(nameof (StartColumn), typeof (int), typeof (ImageBlock), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Int0Box, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ImageBlock.OnPositionsChanged)));
  public static readonly DependencyProperty StartRowProperty = DependencyProperty.Register(nameof (StartRow), typeof (int), typeof (ImageBlock), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Int0Box, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ImageBlock.OnPositionsChanged)));
  public static readonly DependencyProperty EndColumnProperty = DependencyProperty.Register(nameof (EndColumn), typeof (int), typeof (ImageBlock), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Int0Box, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ImageBlock.OnPositionsChanged)));
  public static readonly DependencyProperty EndRowProperty = DependencyProperty.Register(nameof (EndRow), typeof (int), typeof (ImageBlock), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Int0Box, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ImageBlock.OnPositionsChanged)));
  public static readonly DependencyProperty IsPlayingProperty = DependencyProperty.Register(nameof (IsPlaying), typeof (bool), typeof (ImageBlock), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ImageBlock.OnIsPlayingChanged)));
  public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(nameof (Columns), typeof (int), typeof (ImageBlock), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Int1Box, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ImageBlock.OnPositionsChanged)), (ValidateValueCallback) (obj => (int) obj >= 1));
  public static readonly DependencyProperty RowsProperty = DependencyProperty.Register(nameof (Rows), typeof (int), typeof (ImageBlock), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Int1Box, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ImageBlock.OnPositionsChanged)), (ValidateValueCallback) (obj => (int) obj >= 1));
  public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof (Interval), typeof (TimeSpan), typeof (ImageBlock), new PropertyMetadata((object) TimeSpan.FromSeconds(1.0), new PropertyChangedCallback(ImageBlock.OnIntervalChanged)));
  public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof (Source), typeof (ImageSource), typeof (ImageBlock), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ImageBlock.OnSourceChanged)));

  public ImageBlock()
  {
    this._dispatcherTimer = new DispatcherTimer(DispatcherPriority.Render)
    {
      Interval = this.Interval
    };
    this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.ImageBlock_IsVisibleChanged);
  }

  ~ImageBlock() => this.Dispose();

  public void Dispose()
  {
    if (this._isDisposed)
      return;
    this.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.ImageBlock_IsVisibleChanged);
    this._dispatcherTimer.Stop();
    this._isDisposed = true;
    GC.SuppressFinalize((object) this);
  }

  private void ImageBlock_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    if (this.IsVisible)
    {
      this._dispatcherTimer.Tick += new EventHandler(this.DispatcherTimer_Tick);
      if (!this.IsPlaying)
        return;
      this._dispatcherTimer.Start();
    }
    else
    {
      this._dispatcherTimer.Stop();
      this._dispatcherTimer.Tick -= new EventHandler(this.DispatcherTimer_Tick);
    }
  }

  private void UpdateDatas()
  {
    if (this._source == null)
      return;
    this._indexMin = this.StartRow * this._columns + this.StartColumn;
    this._indexMax = this.EndRow * this._columns + this.EndColumn;
    this._currentIndex = this._indexMin;
    this._blockWidth = this._source.PixelWidth / this._columns;
    this._blockHeight = this._source.PixelHeight / this.Rows;
  }

  private static void OnPositionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ImageBlock imageBlock = (ImageBlock) d;
    if (e.Property == ImageBlock.ColumnsProperty)
      imageBlock._columns = (int) e.NewValue;
    imageBlock.UpdateDatas();
  }

  private void DispatcherTimer_Tick(object sender, EventArgs e) => this.InvalidateVisual();

  public int StartColumn
  {
    get => (int) this.GetValue(ImageBlock.StartColumnProperty);
    set => this.SetValue(ImageBlock.StartColumnProperty, (object) value);
  }

  public int StartRow
  {
    get => (int) this.GetValue(ImageBlock.StartRowProperty);
    set => this.SetValue(ImageBlock.StartRowProperty, (object) value);
  }

  public int EndColumn
  {
    get => (int) this.GetValue(ImageBlock.EndColumnProperty);
    set => this.SetValue(ImageBlock.EndColumnProperty, (object) value);
  }

  public int EndRow
  {
    get => (int) this.GetValue(ImageBlock.EndRowProperty);
    set => this.SetValue(ImageBlock.EndRowProperty, (object) value);
  }

  private static void OnIsPlayingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ImageBlock imageBlock = (ImageBlock) d;
    if ((bool) e.NewValue)
      imageBlock._dispatcherTimer.Start();
    else
      imageBlock._dispatcherTimer.Stop();
  }

  public bool IsPlaying
  {
    get => (bool) this.GetValue(ImageBlock.IsPlayingProperty);
    set => this.SetValue(ImageBlock.IsPlayingProperty, ValueBoxes.BooleanBox(value));
  }

  public int Columns
  {
    get => (int) this.GetValue(ImageBlock.ColumnsProperty);
    set => this.SetValue(ImageBlock.ColumnsProperty, (object) value);
  }

  public int Rows
  {
    get => (int) this.GetValue(ImageBlock.RowsProperty);
    set => this.SetValue(ImageBlock.RowsProperty, (object) value);
  }

  private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ImageBlock) d)._dispatcherTimer.Interval = (TimeSpan) e.NewValue;
  }

  public TimeSpan Interval
  {
    get => (TimeSpan) this.GetValue(ImageBlock.IntervalProperty);
    set => this.SetValue(ImageBlock.IntervalProperty, (object) value);
  }

  private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ImageBlock imageBlock = (ImageBlock) d;
    imageBlock._source = e.NewValue as BitmapSource;
    imageBlock.UpdateDatas();
  }

  public ImageSource Source
  {
    get => (ImageSource) this.GetValue(ImageBlock.SourceProperty);
    set => this.SetValue(ImageBlock.SourceProperty, (object) value);
  }

  protected override void OnRender(DrawingContext dc)
  {
    if (this._source == null)
      return;
    CroppedBitmap croppedBitmap1 = new CroppedBitmap(this._source, this.CalDisplayRect());
    DrawingContext drawingContext = dc;
    CroppedBitmap croppedBitmap2 = croppedBitmap1;
    Size renderSize = this.RenderSize;
    double width = renderSize.Width;
    renderSize = this.RenderSize;
    double height = renderSize.Height;
    Rect rectangle = new Rect(0.0, 0.0, width, height);
    drawingContext.DrawImage((ImageSource) croppedBitmap2, rectangle);
  }

  private Int32Rect CalDisplayRect()
  {
    if (this._currentIndex > this._indexMax)
      this._currentIndex = this._indexMin;
    Int32Rect int32Rect = new Int32Rect(this._currentIndex % this._columns * this._blockWidth, this._currentIndex / this._columns * this._blockHeight, this._blockWidth, this._blockHeight);
    ++this._currentIndex;
    return int32Rect;
  }
}
