using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPB.BusinessLogic.Managers.Exceptions;
using UPB.BusinessLogic.Models;

namespace UPB.BusinessLogic.Managers
{
    public class PatientManager
    {
        private List<Patient> _patients;
        private IConfiguration _configuration;

        public PatientManager(IConfiguration configuration)
        {
            _patients = new List<Patient>();
            _configuration = configuration;

            ReadPatientsToList();
        }

        public Patient CreatePatient(Patient patient)
        {
            if(CheckIfPatientExists(patient))
            {
                throw new PatientAlreadyExistsException();
            }

            Patient createdPatient = new Patient()
            {
                Name = patient.Name,
                LastName = patient.LastName,
                CI = patient.CI,
                BloodType = GetRandomBloodType()
            };

            _patients.Add(createdPatient);

            WriteListToFile();

            return createdPatient;
        }

        public Patient UpdatePatient(int ci, Patient UpdatedPatient)
        {
            Patient? patientToUpdate = _patients.Find(x => x.CI == ci);

            if(patientToUpdate == null)
            {
                throw new PatientNotFoundException("UpdatePatient");
            }

            patientToUpdate.Name = UpdatedPatient.Name;
            patientToUpdate.LastName = UpdatedPatient.LastName;
            // patientToUpdate.BloodType = UpdatedPatient.BloodType;

            WriteListToFile();

            return patientToUpdate;
        }

        public List<Patient> Delete(int ci)
        {
            Patient? patientToDelete = _patients.Find(x => x.CI == ci);

            if (patientToDelete == null)
            {
                throw new PatientNotFoundException("Delete");
            }

            _patients.Remove(patientToDelete);

            WriteListToFile();

            return _patients;
        }

        public List<Patient> GetPatients()
        {
            return _patients;
        }

        public Patient GetPatientsByCI(int ci)  
        {
            Patient? foundPatientByCI = _patients.Find(x => x.CI == ci);

            if (foundPatientByCI == null)
            {
                throw new PatientNotFoundException("GetPatientsByCI");
            }

            return foundPatientByCI;
        }

        private static string GetRandomBloodType()
        {
            Random rand = new Random();
            int random_value = rand.Next(0, 81);

            if(random_value < 10)
            {
                return "A+";
            }

            if(random_value < 20)
            {
                return "A-";
            }

            if (random_value < 30)
            {
                return "B+";
            }

            if (random_value < 40)
            {
                return "B-";
            }

            if (random_value < 50)
            {
                return "AB+";
            }

            if (random_value < 60)
            {
                return "AB-";
            }

            if (random_value < 70)
            {
                return "O+";
            }

            return "O-";
        }

        private void ReadPatientsToList()
        {
            string? patientsFile = _configuration.GetSection("FilePaths").GetSection("PatientFile").Value;

            if(patientsFile == null)
            {
                throw new JSONValueNotFoundException(["FilePaths", "PatientFile"]);
            }

            StreamReader reader = new StreamReader(patientsFile);

            Log.Information("Loading Patients from file.");

            _patients.Clear();

            while(!reader.EndOfStream)
            { 
                string line = reader.ReadLine();
                string[] patientInfo = line.Split(",");

                Patient newPatient = new Patient()
                {
                    Name = patientInfo[0],
                    LastName = patientInfo[1],
                    CI = int.Parse(patientInfo[2]),
                    BloodType = patientInfo[3]
                };
                _patients.Add(newPatient);
            }
            reader.Close();
        }

        private void WriteListToFile()
        {
            string? patientsFile = _configuration.GetSection("FilePaths").GetSection("PatientFile").Value;

            if (patientsFile == null)
            {
                throw new JSONValueNotFoundException(["FilePaths", "PatientFile"]);
            }

            StreamWriter writer = new StreamWriter(patientsFile);

            Log.Information("Saving patients to the file.");

            foreach(var patient in _patients)
            {
                string[] patientInfo = { patient.Name, patient.LastName, $"{patient.CI}", patient.BloodType };
                writer.WriteLine(string.Join(",", patientInfo));
            }
            writer.Close();
        }

        private bool CheckIfPatientExists(Patient patient)
        {
            Patient? foundPatient = _patients.Find(x => x.CI == patient.CI);

            if(foundPatient == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
