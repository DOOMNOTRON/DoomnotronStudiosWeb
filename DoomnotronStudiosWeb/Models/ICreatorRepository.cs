using DoomnotronStudiosWeb.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace DoomnotronStudiosWeb.Models
{
    public interface ICreatorRepository
    {
        Task SaveCreator(Creator creator);

        Task<IEnumerable<Creator>> GetAllCreators();

        Task DeleteCreator(int creatorId);

        Task UpdateCreator(Creator creator);

        Task<Creator?> GetCreator(int creatorId);
         
    }

    public class CreatorRepository : ICreatorRepository
    {
        private ApplicationDbContext _context;
        public CreatorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteCreator(int creatorId)
        {
            Creator? creator = await GetCreator(creatorId);

            if (creator != null)
            {
                // Change all related comics to a null Creator
                using DbConnection con = _context.Database.GetDbConnection();
                await con.OpenAsync();
                using DbCommand query = con.CreateCommand();
                query.CommandText = "Update Comics SET ComicCreatorId = null Where ComicCreatorId = " + creator.Id;
                int rowsAffected = await query.ExecuteNonQueryAsync();

                // Remove creator from database
                _context.Creators.Remove(creator);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Creator>> GetAllCreators()
        {
            return await _context.Creators.OrderBy(Creator => Creator.FullName).ToListAsync();
        }

        public async Task<Creator?> GetCreator(int creatorId)
        {
            return await _context.Creators.SingleOrDefaultAsync(i => i.Id == creatorId);
        }

        public async Task SaveCreator(Creator creator)
        {
            _context.Creators.Add(creator);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCreator(Creator creator)
        {
            _context.Add(creator);
            await _context.SaveChangesAsync();
        }
    }
}
