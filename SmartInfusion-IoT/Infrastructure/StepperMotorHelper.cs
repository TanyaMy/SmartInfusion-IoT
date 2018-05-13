using SmartInfusion_IoT.Data.Entities.Treatment;
using System;
using System.Threading.Tasks;

namespace SmartInfusion_IoT.Infrastructure
{
    public class StepperMotorHelper
    {
        private const double INJECTOR_RADIUS = 0.5;
        private const double TURN_RADIUS = 1;
        private readonly Uln2003Driver _uln2003Driver;

        private double _infusionSpeed;
        private double _dosage;
        private double _solutionVolume;
        private double _patientWeight;

        private bool continueInfusion;

        public StepperMotorHelper()
        {
            _uln2003Driver = new Uln2003Driver(26, 13, 6, 5);
        }

        public StepperMotorHelper(TreatmentListItemModel treatment)
        {
            _uln2003Driver = new Uln2003Driver(26, 13, 6, 5);
            _infusionSpeed = treatment.InfusionSpeed;
            _dosage = treatment.Dosage;
            _solutionVolume = treatment.SolutionVolume;
            _patientWeight = treatment.PatientWeight;
        }

        public async Task ReturnToStart()
        {
            for (int i = 0; i < 3; i++)
            {
                await _uln2003Driver.TurnAsync(180, TurnDirection.Right);
            }
        }

        private double CalculateLinearSpeed()
        {
            var volumeSpeed = _infusionSpeed * _patientWeight * _solutionVolume / _dosage;
            var linearSpeed = volumeSpeed / (Math.PI * Math.Pow(INJECTOR_RADIUS, 2));

            return linearSpeed;
        }

        private double CalculateNumberOfDegreesPerMinute(double linearSpeed)
        {
            return linearSpeed / (2 * Math.PI * TURN_RADIUS);
        }

        public async Task StartInfusion()
        {
            var linearSpeed = CalculateLinearSpeed();
            var numberOfDegrees = CalculateNumberOfDegreesPerMinute(linearSpeed);

            var timeToWaitInMiliseconds = (int)((60 - numberOfDegrees * 0.1) / numberOfDegrees * 1000);

            continueInfusion = true;
            while (continueInfusion)
            {
                await _uln2003Driver.TurnAsync(1, TurnDirection.Left);
                await Task.Delay(timeToWaitInMiliseconds);
            }
        }

        public void StopInfusion()
        {
            continueInfusion = false;
            // _uln2003Driver.Dispose();
        }
    }
}
