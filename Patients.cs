namespace CabinetInfirmier;

using System.Xml.Serialization;

[XmlRoot("patients", Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class Patients
{
    [XmlElement("patient")] public List<Patient> _Patients { get; set; }
    
    public Patients(){}

    public Patients(List<Patient> patients)
    {
        _Patients = patients;
    }
    
    public void addPatient(Patient patient)
    {
        _Patients.Add(patient);
    }
    
    public string toString()
    {
        string res = "Liste des patients du cabinet : \n";
        for (int i = 0; i < _Patients.Count; i++)
        {
            res += _Patients[i].toString() + "\n";
        }
        return res;
    }
}