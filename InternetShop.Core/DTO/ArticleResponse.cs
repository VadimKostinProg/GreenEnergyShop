using InternetShop.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.DTO
{
    public class ArticleResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public bool IsHeaderArticle { get; set; }

        public ArticleUpdateRequest ToArticleUpdateRequest()
        {
            return new ArticleUpdateRequest() { Id = this.Id, Title = this.Title, Description = this.Description, IsHeaderArticle = this.IsHeaderArticle };
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            var article = obj as ArticleResponse;

            if(article == null)
                return false;

            return this.Id == article.Id && this.Title == article.Title && this.Description == article.Description;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    public static class ArticleExt
    {
        public static ArticleResponse ToArticleResponse(this Article article)
        {
            return new ArticleResponse()
            {
                Id = article.Id,
                Title = article.Title,
                Description = article.Description,
                IsHeaderArticle = article.IsHeaderArticle
            };
        }
    }
}
