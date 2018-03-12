using System.ComponentModel.DataAnnotations;

namespace XcConnect.Models
{
    /// <summary>
    /// Import Client Model
    /// </summary>
    public class ImportarModel
    {
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = ".csv")]
        [Display(Name = "Cargar Archivo")]
        [Required(ErrorMessage = "Por favor seleccione el archivo para cargar.")]
        public string File { get; set; }
    }
}