namespace Data.Common.Entities
{
    public abstract class Entity
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public abstract object TransformToModel();
    }
}