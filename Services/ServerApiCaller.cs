using GameModel;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.JSInterop;
using System.Formats.Asn1;

namespace ClientSideBoard.Services
{
    public class ServerApiCaller
    {
        private const string SERVER_URI = "https://localhost:5001/";
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        public ServerApiCaller(HttpClient client, IJSRuntime jSRuntime)
        {
            _httpClient = client;
            _jsRuntime = jSRuntime;
        }

        private async Task<HttpRequestMessage> CreateApiRequestAsync(HttpMethod method, string uri)
        {
            var or = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/cookies.js");
            var token = await or.InvokeAsync<string>("getCookie", "gboard_signin_token");
            var request = new HttpRequestMessage(method, uri);
            request.Headers.Add("gboard_signin_token", token);
            return request;
        }

        public async Task<HttpResponseMessage> RemoveMediaAsync(IEnumerable<string> fileNames)
        {

        }

        public async Task<HttpResponseMessage> UploadMediaAsync(IBrowserFile file)
        {
            var request = await CreateApiRequestAsync(HttpMethod.Post, $"{SERVER_URI}UploadMedia/UploadImage");
            
            using var ms = new MemoryStream();

            await file.OpenReadStream().CopyToAsync(ms);

            request.Content = new ByteArrayContent(ms.ToArray());

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> UploadMediaAsync(IReadOnlyList<IBrowserFile> files)
        {
            var request = await CreateApiRequestAsync(HttpMethod.Post, $"{SERVER_URI}UploadMedia/UploadImage");

            using var ms = new MemoryStream();

            var content = new MultipartContent();
            foreach (var file in files)
            {
                await file.OpenReadStream().CopyToAsync(ms);
                content.Add(new ByteArrayContent(ms.ToArray()));
                await ms.FlushAsync();
            }

            request.Content = content;

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> GetUserMediaListAsync()
        {

        }

        public async Task<HttpResponseMessage> SynchronizeGameStateAsync(int gameId)
        {
            var request = await CreateApiRequestAsync(HttpMethod.Get, $"{SERVER_URI}Game/Sync/{gameId}");

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> SynchronizeChatAsync()
        {

        }


    }
}
