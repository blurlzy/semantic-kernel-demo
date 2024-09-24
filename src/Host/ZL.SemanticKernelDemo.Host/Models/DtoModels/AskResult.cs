namespace ZL.SemanticKernelDemo.Host.Models.DtoModels
{
    public class AskResult
    {
        public string Value { get; set; } = string.Empty;

        public IEnumerable<KeyValuePair<string, object?>>? Variables { get; set; } = Enumerable.Empty<KeyValuePair<string, object?>>();
    }
}
