// Decompiled with JetBrains decompiler
// Type: InteropDotNet.RuntimeDllImportAttribute
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace InteropDotNet;

[ComVisible(true)]
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
internal sealed class RuntimeDllImportAttribute : Attribute
{
  public string EntryPoint;
  public CallingConvention CallingConvention;
  public CharSet CharSet;
  public bool SetLastError;
  public bool BestFitMapping;
  public bool ThrowOnUnmappableChar;

  public string LibraryFileName { get; private set; }

  public RuntimeDllImportAttribute(string libraryFileName)
  {
    this.LibraryFileName = libraryFileName;
  }
}
