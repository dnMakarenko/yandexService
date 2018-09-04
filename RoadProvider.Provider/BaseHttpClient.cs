using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace RoadProvider.Provider
{
    public class BaseHttpClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private Parameters _parameters;

        private string providerUrl = ConfigurationManager.AppSettings["apiUrl"];

        public BaseHttpClient()
        {
            _httpClient = GetClient();
        }

        public BaseHttpClient(string regionCode)
        {
            _parameters = GenerateParams(regionCode);
            _httpClient = GetClient();
        }

        public async Task<string> GetRegion(string regionCode)
        {
            try
            {
                _parameters = GenerateParams(regionCode);
                var response = await RequestAsync();

                XDocument doc = XDocument.Parse(response);

                var trafficNode = doc.Element("info").Element("traffic").Element("region");
                try
                {
                    var hintNodes = from hintEl in trafficNode.Elements("hint")
                                    where (string)hintEl.Attribute("lang") == "en"
                                    select hintEl;

                    string trafficHint = hintNodes.Select(q => q.Value).First();

                    return trafficHint;
                }
                catch
                {
                    throw new Exception(string.Format("Provider couldn't have traffic information. Region Code '{0}'.", regionCode));
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Couldn't return traffic from provider. Region Code '{0}'. See inner exception.", regionCode), ex);
            }
        }

        private async Task<string> RequestAsync()
        {

            var path = MakeUri();

            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(path).Result;
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Uri MakeUri()
        {
            try
            {
                var mergedParams = HttpUtility.ParseQueryString(String.Empty);
                var Params = _parameters.ParamsAsCollection();


                foreach (string key in Params)
                    mergedParams[key] = Params[key];

                return new UriBuilder(providerUrl) { Query = mergedParams.ToString() }.Uri;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private Parameters GenerateParams(string regionCode)
        {
            return new Parameters()
            {
                Region = regionCode,
                BustCache = DateTime.Now.ToString()
            };
        }

        private HttpClient GetClient()
        {
            try
            {
                return new HttpClient()
                {
                    BaseAddress = new Uri(providerUrl),
                    DefaultRequestHeaders =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
                },
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            _parameters = null;
            _httpClient.Dispose();
        }
    }
}
