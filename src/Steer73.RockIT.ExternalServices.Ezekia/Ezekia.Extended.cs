using System.Net.Http.Headers;

namespace EzekiaCRM
{
    public partial interface IClient
    {
        Task<T> GetNextPageAsync<T>(string nextPageUrl, CancellationToken cancellationToken) where T : class;

        /// <summary>
        /// Add documents and attachments.
        /// </summary>
        /// <param name="type">The type of model to add a document to.</param>
        /// <param name="id">The item ID of the model.</param>
        /// <param name="documents">Documents to upload</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        Task<document> UploadDocuments(
            Type5 type, 
            string id, 
            IReadOnlyCollection<DocumentDto> documents,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds phone number for a person
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="body"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        Task<PhonePostResponse> PeoplePhonesPostAsync(
           int personId,
           phones2 body,
           CancellationToken cancellationToken);
    }

    public partial class Client : IClient
    {
        public virtual async Task<T> GetNextPageAsync<T>(string nextPageUrl, CancellationToken cancellationToken) where T : class
        {
            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    request_.Method = new System.Net.Http.HttpMethod("GET");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
                    request_.RequestUri = new System.Uri(nextPageUrl, System.UriKind.RelativeOrAbsolute);

                    PrepareRequest(client_, request_, nextPageUrl);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    //var responseText = await response_.Content.ReadAsStringAsync().ConfigureAwait(false);

                    try
                    {
                        var headers_ = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>>();
                        foreach (var item_ in response_.Headers)
                            headers_[item_.Key] = item_.Value;
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<T>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }

       
        public virtual async Task<document> UploadDocuments(
            Type5 type,
            string id,
            IReadOnlyCollection<DocumentDto> documents,
            CancellationToken cancellationToken = default)
        {
            var client_ = _httpClient;
            var disposeClient_ = false;

            try
            {
                using var request_ = new HttpRequestMessage();
                using var content_ = new MultipartFormDataContent();
                foreach (var item in documents)
                {
                    item.Stream.Position = 0;
                    var streamContent = new StreamContent(item.Stream);
                    streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(item.ContentType);
                    content_.Add(streamContent, "files[]", item.FileName);
                }
                
                request_.Content = content_;
                request_.Method = new HttpMethod("POST");
                request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var urlBuilder_ = new System.Text.StringBuilder();
                if (!string.IsNullOrEmpty(_baseUrl)) urlBuilder_.Append(_baseUrl);
                // Operation Path: "{type}/{id}/documents"
                urlBuilder_.Append(Uri.EscapeDataString(ConvertToString(type, System.Globalization.CultureInfo.InvariantCulture)));
                urlBuilder_.Append('/');
                urlBuilder_.Append(Uri.EscapeDataString(id));
                urlBuilder_.Append("/documents");

                PrepareRequest(client_, request_, urlBuilder_);

                var url_ = urlBuilder_.ToString();
                request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

                PrepareRequest(client_, request_, url_);

                var response_ = await client_.SendAsync(
                    request_,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken).ConfigureAwait(false);
                var disposeResponse_ = true;
                try
                {
                    var headers_ = new Dictionary<string, IEnumerable<string>>();
                    foreach (var item_ in response_.Headers)
                        headers_[item_.Key] = item_.Value;
                    if (response_.Content != null && response_.Content.Headers != null)
                    {
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;
                    }

                    ProcessResponse(client_, response_);

                    var status_ = (int)response_.StatusCode;
                    if (status_ == 200)
                    {
                        var objectResponse_ = await ReadObjectResponseAsync<document>(response_, headers_, cancellationToken).ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                        }
                        return objectResponse_.Object;
                    }
                    else
                    if (status_ == 404)
                    {
                        string responseText_ = (response_.Content == null) ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                        throw new ApiException("Item with that ID doesn\'t exist.", status_, responseText_, headers_, null);
                    }
                    else
                    if (status_ == 403)
                    {
                        string responseText_ = (response_.Content == null) ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                        throw new ApiException("The authenticated user does not have permission to see that item.", status_, responseText_, headers_, null);
                    }
                    else
                    if (status_ == 422)
                    {
                        string responseText_ = (response_.Content == null) ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var objectResponse_ = await ReadObjectResponseAsync<_422>(response_, headers_, cancellationToken).ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_, responseText_, headers_, null);
                        }
                        throw new ApiException<_422>("Data was invalid.", status_, responseText_, headers_, objectResponse_.Object, null);
                    }
                    else
                    {
                        var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                        throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                    }
                }
                finally
                {
                    if (disposeResponse_)
                        response_.Dispose();
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }

        public async Task<PhonePostResponse> PeoplePhonesPostAsync(
            int id,
            phones2 body,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(body);

            var client_ = _httpClient;
            var disposeClient_ = false;

            try
            {
                using var request_ = new HttpRequestMessage();

                var json_ = Newtonsoft.Json.JsonConvert.SerializeObject(body, JsonSerializerSettings);
                var content_ = new StringContent(json_);
                content_.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                request_.Content = content_;
                request_.Method = new HttpMethod("POST");
                request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var urlBuilder_ = new System.Text.StringBuilder();
                if (!string.IsNullOrEmpty(_baseUrl)) urlBuilder_.Append(_baseUrl);
                // Operation Path: "people/{id}/phones"
                urlBuilder_.Append("people/");
                urlBuilder_.Append(Uri.EscapeDataString(ConvertToString(id, System.Globalization.CultureInfo.InvariantCulture)));
                urlBuilder_.Append("/phones");

                PrepareRequest(client_, request_, urlBuilder_);

                var url_ = urlBuilder_.ToString();
                request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

                PrepareRequest(client_, request_, url_);

                var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                var disposeResponse_ = true;
                try
                {
                    var headers_ = new Dictionary<string, IEnumerable<string>>();
                    foreach (var item_ in response_.Headers)
                        headers_[item_.Key] = item_.Value;
                    if (response_.Content != null && response_.Content.Headers != null)
                    {
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;
                    }

                    ProcessResponse(client_, response_);

                    var status_ = (int)response_.StatusCode;
                    if (status_ == 200 
                        || status_ == 201)
                    {
                        var objectResponse_ = await ReadObjectResponseAsync<PhonePostResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                        }
                        return objectResponse_.Object;
                    }
                    else
                    if (status_ == 404)
                    {
                        string responseText_ = (response_.Content == null) ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                        throw new ApiException("Item or note with that ID doesn\'t exist.", status_, responseText_, headers_, null);
                    }
                    else
                    if (status_ == 403)
                    {
                        string responseText_ = (response_.Content == null) ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                        throw new ApiException("The authenticated user does not have permission to update that person.", status_, responseText_, headers_, null);
                    }
                    else
                    if (status_ == 422)
                    {
                        var objectResponse_ = await ReadObjectResponseAsync<_422>(response_, headers_, cancellationToken).ConfigureAwait(false);
                        if (objectResponse_.Object == null)
                        {
                            throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                        }
                        throw new ApiException<_422>("Data was invalid.", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                    }
                    else
                    {
                        var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                        throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                    }
                }
                finally
                {
                    if (disposeResponse_)
                        response_.Dispose();
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }
    }

    public class DocumentDto
    {
        public string ContentType { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public Stream Stream { get; set; } = null!;
    }

    public class store6Extended : store6
    {
        [Newtonsoft.Json.JsonProperty("aliases", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ICollection<string> Aliases { get; set; } = [];
    }

    public partial class PhonePostResponse
    {
        [Newtonsoft.Json.JsonProperty("data", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public phones Data { get; set; } = null!;
    }
}

