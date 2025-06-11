// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.IFSDK_PAUSE
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// IFSDK_PAUSE interface. Used for progressive rendering. You must make sure that the class instance will not be collected by the garbage collector after passing this instance to unmanaged code.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public class IFSDK_PAUSE : IDisposable
{
  /// <summary>Version number of the interface. Currently must be 1.</summary>
  private int Version = 1;
  /// <summary>
  /// UserCallback function. See <see cref="T:Patagames.Pdf.NeedToPauseNowCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public NeedToPauseNowCallback needToPauseNowCallback;
  /// <summary>
  /// A user defined data pointer, used by user's application. Can be NULL.
  /// </summary>
  private IntPtr _userData = IntPtr.Zero;

  /// <summary>Gets a user defined data.</summary>
  public byte[] userData
  {
    get
    {
      return SafeArrayMarshaler.GetInstance((string) null).MarshalNativeToManaged(this._userData) as byte[];
    }
    set
    {
      ICustomMarshaler instance = SafeArrayMarshaler.GetInstance((string) null);
      if (this._userData != IntPtr.Zero)
        instance.CleanUpNativeData(this._userData);
      this._userData = instance.MarshalManagedToNative((object) value);
    }
  }

  /// <summary>
  /// Initialize a new instance of the <see cref="T:Patagames.Pdf.IFSDK_PAUSE" /> class using callback function and user data
  /// </summary>
  /// <param name="userdata">user data passed to interface</param>
  public IFSDK_PAUSE(byte[] userdata = null) => this.userData = userdata;

  /// <summary>
  /// Releases all resources used by the <see cref="T:Patagames.Pdf.IFSDK_PAUSE" />.
  /// </summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>
  /// Releases all resources used by the <see cref="T:Patagames.Pdf.IFSDK_PAUSE" />.
  /// </summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    this.userData = (byte[]) null;
    this.needToPauseNowCallback = (NeedToPauseNowCallback) null;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }
}
