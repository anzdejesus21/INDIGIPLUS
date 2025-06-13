using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace INDIGIPLUS.Client.Components.Dialogs
{
    public partial class ConfirmationDialog : ComponentBase
    {
        [CascadingParameter]
        public IMudDialogInstance MudDialog { get; set; } = default!;

        [Parameter]
        public string ContentText { get; set; } = string.Empty;

        [Parameter]
        public string ButtonText { get; set; } = "Confirm";

        [Parameter]
        public Color Color { get; set; } = Color.Primary;

        private void Submit()
        {
            MudDialog.Close(DialogResult.Ok(true));
        }

        private void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
