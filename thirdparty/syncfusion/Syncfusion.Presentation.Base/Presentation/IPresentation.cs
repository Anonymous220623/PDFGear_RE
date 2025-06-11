// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.IPresentation
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Drawing;
using Syncfusion.OfficeChart;
using System;
using System.IO;
using System.Web;

#nullable disable
namespace Syncfusion.Presentation;

public interface IPresentation : IDisposable
{
  ISections Sections { get; }

  ISlides Slides { get; }

  FontSettings FontSettings { get; }

  bool Final { get; set; }

  IMasterSlides Masters { get; }

  void Save(string fileName);

  void Save(Stream stream);

  void Save(string fileName, FormatType formatType, HttpResponse response);

  void Encrypt(string password);

  void RemoveEncryption();

  IOfficeChartToImageConverter ChartToImageConverter { get; set; }

  System.Drawing.Image[] RenderAsImages(Syncfusion.Drawing.ImageType imageType);

  Stream[] RenderAsImages(ImageFormat imageFormat);

  IBuiltInDocumentProperties BuiltInDocumentProperties { get; }

  ICustomDocumentProperties CustomDocumentProperties { get; }

  void Close();

  IPresentation Clone();

  void RemoveMacros();

  bool HasMacros { get; }

  void SetWriteProtection(string password);

  void RemoveWriteProtection();

  bool IsWriteProtected { get; }
}
