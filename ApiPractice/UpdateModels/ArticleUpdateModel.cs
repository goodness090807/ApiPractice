using System.ComponentModel.DataAnnotations;

namespace ApiPractice.UpdateModels
{
    public class ArticleUpdateModel
    {
        [MaxLength(50, ErrorMessage = "標題長度不能大於50個字!!!")]
        public string Title { get; set; }

        [MaxLength(1000, ErrorMessage = "內文長度不能大於1000個字!!!")]
        public string Content { get; set; }
    }
}