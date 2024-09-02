using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Entities;

public partial class CloudResource
{
    public int CloudResourceId { get; set; }
    public string? PublicId { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int? Bytes { get; set; }
    public DateTime CreateDate { get; set; } 
    public DateTime? ModifiedDate { get; set; } 
    
    [JsonIgnore]
    public virtual ICollection<TestSection> TestSections { get; set; } = new List<TestSection>();
    
    [JsonIgnore]
    public virtual ICollection<TestSectionPartition> TestSectionPartitions { get; set; } = new List<TestSectionPartition>();
}