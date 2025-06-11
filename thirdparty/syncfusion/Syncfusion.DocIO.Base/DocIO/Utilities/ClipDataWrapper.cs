// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.Utilities.ClipDataWrapper
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.CompoundFile.DocIO.Native;
using System;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.Utilities;

internal class ClipDataWrapper
{
  private CLIPDATA m_clipStruct = new CLIPDATA();
  private byte[] m_clipData;

  public void Read(PROPVARIANT propVar)
  {
    this.m_clipStruct = (CLIPDATA) Marshal.PtrToStructure((IntPtr) propVar.intPtr, typeof (CLIPDATA));
    this.m_clipData = new byte[(IntPtr) this.m_clipStruct.uintSize];
    Marshal.Copy(this.m_clipStruct.intPtrClipData, this.m_clipData, 0, this.m_clipData.Length);
  }

  public void Read(string data)
  {
    string[] strArray = data != null && data.Length != 0 ? data.Split(' ') : throw new ArgumentException(nameof (data));
    this.m_clipStruct.intClipFmt = strArray.Length >= 2 ? Convert.ToInt32((object) Convert.FromBase64String(strArray[0])) : throw new ArgumentException(nameof (data));
    this.m_clipData = Convert.FromBase64String(strArray[1]);
    this.m_clipStruct.uintSize = (uint) this.m_clipData.Length;
  }

  public void Write(PROPVARIANT propVar)
  {
    this.m_clipStruct.intPtrClipData = Marshal.AllocHGlobal(this.m_clipData.Length);
    Marshal.Copy(this.m_clipData, 0, this.m_clipStruct.intPtrClipData, this.m_clipData.Length);
    this.m_clipStruct.uintSize = (uint) this.m_clipData.Length;
    Marshal.StructureToPtr<CLIPDATA>(this.m_clipStruct, (IntPtr) propVar.intPtr, true);
  }

  public string WriteToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    byte[] bytes = BitConverter.GetBytes(this.m_clipStruct.intClipFmt);
    stringBuilder.Append(Convert.ToBase64String(bytes));
    stringBuilder.Append(" ");
    stringBuilder.Append(Convert.ToBase64String(this.m_clipData));
    return stringBuilder.ToString();
  }
}
