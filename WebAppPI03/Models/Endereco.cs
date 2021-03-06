//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAppPI03.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Endereco
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Endereco()
        {
            this.Pedido = new HashSet<Pedido>();
        }

        [Key]
        public int idEndereco { get; set; }
        public int idCliente { get; set; }

        [Required]
        [Display(Name = "Nome para o endere�o")]
        [MaxLength(100, ErrorMessage = "Limite de 100 caracteres")]
        public string nomeEndereco { get; set; }
        [Required]
        [Display(Name = "Endereco")]
        [MaxLength(100, ErrorMessage = "Limite de 100 caracteres")]
        public string logradouroEndereco { get; set; }
        [Display(Name = "Numero")]
        public string numeroEndereco { get; set; }
        [Required]
        [Display(Name = "CEP")]
        [MaxLength(8, ErrorMessage = "Limite de 8 caracteres")]
        [MinLength(8, ErrorMessage = "No minimo 8 caracteres")]
        public string CEPEndereco { get; set; }
        [Display(Name = "Complemento")]
        [MaxLength(50, ErrorMessage = "Limite de 50 caracteres")]
        public string complementoEndereco { get; set; }
        [Required]
        [Display(Name = "Cidade")]
        [MaxLength(20, ErrorMessage = "Limite de 20 caracteres")]
        [MinLength(4, ErrorMessage = "No minimo 4 caracteres")]
        public string cidadeEndereco { get; set; }
        [Required]
        [Display(Name = "Pais")]
        [MaxLength(20, ErrorMessage = "Limite de 20 caracteres")]
        [MinLength(4, ErrorMessage = "No minimo 4 caracteres")]
        public string paisEndereco { get; set; }
        public string UFEndereco { get; set; }
        public virtual Cliente Cliente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pedido> Pedido { get; set; }
    }
}
