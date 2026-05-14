namespace CabinetInfirmier;

using System.Text.RegularExpressions;
using System.Xml.Serialization;

[XmlRoot("patient", Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class Patient
{
    [XmlElement("nom")] public string Nom { get; set; }
    [XmlElement("prénom")] public string Prenom { get; set; }
    private string sexe;
    [XmlElement("sexe")] public string Sexe
    {
        get { return sexe; }
        set
        {
            string patternSex = @"[MF]";
            if (Regex.IsMatch(value.ToString(), patternSex))
                sexe = value;
            else
                throw new Exception("Le sexe est une valeur parmi {M, F}");
        }
    }
    [XmlElement("naissance")] public string Naissance { get; set; }
    

    private string numero;
    [XmlElement("numéro")]
    public string Numero {
        get { return numero; }
        set {
            string patternNSS = @"^[12]\d{2}(0[1-9]|1[0-2])\d{5}\d{3}\d[1-7]$"; // expliqué dans l'UML
            if (Regex.IsMatch(value, patternNSS))
                numero = value;
            else
                throw new Exception("Le numero de securité social "+value+" n'est pas valide !");
        }
    }
    [XmlElement("adresse")] public Adresse Adresse { get; set; }
    [XmlElement("visite")] public Visite Visite { get; set; }
    
    public Patient(){}

    public Patient(string nom, string prenom, string sexe, string naissance, string numero, Adresse adresse, Visite visite)
    {
        Nom = nom;
        Prenom = prenom;
        Sexe = sexe;
        Naissance = naissance;
        Numero = numero;
        Adresse = adresse;
        Visite = visite;
    }
    
    public string toString()
    {
        String res = "=> Patient : \nNom: " + Nom + "\nPrenom : " + Prenom + "\nSexe : " + Sexe + "\nDate de naissance : " + Naissance + "\nNuméro de sécurité social : " + Numero + "\nAdresse " + Adresse.toString() + "\nVisite " + Visite.toString();
        return res;
    }
}