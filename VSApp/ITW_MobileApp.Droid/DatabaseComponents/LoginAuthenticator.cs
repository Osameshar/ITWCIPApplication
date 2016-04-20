using System;
using System.Text;

using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ITW_MobileApp.Droid
{
    class LoginAuthenticator
    {
        public static async Task<bool> Authenticate(string employeeID, string password)
        {
            int empID = int.Parse(employeeID);
            var users = await IoC.Dbconnect.getClient().GetTable<EmployeeLoginItem>().ToListAsync();
            EmployeeLoginItem user = FindUser(users, empID);
            if (user != null)
            {
                HashAlgorithm algorithm = new SHA1Managed();
                var plainTextBytes = Encoding.ASCII.GetBytes(password);
                var saltBytes = Convert.FromBase64String(user.Salt);
                var plainTextWithSaltBytes = AppendByteArray(plainTextBytes, saltBytes);
                var saltedSHA1Bytes = algorithm.ComputeHash(plainTextWithSaltBytes);
                var saltedSHA1WithAppendedSaltBytes = AppendByteArray(saltedSHA1Bytes, saltBytes);
                string hash = Convert.ToBase64String(saltedSHA1WithAppendedSaltBytes);
                if (hash == user.Hash)
                {
                    return true;
                }
            }
            return false;
        }
        private static EmployeeLoginItem FindUser(List<EmployeeLoginItem> users, int empID)
        {
            foreach (EmployeeLoginItem user in users)
            {
                if (user.EmployeeID == empID)
                {
                    return user;
                }
            }
            return null;
        }
        public static async Task<string> GenerateSaltedSHA1(string plainTextString, int empID)
        {
            HashAlgorithm algorithm = new SHA1Managed();
            var saltBytes = GenerateSalt(4);
            var plainTextBytes = Encoding.ASCII.GetBytes(plainTextString);
            var plainTextWithSaltBytes = AppendByteArray(plainTextBytes, saltBytes);
            var saltedSHA1Bytes = algorithm.ComputeHash(plainTextWithSaltBytes);
            var saltedSHA1WithAppendedSaltBytes = AppendByteArray(saltedSHA1Bytes, saltBytes);
            await IoC.Dbconnect.getClient().GetTable<EmployeeLoginItem>().InsertAsync(new EmployeeLoginItem { EmployeeID = empID, Salt = Convert.ToBase64String(saltBytes), Hash = Convert.ToBase64String(saltedSHA1WithAppendedSaltBytes) });
            return "{SSHA}" + Convert.ToBase64String(saltedSHA1WithAppendedSaltBytes);
        }
        private static byte[] GenerateSalt(int saltSize)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[saltSize];
            rng.GetBytes(buff);
            return buff;
        }
        private static byte[] AppendByteArray(byte[] byteArray1, byte[] byteArray2)
        {
            var byteArrayResult =
                    new byte[byteArray1.Length + byteArray2.Length];

            for (var i = 0; i < byteArray1.Length; i++)
                byteArrayResult[i] = byteArray1[i];
            for (var i = 0; i < byteArray2.Length; i++)
                byteArrayResult[byteArray1.Length + i] = byteArray2[i];

            return byteArrayResult;
        }

    }
}