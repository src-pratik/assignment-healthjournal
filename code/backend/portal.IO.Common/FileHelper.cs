using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace portal.IO.Common
{
    public static class FileHelper
    {
        public static class VerifyFormat
        {
            private static readonly Dictionary<string, List<byte[]>> _fileSignatures = new Dictionary<string, List<byte[]>>
            {
                { "pdf", new List<byte[]> {new byte[] { 0x25, 0x50, 0x44, 0x46,0x2D } } }
            };

            public static bool PDF(Stream stream)
            {
                try
                {
                    stream.Position = 0;
                    var ext = nameof(PDF).ToLower();
                    using (var reader = new BinaryReader(stream, Encoding.Default, true))
                    {
                        var signatures = _fileSignatures[ext];
                        var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                        stream.Position = 0;

                        return signatures.Any(signature =>
                            headerBytes.Take(signature.Length).SequenceEqual(signature));
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}