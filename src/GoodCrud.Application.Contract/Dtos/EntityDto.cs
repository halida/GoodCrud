using System;

namespace GoodCrud.Application.Contract.Dtos
{
    public class EntityDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
