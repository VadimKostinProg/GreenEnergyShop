using InternetShop.Core.Domain.Entities;
using InternetShop.Core.Domain.RepositoryContracts;
using InternetShop.Infrastructure.DataBaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _db;

        public ArticleRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Article Create(Article entity)
        {
            _db.Articles.Add(entity);

            return entity;
        }

        public bool Delete(Guid id)
        {
            Article? article = _db.Articles.FirstOrDefault(x => x.Id == id);

            if(article == null)
            {
                return false;
            }

            _db.Articles.Remove(article);

            return true;
        }

        public IEnumerable<Article> GetAll()
        {
            return _db.Articles.AsEnumerable();
        }

        public Article? GetValueById(Guid id)
        {
            return _db.Articles.FirstOrDefault(x => x.Id == id);
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        public Article? Update(Article entity)
        {
            Article? article = _db.Articles.FirstOrDefault(x => x.Id == entity.Id);

            if(article == null)
            {
                return null;
            }

            article.Title = entity.Title;
            article.Description = entity.Description;
            article.IsHeaderArticle = entity.IsHeaderArticle;

            return article;
        }
    }
}
