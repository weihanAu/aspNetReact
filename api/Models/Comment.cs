using System.ComponentModel.DataAnnotations.Schema;

namespace api.models
{
  [Table("Comments")]
    public class Comment
    {
      public int Id { get; set; }
      public string Title { get; set; } = string.Empty;
      public string content { get; set; } = string.Empty;
      public DateTime createdAt { get; set; } = DateTime.Now;
      public int? stockId { get; set; }
      // Navigation property
      public Stock? stock { get; set; }
    }
}