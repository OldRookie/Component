using Microsoft.VisualStudio.TestTools.UnitTesting;
using Component.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Component.Infrastructure.Tests
{
    [TestClass()]
    public class FileDESEncryptHelperTests
    {
        string dir = @"D:\Project\SourceCode\DotNet\Framework";

        [TestMethod()]
        public void EncryptTest()
        {
            using (FileStream in_stream = new FileStream(Path.Combine( dir,"Job.7z"),
               FileMode.Open, FileAccess.Read))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    //ms.Position = 0;
                    in_stream.CopyTo(ms);
                    var cryptBytes = FileDESEncryptHelper.Encrypt(ms.ToArray(), "...");
                    File.WriteAllBytes(Path.Combine(dir, "Job_en.7z"), cryptBytes);
                }

            }
        }

        [TestMethod()]
        public void DecryptTest()
        {
            using (FileStream in_stream = new FileStream(Path.Combine(dir, "Job_en.7z"),
               FileMode.Open, FileAccess.Read))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    //ms.Position = 0;
                    in_stream.CopyTo(ms);
                    var cryptBytes = FileDESEncryptHelper.Decrypt(ms.ToArray(), "...");
                    File.WriteAllBytes(Path.Combine(dir, "Job_de.7z"), cryptBytes);
                }
            }
        }
    }
}