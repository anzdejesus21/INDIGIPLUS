using CppLearningPlatform.Models;
using INDIGIPLUS.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace INDIGIPLUS.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Role).HasConversion<int>();
            });

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserProgresses)
                .WithOne(up => up.User)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.QuizAttempts)
                .WithOne(qa => qa.User)
                .HasForeignKey(qa => qa.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Lessons)
                .WithOne(l => l.Course)
                .HasForeignKey(l => l.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lesson>()
                .HasMany(l => l.Quizzes)
                .WithOne(q => q.Lesson)
                .HasForeignKey(q => q.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lesson>()
                .HasMany(l => l.UserProgresses)
                .WithOne(up => up.Lesson)
                .HasForeignKey(up => up.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Quiz>()
                .HasMany(q => q.Questions)
                .WithOne(qn => qn.Quiz)
                .HasForeignKey(qn => qn.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Quiz>()
                .HasMany(q => q.QuizAttempts)
                .WithOne(qa => qa.Quiz)
                .HasForeignKey(qa => qa.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.AnswerOptions)
                .WithOne(ao => ao.Question)
                .HasForeignKey(ao => ao.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.UserAnswers)
                .WithOne(ua => ua.Question)
                .HasForeignKey(ua => ua.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuizAttempt>()
                .HasMany(qa => qa.UserAnswers)
                .WithOne(ua => ua.QuizAttempt)
                .HasForeignKey(ua => ua.QuizAttemptId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserAnswer>()
                .HasOne(ua => ua.SelectedAnswerOption)
                .WithMany()
                .HasForeignKey(ua => ua.SelectedAnswerOptionId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Achievement>()
                .HasMany(a => a.UserAchievements)
                .WithOne(ua => ua.Achievement)
                .HasForeignKey(ua => ua.AchievementId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserAchievement>()
                .HasOne(ua => ua.User)
                .WithMany()
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
