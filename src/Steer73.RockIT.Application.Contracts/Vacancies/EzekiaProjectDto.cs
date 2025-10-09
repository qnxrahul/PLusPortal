using EzekiaCRM;

namespace Steer73.RockIT.Vacancies
{
    public class EzekiaProjectDto
    {
        public int Id { get; set; }
        public string ProjectId { get; set; }
        public Relationships3 Relationships { get; set; }

        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties { get; set; }
    }
}
