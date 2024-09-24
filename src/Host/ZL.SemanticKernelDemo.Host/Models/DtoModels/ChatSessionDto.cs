using System.ComponentModel.DataAnnotations;

namespace ZL.SemanticKernelDemo.Host.Models.DtoModels
{
    public record CreateChatSessionDto
    {
        [Required, NotEmptyOrWhitespace]
        public string Title { get; init; }

        public string? Description { get; init; }
    }

    public record UpdateChatSessionDto
    {
        [Required, NotEmptyOrWhitespace]
        public string Title { get; init; }

        // public string? Description { get; init; }
    }
}
