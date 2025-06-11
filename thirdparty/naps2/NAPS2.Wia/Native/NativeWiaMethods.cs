// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.NativeWiaMethods
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

#nullable enable
namespace NAPS2.Wia.Native;

internal static class NativeWiaMethods
{
  private static IntPtr ToPtr(object? comObject)
  {
    return comObject == null ? IntPtr.Zero : Marshal.GetIUnknownForObject(comObject);
  }

  private static object ToObjectNonNull(IntPtr ptr)
  {
    return !(ptr == IntPtr.Zero) ? Marshal.GetObjectForIUnknown(ptr) : throw new ArgumentException();
  }

  private static object? ToObject(IntPtr ptr)
  {
    return ptr == IntPtr.Zero ? (object) null : Marshal.GetObjectForIUnknown(ptr);
  }

  public static uint GetDeviceManager1(out IntPtr deviceManager)
  {
    try
    {
      deviceManager = NativeWiaMethods.ToPtr((object) (IWiaDevMgr) new WiaDevMgr());
      return 0;
    }
    catch (COMException ex)
    {
      deviceManager = IntPtr.Zero;
      return (uint) ex.HResult;
    }
  }

  public static uint GetDeviceManager2(out IntPtr deviceManager)
  {
    try
    {
      deviceManager = NativeWiaMethods.ToPtr((object) (IWiaDevMgr2) new WiaDevMgr2());
      return 0;
    }
    catch (COMException ex)
    {
      deviceManager = IntPtr.Zero;
      return (uint) ex.HResult;
    }
  }

  public static uint GetDevice1(IntPtr deviceManagerPtr, string deviceId, out IntPtr device)
  {
    IWiaDevMgr objectNonNull = (IWiaDevMgr) NativeWiaMethods.ToObjectNonNull(deviceManagerPtr);
    try
    {
      device = NativeWiaMethods.ToPtr((object) objectNonNull.CreateDevice(deviceId));
      return 0;
    }
    catch (COMException ex)
    {
      device = IntPtr.Zero;
      return (uint) ex.HResult;
    }
  }

  public static uint GetDevice2(IntPtr deviceManagerPtr, string deviceId, out IntPtr device)
  {
    IWiaDevMgr2 objectNonNull = (IWiaDevMgr2) NativeWiaMethods.ToObjectNonNull(deviceManagerPtr);
    try
    {
      device = NativeWiaMethods.ToPtr((object) objectNonNull.CreateDevice(0, deviceId));
      return 0;
    }
    catch (COMException ex)
    {
      device = IntPtr.Zero;
      return (uint) ex.HResult;
    }
  }

  public static uint EnumerateDevices1(IntPtr deviceManagerPtr, Action<IntPtr> callback)
  {
    IWiaDevMgr objectNonNull = (IWiaDevMgr) NativeWiaMethods.ToObjectNonNull(deviceManagerPtr);
    try
    {
      IEnumWIA_DEV_INFO enumWiaDevInfo = objectNonNull.EnumDeviceInfo(16 /*0x10*/);
      uint num;
      do
      {
        IWiaPropertyStorage[] rgelt = new IWiaPropertyStorage[1];
        uint pceltFetched;
        num = (uint) enumWiaDevInfo.Next(1U, rgelt, out pceltFetched);
        if (num == 0U)
        {
          if (pceltFetched == 1U)
            callback(NativeWiaMethods.ToPtr((object) rgelt[0]));
          else
            break;
        }
      }
      while (num == 0U);
      return 0;
    }
    catch (COMException ex)
    {
      return (uint) ex.HResult;
    }
  }

  public static uint EnumerateDevices2(IntPtr deviceManagerPtr, Action<IntPtr> callback)
  {
    IWiaDevMgr2 objectNonNull = (IWiaDevMgr2) NativeWiaMethods.ToObjectNonNull(deviceManagerPtr);
    try
    {
      IEnumWIA_DEV_INFO enumWiaDevInfo = objectNonNull.EnumDeviceInfo(16 /*0x10*/);
      uint num;
      do
      {
        IWiaPropertyStorage[] rgelt = new IWiaPropertyStorage[1];
        uint pceltFetched;
        num = (uint) enumWiaDevInfo.Next(1U, rgelt, out pceltFetched);
        if (num == 0U)
        {
          if (pceltFetched == 1U)
            callback(NativeWiaMethods.ToPtr((object) rgelt[0]));
          else
            break;
        }
      }
      while (num == 0U);
      return 0;
    }
    catch (COMException ex)
    {
      return (uint) ex.HResult;
    }
  }

