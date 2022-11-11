namespace Soundbox.Reloaded.Ui.Presentation.Dto
{
    using System;

    public abstract class EntityBaseDto
    {
        protected EntityBaseDto()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}
