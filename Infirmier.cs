namespace CabinetInfirmier;

using System.Text.RegularExpressions;
using System.Xml.Serialization;

[XmlRoot("infirmier", Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]

public class Infirmier
{
    String patternEntierPositif = @"^[0-9]+$";
    private string _id;
    [XmlAttribute("id")] public string Id {
        get
        {
            return _id;
        }
        set
        {
            if (Regex.IsMatch(value, patternEntierPositif))
                _id = value;
            else
                throw new Exception("Un id infimier doit être un entier positif.");
        } 
    }
    
    [XmlElement("nom")] public string Nom { get; set; }
    
    [XmlElement("prénom")] public string Prenom { get; set; }
    
    [XmlElement("photo")] public string Photo { get; set; }
    
    public Infirmier(){}

    public Infirmier(string id, string nom, string prenom, string photo)
    {
        this.Id = id;
        this.Nom = nom;
        this.Prenom = prenom;
        this.Photo = photo;
    }

    public string toString()
    {
        String res = "=> Infirmier : \nNom: " + Nom + "\nPrenom : " + Prenom + "\nPhoto : " + Photo + "\nId : " + Id;
        return res;
    }
}