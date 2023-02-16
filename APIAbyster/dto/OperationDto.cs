using APIAbyster.Models;
using System;
namespace APIAbyster.dto
{
    public class OperationDto
    {

        public decimal IdUser { get; set; }

        public decimal IdCategorie { get; set; }

        public string? LibelleOperation { get; set; }

        public string? MontantOperation { get; set; }

    }
}

