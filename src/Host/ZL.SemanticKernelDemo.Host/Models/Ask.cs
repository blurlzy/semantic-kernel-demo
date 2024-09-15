using System.ComponentModel.DataAnnotations;

namespace ZL.SemanticKernelDemo.Host.Models
{
    public class Ask
    {
        [Required, NotEmptyOrWhitespace]
        public string Input { get; set; } = string.Empty;

        // public IEnumerable<KeyValuePair<string, string>> Variables { get; set; } = Enumerable.Empty<KeyValuePair<string, string>>();
    }
}
