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
        public void CreateSignatureCertTest()
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

        [TestMethod]
        public void CreateServerSSLCertsTest()
        {
            var certRootFileName = "ComponentRootCA";
            var serverCertFileName = "ComponentServerV2";
            var clientCertFileName = "ComponentClientV3";
            var path = FolderHelper.MapPath("~/Cert Kits");
            var makeCertHelper = new MakeCertHelper(path);
            //makeCertHelper.CreateCert(new MakeCertParameter
            //{
            //    subjectName = "Component Root CA",
            //    cy = CertType.authority,
            //    certFileName = certRootFileName,
            //});

            makeCertHelper.CreateCert(new MakeCertParameter
            {
                subjectName = "Component Server V2",
                //keyType = KeyType.exchange,
                certFileName = serverCertFileName,
                issuerCertName = certRootFileName,
                selfSigned = false,
                //eku = "1.3.6.1.5.5.7.3.1"
            });

            //makeCertHelper.CreateCert(new MakeCertParameter
            //{
            //    subjectName = "Component Client V3",
            //    //keyType = KeyType.exchange,
            //    certFileName = clientCertFileName,
            //    issuerCertName = certRootFileName,
            //    selfSigned = false,
            //    //eku = "1.3.6.1.5.5.7.3.2"
            //});
        }
    }
}