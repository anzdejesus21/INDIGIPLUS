using INDIGIPLUS.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace INDIGIPLUS.Client.Components.Layout
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject] private IAuthClientService AuthClientService { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private bool isNavVisible = true;
        private bool isMobileView = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await InitializeMobileView();
            }
        }

        private async Task InitializeMobileView()
        {
            try
            {
                // Hide nav by default on mobile for better UX
                var width = await JSRuntime.InvokeAsync<int>("eval", "window.innerWidth");
                isMobileView = width <= 768;

                if (isMobileView)
                {
                    isNavVisible = false;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                // Log exception or handle gracefully
                Console.WriteLine($"Error initializing mobile view: {ex.Message}");
            }
        }

        private void ToggleNav()
        {
            isNavVisible = !isNavVisible;
        }

        private void NavigateAndClose(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return;

            if (isMobileView)
            {
                isNavVisible = false;
            }

            NavigationManager.NavigateTo(url);
        }

        private bool IsCurrentPage(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            try
            {
                var currentPath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
                return currentPath.Equals(path.TrimStart('/'), StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private async Task HandleLogoutAsync()
        {
            try
            {
                await AuthClientService.LogoutAsync();
                NavigationManager.NavigateTo("/");

                if (isMobileView)
                {
                    isNavVisible = false;
                }
            }
            catch (Exception ex)
            {
                // Log exception or handle gracefully
                Console.WriteLine($"Error during logout: {ex.Message}");
                // Optionally show user-friendly error message
            }
        }

        // Optional: Method to handle window resize events
        [JSInvokable]
        public async Task OnWindowResize(int width)
        {
            var wasMobile = isMobileView;
            isMobileView = width <= 768;

            // If switching from desktop to mobile, hide nav
            if (!wasMobile && isMobileView && isNavVisible)
            {
                isNavVisible = false;
                StateHasChanged();
            }
            // If switching from mobile to desktop, show nav
            else if (wasMobile && !isMobileView && !isNavVisible)
            {
                isNavVisible = true;
                StateHasChanged();
            }
        }

        // Optional: Add dispose pattern if needed for event cleanup
        public void Dispose()
        {
            // Clean up any event subscriptions or resources if needed
        }
    }
}