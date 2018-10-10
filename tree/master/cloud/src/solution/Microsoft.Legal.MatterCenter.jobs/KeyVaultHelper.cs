using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;

namespace Microsoft.Legal.MatterCenter.Jobs
{
    public class KeyVaultHelper
    {

        IConfigurationRoot Configuration;
        public static ClientAssertionCertificate AssertionCert { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public KeyVaultHelper(IConfigurationRoot configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetKeyVaultSecretsCerticate()
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            keyValues = retrieveSecrets(true);
                
        
            foreach (var ky in keyValues)
            {              
                    Configuration[ky.Key] = ky.Value;          
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void GetKeyVaultSecretsSecret()
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            keyValues = retrieveSecrets(false);

            foreach (var ky in keyValues)
            {
                Configuration[ky.Key] = ky.Value;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Dictionary<String, String> retrieveSecrets(bool cert)
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();

            KeyVaultClient kv;

            if (cert)
            {
                kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessToken));

            }
            else
            {
               kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken));
            }


            List<string> secrets = new List<string>();

            var values = kv.GetSecretsAsync(this.Configuration.GetSection("General").GetSection("KeyVaultURI").Value.ToString()).GetAwaiter().GetResult();


            if (values != null)
            {

                foreach (var m in values)
                    secrets.Add(m.Identifier.Name);
            }

            while (values != null && !string.IsNullOrWhiteSpace(values.NextPageLink))
            {
                values = kv.GetSecretsNextAsync(values.NextPageLink).GetAwaiter().GetResult();
                if (values != null)
                {

                    foreach (var m in values)
                        secrets.Add(m.Identifier.Name);
                }
            }
            foreach (var value in secrets)
            {
                var secret = kv.GetSecretAsync(this.Configuration.GetSection("General").GetSection("KeyVaultURI").Value.ToString(), value).GetAwaiter().GetResult();
                keyValues.Add(value.Replace("-",":"), secret.Value);
            }


            return keyValues;
        }


       

        /// <summary>
        /// 
        /// </summary>
        /// Uses the Client Secret and ClientID to get a token to the AD
        /// <param name="authority"></param>
        /// <param name="resource"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public async Task<string> GetToken(string authority, string resource, string scope)
        {

            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(Configuration.GetSection("General").GetSection("ClientId").Value.ToString(),
                       Configuration.GetSection("General").GetSection("AppKey").Value.ToString());
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }



        /// <summary>
        ///
        /// </summary>
        /// <param name="configurtaion"></param>
        public static void GetCert(IConfiguration configurtaion)
        {
            var clientAssertionCertPfx = CertificateHelper.FindCertificateByThumbprint(configurtaion["General:KeyVaultCertThumbPrint"].ToString());
            AssertionCert = new ClientAssertionCertificate(configurtaion["General:KeyVaultClientID"], clientAssertionCertPfx);
        }

        /// <summary>
        /// Uses the ClientId and Screet to get an AD tokenok
        /// 
        /// </summary>
        /// <param name="authority"></param>
        /// <param name="resource"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static async Task<string> GetAccessToken(string authority, string resource, string scope)
        {
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var result = await context.AcquireTokenAsync(resource, AssertionCert);
            return result.AccessToken;
        }

        public static class CertificateHelper
        {
            public static X509Certificate2 FindCertificateByThumbprint(string findValue)
            {
                // Deal with any extra special chars in thumbprint
                string thumbprint = findValue.Replace("\u200e", string.Empty).Replace("\u200f", string.Empty).Replace(" ", string.Empty);

                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                try
                {
                    store.Open(OpenFlags.ReadOnly);
                    X509Certificate2Collection col = store.Certificates.Find(X509FindType.FindByThumbprint,
                        thumbprint, false); // Don't validate certs, since the test root isn't installed.
                    if (col == null || col.Count == 0)
                        return null;
                    return col[0];
                }
                finally
                {
                    store.Close();
                }
            }
        }
    }
}
