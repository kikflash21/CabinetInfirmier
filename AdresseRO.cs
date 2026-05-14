namespace CabinetInfirmier;

using System.Text.RegularExpressions;
using System.Xml.Serialization;

[XmlRoot("adresse", Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class AdresseRO
{
    [XmlIgnore]
    private string patternEntierPositif = @"^[0-9]+$";
    private string patternCodePostale =  @"^\d{5}$";

    private int etage;
    [XmlElement("étage")]
    public int Etage
    {
        get { return etage; }
        init
        {
            if (Regex.IsMatch(value.ToString(), patternEntierPositif))
                etage = value;
            else
                throw new Exception("L'étage d'une adresse doit etre un entier positif.");
        }
    }

    private int numero;
    [XmlElement("numéro")]
    public int Numero
    {
        get { return numero; }
        init
        {
            if (Regex.IsMatch(value.ToString(), patternEntierPositif))
                numero = value;
            else
                throw new Exception("Le numero d'une adresse doit etre un entier positif.");
        }
    }

    [XmlElement("rue")]
    public string Rue { get; init; }

    private string codePostal;
    [XmlElement("codePostal")]
    public string CodePostal
    {
        get { return codePostal; }   
        init
        {
            if (Regex.IsMatch(value, patternCodePostale))
                codePostal = value;
            else
                throw new Exception("Le code postale d'une adresse doit etre un entier à 5 chiffres.");
        }
    }
    
    [XmlElement("ville")]
    public string Ville { get; init; }
    
    public AdresseRO() { }

    public AdresseRO(int etage, int numero, string rue, string codePostal, string ville)
    {
        Numero = numero;
        Rue = rue;
        CodePostal = codePostal;
        Ville = ville;
        Etage = etage;
    }
}
