using System;
using System.Linq;

namespace BETMart.BLL._Core
{
    public interface IUniqueCodeGenerator
    {
        string Generate(int length);
        string GeneratePassword(int length);
    }
    public class UniqueCodeGenerator
        : IUniqueCodeGenerator
    {
        private static readonly Random Random = new Random();
        public string Generate(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        public string GeneratePassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
