using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

[assembly: Extension]
[assembly: ComVisible(false)]
[assembly: InternalsVisibleTo("Tesseract.Net45Tests")]
[assembly: InternalsVisibleTo("Tesseract.NetCore31Tests")]
[assembly: AssemblyCompany("Charles Weld")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyCopyright("Copyright 2012-2020 Charles Weld")]
[assembly: AssemblyDescription("Tesseract 4 adds a new neural net (LSTM) based OCR engine which is focused on line recognition, but also still supports the legacy Tesseract OCR engine of Tesseract 3 which works by recognizing character patterns. Compatibility with Tesseract 3 is enabled by using the Legacy OCR Engine mode (--oem 0). It also needs traineddata files which support the legacy engine, for example those from the tessdata repository.")]
[assembly: AssemblyFileVersion("4.1.1.0")]
[assembly: AssemblyInformationalVersion("4.1.1")]
[assembly: AssemblyProduct("Tesseract")]
[assembly: AssemblyTitle("Tesseract")]
[assembly: AssemblyMetadata("RepositoryUrl", "https://github.com/charlesw/tesseract/")]
[assembly: AssemblyVersion("4.1.1.0")]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
