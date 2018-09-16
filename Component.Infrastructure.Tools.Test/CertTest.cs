using System;
using System.IO;
using Component.Infrastructure.Cert;
using Component.Infrastructure.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Component.Infrastructure.Tools.Test
{
    [TestClass]
    public class CertTest
    {

        [TestMethod]
        public void CreatCertTest()
        {
            var certFileName = "Jwt";
            var path = FolderHelper.MapPath("~/Cert Kits");
            var makeCertHelper = new MakeCertHelper(path);
            makeCertHelper.CreateCert(new MakeCertParameter
            {
                subjectName = "Component Jwt",
                keyType = KeyType.signature,
                certFileName = "Jwt",
            });

            X509CertificationHelper.ExportPrivateKey($"{Path.Combine(path, certFileName)}.pfx", "password", $"{certFileName}PrKey.xml");
            X509CertificationHelper.ExportPublicKey($"{Path.Combine(path, certFileName)}.pfx", $"{certFileName}PublicKey.xml", "password");
        }
    }
}