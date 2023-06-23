using InternetShop.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.DTO
{
    public class ArticleUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        public Article ToArticle()
        {
            return new Article { Id = this.Id, Title = this.Title, Description = this.Description };
        }
    }
}
