using Microsoft.AspNetCore.Components;
using MudBlazor;
using INDIGIPLUS.Client.DTOs.Lessons;
using INDIGIPLUS.Client.Models;
using INDIGIPLUS.Client.Services.Interfaces;

namespace INDIGIPLUS.Client.Components.Dialogs;

public partial class LessonDialog : ComponentBase
{
    [Inject] protected ILessonClientService LessonService { get; set; } = default!;
    [Inject] protected ISnackbar Snackbar { get; set; } = default!;
    [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public int? LessonId { get; set; }

    protected MudForm form = null!;
    protected LessonFormModel model = new();
    protected bool isEditMode = false;
    protected bool isLoading = false;
    protected bool isSaving = false;

    protected override async Task OnInitializedAsync()
    {
        isEditMode = LessonId.HasValue;

        if (isEditMode)
        {
            await LoadLessonAsync();
        }
    }

    protected async Task LoadLessonAsync()
    {
        isLoading = true;
        StateHasChanged(); // now it works

        try
        {
            var lesson = await LessonService.GetLessonByIdAsync(LessonId!.Value);
            if (lesson != null)
            {
                model = new LessonFormModel
                {
                    Id = lesson.Id,
                    Title = lesson.Title,
                    Description = lesson.Description,
                    Content = lesson.Content,
                    OrderIndex = lesson.OrderIndex
                };
            }
            else
            {
                Snackbar.Add("Lesson not found.", Severity.Warning);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error: {ex.Message}", Severity.Error);
        }

        isLoading = false;
        StateHasChanged(); // and here too
    }

    protected async Task Save()
    {
        await form.Validate();
        if (!form.IsValid)
        {
            Snackbar.Add("Please fix validation errors", Severity.Warning);
            return;
        }

        isSaving = true;
        StateHasChanged();

        try
        {
            bool success = false;

            if (isEditMode)
            {
                var result = await LessonService.UpdateLessonAsync(model.Id, new UpdateLessonDto
                {
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    OrderIndex = model.OrderIndex
                });
                success = result != null;
            }
            else
            {
                var result = await LessonService.CreateLessonAsync(new CreateLessonDto
                {
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    OrderIndex = model.OrderIndex
                });
                success = result != null;
            }

            if (success)
            {
                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                Snackbar.Add("Failed to save lesson", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error saving lesson: {ex.Message}", Severity.Error);
        }

        isSaving = false;
        StateHasChanged();
    }

    protected void Cancel() => MudDialog.Cancel();
}
