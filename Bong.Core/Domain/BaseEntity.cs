using System;

namespace Bong.Core.Domain
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        public static bool IsTransient(BaseEntity entity)
        {
            return entity != null && Equals(entity.Id, default(int));
        }

        public virtual bool Equals(BaseEntity entity)
        {
            if (entity == null)
                return false;

            if (ReferenceEquals(this, entity))
                return true;

            if (!IsTransient(this) && !IsTransient(entity) && Equals(Id, entity.Id))
            {
                var thisType = this.GetType();
                var otherType = entity.GetType();
                return thisType.IsAssignableFrom(otherType) || otherType.IsAssignableFrom(thisType);
            }
            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(int)))   
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !Equals(x, y);
        }
    }
}
