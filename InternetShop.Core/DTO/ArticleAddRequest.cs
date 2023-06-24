using InternetShop.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.DTO
{
    public class ArticleAddRequest
    {
        [Required]
        [MaxLength(20)]
        public string Title { get; set; } = null!;

        public string Description { get; set; } = string.Empty;

        public Article ToArticle()
        {
            return new Article { Title = this.Title, Description = this.Description };
        }
    }
}
