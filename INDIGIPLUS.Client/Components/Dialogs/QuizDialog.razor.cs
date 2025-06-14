using Microsoft.AspNetCore.Components;
using MudBlazor;
using INDIGIPLUS.Client.DTOs.Lessons;
using INDIGIPLUS.Client.DTOs.Quizs;
using INDIGIPLUS.Client.Models;
using INDIGIPLUS.Client.Services.Interfaces;

namespace INDIGIPLUS.Client.Components.Dialogs;

public partial class QuizDialog : ComponentBase
{
    [Inject] protected IQuizClientService QuizService { get; set; } = default!;
    [Inject] protected ILessonClientService LessonService { get; set; } = default!;
    [Inject] protected ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public int? QuizId { get; set; }
    [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = default!;

    protected bool IsEditMode => QuizId.HasValue && QuizId.Value > 0;
    protected bool isLoading = false;
    protected bool isSaving = false;
    protected MudForm form = null!;
    protected List<LessonDto> lessons = new();
    protected QuizFormModel quizModel = new();

    protected override async Task OnParametersSetAsync()
    {
        await LoadData();
    }

    protected async Task LoadData()
    {
        isLoading = true;
        StateHasChanged();

        try
        {
            // Load lessons first
            lessons = await LessonService.GetAllLessonsAsync();

            // If editing, load the quiz data
            if (IsEditMode)
            {
                var quiz = await QuizService.GetQuizByIdAsync(QuizId!.Value);
                if (quiz == null)
                {
                    Snackbar.Add("Quiz not found", Severity.Warning);
                    MudDialog.Cancel();
                    return;
                }

                quizModel = new QuizFormModel
                {
                    Id = quiz.Id,
                    Title = quiz.Title ?? string.Empty,
                    Description = quiz.Description ?? string.Empty,
                    LessonId = quiz.LessonId
                };
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading data: {ex.Message}", Severity.Error);
        }
        finally
        {
            isLoading = false;
            StateHasChanged();
        }
    }

    protected void Cancel()
    {
        MudDialog.Cancel();
    }

    protected async Task Save()
    {
        if (form == null)
        {
            Snackbar.Add("Form not initialized", Severity.Error);
            return;
        }

        await form.Validate();
        if (!form.IsValid)
        {
            Snackbar.Add("Please fix validation errors", Severity.Warning);
            return;
        }

        // Additional validation
        if (string.IsNullOrWhiteSpace(quizModel.Title))
        {
            Snackbar.Add("Title is required", Severity.Warning);
            return;
        }

        if (quizModel.LessonId <= 0)
        {
            Snackbar.Add("Please select a lesson", Severity.Warning);
            return;
        }

        isSaving = true;
        StateHasChanged();

        try
        {
            bool success = false;

            if (IsEditMode)
            {
                // Update existing quiz
                var updateDto = new UpdateQuizDto
                {
                    Title = quizModel.Title,
                    Description = quizModel.Description,
                    LessonId = quizModel.LessonId
                };

                Console.WriteLine($"Updating quiz {quizModel.Id} with title: '{quizModel.Title}'");
                var result = await QuizService.UpdateQuizAsync(quizModel.Id, updateDto);
                success = result != null;
            }
            else
            {
                // Create new quiz
                var createDto = new CreateQuizDto
                {
                    Title = quizModel.Title,
                    Description = quizModel.Description,
                    LessonId = quizModel.LessonId
                };

                Console.WriteLine($"Creating quiz with title: '{quizModel.Title}'");
                var result = await QuizService.CreateQuizAsync(createDto);
                success = result != null;
            }

            if (success)
            {
                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                Snackbar.Add($"Failed to {(IsEditMode ? "update" : "create")} quiz", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving quiz: {ex}");
            Snackbar.Add($"Error {(IsEditMode ? "updating" : "creating")} quiz: {ex.Message}", Severity.Error);
        }
        finally
        {
            isSaving = false;
            StateHasChanged();
        }
    }
}