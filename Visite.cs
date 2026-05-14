namespace CabinetInfirmier;

using System.Xml.Serialization;
using System.Text.RegularExpressions;

[XmlRoot("visite", Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class Visite
{
    [XmlAttribute("intervenant")] public string Intervenant { get; set; }

    private string date;

    [XmlAttribute("date")]
    public string Date
    {
        get { return date; }
        set
        {
            string patternDate = @"^\d{4}-(0[1-9]|1[0-2])-([0-2][1-9]|3[01])$";
            if (Regex.IsMatch(value, patternDate))
                date = value;
            else
                throw new Exception("la date de visite fournie "+value+" est invalide");
        }
    }
    
    [XmlElement("acte")] public List<Acte> Acte{ get; set; }
    
    public Visite(){}

    public Visite(string intervenant, string date, List<Acte> acte)
    {
        Intervenant = intervenant;
        Date = date;
        Acte = acte;
    }

    public string toString()
    {
        string res = "Visite : \nIntervenant : " + Intervenant + "\nDate : " + Date + "\nActe : \n";
        foreach (Acte acte in Acte)
        {
            res += acte.toString() + "\n";
        }
        return res;
    }
}