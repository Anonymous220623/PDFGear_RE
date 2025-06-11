// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfCommon
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Text;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents a class that contain a common methods.</summary>
/// <threadsafety>Any public static (Shared in Visual Basic) members of this type are thread safe. Any instance members are not guaranteed to be thread safe.</threadsafety>
public class PdfCommon
{
  private static object _syncIsCheckForMemoryLeaks = new object();
  private static volatile bool _isCheckForMemoryLeaks = false;

  /// <summary>Initialize the SDK library</summary>
  /// <param name="licenseKey">Yours license key. Can be null for demo mode</param>
  /// <param name="specificPath">Path to the pdfium dynamic library (.dll/.dylib). See remarks sections for detail.</param>
  /// <param name="icuSpecificPath">Path to the icudt dynamic labrary (.dll/.dylib). See remarks sections for detail.</param>
  /// <remarks>
  /// <para>You have to call this function before you can call any PDF processing functions.</para>
  /// <para>In some cases, library initializer can not find pdfim/icudt dynamic library (.dll/.dylib) library to load.
  /// A typical example of this situation is a Web application.
  /// Web apps is running in a IIS working directory, unlike the classical apps. As a consequence, standard LoadLibrary function can not find the dll during loading process.
  /// </para>
  /// <para>In this case, you must explicitly specify the full path to the dll files through specificPath parameters in the initialization method.</para>
  /// </remarks>
  public static void Initialize(string licenseKey = null, string specificPath = null, string icuSpecificPath = null)
  {
    Pdfium.FPDF_InitICU(icuSpecificPath);
    Pdfium.FPDF_InitLibrary(licenseKey, specificPath);
  }

  /// <summary>Release all resources allocated by the SDK library</summary>
  /// <remarks>You can call this function to release all memory blocks allocated by the library. After this function called, you should not call any PDF processing functions.</remarks>
  public static void Release() => Pdfium.FPDF_DestroyLibrary();

  /// <summary>
  /// Gets or sets the encoding that is used to encode/decode ANSI strings.
  /// </summary>
  /// <remarks>By default used the system's current ANSI code page.</remarks>
  public static Encoding DefaultAnsiEncoding
  {
    get => Pdfium.DefaultAnsiEncoding;
    set => Pdfium.DefaultAnsiEncoding = value;
  }

  /// <summary>Determines whether the engine is initialized</summary>
  public static bool IsInitialize => Pdfium.IsInitialize;

  /// <summary>
  /// Specifies whether to check a memory leak during object finalization.
  /// </summary>
  /// <value>True - Check for memory leaks.</value>
  /// <remarks>
  /// If it is found undisposed instance of the class then will be thrown <see cref="T:Patagames.Pdf.Net.Exceptions.MemoryLeakException" /> with specifying the class name in the message.
  /// </remarks>
  public static bool IsCheckForMemoryLeaks
  {
    get
    {
      lock (PdfCommon._syncIsCheckForMemoryLeaks)
        return PdfCommon._isCheckForMemoryLeaks;
    }
    set
    {
      lock (PdfCommon._syncIsCheckForMemoryLeaks)
        PdfCommon._isCheckForMemoryLeaks = value;
    }
  }

  /// <summary>Activate Pdfium.NEt SDK at design time</summary>
  public static void DesignTimeActivation() => Patagames.Activation.Activation.SaveLicenseAtDesignTime();
}
