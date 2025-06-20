using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.domain.Models;
using news_application.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Repositories
{
    public class UserArticleInteractionRepository : IUserArticleInteractionRepository
    {
        private readonly NewsDataContext _context;

        public UserArticleInteractionRepository(NewsDataContext context)
        {
            _context = context;
        }

        public async Task<UserArticleInteraction?> GetInteractionAsync(int userId, int articleId)
        {
            return await _context.UserArticleInteractions
                .FirstOrDefaultAsync(x => x.UserId == userId && x.NewsArticleId == articleId);
        }

        public async Task AddOrUpdateInteractionAsync(UserArticleInteraction interaction)
        {
            var existing = await GetInteractionAsync(interaction.UserId, interaction.NewsArticleId);

            if (existing == null)
            {
                _context.UserArticleInteractions.Add(interaction);
            }
            else
            {
                existing.IsLiked = interaction.IsLiked;
                existing.IsDisliked = interaction.IsDisliked;
                existing.Timestamp = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
    }

}
