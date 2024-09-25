using System.ComponentModel.DataAnnotations;

namespace ZL.SemanticKernelDemo.Host.Models.DtoModels
{
    public class Ask
    {
        [Required, NotEmptyOrWhitespace]
        public string ChatSessionId { get; set; }

        [Required, NotEmptyOrWhitespace]
        public string Input { get; set; } = string.Empty;
    }
}
