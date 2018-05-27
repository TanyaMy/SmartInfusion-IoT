using System;

namespace SmartInfusion_IoT.Data.Entities.Treatment
{
    public class TreatmentListItemModel
    {
        public int Id { get; set; }

        public int MedicineId { get; set; }

        public string MedicineTitle { get; set; }

        public string Diagnosis { get; set; }

        public double SolutionVolume { get; set; }

        public double Dosage { get; set; }

        public double InfusionSpeed { get; set; }

        public double PatientWeight { get; set; }

        public int DiseaseHistoryId { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime Created { get; set; }
        
    }
}
