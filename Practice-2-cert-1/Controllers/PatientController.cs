using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPB.BusinessLogic.Models;
using UPB.BusinessLogic.Managers;

namespace UPB.Practice_2_cert_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientManager _patientManager;

        public PatientController(PatientManager patientManager)
        {
            _patientManager = patientManager;
        }

        [HttpGet]
        public List<Patient> Get()
        {
            return _patientManager.GetPatients();
        }

        [HttpGet]
        [Route("{ci}")]
        public Patient Get(int ci)
        {
            return _patientManager.GetPatientsByCI(ci);
        }

        [HttpPost]
        public void Post([FromBody] Patient value)
        {
            _patientManager.CreatePatient(value);
        }

        [HttpPut("{ci}")]
        public void Put(int ci, [FromBody] Patient value)
        {
            _patientManager.UpdatePatient(ci, value);
        }

        [HttpDelete("{ci}")]
        public void Delete(int ci)
        {
            _patientManager.Delete(ci);
        }
    }
}
