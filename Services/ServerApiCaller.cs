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

        public async Task<HttpResponseMessage> UploadMediaAsync(IBrowserFile file) //to do
        {
            var request = await CreateApiRequestAsync(HttpMethod.Post, $"{SERVER_URI}Media/Upload");

            var content = new MultipartContent();

            var media = new MediaFile()
            {
                Type = MediaType.Image,
                Size = file.Size,
                UserDisplayName = file.Name
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(media);

            using var ms = new MemoryStream();

            await file.OpenReadStream().CopyToAsync(ms);

            content.Add(new ByteArrayContent(ms.ToArray()));

            content.Add(new StringContent( json));

            request.Content = content;

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> UploadMediaAsync(IReadOnlyList<IBrowserFile> files) //to do
        {
            var request = await CreateApiRequestAsync(HttpMethod.Post, $"{SERVER_URI}UploadMedia/UploadImage");

            using var ms = new MemoryStream();

            using var content = new MultipartContent();
            
            foreach (var file in files)
            {
                content.Add(new StreamContent(file.OpenReadStream()));
            }

            request.Content = content;

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> GetUserMediaListAsync() //to do
        {
            var request = await CreateApiRequestAsync(HttpMethod.Get, $"{SERVER_URI}Media/");

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
