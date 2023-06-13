using InternetShop.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.DTO
{
    /// <summary>
    /// DTO object of Category entity for response.
    /// </summary>
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<string> CharacteristicsList { get; set; } = new List<string>();

        public CategoryUpdateRequest ToCateogryUpdateRequest()
        {
            CategoryUpdateRequest categoryUpdateRequest = new CategoryUpdateRequest()
            {
                Id = this.Id,
                Name = this.Name,
                CharacteristicsList = this.CharacteristicsList
            };

            return categoryUpdateRequest;
        }

        public override bool Equals(object? obj)
        {
            if(obj == null) 
                return false;
            if(obj.GetType() != typeof(CategoryResponse)) 
                return false;

            var other = obj as CategoryResponse;
            
            if(other == null) 
                return false;

            for(int i = 0; i < this.CharacteristicsList.Count; i++)
            {
                if (this.CharacteristicsList[i] != other.CharacteristicsList[i]) return false;
            }

            return this.Id == other.Id && this.Name == other.Name;
        }
    }

    public static class CategoryExt
    {
        public static CategoryResponse ToCategoryResponse(this Category category)
        {
            CategoryResponse categoryResponse = new CategoryResponse()
            {
                Id = category.Id,
                Name = category.Name,
                CharacteristicsList = category.CharacteristicsList.Split(';').ToList()
            };

            return categoryResponse;
        }
    }
}
