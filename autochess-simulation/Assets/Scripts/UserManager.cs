using Libplanet.Crypto;
using Libplanet.Unity;
using System.IO;
using System.Collections.Generic;


namespace Scripts
{
    public class UserManager
    {
        private List<(PrivateKey, string)> Users { get; set; }

        public UserManager()
        {
        }

        public void ReadTempUserFile()
        {
            string path = Paths.TempPrivateKeysPath;

            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] fields = line.Split(',');

                    var privateKey = fields[0];
                    var alias = fields[1];

                    Users.Add((PrivateKey.FromString(privateKey), alias));
                }
            }
        }
    }
}
