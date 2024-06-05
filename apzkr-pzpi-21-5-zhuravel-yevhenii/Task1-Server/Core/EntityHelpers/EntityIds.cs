using System.Text;

namespace Core.EntityHelpers
{
    public class EntityIds : Dictionary<string, object>
    {
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var keyValuePair in this)
            {
                stringBuilder.Append($"{keyValuePair.Key} = {keyValuePair.Value};");
            }

            return stringBuilder.ToString();
        }
    }
}
