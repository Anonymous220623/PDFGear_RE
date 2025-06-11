// Decompiled with JetBrains decompiler
// Type: Standard.PROPVARIANT
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[StructLayout(LayoutKind.Explicit)]
internal class PROPVARIANT : IDisposable
{
  [FieldOffset(0)]
  private ushort vt;
  [FieldOffset(8)]
  private IntPtr pointerVal;
  [FieldOffset(8)]
  private byte byteVal;
  [FieldOffset(8)]
  private long longVal;
  [FieldOffset(8)]
  private short boolVal;

  public VarEnum VarType => (VarEnum) this.vt;

  public string GetValue()
  {
    return this.vt == (ushort) 31 /*0x1F*/ ? Marshal.PtrToStringUni(this.pointerVal) : (string) null;
  }

  public void SetValue(bool f)
  {
    this.Clear();
    this.vt = (ushort) 11;
    this.boolVal = f ? (short) -1 : (short) 0;
  }

  public void SetValue(string val)
  {
    this.Clear();
    this.vt = (ushort) 31 /*0x1F*/;
    this.pointerVal = Marshal.StringToCoTaskMemUni(val);
  }

  public void Clear() => PROPVARIANT.NativeMethods.PropVariantClear(this);

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  ~PROPVARIANT() => this.Dispose(false);

  private void Dispose(bool disposing) => this.Clear();

  private static class NativeMethods
  {
    [DllImport("ole32.dll")]
    internal static extern HRESULT PropVariantClear(PROPVARIANT pvar);
  }
}
