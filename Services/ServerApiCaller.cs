using GameModel;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.JSInterop;
using System.Formats.Asn1;
using System.IO.Pipelines;

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

        public async Task<HttpResponseMessage> RemoveMediaAsync(MediaFile file) //to do
        {
            var request = await CreateApiRequestAsync(HttpMethod.Get, $"{SERVER_URI}Media/Remove{file.Id}");

            return await _httpClient.SendAsync(request);

        }

        public async Task<HttpResponseMessage> UploadMediaAsync(IReadOnlyList<IBrowserFile> files) //to do
        {
            var request = await CreateApiRequestAsync(HttpMethod.Post, $"{SERVER_URI}Media/Upload");

            using var content = new MultipartFormDataContent();

            content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");
            
            foreach (var file in files)
            {
                /*var resized = await file.RequestImageFileAsync(file.ContentType, 640, 480);
                var buf = new byte[resized.Size];
                using(var stream = file.OpenReadStream())
                {
                    await stream.ReadAsync(buf);
                }

                await file.OpenReadStream().CopyToAsync(ms);*/
                var fileContent = new StreamContent(file.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                content.Add(
                    content : fileContent,
                    name: "image",
                    fileName: file.Name);
                
            }

            request.Content = content;

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> GetUserMediaListAsync() //to do
        {
            var request = await CreateApiRequestAsync(HttpMethod.Get, $"{SERVER_URI}Media/MediaList");

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> SynchronizeGameStateAsync(int gameId) //to do
        {
            var request = await CreateApiRequestAsync(HttpMethod.Get, $"{SERVER_URI}Game/Sync/{gameId}");

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> SynchronizeChatAsync(int gameId) //to do
        {
            var request = await CreateApiRequestAsync(HttpMethod.Get, $"{SERVER_URI}Game/Chat/{gameId}");

            return await _httpClient.SendAsync(request);
        }


    }
}
