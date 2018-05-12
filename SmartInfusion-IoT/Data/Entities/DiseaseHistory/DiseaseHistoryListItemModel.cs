namespace SmartInfusion_IoT.Data.Entities.DiseaseHistory
{
    public class DiseaseHistoryListItemModel
    {
        public int Id { get; set; }

        public int PatientInfoId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string FullName
        {
            get => string.Concat(FirstName, " ", SecondName);
        }

    }
}
