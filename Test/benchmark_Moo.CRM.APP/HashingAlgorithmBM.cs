


using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Text;

namespace benchmark_Moo.CRM.APP
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class HashingAlgorithmBM
    {

        const int _saltLength = 16;
        const int _hashSize = 16;
        const int _iterationCount = 10000;

        const int _argonIterations = 4;
        const int _argonDegree = 8;
        const int _argonMemorySize = 1024 * 1024;

        [Benchmark]
        public void GenerateHash()
        {
            var salt = GenerateSalt();
            var pbkdf2Key = new Rfc2898DeriveBytes("Hello12345", salt, _iterationCount);
            pbkdf2Key.GetBytes(_hashSize);
        }

        [Benchmark]
        public void GenerateHashV2()
        {
            var salt = GenerateSalt();
            var argon = new Argon2id(Encoding.UTF8.GetBytes("Hello12345"))
            {
                Salt = salt,
                DegreeOfParallelism = _argonDegree,
                Iterations = _argonIterations,
                MemorySize = _argonMemorySize
            };

            argon.GetBytes(_hashSize);
        }

        [Benchmark]
        public void GenerateHashV3()
        {
            var salt = GenerateSalt();
            var sha = SHA256.Create();
            var passwordByte = Encoding.UTF8.GetBytes("Hello12345");
            var saltedValue = passwordByte.Concat(salt).ToArray();
            
            sha.ComputeHash(saltedValue);

        }

        [Benchmark]
        public void GenerateHashV4()
        {
            var salt = GenerateSalt();
            var sha = SHA512.Create();
            var passwordByte = Encoding.UTF8.GetBytes("Hello12345");
            var saltedValue = passwordByte.Concat(salt).ToArray();
            
            sha.ComputeHash(saltedValue);


        }

        private static byte[] GenerateSalt()
        {
            var numberGenerator = RandomNumberGenerator.Create();
            var salt = new byte[_saltLength];
            numberGenerator.GetBytes(salt);
            return salt;
        }

    }
}
