using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace StocktakrClient
{
     public static class ZipHelper
     {

          public static string CompressToGzip(string text)
          {
               byte[] buffer = Encoding.UTF8.GetBytes(text);
               MemoryStream ms = new MemoryStream();
               using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
               {
                    zip.Write(buffer, 0, buffer.Length);
               }

               ms.Position = 0;
               MemoryStream outStream = new MemoryStream();

               byte[] compressed = new byte[ms.Length];
               ms.Read(compressed, 0, compressed.Length);

               byte[] gzBuffer = new byte[compressed.Length + 4];
               System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
               System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
               return Convert.ToBase64String(gzBuffer);
          }

          private static void CompressStringToFile(string fileName, string value)
          {
               // A.
               // Write string to temporary file.
               string temp = Path.GetTempFileName();
               File.WriteAllText(temp, value);

               // B.
               // Read file into byte array buffer.
               byte[] b;
               using (FileStream f = new FileStream(temp, FileMode.Open))
               {
                    b = new byte[f.Length];
                    f.Read(b, 0, (int)f.Length);
               }

               // C.
               // Use GZipStream to write compressed bytes to target file.
               using (FileStream f2 = new FileStream(fileName, FileMode.Create))
               using (GZipStream gz = new GZipStream(f2, CompressionMode.Compress, false))
               {
                    gz.Write(b, 0, b.Length);
               }
          }

          public static string DecompressFromGzip(string compressedText)
          {
               byte[] gzBuffer = Convert.FromBase64String(compressedText);
               using (MemoryStream ms = new MemoryStream())
               {
                    int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                    ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                    byte[] buffer = new byte[msgLength];

                    ms.Position = 0;
                    using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                    {
                         zip.Read(buffer, 0, buffer.Length);
                    }

                    return Encoding.UTF8.GetString(buffer);
               }
          }
     }
}
