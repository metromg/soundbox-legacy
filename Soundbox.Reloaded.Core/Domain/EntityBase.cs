namespace Soundbox.Reloaded.Core.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public abstract class EntityBase
    {
        protected EntityBase()
        {
            this.Id = Guid.NewGuid();
        }

        [Required]
        public Guid Id { get; set; }
    }
}
