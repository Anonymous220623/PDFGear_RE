// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.GifImage
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace HandyControl.Controls;

public class GifImage : Image, IDisposable
{
  private static readonly Guid GifGuid = new Guid("{b96b3caa-0728-11d3-9d7b-0000f81ef32e}");
  private static readonly Guid GifSingleFrameGuid = new Guid("{b96b3cb0-0728-11d3-9d7b-0000f81ef32e}");
  internal IntPtr NativeImage;
  private byte[] _rawData;
  private bool _isStart;
  private bool _isLoaded;
  public static readonly DependencyProperty UriProperty = DependencyProperty.Register(nameof (Uri), typeof (Uri), typeof (GifImage), new PropertyMetadata((object) null, new PropertyChangedCallback(GifImage.OnUriChanged)));

  static GifImage()
  {
    UIElement.VisibilityProperty.OverrideMetadata(typeof (GifImage), new PropertyMetadata((object) Visibility.Visible, new PropertyChangedCallback(GifImage.OnVisibilityChanged)));
  }

  private static void OnUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    GifImage gifImage = (GifImage) d;
    gifImage.StopAnimate();
    if (e.NewValue != null)
    {
      Uri newValue = (Uri) e.NewValue;
      gifImage.GetGifStreamFromPack(newValue);
      gifImage.StartAnimate();
    }
    else
      gifImage.Source = (ImageSource) null;
  }

  public Uri Uri
  {
    get => (Uri) this.GetValue(GifImage.UriProperty);
    set => this.SetValue(GifImage.UriProperty, (object) value);
  }

  private static void OnVisibilityChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
  {
    GifImage gifImage = (GifImage) s;
    if (gifImage.NativeImage == IntPtr.Zero)
      return;
    if ((Visibility) e.NewValue != Visibility.Visible)
    {
      gifImage.StopAnimate();
    }
    else
    {
      if (gifImage._isStart)
        return;
      gifImage.StartAnimate();
    }
  }

  ~GifImage() => this.Dispose(false);

  public GifImage()
  {
    this.Loaded += (RoutedEventHandler) ((s, e) =>
    {
      if (DesignerProperties.GetIsInDesignMode((DependencyObject) this) || this._isLoaded)
        return;
      this._isLoaded = true;
      if (this.Uri != (Uri) null)
      {
        this.GetGifStreamFromPack(this.Uri);
        this.StartAnimate();
      }
      else
      {
        if (!(this.Source is BitmapImage source2))
          return;
        this.GetGifStreamFromPack(source2.UriSource);
        this.StartAnimate();
      }
    });
    this.Unloaded += (RoutedEventHandler) ((s, e) => this.Dispose());
  }

  public GifImage(string filename)
  {
    this.Source = (ImageSource) null;
    this.CreateSourceFromFile(filename);
    this.StartAnimate();
  }

  public GifImage(Stream stream)
  {
    this.Source = (ImageSource) null;
    this.CreateSourceFromStream(stream);
    this.StartAnimate();
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (this.NativeImage == IntPtr.Zero)
      return;
    try
    {
      this.StopAnimate();
      InteropMethods.Gdip.GdipDisposeImage(new HandleRef((object) this, this.NativeImage));
    }
    catch
    {
    }
    finally
    {
      this.NativeImage = IntPtr.Zero;
    }
  }

  private void CreateSourceFromFile(string filename)
  {
    filename = Path.GetFullPath(filename);
    IntPtr bitmap;
    int bitmapFromFile = InteropMethods.Gdip.GdipCreateBitmapFromFile(filename, out bitmap);
    if (bitmapFromFile != 0)
      throw InteropMethods.Gdip.StatusException(bitmapFromFile);
    int status = InteropMethods.Gdip.GdipImageForceValidation(new HandleRef((object) null, bitmap));
    if (status != 0)
    {
      InteropMethods.Gdip.GdipDisposeImage(new HandleRef((object) null, bitmap));
      throw InteropMethods.Gdip.StatusException(status);
    }
    this.SetNativeImage(bitmap);
    GifImage.EnsureSave(this, filename, (Stream) null);
  }

  private void CreateSourceFromStream(Stream stream)
  {
    IntPtr bitmap;
    int status1 = stream != null ? InteropMethods.Gdip.GdipCreateBitmapFromStream((InteropValues.IStream) new GPStream(stream), out bitmap) : throw new ArgumentException("stream null");
    if (status1 != 0)
      throw InteropMethods.Gdip.StatusException(status1);
    int status2 = InteropMethods.Gdip.GdipImageForceValidation(new HandleRef((object) null, bitmap));
    if (status2 != 0)
    {
      InteropMethods.Gdip.GdipDisposeImage(new HandleRef((object) null, bitmap));
      throw InteropMethods.Gdip.StatusException(status2);
    }
    this.SetNativeImage(bitmap);
    GifImage.EnsureSave(this, (string) null, stream);
  }

  private void GetGifStreamFromPack(Uri uri)
  {
    try
    {
      this.CreateSourceFromStream(((uri.IsAbsoluteUri ? (!uri.GetLeftPart(UriPartial.Authority).Contains("siteoforigin") ? Application.GetContentStream(uri) ?? Application.GetResourceStream(uri) : Application.GetRemoteStream(uri)) : Application.GetContentStream(uri) ?? Application.GetResourceStream(uri)) ?? throw new FileNotFoundException("Resource not found.", uri.ToString())).Stream);
    }
    catch
    {
    }
  }

  private void SwitchToCommonImage()
  {
    if (this.Source != null || !(this.Uri != (Uri) null))
      return;
    this.SetCurrentValue(Image.SourceProperty, (object) new BitmapImage(this.Uri));
  }

  internal static void EnsureSave(GifImage image, string filename, Stream dataStream)
  {
    if (!image.RawGuid.Equals(GifImage.GifGuid) && !image.RawGuid.Equals(GifImage.GifSingleFrameGuid))
    {
      image.SwitchToCommonImage();
    }
    else
    {
      bool flag = false;
      if (((IEnumerable<Guid>) image.FrameDimensionsList).Select<Guid, GifFrameDimension>((Func<Guid, GifFrameDimension>) (guid => new GifFrameDimension(guid))).Contains<GifFrameDimension>(GifFrameDimension.Time))
        flag = image.GetFrameCount(GifFrameDimension.Time) > 1;
      if (!flag)
      {
        image.SwitchToCommonImage();
      }
      else
      {
        try
        {
          Stream stream = (Stream) null;
          long num = 0;
          if (dataStream != null)
          {
            num = dataStream.Position;
            dataStream.Position = 0L;
          }
          try
          {
            if (dataStream == null)
              stream = dataStream = (Stream) File.OpenRead(filename);
            image._rawData = new byte[(int) dataStream.Length];
            dataStream.Read(image._rawData, 0, (int) dataStream.Length);
          }
          finally
          {
            if (stream != null)
              stream.Close();
            else
              dataStream.Position = num;
          }
        }
        catch (UnauthorizedAccessException ex)
        {
        }
        catch (DirectoryNotFoundException ex)
        {
        }
        catch (IOException ex)
        {
        }
        catch (NotSupportedException ex)
        {
        }
        catch (ObjectDisposedException ex)
        {
        }
        catch (ArgumentException ex)
        {
        }
      }
    }
  }

  internal void SetNativeImage(IntPtr handle)
  {
    this.NativeImage = !(handle == IntPtr.Zero) ? handle : throw new ArgumentException("NativeHandle0");
  }

  internal Guid RawGuid
  {
    get
    {
      Guid format = new Guid();
      int imageRawFormat = InteropMethods.Gdip.GdipGetImageRawFormat(new HandleRef((object) this, this.NativeImage), ref format);
      if (imageRawFormat != 0)
        throw InteropMethods.Gdip.StatusException(imageRawFormat);
      return format;
    }
  }

  internal void StartAnimate()
  {
    ImageAnimator.Animate(this, new EventHandler(this.OnFrameChanged));
    this._isStart = true;
  }

  internal void StopAnimate()
  {
    ImageAnimator.StopAnimate(this, new EventHandler(this.OnFrameChanged));
    this._isStart = false;
  }

  internal Guid[] FrameDimensionsList
  {
    get
    {
      int count;
      int frameDimensionsCount = InteropMethods.Gdip.GdipImageGetFrameDimensionsCount(new HandleRef((object) this, this.NativeImage), out count);
      if (frameDimensionsCount != 0)
        throw InteropMethods.Gdip.StatusException(frameDimensionsCount);
      if (count <= 0)
        return new Guid[0];
      int num1 = Marshal.SizeOf(typeof (Guid));
      IntPtr num2 = Marshal.AllocHGlobal(checked (num1 * count));
      if (num2 == IntPtr.Zero)
        throw InteropMethods.Gdip.StatusException(3);
      int frameDimensionsList1 = InteropMethods.Gdip.GdipImageGetFrameDimensionsList(new HandleRef((object) this, this.NativeImage), num2, count);
      if (frameDimensionsList1 != 0)
      {
        Marshal.FreeHGlobal(num2);
        throw InteropMethods.Gdip.StatusException(frameDimensionsList1);
      }
      Guid[] frameDimensionsList2 = new Guid[count];
      try
      {
        for (int index = 0; index < count; ++index)
          frameDimensionsList2[index] = (Guid) InteropMethods.PtrToStructure((IntPtr) ((long) num2 + (long) (num1 * index)), typeof (Guid));
      }
      finally
      {
        Marshal.FreeHGlobal(num2);
      }
      return frameDimensionsList2;
    }
  }

  internal int GetFrameCount(GifFrameDimension dimension)
  {
    int[] count = new int[1];
    Guid guid = dimension.Guid;
    int frameCount = InteropMethods.Gdip.GdipImageGetFrameCount(new HandleRef((object) this, this.NativeImage), ref guid, count);
    if (frameCount != 0)
      throw InteropMethods.Gdip.StatusException(frameCount);
    return count[0];
  }

  internal GifPropertyItem GetPropertyItem(int propid)
  {
    int size;
    int propertyItemSize = InteropMethods.Gdip.GdipGetPropertyItemSize(new HandleRef((object) this, this.NativeImage), propid, out size);
    if (propertyItemSize != 0)
      throw InteropMethods.Gdip.StatusException(propertyItemSize);
    if (size == 0)
      return (GifPropertyItem) null;
    IntPtr num = Marshal.AllocHGlobal(size);
    if (num == IntPtr.Zero)
      throw InteropMethods.Gdip.StatusException(3);
    int propertyItem = InteropMethods.Gdip.GdipGetPropertyItem(new HandleRef((object) this, this.NativeImage), propid, size, num);
    try
    {
      if (propertyItem != 0)
        throw InteropMethods.Gdip.StatusException(propertyItem);
      return GifPropertyItemInternal.ConvertFromMemory(num, 1)[0];
    }
    finally
    {
      Marshal.FreeHGlobal(num);
    }
  }

  internal int SelectActiveFrame(GifFrameDimension dimension, int frameIndex)
  {
    int[] numArray = new int[1];
    Guid guid = dimension.Guid;
    int status = InteropMethods.Gdip.GdipImageSelectActiveFrame(new HandleRef((object) this, this.NativeImage), ref guid, frameIndex);
    if (status != 0)
      throw InteropMethods.Gdip.StatusException(status);
    return numArray[0];
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
  internal IntPtr GetHbitmap() => this.GetHbitmap(Colors.LightGray);

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
  internal IntPtr GetHbitmap(Color background)
  {
    IntPtr hbitmap;
    int hbitmapFromBitmap = InteropMethods.Gdip.GdipCreateHBITMAPFromBitmap(new HandleRef((object) this, this.NativeImage), out hbitmap, ColorHelper.ToWin32(background));
    if (hbitmapFromBitmap == 2 && (this.Width >= (double) short.MaxValue || this.Height >= (double) short.MaxValue))
      throw new ArgumentException("GdiplusInvalidSize");
    if (hbitmapFromBitmap != 0 && this.NativeImage != IntPtr.Zero)
      throw InteropMethods.Gdip.StatusException(hbitmapFromBitmap);
    return hbitmap;
  }

  private void GetBitmapSource()
  {
    IntPtr num = IntPtr.Zero;
    try
    {
      num = this.GetHbitmap();
      this.Source = (ImageSource) System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(num, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
    }
    catch
    {
    }
    finally
    {
      if (num != IntPtr.Zero)
        InteropMethods.DeleteObject(num);
    }
  }

  private void OnFrameChanged(object sender, EventArgs e)
  {
    this.Dispatcher.BeginInvoke((Delegate) (() =>
    {
      ImageAnimator.UpdateFrames();
      this.Source?.Freeze();
      this.GetBitmapSource();
      this.InvalidateVisual();
    }));
  }
}
