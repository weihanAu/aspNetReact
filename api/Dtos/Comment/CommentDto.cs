namespace api.Dtos.Comment
{
    public class CommentDto
    {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string content { get; set; } = string.Empty;
    public DateTime createdAt { get; set; } = DateTime.Now;
    public int? stockId { get; set; }
    public string? AppUserName { get; set; }
  }
}