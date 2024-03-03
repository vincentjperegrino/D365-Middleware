using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Helper
{
    public static class Compression
    {

        public static async Task<CompressionResult> ToBrotliAsync(this string value, CompressionLevel level = CompressionLevel.Fastest)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            await using var input = new MemoryStream(bytes);
            await using var output = new MemoryStream();
            await using var stream = new BrotliStream(output, level);

            await input.CopyToAsync(stream);
            await stream.FlushAsync();

            var result = output.ToArray();

            return new CompressionResult(
                new CompressionValue(value, bytes.Length),
                new CompressionValue(Convert.ToBase64String(result), result.Length),
                level,
                "Brotli"
            );
        }

        public static async Task<string> FromBrotliAsync(this string value)
        {
            var bytes = Convert.FromBase64String(value);
            await using var input = new MemoryStream(bytes);
            await using var output = new MemoryStream();
            await using var stream = new BrotliStream(input, CompressionMode.Decompress);

            await stream.CopyToAsync(output);

            return Encoding.Unicode.GetString(output.ToArray());
        }


    }

    public record CompressionResult(
    CompressionValue Original,
    CompressionValue Result,
    CompressionLevel Level,
    string Kind)
    {
        public int Difference =>
            Original.Size - Result.Size;

        public decimal Percent =>
          Math.Abs(Difference / (decimal)Original.Size);
    }


    public record CompressionValue(
        string Value,
        int Size
    );


}