  public static uint EnumerateItems1(IntPtr itemPtr, Action<IntPtr> childCallback)
  {
    IWiaItem objectNonNull = (IWiaItem) NativeWiaMethods.ToObjectNonNull(itemPtr);
    try
    {
      int itemType = objectNonNull.GetItemType();
      if ((itemType & 4) == 4 || (itemType & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        IEnumWiaItem enumWiaItem = objectNonNull.EnumChildItems();
        if (enumWiaItem != null)
        {
          uint num;
          do
          {
            IWiaItem[] ppIWiaItem = new IWiaItem[1];
            uint pceltFetched;
            num = (uint) enumWiaItem.Next(1U, ppIWiaItem, out pceltFetched);
            if (num == 0U)
            {
              if (pceltFetched == 1U)
                childCallback(NativeWiaMethods.ToPtr((object) ppIWiaItem[0]));
              else
                break;
            }
          }
          while (num == 0U);
        }
      }
      return 0;
    }
    catch (COMException ex)
    {
      return (uint) ex.HResult;
    }
  }

  public static uint EnumerateItems2(IntPtr itemPtr, Action<IntPtr> childCallback)
  {
    IWiaItem2 objectNonNull = (IWiaItem2) NativeWiaMethods.ToObjectNonNull(itemPtr);
    try
    {
      int itemType = objectNonNull.GetItemType();
      if ((itemType & 4) == 4 || (itemType & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        IEnumWiaItem2 enumWiaItem2 = objectNonNull.EnumChildItems(IntPtr.Zero);
        if (enumWiaItem2 != null)
        {
          uint num;
          do
          {
            IWiaItem2[] ppIWiaItem2 = new IWiaItem2[1];
            uint pcEltFetched;
            num = (uint) enumWiaItem2.Next(1U, ppIWiaItem2, out pcEltFetched);
            if (num == 0U)
            {
              if (pcEltFetched == 1U)
                childCallback(NativeWiaMethods.ToPtr((object) ppIWiaItem2[0]));
              else
                break;
            }
          }
          while (num == 0U);
        }
      }
      return 0;
    }
    catch (COMException ex)
    {
      return (uint) ex.HResult;
    }
  }

  public static uint GetItemPropertyStorage(IntPtr item, out IntPtr propStorage)
  {
    try
    {
      propStorage = NativeWiaMethods.ToPtr((object) item);
      return 0;
    }
    catch (COMException ex)
    {
      propStorage = IntPtr.Zero;
      return (uint) ex.HResult;
    }
  }

  public static uint EnumerateProperties(
    IntPtr propStoragePtr,
    NativeWiaMethods.EnumPropertyCallback func)
  {
    IWiaPropertyStorage objectNonNull = (IWiaPropertyStorage) NativeWiaMethods.ToObjectNonNull(propStoragePtr);
    try
    {
      IEnumSTATPROPSTG enumStatpropstg = objectNonNull.Enum();
      if (enumStatpropstg != null)
      {
        uint num;
        do
        {
          STATPROPSTG[] rgelt = new STATPROPSTG[1];
          uint pceltFetched;
          num = (uint) enumStatpropstg.Next(1U, rgelt, out pceltFetched);
          if (num == 0U)
          {
            if (pceltFetched == 1U)
              func((int) rgelt[0].propid, rgelt[0].lpwstrName, rgelt[0].vt);
            else
              break;
          }
        }
        while (num == 0U);
      }
      return 0;
    }
    catch (COMException ex)
    {
      return (uint) ex.HResult;
    }
  }

  public static uint GetPropertyBstr(IntPtr propStoragePtr, int propId, out string value)
  {
    IWiaPropertyStorage objectNonNull = (IWiaPropertyStorage) NativeWiaMethods.ToObjectNonNull(propStoragePtr);
    PROPSPEC[] rgpspec = new PROPSPEC[1]
    {
      new PROPSPEC()
      {
        ulKind = 1U,
        unionmember = new IntPtr((long) (uint) propId)
      }
    };
    PROPVARIANT[] rgpropvar = new PROPVARIANT[1];
    try
    {
      try
      {
        objectNonNull.ReadMultiple(1U, rgpspec, rgpropvar);
        Win32.PropVariantToBSTR(in rgpropvar[0], out value);
        return 0;
      }
      finally
      {
        Win32.PropVariantClear(in rgpropvar[0]);
      }
    }
    catch (COMException ex)
    {
      value = string.Empty;
      return (uint) ex.HResult;
    }
  }

  public static uint GetPropertyInt(IntPtr propStoragePtr, int propId, out int value)
  {
    IWiaPropertyStorage objectNonNull = (IWiaPropertyStorage) NativeWiaMethods.ToObjectNonNull(propStoragePtr);
    PROPSPEC[] rgpspec = new PROPSPEC[1]
    {
      new PROPSPEC()
      {
        ulKind = 1U,
        unionmember = new IntPtr((long) (uint) propId)
      }
    };
    PROPVARIANT[] rgpropvar = new PROPVARIANT[1];
    try
    {
      try
      {
        objectNonNull.ReadMultiple(1U, rgpspec, rgpropvar);
        Win32.PropVariantToInt32(in rgpropvar[0], out value);
        return 0;
      }
      finally
      {
        Win32.PropVariantClear(in rgpropvar[0]);
      }
    }
    catch (COMException ex)
    {
      value = 0;
      return (uint) ex.HResult;
    }
  }

  public static uint SetPropertyInt(IntPtr propStoragePtr, int propId, int value)
  {
    IWiaPropertyStorage objectNonNull = (IWiaPropertyStorage) NativeWiaMethods.ToObjectNonNull(propStoragePtr);
    try
    {
      PROPSPEC[] rgpspec = new PROPSPEC[1]
      {
        new PROPSPEC()
        {
          ulKind = 1U,
          unionmember = new IntPtr((long) (uint) propId)
        }
      };
      PROPVARIANT[] rgpropvar = new PROPVARIANT[1];
      try
      {
        rgpropvar[0].Init((object) value);
        objectNonNull.WriteMultiple(1U, rgpspec, rgpropvar, 4098U);
      }
      finally
      {
        Win32.PropVariantClear(in rgpropvar[0]);
      }
      return 0;
    }
    catch (COMException ex)
    {
      return (uint) ex.HResult;
    }
  }

  public static uint GetPropertyAttributes(
    IntPtr propStoragePtr,
    int propId,
    out int flags,
    out int min,
    out int nom,
    out int max,
    out int step,
    out int numElems,
    out int[]? elems)
  {
    IWiaPropertyStorage objectNonNull = (IWiaPropertyStorage) NativeWiaMethods.ToObjectNonNull(propStoragePtr);
    PROPSPEC[] rgpspec = new PROPSPEC[1]
    {
      new PROPSPEC()
      {
        ulKind = 1U,
        unionmember = new IntPtr((long) (uint) propId)
      }
    };
    uint[] rgflags = new uint[1];
    PROPVARIANT[] rgpropvar = new PROPVARIANT[1];
    elems = (int[]) null;
    flags = 0;
    max = 0;
    min = 0;
    nom = 0;
    numElems = 0;
    step = 0;
    try
    {
      try
      {
        objectNonNull.GetPropertyAttributes(1U, rgpspec, rgflags, rgpropvar);
        flags = (int) rgflags[0];
        if (((long) flags & 16L /*0x10*/) == 16L /*0x10*/)
        {
          Win32.PropVariantGetInt32Elem(in rgpropvar[0], 0U, out min);
          Win32.PropVariantGetInt32Elem(in rgpropvar[0], 1U, out nom);
          Win32.PropVariantGetInt32Elem(in rgpropvar[0], 2U, out max);
          Win32.PropVariantGetInt32Elem(in rgpropvar[0], 3U, out step);
        }
        if (((long) flags & 32L /*0x20*/) == 32L /*0x20*/)
        {
          numElems = (int) Win32.PropVariantGetElementCount(in rgpropvar[0]);
          Win32.PropVariantGetInt32Elem(in rgpropvar[0], 1U, out nom);
          Win32.PropVariantToInt32VectorAlloc(in rgpropvar[0], out elems, out uint _);
        }
      }
      finally
      {
        Win32.PropVariantClear(in rgpropvar[0]);
      }
      return 0;
    }
    catch (COMException ex)
    {
      return (uint) ex.HResult;
    }
  }

  public static uint StartTransfer1(IntPtr itemPtr, out IntPtr transfer)
  {
    IWiaItem objectNonNull = (IWiaItem) NativeWiaMethods.ToObjectNonNull(itemPtr);
    Guid guid = new Guid(3110812843U, (ushort) 1832, (ushort) 4563, (byte) 157, (byte) 123, (byte) 0, (byte) 0, (byte) 248, (byte) 30, (byte) 243, (byte) 46);
    try
    {
      PROPSPEC[] rgpspec = new PROPSPEC[2]
      {
        new PROPSPEC()
        {
          ulKind = 1U,
          unionmember = new IntPtr(4106L)
        },
        new PROPSPEC()
        {
          ulKind = 1U,
          unionmember = new IntPtr(4108L)
        }
      };
      PROPVARIANT[] rgpropvar = new PROPVARIANT[2];
      rgpropvar[0].Init((object) guid);
      rgpropvar[1].Init((object) 128 /*0x80*/);
      ((IWiaPropertyStorage) objectNonNull).WriteMultiple(2U, rgpspec, rgpropvar, 4098U);
      transfer = NativeWiaMethods.ToPtr((object) (IWiaDataTransfer) objectNonNull);
      return 0;
    }
    catch (COMException ex)
    {
      transfer = IntPtr.Zero;
      return (uint) ex.HResult;
    }
  }

  public static uint StartTransfer2(IntPtr itemPtr, out IntPtr transfer)
  {
    IWiaItem2 objectNonNull = (IWiaItem2) NativeWiaMethods.ToObjectNonNull(itemPtr);
    try
    {
      transfer = NativeWiaMethods.ToPtr((object) (IWiaTransfer) objectNonNull);
      return 0;
    }
    catch (COMException ex)
    {
      transfer = IntPtr.Zero;
      return (uint) ex.HResult;
    }
  }

  public static uint Download1(IntPtr transferPtr, TransferStatusCallback func)
  {
    IWiaDataTransfer objectNonNull = (IWiaDataTransfer) NativeWiaMethods.ToObjectNonNull(transferPtr);
    WiaTransferCallback1 transferCallback1 = new WiaTransferCallback1(func);
    // ISSUE: explicit reference operation
    ref WIA_DATA_TRANSFER_INFO local = @new WIA_DATA_TRANSFER_INFO()
    {
      ulSize = (uint) sizeof (WIA_DATA_TRANSFER_INFO),
      ulBufferSize = 262144U /*0x040000*/,
      bDoubleBuffer = true
    };
    WiaTransferCallback1 pIWiaDataCallback = transferCallback1;
    return (uint) objectNonNull.idtGetBandedData(in local, (IWiaDataCallback) pIWiaDataCallback);
  }

  public static uint Download2(IntPtr transferPtr, TransferStatusCallback func)
  {
    IWiaTransfer objectNonNull = (IWiaTransfer) NativeWiaMethods.ToObjectNonNull(transferPtr);
    try
    {
      WiaTransferCallback2 pIWiaTransferCallback = new WiaTransferCallback2(func);
      objectNonNull.Download(0, (IWiaTransferCallback) pIWiaTransferCallback);
      return 0;
    }
    catch (COMException ex)
    {
      return (uint) ex.HResult;
    }
  }

  public static uint SelectDevice1(
    IntPtr deviceManagerPtr,
    IntPtr hwnd,
    int deviceType,
    int flags,
    out string deviceId,
    out IntPtr device)
  {
    IWiaDevMgr objectNonNull = (IWiaDevMgr) NativeWiaMethods.ToObjectNonNull(deviceManagerPtr);
    try
    {
      deviceId = string.Empty;
      device = NativeWiaMethods.ToPtr((object) objectNonNull.SelectDeviceDlg(hwnd, deviceType, flags, ref deviceId));
      return device == IntPtr.Zero ? 1U : 0U;
    }
    catch (COMException ex)
    {
      deviceId = string.Empty;
      device = IntPtr.Zero;
      return (uint) ex.HResult;
    }
  }

  public static uint SelectDevice2(
    IntPtr deviceManagerPtr,
    IntPtr hwnd,
    int deviceType,
    int flags,
    out string deviceId,
    out IntPtr device)
  {
    IWiaDevMgr2 objectNonNull = (IWiaDevMgr2) NativeWiaMethods.ToObjectNonNull(deviceManagerPtr);
    try
    {
      deviceId = string.Empty;
      device = NativeWiaMethods.ToPtr((object) objectNonNull.SelectDeviceDlg(hwnd, deviceType, flags, ref deviceId));
      return device == IntPtr.Zero ? 1U : 0U;
    }
    catch (COMException ex)
    {
      deviceId = string.Empty;
      device = IntPtr.Zero;
      return (uint) ex.HResult;
    }
  }

  public static uint GetImage1(
    IntPtr deviceManagerPtr,
    IntPtr hwnd,
    int deviceType,
    int flags,
    int intent,
    [MarshalAs(UnmanagedType.BStr)] string filePath,
    IntPtr itemPtr)
  {
    IWiaDevMgr objectNonNull = (IWiaDevMgr) NativeWiaMethods.ToObjectNonNull(deviceManagerPtr);
    IWiaItem wiaItem = (IWiaItem) NativeWiaMethods.ToObject(itemPtr);
    Guid guid1 = new Guid(3110812846U, (ushort) 1832, (ushort) 4563, (byte) 157, (byte) 123, (byte) 0, (byte) 0, (byte) 248, (byte) 30, (byte) 243, (byte) 46);
    try
    {
      IWiaDevMgr wiaDevMgr = objectNonNull;
      IntPtr hwndParent = hwnd;
      int lDeviceType = deviceType;
      int lFlags = flags;
      int lIntent = intent;
      IWiaItem pItemRoot = wiaItem;
      string bstrFilename = filePath;
      Guid guid2 = guid1;
      ref Guid local = ref guid2;
      wiaDevMgr.GetImageDlg(hwndParent, lDeviceType, lFlags, lIntent, pItemRoot, bstrFilename, ref local);
      return 0;
    }
    catch (COMException ex)
    {
      return (uint) ex.HResult;
    }
  }

  public static uint GetImage2(
    IntPtr deviceManagerPtr,
    int flags,
    string deviceId,
    IntPtr hwnd,
    string folder,
    string fileName,
    ref int numFiles,
    ref string[] filePaths,
    ref IntPtr itemPtr)
  {
    IWiaDevMgr2 objectNonNull = (IWiaDevMgr2) NativeWiaMethods.ToObjectNonNull(deviceManagerPtr);
    IWiaItem2 comObject = (IWiaItem2) NativeWiaMethods.ToObject(itemPtr);
    int lFlags = flags;
    string bstrDeviceID = deviceId;
    IntPtr hwndParent = hwnd;
    string bstrFolderName = folder;
    string bstrFilename = fileName;
    ref int local1 = ref numFiles;
    ref string[] local2 = ref filePaths;
    ref IWiaItem2 local3 = ref comObject;
    int imageDlg = objectNonNull.GetImageDlg(lFlags, bstrDeviceID, hwndParent, bstrFolderName, bstrFilename, ref local1, ref local2, ref local3);
    itemPtr = NativeWiaMethods.ToPtr((object) comObject);
    return (uint) imageDlg;
  }

  public static uint ConfigureDevice1(
    IntPtr devicePtr,
    IntPtr hwnd,
    int flags,
    int intent,
    out int itemCount,
    out IntPtr[]? itemPtrs)
  {
    IWiaItem[] ppIWiaItem;
    int num = ((IWiaItem) NativeWiaMethods.ToObjectNonNull(devicePtr)).DeviceDlg(hwnd, flags, intent, out itemCount, out ppIWiaItem);
    itemPtrs = ppIWiaItem != null ? ((IEnumerable<IWiaItem>) ppIWiaItem).Select<IWiaItem, IntPtr>(new Func<IWiaItem, IntPtr>(NativeWiaMethods.ToPtr)).ToArray<IntPtr>() : (IntPtr[]) null;
    return (uint) num;
  }

  public delegate void EnumPropertyCallback(int propId, [MarshalAs(UnmanagedType.LPWStr)] string propName, ushort propType);
}
