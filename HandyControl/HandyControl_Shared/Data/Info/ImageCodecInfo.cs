// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.ImageCodecInfo
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Interop;
using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace HandyControl.Data;

internal sealed class ImageCodecInfo
{
  private string _dllName;

  public Guid Clsid { get; set; }

  public Guid FormatID { get; set; }

  public string CodecName { get; set; }

  public string DllName
  {
    get
    {
      if (this._dllName != null)
        new FileIOPermission(FileIOPermissionAccess.PathDiscovery, this._dllName).Demand();
      return this._dllName;
    }
    set
    {
      if (value != null)
        new FileIOPermission(FileIOPermissionAccess.PathDiscovery, value).Demand();
      this._dllName = value;
    }
  }

  public string FormatDescription { get; set; }

  public string FilenameExtension { get; set; }

  public string MimeType { get; set; }

  public ImageCodecFlags Flags { get; set; }

  public int Version { get; set; }

  public byte[][] SignaturePatterns { get; set; }

  public byte[][] SignatureMasks { get; set; }

  public static ImageCodecInfo[] GetImageDecoders()
  {
    int numDecoders;
    int size;
    int imageDecodersSize = InteropMethods.Gdip.GdipGetImageDecodersSize(out numDecoders, out size);
    if (imageDecodersSize != 0)
      throw InteropMethods.Gdip.StatusException(imageDecodersSize);
    IntPtr num = Marshal.AllocHGlobal(size);
    try
    {
      int imageDecoders = InteropMethods.Gdip.GdipGetImageDecoders(numDecoders, size, num);
      if (imageDecoders != 0)
        throw InteropMethods.Gdip.StatusException(imageDecoders);
      return ImageCodecInfo.ConvertFromMemory(num, numDecoders);
    }
    finally
    {
      Marshal.FreeHGlobal(num);
    }
  }

  public static ImageCodecInfo[] GetImageEncoders()
  {
    int numEncoders;
    int size;
    int imageEncodersSize = InteropMethods.Gdip.GdipGetImageEncodersSize(out numEncoders, out size);
    if (imageEncodersSize != 0)
      throw InteropMethods.Gdip.StatusException(imageEncodersSize);
    IntPtr num = Marshal.AllocHGlobal(size);
    try
    {
      int imageEncoders = InteropMethods.Gdip.GdipGetImageEncoders(numEncoders, size, num);
      if (imageEncoders != 0)
        throw InteropMethods.Gdip.StatusException(imageEncoders);
      return ImageCodecInfo.ConvertFromMemory(num, numEncoders);
    }
    finally
    {
      Marshal.FreeHGlobal(num);
    }
  }

  public static ImageCodecInfo[] ConvertFromMemory(IntPtr memoryStart, int numCodecs)
  {
    ImageCodecInfo[] imageCodecInfoArray = new ImageCodecInfo[numCodecs];
    for (int index1 = 0; index1 < numCodecs; ++index1)
    {
      IntPtr lparam = (IntPtr) ((long) memoryStart + (long) (Marshal.SizeOf(typeof (InteropValues.ImageCodecInfoPrivate)) * index1));
      InteropValues.ImageCodecInfoPrivate codecInfoPrivate = new InteropValues.ImageCodecInfoPrivate();
      InteropValues.ImageCodecInfoPrivate data = codecInfoPrivate;
      InteropMethods.PtrToStructure(lparam, (object) data);
      imageCodecInfoArray[index1] = new ImageCodecInfo()
      {
        Clsid = codecInfoPrivate.Clsid,
        FormatID = codecInfoPrivate.FormatID,
        CodecName = Marshal.PtrToStringUni(codecInfoPrivate.CodecName),
        DllName = Marshal.PtrToStringUni(codecInfoPrivate.DllName),
        FormatDescription = Marshal.PtrToStringUni(codecInfoPrivate.FormatDescription),
        FilenameExtension = Marshal.PtrToStringUni(codecInfoPrivate.FilenameExtension),
        MimeType = Marshal.PtrToStringUni(codecInfoPrivate.MimeType),
        Flags = (ImageCodecFlags) codecInfoPrivate.Flags,
        Version = codecInfoPrivate.Version,
        SignaturePatterns = new byte[codecInfoPrivate.SigCount][],
        SignatureMasks = new byte[codecInfoPrivate.SigCount][]
      };
      for (int index2 = 0; index2 < codecInfoPrivate.SigCount; ++index2)
      {
        imageCodecInfoArray[index1].SignaturePatterns[index2] = new byte[codecInfoPrivate.SigSize];
        imageCodecInfoArray[index1].SignatureMasks[index2] = new byte[codecInfoPrivate.SigSize];
        Marshal.Copy((IntPtr) ((long) codecInfoPrivate.SigMask + (long) (index2 * codecInfoPrivate.SigSize)), imageCodecInfoArray[index1].SignatureMasks[index2], 0, codecInfoPrivate.SigSize);
        Marshal.Copy((IntPtr) ((long) codecInfoPrivate.SigPattern + (long) (index2 * codecInfoPrivate.SigSize)), imageCodecInfoArray[index1].SignaturePatterns[index2], 0, codecInfoPrivate.SigSize);
      }
    }
    return imageCodecInfoArray;
  }
}
