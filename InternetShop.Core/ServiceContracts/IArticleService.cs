using InternetShop.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.ServiceContracts
{
    /// <summary>
    /// Service for articles.
    /// </summary>
    public interface IArticleService
    {
        /// <summary>
        /// Method for adding new article to the data base.
        /// </summary>
        /// <param name="articleAddRequest">Article to add.</param>
        /// <returns>Article added to the data base.</returns>
        Task<ArticleResponse> AddArticle(ArticleAddRequest articleAddRequest);

        /// <summary>
        /// Method for reading all articles from the data base.
        /// </summary>
        /// <param name="includeHeaders">Flag that determines whether include header articles or not.</param>
        /// <returns>Collection IEnumerable of articles.</returns>
        Task<IEnumerable<ArticleResponse>> GetAllArticles(bool includeHeaders = true);

        /// <summary>
        /// Method for reading article by it`s id.
        /// </summary>
        /// <param name="id">Guid of article to read.</param>
        /// <returns>Article with passed id, null - if article was not found.</returns>
        Task<ArticleResponse?> GetArticleById(Guid id);

        /// <summary>
        /// Method for reading article by it`s title.
        /// </summary>
        /// <param name="title">Name of article to read.</param>
        /// <returns>Article with passed title, null - if article was not found.</returns>
        Task<ArticleResponse?> GetArticleByTitle(string title);

        /// <summary>
        /// Method for updating article in the data base.
        /// </summary>
        /// <param name="articleUpdateRequest">Article to update.</param>
        /// <returns>Update article, null - if article with passed id was not found.</returns>
        Task<ArticleResponse?> UpdateArticle(ArticleUpdateRequest articleUpdateRequest);

        /// <summary>
        /// Mehtod for deleting article from the data base.
        /// </summary>
        /// <param name="id">Guid of article to delete.</param>
        /// <returns>True - if deleting is successful, otherwise - false.</returns>
        Task<bool> DeleteArticle(Guid id);
    }
}
