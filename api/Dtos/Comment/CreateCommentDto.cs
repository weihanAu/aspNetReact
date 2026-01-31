using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment
{
  public class CreateCommentDto
  { [Required]
    [MinLength(3, ErrorMessage = "Content must be at least 3 characters long.")]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MinLength(3, ErrorMessage = "Content must be at least 3 characters long.")]
    [MaxLength(100)] public string content { get; set; } = string.Empty;
    public DateTime createdAt { get; set; } = DateTime.Now;
    [Required]
    [Range(1,9999,ErrorMessage ="StockId must be between 1 and 9999")]
     public int? stockId { get; set; }
  }
}