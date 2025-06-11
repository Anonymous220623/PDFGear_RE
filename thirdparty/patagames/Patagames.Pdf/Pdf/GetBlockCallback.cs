// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.GetBlockCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// A function pointer for getting a block of data from specific position.
/// </summary>
/// <param name="param">custom user data</param>
/// <param name="position">Position is specified by byte offset from beginning of the file.</param>
/// <param name="buf">buffer for data allocated inside Pdfium SDK</param>
/// <param name="size">buffer size</param>
/// <returns>should be true if successful, false for error.</returns>
/// <remarks>Required: Yes. Position is specified by byte offset from beginning of the file. It may be possible for FPDFSDK to call this function multiple times for same position.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.Bool)]
public delegate bool GetBlockCallback([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof (SafeArrayMarshaler))] byte[] param, [MarshalAs(UnmanagedType.I4)] uint position, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3), In, Out] byte[] buf, [MarshalAs(UnmanagedType.U4)] uint size);
