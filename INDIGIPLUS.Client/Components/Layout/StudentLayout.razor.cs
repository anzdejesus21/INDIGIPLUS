using INDIGIPLUS.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace INDIGIPLUS.Client.Components.Layout
{
    public partial class StudentLayout : LayoutComponentBase
    {
        [Inject] private IAuthClientService AuthClientService { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        private bool _drawerOpen = false;

        /// <summary>
        /// Toggles the drawer open/closed state
        /// </summary>
        private void ToggleDrawer()
        {
            _drawerOpen = !_drawerOpen;
        }

        /// <summary>
        /// Handles user logout process
        /// </summary>
        /// <returns>Task representing the logout operation</returns>
        private async Task HandleLogoutAsync()
        {
            try
            {
                await AuthClientService.LogoutAsync();
                NavigationManager.NavigateTo("/");
            }
            catch (Exception ex)
            {
                // Log exception or handle gracefully
                Console.WriteLine($"Error during logout: {ex.Message}");
                // Optionally show user-friendly error message
                // You could inject ISnackbar here to show error messages
            }
        }

        /// <summary>
        /// Closes the drawer (useful for mobile navigation)
        /// </summary>
        private void CloseDrawer()
        {
            _drawerOpen = false;
        }

        /// <summary>
        /// Opens the drawer
        /// </summary>
        private void OpenDrawer()
        {
            _drawerOpen = true;
        }

        /// <summary>
        /// Navigates to a specific URL and closes the drawer on mobile
        /// </summary>
        /// <param name="url">The URL to navigate to</param>
        private void NavigateAndCloseDrawer(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return;

            NavigationManager.NavigateTo(url);
            CloseDrawer(); // Close drawer after navigation for better mobile UX
        }

        /// <summary>
        /// Checks if the current page matches the specified path
        /// </summary>
        /// <param name="path">The path to check against</param>
        /// <returns>True if current page matches the path</returns>
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

        // Optional: Add dispose pattern if needed for cleanup
        public void Dispose()
        {
            // Clean up any event subscriptions or resources if needed
        }
    }
}
