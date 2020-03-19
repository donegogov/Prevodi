using Google.Cloud.SecretManager.V1Beta1;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacedonianCroatianEnglishGermanTranslateV3.Helpers
{
    public class GoogleCloudPlatformSecretManager
    {
        private SecretManagerServiceClient client { get; set; }

        public GoogleCloudPlatformSecretManager() { }

        public async Task<String> getSecretManagerValues(String project_id, String secret_id, String secret_version_id)
        {
                client = await SecretManagerServiceClient.CreateAsync();
            // Create the request.
            var request = new AccessSecretVersionRequest
            {
                SecretVersionName = new SecretVersionName(project_id, secret_id, secret_version_id),
            };

            // Access the secret and print the result.
            //
            var version = await client.AccessSecretVersionAsync(request);
            string payload = version.Payload.Data.ToStringUtf8();

            return payload;
        }
    }
}
