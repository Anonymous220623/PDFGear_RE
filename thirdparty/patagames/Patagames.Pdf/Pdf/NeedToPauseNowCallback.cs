// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.NeedToPauseNowCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Check if we need to pause a progressive process now.</summary>
/// <param name="pThis">Pointer to the interface structure itself</param>
/// <returns>True for pause now, False for continue.</returns>
/// <remarks>Required: Yes.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.Bool)]
public delegate bool NeedToPauseNowCallback([MarshalAs(UnmanagedType.LPStruct)] IFSDK_PAUSE pThis);
