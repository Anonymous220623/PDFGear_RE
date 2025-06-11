// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.IDecryptor
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

public interface IDecryptor
{
  void Decrypt(DataProvider provider, int offset, int length, long streamPosition);

  void Decrypt(byte[] buffer, int offset, int length);

  bool SetDecryptionInfo(byte[] docId, byte[] encryptedDocId, byte[] digest, string password);
}
