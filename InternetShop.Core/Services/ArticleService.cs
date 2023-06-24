using InternetShop.Core.Domain.Entities;
using InternetShop.Core.Domain.RepositoryContracts;
using InternetShop.Core.DTO;
using InternetShop.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<ArticleResponse> AddArticle(ArticleAddRequest articleAddRequest)
        {
            if(articleAddRequest == null) 
                throw new ArgumentNullException(nameof(articleAddRequest));

            if(_articleRepository.GetAll().Any(article => article.Title == articleAddRequest.Title))
                throw new ArgumentException("Стаття з такою назвою вже існує");

            if (articleAddRequest.Description == null)
                articleAddRequest.Description = string.Empty;

            Article article = articleAddRequest.ToArticle();
            article.Id = Guid.NewGuid();

            _articleRepository.Create(article);

            await _articleRepository.Save();

            return article.ToArticleResponse();
        }

        public async Task<bool> DeleteArticle(Guid id)
        {
            bool result = _articleRepository.Delete(id);

            if(result)
                await _articleRepository.Save();

            return result;
        }

        public async Task<IEnumerable<ArticleResponse>> GetAllArticles()
        {
            return _articleRepository.GetAll().Select(article => article.ToArticleResponse());
        }

        public async Task<ArticleResponse?> GetArticleById(Guid id)
        {
            Article? article = _articleRepository.GetValueById(id);

            if(article == null)
                return null;

            return article.ToArticleResponse();
        }

        public async Task<ArticleResponse?> GetArticleByTitle(string title)
        {
            Article? article = _articleRepository.GetAll().FirstOrDefault(x => x.Title == title);

            if(article == null)
                return null;

            return article.ToArticleResponse();
        }

        public async Task<ArticleResponse?> UpdateArticle(ArticleUpdateRequest articleUpdateRequest)
        {
            if (articleUpdateRequest == null)
                throw new ArgumentNullException(nameof(articleUpdateRequest));

            Article? articleSearched = _articleRepository.GetAll()
                .FirstOrDefault(article => article.Title == articleUpdateRequest.Title);

            if (articleSearched != null && articleSearched.Id != articleUpdateRequest.Id)
                throw new ArgumentException("Стаття з таким ім'ям вже існує.");

            if (articleUpdateRequest.Description == null)
                articleUpdateRequest.Description = string.Empty;

            Article articleToUpdate = articleUpdateRequest.ToArticle();
            Article? updatedArticle = _articleRepository.Update(articleToUpdate);

            if (updatedArticle == null)
                return null;

            await _articleRepository.Save();

            return updatedArticle.ToArticleResponse();
        }
    }
}
