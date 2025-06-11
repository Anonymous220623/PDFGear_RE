// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.ImageAnimator
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#nullable disable
namespace HandyControl.Tools;

internal class ImageAnimator
{
  private static List<GifImageInfo> ImageInfoList;
  private static readonly ReaderWriterLock RwImgListLock = new ReaderWriterLock();
  private static Thread AnimationThread;
  private static bool AnyFrameDirty;
  [ThreadStatic]
  private static int ThreadWriterLockWaitCount;

  public static bool CanAnimate(GifImage image)
  {
    if (image == null)
      return false;
    lock (image)
    {
      if (((IEnumerable<Guid>) image.FrameDimensionsList).Select<Guid, GifFrameDimension>((Func<Guid, GifFrameDimension>) (guid => new GifFrameDimension(guid))).Contains<GifFrameDimension>(GifFrameDimension.Time))
        return image.GetFrameCount(GifFrameDimension.Time) > 1;
    }
    return false;
  }

  public static void Animate(GifImage image, EventHandler onFrameChangedHandler)
  {
    if (image == null)
      return;
    GifImageInfo gifImageInfo;
    lock (image)
      gifImageInfo = new GifImageInfo(image);
    ImageAnimator.StopAnimate(image, onFrameChangedHandler);
    bool isReaderLockHeld = ImageAnimator.RwImgListLock.IsReaderLockHeld;
    LockCookie lockCookie = new LockCookie();
    ++ImageAnimator.ThreadWriterLockWaitCount;
    try
    {
      if (isReaderLockHeld)
        lockCookie = ImageAnimator.RwImgListLock.UpgradeToWriterLock(-1);
      else
        ImageAnimator.RwImgListLock.AcquireWriterLock(-1);
    }
    finally
    {
      --ImageAnimator.ThreadWriterLockWaitCount;
    }
    try
    {
      if (!gifImageInfo.Animated)
        return;
      if (ImageAnimator.ImageInfoList == null)
        ImageAnimator.ImageInfoList = new List<GifImageInfo>();
      gifImageInfo.FrameChangedHandler = onFrameChangedHandler;
      ImageAnimator.ImageInfoList.Add(gifImageInfo);
      if (ImageAnimator.AnimationThread != null)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ImageAnimator.AnimationThread = new Thread(ImageAnimator.\u003C\u003EO.\u003C0\u003E__AnimateImages50Ms ?? (ImageAnimator.\u003C\u003EO.\u003C0\u003E__AnimateImages50Ms = new ThreadStart(ImageAnimator.AnimateImages50Ms)))
      {
        Name = nameof (ImageAnimator),
        IsBackground = true
      };
      ImageAnimator.AnimationThread.Start();
    }
    finally
    {
      if (isReaderLockHeld)
        ImageAnimator.RwImgListLock.DowngradeFromWriterLock(ref lockCookie);
      else
        ImageAnimator.RwImgListLock.ReleaseWriterLock();
    }
  }

  public static void StopAnimate(GifImage image, EventHandler onFrameChangedHandler)
  {
    if (image == null || ImageAnimator.ImageInfoList == null)
      return;
    bool isReaderLockHeld = ImageAnimator.RwImgListLock.IsReaderLockHeld;
    LockCookie lockCookie = new LockCookie();
    ++ImageAnimator.ThreadWriterLockWaitCount;
    try
    {
      if (isReaderLockHeld)
        lockCookie = ImageAnimator.RwImgListLock.UpgradeToWriterLock(-1);
      else
        ImageAnimator.RwImgListLock.AcquireWriterLock(-1);
    }
    finally
    {
      --ImageAnimator.ThreadWriterLockWaitCount;
    }
    try
    {
      for (int index = 0; index < ImageAnimator.ImageInfoList.Count; ++index)
      {
        GifImageInfo imageInfo = ImageAnimator.ImageInfoList[index];
        if (object.Equals((object) image, (object) imageInfo.Image))
        {
          if (onFrameChangedHandler == imageInfo.FrameChangedHandler || onFrameChangedHandler != null && onFrameChangedHandler.Equals((object) imageInfo.FrameChangedHandler))
          {
            ImageAnimator.ImageInfoList.Remove(imageInfo);
            break;
          }
          break;
        }
      }
      if (ImageAnimator.ImageInfoList.Any<GifImageInfo>())
        return;
      ImageAnimator.AnimationThread?.Join();
      ImageAnimator.AnimationThread = (Thread) null;
    }
    finally
    {
      if (isReaderLockHeld)
        ImageAnimator.RwImgListLock.DowngradeFromWriterLock(ref lockCookie);
      else
        ImageAnimator.RwImgListLock.ReleaseWriterLock();
    }
  }

  public static void UpdateFrames()
  {
    if (!ImageAnimator.AnyFrameDirty || ImageAnimator.ImageInfoList == null || ImageAnimator.ThreadWriterLockWaitCount > 0)
      return;
    ImageAnimator.RwImgListLock.AcquireReaderLock(-1);
    try
    {
      foreach (GifImageInfo imageInfo in ImageAnimator.ImageInfoList)
      {
        lock (imageInfo.Image)
          imageInfo.UpdateFrame();
      }
      ImageAnimator.AnyFrameDirty = false;
    }
    finally
    {
      ImageAnimator.RwImgListLock.ReleaseReaderLock();
    }
  }

  private static void AnimateImages50Ms()
  {
    while (ImageAnimator.ImageInfoList.Any<GifImageInfo>())
    {
      ImageAnimator.RwImgListLock.AcquireReaderLock(-1);
      try
      {
        foreach (GifImageInfo imageInfo in ImageAnimator.ImageInfoList)
        {
          imageInfo.FrameTimer += 5;
          if (imageInfo.FrameTimer >= imageInfo.FrameDelay(imageInfo.Frame))
          {
            imageInfo.FrameTimer = 0;
            if (imageInfo.Frame + 1 < imageInfo.FrameCount)
              ++imageInfo.Frame;
            else
              imageInfo.Frame = 0;
            if (imageInfo.FrameDirty)
              ImageAnimator.AnyFrameDirty = true;
          }
        }
      }
      finally
      {
        ImageAnimator.RwImgListLock.ReleaseReaderLock();
      }
      Thread.Sleep(50);
    }
  }
}
