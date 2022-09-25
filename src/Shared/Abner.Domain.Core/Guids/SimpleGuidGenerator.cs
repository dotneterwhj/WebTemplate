namespace Abner.Domain.Core
{
    public class SimpleGuidGenerator : IGuidGenerator
    {
        public Guid Create()
        {
            return Guid.NewGuid();
        }
    }
}
