using INDIGIPLUS.Client.DTOs.Lessons;
using INDIGIPLUS.Client.DTOs.Quizs;
using INDIGIPLUS.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace INDIGIPLUS.Client.Components.Pages.Student
{
    public class StudentDashboardBase : ComponentBase
    {
        protected List<LessonDto> recentLessons = new();
        protected List<QuizDto> recentQuizzes = new();
        protected int totalLessons = 0;
        protected int totalQuizzes = 0;
        protected bool isLoading = false;

        [Inject] protected ILessonClientService LessonService { get; set; } = default!;
        [Inject] protected IQuizClientService QuizService { get; set; } = default!;
        [Inject] protected IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await LoadDashboardData();
        }

        protected async Task LoadDashboardData()
        {
            isLoading = true;

            try
            {
                await Task.Delay(3000);
                var lessonsTask = LessonService.GetAllLessonsAsync();
                var quizzesTask = QuizService.GetAllQuizzesAsync();

                await Task.WhenAll(lessonsTask, quizzesTask);

                var allLessons = await lessonsTask;
                var allQuizzes = await quizzesTask;

                totalLessons = allLessons?.Count ?? 0;
                totalQuizzes = allQuizzes?.Count ?? 0;

                recentLessons = allLessons?
                    .OrderByDescending(l => l.CreatedAt)
                    .Take(5)
                    .ToList() ?? new List<LessonDto>();

                recentQuizzes = allQuizzes?
                    .OrderByDescending(q => q.CreatedAt)
                    .Take(6)
                    .ToList() ?? new List<QuizDto>();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error loading dashboard data: {ex.Message}", Severity.Error);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }
    }
}
