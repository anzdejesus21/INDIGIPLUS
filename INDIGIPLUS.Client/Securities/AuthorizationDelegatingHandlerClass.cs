//using Microsoft.JSInterop;
//using System.Net.Http.Headers;

//namespace INDIGIPLUS.Client.Securities
//{
//    public class AuthorizationDelegatingHandler : DelegatingHandler
//    {
//        #region Fields

//        private readonly ILocalStorageService _localStorage;
//        private readonly IJSRuntime _jsRuntime;
//        private readonly ILogger<AuthorizationDelegatingHandler> _logger;

//        #endregion Fields

//        #region Public Constructors

//        public AuthorizationDelegatingHandler(
//            ILocalStorageService localStorage,
//            IJSRuntime jsRuntime,
//            ILogger<AuthorizationDelegatingHandler> logger)
//        {
//            _localStorage = localStorage;
//            _jsRuntime = jsRuntime;
//            _logger = logger;
//        }

//        #endregion Public Constructors

//        #region Protected Methods

//        protected override async Task<HttpResponseMessage> SendAsync(
//            HttpRequestMessage request,
//            CancellationToken cancellationToken)
//        {
//            try
//            {
//                // Skip token attachment for auth endpoints
//                if (request.RequestUri?.AbsolutePath.Contains("/api/auth/") == true)
//                {
//                    return await base.SendAsync(request, cancellationToken);
//                }

//                var token = await GetTokenSafelyAsync();

//                if (!string.IsNullOrWhiteSpace(token))
//                {
//                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
//                    _logger.LogDebug("Token attached to request: {Uri}", request.RequestUri);
//                }
//                else
//                {
//                    _logger.LogDebug("No token available for request: {Uri}", request.RequestUri);
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogWarning(ex, "Failed to attach authorization token to request: {Uri}", request.RequestUri);
//            }

//            return await base.SendAsync(request, cancellationToken);
//        }

//        #endregion Protected Methods

//        #region Private Methods

//        private async Task<string?> GetTokenSafelyAsync()
//        {
//            try
//            {
//                // Check if JavaScript interop is available
//                if (_jsRuntime is IJSInProcessRuntime)
//                {
//                    return await _localStorage.GetItemAsync<string>("authToken");
//                }

//                // For server-side rendering, try to get token with timeout
//                using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));
//                return await _localStorage.GetItemAsync<string>("authToken");
//            }
//            catch (InvalidOperationException)
//            {
//                // JavaScript interop not available during prerendering
//                return null;
//            }
//            catch (TaskCanceledException)
//            {
//                // Timeout occurred
//                return null;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogWarning(ex, "Failed to retrieve token from localStorage");
//                return null;
//            }
//        }
//    }
//}