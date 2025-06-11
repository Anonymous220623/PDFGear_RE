// Decompiled with JetBrains decompiler
// Type: InteropDotNet.ILibraryLoaderLogic
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using System;

#nullable disable
namespace InteropDotNet;

internal interface ILibraryLoaderLogic
{
  IntPtr LoadLibrary(string fileName);

  bool FreeLibrary(IntPtr libraryHandle);

  IntPtr GetProcAddress(IntPtr libraryHandle, string functionName);

  string FixUpLibraryName(string fileName);
}
