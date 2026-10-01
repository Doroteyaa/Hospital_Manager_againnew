using System.ComponentModel.DataAnnotations;

namespace Hospital_Manager.ViewModels.Patient
{
    public class PatientCreateViewModel
    {
        [Required(ErrorMessage = "Името е задължително.")]
        [MaxLength(30, ErrorMessage = "Името не може да бъде по-дълго от 30 символа.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Името е задължително.")]
        [MaxLength(30, ErrorMessage = "Името не може да бъде по-дълго от 30 символа.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Имейлът е задължителен.")]
        [EmailAddress(ErrorMessage = "Моля, въведете валиден имейл адрес.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Телефонният номер е задължителен.")]
        [Phone(ErrorMessage = "Моля, въведете валиден телефонен номер.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Моля, въведете лекар.")]
        public List<int> DoctorIds { get; set; }
    }
}
