namespace api.Interface
{
  using api.Dtos.Comment;
  using api.models;
  public interface ICommentRepository
  {
    Task<List<Comment>> GetAllCommentsAsync();
    Task<Comment> AddCommentAsync(Comment comment);
    Task<Comment?> GetCommentByIdAsync(int id);
    Task<Comment?> UpdateCommentAsync(int id, Comment updatedComment);
    Task<Comment> DeleteCommentAsync(int id);
  } 
}