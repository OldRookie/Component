using Component.Infrastructure.CMD;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Infrastructure.Cert
{
    public class MakeCertHelper
    {
        public string KitPath { get; set; }
        public MakeCertHelper(string kitPath) {
            KitPath = kitPath;
        }
        //使用X509证书加密解密以及生成证书

       
        public void CreateCert(MakeCertParameter param)
        {
            string volume = KitPath.Substring(0, KitPath.IndexOf(':'));
            string makecert = "makecert.exe";
            param.subjectName = $"CN=\"{param.subjectName}\"";
            var certArguments = string.Empty;
            if (param.selfSigned)
            {
                certArguments += $" -r";
            }
            certArguments += $" -n {param.subjectName} -pe -a {param.alg}";
            if (param.cy.HasValue)
            {
                certArguments += $" -cy {param.cy.Value.ToString()}";
            }
            if (!string.IsNullOrEmpty(param.issuerCertName))
            {
                certArguments += $" -iv {param.issuerCertName}.pvk -ic {param.issuerCertName}.cer";
            }
            if (param.keyType.HasValue)
            {
                certArguments += $" -sky {param.keyType.Value.ToString()}";
            }
            if (!string.IsNullOrEmpty(param.eku))
            {
                certArguments += $" -eku {param.eku}";
            }
            certArguments += $" -sv {param.certFileName}.pvk {param.certFileName}.cer";

            string pvk2pfx = "pvk2pfx.exe";
            var pvk2pfxCmd = $"{pvk2pfx} -pvk {param.certFileName}.pvk -spc {param.certFileName}.cer -pfx {param.certFileName}.pfx -po {param.password}";
            CMDHelper.ExecBatCommand(action =>
            {
                action($"{volume}:");
                action($"cd {KitPath}");
                action($"{makecert} {certArguments}");
                action(pvk2pfxCmd);
                action("exit 0");
            });
        }
    }

    /// <summary>
    /// 证书类型
    /// </summary>
    public enum CertType
    {
        end,
        authority
    }

    /// <summary>
    /// 密钥类型
    /// </summary>
    public enum KeyType
    {
        signature,
        exchange
    }

    public class MakeCertParameter
    {
        public MakeCertParameter()
        {
            password = "password";
            alg = "sha512";
            selfSigned = true;

        }
        public string subjectName { get; set; }

        public string certFileName { get; set; }

        public string password { get; set; }

        public KeyType? keyType { get; set; }

        public string alg { get; set; }

        public CertType? cy { get; set; }

        public string issuerCertName { get; set; }

        public string eku { get; set; }

        public bool selfSigned { get; set; }
    }
}
