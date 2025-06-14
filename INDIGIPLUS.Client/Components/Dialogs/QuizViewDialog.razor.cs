using Microsoft.AspNetCore.Components;
using MudBlazor;
using INDIGIPLUS.Client.DTOs.Quizs;
using INDIGIPLUS.Client.Services.Interfaces;

namespace INDIGIPLUS.Client.Components.Dialogs;

public partial class QuizViewDialog : ComponentBase
{
    [Inject] protected IQuizClientService QuizService { get; set; } = default!;
    [Inject] protected ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public int QuizId { get; set; }
    [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = default!;

    protected QuizDto? quiz = null;
    protected bool isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadQuiz();
    }

    protected async Task LoadQuiz()
    {
        if (QuizId <= 0)
        {
            Snackbar.Add("Invalid quiz ID", Severity.Warning);
            return;
        }

        isLoading = true;
        StateHasChanged();

        try
        {
            quiz = await QuizService.GetQuizByIdAsync(QuizId);

            if (quiz == null)
            {
                Snackbar.Add("Quiz not found", Severity.Warning);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading quiz: {ex}");
            Snackbar.Add($"Error loading quiz: {ex.Message}", Severity.Error);
        }
        finally
        {
            isLoading = false;
            StateHasChanged();
        }
    }

    protected void Close()
    {
        MudDialog.Close();
    }
}