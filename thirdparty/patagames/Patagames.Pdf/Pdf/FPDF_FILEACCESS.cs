// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FPDF_FILEACCESS
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Class for custom file access.</summary>
[StructLayout(LayoutKind.Sequential)]
public class FPDF_FILEACCESS : IDisposable, IStaticCallbacks
{
  /// <summary>File length, in bytes.</summary>
  [MarshalAs(UnmanagedType.I4)]
  public uint FileLen;
  /// <summary>
  /// User callback function. See <see cref="T:Patagames.Pdf.GetBlockCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  private GetBlockCallback _getBlock;
  private IntPtr _param;

  /// <summary>
  /// Gets a user defined data, passed as the first parameter to getBlockCallback callback.
  /// </summary>
  public byte[] Param
  {
    get
    {
      byte[] managed = SafeArrayMarshaler.GetInstance((string) null).MarshalNativeToManaged(this._param) as byte[];
      byte[] numArray = (byte[]) null;
      ref byte[] local = ref numArray;
      FPDF_FILEACCESS.ExtractKeyAndParam(managed, out local);
      return numArray;
    }
    set
    {
      ICustomMarshaler instance = SafeArrayMarshaler.GetInstance((string) null);
      Guid key = new Guid();
      if (this._param != IntPtr.Zero)
      {
        byte[] managed = instance.MarshalNativeToManaged(this._param) as byte[];
        byte[] numArray = (byte[]) null;
        ref byte[] local = ref numArray;
        key = FPDF_FILEACCESS.ExtractKeyAndParam(managed, out local);
        instance.CleanUpNativeData(this._param);
      }
      if (key == new Guid())
        key = Guid.NewGuid();
      byte[] ManagedObj = FPDF_FILEACCESS.PackKeyAndParam(key, value);
      this._param = instance.MarshalManagedToNative((object) ManagedObj);
    }
  }

  /// <summary>
  /// User callback function. See <see cref="T:Patagames.Pdf.GetBlockCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  public GetBlockCallback GetBlock
  {
    get => CallbackManager.Get<GetBlockCallback>(this.Key);
    set => CallbackManager.Set<GetBlockCallback>(this.Key, value);
  }

  private Guid Key
  {
    get
    {
      byte[] managed = SafeArrayMarshaler.GetInstance((string) null).MarshalNativeToManaged(this._param) as byte[];
      byte[] numArray = (byte[]) null;
      ref byte[] local = ref numArray;
      return FPDF_FILEACCESS.ExtractKeyAndParam(managed, out local);
    }
  }

  /// <summary>
  /// Initialize a new instance of the FPDF_FILEACCESS class using file length and user data
  /// </summary>
  /// <param name="FileLen">File length, in bytes.</param>
  /// <param name="param">A custom pointer for all implementation specific data.</param>
  public FPDF_FILEACCESS(uint FileLen, byte[] param = null)
  {
    this.Param = param;
    this.FileLen = FileLen;
    this._getBlock = new GetBlockCallback(FPDF_FILEACCESS.GetBlockDataCallback);
  }

  /// <summary>
  /// Releases all resources used by the <see cref="T:Patagames.Pdf.FPDF_FILEACCESS" />.
  /// </summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>
  /// Releases all resources used by the <see cref="T:Patagames.Pdf.FPDF_FILEACCESS" />.
  /// </summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    CallbackManager.Remove(this.Key);
    if (this._param != IntPtr.Zero)
      SafeArrayMarshaler.GetInstance((string) null).CleanUpNativeData(this._param);
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  [MonoPInvokeCallback(typeof (GetBlockCallback))]
  private static bool GetBlockDataCallback(byte[] param, uint position, byte[] buf, uint size)
  {
    byte[] outParam = (byte[]) null;
    Guid keyAndParam = FPDF_FILEACCESS.ExtractKeyAndParam(param, out outParam);
    if (keyAndParam == Guid.Empty)
      return false;
    GetBlockCallback getBlockCallback = CallbackManager.Get<GetBlockCallback>(keyAndParam);
    return getBlockCallback != null && getBlockCallback(outParam, position, buf, size);
  }

  private static byte[] PackKeyAndParam(Guid key, byte[] param)
  {
    byte[] byteArray = key.ToByteArray();
    byte[] destinationArray = new byte[2 + byteArray.Length + (param == null ? 0 : param.Length)];
    destinationArray[0] = (byte) byteArray.Length;
    destinationArray[1] = param == null ? (byte) 0 : (byte) 1;
    Array.Copy((Array) byteArray, 0, (Array) destinationArray, 2, byteArray.Length);
    if (param != null && param.Length != 0)
      Array.Copy((Array) param, 0, (Array) destinationArray, byteArray.Length + 2, param.Length);
    return destinationArray;
  }

  private static Guid ExtractKeyAndParam(byte[] inParam, out byte[] outParam)
  {
    outParam = (byte[]) null;
    if (inParam == null || inParam.Length < 2)
      return Guid.Empty;
    byte length = inParam[0];
    bool flag = inParam[1] == (byte) 0;
    if (length <= (byte) 0 || inParam.Length < (int) length + 2)
      return Guid.Empty;
    byte[] numArray = new byte[(int) length];
    Array.Copy((Array) inParam, 2, (Array) numArray, 0, (int) length);
    if (!flag)
    {
      outParam = new byte[inParam.Length - (int) length - 2];
      if (outParam.Length != 0)
        Array.Copy((Array) inParam, (int) length + 2, (Array) outParam, 0, outParam.Length);
    }
    return new Guid(numArray);
  }
}
