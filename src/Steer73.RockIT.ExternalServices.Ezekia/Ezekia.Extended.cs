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
        /// <summary>
        /// Partial method implementation to configure custom JSON serializer settings.
        /// This method is called by the generated client code and survives Swagger regeneration.
        /// </summary>
        static partial void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings)
        {
            // Use custom contract resolver to override property-level StringEnumConverter attributes
            settings.ContractResolver = new PositionTypeContractResolver();
        }

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

    /// <summary>
    /// Contract resolver that replaces StringEnumConverter with PositionTypeConverter for PositionType properties.
    /// This is necessary because property-level [JsonConverter] attributes override global converters.
    /// </summary>
    public class PositionTypeContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override Newtonsoft.Json.Serialization.JsonProperty CreateProperty(
            System.Reflection.MemberInfo member,
            Newtonsoft.Json.MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            // Check if this property is a PositionType or EzekiaPositionType
            var propertyType = property.PropertyType;
            var underlyingType = System.Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (underlyingType == typeof(PositionType) || underlyingType == typeof(EzekiaPositionType))
            {
                // Replace any existing converter with our custom one
                property.Converter = new PositionTypeConverter();
            }

            return property;
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

    /// <summary>
    /// Canonical list of Ezekia position types we care about, including values
    /// that may be missing from the generated swagger enum.
    /// </summary>
    public enum EzekiaPositionType
    {
        Permanent = 0,
        Interim = 1,
        Parttime = 2,
        Contract = 3,
        Temporary = 4,
        Ned = 5,
        Other = 6
    }

    /// <summary>
    /// Custom converter for PositionType enum to handle unknown values from the API.
    /// If an unknown value is encountered (e.g., Swagger regeneration misses "other"),
    /// we map it to EzekiaPositionType.Other while emitting a synthetic PositionType value.
    /// </summary>
    public class PositionTypeConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(System.Type objectType)
        {
            var targetType = System.Nullable.GetUnderlyingType(objectType) ?? objectType;
            return targetType == typeof(PositionType) || targetType == typeof(EzekiaPositionType);
        }

        public override object? ReadJson(Newtonsoft.Json.JsonReader reader, System.Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.TokenType == Newtonsoft.Json.JsonToken.Null)
            {
                if (AllowsNull(objectType))
                {
                    return null;
                }

                throw new Newtonsoft.Json.JsonSerializationException($"Cannot convert null value to {objectType}.");
            }

            if (reader.TokenType != Newtonsoft.Json.JsonToken.String && reader.TokenType != Newtonsoft.Json.JsonToken.Integer)
            {
                throw new Newtonsoft.Json.JsonSerializationException($"Unexpected token {reader.TokenType} when parsing Ezekia position type.");
            }

            var rawValue = reader.Value?.ToString();
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                if (AllowsNull(objectType))
                {
                    return null;
                }

                throw new Newtonsoft.Json.JsonSerializationException("Position type value cannot be empty.");
            }

            var ezekiaValue = ParseEzekiaPositionType(rawValue);
            var targetType = System.Nullable.GetUnderlyingType(objectType) ?? objectType;

            if (targetType == typeof(PositionType))
            {
                return MapToGeneratedEnum(ezekiaValue);
            }

            if (targetType == typeof(EzekiaPositionType))
            {
                return ezekiaValue;
            }

            throw new Newtonsoft.Json.JsonSerializationException($"Unsupported target type {objectType} for PositionTypeConverter.");
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var actualType = value.GetType();

            if (actualType == typeof(PositionType))
            {
                var stringValue = SerializeEzekiaPositionType(MapToCanonicalEnum((PositionType)value));
                writer.WriteValue(stringValue);
                return;
            }

            if (actualType == typeof(EzekiaPositionType))
            {
                writer.WriteValue(SerializeEzekiaPositionType((EzekiaPositionType)value));
                return;
            }

            throw new Newtonsoft.Json.JsonSerializationException($"Unsupported value type {actualType} for PositionTypeConverter.");
        }

        private static bool AllowsNull(System.Type type)
        {
            return !type.IsValueType || System.Nullable.GetUnderlyingType(type) != null;
        }

        private static EzekiaPositionType ParseEzekiaPositionType(string rawValue)
        {
            switch (rawValue.Trim().ToLowerInvariant())
            {
                case "permanent":
                    return EzekiaPositionType.Permanent;
                case "interim":
                    return EzekiaPositionType.Interim;
                case "parttime":
                    return EzekiaPositionType.Parttime;
                case "contract":
                    return EzekiaPositionType.Contract;
                case "temporary":
                    return EzekiaPositionType.Temporary;
                case "ned":
                    return EzekiaPositionType.Ned;
                case "other":
                    return EzekiaPositionType.Other;
                default:
                    return EzekiaPositionType.Other;
            }
        }

        private static string SerializeEzekiaPositionType(EzekiaPositionType positionType)
        {
            return positionType switch
            {
                EzekiaPositionType.Permanent => "permanent",
                EzekiaPositionType.Interim => "interim",
                EzekiaPositionType.Parttime => "parttime",
                EzekiaPositionType.Contract => "contract",
                EzekiaPositionType.Temporary => "temporary",
                EzekiaPositionType.Ned => "ned",
                EzekiaPositionType.Other => "other",
                _ => "other"
            };
        }

        private static PositionType MapToGeneratedEnum(EzekiaPositionType positionType)
        {
            switch (positionType)
            {
                case EzekiaPositionType.Permanent:
                    return PositionType.Permanent;
                case EzekiaPositionType.Interim:
                    return PositionType.Interim;
                case EzekiaPositionType.Parttime:
                    return PositionType.Parttime;
                case EzekiaPositionType.Contract:
                    return PositionType.Contract;
                case EzekiaPositionType.Temporary:
                    return PositionType.Temporary;
                case EzekiaPositionType.Ned:
                    return PositionType.Ned;
                case EzekiaPositionType.Other:
                default:
                    return (PositionType)(int)EzekiaPositionType.Other;
            }
        }

        private static EzekiaPositionType MapToCanonicalEnum(PositionType positionType)
        {
            var numericValue = System.Convert.ToInt32(positionType);
            if (System.Enum.IsDefined(typeof(EzekiaPositionType), numericValue))
            {
                return (EzekiaPositionType)numericValue;
            }

            return EzekiaPositionType.Other;
        }
    }
}
