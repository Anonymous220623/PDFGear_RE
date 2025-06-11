// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.IEncryptor
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

public interface IEncryptor
{
  void SetEncryptionInfo(byte[] docId, string password);

  void Encrypt(DataProvider provider, int offset, int length, long streamPosition);

  void Encrypt(byte[] data, int offset, int length, long streamPosition);

  FilePassRecord GetFilePassRecord();
}
