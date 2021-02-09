using ImageGallery.Core.Models;
using ImageGallery.Core.Models.ImageApi;
using ImageGallery.Core.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallery.Core.Api
{

    public class APIImageClient : IAPIImageClient
    {
        private string _authToken;

        private HttpClient httpClient;
        private AppSettings _settings;

        private readonly ILogger _logger;
        private readonly IImageDetailsRepository _imageDetailsRepository;

        public APIImageClient(ILogger<APIImageClient> logger, IImageDetailsRepository imageDetailsRepository, IOptionsSnapshot<AppSettings> settings)
        {
            httpClient = new HttpClient();

            _logger = logger;
            _settings = settings.Value;
            _imageDetailsRepository = imageDetailsRepository;
        }

        private async Task<BaseResponse> Get(string uri, string query = null, Type responseType = null)
        {
            if (query != null)
            {
                uri = $"{uri}?{query}";
            }

            if (_authToken == null)
            {
                await RefreshAuthTokenAsync();
            }

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
            

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();

            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.RequestUri = new Uri(uri);

            return await SendRequest(httpRequestMessage, responseType);
        }


        private async Task RefreshAuthTokenAsync()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_settings.APIEndpoint}/{Constants.APIMethods.Auth}");

            var authObject = new
            {
                apiKey = _settings.APIKey
            };

            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(authObject), Encoding.UTF8, "application/json");

            var apiKey = await SendRequest(requestMessage, typeof(AuthResponse));

            try
            {
                var result = apiKey.Response as AuthResponse;
                _authToken = result.Token;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source} | {ex.Message}");
            }
        }

        private async Task<BaseResponse> SendRequest(HttpRequestMessage requestMessage, Type responseType = null)
        {
            try
            {
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            await RefreshAuthTokenAsync();
                            break;
                    };
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject(responseContent, responseType);
                return new BaseResponse(result);
            }
            catch (Exception ex)
            {
                var errorInfo = $"{ex.Source} | {ex.Message}";

                _logger.LogError(errorInfo);
                return new BaseResponse(errorInfo);
            }
        }

        public async Task RefreshImagesData()
        {
            var pageNumber = 1;
            Images currentPage;

            var idsList = _imageDetailsRepository.GetAllQ().Select(x => x.Id).ToList();

            do
            {
                var imagesResponse = await Get($"{_settings.APIEndpoint}/{Constants.APIMethods.Images}", $"{Constants.APIParameters.Images.Page}={pageNumber}", typeof(Images));
                if (!imagesResponse.Success)
                {
                    break;
                }

                currentPage = imagesResponse.Response as Images;
                foreach (var image in currentPage.Pictures)
                {
                    var imageDetailsResponse = await Get($"{_settings.APIEndpoint}/{Constants.APIMethods.Images}/{image.Id}", null, typeof(ImageDetails));

                    if (!imageDetailsResponse.Success)
                    {
                        break;
                    }

                    var pictureDetails = imageDetailsResponse.Response as ImageDetails;

                    var containsEntity = idsList.Contains(image.Id);
                    if (containsEntity)
                    {
                        await _imageDetailsRepository.UpdateAsync(pictureDetails);
                        idsList.Remove(image.Id);
                    }
                    else
                    {
                        await _imageDetailsRepository.AddAsync(pictureDetails);
                        await _imageDetailsRepository.CommitAsync();
                    }

                }

                pageNumber++;
            }
            while (currentPage != null && currentPage.HasMore);

            foreach(var id in idsList)
            {
                _imageDetailsRepository.Delete(id);
            }
        }
    }
}
