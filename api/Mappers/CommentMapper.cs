using api.Dtos.Comment;
using api.models;

namespace api.Mappers;
public static class CommentMapper
{
  public static CommentDto ToCommentDto(this Comment comment)
  {
    return new CommentDto
    {
      Id = comment.Id,
      Title = comment.Title,
      content = comment.content,
      createdAt = comment.createdAt,
      stockId = comment.stockId
    };
  }

  public static Comment ToCommentModel(this CreateCommentDto commentDto,int stockId)
  {
    return new Comment
    {
      Title = commentDto.Title,
      content = commentDto.content,
      stockId = stockId
    };
  }

  public static Comment ToCommentModelFromUpdateDto(this UpdateCommentDto updateDto)
  {
    return new Comment
    {
      Title = updateDto.Title,
      content = updateDto.content,
    };
  }

}